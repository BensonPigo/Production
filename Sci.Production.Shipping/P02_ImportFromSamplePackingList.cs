using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Transactions;
using System.Data.SqlClient;
using System.Linq;
using Sci.Production.PublicPrg;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P02_ImportFromSapmlePackingList
    /// </summary>
    public partial class P02_ImportFromSamplePackingList : Win.Subs.Base
    {
        private DataRow masterData;
        private string chkPackingListID = string.Empty;

        /// <summary>
        /// P02_ImportFromSapmlePackingList
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P02_ImportFromSamplePackingList(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorTextColumnSettings ctnno = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings receiver = new DataGridViewGeneratorTextColumnSettings();

            // CTNNo要Trim掉空白字元
            ctnno.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);
                    dr["CTNNo"] = MyUtility.Convert.GetString(e.FormattedValue).Trim();
                }
            };
            receiver.CharacterCasing = CharacterCasing.Normal;
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Category", header: "Category", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CTNNo", header: "CTN No.", width: Widths.AnsiChars(5), settings: ctnno)
                .Numeric("NW", header: "N.W. (kg)", integer_places: 5, decimal_places: 3, maximum: 99999.99m, minimum: 0m)
                .Numeric("Price", header: "Price", integer_places: 6, decimal_places: 4, maximum: 999999.9999m, minimum: 0m)
                .Numeric("ShipQty", header: "Q'ty", decimal_places: 2, iseditingreadonly: true)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8))
                .Text("Receiver", header: "Receiver", width: Widths.AnsiChars(10), settings: receiver)
                .Text("Leader", header: "Team Leader", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true);

            this.gridImport.Columns["CTNNo"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["NW"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["Price"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["UnitID"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["Receiver"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
        }

        // Find Now
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSamplePL.Text))
            {
                this.txtSamplePL.Focus();
                MyUtility.Msg.WarningBox("Sapmle PL# can't empty!!");
                return;
            }

            // 檢查Sapmle PL#是否正確
            if (!this.CheckPLNo(this.txtSamplePL.Text))
            {
                return;
            }

            #region PackingListID 存在Pullout 則不能匯入
            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@PackingID", this.txtSamplePL.Text));
            DataRow dr;
            string sqlcmdChk = @"
select distinct p.ID as PulloutID
from PackingList_Detail pd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
left join factory WITH (NOLOCK)  on o.FactoryID=Factory.ID
inner join Pullout_Detail p on p.PackingListID = pd.ID
where pd.ID = @PackingID
and Factory.IsProduceFty=1
";
            if (MyUtility.Check.Seek(sqlcmdChk, listParameter, out dr))
            {
                MyUtility.Msg.WarningBox($@"Sapmle PL# already in pullout ID: {dr["PulloutID"]}");
                return;
            }
            #endregion

            string sqlCmd = string.Format(
                @"
select *
		, [NW] = ROUND( GW * ((ShipQty * 1.0) / (TtlShipQty *1.0)), 3, 1)  ----無條件捨去到小數點後第三位
        , Description = [dbo].[getBOFMtlDesc](StyleUkey)
from (
    select 
        pd.ID
        , pd.OrderID
        , o.SeasonID
        , o.StyleID
        , Category = 'SMS'
        , CTNNo = '' 
        , GW = p.GW
		, TtlShipQty = p.ShipQty
        , Price = 0.0
        , ShipQty = Sum (pd.ShipQty)
        , UnitID = o.StyleUnit
        , Receiver = '' 
        , LeaderID = o.SMR 
        , Leader = t.Name 
        , o.BrandID
		, o.StyleUkey
    from PackingList p WITH (NOLOCK) 
	inner join PackingList_Detail pd WITH (NOLOCK) on p.id = pd.ID
    left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
    left join TPEPass1 t WITH (NOLOCK) on o.SMR = t.ID
    left join factory WITH (NOLOCK)  on o.FactoryID=Factory.ID
    where pd.ID = '{0}'
          and Factory.IsProduceFty=1
          and p.Type = 'S'
    group by pd.ID, pd.OrderID, o.SeasonID, o.StyleID, p.ShipQty, p.GW, o.StyleUnit, o.SMR, t.Name, o.BrandID, o.StyleUkey
) getSamplePL
", this.txtSamplePL.Text);
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query error." + result.ToString());
                return;
            }

            if (selectData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!!");
            }

            this.chkPackingListID = this.txtSamplePL.Text;

            this.listControlBindingSource1.DataSource = selectData;
        }

        // 檢查Sapmle PL#是否正確
        private bool CheckPLNo(string pLNo)
        {
            // sql參數
            SqlParameter sp1 = new SqlParameter("@id", pLNo);

            IList<SqlParameter> cmds = new List<SqlParameter>();
            cmds.Add(sp1);

            DataTable packListData;
            string sqlCmd = "select ExpressID from PackingList WITH (NOLOCK) where ID = @id and Type = 'S'";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out packListData);
            if (result && packListData.Rows.Count > 0)
            {
                if (!MyUtility.Check.Empty(packListData.Rows[0]["ExpressID"]))
                {
                    MyUtility.Msg.WarningBox("The Sapmle PL# already be assign HC#, so can't assign again!!");
                    return false;
                }
            }
            else
            {
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                }
                else
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                return false;
            }

            return true;
        }

        // Update
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            this.listControlBindingSource1.EndEdit();

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data, can't update!");
                return;
            }

            // 檢查Sapmle PL#是否正確
            if (!this.CheckPLNo(MyUtility.Convert.GetString(dt.Rows[0]["ID"])))
            {
                return;
            }

            // 該單Approved / Junk都不允許調整資料
            if (!Prgs.CheckP02Status(this.masterData["ID"].ToString()))
            {
                return;
            }

            decimal ttlNW = Convert.ToDecimal(dt.Compute("SUM(NW)", string.Empty));
            decimal packingListGW = Convert.ToDecimal(MyUtility.GetValue.Lookup($"SELECT GW FROM  PackingList WHERE ID = '{this.chkPackingListID}'"));

            // 總 N.W. 是否超過 PL 總 G.W.
            if (ttlNW > packingListGW)
            {
                MyUtility.Msg.WarningBox("Total <N.W.> cannot be more than <G.W.> of the Packing List.");
                return;
            }

            IList<string> insertCmds = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                if (MyUtility.Check.Empty(dr["CTNNo"]))
                {
                    MyUtility.Msg.WarningBox("CTN No. can't empty!!");
                    return;
                }

                if (MyUtility.Check.Empty(dr["NW"]))
                {
                    MyUtility.Msg.WarningBox("N.W. (kg) can't empty!!");
                    return;
                }

                if (MyUtility.Check.Empty(dr["UnitID"]))
                {
                    MyUtility.Msg.WarningBox("Unit can't empty!!");
                    return;
                }

                if (MyUtility.Check.Empty(dr["Receiver"]))
                {
                    MyUtility.Msg.WarningBox("Receiver can't empty!!");
                    return;
                }

                if (!this.HcImportCheck(dr["ID"].ToString(), dr["OrderID"].ToString()))
                {
                    return;
                }

                insertCmds.Add(string.Format(
                    @"insert into Express_Detail(
ID ,OrderID 
,Seq1
,SeasonID,StyleID,Description,Qty,NW,CTNNo
,Category,PackingListID,Price,UnitID,Receiver,BrandID,Leader,InCharge,AddName,AddDate)

values(
'{0}','{1}'
,(select ISNULL(RIGHT(REPLICATE('0',3)+CAST(MAX(CAST(Seq1 as int))+1 as varchar),3),'001')from Express_Detail where ID = '{0}' and Seq2 = '') ----Seq1
,'{2}','{3}','{4}',{5},{6},'{7}'
,'2'  ----Category
,'{8}',{9},'{10}','{11}','{12}','{13}','{14}','{14}',GETDATE());",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    MyUtility.Convert.GetString(dr["OrderID"]),
                    MyUtility.Convert.GetString(dr["SeasonID"]),
                    MyUtility.Convert.GetString(dr["StyleID"]),
                    MyUtility.Convert.GetString(dr["Description"]),
                    MyUtility.Convert.GetString(dr["ShipQty"]),
                    MyUtility.Convert.GetString(dr["NW"]),
                    MyUtility.Convert.GetString(dr["CTNNo"]),
                    MyUtility.Convert.GetString(dr["ID"]),
                    MyUtility.Convert.GetString(dr["Price"]),
                    MyUtility.Convert.GetString(dr["UnitID"]),
                    MyUtility.Convert.GetString(dr["Receiver"]),
                    MyUtility.Convert.GetString(dr["BrandID"]),
                    MyUtility.Convert.GetString(dr["LeaderID"]),
                    Env.User.UserID));
            }

            insertCmds.Add($"update PackingList set ExpressID = '{this.masterData["ID"]}' where ID = '{dt.Rows[0]["ID"]}'");
            DualResult result1, result2;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    result1 = DBProxy.Current.Executes(null, insertCmds);
                    result2 = DBProxy.Current.Execute(null, Prgs.ReCalculateExpress(MyUtility.Convert.GetString(this.masterData["ID"])));
                    if (result1 && result2)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        transactionScope.Dispose();

                        string errorMsg = (!result1 ? result1.GetException().Message : string.Empty)
                            + Environment.NewLine + Environment.NewLine +
                            (!result2 ? result1.GetException().Message : string.Empty)
                            ;

                        MyUtility.Msg.WarningBox("Update failed, Pleaes re-try" + Environment.NewLine + errorMsg);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Update complete!!");
        }

        private bool HcImportCheck(string packingListID, string orderID)
        {
            DataTable dt;
            string sqlCmd = $@"
----※ 只要以下條件任一個符合就不允許匯入 ※


---- 1. 訂單 + PL 是否已經存在 HC表身
SELECT [ExistsData]=1
FROm  Express_Detail
WHERE PackingListID='{packingListID}'
      AND OrderID='{orderID}' 
UNION 
---- 2. PL 是否有建立在其他 HC
SELECT [ExistsData]=2
FROm PackingList
WHERE ID='{packingListID}' AND ExpressID<>'{this.masterData["ID"].ToString()}' AND ExpressID <> ''  AND ExpressID IS NOT NULL
";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                string msg = "Update failed, reason as below : " + Environment.NewLine;
                if (dt.AsEnumerable().Any(row => row["ExistsData"].EqualString("1")))
                {
                    msg += $"PackingList: {packingListID}、SP#: {orderID} has existed HC" + Environment.NewLine;
                }

                if (dt.AsEnumerable().Any(row => row["ExistsData"].EqualString("2")))
                {
                    msg += $"PackingList: {packingListID} exists other HC" + Environment.NewLine;
                }

                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            return true;
        }
    }
}

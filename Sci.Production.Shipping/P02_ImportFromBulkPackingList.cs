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
    /// P02_ImportFromBulkPackingList
    /// </summary>
    public partial class P02_ImportFromBulkPackingList : Win.Subs.Base
    {
        private DataRow masterData;
        private string chkPackingListID = string.Empty;

        /// <summary>
        /// P02_ImportFromBulkPackingList
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P02_ImportFromBulkPackingList(DataRow masterData)
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
                .Numeric("ShipQty", header: "Q'ty", decimal_places: 2)
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
            if (MyUtility.Check.Empty(this.txtBulkPL.Text))
            {
                this.txtBulkPL.Focus();
                MyUtility.Msg.WarningBox("Bulk PL# can't empty!!");
                return;
            }

            // 檢查PL#是否正確
            if (!this.CheckPLNo(this.txtBulkPL.Text))
            {
                return;
            }

            #region PackingListID 存在Pullout 則不能匯入
            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@PackingID", this.txtBulkPL.Text));
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
                MyUtility.Msg.WarningBox($@"Bulk PL# already in pullout ID: {dr["PulloutID"]}");
                return;
            }
            #endregion

            string sqlCmd =
                $@"
 select 
        pd.ID
        , pd.OrderID
        , o.SeasonID
        , o.StyleID
        , Category = 'Bulk'
        , CTNNo = '' 
		, [NW] = sum(pd.NWPerPcs * pd.ShipQty)
		, NW_Ps = sum(pd.NWPerPcs)
        , Price = 0.0
        , ShipQty = sum(pd.ShipQty)
        , UnitID = o.StyleUnit
        , Receiver = '' 
        , LeaderID = o.SMR 
        , Leader = t.Name 
        , o.BrandID
		, Description = [dbo].[getBOFMtlDesc](StyleUkey)
    from PackingList_Detail pd WITH (NOLOCK) 	
    inner join PackingList p with (nolock) on pd.id = p.id
    left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
    left join TPEPass1 t WITH (NOLOCK) on o.SMR = t.ID
    left join factory WITH (NOLOCK)  on o.FactoryID=Factory.ID
    where 1=1
	and pd.ID='{this.txtBulkPL.Text}'
    and Factory.IsProduceFty=1
    and p.Type = 'B'
	and not exists(
		select distinct p1.ID as PulloutID
		from PackingList_Detail p2 WITH (NOLOCK) 
		left join Orders o WITH (NOLOCK) on p2.OrderID = o.ID
		left join factory WITH (NOLOCK)  on o.FactoryID=Factory.ID
		inner join Pullout_Detail p1 on p1.PackingListID = p2.ID
		where p2.ID = pd.id
		and Factory.IsProduceFty=1
	)
    group by pd.ID, pd.OrderID, pd.CtnStartNo, o.SeasonID, o.StyleID, o.StyleUnit, o.SMR, t.Name, o.BrandID, o.StyleUkey
";
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
            else
            {
                // 檢查PL#,OrderID 是否可顯示於畫面上!
                foreach (DataRow drchk in selectData.Rows)
                {
                    if (!this.HcImportCheck(drchk["ID"].ToString(), drchk["OrderID"].ToString()))
                    {
                        return;
                    }
                }
            }

            this.chkPackingListID = this.txtBulkPL.Text;

            this.listControlBindingSource1.DataSource = selectData;
        }

        // 檢查PL#是否正確
        private bool CheckPLNo(string pLNo)
        {
            // sql參數
            SqlParameter sp1 = new SqlParameter("@id", pLNo);

            IList<SqlParameter> cmds = new List<SqlParameter>();
            cmds.Add(sp1);

            DataTable packListData;
            string sqlCmd = "select distinct ID, OrderID from PackingList_Detail WITH (NOLOCK) where ID = @id";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out packListData);
            if (result && packListData.Rows.Count > 0)
            {
                // 檢查PL#,OrderID 是否可顯示於畫面上!
                foreach (DataRow drchk in packListData.Rows)
                {
                    if (!this.HcImportCheck(drchk["ID"].ToString(), drchk["OrderID"].ToString()))
                    {
                        return false;
                    }
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

        /// <summary>
        /// 檢查是否存在HC
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        /// <param name="orderID">orderID</param>
        /// <returns>Bool</returns>
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

            // 檢查Bulk PL#是否正確
            if (!this.CheckPLNo(MyUtility.Convert.GetString(dt.Rows[0]["ID"])))
            {
                return;
            }

            // 該單Approved / Junk都不允許調整資料
            if (!Prgs.checkP02Status(this.masterData["ID"].ToString()))
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
                    MyUtility.Msg.WarningBox("CTN No. cannot be empty.");
                    return;
                }

                if (MyUtility.Check.Empty(dr["NW"]))
                {
                    MyUtility.Msg.WarningBox("N.W. (kg) cannot be empty.");
                    return;
                }

                if (MyUtility.Check.Empty(dr["UnitID"]))
                {
                    MyUtility.Msg.WarningBox("Unit cannot be empty.");
                    return;
                }

                if (MyUtility.Check.Empty(dr["Receiver"]))
                {
                    MyUtility.Msg.WarningBox("Receiver cannot be empty.");
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
,'3'  ----Category
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
                        MyUtility.Msg.WarningBox("Update failed, Pleaes re-try");
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
FROm  PackingList
WHERE ID='{packingListID}'
      and ExpressID = '{this.masterData["ID"].ToString()}'
UNION 
---- 2. PL 是否有建立在其他 HC
SELECT [ExistsData]=2
FROm PackingList
WHERE ID='{packingListID}' 
      AND ExpressID<>'{this.masterData["ID"].ToString()}' 
      AND ExpressID <> ''  
      AND ExpressID IS NOT NULL
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
                    msg += $"PackingList: {packingListID}, SP#: {orderID} has existed HC." + Environment.NewLine;
                }

                if (dt.AsEnumerable().Any(row => row["ExistsData"].EqualString("2")))
                {
                    msg += $"PackingList: {packingListID} exists other HC." + Environment.NewLine;
                }

                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            return true;
        }
    }
}

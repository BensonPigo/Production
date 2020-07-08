using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P02_AddByPOItem
    /// </summary>
    public partial class P02_AddByPOItem : Sci.Win.Subs.Input2A
    {
        /// <summary>
        /// P02_AddByPOItem
        /// </summary>
        public P02_AddByPOItem()
        {
            this.InitializeComponent();
        }

        /*Type Category對照
            PackingList Type    Express_Detail Category
            Bulk                Bulk
            Sample              SMS
            FOC                 Sample
        */

        /// <inheritdoc/>
        protected override void OnAttaching(DataRow data)
        {
            base.OnAttaching(data);
            if (this.OperationMode == 1 || this.OperationMode == 4)
            {
                MyUtility.Tool.SetupCombox(this.comboCategory, 2, 1, "1,Sample,2,SMS,3,Bulk");
            }

            if (this.OperationMode == 2 || this.OperationMode == 3)
            {
                MyUtility.Tool.SetupCombox(this.comboCategory, 2, 1, "2,SMS,3,Bulk");
            }

            if (MyUtility.Convert.GetString(data["Category"]) == "1")
            {
                this.labelAirPPNo.Text = "FOC PL#";
                if (this.OperationMode == 3)
                {
                    MyUtility.Tool.SetupCombox(this.comboCategory, 2, 1, "1,Sample");
                    this.txtPackingListID.ReadOnly = true;
                    this.txtPackingListID.IsSupportEditMode = false;
                }
            }

            if (this.OperationMode == 3)
            {
                this.txtSPNo.ReadOnly = true;
                this.txtSPNo.IsSupportEditMode = false;
            }
        }

        // CTN No.
        private void TxtCTNNo_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.txtCTNNo.OldValue != this.txtCTNNo.Text)
            {
                this.CurrentData["CTNNo"] = this.txtCTNNo.Text.Trim();
            }
        }

        // SP#
        private void TxtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtSPNo.Text) && this.txtSPNo.OldValue != this.txtSPNo.Text)
            {
                // sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", this.txtSPNo.Text);

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);

                string sqlCmd = "select Orders.ID from Orders WITH (NOLOCK) ,factory WITH (NOLOCK) where Orders.ID = @id and Orders.FactoryID = Factory.ID and Factory.IsProduceFty = 1";
                DataTable orderData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orderData);
                if (result && orderData.Rows.Count > 0)
                {
                    if (!MyUtility.Check.Seek(string.Format(
                        @"select oq.Id
from Order_QtyShip oq WITH (NOLOCK) 
inner join ShipMode s WITH (NOLOCK) on UseFunction like '%AirPP%' and oq.ShipmodeID = s.ID
where oq.Id = '{0}'", this.txtSPNo.Text)))
                    {
                        string shipMode = MyUtility.GetValue.Lookup(@"select (select ID+',' from ShipMode WITH (NOLOCK) where UseFunction like '%AirPP%'
for xml path('')) as ShipModeID");
                        if (shipMode != string.Empty)
                        {
                            shipMode = shipMode.Substring(0, shipMode.Length - 1);
                        }

                        this.CurrentData["OrderID"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("Ship mode must be '{0}'", shipMode));
                        return;
                    }
                }
                else
                {
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("SP# not found!!");
                    }

                    this.CurrentData["OrderID"] = string.Empty;
                    e.Cancel = true;
                    return;
                }
            }
        }

        // SP#
        private void TxtSPNo_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.txtSPNo.OldValue != this.txtSPNo.Text)
            {
                DataRow orderData;
                if (MyUtility.Check.Seek(
                    string.Format(
                    @"select SeasonID,StyleID,BrandID,SMR,[dbo].[getBOFMtlDesc](StyleUkey) as Description
from Orders WITH (NOLOCK) where ID = '{0}'", this.txtSPNo.Text), out orderData))
                {
                    this.CurrentData["OrderID"] = this.txtSPNo.Text;
                    this.CurrentData["SeasonID"] = orderData["SeasonID"];
                    this.CurrentData["StyleID"] = orderData["StyleID"];
                    this.CurrentData["BrandID"] = orderData["BrandID"];
                    this.CurrentData["Leader"] = orderData["SMR"];
                    if (MyUtility.Check.Empty(this.CurrentData["Description"]))
                    {
                        this.CurrentData["Description"] = orderData["Description"];
                    }
                }
                else
                {
                    this.CurrentData["OrderID"] = this.txtSPNo.Text;
                    this.CurrentData["SeasonID"] = string.Empty;
                    this.CurrentData["StyleID"] = string.Empty;
                    this.CurrentData["BrandID"] = string.Empty;
                    this.CurrentData["Leader"] = string.Empty;
                }
            }
        }

        private void TxtPackingListID_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtPackingListID.OldValue != this.txtPackingListID.Text)
            {
                string packingListTyp = this.ChkPackingListID();
                if (MyUtility.Check.Empty(packingListTyp))
                {
                    this.CurrentData["PackingListID"] = string.Empty;
                    e.Cancel = true;
                    return;
                }
                else
                {
                    string sqlCmd = string.Empty;
                    DataTable dt;
                    switch (packingListTyp)
                    {
                        case "B":
                            sqlCmd = $@"
select 
	   pd.OrderID
    , [CTNNo]=COUNT(pd.Ukey)
    , [TtlQty]=SUM( ISNULL(pd.ShipQty,0) ) 
    , [UnitID]=o.StyleUnit 
    , [Category]= 3 ----Bulk=3
	, [NW]=SUM(pd.GW)
from PackingList p WITH (NOLOCK) 
INNER JOIN PackingList_Detail pd WITH (NOLOCK) on p.id = pd.id
left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
where  pd.ID = '{this.txtPackingListID.Text}'
and p.Type ='B'
and pd.OrderID = '{this.txtSPNo.Text}'
GROUP BY pd.OrderID ,o.StyleUnit ";
                            break;
                        case "S":
                            sqlCmd = $@"

select 
	   pd.OrderID
    , [CTNNo] = ''
    , [TtlQty]=SUM( ISNULL(pd.ShipQty,0) ) 
    , [UnitID]=o.StyleUnit 
    , [Category]= 2 ----SMS=2
	, [NW] = p.GW * ( (SUM(pd.ShipQty) * 1.0) / (TtlShipQty.Value *1.0))
from PackingList p WITH (NOLOCK) 
INNER JOIN PackingList_Detail pd WITH (NOLOCK) on p.id = pd.id
left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
OUTER APPLY(
	SELECT Value=SUM(ShipQty) FROM PackingList_Detail WHERE ID = p.ID 
)TtlShipQty
where  pd.ID = '{this.txtPackingListID.Text}'
and p.Type ='S'
and pd.OrderID = '{this.txtSPNo.Text}'
GROUP BY pd.OrderID ,o.StyleUnit,pd.ShipQty ,TtlShipQty.Value ,p.GW
";
                            break;
                        default:
                            break;
                    }

                    DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);

                    if (result)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];

                            this.CurrentData["PackingListID"] = this.txtPackingListID.Text;
                            this.CurrentData["UnitID"] = dr["UnitID"].ToString();
                            this.CurrentData["Category"] = dr["Category"].ToString();
                            this.comboCategory.SelectedValue = Convert.ToInt32(dr["Category"]);

                            // ISP20191885 移除自動計算後帶入
                            // this.CurrentData["CTNNo"] = dr["CTNNo"].ToString();
                            // this.CurrentData["Qty"] = Convert.ToInt32(dr["TtlQty"]);

                            // this.CurrentData["NW"] = Convert.ToDecimal(dr["NW"]);
                        }
                    }
                }
            }
        }

        private string ChkPackingListID()
        {
            string plType = this.CurrentData["Category"].EqualDecimal(1) ? "'F'" : "'B','S'";
            string packingListType = string.Empty;
            string sqlchk = $@"
select DISTINCT pl.Type
from PackingList pl WITH (NOLOCK)
inner join PackingList_Detail pld WITH (NOLOCK) on pld.id = pl.id
where pl.ID = '{this.txtPackingListID.Text}'
and pl.Type in ({plType})
and pld.OrderID = '{this.txtSPNo.Text}'
";
            packingListType = MyUtility.GetValue.Lookup(sqlchk);

            if (MyUtility.Check.Empty(packingListType))
            {
                MyUtility.Msg.WarningBox("PackingList No. not found!!");
            }

            return packingListType;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentData["OrderID"]))
            {
                this.txtSPNo.Focus();
                MyUtility.Msg.WarningBox("SP# can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["Description"]))
            {
                this.editDescription.Focus();
                MyUtility.Msg.WarningBox("Description can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["CTNNo"]))
            {
                this.txtCTNNo.Focus();
                MyUtility.Msg.WarningBox("CTN No. can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["Qty"]))
            {
                this.numQty.Focus();
                MyUtility.Msg.WarningBox("Q'ty can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["UnitID"]))
            {
                this.txtunit_ftyUnit.Focus();
                MyUtility.Msg.WarningBox("Unit can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["NW"]))
            {
                this.numNW.Focus();
                MyUtility.Msg.WarningBox("N.W. (kg) can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["Category"]))
            {
                this.comboCategory.Focus();
                MyUtility.Msg.WarningBox("Category can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["Receiver"]))
            {
                this.txtReceiver.Focus();
                MyUtility.Msg.WarningBox("Receiver can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["PackingListID"]))
            {
                this.txtPackingListID.Focus();
                MyUtility.Msg.WarningBox("PackingListID No. can't empty!");
                return false;
            }

            // 該單Approved / Junk都不允許調整資料
            if (!Prgs.checkP02Status(this.CurrentData["ID"].ToString()))
            {
                return false;
            }
            #endregion

            string packingListTyp = this.ChkPackingListID();

            // 新增檢查
            if (this.OperationMode == 2)
            {
                if (MyUtility.Check.Empty(packingListTyp))
                {
                    return false;
                }

                if (MyUtility.Check.Seek($@"
select 1 
from packinglist 
where ID = '{this.CurrentData["PackingListID"].ToString()}' 
      and ExpressID<>'{this.CurrentData["ID"].ToString()}' 
      AND ExpressID <> ''  
      AND ExpressID IS NOT NULL"))
                {
                    MyUtility.Msg.WarningBox($"PackingList: {this.CurrentData["PackingListID"].ToString()} exists other HC");
                    return false;
                }

                DataRow seq;
                if (!MyUtility.Check.Seek(
                    string.Format(
                    @"select RIGHT(REPLICATE('0',3)+CAST(MAX(CAST(Seq1 as int))+1 as varchar),3) as Seq1
from Express_Detail WITH (NOLOCK) where ID = '{0}' and Seq2 = ''", MyUtility.Convert.GetString(this.CurrentData["ID"])), out seq))
                {
                    MyUtility.Msg.WarningBox("Get seq fail, pls try again");
                    return false;
                }

                this.CurrentData["Seq1"] = seq["Seq1"];
                this.CurrentData["InCharge"] = Sci.Env.User.UserID;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnSavePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(MyUtility.Convert.GetString(this.CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }

            string cmd = $"update PackingList set ExpressID = '{this.CurrentData["ID"]}' where ID = '{this.CurrentData["PackingListID"]}'";

            result = DBProxy.Current.Execute(null, cmd);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Update PackingList HC No. fail!! " + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        protected override bool OnDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentData["Category"]) == "1")
            {
                DialogResult DiaR = MyUtility.Msg.QuestionBox($@"All the items of PL# {this.CurrentData["PackingListID"]} will be deleted.");

                if (DiaR == DialogResult.No)
                {
                    return false;
                }
            }

            // 該單Approved / Junk都不允許調整資料
            if (!Prgs.checkP02Status(this.CurrentData["ID"].ToString()))
            {
                return false;
            }

            return base.OnDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnDeletePre()
        {
            DualResult failResult;
            if (MyUtility.Convert.GetString(this.CurrentData["Category"]) == "1")
            {
                DualResult result = DBProxy.Current.Execute(null, string.Format("update PackingList set ExpressID = '',pulloutdate=null where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentData["PackingListID"])));
                if (!result)
                {
                    failResult = new DualResult(false, "Update packing list fail!! Pls try again.\r\n" + result.ToString());
                    return failResult;
                }

                result = DBProxy.Current.Execute(null, string.Format("delete Express_Detail where ID = '{0}' and PackingListID = '{1}'", MyUtility.Convert.GetString(this.CurrentData["ID"]), MyUtility.Convert.GetString(this.CurrentData["PackingListID"])));
                if (!result)
                {
                    failResult = new DualResult(false, "Delete fail!! Pls try again.\r\n" + result.ToString());
                    return failResult;
                }
            }
            else
            {
                // 刪除最後一筆才清空PL的HC#
                DualResult result = DBProxy.Current.Execute(null, $@"
declare @count int = 0;

SELECT @count=Count(ID)
FROM Express_Detail
WHERE PackingListID='{this.CurrentData["PackingListID"]}'

update PackingList set ExpressID = ''
FROM PackingList
WHERE ID='{this.CurrentData["PackingListID"]}' AND @count <= 1
");
                if (!result)
                {
                    failResult = new DualResult(false, "Update packing list fail!! Pls try again.\r\n" + result.ToString());
                    return failResult;
                }
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override DualResult OnDeletePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(MyUtility.Convert.GetString(this.CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }
    }
}

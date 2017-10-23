using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;

namespace Sci.Production.Shipping
{
    public partial class P02_AddByPOItem : Sci.Win.Subs.Input2A
    {
        public P02_AddByPOItem()
        {
            InitializeComponent();
        }

        protected override void OnAttaching(DataRow data)
        {
            base.OnAttaching(data);
            if (OperationMode == 1 || OperationMode == 4)
            {
                MyUtility.Tool.SetupCombox(comboCategory, 2, 1, "1,Sample,2,SMS,3,Bulk");
            }
            if (OperationMode == 2 || OperationMode == 3)
            {
                MyUtility.Tool.SetupCombox(comboCategory, 2, 1, "2,SMS,3,Bulk");
            }
            
            if (MyUtility.Convert.GetString(data["Category"]) == "1")
            {
                labelAirPPNo.Text = "FOC PL#";
                if (OperationMode == 3)
                {
                    MyUtility.Tool.SetupCombox(comboCategory, 2, 1, "1,Sample");
                    txtAirPPNo.ReadOnly = true;
                    txtAirPPNo.IsSupportEditMode = false;
                }
            }

            if (OperationMode == 3)
            {
                txtSPNo.ReadOnly = true;
                txtSPNo.IsSupportEditMode = false;
            }
        }

        //CTN No.
        private void txtCTNNo_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtCTNNo.OldValue != txtCTNNo.Text)
            {
                CurrentData["CTNNo"] = txtCTNNo.Text.Trim();
            }
        }

        //SP#
        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(txtSPNo.Text) && txtSPNo.OldValue != txtSPNo.Text)
            {
                //sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", txtSPNo.Text);

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);

                string sqlCmd = "select Orders.ID from Orders WITH (NOLOCK) ,factory WITH (NOLOCK) where Orders.ID = @id and Orders.FactoryID = Factory.ID and Factory.IsProduceFty = 1";
                DataTable OrderData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out OrderData);
                if (result && OrderData.Rows.Count > 0)
                {
                    if (!MyUtility.Check.Seek(string.Format(@"select oq.Id
from Order_QtyShip oq WITH (NOLOCK) 
inner join ShipMode s WITH (NOLOCK) on UseFunction like '%AirPP%' and oq.ShipmodeID = s.ID
where oq.Id = '{0}'", txtSPNo.Text)))
                    {
                        string shipMode = MyUtility.GetValue.Lookup(@"select (select ID+',' from ShipMode WITH (NOLOCK) where UseFunction like '%AirPP%'
for xml path('')) as ShipModeID");
                        if (shipMode != "")
                        {
                            shipMode = shipMode.Substring(0, shipMode.Length - 1);
                        }
                        CurrentData["OrderID"] = "";
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
                    CurrentData["OrderID"] = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        //SP#
        private void txtSPNo_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtSPNo.OldValue != txtSPNo.Text)
            {
                DataRow OrderData;
                if (MyUtility.Check.Seek(string.Format(@"select SeasonID,StyleID,BrandID,SMR,[dbo].[getBOFMtlDesc](StyleUkey) as Description
from Orders WITH (NOLOCK) where ID = '{0}'", txtSPNo.Text), out OrderData))
                {
                    CurrentData["OrderID"] = txtSPNo.Text;
                    CurrentData["SeasonID"] = OrderData["SeasonID"];
                    CurrentData["StyleID"] = OrderData["StyleID"];
                    CurrentData["BrandID"] = OrderData["BrandID"];
                    CurrentData["Leader"] = OrderData["SMR"];
                    if (MyUtility.Check.Empty(CurrentData["Description"]))
                    {
                        CurrentData["Description"] = OrderData["Description"];
                    }
                }
                else
                {
                    CurrentData["OrderID"] = txtSPNo.Text;
                    CurrentData["SeasonID"] = "";
                    CurrentData["StyleID"] = "";
                    CurrentData["BrandID"] = "";
                    CurrentData["Leader"] = "";
                }
            }
        }

        //Air PP No.
        private void txtAirPPNo_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && txtAirPPNo.OldValue != txtAirPPNo.Text)
            {
                if (!ChkAirPP())
                {
                    CurrentData["DutyNo"] = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private bool ChkAirPP()
        {
            DataRow AirPPData;
            if (!MyUtility.Check.Seek(string.Format("select OrderID from AirPP WITH (NOLOCK) where ID = '{0}'  and Status <> 'Junked'", txtAirPPNo.Text), out AirPPData))
            {
                MyUtility.Msg.WarningBox("Air PP No. not found!!");
                return false;
            }
            else
            {
                if (AirPPData["OrderID"].ToString() != txtSPNo.Text)
                {
                    MyUtility.Msg.WarningBox("SP# and Air PP's SP# is inconsistent!!");
                    return false;
                }
            }

            if (MyUtility.Check.Seek(string.Format(@"select ed.ID 
from Express_Detail ed WITH (NOLOCK) 
inner join Express e WITH (NOLOCK) on ed.ID = e.ID and e.Status <> 'Junked'
where DutyNo = '{0}' and ed.ID <> '{1}'", txtAirPPNo.Text, MyUtility.Convert.GetString(CurrentData["ID"])), out AirPPData))
            {
                MyUtility.Msg.WarningBox(string.Format("This Air PP No. already in HC#{0}, so can't be assign!!", MyUtility.Convert.GetString(AirPPData["ID"])));
                return false;
            }
            return true;
        }

        protected override bool OnSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentData["OrderID"]))
            {
                txtSPNo.Focus();
                MyUtility.Msg.WarningBox("SP# can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Description"]))
            {
                editDescription.Focus();
                MyUtility.Msg.WarningBox("Description can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["CTNNo"]))
            {
                txtCTNNo.Focus();
                MyUtility.Msg.WarningBox("CTN No. can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Qty"]))
            {
                numQty.Focus();
                MyUtility.Msg.WarningBox("Q'ty can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["UnitID"]))
            {
                txtunit_ftyUnit.Focus();
                MyUtility.Msg.WarningBox("Unit can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["NW"]))
            {
                numNW.Focus();
                MyUtility.Msg.WarningBox("N.W. (kg) can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Category"]))
            {
                comboCategory.Focus();
                MyUtility.Msg.WarningBox("Category can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Receiver"]))
            {
                txtReceiver.Focus();
                MyUtility.Msg.WarningBox("Receiver can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["DutyNo"]))
            {
                txtAirPPNo.Focus();
                MyUtility.Msg.WarningBox("Air PP No. can't empty!");
                return false;
            }
            #endregion

            //新增檢查
            if (OperationMode == 2)
            {
                if (!ChkAirPP())
                {
                    return false;
                }
                DataRow Seq;
                if (!MyUtility.Check.Seek(string.Format(@"select RIGHT(REPLICATE('0',3)+CAST(MAX(CAST(Seq1 as int))+1 as varchar),3) as Seq1
from Express_Detail WITH (NOLOCK) where ID = '{0}' and Seq2 = ''", MyUtility.Convert.GetString(CurrentData["ID"])), out Seq))
                {
                    MyUtility.Msg.WarningBox("Get seq fail, pls try again");
                    return false;
                }
                CurrentData["Seq1"] = Seq["Seq1"];
                CurrentData["InCharge"] = Sci.Env.User.UserID;
            }

            return true;
        }

        protected override DualResult OnSavePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(MyUtility.Convert.GetString(CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }
//            DualResult result1 = DBProxy.Current.Execute(null,"select * from ")
            return Result.True;
        }

        protected override DualResult OnDeletePre()
        {
            if (MyUtility.Convert.GetString(CurrentData["Category"]) == "1")
            {
                DualResult failResult;
                
                DualResult result = DBProxy.Current.Execute(null, string.Format("update PackingList set ExpressID = '' where ID = '{0}'", MyUtility.Convert.GetString(CurrentData["DutyNo"])));
                if (!result)
                {
                    failResult = new DualResult(false, "Update packing list fail!! Pls try again.\r\n" + result.ToString());
                    return failResult;
                }

                result = DBProxy.Current.Execute(null, string.Format("delete Express_Detail where ID = '{0}' and DutyNo = '{1}'", MyUtility.Convert.GetString(CurrentData["ID"]), MyUtility.Convert.GetString(CurrentData["DutyNo"])));
                if (!result)
                {
                    failResult = new DualResult(false, "Delete fail!! Pls try again.\r\n" + result.ToString());
                    return failResult;
                }
            }
            return Result.True;
        }

        protected override DualResult OnDeletePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(MyUtility.Convert.GetString(CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }
    }
}

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
                MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "1,Sample,2,SMS,3,Bulk");
            }
            if (OperationMode == 2 || OperationMode == 3)
            {
                MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "2,SMS,3,Bulk");
            }
            
            if (MyUtility.Convert.GetString(data["Category"]) == "1")
            {
                label12.Text = "FOC PL#";
                if (OperationMode == 3)
                {
                    MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "1,Sample");
                    textBox4.ReadOnly = true;
                    textBox4.IsSupportEditMode = false;
                }
            }

            if (OperationMode == 3)
            {
                textBox1.ReadOnly = true;
                textBox1.IsSupportEditMode = false;
            }
        }

        //CTN No.
        private void textBox2_Validated(object sender, EventArgs e)
        {
            if (EditMode && textBox2.OldValue != textBox2.Text)
            {
                CurrentData["CTNNo"] = textBox2.Text.Trim();
            }
        }

        //SP#
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(textBox1.Text) && textBox1.OldValue != textBox1.Text)
            {
                //sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", textBox1.Text);

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);

                string sqlCmd = "select ID from Orders where ID = @id";
                DataTable OrderData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out OrderData);
                if (result && OrderData.Rows.Count > 0)
                {
                    if (!MyUtility.Check.Seek(string.Format(@"select oq.Id
from Order_QtyShip oq
inner join ShipMode s on UseFunction like '%AirPP%' and oq.ShipmodeID = s.ID
where oq.Id = '{0}'", textBox1.Text)))
                    {
                        string shipMode = MyUtility.GetValue.Lookup(@"select (select ID+',' from ShipMode where UseFunction like '%AirPP%'
for xml path('')) as ShipModeID");
                        if (shipMode != "")
                        {
                            shipMode = shipMode.Substring(0, shipMode.Length - 1);
                        }
                        MyUtility.Msg.WarningBox(string.Format("Ship mode must be '{0}'",shipMode));
                        CurrentData["OrderID"] = "";
                        e.Cancel = true;
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
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (EditMode && textBox1.OldValue != textBox1.Text)
            {
                DataRow OrderData;
                if (MyUtility.Check.Seek(string.Format(@"select SeasonID,StyleID,BrandID,SMR,[dbo].[getBOFMtlDesc](StyleUkey) as Description
from Orders where ID = '{0}'",textBox1.Text), out OrderData))
                {
                    CurrentData["OrderID"] = textBox1.Text;
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
                    CurrentData["OrderID"] = textBox1.Text;
                    CurrentData["SeasonID"] = "";
                    CurrentData["StyleID"] = "";
                    CurrentData["BrandID"] = "";
                    CurrentData["Leader"] = "";
                }
            }
        }

        //Air PP No.
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && textBox4.OldValue != textBox4.Text)
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
            if (!MyUtility.Check.Seek(string.Format("select OrderID from AirPP where ID = '{0}'  and Status <> 'Junked'", textBox4.Text), out AirPPData))
            {
                MyUtility.Msg.WarningBox("Air PP No. not found!!");
                return false;
            }
            else
            {
                if (AirPPData["OrderID"].ToString() != textBox1.Text)
                {
                    MyUtility.Msg.WarningBox("SP# and Air PP's SP# is inconsistent!!");
                    return false;
                }
            }

            if (MyUtility.Check.Seek(string.Format(@"select ed.ID 
from Express_Detail ed
inner join Express e on ed.ID = e.ID and e.Status <> 'Junked'
where DutyNo = '{0}' and ed.ID <> '{1}'", textBox4.Text, MyUtility.Convert.GetString(CurrentData["ID"])), out AirPPData))
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
                MyUtility.Msg.WarningBox("SP# can't empty!");
                textBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Description"]))
            {
                MyUtility.Msg.WarningBox("Description can't empty!");
                editBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["CTNNo"]))
            {
                MyUtility.Msg.WarningBox("CTN No. can't empty!");
                textBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Qty"]))
            {
                MyUtility.Msg.WarningBox("Q'ty can't empty!");
                numericBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["UnitID"]))
            {
                MyUtility.Msg.WarningBox("Unit can't empty!");
                txtunit_fty1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["NW"]))
            {
                MyUtility.Msg.WarningBox("N.W. (kg) can't empty!");
                numericBox3.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Category"]))
            {
                MyUtility.Msg.WarningBox("Category can't empty!");
                comboBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Receiver"]))
            {
                MyUtility.Msg.WarningBox("Receiver can't empty!");
                textBox3.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["DutyNo"]))
            {
                MyUtility.Msg.WarningBox("Air PP No. can't empty!");
                textBox4.Focus();
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
from Express_Detail where ID = '{0}' and Seq2 = ''", MyUtility.Convert.GetString(CurrentData["ID"])), out Seq))
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

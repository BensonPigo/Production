using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System.Transactions;

namespace Sci.Production.Shipping
{
    public partial class P01 : Sci.Win.Tems.Input1
    {
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            label34.Text = "MR's \r\nComments";
            label35.Text = "Cause &\r\nResponsible\r\nDetails";
            txtuser1.TextBox1.ReadOnly = true;
            txtuser1.TextBox1.IsSupportEditMode = false;
            txtcountry1.TextBox1.ReadOnly = true;
            txtcountry1.TextBox1.IsSupportEditMode = false;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            button5.Enabled = CurrentMaintain["Status"].ToString() == "Approved";
            if (MyUtility.GetValue.Lookup(string.Format(@"select a.ShipQty-b.Qty as BalQty from 
(select isnull(sum(ShipQty),0) as ShipQty from AirPP where Status <> 'Junked' and OrderID = '{0}') a,
(select isnull(sum(Qty),0) as Qty from Order_QtyShip where Id = '{0}' and ShipmodeID in (select ID from ShipMode where UseFunction like '%AirPP%')) b
", CurrentMaintain["OrderID"].ToString())) != "0")
            {
                this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
                this.button2.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
                this.button2.ForeColor = System.Drawing.Color.Black;
            }
            DataTable orderData;
            DualResult result = DBProxy.Current.Select(null, string.Format(@"select o.FactoryID,o.BrandID,o.StyleID,o.Dest,isnull(oq.ShipmodeID,'') as ShipmodeID,isnull(oq.Qty,0) as Qty,oq.BuyerDelivery,isnull(s.Description,'') as Description
from Orders o
left join Order_QtyShip oq on oq.Id = o.ID and oq.Seq = '{1}'
left join Style s on s.Ukey = o.StyleUkey
where o.Id = '{0}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString()), out orderData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query order fail.\r\n" + result.ToString());
            }
            else
            {
                if (orderData.Rows.Count == 0)
                {
                    displayBox2.Value = "";
                    displayBox3.Value = "";
                    displayBox4.Value = "";
                    displayBox5.Value = "";
                    displayBox6.Value = "";
                    numericBox1.Value = 0;
                    dateBox2.Value = null;
                    txtcountry1.TextBox1.Text = "";
                }
                else
                {
                    displayBox2.Value = orderData.Rows[0]["StyleID"].ToString();
                    displayBox3.Value = orderData.Rows[0]["BrandID"].ToString();
                    displayBox4.Value = orderData.Rows[0]["Description"].ToString();
                    displayBox5.Value = orderData.Rows[0]["FactoryID"].ToString();
                    displayBox6.Value = orderData.Rows[0]["ShipmodeID"].ToString();
                    numericBox1.Value = Convert.ToInt32(orderData.Rows[0]["Qty"]);
                    if (MyUtility.Check.Empty(orderData.Rows[0]["BuyerDelivery"]))
                    { dateBox2.Value = null; }
                    else
                    { dateBox2.Value = Convert.ToDateTime(orderData.Rows[0]["BuyerDelivery"]); }
                    txtcountry1.TextBox1.Text = orderData.Rows[0]["Dest"].ToString();
                }
            }
            displayBox8.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Air_Prepaid_Reason' and ID = '{0}'", CurrentMaintain["ReasonID"].ToString()));
            displayBox19.Value = MyUtility.Check.Empty(CurrentMaintain["TPEEditDate"]) ? null : Convert.ToDateTime(CurrentMaintain["TPEEditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            displayBox20.Value = MyUtility.Check.Empty(CurrentMaintain["FtySendDate"]) ? null : Convert.ToDateTime(CurrentMaintain["FtySendDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            //狀態顯示
            switch (CurrentMaintain["Status"].ToString())
            {
                case "New":
                    label30.Text = "New";
                    break;
                case "Junked":
                    label30.Text = "Junk";
                    break;
                case "Checked":
                    label30.Text = "PPIC Apv";
                    break;
                case "Approved":
                    label30.Text = "Fty Apv";
                    break;
                case "Confirmed":
                    label30.Text = "SMR Apv";
                    break;
                case "Locked":
                    label30.Text = "Task Team Lock";
                    break;
                default:
                    label30.Text = "";
                    break;
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["CDate"] = DateTime.Today;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["FtyMgr"] = MyUtility.GetValue.Lookup("Manager", Sci.Env.User.Factory, "Factory", "ID");
            CurrentMaintain["RatioFty"] = 0;
            CurrentMaintain["RatioSubcon"] = 0;
            CurrentMaintain["RatioSCI"] = 0;
            CurrentMaintain["RatioSupp"] = 0;
            CurrentMaintain["RatioBuyer"] = 0;

            ControlFactoryRatio(true);
            ControlSubconRatio(true);
            ControlSCIRatio(true);
            ControlSupplierRatio(true);
            ControlBuyerRatio(true);

        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "New")
            {
                if (!Prgs.GetAuthority(CurrentMaintain["AddName"].ToString()))
                {
                    MyUtility.Msg.WarningBox("You have no permission!");
                    return false;
                }
            }
            else
            {
                if (CurrentMaintain["Status"].ToString() == "Junked")
                {
                    MyUtility.Msg.WarningBox("This.record is cancel. Can't be modify!");
                    return false;
                }
                else
                {
                    MyUtility.Msg.WarningBox("This record is < Approved >. Can't be modify!");
                    return false;
                }
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            ControlFactoryRatio(!Convert.ToBoolean(CurrentMaintain["ResponsibleFty"]));
            ControlSubconRatio(!Convert.ToBoolean(CurrentMaintain["ResponsibleSubcon"]));
            ControlSCIRatio(!Convert.ToBoolean(CurrentMaintain["ResponsibleSCI"]));
            ControlSupplierRatio(!Convert.ToBoolean(CurrentMaintain["ResponsibleSupp"]));
            ControlBuyerRatio(!Convert.ToBoolean(CurrentMaintain["ResponsibleBuyer"]));
        }

        protected override bool ClickSaveBefore()
        {
            //檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("SP No. can't empty!!");
                textBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["OrderShipmodeSeq"]))
            {
                MyUtility.Msg.WarningBox("Seq can't empty!!");
                textBox2.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["PPICMgr"]))
            {
                MyUtility.Msg.WarningBox("PPIC mgr can't empty!!");
                txtuser2.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["FtyMgr"]))
            {
                MyUtility.Msg.WarningBox("Factory mgr can't empty!!");
                txtuser3.TextBox1.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleFty"]) && MyUtility.Check.Empty(CurrentMaintain["RatioFty"]))
            {
                MyUtility.Msg.WarningBox("Factory Ratio% can't empty!!");
                numericBox11.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSubcon"]) && MyUtility.Check.Empty(CurrentMaintain["RatioSubcon"]))
            {
                MyUtility.Msg.WarningBox("Subcon Ratio% can't empty!!");
                numericBox12.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSCI"]) && MyUtility.Check.Empty(CurrentMaintain["RatioSCI"]))
            {
                MyUtility.Msg.WarningBox("SCI Ratio% can't empty!!");
                numericBox13.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSupp"]) && MyUtility.Check.Empty(CurrentMaintain["RatioSupp"]))
            {
                MyUtility.Msg.WarningBox("Supplier Ratio% can't empty!!");
                numericBox14.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleBuyer"]) && MyUtility.Check.Empty(CurrentMaintain["RatioBuyer"]))
            {
                MyUtility.Msg.WarningBox("Buyer Ratio% can't empty!!");
                numericBox15.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleFty"]) && MyUtility.Check.Empty(CurrentMaintain["ResponsibleFtyNo"]))
            {
                MyUtility.Msg.WarningBox("Factory can't empty!!");
                txtfactory1.Focus();
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSubcon"]) && MyUtility.Check.Empty(CurrentMaintain["SubConName"]))
            {
                MyUtility.Msg.WarningBox("Subcon Name can't empty!!");
                textBox6.Focus();
                return false;
            }

            //Ratio加總要等於100
            if (Convert.ToDecimal(CurrentMaintain["RatioFty"]) + Convert.ToDecimal(CurrentMaintain["RatioSubcon"]) + Convert.ToDecimal(CurrentMaintain["RatioSCI"]) + Convert.ToDecimal(CurrentMaintain["RatioSupp"]) + Convert.ToDecimal(CurrentMaintain["RatioBuyer"]) != 100)
            {
                MyUtility.Msg.WarningBox("Total ratio% of cause & responsible detail is not 100%.");
                return false;
            }

            //Air Qty要等於Order Qty
            if (Convert.ToInt32(CurrentMaintain["ShipQty"]) != Convert.ToInt32(numericBox1.Value))
            {
                MyUtility.Msg.WarningBox("Air Q'ty not equal to Order Q'ty!!");
                return false;
            }

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "AP", "AirPP", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        protected override bool ClickSavePost()
        {
            //新增資料時，要寫一筆紀錄到AirPP_History
            if (IsDetailInserting)
            {
                string insertCmd = string.Format(@"insert into AirPP_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','Status','','New','{1}',GETDATE())", CurrentMaintain["ID"].ToString(), Sci.Env.User.UserID);

                DualResult result = DBProxy.Current.Execute(null, insertCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Insert AirPP_History fail!\r\n" + result.ToString());
                    return false;
                }
            }
            return base.ClickSavePost();
        }

        private void textBox8_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name from Reason where ReasonTypeID = 'Air_Prepaid_Reason' and Junk = 0 order by ID", "5,50", this.Text, false, ",");

                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }

                IList<DataRow> reasonData = item.GetSelecteds();
                CurrentMaintain["ReasonID"] = item.GetSelectedString();
                displayBox8.Value = reasonData[0]["Name"].ToString();
            }
        }

        //Calculate Est. Amount
        private void CalculateEstAmt()
        {
            decimal gw = MyUtility.Check.Empty(CurrentMaintain["GW"]) ? 0 : Convert.ToDecimal(CurrentMaintain["GW"]);
            decimal vw = MyUtility.Check.Empty(CurrentMaintain["VW"]) ? 0 : Convert.ToDecimal(CurrentMaintain["VW"]);
            decimal qt = MyUtility.Check.Empty(CurrentMaintain["Quotation"]) ? 0 : Convert.ToDecimal(CurrentMaintain["Quotation"]);
            CurrentMaintain["EstAmount"] = MyUtility.Math.Round((gw > vw ? gw : vw) * qt, 4);
        }

        //Gross Weight(Kgs)
        private void numericBox3_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && numericBox3.OldValue != numericBox3.Value)
            {
                CalculateEstAmt();
            }
        }

        //V.Weight(Kgs)
        private void numericBox4_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && numericBox4.OldValue != numericBox4.Value)
            {
                CalculateEstAmt();
            }
        }

        //Quotation(USD/Kgs)
        private void numericBox6_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && numericBox6.OldValue != numericBox6.Value)
            {
                CalculateEstAmt();
            }
        }

        //Factory
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                //CurrentMaintain["ResponsibleFty"] = Convert.ToBoolean(checkBox1.Value);
                //ControlRatio("ResponsibleFty");
                //ControlFactoryRatio(!Convert.ToBoolean(checkBox1.Value));
            }
        }

        //Subcon
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                //CurrentMaintain["ResponsibleSubcon"] = Convert.ToBoolean(checkBox2.Value);
                //ControlRatio("ResponsibleSubcon");
                //ControlSubconRatio(!Convert.ToBoolean(checkBox2.Value));
            }
        }

        //SCI
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                //CurrentMaintain["ResponsibleSCI"] = Convert.ToBoolean(checkBox3.Value);
                //ControlRatio("ResponsibleSCI");
                //ControlSCIRatio(!Convert.ToBoolean(checkBox3.Value));
            }
        }

        //Supplier
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                //CurrentMaintain["ResponsibleSupp"] = Convert.ToBoolean(checkBox4.Value);
                //ControlRatio("ResponsibleSupp");
                //ControlSupplierRatio(!Convert.ToBoolean(checkBox4.Value));
            }
        }

        //Buyer
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                //CurrentMaintain["ResponsibleBuyer"] = Convert.ToBoolean(checkBox5.Value);
                //ControlRatio("ResponsibleBuyer");
                //ControlBuyerRatio(!Convert.ToBoolean(checkBox5.Value));
            }
        }

        private void ControlRatio(string type)
        {
            #region Factory
            if (type == "All" || type == "ResponsibleFty")
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleFty"]) && CurrentMaintain["ResponsibleFty"].ToString() == "True")
                {
                    numericBox11.ReadOnly = false;
                    txtfactory1.ReadOnly = false;
                }
                else
                {
                    numericBox11.Value = 0;
                    txtfactory1.Text = "";
                    numericBox11.ReadOnly = true;
                    txtfactory1.ReadOnly = true;
                }
            }
            #endregion

            #region Subcon
            if (type == "All" || type == "ResponsibleSubcon")
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSubcon"]) && CurrentMaintain["ResponsibleSubcon"].ToString() == "True")
                {
                    numericBox12.ReadOnly = false;
                    textBox5.ReadOnly = false;
                    textBox6.ReadOnly = false;
                }
                else
                {
                    numericBox12.Value = 0;
                    textBox5.Text = "";
                    textBox6.Text = "";
                    numericBox12.ReadOnly = true;
                    textBox5.ReadOnly = true;
                    textBox6.ReadOnly = true;
                }
            }
            #endregion

            #region SCI
            if (type == "All" || type == "ResponsibleSCI")
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSCI"]) && CurrentMaintain["ResponsibleSCI"].ToString() == "True")
                {
                    numericBox13.ReadOnly = false;
                }
                else
                {
                    numericBox13.Value = 0;
                    numericBox13.ReadOnly = true;
                }
            }
            #endregion

            #region Supplier
            if (type == "All" || type == "ResponsibleSupp")
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSupp"]) && CurrentMaintain["ResponsibleSupp"].ToString() == "True")
                {
                    numericBox14.ReadOnly = false;
                }
                else
                {
                    numericBox14.Value = 0;
                    numericBox14.ReadOnly = true;
                }
            }
            #endregion

            #region Buyer
            if (type == "All" || type == "ResponsibleBuyer")
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleBuyer"]) && CurrentMaintain["ResponsibleBuyer"].ToString() == "True")
                {
                    numericBox15.ReadOnly = false;
                    textBox7.ReadOnly = false;
                }
                else
                {
                    numericBox15.Value = 0;
                    textBox7.Text = "";
                    numericBox15.ReadOnly = true;
                    textBox7.ReadOnly = true;
                }
            }
            #endregion
        }

        private void ControlFactoryRatio(bool ReadOnly)
        {
            if (ReadOnly)
            {
                CurrentMaintain["RatioFty"] = 0;
                CurrentMaintain["ResponsibleFtyNo"] = "";
            }
            numericBox11.ReadOnly = ReadOnly;
            txtfactory1.ReadOnly = ReadOnly;
        }

        private void ControlSubconRatio(bool ReadOnly)
        {
            if (ReadOnly)
            {
                CurrentMaintain["RatioSubcon"] = 0;
                CurrentMaintain["SubconDBCNo"] = "";
                CurrentMaintain["SubConName"] = "";
            }
            numericBox12.ReadOnly = ReadOnly;
            textBox5.ReadOnly = ReadOnly;
            textBox6.ReadOnly = ReadOnly;
        }

        private void ControlSCIRatio(bool ReadOnly)
        {
            if (ReadOnly)
            {
                CurrentMaintain["RatioSCI"] = 0;
            }
            numericBox13.ReadOnly = ReadOnly;
        }

        private void ControlSupplierRatio(bool ReadOnly)
        {
            if (ReadOnly)
            {
                CurrentMaintain["RatioSupp"] = 0;
            }
            numericBox14.ReadOnly = ReadOnly;
        }

        private void ControlBuyerRatio(bool ReadOnly)
        {
            if (ReadOnly)
            {
                CurrentMaintain["RatioBuyer"] = 0;
                CurrentMaintain["BuyerRemark"] = "";
            }
            numericBox15.ReadOnly = ReadOnly;
            textBox7.ReadOnly = ReadOnly;
        }

        //Factory
        private void checkBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.EditMode)
            {
                ControlFactoryRatio(!Convert.ToBoolean(checkBox1.Value));
            }
        }

        //Subcon
        private void checkBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.EditMode)
            {
                ControlSubconRatio(!Convert.ToBoolean(checkBox2.Value));
            }
        }

        //SCI
        private void checkBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.EditMode)
            {
                ControlSCIRatio(!Convert.ToBoolean(checkBox3.Value));
            }
        }

        //Supplier
        private void checkBox4_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.EditMode)
            {
                ControlSupplierRatio(!Convert.ToBoolean(checkBox4.Value));
            }
        }

        //Buyer
        private void checkBox5_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.EditMode)
            {
                ControlBuyerRatio(!Convert.ToBoolean(checkBox5.Value));
            }
        }

        //檢查輸入的SP#是否正確
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (textBox1.Text != textBox1.OldValue)
                {
                    #region 檢查輸入的值是否符合條件
                    if (!MyUtility.Check.Empty(textBox1.Text))
                    {
                        DataRow orderData;
                        string sqlCmd = string.Format("select ID from Orders where ID = '{0}' and FtyGroup = '{1}'", textBox1.Text, Sci.Env.User.Factory);
                        if (!MyUtility.Check.Seek(sqlCmd, out orderData))
                        {
                            MyUtility.Msg.WarningBox("< SP No. > not found!");
                            //OrderID異動，其他相關欄位要跟著異動
                            ChangeOtherData("");
                            textBox1.Text = "";
                            e.Cancel = true;
                        }
                    }
                    #endregion
                }
            }
        }

        //OrderID異動，其他相關欄位要跟著異動
        private void ChangeOtherData(string orderID)
        {
            if (MyUtility.Check.Empty(orderID))
            {
                //OrderID為空值時，要把其他相關欄位值清空
                displayBox2.Value = "";
                displayBox3.Value = "";
                displayBox4.Value = "";
                displayBox5.Value = "";
                displayBox6.Value = "";
                numericBox1.Value = 0;
                dateBox2.Value = null;
                txtcountry1.TextBox1.Text = "";
                CurrentMaintain["OrderShipmodeSeq"] = "";
                CurrentMaintain["POHandle"] = "";
                CurrentMaintain["POSMR"] = "";
                CurrentMaintain["MRHandle"] = "";
                CurrentMaintain["SMR"] = "";
                CurrentMaintain["ShipQty"] = 0;
            }
            else
            {
                DataRow orderData;
                string sqlCmd;
                sqlCmd = string.Format(@"select o.FactoryID,o.BrandID,o.StyleID,o.Dest,isnull(s.Description,'') as Description,p.POHandle,p.POSMR,o.MRHandle,o.SMR
from Orders o
left join Style s on s.Ukey = o.StyleUkey
left join PO p on p.ID = o.POID
where o.Id = '{0}'", orderID);
                if (MyUtility.Check.Seek(sqlCmd, out orderData))
                {
                    //帶出相關欄位的資料
                    displayBox2.Value = orderData["StyleID"].ToString();
                    displayBox3.Value = orderData["BrandID"].ToString();
                    displayBox4.Value = orderData["Description"].ToString();
                    displayBox5.Value = orderData["FactoryID"].ToString();
                    txtcountry1.TextBox1.Text = orderData["Dest"].ToString();
                    CurrentMaintain["POHandle"] = orderData["POHandle"].ToString();
                    CurrentMaintain["POSMR"] = orderData["POSMR"].ToString();
                    CurrentMaintain["MRHandle"] = orderData["MRHandle"].ToString();
                    CurrentMaintain["SMR"] = orderData["SMR"].ToString();

                    #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                    sqlCmd = string.Format(@"select oq.Seq,oq.BuyerDelivery,oq.ShipmodeID,oq.Qty from Order_QtyShip oq,(
select Id,Seq from Order_QtyShip where Id = '{0}' and 
ShipmodeID in (select ID from ShipMode where UseFunction like '%AirPP%')
except
select OrderID as ID,OrderShipmodeSeq as Seq from AirPP where OrderID = '{0}' and ID != '{1}' and Status <> 'Junked') b
where oq.Id = b.Id and oq.Seq = b.Seq", orderID, MyUtility.Check.Empty(CurrentMaintain["ID"]) ? "" : CurrentMaintain["ID"].ToString());
                    DataTable orderQtyData;
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, out orderQtyData);

                    if (result)
                    {
                        if (orderQtyData.Rows.Count == 1)
                        {
                            CurrentMaintain["OrderShipmodeSeq"] = orderQtyData.Rows[0]["Seq"].ToString();
                            displayBox6.Value = orderQtyData.Rows[0]["ShipmodeID"].ToString();
                            numericBox1.Value = Convert.ToInt32(orderQtyData.Rows[0]["Qty"]);
                            CurrentMaintain["ShipQty"] = Convert.ToInt32(orderQtyData.Rows[0]["Qty"]);
                            if (MyUtility.Check.Empty(orderQtyData.Rows[0]["BuyerDelivery"]))
                            { dateBox2.Value = null; }
                            else
                            { dateBox2.Value = Convert.ToDateTime(orderQtyData.Rows[0]["BuyerDelivery"]); }
                        }
                        else
                        {
                            IList<DataRow> orderQtyShipData;
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(orderQtyData, "Seq,BuyerDelivery,ShipModeID,Qty", "4,20,20,10", "", false, "", "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                CurrentMaintain["OrderShipmodeSeq"] = "";
                                displayBox6.Value = "";
                                numericBox1.Value = 0;
                                CurrentMaintain["ShipQty"] = 0;
                                dateBox1.Value = null;
                            }
                            else
                            {
                                orderQtyShipData = item.GetSelecteds();
                                CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                                displayBox6.Value = orderQtyShipData[0]["ShipmodeID"].ToString();
                                numericBox1.Value = Convert.ToInt32(orderQtyShipData[0]["Qty"]);
                                CurrentMaintain["ShipQty"] = Convert.ToInt32(orderQtyShipData[0]["Qty"]);
                                if (MyUtility.Check.Empty(orderQtyShipData[0]["BuyerDelivery"]))
                                { dateBox2.Value = null; }
                                else
                                { dateBox2.Value = Convert.ToDateTime(orderQtyShipData[0]["BuyerDelivery"]); }
                            }
                        }
                    }
                    else
                    {
                        CurrentMaintain["OrderShipmodeSeq"] = "";
                        displayBox6.Value = "";
                        numericBox1.Value = 0;
                        CurrentMaintain["ShipQty"] = 0;
                        dateBox1.Value = null;
                        MyUtility.Msg.ErrorBox("Query Seq fail.\r\n" + result.ToString());
                    }
                    #endregion
                }
            }
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (textBox1.OldValue == textBox1.Text)
            {
                return;
            }

            //OrderID異動，其他相關欄位要跟著異動
            ChangeOtherData(textBox1.Text);
        }

        //Seq按右鍵
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format(@"select oq.Seq,oq.BuyerDelivery,oq.ShipmodeID,oq.Qty from Order_QtyShip oq,(
select Id,Seq from Order_QtyShip where Id = '{0}' and 
ShipmodeID in (select ID from ShipMode where UseFunction like '%AirPP%')
except
select OrderID as ID,OrderShipmodeSeq as Seq from AirPP where OrderID = '{0}' and ID != '{1}' and Status <> 'Junked') b
where oq.Id = b.Id and oq.Seq = b.Seq", MyUtility.Check.Empty(CurrentMaintain["OrderID"]) ? "" : CurrentMaintain["OrderID"], MyUtility.Check.Empty(CurrentMaintain["ID"]) ? "" : CurrentMaintain["ID"].ToString());
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", "", "Seq,Buyer Delivery,ShipMode,Qty");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                CurrentMaintain["OrderShipmodeSeq"] = "";
                CurrentMaintain["ShipModeID"] = "";
                numericBox1.Value = 0;
                CurrentMaintain["ShipQty"] = 0;
                dateBox1.Value = null;
            }
            else
            {
                IList<DataRow> orderQtyShipData;
                orderQtyShipData = item.GetSelecteds();
                CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                displayBox6.Value = orderQtyShipData[0]["ShipmodeID"].ToString();
                numericBox1.Value = Convert.ToInt32(orderQtyShipData[0]["Qty"]);
                CurrentMaintain["ShipQty"] = Convert.ToInt32(orderQtyShipData[0]["Qty"]);
                if (MyUtility.Check.Empty(orderQtyShipData[0]["BuyerDelivery"]))
                { dateBox2.Value = null; }
                else
                { dateBox2.Value = Convert.ToDateTime(orderQtyShipData[0]["BuyerDelivery"]); }
            }
        }

        //Junk
        protected override void ClickJunk()
        {
            if (!Prgs.GetAuthority(CurrentMaintain["AddName"].ToString()))
            {
                MyUtility.Msg.WarningBox("Sorry, you don't have permission to junk this data. ");
                return;
            }
            //問是否要做Junk，確定才繼續往下做
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Junk > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Air_Prepaid_unApprove");
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                string insertCmd = string.Format(@"insert into AirPP_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','Status','New','Junked','{1}','{2}','{3}',GetDate())", CurrentMaintain["ID"].ToString(), callReason.ReturnReason, callReason.ReturnRemark, Sci.Env.User.UserID);
                string updateCmd = string.Format(@"update AirPP set Status = 'Junked', EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        DualResult result = DBProxy.Current.Execute(null, insertCmd);
                        DualResult result2 = DBProxy.Current.Execute(null, updateCmd);

                        if (result && result2)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("Junk failed, Pleaes re-try");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
            SendMail(true);
        }

        //PPIC mgr Approve
        protected override void ClickCheck()
        {
            if (!Prgs.GetAuthority(CurrentMaintain["PPICMgr"].ToString()))
            {
                MyUtility.Msg.WarningBox("Sorry, you don't have permission to check this data. ");
                return;
            }

            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["ShipQty"]))
            {
                MyUtility.Msg.WarningBox("Air Q'ty can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ETA"]))
            {
                MyUtility.Msg.WarningBox("ETA can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["GW"]))
            {
                MyUtility.Msg.WarningBox("Gross Weight(Kgs) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["VW"]))
            {
                MyUtility.Msg.WarningBox("V.Weight(Kgs) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Rate"]))
            {
                MyUtility.Msg.WarningBox("Exchange Rate can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Forwarder"]))
            {
                MyUtility.Msg.WarningBox("Forwarder(N) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Forwarder1"]))
            {
                MyUtility.Msg.WarningBox("Forwarder(1) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Forwarder2"]))
            {
                MyUtility.Msg.WarningBox("Forwarder(2) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Quotation"]) || MyUtility.Check.Empty(CurrentMaintain["Quotation1"]) || MyUtility.Check.Empty(CurrentMaintain["Quotation1"]))
            {
                MyUtility.Msg.WarningBox("Quotation(USD/Kgs) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["EstAmount"]))
            {
                MyUtility.Msg.WarningBox("Est. Amt(USD) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ReasonID"]))
            {
                MyUtility.Msg.WarningBox("Responsibility Justifcation can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["FtyDesc"]))
            {
                MyUtility.Msg.WarningBox("Explanation can't empty!!");
                return;
            }
            #endregion

            string insertCmd = string.Format(@"insert into AirPP_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','Status','New','Checked','{1}',GetDate())", CurrentMaintain["ID"].ToString(), Sci.Env.User.UserID);
            string updateCmd = string.Format(@"update AirPP set Status = 'Checked', PPICMgrApvDate = GetDate(), EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    DualResult result = DBProxy.Current.Execute(null, insertCmd);
                    DualResult result2 = DBProxy.Current.Execute(null, updateCmd);

                    if (result && result2)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("UnCheck failed, Pleaes re-try");
                    }
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //PPIC mgr UnApprove
        protected override void ClickUncheck()
        {
            if (!Prgs.GetAuthority(CurrentMaintain["PPICMgr"].ToString()))
            {
                MyUtility.Msg.WarningBox("Sorry, you don't have permission to uncheck this data. ");
                return;
            }
            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Air_Prepaid_unApprove");
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                string insertCmd = string.Format(@"insert into AirPP_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','Status','Checked','New','{1}','{2}','{3}',GetDate())", CurrentMaintain["ID"].ToString(), callReason.ReturnReason, callReason.ReturnRemark, Sci.Env.User.UserID);
                string updateCmd = string.Format(@"update AirPP set Status = 'New', PPICMgrApvDate = null, EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        DualResult result = DBProxy.Current.Execute(null, insertCmd);
                        DualResult result2 = DBProxy.Current.Execute(null, updateCmd);

                        if (result && result2)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("UnCheck failed, Pleaes re-try");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Factory mgr Approve
        protected override void ClickConfirm()
        {
            if (!Prgs.GetAuthority(CurrentMaintain["FtyMgr"].ToString()))
            {
                MyUtility.Msg.WarningBox("Sorry, you don't have permission to check this data. ");
                return;
            }

            string insertCmd = string.Format(@"insert into AirPP_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','Status','Checked','Approved','{1}',GetDate())", CurrentMaintain["ID"].ToString(), Sci.Env.User.UserID);
            string updateCmd = string.Format(@"update AirPP set Status = 'Approved', FtyMgrApvDate = GetDate(), EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    DualResult result = DBProxy.Current.Execute(null, insertCmd);
                    DualResult result2 = DBProxy.Current.Execute(null, updateCmd);

                    if (result && result2)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try");
                    }
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
            SendMail(true);
        }

        //Status update history
        private void button6_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("AirPP_History", CurrentMaintain["ID"].ToString(), "Status", reasonType: "Air_Prepaid_unApprove", caption: "Status Update History");
            callNextForm.ShowDialog(this);
        }

        //Mail To
        private void button5_Click(object sender, EventArgs e)
        {
            SendMail(true);
        }

        //寄Mail
        private void SendMail(bool visibleForm)
        {
            DataRow dr;
            if (MyUtility.Check.Seek("select * from MailTo where ID = '008'", out dr))
            {
                DataTable allMail;
                string sqlCmd = string.Format(@"select isnull((select EMail from Pass1 where ID = a.PPICMgr),'') as PPICMgrMail,
isnull((select Name from Pass1 where ID = a.PPICMgr),'') as PPICMgrName,
isnull((select ExtNo from Pass1 where ID = a.PPICMgr),'') as PPICMgrExtNo,
isnull((select EMail from Pass1 where ID = a.FtyMgr),'') as FtyMgrMail,
isnull((select Name from Pass1 where ID = a.FtyMgr),'') as FtyMgrName,
isnull((select ExtNo from Pass1 where ID = a.FtyMgr),'') as FtyMgrExtNo,
isnull((select EMail from TPEPass1 where ID = o.MRHandle),'') as MRHandleMail,
isnull((select Name from TPEPass1 where ID = o.MRHandle),'') as MRHandleName,
isnull((select ExtNo from TPEPass1 where ID = o.MRHandle),'') as MRHandleExtNo,
isnull((select EMail from TPEPass1 where ID = o.SMR),'') as SMRMail,
isnull((select Name from TPEPass1 where ID = o.SMR),'') as SMRName,
isnull((select ExtNo from TPEPass1 where ID = o.SMR),'') as SMRExtNo,
isnull((select EMail from TPEPass1 where ID = p.POHandle),'') as POHandleMail,
isnull((select Name from TPEPass1 where ID = p.POHandle),'') as POHandleName,
isnull((select ExtNo from TPEPass1 where ID = p.POHandle),'') as POHandleExtNo,
isnull((select EMail from TPEPass1 where ID = p.POSMR),'') as POSMRMail,
isnull((select Name from TPEPass1 where ID = p.POSMR),'') as POSMRName,
isnull((select ExtNo from TPEPass1 where ID = p.POSMR),'') as POSMRExtNo
from AirPP a
left join Orders o on o.ID = a.OrderID
left join PO p on p.ID = o.POID
where a.ID = '{0}'",CurrentMaintain["ID"].ToString());
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out allMail);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Query mail list fail.\r\n" + result.ToString());
                    return;
                }

                string mailto = allMail.Rows[0]["POSMRMail"].ToString() + ";" + allMail.Rows[0]["POHandleMail"].ToString() + ";" + allMail.Rows[0]["MRHandleMail"].ToString() + ";" + allMail.Rows[0]["SMRMail"].ToString() + ";";
                string cc = allMail.Rows[0]["PPICMgrMail"].ToString() + ";" + allMail.Rows[0]["FtyMgrMail"].ToString() + ";" + dr["ToAddress"].ToString();
                string subject = string.Format(@"<{0}> {1} for SP#{2}, DD{3} - {4}",
                    (string)displayBox5.Value, CurrentMaintain["ID"].ToString(), CurrentMaintain["OrderID"].ToString(), Convert.ToDateTime(CurrentMaintain["CDate"]).ToString("yyyyMMdd"), CurrentMaintain["Status"].ToString() == "Junked" ? "cancel" : "request");
                StringBuilder content = new StringBuilder();
                #region 組Content
                content.Append(string.Format(@"Hi MR team, cc.Production team/Task Team
Please refer to attachment – air pp request and refer to below datas.
SMR: {0} Ext.{1}, POSMR: {2} Ext.{3}
{4} - {5} for SP - {6} buyer del: {7} 
Air q’ty: {8}
Responsibility: {9}{10}{11}{12}{13}
-Be remind!! ---
If the responsibility is belong to the supplier or SCI-MR team (posmr team), please key in Debit Note and ICR#, tks!
If the responsibility is belong to “Buyer”, please remark the reason, tks!
Remind:Please return the air pp request – approved  within 24hrs to avoid any shipment delay.", allMail.Rows[0]["SMRName"].ToString(), allMail.Rows[0]["SMRExtNo"].ToString(),
                                                                                              allMail.Rows[0]["POSMRName"].ToString(), allMail.Rows[0]["POSMRExtNo"].ToString(), displayBox5.Value, CurrentMaintain["ID"].ToString(),
                                                                                              CurrentMaintain["OrderID"].ToString(), Convert.ToDateTime(dateBox2.Value).ToString("yyyy.MM.dd"), CurrentMaintain["ShipQty"].ToString(),
                                                                                              CurrentMaintain["ResponsibleFty"].ToString() == "True" ? "Factory:" + CurrentMaintain["ResponsibleFtyNo"].ToString() + "\r\n" : "",
                                                                                              CurrentMaintain["ResponsibleSubcon"].ToString() == "True" ? "Subcon:DBC #:" + CurrentMaintain["SubconDBCNo"].ToString() + "\r\n" : "",
                                                                                              CurrentMaintain["ResponsibleSCI"].ToString() == "True" ? "SCI ICR #:" + CurrentMaintain["SCIICRNo"].ToString() + "\r\n" : "",
                                                                                              CurrentMaintain["ResponsibleSupp"].ToString() == "True" ? "Supplier:DBC #:" + CurrentMaintain["SuppDBCNo"].ToString() + "\r\n" : "",
                                                                                              CurrentMaintain["ResponsibleBuyer"].ToString() == "True" ? "Buyer:Debit Memo:" + CurrentMaintain["BuyerDBCNo"].ToString() + ", ICR #" + CurrentMaintain["BuyerICRNo"].ToString() + "\r\n" : ""));
                #endregion

                var email = new MailTo(Sci.Env.User.MailAddress, mailto, cc, subject, "", content.ToString(), visibleForm, visibleForm);
                email.ShowDialog(this);
            }
        }

        //AirPP List
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P01_AirPPList callNextForm = new Sci.Production.Shipping.P01_AirPPList(CurrentMaintain["OrderID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Q'ty B'down by Shipmode
        private void button1_Click(object sender, EventArgs e)
        {
            MyUtility.Msg.InfoBox("Wait for PPIC");
        }

        //Q'ty B'down by Order
        private void button3_Click(object sender, EventArgs e)
        {
            MyUtility.Msg.InfoBox("Wait for PPIC");
        }

        //GMT Export
        private void button4_Click(object sender, EventArgs e)
        {
            MyUtility.Msg.InfoBox("Wait for PPIC");
        }
    }
}

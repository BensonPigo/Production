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
using System.Linq;
using System.Transactions;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    public partial class P01 : Sci.Win.Tems.Input1
    {
        string excelFile;
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            labelComments.Text = "MR's \r\nComments";
            labelResponsible.Text = "Cause &\r\nResponsible\r\nDetails";
            txtUserPreparedBy.TextBox1.ReadOnly = true;
            txtUserPreparedBy.TextBox1.IsSupportEditMode = false;
            txtCountryDestination.TextBox1.ReadOnly = true;
            txtCountryDestination.TextBox1.IsSupportEditMode = false;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            btnMailto.Enabled = MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Approved";

            if (MyUtility.GetValue.Lookup(string.Format(@"select a.ShipQty-b.Qty as BalQty from 
(select isnull(sum(ShipQty),0) as ShipQty from AirPP WITH (NOLOCK) where Status <> 'Junked' and OrderID = '{0}') a,
(select isnull(sum(Qty),0) as Qty from Order_QtyShip WITH (NOLOCK) where Id = '{0}' and ShipmodeID in (select ID from ShipMode WITH (NOLOCK) where UseFunction like '%AirPP%')) b
", MyUtility.Convert.GetString(CurrentMaintain["OrderID"]))) != "0")
            {
                this.btnAirPPList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
                this.btnAirPPList.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                this.btnAirPPList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
                this.btnAirPPList.ForeColor = System.Drawing.Color.Black;
            }

            DataTable orderData;
            DualResult result = DBProxy.Current.Select(null, string.Format(@"select o.FactoryID,o.BrandID,o.StyleID,o.Dest,isnull(oq.ShipmodeID,'') as ShipmodeID,isnull(oq.Qty,0) as Qty,oq.BuyerDelivery,isnull(s.Description,'') as Description
from Orders o WITH (NOLOCK) 
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID and oq.Seq = '{1}'
left join Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
where o.Id = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["OrderID"]), MyUtility.Convert.GetString(CurrentMaintain["OrderShipmodeSeq"])), out orderData);

            if (!result || orderData.Rows.Count == 0)
            {
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Query order fail.\r\n" + result.ToString());
                }
                displayStyleNo.Value = "";
                displayBrand.Value = "";
                displayDescription.Value = "";
                displayFactory.Value = "";
                displayShipMode.Value = "";
                numOrderQty.Value = 0;
                dateBuyerDelivery.Value = null;
                txtCountryDestination.TextBox1.Text = "";
            }
            else
            {
                displayStyleNo.Value = MyUtility.Convert.GetString(orderData.Rows[0]["StyleID"]);
                displayBrand.Value = MyUtility.Convert.GetString(orderData.Rows[0]["BrandID"]);
                displayDescription.Value = MyUtility.Convert.GetString(orderData.Rows[0]["Description"]);
                displayFactory.Value = MyUtility.Convert.GetString(orderData.Rows[0]["FactoryID"]);
                displayShipMode.Value = MyUtility.Convert.GetString(orderData.Rows[0]["ShipmodeID"]);
                numOrderQty.Value = MyUtility.Convert.GetInt(orderData.Rows[0]["Qty"]);
                dateBuyerDelivery.Value = MyUtility.Convert.GetDate(orderData.Rows[0]["BuyerDelivery"]);
                txtCountryDestination.TextBox1.Text = MyUtility.Convert.GetString(orderData.Rows[0]["Dest"]);
            }

            displayResponsibilityJustifcation.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Air_Prepaid_Reason' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ReasonID"])));
            displayTPEEditDate.Value = MyUtility.Check.Empty(CurrentMaintain["TPEEditDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["TPEEditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            displayUpdTPEDate.Value = MyUtility.Check.Empty(CurrentMaintain["FtySendDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["FtySendDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            //狀態顯示
            switch (MyUtility.Convert.GetString(CurrentMaintain["Status"]))
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
            //CurrentMaintain["FtyMgr"] = MyUtility.GetValue.Lookup("Manager", Sci.Env.User.Factory, "Factory", "ID");
            CurrentMaintain["RatioFty"] = 0;
            CurrentMaintain["RatioSubcon"] = 0;
            CurrentMaintain["RatioSCI"] = 0;
            CurrentMaintain["RatioSupp"] = 0;
            CurrentMaintain["RatioBuyer"] = 0;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;

            ControlFactoryRatio(true);
            ControlSubconRatio(true);
            ControlSCIRatio(true);
            ControlSupplierRatio(true);
            ControlBuyerRatio(true);

        }

        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "New")
            {
                if (!Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["AddName"])))
                {
                    MyUtility.Msg.WarningBox("You have no permission!");
                    return false;
                }
            }
            else
            {
                if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Junked")
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
                txtSpNo.Focus();
                MyUtility.Msg.WarningBox("SP No. can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["OrderShipmodeSeq"]))
            {
                txtSeq.Focus();
                MyUtility.Msg.WarningBox("Seq can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["PPICMgr"]))
            {
                txtUserPPICmgr.TextBox1.Focus();
                MyUtility.Msg.WarningBox("PPIC mgr can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["FtyMgr"]))
            {
                txtUserFactorymgr.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Factory mgr can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleFty"]) && MyUtility.Check.Empty(CurrentMaintain["RatioFty"]))
            {
                numFactoryRatio.Focus();
                MyUtility.Msg.WarningBox("Factory Ratio% can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSubcon"]) && MyUtility.Check.Empty(CurrentMaintain["RatioSubcon"]))
            {
                numSubconRatio.Focus();
                MyUtility.Msg.WarningBox("Subcon Ratio% can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSCI"]) && MyUtility.Check.Empty(CurrentMaintain["RatioSCI"]))
            {
                numSCIRatio.Focus();
                MyUtility.Msg.WarningBox("SCI Ratio% can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSupp"]) && MyUtility.Check.Empty(CurrentMaintain["RatioSupp"]))
            {
                numSupplierRatio.Focus();
                MyUtility.Msg.WarningBox("Supplier Ratio% can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleBuyer"]) && MyUtility.Check.Empty(CurrentMaintain["RatioBuyer"]))
            {
                numBuyerRatio.Focus();
                MyUtility.Msg.WarningBox("Buyer Ratio% can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleFty"]) && MyUtility.Check.Empty(CurrentMaintain["ResponsibleFtyNo"]))
            {
                txtfactory.Focus();
                MyUtility.Msg.WarningBox("Factory can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["ResponsibleSubcon"]) && MyUtility.Check.Empty(CurrentMaintain["SubConName"]))
            {
                txtSubconName.Focus();
                MyUtility.Msg.WarningBox("Subcon Name can't empty!!");
                return false;
            }

            //Ratio加總要等於100
            if (MyUtility.Convert.GetDecimal(CurrentMaintain["RatioFty"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["RatioSubcon"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["RatioSCI"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["RatioSupp"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["RatioBuyer"]) != 100)
            {
                MyUtility.Msg.WarningBox("Total ratio% of cause & responsible detail is not 100%.");
                return false;
            }

            //Air Qty要等於Order Qty
            if (MyUtility.Convert.GetInt(CurrentMaintain["ShipQty"]) != MyUtility.Convert.GetInt(numOrderQty.Value))
            {
                MyUtility.Msg.WarningBox("Air Q'ty not equal to Order Q'ty!!");
                return false;
            }

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("KeyWord",MyUtility.GetValue.Lookup("FtyGroup", CurrentMaintain["OrderID"].ToString(), "Orders", "ID"),"Factory","ID") + "AP", "AirPP", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            //新增資料時，要寫一筆紀錄到AirPP_History
            if (IsDetailInserting)
            {
                string insertCmd = string.Format(@"insert into AirPP_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','Status','','New','{1}',GETDATE())", MyUtility.Convert.GetString(CurrentMaintain["ID"]), Sci.Env.User.UserID);

                DualResult result = DBProxy.Current.Execute(null, insertCmd);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Insert AirPP_History fail!\r\n" + result.ToString());
                    return failResult;
                }
            }
            return Result.True;
        }

        protected override bool ClickPrint()
        {
            ToExcel(false);
            return base.ClickPrint();
        }

        private bool ToExcel(bool isSendMail)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("No data!!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P01.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[3, 5] = label30.Text;
            worksheet.Cells[3, 8] = Convert.ToDateTime(DateTime.Today).ToString("d");
            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(CurrentMaintain["ID"]);
            worksheet.Cells[4, 7] = MyUtility.Check.Empty(dateBuyerDelivery.Value)?"": Convert.ToDateTime(dateBuyerDelivery.Value).ToString("d");
            worksheet.Cells[5, 7] = MyUtility.GetValue.Lookup(string.Format("select Name from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["MRHandle"])));
            worksheet.Cells[6, 2] = MyUtility.Convert.GetString(CurrentMaintain["OrderID"]);
            worksheet.Cells[6, 6] = displayFactory.Value;
            worksheet.Cells[7, 2] = displayBrand.Value;
            worksheet.Cells[7, 4] = MyUtility.GetValue.Lookup(string.Format("select Alias from Country WITH (NOLOCK) where ID = '{0}'", txtCountryDestination.TextBox1.Text));
            worksheet.Cells[8, 2] = displayStyleNo.Value;
            worksheet.Cells[8, 4] = displayDescription.Value;
            worksheet.Cells[9, 2] = numOrderQty.Value;
            worksheet.Cells[10, 2] = MyUtility.Convert.GetString(CurrentMaintain["ShipQty"]);
            worksheet.Cells[10, 4] = MyUtility.Convert.GetString(CurrentMaintain["Forwarder"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Abb from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Forwarder"])));
            worksheet.Cells[10, 6] = MyUtility.Convert.GetString(CurrentMaintain["Quotation"]) + "/KG";
            worksheet.Cells[11, 2] = MyUtility.Convert.GetString(CurrentMaintain["GW"]);
            worksheet.Cells[11, 4] = MyUtility.Convert.GetString(CurrentMaintain["Forwarder1"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Abb from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Forwarder1"])));
            worksheet.Cells[11, 6] = MyUtility.Convert.GetString(CurrentMaintain["Quotation1"]) + "/KG";
            worksheet.Cells[12, 2] = MyUtility.Convert.GetString(CurrentMaintain["VW"]);
            worksheet.Cells[12, 4] = MyUtility.Convert.GetString(CurrentMaintain["Forwarder2"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Abb from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Forwarder2"])));
            worksheet.Cells[12, 6] = MyUtility.Convert.GetString(CurrentMaintain["Quotation2"]) + "/KG";
            worksheet.Cells[13, 2] = MyUtility.Convert.GetString(CurrentMaintain["Rate"]);
            worksheet.Cells[13, 5] = MyUtility.Convert.GetString(CurrentMaintain["EstAmount"]);
            worksheet.Cells[13, 8] = MyUtility.Convert.GetString(CurrentMaintain["ActualAmount"]);
            worksheet.Cells[14, 2] = MyUtility.Convert.GetString(CurrentMaintain["ReasonID"]) + "." + displayResponsibilityJustifcation.Value;
            worksheet.Cells[15, 2] = MyUtility.Convert.GetString(CurrentMaintain["ResponsibleFty"]).ToUpper() == "TRUE" ? "Y" : "";
            worksheet.Cells[15, 4] = MyUtility.Convert.GetString(CurrentMaintain["RatioFty"]) + "%";
            worksheet.Cells[15, 6] = MyUtility.Convert.GetString(CurrentMaintain["ResponsibleFtyNo"]);
            worksheet.Cells[16, 2] = MyUtility.Convert.GetString(CurrentMaintain["ResponsibleSubcon"]).ToUpper() == "TRUE" ? "Y" : "";
            worksheet.Cells[16, 4] = MyUtility.Convert.GetString(CurrentMaintain["RatioSubcon"]) + "%";
            worksheet.Cells[16, 6] = MyUtility.Convert.GetString(CurrentMaintain["SubconDBCNo"]);
            worksheet.Cells[16, 8] = MyUtility.Convert.GetString(CurrentMaintain["SubConName"]);
            worksheet.Cells[17, 2] = MyUtility.Convert.GetString(CurrentMaintain["ResponsibleSCI"]).ToUpper() == "TRUE" ? "Y" : "";
            worksheet.Cells[17, 4] = MyUtility.Convert.GetString(CurrentMaintain["RatioSCI"]) + "%";
            worksheet.Cells[17, 6] = MyUtility.Convert.GetString(CurrentMaintain["SCIICRNo"]);
            worksheet.Cells[17, 8] = MyUtility.Convert.GetString(CurrentMaintain["SCIICRRemark"]);
            worksheet.Cells[18, 2] = MyUtility.Convert.GetString(CurrentMaintain["ResponsibleSupp"]).ToUpper() == "TRUE" ? "Y" : "";
            worksheet.Cells[18, 4] = MyUtility.Convert.GetString(CurrentMaintain["RatioSupp"]) + "%";
            worksheet.Cells[18, 6] = MyUtility.Convert.GetString(CurrentMaintain["SuppDBCNo"]);
            worksheet.Cells[18, 8] = MyUtility.Convert.GetString(CurrentMaintain["SuppDBCRemark"]);
            worksheet.Cells[19, 2] = MyUtility.Convert.GetString(CurrentMaintain["ResponsibleBuyer"]).ToUpper() == "TRUE" ? "Y" : "";
            worksheet.Cells[19, 4] = MyUtility.Convert.GetString(CurrentMaintain["RatioBuyer"]) + "%";
            worksheet.Cells[19, 6] = MyUtility.Convert.GetString(CurrentMaintain["BuyerDBCNo"]);
            worksheet.Cells[19, 8] = MyUtility.Convert.GetString(CurrentMaintain["BuyerDBCRemark"]);
            worksheet.Cells[20, 6] = MyUtility.Convert.GetString(CurrentMaintain["BuyerICRNo"]);
            worksheet.Cells[20, 8] = MyUtility.Convert.GetString(CurrentMaintain["BuyerICRRemark"]);
            worksheet.Cells[21, 6] = MyUtility.Convert.GetString(CurrentMaintain["BuyerRemark"]);
            worksheet.Cells[22, 2] = MyUtility.Convert.GetString(CurrentMaintain["FtyDesc"]);
            worksheet.Cells[23, 2] = MyUtility.Convert.GetString(CurrentMaintain["MRComment"]);

            #region Save Excel
            excelFile = Sci.Production.Class.MicrosoftFile.GetName("Shipping_P01");
            excel.ActiveWorkbook.SaveAs(excelFile);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            #endregion
            if (!isSendMail)
            {
                excelFile.OpenFile();
            }
            return true;
        }


        private void txtResponsibilityJustifcation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Air_Prepaid_Reason' and Junk = 0 order by ID", "5,50", this.Text, false, ",");

                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }

                IList<DataRow> reasonData = item.GetSelecteds();
                CurrentMaintain["ReasonID"] = item.GetSelectedString();
                displayResponsibilityJustifcation.Value = MyUtility.Convert.GetString(reasonData[0]["Name"]);
            }
        }

        //Calculate Est. Amount
        private void CalculateEstAmt()
        {
            decimal gw = MyUtility.Convert.GetDecimal(CurrentMaintain["GW"]);
            decimal vw = MyUtility.Convert.GetDecimal(CurrentMaintain["VW"]);
            decimal qt = MyUtility.Convert.GetDecimal(CurrentMaintain["Quotation"]);
            CurrentMaintain["EstAmount"] = MyUtility.Math.Round((gw > vw ? gw : vw) * qt, 4);
        }

        //Gross Weight(Kgs)
        private void numGrossWeight_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && numGrossWeight.OldValue != numGrossWeight.Value)
            {
                CalculateEstAmt();
            }
        }

        //V.Weight(Kgs)
        private void numVWeight_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && numVWeight.OldValue != numVWeight.Value)
            {
                CalculateEstAmt();
            }
        }

        //Quotation(USD/Kgs)
        private void numForwarderNQuotation_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && numForwarderNQuotation.OldValue != numForwarderNQuotation.Value)
            {
                CalculateEstAmt();
            }
        }

        //Factory
        private void checkFactory_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                ControlFactoryRatio(!checkFactory.Checked);
                CurrentMaintain["ResponsibleFty"] = checkFactory.Checked;
                if (!checkFactory.Checked)
                {
                    CurrentMaintain["RatioFty"] = 0;
                    CurrentMaintain["ResponsibleFtyNo"] = "";
                }
            }
        }

        //Subcon
        private void checkSubcon_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                ControlSubconRatio(!checkSubcon.Checked);
                CurrentMaintain["ResponsibleSubcon"] = checkSubcon.Checked;
                if (!checkSubcon.Checked)
                {
                    CurrentMaintain["RatioSubcon"] = 0;
                    CurrentMaintain["SubconDBCNo"] = "";
                    CurrentMaintain["SubConName"] = "";
                }
            }
        }

        //SCI
        private void checkSCI_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                ControlSCIRatio(!checkSCI.Checked);
                CurrentMaintain["ResponsibleSCI"] = checkSCI.Checked;
                if (!checkSCI.Checked)
                {
                    CurrentMaintain["RatioSCI"] = 0;
                }
            }
        }

        //Supplier
        private void checkSupplier_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                ControlSupplierRatio(!checkSupplier.Checked);
                CurrentMaintain["ResponsibleSupp"] = checkSupplier.Checked;
                if (!checkSupplier.Checked)
                {
                    CurrentMaintain["RatioSupp"] = 0;
                }
            }
        }

        //Buyer
        private void checkBuyer_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                ControlBuyerRatio(!checkBuyer.Checked);
                CurrentMaintain["ResponsibleBuyer"] = checkBuyer.Checked;
                if (!checkBuyer.Checked)
                {
                    CurrentMaintain["RatioBuyer"] = 0;
                    CurrentMaintain["BuyerRemark"] = "";
                }
            }
        }

        private void ControlFactoryRatio(bool ReadOnly)
        {
            numFactoryRatio.ReadOnly = ReadOnly;
            txtfactory.ReadOnly = ReadOnly;
        }

        private void ControlSubconRatio(bool ReadOnly)
        {
            numSubconRatio.ReadOnly = ReadOnly;
            txtDebitMemo.ReadOnly = ReadOnly;
            txtSubconName.ReadOnly = ReadOnly;
        }

        private void ControlSCIRatio(bool ReadOnly)
        {
            numSCIRatio.ReadOnly = ReadOnly;
        }

        private void ControlSupplierRatio(bool ReadOnly)
        {
            numSupplierRatio.ReadOnly = ReadOnly;
        }

        private void ControlBuyerRatio(bool ReadOnly)
        {
            numBuyerRatio.ReadOnly = ReadOnly;
            txtBuyer.ReadOnly = ReadOnly;
        }

        //檢查輸入的SP#是否正確
        private void txtSpNo_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (txtSpNo.Text != txtSpNo.OldValue)
                {
                    #region 檢查輸入的值是否符合條件
                    if (!MyUtility.Check.Empty(txtSpNo.Text))
                    {
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", txtSpNo.Text);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@mdivisionid", Sci.Env.User.Keyword);

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable orderData;
                        string sqlCmd = "select ID from Orders WITH (NOLOCK) where ID = @id and MDivisionID = @mdivisionid";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orderData);
                        if (!result || orderData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("< SP No. > not found!");
                            }
                            //OrderID異動，其他相關欄位要跟著異動
                            ChangeOtherData("");
                            txtSpNo.Text = "";
                            e.Cancel = true;
                            return;
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
                displayStyleNo.Value = "";
                displayBrand.Value = "";
                displayDescription.Value = "";
                displayFactory.Value = "";
                displayShipMode.Value = "";
                numOrderQty.Value = 0;
                dateBuyerDelivery.Value = null;
                txtCountryDestination.TextBox1.Text = "";
                CurrentMaintain["OrderShipmodeSeq"] = "";
                CurrentMaintain["POHandle"] = "";
                CurrentMaintain["POSMR"] = "";
                CurrentMaintain["MRHandle"] = "";
                CurrentMaintain["SMR"] = "";
                CurrentMaintain["ShipQty"] = 0;
                CurrentMaintain["FtyMgr"] = "";
            }
            else
            {
                DataRow orderData;
                string sqlCmd;
                sqlCmd = string.Format(@"
select o.FactoryID
       , o.FtyGroup
       , o.BrandID
       , o.StyleID
       , o.Dest
       , isnull(s.Description,'') as Description
       , p.POHandle
       , p.POSMR
       , o.MRHandle
       , o.SMR
from Orders o WITH (NOLOCK) 
left join Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
left join PO p WITH (NOLOCK) on p.ID = o.POID
where o.Id = '{0}'", orderID);
                if (MyUtility.Check.Seek(sqlCmd, out orderData))
                {
                    //帶出相關欄位的資料
                    displayStyleNo.Value = MyUtility.Convert.GetString(orderData["StyleID"]);
                    displayBrand.Value = MyUtility.Convert.GetString(orderData["BrandID"]);
                    displayDescription.Value = MyUtility.Convert.GetString(orderData["Description"]);
                    displayFactory.Value = MyUtility.Convert.GetString(orderData["FactoryID"]);
                    txtCountryDestination.TextBox1.Text = MyUtility.Convert.GetString(orderData["Dest"]);
                    CurrentMaintain["POHandle"] = orderData["POHandle"];
                    CurrentMaintain["POSMR"] = orderData["POSMR"];
                    CurrentMaintain["MRHandle"] = orderData["MRHandle"];
                    CurrentMaintain["SMR"] = orderData["SMR"];
                    CurrentMaintain["FtyMgr"] = MyUtility.GetValue.Lookup("Manager", MyUtility.Convert.GetString(orderData["FtyGroup"]),"Factory","ID");

                    #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                    sqlCmd = string.Format(@"
select oq.Seq
       , oq.BuyerDelivery
       , oq.ShipmodeID
       , oq.Qty 
from Order_QtyShip oq WITH (NOLOCK) 
     , (select Id
               , Seq 
        from Order_QtyShip WITH (NOLOCK) 
        where Id = '{0}' 
              and 
              ShipmodeID in (select ID 
                             from ShipMode WITH (NOLOCK) 
                             where UseFunction like '%AirPP%')
        except
        select OrderID as ID
               , OrderShipmodeSeq as Seq 
        from AirPP WITH (NOLOCK) 
        where OrderID = '{0}' 
              and ID != '{1}' 
              and Status <> 'Junked'
      ) b
where oq.Id = b.Id 
      and oq.Seq = b.Seq", orderID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                    DataTable orderQtyData;
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, out orderQtyData);

                    if (result)
                    {
                        if (orderQtyData.Rows.Count == 0)
                        {
                            displayStyleNo.Text = "";
                            dateCreatedate.Value = null;
                            displayBrand.Text = "";
                            displayDescription.Text = "";
                            displayFactory.Text = "";
                            txtCountryDestination.TextBox1.Text = "";
                            txtCountryDestination.DisplayBox1.Text = "";

                            txttpeuserPOHandle.DisplayBox1.Text = "";
                            txttpeuserPOHandle.DisplayBox2.Text = "";
                            txttpeuserPOSMR.DisplayBox1.Text = "";
                            txttpeuserPOSMR.DisplayBox2.Text = "";
                            txttpeuserMR.DisplayBox1.Text = "";
                            txttpeuserMR.DisplayBox2.Text = "";
                            txttpeuserSMR.DisplayBox1.Text = "";
                            txttpeuserSMR.DisplayBox2.Text = "";
                            txttpeuserTask.DisplayBox1.Text = "";
                            txttpeuserTask.DisplayBox2.Text = "";
                            txtSpNo.Text = "";
                            MyUtility.Msg.InfoBox(string.Format("SP#:{0} The Air Pre-Paid is already created.", orderID));
                            return;
                        }
                        if (orderQtyData.Rows.Count == 1)
                        {
                            CurrentMaintain["OrderShipmodeSeq"] = orderQtyData.Rows[0]["Seq"];
                            displayShipMode.Value = MyUtility.Convert.GetString(orderQtyData.Rows[0]["ShipmodeID"]);
                            numOrderQty.Value = MyUtility.Convert.GetInt(orderQtyData.Rows[0]["Qty"]);
                            CurrentMaintain["ShipQty"] = MyUtility.Convert.GetInt(orderQtyData.Rows[0]["Qty"]);
                            dateBuyerDelivery.Value = Convert.ToDateTime(orderQtyData.Rows[0]["BuyerDelivery"]);
                        }
                        else
                        {
                            IList<DataRow> orderQtyShipData;
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(orderQtyData, "Seq,BuyerDelivery,ShipModeID,Qty", "4,20,20,10", "", false, "", "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                CurrentMaintain["OrderShipmodeSeq"] = "";
                                displayShipMode.Value = "";
                                numOrderQty.Value = 0;
                                CurrentMaintain["ShipQty"] = 0;
                                dateCreatedate.Value = null;
                            }
                            else
                            {
                                orderQtyShipData = item.GetSelecteds();
                                CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                                displayShipMode.Value = MyUtility.Convert.GetString(orderQtyShipData[0]["ShipmodeID"]);
                                numOrderQty.Value = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"]);
                                CurrentMaintain["ShipQty"] = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"]);
                                dateBuyerDelivery.Value = Convert.ToDateTime(orderQtyShipData[0]["BuyerDelivery"]);
                            }
                        }
                    }
                    else
                    {
                        CurrentMaintain["OrderShipmodeSeq"] = "";
                        displayShipMode.Value = "";
                        numOrderQty.Value = 0;
                        CurrentMaintain["ShipQty"] = 0;
                        dateCreatedate.Value = null;
                        MyUtility.Msg.ErrorBox("Query Seq fail.\r\n" + result.ToString());
                    }
                    #endregion
                }
            }
        }

        private void txtSpNo_Validated(object sender, EventArgs e)
        {
            if (txtSpNo.OldValue == txtSpNo.Text)
            {
                return;
            }

            //OrderID異動，其他相關欄位要跟著異動
            ChangeOtherData(txtSpNo.Text);
        }

        //Seq按右鍵
        private void txtSeq_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format(@"select oq.Seq,oq.BuyerDelivery,oq.ShipmodeID,oq.Qty from Order_QtyShip oq WITH (NOLOCK) ,(
select Id,Seq from Order_QtyShip WITH (NOLOCK) where Id = '{0}' and 
ShipmodeID in (select ID from ShipMode WITH (NOLOCK) where UseFunction like '%AirPP%')
except
select OrderID as ID,OrderShipmodeSeq as Seq from AirPP WITH (NOLOCK) where OrderID = '{0}' and ID != '{1}' and Status <> 'Junked') b
where oq.Id = b.Id and oq.Seq = b.Seq", MyUtility.Check.Empty(CurrentMaintain["OrderID"]) ? "" : CurrentMaintain["OrderID"], MyUtility.Check.Empty(CurrentMaintain["ID"]) ? "" : CurrentMaintain["ID"].ToString());
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", "", "Seq,Buyer Delivery,ShipMode,Qty");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                CurrentMaintain["OrderShipmodeSeq"] = "";
                CurrentMaintain["ShipModeID"] = "";
                numOrderQty.Value = 0;
                CurrentMaintain["ShipQty"] = 0;
                dateCreatedate.Value = null;
            }
            else
            {
                IList<DataRow> orderQtyShipData;
                orderQtyShipData = item.GetSelecteds();
                CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                displayShipMode.Value = orderQtyShipData[0]["ShipmodeID"].ToString();
                numOrderQty.Value = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"]);
                CurrentMaintain["ShipQty"] = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"]);
                if (MyUtility.Check.Empty(orderQtyShipData[0]["BuyerDelivery"]))
                { dateBuyerDelivery.Value = null; }
                else
                { dateBuyerDelivery.Value = Convert.ToDateTime(orderQtyShipData[0]["BuyerDelivery"]); }
            }
        }

        //Junk
        protected override void ClickJunk()
        {
            if (!Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["AddName"])))
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
values ('{0}','Status','New','Junked','{1}','{2}','{3}',GetDate())", MyUtility.Convert.GetString(CurrentMaintain["ID"]), callReason.ReturnReason, callReason.ReturnRemark, Sci.Env.User.UserID);
                string updateCmd = string.Format(@"update AirPP set Status = 'Junked', EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

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
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("Junk failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
           
            SendMail(false);
        }

        //PPIC mgr Approve
        protected override void ClickCheck()
        {
            if (!Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["PPICMgr"])))
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

            checkqty();

            string insertCmd = string.Format(@"insert into AirPP_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','Status','New','Checked','{1}',GetDate())", MyUtility.Convert.GetString(CurrentMaintain["ID"]), Sci.Env.User.UserID);
            string updateCmd = string.Format(@"update AirPP set Status = 'Checked', PPICMgrApvDate = GetDate(), EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

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
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("UnCheck failed, Pleaes re-try");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

          
        }

        //PPIC mgr UnApprove
        protected override void ClickUncheck()
        {
            if (!Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["PPICMgr"])))
            {
                MyUtility.Msg.WarningBox("Sorry, you don't have permission to uncheck this data. ");
                return;
            }
            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Air_Prepaid_unApprove");
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                string insertCmd = string.Format(@"insert into AirPP_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','Status','Checked','New','{1}','{2}','{3}',GetDate())", MyUtility.Convert.GetString(CurrentMaintain["ID"]), callReason.ReturnReason, callReason.ReturnRemark, Sci.Env.User.UserID);
                string updateCmd = string.Format(@"update AirPP set Status = 'New', PPICMgrApvDate = null, EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

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
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("UnCheck failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
            
        }

        //Factory mgr Approve
        protected override void ClickConfirm()
        {
            if (!Prgs.GetAuthority(MyUtility.Convert.GetString(CurrentMaintain["FtyMgr"])))
            {
                MyUtility.Msg.WarningBox("Sorry, you don't have permission to check this data. ");
                return;
            }

            checkqty();

            string insertCmd = string.Format(@"insert into AirPP_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','Status','Checked','Approved','{1}',GetDate())", MyUtility.Convert.GetString(CurrentMaintain["ID"]), Sci.Env.User.UserID);
            string updateCmd = string.Format(@"update AirPP set Status = 'Approved', FtyMgrApvDate = GetDate(), EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

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
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            RenewData();
            SendMail(false);
        }

        private void checkqty()
        {
            string sp = MyUtility.Convert.GetString(CurrentMaintain["OrderID"]);
            string seq = MyUtility.Convert.GetString(CurrentMaintain["OrderShipmodeSeq"]);
            string ShipQty = MyUtility.Convert.GetString(CurrentMaintain["ShipQty"]);
            string qty = MyUtility.GetValue.Lookup(string.Format("select Qty from Order_QtyShip where id = '{0}' and Seq = '{1}'", sp, seq));
            if (ShipQty != qty)
            {
                MyUtility.Msg.ErrorBox(string.Format("<SP> {0} <Seq> {1} <Air Qty> {2} is not correct, please check again!", sp, seq, ShipQty));
                return;
            }
        }

        //Status update history
        private void btnStatusUpdateHistory_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("AirPP_History", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "Status", reasonType: "Air_Prepaid_unApprove", caption: "Status Update History");
            callNextForm.ShowDialog(this);
        }

        //Mail To
        private void btnMailto_Click(object sender, EventArgs e)
        {
            SendMail(false);
        }

        //寄Mail
        private void SendMail(bool visibleForm)
        {
            DataRow dr;
            if (MyUtility.Check.Seek("select * from MailTo WITH (NOLOCK) where ID = '008'", out dr))
            {
                DataTable allMail;
                string sqlCmd = string.Format(@"select isnull((select EMail from Pass1 WITH (NOLOCK) where ID = a.PPICMgr),'') as PPICMgrMail,
isnull((select Name from Pass1 WITH (NOLOCK) where ID = a.PPICMgr),'') as PPICMgrName,
isnull((select ExtNo from Pass1 WITH (NOLOCK) where ID = a.PPICMgr),'') as PPICMgrExtNo,
isnull((select EMail from Pass1 WITH (NOLOCK) where ID = a.FtyMgr),'') as FtyMgrMail,
isnull((select Name from Pass1 WITH (NOLOCK) where ID = a.FtyMgr),'') as FtyMgrName,
isnull((select ExtNo from Pass1 WITH (NOLOCK) where ID = a.FtyMgr),'') as FtyMgrExtNo,
isnull((select EMail from TPEPass1 WITH (NOLOCK) where ID = o.MRHandle),'') as MRHandleMail,
isnull((select Name from TPEPass1 WITH (NOLOCK) where ID = o.MRHandle),'') as MRHandleName,
isnull((select ExtNo from TPEPass1 WITH (NOLOCK) where ID = o.MRHandle),'') as MRHandleExtNo,
isnull((select EMail from TPEPass1 WITH (NOLOCK) where ID = o.SMR),'') as SMRMail,
isnull((select Name from TPEPass1 WITH (NOLOCK) where ID = o.SMR),'') as SMRName,
isnull((select ExtNo from TPEPass1 WITH (NOLOCK) where ID = o.SMR),'') as SMRExtNo,
isnull((select EMail from TPEPass1 WITH (NOLOCK) where ID = p.POHandle),'') as POHandleMail,
isnull((select Name from TPEPass1 WITH (NOLOCK) where ID = p.POHandle),'') as POHandleName,
isnull((select ExtNo from TPEPass1 WITH (NOLOCK) where ID = p.POHandle),'') as POHandleExtNo,
isnull((select EMail from TPEPass1 WITH (NOLOCK) where ID = p.POSMR),'') as POSMRMail,
isnull((select Name from TPEPass1 WITH (NOLOCK) where ID = p.POSMR),'') as POSMRName,
isnull((select ExtNo from TPEPass1 WITH (NOLOCK) where ID = p.POSMR),'') as POSMRExtNo
from AirPP a WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = a.OrderID
left join PO p WITH (NOLOCK) on p.ID = o.POID
where a.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out allMail);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Query mail list fail.\r\n" + result.ToString());
                    return;
                }

                string mailto = MyUtility.Convert.GetString(allMail.Rows[0]["POSMRMail"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["POHandleMail"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["MRHandleMail"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["SMRMail"]) + ";";
                string cc = MyUtility.Convert.GetString(allMail.Rows[0]["PPICMgrMail"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["FtyMgrMail"]) + ";" + MyUtility.Convert.GetString(dr["ToAddress"]);
                string subject = string.Format(@"<{0}> {1} for SP#{2}, DD{3} - {4}",
                    MyUtility.Convert.GetString(displayFactory.Value), MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["OrderID"]), Convert.ToDateTime(CurrentMaintain["CDate"]).ToString("yyyyMMdd"), MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Junked" ? "cancel" : "request");
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
Remind:Please return the air pp request – approved  within 24hrs to avoid any shipment delay.", MyUtility.Convert.GetString(allMail.Rows[0]["SMRName"]), MyUtility.Convert.GetString(allMail.Rows[0]["SMRExtNo"]),
                                                                                              MyUtility.Convert.GetString(allMail.Rows[0]["POSMRName"]), MyUtility.Convert.GetString(allMail.Rows[0]["POSMRExtNo"]), MyUtility.Convert.GetString(displayFactory.Value), MyUtility.Convert.GetString(CurrentMaintain["ID"]),
                                                                                              MyUtility.Convert.GetString(CurrentMaintain["OrderID"]), MyUtility.Check.Empty(dateBuyerDelivery.Value) ? "" : Convert.ToDateTime(dateBuyerDelivery.Value).ToString("yyyyMMdd"), MyUtility.Convert.GetString(CurrentMaintain["ShipQty"]),
                                                                                              MyUtility.Convert.GetString(CurrentMaintain["ResponsibleFty"]) == "True" ? "Factory:" + MyUtility.Convert.GetString(CurrentMaintain["ResponsibleFtyNo"]) + "\r\n" : "",
                                                                                              MyUtility.Convert.GetString(CurrentMaintain["ResponsibleSubcon"]) == "True" ? "Subcon:DBC #:" + MyUtility.Convert.GetString(CurrentMaintain["SubconDBCNo"]) + "\r\n" : "",
                                                                                              MyUtility.Convert.GetString(CurrentMaintain["ResponsibleSCI"]) == "True" ? "SCI:ICR #:" + MyUtility.Convert.GetString(CurrentMaintain["SCIICRNo"]) + "\r\n" : "",
                                                                                              MyUtility.Convert.GetString(CurrentMaintain["ResponsibleSupp"]) == "True" ? "Supplier:DBC #:" + MyUtility.Convert.GetString(CurrentMaintain["SuppDBCNo"]) + "\r\n" : "",
                                                                                              MyUtility.Convert.GetString(CurrentMaintain["ResponsibleBuyer"]) == "True" ? "Buyer:Debit Memo:" + MyUtility.Convert.GetString(CurrentMaintain["BuyerDBCNo"]) + ", ICR #" + MyUtility.Convert.GetString(CurrentMaintain["BuyerICRNo"]) + "\r\n" : ""));
                #endregion

                //產生Excel
                ToExcel(true);

                var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, cc, subject, excelFile, content.ToString(), visibleForm, visibleForm);
                email.ShowDialog(this);

                //刪除Excel File
                if (System.IO.File.Exists(excelFile))
                {
                    try
                    {
                        System.IO.File.Delete(excelFile);
                    }
                    catch (System.IO.IOException e)
                    {
                        MyUtility.Msg.WarningBox(string.Format("Delete excel file fail!! {0}", e));
                    }
                }
            }
        }

        //AirPP List
        private void btnAirPPList_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P01_AirPPList callNextForm = new Sci.Production.Shipping.P01_AirPPList(MyUtility.Convert.GetString(CurrentMaintain["OrderID"]));
            callNextForm.ShowDialog(this);
        }

        //Q'ty B'down by Shipmode
        private void btnQtyBDownByShipmode_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyShip callNextForm = new Sci.Production.PPIC.P01_QtyShip(MyUtility.Convert.GetString(CurrentMaintain["OrderID"]), MyUtility.GetValue.Lookup(string.Format("select POID from Orders WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["OrderID"]))));
            callNextForm.ShowDialog(this);
        }

        //Q'ty B'down by Order
        private void btnQtyBdownbyOrder_Click(object sender, EventArgs e)
        {
            string sqlCmd = string.Format(@"select o.POID,isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList
from Orders o WITH (NOLOCK) 
where o.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["OrderID"]));
            DataTable OrderData;
            DBProxy.Current.Select(null, sqlCmd, out OrderData);
            int count = OrderData.Rows.Count;

            Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(CurrentMaintain["OrderID"]),(count ==0)? "": MyUtility.Convert.GetString(OrderData.Rows[0]["POID"]),(count==0)?"":  MyUtility.Convert.GetString(OrderData.Rows[0]["PoList"]));
            callNextForm.ShowDialog(this);
        }

        //GMT Export
        private void btnGMTExport_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_GMTExport callNextForm = new Sci.Production.PPIC.P01_GMTExport(MyUtility.Convert.GetString(CurrentMaintain["OrderID"]));
            callNextForm.ShowDialog(this);
        }

        private void txtResponsibilityJustifcation_Validating(object sender, CancelEventArgs e)
        {
            string str = this.txtResponsibilityJustifcation.Text;
            DataRow dr;
            if (!MyUtility.Check.Empty(this.txtResponsibilityJustifcation.Text))
            {
                string cmd = string.Format(@"select ID,Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Air_Prepaid_Reason' and Junk = 0 and id='{0}' order by ID", this.txtResponsibilityJustifcation.Text);
                if (!MyUtility.Check.Seek(cmd,out dr))
                {
                    e.Cancel = true;
                    this.txtResponsibilityJustifcation.Text = "";
                    displayResponsibilityJustifcation.Value = "";
                    MyUtility.Msg.WarningBox(string.Format("{0}, Data not Found!", str));
                    return;
                }
                else
                {
                    displayResponsibilityJustifcation.Value = MyUtility.Convert.GetString(dr["Name"]);
                }
            }
            else
            {
                displayResponsibilityJustifcation.Value = "";
            }
        }
    }
}

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
    /// <summary>
    /// P01
    /// </summary>
    public partial class P01 : Sci.Win.Tems.Input1
    {
        private string excelFile;
        private decimal numVWeightOldValue;
        private decimal numForwarderNQuotationOldValue;

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            this.labelComments.Text = "MR's \r\nComments";
            this.labelResponsible.Text = "Cause &\r\nResponsible\r\nDetails";
            this.txtUserPreparedBy.TextBox1.ReadOnly = true;
            this.txtUserPreparedBy.TextBox1.IsSupportEditMode = false;
            this.txtuserShipLeader.TextBox1.ReadOnly = true;
            this.txtuserShipLeader.TextBox1.IsSupportEditMode = false;
            this.txtCountryDestination.TextBox1.ReadOnly = true;
            this.txtCountryDestination.TextBox1.IsSupportEditMode = false;

            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "All,All,New,New,Checked,PPIC Checked,Approved,FTY Approved,Comfirmed,SMR Comfirmed,Locked,GM Team Locked");

            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = $" Status = '{this.queryfors.SelectedValue}'"; 
                        break;
                }

                this.ReloadDatas();
            };

            // 預設為PPIC Checked
            this.queryfors.SelectedIndex = 2;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnMailto.Enabled = MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Approved";

            if (MyUtility.GetValue.Lookup(string.Format(
                @"select a.ShipQty-b.Qty as BalQty from 
(select isnull(sum(ShipQty),0) as ShipQty from AirPP WITH (NOLOCK) where Status <> 'Junked' and OrderID = '{0}') a,
(select isnull(sum(Qty),0) as Qty from Order_QtyShip WITH (NOLOCK) where Id = '{0}' and ShipmodeID in (select ID from ShipMode WITH (NOLOCK) where UseFunction like '%AirPP%')) b",
                MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]))) != "0")
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
            DualResult result = DBProxy.Current.Select(
                null,
                string.Format(
                    @"select o.FactoryID,o.BrandID,o.StyleID,o.Dest,isnull(oq.ShipmodeID,'') as ShipmodeID,isnull(oq.Qty,0) as Qty,oq.BuyerDelivery,isnull(s.Description,'') as Description
from Orders o WITH (NOLOCK) 
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID and oq.Seq = '{1}'
left join Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
where o.Id = '{0}'",
                    MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["OrderShipmodeSeq"])),
                out orderData);

            if (!result || orderData.Rows.Count == 0)
            {
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Query order fail.\r\n" + result.ToString());
                }

                this.displayStyleNo.Value = string.Empty;
                this.displayBrand.Value = string.Empty;
                this.displayDescription.Value = string.Empty;
                this.displayFactory.Value = string.Empty;
                this.displayShipMode.Value = string.Empty;
                this.numOrderQty.Value = 0;
                this.dateBuyerDelivery.Value = null;
                this.txtCountryDestination.TextBox1.Text = string.Empty;
            }
            else
            {
                this.displayStyleNo.Value = MyUtility.Convert.GetString(orderData.Rows[0]["StyleID"]);
                this.displayBrand.Value = MyUtility.Convert.GetString(orderData.Rows[0]["BrandID"]);
                this.displayDescription.Value = MyUtility.Convert.GetString(orderData.Rows[0]["Description"]);
                this.displayFactory.Value = MyUtility.Convert.GetString(orderData.Rows[0]["FactoryID"]);
                this.displayShipMode.Value = MyUtility.Convert.GetString(orderData.Rows[0]["ShipmodeID"]);
                this.numOrderQty.Value = MyUtility.Convert.GetInt(orderData.Rows[0]["Qty"]);
                this.dateBuyerDelivery.Value = MyUtility.Convert.GetDate(orderData.Rows[0]["BuyerDelivery"]);
                this.txtCountryDestination.TextBox1.Text = MyUtility.Convert.GetString(orderData.Rows[0]["Dest"]);
            }

            this.displayResponsibilityJustifcation.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Air_Prepaid_Reason' and ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ReasonID"])));
            this.displayTPEEditDate.Value = MyUtility.Check.Empty(this.CurrentMaintain["TPEEditDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["TPEEditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            this.displayUpdTPEDate.Value = MyUtility.Check.Empty(this.CurrentMaintain["FtySendDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["FtySendDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));

            // 狀態顯示
            switch (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]))
            {
                case "New":
                    this.label30.Text = "New";
                    break;
                case "Junked":
                    this.label30.Text = "Junk";
                    break;
                case "Checked":
                    this.label30.Text = "PPIC Checked";
                    break;
                case "Approved":
                    this.label30.Text = "FTY Approved";
                    break;
                case "Confirmed":
                    this.label30.Text = "SMR Comfirmed";
                    break;
                case "Locked":
                    this.label30.Text = "GM Team Locked";
                    break;
                default:
                    this.label30.Text = string.Empty;
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["CDate"] = DateTime.Today;
            this.CurrentMaintain["Status"] = "New";

            // CurrentMaintain["FtyMgr"] = MyUtility.GetValue.Lookup("Manager", Sci.Env.User.Factory, "Factory", "ID");
            this.CurrentMaintain["RatioFty"] = 0;
            this.CurrentMaintain["RatioSubcon"] = 0;
            this.CurrentMaintain["RatioSCI"] = 0;
            this.CurrentMaintain["RatioSupp"] = 0;
            this.CurrentMaintain["RatioBuyer"] = 0;
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;

            this.ControlFactoryRatio(true);
            this.ControlSubconRatio(true);
            this.ControlSCIRatio(true);
            this.ControlSupplierRatio(true);
            this.ControlBuyerRatio(true);
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "New")
            {
                if (!Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["AddName"])))
                {
                    MyUtility.Msg.WarningBox("You have no permission!");
                    return false;
                }
            }
            else
            {
                if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Junked")
                {
                    MyUtility.Msg.WarningBox("This.record is cancel. Can't be modify!");
                    return false;
                }
                else
                {
                    if (!MyUtility.Check.Empty(this.CurrentMaintain["APAmountEditDate"]))
                    {
                        MyUtility.Msg.WarningBox("This record is < Approved >. Can't be modify!");
                        return false;
                    }
                }
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {

            if (MyUtility.Check.Empty(this.CurrentMaintain["APAmountEditDate"]) && this.CurrentMaintain["Status"].ToString() != "New")
            {
                this.SetReadonly();
            }
            else
            {
                this.ControlFactoryRatio(!Convert.ToBoolean(this.CurrentMaintain["ResponsibleFty"]));
                this.ControlSubconRatio(!Convert.ToBoolean(this.CurrentMaintain["ResponsibleSubcon"]));
                this.ControlSCIRatio(!Convert.ToBoolean(this.CurrentMaintain["ResponsibleSCI"]));
                this.ControlSupplierRatio(!Convert.ToBoolean(this.CurrentMaintain["ResponsibleSupp"]));
                this.ControlBuyerRatio(!Convert.ToBoolean(this.CurrentMaintain["ResponsibleBuyer"]));
            }

            base.ClickEditAfter();
        }

        private void SetReadonly()
        {
            this.txtSpNo.ReadOnly = true;
            this.numAirQty.ReadOnly = true;
            this.numGrossWeight.ReadOnly = true;
            this.txtSubconForwarder1.TextBox1.ReadOnly = true;
            this.txtsubcon3.TextBox1.ReadOnly = true;
            this.txtSRNo.ReadOnly = true;
            this.numVWeight.ReadOnly = true;
            this.txtResponsibilityJustifcation.ReadOnly = true;
            this.txtSubconName.ReadOnly = true;
            this.numForwarderNQuotation.ReadOnly = true;
            this.numForwarder1Quotation.ReadOnly = true;
            this.numForwarder2Quotation.ReadOnly = true;
            this.numericBoxCW.ReadOnly = true;
            this.numEstAmt.ReadOnly = true;
            this.numActAmt.ReadOnly = true;
            this.numExchangeRate.ReadOnly = true;
            this.txtExplanation.ReadOnly = true;
            this.checkFactory.ReadOnly = true;
            this.checkSubcon.ReadOnly = true;
            this.checkSCI.ReadOnly = true;
            this.checkSupplier.ReadOnly = true;
            this.checkBuyer.ReadOnly = true;
            this.txtfactory.ReadOnly = true;
            this.txtUserPPICmgr.TextBox1.ReadOnly = true;
            this.txtUserFactorymgr.TextBox1.ReadOnly = true;
            this.datePayDate.ReadOnly = true;

            this.numSCIRatio.ReadOnly = true;
            this.numSupplierRatio.ReadOnly = true;
            this.numBuyerRatio.ReadOnly = true;
            this.numFactoryRatio.ReadOnly = true;
            this.numSubconRatio.ReadOnly = true;
            this.txtBuyer.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region//檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]))
            {
                this.txtSpNo.Focus();
                MyUtility.Msg.WarningBox("SP No. can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderShipmodeSeq"]))
            {
                this.txtSeq.Focus();
                MyUtility.Msg.WarningBox("Seq can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipLeader"]))
            {
                this.txtuserShipLeader.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Shipping Leader can't be empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["ResponsibleFty"]) && MyUtility.Check.Empty(this.CurrentMaintain["RatioFty"]))
            {
                this.numFactoryRatio.Focus();
                MyUtility.Msg.WarningBox("Factory Ratio% can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["ResponsibleSubcon"]) && MyUtility.Check.Empty(this.CurrentMaintain["RatioSubcon"]))
            {
                this.numSubconRatio.Focus();
                MyUtility.Msg.WarningBox("Subcon Ratio% can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["ResponsibleSCI"]) && MyUtility.Check.Empty(this.CurrentMaintain["RatioSCI"]))
            {
                this.numSCIRatio.Focus();
                MyUtility.Msg.WarningBox("SCI Ratio% can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["ResponsibleSupp"]) && MyUtility.Check.Empty(this.CurrentMaintain["RatioSupp"]))
            {
                this.numSupplierRatio.Focus();
                MyUtility.Msg.WarningBox("Supplier Ratio% can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["ResponsibleBuyer"]) && MyUtility.Check.Empty(this.CurrentMaintain["RatioBuyer"]))
            {
                this.numBuyerRatio.Focus();
                MyUtility.Msg.WarningBox("Buyer Ratio% can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["ResponsibleFty"]) && MyUtility.Check.Empty(this.CurrentMaintain["ResponsibleFtyNo"]))
            {
                this.txtfactory.Focus();
                MyUtility.Msg.WarningBox("Factory can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["ResponsibleSubcon"]) && MyUtility.Check.Empty(this.CurrentMaintain["SubConName"]))
            {
                this.txtSubconName.Focus();
                MyUtility.Msg.WarningBox("Subcon Name can't empty!!");
                return false;
            }

            // Ratio加總要等於100
            if (MyUtility.Convert.GetDecimal(this.CurrentMaintain["RatioFty"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["RatioSubcon"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["RatioSCI"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["RatioSupp"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["RatioBuyer"]) != 100)
            {
                MyUtility.Msg.WarningBox("Total ratio% of cause & responsible detail is not 100%.");
                return false;
            }

            // Air Qty要等於Order Qty
            if (MyUtility.Convert.GetInt(this.CurrentMaintain["ShipQty"]) > MyUtility.Convert.GetInt(this.numOrderQty.Value))
            {
                MyUtility.Msg.WarningBox("Air Q'ty<" + this.CurrentMaintain["ShipQty"] + "> cannot greater than Order Q'ty<" + this.numOrderQty.Value + ">!!");
                return false;
            }
            #endregion

            // GetID
            if (this.IsDetailInserting)
            {
                string keyWord = MyUtility.GetValue.Lookup("KeyWord", MyUtility.GetValue.Lookup("FtyGroup", this.CurrentMaintain["OrderID"].ToString(), "Orders", "ID"), "Factory", "ID");
                if (MyUtility.Check.Empty(keyWord))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                string id = MyUtility.GetValue.GetID(keyWord + "AP", "AirPP", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;

            }

            string checkStatus = MyUtility.GetValue.Lookup(string.Format("select Status from AirPP where id = '{0}'", this.CurrentMaintain["ID"].ToString()));

            // 避免Form與DB的狀態不一樣，造成Approved之後還能存一堆東西進去
            if (this.CurrentMaintain["Status"].ToString() == "New")
            {
                if (checkStatus.ToUpper() == "APPROVED")
                {
                    MyUtility.Msg.WarningBox(string.Format("{0} already approved, cannot edit again.", this.CurrentMaintain["ID"].ToString()));
                    return false;
                }
            }

            this.ChangeQuotationAVG();
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            // 新增資料時，要寫一筆紀錄到AirPP_History
            if (this.IsDetailInserting)
            {
                string insertCmd = string.Format(
                    @"insert into AirPP_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','Status','','New','{1}',GETDATE())",
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                    Sci.Env.User.UserID);

                DualResult result = DBProxy.Current.Execute(null, insertCmd);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Insert AirPP_History fail!\r\n" + result.ToString());
                    return failResult;
                }
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            this.ToExcel(false);
            return base.ClickPrint();
        }

        private bool ToExcel(bool isSendMail)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("No data!!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P01.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            string status = MyUtility.GetValue.Lookup(string.Format("select Status from AirPP where id = '{0}'", this.CurrentMaintain["id"].ToString()));
            switch (status)
            {
                case "New":
                    status = "New";
                    break;
                case "Junked":
                    status = "Junk";
                    break;
                case "Checked":
                    status = "PPIC Checked";
                    break;
                case "Approved":
                    status = "FTY Approved";
                    break;
                case "Confirmed":
                    status = "SMR Comfirmed";
                    break;
                case "Locked":
                    status = "GM Team Locked";
                    break;
                default:
                    status = string.Empty;
                    break;
            }

            worksheet.Cells[3, 5] = status;
            worksheet.Cells[3, 8] = Convert.ToDateTime(DateTime.Today).ToString("d");
            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            worksheet.Cells[4, 7] = MyUtility.Check.Empty(this.dateBuyerDelivery.Value) ? string.Empty : Convert.ToDateTime(this.dateBuyerDelivery.Value).ToString("d");
            worksheet.Cells[5, 7] = MyUtility.GetValue.Lookup(string.Format("select Name from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["MRHandle"])));
            worksheet.Cells[6, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]);
            worksheet.Cells[6, 6] = this.displayFactory.Value;
            worksheet.Cells[7, 2] = this.displayBrand.Value;
            worksheet.Cells[7, 4] = MyUtility.GetValue.Lookup(string.Format("select Alias from Country WITH (NOLOCK) where ID = '{0}'", this.txtCountryDestination.TextBox1.Text));
            worksheet.Cells[8, 2] = this.displayStyleNo.Value;
            worksheet.Cells[8, 4] = this.displayDescription.Value;
            worksheet.Cells[9, 2] = this.numOrderQty.Value;
            worksheet.Cells[10, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ShipQty"]);
            worksheet.Cells[10, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Abb from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder"])));
            worksheet.Cells[10, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Quotation"]) + "/KG";
            worksheet.Cells[11, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["GW"]);
            worksheet.Cells[11, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder1"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Abb from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder1"])));
            worksheet.Cells[11, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Quotation1"]) + "/KG";
            worksheet.Cells[12, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["VW"]);
            worksheet.Cells[12, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder2"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Abb from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder2"])));
            worksheet.Cells[12, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["Quotation2"]) + "/KG";
            worksheet.Cells[13, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["Rate"]);
            worksheet.Cells[13, 5] = MyUtility.Convert.GetString(this.CurrentMaintain["EstAmount"]);
            worksheet.Cells[13, 8] = MyUtility.Convert.GetString(this.CurrentMaintain["ActualAmountWVAT"]);
            worksheet.Cells[14, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ReasonID"]) + "." + this.displayResponsibilityJustifcation.Value;
            worksheet.Cells[15, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleFty"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
            worksheet.Cells[15, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["RatioFty"]) + "%";
            worksheet.Cells[15, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleFtyNo"]);
            worksheet.Cells[16, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleSubcon"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
            worksheet.Cells[16, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["RatioSubcon"]) + "%";
            worksheet.Cells[16, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["SubconDBCNo"]);
            worksheet.Cells[16, 8] = MyUtility.Convert.GetString(this.CurrentMaintain["SubConName"]);
            worksheet.Cells[17, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleSCI"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
            worksheet.Cells[17, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["RatioSCI"]) + "%";
            worksheet.Cells[17, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["SCIICRNo"]);
            worksheet.Cells[17, 8] = MyUtility.Convert.GetString(this.CurrentMaintain["SCIICRRemark"]);
            worksheet.Cells[18, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleSupp"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
            worksheet.Cells[18, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["RatioSupp"]) + "%";
            worksheet.Cells[18, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["SuppDBCNo"]);
            worksheet.Cells[18, 8] = MyUtility.Convert.GetString(this.CurrentMaintain["SuppDBCRemark"]);
            worksheet.Cells[19, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleBuyer"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
            worksheet.Cells[19, 4] = MyUtility.Convert.GetString(this.CurrentMaintain["RatioBuyer"]) + "%";
            worksheet.Cells[19, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["BuyerDBCNo"]);
            worksheet.Cells[19, 8] = MyUtility.Convert.GetString(this.CurrentMaintain["BuyerDBCRemark"]);
            worksheet.Cells[20, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["BuyerICRNo"]);
            worksheet.Cells[20, 8] = MyUtility.Convert.GetString(this.CurrentMaintain["BuyerICRRemark"]);
            worksheet.Cells[21, 6] = MyUtility.Convert.GetString(this.CurrentMaintain["BuyerRemark"]);
            worksheet.Cells[22, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["FtyDesc"]);
            worksheet.Cells[23, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["MRComment"]);

            #region Save Excel
            this.excelFile = Sci.Production.Class.MicrosoftFile.GetName("Shipping_P01");
            excel.ActiveWorkbook.SaveAs(this.excelFile);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            #endregion
            if (!isSendMail)
            {
                this.excelFile.OpenFile();
            }

            return true;
        }

        private void TxtResponsibilityJustifcation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Air_Prepaid_Reason' and Junk = 0 order by ID", "5,50", this.Text, false, ",");

                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                IList<DataRow> reasonData = item.GetSelecteds();
                this.CurrentMaintain["ReasonID"] = item.GetSelectedString();
                this.displayResponsibilityJustifcation.Value = MyUtility.Convert.GetString(reasonData[0]["Name"]);
            }
        }

        // Calculate Est. Amount
        private void CalculateEstAmt()
        {
            decimal gw = MyUtility.Convert.GetDecimal(this.CurrentMaintain["GW"]);
            decimal vw = MyUtility.Convert.GetDecimal(this.CurrentMaintain["VW"]);
            decimal qt = MyUtility.Convert.GetDecimal(this.CurrentMaintain["Quotation"]);
            this.CurrentMaintain["EstAmount"] = MyUtility.Math.Round((gw > vw ? gw : vw) * qt, 4);
        }

        // Gross Weight(Kgs)
        private void NumGrossWeight_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.numGrossWeight.OldValue != this.numGrossWeight.Value)
            {
                this.CalculateEstAmt();
            }
        }

        // V.Weight(Kgs)
        private void NumVWeight_Validated(object sender, EventArgs e)
        {
            //if (this.EditMode && this.numVWeight.OldValue != this.numVWeight.Value)\
            if (this.EditMode && this.numVWeightOldValue != this.numVWeight.Value)
            {
                this.CalculateEstAmt();
            }
        }

        // Quotation(USD/Kgs)
        private void NumForwarderNQuotation_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.numForwarderNQuotationOldValue != this.numForwarderNQuotation.Value)
            {
                this.CalculateEstAmt();
            }
        }

        // Factory
        private void CheckFactory_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.ControlFactoryRatio(!this.checkFactory.Checked);
                this.CurrentMaintain["ResponsibleFty"] = this.checkFactory.Checked;
                if (!this.checkFactory.Checked)
                {
                    this.CurrentMaintain["RatioFty"] = 0;
                    this.CurrentMaintain["ResponsibleFtyNo"] = string.Empty;
                }
            }
        }

        // Subcon
        private void CheckSubcon_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.ControlSubconRatio(!this.checkSubcon.Checked);
                this.CurrentMaintain["ResponsibleSubcon"] = this.checkSubcon.Checked;
                if (!this.checkSubcon.Checked)
                {
                    this.CurrentMaintain["RatioSubcon"] = 0;
                    this.CurrentMaintain["SubconDBCNo"] = string.Empty;
                    this.CurrentMaintain["SubConName"] = string.Empty;
                }
            }
        }

        // SCI
        private void CheckSCI_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.ControlSCIRatio(!this.checkSCI.Checked);
                this.CurrentMaintain["ResponsibleSCI"] = this.checkSCI.Checked;
                if (!this.checkSCI.Checked)
                {
                    this.CurrentMaintain["RatioSCI"] = 0;
                }
            }
        }

        // Supplier
        private void CheckSupplier_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.ControlSupplierRatio(!this.checkSupplier.Checked);
                this.CurrentMaintain["ResponsibleSupp"] = this.checkSupplier.Checked;
                if (!this.checkSupplier.Checked)
                {
                    this.CurrentMaintain["RatioSupp"] = 0;
                }
            }
        }

        // Buyer
        private void CheckBuyer_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.ControlBuyerRatio(!this.checkBuyer.Checked);
                this.CurrentMaintain["ResponsibleBuyer"] = this.checkBuyer.Checked;
                if (!this.checkBuyer.Checked)
                {
                    this.CurrentMaintain["RatioBuyer"] = 0;
                    this.CurrentMaintain["BuyerRemark"] = string.Empty;
                }
            }
        }

        private void ControlFactoryRatio(bool readOnly)
        {
            this.numFactoryRatio.ReadOnly = readOnly;
            this.txtfactory.ReadOnly = readOnly;
        }

        private void ControlSubconRatio(bool readOnly)
        {
            this.numSubconRatio.ReadOnly = readOnly;
            this.txtSubconName.ReadOnly = readOnly;
        }

        private void ControlSCIRatio(bool readOnly)
        {
            this.numSCIRatio.ReadOnly = readOnly;
        }

        private void ControlSupplierRatio(bool readOnly)
        {
            this.numSupplierRatio.ReadOnly = readOnly;
        }

        private void ControlBuyerRatio(bool readOnly)
        {
            this.numBuyerRatio.ReadOnly = readOnly;
            this.txtBuyer.ReadOnly = readOnly;
        }

        // 檢查輸入的SP#是否正確
        private void TxtSpNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                if (this.txtSpNo.Text != this.txtSpNo.OldValue)
                {
                    #region 檢查輸入的值是否符合條件
                    if (!MyUtility.Check.Empty(this.txtSpNo.Text))
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", this.txtSpNo.Text);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@mdivisionid", Sci.Env.User.Keyword);

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable orderData;
                        string sqlCmd = "select ID from Orders WITH (NOLOCK) where ID = @id and MDivisionID = @mdivisionid and factoryid in (select id from factory where junk = 0 and IsProduceFty = 1)";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orderData);
                        if (!result || orderData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("< SP No. > not found!");
                            }

                            // OrderID異動，其他相關欄位要跟著異動
                            this.ChangeOtherData(string.Empty);
                            this.txtSpNo.Text = string.Empty;
                            e.Cancel = true;
                            return;
                        }
                    }
                    #endregion
                }
            }
        }

        // OrderID異動，其他相關欄位要跟著異動
        private void ChangeOtherData(string orderID)
        {
            if (MyUtility.Check.Empty(orderID))
            {
                // OrderID為空值時，要把其他相關欄位值清空
                this.displayStyleNo.Value = string.Empty;
                this.displayBrand.Value = string.Empty;
                this.displayDescription.Value = string.Empty;
                this.displayFactory.Value = string.Empty;
                this.displayShipMode.Value = string.Empty;
                this.numOrderQty.Value = 0;
                this.dateBuyerDelivery.Value = null;
                this.txtCountryDestination.TextBox1.Text = string.Empty;
                this.CurrentMaintain["OrderShipmodeSeq"] = string.Empty;
                this.CurrentMaintain["POHandle"] = string.Empty;
                this.CurrentMaintain["POSMR"] = string.Empty;
                this.CurrentMaintain["MRHandle"] = string.Empty;
                this.CurrentMaintain["SMR"] = string.Empty;
                this.CurrentMaintain["ShipQty"] = 0;
                this.CurrentMaintain["FtyMgr"] = string.Empty;
                this.CurrentMaintain["ShipLeader"] = string.Empty;
            }
            else
            {
                DataRow orderData;
                string sqlCmd;
                sqlCmd = string.Format(
                    @"
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
       , b.ShipLeader
       , o.Category
from Orders o WITH (NOLOCK) 
left join Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
left join PO p WITH (NOLOCK) on p.ID = o.POID
left join Brand b WITH (NOLOCK) on o.BrandID = b.ID
where o.Id = '{0}'", orderID);
                if (MyUtility.Check.Seek(sqlCmd, out orderData))
                {
                    // 帶出相關欄位的資料
                    this.displayStyleNo.Value = MyUtility.Convert.GetString(orderData["StyleID"]);
                    this.displayBrand.Value = MyUtility.Convert.GetString(orderData["BrandID"]);
                    this.displayDescription.Value = MyUtility.Convert.GetString(orderData["Description"]);
                    this.displayFactory.Value = MyUtility.Convert.GetString(orderData["FactoryID"]);
                    this.txtCountryDestination.TextBox1.Text = MyUtility.Convert.GetString(orderData["Dest"]);
                    this.CurrentMaintain["POHandle"] = orderData["POHandle"];
                    this.CurrentMaintain["POSMR"] = orderData["POSMR"];
                    this.CurrentMaintain["MRHandle"] = orderData["MRHandle"];
                    this.CurrentMaintain["SMR"] = orderData["SMR"];
                    this.CurrentMaintain["FtyMgr"] = MyUtility.GetValue.Lookup("Manager", MyUtility.Convert.GetString(orderData["FtyGroup"]), "Factory", "ID");
                    this.CurrentMaintain["ShipLeader"] = orderData["ShipLeader"];
                    #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                    sqlCmd = string.Format(
                        @"
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
      and oq.Seq = b.Seq",
                        orderID,
                        MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

                    DataTable orderQtyData;
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, out orderQtyData);

                    if (result)
                    {
                        if (orderQtyData.Rows.Count == 0)
                        {
                            this.displayStyleNo.Text = string.Empty;
                            this.dateCreatedate.Value = null;
                            this.displayBrand.Text = string.Empty;
                            this.displayDescription.Text = string.Empty;
                            this.displayFactory.Text = string.Empty;
                            this.txtCountryDestination.TextBox1.Text = string.Empty;
                            this.txtCountryDestination.DisplayBox1.Text = string.Empty;

                            this.txttpeuserPOHandle.DisplayBox1.Text = string.Empty;
                            this.txttpeuserPOHandle.DisplayBox2.Text = string.Empty;
                            this.txttpeuserPOSMR.DisplayBox1.Text = string.Empty;
                            this.txttpeuserPOSMR.DisplayBox2.Text = string.Empty;
                            this.txttpeuserMR.DisplayBox1.Text = string.Empty;
                            this.txttpeuserMR.DisplayBox2.Text = string.Empty;
                            this.txttpeuserSMR.DisplayBox1.Text = string.Empty;
                            this.txttpeuserSMR.DisplayBox2.Text = string.Empty;
                            this.txttpeuserTask.DisplayBox1.Text = string.Empty;
                            this.txttpeuserTask.DisplayBox2.Text = string.Empty;
                            this.txtSpNo.Text = string.Empty;

                            string strCheckOrderIDCanAirPP = $@"
select Id
       , Seq 
from Order_QtyShip WITH (NOLOCK) 
where Id = '{orderID}'
        and 
        ShipmodeID in (select ID 
                        from ShipMode WITH (NOLOCK) 
                        where UseFunction like '%AirPP%')";

                            if (MyUtility.Check.Seek(strCheckOrderIDCanAirPP))
                            {
                                MyUtility.Msg.InfoBox(string.Format("SP#:{0} The Air Pre-Paid is already created.", orderID));
                            }
                            else
                            {
                                MyUtility.Msg.InfoBox("< SP No. > not found!");
                            }

                            return;
                        }

                        if (orderQtyData.Rows.Count == 1)
                        {
                            this.CurrentMaintain["OrderShipmodeSeq"] = orderQtyData.Rows[0]["Seq"];
                            this.displayShipMode.Value = MyUtility.Convert.GetString(orderQtyData.Rows[0]["ShipmodeID"]);
                            this.numOrderQty.Value = MyUtility.Convert.GetInt(orderQtyData.Rows[0]["Qty"]);
                            this.CurrentMaintain["ShipQty"] = MyUtility.Convert.GetInt(orderQtyData.Rows[0]["Qty"]);
                            this.dateBuyerDelivery.Value = Convert.ToDateTime(orderQtyData.Rows[0]["BuyerDelivery"]);
                            if (string.Compare(orderData["Category"].ToString(), "S", true) != 0)
                            {
                                DataRow drPacking;
                                string sqlcmd = $@"
select GW = ISNULL(sum(gw),0),APPEstAmtVW = ISNULL(sum(APPEstAmtVW),0)  
from PackingList_Detail
where OrderID = '{orderID}' and OrderShipmodeSeq = '{orderQtyData.Rows[0]["Seq"]}'
";
                                if (MyUtility.Check.Seek(sqlcmd, out drPacking))
                                {
                                    this.CurrentMaintain["GW"] = drPacking["GW"];
                                    this.CurrentMaintain["VW"] = drPacking["APPEstAmtVW"];
                                }
                            }
                        }
                        else
                        {
                            IList<DataRow> orderQtyShipData;
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(orderQtyData, "Seq,BuyerDelivery,ShipModeID,Qty", "4,20,20,10", string.Empty, false, string.Empty, "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.CurrentMaintain["OrderShipmodeSeq"] = string.Empty;
                                this.displayShipMode.Value = string.Empty;
                                this.numOrderQty.Value = 0;
                                this.CurrentMaintain["ShipQty"] = 0;
                                this.dateCreatedate.Value = null;
                            }
                            else
                            {
                                orderQtyShipData = item.GetSelecteds();
                                this.CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                                this.displayShipMode.Value = MyUtility.Convert.GetString(orderQtyShipData[0]["ShipmodeID"]);
                                this.numOrderQty.Value = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"]);
                                this.CurrentMaintain["ShipQty"] = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"]);
                                this.dateBuyerDelivery.Value = Convert.ToDateTime(orderQtyShipData[0]["BuyerDelivery"]);

                                if (string.Compare(orderData["Category"].ToString(), "S", true) != 0)
                                {
                                    DataRow drPacking;
                                    string sqlcmd = $@"
select GW = ISNULL(sum(gw),0),APPEstAmtVW = ISNULL(sum(APPEstAmtVW),0)  
from PackingList_Detail
where OrderID = '{orderID}' and OrderShipmodeSeq = '{item.GetSelectedString()}'
";
                                    if (MyUtility.Check.Seek(sqlcmd, out drPacking))
                                    {
                                        this.CurrentMaintain["GW"] = drPacking["GW"];
                                        this.CurrentMaintain["VW"] = drPacking["APPEstAmtVW"];
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        this.CurrentMaintain["OrderShipmodeSeq"] = string.Empty;
                        this.displayShipMode.Value = string.Empty;
                        this.numOrderQty.Value = 0;
                        this.CurrentMaintain["ShipQty"] = 0;
                        this.dateCreatedate.Value = null;
                        MyUtility.Msg.ErrorBox("Query Seq fail.\r\n" + result.ToString());
                    }
                    #endregion
                }
            }
        }

        private void TxtSpNo_Validated(object sender, EventArgs e)
        {
            if (this.txtSpNo.OldValue == this.txtSpNo.Text)
            {
                return;
            }

            // OrderID異動，其他相關欄位要跟著異動
            this.ChangeOtherData(this.txtSpNo.Text);
            this.ChangeQuotationAVG();
        }

        // Seq按右鍵
        private void TxtSeq_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format(
                @"select oq.Seq,oq.BuyerDelivery,oq.ShipmodeID,oq.Qty from Order_QtyShip oq WITH (NOLOCK) ,(
select Id,Seq from Order_QtyShip WITH (NOLOCK) where Id = '{0}' and 
ShipmodeID in (select ID from ShipMode WITH (NOLOCK) where UseFunction like '%AirPP%')
except
select OrderID as ID,OrderShipmodeSeq as Seq from AirPP WITH (NOLOCK) where OrderID = '{0}' and ID != '{1}' and Status <> 'Junked') b
where oq.Id = b.Id and oq.Seq = b.Seq",
                MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]) ? string.Empty : this.CurrentMaintain["OrderID"],
                MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? string.Empty : this.CurrentMaintain["ID"].ToString());

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", string.Empty, "Seq,Buyer Delivery,ShipMode,Qty");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                this.CurrentMaintain["OrderShipmodeSeq"] = string.Empty;
                this.CurrentMaintain["ShipModeID"] = string.Empty;
                this.numOrderQty.Value = 0;
                this.CurrentMaintain["ShipQty"] = 0;
                this.dateCreatedate.Value = null;
            }
            else
            {
                IList<DataRow> orderQtyShipData;
                orderQtyShipData = item.GetSelecteds();
                this.CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                this.displayShipMode.Value = orderQtyShipData[0]["ShipmodeID"].ToString();
                this.numOrderQty.Value = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"]);
                this.CurrentMaintain["ShipQty"] = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"]);
                if (MyUtility.Check.Empty(orderQtyShipData[0]["BuyerDelivery"]))
                {
                    this.dateBuyerDelivery.Value = null;
                }
                else
                {
                    this.dateBuyerDelivery.Value = Convert.ToDateTime(orderQtyShipData[0]["BuyerDelivery"]);
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickJunk()
        {
            if (!Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["AddName"])))
            {
                MyUtility.Msg.WarningBox("Sorry, you don't have permission to junk this data. ");
                return;
            }

            // 問是否要做Junk，確定才繼續往下做
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Junk > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Air_Prepaid_unApprove");
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                string insertCmd = string.Format(
                    @"insert into AirPP_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','Status','New','Junked','{1}','{2}','{3}',GetDate())",
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                    callReason.ReturnReason,
                    callReason.ReturnRemark,
                    Sci.Env.User.UserID);

                string updateCmd = string.Format(@"update AirPP set Status = 'Junked', EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

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
                        this.ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }

            this.SendMail(true);
        }

        /// <inheritdoc/>
        protected override void ClickCheck()
        {
            if (!Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["PPICMgr"])))
            {
                MyUtility.Msg.WarningBox("Sorry, you don't have permission to check this data. ");
                return;
            }

            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipQty"]))
            {
                MyUtility.Msg.WarningBox("Air Q'ty can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["GW"]))
            {
                MyUtility.Msg.WarningBox("Gross Weight(Kgs) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["VW"]))
            {
                MyUtility.Msg.WarningBox("V.Weight(Kgs) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Rate"]))
            {
                MyUtility.Msg.WarningBox("Exchange Rate can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Forwarder"]))
            {
                MyUtility.Msg.WarningBox("Forwarder(N) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Forwarder1"]))
            {
                MyUtility.Msg.WarningBox("Forwarder(1) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Forwarder2"]))
            {
                MyUtility.Msg.WarningBox("Forwarder(2) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Quotation"]) || MyUtility.Check.Empty(this.CurrentMaintain["Quotation1"]) || MyUtility.Check.Empty(this.CurrentMaintain["Quotation1"]))
            {
                MyUtility.Msg.WarningBox("Quotation(USD/Kgs) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["EstAmount"]))
            {
                MyUtility.Msg.WarningBox("Est. Amt(USD) can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ReasonID"]))
            {
                MyUtility.Msg.WarningBox("Responsibility Justification can't empty!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FtyDesc"]))
            {
                MyUtility.Msg.WarningBox("Explanation can't empty!!");
                return;
            }

            // Air Qty要等於Order Qty
            if (MyUtility.Convert.GetInt(this.CurrentMaintain["ShipQty"]) > MyUtility.Convert.GetInt(this.numOrderQty.Value))
            {
                MyUtility.Msg.WarningBox("Air Q'ty<" + this.CurrentMaintain["ShipQty"] + "> cannot greater than Order Q'ty<" + this.numOrderQty.Value + ">!!");
                return;
            }
            #endregion
            string insertCmd = string.Format(
                @"insert into AirPP_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','Status','New','Checked','{1}',GetDate())",
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                Sci.Env.User.UserID);

            string updateCmd = string.Format(@"update AirPP set Status = 'Checked',PPICMgr = '{0}',PPICMgrApvDate = GetDate(), EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

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
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickUncheck()
        {
            if (!Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["PPICMgr"])))
            {
                MyUtility.Msg.WarningBox("Sorry, you don't have permission to uncheck this data. ");
                return;
            }

            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Air_Prepaid_unApprove");
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                string insertCmd = string.Format(
                    @"insert into AirPP_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','Status','Checked','New','{1}','{2}','{3}',GetDate())",
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                    callReason.ReturnReason,
                    callReason.ReturnRemark,
                    Sci.Env.User.UserID);

                string updateCmd = string.Format(@"update AirPP set Status = 'New', PPICMgr = '',PPICMgrApvDate = null, EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

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
                        this.ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            if (!Prgs.GetAuthority(MyUtility.Convert.GetString(this.CurrentMaintain["FtyMgr"])))
            {
                MyUtility.Msg.WarningBox("Sorry, you don't have permission to check this data. ");
                return;
            }

            // Air Qty要等於Order Qty
            if (MyUtility.Convert.GetInt(this.CurrentMaintain["ShipQty"]) > MyUtility.Convert.GetInt(this.numOrderQty.Value))
            {
                MyUtility.Msg.WarningBox("Air Q'ty<" + this.CurrentMaintain["ShipQty"] + "> cannot greater than Order Q'ty<" + this.numOrderQty.Value + ">!!");
                return;
            }

            string insertCmd = string.Format(
                @"insert into AirPP_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','Status','Checked','Approved','{1}',GetDate())",
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                Sci.Env.User.UserID);

            string updateCmd = string.Format(@"update AirPP set Status = 'Approved', FtyMgr = '{0}', FtyMgrApvDate = GetDate(), EditName = '{0}', EditDate = GetDate() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

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
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            this.SendMail(true);
        }

        // Status update history
        private void BtnStatusUpdateHistory_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("AirPP_History", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "Status", reasonType: "Air_Prepaid_unApprove", caption: "Status Update History");
            callNextForm.ShowDialog(this);
        }

        // Mail To
        private void BtnMailto_Click(object sender, EventArgs e)
        {
            this.SendMail(false);
        }

        // 寄Mail
        private void SendMail(bool visibleForm)
        {
            DataRow dr;
            if (MyUtility.Check.Seek("select * from MailTo WITH (NOLOCK) where ID = '008'", out dr))
            {
                DataTable allMail;
                string sqlCmd = string.Format(
                    @"select isnull((select EMail from Pass1 WITH (NOLOCK) where ID = a.PPICMgr),'') as PPICMgrMail,
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
where a.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out allMail);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Query mail list fail.\r\n" + result.ToString());
                    return;
                }

                string mailto = MyUtility.Convert.GetString(allMail.Rows[0]["POSMRMail"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["POHandleMail"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["MRHandleMail"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["SMRMail"]) + ";";
                string cc = MyUtility.Convert.GetString(allMail.Rows[0]["PPICMgrMail"]) + ";" + MyUtility.Convert.GetString(allMail.Rows[0]["FtyMgrMail"]) + ";" + MyUtility.Convert.GetString(dr["ToAddress"]);
                string subject = string.Format(
                    @"<{0}> {1} for SP#{2}, DD{3} - {4}",
                    MyUtility.Convert.GetString(this.displayFactory.Value),
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]),
                    Convert.ToDateTime(this.CurrentMaintain["CDate"]).ToString("yyyyMMdd"),
                    MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Junked" ? "cancel" : "request");

                StringBuilder content = new StringBuilder();
                #region 組Content
                content.Append(string.Format(
                    @"Hi MR team, cc.Production team/GM Team
Please refer to attachment – air pp request and refer to below datas.

SMR: {0} Ext.{1}, POSMR: {2} Ext.{3}
{4} - {5} for SP - {6} buyer del: {7} 
Air q’ty: {8}
Responsibility: {9}{10}{11}{12}{13}
-Be remind!! ---
If the responsibility is belong to the supplier or SCI-MR team (posmr team), please key in Debit Note and ICR#, tks!
If the responsibility is belong to “Buyer”, please remark the reason, tks!
Remind:Please return the air pp request – approved  within 24hrs to avoid any shipment delay.",
                    MyUtility.Convert.GetString(allMail.Rows[0]["SMRName"]),
                    MyUtility.Convert.GetString(allMail.Rows[0]["SMRExtNo"]),
                    MyUtility.Convert.GetString(allMail.Rows[0]["POSMRName"]),
                    MyUtility.Convert.GetString(allMail.Rows[0]["POSMRExtNo"]),
                    MyUtility.Convert.GetString(this.displayFactory.Value),
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]),
                    MyUtility.Check.Empty(this.dateBuyerDelivery.Value) ? string.Empty : Convert.ToDateTime(this.dateBuyerDelivery.Value).ToString("yyyyMMdd"),
                    MyUtility.Convert.GetString(this.CurrentMaintain["ShipQty"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleFty"]) == "True" ? "Factory:" + MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleFtyNo"]) + "\r\n" : string.Empty,
                    MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleSubcon"]) == "True" ? "Subcon:DBC #:" + MyUtility.Convert.GetString(this.CurrentMaintain["SubconDBCNo"]) + "\r\n" : string.Empty,
                    MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleSCI"]) == "True" ? "SCI:ICR #:" + MyUtility.Convert.GetString(this.CurrentMaintain["SCIICRNo"]) + "\r\n" : string.Empty,
                    MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleSupp"]) == "True" ? "Supplier:DBC #:" + MyUtility.Convert.GetString(this.CurrentMaintain["SuppDBCNo"]) + "\r\n" : string.Empty,
                    MyUtility.Convert.GetString(this.CurrentMaintain["ResponsibleBuyer"]) == "True" ? "Buyer:Debit Memo:" + MyUtility.Convert.GetString(this.CurrentMaintain["BuyerDBCNo"]) + ", ICR #" + MyUtility.Convert.GetString(this.CurrentMaintain["BuyerICRNo"]) + "\r\n" : string.Empty));
                #endregion

                // 產生Excel
                this.ToExcel(true);

                var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, cc, subject, this.excelFile, content.ToString(), visibleForm, visibleForm);

                email.ShowDialog(this);

                // 刪除Excel File
                if (System.IO.File.Exists(this.excelFile))
                {
                    try
                    {
                        System.IO.File.Delete(this.excelFile);
                    }
                    catch (System.IO.IOException e)
                    {
                        MyUtility.Msg.WarningBox(string.Format("Delete excel file fail!! {0}", e));
                    }
                }
            }
        }

        // AirPP List
        private void BtnAirPPList_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P01_AirPPList callNextForm = new Sci.Production.Shipping.P01_AirPPList(MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]));
            callNextForm.ShowDialog(this);
        }

        // Q'ty B'down by Shipmode
        private void BtnQtyBDownByShipmode_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyShip callNextForm = new Sci.Production.PPIC.P01_QtyShip(MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]), MyUtility.GetValue.Lookup(string.Format("select POID from Orders WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]))));
            callNextForm.ShowDialog(this);
        }

        // Q'ty B'down by Order
        private void BtnQtyBdownbyOrder_Click(object sender, EventArgs e)
        {
            string sqlCmd = string.Format(
                @"select o.POID,isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList
from Orders o WITH (NOLOCK) 
where o.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]));
            DataTable orderData;
            DBProxy.Current.Select(null, sqlCmd, out orderData);
            int count = orderData.Rows.Count;

            Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]), (count == 0) ? string.Empty : MyUtility.Convert.GetString(orderData.Rows[0]["POID"]), (count == 0) ? string.Empty : MyUtility.Convert.GetString(orderData.Rows[0]["PoList"]));
            callNextForm.ShowDialog(this);
        }

        // GMT Export
        private void BtnGMTExport_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_GMTExport callNextForm = new Sci.Production.PPIC.P01_GMTExport(MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]));
            callNextForm.ShowDialog(this);
        }

        private void TxtResponsibilityJustifcation_Validating(object sender, CancelEventArgs e)
        {
            string str = this.txtResponsibilityJustifcation.Text;
            DataRow dr;
            if (!MyUtility.Check.Empty(this.txtResponsibilityJustifcation.Text))
            {
                string cmd = string.Format(@"select ID,Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Air_Prepaid_Reason' and Junk = 0 and id='{0}' order by ID", this.txtResponsibilityJustifcation.Text);
                if (!MyUtility.Check.Seek(cmd, out dr))
                {
                    e.Cancel = true;
                    this.txtResponsibilityJustifcation.Text = string.Empty;
                    this.displayResponsibilityJustifcation.Value = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("{0}, Data not Found!", str));
                    return;
                }
                else
                {
                    this.displayResponsibilityJustifcation.Value = MyUtility.Convert.GetString(dr["Name"]);
                }
            }
            else
            {
                this.displayResponsibilityJustifcation.Value = string.Empty;
            }
        }

        private void ChangeQuotationAVG()
        {
            string strSqlCmd = $@"
select [QuotationAVG] = ISNULL(iIf(sum(a.CW)=0 , 0, convert(float, ROUND(sum(a.ActualAmount) / sum(a.CW) ,2))),0)
from AirPP a
left join orders o on a.OrderID = o.ID
where DATEPART(YEAR,a.CDate) = DATEPART(year, DATEADD(year,-1,getdate()))
and BrandID = '{this.displayBrand.Text}' 
and Dest = '{this.txtCountryDestination.TextBox1.Text}' 
and Forwarder = '{this.txtSubconForwarderN.TextBox1.Text}'";

            DataRow dr;

            if (MyUtility.Check.Seek(strSqlCmd, out dr))
            {
                this.CurrentMaintain["QuotationAVG"] = MyUtility.Check.Empty(dr["QuotationAVG"]) ? 0.00 : dr["QuotationAVG"];
            }
        }

        private void txtSubconForwarderN_Validated(object sender, EventArgs e)
        {
            if (this.txtSubconForwarderN.TextBox1.Text != this.txtSubconForwarderN.TextBox1.OldValue)
            {
                this.ChangeQuotationAVG();
            }
        }

        private void numVWeight_ValueChanged(object sender, EventArgs e)
        {
            this.numVWeightOldValue = this.numVWeight.OldValue.HasValue ? this.numVWeight.OldValue.Value : 0 ;
        }

        private void numForwarderNQuotation_VisibleChanged(object sender, EventArgs e)
        {
            
            this.numForwarderNQuotationOldValue = this.numForwarderNQuotation.OldValue.HasValue ? this.numForwarderNQuotation.OldValue.Value : 0;

        }

        private void detailcont_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

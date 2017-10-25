using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;
using Sci.Production.Class.Commons;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using MsExcel = Microsoft.Office.Interop.Excel;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using Sci.Utility.Excel;
using Sci.Utility.Drawing;

using System.Runtime.InteropServices;
using Sci.Production.PublicForm;
using System.IO;
using System.Linq;

namespace Sci.Production.PPIC
{
    public partial class P01 : Sci.Win.Tems.Input1
    {        
        private string dataType;
        public P01(ToolStripMenuItem menuitem, string Type)
            : base(menuitem)
        {
            InitializeComponent();
            this.IsSupportNew = Type == "1" ? true : false;
            this.IsSupportEdit = Type == "1" ? true : false;

            this.Text = Type == "1" ? "P01. PPIC Master List" : "P011. PPIC Master List (History)";
            this.DefaultFilter = Type == "1" ? string.Format("MDivisionID = '{0}' AND Finished = 0", Sci.Env.User.Keyword) : string.Format("MDivisionID = '{0}' AND Finished = 1", Sci.Env.User.Keyword);
            dataType = Type;
            btnShipmentFinished.Visible = dataType == "1"; //Shipment Finished
            btnBacktoPPICMasterList.Visible = dataType != "1"; //Back to P01. PPIC Master List
         
        }
     
            
        
        protected override void OnDetailDetached()
        {
            base.OnDetailDetached();
            ControlButton();
        }
     
        //按鈕控制
        private void ControlButton()
        {
            btnMCHandleCFM.Enabled = CurrentMaintain != null;
            btnLocalMRCFM.Enabled = CurrentMaintain != null;
            btnProductionOutput.Enabled = CurrentMaintain != null;
            btnOrderRemark.Enabled = CurrentMaintain != null;
            btnFactoryCMT.Enabled = CurrentMaintain != null;
            btnLabelHangtag.Enabled = CurrentMaintain != null;
            btnQtyBdownByShipmode.Enabled = CurrentMaintain != null;
            btnQuantityBreakdown.Enabled = CurrentMaintain != null;
            btnShippingMark.Enabled = CurrentMaintain != null;
            btnTMSCost.Enabled = CurrentMaintain != null;
            btnStdGSDList.Enabled = CurrentMaintain != null;
            btnCMPQRemark.Enabled = CurrentMaintain != null;
            btnCMPQSheet.Enabled = CurrentMaintain != null;
            btnArtwork.Enabled = CurrentMaintain != null;
            btnGarmentExport.Enabled = CurrentMaintain != null;
            btnH.Enabled = CurrentMaintain != null;
            btnCuttingCombo.Enabled = CurrentMaintain != null;
            btnbdown.Enabled = CurrentMaintain != null;
            btnMaterialImport.Enabled = CurrentMaintain != null;
            btnFabricInspectionList.Enabled = CurrentMaintain != null;
            btnAccessoryInspectionList.Enabled = CurrentMaintain != null;
            btnArtworkTransactionList.Enabled = CurrentMaintain != null;
            btnProductionKits.Enabled = CurrentMaintain != null;
            btnMNoticeSheet.Enabled = CurrentMaintain != null;
            btnQtyBdownbySchedule.Enabled = CurrentMaintain != null;
            btnCartonStatus.Enabled = CurrentMaintain != null;
            btnPackingMethod.Enabled = CurrentMaintain != null;
            btnPullForwardRemark.Enabled = CurrentMaintain != null;
            btnShipmentFinished.Enabled = CurrentMaintain != null;
            btnVASSHASInstruction.Enabled = CurrentMaintain != null;
            btnBacktoPPICMasterList.Enabled = CurrentMaintain != null;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //新增Batch Shipment Finished按鈕
            Sci.Win.UI.Button btn = new Sci.Win.UI.Button();
            btn.Text = "Batch Shipment Finished";
            btn.Click += new EventHandler(btn_Click);
            browsetop.Controls.Add(btn);
            btn.Size = new Size(180, 30);//預設是(80,30)
            btn.Visible = dataType == "1";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (!EditMode)
            {
                ControlButton();
            }
            displaySampleReason2.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Order_reMakeSample' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["SampleReason"])));
            displayUpdateDeliveryReason.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Order_BuyerDelivery' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["KPIChangeReason"])));
            displaySpecialMark.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["SpecialMark"])));
            numCPUAmt.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["CPU"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["CPUFactor"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["Qty"]), 3);
            displayMTLCmpltSP.Value = MyUtility.Convert.GetString(CurrentMaintain["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : "";
            displayOutstandingReason2.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Delivery_OutStand' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["OutstandingReason"])));
            displayFinalUpdateOutstandingReasondate.Value = MyUtility.Check.Empty(CurrentMaintain["OutstandingDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["OutstandingDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                numCMPQPrice.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["PoPrice"]);
                label44.Text = "/PCS";
                numCMPQAmt.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["PoPrice"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["Qty"]), 3);
                tooltip.SetToolTip(numCMPQAmt, MyUtility.Convert.GetString(CurrentMaintain["PoPrice"]) + " * " + MyUtility.Convert.GetString(CurrentMaintain["Qty"]));
            }
            else
            {
                numCMPQPrice.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["CMPPrice"]);
                label44.Text = "/" + MyUtility.Convert.GetString(CurrentMaintain["CMPUnit"]);
                numCMPQAmt.Value = numCPUAmt.Value;
                tooltip.SetToolTip(numCMPQAmt, MyUtility.Convert.GetString(CurrentMaintain["CPU"]) + " * " + MyUtility.Convert.GetString(CurrentMaintain["CPUFactor"]) + " * " + MyUtility.Convert.GetString(CurrentMaintain["Qty"]));
            }
            #region 填Description, Exception Form, Fty Remark, Style Apv欄位值
            DataRow StyleData;
            string sqlCmd = string.Format("select Description,ExpectionForm,FTYRemark,ApvDate from Style WITH (NOLOCK) where Ukey = {0}", MyUtility.Convert.GetString(CurrentMaintain["StyleUkey"]));
            if (MyUtility.Check.Seek(sqlCmd, out StyleData))
            {
                displayDescription.Value = MyUtility.Convert.GetString(StyleData["Description"]);
                checkExceptionForm.Value = MyUtility.Convert.GetString(StyleData["ExpectionForm"]);
                editFtyRemark.Text = MyUtility.Convert.GetString(StyleData["FTYRemark"]);
                if (MyUtility.Check.Empty(StyleData["ApvDate"]))
                {
                    dateStyleApv.Value = null;
                }
                else
                {
                    dateStyleApv.Value = MyUtility.Convert.GetDate(StyleData["ApvDate"]);
                }
            }
            else
            {
                displayDescription.Value = "";
                checkExceptionForm.Value = "false";
                editFtyRemark.Text = "";
                dateStyleApv.Value = null;
            }
            #endregion
            #region 填Buyer欄位值, 修改Special id1, Special id2, Special id3顯示值
            DataRow brandData;
            if (MyUtility.Check.Seek(string.Format("select ID,Customize1,Customize2,Customize3,BuyerID from Brand WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["BrandID"])), out brandData))
            {
                displayBuyer.Value = MyUtility.Convert.GetString(brandData["BuyerID"]);
                labelSpecialId1.Text = MyUtility.Convert.GetString(brandData["Customize1"]);
                labelSpecialId2.Text = MyUtility.Convert.GetString(brandData["Customize2"]);
                labelSpecialId3.Text = MyUtility.Convert.GetString(brandData["Customize3"]);
            }
            else
            {
                displayBuyer.Value = "";
                labelSpecialId1.Text = "";
                labelSpecialId2.Text = "";
                labelSpecialId3.Text = "";
            }
            #endregion
            #region 填PO SMR, PO Handle欄位值
            DataRow POData;
            sqlCmd = string.Format("select POSMR,POHandle from PO WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            if (MyUtility.Check.Seek(sqlCmd, out POData))
            {
                txttpeuser3.DisplayBox1Binding = MyUtility.Convert.GetString(POData["POSMR"]);
                txttpeuser4.DisplayBox1Binding = MyUtility.Convert.GetString(POData["POHandle"]);
            }
            else
            {
                txttpeuser3.DisplayBox1Binding = "";
                txttpeuser4.DisplayBox1Binding = "";
            }
            #endregion
            #region 填PO Combo, Cutting Combo, MTLExport, PulloutComplete, Garment L/T, OrderCombo 欄位值
            System.Data.DataTable OrdersData;
            sqlCmd = string.Format(@"select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList,
isnull([dbo].getCuttingComboList(o.ID,o.CuttingSP),'') as CuttingList,
isnull([dbo].getMTLExport(o.POID,o.MTLExport),'') as MTLExport,
isnull([dbo].getPulloutComplete(o.ID,o.PulloutComplete),'') as PulloutComplete,
isnull([dbo].getGarmentLT(o.StyleUkey,o.FactoryID),0) as GMTLT from Orders o WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null,sqlCmd, out OrdersData);
            if (result)
            {
                if (OrdersData.Rows.Count > 0)
                {
                    editPOCombo.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["PoList"]);
                    editCuttingCombo.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["CuttingList"]);
                    displayRMTLETAMasterSP.Value = MyUtility.Convert.GetString(OrdersData.Rows[0]["MTLExport"]);
                    displayActPullout.Value = MyUtility.Convert.GetString(OrdersData.Rows[0]["PulloutComplete"]);
                    numGarmentLT.Value = MyUtility.Convert.GetDecimal(OrdersData.Rows[0]["GMTLT"]);
                }
                else
                {
                    editPOCombo.Text = "";
                    editCuttingCombo.Text = "";
                    displayRMTLETAMasterSP.Value = "";
                    displayActPullout.Value = "";
                    numGarmentLT.Value = 0;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query OrdersData fail!!" + result.ToString());
                editPOCombo.Text = "";
                editCuttingCombo.Text = "";
                displayRMTLETAMasterSP.Value = "";
                displayActPullout.Value = "";
                numGarmentLT.Value = 0;
            }

            displayOrderCombo.Value = MyUtility.GetValue.Lookup(string.Format("Select Top 1 OrderComboList from dbo.Order_OrderComboList with(nolock) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            #endregion
            bool lConfirm = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P01. PPIC Master List", "CanConfirm");
            btnMCHandleCFM.Enabled = CurrentMaintain != null && dataType == "1" && lConfirm && !EditMode;
            btnLocalMRCFM.Enabled = CurrentMaintain != null && dataType == "1" && lConfirm && !EditMode;
            btnbdown.Enabled = CurrentMaintain != null && MyUtility.Convert.GetString(CurrentMaintain["CtnType"]) == "2" && !EditMode;

            //按鈕變色
            bool haveTmsCost = MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            btnProductionOutput.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            btnOrderRemark.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;
            btnFactoryCMT.ForeColor = haveTmsCost ? Color.Blue : Color.Black;
            btnLabelHangtag.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Label"]) ? Color.Blue : Color.Black;
            btnQtyBdownByShipmode.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            btnQuantityBreakdown.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            btnShippingMark.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["MarkFront"]) || !MyUtility.Check.Empty(CurrentMaintain["MarkBack"]) || !MyUtility.Check.Empty(CurrentMaintain["MarkLeft"]) || !MyUtility.Check.Empty(CurrentMaintain["MarkRight"]) ? Color.Blue : Color.Black;
            btnTMSCost.ForeColor = haveTmsCost ? Color.Blue : Color.Black;
            btnStdGSDList.ForeColor = MyUtility.Check.Seek(string.Format("select i.ID from Style s WITH (NOLOCK) , IETMS i WITH (NOLOCK) where s.Ukey = {0} and s.IETMSID = i.ID and s.IETMSVersion = i.Version", MyUtility.Convert.GetString(CurrentMaintain["StyleUkey"]))) && MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            btnCMPQRemark.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["CMPQRemark"]) ? Color.Blue : Color.Black;
            btnCMPQSheet.ForeColor = MyUtility.Check.Seek(string.Format("select ID from orders WITH (NOLOCK) where Junk = 0 and POID='{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;
            btnArtwork.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Artwork WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            btnGarmentExport.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            btnCuttingCombo.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["CuttingSP"]) ? Color.Blue : Color.Black;
            if (MyUtility.Convert.GetString(CurrentMaintain["CtnType"]) == "2")
            {
                btnbdown.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyCTN WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            }
            btnMaterialImport.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Export_Detail WITH (NOLOCK) where PoID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]))) ? (MyUtility.Convert.GetString(CurrentMaintain["POID"]) != "" ? Color.Blue : Color.Black) : Color.Black;
            btnFabricInspectionList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from FIR WITH (NOLOCK) where PoID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]))) ? (MyUtility.Convert.GetString(CurrentMaintain["POID"]) != "" ? Color.Blue : Color.Black) : Color.Black;
            btnAccessoryInspectionList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from AIR WITH (NOLOCK) where PoID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;
            btnArtworkTransactionList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from ArtworkPO_Detail WITH (NOLOCK) where OrderID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            btnProductionKits.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]))) ? Color.Blue : Color.Black;

            #region 控制[m/notice sheet]按鈕是否變色
            bool enableMNotice1 = MyUtility.Check.Seek(string.Format("select ID FROM MnOrder_ColorCombo WITH (NOLOCK) where ID = (select OrderComboID FROM MNOrder where ID = '{1}')", MyUtility.Convert.GetString(CurrentMaintain["POID"]), MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            bool enableMNotice2 = !MyUtility.Check.Empty(CurrentMaintain["SMnorderApv"]);
            bool enableMNotice = enableMNotice1 || enableMNotice2;
            btnMNoticeSheet.ForeColor = enableMNotice ? Color.Blue : Color.Black;
            #endregion

            btnQtyBdownbySchedule.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["SewLine"]) ? Color.Blue : Color.Black;
            btnCartonStatus.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' and ReceiveDate is not null", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            btnPackingMethod.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing"]) ? Color.Blue : Color.Black;
            btnPullForwardRemark.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_PFHis WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            btnVASSHASInstruction.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing2"]) ? Color.Blue : Color.Black;
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "Order_EachCons", "ID")) btnEachCons.ForeColor = Color.Blue;
            else btnEachCons.ForeColor = Color.Black;
            
            //SciDelivery OrigBuyerDelivery
            //CRDDate
            if (!MyUtility.Check.Empty(CurrentMaintain["SciDelivery"]))
                dateDetailsSCIDel.TextForeColor = CurrentMaintain["SciDelivery"].ToString() != CurrentMaintain["BuyerDelivery"].ToString() ? Color.Red : Color.Blue;
            if (!MyUtility.Check.Empty(CurrentMaintain["CRDDate"]))
                dateDetailsCRDdate.TextForeColor = MyUtility.Convert.GetDate(CurrentMaintain["CRDDate"]) < MyUtility.Convert.GetDate(CurrentMaintain["BuyerDelivery"]) ? Color.Red : Color.Blue;
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            txtpaytermar1.TextBox1.ReadOnly = true;
            label44.Text = "/PCS";
            //帶入預設值
            CurrentMaintain["Category"] = "B";
            CurrentMaintain["LocalOrder"] = 1;
            CurrentMaintain["MCHandle"] = Sci.Env.User.UserID;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["FtyGroup"] = Sci.Env.User.Factory;
            CurrentMaintain["CMPUnit"] = "PCS";
            CurrentMaintain["CFMDate"] = DateTime.Today;
            CurrentMaintain["CtnType"] = "1";
            CurrentMaintain["CPUFactor"] = 1;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            txtpaytermar1.TextBox1.ReadOnly = true;
            if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "FALSE")
            {
                //非Local訂單時只能修改FactoryID
                txtProgram.ReadOnly = true;
                txtPONo.ReadOnly = true;
                txtStyle.ReadOnly = true;
                txtModel.ReadOnly = true;
                txtSpecialId1.ReadOnly = true;
                txtSpecialId2.ReadOnly = true;
                txtSpecialId3.ReadOnly = true;
                checkSubconInFromSisterFactory.ReadOnly = true;
                checkCancelledOrder.ReadOnly = true;
                checkFOC.ReadOnly = true;
                checkSP.ReadOnly = true;
                checkTissuePaper.ReadOnly = true;
                checkRainwearTestPassed.ReadOnly = true;
                txtuser1.TextBox1.ReadOnly = true;
                txtuser2.TextBox1.ReadOnly = true;
                dateBuyerDlv.ReadOnly = true;
                dateOrigBuyerDlv.ReadOnly = true;
                dateSCIDlv.ReadOnly = true;
                dateCutOffDate.ReadOnly = true;
                txtcountry1.TextBox1.ReadOnly = true;
                txtcurrency1.ReadOnly = true;
                numUnitPrice.ReadOnly = true;
                numQtyCarton.ReadOnly = true;
            }
        }

        protected override bool ClickSaveBefore()
        {

            if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                #region 檢查必輸欄位
                if (MyUtility.Check.Empty(CurrentMaintain["StyleID"]))
                {
                    MyUtility.Msg.WarningBox("Style# can't empty!!");
                    txtStyle.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
                {
                    MyUtility.Msg.WarningBox("Brand can't empty!!");
                    displayBrand.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["CustPONo"]))
                {
                    MyUtility.Msg.WarningBox("PO No. can't empty!!");
                    txtPONo.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["MCHandle"]))
                {
                    MyUtility.Msg.WarningBox("MC Handle can't empty!!");
                    txtuser1.TextBox1.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["StyleUnit"]))
                {
                    MyUtility.Msg.WarningBox("Unit can't empty!!");
                    displayUnit.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["BuyerDelivery"]))
                {
                    MyUtility.Msg.WarningBox("Buyer Delivery can't empty!!");
                    dateBuyerDlv.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["SCIDelivery"]))
                {
                    MyUtility.Msg.WarningBox("SCI Delivery# can't empty!!");
                    dateSCIDlv.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["SDPDate"]))
                {
                    MyUtility.Msg.WarningBox("Cut off date can't empty!!");
                    dateCutOffDate.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["FactoryID"]))
                {
                    MyUtility.Msg.WarningBox("Factory can't empty!!");
                    txtmfactory.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["CurrencyID"]))
                {
                    MyUtility.Msg.WarningBox("Currency can't empty!!");
                    txtcurrency1.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["CPU"]))
                {
                    MyUtility.Msg.WarningBox("CPU can't empty!!");
                    numCPU.Focus();
                    return false;
                }
                if (MyUtility.Convert.GetString(CurrentMaintain["FOC"]).ToUpper() == "FALSE" && MyUtility.Check.Empty(CurrentMaintain["PoPrice"]))
                {
                    MyUtility.Msg.WarningBox("Unit Price can't empty!!");
                    numUnitPrice.Focus();
                    return false;
                }

                if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE" && MyUtility.Check.Empty(CurrentMaintain["ShipModeList"]))
                {
                    MyUtility.Msg.WarningBox("Ship Mode can't empty!!");
                    editShipMode.Focus();
                    return false;
                }
                #endregion

                //檢查是否幫姊妹廠代工
                if (MyUtility.Convert.GetString(CurrentMaintain["SubconInSisterFty"]).ToUpper() == "FALSE")
                {
                    //sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@programid", MyUtility.Convert.GetString(CurrentMaintain["ProgramID"]));

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    System.Data.DataTable SCIFtyData;
                    string sqlCmd = "select ID from SCIFty WITH (NOLOCK) where ID = @programid";
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out SCIFtyData);
                    if (result && SCIFtyData.Rows.Count > 0)
                    {
                        CurrentMaintain["SubconInSisterFty"] = 1;
                    }
                }

                string strUpd_QtyShip_BuyerDelivery = string.Format(@"
Update oq
set oq.BuyerDelivery = '{0}'
from Order_QtyShip oq WITH (NOLOCK) 
where oq.Id = '{1}'", Convert.ToDateTime(CurrentMaintain["BuyerDelivery"]).ToString("yyyy/MM/dd")
                    , MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                DualResult resultUpd = DBProxy.Current.Execute(null, strUpd_QtyShip_BuyerDelivery);
                if (resultUpd == false)
                {
                    MyUtility.Msg.WarningBox(resultUpd.Description);
                    return false;
                }
            }

            //GetID
            if (IsDetailInserting)
            {
                string id = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Factory + "LO", "Orders", DateTime.Today);
                //string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", CurrentMaintain["ID"].ToString(), "Orders", "ID") + "LO", "Orders", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
                CurrentMaintain["POID"] = id;
                CurrentMaintain["CuttingSP"] = id;
            }
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                string insertCmd;
                DualResult result;
                if (!MyUtility.Check.Seek(string.Format("select ID from Order_Artwork WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
                {
                    insertCmd = string.Format(@"
insert into Order_Artwork(ID,ArtworkTypeID,Article,PatternCode,PatternDesc,ArtworkID,ArtworkName,TMS,Qty,Price,Cost,Remark,AddName,AddDate,Ukey)
select ID,ArtworkTypeID,Article,PatternCode,PatternDesc,ArtworkID,ArtworkName,TMS,Qty,Price,Cost,Remark,loginID,today,rownumber-row
from (
select '{0}'as ID,ArtworkTypeID,Article,PatternCode,PatternDesc,ArtworkID,ArtworkName,TMS,Qty,Price,Cost,Remark,'{1}' as 'loginID',GETDATE() as 'today'
,(select min(Ukey) from Order_Artwork)as 'rownumber', Row_Number() OVER( order by PatternCode ) as 'row'
from Style_Artwork where StyleUkey = {2}
) x
",
    MyUtility.Convert.GetString(CurrentMaintain["ID"]), Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["StyleUkey"]));

                    result = DBProxy.Current.Execute(null, insertCmd);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Save Order_Artwork fail!!\r\n" + result.ToString());
                        return failResult;
                    }
                }

                if (!MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
                {
                    insertCmd = string.Format(@"insert into Order_TmsCost(ID,ArtworkTypeID,Seq,Qty,ArtworkUnit,TMS,Price,AddName,AddDate)
select '{0}',ArtworkTypeID,Seq,Qty,ArtworkUnit,TMS,Price,'{1}',GETDATE() from Style_TmsCost where StyleUkey = {2}",
    MyUtility.Convert.GetString(CurrentMaintain["ID"]), Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["StyleUkey"]));

                    result = DBProxy.Current.Execute(null, insertCmd);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Save Order_TmsCost fail!!\r\n" + result.ToString());
                        return failResult;
                    }
                }
                if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE" && MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
                {
                    string updateCmd = string.Format("update Order_QtyShip set ShipModeID = '{0}' where ID = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["ShipModeList"]), MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                    result = DBProxy.Current.Execute(null, updateCmd);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Save Order_QtyShip fail!!\r\n" + result.ToString());
                        return failResult;
                    }
                }

            }
            return Result.True;
        }

        //Style
        bool chkpopup = false;
        private void txtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            IList<DataRow> StyleData;
            string sqlCmd = "select ID,SeasonID,BrandID,Description,CdCodeID,CPU,StyleUnit,Ukey from Style WITH (NOLOCK) where Junk = 0 ";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "15,8,10,28,5,7,7,6", "", "Style,Season,Brand,Description,CdCode,CPU,Unit,Ukey",columndecimals:"0,0,0,0,0,3,0,0");
            item.Size = new System.Drawing.Size(950, 500);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                setStyleEmptyColumn();
                chkpopup = false;
            }
            else
            {
                StyleData = item.GetSelecteds();
                CurrentMaintain["StyleID"] = item.GetSelectedString();
                CurrentMaintain["BrandID"] = StyleData[0]["BrandID"];
                CurrentMaintain["SeasonID"] = StyleData[0]["SeasonID"];
                CurrentMaintain["CdCodeID"] = StyleData[0]["CdCodeID"];
                CurrentMaintain["CPU"] = StyleData[0]["CPU"];
                CurrentMaintain["StyleUnit"] = StyleData[0]["StyleUnit"];
                CurrentMaintain["StyleUkey"] = StyleData[0]["Ukey"];
                displayDescription.Value = MyUtility.Convert.GetString(StyleData[0]["Description"]);
                chkpopup = true;
            }
        }

        //Style
        private void txtStyle_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && txtStyle.OldValue != txtStyle.Text)
            {
                if (MyUtility.Check.Empty(txtStyle.Text))
                {
                    setStyleEmptyColumn();
                }
                else
                {
                    if (!chkpopup)
                    {
                        //檢查資料是否存在
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@styleid", txtStyle.Text);

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);

                        System.Data.DataTable StyleData;
                        string sqlCmd = "select ID,SeasonID,BrandID,Description,CdCodeID,CPU,StyleUnit,Ukey from Style WITH (NOLOCK) where Junk = 0 and ID = @styleid";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out StyleData);
                        if (!result || StyleData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("Style not found!!");
                            }
                            setStyleEmptyColumn();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            CurrentMaintain["StyleID"] = StyleData.Rows[0]["ID"];
                            CurrentMaintain["BrandID"] = StyleData.Rows[0]["BrandID"];
                            CurrentMaintain["SeasonID"] = StyleData.Rows[0]["SeasonID"];
                            CurrentMaintain["CdCodeID"] = StyleData.Rows[0]["CdCodeID"];
                            CurrentMaintain["CPU"] = StyleData.Rows[0]["CPU"];
                            CurrentMaintain["StyleUnit"] = StyleData.Rows[0]["StyleUnit"];
                            CurrentMaintain["StyleUkey"] = StyleData.Rows[0]["Ukey"];
                            displayDescription.Value = MyUtility.Convert.GetString(StyleData.Rows[0]["Description"]);
                        }
                    }                   
                }
            }
            chkpopup = false;
        }

        private void setStyleEmptyColumn()
        {
            CurrentMaintain["StyleID"] = "";
            CurrentMaintain["BrandID"] = "";
            CurrentMaintain["SeasonID"] = "";
            CurrentMaintain["CdCodeID"] = "";
            CurrentMaintain["CPU"] = 0;
            CurrentMaintain["StyleUnit"] = "";
            CurrentMaintain["StyleUkey"] = 0;
            displayDescription.Value = "";
        }

        //Factory
        private void txtmfactory_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtmfactory.OldValue != txtmfactory.Text)
            {
                if (MyUtility.Check.Empty(txtmfactory.Text))
                {
                    CurrentMaintain["FtyGroup"] = "";
                }
                else
                {
                    CurrentMaintain["FtyGroup"] = MyUtility.GetValue.Lookup("FTYGroup",txtmfactory.Text,"Orders","ID");
                }
            }
        }

        //Cancelled Order
        private void checkCancelledOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (EditMode && checkCancelledOrder.Checked && !MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                if (MyUtility.Check.Seek(string.Format("select ID from SewingOutput_Detail WITH (NOLOCK) where OrderId = '{0}' and QAQty > 0", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
                {
                    CurrentMaintain["Junk"] = 0; 
                    MyUtility.Msg.WarningBox("This record had sewing daily output, can't cancel!!");
                   
                }
            }
        }

        //F.O.C.
        private void checkFOC_CheckedChanged(object sender, EventArgs e)
        {
            if (EditMode)
            {
                CurrentMaintain["FOC"] = checkFOC.Checked;
                numUnitPrice.ReadOnly = checkFOC.Checked;
                if (checkFOC.Checked)
                {
                    CurrentMaintain["PoPrice"] = 0;
                }
            }
        }

        //Batch Shipment Finished按鈕的Click事件
        private void btn_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_BatchShipmentFinished callNextForm = new Sci.Production.PPIC.P01_BatchShipmentFinished();
            callNextForm.ShowDialog(this);
            if (callNextForm.haveupdate)
            {
                ReloadDatas();
            }
        }

        //MC Handle CFM
        private void btnMCHandleCFM_Click(object sender, EventArgs e)
        {
            string sqlCmd = string.Format("update Orders set MCHandle = '{0}' where POID = '{1}'",Sci.Env.User.UserID,MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm MC Handle fail!!" + result.ToString());
                return;
            }
            txtuser1.TextBox1.Text = Sci.Env.User.UserID;
        }

        //Local MR CFM
        private void btnLocalMRCFM_Click(object sender, EventArgs e)
        {
            string sqlCmd = string.Format("update Orders set LocalMR = '{0}' where POID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm Local MR fail!!" + result.ToString());
                return;
            }
            txtuser2.TextBox1.Text = Sci.Env.User.UserID;
        }

        //Production output
        private void btnProductionOutput_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput callNextForm = new Sci.Production.PPIC.P01_ProductionOutput(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Order remark
        private void btnOrderRemark_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["OrderRemark"]), "Order Remark", false, null);
            callNextForm.ShowDialog(this);
        }

        //Factory CMT
        private void btnFactoryCMT_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_FactoryCMT callNextForm = new Sci.Production.PPIC.P01_FactoryCMT(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Label & Hangtag
        private void btnLabelHangtag_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["Label"]), "Label & Hangtag", false, null);
            callNextForm.ShowDialog(this);
        }

        //Q'ty b'down by shipmode
        private void btnQtyBdownByShipmode_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyShip callNextForm = new Sci.Production.PPIC.P01_QtyShip(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        //Quantity breakdown
        private void btnQuantityBreakdown_Click(object sender, EventArgs e)
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                Sci.Production.PPIC.P01_QtyLocalOrder callNextForm = new Sci.Production.PPIC.P01_QtyLocalOrder(MyUtility.Convert.GetString(CurrentMaintain["ID"]), dataType == "1" ? true : false, MyUtility.Convert.GetInt(CurrentMaintain["Qty"]));
                callNextForm.ShowDialog(this);
                if (dataType == "1")
                {
                    RenewData();
                }
            }
            else
            {
                Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["POID"]), editPOCombo.Text);
                callNextForm.ShowDialog(this);
            }
        }

        //Shipping mark
        private void btnShippingMark_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ShippingMark callNextForm = new Sci.Production.PPIC.P01_ShippingMark(false, CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //TMS & Cost
        private void btnTMSCost_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_TMSAndCost callNextForm = new Sci.Production.PPIC.P01_TMSAndCost(false, MyUtility.Convert.GetString(CurrentMaintain["ID"]), null, null);
            callNextForm.ShowDialog(this);
        }

        //Std.GSD List
        private void btnStdGSDList_Click(object sender, EventArgs e)
        {
            Sci.Production.PublicForm.StdGSDList callNextForm = new Sci.Production.PublicForm.StdGSDList(MyUtility.Convert.GetLong(CurrentMaintain["StyleUKey"]));
            callNextForm.ShowDialog(this);
        }

        //CMPQ remark
        private void btnCMPQRemark_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["CMPQRemark"]), "CMPQ Remark", false, null);
            callNextForm.ShowDialog(this);
        }
        
        //CMPQ Sheet
        private void btnCMPQSheet_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Data processing, please wait...");
            // string poid = MyUtility.GetValue.Lookup("select POID FROM dbo.Orders where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", _id) });
            string poid = CurrentMaintain["POID"].ToString();
            System.Data.DataTable rpt3;
            DualResult res = DBProxy.Current.Select("", @"
declare @newLine varchar(10) = CHAR(13)+CHAR(10)
Select IIF(fty.CountryID ='TW', 'STARPORT CORPORATION' , 'SPORTS CITY INTERNATIONAL') as Title
    ,'11F, No.585, Ruiguang Rd. Neihu Dist,
    Taipei , Taiwan 11492 ( R.O.C.) 
    Tel: +886 2 8751-0228 Fax: +886 2 8752-4101' as AbbEN --依規格
    ,o.ID
    ,fty.NameEN as name
    ,fty.AddressEN as AddressEN
    ,fty.Tel
    ,fty.Fax
    ,o.CMPQRemark as remark
    ,o.SeasonID
    ,convert(varchar(10),o.BuyerDelivery,111) as delivery
    ,cty.Alias as des
    ,par.Description as terms
    ,o.StyleID
    ,format(o.Qty,'#,0.')+o.StyleUnit as QTY   --Format : 999,999
    ,sty.Description as descripition
    ,fty.CurrencyID+str( o.CMPPrice,5,2)  +'/'+o.CMPUnit as price
    ,amount = concat(fty.CurrencyID,' ',format(round(IIF(O.LocalOrder=1,o.POprice * o.Qty,o.CPU*o.CPUFactor*o.qty),3),'0.000'))
    ,o.packing ,o.label ,o.packing2
    ,Mark=iif(MarkFront<>'','(A) '+@newLine+MarkFront,'')
    +@newLine+iif(MarkBack<>'','(B) '+@newLine+MarkBack,'')
    +@newLine+iif(MarkLeft<>'','(C) '+@newLine+MarkLeft,'')
    +@newLine+iif(MarkRight<>'','(D) '+@newLine+MarkRight,'')
from Orders o WITH (NOLOCK)  inner join Factory fty WITH (NOLOCK)  ON o.FactoryID = fty.ID
LEFT join Country cty WITH (NOLOCK)  ON o.Dest = cty.ID
LEFT JOIN PaytermAR par WITH (NOLOCK)  ON o.PayTermARID = par.ID
LEFT JOIN Style sty WITH (NOLOCK)  ON o.StyleUkey = sty.Ukey
where o.Junk = 0 and o.POID= @POID order by o.ID
", new List<SqlParameter> { new SqlParameter("@ID", CurrentMaintain["ID"]), new SqlParameter("@POID", poid) }, out rpt3);

            if (!res) return;

            string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_CMPQ.xltx");

            sxrc sxr = new sxrc(xltPath, true);
            int idx = 0;
            sxr.CopySheet.Add(1, rpt3.Rows.Count - 1);
            sxr.VarToSheetName = sxr.VPrefix + "SP";
            Microsoft.Office.Interop.Excel.Worksheet wks = sxr.ExcelApp.ActiveSheet;
            foreach (DataRow row in rpt3.Rows)
            {
                string sIdx = idx.ToString();
                idx += 1;
                string oid = row["ID"].ToString();
                sxr.DicDatas.Add(sxr.VPrefix + "Title" + sIdx, row["Title"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "AbbEN" + sIdx, row["AbbEN"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "SP" + sIdx, oid);
                sxr.DicDatas.Add(sxr.VPrefix + "Style" + sIdx, row["StyleID"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "name" + sIdx, row["name"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "addressen" + sIdx, row["AddressEN"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "tel" + sIdx, row["Tel"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "fax" + sIdx, row["Fax"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "remark" + sIdx, row["remark"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "season" + sIdx, row["SeasonID"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "delivery" + sIdx, row["delivery"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "des" + sIdx, row["des"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "terms" + sIdx, row["terms"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "styleno" + sIdx, row["StyleID"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "qty" + sIdx, row["QTY"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "descripition" + sIdx, row["descripition"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "price" + sIdx, row["price"].ToString());
                sxr.DicDatas.Add(sxr.VPrefix + "amount" + sIdx, row["amount"].ToString());

                int l = 79;
                int la = row["AddressEN"].ToString().Length / l;
                for (int i = 1; i <= la; i++)
                {
                    if (row["AddressEN"].ToString().Length > l * i)
                    {
                        wks.get_Range("A6").RowHeight = 16.5 * (i + 1);
                    }
                }

                System.Data.DataTable[] dts;
                res = DBProxy.Current.SelectSP("", "PPIC_Report03", new List<SqlParameter> { new SqlParameter("@OrderID", oid), new SqlParameter("@ByType", 0) }, out dts);

                if (!res) continue;
                if (dts.Length < 3) continue;

                sxrc.XltRptTable tbl1 = new sxrc.XltRptTable(dts[0], 1, 2, true);
                sxrc.XltRptTable tbl2 = new sxrc.XltRptTable(dts[1], 1, 3);
                sxrc.XltRptTable tbl3 = new sxrc.XltRptTable(dts[2], 1, 0);
                SetColumn1toText(tbl1);
                SetColumn1toText(tbl2);
                SetColumn1toText(tbl3);
                sxr.DicDatas.Add(sxr.VPrefix + "qtybreakdown" + sIdx, tbl1);
                sxr.DicDatas.Add(sxr.VPrefix + "fabcom" + sIdx, tbl2);
                sxr.DicDatas.Add(sxr.VPrefix + "acccom" + sIdx, tbl3);

                sxr.DicDatas.Add(sxr.VPrefix + "shipmark" + sIdx, new sxrc.XltLongString(row["mark"].ToString().Trim()));
                sxr.DicDatas.Add(sxr.VPrefix + "paching" + sIdx, new sxrc.XltLongString(row["packing"].ToString()));
                sxr.DicDatas.Add(sxr.VPrefix + "labelhantag" + sIdx, new sxrc.XltLongString(row["label"].ToString()));
                string UserName;
                UserPrg.GetName(Env.User.UserID, out UserName, UserPrg.NameType.idAndNameAndExt);
                sxr.DicDatas.Add(sxr.VPrefix + "userid" + sIdx, UserName);

            }


            sxr.IsProtect = true; //Excel 加密
            sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("PPIC_P01_CMPQ"));
            this.HideWaitMessage();
        }
        void SetColumn1toText(sxrc.XltRptTable tbl)
        {
            sxrc.XlsColumnInfo c1 = new sxrc.XlsColumnInfo(1);
            c1.NumberFormate = "@";
            tbl.LisColumnInfo.Add(c1);
        }
      
        //Artwork
        private void btnArtwork_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_Artwork callNextForm = new Sci.Production.PPIC.P01_Artwork(false, MyUtility.Convert.GetString(CurrentMaintain["ID"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["StyleID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]));
            callNextForm.ShowDialog(this);
        }

        //Garment export
        private void btnGarmentExport_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_GMTExport callNextForm = new Sci.Production.PPIC.P01_GMTExport(MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        //Sewing Inline History
        private void btnH_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = 
                new Win.UI.ShowHistory("Order_History", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "SewInOffLine", caption: "History SP#", dataType: "D", setGrid: this.showHistory_SetGrid);
            callNextForm.ShowDialog(this);
        }
        //改GridHeader&欄寬
        void showHistory_SetGrid(Sci.Win.UI.ShowHistory history) { 
            
            Helper.Controls.Grid.Generator(history.grid1)
                       .Date("NewValue", header: "New Date", width: Widths.AnsiChars(12), iseditingreadonly: true)
                       .Date("OldValue", header: "Old Date", width: Widths.AnsiChars(12), iseditingreadonly: true)
                       .Text("ReasonID", header: "Reason ID", width: Widths.AnsiChars(10), iseditingreadonly: true)
                       .Text("Reason", header: "Reason", width: Widths.AnsiChars(10), iseditingreadonly: true)
                       .EditText("Remark", header: "Remark", width: Widths.AnsiChars(22), iseditingreadonly: true)
                       .Text("AddBy", header: "Update Date", width: Widths.AnsiChars(43), iseditingreadonly: true);
            //排序
            history.listControlBindingSource1.DataSourceChanged += (s, e) => {
                if (history.listControlBindingSource1.DataSource == null) return;
                var data = (System.Data.DataTable)history.listControlBindingSource1.DataSource;
                data.DefaultView.Sort = "AddDate";

            };
        }
        //Cutting Combo
        private void btnCuttingCombo_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CuttingCombo callNextForm = new Sci.Production.PPIC.P01_CuttingCombo(MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        //b'down
        private void btnbdown_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyCTN callNextForm = new Sci.Production.PPIC.P01_QtyCTN(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Material Import
        private void btnMaterialImport_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_MTLImport callNextForm = new Sci.Production.PPIC.P01_MTLImport(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Fabric inspection list
        private void btnFabricInspectionList_Click(object sender, EventArgs e)
        {
            Sci.Production.Quality.P01 callNextForm = new Sci.Production.Quality.P01(MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        //Accessory inspection list
        private void btnAccessoryInspectionList_Click(object sender, EventArgs e)
        {
            Sci.Production.Quality.P02 callNextForm = new Sci.Production.Quality.P02(MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        //Artwork Transaction List
        private void btnArtworkTransactionList_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ArtworkTrans callNextForm = new Sci.Production.PPIC.P01_ArtworkTrans(MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        //Production Kits
        private void btnProductionKits_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionKit callNextForm = new Sci.Production.PPIC.P01_ProductionKit(dataType == "1" ? true : false, MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["StyleID"]));
            callNextForm.ShowDialog(this);
        }

        private int intSizeSpecColumnCnt = 18;
        void ForSizeSpec(Worksheet oSheet, int rowNo, int columnNo)
        {
            for (int colIdx = 3; colIdx <= intSizeSpecColumnCnt; colIdx++)
            {
                //oSheet.Cells[4, colIdx].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Red);    
                oSheet.Cells[4, colIdx].HorizontalAlignment = XlHAlign.xlHAlignLeft;
            }
            for (int colIdx = 3; colIdx <= intSizeSpecColumnCnt; colIdx++)
            {
                //oSheet.Cells[4 + intSizeSpecRowCnt, colIdx].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Red);
                oSheet.Cells[4, colIdx].HorizontalAlignment = XlHAlign.xlHAlignLeft;
            }
        }

     private DataRow GetTitleDataByCustCD(string poid, string id, bool ByCustCD = true)
        {
            DataRow drvar;
            string cmd = "";
            if (ByCustCD)
            {
                cmd = @"

SELECT MAKER=max(FactoryID),sty=max(StyleID)+'-'+max(SeasonID),QTY=sum(QTY),'SPNO'=RTRIM(POID)+b.spno FROM MNOrder a WITH (NOLOCK) 
OUTER APPLY(SELECT STUFF((SELECT '/'+REPLACE(ID,@poid,'') FROM MNOrder WITH (NOLOCK) WHERE POID = @poid AND CustCDID = (select CustCDID from MNOrder WITH (NOLOCK) where ID = @ID) 
	order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno) b
where POID = @poid and CustCDID = (select CustCDID from MNOrder WITH (NOLOCK) where ID = @ID) group by POID,b.spno";
            }
            else
            {
                cmd = @"SELECT MAKER=max(FactoryID),sty=max(StyleID)+'-'+max(SeasonID),QTY=sum(QTY),'SPNO'=RTRIM(POID)+b.spno FROM MNOrder a WITH (NOLOCK) 
OUTER APPLY(SELECT STUFF((SELECT '/'+REPLACE(ID,@poid,'') FROM MNOrder WITH (NOLOCK) WHERE POID = @poid
	order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno) b
where POID = @poid group by POID,b.spno";
            }

            bool res = MyUtility.Check.Seek(cmd, new List<SqlParameter> { new SqlParameter("@poid", poid), new SqlParameter("@ID", id) }, out drvar, null);
            if (res)
                return drvar;
            else
                return null;
        }
        
        //M/Notice Sheet
     private void btnMNoticeSheet_Click(object sender, EventArgs e)
     {
         if (CurrentMaintain["SMnorderApv"].ToString() == null || CurrentMaintain["SMnorderApv"].ToString() == "")
         {
             var dr = this.CurrentMaintain; if (null == dr) return;
             var frm = new Sci.Production.PPIC.P01_MNoticePrint(null, dr["ID"].ToString());
             frm.ShowDialog(this);
             this.RenewData();
             return;
         }
         else
         {
             string poid = CurrentMaintain["POID"].ToString();
             SMNoticePrg.PrintSMNotice(poid, SMNoticePrg.EnuPrintSMType.Order);
         }
     }

        private static void MoveSubBlockIntoMainSheet(MsExcel.Worksheet mainSheet, ref int rowPosition, MsExcel.Worksheet subBlockSheet, int? blankRowsAfterThisBlock = null)
        {
            //把這個Block3完整複製過去主Sheet(參考rowPosition)
            var thisSheetUsedRange = subBlockSheet.UsedRange;
            (mainSheet.Rows[rowPosition] as MsExcel.Range).EntireRow.InsertIndent(thisSheetUsedRange.Rows.Count);

            var rowStart = thisSheetUsedRange.Rows[1].Row;
            var rowEnd = rowStart + thisSheetUsedRange.Rows.Count;
            //Full Row Copy for row height copy purpose
            subBlockSheet.Range[subBlockSheet.Rows[rowStart], subBlockSheet.Rows[rowEnd]].Copy();
            mainSheet.Range[mainSheet.Rows[rowPosition], mainSheet.Rows[rowPosition + thisSheetUsedRange.Rows.Count]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);

            ////Range Copy for content & cell format  <-- because Range Copy ignore Row height copy, so I have to copy full rows before here
            //thisSheetUsedRange.Copy();
            //mainSheet.Cells[rowPosition, 1].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);

            //rowPosition遞移，給下一個區塊使用
            rowPosition += (thisSheetUsedRange.Rows.Count + blankRowsAfterThisBlock.GetValueOrDefault(0)); //與下個Block空一行

            Marshal.ReleaseComObject(thisSheetUsedRange);
            thisSheetUsedRange = null;
        }



        //Q'ty b'down by schedule
        private void btnQtyBdownbySchedule_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtySewingSchedule callNextForm = new Sci.Production.PPIC.P01_QtySewingSchedule(MyUtility.Convert.GetString(CurrentMaintain["ID"]),MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]));
            callNextForm.ShowDialog(this);
        }

        //Carton Status
        private void btnCartonStatus_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CTNStatus callNextForm = new Sci.Production.PPIC.P01_CTNStatus(MyUtility.Convert.GetString(CurrentMaintain["ID"]), false);
            callNextForm.ShowDialog(this);
        }

        //Packing Method
        private void btnPackingMethod_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["Packing"]), "Packing Method", false, null);
            callNextForm.ShowDialog(this);
        }

        //Pull forward remark
        private void btnPullForwardRemark_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Remark from Order_PFHis WITH (NOLOCK) where Id = '{0}' order by AddDate desc", MyUtility.Convert.GetString(CurrentMaintain["ID"]))), "Pull Forward Remark");

        }

        //Shipment Finished
        private void btnShipmentFinished_Click(object sender, EventArgs e)
        {
            string sqlCmd;
            if (MyUtility.Convert.GetString(CurrentMaintain["Category"]) == "M")
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from PO WITH (NOLOCK) where ID = '{0}' and Complete = 1", MyUtility.Convert.GetString(CurrentMaintain["POID"]))))
                {
                    sqlCmd = string.Format(@"select A.ID
                                            from PO_Supp_Detail A WITH (NOLOCK) 
                                            left join MDivisionPoDetail B WITH (NOLOCK) on B.POID=A.ID and B.Seq1=A.SEQ1 and B.Seq2=A.SEQ2
                                            inner join dbo.Factory F WITH (NOLOCK) on F.id=A.factoryid and F.MDivisionID='{0}'
                                            where A.ID = '{1}' and (ETA > GETDATE() or B.InQty <> B.OutQty - B.AdjustQty)"
                                            , CurrentMaintain["MDivisionID"], CurrentMaintain["POID"]);
                    if (MyUtility.Check.Seek(sqlCmd))
                    {
                        MyUtility.Msg.WarningBox("Warehouse still have material, so can't finish shipment.");
                        return;
                    }
                }
            }
            else
            {
                sqlCmd = string.Format("select (select ID+',' from Orders WITH (NOLOCK) where POID = '{0}' and Qty > 0 and PulloutComplete = 0 for xml path('')) as SP", MyUtility.Convert.GetString(CurrentMaintain["POID"]));
                string spList = MyUtility.GetValue.Lookup(sqlCmd);
                if (!MyUtility.Check.Empty(spList))
                {
                    MyUtility.Msg.WarningBox("Below combined SP# not yet ship!!\r\n" + spList.Substring(0, spList.Length - 1));
                    return;
                }
            }

            DialogResult buttonResult = MyUtility.Msg.QuestionBox("Are you sure you want to finish shipment?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            sqlCmd = string.Format("exec [dbo].usp_closeOrder '{0}','1'",MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Shipment finished fail!!"+result.ToString());
                return;
            }
            ReloadDatas();
            RenewData();
        }

        //VAS/SHAS Instruction
        private void btnVASSHASInstruction_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["Packing2"]), "VAS/SHAS Instruction", false, null);
            callNextForm.ShowDialog(this);
        }

        //Back to P01. PPIC Master List
        private void btnBacktoPPICMasterList_Click(object sender, EventArgs e)
        {
            if (MyUtility.GetValue.Lookup(string.Format("select iif(WhseClose is null, 'TRUE','FALSE') as WHouseClose from Orders WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) == "FALSE")
            {
                MyUtility.Msg.WarningBox("W/House already closed R/mtl, so can not 'Back to P01. PPIC Master List'!!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.QuestionBox("Are you sure you want to 'Back to P01. PPIC Master List'?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string sqlCmd = string.Format("exec [dbo].usp_closeOrder '{0}','2'", MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("'Back to P01. PPIC Master List' fail!!" + result.ToString());
                return;
            }
            ReloadDatas();
            RenewData();
        }

        //ShipMode
        private void editShipMode_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                string sqlCmd = "select ID from ShipMode WITH (NOLOCK) where UseFunction like '%ORDER%' and Junk = 0";
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10",editShipMode.Text,"Ship Mode");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) return;

                CurrentMaintain["ShipModeList"] = item.GetSelectedString();
            }
        }

        // edit前檢查，非LOCAL單，不可修改
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() != "TRUE")
            {
                MyUtility.Msg.WarningBox("Only Local Order can edit !!", "Error");
                return false;
            }
            return base.ClickEditBefore();
        }

        private void btnEachCons_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            var frm = new Sci.Production.PublicForm.EachConsumption(false, CurrentMaintain["id"].ToString(), null, null, false, true,false);
            frm.ShowDialog(this);
            this.OnDetailEntered();
        }

        private void btneachconsprint_Click(object sender, EventArgs e)
        {
            if (null == this.CurrentMaintain) return;
            string ID = this.CurrentMaintain["ID"].ToString();
            var frm = new Print_OrderList(ID);
            frm.ShowDialog();
        }

        private void btnMeterialStatus_Click(object sender, EventArgs e)
        {           
            string fullpath = System.Windows.Forms.Application.StartupPath+ ".\\Sci.Production.Warehouse.dll";
            var assemblys = Assembly.LoadFile(fullpath);
            var types = assemblys.GetTypes().ToList();
            var myClass = types.Where(x => x.FullName == "Sci.Production.Warehouse.P03").First();

            if (myClass != null)
            {                
                var callMethod = myClass.GetMethod("Call");
                callMethod.Invoke(null, new object[] { CurrentMaintain["ID"].ToString(), this.MdiParent});                
            }
        }

        private void btnMeterialStatus_Local_Click(object sender, EventArgs e)
        {
            var fullpath = System.Windows.Forms.Application.StartupPath + ".\\Sci.Production.Warehouse.dll";
            var assemblys = Assembly.LoadFile(fullpath);
            var types = assemblys.GetTypes().ToList();
            var myClass = types.Where(x => x.FullName == "Sci.Production.Warehouse.P04").First();

            if (myClass != null)
            {
                var callMethod = myClass.GetMethod("Call");
                callMethod.Invoke(null, new object[] { CurrentMaintain["ID"].ToString() });
            }
        }

        private void disCustCD_MouseDown(object sender, MouseEventArgs e)
        {
            if (EditMode)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format(
                @"select id,countryid,city from Custcd where brandid= '{0}' and junk=0 ", displayBrand.Text), "16,2,16", this.disCustCD.Text);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) return;
                    CurrentMaintain["CustCDID"] = item.GetSelectedString();
                }

            }
        }
    }
}

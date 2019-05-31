﻿using System;
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
    /// <summary>
    /// P01
    /// </summary>
    public partial class P01 : Sci.Win.Tems.Input1
    {
        private string dataType;
        private bool muustEmpty = false;
        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        /// <param name="type">string Type</param>
        public P01(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.IsSupportNew = type == "1" ? true : false;
            this.IsSupportEdit = type == "1" ? true : false;

            this.Text = type == "1" ? "P01. PPIC Master List" : "P011. PPIC Master List (History)";
            this.DefaultFilter = type == "1" ? string.Format("MDivisionID = '{0}' AND Finished = 0", Sci.Env.User.Keyword) : string.Format("MDivisionID = '{0}' AND Finished = 1", Sci.Env.User.Keyword);
            this.dataType = type;
            this.btnShipmentFinished.Visible = this.dataType == "1"; // Shipment Finished
            this.btnBacktoPPICMasterList.Visible = this.dataType != "1"; // Back to P01. PPIC Master List
        }

        /// <inheritdoc/>
        protected override void OnDetailDetached()
        {
            if (this.CurrentDataRow!=null)
                this.GetCustCDKit();

            base.OnDetailDetached();
            this.ControlButton();
        }

      // 按鈕控制
      private void ControlButton()
      {
         this.btnMCHandleCFM.Enabled = this.CurrentMaintain != null;
         this.btnLocalMRCFM.Enabled = this.CurrentMaintain != null;
         this.btnProductionOutput.Enabled = this.CurrentMaintain != null;
         this.btnOrderRemark.Enabled = this.CurrentMaintain != null;
            this.btnPoRemark.Enabled = this.CurrentMaintain != null;
            this.btnFactoryCMT.Enabled = this.CurrentMaintain != null;
         this.btnLabelHangtag.Enabled = this.CurrentMaintain != null;
         this.btnQtyBdownByShipmode.Enabled = this.CurrentMaintain != null;
         this.btnQuantityBreakdown.Enabled = this.CurrentMaintain != null;
         this.btnShippingMark.Enabled = this.CurrentMaintain != null;
         this.btnTMSCost.Enabled = this.CurrentMaintain != null;
         this.btnStdGSDList.Enabled = this.CurrentMaintain != null;
         this.btnCMPQRemark.Enabled = this.CurrentMaintain != null;
         this.btnCMPQSheet.Enabled = this.CurrentMaintain != null;
         this.btnArtwork.Enabled = this.CurrentMaintain != null;
         this.btnGarmentExport.Enabled = this.CurrentMaintain != null;
         this.btnH.Enabled = this.CurrentMaintain != null;
         this.btnCuttingCombo.Enabled = this.CurrentMaintain != null;
         this.btnbdown.Enabled = this.CurrentMaintain != null;
         this.btnMaterialImport.Enabled = this.CurrentMaintain != null;
         this.btnFabricInspectionList.Enabled = this.CurrentMaintain != null;
         this.btnAccessoryInspectionList.Enabled = this.CurrentMaintain != null;
         this.btnArtworkTransactionList.Enabled = this.CurrentMaintain != null;
         this.btnProductionKits.Enabled = this.CurrentMaintain != null;
         this.btnMNoticeSheet.Enabled = this.CurrentMaintain != null;
         this.btnQtyBdownbySchedule.Enabled = this.CurrentMaintain != null;
         this.btnCartonStatus.Enabled = this.CurrentMaintain != null;
         this.btnPackingMethod.Enabled = this.CurrentMaintain != null;
         this.btnPullForwardRemark.Enabled = this.CurrentMaintain != null;
         this.btnShipmentFinished.Enabled = this.CurrentMaintain != null;
         this.btnVASSHASInstruction.Enabled = this.CurrentMaintain != null;
         this.btnBacktoPPICMasterList.Enabled = this.CurrentMaintain != null;
         var dateBuyerDlv = this.dateBuyerDlv.Value;
         var dateSCIDlv = this.dateSCIDlv.Value;
         if (dateBuyerDlv > dateSCIDlv)
         {
            this.dateSCIDlv.TextBackColor = System.Drawing.Color.Yellow;
         }
         else
         {
            this.dateSCIDlv.TextBackColor = System.Drawing.Color.FromArgb(183, 227, 225);
         }

      }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 新增Batch Shipment Finished按鈕
            Sci.Win.UI.Button btn = new Sci.Win.UI.Button();
            btn.Text = "Batch Shipment Finished";
            btn.Click += new EventHandler(this.Btn_Click);
            this.browsetop.Controls.Add(btn);
            btn.Size = new Size(180, 30); // 預設是(80,30)
            btn.Visible = this.dataType == "1";
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (!this.EditMode)
            {
                this.ControlButton();
            }

            this.displaySampleReason2.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Order_reMakeSample' and ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["SampleReason"])));
            this.displayUpdateDeliveryReason.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Order_BuyerDelivery' and ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["KPIChangeReason"])));
            this.displaySpecialMark.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["SpecialMark"])));
            this.numCPUAmt.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["CPU"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["CPUFactor"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["Qty"]), 3);
            this.displayMTLCmpltSP.Value = MyUtility.Convert.GetString(this.CurrentMaintain["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
            this.displayOutstandingReason2.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Delivery_OutStand' and ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["OutstandingReason"])));
            this.displayFinalUpdateOutstandingReasondate.Value = MyUtility.Check.Empty(this.CurrentMaintain["OutstandingDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["OutstandingDate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));
            if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                this.numCMPQPrice.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["PoPrice"]);
                this.label44.Text = "/PCS";
                this.numCMPQAmt.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["PoPrice"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["Qty"]), 3);
                this.tooltip.SetToolTip(this.numCMPQAmt, MyUtility.Convert.GetString(this.CurrentMaintain["PoPrice"]) + " * " + MyUtility.Convert.GetString(this.CurrentMaintain["Qty"]));
            }
            else
            {
                this.numCMPQPrice.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["CMPPrice"]);
                this.label44.Text = "/" + MyUtility.Convert.GetString(this.CurrentMaintain["CMPUnit"]);
                this.numCMPQAmt.Value = this.numCPUAmt.Value;
                this.tooltip.SetToolTip(this.numCMPQAmt, MyUtility.Convert.GetString(this.CurrentMaintain["CPU"]) + " * " + MyUtility.Convert.GetString(this.CurrentMaintain["CPUFactor"]) + " * " + MyUtility.Convert.GetString(this.CurrentMaintain["Qty"]));
            }
            #region 填Description, Exception Form, Fty Remark, Style Apv欄位值
            DataRow styleData;
            string sqlCmd = string.Format("select Description,ExpectionForm,FTYRemark,ApvDate,ExpectionFormRemark from Style WITH (NOLOCK) where Ukey = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["StyleUkey"]));
            if (MyUtility.Check.Seek(sqlCmd, out styleData))
            {
                this.displayDescription.Value = MyUtility.Convert.GetString(styleData["Description"]);
                this.checkExceptionForm.Value = MyUtility.Convert.GetString(styleData["ExpectionForm"]);
                this.editFtyRemark.Text = MyUtility.Convert.GetString(styleData["FTYRemark"]);
                if (MyUtility.Check.Empty(styleData["ApvDate"]))
                {
                    this.dateStyleApv.Value = null;
                }
                else
                {
                    this.dateStyleApv.Value = MyUtility.Convert.GetDate(styleData["ApvDate"]);
                }

                if (MyUtility.Check.Empty(styleData["ExpectionFormRemark"]))
                {
                    this.btnExpectionFormRemark.Enabled = false;
                }
                else
                {
                    this.btnExpectionFormRemark.Enabled = true;
                }
            }
            else
            {
                this.btnExpectionFormRemark.Enabled = false;
                this.displayDescription.Value = string.Empty;
                this.checkExceptionForm.Value = "false";
                this.editFtyRemark.Text = string.Empty;
                this.dateStyleApv.Value = null;
            }
            #endregion
            #region 填Buyer欄位值, 修改Special id1, Special id2, Special id3顯示值
            DataRow brandData;
            if (MyUtility.Check.Seek(string.Format("select ID,Customize1,Customize2,Customize3,BuyerID from Brand WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"])), out brandData))
            {
                this.displayBuyer.Value = MyUtility.Convert.GetString(brandData["BuyerID"]);
                this.labelSpecialId1.Text = MyUtility.Convert.GetString(brandData["Customize1"]);
                this.labelSpecialId2.Text = MyUtility.Convert.GetString(brandData["Customize2"]);
                this.labelSpecialId3.Text = MyUtility.Convert.GetString(brandData["Customize3"]);
            }
            else
            {
                this.displayBuyer.Value = string.Empty;
                this.labelSpecialId1.Text = string.Empty;
                this.labelSpecialId2.Text = string.Empty;
                this.labelSpecialId3.Text = string.Empty;
            }
            #endregion
            #region 填PO SMR, PO Handle欄位值
            DataRow pOData;
            sqlCmd = string.Format("select POSMR,POHandle,PCHandle from PO WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            if (MyUtility.Check.Seek(sqlCmd, out pOData))
            {
                this.txttpeuser3.DisplayBox1Binding = MyUtility.Convert.GetString(pOData["POSMR"]);
                this.txttpeuser4.DisplayBox1Binding = MyUtility.Convert.GetString(pOData["POHandle"]);
                this.PcHandleText.DisplayBox1Binding = MyUtility.Convert.GetString(pOData["PCHandle"]);
            }
            else
            {
                this.txttpeuser3.DisplayBox1Binding = string.Empty;
                this.txttpeuser4.DisplayBox1Binding = string.Empty;
                this.PcHandleText.DisplayBox1Binding = string.Empty;
            }
            #endregion
            #region 填PO Combo, Cutting Combo, MTLExport, PulloutComplete, Garment L/T, OrderCombo 欄位值
            System.Data.DataTable ordersData;
            sqlCmd = string.Format(
                @"select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList,
isnull([dbo].getCuttingComboList(o.ID,o.CuttingSP),'') as CuttingList,
isnull([dbo].getMTLExport(o.POID,o.MTLExport),'') as MTLExport,
isnull([dbo].getPulloutComplete(o.ID,o.PulloutComplete),'') as PulloutComplete,
isnull([dbo].getGarmentLT(o.StyleUkey,o.FactoryID),0) as GMTLT from Orders o WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out ordersData);
            if (result)
            {
                if (ordersData.Rows.Count > 0)
                {
                    this.editPOCombo.Text = MyUtility.Convert.GetString(ordersData.Rows[0]["PoList"]);
                    this.editCuttingCombo.Text = MyUtility.Convert.GetString(ordersData.Rows[0]["CuttingList"]);
                    this.displayRMTLETAMasterSP.Value = MyUtility.Convert.GetString(ordersData.Rows[0]["MTLExport"]);
                    this.displayActPullout.Value = MyUtility.Convert.GetString(ordersData.Rows[0]["PulloutComplete"]);
                    this.numGarmentLT.Value = MyUtility.Convert.GetDecimal(ordersData.Rows[0]["GMTLT"]);
                }
                else
                {
                    this.editPOCombo.Text = string.Empty;
                    this.editCuttingCombo.Text = string.Empty;
                    this.displayRMTLETAMasterSP.Value = string.Empty;
                    this.displayActPullout.Value = string.Empty;
                    this.numGarmentLT.Value = 0;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query OrdersData fail!!" + result.ToString());
                this.editPOCombo.Text = string.Empty;
                this.editCuttingCombo.Text = string.Empty;
                this.displayRMTLETAMasterSP.Value = string.Empty;
                this.displayActPullout.Value = string.Empty;
                this.numGarmentLT.Value = 0;
            }

            this.displayOrderCombo.Value = MyUtility.GetValue.Lookup(string.Format("Select Top 1 OrderComboList from dbo.Order_OrderComboList with(nolock) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            #endregion
            bool lConfirm = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P01. PPIC Master List", "CanConfirm");
            this.btnMCHandleCFM.Enabled = this.CurrentMaintain != null && this.dataType == "1" && lConfirm && !this.EditMode;
            this.btnLocalMRCFM.Enabled = this.CurrentMaintain != null && this.dataType == "1" && lConfirm && !this.EditMode;
            this.btnbdown.Enabled = this.CurrentMaintain != null && MyUtility.Convert.GetString(this.CurrentMaintain["CtnType"]) == "2" && !this.EditMode;

            // 按鈕變色
            bool haveTmsCost = MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            this.btnProductionOutput.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnOrderRemark.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;

            //若有資料顯示藍色，否則黑色
            this.btnPoRemark.ForeColor = MyUtility.Check.Seek(string.Format("select PoRemark from PO WITH (NOLOCK) where ID = '{0}' AND PoRemark != '' ", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;

            this.btnFactoryCMT.ForeColor = haveTmsCost ? Color.Blue : Color.Black;
            this.btnLabelHangtag.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["Label"]) ? Color.Blue : Color.Black;
            this.btnQtyBdownByShipmode.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnQuantityBreakdown.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnShippingMark.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["MarkFront"]) || !MyUtility.Check.Empty(this.CurrentMaintain["MarkBack"]) || !MyUtility.Check.Empty(this.CurrentMaintain["MarkLeft"]) || !MyUtility.Check.Empty(this.CurrentMaintain["MarkRight"]) ? Color.Blue : Color.Black;
            this.btnTMSCost.ForeColor = haveTmsCost ? Color.Blue : Color.Black;
            this.btnStdGSDList.ForeColor = MyUtility.Check.Seek(string.Format("select i.ID from Style s WITH (NOLOCK) , IETMS i WITH (NOLOCK) where s.Ukey = '{0}' and s.IETMSID = i.ID and s.IETMSVersion = i.Version", MyUtility.Convert.GetString(this.CurrentMaintain["StyleUkey"]))) && MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnCMPQRemark.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["CMPQRemark"]) ? Color.Blue : Color.Black;
            this.btnCMPQSheet.ForeColor = MyUtility.Check.Seek(string.Format("select ID from orders WITH (NOLOCK) where Junk = 0 and POID='{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;
            this.btnArtwork.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Artwork WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnGarmentExport.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnCuttingCombo.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["CuttingSP"]) ? Color.Blue : Color.Black;
            if (MyUtility.Convert.GetString(this.CurrentMaintain["CtnType"]) == "2")
            {
                this.btnbdown.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyCTN WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            }

            this.btnMaterialImport.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Export_Detail WITH (NOLOCK) where PoID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]))) ? (MyUtility.Convert.GetString(this.CurrentMaintain["POID"]) != string.Empty ? Color.Blue : Color.Black) : Color.Black;
            this.btnFabricInspectionList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from FIR WITH (NOLOCK) where PoID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]))) ? (MyUtility.Convert.GetString(this.CurrentMaintain["POID"]) != string.Empty ? Color.Blue : Color.Black) : Color.Black;
            this.btnAccessoryInspectionList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from AIR WITH (NOLOCK) where PoID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;
            this.btnArtworkTransactionList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from ArtworkPO_Detail WITH (NOLOCK) where OrderID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnProductionKits.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits WITH (NOLOCK) where StyleUkey = '{0}' ", MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"]))) ? Color.Blue : Color.Black;
            this.btnPFHistory.ForeColor = MyUtility.Check.Seek($@"select id from Order_PFHis with(nolock) where id = '{this.CurrentMaintain["ID"]}'") ? Color.Blue : Color.Black;

            #region 控制[m/notice sheet]按鈕是否變色
            bool enableMNotice1 = MyUtility.Check.Seek(string.Format("select ID FROM MnOrder_ColorCombo WITH (NOLOCK) where ID = (select OrderComboID FROM MNOrder where ID = '{1}')", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            bool enableMNotice2 = !MyUtility.Check.Empty(this.CurrentMaintain["SMnorderApv"]);
            bool enableMNotice = enableMNotice1 || enableMNotice2;
            this.btnMNoticeSheet.ForeColor = enableMNotice ? Color.Blue : Color.Black;
            #endregion

            this.btnQtyBdownbySchedule.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["SewLine"]) ? Color.Blue : Color.Black;
            this.btnCartonStatus.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' and ReceiveDate is not null", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnPackingMethod.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["Packing"]) ? Color.Blue : Color.Black;
            this.btnPullForwardRemark.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_PFHis WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnVASSHASInstruction.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["Packing2"]) ? Color.Blue : Color.Black;
            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "Order_EachCons", "ID"))
            {
                this.btnEachCons.ForeColor = Color.Blue;
            }
            else
            {
                this.btnEachCons.ForeColor = Color.Black;
            }

            string sqkchkEMNF = $@"select 1 From Order_ECMNFailed f Left Join Orders o on f.id	= o.ID Where (o.ID = '{this.CurrentMaintain["ID"]}' And f.Type = 'EC')or (o.POID = '{this.CurrentMaintain["POID"]}'  And f.Type = 'MN')";

            // 只要 Each Cons.Apv. & M.Notice Apv.有核准的日期 [Econs / MN Failed]按鈕 就不用改變文字顏色、並且Each Cons/M.Notice Failed 畫面不顯示資料
            if (!MyUtility.Check.Empty(this.CurrentMaintain["EachConsApv"]) && !MyUtility.Check.Empty(this.CurrentMaintain["MnorderApv"]))
            {
                this.muustEmpty = true;
                this.btnEConsMNFailed.ForeColor = Color.Black;
            }
            else
            {
                this.muustEmpty = false;
                if (MyUtility.Check.Seek(sqkchkEMNF))
                {
                    this.btnEConsMNFailed.ForeColor = Color.Blue;
                }
                else
                {
                    this.btnEConsMNFailed.ForeColor = Color.Black;
                }
            }

            // SciDelivery OrigBuyerDelivery
            // CRDDate
            if (!MyUtility.Check.Empty(this.CurrentMaintain["SciDelivery"]))
            {
                this.dateDetailsSCIDel.TextForeColor = this.CurrentMaintain["SciDelivery"].ToString() != this.CurrentMaintain["BuyerDelivery"].ToString() ? Color.Red : Color.Blue;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["CRDDate"]))
            {
                this.dateDetailsCRDdate.TextForeColor = MyUtility.Convert.GetDate(this.CurrentMaintain["CRDDate"]) < MyUtility.Convert.GetDate(this.CurrentMaintain["BuyerDelivery"]) ? Color.Red : Color.Blue;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["IsForecast"].ToString()))
            {
                // 訂單屬於 Forecast，Buy Month 文字改成 Est. Download Date
                if ((bool)this.CurrentMaintain["IsForecast"])
                {
                    this.labelBuyMonth.Text = "Est. Download Date";

                    // 加寬
                    this.labelBuyMonth.Size = new System.Drawing.Size(119, 21);

                    // 其餘控制項往右推
                    this.displayBuyMonth.Location = new System.Drawing.Point(697, 139);
                    this.labelOrderQty.Location = new System.Drawing.Point(861, 112);
                    this.numOrderQty.Location = new System.Drawing.Point(926, 112);
                    this.labelFOCQty.Location = new System.Drawing.Point(861, 139);
                    this.numFOCQty.Location = new System.Drawing.Point(926, 139);
                }
                else
                {
                    this.labelBuyMonth.Text = "Buy Month";
                    this.labelBuyMonth.Size = new System.Drawing.Size(65, 21);

                    this.displayBuyMonth.Location = new System.Drawing.Point(644, 139);
                    this.labelOrderQty.Location = new System.Drawing.Point(815, 112);
                    this.numOrderQty.Location = new System.Drawing.Point(880, 112);
                    this.labelFOCQty.Location = new System.Drawing.Point(815, 139);
                    this.numFOCQty.Location = new System.Drawing.Point(880, 139);
                }
            }

            GetIsDevSample();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.txtpaytermar1.TextBox1.ReadOnly = true;
            this.txtDevSample.ReadOnly = true;
            this.label44.Text = "/PCS";

            // 帶入預設值
            this.CurrentMaintain["Category"] = "B";
            this.CurrentMaintain["LocalOrder"] = 1;
            this.CurrentMaintain["MCHandle"] = Sci.Env.User.UserID;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["FtyGroup"] = Env.User.Factory;
            this.CurrentMaintain["CMPUnit"] = "PCS";
            this.CurrentMaintain["CFMDate"] = DateTime.Today;
            this.CurrentMaintain["CtnType"] = "1";
            this.CurrentMaintain["CPUFactor"] = 1;
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtpaytermar1.TextBox1.ReadOnly = true;
            if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalOrder"]).ToUpper() == "FALSE")
            {
                // 非Local訂單時只能修改FactoryID
                this.txtProgram.ReadOnly = true;
                this.txtPONo.ReadOnly = true;
                this.txtStyle.ReadOnly = true;
                this.txtModel.ReadOnly = true;
                this.txtSpecialId1.ReadOnly = true;
                this.txtSpecialId2.ReadOnly = true;
                this.txtSpecialId3.ReadOnly = true;
                this.checkSubconInFromSisterFactory.ReadOnly = true;
                this.checkCancelledOrder.ReadOnly = true;
                this.checkFOC.ReadOnly = true;
                this.checkSP.ReadOnly = true;
                this.checkTissuePaper.ReadOnly = true;
                this.checkRainwearTestPassed.ReadOnly = true;
                this.txtuser1.TextBox1.ReadOnly = true;
                this.txtuser2.TextBox1.ReadOnly = true;
                this.dateBuyerDlv.ReadOnly = true;
                this.dateOrigBuyerDlv.ReadOnly = true;
                this.dateSCIDlv.ReadOnly = true;
                this.dateCutOffDate.ReadOnly = true;
                this.txtcountry1.TextBox1.ReadOnly = true;
                this.txtcurrency1.ReadOnly = true;
                this.numUnitPrice.ReadOnly = true;
                this.numQtyCarton.ReadOnly = true;
                this.txtDevSample.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                #region 檢查必輸欄位
                if (MyUtility.Check.Empty(this.CurrentMaintain["StyleID"]))
                {
                    MyUtility.Msg.WarningBox("Style# can't empty!!");
                    this.txtStyle.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
                {
                    MyUtility.Msg.WarningBox("Brand can't empty!!");
                    this.displayBrand.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["CustPONo"]))
                {
                    MyUtility.Msg.WarningBox("PO No. can't empty!!");
                    this.txtPONo.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["MCHandle"]))
                {
                    MyUtility.Msg.WarningBox("MC Handle can't empty!!");
                    this.txtuser1.TextBox1.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["StyleUnit"]))
                {
                    MyUtility.Msg.WarningBox("Unit can't empty!!");
                    this.displayUnit.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["BuyerDelivery"]))
                {
                    MyUtility.Msg.WarningBox("Buyer Delivery can't empty!!");
                    this.dateBuyerDlv.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["SCIDelivery"]))
                {
                    MyUtility.Msg.WarningBox("SCI Delivery# can't empty!!");
                    this.dateSCIDlv.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["SDPDate"]))
                {
                    MyUtility.Msg.WarningBox("Cut off date can't empty!!");
                    this.dateCutOffDate.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
                {
                    MyUtility.Msg.WarningBox("Factory can't empty!!");
                    this.txtmfactory.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["CurrencyID"]))
                {
                    MyUtility.Msg.WarningBox("Currency can't empty!!");
                    this.txtcurrency1.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["CPU"]))
                {
                    MyUtility.Msg.WarningBox("CPU can't empty!!");
                    this.numCPU.Focus();
                    return false;
                }

                if (MyUtility.Convert.GetString(this.CurrentMaintain["FOC"]).ToUpper() == "FALSE" && MyUtility.Check.Empty(this.CurrentMaintain["PoPrice"]))
                {
                    MyUtility.Msg.WarningBox("Unit Price can't empty!!");
                    this.numUnitPrice.Focus();
                    return false;
                }

                if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE" && MyUtility.Check.Empty(this.CurrentMaintain["ShipModeList"]))
                {
                    MyUtility.Msg.WarningBox("Ship Mode can't empty!!");
                    this.editShipMode.Focus();
                    return false;
                }
                #endregion

                // 檢查是否幫姊妹廠代工
                if (MyUtility.Convert.GetString(this.CurrentMaintain["SubconInSisterFty"]).ToUpper() == "FALSE")
                {
                    // sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@programid", MyUtility.Convert.GetString(this.CurrentMaintain["ProgramID"]));

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    System.Data.DataTable sCIFtyData;
                    string sqlCmd = "select ID from SCIFty WITH (NOLOCK) where ID = @programid";
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out sCIFtyData);
                    if (result && sCIFtyData.Rows.Count > 0)
                    {
                        this.CurrentMaintain["SubconInSisterFty"] = 1;
                    }
                }

                string strUpd_QtyShip_BuyerDelivery = string.Format(
                    @"
Update oq
set oq.BuyerDelivery = '{0}'
from Order_QtyShip oq WITH (NOLOCK) 
where oq.Id = '{1}'", Convert.ToDateTime(this.CurrentMaintain["BuyerDelivery"]).ToString("yyyy/MM/dd"),
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                DualResult resultUpd = DBProxy.Current.Execute(null, strUpd_QtyShip_BuyerDelivery);
                if (resultUpd == false)
                {
                    MyUtility.Msg.WarningBox(resultUpd.Description);
                    return false;
                }
            }

            // GetID
            if (this.IsDetailInserting)
            {
                string id = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Factory + "LO", "Orders", DateTime.Today);

                // string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", CurrentMaintain["ID"].ToString(), "Orders", "ID") + "LO", "Orders", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
                this.CurrentMaintain["POID"] = id;
                this.CurrentMaintain["CuttingSP"] = id;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                string insertCmd;
                DualResult result;
                if (!MyUtility.Check.Seek(string.Format("select ID from Order_Artwork WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
                {
                    insertCmd = string.Format(
                        @"
insert into Order_Artwork(ID,ArtworkTypeID,Article,PatternCode,PatternDesc,ArtworkID,ArtworkName,TMS,Qty,Price,Cost,Remark,AddName,AddDate,Ukey)
select ID,ArtworkTypeID,Article,PatternCode,PatternDesc,ArtworkID,ArtworkName,TMS,Qty,Price,Cost,Remark,loginID,today,rownumber-row
from (
select '{0}'as ID,ArtworkTypeID,Article,PatternCode,PatternDesc,ArtworkID,ArtworkName,TMS,Qty,Price,Cost,Remark,'{1}' as 'loginID',GETDATE() as 'today'
,(select isnull(MIN(UKey),0) from Order_Artwork)as 'rownumber', Row_Number() OVER( order by PatternCode ) as 'row'
from Style_Artwork where StyleUkey = '{2}'
) x",
                         MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                         Env.User.UserID,
                         MyUtility.Convert.GetString(this.CurrentMaintain["StyleUkey"]));

                    result = DBProxy.Current.Execute(null, insertCmd);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Save Order_Artwork fail!!\r\n" + result.ToString());
                        return failResult;
                    }
                }

                if (!MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
                {
                    insertCmd = string.Format(
                        @"
insert into Order_TmsCost(ID,ArtworkTypeID,Seq,Qty,ArtworkUnit,TMS,Price,AddName,AddDate)
select '{0}',ArtworkTypeID,Seq,Qty,ArtworkUnit,TMS,Price,'{1}',GETDATE() from Style_TmsCost where StyleUkey = {2}",
                        MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                        Sci.Env.User.UserID,
                        MyUtility.Convert.GetString(this.CurrentMaintain["StyleUkey"]));

                    result = DBProxy.Current.Execute(null, insertCmd);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Save Order_TmsCost fail!!\r\n" + result.ToString());
                        return failResult;
                    }
                }

                if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE" && MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
                {
                    string updateCmd = string.Format("update Order_QtyShip set ShipModeID = '{0}' where ID = '{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeList"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
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

        // Style
        private bool chkpopup = false;

        private void TxtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            IList<DataRow> styleData;
            string sqlCmd = "select ID,SeasonID,BrandID,Description,CdCodeID,CPU,StyleUnit,Ukey from Style WITH (NOLOCK) where Junk = 0 ";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "15,8,10,28,5,7,7,6", string.Empty, "Style,Season,Brand,Description,CdCode,CPU,Unit,Ukey", columndecimals: "0,0,0,0,0,3,0,0");
            item.Size = new System.Drawing.Size(950, 500);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                this.SetStyleEmptyColumn();
                this.chkpopup = false;
            }
            else
            {
                styleData = item.GetSelecteds();
                this.CurrentMaintain["StyleID"] = item.GetSelectedString();
                this.CurrentMaintain["BrandID"] = styleData[0]["BrandID"];
                this.CurrentMaintain["SeasonID"] = styleData[0]["SeasonID"];
                this.CurrentMaintain["CdCodeID"] = styleData[0]["CdCodeID"];
                this.CurrentMaintain["CPU"] = styleData[0]["CPU"];
                this.CurrentMaintain["StyleUnit"] = styleData[0]["StyleUnit"];
                this.CurrentMaintain["StyleUkey"] = styleData[0]["Ukey"];
                this.displayDescription.Value = MyUtility.Convert.GetString(styleData[0]["Description"]);
                this.chkpopup = true;
            }
        }

        // Style
        private void TxtStyle_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtStyle.OldValue != this.txtStyle.Text)
            {
                if (MyUtility.Check.Empty(this.txtStyle.Text))
                {
                    this.SetStyleEmptyColumn();
                }
                else
                {
                    if (!this.chkpopup)
                    {
                        // 檢查資料是否存在
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@styleid", this.txtStyle.Text);

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);

                        System.Data.DataTable styleData;
                        string sqlCmd = "select ID,SeasonID,BrandID,Description,CdCodeID,CPU,StyleUnit,Ukey from Style WITH (NOLOCK) where Junk = 0 and ID = @styleid";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleData);
                        if (!result || styleData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("Style not found!!");
                            }

                            this.SetStyleEmptyColumn();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            this.CurrentMaintain["StyleID"] = styleData.Rows[0]["ID"];
                            this.CurrentMaintain["BrandID"] = styleData.Rows[0]["BrandID"];
                            this.CurrentMaintain["SeasonID"] = styleData.Rows[0]["SeasonID"];
                            this.CurrentMaintain["CdCodeID"] = styleData.Rows[0]["CdCodeID"];
                            this.CurrentMaintain["CPU"] = styleData.Rows[0]["CPU"];
                            this.CurrentMaintain["StyleUnit"] = styleData.Rows[0]["StyleUnit"];
                            this.CurrentMaintain["StyleUkey"] = styleData.Rows[0]["Ukey"];
                            this.displayDescription.Value = MyUtility.Convert.GetString(styleData.Rows[0]["Description"]);
                        }
                    }
                }
            }

            this.chkpopup = false;
        }

        private void SetStyleEmptyColumn()
        {
            this.CurrentMaintain["StyleID"] = string.Empty;
            this.CurrentMaintain["BrandID"] = string.Empty;
            this.CurrentMaintain["SeasonID"] = string.Empty;
            this.CurrentMaintain["CdCodeID"] = string.Empty;
            this.CurrentMaintain["CPU"] = 0;
            this.CurrentMaintain["StyleUnit"] = string.Empty;
            this.CurrentMaintain["StyleUkey"] = 0;
            this.displayDescription.Value = string.Empty;
        }

        // Factory
        private void Txtmfactory_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.txtmfactory.OldValue != this.txtmfactory.Text)
            {
                if (MyUtility.Check.Empty(this.txtmfactory.Text))
                {
                    this.CurrentMaintain["FtyGroup"] = string.Empty;
                }
                else
                {
                    this.CurrentMaintain["FtyGroup"] = MyUtility.GetValue.Lookup("FTYGroup", this.txtmfactory.Text, "Orders", "ID");
                }
            }
        }

        // Cancelled Order
        private void CheckCancelledOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode && this.checkCancelledOrder.Checked && !MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                if (MyUtility.Check.Seek(string.Format("select ID from SewingOutput_Detail WITH (NOLOCK) where OrderId = '{0}' and QAQty > 0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
                {
                    this.CurrentMaintain["Junk"] = 0;
                    MyUtility.Msg.WarningBox("This record had sewing daily output, can't cancel!!");
                }
            }
        }

        // F.O.C.
        private void CheckFOC_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.CurrentMaintain["FOC"] = this.checkFOC.Checked;
                this.numUnitPrice.ReadOnly = this.checkFOC.Checked;
                if (this.checkFOC.Checked)
                {
                    this.CurrentMaintain["PoPrice"] = 0;
                }
            }
        }

        // Batch Shipment Finished按鈕的Click事件
        private void Btn_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_BatchShipmentFinished callNextForm = new Sci.Production.PPIC.P01_BatchShipmentFinished();
            callNextForm.ShowDialog(this);
            if (callNextForm.Haveupdate)
            {
                this.ReloadDatas();
            }
        }

        // MC Handle CFM
        private void BtnMCHandleCFM_Click(object sender, EventArgs e)
        {
            string sqlCmd = string.Format("update Orders set MCHandle = '{0}' where POID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm MC Handle fail!!" + result.ToString());
                return;
            }

            this.txtuser1.TextBox1.Text = Sci.Env.User.UserID;
        }

        // Local MR CFM
        private void BtnLocalMRCFM_Click(object sender, EventArgs e)
        {
            string sqlCmd = string.Format("update Orders set LocalMR = '{0}' where POID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm Local MR fail!!" + result.ToString());
                return;
            }

            this.txtuser2.TextBox1.Text = Sci.Env.User.UserID;
        }

        // Production output
        private void BtnProductionOutput_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput callNextForm = new Sci.Production.PPIC.P01_ProductionOutput(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Order remark
        private void BtnOrderRemark_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["OrderRemark"]), "Order Remark", false, null);
            callNextForm.ShowDialog(this);
        }

        //Po Rematk
        private void btnPoRemark_Click(object sender, EventArgs e)
        {
            System.Data.DataTable data;
            //串接規則Orders.ID=PO.ID
            string sqlCmd = string.Format("select PoRemark from PO WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"]);

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out data);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (data.Rows.Count > 0)
            {
                Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(data.Rows[0]["PoRemark"].ToString(), "PO Remark", false, null);
                callNextForm.ShowDialog(this);
            }
            else
            {
                Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(" ", "PO Remark", false, null);
                callNextForm.ShowDialog(this);
            }
        }

        // Factory CMT
        private void BtnFactoryCMT_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_FactoryCMT callNextForm = new Sci.Production.PPIC.P01_FactoryCMT(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Label & Hangtag
        private void BtnLabelHangtag_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["Label"]), "Label & Hangtag", false, null);
            callNextForm.ShowDialog(this);
        }

        // Q'ty b'down by shipmode
        private void BtnQtyBdownByShipmode_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyShip callNextForm = new Sci.Production.PPIC.P01_QtyShip(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        // Quantity breakdown
        private void BtnQuantityBreakdown_Click(object sender, EventArgs e)
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                Sci.Production.PPIC.P01_QtyLocalOrder callNextForm = new Sci.Production.PPIC.P01_QtyLocalOrder(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), this.dataType == "1" ? true : false, MyUtility.Convert.GetInt(this.CurrentMaintain["Qty"]));
                callNextForm.ShowDialog(this);
                if (this.dataType == "1")
                {
                    this.RenewData();
                }
            }
            else
            {
                Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["POID"]), this.editPOCombo.Text);
                callNextForm.ShowDialog(this);
            }
        }

        // Shipping mark
        private void BtnShippingMark_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ShippingMark callNextForm = new Sci.Production.PPIC.P01_ShippingMark(false, this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // TMS & Cost
        private void BtnTMSCost_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_TMSAndCost callNextForm = new Sci.Production.PPIC.P01_TMSAndCost(false, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), null, null);
            callNextForm.ShowDialog(this);
        }

        // Std.GSD List
        private void BtnStdGSDList_Click(object sender, EventArgs e)
        {
            Sci.Production.PublicForm.StdGSDList callNextForm = new Sci.Production.PublicForm.StdGSDList(MyUtility.Convert.GetLong(this.CurrentMaintain["StyleUKey"]));
            callNextForm.ShowDialog(this);
        }

        // CMPQ remark
        private void BtnCMPQRemark_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["CMPQRemark"]), "CMPQ Remark", false, null);
            callNextForm.ShowDialog(this);
        }

        // CMPQ Sheet
        private void BtnCMPQSheet_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Data processing, please wait...");

            // string poid = MyUtility.GetValue.Lookup("select POID FROM dbo.Orders where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", _id) });
            string poid = this.CurrentMaintain["POID"].ToString();
            System.Data.DataTable rpt3;
            string strSqlSelect = @"
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
where o.Junk = 0 and o.POID= @POID order by o.ID";

            DualResult res = DBProxy.Current.Select(string.Empty, strSqlSelect, new List<SqlParameter> { new SqlParameter("@ID", this.CurrentMaintain["ID"]), new SqlParameter("@POID", poid) }, out rpt3);

            if (!res)
            {
                return;
            }

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
                res = DBProxy.Current.SelectSP(string.Empty, "PPIC_Report03", new List<SqlParameter> { new SqlParameter("@OrderID", oid), new SqlParameter("@ByType", 0) }, out dts);

                if (!res)
                {
                    continue;
                }

                if (dts.Length < 3)
                {
                    continue;
                }

                sxrc.XltRptTable tbl1 = new sxrc.XltRptTable(dts[0], 1, 2, true);
                sxrc.XltRptTable tbl2 = new sxrc.XltRptTable(dts[1], 1, 3);
                sxrc.XltRptTable tbl3 = new sxrc.XltRptTable(dts[2], 1, 0);
                this.SetColumn1toText(tbl1);
                this.SetColumn1toText(tbl2);
                this.SetColumn1toText(tbl3);
                sxr.DicDatas.Add(sxr.VPrefix + "qtybreakdown" + sIdx, tbl1);
                sxr.DicDatas.Add(sxr.VPrefix + "fabcom" + sIdx, tbl2);
                sxr.DicDatas.Add(sxr.VPrefix + "acccom" + sIdx, tbl3);

                sxr.DicDatas.Add(sxr.VPrefix + "shipmark" + sIdx, new sxrc.XltLongString(row["mark"].ToString().Trim()));
                sxr.DicDatas.Add(sxr.VPrefix + "paching" + sIdx, new sxrc.XltLongString(row["packing"].ToString()));
                sxr.DicDatas.Add(sxr.VPrefix + "labelhantag" + sIdx, new sxrc.XltLongString(row["label"].ToString()));
                string userName;
                UserPrg.GetName(Env.User.UserID, out userName, UserPrg.NameType.idAndNameAndExt);
                sxr.DicDatas.Add(sxr.VPrefix + "userid" + sIdx, userName);
            }

            sxr.IsProtect = true; // Excel 加密
            sxr.Save(Sci.Production.Class.MicrosoftFile.GetName("PPIC_P01_CMPQ"));
            this.HideWaitMessage();
        }

        private void SetColumn1toText(sxrc.XltRptTable tbl)
        {
            sxrc.XlsColumnInfo c1 = new sxrc.XlsColumnInfo(1);
            c1.NumberFormate = "@";
            tbl.LisColumnInfo.Add(c1);
        }

        // Artwork
        private void BtnArtwork_Click(object sender, EventArgs e)
        {

            Sci.Production.PPIC.P01_Artwork callNextForm = new Sci.Production.PPIC.P01_Artwork(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        // Garment export
        private void BtnGarmentExport_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_GMTExport callNextForm = new Sci.Production.PPIC.P01_GMTExport(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        // Sewing Inline History
        private void BtnH_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm =
                new Win.UI.ShowHistory("Order_History", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "SewInOffLine", caption: "History SP#", dataType: "D", setGrid: this.ShowHistory_SetGrid);
            callNextForm.ShowDialog(this);
        }

        // 改GridHeader&欄寬
        private void ShowHistory_SetGrid(Win.UI.ShowHistory history)
        {
            this.Helper.Controls.Grid.Generator(history.grid1)
                       .Date("NewValue", header: "New Date", width: Widths.AnsiChars(12), iseditingreadonly: true)
                       .Date("OldValue", header: "Old Date", width: Widths.AnsiChars(12), iseditingreadonly: true)
                       .Text("ReasonID", header: "Reason ID", width: Widths.AnsiChars(10), iseditingreadonly: true)
                       .Text("Reason", header: "Reason", width: Widths.AnsiChars(10), iseditingreadonly: true)
                       .EditText("Remark", header: "Remark", width: Widths.AnsiChars(22), iseditingreadonly: true)
                       .Text("AddBy", header: "Update Date", width: Widths.AnsiChars(43), iseditingreadonly: true);

            // 排序
            history.listControlBindingSource1.DataSourceChanged += (s, e) =>
            {
                if (history.listControlBindingSource1.DataSource == null)
                {
                    return;
                }

                var data = (System.Data.DataTable)history.listControlBindingSource1.DataSource;
                data.DefaultView.Sort = "AddDate";
            };
        }

        // Cutting Combo
        private void BtnCuttingCombo_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CuttingCombo callNextForm = new Sci.Production.PPIC.P01_CuttingCombo(MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        // b'down
        private void Btnbdown_Click(object sender, EventArgs e)
        {
            P01_QtyCTN callNextForm = new P01_QtyCTN(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Material Import
        private void BtnMaterialImport_Click(object sender, EventArgs e)
        {
            P01_MTLImport callNextForm = new P01_MTLImport(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Fabric inspection list
        private void BtnFabricInspectionList_Click(object sender, EventArgs e)
        {
            Quality.P01 callNextForm = new Quality.P01(MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        // Accessory inspection list
        private void BtnAccessoryInspectionList_Click(object sender, EventArgs e)
        {
            Sci.Production.Quality.P02 callNextForm = new Sci.Production.Quality.P02(MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        // Artwork Transaction List
        private void BtnArtworkTransactionList_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ArtworkTrans callNextForm = new Sci.Production.PPIC.P01_ArtworkTrans(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        // Production Kits
        private void BtnProductionKits_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionKit callNextForm = new Sci.Production.PPIC.P01_ProductionKit(this.dataType == "1" ? true : false, MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"]), null, null, MyUtility.Convert.GetString(this.CurrentMaintain["StyleID"]));
            callNextForm.ShowDialog(this);
        }

        private int intSizeSpecColumnCnt = 18;

        private void ForSizeSpec(Worksheet oSheet, int rowNo, int columnNo)
        {
            for (int colIdx = 3; colIdx <= this.intSizeSpecColumnCnt; colIdx++)
            {
                // oSheet.Cells[4, colIdx].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Red);
                oSheet.Cells[4, colIdx].HorizontalAlignment = XlHAlign.xlHAlignLeft;
            }

            for (int colIdx = 3; colIdx <= this.intSizeSpecColumnCnt; colIdx++)
            {
                // oSheet.Cells[4 + intSizeSpecRowCnt, colIdx].Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Red);
                oSheet.Cells[4, colIdx].HorizontalAlignment = XlHAlign.xlHAlignLeft;
            }
        }

        private void GetCustCDKit()
        {
            DataRow tmp;
            string cmd = string.Empty;
            cmd = @"
            SELECT DISTINCT c.Kit
            FROm CustCD c
            LEFT JOIN Orders o
            ON c.ID=o.CustCDID AND c.BrandID=@BrandID
            WHERE o.ID=@OrderId ";

            //主索引鍵：CustCD.ID+CustCD.BrandID

            bool res = MyUtility.Check.Seek(cmd, new List<SqlParameter> { new SqlParameter("@OrderId", this.CurrentDataRow["ID"]), new SqlParameter("@BrandID", this.CurrentDataRow["BrandID"]) }, out tmp, null);
            if (res && tmp["Kit"] != null)
            {

                this.displayKit.Text = tmp["Kit"].ToString();
            }
            else
                this.displayKit.Text = string.Empty;
        }
    
        private void GetIsDevSample()
        {
            string result = MyUtility.GetValue.Lookup($"SELECT ot.IsDevSample FROM OrderType ot INNER JOIN Orders o ON o.BrandID=ot.BrandID AND  o.OrderTypeID=ot.ID WHERE o.ID='{this.displaySPNo.Text}'");
            bool IsDevSample = false;
            if (result!="" && result.ToUpper()=="TRUE")
                IsDevSample = true;     
            this.txtDevSample.Text = IsDevSample ? "Y" : "";
        }

     private DataRow GetTitleDataByCustCD(string poid, string id, bool byCustCD = true)
        {
            DataRow drvar;
            string cmd = string.Empty;
            if (byCustCD)
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
            {
                return drvar;
            }
            else
            {
                return null;
            }
        }

        // M/Notice Sheet
     private void BtnMNoticeSheet_Click(object sender, EventArgs e)
     {
         if (this.CurrentMaintain["SMnorderApv"].ToString() == null || this.CurrentMaintain["SMnorderApv"].ToString() == string.Empty)
         {
             var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            var frm = new Sci.Production.PPIC.P01_MNoticePrint(null, dr["ID"].ToString());
             frm.ShowDialog(this);
             this.RenewData();
             return;
         }
         else
         {
             string poid = this.CurrentMaintain["POID"].ToString();
             SMNoticePrg.PrintSMNotice(poid, SMNoticePrg.EnuPrintSMType.Order);
         }
     }

        /// <summary>
        /// MoveSubBlockIntoMainSheet
        /// </summary>
        /// <param name="mainSheet">Worksheet mainSheet</param>
        /// <param name="rowPosition">int rowPosition</param>
        /// <param name="subBlockSheet">MsExcel.Worksheet subBlockSheet</param>
        /// <param name="blankRowsAfterThisBlock">int? blankRowsAfterThisBlock</param>
        public static void MoveSubBlockIntoMainSheet(Worksheet mainSheet, ref int rowPosition, MsExcel.Worksheet subBlockSheet, int? blankRowsAfterThisBlock = null)
        {
            // 把這個Block3完整複製過去主Sheet(參考rowPosition)
            var thisSheetUsedRange = subBlockSheet.UsedRange;
            (mainSheet.Rows[rowPosition] as MsExcel.Range).EntireRow.InsertIndent(thisSheetUsedRange.Rows.Count);

            var rowStart = thisSheetUsedRange.Rows[1].Row;
            var rowEnd = rowStart + thisSheetUsedRange.Rows.Count;

            // Full Row Copy for row height copy purpose
            subBlockSheet.Range[subBlockSheet.Rows[rowStart], subBlockSheet.Rows[rowEnd]].Copy();
            mainSheet.Range[mainSheet.Rows[rowPosition], mainSheet.Rows[rowPosition + thisSheetUsedRange.Rows.Count]].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);

            ////Range Copy for content & cell format  <-- because Range Copy ignore Row height copy, so I have to copy full rows before here
            // thisSheetUsedRange.Copy();
            // mainSheet.Cells[rowPosition, 1].PasteSpecial(MsExcel.XlPasteType.xlPasteAll, MsExcel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);

            // rowPosition遞移，給下一個區塊使用
            rowPosition += thisSheetUsedRange.Rows.Count + blankRowsAfterThisBlock.GetValueOrDefault(0); // 與下個Block空一行

            Marshal.ReleaseComObject(thisSheetUsedRange);
            thisSheetUsedRange = null;
        }

        // Q'ty b'down by schedule
        private void BtnQtyBdownbySchedule_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtySewingSchedule callNextForm = new Sci.Production.PPIC.P01_QtySewingSchedule(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"]));
            callNextForm.ShowDialog(this);
        }

        // Carton Status
        private void BtnCartonStatus_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CTNStatus callNextForm = new Sci.Production.PPIC.P01_CTNStatus(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), false);
            callNextForm.ShowDialog(this);
        }

        // Packing Method
        private void BtnPackingMethod_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["Packing"]), "Packing Method", false, null);
            callNextForm.ShowDialog(this);
        }

        // Pull forward remark
        private void BtnPullForwardRemark_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Remark from Order_PFHis WITH (NOLOCK) where Id = '{0}' order by AddDate desc", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))), "Pull Forward Remark");
        }

        // Shipment Finished
        private void BtnShipmentFinished_Click(object sender, EventArgs e)
        {
            // orders.CFMDate15天(包含)內的資料不能被關單
            System.Data.DataTable chkDt;
            DualResult result = DBProxy.Current.Select(null, $"select [Result] = 'SP:' + id + ' order CFMDate is ' + FORMAT(CFMDate,'yyyy/MM/dd') from dbo.orders WITH (NOLOCK) where POID = '{this.CurrentMaintain["POID"]}' and CFMDate >= convert(date,getdate()-15)", out chkDt);
            if (result == false)
            {
                this.ShowErr(result);
                return;
            }
            else
            {
                if (chkDt.Rows.Count > 0)
                {
                    MyUtility.Msg.WarningBox($"When Order CFMDate within 15 days ,you can't close it." + Environment.NewLine +
                        chkDt.AsEnumerable().Select(s => s["Result"].ToString()).ToList().JoinToString(Environment.NewLine));
                    return;
                }
            }

            string sqlCmd;
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Category"]) == "M" || MyUtility.Convert.GetString(this.CurrentMaintain["Category"]) == "T")
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from PO WITH (NOLOCK) where ID = '{0}' and Complete = 1", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]))))
                {
                    // Category = T的單子不用檢查是否已經Pullout Complete
                    string category_where = MyUtility.Convert.GetString(this.CurrentMaintain["Category"]) == "T" ? string.Empty : "or B.InQty <> B.OutQty - B.AdjustQty ";

                    sqlCmd = $@"select A.ID
                        from PO_Supp_Detail A WITH (NOLOCK) 
                        left join MDivisionPoDetail B WITH (NOLOCK) on B.POID=A.ID and B.Seq1=A.SEQ1 and B.Seq2=A.SEQ2
                        inner join dbo.Factory F WITH (NOLOCK) on F.id=A.factoryid and F.MDivisionID='{this.CurrentMaintain["MDivisionID"]}'
                        where A.ID = '{this.CurrentMaintain["POID"]}' and (A.Complete = 0  {category_where})";

                    if (MyUtility.Check.Seek(sqlCmd))
                    {
                        MyUtility.Msg.WarningBox("Warehouse still have material, so can't finish shipment.");
                        return;
                    }
                }
            }
            else
            {
                sqlCmd = string.Format("select (select ID+',' from Orders WITH (NOLOCK) where POID = '{0}' and Qty > 0 and PulloutComplete = 0 for xml path('')) as SP", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
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

            sqlCmd = string.Format("exec [dbo].usp_closeOrder '{0}','1'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Shipment finished fail !!" + result.ToString());
                return;
            }

            sqlCmd = string.Format("update sewingschedule set OrderFinished = 1 where OrderID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("update sewingschedule.OrderFinished Fail !!" + result.ToString());
                return;
            }

            this.ReloadDatas();
            this.RenewData();
        }

        // VAS/SHAS Instruction
        private void BtnVASSHASInstruction_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["Packing2"]), "VAS/SHAS Instruction", false, null);
            callNextForm.ShowDialog(this);
        }

        // Back to P01. PPIC Master List
        private void BtnBacktoPPICMasterList_Click(object sender, EventArgs e)
        {
            if (MyUtility.GetValue.Lookup(string.Format("select iif(WhseClose is null, 'TRUE','FALSE') as WHouseClose from Orders WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) == "FALSE")
            {
                MyUtility.Msg.WarningBox("W/House already closed R/mtl, so can not 'Back to P01. PPIC Master List'!!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.QuestionBox("Are you sure you want to 'Back to P01. PPIC Master List'?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string sqlCmd = string.Format("exec [dbo].usp_closeOrder '{0}','2'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("'Back to P01. PPIC Master List' fail!!" + result.ToString());
                return;
            }

            sqlCmd = string.Format("update sewingschedule set OrderFinished = 0 where OrderID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("update sewingschedule.OrderFinished Fail !!" + result.ToString());
                return;
            }

            this.ReloadDatas();
            this.RenewData();
        }

        // ShipMode
        private void EditShipMode_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (this.EditMode && MyUtility.Convert.GetString(this.CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                string sqlCmd = "select ID from ShipMode WITH (NOLOCK) where UseFunction like '%ORDER%' and Junk = 0";
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", this.editShipMode.Text, "Ship Mode");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["ShipModeList"] = item.GetSelectedString();
            }
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            // edit前檢查，非LOCAL單，不可修改
            if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalOrder"]).ToUpper() != "TRUE")
            {
                MyUtility.Msg.WarningBox("Only Local Order can edit !!", "Error");
                return false;
            }

            return base.ClickEditBefore();
        }

        private void BtnEachCons_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new EachConsumption(false, this.CurrentMaintain["id"].ToString(), null, null, false, true, false);
            frm.ShowDialog(this);
            this.OnDetailEntered();
        }

        private void Btneachconsprint_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            string iD = this.CurrentMaintain["ID"].ToString();
            var frm = new Print_OrderList(iD);
            frm.ShowDialog();
        }

        private void BtnMeterialStatus_Click(object sender, EventArgs e)
        {
            string fullpath = System.Windows.Forms.Application.StartupPath + ".\\Sci.Production.Warehouse.dll";
            var assemblys = Assembly.LoadFile(fullpath);
            var types = assemblys.GetTypes().ToList();
            var myClass = types.Where(x => x.FullName == "Sci.Production.Warehouse.P03").First();

            if (myClass != null)
            {
                var callMethod = myClass.GetMethod("Call");
                callMethod.Invoke(null, new object[] { this.CurrentMaintain["ID"].ToString(), this.MdiParent });
            }
        }

        private void BtnMeterialStatus_Local_Click(object sender, EventArgs e)
        {
            var fullpath = System.Windows.Forms.Application.StartupPath + ".\\Sci.Production.Warehouse.dll";
            var assemblys = Assembly.LoadFile(fullpath);
            var types = assemblys.GetTypes().ToList();
            var myClass = types.Where(x => x.FullName == "Sci.Production.Warehouse.P04").First();

            if (myClass != null)
            {
                var callMethod = myClass.GetMethod("Call");
                callMethod.Invoke(null, new object[] { this.CurrentMaintain["ID"].ToString() });
            }
        }

        private void DisCustCD_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.EditMode)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    string strSQLSelect = string.Format(
                @"select id,countryid,city from Custcd where brandid= '{0}' and junk=0 ", this.displayBrand.Text);

                    Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(strSQLSelect, "16,2,16", this.disCustCD.Text);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentMaintain["CustCDID"] = item.GetSelectedString();
                }
            }
        }

        private void BtnBuyerDeliveryHistory_Click(object sender, EventArgs e)
        {
            string orderID = this.CurrentMaintain["id"].ToString();
            P01_BuyerDeliveryHistory pb = new P01_BuyerDeliveryHistory("Order", "Orders", "OrdersBuyerDelivery", orderID);
            pb.ShowDialog(this);
        }

        private void btnExpectionFormRemark_Click(object sender, EventArgs e)
        {
            DataRow styleData;
            string sqlCmd = string.Format("select ExpectionFormRemark from Style WITH (NOLOCK) where Ukey = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["StyleUkey"]));
            if (MyUtility.Check.Seek(sqlCmd, out styleData))
            {
                Sci.Win.Tools.EditMemo form = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(styleData["ExpectionFormRemark"]), "Expection Form Remark", false, null);
                form.ShowDialog(this);
            }
        }

        private void btnPFHistory_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            var dlg = new PFHis(false, dr["ID"].ToString(), string.Empty, string.Empty, dr);
            dlg.ShowDialog();
            this.RenewData();
        }

        private void BtnEConsMNFailed_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["ID"]) && !MyUtility.Check.Empty(this.CurrentMaintain["POID"]))
            {
                var dlg = new P01_EConsMNoticeFailed(false, this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["POID"].ToString(), null, this.muustEmpty);
                dlg.ShowDialog();

                this.RenewData();
            }
        }
    }
}

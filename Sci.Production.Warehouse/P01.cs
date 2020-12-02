using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Data.SqlClient;
using Sci.Production.PublicForm;
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P01 : Win.Tems.Input1
    {
        private string dataType = string.Empty;

        private void OpenForm(Win.Forms.Base form)
        {
            form.MdiParent = this;
            form.Show();
            form.Focus();
        }

        /// <inheritdoc/>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("IsForecast = 0 and Whseclose is null");
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("0", string.Empty);
            comboBox1_RowSource.Add("1", "Subcon-in from sister factory (same M division)");
            comboBox1_RowSource.Add("2", "Subcon-in from sister factory (different M division)");
            comboBox1_RowSource.Add("3", "Subcon-in from non-sister factory");
            comboBox1_RowSource.Add(string.Empty, string.Empty);
            this.comboSubconInType.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboSubconInType.ValueMember = "Key";
            this.comboSubconInType.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        public P01(ToolStripMenuItem menuitem, string history)
            : this(menuitem)
        {
            this.Text = history != "Y" ? this.Text : this.Text + " (History)";
            this.btnCloseMTL.Text = history != "Y" ? this.btnCloseMTL.Text : "Re-Transfer Mtl. to Scrap";
            this.DefaultFilter = history != "Y" ? string.Format("IsForecast = 0 and Whseclose is null")
                : string.Format("IsForecast = 0 and Whseclose is not null");
            this.dataType = history;
            this.btnCloseMTL.Enabled = history != "Y";
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("0", "Fabric");
            comboBox1_RowSource.Add("1", "Subcon-in from sister factory (same zone)");
            comboBox1_RowSource.Add("2", "Subcon-in from sister factory (different zone)");
            comboBox1_RowSource.Add("3", "Subcon-in from non-sister factory");
            comboBox1_RowSource.Add(string.Empty, string.Empty);
            this.comboSubconInType.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboSubconInType.ValueMember = "Key";
            this.comboSubconInType.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        protected override void OnDetailDetached()
        {
            base.OnDetailDetached();
            this.ControlButton();
        }

        // 按鈕控制
        private void ControlButton()
        {
            this.btnProductionOutput.Enabled = this.CurrentMaintain != null;
            this.btnOrderRemark.Enabled = this.CurrentMaintain != null;
            this.btnLabelHangtag.Enabled = this.CurrentMaintain != null;
            this.btnQuantityBreakdown.Enabled = this.CurrentMaintain != null;
            this.btnArtwork.Enabled = this.CurrentMaintain != null;
            this.btnMaterialImport.Enabled = this.CurrentMaintain != null;
            this.btnEachConsumption.Enabled = this.CurrentMaintain != null;
            this.btnProductionKits.Enabled = this.CurrentMaintain != null;
            this.btnPackingMethod.Enabled = this.CurrentMaintain != null;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region 新增Batch Shipment Finished按鈕
            Win.UI.Button btnBatchClose = new Win.UI.Button();
            if (this.dataType == "Y")
            {
                btnBatchClose.Text = "Batch Re-Transfer Mtl. to Scrap";
                btnBatchClose.Size = new Size(260, 30); // 預設是(80,30)
            }
            else
            {
                btnBatchClose.Text = "Batch close R/MTL";
                btnBatchClose.Size = new Size(180, 30); // 預設是(80,30)
            }

            btnBatchClose.Click += new EventHandler(this.BtnBatchClose_Click);
            this.browsetop.Controls.Add(btnBatchClose);

            #endregion

        }

        private void BtnBatchClose_Click(object sender, EventArgs e)
        {
            if (this.dataType == "Y")
            {
                var frm = new P01_BatchReTransferMtlToScrap();
                frm.ShowDialog(this);
                this.ReloadDatas();
                this.RenewData();
            }
            else
            {
                var frm = new P01_BatchCloseRowMaterial();
                this.ShowWaitMessage("Data Loading....");
                frm.QueryData(true);
                this.HideWaitMessage();
                frm.ShowDialog(this);
                this.ReloadDatas();
                this.RenewData();
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (!this.EditMode)
            {
                this.ControlButton();
            }

            this.displayUpdateDeliveryReason.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Order_BuyerDelivery' and ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["KPIChangeReason"])));
            this.displaySpecialMark.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["SpecialMark"])));
            this.displayMTLCmpltSP.Value = MyUtility.Convert.GetString(this.CurrentMaintain["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : string.Empty;

            #region 填Description, Exception Form, Fty Remark, Style Apv欄位值
            DataRow styleData;
            string sqlCmd = string.Format("select Description,ExpectionForm,FTYRemark,ApvDate,ExpectionFormRemark from Style WITH (NOLOCK) where Ukey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["StyleUkey"]));
            if (MyUtility.Check.Seek(sqlCmd, out styleData))
            {
                this.displayDescription.Value = MyUtility.Convert.GetString(styleData["Description"]);
                this.checkExceptionForm.Value = MyUtility.Convert.GetString(styleData["ExpectionForm"]);
                this.editFtyRemark.Text = MyUtility.Convert.GetString(styleData["FTYRemark"]);

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
            sqlCmd = string.Format("select POSMR,POHandle from PO WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            if (MyUtility.Check.Seek(sqlCmd, out pOData))
            {
                this.txttpeuserPOSMR.DisplayBox1Binding = MyUtility.Convert.GetString(pOData["POSMR"]);
                this.txttpeuserPOHandle.DisplayBox1Binding = MyUtility.Convert.GetString(pOData["POHandle"]);
            }
            else
            {
                this.txttpeuserPOSMR.DisplayBox1Binding = string.Empty;
                this.txttpeuserPOHandle.DisplayBox1Binding = string.Empty;
            }
            #endregion
            #region 填PO Combo, Cutting Combo, MTLExport, PulloutComplete, Garment L/T欄位值
            DataTable ordersData;
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
                    this.displayRMTLETAMasterSP.Value = MyUtility.Convert.GetString(ordersData.Rows[0]["MTLExport"]);
                    this.displayActPullout.Value = MyUtility.Convert.GetString(ordersData.Rows[0]["PulloutComplete"]);
                }
                else
                {
                    this.editPOCombo.Text = string.Empty;
                    this.displayRMTLETAMasterSP.Value = string.Empty;
                    this.displayActPullout.Value = string.Empty;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query OrdersData fail!!" + result.ToString());
                this.editPOCombo.Text = string.Empty;
                this.displayRMTLETAMasterSP.Value = string.Empty;
                this.displayActPullout.Value = string.Empty;
            }
            #endregion
            bool lConfirm = PublicPrg.Prgs.GetAuthority(Env.User.UserID, "P01. PPIC Master List", "CanConfirm");

            // 按鈕變色
            bool haveTmsCost = MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            this.btnProductionOutput.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnOrderRemark.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;

            this.btnLabelHangtag.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["Label"]) ? Color.Blue : Color.Black;

            this.btnArtwork.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Artwork WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;

            this.btnMaterialImport.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Export_Detail WITH (NOLOCK) where PoID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;

            this.btnProductionKits.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"]))) ? Color.Blue : Color.Black;

            this.btnPackingMethod.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["Packing"]) ? Color.Blue : Color.Black;

            this.btnEachConsumption.ForeColor = MyUtility.Check.Seek(string.Format("select ID From dbo.Order_EachCons a WITH (NOLOCK)  where a.id ='{0}'", this.CurrentMaintain["id"].ToString())) ? Color.Blue : Color.Black;

            this.btnQuantityBreakdown.ForeColor = MyUtility.Check.Seek(string.Format(
                @"select oq.Article from Orders o WITH (NOLOCK) inner join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
where o.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;

            this.btnOrderRemark.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;

            this.btnPFHistory.ForeColor = MyUtility.Check.Seek($@"select id from Order_PFHis with(nolock) where id = '{this.CurrentMaintain["ID"]}'") ? Color.Blue : Color.Black;

            switch (this.CurrentMaintain["IsMixMarker"].ToString())
            {
                case "0":
                    this.displayIsMixMarker.Text = "Is Single Marker";
                    break;

                case "1":
                    this.displayIsMixMarker.Text = "Is Mix Marker";
                    break;

                case "2":
                    this.displayIsMixMarker.Text = "Is Mix Marker - SCI";
                    break;

                default:
                    this.displayIsMixMarker.Text = this.CurrentMaintain["IsMixMarker"].ToString();
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();

            // 帶入預設值
            this.CurrentMaintain["Category"] = "B";
            this.CurrentMaintain["LocalOrder"] = 1;
            this.CurrentMaintain["MCHandle"] = Env.User.UserID;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["FtyGroup"] = Env.User.Factory;
            this.CurrentMaintain["CMPUnit"] = "PCS";
            this.CurrentMaintain["CFMDate"] = DateTime.Today;
            this.CurrentMaintain["CtnType"] = "1";
            this.CurrentMaintain["CPUFactor"] = 1;
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["SubconInType"] = "0";
        }

        // Production output
        private void BtnProductionOutput_Click(object sender, EventArgs e)
        {
            PPIC.P01_ProductionOutput callNextForm = new PPIC.P01_ProductionOutput(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Order remark
        private void BtnOrderRemark_Click(object sender, EventArgs e)
        {
            Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["OrderRemark"]), "Order Remark", false, null);
            callNextForm.ShowDialog(this);
        }

        // Factory CMT
        private void Button5_Click(object sender, EventArgs e)
        {
            PPIC.P01_FactoryCMT callNextForm = new PPIC.P01_FactoryCMT(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Label & Hangtag
        private void BtnLabelHangtag_Click(object sender, EventArgs e)
        {
            Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["Label"]), "Label & Hangtag", false, null);
            callNextForm.ShowDialog(this);
        }

        // Shipping mark
        private void Button9_Click(object sender, EventArgs e)
        {
            PPIC.P01_ShippingMark callNextForm = new PPIC.P01_ShippingMark(false, this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // TMS & Cost
        private void Button10_Click(object sender, EventArgs e)
        {
            PPIC.P01_TMSAndCost callNextForm = new PPIC.P01_TMSAndCost(false, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), null, null);
            callNextForm.ShowDialog(this);
        }

        // Std.GSD List
        private void Button11_Click(object sender, EventArgs e)
        {
            StdGSDList callNextForm = new StdGSDList(MyUtility.Convert.GetLong(this.CurrentMaintain["StyleUKey"]));
            callNextForm.ShowDialog(this);
        }

        // Artwork
        private void BtnArtwork_Click(object sender, EventArgs e)
        {
            // Sci.Production.PPIC.P01_Artwork callNextForm = new Sci.Production.PPIC.P01_Artwork(false, MyUtility.Convert.GetString(CurrentMaintain["ID"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["StyleID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]));
            // callNextForm.ShowDialog(this);
            PPIC.P01_Artwork callNextForm = new PPIC.P01_Artwork(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        // Garment export
        private void Button15_Click(object sender, EventArgs e)
        {
            PPIC.P01_GMTExport callNextForm = new PPIC.P01_GMTExport(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        // Cutting Combo
        private void Button17_Click(object sender, EventArgs e)
        {
            PPIC.P01_CuttingCombo callNextForm = new PPIC.P01_CuttingCombo(MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        // Material Import
        private void Button19_Click(object sender, EventArgs e)
        {
            PPIC.P01_MTLImport callNextForm = new PPIC.P01_MTLImport(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Artwork Transaction List
        private void Button24_Click(object sender, EventArgs e)
        {
            PPIC.P01_ArtworkTrans callNextForm = new PPIC.P01_ArtworkTrans(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        // Production Kits
        private void Button25_Click(object sender, EventArgs e)
        {
            PPIC.P01_ProductionKit callNextForm = new PPIC.P01_ProductionKit(this.dataType == "Y" ? true : false, MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"]), null, null, MyUtility.Convert.GetString(this.CurrentMaintain["StyleID"]));
            callNextForm.ShowDialog(this);
        }

        // Q'ty b'down by schedule
        private void Button27_Click(object sender, EventArgs e)
        {
            PPIC.P01_QtySewingSchedule callNextForm = new PPIC.P01_QtySewingSchedule(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"]));
            callNextForm.ShowDialog(this);
        }

        // Carton Status
        private void Button28_Click(object sender, EventArgs e)
        {
            PPIC.P01_CTNStatus callNextForm = new PPIC.P01_CTNStatus(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), false);
            callNextForm.ShowDialog(this);
        }

        // Pull forward remark
        private void Button30_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Remark from Order_PFHis WITH (NOLOCK) where Id = '{0}' order by AddDate desc", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))), "Pull Forward Remark");
        }

        ////Shipment Finished
        // private void button31_Click(object sender, EventArgs e)
        // {
        //    string sqlCmd;
        //    if (MyUtility.Convert.GetString(CurrentMaintain["Category"]) == "M")
        //    {
        //        if (!MyUtility.Check.Seek(string.Format("select ID from PO WITH (NOLOCK) where ID = '{0}' and Complete = 1", MyUtility.Convert.GetString(CurrentMaintain["POID"]))))
        //        {
        //            if (MyUtility.Check.Seek(string.Format("select ID from PO_Supp_Detail WITH (NOLOCK) where ID = '{0}' and (ETA > GETDATE() or InQty <> OutQty - AdjustQty)", MyUtility.Convert.GetString(CurrentMaintain["POID"]))))
        //            {
        //                MyUtility.Msg.WarningBox("Warehouse still have material, so can't finish shipment.");
        //                return;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        sqlCmd = string.Format("select (select ID+',' from Orders WITH (NOLOCK) where POID = '{0}' and Qty > 0 and PulloutComplete = 0 for xml path('')) as SP", MyUtility.Convert.GetString(CurrentMaintain["POID"]));
        //        string spList = MyUtility.GetValue.Lookup(sqlCmd);
        //        if (!MyUtility.Check.Empty(spList))
        //        {
        //            MyUtility.Msg.WarningBox("Below combined SP# not yet ship!!\r\n" + spList.Substring(0, spList.Length - 1));
        //            return;
        //        }
        //    }

        // DialogResult buttonResult = MyUtility.Msg.QuestionBox("Are you sure you want to finish shipment?", "Warning", MessageBoxButtons.YesNo);
        //    if (buttonResult == System.Windows.Forms.DialogResult.No)
        //    {
        //        return;
        //    }
        //    sqlCmd = string.Format("exec [dbo].usp_closeOrder '{0}','1'", MyUtility.Convert.GetString(CurrentMaintain["POID"]));
        //    DualResult result = DBProxy.Current.Execute(null, sqlCmd);
        //    if (!result)
        //    {
        //        MyUtility.Msg.ErrorBox("Shipment finished fail!!" + result.ToString());
        //        return;
        //    }
        //    ReloadDatas();
        //    RenewData();
        // }

        // VAS/SHAS Instruction
        // private void button32_Click(object sender, EventArgs e)
        // {
        //    Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["Packing2"]), "VAS/SHAS Instruction", false, null);
        //    callNextForm.ShowDialog(this);
        // }

        // Back to P01. PPIC Master List
        // private void button33_Click(object sender, EventArgs e)
        // {
        //    if (MyUtility.GetValue.Lookup(string.Format("select iif(WhseClose is null, 'TRUE','FALSE') as WHouseClose from Orders WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) == "FALSE")
        //    {
        //        MyUtility.Msg.WarningBox("W/House already closed R/mtl, so can not 'Back to P01. PPIC Master List'!!");
        //        return;
        //    }

        // DialogResult buttonResult = MyUtility.Msg.QuestionBox("Are you sure you want to 'Back to P01. PPIC Master List'?", "Warning", MessageBoxButtons.YesNo);
        //    if (buttonResult == System.Windows.Forms.DialogResult.No)
        //    {
        //        return;
        //    }
        //    string sqlCmd = string.Format("exec [dbo].usp_closeOrder '{0}','2'", MyUtility.Convert.GetString(CurrentMaintain["POID"]));
        //    DualResult result = DBProxy.Current.Execute(null, sqlCmd);
        //    if (!result)
        //    {
        //        MyUtility.Msg.ErrorBox("'Back to P01. PPIC Master List' fail!!" + result.ToString());
        //        return;
        //    }
        //    ReloadDatas();
        //    RenewData();
        // }

        // Packing Method
        private void BtnPackingMethod_Click(object sender, EventArgs e)
        {
            Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["Packing"]), "Packing Method", false, null);
            callNextForm.ShowDialog(this);
        }

        // [Trim Card Print]
        private void BtnTrimCard_Click(object sender, EventArgs e)
        {
            // ID , StyleID , SeasonID , FactoryID
            P01_TrimCardPrint trimCardPrint = new P01_TrimCardPrint(
                this.CurrentMaintain["ID"].ToString().Trim(),
                this.CurrentMaintain["StyleID"].ToString().Trim(),
                this.CurrentMaintain["SeasonID"].ToString().Trim(),
                this.CurrentMaintain["FactoryID"].ToString().Trim(),
                this.CurrentMaintain["BrandID"].ToString().Trim(),
                this.CurrentMaintain["POID"].ToString().Trim());
            trimCardPrint.ShowDialog(this);
        }

        private void BtnCloseMTL_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            if (!MyUtility.Check.Seek(string.Format("select MDivisionID from dbo.Factory where ID='{0}' and MDivisionID='{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["FtyGroup"]), Env.User.Keyword)))
            {
                MyUtility.Msg.WarningBox("Insufficient permissions!!");
                return;
            }

            if (this.dataType != "Y")
            {
                #region 檢查B倉是否還有資料Lock
                bool existsFtyInventoryLock = MyUtility.Check.Seek($"select 1 from FtyInventory with (nolock) where POID = '{dr["poid"].ToString()}' and StockType='B' and Lock = 1");
                if (existsFtyInventoryLock)
                {
                    MyUtility.Msg.WarningBox("Still lock material. can not do close.");
                    return;
                }
                #endregion

                DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to close this R/Mtl?");
                if (dResult.ToString().ToUpper() == "NO")
                {
                    return;
                }

                DualResult result;
                #region store procedure parameters
                IList<SqlParameter> cmds = new List<SqlParameter>();
                SqlParameter sp_StocktakingID = new SqlParameter();
                sp_StocktakingID.ParameterName = "@poid";
                sp_StocktakingID.Value = dr["poid"].ToString().Trim();
                cmds.Add(sp_StocktakingID);
                SqlParameter sp_mdivision = new SqlParameter();
                sp_mdivision.ParameterName = "@MDivisionid";
                sp_mdivision.Value = Env.User.Keyword;
                cmds.Add(sp_mdivision);
                SqlParameter sp_factory = new SqlParameter();
                sp_factory.ParameterName = "@factoryid";
                sp_factory.Value = Env.User.Factory;
                cmds.Add(sp_factory);
                SqlParameter sp_loginid = new SqlParameter();
                sp_loginid.ParameterName = "@loginid";
                sp_loginid.Value = Env.User.UserID;
                cmds.Add(sp_loginid);
                #endregion
                if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.usp_WarehouseClose", cmds)))
                {
                    // MyUtility.Msg.WarningBox(result.Messages[1].ToString());
                    Exception ex = result.GetException();
                    MyUtility.Msg.InfoBox(ex.Message.Substring(ex.Message.IndexOf("Error Message:") + "Error Message:".Length));
                    return;
                }
                #region Sent W/H Fabric to Gensong

                // WHClose
                if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
                {
                    DataTable dtMain = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                    dtMain.ImportRow(this.CurrentMaintain);
                    Task.Run(() => new Gensong_AutoWHFabric().SentWHCloseToGensongAutoWHFabric(dtMain))
                   .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                }

                // SubTransfer_Detail
                if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
                {
                    DataTable dtMain = new DataTable();
                    dtMain.Columns.Add("ID", typeof(string));
                    dtMain.Columns.Add("Type", typeof(string));
                    DataRow row = dtMain.NewRow();
                    row["ID"] = this.CurrentMaintain["Poid"].ToString();
                    row["Type"] = "D";
                    dtMain.Rows.Add(row);
                    Task.Run(() => new Gensong_AutoWHFabric().SentSubTransfer_DetailToGensongAutoWHFabric(dtMain))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                }
                #endregion

                MyUtility.Msg.WarningBox("Finished!");
                this.ReloadDatas();
                this.RenewData();
            }
            else
            {
                var frm = new P01_ReTransferMtlToScrap(this.CurrentMaintain["poid"].ToString());
                frm.ShowDialog(this);
            }
        }

        // Quantity breakdown
        private void BtnQuantityBreakdown_Click(object sender, EventArgs e)
        {
            PPIC.P01_Qty callNextForm = new PPIC.P01_Qty(this.CurrentMaintain["id"].ToString(), this.CurrentMaintain["poid"].ToString(), this.editPOCombo.Text);
            callNextForm.ShowDialog(this);
        }

        private void BtnEachConsumption_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            var frm = new EachConsumption(false, this.CurrentMaintain["id"].ToString(), null, null, false, false, false);
            frm.ShowDialog(this);
        }

        private void BtnProductionKits_Click(object sender, EventArgs e)
        {
            PPIC.P01_ProductionKit callNextForm =
                new PPIC.P01_ProductionKit(false, this.CurrentMaintain["StyleUkey"].ToString(), null, null, null);
            callNextForm.ShowDialog(this);
        }

        private void BtnMaterialImport_Click(object sender, EventArgs e)
        {
            PPIC.P01_MTLImport callNextForm = new PPIC.P01_MTLImport(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        private void BtnMeterialStatus_Click(object sender, EventArgs e)
        {
            if (this.callP03 != null && this.callP03.Visible == true)
            {
                this.callP03.P03Data(this.CurrentMaintain["ID"].ToString());
                this.callP03.Activate();
            }
            else
            {
                this.P03FormOpen();
            }
        }

        private void BtnMeterialStatus_Local_Click(object sender, EventArgs e)
        {
            if (this.callP04 != null && this.callP04.Visible == true)
            {
                this.callP04.P04Data(this.CurrentMaintain["ID"].ToString());
            }
            else
            {
                this.P04FormOpen();
            }
        }

        private P03 callP03 = null;

        private void P03FormOpen()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is P03)
                {
                    form.Activate();
                    P03 activateForm = (P03)form;
                    activateForm.SetTxtSPNo(this.CurrentMaintain["ID"].ToString());
                    activateForm.Query();
                    return;
                }
            }

            ToolStripMenuItem p03MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Warehouse"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P03. Material Status"))
                            {
                                p03MenuItem = (ToolStripMenuItem)subMenuItem;
                                break;
                            }
                        }
                    }
                }
            }

            this.callP03 = new P03(this.CurrentMaintain["ID"].ToString(), p03MenuItem);
            this.callP03.MdiParent = this.MdiParent;
            this.callP03.Show();

            // 改到P03詢查相關的資料都要去檢查PPIC.P01 & WH / P01的[Material Status]
            this.callP03.P03Data(this.CurrentMaintain["ID"].ToString());
            this.callP03.ChangeDetailColor();
            #region BackUP

            // callP03.FormClosed += (s, e) =>
            // {
            // btnMeterialStatus.Enabled = true;
            // };
            #endregion
        }

        private P04 callP04 = null;

        private void P04FormOpen()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is P04)
                {
                    form.Activate();
                    P04 activateForm = (P04)form;
                    activateForm.SetTxtSPNo(this.CurrentMaintain["ID"].ToString());
                    activateForm.Event_Query();
                    return;
                }
            }

            ToolStripMenuItem p04MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Warehouse"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P04. Material Status (Local)"))
                            {
                                p04MenuItem = (ToolStripMenuItem)subMenuItem;
                                break;
                            }
                        }
                    }
                }
            }

            this.callP04 = new P04(this.CurrentMaintain["ID"].ToString(), p04MenuItem);
            this.callP04.MdiParent = this.MdiParent;
            this.callP04.Show();
        }

        private void BtnReCalculate_Click(object sender, EventArgs e)
        {
            // 母單批次Re-Cal
            DualResult result;
            DataTable dtPo;
            string sqlcmd = $@"
select po3.ID as Poid,po3.SEQ1,po3.SEQ2,mdp.Ukey 
from PO_Supp_Detail po3
left join MDivisionPoDetail mdp on mdp.POID=po3.ID
and mdp.Seq1=po3.SEQ1 and mdp.Seq2=po3.SEQ2
where po3.ID in (select poid from orders where id='{this.CurrentMaintain["id"]}')
and po3.junk=0
";
            if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd, out dtPo)))
            {
                this.ShowErr(result);
                return;
            }

            if (dtPo == null)
            {
                return;
            }

            int cnt = 1;

            foreach (DataRow dr in dtPo.Rows)
            {
                List<SqlParameter> listSQLParameter = new List<SqlParameter>();
                listSQLParameter.Add(new SqlParameter("@Ukey", dr["Ukey"]));
                listSQLParameter.Add(new SqlParameter("@Poid", dr["Poid"]));
                listSQLParameter.Add(new SqlParameter("@Seq1", dr["Seq1"]));
                listSQLParameter.Add(new SqlParameter("@Seq2", dr["Seq2"]));

                if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.usp_SingleItemRecaculate", listSQLParameter)))
                {
                    Exception ex = result.GetException();
                    MyUtility.Msg.WarningBox(ex.Message);
                    this.HideWaitMessage();
                    return;
                }

                this.ShowWaitMessage($"Data Processing.... ({cnt}/{dtPo.Rows.Count})");
                cnt++;
            }

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Finished!!");
        }

        private void BtnExpectionFormRemark_Click(object sender, EventArgs e)
        {
            DataRow styleData;
            string sqlCmd = string.Format("select ExpectionFormRemark from Style WITH (NOLOCK) where Ukey = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["StyleUkey"]));
            if (MyUtility.Check.Seek(sqlCmd, out styleData))
            {
                Win.Tools.EditMemo form = new Win.Tools.EditMemo(MyUtility.Convert.GetString(styleData["ExpectionFormRemark"]), "Expection Form Remark", false, null);
                form.ShowDialog(this);
            }
        }

        private void BtnPFHistory_Click(object sender, EventArgs e)
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
    }
}

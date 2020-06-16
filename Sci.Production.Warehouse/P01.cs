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
using System.Reflection;
using System.Data.SqlClient;
using Sci.Win;
using System.Linq;
using Sci.Production.PublicForm;
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.Warehouse
{
    public partial class P01 : Sci.Win.Tems.Input1
    {        
        private string dataType="";
        private void OpenForm(Sci.Win.Forms.Base form)
        {
            form.MdiParent = this;
            form.Show();
            form.Focus();
        }

        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("IsForecast = 0 and Whseclose is null");
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("0", "");
            comboBox1_RowSource.Add("1", "Subcon-in from sister factory (same M division)");
            comboBox1_RowSource.Add("2", "Subcon-in from sister factory (different M division)");
            comboBox1_RowSource.Add("3", "Subcon-in from non-sister factory");
            comboBox1_RowSource.Add(string.Empty, string.Empty);
            this.comboSubconInType.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboSubconInType.ValueMember = "Key";
            this.comboSubconInType.DisplayMember = "Value";
        }

        public P01(ToolStripMenuItem menuitem, string history)
            : this(menuitem)
        {
            this.Text = history != "Y" ? this.Text : this.Text + " (History)";
            btnCloseMTL.Text = history != "Y" ? btnCloseMTL.Text : "Re-Transfer Mtl. to Scrap";
            this.DefaultFilter = history != "Y" ? string.Format("IsForecast = 0 and Whseclose is null")
                : string.Format("IsForecast = 0 and Whseclose is not null");
            dataType = history;
            btnCloseMTL.Enabled = history != "Y";
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

        protected override void OnDetailDetached()
        {
            base.OnDetailDetached();
            ControlButton();
        }

        //按鈕控制
        private void ControlButton()
        {
            btnProductionOutput.Enabled = CurrentMaintain != null;
            btnOrderRemark.Enabled = CurrentMaintain != null;
            btnLabelHangtag.Enabled = CurrentMaintain != null;
            btnQuantityBreakdown.Enabled = CurrentMaintain != null;
            btnArtwork.Enabled = CurrentMaintain != null;
            btnMaterialImport.Enabled = CurrentMaintain != null;
            btnEachConsumption.Enabled = CurrentMaintain != null;
            btnProductionKits.Enabled = CurrentMaintain != null;
            btnPackingMethod.Enabled = CurrentMaintain != null;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region 新增Batch Shipment Finished按鈕
            Sci.Win.UI.Button btnBatchClose = new Sci.Win.UI.Button();
            if (this.dataType == "Y")
            {
                btnBatchClose.Text = "Batch Re-Transfer Mtl. to Scrap";
                btnBatchClose.Size = new Size(260, 30);//預設是(80,30)
            }
            else
            {
                btnBatchClose.Text = "Batch close R/MTL";
                btnBatchClose.Size = new Size(180, 30);//預設是(80,30)
            }
            btnBatchClose.Click += new EventHandler(btnBatchClose_Click);
            browsetop.Controls.Add(btnBatchClose);
           
            #endregion

        }

        private void btnBatchClose_Click(object sender, EventArgs e)
        {
            if (this.dataType == "Y")
            {
                var frm = new Sci.Production.Warehouse.P01_BatchReTransferMtlToScrap();
                frm.ShowDialog(this);
                ReloadDatas();
                this.RenewData();
            }
            else
            {
                var frm = new Sci.Production.Warehouse.P01_BatchCloseRowMaterial();
                this.ShowWaitMessage("Data Loading....");
                frm.QueryData(true);
                this.HideWaitMessage();
                frm.ShowDialog(this);
                ReloadDatas();
                this.RenewData();
            }
        }

        protected override void OnDetailEntered()
        {

            base.OnDetailEntered();
            if (!EditMode)
            {
                ControlButton();
            }

            displayUpdateDeliveryReason.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Order_BuyerDelivery' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["KPIChangeReason"])));
            displaySpecialMark.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["SpecialMark"])));
            displayMTLCmpltSP.Value = MyUtility.Convert.GetString(CurrentMaintain["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : "";

            #region 填Description, Exception Form, Fty Remark, Style Apv欄位值
            DataRow StyleData;
            string sqlCmd = string.Format("select Description,ExpectionForm,FTYRemark,ApvDate,ExpectionFormRemark from Style WITH (NOLOCK) where Ukey = {0}", MyUtility.Convert.GetString(CurrentMaintain["StyleUkey"]));
            if (MyUtility.Check.Seek(sqlCmd, out StyleData))
            {
                displayDescription.Value = MyUtility.Convert.GetString(StyleData["Description"]);
                checkExceptionForm.Value = MyUtility.Convert.GetString(StyleData["ExpectionForm"]);
                editFtyRemark.Text = MyUtility.Convert.GetString(StyleData["FTYRemark"]);

                if (MyUtility.Check.Empty(StyleData["ExpectionFormRemark"]))
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
                displayDescription.Value = "";
                checkExceptionForm.Value = "false";
                editFtyRemark.Text = "";
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
                txttpeuserPOSMR.DisplayBox1Binding = MyUtility.Convert.GetString(POData["POSMR"]);
                txttpeuserPOHandle.DisplayBox1Binding = MyUtility.Convert.GetString(POData["POHandle"]);
            }
            else
            {
                txttpeuserPOSMR.DisplayBox1Binding = "";
                txttpeuserPOHandle.DisplayBox1Binding = "";
            }
            #endregion
            #region 填PO Combo, Cutting Combo, MTLExport, PulloutComplete, Garment L/T欄位值
            DataTable OrdersData;
            sqlCmd = string.Format(@"select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList,
isnull([dbo].getCuttingComboList(o.ID,o.CuttingSP),'') as CuttingList,
isnull([dbo].getMTLExport(o.POID,o.MTLExport),'') as MTLExport,
isnull([dbo].getPulloutComplete(o.ID,o.PulloutComplete),'') as PulloutComplete,
isnull([dbo].getGarmentLT(o.StyleUkey,o.FactoryID),0) as GMTLT from Orders o WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out OrdersData);
            if (result)
            {
                if (OrdersData.Rows.Count > 0)
                {
                    editPOCombo.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["PoList"]);
                    displayRMTLETAMasterSP.Value = MyUtility.Convert.GetString(OrdersData.Rows[0]["MTLExport"]);
                    displayActPullout.Value = MyUtility.Convert.GetString(OrdersData.Rows[0]["PulloutComplete"]);
                }
                else
                {
                    editPOCombo.Text = "";
                    displayRMTLETAMasterSP.Value = "";
                    displayActPullout.Value = "";
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query OrdersData fail!!" + result.ToString());
                editPOCombo.Text = "";
                displayRMTLETAMasterSP.Value = "";
                displayActPullout.Value = "";
            }
            #endregion
            bool lConfirm = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P01. PPIC Master List", "CanConfirm");

            //按鈕變色
            bool haveTmsCost = MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])));

            btnProductionOutput.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            btnOrderRemark.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;

            btnLabelHangtag.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Label"]) ? Color.Blue : Color.Black;


            btnArtwork.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Artwork WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;

            btnMaterialImport.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Export_Detail WITH (NOLOCK) where PoID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;

            btnProductionKits.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]))) ? Color.Blue : Color.Black;
            
            btnPackingMethod.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing"]) ? Color.Blue : Color.Black;

            btnEachConsumption.ForeColor = MyUtility.Check.Seek(string.Format("select ID From dbo.Order_EachCons a WITH (NOLOCK)  where a.id ='{0}'", CurrentMaintain["id"].ToString())) ? Color.Blue : Color.Black;

            btnQuantityBreakdown.ForeColor = MyUtility.Check.Seek(string.Format(@"select oq.Article from Orders o WITH (NOLOCK) inner join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
where o.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;

            btnOrderRemark.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;

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

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
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
            CurrentMaintain["SubconInType"] = "0";
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
        private void button5_Click(object sender, EventArgs e)
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
        
        //Shipping mark
        private void button9_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ShippingMark callNextForm = new Sci.Production.PPIC.P01_ShippingMark(false, CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //TMS & Cost
        private void button10_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_TMSAndCost callNextForm = new Sci.Production.PPIC.P01_TMSAndCost(false, MyUtility.Convert.GetString(CurrentMaintain["ID"]), null, null);
            callNextForm.ShowDialog(this);
        }

        //Std.GSD List
        private void button11_Click(object sender, EventArgs e)
        {
            Sci.Production.PublicForm.StdGSDList callNextForm = new Sci.Production.PublicForm.StdGSDList(MyUtility.Convert.GetLong(CurrentMaintain["StyleUKey"]));
            callNextForm.ShowDialog(this);
        }

        //Artwork
        private void btnArtwork_Click(object sender, EventArgs e)
        {
            //Sci.Production.PPIC.P01_Artwork callNextForm = new Sci.Production.PPIC.P01_Artwork(false, MyUtility.Convert.GetString(CurrentMaintain["ID"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["StyleID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]));
            //callNextForm.ShowDialog(this);

            Sci.Production.PPIC.P01_Artwork callNextForm = new Sci.Production.PPIC.P01_Artwork(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        //Garment export
        private void button15_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_GMTExport callNextForm = new Sci.Production.PPIC.P01_GMTExport(MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        //Cutting Combo
        private void button17_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CuttingCombo callNextForm = new Sci.Production.PPIC.P01_CuttingCombo(MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        //Material Import
        private void button19_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_MTLImport callNextForm = new Sci.Production.PPIC.P01_MTLImport(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Artwork Transaction List
        private void button24_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ArtworkTrans callNextForm = new Sci.Production.PPIC.P01_ArtworkTrans(MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        //Production Kits
        private void button25_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionKit callNextForm = new Sci.Production.PPIC.P01_ProductionKit(dataType == "Y" ? true : false, MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["StyleID"]));
            callNextForm.ShowDialog(this);
        }

        //Q'ty b'down by schedule
        private void button27_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtySewingSchedule callNextForm = new Sci.Production.PPIC.P01_QtySewingSchedule(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]));
            callNextForm.ShowDialog(this);
        }

        //Carton Status
        private void button28_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CTNStatus callNextForm = new Sci.Production.PPIC.P01_CTNStatus(MyUtility.Convert.GetString(CurrentMaintain["ID"]), false);
            callNextForm.ShowDialog(this);
        }

        //Pull forward remark
        private void button30_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Remark from Order_PFHis WITH (NOLOCK) where Id = '{0}' order by AddDate desc", MyUtility.Convert.GetString(CurrentMaintain["ID"]))), "Pull Forward Remark");
        }

        ////Shipment Finished
        //private void button31_Click(object sender, EventArgs e)
        //{
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

        //    DialogResult buttonResult = MyUtility.Msg.QuestionBox("Are you sure you want to finish shipment?", "Warning", MessageBoxButtons.YesNo);
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
        //}

        //VAS/SHAS Instruction
        //private void button32_Click(object sender, EventArgs e)
        //{
        //    Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["Packing2"]), "VAS/SHAS Instruction", false, null);
        //    callNextForm.ShowDialog(this);
        //}

        //Back to P01. PPIC Master List
        //private void button33_Click(object sender, EventArgs e)
        //{
        //    if (MyUtility.GetValue.Lookup(string.Format("select iif(WhseClose is null, 'TRUE','FALSE') as WHouseClose from Orders WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) == "FALSE")
        //    {
        //        MyUtility.Msg.WarningBox("W/House already closed R/mtl, so can not 'Back to P01. PPIC Master List'!!");
        //        return;
        //    }

        //    DialogResult buttonResult = MyUtility.Msg.QuestionBox("Are you sure you want to 'Back to P01. PPIC Master List'?", "Warning", MessageBoxButtons.YesNo);
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
        //}

        //Packing Method
        private void btnPackingMethod_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["Packing"]), "Packing Method", false, null);
            callNextForm.ShowDialog(this);
        }

        //[Trim Card Print]
        private void btnTrimCard_Click(object sender, EventArgs e)
        {
            //ID , StyleID , SeasonID , FactoryID
            P01_TrimCardPrint TrimCardPrint = new P01_TrimCardPrint(CurrentMaintain["ID"].ToString().Trim()
                                                                  , CurrentMaintain["StyleID"].ToString().Trim()
                                                                  , CurrentMaintain["SeasonID"].ToString().Trim()
                                                                  , CurrentMaintain["FactoryID"].ToString().Trim()
                                                                  , CurrentMaintain["BrandID"].ToString().Trim()
                                                                  , CurrentMaintain["POID"].ToString().Trim());
            TrimCardPrint.ShowDialog(this);
        }

        private void btnCloseMTL_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            if (!MyUtility.Check.Seek(string.Format("select MDivisionID from dbo.Factory where ID='{0}' and MDivisionID='{1}'", MyUtility.Convert.GetString(CurrentMaintain["FtyGroup"]), Sci.Env.User.Keyword)))
            {
                MyUtility.Msg.WarningBox("Insufficient permissions!!");
                return;
            }

            if (dataType != "Y")
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
                if (dResult.ToString().ToUpper() == "NO") return;

                DualResult result;
                #region store procedure parameters
                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                System.Data.SqlClient.SqlParameter sp_StocktakingID = new System.Data.SqlClient.SqlParameter();
                sp_StocktakingID.ParameterName = "@poid";
                sp_StocktakingID.Value = dr["poid"].ToString().Trim();
                cmds.Add(sp_StocktakingID);
                System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
                sp_mdivision.ParameterName = "@MDivisionid";
                sp_mdivision.Value = Sci.Env.User.Keyword;
                cmds.Add(sp_mdivision);
                System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
                sp_factory.ParameterName = "@factoryid";
                sp_factory.Value = Sci.Env.User.Factory;
                cmds.Add(sp_factory);
                System.Data.SqlClient.SqlParameter sp_loginid = new System.Data.SqlClient.SqlParameter();
                sp_loginid.ParameterName = "@loginid";
                sp_loginid.Value = Sci.Env.User.UserID;
                cmds.Add(sp_loginid);
                #endregion
                if (!(result = DBProxy.Current.ExecuteSP("", "dbo.usp_WarehouseClose", cmds)))
                {
                    //MyUtility.Msg.WarningBox(result.Messages[1].ToString()); 
                    Exception ex =  result.GetException();
                    MyUtility.Msg.InfoBox(ex.Message.Substring(ex.Message.IndexOf("Error Message:") + "Error Message:".Length));
                    return;
                }
                MyUtility.Msg.WarningBox("Finished!");
                ReloadDatas();
                RenewData();
            }
            else
            {
                var frm = new P01_ReTransferMtlToScrap(CurrentMaintain["poid"].ToString());
                frm.ShowDialog(this);
            }

            #region Sent WHClose to Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                string strPOID = CurrentMaintain["PoID"].ToString();
                Task.Run(() => new Gensong_AutoWHFabric().SentWHCloseToGensongAutoWHFabric(strPOID))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
            #endregion
        }

        //Quantity breakdown
        private void btnQuantityBreakdown_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(CurrentMaintain["id"].ToString(), CurrentMaintain["poid"].ToString(), editPOCombo.Text);
            callNextForm.ShowDialog(this);
        }

        private void btnEachConsumption_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain; if (null == dr) return;
            var frm = new Sci.Production.PublicForm.EachConsumption(false, CurrentMaintain["id"].ToString(), null, null, false, false,false);
            frm.ShowDialog(this);
        }

        private void btnProductionKits_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionKit callNextForm =
                new Sci.Production.PPIC.P01_ProductionKit(false, CurrentMaintain["StyleUkey"].ToString(), null, null, null);
            callNextForm.ShowDialog(this);
        }

        private void btnMaterialImport_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_MTLImport callNextForm = new Sci.Production.PPIC.P01_MTLImport(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        private void btnMeterialStatus_Click(object sender, EventArgs e)
        {
            if (callP03 != null && callP03.Visible == true)
            {
                callP03.P03Data(CurrentMaintain["ID"].ToString());
                callP03.Activate();
            }
            else
            {
                P03FormOpen();
            }  
        }

        private void btnMeterialStatus_Local_Click(object sender, EventArgs e)
        {
            if (callP04 != null && callP04.Visible == true)
            {
                callP04.P04Data(CurrentMaintain["ID"].ToString());
            }
            else
            {
                P04FormOpen();
            }       
        }        

        Sci.Production.Warehouse.P03 callP03 = null;
        private void P03FormOpen()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Sci.Production.Warehouse.P03)
                {
                    form.Activate();
                    Sci.Production.Warehouse.P03 activateForm = (Sci.Production.Warehouse.P03)form;
                    activateForm.setTxtSPNo (CurrentMaintain["ID"].ToString());
                    activateForm.Query();
                    return;
                }
            }

            ToolStripMenuItem P03MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Sci.Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Warehouse"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(System.Windows.Forms.ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P03. Material Status"))
                            {
                                P03MenuItem = ((ToolStripMenuItem)subMenuItem);
                                break;
                            }
                        }
                    }
                }
            }

            callP03 = new P03(CurrentMaintain["ID"].ToString(), P03MenuItem);            
            callP03.MdiParent = MdiParent;                      
            callP03.Show();
            //改到P03詢查相關的資料都要去檢查PPIC.P01 & WH / P01的[Material Status]
            callP03.P03Data(CurrentMaintain["ID"].ToString());
            callP03.ChangeDetailColor();
            #region BackUP
            //callP03.FormClosed += (s, e) =>
            //{
            //btnMeterialStatus.Enabled = true;
            //};
            #endregion
        }

        Sci.Production.Warehouse.P04 callP04 = null;
        private void P04FormOpen()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Sci.Production.Warehouse.P04)
                {
                    form.Activate();
                    Sci.Production.Warehouse.P04 activateForm = (Sci.Production.Warehouse.P04)form;
                    activateForm.setTxtSPNo(CurrentMaintain["ID"].ToString());
                    activateForm.event_Query();
                    return;
                }
            }

            ToolStripMenuItem P04MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Sci.Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Warehouse"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(System.Windows.Forms.ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P04. Material Status (Local)"))
                            {
                                P04MenuItem = ((ToolStripMenuItem)subMenuItem);
                                break;
                            }
                        }
                    }
                }
            }

            callP04 = new P04(CurrentMaintain["ID"].ToString(), P04MenuItem);
            callP04.MdiParent = MdiParent;
            callP04.Show();
        }

        private void btnReCalculate_Click(object sender, EventArgs e)
        {
            // 母單批次Re-Cal
            DualResult result;
            DataTable dtPo;
            string sqlcmd = $@"
select po3.ID as Poid,po3.SEQ1,po3.SEQ2,mdp.Ukey 
from PO_Supp_Detail po3
left join MDivisionPoDetail mdp on mdp.POID=po3.ID
and mdp.Seq1=po3.SEQ1 and mdp.Seq2=po3.SEQ2
where po3.ID in (select poid from orders where id='{CurrentMaintain["id"]}')
and po3.junk=0
";
            if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd, out dtPo)))
            {
                ShowErr(result);
                return;
            }
            if (dtPo == null) return;
            int cnt = 1;
            
            foreach (DataRow dr in dtPo.Rows)
            {
                List<SqlParameter> listSQLParameter = new List<SqlParameter>();
                listSQLParameter.Add(new SqlParameter("@Ukey", dr["Ukey"]));
                listSQLParameter.Add(new SqlParameter("@Poid", dr["Poid"]));
                listSQLParameter.Add(new SqlParameter("@Seq1", dr["Seq1"]));
                listSQLParameter.Add(new SqlParameter("@Seq2", dr["Seq2"]));
                
                if (!(result = DBProxy.Current.ExecuteSP("", "dbo.usp_SingleItemRecaculate", listSQLParameter)))
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
    }
}

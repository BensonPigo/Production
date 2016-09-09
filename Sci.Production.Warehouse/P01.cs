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


namespace Sci.Production.Warehouse
{
    public partial class P01 : Sci.Win.Tems.Input1
    {
        private string dataType="";

        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("IsForecast = 0 and Whseclose is null and mdivisionid='{0}'", Sci.Env.User.Keyword);
        }

        public P01(ToolStripMenuItem menuitem, string history)
            : this(menuitem)
        {
            this.Text = history != "Y" ? this.Text : this.Text += " (History)";
            this.DefaultFilter = history != "Y" ? string.Format("IsForecast = 0 and Whseclose is null and mdivisionid='{0}'", Sci.Env.User.Keyword)
                : string.Format("IsForecast = 0 and Whseclose is not null and mdivisionid='{0}'", Sci.Env.User.Keyword);
            dataType = history;
            btnCloseMTL.Enabled = history != "Y";

        }

        protected override void OnDetailDetached()
        {
            base.OnDetailDetached();
            ControlButton();
        }

        //按鈕控制
        private void ControlButton()
        {
            button3.Enabled = CurrentMaintain != null;
            button4.Enabled = CurrentMaintain != null;
            button6.Enabled = CurrentMaintain != null;
            button8.Enabled = CurrentMaintain != null;
            button14.Enabled = CurrentMaintain != null;
            button19.Enabled = CurrentMaintain != null;
            button23.Enabled = CurrentMaintain != null;
            button25.Enabled = CurrentMaintain != null;
            button29.Enabled = CurrentMaintain != null;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region 新增Batch Shipment Finished按鈕
            Sci.Win.UI.Button btnBatchClose = new Sci.Win.UI.Button();
            btnBatchClose.Text = "Batch Shipment Finished";
            btnBatchClose.Click += new EventHandler(btnBatchClose_Click);
            browsetop.Controls.Add(btnBatchClose);
            btnBatchClose.Size = new Size(180, 30);//預設是(80,30)
            btnBatchClose.Visible = dataType != "Y";
            #endregion
            MyUtility.Tool.SetupCombox(cbbCategory, 2, 1, "B,Bulk,S,Sample,M,Material");
        }

        private void btnBatchClose_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P01_BatchCloseRowMaterial();
            frm.ShowDialog(this);
            this.RenewData();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (!EditMode)
            {
                ControlButton();
            }

            displayBox6.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Order_BuyerDelivery' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["KPIChangeReason"])));
            displayBox14.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["SpecialMark"])));
            displayBox17.Value = MyUtility.Convert.GetString(CurrentMaintain["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : "";

            #region 填Description, Exception Form, Fty Remark, Style Apv欄位值
            DataRow StyleData;
            string sqlCmd = string.Format("select Description,ExpectionForm,FTYRemark,ApvDate from Style where Ukey = {0}", MyUtility.Convert.GetString(CurrentMaintain["StyleUkey"]));
            if (MyUtility.Check.Seek(sqlCmd, out StyleData))
            {
                displayBox5.Value = MyUtility.Convert.GetString(StyleData["Description"]);
                checkBox11.Value = MyUtility.Convert.GetString(StyleData["ExpectionForm"]);
                editBox3.Text = MyUtility.Convert.GetString(StyleData["FTYRemark"]);

            }
            else
            {
                displayBox5.Value = "";
                checkBox11.Value = "false";
                editBox3.Text = "";
            }
            #endregion
            #region 填Buyer欄位值, 修改Special id1, Special id2, Special id3顯示值
            DataRow brandData;
            if (MyUtility.Check.Seek(string.Format("select ID,Customize1,Customize2,Customize3,BuyerID from Brand where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["BrandID"])), out brandData))
            {
                displayBox2.Value = MyUtility.Convert.GetString(brandData["BuyerID"]);
                label32.Text = MyUtility.Convert.GetString(brandData["Customize1"]);
                label33.Text = MyUtility.Convert.GetString(brandData["Customize2"]);
                label34.Text = MyUtility.Convert.GetString(brandData["Customize3"]);
            }
            else
            {
                displayBox2.Value = "";
                label32.Text = "";
                label33.Text = "";
                label34.Text = "";
            }
            #endregion
            #region 填PO SMR, PO Handle欄位值
            DataRow POData;
            sqlCmd = string.Format("select POSMR,POHandle from PO where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]));
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
            #region 填PO Combo, Cutting Combo, MTLExport, PulloutComplete, Garment L/T欄位值
            DataTable OrdersData;
            sqlCmd = string.Format(@"select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList,
isnull([dbo].getCuttingComboList(o.ID,o.CuttingSP),'') as CuttingList,
isnull([dbo].getMTLExport(o.POID,o.MTLExport),'') as MTLExport,
isnull([dbo].getPulloutComplete(o.ID,o.PulloutComplete),'') as PulloutComplete,
isnull([dbo].getGarmentLT(o.StyleUkey,o.FactoryID),0) as GMTLT from Orders o where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out OrdersData);
            if (result)
            {
                if (OrdersData.Rows.Count > 0)
                {
                    editBox2.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["PoList"]);
                    editBox4.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["CuttingList"]);
                    displayBox18.Value = MyUtility.Convert.GetString(OrdersData.Rows[0]["MTLExport"]);
                    displayBox20.Value = MyUtility.Convert.GetString(OrdersData.Rows[0]["PulloutComplete"]);
                }
                else
                {
                    editBox2.Text = "";
                    editBox4.Text = "";
                    displayBox18.Value = "";
                    displayBox20.Value = "";
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query OrdersData fail!!" + result.ToString());
                editBox2.Text = "";
                editBox4.Text = "";
                displayBox18.Value = "";
                displayBox20.Value = "";
            }
            #endregion
            bool lConfirm = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P01. PPIC Master List", "CanConfirm");

            //按鈕變色
            bool haveTmsCost = MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            button3.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button4.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;

            button6.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Label"]) ? Color.Blue : Color.Black;


            button14.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Artwork where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;

            button19.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Export_Detail where PoID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;

            button25.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]))) ? Color.Blue : Color.Black;


            button29.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing"]) ? Color.Blue : Color.Black;

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
        }

        //Production output
        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput callNextForm = new Sci.Production.PPIC.P01_ProductionOutput(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Order remark
        private void button4_Click(object sender, EventArgs e)
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
        private void button6_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["Label"]), "Label & Hangtag", false, null);
            callNextForm.ShowDialog(this);
        }

        //Quantity breakdown
        private void button8_Click(object sender, EventArgs e)
        {

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
        private void button14_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_Artwork callNextForm = new Sci.Production.PPIC.P01_Artwork(false, MyUtility.Convert.GetString(CurrentMaintain["ID"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["StyleID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]));
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
            MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Remark from Order_PFHis where Id = '{0}' order by AddDate desc", MyUtility.Convert.GetString(CurrentMaintain["ID"]))), "Pull Forward Remark");
        }

        //Shipment Finished
        private void button31_Click(object sender, EventArgs e)
        {
            string sqlCmd;
            if (MyUtility.Convert.GetString(CurrentMaintain["Category"]) == "M")
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from PO where ID = '{0}' and Complete = 1", MyUtility.Convert.GetString(CurrentMaintain["POID"]))))
                {
                    if (MyUtility.Check.Seek(string.Format("select ID from PO_Supp_Detail where ID = '{0}' and (ETA > GETDATE() or InQty <> OutQty - AdjustQty)", MyUtility.Convert.GetString(CurrentMaintain["POID"]))))
                    {
                        MyUtility.Msg.WarningBox("Warehouse still have material, so can't finish shipment.");
                        return;
                    }
                }
            }
            else
            {
                sqlCmd = string.Format("select (select ID+',' from Orders where POID = '{0}' and Qty > 0 and PulloutComplete = 0 for xml path('')) as SP", MyUtility.Convert.GetString(CurrentMaintain["POID"]));
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
            sqlCmd = string.Format("exec [dbo].usp_closeOrder '{0}','1'", MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Shipment finished fail!!" + result.ToString());
                return;
            }
            ReloadDatas();
            RenewData();
        }

        //VAS/SHAS Instruction
        private void button32_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["Packing2"]), "VAS/SHAS Instruction", false, null);
            callNextForm.ShowDialog(this);
        }

        //Back to P01. PPIC Master List
        private void button33_Click(object sender, EventArgs e)
        {
            if (MyUtility.GetValue.Lookup(string.Format("select iif(WhseClose is null, 'TRUE','FALSE') as WHouseClose from Orders where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) == "FALSE")
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

        //Packing Method
        private void button29_Click(object sender, EventArgs e)
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
                                                                  , CurrentMaintain["BrandID"].ToString().Trim());
            TrimCardPrint.ShowDialog(this);
        }

        private void btnCloseMTL_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            if (dataType != "Y")
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to close this R/Mtl?");
                if (dResult.ToString().ToUpper() == "NO") return;

                DualResult result;
                #region store procedure parameters
                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                System.Data.SqlClient.SqlParameter sp_StocktakingID = new System.Data.SqlClient.SqlParameter();
                sp_StocktakingID.ParameterName = "@poid";
                sp_StocktakingID.Value = dr["poid"].ToString();
                cmds.Add(sp_StocktakingID);
                System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
                sp_mdivision.ParameterName = "@MDivisionid";
                sp_mdivision.Value = Sci.Env.User.Keyword;
                cmds.Add(sp_mdivision);
                System.Data.SqlClient.SqlParameter sp_loginid = new System.Data.SqlClient.SqlParameter();
                sp_loginid.ParameterName = "@loginid";
                sp_loginid.Value = Sci.Env.User.UserID;
                cmds.Add(sp_loginid);
                #endregion
                if (!(result = DBProxy.Current.ExecuteSP("", "dbo.usp_WarehouseClose", cmds)))
                {
                    //MyUtility.Msg.WarningBox(result.Messages[1].ToString()); 
                    Exception ex = result.GetException();
                    MyUtility.Msg.WarningBox(ex.Message);
                    return;
                }
            }
            else
            {
                var frm = new Sci.Win.Tems.Input6(null);
                frm = new Sci.Production.Warehouse.P25(null, CurrentMaintain["poid"].ToString());
                frm.ShowDialog(this);
            }
        }
 
    }
}

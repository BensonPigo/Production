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
//using CSCHEMAS = Sci.Production.Report.Order.Schemas.P01;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using Sci.Utility.Excel;
using Sci.Utility.Drawing;


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
            button31.Visible = dataType == "1"; //Shipment Finished
            button33.Visible = dataType != "1"; //Back to P01. PPIC Master List
         
        }
     
            
        
        protected override void OnDetailDetached()
        {
            base.OnDetailDetached();
            ControlButton();
        }
     
        //按鈕控制
        private void ControlButton()
        {
            button1.Enabled = CurrentMaintain != null;
            button2.Enabled = CurrentMaintain != null;
            button3.Enabled = CurrentMaintain != null;
            button4.Enabled = CurrentMaintain != null;
            button5.Enabled = CurrentMaintain != null;
            button6.Enabled = CurrentMaintain != null;
            button7.Enabled = CurrentMaintain != null;
            button8.Enabled = CurrentMaintain != null;
            button9.Enabled = CurrentMaintain != null;
            button10.Enabled = CurrentMaintain != null;
            button11.Enabled = CurrentMaintain != null;
            button12.Enabled = CurrentMaintain != null;
            button13.Enabled = CurrentMaintain != null;
            button14.Enabled = CurrentMaintain != null;
            button15.Enabled = CurrentMaintain != null;
            button16.Enabled = CurrentMaintain != null;
            button17.Enabled = CurrentMaintain != null;
            button18.Enabled = CurrentMaintain != null;
            button19.Enabled = CurrentMaintain != null;
            button20.Enabled = CurrentMaintain != null;
            button21.Enabled = CurrentMaintain != null;
            button24.Enabled = CurrentMaintain != null;
            button25.Enabled = CurrentMaintain != null;
            button26.Enabled = CurrentMaintain != null;
            button27.Enabled = CurrentMaintain != null;
            button28.Enabled = CurrentMaintain != null;
            button29.Enabled = CurrentMaintain != null;
            button30.Enabled = CurrentMaintain != null;
            button31.Enabled = CurrentMaintain != null;
            button32.Enabled = CurrentMaintain != null;
            button33.Enabled = CurrentMaintain != null;
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
            displayBox28.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Order_reMakeSample' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["SampleReason"])));
            displayBox6.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Order_BuyerDelivery' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["KPIChangeReason"])));
            displayBox14.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["SpecialMark"])));
            numericBox6.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["CPU"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["CPUFactor"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["Qty"]), 3);
            displayBox17.Value = MyUtility.Convert.GetString(CurrentMaintain["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : "";
            displayBox22.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Delivery_OutStand' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["OutstandingReason"])));
            displayBox23.Value = MyUtility.Check.Empty(CurrentMaintain["OutstandingDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["OutstandingDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                numericBox7.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["PoPrice"]);
                label44.Text = "/PCS";
                numericBox8.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["PoPrice"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["Qty"]), 3);
                tooltip.SetToolTip(numericBox8, MyUtility.Convert.GetString(CurrentMaintain["PoPrice"]) + " * " + MyUtility.Convert.GetString(CurrentMaintain["Qty"]));
            }
            else
            {
                numericBox7.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["CMPPrice"]);
                label44.Text = "/" + MyUtility.Convert.GetString(CurrentMaintain["CMPUnit"]);
                numericBox8.Value = numericBox6.Value;
                tooltip.SetToolTip(numericBox8, MyUtility.Convert.GetString(CurrentMaintain["CPU"]) + " * " + MyUtility.Convert.GetString(CurrentMaintain["CPUFactor"]) + " * " + MyUtility.Convert.GetString(CurrentMaintain["Qty"]));
            }
            #region 填Description, Exception Form, Fty Remark, Style Apv欄位值
            DataRow StyleData;
            string sqlCmd= string.Format("select Description,ExpectionForm,FTYRemark,ApvDate from Style where Ukey = {0}", MyUtility.Convert.GetString(CurrentMaintain["StyleUkey"]));
            if (MyUtility.Check.Seek(sqlCmd, out StyleData))
            {
                displayBox5.Value = MyUtility.Convert.GetString(StyleData["Description"]);
                checkBox11.Value = MyUtility.Convert.GetString(StyleData["ExpectionForm"]);
                editBox3.Text = MyUtility.Convert.GetString(StyleData["FTYRemark"]);
                if (MyUtility.Check.Empty(StyleData["ApvDate"]))
                {
                    dateBox26.Value = null;
                }
                else
                {
                    dateBox26.Value = MyUtility.Convert.GetDate(StyleData["ApvDate"]);
                }
            }
            else
            {
                displayBox5.Value = "";
                checkBox11.Value = "false";
                editBox3.Text = "";
                dateBox26.Value = null;
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
            System.Data.DataTable OrdersData;
            sqlCmd = string.Format(@"select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList,
isnull([dbo].getCuttingComboList(o.ID,o.CuttingSP),'') as CuttingList,
isnull([dbo].getMTLExport(o.POID,o.MTLExport),'') as MTLExport,
isnull([dbo].getPulloutComplete(o.ID,o.PulloutComplete),'') as PulloutComplete,
isnull([dbo].getGarmentLT(o.StyleUkey,o.FactoryID),0) as GMTLT from Orders o where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null,sqlCmd, out OrdersData);
            if (result)
            {
                if (OrdersData.Rows.Count > 0)
                {
                    editBox2.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["PoList"]);
                    editBox4.Text = MyUtility.Convert.GetString(OrdersData.Rows[0]["CuttingList"]);
                    displayBox18.Value = MyUtility.Convert.GetString(OrdersData.Rows[0]["MTLExport"]);
                    displayBox20.Value = MyUtility.Convert.GetString(OrdersData.Rows[0]["PulloutComplete"]);
                    numericBox10.Value = MyUtility.Convert.GetDecimal(OrdersData.Rows[0]["GMTLT"]);
                }
                else
                {
                    editBox2.Text = "";
                    editBox4.Text = "";
                    displayBox18.Value = "";
                    displayBox20.Value = "";
                    numericBox10.Value = 0;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query OrdersData fail!!" + result.ToString());
                editBox2.Text = "";
                editBox4.Text = "";
                displayBox18.Value = "";
                displayBox20.Value = "";
                numericBox10.Value = 0;
            }
            #endregion
            bool lConfirm = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P01. PPIC Master List", "CanConfirm");
            button1.Enabled = CurrentMaintain != null && dataType == "1" && lConfirm && !EditMode;
            button2.Enabled = CurrentMaintain != null && dataType == "1" && lConfirm && !EditMode;
            button18.Enabled = CurrentMaintain != null && MyUtility.Convert.GetString(CurrentMaintain["CtnType"]) == "2" && !EditMode;

            //按鈕變色
            bool haveTmsCost = MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            button3.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button4.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;
            button5.ForeColor = haveTmsCost ? Color.Blue : Color.Black;
            button6.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Label"]) ? Color.Blue : Color.Black;
            button7.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button8.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button9.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["MarkFront"]) || !MyUtility.Check.Empty(CurrentMaintain["MarkBack"]) || !MyUtility.Check.Empty(CurrentMaintain["MarkLeft"]) || !MyUtility.Check.Empty(CurrentMaintain["MarkRight"]) ? Color.Blue : Color.Black;
            button10.ForeColor = haveTmsCost ? Color.Blue : Color.Black;
            button11.ForeColor = MyUtility.Check.Seek(string.Format("select i.ID from Style s, IETMS i where s.Ukey = {0} and s.IETMSID = i.ID and s.IETMSVersion = i.Version", MyUtility.Convert.GetString(CurrentMaintain["StyleUkey"]))) && MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button12.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["CMPQRemark"]) ? Color.Blue : Color.Black;
            button13.ForeColor = MyUtility.Check.Seek(string.Format("select ID from orders where Junk = 0 and POID='{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;
            button14.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Artwork where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button15.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button17.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["CuttingSP"]) ? Color.Blue : Color.Black;
            if (MyUtility.Convert.GetString(CurrentMaintain["CtnType"]) == "2")
            {
                button18.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyCTN where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            }
            button19.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Export_Detail where PoID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;
            button20.ForeColor = MyUtility.Check.Seek(string.Format("select ID from FIR where PoID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;
            button21.ForeColor = MyUtility.Check.Seek(string.Format("select ID from AIR where PoID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["POID"]))) ? Color.Blue : Color.Black;
            button24.ForeColor = MyUtility.Check.Seek(string.Format("select ID from ArtworkPO_Detail where OrderID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button25.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]))) ? Color.Blue : Color.Black;
            button26.ForeColor = MyUtility.Check.Seek(string.Format("select ID FROM MNOrder where POID = '{0}' and CustCDID = (select CustCDID from Orders where ID = '{1}')", MyUtility.Convert.GetString(CurrentMaintain["POID"]), MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button27.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["SewLine"]) ? Color.Blue : Color.Black;
            button28.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail where OrderID = '{0}' and ReceiveDate is not null", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button29.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing"]) ? Color.Blue : Color.Black;
            button30.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_PFHis where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            button32.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing2"]) ? Color.Blue : Color.Black;
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
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
                textBox8.ReadOnly = true;
                checkBox2.ReadOnly = true;
                checkBox4.ReadOnly = true;
                checkBox6.ReadOnly = true;
                checkBox7.ReadOnly = true;
                checkBox9.ReadOnly = true;
                checkBox12.ReadOnly = true;
                txtuser1.TextBox1.ReadOnly = true;
                txtuser2.TextBox1.ReadOnly = true;
                dateBox1.ReadOnly = true;
                dateBox2.ReadOnly = true;
                dateBox3.ReadOnly = true;
                dateBox4.ReadOnly = true;
                txtcountry1.TextBox1.ReadOnly = true;
                txtcurrency1.ReadOnly = true;
                numericBox3.ReadOnly = true;
                numericBox11.ReadOnly = true;
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
                    textBox4.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
                {
                    MyUtility.Msg.WarningBox("Brand can't empty!!");
                    displayBox10.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["CustPONo"]))
                {
                    MyUtility.Msg.WarningBox("PO No. can't empty!!");
                    textBox3.Focus();
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
                    displayBox12.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["BuyerDelivery"]))
                {
                    MyUtility.Msg.WarningBox("Buyer Delivery can't empty!!");
                    dateBox1.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["SCIDelivery"]))
                {
                    MyUtility.Msg.WarningBox("SCI Delivery# can't empty!!");
                    dateBox3.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["SDPDate"]))
                {
                    MyUtility.Msg.WarningBox("Cut off date can't empty!!");
                    dateBox4.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["FactoryID"]))
                {
                    MyUtility.Msg.WarningBox("Factory can't empty!!");
                    txtmfactory1.Focus();
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
                    numericBox5.Focus();
                    return false;
                }
                if (MyUtility.Convert.GetString(CurrentMaintain["FOC"]).ToUpper() == "FALSE" && MyUtility.Check.Empty(CurrentMaintain["PoPrice"]))
                {
                    MyUtility.Msg.WarningBox("Unit Price can't empty!!");
                    numericBox3.Focus();
                    return false;
                }

                if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE" && MyUtility.Check.Empty(CurrentMaintain["ShipModeList"]))
                {
                    MyUtility.Msg.WarningBox("Ship Mode can't empty!!");
                    editBox1.Focus();
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
                    string sqlCmd = "select ID from SCIFty where ID = @programid";
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out SCIFtyData);
                    if (result && SCIFtyData.Rows.Count > 0)
                    {
                        CurrentMaintain["SubconInSisterFty"] = 1;
                    }
                }

                //Buyer Delivery：要先檢查Order_QtyShip是否有資料，若有，就要填入最小的Buyer Deliver
                DataRow OrderShip;
                if (MyUtility.Check.Seek(string.Format("select MIN(BuyerDelivery) as BuyerDelivery from Order_QtyShip where Id = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])), out OrderShip))
                {
                    if (!MyUtility.Check.Empty(OrderShip["BuyerDelivery"]))
                    {
                        CurrentMaintain["BuyerDelivery"] = OrderShip["BuyerDelivery"];
                    } 
                }
            }

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", CurrentMaintain["ID"].ToString(), "Orders", "ID") + "LO", "Orders", DateTime.Today, 2, "Id", null);
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
                if (!MyUtility.Check.Seek(string.Format("select ID from Order_Artwork where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
                {
                    insertCmd = string.Format(@"insert into Order_Artwork(ID,ArtworkTypeID,Article,PatternCode,PatternDesc,ArtworkID,ArtworkName,TMS,Qty,Price,Cost,Remark,AddName,AddDate,Ukey)
select '{0}',ArtworkTypeID,Article,PatternCode,PatternDesc,ArtworkID,ArtworkName,TMS,Qty,Price,Cost,Remark,'{1}',GETDATE(),(select min(Ukey)-1 from Order_Artwork) from Style_Artwork where StyleUkey = {2}",
    MyUtility.Convert.GetString(CurrentMaintain["ID"]), Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["StyleUkey"]));

                    result = DBProxy.Current.Execute(null, insertCmd);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Save Order_Artwork fail!!\r\n" + result.ToString());
                        return failResult;
                    }
                }

                if (!MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
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
                if (MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE" && MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
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
        private void textBox4_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            IList<DataRow> StyleData;
            string sqlCmd = "select ID,SeasonID,BrandID,Description,CdCodeID,CPU,StyleUnit,Ukey from Style where Junk = 0 and LocalStyle = 1";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "15,8,10,28,5,7,7,6", "", "Style,Season,Brand,Description,CdCode,CPU,Unit,Ukey",columndecimals:"0,0,0,0,0,3,0,0");
            item.Size = new System.Drawing.Size(950, 500);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                setStyleEmptyColumn();
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
                displayBox5.Value = MyUtility.Convert.GetString(StyleData[0]["Description"]);
            }
        }

        //Style
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && textBox4.OldValue != textBox4.Text)
            {
                if (MyUtility.Check.Empty(textBox4.Text))
                {
                    setStyleEmptyColumn();
                }
                else
                {
                    //檢查資料是否存在
                    //sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@styleid", textBox4.Text);

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);

                    System.Data.DataTable StyleData;
                    string sqlCmd = "select ID,SeasonID,BrandID,Description,CdCodeID,CPU,StyleUnit,Ukey from Style where Junk = 0 and LocalStyle = 1 and ID = @styleid";
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
                        displayBox5.Value = MyUtility.Convert.GetString(StyleData.Rows[0]["Description"]);
                    }
                }
            }
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
            displayBox5.Value = "";
        }

        //Factory
        private void txtmfactory1_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtmfactory1.OldValue != txtmfactory1.Text)
            {
                if (MyUtility.Check.Empty(txtmfactory1.Text))
                {
                    CurrentMaintain["FtyGroup"] = "";
                }
                else
                {
                    CurrentMaintain["FtyGroup"] = MyUtility.GetValue.Lookup("FTYGroup",txtmfactory1.Text,"Orders","ID");
                }
            }
        }

        //Cancelled Order
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (EditMode && checkBox4.Checked && !MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                if (MyUtility.Check.Seek(string.Format("select ID from SewingOutput_Detail where OrderId = '{0}' and QAQty > 0",MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
                {
                    MyUtility.Msg.WarningBox("This record had sewing daily output, can't cancel!!");
                    CurrentMaintain["Junk"] = 0;
                }
            }
        }

        //F.O.C.
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (EditMode)
            {
                CurrentMaintain["FOC"] = checkBox6.Checked;
                numericBox3.ReadOnly = checkBox6.Checked;
                if (checkBox6.Checked)
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
        private void button1_Click(object sender, EventArgs e)
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
        private void button2_Click(object sender, EventArgs e)
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

        //Q'ty b'down by shipmode
        private void button7_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyShip callNextForm = new Sci.Production.PPIC.P01_QtyShip(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        //Quantity breakdown
        private void button8_Click(object sender, EventArgs e)
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
                Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["POID"]), editBox2.Text);
                callNextForm.ShowDialog(this);
            }
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

        //CMPQ remark
        private void button12_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["CMPQRemark"]), "CMPQ Remark", false, null);
            callNextForm.ShowDialog(this);
        }
        
        //CMPQ Sheet
        private void button13_Click(object sender, EventArgs e)
        { 
            this.ShowWaitMessage("Data processing, please wait ...");
            string poid = CurrentMaintain["POID"].ToString(); //MyUtility.GetValue.Lookup("select POID FROM dbo.Orders where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", CurrentMaintain["POID"]) });
           
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
,o.Qty * o.CPU * r.Rate as amount
,o.packing ,o.label ,o.packing2
,Mark=iif(MarkFront<>'','(A) '+@newLine+MarkFront,'')
+@newLine+iif(MarkBack<>'','(B) '+@newLine+MarkBack,'')
+@newLine+iif(MarkLeft<>'','(C) '+@newLine+MarkLeft,'')
+@newLine+iif(MarkRight<>'','(D) '+@newLine+MarkRight,'')
from Orders o WITH (NOLOCK)  inner join Factory fty WITH (NOLOCK)  ON o.FactoryID = fty.ID
LEFT join Country cty WITH (NOLOCK)  ON o.Dest = cty.ID
LEFT JOIN PaytermAR par WITH (NOLOCK)  ON o.PayTermARID = par.ID
LEFT JOIN Style sty WITH (NOLOCK)  ON o.StyleUkey = sty.Ukey
OUTER APPLY (SELECT RateCost as pgRate FROM Program pg WHERE pg.BrandID = o.BrandID and pg.ID = o.ProgramID) pgr
OUTER APPLY (SELECT CpuRate as otRate FROM OrderType ot WHERE ot.BrandID = o.BrandID and ot.ID = o.OrderTypeID) otr
outer apply (select iif(isnull(pgRate,0) > isnull(otRate,0), pgRate, iif(otRate > 1, otRate, 1)) as Rate) r
where o.Junk = 0 and o.POID= @POID order by o.ID
", new List<SqlParameter> { new SqlParameter("@ID", CurrentMaintain["ID"]), new SqlParameter("@POID", poid) }, out rpt3);
                
                if (!res) return;
                
                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_CMPQ.xltx");

                sxrc sxr = new sxrc(xltPath);
                int idx = 0;
                sxr.CopySheet.Add(1, rpt3.Rows.Count - 1);
                sxr.VarToSheetName = sxr._v + "SP";

                foreach (DataRow row in rpt3.Rows)
                {
                    string sIdx = (idx == 0) ? "" : idx.ToString();
                    idx += 1;
                    string oid = row["ID"].ToString();
                    sxr.dicDatas.Add(sxr._v + "Title" + sIdx, row["Title"].ToString());
                    sxr.dicDatas.Add(sxr._v + "AbbEN" + sIdx, row["AbbEN"].ToString());
                    sxr.dicDatas.Add(sxr._v + "SP" + sIdx, oid);
                    sxr.dicDatas.Add(sxr._v + "Style" + sIdx, row["StyleID"].ToString());
                    sxr.dicDatas.Add(sxr._v + "name" + sIdx, row["name"].ToString());
                    sxr.dicDatas.Add(sxr._v + "addressen" + sIdx, row["AddressEN"].ToString());
                    sxr.dicDatas.Add(sxr._v + "tel" + sIdx, row["Tel"].ToString());
                    sxr.dicDatas.Add(sxr._v + "fax" + sIdx, row["Fax"].ToString());
                    sxr.dicDatas.Add(sxr._v + "remark" + sIdx, row["remark"].ToString());
                    sxr.dicDatas.Add(sxr._v + "season" + sIdx, row["SeasonID"].ToString());
                    sxr.dicDatas.Add(sxr._v + "delivery" + sIdx, row["delivery"].ToString());
                    sxr.dicDatas.Add(sxr._v + "des" + sIdx, row["des"].ToString());
                    sxr.dicDatas.Add(sxr._v + "terms" + sIdx, row["terms"].ToString());
                    sxr.dicDatas.Add(sxr._v + "styleno" + sIdx, row["StyleID"].ToString());
                    sxr.dicDatas.Add(sxr._v + "qty" + sIdx, row["QTY"].ToString());
                    sxr.dicDatas.Add(sxr._v + "descripition" + sIdx, row["descripition"].ToString());
                    sxr.dicDatas.Add(sxr._v + "price" + sIdx, row["price"].ToString());
                    sxr.dicDatas.Add(sxr._v + "amount" + sIdx, row["amount"].ToString());

                    System.Data.DataTable[] dts;
                    res = DBProxy.Current.SelectSP("", "PPIC_Report03", new List<SqlParameter> { new SqlParameter("@OrderID", oid), new SqlParameter("@ByType", 0) }, out dts);

                    if (!res) continue;
                    if (dts.Length < 3) continue;

                    sxrc.xltRptTable tbl1 = new sxrc.xltRptTable(dts[0], 1, 2, true);
                    sxrc.xltRptTable tbl2 = new sxrc.xltRptTable(dts[1], 1, 3);
                    sxrc.xltRptTable tbl3 = new sxrc.xltRptTable(dts[2], 1, 0);
                    SetColumn1toText(tbl1);
                    SetColumn1toText(tbl2);
                    SetColumn1toText(tbl3);
                    sxr.dicDatas.Add(sxr._v + "qtybreakdown" + sIdx, tbl1);
                    sxr.dicDatas.Add(sxr._v + "fabcom" + sIdx, tbl2);
                    sxr.dicDatas.Add(sxr._v + "acccom" + sIdx, tbl3);

                    sxr.dicDatas.Add(sxr._v + "shipmark" + sIdx, new sxrc.xltImageString(row["mark"].ToString()));
                    sxr.dicDatas.Add(sxr._v + "paching" + sIdx, new sxrc.xltImageString(row["packing"].ToString()));
                    sxr.dicDatas.Add(sxr._v + "labelhantag" + sIdx, new sxrc.xltImageString(row["label"].ToString()));
                    string UserName;
                    UserPrg.GetName(Env.User.UserID, out UserName, UserPrg.NameType.idAndNameAndExt);
                    sxr.dicDatas.Add(sxr._v + "userid" + sIdx, UserName);

                }
                sxr.Save();
                this.HideWaitMessage();
            }
        void SetColumn1toText(sxrc.xltRptTable tbl)
        {
            sxrc.xlsColumnInfo c1 = new sxrc.xlsColumnInfo(1);
            c1.NumberFormate = "@";
            tbl.lisColumnInfo.Add(c1);
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

        //Sewing Inline History
        private void button16_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = new Win.UI.ShowHistory("Order_History", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "SewInOffLine", caption: "History SP#", dataType: "D");
            callNextForm.ShowDialog(this);
        }

        //Cutting Combo
        private void button17_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CuttingCombo callNextForm = new Sci.Production.PPIC.P01_CuttingCombo(MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        //b'down
        private void button18_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyCTN callNextForm = new Sci.Production.PPIC.P01_QtyCTN(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Material Import
        private void button19_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_MTLImport callNextForm = new Sci.Production.PPIC.P01_MTLImport(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Fabric inspection list
        private void button20_Click(object sender, EventArgs e)
        {
            Sci.Production.Quality.P01 callNextForm = new Sci.Production.Quality.P01(MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        //Accessory inspection list
        private void button21_Click(object sender, EventArgs e)
        {
            Sci.Production.Quality.P02 callNextForm = new Sci.Production.Quality.P02(MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        //Each Cons. Print
        private void button22_Click(object sender, EventArgs e)
        {

        }

        //Each Consumption
        private void button23_Click(object sender, EventArgs e)
        {
            

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
            Sci.Production.PPIC.P01_ProductionKit callNextForm = new Sci.Production.PPIC.P01_ProductionKit(dataType == "1" ? true : false, MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["StyleID"]));
            callNextForm.ShowDialog(this);
        }

        private int intSizeSpecRowCnt = 0;
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

SELECT MAKER=max(FactoryID),sty=max(StyleID)+'-'+max(SeasonID),QTY=sum(QTY),'SPNO'=RTRIM(POID)+b.spno FROM MNOrder a
OUTER APPLY(SELECT STUFF((SELECT '/'+REPLACE(ID,@poid,'') FROM MNOrder WHERE POID = @poid AND CustCDID = (select CustCDID from MNOrder where ID = @ID) 
	order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno) b
where POID = @poid and CustCDID = (select CustCDID from MNOrder where ID = @ID) group by POID,b.spno";
            }
            else
            {
                cmd = @"SELECT MAKER=max(FactoryID),sty=max(StyleID)+'-'+max(SeasonID),QTY=sum(QTY),'SPNO'=RTRIM(POID)+b.spno FROM MNOrder a
OUTER APPLY(SELECT STUFF((SELECT '/'+REPLACE(ID,@poid,'') FROM MNOrder WHERE POID = @poid
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
        private void button26_Click(object sender, EventArgs e)
        {
              string _id = CurrentMaintain["id"].ToString();

              string poid = CurrentMaintain["POID"].ToString(); //MyUtility.GetValue.Lookup("select POID FROM dbo.Orders where ID = @ID", new List<SqlParameter> { new SqlParameter("@ID", _id) });

                DataRow drvar = GetTitleDataByCustCD(poid, _id);

                if (drvar == null) return ;

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "PPIC_P01_M_Notice.xltx");
                sxrc sxr = new sxrc(xltPath);
                sxr.dicDatas.Add(sxr._v + "Now", DateTime.Now);
                sxr.dicDatas.Add(sxr._v + "PO_MAKER", drvar["MAKER"].ToString());
                sxr.dicDatas.Add(sxr._v + "PO_STYLENO", drvar["sty"].ToString());
                sxr.dicDatas.Add(sxr._v + "PO_QTY", drvar["QTY"]);
                sxr.dicDatas.Add(sxr._v + "POID", poid);

                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "PPIC_Report_SizeSpec", new List<SqlParameter> { new SqlParameter("@POID", poid), new SqlParameter("@WithZ", false), new SqlParameter("@fullsize", 1) }, out dts);
                
                sxrc.xltRptTable xltTbl = new sxrc.xltRptTable(dts[0], 1, 0, false, 18, 2);
                for (int i = 3; i <= 18; i++)
                {
                    sxrc.xlsColumnInfo xcinfo = new sxrc.xlsColumnInfo(i, false, 0, XlHAlign.xlHAlignLeft);
                    xcinfo.NumberFormate = "@";
                    xltTbl.lisColumnInfo.Add(xcinfo);
                }

                sxr.dicDatas.Add(sxr._v + "S1_Tbl1", xltTbl);
                intSizeSpecRowCnt = dts[0].Rows.Count + 1 + 2; //起始位置加一、格線加二
                sxrc.ReplaceAction ra = ForSizeSpec;
                sxr.dicDatas.Add(sxr._v + "ExtraAction", ra);

                System.Data.DataTable dt;
                DualResult getIds = DBProxy.Current.Select("", "select ID, FactoryID as MAKER, StyleID+'-'+SeasonID as sty, QTY from MNorder where poid = @poid", new List<SqlParameter> { new SqlParameter("poid", poid) }, out dt);
                if (!getIds && dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.ErrorBox(getIds.ToString(), "error");
                    return ;
                }

                sxr.CopySheet.Add(2, dt.Rows.Count - 1);
                sxr.VarToSheetName = sxr._v + "SP";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string ID = dt.Rows[i]["ID"].ToString();
                    string idxStr = (i == 0) ? "" : i.ToString();

                    res = DBProxy.Current.SelectSP("", "PPIC_Report02", new List<SqlParameter> { new SqlParameter("@ID", ID), new SqlParameter("@WithZ", false) }, out dts);

                    sxr.dicDatas.Add(sxr._v + "SP" + idxStr, ID);
                    sxr.dicDatas.Add(sxr._v + "MAKER" + idxStr, dt.Rows[i]["MAKER"].ToString());
                    sxr.dicDatas.Add(sxr._v + "STYLENO" + idxStr, dt.Rows[i]["sty"].ToString());
                    sxr.dicDatas.Add(sxr._v + "QTY" + idxStr, dt.Rows[i]["QTY"].ToString());

                    sxrc.xltRptTable tbl1 = new sxrc.xltRptTable(dts[0], 1, 2, true);
                    sxrc.xltRptTable tbl2 = new sxrc.xltRptTable(dts[1], 1, 3);
                    sxrc.xltRptTable tbl3 = new sxrc.xltRptTable(dts[2], 1, 0);
                    SetColumn1toText(tbl1);
                    SetColumn1toText(tbl2);
                    SetColumn1toText(tbl3);

                    sxr.dicDatas.Add(sxr._v + "S2_Tbl1" + idxStr, tbl1);
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl2" + idxStr, tbl2);
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl3" + idxStr, tbl3);
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl4" + idxStr, dts[3]); //COLOR list
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl5" + idxStr, dts[4]); //Fabric list
                    sxr.dicDatas.Add(sxr._v + "S2_Tbl6" + idxStr, dts[5]); //Accessories list
                    sxr.dicDatas.Add(sxr._v + "S2SHIPINGMARK" + idxStr, new sxrc.xltImageString(dts[6].Rows[0]["shipingMark"].ToString()));
                    sxr.dicDatas.Add(sxr._v + "S2PACKING" + idxStr, new sxrc.xltImageString(dts[7].Rows[0]["Packing"].ToString()));
                    sxr.dicDatas.Add(sxr._v + "S2LH" + idxStr, new sxrc.xltImageString(dts[8].Rows[0]["Label"].ToString()));

                }           
                sxr.boOpenFile = true;
                sxr.Save();
            }

          
        //Q'ty b'down by schedule
        private void button27_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtySewingSchedule callNextForm = new Sci.Production.PPIC.P01_QtySewingSchedule(MyUtility.Convert.GetString(CurrentMaintain["ID"]),MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]));
            callNextForm.ShowDialog(this);
        }

        //Carton Status
        private void button28_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CTNStatus callNextForm = new Sci.Production.PPIC.P01_CTNStatus(MyUtility.Convert.GetString(CurrentMaintain["ID"]), false);
            callNextForm.ShowDialog(this);
        }

        //Packing Method
        private void button29_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["Packing"]), "Packing Method", false, null);
            callNextForm.ShowDialog(this);
        }

        //Pull forward remark
        private void button30_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Remark from Order_PFHis where Id = '{0}' order by AddDate desc", MyUtility.Convert.GetString(CurrentMaintain["ID"]))),"Pull Forward Remark");

        }

        //Shipment Finished
        private void button31_Click(object sender, EventArgs e)
        {
            string sqlCmd;
            if (MyUtility.Convert.GetString(CurrentMaintain["Category"]) == "M")
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from PO where ID = '{0}' and Complete = 1", MyUtility.Convert.GetString(CurrentMaintain["POID"]))))
                {
                    sqlCmd = string.Format(@"select ID
                                            from PO_Supp_Detail A
                                            left join MDivisionPoDetail B on B.MDivisionID='{0}' and B.POID=A.ID and B.Seq1=A.SEQ1 and B.Seq2=A.SEQ2
                                            where ID = '{1}' and (ETA > GETDATE() or B.InQty <> B.OutQty - B.AdjustQty)"
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
        private void button32_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["Packing2"]), "VAS/SHAS Instruction", false, null);
            callNextForm.ShowDialog(this);
        }

        //Back to P01. PPIC Master List
        private void button33_Click(object sender, EventArgs e)
        {
            if (MyUtility.GetValue.Lookup(string.Format("select iif(WhseClose is null, 'TRUE','FALSE') as WHouseClose from Orders where ID = '{0}'",MyUtility.Convert.GetString(CurrentMaintain["ID"]))) == "FALSE")
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
        private void editBox1_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalOrder"]).ToUpper() == "TRUE")
            {
                string sqlCmd = "select ID from ShipMode where UseFunction like '%ORDER%' and Junk = 0";
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10",editBox1.Text,"Ship Mode");
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


    }
}

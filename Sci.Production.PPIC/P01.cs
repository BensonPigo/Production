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


namespace Sci.Production.PPIC
{
    public partial class P01 : Sci.Win.Tems.Input1
    {
        private string dataType;
        public P01(ToolStripMenuItem menuitem, string Type)
            : base(menuitem)
        {
            InitializeComponent();
            this.Text = Type == "1" ? "P01. PPIC Master List" : "P011. PPIC Master List (History)";
            this.DefaultFilter = Type == "1" ? string.Format("FtyGroup = '{0}' AND Finished = 0", Sci.Env.User.Factory) : string.Format("FtyGroup = '{0}' AND Finished = 1", Sci.Env.User.Factory);
            dataType = Type;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            displayBox6.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Order_BuyerDelivery' and ID = '{0}'", CurrentMaintain["KPIChangeReason"].ToString()));
            displayBox14.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", CurrentMaintain["SpecialMark"].ToString()));
            numericBox6.Value = MyUtility.Math.Round(Convert.ToDecimal(CurrentMaintain["CPU"]) * Convert.ToDecimal(CurrentMaintain["CPUFactor"]) * Convert.ToDecimal(CurrentMaintain["Qty"]), 3);
            displayBox17.Value = CurrentMaintain["MTLComplete"].ToString().ToUpper() == "TRUE" ? "Y" : "";
            displayBox22.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Delivery_OutStand' and ID = '{0}'", CurrentMaintain["OutstandingReason"].ToString()));
            displayBox23.Value = MyUtility.Check.Empty(CurrentMaintain["OutstandingDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["OutstandingDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            if (CurrentMaintain["LocalOrder"].ToString().ToUpper() == "TRUE")
            {
                numericBox7.Value = Convert.ToDecimal(CurrentMaintain["PoPrice"]);
                label44.Text = "/PCS";
                numericBox8.Value = MyUtility.Math.Round(Convert.ToDecimal(CurrentMaintain["PoPrice"]) * Convert.ToDecimal(CurrentMaintain["Qty"]), 3);
                tooltip.SetToolTip(numericBox8, Convert.ToString(CurrentMaintain["PoPrice"]) + '*' + Convert.ToString(CurrentMaintain["Qty"]));
            }
            else
            {
                numericBox7.Value = Convert.ToDecimal(CurrentMaintain["CMPPrice"]);
                label44.Text = "/" + CurrentMaintain["CMPUnit"].ToString();
                numericBox8.Value = numericBox6.Value;
                tooltip.SetToolTip(numericBox8, Convert.ToString(CurrentMaintain["CPU"]) + '*' + Convert.ToString(CurrentMaintain["CPUFactor"]) + '*' + Convert.ToString(CurrentMaintain["Qty"]));
            }
            #region 填Description, Exception Form, Fty Remark, Style Apv欄位值
            DataRow StyleData;
            string sqlCmd = string.Format("select Description,ExpectionForm,FTYRemark,ApvDate from Style where Ukey = {0}", CurrentMaintain["StyleUkey"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out StyleData))
            {
                displayBox5.Value = StyleData["Description"].ToString();
                checkBox1.Value = StyleData["ExpectionForm"].ToString();
                editBox3.Text = StyleData["FTYRemark"].ToString();
                if (MyUtility.Check.Empty(StyleData["ApvDate"]))
                {
                    dateBox26.Value = null;
                }
                else
                {
                    dateBox26.Value = Convert.ToDateTime(StyleData["ApvDate"]);
                }
            }
            else
            {
                displayBox5.Value = "";
                checkBox1.Value = "false";
                editBox3.Text = "";
                dateBox26.Value = null;
            }
            #endregion
            #region 填Buyer欄位值, 修改Special id1, Special id2, Special id3顯示值
            DataRow brandData;
            if (MyUtility.Check.Seek(string.Format("select ID,Customize1,Customize2,Customize3,BuyerID from Brand where ID = '{0}'", CurrentMaintain["BrandID"].ToString()), out brandData))
            {
                displayBox2.Value = brandData["BuyerID"].ToString();
                label32.Text = brandData["Customize1"].ToString();
                label33.Text = brandData["Customize2"].ToString();
                label34.Text = brandData["Customize3"].ToString();
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
            sqlCmd = string.Format("select POSMR,POHandle from PO where ID = '{0}'", CurrentMaintain["POID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out POData))
            {
                txttpeuser3.DisplayBox1Binding = POData["POSMR"].ToString();
                txttpeuser4.DisplayBox1Binding = POData["POHandle"].ToString();
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
isnull([dbo].getGarmentLT(o.StyleUkey,o.FactoryID),0) as GMTLT from Orders o where ID = '{0}'", CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null,sqlCmd, out OrdersData);
            if (result)
            {
                editBox2.Text = OrdersData.Rows[0]["PoList"].ToString();
                editBox4.Text = OrdersData.Rows[0]["CuttingList"].ToString();
                displayBox18.Value = OrdersData.Rows[0]["MTLExport"].ToString();
                displayBox20.Value = OrdersData.Rows[0]["PulloutComplete"].ToString();
                numericBox10.Value = Convert.ToDecimal(OrdersData.Rows[0]["GMTLT"]);
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

            //按鈕變色
            bool haveTmsCost = MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost where ID = '{0}'", CurrentMaintain["ID"].ToString()));
            button4.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;
            button5.ForeColor = haveTmsCost ? Color.Blue : Color.Black;
            button6.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Label"]) ? Color.Blue : Color.Black;
            button10.ForeColor = haveTmsCost ? Color.Blue : Color.Black;
            button11.ForeColor = MyUtility.Check.Seek(string.Format("select i.ID from Style s, IETMS i where s.Ukey = {0} and s.IETMSID = i.ID and s.IETMSVersion = i.Version", CurrentMaintain["StyleUkey"].ToString())) && MyUtility.Check.Seek(string.Format("select ID from Order_TmsCost where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button12.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["CMPQRemark"]) ? Color.Blue : Color.Black;
            button14.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Artwork where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button15.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button19.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Export_Detail where PoID = '{0}'", CurrentMaintain["POID"].ToString())) ? Color.Blue : Color.Black;
            button24.ForeColor = MyUtility.Check.Seek(string.Format("select ID from ArtworkPO_Detail where OrderID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button25.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits where StyleUkey = {0}", CurrentMaintain["StyleUKey"].ToString())) ? Color.Blue : Color.Black;
            button28.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail where OrderID = '{0}' and ClogReceiveID <> ''", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button29.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing"]) ? Color.Blue : Color.Black;
            button30.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_PFHis where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            button32.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing2"]) ? Color.Blue : Color.Black;
        }

        //Style
        private void textBox4_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            IList<DataRow> StyleData;
            string sqlCmd = "select ID,SeasonID,BrandID,Description,CdCodeID,CPU,StyleUnit,Ukey from Style where Junk = 0 and LocalStyle = 1";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,8,10,20,6,6,8,0", "", "Style,Season,Brand,Description,CdCode,CPU,Unit,");
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
                displayBox5.Value = StyleData[0]["Description"].ToString();
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
                    DataRow StyleData;
                    string sqlCmd = string.Format("select ID,SeasonID,BrandID,Description,CdCodeID,CPU,StyleUnit,Ukey from Style where Junk = 0 and LocalStyle = 1 and ID = '{0}'", textBox4.Text);
                    if (MyUtility.Check.Seek(sqlCmd, out StyleData))
                    {
                        CurrentMaintain["StyleID"] = StyleData["ID"];
                        CurrentMaintain["BrandID"] = StyleData["BrandID"];
                        CurrentMaintain["SeasonID"] = StyleData["SeasonID"];
                        CurrentMaintain["CdCodeID"] = StyleData["CdCodeID"];
                        CurrentMaintain["CPU"] = StyleData["CPU"];
                        CurrentMaintain["StyleUnit"] = StyleData["StyleUnit"];
                        CurrentMaintain["StyleUkey"] = StyleData["Ukey"];
                        displayBox5.Value = StyleData["Description"].ToString();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Style not found!!");
                        setStyleEmptyColumn();
                        e.Cancel = true;
                        return;
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
        private void txtfactory1_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtfactory1.OldValue != txtfactory1.Text)
            {
                if (MyUtility.Check.Empty(txtfactory1.Text))
                {
                    CurrentMaintain["FtyGroup"] = "";
                }
                else
                {
                    CurrentMaintain["FtyGroup"] = MyUtility.GetValue.Lookup(string.Format("select FTYGroup from Factory where ID = '{0}'",txtfactory1.Text));
                }
            }
        }

        //Cancelled Order
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (EditMode && checkBox4.Checked && !MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                if (MyUtility.Check.Seek(string.Format("select ID from SewingOutput_Detail where OrderId = '{0}' and QAQty > 0",CurrentMaintain["ID"].ToString())))
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

        //MC Handle CFM
        private void button1_Click(object sender, EventArgs e)
        {

        }

        //Local MR CFM
        private void button2_Click(object sender, EventArgs e)
        {

        }

        //Production output
        private void button3_Click(object sender, EventArgs e)
        {

        }

        //Order remark
        private void button4_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["OrderRemark"].ToString(), "Order Remark", false, null);
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
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["Label"].ToString(), "Label & Hangtag", false, null);
            callNextForm.ShowDialog(this);
        }

        //Q'ty b'down by shipmode
        private void button7_Click(object sender, EventArgs e)
        {

        }

        //Quantity breakdown
        private void button8_Click(object sender, EventArgs e)
        {

        }

        //Shipping mark
        private void button9_Click(object sender, EventArgs e)
        {

        }

        //TMS & Cost
        private void button10_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_TMSAndCost callNextForm = new Sci.Production.PPIC.P01_TMSAndCost(false, CurrentMaintain["ID"].ToString(), null, null);
            callNextForm.ShowDialog(this);
        }

        //Std.GSD List
        private void button11_Click(object sender, EventArgs e)
        {
            Sci.Production.PublicForm.StdGSDList callNextForm = new Sci.Production.PublicForm.StdGSDList(Convert.ToInt64(CurrentMaintain["StyleUKey"]));
            callNextForm.ShowDialog(this);
        }

        //CMPQ remark
        private void button12_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["CMPQRemark"].ToString(), "CMPQ Remark", false, null);
            callNextForm.ShowDialog(this);
        }
        
        //CMPQ Sheet
        private void button13_Click(object sender, EventArgs e)
        {

        }

        //Artwork
        private void button14_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_Artwork callNextForm = new Sci.Production.PPIC.P01_Artwork(false, CurrentMaintain["ID"].ToString(), null, null, CurrentMaintain["StyleID"].ToString(), CurrentMaintain["SeasonID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Garment export
        private void button15_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_GMTExport callNextForm = new Sci.Production.PPIC.P01_GMTExport(CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Sewing Inline History
        private void button16_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = new Win.UI.ShowHistory("Order_History", CurrentMaintain["ID"].ToString(), "Sewing", caption: "History",dataType:"D");
            callNextForm.ShowDialog(this);
        }

        //Cutting Combo
        private void button17_Click(object sender, EventArgs e)
        {

        }

        //b'down
        private void button18_Click(object sender, EventArgs e)
        {

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

        }

        //Accessory inspection list
        private void button21_Click(object sender, EventArgs e)
        {

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
            Sci.Production.PPIC.P01_ArtworkTrans callNextForm = new Sci.Production.PPIC.P01_ArtworkTrans(CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Production Kits
        private void button25_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionKit callNextForm = new Sci.Production.PPIC.P01_ProductionKit(dataType == "1" ? true : false, CurrentMaintain["StyleUKey"].ToString(), null, null, CurrentMaintain["StyleID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //M/Notice Sheet
        private void button26_Click(object sender, EventArgs e)
        {

        }
        
        //Q'ty b'down by schedule
        private void button27_Click(object sender, EventArgs e)
        {

        }

        //Carton Status
        private void button28_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CTNStatus callNextForm = new Sci.Production.PPIC.P01_CTNStatus(CurrentMaintain["ID"].ToString(), false);
            callNextForm.ShowDialog(this);
        }

        //Packing Method
        private void button29_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["Packing"].ToString(), "Packing Method", false, null);
            callNextForm.ShowDialog(this);
        }

        //Pull forward remark
        private void button30_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Remark from Order_PFHis where Id = '{0}' order by AddDate desc", CurrentMaintain["ID"].ToString())),"Pull Forward Remark");

        }

        //Shipment Finished
        private void button31_Click(object sender, EventArgs e)
        {

        }

        //VAS/SHAS Instruction
        private void button32_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["Packing2"].ToString(), "VAS/SHAS Instruction", false, null);
            callNextForm.ShowDialog(this);
        }

    }
}

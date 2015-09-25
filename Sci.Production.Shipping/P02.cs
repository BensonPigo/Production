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

namespace Sci.Production.Shipping
{
    public partial class P02 : Sci.Win.Tems.Input2
    {
        //宣告Context Menu Item
        ToolStripMenuItem focpl, purchase, poitem, newitem, edit, delete, print, batchprint;

        protected string carrierID = "";
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "1,Factory,2,Brand");
            MyUtility.Tool.SetupCombox(comboBox2, 2, 1, "1,SCI,2,Factory,3,Sullpier,4,Brand");
            detailgrid.CellToolTipTextNeeded += (s, e) =>
                {
                    e.ToolTipText = "You can show the function form to press the right key under inquiring the state.";
                };

            detailgridmenus.Items.Clear();//清空原有的Menu Item
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Import from FOC PL# (Garment FOC)", onclick: (s, e) => ImportFromFOCPL()).Get(out focpl);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Import from purchase (Material)", onclick: (s, e) => ImportFromPurchase()).Get(out purchase);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Add by PO# item (Garment Chargeable)", onclick: (s, e) => AddByPOItem()).Get(out poitem);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Add new Item", onclick: (s, e) => AddNewItem()).Get(out newitem);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Separator();
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Delete this Record - ", onclick: (s, e) => MenuDelete()).Get(out delete);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Edit this Record's detail - ", onclick: (s, e) => MenuEdit()).Get(out edit);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Separator();
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Print", onclick: (s, e) => Print()).Get(out print);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Batch Print", onclick: (s, e) => BatchPrint()).Get(out batchprint);
            SetContextMenuStatus(false); //預設先將Context ment設定為disable
        }

        //設定Context Menu的Enable/Disable
        private void SetContextMenuStatus(bool status)
        {
            focpl.Enabled = status;
            purchase.Enabled = status;
            poitem.Enabled = status;
            newitem.Enabled = status;
            delete.Enabled = status;
            edit.Enabled = status;
            print.Enabled = status;
            batchprint.Enabled = status;
        }

        //Context Menu選擇Import from FOC PL# (Garment FOC)
        private void ImportFromFOCPL()
        {
            Sci.Production.Shipping.P02_ImportFromFOCPackingList callFOCPLForm = new Sci.Production.Shipping.P02_ImportFromFOCPackingList(CurrentMaintain);
            callFOCPLForm.ShowDialog(this);
            RenewData();
        }

        //Context Menu選擇Import from purchase (Material)
        private void ImportFromPurchase()
        {
            Sci.Production.Shipping.P02_ImportFromPO callPurchaseForm = new Sci.Production.Shipping.P02_ImportFromPO(CurrentMaintain);
            callPurchaseForm.ShowDialog(this);
            RenewData();
        }

        //Context Menu選擇Add by PO# item (Garment Chargeable)
        private void AddByPOItem()
        {
            Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
            DataRow dr = ((DataTable)detailgridbs.DataSource).NewRow();
            dr["ID"] = CurrentMaintain["ID"].ToString();
            callPOItemForm.SetInsert(dr);
            callPOItemForm.ShowDialog(this);
            RenewData();
        }

        //Context Menu選擇Add new Item
        private void AddNewItem()
        {
            Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
            DataRow dr = ((DataTable)detailgridbs.DataSource).NewRow();
            dr["ID"] = CurrentMaintain["ID"].ToString();
            callNewItemForm.SetInsert(dr);
            callNewItemForm.ShowDialog(this);
            RenewData();
        }

        //Context Menu選擇Edit this Record's detail
        private void MenuEdit()
        {
            if (CurrentDetailData["Category"].ToString() == "1" || CurrentDetailData["Category"].ToString() == "2" || CurrentDetailData["Category"].ToString() == "3")
            {
                Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
                if (CurrentDetailData["Category"].ToString() == "1")
                {
                    callPOItemForm.Text = "International Air/Express - Import From FOC PL#";
                }
                callPOItemForm.SetUpdate(CurrentDetailData);
                callPOItemForm.ShowDialog(this);
            }

            if (CurrentDetailData["Category"].ToString() == "4")
            {
                Sci.Production.Shipping.P02_EditFromPO callEditPOForm = new Sci.Production.Shipping.P02_EditFromPO();
                callEditPOForm.SetUpdate(CurrentDetailData);
                callEditPOForm.ShowDialog(this);
            }

            if (CurrentDetailData["Category"].ToString() == "5" || CurrentDetailData["Category"].ToString() == "6" || CurrentDetailData["Category"].ToString() == "7" || CurrentDetailData["Category"].ToString() == "8" || CurrentDetailData["Category"].ToString() == "9")
            {
                Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
                callNewItemForm.SetUpdate(CurrentDetailData);
                callNewItemForm.ShowDialog(this);
            }
            RenewData();
        }

        //Context Menu選擇Delete this Record
        private void MenuDelete()
        {
            if (CurrentDetailData["Category"].ToString() == "1" || CurrentDetailData["Category"].ToString() == "2" || CurrentDetailData["Category"].ToString() == "3")
            {
                Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
                if (CurrentDetailData["Category"].ToString() == "1")
                {
                    callPOItemForm.Text = "International Air/Express - Import From FOC PL#";
                }
                callPOItemForm.SetDelete(CurrentDetailData);
                callPOItemForm.ShowDialog(this);
            }

            if (CurrentDetailData["Category"].ToString() == "4")
            {
                Sci.Production.Shipping.P02_EditFromPO callEditPOForm = new Sci.Production.Shipping.P02_EditFromPO();
                callEditPOForm.SetDelete(CurrentDetailData);
                callEditPOForm.ShowDialog(this);
            }

            if (CurrentDetailData["Category"].ToString() == "5" || CurrentDetailData["Category"].ToString() == "6" || CurrentDetailData["Category"].ToString() == "7" || CurrentDetailData["Category"].ToString() == "8" || CurrentDetailData["Category"].ToString() == "9")
            {
                Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
                callNewItemForm.SetDelete(CurrentDetailData);
                callNewItemForm.ShowDialog(this);
            }
            RenewData();
        }

        //Context Menu選擇Print
        private void Print()
        {
        }

        //Context Menu選擇Batch Print
        private void BatchPrint()
        {
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            displayBox2.Value = MyUtility.GetValue.Lookup("NameEN", CurrentMaintain["FromSite"].ToString(), "Brand", "ID");
            displayBox3.Value = "";
            if (CurrentMaintain["ToTag"].ToString() == "3")
            {
                displayBox3.Value = MyUtility.GetValue.Lookup("AbbEN", CurrentMaintain["FromSite"].ToString(), "Supp", "ID");
            }
            else
            {
                if (CurrentMaintain["ToTag"].ToString() == "4")
                {
                    displayBox3.Value = MyUtility.GetValue.Lookup("NameEN", CurrentMaintain["FromSite"].ToString(), "Brand", "ID");
                }
            }
            numericBox4.Value = CurrentMaintain["NW"].ToString() == "" ? 0 : Convert.ToDecimal(CurrentMaintain["NW"].ToString()) + Convert.ToDecimal(CurrentMaintain["CTNNW"].ToString());
            displayBox4.Value = MyUtility.GetValue.Lookup(string.Format("select c.SuppID + '-' + s.AbbEN from Carrier c left join Supp s on c.SuppID = s.ID where c.ID = '{0}'", CurrentMaintain["CarrierID"].ToString()));
            if (MyUtility.Check.Empty(CurrentMaintain["StatusUpdateDate"]))
            {
                displayBox6.Value = null;
            }
            else
            {
                displayBox6.Value = Convert.ToDateTime(CurrentMaintain["StatusUpdateDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SendDate"]))
            {
                displayBox7.Value = null;
            }
            else
            {
                displayBox7.Value = Convert.ToDateTime(CurrentMaintain["SendDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            }

            button1.Enabled = !EditMode && (CurrentMaintain["Status"].ToString() == "Send" || CurrentMaintain["Status"].ToString() == "Approve") && (PublicPrg.Prgs.GetAuthority(CurrentMaintain["Handle"].ToString()) || PublicPrg.Prgs.GetAuthority(CurrentMaintain["Manager"].ToString()));

            if (CurrentDetailData == null)
            {
                SetContextMenuStatus(false);
            }
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select ed.*,p.Refno,ed.SuppID+'-'+isnull(s.AbbEN,'') as Supplier,ec.CTNNW,dbo.getMtlDesc(ed.OrderID,ed.Seq1,ed.Seq2,1,0) as MtlDesc,
isnull(cast(ec.CtnLength as varchar),'')+'*'+isnull(cast(ec.CtnWidth as varchar),'')+'*'+isnull(cast(ec.CtnHeight as varchar),'') as Dimension,
isnull((ed.InCharge+' '+(select Name+' #'+ExtNo from Pass1 where ID = ed.InCharge)),ed.InCharge) as InChargeName,
isnull((ed.Receiver+' '+(select Name+' #'+ExtNo from TPEPass1 where ID = ed.Receiver)),ed.Receiver) as ReceiverName,
isnull((ed.Leader+' '+(select Name+' #'+ExtNo from TPEPass1 where ID = ed.Leader)),ed.Leader) as LeaderName,
CASE ed.Category
WHEN '1' THEN N'Sample'
WHEN '2' THEN N'SMS'
WHEN '3' THEN N'Bulk'
WHEN '4' THEN N'Material'
WHEN '5' THEN N'Dox'
WHEN '6' THEN N'Machine/Parts'
WHEN '7' THEN N'Mock Up'
WHEN '8' THEN N'Other Sample'
WHEN '9' THEN N'Other Material'
ELSE N''
END as CategoryName
from Express_Detail ed
left join PO_Supp_Detail p on ed.OrderID = p.ID and ed.Seq1 = p.SEQ1 and ed.Seq2 = p.SEQ2
left join Supp s on ed.SuppID = s.ID
left join Express_CTNData ec on ed.ID = ec.ID and ed.CTNNo = ec.CTNNo
where ed.ID = '{0}'
Order by ed.CTNNo,ed.Seq1", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Ict.Win.DataGridViewGeneratorTextColumnSettings orderid = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            orderid.CellMouseDoubleClick += (s, e) =>
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = detailgrid.GetDataRow<DataRow>(e.RowIndex);

                            if (dr["Category"].ToString() == "1" || dr["Category"].ToString() == "2" || dr["Category"].ToString() == "3")
                            {
                                Sci.Production.Shipping.P02_AddByPOItem callPOItemForm = new Sci.Production.Shipping.P02_AddByPOItem();
                                callPOItemForm.SetView(dr);
                                callPOItemForm.ShowDialog(this);
                            }

                            if (dr["Category"].ToString() == "4")
                            {
                                Sci.Production.Shipping.P02_EditFromPO callEditPOForm = new Sci.Production.Shipping.P02_EditFromPO();
                                callEditPOForm.SetView(dr);
                                callEditPOForm.ShowDialog(this);
                            }

                            if (dr["Category"].ToString() == "5" || dr["Category"].ToString() == "6" || dr["Category"].ToString() == "7" || dr["Category"].ToString() == "8" || dr["Category"].ToString() == "9")
                            {
                                Sci.Production.Shipping.P02_AddNewItem callNewItemForm = new Sci.Production.Shipping.P02_AddNewItem();
                                callNewItemForm.SetView(dr);
                                callNewItemForm.ShowDialog(this);
                            }
                        }
                    }
                };

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), settings: orderid)
                .Text("Seq1", header: "Seq1#", width: Widths.AnsiChars(3))
                .Text("Seq2", header: "Seq2#", width: Widths.AnsiChars(2))
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(10))
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15))
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(15))
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(15))
                .Text("CTNNo", header: "C/No.", width: Widths.AnsiChars(5))
                .Numeric("NW", header: "N.W.", width: Widths.AnsiChars(10), decimal_places: 2)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(10), decimal_places: 4)
                .Numeric("Qty", header: "Q'ty", width: Widths.AnsiChars(10), decimal_places: 2)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(5))
                .Text("CategoryName", header: "Category", width: Widths.AnsiChars(14))
                .Text("DutyNo", header: "AirPP#/FOC PL#", width: Widths.AnsiChars(13))
                .Text("InChargeName", header: "In Charge", width: Widths.AnsiChars(13))
                .Text("ReceiverName", header: "Receiver", width: Widths.AnsiChars(13))
                .Text("LeaderName", header: "Team Leader", width: Widths.AnsiChars(13))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(20))
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20))
                .Text("Dimension", header: "Dimension (cm)", width: Widths.AnsiChars(13))
                .Numeric("CTNNW", header: "Carton Weight", width: Widths.AnsiChars(10), decimal_places: 2);
        }

        protected override void OnDetailGridRowChanged()
        {
            base.OnDetailGridRowChanged();
            editBox2.Text = "";
            if (CurrentDetailData != null)
            {
                if (MyUtility.Check.Empty(CurrentDetailData["OrderID"]) || MyUtility.Check.Empty(CurrentDetailData["Seq2"]))
                {
                    editBox2.Text = CurrentDetailData["Description"].ToString();
                }
                else
                {
                    editBox2.Text = CurrentDetailData["MtlDesc"].ToString();
                }

                //先將Menu狀態全打開
                SetContextMenuStatus(true);

                //只有HC的Handle或此項次的申請人才可以刪除及修改
                bool authority = PublicPrg.Prgs.GetAuthority(CurrentMaintain["Handle"].ToString()) || PublicPrg.Prgs.GetAuthority(CurrentDetailData["InCharge"].ToString());
                delete.Enabled = authority;
                edit.Enabled = authority;
                //Approve後就不可以再做任何動作(更改Context Menu Item的Enable/Disable)
                if (CurrentMaintain["Status"].ToString() == "Approved")
                {
                    focpl.Enabled = false;
                    purchase.Enabled = false;
                    poitem.Enabled = false;
                    newitem.Enabled = false;
                    delete.Enabled = false;
                    edit.Enabled = false;
                }
                //修改顯示內容
                delete.Text = string.Format("Delete this Record - {0} {1}-{2}", CurrentDetailData["OrderID"].ToString(), CurrentDetailData["Seq1"].ToString(), CurrentDetailData["Seq2"].ToString());
                edit.Text = string.Format("Edit this Record's detail - {0} {1}-{2}", CurrentDetailData["OrderID"].ToString(), CurrentDetailData["Seq1"].ToString(), CurrentDetailData["Seq2"].ToString());

                //如果狀態是Junk的話，Context Menu要Disable
                if (CurrentMaintain["Status"].ToString() == "Junked")
                {
                    SetContextMenuStatus(false);
                }
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Handle"] = Sci.Env.User.UserID;
            CurrentMaintain["Manager"] = MyUtility.GetValue.Lookup("Supervisor", Sci.Env.User.UserID, "Pass1", "ID");
            CurrentMaintain["NW"] = 0;
            CurrentMaintain["CTNNW"] = 0;
            textBox5.ReadOnly = true; //因為Key Down事件，如果按Delete or Backspace按鍵會真的將字元給移除，如果跳出視窗後按Cancel的話，資料會不正確，所以就把此欄位設定為ReadOnly
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Status is not 'New', can't modify!");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            textBox2.ReadOnly = CurrentMaintain["ToTag"].ToString() == "1" ? true : false;
            textBox5.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["FromSite"]))
            {
                MyUtility.Msg.WarningBox("From (Site) can't empty");
                textBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ToSite"]))
            {
                MyUtility.Msg.WarningBox("To (Site) can't empty");
                textBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Dest"]))
            {
                MyUtility.Msg.WarningBox("Destination can't empty");
                txtcountry1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["PortAir"]))
            {
                MyUtility.Msg.WarningBox("Port can't empty");
                textBox4.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ShipDate"]))
            {
                MyUtility.Msg.WarningBox("Ship. Date can't empty");
                dateBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ETD"]))
            {
                MyUtility.Msg.WarningBox("ETD can't empty");
                dateBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ETA"]))
            {
                MyUtility.Msg.WarningBox("ETA can't empty");
                dateBox3.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["CarrierID"]))
            {
                MyUtility.Msg.WarningBox("Carrier can't empty");
                textBox5.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                MyUtility.Msg.WarningBox("B/L No. can't empty");
                textBox6.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("Handle can't empty");
                txtuser1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Manager"]))
            {
                MyUtility.Msg.WarningBox("Manager can't empty");
                txtuser2.TextBox1.Focus();
                return false;
            }
            #endregion

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "HC", "Express", Convert.ToDateTime(CurrentMaintain["ShipDate"]), 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        //From
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CurrentMaintain["FromTag"] = comboBox1.SelectedValue;
            CurrentMaintain["FromSite"] = "";
            displayBox2.Value = "";
            CurrentMaintain["CarrierID"] = "";
            displayBox4.Value = "";
            CurrentMaintain["ExpressACNo"] = "";
        }

        //From Site
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["FromTag"]))
            {
                return;
            }

            string sqlCmd = "";
            Sci.Win.Tools.SelectItem item;
            if (CurrentMaintain["FromTag"].ToString() == "1")
            {
                sqlCmd = "select ID from Factory where Junk = 0 and ExpressGroup <> ''";
                item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", textBox1.Text);
            }
            else
            {
                sqlCmd = "select ID,NameEN from Brand where Junk = 0";
                item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,50", textBox1.Text);
            }

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            CurrentMaintain["FromSite"] = item.GetSelectedString();
            if (CurrentMaintain["FromTag"].ToString() == "2")
            {
                IList<DataRow> brand = item.GetSelecteds();
                displayBox2.Value = brand[0]["NameEN"].ToString();
            }
        }

        //From Site
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.OldValue != textBox1.Text)
            {
                if (MyUtility.Check.Empty(textBox1.Text))
                {
                    CurrentMaintain["FromSite"] = "";
                    displayBox2.Value = "";
                }
                else
                {
                    if (CurrentMaintain["FromTag"].ToString() == "1")
                    {
                        if (!MyUtility.Check.Seek(string.Format("select ID from Factory where Junk = 0 and ExpressGroup <> '' and ID = '{0}'", textBox1.Text)))
                        {
                            MyUtility.Msg.WarningBox(string.Format("Factory: {0} does not exist.", textBox1.Text));
                            CurrentMaintain["FromSite"] = "";
                            displayBox2.Value = "";
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            CurrentMaintain["FromSite"] = textBox1.Text;
                            CurrentMaintain["ShipMark"] = textBox1.Text;
                            displayBox2.Value = "";
                        }
                    }
                    else
                    {
                        if (CurrentMaintain["FromTag"].ToString() == "2")
                        {
                            DataRow brandData;
                            if (!MyUtility.Check.Seek(string.Format("select NameEN from Brand where Junk = 0 and ID = '{0}'", textBox1.Text), out brandData))
                            {
                                MyUtility.Msg.WarningBox(string.Format("Brand: {0} does not exist.", textBox1.Text));
                                CurrentMaintain["FromSite"] = "";
                                displayBox2.Value = "";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                CurrentMaintain["FromSite"] = textBox1.Text;
                                displayBox2.Value = brandData["NameEN"].ToString();
                            }
                        }
                    }
                }
                GetCarrier();
            }
        }

        //To
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CurrentMaintain["ToTag"] = comboBox2.SelectedValue;
            CurrentMaintain["ToSite"] = comboBox2.SelectedValue.ToString() == "1" ? "SCI" : "";
            displayBox3.Value = "";
            CurrentMaintain["CarrierID"] = "";
            displayBox4.Value = "";
            CurrentMaintain["ExpressACNo"] = "";
            textBox2.ReadOnly = comboBox2.SelectedValue.ToString() == "1" ? true : false;
            if (comboBox2.SelectedValue.ToString() == "1")
            {
                GetCarrier();
            }
        }

        //To Site
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ToTag"]))
            {
                return;
            }

            string sqlCmd = "";
            Sci.Win.Tools.SelectItem item;
            if (CurrentMaintain["ToTag"].ToString() == "2")
            {
                sqlCmd = "select ID from SCIFty where Junk = 0 AND ExpressGroup <> ''";
                item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", textBox2.Text);
            }
            else
            {
                if (CurrentMaintain["ToTag"].ToString() == "3")
                {
                    sqlCmd = "select ID,AbbCH,AbbEN from Supp where Junk = 0";
                    item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,20,20", textBox2.Text);
                }
                else
                {
                    sqlCmd = "select ID,NameEN,CountryID from Brand where Junk = 0";
                    item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,20,0", textBox2.Text);
                }
            }

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            CurrentMaintain["ToSite"] = item.GetSelectedString();
            IList<DataRow> brand = item.GetSelecteds();
            if (CurrentMaintain["ToTag"].ToString() == "3")
            {
                displayBox3.Value = brand[0]["AbbEN"].ToString();
            }
            if (CurrentMaintain["ToTag"].ToString() == "4")
            {
                displayBox3.Value = brand[0]["NameEN"].ToString();
                CurrentMaintain["Dest"] = brand[0]["CountryID"].ToString();
            }
        }

        //To Site
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (textBox2.OldValue != textBox2.Text)
            {
                if (MyUtility.Check.Empty(textBox2.Text))
                {
                    CurrentMaintain["ToSite"] = "";
                    CurrentMaintain["PortAir"] = "";
                    displayBox3.Value = "";
                }
                else
                {
                    if (CurrentMaintain["ToTag"].ToString() == "2")
                    {
                        DataRow sciftyData;
                        if (!MyUtility.Check.Seek(string.Format("select ID,ExpressGroup,CountryID,PortAir from SCIFty where Junk = 0 AND ExpressGroup <> '' and ID = '{0}'", textBox2.Text), out sciftyData))
                        {
                            MyUtility.Msg.WarningBox(string.Format("Factory: {0} does not exist.", textBox2.Text));
                            CurrentMaintain["ToSite"] = "";
                            CurrentMaintain["PortAir"] = "";
                            displayBox3.Value = "";
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            CurrentMaintain["ToSite"] = textBox2.Text;
                            CurrentMaintain["PortAir"] = sciftyData["PortAir"].ToString();
                            displayBox3.Value = "";
                            if (MyUtility.Check.Empty(CurrentMaintain["ShipMark"]))
                            {
                                CurrentMaintain["ShipMark"] = textBox2.Text;
                                CurrentMaintain["Dest"] = sciftyData["CountryID"].ToString();
                            }
                        }
                    }
                    else
                    {
                        if (CurrentMaintain["ToTag"].ToString() == "3")
                        {
                            DataRow suppData;
                            if (!MyUtility.Check.Seek(string.Format("select AbbEN from Supp where Junk = 0 and ID = '{0}'", textBox2.Text), out suppData))
                            {
                                MyUtility.Msg.WarningBox(string.Format("Supplier: {0} does not exist.", textBox2.Text));
                                CurrentMaintain["ToSite"] = "";
                                CurrentMaintain["PortAir"] = "";
                                displayBox3.Value = "";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                CurrentMaintain["ToSite"] = textBox2.Text;
                                displayBox3.Value = suppData["AbbEN"].ToString();
                                CurrentMaintain["PortAir"] = "";
                            }
                        }
                        else
                        {
                            if (CurrentMaintain["ToTag"].ToString() == "4")
                            {
                                DataRow brandData;
                                if (!MyUtility.Check.Seek(string.Format("select NameEN,CountryID from Brand where Junk = 0 and ID = '{0}'", textBox2.Text), out brandData))
                                {
                                    MyUtility.Msg.WarningBox(string.Format("Brand: {0} does not exist.", textBox2.Text));
                                    CurrentMaintain["ToSite"] = "";
                                    CurrentMaintain["PortAir"] = "";
                                    displayBox3.Value = "";
                                    e.Cancel = true;
                                    return;
                                }
                                else
                                {
                                    CurrentMaintain["ToSite"] = textBox2.Text;
                                    displayBox3.Value = brandData["NameEN"].ToString();
                                    CurrentMaintain["Dest"] = brandData["CountryID"].ToString();
                                    CurrentMaintain["PortAir"] = "";
                                }
                            }
                        }
                    }
                }
                GetCarrier();
            }
        }

        private void GetCarrier()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["FromTag"]) && CurrentMaintain["FromTag"].ToString() == "1" && !MyUtility.Check.Empty(CurrentMaintain["FromSite"]) && !MyUtility.Check.Empty(CurrentMaintain["ToSite"]))
            {
                string fromSite = "", toSite = "", fromCountry = "", toCountry = "";
                fromSite = CurrentMaintain["FromSite"].ToString();
                fromCountry = MyUtility.GetValue.Lookup(string.Format("select CountryID from Factory where ID = '{0}'", fromSite));
                toSite = CurrentMaintain["ToSite"].ToString();
                switch (CurrentMaintain["ToTag"].ToString())
                {
                    case "1":
                        toCountry = "TW";
                        break;
                    case "2":
                        toCountry = MyUtility.GetValue.Lookup(string.Format("select CountryID from SCIFty where ID = '{0}'", toSite));
                        break;
                    case "3":
                        toCountry = MyUtility.GetValue.Lookup(string.Format("select CountryID from Supp where ID = '{0}'", toSite));
                        break;
                    case "4":
                        toCountry = MyUtility.GetValue.Lookup(string.Format("select CountryID from Brand where ID = '{0}'", toSite));
                        break;
                }


                string sqlCmd = string.Format(@"declare @1st varchar(4),@2nd varchar(4),@3rd varchar(4),@4th varchar(4),@5th varchar(4),@6th varchar(4) ;
select @1st=ID from Carrier_FtyRule where FromSite = '{0}' and ToSite = '{2}';
select @2nd=ID from Carrier_FtyRule where FromSite = '{0}' and ToCountry = '{3}';
select @3rd=ID from Carrier_FtyRule where FromCountry = '{1}' and ToSite = '{2}';
select @4th=ID from Carrier_FtyRule where FromCountry = '{1}' and ToCountry = '{3}';
select @5th=ID from Carrier_FtyRule where FromSite = '{0}';
select @6th=ID from Carrier_FtyRule where FromCountry = '{1}';

select c.ID,c.Account,c.SuppID,isnull(s.AbbEN,'') as Abb from Carrier c
left join Supp s on c.SuppID = s.ID
where c.ID = (select iif(@1st is null,(iif(@2nd is null,iif(@3rd is null,iif(@4th is null,iif(@5th is null,@6th,@5th),@4th),@3rd),@2nd)),@1st));",
                                                                                                                          fromSite, fromCountry, toSite, toCountry);
                DataTable CarrierData;
                DualResult result = DBProxy.Current.Select(null,sqlCmd,out CarrierData);
                if (result && CarrierData.Rows.Count > 0)
                {
                    CurrentMaintain["CarrierID"] = CarrierData.Rows[0]["ID"].ToString();
                    CurrentMaintain["ExpressACNo"] = CarrierData.Rows[0]["Account"].ToString();
                    displayBox4.Value = CarrierData.Rows[0]["SuppID"].ToString() + " " + CarrierData.Rows[0]["Abb"].ToString();
                }
                else
                {
                    CurrentMaintain["CarrierID"] = "";
                    CurrentMaintain["ExpressACNo"] = "";
                    displayBox4.Value = "";
                }
            }
        }

        //ETD
        private void dateBox2_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(dateBox2.Value) && dateBox2.OldValue != dateBox2.Value)
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["ETA"]) && dateBox2.Value > Convert.ToDateTime(CurrentMaintain["ETA"]))
                {
                    MyUtility.Msg.WarningBox("ETD can't later than ETA!");
                    CurrentMaintain["ETD"] = DBNull.Value;
                    e.Cancel = true;
                    return;
                }
            }
        }

        //ETA
        private void dateBox3_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(dateBox3.Value) && dateBox3.OldValue != dateBox3.Value)
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["ETD"]))
                {
                    if (dateBox3.Value < Convert.ToDateTime(CurrentMaintain["ETD"]))
                    {
                        MyUtility.Msg.WarningBox("ETA can't early than ETD!");
                        CurrentMaintain["ETA"] = DBNull.Value;
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        if (dateBox3.Value > Convert.ToDateTime(CurrentMaintain["ETD"]).AddDays(90))
                        {
                            MyUtility.Msg.WarningBox("ETA can't later than ETD more than 90 days!");
                            CurrentMaintain["ETA"] = DBNull.Value;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        //Carrier
        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (EditMode)
            {
                CarrierPopup();
            }
        }

        //Carrier
        private void textBox5_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            CarrierPopup();
        }

        //Carrier按右鍵帶出的資料
        private void CarrierPopup()
        {
            string sqlCmd = @"select c.ID,c.SuppID,isnull(s.AbbEN,'') as Abb,c.Account
from Carrier c
left join Supp s on c.SuppID = s.ID";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "5,8,20,20", textBox5.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            IList<DataRow> carrier = item.GetSelecteds();
            CurrentMaintain["CarrierID"] = item.GetSelectedString();
            displayBox4.Value = carrier[0]["SuppID"].ToString() + " " + carrier[0]["Abb"].ToString();
            CurrentMaintain["ExpressACNo"] = carrier[0]["Account"].ToString();
        }

        //B/L No.
        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(textBox6) && textBox6.OldValue != textBox6.Text)
            {
                for (int i = 0; i < textBox6.Text.Trim().Length; i++)
                {
                    var asc = ASCIIEncoding.ASCII.GetBytes(textBox6.Text.Substring(i, 1));
                    if (!((asc[0] >= 48 && asc[0] <= 57) || (asc[0] >= 65 && asc[0] <= 90)))
                    {
                        MyUtility.Msg.WarningBox("B/L No. format error, only keyin (0-9,A-Z)!");
                        CurrentMaintain["BLNo"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        //Carton Dimension && Weight
        private void button2_Click(object sender, EventArgs e)
        {
            if (null == CurrentMaintain) return;

            string sqlCmd = string.Format(@"with NewCtn
as
(
select distinct ed.ID,ed.CTNNo,'N' as Status 
from Express_Detail ed
where ed.ID = '{0}'
and not exists (select 1 from Express_CTNData ec where ec.ID = ed.ID and ec.CTNNo = ed.CTNNo)
),
DeleteCtn
as
(
select distinct ec.ID,ec.CTNNo,'D' as Status 
from Express_CTNData ec
where ec.ID = '{0}'
and not exists (select 1 from Express_Detail ed where ec.ID = ed.ID and ec.CTNNo = ed.CTNNo)
)
select * from NewCtn
union
select * from DeleteCtn", CurrentMaintain["ID"].ToString());
            DataTable CTNData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out CTNData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query Ctn data fail, please try again.\r\n" + result.ToString());
                return;
            }

            if (CTNData.Rows.Count > 0)
            {
                IList<string> updateCmds = new List<string>();
                foreach (DataRow dr in CTNData.Rows)
                {
                    if (dr["Status"].ToString() == "N")
                    {
                        updateCmds.Add(string.Format("insert into Express_CTNData(ID,CTNNo,AddName, AddDate) values ('{0}','{1}','{2}',GETDATE());", dr["ID"].ToString(), dr["CTNNo"].ToString(), Sci.Env.User.UserID));
                    }
                    else
                    {
                        updateCmds.Add(string.Format("update Express_CTNData set EditName = '{2}', EditDate = GETDATE() where ID = '{0}' and CTNNo = '{1}';", dr["ID"].ToString(), dr["CTNNo"].ToString(), Sci.Env.User.UserID));
                        updateCmds.Add(string.Format("delete from Express_CTNData where ID = '{0}' and CTNNo = '{1}';", dr["ID"].ToString(), dr["CTNNo"].ToString()));
                    }
                }
                result = DBProxy.Current.Executes(null, updateCmds);

                if (!result)
                {
                    MyUtility.Msg.WarningBox("Update Ctn data fail, please try again.\r\n" + result.ToString());
                    return;
                }
            }

            Sci.Production.Shipping.P02_CTNDimensionAndWeight callNextForm = new Sci.Production.Shipping.P02_CTNDimensionAndWeight((CurrentMaintain["Status"].ToString() == "New" || CurrentMaintain["Status"].ToString() == "Send") && (PublicPrg.Prgs.GetAuthority(CurrentMaintain["Handle"].ToString()) || PublicPrg.Prgs.GetAuthority(CurrentMaintain["Manager"].ToString())), CurrentMaintain["ID"].ToString(), null, null);
            callNextForm.ShowDialog(this);
            this.RenewData();

        }

        //Send
        protected override void ClickSend()
        {
            base.ClickSend();
            if (!(PublicPrg.Prgs.GetAuthority(CurrentMaintain["Handle"].ToString()) || PublicPrg.Prgs.GetAuthority(CurrentMaintain["Manager"].ToString())))
            {
                MyUtility.Msg.WarningBox("You don't have permission to send.");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                MyUtility.Msg.WarningBox("B/L No. can't empty!!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Send > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            SendMail();
            string updateCmd = string.Format("update Express set Status = 'Send', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Send data faile.\r\n" + result.ToString());
                return;
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Recall
        protected override void ClickRecall()
        {
            base.ClickRecall();
            string updateCmd = string.Format("update Express set Status = 'New', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Recall data faile.\r\n" + result.ToString());
                return;
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Junk
        protected override void ClickJunk()
        {
            base.ClickJunk();
            if (!(PublicPrg.Prgs.GetAuthority(CurrentMaintain["Handle"].ToString()) || PublicPrg.Prgs.GetAuthority(CurrentMaintain["Manager"].ToString())))
            {
                MyUtility.Msg.WarningBox("You don't have permission to send.");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Junk > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            IList<string> updateCmds = new List<string>();
            updateCmds.Add(string.Format("update Express set Status = 'Junk', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString()));
            updateCmds.Add(string.Format("update PackingList set ExpressID = '' where ExpressID = '{0}'", CurrentMaintain["ID"].ToString()));

            DualResult result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Junk data faile.\r\n" + result.ToString());
                return;
            }
            SendMail();
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Approve
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            RenewData();
            if (!(PublicPrg.Prgs.GetAuthority(CurrentMaintain["Handle"].ToString()) || PublicPrg.Prgs.GetAuthority(CurrentMaintain["Manager"].ToString())))
            {
                MyUtility.Msg.WarningBox("You don't have permission to approve.");
                return;
            }
            if (!MyUtility.Check.Seek(string.Format("select ID from Express_Detail where ID = '{0}'", CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("There's no detail data, don't need to approve.");
                return;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["NW"]))
            {
                MyUtility.Msg.WarningBox("< ttl N.W. > can't empty!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                MyUtility.Msg.WarningBox("< B/L No. > can't empty!");
                return;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["FtyInvNo"]))
            {
                MyUtility.Msg.WarningBox("< Invoice No. > can't empty!");
                return;
            }

            if (MyUtility.Check.Seek(string.Format("select ID from Express_Detail where ID = '{0}' and NW <= 0", CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("N.W. detail data can't be 0.");
                return;
            }

            if (MyUtility.Check.Seek(string.Format("select ID from Express_Detail where ID = '' and Qty <= 0", CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("Q'ty detail data can't be 0.");
                return;
            }

            if (MyUtility.Check.Seek(string.Format("select ID from Express_CTNData where ID = '{0}' and (CtnLength <= 0 or CtnWidth <= 0 or CtnHeight <= 0 or CTNNW <= 0)", CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("Carton Dimension & Weight data can't empty!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Approve > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string updateCmd = string.Format("update Express set Status = 'Approved', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Approve data faile.\r\n" + result.ToString());
                return;
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Unapprove
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!(PublicPrg.Prgs.GetAuthority(CurrentMaintain["Handle"].ToString()) || PublicPrg.Prgs.GetAuthority(CurrentMaintain["Manager"].ToString())))
            {
                MyUtility.Msg.WarningBox("You don't have permission to unapprove.");
                return;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["PayDate"]))
            {
                MyUtility.Msg.WarningBox("Already have payment, can't unapprove!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unapprove > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string updateCmd = string.Format("update Express set Status = 'Send', StatusUpdateDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unapprove data faile.\r\n" + result.ToString());
                return;
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        // Mail to
        private void button1_Click(object sender, EventArgs e)
        {
            SendMail();
        }

        // Mail to
        private void SendMail()
        {
            DataRow dr;
            if (MyUtility.Check.Seek("select * from MailTo where ID = '003'", out dr))
            {
                string mailto = dr["ToAddress"].ToString();
                string cc = dr["CcAddress"].ToString();
                string subject = string.Format("<{0}-{1}> TO <{2}{3}> HC#({4}) International Express ({5}){6}",
                    CurrentMaintain["FromSite"].ToString(), CurrentMaintain["FromTag"].ToString() == "1" ? CurrentMaintain["FromSite"].ToString() : displayBox2.Value.ToString(),
                    CurrentMaintain["ToSite"].ToString(), CurrentMaintain["FromTag"].ToString() == "3" ? "-" + displayBox3.Value.ToString() : "",
                    CurrentMaintain["ID"].ToString(),
                    MyUtility.Check.Empty(CurrentMaintain["ShipDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["ShipDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat)),
                    CurrentMaintain["Status"].ToString() == "Junk" ? " -Cancel" : "");
                StringBuilder content = new StringBuilder();
                #region 組Content
                content.Append(string.Format(@"< {0} > will have EXPRESS shipment from {1}-{2} TO {3}{4} ON {5} 

If anyone who wants to follow this shipment, must fill the detail & sp# in this HC#. And team leader need to approve. 
When first applicant's team leader approval, anyone can not do any modification. Please noted.


---Pls notice the Qty & G.W must coincide w/ shipping document. If found out a great inconsistency between system & document then application team have to be deputy of all express application for one month.
---If the goods belong to BULK ORDER, pls must fill in the system with ICR# or Debit note#.
---Applicant need to notify factory & supplier's attn. to arrange the shipping time by oneself before export the goods.
---Applicant also need to notify factory , supplier & customer's attn. to make sure that they can receive the goods by oneself after shipped out  the goods.
", CurrentMaintain["Manager"].ToString() + "-" + MyUtility.GetValue.Lookup(string.Format("select Name from Pass1 where ID = '{0}'", CurrentMaintain["Manager"].ToString())),
 CurrentMaintain["FromSite"].ToString(), CurrentMaintain["FromTag"].ToString() == "1" ? CurrentMaintain["FromSite"].ToString() : displayBox2.Value.ToString(),
 CurrentMaintain["ToSite"].ToString(), CurrentMaintain["FromTag"].ToString() == "3" ? "-" + displayBox3.Value.ToString() : "",
 MyUtility.Check.Empty(CurrentMaintain["ShipDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["ShipDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat))));
                #endregion

                var email = new MailTo(Sci.Env.User.MailAddress, mailto, cc, subject, "", content.ToString(), false, false);
                email.ShowDialog(this);
            }
        }
    }
}

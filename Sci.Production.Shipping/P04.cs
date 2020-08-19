using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P04
    /// </summary>
    public partial class P04 : Win.Tems.Input6
    {
        /// <summary>
        /// P04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
            this.txtSisFtyWK.ReadOnly = true;

            #region Shipping Mode
            string sqlcmd = @"select ID,UseFunction from ShipMode WITH (NOLOCK) where id !='AIR' order by ID";
            DualResult cbResult;
            DataTable shipModeTable = new DataTable();
            if (cbResult = DBProxy.Current.Select(null, sqlcmd, out shipModeTable))
            {
                this.comboShippMode.DataSource = shipModeTable;
                this.comboShippMode.DisplayMember = "ID";
                this.comboShippMode.ValueMember = "ID";
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
             string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
             this.DetailSelectCommand = string.Format(
                 @"select ed.*,isnull(o.FactoryID,'') as FactoryID,isnull(o.BrandID,'') as BrandID,o.BuyerDelivery,o.SciDelivery,
(left(ed.Seq1+' ',3)+'-'+ed.Seq2) as Seq,(ed.SuppID+'-'+iif(fe.Type = 4,(select Abb from LocalSupp WITH (NOLOCK) where ID = ed.SuppID),(select AbbEN from Supp WITH (NOLOCK) where ID = ed.SuppID))) as Supp,
ed.RefNo,isnull(iif(fe.Type = 4,(select Description from LocalItem WITH (NOLOCK) where RefNo = ed.RefNo),(select DescDetail from Fabric WITH (NOLOCK) where SCIRefno = ed.SCIRefNo)),'') as Description,
(case when ed.FabricType = 'F' then 'Fabric' when ed.FabricType = 'A' then 'Accessory' else '' end) as Type
from FtyExport_Detail ed WITH (NOLOCK) 
left join FtyExport fe WITH (NOLOCK) on fe.ID = ed.ID
left join Orders o WITH (NOLOCK) on o.ID = ed.PoID
where ed.ID = '{0}'", masterID);
             return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.radio3rdCountry.Checked || this.radioTransferIn.Checked || this.radioLocalPurchase.Checked)
            {
                this.lbDeclareation.Text = "Import Declaration ID";
                #region Declaration ID
                if (!MyUtility.Check.Empty(this.CurrentMaintain["Blno"]))
                {
                    string sqlcmd = $@"select ID from VNImportDeclaration where BLNo='{this.CurrentMaintain["Blno"]}' and IsFtyExport = 1";
                    this.displayDeclarationID.Text = MyUtility.GetValue.Lookup(sqlcmd);
                }
                else
                {
                    this.displayDeclarationID.Text = string.Empty;
                }

                if (MyUtility.Check.Empty(this.displayDeclarationID.Text))
                {
                    string sqlcmd = $@"select ID from VNImportDeclaration where WKNo='{this.CurrentMaintain["ID"]}' and IsFtyExport = 1";
                    this.displayDeclarationID.Text = MyUtility.GetValue.Lookup(sqlcmd);
                }
                #endregion
                #region CustomsDeclareNo
                if (!MyUtility.Check.Empty(this.CurrentMaintain["Blno"]))
                {
                    string sqlcmd = $@"select DeclareNo from VNImportDeclaration where BLNo='{this.CurrentMaintain["Blno"]}' and IsFtyExport = 1";
                    this.displayCustomsDeclareNo.Text = MyUtility.GetValue.Lookup(sqlcmd);
                }
                else
                {
                    this.displayCustomsDeclareNo.Text = string.Empty;
                }

                if (MyUtility.Check.Empty(this.displayDeclarationID.Text))
                {
                    string sqlcmd = $@"select DeclareNo from VNImportDeclaration where WKNo='{this.CurrentMaintain["ID"]}' and IsFtyExport = 1";
                    this.displayCustomsDeclareNo.Text = MyUtility.GetValue.Lookup(sqlcmd);
                }
                #endregion
            }
            else if (this.radioTransferOut.Checked)
            {
                this.lbDeclareation.Text = "Export Declaration ID";
                string sqlcmd = $@"select DeclareNo from VNContractQtyAdjust where WKNo='{this.CurrentMaintain["ID"]}'";
                this.displayDeclarationID.Text = string.Empty;
                this.displayCustomsDeclareNo.Text = MyUtility.GetValue.Lookup(sqlcmd);
            }

            this.ControlColor();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("FactoryID", header: "Prod. Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Del.", iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCI Del.", iseditingreadonly: true)
                .Text("Seq", header: "SEQ", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Import Q'ty", decimal_places: 2)
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2)
                .Numeric("WeightKg", header: "G.W.(kg)", decimal_places: 2);
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Type"] = 1;
            this.CurrentMaintain["Handle"] = Env.User.UserID;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            string sqlCmd = string.Empty;
            DataTable dataTable = new DataTable();
            #region 存檔不可為空判斷

            // Type = 3 (Transfer out) Arrive Port Date and Dox Rcv Date 可以為空
            if (this.CurrentMaintain["Type"].ToString() != "3")
            {
                if (MyUtility.Check.Empty(this.dateArrivePortDate.Value))
                {
                    this.dateArrivePortDate.Focus();
                    MyUtility.Msg.WarningBox("Arrive Port Date cannot be empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(this.dateDoxRcvDate.Value))
                {
                    this.dateDoxRcvDate.Focus();
                    MyUtility.Msg.WarningBox("Dox Rcv Date cannot be empty!");
                    return false;
                }
            }

            if (MyUtility.Check.Empty(this.txtInvoiceNo.Text))
            {
                this.txtInvoiceNo.Focus();
                MyUtility.Msg.WarningBox("Invoice No. cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtLocalSupp.TextBox1.Text))
            {
                this.txtLocalSupp.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Shipper cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Forwarder"]))
            {
                this.txtSubconForwarder.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Forwarder cannot be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.comboShippMode.Text))
            {
                this.comboShippMode.Select();
                MyUtility.Msg.WarningBox("Shipping Mode cannot be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(MyUtility.Convert.GetInt(this.numPackages.Text)))
            {
                this.numPackages.Focus();
                MyUtility.Msg.WarningBox("Packages cannot be 0!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.numCBM.Value))
            {
                this.numCBM.Focus();
                MyUtility.Msg.WarningBox("CBM cannot be 0!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtConsignee.Text))
            {
                this.txtConsignee.Focus();
                MyUtility.Msg.WarningBox("Consignee cannot be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtBLAWBNo.Text))
            {
                this.txtBLAWBNo.Focus();
                MyUtility.Msg.WarningBox("B/L(AWB) No. cannot be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtVslvoyFltNo.Text))
            {
                this.txtVslvoyFltNo.Focus();
                MyUtility.Msg.WarningBox("Vsl voy/Flt No. cannot be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtUserHandle.TextBox1.Text))
            {
                this.txtUserHandle.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Handle cannot be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.dateOnBoardDate.Value))
            {
                this.dateOnBoardDate.Focus();
                MyUtility.Msg.WarningBox("On Board Date cannot be empty!!");
                return false;
            }

            #endregion

            // 提單號碼不可重複
            if (!MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]))
            {
                if (MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"]).IndexOf("'") != -1)
                {
                    MyUtility.Msg.WarningBox("B/L(AWB) No. can not enter the  '  character!!");
                    return false;
                }

                sqlCmd = string.Format("select ID from FtyExport WITH (NOLOCK) where BLNo = '{0}' and ID != '{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                if (MyUtility.Check.Seek(sqlCmd))
                {
                    this.txtBLAWBNo.Focus();
                    MyUtility.Msg.WarningBox("B/L(AWB) No. already exist.");
                    return false;
                }
            }

            // 表身不能沒有資料
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail cannot be empty");
                return false;
            }

            if (!this.IsDetailInserting)
            {
                // 檢查已存在ShareExpense資料是否[Shipping Mode]是否不同
                sqlCmd = string.Format(
                      @"select distinct ShipModeID
                            from ShareExpense WITH (NOLOCK) 
                            where (InvNo = '{0}' or WKNO = '{0}')
                            and len(ShipModeID) > 0
                            and Junk = 0 ",
                      MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                DBProxy.Current.Select(null, sqlCmd, out dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    bool bolHasValue = dataTable.AsEnumerable().Distinct().Where(x => !x["ShipModeID"].Equals(this.comboShippMode.SelectedValue.ToString())).ToList().Count() > 0;
                    if (bolHasValue)
                    {
                        MyUtility.Msg.WarningBox("Can not revise < Shipping Mode > because share expense shipping mode is different.");
                        this.comboShippMode.SelectedIndex = -1;
                        return false;
                    }
                }
            }

            // 加總表身欄位回寫表頭
            double nw = 0.0, gw = 0.0;
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    nw = MyUtility.Math.Round(nw + MyUtility.Convert.GetDouble(dr["NetKg"]), 2);
                    gw = MyUtility.Math.Round(gw + MyUtility.Convert.GetDouble(dr["WeightKg"]), 2);
                }
            }

            this.CurrentMaintain["NetKg"] = nw;
            this.CurrentMaintain["WeightKg"] = gw;

            if (this.IsDetailInserting)
            {
                string newID = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("select RgCode from System WITH (NOLOCK) ").Trim() + "FE", "FtyExport", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(newID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = newID;
            }

            if (this.CurrentMaintain["Type"].ToString() == "2" && MyUtility.Check.Empty(this.txtSisFtyWK.Text))
            {
                this.txtSisFtyWK.Focus();
                DialogResult questionResult = MyUtility.Msg.QuestionBox($@" [Sis Fty WK#] is empty. Do you want to save data?", caption: "Question", buttons: MessageBoxButtons.YesNo);

                if (questionResult == DialogResult.No)
                {
                    return false;
                }
            }

            // 已經有做出口費用分攤，不能勾選[No Import/Export Charge]
            if (MyUtility.Check.Seek(string.Format(@"select WKNO from ShareExpense WITH (NOLOCK) where InvNo = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
                && this.chkNoCharge.Checked)
            {
                MyUtility.Msg.WarningBox("This WK# has share expense, please unselect [No Import/Export Charge].");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            // 已經有做出口費用分攤就不可以被刪除
            if (MyUtility.Check.Seek(string.Format(@"select ShippingAPID from ShareExpense WITH (NOLOCK) where InvNo = '{0}' or WKNO = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("This record have expense data, can't be deleted!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // Port of Loading按右鍵
        private void TxtPortofLoading_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select ID,CountryID from Port WITH (NOLOCK) where Junk = 0";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlWhere, "20,3", this.txtPortofLoading.Text);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> portData;
            portData = item.GetSelecteds();
            this.CurrentMaintain["ExportPort"] = item.GetSelectedString();
            this.CurrentMaintain["ExportCountry"] = portData[0]["CountryID"];
        }

        // Port of Discharge按右鍵
        private void TxtPortofDischarge_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select ID,CountryID from Port WITH (NOLOCK) where Junk = 0";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlWhere, "20,3", this.txtPortofDischarge.Text);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> portData;
            portData = item.GetSelecteds();
            this.CurrentMaintain["ImportPort"] = item.GetSelectedString();
            this.CurrentMaintain["ImportCountry"] = portData[0]["CountryID"];
        }

        // Radio
        private void RadioPanel1_ValueChanged(object sender, EventArgs e)
        {
            if (this.radioPanel1.Value == "3")
            {
                this.labelArrivePortDate.Text = "Ship Date";
            }
            else
            {
                this.labelArrivePortDate.Text = "Arrive Port Date";
            }

            if (this.EditMode)
            {
                foreach (DataRow dr in this.DetailDatas)
                {
                    dr.Delete();
                }
            }
        }

        // Expense Data
        private void BtnExpenseData_Click(object sender, EventArgs e)
        {
            P05_ExpenseData callNextForm = new P05_ExpenseData(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["Type"]) == "3" ? "InvNo" : "WKNo", false);
            callNextForm.ShowDialog(this);
        }

        // Import Data
        private void BtnImportData_Click(object sender, EventArgs e)
        {
            switch (this.CurrentMaintain["Type"].ToString())
            {
                case "1":
                    P04_Import3rd call3rdForm = new P04_Import3rd((DataTable)this.detailgridbs.DataSource);
                    call3rdForm.ShowDialog(this);
                    break;
                case "2":
                    P04_ImportTransferIn callTransferInForm = new P04_ImportTransferIn((DataTable)this.detailgridbs.DataSource);
                    callTransferInForm.ShowDialog(this);
                    break;
                case "3":
                    P04_ImportTransferOut callTransferOutForm = new P04_ImportTransferOut((DataTable)this.detailgridbs.DataSource);
                    callTransferOutForm.ShowDialog(this);
                    break;
                case "4":
                    P04_ImportLocalPO callLocalPOForm = new P04_ImportLocalPO((DataTable)this.detailgridbs.DataSource);
                    callLocalPOForm.ShowDialog(this);
                    break;
                default:
                    break;
            }
        }

        private void RadioTransferIn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                if (this.radioTransferIn.Checked)
                {
                    this.txtSisFtyWK.ReadOnly = false;
                }
                else
                {
                    if (this.CurrentMaintain != null)
                    {
                        this.txtSisFtyWK.ReadOnly = true;
                        this.CurrentMaintain["SisFtyID"] = string.Empty;
                    }
                }
            }
            else
            {
                this.txtSisFtyWK.ReadOnly = true;
            }
        }

        private void ControlColor()
        {
            string col = MyUtility.Convert.GetString(this.CurrentMaintain["Type"]) == "3" ? "InvNo" : "WKNo";
            DataTable gridData;
            string sqlCmd = string.Empty;

            switch (col)
            {
                case "InvNo":
                    sqlCmd = string.Format(
                        @"select 1
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN SciFMS_AccountNo a on se.AccountID = a.ID
where se.InvNo = '{0}' and se.junk=0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                    break;
                case "WKNo":
                    sqlCmd = string.Format(
                        @"select 1
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN SciFMS_AccountNo a on se.AccountID = a.ID
where se.WKNo = '{0}' and se.junk=0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                    break;
                default:
                    sqlCmd = "select 1 from ShareExpense WITH (NOLOCK) where 1=2";
                    break;
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (gridData.Rows.Count > 0)
            {
                this.btnExpenseData.ForeColor = Color.Blue;
            }
            else
            {
                this.btnExpenseData.ForeColor = Color.Black;
            }
        }
    }
}

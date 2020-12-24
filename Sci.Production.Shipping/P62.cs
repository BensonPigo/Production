using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tems;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P62 : Sci.Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P62(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;

            // 關閉表身Grid DoubleClick 會新增row的功能
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.labelNotApprove.Text = this.CurrentMaintain["status"].ToString();

            this.ReCalculat();
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select ked2.* 
,[PoNo] = o.CustPONo
,[DiffNw] = ked2.NetKg - ked2.ActNetKg
,[DiffGW] = ked2.WeightKg - ked2.ActWeightKg
,[TtlFOB] = ked2.POPrice * ked2.ShipModeSeqQty
,[StyleID] = s.ID
from KHExportDeclaration_Detail ked2
left join Style s on ked2.StyleUkey = s.Ukey
left join orders o on o.ID = ked2.OrderID
where ked2.id = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings col_ActNW = new DataGridViewGeneratorNumericColumnSettings();
            col_ActNW.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null && !this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                decimal actNW = MyUtility.Convert.GetDecimal(e.FormattedValue);
                dr["DiffNW"] = MyUtility.Convert.GetDecimal(dr["NetKg"]) - actNW;
                dr["ActNetKg"] = e.FormattedValue;
                dr.EndEdit();
            };

            DataGridViewGeneratorNumericColumnSettings col_ActGW = new DataGridViewGeneratorNumericColumnSettings();
            col_ActGW.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null && !this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                decimal actGW = MyUtility.Convert.GetDecimal(e.FormattedValue);
                dr["DiffGW"] = MyUtility.Convert.GetDecimal(dr["WeightKg"]) - actGW;
                dr["ActWeightKg"] = e.FormattedValue;
                dr.EndEdit();
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
           .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
           .Text("PoNo", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
           .Text("INVNo", header: "Invoice No", width: Widths.AnsiChars(25), iseditingreadonly: true)
           .Date("ETD", header: "ETD", width: Widths.AnsiChars(12), iseditingreadonly: true)
           .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
           .Numeric("ShipModeSeqQty", header: "Qty", width: Widths.AnsiChars(9), decimal_places: 0, integer_places: 9, iseditingreadonly: true)
           .Numeric("CTNQty", header: "CTN", width: Widths.AnsiChars(9), decimal_places: 0, integer_places: 9, iseditingreadonly: true)
           .Numeric("POPrice", header: "FOB", width: Widths.AnsiChars(9), decimal_places: 4, integer_places: 5, iseditingreadonly: true)
           .Numeric("TtlFOB", header: "Ttl FOB", width: Widths.AnsiChars(11), decimal_places: 6, integer_places: 5, iseditingreadonly: true)
           .Text("LocalINVNo", header: "Local Invoice No", width: Widths.AnsiChars(25), iseditingreadonly: false)
           .Numeric("NetKg", header: "N.W.", width: Widths.AnsiChars(9), decimal_places: 3, integer_places: 6, iseditingreadonly: true)
           .Numeric("WeightKg", header: "G.W.", width: Widths.AnsiChars(9), decimal_places: 3, integer_places: 6, iseditingreadonly: true)
           .Numeric("ActNetKg", header: "Act N.W.", width: Widths.AnsiChars(9), decimal_places: 3, integer_places: 6, iseditingreadonly: false, settings: col_ActNW)
           .Numeric("ActWeightKg", header: "Act G.W.", width: Widths.AnsiChars(9), decimal_places: 3, integer_places: 6, iseditingreadonly: false, settings: col_ActGW)
           .Numeric("DiffNW", header: "Diff N.W.", width: Widths.AnsiChars(9), decimal_places: 3, integer_places: 6, iseditingreadonly: true)
           .Numeric("DiffGW", header: "Diff G.W.", width: Widths.AnsiChars(9), decimal_places: 3, integer_places: 6, iseditingreadonly: true)
           .Text("Description", header: "Style Description", width: Widths.AnsiChars(30), iseditingreadonly: false)
           .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(14), iseditingreadonly: false)
           .Text("COFormType", header: "CO Form Type", width: Widths.AnsiChars(20), iseditingreadonly: false)
           .Text("COID", header: "COID", width: Widths.AnsiChars(25), iseditingreadonly: false)
           .Date("CODate", header: "CO Date", width: Widths.AnsiChars(10), iseditingreadonly: false);
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            this.InitReadOnly(true);
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.CurrentMaintain["Status"] = "New";
            this.InitReadOnly(false);
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            if (this.IsDetailInserting)
            {
                this.InitReadOnly(false);
            }
            else
            {
                this.InitReadOnly(true);
            }

            base.ClickUndo();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            this.InitReadOnly(true);
            return base.ClickSave();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string sqlcmd = $@"
update KHExportDeclaration
set Status='Confirmed'
,EditDate = GetDate()
,EditName = '{Env.User.UserID}'
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail !\r\n" + result.ToString());
                return;
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            string sqlcmd = $@"
update KHExportDeclaration
set Status='New'
,EditDate = GetDate()
,EditName = '{Env.User.UserID}'
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("UnConfirmed fail !\r\n" + result.ToString());
                return;
            }

            MyUtility.Msg.InfoBox("UnConfirmed successful");
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            // 表頭檢查
            if (MyUtility.Check.Empty(this.CurrentMaintain["Buyer"]) || MyUtility.Check.Empty(this.CurrentMaintain["ShipModeID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["CustCDID"]) || MyUtility.Check.Empty(this.CurrentMaintain["Dest"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Cdate"]) || MyUtility.Check.Empty(this.CurrentMaintain["DeclareNo"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Forwarder"]) || MyUtility.Check.Empty(this.CurrentMaintain["ExportPort"]))
            {
                MyUtility.Msg.WarningBox(@"<Buyer>, <Shipmode>, <CustCD>, <Destination>, <Declaration Date>, < Declaration#>, <Forwarder>, <Loading> cannot be empty.");
                return false;
            }

            // 表身檢查
            if (this.DetailDatas.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Please input <Invoice NO> first.");
                return false;
            }

            string sqlcmd = string.Empty;
            foreach (DataRow dr in this.DetailDatas)
            {
                // <Local Invoice No.>、<N.W.>、<G.W.>、<HS Code>不能為空
                if (MyUtility.Check.Empty(dr["LocalINVNo"]) ||
                    MyUtility.Check.Empty(dr["NetKg"]) ||
                    MyUtility.Check.Empty(dr["WeightKg"]) ||
                    MyUtility.Check.Empty(dr["HSCode"]))
                {
                    MyUtility.Msg.WarningBox(@"<Local Invoice No.>, <N.W.>, <G.W.>and <HS Code> cannot be empty.");
                    return false;
                }

                // 若<Shipmode>為A/C或A/P則<CO Form Type>、<CO#>、<CO Date>必須要有值
                if (this.CurrentMaintain["ShipModeID"].ToString() == "A/C" || this.CurrentMaintain["ShipModeID"].ToString() == "A/P")
                {
                    if (MyUtility.Check.Empty(dr["COFormType"]) ||
                   MyUtility.Check.Empty(dr["COID"]) ||
                   MyUtility.Check.Empty(dr["CODate"]))
                    {
                        MyUtility.Msg.WarningBox(@"<CO Form Type>, <CO#>and<CO Date> cannot be empty if <Shipmode> is A/C or A/P");
                        return false;
                    }
                }

                // 判斷表身[Invoice NO.]的Shipper和表頭的Shipper不相同
                sqlcmd = $@"select * from GMTBooking where ID = '{dr["INVNo"]}' and Shipper <> '{this.CurrentMaintain["Shipper"]}'";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox($@"The <Shipper> of <Invoice NO.>: {dr["INVNo"]} is not belong to this [Shipper]");
                    return false;
                }

                // 判斷表身[Invoice NO.]的BrandID對應Buyer與表頭Buyer相同
                sqlcmd = $@"select * from GMTBooking a inner join Brand b on a.BrandID = b.ID where a.ID = '{dr["INVNo"]}' and b.BuyerID != '{this.CurrentMaintain["Buyer"]}'";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox($@"The [Brand] of <Invoice NO.> : {dr["INVNo"]} is not belong to this [Brand].");
                    return false;
                }

                // 判斷表身[Invoice NO.]的ShipmodeID和表頭Shipmode相同
                sqlcmd = $@"select * from GMTBooking where ID = '{dr["INVNo"]}' and ShipModeID <> '{this.CurrentMaintain["ShipModeID"]}'";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox($@"The <Shipmode> of <Invoice NO.>: {dr["INVNo"]} is not belong to this [Shipmode]");
                    return false;
                }

                // 判斷表身[Invoice NO.]的CustCDID和表頭CustCD相同
                sqlcmd = $@"select * from GMTBooking where ID = '{dr["INVNo"]}' and CustCDID <> '{this.CurrentMaintain["CustCDID"]}'";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox($@"The <CustCD> of <Invoice NO.>: {dr["INVNo"]} is not belong to this [CustCD]");
                    return false;
                }

                // 判斷表身[Invoice NO.]的Dest和表頭Destination相同
                sqlcmd = $@"select * from GMTBooking where ID = '{dr["INVNo"]}' and dest <> '{this.CurrentMaintain["dest"]}'";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox($@"The <Destination> of <Invoice NO.>: {dr["INVNo"]} is not belong to this [Destination]");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["ActNetKg"]) || MyUtility.Check.Empty(dr["ActWeightKg"]))
                {
                    MyUtility.Msg.WarningBox(@"<Act N.W.]> or <Act G.W.> cannot be empty.");
                    return false;
                }
            }

            this.ReCalculat();

            // 取單號
            if (this.IsDetailInserting)
            {
                string getID = MyUtility.GetValue.GetID(Env.User.Keyword + "KE", "KHExportDeclaration");
                if (MyUtility.Check.Empty(getID))
                {
                    MyUtility.Msg.ErrorBox("Get ID fail !!");
                    return false;
                }

                this.CurrentMaintain["id"] = getID;
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// 將表身資料加總後更新到表頭
        /// </summary>
        private void ReCalculat()
        {
            decimal ttlDecQty = 0, ttlDecCTN = 0, ttlDecNW = 0, ttlDecGW = 0, ttlDecAmount = 0, ttlActDecNW = 0, ttlActDecGW = 0;
            foreach (DataRow dr in this.DetailDatas)
            {
                ttlDecQty += MyUtility.Convert.GetDecimal(dr["ShipModeSeqQty"]);
                ttlDecCTN += MyUtility.Convert.GetDecimal(dr["CTNQty"]);
                ttlDecNW += MyUtility.Convert.GetDecimal(dr["NetKg"]);
                ttlDecGW += MyUtility.Convert.GetDecimal(dr["WeightKg"]);
                ttlActDecNW += MyUtility.Convert.GetDecimal(dr["ActNetKg"]);
                ttlActDecGW += MyUtility.Convert.GetDecimal(dr["ActWeightKg"]);
                ttlDecAmount += MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["ShipModeSeqQty"]) * MyUtility.Convert.GetDecimal(dr["POPrice"]), 4);
            }

            this.numDecQty.Value = ttlDecQty;
            this.numDecCTN.Value = ttlDecCTN;
            this.numTtlDeclGW.Value = ttlDecGW;
            this.numTtlDeclNW.Value = ttlDecNW;
            this.numTtlActDeclNW.Value = ttlActDecNW;
            this.numTtlActDeclGW.Value = ttlActDecGW;
            this.numDecAmount.Value = ttlDecAmount;
        }

        /// <summary>
        /// 設定是否可以編輯
        /// </summary>
        /// <param name="readOnly">bool</param>
        private void InitReadOnly(bool readOnly)
        {
            // Status=New, 可以編輯
            bool isNewStatus = false;
            if (this.CurrentMaintain["Status"].EqualString("NEW"))
            {
                isNewStatus = true;
            }

            this.comboShipper.ReadOnly = readOnly;
            this.txtcountry.TextBox1.ReadOnly = readOnly;
            this.dateDeclarationDate.ReadOnly = isNewStatus ? false : readOnly;
            this.txtDeclaration.ReadOnly = isNewStatus ? false : readOnly;
            this.txtbuyer.ReadOnly = readOnly;
            this.txtshipmode.ReadOnly = readOnly;
            this.txtcustcd.ReadOnly = readOnly;
            this.txtForwarder.TextBox1.ReadOnly = isNewStatus ? false : readOnly;
            this.txtLoadingPort.ReadOnly = readOnly;
            this.btnBatchImport.Enabled = isNewStatus ? true : false;
            this.gridicon.Enabled = isNewStatus ? true : false;
            this.detailgrid.IsEditable = isNewStatus ? true : false;
            this.InsertDetailGridOnDoubleClick = isNewStatus ? true : false;
        }

        private void BtnBatchImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["custCDID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["shipModeID"]))
            {
                MyUtility.Msg.WarningBox("[CustCD], [Shipmode] cannot be empty!");
                return;
            }

            var frm = new P62_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void TxtLoadingPort_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain) && this.EditMode)
            {
                string sqlcmd = @"select ID, Name from Port where CountryID ='KH' and Junk =0";
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "15,20", this.CurrentMaintain["ExportPort"].ToString(), "ID,Name");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["ExportPort"] = item.GetSelecteds()[0]["ID"];
            }
        }

        private void TxtLoadingPort_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain) && this.EditMode && !MyUtility.Check.Empty(this.CurrentMaintain["ExportPort"]))
            {
                DataRow dr;
                string sqlcmd = $@"select ID, Name from Port where CountryID ='KH' and Junk =0 and id ='{this.CurrentMaintain["ExportPort"]}'";
                if (!MyUtility.Check.Seek(sqlcmd, out dr))
                {
                    MyUtility.Msg.WarningBox("Cannot find this [Loading (Port)].");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    this.CurrentMaintain["ExportPort"] = dr["ID"];
                }
            }
        }
    }
}

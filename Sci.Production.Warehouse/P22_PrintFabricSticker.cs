using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P22_PrintFabricSticker : Win.Tems.QueryForm
    {
        private DataTable fromdt;
        private DataTable todt;
        private string printType;

        /// <inheritdoc/>
        public P22_PrintFabricSticker(DataTable fromdt, DataTable todt, string printType)
        {
            this.InitializeComponent();
            this.fromdt = fromdt;
            this.todt = todt;
            this.printType = printType;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            DataGridViewGeneratorCheckBoxColumnSettings col_Selected = new DataGridViewGeneratorCheckBoxColumnSettings { HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None };
            col_Selected.CellEditable += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                e.IsEditable = MyUtility.Convert.GetDecimal(dr["Qty"]) > 0;
            };
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0, settings: col_Selected)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Text("Seq", header: "Seq#", iseditingreadonly: true)
                .Text("Roll", header: "Roll", iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", iseditingreadonly: true)
                .Text("TransQty", header: "Total Trans. Qty", iseditingreadonly: true)
                .Text("Qty", header: "Balance Qty", iseditingreadonly: true)
                .Text("Barcode", header: "QR Code", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 ;

            DataGridViewGeneratorCheckBoxColumnSettings col_Selected2 = new DataGridViewGeneratorCheckBoxColumnSettings { HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None };
            col_Selected2.CellEditable += (s, e) =>
            {
                DataRow dr = this.grid2.GetDataRow(e.RowIndex);
                e.IsEditable = MyUtility.Convert.GetDecimal(dr["Qty"]) > 0;
            };
            this.grid2.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0, settings: col_Selected2)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Text("Seq", header: "Seq#", iseditingreadonly: true)
                .Text("Roll", header: "Roll", iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", iseditingreadonly: true)
                .Text("Qty", header: "Receive Qty", iseditingreadonly: true)
                .Text("Barcode", header: "QR Code", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 ;

            this.listControlBindingSource1.DataSource = this.fromdt;
            this.listControlBindingSource2.DataSource = this.todt;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (this.fromdt.Select("Sel = 1").Length == 0 && this.todt.Select("Sel = 1").Length == 0)
            {
                MyUtility.Msg.WarningBox("Select data first.");
                return;
            }

            DataTable dt = this.fromdt.Clone();
            foreach (DataRow row in this.fromdt.Select("Sel = 1"))
            {
                dt.ImportRow(row);
            }

            foreach (DataRow row in this.todt.Select("Sel = 1"))
            {
                dt.ImportRow(row);
            }

            P07_QRCodeSticker.PrintQRCode_RDLC(dt.AsEnumerable().ToList(), this.printType, "P22");
        }
    }
}

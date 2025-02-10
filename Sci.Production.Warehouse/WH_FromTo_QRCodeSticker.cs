using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicForm;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class WH_FromTo_QRCodeSticker : Win.Tems.QueryForm
    {
        private DataTable fromdt;
        private DataTable todt;
        private string printType;
        private string callFrom;

        /// <inheritdoc/>
        public WH_FromTo_QRCodeSticker(DataTable fromdt, DataTable todt, string printType, string callFormName)
        {
            this.InitializeComponent();
            this.fromdt = fromdt;
            this.todt = todt;
            this.callFrom = callFormName;
            this.printType = printType;
            if (callFormName == "P22")
            {
                this.labFrom.Text = "From Bulk";
                this.labTo.Text = "To Inventory";
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            DataGridViewGeneratorCheckBoxColumnSettings col_Selected = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_Selected.CellEditable += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                e.IsEditable = MyUtility.Convert.GetDecimal(dr["Qty"]) > 0 && !MyUtility.Check.Empty(dr["Barcode"]);
            };

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0, settings: col_Selected)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Text("Seq", header: "Seq#", iseditingreadonly: true)
                .Text("Roll", header: "Roll", iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", iseditingreadonly: true)
                .Text("TransQty", header: "Total Trans. Qty", iseditingreadonly: true)
                .Numeric("Qty", header: "Balance Qty", decimal_places: 2, iseditingreadonly: true)
                .Text("Barcode", header: "QR Code", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 ;

            DataGridViewGeneratorCheckBoxColumnSettings col_Selected2 = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_Selected2.CellEditable += (s, e) =>
            {
                DataRow dr = this.grid2.GetDataRow(e.RowIndex);
                e.IsEditable = MyUtility.Convert.GetDecimal(dr["Qty"]) > 0;
                e.IsEditable = MyUtility.Convert.GetDecimal(dr["Qty"]) > 0 && !MyUtility.Check.Empty(dr["Barcode"]);
            };

            this.grid2.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0, settings: col_Selected2)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Text("Seq", header: "Seq#", iseditingreadonly: true)
                .Text("Roll", header: "Roll", iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", iseditingreadonly: true)
                .Numeric("Qty", header: "Receive Qty", decimal_places: 2, iseditingreadonly: true)
                .Text("Barcode", header: "QR Code", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 ;

            this.listControlBindingSource1.DataSource = this.fromdt;
            this.listControlBindingSource2.DataSource = this.todt;

            #region 更改To From label text
            switch (this.callFrom)
            {
                case "P22":
                    this.labFrom.Text = "From Bulk";
                    this.labTo.Text = "To Inventory";
                    break;
                case "P24":
                    this.labFrom.Text = "From Bulk";
                    this.labTo.Text = "To Scrap";
                    break;
                case "P25":
                    this.labFrom.Text = "From Bulk";
                    this.labTo.Text = "To Scrap";
                    break;
                case "P36":
                    this.labFrom.Text = "From Scrap";
                    this.labTo.Text = "To Inventory";
                    break;
                case "P31":
                    this.labFrom.Text = "Borrow From";
                    this.labTo.Text = "Borrow To";
                    break;
                case "P32":
                    this.labFrom.Text = "Borrow From";
                    this.labTo.Text = "Borrow To";
                    break;
                default:
                    break;
            }
            #endregion
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

            WH_Receive_QRCodeSticker.PrintQRCode_RDLC(dt.AsEnumerable().ToList(), this.printType, "P22");
        }

        private void Grid1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                this.grid1.ValidateControl();
                foreach (DataRow row in this.fromdt.Rows)
                {
                    row["Sel"] = MyUtility.Convert.GetDecimal(row["Qty"]) > 0;
                }
            }
        }

        private void Grid2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                this.grid2.ValidateControl();
                foreach (DataRow row in this.todt.Rows)
                {
                    row["Sel"] = MyUtility.Convert.GetDecimal(row["Qty"]) > 0;
                }
            }
        }
    }
}

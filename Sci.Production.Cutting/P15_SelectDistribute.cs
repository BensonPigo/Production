using Ict;
using Ict.Win;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P15_SelectDistribute : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public DataTable Sel_Distribute { get; set; }

        private readonly int type;
        private readonly P15 p15;
        private DataTable Dt_Distribute;

        /// <inheritdoc/>
        public P15_SelectDistribute(DataTable workOrder_Distribute, P15 p15, int type)
        {
            this.InitializeComponent();
            this.Dt_Distribute = workOrder_Distribute;
            this.p15 = p15;
            this.type = type;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Filter(false);
        }

        private void GridSetup()
        {
            DataGridViewGeneratorCheckBoxColumnSettings sel = new DataGridViewGeneratorCheckBoxColumnSettings
            {
                HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None, // 取消全勾選事件
            };

            DataGridViewGeneratorNumericColumnSettings cutoutput = new DataGridViewGeneratorNumericColumnSettings();
            cutoutput.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                int oldValue = MyUtility.Convert.GetInt(dr["cutOutput"]);
                if (this.p15.GetBalancebyDistribute(dr, MyUtility.Convert.GetInt(e.FormattedValue)) > 0)
                {
                    MyUtility.Msg.InfoBox($"[{dr["OrderID"]}][{dr["Article"]}][{dr["SizeCode"]}] can't exceed the work order distribute qty.");
                    dr["cutOutput"] = oldValue;
                    dr.EndEdit();
                    return;
                }

                dr["cutOutput"] = e.FormattedValue;
                dr.EndEdit();
            };
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: sel)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("cutoutput", header: "Qty", width: Widths.AnsiChars(3), settings: cutoutput)
                .Text("isEXCESS", header: "EXCESS", width: Widths.AnsiChars(2), iseditingreadonly: true)
                ;

            this.grid1.DataSource = this.Dt_Distribute;
            if (this.type == 2)
            {
                this.grid1.Columns["Sel"].Visible = false;
            }
        }

        private void Grid1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                this.Filter(e.RowIndex == -1);
            }
        }

        private void Filter(bool clickheader)
        {
            this.grid1.ValidateControl();
            DataRow[] selrow = this.Dt_Distribute.Select("Sel = 1");
            this.Dt_Distribute.DefaultView.RowFilter = string.Empty;
            if (selrow.Any())
            {
                string filter = $"Article = '{selrow[0]["Article"]}' and SizeCode = '{selrow[0]["SizeCode"]}'";
                this.Dt_Distribute.DefaultView.RowFilter = filter;
                bool nosel = this.Dt_Distribute.DefaultView.ToTable().AsEnumerable().Where(w => !MyUtility.Convert.GetBool(w["sel"])).Any();
                this.Dt_Distribute.Select(filter).ToList().Where(w => clickheader).ToList().ForEach(r => r["sel"] = nosel);
                bool sel = this.Dt_Distribute.DefaultView.ToTable().AsEnumerable().Where(w => MyUtility.Convert.GetBool(w["sel"])).Any();
                if (!sel)
                {
                    this.Dt_Distribute.DefaultView.RowFilter = string.Empty;
                }
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (this.Dt_Distribute.Rows.Count == 0)
            {
                return;
            }

            if (this.type == 1)
            {
                if (!this.Dt_Distribute.Select("Sel = 1").Any())
                {
                    MyUtility.Msg.WarningBox("Please select data!");
                    return;
                }

                this.Sel_Distribute = this.Dt_Distribute.Select("Sel = 1").CopyToDataTable();
            }
            else if (this.type == 2)
            {
                if (this.grid1.CurrentDataRow == null)
                {
                    this.grid1.CurrentCell = this.grid1.Rows[0].Cells[1]; // 移動到指定cell
                }

                this.grid1.CurrentDataRow["sel"] = 1;
                this.Sel_Distribute = null;
                this.Sel_Distribute = this.Dt_Distribute.Clone();
                this.Sel_Distribute.ImportRow(this.grid1.CurrentDataRow);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

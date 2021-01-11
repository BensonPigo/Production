using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class B10_MoistureStandardList : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public string BrandID { get; set; }

        /// <inheritdoc/>
        public B10_MoistureStandardList()
        {
            this.InitializeComponent();
            this.Text = $"QA B10. Moisture Standard List ({this.BrandID})";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("MaterialCompositionGrp", header: "Material Composition Group", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("MaterialCompositionItem", header: "Material Composition Item", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("MoistureStandardDesc", header: "Moisture Standard", width: Widths.AnsiChars(2), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            string sqlcmd = $@"select *from Brand_QAMoistureStandardList b where BrandID = '{this.BrandID}'";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void Grid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0 || this.grid1.Columns[e.ColumnIndex].Name != "MaterialCompositionGrp")
            {
                return;
            }

            if (this.IsTheSamePreviousCellValue("MaterialCompositionGrp", e.RowIndex, this.grid1))
            {
                e.Value = string.Empty;
                e.FormattingApplied = true;
            }
        }

        private void Grid1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex == this.grid1.Rows.Count - 1 || e.ColumnIndex < 0)
            {
                return;
            }

            if (this.grid1.Columns[e.ColumnIndex].Name != "MaterialCompositionGrp")
            {
                return;
            }

            e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            if (this.IsTheSameNextCellValue("MaterialCompositionGrp", e.RowIndex, this.grid1))
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Bottom = this.grid1.AdvancedCellBorderStyle.Bottom;
            }
        }

        private bool IsTheSamePreviousCellValue(string column, int row, DataGridView tarGrid)
        {
            DataGridViewCell cell1 = tarGrid[column, row];
            DataGridViewCell cell2 = tarGrid[column, row - 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private bool IsTheSameNextCellValue(string column, int row, DataGridView tarGrid)
        {
            DataGridViewCell cell1 = tarGrid[column, row];
            DataGridViewCell cell2 = tarGrid[column, row + 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }
    }
}

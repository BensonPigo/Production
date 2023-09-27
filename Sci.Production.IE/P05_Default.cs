using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.Production.IE.AutoLineMappingGridSyncScroll;

namespace Sci.Production.IE
{
    /// <summary>
    /// P05_Default
    /// </summary>
    public partial class P05_Default : Sci.Win.Tems.QueryForm
    {
        private AutoLineMappingGridSyncScroll autoLineMappingGridSyncScroll;

        /// <summary>
        /// P05_Default
        /// </summary>
        /// <param name="dtAutomatedLineMapping_DetailAuto">dtAutomatedLineMapping_DetailAuto</param>
        public P05_Default(DataTable dtAutomatedLineMapping_DetailAuto)
        {
            this.InitializeComponent();

            this.gridMain.CellFormatting += this.GridMain_CellFormatting;

            this.gridMainBs.DataSource = dtAutomatedLineMapping_DetailAuto;
            this.gridMain.DataSource = this.gridMainBs;
            this.gridMainBs.Filter = " PPA <> 'C' and IsNonSewingLine = 0";

            this.autoLineMappingGridSyncScroll = new AutoLineMappingGridSyncScroll(this.gridMain, this.gridSub, "No", SubGridType.LineMapping);
        }

        private void GridMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Win.UI.Grid sourceGrid = (Win.UI.Grid)sender;
            DataRow dr = sourceGrid.GetDataRow(e.RowIndex);
            if (e.ColumnIndex > 1)
            {
                e.CellStyle.BackColor = MyUtility.Convert.GetInt(dr["TimeStudyDetailUkeyCnt"]) > 1 ? Color.FromArgb(255, 255, 153) : sourceGrid.DefaultCellStyle.BackColor;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridMain)
               .Text("No", header: "No", width: Widths.AnsiChars(4))
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("PPADesc", header: "PPA", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("MachineTypeID", header: "ST/MC type", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(23), iseditingreadonly: true)
               .Text("Attachment", header: "Attachment", width: Widths.AnsiChars(10))
               .Text("SewingMachineAttachmentID", header: "Part ID", width: Widths.AnsiChars(25))
               .Text("Template", header: "Template", width: Widths.AnsiChars(10))
               .Numeric("GSD", header: "GSD Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
               .Numeric("SewerDiffPercentageDesc", header: "%", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("DivSewer", header: "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("OriSewer", header: "Ori. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Text("ThreadComboID", header: "Thread" + Environment.NewLine + "Combination", width: Widths.AnsiChars(10));

            this.Helper.Controls.Grid.Generator(this.gridSub)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10))
               .Text("TotalGSDTime", header: "Total" + Environment.NewLine + "GSD Time", width: Widths.AnsiChars(10))
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10));

            this.autoLineMappingGridSyncScroll.RefreshSubData();
            this.numLBR.Value = this.autoLineMappingGridSyncScroll.LBR;
            this.numHighestGSD.Value = this.autoLineMappingGridSyncScroll.HighestGSD;
        }
    }
}

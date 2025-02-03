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
using static Sci.Production.IE.AutoLineMappingGridSyncScroll;

namespace Sci.Production.IE
{
    /// <summary>
    /// P06_Default
    /// </summary>
    public partial class P06_Default : Sci.Win.Tems.QueryForm
    {
        private AutoLineMappingGridSyncScroll autoLineMappingGridSyncScroll;

        /// <summary>
        /// P06_Default
        /// </summary>
        /// <param name="automatedLineMappingID">automatedLineMappingID</param>
        public P06_Default(string automatedLineMappingID)
        {
            this.InitializeComponent();

            this.gridMain.CellFormatting += this.GridMain_CellFormatting;

            string sqlGetAutomatedLineMappingID = $@"
            select  cast(0 as bit) as Selected,
                    ad.*,
                    [PPADesc] = isnull(d.Name, ''),
                    [OperationDesc] = iif(isnull(op.DescEN, '') = '', ad.OperationID, op.DescEN),
                    [SewerDiffPercentageDesc] = round(ad.SewerDiffPercentage * 100, 0),
                    [TimeStudyDetailUkeyCnt] = Count(TimeStudyDetailUkey) over (partition by TimeStudyDetailUkey),
                    [IsNotShownInP05] = isnull(md.IsNotShownInP05,0) 
            from AutomatedLineMapping_Detail ad WITH (NOLOCK)
            left join AutomatedLineMapping alm on alm.ID = ad.ID
            left join MachineType_Detail md on md.ID = ad.MachineTypeID  and md.FactoryID = alm.FactoryID
            left join DropDownList d with (nolock) on d.ID = ad.PPA  and d.Type = 'PMS_IEPPA'
            left join Operation op with (nolock) on op.ID = ad.OperationID
            where ad.ID = '{automatedLineMappingID}' and isnull(md.IsNotShownInP06,0) = 0
            order by iif(ad.No = '', 'ZZ', ad.No), ad.Seq";

            DataTable dtAutomatedLineMapping_Detail;

            DualResult result = DBProxy.Current.Select(null, sqlGetAutomatedLineMappingID, out dtAutomatedLineMapping_Detail);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridMainBs.DataSource = dtAutomatedLineMapping_Detail;
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
               .Numeric("Cycle", header: "Cycle Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
               .Numeric("SewerDiffPercentageDesc", header: "%", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("DivSewer", header: "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("OriSewer", header: "Ori. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Text("ThreadComboID", header: "Thread" + Environment.NewLine + "Combination", width: Widths.AnsiChars(10));

            this.Helper.Controls.Grid.Generator(this.gridSub)
               .Text("No", header: "No. Of" + Environment.NewLine + "Station", width: Widths.AnsiChars(10))
               .Text("TotalGSDTime", header: "Total" + Environment.NewLine + "Cycle Time", width: Widths.AnsiChars(10))
               .Text("OperatorLoading", header: "Operator" + Environment.NewLine + "Loading (%)", width: Widths.AnsiChars(10));

            this.autoLineMappingGridSyncScroll.RefreshSubData();
            this.numLBR.Value = this.autoLineMappingGridSyncScroll.LBR;
            this.numHighestGSD.Value = this.autoLineMappingGridSyncScroll.HighestGSD;
        }
    }
}

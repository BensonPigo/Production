using Ict;
using Ict.Win;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Sci.Production.Planning
{
    /// <inheritdoc/>
    public partial class P07 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Date", header: "Date", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("WeekDay", header: "WeekDay", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("DailyCPULoading", header: "Daily" + Environment.NewLine + "CPU Loading", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("NewTarget", header: "New Target base on Actual" + Environment.NewLine + "output and left working days", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("ActCPUPerformed", header: "Actual CPU " + Environment.NewLine + "Performed", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 3)
            .Numeric("DailyCPUVarience", header: "Daily CPU Varience" + Environment.NewLine + "(based on new target)", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 3)
            .Numeric("AccuLoading", header: "Accumu-lated" + Environment.NewLine + "Loading", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 3)
            .Numeric("AccuActCPUPerformed", header: "Accumu-lated Actual" + Environment.NewLine + "CPU Performed", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 3)
            .Numeric("AccuCPUVariance", header: "Accumu-lated CPU" + Environment.NewLine + "Variance", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 3)
            .Numeric("LeftWorkDays", header: "Left working days", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("AvgWorkhours", header: "Average" + Environment.NewLine + "w/hours", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            .Numeric("PPH", header: "PPH", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            .Numeric("Direct", header: "DIRECT", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("Active", header: "ACTIVE", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("VPH", header: "VPH", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            .Numeric("ManpowerRatio", header: "Manpower" + Environment.NewLine + "Ratio", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            .Numeric("LineNo", header: "No. of" + Environment.NewLine + "Line#", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("LineManpower", header: "Manpower" + Environment.NewLine + "/Line", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("GPH", header: "GPH", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            .Numeric("SPH", header: "SPH", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            ;
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["UKEY"].ToString();

            this.DetailSelectCommand = $@"
             SELECT  
             [DailyAccuCPULoadingUkey]
            ,[Date]
            ,[WeekDay]
            ,[DailyCPULoading]
            ,[NewTarget]
            ,[ActCPUPerformed]
            ,[DailyCPUVarience]
            ,[AccuLoading]
            ,[AccuActCPUPerformed]
            ,[AccuCPUVariance]
            ,[LeftWorkDays]
            ,[AvgWorkhours]
            ,[PPH]
            ,[Direct]
            ,[Active]
            ,[VPH]
            ,[ManpowerRatio]
            ,[LineNo]
            ,[LineManpower]
            ,[GPH]
            ,[SPH]
            FROM DailyAccuCPULoading_Detail where DailyAccuCPULoadingUkey = '{masterID}'";

            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnImportFromExcel_Click(object sender, EventArgs e)
        {
            var windowsForm = new P07_Import();
            windowsForm.ShowDialog(this);
            this.ReloadDatas();
        }
    }
}

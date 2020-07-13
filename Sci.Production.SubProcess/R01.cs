using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using System.Data.SqlClient;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.SubProcess
{
    /// <summary>
    /// R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private DataTable dtPrint = null;
        private int intRowHeaderCount = 2;
        DateTime? date1;
        DateTime? date2;

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.date1 = this.dateRange.Value1;
            this.date2 = this.dateRange.Value2;

            // Date 為必輸條件
            if (MyUtility.Check.Empty(this.date1) || MyUtility.Check.Empty(this.date2))
            {
                MyUtility.Msg.InfoBox(" Date can't empty!!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            this.dtPrint = null;
            string strStartDate = this.dateRange.Value1 == null ? string.Empty : Convert.ToDateTime(this.dateRange.Value1).ToString("yyyy/MM/dd");
            string strEndDate = this.dateRange.Value2 == null ? string.Empty : Convert.ToDateTime(this.dateRange.Value2).ToString("yyyy/MM/dd");
            string strDateFilte = string.IsNullOrEmpty(strStartDate) || string.IsNullOrEmpty(strEndDate) ? string.Empty : $"and spo.OutputDate between @StartDate and @EndDate";

            List<SqlParameter> listSqlParameters = new List<SqlParameter>();
            listSqlParameters.Add(new SqlParameter("@Type", this.txtType.Text));
            listSqlParameters.Add(new SqlParameter("@StartDate", strStartDate));
            listSqlParameters.Add(new SqlParameter("@EndDate", strEndDate));

            string strQuerySql = $@"
declare @Tms int;

select top 1 @Tms = stdTms
from System

select *
	   , Eff = Round ((TotalCPU * @Tms) / (TotalWorkingHour * 60 * 60), 2)
	   , PPH = Round (TotalCPU / TotalWorkingHour, 3)
from (
    select [Date] = spo.OutputDate
	       , Sewer = sum (spo.Manpower)
	       , TotalCPU = isnull (Round (sum (spod.TotalCPU), 0), 0)
	       , WorkingHour = sum (spo.WHour)
	       , TotalWorkingHour = Round (sum (spod.TotalWorkingHour), 0)
    from SubProcessOutput spo
    outer apply (
	    select TotalCPU = sum (spod.TotalCPU)
		       , TotalWorkingHour = sum (spod.TTLWorkingHours)
	    from SubProcessOutput_Detail spod
	    where spo.ID = spod.ID
    ) spod
    where spo.TypeID = @Type
	      and spo.Status = 'Confirmed'
          {strDateFilte}
    group by spo.OutputDate
) tmp";

            return DBProxy.Current.Select(null, strQuerySql, listSqlParameters, out this.dtPrint);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtPrint == null
                || this.dtPrint.Rows.Count == 0)
            {
                this.SetCount(0);
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dtPrint.Rows.Count);

            int intStdTms = Convert.ToInt32(MyUtility.GetValue.Lookup("select top 1 stdTms from System"));
            int intWorkingDate = this.dtPrint.AsEnumerable().Where(row => row["TotalCPU"].EqualDecimal(0) == false).Count();

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\SubProcess_R01.xltx");
            Excel.Worksheet workSheet = objApp.Sheets[1];

            for (int i = 1; i <= this.dtPrint.Rows.Count; i++)
            {
                workSheet.Cells[i + this.intRowHeaderCount, 2] = this.dtPrint.Rows[i - 1]["Date"];
                workSheet.Cells[i + this.intRowHeaderCount, 3] = this.dtPrint.Rows[i - 1]["Sewer"];
                workSheet.Cells[i + this.intRowHeaderCount, 4] = this.dtPrint.Rows[i - 1]["TotalCPU"];
                workSheet.Cells[i + this.intRowHeaderCount, 5] = this.dtPrint.Rows[i - 1]["WorkingHour"];
                workSheet.Cells[i + this.intRowHeaderCount, 6] = this.dtPrint.Rows[i - 1]["TotalWorkingHour"];
                workSheet.Cells[i + this.intRowHeaderCount, 7] = $"={this.dtPrint.Rows[i - 1]["Eff"]}";
                workSheet.Cells[i + this.intRowHeaderCount, 8] = this.dtPrint.Rows[i - 1]["PPH"];
            }

            string strStartDate = this.dateRange.Value1 == null ? string.Empty : Convert.ToDateTime(this.dateRange.Value1).ToString("yyyy/MM/dd");
            string strEndDate = this.dateRange.Value2 == null ? string.Empty : Convert.ToDateTime(this.dateRange.Value2).ToString("yyyy/MM/dd");

            workSheet.Cells[1, 2] = $"{this.txtType.Text} Monthly Output Report ({strStartDate} ~ {strEndDate})";
            workSheet.Cells[4, 11] = $"=K3*{intStdTms}/ 60";
            workSheet.Cells[7, 11] = intWorkingDate;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("SubProcess_R01");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workSheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}

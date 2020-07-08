using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using System.Data.SqlClient;
using Sci.Data;

// using Excel = Microsoft.Office.Interop.Excel;
using Sci.Utility.Excel;

namespace Sci.Production.Thread
{
    public partial class R22 : Win.Tems.PrintForm
    {
        private DateTime? Date_s;
        private DateTime? Date_e;
        private string RefNo_s = string.Empty;
        private string RefNo_e = string.Empty;
        private string Shade = string.Empty;
        private string Type = string.Empty;
        private List<SqlParameter> Parameters = new List<SqlParameter>();
        private string sqlWhere = string.Empty;

        private DataTable printData;

        public R22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            this.Parameters.Clear();

            if (MyUtility.Check.Empty(this.dateRange.Value1) && MyUtility.Check.Empty(this.dateRange.Value2))
            {
                MyUtility.Msg.WarningBox("<Date> can't be empty!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.dateRange.Value1))
            {
                this.Date_s = this.dateRange.Value1;
                this.Parameters.Add(new SqlParameter("@Date_s", this.Date_s));
            }

            if (!MyUtility.Check.Empty(this.dateRange.Value2))
            {
                this.Date_e = this.dateRange.Value2;
                this.Parameters.Add(new SqlParameter("@Date_e", this.Date_e.Value.AddDays(1).AddSeconds(-1)));
            }

            if (!MyUtility.Check.Empty(this.RefNo_Start.Text))
            {
                this.RefNo_s = this.RefNo_Start.Text;
                this.Parameters.Add(new SqlParameter("@RefNo_s", this.RefNo_s));
            }

            if (!MyUtility.Check.Empty(this.RefNo_End.Text))
            {
                this.RefNo_e = this.RefNo_End.Text;
                this.Parameters.Add(new SqlParameter("@RefNo_e", this.RefNo_e));
            }

            if (!MyUtility.Check.Empty(this.textShade.Text))
            {
                this.Shade = this.textShade.Text;
                this.Parameters.Add(new SqlParameter("@Shade", this.Shade));
            }

            this.Type = this.cmbType.Text;
            if (!MyUtility.Check.Empty(this.cmbType.Text))
            {
                this.Parameters.Add(new SqlParameter("@Type", this.Type));
            }

            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            this.sqlWhere = string.Empty;

            if (!MyUtility.Check.Empty(this.RefNo_Start.Text))
            {
                this.sqlWhere = " AND ts.Refno >= @Refno_s ";
            }

            if (!MyUtility.Check.Empty(this.RefNo_End.Text))
            {
                this.sqlWhere += " AND ts.Refno <= @RefNo_e ";
            }

            if (!MyUtility.Check.Empty(this.textShade.Text))
            {
                this.sqlWhere += " AND ts.ThreadColorID = @Shade ";
            }

            if (!MyUtility.Check.Empty(this.Type))
            {
                this.sqlWhere += " AND li.Category = @Type ";
            }

            StringBuilder sqlCmd = new StringBuilder();

            #region SQL

            sqlCmd.Append($@"
select ts.Refno
	,ts.ThreadColorID
into #tmp
from ThreadStock ts 
left join LocalItem li on li.RefNo = ts.RefNo
where 1=1
{this.sqlWhere}
group by ts.Refno,ts.ThreadColorID

select src.Refno
      ,src.ThreadColorID
	  , (select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= src.Refno) [Description]
	  ,(incoming_before.ttl_cone - issue_before.ttl_cone + adjust_before.ttl_cone) [Initial_Cone]
	  ,incoming_between.ttl_cone [Incoming_Cone] 
	  ,issue_between.ttl_cone [Issue_Cone] 
	  ,adjust_between.ttl_cone [Adjust_Cone]
	  -- Balance = Initial + incoming - issue + adjust
	  ,((incoming_before.ttl_cone - issue_before.ttl_cone + adjust_before.ttl_cone) + incoming_between.ttl_cone - issue_between.ttl_cone + adjust_between.ttl_cone)[Balance_Cone] 
from #tmp src
  -- incoming時間區間間內
  outer apply(select ISNULL(sum(isnull(tcd.NewCone, 0)), 0) + ISNULL(sum(isnull(tcd.UsedCone, 0)), 0) as ttl_cone 
				from ThreadIncoming tc
					, ThreadIncoming_Detail tcd 
				where 1=1
				and tc.id=tcd.ID
				and tcd.Refno = src.RefNo
				and tcd.ThreadColorID = src.ThreadColorID
				and tc.Status='Confirmed'
				--------paramter-----------
				and tc.Cdate between @Date_s and @Date_e 
				---------------------------
             ) incoming_between
  -- incoming時間區間以前
  outer apply(select ISNULL(sum(isnull(tcd.NewCone, 0)), 0) + ISNULL(sum(isnull(tcd.UsedCone, 0)), 0) as ttl_cone 
				from ThreadIncoming tc
					, ThreadIncoming_Detail tcd 
				where 1=1
				and tc.id=tcd.ID
				and tcd.Refno = src.RefNo
				and tcd.ThreadColorID = src.ThreadColorID
				and tc.Status='Confirmed'
				--------paramter-----------
				and tc.Cdate < @Date_s
				---------------------------
             ) incoming_before
  -- issue時間區間內
  outer apply(select ISNULL(sum(isnull(tid.NewCone, 0)), 0) + ISNULL(sum(isnull(tid.UsedCone, 0)), 0) as ttl_cone 
				from ThreadIssue ti
					, ThreadIssue_Detail tid 
				where 1=1
				and ti.id=tid.ID
				and tid.Refno = src.RefNo
				and tid.ThreadColorID = src.ThreadColorID
				and ti.Status='Confirmed'
				--------paramter-----------
				and ti.Cdate between @Date_s and @Date_e 
				---------------------------
             ) issue_between
  -- issue時間區間以前
  outer apply(select ISNULL(sum(isnull(tid.NewCone, 0)), 0) + ISNULL(sum(isnull(tid.UsedCone, 0)), 0) as ttl_cone 
				from ThreadIssue ti
					, ThreadIssue_Detail tid 
				where 1=1
				and ti.id=tid.ID
				and tid.Refno = src.RefNo
				and tid.ThreadColorID = src.ThreadColorID
				and ti.Status='Confirmed'
				--------paramter-----------
				and ti.Cdate < @Date_s
				---------------------------
             ) issue_before
  -- adjust時間區間內
  outer apply(select ISNULL(sum(isnull(tad.NewCone, 0)), 0)+ ISNULL(sum(isnull(tad.UsedCone, 0)), 0) - ISNULL(sum(isnull(tad.NewConeBook, 0)), 0) - ISNULL(sum(isnull(tad.UsedConeBook, 0)), 0) as ttl_cone 
				from ThreadAdjust ta
					, ThreadAdjust_Detail tad 
				where 1=1
				and ta.id=tad.ID
				and tad.Refno = src.RefNo
				and tad.ThreadColorID = src.ThreadColorID
				and ta.Status='Confirmed'
				--------paramter-----------
				and ta.Cdate between @Date_s and @Date_e 
				---------------------------
             ) adjust_between
  -- adjust時間區間以前
  outer apply(select ISNULL(sum(isnull(tad.NewCone, 0)), 0)+ ISNULL(sum(isnull(tad.UsedCone, 0)), 0) - ISNULL(sum(isnull(tad.NewConeBook, 0)), 0) - ISNULL(sum(isnull(tad.UsedConeBook, 0)), 0) as ttl_cone 
				from ThreadAdjust ta
					, ThreadAdjust_Detail tad 
				where 1=1
				and ta.id=tad.ID
				and tad.Refno = src.RefNo
				and tad.ThreadColorID = src.ThreadColorID
				and ta.Status='Confirmed'
				--------paramter-----------
				and ta.Cdate < @Date_s
				---------------------------
             ) adjust_before
 where 1=1
 order by src.Refno,src.ThreadColorID
OPTION (OPTIMIZE FOR UNKNOWN);
drop table #tmp
");
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), this.Parameters, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.printData.Rows.Count);

            // Note：匯出Excel的兩種方法   1.用 Microsoft.Office.Interop.Excel 、 2.用Sci.Utility.Excel

            // 第一種
            // Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_R22.xltx"); // 預先開啟excel app
            // MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Thread_R22.xltx", 2, showExcel: false, showSaveMsg: false, excelApp: objApp);

            // this.ShowWaitMessage("Excel Processing...");
            // Excel.Worksheet worksheet = objApp.Sheets[1];

            // #region Save & Show Excel
            // string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Thread_R22");
            // objApp.ActiveWorkbook.SaveAs(strExcelName);
            // objApp.Quit();
            // Marshal.ReleaseComObject(objApp);
            // Marshal.ReleaseComObject(worksheet);

            // strExcelName.OpenFile();
            // #endregion
            // this.HideWaitMessage();
            // return true;

            //-------------------

            // 第二種
            // 等待訊息
            this.ShowWaitMessage("Excel Processing...");

            SaveXltReportCls x1 = new SaveXltReportCls("Thread_R22.xltx");
            SaveXltReportCls.XltRptTable dt1 = new SaveXltReportCls.XltRptTable(this.printData);

            // Header範本有了，因此不用

            // ## ，直接替換範本檔儲存格裡面的物件，從stering到整個DataTeble都可以替換
            dt1.ShowHeader = false;
            x1.DicDatas.Add("##Date_start", this.Date_s.Value.ToString("yyyy-MM-dd"));
            x1.DicDatas.Add("##Date_end", this.Date_e.Value.ToString("yyyy-MM-dd"));
            x1.DicDatas.Add("##DATA", dt1);

            x1.Save();

            this.HideWaitMessage();

            return true;
        }
    }
}

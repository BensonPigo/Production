using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private string factory;
        private string style;
        private string season;
        private string team;
        //private string inline1;
        //private string inline2;
        //private string sewingline1;
        //private string sewingline2;
        private bool bolSummary;
        private bool bolBalancing;
        private string phase;
        private bool latestVersion;

        private DataTable printData_Summary;
        private DataTable printData_Detail_Station;
        private DataTable printData_Detail_Operation;
        private StringBuilder sqlCmd = new StringBuilder();

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboSewingTeam1.SetDataSource();
            MyUtility.Tool.SetupCombox(this.comboPhase, 1, 1, ",Initial,Prelim,Final");
        }

        // Factory
        private void TxtFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct FTYGroup from Factory WITH (NOLOCK) where Junk = 0 AND FTYGroup!=''";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "8", this.txtFactory.Text, "Factory");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtFactory.Text = item.GetSelectedString();
        }

        // Style
        private void TxtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct ID,BrandID,Description from Style WITH (NOLOCK) where Junk = 0 order by ID";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "16,10,50", this.txtStyle.Text, "Style#,Brand,Description")
            {
                Width = 800,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtStyle.Text = item.GetSelectedString();
        }

        // Season
        private void TxtSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct ID from Season WITH (NOLOCK) where Junk = 0 ORDER BY ID DESC";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "10", this.txtSeason.Text, "Season")
            {
                Width = 300,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeason.Text = item.GetSelectedString();
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (!this.dateInlineDate.HasValue1 && !this.dateInlineDate.HasValue2 && !this.dateSewingDate.HasValue1 && !this.dateSewingDate.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <Inline Date> or <Sewing Date> first!!");
                return false;
            }

            this.factory = this.txtFactory.Text;
            this.style = this.txtStyle.Text;
            this.season = this.txtSeason.Text;
            this.team = this.comboSewingTeam1.Text;
            this.bolSummary = this.radioSummary.Checked;
            this.bolBalancing = this.chkBalancing.Checked;
            this.phase = this.comboPhase.Text;
            this.latestVersion = this.chkLastVersion.Checked;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result;
            StringBuilder cmdP03_Summary = new StringBuilder();
            StringBuilder cmdP05_Summary = new StringBuilder();
            StringBuilder cmdP06_Summary = new StringBuilder();
            StringBuilder cmdP03_Station = new StringBuilder();
            StringBuilder cmdP05_Station = new StringBuilder();
            StringBuilder cmdP06_Station = new StringBuilder();
            StringBuilder cmdP03_Operation = new StringBuilder();
            StringBuilder cmdP05_Operation = new StringBuilder();
            StringBuilder cmdP06_Operation = new StringBuilder();

            if (this.bolSummary)
            {
                cmdP03_Summary = this.GetSummaryP03();
                cmdP05_Summary = this.GetSummaryP05();
                cmdP06_Summary = this.GetSummaryP06();

                result = DBProxy.Current.Select(null, cmdP03_Summary.ToString(), out DataTable p03_Summary);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                result = DBProxy.Current.Select(null, cmdP05_Summary.ToString(), out DataTable p05_Summary);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                result = DBProxy.Current.Select(null, cmdP06_Summary.ToString(), out DataTable p06_Summary);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                this.printData_Summary = p03_Summary.Clone();
                this.printData_Summary.Merge(p03_Summary);
                this.printData_Summary.AcceptChanges();

                // 欄位格式不一樣會導致merge失敗，用下面這個檢查
                //foreach (DataColumn col in this.printData_Summary.Columns)
                //{
                //    string colName = col.ColumnName;
                //    var tCol = p05_Summary.Columns[colName];

                //    if (col.DataType != tCol.DataType)
                //    {

                //    }
                //    else
                //    {

                //    }
                //}

                this.printData_Summary.Merge(p05_Summary, true);
                this.printData_Summary.AcceptChanges();
                this.printData_Summary.Merge(p06_Summary, true);
                this.printData_Summary.AcceptChanges();
            }
            else
            {
                this.printData_Detail_Station = new DataTable();
                this.printData_Detail_Operation = new DataTable();
                cmdP03_Station = this.GetDetailP03_Station();
                cmdP05_Station = this.GetDetailP05_Station();
                cmdP06_Station = this.GetDetailP06_Station();
                cmdP03_Operation = this.GetDetailP03_Operation();
                cmdP05_Operation = this.GetDetailP05_Operation();
                cmdP06_Operation = this.GetDetailP06_Operation();

                // Station
                result = DBProxy.Current.Select(null, cmdP03_Station.ToString(), out DataTable p03_Station);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                result = DBProxy.Current.Select(null, cmdP05_Station.ToString(), out DataTable p05_Station);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                result = DBProxy.Current.Select(null, cmdP06_Station.ToString(), out DataTable p06_Station);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                // Operation
                result = DBProxy.Current.Select(null, cmdP03_Operation.ToString(), out DataTable p03_Operation);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                result = DBProxy.Current.Select(null, cmdP05_Operation.ToString(), out DataTable p05_Operation);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                result = DBProxy.Current.Select(null, cmdP06_Operation.ToString(), out DataTable p06_Operation);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                this.printData_Detail_Station = p03_Station.Clone();
                this.printData_Detail_Station.Merge(p03_Station);
                this.printData_Detail_Station.AcceptChanges();
                this.printData_Detail_Station.Merge(p05_Station, true);
                this.printData_Detail_Station.AcceptChanges();
                this.printData_Detail_Station.Merge(p06_Station, true);
                this.printData_Detail_Station.AcceptChanges();

                this.printData_Detail_Operation = p03_Operation.Clone();
                this.printData_Detail_Operation.Merge(p03_Operation);
                this.printData_Detail_Operation.AcceptChanges();
                this.printData_Detail_Operation.Merge(p05_Operation, true);
                this.printData_Detail_Operation.AcceptChanges();
                this.printData_Detail_Operation.Merge(p06_Operation, true);
                this.printData_Detail_Operation.AcceptChanges();
            }

            return result;
        }

        /// <summary>
        /// OnToExcel 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            if (this.bolSummary)
            {
                this.SetCount(this.printData_Summary.Rows.Count);
                if (this.printData_Summary.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }
            }
            else
            {
                this.SetCount(this.printData_Detail_Station.Rows.Count);
                if (this.printData_Detail_Station.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string fileName = this.bolSummary ? "IE_R01_Summary" : "IE_R01_Detail";
            string strXltName = Env.Cfg.XltPathDir + $"\\{fileName}.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            if (this.bolSummary)
            {
                MyUtility.Excel.CopyToXls(this.printData_Summary, string.Empty, fileName + ".xltx", 1, showExcel: false, fieldList: null, excelApp: excel);
            }
            else
            {
                Microsoft.Office.Interop.Excel.Worksheet wsSheet = excel.ActiveWorkbook.Worksheets[1];
                MyUtility.Excel.CopyToXls(this.printData_Detail_Station, string.Empty, fileName + ".xltx", 1, showExcel: false, fieldList: null, wSheet: wsSheet, excelApp: excel);
                wsSheet = excel.ActiveWorkbook.Worksheets[2];
                MyUtility.Excel.CopyToXls(this.printData_Detail_Operation, string.Empty, fileName + ".xltx", 1, showExcel: false, fieldList: null, wSheet: wsSheet, excelApp: excel);
            }

            //excel.Cells.EntireColumn.AutoFit();
            excel.ActiveWorkbook.Worksheets[1].Columns["A"].ColumnWidth = 10;
            excel.ActiveWorkbook.Worksheets[1].Columns["P"].ColumnWidth = 30;
            excel.ActiveWorkbook.Worksheets[2].Columns["K"].ColumnWidth = 20;
            excel.ActiveWorkbook.Worksheets[2].Columns["L"].ColumnWidth = 20;
            excel.ActiveWorkbook.Worksheets[2].Columns["M"].ColumnWidth = 30;
            excel.ActiveWorkbook.Worksheets[2].Columns["N"].ColumnWidth = 30;

            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[1];
            worksheet.get_Range("M:M").WrapText = false;
            worksheet.get_Range("N:N").WrapText = false;
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(fileName);
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void TxtFactory_Validating(object sender, CancelEventArgs e)
        {
            DataTable factoryData;
            string fac = string.Empty;
            string sqlCmd = "select distinct FTYGroup from Factory WITH (NOLOCK) where Junk = 0 AND FTYGroup!=''";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out factoryData);
            foreach (DataRow dr in factoryData.Rows)
            {
                fac = dr["FTYGroup"].ToString();
                if (this.txtFactory.Text == fac)
                {
                    return;
                }
            }

            if (this.txtFactory.Text == string.Empty)
            {
                this.txtFactory.Text = string.Empty;
                return;
            }

            if (this.txtFactory.Text != fac)
            {
                this.txtFactory.Text = string.Empty;
                MyUtility.Msg.WarningBox("This Factory is wrong!");
                return;
            }
        }

        private void TxtStyle_Validating(object sender, CancelEventArgs e)
        {
            DataTable styleData;
            string sty = string.Empty;
            string sqlCmd = "select distinct ID,BrandID,Description from Style WITH (NOLOCK) where Junk = 0 order by ID";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out styleData);
            foreach (DataRow dr in styleData.Rows)
            {
                sty = dr["ID"].ToString();
                if (this.txtStyle.Text == sty)
                {
                    return;
                }
            }

            if (this.txtStyle.Text == string.Empty)
            {
                this.txtStyle.Text = string.Empty;
                return;
            }

            if (this.txtStyle.Text != sty)
            {
                this.txtStyle.Text = string.Empty;
                MyUtility.Msg.WarningBox("This Style# is wrong!");
                return;
            }
        }

        private void TxtSeason_Validating(object sender, CancelEventArgs e)
        {
            DataTable seasonData;
            string season = string.Empty;
            string sqlCmd = "select distinct ID from Season WITH (NOLOCK) where Junk = 0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out seasonData);
            foreach (DataRow dr in seasonData.Rows)
            {
                season = dr["ID"].ToString();
                if (this.txtSeason.Text == season)
                {
                    return;
                }
            }

            if (this.txtSeason.Text == string.Empty)
            {
                this.txtSeason.Text = string.Empty;
                return;
            }

            if (this.txtSeason.Text != season)
            {
                this.txtSeason.Text = string.Empty;
                MyUtility.Msg.WarningBox("This Season is wrong!");
                return;
            }
        }

        private StringBuilder GetDetailP03_Station()
        {
            StringBuilder cmd = new StringBuilder();

            #region Inline & Sewing Date is not null

            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                cmd.Append($@"
select *
INTO #LineMapping
from LineMapping t WITH (NOLOCK) 
where exists(
	select 1
	from SewingSchedule ss
	join Orders o on ss.OrderID = o.ID
	where 1=1
	{dateQuery}
	and o.StyleID = t.StyleID and o.SeasonID = t.SeasonID and o.BrandID = t.BrandID
 )

");
            }
            #endregion

            if (!MyUtility.Check.Empty(this.factory))
            {
                cmd.Append(string.Format(" and t.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                cmd.Append(string.Format(" and t.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                cmd.Append(string.Format(" and t.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.team))
            {
                cmd.Append(string.Format(" and t.Team = '{0}'", this.team));
            }

            if (!MyUtility.Check.Empty(this.phase))
            {
                cmd.Append(string.Format(" and t.Phase = '{0}'", this.phase));
            }

            cmd.Append(@"

select lmd.*
INTO #LineMapping_Detail
from #LineMapping lm
inner join LineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
where lmd.IsHide = 0 and lmd.No <> ''

select distinct
	lm.FactoryID,
	lm.StyleID,
	lm.ComboType,
	lm.SeasonID,
    lm.Phase,
	lm.BrandID,
    lm.SewingLineID,
    lm.Team,
	lm.Version,
	lmd.No,
    TotalGSD = DetailSum.TotalGSD,
	ActCycleTime = DetailSum.TotalCycle, -- 公式：加總同No.的LineMapping_Detail.Cycle
    GSDvsActTimeDiff = IIF(DetailSum.TotalGSD = 0, 0, (DetailSum.TotalGSD - DetailSum.TotalCycle) / DetailSum.TotalGSD ) , ---- 公式: ( [Ttl GSD time] - [Act. Cycle Time]) / [Ttl GSD time]
	ActCycleTimeAvg = IIF(lm.CurrentOperators = 0, 0, ROUND(1.0 * lm.TotalCycle / lm.CurrentOperators ,2)),  ---- 公式: [Total Cycle time] / [Current Oprts] 兩個都是表頭欄位
	ActTimeDiffAvg = IIF( IIF(lm.CurrentOperators = 0, 0, 1.0 * lm.TotalCycle / lm.CurrentOperators) = 0 ,0,  
		(IIF(lm.CurrentOperators = 0, 0, ROUND(1.0 * lm.TotalCycle / lm.CurrentOperators ,2)) - DetailSum.TotalCycle) / IIF(lm.CurrentOperators = 0, 0, ROUND(1.0 * lm.TotalCycle / lm.CurrentOperators ,2))
		), 　---- 公式: ( [Act. Cycle Time (average)] - [ Act. Cycle time] ) / [Act. Cycle Time (average)]
	NotHitTargetReason = lbr.name,
    IsFrom = 'IE P03'
    ,lmd.EmployeeID
    ,EmployeeName = (select Employee.LastName + ',' + Employee.FirstName from Employee where id = lmd.EmployeeID)
from #LineMapping lm WITH (NOLOCK) 
inner join #LineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
left join IEReasonLBRNotHit_Detail lbr WITH (NOLOCK) on lmd.IEReasonLBRNotHit_DetailUkey = lbr.Ukey and lbr.junk = 0
outer apply (
	select avgTotalCycle = avg(TotalCycle)
	from 
	(
		select ID, NO, TotalCycle
		from #LineMapping_Detail
		where ID = lm.ID
		and No <> '' 
		group by ID, NO, TotalCycle
	) lmdavg
)lmdavg
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID 
				--and lm.status = 'Confirmed' 
				--and c.EffectiveDate < lm.Editdate 
				and c. Type ='LBR'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)LinebalancingTarget 
OUTER APPLY(
	select TotalCycle = SUM(lmdd.Cycle) * 1.0 ,TotalGSD = SUM(lmdd.GSD) * 1.0
	from #LineMapping_Detail lmdd
	where lm.ID = lmdd.ID and lmd.No=lmdd.No
)DetailSum
where 1=1 
");

            if (this.bolBalancing)
            {
                cmd.Append(@"
and (((lmdavg.avgTotalCycle - lmd.TotalCycle) / lmdavg.avgTotalCycle) * 100 >  (100 - LinebalancingTarget.Target) 
	or  ((lmdavg.avgTotalCycle - lmd.TotalCycle) / lmdavg.avgTotalCycle) * 100 < (100 - LinebalancingTarget.Target) * -1 )
");
            }

            if (this.latestVersion)
            {
                cmd.Append(@"
 and lm.Version = (
	select MAX(l.Version)
	from LineMapping l
	where l.StyleUKey = lm.StyleUKey
	and l.FactoryID = lm.FactoryID
	and l.Phase = lm.Phase
    and l.SewingLineID = lm.SewingLineID
	group by l.StyleUKey, l.FactoryID,l.Phase,l.SewingLineID
 )
");
            }

            cmd.Append(Environment.NewLine + "DROP TABLE #LineMapping_Detail,#LineMapping ");
            return cmd;
        }

        private StringBuilder GetDetailP05_Station()
        {
            StringBuilder cmd = new StringBuilder();

            #region Inline & Sewing Date is not null

            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                cmd.Append($@"
select *
INTO #AutomatedLineMapping
from AutomatedLineMapping t WITH (NOLOCK) 
 where exists(
	select 1
	from SewingSchedule ss
	join Orders o on ss.OrderID = o.ID
	where 1=1
	{dateQuery}
	and o.StyleID = t.StyleID and o.SeasonID = t.SeasonID and o.BrandID = t.BrandID
 )
");
            }
            #endregion

            if (!MyUtility.Check.Empty(this.factory))
            {
                cmd.Append(string.Format(" and t.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                cmd.Append(string.Format(" and t.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                cmd.Append(string.Format(" and t.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.phase))
            {
                cmd.Append(string.Format(" and t.Phase = '{0}'", this.phase));
            }

            cmd.Append(@"
select lmd.*
INTO #AutomatedLineMapping_Detail
from #AutomatedLineMapping lm
inner join AutomatedLineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
where lmd.No <> ''

select DISTINCT lm.ID, lmd.No, [Reason] = i.Description
INTO #NotHitTargetReason
from AutomatedLineMapping_NotHitTargetReason r
inner join IEReason i on i.ID = r.IEReasonID and i.Type = 'AS'
inner join #AutomatedLineMapping_Detail lmd on r.ID = lmd.ID and r.No = lmd.No
inner join #AutomatedLineMapping lm on lm.ID = lmd.ID

select distinct
	lm.FactoryID,
	lm.StyleID,
	lm.ComboType,
	lm.SeasonID,
    lm.Phase,
	lm.BrandID,
    SewingLineID = '',
    Team = '',
	lm.Version,
	lmd.No,
    TotalGSD = DetailSum.TotalGSD,
	ActCycleTime =  Cast( NULL as decimal), -- P05沒有
    GSDvsActTimeDiff = Cast( NULL as decimal), ----  P05沒有
	ActCycleTimeAvg = Cast( NULL as decimal),  ---- P05沒有
	ActTimeDiffAvg = Cast( NULL as decimal),　----  P05沒有
	NotHitTargetReason = NotHitTargetReason.Reason,
    IsFrom = 'IE P05'
from #AutomatedLineMapping lm WITH (NOLOCK) 
inner join #AutomatedLineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
OUTER APPLY(
	select TotalGSD = SUM(lmdd.GSD * lmdd.SewerDiffPercentage)
	from #AutomatedLineMapping_Detail lmdd
	where lm.ID = lmdd.ID and lmd.No=lmdd.No
)DetailSum
outer apply(
    select TOP 1 r.Reason
    from  #NotHitTargetReason r
    where r.ID = lm.ID and r.No = lmd.No
)NotHitTargetReason
where 1 = 1
");

            if (this.latestVersion)
            {
                cmd.Append(@"
 and lm.Version = (
	select MAX(l.Version)
	from AutomatedLineMapping l
	where l.StyleUKey = lm.StyleUKey
	and l.FactoryID = lm.FactoryID
	and l.Phase = lm.Phase
	group by l.StyleUKey, l.FactoryID,l.Phase
 )
");
            }


            cmd.Append(Environment.NewLine + "DROP TABLE #AutomatedLineMapping,#AutomatedLineMapping_Detail,#NotHitTargetReason  ");
            return cmd;
        }

        private StringBuilder GetDetailP06_Station()
        {
            StringBuilder cmd = new StringBuilder();

            #region Inline & Sewing Date is not null

            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                cmd.Append($@"
select *
INTO #LineMappingBalancing
from LineMappingBalancing t WITH (NOLOCK) 
where exists(
	select 1
	from SewingSchedule ss
	join Orders o on ss.OrderID = o.ID
	where 1=1
	{dateQuery}
	and o.StyleID = t.StyleID and o.SeasonID = t.SeasonID and o.BrandID = t.BrandID
 )
");
            }
            #endregion

            if (!MyUtility.Check.Empty(this.factory))
            {
                cmd.Append(string.Format(" and t.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                cmd.Append(string.Format(" and t.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                cmd.Append(string.Format(" and t.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.team))
            {
                cmd.Append(string.Format(" and t.Team = '{0}'", this.team));
            }

            if (!MyUtility.Check.Empty(this.phase))
            {
                cmd.Append(string.Format(" and t.Phase = '{0}'", this.phase));
            }

            cmd.Append(@"
select lmd.*
INTO #LineMappingBalancing_Detail
from #LineMappingBalancing lm
inner join LineMappingBalancing_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
where lmd.No <> ''

select DISTINCT lm.ID, lmd.No, [Reason] = i.Description
INTO #NotHitTargetReason
from LineMappingBalancing_NotHitTargetReason r
inner join IEReason i on i.ID = r.IEReasonID and i.Type = 'AS'
inner join #LineMappingBalancing_Detail lmd on r.ID = lmd.ID and r.No = lmd.No
inner join #LineMappingBalancing lm on lm.ID = lmd.ID

select distinct
	lm.FactoryID,
	lm.StyleID,
	lm.ComboType,
	lm.SeasonID,
    lm.Phase,
	lm.BrandID,
    lm.SewingLineID,
    lm.Team,
	lm.Version,
	lmd.No,
    TotalGSD = DetailSum.TotalGSD,
	ActCycleTime = DetailSum.TotalCycle, -- 公式：加總同No.的LineMapping_Detail.Cycle
    GSDvsActTimeDiff = IIF(DetailSum.TotalCycle = 0, 0, (DetailSum.TotalGSD - DetailSum.TotalCycle) / DetailSum.TotalGSD ) , ---- 公式: ( [Ttl GSD time] - [Act. Cycle Time]) / [Ttl GSD time]
	ActCycleTimeAvg = IIF(lm.SewerManpower = 0, 0, ROUND(1.0 * lm.TotalCycleTime / lm. SewerManpower ,2)),  ---- 公式: [Total Cycle time] / [Current Oprts] 兩個都是表頭欄位
	ActTimeDiffAvg = IIF(lm. SewerManpower = 0 or lm.TotalCycleTime / lm. SewerManpower = 0 , 0 ,
	
						(ROUND((1.0 * lm.TotalCycleTime / lm. SewerManpower) ,2) -  DetailSum.TotalCycle) / ROUND( lm.TotalCycleTime / lm. SewerManpower ,2)
					) ,　---- 公式: ( [Act. Cycle Time (average)] - [ Act. Cycle time] ) / [Act. Cycle Time (average)]
	NotHitTargetReason = NotHitTargetReason.Reason,
    IsFrom = 'IE P06' 
    ,lmd.EmployeeID
    ,EmployeeName = (select Employee.LastName + ',' + Employee.FirstName from Employee where id = lmd.EmployeeID)
from #LineMappingBalancing lm WITH (NOLOCK) 
inner join #LineMappingBalancing_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
outer apply (
	select avgTotalCycle = avg(Cycle)
	from 
	(
		select ID, NO, Cycle
		from #LineMappingBalancing_Detail
		where ID = lm.ID
		and No <> '' 
		group by ID, NO, Cycle
	) lmdavg
)lmdavg
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID 
				--and lm.status = 'Confirmed' 
				--and c.EffectiveDate < lm.Editdate 
				and c. Type ='LBR'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)LinebalancingTarget 
OUTER APPLY(
	select TotalCycle = SUM(lmdd.Cycle * lmdd.SewerDiffPercentage) ,TotalGSD = SUM(lmdd.GSD * lmdd.SewerDiffPercentage)
	from #LineMappingBalancing_Detail lmdd
	where lm.ID = lmdd.ID and lmd.No=lmdd.No
)DetailSum
outer apply(
    select TOP 1 r.Reason
    from  #NotHitTargetReason r
    where r.ID = lm.ID and r.No = lmd.No
)NotHitTargetReason
where 1 = 1
");

            if (this.bolBalancing)
            {
                cmd.Append(@"
and (((lmdavg.avgTotalCycle - DetailSum.TotalCycle) / lmdavg.avgTotalCycle) * 100 >  (100 - LinebalancingTarget.Target) 
	or  ((lmdavg.avgTotalCycle - DetailSum.TotalCycle) / lmdavg.avgTotalCycle) * 100 < (100 - LinebalancingTarget.Target) * -1 )
");
            }

            if (this.latestVersion)
            {
                cmd.Append(@"
 and lm.Version = (
	select MAX(l.Version)
	from LineMappingBalancing l
	where l.StyleUKey = lm.StyleUKey
	and l.FactoryID = lm.FactoryID
	and l.Phase = lm.Phase
    and l.SewingLineID = lm.SewingLineID
	group by l.StyleUKey, l.FactoryID,l.Phase,l.SewingLineID
 )
");
            }

            cmd.Append(Environment.NewLine + "DROP TABLE #LineMappingBalancing ,#LineMappingBalancing_Detail,#NotHitTargetReason ");
            return cmd;
        }

        private StringBuilder GetDetailP03_Operation()
        {
            StringBuilder cmd = new StringBuilder();

            #region Inline & Sewing Date is not null

            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                cmd.Append($@"
select *
INTO #LineMapping
from LineMapping t WITH (NOLOCK) 
where exists(
	select 1
	from SewingSchedule ss
	INNER join Orders o on ss.OrderID = o.ID
	where 1=1
	{dateQuery}
	and o.StyleID = t.StyleID and o.SeasonID = t.SeasonID and o.BrandID = t.BrandID
 )
");
            }
            #endregion

            if (!MyUtility.Check.Empty(this.factory))
            {
                cmd.Append(string.Format(" and t.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                cmd.Append(string.Format(" and t.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                cmd.Append(string.Format(" and t.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.team))
            {
                cmd.Append(string.Format(" and t.Team = '{0}'", this.team));
            }

            if (!MyUtility.Check.Empty(this.phase))
            {
                cmd.Append(string.Format(" and t.Phase = '{0}'", this.phase));
            }

            cmd.Append(@"
select lmd.*
INTO #LineMapping_Detail
from #LineMapping lm
inner join LineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
where lmd.IsHide = 0 and lmd.No <> ''

select distinct
	 lm.ID
	,lm.FactoryID
	,lm.StyleID
	,lm.ComboType
	,lm.SeasonID
    ,lm.Phase
	,lm.BrandID
    ,lm.SewingLineID
    ,lm.Team
	,lm.Version
	,lmd.No
    ,[OperationID] = lmd.OperationID 
    ,[OperationDesc] = o.DescEN 
    ,IsFrom = 'IE P03'
into #tmp
from #LineMapping lm WITH (NOLOCK) 
inner join #LineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
inner join Operation o on lmd.OperationID = o.ID
outer apply (
	select avgTotalCycle = avg(TotalCycle)
	from 
	(
		select ID, NO, TotalCycle
		from #LineMapping_Detail
		where ID = lm.ID
		and No <> '' 
		group by ID, NO, TotalCycle
	) lmdavg
)lmdavg
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID 
				--and lm.status = 'Confirmed' 
				--and c.EffectiveDate < lm.Editdate 
				and c. Type ='LBR'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)LinebalancingTarget 
where 1=1 
");

            if (this.bolBalancing)
            {
                cmd.Append(@"
and (((lmdavg.avgTotalCycle - lmd.TotalCycle) / lmdavg.avgTotalCycle) * 100 >  (100 - LinebalancingTarget.Target) 
	or  ((lmdavg.avgTotalCycle - lmd.TotalCycle) / lmdavg.avgTotalCycle) * 100 < (100 - LinebalancingTarget.Target) * -1 )
");
            }

            if (this.latestVersion)
            {
                cmd.Append(@"
 and lm.Version = (
	select MAX(l.Version)
	from LineMapping l
	where l.StyleUKey = lm.StyleUKey
	and l.FactoryID = lm.FactoryID
	and l.Phase = lm.Phase
    and l.SewingLineID = lm.SewingLineID
	group by l.StyleUKey, l.FactoryID,l.Phase,l.SewingLineID
 )
");
            }

            cmd.Append(@"
select     
	t.FactoryID,
	t.StyleID,
	t.ComboType,
	t.SeasonID,
    t.Phase,
	t.BrandID,
    t.SewingLineID,
    t.Team,
	t.Version,
	t.No,
	MachineTypeID = MachineType.Val,
	MasterPlusGroup = MasterPlusGroup.Val,
	Operation = t.OperationDesc,
	Annotation = Annotation.Val,
	GSDTime = DetailSum.TotalGSD,
	CycleTime = DetailSum.TotalCycle ,
	Eff= iif(DetailSum.TotalCycle = 0,0,DetailSum.TotalGSD/ DetailSum.TotalCycle),
    t.IsFrom
from #tmp t
OUTER APPLY(
	select Val = STUFF( (
		select DISTINCT ',' + MachineTypeID
		from #LineMapping_Detail lmdd
		where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
		FOR XML PATH('')
		),1,1,'')
)MachineType
OUTER APPLY(
	select Val = STUFF( (
		select DISTINCT ',' + MasterPlusGroup
		from #LineMapping_Detail lmdd
		where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
		FOR XML PATH('')
		),1,1,'')
)MasterPlusGroup
OUTER APPLY(
	select Val = STUFF( (
		select DISTINCT ',' + Annotation
		from #LineMapping_Detail lmdd
		where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
		FOR XML PATH('')
		),1,1,'')
)Annotation
OUTER APPLY(
	select TotalCycle = SUM(lmdd.Cycle) * 1.0 ,TotalGSD = SUM(lmdd.GSD) * 1.0
	from #LineMapping_Detail lmdd
	where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
)DetailSum


");

            cmd.Append(Environment.NewLine + "DROP TABLE #tmp,#LineMapping,#LineMapping_Detail ");
            return cmd;
        }

        private StringBuilder GetDetailP05_Operation()
        {
            StringBuilder cmd = new StringBuilder();

            #region Inline & Sewing Date is not null

            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                cmd.Append($@"
select *
INTO #AutomatedLineMapping
from AutomatedLineMapping t WITH (NOLOCK) 
 where exists(
	select 1
	from SewingSchedule ss
	join Orders o on ss.OrderID = o.ID
	where 1=1
	{dateQuery}
	and o.StyleID = t.StyleID and o.SeasonID = t.SeasonID and o.BrandID = t.BrandID
 )
");
            }
            #endregion

            if (!MyUtility.Check.Empty(this.factory))
            {
                cmd.Append(string.Format(" and t.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                cmd.Append(string.Format(" and t.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                cmd.Append(string.Format(" and t.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.phase))
            {
                cmd.Append(string.Format(" and t.Phase = '{0}'", this.phase));
            }

            cmd.Append(@"
select lmd.*
INTO #AutomatedLineMapping_Detail
from #AutomatedLineMapping lm
inner join AutomatedLineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
where lmd.No <> ''

select distinct
	 lm.ID
	,lm.FactoryID
	,lm.StyleID
	,lm.StyleUKey
	,lm.ComboType
	,lm.SeasonID
    ,lm.Phase
	,lm.BrandID
    ,SewingLineID = ''
    ,Team = ''
	,lm.Version
	,lmd.No
    ,[OperationID] = lmd.OperationID 
    ,[OperationDesc] = o.DescEN 
    ,IsFrom = 'IE P05'
into #tmp
from #AutomatedLineMapping lm WITH (NOLOCK) 
inner join #AutomatedLineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
inner join Operation o on lmd.OperationID = o.ID
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID 
				--and lm.status = 'Confirmed' 
				--and c.EffectiveDate < lm.Editdate 
				and c. Type ='LBR'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)LinebalancingTarget 
where 1=1 

select     
	t.FactoryID,
	t.StyleID,
	t.ComboType,
	t.SeasonID,
    t.Phase,
	t.BrandID,
    t.SewingLineID,
    t.Team,
	t.Version,
	t.No,
	MachineTypeID = MachineType.Val,
	MasterPlusGroup = MasterPlusGroup.Val,
	Operation = t.OperationDesc,
	Annotation = Annotation.Val,
	GSDTime = DetailSum.TotalGSD,
	CycleTime = Cast( NULL as decimal),
	Eff= Cast( NULL as decimal),
    t.IsFrom
from #tmp t
OUTER APPLY(
	select Val = STUFF( (
		select DISTINCT ',' + MachineTypeID
		from #AutomatedLineMapping_Detail lmdd
		where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
		FOR XML PATH('')
		),1,1,'')
)MachineType
OUTER APPLY(
	select Val = STUFF( (
		select DISTINCT ',' + MasterPlusGroup
		from #AutomatedLineMapping_Detail lmdd
		where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
		FOR XML PATH('')
		),1,1,'')
)MasterPlusGroup
OUTER APPLY(
	select Val = STUFF( (
		select DISTINCT ',' + Annotation
		from #AutomatedLineMapping_Detail lmdd
		where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
		FOR XML PATH('')
		),1,1,'')
)Annotation
OUTER APPLY(
	select TotalGSD = SUM(lmdd.GSD*lmdd.SewerDiffPercentage) * 1.0
	from #AutomatedLineMapping_Detail lmdd
	where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
)DetailSum
WHERE 1=1
");

            if (this.latestVersion)
            {
                cmd.Append(@"
 and t.Version = (
	select MAX(l.Version)
	from AutomatedLineMapping l
	where l.StyleUKey = t.StyleUKey
	and l.FactoryID = t.FactoryID
	and l.Phase = t.Phase
	group by l.StyleUKey, l.FactoryID,l.Phase
 )
");
            }

            cmd.Append(Environment.NewLine + "DROP TABLE #AutomatedLineMapping,#AutomatedLineMapping_Detail,#NotHitTargetReason  ");
            return cmd;
        }

        private StringBuilder GetDetailP06_Operation()
        {
            StringBuilder cmd = new StringBuilder();

            #region Inline & Sewing Date is not null

            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                cmd.Append($@"
select *
INTO #LineMappingBalancing
from LineMappingBalancing t WITH (NOLOCK) 
where exists(
	select 1
	from SewingSchedule ss
	INNER join Orders o on ss.OrderID = o.ID
	where 1=1
	{dateQuery}
	and o.StyleID = t.StyleID and o.SeasonID = t.SeasonID and o.BrandID = t.BrandID
 )
");
            }
            #endregion

            if (!MyUtility.Check.Empty(this.factory))
            {
                cmd.Append(string.Format(" and t.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                cmd.Append(string.Format(" and t.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                cmd.Append(string.Format(" and t.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.team))
            {
                cmd.Append(string.Format(" and t.Team = '{0}'", this.team));
            }

            if (!MyUtility.Check.Empty(this.phase))
            {
                cmd.Append(string.Format(" and t.Phase = '{0}'", this.phase));
            }

            cmd.Append(@"
select lmd.*
INTO #LineMappingBalancing_Detail
from #LineMappingBalancing lm
inner join LineMappingBalancing_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
where lmd.No <> ''

select distinct
	 lm.ID
	,lm.FactoryID
	,lm.StyleID
	,lm.ComboType
	,lm.SeasonID
    ,lm.Phase
	,lm.BrandID
    ,lm.SewingLineID
    ,lm.Team
	,lm.Version
	,lmd.No
	,[OperationID] = lmd.OperationID
	,[OperationDesc] = o.DescEN
    ,IsFrom = 'IE P06'
INTO #tmp
from #LineMappingBalancing lm WITH (NOLOCK) 
inner join #LineMappingBalancing_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
inner join Operation o on lmd.OperationID = o.ID
outer apply (
	select avgTotalCycle = avg(Cycle)
	from 
	(
		select ID, NO, Cycle
		from #LineMappingBalancing_Detail
		where ID = lm.ID
		and No <> '' 
		group by ID, NO, Cycle
	) lmdavg
)lmdavg
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID 
				--and lm.status = 'Confirmed' 
				--and c.EffectiveDate < lm.Editdate 
				and c. Type ='LBR'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)LinebalancingTarget 
OUTER APPLY(
	select TotalCycle = SUM(lmdd.Cycle) ,TotalGSD = SUM(lmdd.GSD)
	from #LineMappingBalancing_Detail lmdd
	where lm.ID = lmdd.ID and lmd.No=lmdd.No
)DetailSum
where 1=1 
");

            if (this.bolBalancing)
            {
                cmd.Append(@"
and (((lmdavg.avgTotalCycle - DetailSum.TotalCycle) / lmdavg.avgTotalCycle) * 100 >  (100 - LinebalancingTarget.Target) 
	or  ((lmdavg.avgTotalCycle - DetailSum.TotalCycle) / lmdavg.avgTotalCycle) * 100 < (100 - LinebalancingTarget.Target) * -1 )
");
            }

            if (this.latestVersion)
            {
                cmd.Append(@"
 and lm.Version = (
	select MAX(l.Version)
	from LineMappingBalancing l
	where l.StyleUKey = lm.StyleUKey
	and l.FactoryID = lm.FactoryID
	and l.Phase = lm.Phase
    and l.SewingLineID = lm.SewingLineID
	group by l.StyleUKey, l.FactoryID,l.Phase,l.SewingLineID
 )
");
            }

            cmd.Append(@"
select     
	t.FactoryID,
	t.StyleID,
	t.ComboType,
	t.SeasonID,
    t.Phase,
	t.BrandID,
    t.SewingLineID,
    t.Team,
	t.Version,
	t.No,
	MachineTypeID = MachineType.Val,
	MasterPlusGroup = MasterPlusGroup.Val,
	Operation = t.OperationDesc,
	Annotation = Annotation.Val,
	GSDTime = DetailSum.TotalGSD,
	CycleTime = DetailSum.TotalCycle ,
	Eff= iif(DetailSum.TotalCycle = 0,0,DetailSum.TotalGSD/ DetailSum.TotalCycle),
    t.IsFrom
from #tmp t
OUTER APPLY(
	select Val = STUFF( (
		select DISTINCT ',' + MachineTypeID
		from #LineMappingBalancing_Detail lmdd
		where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
		FOR XML PATH('')
		),1,1,'')
)MachineType
OUTER APPLY(
	select Val = STUFF( (
		select DISTINCT ',' + MasterPlusGroup
		from #LineMappingBalancing_Detail lmdd
		where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
		FOR XML PATH('')
		),1,1,'')
)MasterPlusGroup
OUTER APPLY(
	select Val = STUFF( (
		select DISTINCT ',' + Annotation
		from #LineMappingBalancing_Detail lmdd
		where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
		FOR XML PATH('')
		),1,1,'')
)Annotation
OUTER APPLY(
	select TotalCycle = SUM(lmdd.Cycle) * 1.0 ,TotalGSD = SUM(lmdd.GSD * SewerDiffPercentage) * 1.0
	from #LineMappingBalancing_Detail lmdd
	where t.ID = lmdd.ID and t.No=lmdd.No and lmdd.OperationID = t.OperationID
)DetailSum


");

            cmd.Append(Environment.NewLine + "DROP TABLE #tmp,#LineMappingBalancing,#LineMappingBalancing_Detail ");
            return cmd;
        }

        private StringBuilder GetSummaryP03()
        {
            StringBuilder cmd = new StringBuilder();

            #region Inline & Sewing Date is not null

            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                cmd.Append($@"
select *
INTO #LineMapping
from LineMapping t WITH (NOLOCK) 
where exists(
	select 1
	from SewingSchedule ss
	join Orders o on ss.OrderID = o.ID
	where 1=1
	{dateQuery}
	and o.StyleID = t.StyleID and o.SeasonID = t.SeasonID and o.BrandID = t.BrandID
 )

");
            }
            #endregion

            if (!MyUtility.Check.Empty(this.factory))
            {
                cmd.Append(string.Format(" and t.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                cmd.Append(string.Format(" and t.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                cmd.Append(string.Format(" and t.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.team))
            {
                cmd.Append(string.Format(" and t.Team = '{0}'", this.team));
            }

            if (!MyUtility.Check.Empty(this.phase))
            {
                cmd.Append(string.Format(" and t.Phase = '{0}'", this.phase));
            }

            cmd.Append(@"

select lmd.*
INTO #LineMapping_Detail
from #LineMapping lm
inner join LineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
where lmd.IsHide = 0 and lmd.No <> ''

select distinct
	f.CountryID,
	lm.FactoryID,
	lm.StyleID,
	lm.ComboType,
	lm.SeasonID,
    lm.Phase,
	lm.BrandID,
	s.Description,
	lm.Version,
	lm.SewingLineID,
	lm.Team,

	[Current # of Optrs] = Cast( lm.CurrentOperators as int),

	---- 公式：( 3600 * [Current # of Optrs] ) / [Total Cycle Time]
	[Target/Hr. (100%)] = CAST( IIF(lm.TotalCycle=0, 0, ROUND( (3600.0 * lm.CurrentOperators) / lm.TotalCycle, 0) ) as int),

	---- 公式：( 3600 * [No. of Hours] ) / [Daily Demand / Shift]
	[Takt Time] = ROUND(
                    IIF( CAST(  ROUND( CAST( IIF(lm.TotalCycle=0, 0, ROUND( (3600.0 * lm.CurrentOperators) / lm.TotalCycle, 0) ) as int) * lm.Workhour ,0) as int ) = 0
                    ,0
                    ,3600 * lm.Workhour / CAST(  ROUND( CAST( IIF(lm.TotalCycle=0, 0, ROUND( (3600.0 * lm.CurrentOperators) / lm.TotalCycle, 0) ) as int) * lm.Workhour ,0) as int ))
                ,0),
    [Std. SMV] = Cast(ISNULL(tdd.StdSMV,tddh.StdSMV) as decimal(10,2)),
    [GSD Style]= ISNULL(t.StyleID,th.StyleID),
    [GSD Season]= ISNULL(t.SeasonID,th.SeasonID),
    [GSD BrandID]= ISNULL(t.BrandID,th.BrandID),
    [Ori. Total GSD Time] = Cast(lm.OriTotalGSD as decimal(10,2)),
	[Total GSD Time] = lm.TotalGSD * 1.0,
	[Total Cycle Time] = lm.TotalCycle * 1.0,
	
	---- 公式: [Total Cycle Time] / [Current # of Optrs]
	[Avg. Cycle Time] =   IIF(lm.TotalCycle=0 OR lm.CurrentOperators = 0,0, 1.0 * lm.TotalCycle / lm.CurrentOperators),

	[CPU / PC] = s.CPU,
	[No. of Hours] = lm.Workhour,
	---- 公式：[Target / Hr.(100%)] * [No. of Hours]
	[Daily Demand / Shift] = CAST(  ROUND( CAST( IIF(lm.TotalCycle=0, 0, ROUND( (3600.0 * lm.CurrentOperators) / lm.TotalCycle, 0) ) as int) * lm.Workhour ,0) as int ),

    ---- P03 為空
	[Optrs of Presser] = 0,
    ---- P03 為空
	[Optrs of Packer] = 0,

	---- 公式：3600 / [Highest Cycle Time]
	[EOLR] = IIF(lm.TotalCycle=0 ,0 ,3600.0 / lm.HighestCycle),

	[Efficiency %] = IIF(lm.HighestCycle = 0 OR lm.CurrentOperators = 0 ,0 , 1.0 * lm.TotalGSD / lm.HighestCycle / lm.CurrentOperators ) ,

	---- P03 公式：[Total Cycle Time] / [HighestCycle] / [Current # of Optrs]
	[Line Balancing %] = IIF( lm.HighestCycle =0 or lm.CurrentOperators=0 ,0 , 1.0 * lm.TotalCycle / lm.HighestCycle / lm.CurrentOperators),

	[Target Line Balancing% ]= (select top 1 co.Target from ChgOverTarget co where co.Type = 'LBR') / 100 ,
	[Not Hit Target Type] =  iif(lm.Version = 1, 
								(select TypeGroup from IEReasonLBRnotHit_1st where Ukey = lm.IEReasonLBRnotHit_1stUkey),
								(select STUFF ((
										select distinct CONCAT (',', a.TypeGroup) 
											from (
												select lbr.TypeGroup
												from LineMapping_Detail l2 WITH (NOLOCK)
												inner join IEReasonLBRNotHit_Detail lbr WITH (NOLOCK) on l2.IEReasonLBRNotHit_DetailUkey = lbr.Ukey and lbr.junk = 0
												where lm.ID = l2.ID
											) a 
											for xml path('')
									), 1, 1, ''))
	) ,
	[Total No. of Not Hit Target ] = iif(lm.Version <> 1,0,(select cnt = iif(count(*) = 0, '', cast(count(1) as varchar))
                    from (
	                    select distinct l2.NO, l2.IEReasonLBRNotHit_DetailUkey
	                    from #LineMapping_Detail l2 WITH (NOLOCK)
	                    where lm.ID = l2.ID
	                    and ISNULL(l2.IEReasonLBRNotHit_DetailUkey, '') <> ''
                    )a )),
    [Not Hit Target Reason] = iif(lm.Version = 1, 
								    (select Name from IEReasonLBRnotHit_1st where Ukey = lm.IEReasonLBRnotHit_1stUkey),
								    (select STUFF ((
									    select distinct CONCAT (',', a.Name ) 
										    from (
											    select lbr.Name 
											    from #LineMapping_Detail l2 WITH (NOLOCK)
											    inner join IEReasonLBRNotHit_Detail lbr WITH (NOLOCK) on l2.IEReasonLBRNotHit_DetailUkey = lbr.Ukey and lbr.junk = 0
											    where lm.ID = l2.ID
										    ) a 
										    for xml path('')
								    ), 1, 1, ''))
    ),
	---- 公式：[Total Cycle time] / [Takt Time] / [Current # of Optrs]
	[Lean Line Eff %] = IIF(lm.Workhour = 0 OR lm.TotalCycle =0 OR lm.CurrentOperators = 0 ,0,
        1.0 * lm.TotalCycle 
        /  ROUND(
                    IIF( CAST(  ROUND( CAST( IIF(lm.TotalCycle=0, 0, ROUND( (3600.0 * lm.CurrentOperators) / lm.TotalCycle, 0) ) as int) * lm.Workhour ,0) as int ) = 0
                    ,0
                    ,3600 * lm.Workhour / CAST(  ROUND( CAST( IIF(lm.TotalCycle=0, 0, ROUND( (3600.0 * lm.CurrentOperators) / lm.TotalCycle, 0) ) as int) * lm.Workhour ,0) as int ))
                ,0)
        / lm.CurrentOperators ),

	---- 公式：( [EOLR] * [CPU / PC] ) / [Current # of Optrs]
	[PPH] =IIF(lm.HighestCycle =0 OR lm.CurrentOperators =0,0,  ( ( 3600.0 / lm.HighestCycle) * s.CPU ) / lm.CurrentOperators),

	lm.Status,
	[GSD Status] = lm.TimeStudyPhase,
	[GSDVersion] = lm.TimeStudyVersion,
	lm.AddName,
	lm.AddDate,
	lm.EditName,
	lm.EditDate,
    IsFrom = 'IE P03'
from #LineMapping lm WITH (NOLOCK) 
inner join Factory f on f.ID = lm.FactoryID
inner join Style s on s.Ukey = lm.StyleUKey
left join TimeStudy t WITH (NOLOCK) on lm.TimeStudyID = t.ID
left join TimeStudyHistory th WITH (NOLOCK) on lm.TimeStudyID = th.ID
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID and c.EffectiveDate < iif(lm.Editdate is null,lm.adddate,lm.Editdate) and c. Type ='LBR'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)LinebalancingTarget 
outer apply(
	select StdSMV =  SUM(td.StdSMV)
	from TimeStudy_Detail td WITH (NOLOCK) where t.id = td.id
        and td.IsSubprocess = 0
        and td.IsNonSewingLine =0
        and td.PPA <> 'C'
)tdd 
outer apply(
	select StdSMV =  SUM(td.StdSMV)
	from TimeStudyHistory_Detail td WITH (NOLOCK) where th.id = td.id
        and td.IsSubprocess = 0
        and td.IsNonSewingLine =0
        and td.PPA <> 'C'
)tddh
where 1 = 1
");

            if (this.bolBalancing)
            {
                cmd.Append(@"
and IIF(lm.HighestCycle*lm.CurrentOperators = 0,0,CONVERT(DECIMAL,lm.TotalCycle)/lm.HighestCycle/lm.CurrentOperators) * 100 < LinebalancingTarget.Target
");
            }

            if (this.latestVersion)
            {
                cmd.Append(@"
 and lm.Version = (
	select MAX(l.Version)
	from LineMapping l
	where l.StyleUKey = lm.StyleUKey
	and l.FactoryID = lm.FactoryID
	and l.Phase = lm.Phase
    and l.SewingLineID = lm.SewingLineID
	group by l.StyleUKey, l.FactoryID,l.Phase,l.SewingLineID
 )
");
            }

            cmd.Append(Environment.NewLine + "DROP TABLE #LineMapping,#LineMapping_Detail ");

            return cmd;
        }

        private StringBuilder GetSummaryP05()
        {
            StringBuilder cmd = new StringBuilder();

            #region Inline & Sewing Date is not null

            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                cmd.Append($@"
select *
INTO #AutomatedLineMapping
from AutomatedLineMapping t WITH (NOLOCK) 
where exists(
	select 1
	from SewingSchedule ss
	join Orders o on ss.OrderID = o.ID
	where 1=1
	{dateQuery}
	and o.StyleID = t.StyleID and o.SeasonID = t.SeasonID and o.BrandID = t.BrandID
 )

");
            }
            #endregion

            if (!MyUtility.Check.Empty(this.factory))
            {
                cmd.Append(string.Format(" and t.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                cmd.Append(string.Format(" and t.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                cmd.Append(string.Format(" and t.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.phase))
            {
                cmd.Append(string.Format(" and t.Phase = '{0}'", this.phase));
            }

            cmd.Append(@"
select lmd.*
INTO #AutomatedLineMapping_Detail
from #AutomatedLineMapping lm
inner join AutomatedLineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
where lmd.No <> ''

select DISTINCT lm.ID, [Reason] = i.Description
INTO #NotHitTargetReason
from AutomatedLineMapping_NotHitTargetReason r
inner join IEReason i on i.ID = r.IEReasonID and i.Type = 'AS'
inner join #AutomatedLineMapping_Detail lmd on r.ID = lmd.ID and r.No = lmd.No
inner join #AutomatedLineMapping lm on lm.ID = lmd.ID

select distinct
	f.CountryID,
	lm.FactoryID,
	lm.StyleID,
	lm.ComboType,
	lm.SeasonID,
    lm.Phase,
	lm.BrandID,
	s.Description,
	lm.Version,
	SewingLineID = '',
	Team = '',


	[Current # of Optrs] = Cast( lm.SewerManpower as int),

	[Target/Hr. (100%)] = CAST( IIF(lm.TotalGSDTime = 0,0 , ROUND( ( 3600.0 * lm.SewerManpower ) / lm.TotalGSDTime, 0)  ) as int),

	---- 公式：( 3600.* [No. of Hours] ) / [Daily Demand / Shift]
	---- P05沒有TotalCycle，所以用GSD
	[Takt Time] = ROUND(
                    IIF( CAST(  ROUND( CAST( IIF(lm.TotalGSDTime = 0,0 , ROUND( ( 3600.0 * lm.SewerManpower ) / lm.TotalGSDTime, 0)  ) as int) * lm.WorkHour ,0) as int ) = 0
                    ,0
                    ,3600 * lm.Workhour / CAST(  ROUND( CAST( IIF(lm.TotalGSDTime = 0,0 , ROUND( ( 3600.0 * lm.SewerManpower ) / lm.TotalGSDTime, 0)  ) as int) * lm.WorkHour ,0) as int ) )
                ,2),

    [Std. SMV] = Cast(ISNULL(tdd.StdSMV,tddh.StdSMV) as decimal(10,2)),
    [GSD Style]= ISNULL(t.StyleID,th.StyleID),
    [GSD Season]= ISNULL(t.SeasonID,th.SeasonID),
    [GSD BrandID]= ISNULL(t.BrandID,th.BrandID),
    [Ori. Total GSD Time] =  Cast(NULL as decimal(10,2)),
	[Total GSD Time] = lm.TotalGSDTime * 1.0,
	[Total Cycle Time] = Cast( NULL as decimal),
	
	---- 公式: P05沒有TotalCycle，所以為0
	[Avg. Cycle Time] = Cast( NULL as decimal),

	[CPU / PC] = lm.StyleCPU,
	[No. of Hours] = lm.Workhour,
	---- 公式：P05沒有TotalCycle，所以為0
	[Daily Demand / Shift] = CAST(  ROUND( CAST( IIF(lm.TotalGSDTime = 0,0 , ROUND( ( 3600.0 * lm.SewerManpower ) / lm.TotalGSDTime, 0)  ) as int) * lm.WorkHour ,0) as int ),

	[Optrs of Presser] = Cast( lm.PresserManpower as int),
	[Optrs of Packer] =  Cast( lm.PackerManpower as int),

	---- 公式：3600 / [Highest GSD Time]
	[EOLR] = IIF(lm.HighestGSDTime=0 ,0 ,3600.0 / lm.HighestGSDTime),

	---- P05呈現空白
	[Efficiency %] = Cast( NULL as decimal) ,

	---- P05 公式：P05沒有TotalCycle，所以用GSD
	[Line Balancing %] = IIF( lm.HighestGSDTime = 0 or lm.SewerManpower = 0 ,0 , lm.TotalGSDTime / lm.HighestGSDTime / lm.SewerManpower ),

	[Target Line Balancing% ]= (select top 1 co.Target from ChgOverTarget co where co.Type = 'LBR') / 100,
	[Not Hit Target Type] = '',
	[Total No. of Not Hit Target ] = iif(lm.Version <> 1,0,(select cnt = iif(count(*) = 0, '', cast(count(1) as varchar))
                    from (
	                    select l2.ID, l2.NO
	                    from AutomatedLineMapping_NotHitTargetReason l2 WITH (NOLOCK)
	                    where lm.ID = l2.ID
                    )a )),
	[Not Hit Target Reason] = NotHitTargetReason.Val,

	---- P05空白
	[Lean Line Eff %] = Cast( NULL as decimal),
	
	---- 公式：( [EOLR] * [CPU / PC] ) / [Current # of Optrs]
	[PPH] =IIF(lm.HighestGSDTime =0 OR lm.SewerManpower =0,0,  ( ( 3600.0 / lm.HighestGSDTime) * lm.StyleCPU ) / lm.SewerManpower),

	lm.Status,
	[GSD Status] = lm.TimeStudyStatus,
	[GSDVersion] = lm.TimeStudyVersion,
	lm.AddName,
	lm.AddDate,
	lm.EditName,
	lm.EditDate,
    IsFrom = 'IE P05'
from #AutomatedLineMapping lm WITH (NOLOCK) 
inner join Factory f on f.ID = lm.FactoryID
inner join Style s on s.Ukey = lm.StyleUKey
left join TimeStudy t WITH (NOLOCK) on  lm.TimeStudyID = t.ID
left join TimeStudyHistory th WITH (NOLOCK) on lm.TimeStudyID = th.ID
outer apply(
	select Val = STUFF( (
        select DISTINCT ',' + Reason
        from #NotHitTargetReason r 
        where r.ID = lm.ID
		FOR XML PATH('')
		),1,1,'')
)NotHitTargetReason
outer apply(
	select StdSMV =  SUM(td.StdSMV)
	from TimeStudy_Detail td WITH (NOLOCK) where t.id = td.id
        and td.IsSubprocess = 0
        and td.IsNonSewingLine =0
        and td.PPA <> 'C'
)tdd 
outer apply(
	select StdSMV =  SUM(td.StdSMV)
	from TimeStudyHistory_Detail td WITH (NOLOCK) where th.id = td.id
        and td.IsSubprocess = 0
        and td.IsNonSewingLine =0
        and td.PPA <> 'C'
)tddh
where 1 = 1
");

            if (this.latestVersion)
            {
                cmd.Append(@"
 and lm.Version = (
	select MAX(l.Version)
	from #AutomatedLineMapping l
	where l.StyleUKey = lm.StyleUKey
	and l.FactoryID = lm.FactoryID
	and l.Phase = lm.Phase
	group by l.StyleUKey, l.FactoryID,l.Phase
 )
");
            }

            cmd.Append(Environment.NewLine + "DROP TABLE #AutomatedLineMapping,#AutomatedLineMapping_Detail,#NotHitTargetReason ");

            return cmd;
        }

        private StringBuilder GetSummaryP06()
        {
            StringBuilder cmd = new StringBuilder();

            #region Inline & Sewing Date is not null

            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                cmd.Append($@"
select *
INTO #LineMappingBalancing
from LineMappingBalancing t WITH (NOLOCK) 
where exists(
	select 1
	from SewingSchedule ss
	join Orders o on ss.OrderID = o.ID
	where 1=1
	{dateQuery}
	and o.StyleID = t.StyleID and o.SeasonID = t.SeasonID and o.BrandID = t.BrandID
 )

");
            }
            #endregion

            if (!MyUtility.Check.Empty(this.factory))
            {
                cmd.Append(string.Format(" and t.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                cmd.Append(string.Format(" and t.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                cmd.Append(string.Format(" and t.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.team))
            {
                cmd.Append(string.Format(" and t.Team = '{0}'", this.team));
            }

            if (!MyUtility.Check.Empty(this.phase))
            {
                cmd.Append(string.Format(" and t.Phase = '{0}'", this.phase));
            }

            cmd.Append(@"

select lmd.*
INTO #LineMappingBalancing_Detail
from #LineMappingBalancing lm
inner join LineMappingBalancing_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
where lmd.No <> ''


select DISTINCT lm.ID, [Reason] = i.Description
INTO #NotHitTargetReason
from LineMappingBalancing_NotHitTargetReason r
inner join IEReason i on i.ID = r.IEReasonID and i.Type = 'AS'
inner join #LineMappingBalancing_Detail lmd on r.ID = lmd.ID and r.No = lmd.No
inner join #LineMappingBalancing lm on lm.ID = lmd.ID

select distinct
	f.CountryID,
	lm.FactoryID,
	lm.StyleID,
	lm.ComboType,
	lm.SeasonID,
    lm.Phase,
	lm.BrandID,
	s.Description,
	lm.Version,
	lm.SewingLineID,
	lm.Team,


	[Current # of Optrs] = Cast(lm.SewerManpower as int),

	---- 公式：( 3600 * [Current # of Optrs] ) / [Total Cycle Time]
	[Target/Hr. (100%)] = CAST( IIF(lm.TotalCycleTime=0 ,0  ,ROUND( (3600.0 * lm.SewerManpower) / lm.TotalCycleTime, 0)) as int),

	---- 公式：( 3600 * [No. of Hours] ) / [Daily Demand / Shift]	
	[Takt Time] = ROUND(
                    IIF( CAST(  ROUND( CAST( IIF(lm.TotalCycleTime=0 ,0  ,ROUND( (3600.0 * lm.SewerManpower) / lm.TotalCycleTime, 0)) as int) * lm.WorkHour ,0) as int ) = 0
                    ,0
                    ,3600 * lm.Workhour / CAST(  ROUND( CAST( IIF(lm.TotalCycleTime=0 ,0  ,ROUND( (3600.0 * lm.SewerManpower) / lm.TotalCycleTime, 0)) as int) * lm.WorkHour ,0) as int ) )
                ,2),

    [Std. SMV] = Cast(ISNULL(tdd.StdSMV,tddh.StdSMV) as decimal(10,2)),
    [GSD Style]= ISNULL(t.StyleID,th.StyleID),
    [GSD Season]= ISNULL(t.SeasonID,th.SeasonID),
    [GSD BrandID]= ISNULL(t.BrandID,th.BrandID),
    [Ori. Total GSD Time] =  Cast(lm.OriTotalGSDTime as decimal(10,2)),
	[Total GSD Time] = lm.TotalGSDTime * 1.0,
	[Total Cycle Time] = lm.TotalCycleTime * 1.0,
	
	---- 公式: [Total Cycle Time] / [Current # of Optrs]
	[Avg. Cycle Time] = 1.0 * lm.TotalCycleTime / lm.SewerManpower,

	[CPU / PC] = lm.StyleCPU,
	[No. of Hours] = lm.Workhour,
	---- 公式：[Target / Hr.(100%)] * [No. of Hours]
	[Daily Demand / Shift] = CAST(  ROUND( CAST( IIF(lm.TotalCycleTime=0 ,0  ,ROUND( (3600.0 * lm.SewerManpower) / lm.TotalCycleTime, 0)) as int) * lm.WorkHour ,0) as int ),

    ---- P03 為空
	[Optrs of Presser] = Cast( lm.PresserManpower as int),
    ---- P03 為空
	[Optrs of Packer] = Cast( lm.PackerManpower as int),

	---- 公式：3600 / [Highest Cycle Time]
	[EOLR] = 3600.0 / lm.HighestCycleTime,

	---- P06空白
	[Efficiency %] = Cast( NULL as decimal),

	---- P03 公式：[Total Cycle Time] / [HighestCycle] / [Current # of Optrs]
	[Line Balancing %] = 1.0 * lm.TotalCycleTime / lm.HighestCycleTime / lm.SewerManpower,

	[Target Line Balancing% ]= (select top 1 co.Target from ChgOverTarget co where co.Type = 'LBR') / 100,
	[Not Hit Target Type] = '',
	[Total No. of Not Hit Target ] = iif(lm.Version <> 1,0,(select cnt = iif(count(*) = 0, '', cast(count(1) as varchar))
                    from (
	                    select l2.ID, l2.NO
	                    from LineMappingBalancing_NotHitTargetReason l2 WITH (NOLOCK)
	                    where lm.ID = l2.ID
                    )a )),
	[Not Hit Target Reason]=NotHitTargetReason.Val,

	---- P06空白
	[Lean Line Eff %] = Cast( NULL as decimal),

	---- 公式：( [EOLR] * [CPU / PC] ) / [Current # of Optrs]
	[PPH] = IIF(lm.HighestCycleTime = 0 or lm.SewerManpower = 0 , 0 ,  ( 3600.0 / lm.HighestCycleTime) * s.CPU / lm.SewerManpower),

	lm.Status,
	[GSD Status] = lm.TimeStudyStatus,
	[GSDVersion] = lm.TimeStudyVersion,
	lm.AddName,
	lm.AddDate,
	lm.EditName,
	lm.EditDate,
    IsFrom = 'IE P06'
from #LineMappingBalancing lm WITH (NOLOCK) 
inner join Factory f on f.ID = lm.FactoryID
inner join Style s on s.Ukey = lm.StyleUKey
left join TimeStudy t WITH (NOLOCK) on lm.TimeStudyID = t.ID
left join TimeStudyHistory th WITH (NOLOCK) on lm.TimeStudyID = th.ID
outer apply(
	select Val = STUFF( (
        select DISTINCT ',' + Reason
        from #NotHitTargetReason r 
        where r.ID = lm.ID
		FOR XML PATH('')
		),1,1,'')
)NotHitTargetReason
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID and c.EffectiveDate < iif(lm.Editdate is null,lm.adddate,lm.Editdate) and c. Type ='LBR'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)LinebalancingTarget 
outer apply(
	select StdSMV =  SUM(td.StdSMV)
	from TimeStudy_Detail td WITH (NOLOCK) where t.id = td.id
        and td.IsSubprocess = 0
        and td.IsNonSewingLine =0
        and td.PPA <> 'C'
)tdd 
outer apply(
	select StdSMV =  SUM(td.StdSMV)
	from TimeStudyHistory_Detail td WITH (NOLOCK) where th.id = td.id
        and td.IsSubprocess = 0
        and td.IsNonSewingLine =0
        and td.PPA <> 'C'
)tddh
where 1 = 1
");

            if (this.bolBalancing)
            {
                cmd.Append(@"
and IIF(lm.HighestCycleTime*lm.SewerManpower = 0,0,CONVERT(DECIMAL,lm.TotalCycleTime)/lm.HighestCycleTime/lm.SewerManpower) * 100 < LinebalancingTarget.Target
");
            }

            if (this.latestVersion)
            {
                cmd.Append(@"
 and lm.Version = (
	select MAX(l.Version)
	from #LineMappingBalancing l
	where l.StyleUKey = lm.StyleUKey
	and l.FactoryID = lm.FactoryID
	and l.Phase = lm.Phase
    and l.SewingLineID = lm.SewingLineID
	group by l.StyleUKey, l.FactoryID,l.Phase,l.SewingLineID
 )
");
            }

            cmd.Append(Environment.NewLine + "DROP TABLE #LineMappingBalancing,#LineMappingBalancing_Detail ");

            return cmd;
        }
    }
}

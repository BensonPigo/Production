using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Configuration;
using System.Linq;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// R08
    /// </summary>
    public partial class R08 : Win.Tems.PrintForm
    {
        private string M;
        private string factory;
        private string style;
        private string season;
        private string brand;
        private string team;
        private string phase;
        private string line;
        private string reportType;
        private bool latestVersion;
        private DataTable PrintData;
        private DataTable BrandData;
        private DataTable SeasonData;
        private DataTable StyleData;

        /// <summary>
        /// R07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboPhase, 1, 1, ",Initial,Prelim,Final");
            MyUtility.Tool.SetupCombox(this.comboReportType, 1, 1, "Line Mapping,Auto Line Mapping");
            this.comboM.SetDefalutIndex();
            this.comboFty.SetDefalutIndex(string.Empty);
            this.txtbrand.MultiSelect = true;

            this.InitAllRegionData();
        }

        private void InitAllRegionData()
        {

            string brandSql = $@"select ID, NameCH, NameEN from Brand where Junk = 0";
            string seasonSql = $@"select distinct ID from Season WITH (NOLOCK) where Junk = 0";
            string styleSql = $@"select distinct ID,BrandID,Description from Style WITH (NOLOCK) where Junk = 0 ";

            #region --由Factory.PmsPath抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>();
            foreach (string ss in strSevers)
            {
                if (!MyUtility.Check.Empty(ss))
                {
                    var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                    connectionString.Add(connections);
                }
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                this.ShowErr(new DualResult(false, "no connection loaded."));
            }
            #endregion

            DualResult result;
            foreach (string conString in connectionString)
            {
                SqlConnection conn = new SqlConnection(conString);

                // 全廠區 Brand
                result = DBProxy.Current.SelectByConn(conn, brandSql, null, out DataTable dtBrand);

                if (!result)
                {
                    DBProxy.Current.DefaultTimeout = 300;
                    this.ShowErr(result);
                }

                if (dtBrand == null)
                {
                    this.BrandData = dtBrand.Clone();
                }
                else
                {
                    this.BrandData.Merge(dtBrand);
                }

                // 全廠區 Season
                result = DBProxy.Current.SelectByConn(conn, brandSql, null, out DataTable dtSeason);

                if (!result)
                {
                    DBProxy.Current.DefaultTimeout = 300;
                    this.ShowErr(result);
                }

                if (dtSeason == null)
                {
                    this.SeasonData = dtSeason.Clone();
                }
                else
                {
                    this.SeasonData.Merge(dtSeason);
                }

                // 全廠區 Season
                result = DBProxy.Current.SelectByConn(conn, styleSql, null, out DataTable dtStyle);

                if (!result)
                {
                    DBProxy.Current.DefaultTimeout = 300;
                    this.ShowErr(result);
                }

                if (dtStyle == null)
                {
                    this.StyleData = dtStyle.Clone();
                }
                else
                {
                    this.StyleData.Merge(dtStyle);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (!this.dtOutputDate.HasValue1 && !this.dtOutputDate.HasValue2
                && !this.dtInlineDate.HasValue1 && !this.dtInlineDate.HasValue2
                && !this.dtSewingDate.HasValue1 && !this.dtSewingDate.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please at least fill in one date selection!!");
                return false;
            }

            this.M = this.comboM.Text;
            this.factory = this.comboFty.Text;
            this.style = this.txtstyle.Text;
            this.season = this.txtSeason.Text;
            this.brand = this.txtbrand.Text;
            this.phase = this.comboPhase.Text;
            this.line = this.txtsewingline.Text;
            this.reportType = this.comboReportType.Text;
            this.latestVersion = this.chkLatestVersion.Checked;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result;
            StringBuilder cmd = new StringBuilder();

            if (this.reportType == "Line Mapping")
            {
                cmd = this.GetSummaryP03();
            }

            if (this.reportType == "Auto Line Mapping")
            {
                cmd = this.GetSummaryP05();
            }

            #region --由Factory.PmsPath抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>();
            foreach (string ss in strSevers)
            {
                if (!MyUtility.Check.Empty(ss))
                {
                    var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                    connectionString.Add(connections);
                }
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            DBProxy.Current.DefaultTimeout = 1800;

            foreach (string conString in connectionString)
            {
                SqlConnection conn = new SqlConnection(conString);
                result = DBProxy.Current.SelectByConn(conn, cmd.ToString(), null, out DataTable dt);

                if (!result)
                {
                    DBProxy.Current.DefaultTimeout = 300;
                    this.ShowErr(result);
                    return result;
                }

                if (dt == null)
                {
                    this.PrintData = dt.Clone();
                }

                this.PrintData.Merge(dt);
            }

            DBProxy.Current.DefaultTimeout = 300;

            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.PrintData.Rows.Count);
            if (this.PrintData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string fileName = "Centralized_R08";
            string strXltName = Env.Cfg.XltPathDir + $"\\{fileName}.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            MyUtility.Excel.CopyToXls(this.PrintData, string.Empty, fileName + ".xltx", 1, showExcel: false, fieldList: null, excelApp: excel);

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
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

        private StringBuilder GetSummaryP03()
        {
            StringBuilder cmd = new StringBuilder();

            #region Inline & Sewing Date is not null

            if (this.dtSewingDate.Value1.HasValue || this.dtSewingDate.Value2.HasValue
                || this.dtInlineDate.Value1.HasValue || this.dtInlineDate.Value2.HasValue
                || this.dtOutputDate.Value1.HasValue || this.dtOutputDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dtInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dtInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dtInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dtInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dtSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dtSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dtSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dtSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dtSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dtSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
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

            if (!MyUtility.Check.Empty(this.phase))
            {
                cmd.Append(string.Format(" and t.Phase = '{0}'", this.phase));
            }

            if (!MyUtility.Check.Empty(this.line))
            {
                cmd.Append(string.Format(" and t.SewingLineID = '{0}'", this.line));
            }

            cmd.Append(@"
select lmd.*
INTO #LineMapping_Detail
from #LineMapping lm
inner join LineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID

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

	---- 公式：[Target / Hr.(100%)] * [No. of Hours]
	[Daily Demand / Shift] = ((3600.0 * lm.CurrentOperators) / lm.TotalCycle) * lm.Workhour,

	[Current # of Optrs] = Cast( lm.CurrentOperators as int),

	---- 公式：( 3600 * [Current # of Optrs] ) / [Total Cycle Time]
	[Target/Hr. (100%)] = (3600.0 * lm.CurrentOperators) / lm.TotalCycle,

	---- 公式：( 3600 * [No. of Hours] ) / [Daily Demand / Shift]
	[Takt Time] = ( 3600.0 * lm.Workhour ) / ( ((3600.0 * lm.CurrentOperators) / lm.TotalCycle) * lm.Workhour ),

	[Total GSD Time] = lm.TotalGSD * 1.0,
	[Total Cycle Time] = lm.TotalCycle * 1.0,
	
	---- 公式: [Total Cycle Time] / [Current # of Optrs]
	[Avg. Cycle Time] = 1.0 * lm.TotalCycle / lm.CurrentOperators,

	[CPU / PC] = s.CPU,
	[No. of Hours] = lm.Workhour,

    ---- P03 為空
	[Optrs of Presser] = 0,
    ---- P03 為空
	[Optrs of Packer] = 0,

	---- 公式：3600 / [Highest Cycle Time]
	[EOLR] = 3600.0 / lm.HighestCycle,

	[Efficiency %] = IIF(lm.HighestCycle*lm.CurrentOperators = 0 ,0 , 1.0 * lm.TotalGSD / lm.HighestCycle / lm.CurrentOperators ) ,

	---- P03 公式：[Total Cycle Time] / [HighestCycle] / [Current # of Optrs] * 100
	[Line Balancing %] = 1.0 * lm.TotalCycle / lm.HighestCycle / lm.CurrentOperators * 100,

	[Target Line Balancing% ]= (select top 1 co.Target from ChgOverTarget co where co.Type = 'LBR') ,
	[Not Hit Target Type] = ISNULL(i.TypeGroup ,'') ,
	[Total No. of Not Hit Target ] = iif(lm.Version = 1,0,(select cnt = iif(count(*) = 0, '', cast(count(1) as varchar))
                    from (
	                    select distinct l2.NO, l2.IEReasonLBRNotHit_DetailUkey
	                    from #LineMapping_Detail l2 WITH (NOLOCK)
	                    where lm.ID = l2.ID
	                    and ISNULL(l2.IEReasonLBRNotHit_DetailUkey, '') <> ''
                    )a )),
	[Not Hit Target Reason]=ISNULL(i.Name ,'') ,

	---- 公式：[Total Cycle time] / [Takt Time] / [Current # of Optrs] * 100
	[Lean Line Eff %] = 1.0 * lm.TotalCycle / ( ( 3600.0 * lm.Workhour ) / ( ((3600.0 * lm.CurrentOperators) / lm.TotalCycle) * lm.Workhour )) / lm.CurrentOperators * 100,

	---- 公式：( [EOLR] * [CPU / PC] ) / [Current # of Optrs]
	[PPH] = ( ( 3600.0 / lm.HighestCycle) * s.CPU ) / lm.CurrentOperators,

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
left join IEReasonLBRnotHit_1st i on i.Ukey = lm.IEReasonLBRNotHit_1stUkey
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID and c.EffectiveDate < iif(lm.Editdate is null,lm.adddate,lm.Editdate) and c. Type ='LBR'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)LinebalancingTarget 
where 1 = 1
");

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

            if (this.dtSewingDate.Value1.HasValue || this.dtSewingDate.Value2.HasValue
                || this.dtInlineDate.Value1.HasValue || this.dtInlineDate.Value2.HasValue
                || this.dtOutputDate.Value1.HasValue || this.dtOutputDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dtInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dtInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dtInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dtInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dtSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dtSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dtSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dtSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dtSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dtSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
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

	---- 公式：P05沒有TotalCycle，所以為0
	[Daily Demand / Shift] = 0.0,

	[Current # of Optrs] = Cast( lm.SewerManpower as int),

	---- 公式：P05沒有TotalCycle，所以為0
	[Target/Hr. (100%)] = 0.0,

	---- 公式：P05沒有TotalCycle，所以為0
	[Takt Time] = 0.0,

	[Total GSD Time] = lm.TotalGSDTime * 1.0,
	[Total Cycle Time] = 0.0,
	
	---- 公式: P05沒有TotalCycle，所以為0
	[Avg. Cycle Time] = 0.0,

	[CPU / PC] = s.CPU,
	[No. of Hours] = lm.Workhour,

	[Optrs of Presser] = Cast( lm.PresserManpower as int),
	[Optrs of Packer] =  Cast( lm.PackerManpower as int),

	---- 公式：P05沒有Cycle，所以為0
	[EOLR] = 0.0,

	---- P05/P06呈現空白
	[Efficiency %] = 0.0 ,

	---- P05 公式：P05沒有TotalCycle，所以為0
	[Line Balancing %] = 0.0,

	[Target Line Balancing% ]= (select top 1 co.Target from ChgOverTarget co where co.Type = 'LBR') ,
	[Not Hit Target Type] = '',
	[Total No. of Not Hit Target ] = iif(lm.Version = 1,0,(select cnt = iif(count(*) = 0, '', cast(count(1) as varchar))
                    from (
	                    select distinct l2.NO, l2.Ukey
	                    from #AutomatedLineMapping_Detail l2 WITH (NOLOCK)
	                    where lm.ID = l2.ID
	                    and ISNULL(l2.Ukey, '') <> ''
                    )a )),
	[Not Hit Target Reason] = '',

	---- 公式：P05沒有TotalCycle，所以為0
	[Lean Line Eff %] = 0.0,
	
	---- 公式：P05沒有Cycle，所以為0
	[PPH] = 0.0,

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
where 1 = 1
");

            if (this.latestVersion)
            {
                cmd.Append(@"
 and lm.Version = (
	select MAX(l.Version)
	from LineMapping l
	where l.StyleUKey = lm.StyleUKey
	and l.FactoryID = lm.FactoryID
	and l.Phase = lm.Phase
	group by l.StyleUKey, l.FactoryID,l.Phase
 )
");
            }

            cmd.Append(Environment.NewLine + "DROP TABLE #AutomatedLineMapping,#AutomatedLineMapping_Detail ");
            return cmd;
        }
    }
}

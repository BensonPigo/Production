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
using System.Reflection;
using Sci.Production.Prg;

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
        private string line;
        private string category;
        private DataTable PrintData;
        private DataTable BrandData;
        private DataTable SeasonData;
        private DataTable StyleData;
        private DataTable LineData;

        public static class OrderCategory
        {
            public static string Bulk { get { return "B"; } }

            public static string Garment { get { return "G"; } }

            public static string Material { get { return "M"; } }

            public static string Sample { get { return "S"; } }

            public static string SMTL { get { return "T"; } }

            public static string GetValueByPropertyName(string propertyName)
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    return string.Empty;
                }

                // 用 '+' 分隔出多個屬性名稱
                var propertyNames = propertyName.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                // 用來存放各屬性的對應值
                var values = new List<string>();

                foreach (var name in propertyNames)
                {
                    // 去除前後空白
                    var trimmedName = name.Trim();

                    // 利用反射找出對應的靜態屬性
                    var property = typeof(OrderCategory).GetProperty(trimmedName, BindingFlags.Public | BindingFlags.Static);
                    if (property == null)
                    {
                        throw new ArgumentException($"Property '{trimmedName}' does not exist in OrderCategory.");
                    }

                    // 取得屬性的值，轉成字串後放進集合
                    values.Add(property.GetValue(null)?.ToString());
                }

                // 依需求組合字串，這裡示範回傳格式：'B','S'
                return $"'{string.Join("','", values)}'";
            }
        }

        /// <summary>
        /// R08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.SetDefalutIndex();
            this.comboFty.SetDefalutIndex(string.Empty);

            MyUtility.Tool.SetupCombox(this.comboCategory, 1, 1, ",Bulk,Sample,Garment,Material,SMTL,Bulk+Sample,Bulk+Sample+Garment");
            this.comboCategory.SelectedIndex = 0;
            this.InitAllRegionData();
        }

        private void InitAllRegionData()
        {
            string brandSql = $@"select ID, NameCH, NameEN from Brand where Junk = 0 ";
            string seasonSql = $@"select distinct ID,BrandID from Season WITH (NOLOCK) where Junk = 0";
            string styleSql = $@"select distinct ID,BrandID,SeasonID,Description from Style WITH (NOLOCK) where Junk = 0 ";
            string lineSql = $@"Select ID,FactoryID,Description From SewingLine WHERE junk=0 ";

            #region --由Factory.PmsPath抓各個連線路徑
            this.SetLoadingText("Load connections... ");

            List<string> connectionString = CentralizedClass.AllFactoryConnectionString();

            if (connectionString == null || connectionString.Count == 0)
            {
                this.ShowErr(new DualResult(false, "no connection loaded."));
            }
            #endregion

            DualResult result;
            this.BrandData = new DataTable();
            this.SeasonData = new DataTable();
            this.StyleData = new DataTable();
            this.LineData = new DataTable();

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
                    this.BrandData.Merge(dtBrand, true);
                }

                // 全廠區 Season
                result = DBProxy.Current.SelectByConn(conn, seasonSql, null, out DataTable dtSeason);

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
                    this.SeasonData.Merge(dtSeason, true);
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
                    this.StyleData.Merge(dtStyle, true);
                }

                // 全廠區 Line
                result = DBProxy.Current.SelectByConn(conn, lineSql, null, out DataTable dtLine);

                if (!result)
                {
                    DBProxy.Current.DefaultTimeout = 300;
                    this.ShowErr(result);
                }

                if (dtLine == null)
                {
                    this.LineData = dtLine.Clone();
                }
                else
                {
                    this.LineData.Merge(dtLine, true);
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
            this.style = this.txtStyle.Text;
            this.season = this.txtSeason.Text;
            this.brand = this.txtBrand.Text;
            this.line = this.txtLine.Text;
            this.category = OrderCategory.GetValueByPropertyName(this.comboCategory.Text);

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
            this.PrintData = new DataTable();
            StringBuilder cmd = this.GetSqlCmd();

            #region --由Factory.PmsPath抓各個連線路徑
            List<string> connectionString = CentralizedClass.AllFactoryConnectionString();
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
                    continue;
                }

                if (this.PrintData == null)
                {
                    this.PrintData = dt.Clone();
                }

                if (dt.Rows.Count > 0)
                {
                    this.PrintData.Merge(dt);
                }
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

        private StringBuilder GetSqlCmd()
        {
            StringBuilder cmd = new StringBuilder();

            string headQuery = string.Empty;
            #region Output、 Inline 、Sewing Date
            if (this.dtOutputDate.Value1.HasValue && this.dtOutputDate.Value2.HasValue)
            {
                headQuery += $@" and a.OutputDate BETWEEN '{this.dtOutputDate.Value1.Value.ToString("yyyy-MM-dd")}' AND '{this.dtOutputDate.Value2.Value.ToString("yyyy-MM-dd")}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                headQuery += $@"and a.FactoryID = '{this.factory}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.M))
            {
                headQuery += $@"and a.MDivisionID = '{this.M}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.line))
            {
                headQuery += $@"and a.SewingLineID = '{this.line}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.dtInlineDate.Value1))
            {
                headQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dtInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
            }

            if (!MyUtility.Check.Empty(this.dtInlineDate.Value2))
            {
                headQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dtInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
            }

            if (!MyUtility.Check.Empty(this.dtSewingDate.Value1))
            {
                headQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dtSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dtSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
            }

            if (!MyUtility.Check.Empty(this.dtSewingDate.Value2))
            {
                headQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dtSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dtSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
            }

            #endregion

            string detailQuery = string.Empty;
            string spQuery = string.Empty;
            string spQuery2 = string.Empty;
            #region detailQuery
            if (!MyUtility.Check.Empty(this.brand))
            {
                detailQuery += $@"and o.BrandID = '{this.brand}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                detailQuery += $@"and o.SeasonID = '{this.season}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                detailQuery += $@"and s.ID = '{this.style}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.category))
            {
                detailQuery += $@"and o.Category IN ({this.category}) " + Environment.NewLine;
                spQuery += $@"and Category IN ({this.category}) " + Environment.NewLine;
                spQuery2 += $@"and o.Category IN ({this.category}) " + Environment.NewLine;
            }
            #endregion

            cmd.Append($@"
---- 1. 整理出Sewing output有哪些產線，並將 Sewing 需要群組加總的先加一加
select DISTINCT a.*
into #SewingOutput
from SewingOutput a
INNER JOIN SewingOutput_Detail b WITH (NOLOCK) on a.ID = b.ID
INNER JOIN SewingSchedule ss ON b.OrderId = ss.OrderID
where 1=1
{headQuery}

select DISTINCT o.StyleUkey
    ,Program = IIF(o.Category='M', MockupProgram.Program, OrderProgram.Program)
    ,Category = cc.Val
INTO #OrderInfo
from #SewingOutput a
inner join SewingOutput_Detail b WITH (NOLOCK) on a.ID = b.ID
inner join Orders o on b.OrderId = o.ID
OUTER APPLY(
	select Val = STUFF((
		select distinct ',' + 
                CASE WHEN x.Category = '{OrderCategory.Bulk}' THEN 'Bulk'
                    WHEN x.Category = '{OrderCategory.Sample}' THEN 'Sample'
                    WHEN x.Category = '{OrderCategory.Garment}' THEN 'Garment'
                    WHEN x.Category = '{OrderCategory.Material}' THEN 'Material'
                    WHEN x.Category = '{OrderCategory.SMTL}' THEN 'SMTL'
                    ELSE x.Category
                END
		from Orders x
		where x.StyleUkey = o.StyleUkey {spQuery}
		FOR XML PATH('')
	),1,1,'')
)cc
OUTER APPLY(
	select Program = STUFF((
		select distinct ',' + o.ProgramID
		from Orders x
		where x.StyleUkey = o.StyleUkey
		FOR XML PATH('')
	),1,1,'')
)OrderProgram
OUTER APPLY(
	select Program = STUFF((
		select distinct ',' + o.ProgramID
		from MockupOrder x
		inner join Orders xo on b.OrderId = xo.ID
		where xo.StyleUkey = o.StyleUkey
		FOR XML PATH('')
	),1,1,'')
)MockupProgram
where o.SubconInSisterFty = 0  {spQuery2}

select b.ID
	,o.StyleUkey
	,b.ComboType
    ,p.Program
    ,p.Category
	,c.CountryID
	,o.BrandID
	,o.StyleID
	,s.Lining
	,s.Gender
	,s.SeasonID
	,s.CPU
	,s.ApparelType
	,s.FabricType
	,s.Construction
	,WorkHour = SUM(b.WorkHour)
	,InlineQty = SUM(b.InlineQty)
	,QAQty = SUM(b.QAQty)
	,b.CumulateSimilar
INTO #SewingOutput_Detail
from #SewingOutput a
inner join SewingOutput_Detail b WITH (NOLOCK) on a.ID = b.ID
inner join Factory c on a.FactoryID=c.ID
inner join Orders o on b.OrderId = o.ID
inner join Style s on o.StyleUkey = s.Ukey
inner join #OrderInfo p on p.StyleUkey=  s.Ukey
where SubconInSisterFty = 0 
{detailQuery}
GROUP BY b.ID,o.StyleUkey,b.ComboType,c.CountryID,o.BrandID,o.StyleID,s.Lining
	,s.Gender,s.SeasonID,s.CPU,s.ApparelType,s.FabricType,s.Construction,p.Program,p.Category,b.CumulateSimilar


---- 2. 根據Sewing的群組條件，挑出可以拿來篩選Line Mapping的欄位
----     * Line Mapping有三種資料來源，分別是P03、P05、P06，對應Table為LineMapping、AutomatedLineMapping、LineMappingBalancing
select DISTINCT b.StyleUKey
	,a.FactoryID
	,a.SewingLineID
	,a.Team
	,b.ComboType
INTO #BaseData
from #SewingOutput a
inner join #SewingOutput_Detail b on a.ID = b.ID


----- 每個產線，會有Before 和 After兩個產線計畫，這兩個產線計畫的資料來源可能來自P03、P05、P06，以下步驟開始找出「這兩筆資料在哪裡」


---- 3.  找出P03、P05、P06產線計畫每種Phase的最新版本：
---- 注意 P03 Before不會知道要在哪個 SewingLineID、Team
--P03
select lm.StyleUKey
	,lm.FactoryID
	,lm.SewingLineID
	,lm.Team
	,lm.ComboType
	,lm.Phase
    ,lm.Status
	,Version = MAX(lm.Version)
	,AddDate = MAX(lm.AddDate)
	,EditDate  = MAX(lm.EditDate )
	,ID  = MAX(lm.ID )
INTO #P03MaxVer ---- P03
from LineMapping lm 
where exists(
	select 1 from #BaseData a
	where lm.StyleUKey = a.StyleUkey and a.FactoryID=lm.FactoryID /*and lm.SewingLineID = a.SewingLineID and a.Team=lm.Team*/ and a.ComboType=lm.ComboType
)
GROUP BY lm.StyleUKey,lm.FactoryID,lm.SewingLineID,lm.Team,lm.ComboType,lm.Phase,lm.Status
ORDER BY lm.StyleUKey,lm.FactoryID,lm.SewingLineID,lm.Team,lm.ComboType,lm.Phase,lm.Status

---- 注意 P03 Before不會知道要在哪個 SewingLineID、Team
select lm.StyleUKey
	,lm.FactoryID
	,lm.SewingLineID
	,lm.Team
	,lm.ComboType
	,lm.Phase
    ,lm.Status
	,lm.Version
	,lm.AddDate
	,lm.EditDate
	,lm.ID
INTO #AllP03 ---- P03
from LineMapping lm 
where exists(
	select 1 from #BaseData a
	where lm.StyleUKey = a.StyleUkey and a.FactoryID=lm.FactoryID /*and lm.SewingLineID = a.SewingLineID and a.Team=lm.Team*/ and a.ComboType=lm.ComboType
)
ORDER BY lm.StyleUKey,lm.FactoryID,lm.SewingLineID,lm.Team,lm.ComboType,lm.Phase,lm.Status

--P05
select lm.StyleUKey
	,lm.FactoryID
	,SewingLineID = ''
	,Team = ''
	,lm.ComboType
	,lm.Phase
    ,lm.Status
	,Version = MAX(lm.Version)
	,AddDate = MAX(lm.AddDate)
	,EditDate  = MAX(lm.EditDate )
	,ID  = MAX(lm.ID )
INTO #P05MaxVer ---- P05
from AutomatedLineMapping lm 
where exists(
	select 1 from #BaseData a
	where lm.StyleUKey = a.StyleUkey and a.FactoryID=lm.FactoryID and a.ComboType=lm.ComboType 
)
GROUP BY lm.StyleUKey,lm.FactoryID,lm.ComboType,lm.Phase,lm.Status
ORDER BY lm.StyleUKey,lm.FactoryID,lm.ComboType,lm.Phase,lm.Status

select lm.StyleUKey
	,lm.FactoryID
	,SewingLineID = ''
	,Team = ''
	,lm.ComboType
	,lm.Phase
    ,lm.Status
	,lm.Version
	,lm.AddDate
	,lm.EditDate
	,lm.ID
INTO #AllP05 ---- P05
from AutomatedLineMapping lm 
where exists(
	select 1 from #BaseData a
	where lm.StyleUKey = a.StyleUkey and a.FactoryID=lm.FactoryID and a.ComboType=lm.ComboType 
)
ORDER BY lm.StyleUKey,lm.FactoryID,lm.ComboType,lm.Phase,lm.Status

-- P06：P05(AutomatedLineMapping)   > P06 (LineMappingBalancing.AutomatedLineMappingID)，所以找出P05衍生出的P06即可
select lm.StyleUKey
	,lm.FactoryID
	,lm.SewingLineID
	,lm.Team
	,lm.ComboType
	,lm.Phase
    ,lm.Status
	,Version = MAX(lm.Version)
	,AddDate = MAX(lm.AddDate)
	,EditDate  = MAX(lm.EditDate )
	,ID  = MAX(lm.ID )
INTO #P06MaxVer ---- P06
from LineMappingBalancing lm 
where exists(
	select 1 from #BaseData a
	where lm.StyleUKey = a.StyleUkey and a.FactoryID=lm.FactoryID and lm.SewingLineID = a.SewingLineID and a.Team=lm.Team and a.ComboType=lm.ComboType
)
and exists(
	select 1 from #P05MaxVer p05
	where p05.ID = lm.AutomatedLineMappingID
)
GROUP BY lm.StyleUKey,lm.FactoryID,lm.SewingLineID,lm.Team,lm.ComboType,lm.Phase,lm.Status
ORDER BY lm.StyleUKey,lm.FactoryID,lm.SewingLineID,lm.Team,lm.ComboType,lm.Phase,lm.Status

select lm.StyleUKey
	,lm.FactoryID
	,lm.SewingLineID
	,lm.Team
	,lm.ComboType
	,lm.Phase
    ,lm.Status
	,lm.Version
	,lm.AddDate
	,lm.EditDate
	,lm.ID
INTO #AllP06 ---- P06
from LineMappingBalancing lm 
where exists(
	select 1 from #BaseData a
	where lm.StyleUKey = a.StyleUkey and a.FactoryID=lm.FactoryID and lm.SewingLineID = a.SewingLineID and a.Team=lm.Team and a.ComboType=lm.ComboType
)
ORDER BY lm.StyleUKey,lm.FactoryID,lm.SewingLineID,lm.Team,lm.ComboType,lm.Phase,lm.Status

---- 4.  開始After Data準備
---- After Data的找法：
---- (1) 資料來源只有P03、P06 (其中P06是從P05轉過去的)
---- (2) P03、P06找出Phase = Final的產線計畫
---- (3) 每筆的Key值為 factory, brand, style, season, combo type, Line, Team，從P03或P06取一個
---- (4) 取的方式：如果這組只有P03或P06有就直接取；P03、P06 同時有則取 EditDate 大的那邊 (若皆無 EditDate 則比較 AddDate) 

--取column結構
select top 1 p03.*,SourceTable = 'IE P03'
INTO #AfterData
from #P03MaxVer p03
where 1=0

;WITH CombinedTable AS (
    SELECT *,'IE P03' AS SourceTable
    FROM #P03MaxVer a
	where Phase='Final'
    UNION ALL
    SELECT *,'IE P06' AS SourceTabl
    FROM #P06MaxVer a
	where Phase='Final'
),
RankedTable AS (
    SELECT 
        *,---- P03、P06交集，代表相同Key值有重複，因此判斷EditDate、AddDate
        ROW_NUMBER() OVER (PARTITION BY StyleUKey,FactoryID,SewingLineID,Team,ComboType,Phase ORDER BY 
            CASE 
                WHEN EditDate IS NOT NULL THEN EditDate 
                ELSE AddDate 
            END DESC) AS RowNum
    FROM CombinedTable
)

--SELECT * FROM RankedTable WHERE RowNum = 1;
INSERT INTO #AfterData (StyleUKey,FactoryID,SewingLineID,Team,ComboType,Phase,Version,AddDate,EditDate ,SourceTable,ID,Status)
SELECT a.StyleUKey,a.FactoryID,a.SewingLineID,a.Team,a.ComboType,Phase,Version,AddDate,EditDate ,SourceTable,ID,Status
FROM RankedTable a
INNER JOIN #BaseData b on b.StyleUKey = a.StyleUkey and a.FactoryID=b.FactoryID and b.SewingLineID = a.SewingLineID and a.Team=b.Team and a.ComboType=b.ComboType 
WHERE RowNum = 1
ORDER BY a.StyleUKey,a.FactoryID,a.SewingLineID,a.Team,a.ComboType,a.Phase,Version,AddDate,EditDate 

---- 5. 開始Before Data準備
---- Before Data的找法：
---- (1) 資料來源只有P03、P05 (因為P06是從P05轉過去的，所以只需要找出P06來源的P05即可)
---- (2) P03找出Phase = Initial 或 Prelim 的產線計畫，處理內部Phase的優先度問題，優先度：Prelim > Initial，先找出每個Key的Phase要用哪一種
---- (3) P03每筆的Key值為 factory, brand, style, season, combo type,，before本來就不會知道會在哪一條線生產，不要加Line、Team、Sewer等等判斷

---- 處理P03的Prelim、Initial 優先度問題。
;WITH PhaseRankedTableP03 AS (
	select 
        *,
        ROW_NUMBER() OVER (PARTITION BY StyleUKey,FactoryID,SewingLineID,Team,ComboType ORDER BY 
            CASE 
                WHEN Phase = 'Prelim' THEN 2
                ELSE 1
            END DESC) AS RowNum
	from #P03MaxVer p
	where Phase IN ('Prelim','Initial')
	and exists(
		select 1 from #AfterData af
		where SourceTable =  'IE P03'
		and p.FactoryID	 = af.FactoryID
		and p.StyleUKey	 = af.StyleUKey
		--and p.SewingLineID	 = af.SewingLineID
		--and p.Team	 = af.Team
		and p.combotype	 = af.combotype
	)
)

SELECT StyleUKey,FactoryID,SewingLineID,Team,ComboType,Phase,Version,AddDate,EditDate ,ID
INTO #P03Rank
FROM PhaseRankedTableP03
WHERE RowNum = 1


---- 處理P05的Prelim、Initial 優先度問題。
;WITH PhaseRankedTableP05 AS (
	select 
        maxVer.*,
        ROW_NUMBER() OVER (PARTITION BY maxVer.StyleUKey,maxVer.FactoryID,maxVer.SewingLineID,maxVer.Team,maxVer.ComboType ORDER BY 
            CASE 
                WHEN maxVer.Phase = 'Prelim' THEN 2
                ELSE 1
            END DESC) AS RowNum
	from #P05MaxVer maxVer
	inner join AutomatedLineMapping p05 on maxVer.ID = p05.ID
	where maxVer.Phase IN ('Prelim','Initial')
)

SELECT StyleUKey,FactoryID,SewingLineID,Team,ComboType,Phase,Version,AddDate,EditDate ,ID
INTO #P05Rank
FROM PhaseRankedTableP05
WHERE RowNum = 1
--組合所有Before：P03 Before => P03 After；P05 Before => P06 After

--P03的Before
select p03.*
,SourceTable = 'IE P03'
INTO #BeforeData
from #P03Rank p03
UNION
--P05Before (有P06 After，從P06推導得出，必須在 P06.AutomatedLineMappingID 當中)
select distinct p05.StyleUKey,p05.FactoryID,p05.SewingLineID,p05.Team,p05.ComboType,p05.Phase,p05.Version,p05.AddDate,p05.EditDate ,p05.ID
,SourceTable = 'IE P05'
from #AfterData p06
inner join  LineMappingBalancing lm on p06.ID = lm.ID
inner join #P05MaxVer p05 on p05.ID = lm.AutomatedLineMappingID
UNION
--P05的Before (沒有P06 After)
select p05.*
,SourceTable = 'IE P05' ---- P05 沒有Line Team
from #BaseData a
INNER join #P05Rank p05 on p05.StyleUKey = a.StyleUkey and a.FactoryID=p05.FactoryID and a.ComboType=p05.ComboType

---- 6. 前面已經鎖定了每一組 factory, brand, style, season, combo type, Line, Team ，對應到的兩個產線計畫(分別是Before和After)，最後可以去P03、P05、P06找出最終的那一筆，並取出需要的欄位就好
select a.*,b.Status
	,b.TotalGSD
	,b.TotalCycle
	,b.CurrentOperators
	,b.HighestCycle
	,b.TaktTime
	,b.Workhour
	,b.HighestGSD
    ,[Std. SMV] = Cast(ISNULL(tdd.StdSMV,tddh.StdSMV) as decimal(10,2))
    ,[GSD Style]= ISNULL(t.StyleID,th.StyleID)
    ,[GSD Season]= ISNULL(t.SeasonID,th.SeasonID)
    ,[GSD BrandID]= ISNULL(t.BrandID,th.BrandID)
INTO #FinalBeforeData
from #BeforeData a
inner join LineMapping b on a.ID = b.ID　---- P03
left join TimeStudy t WITH (NOLOCK) on b.TimeStudyID = t.ID
left join TimeStudyHistory th WITH (NOLOCK) on b.TimeStudyID = th.ID
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
WHERE a.SourceTable='IE P03'
UNION 
select a.*,b.Status
	,TotalGSD = TotalGSDTime
	,TotalCycle = 0
	,CurrentOperators = b.SewerManpower
	,HighestCycle = 0
	,TaktTime = 0
	,b.Workhour
	,HighestGSD = b.HighestGSDTime
    ,[Std. SMV] = Cast(ISNULL(tdd.StdSMV,tddh.StdSMV) as decimal(10,2))
    ,[GSD Style]= ISNULL(t.StyleID,th.StyleID)
    ,[GSD Season]= ISNULL(t.SeasonID,th.SeasonID)
    ,[GSD BrandID]= ISNULL(t.BrandID,th.BrandID)
from #BeforeData a
inner join AutomatedLineMapping b on a.ID = b.ID　---- P05
left join TimeStudy t WITH (NOLOCK) on b.TimeStudyID = t.ID
left join TimeStudyHistory th WITH (NOLOCK) on b.TimeStudyID = th.ID
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
WHERE a.SourceTable='IE P05'

select a.*
	,b.TotalGSD
	,b.TotalCycle
	,b.CurrentOperators
	,b.HighestCycle
	,b.HighestGSD
    ,[Ori. Total GSD Time] = Cast(b.OriTotalGSD as decimal(10,2))
INTO #FinalAfterData 
from #AfterData a
inner join LineMapping b on a.ID = b.ID ---- P03
WHERE a.SourceTable='IE P03'
UNION ALL
select a.*
	,TotalGSD = b.TotalGSDTime
	,TotalCycle = b.TotalCycleTime
	,CurrentOperators = b.SewerManpower
	,HighestCycle = b.HighestCycleTime
	,HighestGSD = b.HighestGSDTime
    ,[Ori. Total GSD Time] =  Cast(b.OriTotalGSDTime as decimal(10,2))
from #AfterData a
inner join LineMappingBalancing b on a.ID = b.ID ---- P06
WHERE a.SourceTable='IE P06'

---- 7. History LBR 需要從所有的After 篩選，所以準備所有的After
select a.*
	,b.TotalGSD
	,b.TotalCycle
	,b.CurrentOperators
	,b.HighestCycle
	,b.HighestGSD
    ,[Ori. Total GSD Time] = Cast(b.OriTotalGSD as decimal(10,2))
	,SourceTable='IE P03'
INTO #AllAfter
from #AllP03 a
inner join LineMapping b on a.ID = b.ID
UNION
select a.*
	,TotalGSD = b.TotalGSDTime
	,TotalCycle = b.TotalCycleTime
	,CurrentOperators = b.SewerManpower
	,HighestCycle = b.HighestCycleTime
	,HighestGSD = b.HighestGSDTime
    ,[Ori. Total GSD Time] =  Cast(b.OriTotalGSDTime as decimal(10,2))
	,SourceTable='IE P06'
from #AllP06 a
inner join LineMappingBalancing b on a.ID = b.ID

---- 8. 開始兜報表的欄位，把P03 to P03 、 P05 to P06兩種分開寫
select 
	b.CountryID
    --,b.StyleUkey
	,a.FactoryID
	,a.OutputDate
	,b.BrandID
	,b.StyleID
	,b.ComboType
    ,b.Program
	,ProductType = r1.Name
	,FabricType = r2.Name
	,b.Lining
	,b.Gender
	,Construction = ddl.Name
	,b.SeasonID
	,a.SewingLineID
	,a.Team
	,[Act. Manpower] = a.Manpower
	,[No.of Hours] = b.WorkHour

	---- 公式 [Act. Manpower] * [No.of Hours]
	,[Total Manhours] = a.Manpower * b.WorkHour
	,[CPU/piece] = b.CPU
	,[Prod. Output] = b.InlineQty

	---- 公式 [CPU/piece] * [Total Output]
	,[Total CPU] = b.CPU * b.QAQty
	,[Cumulate Of Days] = b.CumulateSimilar
	,[Inline Category] = CONCAT(a.SewingReasonIDForTypeIC, '-' + sr.Description) 
	,[New Style/Repeat style] = (select dbo.IsRepeatStyleBySewingOutput(a.FactoryID, a.OutputDate, a.SewinglineID, a.Team, b.StyleUkey))
    ,[Std. SMV] = BeforeDataP03.[Std. SMV]
    ,[GSD Style]= BeforeDataP03.[GSD Style]
    ,[GSD Season]= BeforeDataP03.[GSD Season]
    ,[GSD BrandID]= BeforeDataP03.[GSD BrandID]
	------------------------------------------------After ------------------------------------------------
	,[Phase after inline] = AfterDataP03.Phase
	,[Version after inline] = AfterDataP03.Version
	,[Oprts after inline] = AfterDataP03.CurrentOperators
    ,[Ori. Total GSD Time] =  AfterDataP03.[Ori. Total GSD Time] 
	,[Cycle Time] =  AfterDataP03.TotalCycle
	,[Avg. Cycle] =  AfterDataP03.AvgCycle
	,[LBR after inline] = AfterDataP03.LBR
	,[Target LBR] = LinebalancingTarget.Target 
	,[After inline Is From] = AfterDataP03.SourceTable
	,[After inline Status] =  AfterDataP03.Status
	,[Est. PPH] = AfterDataP03.EstPPH
	------------------------------------------------After ------------------------------------------------

	------------------------------------------------Before ------------------------------------------------
	,[Phase before inline] =   BeforeDataP03.Phase 
	,[Version before inline] = BeforeDataP03.Version
	,[Oprts before inline] = BeforeDataP03.CurrentOperators
	,[GSD time] = BeforeDataP03.TotalGSD
	,[Takt time] = BeforeDataP03.Takt

	,[LBR before inline] = BeforeDataP03.LBR
	,[Before inline Is From] = BeforeDataP03.SourceTable
	,[Before inline Status] = BeforeDataP03.Status
	------------------------------------------------Before ------------------------------------------------

	,[Oprts Diff] = AfterDataP03.CurrentOperators - BeforeDataP03.CurrentOperators
	,[LBR Diff (%)] = AfterDataP03.LBR - BeforeDataP03.LBR
	,[Total % Time diff] = IIF(BeforeDataP03.TotalGSD = 0 , 0 , (( BeforeDataP03.TotalGSD - AfterDataP03.TotalCycle) / AfterDataP03.TotalCycle )) * 100
	,[By style] = IIF(AfterDataP03.Status = 'Confirmed' OR BeforeDataP03.Status = 'Confirmed','Y','N')
	,[By Line] = IIF(AfterDataP03.Status = 'Confirmed','Y','N')
	,[Last Version From] = LastVersion.SourceTable
	,[Last Version Phase] = LastVersion.Phase
	,[Last Version Status] = LastVersion.Status
	,[History LBR] = AfterData.LBR
    ,b.Category
from #SewingOutput a 
inner join #SewingOutput_Detail b on a.ID = b.ID
left join Reason r1 on r1.ReasonTypeID= 'Style_Apparel_Type' and r1.ID = b.ApparelType
left join Reason r2 on r2.ReasonTypeID= 'Fabric_Kind' and r2.ID = b.FabricType
left join DropDownList ddl on ddl.Type= 'StyleConstruction' and ddl.ID = b.Construction
left join SewingReason sr on  sr.ID = a.SewingReasonIDForTypeIC and sr.Type='IC'
Outer Apply(
	select TOP 1 * ---- 因為產線計畫不會有 OutputDate 的區別，因此都會長得一樣，取Top 1即可
	---- Avg. Cycle 公式: [Total Cycle Time] / [Oprts after inline]
	,[AvgCycle] = IIF(lm.CurrentOperators = 0 ,0 , 1.0 * lm.TotalCycle / lm.CurrentOperators)
	---- P03公式: [Total Cycle Time] / [Highest cycle time of operator in shift] / [Current No of Oprts] * 100
	,[LBR] = IIF( lm.HighestCycle =0 OR lm.CurrentOperators = 0, 0,  1.0 * lm.TotalCycle / lm.HighestCycle / lm.CurrentOperators * 100 )
	---- 公式: [ELOR] × [CPU /PC] / [Oprts after inline]
	--- EOLR公式：3600 / [Highest Cycle Time]
	,[EstPPH] = IIF (lm.HighestCycle = 0  or lm.CurrentOperators = 0, 0,  (1.0 * 3600 / lm.HighestCycle) * b.CPU / lm.CurrentOperators )
	from #FinalAfterData lm
	where lm.StyleUKey = b.StyleUkey and a.FactoryID=lm.FactoryID and lm.SewingLineID = a.SewingLineID and a.Team=lm.Team and b.ComboType=lm.ComboType and lm.SourceTable = 'IE P03'
)AfterDataP03
Outer Apply(
	select top 1 * ---- 因為產線計畫不會有 OutputDate 的區別，因此都會長得一樣，取Top 1即可
	---- Avg. Cycle 公式: [Total Cycle Time] / [Oprts after inline]
	,[AvgCycle] = IIF(lm.CurrentOperators = 0 ,0 , 1.0 * lm.TotalCycle / lm.CurrentOperators)
	---- P03公式: [Total Cycle Time] / [Highest cycle time of operator in shift] / [Current No of Oprts] * 100
	,[LBR] = IIF( lm.HighestCycle =0 OR lm.CurrentOperators = 0, 0,  1.0 * lm.TotalCycle / lm.HighestCycle / lm.CurrentOperators * 100 )
	---- 公式: [ELOR] × [CPU /PC] / [Oprts after inline]
	--- EOLR公式：3600 / [Highest Cycle Time]
	,[EstPPH] = IIF (lm.HighestCycle = 0  or lm.CurrentOperators = 0, 0,  (1.0 * 3600 / lm.HighestCycle) * b.CPU / lm.CurrentOperators )
	from #AllAfter lm
	where lm.StyleUKey = b.StyleUkey and a.FactoryID=lm.FactoryID and lm.SewingLineID = a.SewingLineID and a.Team=lm.Team and b.ComboType=lm.ComboType AND CAST(lm.EditDate as Date) = a.OutputDate
	order by lm.EditDate desc
)AfterData
Outer Apply(
	select TOP 1 * ---- 因為產線計畫不會有 OutputDate 的區別，因此都會長得一樣，取Top 1即可
	,[AvgCycle] = IIF(lm.CurrentOperators = 0 ,0 , 1.0 * lm.TotalCycle / lm.CurrentOperators)
	,[Takt] = CAST( CASE  WHEN lm.SourceTable = 'IE P03' THEN lm.TaktTime
				   ELSE 0 END AS DECIMAL(7,2))
	------ 公式: [Total cycle time] / [Highest cycle time] / [Oprts after inline] * 100
	,[LBR] = CASE WHEN lm.SourceTable = 'IE P03' THEN IIF( lm.HighestGSD =0 OR lm.CurrentOperators = 0, 0,  1.0 * lm.TotalGSD / lm.HighestGSD / lm.CurrentOperators * 100 )
			 ELSE 0 END
	from #FinalBeforeData lm
	where lm.StyleUKey =b.StyleUkey and a.FactoryID=lm.FactoryID and b.ComboType=lm.ComboType
	and lm.SourceTable = 'IE P03'
)BeforeDataP03
outer apply(
	select top 1 ct.Target
	from factory f
	left join ChgOverTarget ct on ct.MDivisionID= f.MDivisionID 
				and ct. Type ='LBR'
	where f.id = a.FactoryID
	order by EffectiveDate desc
)LinebalancingTarget 
outer apply(
	select top 1 *
	from (
		select *
		from (
			select *,SourceTable='P03' from #P03MaxVer
		)LastData
		where b.StyleUkey = LastData.StyleUKey 
			and a.FactoryID = LastData.FactoryID 
			and b.ComboType = LastData.ComboType 
	)aa
	ORDER BY 
	  CASE Phase
		WHEN 'Final' THEN 3
		WHEN 'Prelim' THEN 2
		WHEN 'Initial' THEN 1
		ELSE 0
	  END DESC,
	  Version DESC
)LastVersion
WHERE BeforeDataP03.SourceTable IS NOT NULL
UNION
select 
	b.CountryID
    --,b.StyleUkey
	,a.FactoryID
	,a.OutputDate
	,b.BrandID
	,b.StyleID
	,b.ComboType
    ,b.Program
	,ProductType = r1.Name
	,FabricType = r2.Name
	,b.Lining
	,b.Gender
	,Construction = ddl.Name
	,b.SeasonID
	,a.SewingLineID
	,a.Team
	,[Act. Manpower] = a.Manpower
	,[No.of Hours] = b.WorkHour

	---- 公式 [Act. Manpower] * [No.of Hours]
	,[Total Manhours] = a.Manpower * b.WorkHour
	,[CPU/piece] = b.CPU
	,[Prod. Output] = b.InlineQty

	---- 公式 [CPU/piece] * [Total Output]
	,[Total CPU] = b.CPU * b.QAQty
	,[Cumulate Of Days] = b.CumulateSimilar
	,[Inline Category] = CONCAT(a.SewingReasonIDForTypeIC, '-' + sr.Description) 
	,[New Style/Repeat style] = (select dbo.IsRepeatStyleBySewingOutput(a.FactoryID, a.OutputDate, a.SewinglineID, a.Team, b.StyleUkey))
    ,[Std. SMV] = BeforeDataP05.[Std. SMV]
    ,[GSD Style]= BeforeDataP05.[GSD Style]
    ,[GSD Season]= BeforeDataP05.[GSD Season]
    ,[GSD BrandID]= BeforeDataP05.[GSD BrandID]
	------------------------------------------------After ------------------------------------------------
	,[Phase after inline] = AfterDataP06.Phase
	,[Version after inline] = AfterDataP06.Version
	,[Oprts after inline] = AfterDataP06.CurrentOperators
    ,[Ori. Total GSD Time] = AfterDataP06.[Ori. Total GSD Time]
	,[Cycle Time] = AfterDataP06.TotalCycle
	,[Avg. Cycle] = AfterDataP06.AvgCycle
	,[LBR after inline] = AfterDataP06.LBR
	,[Target LBR] = LinebalancingTarget.Target 
	,[After inline Is From] =  AfterDataP06.SourceTable
	,[After inline Status] =   AfterDataP06.Status
	,[Est. PPH] = AfterDataP06.EstPPH
	------------------------------------------------After ------------------------------------------------

	------------------------------------------------Before ------------------------------------------------
	,[Phase before inline] = BeforeDataP05.Phase
	,[Version before inline] = BeforeDataP05.Version
	,[Oprts before inline] = BeforeDataP05.CurrentOperators
	,[GSD time] = BeforeDataP05.TotalGSD
	,[Takt time] = BeforeDataP05.Takt

	,[LBR before inline] =BeforeDataP05.LBR
	,[Before inline Is From] = BeforeDataP05.SourceTable
	,[Before inline Status] = BeforeDataP05.Status
	------------------------------------------------Before ------------------------------------------------

	,[Oprts Diff] = AfterDataP06.CurrentOperators - BeforeDataP05.CurrentOperators
	,[LBR Diff (%)] = AfterDataP06.LBR - BeforeDataP05.LBR
	,[Total % Time diff] = IIF(BeforeDataP05.TotalGSD = 0 , 0 , (( BeforeDataP05.TotalGSD - AfterDataP06.TotalCycle) / AfterDataP06.TotalCycle )) * 100
	,[By style] = IIF( AfterDataP06.Status = 'Confirmed' OR BeforeDataP05.Status = 'Confirmed','Y','N')
	,[By Line] = IIF( AfterDataP06.Status = 'Confirmed','Y','N')
	,[Last Version From] = LastVersion.SourceTable
	,[Last Version Phase] = LastVersion.Phase
	,[Last Version Status] = LastVersion.Status
	,[History LBR] = AfterData.LBR
    ,b.Category
from #SewingOutput a 
inner join #SewingOutput_Detail b on a.ID = b.ID
left join Reason r1 on r1.ReasonTypeID= 'Style_Apparel_Type' and r1.ID = b.ApparelType
left join Reason r2 on r2.ReasonTypeID= 'Fabric_Kind' and r2.ID = b.FabricType
left join DropDownList ddl on ddl.Type= 'StyleConstruction' and ddl.ID = b.Construction
left join SewingReason sr on  sr.ID = a.SewingReasonIDForTypeIC and sr.Type='IC'
Outer Apply(
	select TOP 1 * ---- 因為產線計畫不會有 OutputDate 的區別，因此都會長得一樣，取Top 1即可
	,[AvgCycle] = IIF(lm.CurrentOperators = 0 ,0 , 1.0 * lm.TotalCycle / lm.CurrentOperators)
	,[Takt] = CAST( CASE  WHEN lm.SourceTable = 'IE P05' THEN IIF( lm.CurrentOperators  = 0 OR lm.TotalGSD = 0 OR lm.TotalGSD = 0 OR ( ( 3600 * lm.CurrentOperators / lm.TotalGSD) * lm.WorkHour ) = 0 
														,0
														,( 3600 * lm.WorkHour ) / ( ( 3600 * lm.CurrentOperators / lm.TotalGSD) * lm.WorkHour )
													)
				   ELSE 0 END AS DECIMAL(7,2))
	------ 公式: [Total cycle time] / [Highest cycle time] / [Oprts after inline] * 100
	,[LBR] = CASE WHEN lm.SourceTable = 'IE P05' THEN IIF( lm.HighestGSD =0 OR lm.CurrentOperators = 0, 0,  1.0 * lm.TotalGSD / lm.HighestGSD / lm.CurrentOperators * 100 )
			 ELSE 0 END
	from #FinalBeforeData lm
	where lm.StyleUKey =b.StyleUkey and a.FactoryID=lm.FactoryID --and lm.SewingLineID = a.SewingLineID and a.Team=lm.Team 
	and b.ComboType=lm.ComboType 
	and lm.SourceTable = 'IE P05'
)BeforeDataP05
Outer Apply(
	select TOP 1 * ---- 因為產線計畫不會有 OutputDate 的區別，因此都會長得一樣，取Top 1即可
	---- Avg. Cycle 公式: [Total Cycle Time] / [Oprts after inline]
	,[AvgCycle] = IIF(lm.CurrentOperators = 0 ,0 , 1.0 * lm.TotalCycle / lm.CurrentOperators)
	---- 公式: [Total Cycle Time] / [Highest cycle time of operator in shift] / [Current No of Oprts] * 100
	,[LBR] = IIF( lm.HighestCycle =0 OR lm.CurrentOperators = 0, 0,  1.0 * lm.TotalCycle / lm.HighestCycle / lm.CurrentOperators * 100 )
	---- 公式: [ELOR] × [CPU /PC] / [Oprts after inline]
	--- EOLR公式：3600 / [Highest Cycle Time]
	,[EstPPH] =  IIF(lm.HighestGSD = 0  or lm.CurrentOperators = 0, 0,  (1.0 * 3600 / lm.HighestCycle) * b.CPU / lm.CurrentOperators )
	from #FinalAfterData lm
	where lm.StyleUKey = b.StyleUkey and a.FactoryID=lm.FactoryID and lm.SewingLineID = a.SewingLineID and a.Team=lm.Team and b.ComboType=lm.ComboType and lm.SourceTable = 'IE P06'
)AfterDataP06
Outer Apply(
	select top 1 * ---- 因為產線計畫不會有 OutputDate 的區別，因此都會長得一樣，取Top 1即可
	---- Avg. Cycle 公式: [Total Cycle Time] / [Oprts after inline]
	,[AvgCycle] = IIF(lm.CurrentOperators = 0 ,0 , 1.0 * lm.TotalCycle / lm.CurrentOperators)
	---- 公式: [Total Cycle Time] / [Highest cycle time of operator in shift] / [Current No of Oprts] * 100
	,[LBR] = IIF( lm.HighestCycle =0 OR lm.CurrentOperators = 0, 0,  1.0 * lm.TotalCycle / lm.HighestCycle / lm.CurrentOperators * 100 )
	---- 公式: [ELOR] × [CPU /PC] / [Oprts after inline]
	--- EOLR公式：3600 / [Highest Cycle Time]
	,[EstPPH] =  IIF(lm.HighestGSD = 0  or lm.CurrentOperators = 0, 0,  (1.0 * 3600 / lm.HighestCycle) * b.CPU / lm.CurrentOperators )
	from #AllAfter lm
	where lm.StyleUKey = b.StyleUkey and a.FactoryID=lm.FactoryID and lm.SewingLineID = a.SewingLineID and a.Team=lm.Team and b.ComboType=lm.ComboType AND CAST(lm.EditDate as Date) = a.OutputDate
	order by lm.EditDate desc
)AfterData
outer apply(
	select top 1 ct.Target
	from factory f
	left join ChgOverTarget ct on ct.MDivisionID= f.MDivisionID 
				and ct. Type ='LBR'
	where f.id = a.FactoryID
	order by EffectiveDate desc
)LinebalancingTarget 
outer apply(
	select top 1 *
	from (
		select *
		from (
			select *,SourceTable='P05' from #P05MaxVer
			union
			select *,SourceTable='P06' from #P06MaxVer
		)LastData
		where b.StyleUkey = LastData.StyleUKey 
			and a.FactoryID = LastData.FactoryID 
			and b.ComboType = LastData.ComboType 
	)aa
	ORDER BY 
	  CASE Phase
		WHEN 'Final' THEN 3
		WHEN 'Prelim' THEN 2
		WHEN 'Initial' THEN 1
		ELSE 0
	  END DESC,
	  Version DESC
)LastVersion
WHERE BeforeDataP05.SourceTable IS NOT NULL

drop table #BaseData
,#P03MaxVer
,#P05MaxVer
,#P06MaxVer
,#P03Rank
,#AfterData
,#BeforeData
,#FinalAfterData
,#FinalBeforeData
,#SewingOutput_Detail
,#SewingOutput
,#OrderInfo
,#AllP03
,#AllP05
,#AllP06
,#AllAfter
");

            return cmd;
        }

        #region 控制項事件
        private void TxtBrand_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            var filterData = this.BrandData;

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(filterData, "ID,NameCH,NameEN", "10,20,20", this.txtBrand.Text, false, ",", "ID,NameCH,NameEN")
            {
                Size = new System.Drawing.Size(690, 555),
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtBrand.Text = item.GetSelectedString();
        }

        private void TxtBrand_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtBrand.Text))
            {
                return;
            }

            var filterData = this.BrandData;

            this.ShowWaitMessage("Data searching...");

            var tmp = filterData.AsEnumerable().Where(o => o["ID"].ToString() == this.txtBrand.Text);
            if (tmp == null || tmp.Count() == 0)
            {
                filterData = new DataTable();
                filterData = this.BrandData.Clone();
            }
            else
            {
                filterData = this.BrandData.AsEnumerable().Where(o => o["ID"].ToString() == this.txtBrand.Text).CopyToDataTable();
            }

            this.HideWaitMessage();

            if (filterData == null || filterData.Rows.Count == 0)
            {
                this.txtBrand.Text = string.Empty;
                this.txtSeason.Text = string.Empty;
                this.txtStyle.Text = string.Empty;
                MyUtility.Msg.InfoBox("Brand not found.");
            }
        }

        private void TxtSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            var filterData = this.SeasonData;

            this.ShowWaitMessage("Data searching...");

            // 若先選過Brand則加入篩選條件
            if (!string.IsNullOrEmpty(this.txtBrand.Text))
            {
                filterData = this.SeasonData.AsEnumerable().Where(o => o["BrandID"].ToString() == this.txtBrand.Text).CopyToDataTable();
            }

            filterData = filterData.AsEnumerable().Distinct(DataRowComparer.Default).OrderByDescending(o => o["ID"].ToString()).CopyToDataTable();

            this.HideWaitMessage();

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(filterData, "ID,BrandID", "10,10", this.txtSeason.Text, false, ",", "ID,BrandID ")
            {
                Size = new System.Drawing.Size(690, 555),
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeason.Text = item.GetSelectedString();
        }

        private void TxtSeason_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSeason.Text))
            {
                return;
            }

            var filterData = this.SeasonData;
            this.ShowWaitMessage("Data searching...");

            // 若先選過Brand則加入篩選條件
            if (!string.IsNullOrEmpty(this.txtBrand.Text))
            {
                filterData = this.SeasonData.AsEnumerable().Where(o => o["BrandID"].ToString() == this.txtBrand.Text).CopyToDataTable();
            }

            if (!string.IsNullOrEmpty(this.txtSeason.Text))
            {
                var tmp = filterData.AsEnumerable().Where(o => o["ID"].ToString() == this.txtSeason.Text);
                if (tmp == null || tmp.Count() == 0)
                {
                    filterData = new DataTable();
                    filterData = this.SeasonData.Clone();
                }
                else
                {
                    filterData = tmp.CopyToDataTable();
                }
            }

            this.HideWaitMessage();

            if (filterData == null || filterData.Rows.Count == 0)
            {
                this.txtSeason.Text = string.Empty;
                this.txtStyle.Text = string.Empty;
                MyUtility.Msg.InfoBox("Season not found.");
            }
        }

        private void TxtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            var filterData = this.StyleData;

            this.ShowWaitMessage("Data searching...");

            // 若先選過Brand則加入篩選條件
            if (!string.IsNullOrEmpty(this.txtBrand.Text))
            {
                filterData = filterData.AsEnumerable().Where(o => o["BrandID"].ToString() == this.txtBrand.Text).CopyToDataTable();
            }

            // 若先選過Season則加入篩選條件
            if (!string.IsNullOrEmpty(this.txtSeason.Text))
            {
                filterData = filterData.AsEnumerable().Where(o => o["SeasonID"].ToString() == this.txtSeason.Text).CopyToDataTable();
            }

            filterData = filterData.AsEnumerable().Distinct(DataRowComparer.Default).OrderByDescending(o => o["SeasonID"].ToString()).CopyToDataTable();

            this.HideWaitMessage();

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(filterData, "ID,BrandID,SeasonID,Description", "10,15,15,25", this.txtStyle.Text, false, ",", "ID,BrandID,SeasonID,Description")
            {
                Size = new System.Drawing.Size(750, 555),
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtStyle.Text = item.GetSelectedString();
        }

        private void TxtStyle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtStyle.Text))
            {
                return;
            }

            var filterData = this.StyleData;

            this.ShowWaitMessage("Data searching...");

            // 若先選過Brand則加入篩選條件
            if (!string.IsNullOrEmpty(this.txtBrand.Text))
            {
                filterData = filterData.AsEnumerable().Where(o => o["BrandID"].ToString() == this.txtBrand.Text).CopyToDataTable();
            }

            // 若先選過Season則加入篩選條件
            if (!string.IsNullOrEmpty(this.txtSeason.Text))
            {
                filterData = filterData.AsEnumerable().Where(o => o["SeasonID"].ToString() == this.txtSeason.Text).CopyToDataTable();
            }

            if (!string.IsNullOrEmpty(this.txtStyle.Text))
            {
                var tmp = filterData.AsEnumerable().Where(o => o["ID"].ToString() == this.txtStyle.Text);
                if (tmp == null || tmp.Count() == 0)
                {
                    filterData = new DataTable();
                    filterData = this.StyleData.Clone();
                }
                else
                {
                    filterData = filterData.AsEnumerable().Where(o => o["ID"].ToString() == this.txtStyle.Text).CopyToDataTable();
                }
            }

            this.HideWaitMessage();

            if (filterData == null || filterData.Rows.Count == 0)
            {
                this.txtStyle.Text = string.Empty;
                MyUtility.Msg.InfoBox("Style not found.");
            }
        }

        private void TxtLine_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            var filterData = this.LineData;

            this.ShowWaitMessage("Data searching...");

            // 若先選過FactoryID則加入篩選條件
            if (!string.IsNullOrEmpty(this.comboFty.Text))
            {
                filterData = filterData.AsEnumerable().Where(o => o["FactoryID"].ToString() == this.comboFty.Text).CopyToDataTable();
            }

            filterData = filterData.AsEnumerable().Distinct(DataRowComparer.Default).OrderBy(o => o["ID"].ToString()).CopyToDataTable();

            this.HideWaitMessage();

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(filterData, "ID,FactoryID,Description", "10,10,20", this.txtLine.Text, false, ",", "ID,FactoryID,Description")
            {
                Size = new System.Drawing.Size(690, 555),
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtLine.Text = item.GetSelectedString();
        }

        private void TxtLine_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtLine.Text))
            {
                return;
            }

            var filterData = this.LineData;

            // 若先選過Factory則加入篩選條件
            if (!string.IsNullOrEmpty(this.comboFty.Text))
            {
                filterData = filterData.AsEnumerable().Where(o => o["FactoryID"].ToString() == this.comboFty.Text).CopyToDataTable();
            }

            if (!string.IsNullOrEmpty(this.txtLine.Text))
            {
                var tmp = filterData.AsEnumerable().Where(o => o["ID"].ToString() == this.txtLine.Text);
                if (tmp == null || tmp.Count() == 0)
                {
                    filterData = new DataTable();
                    filterData = this.LineData.Clone();
                }
                else
                {
                    filterData = filterData.AsEnumerable().Where(o => o["ID"].ToString() == this.txtLine.Text).CopyToDataTable();
                }
            }

            if (filterData == null || filterData.Rows.Count == 0)
            {
                this.txtLine.Text = string.Empty;
                MyUtility.Msg.InfoBox("Line not found.");
            }
        }

        private void ComboFty_SelectedValueChanged(object sender, EventArgs e)
        {
            this.txtLine.Text = string.Empty;
        }
        #endregion
    }
}

using Ict;
using Sci.Data;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R22 : Win.Tems.PrintForm
    {
        private DateTime? AuditDate1;
        private DateTime? AuditDate2;
        private System.Data.DataTable printData;

        public R22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateAuditDate.Control2.Enabled = false;
        }

        // radio改變
        private void RadioPerBrand_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioPerBrand.Checked)
            {
                this.dateAuditDate.IsRequired = false;
                this.dateAuditDate.Control2.Text = string.Empty;
                this.dateAuditDate.Control2.Enabled = false;
            }
        }

        private void RadioPerDateFactory_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioPerDateFactory.Checked)
            {
                this.dateAuditDate.IsRequired = true;
                this.dateAuditDate.Control2.Enabled = true;
            }
        }

        private void RadioPerDateBrand_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioPerDateBrand.Checked)
            {
                this.dateAuditDate.IsRequired = true;
                this.dateAuditDate.Control2.Enabled = true;
            }
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.AuditDate1 = this.dateAuditDate.Value1;
            this.AuditDate2 = this.dateAuditDate.Value2;

            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string d1 = string.Empty, d2 = string.Empty;
            if (!MyUtility.Check.Empty(this.AuditDate1))
            {
                d1 = Convert.ToDateTime(this.AuditDate1).ToString("d");
            }

            if (!MyUtility.Check.Empty(this.AuditDate2))
            {
                d2 = Convert.ToDateTime(this.AuditDate2).ToString("d");
            }

            #region radioPerBrand
            if (this.radioPerBrand.Checked)
            {
                #region P1
                sqlCmd.Append(@"
DECLARE @cols NVARCHAR(MAX)= N''
DECLARE @cols2 NVARCHAR(MAX)= N''
DECLARE @cols3 NVARCHAR(MAX)= N''
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
IF OBJECT_ID('tempdb.dbo.#ALL', 'U') IS NOT NULL
  DROP TABLE #All
 IF OBJECT_ID('tempdb.dbo.#C', 'U') IS NOT NULL
  DROP TABLE #C
;with cte as
(
select FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result ,min(AddDate) min_addDate
from dbo.Cfa a WITH (NOLOCK) 
where 1=1 
and a.Status = 'Confirmed' 
");
                #endregion
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(this.AuditDate1))
                {
                    sqlCmd.Append(string.Format(@" and cdate = '{0}'", d1));
                }
                #endregion
                #region P2
                sqlCmd.Append(@"
group by FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result
)

,src as 
(
select cte.FactoryID,o.BrandID
	, cast (count(*) as numeric(15,4)) [1inspTimes]
	, cast (sum(iif(Result = 'P',1,0)) as numeric(15,4)) [2passTimes]
	, cast (ROUND(sum(iif(Result = 'P',1.0,0.0))/count(*), 4) as numeric(15,4)) [3pass_rate]
from cte inner join dbo.orders o WITH (NOLOCK) on o.id = cte.OrderID
group by cte.FactoryID,o.BrandID
)


, _unpivot as
(
	select * from
	SRC
	unpivot (
	total
	for vv  in ( [1inspTimes] ,[2passTimes], [3pass_rate])
	)as pvt 
)

, query as 
(
SELECT
      *
	  ,rtrim(brandid) +'_'+ vv [DynamicColumnName]
      ,rtrim(brandid) + CAST(DENSE_RANK() 
		OVER (PARTITION BY factoryid,brandid ORDER BY factoryid,brandid,vv ASC) AS NVARCHAR) AS [PetNamePivot]
FROM _unpivot
)

select *
into #tmpall
from query

IF not exists(select * from #tmpall)
GOTO AllEnd

SELECT @cols = concat(@cols , ',' , QUOTENAME(DynamicColumnName) , '= iif(max(' , QUOTENAME(DynamicColumnName),')<>0,max(' , QUOTENAME(DynamicColumnName),') ,null)')
FROM 
(
    SELECT DISTINCT DynamicColumnName
    FROM #tmpall 
) t
order by DynamicColumnName
SELECT @cols2 = concat(@cols2 , iif(@cols2 = N'',QUOTENAME(DynamicColumnName),concat( N',' , QUOTENAME(DynamicColumnName))))
FROM 
(
    SELECT DISTINCT DynamicColumnName
    FROM #tmpall 
) t
order by DynamicColumnName
SELECT @cols3 = concat(@cols3 , ',' , QUOTENAME(DynamicColumnName) , iif(DynamicColumnName like '%_rate',concat('= iif(avg(' , QUOTENAME(DynamicColumnName),')<>0, avg(' , QUOTENAME(DynamicColumnName),'),null)') ,concat('= iif(sum(' , QUOTENAME(DynamicColumnName),')<>0,sum(' , QUOTENAME(DynamicColumnName),'),null)')))
FROM 
(
    SELECT DISTINCT DynamicColumnName
    FROM #tmpall 
) t
order by DynamicColumnName

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with cte as
(select FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result ,min(AddDate) min_addDate
from dbo.Cfa a WITH (NOLOCK) 
where 1=1
and a.Status = ''Confirmed'' 
");
                #endregion
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(this.AuditDate1))
                {
                    sqlCmd.Append(string.Format(@" and cdate = ''{0}''", d1));
                }
                #endregion
                #region P3
                sqlCmd.Append(@"
group by FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result
)

,src as 
(select cte.FactoryID,o.BrandID
	, cast (count(*) as numeric(15,4)) [1inspTimes]
	, cast (sum(iif(Result = ''P'',1,0)) as numeric(15,4)) [2passTimes]
	, cast (ROUND(sum(iif(Result = ''P'',1.0,0.0))/count(*), 4) as numeric(15,4)) [3pass_rate]
from cte inner join dbo.orders o WITH (NOLOCK) on o.id = cte.OrderID
group by cte.FactoryID,o.BrandID
)


, _unpivot as
(
	select * from
	SRC
	unpivot (
	total
	for vv  in ( [1inspTimes] ,[2passTimes], [3pass_rate])
	)as pvt 
)

, query as 
(
SELECT
      *
	  ,rtrim(brandid) +''_''+ vv [DynamicColumnName]
      ,rtrim(brandid) + CAST(DENSE_RANK() 
		OVER (PARTITION BY factoryid,brandid ORDER BY factoryid,brandid,vv ASC) AS NVARCHAR) AS [PetNamePivot]
FROM _unpivot
)

select * 
into #All
from query

;with A as(
	select
		  [FactoryID] = FactoryID
		,[sum_inspTimes] = iif(sum([1inspTimes])<>0,sum([1inspTimes]),null)
		,[sum_passTimes] = iif(sum([2passTimes])<>0,sum([2passTimes]),null)
		,[sum_pass_rate] = iif(round(sum([2passTimes])/sum([1inspTimes]),4)<>0,round(sum([2passTimes])/sum([1inspTimes]),4),null)
	from #All
	pivot
	(
		max(total)
			for [vv] in ([1inspTimes],[2passTimes],[3pass_rate])
	) a
	group by FactoryID
)

,B as (
	select 
		 [FactoryB] = FactoryID
		 '+@cols+'
from #All
	pivot
	(
		max(total)
			for [DynamicColumnName] in ('+@cols2+')
) a
	group by FactoryID
) 

select * 
into #C
from A left join B on A.FactoryID = B.FactoryB
order by A.FactoryID

alter table #C drop column [FactoryB]

select *
from #C
union all
(
select 
	[FactoryID] = ''Total'' 
	,sum([sum_inspTimes])
	,sum([sum_passTimes])
	,avg([sum_pass_rate])
	'+@cols3+'
from #C
)
'
EXEC sp_executesql @sql
AllEnd:
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
IF OBJECT_ID('tempdb.dbo.#ALL', 'U') IS NOT NULL
  DROP TABLE #All
 IF OBJECT_ID('tempdb.dbo.#C', 'U') IS NOT NULL
  DROP TABLE #C
");
                #endregion
            }
            #endregion

            #region radioPerDateFactory
            if (this.radioPerDateFactory.Checked)
            {
                #region P1
                sqlCmd.Append(@"
DECLARE @cols NVARCHAR(MAX)= N''
DECLARE @cols2 NVARCHAR(MAX)= N''
DECLARE @cols3 NVARCHAR(MAX)= N''
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
IF OBJECT_ID('tempdb.dbo.#ALL', 'U') IS NOT NULL
  DROP TABLE #All
 IF OBJECT_ID('tempdb.dbo.#C', 'U') IS NOT NULL
  DROP TABLE #C

;with cte as
(
select FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result ,min(AddDate) min_addDate
from dbo.Cfa a WITH (NOLOCK) 
where 1=1 
and a.Status = 'Confirmed' 
");
                #endregion
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(this.AuditDate1))
                {
                    sqlCmd.Append(string.Format(@" and cdate >= '{0}'", d1));
                }

                if (!MyUtility.Check.Empty(this.AuditDate2))
                {
                    sqlCmd.Append(string.Format(@" and cdate <= '{0}'", d2));
                }
                #endregion
                #region P2
                sqlCmd.Append(@"
group by FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result
)

,src as 
(
select cte.CDate, cte.FactoryID
	, cast (count(*) as numeric(15,4)) [1inspTimes]
	, cast (sum(iif(Result = 'P',1,0)) as numeric(15,4)) [2passTimes]
	, cast (ROUND(sum(iif(Result = 'P',1.0,0.0))/count(*), 4) as numeric(15,4)) [3pass_rate]
from cte inner join dbo.orders o WITH (NOLOCK) on o.id = cte.OrderID
group by cte.CDate, cte.FactoryID
)

, _unpivot as
(
	select * from
	SRC
	unpivot (
	total
	for vv  in ( [1inspTimes] ,[2passTimes], [3pass_rate])
	)as pvt 
)

, query as 
(
SELECT
      *
	  ,rtrim(FactoryID) +'_'+ vv [DynamicColumnName]
      ,rtrim(FactoryID) +'_'+ CAST(DENSE_RANK() 
		OVER (PARTITION BY CDate, factoryid ORDER BY CDate, factoryid,vv ASC) AS NVARCHAR) AS [PetNamePivot]
FROM _unpivot
)

select *
into #tmpall
from query

IF not exists(select * from #tmpall)
GOTO AllEnd

SELECT @cols = concat(@cols , ',' , QUOTENAME(DynamicColumnName) , '= iif(max(' , QUOTENAME(DynamicColumnName),')<>0,max(' , QUOTENAME(DynamicColumnName),') ,null)')
FROM 
(
    SELECT DISTINCT DynamicColumnName
    FROM #tmpall 
) t
order by DynamicColumnName
--print @cols
SELECT @cols2 = concat(@cols2 , iif(@cols2 = N'',QUOTENAME(DynamicColumnName),concat( N',' , QUOTENAME(DynamicColumnName))))
FROM 
(
    SELECT DISTINCT DynamicColumnName
    FROM #tmpall 
) t
order by DynamicColumnName
--print @cols2
SELECT @cols3 = concat(@cols3 , ',' , QUOTENAME(DynamicColumnName) , iif(DynamicColumnName like '%_rate',concat('= iif(avg(' , QUOTENAME(DynamicColumnName),')<>0, avg(' , QUOTENAME(DynamicColumnName),'),null)') ,concat('= iif(sum(' , QUOTENAME(DynamicColumnName),')<>0,sum(' , QUOTENAME(DynamicColumnName),'),null)')))
FROM 
(
    SELECT DISTINCT DynamicColumnName
    FROM #tmpall 
) t
order by DynamicColumnName
--print @cols3

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with cte as
(
select FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result ,min(AddDate) min_addDate
from dbo.Cfa a
where 1=1 
and a.Status = ''Confirmed'' 
");
                #endregion
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(this.AuditDate1))
                {
                    sqlCmd.Append(string.Format(@" and cdate >= ''{0}''", d1));
                }

                if (!MyUtility.Check.Empty(this.AuditDate2))
                {
                    sqlCmd.Append(string.Format(@" and cdate <= ''{0}''", d2));
                }
                #endregion
                #region P3
                sqlCmd.Append(@"
group by FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result
)

,src as 
(select cte.CDate, cte.FactoryID
	, cast (count(*) as numeric(15,4)) [1inspTimes]
	, cast (sum(iif(Result = ''P'',1,0)) as numeric(15,4)) [2passTimes]
	, cast (ROUND(sum(iif(Result = ''P'',1.0,0.0))/count(*), 4) as numeric(15,4)) [3pass_rate]
from cte inner join dbo.orders o WITH (NOLOCK) on o.id = cte.OrderID
group by cte.CDate, cte.FactoryID
)

, _unpivot as
(
	select * from
	SRC
	unpivot (
	total
	for vv  in ( [1inspTimes] ,[2passTimes], [3pass_rate])
	)as pvt 
)

, query as 
(
SELECT
      *
	  ,rtrim(FactoryID) +''_''+ vv [DynamicColumnName]
      ,rtrim(FactoryID) +''_''+ CAST(DENSE_RANK() 
		OVER (PARTITION BY CDate, factoryid ORDER BY CDate, factoryid,vv ASC) AS NVARCHAR) AS [PetNamePivot]
FROM _unpivot
)

select *
into #All
from query

;with A as(
	select
		[DateA] = CDate
		,[sum_inspTimes] = iif(sum([1inspTimes])<>0,sum([1inspTimes]),null)
		,[sum_passTimes] = iif(sum([2passTimes])<>0,sum([2passTimes]),null)
		,[sum_pass_rate] = iif(round(sum([2passTimes])/sum([1inspTimes]),4)<>0,round(sum([2passTimes])/sum([1inspTimes]),4),null)
	from #All
	pivot
	(
		max(total)
			for [vv] in ([1inspTimes],[2passTimes],[3pass_rate])
	) a
	group by CDate
)

,B as (
	select 
		[Date] = CDate
		'+@cols+'

	from #All
	pivot
	(
		max(total)
			for [DynamicColumnName] in ('+@cols2+')
	) a
	group by CDate
) 

select * 
into #C
from B left join A on A.DateA = B.[Date]
order by A.DateA

alter table #C drop column [DateA]

select * 
from #C
union all
(
select 
	null
	'+@cols3+'
	,sum([sum_inspTimes])
	,sum([sum_passTimes])
	,avg([sum_pass_rate])
from #C
)
'
EXEC sp_executesql @sql
AllEnd:
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
IF OBJECT_ID('tempdb.dbo.#ALL', 'U') IS NOT NULL
  DROP TABLE #All
 IF OBJECT_ID('tempdb.dbo.#C', 'U') IS NOT NULL
  DROP TABLE #C
");
                #endregion
            }
            #endregion

            #region radioPerDateBrand
            if (this.radioPerDateBrand.Checked)
            {
                #region P1
                sqlCmd.Append(@"
DECLARE @cols NVARCHAR(MAX)= N''
DECLARE @cols2 NVARCHAR(MAX)= N''
DECLARE @cols3 NVARCHAR(MAX)= N''
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
IF OBJECT_ID('tempdb.dbo.#ALL', 'U') IS NOT NULL
  DROP TABLE #All
 IF OBJECT_ID('tempdb.dbo.#C', 'U') IS NOT NULL
  DROP TABLE #C
;with cte as
(
select FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result ,min(AddDate) min_addDate
from dbo.Cfa a WITH (NOLOCK) 
where 1=1 
and a.Status = 'Confirmed' 
");
                #endregion
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(this.AuditDate1))
                {
                    sqlCmd.Append(string.Format(@" and cdate >= '{0}'", d1));
                }

                if (!MyUtility.Check.Empty(this.AuditDate2))
                {
                    sqlCmd.Append(string.Format(@" and cdate <= '{0}'", d2));
                }
                #endregion
                #region P2
                sqlCmd.Append(@"
group by FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result
)

,src as 
(select cte.CDate,o.BrandID
	, cast (count(*) as numeric(15,4)) [1inspTimes]
	, cast (sum(iif(Result = 'P',1,0)) as numeric(15,4)) [2passTimes]
	, cast (ROUND(sum(iif(Result = 'P',1.0,0.0))/count(*), 4) as numeric(15,4)) [3pass_rate]
from cte inner join dbo.orders o WITH (NOLOCK) on o.id = cte.OrderID
group by cte.CDate,o.BrandID
)

, _unpivot as
(
	select * from
	SRC
	unpivot (
	total
	for vv  in ( [1inspTimes] ,[2passTimes], [3pass_rate])
	)as pvt 
)

, query as 
(
SELECT
      *
	  ,rtrim(brandid) +'_'+ vv [DynamicColumnName]
      ,rtrim(brandid) + CAST(DENSE_RANK() 
		OVER (PARTITION BY CDate,brandid ORDER BY CDate,brandid,vv ASC) AS NVARCHAR) AS [PetNamePivot]
FROM _unpivot
)

select *
into #tmpall
from query

IF not exists(select * from #tmpall)
GOTO AllEnd

SELECT @cols = concat(@cols , ',' , QUOTENAME(DynamicColumnName) , '= iif(max(' , QUOTENAME(DynamicColumnName),')<>0,max(' , QUOTENAME(DynamicColumnName),') ,null)')
FROM 
(
    SELECT DISTINCT DynamicColumnName
    FROM #tmpall 
) t
order by DynamicColumnName
--print @cols
SELECT @cols2 = concat(@cols2 , iif(@cols2 = N'',QUOTENAME(DynamicColumnName),concat( N',' , QUOTENAME(DynamicColumnName))))
FROM 
(
    SELECT DISTINCT DynamicColumnName
    FROM #tmpall 
) t
order by DynamicColumnName
--print @cols2
SELECT @cols3 = concat(@cols3 , ',' , QUOTENAME(DynamicColumnName) , iif(DynamicColumnName like '%_rate',concat('= iif(avg(' , QUOTENAME(DynamicColumnName),')<>0, avg(' , QUOTENAME(DynamicColumnName),'),null)') ,concat('= iif(sum(' , QUOTENAME(DynamicColumnName),')<>0,sum(' , QUOTENAME(DynamicColumnName),'),null)')))
FROM 
(
    SELECT DISTINCT DynamicColumnName
    FROM #tmpall 
) t
order by DynamicColumnName
--print @cols3

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with cte as
(
select FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result ,min(AddDate) min_addDate
from dbo.Cfa a WITH (NOLOCK) 
where 1=1 
and a.Status = ''Confirmed'' 
");
                #endregion
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(this.AuditDate1))
                {
                    sqlCmd.Append(string.Format(@" and cdate >= ''{0}''", d1));
                }

                if (!MyUtility.Check.Empty(this.AuditDate2))
                {
                    sqlCmd.Append(string.Format(@" and cdate <= ''{0}''", d2));
                }
                #endregion
                #region P3
                sqlCmd.Append(@"
group by FactoryID, CDate, OrderID, SewingLineID, Shift, a.GarmentOutput,Result
)

,src as 
(select cte.CDate,o.BrandID
	, cast (count(*) as numeric(15,4)) [1inspTimes]
	, cast (sum(iif(Result = ''P'',1,0)) as numeric(15,4)) [2passTimes]
	, cast (ROUND(sum(iif(Result = ''P'',1.0,0.0))/count(*), 4) as numeric(15,4)) [3pass_rate]
from cte inner join dbo.orders o WITH (NOLOCK) on o.id = cte.OrderID
group by cte.CDate,o.BrandID
)

, _unpivot as
(
	select * from
	SRC
	unpivot (
	total
	for vv  in ( [1inspTimes] ,[2passTimes], [3pass_rate])
	)as pvt 
)

, query as 
(
SELECT
      *
	  ,rtrim(brandid) +''_''+ vv [DynamicColumnName]
      ,rtrim(brandid) + CAST(DENSE_RANK() 
		OVER (PARTITION BY CDate,brandid ORDER BY CDate,brandid,vv ASC) AS NVARCHAR) AS [PetNamePivot]
FROM _unpivot
)

select *
into #All
from query

;with A as(
	select
		[DateA] = CDate
		,[sum_inspTimes] = iif(sum([1inspTimes])<>0,sum([1inspTimes]),null)
		,[sum_passTimes] = iif(sum([2passTimes])<>0,sum([2passTimes]),null)
		,[sum_pass_rate] = iif(round(sum([2passTimes])/sum([1inspTimes]),4)<>0,round(sum([2passTimes])/sum([1inspTimes]),4),null)
	from #All
	pivot
	(
		max(total)
			for [vv] in ([1inspTimes],[2passTimes],[3pass_rate])
	) a
	group by CDate
)

,B as (
	select 
		[Date] = CDate
		'+@cols+'

	from #All
	pivot
	(
		max(total)
			for [DynamicColumnName] in ('+@cols2+')
	) a
	group by CDate
) 

select * 
into #C
from B left join A on A.DateA = B.[Date]
order by A.DateA

alter table #C drop column [DateA]

select * 
from #C
union all
(
select 
	null
	'+@cols3+'
	,sum([sum_inspTimes])
	,sum([sum_passTimes])
	,avg([sum_pass_rate])
from #C
)
'
EXEC sp_executesql @sql
AllEnd:
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
IF OBJECT_ID('tempdb.dbo.#ALL', 'U') IS NOT NULL
  DROP TABLE #All
 IF OBJECT_ID('tempdb.dbo.#C', 'U') IS NOT NULL
  DROP TABLE #C
");
                #endregion
            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        protected string IntToExcelcolumn(int cc) // 將數字轉換EXCEL的英文欄位;例27→AA
        {
            int cc1 = 0, cc2 = 0;
            string c3 = string.Empty;

            // 將欄位數轉成英文字元
            if (cc > 26)
            {
                cc1 = cc / 26;
                char c = Convert.ToChar(64 + cc1);
                cc2 = cc % 26;
                char c2 = Convert.ToChar(64 + cc2);
                c3 = c.ToString() + c2.ToString();
            }
            else
            {
                char c2 = Convert.ToChar(64 + cc);
                c3 = c2.ToString();
            }

            return c3;
        }

        // 產生Excel

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // int cc1 = 0, cc2 = 0;
            // int cc = printData.Columns.Count;
            // string c3 = "";
            string d1 = string.Empty, d2 = string.Empty;
            if (!MyUtility.Check.Empty(this.AuditDate1))
            {
                d1 = Convert.ToDateTime(this.AuditDate1).ToString("d");
            }

            if (!MyUtility.Check.Empty(this.AuditDate1))
            {
                d2 = Convert.ToDateTime(this.AuditDate1).ToString("d");
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            #region radioPerBrand
            if (this.radioPerBrand.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_R22_PerBrand.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Quality_R22_PerBrand.xltx", 4, false, null, objApp); // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                objSheets.Cells[2, 2] = "Audit Date:" + d1; // 加入日期條件

                objSheets.get_Range("A1", this.IntToExcelcolumn(this.printData.Columns.Count) + "1").Merge(false); // 合併欄位,要合併到哪個欄位並加入Pass Rate字串
                objSheets.Cells[1, 1] = "Pass Rate"; // 加入Pass Rate

                #region 依據欄位數量,指定合併,框線,底色

                // 第4列,在summary之後要做幾次合併
                for (int i = 1; i < ((this.printData.Columns.Count - 4) / 3) + 1; i++)
                {
                    string s1 = this.IntToExcelcolumn(2 + (i * 3)) + "4";
                    string s2 = this.IntToExcelcolumn(4 + (i * 3)) + "4";
                    string s3 = this.IntToExcelcolumn(4 + (i * 3));
                    objSheets.get_Range(s1, s2).Merge(false); // 合併欄位
                    objSheets.get_Range(s1, s2).Interior.Color = Color.FromArgb(222, 186, 252); // 設定指定儲存格背景色
                    string[] cname = this.printData.Columns[2 + (i * 3)].ToString().Split('_'); // 把欄位名稱以_字元分割
                    objSheets.Cells[4, 2 + (i * 3)] = cname[0]; // 加入品名
                    objSheets.Cells[3, 2 + (i * 3)] = "Total Times PO Inspected";
                    objSheets.Cells[3, 3 + (i * 3)] = "Total Times P.O Passed";
                    objSheets.Cells[3, 4 + (i * 3)] = "PASS RATE";
                    objSheets.get_Range(s1, s2).ColumnWidth = 8; // 設定單一儲存格寬度，因自動調整會很亂
                    objSheets.get_Range(s3 + ":" + s3, Type.Missing).NumberFormat = "0.00%";
                }
                #endregion
                objSheets.get_Range("D" + 5, "D" + (4 + this.printData.Rows.Count)).Interior.Color = Color.FromArgb(204, 255, 102); // summary_PASS RATE儲存格背景色
                string lastright = this.IntToExcelcolumn(this.printData.Columns.Count) + (4 + this.printData.Rows.Count);
                objSheets.get_Range("A" + (4 + this.printData.Rows.Count), lastright).Interior.Color = Color.FromArgb(204, 255, 102); // 最後一列儲存格背景色
                objSheets.get_Range("A3", lastright).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous; // 設定有資料範圍所有框線
                objSheets.get_Range("A3", lastright).EntireRow.AutoFit(); // 自動調整列高

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Quality_R22_PerBrand");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                strExcelName.OpenFile();
                #endregion
            }
            #endregion

            #region radioPerDateFactory
            if (this.radioPerDateFactory.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_R22_PerDate(Factory).xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Quality_R22_PerDate(Factory).xltx", 3, false, null, objApp); // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                #region 依據欄位數量,指定合併,框線,底色

                // 第3列,在date之後要做幾次合併
                for (int i = 1; i < ((this.printData.Columns.Count - 1) / 3) + 1; i++)
                {
                    string s1 = this.IntToExcelcolumn(-1 + (i * 3)) + "3";
                    string s2 = this.IntToExcelcolumn(1 + (i * 3)) + "3";
                    string s3 = this.IntToExcelcolumn(1 + (i * 3));
                    objSheets.get_Range(s1, s2).Merge(false); // 合併欄位
                    objSheets.get_Range(s1, s2).Interior.Color = Color.FromArgb(222, 186, 252); // 設定指定儲存格背景色
                    string[] cname = this.printData.Columns[(i * 3) - 1].ToString().Split('_'); // 把欄位名稱以_字元分割
                    objSheets.Cells[3, (i * 3) - 1] = cname[0]; // 加入group欄位名稱
                    objSheets.Cells[2, -1 + (i * 3)] = "Total Times PO Inspected";
                    objSheets.Cells[2, i * 3] = "Total Times P.O Passed";
                    objSheets.Cells[2, 1 + (i * 3)] = "PASS RATE";
                    objSheets.get_Range(s1, s2).ColumnWidth = 8; // 設定單一儲存格寬度，因自動調整會很亂
                    objSheets.get_Range(s3 + ":" + s3, Type.Missing).NumberFormat = "0.00%";
                }
                #endregion
                string right = this.IntToExcelcolumn(this.printData.Columns.Count);
                string lastright = this.IntToExcelcolumn(this.printData.Columns.Count) + (3 + this.printData.Rows.Count);

                objSheets.Cells[this.printData.Rows.Count + 3, 1] = "Total";

                objSheets.get_Range(this.IntToExcelcolumn(this.printData.Columns.Count - 2) + 3, right + 3).Interior.Color = Color.FromArgb(204, 255, 102); // summary儲存格背景色

                objSheets.get_Range(right + 4, right + (this.printData.Rows.Count + 3)).Interior.Color = Color.FromArgb(204, 255, 102); // summary_PASS RATE儲存格背景色
                objSheets.get_Range("A" + (3 + this.printData.Rows.Count), lastright).Interior.Color = Color.FromArgb(204, 255, 102); // 最後一列儲存格背景色
                objSheets.get_Range("A2", lastright).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous; // 設定有資料範圍所有框線
                objSheets.get_Range("A2", lastright).EntireRow.AutoFit(); // 自動調整列高
                objSheets.Columns.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Quality_R22_PerDate(Factory)");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                strExcelName.OpenFile();
                #endregion
            }
            #endregion

            #region radioPerDateBrand
            if (this.radioPerDateBrand.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_R22_PerDate(Brand).xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Quality_R22_PerDate(Brand).xltx", 3, false, null, objApp); // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                #region 依據欄位數量,指定合併,框線,底色
                for (int i = 1; i < ((this.printData.Columns.Count - 1) / 3) + 1; i++) // 第3列,在date之後要做幾次合併
                {
                    string s1 = this.IntToExcelcolumn(-1 + (i * 3)) + "3";
                    string s2 = this.IntToExcelcolumn(1 + (i * 3)) + "3";
                    string s3 = this.IntToExcelcolumn(1 + (i * 3));
                    objSheets.get_Range(s1, s2).Merge(false); // 合併欄位
                    objSheets.get_Range(s1, s2).Interior.Color = Color.FromArgb(222, 186, 252); // 設定指定儲存格背景色
                    string[] cname = this.printData.Columns[(i * 3) - 1].ToString().Split('_'); // 把欄位名稱以_字元分割
                    objSheets.Cells[3, (i * 3) - 1] = cname[0]; // 加入group欄位名稱
                    objSheets.Cells[2, -1 + (i * 3)] = "Total Times PO Inspected";
                    objSheets.Cells[2, i * 3] = "Total Times P.O Passed";
                    objSheets.Cells[2, 1 + (i * 3)] = "PASS RATE";
                    objSheets.get_Range(s1, s2).ColumnWidth = 8; // 設定單一儲存格寬度，因自動調整會很亂
                    objSheets.get_Range(s3 + ":" + s3, Type.Missing).NumberFormat = "0.00%";
                }
                #endregion
                string right = this.IntToExcelcolumn(this.printData.Columns.Count);
                string lastright = this.IntToExcelcolumn(this.printData.Columns.Count) + (3 + this.printData.Rows.Count);

                objSheets.Cells[this.printData.Rows.Count + 3, 1] = "Total";

                objSheets.get_Range(this.IntToExcelcolumn(this.printData.Columns.Count - 2) + 3, right + 3).Interior.Color = Color.FromArgb(204, 255, 102); // summary儲存格背景色

                objSheets.get_Range(right + 4, right + (this.printData.Rows.Count + 3)).Interior.Color = Color.FromArgb(204, 255, 102); // summary_PASS RATE儲存格背景色
                objSheets.get_Range("A" + (3 + this.printData.Rows.Count), lastright).Interior.Color = Color.FromArgb(204, 255, 102); // 最後一列儲存格背景色
                objSheets.get_Range("A2", lastright).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous; // 設定有資料範圍所有框線
                objSheets.get_Range("A2", lastright).EntireRow.AutoFit(); // 自動調整列高

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Quality_R22_PerDate(Brand)");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                strExcelName.OpenFile();
                #endregion
            }
            #endregion

            return true;
        }
    }
}

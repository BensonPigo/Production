using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R20 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        DateTime? Period1, Period2;
        string Factory, Brand, Line, Cell, DefectCode, DefectType;

        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            DataTable factory;
            InitializeComponent();
            DBProxy.Current.Select(null, "select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(ComboFactory, 1, factory);
            ComboFactory.Text = Sci.Env.User.Keyword;            
        }

        private void radioPerLine_CheckedChanged(object sender, EventArgs e)
        {
            if (radioPerLine.Checked)
            {
                txtDefectCode.Text = txtDefectType.Text = "";
                txtDefectCode.Enabled = txtDefectType.Enabled = false;
            }
        }

        private void radioPerCell_CheckedChanged(object sender, EventArgs e)
        {
            if (radioPerCell.Checked)
            {
                txtDefectCode.Text = txtDefectType.Text = "";
                txtDefectCode.Enabled = txtDefectType.Enabled = false;
            }
        }

        private void radioAllData_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAllData.Checked)
            {
                txtDefectCode.Text = txtDefectType.Text = "";
                txtDefectCode.Enabled = txtDefectType.Enabled = false;
            }
        }

        private void radioDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDetail.Checked)
            {
                txtDefectCode.Enabled = txtDefectType.Enabled = true;
            }
        }

        private void radioSummybySP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSummybySP.Checked)
            {
                txtDefectCode.Enabled = txtDefectType.Enabled = true;
            }
        }

        private void radioSummybyStyle_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSummybyStyle.Checked)
            {
                txtDefectCode.Enabled = txtDefectType.Enabled = true;
            }
        }

        private void radioSummybyDateandStyle_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSummybyDateandStyle.Checked)
            {
                txtDefectCode.Enabled = txtDefectType.Enabled = true;
            }
        }        

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            Period1 = datePeriod.Value1;
            Period2 = datePeriod.Value2;
            Factory = ComboFactory.Text;
            Brand = txtBrand.Text;
            Line = txtLine.Text;
            Cell = txtCell.Text;
            DefectCode = txtDefectCode.Text;
            DefectType = txtDefectType.Text;

            return base.ValidateInput();
        }

        //非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string d1 = "", d2 = "";
            if (!MyUtility.Check.Empty(Period1))
            {
                d1 = Convert.ToDateTime(Period1).ToString("d");
            }
            if (!MyUtility.Check.Empty(Period2))
            {
                d2 = Convert.ToDateTime(Period2).ToString("d");
            }

            #region radiobtn_PerLine
            if (radioPerLine.Checked)
            {

                string sqlWhere="";
                List<string> sqlList = new List<string>();
                  #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlList.Add(string.Format(" CDate >= '{0}' ", Convert.ToDateTime(Period1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(Period2))
                {
                    sqlList.Add(string.Format(" CDate <= '{0}' ", Convert.ToDateTime(Period2).ToString("d")));
                }
                sqlWhere = string.Join(" and ",sqlList);
                if (!MyUtility.Check.Empty(sqlWhere))
                {
                    sqlWhere = " and " + sqlWhere;
                }
                #endregion
                sqlCmd.Append(@"
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(CDate),N',' + QUOTENAME(CDate))
FROM 
(
    SELECT DISTINCT(CDate) 
    FROM RFT WITH (NOLOCK) 
) t
WHERE 1=1" + sqlWhere+
           @"
order by CDate
DECLARE @cols2 NVARCHAR(MAX)= N''
SELECT @cols2 = @cols2  + iif( @cols2 = N'',N'isnull('+QUOTENAME(CDate)+ N',0) '+QUOTENAME(CDate),N',' +N'isnull('+ QUOTENAME(CDate)+ N',0) '+QUOTENAME(CDate))
FROM 
(
    SELECT DISTINCT(CDate) 
    FROM RFT WITH (NOLOCK) 
) t
WHERE 1=1" + sqlWhere);
              
                sqlCmd.Append(@"
order by CDate

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
IF OBJECT_ID(''tempdb.dbo.#tmpall'', ''U'') IS NOT NULL
  DROP TABLE #tmpall
Select
	[Factory] = A.FACTORYID,
	[Line] = A.SEWINGLINEID,
	[CDate] = A.CDATE,
	[RFT] = Round(sum(Round(( A.InspectQty - A.RejectQty ) / A.InspectQty * 100,2))/count(*),2)
into #tmpall
from RFT A WITH (NOLOCK) 
INNER JOIN DBO.ORDERS C WITH (NOLOCK) ON C.ID = A.OrderID
WHERE 1=1 AND A.InspectQty<>0 
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate >= ''{0}'' ", Convert.ToDateTime(Period1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(Period2))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate <= ''{0}'' ", Convert.ToDateTime(Period2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(Factory))
                {
                    sqlCmd.Append(string.Format(" AND A.FactoryID = ''{0}''", Factory));
                }

                if (!MyUtility.Check.Empty(Brand))
                {
                    sqlCmd.Append(string.Format(" AND C.BrandID = ''{0}''", Brand));
                }

                if (!MyUtility.Check.Empty(Line))
                {
                    sqlCmd.Append(string.Format(" AND A.SewinglineID = ''{0}''", Line));
                }

                if (!MyUtility.Check.Empty(Cell))
                {
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WITH (NOLOCK) WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = ''0{0}'' ", Cell));
                }
                #endregion

                sqlCmd.Append(@"
group by  A.FACTORYID,A.SEWINGLINEID,A.CDATE

Order by [Factory], [Line],[CDate]'

if @cols = '' or @cols is null
	set @sql += '
select *
into #tmpnn
from #tmpall as S
'
else 
	set @sql += '
select *
into #tmpnn
from #tmpall as S
pivot(
  AVG(RFT)
  for [CDate] in ('+@cols+')
) as X
'

if @cols2 = '' or @cols2 is null
	set @sql += '
select  [Factory]
		, [Line]
from #tmpnn
'
else
	set @sql += '
select	[Factory]
		, [Line]
		,'+@cols2+'
from #tmpnn
'

set @sql += 'drop table #tmpall,#tmpnn'

EXEC sp_executesql @sql
");
            }
            #endregion

            #region radiobtn_PerCell
            if (radioPerCell.Checked)
            {
                string sqlWhere = "";
                List<string> sqlList = new List<string>();
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlList.Add(string.Format(" CDate >= '{0}' ", Convert.ToDateTime(Period1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(Period2))
                {
                    sqlList.Add(string.Format(" CDate <= '{0}' ", Convert.ToDateTime(Period2).ToString("d")));
                }
                sqlWhere = string.Join(" and ", sqlList);
                if (!MyUtility.Check.Empty(sqlWhere))
                {
                    sqlWhere = " and " + sqlWhere;
                }
                #endregion
                sqlCmd.Append(@"
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(CDate),N',' + QUOTENAME(CDate))
FROM 
(
    SELECT DISTINCT(CDate) 
    FROM RFT WITH (NOLOCK) 
) t
WHERE 1=1" + sqlWhere +
           @"
order by CDate
DECLARE @cols2 NVARCHAR(MAX)= N''
SELECT @cols2 = @cols2  + iif( @cols2 = N'',N'isnull('+QUOTENAME(CDate)+ N',0) '+QUOTENAME(CDate),N',' +N'isnull('+ QUOTENAME(CDate)+ N',0) '+QUOTENAME(CDate))
FROM 
(
    SELECT DISTINCT(CDate) 
    FROM RFT WITH (NOLOCK) 
) t
WHERE 1=1" + sqlWhere);

                sqlCmd.Append(@"
order by CDate

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
IF OBJECT_ID(''tempdb.dbo.#tmpall'', ''U'') IS NOT NULL
  DROP TABLE #tmpall
Select
	[Factory] = A.FACTORYID,
	[Cell] = SewingCell.SewingCell,
	[CDate] = A.CDATE,
	[RFT] = Round(sum(Round(( A.InspectQty - A.RejectQty ) / A.InspectQty * 100,2))/count(*),2)
into #tmpall
from RFT A WITH (NOLOCK) 
INNER JOIN DBO.ORDERS C ON C.ID = A.OrderID
Outer Apply(
	SELECT SewingCell 
	FROM SewingLine WITH (NOLOCK) 
	WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID
) as SewingCell
WHERE 1=1 AND A.InspectQty<>0 
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate >= ''{0}'' ", Convert.ToDateTime(Period1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(Period2))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate <= ''{0}'' ", Convert.ToDateTime(Period2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(Factory))
                {
                    sqlCmd.Append(string.Format(" AND A.FactoryID = ''{0}''", Factory));
                }

                if (!MyUtility.Check.Empty(Brand))
                {
                    sqlCmd.Append(string.Format(" AND C.BrandID = ''{0}''", Brand));
                }

                if (!MyUtility.Check.Empty(Line))
                {
                    sqlCmd.Append(string.Format(" AND A.SewinglineID = ''{0}''", Line));
                }

                if (!MyUtility.Check.Empty(Cell))
                {
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WITH (NOLOCK) WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = ''0{0}'' ", Cell));
                }
                #endregion

                sqlCmd.Append(@"
group by  A.FACTORYID,SewingCell.SewingCell,A.CDATE

Order by [Factory], [Cell],[CDate]

select *
into #tmpnn
from #tmpall as S
pivot(
  AVG(RFT)
  for [CDate] in ('+@cols+')
) as X

select Factory,Cell,'+@cols2+'
from #tmpnn

drop table #tmpall,#tmpnn'
EXEC sp_executesql @sql
");
            }
            #endregion

            #region radiobtn_AllData
            if (radioAllData.Checked)
            {
                sqlCmd.Append(@"
select
	[Factory] = A.FACTORYID,
	[CDate] = A.CDATE,
	[OrderID] = A.ORDERID,
	[Brand] = C.BRANDID,
	[Style] = C.STYLEID,
	[CDCode] = C.CDCODEID,
	[Team] = A.TEAM,
	[Shift] = A.SHIFT,
	[Line] = A.SEWINGLINEID,
	[Cell] = E.SewingCell,
	[InspectQty] = A.INSPECTQTY,
	[RejectQty] = A.REJECTQTY,
	[RFT (%)] = iif(isnull(a.InspectQty,0)=0,0,round((a.InspectQty-a.RejectQty)/a.InspectQty * 100,2)),
	[Over] = A.Status,
	[QC] = D.CpuRate * C.CPU * A.RejectQty 
    , [Remark] = A.Remark
From DBO.Rft A WITH (NOLOCK) 
INNER JOIN DBO.ORDERS C ON C.ID = A.OrderID
OUTER APPLY DBO.GetCPURate(C.OrderTypeID,C.ProgramID,C.Category,C.BrandID,'O')D
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine WITH (NOLOCK) 
	WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID
) E

WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate >= '{0}' ", Convert.ToDateTime(Period1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(Period2))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate <= '{0}' ", Convert.ToDateTime(Period2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(Factory))
                {
                    sqlCmd.Append(string.Format(" and A.FactoryID = '{0}'", Factory));
                }

                if (!MyUtility.Check.Empty(Brand))
                {
                    sqlCmd.Append(string.Format(" and C.BrandID = '{0}'", Brand));
                }

                if (!MyUtility.Check.Empty(Line))
                {
                    sqlCmd.Append(string.Format(" and A.SewinglineID = '{0}'", Line));
                }

                if (!MyUtility.Check.Empty(Cell))
                {
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WITH (NOLOCK) WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = '0{0}' ", Cell));
                }
                #endregion

                sqlCmd.Append(@"
Order by [Factory], [CDate], [OrderID]
");
            }
            #endregion            

            #region radioBtn_Detail
            if (radioDetail.Checked)
            {
                sqlCmd.Append(@"
select
	[Factory] = A.FACTORYID,
	[CDate] = A.CDATE,
	[OrderID] = A.ORDERID,
	[Brand] = C.BRANDID,
	[Style] = C.STYLEID,
	[Team] = A.TEAM,
	[Shift] = Case when A.SHIFT = 'D' then 'Day' 
				   when A.SHIFT = 'N' then 'NIGHT' 
				   when A.SHIFT = 'O ' then 'SUBCON OUT' 
				   when A.SHIFT = 'I ' then 'SUBCON IN' 
				   else null End,
	[Line] = A.SEWINGLINEID,
	[Cell] = E.SewingCell,
	[InspectQty] = A.INSPECTQTY,
	[RejectQty] = A.REJECTQTY,
	[RFT (%)] = iif(isnull(a.InspectQty,0)=0,0,round((a.InspectQty-a.RejectQty)/a.InspectQty * 100,2)),
	[Over] = A.Status,
	[Defaect Kind] = B.GarmentDefectTypeid,
	[Defaect code] = B.GarmentDefectCodeID,
	[Description] = F.Description,
	[Qty] = B.Qty,
	[QC] = D.CpuRate * C.CPU * A.RejectQty 
    , [Remark] = A.Remark
From DBO.Rft A WITH (NOLOCK) 
INNER JOIN DBO.Rft_Detail B WITH (NOLOCK) ON B.ID = A.ID
INNER JOIN DBO.ORDERS C WITH (NOLOCK) ON C.ID = A.OrderID
OUTER APPLY DBO.GetCPURate(C.OrderTypeID,C.ProgramID,C.Category,C.BrandID,'O')D
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine WITH (NOLOCK) 
	WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID
) E
Outer Apply (
	select g.Description 
	from dbo.GarmentDefectCode g WITH (NOLOCK) 
	where id = B.GarmentDefectCodeID
) F

WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate >= '{0}' ", Convert.ToDateTime(Period1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(Period2))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate <= '{0}' ", Convert.ToDateTime(Period2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(Factory))
                {
                    sqlCmd.Append(string.Format(" and A.FactoryID = '{0}'", Factory));
                }

                if (!MyUtility.Check.Empty(Brand))
                {
                    sqlCmd.Append(string.Format(" and C.BrandID = '{0}'", Brand));
                }

                if (!MyUtility.Check.Empty(Line))
                {
                    sqlCmd.Append(string.Format(" and A.SewinglineID = '{0}'", Line));
                }

                if (!MyUtility.Check.Empty(Cell))
                {
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WITH (NOLOCK) WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = '0{0}' ", Cell));
                }

                if (!MyUtility.Check.Empty(DefectCode))
                {
                    sqlCmd.Append(string.Format(" AND B.GarmentDefectCodeID = '{0}' ", DefectCode));
                }

                if (!MyUtility.Check.Empty(DefectType))
                {
                    sqlCmd.Append(string.Format(" AND B.GarmentDefectTypeid = '{0}' ", DefectType));
                }
                #endregion

                sqlCmd.Append(@"
Order by [Factory], [CDate], [OrderID], [Defaect Kind], [Defaect code]
");
            }
            #endregion

            #region radioBtn_SummybySP
            if (radioSummybySP.Checked)
            {
                sqlCmd.Append(@"
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
select
	[Factory] = A.FACTORYID,
	[OrderID] = A.ORDERID
into #tmpall
From DBO.Rft A WITH (NOLOCK) 

WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate >= '{0}' ", Convert.ToDateTime(Period1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(Period2))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate <= '{0}' ", Convert.ToDateTime(Period2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(Factory))
                {
                    sqlCmd.Append(string.Format(" and A.FactoryID = '{0}'", Factory));
                }
                
                if (!MyUtility.Check.Empty(Line))
                {
                    sqlCmd.Append(string.Format(" and A.SewinglineID = '{0}'", Line));
                }
                #endregion

                sqlCmd.Append(@"
group by A.FACTORYID,A.ORDERID
Order by [Factory], [OrderID]

select
	[Factory] = Z.Factory,
	[OrderID] = Z.OrderID,
	[Brand] = C.BRANDID,
	[Style] = C.STYLEID,
	[Team] = A.TEAM,
	[Shift] = Case when A.SHIFT = 'D' then 'Day' 
				   when A.SHIFT = 'N' then 'NIGHT' 
				   when A.SHIFT = 'O ' then 'SUBCON OUT' 
				   when A.SHIFT = 'I ' then 'SUBCON IN' 
				   else null End,
	[Line] = A.SEWINGLINEID,
	[Cell] = E.SewingCell,
	[InspectQty] = A.INSPECTQTY,
	[RejectQty] = A.REJECTQTY,
	[RFT (%)] = iif(isnull(a.InspectQty,0)=0,0,round((a.InspectQty-a.RejectQty)/a.InspectQty * 100,2)),
	[Defaect Kind] = B.GarmentDefectTypeid,
	[Defaect code] = B.GarmentDefectCodeID,
	[Description] = F.Description,
	[Qty] = B.Qty
from #tmpall as Z
Inner Join DBO.Rft A WITH (NOLOCK) on Z.OrderID = A.OrderID
Inner Join DBO.Rft_Detail B WITH (NOLOCK) ON B.ID = A.ID
Inner Join DBO.ORDERS C WITH (NOLOCK) ON C.ID = Z.OrderID
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine WITH (NOLOCK) 
	WHERE FactoryID = Z.Factory AND ID = A.SewinglineID
) E
Outer Apply (
	select g.Description 
	from dbo.GarmentDefectCode g WITH (NOLOCK) 
	where id = B.GarmentDefectCodeID
) F

WHERE 1=1
");

                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Brand))
                {
                    sqlCmd.Append(string.Format(" and C.BrandID = '{0}'", Brand));
                }


                if (!MyUtility.Check.Empty(Cell))
                {
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WITH (NOLOCK) WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = '0{0}' ", Cell));
                }

                if (!MyUtility.Check.Empty(DefectCode))
                {
                    sqlCmd.Append(string.Format(" AND B.GarmentDefectCodeID = '{0}' ", DefectCode));
                }

                if (!MyUtility.Check.Empty(DefectType))
                {
                    sqlCmd.Append(string.Format(" AND B.GarmentDefectTypeid = '{0}' ", DefectType));
                }
                #endregion

                sqlCmd.Append(@"
Order by [Factory], [OrderID]

drop table #tmpall
");
            }
            #endregion

            #region radiobtn_SummybyStyle
            if (radioSummybyStyle.Checked)
            {
                sqlCmd.Append(@"
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
select
	[Factory] = A.FACTORYID,
	[Brand] = C.BRANDID,
	[Style] = C.STYLEID
into #tmpall
From DBO.Rft A WITH (NOLOCK) 
INNER JOIN DBO.ORDERS C WITH (NOLOCK) ON C.ID = A.OrderID
WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate >= '{0}' ", Convert.ToDateTime(Period1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(Period2))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate <= '{0}' ", Convert.ToDateTime(Period2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(Factory))
                {
                    sqlCmd.Append(string.Format(" and A.FactoryID = '{0}'", Factory));
                }

                if (!MyUtility.Check.Empty(Line))
                {
                    sqlCmd.Append(string.Format(" and A.SewinglineID = '{0}'", Line));
                }

                if (!MyUtility.Check.Empty(Brand))
                {
                    sqlCmd.Append(string.Format(" and C.BrandID = '{0}'", Brand));
                }
                #endregion

                sqlCmd.Append(@"
group by A.FACTORYID, C.BRANDID, C.STYLEID
Order by [Factory], [Style]

select
	[Factory] = Z.Factory,
	[Brand] = Z.Brand,
	[Style] = Z.Style,
	[Team] = A.TEAM,
	[Shift] = Case when A.SHIFT = 'D' then 'Day' 
				   when A.SHIFT = 'N' then 'NIGHT' 
				   when A.SHIFT = 'O ' then 'SUBCON OUT' 
				   when A.SHIFT = 'I ' then 'SUBCON IN' 
				   else null End,
	[Line] = A.SEWINGLINEID,
	[Cell] = E.SewingCell,
	[InspectQty] = A.INSPECTQTY,
	[RejectQty] = A.REJECTQTY,
	[RFT (%)] = iif(isnull(a.InspectQty,0)=0,0,round((a.InspectQty-a.RejectQty)/a.InspectQty * 100,2)),
	[Defaect Kind] = B.GarmentDefectTypeid,
	[Defaect code] = B.GarmentDefectCodeID,
	[Description] = F.Description,
	[Qty] = B.Qty
from #tmpall as Z
Inner Join DBO.Rft A WITH (NOLOCK) on Z.Factory = A.FactoryID
Inner Join DBO.Rft_Detail B WITH (NOLOCK) ON B.ID = A.ID
Inner Join DBO.ORDERS C WITH (NOLOCK) ON C.ID = A.OrderID and Z.Style = C.STYLEID
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine WITH (NOLOCK) 
	WHERE FactoryID = Z.Factory AND ID = A.SewinglineID
) E
Outer Apply (
	select g.Description 
	from dbo.GarmentDefectCode g WITH (NOLOCK) 
	where id = B.GarmentDefectCodeID
) F

WHERE 1=1
");

                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Cell))
                {
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WITH (NOLOCK) WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = '0{0}' ", Cell));
                }

                if (!MyUtility.Check.Empty(DefectCode))
                {
                    sqlCmd.Append(string.Format(" AND B.GarmentDefectCodeID = '{0}' ", DefectCode));
                }

                if (!MyUtility.Check.Empty(DefectType))
                {
                    sqlCmd.Append(string.Format(" AND B.GarmentDefectTypeid = '{0}' ", DefectType));
                }
                #endregion

                sqlCmd.Append(@"
Order by [Factory], [Style]

drop table #tmpall
");
            }
            #endregion

            #region radiobtn_SummybyDateandStyle
            if (radioSummybyDateandStyle.Checked)
            {
                sqlCmd.Append(@"
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
select
	[Factory] = A.FACTORYID,
	[CDate] = A.CDATE,
	[Brand] = C.BRANDID,
	[Style] = C.STYLEID
into #tmpall
From DBO.Rft A WITH (NOLOCK) 
INNER JOIN DBO.ORDERS C WITH (NOLOCK) ON C.ID = A.OrderID
WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate >= '{0}' ", Convert.ToDateTime(Period1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(Period2))
                {
                    sqlCmd.Append(string.Format(@" and A.CDate <= '{0}' ", Convert.ToDateTime(Period2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(Factory))
                {
                    sqlCmd.Append(string.Format(" and A.FactoryID = '{0}'", Factory));
                }

                if (!MyUtility.Check.Empty(Line))
                {
                    sqlCmd.Append(string.Format(" and A.SewinglineID = '{0}'", Line));
                }

                if (!MyUtility.Check.Empty(Brand))
                {
                    sqlCmd.Append(string.Format(" and C.BrandID = '{0}'", Brand));
                }
                #endregion

                sqlCmd.Append(@"
group by A.FACTORYID, A.CDATE, C.BRANDID, C.STYLEID
Order by [Factory],[CDate],[Style]

select
	[Factory] = Z.Factory,
	[CDate] = Z.CDATE,
	[Brand] = Z.Brand,
	[Style] = Z.Style,
	[Team] = A.TEAM,
	[Shift] = Case when A.SHIFT = 'D' then 'Day' 
				   when A.SHIFT = 'N' then 'NIGHT' 
				   when A.SHIFT = 'O ' then 'SUBCON OUT' 
				   when A.SHIFT = 'I ' then 'SUBCON IN' 
				   else null End,
	[Line] = A.SEWINGLINEID,
	[Cell] = E.SewingCell,
	[InspectQty] = A.INSPECTQTY,
	[RejectQty] = A.REJECTQTY,
	[RFT (%)] = iif(isnull(a.InspectQty,0)=0,0,round((a.InspectQty-a.RejectQty)/a.InspectQty * 100,2)),
	[Defaect Kind] = B.GarmentDefectTypeid,
	[Defaect code] = B.GarmentDefectCodeID,
	[Description] = F.Description,
	[Qty] = B.Qty
from #tmpall as Z
Inner Join DBO.Rft A WITH (NOLOCK) on Z.Factory = A.FactoryID
Inner Join DBO.Rft_Detail B WITH (NOLOCK) ON B.ID = A.ID
Inner Join DBO.ORDERS C WITH (NOLOCK) ON C.ID = A.OrderID and Z.Style = C.STYLEID
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine WITH (NOLOCK) 
	WHERE FactoryID = Z.Factory AND ID = A.SewinglineID
) E
Outer Apply (
	select g.Description 
	from dbo.GarmentDefectCode g WITH (NOLOCK) 
	where id = B.GarmentDefectCodeID
) F

WHERE 1=1
");

                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Cell))
                {
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WITH (NOLOCK) WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = '0{0}' ", Cell));
                }

                if (!MyUtility.Check.Empty(DefectCode))
                {
                    sqlCmd.Append(string.Format(" AND B.GarmentDefectCodeID = '{0}' ", DefectCode));
                }

                if (!MyUtility.Check.Empty(DefectType))
                {
                    sqlCmd.Append(string.Format(" AND B.GarmentDefectTypeid = '{0}' ", DefectType));
                }
                #endregion

                sqlCmd.Append(@"
Order by [Factory], [Style]

drop table #tmpall
");
            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            #region radiobtn_PerLine
            if (radioPerLine.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_PerLine.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_PerLine.xltx", 1, false, null, objApp);// 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                for (int i = 2; i < printData.Columns.Count; i++)
                {
                    objSheets.Cells[1, i+1] = printData.Columns[i].ColumnName.ToString();
                }

                objSheets.Columns.AutoFit();
                objSheets.Rows.AutoFit();

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_R20_PerLine");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                strExcelName.OpenFile();
                #endregion
            }
            #endregion

            #region radiobtn_PerCell
            if (radioPerCell.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_PerCell.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_PerCell.xltx", 1, false, null, objApp);// 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                for (int i = 2; i < printData.Columns.Count; i++)
                {
                    objSheets.Cells[1, i + 1] = printData.Columns[i].ColumnName.ToString();
                }

                objSheets.Columns.AutoFit();
                objSheets.Rows.AutoFit();

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_R20_PerCell");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                strExcelName.OpenFile();
                #endregion
            }
            #endregion

            #region radiobtn_AllData
            if (radioAllData.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_AllData.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_AllData.xltx", 1, false, null, objApp);// 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                int count = printData.Rows.Count;
                objSheets.Cells[count + 3, 11] = "Total RFT (%):";
                objSheets.Cells[count + 3, 12] = string.Format(@"=ROUND((SUM(K2:K{0})-SUM(L2:L{0}))/SUM(K2:K{0})*100,2) &"" %""", count + 1);

                objSheets.Cells[count + 3, 14] = "Total QC:";
                objSheets.Cells[count + 3, 15] = string.Format(@"=SUM(O2:O{0})", count + 1);
                //objSheets.get_Range(string.Format("L:L{0}", count + 3), Type.Missing).NumberFormat = "0.00%";
                
                objApp.Cells.EntireColumn.AutoFit();    //自動欄寬
                objApp.Cells.EntireRow.AutoFit();       ////自動欄高

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_R20_AllData");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                strExcelName.OpenFile();
                #endregion
            }
            #endregion

            #region radioBtn_Detail
            if (radioDetail.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_Detail.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_Detail.xltx", 1, true, null, objApp);// 將datatable copy to excel
            }
            #endregion

            #region radioBtn_SummybySP
            if (radioSummybySP.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_SummarybySP.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_SummarybySP.xltx", 1, true, null, objApp);// 將datatable copy to excel
            }
            #endregion

            #region radiobtn_SummybyStyle
            if (radioSummybyStyle.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_SummarybyStyle.xltx"); //預先開啟excel app
                 MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_SummarybyStyle.xltx", 1, true, null, objApp);// 將datatable copy to excel
            }
            #endregion

            #region radiobtn_SummybyDateandStyle
            if (radioSummybyDateandStyle.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_SummarybyDateaAndStyle.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_SummarybyDateaAndStyle.xltx", 1, true, null, objApp);// 將datatable copy to excel
            }
            #endregion

            return true;
        }

        private void ComboFactory_TextChanged(object sender, EventArgs e)
        {
            this.txtBrand.Text = "";
            this.txtLine.Text = "";
            this.txtCell.Text = "";
        }
    }
}

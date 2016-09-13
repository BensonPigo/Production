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
            DBProxy.Current.Select(null, "select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(ComboFactory, 1, factory);
            ComboFactory.Text = Sci.Env.User.Keyword;
        }

        private void radiobtn_PerLine_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_PerLine.Checked)
            {
                txtDefectCode.Text = txtDefectType.Text = "";
                txtDefectCode.Enabled = txtDefectType.Enabled = false;
            }
        }

        private void radiobtn_PerCell_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_PerCell.Checked)
            {
                txtDefectCode.Text = txtDefectType.Text = "";
                txtDefectCode.Enabled = txtDefectType.Enabled = false;
            }
        }

        private void radiobtn_AllData_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_AllData.Checked)
            {
                txtDefectCode.Text = txtDefectType.Text = "";
                txtDefectCode.Enabled = txtDefectType.Enabled = false;
            }
        }

        private void radioBtn_Detail_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtn_Detail.Checked)
            {
                txtDefectCode.Enabled = txtDefectType.Enabled = true;
            }
        }
        
        private void radioBtn_SummybySP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtn_SummybySP.Checked)
            {
                txtDefectCode.Enabled = txtDefectType.Enabled = true;
            }
        }

        private void radiobtn_SummybyStyle_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_SummybyStyle.Checked)
            {
                txtDefectCode.Enabled = txtDefectType.Enabled = true;
            }
        }

        private void radiobtn_SummybyDateandStyle_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_SummybyDateandStyle.Checked)
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
            if (radiobtn_PerLine.Checked)
            {
                sqlCmd.Append(@"
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(CDate),N',' + QUOTENAME(CDate))
FROM 
(
    SELECT DISTINCT(CDate) 
    FROM RFT 
) t
WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" AND CDate BETWEEN '{0}' AND '{1}'", d1, d2));
                }
                #endregion
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
from RFT A
INNER JOIN DBO.ORDERS C ON C.ID = A.OrderID
WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" AND A.CDate BETWEEN ''{0}'' AND ''{1}''", d1, d2));
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
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = ''0{0}'' ", Cell));
                }
                #endregion

                sqlCmd.Append(@"
group by  A.FACTORYID,A.SEWINGLINEID,A.CDATE

Order by [Factory], [Line],[CDate]

select *
from #tmpall as S
pivot(
  AVG(RFT)
  for [CDate] in ('+@cols+')
) as X

drop table #tmpall'
EXEC sp_executesql @sql
");
            }
            #endregion

            #region radiobtn_PerCell
            if (radiobtn_PerCell.Checked)
            {
                sqlCmd.Append(@"
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(CDate),N',' + QUOTENAME(CDate))
FROM 
(
    SELECT DISTINCT(CDate) 
    FROM RFT 
) t
WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" AND CDate BETWEEN '{0}' AND '{1}'", d1, d2));
                }
                #endregion
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
from RFT A
INNER JOIN DBO.ORDERS C ON C.ID = A.OrderID
Outer Apply(
	SELECT SewingCell 
	FROM SewingLine 
	WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID
) as SewingCell
WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" AND A.CDate BETWEEN ''{0}'' AND ''{1}''", d1, d2));
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
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = ''0{0}'' ", Cell));
                }
                #endregion

                sqlCmd.Append(@"
group by  A.FACTORYID,SewingCell.SewingCell,A.CDATE

Order by [Factory], [Cell],[CDate]

select *
from #tmpall as S
pivot(
  AVG(RFT)
  for [CDate] in ('+@cols+')
) as X

drop table #tmpall'
EXEC sp_executesql @sql
");
            }
            #endregion

            #region radiobtn_AllData
            if (radiobtn_AllData.Checked)
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

From DBO.Rft A
INNER JOIN DBO.ORDERS C ON C.ID = A.OrderID
OUTER APPLY DBO.GetCPURate(C.OrderTypeID,C.ProgramID,C.Category,C.BrandID,'O')D
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine 
	WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID
) E

WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" AND A.CDate BETWEEN '{0}' and '{1}'",
                    Convert.ToDateTime(Period1).ToString("d"), Convert.ToDateTime(Period2).ToString("d")));
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
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = '0{0}' ", Cell));
                }
                #endregion

                sqlCmd.Append(@"
Order by [Factory], [CDate], [OrderID]
");
            }
            #endregion            

            #region radioBtn_Detail
            if (radioBtn_Detail.Checked)
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

From DBO.Rft A
INNER JOIN DBO.Rft_Detail B ON B.ID = A.ID
INNER JOIN DBO.ORDERS C ON C.ID = A.OrderID
OUTER APPLY DBO.GetCPURate(C.OrderTypeID,C.ProgramID,C.Category,C.BrandID,'O')D
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine 
	WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID
) E
Outer Apply (
	select g.Description 
	from dbo.GarmentDefectCode g 
	where id = B.GarmentDefectCodeID
) F

WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" AND A.CDate BETWEEN '{0}' and '{1}'",
                    Convert.ToDateTime(Period1).ToString("d"), Convert.ToDateTime(Period2).ToString("d")));
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
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = '0{0}' ", Cell));
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
            if (radioBtn_SummybySP.Checked)
            {
                sqlCmd.Append(@"
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
select
	[Factory] = A.FACTORYID,
	[OrderID] = A.ORDERID
into #tmpall
From DBO.Rft A

WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" AND A.CDate BETWEEN '{0}' and '{1}'",
                    Convert.ToDateTime(Period1).ToString("d"), Convert.ToDateTime(Period2).ToString("d")));
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
Inner Join DBO.Rft A on Z.OrderID = A.OrderID
Inner Join DBO.Rft_Detail B ON B.ID = A.ID
Inner Join DBO.ORDERS C ON C.ID = Z.OrderID
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine 
	WHERE FactoryID = Z.Factory AND ID = A.SewinglineID
) E
Outer Apply (
	select g.Description 
	from dbo.GarmentDefectCode g 
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
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = '0{0}' ", Cell));
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
            if (radiobtn_SummybyStyle.Checked)
            {
                sqlCmd.Append(@"
IF OBJECT_ID('tempdb.dbo.#tmpall', 'U') IS NOT NULL
  DROP TABLE #tmpall
select
	[Factory] = A.FACTORYID,
	[Brand] = C.BRANDID,
	[Style] = C.STYLEID
into #tmpall
From DBO.Rft A
INNER JOIN DBO.ORDERS C ON C.ID = A.OrderID
WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" AND A.CDate BETWEEN '{0}' and '{1}'",
                    Convert.ToDateTime(Period1).ToString("d"), Convert.ToDateTime(Period2).ToString("d")));
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
Inner Join DBO.Rft A on Z.Factory = A.FactoryID
Inner Join DBO.Rft_Detail B ON B.ID = A.ID
Inner Join DBO.ORDERS C ON C.ID = A.OrderID and Z.Style = C.STYLEID
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine 
	WHERE FactoryID = Z.Factory AND ID = A.SewinglineID
) E
Outer Apply (
	select g.Description 
	from dbo.GarmentDefectCode g 
	where id = B.GarmentDefectCodeID
) F

WHERE 1=1
");

                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Cell))
                {
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = '0{0}' ", Cell));
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
            if (radiobtn_SummybyDateandStyle.Checked)
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
From DBO.Rft A
INNER JOIN DBO.ORDERS C ON C.ID = A.OrderID
WHERE 1=1
");
                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Period1))
                {
                    sqlCmd.Append(string.Format(@" AND A.CDate BETWEEN '{0}' and '{1}'",
                    Convert.ToDateTime(Period1).ToString("d"), Convert.ToDateTime(Period2).ToString("d")));
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
Inner Join DBO.Rft A on Z.Factory = A.FactoryID
Inner Join DBO.Rft_Detail B ON B.ID = A.ID
Inner Join DBO.ORDERS C ON C.ID = A.OrderID and Z.Style = C.STYLEID
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine 
	WHERE FactoryID = Z.Factory AND ID = A.SewinglineID
) E
Outer Apply (
	select g.Description 
	from dbo.GarmentDefectCode g 
	where id = B.GarmentDefectCodeID
) F

WHERE 1=1
");

                #region Append畫面上的條件
                if (!MyUtility.Check.Empty(Cell))
                {
                    sqlCmd.Append(string.Format(" and (SELECT SewingCell FROM SewingLine WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID) = '0{0}' ", Cell));
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
            if (radiobtn_PerLine.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_PerLine.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_PerLine.xltx", 1, true, null, objApp);// 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                for (int i = 2; i < printData.Columns.Count; i++)
                {
                    objSheets.Cells[1, i+1] = printData.Columns[i].ColumnName.ToString();
                }
                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            #region radiobtn_PerCell
            if (radiobtn_PerCell.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_PerCell.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_PerCell.xltx", 1, true, null, objApp);// 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                for (int i = 2; i < printData.Columns.Count; i++)
                {
                    objSheets.Cells[1, i + 1] = printData.Columns[i].ColumnName.ToString();
                }
                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            #region radiobtn_AllData
            if (radiobtn_AllData.Checked)
            {
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_AllData.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_AllData.xltx", 1, true, null, objApp);// 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            #region radioBtn_Detail
            if (radioBtn_Detail.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_Detail.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_Detail.xltx", 1, true, null, objApp);// 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            #region radioBtn_SummybySP
            if (radioBtn_SummybySP.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_SummarybySP.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_SummarybySP.xltx", 1, true, null, objApp);// 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            #region radiobtn_SummybyStyle
            if (radiobtn_SummybyStyle.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_SummarybyStyle.xltx"); //預先開啟excel app
                 MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_SummarybyStyle.xltx", 1, true, null, objApp);// 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            #region radiobtn_SummybyDateandStyle
            if (radiobtn_SummybyDateandStyle.Checked)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R20_SummarybyDateaAndStyle.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Quality_R20_SummarybyDateaAndStyle.xltx", 1, true, null, objApp);// 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            #endregion

            return true;
        }
    }
}

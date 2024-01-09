using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <summary>
    /// 有調整到需要一併更新至BI
    /// </summary>
    public class Sewing_R02
    {
        /// <inheritdoc/>
        public ResultReport GetMonthlyProductionOutputReport(Sewing_R02_MonthlyProductionOutputReport model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@StartOutputDate", model.StartOutputDate),
                new SqlParameter("@EndOutputDate", model.EndOutputDate),
                new SqlParameter("@Factory", model.Factory),
                new SqlParameter("@M", model.M),
                new SqlParameter("@ReportType", model.ReportType),
                new SqlParameter("@StartSewingLine", model.StartSewingLine),
                new SqlParameter("@EndSewingLine", model.EndSewingLine),
                new SqlParameter("@OrderBy", model.OrderBy),
                new SqlParameter("@ExcludeNonRevenue", model.ExcludeNonRevenue),
                new SqlParameter("@ExcludeSampleFactory", model.ExcludeSampleFactory),
                new SqlParameter("@ExcludeOfMockUp", model.ExcludeOfMockUp),
            };

            string sql = @"
exec dbo.GetMonthlyProductionOutputReport   @StartOutputDate,
                                            @EndOutputDate,
                                            @Factory,
                                            @M,
                                            @ReportType,
                                            @StartSewingLine,
                                            @EndSewingLine,
                                            @OrderBy,
                                            @ExcludeNonRevenue,
                                            @ExcludeSampleFactory,
                                            @ExcludeOfMockUp
";
            ResultReport resultReport = new ResultReport
            {
                Result = DBProxy.Current.Select(null, sql, listPar, out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }

        /// <inheritdoc/>
        public ResultReport GetTotalExcludeSubconIn(DataTable dt)
        {
            string sql = string.Format(@"
;with tmpQty as (
	select StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
		   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	from #tmp
	where LastShift <> 'O' 
          --排除Subcon in non sister資料
          and LastShift <> 'I'
		  or (LastShift = 'I' and SubconInType in ('1','2'))
	group by StdTMS
),
tmpTtlManPower as (
	/*select ManPower = Sum(Manpower)  算法更改，用下面的，舊的保留
	from (
		select OutputDate
			   , FactoryID
			   , SewingLineID
			   , LastShift
			   , Team
			   , ManPower = Max(ActManPower) 
		from #tmp
		where LastShift <> 'O' 
			  --排除Subcon in non sister資料
              and LastShift <> 'I'
		      or (LastShift = 'I' and SubconInType in ('1','2'))
		group by OutputDate, FactoryID, SewingLineID, LastShift, Team
	) a*/

	select ManPower = Sum(a.Manpower)  - sum(iif(LastShift = 'I', 0, isnull(d.ManPower, 0)))
	from (
		select OutputDate
				, FactoryID
				, SewingLineID
				, LastShift
				, Team
				, ManPower = Max(ActManPower)
		from #tmp
		where LastShift <> 'O'
		--排除 subcon in non sister的數值
        and ((LastShift <> 'I') or ( LastShift = 'I' and SubconInType not in ('0','3')))   
		group by OutputDate, FactoryID, SewingLineID, LastShift, Team 
	) a
	outer apply
	(
		select ManPower
		from (
			select OutputDate
					, FactoryID
					, SewingLineID
					, LastShift
					, Team
					, ManPower = Max(ActManPower)
					,SubconInType
			from #tmp
			where LastShift <> 'O'
			group by OutputDate, FactoryID, SewingLineID, LastShift, Team,SubconInType
		) m2
		where  (m2.LastShift = 'I' and m2.SubconInType in ('1','2'))
				and m2.Team = a.Team 
				and m2.SewingLineID = a.SewingLineID	
				and a.OutputDate = m2.OutputDate
				and m2.FactoryID = a.FactoryID	
	) d
)
select q.QAQty
	   , q.TotalCPU
	   , CPUSewer = IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 3))
	   , AvgWorkHour = IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2))
	   , q.ManHour
	   , Eff = IIF(q.ManHour * q.StdTMS = 0, 0, Round(q.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2))
from tmpQty q
left join tmpTtlManPower mp on 1 = 1");
            ResultReport resultReport = new ResultReport
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category,SubconInType", sqlcmd: sql, result: out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        /// <inheritdoc/>
        public ResultReport GetNoNSisterSubConIn(DataTable dt)
        {
            string sql = string.Format(@"
;with tmpQty as (
	select StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
		   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	from #tmp
	where LastShift = 'I' and SubconInType in ('0','3')
	group by StdTMS
)
select q.QAQty
	   , q.TotalCPU
	   , CPUSewer = IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 3))
	   , q.ManHour
	   , Eff = IIF(q.ManHour * q.StdTMS = 0, 0, Round(q.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2))
from tmpQty q");
            ResultReport resultReport = new ResultReport
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category,SubconInType", sqlcmd: sql, result: out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        /// <inheritdoc/>
        public ResultReport GetSisterSubConIn(DataTable dt)
        {
            string sql = string.Format(@"
;with tmpQty as (
	select StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
		   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	from #tmp
	where LastShift = 'I' and SubconInType in ('1','2')
	group by StdTMS
)
select q.QAQty
	   , q.TotalCPU
	   , CPUSewer = IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 3))
	   , q.ManHour
	   , Eff = IIF(q.ManHour * q.StdTMS = 0, 0, Round(q.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2))
from tmpQty q");
            ResultReport resultReport = new ResultReport
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category,SubconInType", sqlcmd: sql, result: out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        /// <inheritdoc/>
        public ResultReport GetCPUFactor(DataTable dt)
        {
            string sql = string.Format(@"
;with tmpData as (
	select CPUFactor = IIF(Category = 'M', MockupCPUFactor, OrderCPUFactor)
		   , QAQty
		   , CPU = QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)
		   , Style
	from #tmp
),
tmpSumQAQty as (
	select CPUFactor
		   , QAQty = sum(QAQty)
   	from tmpData 
   	group by CPUFactor
),
tmpSumCPU as (
	select CPUFactor
		   , CPU = sum(CPU)
   	from tmpData 
   	group by CPUFactor
),
tmpCountStyle as (
	select CPUFactor
		   , Style = COUNT(distinct Style)
   	from tmpData 
   	group by CPUFactor
)
select q.* 
	   , c.CPU
	   , s.Style
from tmpSumQAQty q
left join tmpSumCPU c on q.CPUFactor = c.CPUFactor
left join tmpCountStyle s on q.CPUFactor = s.CPUFactor");

            ResultReport resultReport = new ResultReport
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, "Category,MockupCPUFactor,OrderCPUFactor,QAQty,MockupCPU,OrderCPU,Rate,Style", sqlcmd: sql, result: out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        /// <inheritdoc/>
        public ResultReport GetSubprocess(DataTable dt)
        {
            string sql = string.Format(@"
alter table #tmp alter column OrderId varchar(13)
alter table #tmp alter column ComboType varchar(1)
alter table #tmp alter column QAQty int
alter table #tmp alter column LastShift varchar(1)
alter table #tmp alter column SubconInType varchar(1)

    Select ID
		   , rs = iif(ProductionUnit = 'TMS', 'CPU'
		   									, iif(ProductionUnit = 'QTY', 'AMT'
		   																, '')),
           [DecimalNumber] =case    when ProductionUnit = 'QTY' then 4
							        when ProductionUnit = 'TMS' then 3
							        else 0 end
    into #tmpArtwork
	from ArtworkType WITH (NOLOCK)
	where Classify in ('I','A','P') 
		  and IsTtlTMS = 0
          and IsPrintToCMP=1

	--準備台北資料(須排除這些)
	select ps.ID
	into #TPEtmp
	from PO_Supp ps
	inner join PO_Supp_Detail psd on ps.ID=psd.id and ps.SEQ1=psd.Seq1
	inner join Fabric fb on psd.SCIRefno = fb.SCIRefno 
	inner join MtlType ml on ml.id = fb.MtlTypeID
	where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
	and ml.isThread=1 
	and ps.SuppID <> 'FTY' and ps.Seq1 not Like '5%'
    
    select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
           , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
    into  #tmpAllSubprocess
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category NOT IN ('G','A')
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
            --排除 subcon in non sister的數值
          and ((a.LastShift <> 'I') or ( a.LastShift = 'I' and a.SubconInType not in ('0','3') ))           
          and ot.Price > 0 		    
		  and ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')
	group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price

    --FMS傳票部分顯示AT不分Hand/Machine，是因為政策問題，但比對Sewing R02時，會有落差，請根據SP#落在Hand CPU:10 /Machine:5，則只撈出Hand CPU:10這筆，抓其大值，以便加總總和等同於FMS傳票AT
    -- 當AT(Machine) = AT(Hand)時, 也要將Price歸0 (ISP20190520)
    update s set s.Price = 0
        from #tmpAllSubprocess s
        inner join (select * from #tmpAllSubprocess where ArtworkTypeID = 'AT (HAND)') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType
        where s.ArtworkTypeID = 'AT (MACHINE)'  and s.Price <= a.Price

    update s set s.Price = 0
        from #tmpAllSubprocess s
        inner join (select * from #tmpAllSubprocess where ArtworkTypeID = 'AT (MACHINE)') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType
        where s.ArtworkTypeID = 'AT (HAND)'  and s.Price <= a.Price

select ArtworkTypeID = t1.ID
	   , Price = isnull(sum(Round(t2.Price,t1.DecimalNumber)), 0)
	   , rs
from #tmpArtwork t1
left join #tmpAllSubprocess t2 on t2.ArtworkTypeID = t1.ID
group by t1.ID, rs
order by t1.ID");

            ResultReport resultReport = new ResultReport
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, "OrderId,ComboType,QAQty,LastShift,SubconInType", sqlcmd: sql, result: out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        /// <inheritdoc/>
        public ResultReport GetSubprocessbyCompanySubconIn(DataTable dt)
        {
            string sql = string.Format(@"

alter table #tmp alter column OrderId varchar(13)
alter table #tmp alter column ComboType varchar(1)
alter table #tmp alter column QAQty int
alter table #tmp alter column LastShift varchar(1)
alter table #tmp alter column SubconInType varchar(1)
alter table #tmp alter column Program varchar(12)

Select ID
		, rs = iif(ProductionUnit = 'TMS', 'CPU'
		   								, iif(ProductionUnit = 'QTY', 'AMT'
		   															, '')),
        [DecimalNumber] =case    when ProductionUnit = 'QTY' then 4
							    when ProductionUnit = 'TMS' then 3
							    else 0 end
into #tmpArtwork
from ArtworkType WITH (NOLOCK)
where Classify in ('I','A','P') 
		and IsTtlTMS = 0
        and IsPrintToCMP=1

--準備台北資料(須排除這些)
select ps.ID
into #TPEtmp
from PO_Supp ps
inner join PO_Supp_Detail psd on ps.ID=psd.id and ps.SEQ1=psd.Seq1
inner join Fabric fb on psd.SCIRefno = fb.SCIRefno 
inner join MtlType ml on ml.id = fb.MtlTypeID
where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
and ml.isThread=1 
and ps.SuppID <> 'FTY' and ps.Seq1 not Like '5%'

select ot.ArtworkTypeID
		, a.OrderId
		, a.ComboType
        , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
        , a.Program 
into  #tmpAllSubprocess
from #tmp a
inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category NOT IN ('G','A')
where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
		and a.LastShift not in('O','D','N')
		and ot.Price > 0 		    
		and ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			or ot.ArtworkTypeID <> 'SP_THREAD')
group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price,a.Program

--FMS傳票部分顯示AT不分Hand/Machine，是因為政策問題，但比對Sewing R02時，會有落差，請根據SP#落在Hand CPU:10 /Machine:5，則只撈出Hand CPU:10這筆，抓其大值，以便加總總和等同於FMS傳票AT
-- 當AT(Machine) = AT(Hand)時, 也要將Price歸0 (ISP20190520)
update s set s.Price = 0
    from #tmpAllSubprocess s
    inner join (select * from #tmpAllSubprocess where ArtworkTypeID = 'AT (HAND)') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType
    where s.ArtworkTypeID = 'AT (MACHINE)'  and s.Price <= a.Price

update s set s.Price = 0
    from #tmpAllSubprocess s
    inner join (select * from #tmpAllSubprocess where ArtworkTypeID = 'AT (MACHINE)') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType
    where s.ArtworkTypeID = 'AT (HAND)'  and s.Price <= a.Price

select ArtworkTypeID = t1.ID
	   , Price = isnull(sum(Round(Price,t1.DecimalNumber)), 0)
	   , rs
       , [Company] = t2.Program
from #tmpArtwork t1
left join #tmpAllSubprocess t2 on t2.ArtworkTypeID = t1.ID
group by t1.ID, rs,t2.Program having isnull(sum(Price), 0) > 0
order by t1.ID");

            ResultReport resultReport = new ResultReport
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, "OrderId,ComboType,QAQty,LastShift,SubconInType,Program", sqlcmd: sql, result: out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        /// <inheritdoc/>
        public ResultReport GetSubprocessbyCompanySubconOut(DataTable dt)
        {
            string sql = string.Format(@"
alter table #tmp alter column OrderId varchar(13)
alter table #tmp alter column ComboType varchar(1)
alter table #tmp alter column QAQty int
alter table #tmp alter column LastShift varchar(1)
alter table #tmp alter column SubconInType varchar(1)
alter table #tmp alter column SubconOutFty varchar(8)

Select ID
		, rs = iif(ProductionUnit = 'TMS', 'CPU'
		   								, iif(ProductionUnit = 'QTY', 'AMT'
		   															, '')),
        [DecimalNumber] =case    when ProductionUnit = 'QTY' then 4
							    when ProductionUnit = 'TMS' then 3
							    else 0 end
into #tmpArtwork
from ArtworkType WITH (NOLOCK)
where Classify in ('I','A','P') 
		and IsTtlTMS = 0
        and IsPrintToCMP=1

--準備台北資料(須排除這些)
select ps.ID
into #TPEtmp
from PO_Supp ps
inner join PO_Supp_Detail psd on ps.ID=psd.id and ps.SEQ1=psd.Seq1
inner join Fabric fb on psd.SCIRefno = fb.SCIRefno 
inner join MtlType ml on ml.id = fb.MtlTypeID
where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
and ml.isThread=1 
and ps.SuppID <> 'FTY' and ps.Seq1 not Like '5%'

select ot.ArtworkTypeID
		, a.OrderId
		, a.ComboType
        , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
        , a.SubconOutFty 
into  #tmpAllSubprocess
from #tmp a
inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category NOT IN ('G','A')
where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
		and a.LastShift not in('I','D','N')
		and ot.Price > 0 		    
and ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
	or ot.ArtworkTypeID <> 'SP_THREAD')
group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price,a.SubconOutFty

--FMS傳票部分顯示AT不分Hand/Machine，是因為政策問題，但比對Sewing R02時，會有落差，請根據SP#落在Hand CPU:10 /Machine:5，則只撈出Hand CPU:10這筆，抓其大值，以便加總總和等同於FMS傳票AT
-- 當AT(Machine) = AT(Hand)時, 也要將Price歸0 (ISP20190520)
update s set s.Price = 0
    from #tmpAllSubprocess s
    inner join (select * from #tmpAllSubprocess where ArtworkTypeID = 'AT (HAND)') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType
    where s.ArtworkTypeID = 'AT (MACHINE)'  and s.Price <= a.Price

update s set s.Price = 0
    from #tmpAllSubprocess s
    inner join (select * from #tmpAllSubprocess where ArtworkTypeID = 'AT (MACHINE)') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType
    where s.ArtworkTypeID = 'AT (HAND)'  and s.Price <= a.Price

select ArtworkTypeID = t1.ID
	   , Price = isnull(sum(Round(Price,t1.DecimalNumber)), 0)
	   , rs
       , [Company] = t2.SubconOutFty
from #tmpArtwork t1
left join #tmpAllSubprocess t2 on t2.ArtworkTypeID = t1.ID
group by t1.ID, rs,t2.SubconOutFty having isnull(sum(Price), 0) > 0
order by t1.ID");

            ResultReport resultReport = new ResultReport
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, "OrderId,ComboType,QAQty,LastShift,SubconInType,SubconOutFty", sqlcmd: sql, result: out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        /// <inheritdoc/>
        public ResultReport GetSubcon(DataTable dt)
        {
            string sql = string.Format(@"
;with tmpSubconIn as (
	Select 'I' as Type
		   , Company = Program 
		   , TtlCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	from #tmp
	where LastShift = 'I'
	group by Program
),
tmpSubconOut as (
    Select Type = 'O'
		   , Company = t.SubconOutFty
		   , TtlCPU = ROUND(Sum(t.QAQty*IIF(t.Category = 'M', t.MockupCPU * t.MockupCPUFactor, t.OrderCPU * t.OrderCPUFactor * t.Rate)),3)
	from #tmp t
	where LastShift = 'O'
	group by t.SubconOutFty
)
select * from (
select * from tmpSubconIn
union all
select * from tmpSubconOut ) as a 
order by Type,iif(Company = 'Other','Z','A'),Company");

            ResultReport resultReport = new ResultReport
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, "SewingLineID,QAQty,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,OrderId,Program,Category,FactoryID,SubconOutFty", sqlcmd: sql, result: out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        /// <inheritdoc/>
        public ResultReport GetWorkDay(DataTable dt, Sewing_R02_MonthlyProductionOutputReport model)
        {
            ResultReport resultReport = new ResultReport();
            string sql = string.Empty;
            #region  中國工廠自抓/其它場Pams [Total Work Day]
            if (model.IsCN)
            {
                sql = @"select Distinct OutputDate from #tmp where LastShift <> 'O'";
                resultReport.Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sql, out DataTable dtWorkDay);
                if (!resultReport.Result)
                {
                    resultReport.IntValue = 0;
                }
                else
                {
                    resultReport.IntValue = dtWorkDay.Rows.Count;
                }
            }
            else
            {
                sql = $@"
alter table #tmp alter column FactoryID varchar(8)

select distinct t.FactoryID, f.IsSampleRoom
into    #tmpResult
from    #tmp t
inner join Factory f with (nolock) on f.ID = t.FactoryID

select FactoryID from #tmpResult where IsSampleRoom = 0
select FactoryID from #tmpResult where IsSampleRoom = 1
";

                resultReport.Result = MyUtility.Tool.ProcessWithDatatable(dt, "FactoryID", sql, out DataTable[] datatable);
                if (!resultReport.Result)
                {
                    resultReport.IntValue = 0;
                    return resultReport;
                }

                DataTable dtNotSampleRoomFty = datatable[0];
                DataTable dtIsSampleRoomFty = datatable[1];
                List<string> listWorkDate = new List<string>();

                if (dtIsSampleRoomFty.Rows.Count > 0)
                {
                    string whereFty = dtIsSampleRoomFty.AsEnumerable().Select(s => $"'{s["FactoryID"].ToString()}'").JoinToString(",");
                    string strWorkDay = $@"select Distinct [OutputDate] = Format(OutputDate,'yyyyMMdd') from #tmp where LastShift <> 'O' and FactoryID in ({whereFty})";
                    resultReport.Result = MyUtility.Tool.ProcessWithDatatable(dt, null, strWorkDay, out DataTable dtWorkDay);
                    if (!resultReport.Result)
                    {
                        resultReport.IntValue = 0;
                    }
                    else if (dtWorkDay.Rows.Count > 0)
                    {
                        listWorkDate.AddRange(dtWorkDay.AsEnumerable().Select(s => s["OutputDate"].ToString()).ToList());
                    }
                }

                if (dtNotSampleRoomFty.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtNotSampleRoomFty.Rows)
                    {
                        List<APIData> listAPIData = new List<APIData>();
                        GetApiData.GetAPIData(model.M, dr["FactoryID"].ToString(), model.StartDate, model.EndDate, out listAPIData);
                        if (listAPIData != null)
                        {
                            listWorkDate.AddRange(listAPIData.Where(w => w.SewTtlManhours != 0).Select(s => s.DateYYYYMMDD));
                        }
                    }
                }

                resultReport.IntValue = listWorkDate.Distinct().Count();
            }
            #endregion
            return resultReport;
        }
    }
}

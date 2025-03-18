using Ict;
using Ict.Win.UI;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <summary>
    /// 有調整到需要一併更新至BI
    /// </summary>
    public class Sewing_R02
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Sewing_R02()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 900,
            };
        }

        /// <inheritdoc/>
        public Base_ViewModel GetMonthlyProductionOutputReport(Sewing_R02_ViewModel model)
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
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sql, listPar, out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetTotalExcludeSubconIn(DataTable dt)
        {
            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
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
                Base_ViewModel resultReport = new Base_ViewModel
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category,SubconInType", sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };

                if (!resultReport.Result)
                {
                    return resultReport;
                }

                resultReport.Dt = dataTable;
                return resultReport;
            }
        }

        /// <inheritdoc/>
        public Base_ViewModel GetNoNSisterSubConIn(DataTable dt)
        {
            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
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
                Base_ViewModel resultReport = new Base_ViewModel
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category,SubconInType", sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };

                if (!resultReport.Result)
                {
                    return resultReport;
                }

                resultReport.Dt = dataTable;
                return resultReport;
            }
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSisterSubConIn(DataTable dt)
        {
            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
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
                Base_ViewModel resultReport = new Base_ViewModel
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, "OutputDate,StdTMS,QAQty,WorkHour,ActManPower,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,FactoryID,SewingLineID,Team,Category,SubconInType", sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };

                if (!resultReport.Result)
                {
                    return resultReport;
                }

                resultReport.Dt = dataTable;
                return resultReport;
            }
        }

        /// <inheritdoc/>
        public Base_ViewModel GetCPUFactor(DataTable dt)
        {
            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
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
                Base_ViewModel resultReport = new Base_ViewModel
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, "Category,MockupCPUFactor,OrderCPUFactor,QAQty,MockupCPU,OrderCPU,Rate,Style", sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };

                if (!resultReport.Result)
                {
                    return resultReport;
                }

                resultReport.Dt = dataTable;
                return resultReport;
            }
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSubprocess(DataTable dt)
        {
            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
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
	union all
    -- 取得特殊的轉移訂單ArtworkTypeID 
	select distinct ID = ap.LocalSuppID + ' ' + a.ID
	, rs = iif(ProductionUnit = 'TMS', 'CPU'
		   									, iif(ProductionUnit = 'QTY', 'AMT'
		   																, '')),
           [DecimalNumber] =case    when ProductionUnit = 'QTY' then 4
							        when ProductionUnit = 'TMS' then 3
							        else 0 end
	from ArtworkType a WITH (NOLOCK)
	inner join artworkpo ap WITH (NOLOCK) on ap.ArtworkTypeID = a.ID and LocalSuppID in ('G168','SPP')

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
    
    SELECT  ArtworkTypeID = case when isnull(apd.ArtworkTypeID,'') !='' then apd.LocalSuppID + ' ' + apd.ArtworkTypeID
							else ot.ArtworkTypeID end
		   , a.OrderId
		   , a.ComboType
           , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
    into #tmpAllSubprocess
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category NOT IN ('G','A')
	left join (		
		  select distinct apd.ArtworkTypeID,apd.OrderID,ap.LocalSuppID 
		  from ArtworkPO ap
		  inner join ArtworkPO_Detail apd on ap.ID= apd.ID
	) apd on apd.OrderID = a.OrderID and ot.ArtworkTypeID = apd.ArtworkTypeID and apd.LocalSuppID in ('G168','SPP')
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
            --排除 subcon in non sister的數值
          and ((a.LastShift <> 'I') or ( a.LastShift = 'I' and a.SubconInType not in ('0','3') ))           
          and ot.Price > 0 		    
		  and ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')
	group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price,apd.ArtworkTypeID,apd.LocalSuppID

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
order by case when t1.ID like 'SPP%' or t1.ID like 'G168%' then 1 else 0 end, t1.ID

");
                Base_ViewModel resultReport = new Base_ViewModel
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, "OrderId,ComboType,QAQty,LastShift,SubconInType", sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };

                if (!resultReport.Result)
                {
                    return resultReport;
                }

                resultReport.Dt = dataTable;
                return resultReport;
            }
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSubprocessByFactoryOutPutDate(DataTable dt)
        {
            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
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
    
    select a.FactoryID
		   , [OutputDate] = dateadd(day, -1, cast(FORMAT(dateadd(month, 1, a.OutputDate), 'yyyy/MM/01') as date))
		   , ot.ArtworkTypeID
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
	group by a.FactoryID
		   , dateadd(day, -1, cast(FORMAT(dateadd(month, 1, a.OutputDate), 'yyyy/MM/01') as date))
		   , ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price

    --FMS傳票部分顯示AT不分Hand/Machine，是因為政策問題，但比對Sewing R02時，會有落差，請根據SP#落在Hand CPU:10 /Machine:5，則只撈出Hand CPU:10這筆，抓其大值，以便加總總和等同於FMS傳票AT
    -- 當AT(Machine) = AT(Hand)時, 也要將Price歸0 (ISP20190520)
    update s set s.Price = 0
        from #tmpAllSubprocess s
        inner join (select * from #tmpAllSubprocess where ArtworkTypeID = 'AT (HAND)') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType and s.FactoryID = a.FactoryID and s.OutputDate = a.OutputDate
        where s.ArtworkTypeID = 'AT (MACHINE)'  and s.Price <= a.Price

    update s set s.Price = 0
        from #tmpAllSubprocess s
        inner join (select * from #tmpAllSubprocess where ArtworkTypeID = 'AT (MACHINE)') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType and s.FactoryID = a.FactoryID and s.OutputDate = a.OutputDate
        where s.ArtworkTypeID = 'AT (HAND)'  and s.Price <= a.Price

	select s.FactoryID, s.OutputDate
		, Price = isnull(sum(Round(s.Price,t.DecimalNumber)), 0)	
	from #tmpAllSubprocess s
	left join #tmpArtwork t on s.ArtworkTypeID = t.ID
	group by s.FactoryID, s.OutputDate");
                Base_ViewModel resultReport = new Base_ViewModel
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };

                if (!resultReport.Result)
                {
                    return resultReport;
                }

                resultReport.Dt = dataTable;
                return resultReport;
            }
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSubprocessbyCompanySubconIn(DataTable dt)
        {
            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
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
                Base_ViewModel resultReport = new Base_ViewModel
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, "OrderId,ComboType,QAQty,LastShift,SubconInType,Program", sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };

                if (!resultReport.Result)
                {
                    return resultReport;
                }

                resultReport.Dt = dataTable;
                return resultReport;
            }
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSubprocessbyCompanySubconOut(DataTable dt)
        {
            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
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
                Base_ViewModel resultReport = new Base_ViewModel
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, "OrderId,ComboType,QAQty,LastShift,SubconInType,SubconOutFty", sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };

                if (!resultReport.Result)
                {
                    return resultReport;
                }

                resultReport.Dt = dataTable;
                return resultReport;
            }
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSubcon(DataTable dt)
        {
            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
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
                Base_ViewModel resultReport = new Base_ViewModel
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, "SewingLineID,QAQty,LastShift,MockupCPU,MockupCPUFactor,OrderCPU,OrderCPUFactor,Rate,OrderId,Program,Category,FactoryID,SubconOutFty", sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };

                if (!resultReport.Result)
                {
                    return resultReport;
                }

                resultReport.Dt = dataTable;
                return resultReport;
            }
        }

        /// <inheritdoc/>
        public Base_ViewModel GetWorkDay(DataTable dt, Sewing_R02_ViewModel model)
        {
            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
            {
                Base_ViewModel resultReport = new Base_ViewModel();
                string sql = string.Empty;
                #region  中國工廠自抓/其它場Pams [Total Work Day]
                if (model.IsCN)
                {
                    sql = $@"select Distinct OutputDate from #tmp where LastShift <> 'O' and OutputDate >= '{model.StartDate.ToString("yyyy/MM/dd")}' and OutputDate <= '{model.EndDate.ToString("yyyy/MM/dd")}' ";
                    if (!string.IsNullOrEmpty(model.Factory))
                    {
                        sql += $"and FactoryID = '{model.Factory}' ";
                    }

                    resultReport.Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sql, out DataTable dtWorkDay, conn: sqlConn);
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
                    resultReport.Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sql, out DataTable[] datatable, conn: sqlConn);
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
                        string whereFty = dtIsSampleRoomFty.AsEnumerable().Select(s => $"'{s["FactoryID"]}'").JoinToString(",");
                        string strWorkDay = $@"select Distinct [OutputDate] = Format(OutputDate,'yyyyMMdd') from #tmp where LastShift <> 'O' and MDivisionID = '{model.M}' and FactoryID in ({whereFty}) and OutputDate >= '{model.StartDate.ToString("yyyy/MM/dd")}' and OutputDate <= '{model.EndDate.ToString("yyyy/MM/dd")}'";
                        resultReport.Result = this.DBProxy.SelectByConn(conn: sqlConn, cmdtext: strWorkDay, datas: out DataTable dtWorkDay);
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

        /// <summary>
        /// Direct Manpower(From PAMS)
        /// </summary>
        /// <param name="model">Sewing R02 ViewModel</param>
        /// <returns>List<APIData></returns>
        public List<APIData> GetPAMS(Sewing_R02_ViewModel model)
        {
            List<APIData> pams = new List<APIData>();
            if (!Env.User.Keyword.EqualString("CM1"))
            {
                GetApiData.GetAPIData(model.M, model.Factory, model.StartDate, model.EndDate, out List<APIData> dataMode);
                if (dataMode != null)
                {
                    pams = dataMode.ToList();
                }
            }

            return pams;
        }

        /// <inheritdoc/>
        public DualResult GetPamsAttendanceSummaryAsync(AttendanceSummary_APICondition model, out DataSet returnResult)
        {
            returnResult = new DataSet();
            string url = MyUtility.GetValue.Lookup(@"select top 1 URL from WebApiUrl where Description like 'PAMS WEB API%'", "ManufacturingExecution");
            string api = "api/AttendanceSummary/AttendanceSummary_Summary";

            #region 台北端PMSDB連線
            DataTable dtQry;
            // Port同Pmsdb IIS設定
            var resultQry = this.DBProxy.Select(
                null,
@"select APIPort = Case @@SERVERNAME
       When 'PMSDB\ESP' Then '9530'
       When 'PMSDB\SNP' Then '9531'
       When 'PMSDB\SPT' Then '9532'
       When 'PMSDB\PH1' Then '9533'
       When 'PMSDB\PH2' Then '9534'
       When 'PMSDB\PAN' Then '9535'
       When 'PMSDB\SPR' Then '9536'
       When 'PMSDB\SPS' Then '9537'
       When 'PMSDB\HZG' Then '9538'
       When 'PMSDB\SWR' Then '9539'
       When 'Testing\ESP' Then '9530'
       When 'Testing\SNP' Then '9531'
       When 'Testing\SPT' Then '9532'
       When 'Testing\PH1' Then '9533'
       When 'Testing\PH2' Then '9534'
       When 'Testing\PAN' Then '9535'
       When 'Testing\SPR' Then '9536'
       When 'Testing\SPS' Then '9537'
       When 'Testing\HZG' Then '9538'
       When 'Testing\SWR' Then '9539'
       Else ''
       End ", out dtQry);

            if (!resultQry)
            {
                return new DualResult(false, resultQry.Messages.ToString());
            }

            if (dtQry != null && dtQry.Rows.Count > 0 && !dtQry.Rows[0]["APIPort"].ToString().Empty())
            {
                // 目前是call各環境 pmsdb api
                url = "http://172.17.11.98:" + dtQry.Rows[0]["APIPort"].ToString();
            }

            #endregion 台北端PMSDB連線

            if (MyUtility.Check.Empty(url))
            {
                return new DualResult(false, "PAMS WEB API not exists");
            }

            try
            {
                // 初始化 HttpClient 來發送請求
                using (HttpClient client = new HttpClient())
                {
                    // 將Model 轉成Json格式
                    string jsonBody = JsonConvert.SerializeObject(model);

                    // 再將JSON 字串轉換為跳脫的字串
                    string escapedJson = JsonConvert.SerializeObject(jsonBody);

                    // 建立 HttpContent 物件，指定內容類型為 application/json
                    var content = new StringContent(escapedJson, Encoding.UTF8, "application/json");

                    // 發送同步Post 請求
                    HttpResponseMessage response = client.PostAsync(url + "//" + api, content).Result;

                    // 確認回應是成功的
                    response.EnsureSuccessStatusCode();

                    // 將回應的 JSON 內容轉換為字串
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    AttendanceSummaryResult result = JsonConvert.DeserializeObject<AttendanceSummaryResult>(responseBody);

                    if (!MyUtility.Check.Empty(result.Exception))
                    {
                        return new DualResult(false, result.Exception);
                    }

                    // 將 JSON 字串轉換為 DataSet
                    returnResult = result.QueryResult;
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }

            return new DualResult(true);
        }

        /// <summary>
        /// SewingR04 外發加工段計算 + SPH TotalCPU 計算
        /// </summary>
        /// <param name="dt">GetMonthlyProductionOutputReport Table 1</param>
        /// <param name="model">Sewing_R02_ViewModel</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel GetSubprocessAndSPHTotalCPU(DataTable dt, Sewing_R02_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@M", model.M),
                new SqlParameter("@Factory", model.Factory),
                new SqlParameter("@StartDate",  model.StartOutputDate),
                new SqlParameter("@EndDate",  model.EndOutputDate),
                new SqlParameter("@line1", model.StartSewingLine),
                new SqlParameter("@line2", model.EndSewingLine),
            };

            string sqlwhere = string.Empty;
            if (!model.StartSewingLine.Empty())
            {
                sqlwhere += " and s.SewingLineID >= @line1";
            }

            if (!model.EndSewingLine.Empty())
            {
                sqlwhere += " and s.SewingLineID <= @line2";
            }

            if (!MyUtility.Check.Empty(model.Factory))
            {
                sqlwhere += " and s.FactoryID = @Factory ";
            }

            if (!MyUtility.Check.Empty(model.M))
            {
                sqlwhere += " and s.MDivisionID = @M ";
            }

            string sql = $@"
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
union all
select distinct ID = ap.LocalSuppID + ' ' + a.ID
, rs = iif(ProductionUnit = 'TMS', 'CPU'
		   								, iif(ProductionUnit = 'QTY', 'AMT'
		   															, '')),
        [DecimalNumber] =case    when ProductionUnit = 'QTY' then 4
							    when ProductionUnit = 'TMS' then 3
							    else 0 end
from ArtworkType a WITH (NOLOCK)
inner join ArtworkPO ap WITH (NOLOCK) on ap.ArtworkTypeID = a.ID and LocalSuppID in ('G168','SPP')

--準備台北資料(須排除這些)
select ps.ID
into #TPEtmp
from PO_Supp ps WITH (NOLOCK)
inner join PO_Supp_Detail psd WITH (NOLOCK) on ps.ID=psd.id and ps.SEQ1=psd.Seq1
inner join Fabric fb WITH (NOLOCK) on psd.SCIRefno = fb.SCIRefno 
inner join MtlType ml WITH (NOLOCK) on ml.id = fb.MtlTypeID
where ml.Junk = 0 
and psd.Junk = 0 
and fb.Junk = 0
and ml.isThread = 1 
and ps.SuppID <> 'FTY' 
and ps.Seq1 not Like '5%'
    
SELECT  ArtworkTypeID = case when isnull(apd.ArtworkTypeID,'') !='' then apd.LocalSuppID + ' ' + apd.ArtworkTypeID
						else ot.ArtworkTypeID end
	   , a.OrderId
	   , a.ComboType
       , Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](a.OrderId ,a.ComboType), 100) / 100)
into #tmpAllSubprocess
from #tmp a
inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category NOT IN ('G','A')
left join (		
	  select distinct apd.ArtworkTypeID,apd.OrderID,ap.LocalSuppID 
	  from ArtworkPO ap WITH (NOLOCK)
	  inner join ArtworkPO_Detail apd WITH (NOLOCK) on ap.ID= apd.ID
      where ap.LocalSuppID in ('G168','SPP')
) apd on apd.OrderID = a.OrderID and ot.ArtworkTypeID = apd.ArtworkTypeID
where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O') ) 
        --排除 subcon in non sister的數值
      and ((a.LastShift <> 'I') or ( a.LastShift = 'I' and a.SubconInType not in ('0','3') ))           
      and ot.Price > 0 		    
	  and ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
		  or ot.ArtworkTypeID <> 'SP_THREAD')
group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price,apd.ArtworkTypeID,apd.LocalSuppID

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
into #tmpFinalArtwork 
from #tmpArtwork t1
left join #tmpAllSubprocess t2 on t2.ArtworkTypeID = t1.ID
group by t1.ID, rs
order by t1.ID

-- 取得SubConOut 數值

select  FactoryID = s.FactoryID
    ,OrderId = sd.OrderId
    ,Team = s.Team
    ,OutputDate = s.OutputDate
    ,SewingLineID = s.SewingLineID
    ,LastShift = IIF(s.Shift <> 'O' and s.Category <> 'M' and o.LocalOrder = 1, 'I',s.Shift) 
    ,Category = s.Category
    ,ComboType = sd.ComboType
    ,SubconOutFty = s.SubconOutFty
    ,SubConOutContractNumber = s.SubConOutContractNumber
    ,[Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id,sd.ComboType),100)/100
    ,sd.QAQty
    ,ot.Price
    ,ot.ArtworkTypeID
    ,ttlPrice = Round(sum(sd.QAQty*isnull([dbo].[GetOrderLocation_Rate](o.id,sd.ComboType),100)/100 * ot.Price)over(partition by s.FactoryID,sd.OrderId,s.Team,s.OutputDate,s.SewingLineID, IIF(s.Shift <> 'O' and s.Category <> 'M' and o.LocalOrder = 1, 'I',s.Shift) ,s.Category,sd.ComboType,s.SubconOutFty,s.SubConOutContractNumber,ot.ArtworkTypeID),3)
    ,ta.rs
into #tmpFinal
from SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
inner join Orders o WITH (NOLOCK) on o.ID = sd.OrderId
inner join Order_TmsCost ot WITH (NOLOCK) on ot.id = o.id
inner join #tmpArtwork ta WITH (NOLOCK) on ot.ArtworkTypeID = ta.ID
where exists(
	SELECT 1 
	from ArtworkPo po with (nolock)
	inner join ArtworkPo_Detail apd with (nolock) on po.ID = apd.ID
	where	apd.PoQty > 0 
			and apd.OrderID = sd.OrderID
			and po.FactoryID = s.FactoryID
			and po.MDivisionID = s.MDivisionID
			and po.artworktypeid = ot.ArtworkTypeID
			and po.LocalSuppID not in ('SPP','G168')
            and po.Status = 'Approved'
)
and (@StartDate is null or s.OutputDate >= @StartDate) and (@EndDate is null or s.OutputDate <= @EndDate) and
((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')
--排除subcon-in的資料
and not (s.Shift <> 'O' and o.LocalOrder = 1 and o.SubconInType <> 0)
{sqlwhere}

-- 取得外發清單
select ArtworkTypeID
    ,[ProductionUnit] = iif(a.ProductionUnit = 'TMS', 'CPU'
		   			    , iif(a.ProductionUnit = 'QTY', 'AMT', ''))
    ,[TTL_Price] = sum(ttlPrice) 
from #tmpFinal t
left join ArtworkType a WITH (NOLOCK) on t.ArtworkTypeID = a.ID
group by t.ArtworkTypeID,a.ProductionUnit

declare　@TTLCPU float = (select sum(Price) from #tmpFinalArtwork where rs ='CPU')
declare　@AMT float = (select sum(Price) from #tmpFinalArtwork where rs ='AMT' and ArtworkTypeID in ('EMBROIDERY','Garment Dye','GMT WASH','PRINTING'))
declare @SubConOutCPU float = (select [TTL_Price] = isnull(sum(ttlPrice),0) from #tmpFinal where rs ='CPU')
declare @SubConOutAMT float = (select [TTL_Price] = isnull(sum(ttlPrice),0) from #tmpFinal where rs ='AMT' and ArtworkTypeID in ('EMBROIDERY','Garment Dye','GMT WASH','PRINTING'))


-- 取得SPH Total CPU
select [SPH_ttlCPU] = CAST(isnull(@TTLCPU,0) - @SubConOutCPU + ((isnull(@AMT,0) - isnull(@SubConOutAMT,0))/2.5) as decimal(12,4))

drop table #tmp,#tmpAllSubprocess,#tmpArtwork,#TPEtmp,#tmpFinalArtwork,#tmpFinal";

            this.DBProxy.OpenConnection("Production", out SqlConnection sqlConn);
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = PublicPrg.CrossUtility.ProcessWithDatatable(dt, string.Empty, sqlcmd: sql, result: out DataTable[] dtarr, conn: sqlConn, paramters: listPar),
            };
            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dtarr;
            return resultReport;
        }
    }
}

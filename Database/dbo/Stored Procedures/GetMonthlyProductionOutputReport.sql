CREATE PROCEDURE [dbo].[GetMonthlyProductionOutputReport]
	@StartOutputDate date,
	@EndOutputDate date,
	@Factory varchar(8) = '',
	@M varchar(8) = '',
	@ReportType int = 1, --可傳入(1,2,3), 可傳入null表示1, [1=By Date, 2=By Sewing Line, 3=By Sewing Line By Team]
	@StartSewingLine varchar(5) = '',
	@EndSewingLine varchar(5) = '',
	@OrderBy int = 1, -- reporttype = 2的時候用的
	@ExcludeNonRevenue bit = 0,
	@ExcludeSampleFactory bit = 0
AS
begin

select  s.OutputDate
		, s.Category
		, s.Shift
		, s.SewingLineID
		, [ActManPower] = s.Manpower
		, s.Team
		, sd.OrderId
		, sd.ComboType
		, sd.WorkHour
		, sd.QAQty
		, sd.InlineQty
		, [OrderCategory] = isnull(o.Category,'')
		, o.LocalOrder
		, s.FactoryID
		, [OrderProgram] = isnull(o.ProgramID,'') 
		, [MockupProgram] = isnull(mo.ProgramID,'')
		, [OrderCPU] = isnull(o.CPU,0)
		, [OrderCPUFactor] = isnull(o.CPUFactor,0)
		, [MockupCPU] = isnull(mo.Cpu,0)
		, [MockupCPUFactor] = isnull(mo.CPUFactor,0)
		, [OrderStyle] = isnull(o.StyleID,'')
		, [MockupStyle] = isnull(mo.StyleID,'')
        , [Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id ,sd.ComboType),100)/100
		, System.StdTMS
        , o.SubconInType
        , [SubconOutFty] = iif(sf.id is null,'Other',s.SubconOutFty)
INTO #tmpSewingDetail
from System,SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
left join SCIFty sf WITH (NOLOCK) on sf.ID = s.SubconOutFty
left join factory f WITH (NOLOCK) on f.id=s.FactoryID
where	(o.CateGory NOT IN ('G','A') or s.Category='M') and
		s.OutputDate between @StartOutputDate and @EndOutputDate and 
		s.SewingLineID >= iif(isnull(@StartSewingLine, '') = '', s.SewingLineID, @StartSewingLine) and
		s.SewingLineID <= iif(isnull(@EndSewingLine, '') = '', s.SewingLineID, @EndSewingLine) and
		s.FactoryID = iif(isnull(@Factory, '') = '', s.FactoryID, @Factory) and
		s.MDivisionID = iif(isnull(@M, '') = '', s.MDivisionID, @M) and
		(@ExcludeSampleFactory = 0 or f.type <> 'S') and
		(@ExcludeNonRevenue = 0 or isnull(o.NonRevenue, 0) = 0)

select OutputDate,Category
	   , Shift
	   , SewingLineID
	   , ActManPower1 = ActManPower
	   , Team
	   , OrderId
	   , ComboType
	   , WorkHour = Round(sum(WorkHour),3)
	   , QAQty = sum(QAQty)
	   , InlineQty = sum(InlineQty)
	   , OrderCategory
	   , LocalOrder
	   , FactoryID
	   , OrderProgram
	   , MockupProgram
	   , OrderCPU
	   , OrderCPUFactor
	   , MockupCPU
	   , MockupCPUFactor
	   , OrderStyle
	   , MockupStyle
	   , Rate
	   , StdTMS
	   , IIF(Shift <> 'O' and Category <> 'M' and LocalOrder = 1, 'I',Shift) as LastShift
       , SubconInType
       , [SubconOutFty] = isnull(SubconOutFty,'')
INTO #tmpSewingGroup
from #tmpSewingDetail
group by OutputDate, Category, Shift, SewingLineID, Team, OrderId, ComboType
		 , OrderCategory, LocalOrder, FactoryID, OrderProgram, MockupProgram
		 , OrderCPU, OrderCPUFactor, MockupCPU, MockupCPUFactor, OrderStyle
		 , MockupStyle, Rate, StdTMS,SubconInType,isnull(SubconOutFty,'')
        ,ActManPower

select t.*
	   , isnull(w.Holiday, 0) as Holiday
	   , ActManPower1 as ActManPower
INTO #tmp1stFilter
from #tmpSewingGroup t
left join WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID 
									  and w.Date = t.OutputDate 
									  and w.SewingLineID = t.SewingLineID


select OutputDate
	   , Shift = IIF(LastShift = 'D', 'Day'
									, IIF(LastShift = 'N', 'Night'
														 , IIF(LastShift = 'O', 'Subcon-Out'
														 					  , 'Subcon-In')))
	   , Team
	   , SewingLineID
	   , OrderId
	   , Style = IIF(Category = 'M', MockupStyle, OrderStyle)
	   , QAQty
	   , ActManPower
	   , Program = IIF(Category = 'M',MockupProgram,OrderProgram)
	   , WorkHour
	   , StdTMS
	   , MockupCPU
	   , MockupCPUFactor
	   , OrderCPU
	   , OrderCPUFactor
	   , Rate
	   , Category
	   , LastShift
	   , ComboType
	   , FactoryID
       , SubconInType
       , SubconOutFty
into #tmp
from #tmp1stFilter

select * from #tmp

if(@ReportType = 1)
begin
	;with AllOutputDate as (
	    select distinct OutputDate		   
		from #tmp
	),
	tmpQty as (
		select OutputDate
			   , StdTMS
			   , QAQty = Sum(QAQty)
			   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
		from #tmp
		where LastShift <> 'O'
		group by OutputDate, StdTMS
	),
	tmpTtlCPU as (
		select OutputDate
			   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3) 
		from #tmp
		where LastShift <> 'O'
		group by OutputDate
	),
	tmpSubconInCPU as (
		select OutputDate
			   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
		from #tmp
		where LastShift = 'I'
		group by OutputDate
	),
	tmpSubconOutCPU as (
		select OutputDate
			   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3) 
		from #tmp
		where LastShift = 'O'
		group by OutputDate
	),
	tmpTtlManPower as (
		select OutputDate
			   , ManPower = Sum(a.Manpower) - sum(iif(LastShift = 'I', 0, isnull(d.ManPower, 0)))
		from (
			select OutputDate
				   , FactoryID
				   , SewingLineID
				   , LastShift
				   , Team
				   , ManPower = Max(ActManPower)
			from #tmp
			where LastShift <> 'O'
			group by OutputDate, FactoryID, SewingLineID, LastShift, Team
		) a
		outer apply(
			select ManPower
			from (
				select OutputDate
					   , FactoryID
					   , SewingLineID
					   , LastShift
					   , Team
					   , ManPower = Max(ActManPower)
				from #tmp
				where LastShift <> 'O'
				group by OutputDate, FactoryID, SewingLineID, LastShift, Team
			) m2
			where m2.LastShift = 'I' 
				  and m2.Team = a.Team 
				  and m2.SewingLineID = a.SewingLineID	
				  and a.OutputDate = m2.OutputDate
				  and m2.FactoryID = a.FactoryID	
		) d
		group by OutputDate
	)
	select aDate.OutputDate
		   , QAQty = isnull (q.QAQty, 0)
		   , TotalCPU = isnull (tc.TotalCPU, 0)
		   , SInCPU = isnull (ic.TotalCPU, 0)
		   , SoutCPU = isnull (oc.TotalCPU, 0)
		   , CPUSewer = isnull (IIF(q.ManHour = 0, 0, isnull(tc.TotalCPU, 0) / q.ManHour), 0)
		   , AvgWorkHour = isnull (IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2)), 0)
		   , ManPower = isnull (mp.ManPower, 0)
		   , ManHour = isnull (q.ManHour, 0)
		   , Eff = isnull (IIF(q.ManHour * q.StdTMS = 0, 0, Round(tc.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2)), 0)
	       , q.StdTMS
	from AllOutputDate aDate
	left join tmpQty q on aDate.OutputDate = q.OutputDate
	left join tmpTtlCPU tc on aDate.OutputDate = tc.OutputDate
	left join tmpSubconInCPU ic on aDate.OutputDate = ic.OutputDate
	left join tmpSubconOutCPU oc on aDate.OutputDate = oc.OutputDate
	left join tmpTtlManPower mp on aDate.OutputDate = mp.OutputDate
	order by aDate.OutputDate
end
else if(@ReportType = 2)
begin
	;with AllSewingLine as (
	    select distinct SewingLineID		   
		from #tmp
	),
	tmpQty as (
		select SewingLineID
			   , StdTMS
			   , QAQty = Sum(QAQty)
			   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
		from #tmp
		where LastShift <> 'O'
		group by SewingLineID, StdTMS
	),
	tmpTtlCPU as (
		select SewingLineID
			   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
		from #tmp
		where LastShift <> 'O'
		group by SewingLineID
	),
	tmpSubconInCPU as (
		select SewingLineID
			   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
		from #tmp
		where LastShift = 'I'
		group by SewingLineID
	),
	tmpSubconOutCPU as (
		select SewingLineID
			   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
		from #tmp
		where LastShift = 'O'
		group by SewingLineID
	),
	tmpTtlManPower as (
		select SewingLineID
			   , ManPower = Sum(a.Manpower) - sum(iif(LastShift = 'I', 0, isnull(d.ManPower, 0)))
		from (
			select OutputDate
				   , FactoryID
				   , SewingLineID
				   , LastShift
				   , Team
				   , ManPower = Max(ActManPower)
			from #tmp
			where LastShift <> 'O'
			group by OutputDate, FactoryID, SewingLineID, LastShift, Team
		) a
		outer apply(
			select ManPower
			from (
				select OutputDate
					   , FactoryID
					   , SewingLineID
					   , LastShift
					   , Team
					   , ManPower = Max(ActManPower)
				from #tmp
				where LastShift <> 'O'
				group by OutputDate, FactoryID, SewingLineID, LastShift, Team
			) m2
			where m2.LastShift = 'I' 
				  and m2.Team = a.Team 
				  and m2.SewingLineID = a.SewingLineID	
				  and a.OutputDate = m2.OutputDate
		) d
		group by SewingLineID
	)
	select aLine.SewingLineID
		   , QAQty = isnull (q.QAQty, 0)
	 	   , TotalCPU = isnull (tc.TotalCPU, 0)
	 	   , SInCPU = isnull(ic.TotalCPU,0)
	 	   , SoutCPU = isnull(oc.TotalCPU,0)
	 	   , CPUSewer = isnull (IIF(q.ManHour = 0, 0, isnull(tc.TotalCPU,0) / q.ManHour), 0)
	 	   , AvgWorkHour = isnull (IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2)), 0)
	 	   , ManPower = isnull (mp.ManPower, 0)
	 	   , ManHour = isnull (q.ManHour, 0)
	 	   , Eff = isnull (IIF(q.ManHour * q.StdTMS = 0, 0, Round(tc.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2)), 0)
	from AllSewingLine aLine
	left join tmpQty q on aLine.SewingLineID = q.SewingLineID
	left join tmpTtlCPU tc on aLine.SewingLineID = tc.SewingLineID
	left join tmpSubconInCPU ic on aLine.SewingLineID = ic.SewingLineID
	left join tmpSubconOutCPU oc on aLine.SewingLineID = oc.SewingLineID
	left join tmpTtlManPower mp on aLine.SewingLineID = mp.SewingLineID
	order by iif(@OrderBy = 2, isnull (IIF(q.ManHour = 0, 0, isnull(tc.TotalCPU,0) / q.ManHour), 0), 1),
			 iif(@OrderBy = 1, aLine.SewingLineID, '1')
end
else
begin
	;with AllSewingLine as (
	    select distinct SewingLineID ,Team
		from #tmp
	),
	tmpQty as (
		select SewingLineID
			   , Team
			   , StdTMS
			   , QAQty = Sum(QAQty)
			   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
		from #tmp
		where LastShift <> 'O'
		group by SewingLineID, Team, StdTMS
	),
	tmpTtlCPU as (
		select SewingLineID
			   , Team
			   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
		from #tmp
		where LastShift <> 'O'
		group by SewingLineID, Team
	),
	tmpSubconInCPU as (
		select SewingLineID
			   , Team
			   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
		from #tmp
		where LastShift = 'I'
		group by SewingLineID, Team
	),
	tmpSubconOutCPU as (
		select SewingLineID
			   , Team
			   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
		from #tmp
		where LastShift = 'O'
		group by SewingLineID, Team
	),
	tmpTtlManPower as (
		select SewingLineID
			   , Team
			   , ManPower = Sum(a.Manpower) - sum(iif(LastShift = 'I', 0, isnull(d.ManPower, 0)))
		from (
			select OutputDate
				   , FactoryID
				   , SewingLineID
				   , LastShift
				   , Team
				   , ManPower = Max(ActManPower)
			from #tmp
			where LastShift <> 'O'
			group by OutputDate, FactoryID, SewingLineID, LastShift, Team
		) a
		outer apply(
			select ManPower
			from (
				select OutputDate
					   , FactoryID
					   , SewingLineID
					   , LastShift
					   , Team
					   , ManPower = Max(ActManPower)
				from #tmp
				where LastShift <> 'O'
				group by OutputDate, FactoryID, SewingLineID, LastShift, Team
			) m2
			where m2.LastShift = 'I' 
				  and m2.Team = a.Team 
				  and m2.SewingLineID = a.SewingLineID	
				  and a.OutputDate = m2.OutputDate
		) d
		group by SewingLineID, Team
	)
	select aLine.SewingLineID
		   , aLine.Team
		   , QAQty = isnull (q.QAQty, 0)
	 	   , TotalCPU = isnull (tc.TotalCPU, 0)
	 	   , SInCPU = isnull(ic.TotalCPU,0)
	 	   , SoutCPU = isnull(oc.TotalCPU,0)
	 	   , CPUSewer = isnull (IIF(q.ManHour = 0, 0, isnull(tc.TotalCPU,0) / q.ManHour), 0)
	 	   , AvgWorkHour = isnull (IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2)), 0)
	 	   , ManPower = isnull (mp.ManPower, 0)
	 	   , ManHour = isnull (q.ManHour, 0)
	 	   , Eff = isnull (IIF(q.ManHour * q.StdTMS = 0, 0, Round(tc.TotalCPU / (q.ManHour * 3600 / q.StdTMS) * 100, 2)), 0)	   
	from AllSewingLine aLine
	left join tmpQty q on aLine.SewingLineID = q.SewingLineID and aLine.Team = q.Team
	left join tmpTtlCPU tc on aLine.SewingLineID = tc.SewingLineID and aLine.Team = tc.Team
	left join tmpSubconInCPU ic on aLine.SewingLineID = ic.SewingLineID and aLine.Team = ic.Team
	left join tmpSubconOutCPU oc on aLine.SewingLineID = oc.SewingLineID and aLine.Team = oc.Team
	left join tmpTtlManPower mp on aLine.SewingLineID = mp.SewingLineID and aLine.Team = mp.Team
	order by aLine.SewingLineID, aLine.Team
end

drop table #tmpSewingDetail, #tmpSewingGroup, #tmp1stFilter, #tmp

end

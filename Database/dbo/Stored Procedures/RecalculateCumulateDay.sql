Create Procedure [dbo].[RecalculateCumulateDay]
(	
	 @Line as varchar(5)
	 , @FactoryID as varchar(8)
	 , @OutputDate as date
)
As
Begin
	SET NOCOUNT ON;

	if isnull(@Line, '') = '' or isnull(@FactoryID, '') = '' or @OutputDate is null
	begin
		return;
	end

	select distinct [OrderStyle] = ISNULL(o.StyleID, '')
		, [MockupStyle] = ISNULL(mo.StyleID, '')
		, s.SewingLineID
		, s.FactoryID
		, s.OutputDate
		, s.Shift
		, s.Team	
		, sd.ComboType	
		, m.MasterStyleID
		, m.MasterBrandID
	into #tmpSewingOutput_Base
	from SewingOutput s
	inner join SewingOutput_Detail sd on s.ID = sd.ID
	inner join Orders o on sd.OrderId = o.ID
	left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
	Outer apply (
		select distinct MasterBrandID, MasterStyleID 
		from (
			select MasterBrandID,MasterStyleID 
			from Style_SimilarStyle s2 WITH (NOLOCK) 
			where exists( select 1 from Style s with (nolock) 
							where s.Ukey = o.StyleUkey and s2.MasterStyleID = s.ID and s2.MasterBrandID = s.BrandID)
			union all
			select MasterBrandID,MasterStyleID
			from Style_SimilarStyle s2 WITH (NOLOCK) 
			where exists( select 1 from Style s with (nolock) 
							where s.Ukey = o.StyleUkey and s2.ChildrenStyleID = s.ID and s2.ChildrenBrandID = s.BrandID)
		)m
	)m
	where s.SewingLineID = @Line
	and s.FactoryID = @FactoryID
	and s.OutputDate = @OutputDate

	select s.OutputDate
		, sd.UKey
		, s.SewingLineID
		, s.FactoryID
		, [OrderStyle] = ISNULL(o.StyleID, '')
		, [MockupStyle] = ISNULL(mo.StyleID, '')
		, s.Shift
		, s.Team
		, sd.ComboType
		, o.StyleUkey
		, m.MasterStyleID
		, m.MasterBrandID
	into #tmpSewingGroup
	from SewingOutput s WITH (NOLOCK)
	inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
	inner join Orders o WITH (NOLOCK) on sd.OrderId = o.ID
	left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
	Outer apply (
		select distinct MasterBrandID, MasterStyleID 
		from (
			select MasterBrandID,MasterStyleID 
			from Style_SimilarStyle s2 WITH (NOLOCK) 
			where exists( select 1 from Style s with (nolock) 
							where s.Ukey = o.StyleUkey and s2.MasterStyleID = s.ID and s2.MasterBrandID = s.BrandID)
			union all
			select MasterBrandID,MasterStyleID
			from Style_SimilarStyle s2 WITH (NOLOCK) 
			where exists( select 1 from Style s with (nolock) 
							where s.Ukey = o.StyleUkey and s2.ChildrenStyleID = s.ID and s2.ChildrenBrandID = s.BrandID)
		)m
	)m
	where exists (select 1 from #tmpSewingOutput_Base t where s.SewingLineID = t.SewingLineID and s.FactoryID = t.FactoryID and s.Shift = t.Shift and s.Team = t.Team and sd.ComboType = t.ComboType
		and t.OrderStyle = isnull(o.StyleID, '') and t.MockupStyle = isnull(mo.StyleID, '') and s.OutputDate >= t.OutputDate
	)
	or exists (select 1 from #tmpSewingOutput_Base t where s.SewingLineID = t.SewingLineID and s.FactoryID = t.FactoryID and s.Shift = t.Shift and s.Team = t.Team and sd.ComboType = t.ComboType
		and t.MasterBrandID = m.MasterBrandID and t.MasterStyleID = m.MasterStyleID and s.OutputDate >= t.OutputDate

	)
	
	select [MaxOutputDate] = Max(OutputDate), [MinOutputDate] = MIN(OutputDate), MockupStyle, OrderStyle, SewingLineID, FactoryID, MasterStyleID, MasterBrandID 
	into #tmpOutputDate
	from(
		select distinct OutputDate, MockupStyle, OrderStyle, SewingLineID, FactoryID, MasterStyleID, MasterBrandID from #tmpSewingGroup
	) a
	group by MockupStyle, OrderStyle, SewingLineID, FactoryID, MasterStyleID, MasterBrandID

	select distinct t.FactoryID, t.SewingLineID ,t.OrderStyle, t.MockupStyle, s.OutputDate
	into #tmpSewingOutput
	from #tmpOutputDate t
	inner join SewingOutput s WITH (NOLOCK) on s.SewingLineID = t.SewingLineID and s.FactoryID = t.FactoryID and s.OutputDate between dateadd(day,-240, t.MinOutputDate) and t.MaxOutputDate
	where  exists(	select 1 from SewingOutput_Detail sd WITH (NOLOCK)
					left join Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
					left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
					where s.ID = sd.ID and (o.StyleID = t.OrderStyle or mo.StyleID = t.MockupStyle))
	or exists (select 1 from SewingOutput_Detail sd WITH (NOLOCK)
			left join Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
			Outer apply (
				select distinct MasterBrandID, MasterStyleID 
				from (
					select MasterBrandID,MasterStyleID 
					from Style_SimilarStyle s2 WITH (NOLOCK) 
					where exists( select 1 from Style s with (nolock) 
									where s.Ukey = o.StyleUkey and s2.MasterStyleID = s.ID and s2.MasterBrandID = s.BrandID)
					union all
					select MasterBrandID,MasterStyleID
					from Style_SimilarStyle s2 WITH (NOLOCK) 
					where exists( select 1 from Style s with (nolock) 
									where s.Ukey = o.StyleUkey and s2.ChildrenStyleID = s.ID and s2.ChildrenBrandID = s.BrandID)
				)m
			)m
			where s.ID = sd.ID and t.MasterStyleID = m.MasterStyleID and t.MasterBrandID = m.MasterBrandID
	)

	select w.FactoryID, w.SewingLineID ,t.OrderStyle, t.MockupStyle, w.Date
	into #tmpWorkHour
	from WorkHour w WITH (NOLOCK)
	left join #tmpOutputDate t on t.SewingLineID = w.SewingLineID and t.FactoryID = w.FactoryID and w.Date between dateadd(day,-240, t.MinOutputDate) and t.MaxOutputDate
	where w.Holiday=0 and isnull(w.Hours,0) != 0 
	and exists (select 1 from #tmpOutputDate where w.Date between dateadd(day,-240, MinOutputDate) and MaxOutputDate)
	and w.FactoryID = @FactoryID
	and w.SewingLineID = @Line

	select t.*
		, [CumulateDate_Before] = CumulateDate.val
	into #tmp1stFilter_First
	from #tmpSewingGroup t
	outer apply (	select val = IIF(Count(1)=0, 1, Count(1))
					from #tmpSewingOutput s
					where	s.FactoryID = t.FactoryID and
							s.MockupStyle = t.MockupStyle and
							s.OrderStyle = t.OrderStyle and
							s.SewingLineID = t.SewingLineID and
							s.OutputDate <= t.OutputDate and
							s.OutputDate >(
											select case when max(iif(s1.OutputDate is null, w.Date, null)) is not null then max(iif(s1.OutputDate is null, w.Date, null))
														--區間內都連續生產，第一天也要算是生產日，所以要減一天
														when min(w.Date) is not null then DATEADD(day, -1, min(w.Date))
														else t.OutputDate end
											from #tmpWorkHour w 
											left join #tmpSewingOutput s1 on s1.OutputDate = w.Date and
																			 s1.FactoryID = w.FactoryID and
																			 s1.MockupStyle = t.MockupStyle and
																			 s1.OrderStyle = t.OrderStyle and
																			 s1.SewingLineID = w.SewingLineID
											where	w.FactoryID = t.FactoryID and
													isnull(w.MockupStyle, t.MockupStyle) = t.MockupStyle and
													isnull(w.OrderStyle, t.OrderStyle) = t.OrderStyle and
													w.SewingLineID = t.SewingLineID and
													w.Date <= t.OutputDate
										)
	) CumulateDate

	select w.*
		, [RID] = ROW_NUMBER() over(partition by w.FactoryID, w.SewingLineID order by w.Date)
	into #tmpWorkHour_Factory
	from (
		select w.SewingLineID, w.FactoryID, w.Date
		from WorkHour w
		where w.Holiday = 0 and w.Hours != 0
		union --假日但有產出也要計算
		select t.SewingLineID, t.FactoryID, t.OutputDate
		from #tmp1stFilter_First t 
	) w 

	select t.*
		, [CumulateDate] = coalesce(t.CumulateDate_Before, 0)
		, [CumulateDateSimilar] = iif(t.MasterStyleID <> '', coalesce(t2.CumulateDateSimilar, 0), coalesce(t.CumulateDate_Before, 0))
	into #tmp1stFilter
	from #tmp1stFilter_First t 
	left join (
		select t.MasterBrandID
			, t.MasterStyleID
			, t.OutputDate
			, t.FactoryID
			, t.SewingLineID
			, [CumulateDateSimilar] = case when SEQ - (ROW_NUMBER() OVER (PARTITION BY t.MasterStyleID, t.MasterBrandID, t.FactoryID, t.SewingLineID, (RID - Seq) ORDER BY t.OutputDate, t.MasterStyleID, t.MasterBrandID, t.FactoryID, t.SewingLineID, RID) - 1) = 1 
					then t.CumulateDate_Max + ROW_NUMBER() OVER (PARTITION BY t.MasterStyleID, t.MasterBrandID, t.FactoryID, t.SewingLineID, (RID - Seq) ORDER BY t.OutputDate, t.MasterStyleID, t.MasterBrandID, t.FactoryID, t.SewingLineID, RID) - 1
				else ROW_NUMBER() OVER (PARTITION BY t.MasterStyleID, t.MasterBrandID, t.FactoryID, t.SewingLineID, (RID - Seq) ORDER BY t.OutputDate, t.MasterStyleID, t.MasterBrandID, t.FactoryID, t.SewingLineID, RID)
				end
		from 
		(
			select t.*
				, t2.CumulateDate_Max
				, [SEQ] = ROW_NUMBER() over(partition by t.MasterStyleID, t.MasterBrandID, t.FactoryID, t.SewingLineID order by t.OutputDate)
			from 
			(
				select distinct t.MasterStyleID, t.MasterBrandID, t.OutputDate, t.FactoryID, t.SewingLineID, w.RID
				from #tmp1stFilter_First t
				inner join #tmpWorkHour_Factory w on w.FactoryID = t.FactoryID and w.Date = t.OutputDate and w.SewingLineID = t.SewingLineID
				where MasterStyleID <> ''
			)t
			outer apply (
				select MasterStyleID, MasterBrandID, FactoryID, OutputDate, SewingLineID
					, [CumulateDate_Max] = MAX(CumulateDate_Before)
				from #tmp1stFilter_First t2
				where t.MasterStyleID = t2.MasterStyleID
				and t.MasterBrandID = t2.MasterBrandID
				and t.FactoryID = t2.FactoryID
				and t.SewingLineID = t2.SewingLineID
				and OutputDate in (
					select MIN(OutputDate)
					from #tmp1stFilter_First a
					where a.MasterStyleID = t2.MasterStyleID
					and a.MasterBrandID = t2.MasterBrandID	
					and a.FactoryID = t2.FactoryID
					and a.SewingLineID = t2.SewingLineID
				)
				group by MasterStyleID, MasterBrandID, FactoryID, OutputDate, SewingLineID
			) t2
		)t
	)t2 on t.MasterStyleID = t2.MasterStyleID and t.MasterBrandID = t2.MasterBrandID and t.FactoryID = t2.FactoryID and t.OutputDate = t2.OutputDate and t.SewingLineID = t2.SewingLineID
	order by t.OutputDate, t.UKey

	update s
		set s.Cumulate = t.CumulateDate
			, s.CumulateSimilar = t.CumulateDateSimilar
	from SewingOutput_Detail s
	inner join #tmp1stFilter t on s.UKey = t.UKey


	drop table #tmpOutputDate, #tmpSewingGroup, #tmpSewingOutput, #tmpSewingOutput_Base, #tmpWorkHour, #tmp1stFilter_First, #tmpWorkHour_Factory
END
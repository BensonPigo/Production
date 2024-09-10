-- =============================================
-- Description:	資料來源SewingSchedule + P_LineMapping
-- =============================================
CREATE PROCEDURE [dbo].[P_Import_LineBalancingRate]
	@StartDate date
AS

BEGIN	
	SET NOCOUNT ON;

	declare @SDate date = CONVERT(date, @StartDate)
	declare @EDate date = CONVERT(date,DATEADD(day,7, @StartDate)) 

	select distinct SewingDate = CONVERT(date,@SDate),FactoryID  
	into #tmpMain
	from  [MainServer].Production.dbo.SewingSchedule s
	where @SDate >=CONVERT(date, s.Inline) and @EDate <= CONVERT(date, s.Offline)

	create table #P_LineBalancingRate
	(
		SewingDate date,
		FactoryID varchar(8),
		[Total SP Qty] int,
		[Total LBR] numeric(7,2),
		[Avg. LBR] numeric(7,2),
		[Total SP Qty In7Days] int,
		[Total LBR In7Days] numeric(12,2),
		[Avg. LBR In7Days] numeric(7,2)
	)

	insert #P_LineBalancingRate(SewingDate,FactoryID,[Total SP Qty],[Total LBR],[Avg. LBR])
	select t.SewingDate,t.FactoryID
	,[Total SP Qty] = s.SPCnt
	,[Total LBR] = s.LBR
	,[Avg. LBR] = ROUND(s.LBR /  s.SPCnt,2)
	from #tmpMain t
	left join(
		select s.FactoryID,SPCnt = count(1),LBR = sum(LBR.value)
		from [MainServer].Production.dbo.SewingSchedule s with(nolock)
		inner join [MainServer].Production.dbo.orders o with(nolock) on s.OrderId = o.id			
		outer apply(
			select value = SUM([LBR By Cycle Time(%)])
			from (
				select s.FactoryID,s.ComboType,o.StyleUkey,rowno = ROW_NUMBER() over(partition by s.FactoryID,o.StyleUKey,s.ComboType order by version desc)
				,[LBR By Cycle Time(%)] = AVG([LBR By Cycle Time(%)]) over (partition by s.FactoryID,o.StyleUKey,s.ComboType,lm.SewingLine order by version desc)
				from [MainServer].Production.dbo.SewingSchedule s with(nolock)
				inner join [MainServer].Production.dbo.orders o with(nolock) on s.OrderId = o.id	
				left join P_LineMapping lm with(nolock) 
					on lm.StyleUKey = o.StyleUkey 
					and  s.FactoryID = lm.Factory	
					and s.ComboType = lm.ComboType 		
				where 1=1
				and CONVERT(date,@SDate) between convert(date,s.Inline) and CONVERT(date,s.Offline)
				and lm.Phase = 'Final'
			) a
			where rowno=1
			and a.FactoryID = s.FactoryID and a.ComboType = s.ComboType and a.StyleUkey = o.StyleUkey
		)LBR
		where 1=1
		and CONVERT(date,@SDate) between convert(date,Inline) and CONVERT(date, Offline)
		and exists(
			select 1 from P_LineMapping lm with(nolock) 
			where lm.StyleUKey = o.StyleUkey 
			and  s.FactoryID = lm.Factory	
			and s.ComboType = lm.ComboType 
			and lm.Phase = 'Final'
		)
		group by s.FactoryID
	) s on s.FactoryID = t.FactoryID
	option (recompile)

	/*
	跑迴圈一天一天加總塞資料
	避免用Range會無法算到相同訂單資料
	*/
	DECLARE @Day INT = 1;

	WHILE @Day <= 7
	BEGIN	
		select   t.SewingDate,t.FactoryID,t.[Total SP Qty],t.[Total LBR],t.[Avg. LBR]
		,[Total SP Qty In7Days] = s.SPCnt
		,[Total LBR In7Days] = s.LBR
		into #tmpLoop
		from #P_LineBalancingRate t
		left join(
			select s.FactoryID,SPCnt = count(1),LBR = sum(LBR.value)
			from [MainServer].Production.dbo.SewingSchedule s with(nolock)
			inner join [MainServer].Production.dbo.orders o with(nolock) on s.OrderId = o.id			
			outer apply(
				select value = SUM([LBR By Cycle Time(%)])
				from (
					select s.FactoryID,s.ComboType,o.StyleUkey,rowno = ROW_NUMBER() over(partition by s.FactoryID,o.StyleUKey,s.ComboType order by version desc)
					,[LBR By Cycle Time(%)] = AVG([LBR By Cycle Time(%)]) over (partition by s.FactoryID,o.StyleUKey,s.ComboType,lm.SewingLine order by version desc)
					from [MainServer].Production.dbo.SewingSchedule s with(nolock)
					inner join [MainServer].Production.dbo.orders o with(nolock) on s.OrderId = o.id	
					left join P_LineMapping lm with(nolock) 
						on lm.StyleUKey = o.StyleUkey 
						and  s.FactoryID = lm.Factory	
						and s.ComboType = lm.ComboType 		
					where 1=1
					and CONVERT(date,DATEADD(day,@Day, @SDate)) between convert(date,s.Inline) and CONVERT(date,s.Offline)
					and lm.Phase = 'Final'
				) a
				where rowno=1
				and a.FactoryID = s.FactoryID and a.ComboType = s.ComboType and a.StyleUkey = o.StyleUkey
			)LBR
			where 1=1
			and CONVERT(date,DATEADD(day,@Day, @SDate)) between convert(date,Inline) and CONVERT(date, Offline)
			and exists(
				select 1 from P_LineMapping lm with(nolock) 
				where lm.StyleUKey = o.StyleUkey 
				and  s.FactoryID = lm.Factory	
				and s.ComboType = lm.ComboType 
				and lm.Phase = 'Final'
			)
			group by s.FactoryID
		) s on s.FactoryID = t.FactoryID
		option (recompile)

		update t
		set [Total SP Qty In7Days] = isnull(t.[Total SP Qty In7Days],0) + s.[Total SP Qty In7Days]
		,[Total LBR In7Days] = isnull(t.[Total LBR In7Days],0) + s.[Total LBR In7Days]
		from #P_LineBalancingRate t
		inner join #tmpLoop s on t.FactoryID = s.FactoryID and t.SewingDate = s.SewingDate

		drop table #tmpLoop

		-- 增加日期和天數
		SET @Day = @Day + 1;
	END

	-- 計算加總後未來七天的Avg

	update t
	set [Avg. LBR In7Days]  = ROUND(convert(float,[Total LBR In7Days]) / [Total SP Qty In7Days],2)
	from #P_LineBalancingRate t


	delete t
	from P_LineBalancingRate t
	where not exists(
		select 1 from #P_LineBalancingRate s
		where s.SewingDate = t.SewingDate
		and s.FactoryID = t.FactoryID
	)
	and SewingDate = @SDate

	update t
	set t.[Total SP Qty] = s.[Total SP Qty] 
	,t.[Total LBR] = s.[Total LBR]
	,t.[Avg. LBR] = s.[Avg. LBR]
	,t.[Total SP Qty In7Days] = s.[Total SP Qty In7Days]
	,t.[Total LBR In7Days] = s.[Total LBR In7Days]
	,t.[Avg. LBR In7Days] = s.[Avg. LBR In7Days]
	from P_LineBalancingRate t
	inner join #P_LineBalancingRate s 
		on s.SewingDate = t.SewingDate
		and s.FactoryID = t.FactoryID

	insert into P_LineBalancingRate(
	[SewingDate]
	,[FactoryID]
	,[Total SP Qty]
	,[Total LBR]
	,[Avg. LBR]
	,[Total SP Qty In7Days]
	,[Total LBR In7Days]
	,[Avg. LBR In7Days]
	)
	select  
	[SewingDate]
	,[FactoryID]
	,[Total SP Qty]
	,[Total LBR]
	,[Avg. LBR]
	,[Total SP Qty In7Days]
	,[Total LBR In7Days]
	,[Avg. LBR In7Days]
	from #P_LineBalancingRate t
	where not exists(
		select 1 from P_LineBalancingRate s
		where s.SewingDate = t.SewingDate
		and s.FactoryID = t.FactoryID
	)

	if exists(select 1 from BITableInfo where Id = 'P_LineBalancingRate')
	begin
		update BITableInfo set TransferDate = @SDate
		where Id = 'P_LineBalancingRate'
	end
	else
	begin
		insert into BITableInfo(Id, TransferDate, IS_Trans) values('P_LineBalancingRate', @SDate, 0)
	end


	drop table #tmpMain,#P_LineBalancingRate

end
GO


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
	from  [MainServer].Production.dbo.SewingSchedule s with(nolock)
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
	,[Total SP Qty] = isnull(s.SPCnt,0)
	,[Total LBR] = isnull(s.LBR,0)
	,[Avg. LBR] = iif(isnull(s.spcnt,0) = 0 ,0,  ROUND(isnull(s.LBR,0) /  isnull(s.SPCnt,0), 2))
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
					and  s.FactoryID = lm.FactoryID	
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
			and  s.FactoryID = lm.FactoryID	
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

	-- 先創建臨時表，避免多次創建和刪除臨時表
	CREATE TABLE #tmpLoop 
	(
		SewingDate DATE,
		FactoryID varchar(8),
		[Total SP Qty] INT,
		[Total LBR] numeric(7,2),
		[Avg. LBR] numeric(7,2),
		[Total SP Qty In7Days] INT,
		[Total LBR In7Days] numeric(12,2),
	);


	WHILE @Day <= 7
	BEGIN    
		-- 將結果插入臨時表
		INSERT INTO #tmpLoop (SewingDate, FactoryID, [Total SP Qty], [Total LBR], [Avg. LBR], [Total SP Qty In7Days], [Total LBR In7Days])
		SELECT 
			t.SewingDate,
			t.FactoryID,
			t.[Total SP Qty],
			t.[Total LBR],
			t.[Avg. LBR],
			s.SPCnt AS [Total SP Qty In7Days],
			s.LBR AS [Total LBR In7Days]
		FROM #P_LineBalancingRate t
		LEFT JOIN (
			SELECT 
				s.FactoryID,
				COUNT(1) AS SPCnt,
				SUM(LBR.value) AS LBR
			FROM [MainServer].Production.dbo.SewingSchedule s WITH(NOLOCK)
			INNER JOIN [MainServer].Production.dbo.orders o WITH(NOLOCK) 
				ON s.OrderId = o.id            
			OUTER APPLY (
				SELECT 
					SUM([LBR By Cycle Time(%)]) AS value
				FROM (
					SELECT 
						s.FactoryID,
						s.ComboType,
						o.StyleUkey,
						ROW_NUMBER() OVER(PARTITION BY s.FactoryID, o.StyleUKey, s.ComboType ORDER BY version DESC) AS rowno,
						AVG([LBR By Cycle Time(%)]) OVER(PARTITION BY s.FactoryID, o.StyleUKey, s.ComboType, lm.SewingLine ORDER BY version DESC) AS [LBR By Cycle Time(%)]
					FROM [MainServer].Production.dbo.SewingSchedule s WITH(NOLOCK)
					INNER JOIN [MainServer].Production.dbo.orders o WITH(NOLOCK) 
						ON s.OrderId = o.id    
					LEFT JOIN P_LineMapping lm WITH(NOLOCK) 
						ON lm.StyleUKey = o.StyleUkey 
						AND s.FactoryID = lm.FactoryID    
						AND s.ComboType = lm.ComboType         
					WHERE 
						CONVERT(DATE, DATEADD(DAY, @Day, @SDate)) BETWEEN CONVERT(DATE, s.Inline) AND CONVERT(DATE, s.Offline)
						AND lm.Phase = 'Final'
				) a
				WHERE rowno = 1
				AND a.FactoryID = s.FactoryID 
				AND a.ComboType = s.ComboType 
				AND a.StyleUkey = o.StyleUkey
			) LBR
			WHERE 
				CONVERT(DATE, DATEADD(DAY, @Day, @SDate)) BETWEEN CONVERT(DATE, s.Inline) AND CONVERT(DATE, s.Offline)
				AND EXISTS (
					SELECT 1 
					FROM P_LineMapping lm WITH(NOLOCK)
					WHERE lm.StyleUKey = o.StyleUkey 
					AND s.FactoryID = lm.FactoryID    
					AND s.ComboType = lm.ComboType 
					AND lm.Phase = 'Final'
				)
			GROUP BY s.FactoryID
		) s ON s.FactoryID = t.FactoryID
		OPTION (RECOMPILE);

		-- 更新主表中的累計數據
		UPDATE t
		SET 
			[Total SP Qty In7Days] = ISNULL(t.[Total SP Qty In7Days], 0) + ISNULL(s.[Total SP Qty In7Days], 0),
			[Total LBR In7Days] = ISNULL(t.[Total LBR In7Days], 0) + ISNULL(s.[Total LBR In7Days], 0)
		FROM #P_LineBalancingRate t
		INNER JOIN #tmpLoop s 
			ON t.FactoryID = s.FactoryID 
			AND t.SewingDate = s.SewingDate;

		-- 清空臨時表內容以準備下一次迴圈
		TRUNCATE TABLE #tmpLoop;

		-- 增加日期和天數
		SET @Day = @Day + 1;
	END

	-- 計算加總後未來七天的Avg

	update t
	set [Avg. LBR In7Days]  = iif(isnull([Total SP Qty In7Days],0) = 0 , 0, ROUND(convert(float,[Total LBR In7Days]) / [Total SP Qty In7Days],2))
	, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
    , [BIInsertDate] = GetDate()
	from #P_LineBalancingRate t

	Select * from  #P_LineBalancingRate

	drop table #tmpMain,#P_LineBalancingRate,#tmpLoop
end;


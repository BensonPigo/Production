-- =============================================
-- Description:	轉入P_StyleChangeover當日前後七天內的Inline資料
-- =============================================
CREATE PROCEDURE P_Import_Changeover
	@StartDate date,
	@EndDate date	
AS
BEGIN	
	SET NOCOUNT ON;

	declare @SDate date = @StartDate
	declare @EDate date = @EndDate

	-- 固定Sdate & Edatye 正負七天
	if @StartDate is null
	begin
		set @SDate =  CONVERT(date,DATEADD(day,-7, GETDATE()))	
	end

	if @EndDate is null
	begin 
		set @EDate =  CONVERT(date,DATEADD(day,7, GETDATE())) 
	end

	

	-- 遞迴取出今天-7天包含今日 共8天日期
	DECLARE  @t TABLE
	(
	StartDate date,
	EndDate date
	);

	INSERT  INTO  @t
			( StartDate, EndDate )
	VALUES  ( @SDate,   @EDate	);

	;WITH CTE (Dates,EndDate) AS
	(
		SELECT StartDate AS Dates,EndDate AS EndDate
		FROM @t
		UNION ALL --注意這邊使用 UNION ALL
		SELECT DATEADD(DAY,1,Dates),EndDate
		FROM CTE 
		WHERE DATEADD(DAY,1,Dates) <= EndDate --判斷是否目前遞迴月份小於結束日期
	),
	F as(
		select distinct Factory from P_StyleChangeover with(nolock)
	)
	SELECT Dates,Factory
	into #tmpAllDate
	FROM CTE, F
	OPTION (MAXRECURSION 0); -- 如果日期範圍很大，需要設置遞迴的最大次數


	select 
	 [TransferDate] = d.Dates
	,[FactoryID] = Factory
	,[ChgOverInTransferDate]  = isnull(
	(
		SELECT COUNT(*) 
		FROM P_StyleChangeover with(nolock)
		WHERE CONVERT(DATE, Inline) = CONVERT(DATE, d.Dates)
	),0)
	,[ChgOverIn1Day]  = isnull(
	(
		SELECT COUNT(*) 
		FROM P_StyleChangeover with(nolock)
		WHERE CONVERT(DATE, Inline) = DATEADD(DAY, 1, CONVERT(DATE, d.Dates))
	),0)
	,[ChgOverIn7Days] = isnull(
	(
		SELECT COUNT(*) 
		FROM P_StyleChangeover with(nolock)
		WHERE CONVERT(DATE, Inline) >= DATEADD(DAY, 1, CONVERT(DATE, d.Dates))
		AND CONVERT(DATE, Inline) <= DATEADD(DAY, 7, CONVERT(DATE, d.Dates))
	),0)
	,[COTInPast1Day] = isnull(
	(
		SELECT CONVERT(numeric(8,2),AVG(isnull([COT(min)],0))) 
		FROM P_StyleChangeover with(nolock)
		WHERE CONVERT(DATE, Inline) = DATEADD(DAY, -1, CONVERT(DATE, d.Dates))
	),0)
	,[COTInPast7Days] = isnull(
	(
		SELECT CONVERT(numeric(8,2),AVG(isnull([COT(min)],0))) 
		FROM P_StyleChangeover with(nolock)
		WHERE CONVERT(DATE, Inline) BETWEEN DATEADD(DAY, -7, CONVERT(DATE, d.Dates)) 
		AND DATEADD(DAY, -1, CONVERT(DATE, d.Dates))
	),0)
	,[COPTInPast1Day] = isnull(
	(
		SELECT CONVERT(numeric(8,2), AVG(isnull([COPT(min)],0))) 
		FROM P_StyleChangeover with(nolock)
		WHERE CONVERT(DATE, Inline) = DATEADD(DAY, -1, CONVERT(DATE, d.Dates))
	),0)
	,[COPTInPast7Days] = isnull(
	(
		SELECT CONVERT(numeric(8,2),AVG(isnull([COPT(min)],0))) 
		FROM P_StyleChangeover with(nolock)
		WHERE CONVERT(DATE, Inline) BETWEEN DATEADD(DAY, -7, CONVERT(DATE, d.Dates)) 
		AND DATEADD(DAY, -1, CONVERT(DATE, d.Dates))
	),0)
	into #tmpFinal
	from #tmpAllDate d
	where CONVERT(DATE, d.Dates) <= CONVERT(date,GETDATE()) -- 為了滿足只抓包含當天-7天 共8日資料

	-- 寫入P_Changeover
	
	delete t
	from P_Changeover t
	where not exists(
		select 1 from #tmpFinal s
		where t.FactoryID = s.FactoryID
		and t.TransferDate = s.TransferDate
	)
	and t.TransferDate between @SDate and @EndDate

	update t 
	set      t.TransferDate						   = s.TransferDate					
			,t.[FactoryID]						   = s.[FactoryID]					
			,t.[ChgOverInTransferDate]			   = s.[ChgOverInTransferDate]					
			,t.[ChgOverIn1Day]					   = s.[ChgOverIn1Day]					
			,t.[ChgOverIn7Days]					   = s.[ChgOverIn7Days]					
			,t.[COTInPast1Day]					   = s.[COTInPast1Day]							
			,t.[COTInPast7Days]					   = s.[COTInPast7Days]					
			,t.[COPTInPast1Day]					   = s.[COPTInPast1Day]			
			,t.[COPTInPast7Days]				   = s.[COPTInPast7Days]
	from P_Changeover t
	inner join #tmpFinal s on t.FactoryID = s.FactoryID and t.TransferDate = s.TransferDate

	insert into P_Changeover([TransferDate]
		  ,[FactoryID]
		  ,[ChgOverInTransferDate]
		  ,[ChgOverIn1Day]
		  ,[ChgOverIn7Days]
		  ,[COTInPast1Day]
		  ,[COTInPast7Days]
		  ,[COPTInPast1Day]
		  ,[COPTInPast7Days])
	SELECT [TransferDate]
		  ,[FactoryID]
		  ,[ChgOverInTransferDate]
		  ,[ChgOverIn1Day]
		  ,[ChgOverIn7Days]
		  ,[COTInPast1Day]
		  ,[COTInPast7Days]
		  ,[COPTInPast1Day]
		  ,[COPTInPast7Days]
	 from #tmpFinal t
	 where not exists(
			select 1 from P_Changeover s
			where t.FactoryID = s.FactoryID 
			and t.TransferDate = s.TransferDate
	  )

	if exists(select 1 from BITableInfo where Id = 'P_Changeover')
	begin
		update BITableInfo set TransferDate = getdate()
		where Id = 'P_Changeover'
	end
	else
	begin
		insert into BITableInfo(Id, TransferDate, IS_Trans) values('P_Changeover', GETDATE(), 0)
	end

	drop table #tmpAllDate,#tmpFinal
END
GO
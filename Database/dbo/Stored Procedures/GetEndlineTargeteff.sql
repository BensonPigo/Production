CREATE PROCEDURE [dbo].[GetEndlineTargeteff]
AS
BEGIN
	/*宣告變數*/
	DECLARE @APSDate Datetime = (Select MAX(LastDownloadAPSDate)  From Factory WHERE Junk = 0); --抓現在APS的時間
	DECLARE @MAX_TargetEff_APSDate Datetime = (Select MAX(APSDate)  From [ExtendServer].[ManufacturingExecution].dbo.TargetEffUpdateByAps); -- TargetEffUpdateByAps.APSDate 最大值
	DECLARE @tmpCount int ;

	/* 撈變數 @APSDate,@RgCode,@tmpCount,@MAX_TargetEff_APSDate */
	SELECT 
	 [APSNo]
	,[FactoryID]
	,[SewingDay]
	,[SewingLine] = SewingLineID
	,[StyleID] = StyleID
	,StdOutput
	,SewingCPU
	,Sewer
	,WorkingTime 
	,SewingStart
	,[RowNum] = ROW_NUMBER() OVER(PARTITION BY SewingLineID, FactoryID, StyleID, SewingDay ORDER BY SewingEnd desc)
	into #tmp
	FROM dbo.GetSewingLineScheduleDataBase(GETDATE(),GETDATE(),default,default,default,default, default, default, default, default, default)
	WHERE SEWINGDAY = FORMAT(GETDATE(), 'yyyy-MM-dd') --AND SewingLineID = 'A35' and StyleID = 'LW6CUUS'

	SELECT
	 [APSNo]
	,[FactoryID]
	,[SewingDay]
	,[SewingLine]
	,[StyleID]
	,[TargetEff]= CONVERT(FLOAT, ROUND((SUM(ROUND(StdOutput, 0)) * ROUND(SewingCPU, 5)) / (Sewer * SUM(ROUND(WorkingTime, 5)) * 2.57),4)) * 100
	,[APSDate] = @APSDate
	--,[RowNum]
	INTO #tmp_Effi
	FROM #tmp
	WHERE SEWINGDAY = FORMAT(GETDATE(), 'yyyy-MM-dd') and RowNum = 1
	Group BY SewingCPU,Sewer,SewingLine,StyleID,[SewingDay],[FactoryID],[APSNo],[RowNum]

	SELECT 
	 [APSNo] = T.APSNo
	,[FactoryID] = T.FactoryID
	,[Date] = T.SewingDay
	,[SewingLine] = T.SewingLine
	,[StyleID] = T.StyleID
	,[TargetEff] = T.TargetEff
	,[APSDate] = T.APSDate
	into #tmpEnd
	FROM #tmp_Effi T WITH(NOLOCK)
	LEFT JOIN [ExtendServer].[ManufacturingExecution].dbo.TargetEffUpdateByAps TEU WITH(NOLOCK) ON TEU.APSNo = T.APSNo AND TEU.FactoryID = T.FactoryID and TEU.[Date] = T.SewingDay AND TEU.SewingLine = T.SewingLine AND TEU.StyleID = T.StyleID AND TEU.[Date] = GETDATE()
	WHERE TEU.APSNo IS NULL

	SET @tmpCount =(Select count(1)  from #tmpEnd)

	/*
		判斷APS是否有被更新及Style是否有換款
	*/
	if ((@APSDate > @MAX_TargetEff_APSDate) or (@tmpCount > 0))
	BEGIN
		-- SELECT @APSDate,@RgCode,@tmpCount,@MAX_TargetEff_APSDate 
		UPDATE TEU 
		SET
		[APSNo] 		 = ISNULL(T.APSNo,0)
		,[TargetEff]	 = ISNULL(T.TargetEff,'')
		,[APSDate]		 = @APSDate
		-- SELECT *
		FROM [ExtendServer].[ManufacturingExecution].dbo.TargetEffUpdateByAps TEU
		INNER JOIN #tmpEnd T ON TEU.FactoryID = T.FactoryID and TEU.[Date] = T.[Date] AND TEU.SewingLine = T.SewingLine AND TEU.StyleID = T.StyleID


		INSERT INTO [ExtendServer].[ManufacturingExecution].dbo.TargetEffUpdateByAps
		(
			[APSNo]
		  ,[FactoryID]
		  ,[Date]
		  ,[SewingLine]
		  ,[StyleID]
		  ,[TargetEff]
		  ,[APSDate]
		)
		SELECT 
		[APSNo] 		= ISNULL(T.APSNo,0)
		,[FactoryID]	= ISNULL(T.FactoryID,'')
		,[Date]			=  T.[DATE]
		,[SewingLine]	= ISNULL(T.SewingLine,'')
		,[StyleID]  		= ISNULL(T.StyleID,'')
		,[TargetEff]	= ISNULL(T.TargetEff,0)
		,[APSDate]		= T.APSDate
		FROM #tmpEnd T
		where not exists (
			select 1 from [ExtendServer].[ManufacturingExecution].dbo.TargetEffUpdateByAps TEU 
			where TEU.FactoryID = T.FactoryID and TEU.[Date] = T.[Date] AND TEU.SewingLine = T.SewingLine AND TEU.StyleID = T.StyleID
		)

	END
	DROP TABLE #tmp,#tmp_Effi,#tmpEnd
end
GO

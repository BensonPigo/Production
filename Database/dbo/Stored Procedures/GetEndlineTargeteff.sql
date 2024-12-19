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
	into #tmp
	FROM dbo.GetSewingLineScheduleDataBase(GETDATE(),GETDATE(),default,default,default,default, default, default, default, default, default)
	WHERE SEWINGDAY = FORMAT(GETDATE(), 'yyyy-MM-dd')

	SELECT
	 [FactoryID]
	,[SewingDay]
	,[SewingLine]
	,[StyleID]
	,[TargetEff]= CONVERT(FLOAT, ROUND((SUM(ROUND(StdOutput, 0)) * ROUND(SUM(SewingCPU), 5)) / (Sewer * SUM(ROUND(WorkingTime, 5)) * 2.57),4)) * 100
	,[APSDate] = @APSDate
	INTO #tmp_Effi
	FROM #tmp
	WHERE SEWINGDAY = FORMAT(GETDATE(), 'yyyy-MM-dd')
	Group BY Sewer,SewingLine,StyleID,[SewingDay],[FactoryID]


	SELECT
	 [APSNo] = tmp.APSNo
	,[FactoryID] = tmp.FactoryID
	,[Date] = tmp.[Date]
	,[SewingLine] = tmp.SewingLine
	,[StyleID] = tmp.StyleID
	,[TargetEff] = tmp.TargetEff
	,[APSDate] = tmp.APSDate
	INTO #tmpEnd
	FROM
	(
		SELECT 
		 [APSNo] = APSNo.val
		,[FactoryID] = T.FactoryID
		,[Date] = T.SewingDay
		,[SewingLine] = T.SewingLine
		,[StyleID] = T.StyleID
		,[TargetEff] = T.TargetEff
		,[APSDate] = T.APSDate
		FROM #tmp_Effi T WITH(NOLOCK)
		OUTER APPLY
		(
			SELECT val = stuff(
			(
			select concat(',',APSNo)
			FROM
			(
				SELECT APSNo
					FROM #TMP TT
					WHERE TT.FactoryID = T.FactoryID AND
							TT.SewingDay = T.SewingDay AND
							TT.SewingLine = T.SewingLine AND
							TT.StyleID = T.StyleID
			)
			tmp for xml path('')),1,1,'')
		)APSNo
	)tmp
	LEFT JOIN [ExtendServer].[ManufacturingExecution].dbo.TargetEffUpdateByAps TEU WITH(NOLOCK) ON TEU.APSNo = tmp.APSNo AND TEU.FactoryID = tmp.FactoryID and TEU.[Date] = tmp.[Date] AND TEU.SewingLine = tmp.SewingLine AND TEU.StyleID = tmp.StyleID AND TEU.[Date] = GETDATE()
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

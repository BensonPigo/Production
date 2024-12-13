-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/22>
-- Description:	<import part>
-- =============================================
CREATE PROCEDURE [dbo].[imp_MESbase]
AS
BEGIN

	SELECT 
	Type
	,ID
	,Description
	,Junk
	,AddName
	,AddDate
	,EditName
	,EditDate
	INTO #Trade_DQSReason
	FROM [Mainserver].Trade_To_Pms.dbo.DQSReason
	;

	INSERT INTO ManufacturingExecution.dbo.DQSReason 
			   ([Type]
			   ,[ID]
			   ,[Description]
			   ,[Junk]
			   ,[AddName]
			   ,[AddDate]
			   ,[EditName]
			   ,[EditDate])
	SELECT 
				 isnull(s.Type       , '')
				,isnull(s.ID         , '')
				,isnull(s.Description, '')
				,isnull(s.Junk       , 0)
				,isnull(s.AddName    , '')
				,s.AddDate
				,isnull(s.EditName   , '')
				,s.EditDate
	FROM #Trade_DQSReason s WITH (NOLOCK) 
	WHERE NOT EXISTS(SELECT 1 FROM ManufacturingExecution.dbo.DQSReason t WHERE t.ID=s.ID AND t.Type = s.Type)


	UPDATE t
	SET   t.Description = isnull( s.Description, '')
		, t.Junk = isnull( s.Junk              , 0)
		, t.AddName = isnull( s.AddName        , '')
		, t.AddDate =  s.AddDate
		, t.EditName = isnull( s.EditName      , '')
		, t.EditDate = s.EditDate
	FROM ManufacturingExecution.dbo.DQSReason t
	INNER JOIN #Trade_DQSReason s ON t.ID=s.ID AND t.Type = s.Type

	
	DROP TABLE #Trade_DQSReason


	----------MeasurementTranslate----------
	
	SELECT *
	INTO #Trade_MeasurementTranslate
	FROM [Mainserver].Trade_To_Pms.dbo.MeasurementTranslate
	;	
	INSERT INTO ManufacturingExecution.dbo.MeasurementTranslate 
			   (Ukey
			   ,DescEN
			   ,DescCHS
			   ,DescVN
			   ,DescKH
			   ,[AddName]
			   ,[AddDate]
			   ,[EditName]
			   ,[EditDate])
	SELECT 
				 isnull(s.Ukey    , 0)
				,isnull(s.DescEN  , '')
				,isnull(s.DescCHS , '')
				,isnull(s.DescVN  , '')
				,isnull(s.DescKH  , '')
				,isnull(s.AddName , '')
				,s.AddDate
				,isnull(s.EditName, '')
				,s.EditDate
	FROM #Trade_MeasurementTranslate s WITH (NOLOCK) 
	WHERE NOT EXISTS(SELECT 1 FROM ManufacturingExecution.dbo.MeasurementTranslate t WHERE t.Ukey=s.Ukey)
	;
	UPDATE t
	SET   t.DescEN = isnull( s.DescEN    , '')
		, t.DescCHS = isnull( s.DescCHS  , '')
		, t.DescVN = isnull( s.DescVN    , '')
		, t.DescKH = isnull( s.DescKH    , '')
		, t.AddName = isnull( s.AddName  , '')
		, t.AddDate = s.AddDate
		, t.EditName = isnull( s.EditName, '')
		, t.EditDate = s.EditDate
	FROM ManufacturingExecution.dbo.MeasurementTranslate t
	INNER JOIN #Trade_MeasurementTranslate s ON t.Ukey=s.Ukey
	;

	SELECT t.Ukey
		, [DescSHA1] = HASHBYTES('SHA1', ManufacturingExecution.dbo.MeasurementTrim(t.Description))
	INTO #tmp_Measurement
	FROM ManufacturingExecution.dbo.Measurement t WITH (NOLOCK) 
	WHERE t.MeasurementTranslateUkey = 0
	AND t.AddDate >= DATEADD(MONTH, -6, GETDATE())

	SELECT [MeasurementTranslateUkey] = Ukey
		, [DescSHA1] = HASHBYTES('SHA1', ManufacturingExecution.dbo.MeasurementTrim(mt.DescEN))
	INTO #tmp_MeasurementTranslate
	FROM ManufacturingExecution.dbo.MeasurementTranslate mt WITH (NOLOCK) 
	 
	UPDATE m 
		SET m.MeasurementTranslateUkey = isnull(tm.MeasurementTranslateUkey, 0)
	FROM ManufacturingExecution.dbo.Measurement m WITH (NOLOCK) 
	INNER JOIN #tmp_Measurement t on m.Ukey = t.Ukey
	INNER JOIN #tmp_MeasurementTranslate tm on t.DescSHA1 = tm.DescSHA1


	DROP TABLE #tmp_Measurement, #tmp_MeasurementTranslate
	DROP TABLE #Trade_MeasurementTranslate
END

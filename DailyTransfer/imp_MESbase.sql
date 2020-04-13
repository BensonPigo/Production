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
				 s.Type 
				,s.ID
				,s.Description
				,s.Junk
				,s.AddName
				,s.AddDate
				,s.EditName
				,s.EditDate
	FROM #Trade_DQSReason s WITH (NOLOCK) 
	WHERE NOT EXISTS(SELECT 1 FROM ManufacturingExecution.dbo.DQSReason t WHERE t.ID=s.ID AND t.Type = s.Type)


	UPDATE t
	SET   t.Description = s.Description
		, t.Junk = s.Junk
		, t.AddName = s.AddName
		, t.AddDate = s.AddDate
		, t.EditName = s.EditName
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
				 s.Ukey 
				,s.DescEN
				,s.DescCHS
				,s.DescVN
				,s.DescKH
				,s.AddName
				,s.AddDate
				,s.EditName
				,s.EditDate
	FROM #Trade_MeasurementTranslate s WITH (NOLOCK) 
	WHERE NOT EXISTS(SELECT 1 FROM ManufacturingExecution.dbo.MeasurementTranslate t WHERE t.Ukey=s.Ukey)
	;
	UPDATE t
	SET   t.Ukey = s.Ukey
		, t.DescEN = s.DescEN
		, t.DescCHS = s.DescCHS
		, t.DescVN = s.DescVN
		, t.DescKH = s.DescKH
		, t.AddName = s.AddName
		, t.AddDate = s.AddDate
		, t.EditName = s.EditName
		, t.EditDate = s.EditDate
	FROM ManufacturingExecution.dbo.MeasurementTranslate t
	INNER JOIN #Trade_MeasurementTranslate s ON t.Ukey=s.Ukey
	;

	update t set MeasurementTranslateUkey = MeasurementTranslateUk.Ukey
	from dbo.Measurement t with (nolock)
	outer apply(select top 1 ukey from MeasurementTranslate mt with (nolock) where dbo.MeasurementTrim(mt.DescEN) = dbo.MeasurementTrim(t.Description)) MeasurementTranslateUk
	where t.MeasurementTranslateUkey is null

	DROP TABLE #Trade_MeasurementTranslate
END

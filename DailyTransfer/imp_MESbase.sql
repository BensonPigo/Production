-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/22>
-- Description:	<import part>
-- =============================================
CREATE PROCEDURE [dbo].[imp_MESbase]
AS
BEGIN

	SELECT *
	INTO #Trade_DQSReason
	FROM Trade_To_Pms.dbo.DQSReason
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
END

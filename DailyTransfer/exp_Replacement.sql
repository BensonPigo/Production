
-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_Replacement>
-- =============================================
Create PROCEDURE [dbo].[exp_Replacement]
	
AS

IF OBJECT_ID(N'ReplacementReport') IS NOT NULL
BEGIN
  DROP TABLE ReplacementReport
END

IF OBJECT_ID(N'ReplacementReport_Detail') IS NOT NULL
BEGIN
  DROP TABLE ReplacementReport_Detail
END



SELECT * 
INTO ReplacementReport
FROM Production.dbo.ReplacementReport
WHERE ExportToTPE >=CONVERT(DATE,DATEADD(day,-30,GETDATE()))
or ExportToTPE is null
OR EditDate >=  CONVERT(DATE,DATEADD(day,-30,GETDATE()))
ORDER BY Id 


SELECT b.* 
INTO ReplacementReport_Detail
FROM Production.dbo.ReplacementReport a, Production.dbo.ReplacementReport_Detail b
WHERE a.Id = b.Id 
ORDER BY b.Id 


UPDATE ReplacementReport
SET ExportToTPE =CONVERT(date, GetDate())
Where ExportToTPE is null







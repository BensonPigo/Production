
Create PROCEDURE [dbo].[exp_GASA]
AS

IF OBJECT_ID(N'dbo.SentReport') IS NOT NULL
BEGIN
  DROP TABLE SentReport
END


IF OBJECT_ID(N'dbo.FirstDyelot') IS NOT NULL
BEGIN
  DROP TABLE FirstDyelot
END

SELECT [Export_DetailUkey]
      ,[InspectionReport]
      ,[TestReport]
      ,[ContinuityCard]
      ,[T2InspYds]
      ,[T2DefectPoint]
      ,[T2Grade]
      ,[EditName]
      ,[EditDate]
      ,[TestReportCheckClima]
INTO SentReport
FROM Production.dbo.SentReport
WHERE EditDate >= Convert(DATE,DATEADD(day,-30,GETDATE()))
;

SELECT [TestDocFactoryGroup]
      ,[Refno]
      ,[SuppID]
      ,[ColorID]
      ,[SeasonSCIID]
      ,[Period]
      ,[FirstDyelot]
      ,[EditName]
      ,[EditDate]
INTO FirstDyelot
FROM Production.dbo.FirstDyelot
WHERE EditDate >= Convert(DATE,DATEADD(day,-30,GETDATE()))
;
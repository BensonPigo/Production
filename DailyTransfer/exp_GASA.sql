
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

SELECT * 
INTO SentReport
FROM Production.dbo.SentReport
WHERE EditDate < Convert(DATE,DATEADD(day,-30,GETDATE()))
;

SELECT * 
INTO FirstDyelot
FROM Production.dbo.FirstDyelot
WHERE EditDate < Convert(DATE,DATEADD(day,-30,GETDATE()))
;
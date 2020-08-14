
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

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='GASA';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))

--3.更新Pms_To_Trade.dbo.dateInfo	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateStart);
------------------------------------------------------------------------------------------------------

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
WHERE EditDate >= @DateStart
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
WHERE EditDate >= @DateStart
;
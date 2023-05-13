
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
--***��ƥ洫�����󭭨�***
--1. �u�����oProduction.dbo.DateInfo
declare @DateInfoName varchar(30) ='GASA';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.���o�w�]��
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))

--3.��sPms_To_Trade.dbo.dateInfo	
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateStart, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateStart,@Remark);
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

SELECT [SuppID]
	  ,[TestDocFactoryGroup]
      ,[BrandRefno]      
      ,[ColorID]
      ,[SeasonID]
      ,[Period]
      ,[FirstDyelot]
      ,[EditName]
      ,[EditDate]
	  ,[FTYReceivedReport]
INTO FirstDyelot
FROM Production.dbo.FirstDyelot
WHERE EditDate >= @DateStart
;
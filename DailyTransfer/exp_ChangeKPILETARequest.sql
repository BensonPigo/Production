CREATE PROCEDURE [dbo].[exp_ChangeKPILETARequest]
AS
IF OBJECT_ID(N'ChangeKPILETARequest') IS NOT NULL
BEGIN
  DROP TABLE ChangeKPILETARequest
END

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='ChangeKPILETARequest';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE, GETDATE())	

--3.更新Pms_To_Trade.dbo.dateInfo
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateEnd);
------------------------------------------------------------------------------------------------------

SELECT *
INTO ChangeKPILETARequest
FROM Production.dbo.ChangeKPILETARequest
WHERE Status = 'Sent' 
and ((AddDate between @DateStart and @DateEnd)
	or
	(EditDate between @DateStart and @DateEnd and EditDate is not null))
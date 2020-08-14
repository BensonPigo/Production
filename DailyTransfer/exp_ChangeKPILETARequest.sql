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
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE, GETDATE())	

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

SELECT *
INTO ChangeKPILETARequest
FROM Production.dbo.ChangeKPILETARequest
WHERE Status = 'Sent' 
and ((AddDate between @DateStart and @DateEnd)
	or
	(EditDate between @DateStart and @DateEnd and EditDate is not null))
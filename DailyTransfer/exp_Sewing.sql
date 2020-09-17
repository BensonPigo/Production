

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_sewing>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Sewing]

AS
BEGIN

IF OBJECT_ID(N'SewingOutput') IS NOT NULL
BEGIN
  DROP TABLE SewingOutput
END

IF OBJECT_ID(N'SewingOutput_Detail') IS NOT NULL
BEGIN
  DROP TABLE SewingOutput_Detail
END

IF OBJECT_ID(N'SewingOutput_Detail_Detail') IS NOT NULL
BEGIN
  DROP TABLE SewingOutput_Detail_Detail
END

IF OBJECT_ID(N'Order_Location') IS NOT NULL
BEGIN
  DROP TABLE Order_Location
END

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='SewingOutput';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= (SELECT DATEADD(DAY,1,SewLock) FROM Production.dbo.System)
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE, GETDATE())	

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName2 varchar(30) ='SewingOutput2';
declare @DateStart2 date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName2);
declare @DateEnd2 date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName2);
set @Remark = (select Remark from Production.dbo.DateInfo where name = @DateInfoName2);

--2.取得預設值
if @DateStart2 is Null
	set @DateStart2= CONVERT(DATE,DATEADD(day,-7,GETDATE()))
if @DateEnd2 is Null
	set @DateEnd2 = CONVERT(DATE, GETDATE())	

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName2 )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart2,DateEnd = @DateEnd2, Remark=@Remark where Name = @DateInfoName2 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName2,@DateStart2,@DateEnd2,@Remark);
------------------------------------------------------------------------------------------------------

--SewingOutput
SELECT *
INTO SewingOutput
FROM Production.dbo.SewingOutput a
WHERE a.LockDate  BETWEEN @DateStart AND @DateEnd
OR a.LockDate is null
or a.ReDailyTransferDate between @DateStart2 and @DateEnd2

--SewingOutput_detail
SELECT b.*
INTO SewingOutput_Detail
from Pms_To_Trade.dbo.SewingOutput a, Production.dbo.SewingOutput_Detail b
where a.Id = b.Id 

----SewingOutput_detail_detail
SELECT b.*
INTO SewingOutput_Detail_Detail
FROM Pms_To_Trade.dbo.SewingOutput a ,Production.dbo.SewingOutput_Detail_Detail b
where a.id=b.ID

select ol.*
into Order_Location
from Production.dbo.Order_Location ol
where exists(select 1 from Pms_To_Trade.dbo.SewingOutput_Detail where OrderId = ol.OrderId)

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName3 varchar(30) ='SewingOutputTransfer';
declare @DateStart3 date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName3);
declare @DateEnd3 date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName3);
set @Remark = (select Remark from Production.dbo.DateInfo where name = @DateInfoName3);

--2.取得預設值
if @DateStart3 is Null
	set @DateStart3= CONVERT(DATE,DATEADD(day,-7,GETDATE()))
if @DateEnd3 is Null
	set @DateEnd3 = CONVERT(DATE, GETDATE())
	
--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName3 )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart3,DateEnd = @DateEnd3, Remark=@Remark where Name = @DateInfoName3 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName3,@DateStart3,@DateEnd3,@Remark);

--SewingOutputTransfer
select *
into SewingOutputTransfer
from Production.dbo.SewingOutputTransfer sot
where sot.Status = 'Confirmed'
and sot.EditDate between @DateStart3 and @DateEnd3

--SewingOutputTransfer_Detail
select sotd.*
into SewingOutputTransfer_Detail
from Production.dbo.SewingOutputTransfer_Detail sotd
inner join Pms_To_Trade.dbo.SewingOutputTransfer sot on sot.ID = sotd.ID
END
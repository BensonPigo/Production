

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
declare @DateInfoName varchar(30) ='SewingOutput';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= (SELECT DATEADD(DAY,1,SewLock) FROM Production.dbo.System)
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE, GETDATE())	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateEnd);
------------------------------------------------------------------------------------------------------
declare @DateInfoName2 varchar(30) ='SewingOutput2';
declare @DateStart2 date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName2);
declare @DateEnd2 date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName2);
if @DateStart2 is Null
	set @DateStart2= CONVERT(DATE,DATEADD(day,-7,GETDATE()))
if @DateEnd2 is Null
	set @DateEnd2 = CONVERT(DATE, GETDATE())	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName2 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName2,@DateStart2,@DateEnd2);
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
END
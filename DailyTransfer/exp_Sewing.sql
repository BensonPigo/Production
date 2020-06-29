

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

declare @DateStart date= (SELECT DATEADD(DAY,1,SewLock) FROM Production.dbo.System);
declare @DateEnd date=CONVERT(date, GETDATE());
declare @DateInfoName varchar(30) ='SewingOutput';

If Exists (Select 1 From Pms_To_Trade.dbo.DateInfo Where Name = @DateInfoName )
Begin
	update Pms_To_Trade.dbo.dateInfo
	set DateStart=@DateStart,
	DateEnd=@DateEnd
	Where Name = @DateInfoName 
end;
else
Begin 
	insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
	values (@DateInfoName,@DateStart,@DateEnd);
end;


--SewingOutput
SELECT *
INTO SewingOutput
FROM Production.dbo.SewingOutput a
WHERE a.LockDate  BETWEEN (select DATEADD(DAY,1,SewLock) from Production.dbo.System)  AND CONVERT(date, GETDATE())
OR a.LockDate is null
or a.ReDailyTransferDate between cast(DATEADD(DAY,-7,GETDATE()) as date) and cast(GETDATE() as date)

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





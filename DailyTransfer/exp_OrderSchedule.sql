
Create PROCEDURE [dbo].[exp_OrderSchedule]
	
AS
BEGIN
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.OrderSchedule') IS NOT NULL
BEGIN
DROP TABLE OrderSchedule
END

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='OrderSchedule';
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateEnd is Null
	set @DateEnd = EOMONTH(GETDATE(),3)	

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateEnd,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateEnd,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

Select	o.ID
		, o.SewInLine
		, o.SewOffLine
		, o.PulloutDate
		, o.InspDate
		, CutInLine = (select CutInLine from [Production].dbo.Cutting where ID = o.CuttingSP)
		, CutOffLine = (select CutOffLine from [Production].dbo.Cutting where ID = o.CuttingSP)
		, o.SewLine
		, FirstCutDate = (select FirstCutDate from [Production].dbo.Cutting where ID = o.CuttingSP)
INTO OrderSchedule
from [Production].dbo.Orders o
where	o.SCIDelivery <= @DateEnd
		and o.Finished = 0
		and (o.Junk=0 or (o.Junk=1 and o.NeedProduction=1))
		and o.IsForecast = 0
		and o.LocalOrder = 0
		and o.SewInLine is not null
		and o.SewOffLine is not null

END

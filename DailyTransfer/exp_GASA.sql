
Create PROCEDURE [dbo].[exp_GASA]
AS

IF OBJECT_ID(N'dbo.FirstDyelot') IS NOT NULL
BEGIN
  DROP TABLE FirstDyelot
END

IF OBJECT_ID(N'dbo.GASAClip') IS NOT NULL
BEGIN
  DROP TABLE GASAClip
END

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='GASA';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))

--3.更新Pms_To_Trade.dbo.dateInfo	
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateStart, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateStart,@Remark);
------------------------------------------------------------------------------------------------------

SELECT[SuppID]
      ,[TestDocFactoryGroup]
      ,[BrandRefno]
      ,[ColorID]
      ,[SeasonID]
      ,[Period]
      ,[FirstDyelot]
      ,[AWBno]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[FTYReceivedReport]
      ,[ReceivedDate]
      ,[ReceivedRemark]
      ,[DocumentName]
      ,[BrandID]
      ,[LOT]
INTO FirstDyelot
FROM Production.dbo.FirstDyelot
WHERE EditDate >= @DateStart
;

-- [GASAClip] only export 7 days Factory create GasaClip data
SELECT [PKey]
      ,[TableName]
      ,[UniqueKey]
      ,[SourceFile]
      ,[Description]
      ,g.[AddName]
      ,g.[AddDate]
INTO [GASAClip]
FROM [Production].[dbo].[GASAClip] g
inner join [Production].[dbo].[Pass1] p on g.AddName = p.ID
WHERE g.AddDate >= CONVERT(DATE,DATEADD(day,-7,GETDATE()))



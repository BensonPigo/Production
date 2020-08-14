
CREATE PROCEDURE  [dbo].[exp_Style_ProductionKits]	
AS
BEGIN
	SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.Style_ProductionKits') IS NOT NULL
BEGIN
DROP TABLE Style_ProductionKits
END

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='Style_ProductionKits';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(month, -2, GetDate()))

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateStart, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateStart,@Remark);
------------------------------------------------------------------------------------------------------

SELECT Ukey, ReceiveDate, FtyHandle,
(SELECT Email FROM [Production].dbo.Pass1 where Pass1.id= [Production].dbo.Style_ProductionKits.FtyHandle) as MCEMAIL,
(SELECT ExtNo FROM [Production].dbo.Pass1 where Pass1.id= [Production].dbo.Style_ProductionKits.FtyHandle) as MCEXT,
FtyLastDate, FtyRemark 
INTO  Style_ProductionKits
FROM [Production].dbo.Style_ProductionKits
WHERE CONVERT(DATE,FtyLastDate) >= @DateStart

END
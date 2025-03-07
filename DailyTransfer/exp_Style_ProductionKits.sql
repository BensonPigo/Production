
CREATE PROCEDURE  [dbo].[exp_Style_ProductionKits]	
AS
BEGIN
	SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.Style_ProductionKits') IS NOT NULL
BEGIN
DROP TABLE Style_ProductionKits
END

------------------------------------------------------------------------------------------------------
--***��ƥ洫�����󭭨�***
--1. �u�����oProduction.dbo.DateInfo
declare @DateInfoName varchar(30) ='Style_ProductionKits';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.���o�w�]��
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(month, -2, GetDate()))

--3.��sPms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateStart, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateStart,@Remark);
------------------------------------------------------------------------------------------------------

SELECT Ukey, ReceiveDate, FtyHandle,
(SELECT ISNULL(Email,'') FROM [Production].dbo.Pass1 where Pass1.id= [Production].dbo.Style_ProductionKits.FtyHandle) as MCEMAIL,
(SELECT ISNULL(ExtNo,'') FROM [Production].dbo.Pass1 where Pass1.id= [Production].dbo.Style_ProductionKits.FtyHandle) as MCEXT,
FtyLastDate, FtyRemark 
,Reject
INTO  Style_ProductionKits
FROM [Production].dbo.Style_ProductionKits
WHERE CONVERT(DATE,FtyLastDate) >= @DateStart

END
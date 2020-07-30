
CREATE PROCEDURE  [dbo].[exp_Style_ProductionKits]	
AS
BEGIN
	SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.Style_ProductionKits') IS NOT NULL
BEGIN
DROP TABLE Style_ProductionKits
END

declare @DateInfoName varchar(30) ='Style_ProductionKits';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = NULL--(select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(month, -2, GetDate()))
if @DateEnd is Null
	set @DateEnd = NULL-- CONVERT(DATE, GETDATE())
	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateEnd);

SELECT Ukey, ReceiveDate, FtyHandle,
(SELECT Email FROM [Production].dbo.Pass1 where Pass1.id= [Production].dbo.Style_ProductionKits.FtyHandle) as MCEMAIL,
(SELECT ExtNo FROM [Production].dbo.Pass1 where Pass1.id= [Production].dbo.Style_ProductionKits.FtyHandle) as MCEXT,
FtyLastDate, FtyRemark 
INTO  Style_ProductionKits
FROM [Production].dbo.Style_ProductionKits
WHERE CONVERT(DATE,FtyLastDate) >= @DateStart

END






-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_Replacement>
-- =============================================
Create PROCEDURE [dbo].[exp_Replacement]
	
AS

IF OBJECT_ID(N'ReplacementReport') IS NOT NULL
BEGIN
  DROP TABLE ReplacementReport
END

IF OBJECT_ID(N'ReplacementReport_Detail') IS NOT NULL
BEGIN
  DROP TABLE ReplacementReport_Detail
END


declare @DateInfoName varchar(30) ='ReplacementReport';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = NULL--(select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))
if @DateEnd is Null
	set @DateEnd = NULL-- CONVERT(DATE, GETDATE())
	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateEnd);


SELECT * 
INTO ReplacementReport
FROM Production.dbo.ReplacementReport
WHERE ExportToTPE >= @DateStart
or ExportToTPE is null
OR EditDate >= @DateStart
ORDER BY Id 


SELECT b.* 
INTO ReplacementReport_Detail
FROM Pms_To_Trade.dbo.ReplacementReport a, Production.dbo.ReplacementReport_Detail b
WHERE a.Id = b.Id 
ORDER BY b.Id 


UPDATE ReplacementReport
SET ExportToTPE =CONVERT(date, GetDate())
Where ExportToTPE is null







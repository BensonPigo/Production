
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

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='ReplacementReport';
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







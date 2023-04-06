CREATE PROCEDURE [dbo].[exp_MaterialDoc] 
AS
BEGIN
SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.UASentReport') IS NOT NULL
BEGIN
  DROP TABLE UASentReport
END

IF OBJECT_ID(N'dbo.NewSentReport') IS NOT NULL
BEGIN
  DROP TABLE NewSentReport
END

IF OBJECT_ID(N'dbo.ExportRefnoSentReport') IS NOT NULL
BEGIN
  DROP TABLE ExportRefnoSentReport
END

------------------------------------------------------------------------------------------------------
declare @DateStart date;
--***資料交換的條件限制***
--1.取得預設值
if @DateStart is Null
begin
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))
end
------------------------------------------------------------------------------------------------------

SELECT
	BrandRefno
	,ColorID
	,SuppID
	,DocumentName
	,BrandID
	,TestSeasonID
	,DueSeason
	,DueDate
	,Ukey
	,TestReport
	,FTYReceivedReport
	,TestReportTestDate
	,AddDate
	,AddName
	,EditDate
	,Editname
into UASentReport
FROM Production.dbo.UASentReport with (nolock)
where isnull(EditDate,AddDate) BETWEEN @DateStart and GETDATE()

--	NewSentReport
select 
	  [ExportID]
      ,[PoID]
      ,[Seq1]
      ,[Seq2]
      ,[DocumentName]
      ,[BrandID]
      ,[ReportDate]
      ,[FTYReceivedReport]
      ,[AWBno]
      ,[T2InspYds]
      ,[T2DefectPoint]
      ,[T2Grade]
      ,[TestReportTestDate]
      ,[Ukey]
      ,[AddDate]
      ,[AddName]
      ,[EditDate]
      ,[Editname]
into [NewSentReport]
FROM [Production].[dbo].[NewSentReport]
where isnull(EditDate,AddDate) BETWEEN  @DateStart and GETDATE()


--	ExportRefnoSentReport
SELECT [ExportID]
      ,[BrandRefno]
      ,[ColorID]
      ,[DocumentName]
      ,[BrandID]
      ,[ReportDate]
      ,[FTYReceivedReport]
      ,[AWBno]
      ,[UKEY]
      ,[AddDate]
      ,[AddName]
      ,[EditDate]
      ,[Editname]
into [ExportRefnoSentReport]
FROM [Production].[dbo].[ExportRefnoSentReport]
where isnull(EditDate,AddDate) BETWEEN  @DateStart  and GETDATE()


END
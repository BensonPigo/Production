CREATE PROCEDURE [dbo].[exp_MaterialDoc] 
AS
BEGIN
SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.UASentReport') IS NOT NULL
BEGIN
  DROP TABLE UASentReport
END

------------------------------------------------------------------------------------------------------
declare @DateStart date;
--***資料交換的條件限制***
--1.取得預設值
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))
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
where isnull(EditDate,AddDate) BETWEEN  @DateStart  and GETDATE()
END





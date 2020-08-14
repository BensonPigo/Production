

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_wkhours>
-- =============================================
CREATE PROCEDURE [dbo].[exp_WkHours]
AS

IF OBJECT_ID('dbo.Workhours') IS NOT NULL
BEGIN
  DROP TABLE Workhours
END

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='WorkHour';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= CONVERT(DATE, DATEADD(mm,  DATEDIFF(mm,0,dateadd(month,-1,getdate())),  0))

--3.更新Pms_To_Trade.dbo.dateInfo
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateStart);
------------------------------------------------------------------------------------------------------

SELECT  FactoryID, Date,AVG(Hours) as WHours 
INTO Workhours
FROM Production.dbo.WorkHour 
WHERE Date>= @DateStart
GROUP BY FactoryID, Date


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


declare @DateInfoName varchar(30) ='WorkHour';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = NULL--(select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= CONVERT(DATE, DATEADD(mm,  DATEDIFF(mm,0,dateadd(month,-1,getdate())),  0))
if @DateEnd is Null
	set @DateEnd = NULL-- CONVERT(DATE, GETDATE())
	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateEnd);

SELECT  FactoryID, Date,AVG(Hours) as WHours 
INTO Workhours
FROM Production.dbo.WorkHour 
WHERE Date>= @DateStart
GROUP BY FactoryID, Date
	






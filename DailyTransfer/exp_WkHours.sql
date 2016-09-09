

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


SELECT  FactoryID, Date,AVG(Hours) as WHours 
INTO Workhours
FROM Production.dbo.WorkHour 
WHERE Date>= (SELECT CONVERT(DATE, DATEADD(mm,  DATEDIFF(mm,0,dateadd(month,-1,getdate())),  0))) 
GROUP BY FactoryID, Date
	






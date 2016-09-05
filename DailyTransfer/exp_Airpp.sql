USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_Airpp]    Script Date: 2016/9/2 ¤W¤È 10:37:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Leo 01921>
-- Create date: <2016/08/17>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Airpp] 
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.AirPP') IS NOT NULL
BEGIN
  DROP TABLE Express
END
SELECT *
,LEFT((SELECT  S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder),12) AS ForWardN
,LEFT((SELECT  S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder1),12) AS ForWard1N
,LEFT((SELECT  S.Abb FROM Production.dbo.LocalSupp S  WHERE S.ID = A.ForWarder2),12) AS ForWard2N
INTO AirPP
FROM Production.dbo.AirPP AS A
WHERE 
((EditDate >= DATEADD(DAY, -30, GETDATE()) AND CONVERT(DATETIME, TPEEditDate) <= CONVERT(DATETIME, EditDate))
OR 
(CONVERT(DATETIME, EditDate) >= DATEADD(DAY, -30, GETDATE()) AND CONVERT(DATETIME,AddDate) <= CONVERT(DATETIME, EditDate)))
AND Status IN ('New','Checked','Approved','Junked')
ORDER BY Id 


UPDATE Production.dbo.AirPP
SET FtySendDate = GETDATE()
WHERE 
((EditDate >= DATEADD(DAY, -30, GETDATE()) AND CONVERT(DATETIME, TPEEditDate) <= CONVERT(DATETIME, EditDate))
OR
(CONVERT(DATETIME, EditDate) >= DATEADD(DAY, -30, GETDATE()) 
AND CONVERT(DATETIME, AddDate) <= CONVERT(DATETIME, EditDate)))
AND Status IN ('New','Checked','Approved','Junked') AND FtyMgrApvDate is not null AND FtySendDate is null
END

GO



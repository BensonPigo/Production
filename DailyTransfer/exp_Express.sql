USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_Express]    Script Date: 2016/9/2 ¤W¤È 10:39:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Leo 01921>
-- Create date: <2016/8/17>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Express]
	---- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.Express') IS NOT NULL
BEGIN
  DROP TABLE Express
END
IF OBJECT_ID(N'dbo.Express_Detail') IS NOT NULL
BEGIN
  DROP TABLE Express_Detail
END
IF OBJECT_ID(N'dbo.Express_CTNData') IS NOT NULL
BEGIN
  DROP TABLE Express_CTNData
END

SELECT * INTO Express
FROM [Production].dbo.Express  WHERE (AddDate >= DATEADD(DAY, -30, GETDATE()) or EditDate >= DATEADD(DAY, -30, GETDATE())) ORDER BY Id

SELECT B.* INTO Express_Detail
FROM  Production.dbo.Express  A, [Production].dbo.Express_Detail  B WHERE A.ID = B.ID ORDER BY B.ID 

SELECT B.* INTO  Express_CTNData
FROM Production.dbo.Express A, [Production].dbo.Express_CTNData B WHERE A.ID = B.ID ORDER BY B.ID 

UPDATE [Production].dbo.Express 
SET  SendDate =GETDATE()
FROM [Production].dbo.Express A  INNER JOIN Production.dbo.Express ON A.Id = Express.Id
WHERE A.SendDate IS NULL
AND A.Status in ('Approved','Junked')




END

GO



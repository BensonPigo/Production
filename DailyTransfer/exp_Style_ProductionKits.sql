USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_Style_ProductionKits]    Script Date: 2016/9/2 ¤W¤È 10:48:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[exp_Style_ProductionKits]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.Style_ProductionKits') IS NOT NULL
BEGIN
DROP TABLE Style_ProductionKits
END

SELECT Ukey, ReceiveDate, FtyHandle,
(SELECT Email FROM [Production].dbo.Pass1 where Pass1.id= Style_ProductionKits.FtyHandle) as MCEMAIL,
(SELECT ExtNo FROM [Production].dbo.Pass1 where Pass1.id= Style_ProductionKits.FtyHandle) as MCEXT,
FtyLastDate, FtyRemark 
INTO  Style_ProductionKits
FROM [Production].dbo.Style_ProductionKits
WHERE CONVERT(DATE,FtyLastDate) >= CONVERT(DATE,DATEADD(month, -2, GetDate()))

END

GO



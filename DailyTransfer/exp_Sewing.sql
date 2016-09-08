USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_Sewing]    Script Date: 2016/9/2 ¤W¤È 10:47:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_sewing>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Sewing]

AS
BEGIN

IF OBJECT_ID(N'SewingOutput') IS NOT NULL
BEGIN
  DROP TABLE SewingOutput
END

IF OBJECT_ID(N'SewingOutput_Detail') IS NOT NULL
BEGIN
  DROP TABLE SewingOutput_Detail
END

IF OBJECT_ID(N'SewingOutput_Detail_Detail') IS NOT NULL
BEGIN
  DROP TABLE SewingOutput_Detail_Detail
END


--SewingOutput
SELECT *
INTO SewingOutput
FROM Production.dbo.SewingOutput a
WHERE a.LockDate  BETWEEN (select DATEADD(DAY,1,SewLock) from Production.dbo.System)  AND CONVERT(date, GETDATE())
OR a.LockDate is null

--SewingOutput_detail
SELECT b.*
INTO SewingOutput_Detail
from Pms_To_Trade.dbo.SewingOutput a, Production.dbo.SewingOutput_Detail b
where a.Id = b.Id 


----SewingOutput_detail_detail
SELECT b.*
INTO SewingOutput_Detail_Detail
FROM Pms_To_Trade.dbo.SewingOutput a ,Production.dbo.SewingOutput_Detail_Detail b
where a.id=b.ID


END

GO



USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_Machine]    Script Date: 2016/9/2 ¤W¤È 10:39:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_machine>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Machine]

AS
IF OBJECT_ID(N'Machine') IS NOT NULL
BEGIN
  DROP TABLE Machine
END


SELECT OwnedFactory , MachineGroupID , COUNT(*) AS Qty 
INTO Machine
FROM Machine.dbo.Machine
WHERE Junk = 0 AND Status <> 'Disposed' 
GROUP BY OwnedFactory, MachineGroupID



GO



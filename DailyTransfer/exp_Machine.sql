

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







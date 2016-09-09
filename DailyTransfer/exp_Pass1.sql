

-- =============================================
-- Author:		<Leo 01921>
-- Create date: <2016/08/17>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Pass1] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF OBJECT_ID(N'dbo.Pass1') IS NOT NULL
BEGIN
  DROP TABLE Pass1
END

Select *  into Pass1 
from(
     Select * from [Production].dbo.Pass1
     Union all 
     Select * from [Machine].dbo.Pass1 as m
     where not exists (Select ID from [Production].dbo.Pass1 a where a.ID = m.ID)
         
) as s


Update Pass1 
set Factory = (select RgCode from [Production].dbo.System)
END





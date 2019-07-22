
-- =============================================
-- Author:		<Leo 01921>
-- Create date: <2016/08/17>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[exp_adicomp] 
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.ADIDASComplain') IS NOT NULL
BEGIN
  DROP TABLE ADIDASComplain
END

IF OBJECT_ID(N'dbo.ADIDASComplain_Detail') IS NOT NULL
BEGIN
  DROP TABLE ADIDASComplain_Detail
END

declare @FtyApvDate  date = getdate() - 7

SELECT
	ID
	,FtyApvName
	,FtyApvDate
	into ADIDASComplain
FROM Production.dbo.ADIDASComplain with (nolock)
where FtyApvDate >= @FtyApvDate

SELECT
	b.UKey,
	b.SuppID,
	b.Refno,
	b.IsEM
	into ADIDASComplain_Detail
FROM Production.dbo.ADIDASComplain a with (nolock)
inner join Production.dbo.ADIDASComplain_Detail b with (nolock) on a.ID = b.ID
where a.FtyApvDate >= @FtyApvDate

END





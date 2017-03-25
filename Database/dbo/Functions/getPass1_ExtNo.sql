
-- =============================================
-- Author:		<JEFF.YEH>
-- Create date: <2017/03/25>
-- Description:	<Pass1的id+Name+ExtNo>
-- =============================================
CREATE FUNCTION [dbo].[getPass1_ExtNo]
(
	-- Add the parameters for the function here
	@id as varchar(10)
)
RETURNS varchar(45)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @rtn as varchar(45);

	-- Add the T-SQL statements to compute the return value here
	SELECT @rtn = CONCAT(a.id, ':', a.Name, iif(a.ExtNo is null or  a.ExtNo = '','', CONCAT(' #', a.ExtNo))) 
	from dbo.Pass1 a WITH (NOLOCK) where a.ID = @id;

	-- Return the result of the function
	RETURN @rtn;
END
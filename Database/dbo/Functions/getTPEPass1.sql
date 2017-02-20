-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION dbo.getTPEPass1
(
	-- Add the parameters for the function here
	@id as varchar(10)
)
RETURNS varchar(45)
AS
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	-- Declare the return variable here
	DECLARE @rtn as varchar(45);

	-- Add the T-SQL statements to compute the return value here
	SELECT @rtn = a.id+':'+a.Name from dbo.TPEPASS1 a where a.ID=@id;

	-- Return the result of the function
	RETURN @rtn;
END
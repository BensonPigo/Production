CREATE FUNCTION [dbo].[getPassEmail]
(
	@id as varchar(20)
)
RETURNS varchar(80)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @rtn as varchar(80);

	set @rtn = ''
	-- Add the T-SQL statements to compute the return value here
	select @rtn = isnull(EMail,'') from dbo.Pass1 a WITH (NOLOCK) where a.ID = @id;

	if @rtn = ''
	begin 
		select @rtn = isnull(EMail,'') from dbo.TPEPass1 a WITH (NOLOCK) where a.ID = @id;
	end

	-- Return the result of the function
	RETURN @rtn;
END
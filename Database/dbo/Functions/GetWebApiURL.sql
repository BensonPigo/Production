CREATE FUNCTION [dbo].[GetWebApiURL]
(
	@suppID varchar(6),
	@moduleName varchar(20)
)
RETURNS varchar(100)
AS
BEGIN
	Declare @ModuleType varchar(20)
	Declare @Url varchar(100)
	if(	@@servername like '%TESTING%' or
		@@servername like '%MIS%' or
		@@servername like '%Training%' )
	begin
		set @ModuleType = 'Dummy'
	end
	else
	begin
		set @ModuleType = 'Formal'
	end

	select @Url = URL from WebApiURL with (nolock) where SuppID = @suppID and ModuleName = @moduleName and ModuleType = @ModuleType and Junk = 0

	RETURN isnull(@Url,'')
END

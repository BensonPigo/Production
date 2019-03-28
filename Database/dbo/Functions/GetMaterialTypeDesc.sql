CREATE FUNCTION [dbo].[GetMaterialTypeDesc]
(
	@type varchar(20)
)
RETURNS varchar(20)
AS
BEGIN
	if(@type = 'A')
	begin
		return 'Accessory'
	end

	if(@type = 'F')
	begin
		return 'Fabric'
	end

	RETURN isnull(@type,'')
END

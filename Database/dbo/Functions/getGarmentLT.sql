
CREATE FUNCTION [dbo].[getGarmentLT](@styleukey bigint, @factoryid varchar(8))
RETURNS smallint
BEGIN
	DECLARE @smallint smallint --要回傳的數值
	IF @factoryid <> ''
		BEGIN
			select @smallint = GMTLT from Style_GMTLTFty where StyleUkey = @styleukey and FactoryID = @factoryid
		END
	IF @smallint is null
		BEGIN
			Select @smallint = GMTLT from Style where Ukey = @styleukey
		END
	RETURN @smallint
END
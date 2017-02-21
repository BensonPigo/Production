
CREATE FUNCTION [dbo].[getUnitRate](@fromUnit varchar(8), @toUnit varchar(8))
RETURNS decimal
BEGIN
	DECLARE @rate decimal
	SET @rate = 1
	select @rate = isnull(RateValue,'1') from View_Unitrate where FROM_U = @fromUnit and TO_U = @toUnit
	
	RETURN @rate
END
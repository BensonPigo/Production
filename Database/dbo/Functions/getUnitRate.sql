
CREATE FUNCTION [dbo].[getUnitRate](@fromUnit varchar(8), @toUnit varchar(8))
RETURNS float
BEGIN
	DECLARE @rate float
	SET @rate = 1
	select @rate = isnull(RateValue,'1') from Unit_Rate WITH (NOLOCK) where UnitFrom = @fromUnit and UnitTo = @toUnit
	
	RETURN @rate
END
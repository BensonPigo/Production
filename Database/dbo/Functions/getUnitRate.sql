
CREATE FUNCTION [dbo].[getUnitRate](@fromUnit varchar(8), @toUnit varchar(8))
RETURNS float
BEGIN
	DECLARE @rate float
	SET @rate = 1
	select @rate = isnull(IIF(Denominator = 0,0, Numerator / Denominator),'1') from Unit_Rate WITH (NOLOCK) where UnitFrom = @fromUnit and UnitTo = @toUnit
	
	RETURN @rate
END
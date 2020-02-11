
Create FUNCTION [dbo].[GetAmount]
(
	-- Add the parameters for the function here
	@Price numeric(16,4),
	@Qty numeric(12,4),
	@Unit_PriceRate numeric(5,0) = 1,
	@Round int 
)
RETURNS numeric(24,5)
AS
BEGIN
	
	RETURN round(@Price * @Qty / COALESCE(NULLIF(@Unit_PriceRate, 0), 1), @Round)

END
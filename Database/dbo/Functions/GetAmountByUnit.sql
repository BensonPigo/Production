
Create FUNCTION [dbo].[GetAmountByUnit]
(	
	-- Add the parameters for the function here
	@Price numeric(16,4),
	@Qty numeric(12,4),
	@UnitID char(8),
	@Round int 	
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	SELECT dbo.GetAmount(@Price,@Qty,PriceRate,@Round) as Amount
	from dbo.Unit where ID = @UnitID
)
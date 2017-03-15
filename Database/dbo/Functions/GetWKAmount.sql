CREATE FUNCTION [dbo].[GetWKAmount]
(
	@ExportID VARCHAR(13),
	@APDate date,
	@RateType varchar(2)
)
RETURNS DECIMAL(18, 2) 
BEGIN
	DECLARE @return DECIMAL(18, 2), @immediateRate DECIMAL(18, 8)
	
	select @return =  round(sum(Amount),2) 
	from (
		SELECT Price * Qty * dbo.GetFinanceRate(@RateType,@APDate,(Select CurrencyID from Supp where ID = Export_Detail.SuppID),'USD') / iif(Export_Detail.UnitID = 'P',100,iif(Export_Detail.UnitID = 'PX',1000,1)) as Amount 
		FROM EXPORT_dETAIL 
		Where id = @ExportID) as A

	RETURN @return
END









GO


USE [Production]
GO

/****** Object:  UserDefinedFunction [dbo].[GetExportOrderAmount]    Script Date: 2017/3/15 下午 12:01:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[GetExportOrderAmount]
(
	@ExportID VARCHAR(13),
	@OrderID VARCHAR(13),
	@APDate date,
	@RateType varchar(2)
)
RETURNS DECIMAL(18, 2) 
BEGIN
	DECLARE @return DECIMAL(18, 2), @immediateRate DECIMAL(18, 8)
	
	select @return = round(sum(Amount),2) 
	from (
		SELECT Price * Qty * dbo.GetFinanceRate(@RateType,@APDate,(Select CurrencyID from Supp where ID = Export_Detail.SuppID),'USD') / iif(Export_Detail.UnitID = 'P',100,iif(Export_Detail.UnitID = 'PX',1000,1)) as Amount 
		FROM EXPORT_dETAIL 
		Where id = @ExportID and POID = @OrderID ) as A

	RETURN @return
END









GO


CREATE FUNCTION [dbo].[GetFinanceRate]
(
	@rateTypeID VARCHAR(2), 
	@rateDate DATETIME,
	@originalCurrency VARCHAR(3),
	@exchangeCurrency VARCHAR(3)
)
RETURNS DECIMAL(18, 8) 
BEGIN
	DECLARE @returnRate DECIMAL(18, 8), @immediateRate DECIMAL(18, 8)
	
	IF @originalCurrency = @exchangeCurrency
		RETURN 1

	--正向
	SELECT @returnRate = Rate FROM FinanceRate WHERE RateTypeID = @rateTypeID AND OriginalCurrency = @originalCurrency AND ExchangeCurrency = @exchangeCurrency AND BeginDate <= @rateDate AND EndDate >= @rateDate
	IF @returnRate <> 0
		RETURN @returnRate
	
	--反向
	SELECT @returnRate = Rate FROM FinanceRate WHERE RateTypeID = @rateTypeID AND OriginalCurrency = @exchangeCurrency AND ExchangeCurrency = @originalCurrency AND BeginDate <= @rateDate AND EndDate >= @rateDate
	IF @returnRate <> 0
		RETURN 1 / @returnRate

	--間接正向
	SELECT @immediateRate = Rate FROM FinanceRate WHERE RateTypeID = @rateTypeID AND OriginalCurrency = @originalCurrency AND ExchangeCurrency = 'TWD' AND BeginDate <= @rateDate AND EndDate >= @rateDate
	IF @immediateRate <> 0
	BEGIN
		SELECT @returnRate = Rate FROM FinanceRate WHERE RateTypeID = @rateTypeID AND OriginalCurrency = 'TWD' AND ExchangeCurrency = @exchangeCurrency AND BeginDate <= @rateDate AND EndDate >= @rateDate
		IF @returnRate <> 0
			RETURN ROUND(@immediateRate * @returnRate, 8)

		SELECT @returnRate = Rate FROM FinanceRate WHERE RateTypeID = @rateTypeID AND OriginalCurrency = @exchangeCurrency AND ExchangeCurrency = 'TWD' AND BeginDate <= @rateDate AND EndDate >= @rateDate
		IF @returnRate <> 0
			RETURN ROUND(@immediateRate / @returnRate, 8)
	END

	--間接反向
	SELECT @immediateRate = Rate FROM FinanceRate WHERE RateTypeID = @rateTypeID AND OriginalCurrency = @exchangeCurrency AND ExchangeCurrency = 'TWD' AND BeginDate <= @rateDate AND EndDate >= @rateDate
	IF @immediateRate <> 0
	BEGIN
		SELECT @returnRate = Rate FROM FinanceRate WHERE RateTypeID = @rateTypeID AND OriginalCurrency = 'TWD' AND ExchangeCurrency = @originalCurrency AND BeginDate <= @rateDate AND EndDate >= @rateDate
		IF @returnRate <> 0
			RETURN rOUND(1/ROUND(@immediateRate * @returnRate, 8),8)

		SELECT @returnRate = Rate FROM FinanceRate WHERE RateTypeID = @rateTypeID AND OriginalCurrency = @originalCurrency AND ExchangeCurrency = 'TWD' AND BeginDate <= @rateDate AND EndDate >= @rateDate
		IF @returnRate <> 0
			RETURN rOUND(1/ROUND(@immediateRate / @returnRate, 8),8)
	END

	RETURN 1
END








GO


Create VIEW [dbo].[View_Rate]
AS
SELECT         ExchangeTypeID AS RateTypeID, DateStart AS BeginDate, DateEnd AS EndDate, CurrencyFrom AS OriginalCurrency, CurrencyTo AS ExchangeCurrency, Rate
FROM           dbo.Exchange
WHERE          Junk = 0
UNION
SELECT         RateTypeID, BeginDate, EndDate, OriginalCurrency, ExchangeCurrency, Rate
FROM           SciFMS_Rate

CREATE Function [dbo].[GetCurrencyRate]
(
	  @ExchangeTypeID	VarChar(2)			--
	 ,@FromCurrency		VarChar(3)			--
	 ,@ToCurrency		VarChar(3)			--
	 ,@Date				Date
)
Returns @rtnTable TABLE (
	rate DECIMAL(18, 8)
	, Exact tinyint
)
AS
BEGIN

	-- 取得 Currency To的標準匯率 & 小數進位數
	insert into @rtnTable
	Select CAST(iIf(@FromCurrency != @ToCurrency, CurrencyRate.Rate, 1) AS numeric(13,8)) Rate
		 , CAST(ct.Exact AS numeric(2,0)) Exact--@Exact
	  From (select 1 row1) c
	  outer apply (select top 1 Exact from Currency with (NOLOCK) Where ID = @ToCurrency) ct
	  outer apply (select dbo.GetRate(@ExchangeTypeID, @FromCurrency, @ToCurrency, @Date) Rate) CurrencyRate

	 Return 
END

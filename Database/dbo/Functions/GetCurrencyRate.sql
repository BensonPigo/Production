CREATE Function [dbo].[GetCurrencyRate]
(
	  @ExchangeTypeID	VarChar(2)			--
	 ,@FromCurrency		VarChar(3)			--
	 ,@ToCurrency		VarChar(3)			--
	 ,@Date				Date
)
Returns TABLE
AS
RETURN 
(
    SELECT  rate = iif(ExchangeRate.value != 0, ExchangeRate.value
											  , iif(cto.StdRate = 0, 0
												                   , cfrom.StdRate/cto.StdRate)) 
			, cTo.Exact
    FROM dbo.Currency as cFrom WITH (NOLOCK)
    left join dbo.Currency as cTo WITH (NOLOCK) on cTo.ID = @toCurrency
    left join dbo.exchange as ex WITH (NOLOCK) on	ex.ExchangeTypeID=@ExchangeTypeID 
													and ex.CurrencyFrom= cFrom.ID 
													and ex.CurrencyTo= cTo.ID
													and @Date between ex.DateStart and ex.DateEnd
	outer apply(
	-- 分開寫是因為 換算的結果會因為 inull 的欄位結構而改變
	-- ex: 0.00000444444 會變成 0.0000
		select value = isnull(ex.rate, 0)
	) ExchangeRate
    where cFrom.ID = @fromCurrency
);
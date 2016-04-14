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
    SELECT isnull( ex.rate, iif(cto.StdRate = 0, 0, cfrom.StdRate/cto.StdRate)) as rate,cTo.Exact
    FROM dbo.Currency as cFrom
     left join dbo.Currency as cTo on cTo.ID = @toCurrency
     left join dbo.exchange as ex on ex.ExchangeTypeID=@ExchangeTypeID and ex.CurrencyFrom= cFrom.ID and ex.CurrencyTo= cTo.ID
              and @Date between ex.DateStart and ex.DateEnd
     where cFrom.ID = @fromCurrency
);
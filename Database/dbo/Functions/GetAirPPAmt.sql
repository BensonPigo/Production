
CREATE FUNCTION [dbo].[GetAirPPAmt]
(
	@APID as varchar(13) = '',
	@AirPPID as varchar(13) = ''
)
RETURNS @tbl table(
	AirPPID varchar(13)
	,ActAmt NUMERIC(18, 2)
	,ExchangeRate NUMERIC(11, 6)
)
AS
BEGIN



insert into @tbl	
select	a.AirPPID,
		[ActAmtUSD] = isnull(ActAmtUSD.val, 0),
		[ExchangeRate] = isnull(minExchangeRate.val, 0)
FROM  (select distinct AirPPID from ShareExpense_APP with (nolock) where ShippingAPID = @APID
	   union
	   select [AirPPID] = @AirPPID
	  ) a
OUTER apply (SELECT [val] = sum(cast(iif(sa.APPExchageRate = 0, 0, (sea.AmtFty + sea.AmtOther) / sa.APPExchageRate) AS decimal(18, 2)))
             FROM ShareExpense_APP sea 
			 LEFT JOIN ShippingAP sa ON sea.ShippingAPID = sa.ID
             WHERE sea.AirPPID = a.AirPPID AND sea.Junk = 0) ActAmtUSD 
OUTER apply (SELECT [val] = min(sa.APPExchageRate)
             FROM  ShareExpense_APP sea 
			 LEFT JOIN ShippingAP sa ON sea.ShippingAPID = sa.ID 
			 OUTER apply (SELECT [val] = min(AddDate) FROM ShippingAP WHERE ID = sa.ID) minAddDate
			 WHERE sa.AddDate = minAddDate.val AND sea.AirPPID = a.AirPPID AND sea.Junk = 0) minExchangeRate
where a.AirPPID <> ''

return

END
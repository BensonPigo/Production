CREATE view  [dbo].[View_AirPP]
as

SELECT [VoucherID] = VoucherID.val
	,[VoucherDate] = VoucherDate.val
	,[ActAmtUSD] = ActAmtUSD.val
	,[APPExchageRate] = APPExchageRate.val
	,app.ID
FROM AirPP app
Outer apply
(
	SELECT [val] = STUFF ((SELECT DISTINCT CONCAT (',', tV.VoucherID) 
							FROM (
								SELECT	sa.VoucherID 
								FROM ShippingAP sa
								WHERE EXISTS(
									SELECT 1
									FROM ShareExpense_APP sea 
									INNER JOIN FinanceEN.dbo.AccountNo an ON sea.AccountID = an.ID
									WHERE	an.IsAPP=1 
											AND sea.ShippingAPID=sa.ID
											AND sea.AirPPID= app.ID
									)
							)tV
							WHERE isnull(tV.VoucherID,'' ) != ''										  
							FOR XML PATH('')
					), 1, 1, '')
 
)VoucherID
Outer apply
(
	SELECT [val] = (SELECT MIN (tV.VoucherDate)
					FROM (
						SELECT sa.VoucherDate
						FROM ShippingAP sa
						WHERE EXISTS(
							SELECT 1
							FROM ShareExpense_APP sea 
							INNER JOIN FinanceEN.dbo.AccountNo an ON sea.AccountID = an.ID
							WHERE	an.IsAPP=1 
									AND sea.ShippingAPID=sa.ID
									AND sea.AirPPID= app.ID
						)
					)tV
					where tV.VoucherDate IS NOT NULL)
 
)VoucherDate
 OUTER APPLY
 (
	select [val] = sum(ActAmtUSD)
	from 
	(
		select [ActAmtUSD] = cast(iif(sa.APPExchageRate = 0, 0, (sea.AmtFty + sea.AmtOther) / sa.APPExchageRate) as decimal(18,2)) 
		from ShareExpense_APP sea
		left join ShippingAP sa on sea.ShippingAPID = sa.ID 
		where sea.AirPPID = app.ID
	)ActAmtUSD 
 )ActAmtUSD
outer apply
(
	select [val] = min(sa.APPExchageRate)
	from ShareExpense_APP sea
	left join ShippingAP sa on sea.ShippingAPID = sa.ID
	outer apply (
		select [val] = min(AddDate)
		from ShippingAP
		where ID = sa.ID
	)minAddDate
	where sa.AddDate = minAddDate.val
	and sea.AirPPID = app.ID
)APPExchageRate 

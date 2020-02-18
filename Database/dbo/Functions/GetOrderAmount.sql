CREATE FUNCTION [dbo].[GetOrderAmount]
(
	@ID varchar(13)
)
RETURNS table
AS
RETURN
(
	select 
		getAmount.Amount,
		getAmount.OriAmount,
		isnull(q2.AmountSur ,0) AmountSur,
		isnull(q2.OriAmountSur ,0) OriAmountSur,
		isnull(getAmount.Amount, 0) + isnull(q2.AmountSur, 0) as TotalAmount,
		isnull(getAmount.OriAmount, 0) + isnull(q2.OriAmountSur, 0) as TotalOriAmount
	from Orders o
	outer apply (
		select 
			'Amount' = sum(isnull(oq.Qty * IsNull(COALESCE(ou.POPrice, ou2.POPrice, ou3.POPrice), 0), 0))
			,'OriAmount' = sum(isnull(oq.OriQty * IsNull(COALESCE(ou.POPrice, ou2.POPrice, ou3.POPrice), 0), 0))
		from Order_Qty oq
		left join Order_UnitPrice ou on ou.Id = oq.ID and ou.Article = oq.Article and ou.SizeCode = oq.SizeCode
		left join Order_UnitPrice ou2 on ou2.Id = oq.ID and ou2.Article = oq.Article and ou2.SizeCode = '----'
		left join Order_UnitPrice ou3 on ou3.Id = oq.ID and ou3.Article = '----' and ou3.SizeCode = '----'		
		where oq.ID = o.id
	) q1
	outer apply (
		select 
			'AmountSur' = sum(isnull(iif(os.PriceType = '1', os.Price, os.Price * qq.totalQty), 0))
			, 'OriAmountSur' = sum(isnull(iif(os.PriceType = '1', os.Price, os.Price * qq.totalOriQty), 0))
		from Order_Surcharge os
			outer apply (
			select sum(iif(isnull(COALESCE(ou.POPrice, ou2.POPrice, ou3.POPrice), 0) = 0, 0, Qty)) as totalQty
					, sum(iif(isnull(COALESCE(ou.POPrice, ou2.POPrice, ou3.POPrice), 0) = 0, 0, OriQty)) as totalOriQty
			from Order_Qty oq 
			left join Order_UnitPrice ou on ou.Id = oq.ID and ou.Article = oq.Article and ou.SizeCode = oq.SizeCode
			left join Order_UnitPrice ou2 on ou2.Id = oq.ID and ou2.Article = oq.Article and ou2.SizeCode = '----'
			left join Order_UnitPrice ou3 on ou3.Id = oq.ID and ou3.Article = '----' and ou3.SizeCode = '----'		
			where oq.id = os.id) qq
		where os.id	= o.id
	) q2
	outer apply (
		select iif(Year(o.CFMDate) <= 2014, (o.Qty - o.FOCQty) * o.PoPrice, q1.Amount) as Amount
		, iif(Year(o.CFMDate) <= 2014, (o.Qty - o.FOCQty) * o.PoPrice, q1.OriAmount) as OriAmount
	) getAmount
	where o.ID = @ID
)
 
GO

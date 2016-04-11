
CREATE FUNCTION [dbo].[getOrderUnitPriceByOrderSize](@styleUkey bigint, @sizecode varchar(8))
RETURNS decimal
BEGIN
	DECLARE @price decimal,
			@orderID varchar(13),
			@price1 decimal,
			@price2 decimal
	select @orderID = o.ID from Orders o,Order_Qty oq
	where o.ID = oq.ID and oq.SizeCode = @sizecode and o.StyleUkey = @styleUkey order by o.BuyerDelivery;

	select @price1 = POPrice from Order_UnitPrice where Id = @orderID and Article = '----' and SizeCode = '----'
	select @price2 = POPrice from Order_UnitPrice where Id = @orderID and SizeCode = @sizecode
	SET @price = IIF(@price1 is null,@price2,@price1);
	SET @price = IIF(@price is null,0,@price);
	RETURN @price
END
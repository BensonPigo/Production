
CREATE FUNCTION [dbo].[getOrderUnitPrice](@type tinyint, @styleUkey bigint,@orderid varchar(13),@article varchar(8), @sizecode varchar(8))
RETURNS numeric(7, 2)
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	DECLARE @price numeric(7, 2),
			@queryorderID varchar(13),
			@price1 numeric(7, 2),
			@price2 numeric(7, 2)
	if @type = 1 --By StyleUkey,Size
		begin
			select @queryorderID = o.ID from Orders o,Order_Qty oq
			where o.ID = oq.ID and oq.SizeCode = @sizecode and o.StyleUkey = @styleUkey order by o.BuyerDelivery;

			select @price1 = POPrice from Order_UnitPrice where Id = @queryorderID and Article = '----' and SizeCode = '----'
			select @price2 = POPrice from Order_UnitPrice where Id = @queryorderID and SizeCode = @sizecode
			SET @price = IIF(@price1 is null,@price2,@price1);
			SET @price = IIF(@price is null,0.0,@price);
		end
	else if @type = 2 --By OrderID,Article,SizeCode
		begin
			select @price1 = POPrice from Order_UnitPrice where Id = @orderID and Article = '----' and SizeCode = '----'
			select @price2 = POPrice from Order_UnitPrice where Id = @orderID and Article = @article and SizeCode = @sizecode
			SET @price = IIF(@price2 is null,@price1,@price2);
			SET @price = IIF(@price is null,0.0,@price);
		end
	else
		SET @price = 0.0
	RETURN @price
END
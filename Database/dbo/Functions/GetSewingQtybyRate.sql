
CREATE FUNCTION [dbo].[GetSewingQtybyRate](@orderid varchar(13), @article varchar(8), @sizecode varchar(8))

RETURNS decimal(15,4)
BEGIN
	DECLARE @ret decimal(15,4) = 0

	IF @article is null and @sizecode is null
	BEGIN
		select @ret =sum(isnull(sdd.QAQty,0) * isnull(ol.Rate,0) / 100)
		from Order_Location ol WITH (NOLOCK)
		left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = ol.Location
		where ol.OrderID = @orderid
	END
	ELSE
	IF @sizecode is null
	BEGIN
		select @ret = sum(isnull(sdd.QAQty,0) * isnull(ol.Rate,0) / 100)
		from Order_Location ol WITH (NOLOCK)
		left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = ol.Location and sdd.Article = @article
		where ol.OrderID = @orderid
	END
	ELSE
	BEGIN
		select @ret = sum(isnull(sdd.QAQty,0) * isnull(ol.Rate,0) / 100)
		from Order_Location ol WITH (NOLOCK)
		left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = ol.Location and sdd.Article = @article and sdd.SizeCode = @sizecode
		where ol.OrderID = @orderid
	END
	
	RETURN isnull(@ret,0);
end
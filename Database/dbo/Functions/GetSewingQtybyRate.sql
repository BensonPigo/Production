
CREATE FUNCTION [dbo].[GetSewingQtybyRate](@orderid varchar(13), @article varchar(8), @sizecode varchar(8))

RETURNS decimal(15,4)
BEGIN
	DECLARE @ret decimal(15,4) = 0

	IF @article is null and @sizecode is null
	BEGIN
		select @ret =sum(isnull(sdd.QAQty,0) * isnull(ol.Rate,0) / 100)
		from Order_Location ol WITH (NOLOCK)
		left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = ol.Location
		inner join SewingOutput so on sdd.ID = so.ID
		where ol.OrderID = @orderid and so.LockDate is not null
	END
	ELSE
	IF @sizecode is null
	BEGIN
		select @ret = sum(isnull(sdd.QAQty,0) * isnull(ol.Rate,0) / 100)
		from Order_Location ol WITH (NOLOCK)
		left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = ol.Location and sdd.Article = @article
		inner join SewingOutput so on sdd.ID = so.ID
		where ol.OrderID = @orderid and so.LockDate is not null
	END
	ELSE
	BEGIN
		select @ret = sum(isnull(sdd.QAQty,0) * isnull(ol.Rate,0) / 100)
		from Order_Location ol WITH (NOLOCK)
		left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = ol.Location and sdd.Article = @article and sdd.SizeCode = @sizecode
		inner join SewingOutput so on sdd.ID = so.ID
		where ol.OrderID = @orderid and so.LockDate is not null
	END
	
	RETURN isnull(@ret,0);
end

CREATE FUNCTION [dbo].GetSewingQtybyRate(@orderid varchar(13), @article varchar(8), @sizecode varchar(8))

RETURNS decimal(15,3)
BEGIN
	DECLARE @ret decimal(15,2) = 0
	DECLARE @StyleUnit varchar(5), @StyleUkey bigint
	select @StyleUkey = StyleUkey,@StyleUnit = StyleUnit from Orders WITH (NOLOCK) where ID = @orderid

	IF @StyleUnit = 'PCS'
	BEGIN
		IF @article is null and @sizecode is null
		BEGIN
			select @ret = sum(isnull(sdd.QAQty,0))
			from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
			where sdd.OrderId = @orderid
		END
		Else
		IF @sizecode is null
		BEGIN
			select @ret = sum(isnull(sdd.QAQty,0))
			from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
			where sdd.OrderId = @orderid
			and sdd.Article = @article
		END
		ELSE
		BEGIN
			select @ret = sum(isnull(sdd.QAQty,0))
			from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
			where sdd.OrderId = @orderid
			and sdd.Article = @article
			and sdd.SizeCode = @sizecode
		END 
	END	
	ELSE
	BEGIN
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
	END
	
	RETURN isnull(@ret,0);
end
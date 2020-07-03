
CREATE FUNCTION [dbo].[getMinCompleteSewQty](@orderid varchar(13), @article varchar(8), @sizecode varchar(8))
RETURNS int
BEGIN
	DECLARE @minSeqQty int = 0

	IF @article is null and @sizecode is null
	BEGIN
		select @minSeqQty = MIN(a.QAQty)
		from (
			select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
			from Order_Location sl WITH (NOLOCK)
			left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = sl.Location
			where sl.OrderID = @orderid
			group by sl.Location
		) a
	END
	ELSE
	IF @sizecode is null
	BEGIN
		select @minSeqQty = MIN(a.QAQty)
		from (
			select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
			from Order_Location sl WITH (NOLOCK)
			left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = sl.Location and sdd.Article = @article
			where sl.OrderID = @orderid
			group by sl.Location
		) a
	END
	ELSE
	BEGIN
		select @minSeqQty = MIN(a.QAQty)
		from (
			select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
			from Order_Location sl WITH (NOLOCK)
			left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = sl.Location and sdd.Article = @article and sdd.SizeCode = @sizecode
			where sl.OrderID = @orderid
			group by sl.Location
		) a
	END

	RETURN @minSeqQty
END


CREATE FUNCTION [dbo].[getMinCompleteSewTransferQty](@fromOrderid  varchar(13), @ToOrderid varchar(13), @article varchar(8), @sizecode varchar(8))
RETURNS int
BEGIN
	DECLARE @minSeqQty int
	DECLARE @StyleUnit as varchar(5), @StyleUkey as varchar(20)
	SET @minSeqQty = 0

	select @StyleUkey = StyleUkey,@StyleUnit = StyleUnit from Orders WITH (NOLOCK) where ID = @ToOrderid

	IF @article is null and @sizecode is null
	BEGIN
		select @minSeqQty = MIN(a.TransferQty)
		from (
			select ol.Location, TransferQty = isnull(sum(std.TransferQty),0)
			from Order_Location ol WITH (NOLOCK)
			outer apply(
				select std.TransferQty
				from SewingOutputTransfer_Detail std WITH (NOLOCK) 
				inner join SewingOutputTransfer st WITH (NOLOCK) on std.ID = st.id
				where std.FromOrderID = @fromOrderid and std.ToOrderID = ol.OrderID  and std.ToComboType = ol.Location
				and st.status = 'Confirmed'
			)std
			where ol.OrderID = @ToOrderid
			group by ol.Location
		) a
	END
	ELSE
		IF @sizecode is null
		BEGIN
			select @minSeqQty = MIN(a.TransferQty)
			from (
				select ol.Location, TransferQty = isnull(sum(std.TransferQty),0)
				from Order_Location ol WITH (NOLOCK)
				outer apply(
					select std.TransferQty
					from SewingOutputTransfer_Detail std WITH (NOLOCK) 
					inner join SewingOutputTransfer st WITH (NOLOCK) on std.ID = st.id
					where std.FromOrderID = @fromOrderid and std.ToOrderID = ol.OrderID  and std.ToComboType = ol.Location
					and std.Article = @article
					and st.status = 'Confirmed'
				)std
				where ol.OrderID = @ToOrderid
				group by ol.Location
			) a
		END
		ELSE
		BEGIN
			select @minSeqQty = MIN(a.TransferQty)
			from (
				select ol.Location, TransferQty = isnull(sum(std.TransferQty),0)
				from Order_Location ol WITH (NOLOCK)
				outer apply(
					select std.TransferQty
					from SewingOutputTransfer_Detail std WITH (NOLOCK) 
					inner join SewingOutputTransfer st WITH (NOLOCK) on std.ID = st.id
					where std.FromOrderID = @fromOrderid and std.ToOrderID = ol.OrderID  and std.ToComboType = ol.Location
					and std.Article = @article
					and std.SizeCode = @sizecode
					and st.status = 'Confirmed'
				)std
				where ol.OrderID = @ToOrderid
				group by ol.Location
			) a
		END
	RETURN @minSeqQty
END


CREATE FUNCTION [dbo].[getMinCompleteSewTransferQty](@fromOrderid  varchar(13), @ToOrderid varchar(13), @article varchar(8), @sizecode varchar(8))
RETURNS int
BEGIN
	DECLARE @minSeqQty int
	DECLARE @StyleUnit as varchar(5), @StyleUkey as varchar(20)
	SET @minSeqQty = 0

	select @StyleUkey = StyleUkey,@StyleUnit = StyleUnit from Orders WITH (NOLOCK) where ID = @ToOrderid

	IF @StyleUnit = 'PCS'
		BEGIN
		IF @article is null and @sizecode is null
			BEGIN
				select @minSeqQty = ToQty
				from (
					select sum(isnull(std.ToQty,0)) as ToQty
					from SewingOutputTransfer_Detail std WITH (NOLOCK) 
					inner join SewingOutputTransfer st WITH (NOLOCK) on std.ID = st.id
					where std.FromOrderID = @fromOrderid and std.ToOrderID = @ToOrderid
					and st.status = 'Confirmed'
				)a 
					
			END
		Else
			IF @sizecode is null
				BEGIN
					select @minSeqQty = ToQty
					from (
						select sum(isnull(std.ToQty,0)) as ToQty
						from SewingOutputTransfer_Detail std WITH (NOLOCK) 
						inner join SewingOutputTransfer st WITH (NOLOCK) on std.ID = st.id
						where std.FromOrderID = @fromOrderid and std.ToOrderID = @ToOrderid
						and std.ToArticle = @article
						and st.status = 'Confirmed'
					)a  
				END
			ELSE
				BEGIN
					select @minSeqQty = ToQty
					from (
						select sum(isnull(std.ToQty,0)) as ToQty
						from SewingOutputTransfer_Detail std WITH (NOLOCK) 
						inner join SewingOutputTransfer st WITH (NOLOCK) on std.ID = st.id
						where std.FromOrderID = @fromOrderid and std.ToOrderID = @ToOrderid
						and std.ToArticle = @article
						and std.ToSizeCode = @sizecode
						and st.status = 'Confirmed'
					)a  
				END 
		END	
	ELSE
		BEGIN
		IF @article is null and @sizecode is null
			BEGIN
				select @minSeqQty = MIN(a.ToQty)
				from (
					select sl.Location, sum(isnull(std.ToQty,0)) as ToQty
					from Order_Location sl WITH (NOLOCK)
					outer apply(
						select std.*
						from SewingOutputTransfer_Detail std WITH (NOLOCK) 
						inner join SewingOutputTransfer st WITH (NOLOCK) on std.ID = st.id
						where std.FromOrderID = @fromOrderid and std.ToOrderID = @ToOrderid and std.ToComboType = sl.Location
						and st.status = 'Confirmed'
					)std
					where sl.OrderID = @ToOrderid
					group by sl.Location
				) a
			END
		ELSE
			IF @sizecode is null
				BEGIN
					select @minSeqQty = MIN(a.ToQty)
					from (
						select sl.Location, sum(isnull(std.ToQty,0)) as ToQty
						from Order_Location sl WITH (NOLOCK)
						outer apply(
							select std.*
							from SewingOutputTransfer_Detail std WITH (NOLOCK) 
							inner join SewingOutputTransfer st WITH (NOLOCK) on std.ID = st.id
							where std.FromOrderID = @fromOrderid and std.ToOrderID = @ToOrderid and std.ToComboType = sl.Location
							and st.status = 'Confirmed'
						)std
						where sl.OrderID = @ToOrderid
						group by sl.Location
					) a
				END
			ELSE
				BEGIN
					select @minSeqQty = MIN(a.ToQty)
					from (
						select sl.Location, sum(isnull(std.ToQty,0)) as ToQty
						from Order_Location sl WITH (NOLOCK)
						outer apply(
							select std.*
							from SewingOutputTransfer_Detail std WITH (NOLOCK) 
							inner join SewingOutputTransfer st WITH (NOLOCK) on std.ID = st.id
							where std.FromOrderID = @fromOrderid and std.ToOrderID = @ToOrderid and std.ToComboType = sl.Location
							and st.status = 'Confirmed'
						)std
						where sl.OrderID = @ToOrderid
						group by sl.Location
					) a
				END
		END
	RETURN @minSeqQty
END
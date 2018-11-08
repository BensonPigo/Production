
Create FUNCTION [dbo].[getMinCompleteGMTQty](@orderid varchar(13), @article varchar(8), @sizecode varchar(8))
RETURNS int
BEGIN
	DECLARE @minSeqQty int --要回傳的數值
	DECLARE @StyleUnit as varchar(5), @StyleUkey as varchar(20)
	SET @minSeqQty = 0

	select @StyleUkey = StyleUkey,@StyleUnit = StyleUnit from Orders WITH (NOLOCK) where ID = @orderid
	IF @StyleUnit = 'PCS'
		BEGIN
		IF @article is null and @sizecode is null
			BEGIN
				select @minSeqQty = QAQty
				from (
					select sum(isnull(sdd.QAQty,0)) as QAQty
					from SewingOutput_Detail_Detail_Garment sdd WITH (NOLOCK) 
					where sdd.OrderIdFrom = @orderid)a 
			END
		Else
			IF @sizecode is null
				BEGIN
					select @minSeqQty = QAQty
					from (
						select sum(isnull(sdd.QAQty,0)) as QAQty
						from SewingOutput_Detail_Detail_Garment sdd WITH (NOLOCK) 
						where sdd.OrderIdFrom = @orderid
						and sdd.Article = @article)a  
				END
			ELSE
				BEGIN
					select @minSeqQty = QAQty
					from (
						select sum(isnull(sdd.QAQty,0)) as QAQty
						from SewingOutput_Detail_Detail_Garment sdd WITH (NOLOCK) 
						where sdd.OrderIdFrom = @orderid
						and sdd.Article = @article
						and sdd.SizeCode = @sizecode)a  
				END 
		END	
	ELSE
		BEGIN
		IF @article is null and @sizecode is null
			BEGIN
				select @minSeqQty = MIN(a.QAQty)
				from (select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
					  from Style_Location sl WITH (NOLOCK)
					  left join SewingOutput_Detail_Detail_Garment sdd WITH (NOLOCK) on sdd.OrderIdFrom = @orderid and sdd.ComboType = sl.Location
					  where StyleUkey = @StyleUkey
					  group by sl.Location) a
			END
		ELSE
			IF @sizecode is null
				BEGIN
					select @minSeqQty = MIN(a.QAQty)
					from (select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
					  from Style_Location sl WITH (NOLOCK)
					  left join SewingOutput_Detail_Detail_Garment sdd WITH (NOLOCK) on sdd.OrderIdFrom = @orderid and sdd.ComboType = sl.Location and sdd.Article = @article
					  where StyleUkey = @StyleUkey
					  group by sl.Location) a
				END
			ELSE
				BEGIN
					select @minSeqQty = MIN(a.QAQty)
					from (select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
					  from Style_Location sl WITH (NOLOCK)
					  left join SewingOutput_Detail_Detail_Garment sdd WITH (NOLOCK) on sdd.OrderIdFrom = @orderid and sdd.ComboType = sl.Location and sdd.Article = @article and sdd.SizeCode = @sizecode
					  where StyleUkey = @StyleUkey
					  group by sl.Location) a
				END	
		END
	RETURN @minSeqQty
END
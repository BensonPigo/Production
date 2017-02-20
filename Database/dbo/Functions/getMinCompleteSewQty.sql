
CREATE FUNCTION [dbo].[getMinCompleteSewQty](@orderid varchar(13), @article varchar(8), @sizecode varchar(8))
RETURNS int
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	DECLARE @minSeqQty int --要回傳的數值
	SET @minSeqQty = 0
	IF @article is null and @sizecode is null
		BEGIN
			select @minSeqQty = MIN(a.QAQty)
			from (select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
				  from Style_Location sl
				  left join SewingOutput_Detail_Detail sdd on sdd.OrderId = @orderid and sdd.ComboType = sl.Location
				  where StyleUkey = (select StyleUkey from Orders where ID = @orderid)
				  group by sl.Location) a
		END
	ELSE
		IF @sizecode is null
			BEGIN
				select @minSeqQty = MIN(a.QAQty)
				from (select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
				  from Style_Location sl
				  left join SewingOutput_Detail_Detail sdd on sdd.OrderId = @orderid and sdd.ComboType = sl.Location and sdd.Article = @article
				  where StyleUkey = (select StyleUkey from Orders where ID = @orderid)
				  group by sl.Location) a
			END
		ELSE
			BEGIN
				select @minSeqQty = MIN(a.QAQty)
				from (select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
				  from Style_Location sl
				  left join SewingOutput_Detail_Detail sdd on sdd.OrderId = @orderid and sdd.ComboType = sl.Location and sdd.Article = @article and sdd.SizeCode = @sizecode
				  where StyleUkey = (select StyleUkey from Orders where ID = @orderid)
				  group by sl.Location) a
			END
	RETURN @minSeqQty
END
Create FUNCTION [dbo].[getFOCInvAdjQty](@orderid varchar(13), @seq varchar(2))
RETURNS int
BEGIN
	DECLARE @int int --要回傳的字串
	select @int = isnull(sum(iq.DiffQty),0)
	from InvAdjust i WITH (NOLOCK)
	inner join InvAdjust_Qty iq WITH (NOLOCK) on i.ID =iq.ID 
	where i.OrderID = @orderid
	and i.OrderShipmodeSeq = @seq
	and iq.Price = 0

	RETURN @int
END
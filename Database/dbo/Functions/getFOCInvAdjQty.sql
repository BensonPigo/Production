
Create FUNCTION [dbo].[getFOCInvAdjQty](@orderid varchar(13), @seq varchar(2))
RETURNS int
BEGIN
	DECLARE @int int --要回傳的字串
	select @int = isnull(sum(i.AdjustPulloutQty),0)
	from InvAdjust i WITH (NOLOCK)
	inner join PackingList pl WITH (NOLOCK) on pl.INVNo = i.GarmentInvoiceID
	where i.OrderID = @orderid
	and i.OrderShipmodeSeq = @seq
	
	RETURN @int
END
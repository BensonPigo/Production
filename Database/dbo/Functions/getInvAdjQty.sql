
CREATE FUNCTION [dbo].[getInvAdjQty](@orderid varchar(13), @seq varchar(2))
RETURNS int
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	DECLARE @int int --要回傳的字串
	select @int = isnull(sum(iq.DiffQty),0)
	from InvAdjust i, InvAdjust_Qty iq
	where i.ID = iq.ID
	and i.OrderID = @orderid
	and i.OrderShipmodeSeq = @seq
	
	RETURN @int
END
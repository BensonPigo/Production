
-- =============================================
-- Author:		Aaron
-- Create date: 2017/10/24
-- Description:	getInvAdjQty 增加pulldate條件 for planning R17使用
-- =============================================
CREATE FUNCTION [dbo].[getInvAdjQtyByDate](@orderid varchar(13), @seq varchar(2),@Pulldate date,@Condition varchar(5))
RETURNS INT
AS
BEGIN
	DECLARE @AdjQty int

	IF @Condition = '<=' 
	BEGIN
		select @AdjQty = isnull(sum(iq.DiffQty),0)
				 from InvAdjust i WITH (NOLOCK), InvAdjust_Qty iq WITH (NOLOCK) 
				 where i.ID = iq.ID											
				 and i.OrderID = @orderid										
				 and i.OrderShipmodeSeq = isnull(@seq,i.OrderShipmodeSeq)				
				 and i.PullDate  <=  @Pulldate
	END
	ELSE
	BEGIN
		select @AdjQty = isnull(sum(iq.DiffQty),0)
				 from InvAdjust i WITH (NOLOCK), InvAdjust_Qty iq WITH (NOLOCK) 
				 where i.ID = iq.ID											
				 and i.OrderID = @orderid										
				 and i.OrderShipmodeSeq = isnull(@seq,i.OrderShipmodeSeq)				
				 and i.PullDate  >  @Pulldate

	END



	
	RETURN @AdjQty
END


create PROCEDURE [dbo].[Cutting_Color_P01_OrderQtyDown_POCombo]
	@OrderID varchar(13)
	,@ByType int = 0 --0單張 , 1 By CustCDID , 2 By PO
AS
BEGIN

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

exec Order_Report_QtyBreakdown @OrderID,@ByType
exec Order_Report_FabColorCombination @OrderID,@ByType
exec Order_Report_AccColorCombination @OrderID,@ByType


END
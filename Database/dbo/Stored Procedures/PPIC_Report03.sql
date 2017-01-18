

CREATE PROCEDURE [dbo].[PPIC_Report03]
	@OrderID varchar(13)
	,@ByType int = 0 --0單張 , 1 By CustCDID , 2 By PO
AS
BEGIN


exec Order_Report_QtyBreakdown @OrderID,@ByType
exec Order_Report_FabColorCombination @OrderID,@ByType
exec Order_Report_AccColorCombination @OrderID,@ByType


END
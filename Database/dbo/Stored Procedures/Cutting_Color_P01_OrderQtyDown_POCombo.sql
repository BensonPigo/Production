
CREATE PROCEDURE [dbo].[Cutting_Color_P01_OrderQtyDown_POCombo]
	@OrderID varchar(13)
	,@ByType int = 0 --0單張 , 1 By CustCDID , 2 By PO
	,@showJunk int = 1 --1 all, 0 排除Junk
AS
BEGIN


exec Cutting_Report_QtyBreakdown @OrderID,@ByType,0,0,@showJunk
exec Cutting_Report_FabColorCombination @OrderID,@ByType,@showJunk
exec Cutting_Report_AccColorCombination @OrderID,@ByType,@showJunk


END
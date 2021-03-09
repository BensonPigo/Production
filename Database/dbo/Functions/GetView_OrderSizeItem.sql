
CREATE FUNCTION [dbo].[GetView_OrderSizeItem]
(	
	@SizeGroup varchar(1) = ''
)
RETURNS TABLE 
AS
RETURN 
(
	Select Distinct osi.*
		, SizeGroup = isnull(ost.SizeGroup, @SizeGroup)
		, Lower = Isnull(ost.Lower, '')
		, Upper = Isnull(ost.Upper, '')
	From Order_SizeItem osi
	Left join Order_SizeTol ost on osi.Id = ost.Id And osi.SizeItem = ost.SizeItem 
		And (@SizeGroup = '' Or (@SizeGroup <> '' And ost.SizeGroup = @SizeGroup))
)
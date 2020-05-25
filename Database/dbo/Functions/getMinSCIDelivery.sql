

CREATE FUNCTION [dbo].[getMinSCIDelivery](@poid varchar(13),@category varchar(1))
RETURNS date
BEGIN
	DECLARE @minscidlv date --要回傳的數值
	Select @minscidlv = Min(Orders.SciDelivery)
	From dbo.Orders as MainPO  WITH (NOLOCK)
	left join dbo.Orders WITH (NOLOCK) on Orders.POID =@poID
	Where MainPO.ID = @PoID
	And ((@Category != '' And Orders.Category != 'S') 
		Or (@Category = '' And Not (MainPO.Category = 'B' And Orders.Category = 'S'))
		Or (MainPO.Category = Orders.Category))
	And (MainPO.Junk=0 or (MainPO.Junk=1 and MainPO.NeedProduction=1))
	RETURN @minscidlv
END
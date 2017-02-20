

CREATE FUNCTION [dbo].[getMinSCIDelivery](@poid varchar(13),@category varchar(1))
RETURNS date
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	DECLARE @minscidlv date --要回傳的數值
	Select @minscidlv = Min(Orders.SciDelivery)
	From dbo.Orders as MainPO 
	left join dbo.Orders on Orders.POID =@poID
	Where MainPO.ID = @PoID
	And ((@Category != '' And Orders.Category != 'S') 
		Or (@Category = '' And Not (MainPO.Category = 'B' And Orders.Category = 'S'))
		Or (MainPO.Category = Orders.Category))
	And Orders.Qty > 0
	RETURN @minscidlv
END
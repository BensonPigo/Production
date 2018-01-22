

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GetSCI]
(	
	 @PoID		VarChar(13)
	,@Category	VarChar(1)
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	select @PoID as ID , s1.* , s2.* 
		,dbo.GetRequestETA(@PoID) as RequestETA
		,isNull(s2.MinLETA ,dbo.GetRequestETA(@PoID)) as EstLETA
	from (
		Select Min(Orders.SciDelivery) as MinSciDelivery
			 , Min(Orders.BuyerDelivery) as MinBuyerDelivery
		  From dbo.Orders as MainPO 
		  left join dbo.Orders  on Orders.POID =@PoID
		 Where MainPO.ID = @PoID
		   And (   (@Category != '' And Orders.Category != 'S')
				Or (@Category = '' And Not (MainPO.Category = 'B' And Orders.Category = 'S'))
				Or (MainPO.Category = Orders.Category)
			   )
		   --And Orders.Qty > 0
	 ) as s1
	 outer apply (
	   Select  Min(Orders.SewInLIne) AS MinSewinLine
		 ,  Min(IIF(Orders.PFETA Is Null, IIF(Orders.Category != 'S', Orders.LETA, Orders.PFETA), Orders.PFETA)) as MinLETA
		 ,  Min(Orders.PFETA) as MinPFETA
		  From dbo.Orders
		  Where Orders.PoID = @PoID
		   --And Orders.Qty > 0
	 ) as s2 
)
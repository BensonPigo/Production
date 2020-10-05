USE Production

CREATE FUNCTION [dbo].[Get_ShippingMarkTemplate_DefineColumn]
(
	@PackingListID varchar(15),
	@OrderID varchar(13),
	@SCICtnNo varchar(15),
	@CTNStartNo varchar(6),
	@RefNo varchar(21)
)
RETURNS @returntable TABLE
(
	ID varchar(60) ,
	FromPMS bit  ,
	[Desc] varchar(200) ,
	[Value] varchar(100) 
)
AS
BEGIN
	INSERT @returntable	
	SELECT col.*
		,[Value]=
					CASE ID 
						WHEN 'CABCode' THEN Info.CAB
						WHEN 'Customer_PO' THEN Info.Customer_PO
						WHEN 'FinalDestination' THEN Info.FinalDest
						WHEN 'AFS_STOCK_CATEGORY' THEN Info.AFS_STOCK_CATEGORY
					ELSE ''
					END
	FROM ShippingMarkTemplate_DefineColumn col
	OUTER APPLY(
		SELECT DISTINCT CAB, Customer_PO ,FinalDest, AFS_STOCK_CATEGORY 
		FROM PackingList p
		INNER JOIN PackingList_Detail pd ON p.ID= pd.ID
		INNER JOIN Orders o ON o.ID = pd.OrderID
		WHERE p.ID = @PackingListID 
		AND pd.OrderID = @OrderID 
		AND pd.RefNo = @RefNo 
		AND pd.CTNStartNo = @CTNStartNo
		AND pd.SCICtnNo = @SCICtnNo
	)Info

	RETURN
END

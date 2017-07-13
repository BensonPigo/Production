
CREATE view [dbo].[Order_OrderComboList]
as
SELECT	Orders.ID
		, OrderComboList = IIF(Orders.ID = Orders.OrderComboID, OrderComboList.OrderComboList, 'master:' + Orders.OrderComboID) 
FROM	Orders 
LEFT JOIN (
	SELECT	ID
			, OrderComboList = (SELECT IIF(ID = OrderComboID, RTrim(OrderComboID)
															, '/' + Replace(Rtrim(ID), SubString(Rtrim(OrderComboID), 1, 10), ''))
                                FROM Orders AS tmp
                                WHERE tmp.OrderComboID = Orders.OrderComboID
                                ORDER BY ID 
								FOR XML path(''))
	FROM Orders
) OrderComboList ON Orders.ID = OrderComboList.ID
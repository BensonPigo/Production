CREATE VIEW [dbo].[View_ShareExpense]
	AS 
	SELECT	Junk
			,ShippingAPID
			,BLNo
			,WKNo
			,InvNo
			,Type
			,[GW] = sum(GW)
			,[CBM] = sum(CBM)
			,CurrencyID
			,[Amount] = sum(Amount)
			,ShipModeID
			,ShareBase
			,FtyWK
			,EditName
			,EditDate
			,AccountID
			,DebitID
	FROM [ShareExpense]
	group by Junk
			,ShippingAPID
			,BLNo
			,WKNo
			,InvNo
			,Type
			,CurrencyID
			,ShipModeID
			,ShareBase
			,FtyWK
			,EditName
			,EditDate
			,AccountID
			,DebitID
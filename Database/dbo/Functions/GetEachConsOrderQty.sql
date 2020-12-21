
Create Function [dbo].[GetEachConsOrderQty]
	(
	 @OrderID		VarChar(13)
	)
Returns Numeric(6,0)
As
Begin
	Return (
		select Qty = max(Qty) from (
			select oe.FabricCombo, Qty = SUM(oec.Orderqty)
			from Order_EachCons_Color oec
			inner join Order_EachCons oe on oec.Order_EachConsUkey = oe.Ukey
			WHERE oe.id = @OrderID AND OE.CuttingPiece = 0
			GROUP BY oe.FabricCombo
		) cc)
End
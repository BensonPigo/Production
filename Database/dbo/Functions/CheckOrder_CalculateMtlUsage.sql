
Create Function [dbo].[CheckOrder_CalculateMtlUsage]
(
   @ID		VarChar(13)
)
Returns BIT
As
Begin
	Return isnull((SELECT iif(Junk = 1 and NeedProduction = 0 and KeepPanels = 0, 0, 1) FROM Orders WHERE ID = @ID), 0)
End
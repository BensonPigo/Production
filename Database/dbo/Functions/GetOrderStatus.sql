
CREATE FUNCTION [dbo].[GetOrderStatus]
(
	@ID varchar(13)
)
Returns varchar(2000) 
As
Begin
	--測試用
	--Declare @POID varchar(13) = ''

	Declare @Status varchar(2000) = ''

	Select @Status = 
			Case When o.NeedProduction = 1 And o.Junk = 1
			Then 'Cancel Still need to continue production'
			When o.KeepPanels = 1 And o.Junk = 1
			Then 'Keep Panel without production'
			When o.IsBuyBack = 1
			Then 'Buy Back'
			When o.NeedProduction = 0 And o.KeepPanels = 0 And o.Junk = 1
			Then 'Cancel'
			Else ''
			End
	From Orders o with(nolock)
	Where o.ID = @ID
	Return @Status
End
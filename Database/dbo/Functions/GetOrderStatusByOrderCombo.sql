
CREATE FUNCTION [dbo].[GetOrderStatusByOrderCombo]
(
	@POID varchar(13)
	, @OrderID varchar(13)
)
Returns varchar(2000) 
As
Begin
	--測試用
	--Declare @OrderComboID varchar(13) = ''
	--Declare @POID varchar(13) = ''

	Declare @Status varchar(2000) = ''
	Declare @OrderComboID varchar(13) = (select top 1 OrderComboID from Orders where id = @OrderID)
	IF exists(Select Top 1 1 From Orders with(nolock) Where OrderComboID = @OrderComboID And IsBuyBack = 1)
	Begin 
		-- Buy Back
		Set @Status = 'Buy Back'
	End 
	Else
	Begin
		-- Cancel Still need to continue production
		-- Keep Panel without production
		-- Cancel
		Set @Status = Stuff((
			Select Concat(main.value, char(10))
			From (
				Select distinct 
					value = Case When o.NeedProduction = 1 And o.Junk = 1
							Then 'Cancel Still need to continue production: ' 
								+ iif(right(getNeedProductionComboList.value, 1) = '/'
									, SubString(getNeedProductionComboList.value, 1, len(getNeedProductionComboList.value) - 1)
									, getNeedProductionComboList.value)
							When o.KeepPanels = 1 And o.Junk = 1
							Then 'Keep Panel without production: ' 
								+ iif(right(getKeepPanelsComboList.value, 1) = '/'
									, SubString(getKeepPanelsComboList.value, 1, len(getKeepPanelsComboList.value) - 1)
									, getKeepPanelsComboList.value)
							When o.NeedProduction = 0 And o.KeepPanels = 0 And o.Junk = 1
							Then 'Cancel: ' 
								+ iif(right(getCancelComboList.value, 1) = '/'
									, SubString(getCancelComboList.value, 1, len(getCancelComboList.value) - 1)
									, getCancelComboList.value)
							End
					, Seq = Case When o.NeedProduction = 1 And o.Junk = 1 
							Then 1
							When o.KeepPanels = 1 And o.Junk = 1
							Then 2
							Else 3
							End
				From Orders o with(nolock)
				Outer apply (
					Select value = stuff(( 
							o.POID + SubString((Select '/' + REPLACE(do.ID, @POID, '') 
							From dbo.Orders do with(nolock)
							Where do.POID = o.POID And do.OrderComboID = o.OrderComboID And do.NeedProduction = 1 And do.Junk = 1
							Order by do.ID
							For xml path('')
							), 2, 13)
						), 1, 0, '')
				) getNeedProductionComboList
				Outer apply (
					Select value = stuff(( 
							o.POID + SubString((Select '/' + REPLACE(do.ID, @POID, '') 
							From dbo.Orders do with(nolock)
							Where do.POID = o.POID And do.OrderComboID = o.OrderComboID And do.KeepPanels = 1 And do.Junk = 1
							Order by do.ID
							For xml path('')
							), 2, 13)
						), 1, 0, '')
				) getKeepPanelsComboList
				Outer apply (
					Select value = stuff(( 
							o.POID + SubString((Select '/' + REPLACE(do.ID, @POID, '') 
							From dbo.Orders do with(nolock)
							Where do.POID = o.POID 
								And do.OrderComboID = o.OrderComboID
								And do.NeedProduction = 0
								And do.KeepPanels = 0
								And do.Junk = 1
							Order by do.ID
							For xml path('')
							), 2, 13)
						), 1, 0, '')
				) getCancelComboList
				Where o.POID = @POID And o.OrderComboID = @OrderComboID
			) main
			Where Isnull(main.value, '') <> ''
			Order by main.Seq
			For xml path('')
		), 1, 0, '')
	End
	Return @Status
End
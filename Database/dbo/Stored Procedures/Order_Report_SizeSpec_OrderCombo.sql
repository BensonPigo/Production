CREATE PROCEDURE [dbo].[Order_Report_SizeSpec_OrderCombo]
	@ID varchar(13)
	,@WithZ bit = 0
	,@fullsize bit = 1
AS
BEGIN

	Declare @POID varchar(13) = (select POID from Orders where ID = @ID)

	SELECT distinct a.SizeCode,Seq into #tmp_Col FROM Order_SizeCode a
	where a.Id = @POID and (@fullsize = 1 or (a.SizeCode in (select SizeCode from Order_Qty b where b.ID = @ID)))

	select col = 1 into #tmp_OC From Order_SizeSpec_OrderCombo b Where b.Id = @ID

	if exists(select 1 from #tmp_Col) and exists(select 1 from #tmp_OC)
	begin
		declare @str nvarchar(max)
		, @Unit nvarchar(10)
		, @Weight nvarchar(30)

		select @str = STUFF((SELECT ',['+SizeCode+']' FROM Order_SizeCode where id = @ID order by Seq FOR XML PATH('')),1,1,'')
		select top 1 @Unit = SizeUnit, @Weight = isnull(SizeUnitWeight, '') from Orders where Id = @POID
		if (@WithZ = 1) 
			SET @Weight = '     Weight Unit : ' + @Weight
		else
			SET @Weight = ''

		declare @sql nvarchar(max) = '
		Select oso.ID
			, oso.OrderComboID
			, oso.SizeItem
			, [Description] = os.SizeDesc
			, osc.SizeGroup
			, osC.SizeCode
		into #tmpA
		from Order_SizeSpec_OrderCombo oso
		full join Order_SizeCode osc on oso.Id = osc.Id
		Left join dbo.GetView_OrderSizeItem('''') os on oso.SizeItem = os.SizeItem and oso.Id = os.Id
		where oso.Id = '''+@ID+'''
		group by oso.ID,oso.OrderComboID,oso.SizeItem,os.SizeDesc, osc.sizeGroup, osC.SizeCode
		order by oso.OrderComboID,osc.SizeCode

		select oso.*, osc.SizeGroup
		into #tmpB
		from Order_SizeSpec_OrderCombo oso
		Left join Order_SizeCode osc on oso.id = osc.id And oso.SizeCode = osc.SizeCode
		where oso.id = '''+@ID+'''

		Select OrderComboID
		, SizeItem
		,[Description          Unit : '+ @Unit +' '+ @Weight +']= Description
		,'+ @str +'
		From(
			select a.OrderComboID
			, a.SizeItem
			, a.Description
			, a.sizeCode
			, b.sizeSpec
			From #tmpA a
			Left join #tmpB b on a.id = b.id and a.OrderComboID = b.OrderComboID and a.SizeGroup = b.SizeGroup and a.SizeCode = b.SizeCode and a.sizeitem = b.sizeitem
		)a PIVOT (max(SizeSpec) for SizeCode in ('+ @str +'))b

		drop table #tmpA
		drop table #tmpB
		'
		exec (@sql)

	end
	else
		select ' ' = null

	drop table #tmp_Col
	drop table #tmp_OC

End

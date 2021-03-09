
CREATE PROCEDURE [dbo].[PPIC_Report_SizeSpec]
	@ID varchar(13)
	,@WithZ bit = 0
	,@fullsize bit = 0
	,@ByType int = 0 --0單張 , 1 By OrderCombo , 2 By PO
AS
BEGIN
	
	--select SizeCode,Seq into #tmp_Col from dbo.GetSizeCodeColumnByID(@poid,1)
	declare @POID varchar(13) = (select POID from Orders where ID = @ID)

	SELECT distinct a.SizeCode,Seq into #tmp_Col FROM Order_SizeCode a
	where a.Id = @POID and (@fullsize = 1 or (a.SizeCode in (select SizeCode from Order_Qty b where b.ID = @ID)))

	if exists(select 1 from #tmp_Col)
		begin
			declare @str1 nvarchar(max), @Unit nvarchar(10), @Weight nvarchar(30)
			select @str1=STUFF((SELECT ',['+SizeCode+']' FROM #tmp_Col order by Seq FOR XML PATH('')),1,1,'')
			select top 1 @Unit = SizeUnit, @Weight = isnull(SizeUnitWeight, '') from Orders where Id = @POID
			
			if (@WithZ = 1) 
				SET @Weight = '     Weight Unit : ' + @Weight
			else
				SET @Weight = ''

			declare @sql nvarchar(max) = 'select SizeItem,[Description          Unit : '+ @Unit +' '+ @Weight +']=Description,'+ @str1 +' from (

				select a.SizeItem,[Description] = a.SizeDesc
					,iif(b.SizeCode is not null,b.SizeCode,c.SizeCode) as SizeCode
					,iif('+ cast(@ByType as varchar(1)) +' = ''1'' and exists(select 1 from Order_SizeSpec_OrderCombo oso where oso.ID = '''+ @poid +''' and oso.OrderComboID = '''+ @id +''' and oso.SizeItem = a.SizeItem), c.SizeSpec, b.SizeSpec ) as SizeSpec
				from Production.dbo.Order_SizeItem a
					left join Production.dbo.Order_SizeCode os on a.Id = os.Id
					left join Production.dbo.Order_SizeSpec b on a.Id = b.Id and a.SizeItem = b.SizeItem and b.SizeCode = os.SizeCode
					left join Production.dbo.Order_SizeSpec_OrderCombo c on a.Id = c.Id and c.OrderComboID = '''+ @id +''' and a.SizeItem = c.SizeItem and c.SizeCode = os.SizeCode --and (b.SizeCode = c.SizeCode or b.SizeCode is null)
				where a.Id = '''+ @poid +''' and (A.SizeItem like ''S%'' or ('+ cast(@WithZ as varchar(1)) +' = ''1'' and (A.SizeItem like ''Z%'' or A.SizeItem like ''W%'')))

			) a pivot (max(SizeSpec) for SizeCode in ('+ @str1 +')) b
			order by SizeItem'
		
			exec (@sql)
		end	
	else
		select ' ' = null

	drop table #tmp_Col
	

END
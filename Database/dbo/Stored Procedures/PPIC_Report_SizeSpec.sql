
CREATE PROCEDURE [dbo].[PPIC_Report_SizeSpec]
	@ID varchar(13)
	,@WithZ bit = 0
	,@fullsize bit = 0
	,@ByType int = 0 --0單張 , 1 By OrderCombo , 2 By PO
AS
BEGIN
	
	declare @POID varchar(13) = (select POID from Orders WITH (NOLOCK) where ID = @ID)

	SELECT distinct a.SizeCode,Seq into #tmp_Col FROM Order_SizeCode a WITH (NOLOCK)
	where a.Id = @POID and (@fullsize = 1 or (a.SizeCode in (select SizeCode from Order_Qty b WITH (NOLOCK) where b.ID = @ID)))
	
	if exists(select 1 from #tmp_Col)
		begin
			declare @str1 nvarchar(max), @Unit nvarchar(10)
			select @str1=STUFF((SELECT ',['+SizeCode+']' FROM #tmp_Col order by Seq FOR XML PATH('')),1,1,'')
			select top 1 @Unit = SizeUnit from Orders WITH (NOLOCK) where Id = @POID
			declare @sql nvarchar(max) = 'select SizeItem,[Description          Unit : '+ @Unit +']=Description,'+ @str1 +' from (
				select a.SizeItem,Description = a.SizeDesc
					,iif(b.SizeCode is not null,b.SizeCode,c.SizeCode) as SizeCode
					,iif('+ cast(@ByType as varchar(1)) +' = ''1'' 
					and exists(
						select 1 from Order_SizeSpec_OrderCombo oso 
						where oso.id = '''+ @POID +''' 
						and oso.OrderComboID = '''+ @id +''' 
						and oso.SizeItem = a.SizeItem), c.SizeSpec, b.SizeSpec ) as SizeSpec
					from Production.dbo.Order_SizeItem a
					left join Production.dbo.Order_SizeCode os on a.Id = os.Id
					left join Production.dbo.Order_SizeSpec b on a.Id = b.Id and a.SizeItem = b.SizeItem 
						and b.SizeCode = os.SizeCode
					left join Production.dbo.Order_SizeSpec_OrderCombo c on c.OrderComboID = '''+ @POID +''' 
						and a.SizeItem = c.SizeItem and c.SizeCode = os.SizeCode
				--and (b.SizeCode is not null or c.SizeCode is not null)
				where a.Id = '''+ @POID +''' AND ('+ cast(@WithZ as varchar(1)) +' = ''1'' or A.SizeItem like ''S%'')
			) a pivot (max(SizeSpec) for SizeCode in ('+ @str1 +')) b
			order by SizeItem'
		
			exec (@sql)
			print @sql
		end	
	else
		select ' ' = null

	drop table #tmp_Col
	

END
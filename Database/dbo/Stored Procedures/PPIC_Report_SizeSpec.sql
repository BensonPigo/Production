
CREATE PROCEDURE [dbo].[PPIC_Report_SizeSpec]
	@ID varchar(13)
	,@WithZ bit = 0
	,@fullsize bit = 0
	,@ByType int = 0 --0單張 , 1 By OrderCombo , 2 By PO
AS
BEGIN
	

	declare @POID varchar(13) = (select POID from MNOrder where ID = @ID)

	SELECT distinct a.SizeCode,Seq into #tmp_Col FROM MNOrder_SizeCode a WITH (NOLOCK) inner join MNOrder_Qty b WITH (NOLOCK) on a.Id = b.ID and (@fullsize = 1 or a.SizeCode = b.SizeCode)
	where a.Id = @POID

	if exists(select 1 from #tmp_Col)
		begin
			declare @str1 nvarchar(max), @Unit nvarchar(10)
			select @str1=STUFF((SELECT ',['+SizeCode+']' FROM #tmp_Col order by Seq FOR XML PATH('')),1,1,'')
			select top 1 @Unit = SizeUnit from Order_SizeItem where Id = @POID
			select top 1 @Unit = SizeUnit from MNOrder_SizeItem WITH (NOLOCK) where Id = @POID
			declare @sql nvarchar(max) = 'select SizeItem,[Description          Unit : '+ @Unit +']=SizeDesc,'+ @str1 +' from (
				--select a.SizeItem,a.Description,b.SizeCode,b.SizeSpec from Trade.dbo.MNOrder_SizeItem a
				--left join Production.dbo.Order_SizeSpec b on a.Id = b.Id and a.SizeItem = b.SizeItem

				select a.SizeItem,a.SizeDesc
					,iif(b.SizeCode is not null,b.SizeCode,c.SizeCode) as SizeCode
					,iif('+ cast(@ByType as varchar(1)) +' = ''1'' and c.SizeSpec is not null, c.SizeSpec, b.SizeSpec ) as SizeSpec
				from Production.dbo.MNOrder_SizeItem a
					left join Production.dbo.MNOrder_SizeCode os on a.Id = os.Id
					left join Production.dbo.MNOrder_SizeSpec b on a.Id = b.Id and a.SizeItem = b.SizeItem and b.SizeCode = os.SizeCode
					left join Production.dbo.MNOrder_SizeSpec_OrderCombo c on a.Id = c.Id and c.OrderComboID = '''+ @id +''' and a.SizeItem = c.SizeItem and c.SizeCode = os.SizeCode --and (b.SizeCode = c.SizeCode or b.SizeCode is null)
				where a.Id = '''+ @poid +''' AND ('+ cast(@WithZ as varchar(1)) +' = ''1'' or A.SizeItem like ''S%'')
				--and (b.SizeCode is not null or c.SizeCode is not null)

				--where a.Id = '''+ @poid +''' AND ('+ cast(@WithZ as varchar(1)) +' = ''1'' or A.SizeItem like ''S%'')
			) a pivot (max(SizeSpec) for SizeCode in ('+ @str1 +')) b
			order by SizeItem'
		
			exec (@sql)
		end	
	else
		select ' ' = null

	drop table #tmp_Col
	

END
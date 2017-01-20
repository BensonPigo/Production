

CREATE PROCEDURE [dbo].[PPIC_Report_SizeSpec]
	@POID varchar(13)
	,@WithZ bit = 0
	,@fullsize bit = 0
AS
BEGIN
		
	--select SizeCode,Seq into #tmp_Col from dbo.GetSizeCodeColumnByID(@poid,1)
	
	SELECT distinct a.SizeCode,Seq into #tmp_Col FROM MNOrder_SizeCode a inner join Order_Qty b on a.Id = b.ID and (@fullsize = 1 or a.SizeCode = b.SizeCode)
	where a.Id = @POID

	if exists(select 1 from #tmp_Col)
		begin
			declare @str1 nvarchar(max), @Unit nvarchar(10)
			select @str1=STUFF((SELECT ',['+SizeCode+']' FROM #tmp_Col order by Seq FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'')
			select top 1 @Unit = SizeUnit from MNOrder_SizeItem where Id = @POID
			declare @sql nvarchar(max) = 'select SizeItem,[Description          Unit : '+ @Unit +']=Description,'+ @str1 +' from (
				select a.SizeItem,a.Description,b.SizeCode,b.SizeSpec from MNOrder_SizeItem a
				left join MNOrder_SizeSpec b on a.Id = b.Id and a.SizeItem = b.SizeItem
				where a.Id = '''+ @poid +''' AND ('+ cast(@WithZ as varchar(1)) +' = ''1'' or A.SizeItem like ''S%'')
			) a pivot (max(SizeSpec) for SizeCode in ('+ @str1 +')) b
			order by SizeItem'
		
			exec (@sql)
		end	
	else
		select ' ' = null

	drop table #tmp_Col
	

END
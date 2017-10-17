
Create PROCEDURE [dbo].[Order_Report_QtyBreakdown]
	@OrderID varchar(13)
	,@ByType int = 0 --0單張 , 1 By OrderCombo , 2 By PO
AS
BEGIN

declare @OrderComboID varchar(13) = (select OrderComboID from MNOrder where ID = @OrderID)
declare @tbl table (seq bigint, id varchar(13), Article varchar(8))

if(@ByType = 0)
	insert into @tbl SELECT seq,id,Article FROM DBO.ORDER_ARTICLE WHERE ID = (select id from Production.dbo.MNOrder where id = @OrderID and qty !=0)
else if(@ByType = 1)
	insert into @tbl SELECT seq,id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from Production.dbo.MNOrder where OrderComboID = @OrderComboID and qty !=0)
else if(@ByType = 2)
	insert into @tbl SELECT seq,id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from Production.dbo.MNOrder where OrderComboID = @OrderComboID and qty !=0)

--主要資料
SELECT c.seq, b.id,b.Article,SizeCode,Qty into #tmp FROM @tbl a 
	left join DBO.Order_Qty b on a.Article = b.Article and a.id = b.ID
	left join ( select Article, min(seq) as seq from @tbl group by Article) c on a.Article = c.Article
where b.ID is not null


--有使用到的Size表
select * into #tmp_col from GetSizeCodeColumnByID(@OrderID,@ByType)


if exists(select 1 from #tmp_col)
	begin
		declare @str1 nvarchar(max),@str2 nvarchar(max),@str3 nvarchar(max),@str4 nvarchar(max)
		select @str1=STUFF((SELECT ',['+SizeCode+']' FROM #tmp_col order by Seq FOR XML PATH('')),1,1,'')
		select @str2=STUFF((SELECT '+isnull(['+SizeCode+'],0)' FROM #tmp_col order by Seq FOR XML PATH('')),1,1,'')
		select @str3=STUFF((SELECT ',sum(['+SizeCode+'])' FROM #tmp_col order by Seq FOR XML PATH('')),1,1,'')
		select @str4=STUFF((SELECT '+isnull(sum(['+SizeCode+']),0)' FROM #tmp_col order by Seq FOR XML PATH('')),1,1,'')

		declare @sql nvarchar(max) = 'select '' '' = Article,'+@str1+',Total from (
			select Seq, Article,'+ @str1 +',Total='+ @str2 +'
			from (SELECT Seq,Article,SizeCode,Qty FROM #tmp) a
			pivot ( sum(Qty) for SizeCode in ('+@str1+') ) b
			union
			select Seq=999, ''TTL.'','+ @str3 +',Total='+ @str4 +'
			from (SELECT Article,SizeCode,Qty FROM #tmp) a
			pivot ( sum(Qty) for SizeCode in ('+ @str1 +') ) b 
		) c order by Seq,Article'

		print @sql
		exec (@sql)
	end
else
	begin
		select * from (select 'null' = null) c where 1 = 2
	end

drop table #tmp
drop table #tmp_col


END
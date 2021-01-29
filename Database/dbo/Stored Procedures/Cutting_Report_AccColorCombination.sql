

CREATE PROCEDURE [dbo].[Cutting_Report_AccColorCombination]
	@OrderID varchar(13)
	,@ByType int = 0 --0單張 , 1 By OrderCombo , 2 By PO
	,@showJunk int = 1 --1 all, 0 排除Junk
AS
BEGIN

declare @poid varchar(13) = (select POID from Orders where ID = @OrderID)
declare @tbl table (seq bigint, id varchar(13), Article varchar(8))

if(@ByType = 0)
	insert into @tbl SELECT seq,id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from Production.dbo.Orders where id = @OrderID and (@showJunk = 1 or (@showJunk = 0 and dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1)))
else if(@ByType = 1)
	insert into @tbl SELECT seq,id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from Production.dbo.Orders where POID = @poid AND OrderComboID = @OrderID and (@showJunk = 1 or (@showJunk = 0 and dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1)))
else if(@ByType = 2)
	insert into @tbl SELECT seq,id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from Production.dbo.Orders where POID = @poid and (@showJunk = 1 or (@showJunk = 0 and dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1)))

SELECT tbl.seq,a.Article,c.ColorID,FabricPanelCode 
into #tmp FROM dbo.Order_ColorCombo a
left join dbo.Orders b on a.Id = b.ID
outer apply (	
	select ColorID=STUFF((SELECT CHAR(10)+ColorID FROM dbo.Color_multiple d where BrandID = b.BrandID and d.ID = a.ColorID FOR XML PATH('')),1,1,'')
) c
outer apply(select min(seq) seq,article from @tbl tbl where tbl.Article = a.Article group by Article ) tbl
WHERE a.ID = @poid and a.Article in (select article from @tbl) and a.FabricType = 'A'

if exists(select 1 from #tmp)
	begin
		declare @minCode nvarchar(3) = (select min(FabricPanelCode) from #tmp)
		declare @maxCode nvarchar(3) = (select max(FabricPanelCode) from #tmp)
		declare @rptcol nvarchar(max) = STUFF((
			--SELECT ',['+FabricPanelCode+']' FROM #tmp group by FabricPanelCode order by FabricPanelCode FOR XML PATH('')
			select ',['+Data+']' from dbo.SplitString('AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK,AL,AM,AN,AO,AP,AQ,AR,AS,AT,AU,AV,AW,AX,AY,AZ',',')
			where Data between @minCode	and @maxCode FOR XML PATH('')
		),1,1,'')
		declare @sql nvarchar(max) = 'select CODE=Article,'+@rptcol+' from (
		select * from #tmp
		) a pivot (max(ColorID) for FabricPanelCode in ('+@rptcol+')) b order by Seq,Article'
		exec (@sql)
	end
else
	begin
		select ' ' = null
	end

drop table #tmp

END
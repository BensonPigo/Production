
CREATE PROCEDURE [dbo].[Order_Report_AccColorCombination]
	@OrderID varchar(13)
	,@ByType int = 0 --0單張 , 1 By OrderCombo , 2 By PO
AS
BEGIN

declare @poid varchar(13) = (select POID from MNOrder where ID = @OrderID)
declare @tbl table (id varchar(13), Article varchar(8))

if(@ByType = 0)
	insert into @tbl SELECT id,Article FROM DBO.ORDER_ARTICLE WHERE ID = @OrderID
else if(@ByType = 1)
	insert into @tbl SELECT id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from MNOrder where POID = @poid AND OrderComboID = @OrderID)
else if(@ByType = 2)
	insert into @tbl SELECT id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from MNOrder where POID = @poid )

SELECT Article,c.ColorID,FabricPanelCode into #tmp FROM dbo.Order_ColorCombo a
left join dbo.MNOrder b on a.Id = b.ID
outer apply (	
	select ColorID=STUFF((SELECT CHAR(10)+ColorID FROM dbo.Color_multiple d where BrandID = b.BrandID and d.ID = a.ColorID FOR XML PATH('')),1,1,'')
) c
WHERE a.ID = @poid AND FABRICCODE = '' and a.Article in (select Article from @tbl)

if exists(select 1 from #tmp)
	begin
		declare @rptcol nvarchar(max) = STUFF((SELECT ',['+FabricPanelCode+']' FROM #tmp group by FabricPanelCode order by FabricPanelCode FOR XML PATH('')),1,1,'')
		declare @sql nvarchar(max) = 'select CODE=Article,'+@rptcol+' from (
		select * from #tmp
		) a pivot (max(ColorID) for FabricPanelCode in ('+@rptcol+')) b'
		exec (@sql)
	end
else
	begin
		select ' ' = null
	end

drop table #tmp

END
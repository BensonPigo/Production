
CREATE PROCEDURE [dbo].[Order_Report_FabColorCombination]
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
	
--FABRIC 主料，有FabricCode
SELECT Article,c.BrandID,d.ColorID,a.FabricPanelCode,QTFabricPanelCode=isnull(b.QTFabricPanelCode,''),FabricCode 
into #tmp FROM dbo.MNOrder_ColorCombo a
left join (
	SELECT Id,FabricPanelCode,QTFabricPanelCode FROM DBO.MNOrder_FabricCode_QT where FabricPanelCode <> QTFabricPanelCode
) b on a.Id = b.Id and a.FabricPanelCode = b.FabricPanelCode
left join dbo.MNOrder c on a.Id = c.ID
outer apply (	
	select ColorID=STUFF((SELECT CHAR(10)+ColorID FROM dbo.Color_multiple d where BrandID = c.BrandID and d.ID = a.ColorID FOR XML PATH('')),1,1,'')
) d
WHERE a.ID = @poid AND FABRICCODE != '' and a.Article in (select Article from @tbl)

if exists(select 1 from #tmp)
	begin
		declare @rptcol nvarchar(max) = STUFF((SELECT ',['+FabricPanelCode+']' FROM #tmp group by FabricPanelCode order by FabricPanelCode FOR XML PATH('')),1,1,'')
		declare @sql nvarchar(max) = 'SELECT CODE=Article,'+@rptcol+' FROM(
			select Article,ColorID,FabricPanelCode from #tmp
			union select ''MAT.'',FabricCode,FabricPanelCode from #tmp
			union select ''QT With'',QTFabricPanelCode,FabricPanelCode from #tmp
		) a pivot (
			max(ColorID) FOR FabricPanelCode in ('+@rptcol+')
		) b'

		exec (@sql)
	end
else
	begin
		select ' ' = null
	end

drop table #tmp

END
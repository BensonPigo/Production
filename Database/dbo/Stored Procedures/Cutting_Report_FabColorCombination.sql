
CREATE PROCEDURE [dbo].[Cutting_Report_FabColorCombination]
	@OrderID varchar(13)
	,@ByType int = 0 --0單張 , 1 By OrderCombo , 2 By PO
	,@showJunk int = 1 --1 all, 0 排除Junk
AS
BEGIN

declare @poid varchar(13) = (select POID from Orders where ID = @OrderID)
declare @tbl table  (seq bigint, id varchar(13), Article varchar(8))

if(@ByType = 0)
	insert into @tbl SELECT seq,id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from Production.dbo.Orders where id = @OrderID and (@showJunk = 1 or (@showJunk = 0 and dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1)))
else if(@ByType = 1)
	insert into @tbl SELECT seq,id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from Production.dbo.Orders where POID = @poid AND OrderComboID = @OrderID and (@showJunk = 1 or (@showJunk = 0 and dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1)))
else if(@ByType = 2)
	insert into @tbl SELECT seq,id,Article FROM DBO.ORDER_ARTICLE WHERE ID in (select id from Production.dbo.Orders where POID = @poid and (@showJunk = 1 or (@showJunk = 0 and dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1)))
	
--FABRIC 主料，有FabricCode
SELECT tbl.seq,cc.Article,c.BrandID,d.ColorID,a.FabricPanelCode,QTFabricPanelCode=isnull(b.QTFabricPanelCode,''),a.FabricCode 
into #tmp FROM dbo.Order_FabricCode a
left join dbo.Order_ColorCombo cc
	on a.FabricPanelCode = cc.FabricPanelCode and a.Id = cc.Id
left join (
	SELECT Id,FabricPanelCode,QTFabricPanelCode FROM DBO.Order_FabricCode_QT where FabricPanelCode <> QTFabricPanelCode
) b on a.Id = b.Id and a.FabricPanelCode = b.FabricPanelCode
left join dbo.Orders c on a.Id = c.ID
outer apply (	
	select ColorID=STUFF((SELECT CHAR(10)+ColorID FROM dbo.Color_multiple d where BrandID = c.BrandID and d.ID = cc.ColorID FOR XML PATH('')),1,1,'')
) d
outer apply(select min(seq) seq,Article from @tbl tbl where tbl.Article = cc.Article group by Article ) tbl
WHERE a.ID = @poid And cc.Article in (select article from @tbl) AND FabricType = 'F'

if exists(select 1 from #tmp)
	begin
		declare @minCode nvarchar(3) = (select min(FabricPanelCode) from #tmp)
		declare @maxCode nvarchar(3) = (select max(FabricPanelCode) from #tmp)
		declare @rptcol nvarchar(max) = STUFF((
			--SELECT ',['+FabricPanelCode+']' FROM #tmp group by FabricPanelCode order by FabricPanelCode FOR XML PATH('')
			select ',['+Data+']' from dbo.SplitString('A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z',',')
			where Data between @minCode	and @maxCode FOR XML PATH('')
		),1,1,'')			
		declare @sql nvarchar(max) = 'SELECT CODE=Article,'+@rptcol+' FROM(
			select seq,Article,ColorID,FabricPanelCode from #tmp
			union select seq=998,''MAT.'',FabricCode,FabricPanelCode from #tmp
			union select seq=999,''QT With'',QTFabricPanelCode,FabricPanelCode from #tmp
		) a pivot (
			max(ColorID) FOR FabricPanelCode in ('+@rptcol+')
		) b order by seq,Article'

		exec (@sql)
	end
else
	begin
		select ' ' = null
	end

drop table #tmp

END
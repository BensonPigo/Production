﻿
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CuttingP20calculateCutQty]
	@type int,--1:pre_qty, 2:all day qty
	@id varchar(20),
	@Cdate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select distinct o.ID,  o.MDivisionid
	into #tmpPOID 
	from CuttingOutput_Detail cd WITH (NOLOCK)
	inner join Orders o with (nolock) on cd.CuttingID = o.POID
	where cd.id = @id

	select	wd.OrderID, 
			wd.SizeCode, 
			wd.Article, 
			wp.PatternPanel, 
			wd.WorkOrderUkey, 
			cutqty = iif(sum(cod.Layer * ws.Qty) > wd.Qty, wd.Qty, sum(cod.Layer * ws.Qty)), 
			o.MDivisionid, 
			TotalCutQty = sum(cod.Layer * ws.qty) 
	into #CutQtytmp1
	from #tmpPOID o with (nolock)
	inner join WorkOrder_Distribute wd WITH (NOLOCK) on o.id = wd.OrderID		
	inner join WorkOrder_PatternPanel wp WITH (NOLOCK) on wp.WorkOrderUkey = wd.WorkOrderUkey
	inner join WorkOrder_SizeRatio ws WITH (NOLOCK) on ws.WorkOrderUkey = wd.WorkOrderUkey and ws.SizeCode = wd.SizeCode
	inner join CuttingOutput_Detail cod on cod.WorkOrderUkey = wd.WorkOrderUkey
	inner join CuttingOutput co WITH (NOLOCK) on co.id = cod.id
	where	((@type=0 and co.cdate <= @Cdate) or (@type=1 and co.cdate < @Cdate) or @type = 2) and
			co.Status <> 'New'
	group by wd.OrderID,wd.SizeCode,wd.Article,wp.PatternPanel,o.MDivisionid,wd.Qty,wd.WorkOrderUkey

	------------------
	select * ,AccuCutQty=sum(cutqty) over(partition by WorkOrderUkey,patternpanel,sizecode order by WorkOrderUkey,orderid)
		,Rowid=ROW_NUMBER() over(partition by WorkOrderUkey,patternpanel,sizecode order by WorkOrderUkey,orderid)
	into #CutQtytmp2
	from #CutQtytmp1
	------------------
	select *,Lagaccu= LAG(AccuCutQty,1,AccuCutQty) over(partition by WorkOrderUkey,patternpanel,sizecode order by WorkOrderUkey,orderid)
	into #Lagtmp
	from #CutQtytmp2 
	------------------
	select *,cQty=iif(TotalCutQty < AccuCutQty and TotalCutQty > Lagaccu,TotalCutQty-Lagaccu,cutqty)
	into #tmp2_1
	from #Lagtmp where TotalCutQty>= AccuCutQty or (TotalCutQty < AccuCutQty and TotalCutQty > Lagaccu)
	------------------
	select OrderID,SizeCode,Article,PatternPanel,MDivisionid,[cutqty] = sum(cQty)
	from #tmp2_1
	group by OrderID,SizeCode,Article,PatternPanel,MDivisionid

	drop table #CutQtytmp1, #CutQtytmp2, #Lagtmp, #tmp2_1
END
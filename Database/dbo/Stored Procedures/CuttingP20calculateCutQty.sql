
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CuttingP20calculateCutQty]
	@type int,--1:pre_qty
	@id varchar(20),
	@Cdate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select wd.OrderID,wd.SizeCode,wd.Article,wp.PatternPanel,wd.WorkOrderUkey,
		cutqty= iif(sum(cod.Layer*ws.Qty)>wd.Qty,wd.Qty,sum(cod.Layer*ws.Qty)),
		co.MDivisionid,
		TotalCutQty=sum(cod.Layer*ws.qty)
	into #CutQtytmp1
	from WorkOrder_Distribute wd WITH (NOLOCK)
	inner join WorkOrder_PatternPanel wp WITH (NOLOCK) on wp.WorkOrderUkey = wd.WorkOrderUkey
	inner join WorkOrder_SizeRatio ws WITH (NOLOCK) on ws.WorkOrderUkey = wd.WorkOrderUkey and ws.SizeCode = wd.SizeCode
	inner join CuttingOutput_Detail cod on cod.WorkOrderUkey = wd.WorkOrderUkey
	inner join CuttingOutput co WITH (NOLOCK) on co.id = cod.id and co.Status <> 'New'
	inner join orders o WITH (NOLOCK) on o.id = wd.OrderID
	where ((co.cdate <= @Cdate and @type=0) or(co.cdate < @Cdate and @type=1))
	and exists (select 1 from CuttingOutput_Detail WITH (NOLOCK) where CuttingOutput_Detail.ID = @ID and CuttingID = o.poid)
	group by wd.OrderID,wd.SizeCode,wd.Article,wp.PatternPanel,co.MDivisionid,wd.Qty,wd.WorkOrderUkey
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
	if @type = 0
	begin
		select OrderID,SizeCode,Article,PatternPanel,MDivisionid,[cutqty] = sum(cQty)
		from #tmp2_1
		group by OrderID,SizeCode,Article,PatternPanel,MDivisionid
	end
	else
	begin
		select OrderID,SizeCode,Article,PatternPanel,MDivisionid,[cutqty] = sum(cQty)
		from #tmp2_1
		group by OrderID,SizeCode,Article,PatternPanel,MDivisionid
	end
END
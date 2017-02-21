-- =============================================
-- Author:		<JEFF S01952>
-- Create date: <2017/02/18>
-- Description:	<Cutting_P01_print_CuttingWorkOrder 01>
-- =============================================
CREATE PROCEDURE [dbo].[Cutting_P01_print_CuttingWorkOrder]
	@OrderID VARCHAR(13)
AS
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	--抓取ID為POID
	select @OrderID=POID FROM dbo.Orders where ID = @OrderID

	select 
	PoList = isnull([dbo].getPOComboList(o.ID,o.POID),''),
	[CutLine] = concat(format(c.CutInLine,'yyyy/MM/dd'),'~',format(c.CutOffLine,'yyyy/MM/dd'))
	from Orders o ,Cutting c
	where  o.ID = c.ID and o.id = @OrderID
	--
	Select isnull(sum(Qty),0) as OrderQty from Orders where CuttingSp = @OrderID
	--
	select distinct sizecode
	into #tmp
	from WorkOrder_SizeRatio ws inner join workorder w on ws.WorkOrderUkey = w.Ukey
	where w.id = @OrderID
	order by sizecode
	DECLARE @cols NVARCHAR(MAX)= N''
	SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(sizecode),N',' + QUOTENAME(sizecode))
	from #tmp
	drop table #tmp
	DECLARE @sql NVARCHAR(MAX)
	SET @sql = N'
	select *
	into #tmp2
	from(
		select Ukey,sizecode,Qty
		from WorkOrder w left join WorkOrder_SizeRatio ws on ws.WorkOrderUkey =w.Ukey 
		where w.id = '''+ @OrderID +N'''
	) asp
	pivot(
		max(Qty) for sizecode in ('+@cols+N')
	) as pt

	select 
	[CutRef#] = w.CutRef,
	[MarkerName]=w.Markername,w.FabricCombo,w.Cutno,
	'+@cols+N',
	[SP#]=w.OrderID,
	w.MarkerLength,
	[Cut Perimeter] = C.ActCuttingPerimeter,
	[Unit Cons] = w.ConsPC,[Layers]=w.Layer,
	[Article] = stuff(A.Article,1,1,''''),--[Article]=舊系統的ColorWay
	[Color] = w.Colorid,
	[Cut Qty] = B.cutqty,
	[Acumm.Qty] = sum(B.cutqty) over (order by w.cutref),
	[Cons] = w.Cons,
	[Acumm.Con] = sum(w.Cons) over (order by w.cutref)
	from WorkOrder w inner join #tmp2 t on w.Ukey = t.Ukey
	outer apply
	(
		Select Article =
		( 
			select distinct concat(''/'',Article )
			From dbo.WorkOrder_Distribute b
			Where b.workorderukey = w.Ukey and b.article!=''''
			For XML path('''')
		)
	)A
	outer apply
	(
		select  cutqty =
		(		
			Select sum(c.qty * w2.layer)
			From WorkOrder_SizeRatio c inner join WorkOrder w2 on c.WorkOrderUkey = w2.Ukey
			Where  c.WorkOrderUkey =w.Ukey 
			group by w2.ID,w2.Markername,w2.MarkerNo
		)
	)B
	outer apply
	(
		select ActCuttingPerimeter = 
		(
			select om.ActCuttingPerimeter
			from WorkOrder w3
			left join Order_MarkerList om on w3.id = om.id and w3.Markername = om.MarkerName and w3.MarkerNo = om.MarkerNo
			where w3.ukey = w.ukey
		)
	)C
	where w.id = '''+@OrderID+N'''
	order by w.CutRef,w.Markername,w.FabricCombo,w.Cutno
	'
	EXEC sp_executesql @sql
END
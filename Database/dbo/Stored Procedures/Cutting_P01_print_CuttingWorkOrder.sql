-- =============================================
-- Author:		<JEFF S01952>
-- Create date: <2017/02/18>
-- Description:	<Cutting_P01_print_CuttingWorkOrder 01>
-- =============================================
CREATE PROCEDURE [dbo].[Cutting_P01_print_CuttingWorkOrder]
	@OrderID VARCHAR(13)
AS
BEGIN	
	DECLARE @Id VARCHAR(13) = ''
	SELECT TOP 1 @Id = ID FROM WorkOrder WHERE ID = @OrderID
	IF @Id = ''
	BEGIN
		RETURN;
	END

	--抓取ID為POID
	select @OrderID=POID FROM dbo.Orders where ID = @OrderID
	select 
	PoList = isnull([dbo].getPOComboList(o.ID,o.POID),''),
	o.StyleID,
	[CutLine] = concat(format(c.CutInLine,'yyyy/MM/dd'),'~',format(c.CutOffLine,'yyyy/MM/dd'))
	from Orders o ,Cutting c
	where  o.ID = c.ID and o.id = @OrderID
	--
	Select isnull(sum(Qty),0) as OrderQty from Orders where CuttingSp = @OrderID
	--
	select	distinct Seq		
			, SizeCode
	into #tmp
	from Order_SizeCode os
	where	os.SizeCode in (select SizeCode 
							from WorkOrder_SizeRatio ws 
							inner join workorder w on ws.WorkOrderUkey = w.Ukey
							where w.id = @OrderID)
			and os.Id = @OrderID
	order by Seq
	--

	DECLARE @cols NVARCHAR(MAX)= N''
	SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(sizecode),N',' + QUOTENAME(sizecode))
	from #tmp
	order by Seq asc

	DECLARE @cols2 NVARCHAR(MAX)= N''
	SELECT @cols2 = @cols2 + iif(@cols2 = N'',N'isnull(' +QUOTENAME(sizecode)+',999)',N',isnull(' + QUOTENAME(sizecode)+',999)')
	from #tmp
	order by Seq desc
	drop table #tmp

	DECLARE @sql NVARCHAR(MAX)
	SET @sql = N'
	select *
	into #tmp2
	from(
		select	Ukey
				, sizecode,Qty
				,ct=COUNT(1) over(PARTITION by Ukey)
		from WorkOrder w 
		left join WorkOrder_SizeRatio ws on ws.WorkOrderUkey = w.Ukey 
		where w.id = '''+ @OrderID +N'''
	) asp
	pivot(
		max(Qty) for sizecode in ('+@cols+N')
	) as pt

	select	[CutRef#] = w.CutRef
			, [Marker Name] = w.Markername
			, [Fabric Combo] = w.FabricCombo
			, w.Cutno
			, '+@cols+N'
			, [SP#] = w.OrderID
			, w.MarkerLength
			, [Cut Perimeter] = C.ActCuttingPerimeter
			, [Unit Cons] = w.ConsPC
			, [Layers] = w.Layer
			, [Article] = A.Article
			--[Article]=舊系統的ColorWay
			, [Color] = w.Colorid
			, [Cut Qty] = B.cutqty
			, [Acumm.Qty] = sum(B.cutqty) over (order by w.cutref)
			, [Cons] = w.Cons
			, [Acumm.Con] = sum(w.Cons) over (order by w.cutref)
	from WorkOrder w 
	inner join #tmp2 t on w.Ukey = t.Ukey
	outer apply
	(
		Select Article = stuff(( 
							select distinct concat(''/'',Article )
							From dbo.WorkOrder_Distribute b
							Where b.workorderukey = w.Ukey and b.article!=''''
							For XML path('''')
						), 1, 1, '''')
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
			select distinct om.ActCuttingPerimeter
			from WorkOrder w3
			left join Order_EachCons om on w3.id = om.id and w3.Markername = om.MarkerName and w3.MarkerNo = om.MarkerNo
			where w3.ukey = w.ukey
		)
	)C
	where w.id = '''+@OrderID+N'''
	order by w.FabricCombo,w.Colorid,isnull(w.CutNo,9999),iif(t.ct>1,2,1), '+@cols2+N' ,w.Markername

	select	distinct Info = concat(''<'', wOrder.FabricPanelCode, ''>#'', wOrder.Refno, '' '', F.Description)
	from WorkOrder wOrder
	left join Fabric F on wOrder.SCIRefno = F.SCIRefno
	where wOrder.ID = '''+@OrderID+N'''
	'
	EXEC sp_executesql @sql
END
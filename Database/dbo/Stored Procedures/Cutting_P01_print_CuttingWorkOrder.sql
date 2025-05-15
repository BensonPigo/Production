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
	SELECT TOP 1 @Id = ID FROM WorkOrderForOutput WHERE ID = @OrderID
	IF @Id = ''
	BEGIN
		RETURN;
	END

	--���ID��POID
	select @OrderID=POID FROM dbo.Orders where ID = @OrderID
	select 
	PoList = isnull([dbo].getPOComboList(o.ID,o.POID),''),
	o.StyleID,
	[CutLine] = concat(format(c.CutForOutputInline,'yyyy/MM/dd'),'~',format(c.CutForOutputOffLine,'yyyy/MM/dd'))
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
							from WorkOrderForOutput_SizeRatio ws 
							inner join WorkOrderForOutput w on ws.WorkOrderForOutputUkey = w.Ukey
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
		from WorkOrderForOutput w 
		left join WorkOrderForOutput_SizeRatio ws on ws.WorkOrderForOutputUkey = w.Ukey 
		where w.id = '''+ @OrderID +N'''
	) asp
	pivot(
		max(Qty) for sizecode in ('+@cols+N')
	) as pt

	select	[CutRef#] = w.CutRef
			, [Fabric Kind] = D.FabricKind
			, [Marker Name] = w.Markername
			, [Pattern Panel] = wp1.PatternPanel_CONCAT
			, w.Cutno
			, '+@cols+N'
			, [SP#] = w.OrderID
			, w.MarkerLength
			, [Cut Perimeter] = C.ActCuttingPerimeter
			, [Unit Cons] = w.ConsPC
			, [Layers] = w.Layer
			, [Article] = A.Article
			--[Article]=�¨t�Ϊ�ColorWay
			, [Color] = w.Colorid
			, [Cut Qty] = B.cutqty
			, [Acumm.Qty] = sum(B.cutqty) over (order by w.cutref)
			, [Cons] = w.Cons
			, [Acumm.Con] = sum(w.Cons) over (order by w.cutref)
	from WorkOrderForOutput w 
	inner join #tmp2 t on w.Ukey = t.Ukey
	outer apply
	(
		Select Article = stuff(( 
							select distinct concat(''/'',Article )
							From dbo.WorkOrderForOutput_Distribute b
							Where b.WorkOrderForOutputukey = w.Ukey and b.article!=''''
							For XML path('''')
						), 1, 1, '''')
	)A
	outer apply
	(
		select  cutqty =
		(		
			Select sum(c.qty * w2.layer)
			From WorkOrderForOutput_SizeRatio c inner join WorkOrderForOutput w2 on c.WorkOrderForOutputUkey = w2.Ukey
			Where  c.WorkOrderForOutputUkey =w.Ukey 
			group by w2.ID,w2.Markername,w2.MarkerNo
		)
	)B
	outer apply
	(
		select ActCuttingPerimeter = 
		(
			select distinct om.ActCuttingPerimeter
			from WorkOrderForOutput  w3
			left join Order_EachCons om on w3.id = om.id and w3.Markername = om.MarkerName and w3.MarkerNo = om.MarkerNo
			where w3.ukey = w.ukey
		)
	)C
	outer apply
	(
		SELECT TOP 1 FabricKind = DD.id + ''-'' + DD.NAME 
		FROM dropdownlist DD 
		OUTER apply(
			SELECT
				OB.kind, 
				OCC.id, 
				OCC.article, 
				OCC.colorid, 
				OCC.fabricpanelcode, 
				OCC.patternpanel 
			FROM order_colorcombo OCC 
			INNER JOIN order_bof OB 
			ON OCC.id = OB.id 
			AND OCC.fabriccode = OB.fabriccode
		) LIST 
		WHERE LIST.id = w.id
		AND LIST.patternpanel = w.FabricCombo
		AND DD.[type] = ''FabricKind'' 
		AND DD.id = LIST.kind 
	)D
	OUTER APPLY (
		SELECT PatternPanel_CONCAT = STUFF((
			SELECT DISTINCT CONCAT(''+'', PatternPanel)
			FROM WorkOrderForOutPut_PatternPanel  WITH (NOLOCK) 
			WHERE WorkOrderForOutPutUkey  = w.Ukey
			FOR XML PATH ('''')), 1, 1, '''')
	) wp1


	where w.id = '''+@OrderID+N'''
	order by wp1.PatternPanel_CONCAT,isnull(w.CutNo,9999),iif(t.ct>1,2,1), '+@cols2+N' ,w.Markername

	select	distinct Info = concat(''<'', wOrder.FabricPanelCode, ''>#'', wOrder.Refno, '' '', F.Description)
	from WorkOrderForOutput wOrder
	left join Fabric F on wOrder.SCIRefno = F.SCIRefno
	where wOrder.ID = '''+@OrderID+N'''
	'

	EXEC sp_executesql @sql
END
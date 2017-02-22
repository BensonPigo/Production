-- =============================================
-- Author:		<JEFF S01952>
-- Create date: <2017/02/20>
-- Description:	<[Cutting_P01_QtyBreakdown_PoCombbySPList] 05>
-- =============================================
CREATE PROCEDURE [dbo].[Cutting_P01_QtyBreakdown_PoCombbySPList]
	@OrderID VARCHAR(13)
AS
BEGIN
	select distinct sizecode
	into #tmp
	from WorkOrder_Distribute
	where id = @OrderID
	order by sizecode
	DECLARE @cols NVARCHAR(MAX)= N''
	SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(sizecode),N',' + QUOTENAME(sizecode))
	from #tmp
	DECLARE @cols2 NVARCHAR(MAX)= N''
	SELECT @cols2 = @cols2 + iif(@cols2 = N'','sum('+QUOTENAME(sizecode)+')',N',sum(' + QUOTENAME(sizecode)+')')
	from #tmp
	drop table #tmp

	DECLARE @sql NVARCHAR(MAX)
	SET @sql = N'
	;with a as(
		select *
		from (
			select OrderID,SizeCode,Qty
			from WorkOrder_Distribute 
			where id = '''+@OrderID+N'''
		) asp
		pivot(
			max(Qty) for sizecode in ('+@cols+N')
		) as pt
	)
	SELECT distinct
		[SP#] = o.ID,
		[Style] = o.StyleID,
		oa.Article,
		oc.ColorID,
		[OrderNo] = o.Customize1,
		[PONo] = o.CustPONo,
		[CustCD] = o.CustCDID,
		[TTL] = o.Qty,
		'+@cols+N'
	into #tmp2
	FROM orders o
	inner join Order_Article oa on o.ID = oa.id
	inner join a on a.OrderID = o.ID
	inner join Order_ColorCombo oc on oa.Article = oc.Article and oc.PatternPanel = ''FA''
	where o.POID = '''+@OrderID+N'''
	order by o.ID
	select * from #tmp2
	union all
	select ''Total'','''','''','''','''','''','''',sum(TTL),'+@cols2+N'
	from #tmp2
	drop table #tmp2
	'
	EXEC sp_executesql @sql
END
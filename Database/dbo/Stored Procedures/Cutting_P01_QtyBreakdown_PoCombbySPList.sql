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
	SELECT
		[SP#] = o.ID,
		[Style] = o.StyleID,
		oa.Article,
		[OrderNo] = o.Customize1,
		[PONo] = o.CustPONo,
		[CustCD] = o.CustCDID,
		[TTL] = o.Qty,
		'+@cols+N'
	FROM orders o
	inner join Order_Article oa on o.ID = oa.id
	inner join a on a.OrderID = o.ID
	where o.POID = '''+@OrderID+N'''
	order by o.ID
	'
	EXEC sp_executesql @sql
END
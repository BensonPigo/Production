-- =============================================
-- Author:		<JEFF S01952>
-- Create date: <2017/02/20>
-- Description:	<[Cutting_P01_QtyBreakdown_PoCombbySPList] 05>
-- =============================================
CREATE PROCEDURE [dbo].[Cutting_P01_QtyBreakdown_PoCombbySPList]
	@OrderID VARCHAR(13)
AS
BEGIN
	DECLARE @Id VARCHAR(13) = ''
	SELECT TOP 1 @Id = ID FROM WorkOrder WHERE ID = @OrderID
	IF @Id = ''
	BEGIN
		RETURN;
	END

	
	select distinct sizecode,Seq
	into #tmp
	from Order_SizeCode 
	where sizecode in (
	SELECT oq.sizecode	
         FROM   orders o WITH (nolock) 
                INNER JOIN order_qty oq WITH (nolock) 
                        ON o.id = oq.id 
                LEFT JOIN order_article oa WITH (nolock) 
                       ON oa.id = oq.id 
                          AND oa.article = oq.article 
				left join order_colorcombo oc
				 ON oa.article = oc.article 
                 AND oc.patternpanel = 'FA' 
                 AND oc.id = o.poid 
         WHERE  o.poid = @OrderID
	)
	and id = @OrderID
	order by Seq

	DECLARE @cols NVARCHAR(MAX)= N''
	SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(sizecode),N',' + QUOTENAME(sizecode))
	from #tmp
	order by seq


	drop table #tmp
		
	DECLARE @sql NVARCHAR(MAX)
	SET @sql = N'
	WITH tmpdata 
     AS (SELECT 
				[SP#]=o.id,
				[Style]=o.StyleID, 
                oa.article, 
				oc.colorid, 
				oq.sizecode,
				[OrderNo] = o.customize1, 
                [PONo] = o.custpono, 
                [CustCD] = o.custcdid,                 
                oq.qty 				
         FROM   orders o WITH (nolock) 
                INNER JOIN order_qty oq WITH (nolock) 
                        ON o.id = oq.id 
                LEFT JOIN order_article oa WITH (nolock) 
                       ON oa.id = oq.id 
                          AND oa.article = oq.article 
				left join order_colorcombo oc
				 ON oa.article = oc.article 
                 AND oc.patternpanel = ''FA'' 
                 AND oc.id = o.poid 
         WHERE  o.poid = '''+@OrderID+N'''), 
     subtotal 
     AS (SELECT ''Total''       AS ID, 
				''''       as StyleID,
                ''''    AS Article, 
				''''       as colorid,
                sizecode, 
				''''		 as OrderNo,
				''''		 as PONo,                
				''''		 as CustCD,
				Sum(qty) AS Qty
         FROM   tmpdata 
         GROUP  BY sizecode), 
     uniondata 
     AS (SELECT * 
         FROM   tmpdata 
         UNION ALL 
         SELECT * 
         FROM   subtotal), 
     pivotdata 
     AS (SELECT * 
         FROM   uniondata 
                PIVOT( Sum(qty) 
                     FOR sizecode IN ('+@cols+N' )) a) 			
SELECT *, 

       [TTL]=(SELECT Sum(qty) 
        FROM   uniondata 
        WHERE  SP# = p.SP#
               AND article = p.article) 	
FROM   pivotdata p 
	'
	EXEC sp_executesql @sql
END


-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2017/12/08>
-- Description:	<[Cutting_P01_QtyBreakdown_PoCombbySPList] Table Of Goods 
-- =============================================
CREATE PROCEDURE [dbo].[Cutting_P01_QtyBreakdown_PoCombbySPList_TableOfGoods]
	@PoID VARCHAR(13)
AS
BEGIN
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
         WHERE  o.poid = @PoID
	)
	and id = @PoID
	order by Seq

	DECLARE @cols NVARCHAR(MAX)= N''
	SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(sizecode),N',' + QUOTENAME(sizecode))
	from #tmp
	order by seq

	print @cols;


	drop table #tmp
		
	DECLARE @sql NVARCHAR(MAX)
	SET @sql = N'
	WITH tmpdata AS (
		SELECT	[Orderid]=o.id,
				[Style]=o.StyleID, 
                oa.article, 
				oc.colorid, 
				oq.sizecode,
				[OrderNo] = o.customize1, 
                [PONo] = o.custpono, 
                [CustCD] = o.custcdid,                 
                oq.qty ,
				o.BrandID ,	
				o.Dest ,
				o.BuyerDelivery		 				
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
         WHERE  o.poid = '''+@PoID+N'''
	), 
    subtotal AS (
		SELECT	''''       AS ID, 
				''''       as StyleID,
                Article, 
				colorid,
                sizecode, 
				''''		 as OrderNo,
				''''		 as PONo,                
				''''		 as CustCD,
				Sum(qty) AS Qty,
				'''' as BrandID,
				'''' as Dest,
				null as BuyerDelivery
         FROM   tmpdata 
         GROUP  BY sizecode,Article,colorid
	), 
	totalAll AS (
		SELECT	''Total'' + '''+@PoID+N'''      AS ID, 
				''''         as StyleID,
                ''ZZ''		 as Article, 
				''''		 as colorid,
                sizecode, 
				''''		 as OrderNo,
				''''		 as PONo,                
				''''		 as CustCD,
				Sum(qty) AS Qty,
				'''' as BrandID,
				'''' as Dest,
				null as BuyerDelivery
         FROM   tmpdata 
         GROUP  BY sizecode
		 	 ),
    uniondata AS (
		SELECT * 
        FROM   tmpdata 
        
		UNION ALL 
        SELECT * 
        FROM   subtotal

		UNION ALL 
        SELECT * 
        FROM   totalAll
	)
	select * 
	into #tmpUniondata
	from uniondata

	SELECT * 
	into #tmpAll
    FROM   #tmpUniondata 
    PIVOT( Sum(qty) FOR sizecode IN ('+@cols+N' )) a
	
	SELECT distinct
	[SHELL A/ SIZE] = case when (orderid<>'''' and Style<>'''') 
	then isnull(colordes.ColorName,'''') + CHAR(13) + ''<'' +isnull(p.Article,'''') +''>''
	else iif(substring(isnull(p.Orderid,''''),1,5)=''Total'',''Total: ''+ '''+@PoID+N''', ''Total: '' + isnull(p.ColorID,'''') + ''<'' + isnull(p.Article,'''') + ''>'') end
	, Article
	, [Sewing Line] =isnull(Line.SewingLineID ,line2.SewingLineID)
	, '+@cols+N'
	, [TOTAL] = (SELECT Sum(qty) 
				  FROM   #tmpUniondata 
				  WHERE  Orderid = p.Orderid
						 AND article = p.article) 	
	, [SPNO] = SUBSTRING(p.Orderid,9,LEN(p.orderid)) + '' - ''+ country.NameEN
	, [OrderNo] = p.OrderNo
	, [P.O.No] = p.PONo
	, [CUST CD] = p.CustCD
	, [EX-FTY Date] = CONVERT( varchar,iif( DATEPART(WEEKDAY, p.BuyerDelivery)=7 ,dateadd(dd,-1,p.BuyerDelivery),iif( DATEPART(WEEKDAY,  p.BuyerDelivery)=1,dateadd(dd,-2,p.BuyerDelivery),p.BuyerDelivery)),103)	
	, [Remark]=''''
	,fo.zz
	into #tmplast
	FROM   #tmpAll p 
	outer apply(
		select TOP 1 SewingLineID  from sewingschedule_detail
		where OrderID=p.Orderid and Article=p.Article
	) line
	outer apply(
		select TOP 1 SewingLineID  from sewingschedule
		where OrderID=p.Orderid
	) line2
	outer apply (
		select ColorName = Name
		from Color where id=p.ColorID and BrandID=p.BrandID
	) colordes
	outer apply (
		select NameEN from Country
		where id=p.Dest
	) country
	outer apply	(select zz = case when (orderid<>'''' and Style<>'''') then isnull(colordes.ColorName,'''')else ''zz''end)fo
	
	select 
	[SHELL A/ SIZE] 
	, Article
	, [Sewing Line] 
	, '+@cols+N'
	, [TOTAL] 
	, [SPNO]
	, [OrderNo]
	, [P.O.No] 
	, [CUST CD] 
	, [EX-FTY Date] 
	, [Remark]
	from #tmplast
	order by Article,zz,[SPNO]

select distinct line.SewingLineID
,p.ColorID
,colordes.ColorName
,p.Article
,Category= CASE WHEN o.Category=''S'' then ''S'' else ''Bulk'' end
,p.buyerdelivery
into #tmp2
from #tmpUniondata p
outer apply(
	select top 1 sewinglineid from (
		SELECT top 1 sewinglineid FROM sewingschedule_detail
		WHERE ORDERID=p.Orderid AND article=p.Article
		union all
		SELECT top 1 sewinglineid FROM sewingschedule
		WHERE ORDERID=p.Orderid
	) tt
) line	
outer apply (
	select ColorName = Name
	from Color where id=p.ColorID and BrandID=p.BrandID
) colordes	
outer apply (
	select category from Orders
	where id=p.Orderid
) o
where SewingLineID is not null--�h��Total ����

select * 
from #tmp2 t
outer apply (
	select SizeList = STUFF((
		select concat('','',SizeCode) 
		from (
			select distinct sizecode 
			from #tmpUniondata a
			inner join (select id,Category= CASE WHEN Category=''S'' then ''S'' else ''Bulk'' end from Orders) o 
			on a.Orderid=o.ID and t.Category =o.Category			
			where  a.Article=t.Article			
			and a.buyerdelivery=t.buyerdelivery
		) s 	
		outer apply(
			select seq from Order_SizeCode 
			where sizecode=s.SizeCode and id='''+@PoID+N'''
		) size
		order by size.Seq desc
		for xml path ('''') 
	),1,1,'''')	
)sizecode
outer apply(
	select [No] = stuff((
		select CONCAT(''/'',id)
		from(			
			select distinct CONVERT(varchar,SUBSTRING(o.id,9,LEN(o.id)))id 
			from #tmpUniondata a
			inner join (select id,Category= CASE WHEN Category=''S'' then ''S'' else ''Bulk'' end from Orders) o 
			on a.Orderid=o.ID and t.Category =o.Category	
			outer apply(
				select top 1 sewinglineid from (
					SELECT top 1 sewinglineid FROM sewingschedule_detail
					WHERE ORDERID=a.Orderid AND article=a.Article
					union all
					SELECT top 1 sewinglineid FROM sewingschedule
					WHERE ORDERID=a.Orderid
				) tt
			) line		
			where	a.Article=t.Article and line.SewingLineID=t.SewingLineID and a.buyerdelivery=t.buyerdelivery
		) oo
		for xml path('''')
	),1,1,'''')	
) sp
outer apply(
	select sum(Qty) qty
	from (
		select distinct a.*
		from #tmpUniondata a
		inner join (select id,Category= CASE WHEN Category=''S'' then ''S'' else ''Bulk'' end from Orders) o 
		on a.Orderid=o.ID and t.Category =o.Category	
		outer apply(
				select top 1 sewinglineid from (
					SELECT top 1 sewinglineid FROM sewingschedule_detail
					WHERE ORDERID=a.Orderid AND article=a.Article
					union all
					SELECT top 1 sewinglineid FROM sewingschedule
					WHERE ORDERID=a.Orderid
				) tt
		) line		
		where a.Article=t.Article 
		and line.SewingLineID=t.SewingLineID 
		and a.buyerdelivery=t.buyerdelivery
	)a	
) total


drop table #tmpAll, #tmpUniondata,#tmp2
	'
	EXEC sp_executesql @sql
END
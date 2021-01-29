
CREATE PROCEDURE [dbo].[Cutting_P01print_Eachcons_vs_OrderQtyDown_POCombo]
	@OrderID VARCHAR(13)
AS
BEGIN
	
	--抓取ID為POID
	select @OrderID=POID FROM dbo.Orders WITH (NOLOCK) where ID = @OrderID
	
	SELECT ORDERNO=RTRIM(POID) + d.spno, StyleID FROM dbo.Orders WITH (NOLOCK)
	OUTER APPLY(SELECT STUFF((SELECT '/'+SUBSTRING(ID,11,4) FROM Production.dbo.Orders WITH (NOLOCK) WHERE POID = @OrderID  order by ID FOR XML PATH('')),1,1,'') as spno
	) d WHERE POID = @OrderID GROUP BY POID,d.spno,StyleID
	
SELECT
	'#' = a.FabricPanelCode
   ,'COLOR' = oc.Article
   ,'Size' = currSizeCode
   ,CutQty = IsNull(c.CutQty, 0)
   ,OrderQty = IsNull(c.Orderqty, 0)
   ,Balance = IsNull(c.CutQty, 0) - IsNull(c.Orderqty, 0)
	from dbo.Order_EachCons a
	left join (select articleSeq = Min(oa.Seq), Order_Qty.Article, SizeCode, sum(Qty) as Qty 
			from Order_Qty 
			left join (
				--2018/01/15 [9785] modify by Anderson
				--Order_Article
				select @OrderID as ID, Article,min(Seq) as seq 
				from Order_Article 
				where id IN (SELECT ID FROM Orders WHERE POID = @OrderID and dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1) 
				group by Article
			) oa on /*Order_Qty.ID = oa.id and */ Order_Qty.Article = oa.Article
			where order_qty.id IN (SELECT ID FROM Orders WHERE POID = @OrderID and dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1) and Qty > 0 
			group by Order_Qty.Article,SizeCode
	) oq on 1=1		
	Left Join dbo.Order_SizeSpec os2
	On os2.ID = a.Id
		And os2.SizeItem = 'S00'
		And os2.SizeCode = oq.SizeCode
	outer apply (select currSizeCode = IsNull(os2.SizeSpec, oq.SizeCode)) ss
	left join dbo.Order_SizeCode os on a.Id = os.Id and ss.currSizeCode = os.SizeCode
	left join dbo.Order_ColorCombo oc on oc.Id = a.Id and oc.FabricPanelCode = a.FabricPanelCode and oc.Article = oq.Article
	left join dbo.Order_EachCons_Color b on a.Ukey = b.Order_EachConsUkey and a.Id = b.Id
	left join dbo.Order_EachCons_Color_Article c on b.Ukey = c.Order_EachCons_ColorUkey and a.id = c.Id and c.Article = oc.Article and c.SizeCode = currSizeCode
	--Edward 先用Ukey抓Order_EachCons_Article，若有資料表示ForArticle，接著比對Order_Qty的Article一致才顯示1、否則0，沒有存在Order_EachCons_Article則表全部，值為1
	outer apply (select IsArticleOK = iif(exists(select 1 from Order_EachCons_Article oea where oea.Order_EachConsUkey = a.Ukey),
					iif(exists(select 1 from Order_EachCons_Article oea where oea.Order_EachConsUkey = a.Ukey and oq.Article = oea.Article), 1, 0), 1) ) ifa
	WHERE a.ID = @OrderID
		AND a.CuttingPiece = 0
		AND ISNULL (c.CutQty, 0) > 0 --and c.CutQty is not null
		AND oc.Article IS NOT NULL
		AND IsArticleOK = 1
	group by a.FabricPanelCode,oc.Article,oq.articleSeq,os.Seq,currSizeCode,c.Orderqty,c.CutQty
	ORDER BY a.FabricPanelCode, oq.articleSeq, oc.Article, os.Seq--,c.SizeCode

	--select '#'=a.FabricPanelCode,[Article]=c.Article,'Size'=c.SizeCode,CutQty=max(c.CutQty),OrderQty=max(c.Orderqty),Balance=max(c.Variance)
	--from dbo.Order_EachCons a WITH (NOLOCK)
	--inner join dbo.Order_EachCons_Color b WITH (NOLOCK) on a.Ukey = b.Order_EachConsUkey and a.Id = b.Id
	--inner join dbo.Order_EachCons_Color_Article c WITH (NOLOCK) on b.Ukey = c.Order_EachCons_ColorUkey and a.id = c.Id
	--where a.Id = @OrderID AND a.CuttingPiece = 0
	--group by a.FabricPanelCode,c.Article,c.SizeCode
	--order by a.FabricPanelCode,c.Article,c.SizeCode
	
END
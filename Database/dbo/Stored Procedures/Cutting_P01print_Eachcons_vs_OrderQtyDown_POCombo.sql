
CREATE PROCEDURE [dbo].[Cutting_P01print_Eachcons_vs_OrderQtyDown_POCombo]
	@OrderID VARCHAR(13)
AS
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	
	--抓取ID為POID
	select @OrderID=POID FROM dbo.Orders where ID = @OrderID

	SELECT ORDERNO=RTRIM(POID) + d.spno, StyleID FROM dbo.Orders
	OUTER APPLY(SELECT STUFF((SELECT '/'+SUBSTRING(ID,11,4) FROM Production.dbo.Orders WHERE POID = @OrderID  order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno
	) d WHERE POID = @OrderID GROUP BY POID,d.spno,StyleID

	select '#'=a.LectraCode,'COLOR'=c.Article,'Size'=c.SizeCode,CutQty=max(c.CutQty),OrderQty=max(c.Orderqty),Balance=max(c.Variance)
	from dbo.Order_EachCons a
	inner join dbo.Order_EachCons_Color b on a.Ukey = b.Order_EachConsUkey and a.Id = b.Id
	inner join dbo.Order_EachCons_Color_Article c on b.Ukey = c.Order_EachCons_ColorUkey and a.id = c.Id
	where a.Id = @OrderID AND a.CuttingPiece = 0
	group by a.LectraCode,c.Article,c.SizeCode
	order by a.LectraCode,c.Article,c.SizeCode
	
END
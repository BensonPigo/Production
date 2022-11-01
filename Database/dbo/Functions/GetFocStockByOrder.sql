USE [Production]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetFocStockByOrder]
(
	@OrderID varchar(20)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @FocStockQty as int;	
	DECLARE @FocQty_InOrder as int;
	DECLARE @TotalShipQty as int;
	DECLARE @TotalDiffQty as int;

	
	DECLARE @Tmp1 TABLE (
	Article varchar(20),
	SizeCode varchar(20),
	Price decimal(12,4)
	)
		
	DECLARE @Tmp2 TABLE (
	Article varchar(20),
	SizeCode varchar(20),
	ShipQty int
	)

	DECLARE @Tmp3 TABLE (
	Article varchar(20),
	SizeCode varchar(20),
	DiffQty int
	)

	--如何計算FOC 庫存？
	--FOC 庫存數量 = [訂單 FOC 總數量] - [Size/Article 為 FOC 的總出貨數] + [Size/Article 為 FOC 的台北財務調整的數量]

	--1. 訂單 FOC 數量
	SELECT @FocQty_InOrder=FOCQty
	FROM Orders 
	WHERE ID = @OrderID

	--2. Size/Article 為 FOC 的定義，先整理出Size/Article 的價格，如果價格是 0 即是FOC，把這些為 0 的Size/Article記錄下來
	INSERT @Tmp1
	SELECT   [Article]=oq.Article
			,[SizeCode]=oq.SizeCode 
			,[Price]=ISNULL(ou1.POPrice, ISNULL(ou2.POPrice,-1))
	FROM Order_Qty oq
	LEFT JOIN Order_UnitPrice ou1 ON ou1.Id=oq.Id AND ou1.Article=oq.Article AND ou1.SizeCode=oq.SizeCode 
	LEFT JOIN Order_UnitPrice ou2 ON ou2.Id=oq.Id AND ou2.Article='----' AND ou2.SizeCode='----' 
	WHERE oq.ID = @OrderID
	ORDER BY  oq.Article,oq.SizeCode 


	--3. 再串回Packing，要找出價格為0的總出貨數，Status <> New代表已經出貨了，扣掉出貨剩下就是庫存
	INSERT @Tmp2
    select pd.Article,pd.SizeCode,[ShipQty]=SUM(pd.ShipQty)
    from PackingList p
    inner join PackingList_Detail pd on p.ID = pd.ID
    where p.PulloutStatus <> 'New'
    and p.PulloutID <> ''
    and pd.OrderID = @OrderID
    group by pd.Article,pd.SizeCode
    order by pd.Article,pd.SizeCode


	--4. 台北財務調整的數量
	INSERT @Tmp3
	SELECt iq.Article,iq.SizeCode,[DiffQty]= SUM(iq.DiffQty)
	FROm InvAdjust i
	INNER JOIN InvAdjust_Qty iq ON i.ID = iq.ID
	WHERE i.OrderID = @OrderID
	GROUP BY iq.Article,iq.SizeCode

	----P.S  3、4的部分都根據Article、SizeCode先分組加總再最後加總避免發散

	--P.S 如果以後要增加各Article/SizeCode的FOC出貨，從這找
	SELECT --t1.Article,t1.SizeCode
			 @TotalShipQty=SUM(ISNULL(t2.ShipQty,0))
			,@TotalDiffQty=SUM(ISNULL(t3.DiffQty,0))
	FROM @tmp1 t1
	LEFT JOIN @tmp2 t2 ON t1.Article=t2.Article AND t1.SizeCode=t2.SizeCode
	LEFT JOIN @tmp3 t3 ON t1.Article=t3.Article AND t1.SizeCode=t3.SizeCode
	WHERE t1.Price=0
	--GROUP BY t1.Article,t1.SizeCode

	--台北調整，調整的是出貨數量，因此不能直接用庫存數去減
	SET @FocStockQty = @FocQty_InOrder - ( ISNULL(@TotalShipQty,0) +  ISNULL(@TotalDiffQty,0) )

	-- Return the result of the function
	RETURN @FocStockQty

END
GO



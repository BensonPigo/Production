-- Batch submitted through debugger: SQLQuery7.sql|7|0|C:\Users\JIMMY~1.LIA\AppData\Local\Temp\~vsCB42.sql
-- =============================================
-- Author:		Mike
-- Create date: 2015/12/31
-- Description:	BOA AutoPick Prepare
-- =============================================
CREATE PROCEDURE [dbo].[usp_BoaByIssueBreakDown]
	@IssueID			varchar(13)				--	Issue ID
	, @POID				VarChar(13)				--採購母單
	, @OrderID			VarChar(13)				--訂單子單
	, @Order_BOAUkey	BigInt		= 0			--BOA Ukey
	, @TestType			Bit			= 0			--是否為虛擬庫存計算
	, @UserID			VarChar(10) = ''
	, @IssueType		VarChar(20) = 'Sewing'	-- MtlType.IssueType
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @count as int;
	create Table #tmpOrder_Qty
	(  
		ID Varchar(13)
		, FactoryID Varchar(8)
		, CustCDID Varchar(16)
		, ZipperInsert Varchar(5)
		, CustPONo VarChar(30)
		, BuyMonth VarChar(16)
		, CountryID VarChar(2)
		, StyleID Varchar(15)
		, Article VarChar(8)
		, SizeSeq VarChar(2)
		, SizeCode VarChar(8)
		, Qty Numeric(6,0)
	);

    Insert Into #tmpOrder_Qty
	Select	Orders.ID
		, Orders.FactoryID
		, Orders.CustCDID
		, CustCD.ZipperInsert
		, Orders.CustPONo
		, Orders.BuyMonth
		, Factory.CountryID
		, Orders.StyleID
		, Order_Article.Article
		, Order_SizeCode.Seq
		, Order_SizeCode.SizeCode
		, IsNull(Issue_Breakdown.Qty, 0) Qty
	From dbo.Orders WITH (NOLOCK)
	Left Join dbo.Order_SizeCode WITH (NOLOCK) On Order_SizeCode.ID = Orders.POID
	Left Join dbo.Order_Article WITH (NOLOCK) On Order_Article.ID = Orders.ID
	Left Join dbo.Issue_Breakdown WITH (NOLOCK) On Issue_Breakdown.OrderID = Orders.ID
													And Issue_Breakdown.SizeCode = Order_SizeCode.SizeCode
													And Issue_Breakdown.Article = Order_Article.Article
	Left Join dbo.CustCD WITH (NOLOCK) On	 CustCD.BrandID = Orders.BrandID
											And CustCD.ID = Orders.CustCDID
	Left Join dbo.Factory WITH (NOLOCK) On  Factory.ID = Orders.FactoryID
	Where	Orders.POID = @POID
			And Orders.Junk = 0
			AND Issue_Breakdown.ID = @IssueID
	--Order By ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
	--	, CountryID, StyleID, Article, Seq, SizeCode;

	select @count = count(1) from #tmpOrder_Qty;
	if @count = 0
	begin
		Insert Into #tmpOrder_Qty
		Select	Orders.ID
				, Orders.FactoryID
				, Orders.CustCDID
				, CustCD.ZipperInsert
				, Orders.CustPONo
				, Orders.BuyMonth
				, Factory.CountryID
				, Orders.StyleID
				, Order_Article.Article
				, Order_SizeCode.Seq
				, Order_SizeCode.SizeCode
				, IsNull(Order_Qty.Qty, 0) Qty
		From dbo.Orders WITH (NOLOCK)
		Left Join dbo.Order_SizeCode WITH (NOLOCK) On  Order_SizeCode.ID = Orders.POID
		Left Join dbo.Order_Article WITH (NOLOCK) On  Order_Article.ID = Orders.ID
		Left Join dbo.Order_Qty WITH (NOLOCK) On  Order_Qty.ID = Orders.ID
												  And Order_Qty.SizeCode = Order_SizeCode.SizeCode
												  And Order_Qty.Article = Order_Article.Article
		Left Join dbo.CustCD WITH (NOLOCK) On  CustCD.BrandID = Orders.BrandID 
											   And CustCD.ID = Orders.CustCDID
		Left Join dbo.Factory WITH (NOLOCK)	On  Factory.ID = Orders.FactoryID
		Where	Orders.POID = @POID
				And Orders.Junk = 0
				AND Order_Qty.ID = @OrderID
		 --Order By ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
			--	, CountryID, StyleID, Article, Seq, SizeCode;
		select @count = count(1) from #tmpOrder_Qty;
		if @count = 0
		begin
			Insert Into #tmpOrder_Qty
			Select	Orders.ID
					, Orders.FactoryID
					, Orders.CustCDID
					, CustCD.ZipperInsert
					, Orders.CustPONo
					, Orders.BuyMonth
					, Factory.CountryID
					, Orders.StyleID
					, Order_Article.Article
					, Order_SizeCode.Seq
					, Order_SizeCode.SizeCode
					, IsNull(Order_Qty.Qty, 0) Qty
			From dbo.Orders WITH (NOLOCK)
			Left Join dbo.Order_SizeCode WITH (NOLOCK) On  Order_SizeCode.ID = Orders.POID
			Left Join dbo.Order_Article WITH (NOLOCK) On  Order_Article.ID = Orders.ID
			Left Join dbo.Order_Qty WITH (NOLOCK) On  Order_Qty.ID = Orders.ID
													  And Order_Qty.SizeCode = Order_SizeCode.SizeCode
													  And Order_Qty.Article = Order_Article.Article
			Left Join dbo.CustCD WITH (NOLOCK) On  CustCD.BrandID = Orders.BrandID
												   And CustCD.ID = Orders.CustCDID
			Left Join dbo.Factory WITH (NOLOCK)	On  Factory.ID = Orders.FactoryID
			Where	Orders.POID = @POID
					And Orders.Junk = 0
			--Order By ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
			--	, CountryID, StyleID, Article, Seq, SizeCode;
		end
	end

	Create Table #Tmp_BoaExpend (	
		ExpendUkey BigInt Identity(1,1) Not Null
		, ID Varchar(13)
		, Order_BOAUkey BigInt
		, RefNo VarChar(20)
		, SCIRefNo VarChar(30)
		, Article VarChar(8)
		, ColorID VarChar(6)
		, SuppColor NVarChar(Max)
		, SizeCode VarChar(8)
		, SizeSpec VarChar(15)
		, SizeUnit VarChar(8)
		, Remark NVarChar(Max)
		, OrderQty Numeric(6,0)
		, UsageQty Numeric(9,2)
		, UsageUnit VarChar(8)
		, SysUsageQty  Numeric(9,2)
		, BomZipperInsert VarChar(5)
		, BomCustPONo VarChar(30)
		, OrderList VarChar(max)
		, Primary Key (ExpendUkey)
	);
	Create NonClustered Index Idx_ID on #Tmp_BoaExpend (ID, Order_BOAUkey, ColorID) -- table index

	Exec BoaExpend @POID, @Order_BOAUkey, @TestType, @UserID,0,1;
	Drop Table #tmpOrder_Qty;

	select	p.id as [poid]
			, p.seq1
			, p.seq2
			, p.SCIRefno
			, dbo.getMtlDesc(p.id, p.seq1, p.seq2,2,0) [description] 
			, p.ColorID
			, p.SizeSpec
			, p.Spec
			, p.Special
			, p.Remark 
	into #tmpPO_supp_detail
	from dbo.PO_Supp_Detail as p WITH (NOLOCK)
	inner join dbo.Fabric f WITH (NOLOCK) on f.SCIRefno = p.SCIRefno
	inner join dbo.MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID
	where p.id=@POID and p.FabricType = 'A' and m.IssueType=@IssueType

	;with cte2 as (
		select	m.*
				, [balanceqty] = m.InQty - m.OutQty + m.AdjustQty - m.ReturnQty
		from #tmpPO_supp_detail 
		inner join dbo.FtyInventory m WITH (NOLOCK) on  m.POID = #tmpPO_supp_detail.poid 
														and m.seq1 = #tmpPO_supp_detail.seq1 
														and m.seq2 = #tmpPO_supp_detail.SEQ2
														and m.StockType = 'B' and Roll = ''
		where lock = 0
	)
	select	0 as [Selected]
			, '' as id
			, b.*
			, isnull (sum (a.OrderQty), 0.00) qty
			, left (b.seq1 + '   ', 3) + b.seq2 as seq
			, cte2.balanceqty
			, cte2.Ukey as ftyinventoryukey
			, cte2.StockType
			, cte2.Roll
			, cte2.Dyelot
	from #tmpPO_supp_detail b
	left join cte2 WITH (NOLOCK) on  cte2.poid = b.poid 
									 and cte2.seq1 = b.seq1 
									and cte2.SEQ2 = b.SEQ2
	left join #Tmp_BoaExpend a on  b.SCIRefno = a.scirefno 
								   and b.poid = a.ID
								   and (b.SizeSpec = a.SizeSpec) 
								   and (b.ColorID = a.ColorID)
	group by b.poid, b.seq1, b.seq2, b.[description], b.ColorID, b.SizeSpec, b.SCIRefno
			, b.Spec, b.Special, b.Remark, cte2.balanceqty, cte2.Ukey, cte2.StockType
			, cte2.Roll, cte2.Dyelot
	order by b.scirefno, b.ColorID, b.SizeSpec, b.Special, b.poid, b.seq1, b.seq2;

	with cte as(
		select	b.poid
				, b.seq1
				, b.seq2
				, a.SizeCode
				, isnull (sum (a.OrderQty), 0.00) qty 
		from (#tmpPO_supp_detail b 
		left join #Tmp_BoaExpend a on  b.SCIRefno = a.scirefno 
										and b.poid = a.ID 
										and (b.SizeSpec = a.SizeSpec) 
										and (b.ColorID = a.ColorID)) 
		group by b.poid, b.seq1, b.seq2, a.SizeCode
	)	 
	select	z.*
			, isnull (cte.qty, 0) as qty
			, isnull (cte.qty, 0) as ori_qty 
	from (
		select	x.poid
				, x.seq1
				, x.seq2
				, order_sizecode.SizeCode
				, Order_SizeCode.Seq 
		from dbo.order_sizecode WITH (NOLOCK)
			 , (select	distinct poid
						, seq1
						, seq2 
				from cte WITH (NOLOCK)
			   ) as x
		where Order_SizeCode.id = @POID
	) z 
	left join cte WITH (NOLOCK) on  cte.SizeCode = z.SizeCode 
									and cte.poid = z.poid 
									and cte.seq1 = z.seq1 
									and cte.seq2 = z.seq2
	order by z.seq1,z.seq2,z.Seq
END
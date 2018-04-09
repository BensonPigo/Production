	              

CREATE Procedure [dbo].[BoaExpend]
(
	  @ID				VarChar(13)				--採購母單
	 ,@Order_BOAUkey	BigInt		= 0			--BOA Ukey
	 ,@TestType			Int			= 0			--是否為虛擬庫存計算(0: 實際寫入Table; 1: 僅傳出Temp Table; 2: 不回傳Temp Table; 3: 實際寫入Table，但不回傳Temp Table)
	 ,@UserID			VarChar(10) = ''
	 ,@IsGetFabQuot		Bit			= 1
	 ,@IsExpendDetail	Bit			= 0			--是否一律展開至最詳細
)
As
Begin
	Set NoCount On;
	If Object_ID('tempdb..#Tmp_BoaExpend') Is Null
	Begin
		Create Table #Tmp_BoaExpend
		(  
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
			, Index Idx_ID NonClustered (ID, Order_BOAUkey, ColorID) -- table index
		);
	End;	

	If Object_ID('tempdb..#Tmp_BoaExpend_OrderList') Is Null
	Begin
		Create Table #Tmp_BoaExpend_OrderList
		(
			ExpendUkey BigInt
			, ID Varchar(13)
			, OrderID Varchar(13)
			Index Idx_ID NonClustered (ExpendUkey, ID, OrderID) -- table index
		);
	End;
		
	If Object_ID('tempdb..#Tmp_Order_Qty') Is Null
	Begin
		Select	Order_Qty.* 
		Into #Tmp_Order_Qty
		From dbo.Order_Qty WITH (NOLOCK)
		Inner Join dbo.Orders WITH (NOLOCK)	On Orders.ID = Order_Qty.ID
		Where Orders.PoID = @ID;
	End;

	Declare @ExpendUkey BigInt;

	--取得訂單基本資料:Brand, Style, Season, Program, Category, Cfm. Date
	Declare @BrandID VarChar(8);
	Declare @StyleID VarChar(15);
	Declare @SeasonID VarChar(10);
	Declare @Category VarChar(1);
	Declare @ProgramID VarChar(12);
	Declare @CfmDate Date;

	Select	@BrandID = Orders.BrandID
			, @StyleID = Orders.StyleID
			, @SeasonID = Orders.SeasonID
			, @ProgramID = Orders.ProgramID
			, @Category = Orders.Category
			, @CfmDate = Orders.CfmDate
	From dbo.Orders WITH (NOLOCK)
	Where ID = @ID;

	--定義欄位
	Declare @SCIRefNo Varchar(30);
	Declare @RefNo Varchar(20);
	Declare @Price Numeric(8,4);
	Declare @OrderQty Numeric(6,0);
	Declare @UsageQty Numeric(9,2);
	Declare @UsageUnit Varchar(8);
	Declare @Article Varchar(8);
	Declare @ColorID Varchar(6);
	Declare @SuppColor NVarchar(70);
	Declare @SizeItem VarChar(3);
	Declare @SizeCode Varchar(8);
	Declare @Sizespec Varchar(15);
	Declare @SizeUnit Varchar(8);
	Declare @SysUsageQty Numeric(9,2)
	Declare @Remark NVarchar(Max);
	Declare @BomZipperInsert VarChar(5);
	Declare @BomCustPONo VarChar(30);
	Declare @OrderID Varchar(13);
	Declare @MtlTypeID VarChar(20);
	Declare @BomTypeCalculate Bit;
	Declare @NoSizeUnit Bit;
	Declare @SizeSpec_Cal Numeric(15,4);
	Declare @tmpBOACustCdIsExist Bit;

	--取得採購組合的訂單Q'ty breakdown
	Declare @tmpID Varchar(13);
	Declare @tmpOrderComboID Varchar(13);
	Declare @tmpPoID Varchar(13);
	Declare @tmpFactoryID Varchar(8);
	Declare @tmpCustCDID Varchar(16);
	Declare @tmpZipperInsert Varchar(5);
	Declare @tmpCustPONo VarChar(30);
	Declare @tmpBuyMonth VarChar(16);
	Declare @tmpCountryID VarChar(2);
	Declare @tmpStyleID Varchar(15);
	Declare @tmpArticle VarChar(8);
	Declare @tmpSizeSeq VarChar(2);
	Declare @tmpSizeCode VarChar(8);
	Declare @tmpSizeUnit Varchar(8);
	Declare @tmpSizeSpec Varchar(15);
	Declare @tmpQty Numeric(6,0);

	Declare @tmpOrder_Qty Table
	(  
		RowID BigInt Identity(1,1) Not Null
		, ID Varchar(13)
		, OrderComboID VarChar(13)
		, POID VarChar(13)
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

	Declare @Order_QtyRowID Int;		--tmpOrder_Qty ID
	Declare @Order_QtyRowCount Int;		--tmpOrder_Qty總資料筆數

	Insert Into @tmpOrder_Qty
	Select	Orders.ID
			, Orders.OrderComboID
			, Orders.POID
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
			, IsNull(Tmp_Order_Qty.Qty, 0) Qty
	From Orders WITH (NOLOCK)
	Left Join dbo.Order_SizeCode WITH (NOLOCK) On Order_SizeCode.ID = Orders.POID
	Left Join dbo.Order_Article WITH (NOLOCK) On Order_Article.ID = Orders.ID
	Left Join #Tmp_Order_Qty as Tmp_Order_Qty On Tmp_Order_Qty.ID = Orders.ID
												 and Tmp_Order_Qty.SizeCode = Order_SizeCode.SizeCode
												 and Tmp_Order_Qty.Article = Order_Article.Article
	Left Join dbo.CustCD WITH (NOLOCK) On CustCD.BrandID = Orders.BrandID
										  and CustCD.ID = Orders.CustCDID
	Left Join dbo.Factory WITH (NOLOCK)	On Factory.ID = Orders.FactoryID
	Where	Orders.POID = @ID
			and Orders.Junk = 0
	Order By ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
			 , CountryID, StyleID, Article, Seq, SizeCode;
	
	Declare @Sum_Qty Table
	(  
		RowID BigInt Identity(1,1) Not Null
		, OrderID VarChar(13)
		, ColorID VarChar(6)
		, Article VarChar(8)
		, BomZipperInsert Varchar(5)
		, BomCustPONo VarChar(30)
		, BomArticle VarChar(8)
		, SizeSeq VarChar(2)
		, SizeCode VarChar(8)
		, SizeSpec VarChar(15)
		, SizeUnit VarChar(8)
		, OrderQty Numeric(6,0)
		, UsageQty Numeric(9,2)
		, OrderList VarChar(Max)
	);

	Declare @tmpTbl Table
	( 
		RowID BigInt Identity(1,1) Not Null
		, ID VarChar(13)
		, ColorID VarChar(6)
		, Article VarChar(8)
		, BomZipperInsert Varchar(5)
		, BomCustPONo VarChar(30)
		, SizeSeq VarChar(2)
		, SizeCode VarChar(8)
		, SizeSpec VarChar(15)
		, SizeUnit VarChar(8)
		, OrderQty Numeric(6,0)
		, UsageQty Numeric(9,2)
	);

	Declare @Sum_QtyRowID Int;		--Sum_Qty ID
	Declare @Sum_QtyRowCount Int;	--Sum_Qty總資料筆數

	Declare @BoaUkey BigInt;
	Declare @BoaSuppID VarChar(6);
	Declare @BoaFabricPanelCode VarChar(2);
	Declare @BoaSizeItem VarChar(3);
	Declare @BoaSizeItem_Elastic VarChar(3);
	Declare @BoaConsPC Numeric(8,4);
	Declare @BoaIsForArticle Bit;
	Declare @BoaBomTypeFactory Bit;
	Declare @BoaBomTypeZipper Bit;
	Declare @BoaBomTypePo Bit;
	Declare @BoaBomTypeSize Bit;
	Declare @BoaBomTypeColor Bit;
	Declare @BoaIsCustCD numeric(1,0);
	
	Declare @BoaCursor Table
	(	
		RowID BigInt Identity(1,1) Not Null
		, Ukey BigInt
		, SCIRefNo VarChar(30)
		, SuppID VarChar(6)
		, FabricPanelCode VarChar(2)
		, SizeItem VarChar(3)
		, SizeItem_Elastic VarChar(3)
		, ConsPC Numeric(8,4)
		, Remark NVarChar(Max)
		, BomTypeZipper Bit
		, BomTypePo Bit
		, BomTypeSize Bit
		, BomTypeColor Bit
		, IsCustCD numeric(1,0)
	);

	Declare @BoaRowID Int;
	Declare @BoaRowCount Int;	--總資料筆數
	
	Insert Into @BoaCursor
	(  
		Ukey
		, SCIRefNo
		, SuppID
		, FabricPanelCode
		, SizeItem
		, SizeItem_Elastic
		, ConsPC
		, Remark
		, BomTypeZipper
		, BomTypePo
		, BomTypeSize
		, BomTypeColor
		, IsCustCD
	)
	Select	Ukey
			, SCIRefNo
			, SuppID
			, FabricPanelCode
			, SizeItem
			, SizeItem_Elastic
			, ConsPC
			, Remark
			, BomTypeZipper
			, BomTypePo
			, BomTypeSize
			, BomTypeColor
			, IsCustCD
	From dbo.Order_BOA WITH (NOLOCK)
	Where	ID = @ID 
			and (IsNull(@Order_BOAUkey, 0) = 0 Or Ukey = @Order_BOAUkey)
		    and SubString(Seq, 1, 1) != '7'
		    and RefNo != 'LABEL'
	Order by Ukey;

	--取得此單的所有SP
	declare @OrderList_Full varchar(max) = (select ID+',' from Orders where POID = @ID order by ID for xml path(''))

	Set @BoaRowID = 1;
	Select	@BoaRowID = Min(RowID)
			, @BoaRowCount = Max(RowID) 
	From @BoaCursor;

	While @BoaRowID <= @BoaRowCount
	Begin
		Select	@BoaUkey = Ukey
				, @SCIRefNo = SciRefNo
				, @BoaSuppID = SuppID
				, @BoaFabricPanelCode = FabricPanelCode
				, @BoaSizeItem = SizeItem
				, @BoaSizeItem_Elastic = SizeItem_Elastic
				, @BoaConsPC = ConsPC
				, @Remark = Remark
				, @BoaBomTypeZipper = BomTypeZipper
				, @BoaBomTypePo = BomTypePo
				, @BoaBomTypeSize = BomTypeSize
				, @BoaBomTypeColor = BomTypeColor
				, @BoaIsCustCD = IsCustCD
		From @BoaCursor
		Where RowID = @BoaRowID;
		
		--取得物料基本資料
		Select	@RefNo = Refno
				, @MtlTypeID = MtlTypeID
				, @UsageUnit = UsageUnit
				, @BomTypeCalculate = BomTypeCalculate
				, @NoSizeUnit = NoSizeUnit
		From dbo.Fabric WITH (NOLOCK)
		Where SCIRefNo = @SciRefNo;
		
		--取得SizeItem,當為Elastic，且SizeItem為S開頭時，改取SizeItem_Elastic
		Set @SizeItem = IIF(@BoaSizeItem_Elastic != '', @BoaSizeItem_Elastic, @BoaSizeItem);

		--取得SizeUnit
		Select @SizeUnit = IIF(@NoSizeUnit = 0, Order_SizeItem.SizeUnit, '')
		From dbo.Order_SizeItem WITH (NOLOCK)
		Where	Order_SizeItem.ID = @ID
				and Order_SizeItem.SizeItem = @SizeItem
		
		--判斷是否有 For Article
		Set @BoaIsForArticle = 0
		If Exists (Select 1 From dbo.Order_BOA_Article Where order_BOAUkey = @BoaUkey)
		Begin
			Set @BoaIsForArticle = 1;
		End;

		Delete From @Sum_Qty;
		Delete From @tmpTbl;

		insert into @tmpTbl
		Select	tmpQtyBreakDown.ID
				, IIF(@BoaBomTypeColor = 1 Or @IsExpendDetail = 1, IsNull(Order_ColorCombo.ColorID,''), '') as ColorID
				, IIF(@BoaBomTypeColor = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.Article, '') as Article
				, IIF(@BoaBomTypeZipper = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.ZipperInsert, '') as BomZipperInsert
				, IIF(@BoaBomTypePo = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.CustPONo, '') as BomCustPONo
				, IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.SizeSeq, '') as SizeSeq
				, IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.SizeCode, '') as SizeCode
				, IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, IsNull(tmpOrder_SizeSpec_OrderCombo.SizeSpec, tmpOrder_SizeSpec.SizeSpec), '') as SizeSpec
				, IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, @tmpSizeUnit, '') as SizeUnit
				, Qty as OrderQty
				, (Qty * IIF(IsNull(@SizeItem, '') = '', 1, IsNull(IsNull(tmpOrder_SizeSpec_OrderCombo.SizeSpec_Cal, tmpOrder_SizeSpec.SizeSpec_Cal), IIF(@BomTypeCalculate = 1, 0, 1))) * @BoaConsPC) as UsageQty
		From @tmpOrder_Qty as tmpQtyBreakDown
		Left Join dbo.Order_ColorCombo On	Order_ColorCombo.ID = @ID
										   And Order_ColorCombo.Article = tmpQtyBreakDown.Article
										   And Order_ColorCombo.FabricPanelCode = @BoaFabricPanelCode
		Outer Apply (	
			Select	ID
					, SizeItem
					, SizeCode
					, SizeSpec
					, IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' Or @UsageUnit = 'INCH', dbo.GetDigitalValue(SizeSpec), 0), 1) as SizeSpec_Cal
			From dbo.Order_SizeSpec
			Where	Order_SizeSpec.ID = tmpQtyBreakDown.POID
					And Order_SizeSpec.SizeItem = @SizeItem
					And Order_SizeSpec.SizeCode = tmpQtyBreakDown.SizeCode
		) as tmpOrder_SizeSpec
		Outer Apply (	
			Select	ID
					, SizeItem
					, SizeCode
					, SizeSpec
					, IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' Or @UsageUnit = 'INCH', dbo.GetDigitalValue(SizeSpec), 0), 1) as SizeSpec_Cal
			From dbo.Order_SizeSpec_OrderCombo
			Where	Order_SizeSpec_OrderCombo.ID = tmpQtyBreakDown.POID
					And Order_SizeSpec_OrderCombo.OrderComboID = tmpQtyBreakDown.OrderComboID
					And Order_SizeSpec_OrderCombo.SizeItem = @SizeItem
					And Order_SizeSpec_OrderCombo.SizeCode = tmpQtyBreakDown.SizeCode
		) as tmpOrder_SizeSpec_OrderCombo
		Outer Apply (
			Select Top 1 SCIRefno 
			From dbo.Order_BOA_CustCD 
			Where	Order_BOA_CustCD.Order_BOAUkey = @BoaUkey 
					and CustCDID = tmpQtyBreakDown.CustCDID
		) as tmpReplaceSciRefNo
		Where	--排除For Article
				( @BoaIsForArticle = 0 
				  or (@BoaIsForArticle = 1 And Exists (Select 1 From dbo.Order_BOA_Article Where order_BOAUkey = @BoaUkey And Article = tmpQtyBreakDown.Article))
				)
				--符合CustCD設定，包含設定為1 = 全部
				And ((@BoaIsCustCD = 1)
					 Or	((@BoaIsCustCD = 2) And (Exists (Select Top 1 1 From dbo.Order_BOA_CustCD Where Order_BOA_CustCD.Order_BOAUkey = @BoaUkey And Order_BOA_CustCD.CustCDID = tmpQtyBreakDown.CustCDID)))
					 Or	((@BoaIsCustCD = 3) And (Not Exists (Select Top 1 1 From dbo.Order_BOA_CustCD Where Order_BOA_CustCD.Order_BOAUkey = @BoaUkey And Order_BOA_CustCD.CustCDID = tmpQtyBreakDown.CustCDID)))
					 Or (@BoaIsCustCD = 4)
				)

		Insert Into @Sum_Qty
		(  
			ColorID
			, Article
			, BomZipperInsert
			, BomCustPONo
			, SizeSeq
			, SizeCode
			, SizeSpec
			, SizeUnit
			, OrderQty
			, UsageQty
			, Orderlist
		)
		Select	ColorID
				, Article
				, BomZipperInsert
				, BomCustPONo
				, SizeSeq
				, SizeCode
				, SizeSpec
				, SizeUnit
				, Sum(OrderQty) as OrderQty
				, Sum(UsageQty) as UsageQty
				, Orderlist
		From @tmpTbl tmp1
		Cross Apply (
			Select Orderlist = (Select distinct ID + ',' 
								From @tmpTbl as tmp3 
								where	tmp3.ColorID = tmp1.ColorID 
										and tmp3.Article = tmp1.Article 
										and tmp3.BomZipperInsert = tmp1.BomZipperInsert 
										and tmp3.BomCustPONo = tmp1.BomCustPONo 
										and tmp3.SizeSeq = tmp1.SizeSeq 
										and tmp3.SizeCode = tmp1.SizeCode 
										and tmp3.SizeSpec = tmp1.SizeSpec 
										and tmp3.SizeUnit = tmp1.SizeUnit
								for xml path(''))
		) as tmp2
		Where OrderQty > 0
		Group by ColorID, Article, BomZipperInsert, BomCustPONo
					, SizeSeq, SizeCode, SizeSpec, SizeUnit, Orderlist
		Order by BomZipperInsert, BomCustPONo
					, Article, ColorID, SizeSeq, SizeCode, SizeSpec;

		Insert Into #Tmp_BoaExpend (  
			ID
			, Order_BOAUkey
			, RefNo
			, SCIRefNo
			, Article
			, ColorID
			, SuppColor
			, SizeCode
			, SizeSpec
			, SizeUnit
			, Remark
			, OrderQty
			, UsageQty
			, UsageUnit
			, SysUsageQty
			, BomZipperInsert
			, BomCustPONo
			, OrderList
		)
		Select	@ID
				, @BoaUkey
				, @RefNo
				, @SciRefNo
				, Sum_Qty.Article
				, Sum_Qty.ColorID
				, tmpSuppColor.SuppColor
				, Sum_Qty.SizeCode
				, Sum_Qty.SizeSpec
				, Sum_Qty.SizeUnit
				, @Remark
				, Sum_Qty.OrderQty
				, Sum_Qty.UsageQty
				, @UsageUnit
				, Sum_Qty.UsageQty
				, Sum_Qty.BomZipperInsert
				, Sum_Qty.BomCustPONo
				, Sum_Qty.OrderList
		From @Sum_Qty as Sum_Qty
		Outer Apply (
			Select SuppColor = IsNull(dbo.GetSuppColorList(@SciRefNo, @BoaSuppID, Sum_Qty.ColorID, @BrandID, @SeasonID, @ProgramID, @StyleID), '')
		) as tmpSuppColor

		Set @BoaRowID += 1
	End;

	--Update OrderList 包含整組的sp，改為空白
	update #Tmp_BoaExpend
		set OrderList = ''
	where OrderList = @OrderList_Full

	If (@TestType <> 2) And (@TestType <> 3)
	Begin
		Select * From #Tmp_BoaExpend;
		Select * From #Tmp_BoaExpend_OrderList;
	End;
	
	--若@TestType = 0，表示需實際寫入Table
	If @TestType = 0 Or @TestType = 3
	Begin
		Exec BoaExpend_Insert @ID, @Order_BOAUkey, @UserID;
	End;
	
	--Drop Table #Tmp_BoaExpend;
	Drop Table #Tmp_BoaExpend_OrderList;
End
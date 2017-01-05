	              

Create Procedure [dbo].[BoaExpend]
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
			(  ExpendUkey BigInt Identity(1,1) Not Null, ID Varchar(13), Order_BOAUkey BigInt
			 , RefNo VarChar(20), SCIRefNo VarChar(26), Article VarChar(8), ColorID VarChar(6), SuppColor NVarChar(Max)
			 , SizeCode VarChar(8), SizeSpec VarChar(15), SizeUnit VarChar(8), Remark NVarChar(Max)
			 , OrderQty Numeric(6,0), Price Numeric(8,4), UsageQty Numeric(9,2), UsageUnit VarChar(8), SysUsageQty  Numeric(9,2)
			 , BomZipperInsert VarChar(5), BomCustPONo VarChar(30), Keyword VarChar(Max)
			 , Primary Key (ExpendUkey)
			 , Index Idx_ID NonClustered (ID, Order_BOAUkey, ColorID) -- table index
			);
	End;	
	If Object_ID('tempdb..#Tmp_BoaExpend_OrderList') Is Null
	Begin
		Create Table #Tmp_BoaExpend_OrderList
			(ExpendUkey BigInt, ID Varchar(13), OrderID Varchar(13)
			 Index Idx_ID NonClustered (ExpendUkey, ID, OrderID) -- table index
			);
	End;	
	If Object_ID('tempdb..#Tmp_Order_Qty') Is Null
	Begin
		Select Order_Qty.* Into #Tmp_Order_Qty
		  From dbo.Order_Qty
		 Inner Join dbo.Orders
			On Orders.ID = Order_Qty.ID
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

	Select @BrandID = Orders.BrandID
		 , @StyleID = Orders.StyleID
		 , @SeasonID = Orders.SeasonID
		 , @ProgramID = Orders.ProgramID
		 , @Category = Orders.Category
		 , @CfmDate = Orders.CfmDate
	  From dbo.Orders
	 Where ID = @ID;

	--定義欄位
	Declare @SCIRefNo Varchar(26);
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
	--Declare @BomFactory VarChar(8);
	--Declare @BomCustCD VarChar(16);
	Declare @BomZipperInsert VarChar(5);
	Declare @BomCustPONo VarChar(30);
	--Declare @BomBuymonth VarChar(16);
	--Declare @BomCountry VarChar(2);
	--Declare @BomStyle VarChar(15);
	--Declare @BomArticle VarChar(8);
	Declare @Keyword VarChar(Max);
	Declare @Keyword_Trans VarChar(Max);

	Declare @OrderID Varchar(13);

	Declare @MtlTypeID VarChar(20);
	Declare @BomTypeCalculate Bit;
	Declare @NoSizeUnit Bit;
	Declare @SizeSpec_Cal Numeric(15,4);

	--取得採購組合的訂單Q'ty breakdown
	Declare @tmpOrder_Qty Table
		(  ID Varchar(13), FactoryID Varchar(8), CustCDID Varchar(16), ZipperInsert Varchar(5)
		 , CustPONo VarChar(30), BuyMonth VarChar(16), CountryID VarChar(2), StyleID Varchar(15)
		 , Article VarChar(8), SizeSeq VarChar(2), SizeCode VarChar(8), Qty Numeric(6,0)
		);
	Insert Into @tmpOrder_Qty
		Select Orders.ID, Orders.FactoryID, Orders.CustCDID, CustCD.ZipperInsert
			 , Orders.CustPONo, Orders.BuyMonth, Factory.CountryID, Orders.StyleID
			 , Order_Article.Article, Order_SizeCode.Seq, Order_SizeCode.SizeCode
			 , IsNull(Tmp_Order_Qty.Qty, 0) Qty
		  From dbo.Orders
		  Left Join dbo.Order_SizeCode
			On Order_SizeCode.ID = Orders.POID
		  Left Join dbo.Order_Article
			On Order_Article.ID = Orders.ID
		  Left Join #Tmp_Order_Qty as Tmp_Order_Qty
			On	   Tmp_Order_Qty.ID = Orders.ID
			   And Tmp_Order_Qty.SizeCode = Order_SizeCode.SizeCode
			   And Tmp_Order_Qty.Article = Order_Article.Article
		  Left Join dbo.CustCD
			On	   CustCD.BrandID = Orders.BrandID
			   And CustCD.ID = Orders.CustCDID
		  Left Join dbo.Factory
			On Factory.ID = Orders.FactoryID
		 Where Orders.POID = @ID
		   And Orders.Junk = 0
		 Order By ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
				, CountryID, StyleID, Article, Seq, SizeCode;
	
	Declare @Sum_Qty Table
		(  RowID BigInt Identity(1,1) Not Null, OrderID VarChar(13), ColorID VarChar(6), Article VarChar(8)
		 , BomZipperInsert Varchar(5), BomCustPONo VarChar(30)
		 , SizeSeq VarChar(2), SizeCode VarChar(8), SizeSpec VarChar(8), SizeUnit VarChar(8)
		 , OrderQty Numeric(6,0), UsageQty Numeric(9,2)
		);
	Declare @Sum_QtyRowID Int;		--Sum_Qty ID
	Declare @Sum_QtyRowCount Int;	--Sum_Qty總資料筆數

	Declare @BoaUkey BigInt;
	Declare @BoaSuppID VarChar(6);
	Declare @BoaPatternPanel VarChar(2);
	Declare @BoaSizeItem VarChar(3);
	Declare @BoaSizeItem_Elastic VarChar(3);
	Declare @BoaConsPC Numeric(8,4);
	Declare @BoaBomTypeFactory Bit;
	Declare @BoaBomTypeZipper Bit;
	Declare @BoaBomTypePo Bit;
	Declare @BoaBomTypeSize Bit;
	Declare @BoaBomTypeColor Bit;
	Declare @BoaCursor Table
		(  RowID BigInt Identity(1,1) Not Null, Ukey BigInt, SCIRefNo VarChar(26), SuppID VarChar(6)
		 , PatternPanel VarChar(2), SizeItem VarChar(3), SizeItem_Elastic VarChar(3), ConsPC Numeric(8,4), Remark NVarChar(Max)
		 , BomTypeZipper Bit, BomTypePo Bit
		 , BomTypeSize Bit, BomTypeColor Bit
		 --, BomTypeArticle Bit
		 , Keyword VarChar(Max)
		);
	Declare @BoaRowID Int;
	Declare @BoaRowCount Int;	--總資料筆數
	
	Insert Into @BoaCursor
		(  Ukey, SCIRefNo, SuppID, PatternPanel, SizeItem, SizeItem_Elastic, ConsPC, Remark
		 , BomTypeZipper, BomTypePo, BomTypeSize, BomTypeColor, Keyword
		)
		Select Ukey, SCIRefNo, SuppID, PatternPanel, SizeItem, SizeItem_Elastic, ConsPC, Remark
			 , BomTypeZipper, BomTypePo, BomTypeSize, BomTypeColor, Keyword
		  From dbo.Order_BOA
		 Where ID = @ID
		   And (   IsNull(@Order_BOAUkey, 0) = 0
				Or Ukey = @Order_BOAUkey
			   )
		   And SubString(Seq, 1, 1) != '7'
		   And RefNo != 'LABEL'
		 Order by Ukey;

	Set @BoaRowID = 1;
	Select @BoaRowID = Min(RowID), @BoaRowCount = Max(RowID) From @BoaCursor;
	While @BoaRowID <= @BoaRowCount
	Begin
		Select @BoaUkey = Ukey
			 , @SCIRefNo = SciRefNo
			 , @BoaSuppID = SuppID
			 , @BoaPatternPanel = PatternPanel
			 , @BoaSizeItem = SizeItem
			 , @BoaSizeItem_Elastic = SizeItem_Elastic
			 , @BoaConsPC = ConsPC
			 , @Remark = Remark
			 , @BoaBomTypeZipper = BomTypeZipper
			 , @BoaBomTypePo = BomTypePo
			 , @BoaBomTypeSize = BomTypeSize
			 , @BoaBomTypeColor = BomTypeColor
			 , @Keyword = IsNull(Keyword, '')
		  From @BoaCursor
		 Where RowID = @BoaRowID;
		
		--取得物料基本資料
		Select @RefNo = Refno
			 , @MtlTypeID = MtlTypeID
			 , @UsageUnit = UsageUnit
			 , @BomTypeCalculate = BomTypeCalculate
			 , @NoSizeUnit = NoSizeUnit
		  From dbo.Fabric
		 Where SCIRefNo = @SciRefNo;
		
		--取得SizeItem,當為Elastic，且SizeItem為S開頭時，改取SizeItem_Elastic
		Set @SizeItem = IIF(@MtlTypeID = 'ELASTIC' And SubString(@BoaSizeItem,1,1) = 'S', @BoaSizeItem_Elastic, @BoaSizeItem);
		
		--取得SizeUnit
		Select @SizeUnit = IIF(@NoSizeUnit = 0, Order_SizeItem.SizeUnit, '')
		  From dbo.Order_SizeItem
		 Where Order_SizeItem.ID = @ID
		   And Order_SizeItem.SizeItem = @SizeItem
		
		Delete From @Sum_Qty;

		Insert Into @Sum_Qty
			(  ColorID, Article, BomZipperInsert, BomCustPONo
			 , SizeSeq, SizeCode, SizeSpec, SizeUnit, OrderQty, UsageQty
			)
			Select ColorID, Article, BomZipperInsert, BomCustPONo
				 , SizeSeq, SizeCode, SizeSpec, SizeUnit
				 , Sum(OrderQty) as OrderQty, Sum(UsageQty) as UsageQty
			  From (Select tmpQtyBreakDown.ID
						 , IIF(@BoaBomTypeColor = 1 Or @IsExpendDetail = 1, IsNull(Order_ColorCombo.ColorID,''), '') as ColorID
						 , IIF(@BoaBomTypeColor = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.Article, '') as Article
						 --, IIF(@BoaBomTypeFactory = 1, tmpQtyBreakDown.FactoryID, '') as BomFactory
						 --, IIF(@BoaBomTypeCustCD = 1, tmpQtyBreakDown.CustCDID, '') as BomCustCD
						 , IIF(@BoaBomTypeZipper = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.ZipperInsert, '') as BomZipperInsert
						 , IIF(@BoaBomTypePo = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.CustPONo, '') as BomCustPONo
						 --, IIF(@BoaBomTypeBuyMonth = 1, tmpQtyBreakDown.BuyMonth, '') as BomBuymonth
						 --, IIF(@BoaBomTypeCountry = 1, tmpQtyBreakDown.CountryID, '') as BomCountry
						 --, IIF(@BoaBomTypeStyle = 1, tmpQtyBreakDown.StyleID, '') as BomStyle
						 --, IIF(@BoaBomTypeArticle = 1, tmpQtyBreakDown.Article, '') as BomArticle
						 , IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.SizeSeq, '') as SizeSeq
						 , IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.SizeCode, '') as SizeCode
						 --, tmpQtyBreakDown.SizeSeq as SizeSeq
						 --, tmpQtyBreakDown.SizeCode as SizeCode
						 , IIF(@BoaBomTypeSize = 1, tmpOrder_SizeSpec.SizeSpec, '') as SizeSpec
						 , IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, @SizeUnit, '') as SizeUnit
						 --, IIF(@BoaBomTypeSize = 1 And IsNull(tmpOrder_SizeSpec.SizeSpec, '') = '', 0, Qty) as OrderQty
						 , Qty as OrderQty
						 , (Qty * IIF(IsNull(@SizeItem, '') = '', 1, IsNull(tmpOrder_SizeSpec.SizeSpec_Cal, IIF(@BomTypeCalculate = 1, 0, 1))) * @BoaConsPC) as UsageQty
					  From @tmpOrder_Qty as tmpQtyBreakDown
					  Left Join dbo.Order_ColorCombo
						On	   Order_ColorCombo.ID = @ID
						   And Order_ColorCombo.Article = tmpQtyBreakDown.Article
						   --And Order_ColorCombo.LectraCode = @BoaPatternPanel
						   And Order_ColorCombo.PatternPanel = @BoaPatternPanel
					  Left Join (Select ID, SizeItem, SizeCode, SizeSpec
									  , IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' Or @UsageUnit = 'INCH', dbo.GetDigitalValue(SizeSpec), 0), 1) as SizeSpec_Cal
								   From dbo.Order_SizeSpec
								) tmpOrder_SizeSpec
					    On	   tmpOrder_SizeSpec.ID = @ID
						   And tmpOrder_SizeSpec.SizeItem = @SizeItem
						   And tmpOrder_SizeSpec.SizeCode = tmpQtyBreakDown.SizeCode
					  Left Join (Select Order_BOA_CustCD.*
									  , Convert(Bit, 1) as IsExist
								   From dbo.Order_BOA_CustCD
								) as tmpBOA_CustCD
						On tmpBOA_CustCD.Order_BOAUkey = @BoaUkey
					 Where (IsNull(tmpBOA_CustCD.IsExist, 0) = 0)
						Or (	tmpBOA_CustCD.IsExist = 1
							And tmpBOA_CustCD.CustCDID = tmpQtyBreakDown.CustCDID
						   )
				   ) as tmpQtyBreakDown
			 Where OrderQty > 0
			 Group by ColorID, Article, BomZipperInsert, BomCustPONo
					, SizeSeq, SizeCode, SizeSpec, SizeUnit
			 Order by BomZipperInsert, BomCustPONo
					, Article, ColorID, SizeSeq, SizeCode, SizeSpec;
		
		Set @Sum_QtyRowID = 1;
		--Select @Sum_QtyRowCount = Count(*) From @Sum_Qty;
		Select @Sum_QtyRowID = Min(RowID), @Sum_QtyRowCount = Max(RowID) From @Sum_Qty;
		While @Sum_QtyRowID <= @Sum_QtyRowCount
		Begin
			Select @OrderID = OrderID
				 , @ColorID = ColorID
				 , @Article = Article
				 --, @BomFactory = BomFactory
				 --, @BomCustCD = BomCustCD
				 , @BomZipperInsert = BomZipperInsert
				 , @BomCustPONo = BomCustPONo
				 --, @BomBuymonth = BomBuymonth
				 --, @BomCountry = BomCountry
				 --, @BomStyle = BomStyle
				 --, @BomArticle = BomArticle
				 , @SizeCode = SizeCode
				 , @SizeSpec = SizeSpec
				 , @SizeUnit = SizeUnit
				 , @OrderQty = OrderQty
				 , @UsageQty = UsageQty
			  From @Sum_Qty
			 Where RowID = @Sum_QtyRowID;
			
			--取得Keyword
			Set @Keyword_Trans = ''
			If @Keyword != ''
			Begin
				Exec dbo.GetKeyword @ID, @BoaUkey, @Keyword, @Keyword_Trans Output, @Article, @SizeCode;
			End;
			
			Set @SysUsageQty = @UsageQty;
			--取得 Supplier Color
			Set @SuppColor = IsNull(dbo.GetSuppColorList(@SciRefNo, @BoaSuppID, @ColorID, @BrandID, @SeasonID, @ProgramID, @StyleID), '');
			
			--取得 Fabric Price
			If @IsGetFabQuot = 1
			Begin
				Set @Price = IsNull(dbo.GetPriceFromMtl(@SciRefNo, @BoaSuppID, @SeasonID, @UsageQty, @Category, @CfmDate, '', @ColorID), 0);
			End;
			Else
			Begin
				Set @Price = 0;
			End;
			
			Insert Into #Tmp_BoaExpend
				(  ID, Order_BOAUkey, RefNo, SCIRefNo, Article, ColorID, SuppColor
				 , SizeCode, SizeSpec, SizeUnit, Remark, OrderQty, Price, UsageQty
				 , UsageUnit, SysUsageQty, BomZipperInsert, BomCustPONo, Keyword
				)
			Values
				(  @ID, @BoaUkey, @RefNo, @SCIRefNo, @Article, @ColorID, @SuppColor
				 , @SizeCode, @SizeSpec, @SizeUnit, @Remark, @OrderQty, @Price, @UsageQty
				 , @UsageUnit, @SysUsageQty, @BomZipperInsert, @BomCustPONo, @Keyword_Trans
				);
			
			Set @ExpendUkey = Scope_Identity();

			Insert Into #Tmp_BoaExpend_OrderList
				(ID, ExpendUkey, OrderID)
				Select @ID, @ExpendUkey, tmpQtyBreakDown.ID
				  From @tmpOrder_Qty as tmpQtyBreakDown
				  Left Join dbo.Order_ColorCombo
					On	   Order_ColorCombo.ID = @ID
					   And Order_ColorCombo.Article = tmpQtyBreakDown.Article
					   And Order_ColorCombo.LectraCode = @BoaPatternPanel
				  Left Join dbo.Order_SizeSpec
				    On	   Order_SizeSpec.ID = @ID
					   And Order_SizeSpec.SizeItem = @SizeItem
					   And Order_SizeSpec.SizeCode = tmpQtyBreakDown.SizeCode
				  Left Join (Select Order_BOA_CustCD.*
								  , Convert(Bit, 1) as IsExist
							   From dbo.Order_BOA_CustCD
							) as tmpBOA_CustCD
					On tmpBOA_CustCD.Order_BOAUkey = @BoaUkey
				 Where IIF(@BoaBomTypeColor = 1 Or @IsExpendDetail = 1, Order_ColorCombo.ColorID, '') = @ColorID
				   And IIF(@BoaBomTypeColor = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.Article, '') = @Article
				   --And IIF(@BoaBomTypeFactory = 1, tmpQtyBreakDown.FactoryID, '') = @BomFactory
				   --And IIF(@BoaBomTypeCustCD = 1, tmpQtyBreakDown.CustCDID, '') = @BomCustCD
				   And IIF(@BoaBomTypeZipper = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.ZipperInsert, '') = @BomZipperInsert
				   And IIF(@BoaBomTypePo = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.CustPONo, '') = @BomCustPONo
				   --And IIF(@BoaBomTypeBuyMonth = 1, tmpQtyBreakDown.BuyMonth, '') = @BomBuymonth
				   --And IIF(@BoaBomTypeCountry = 1, tmpQtyBreakDown.CountryID, '') = @BomCountry
				   --And IIF(@BoaBomTypeStyle = 1, tmpQtyBreakDown.StyleID, '') = @BomStyle
				   --And IIF(@BoaBomTypeArticle = 1, tmpQtyBreakDown.Article, '') = @BomArticle
				   And IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.SizeCode, '') = @SizeCode
				   --And tmpQtyBreakDown.SizeCode = @SizeCode
				   --And IIF(@BoaBomTypeSize = 1 And IsNull(Order_SizeSpec.SizeSpec, '') = '', 0, Qty) > 0
				   And Qty > 0
				   And (   (IsNull(tmpBOA_CustCD.IsExist, 0) = 0)
						Or (	IsNull(tmpBOA_CustCD.IsExist, 0) = 1
							And tmpBOA_CustCD.CustCDID = tmpQtyBreakDown.CustCDID
						   )
					   )
				 Group by tmpQtyBreakDown.ID;
			
			Set @Sum_QtyRowID += 1;
		End;

		Set @BoaRowID += 1
	End;

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
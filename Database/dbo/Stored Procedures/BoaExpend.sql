
CREATE Procedure [dbo].[BoaExpend]
(
	  @ID				VarChar(13)				--採購母單
	 ,@Order_BOAUkey	BigInt		= 0			--BOA Ukey
	 ,@TestType			Bit			= 0			--是否為虛擬庫存計算
	 ,@UserID			VarChar(10) = ''
)
As
Begin
	Set NoCount On;
	/*
	If @TestType = 0
	Begin
		--
	End;
	*/
	IF OBJECT_ID('tempdb..#Tmp_BoaExpend') IS NULL
    begin
            
		Create Table #Tmp_BoaExpend
			(  ExpendUkey BigInt Identity(1,1) Not Null, ID Varchar(13), Order_BOAUkey BigInt
			 , RefNo VarChar(20), SCIRefNo VarChar(26), Article VarChar(8), ColorID VarChar(6), SuppColor NVarChar(Max)
			 , SizeCode VarChar(8), SizeSpec VarChar(15), SizeUnit VarChar(8), Remark NVarChar(Max)
			 , OrderQty Numeric(6,0), Price Numeric(8,4), UsageQty Numeric(9,2), UsageUnit VarChar(8), SysUsageQty  Numeric(9,2)
			 , BomFactory VarChar(8), BomCountry VarChar(2), BomStyle VarChar(15), BomCustCD VarChar(20)
			 , BomArticle VarChar(8), BomZipperInsert VarChar(5), BomBuymonth VarChar(10), BomCustPONo VarChar(30)
			 , Primary Key (ExpendUkey)
			 , Index Idx_ID NonClustered (ID, Order_BOAUkey, ColorID) -- table index
			);

    end
	Create Table #Tmp_BoaExpend_OrderList
		(ExpendUkey BigInt, ID Varchar(13), OrderID Varchar(13)
		 Index Idx_ID NonClustered (ExpendUkey, ID, OrderID) -- table index
		);

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
	Declare @BomFactory VarChar(8);
	Declare @BomCustCD VarChar(16);
	Declare @BomZipperInsert VarChar(5);
	Declare @BomCustPONo VarChar(30);
	Declare @BomBuymonth VarChar(16);
	Declare @BomCountry VarChar(2);
	Declare @BomStyle VarChar(15);
	Declare @BomArticle VarChar(8);

	Declare @OrderID Varchar(13);

	Declare @MtlTypeID VarChar(20);
	Declare @BomTypeCalculate Bit;
	Declare @NoSizeUnit Bit;
	Declare @SizeSpec_Cal Numeric(15,4);

	--取得採購組合的訂單Q'ty breakdown
	if (@TestType = 0)
	begin 
	create Table #tmpOrder_Qty
		(  ID Varchar(13), FactoryID Varchar(8), CustCDID Varchar(16), ZipperInsert Varchar(5)
		 , CustPONo VarChar(30), BuyMonth VarChar(16), CountryID VarChar(2), StyleID Varchar(15)
		 , Article VarChar(8), SizeSeq VarChar(2), SizeCode VarChar(8), Qty Numeric(6,0)
		);
	Insert Into #tmpOrder_Qty
		Select Orders.ID AS ID, Orders.FactoryID, Orders.CustCDID, CustCD.ZipperInsert
			 , Orders.CustPONo, Orders.BuyMonth, Factory.CountryID AS CountryID, Orders.StyleID
			 , Order_Article.Article Article, Order_SizeCode.Seq Seq, Order_SizeCode.SizeCode SizeCode
			 , IsNull(Order_Qty.Qty, 0) Qty
		  From dbo.Orders
		  Left Join dbo.Order_SizeCode
			On Order_SizeCode.ID = Orders.POID
		  Left Join dbo.Order_Article
			On Order_Article.ID = Orders.ID
		  Left Join dbo.Order_Qty
			On	   Order_Qty.ID = Orders.ID
			   And Order_Qty.SizeCode = Order_SizeCode.SizeCode
			   And Order_Qty.Article = Order_Article.Article
		  Left Join dbo.CustCD
			On	   CustCD.BrandID = Orders.BrandID
			   And CustCD.ID = Orders.CustCDID
		  Left Join dbo.Factory
			On Factory.ID = Orders.FactoryID
		 Where Orders.POID = @ID
		   And Orders.Junk = 0
		 Order By ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
				, CountryID, StyleID, Article, Seq, SizeCode;
	end;

	Declare @Sum_Qty Table
		(  RowID BigInt Identity(1,1) Not Null, OrderID VarChar(13), ColorID VarChar(6), Article VarChar(8)
		 , BomFactory Varchar(8), BomCustCD Varchar(16), BomZipperInsert Varchar(5), BomCustPONo VarChar(30)
		 , BomBuymonth VarChar(16), BomCountry VarChar(2), BomStyle Varchar(15), BomArticle VarChar(8)
		 , SizeSeq VarChar(2), SizeCode VarChar(8), SizeSpec VarChar(8), SizeUnit VarChar(8), OrderQty Numeric(6,0), UsageQty Numeric(9,2)
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
	Declare @BoaBomTypeCustCD Bit;
	Declare @BoaBomTypeZipper Bit;
	Declare @BoaBomTypePo Bit;
	Declare @BoaBomTypeBuyMonth Bit;
	Declare @BoaBomTypeCountry Bit;
	Declare @BoaBomTypeStyle Bit;
	Declare @BoaBomTypeSize Bit;
	Declare @BoaBomTypeArticle Bit;
	Declare @BoaBomTypeColor Bit;
	Declare @BoaCursor Table
		(  RowID BigInt Identity(1,1) Not Null, Ukey BigInt, SCIRefNo VarChar(26), SuppID VarChar(6)
		 , PatternPanel VarChar(2), SizeItem VarChar(3), SizeItem_Elastic VarChar(3), ConsPC Numeric(8,4), Remark NVarChar(Max)
		 , BomTypeFactory Bit, BomTypeCustCD Bit, BomTypeZipper Bit, BomTypePo Bit
		 , BomTypeBuyMonth Bit, BomTypeCountry Bit, BomTypeStyle Bit, BomTypeSize Bit
		 , BomTypeArticle Bit, BomTypeColor Bit
		);
	Declare @BoaRowID Int;
	Declare @BoaRowCount Int;	--總資料筆數
	
	Insert Into @BoaCursor
		(  Ukey, SCIRefNo, SuppID, PatternPanel, SizeItem, SizeItem_Elastic, ConsPC, Remark
		 , BomTypeFactory, BomTypeCustCD, BomTypeZipper, BomTypePo
		 , BomTypeBuyMonth, BomTypeCountry, BomTypeStyle, BomTypeSize
		 , BomTypeArticle, BomTypeColor
		)
		Select Ukey, SCIRefNo, SuppID, PatternPanel, SizeItem, SizeItem_Elastic, ConsPC, Remark
			 , BomTypeFactory, BomTypeCustCD, BomTypeZipper, BomTypePo
			 , BomTypeBuyMonth, BomTypeCountry, BomTypeStyle, BomTypeSize
			 , BomTypeArticle, BomTypeColor
		  From dbo.Order_BOA
		 Where ID = @ID
		   And (   IsNull(@Order_BOAUkey, 0) = 0
				Or Ukey = @Order_BOAUkey
			   )
		 Order by Ukey;

	Set @BoaRowID = 1;
	--Select @BoaRowCount = Count(*) From @BoaCursor;
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
			 , @BoaBomTypeFactory = BomTypeFactory
			 , @BoaBomTypeCustCD = BomTypeCustCD
			 , @BoaBomTypeZipper = BomTypeZipper
			 , @BoaBomTypePo = BomTypePo
			 , @BoaBomTypeBuyMonth = BomTypeBuyMonth
			 , @BoaBomTypeCountry = BomTypeCountry
			 , @BoaBomTypeStyle = BomTypeStyle
			 , @BoaBomTypeSize = BomTypeSize
			 , @BoaBomTypeArticle = BomTypeArticle
			 , @BoaBomTypeColor = BomTypeColor
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
			(  ColorID, Article, BomFactory, BomCustCD, BomZipperInsert, BomCustPONo, BomBuymonth
			 , BomCountry, BomStyle, BomArticle, SizeSeq, SizeCode, SizeSpec, SizeUnit, OrderQty, UsageQty
			)
			Select ColorID, Article, BomFactory, BomCustCD, BomZipperInsert, BomCustPONo
				 , BomBuymonth, BomCountry, BomStyle, BomArticle, SizeSeq, SizeCode, SizeSpec, SizeUnit
				 , Sum(OrderQty) as OrderQty, Sum(UsageQty) as UsageQty
			  From (Select tmpOrder_Qty.ID
						 , IIF(@BoaBomTypeColor = 1, IsNull(Order_ColorCombo.ColorID,''), '') ColorID
						 , IIF(@BoaBomTypeColor = 1, tmpOrder_Qty.Article, '') Article
						 , IIF(@BoaBomTypeFactory = 1, tmpOrder_Qty.FactoryID, '') BomFactory
						 , IIF(@BoaBomTypeCustCD = 1, tmpOrder_Qty.CustCDID, '') BomCustCD
						 , IIF(@BoaBomTypeZipper = 1, tmpOrder_Qty.ZipperInsert, '') BomZipperInsert
						 , IIF(@BoaBomTypePo = 1, tmpOrder_Qty.CustPONo, '') BomCustPONo
						 , IIF(@BoaBomTypeBuyMonth = 1, tmpOrder_Qty.BuyMonth, '') BomBuymonth
						 , IIF(@BoaBomTypeCountry = 1, tmpOrder_Qty.CountryID, '') BomCountry
						 , IIF(@BoaBomTypeStyle = 1, tmpOrder_Qty.StyleID, '') BomStyle
						 , IIF(@BoaBomTypeArticle = 1, tmpOrder_Qty.Article, '') BomArticle
						 , IIF(@BoaBomTypeSize = 1, tmpOrder_Qty.SizeSeq, '') SizeSeq
						 , IIF(@BoaBomTypeSize = 1, tmpOrder_Qty.SizeCode, '') SizeCode
						 , IIF(@BoaBomTypeSize = 1, tmpOrder_SizeSpec.SizeSpec, '') SizeSpec
						 , IIF(@BoaBomTypeSize = 1, @SizeUnit, '') SizeUnit
						 , IIF(@BoaBomTypeSize = 1 And IsNull(tmpOrder_SizeSpec.SizeSpec, '') = '', 0, Qty) as OrderQty
						 , (Qty * IsNull(tmpOrder_SizeSpec.SizeSpec_Cal, 1) * @BoaConsPC) as UsageQty
					  From #tmpOrder_Qty as tmpOrder_Qty
					  Left Join dbo.Order_ColorCombo
						On	   Order_ColorCombo.ID = @ID
						   And Order_ColorCombo.Article = tmpOrder_Qty.Article
						   And Order_ColorCombo.LectraCode = @BoaPatternPanel
					  Left Join (Select ID, SizeItem, SizeCode, SizeSpec
									  , IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' Or @UsageUnit = 'INCH', dbo.GetDigitalValue(SizeSpec), 0), 1) as SizeSpec_Cal
								   From dbo.Order_SizeSpec
								) tmpOrder_SizeSpec
					    On	   tmpOrder_SizeSpec.ID = @ID
						   And tmpOrder_SizeSpec.SizeItem = @SizeItem
						   And tmpOrder_SizeSpec.SizeCode = tmpOrder_Qty.SizeCode
				   ) as tmpOrder_Qty
			 Group by ColorID, Article, BomFactory, BomCustCD, BomZipperInsert, BomCustPONo, BomBuymonth
					, BomCountry, BomStyle, BomArticle, SizeSeq, SizeCode, SizeSpec, SizeUnit
			 Order by BomFactory, BomCustCD, BomZipperInsert, BomCustPONo, BomBuymonth
					, BomCountry, BomStyle, Article, ColorID, BomArticle, SizeSeq, SizeCode, SizeSpec;
		
		Set @Sum_QtyRowID = 1;
		--Select @Sum_QtyRowCount = Count(*) From @Sum_Qty;
		Select @Sum_QtyRowID = Min(RowID), @Sum_QtyRowCount = Max(RowID) From @Sum_Qty;
		While @Sum_QtyRowID <= @Sum_QtyRowCount
		Begin
			Select @OrderID = OrderID
				 , @ColorID = ColorID
				 , @Article = Article
				 , @BomFactory = BomFactory
				 , @BomCustCD = BomCustCD
				 , @BomZipperInsert = BomZipperInsert
				 , @BomCustPONo = BomCustPONo
				 , @BomBuymonth = BomBuymonth
				 , @BomCountry = BomCountry
				 , @BomStyle = BomStyle
				 , @BomArticle = BomArticle
				 , @SizeCode = SizeCode
				 , @SizeSpec = SizeSpec
				 , @SizeUnit = SizeUnit
				 , @OrderQty = OrderQty
				 , @UsageQty = UsageQty
			  From @Sum_Qty
			 Where RowID = @Sum_QtyRowID;
			
			Set @SysUsageQty = @UsageQty;
			--取得 Supplier Color
			Set @SuppColor = dbo.GetSuppColorList(@SciRefNo, @BoaSuppID, @ColorID, @BrandID, @SeasonID, @ProgramID, @StyleID);
			
			--取得 Fabric Price
			Set @Price = IsNull(dbo.GetPriceFromMtl(@SciRefNo, @BoaSuppID, @SeasonID, @UsageQty, @Category, @CfmDate, '', @ColorID), 0);
			
			Insert Into #Tmp_BoaExpend
				(  ID, Order_BOAUkey, RefNo, SCIRefNo, Article, ColorID, SuppColor
				 , SizeCode, SizeSpec, SizeUnit, Remark, OrderQty, Price, UsageQty
				 , UsageUnit, SysUsageQty, BomFactory, BomCountry, BomStyle, BomCustCD
				 , BomArticle, BomZipperInsert, BomBuymonth, BomCustPONo
				)
			Values
				(  @ID, @BoaUkey, @RefNo, @SCIRefNo, @Article, @ColorID, @SuppColor
				 , @SizeCode, @SizeSpec, @SizeUnit, @Remark, @OrderQty, @Price, @UsageQty
				 , @UsageUnit, @SysUsageQty, @BomFactory, @BomCountry, @BomStyle, @BomCustCD
				 , @BomArticle, @BomZipperInsert, @BomBuymonth, @BomCustPONo
				);
			
			Set @ExpendUkey = @@IDENTITY;

			Insert Into #Tmp_BoaExpend_OrderList
				(ID, ExpendUkey, OrderID)
				Select @ID, @ExpendUkey, tmpOrder_Qty.ID
				  From #tmpOrder_Qty as tmpOrder_Qty
				  Left Join dbo.Order_ColorCombo
					On	   Order_ColorCombo.ID = @ID
					   And Order_ColorCombo.Article = tmpOrder_Qty.Article
					   And Order_ColorCombo.LectraCode = @BoaPatternPanel
				  Left Join dbo.Order_SizeSpec
				    On	   Order_SizeSpec.ID = @ID
					   And Order_SizeSpec.SizeItem = @SizeItem
					   And Order_SizeSpec.SizeCode = tmpOrder_Qty.SizeCode
				 Where IIF(@BoaBomTypeColor = 1, Order_ColorCombo.ColorID, '') = @ColorID
				   And IIF(@BoaBomTypeColor = 1, tmpOrder_Qty.Article, '') = @Article
				   And IIF(@BoaBomTypeFactory = 1, tmpOrder_Qty.FactoryID, '') = @BomFactory
				   And IIF(@BoaBomTypeCustCD = 1, tmpOrder_Qty.CustCDID, '') = @BomCustCD
				   And IIF(@BoaBomTypeZipper = 1, tmpOrder_Qty.ZipperInsert, '') = @BomZipperInsert
				   And IIF(@BoaBomTypePo = 1, tmpOrder_Qty.CustPONo, '') = @BomCustPONo
				   And IIF(@BoaBomTypeBuyMonth = 1, tmpOrder_Qty.BuyMonth, '') = @BomBuymonth
				   And IIF(@BoaBomTypeCountry = 1, tmpOrder_Qty.CountryID, '') = @BomCountry
				   And IIF(@BoaBomTypeStyle = 1, tmpOrder_Qty.StyleID, '') = @BomStyle
				   And IIF(@BoaBomTypeArticle = 1, tmpOrder_Qty.Article, '') = @BomArticle
				   And IIF(@BoaBomTypeSize = 1, tmpOrder_Qty.SizeCode, '') = @SizeCode
				   And IIF(@BoaBomTypeSize = 1 And IsNull(Order_SizeSpec.SizeSpec, '') = '', 0, Qty) > 0
				 Group by tmpOrder_Qty.ID;

			Set @Sum_QtyRowID += 1;
		End;

		Set @BoaRowID += 1
	End;

	Select * From #Tmp_BoaExpend;
	Select * From #Tmp_BoaExpend_OrderList;
	
	--若@TestType = 0，表示需實際寫入Table
	If @TestType = 0
	Begin
		Exec BoaExpend_Insert @ID, @Order_BOAUkey, @UserID;
	End;
	
	--Drop Table #Tmp_BoaExpend;
	Drop Table #Tmp_BoaExpend_OrderList;
End

CREATE function [dbo].[GetBOAExpend]
(
	  @ID				VarChar(13)				--採購母單
	 ,@Order_BOAUkey	BigInt		= 0			--BOA Ukey
	 ,@IsGetFabQuot		Bit			= 1
	 ,@IsExpendDetail	Bit			= 0			--是否一律展開至最詳細	 
	 ,@Tmp_Order_Qty    dbo.QtyBreakdown readonly
	 ,@IsExpendArticle	Bit			= 0			--add by Edward 是否展開至Article，For U.A轉單
	 ,@IncludeQtyZero	Bit			= 0			--add by Edward 是否包含數量為0
)
RETURNS @Tmp_BoaExpend table (  ExpendUkey BigInt Identity(1,1) Not Null, ID Varchar(13), Order_BOAUkey BigInt
	 , RefNo VarChar(20), SCIRefNo VarChar(30), Article VarChar(8), ColorID VarChar(6), SuppColor NVarChar(Max)
	 , SizeSeq VarChar(2), SizeCode VarChar(8), SizeSpec VarChar(15), SizeUnit VarChar(8), Remark NVarChar(Max)
	 , OrderQty Numeric(6,0), UsageQty Numeric(11,2), UsageUnit VarChar(8), SysUsageQty  Numeric(11,2)
	 , BomZipperInsert VarChar(5), BomCustPONo VarChar(30), Keyword VarChar(Max), OrderList varchar(max), ColorDesc varchar(150)
	 , Index Idx_ID NonClustered (ID, Order_BOAUkey, ColorID) -- table index
	)
As
begin
	
	--if not exists(select 1 from @Tmp_Order_Qty)
	--begin
	--	insert into @Tmp_Order_Qty
	--	select Order_Qty.ID ,Article ,SizeCode ,Order_Qty.Qty ,SewOutputQty ,SewOutputUpdate ,OriQty
	--		From dbo.Order_Qty
	--		Inner Join dbo.Orders On Orders.ID = Order_Qty.ID
	--	 Where Orders.PoID = @ID
	--end

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
	Declare @SCIRefNo Varchar(30);
	Declare @RefNo Varchar(20);
	Declare @OrderQty Numeric(6,0);
	Declare @UsageQty Numeric(11,2);
	Declare @UsageUnit Varchar(8);
	Declare @Article Varchar(8);
	Declare @ColorID Varchar(6);
	Declare @SuppColor NVarchar(70);
	Declare @SizeSeq Varchar(2);
	Declare @SizeItem VarChar(3);
	Declare @SizeCode Varchar(8);
	Declare @SizeSpec Varchar(15);
	Declare @SizeUnit Varchar(8);
	Declare @SysUsageQty Numeric(11,2)
	Declare @Remark NVarchar(Max);
	--Declare @BomFactory VarChar(8);
	--Declare @BomCustCD VarChar(16);
	Declare @BomZipperInsert VarChar(5);
	Declare @BomCustPONo VarChar(30);
	--Declare @BomBuymonth VarChar(16);
	--Declare @BomCountry VarChar(2);
	--Declare @BomStyle VarChar(15);
	Declare @BomArticle VarChar(8);
	Declare @Keyword VarChar(Max);
	Declare @Keyword_Trans VarChar(Max);
	Declare @OrderList VarChar(Max);

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
		(  RowID BigInt Identity(1,1) Not Null
		 , ID Varchar(13), OrderComboID VarChar(13), POID VarChar(13), FactoryID Varchar(8), CustCDID Varchar(16), ZipperInsert Varchar(5)
		 , CustPONo VarChar(30), BuyMonth VarChar(16), CountryID VarChar(2), StyleID Varchar(15)
		 , Article VarChar(8), SizeSeq VarChar(2), SizeCode VarChar(8), Qty Numeric(6,0)		 
		);
	Declare @Order_QtyRowID Int;		--tmpOrder_Qty ID
	Declare @Order_QtyRowCount Int;		--tmpOrder_Qty總資料筆數

	Insert Into @tmpOrder_Qty
		(ID, OrderComboID, POID, FactoryID, CustCDID, ZipperInsert
		 , CustPONo, BuyMonth, CountryID, StyleID
		 , Article, SizeSeq, SizeCode, Qty
		)
		Select Orders.ID, Orders.OrderComboID, Orders.POID, Orders.FactoryID, Orders.CustCDID, CustCD.ZipperInsert
			 , Orders.CustPONo, Orders.BuyMonth, Factory.CountryID, Orders.StyleID
			 , Order_Article.Article, Order_SizeCode.Seq, Order_SizeCode.SizeCode
			 , IsNull(Tmp_Order_Qty.Qty, 0) Qty
		From dbo.Orders
			Left Join dbo.Order_SizeCode 
				On Order_SizeCode.ID = Orders.POID
			Left Join dbo.Order_Article 
				On Order_Article.ID = Orders.ID
			Left Join @Tmp_Order_Qty as Tmp_Order_Qty 
				On Tmp_Order_Qty.ID = Orders.ID
					And Tmp_Order_Qty.SizeCode = Order_SizeCode.SizeCode
					And Tmp_Order_Qty.Article = Order_Article.Article
			Left Join Production.dbo.CustCD 
				On CustCD.BrandID = Orders.BrandID
					And CustCD.ID = Orders.CustCDID
			Left Join Production.dbo.Factory 
				On Factory.ID = Orders.FactoryID
		 Where Orders.POID = @ID
			And Orders.Junk = 0
		 Order By ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
			, CountryID, StyleID, Article, Seq, SizeCode;
	
	Declare @Sum_Qty Table
		( RowID BigInt Identity(1,1) Not Null, OrderID VarChar(13), ColorID VarChar(6), Article VarChar(8)
			, BomZipperInsert Varchar(5), BomCustPONo VarChar(30), BomArticle VarChar(8)
			, SizeSeq VarChar(2), SizeCode VarChar(8), SizeSpec VarChar(15), SizeUnit VarChar(8)
			, OrderQty Numeric(6,0), UsageQty Numeric(11,2), Keyword VarChar(Max), OrderList VarChar(Max)
		);
	Declare @tmpTbl Table
		( RowID BigInt Identity(1,1) Not Null, ID VarChar(13), ColorID VarChar(6), Article VarChar(8)
			, BomZipperInsert Varchar(5), BomCustPONo VarChar(30)
			, SizeSeq VarChar(2), SizeCode VarChar(8), SizeSpec VarChar(15), SizeUnit VarChar(8)
			, OrderQty Numeric(6,0), UsageQty Numeric(11,2), Keyword VarChar(Max)
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
		(  RowID BigInt Identity(1,1) Not Null, Ukey BigInt, SCIRefNo VarChar(30), SuppID VarChar(6)
			, FabricPanelCode VarChar(2), SizeItem VarChar(3), SizeItem_Elastic VarChar(3), ConsPC Numeric(8,4), Remark NVarChar(Max)
			, BomTypeZipper Bit, BomTypePo Bit
			, BomTypeSize Bit, BomTypeColor Bit
			, Keyword VarChar(Max), IsCustCD numeric(1,0)
		);
	Declare @BoaRowID Int;
	Declare @BoaRowCount Int;	--總資料筆數
	
	Insert Into @BoaCursor
		( Ukey, SCIRefNo, SuppID, FabricPanelCode, SizeItem, SizeItem_Elastic, ConsPC, Remark
			, BomTypeZipper, BomTypePo, BomTypeSize, BomTypeColor, Keyword, IsCustCD
		)
		Select Ukey, SCIRefNo, SuppID, FabricPanelCode, SizeItem, SizeItem_Elastic, ConsPC, Remark
			, BomTypeZipper, BomTypePo, BomTypeSize, BomTypeColor, Keyword, IsCustCD
		From dbo.Order_BOA
		Where ID = @ID
			And ( IsNull(@Order_BOAUkey, 0) = 0
				Or Ukey = @Order_BOAUkey
				)
			And SubString(Seq1, 1, 1) != '7'
			And RefNo != 'LABEL'
		 Order by Ukey;
	
	--取得此單的所有SP
	declare @OrderList_Full varchar(max) = (select ID+',' from Orders where POID = @ID order by ID for xml path(''))

	Set @BoaRowID = 1;
	Select @BoaRowID = Min(RowID), @BoaRowCount = Max(RowID) From @BoaCursor;
	While @BoaRowID <= @BoaRowCount
	Begin
		Select @BoaUkey = Ukey
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
			 , @Keyword = IsNull(Keyword, '')
			 , @BoaIsCustCD = IsCustCD
		From @BoaCursor
		Where RowID = @BoaRowID;

		--取得物料基本資料
		Select @RefNo = Refno
			, @MtlTypeID = MtlTypeID
			, @UsageUnit = UsageUnit
			, @BomTypeCalculate = BomTypeCalculate
			, @NoSizeUnit = NoSizeUnit
		From Production.dbo.Fabric
		Where SCIRefNo = @SciRefNo;
		
		--取得SizeItem,當為Elastic，且SizeItem為S開頭時，改取SizeItem_Elastic
		Set @SizeItem = IIF(@BoaSizeItem_Elastic != '', @BoaSizeItem_Elastic, @BoaSizeItem);
		
		--取得SizeUnit
		Select @tmpSizeUnit = IIF(@NoSizeUnit = 0, Order_SizeItem.SizeUnit, '')
		From dbo.Order_SizeItem
		Where Order_SizeItem.ID = @ID
		   And Order_SizeItem.SizeItem = @SizeItem;
		
		--判斷是否有 For Article
		Set @BoaIsForArticle = 0
		If Exists (Select 1 From dbo.Order_BOA_Article Where order_BOAUkey = @BoaUkey)
		Begin
			Set @BoaIsForArticle = 1;
		End;

		Delete From @Sum_Qty;
		
		Delete From @tmpTbl;

		insert into @tmpTbl
		Select tmpQtyBreakDown.ID
			, IIF(@BoaBomTypeColor = 1 Or @IsExpendDetail = 1, IsNull(Order_ColorCombo.ColorID,''), '') as ColorID
			, IIF(@BoaBomTypeColor = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.Article, '') as Article
			, IIF(@BoaBomTypeZipper = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.ZipperInsert, '') as BomZipperInsert
			, IIF(@BoaBomTypePo = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.CustPONo, '') as BomCustPONo
			, IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.SizeSeq, '') as SizeSeq
			, IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, tmpQtyBreakDown.SizeCode, '') as SizeCode
			--, IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, IsNull(tmpOrder_SizeSpec_OrderCombo.SizeSpec, tmpOrder_SizeSpec.SizeSpec), '') as SizeSpec
			, IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, IIF(IsNull(tmpExist_SizeSpec_OrderCombo.IsExist, 0) = 1, IsNull(tmpOrder_SizeSpec_OrderCombo.SizeSpec, ''), IsNull(tmpOrder_SizeSpec.SizeSpec, '')), '') as SizeSpec
			, IIF(@BoaBomTypeSize = 1 Or @IsExpendDetail = 1, @tmpSizeUnit, '') as SizeUnit
			, Qty as OrderQty
			--, (Qty * IIF(IsNull(@SizeItem, '') = '', 1, IsNull(IsNull(tmpOrder_SizeSpec_OrderCombo.SizeSpec_Cal, tmpOrder_SizeSpec.SizeSpec_Cal), IIF(@BomTypeCalculate = 1, 0, 1))) * @BoaConsPC) as UsageQty
			, (Qty * IIF(IsNull(@SizeItem, '') = '', 1, IsNull(IIF(IsNull(tmpExist_SizeSpec_OrderCombo.IsExist, 0) = 1, tmpOrder_SizeSpec_OrderCombo.SizeSpec_Cal, tmpOrder_SizeSpec.SizeSpec_Cal), IIF(@BomTypeCalculate = 1, 0, 1))) * @BoaConsPC) as UsageQty
			--取得 Keyword
			, dbo.Getkeyword( tmpQtyBreakDown.ID, @BoaUkey, @Keyword, tmpQtyBreakDown.Article, tmpQtyBreakDown.SizeCode ) as Keyword
		From @tmpOrder_Qty as tmpQtyBreakDown
			Left Join dbo.Order_ColorCombo On Order_ColorCombo.ID = @ID
			   And Order_ColorCombo.Article = tmpQtyBreakDown.Article
			   And Order_ColorCombo.FabricPanelCode = @BoaFabricPanelCode
			/*
			Left Join (Select ID, SizeItem, SizeCode, SizeSpec
						  , IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' Or @UsageUnit = 'INCH', Production.dbo.GetDigitalValue(SizeSpec), 0), 1) as SizeSpec_Cal
					   From dbo.Order_SizeSpec ) tmpOrder_SizeSpec
			On tmpOrder_SizeSpec.ID = tmpQtyBreakDown.OrderComboID
			   And tmpOrder_SizeSpec.SizeItem = @SizeItem
			   And tmpOrder_SizeSpec.SizeCode = tmpQtyBreakDown.SizeCode
		    */
			Outer Apply (	Select Order_SizeSpec.ID, Order_SizeSpec.SizeItem, SizeCode, SizeSpec
								 , IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' Or @UsageUnit = 'INCH',
										 Production.dbo.GetUnitQty(Order_SizeItem.SizeUnit,@UsageUnit,Production.dbo.GetDigitalValue(SizeSpec)), 0), 1) as SizeSpec_Cal
							  From dbo.Order_SizeSpec
							  inner join dbo.Order_SizeItem on Order_SizeSpec.Id = Order_SizeItem.id 
									and Order_SizeSpec.SizeItem = Order_SizeItem.SizeItem
							 Where Order_SizeSpec.ID = tmpQtyBreakDown.POID
							   And Order_SizeSpec.SizeItem = @SizeItem
							   And Order_SizeSpec.SizeCode = tmpQtyBreakDown.SizeCode
						) as tmpOrder_SizeSpec
			-----------------------------------------------------------------------------------------------------
			--2017/09/14 add by Ben, 當by OrderCombo設定SizeSpec時，一律抓取該OrderCombo的設定進行Expand
			Outer Apply (	Select IIF(Exists(	Select Top 1 1
												  From dbo.Order_SizeSpec_OrderCombo
												 Where Order_SizeSpec_OrderCombo.ID = tmpQtyBreakDown.POID
												   And Order_SizeSpec_OrderCombo.OrderComboID = tmpQtyBreakDown.OrderComboID
												   And Order_SizeSpec_OrderCombo.SizeItem = @SizeItem
											 )
									  , 1, 0) as IsExist
						) as tmpExist_SizeSpec_OrderCombo
			-----------------------------------------------------------------------------------------------------
			Outer Apply (	Select Order_SizeSpec_OrderCombo.ID, Order_SizeSpec_OrderCombo.SizeItem, SizeCode, SizeSpec
								 , IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' Or @UsageUnit = 'INCH',
										 Production.dbo.GetUnitQty(Order_SizeItem.SizeUnit,@UsageUnit,Production.dbo.GetDigitalValue(SizeSpec)), 0), 1) as SizeSpec_Cal
							  From dbo.Order_SizeSpec_OrderCombo
							  inner join dbo.Order_SizeItem on Order_SizeSpec_OrderCombo.Id = Order_SizeItem.id 
									and Order_SizeSpec_OrderCombo.SizeItem = Order_SizeItem.SizeItem
							 Where Order_SizeSpec_OrderCombo.ID = tmpQtyBreakDown.POID
							   And Order_SizeSpec_OrderCombo.OrderComboID = tmpQtyBreakDown.OrderComboID
							   And Order_SizeSpec_OrderCombo.SizeItem = @SizeItem
							   And Order_SizeSpec_OrderCombo.SizeCode = tmpQtyBreakDown.SizeCode
						) as tmpOrder_SizeSpec_OrderCombo
			Outer Apply (Select Top 1 SCIRefno From dbo.Order_BOA_CustCD Where Order_BOA_CustCD.Order_BOAUkey = @BoaUkey and CustCDID = tmpQtyBreakDown.CustCDID) as tmpReplaceSciRefNo
		Where 
		--排除For Article
		(@BoaIsForArticle = 0 or (@BoaIsForArticle = 1 And Exists (Select 1 From dbo.Order_BOA_Article Where order_BOAUkey = @BoaUkey And Article = tmpQtyBreakDown.Article)))
		--符合CustCD設定，包含設定為1 = 全部
		And (	(@BoaIsCustCD = 0 or @BoaIsCustCD = 1 or @BoaIsCustCD = 4)
			 Or	((@BoaIsCustCD = 2) And (Exists (Select Top 1 1 From dbo.Order_BOA_CustCD Where Order_BOA_CustCD.Order_BOAUkey = @BoaUkey And Order_BOA_CustCD.CustCDID = tmpQtyBreakDown.CustCDID)))
			 Or	((@BoaIsCustCD = 3) And (Not Exists (Select Top 1 1 From dbo.Order_BOA_CustCD Where Order_BOA_CustCD.Order_BOAUkey = @BoaUkey And Order_BOA_CustCD.CustCDID = tmpQtyBreakDown.CustCDID)))
			)
		And ((@BoaBomTypeColor = 0) or ((@BoaBomTypeColor = 1 Or @IsExpendDetail = 1) and isnull(Order_ColorCombo.ColorID,'') <> ''))
		And ((@BoaBomTypeSize = 0)  or ((@BoaBomTypeSize = 1 Or @IsExpendDetail = 1) And IIF(IsNull(tmpExist_SizeSpec_OrderCombo.IsExist, 0) = 1, IsNull(tmpOrder_SizeSpec_OrderCombo.SizeSpec, ''), IsNull(tmpOrder_SizeSpec.SizeSpec, '')) != ''))

		Insert Into @Sum_Qty
			(  ColorID, Article, BomZipperInsert, BomCustPONo
			 , SizeSeq, SizeCode, SizeSpec, SizeUnit, Keyword, OrderQty, UsageQty, Orderlist
			)
			Select ColorID, Article, BomZipperInsert, BomCustPONo
				 , SizeSeq, SizeCode, SizeSpec, SizeUnit, Keyword 
				 , Sum(OrderQty) as OrderQty, Sum(UsageQty) as UsageQty
				 , Orderlist
			From @tmpTbl tmp1
			Cross Apply (Select Orderlist = (Select distinct ID + ',' From @tmpTbl as tmp3 
				where  tmp3.ColorID = tmp1.ColorID and tmp3.Article = tmp1.Article 
					and tmp3.BomZipperInsert = tmp1.BomZipperInsert and tmp3.BomCustPONo = tmp1.BomCustPONo 
					and tmp3.SizeSeq = tmp1.SizeSeq and tmp3.SizeCode = tmp1.SizeCode 
					and tmp3.SizeSpec = tmp1.SizeSpec and tmp3.SizeUnit = tmp1.SizeUnit and tmp3.Keyword = tmp1.Keyword 
				for xml path(''))) as tmp2
			Where (@IncludeQtyZero = 0 and OrderQty > 0) or @IncludeQtyZero = 1
			Group by ColorID, Article, BomZipperInsert, BomCustPONo
				, SizeSeq, SizeCode, SizeSpec, SizeUnit, Keyword, Orderlist
			Order by BomZipperInsert, BomCustPONo
				, Article, ColorID, SizeSeq, SizeCode, SizeSpec;
		/*
		Set @Order_QtyRowID = 1;
		Select @Order_QtyRowID = Min(RowID), @Order_QtyRowCount = Max(RowID) From @tmpOrder_Qty;
		While @Order_QtyRowID <= @Order_QtyRowCount
		Begin
			Select @tmpID = ID
				 , @tmpPoID = POID
				 , @tmpArticle = Article
				 , @tmpSizeSeq = SizeSeq
				 , @tmpSizeCode = SizeCode
				 , @tmpZipperInsert = ZipperInsert
				 , @tmpCustPONo = CustPONo
				 , @tmpCustCDID = CustCDID
				 , @tmpQty = Qty
			  From @tmpOrder_Qty
			 Where RowID = @Order_QtyRowID;
			
			--排除For Article
			If @BoaIsForArticle = 1
			Begin
				If Not Exists (Select 1 From dbo.Order_BOA_Article Where order_BOAUkey = @BoaUkey And Article = @tmpArticle)
				Begin
					Set @Order_QtyRowID += 1;
					Continue;
				End;
			End;

			--If (@tmpBOACustCdIsExist = 0) Or ((@tmpBOACustCdIsExist = 1 And Exists (Select Top 1 1 From dbo.Order_BOA_CustCD Where Order_BOAUkey = @BoaUkey And CustCDID = @tmpCustCDID)))
			If	  (@BoaIsCustCD = 1)
			   Or ((@BoaIsCustCD = 2) And (Exists (Select Top 1 1 From dbo.Order_BOA_CustCD Where Order_BOAUkey = @BoaUkey And CustCDID = @tmpCustCDID)))
			   Or ((@BoaIsCustCD = 3) And (Not Exists (Select Top 1 1 From dbo.Order_BOA_CustCD Where Order_BOAUkey = @BoaUkey And CustCDID = @tmpCustCDID)))
			   Or (@BoaIsCustCD = 4)
			Begin
				Set @ColorID = '';
				Set @Article = '';
				If @BoaBomTypeColor = 1 Or @IsExpendDetail = 1
				Begin
					Select @ColorID = IsNull(Order_ColorCombo.ColorID, '')
					  From dbo.Order_ColorCombo
					 Where Order_ColorCombo.ID = @tmpPoID
					   And Order_ColorCombo.Article = @tmpArticle
					   And Order_ColorCombo.PatternPanel = @BoaFabricPanelCode;
					
					If @ColorID = ''
					Begin
						Set @Order_QtyRowID += 1;
						continue;
					End;

					Set @Article = @tmpArticle;
				End;

				--替換料
				If @BoaIsCustCD = 4
				Begin
					Select Top 1
						   @RefNo = IsNull(RefNo, @RefNo)
						 , @SCIRefNo = IsNull(SCIRefno, @SCIRefNo)
					  From dbo.Order_BOA_CustCD
					  Where Order_BOAUkey = @BoaUkey And CustCDID = @tmpCustCDID;
				End;

				If Not Exists (Select Top 1 1 From Production.dbo.Fabric Where SCIRefno = @SCIRefNo)
				Begin
					Set @Order_QtyRowID += 1;
					continue;
				End;
				
				Set @BomZipperInsert = '';
				If @BoaBomTypeZipper = 1 Or @IsExpendDetail = 1
				Begin
					Set @BomZipperInsert = @tmpZipperInsert;
				End;

				Set @BomCustPONo = '';
				If @BoaBomTypePo = 1 Or @IsExpendDetail = 1
				Begin
					Set @BomCustPONo = @tmpCustPONo
				End;
				
				Set @SizeSeq = '';
				Set @SizeCode = '';
				Set @SizeSpec = '';
				Set @SizeUnit = '';
				Set @SizeSpec_Cal = Null;
				
				Select @SizeSpec_Cal = IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' Or @UsageUnit = 'INCH', Production.dbo.GetDigitalValue(SizeSpec), 0), 1)
					 , @tmpSizeSpec = SizeSpec
				  From dbo.Order_SizeSpec
				 Where Order_SizeSpec.ID = @tmpPoID
				   And Order_SizeSpec.SizeItem = @SizeItem
				   And Order_SizeSpec.SizeCode = @tmpSizeCode;
								
				If @BoaBomTypeSize = 1 Or @IsExpendDetail = 1
				Begin
					Set @SizeSeq = @tmpSizeSeq;
					Set @SizeCode = @tmpSizeCode;
					Set @SizeUnit = @tmpSizeUnit;
					Set @SizeSpec = @tmpSizeSpec;
				End;

				Set @Keyword_Trans = '';
				
				If @Keyword != ''
				Begin
					Exec dbo.GetKeyword @tmpID, @BoaUkey, @Keyword, @Keyword_Trans Output, @tmpArticle, @tmpSizeCode;
				End;
				
				Update @tmpOrder_Qty Set Keyword = IsNull(@Keyword_Trans, '') Where RowID = @Order_QtyRowID;

				Set @OrderQty = @tmpQty;
				Set @UsageQty = @tmpQty * IIF(IsNull(@SizeItem, '') = '', 1, IsNull(@SizeSpec_Cal, IIF(@BomTypeCalculate = 1, 0, 1))) * @BoaConsPC;

				If Exists (Select Top 1 1 From @Sum_Qty
							Where ColorID = @ColorID
							  And Article = @Article
							  And BomZipperInsert = @BomZipperInsert
							  And BomCustPONo = @BomCustPONo
							  And SizeSeq = @SizeSeq
							  And SizeCode = @SizeCode
							  And SizeSpec = @SizeSpec
							  And SizeUnit = @SizeUnit
							  And Keyword = @Keyword_Trans
						  )
				Begin
					Update @Sum_Qty
					   Set OrderQty += @OrderQty
						 , UsageQty += @UsageQty
					 Where ColorID = @ColorID
					   And Article = @Article
					   And BomZipperInsert = @BomZipperInsert
					   And BomCustPONo = @BomCustPONo
					   And SizeSeq = @SizeSeq
					   And SizeCode = @SizeCode
					   And SizeSpec = @SizeSpec
					   And SizeUnit = @SizeUnit
					   And Keyword = @Keyword_Trans;
				End;
				Else
				Begin
					Insert Into @Sum_Qty
						(ColorID, Article, BomZipperInsert, BomCustPONo, SizeSeq, SizeCode, SizeSpec, SizeUnit, Keyword, OrderQty, UsageQty)
					Values
						(@ColorID, @Article, @BomZipperInsert, @BomCustPONo, @SizeSeq, @SizeCode, @SizeSpec, @SizeUnit, @Keyword_Trans, @OrderQty, @UsageQty);
				End;

			End;

			Set @Order_QtyRowID += 1;
		End;
		*/
		
		/*
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
				 , @Keyword_Trans = Keyword
				 , @OrderList = OrderList
			  From @Sum_Qty
			 Where RowID = @Sum_QtyRowID;
			/*
			--取得Keyword
			Set @Keyword_Trans = ''
			If @Keyword != ''
			Begin
				Exec dbo.GetKeyword @ID, @BoaUkey, @Keyword, @Keyword_Trans Output, @Article, @SizeCode;
			End;
			*/
			Set @SysUsageQty = @UsageQty;
			--取得 Supplier Color
			Set @SuppColor = IsNull(Production.dbo.GetSuppColorList(@SciRefNo, @BoaSuppID, @ColorID, @BrandID, @SeasonID, @ProgramID, @StyleID), '');
			
			--取得 Fabric Price
			If @IsGetFabQuot = 1
			Begin
				Set @Price = IsNull(Production.dbo.GetPriceFromMtl(@SciRefNo, @BoaSuppID, @SeasonID, @UsageQty, @Category, @CfmDate, '', @ColorID), 0);
			End;
			Else
			Begin
				Set @Price = 0;
			End;
			
			Insert Into @Tmp_BoaExpend
				(  ID, Order_BOAUkey, RefNo, SCIRefNo, Article, ColorID, SuppColor
				 , SizeCode, SizeSpec, SizeUnit, Remark, OrderQty, Price, UsageQty
				 , UsageUnit, SysUsageQty, BomZipperInsert, BomCustPONo, Keyword, OrderList
				)
			Values
				(  @ID, @BoaUkey, @RefNo, @SCIRefNo, @Article, @ColorID, @SuppColor
				 , @SizeCode, @SizeSpec, @SizeUnit, @Remark, @OrderQty, @Price, @UsageQty
				 , @UsageUnit, @SysUsageQty, @BomZipperInsert, @BomCustPONo, @Keyword_Trans, @OrderList
				);
			
			Set @Sum_QtyRowID += 1;
		End;
		*/
		Insert Into @Tmp_BoaExpend
			(  ID, Order_BOAUkey, RefNo, SCIRefNo, Article, ColorID, SuppColor
			 , SizeSeq, SizeCode, SizeSpec, SizeUnit, Remark, OrderQty, UsageQty
			 , UsageUnit, SysUsageQty, BomZipperInsert, BomCustPONo, Keyword, OrderList, ColorDesc
			)
			Select @ID, @BoaUkey, @RefNo, @SciRefNo, Sum_Qty.Article, Sum_Qty.ColorID, tmpSuppColor.SuppColor
				 ,Sum_Qty.SizeSeq , Sum_Qty.SizeCode, Sum_Qty.SizeSpec, Sum_Qty.SizeUnit, @Remark, Sum_Qty.OrderQty
				 , Sum_Qty.UsageQty
				 , @UsageUnit, Sum_Qty.UsageQty, Sum_Qty.BomZipperInsert, Sum_Qty.BomCustPONo, Sum_Qty.Keyword, Sum_Qty.OrderList, Color.Name
			  From @Sum_Qty as Sum_Qty
              Left Join Production.dbo.Color On Color.BrandID = @BrandID And Color.ID = Sum_Qty.ColorID

			 Outer Apply (Select IsNull(Production.dbo.GetSuppColorList(@SciRefNo, @BoaSuppID, Sum_Qty.ColorID, @BrandID, @SeasonID, @ProgramID, @StyleID), '') as SuppColor) as tmpSuppColor
		
		Set @BoaRowID += 1
	End;		
	
	--Update OrderList 包含整組的sp，改為空白
	update @Tmp_BoaExpend
		set OrderList = ''
	where OrderList = @OrderList_Full
	
	return ;
end

CREATE Function [dbo].[GetLossAccessory]
(
	  @PoID			VarChar(13)		--採購母單
	 ,@SCIRefNo		VarChar(26)		--SCI Ref No.(空值表示為全部計算)
)
Returns @AccessoryColorQty Table
	(  RowID			BigInt Identity(1,1) Not Null
	 , SciRefNo			VarChar(26)
	 , LossType			Numeric(1,0)
	 , MtltypeID		VarChar(20)
	 , ColorID			VarChar(100)
	 --, Article			VarChar(8)
	 --, SizeCode			VarChar(8)
	 , SizeSpec			VarChar(15)
	 --, BomFactory		VarChar(8)
	 --, BomCountry		VarChar(2)
	 --, BomStyle			VarChar(15)
	 --, BomCustCD		VarChar(20)
	 --, BomArticle		VarChar(8)
	 , BomZipperInsert	VarChar(5)
	 --, BomBuymonth		VarChar(10)
	 , BomCustPONo		VarChar(30)
	 , Keyword			NVarChar(Max)
	 , LossYds			Numeric(9,2)
	 , LossYds_FOC		Numeric(9,2)
	 , PlusName			VarChar(30)
	)
As
Begin
	Set @SCIRefNo = IsNull(@SCIRefNo, '');

	Declare @Category VarChar(1);
	Declare @BrandID VarChar(8);
	Declare @StyleID VarChar(15);
	Declare @SeasonID VarChar(10);
	Declare @LossSampleAccessory Numeric(3,1);
	
	Declare @MtltypeID VarChar(20);
	Declare @UsageUnit VarChar(8);
	
	Declare @LimitUp Numeric(7,2);
	Declare @LossUnit Numeric(1,0);
	Declare @PlusLoss Numeric(2,0);
	Declare @PerQty Numeric(5,0);
	Declare @PlusQty Numeric(3,0);
	Declare @FOC Numeric(2,0);

	Declare @LossYDS Numeric(9,2);
	Declare @LossYDS_FOC Numeric(9,2);
	Declare @PlusName VarChar(30);
	---------------------------------------------------------------------------
	Select @Category = Category
		 , @BrandID = BrandID
		 , @StyleID = StyleID
		 , @SeasonID = SeasonID
	  From dbo.Orders
	 Where ID = @PoID;
	
	Select @LossSampleAccessory = LossSampleAccessory
	  From Trade.dbo.Brand
	 Where ID = @BrandID;
	---------------------------------------------------------------------------
	Declare @BoaUkey BigInt;
	Declare @SuppID VarChar(6);
	Declare @Supp_Country VarChar(2);
	Declare @LossType Numeric(1,0);
	Declare @LossPercent Numeric(3,1);
	Declare @LossQty Numeric(3,0);
	Declare @LossStep Numeric(6,0);
	Declare @tmpBOA Table
		(  RowID BigInt Identity(1,1) Not Null, BoaUkey BigInt, SCIRefNo VarChar(26)
		 , SuppID VarChar(6), LossType Numeric(1,0), LossPercent Numeric(3,1)
		 , LossQty Numeric(3,0), LossStep Numeric(6,0)
		);
	Declare @tmpBOARowID Int;		--Row ID
	Declare @tmpBOARowCount Int;	--總資料筆數
	
	Declare @ColorID VarChar(100);
	--Declare @Article VarChar(8);
	--Declare @SizeCode VarChar(8);
	Declare @SizeSpec VarChar(15);
	--Declare @BomFactory VarChar(8);
	--Declare @BomCountry VarChar(2);
	--Declare @BomStyle VarChar(15);
	--Declare @BomCustCD VarChar(20);
	--Declare @BomArticle VarChar(8);
	Declare @BomZipperInsert VarChar(5);
	--Declare @BomBuymonth VarChar(10);
	Declare @BomCustPONo VarChar(30);
	Declare @Keyword NVarChar(Max);
	Declare @UsageQty Numeric(9,2);
	Declare @tmpBOA_Expend Table
		(  RowID BigInt Identity(1,1) Not Null, ColorID VarChar(100)--, Article VarChar(8)
		 --, SizeCode VarChar(8), BomFactory VarChar(8), BomZipperInsert VarChar(5)
		 , SizeSpec  VarChar(15), BomFactory VarChar(8), BomZipperInsert VarChar(5)
		 , BomCustPONo VarChar(30), Keyword NVarChar(Max), UsageQty Numeric(9,2)
		);
	Declare @tmpBOA_ExpendRowID Int;		--Row ID
	Declare @tmpBOA_ExpendRowCount Int;		--總資料筆數

	Insert Into @tmpBOA
		(BoaUkey, SCIRefNo, SuppID, LossType, LossPercent, LossQty, LossStep)
		Select Ukey, SCIRefNo, SuppID, LossType, LossPercent, LossQty, LossStep
		  From dbo.Order_BOA
		 Where ID = @PoID
		   And (   (@SCIRefNo = '')
				Or (	@SCIRefNo != ''
					And Order_BOA.SCIRefno = @SCIRefNo
				   )
			   )
		 Order by SCIRefNo;
	--------------------Loop Start @tmpBOA--------------------
	Set @tmpBOARowID = 1;
	Select @tmpBOARowID = Min(RowID), @tmpBOARowCount = Max(RowID) From @tmpBOA;
	While @tmpBOARowID <= @tmpBOARowCount
	Begin
		Select @BoaUkey = BoaUkey
			 , @SCIRefNo = SCIRefNo
			 , @SuppID = SuppID
			 , @LossType = LossType
			 , @LossPercent = LossPercent
			 , @LossQty = LossQty
			 , @LossStep = LossStep
		  From @tmpBOA
		 Where RowID = @tmpBOARowID;
		
		Set @Supp_Country = '';
		Select @Supp_Country = CountryID
		  From Trade.dbo.Supp
		 Where ID = @SuppID;
		
		Set @MtltypeID = '';
		Set @UsageUnit = '';
		Select @MtltypeID = MtltypeID
			 , @UsageUnit = UsageUnit
		  From Trade.dbo.Fabric
		 Where SciRefNo = @SciRefNo;
		
		Delete @tmpBOA_Expend;
		/*
		Insert Into @tmpBOA_Expend
			(  ColorID, Article, SizeCode, BomZipperInsert, BomCustPONo, UsageQty
			)
			Select ColorID, Article, SizeCode, BomZipperInsert, BomCustPONo, UsageQty
			 From dbo.Order_BOA_Expend
			Where Order_BOAUkey = @BoaUkey
			Order by ColorID, Article, SizeCode, Remark, BomZipperInsert, BomCustPONo, UsageQty;
		*/
		Insert Into @tmpBOA_Expend
			(  ColorID, SizeSpec, BomZipperInsert, BomCustPONo, KeyWord, UsageQty)
			Select ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Keyword, Sum(UsageQty) as UsageQty
			  From dbo.Order_BOA_Expend
			 Where Order_BOAUkey = @BoaUkey
			 Group by ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Keyword
			 Order by ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Keyword, UsageQty;
		--------------------Loop Start @tmpBOA_Expend--------------------
		Set @tmpBOA_ExpendRowID = 1;
		Select @tmpBOA_ExpendRowID = Min(RowID), @tmpBOA_ExpendRowCount = Max(RowID) From @tmpBOA_Expend;
		While @tmpBOA_ExpendRowID <= @tmpBOA_ExpendRowCount
		Begin
			Select @ColorID = ColorID
				 --, @Article = Article
				 --, @SizeCode = SizeCode
				 , @SizeSpec = IsNull(SizeSpec, '')
				 --, @BomFactory = BomFactory
				 --, @BomCountry = BomCountry
				 --, @BomStyle = BomStyle
				 --, @BomCustCD = BomCustCD
				 --, @BomArticle = BomArticle
				 , @BomZipperInsert = IsNull(BomZipperInsert, '')
				 --, @BomBuymonth = BomBuymonth
				 , @BomCustPONo = IsNull(BomCustPONo, '')
				 , @Keyword = IsNull(Keyword, '')
				 , @UsageQty = UsageQty
			  From @tmpBOA_Expend
			 Where RowID = @tmpBOA_ExpendRowID;
			
			Set @LossYDS = 0;
			Set @LossYDS_FOC = 0;
			Set @PlusName = '';
			If @LossType = 1 And @Category = 'S'
			Begin
				Set @LossYDS = @UsageQty * (@LossSampleAccessory / 100);
				Set @PlusName = 'Sample Loss:' + LTrim(Convert(VarChar(4), @LossSampleAccessory)) + '%';
			End;	--End If (@LossType = 1 And @Category = 'S')
			Else
			Begin
				If @LossType = 2	--Loss by Percent
				Begin
					Set @LossYDS = @UsageQty * (@LossPercent / 100);
					Set @PlusName = 'By Percent:' + LTrim(Convert(VarChar(4), @LossPercent)) + '%';
				End;	--End If (@LossType = 2)
				Else
				Begin
					If @LossType = 3	--Loss by Qty
					Begin
						Set @LossYDS = @LossQty * Ceiling(@UsageQty / @LossStep);
						Set @PlusName = 'By Qty:' + LTrim(Convert(VarChar(3), @LossQty)) + ' Step:' + LTrim(Convert(VarChar(6), @LossStep));
					End;	--End If (@LossType = 3)
					Else
					Begin
						Set @LossUnit = 0;
						Set @PlusLoss = 0;
						Set @PerQty = 0;
						Set @PlusQty = 0;
						Set @FOC = 0;
						Select @LossUnit = LossUnit
							 , @PlusLoss = IIF(@Supp_Country = 'TW', LossTW, LossNonTW)
							 , @PerQty = IIF(@Supp_Country = 'TW', PerQtyTW, PerQtyNonTW)
							 , @PlusQty = IIF(@Supp_Country = 'TW', PlsQtyTW, PlsQtyNonTW)
							 , @FOC = IIF(@Supp_Country = 'TW', FOCTW, FOCNonTW)
						  From Trade.dbo.LossRateAccessory
						 Where MtlTypeID = @MtltypeID
						
						If @@RowCount > 0
						Begin
							--損耗的上限依照TYPE ID和使用單位check是否有做設定
							Set @LimitUp = 0;
							Select @LimitUp = LimitUp
							  From Trade.dbo.LossRateAccessory_Limit
							 Where MtltypeID = @MtltypeID
							   And UsageUnit = @UsageUnit;

							If @LossUnit = 1	-- 1.(%)
							Begin
								Set @LossYDS = Round(@UsageQty * @PlusLoss / 100, 1);
								Set @PlusName = 'By Percent:' + LTrim(Convert(VarChar(4), @PlusLoss)) + '%';
							End;
							Else				-- 2.(Qty)
							Begin
								Set @LossYDS = @PlusQty * Ceiling(@UsageQty / @PerQty);
								Set @PlusName = 'By Qty:' + LTrim(Convert(VarChar(3), @PlusQty)) + ' Step:' + LTrim(Convert(VarChar(5), @PerQty));
							End;
						
							--計算過後如果超過上限值，則帶上限值。
							If @LimitUp > 0 And @LossYDS > @LimitUp
							Begin
								Set @LossYDS = @LimitUp;
							End;

							If @FOC > 0
							Begin
								Set @LossYDS_FOC = Round(@UsageQty * @FOC / 100, 1);
							End;	--End If @FOC > 0
						End;	--End If @@RowCount > 0
					End;	--End If !(@LossType = 3)
				End;	--End If @(@LossType = 2)
			End;	--End If !(@LossType = 1 And @Category = 'S')

			Insert Into @AccessoryColorQty
				--(  SciRefNo, LossType, MtltypeID, ColorID, Article, SizeCode
				(  SciRefNo, LossType, MtltypeID, ColorID, Keyword, SizeSpec
				 , BomZipperInsert, BomCustPONo, LossYds, LossYds_FOC, PlusName
				)
			Values
				--(  @SciRefNo, @LossType, @MtltypeID, @ColorID, @Article, @SizeCode
				(  @SciRefNo, @LossType, @MtltypeID, @ColorID, @Keyword, @SizeSpec
				 , @BomZipperInsert, @BomCustPONo, @LossYds, @LossYds_FOC, @PlusName
				);
			Set @tmpBOA_ExpendRowID += 1;
		End;
		--------------------Loop End @tmpBOA_Expend--------------------
		Set @tmpBOARowID += 1;
	End;
	--------------------Loop End @tmpBOA--------------------

	Return;
End
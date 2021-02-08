
CREATE Function [dbo].[GetLossAccessory]
(
	  @PoID			VarChar(13)		--採購母單
	 ,@SCIRefNo		VarChar(30)		--SCI Ref No.(空值表示為全部計算)
	 ,@PreExpend	bit
 	 ,@IsExpendArticle	Bit			= 0			--add by Edward 是否展開至Article，For U.A轉單
	 ,@IncludeQtyZero	Bit			= 0			--add by Edward 是否包含數量為0
)
Returns @AccessoryColorQty Table
	(  RowID			BigInt Identity(1,1) Not Null
	 , Seq1				VarChar(3)
	 , SciRefNo			VarChar(30)
	 , LossType			Numeric(1,0)
	 , MtltypeID		VarChar(20)
	 , ColorID			VarChar(6)
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
	 , Remark			NVarChar(Max)
	 , Keyword			NVarChar(Max)
	 , Special			NVarChar(Max)	--2018.05.15 Add by Ben
	 , LossYds			Numeric(12,2)
	 , LossYds_FOC		Numeric(12,2)
	 , PlusName			VarChar(100)
	)
As
Begin
	Set @SCIRefNo = IsNull(@SCIRefNo, '');
	
	Declare @Seq1 VarChar(3);
	Declare @Category VarChar(1);
	Declare @BrandID VarChar(8);
	Declare @StyleID VarChar(15);
	Declare @SeasonID VarChar(10);
	Declare @LossSampleAccessory Numeric(3,1);
	
	Declare @MtltypeID VarChar(20);
	Declare @UsageUnit VarChar(8);
	Declare @POUnit VarChar(8);
	Declare @LossQtyCalculateType Varchar(1);
	Declare @IsThread bit;
	Declare @LimitUp Numeric(7,2);
	Declare @LimitDown Numeric(7,2);
	Declare @LossUnit Numeric(1,0);
	Declare @PlusLoss Numeric(2,0);
	Declare @PerQty Numeric(5,0);
	Declare @PlusQty Numeric(3,0);
	Declare @FOC Numeric(2,0);

	Declare @LossYDS Numeric(12,2);
	Declare @LossYDS_FOC Numeric(12,2);
	Declare @PlusName VarChar(100);

	Declare @Tmp_Order_Qty dbo.QtyBreakdown;
	---------------------------------------------------------------------------
	Select @Category = Category
		 , @BrandID = BrandID
		 , @StyleID = StyleID
		 , @SeasonID = SeasonID
	  From dbo.Orders
	 Where ID = @PoID;
	
	Select @LossSampleAccessory = LossSampleAccessory
	  From Production.dbo.Brand
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
		(  RowID BigInt Identity(1,1) Not Null, BoaUkey BigInt, Seq1 VarChar(3), SCIRefNo VarChar(30)
		 , SuppID VarChar(6), LossType Numeric(1,0), LossPercent Numeric(3,1)
		 , LossQty Numeric(3,0), LossStep Numeric(6,0)
		);
	Declare @tmpBOARowID Int;		--Row ID
	Declare @tmpBOARowCount Int;	--總資料筆數
	
	Declare @ColorID VarChar(6);
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
	Declare @Remark NVarChar(Max);
	Declare @Keyword NVarChar(Max);
	Declare @Special NVarChar(Max);
	Declare @UsageQty Numeric(11,2);
	Declare @tmpBOA_Expend Table
		(  RowID BigInt Identity(1,1) Not Null, ColorID VarChar(6)--, Article VarChar(8)
		 --, SizeCode VarChar(8), BomFactory VarChar(8), BomZipperInsert VarChar(5)
		 , SizeSpec  VarChar(15), BomFactory VarChar(8), BomZipperInsert VarChar(5)
		 , BomCustPONo VarChar(30), Remark NVarChar(Max), Keyword NVarChar(Max), Special NVarChar(Max)
		 , UsageQty Numeric(11,2)
		);
	Declare @tmpBOA_ExpendRowID Int;		--Row ID
	Declare @tmpBOA_ExpendRowCount Int;		--總資料筆數
	
	Declare @BOA_TotalUsage Table
		(  RowID BigInt Identity(1,1) Not Null, Seq1 VarChar(3), SCIRefNo VarChar(30)
		 , SuppID VarChar(6), LossType Numeric(1,0), LossPercent Numeric(3,1)
		 , LossQty Numeric(3,0), LossStep Numeric(6,0)
		 , ColorID VarChar(6), SizeSpec  VarChar(15), BomFactory VarChar(8)
		 , BomZipperInsert VarChar(5), BomCustPONo VarChar(30)
		 , Remark NVarChar(Max), Keyword NVarChar(Max), Special NVarChar(Max)
		 , UsageQty Numeric(11,2), LimitUp Numeric(7,2), LimitDown Numeric(7,2)
		 , MtltypeID VarChar(20), UsageUnit VarChar(8), POUnit VarChar(8), LossQtyCalculateType VarChar(1)
		);
	Declare @BOA_TotalUsageRowID Int;		--Row ID
	Declare @BOA_TotalUsageRowCount Int;		--總資料筆數
	
	Insert Into @tmpBOA
		(BoaUkey, Seq1, SCIRefNo, SuppID, LossType, LossPercent, LossQty, LossStep)
		Select Ukey, Seq1, SCIRefNo, SuppID, LossType, LossPercent, LossQty, LossStep
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
			 , @Seq1 = Seq1
			 , @SCIRefNo = SCIRefNo
			 , @SuppID = SuppID
			 , @LossType = LossType
			 , @LossPercent = LossPercent
			 , @LossQty = LossQty
			 , @LossStep = LossStep
		  From @tmpBOA
		 Where RowID = @tmpBOARowID;
		
		Set @MtltypeID = '';
		Set @UsageUnit = '';
		set @POUnit = '';
		set @LossQtyCalculateType = 1;
		Select @MtltypeID = MtltypeID
			 , @UsageUnit = UsageUnit
			 , @POUnit = fs.POUnit
			 , @LossQtyCalculateType = mtl.LossQtyCalculateType
			 , @IsThread = mtl.IsThread
		  From Production.dbo.Fabric
			inner join Production.dbo.MtlType mtl on Fabric.MtltypeId = mtl.ID
			inner join Production.dbo.Fabric_Supp fs on Fabric.SCIRefno = fs.SCIRefno and fs.SuppID = @SuppID
		  Where Fabric.SciRefNo = @SciRefNo;


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
		if(@PreExpend = 0)
		begin		
			Insert Into @tmpBOA_Expend
				(  ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, KeyWord, Special, UsageQty)
				Select ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, Keyword, Special, Sum(UsageQty) as UsageQty
				  From dbo.Order_BOA_Expend
				 Where Order_BOAUkey = @BoaUkey
				 Group by ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, Keyword, Special
				 Order by ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, Keyword, Special, UsageQty;
		
			--2017.02.13 mark by Ben 暫時Mark
			--2017.02.23 Cancel mark by Edward 修正GetBOAExpend，keyword部分可正常執行
			--若不存在Boa展開，則預跑一次Boa展開資料
			if not exists(select 1 from @tmpBOA_Expend)
				Begin

					delete from @Tmp_Order_Qty

					insert into @Tmp_Order_Qty 
						Select Orders.ID ,Article ,SizeCode ,Order_Qty.Qty ,OriQty
						From dbo.Order_Qty
							Inner Join dbo.Orders On Orders.ID = Order_Qty.ID
						Where Orders.PoID = @PoID;
							
						Insert Into @tmpBOA_Expend
						( ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, KeyWord, Special, UsageQty )
							Select ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, Keyword, Special, Sum(UsageQty) as UsageQty
							from dbo.GetBOAExpend(@PoID,@BoaUkey,1,0,@Tmp_Order_Qty,@IsExpendArticle,@IncludeQtyZero)
							group by ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, Keyword, Special
				End;
		end
		else
		begin
			delete from @Tmp_Order_Qty

			insert into @Tmp_Order_Qty 
				Select Orders.ID ,Article ,SizeCode ,Order_Qty.Qty ,OriQty
				From dbo.Order_Qty
					Inner Join dbo.Orders On Orders.ID = Order_Qty.ID
				Where Orders.PoID = @PoID;
					
				Insert Into @tmpBOA_Expend
				( ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, KeyWord, Special, UsageQty )
					Select ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, Keyword, Special, Sum(UsageQty) as UsageQty
					from dbo.GetBOAExpend(@PoID,@BoaUkey,1,0,@Tmp_Order_Qty,@IsExpendArticle,@IncludeQtyZero)
					group by ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, Keyword, Special
		end

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
				 , @Remark = IsNull(Remark, '')
				 , @Keyword = IsNull(Keyword, '')
				 , @Special = IsNull(Special, '')
				 , @UsageQty = UsageQty
			  From @tmpBOA_Expend
			 Where RowID = @tmpBOA_ExpendRowID;

			Set @LimitUp = 0;
			Set @LimitDown = 0;
			
			--則回該SP抓上下限設定
			select 
				@LimitUp = LimitUp
				, @LimitDown = LimitDown
			from dbo.Order_BOA where Ukey = @BoaUkey;
			
			if (@LimitUp <=0 
				And Not Exists (
					select 1 from Production.dbo.LossRateAccessory
					where MtltypeId = @MtltypeId
					and CHARINDEX(@BrandID, IgnoreLimitUpBrand) > 0))
			BEGIN
				--損耗的上限依照TYPE ID和使用單位check是否有做設定
				Select 
					@LimitUp = LimitUp
				From Production.dbo.LossRateAccessory_Limit
				Where MtltypeID = @MtltypeID
				And UsageUnit = iif(@LossQtyCalculateType = '1', @UsageUnit, @POUnit);
			END
			
			If Exists (	Select 1 From @BOA_TotalUsage
						 Where Seq1 = @Seq1
						   And SCIRefNo = @SCIRefNo
						   And SuppID = @SuppID
						   And LossType = @LossType
						   And LossPercent = @LossPercent
						   And LossQty = @LossQty
						   And LossStep = @LossStep
						   And ColorID = @ColorID
						   And SizeSpec = @SizeSpec
						   And BomZipperInsert = @BomZipperInsert
						   And BomCustPONo = @BomCustPONo
						   And Remark = @Remark
						   And Keyword = @Keyword
						   And Special = @Special
						   And LimitUp = @LimitUp
						   And LimitDown = @LimitDown
						   And MtltypeID = @MtltypeID
						   And UsageUnit = @UsageUnit
						   And POUnit = @POUnit
						   And LossQtyCalculateType = @LossQtyCalculateType
					  )
			Begin
				Update @BOA_TotalUsage
				   Set UsageQty += @UsageQty
				 Where Seq1 = @Seq1
				   And SCIRefNo = @SCIRefNo
				   And SuppID = @SuppID
				   And LossType = @LossType
				   And LossPercent = @LossPercent
				   And LossQty = @LossQty
				   And LossStep = @LossStep
				   And ColorID = @ColorID
				   And SizeSpec = @SizeSpec
				   And BomZipperInsert = @BomZipperInsert
				   And BomCustPONo = @BomCustPONo
				   And Remark = @Remark
				   And Keyword = @Keyword
				   And Special = @Special
				   And LimitUp = @LimitUp
				   And LimitDown = @LimitDown
				   And MtltypeID = @MtltypeID
				   And UsageUnit = @UsageUnit
				   And POUnit = @POUnit
				   And LossQtyCalculateType = @LossQtyCalculateType;
			End;
			Else
			Begin
				Insert Into @BOA_TotalUsage
					(Seq1, SCIRefNo, SuppID, LossType, LossPercent, LossQty, LossStep
					 , ColorID, SizeSpec, BomZipperInsert, BomCustPONo, Remark, Keyword, Special
					 , UsageQty, LimitUp, LimitDown, MtltypeID, UsageUnit, POUnit, LossQtyCalculateType
					)
				Values
					(@Seq1, @SCIRefNo, @SuppID, @LossType, @LossPercent, @LossQty, @LossStep
					 , @ColorID, @SizeSpec, @BomZipperInsert, @BomCustPONo, @Remark, @Keyword, @Special
					 , @UsageQty, @LimitUp, @LimitDown, @MtltypeID, @UsageUnit, @POUnit, @LossQtyCalculateType
					)
			End;

			Set @tmpBOA_ExpendRowID += 1;
		End;
		--------------------Loop End @tmpBOA_Expend--------------------
		Set @tmpBOARowID += 1;
	End;
	--------------------Loop End @tmpBOA--------------------

	--------------------Loop Start @BOA_TotalUsage--------------------
	Set @BOA_TotalUsageRowID = 1;
	Select @BOA_TotalUsageRowID = Min(RowID), @BOA_TotalUsageRowCount = Max(RowID) From @BOA_TotalUsage;
	While @BOA_TotalUsageRowID <= @BOA_TotalUsageRowCount
	Begin
		Select @Seq1 = Seq1
			 , @SCIRefNo = SCIRefNo
			 , @SuppID = SuppID
			 , @LossType = LossType
			 , @LossPercent = LossPercent
			 , @LossQty = LossQty
			 , @LossStep = LossStep
			 , @ColorID = IsNull(ColorID, '')
			 , @SizeSpec = IsNull(SizeSpec, '')
			 , @BomZipperInsert = IsNull(BomZipperInsert, '')
			 , @BomCustPONo = IsNull(BomCustPONo, '')
			 , @Remark = IsNull(Remark, '')
			 , @Keyword = IsNull(Keyword, '')
			 , @Special = IsNull(Special, '')
			 , @UsageQty = UsageQty
			 , @LimitUp = LimitUp
			 , @LimitDown = LimitDown
			 , @MtltypeID = MtltypeID
			 , @UsageUnit = UsageUnit
			 , @POUnit = POUnit
			 , @LossQtyCalculateType = LossQtyCalculateType
		  From @BOA_TotalUsage
		 Where RowID = @BOA_TotalUsageRowID;
		
		If @@RowCount > 0
		Begin
			Set @Supp_Country = '';
			Select @Supp_Country = CountryID
			  From Production.dbo.Supp
			 Where ID = @SuppID;
			
			--依照LossQtyCalculateType決定要怎麼計算UsageQty
			set @UsageQty = iif(@LossQtyCalculateType = 1, @UsageQty, Production.dbo.GetCeiling(Production.dbo.GetUnitQty(@UsageUnit, @POUnit, @UsageQty), 0, 0))

			Set @LossYDS = 0;
			Set @LossYDS_FOC = 0;
			Set @PlusName = '';
			If @LossType = 1 And @Category In ('S','T')--2018/03/01 [IST20180148] modify by Anderson 加上Category=T判斷
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
						Set @LossYDS = iif(@LossStep = 0, 0, @LossQty * Ceiling(@UsageQty / @LossStep));
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
							 , @PerQty =  IIF(@LossQtyCalculateType = '1', PreQty, Production.dbo.GetUnitQty(@UsageUnit, @POUnit, PreQty))
							 , @PlusQty = IIF(@LossQtyCalculateType = '1', PlusQty, Production.dbo.GetUnitQty(@UsageUnit, @POUnit, PlusQty))
							 , @FOC = IIF(@Supp_Country = 'TW', FOCTW, FOCNonTW)
						  From Production.dbo.LossRateAccessory
						  outer apply (select PreQty = IIF(@Supp_Country = 'TW', PerQtyTW, PerQtyNonTW)) perq
						  outer apply (select PlusQty = IIF(@Supp_Country = 'TW', PlsQtyTW, PlsQtyNonTW)) plusq
						 Where MtlTypeID = @MtltypeID
						
						If @@RowCount > 0
						Begin
							If @LossUnit = 1	-- 1.(%)
							Begin
								Set @LossYDS = @UsageQty * @PlusLoss / 100;
								Set @PlusName = 'By Percent:' + LTrim(Convert(VarChar(4), @PlusLoss)) + '%';
							End;
							Else				-- 2.(Qty)
							Begin
								Set @LossYDS = IIF(@PerQty = 0, 0, @PlusQty * Ceiling(@UsageQty / @PerQty));								
								Set @PlusName = 'By Qty:' + LTrim(Convert(VarChar(3), @PlusQty)) + ' Step:' + LTrim(Convert(VarChar(5), @PerQty));
							End;
						
							If @FOC > 0
							Begin
								Set @LossYDS_FOC = Round(@UsageQty * @FOC / 100, 1);
							End;	--End If @FOC > 0
						End;	--End If @@RowCount > 0
					End;	--End If !(@LossType = 3)
				End;	--End If @(@LossType = 2)
			End;	--End If !(@LossType = 1 And @Category = 'S')

			--計算過後如果超過上限值，則帶上限值。
			If @LimitUp > 0 And @LossYDS > @LimitUp
			Begin
				Set @LossYDS = @LimitUp;
			End;

			--計算過後如果低於下限值，則帶下限值。
			If @LimitDown > 0 And @LossYDS < @LimitDown
			Begin
				Set @LossYDS = @LimitDown;
			End;

			If NOT EXISTS (SELECT * 
							 From @AccessoryColorQty
							 Where Seq1 = @Seq1
							 And SciRefNo = @SCIRefNo
							 And LossType = @LossType
							 And MtltypeID = @MtltypeID
							 And ColorID = @ColorID
							 And Remark = @Remark
							 And Keyword = @Keyword
							 And Special = @Special
							 And SizeSpec = @SizeSpec
							 And BomZipperInsert = @BomZipperInsert
							 And BomCustPONo = @BomCustPONo
							 And LossYds = @LossYds
							 And LossYds_FOC = @LossYds_FOC
							 And PlusName = @PlusName)
			Begin				
				Insert Into @AccessoryColorQty
					(  Seq1, SciRefNo, LossType, MtltypeID, ColorID, Remark, Keyword, Special, SizeSpec
					 , BomZipperInsert, BomCustPONo, LossYds, LossYds_FOC, PlusName
					)
				Values
					(  @Seq1, @SciRefNo, @LossType, @MtltypeID, @ColorID, @Remark, @Keyword, @Special, @SizeSpec
					 , @BomZipperInsert, @BomCustPONo, @LossYds, @LossYds_FOC, @PlusName
					);
			End
			ELSE
			BEGIN
				UPDATE @AccessoryColorQty
					SET 
						LossYDS += @LossYds
						, LossYds_FOC += @LossYds_FOC
				Where Seq1 = @Seq1
					And SciRefNo = @SCIRefNo
					And LossType = @LossType
					And MtltypeID = @MtltypeID
					And ColorID = @ColorID
					And Remark = @Remark
					And Keyword = @Keyword
					And Special = @Special
					And SizeSpec = @SizeSpec
					And BomZipperInsert = @BomZipperInsert
					And BomCustPONo = @BomCustPONo
					And LossYds = @LossYds
					And LossYds_FOC = @LossYds_FOC
					And PlusName = @PlusName
			END;
		End;

		Set @BOA_TotalUsageRowID += 1;
	End;
	--------------------Loop Start @BOA_TotalUsage--------------------
	Return;
End
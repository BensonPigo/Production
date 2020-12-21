

CREATE Function [dbo].[GetLossFabric]
(
	  @PoID			VarChar(13)		--採購母單
	 ,@FabricCode	VarChar(3)		--Fabric Code(空值表示為全部計算)
 	 ,@IsExpendArticle	Int			= 0			
	 --add by harry  0.無資料重展開BOF,不依Article展開 1.無資料重展開BOF,依Article展開 2.一律重展開BOF,不依Article展開 3.一律重展開BOF,依Article展開
)
Returns @FabricColorQty Table
	(  RowID		BigInt Identity(1,1) Not Null
	 , SciRefNo		VarChar(30)
	 , LossType		Numeric(1,0)
	 , WeaveTypeID	VarChar(20)
	 , FabricCode	VarChar(3)
	 , ColorID		VarChar(6)
	 , Special		NVarChar(Max)
	 , MarkerLength	Numeric(7,4)
	 , MarkerYDS	Numeric(10,4)
	 , LossUp       Numeric(10,4) -- 最終上限判斷值
	 , LossDown     Numeric(10,4) -- 最終下限判斷值
	 , LossYds		Numeric(10,4)
	 , RealLoss		Numeric(10,4)
	 , PlusName		VarChar(30)
	)
As
Begin
	Set @FabricCode = IsNull(@FabricCode, '');
	
	Declare @FabricColorQtyRowID Int;		--Row ID
	Declare @FabricColorQtyRowCount Int;		--資料筆數
	
	Declare @FabricColorQty_Detial Table
		(  RowID BigInt Identity(1,1) Not Null, SciRefNo VarChar(30), LossType Numeric(1,0)
		 , FabricCombo VarChar(2), FabricCode VarChar(3)
		 , ColorID VarChar(6), Special NVarChar(Max), Article VarChar(8), SizeSeq VarChar(2), SizeCode VarChar(8)
		 , MarkerLength Numeric(7,4), MarkerYDS Numeric(10,4), TotalQty Numeric(7,0), Qty Numeric(6,0)
		 , ConsPC Numeric(12,4), LossQty Numeric(6,0), LossYDS Numeric(10,4), LimitDown decimal(7, 2), LossUp decimal(7, 2), LossDown decimal(7, 2)
		);
	Declare @FabricColorQty_DetialRowID Int;		--Row ID
	Declare @FabricColorQty_DetialRowCount Int;		--資料筆數
	
	Declare @Category VarChar(1);
	Declare @BrandID VarChar(8);
	Declare @StyleID VarChar(15);
	Declare @SeasonID VarChar(10);
	Declare @FabricType VarChar(5);
	Declare @LimitUP_Rate Numeric(4,1);
	Declare @LimitUP_Allowance Numeric(4,0);
	Declare @LimitUP_LossQty Numeric(6,0);
	Declare @LossSampleFabric Numeric(3,1);

	Declare @MinGmtQty Numeric(6,0);
	Declare @MinLossQty Numeric(6,0);
	Declare @PerGmtQty Numeric(6,0);
	Declare @PlsLossQty Numeric(6,0);
	Declare @Real_MaxLossQty Numeric(6,0);
	
	Declare @SciRefNo VarChar(30);
	Declare @WeaveTypeID VarChar(20);
	Declare @ColorID VarChar(6);
	Declare @Article VarChar(8);
	Declare @SizeSeq VarChar(2);
	Declare @SizeCode VarChar(8);
	Declare @Qty_Total Numeric(6,0);
	Declare @Qty_Article Numeric(6,0);
	Declare @Qty Numeric(6,0);
	Declare @LossQty_Total Numeric(6,0);
	Declare @LossQty_Article Numeric(6,0);
	Declare @LossQty Numeric(6,0);
	Declare @Sum_Qty_Article Numeric(6,0);
	Declare @Sum_Qty Numeric(6,0);
	Declare @Sum_LossQty_Article Numeric(6,0);
	Declare @Sum_LossQty Numeric(6,0);
	Declare @LossYDS Numeric(10,4);
	Declare @RealLoss Numeric(10,4);
	Declare @PlusName VarChar(30);
	---------------------------------------------------------------------------
	Select @Category = Category
		 , @BrandID = BrandID
		 , @StyleID = StyleID
		 , @SeasonID = SeasonID
	  From dbo.Orders
	 Where ID = @PoID;
	
	Select @FabricType = FabricType
	  From production.dbo.Style
	 Where BrandID = @BrandID
	   And ID = @StyleID
	   And SeasonID = @SeasonID;
	
	--為了WOVEN、KNIT + spandex (8%↑)後代碼不同，需用同樣的Loss計算方式
	Declare @ReasonGroup Varchar(5) = ISNULL((SELECT ReasonGroup FROM Reason WHERE ReasonTypeID = 'Fabric_Kind' and ID = @FabricType), '')
	IF @ReasonGroup != ''
	BEGIN
		SET @FabricType = @ReasonGroup;
	END
	
	Select @LossSampleFabric = LossSampleFabric
	  From production.dbo.Brand
	 Where ID = @BrandID;
	
	Set @LimitUP_Rate = 0;
	Set @LimitUP_Allowance = 0;
	Set @LimitUP_LossQty = 0;
	Select @LimitUP_Rate = TWLimitUp
		 , @LimitUP_Allowance = Allowance
		 , @LimitUP_LossQty = MaxLossQty
		 , @MinGmtQty = MinGmtQty
		 , @MinLossQty = MinLossQty
		 , @PerGmtQty = PerGmtQty
		 , @PlsLossQty = PlsLossQty
	  From production.dbo.LossRateFabric
	 Where WeaveTypeID = @FabricType;
	---------------------------------------------------------------------------
	--取得各Article/Size的Qty數、By Article加總的Qty數、Qty總數
	Declare @Article_SizeCode_Qty Table
		(  Article VarChar(8), Seq VarChar(2), SizeCode VarChar(13)
		 , Qty Numeric(6,0), LossQty Numeric(6,0)
		);
	
	Insert Into @Article_SizeCode_Qty
		(Article, Seq, SizeCode, Qty)
		Select Article, Seq, SizeCode, Qty
		  From (Select Order_Qty.Article, Min(Order_SizeCode.Seq) as Seq, IsNull(Order_SizeSpec.SizeSpec, Order_Qty.SizeCode) as SizeCode, Sum(Order_Qty.Qty) as Qty
				 From dbo.Orders
				 Left Join dbo.Order_SizeCode
				   On Order_SizeCode.ID = Orders.PoID
				 Left Join dbo.Order_Qty
				   On	  Order_Qty.ID = Orders.ID
					  And Order_Qty.SizeCode = Order_SizeCode.SizeCode
				 Left Join dbo.Order_SizeSpec
				   On Order_SizeSpec.ID = Orders.PoID
					  And Order_SizeSpec.SizeItem = 'S00'
					  And Order_SizeSpec.SizeCode = Order_Qty.SizeCode
				Where Orders.PoID = @PoID
				  And dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1
				  And Order_Qty.Qty > 0
				Group by Order_Qty.Article, IsNull(Order_SizeSpec.SizeSpec, Order_Qty.SizeCode)
			   ) Sum_Article
		 Order by Article, Seq;
	
	--Total Qty
	Select @Qty_Total = Sum(Qty)
	  From @Article_SizeCode_Qty;

	--Total Loss Qty
	Set @LossQty_Total = Ceiling(@Qty_Total * @LimitUP_Rate / 100);

	Declare @Qty_Rate_Article Numeric(6,4);
	Declare @Article_Loss Table
		(RowID BigInt Identity(1,1) Not Null, Article VarChar(8), TtlQty Numeric(6,0));
	Declare @Article_LossRowID Int;		--Row ID
	Declare @Article_LossRowCount Int;	--資料筆數
	
	Declare @Qty_Rate Numeric(6,4);
	Declare @Size_Loss Table
		(RowID BigInt Identity(1,1) Not Null, SizeCode VarChar(8), Qty Numeric(6,0));
	Declare @Size_LossRowID Int;	--Row ID
	Declare @Size_LossRowCount Int;	--資料筆數
	
	Insert Into @Article_Loss
		(Article, TtlQty)
		Select Article, Sum(Qty) as TtlQty
		  From @Article_SizeCode_Qty
		 Group by Article
		 Order by TtlQty;

	Set @Sum_Qty = 0;
	Set @Sum_LossQty = 0;
	Set @Sum_Qty_Article = 0;
	Set @Sum_LossQty_Article = 0;
	--------------------Loop Start @Article_Loss--------------------
	--將Total Loss Qty依各Qty佔總Qty之比例，分攤至各Article/Size
	Set @Article_LossRowID = 1;
	Select @Article_LossRowID = Min(RowID), @Article_LossRowCount = Max(RowID) From @Article_Loss;
	While @Article_LossRowID <= @Article_LossRowCount
	Begin
		Select @Article = Article
			 , @Qty_Article = TtlQty
		  From @Article_Loss
		 Where RowID = @Article_LossRowID;
		--------------------------------
		--By Article分配所需的Loss Qty--
		Set @Sum_Qty_Article += @Qty_Article;	--加總已分配的Article Qty
		
		If @Sum_Qty_Article = @Qty_Total
		Begin
			-- 當已分配的Article Qty = Total Qty時
			-- Article Loss Qty = Total Loss Qty - 已分配Article Loss Qty
			Set @LossQty_Article = @LossQty_Total - @Sum_LossQty_Article;
		End;
		Else
		Begin
			--依照Article Qty佔Total Qty的比例，計算Article Loss Qty
			Set @Qty_Rate_Article = IIF(@Qty_Total = 0, 0, @Qty_Article / @Qty_Total);
			Set @LossQty_Article = Ceiling(@LossQty_Total * @Qty_Rate_Article);
		End;
		Set @Sum_LossQty_Article += @LossQty_Article;
		
		--2017.11.22 add by Ben, 控制最大Loss
		If @Sum_LossQty_Article > @LossQty_Total
		Begin
			Set @Sum_LossQty_Article = @LossQty_Total;
		End;
		--------------------------------
		
		--總Qty若小於設定的成衣限制，則以固定數為上限，若大於則以累進數為上限
		if(@Qty_Article < @MinGmtQty)
		BEGIN			
			If @LossQty_Article > @LimitUP_LossQty
			Begin
				set @LossQty_Article = @LimitUP_LossQty;
			End;
		END
		ELSE
		BEGIN
			set @Real_MaxLossQty = (Production.dbo.GetCeiling(@Qty_Article, 0, @PerGmtQty) / @PerGmtQty - 1) * @PlsLossQty
			If @LossQty_Article > @Real_MaxLossQty
			Begin
				Set @LossQty_Article = @Real_MaxLossQty;
			End;
		END
		
		--將Article Loss Qty 依比例分配至各Size
		Delete From @Size_Loss;
		Insert Into @Size_Loss
			(SizeCode, Qty)
			Select SizeCode, Qty
			  From @Article_SizeCode_Qty
			 Where Article = @Article
			 Order by Qty;
		
		Set @Sum_Qty = 0;
		Set @Sum_LossQty = 0;
		--------------------Loop Start @Size_Loss--------------------
		Set @Size_LossRowID = 1;
		Select @Size_LossRowID = Min(RowID), @Size_LossRowCount = Max(RowID) From @Size_Loss;
		While @Size_LossRowID <= @Size_LossRowCount
		Begin
			Select @SizeCode = SizeCode
				 , @Qty = Qty
			  From @Size_Loss
			 Where RowID = @Size_LossRowID;
			
			-------------------------------------
			--By Article/Size分配所需的Loss Qty--
			--依照Article/Size Qty佔Total Article Qty的比例，計算Article/Size Loss Qty
			Set @Qty_Rate = IIF(@Qty_Article = 0, 0, @Qty / @Qty_Article);
			Set @LossQty = Ceiling(@LossQty_Article * @Qty_Rate);
			
			-- 當已分配的Article/Size Qty >= Total Article Qty時
			-- Article/Size Loss Qty = Total Article Loss Qty - 已分配Article/Size Loss Qty
			If @Sum_LossQty + @LossQty >= @LossQty_Article
			Begin
				Set @LossQty = @LossQty_Article - @Sum_LossQty;
				Set @Sum_LossQty = 0;
			End;
			Else
			Begin
				Set @Sum_LossQty += @LossQty;
			End;
			-------------------------------------
			Update @Article_SizeCode_Qty
			   Set LossQty = @LossQty
			 Where Article = @Article
			   And SizeCode = @SizeCode;
			
			Set @Size_LossRowID += 1;
		End;
		--------------------Loop End @Size_Loss--------------------
		Set @Article_LossRowID += 1;
	End;
	--------------------Loop End @Article_Loss--------------------
	Declare @BofUkey BigInt;
	Declare @LossType Numeric(1,0);
	Declare @LossPercent Numeric(5,1);
	Declare @tmpBOF Table
		(RowID BigInt Identity(1,1) Not Null, BofUkey BigInt, FabricCode VarChar(3), SCIRefNo VarChar(30), LossType Numeric(1,0), LossPercent Numeric(5,1), SuppID varchar(6), Kind varchar(1), SpecialWidth numeric(5,1), LimitUp decimal(7, 2), LimitDown decimal(7, 2));
	Declare @tmpBOFRowID Int;		--Row ID
	Declare @tmpBOFRowCount Int;	--總資料筆數
	Declare @SuppID VarChar(6);
	Declare @Kind varchar(1); 
	Declare @SpecialWidth numeric(5,1)
	Declare @LimitUp decimal(7, 2)
	Declare @LimitDown decimal(7, 2)
	Declare @LossUp decimal(7, 2)
	Declare @LossDown decimal(7, 2)

	Declare @IsQT bit;
	Declare @CountryID VarChar(2);
	Declare @UsageQty Numeric(9,2);
	Declare @QTFabricPanelCode NVarChar(100);
	Declare @Special NVarChar(Max);
	Declare @tmpBOF_Expend Table
		(RowID BigInt Identity(1,1) Not Null, ColorID VarChar(6), Special NVarChar(Max), UsageQty Numeric(9,2), QTFabricPanelCode NVarChar(100));
	Declare @tmpBOF_ExpendRowID Int;		--Row ID
	Declare @tmpBOF_ExpendRowCount Int;		--總資料筆數
	
	Declare @FabricCombo VarChar(2);
	Declare @FabricPanelCode VarChar(2);
	Declare @CuttingPiece Bit;
	Declare @MarkerUkey BigInt;
	Declare @For_Article Numeric(1);
	Declare @ConsPC Numeric(12,4);
	Declare @MarkerLength Numeric(7,4);
	Declare @MarkerQty Numeric(4,0);
	Declare @MarkerYDS Numeric(10,4);
	Declare @MarkerList Table
		(  RowID BigInt Identity(1,1) Not Null, FabricCombo VarChar(2), FabricCode VarChar(3), FabricPanelCode VarChar(2), CuttingPiece Bit
		 , MarkerName VarChar(20), Ukey BigInt, For_Article Numeric(1)
		 , ConsPC Numeric(12,4), MarkerLength Numeric(7,4), SizeCode VarChar(8), Qty Numeric(4,0)
		);
	Declare @MarkerListRowID Int;		--Marker List Row ID
	Declare @MarkerListRowCount Int;	--Marker List 總資料筆數

	Declare @ColorCombo Table
		(RowID BigInt Identity(1,1) Not Null, Article VarChar(8), ColorID VarChar(6))
	Declare @ColorComboRowID Int;		--Color Combo Row ID
	Declare @ColorComboRowCount Int;	--Color Combo 總資料筆數

	Declare @WeightM2 numeric(5,1);
	Declare @mLimit numeric(1,0);
	Declare @LRFLossType numeric(1,0);
	Declare @LRFLimit numeric(4,0);	
	Declare @LRFLimitDown numeric(1,0);
	Declare @LRFLimitUp numeric(1,0);
	Declare @LRFTWLimitDown numeric(4,0);
	Declare @LRFNonTWLimitDown numeric(1,0);
	Declare @LRFTWLimitUp numeric(4,0);
	Declare @LRFNonTWLimitUp numeric(1,0);
	Declare @CalValue numeric(9,2);
	Declare @mPlus numeric(4,1);

	Insert Into @tmpBOF
		(BofUkey, FabricCode, SCIRefNo, LossType, LossPercent, SuppID, Kind, SpecialWidth, LimitUp, LimitDown)
		Select Ukey, FabricCode, SCIRefNo, LossType, LossPercent, SuppID, Kind, SpecialWidth, LimitUp, LimitDown
		  From dbo.Order_BOF
		 Where ID = @PoID
		   And (   @FabricCode = ''
				Or FabricCode = @FabricCode
			   )
		 Order by FabricCode;
	--------------------Loop Start @tmpBOF--------------------
	Set @tmpBOFRowID = 1;
	Select @tmpBOFRowID = Min(RowID), @tmpBOFRowCount = Max(RowID) From @tmpBOF;
	While @tmpBOFRowID <= @tmpBOFRowCount
	Begin
		Select @BofUkey = BofUkey
			 , @FabricCode = FabricCode
			 , @SciRefNo = SCIRefNo
			 , @LossType = LossType
			 , @LossPercent = LossPercent
			 , @SuppID = SuppID
			 , @Kind = Kind
			 , @SpecialWidth = SpecialWidth
			 , @LimitUp = LimitUp
			 , @LimitDown = LimitDown
		  From @tmpBOF
		 Where RowID = @tmpBOFRowID;

		select 
			@CountryID = CountryID
		from production.dbo.Supp where id = @SuppID;
		
		-- 2020/05/06 [IST20200411] 取得上限與下限設定,若BOF有設定先抓BOF,沒有設定再抓LossRateFabric
		if @LimitUp > 0
		Begin
			Set @LossUp = @LimitUp;
		End
		Else
		Begin
			Set @LossUp = @LimitUP_Allowance;
		End;

		Set @LossDown = isnull(@LimitDown,0);

		select 
			@CountryID = CountryID
		from Production.dbo.Supp where id = @SuppID;
		
		set @isQt = 0;
		select @isQt = 1 from Order_BOF 
		inner join Order_FabricCode on Order_BOF.Id = Order_FabricCode.Id and Order_BOF.FabricCode = Order_FabricCode.FabricCode 
		where Ukey = @BofUkey 
			AND FabricPanelCode in (select FabricPanelCode from Order_FabricCode_QT where Id = @PoID and FabricPanelCode <> QTFabricPanelCode)
			
		Delete @tmpBOF_Expend;
		if(@IsExpendArticle<2)
		begin
			INSERT INTO @tmpBOF_Expend (ColorID, Special, UsageQty, QTFabricPanelCode)
				SELECT
					ColorID
				   ,Special
				   ,UsageQty
				   ,QTFabricPanelCode
				FROM dbo.Order_BOF_Expend
				WHERE Order_BOFUkey = @BofUkey
				ORDER BY ColorID;

			--若不存在Bof展開，則預跑一次Bof展開資料
			IF NOT EXISTS (SELECT
						1
					FROM @tmpBOF_Expend)
			BEGIN
			INSERT INTO @tmpBOF_Expend (ColorID, Special, UsageQty, QTFabricPanelCode)
				SELECT
					ColorID
				   ,Special
				   ,SUM(UsageQty) AS UsageQty
				   ,QTFabricPanelCode
				FROM dbo.GetBOFExpend(@PoID, @FabricCode, @IsExpendArticle)
				GROUP BY ColorID
						,Special
						,QTFabricPanelCode
			END;
		END;
		Else
			Begin
				INSERT INTO @tmpBOF_Expend (ColorID, Special, UsageQty, QTFabricPanelCode)				
				SELECT
					ColorID
				   ,Special
				   ,SUM(UsageQty) AS UsageQty
				   ,QTFabricPanelCode
				FROM dbo.GetBOFExpend(@PoID, @FabricCode, @IsExpendArticle-2)
				GROUP BY ColorID
						,Special
						,QTFabricPanelCode
			END;
		--------------------Loop Start @tmpBOF_Expend--------------------
		Set @tmpBOF_ExpendRowID = 1;
		Select @tmpBOF_ExpendRowID = Min(RowID), @tmpBOF_ExpendRowCount = Max(RowID) From @tmpBOF_Expend;
		While @tmpBOF_ExpendRowID <= @tmpBOF_ExpendRowCount
		Begin
			Select @ColorID = IsNull(ColorID, '')
				 , @Special = IsNull(Special, '')
				 , @UsageQty = UsageQty
				 , @QTFabricPanelCode = QTFabricPanelCode
			  From @tmpBOF_Expend
			 Where RowID = @tmpBOF_ExpendRowID;
			
			Set @LossYDS = 0;
			Set @PlusName = '';
			
			If (@LossType = 1 And @Category In ('S','T')) Or (@LossType = 2) or (@IsQT = 1)--2018/03/01 [IST20180148] modify by Anderson 加上Category=T判斷
			Begin
				Set @WeaveTypeID = '';
				Set @WeightM2 = 0;
				Set @mLimit = 0;
				
				Select @WeaveTypeID = Fabric.WeaveTypeID,
					@WeightM2 = WeightM2
				  From production.dbo.Fabric
				 Where SciRefNo = @SciRefNo;
				
				--Loss by Dafault And Category In ('Sample', 'SMTL') 
				If (@LossType = 1 And @Category In ('S','T')) --2018/03/01 [IST20180148] modify by Anderson 加上Category=T判斷
				Begin
					Set @LossYDS = @UsageQty * (@LossSampleFabric / 100);
					Set @PlusName = 'Sample Loss:' + LTrim(Convert(VarChar(4), @LossSampleFabric)) + '%';
				End;
				else
				--Loss by Percent
				If (@LossType = 2)
				Begin
					Set @LossYDS = @UsageQty * (@LossPercent / 100);
					Set @PlusName = 'By Percent:' + LTrim(Convert(VarChar(5), @LossPercent)) + '%';
				End;
				
				if (@isQt = 1 and @SpecialWidth > 0) --2018/07/11 [IST20180936] modify by Vicky 移除@Kind = 3判斷
				begin
					set @WeaveTypeID = 'QT'

					if exists( select 1 from production.dbo.LossRateFabric where WeaveTypeID = @WeaveTypeID)
					begin
						select
							@LRFLimitUp = LimitUp,
							@LRFLimitDown = LimitDown,
							@LRFTWLimitUp = TWLimitUp,
							@LRFTWLimitDown = TWLimitDown,
							@LRFNonTWLimitUP = NonTWLimitUP,
							@LRFNonTWLimitDown = NonTWLimitDown,
							@LRFLimit = Limit,
							@LRFLossType = LossType
						from production.dbo.LossRateFabric where WeaveTypeID = @WeaveTypeID
						
						if (@LRFLossType = 2)
							set @CalValue = @WeightM2;
						else
							set @CalValue = @UsageQty;

						if(@CalValue > @LRFLimit)
						begin
							set @mLimit = @LRFLimitUp;
							set @mPlus = iif(@CountryID = 'TW', @LRFTWLimitUp, @LRFNonTWLimitUP);
						end
						else
						begin
							set @mLimit = @LRFLimitDown;
							set @mPlus = iif(@CountryID = 'TW', @LRFTWLimitDown, @LRFNonTWLimitDown);
						end						
						
						If (@mLimit = 1)
						Begin
							--根據....% ...計算
							Set @LossYDS = @UsageQty * (@mPlus / 100);
							Set @PlusName = 'By Percent:' + LTrim(Convert(VarChar(4), @mPlus)) + '%';
						End
						ELSE
						Begin
							--根據....Qty ...計算
							Set @LossYDS = @mPlus;
							Set @PlusName = 'By Qty: LossQty =' + LTrim(Convert(VarChar(4), @mPlus));
						End;
					end
				end
				
				/*--計算過後如果超過上限值，則帶上限值。
				If @LimitUp > 0 And @LossYDS > @LimitUp
				Begin
					Set @LossYDS = @LimitUp;
				End;*/

				--計算過後如果低於下限值，則帶下限值。
				If @LimitDown > 0 And @LossYDS < @LimitDown
				Begin
					Set @LossYDS = @LimitDown;
				End;

				Insert Into @FabricColorQty
					(SciRefNo, LossType, WeaveTypeID, FabricCode, ColorID, Special, MarkerLength, MarkerYDS, LossUp, LossDown, LossYds, RealLoss, PlusName)
				Values
					(@SciRefNo, @LossType, @WeaveTypeID, @FabricCode, @ColorID, @Special, 0, 0, @LossUp, @LossDown, @LossYDS, @LossYDS, @PlusName);
			End;	-- End If (@LossType = 1 And @Category = 'S') Or (@LossType = 2)
			Else
			Begin
				Delete From @MarkerList;
				Insert Into @MarkerList
					(  FabricCombo, FabricCode, FabricPanelCode, CuttingPiece, MarkerName, Ukey, For_Article
					 , ConsPC, MarkerLength, SizeCode, Qty
					)
					Select FabricCombo, FabricCode, FabricPanelCode, CuttingPiece, MarkerName, Ukey
						 , IsNull((Select Top 1 1 From dbo.Order_MarkerList_Article Where Order_MarkerList_Article.Order_MarkerlistUkey = Order_MarkerList.Ukey), 0) as For_Article
						 , ConsPC, dbo.MarkerLengthToYDS(MarkerLength) MarkerLength
						 , SizeCode, Qty
					  From dbo.Order_MarkerList
					  Left Join dbo.Order_MarkerList_SizeQty
						On Order_MarkerList_SizeQty.Order_MarkerListUkey = Order_MarkerList.Ukey
					 Where Order_MarkerList.ID = @PoID
					   And Order_MarkerList.FabricCode = @FabricCode
					   And Order_MarkerList.MixedSizeMarker = 1
					 Order by FabricCombo, FabricCode, FabricPanelCode, CuttingPiece, MarkerName, SizeCode;
				--------------------Loop Start @MarkerList--------------------
				Set @MarkerListRowID = 1;
				Select @MarkerListRowID = Min(RowID), @MarkerListRowCount = Max(RowID) From @MarkerList;
				While @MarkerListRowID <= @MarkerListRowCount
				Begin
					Select @FabricCombo = FabricCombo
						 , @FabricPanelCode = FabricPanelCode
						 , @CuttingPiece = CuttingPiece
						 , @MarkerUkey = Ukey
						 , @For_Article = For_Article
						 , @ConsPC = ConsPC
						 , @MarkerLength = MarkerLength
						 , @SizeCode = SizeCode
						 , @MarkerQty = Qty
					  From @MarkerList
					 Where RowID = @MarkerListRowID;
					
					Set @MarkerYDS = @MarkerQty * 2 * @ConsPC	--計算兩層所需的用量
					
					Delete From @ColorCombo;
					Insert Into @ColorCombo
						(Article, ColorID)
						Select Article, ColorID
						  From dbo.Order_ColorCombo
						 Where ID = @PoID
						   And FabricPanelCode = @FabricPanelCode
						   And ColorID = @ColorID
						   --And Article = @Special
						   And (	(@Special != '' And Article = @Special)
								Or	(@Special = '')
							   )
						   And (   @For_Article = 0
								Or (	@For_Article = 1
									--And Order_ColorCombo.Article In (Select Article From dbo.Order_MarkerList_Article Where Order_MarkerlistUkey = @MarkerUkey)
									And Exists (Select 1 From dbo.Order_MarkerList_Article Where Order_MarkerlistUkey = @MarkerUkey And Article = Order_ColorCombo.Article)
								   )
							   );
					--------------------Loop Start @ColorCombo--------------------
					Set @ColorComboRowID = 1;
					Select @ColorComboRowID = Min(RowID), @ColorComboRowCount = Max(RowID) From @ColorCombo;
					While @ColorComboRowID <= @ColorComboRowCount
					Begin
						Select @Article = Article
						  From @ColorCombo
						 Where RowID = @ColorComboRowID;
						------------------------------------
						--取得該Article/Size所需的Loss Qty--
						Set @SizeSeq = '';
						Set @Qty = 0;
						Set @LossQty = 0;
						Select @SizeSeq = Seq
							 , @Qty = Qty
							 , @LossQty = LossQty
						  From @Article_SizeCode_Qty
						 Where Article = @Article
						   And SizeCode = @SizeCode;
						------------------------------------
						If @@RowCount > 0
						Begin
							Set @LossYDS = @LossQty * @ConsPC;

							/*--計算過後如果超過上限值，則帶上限值。
							If @LimitUp > 0 And @LossYDS > @LimitUp
							Begin
								Set @LossYDS = @LimitUp;
							End;*/

							--計算過後如果低於下限值，則帶下限值。
							/*If @LimitDown > 0 And @LossYDS < @LimitDown
							Begin
								Set @LossYDS = @LimitDown;
							End;*/

							Update @FabricColorQty_Detial
							   Set Qty += @Qty
							 Where SciRefNo = @SciRefNo
							   And FabricCode = @FabricCode
							   And ColorID = @ColorID
							   And Special = @Special
							   And Article = @Article
							   And SizeCode = @SizeCode
							   And MarkerLength = @MarkerLength
							   And ConsPC = @ConsPC;
				
							If @@RowCount = 0
							Begin
								Insert Into @FabricColorQty_Detial
									(SciRefNo, LossType, FabricCode, FabricCombo, ColorID, Special, Article, SizeSeq, SizeCode, MarkerLength, MarkerYDS
									 , Qty, ConsPC, LossQty, LossYDS, LimitDown, LossUp, LossDown
									)
								Values
									(@SciRefNo, @LossType, @FabricCode, @FabricCombo, @ColorID, @Special, @Article, @SizeSeq, @SizeCode, @MarkerLength, @MarkerYDS
									 , @Qty, @ConsPC, @LossQty, @LossYDS, @LimitDown, @LossUp, @LossDown
									);
							End;
						End;
						Set @ColorComboRowID += 1;
					End;
					--------------------Loop End @ColorCombo--------------------
					Set @MarkerListRowID += 1;
				End;
				--------------------Loop End @MarkerList--------------------
			End;	-- End If !((@LossType = 1 And @Category = 'S') Or (@LossType = 2))
			Set @tmpBOF_ExpendRowID += 1;
		End;
		--------------------Loop End @tmpBOF_Expend--------------------
		Set @tmpBOFRowID += 1;
	End;
	--------------------Loop End @tmpBOFRowID--------------------
	---------------------------------------------------------------------------
	-- By 布種/顏色加總Loss Yds以及取得該布種最長的Marker Length及其所需之兩層長度
	Set @FabricColorQty_DetialRowID = 1;
	Select @FabricColorQty_DetialRowID = Min(RowID), @FabricColorQty_DetialRowCount = Max(RowID) From @FabricColorQty_Detial;
	While @FabricColorQty_DetialRowID <= @FabricColorQty_DetialRowCount
	Begin
		Select @SciRefNo = SciRefNo
			 , @LossType = LossType
			 , @FabricCode = FabricCode
			 , @ColorID = ColorID
			 , @Special = Special
			 , @MarkerLength = MarkerLength
			 , @MarkerYDS = MarkerYDS
			 , @LossYDS = LossYDS
			 , @LimitDown = LimitDown
			 , @LossUp = LossUp
			 , @LossDown = LossDown
		  From @FabricColorQty_Detial
		 Where RowID = @FabricColorQty_DetialRowID;
		
		Set @WeaveTypeID = '';
		Select @WeaveTypeID = Fabric.WeaveTypeID
		  From Production.dbo.Fabric
		  Join Production.dbo.LossRateFabric
			On LossRateFabric.WeaveTypeID = Fabric.WeaveTypeID
		 Where SciRefNo = @SciRefNo;
				
		Update @FabricColorQty
		   Set LossYds += @LossYDS
			 , MarkerLength = IIF(MarkerLength <= @MarkerLength, @MarkerLength, MarkerLength)
			 , MarkerYDS = IIF(MarkerLength <= @MarkerLength, @MarkerYDS, MarkerYDS)
		 Where SciRefNo = @SciRefNo
		   And FabricCode = @FabricCode
		   And ColorID = @ColorID
		   And Special = @Special
		
		If @@RowCount = 0
		Begin
			INSERT INTO @FabricColorQty (SciRefNo, LossType, WeaveTypeID, FabricCode, ColorID, Special, MarkerLength, MarkerYDS, LossUp, LossDown, LossYDS)
				VALUES (@SciRefNo, @LossType, @WeaveTypeID, @FabricCode, @ColorID, @Special, @MarkerLength, @MarkerYDS, @LossUp, @LossDown, @LossYDS);
			END;
			SET @FabricColorQty_DetialRowID += 1;
	End;
			---------------------------------------------------------------------------
			--計算最終的Loss YDS
			-- 1. 當Fabric的WeaveType不存在於LossRateFabric內時，Loss YDS = 0
			-- 2. 當Loss Yds小於最終判斷上限值(LossDown)，Loss YDS = LossDown
			-- 3. 當Loss Yds大於最終判斷上限值(LossUp)，Loss YDS = LossUp
			-- LossUp => 比較最長馬克之兩層長度(MarkerYDS)與BOF設定的Lower Limit,取大的
			-- LossDown => 先抓BOF設定的Upper Limit若沒有設定則抓系統設定值
			-- 2020/05/06 [IST20200411] 取消Category判斷,不看Category
			UPDATE @FabricColorQty
			SET RealLoss = IIF(ISNULL(WeaveTypeID, '') = '', 0
								, IIF(IIF(MarkerYDS >= LossYDS, MarkerYDS, LossYDS) >= LossUp, LossUp
									 , IIF(IIF(MarkerYDS >= LossDown, MarkerYDS, LossDown) >= LossYDS, iif(MarkerYDS >= LossDown, MarkerYDS, LossDown), LossYDS)
									 )
							 )
			, LossDown = iif(MarkerYDS >= LossDown, MarkerYDS, LossDown)
			WHERE LossType = 1;

			UPDATE @FabricColorQty
			SET PlusName = CONVERT(VARCHAR(9), RealLoss)
			WHERE LossType = 1
			AND WeaveTypeID <> 'QT';
	
	Return;
End
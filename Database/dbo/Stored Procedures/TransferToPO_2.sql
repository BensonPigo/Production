-- =============================================
-- Author:		Jeff
-- Create date: 2023/02/16
-- Description:	From trade 只取需要部份 for < WH P01 Material Compare >
-- =============================================
Create Procedure [dbo].[TransferToPO_2]
	(
	  @PoID			VarChar(13)		--採購母單
	 ,@AppType		Bit				--重新產生資料時是否要覆蓋原始資料 -- < WH P01 Material Compare > = 0
	 ,@TestType		Int				--資料來源是否為暫存檔
	 ,@UserID		VarChar(10) = ''
	)
As
Begin
	Set NoCount On;
	----------------------------------------------------------------------
	If Object_ID('tempdb..#tmpPO_Supp') Is Null
	Begin
        Create Table #tmpPO_Supp
        (  RowID BigInt Identity(1,1) Not Null, ID VarChar(13), Seq1 VarChar(3), SuppID VarChar(6) default ''
            --, ShipTermID VarChar(5) default '', PayTermAPID VarChar(5) default ''
            --, Remark NVarChar(Max) default ''
            --, Description NVarChar(Max) default '', CompanyID Numeric(2,0) default 0
            , StyleID VarChar(15)
            , Junk Bit
            , Primary Key (ID, Seq1)
        );
	End;
	If Object_ID('tempdb..#tmpPO_Supp_Detail') Is Null
	Begin
        Create Table #tmpPO_Supp_Detail
        (  RowID BigInt Identity(1,1) Not Null, ID VarChar(13), Seq1 VarChar(3), Seq2 VarChar(2), RefNo VarChar(36) default '', SCIRefNo VarChar(30) default ''
            , FabricType VarChar(1) default ''
            --, Price Numeric(14,4) default 0
            --, UsedQty Numeric(10,4) default 0
            , Qty Numeric(10,2) default 0
            , POUnit VarChar(8) default '', Complete Bit default 0, SystemETD Date, CFMETD Date, RevisedETD Date, FinalETD Date, EstETA Date
            , ShipModeID VarChar(10) default '', PrintDate DateTime, PINO VarChar(25) default '', PIDate Date
            , ColorID VarChar(6) default '', SuppColor NVarChar(Max) default '', SizeSpec VarChar(15) default '', SizeUnit VarChar(8) default ''
            --, Remark NVarChar(Max) default ''
            , Special NVarChar(Max) default '', Width Numeric(5,2) default 0
            , StockQty Numeric(12,1) default 0, NetQty Numeric(10,2) default 0, LossQty Numeric(10,2) default 0, SystemNetQty Numeric(10,2) default 0
            , SystemCreate bit default 0, FOC Numeric(10,2) default 0, Junk bit default 0, ColorDetail NVarChar(200) default ''
            , BomZipperInsert VarChar(5) default '', BomCustPONo VarChar(30) default ''
            , ShipQty Numeric(10,2) default 0, Shortage Numeric(10,2) default 0, ShipFOC Numeric(10,2) default 0, ApQty Numeric(10,2) default 0
            , InputQty Numeric(10,2) default 0, OutputQty Numeric(10,2) default 0, Spec NVarChar(Max) default '', ShipETA Date, SystemLock Date
            , OutputSeq1 VarChar(3) default '', OutputSeq2 VarChar(2) default '', FactoryID VarChar(8) default ''
            , StockPOID VarChar(13) default '', StockSeq1 VarChar(3) default '', StockSeq2 VarChar(2) default '', InventoryUkey bigint default 0
            , KeyWord NVarChar(Max) default '', Article varchar(8)
            , Seq2_Count Int
            --, Remark_Shell NVarChar(Max) default ''
            , Status varchar(1), Sel bit default 0, IsForOtherBrand bit, CannotOperateStock bit, Keyword_Original varchar(max)    
            Primary Key (ID, Seq1, Seq2, Seq2_Count)
        );
		Create Table #tmpPO_Supp_Detail_OrderList
			(  RowID BigInt Identity(1,1) Not Null, ID VarChar(13), Seq1 VarChar(3), Seq2 VarChar(2), OrderID VarChar(13), Seq2_Count Int
			 , Primary Key (ID, Seq1, Seq2, OrderID, Seq2_Count)
			);
		Create Table #tmpPO_Supp_Detail_Spec
		(  RowID BigInt Identity(1,1) Not Null, ID VarChar(13), Seq1 VarChar(3), Seq2 VarChar(2), SpecColumnID VarChar(50), SpecValue VarChar(50), Seq2_Count Int
			, Primary Key (ID, Seq1, Seq2, SpecColumnID, Seq2_Count)
		);
		Create Table #tmpPO_Supp_Detail_Keyword
		(  RowID BigInt Identity(1,1) Not Null, ID VarChar(13), Seq1 VarChar(3), Seq2 VarChar(2), KeywordField VarChar(30), KeywordValue VarChar(200), Seq2_Count Int
			, Primary Key (ID, Seq1, Seq2, KeywordField, Seq2_Count)
		);
	End;
	----------------------------------------------------------------------
	--Declare @ExecDate DateTime;
	--Set @ExecDate = GetDate();

	Declare @Seq1 VarChar(3);
	Declare @SuppID VarChar(6);
	--Declare @ShipTermID VarChar(5);
	--Declare @PayTermAPID VarChar(5);
	--Declare @Remark NVarChar(Max);
	--Declare @Description NVarChar(Max);
	--Declare @CompanyID Numeric(2,0);

	--Declare @Seq2 VarChar(2);
	Declare @Seq2_Count Int;;
	--Declare @Seq2_Add Int;
	--Declare @NewSeq2 Numeric(2);
	Declare @NewSeq2_Chr VarChar(2);
	--Declare @RefNo VarChar(36);
	--Declare @SciRefNo VarChar(30);
	--Declare @FabricType VarChar(1);
	--Declare @ShipModeID VarChar(10);
	--Declare @ColorID VarChar(6);
	--Declare @MtlLT Numeric(3,0);
	--Declare @LTDay Numeric(1,0);
	--Declare @SystemETD Date;
	--Declare @Price Numeric(14,4);
	--Declare @tmpPrice Numeric(14,4);
	--Declare @Fee Numeric(14,4);
	--Declare @FeeUnit VarChar(8);
	Declare @Qty Numeric(10,2);
	--Declare @POUnit VarChar(8);
	--Declare @StockQty Numeric(10,1);
	--Declare @SizeSpec VarChar(15);
	--Declare @Width Numeric(5, 2);
	--Declare @Special NVarChar(Max);
	--Declare @Spec NVarChar(Max);
	--Declare @SuppRefno Varchar(30);
	--Declare @BrandRefNo varchar(50);
	--Declare @BrandID VarChar(8);
	--Declare @StyleID VarChar(15);
	--Declare @SeasonID VarChar(10);
	--Declare @FactoryID VarChar(8);
	--Declare @Category VarChar(1);
	--Declare @CfmDate Date;
	--Declare @OrderTypeID VarChar(20)
	--Declare @StyleUkey BigInt;
	--Declare @GmtLT Numeric(3,0);
	--Declare @ProjectID VarChar(5);
	--Declare @ProgramID VarChar(12);
	--DECLARE @MtlTypeID varchar(20);
	--Declare @SuppColor varchar(max);
	--Declare @RefnoSpec dbo.Refno_Spec;
	--Declare @CannotOperateStock bit;

	--Select @BrandID = BrandID
	--	 , @StyleID = StyleID
	--	 , @SeasonID = SeasonID
	--	 , @FactoryID = FactoryID
	--	 , @Category = Category
	--	 , @CfmDate = CfmDate
	--	 , @ProjectID = ProjectID
	--	 , @ProgramID = ProgramID
	--	 , @OrderTypeID = OrderTypeID
	--  From dbo.Orders
	-- Where ID = @PoID;

	--DECLARE @IsThirdSchedule BIT = 0;

	--SELECT 
	--	@IsThirdSchedule = IsThirdSchedule 
	--FROM dbo.PO 
	--WHERE ID = @PoID;

	 --DECLARE @KeywordValue NVARCHAR(MAX) = ''

	 --IF EXISTS(SELECT 1 FROM Production.dbo.Brand_PODesc WHERE ID = @BrandID AND ProjectID = @ProjectID AND Category = Category)
	 --BEGIN
		--select @KeywordValue = dbo.GetKeyword_New(@PoID, 0, Keyword, '', '', '', 1) from Production.dbo.Brand_PODesc where ID = @BrandID AND ProjectID = @ProjectID AND Category = @Category
	 --END

	--Select @StyleUkey = Ukey
	--	 , @GmtLT = GmtLT
	--  From Production.dbo.Style
	-- Where BrandID = @BrandID
	--   And ID = @StyleID
	--   And SeasonID = @SeasonID;

	--Declare @Po_Supp_Detail_Seq Table
	--	(Seq1 VarChar(3), Seq2 VarChar(2));
	
	--If @AppType = 1
	--Begin
	--	Insert Into @Po_Supp_Detail_Seq
	--	Select Seq1, Seq2
	--	  From dbo.PO_Supp_Detail
	--	 Where ID = @PoID
	--	 Order by Seq1, Seq2;
	--End;
	----------------------------------------------------------------------
	Declare @NewSeq1 Numeric(3);
	Declare @NewSeq1_Chr VarChar(3);
	--Declare @NewSeq1_Prefix VarChar(1);
	--Declare @Format varchar(2);

	Declare @NewSeq1_List Table (Seq1 VarChar(3));
	--Declare @ImportPort VarCHar(20);
	--Declare @FactoryCountry VarCHar(2);
	--Declare @SuppCountry VarCHar(2);
	Declare @LockDate Date;

	--Select @FactoryCountry = CountryID
	--	 , @ImportPort = PortSea
	--  From Production.dbo.Factory
	-- Where ID = @FactoryID;
	
	Declare @tmpPo_SuppRowID Int;		--Row ID
	Declare @tmpPo_SuppRowCount Int;	--總資料筆數

	Declare @tmpPo_Supp_DetailRowID Int;	--Row ID
	Declare @tmpPo_Supp_DetailRowCount Int;	--總資料筆數
	--------------------Loop Start #tmpPO_Supp--------------------
	Set @tmpPo_SuppRowID = 1;
	Select @tmpPo_SuppRowID = Min(RowID), @tmpPo_SuppRowCount = Max(RowID) From #tmpPO_Supp;
	While @tmpPo_SuppRowID <= @tmpPo_SuppRowCount
	Begin
		Select @Seq1 = Seq1
			 , @SuppID = SuppID
			 --, @Description = Description
		  From #tmpPO_Supp
		 Where RowID = @tmpPo_SuppRowID;
		
		Set @LockDate = Null;
		Select
             --@ShipModeID = dbo.GetFactoryDefaultShip(@FactoryID, Supp.ID)
			 --, @ShipTermID = newSt.ShipTermID
			 --, @PayTermAPID = Production.dbo.GetSuppPaymentTerm(@BrandID, @SuppID, @Category)
			 --, @SuppCountry = Supp.CountryID
			 @LockDate = Supp.LockDate
			 --, @CompanyID = newSt.CompanyID
		  From Production.dbo.Supp
		  --OUTER APPLY (select * from dbo.GetReplaceSupp_ShipTerm(Supp.ID, @Category, @FactoryCountry, @FactoryID)) newSt
		 Where ID = @SuppID;
		
		--Set @ShipModeID = IIF(IsNull(@ShipModeID, '') = '', 'SEA', @ShipModeID);	--若Supplier基本檔未設定ShipMode，預設為SEA

		If @LockDate Is Not Null	--已Lock不轉
		Begin
			Delete From #tmpPO_Supp Where ID = @PoID And Seq1 = @Seq1;
			Set @tmpPo_SuppRowID += 1;
			Continue;
		End;
		-------------------------------------
		--If @AppType = 1
		--Begin
		--	Set @Seq2_Add = 0;
		--	Select @NewSeq2_Chr = IsNull(Max(Seq2), '0')
		--	  From @Po_Supp_Detail_Seq
		--	 Where Seq1 = @NewSeq1_Chr;
		--	Set @NewSeq2 = Convert(Numeric(2), @NewSeq2_Chr);
		--End;
		--Else
		--Begin
		--	Set @Seq2_Add = 0;
		--End;
		-------------------------------------
		Delete From @NewSeq1_List;
		--------------------Loop Start #tmpPO_Supp_Detail--------------------
		Set @tmpPo_Supp_DetailRowID = 1;
		Select @tmpPo_Supp_DetailRowID = Min(RowID), @tmpPo_Supp_DetailRowCount = Max(RowID) From #tmpPO_Supp_Detail Where ID = @PoID And Seq1 = @Seq1;
		While @tmpPo_Supp_DetailRowID <= @tmpPo_Supp_DetailRowCount
		Begin
			Select
                 --@Seq2 = #tmpPO_Supp_Detail.Seq2
				  @Seq2_Count = #tmpPO_Supp_Detail.Seq2_Count
				 --, @RefNo = RefNo
				 --, @SciRefNo = SCIRefNo
				 --, @FabricType = FabricType
				 --, @ColorID = isnull(sColor.SpecValue, '')
				 , @Qty = trueQty
				 --, @POUnit = POUnit
				 --, @SizeSpec = isnull(sSize.SpecValue, '')
				 --, @Width = Width
				 --, @Special = Special
				 --, @Spec = Spec
				 --, @SuppColor = SuppColor
			  From #tmpPO_Supp_Detail
			 -- left join #tmpPO_Supp_Detail_Spec sColor
				--on sColor.ID = #tmpPO_Supp_Detail.ID and sColor.Seq1 = #tmpPO_Supp_Detail.Seq1 and sColor.Seq2_Count = #tmpPO_Supp_Detail.Seq2_Count and sColor.SpecColumnID = 'Color'
			 -- left join #tmpPO_Supp_Detail_Spec sSize
				--on sSize.ID = #tmpPO_Supp_Detail.ID and sSize.Seq1 = #tmpPO_Supp_Detail.Seq1 and sSize.Seq2_Count = #tmpPO_Supp_Detail.Seq2_Count and sSize.SpecColumnID = 'Size'
				--2019.05.09 加入下限值判斷
				outer apply (select fb.MtltypeId from Production.dbo.Fabric fb where fb.SCIRefno = #tmpPO_Supp_Detail.SCIRefno) fb
				outer apply (select top 1 LimitDown from Production.dbo.MtlType_Limit ml where ml.Id = fb.MtlTypeID and ml.PoUnit = #tmpPO_Supp_Detail.POUnit) ml
				outer apply (select trueQty = iif(#tmpPO_Supp_Detail.Qty < ml.LimitDown, ml.LimitDown, #tmpPO_Supp_Detail.Qty)) tq
			 Where #tmpPO_Supp_Detail.RowID = @tmpPo_Supp_DetailRowID
			   And #tmpPO_Supp_Detail.ID = @PoID
			   And #tmpPO_Supp_Detail.Seq1 = @Seq1;
			
			--Select @MtlTypeID = MtltypeID, @CannotOperateStock = CannotOperateStock From Production.dbo.Fabric Where SCIRefno = @SCIRefNo;

			If @@RowCount > 0
			Begin
				--取得物料LeadTime
				--Set @MtlLT = Production.dbo.GetMtlLT(@SciRefNo, @Category, @SuppID, @StyleID, @BrandID, @SeasonID, @FactoryCountry, @OrderTypeID, @ColorID);

				--Select @LTDay = Fabric_Supp.LTDay
				--, @SuppRefno = Fabric_Supp.SuppRefno
				--, @BrandRefNo = Fabric.BrandRefNo
				--From Production.dbo.Fabric_Supp
				--Left join Production.dbo.Fabric on Fabric_Supp.SCIRefno = Fabric.SCIRefno
				--Where Fabric_Supp.SCIRefno = @SciRefNo
				--And Fabric_Supp.SuppID = @SuppID;
				
				--IF @IsThirdSchedule = 1
				--BEGIN
				--	DECLARE @eta DATETIME = (SELECT TOP 1 MinKPILETA FROM dbo.GetSCI_FromTrade(@PoID, @Category));
				--	Set @SystemETD = (SELECT TOP 1 * from Production.dbo.GetThirdSchedule(@FactoryID, @SuppID, @ShipModeID, @ShipTermID, @eta) ORDER BY LoadDate ASC);				END
				--ELSE
				--BEGIN
				--	If @LTDay = 2
				--	Begin
				--		Set @SystemETD = Production.dbo.GetWorkDay(@CfmDate, @MtlLT);
				--	End;
				--	Else
				--	Begin
				--		Set @SystemETD = DateAdd(dd, @MtlLT, @CfmDate);
				--	End;
				--END
				--------------------------------------------------------
				--2017.11.24 add by Ben, 調整大小項編號方式
				Select @NewSeq1_Chr = Seq1
					 , @NewSeq2_Chr = Seq2
				  From Production.dbo.GetPOSeq(@Seq1, @Seq2_Count /*+ @Seq2_Add*/);
				
				If Not Exists (Select 1 From #tmpPO_Supp Where ID = @PoID And Seq1 = @NewSeq1_Chr)
				Begin
					If Not Exists (Select 1 From @NewSeq1_List Where Seq1 = @NewSeq1_Chr)
					Begin
						Insert Into @NewSeq1_List (Seq1) Values (@NewSeq1_Chr);
					End;
				End;
				--------------------------------------------------------
				--重新抓單價
				--Set @tmpPrice = Production.dbo.GetPriceFromMtl(@SciRefNo, @SuppID,@SeasonID, @Qty, @Category, @CfmDate, @SizeSpec, @ColorID, @FactoryID);
				--Set @FeeUnit = ''
				--Set @Fee = 0
				--Select @FeeUnit = IsNull(UnitID, ''), @Fee = IsNull(Fee, 0) 
				--From Production.dbo.Supp_AdditionalFee 
				--Where SuppID = @SuppID And Destination = @FactoryCountry

				--If(@FeeUnit <> '')
				--Begin
				--	Set @Price = @tmpPrice + (Production.dbo.GetUnitQty(@FeeUnit, @POUnit, 1) * @Fee)
				--End
				--Else
				--Begin
				--	Set @Price = @tmpPrice
				--End
				--------------------------------------------------------
				--重新抓庫存
				--If SubString(@Seq1, 1, 1) != 'A'	--只有A大項不需計算庫存
				--Begin
				--	Set @StockQty = 0;
				--	DECLARE @ByProjectRegion bit = IIF(@PoID LIKE 'A%', 1, 0);
					
				--	delete from @RefnoSpec
				--	insert into @RefnoSpec (SpecColumnID, SpecValue, BomType)
				--	select SpecColumnID, SpecValue, cast(1 as bit)
				--	from #tmpPO_Supp_Detail_Spec
				--	Where ID = @PoID
				--	   And Seq1 = @Seq1
				--	   And Seq2_Count = @Seq2_Count;

				--	Set @StockQty = dbo.GetStockQty('', @StyleID, @FactoryID, @ProjectID, @ProgramID, @BrandID, @OrderTypeID,
				--		@RefNo, @Width, @FabricType, @Category, @MtlTypeID, @PoID, @ByProjectRegion, @SuppRefno, @SuppColor, @BrandRefNo, @RefnoSpec);

				--End;
				--------------------------------------------------------
				--更新Temp Table - PO_Supp_Detail
				Update #tmpPO_Supp_Detail
				   Set Seq1 = @NewSeq1_Chr
					 , Seq2 = @NewSeq2_Chr
					 --, ShipModeID = @ShipModeID
					 --, SystemETD = @SystemETD
					 --, FinalETD = @SystemETD
					 --, Price = @Price
					 --, StockQty = isnull(@StockQty, 0)
					 , Qty = @Qty
					 --, CannotOperateStock = @CannotOperateStock
				 Where ID = @PoID
				   And Seq1 = @Seq1
				   --And Seq2 = @Seq2
				   And Seq2_Count = @Seq2_Count;
				
				Update #tmpPO_Supp_Detail_OrderList
				   Set Seq1 = @NewSeq1_Chr
					 , Seq2 = @NewSeq2_Chr
				 Where ID = @PoID
				   And Seq1 = @Seq1
				   --And Seq2 = @Seq2
				   And Seq2_Count = @Seq2_Count;

				Update #tmpPO_Supp_Detail_Spec
					Set Seq1 = @NewSeq1_Chr
					, Seq2 = @NewSeq2_Chr
				Where ID = @PoID
					And Seq1 = @Seq1
					And Seq2_Count = @Seq2_Count;

				Update #tmpPO_Supp_Detail_Keyword
				   Set Seq1 = @NewSeq1_Chr
					 , Seq2 = @NewSeq2_Chr
				 Where ID = @PoID
				   And Seq1 = @Seq1
				   And Seq2_Count = @Seq2_Count;
			End;
			Set @tmpPo_Supp_DetailRowID += 1;
		End;
		--------------------Loop End #tmpPO_Supp_Detail--------------------
		-------------------------------------
		--取得Supplier Remaek
		--Exec Production.dbo.GetSuppRemark @PoID, @SuppID, @BrandID, @FactoryCountry, @FactoryID, @FabricType, 'P', @Remark Output;
		-------------------------------------
		--更新Temp Table - PO_Supp
		--Update #tmpPO_Supp
		--   Set ShipTermID = @ShipTermID
		--	 , PayTermAPID = @PayTermAPID
		--	 , CompanyID = @CompanyID
		--	 --, Remark = @Remark
		--	 , Description = @KeywordValue + @Description
		-- Where ID = @PoID
		-- And Seq1 = @Seq1;
		
		Insert Into #tmpPO_Supp
			(ID, Seq1, SuppID
            --, ShipTermID, PayTermAPID
            --, Remark
            --, Description, CompanyID
            )
			Select #tmpPO_Supp.ID, NewSeq1_List.Seq1, #tmpPO_Supp.SuppID
                --, #tmpPO_Supp.ShipTermID, #tmpPO_Supp.PayTermAPID
				--, #tmpPO_Supp.Remark
                --, #tmpPO_Supp.Description, #tmpPO_Supp.CompanyID
			  From @NewSeq1_List as NewSeq1_List
			 Inner Join #tmpPO_Supp
				On #tmpPO_Supp.ID = @POID And #tmpPO_Supp.Seq1 = @Seq1
		-------------------------------------

		Set @tmpPo_SuppRowID += 1;
	End;
	--------------------Loop End #tmpPO_Supp--------------------
	Delete From #tmpPO_Supp_Detail 
	 Where Not Exists (Select ID, Seq1
						From #tmpPO_Supp
					   Where #tmpPO_Supp.ID = #tmpPO_Supp_Detail.ID
					     And #tmpPO_Supp.Seq1 = #tmpPO_Supp_Detail.Seq1
					  );
	Delete From #tmpPO_Supp
	 Where Not Exists (Select ID, Seq1
						From #tmpPO_Supp_Detail
					   Where #tmpPO_Supp_Detail.ID = #tmpPO_Supp.ID
					     And #tmpPO_Supp_Detail.Seq1 = #tmpPO_Supp.Seq1
					  );
End
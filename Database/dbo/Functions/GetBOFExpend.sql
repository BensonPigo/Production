

CREATE function [dbo].[GetBOFExpend]
(
	  @ID				VarChar(13)				--採購母單
	 ,@FabricCode		varchar(3) = null
	 ,@IsExpendArticle	Bit			= 0			--add by Edward 是否展開至Article，For U.A轉單
)
RETURNS @Tmp_BofExpend table ( ExpendUkey BigInt Identity(1,1) Not Null, ID Varchar(13), Order_BOFUkey BigInt
	 , ColorID VarChar(6), SuppColor NVarChar(Max)
	 , OrderQty Numeric(10,4), Price Numeric(9,4), UsageQty Numeric(9,2), UsageUnit VarChar(8)
	 , Width Numeric(5,2), SysUsageQty Numeric(9,2), QTFabricPanelCode NVarchar(100), Remark NVarchar(max), OrderList NVarchar(max), ColorDesc varchar(150)
	 , Special VarChar(Max)
	 , Primary Key (ExpendUKey)
	 , Index Idx_ID NonClustered (ID, Order_BOFUkey, ColorID) -- table index
	)
As
begin
	
	--取得訂單基本資料:Brand, Style, Season, Program, Category, Cfm. Date
	Declare @BrandID VarChar(8);
	Declare @StyleID VarChar(15);
	Declare @SeasonID VarChar(10);
	Declare @FactoryID VarChar(8);
	Declare @Category VarChar(1);
	Declare @ProgramID VarChar(12);
	Declare @CfmDate Date;
	Declare @CalByBOFConsumption bit = 0;

	Select @BrandID = Orders.BrandID
		, @StyleID = Orders.StyleID
		, @SeasonID = Orders.SeasonID
		, @FactoryID = Orders.FactoryID
		, @ProgramID = Orders.ProgramID
		, @Category = Orders.Category
		, @CfmDate = Orders.CfmDate
		, @CalByBOFConsumption = isnull(OrderType.CalByBOFConsumption, 0)
	From dbo.Orders
	left join Production.dbo.OrderType on Orders.OrderTypeID = OrderType.ID and Orders.BrandID = OrderType.BrandID
	Where Orders.ID = @ID;
	
	--計算總件數
	Declare @TotalQty Numeric(8,0);
	
	Set @TotalQty = 0;
	Select @TotalQty = Sum(Qty)
	From dbo.Order_Qty
	Where Exists (Select 1 From dbo.Orders Where Orders.ID = Order_Qty.ID And Orders.PoID = @ID);
	
	--定義欄位
	Declare @Order_BOFUkey BigInt;
	Declare @ColorID Varchar(6);
	Declare @ColorDesc varchar(90);
	Declare @Special Varchar(max);
	Declare @SuppColor NVarchar(Max);
	Declare @OrderQty Numeric(10,4);
	Declare @Price Numeric(9,4);
	Declare @UsageQty Numeric(9,2);
	Declare @UsageUnit Varchar(8);
	Declare @Width Numeric(5,2);
	Declare @SysUsageQty Numeric(9,2);
	Declare @QTFabricPanelCode NVarchar(100);
	Declare @Remark NVarchar(max);
	Declare @ConsPC Numeric(12, 4);
	Declare @OrderList VarChar(Max);
	Declare @BofUkey BigInt;
	Declare @BofFabricCode VarChar(3);
	Declare @BofSciRefNo VarChar(30);
	Declare @BofSuppID VarChar(6);
	Declare @BofCursor Table
		( RowID BigInt Identity(1,1) Not Null, Ukey BigInt, FabricCode VarChar(3)
			, SciRefNo VarChar(30), SuppID VarChar(6), Remark NVarchar(Max)
		);
	Declare @BofRowID Int;		--BOF ID
	Declare @BofRowCount Int;	--BOF總資料筆數
	
	Insert Into @BofCursor
		(Ukey, FabricCode, SCIRefNo, SuppID, Remark)
		Select Ukey, Order_BOF.FabricCode, SCIRefNo, SuppID, Remark From dbo.Order_BOF
		Where Order_BOF.ID = @ID
			And ( IsNull(@FabricCode,'') = ''
				Or Order_BOF.FabricCode = @FabricCode
			)
		 Order by FabricCode;
		 
	if (@CalByBOFConsumption = 0)
	BEGIN
		--CalByBOFConsumption為0，走原本的規則

		Declare @EachCons_ColorUkey BigInt;
		Declare @EachCons_ColorCursor Table
			( RowID BigInt Identity(1,1) Not Null, Ukey BigInt, ColorID Varchar(6)
				, UsageQty Numeric(9,2), OrderQty Numeric(10,4), OrderList NVarchar(max)
				, Special Varchar(8)
			);
		Declare @EachCons_ColorRowID Int;		--EachCons_Color ID
		Declare @EachCons_ColorRowCount Int;	--EachCons_Color總資料筆數
	
		--取得此單的所有SP
		-- 2017.09.30 modify by Ben, 排除Junk以及Qty = 0
		--declare @OrderList_Full varchar(max) = (select ID+',' from Orders where POID = @ID order by ID for xml path(''))
		declare @OrderList_Full varchar(max) = (select ID+',' from Orders where POID = @ID And Junk = 0 And Qty > 0 order by ID for xml path(''))

		Set @BofRowID = 1;
		--Select @BofRowCount = Count(*) From @BofCursor;
		Select @BofRowID = Min(RowID), @BofRowCount = Max(RowID) From @BofCursor;
		While @BofRowID <= @BofRowCount
		Begin
			Select @BofUkey = Ukey
				, @BofFabricCode = FabricCode
				, @BofSciRefNo = SCIRefNo
				, @BofSuppID = SuppID
				, @Remark = Remark 
			From @BofCursor
			Where RowID = @BofRowID;
			
			--取得物料基本資料:UsageUnit, Width
			Set @UsageUnit = '';
			Set @Width = 0;
			Select @UsageUnit = Fabric.UsageUnit
				, @Width = Fabric.Width
			From production.dbo.Fabric
			Where SCIRefno = @BofSciRefNo;
		
			Delete From @EachCons_ColorCursor;
			/*	-- 2017.09.30 mark by Ben
			with o as(
				Select Order_Qty.Article, Order_Qty.SizeCode, Orders.ID
					From dbo.Orders
					left join dbo.Order_Qty on Orders.ID = Order_Qty.ID
					where POID = @ID
			)
			*/
			Insert Into @EachCons_ColorCursor
				(Ukey, ColorID, UsageQty, OrderQty/*, OrderList*/, Special)
				Select Order_EachCons_Color.Ukey, Order_EachCons_Color.ColorID
					--, Order_EachCons_Color.Yds
					--, Order_EachCons_Color.OrderQty
					, IIF(@IsExpendArticle = 0, sum(Order_EachCons_Color.Yds), sum(Order_EachCons.ConsPC * Order_EachCons_Color_Article.CutQty))
					, IIF(@IsExpendArticle = 0, sum(Order_EachCons_Color.OrderQty), sum(Order_EachCons_Color_Article.OrderQty))
					--, ol.OrderList
					, Special
				From dbo.Order_EachCons
				Left Join dbo.Order_EachCons_Color
					On Order_EachCons_Color.Order_EachConsUkey = Order_EachCons.Ukey
				Left join dbo.Order_EachCons_Color_Article
					on Order_EachCons_Color.Ukey = Order_EachCons_Color_Article.Order_EachCons_ColorUkey
					and @IsExpendArticle = 1
				Outer apply ( select Special = IIF(@IsExpendArticle = 0, '', Order_EachCons_Color_Article.Article) ) spc
				/*  -- 2017.09.30 mark by Ben
				cross apply (select OrderList = (select o.ID + ','
					from o where exists(select 1 from Order_EachCons_Color_Article oeca 
						where oeca.Order_EachCons_ColorUkey = Order_EachCons_Color.Ukey
							And oeca.ColorID = Order_EachCons_Color.ColorID
							and oeca.Article = o.Article and oeca.SizeCode = o.SizeCode ) 
					group by o.ID for xml path('')) ) ol
				*/
				Where Order_EachCons.ID = @ID
					And Order_EachCons.FabricCode = @BofFabricCode
				group by 
					Order_EachCons_Color.Ukey, 
					Order_EachCons_Color.ColorID, 
					Special
				Order by Order_EachCons_Color.ColorID/*, len(orderlist)*/ desc;
		
			Set @EachCons_ColorRowID = 1;
			--Select @EachCons_ColorRowCount = Count(*) From @EachCons_ColorCursor;
			Select @EachCons_ColorRowID = Min(RowID), @EachCons_ColorRowCount = Max(RowID) From @EachCons_ColorCursor;
			While @EachCons_ColorRowID <= @EachCons_ColorRowCount
			Begin
				Select @EachCons_ColorUkey = Ukey
					, @ColorID = ColorID
					, @UsageQty = UsageQty
					, @OrderQty = OrderQty
					, @OrderList = OrderList
					, @Special = Special
				From @EachCons_ColorCursor
				Where RowID = @EachCons_ColorRowID

				Set @SysUsageQty = @UsageQty;

				--若資料存在則累加UsageQty, SysUsageQty, OrderQty
				Update @Tmp_BofExpend Set UsageQty += @UsageQty
					, SysUsageQty += @SysUsageQty
					, OrderQty += @OrderQty
				Where Order_BOFUkey = @BofUkey And ColorID = @ColorID And Special = @Special;
				
				If @@RowCount = 0
				Begin
					------------------------------------------------------------------
					-- 2017.09.30 modify by Ben
					Set @OrderList = '';
					--Set @OrderList = IsNull((Select distinct SUBSTRING(tmpOrder.ID,9,LEN(tmpOrder.ID)) + '/' 
					Set @OrderList = IsNull((Select distinct tmpOrder.ID + ',' 
												From dbo.Order_EachCons
												Left Join dbo.Order_EachCons_Color
													On Order_EachCons_Color.Order_EachConsUkey = Order_EachCons.Ukey
												Left Join dbo.Order_EachCons_Color_Article
													On Order_EachCons_Color_Article.Order_EachCons_ColorUkey = Order_EachCons_Color.Ukey
												Left Join (Select Orders.PoID, Order_Qty.Article, Order_Qty.ID
													From dbo.Orders
													Left Join dbo.Order_Qty
														On Order_Qty.ID = Orders.ID
															----------------------------------
															-- 2017.09.30 modify by Ben, 排除Junk以及Qty = 0
															And dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1
															And Order_Qty.Qty > 0
															----------------------------------
													Group by Orders.PoID, Order_Qty.Article, Order_Qty.ID
													) as tmpOrder
												On tmpOrder.PoID = Order_EachCons.ID
													And tmpOrder.Article = Order_EachCons_Color_Article.Article
												Where Order_EachCons.ID = @ID
												And Order_EachCons.FabricCode = @BofFabricCode
												And Order_EachCons_Color.ColorID = @ColorID
												Group by tmpOrder.ID
												for xml path('')), '');
					------------------------------------------------------------------
				
					--取得 Supplier Color
					Set @SuppColor = IsNull(production.dbo.GetSuppColorList(@BofSciRefNo, @BofSuppID, @ColorID, @BrandID, @SeasonID, @ProgramID, @StyleID), '');
					
					--取得 Fabric Price
					--Set @Price = IsNull(dbo.GetPriceFromMtl(@BofSciRefNo, @BofSuppID, @SeasonID, @UsageQty, @Category, @CfmDate, '', @ColorID), 0);

					set @ColorDesc = (select Color.Name from production.dbo.Color where BrandId = @BrandID and Color.ID = @ColorID)
					
					Insert Into @Tmp_BofExpend
						( ID, Order_BOFUkey, ColorID, SuppColor, UsageUnit, Width, Remark
							, UsageQty, SysUsageQty, OrderQty, Price, OrderList, ColorDesc
							, Special
						)
					Values
						( @ID, @BofUkey, @ColorID, @SuppColor, @UsageUnit, @Width, @Remark
							, @UsageQty, @SysUsageQty, @OrderQty, @Price, @OrderList, @ColorDesc
							, @Special
						);
						
					--Update OrderList 包含整組的sp，改為空白
					update @Tmp_BofExpend
						set OrderList = ''
					where OrderList = @OrderList_Full
				
				End;

				Set @EachCons_ColorRowID += 1;
			End;

			Set @BofRowID += 1;
		End;

	END
	ELSE
	BEGIN
		--走Consumption依照BOF的Size, Color展開計算
		declare @TmpTbl table ( POID Varchar(13), Order_BOFUkey BigInt
			 , ColorID VarChar(6), SuppColor NVarChar(Max)
			 , OrderQty Numeric(10,4), Price Numeric(9,4), UsageQty Numeric(9,2), UsageUnit VarChar(8)
			 , Width Numeric(5,2), SysUsageQty Numeric(9,2), QTFabricPanelCode NVarchar(100), Remark NVarchar(max), OrderList NVarchar(max), ColorDesc varchar(150)
			 , Special VarChar(Max)
			 , ID VarChar(13)
			)
		--為了組OrderList先暫存至Tmp資料表
		insert into @TmpTbl ( POID, Order_BOFUkey, ColorID, SuppColor, UsageUnit, Width, Remark
			, UsageQty, SysUsageQty, OrderQty, /*Price,*/ OrderList, ColorDesc
			, Special, ID)
		select @ID, bof.Ukey, oc.ColorID, SuppColor, UsageUnit, Width, bof.Remark
			, usageQty, usageQty, oq.Qty, /*Price, */'' as OrderList, ColorDesc
			, '' as Special, o.ID
		from @BofCursor bof
		left join dbo.Orders o on 1 = 1
		left join dbo.Order_Article oa on o.ID = oa.id
		left join dbo.Order_ColorCombo oc on o.POID = oc.Id and oa.Article = oc.Article and bof.FabricCode = oc.FabricCode
		left join dbo.Order_Qty oq on o.ID = oq.ID and oa.Article = oq.Article
		left join dbo.Order_FabricCode ofabric on oc.Id = ofabric.Id and oc.FabricPanelCode = ofabric.FabricPanelCode
		outer apply (
			select usageQty = oq.Qty * ofabric.ConsPC
		) outPly
		outer apply (
			select 
			SuppColor = IsNull(Production.dbo.GetSuppColorList(bof.SciRefNo, bof.SuppID, oc.ColorID, o.BrandID, o.SeasonID, o.ProgramID, o.StyleID), '')
			--, Price = IsNull(dbo.GetPriceFromMtl(bof.SciRefNo, bof.SuppID, o.SeasonID, outPly.usageQty, o.Category, o.CFMDate, '', oc.ColorID, o.FactoryID), 0)
			, ColorDesc = (select Color.Name from production.dbo.Color where BrandId = o.BrandID and Color.ID = oc.ColorID)
		) outPly1
		outer apply (Select UsageUnit = Fabric.UsageUnit, Width = Fabric.Width From production.dbo.Fabric Where SCIRefno = bof.SciRefNo) f
		where o.POID = @ID and oc.FabricType = 'F'
		

		Insert Into @Tmp_BofExpend
		( ID, Order_BOFUkey, ColorID, SuppColor, UsageUnit, Width, Remark
			, UsageQty, SysUsageQty, OrderQty, Price, OrderList, ColorDesc
			, Special
		)
		select POID, Order_BOFUkey, ColorID, SuppColor, UsageUnit, Width, Remark
				, UsageQty, SysUsageQty, OrderQty, Price, OrderList, ColorDesc
				, Special
		from (
			select POID, Order_BOFUkey, ColorID, SuppColor, UsageUnit, Width, Remark
				, sum(UsageQty) as UsageQty, sum(SysUsageQty) as SysUsageQty, sum(OrderQty) as OrderQty, Price, ColorDesc
				, Special
			from @TmpTbl
			group by POID, Order_BOFUkey, ColorID, SuppColor, UsageUnit, Width, Remark, Price, ColorDesc, Special
		) main
		outer apply (
			--設定OrderList
			select OrderList = stuff((select DISTINCT ',' + ID  from @TmpTbl tmp 
				where tmp.Order_BOFUkey = tmp.Order_BOFUkey and tmp.ColorID = main.ColorID for xml path('')), 1, 1, '')
		) ol

		set @TotalQty = (select sum(OrderQty) from @Tmp_BofExpend);
	END

	
	--批次更新 Fabric Price
	If @TotalQty > 0
	Begin
		Update @Tmp_BofExpend
			set OrderQty = UsageQty / @TotalQty
		From @Tmp_BofExpend as bf
		inner Join @BofCursor as Bof
		on Bof.Ukey = bf.Order_BOFUkey
	End;
				
	return ;
end

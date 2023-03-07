
CREATE Procedure [dbo].[BoaExpend]
(
	  @ID				VarChar(13)				--採購母單
	 ,@Order_BOAUkey	BigInt		= 0			--BOA Ukey
	 ,@TestType			Int			= 0			--是否為虛擬庫存計算(0: 實際寫入Table; 1: 僅傳出Temp Table; 2: 不回傳Temp Table; 3: 實際寫入Table，但不回傳Temp Table)
	 ,@UserID			VarChar(10) = ''
	 ,@IsGetFabQuot		Bit			= 1
	 ,@IsExpendDetail	Bit			= 0			--是否一律展開至最詳細
	 ,@IsExpendArticle	Bit			= 0			--add by Edward 是否展開至Article，For U.A轉單
	 ,@IncludeQtyZero	Bit			= 0			--add by Edward 是否包含數量為0
)
As
Begin
	Set NoCount On;

	If Object_ID('tempdb..#Tmp_BoaExpend') Is Null
	Begin
		Create Table #Tmp_BoaExpend
			(  ExpendUkey BigInt Identity(1,1) Not Null, ID Varchar(13), Order_BOAUkey BigInt
			 , RefNo VarChar(20), SCIRefNo VarChar(30), Article VarChar(8), ColorID VarChar(6), SuppColor NVarChar(Max)
			 , SizeCode VarChar(8), SizeSpec VarChar(15), SizeUnit VarChar(8), Remark NVarChar(Max)
			 , OrderQty Numeric(6,0)
			 --, Price Numeric(12,4)
			 , UsageQty Numeric(11,2), UsageUnit VarChar(8), SysUsageQty  Numeric(11,2)
			 , BomZipperInsert VarChar(5), BomCustPONo VarChar(30), Keyword VarChar(Max), OrderList nvarchar(max), ColorDesc nvarchar(150)
			 , Special nvarchar(max)
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
	
	--#Tmp_Order_Qty 轉存至 @Tmp_Order_Qty，順便排除不必要欄位
	declare @Tmp_Order_Qty dbo.QtyBreakdown
	insert into @Tmp_Order_Qty
	select ID ,Article ,SizeCode ,Qty ,OriQty from #Tmp_Order_Qty

	--#Tmp_BoaExpend
	Insert Into #Tmp_BoaExpend(ID ,Order_BOAUkey ,RefNo ,SCIRefNo ,Article ,ColorID ,SuppColor ,SizeCode
		,SizeSpec ,SizeUnit ,Remark ,OrderQty
		--,Price 
		,UsageQty ,UsageUnit ,SysUsageQty ,BomZipperInsert
		,BomCustPONo ,Keyword ,OrderList ,ColorDesc, Special)
		select ID ,Order_BOAUkey ,RefNo ,SCIRefNo ,Article ,ColorID ,SuppColor ,SizeCode
			,SizeSpec ,SizeUnit ,Remark ,OrderQty
			--,Price
			,UsageQty ,UsageUnit ,SysUsageQty ,BomZipperInsert
			,BomCustPONo ,Keyword
			,OrderList
			,ColorDesc, Special
		from GetBOAExpend(@ID, @Order_BOAUkey, @IsGetFabQuot, @IsExpendDetail, @Tmp_Order_Qty, @IsExpendArticle, @IncludeQtyZero)
		order by RefNo, SCIRefNo, ColorID, Article, SizeSeq, SizeSpec, BomZipperInsert, Keyword, Special
	
	--#Tmp_BoaExpend_OrderList
	Insert Into #Tmp_BoaExpend_OrderList (ID, ExpendUkey, OrderID)
		Select ID, ExpendUkey, tmp.Data
		From #Tmp_BoaExpend
		Cross Apply (Select * From dbo.SplitString(#Tmp_BoaExpend.OrderList, ',') where Data <> '') as tmp		

	If (@TestType <> 2) And (@TestType <> 3)
	Begin
		Select * From #Tmp_BoaExpend;
		Select * From #Tmp_BoaExpend_OrderList;
	End;
	
	--若@TestType = 0，表示需實際寫入Table
	--If @TestType = 0 Or @TestType = 3
	--Begin
	--	Exec BoaExpend_Insert @ID, @Order_BOAUkey, @UserID;
	--End;

	Drop Table #Tmp_BoaExpend_OrderList;
End
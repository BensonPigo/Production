
Create Procedure [dbo].[BoaExpend_Insert]
	-- Add the parameters for the stored procedure here
	(
	  @ID				VarChar(13)				--採購母單
	 ,@Order_BOAUkey	BigInt		= 0			--BOA Ukey
	 ,@UserID			VarChar(10) = ''
	)
As
Begin
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	Set NoCount On;

	Begin Try
		Begin Transaction

		--刪除 Bill of Accessory Expend
		Delete From dbo.Order_BOA_Expend
		 Where ID = @ID
		   And (   IsNull(@Order_BOAUkey, 0) = 0
				Or Order_BOAUkey = @Order_BOAUkey
			   );
		--刪除 Bill of Accessory Expend - Order List
		Delete From dbo.Order_BOA_Expend_OrderList
		 Where Order_BOA_ExpendUkey In (Select Ukey From dbo.Order_BOA_Expend
										 Where ID = @ID
										   And (   IsNull(@Order_BOAUkey, 0) = 0
												Or Order_BOAUkey = @Order_BOAUkey
											   )
									   );
		
		Declare @Order_BOA_ExpendUkey BigInt;
		Declare @ExpendUkey BigInt;
		Declare @ExpendCursor Table
			(RowID BigInt Identity(1,1) Not Null, ExpendUkey BigInt);
		
		Declare @RowID Int;		--ID起始比數
		Declare @RowCount Int;	--總資料筆數

		Insert Into @ExpendCursor
			(ExpendUkey)
			Select ExpendUkey From #Tmp_BoaExpend;
		
		Set @RowID = 1
		--Select @RowCount = Count(*) From #Tmp_BoaExpend
		Select @RowID = Min(RowID), @RowCount = Max(RowID) From @ExpendCursor;
		While @RowID <= @RowCount
		Begin
			Select @ExpendUkey = ExpendUkey
			  From @ExpendCursor
			 Where RowID = @RowID;

			Insert Into dbo.Order_BOA_Expend
				(  ID, Order_BOAUkey, RefNo, SCIRefNo, Article, ColorID, SuppColor
				 , SizeCode, SizeSpec, SizeUnit, Remark, OrderQty, Price, UsageQty
				 , UsageUnit, SysUsageQty, BomFactory, BomCountry, BomStyle, BomCustCD
				 , BomArticle, BomZipperInsert, BomBuymonth, BomCustPONo
				 , AddName, AddDate
				)
				Select ID, Order_BOAUkey, RefNo, SCIRefNo, Article, ColorID, SuppColor
					 , SizeCode, SizeSpec, SizeUnit, Remark, OrderQty, Price, UsageQty
					 , UsageUnit, SysUsageQty, BomFactory, BomCountry, BomStyle, BomCustCD
					 , BomArticle, BomZipperInsert, BomBuymonth, BomCustPONo
					 , @UserID AddName, GetDate() AddDate
				  From #Tmp_BoaExpend
				 Where ExpendUkey = @ExpendUkey;
			
			Set @Order_BOA_ExpendUkey = @@IDENTITY;

			Insert Into dbo.Order_BOA_Expend_OrderList
				(ID, Order_BOA_ExpendUkey, OrderID, AddName, AddDate)
				Select ID, @Order_BOA_ExpendUkey, OrderID, @UserID AddName, GetDate() AddDate
				  From #Tmp_BoaExpend_OrderList
				 Where ExpendUkey = @ExpendUkey;

			Set @RowID += 1
		End;

		Commit Transaction;
	End Try
	Begin Catch
		RollBack Transaction

		Declare @ErrorMessage NVarChar(4000);
		Declare @ErrorSeverity Int;
		Declare @ErrorState Int;

		Set @ErrorMessage = Error_Message();
		Set @ErrorSeverity = Error_Severity();
		Set @ErrorState = Error_State();

		RaisError (@ErrorMessage,	-- Message text.
				   @ErrorSeverity,	-- Severity.
				   @ErrorState		-- State.
				  );
	End Catch
	
End
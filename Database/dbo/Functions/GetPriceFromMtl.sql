
Create Function [dbo].[GetPriceFromMtl]
(
	  @SCIRefNo			VarChar(26)				--SCI Ref#
	 ,@SuppID			VarChar(6)				--Supplier ID
	 ,@SeasonID			VarChar(10)				--Season ID
	 ,@PoQty			Numeric(9,2)			--PoQty
	 ,@Category			VarChar(1)				--訂單Category
	 ,@CfmDate			DateTime				--Order Cfm. Date
	 ,@SizeSpec			VarChar(15)	= Null		--展開物料的Size
	 ,@ColorID			VarChar(6)	= Null		--展開物料的顏色
)
Returns Numeric (9,4)
As
Begin
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	--Set NoCount On;
	Declare @PoPrice Numeric (11,4);

	Declare @Ukey BigInt;
	Declare @PoPrice_Detail Numeric(11,4);

	Select Top 1 @Ukey = Fabric_Quot.Ukey
		 , @PoPrice = Fabric_Quot.POPrice
	  From dbo.Fabric_Quot
	 Where Fabric_Quot.SCIRefNo = @SCIRefNo
	   And Fabric_Quot.SuppID = @SuppID
	   And Fabric_Quot.SeasonID = @SeasonID
	   And Fabric_Quot.Category = @Category
	   And Fabric_Quot.[Status] = 'Confirmed'
	   And Fabric_Quot.CfmDate <= @CfmDate
	   And (   IsNull(@SizeSpec,'') = ''
			Or Fabric_Quot.SizeSpec = @SizeSpec
		   )
	   And (   IsNull(@ColorID,'') = ''
			Or Fabric_Quot.ColorID = @ColorID
		   )
	 Order by Fabric_Quot.CfmDate Desc;
	
	If IsNull(@PoPrice, 0) = 0
	Begin
		Select Top 1 @Ukey = Fabric_Quot.Ukey
			 , @PoPrice = Fabric_Quot.POPrice
		  From dbo.Fabric_Quot
		 Where Fabric_Quot.SCIRefNo = @SCIRefNo
		   And Fabric_Quot.SuppID = @SuppID
		   And Fabric_Quot.SeasonID = @SeasonID
		   And Fabric_Quot.Category = @Category
		   And Fabric_Quot.[Status] = 'Confirmed'
		   And Fabric_Quot.CfmDate <= @CfmDate
		 Order by Fabric_Quot.CfmDate Desc;
	End;

	Select Top 1 @PoPrice_Detail = Fabric_Quot_Detail.POPrice
	  From dbo.Fabric_Quot_Detail
	 Where Fabric_Quot_Detail.Fabric_QuotUkey = @Ukey
	   And @PoQty Between Fabric_Quot_Detail.FromQty And Fabric_Quot_Detail.ToQty;
	
	If IsNull(@PoPrice_Detail, 0) > 0
	Begin
		Set @PoPrice = @PoPrice_Detail;
	End;

	Return IsNull(@PoPrice,0);
End

Create Function [dbo].[GetProvideDate]
(		
	 @Type		VarChar(1)		-- 0. EC; 1. M/Notice
	,@OrderID	VarChar(13)
)
Returns @Tmp_Provide Table
	(
	  OrderID			VarChar(13)
	 ,Type_Brand		VarChar(50)		Default ''
	 ,Days				Numeric(2,0)				--EC: EachConsDays; M/Notice: Day
	 ,ApproveDate		Date						--EC: EachCons Approve Date; M/Notice: M/Notice Approve Date
	 ,CheckDate			Date						--EC: Check Date; M/Notice: Provide Date
	 ,CheckDiffDay		Int				Default 0	--EC: Check Diff. Days; M/Notice: Provide Diff. Date
	 ,IsDifferent		Bit				Default 0
	 ,IsPullForward		VarChar(1)		Default ''
	 ,IsError			Bit				Default 0
	 ,ErrorMessage		VarChar(Max)	Default ''
	)
As
Begin
	--Set NoCount On;
	Declare @IsError Bit;
	Declare @Days Numeric(2,0);
	Declare @CheckDate Date;
	Declare @CheckDiffDay Int;
	Declare @Type_Brand VarChar(50)
	Declare @KPIProjectID VarChar(5);
	Declare @IsPullForward VarChar(1);
	Declare @IsDifferent Bit;
	Declare @ErrorMessage VarChar(Max);

	Declare @ProvideDate Date;

	Declare @CheckDate_Old Date;
	Declare @ProvideDate_Old Date;
	
	Declare @EachConsDays Numeric(2,0);
	Declare @EachCons_ApvDate Date;
	Declare @MNotice_ApvDate Date;
	Declare @CMPQDate Date;
	
	Declare @POID VarChar(13);
	Declare @BrandID VarChar(8);
	Declare @StyleID VarChar(15);
	Declare @SeasonID VarChar(10);
	Declare @FactoryID VarChar(8);
	Declare @OrderTypeID VarChar(20);
	Declare @Category VarChar(1);
	Declare @FtyCountry VarChar(2);
	Declare @VasShas Bit;
	Declare @Verifydate Date;
	Declare @CfmDate Date;
	Declare @SciDelivery Date;
	Declare @BuyerDelivery Date;
	Declare @ProgramID VarChar(12);
	Declare @GarmentLT Numeric(3,0);
	Declare @MrTeam VarChar(5);
	Declare @IsPF Bit;
	
	Set @EachConsDays = 0;
	Set @CheckDate = Null;
	Set @CheckDiffDay = 0;
	Set @Type_Brand = '';
	Set @EachCons_ApvDate = Null;
	Set @KPIProjectID = '';
	Set @IsPF = '';
	Set @IsDifferent = 0;
	Set @IsError = 0;
	Set @ErrorMessage = '';

	Select @BrandID = Orders.BrandID
		 , @StyleID = Orders.StyleID
		 , @SeasonID = Orders.SeasonID
		 , @FactoryID = Orders.FactoryID
		 , @OrderTypeID = Orders.OrderTypeID
		 , @Category = Orders.Category
		 , @FtyCountry = IsNull(Factory.CountryID, '')
		 , @EachConsDays = IIF(Orders.Category = OrderType.Category, OrderType.EachConsDays, 0)
		 , @EachCons_ApvDate = Orders.EachConsApv
		 , @VasShas = Orders.VasShas
		 , @CfmDate = Orders.CfmDate
		 , @SciDelivery = Orders.SciDelivery
		 , @BuyerDelivery = Orders.BuyerDelivery
		 , @ProgramID = Orders.ProgramID
		 , @MNotice_ApvDate = Orders.MnorderApv
		 , @CMPQDate = Orders.CMPQDate
		 , @KPIProjectID = OrderType.KPIProjectID -- 2018/04/09 用來判斷是否需走專案單的規則(Cfm. Date + 5天)
		 , @POID = Orders.POID
	  From dbo.Orders
	  Left Join dbo.Factory
		On Factory.ID = Orders.FactoryID
	  Left Join dbo.OrderType
		On	   OrderType.BrandID = Orders.BrandID
		   And OrderType.ID = Orders.OrderTypeID
	 Where Orders.ID = @OrderID;

	Set @GarmentLT = dbo.GetStyleGMTLT(@BrandID, @StyleID, @SeasonID, @FactoryID);
	
	If @Type = '0'		--0. EC
	Begin
		If @Category In ('S','T') And @EachConsDays <= 0--2018/03/01 [IST20180148] modify by Anderson 加上Category=T判斷
		Begin
			Set @ErrorMessage = 'if Category is ''S'' or ''T'',then Ec_day must > 0';

			Insert Into @Tmp_Provide
				(OrderID, ErrorMessage)
			Values
				(@OrderID, @ErrorMessage);
			
			Return;
		End;

		--當Each cons.Approved Date為空白時，就依執行的當日為比對的標準
		If @EachCons_ApvDate Is Null
		Begin
			Set @EachCons_ApvDate = GetDate();
		End;
		
		Select @MrTeam = Brand.MrTeam From dbo.Brand Where Brand.ID = @BrandID;

		-- 2018/04/12 modi by Vicky, Each Cons 要依POID 最早的SCI DEL 抓
		-- Select Top 1 @SciDelivery = SciDelivery from dbo.Orders where POID = @POID and Category != 'S' order by SciDelivery;

		If @Category = 'B'
		Begin
			If @KPIProjectID != '' and not (@BrandID = 'U.ARMOUR' and @OrderTypeID in ('AR-RM','AR-RM-TPO'))
			Begin
				--專案單使用order cfmdate + 5天.
				Set @EachConsDays = IIF(@EachConsDays = 0, 5, @EachConsDays);
				Set @Verifydate = @CfmDate;
				Set @CheckDate = DateAdd(Day, @EachConsDays, @Verifydate);
				Set @CheckDate = dbo.CheckHolidayReturnDate(@CheckDate, @FactoryID, @FtyCountry, 0);

				If DateDiff(Day, @CfmDate, @EachCons_ApvDate) > @EachConsDays
				Begin
					Set @CheckDiffDay = DateDiff(Day, @CheckDate, @EachCons_ApvDate);
					Set @ErrorMessage = 'Is order type:CR/CC/CM/ZMII for Adidas,5 days after order cfm date';
					Set @IsError = 1;
				End;
			End;
			Else
			Begin
				Set @Verifydate = @SciDelivery;
				If @EachConsDays = 0
				Begin
					--大貨且日期為零時依規則抓
					If @ProgramID = 'SMU-USA'	--BULK中只有此項是抓Buyer Delivery
					Begin
						Set @EachConsDays = 30;
						Set @Verifydate = @BuyerDelivery;
						Set @Type_Brand = @ProgramID;
					End;
					Else
					Begin
						Set @IsPF = dbo.CheckIsPullForward(@BuyerDelivery, @SciDelivery);
						Set @IsPullForward = IIF(@IsPF = 1, 'Y', '');						
						Set @Type_Brand = @BrandID + 'GL:' + LTrim(Str(@GarmentLT)) + IIF(@IsPF = 1, 'PF:Y', '');
						Set @EachConsDays = 30;

						If @VasShas = 1 and @BrandID in ('ADIDAS', 'REEBOK')
						Begin
							Set @Type_Brand = 'VAS/SHAS';
							Set @EachConsDays = 34;
							Set @ErrorMessage = 'VAS/SHA order:for Adidas,34 days before SCI delivery';
						End;
						Else If @GarmentLT <= 30 and DateDiff(Day, @CfmDate, @BuyerDelivery) <= 40 and @BrandID in ('ADIDAS', 'REEBOK')
						Begin
							Set @EachConsDays = -5;
							Set @Verifydate = @CfmDate;
							Set @ErrorMessage = 'Is short lead time order: GL ≦ 30 days style,5 days after order cfm date';
						End;
						Else If @GarmentLT = 30 and @BrandID in ('ADIDAS', 'REEBOK')
						Begin
							Set @EachConsDays = 30;
							Set @ErrorMessage = 'order:for Adidas,30 days before SCI delivery';
						End;
						Else If @GarmentLT between 31 and 75 and @BrandID in ('ADIDAS', 'REEBOK')
						Begin
							Set @ErrorMessage = 'Is short lead time order: GL ≦ 75~31 days style,30 days before SCI delivery';
						End;
						Else If @IsPF = 1 or (@BrandID = 'U.ARMOUR' and @OrderTypeID in ('AR-RM','AR-RM-TPO'))
						Begin
							Set @ErrorMessage = 'Is Pull forward,not order type:CR/CC/CM/ZMII for Adidas,30 days before SCI delivery';
						End;
						Else If @MrTeam != 'MR01'
						Begin
							Set @ErrorMessage = 'not Pull forward,Marketing II~V,30 days before SCI delivery';
						End;
						Else
						Begin
							Set @Type_Brand = @BrandID;
							Set @EachConsDays = 45;
							Set @ErrorMessage = 'not Pull forward,adidas/Rebook,45 days before SCI delivery';
						End;
					End;
				End;
				
				Set @CheckDate = DateAdd(Day, @EachConsDays * -1, @Verifydate);
				Set @CheckDate_Old = @CheckDate;
				Set @CheckDate = dbo.CheckHolidayReturnDate(@CheckDate, @FactoryID, @FtyCountry, 0);

				Set @IsDifferent = IIF(@CheckDate = @CheckDate_Old, 0, 1);
				Set @CheckDiffDay = DateDiff(Day, @EachCons_ApvDate, @CheckDate);
				
				If @CheckDiffDay < 0	-- 2017.11.10 modify by Ben
				Begin
					Set @IsError = 1;
				End;
			End;
		End;
		Else If @Category = 'G' and @MrTeam In ('MR01', 'MR03')
		Begin
			Set @EachConsDays = 0;
			Set @CheckDate = DateAdd(Day, @EachConsDays, @SciDelivery);
			Set @CheckDate_Old = @CheckDate;
			Set @CheckDate = dbo.CheckHolidayReturnDate(@CheckDate, @FactoryID, @FtyCountry, 0);

			Set @IsDifferent = IIF(@CheckDate = @CheckDate_Old, 0, 1);

			If	@CheckDiffDay < 0
			Begin
				Set @ErrorMessage = 'Is Garment order:for MR01/MR03, 0 days before SCI delivery'
				Set @IsError = 1;
			End;
		End;
		Else
		Begin
			Set @Verifydate = @BuyerDelivery;
			Set @CheckDate = DateAdd(Day, @EachConsDays * -1, @Verifydate);
			Set @CheckDate_Old = @CheckDate;
			Set @CheckDate = dbo.CheckHolidayReturnDate(@CheckDate, @FactoryID, @FtyCountry, 0);
			Set @IsDifferent = IIF(@CheckDate = @CheckDate_Old, 0, 1);
			Set @ErrorMessage = 'Is Sample,by OrderType to check';
			Set @CheckDiffDay = DateDiff(Day, @CheckDate, @EachCons_ApvDate);
			If @CheckDiffDay > 0	-- 2017.11.10 modify by Ben
			Begin
				Set @IsError = 1;
			End;
		End;

		Insert Into @Tmp_Provide
			(OrderID,  Type_Brand, Days, ApproveDate, CheckDate, CheckDiffDay, IsDifferent, IsPullForward, IsError, ErrorMessage)
		Values
			(@OrderID,  @Type_Brand, @EachConsDays, @EachCons_ApvDate, @CheckDate, @CheckDiffDay, @IsDifferent, @IsPullForward, @IsError, IIF(@IsError = 1, @ErrorMessage, ''))
	End;
	Else IF @type = '1'	--1. M/Notice
	Begin
		Set @IsPF = dbo.CheckIsPullForward(@BuyerDelivery, @SciDelivery);
		Set @IsPullForward = IIF(@IsPF = 1, 'Y', '');
		Select @MrTeam = Brand.MrTeam From dbo.Brand Where Brand.ID = @BrandID;

		--當M/NOTICE的APV & CMPQ 是空白時，給列印當日的日期
		If @MNotice_ApvDate Is Null
		Begin
			Set @MNotice_ApvDate = GetDate();
		End;

		If @CMPQDate Is Null
		Begin
			Set @CMPQDate = GetDate();
		End;

		If @KPIProjectID != '' And @Category = 'B' And not (@BrandID = 'U.ARMOUR' and @OrderTypeID in ('AR-RM','AR-RM-TPO'))
		Begin
			--大貨專案單，Cfm Date + 5 天內要壓approve
			Set @Days = 5;
			Set @ProvideDate = DateAdd(Day, @Days, @CfmDate);
			Set @ProvideDate_Old = @ProvideDate;
			Set @ProvideDate = dbo.CheckHolidayReturnDate(@ProvideDate, @FactoryID, @FtyCountry, 0);

			Set @IsDifferent = IIF(@ProvideDate = @ProvideDate_Old, 0, 1);

			If	  (@CMPQDate > @ProvideDate_Old)
			   Or (@MNotice_ApvDate > @ProvideDate_Old)
			Begin
				Set @ErrorMessage = 'Is project and Bulk bigger than Order cfm Date add 5';
				Set @IsError = 1;
			End;
		End;
		Else If @Category = 'G' and @MrTeam In ('MR01', 'MR03')
		Begin
			Set @Days = 5;
			Set @ProvideDate = DateAdd(Day, @Days, @CfmDate);
			Set @ProvideDate_Old = @ProvideDate;
			Set @ProvideDate = dbo.CheckHolidayReturnDate(@ProvideDate, @FactoryID, @FtyCountry, 0);

			Set @IsDifferent = IIF(@ProvideDate = @ProvideDate_Old, 0, 1);

			If	  (@CMPQDate > @ProvideDate_Old)
				Or (@MNotice_ApvDate > @ProvideDate_Old)
			Begin
				Set @ErrorMessage = 'Is Garment order:for MR01/MR03, Order cfm Date add 5 days is fail'
				Set @IsError = 1;
			End;
		End;
		Else
		Begin
			Set @Days = 30;

			If @VasShas = 1 And @BrandID In ('ADIDAS', 'REEBOK')
			Begin
				Set @ErrorMessage = 'Is VAS/SHA order:for Adidas,34 days before SCI delivery is fail'
				Set @Days = 34;
			End;
			Else If @GarmentLT <= 30 and DateDiff(Day, @CfmDate, @BuyerDelivery) <= 40 and @BrandID in ('ADIDAS', 'REEBOK')
			Begin
				Set @Days = -5;
				Set @SciDelivery = @CfmDate;
				Set @ErrorMessage = 'Is short lead time order: GL ≦ 30 days style,5 days after order cfm date';
			End;
			Else If @GarmentLT = 30 and @BrandID in ('ADIDAS', 'REEBOK')
			Begin
				Set @Days = 30;
				Set @ErrorMessage = 'order:for Adidas,30 days before SCI delivery';
			End;
			Else If @GarmentLT between 31 and 75 and @BrandID in ('ADIDAS', 'REEBOK')
			Begin
				Set @ErrorMessage = 'Is short lead time order: GL ≦ 75~31 days style,30 days before SCI delivery';
			End;
			Else If @IsPF = 1 or (@BrandID = 'U.ARMOUR' and @OrderTypeID in ('AR-RM','AR-RM-TPO'))
			Begin
				Set @ErrorMessage = 'Is Pull forward,30 days before SCI delivery is fail';
			End;
			Else If @BrandID = 'LLL'
			Begin
				Set @ErrorMessage = 'Is LLL,30 days before SCI delivery is fail';
			End;
			Else
			Begin
				Set @ErrorMessage = 'Is adidas/Rebook or Marketing II~V,45 days before SCI delivery is fail'
				Set @Days = 45;
			End;

			Set @ProvideDate = DateAdd(Day, @Days * -1, @SciDelivery);
			Set @ProvideDate_Old = @ProvideDate;
			Set @ProvideDate = dbo.CheckHolidayReturnDate(@ProvideDate, @FactoryID, @FtyCountry, 0);

			Set @IsDifferent = IIF(@ProvideDate = @ProvideDate_Old, 0, 1);

			If	  (@CMPQDate > @ProvideDate_Old)
			   Or (@MNotice_ApvDate > @ProvideDate_Old)
			Begin
				Set @IsError = 1;
			End;
		End;
			
		Insert Into @Tmp_Provide
			(OrderID,  Days, ApproveDate, CheckDate, IsDifferent, IsPullForward, IsError, ErrorMessage)
		Values
			(@OrderID,  @Days, @MNotice_ApvDate, @ProvideDate, @IsDifferent, @IsPullForward, @IsError, IIF(@IsError = 1, @ErrorMessage, ''))
	End;

	Return;
End

Create Procedure [dbo].[PowerBI_OrderComparison]
	(  @_ExchangeType VarChar(2) = 'FX'
	 , @_ToCurrencyID VarChar(3) = 'USD'
	)
As
Begin
	Set NoCount On;
	
	If Exists(Select * From PBIReportData.sys.tables Where Name = 'OrderComparison')
	Begin
		Drop Table PBIReportData.dbo.OrderComparison;
	End;
	Create Table PBIReportData.dbo.OrderComparison
		(  MrTeam				VarChar(5)
		 , Brand				VarChar(8)
		 , Style				VarChar(15)
		 , Season				VarChar(10)
		 , SeasonSCI			VarChar(10)
		 , Category				VarChar(30)
		 , Project				VarChar(5)
		 , Program				VarChar(12)
		 , CdCode				VarChar(100)
		 , ApparelType			VarChar(50)
		 , FabricType			VarChar(5)
		 , Factory				VarChar(8)
		 , FactoryCountry		VarChar(2)
		 , Destination			VarChar(2)
		 , Currency				VarChar(3)
		 , SMR					VarChar(10)
		 , SMRNM				VarChar(150)
		 , MRHandle				VarChar(10)
		 , MRHandleNM			VarChar(150)
		 , Ukey					BigInt
		)
	;
		
	If Exists(Select * From PBIReportData.sys.tables Where Name = 'OrderComparison_Detail')
	Begin
		Drop Table PBIReportData.dbo.OrderComparison_Detail;
	End;
	Create Table PBIReportData.dbo.OrderComparison_Detail
		(  MrTeam				VarChar(5)
		 , Brand				VarChar(8)
		 , Style				VarChar(15)
		 , Season				VarChar(10)
		 , SeasonSCI			VarChar(10)
		 , OrderID				VarChar(13)
		 , CFMDate				Date
		 , CFMYM				VarChar(6)
		 , CFMYear				VarChar(4)
		 , CFMYM_L				VarChar(6)
		 , CFMYear_L			VarChar(4)
		 , CFMMonth				VarChar(2)
		 , CFMMonthNM			VarChar(30)
		 , Category				VarChar(30)
		 , Project				VarChar(5)
		 , Program				VarChar(12)
		 , CdCode				VarChar(100)
		 , ApparelType			VarChar(50)
		 , FabricType			VarChar(5)
		 , Factory				VarChar(8)
		 , FactoryCountry		VarChar(2)
		 , Destination			VarChar(2)
		 , Currency				VarChar(3)
		 , SMR					VarChar(10)
		 , SMRNM				VarChar(150)
		 , MRHandle				VarChar(10)
		 , MRHandleNM			VarChar(150)
		 , CfmPrice				Numeric(16,4)
		 , CPU					Numeric(8,3)
		 , Qty					Numeric(6,0)
		 , OrderAmt				Numeric(16,2)
		 , OrderSurcharge		Numeric(16,2)
		 , OrderTotalAmt		Numeric(16,2)
		 , OrderAmt_FX			Numeric(16,2)
		 , OrderSurcharge_FX	Numeric(16,2)
		 , OrderTotalAmt_FX		Numeric(16,2)
		 , OrderComparisonUkey	BigInt
		)
	;

	Declare @ExchangeType VarChar(2) = @_ExchangeType;
	Declare @ToCurrencyID VarChar(3) = @_ToCurrencyID;
		
	Declare @ExecSQL NVarChar(MAX);
	Set @ExecSQL =
			'
			Insert Into PBIReportData.dbo.OrderComparison_Detail
				Select *
				  From OpenQuery([TradeDB],
				   ''Select *
						 , OrderComparisonUkey	= Dense_Rank() Over (Order by MrTeam, Brand, Style, Season, SeasonSCI
																			, Category, Project, Program, CdCode, ApparelType, FabricType
																			, Factory, FactoryCountry, Destination
																			, Currency, SMR, SMRNM, MRHandle, MRHandleNM
																	)
					  From (Select MrTeam				= b.MrTeam
								 , Brand				= o.BrandID
								 , Style				= o.StyleID
								 , Season				= o.SeasonID
								 , SeasonSCI			= ss.SeasonSCIID
								 , OrderID				= o.ID
								 , CFMDate				= o.CFMDate
								 , CfmYM				= Convert(VarChar(6), o.CFMDate, 112)
								 , CfmYear				= Year(o.CFMDate)
								 , CfmYM_L				= Convert(VarChar(6), LastCfm.CfmDateL, 112)
								 , CfmYear_L			= Year(LastCfm.CfmDateL)
								 , CfmMonth				= Format(o.CFMDate, ''''MM'''')
								 , CfmMonthNM			= DateName(Month, o.CFMDate)
								 , Category				= IsNull(cat.Name, o.Category)
								 , Project				= IsNull(o.ProjectID, '''''''')
								 , Program				= IsNull(o.ProgramID, '''''''')
								 , CdCode				= IsNull(o.CdCodeID, '''''''') + IIF(IsNull(cdc.Description, '''''''') = '''''''', '''''''', ''''-'''' + cdc.Description)
								 , ApparelType			= IsNull(apptype.Name, IsNull(s.ApparelType, ''''''''))
								 , FabricType			= IsNull(s.FabricType, '''''''')
								 , Factory				= o.FactoryID
								 , FactoryCountry		= f.CountryID
								 , Destination			= IIF(IsNull(o.Dest, '''''''') = '''''''', IsNull(ccd.CountryID, ''''''''), o.Dest)
								 , Currency				= o.CurrencyID
								 , SMR					= o.SMR
								 , SMRNM				= IsNull(SMR.NameAndExt, '''''''')
								 , MRHandle				= o.MRHandle
								 , MRHandleNM			= IsNull(MRHandle.NameAndExt, '''''''')
								 , CFMPrice				= o.CFMPrice
								 , CPU					= o.CPU
								 , Qty					= o.Qty
								 , OrderAmt				= IsNull(ordamt.Amount, 0)
								 , OrderSurcharge		= IsNull(ordamt.AmountSur, 0)
								 , OrderTotalAmt		= IsNull(ordamt.Amount, 0) + IsNull(ordamt.AmountSur, 0)
								 , OrderAmt_FX			= IsNull(amtFX.Amount, 0)
								 , OrderSurcharge_FX	= IsNull(amtFX.AmountSur, 0)
								 , OrderTotalAmt_FX		= IsNull(amtFX.AmountTtl, 0)
							  From Trade.dbo.Orders as o
							 Inner Join Trade.dbo.Style as s On s.Ukey = o.StyleUkey
							  Left Join Trade.dbo.Season as ss On ss.BrandID = o.BrandID And ss.ID = o.SeasonID
							  Left Join Trade.dbo.Brand as b On b.ID = o.BrandID
							  Left Join Trade.dbo.GetName as MRHandle On MRHandle.ID = o.MRHandle
							  Left Join Trade.dbo.GetName as SMR On SMR.ID = o.SMR
							  Left Join Trade.dbo.Factory as f On f.ID = o.FactoryID
							  Left Join Trade.dbo.CDCode as cdc On cdc.ID = o.CdCodeID
							  Left Join Trade.dbo.CustCD as ccd On ccd.BrandID = o.BrandID And ccd.ID = o.CustCDID
							  Left Join Trade.dbo.Reason as apptype On apptype.ReasonTypeID = ''''Style_Apparel_Type'''' And apptype.ID = s.ApparelType And apptype.Junk = 0
							  Left Join Trade.dbo.DropDownList as cat On cat.Type = ''''Category'''' And cat.ID = o.Category
							 Outer Apply (	Select Qty = Sum(Qty) From Trade.dbo.Order_Qty as oq Where oq.ID = o.ID) as ordqty
							 Outer Apply (	Select * From Trade.dbo.GetOrderAmount(o.ID)) as ordamt
							 Outer Apply (	Select * From Trade.dbo.GetCurrencyRate(''''' + @ExchangeType + ''''', o.CurrencyID, ''''' + @ToCurrencyID + ''''', o.cfmDate) as currency) as cur
							 Outer Apply (	Select Amount		= Round(IsNull(ordamt.Amount, 0) * cur.Rate, cur.Exact)
												 , AmountSur	= Round(IsNull(ordamt.AmountSur, 0) * cur.Rate, cur.Exact)
												 , AmountTtl	= Round(IsNull(ordamt.Amount, 0) + IsNull(ordamt.AmountSur, 0) * cur.Rate, cur.Exact)
										 ) as amtFX
							 Outer Apply (	Select CfmDateL = DateAdd(Year, -1, o.CFMDate)) as LastCfm
							 Where IsNull(o.Junk, 0) = 0
						 ) as ord
					 Order by OrderComparisonUkey
				   '');
			';
	Exec (@ExecSQL);

	Insert PBIReportData.dbo.OrderComparison
		(MrTeam, Brand, Style, Season, SeasonSCI
		 , Category, Project, Program, CdCode, ApparelType, FabricType
		 , Factory, FactoryCountry, Destination
		 , Currency, SMR, SMRNM, MRHandle, MRHandleNM
		 , Ukey
		)
		Select MrTeam, Brand, Style, Season, SeasonSCI
			 , Category, Project, Program, CdCode, ApparelType, FabricType
			 , Factory, FactoryCountry, Destination
			 , Currency, SMR, SMRNM, MRHandle, MRHandleNM, OrderComparisonUkey
		  From PBIReportData.dbo.OrderComparison_Detail
		 Group by MrTeam, Brand, Style, Season, SeasonSCI
				, Category, Project, Program, CdCode, ApparelType, FabricType
				, Factory, FactoryCountry, Destination
				, Currency, SMR, SMRNM, MRHandle, MRHandleNM, OrderComparisonUkey
	;

End
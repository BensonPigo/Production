


Create Function [dbo].[GetRequestETA]
(		
	@ID		VarChar(13)
)
Returns Date
As
Begin
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	--Set NoCount On;
	Declare @RequestETA Date;
	
	Declare @Category VarChar(1);
	Declare @BrandID VarChar(8);
	Declare @StyleID VarChar(15);
	Declare @SeasonID VarChar(10);
	Declare @FactoryID VarChar(8);
	Declare @CfmDate Date;
	Declare @FtyCountry VarChar(2);
	Declare @MrTeam VarChar(5);
	Declare @GmtLT Int;
	Declare @SciDelivery Date;

	Select @Category = Orders.Category
		 , @BrandID = Orders.BrandID
		 , @StyleID = Orders.StyleID
		 , @SeasonID = Orders.SeasonID
		 , @FactoryID = Orders.FactoryID
		 , @CfmDate = Orders.CfmDate
		 , @FtyCountry = Factory.CountryID
		 , @MrTeam = Brand.MrTeam
	  From dbo.Orders
	  Left Join dbo.Factory
		On Factory.ID = Orders.FactoryID
	  Left Join dbo.Brand
		On Brand.ID = Orders.BrandID
	 Where Orders.ID = @ID;
	
	Set @GmtLT = dbo.GetStyleGMTLT(@BrandID, @StyleID, @SeasonID, @FactoryID);

	Select @SciDelivery = MinSciDelivery
	  From dbo.GetSCI(@ID, 'S');
	
	If @Category = 'S'
	Begin
		--Sample固定為21天
		Set @RequestETA = DateAdd(day, -21, @SciDelivery);
	End;
	Else
	Begin
		Set @RequestETA = 
			Case
			When @FtyCountry = 'PH' And @MrTeam = '01' Then
				DateAdd(day, -21, @SciDelivery)
			When @GmtLT <= 75 And @MrTeam = '01' Then
				DateAdd(day, -28, @SciDelivery)
			When @FtyCountry = 'PH' And @MrTeam != '01' Then
				DateAdd(day, -30, @SciDelivery)
			Else
				DateAdd(day, -40, @SciDelivery)
			End;
	End;
	
	If @RequestETA < @CfmDate
	Begin
		Set @RequestETA = @CfmDate;
	End;
	
	Return @RequestETA;
End
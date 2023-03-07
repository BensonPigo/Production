-- =============================================
-- Author:		Jeff
-- Create date: 2023/02/17
-- Description:	From trade 只取需要部份 for < WH P01 Material Compare >
-- =============================================
Create Function [dbo].[GetECFA]
	-- Add the parameters for the stored procedure here
	(
	  @OrderID		VarChar(13)		
	)
Returns Bit
As
Begin
	Declare @IsECFA Bit;
	Set @IsECFA = 0;

	Declare @Dest VarChar(2);	--進口國
	Declare @FactoryID VarChar(8);
	Declare @FactoryIsECFA Bit;
	Declare @FactoryCountry VarChar(2);

	Select @Dest = Dest
		 , @FactoryID = FactoryID
	  From dbo.Orders WITH (nolock)
	 Where ID = @OrderID;
	
	Select @FactoryIsECFA = IsECFA
		 , @FactoryCountry = CountryID
	  From Production.dbo.Factory WITH (nolock)
	 Where ID = @FactoryID;
	
	--1. 工廠可走ECFA
	--2. 內銷單(出貨地與進口別為一致時)
	If (@FactoryIsECFA = 1) And (@Dest = @FactoryCountry)
	Begin
		Set @IsECFA = 1;
	End;

	Return @IsECFA;
End
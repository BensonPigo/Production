
CREATE Function [dbo].[MarkerLengthToYDS]
(
	  @MarkerLength		VarChar(15)
)
Returns Numeric(7,4)
As
Begin
	Declare @MarkerYds Numeric(7,4)
	Set @MarkerYds = 0;
	
	Declare @LocateYd Int;
	Declare @LocateInch Int;
	Declare @LocateS1 Int;
	Declare @LocateS2 Int;
	Declare @LocateS3 Int;

	Set @LocateYd = CharIndex('Y', @MarkerLength);
	Set @LocateInch = CharIndex('-', @MarkerLength);
	Set @LocateS1 = CharIndex('/', @MarkerLength);
	Set @LocateS2 = CharIndex('+', @MarkerLength);
	Set @LocateS3 = CharIndex('"', @MarkerLength);
	If @LocateS3 = 0
	Begin
		Set @LocateS3 = Len(@MarkerLength) + 1;
	End;

	Declare @Yds Numeric(7,4);
	Declare @Inch Numeric(7,4);
	Declare @M1 Numeric(7,4);
	Declare @M2 Numeric(7,4);
	Declare @M3 Numeric(7,4);

	Set @Yds = Cast(SubString(@MarkerLength, 1, @LocateYd - 1) as Numeric(7,4));
	Set @Inch = Cast(SubString(@MarkerLength, @LocateYd + 1, @LocateInch - @LocateYd - 1) as Numeric(7,4));
	Set @M1 = Cast(SubString(@MarkerLength, @LocateInch + 1, @LocateS1 - @LocateInch - 1) as Numeric(7,4));
	If @LocateS2 = 0
	Begin
		Set @M2 = Cast(SubString(@MarkerLength, @LocateS1 + 1, @LocateS3 - @LocateS1 - 1) as Numeric(7,4));
		Set @M3 = 0;
	End;
	Else
	Begin
		Set @M2 = Cast(SubString(@MarkerLength, @LocateS1 + 1, @LocateS2 - @LocateS1 - 1) as Numeric(7,4));
		Set @M3 = Cast(SubString(@MarkerLength, @LocateS2 + 1, @LocateS3 - @LocateS2 - 1) as Numeric(7,4));
	End;
	
	If @M2 = 0
	Begin
		Set @MarkerYds = ((@Yds * 36) + @Inch + @M3) / 36
	End;
	Else
	Begin
		Set @MarkerYds = ((@Yds * 36) + @Inch + @M3 + (@M1 / @M2)) / 36
	End;

	-- Return the result of the function
	Return @MarkerYds

End
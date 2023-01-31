
Create Function [dbo].[ActCuttingPerimeterToInch]
(
	  @ActCuttingPerimeter	VarChar(50)
)
Returns Numeric(20,4)
As
Begin
	Declare @MarkerYds Numeric(20,4)
	Set @MarkerYds = 0;

	
	Declare @LocateYd Int;
	Declare @LocateInch Int;
	Declare @Length Int;
	Declare @LocateS1 Numeric(10,4);
	Declare @LocateS2 Numeric(10,4);
	Declare @LocateS3 Numeric(10,4);

	Set @LocateYd = CharIndex('Y', @ActCuttingPerimeter);
	Set @LocateInch = CharIndex('"', @ActCuttingPerimeter);
	Set @Length = LEN(@ActCuttingPerimeter);
	Set @LocateS1 = Cast(SubString(@ActCuttingPerimeter, 1, @LocateYd - 1) as Numeric(10,4));
	Set @LocateS2 = Cast(SubString(@ActCuttingPerimeter, @LocateYd + 2, @LocateInch - @LocateYd - 2) as Numeric(10,4));
	Set @LocateS3 = Cast(SubString(@ActCuttingPerimeter, @LocateInch + 1, @Length) as Numeric(10,4));

	if(@LocateS1 != 0 and @LocateS3 !=0)
	Begin
		Set @MarkerYds = Cast((@LocateS1*36) + @LocateS2 + (@LocateS3/32) as Numeric(20,4));
	End;
	else if(@LocateS1 = 0 and @LocateS3 != 0)
	Begin
		Set @MarkerYds = Cast(@LocateS2 + (@LocateS3/32) as Numeric(20,4));
	End;
	else if (@LocateS1 != 0 and @LocateS3 = 0)
	Begin
		Set @MarkerYds = Cast((@LocateS1*36) + @LocateS2 as Numeric(20,4));
	End;
	else if (@LocateS1 = 0 and @LocateS3 = 0)
	Begin
		Set @MarkerYds = Cast(@LocateS2 as Numeric(20,4));
	End;
	-- Return the result of the function
	Return @MarkerYds
End
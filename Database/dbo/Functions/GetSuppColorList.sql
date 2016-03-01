
Create Function [dbo].[GetSuppColorList]
(
	  @SCIRefNo		VarChar(20)				--
	 ,@SuppID		VarChar(6)				--
	 ,@ColorID		VarChar(6)				--
	 ,@BrandID		VarChar(8)
	 ,@SeasonID		VarChar(10)
	 ,@ProgramID	VarChar(12)
	 ,@StyleID		VarChar(15)
)
Returns VarChar(Max)
As
Begin
	--Set NoCount On;
	Declare @SuppColorList Varchar(Max);

	Set @SuppColorList = '';

	Declare @SuppColor VarChar(30);
	Declare @SuppColorCursor Table
		(RowID BigInt Identity(1,1) Not Null, SuppColor Varchar(30));
	Declare @SuppColorRowID Int;		--Supp. Color Row ID
	Declare @SuppColorRowCount Int;		--Supp. Color 總資料筆數

	Insert Into @SuppColorCursor
		(SuppColor)
		Select SuppColor From dbo.GetSuppColor(@SCIRefNo, @SuppID, @ColorID, @BrandID, @SeasonID, @ProgramID, @StyleID);
	
	Select @SuppColorRowID = Min(RowID), @SuppColorRowCount = Max(RowID) From @SuppColorCursor;
	While @SuppColorRowID <= @SuppColorRowCount
	Begin
		Select @SuppColor = SuppColor
		  From @SuppColorCursor
		 Where RowID = @SuppColorRowID;
		
		Set @SuppColorList += RTrim(@SuppColor) + ',';

		Set @SuppColorRowID += 1;
	End;

	Return @SuppColorList;
End
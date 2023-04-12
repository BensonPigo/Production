CREATE Function [dbo].[GetSuppColorList]
(
	  @SCIRefNo		VarChar(30)				--
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
	
	Set @SuppColorList = IsNull(stuff(
								(Select ',' + RTrim(SuppColor)
								   From Production.dbo.GetSuppColor_NEW(@SCIRefNo, @SuppID, @ColorID, @BrandID, @SeasonID, @ProgramID, @StyleID)
								  Order by SeqNo
									For Xml Path('')
								),1,1,''), '');

	IF(Replace(@SuppColorList,',','') = '')
	Begin
		Set @SuppColorList = '';
	End
	Return @SuppColorList;
End
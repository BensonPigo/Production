CREATE Function [dbo].[GetColorName]
	(
	  @BrandID		VarChar(8)
	 ,@ColorID		VarChar(6)
	 ,@ColorType	VarChar(1)	--顏色的組合方式: 1.ColorId – ColorName; 2. ColorName (SuppColorCode); 3. ColorName; 4. ColorID
	 ,@SuppColor	VarChar(Max)
	)
Returns VarChar(Max)
As
Begin
	--Set NoCount On;
	Declare @ColorName VarChar(Max);
	Set @ColorName = '';
	
	If IsNull(@ColorType, '') = ''
	Begin
		Set @ColorType = '1';
	End;
	
	Declare @SuppColorIndex Int;
	--Declare @SuppColorLocation Int;
	Declare @SuppColorID VarChar(20);
	Declare @MultipleColorID VarChar(6);
	Declare @MultipleColorName NVarChar(max);
	Declare @MultipleColorFullName NVarChar(Max);
	Declare @Color_Multiple Table
		(RowID BigInt Identity(1,1) Not Null, ColorID VarChar(6));
	Declare @ColorRowID Int;		--Color Row ID
	Declare @ColorRowCount Int;		--Color 總資料筆數
	
	declare @spColorList table(Data nvarchar(Max), no int)
	insert into @spColorList
	select * from Production.dbo.SplitString(@SuppColor,',')

	Insert Into @Color_multiple
		(ColorID)
		Select Color_Multiple.ColorID
		  From Production.dbo.Color
		 Inner Join Production.dbo.Color_Multiple
			On Color_Multiple.ColorUkey = Color.Ukey
		 Where Color.BrandID = @BrandID
		   And Color.ID = @ColorID
		 Order by SeqNo;
	If @@RowCount = 0
	Begin
		Insert Into @Color_Multiple
			(ColorID)
		Values
			(@ColorID);
	End;
	
	Set @ColorRowID = 1;
	Set @SuppColorIndex = 1;
	--Set @SuppColorLocation = 1;
	Select @ColorRowID = Min(RowID), @ColorRowCount = Max(RowID) From @Color_multiple;
	While @ColorRowID <= @ColorRowCount
	Begin
		Select @MultipleColorID = ColorID
		  From @Color_Multiple
		 Where RowID = @ColorRowID;
		
		If IsNull(@MultipleColorID, '') != ''
		Begin
			Select @MultipleColorName = RTrim(Color.Name)
			  From Production.dbo.Color
			 Where Color.BrandID = @BrandID
			   And Color.ID = @MultipleColorID;
			
			--取得對應的Supplier Color(依照逗號分隔順序)
			If IsNull(@SuppColor,'') = ''
			Begin
				Set @SuppColorID = '';
			End;
			Else
			Begin
				--if(CharIndex(',', @SuppColor, @SuppColorLocation + 1) - @SuppColorLocation >= 0)
				--	Set @SuppColorID = SubString(@SuppColor, @SuppColorLocation, CharIndex(',', @SuppColor, @SuppColorLocation + 1) - @SuppColorLocation);
				set @SuppColorID = (select Data from @spColorList where [no] = @ColorRowID)
			End;

			If @ColorType In ('2', '4')
			Begin
				Set @MultipleColorFullName = @MultipleColorName + IIF(IsNull(@SuppColorID,'') != '', '(' + Rtrim(@SuppColorID) + ')', '');
				If @ColorRowID = 1
				Begin
					Set @ColorName = @MultipleColorFullName;
				End;
				Else
				Begin
					If @ColorType = '2'
					Begin
						Set @ColorName += IIF(IsNull(@MultipleColorFullName,'') = '', '', Char(13) + Char(10) + @MultipleColorFullName);
					End;
					Else
					Begin
						Set @ColorName += IIF(IsNull(@MultipleColorFullName,'') = '', '', '/' + @MultipleColorFullName);
					End;
				End;
			End;
			Else
			Begin
				If @ColorRowID = 1
				Begin
					If @ColorType In ('3', '5')
					Begin
						Set @ColorName = @MultipleColorName;
					End;
					Else
					Begin
						Set @ColorName = @MultipleColorID + '-' + @MultipleColorName;
					End;
				End;
				Else
				Begin
					Set @ColorName += 
					Case
					When @ColorType = '3' Then
						IIF(IsNull(@MultipleColorName,'') = '', '', Char(13) + Char(10) + @MultipleColorName)
					When @ColorType = '5' Then
						IIF(IsNull(@MultipleColorName,'') = '', '', '/' + @MultipleColorName)
					Else
						IIF(IsNull(@MultipleColorName,'') = '', '', Char(13) + Char(10) + @MultipleColorID + '-' + @MultipleColorName)
					End;
				End;
			End;
		End;

		Set @ColorRowID += 1;
		--Set @SuppColorLocation = CharIndex(',', @SuppColor, @SuppColorLocation + 1) + 1;
	End;

	Return @ColorName;
End

Create Function [dbo].[CheckHolidayReturnDate]
	(
	  @Date			Date
	 ,@FactoryID	VarChar(8)
	 ,@CountryID	VarChar(2)
	 ,@Type			VarChar(1)	--0.遇假日時日期往前一天; 1.遇假日時日期順延一天
	)
Returns Date
As
Begin
	--Set NoCount On;
	Declare @FtyWorkDate Date;
	Set @FtyWorkDate = @Date
	
	Declare @IsHoliday Bit;
	Declare @Holiday Date;
	Set @IsHoliday = 1;
	While @IsHoliday = 1
	Begin
		Set @Holiday = Null;
		Select @Holiday = Holiday.HolidayDate
		  From dbo.Holiday
		 Where Holiday.HolidayDate = @FtyWorkDate
		   AND (Isnull(Holiday.FactoryID, '') = '' or Isnull(Holiday.FactoryID, '') Like '%'+@FactoryID+'%')
		  -- And (   (@FactoryID = '' And Holiday.Holiday4Fty = '')
				--Or (@FactoryID != '' And Holiday.Holiday4Fty Like '%'+@FactoryID+'%' )
			 --  );
	
		If @Holiday is Not Null
		Begin
			Set @FtyWorkDate = 
				Case
					When @Type = '0' Then	--遇假日時日期往前一天
						DateAdd(Day, -1, @FtyWorkDate)
					When @Type = '1' Then	--遇假日時日期順延一天
						DateADD(Day, 1, @FtyWorkDate)
				End;
		End;
		Else
		Begin
			Set @IsHoliday = 0;
		End;
	End;

	Return @FtyWorkDate;
End
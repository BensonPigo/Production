
Create Function [dbo].[GetSuppColor_Solution]
(
	  @ColorUkey	BigInt
	 ,@SeasonID		VarChar(10)
	 ,@SuppID		VarChar(6)
	 ,@BrandID		VarChar(8)
	 ,@ProgramID	VarChar(12)
	 ,@StyleID		VarChar(15)
	 ,@RefNo		VarChar(20)
)
Returns Varchar(30)
As
Begin
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	--Set NoCount On;
	Declare @SuppColor VarChar(30)

	--1. 依照Season + Style + Program + RefNo
	Select Top 1 @SuppColor = SuppColor
	  From dbo.Color_SuppColor
	 Where ColorUkey = @ColorUkey
	   And SuppID = @SuppID
	   And SeasonID = @SeasonID
	   And StyleID = @StyleID
	   And ProgramID = @ProgramID
	   And RefNo = @RefNo;
			
	--2. 依照Season + Style + RefNo，且排除Program有值的資料
	If IsNull(@SuppColor, '') = ''
	Begin
		Select Top 1 @SuppColor = SuppColor
		  From dbo.Color_SuppColor
		 Where ColorUkey = @ColorUkey
		   And SuppID = @SuppID
		   And SeasonID = @SeasonID
		   And StyleID = @StyleID
		   And IsNull(ProgramID,'') = ''
		   And RefNo = @RefNo;
	End;
	
	--3. 依照Season + Program + RefNo，且排除Style有值的資料
	If IsNull(@SuppColor, '') = ''
	Begin
		Select Top 1 @SuppColor = SuppColor
		  From dbo.Color_SuppColor
		 Where ColorUkey = @ColorUkey
		   And SuppID = @SuppID
		   And SeasonID = @SeasonID
		   And IsNull(StyleID,'') = ''
		   And ProgramID = @ProgramID
		   And RefNo = @RefNo;
	End;
	
	--4. 依照Season + RefNo，且排除Style & Program有值的資料
	If IsNull(@SuppColor, '') = ''
	Begin
		Select Top 1 @SuppColor = SuppColor
		  From dbo.Color_SuppColor
		 Where ColorUkey = @ColorUkey
		   And SuppID = @SuppID
		   And SeasonID = @SeasonID
		   And IsNull(StyleID,'') = ''
		   And IsNull(ProgramID,'') = ''
		   And RefNo = @RefNo;
	End;
	
	--5. 依照Season + Style + Program，且排除RefNo有值的資料
	If IsNull(@SuppColor, '') = ''
	Begin
		Select Top 1 @SuppColor = SuppColor
		  From dbo.Color_SuppColor
		 Where ColorUkey = @ColorUkey
		   And SuppID = @SuppID
		   And SeasonID = @SeasonID
		   And StyleID = @StyleID
		   And ProgramID = @ProgramID
		   And IsNull(RefNo,'') = '';
	End;
	
	--6. 依照Season + Style，且排除Program & RefNo有值的資料
	If IsNull(@SuppColor, '') = ''
	Begin
		Select Top 1 @SuppColor = SuppColor
		  From dbo.Color_SuppColor
		 Where ColorUkey = @ColorUkey
		   And SuppID = @SuppID
		   And SeasonID = @SeasonID
		   And StyleID = @StyleID
		   And IsNull(ProgramID,'') = ''
		   And IsNull(RefNo,'') = '';
	End;
	
	--7. 依照Season + Program，且排除Style & RefNo有值的資料
	If IsNull(@SuppColor, '') = ''
	Begin
		Select Top 1 @SuppColor = SuppColor
		  From dbo.Color_SuppColor
		 Where ColorUkey = @ColorUkey
		   And SuppID = @SuppID
		   And SeasonID = @SeasonID
		   And IsNull(StyleID,'') = ''
		   And ProgramID = @ProgramID
		   And IsNull(RefNo,'') = '';
	End;
	
	--7. 依照Season，且排除Style & RefNo & Program有值的資料
	If IsNull(@SuppColor, '') = ''
	Begin
		Select Top 1 @SuppColor = SuppColor
		  From dbo.Color_SuppColor
		 Where ColorUkey = @ColorUkey
		   And SuppID = @SuppID
		   And SeasonID = @SeasonID
		   And IsNull(StyleID,'') = ''
		   And IsNull(ProgramID,'') = ''
		   And IsNull(RefNo,'') = '';
	End;
	
	Return @SuppColor;
End

CREATE Function [dbo].[GetSuppColor_NEW]
(
	  @SCIRefNo		VarChar(30)				--
	 ,@SuppID		VarChar(6)				--
	 ,@ColorID		VarChar(6)				--
	 ,@BrandID		VarChar(8)
	 ,@SeasonID		VarChar(10)
	 ,@ProgramID	VarChar(12)
	 ,@StyleID		VarChar(15)
)
Returns @Tmp_SuppColor Table
	( SeqNo			Varchar(2)
	 ,ColorID		Varchar(13)
	 ,ColorDesc		Varchar(90)
	 ,SuppColor		Varchar(30)
	)
As
Begin
	--Set NoCount On;
	If IsNULL(@ColorID, '') = ''
		Return;

	Declare @SuppColor Varchar(30);
	Declare @ShowSuppColor Bit;	
	Declare @RefNo VarChar(36);
	Declare @BrandGroup VarChar(8);

	Declare @SeqNo_Detail VarChar(2)
	Declare @ColorID_Detail VarChar(6)
	Declare @Color_MultipleCursor Table
		(RowID BigInt Identity(1,1) Not Null, SeqNo VarChar(2), ColorID VarChar(6))
	Declare @ColorRowID Int;		--Color Row ID
	Declare @ColorRowCount Int;		--Color 總資料筆數

	Declare @ColorUkey BigInt;
	Declare @ColorDesc Varchar(90);
	Declare @MtlTypeId varchar(20);

	Set @SuppColor = '';
	
	--是否要顯示SuppColor
	Select @ShowSuppColor = Fabric_Supp.ShowSuppColor
	  From dbo.Fabric_Supp
	 Where SCIRefNo = @SCIRefNo
	   And SuppID = @SuppID;
	
	--Brand RefNo
	Select @RefNo = Fabric.RefNo,
		@MtlTypeId = Fabric.MtltypeId
	  From dbo.Fabric
	 Where SCIRefNo = @SCIRefNo;
	
	--Brand Group
	Select @BrandGroup = Brand.BrandGroup
	  From dbo.Brand
	 Where ID = @BrandID;
	
	--多色組資料(若為單一色，仍會寫入一筆資料於Color_Multiple)
	Insert Into @Color_MultipleCursor
		(SeqNo, ColorID)
		Select SeqNo, ColorID From Color_Multiple Where BrandID = @BrandID And ID = @ColorID Order by SeqNo;
	
	If @@RowCount = 0
	Begin
		Insert Into @Color_MultipleCursor
			(SeqNo, ColorID)
		Values
			('01', @ColorID);
	End;

	Set @ColorRowID = 1;
	Select @ColorRowID = Min(RowID), @ColorRowCount = Max(RowID) From @Color_MultipleCursor;
	While @ColorRowID <= @ColorRowCount
	Begin
		Select @SeqNo_Detail = SeqNo
				, @ColorID_Detail = ColorID
			From @Color_MultipleCursor
			Where RowID = @ColorRowID;
		
		Select @ColorUkey = Color.Ukey
				, @ColorDesc = Color.Name
			From dbo.Color
			Where ID = @ColorID_Detail
			And BrandID = @BrandID;
			
		Set @SuppColor = '';
		If @ShowSuppColor = 1	--當@ShowSuppColor = true時，才表示需要帶出廠商顏色代碼
		Begin
			If @ColorUkey > 0
			Begin
				Set @SuppColor = dbo.GetSuppColor_Solution_NEW(@ColorUkey, @SeasonID, @SuppID, @BrandID, @ProgramID, @StyleID, @RefNo, @MtlTypeId);

				--當仍無資料時，使用SeasonID等於空白尋找資料
				If IsNull(@SuppColor, '') = ''
				Begin
					Set @SuppColor = dbo.GetSuppColor_Solution_NEW(@ColorUkey, '', @SuppID, @BrandID, @ProgramID, @StyleID, @RefNo, @MtlTypeId);
				End;
			End;
		End;
		Insert Into @Tmp_SuppColor
			(SeqNo, ColorID, ColorDesc, SuppColor)
		Values
			(@SeqNo_Detail, @ColorID_Detail, @ColorDesc, IsNull(@SuppColor, ''));
		

		Set @ColorRowID += 1;
	End;

	Return;
End
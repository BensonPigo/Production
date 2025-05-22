
CREATE FUNCTION [dbo].[GetKeyword_New]
(
	  @OrderID				VarChar(13)
	 ,@Order_BoaUkey		BigInt
	 ,@Keyword				VarChar(Max)
	 ,@Article				VarChar(8)	= ''
	 ,@SizeCode				VarChar(8)	= ''
	 ,@Location				VarChar(1)	= ''
	 ,@Type					int = 1 -- 1. 轉換Keyword字串 2. 回傳KeywordField及KeywordValue的xml字串
)
RETURNS nvarchar(max)
AS
BEGIN
	
	Declare @Keyword_AfterTrans VarChar(Max) = @Keyword;
	
	Declare @PoID VarChar(13);
	Declare @OrderComboID VarChar(13);
	Declare @BrandID VarChar(8);

	Declare @KeywordID VarChar(30);
	Declare @IsSize Bit;
	Declare @IsPatternPanel Bit;
	Declare @IsKeyworExist Bit;
	Declare @FieldName VarChar(100);
	Declare @FieldTable VarChar(100);
	Declare @KeyValue VarChar(Max);
	
	Declare @SizeItem VarChar(3);
	Declare @FabricPanelCode VarChar(3);

	Declare @Keyword_RowID Int;
	Declare @Keyword_RowCount Int;
	Declare @tmp_Keyword Table
		(RowID BigInt Identity(1,1) Not Null, KeywordID VarChar(Max), KeyValue VarChar(Max));
	
	Select @PoID = PoID
		 , @OrderComboID = OrderComboID
		 , @BrandID = BrandID
	  From dbo.Orders
	 Where ID = @OrderID;

	With Cte_Keyword (Keyword, StartPos, EndPos) As
	(
		Select @Keyword
			 , CharIndex('{',@Keyword) as StartPos
			 , CharIndex('}',@Keyword) as EndPos
		Union All
		Select Keyword
			 , CharIndex('{',Keyword, EndPos + 1) as StartPos
			 , CharIndex('}',Keyword, EndPos + 1) as EndPos
		  From Cte_Keyword
		 Where StartPos > 0
		   And EndPos > 0
	)
	Insert Into @tmp_Keyword
		(KeywordID)
		Select SubString(Keyword, StartPos + 1, (EndPos - StartPos) - 1) as KeywordID
		  From Cte_Keyword
		 Where StartPos > 0
		   And EndPos > 0
		   And StartPos < EndPos
		 Order by StartPos;
	
	Select @Keyword_RowID = Min(RowID), @Keyword_RowCount = Max(RowID) From @tmp_Keyword;
	While @Keyword_RowID <= @Keyword_RowCount
	Begin
		Select @KeywordID = tmp_Keyword.KeywordID
			 , @FieldName = tmp.FieldName
			 , @IsSize = tmp.IsSize
			 , @IsPatternPanel = tmp.IsPatternPanel
			 , @IsKeyworExist = IsNull(tmp.IsExist, 0)
		  From @tmp_Keyword as tmp_Keyword
		  Outer Apply (	Select *, 1 as IsExist
						  From Production.dbo.Keyword
						 Where Keyword.ID = tmp_Keyword.KeywordID
					  ) as tmp
		 Where RowID = @Keyword_RowID;
		
		IF @KeywordID = 'PONo_Combine'
		Begin
			Delete @tmp_Keyword Where RowID = @Keyword_RowID;
			Set @Keyword_RowID += 1;
			Continue;
		End

		Set @KeyValue = '';

		Set @FieldTable = '';
		If @FieldName != ''
		Begin
			Set @FieldTable = SubString(@FieldName, 1, CharIndex('.', @FieldName) - 1);
		End;

		If @IsKeyworExist = 1
		Begin
			If @IsSize = 1 Or Upper(@KeywordID) in (Upper('Sourcing size'), Upper('Customer size'), Upper('Dec lable size'))
			Begin
				If Upper(@KeywordID) = Upper('Sourcing size')
				Begin
					Set @SizeItem = 'S01';
				End;
				Else If Upper(@KeywordID) in (Upper('Customer size'), Upper('Dec lable size'))
				Begin
					Select @SizeItem = case Upper(@KeywordID)
						when Upper('Customer size') then boa.CustomerSizeRelation
						when Upper('Dec lable size') then boa.DecLabelSizeRelation
						else '' end
					from dbo.Order_BOA boa with (nolock)
					where boa.Ukey = @Order_BoaUkey;
				End
				Else
				Begin
					Select @SizeItem = Left(Relation, 3)
					  From dbo.Order_BOA_KeyWord
					 Where ID = @PoID
					   And Order_BOAUkey = @Order_BoaUkey
					   And KeywordID = @KeywordID;
				End;

				If Exists (Select 1 From Production.dbo.Order_SizeSpec_OrderCombo Where ID = @PoID And OrderComboID = @OrderComboID And SizeItem = @SizeItem)
				Begin
					Select @KeyValue = IsNull(SizeSpec, '')
					  From dbo.Order_SizeSpec_OrderCombo
					 Where ID = @PoID
					   And OrderComboID = @OrderComboID
					   And SizeItem = @SizeItem
					   And SizeCode = @SizeCode;
				End;
				Else
				Begin
					Select @KeyValue = IsNull(SizeSpec, '')
					  From dbo.Order_SizeSpec
					 Where ID = @PoID
					   And SizeItem = @SizeItem
					   And SizeCode = @SizeCode;
				End;
			End;
			Else
			Begin
				If IsNull(@KeywordID, '') = Upper('Color Descciption') or IsNull(@KeywordID, '') = Upper('Color Description')
				Begin
					Set @FabricPanelCode = '';

					Select @FabricPanelCode = FabricPanelCode
					  From dbo.Order_BOA
					 Where ID = @PoID
					   And Ukey = @Order_BoaUkey
				
					If IsNull(@FabricPanelCode, '') != ''
					Begin
						Select @KeyValue = ColorID
						  From dbo.Order_ColorCombo
						 Where ID = @PoID
						   And Article = @Article
						   And FabricPanelCode = @FabricPanelCode;
					End;

					Set @KeyValue = Production.dbo.GetColorName(@BrandID, @KeyValue, '3', '');
				End;
				Else
				Begin
					If IsNull(@KeyValue, '') = '' And Upper(@KeywordID) = Upper('Article')
					Begin
						Set @KeyValue = @Article;
					End;
					Else
					Begin
						If IsNull(@KeyValue, '') = ''
						Begin
							set @KeyValue = dbo.GetKeywordValue_New(@OrderID,@KeywordID,@Article,@SizeCode,@Location)
						End;
					End;
				End;
			End;
		End;

		Set @Keyword_AfterTrans = Replace(@Keyword_AfterTrans, '{' + @KeywordID + '}', @KeyValue);

		Update @tmp_Keyword set KeyValue = @KeyValue Where RowID = @Keyword_RowID;

		Set @Keyword_RowID += 1;
	End;

	if @Type = 2
	Begin
		Return (
			select distinct KeywordField = KeywordID, KeywordValue = KeyValue
			from @tmp_Keyword t
			where exists (select 1 from Production.dbo.Keyword Where Keyword.ID = t.KeywordID)
			For xml path('row')
		)		
	End
	
	Return @Keyword_AfterTrans;
END
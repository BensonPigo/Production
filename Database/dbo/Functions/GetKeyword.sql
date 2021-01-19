CREATE FUNCTION [dbo].[GetKeyword]
(
	  @OrderID				VarChar(13)
	 ,@Order_BoaUkey		BigInt
	 ,@Keyword				VarChar(Max)
	 ,@Article				VarChar(8)	= ''
	 ,@SizeCode				VarChar(8)	= ''
	 ,@Location				VarChar(1)	= ''
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
	Declare @PatternPanel VarChar(3);

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
		
		Set @KeyValue = '';

		Set @FieldTable = '';
		If @FieldName != ''
		Begin
			Set @FieldTable = SubString(@FieldName, 1, CharIndex('.', @FieldName) - 1);
		End;

		If @IsKeyworExist = 1
		Begin
			If @IsSize = 1 Or Upper(@KeywordID) = Upper('Sourcing size')
			Begin
				If Upper(@KeywordID) = Upper('Sourcing size')
				Begin
					Set @SizeItem = 'S01';
				End;
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
				If IsNull(@KeyValue, '') = '' And @IsPatternPanel = 1
				Begin
					Set @PatternPanel = '';

					Select @PatternPanel = Left(Relation, 2)
					  From dbo.Order_BOA_KeyWord
					 Where ID = @PoID
					   And Order_BOAUkey = @Order_BoaUkey
					   And KeywordID = @KeywordID;
				
					If IsNull(@PatternPanel, '') != ''
					Begin
						Select @KeyValue = ColorID
						  From dbo.Order_ColorCombo
						 Where ID = @PoID
						   And Article = @Article
						   And PatternPanel = @PatternPanel;
					End;

					If Upper(@KeywordID) = Upper('Color Descciption')
					Begin
						Set @KeyValue = Production.dbo.GetColorName(@BrandID, @KeyValue, '3', '');
					End;
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
							set @KeyValue = dbo.GetKeywordValue(@OrderID,@KeywordID,@Article,@SizeCode,@Location)

							If Upper(@KeywordID) = Upper('Orig Buyer deliver')
							Begin
								Set @KeyValue = Format(Month(@KeyValue), '00') + '/' + Right(Format(Year(@KeyValue), '0000'), 2);
							End;
						End;
					End;
				End;
			End;
		End;

		Set @Keyword_AfterTrans = Replace(@Keyword_AfterTrans, '{' + @KeywordID + '}', @KeyValue);

		Set @Keyword_RowID += 1;
	End;

	Return @Keyword_AfterTrans;

END

Create Function [dbo].[GetDigitalValue]
(
	  @StrValue		VarChar(15)
)
Returns Numeric(15,4)
As
Begin
Declare @ReturnValue Numeric(15,4)
	Set @ReturnValue = 0;
	Set @StrValue = LTrim(RTrim(@StrValue));	--將文字前後的空白都去掉
	Set @StrValue = replace(@StrValue,char(10),' ');	--將文字換行符號改為空白

	Declare @SymbolCount Int;	--符號個數
	Declare @IsError Bit;		--是否有誤
	Set @SymbolCount = 0;
	Set @IsError = 0;

	If IsNumeric(@StrValue) = 1
		Begin
			Set @ReturnValue = Convert(Numeric(15,4), @StrValue);
		End;
	Else
		Begin
		--檢查格式是否正確
		-- 1. '.' 只能出現在數字格式
		If CharIndex('.', @StrValue) > 0
			Begin
				Set @IsError = 1;
			End;
		-- 2. '-' 僅能出現一次
		If Len(@StrValue) - Len(Replace(@StrValue, '-', '')) > 1	--出現次數 = 字串長度 - 去掉符號後之字串長度
			Begin
				Set @IsError = 1;
			End;
		Else IF Len(@StrValue) - Len(Replace(@StrValue, '-', '')) = 1
			Begin
				Set @SymbolCount += 1;
			End;
		-- 3. '"' 僅能出現一次
		If Len(@StrValue) - Len(Replace(@StrValue, '"', '')) > 1	--出現次數 = 字串長度 - 去掉符號後之字串長度
			Begin
				Set @IsError = 1;
			End;
		Else IF Len(@StrValue) - Len(Replace(@StrValue, '"', '')) = 1
			Begin
				Set @SymbolCount += 1;
			End;
		-- 4. '/' 僅能出現一次
		If Len(@StrValue) - Len(Replace(@StrValue, '/', '')) > 1	--出現次數 = 字串長度 - 去掉符號後之字串長度
			Begin
				Set @IsError = 1;
			End;
		-- 5. '-' & '"' 不可同時出現
		If @SymbolCount > 1
			Begin
				Set @IsError = 1;
			End;
		
		-- 6. 不可出現文字
		If @StrValue like '%[a-zA-Z]%'
			Begin
				Set @IsError = 1;
			End


		If @IsError = 0
			Begin
			Set @StrValue = Replace(Replace(@StrValue, '-', ' '), '"', ' ');	--先將文字內的'-'或'"'轉換為' '，方便取得整數位
			
			Declare @Length Int;	--字串長度
			Declare @Empty Int = 0;	--' '在文字中的位置
			Declare @Divide Int = 0;	--'/'在文字中的位置

			Set @Length = Len(@StrValue);
			Set @Empty = CharIndex(' ', @StrValue);
			Set @Divide = CharIndex('/', @StrValue);

			Declare @Integer Numeric(15,4);		--整數
			Declare @Numerator Numeric(15,4);	--分子
			Declare @Denominator Numeric(15,4);	--分母
			
			--判斷是否有整數
			if @Empty > 0
				begin 
				Set @Integer = Convert(Numeric(15,4), SubString(@StrValue, 1, @Empty - 1));
				end
			else
				begin
				Set @Integer = 0;
				end
			
			--判斷是否有分數
			if @Divide > 0
				begin
				Set @Numerator = Convert(Numeric(15,4), SubString(@StrValue, @Empty + 1, @Divide - @Empty - 1));
				Set @Denominator = Convert(Numeric(15,4), SubString(@StrValue, @Divide + 1, @Length - @Divide));
			
				Set @ReturnValue = @Integer + (@Numerator / @Denominator);
				end
			else
				begin
				Set @ReturnValue = @Integer
				end
			End;
		End;

	-- Return the result of the function
	Return @ReturnValue
End

CREATE Function [dbo].[GetDigitalValue_New]
(
	  @StrValue		VarChar(15)
)
Returns Numeric(15,4)
As
Begin
	Declare @ReturnValue Numeric(15,4)
	Set @ReturnValue = 0;
	Set @StrValue = LTrim(RTrim(@StrValue));	--將文字前後的空白都去掉
	
	--若有不包含數字、雙引號、斜線、空白、小數點的字元則Return 0
	DECLARE @reg VARCHAR(20) = '%[^0-9./" -]%'
	IF (PATINDEX(@reg COLLATE Chinese_Taiwan_Stroke_BIN2, @StrValue) > 0) 
		OR (@StrValue = '.') 
		OR (@StrValue = '-')
		OR (CHARINDEX('.', @StrValue) > 0 
			AND (PATINDEX('[0-9]', SUBSTRING(@StrValue, CHARINDEX('.', @StrValue) - 1, 1)) = 0
				OR PATINDEX('[0-9]', SUBSTRING(@StrValue, CHARINDEX('.', @StrValue) + 1, 1)) = 0))
		OR (CHARINDEX('/', @StrValue) > 0 
			AND (PATINDEX('[0-9]', SUBSTRING(@StrValue, CHARINDEX('/', @StrValue) - 1, 1)) = 0
				OR PATINDEX('[0-9]', SUBSTRING(@StrValue, CHARINDEX('/', @StrValue) + 1, 1)) = 0
				OR SUBSTRING(@StrValue, CHARINDEX('/', @StrValue) + 1, 1) = 0))
		OR (LEN(@StrValue) - LEN(REPLACE(@StrValue, '.', '')) > 1) --點號出現兩次以上
		OR (CHARINDEX(' ', @StrValue) > CHARINDEX('/', @StrValue))
		OR (CHARINDEX('-', @StrValue) > CHARINDEX('/', @StrValue))
		OR (PATINDEX('% [0-9]"%', @StrValue) > 0)
	BEGIN
    	RETURN @ReturnValue
    END

	Declare @SymbolCount Int;	--符號個數
	Declare @IsError Bit;		--是否有誤
	Set @SymbolCount = 0;
	Set @IsError = 0;

	If IsNumeric(@StrValue) = 1
	Begin
		Set @ReturnValue = Convert(Numeric(15,4), Replace(@StrValue, ',', '.'));
	End;
	Else If @StrValue = ''
	Begin
		Set @ReturnValue = Convert(Numeric(15,4), 0);
	End;
	Else
	Begin
		--檢查格式是否正確
		-- 1. '.' 只能出現在數字格式
		--If @IsError = 0 and CharIndex('.', @StrValue) > 0
		--Begin
		--	Set @IsError = 1;
		--End;
		-- 2. '-' 僅能出現一次
		If @IsError = 0 and Len(@StrValue) - Len(Replace(@StrValue, '-', '')) > 1	--出現次數 = 字串長度 - 去掉符號後之字串長度
		Begin
			Set @IsError = 1;
		End;
		Else
		Begin
			If @IsError = 0 and CharIndex('-', @StrValue) > 0
			Begin
				Set @SymbolCount += 1;
			End;
		End;
		-- 3. '"' 僅能出現一次
		If @IsError = 0 and Len(@StrValue) - Len(Replace(@StrValue, '"', '')) > 1	--出現次數 = 字串長度 - 去掉符號後之字串長度
		Begin
			Set @IsError = 1;
		End;
		Else
		Begin
			If @IsError = 0 and CharIndex('"', @StrValue) > 0
			Begin
				Set @SymbolCount += 1;
			End;
		End;
		-- 4. '/' 僅能出現一次
		If @IsError = 0 and Len(@StrValue) - Len(Replace(@StrValue, '/', '')) > 1	--出現次數 = 字串長度 - 去掉符號後之字串長度
		Begin
			Set @IsError = 1;
		End;
		-- 5. '-' & '"' 不可同時出現
		If @IsError = 0 and @SymbolCount > 1
		Begin
			Set @IsError = 1;
		End;
		-- 6. 第一個字元須為數字
		If @IsError = 0 and PATINDEX('%[^0-9]%', substring(@StrValue,1,1)) = 1
		Begin
			Set @IsError = 1;
		End;
		-- 7. 排除包含'*'或'?'字元
		If @IsError = 0 and (CharIndex('*', @StrValue) > 0 or CharIndex('?', @StrValue) > 0)
		Begin
			Set @IsError = 1;
		End;
		---- 8. 需包含' '或'-'或'"'
		--If @IsError = 0 and CharIndex(' ', @StrValue) = 0 and CharIndex('-', @StrValue) = 0 and CharIndex('"', @StrValue) = 0 
		--Begin
		--	Set @IsError = 1;
		--End;
		-- 9. 不可出現英文字母
		If @IsError = 0 and PATINDEX('%[a-zA-Z]%', @StrValue) > 0
		Begin
			Set @IsError = 1;
		End;
		-- 10. ' '僅能出現一次
		If @IsError = 0 and Len(@StrValue) - Len(Replace(@StrValue, ' ', '')) > 1
		Begin
			Set @IsError = 1;
		End;

		If @IsError = 0
		Begin
			Set @StrValue = Replace(Replace(@StrValue, '-', ' '), '"', ' ');	--先將文字內的'-'或'"'轉換為' '，方便取得整數位
			
			Declare @Length Int;	--字串長度
			Declare @Empty Int;		--' '在文字中的位置
			Declare @Divide Int;	--'/'在文字中的位置

			Set @Length = Len(@StrValue);
			Set @Empty = CharIndex(' ', @StrValue);
			Set @Divide = CharIndex('/', @StrValue);

			Declare @Integer Numeric(15,4);		--整數
			Declare @Numerator Numeric(15,4);		--分子
			Declare @Denominator Numeric(15,4);	--分母

			if (@Divide > 0)
			begin
				IF (@Empty > 0) AND ISNUMERIC(SubString(@StrValue, 1, @Empty - 1)) = 1
				BEGIN
					Set @Integer = Convert(Int, SubString(@StrValue, 1, @Empty - 1));
				END
				ELSE
				BEGIN
					SET @Integer = 0;
				END

				IF (@Divide - @Empty - 1) > 0
				BEGIN
					Set @Numerator = Convert(decimal(16, 4), rtrim(SubString(@StrValue, @Empty + 1, @Divide - @Empty - 1)));
				END
				ELSE	
				BEGIN                	
					Set @Numerator = 0;
					SET @Integer = 0;
                END

				IF (LEN(@StrValue) >= @Divide + 1)
				BEGIN                	
					Set @Denominator = Convert(decimal(16, 4), SubString(@StrValue, @Divide + 1, @Length - @Divide));
                END
				ELSE
				BEGIN
					Set @Denominator = 1;
				END	
			
				Set @ReturnValue = @Integer + (@Numerator / @Denominator);
			end
			else
			begin
				Set @Integer = Convert(decimal(16, 4), SubString(@StrValue, 1, @Empty - 1));
				IF(LEN(@StrValue) >= @Empty + 1)
				BEGIN
					Set @Numerator = Convert(decimal(16, 4), SubString(@StrValue, @Empty + 1, len(@StrValue)));
				END
				ELSE
				BEGIN
					SET @Numerator = 0;
				END

				Set @ReturnValue = @Integer + @Numerator;
			end
			
		End;
	End;

	-- Return the result of the function
	Return @ReturnValue

End
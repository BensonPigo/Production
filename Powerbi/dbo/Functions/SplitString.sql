
Create Function [dbo].[SplitString]
(
	@SplitStr nvarchar(max),
	@SplitChar nvarchar(5)
) 
RETURNS @RtnValue table 
(
Data nvarchar(50),no Int
) 
AS 
BEGIN 
Declare @Count int
Set @Count = 1

	IF @SplitStr != ''
	BEGIN

		While (Charindex(@SplitChar,@SplitStr)>0)
		Begin
			Insert Into @RtnValue (Data, no)
			Select 
				 Data = ltrim(rtrim(Substring(@SplitStr,1,Charindex(@SplitChar,@SplitStr)-1)))
				,no = @Count

			Set @SplitStr = Substring(@SplitStr,Charindex(@SplitChar,@SplitStr)+ iif(len(@SplitChar) = 0, 1, len(@SplitChar)),len(@SplitStr))
			Set @Count += 1
		End

		Insert Into @RtnValue (Data,no)
		Select Data = ltrim(rtrim(@SplitStr)),no = @Count

	END

Return
END
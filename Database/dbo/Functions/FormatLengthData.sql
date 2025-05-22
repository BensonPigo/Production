CREATE FUNCTION dbo.FormatLengthData (@Input NVARCHAR(100))
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @Result NVARCHAR(100) = @Input;

    -- 簡單格式檢查，防止無效資料處理
    IF @Input LIKE '[0-9]%Yd[0-9]%"[0-9]%'
    BEGIN
        DECLARE @part1 NVARCHAR(10);
        DECLARE @part2 NVARCHAR(10);
        DECLARE @part3 NVARCHAR(10);

        -- 位置計算
        DECLARE @posYd INT = CHARINDEX('Yd', @Input);
        DECLARE @posQuote INT = CHARINDEX('"', @Input);

        -- 擷取各段內容
        SET @part1 = SUBSTRING(@Input, 1, @posYd - 1);
        SET @part2 = SUBSTRING(@Input, @posYd + 2, @posQuote - (@posYd + 2));
        SET @part3 = SUBSTRING(@Input, @posQuote + 1, LEN(@Input) - @posQuote);

        -- 補零格式化
        SET @part1 = RIGHT('000' + @part1, 3);
        SET @part2 = RIGHT('00' + @part2, 2);
        SET @part3 = RIGHT('00' + @part3, 2);

        -- 重組格式
        SET @Result = @part1 + 'Yd' + @part2 + '"' + @part3;
    END

    RETURN @Result;
END;
GO
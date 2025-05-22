CREATE FUNCTION dbo.FormatMarkerLength (@Input NVARCHAR(100))
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @Result NVARCHAR(100) = @Input;

    -- 驗證格式是否符合：類似 4Y5-7/8+1"
    IF @Input LIKE '[0-9]%Y[0-9]%-[0-9]%/[0-9]%+[0-9]"'
    BEGIN
        DECLARE @part1 NVARCHAR(10);
        DECLARE @part2 NVARCHAR(10);
        DECLARE @part3 NVARCHAR(10);
        DECLARE @part4 NVARCHAR(10);
        DECLARE @part5 NVARCHAR(10);

        -- 解析每個部分
        -- 使用 CHARINDEX 與 SUBSTRING 模擬 Regex Group
        DECLARE @posY INT = CHARINDEX('Y', @Input);
        DECLARE @posDash INT = CHARINDEX('-', @Input);
        DECLARE @posSlash INT = CHARINDEX('/', @Input);
        DECLARE @posPlus INT = CHARINDEX('+', @Input);
        DECLARE @posQuote INT = CHARINDEX('"', @Input);

        -- 提取每段內容
        SET @part1 = SUBSTRING(@Input, 1, @posY - 1);
        SET @part2 = SUBSTRING(@Input, @posY + 1, @posDash - @posY - 1);
        SET @part3 = SUBSTRING(@Input, @posDash + 1, @posSlash - @posDash - 1);
        SET @part4 = SUBSTRING(@Input, @posSlash + 1, @posPlus - @posSlash - 1);
        SET @part5 = SUBSTRING(@Input, @posPlus + 1, @posQuote - @posPlus - 1);

        -- 格式化長度為固定寬度（補 0）
        SET @part1 = RIGHT('00' + @part1, 2);
        SET @part2 = RIGHT('00' + @part2, 2);
        SET @part3 = RIGHT('0' + @part3, 1);
        SET @part4 = RIGHT('0' + @part4, 1);
        SET @part5 = RIGHT('0' + @part5, 1);

        -- 重組格式化後的字串
        SET @Result = @part1 + 'Y' + @part2 + '-' + @part3 + '/' + @part4 + '+' + @part5 + '"';
    END

    RETURN @Result;
END;
GO
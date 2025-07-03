CREATE FUNCTION [dbo].[SplitString_Unicode]
(
    @SplitStr NVARCHAR(MAX),
    @SplitToken NVARCHAR(20)
)
RETURNS @RtnValue TABLE 
(
    Data NVARCHAR(MAX),
    No INT
)
AS
BEGIN
    DECLARE @Count INT = 1
    DECLARE @Pos INT
    DECLARE @LenToken INT = LEN(@SplitToken)

    -- 處理 Unicode 對應符號（僅符號型才做處理）
    IF ISNUMERIC(@SplitToken) = 0 AND LEN(@SplitToken) = 1
    BEGIN
        IF @SplitToken = N' ' -- 空格
        BEGIN
            DECLARE @spaceSet TABLE (sym NCHAR(1))
            INSERT INTO @spaceSet (sym)
            VALUES
                (NCHAR(160)), (NCHAR(5760)), (NCHAR(8192)), (NCHAR(8193)), (NCHAR(8194)),
                (NCHAR(8195)), (NCHAR(8196)), (NCHAR(8197)), (NCHAR(8198)), (NCHAR(8199)),
                (NCHAR(8200)), (NCHAR(8201)), (NCHAR(8202)), (NCHAR(8239)), (NCHAR(8287)),
                (NCHAR(12288)), (NCHAR(8203))
            DECLARE @sym NCHAR(1)
            DECLARE cur CURSOR FOR SELECT sym FROM @spaceSet
            OPEN cur
            FETCH NEXT FROM cur INTO @sym
            WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @SplitStr = REPLACE(@SplitStr, @sym, @SplitToken)
                FETCH NEXT FROM cur INTO @sym
            END
            CLOSE cur
            DEALLOCATE cur
        END
        ELSE IF @SplitToken = N',' BEGIN SET @SplitStr = REPLACE(@SplitStr, NCHAR(65292), @SplitToken) END
        ELSE IF @SplitToken = N';' BEGIN SET @SplitStr = REPLACE(@SplitStr, NCHAR(65307), @SplitToken) END
        ELSE IF @SplitToken = N'/' BEGIN SET @SplitStr = REPLACE(@SplitStr, NCHAR(65295), @SplitToken) END
        ELSE IF @SplitToken = N'|' BEGIN SET @SplitStr = REPLACE(@SplitStr, NCHAR(65372), @SplitToken) END
    END

    SET @SplitStr = LTRIM(RTRIM(@SplitStr))

    -- 主邏輯（支援文字或符號）
    WHILE LEN(@SplitStr) > 0
    BEGIN
        SET @Pos = CHARINDEX(@SplitToken, @SplitStr)

        IF @Pos = 0
        BEGIN
            INSERT INTO @RtnValue (Data, No) SELECT @SplitStr, @Count
            BREAK
        END

        INSERT INTO @RtnValue (Data, No)
        SELECT SUBSTRING(@SplitStr, 1, @Pos - 1), @Count

        SET @SplitStr = SUBSTRING(@SplitStr, @Pos + @LenToken, LEN(@SplitStr))
        SET @SplitStr = LTRIM(RTRIM(@SplitStr))
        SET @Count += 1
    END

    RETURN
END
GO

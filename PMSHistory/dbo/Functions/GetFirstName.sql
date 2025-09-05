CREATE FUNCTION dbo.GetFirstName
(
    @FullName NVARCHAR(100)
)
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @FirstName NVARCHAR(100)
    DECLARE @Pos INT

    SET @FullName = LTRIM(RTRIM(@FullName))
    SET @Pos = CHARINDEX(N' ', @FullName)

    IF @Pos > 0
    BEGIN
        -- 英文姓名：去掉第一個單字
        SET @FirstName = LTRIM(SUBSTRING(@FullName, @Pos + 1, LEN(@FullName)))
    END
    ELSE
    BEGIN
        -- 無空格：判斷是否純英文（只包含 A-Z/a-z）
        IF PATINDEX('%[^A-Za-z]%', @FullName COLLATE Latin1_General_BIN) = 0
        BEGIN
            -- 英文單字：不處理
            SET @FirstName = @FullName;
        END
        ELSE
        BEGIN
            -- 中文或混合：去掉第一個字
            SET @FirstName = SUBSTRING(@FullName, 2, LEN(@FullName));
        END
    END

    RETURN @FirstName
END
GO
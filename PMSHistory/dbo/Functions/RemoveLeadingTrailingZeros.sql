
CREATE FUNCTION dbo.RemoveLeadingTrailingZeros(@input NVARCHAR(MAX))
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @result NVARCHAR(MAX);

    IF @input IS NULL OR @input NOT LIKE '%[^0]%'
        SET @result = '';
    ELSE
        SET @result = SUBSTRING(
            @input,
            PATINDEX('%[^0]%', @input),
            LEN(@input) - PATINDEX('%[^0]%', @input) - PATINDEX('%[^0]%', REVERSE(@input)) + 2
        );

    RETURN @result;
END
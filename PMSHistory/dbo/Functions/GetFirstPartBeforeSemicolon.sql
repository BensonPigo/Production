
CREATE FUNCTION dbo.GetFirstPartBeforeSemicolon (
    @input NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN CASE 
               WHEN CHARINDEX(';', @input) > 0 THEN LEFT(@input, CHARINDEX(';', @input) - 1)
               ELSE @input
           END;
END
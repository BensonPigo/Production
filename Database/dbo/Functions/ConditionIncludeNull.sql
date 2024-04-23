CREATE FUNCTION dbo.ConditionIncludeNull (@value1 NVARCHAR(255), @value2 NVARCHAR(255))
RETURNS BIT
AS
BEGIN
    DECLARE @result BIT = 0;

    IF isnull(@value1, '') = isnull(@value2, '')
        SET @result = 1;

    RETURN @result;
END
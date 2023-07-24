CREATE FUNCTION dbo.ConditionIncludeNull (@value1 NVARCHAR(255), @value2 NVARCHAR(255))
RETURNS BIT
AS
BEGIN
    DECLARE @result BIT = 0;

    IF (@value1 = @value2 OR (@value1 IS NULL AND @value2 IS NULL))
        SET @result = 1;

    RETURN @result;
END
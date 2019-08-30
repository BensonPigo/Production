
CREATE FUNCTION [RemoveNumericCharacters]
(
	-- Add the parameters for the function here
	@Temp NVarChar(max)
)
RETURNS NVarChar(max)
AS
BEGIN
    Declare @NumRange as Nvarchar(100) = '%[0-9]%'
    While PatIndex(@NumRange, @Temp) > 0
        Set @Temp = Stuff(@Temp, PatIndex(@NumRange, @Temp), 1, '')

    Return @Temp

END
GO


CREATE FUNCTION IsQRCodeCreatedByPMS
(
    @checkQRCode varchar(80)
)
RETURNS bit
AS
BEGIN
    if(
        SUBSTRING(@checkQRCode,1,1) = 'F'
        and LEN(@checkQRCode) = 13
        and ISNUMERIC(SUBSTRING(@checkQRCode,2,12)) = 1
    )
    Begin
        return cast(1 as bit)
    End
    
    return cast(0 as bit)
END
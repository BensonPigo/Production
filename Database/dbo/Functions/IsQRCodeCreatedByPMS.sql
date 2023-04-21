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

    if(
        SUBSTRING(@checkQRCode,4,1) = 'F'
        and LEN(@checkQRCode) = 16
        and exists(select 1 from dbo.System where RgCode = SUBSTRING(@checkQRCode,1,3))
        and ISNUMERIC(SUBSTRING(@checkQRCode,5,12)) = 1
    )
    begin
        return cast(1 as bit)
    end
    
    return cast(0 as bit)
END
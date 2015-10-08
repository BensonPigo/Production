-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	Get Stock Unit
-- =============================================
CREATE FUNCTION dbo.getStockType
(
	-- Add the parameters for the function here
	@scirefno varchar(26),@suppid varchar(6)
)
RETURNS varchar(8)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @stockunit varchar(8);
	DECLARE @mtltypeid varchar(20);
	declare @tmpunit varchar(8);
	declare @isExt bit;
	declare @extunit varchar(8);
	set @stockunit = '';

	-- Add the T-SQL statements to compute the return value here
	SELECT @tmpunit=CASE B.OutputUnit WHEN 1 THEN A.UsageUnit WHEN 2 THEN C.POUnit END
	, @isExt = b.IsExtensionUnit
	FROM DBO.Fabric A INNER JOIN DBO.MtlType B ON A.MtlTypeID = B.ID 
	INNER JOIN DBO.Fabric_Supp C ON A.SCIRefno = C.SCIRefno 
	WHERE A.SCIRefno=@scirefno AND C.SuppID = @suppid;

	if @isExt = '1'
	begin
		select @extunit=unit.ExtensionUnit from dbo.Unit where id = @tmpunit;

		if @extunit is null or @extunit=''
		begin
			return @tmpunit;
		end
		set @stockunit = @extunit;
	end

	if @isExt = '0'
	begin
		set @stockunit = @tmpunit;
	end
	
	-- Return the result of the function
	RETURN @stockunit
END
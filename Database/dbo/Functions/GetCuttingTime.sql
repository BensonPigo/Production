
CREATE FUNCTION GetCuttingTime
(
	@Setuptime numeric(20,4),
	@TotalCuttingPerimeter numeric(20,4),
	@ActualSpeed numeric(20,4),
	@Windowtime numeric(20,4),
	@WindowNo numeric(20,4)
)
RETURNS  numeric(20,4)
AS
BEGIN
	DECLARE @CuttingTime numeric(20,4)

	set @CuttingTime = @Setuptime + iif(@ActualSpeed=0,0,@TotalCuttingPerimeter/@ActualSpeed) + @Windowtime * @WindowNo

	RETURN @CuttingTime
END
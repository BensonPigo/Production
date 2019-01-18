
CREATE FUNCTION GetSpreadingTime 
(
	@PreparationTime numeric(20,4),
	@MarkerLength numeric(20,4),
	@Changeovertime numeric(20,4),
	@NoofRoll numeric(20,4),
	@Setuptime numeric(20,4),
	@MachineSpreadingTime numeric(20,4),
	@Layer numeric(20,4),
	@NoofbSeparator numeric(20,4),
	@Dyelot int,
	@ForwardTime numeric(20,4)
)
RETURNS  numeric(20,4)
AS
BEGIN
	DECLARE @SpreadingTime numeric(20,4)

	set @SpreadingTime = @PreparationTime * @MarkerLength + 
						 @Changeovertime * @NoofRoll +
						 @Setuptime +
						 @MachineSpreadingTime * @MarkerLength * @Layer +
						 (@NoofbSeparator * @Dyelot -1) +
						 @ForwardTime

	RETURN @SpreadingTime

END
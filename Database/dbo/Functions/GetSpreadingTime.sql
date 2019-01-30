--Function SpreadingTime 
Create FUNCTION [dbo].[GetSpreadingTime] 
(
	@WeaveTypeID varchar(20),
	@Refno varchar(20), --(用以判斷Roll/UnRoll)、
	@NoOfRoll int,
	@Layer int,
	@Cons numeric(20,4),
	@Dyelot numeric(20,4)
)
RETURNS  numeric(20,4)
AS
BEGIN
	DECLARE
	@PreparationTime numeric(20,4),
	@isRoll bit,
	@Changeovertime numeric(20,4),
	@Setuptime numeric(20,4),
	@MachineSpreadingTime numeric(20,4),
	@NoOfbSeparator numeric(20,4),
	@ForwardTime numeric(20,4),
	@MarkerLength float = @cons/@Layer
	
	set @isRoll = isnull((
			select fr.IsRoll
			from ManufacturingExecution.dbo.RefnoRelaxtime rr 
			inner join ManufacturingExecution.dbo.FabricRelaxation fr on rr.FabricRelaxationID = fr.ID
			where rr.Refno = @Refno
		),0)

	select 
		@PreparationTime=PreparationTime,
		@Changeovertime=iif(@isRoll = 0,ChangeOverRollTime,ChangeOverUnRollTime),
		@Setuptime=Setuptime,
		@MachineSpreadingTime=SpreadingTime,
		@NoOfbSeparator=SeparatorTime,
		@ForwardTime = ForwardTime
	from SpreadingTime 
	where WeaveTypeID=@WeaveTypeID


	DECLARE @SpreadingTime numeric(20,4)
	set @SpreadingTime = @PreparationTime * @MarkerLength + 
						 @Changeovertime * @NoofRoll +
						 @Setuptime +
						 @MachineSpreadingTime * @cons +
						 (@NoofbSeparator * @Dyelot -1) +
						 @ForwardTime

	RETURN @SpreadingTime

END
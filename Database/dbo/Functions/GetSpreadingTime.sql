
Create FUNCTION [dbo].[GetSpreadingTime] 
(
	@MtlTypeID varchar(20),
	@Refno varchar(20), --(用以判斷Roll/UnRoll)、
	@MarkerLength numeric(20,4),--(單位：碼YDS), WorkOrder.[ConsPC] 是已經轉換過的MarkerLength
	@NoOfRoll int,
	@Layer int,
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
	@ForwardTime numeric(20,4)

	set @isRoll = (
			select fr.IsRoll
			from ManufacturingExecution.dbo.RefnoRelaxtime rr 
			inner join ManufacturingExecution.dbo.FabricRelaxation fr on rr.FabricRelaxationID = fr.ID
			where rr.Refno = @Refno
		)

	select 
		@PreparationTime=PreparationTime,
		@Changeovertime=iif(@isRoll = 0,ChangeOverRollTime,ChangeOverUnRollTime),
		@Setuptime=Setuptime,
		@MachineSpreadingTime=SpreadingTime,
		@NoOfbSeparator=SeparatorTime,
		@ForwardTime = ForwardTime
	from SpreadingTime 
	where MtlTypeID=@MtlTypeID


	DECLARE @SpreadingTime numeric(20,4)
	set @SpreadingTime = @PreparationTime * @MarkerLength + 
						 @Changeovertime * @NoofRoll +
						 @Setuptime +
						 @MachineSpreadingTime * @MarkerLength * @Layer +
						 (@NoofbSeparator * @Dyelot -1) +
						 @ForwardTime

	RETURN @SpreadingTime

END
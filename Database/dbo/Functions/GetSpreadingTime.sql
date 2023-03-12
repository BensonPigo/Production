--Function SpreadingTime 
CREATE FUNCTION [dbo].[GetSpreadingTime] 
(
	@WeaveTypeID varchar(20),
	@Refno varchar(36), --(用以判斷Roll/UnRoll)、
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
	@MarkerLength float = iif(@Layer=0,0,@cons/@Layer)
	
	set @isRoll = isnull((
			select fr.IsRoll
			from SciMES_RefnoRelaxtime rr 
			inner join SciMES_FabricRelaxation fr on rr.FabricRelaxationID = fr.ID
			where rr.Refno = @Refno
		),0)

	select 
		@PreparationTime=PreparationTime,
		@Changeovertime=iif(@isRoll = 0,ChangeOverUnRollTime,ChangeOverRollTime),
		@Setuptime=Setuptime,
		@MachineSpreadingTime=SpreadingTime,
		@NoOfbSeparator=SeparatorTime,
		@ForwardTime = ForwardTime
	from SpreadingTime 
	where WeaveTypeID=@WeaveTypeID


	DECLARE @SpreadingTime numeric(20,4)
	set @SpreadingTime = isnull(@PreparationTime * @MarkerLength,0) + 
						 isnull(@Changeovertime * @NoofRoll,0) +
						 isnull(@Setuptime,0) +
						 isnull(@MachineSpreadingTime * @cons,0) +
						 isnull(@NoofbSeparator * (@Dyelot -1),0) +
						 isnull(@ForwardTime,0)

	RETURN @SpreadingTime

END
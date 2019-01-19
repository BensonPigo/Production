
CREATE FUNCTION [dbo].[GetCuttingTime]
(
	@TotalCuttingPerimeter numeric(20,4),--(單位：公尺)、
	@CutCell varchar(2),--裁桌+
	@Layer int,--層數+
	@MtlTypeID varchar(20),--(用以抓取Actual Speed)、
	@cons numeric(20,4)
	
)
RETURNS  numeric(20,4)
AS
BEGIN
	DECLARE
	@Setuptime numeric(20,4),
	@ActualSpeed numeric(20,4),
	@Windowtime numeric(20,4),
	@WindowNo numeric(20,4),
	@MarkerLength float = @cons/@Layer * 0.9144-- = Cons/Layer (單位：碼YDS)*0.9144 轉公尺 (一層Layer上的總長)
	--Window No.(計算方式：Marker Length(單位：碼YDS)/[WindowLength]

	select @ActualSpeed = ActualSpeed
	from CuttingMachine_detail cmd
	inner join CutCell cc on cc.CuttingMachineID = cmd.id
	where cc.id = @CutCell 
	and @layer between cmd.LayerLowerBound and cmd.LayerUpperBound
	and cmd.MtlTypeID = @MtlTypeID 

	select
		@Setuptime = Setuptime,
		@Windowtime = WindowTime,
		@WindowNo = @MarkerLength/WindowLength
	from CuttingTime
	where MtlTypeID = @MtlTypeID

	DECLARE @CuttingTime numeric(20,4)
	set @CuttingTime = @Setuptime + iif(@ActualSpeed=0,0,@TotalCuttingPerimeter/@ActualSpeed) + @Windowtime * @WindowNo

	RETURN @CuttingTime
END

CREATE FUNCTION [dbo].[IsShowRFIDScan]
(
	@poid as varchar(13),
	@patternPanel as varchar(2)
)
RETURNS bit
AS
BEGIN
	DECLARE @firstPatternPanel AS varchar(2)
	Declare @RTN bit
	select top 1 @firstPatternPanel = PatternPanel
	from Order_ColorCombo 
	where id = @poid and FabricType = 'F'
	order by PatternPanel 

	IF @patternPanel = @firstPatternPanel
	SET @RTN = 1
	ELSE
	SET @RTN = 0

	RETURN @RTN
END
CREATE FUNCTION [dbo].[GetSewingLineScheduleData_Efficiency]
(
	@targetDate date,
	@targetLine varchar(10),
	@factory varchar(10)
)
RETURNS float
AS
BEGIN

/*****************************************************
	要修改，需連同MES.Production 一併修改。
	ISP20201812
*****************************************************/

	declare @TargetEff float
	select @TargetEff =
	CONVERT(FLOAT, ROUND(
		(sum(StdOutput* SewingCPU) ) / ( sum(sewer *New_WorkingTime))/ 3600 * 1400
		,4) )
		* 100
	from dbo.GetSewingLineScheduleDataBase(@targetDate,@targetDate,@targetLine,@targetLine,'',@factory, default, default, default, default, default)

	RETURN isnull(@TargetEff,0)
END

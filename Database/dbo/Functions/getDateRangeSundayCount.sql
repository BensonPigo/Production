-- =============================================
-- Author:		<Jimmy>
-- Create date: <2017/07/26>
-- Description:	<取得一段時間中有多少個星期日>
-- =============================================
CREATE FUNCTION [dbo].[getDateRangeSundayCount]
(
	@StartDate as date
	, @EndDate as date
)
RETURNS int
AS
BEGIN
	if @StartDate > @EndDate
		return 0;
	else
		declare @returnCount int = 0;
		declare @StartWeekday int = DATEPART (WEEKDAY, @StartDate) 
		declare @EndWeekday int = DATEPART (WEEKDAY, @EndDate)
		-- DATEPART(dw, [Date]) = 1 代表 星期日

		select @returnCount = count(*)
		from (
			select	top (DATEDIFF (Day, @StartDate, @EndDate) + 1) 
					[Date] = DATEADD(Day, Row_Number() over (order by c1.name)
					, DATEADD (DD, -1, @StartDate))
			from [master].[dbo].[spt_values] c1 
		)x
		where DATEPART(dw, [Date]) = 1;

		return @returnCount
END
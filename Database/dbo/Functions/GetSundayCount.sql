-- =============================================
-- Author:		Aaron
-- Create date: 2019/05/09
-- Description:	回傳傳入日期區間內星期天的數量
-- =============================================
CREATE FUNCTION [dbo].[GetSundayCount]
(
	-- Add the parameters for the function here
	@FromDate as date,
	@ToDate as date
)
RETURNS INT
AS
BEGIN
	DECLARE @Count AS int = 0;
	DECLARE @tmpDate date = @FromDate

	WHILE (@tmpDate <= @ToDate)
	BEGIN
		if(DATEPART (WEEKDAY,@tmpDate) = '1')
		begin
			set @Count = @Count + 1
		end
		set @tmpDate = DateAdd(Day,1,@tmpDate)
	END
	
	RETURN @Count;
END
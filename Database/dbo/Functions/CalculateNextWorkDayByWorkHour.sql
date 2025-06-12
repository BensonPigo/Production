CREATE FUNCTION [dbo].[CalculateNextWorkDayByWorkHour]
(
    @SDATE as Date,
    @Day as INT,
    @FactoryID as varchar(10),
	@SewingLine as varchar(5)
)
RETURNS DATE
AS
BEGIN
	IF @SDate is null
	BEGIN
		RETURN null
	END

	IF @Day = 0
	BEGIN
		RETURN @SDate
	END

	DECLARE @WORKDAY AS DATE
	SELECT @WORKDAY = w.Date
	FROM (
		SELECT RID = ROW_NUMBER() OVER(ORDER BY w.Date)
			, w.Date
		FROM Workhour_Detail w
		WHERE w.Date > @SDate
		AND w.SewingLineID = @SewingLine
		AND w.FactoryID = @FactoryID　
		GROUP BY w.Date
	) w
	WHERE w.RID = @Day
	
	RETURN @WORKDAY
END


GO

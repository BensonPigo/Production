CREATE FUNCTION [dbo].[CalculateWorkDayByWorkHour]
(
    @SDATE as Date,
    @EDATE as Date,
    @FactoryID as varchar(10),
	@SewingLine as varchar(5)
)
RETURNS INT
AS
BEGIN
	IF @SDate is null
	BEGIN
		RETURN null
	END 

	IF @EDate is null
	BEGIN
		RETURN null
	END 

	DECLARE @TDATE AS DATE
	IF @EDATE < @SDATE
	BEGIN
		SET @TDATE = @EDATE
		SET @EDATE = @SDATE
		SET @SDATE = @TDATE
	END

	DECLARE @WORKDAY AS INT

	SELECT @WORKDAY = (COUNT(DISTINCT Date) - 1)
		* IIF(@TDATE IS NULL, 1, -1)
	FROM Workhour_Detail w
	WHERE w.Date >= @SDate
	AND w.Date <= @EDate
	AND w.SewingLineID = @SewingLine
	AND w.FactoryID = @FactoryID　
	
	RETURN @WORKDAY
END

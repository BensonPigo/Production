CREATE FUNCTION GetDateRange
(	
	 @StartDate DATE ,
	 @EndDate DATE 
)
RETURNS TABLE 
AS
RETURN 
(
	WITH cte AS (
		SELECT [date] = @StartDate
		UNION ALL
		SELECT DATEADD(day , 1, [date]) FROM cte WHERE ([date] <= DATEADD(DAY,-1,@EndDate))
	)
	SELECT [date] = cast([date] as date) 
	FROM cte
)
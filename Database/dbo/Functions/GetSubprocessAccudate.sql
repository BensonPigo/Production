-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetSubprocessAccudate]
(
	@style varchar(15),
	@SubprocessLineID varchar(10),
	@maxOutputDate datetime,
	@factory varchar(8)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @accudate int

	-- Add the T-SQL statements to compute the return value here
	
	DECLARE @startdate DATETIME = dateadd(day,-90,@maxOutputDate)
	;WITH cte AS (
		SELECT [date] = @startdate
		UNION ALL
		SELECT [date] + 1 FROM cte WHERE ([date] <= DATEADD(DAY,-1,@maxOutputDate))
	)
	
	select @accudate = count(d.date)
	from cte d
	where d.date >
	(
		select max(d.date)
		from cte d 
		left join (
			select s.OutputDate
			from SubProcessOutput_Detail sd with(nolock)
			left join SubProcessOutput s with(nolock) on s.id=sd.id
			left join Orders o with(nolock) on o.id = sd.orderid
			where s.Status='Confirmed' and sd.SubprocessLineID = @SubprocessLineID and o.StyleID = @style
		) t on d.date = t.OutputDate
		where t.OutputDate is null
	)
	and not exists(select 1 from holiday h with(nolock) where d.date = h.holidaydate and h.FactoryID = @factory)
	and DATEPART(WEEKDAY, d.date-1) != 7

	-- Return the result of the function
	RETURN @accudate

END
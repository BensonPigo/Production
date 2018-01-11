CREATE FUNCTION [dbo].[CalculateWorkDate]
(
	@StartDate as Date,
	@CalculateDate as int,
	@FactoryID as varchar(10)
)
RETURNS Date
AS
BEGIN
	Declare @returnDate Date = @StartDate;
	Declare @countDate int = 0;
	Declare @computeDate int = iif (@CalculateDate > 0, 1, -1)
	Declare @diffDate int = abs(@CalculateDate)

	if (@FactoryID = '' or @FactoryID is null)
	begin
		while (@countDate < @diffDate)
		begin
			set @returnDate = DATEADD(Day, @computeDate, @returnDate)

			if (DATEPART(WEEKDAY, @returnDate) != 1
				and @returnDate not in (select HolidayDate
										from Holiday))
			begin
				set @countDate = @countDate + 1
			end
		end
	end
	else
	begin
		while (@countDate < @diffDate)
		begin
			set @returnDate = DATEADD(Day, @computeDate, @returnDate)

			if (DATEPART(WEEKDAY, @returnDate) != 1
				and @returnDate not in (select HolidayDate
										from Holiday
										where FactoryID = @FactoryID))
			begin
				set @countDate = @countDate + 1
			end
		end
	end;

	
	
	RETURN @returnDate;
END
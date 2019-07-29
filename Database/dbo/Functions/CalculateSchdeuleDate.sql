CREATE FUNCTION [dbo].[CalculateSchdeuleDate]
(
    @EndDate as Date,
    @DiffDate as int = 0,
    @FactoryID as varchar(10),
	@isHoliday as bit = 0
)
RETURNS Date
AS
BEGIN
	if @EndDate is null
	begin
		return null
	end 

    DECLARE @StartDate as Date 
	DECLARE @HolidayB as int = 0, @HolidayE as int = 1 
	DECLARE @DiffDateCh as int 

	set @DiffDateCh = @DiffDate
	select @StartDate = dateadd(d, @DiffDate * -1, @EndDate) 

	if (@FactoryID = '' or @FactoryID is null)
	begin 
		while @HolidayB <> @HolidayE
		begin
			set @HolidayB = @HolidayE

			;WITH WorkDays
			AS(
				SELECT @startDate AS WorkDay, @DiffDateCh AS DiffDays
				UNION ALL
				SELECT DATEADD(dd, 1, WorkDay),DiffDays -1
				FROM WorkDays wd
				WHERE DiffDays > 0
			),
			cntHoliday as (
				SELECT count(*) cntHoliday
				FROM WorkDays wd
				WHERE DATEPART(dw, WorkDay ) = 1
				or exists (SELECT * FROM Holiday h WHERE h.HolidayDate = wd.WorkDay and @isHoliday = 1)
			)
			select @HolidayE =  cntHoliday
			from cntHoliday  

			set @DiffDateCh = @DiffDate + @HolidayE
			select @StartDate = dateadd(d, @DiffDateCh * -1, @EndDate) 
		end  
	end
	else
	begin 
		while @HolidayB <> @HolidayE
		begin
			set @HolidayB = @HolidayE

			;WITH WorkDays
			AS(
				SELECT @startDate AS WorkDay, @DiffDateCh AS DiffDays
				UNION ALL
				SELECT DATEADD(dd, 1, WorkDay),DiffDays -1
				FROM WorkDays wd
				WHERE DiffDays > 0
			),
			cntHoliday as (
				SELECT count(*) cntHoliday
				FROM WorkDays wd
				WHERE DATEPART(dw, WorkDay ) = 1
				or exists (SELECT * FROM Holiday h WHERE h.HolidayDate = wd.WorkDay and h.FactoryID = @FactoryID and @isHoliday = 1)
			)
			select @HolidayE =  cntHoliday
			from cntHoliday  

			set @DiffDateCh = @DiffDate + @HolidayE 
			select @StartDate = dateadd(d, @DiffDateCh * -1, @EndDate) 
		end  
	end
	
	return dateadd(d, @DiffDateCh * -1, @EndDate)
END
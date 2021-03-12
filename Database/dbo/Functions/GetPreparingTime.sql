-- =============================================
-- Create date: <Create Date,,>
-- Description:	Get Preparing Time
-- =============================================
CREATE FUNCTION GetPreparingTime
(	
	@StartDate DateTime,
	@EndDate DateTime,
	@M varchar(10)
)
RETURNS TABLE 
AS
RETURN 
(
	-- 遞迴取得自製月曆
	WITH CTE AS
	(
		SELECT [Dates] = @StartDate,[EndDate] = @EndDate
		UNION ALL --注意這邊使用 UNION ALL
		SELECT DATEADD(DAY,1,Dates),@EndDate
		FROM CTE 
		WHERE DATEADD(DAY,1,Dates) <= @EndDate --判斷是否目前遞迴月份小於結束日期	
	),
	CalendarCTE as
	(
		SELECT [Dates] = iif( convert(date, CTE.Dates) = convert(date,@EndDate),@EndDate,CTE.Dates)
		FROM CTE
	),
	Calendar as
	(
		select * 
		from CalendarCTE a
		where 1=1
		and not exists(
			select dates
			from CalendarCTE			
			where (DATEPART(WEEKDAY, Dates) = 1  --只能是星期天
			or exists(	
					select 1 from Holiday 
					where HolidayDate = CONVERT(date, Dates) 
					and exists(
						select 1 from Factory f
						where f.id = Holiday.FactoryID
						and f.MDivisionID = @M
					)
				)
			) -- 只能是假日)		
			and Dates = a.Dates
		)
	)
	
	select c.Dates,WCalendar.*,wcWorking.BeginTime,wcWorking.EndTime
	,ttlminute.minute
	,WorkHour = iif(CONVERT(varchar,ttlminute.minute % 60) = 0, CONVERT(varchar, ttlminute.minute / 60),CONVERT(varchar, ttlminute.minute / 60) + ':' +  iif( len(CONVERT(varchar,ttlminute.minute % 60)) = 1, '0'+ CONVERT(varchar,ttlminute.minute % 60),CONVERT(varchar,ttlminute.minute % 60))) 
	from Calendar c
	outer apply(
		select StartDate = MAX(wc.StartDate),wc.MDivision
		from WHWorkingCalendar wc 
		where c.Dates >= wc.StartDate
		group by wc.MDivision
	)WCalendar
	outer apply(
		select * from WHWorkingCalendar wc
		where wc.MDivision = WCalendar.MDivision
		and wc.StartDate = WCalendar.StartDate
	)wcWorking
	 outer apply(
		select [minute] = case when CONVERT(date,c.Dates) = CONVERT(date,@StartDate) then  DATEDIFF(MINUTE,CONVERT(time,@StartDate),SUBSTRING(s.EndTime,1,2)+':'+SUBSTRING(s.EndTime,3,2))
							   when (convert(date,c.Dates) = CONVERT(date,@EndDate)) then DATEDIFF(MINUTE,SUBSTRING(s.BeginTime,1,2)+':'+SUBSTRING(s.BeginTime,3,2),CONVERT(time,@EndDate))
		else DATEDIFF(MINUTE,SUBSTRING(s.BeginTime,1,2)+':'+SUBSTRING(s.BeginTime,3,2),SUBSTRING(s.EndTime,1,2)+':'+SUBSTRING(s.EndTime,3,2)) end
		from WHWorkingCalendar s
		where s.MDivision = WCalendar.MDivision
		and s.StartDate = WCalendar.StartDate
	 )ttlminute
)
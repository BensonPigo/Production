-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[getSewingOutputCumulateOfDays]
(	
	-- Add the parameters for the function here
	@style VARCHAR(15), @sewingline VARCHAR(2), @outputdate DATE, @factory VARCHAR(8)
)
RETURNS TABLE 
AS
RETURN 
(
	with SewingDates as (
		select s.OutputDate
		from SewingOutput s WITH (NOLOCK)
		inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
		left join Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
		left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
		where (o.StyleID = @style or mo.StyleID = @style)
		and s.SewingLineID = @sewingline
		and  s.OutputDate between dateadd(day,-365,@outputdate) and  @outputdate
		and s.FactoryID = @factory
	), wkHour as(
		select top 360  w.Hours, w.Date
		from WorkHour w WITH (NOLOCK)
		where w.FactoryID = @factory
		and w.SewingLineID = @sewingline
		and w.Date between dateadd(day,-365,@outputdate) and  @outputdate
		--and w.Date <= @outputdate	
		order by w.date desc
	)
	select cumulate = IIF(Count(wkHour.Date)=0,1,Count(wkHour.Date))
	from wkHour
	where wkHour.Date >  (select max(date) 
						from wkHour a 
						--left join SewingDates b on a.Date =b.OutputDate 
						--where b.OutputDate is null and a.Hours <> 0

						where --exists( select 1 from SewingDates b where a.Date =b.OutputDate and  a.Hours <> 0)
								 (a.Hours <> 0 and not exists(select 1 from SewingDates b where a.Date =b.OutputDate  ) )

					)
)
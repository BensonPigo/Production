-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[getSewingOutputCumulateOfDays]
(	
	@style VARCHAR(15), @sewingline VARCHAR(2), @outputdate DATE, @factory VARCHAR(8)
)
RETURNS TABLE 
AS
RETURN 
(
	with stmp as (
		select distinct s.OutputDate
		from SewingOutput s WITH (NOLOCK)
		inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
		left join Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
		left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
		where (o.StyleID = @style or mo.StyleID = @style)
		and s.SewingLineID = @sewingline
		and s.OutputDate between dateadd(day,-90,@outputdate) and  @outputdate
		and s.FactoryID = @factory
	), wtmp as(
		select top 360 w.Hours, w.Date
		from WorkHour w WITH (NOLOCK)
		where w.FactoryID = @factory
		and w.SewingLineID = @sewingline
		and w.Date between dateadd(day,-90,@outputdate) and  @outputdate
		and w.Holiday=0
		and isnull(w.Hours,0) != 0
	)
	select cumulate = IIF(Count(1)=0, 1, Count(1))
	from stmp s
	where s.OutputDate >(
							select date = max(Date)
							from wtmp w 
							left join stmp s on s.OutputDate = w.Date
							where s.OutputDate is null
						)
)
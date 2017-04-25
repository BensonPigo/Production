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
	select cumulate = IIF(Count(w.Date)=0,1,Count(w.Date))
	from WorkHour w WITH (NOLOCK)
	where w.FactoryID = @factory
	and w.SewingLineID = @sewingline
	and w.Date <= @outputdate
	and w.Date > 
	(
		select max(w.Date)
		from WorkHour w WITH (NOLOCK)
		outer apply(
			select OutputDate
			from SewingOutput s
			left join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
			left join  Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
			left join  MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId 
			where (o.StyleID = @style or mo.StyleID = @style)
			and s.SewingLineID = w.SewingLineID
			and s.FactoryID = w.FactoryID
			and s.OutputDate = w.Date 
		)a
		where w.FactoryID = @factory
		and w.SewingLineID = @sewingline
		and w.Date between dateadd(day,-180,@outputdate) and  @outputdate
		and(w.Hours = 0 or a.OutputDate is null)
	)
)
-- =============================================
-- Author:		JIMMY
-- Create date: 2017/05/06
-- Description:	<SP#>
-- =============================================
CREATE FUNCTION [dbo].[getDailystdq]
(
	@tSP varchar(20)
)
RETURNS Table
AS
Return
(
	--step 1 計算 AccuStdQ 
	----須排除假日 1. Sunday 2. 有列在 Holiday 的日期
	with temp as (
		select	ss.ID
				, ss.OrderID
				, Inline = Convert(char(10), ss.Inline, 120)
				, Offline = Convert(char(10), ss.Offline, 120)
				, wh.Date
				, mHours = maxHours.value
				, ss.AlloQty
				, ss.StandardOutput
				, baseStdQ = Round(maxHours.value * ss.StandardOutput, 0)
				, AccuStdQ = sum(maxHours.value * ss.StandardOutput) over (partition by ss.id order by ss.id, wh.Date)
				, ss.ComboType
		from SewingSchedule ss
		inner join WorkHour wh on	wh.SewingLineID = ss.SewingLineID 
									and wh.FactoryID = ss.FactoryID
									and wh.date between Convert(char(10), ss.Inline, 120) and Convert(char(10), ss.Offline, 120)
		left join Holiday h on	h.HolidayDate = CONVERT(char(10), wh.Date, 120)
								and h.FactoryID = wh.FactoryID
		outer apply (
			select value = (select max(mHour.Hours)
							from SewingSchedule mss
							inner join WorkHour mHour on	mHour.SewingLineID = mss.SewingLineID 
															and mHour.FactoryID = mss.FactoryID
															and mHour.date between Convert(char(10), mss.Inline, 120) and Convert(char(10), mss.Offline, 120)
							where	ss.ID = mss.ID)
		) maxHours
		where	ss.OrderID = @tSP
				and (ss.Offline is not null and ss.Offline != '')
				and DATEPART(DW, wh.Date) != 1
				and h.HolidayDate is null
	)
	--step 2 判斷 AccuStdQ 是否大於 AlloQty，計算 StdQ
	, temp2 as (
		select	t.Date
				, StdQ = StdQ.value
				, t.ComboType
		from temp t
		outer apply (
			select value = IIF(AccuStdQ > AlloQty, IIF(AccuStdQ - t.baseStdQ <= 0, t.baseStdQ
																				 , IIF(t.baseStdQ + (AlloQty - AccuStdQ) < 0, 0
																						  							   	    , t.baseStdQ + (AlloQty - AccuStdQ)))
												 , t.baseStdQ)
		) StdQ
	)
	--step3 計算每一天同 ComboType 的產量
	select	t.Date
			, StdQ = sum(StdQ)
	from temp2 t
	group by t.Date, t.ComboType
)
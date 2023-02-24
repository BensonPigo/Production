-- =============================================
-- Author:		Jeff
-- Create date: 2023/02/16
-- Description:	From trade 只取需要部份 for < WH P01 Material Compare >
-- =============================================
Create FUNCTION [dbo].[GetThreadReplace] 
(
	@SuppID			varchar(6)
	, @SCIRefno		varchar(30)
	, @SuppColor	varchar(8)
	, @CFMDate		date
)
RETURNS TABLE 
AS
RETURN 
(
	select top 1 tr.FromSCIRefno, tr.FromSuppColor, trdd.ToSCIRefno, trdd.ToBrandColorID, trdd.ToBrandSuppColor
	from Production.dbo.Thread_Replace tr
	left join Production.dbo.Thread_Replace_Detail trd on tr.Ukey = trd.Thread_ReplaceUkey
	left join Production.dbo.Thread_Replace_Detail_Detail trdd on trd.Ukey = trdd.Thread_Replace_DetailUkey
	where 
		(isnull(trdd.SuppID, '') = '' or trdd.SuppID = @SuppID)
		and tr.FromSCIRefno = @SCIRefno 
		and tr.FromSuppColor = @SuppColor
		and @CFMDate between StartDate and isnull(EndDate, '9999/12/31')
	order by trd.StartDate DESC
)
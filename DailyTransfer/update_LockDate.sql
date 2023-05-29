-- =============================================
-- Author:		<Author,,Name>
-- Create date: <2016/11/08>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE  Update_LockDate	
AS
BEGIN

declare @Lockdate date = (select sewlock from Trade_To_Pms.dbo.TradeSystem WITH (NOLOCK))

declare @PullOutLock date = (select PullOutLock from Trade_To_Pms.dbo.TradeSystem WITH (NOLOCK))


-- SewingOutput/Cutting
update SewingOutput 
set LockDate = CONVERT(date, GETDATE()), Status='Locked'
where (OutputDate < = @Lockdate and LockDate is null)
   or (OutputDate < = @Lockdate and Status = 'Sent')

update CuttingOutput 
set Lock = CONVERT(date, GETDATE()), Status='Locked'
where cDate <= @Lockdate
and Lock is null and Status='Confirmed'


update System set SewLock=@Lockdate


if (select cnt = count(1) from SewingMonthlyLock) <= 0
begin 
	insert into SewingMonthlyLock(FactoryID)	
	select ID from Factory where Junk = 0
end


update SewingMonthlyLock
   set LockDate=@Lockdate
     , EditName = 'SCIMIS'
	, EditDate = getdate() 

--PullOut
select * 
into #tmpPullout
from PullOut
where PulloutDate <= @PullOutLock 
and LockDate is null 
and Status='Confirmed'  

-- Pullout
update t
set LockDate=CONVERT(date, GETDATE()), Status='Locked'
from PullOut t
where exists(
	select 1 from #tmpPullout s
	where t.ID = s.ID
)

-- PackingList
update t
set PulloutStatus = 'Locked'
from PackingList t
where exists(
	select 1 from #tmpPullout s
	where t.PulloutID = s.ID
)

drop table #tmpPullout                                      

update System set PullLock=@PullOutLock


END

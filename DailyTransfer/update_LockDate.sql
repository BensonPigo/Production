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
where OutputDate < = @Lockdate
and LockDate is null 

update CuttingOutput 
set Lock = CONVERT(date, GETDATE()), Status='Locked'
where cDate <= @Lockdate
and Lock is null and Status='Confirmed'


update System set SewLock=@Lockdate

--PullOut
update PullOut
set LockDate=CONVERT(date, GETDATE()), Status='Locked'
where PulloutDate <= @PullOutLock and LockDate is null and Status='Confirmed'  
                                      

update System set PullLock=@PullOutLock


END

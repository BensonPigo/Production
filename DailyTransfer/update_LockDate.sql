-- =============================================
-- Author:		<Author,,Name>
-- Create date: <2016/11/08>
-- Description:	<Description,,>
-- =============================================
Alter PROCEDURE  Update_LockDate	
AS
BEGIN

declare @Lockdate date = (select sewlock from Trade_To_Pms.dbo.TradeSystem WITH (NOLOCK))

declare @PullOutLock date = (select PullOutLock from Trade_To_Pms.dbo.TradeSystem WITH (NOLOCK))


-- SewingOutput/Cutting
update SewingOutput 
set LockDate = CONVERT(date, GETDATE())
where OutputDate < = @Lockdate
and LockDate is null

update CuttingOutput 
set Lock = CONVERT(date, GETDATE())
where cDate <= @Lockdate
and Lock is null and [Status]='Lock'

update System set SewLock=@Lockdate

--PullOut
update PullOut
set LockDate=CONVERT(date, GETDATE())
where PulloutDate <= @PullOutLock and LockDate is null and Status='Locked'                                          

update System set PullLock=@PullOutLock


END

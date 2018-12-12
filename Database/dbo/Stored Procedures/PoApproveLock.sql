-- =============================================
-- Author:		ALGER
-- Create date: 2016/11/04
-- Description:	daily schedule:PoApproveLock
-- =============================================
CREATE PROCEDURE [dbo].[PoApproveLock]

AS
BEGIN

	SET NOCOUNT ON;

    declare @poapprovename varchar(10),
		@poapproveday tinyint

	select @poapprovename = POApproveName,@poapproveday = POApproveDay from System

	IF @poapprovename  != ''
	BEGIN 
		update LocalPO set LockName = @poapprovename, LockDate = GETDATE(),ApvName = @poapprovename, ApvDate = GETDATE(), Status = 'Approved'
		where ApvDate is null and IssueDate <= DATEADD(DAY,0-@poapproveday,GETDATE()) and Status in ('NEW','Locked')

		update ArtworkPO set LockName = @poapprovename, LockDate = GETDATE(),ApvName = @poapprovename, ApvDate = GETDATE(), Status = 'Approved'
		where ApvDate is null and IssueDate <= DATEADD(DAY,0-@poapproveday,GETDATE()) and Status in ('NEW','Locked')

		update Machine.dbo.MiscPO set LockName = @poapprovename, LockDate = GETDATE(),Approve = @poapprovename, ApproveDate = GETDATE(), Status = 'Approved'
		where ApproveDate is null and CDate <= DATEADD(DAY,0-@poapproveday,GETDATE()) and PurchaseFrom = 'L' and Status in ('NEW','Locked')
	END

END

GO



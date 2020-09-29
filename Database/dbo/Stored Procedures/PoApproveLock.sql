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
		@poapproveday tinyint,
		@MiscPOApproveName varchar(10),
		@MiscPOApproveDay tinyint

	select	@poapprovename = POApproveName,
			@poapproveday = POApproveDay ,
			@MiscPOApproveName = MiscPOApproveName,
			@MiscPOApproveDay = MiscPOApproveDay 
			from System

	IF @poapprovename  != ''
	BEGIN 
		update LocalPO set LockName = @poapprovename, LockDate = GETDATE(),ApvName = @poapprovename, ApvDate = GETDATE(), Status = 'Approved'
		where ApvDate is null and IssueDate <= DATEADD(DAY,0-@poapproveday,GETDATE()) and Status in ('NEW','Locked')

		update ArtworkPO set LockName = @poapprovename, LockDate = GETDATE(),ApvName = @poapprovename, ApvDate = GETDATE(), Status = 'Approved'
		where ApvDate is null and IssueDate <= DATEADD(DAY,0-@poapproveday,GETDATE()) and Status in ('NEW','Locked')
		
	END

	if @MiscPOApproveName != ''
	begin
		update SciMachine_MiscPO set LockName = @MiscPOApproveName, LockDate = GETDATE(),Approve = @MiscPOApproveName, ApproveDate = GETDATE(), Status = 'Approved'
			where ApproveDate is null and CDate <= DATEADD(DAY,0-@MiscPOApproveDay,GETDATE()) and PurchaseFrom = 'L' and Status in ('NEW','Locked')
	end

END

GO



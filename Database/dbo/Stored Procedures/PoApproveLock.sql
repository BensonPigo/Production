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

	update LocalPO set ApvName = @poapprovename, ApvDate = GETDATE()
	where ApvDate is null and IssueDate <= DATEADD(DAY,0-@poapproveday,GETDATE())

	update ArtworkPO set ApvName = @poapprovename, ApvDate = GETDATE()
	where ApvDate is null and IssueDate <= DATEADD(DAY,0-@poapproveday,GETDATE())

	update Machine.dbo.MiscPO set Approve = @poapprovename, ApproveDate = GETDATE()
	where ApproveDate is null and CDate <= DATEADD(DAY,0-@poapproveday,GETDATE()) and PurchaseFrom = 'L'


END

GO



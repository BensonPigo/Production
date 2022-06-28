-- =============================================
-- Description:	<imp_ChangeKPILETARequest>
-- =============================================
CREATE PROCEDURE [imp_ChangeKPILETARequest]
	
AS
BEGIN

	declare @M table(id varchar(10)) --Mdivision¥N½X
	insert @M select distinct MDivisionID from Production.dbo.Factory

	Merge Production.dbo.ChangeKPILETARequest as t
	Using (select * from Trade_To_Pms.dbo.ChangeKPILETARequest WITH (NOLOCK) 
			where MdivisionID in (select id from @M))as s
	on t.id=s.id 
	When matched  then
		update set
		t.Status =			    isnull(s.Status,              ''),
		t.ConfirmedNewKPILETA =	s.ConfirmedNewKPILETA,
		t.ApproveName =			isnull(s.ApproveName,         ''),
		t.ApproveDate =			s.ApproveDate,
		t.ConfirmName =			isnull(s.ConfirmName,         ''),
		t.ConfirmDate =			s.ConfirmDate,
		t.TPERemark =			isnull(s.TPERemark,           ''),
		t.TPEEditName =			isnull(s.TPEEditName,         ''),
		t.TPEEditDate =			s.TPEEditDate 
		;		                                            
END
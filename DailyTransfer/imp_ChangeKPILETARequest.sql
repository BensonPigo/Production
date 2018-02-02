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
		t.TPEConfirmName =	s.TPEConfirmName,
		t.TPEConfirmDate =	s.TPEConfirmDate,
		t.TPEEditName =		s.TPEEditName,
		t.TPEEditDate =		s.TPEEditDate,
		t.TPERemark =		s.TPERemark,
		t.Status =			s.Status;		
END
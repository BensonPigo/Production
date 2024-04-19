-- =============================================
-- Description:	<imp_ChangeKPILETARequest>
-- =============================================
CREATE PROCEDURE [imp_ChangeKPILETARequest]
	
AS
BEGIN
	---- Check TransferDate before everything
	IF NOT  EXISTS(
		select 1 from Trade_To_PMS..DateInfo 
		where Name = 'TransferDate'
		AND DateStart in (CAST(DATEADD(DAY,-1,GETDATE()) AS date), CAST(GETDATE() AS DATE))
	)
	BEGIN
		-- �ߥX���~
		RAISERROR ('The DB transferdate is wrong. Trade_To_PMS..DateInfo  �����s�b�ŦX���� TransferDate �O���C', 16, 1); -- 16�O���~���Y���{�סA1�O���~���A	
		RETURN; 
	END

	declare @M table(id varchar(10)) --Mdivision�N�X
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
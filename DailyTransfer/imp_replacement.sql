
-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/19>
-- Description:	<import Replacement>
-- =============================================
Create PROCEDURE [dbo].[imp_Replacement]
	
AS
BEGIN

	declare @Sayfty table(id varchar(10)) --¤u¼t¥N½X
	insert @Sayfty select id from Production.dbo.Factory

	declare  @tReplace table (id varchar(13))

	--Merge Replace1
	Merge Production.dbo.ReplacementReport as t
	Using (select * from Trade_To_Pms.dbo.ReplacementReport WITH (NOLOCK) where factoryid in (select id from @Sayfty))as s
	on t.id=s.id 
		When matched then
		update set
		t.TPECFMName = s.TPECFMName,
		t.TPECFMDate =s.TPECFMDate,
		t.TPEEditName=s.EditName,
		t.TPEEditDate=s.EditDate,
		t.Status = s.Status,
		t.RMtlAmt = iif(s.LockDate is not null,t.RMtlAmt, s.RMtlAmt),
		t.ActFreight = iif(s.LockDate is not null, t.ActFreight,s.ActFreight),
		t.EstFreight = iif(s.LockDate is not null, t.EstFreight,s.EstFreight),
		t.SurchargeAmt = iif(s.LockDate is not null, t.SurchargeAmt,s.SurchargeAmt),
		t.LockDate = s.LockDate,
		t.CompleteDate = s.CompleteDate,
		t.TransferName = s.TransferName,
		t.TransferDate = s.TransferDate,
		t.TransferResponsible = s.TransferResponsible,
		t.TransferNo = s.TransferNo
	output inserted.id into @tReplace;

	--Merge Replace2
	Merge Production.dbo.ReplacementReport_Detail as t
	Using (select * from Trade_To_Pms.dbo.ReplacementReport_Detail WITH (NOLOCK) where id in (select id from @tReplace)) as s
	on t.ukey=s.ukey
		when matched then
		update set
	    t.EstInQty = s.EstInQty,
		t.ActInQty = s.ActInQty, 
		t.AGradeRequest = s.AGradeRequest, 
		t.BGradeRequest = s.BGradeRequest, 
		t.NarrowRequest = s.NarrowRequest, 
		t.TotalRequest = s.TotalRequest, 
		t.AfterCuttingRequest = s.AfterCuttingRequest,
		t.Junk = s.Junk,
		t.NewSeq1 = s.NewSeq1,
		t.NewSeq2 = s.NewSeq2,
		t.PurchaseID = s.PurchaseID
		;

	Merge Production.dbo.Clip as t
	Using Trade_To_Pms.dbo.Clip as s
	on t.Pkey=s.Pkey
	when matched then
		update set
			 t.[TableName]	=s.[TableName]
			,t.[UniqueKey]	=s.[UniqueKey]
			,t.[SourceFile]	=s.[SourceFile]
			,t.[Description]=s.[Description]
			,t.[AddName]	=s.[AddName]
			,t.[AddDate]	=s.[AddDate]
	when not matched by target then
	insert ([PKey],[TableName],[UniqueKey],[SourceFile],[Description],[AddName],[AddDate]	)
	values(s.[PKey],s.[TableName],s.[UniqueKey],s.[SourceFile],s.[Description],s.[AddName],s.[AddDate])
	;
END






-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/19>
-- Description:	<import Replacement>
-- =============================================
Create PROCEDURE [dbo].[imp_Replacement]
	
AS
BEGIN

	---- Check TransferDate before everything
	IF NOT  EXISTS(
		select 1 from Trade_To_PMS..DateInfo 
		where Name = 'TransferDate'
		AND DateStart in (CAST(DATEADD(DAY,-1,GETDATE()) AS date), CAST(GETDATE() AS DATE))
	)
	BEGIN
		-- 拋出錯誤
		RAISERROR ('The DB transferdate is wrong. Trade_To_PMS..DateInfo  中不存在符合條件的 TransferDate 記錄。', 16, 1); -- 16是錯誤的嚴重程度，1是錯誤狀態	
		RETURN; 
	END
	

	declare @Sayfty table(id varchar(10)) --工廠代碼
	insert @Sayfty select id from Production.dbo.Factory

	declare  @tReplace table (id varchar(13))

	--Merge Replace1
	Merge Production.dbo.ReplacementReport as t
	Using (select * from Trade_To_Pms.dbo.ReplacementReport WITH (NOLOCK) where factoryid in (select id from @Sayfty))as s
	on t.id=s.id 
		When matched then
		update set
		t.TPECFMName = isnull( s.TPECFMName,''),
		t.TPECFMDate = s.TPECFMDate,
		t.TPEEditName= isnull(s.EditName,   ''),
		t.TPEEditDate= s.EditDate,
		t.Status = isnull( s.Status,        ''),
		t.RMtlAmt = isnull(iif(t.LockDate is not null,t.RMtlAmt, s.RMtlAmt),0),
		t.ActFreight = isnull(iif(t.LockDate is not null, t.ActFreight,s.ActFreight),0),
		t.EstFreight = isnull(iif(t.LockDate is not null, t.EstFreight,s.EstFreight),0),
		t.SurchargeAmt = isnull(iif(t.LockDate is not null, t.SurchargeAmt,s.SurchargeAmt),0),
		t.LockDate = s.LockDate,
		t.CompleteDate = s.CompleteDate,
		t.TransferName = isnull( s.TransferName,''),
		t.TransferDate = s.TransferDate,
		t.TransferResponsible = isnull( s.TransferResponsible,''),
		t.TransferNo = isnull( s.TransferNo,''),
		t.CompleteName = isnull (s.CompleteName, ''),
		t.isComplete = isnull (s.isComplete, 0)
	output inserted.id into @tReplace;

	--Merge Replace2
	Merge Production.dbo.ReplacementReport_Detail as t
	Using (select * from Trade_To_Pms.dbo.ReplacementReport_Detail WITH (NOLOCK) where id in (select id from @tReplace)) as s
	on t.ukey=s.ukey AND t.ID = s.ID
		when matched then
		update set
	    t.EstInQty = isnull( s.EstInQty,                       0),
		t.ActInQty = isnull( s.ActInQty,                       0),
		t.AGradeRequest = isnull( s.AGradeRequest,             0),
		t.BGradeRequest = isnull( s.BGradeRequest,             0),
		t.NarrowRequest = isnull( s.NarrowRequest,             0),
		t.TotalRequest = isnull( s.TotalRequest,               0),
		t.AfterCuttingRequest = isnull( s.AfterCuttingRequest, 0),
		t.Junk = isnull( s.Junk,                               0),
		t.NewSeq1 = isnull( s.NewSeq1,                         ''),
		t.NewSeq2 = isnull( s.NewSeq2,                         ''),
		t.PurchaseID = isnull( s.PurchaseID,                   '')
		;

	----Clip
	update t
	 set	 t.[TableName]	=isnull(s.[TableName], '')
			,t.[UniqueKey]	=s.[UniqueKey]
			,t.[SourceFile]	=isnull(s.[SourceFile], '')
			,t.[Description]=isnull(s.[Description], '')
			,t.[AddName]	=isnull(s.[AddName], '')
			,t.[AddDate]	=s.[AddDate]
	from Production.dbo.Clip t
	inner join Trade_To_Pms.dbo.Clip s ON t.Pkey = s.Pkey AND  t.UniqueKey = s.UniqueKey 
	;
	INSERT INTO Production.dbo.Clip ([PKey],[TableName],[UniqueKey],[SourceFile],[Description],[AddName],[AddDate]	)
	SELECT [PKey],[TableName],[UniqueKey],[SourceFile],[Description],[AddName],[AddDate]	
	FROM Trade_To_Pms.dbo.Clip s
	WHERE NOT EXISTS ( select 1 from Production.dbo.Clip t WHERE t.Pkey = s.Pkey AND  t.UniqueKey = s.UniqueKey )

END





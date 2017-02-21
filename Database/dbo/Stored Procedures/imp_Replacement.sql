
-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/19>
-- Description:	<import Replacement>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Replacement]
	
AS
BEGIN

	declare @Sayfty table(id varchar(10)) --�u�t�N�X
	insert @Sayfty select id from Production.dbo.Factory

	declare  @tReplace table (id varchar(13))

	--Merge Replace1
	Merge Production.dbo.ReplacementReport as t
	Using (select * from Trade_To_Pms.dbo.ReplacementReport WITH (NOLOCK) where factoryid in (select id from @Sayfty))as s
	on t.id=s.id 
		When matched and t.TPECFMDate <> s.TPECFMDate then
		update set
		t.TPECFMName = s.TPECFMName,
		t.TPECFMDate =s.TPECFMDate,
		t.TPEEditName=s.EditName,
		t.TPEEditDate=s.EditDate
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
		t.AfterCuttingRequest = s.AfterCuttingRequest;


END
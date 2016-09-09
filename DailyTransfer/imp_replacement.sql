
-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/19>
-- Description:	<import Replacement>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Replacement]
	
AS
BEGIN

	declare @Sayfty table(id varchar(10)) --¤u¼t¥N½X
	insert @Sayfty select id from Production.dbo.Factory


-- update Production.dbo.ReplacementReport
	update Production.dbo.ReplacementReport
	set TPECFMName=tr1.TPECFMName,
	TPECFMDate=tr1.TPECFMDate,
	TPEEditName=tr1.EditName,
	TPEEditDate=tr1.EditDate
	from Trade_To_Pms.dbo.ReplacementReport tr1
	inner join Production.dbo.ReplacementReport pr1 on tr1.id=pr1.ID
	where tr1.TPECFMDate <> pr1.TPECFMDate
	and tr1.FactoryID in (select id from @Sayfty)

-- update Production.dbo.ReplacementReport_Detail
	update Production.dbo.ReplacementReport_Detail
	set EstInQty = tr2.EstInQty,
	ActInQty = tr2.ActInQty, 
	AGradeRequest = tr2.AGradeRequest, 
	BGradeRequest = tr2.BGradeRequest, 
	NarrowRequest = tr2.NarrowRequest, 
	TotalRequest = tr2.TotalRequest, 
	AfterCuttingRequest = tr2.AfterCuttingRequest
	from Trade_To_Pms.dbo.ReplacementReport_Detail tr2
	inner join Production.dbo.ReplacementReport_Detail pr2 on tr2.id=pr2.id
	inner join Trade_To_Pms.dbo.ReplacementReport tr1 on tr1.id=tr2.id
	where tr1.FactoryID in (select id from @Sayfty)


END





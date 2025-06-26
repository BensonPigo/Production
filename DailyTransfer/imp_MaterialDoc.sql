Create PROCEDURE  [dbo].[imp_MaterialDoc]
AS
BEGIN
	SET NOCOUNT ON;
	/*UASentReport */
	-------UPDATE
	UPDATE a
	SET 
	--BrandRefno
	--ColorID
	--SuppID
	--DocumentName
	--BrandID
	a.TestSeasonID = ISNULL(b.TestSeasonID, '')
	,a.DueSeason = ISNULL(b.DueSeason, '')
	,a.DueDate = b.DueDate
	,a.TestReport = b.TestReport
	,a.FTYReceivedReport = b.FTYReceivedReport
	,a.TestReportTestDate = b.TestReportTestDate
	,a.AddDate = b.AddDate
	,a.AddName = ISNULL(b.AddName, '')
	,a.EditDate = b.EditDate
	,a.Editname = ISNULL(b.Editname, '')
	from [Production].[dbo].UASentReport as a with(nolock)
	inner join [Trade_To_Pms].[dbo].UASentReport  as b with(nolock) on a.BrandRefno = b.BrandRefno and a.ColorID = b.ColorID and a.SuppID = b.SuppID and a.DocumentName = b.DocumentName and a.BrandID = b.BrandID
	where isnull(a.EditDate,a.AddDate) < isnull(b.EditDate,b.AddDate)
	-------------------------- INSERT INTO 
	
	INSERT INTO [Production].[dbo].UASentReport
	 (
		BrandRefno
		,ColorID
		,SuppID
		,DocumentName
		,BrandID
		,TestSeasonID
		,DueSeason
		,DueDate
		,TestReport
		,FTYReceivedReport
		,TestReportTestDate
		,AddDate
		,AddName
		,EditDate
		,Editname
		,UniqueKey
	)
	SELECT
		BrandRefno
		,ColorID
		,SuppID
		,DocumentName
		,BrandID
		,ISNULL(TestSeasonID, '')
		,ISNULL(DueSeason, '')
		,DueDate
		,TestReport
		,FTYReceivedReport
		,TestReportTestDate
		,AddDate
		,ISNULL(AddName, '')
		,EditDate
		,ISNULL(Editname, '')
		,UniqueKey
	from [Trade_To_Pms].[dbo].UASentReport as b WITH (NOLOCK)
	where not exists(select 1 from [Production].[dbo].UASentReport as a WITH (NOLOCK) where a.BrandRefno = b.BrandRefno and a.ColorID = b.ColorID and a.SuppID = b.SuppID and a.DocumentName = b.DocumentName and a.BrandID = b.BrandID)
	

	/*NewSentReport */
	-------UPDATE
	UPDATE a
	SET 	
	 a.ReportDate = b.ReportDate
	,a.AWBno = ISNULL(b.AWBno, '')
	,a.EditDate = b.EditDate
	,a.Editname = ISNULL(b.Editname, '')
	from [Production].[dbo].NewSentReport as a with(nolock)
	inner join [Trade_To_Pms].[dbo].NewSentReport  as b with(nolock) on a.ExportID = b.ExportID 
		and a.PoID = b.PoID 
		and a.Seq1 = b.Seq1 
		and a.Seq2 = b.Seq2
		and a.DocumentName = b.DocumentName 
		and a.BrandID = b.BrandID
	where 
	(
		(
			(a.EditDate is not null and b.EditDate is not null)
			and			
			a.EditDate < b.EditDate
		)
		or
		(
			a.EditDate is null and b.EditDate is not null
		)
		or
		(
			(a.EditDate is null and b.EditDate is null)
			and			
			a.AddDate < b.AddDate
		)
	)
	-------------------------- INSERT INTO 
	INSERT INTO [Production].[dbo].NewSentReport
	 (
	   [ExportID]
      ,[PoID]
      ,[Seq1]
      ,[Seq2]
      ,[DocumentName]
      ,[BrandID]
      ,[ReportDate]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[AWBno]
      ,[T2InspYds]
      ,[T2DefectPoint]
      ,[T2Grade]
      ,[TestReportTestDate]
      ,[FTYReceivedReport]
	  ,UniqueKey
	)
	SELECT
	   [ExportID]
      ,[PoID]
      ,[Seq1]
      ,[Seq2]
      ,[DocumentName]
      ,[BrandID]
      ,[ReportDate]
      ,ISNULL([AddName], '')
      ,[AddDate]
      ,ISNULL([EditName], '')
      ,[EditDate]
      ,ISNULL([AWBno], '')
      ,ISNULL([T2InspYds], 0)
      ,ISNULL([T2DefectPoint], 0)
      ,ISNULL([T2Grade], '')
      ,[TestReportTestDate]
      ,[FTYReceivedReport]
	  ,ISNULL(UniqueKey, '')
	from [Trade_To_Pms].[dbo].NewSentReport as b WITH (NOLOCK)
	where not exists(
		select 1 
		from [Production].[dbo].NewSentReport as a WITH (NOLOCK) 
		where a.ExportID = b.ExportID and a.PoID = b.PoID and a.Seq1 = b.Seq1 
		and a.Seq2 = b.Seq2 
		and a.DocumentName = b.DocumentName 
		and a.BrandID = b.BrandID
	)

	/* ExportRefnoSentReport */
	-------UPDATE
	UPDATE a
	SET 	
	 a.ReportDate = b.ReportDate
	,a.AWBno = ISNULL(b.AWBno, '')
	,a.EditDate = b.EditDate
	,a.Editname = ISNULL(b.Editname, '')
	from [Production].[dbo].ExportRefnoSentReport as a with(nolock)
	inner join [Trade_To_Pms].[dbo].ExportRefnoSentReport  as b with(nolock) on a.ExportID = b.ExportID 
		and a.BrandRefno = b.BrandRefno 
		and a.ColorID = b.ColorID 
		and a.DocumentName = b.DocumentName 
		and a.BrandID = b.BrandID
	where 
	(
		(
			(a.EditDate is not null and b.EditDate is not null)
			and			
			a.EditDate < b.EditDate
		)
		or
		(
			a.EditDate is null and b.EditDate is not null
		)
		or
		(
			(a.EditDate is null and b.EditDate is null)
			and			
			a.AddDate < b.AddDate
		)
	)
	-------------------------- INSERT INTO 
	INSERT INTO [Production].[dbo].ExportRefnoSentReport
	 (
	   [ExportID]
      ,[BrandRefno]
      ,[ColorID]
      ,[DocumentName]
      ,[BrandID]
      ,[ReportDate]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[AWBno]
      ,[FTYReceivedReport]
	  ,UniqueKey
	)
	SELECT
	   [ExportID]
      ,[BrandRefno]
      ,[ColorID]
      ,[DocumentName]
      ,[BrandID]
      ,[ReportDate]
      ,ISNULL([AddName], '')
      ,[AddDate]
      ,ISNULL([EditName], '')
      ,[EditDate]
      ,ISNULL([AWBno], '')
      ,[FTYReceivedReport]
	  ,ISNULL(UniqueKey, '')
	from [Trade_To_Pms].[dbo].ExportRefnoSentReport as b WITH (NOLOCK)
	where not exists(
		select 1 
		from [Production].[dbo].ExportRefnoSentReport as a WITH (NOLOCK) 
		where a.ExportID = b.ExportID 
		and a.BrandRefno = b.BrandRefno 
		and a.ColorID = b.ColorID 
		and a.DocumentName = b.DocumentName 
		and a.BrandID = b.BrandID
	)

/*TrimApproval*/
----------------------Delete
Delete a
from [Production].[dbo].TrimApproval as a 
left join [Trade_To_Pms].[dbo].TrimApproval  as b
	on a.SuppID = b.SuppID and a.TestDocFactoryGroup = b.TestDocFactoryGroup and a.Refno = b.Refno
	and a.ColorID = b.ColorID and a.SeasonID = b.SeasonID and a.DocumentName = b.DocumentName and a.BrandID = b.BrandID
	and a.LOT = b.LOT
where b.ColorID is null 
and b.SeasonID is null
and b.DocumentName is null
and b.BrandID is null
and b.SuppID is null
and b.TestDocFactoryGroup is null
and b.Refno is null
---------------------------UPDATE 
UPDATE a
SET a.Period = b.Period
	,a.FirstDyelot = isnull(b.FirstDyelot, '')
	,a.AWBno = isnull(b.AWBno, '')
	,a.FTYReceivedReport = isnull(b.FTYReceivedReport, '')
	,a.ReceivedDate = b.ReceivedDate
	,a.ReceivedRemark = isnull(b.ReceivedRemark, '')
	,a.AddDate = b.AddDate
	,a.AddName = isnull(b.AddName, '')
	,a.EditDate = b.EditDate
	,a.EditName = isnull(b.EditName, '')
	,a.Junk = isnull(b.Junk, 0)
from [Production].[dbo].TrimApproval as a 
inner join [Trade_To_Pms].[dbo].TrimApproval  as b
	on a.SuppID = b.SuppID and a.TestDocFactoryGroup = b.TestDocFactoryGroup and a.Refno = b.Refno
	and a.ColorID = b.ColorID and a.SeasonID = b.SeasonID and a.DocumentName = b.DocumentName and a.BrandID = b.BrandID
	and a.LOT = b.LOT
-------------------------- INSERT INTO 
INSERT INTO [Production].[dbo].TrimApproval
 (
	 [SuppID]
      ,[TestDocFactoryGroup]
      ,[Refno]
      ,[ColorID]
      ,[SeasonID]
      ,[Period]
      ,[FirstDyelot]
      ,[AWBno]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[FTYReceivedReport]
      ,[ReceivedDate]
      ,[ReceivedRemark]
      ,[DocumentName]
      ,[BrandID]
      ,[Junk]
      ,[LOT]
)
SELECT
	 [SuppID]
      ,[TestDocFactoryGroup]
      ,[Refno]
      ,[ColorID]
      ,[SeasonID]
      ,[Period]
      ,[FirstDyelot]
      ,[AWBno]
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[FTYReceivedReport]
      ,[ReceivedDate]
      ,[ReceivedRemark]
      ,[DocumentName]
      ,[BrandID]
      ,[Junk]
      ,[LOT]
from [Trade_To_Pms].[dbo].TrimApproval as b WITH (NOLOCK)
where not exists(
	select 1 from [Production].[dbo].TrimApproval as a WITH (NOLOCK) 
	where a.SuppID = b.SuppID and a.TestDocFactoryGroup = b.TestDocFactoryGroup and a.Refno = b.Refno
	and a.ColorID = b.ColorID and a.SeasonID = b.SeasonID and a.DocumentName = b.DocumentName and a.BrandID = b.BrandID
	and a.LOT = b.LOT
)

end
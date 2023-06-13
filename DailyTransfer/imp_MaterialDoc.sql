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
	a.TestSeasonID = b.TestSeasonID
	,a.DueSeason = b.DueSeason
	,a.DueDate = b.DueDate
	,a.TestReport = b.TestReport
	,a.FTYReceivedReport = b.FTYReceivedReport
	,a.TestReportTestDate = b.TestReportTestDate
	,a.AddDate = b.AddDate
	,a.AddName = b.AddName
	,a.EditDate = b.EditDate
	,a.Editname = b.Editname
	from [Production].[dbo].UASentReport as a with(nolock)
	inner join [Trade_To_Pms].[dbo].UASentReport  as b with(nolock) on a.BrandRefno = b.BrandRefno and a.ColorID = b.ColorID and a.SuppID = b.SuppID and a.DocumentName = b.DocumentName and a.BrandID = b.BrandID
	where isnull(a.EditDate,a.AddDate) < isnull(b.EditDate,b.AddDate)
	-------------------------- INSERT INTO 
	set IDENTITY_INSERT UASentReport on
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
		,Ukey
		,UniqueKey
	)
	SELECT
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
		,Ukey
		,UniqueKey
	from [Trade_To_Pms].[dbo].UASentReport as b WITH (NOLOCK)
	where not exists(select 1 from [Production].[dbo].UASentReport as a WITH (NOLOCK) where a.BrandRefno = b.BrandRefno and a.ColorID = b.ColorID and a.SuppID = b.SuppID and a.DocumentName = b.DocumentName and a.BrandID = b.BrandID)
	set IDENTITY_INSERT UASentReport off

	/*NewSentReport */
	-------UPDATE
	UPDATE a
	SET 	
	 a.ReportDate = b.ReportDate
	,a.AWBno = b.AWBno
	,a.EditDate = b.EditDate
	,a.Editname = b.Editname
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
	set IDENTITY_INSERT NewSentReport on
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
      ,[UKEY]
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
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[AWBno]
      ,[T2InspYds]
      ,[T2DefectPoint]
      ,[T2Grade]
      ,[TestReportTestDate]
      ,isnull([UKEY],0)
      ,[FTYReceivedReport]
	  ,UniqueKey
	from [Trade_To_Pms].[dbo].NewSentReport as b WITH (NOLOCK)
	where not exists(
		select 1 
		from [Production].[dbo].NewSentReport as a WITH (NOLOCK) 
		where a.ExportID = b.ExportID and a.PoID = b.PoID and a.Seq1 = b.Seq1 
		and a.Seq2 = b.Seq2 
		and a.DocumentName = b.DocumentName 
		and a.BrandID = b.BrandID
	)
	set IDENTITY_INSERT NewSentReport off

	/* ExportRefnoSentReport */
	-------UPDATE
	UPDATE a
	SET 	
	 a.ReportDate = b.ReportDate
	,a.AWBno = b.AWBno
	,a.EditDate = b.EditDate
	,a.Editname = b.Editname
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
	set IDENTITY_INSERT ExportRefnoSentReport on
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
      ,[UKEY]
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
      ,[AddName]
      ,[AddDate]
      ,[EditName]
      ,[EditDate]
      ,[AWBno]
      ,[UKEY]
      ,[FTYReceivedReport]
	  ,UniqueKey
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
	set IDENTITY_INSERT ExportRefnoSentReport off

end
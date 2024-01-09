CREATE PROCEDURE [dbo].[P_ImportProductionKitsTracking]
as
BEGIN
SET NOCOUNT ON
	DECLARE @SqlCmdAll NVARCHAR(MAX) = '';
	DECLARE @sqlcmddeclare NVARCHAR(MAX) = '';
	DECLARE @sqlcmdSELECT NVARCHAR(MAX) = '';
	DECLARE @sqlcmdInsert NVARCHAR(MAX) = '';
	DECLARE @sqlcmdUpdate NVARCHAR(MAX) = '';
	DECLARE @sqlcmdDelete NVARCHAR(MAX) = '';

	SET @sqlcmddeclare = '
	DECLARE @FirstDate VARCHAR(10);
	DECLARE @StartDate VARCHAR(10);
	DECLARE @EndDate VARCHAR(10);

	IF NOT EXISTS (SELECT 1 FROM [DBO].[P_ProductionKitsTracking])
	BEGIN
		SET @FirstDate = ''2022-01-01'';
	END
	ELSE
	BEGIN
		SET @StartDate = CONVERT(VARCHAR(10), DATEADD(DAY, -30, GETDATE()), 120);
		SET @EndDate = CONVERT(VARCHAR(10), GETDATE(), 120);
	END;'

	SET @sqlcmdSELECT = '
	select 
		 [BrandID] = s.BrandID
		,[StyleID] = s.ID
		,[SeasonID] = s.SeasonID
		,[Article] = sp.Article
		,[Mdivision] = sp.MDivisionID
		,[FactoryID] = sp.FactoryID
		,[Doc] = CONCAT(sp.DOC,''-'',r.Name)
		,[TWSendDate] = sp.SendDate
		,[FtyMRRcvDate] = sp.ReceiveDate
		,[FtySendtoQADate] = sp.SendToQA
		,[QARcvDate] = sp.QAReceived
		,[UnnecessaryToSend] = iif(Len(isnull(sp.ReasonID,'''')) = 0,''N'',''Y'')
		,[ProvideDate] = sp.ProvideDate
		,[SPNo] = sp.OrderId
		,[SCIDelivery] = sp.SCIDelivery
		,[BuyerDelivery] = sp.BuyerDelivery
		,[PullForward] = IIF(sp.IsPF = 1,''Y'',''N'')
		,[Handle] = CONCAT(sp.SendName,''-'',(select Name from  MainServer.Production.dbo.TPEPass1 WITH (NOLOCK) where ID = sp.SendName))
		,[MRHandle] = CONCAT(sp.MRHandle,''-'',(select Name from  MainServer.Production.dbo.TPEPass1 WITH (NOLOCK) where ID = sp.MRHandle))
		,[SMR] = CONCAT(sp.SMR,''-'',(select Name from  MainServer.Production.dbo.TPEPass1 WITH (NOLOCK) where ID = sp.SMR))
		,[POHandle] = CONCAT(sp.PoHandle,''-'',(select Name from  MainServer.Production.dbo.TPEPass1 WITH (NOLOCK) where ID = sp.PoHandle))
		,[POSMR] = CONCAT(sp.POSMR,''-'',(select Name from  MainServer.Production.dbo.TPEPass1 WITH (NOLOCK) where ID = sp.POSMR))
		,[FtyHandle] = CONCAT(sp.FtyHandle,''-'',(select Name from  MainServer.Production.dbo.Pass1 WITH (NOLOCK) where ID = sp.FtyHandle))
		,[ProductionKitsGroup] = sp.ProductionKitsGroup
		,sp.AddDate
		,sp.EditDate
	into #tmp
	from MainServer.Production.dbo.Style_ProductionKits sp WITH (NOLOCK) 
	inner join MainServer.Production.dbo.Style s WITH (NOLOCK) on s.Ukey = sp.StyleUkey
	left join MainServer.Production.dbo.Reason r WITH (NOLOCK) on r.ReasonTypeID = ''ProductionKits'' and r.ID = sp.DOC
	where (@FirstDate IS NULL OR sp.SCIDelivery >= @FirstDate)
	AND 
	(
	 ((@StartDate IS NULL AND @EndDate IS NULL) OR sp.AddDate >= @StartDate AND sp.AddDate <= @EndDate)
		OR 
	 ((@StartDate IS NULL AND @EndDate IS NULL) OR sp.EditDate >= @StartDate AND sp.EditDate <= @EndDate)
	)
	'
	SET @sqlcmdDelete = '
	DELETE P_ProductionKitsTracking
	FROM P_ProductionKitsTracking p
	where not exists (select 1 from #tmp t where t.Article = p.Article 
											and t.FactoryID = p.FactoryID 
											and t.Doc = p.Doc 
											and t.SPNo = p.SPNo 
											and t.ProductionKitsGroup = p.ProductionKitsGroup)
	and (p.AddDate >= @StartDate AND p.AddDate <= @EndDate
		or p.EditDate >= @StartDate AND p.EditDate <= @EndDate)
	'
	set @sqlcmdUpdate = '
	UPDATE p SET
		p.BrandID				= isnull(t.BrandID,'''')
		,p.StyleID				= isnull(t.StyleID,'''')
		,p.SeasonID				= isnull(t.SeasonID,'''')
		,p.Article				= isnull(t.Article,'''')
		,p.Mdivision			= isnull(t.Mdivision,'''')
		,p.FactoryID			= isnull(t.FactoryID,'''')
		,p.Doc					= isnull(t.Doc,'''')
		,p.TWSendDate			= t.TWSendDate
		,p.FtyMRRcvDate			= t.FtyMRRcvDate
		,p.FtySendtoQADate		= t.FtySendtoQADate
		,p.QARcvDate			= t.QARcvDate
		,p.UnnecessaryToSend	= isnull(t.UnnecessaryToSend,'''')
		,p.ProvideDate			= t.ProvideDate
		,p.SPNo					= isnull(t.SPNo,'''')
		,p.SCIDelivery			= t.SCIDelivery
		,p.BuyerDelivery		= t.BuyerDelivery
		,p.Pullforward			= isnull(t.Pullforward,'''')
		,p.Handle				= isnull(t.Handle,'''')
		,p.MRHandle				= isnull(t.MRHandle,'''')
		,p.SMR					= isnull(t.SMR,'''')
		,p.POHandle				= isnull(t.POHandle,'''')
		,p.POSMR				= isnull(t.POSMR,'''')
		,p.FtyHandle			= isnull(t.FtyHandle,'''')
		,p.ProductionKitsGroup	= isnull(t.ProductionKitsGroup,'''')
		,p.AddDate				= t.AddDate
		,p.EditDate				= t.EditDate
	FROM P_ProductionKitsTracking p
	inner join #tmp t on t.Article = p.Article and t.FactoryID = p.FactoryID and t.Doc = p.Doc and t.SPNo = p.SPNo and t.ProductionKitsGroup = p.ProductionKitsGroup'

	SET @sqlcmdInsert = '
	insert into P_ProductionKitsTracking (
		 BrandID
		,StyleID
		,SeasonID
		,Article
		,Mdivision
		,FactoryID
		,Doc
		,TWSendDate
		,FtyMRRcvDate
		,FtySendtoQADate
		,QARcvDate
		,UnnecessaryToSend
		,ProvideDate
		,SPNo
		,SCIDelivery
		,BuyerDelivery
		,Pullforward
		,Handle
		,MRHandle
		,SMR
		,POHandle
		,POSMR
		,FtyHandle
		,ProductionKitsGroup
		,AddDate
		,EditDate
	)
	SELECT
		isnull(t.BrandID,'''')
		,isnull(t.StyleID,'''')
		,isnull(t.SeasonID,'''')
		,isnull(t.Article,'''')
		,isnull(t.Mdivision,'''')
		,isnull(t.FactoryID,'''')
		,isnull(t.Doc,'''')
		,t.TWSendDate
		,t.FtyMRRcvDate
		,t.FtySendtoQADate
		,t.QARcvDate
		,isnull(t.UnnecessaryToSend,'''')
		,t.ProvideDate
		,isnull(t.SPNo,'''')
		,t.SCIDelivery
		,t.BuyerDelivery
		,isnull(t.Pullforward,'''')
		,isnull(t.Handle,'''')
		,isnull(t.MRHandle,'''')
		,isnull(t.SMR,'''')
		,isnull(t.POHandle,'''')
		,isnull(t.POSMR,'''')
		,isnull(t.FtyHandle,'''')
		,isnull(t.ProductionKitsGroup,'''')
		,t.AddDate
		,t.EditDate
	FROM #tmp t 
	where not exists (select 1 from P_ProductionKitsTracking p where t.Article = p.Article 
																and t.FactoryID = p.FactoryID 
																and t.Doc = p.Doc 
																and t.SPNo = p.SPNo
																and t.ProductionKitsGroup = p.ProductionKitsGroup)

	/* BI Info */
	IF EXISTS (select 1 from BITableInfo b where b.id = ''P_ImportProductionKitsTracking'')
	BEGIN
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = ''P_ImportProductionKitsTracking''
	END
	ELSE 
	BEGIN
		insert into BITableInfo(Id, TransferDate)
		values(''P_ImportProductionKitsTracking'', getdate())
	END'

	SET @SqlCmdAll = @sqlcmddeclare + @sqlcmdSELECT +@sqlcmdDelete +@sqlcmdUpdate + @sqlcmdInsert

	EXEC sp_executesql @SqlCmdAll
END

CREATE PROCEDURE [dbo].[GetCuttingScheduleOutputList]
AS
BEGIN
SET NOCOUNT ON

	DECLARE @StartDate VARCHAR(10);
	DECLARE @EndDate VARCHAR(10);

	IF NOT EXISTS (SELECT 1 FROM [ExtendServer].[POWERBIReportData].[DBO].[P_CuttingScheduleOutputList])
	BEGIN
		SET @StartDate = '2022-01-01';
		SET @EndDate = NULL;
	END
	ELSE
	BEGIN
		SET @StartDate = CONVERT(VARCHAR(10), GETDATE(), 120);
		SET @EndDate = CONVERT(VARCHAR(10), DATEADD(DAY, 7, GETDATE()), 120);
	END
	declare @tmp_main TABLE
	(
		 [MDivisionID]				[varchar](8) 
		,[FactoryID]				[varchar](8)
		,[Fabrication]				[varchar](20)
		,[EstCuttingDate]			[Date]
		,[ActCuttingDate]			[Date]
		,[EarliestSewingInline]		[Date]
		,[POID]						[varchar](13)
		,[BrandID]					[varchar](8)
		,[StyleID]					[varchar](15)
		,[FabRef]					[varchar](36)
		,[SwitchToWorkorderType]	[varchar](11)
		,[CutRef]					[varchar](6)
		,[CutNo]					[numeric](6, 0)
		,[SpreadingNoID]			[varchar](5)
		,[CutCell]					[varchar](2)
		,[Combination]				[varchar](2)
		,[Layers]					[numeric](5, 0)
		,[LackingLayers]			[numeric](10, 0)
		,[Ratio]					[varchar](MAX)	
		,[Consumption]				[numeric](9, 4)
		,[MarkerName]				[varchar](20)
		,[MarkerNo]					[varchar](10)
		,[MarkerLength]				[varchar](15)
	)
	INSERT INTO @tmp_main
	SELECT 
	 [MDivisionID] = wo.MDivisionID
	,[FactoryID] = wo.FactoryID
	,[Fabrication] = f.WeaveTypeID
	,[EstCuttingDate]= wo.EstCutDate
	,[ActCuttingDate] = MincDate.MincoDate
	,[EarliestSewingInline] = c.SewInLine
	,[POID] = wo.ID
	,[BrandID]=o.BrandID
	,[StyleID] = o.StyleID
	,[FabRef] = wo.Refno
	,[SwitchToWorkorderType] = iif(c.WorkType='1','Combination',Iif(c.WorkType='2','By SP#',''))
	,[CutRef] = wo.CutRef
	,[CutNo] = wo.Cutno
	,[SpreadingNoID]=wo.SpreadingNoID
	,[CutCell] = wo.CutCellID
	,[Combination] = wo.FabricCombo
	,[Layers] = sum(wo.Layer)
	,[LackingLayers] = isnull(acc.val,0)
	,[Ratio] = stuff(SQty.val,1,1,'')
	,[Consumption] = sum(wo.cons) 
	,[Marker Name] = wo.Markername
	,[MarkerNo] = wo.MarkerNo
	,[MarkerLength] = wo.MarkerLength
	FROM [Production].[dbo].[WorkOrder] wo WITH(NOLOCK)
	LEFT JOIN [Production].[dbo].[Orders] o WITH(NOLOCK) ON o.ID = wo.ID
	LEFT JOIN  [Production].[dbo].[Cutting] c WITH (NOLOCK) ON c.ID = o.CuttingSP
	LEFT JOIN [Production].[dbo].[Fabric] f WITH (NOLOCK) ON f.SCIRefno = wo.SCIRefno
	OUTER APPLY
	(
		SELECT val = sum(cd.Layer) 
		FROM [Production].[dbo].[Cuttingoutput_Detail] cd WITH (NOLOCK)
		INNER JOIN [Production].[dbo].[CuttingOutput] c WITH (NOLOCK) ON cd.ID = c.ID
		WHERE cd.CutRef = wo.CutRef and wo.CutRef <> ''
	)acc
	OUTER APPLY(
		SELECT MincoDate = MIN(co.cdate)
		FROM [Production].[dbo].[CuttingOutput] co WITH (NOLOCK) 
		INNER JOIN [Production].[dbo].[Cuttingoutput_Detail] cod WITH (NOLOCK) on co.id = cod.id
		Where cod.CutRef = wo.CutRef and co.Status != 'New' and co.FactoryID = wo.FactoryID and wo.CutRef <> ''
	)MincDate
	OUTER APPLY
	(
		SELECT val = 
		(
			SELECT DISTINCT  concat(',',SizeCode+'/'+Convert(VARCHAR,Qty))
			FROM [Production].[dbo].[WorkOrder_SizeRatio] WITH (NOLOCK) 
			WHERE WorkOrderUkey = wo.UKey
			FOR XML PATH('')
		)
	)as SQty
	WHERE 1=1
	and wo.EstCutDate is not null
	and (wo.EstCutDate > @StartDate AND @EndDate is null)
	OR (wo.EstCutDate >= @StartDate and wo.EstCutDate <= @EndDate)
	GROUP BY wo.MDivisionId,wo.FactoryID,f.WeaveTypeID,wo.EstCutDate,MincDate.MincoDate,c.SewInLine,wo.ID,o.BrandID,o.StyleID,
		    wo.Refno,c.WorkType,wo.CutRef,wo.Cutno,wo.SpreadingNoID,wo.CutCellid,wo.FabricCombo,sqty.val,
		    wo.Markername,wo.MarkerNo,wo.MarkerLength,acc.val
	ORDER by [EstCuttingDate]

	declare @tmp_Finish TABLE
	(
		 [MDivisionID]				[varchar](8) 
		,[FactoryID]				[varchar](8)
		,[Fabrication]				[varchar](20)
		,[EstCuttingDate]			[Date]
		,[ActCuttingDate]			[Date]
		,[EarliestSewingInline]		[Date]
		,[POID]						[varchar](13)
		,[BrandID]					[varchar](8)
		,[StyleID]					[varchar](15)
		,[FabRef]					[varchar](36)
		,[SwitchToWorkorderType]	[varchar](11)
		,[CutRef]					[varchar](6)
		,[CutNo]					[numeric](6, 0)
		,[SpreadingNoID]			[varchar](5)
		,[CutCell]					[varchar](2)
		,[Combination]				[varchar](2)
		,[Layers]					[numeric](5, 0)
		,[LayersLevel]				[varchar](10)
		,[LackingLayers]			[numeric](10, 0)
		,[Ratio]					[varchar](MAX)	
		,[Consumption]				[numeric](9, 4)
		,[ActConsOutput]			[varchar](20)
		,[BalanceCons]				[varchar](20)
		,[MarkerName]				[varchar](20)
		,[MarkerNo]					[varchar](10)
		,[MarkerLength]				[varchar](15)
		,[CuttingPerimeter]			[varchar](15)
		,[StraightLength]			[varchar](15)
		,[CurvedLength]				[varchar](15)
		,[DelayReason]				[nvarchar](100)
		,[Remark]					[nvarchar](MAX)
	)
	INSERT INTO @tmp_Finish
	SELECT 
	[MDivisionID],
	[FactoryID],
	[Fabrication],
	[EstCuttingDate],
	[ActCuttingDate],
	[EarliestSewingInline] ,
	[POID], 
	[BrandID],
	[StyleID],
	[FabRef],
	[SwitchToWorkorderType],
	[CutRef],
	[CutNo],
	[SpreadingNoID],
	[CutCell],
	[Combination],
	[Layer] = SUM([Layers]),
	[Layers Level]= CASE WHEN SUM([Layers]) between 1 and 5 THEN '1~5'
						 WHEN SUM([Layers]) between 6 and 10 THEN '6~10'
						 WHEN SUM([Layers]) between 11 and 15 THEN '11~15'
						 WHEN SUM([Layers]) between 16 and 30 THEN '16~30'
						 WHEN SUM([Layers]) between 31 and 50 THEN '31~50'
						 ELSE '50 above'
						 END ,
	[LackingLayers] = SUM([Layers])-[LackingLayers],
	[Ratio],
	[Consumption] = SUM([Consumption]),
	[ActConsOutput] = IIF(LEN([MarkerLength]) > 0, CAST(ISNULL(IIF(SUM([Layers])-[LackingLayers] = 0, SUM([Consumption]), [LackingLayers]* dbo.MarkerLengthToYDS([MarkerLength])),0) AS VARCHAR),''),
	[BalanceCons] = IIF(LEN([MarkerLength]) > 0,CAST(SUM([Consumption])- ISNULL(IIF(SUM([Layers])-[LackingLayers] = 0, SUM([Consumption]), [LackingLayers]* dbo.MarkerLengthToYDS([MarkerLength])),0) AS VARCHAR),''), 
	[MarkerName],
	[MarkerNo],
	[MarkerLength],
	[CuttingPerimeter] = wk.ActCuttingPerimeter,
	[StraightLength] = wk.StraightLength,
	[CurvedLength] = wk.CurvedLength,
	[DelayReason] = dw.[Name],
	[Remark] = wk.Remark
	FROM @tmp_main t
	outer apply (SELECT TOP 1 ActCuttingPerimeter,StraightLength,CurvedLength,Remark,UnfinishedCuttingReason FROM [Production].[dbo].[WorkOrder] wo WITH (NOLOCK) WHERE wo.CutRef = t.[CutRef]) wk
	left join [Production].[dbo].[DropDownList] dw WITH (NOLOCK) ON dw.[Type] = 'PMS_UnFinCutReason' and dw.ID = wk.UnfinishedCuttingReason
	Where [EstCuttingDate] is not null
	GROUP BY [MDivisionID],[FactoryID],[Fabrication],[EstCuttingDate],[ActCuttingDate],[EarliestSewingInline],
	[POID],[BrandID],[StyleID],[FabRef],[SwitchToWorkorderType],[CutRef],
	[CutNo],[SpreadingNoID],[CutCell],[Combination],[LackingLayers],[Ratio],[MarkerName],
	[MarkerNo], [MarkerLength],wk.ActCuttingPerimeter,
	wk.StraightLength,wk.CurvedLength,
	dw.[Name],wk.Remark


	SELECT *FROM @tmp_Finish
END

CREATE PROCEDURE [dbo].[P_ImportCuttingScheduleOutputList]
as
begin
SET NOCOUNT ON
	
	declare @current_PMS_ServerName nvarchar(50) = 'MainServer'
	declare @SqlCmd nvarchar(max) ='';
	declare @SqlCmdDelete nvarchar(max) ='';
	declare @SqlCmdUpdataIntegrate nvarchar(max) = '';
	declare @SqlCmdUpdata nvarchar(max) ='';
	declare @SqlCmdinsert nvarchar(max) ='';
	set @SqlCmd = '
	/************* 抓Cutting.R13報表資料*************/
	select 
	*
	into #tmp
	from OPENQUERY(['+@current_PMS_ServerName+'],
	''exec production.[dbo].GetCuttingScheduleOutputList'')'

	set @SqlCmdDelete = 
	'
	/************* 刪除P_CuttingScheduleOutputList的資料，規則刪除相同的WorkOrder.ID*************/
	Delete P_CuttingScheduleOutputList
	from P_CuttingScheduleOutputList as a 
	inner join #tmp as b on a.FactoryID = b.FactoryID and a.POID =b.POID and a.EstCuttingDate = b.EstCuttingDate
	'
	set @SqlCmdinsert = '
	/************* 新增P_CuttingScheduleOutputList的資料(ActCuttingDate,LackingLayers新增時欄位都要為空)*************/
	insert into P_CuttingScheduleOutputList
	(
		[MDivisionID]				
		,[FactoryID]			
		,[Fabrication]			
		,[EstCuttingDate]		
		,[ActCuttingDate]		
		,[EarliestSewingInline]	
		,[POID]					
		,[BrandID]				
		,[StyleID]				
		,[FabRef]				
		,[SwitchToWorkorderType]
		,[CutRef]				
		,[CutNo]				
		,[SpreadingNoID]		
		,[CutCell]				
		,[Combination]			
		,[Layers]
		,[LayersLevel]
		,[LackingLayers]		
		,[Ratio]				
		,[Consumption]
		,[ActConsOutput]
		,[BalanceCons]
		,[MarkerName]			
		,[MarkerNo]
		,[MarkerLength]
		,[CuttingPerimeter]
		,[StraightLength]
		,[CurvedLength]
		,[DelayReason]
		,[Remark]
	)
	select 
	  [MDivisionID] = isnull([MDivisionID],'''')				
	, [FactoryID] = isnull([FactoryID],'''')
	, [Fabrication] = isnull([Fabrication],'''')
	, [EstCuttingDate]
	, [ActCuttingDate] = null
	, [EarliestSewingInline]
	, [POID]=isnull([POID],'''')
	, [BrandID] = isnull([BrandID],'''')
	, [StyleID] = isnull([StyleID],'''')
	, [FabRef] = isnull([FabRef],'''')
	, [SwitchToWorkorderType] = isnull([SwitchToWorkorderType],'''')
	, [CutRef] = isnull([CutRef],'''')
	, [CutNo] = isnull([CutNo],0)
	, [SpreadingNoID] = isnull([SpreadingNoID],'''')
	, [CutCell] = isnull([CutCell],'''')
	, [Combination] = isnull([Combination],'''')
	, [Layers] = isnull([Layers],0)
	, [LayersLevel] = isnull([LayersLevel],'''')
	, [LackingLayers] = 0
	, [Ratio] = isnull([Ratio],'''')
	, [Consumption] = isnull([Consumption],0)
	, [ActConsOutput] = isnull([ActConsOutput],'''')
	, [BalanceCons] = isnull([BalanceCons],'''')
	, [MarkerName] = isnull([MarkerName],'''')
	, [MarkerNo] = isnull([MarkerNo],'''')
	, [MarkerLength] = isnull([MarkerLength],'''')
	, [CuttingPerimeter] = isnull([CuttingPerimeter],'''')
	, [StraightLength] = isnull([StraightLength],'''')
	, [CurvedLength] = isnull([CurvedLength],'''')
	, [DelayReason] = isnull([DelayReason],'''')
	, [Remark] = isnull([Remark],'''')
	from #tmp
	'

	set @SqlCmdUpdataIntegrate ='
	/************* 更新ActCuttingDate、LackingLayers欄位前的整合資料*************/
	/*************找出CuttingOutput，有被新增及修改的資料*************/
	SELECT
	[UpperSum] =  SUM(Layer)
	,[CuttingID] = CuttingID
	,[CutRef] = CutRef
	,[cDate]
	,[FactoryID] = co.FactoryID
	into #cuttingSum
	FROM [MainServer].[Production].[dbo].[CuttingOutput] co
	INNER JOIN [MainServer].[Production].[dbo].[CuttingOutput_Detail] cod ON co.id = cod.ID
	WHERE 
	(co.EditDate BETWEEN GETDATE() - 7 AND GETDATE()) OR
	(co.AddDate BETWEEN GETDATE() - 7 AND GETDATE())
	GROUP BY cod.CuttingID,cod.CutRef,co.cDate,co.FactoryID
	/*************找出的workOrder的資料*************/
	SELECT 
	[LowerSum] = SUM(Layer)  
	,ID
	,wo.CutRef
	,wo.FactoryID
	into #workOrderSUM
	FROM [MainServer].[Production].[dbo].[WorkOrder] wo
	inner join #cuttingSum cs with(NOLOCK) on wo.ID = cs.[CuttingID] and wo.CutRef = cs.CutRef
	GROUP BY ID,wo.CutRef,wo.FactoryID
	/*************找出workOrder與CuttingOutput的Layer相等的資料*************/
	SELECT
		[ID] = b.id
		,[CutRef] =  a.CutRef
		,[LackingLayers] = UpperSum - acc.val
		,[cDate] = MincDate.MincoDate
		,[FactoryID] = b.FactoryID
	into #sum
	FROM #cuttingSum a 
	inner join #workOrderSUM b on a.CuttingID = b.id and a.CutRef = b.CutRef
	OUTER APPLY
	(
		SELECT MincoDate = MIN(co.cdate)
		FROM [Production].[dbo].[CuttingOutput] co WITH (NOLOCK) 
		INNER JOIN [Production].[dbo].[Cuttingoutput_Detail] cod WITH (NOLOCK) on co.id = cod.id
		Where cod.CutRef = b.[CutRef] and co.Status != ''New'' and co.FactoryID = b.FactoryID and b.CutRef <> ''''
	)MincDate
	OUTER APPLY
	(
		SELECT val = sum(cd.Layer) 
		FROM [MainServer].[Production].[dbo].[Cuttingoutput_Detail] cd WITH (NOLOCK)
		INNER JOIN [MainServer].[Production].[dbo].[CuttingOutput] c WITH (NOLOCK) ON cd.ID = c.ID
		WHERE cd.CutRef = b.[CutRef] and b.[CutRef] <> ''''
	)acc
	where UpperSum = LowerSum

	/*************將上面#tmp表與#sum表union起來*************/
	select 
	*
	into #Integrate
	FROM
	(
		select 
		[ID]
		,[CutRef]
		,[LackingLayers]
		,[cDate]
		,[FactoryID]
		from #sum 
		union
		select 
		[ID] = t.[POID]
		,[CutRef] = t.[CutRef]
		,[LackingLayers] = t.[LackingLayers]
		,[cDate] = t.[ActCuttingDate]
		,[FactoryID] = t.[FactoryID]
		from #tmp t
	)aa
	'

	set @SqlCmdUpdata = '
	update p set
	p.[ActCuttingDate] = t.[cDate],
	p.[LackingLayers] = t.[LackingLayers]
	from P_CuttingScheduleOutputList p with(nolock)
	inner join #Integrate t with(nolock) on t.[FactoryID] = p.[FactoryID] and t.[ID] = p.[POID] and p.[CutRef] = t.[CutRef]

	IF EXISTS (select 1 from BITableInfo b where b.id = ''P_CuttingScheduleOutputList'')
	BEGIN
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = ''P_CuttingScheduleOutputList''
	END
	ELSE 
	BEGIN
		insert into BITableInfo(Id, TransferDate)
		values(''P_CuttingScheduleOutputList'', getdate())
	END
	'
	DECLARE @SqlCmdAll nVARCHAR(MAX);
	set @SqlCmdAll = @SqlCmd + @SqlCmdDelete+@SqlCmdinsert+@SqlCmdUpdataIntegrate+@SqlCmdUpdata
	EXEC sp_executesql @Sqlcmdall
end


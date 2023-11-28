CREATE PROCEDURE [dbo].[P_ImportCuttingBCS]
as
begin
SET NOCOUNT ON;
	declare @current_PMS_ServerName nvarchar(50) = 'MainServer'
	declare @SqlCmd nvarchar(max) ='';
	declare @SqlCmdDelete nvarchar(max) ='';
	declare @SqlCmdUpdataIntegrate nvarchar(max) = '';
	declare @SqlCmdUpdata nvarchar(max) ='';
	declare @SqlCmdinsert nvarchar(max) ='';
	declare @SqlCmdUpdata_1 nvarchar(max) ='';
	set @SqlCmd = '
	/************* 抓Cutting.R13報表資料*************/
	select 
	*
	into #tmp
	from OPENQUERY(['+@current_PMS_ServerName+'],
	''exec Production.[dbo].[GetCuttingBCS]'')'

	set @SqlCmdUpdata_1 = '
	Select DISTINCT [OrderID] into #tmp_1 FROM #TMP
	update b set b.BIPImportCuttingBCCmdTime = GETDATE()
	from #tmp_1 a
	inner join ['+@current_PMS_ServerName+'].Production.dbo.SewingSchedule b on a.OrderID = b.OrderID'

	set @SqlCmdDelete = '
	/************* 刪除P_CuttingBCS的資料，規則刪除相同的OrderID*************/
	Delete P_CuttingBCS
	from P_CuttingBCS as a 
	inner join #tmp as b on a.FactoryID = b.FactoryID and a.OrderID =b.OrderID 
	'

	set @SqlCmdinsert = '
	/************* 新增P_CuttingBCS的資料*************/
	insert into P_CuttingBCS
	(
		 [MDivisionID]
		,[FactoryID]
		,[BrandID]
		,[StyleID]
		,[SeasonID]
		,[CDCodeNew]
		,[FabricType]
		,[POID]
		,[Category]
		,[WorkType]
		,[MatchFabric]
		,[OrderID]
		,[SciDelivery]
		,[BuyerDelivery]
		,[OrderQty]
		,[SewInLineDate]
		,[SewOffLineDate]
		,[SewingLineID]
		,[RequestDate]
		,[StdQty]
		,[StdQtyByLine]
		,[AccuStdQty]
		,[AccuStdQtyByLine]
		,[AccuEstCutQty]
		,[AccuEstCutQtyByLine]
		,[SupplyCutQty]
		,[SupplyCutQtyByLine]
		,[BalanceCutQty]
		,[BalanceCutQtyByLine]
		,[SupplyCutQtyVSStdQty]
		,[SupplyCutQtyVSStdQtyByLine]
	)
	select 
	 [MDivisionID] = isnull([MDivisionID],'''')	
	,[FactoryID] = isnull([FactoryID],'''')	
	,[BrandID] = isnull([BrandID],'''')	
	,[StyleID] = isnull([StyleID],'''')	
	,[SeasonID] = isnull([SeasonID],'''')	
	,[CDCodeNew] = isnull([CDCodeNew],'''')	
	,[FabricType] = isnull([FabricType],'''')	
	,[POID] = isnull([POID],'''')	
	,[Category] = isnull([Category],'''')	
	,[WorkType] = isnull([WorkType],'''')	
	,[MatchFabric] = isnull([MatchFabric],'''')	
	,[OrderID] = isnull([OrderID],'''')	
	,[SciDelivery]
	,[BuyerDelivery]
	,[OrderQty] = isnull([OrderQty],0)
	,[SewInLineDate]
	,[SewOffLineDate]
	,[SewingLineID] = isnull([SewingLineID],'''')	
	,[RequestDate]
	,[StdQty] = isnull([StdQty],0)
	,[StdQtyByLine] = isnull([StdQtyByLine],0)
	,[AccuStdQty] = isnull([AccuStdQty],0)
	,[AccuStdQtyByLine] = isnull([AccuStdQtyByLine],0)
	,[AccuEstCutQty] = isnull([AccuEstCutQty],0)
	,[AccuEstCutQtyByLine] = isnull([AccuEstCutQtyByLine],0)
	,[SupplyCutQty] = isnull([SupplyCutQty],0)
	,[SupplyCutQtyByLine] = isnull([SupplyCutQtyByLine],0)
	,[BalanceCutQty] = isnull([BalanceCutQty],0)
	,[BalanceCutQtyByLine] = isnull([BalanceCutQtyByLine],0)
	,[SupplyCutQtyVSStdQty] = isnull([SupplyCutQtyVSStdQty],0)
	,[SupplyCutQtyVSStdQtyByLine] = isnull([SupplyCutQtyVSStdQtyByLine],0)
	from #tmp
	'

	set @SqlCmdUpdata = '
	IF EXISTS (select 1 from BITableInfo b where b.id = ''P_ImportCuttingBCS'')
	BEGIN
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = ''P_ImportCuttingBCS''
	END
	ELSE 
	BEGIN
		insert into BITableInfo(Id, TransferDate)
		values(''P_ImportCuttingBCS'', getdate())
	END
	'
	DECLARE @SqlCmdAll nVARCHAR(MAX);
	set @SqlCmdAll = @SqlCmd  + @SqlCmdDelete+@SqlCmdinsert+ @SqlCmdUpdata_1 +@SqlCmdUpdata
	EXEC sp_executesql @Sqlcmdall
END
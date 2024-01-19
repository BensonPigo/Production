CREATE PROCEDURE [dbo].[P_ImportCuttingBCS]
	@StartDate date = null,
	@EndDate date = null
as
begin
	SET NOCOUNT ON;

	if @StartDate is null
	begin
		set @StartDate = DATEADD(DAY, -30, GETDATE())
	end

	if @EndDate is null
	begin
		set @EndDate = DATEADD(DAY, 75, GETDATE())
	end

	SELECT *
	INTO #tmp_P_CuttingBCS
	FROM P_CuttingBCS
	WHERE 1 = 0

	/************* 抓Cutting.R13報表資料*************/
	DECLARE @SqlCmd NVARCHAR(MAX) = '
	INSERT INTO #tmp_P_CuttingBCS
	exec [MainServer].Production.[dbo].[GetCuttingBCS] ''' + format(@StartDate, 'yyyy/MM/dd') + ''', ''' + format(@EndDate, 'yyyy/MM/dd') + ''' '

	EXEC sp_executesql @SqlCmd

	update b 
		set b.BIPImportCuttingBCSCmdTime = GETDATE()
	from [MainServer].[Production].[dbo].[SewingSchedule] b 
	where exists (select 1 from #tmp_P_CuttingBCS t where t.OrderID = b.OrderID)

	/************* 刪除P_CuttingBCS的資料，規則刪除相同的OrderID*************/
	Delete a
	from P_CuttingBCS a 
	where exists (select 1 from #tmp_P_CuttingBCS b where a.FactoryID = b.FactoryID and a.OrderID = b.OrderID)

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
	select [MDivisionID]
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
	from #tmp_P_CuttingBCS


	IF EXISTS (select 1 from BITableInfo b where b.id = 'P_CuttingBCS')
	BEGIN
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = 'P_CuttingBCS'
	END
	ELSE 
	BEGIN
		insert into BITableInfo(Id, TransferDate)
		values('P_CuttingBCS', getdate())
	END

END
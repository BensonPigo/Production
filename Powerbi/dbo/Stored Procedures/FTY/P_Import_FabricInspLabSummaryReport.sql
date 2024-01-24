CREATE PROCEDURE [dbo].[P_Import_FabricInspLabSummaryReport]
	@StartDate as date = null,
	@EndDate as date = null
AS
BEGIN
	SET NOCOUNT ON;

	if @StartDate is null 
	begin
		SET @StartDate = CONVERT(VARCHAR(10), DATEADD(MONTH, -3, GETDATE()), 120);
	end

	if @EndDate is null 
	begin
		SET @EndDate = CONVERT(VARCHAR(10), GETDATE(), 120);
	end

	select *
	into #tmp_P_FabricInspLabSummaryReport
	from P_FabricInspLabSummaryReport
	where 1 = 0

	declare @SqlCmd nvarchar(max) = '
	/************* 抓QA.R01報表資料*************/
	insert into #tmp_P_FabricInspLabSummaryReport
	select *
	from OPENQUERY([MainServer], ''exec Production.[dbo].[GetFabricInspLabSummaryReport] ''''' + FORMAT(@StartDate, 'yyyy/MM/dd') + ''''', ''''' + FORMAT(@EndDate, 'yyyy/MM/dd') + ''''' '')'
	EXEC sp_executesql @SqlCmd

	/************* 刪除P_FabricInspLabSummaryReport的資料*************/
	delete p
	from P_FabricInspLabSummaryReport p
	where not exists (select 1 from #tmp_P_FabricInspLabSummaryReport t where t.FactoryID = p.FactoryID 
																	AND t.POID = p.POID 
																	AND t.SEQ = p.SEQ
																	AND t.ReceivingID = p.ReceivingID
																	AND t.StockType = p.StockType)
	and ((p.AddDate >= @StartDate and p.AddDate <= @EndDate)
	 or (p.EditDate >= @StartDate and p.EditDate <= @EndDate))

	update p set
		p.Category							= t.Category
		,p.BrandID							= t.BrandID
		,p.StyleID							= t.StyleID
		,p.SeasonID							= t.SeasonID
		,p.Wkno								= t.Wkno
		,p.InvNo							= t.InvNo
		,p.CuttingDate						= t.CuttingDate
		,p.ArriveWHDate						= t.ArriveWHDate
		,p.ArriveQty						= t.ArriveQty
		,p.Inventory						= t.Inventory
		,p.[Bulk]							= t.[Bulk]
		,p.BalanceQty						= t.BalanceQty
		,p.TtlRollsCalculated				= t.TtlRollsCalculated
		,p.BulkLocation						= t.BulkLocation
		,p.FirstUpdateBulkLocationDate		= t.FirstUpdateBulkLocationDate
		,p.InventoryLocation				= t.InventoryLocation
		,p.FirstUpdateStocksLocationDate	= t.FirstUpdateStocksLocationDate
		,p.EarliestSCIDelivery				= t.EarliestSCIDelivery
		,p.BuyerDelivery					= t.BuyerDelivery
		,p.Refno							= t.Refno
		,p.[Description]					= t.[Description]
		,p.Color							= t.Color
		,p.ColorName						= t.ColorName
		,p.SupplierCode						= t.SupplierCode
		,p.SupplierName						= t.SupplierName
		,p.WeaveType						= t.WeaveType
		,p.NAPhysical						= t.NAPhysical 
		,p.InspectionOverallResult			= t.InspectionOverallResult
		,p.PhysicalInspResult				= t.PhysicalInspResult
		,p.TtlYrdsUnderBCGrade 				= t.TtlYrdsUnderBCGrade
		,p.TtlPointsUnderBCGrade 			= t.TtlPointsUnderBCGrade
		,p.TtlPointsUnderAGrade 			= t.TtlPointsUnderAGrade
		,p.PhysicalInspector				= t.PhysicalInspector
		,p.PhysicalInspDate					= t.PhysicalInspDate
		,p.ActTtlYdsInspection				= t.ActTtlYdsInspection
		,p.InspectionPCT					= t.InspectionPCT
		,p.PhysicalInspDefectPoint			= t.PhysicalInspDefectPoint
		,p.CustInspNumber					= t.CustInspNumber
		,p.WeightTestResult					= t.WeightTestResult
		,p.WeightTestInspector				= t.WeightTestInspector
		,p.WeightTestDate					= t.WeightTestDate
		,p.CutShadebandQtyByRoll			= t.CutShadebandQtyByRoll
		,p.CutShadebandPCT					= t.CutShadebandPCT
		,p.ShadeBondResult					= t.ShadeBondResult
		,p.ShadeBondInspector				= t.ShadeBondInspector
		,p.ShadeBondDate					= t.ShadeBondDate
		,p.NoOfRollShadebandPass			= t.NoOfRollShadebandPass
		,p.NoOfRollShadebandFail			= t.NoOfRollShadebandFail
		,p.ContinuityResult					= t.ContinuityResult
		,p.ContinuityInspector				= t.ContinuityInspector
		,p.ContinuityDate					= t.ContinuityDate
		,p.OdorResult						= t.OdorResult
		,p.OdorInspector					= t.OdorInspector
		,p.OdorDate							= t.OdorDate
		,p.MoistureResult					= t.MoistureResult
		,p.MoistureDate						= t.MoistureDate
		,p.CrockingShrinkageOverAllResult	= t.CrockingShrinkageOverAllResult
		,p.NACrocking						= t.NACrocking
		,p.CrockingResult					= t.CrockingResult
		,p.CrockingInspector				= t.CrockingInspector
		,p.CrockingTestDate					= t.CrockingTestDate
		,p.NAHeatShrinkage					= t.NAHeatShrinkage
		,p.HeatShrinkageTestResult			= t.HeatShrinkageTestResult
		,p.HeatShrinkageInspector			= t.HeatShrinkageInspector
		,p.HeatShrinkageTestDate			= t.HeatShrinkageTestDate
		,p.NAWashShrinkage					= t.NAWashShrinkage
		,p.WashShrinkageTestResult			= t.WashShrinkageTestResult
		,p.WashShrinkageInspector			= t.WashShrinkageInspector
		,p.WashShrinkageTestDate			= t.WashShrinkageTestDate
		,p.OvenTestResult					= t.OvenTestResult
		,p.OvenTestInspector				= t.OvenTestInspector
		,p.ColorFastnessResult				= t.ColorFastnessResult
		,p.ColorFastnessInspector			= t.ColorFastnessInspector
		,p.LocalMR							= t.LocalMR
		,p.OrderType						= t.OrderType
		,p.AddDate							= t.AddDate
		,p.EditDate							= t.EditDate
	from P_FabricInspLabSummaryReport p
	inner join #tmp_P_FabricInspLabSummaryReport t on p.FactoryID = t.FactoryID 
													AND p.POID = t.POID 
													AND p.SEQ = t.SEQ
													AND p.ReceivingID = t.ReceivingID
													AND p.StockType = t.StockType


	insert into P_FabricInspLabSummaryReport([Category], [POID], [SEQ], [FactoryID], [BrandID]
	, [StyleID], [SeasonID], [Wkno], [InvNo], [CuttingDate], [ArriveWHDate], [ArriveQty]
	, [Inventory], [Bulk], [BalanceQty], [TtlRollsCalculated], [BulkLocation], [FirstUpdateBulkLocationDate]
	, [InventoryLocation], [FirstUpdateStocksLocationDate], [EarliestSCIDelivery], [BuyerDelivery]
	, [Refno], [Description], [Color], [ColorName], [SupplierCode], [SupplierName], [WeaveType]
	, [NAPhysical], [InspectionOverallResult], [PhysicalInspResult], [TtlYrdsUnderBCGrade]
	, [TtlPointsUnderBCGrade], [TtlPointsUnderAGrade], [PhysicalInspector], [PhysicalInspDate]
	, [ActTtlYdsInspection], [InspectionPCT], [PhysicalInspDefectPoint], [CustInspNumber], [WeightTestResult]
	, [WeightTestInspector], [WeightTestDate], [CutShadebandQtyByRoll], [CutShadebandPCT], [ShadeBondResult]
	, [ShadeBondInspector], [ShadeBondDate], [NoOfRollShadebandPass], [NoOfRollShadebandFail]
	, [ContinuityResult], [ContinuityInspector], [ContinuityDate], [OdorResult], [OdorInspector]
	, [OdorDate], [MoistureResult], [MoistureDate], [CrockingShrinkageOverAllResult], [NACrocking]
	, [CrockingResult], [CrockingInspector], [CrockingTestDate], [NAHeatShrinkage], [HeatShrinkageTestResult]
	, [HeatShrinkageInspector], [HeatShrinkageTestDate], [NAWashShrinkage], [WashShrinkageTestResult]
	, [WashShrinkageInspector], [WashShrinkageTestDate], [OvenTestResult], [OvenTestInspector]
	, [ColorFastnessResult], [ColorFastnessInspector], [LocalMR], [OrderType], [ReceivingID], [AddDate]
	, [EditDate], [StockType])
	SELECT [Category], [POID], [SEQ], [FactoryID], [BrandID]
	, [StyleID], [SeasonID], [Wkno], [InvNo], [CuttingDate], [ArriveWHDate], [ArriveQty]
	, [Inventory], [Bulk], [BalanceQty], [TtlRollsCalculated], [BulkLocation], [FirstUpdateBulkLocationDate]
	, [InventoryLocation], [FirstUpdateStocksLocationDate], [EarliestSCIDelivery], [BuyerDelivery]
	, [Refno], [Description], [Color], [ColorName], [SupplierCode], [SupplierName], [WeaveType]
	, [NAPhysical], [InspectionOverallResult], [PhysicalInspResult], [TtlYrdsUnderBCGrade]
	, [TtlPointsUnderBCGrade], [TtlPointsUnderAGrade], [PhysicalInspector], [PhysicalInspDate]
	, [ActTtlYdsInspection], [InspectionPCT], [PhysicalInspDefectPoint], [CustInspNumber], [WeightTestResult]
	, [WeightTestInspector], [WeightTestDate], [CutShadebandQtyByRoll], [CutShadebandPCT], [ShadeBondResult]
	, [ShadeBondInspector], [ShadeBondDate], [NoOfRollShadebandPass], [NoOfRollShadebandFail]
	, [ContinuityResult], [ContinuityInspector], [ContinuityDate], [OdorResult], [OdorInspector]
	, [OdorDate], [MoistureResult], [MoistureDate], [CrockingShrinkageOverAllResult], [NACrocking]
	, [CrockingResult], [CrockingInspector], [CrockingTestDate], [NAHeatShrinkage], [HeatShrinkageTestResult]
	, [HeatShrinkageInspector], [HeatShrinkageTestDate], [NAWashShrinkage], [WashShrinkageTestResult]
	, [WashShrinkageInspector], [WashShrinkageTestDate], [OvenTestResult], [OvenTestInspector]
	, [ColorFastnessResult], [ColorFastnessInspector], [LocalMR], [OrderType], [ReceivingID], [AddDate]
	, [EditDate], [StockType]
	from #tmp_P_FabricInspLabSummaryReport t
	where not exists (select 1 from P_FabricInspLabSummaryReport p where p.FactoryID = t.FactoryID 
																	and p.POID = t.POID 
																	and p.SEQ = t.SEQ
																	and p.ReceivingID = t.ReceivingID
																	and p.StockType = t.StockType)

	IF EXISTS (select 1 from BITableInfo b where b.id = 'P_FabricInspLabSummaryReport')
	BEGIN
		update b
			set b.TransferDate = getdate()
		from BITableInfo b
		where b.id = 'P_FabricInspLabSummaryReport'
	END
	ELSE 
	BEGIN
		insert into BITableInfo(Id, TransferDate)
		values('P_FabricInspLabSummaryReport', getdate())
	END

END


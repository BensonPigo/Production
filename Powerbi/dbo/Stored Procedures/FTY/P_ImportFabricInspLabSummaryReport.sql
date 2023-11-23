CREATE PROCEDURE [dbo].[P_ImportFabricInspLabSummaryReport]
as
begin
SET NOCOUNT ON;
	declare @current_PMS_ServerName nvarchar(50) = 'MainServer'
	declare @SqlCmd nvarchar(max) ='';
	declare @SqlCmdDelete nvarchar(max) ='';
	declare @SqlCmdUpdata nvarchar(max) ='';
	declare @SqlCmdinsert nvarchar(max) ='';
	set @SqlCmd = '
	/************* 抓QA.R01報表資料*************/
	select 
	*
	into #tmp
	from OPENQUERY(['+@current_PMS_ServerName+'],
	''exec Production.[dbo].[GetFabricInspLabSummaryReport]'')'

	set @SqlCmdDelete = '
	/************* 刪除P_FabricInspLabSummaryReport的資料*************/
	Delete P_FabricInspLabSummaryReport
	from P_FabricInspLabSummaryReport as a 
	inner join #tmp as b on a.FactoryID = b.FactoryID 
						AND a.POID = b.POID 
						AND A.SEQ = B.SEQ
						AND A.ReceivingID = B.ReceivingID
	WHERE B.FactoryID IS NULL AND B.POID IS NULL AND B.SEQ IS NULL AND B.ReceivingID IS NULL'

	set @SqlCmdUpdata = '
	update a set
	a.Category							= isnull(b.Category,'''')
	,a.BrandID							= isnull(b.BrandID,'''')
	,a.StyleID							= isnull(b.StyleID,'''')
	,a.SeasonID							= isnull(b.SeasonID,'''')
	,a.Wkno								= isnull(b.Wkno,'''')
	,a.InvNo							= isnull(b.InvNo,'''')
	,a.CuttingDate						= b.CuttingDate
	,a.ArriveWHDate						= b.ArriveWHDate
	,a.ArriveQty						= isnull(b.ArriveQty,0)
	,a.Inventory						= isnull(b.Inventory,0)
	,a.[Bulk]							= isnull(b.[Bulk],0)
	,a.BalanceQty						= isnull(b.BalanceQty,0)
	,a.TtlRollsCalculated				= isnull(b.TtlRollsCalculated,0)
	,a.BulkLocation						= isnull(b.BulkLocation,'''')
	,a.FirstUpdateBulkLocationDate		= b.FirstUpdateBulkLocationDate
	,a.InventoryLocation				= isnull(b.InventoryLocation,'''')
	,a.FirstUpdateStocksLocationDate	= b.FirstUpdateStocksLocationDate
	,a.EarliestSCIDelivery				= b.EarliestSCIDelivery
	,a.BuyerDelivery					= b.BuyerDelivery
	,a.Refno							= isnull(b.Refno,'''')
	,a.Description						= isnull(b.Description,'''')
	,a.Color							= isnull(b.Color,'''')
	,a.ColorName						= isnull(b.ColorName,'''')
	,a.SupplierCode						= isnull(b.SupplierCode,'''')
	,a.SupplierName						= isnull(b.SupplierName,'''')
	,a.WeaveType						= isnull(b.WeaveType,'''')
	,a.NAPhysical						= isnull(b.NAPhysical,'''') 
	,a.InspectionOverallResult			= isnull(b.InspectionOverallResult,'''')
	,a.PhysicalInspResult				= isnull(b.PhysicalInspResult,'''')
	,a.TtlYrdsUnderBCGrade 				= isnull(b.TtlYrdsUnderBCGrade,0)
	,a.TtlPointsUnderBCGrade 			= isnull(b.TtlPointsUnderBCGrade,0)
	,a.TtlPointsUnderAGrade 			= isnull(b.TtlPointsUnderAGrade,0)
	,a.PhysicalInspector				= isnull(b.PhysicalInspector,'''')
	,a.PhysicalInspDate					= b.PhysicalInspDate
	,a.ActTtlYdsInspection				= isnull(b.ActTtlYdsInspection,0)
	,a.InspectionPCT					= isnull(b.InspectionPCT,0)
	,a.PhysicalInspDefectPoint			= isnull(b.PhysicalInspDefectPoint,0)
	,a.CustInspNumber					= isnull(b.CustInspNumber,'''')
	,a.WeightTestResult					= isnull(b.WeightTestResult,'''')
	,a.WeightTestInspector				= isnull(b.WeightTestInspector,'''')
	,a.WeightTestDate					= b.WeightTestDate
	,a.CutShadebandQtyByRoll			= isnull(b.CutShadebandQtyByRoll,0)
	,a.CutShadebandPCT					= isnull(b.CutShadebandPCT,0)
	,a.ShadeBondResult					= isnull(b.ShadeBondResult,'''')
	,a.ShadeBondInspector				= isnull(b.ShadeBondInspector,'''')
	,a.ShadeBondDate					= b.ShadeBondDate
	,a.NoOfRollShadebandPass			= isnull(b.NoOfRollShadebandPass,0)
	,a.NoOfRollShadebandFail			= isnull(b.NoOfRollShadebandFail,0)
	,a.ContinuityResult					= isnull(b.ContinuityResult,'''')
	,a.ContinuityInspector				= isnull(b.ContinuityInspector,'''')
	,a.ContinuityDate					= b.ContinuityDate
	,a.OdorResult						= isnull(b.OdorResult,'''')
	,a.OdorInspector					= isnull(b.OdorInspector,'''')
	,a.OdorDate							= b.OdorDate
	,a.MoistureResult					= isnull(b.MoistureResult,'''')
	,a.MoistureDate						= b.MoistureDate
	,a.CrockingShrinkageOverAllResult	= isnull(b.CrockingShrinkageOverAllResult,'''')
	,a.NACrocking						= isnull(b.NACrocking,'''')
	,a.CrockingResult					= isnull(b.CrockingResult,'''')
	,a.CrockingInspector				= isnull(b.CrockingInspector,'''')
	,a.CrockingTestDate					= b.CrockingTestDate
	,a.NAHeatShrinkage					= isnull(b.NAHeatShrinkage,'''')
	,a.HeatShrinkageTestResult			= isnull(b.HeatShrinkageTestResult,'''')
	,a.HeatShrinkageInspector			= isnull(b.HeatShrinkageInspector,'''')
	,a.HeatShrinkageTestDate			= b.HeatShrinkageTestDate
	,a.NAWashShrinkage					= isnull(b.NAWashShrinkage,'''')
	,a.WashShrinkageTestResult			= isnull(b.WashShrinkageTestResult,'''')
	,a.WashShrinkageInspector			= isnull(b.WashShrinkageInspector,'''')
	,a.WashShrinkageTestDate			= b.WashShrinkageTestDate
	,a.OvenTestResult					= isnull(b.OvenTestResult,'''')
	,a.OvenTestInspector				= isnull(b.OvenTestInspector,'''')
	,a.ColorFastnessResult				= isnull(b.ColorFastnessResult,'''')
	,a.ColorFastnessInspector			= isnull(b.ColorFastnessInspector,'''')
	,a.LocalMR							= isnull(b.LocalMR,'''')
	,a.OrderType						= isnull(b.OrderType,'''')
	,a.StockType                        = isnull(b.StockType,'''')
	,a.AddDate							= b.AddDate
	,a.EditDate							= b.EditDate
	from P_FabricInspLabSummaryReport a
	inner join #tmp as b on a.FactoryID = b.FactoryID 
					AND a.POID = b.POID 
					AND A.SEQ = B.SEQ
					AND A.ReceivingID = B.ReceivingID
	'
	
	set @SqlCmdinsert = '
	insert into P_FabricInspLabSummaryReport (
	Category
	,POID
	,SEQ
	,FactoryID
	,BrandID
	,StyleID
	,SeasonID
	,Wkno
	,InvNo
	,CuttingDate
	,ArriveWHDate
	,ArriveQty
	,Inventory
	,[Bulk]
	,BalanceQty
	,TtlRollsCalculated
	,BulkLocation
	,FirstUpdateBulkLocationDate
	,InventoryLocation
	,FirstUpdateStocksLocationDate
	,EarliestSCIDelivery
	,BuyerDelivery
	,Refno
	,Description
	,Color
	,ColorName
	,SupplierCode
	,SupplierName
	,WeaveType
	,NAPhysical
	,InspectionOverallResult
	,PhysicalInspResult
	,TtlYrdsUnderBCGrade 
	,TtlPointsUnderBCGrade 
	,TtlPointsUnderAGrade 
	,PhysicalInspector
	,PhysicalInspDate
	,ActTtlYdsInspection
	,InspectionPCT
	,PhysicalInspDefectPoint
	,CustInspNumber
	,WeightTestResult
	,WeightTestInspector
	,WeightTestDate
	,CutShadebandQtyByRoll
	,CutShadebandPCT
	,ShadeBondResult
	,ShadeBondInspector
	,ShadeBondDate
	,NoOfRollShadebandPass
	,NoOfRollShadebandFail
	,ContinuityResult
	,ContinuityInspector
	,ContinuityDate
	,OdorResult
	,OdorInspector
	,OdorDate
	,MoistureResult
	,MoistureDate
	,CrockingShrinkageOverAllResult
	,NACrocking
	,CrockingResult
	,CrockingInspector
	,CrockingTestDate
	,NAHeatShrinkage
	,HeatShrinkageTestResult
	,HeatShrinkageInspector
	,HeatShrinkageTestDate
	,NAWashShrinkage
	,WashShrinkageTestResult
	,WashShrinkageInspector
	,WashShrinkageTestDate
	,OvenTestResult
	,OvenTestInspector
	,ColorFastnessResult
	,ColorFastnessInspector
	,LocalMR
	,OrderType
	,ReceivingID
	,StockType
	,AddDate
	,EditDate
	)
	SELECT
	 isnull(b.Category,'''')
	,ISNULL(B.POID,'''')
	,ISNULL(B.SEQ,'''')
	,ISNULL(B.FactoryID,'''')
	,isnull(b.BrandID,'''')
	,isnull(b.StyleID,'''')
	,isnull(b.SeasonID,'''')
	,isnull(b.Wkno,'''')
	,isnull(b.InvNo,'''')
	,b.CuttingDate
	,b.ArriveWHDate
	,isnull(b.ArriveQty,0)
	,isnull(b.Inventory,0)
	,isnull(b.[Bulk],0)
	,isnull(b.BalanceQty,0)
	,isnull(b.TtlRollsCalculated,0)
	,isnull(b.BulkLocation,'''')
	,b.FirstUpdateBulkLocationDate
	,isnull(b.InventoryLocation,'''')
	,b.FirstUpdateStocksLocationDate
	,b.EarliestSCIDelivery
	,b.BuyerDelivery
	,isnull(b.Refno,'''')
	,isnull(b.Description,'''')
	,isnull(b.Color,'''')
	,isnull(b.ColorName,'''')
	,isnull(b.SupplierCode,'''')
	,isnull(b.SupplierName,'''')
	,isnull(b.WeaveType,'''')
	,isnull(b.NAPhysical,'''') 
	,isnull(b.InspectionOverallResult,'''')
	,isnull(b.PhysicalInspResult,'''')
	,isnull(b.TtlYrdsUnderBCGrade,0)
	,isnull(b.TtlPointsUnderBCGrade,0)
	,isnull(b.TtlPointsUnderAGrade,0)
	,isnull(b.PhysicalInspector,'''')
	,b.PhysicalInspDate
	,isnull(b.ActTtlYdsInspection,0)
	,isnull(b.InspectionPCT,0)
	,isnull(b.PhysicalInspDefectPoint,0)
	,isnull(b.CustInspNumber,'''')
	,isnull(b.WeightTestResult,'''')
	,isnull(b.WeightTestInspector,'''')
	,b.WeightTestDate
	,isnull(b.CutShadebandQtyByRoll,0)
	,isnull(b.CutShadebandPCT,0)
	,isnull(b.ShadeBondResult,'''')
	,isnull(b.ShadeBondInspector,'''')
	,b.ShadeBondDate
	,isnull(b.NoOfRollShadebandPass,0)
	,isnull(b.NoOfRollShadebandFail,0)
	,isnull(b.ContinuityResult,'''')
	,isnull(b.ContinuityInspector,'''')
	,b.ContinuityDate
	,isnull(b.OdorResult,'''')
	,isnull(b.OdorInspector,'''')
	,b.OdorDate
	,isnull(b.MoistureResult,'''')
	,b.MoistureDate
	,isnull(b.CrockingShrinkageOverAllResult,'''')
	,isnull(b.NACrocking,'''')
	,isnull(b.CrockingResult,'''')
	,isnull(b.CrockingInspector,'''')
	,b.CrockingTestDate
	,isnull(b.NAHeatShrinkage,'''')
	,isnull(b.HeatShrinkageTestResult,'''')
	,isnull(b.HeatShrinkageInspector,'''')
	,b.HeatShrinkageTestDate
	,isnull(b.NAWashShrinkage,'''')
	,isnull(b.WashShrinkageTestResult,'''')
	,isnull(b.WashShrinkageInspector,'''')
	,b.WashShrinkageTestDate
	,isnull(b.OvenTestResult,'''')
	,isnull(b.OvenTestInspector,'''')
	,isnull(b.ColorFastnessResult,'''')
	,isnull(b.ColorFastnessInspector,'''')
	,isnull(b.LocalMR,'''')
	,isnull(b.OrderType,'''')
	,ISNULL(B.ReceivingID,'''')
	,isnull(b.StockType,'''')
	,b.AddDate
	,b.EditDate
	FROM #tmp B
	left join P_FabricInspLabSummaryReport A on A.FactoryID = B.FactoryID 
											AND A.POID = B.POID 
											AND A.SEQ = B.SEQ
						                    AND A.ReceivingID = B.ReceivingID
	where A.FactoryID is null AND A.POID is null AND A.SEQ is null AND A.ReceivingID is null



	IF EXISTS (select 1 from BITableInfo b where b.id = ''P_ImportFabricInspLabSummaryReport'')
	BEGIN
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = ''P_ImportFabricInspLabSummaryReport''
	END
	ELSE 
	BEGIN
		insert into BITableInfo(Id, TransferDate)
		values(''P_ImportFabricInspLabSummaryReport'', getdate())
	END
	'
	DECLARE @SqlCmdAll nVARCHAR(MAX);
	set @SqlCmdAll = @SqlCmd + @SqlCmdDelete+@SqlCmdUpdata+@SqlCmdinsert
	EXEC sp_executesql @Sqlcmdall
end



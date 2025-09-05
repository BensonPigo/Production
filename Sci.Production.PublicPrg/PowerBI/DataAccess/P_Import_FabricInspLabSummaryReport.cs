using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_FabricInspLabSummaryReport
    {
        /// <inheritdoc/>
        public Base_ViewModel P_FabricInspLabSummaryReport(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            QA_R01 biModel = new QA_R01();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddMonths(-3).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Now;
            }

            try
            {
                QA_R01_ViewModel qa_R01_ViewModel = new QA_R01_ViewModel()
                {
                    StartBIDate = item.SDate,
                    EndBIDate = item.EDate,
                    IsBI = true,
                };

                Base_ViewModel resultReport = biModel.Get_QA_R01(qa_R01_ViewModel);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.DtArr[0];

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        /// <inheritdoc/>
        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;

            string where = @"((p.AddDate >= @StartDate and p.AddDate <= @EndDate)  or (p.EditDate >= @StartDate and p.EditDate <= @EndDate))";

            string tmp = new Base().SqlBITableHistory("P_FabricInspLabSummaryReport", "P_FabricInspLabSummaryReport_History", "#tmp", where, false, true);

            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"

            -- 更新
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
            ,p.[TotalYardageForInspection]      = t.TotalYardage
            ,p.[ActualRemainingYardsForInspection]	= t.TotalYardageArrDate

            ,p.[KPILETA]                    = t.[KPILETA] 
		    ,p.[ACTETA]                     = t.[ACTETA] 
		    ,p.[Packages]                   = t.[Packages]
		    ,p.[SampleRcvDate]              = t.[SampleRcvDate]
		    ,p.[InspectionGroup]            = t.[InspectionGroup]
		    ,p.[CGradeTOP3Defects]          = t.[CGradeTOP3Defects]
		    ,p.[AGradeTOP3Defects]          = t.[AGradeTOP3Defects]
		    ,p.[TotalLotNumber]             = t.[TotalLotNumber]
		    ,p.[InspectedLotNumber]         = t.[InspectedLotNumber]
		    ,p.[CutShadebandTime]           = t.[CutShadebandTime]
		    ,p.[OvenTestDate]               = t.[OvenTestDate]
		    ,p.[ColorFastnessTestDate]      = t.[ColorFastnessTestDate]
		    ,p.[MCHandle]                   = t.[MCHandle] 
		    ,p.[OrderQty]                   = t.[OrderQty]
            ,p.[ActTotalRollInspection]     = t.[ActTotalRollInspection]
			,p.[Complete]                   = t.[Complete]

            ,p.[BIFactoryID]                = @BIFactoryID
            ,p.[BIInsertDate]               = GETDATE()
            ,p.[BIStatus]                   = 'New'
			from P_FabricInspLabSummaryReport p
			inner join #tmp t on p.FactoryID = t.FactoryID 
							 AND p.POID = t.POID 
							 AND p.SEQ = t.SEQ
							 AND p.ReceivingID = t.ReceivingID
							 AND p.StockType = t.StockType

            --新增
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
	        , [EditDate], [StockType],[TotalYardageForInspection],[ActualRemainingYardsForInspection] ,[KPILETA],[ACTETA] 
            ,[Packages],[SampleRcvDate],[InspectionGroup],[CGradeTOP3Defects],[AGradeTOP3Defects],[TotalLotNumber]
            ,[InspectedLotNumber],[CutShadebandTime],[OvenTestDate],[ColorFastnessTestDate] ,[MCHandle],[OrderQty],[ActTotalRollInspection],[Complete], [BIFactoryID], [BIInsertDate], [BIStatus])
            SELECT                                                                                                
              [Category], [POID], [SEQ], [FactoryID], [BrandID]
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
	        , [EditDate], [StockType],t.TotalYardage,t.TotalYardageArrDate,[KPILETA],[ACTETA] ,[Packages]
            ,[SampleRcvDate],[InspectionGroup],[CGradeTOP3Defects],[AGradeTOP3Defects],[TotalLotNumber]
            ,[InspectedLotNumber],[CutShadebandTime],[OvenTestDate],[ColorFastnessTestDate] 
            ,[MCHandle],[OrderQty] ,[ActTotalRollInspection],[Complete], @BIFactoryID, GETDATE(), 'New'
            from #tmp t
	        where not exists (select 1 from P_FabricInspLabSummaryReport p where p.FactoryID = t.FactoryID 
																	         and p.POID = t.POID 
																	         and p.SEQ = t.SEQ
																	         and p.ReceivingID = t.ReceivingID
																	         and p.StockType = t.StockType)
             {tmp}

            /************* 刪除P_FabricInspLabSummaryReport的資料*************/
	        delete p
	        from P_FabricInspLabSummaryReport p
	        where not exists (select 1 from #tmp t where t.FactoryID = p.FactoryID 
																	        AND t.POID = p.POID 
																	        AND t.SEQ = p.SEQ
																	        AND t.ReceivingID = p.ReceivingID
																	        AND t.StockType = p.StockType)
	        and ((p.AddDate >= @StartDate and p.AddDate <= @EndDate)
	            or (p.EditDate >= @StartDate and p.EditDate <= @EndDate))
            ";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", item.SDate),
                    new SqlParameter("@EndDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };

                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sqlcmd, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

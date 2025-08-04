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
    public class P_Import_WIP
    {
        /// <inheritdoc/>
        public Base_ViewModel P_WIP(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Planning_R15 planning_R15 = new Planning_R15();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-30).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.AddDays(+30).ToString("yyyy/MM/dd"));
            }

            try
            {
                Planning_R15_ViewModel r15_vm = new Planning_R15_ViewModel()
                {
                    StartSciDelivery = item.SDate,
                    EndSciDelivery = item.EDate,
                    Category = "'B'",
                    OrderBy = "orderId",
                    SummaryBy = "1",
                    FormParameter = "1",
                    RFIDProcessLocation = string.Empty,
                    IsBI = true,
                };

                Base_ViewModel resultReport = planning_R15.GetPlanning_R15(r15_vm, null);
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
            string where = @" p.SciDelivery between @StartDate and @EndDate and NOT EXISTS(SELECT 1 FROM #tmp t WHERE P.SPNO = T.OrderID)";
            string tmp = new Base().SqlBITableHistory("P_WIP", "P_WIP_History", "#tmp", where, false, false);

            Base_ViewModel finalResult;
            string sqlCmd = $@"
              -- 更新
            UPDATE P SET						
             P.[MDivisionID]					=	ISNULL(T.[MDivisionID],'')
            ,P.[FactoryID]						=	ISNULL(T.[FactoryID],'')
            ,P.[SewLine]						=	ISNULL(T.[SewLine],'')
            ,P.[BuyerDelivery]					=	T.[OrdersBuyerDelivery]
            ,P.[SciDelivery]					=	T.[SciDelivery]
            ,P.[SewInLine]						=	T.[SewInLine]	
            ,P.[SewOffLine]						=	T.[SewOffLine]	
            ,P.[IDD]							=	ISNULL(T.[val],'')
            ,P.[BrandID]						=	ISNULL(T.[BrandID],'')
            ,P.[MasterSP]						=	ISNULL(T.[POID],'')
            ,P.[IsBuyBack]						=	ISNULL(T.[Buy Back],'')
            ,P.[Cancelled]						=	ISNULL(T.[Cancelled],'')
            ,P.[CancelledStillNeedProd]			=	ISNULL(T.[Cancelled but Sill],'')
            ,P.[Dest]							=	ISNULL(T.[Dest],'')
            ,P.[StyleID]						=	ISNULL(T.[StyleID],'')
            ,P.[OrderTypeID]					=	ISNULL(T.[OrderTypeID],'')
            ,P.[ShipMode]						=	ISNULL(T.[ShipModeList],'')
            ,P.[PartialShipping]				=	ISNULL(T.[PartialShipping],'')
            ,P.[OrderNo]						=	ISNULL(T.[OrderNo],'')
            ,P.[PONO]							=	ISNULL(T.[CustPONo],'')
            ,P.[ProgramID]						=	ISNULL(T.[ProgramID],'')
            ,P.[CdCodeID]						=	ISNULL(T.[CdCodeID],'')
            ,P.[CDCodeNew]						=	ISNULL(T.[CDCodeNew],'')	
            ,P.[ProductType]					=	ISNULL(T.[ProductType],'')
            ,P.[FabricType]						=	ISNULL(T.[FabricType],'')	
            ,P.[Lining]							=	ISNULL(T.[Lining],'')	
            ,P.[Gender]							=	ISNULL(T.[Gender],'')	
            ,P.[Construction]					=	ISNULL(T.[Construction],'')
            ,P.[KPILETA]						=	T.[KPILETA]
            ,P.[SCHDLETA]						=	T.[LETA]
            ,P.[ActMTLETA_Master SP]			=	T.[MTLETA]
            ,P.[SewMTLETA_SP]					=	T.[SewETA]	
            ,P.[PkgMTLETA_SP]					=	T.[PackETA]	
            ,P.[Cpu]							=	ISNULL(T.[Cpu],0)
            ,P.[TTLCPU]							=	ISNULL(T.[TTL CPU],0)	
            ,P.[CPUClosed]						=	ISNULL(T.[CPU Closed],0)
            ,P.[CPUBal]							=	ISNULL(T.[CPU bal],0)	
            ,P.[Article]						=	ISNULL(T.[article_list],'')
            ,P.[Qty]							=	ISNULL(T.[Qty],0)
            ,P.[StandardOutput]					=	ISNULL(T.[StandardOutput],'')
            ,P.[OrigArtwork]					=	ISNULL(T.[oriArtwork],'')
            ,P.[AddedArtwork]					=	ISNULL(T.[AddedArtwork],'')	
            ,P.[BundleArtwork]					=	ISNULL(T.[Artwork],'')
            ,P.[SubProcessDest]					=	ISNULL(T.[SubProcessDest],'')
            ,P.[EstCutDate]						=	T.[EstimatedCutDate]
            ,P.[1stCutDate]						=	T.[first_cut_date]
            ,P.[CutQty]							=	ISNULL(T.[cut_qty],0)
            ,P.[RFIDCutQty]						=	ISNULL(T.[RFID Cut Qty],0)	
            ,P.[RFIDSewingLineInQty]			=	ISNULL(T.[RFID SewingLine In Qty],0)
            ,P.[RFIDLoadingQty]					=	ISNULL(T.[RFID Loading Qty],0)	
            ,P.[RFIDEmbFarmInQty]				=	ISNULL(T.[RFID Emb Farm In Qty],0)
            ,P.[RFIDEmbFarmOutQty]				=	ISNULL(T.[RFID Emb Farm Out Qty],0)	
            ,P.[RFIDBondFarmInQty]				=	ISNULL(T.[RFID Bond Farm In Qty],0)
            ,P.[RFIDBondFarmOutQty]				=	ISNULL(T.[RFID Bond Farm Out Qty],0)
            ,P.[RFIDPrintFarmInQty]				=	ISNULL(T.[RFID Print Farm In Qty],0)
            ,P.[RFIDPrintFarmOutQty]			=	ISNULL(T.[RFID Print Farm Out Qty],0)
            ,P.[RFIDATFarmInQty]				=	ISNULL(T.[RFID AT Farm In Qty],0)
            ,P.[RFIDATFarmOutQty]				=	ISNULL(T.[RFID AT Farm Out Qty],0)
            ,P.[RFIDPadPrintFarmInQty]			=	ISNULL(T.[RFID Pad Print Farm In Qty],0)
            ,P.[RFIDPadPrintFarmOutQty]			=	ISNULL(T.[RFID Pad Print Farm Out Qty],0)
            ,P.[RFIDEmbossDebossFarmInQty]		=	ISNULL(T.[RFID Emboss Farm In Qty],0)	
            ,P.[RFIDEmbossDebossFarmOutQty]		=	ISNULL(T.[RFID Emboss Farm Out Qty],0)
            ,P.[RFIDHTFarmInQty]				=	ISNULL(T.[RFID HT Farm In Qty],0)
            ,P.[RFIDHTFarmOutQty]				=	ISNULL(T.[RFID HT Farm Out Qty],0)
            ,P.[RFIDAUTFarmInQty]				=	ISNULL(T.[RFID AUT Farm In Qty],0)
            ,P.[RFIDAUTFarmOutQty]				=	ISNULL(T.[RFID AUT Farm Out Qty],0)
            ,P.[RFIDFMFarmInQty]				=	ISNULL(T.[RFID FM Farm In Qty],0)
            ,P.[RFIDFMFarmOutQty]				=	ISNULL(T.[RFID FM Farm Out Qty],0)
            ,P.[SubProcessStatus]				=	ISNULL(T.[SubProcessStatus],'')	
            ,P.[EmbQty]							=	ISNULL(T.[EMBROIDERY_qty],0)
            ,P.[BondQty]						=	ISNULL(T.[BONDING_qty],0)
            ,P.[PrintQty]						=	ISNULL(T.[PRINTING_qty],0)	
            ,P.[SewQty]							=	ISNULL(T.[sewing_output],0)	
            ,P.[SewBal]							=	ISNULL(T.[Balance],'')
            ,P.[1stSewDate]						=	T.[firstSewingDate]
            ,P.[LastSewDate]					=	T.[Last Sewn Date]
            ,P.[AverageDailyOutput]				=	ISNULL(T.[AVG_QAQTY],0)
            ,P.[EstOfflinedate]					=	T.[Est_offline]
            ,P.[ScannedQty]						=	ISNULL(T.[Scanned_Qty],0)	
            ,P.[PackedRate]						=	ISNULL(T.[pack_rate],0)
            ,P.[TTLCTN]							=	ISNULL(T.[TotalCTN],0)
            ,P.[FtyCTN]							=	ISNULL(T.[FtyCtn],0)
            ,P.[cLogCTN]						=	ISNULL(T.[ClogCTN],0)
            ,P.[CFACTN]							=	ISNULL(T.[CFACTN],0)	
            ,P.[InspDate]						=	ISNULL(T.[InspDate],'')
            ,P.[InspResult]						=	ISNULL(T.[InspResult],'')
            ,P.[CFAName]						=	ISNULL(T.[CFA Name],'')
            ,P.[ActPulloutDate]					=	T.[ActPulloutDate]	
            ,P.[KPIDeliveryDate]				=	T.[FtyKPI]
            ,P.[UpdateDeliveryReason]			=	ISNULL(T.[KPIChangeReason],'')
            ,P.[PlanDate]						=	T.[PlanDate]	
            ,P.[SMR]							=	ISNULL(T.[SMR],'')
            ,P.[Handle]							=	ISNULL(T.[Handle],'')	
            ,P.[Posmr]							=	ISNULL(T.[PO SMR],'')
            ,P.[PoHandle]						=	ISNULL(T.[PO Handle],'')
            ,P.[MCHandle]						=	ISNULL(T.[MC Handle],'')
            ,P.[doxtype]						=	ISNULL(T.[DoxType],'')
            ,P.[SpecialMark]					=	ISNULL(T.[SpecMark],'')
            ,P.[GlobalFoundationRange]			=	ISNULL(T.[GFR],0)
            ,P.[SampleReason]					=	ISNULL(T.[SampleReason],'')
            ,P.[TMS]							=	ISNULL(T.[TMS],0)
            ,P.BIFactoryID                      =   @BIFactoryID
            ,P.BIInsertDate                     =   GetDate()
            ,P.BIStatus                         =   'New'
            FROM P_WIP P						
            INNER JOIN #tmp T ON P.SPNO = T.OrderID 

            -- 新增
            INSERT INTO P_WIP
            (
	            [MDivisionID]
	            ,[FactoryID]
	            ,[SewLine]
	            ,[BuyerDelivery]
	            ,[SciDelivery]
	            ,[SewInLine]
	            ,[SewOffLine]
	            ,[IDD]
	            ,[BrandID]
	            ,[SPNO]
	            ,[MasterSP]
	            ,[IsBuyBack]
	            ,[Cancelled]
	            ,[CancelledStillNeedProd]
	            ,[Dest]
	            ,[StyleID]
	            ,[OrderTypeID]
	            ,[ShipMode]
	            ,[PartialShipping]
	            ,[OrderNo]
	            ,[PONO]
	            ,[ProgramID]
	            ,[CdCodeID]
	            ,[CDCodeNew]
	            ,[ProductType]
	            ,[FabricType]
	            ,[Lining]
	            ,[Gender]
	            ,[Construction]
	            ,[KPILETA]
	            ,[SCHDLETA]
	            ,[ActMTLETA_Master SP]
	            ,[SewMTLETA_SP]
	            ,[PkgMTLETA_SP]
	            ,[Cpu]
	            ,[TTLCPU]
	            ,[CPUClosed]
	            ,[CPUBal]
	            ,[Article]
	            ,[Qty]
	            ,[StandardOutput]
	            ,[OrigArtwork]
	            ,[AddedArtwork]
	            ,[BundleArtwork]
	            ,[SubProcessDest]
	            ,[EstCutDate]
	            ,[1stCutDate]
	            ,[CutQty]
	            ,[RFIDCutQty]
	            ,[RFIDSewingLineInQty]
	            ,[RFIDLoadingQty]
	            ,[RFIDEmbFarmInQty]
	            ,[RFIDEmbFarmOutQty]
	            ,[RFIDBondFarmInQty]
	            ,[RFIDBondFarmOutQty]
	            ,[RFIDPrintFarmInQty]
	            ,[RFIDPrintFarmOutQty]
	            ,[RFIDATFarmInQty]
	            ,[RFIDATFarmOutQty]
	            ,[RFIDPadPrintFarmInQty]
	            ,[RFIDPadPrintFarmOutQty]
	            ,[RFIDEmbossDebossFarmInQty]
	            ,[RFIDEmbossDebossFarmOutQty]
	            ,[RFIDHTFarmInQty]
	            ,[RFIDHTFarmOutQty]
	            ,[RFIDAUTFarmInQty]
	            ,[RFIDAUTFarmOutQty]
	            ,[RFIDFMFarmInQty]
	            ,[RFIDFMFarmOutQty]
	            ,[SubProcessStatus]
	            ,[EmbQty]
	            ,[BondQty]
	            ,[PrintQty]
	            ,[SewQty]
	            ,[SewBal]
	            ,[1stSewDate]
	            ,[LastSewDate]
	            ,[AverageDailyOutput]
	            ,[EstOfflinedate]
	            ,[ScannedQty]
	            ,[PackedRate]
	            ,[TTLCTN]
	            ,[FtyCTN]
	            ,[cLogCTN]
	            ,[CFACTN]
	            ,[InspDate]
	            ,[InspResult]
	            ,[CFAName]
	            ,[ActPulloutDate]
	            ,[KPIDeliveryDate]
	            ,[UpdateDeliveryReason]
	            ,[PlanDate]
	            ,[SMR]
	            ,[Handle]
	            ,[Posmr]
	            ,[PoHandle]
	            ,[MCHandle]
	            ,[doxtype]
	            ,[SpecialMark]
	            ,[GlobalFoundationRange]
	            ,[SampleReason]
	            ,[TMS]
                ,[BIFactoryID]
                ,[BIInsertDate]
                ,[BIStatus]
            )
            SELECT
             ISNULL(T.[MDivisionID],'')
            ,ISNULL(T.[FactoryID],'')
            ,ISNULL(T.[SewLine],'')
            ,T.[OrdersBuyerDelivery]
            ,T.[SciDelivery]
            ,T.[SewInLine]
            ,T.[SewOffLine]
            ,ISNULL(T.[val],'')
            ,ISNULL(T.[BrandID],'')
            ,ISNULL(T.[OrderID],'')
            ,ISNULL(T.[POID],'')
            ,ISNULL(T.[Buy Back],'')
            ,ISNULL(T.[Cancelled],'')
            ,ISNULL(T.[Cancelled but Sill],'')
            ,ISNULL(T.[Dest],'')
            ,ISNULL(T.[StyleID],'')
            ,ISNULL(T.[OrderTypeID],'')
            ,ISNULL(T.[ShipModeList],'')
            ,ISNULL(T.[PartialShipping],'')
            ,ISNULL(T.[OrderNo],'')
            ,ISNULL(T.[CustPONo],'')
            ,ISNULL(T.[ProgramID],'')
            ,ISNULL(T.[CdCodeID],'')
            ,ISNULL(T.[CDCodeNew],'')
            ,ISNULL(T.[ProductType],'')
            ,ISNULL(T.[FabricType],'')
            ,ISNULL(T.[Lining],'')
            ,ISNULL(T.[Gender],'')
            ,ISNULL(T.[Construction],'')
            ,T.[KPILETA]
            ,T.[LETA]
            ,T.[MTLETA]
            ,T.[SewETA]
            ,T.[PackETA]
            ,ISNULL(T.[Cpu],0)
            ,ISNULL(T.[TTL CPU],0)
            ,ISNULL(T.[CPU Closed],0)
            ,ISNULL(T.[CPU bal],0)
            ,ISNULL(T.[article_list],'')
            ,ISNULL(T.[Qty],0)
            ,ISNULL(T.[StandardOutput],'')
            ,ISNULL(T.[oriArtwork],'')
            ,ISNULL(T.[AddedArtwork],'')
            ,ISNULL(T.[Artwork],'')
            ,ISNULL(T.[SubProcessDest],'')
            ,T.[EstimatedCutDate]
            ,T.[first_cut_date]
            ,ISNULL(T.[cut_qty],0)
            ,ISNULL(T.[RFID Cut Qty],0)
            ,ISNULL(T.[RFID SewingLine In Qty],0)
            ,ISNULL(T.[RFID Loading Qty],0)
            ,ISNULL(T.[RFID Emb Farm In Qty],0)
            ,ISNULL(T.[RFID Emb Farm Out Qty],0)
            ,ISNULL(T.[RFID Bond Farm In Qty],0)
            ,ISNULL(T.[RFID Bond Farm Out Qty],0)
            ,ISNULL(T.[RFID Print Farm In Qty],0)
            ,ISNULL(T.[RFID Print Farm Out Qty],0)
            ,ISNULL(T.[RFID AT Farm In Qty],0)
            ,ISNULL(T.[RFID AT Farm Out Qty],0)
            ,ISNULL(T.[RFID Pad Print Farm In Qty],0)
            ,ISNULL(T.[RFID Pad Print Farm Out Qty],0)
            ,ISNULL(T.[RFID Emboss Farm In Qty],0)
            ,ISNULL(T.[RFID Emboss Farm Out Qty],0)
            ,ISNULL(T.[RFID HT Farm In Qty],0)
            ,ISNULL(T.[RFID HT Farm Out Qty],0)
            ,ISNULL(T.[RFID AUT Farm In Qty],0)
            ,ISNULL(T.[RFID AUT Farm Out Qty],0)
            ,ISNULL(T.[RFID FM Farm In Qty],0)
            ,ISNULL(T.[RFID FM Farm Out Qty],0)
            ,ISNULL(T.[SubProcessStatus],'')
            ,ISNULL(T.[EMBROIDERY_qty],0)
            ,ISNULL(T.[BONDING_qty],0)
            ,ISNULL(T.[PRINTING_qty],0)
            ,ISNULL(T.[sewing_output],0)
            ,ISNULL(T.[Balance],'')
            ,T.[firstSewingDate]
            ,T.[Last Sewn Date]
            ,ISNULL(T.[AVG_QAQTY],0)
            ,T.[Est_offline]
            ,ISNULL(T.[Scanned_Qty],0)
            ,ISNULL(T.[pack_rate],0)
            ,ISNULL(T.[TotalCTN],0)
            ,ISNULL(T.[FtyCtn],0)
            ,ISNULL(T.[ClogCTN],0)
            ,ISNULL(T.[CFACTN],0)
            ,ISNULL(T.[InspDate],'')
            ,ISNULL(T.[InspResult],'')
            ,ISNULL(T.[CFA Name],'')
            ,T.[ActPulloutDate]
            ,T.[FtyKPI]
            ,ISNULL(T.[KPIChangeReason],'')
            ,T.[PlanDate]
            ,ISNULL(T.[SMR],'')
            ,ISNULL(T.[Handle],'')
            ,ISNULL(T.[PO SMR],'')
            ,ISNULL(T.[PO Handle],'')
            ,ISNULL(T.[MC Handle],'')
            ,ISNULL(T.[DoxType],'')
            ,ISNULL(T.[SpecMark],'')
            ,ISNULL(T.[GFR],0)
            ,ISNULL(T.[SampleReason],'')
            ,ISNULL(T.[TMS],0) 
            ,@BIFactoryID
            ,GetDate()
            , 'New'
            FROM #tmp T
            WHERE NOT EXISTS(SELECT 1 FROM P_WIP P WHERE P.SPNO = T.OrderID)

            {tmp}

            -- 刪除
            DELETE P_WIP 
            FROM P_WIP p 
            where  p.SciDelivery between @StartDate and @EndDate
            and NOT EXISTS(SELECT 1 FROM #tmp t WHERE P.SPNO = T.OrderID)

";

            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
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
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sqlCmd, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

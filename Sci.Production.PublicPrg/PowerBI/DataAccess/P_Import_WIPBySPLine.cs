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
    public class P_Import_WIPBySPLine
    {
        /// <inheritdoc/>
        public Base_ViewModel P_WIPBySPLine(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Planning_R15 planning_R15 = new Planning_R15();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Planning_R15_ViewModel r15_vm = new Planning_R15_ViewModel()
                {
                    StartSciDelivery = item.SDate,
                    EndSciDelivery = item.EDate,
                    Category = "'B'",
                    OrderBy = "orderId",
                    SummaryBy = "3",
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
            Base_ViewModel finalResult;
            string sqlCmd = $@"
            if @IsTrans = 1
            begin
                insert into P_WIPBySPLine_History([Ukey], [BIFactoryID], [BIInsertDate])
                select [Ukey], [BIFactoryID], GETDATE()
                FROM P_WIPBySPLine p 
                where  p.SciDelivery between @StartDate and @EndDate
            end

            -- 刪除
            DELETE P 
            FROM P_WIPBySPLine p 
            where  p.SciDelivery between @StartDate and @EndDate

            -- 新增
            INSERT INTO P_WIPBySPLine
            (
	            [MDivisionID]
	            ,[FactoryID]
	            ,[SewingLineID]
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
	            ,[ActMTLETA_MasterSP]
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
            ,ISNULL(T.[SewingLineID],'')
            ,T.[OrdersBuyerDelivery]
            ,T.[SciDelivery]
            ,T.[InLine]
            ,T.[OffLine]
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

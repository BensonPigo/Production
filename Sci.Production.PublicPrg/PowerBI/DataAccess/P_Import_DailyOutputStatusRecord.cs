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
    public class P_Import_DailyOutputStatusRecord
    {
        /// <inheritdoc/>
        public Base_ViewModel P_DailyOutputStatusRecord(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-30).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.AddDays(30).ToString("yyyy/MM/dd"));
            }

            try
            {
                Planning_P08_ViewModel viewModel = new Planning_P08_ViewModel()
                {
                    SewingSDate = item.SDate,
                    SewingEDate = item.EDate,
                    IsBI = true,
                };

                Base_ViewModel resultReport = new Planning_P08().GetPlanning_P08(viewModel);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;

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

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            string where = @" 
p.SewingOutputDate Between @sDate and @eDate 
AND NOT EXISTS (
    SELECT 1 FROM #tmp t
    WHERE t.SewingLineID = p.SewingLineID
	  AND t.SewingDate   = p.SewingOutputDate
	  AND t.FactoryID    = p.FactoryID
	  AND t.OrderID      = p.SPNo
)";

            string tmp = new Base().SqlBITableHistory("P_SewingDailyOutputStatusRecord", "P_SewingDailyOutputStatusRecord_History", "#tmp", where, false, false);

            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@EDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                };

                // Output 欄位 因 SA 很堅持 NOT NULL, 又要區分 0 與 [沒有加工段 & 尚未開始加工段], Null 轉 -1 到BI
                // UPDATE
                string sql = $@"
UPDATE p
SET MDivisionID          = ISNULL(t.MDivisionID, '')
   ,BrandID              = ISNULL(t.BrandID, '')
   ,StyleID              = ISNULL(t.StyleID, '')
   ,SeasonID             = ISNULL(t.SeasonID, '')
   ,CDCodeNew            = ISNULL(t.CDCodeNew, '')
   ,Article              = ISNULL(t.Article, '')
   ,POID                 = ISNULL(t.POID, '')
   ,Category             = ISNULL(t.Category, '')
   ,SCIDelivery          = t.SCIDelivery
   ,BuyerDelivery        = t.BuyerDelivery
   ,OrderQty             = ISNULL(t.OrderQty, 0)
   ,AlloQty              = ISNULL(t.AlloQty, 0)
   ,Artwork              = ISNULL(t.Artwork, '')
   ,JITDate              = t.JITDate
   ,BCSDate              = t.BCSDate
   ,SewingInLine         = t.Inline
   ,ReadyDate            = t.ReadyDate
   ,SewingOffLine        = t.Offline
   ,StardardOutputPerDay = ISNULL(t.StdQty, 0)
   ,WorkHourPerDay       = ISNULL(t.WorkHourPerDay, 0)
   ,CuttingOutput        = t.CuttingOutput
   ,CuttingRemark        = ISNULL(t.CuttingRemark, '')
   ,Consumption          = ISNULL(t.Consumption, 0)
   ,ActConsOutput        = ISNULL(t.ActConsOutput, 0)
   ,LoadingOutput        = t.LoadingOutput
   ,LoadingRemark        = ISNULL(t.LoadingRemark, '')
   ,LoadingExclusion     = ISNULL(t.LoadingExclusion, 0)
   ,ATOutput             = ISNULL(t.ATOutput, -1)
   ,ATRemark             = ISNULL(t.ATRemark, '')
   ,ATExclusion          = ISNULL(t.ATExclusion, 0)
   ,AUTOutput            = ISNULL(t.AUTOutput, -1)
   ,AUTRemark            = ISNULL(t.AUTRemark, '')
   ,AUTExclusion         = ISNULL(t.AUTExclusion, 0)
   ,HTOutput             = ISNULL(t.HTOutput, -1)
   ,HTRemark             = ISNULL(t.HTRemark, '')
   ,HTExclusion          = ISNULL(t.HTExclusion, 0)
   ,BOOutput             = ISNULL(t.BOOutput, -1)
   ,BORemark             = ISNULL(t.BORemark, '')
   ,BOExclusion          = ISNULL(t.BOExclusion, 0)
   ,FMOutput             = ISNULL(t.FMOutput, -1)
   ,FMRemark             = ISNULL(t.FMRemark, '')
   ,FMExclusion          = ISNULL(t.FMExclusion, 0)
   ,PRTOutput            = ISNULL(t.PRTOutput, -1)
   ,PRTRemark            = ISNULL(t.PRTRemark, '')
   ,PRTExclusion         = ISNULL(t.PRTExclusion, 0)
   ,PADPRTOutput         = ISNULL(t.PADPRTOutput, -1)
   ,PADPRTRemark         = ISNULL(t.PADPRTRemark, '')
   ,PADPRTExclusion      = ISNULL(t.PADPRTExclusion, 0)
   ,EMBOutput            = ISNULL(t.EMBOutput, -1)
   ,EMBRemark            = ISNULL(t.EMBRemark, '')
   ,EMBExclusion         = ISNULL(t.EMBExclusion, 0)
   ,FIOutput             = ISNULL(t.FIOutput, -1)
   ,FIRemark             = ISNULL(t.FIRemark, '')
   ,FIExclusion          = ISNULL(t.FIExclusion, 0)
   ,[BIFactoryID]        = @BIFactoryID
   ,[BIInsertDate]       = GETDATE()
FROM P_SewingDailyOutputStatusRecord p
INNER JOIN #tmp t ON t.SewingLineID = p.SewingLineID
                 AND t.SewingDate   = p.SewingOutputDate
                 AND t.FactoryID    = p.FactoryID
                 AND t.OrderID      = p.SPNo
";

                // DELETE
                sql += $@"
{tmp}

Delete p 
FROM P_SewingDailyOutputStatusRecord p
Where p.SewingOutputDate Between @sDate and @eDate
AND NOT EXISTS (
    SELECT 1 FROM #tmp t
    WHERE t.SewingLineID = p.SewingLineID
	  AND t.SewingDate   = p.SewingOutputDate
	  AND t.FactoryID    = p.FactoryID
	  AND t.OrderID      = p.SPNo
)
";

                // INSERT
                sql += $@"
INSERT INTO [dbo].[P_SewingDailyOutputStatusRecord]
            ([SewingLineID]
            ,[SewingOutputDate]
            ,[FactoryID]
            ,[SPNo]
            ,[MDivisionID]
            ,[BrandID]
            ,[StyleID]
            ,[SeasonID]
            ,[CDCodeNew]
            ,[Article]
            ,[POID]
            ,[Category]
            ,[SCIDelivery]
            ,[BuyerDelivery]
            ,[OrderQty]
            ,[AlloQty]
            ,[Artwork]
            ,[JITDate]
            ,[BCSDate]
            ,[SewingInLine]
            ,[ReadyDate]
            ,[SewingOffLine]
            ,[StardardOutputPerDay]
            ,[WorkHourPerDay]
            ,[CuttingOutput]
            ,[CuttingRemark]
            ,[Consumption]
            ,[ActConsOutput]
            ,[LoadingOutput]
            ,[LoadingRemark]
            ,[LoadingExclusion]
            ,[ATOutput]
            ,[ATRemark]
            ,[ATExclusion]
            ,[AUTOutput]
            ,[AUTRemark]
            ,[AUTExclusion]
            ,[HTOutput]
            ,[HTRemark]
            ,[HTExclusion]
            ,[BOOutput]
            ,[BORemark]
            ,[BOExclusion]
            ,[FMOutput]
            ,[FMRemark]
            ,[FMExclusion]
            ,[PRTOutput]
            ,[PRTRemark]
            ,[PRTExclusion]
            ,[PADPRTOutput]
            ,[PADPRTRemark]
            ,[PADPRTExclusion]
            ,[EMBOutput]
            ,[EMBRemark]
            ,[EMBExclusion]
            ,[FIOutput]
            ,[FIRemark]
            ,[FIExclusion]
            ,[BIFactoryID]
            ,[BIInsertDate]
)
SELECT
	 [SewingLineID]
    ,[SewingDate]
    ,[FactoryID]
    ,[OrderID]
    ,ISNULL([MDivisionID], '')
    ,ISNULL([BrandID], '')
    ,ISNULL([StyleID], '')
    ,ISNULL([SeasonID], '')
    ,ISNULL([CDCodeNew], '')
    ,ISNULL([Article], '')
    ,ISNULL([POID], '')
    ,ISNULL([Category], '')
    ,[SCIDelivery]
    ,[BuyerDelivery]
    ,ISNULL([OrderQty], 0)
    ,ISNULL([AlloQty], 0)
    ,ISNULL([Artwork], '')
    ,[JITDate]
    ,[BCSDate]
    ,[Inline]
    ,[ReadyDate]
    ,[Offline]
    ,ISNULL([StdQty], 0)
    ,ISNULL([WorkHourPerDay], 0)
    ,[CuttingOutput]
    ,ISNULL([CuttingRemark], '')
    ,ISNULL([Consumption], 0)
    ,ISNULL([ActConsOutput], 0)
    ,[LoadingOutput]
    ,ISNULL([LoadingRemark], '')
    ,ISNULL([LoadingExclusion], 0)
    ,ISNULL([ATOutput], -1)
    ,ISNULL([ATRemark], '')
    ,ISNULL([ATExclusion], 0)
    ,ISNULL([AUTOutput], -1)
    ,ISNULL([AUTRemark], '')
    ,ISNULL([AUTExclusion], 0)
    ,ISNULL([HTOutput], -1)
    ,ISNULL([HTRemark], '')
    ,ISNULL([HTExclusion], 0)
    ,ISNULL([BOOutput], -1)
    ,ISNULL([BORemark], '')
    ,ISNULL([BOExclusion], 0)
    ,ISNULL([FMOutput], -1)
    ,ISNULL([FMRemark], '')
    ,ISNULL([FMExclusion], 0)
    ,ISNULL([PRTOutput], -1)
    ,ISNULL([PRTRemark], '')
    ,ISNULL([PRTExclusion], 0)
    ,ISNULL([PADPRTOutput], -1)
    ,ISNULL([PADPRTRemark], '')
    ,ISNULL([PADPRTExclusion], 0)
    ,ISNULL([EMBOutput], -1)
    ,ISNULL([EMBRemark], '')
    ,ISNULL([EMBExclusion], 0)
    ,ISNULL([FIOutput], -1)
    ,ISNULL([FIRemark], '')
    ,ISNULL([FIExclusion], 0)
    , @BIFactoryID
    , GETDATE()
FROM #tmp t
WHERE NOT EXISTS(
	SELECT 1
	FROM P_SewingDailyOutputStatusRecord p
    WHERE t.SewingLineID = p.SewingLineID
	  AND t.SewingDate   = p.SewingOutputDate
	  AND t.FactoryID    = p.FactoryID
	  AND t.OrderID      = p.SPNo
)
";

                // 執行 SQL 並回傳 Base_ViewModel
                return new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable _, conn: sqlConn, paramters: sqlParameters),
                };
            }
        }
    }
}

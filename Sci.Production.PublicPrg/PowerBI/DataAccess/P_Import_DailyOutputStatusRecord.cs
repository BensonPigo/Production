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
        public Base_ViewModel P_DailyOutputStatusRecord(DateTime? sDate, DateTime? eDate, string biTableInfoID)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-30).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.AddDays(30).ToString("yyyy/MM/dd"));
            }

            try
            {
                Planning_P08_ViewModel viewModel = new Planning_P08_ViewModel()
                {
                    SewingSDate = sDate,
                    SewingEDate = eDate,
                    IsBI = true,
                };

                Base_ViewModel resultReport = new Planning_P08().GetPlanning_P08(viewModel);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, sDate.Value, eDate.Value, biTableInfoID);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate, DateTime eDate, string biTableInfoID)
        {
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
                    new SqlParameter("@EDate", eDate),
                };

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
   ,CuttingOutput        = ISNULL(t.CuttingOutput, 0)
   ,CuttingRemark        = ISNULL(t.CuttingRemark, '')
   ,Consumption          = ISNULL(t.Consumption, 0)
   ,ActConsOutput        = ISNULL(t.ActConsOutput, 0)
   ,LoadingOutput        = ISNULL(t.LoadingOutput, 0)
   ,LoadingRemark        = ISNULL(t.LoadingRemark, '')
   ,LoadingExclusion     = ISNULL(t.LoadingExclusion, 0)
   ,ATOutput             = ISNULL(t.ATOutput, 0)
   ,ATRemark             = ISNULL(t.ATRemark, '')
   ,ATExclusion          = ISNULL(t.ATExclusion, 0)
   ,AUTOutput            = ISNULL(t.AUTOutput, 0)
   ,AUTRemark            = ISNULL(t.AUTRemark, '')
   ,AUTExclusion         = ISNULL(t.AUTExclusion, 0)
   ,HTOutput             = ISNULL(t.HTOutput, 0)
   ,HTRemark             = ISNULL(t.HTRemark, '')
   ,HTExclusion          = ISNULL(t.HTExclusion, 0)
   ,BOOutput             = ISNULL(t.BOOutput, 0)
   ,BORemark             = ISNULL(t.BORemark, '')
   ,BOExclusion          = ISNULL(t.BOExclusion, 0)
   ,FMOutput             = ISNULL(t.FMOutput, 0)
   ,FMRemark             = ISNULL(t.FMRemark, '')
   ,FMExclusion          = ISNULL(t.FMExclusion, 0)
   ,PRTOutput            = ISNULL(t.PRTOutput, 0)
   ,PRTRemark            = ISNULL(t.PRTRemark, '')
   ,PRTExclusion         = ISNULL(t.PRTExclusion, 0)
FROM P_SewingDailyOutputStatusRecord p
INNER JOIN #tmp t ON t.SewingLineID = p.SewingLineID
                 AND t.SewingDate   = p.SewingOutputDate
                 AND t.FactoryID    = p.FactoryID
                 AND t.OrderID      = p.SPNo
";

                // DELETE
                sql += $@"
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
            ,[PRTExclusion])
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
    ,ISNULL([CuttingOutput], 0)
    ,ISNULL([CuttingRemark], '')
    ,ISNULL([Consumption], 0)
    ,ISNULL([ActConsOutput], 0)
    ,ISNULL([LoadingOutput], 0)
    ,ISNULL([LoadingRemark], '')
    ,ISNULL([LoadingExclusion], 0)
    ,ISNULL([ATOutput], 0)
    ,ISNULL([ATRemark], '')
    ,ISNULL([ATExclusion], 0)
    ,ISNULL([AUTOutput], 0)
    ,ISNULL([AUTRemark], '')
    ,ISNULL([AUTExclusion], 0)
    ,ISNULL([HTOutput], 0)
    ,ISNULL([HTRemark], '')
    ,ISNULL([HTExclusion], 0)
    ,ISNULL([BOOutput], 0)
    ,ISNULL([BORemark], '')
    ,ISNULL([BOExclusion], 0)
    ,ISNULL([FMOutput], 0)
    ,ISNULL([FMRemark], '')
    ,ISNULL([FMExclusion], 0)
    ,ISNULL([PRTOutput], 0)
    ,ISNULL([PRTRemark], '')
    ,ISNULL([PRTExclusion], 0)
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

                // 加上 BITableInfo 更新字串
                sql += new Base().SqlBITableInfo(biTableInfoID, false);

                // 執行 SQL 並回傳 Base_ViewModel
                return new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable _, conn: sqlConn, paramters: sqlParameters),
                };
            }
        }
    }
}

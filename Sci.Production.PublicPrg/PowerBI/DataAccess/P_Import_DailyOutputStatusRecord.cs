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
            PPIC_R01 biModel = new PPIC_R01();
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
                // TO DO
                //var viewModel = new ()
                //{
                //    SewingDate1 = sDate,
                //    SewingDate2 = eDate,
                //};

                //Base_ViewModel resultReport = biModel.GetSewingDailyOutputStatusRecord(viewModel);
                Base_ViewModel resultReport = new Base_ViewModel();
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.DtArr[0];

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
SET [SewingLineID]         = t.[SewingLineID]
   ,[SewingOutputDate]	   = t.[SewingOutputDate]
   ,[MDivisionID]		   = t.[MDivisionID]
   ,[FactoryID]			   = t.[FactoryID]
   ,[BrandID]			   = t.[BrandID]
   ,[StyleID]			   = t.[StyleID]
   ,[SPNo]				   = t.[SPNo]
   ,[SeasonID]			   = t.[SeasonID]
   ,[CDCodeNew]			   = t.[CDCodeNew]
   ,[Article]			   = t.[Article]
   ,[POID]				   = t.[POID]
   ,[Category]			   = t.[Category]
   ,[SCIDelivery]		   = t.[SCIDelivery]
   ,[BuyerDelivery]		   = t.[BuyerDelivery]
   ,[OrderQty]			   = t.[OrderQty]
   ,[AlloQty]			   = t.[AlloQty]
   ,[Artwork]			   = t.[Artwork]
   ,[JITDate]			   = t.[JITDate]
   ,[BCSDate]			   = t.[BCSDate]
   ,[SewingInLine]		   = t.[SewingInLine]
   ,[ReadyDate]			   = t.[ReadyDate]
   ,[SewingOffLine]		   = t.[SewingOffLine]
   ,[StardardOutputPerDay] = t.[StardardOutputPerDay]
   ,[WorkHourPerDay]	   = t.[WorkHourPerDay]
   ,[CuttingOutput]		   = t.[CuttingOutput]
   ,[CuttingRemark]		   = t.[CuttingRemark]
   ,[Consumption]		   = t.[Consumption]
   ,[ActConsOutput]		   = t.[ActConsOutput]
   ,[LoadingOutput]		   = t.[LoadingOutput]
   ,[LoadingRemark]		   = t.[LoadingRemark]
   ,[LoadingExclusion]	   = t.[LoadingExclusion]
   ,[ATOutput]			   = t.[ATOutput]
   ,[ATRemark]			   = t.[ATRemark]
   ,[ATExclusion]		   = t.[ATExclusion]
   ,[AUTOutput]			   = t.[AUTOutput]
   ,[AUTRemark]			   = t.[AUTRemark]
   ,[AUTExclusion]		   = t.[AUTExclusion]
   ,[HTOutput]			   = t.[HTOutput]
   ,[HTRemark]			   = t.[HTRemark]
   ,[HTExclusion]		   = t.[HTExclusion]
   ,[BOOutput]			   = t.[BOOutput]
   ,[BORemark]			   = t.[BORemark]
   ,[BOExclusion]		   = t.[BOExclusion]
   ,[FMOutput]			   = t.[FMOutput]
   ,[FMRemark]			   = t.[FMRemark]
   ,[FMExclusion]		   = t.[FMExclusion]
   ,[PRTOutput]			   = t.[PRTOutput]
   ,[PRTRemark]			   = t.[PRTRemark]
   ,[PRTExclusion]		   = t.[PRTExclusion]
FROM P_SewingDailyOutputStatusRecord p
INNER JOIN #tmp t ON t.SewingLineID     = p.SewingLineID
				 AND t.SewingOutputDate = p.SewingOutputDate
				 AND t.MDivisionID	    = p.MDivisionID
				 AND t.FactoryID	    = p.FactoryID
";

                // DELETE
                sql += $@"
Delete p 
FROM P_SewingDailyOutputStatusRecord p
Where p.SewingOutputDate Between @sDate and @eDate
AND NOT EXISTS (
	SELECT 1 FROM #tmp t
	WHERE t.SewingLineID   = p.SewingLineID
	AND t.SewingOutputDate = p.SewingOutputDate
	AND t.MDivisionID      = p.MDivisionID
	AND t.FactoryID        = p.FactoryID
)
";

                // INSERT
                sql += $@"
INSERT INTO [dbo].[P_SewingDailyOutputStatusRecord]
           ([SewingLineID]
           ,[SewingOutputDate]
           ,[MDivisionID]
           ,[FactoryID]
           ,[BrandID]
           ,[StyleID]
           ,[SPNo]
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
    ,[SewingOutputDate]
    ,[MDivisionID]
    ,[FactoryID]
    ,[BrandID]
    ,[StyleID]
    ,[SPNo]
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
FROM #tmp t
WHERE NOT EXISTS(
	SELECT 1
	FROM P_SewingDailyOutputStatusRecord p
	WHERE t.SewingLineID   = p.SewingLineID
	AND t.SewingOutputDate = p.SewingOutputDate
	AND t.MDivisionID      = p.MDivisionID
	AND t.FactoryID        = p.FactoryID
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

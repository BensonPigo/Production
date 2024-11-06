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
    public class P_Import_DailyAccuCPULoading
    {
        /// <inheritdoc/>
        public Base_ViewModel P_DailyAccuCPULoading(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
                if (!sDate.HasValue)
                {
                    sDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
                }

                if (!eDate.HasValue)
                {
                    eDate = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy/MM/dd"));
                }
                else
                {
                    eDate = eDate.Value.AddDays(1);
                }

                Base_ViewModel resultReport = this.GetDailyAccuCPULoading(sDate, eDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt, sDate, eDate);
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

        private Base_ViewModel GetDailyAccuCPULoading(DateTime? sDate, DateTime? eDate)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", sDate),
                new SqlParameter("@eDate", eDate),
            };
            string sql = @"
            --DECLARE @sDate datetime = '2024-10-07'
            --DECLARE @eDate datetime = '2024-10-15'
            SELECT
             [Year] = ISNULL(DA.[Year], '')
            ,[Month] = ISNULL(DA.[Month], '')
            ,[FactoryID] = ISNULL(DA.[FactoryID], '')
            ,[TTLCPULoaded] = ISNULL(DA.[TTLCPULoaded], 0)
            ,[UnfinishedLastMonth] = ISNULL(DA.[UnfinishedLastMonth], 0)
            ,[FinishedLastMonth] = ISNULL(DA.[FinishedLastMonth], 0)
            ,[CanceledStillNeedProd] = ISNULL(DA.[CanceledStillNeedProd], 0)
            ,[SubconToSisFactory] = ISNULL(DA.SubconToSisFactory, 0)
            ,[SubconFromSisterFactory] = ISNULL(DA.SubconFromSisterFactory, 0)
            ,[PullForwardFromNextMonths] = ISNULL(DA.PullForwardFromNextMonths, 0)
            ,[LoadingDelayFromThisMonth] = ISNULL(DA.LoadingDelayFromThisMonth, 0)
            ,[LocalSubconInCPU] = ISNULL(DA.LocalSubconInCPU, 0)
            ,[LocalSubconOutCPU] = ISNULL(DA.LocalSubconOutCPU, 0)
            ,[RemainCPUThisMonth] = ISNULL(DA.[RemainCPUThisMonth], 0)
            ,[AddName] = ISNULL(DA.[AddName], '')
            ,[AddDate] = DA.[AddDate]
            ,[EditName] = ISNULL(DA.[EditName], '')
            ,[EditDate] = DA.[EditDate]
            ,[Date] = ISNULL(DAD.[Date], '')
            ,[WeekDay] = ISNULL(DAD.[WeekDay], '')
            ,[DailyCPULoading] = ISNULL(DAD.[DailyCPULoading], 0)
            ,[NewTarget] = ISNULL(DAD.[NewTarget], 0)
            ,[ActCPUPerformed] = ISNULL(DAD.[ActCPUPerformed], 0)
            ,[DailyCPUVarience] = ISNULL(DAD.[DailyCPUVarience], 0)
            ,[AccuLoading] = ISNULL(DAD.[AccuLoading], 0)
            ,[AccuActCPUPerformed] = ISNULL(DAD.[AccuActCPUPerformed], 0)
            ,[AccuCPUVariance] = ISNULL(DAD.[AccuCPUVariance], 0)
            ,[LeftWorkDays] = ISNULL(DAD.[LeftWorkDays], 0)
            ,[AvgWorkhours] = ISNULL(DAD.[AvgWorkhours], 0)
            ,[PPH] = ISNULL(DAD.[PPH], 0)
            ,[Direct] = ISNULL(DAD.[Direct], 0)
            ,[Active] = ISNULL(DAD.[Active], 0)
            ,[VPH] = ISNULL(DAD.[VPH], 0)
            ,[ManpowerRatio] = ISNULL(DAD.[ManpowerRatio], 0)
            ,[LineNo] = ISNULL(DAD.[LineNo], 0)
            ,[LineManpower] = ISNULL(DAD.[LineManpower], 0)
            ,[GPH] = ISNULL(DAD.[GPH], 0)
            ,[SPH] = ISNULL(DAD.[SPH], 0)
            FROM MainServer.Production.dbo.DailyAccuCPULoading DA WITH(NOLOCK)
            INNER JOIN MainServer.Production.dbo.DailyAccuCPULoading_Detail DAD WITH(NOLOCK) ON DAD.DailyAccuCPULoadingUkey = DA.UKEY
            WHERE
            ((DA.AddDate >= @sDate and DA.AddDate < @eDate) or(DA.EditDate >= @sDate and DA.EditDate < @eDate))";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sql, sqlParameters, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", sDate),
                new SqlParameter("@eDate", eDate),
            };
            using (sqlConn)
            {
                string sql = @"
                UPDATE PDA
                SET
                 PDA.[TTLCPULoaded]						= T.[TTLCPULoaded]				
                ,PDA.[UnfinishedLastMonth]				= T.[UnfinishedLastMonth]		
                ,PDA.[FinishedLastMonth]				= T.[FinishedLastMonth]			
                ,PDA.[CanceledStillNeedProd]			= T.[CanceledStillNeedProd]		
                ,PDA.[SubconToSisFactory]				= T.[SubconToSisFactory]			
                ,PDA.[SubconFromSisterFactory]			= T.[SubconFromSisterFactory]	
                ,PDA.[PullForwardFromNextMonths]		= T.[PullForwardFromNextMonths]	
                ,PDA.[LoadingDelayFromThisMonth]		= T.[LoadingDelayFromThisMonth]	
                ,PDA.[LocalSubconInCPU]					= T.[LocalSubconInCPU]			
                ,PDA.[LocalSubconOutCPU]				= T.[LocalSubconOutCPU]			
                ,PDA.[RemainCPUThisMonth]				= T.[RemainCPUThisMonth]							
                ,PDA.[WeekDay]							= T.[WeekDay]					
                ,PDA.[DailyCPULoading]					= T.[DailyCPULoading]			
                ,PDA.[NewTarget]						= T.[NewTarget]					
                ,PDA.[ActCPUPerformed]					= T.[ActCPUPerformed]			
                ,PDA.[DailyCPUVarience]					= T.[DailyCPUVarience]			
                ,PDA.[AccuLoading]						= T.[AccuLoading]				
                ,PDA.[AccuActCPUPerformed]				= T.[AccuActCPUPerformed]		
                ,PDA.[AccuCPUVariance]					= T.[AccuCPUVariance]			
                ,PDA.[LeftWorkDays]						= T.[LeftWorkDays]				
                ,PDA.[AvgWorkhours]						= T.[AvgWorkhours]				
                ,PDA.[PPH]								= T.[PPH]						
                ,PDA.[Direct]							= T.[Direct]						
                ,PDA.[Active]							= T.[Active]						
                ,PDA.[VPH]								= T.[VPH]						
                ,PDA.[ManpowerRatio]					= T.[ManpowerRatio]				
                ,PDA.[LineNo]							= T.[LineNo]						
                ,PDA.[LineManpower]						= T.[LineManpower]				
                ,PDA.[GPH]								= T.[GPH]						
                ,PDA.[SPH]								= T.[SPH]						
                FROM P_DailyAccuCPULoading PDA
                INNER JOIN #TMP T ON PDA.[Year] = T.[Year] AND PDA.[Month] = T.[Month] AND PDA.[FactoryID] = T.[FactoryID] AND PDA.[Date] = t.[Date]

                INSERT INTO [dbo].[P_DailyAccuCPULoading]
                ([Year]
                ,[Month]
                ,[FactoryID]
                ,[TTLCPULoaded]
                ,[UnfinishedLastMonth]
                ,[FinishedLastMonth]
                ,[CanceledStillNeedProd]
                ,[SubconToSisFactory]
                ,[SubconFromSisterFactory]
                ,[PullForwardFromNextMonths]
                ,[LoadingDelayFromThisMonth]
                ,[LocalSubconInCPU]
                ,[LocalSubconOutCPU]
                ,[RemainCPUThisMonth]
                ,[AddName]
                ,[AddDate]
                ,[EditName]
                ,[EditDate]
                ,[Date]
                ,[WeekDay]
                ,[DailyCPULoading]
                ,[NewTarget]
                ,[ActCPUPerformed]
                ,[DailyCPUVarience]
                ,[AccuLoading]
                ,[AccuActCPUPerformed]
                ,[AccuCPUVariance]
                ,[LeftWorkDays]
                ,[AvgWorkhours]
                ,[PPH]
                ,[Direct]
                ,[Active]
                ,[VPH]
                ,[ManpowerRatio]
                ,[LineNo]
                ,[LineManpower]
                ,[GPH]
                ,[SPH])
                SELECT
                [Year]
                ,[Month]
                ,[FactoryID]
                ,[TTLCPULoaded]
                ,[UnfinishedLastMonth]
                ,[FinishedLastMonth]
                ,[CanceledStillNeedProd]
                ,[SubconToSisFactory]
                ,[SubconFromSisterFactory]
                ,[PullForwardFromNextMonths]
                ,[LoadingDelayFromThisMonth]
                ,[LocalSubconInCPU]
                ,[LocalSubconOutCPU]
                ,[RemainCPUThisMonth]
                ,[AddName]
                ,[AddDate]
                ,[EditName]
                ,[EditDate]
                ,[Date]
                ,[WeekDay]
                ,[DailyCPULoading]
                ,[NewTarget]
                ,[ActCPUPerformed]
                ,[DailyCPUVarience]
                ,[AccuLoading]
                ,[AccuActCPUPerformed]
                ,[AccuCPUVariance]
                ,[LeftWorkDays]
                ,[AvgWorkhours]
                ,[PPH]
                ,[Direct]
                ,[Active]
                ,[VPH]
                ,[ManpowerRatio]
                ,[LineNo]
                ,[LineManpower]
                ,[GPH]
                ,[SPH]
                FROM #TMP T
                where not exists (
                    select 1 from P_DailyAccuCPULoading PDA 
                    where PDA.FactoryID = T.FactoryID
	                and PDA.[Month] = T.[Month]
	                and PDA.[Year] = T.[Year]
                    AND PDA.[Date] = t.[Date]
                )

                delete PDA 
                from dbo.P_DailyAccuCPULoading PDA
                where not exists (
                    select 1 from #tmp T 
                    where PDA.FactoryID = T.FactoryID
	                and PDA.[Month] = T.[Month]
	                and PDA.[Year] = T.[Year]
                    AND PDA.[Date] = t.[Date]
                )
                and 
                (
	                (PDA.AddDate between @sDate and @eDate)
	                or
	                (PDA.EditDate between @sDate and @eDate)
                )


                if exists (select 1 from BITableInfo b where b.id = 'P_DailyAccuCPULoading')
	            begin
		            update b
			            set b.TransferDate = getdate()
		            from BITableInfo b
		            where b.id = 'P_DailyAccuCPULoading'
	            end
	            else 
	            begin
		            insert into BITableInfo(Id, TransferDate)
		            values('P_DailyAccuCPULoading', getdate())
	            end

                DROP TABLE #TMP
                ";
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

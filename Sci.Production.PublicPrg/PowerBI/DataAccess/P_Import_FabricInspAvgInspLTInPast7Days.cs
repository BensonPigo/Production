using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_FabricInspAvgInspLTInPast7Days
    {
        /// <inheritdoc/>
        public Base_ViewModel P_FabricInspAvgInspLTInPast7Days(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                // insert into PowerBI
                finalResult = this.UpdateBIData(item);
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
        private Base_ViewModel UpdateBIData(ExecutedList item)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string where = @" NOT EXISTS (SELECT 1 FROM #TMP T WHERE T.TransferDate = P.TransferDate AND P.FactoryID = T.FactoryID)
            AND p.TransferDate NOT BETWEEN  @StartDate AND @EndDate ";

            string tmp = new Base().SqlBITableHistory("P_FabricInspAvgInspLTInPast7Days", "P_FabricInspAvgInspLTInPast7Days_History", "#tmp", where);

            string sqlcmd = $@"          
            DECLARE @Number INT = -7;
            Create table #tmp
            (
	            TransferDate VARCHAR(10),
                FactoryID VARCHAR(10),
                AvgInspLTInPast7Days FLOAT,
                BIFactoryID VARCHAR(8),
                BIInsertDate date             
            )

            WHILE @Number <= 0
            BEGIN
            INSERT INTO #tmp (TransferDate, FactoryID, AvgInspLTInPast7Days, BIFactoryID, BIInsertDate)
              SELECT
	            [TransferDate] = A.TransferDate
	            ,[FactoryID] = A.FactoryID
	            ,[AvgInspLTInPast7Days] = IIF(SUM(A.SumofDuration) = 0, 0, ROUND(CAST(SUM(A.SumofDuration) as FLOAT) / A.DataCount, 2))
                ,[BIFactoryID] = @BIFactoryID
                ,[BIInsertDate] = GETDATE()
              FROM (
                SELECT 
                  [TransferDate] = FORMAT(DATEADD(day, @Number, @EndDate), 'yyyy/MM/dd')
                  ,[FactoryID] = F.FTYGroup
                  ,[SumofDuration] = DATEDIFF(day, FORMAT(P.ArriveWHDate, 'yyyy/MM/dd'), FORMAT(P.PhysicalInspDate, 'yyyy/MM/dd'))
                  ,[DataCount] = op_Count.val
                FROM P_FabricInspLabSummaryReport P
                INNER JOIN MainServer.Production.dbo.Factory F ON P.FactoryID = F.ID AND F.IsProduceFty = 1 AND F.Junk = 0
                OUTER APPLY (
                  SELECT val = COUNT(1) 
                  FROM P_FabricInspLabSummaryReport op
                  INNER JOIN MainServer.Production.dbo.Factory opF ON op.FactoryID = opF.ID AND opF.IsProduceFty = 1 AND opF.Junk = 0
                  WHERE 
                    op.PhysicalInspDate >= FORMAT(DATEADD(day, @Number, @StartDate), 'yyyy/MM/dd') AND op.PhysicalInspDate < FORMAT(DATEADD(day, @Number, @EndDate), 'yyyy/MM/dd')
                    AND opF.FTYGroup = F.FTYGroup
                    AND op.PhysicalInspDate IS NOT NULL 
                    AND op.ArriveWHDate IS NOT NULL
                    AND op.Category = 'Bulk'
                    AND op.NAPhysical <> 'Y'
                ) op_Count
                WHERE 
                  PhysicalInspDate >= FORMAT(DATEADD(day, @Number, @StartDate), 'yyyy/MM/dd') AND PhysicalInspDate < FORMAT(DATEADD(day, @Number, @EndDate), 'yyyy/MM/dd')
                  AND P.PhysicalInspDate IS NOT NULL 
                  AND P.ArriveWHDate IS NOT NULL
                  AND P.Category = 'Bulk'
                  AND P.NAPhysical <> 'Y'
              ) A
              GROUP BY A.TransferDate, A.FactoryID, A.DataCount

              SET @Number = @Number + 1;
            END;


            ----更新
            UPDATE P SET
             P.[AvgInspLTInPast7Days] = ISNULL(T.[AvgInspLTInPast7Days],0),
             P.[BIFactoryID] = T.[BIFactoryID],
             P.[BIInsertDate] = T.[BIInsertDate]
            FROM P_FabricInspAvgInspLTInPast7Days P
            INNER JOIN #TMP T ON P.[TransferDate] = T.[TransferDate] AND P.[FactoryID] = T.[FactoryID]
            
            -----新增
            INSERT INTO [dbo].[P_FabricInspAvgInspLTInPast7Days]
            (
	            [TransferDate]
	            ,[FactoryID]
	            ,[AvgInspLTInPast7Days]
                ,[BIFactoryID]
                ,[BIInsertDate] 
            )
            SELECT
             [TransferDate]
            ,[FactoryID] = ISNULL(T.[FactoryID],'')
            ,[AvgInspLTInPast7Days] = ISNULL(T.[AvgInspLTInPast7Days],0)
            ,T.[BIFactoryID]
            , GETDATE()
            from #tmp T
            Where NOT EXISTS(SELECT 1 FROM P_FabricInspAvgInspLTInPast7Days P WHERE P.[TransferDate] = T.[TransferDate] AND P.[FactoryID] = T.[FactoryID])   

            {tmp}

            ----- 刪除
            DELETE P
            FROM P_FabricInspAvgInspLTInPast7Days P
            WHERE NOT EXISTS (SELECT 1 FROM #TMP T WHERE T.TransferDate = P.TransferDate AND P.FactoryID = T.FactoryID)
            AND p.TransferDate NOT BETWEEN  @StartDate AND @EndDate 


            Drop Table #tmp
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
                    Result = TransactionClass.ExecuteByConnTransactionScope(conn: sqlConn, cmdtext: sqlcmd, parameters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

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
        public Base_ViewModel P_FabricInspAvgInspLTInPast7Days(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                // insert into PowerBI
                finalResult = this.UpdateBIData(sDate.Value, eDate.Value);
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

        /// <inheritdoc/>
        private Base_ViewModel UpdateBIData(DateTime sDate, DateTime eDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"          
            DECLARE @Number INT = -7;
            Create table #tmp
            (
	            TransferDate VARCHAR(10),
                FactoryID VARCHAR(10),
                AvgInspLTInPast7Days FLOAT
            )

            WHILE @Number <= 0
            BEGIN
            INSERT INTO #tmp (TransferDate, FactoryID, AvgInspLTInPast7Days)
              SELECT
	            [TransferDate] = A.TransferDate
	            ,[FactoryID] = A.FactoryID
	            ,[AvgInspLTInPast7Days] = IIF(SUM(A.SumofDuration) = 0, 0, ROUND(CAST(SUM(A.SumofDuration) as FLOAT) / A.DataCount, 2))
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
             P.[AvgInspLTInPast7Days] = ISNULL(T.[AvgInspLTInPast7Days],0)
            FROM P_FabricInspAvgInspLTInPast7Days P
            INNER JOIN #TMP T ON P.[TransferDate] = T.[TransferDate] AND P.[FactoryID] = T.[FactoryID]
            
            -----新增
            INSERT INTO [dbo].[P_FabricInspAvgInspLTInPast7Days]
            (
	            [TransferDate]
	            ,[FactoryID]
	            ,[AvgInspLTInPast7Days]
            )
            SELECT
             [TransferDate]
            ,[FactoryID] = ISNULL(T.[FactoryID],'')
            ,[AvgInspLTInPast7Days] = ISNULL(T.[AvgInspLTInPast7Days],0)
            from #tmp T
            Where NOT EXISTS(SELECT 1 FROM P_FabricInspAvgInspLTInPast7Days P WHERE P.[TransferDate] = T.[TransferDate] AND P.[FactoryID] = T.[FactoryID])   

            ----- 刪除
            DELETE P
            FROM P_FabricInspAvgInspLTInPast7Days P
            WHERE 
            NOT EXISTS (SELECT 1 FROM #TMP T WHERE T.TransferDate = P.TransferDate AND P.FactoryID = T.FactoryID)
            AND TransferDate NOT BETWEEN  @StartDate AND @EndDate 

            IF EXISTS (SELECT 1 FROM BITableInfo B WHERE B.ID = 'P_FabricInspAvgInspLTInPast7Days')
            BEGIN
	            UPDATE B
	            SET b.TransferDate = getdate()
	            FROM BITableInfo B
	            WHERE B.ID = 'P_FabricInspAvgInspLTInPast7Days'
            END
            ELSE 
            BEGIN
	            INSERT INTO BITableInfo(Id, TransferDate)
	            VALUES('P_FabricInspAvgInspLTInPast7Days', GETDATE())
            END
            Drop Table #tmp
            ";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", sDate),
                    new SqlParameter("@EndDate", eDate),

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

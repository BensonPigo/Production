using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SubProInsReportMonthlyRate
    {
        /// <inheritdoc/>
        public Base_ViewModel P_SubProInsReportMonthlyRate(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DateTime now = DateTime.Now;

            if (!item.SDate.HasValue)
            {
                item.SDate = MyUtility.Convert.GetDate(new DateTime(now.Year, now.Month, 1).AddMonths(-1).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = MyUtility.Convert.GetDate(new DateTime(now.Year, now.Month, 1).AddDays(-1).ToString("yyyy/MM/dd"));
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

        private Base_ViewModel UpdateBIData(ExecutedList item)
        {
            string where = @"  [Month] != Month(@StartDate)";
            string tmp = new Base().SqlBITableHistory("P_SubProInsReportMonthlyRate", "P_SubProInsReportMonthlyRate_History", "#tmp", where, false, false);

            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"
            
            SELECT
            [Month]
            ,FactoryID
            ,[SubprocessRate] = CAST(A.TotalPassQty / TotalQty * 100 AS DECIMAL(10, 2))
            ,[TotalPassQty]
            ,[TotalQty]
            ,[BIFactoryID] = @BIFactoryID
            ,[BIInsertDate] = GetDate()
            INTO #tmp
            FROM
            (
	            select 
	            [Month] = CASE WHEN DATEPART(month, GETDATE()) = 1 THEN 12 
                               ELSE DATEPART(month, GETDATE()) - 1 
                          END
	            ,FactoryID
	            ,[TotalPassQty] = SUM(Qty) - SUM(RejectQty)
	            ,[TotalQty] = SUM(Qty)
	            from P_SubProInsReport
	            where InspectionDate BETWEEN @StartDate and @EndDate
	            Group BY FactoryID
            )A

            {tmp}
            ----- 刪除
            DELETE P_SubProInsReportMonthlyRate WHERE [Month] != Month(@StartDate)

            ----更新
            UPDATE P SET
             P.[SubprocessRate] = ISNULL(T.[SubprocessRate],0)
            ,P.[TotalPassQty] = ISNULL(T.[TotalPassQty],0)
            ,P.[TotalQty] = ISNULL(T.[TotalQty],0)
            ,P.BIFactoryID = ISNULL(T.BIFactoryID, '')
            ,P.BIInsertDate = ISNULL(T.BIInsertDate, GetDate())
            FROM P_SubProInsReportMonthlyRate P
            INNER JOIN #TMP T ON P.[Month] = T.[Month] AND P.[FactoryID] = T.[FactoryID]
            
            
            -----新增
            INSERT INTO [dbo].[P_SubProInsReportMonthlyRate]
            (
	            [Month]
	            ,[FactoryID]
	            ,[SubprocessRate]
	            ,[TotalPassQty]
	            ,[TotalQty]
                ,[BIFactoryID]
                ,[BIInsertDate]
            )
            SELECT
             [Month] = Month(@StartDate)
            ,[FactoryID] = ISNULL(T.[FactoryID],'')
            ,[SubprocessRate] = ISNULL(T.[SubprocessRate],0)
            ,[TotalPassQty] = ISNULL(T.[TotalPassQty],0)
            ,[TotalQty] = ISNULL(T.[TotalQty],0)
            ,[BIFactoryID] = isnull(T.BIFactoryID, '')
            ,[BIInsertDate] = isnull(T.BIInsertDate, GetDate())
            from #tmp T
            Where NOT EXISTS(SELECT 1 FROM P_SubProInsReportMonthlyRate P WHERE P.[Month] = T.[Month] AND P.[FactoryID] = T.[FactoryID])   

            Drop Table #tmp
            ";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", item.SDate.Value.ToString("yyyy/MM/dd")),
                    new SqlParameter("@EndDate", item.EDate.Value.ToString("yyyy/MM/dd")),
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

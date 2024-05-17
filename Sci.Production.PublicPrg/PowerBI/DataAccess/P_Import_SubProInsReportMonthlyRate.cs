﻿using Sci.Data;
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
        public Base_ViewModel P_SubProInsReportMonthlyRate(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DateTime now = DateTime.Now;

            if (!sDate.HasValue)
            {
                sDate = MyUtility.Convert.GetDate(new DateTime(now.Year, now.Month, 1).AddMonths(-1).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = MyUtility.Convert.GetDate(new DateTime(now.Year, now.Month, 1).AddDays(-1).ToString("yyyy/MM/dd"));
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

        private Base_ViewModel UpdateBIData(DateTime sDate, DateTime eDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"
            
            SELECT
            [Month]
            ,FactoryID
            ,[SubprocessRate] = CAST(A.TotalPassQty / TotalQty * 100 AS DECIMAL(10, 2))
            ,[TotalPassQty]
            ,[TotalQty]
            INTO #tmp
            FROM
            (
	            select 
	            [Month] = DATEPART(month, GETDATE())-1
	            ,FactoryID
	            ,[TotalPassQty] = SUM(Qty) - SUM(RejectQty)
	            ,[TotalQty] = SUM(Qty)
	            from P_SubProInsReport
	            where InspectionDate BETWEEN @StartDate and @EndDate
	            Group BY FactoryID
            )A

            ----- 刪除
            DELETE P_SubProInsReportMonthlyRate WHERE [Month] != Month(@StartDate)

            ----更新
            UPDATE P SET
             P.[SubprocessRate] = 22 --ISNULL(T.[SubprocessRate],0)
            ,P.[TotalPassQty] = ISNULL(T.[TotalPassQty],0)
            ,P.[TotalQty] = ISNULL(T.[TotalQty],0)
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
            )
            SELECT
             [Month] = Month(@StartDate)
            ,[FactoryID] = ISNULL(T.[FactoryID],'')
            ,[SubprocessRate] = ISNULL(T.[SubprocessRate],0)
            ,[TotalPassQty] = ISNULL(T.[TotalPassQty],0)
            ,[TotalQty] = ISNULL(T.[TotalQty],0)
            from #tmp T
            Where NOT EXISTS(SELECT 1 FROM P_SubProInsReportMonthlyRate P WHERE P.[Month] = T.[Month] AND P.[FactoryID] = T.[FactoryID])   

            IF EXISTS (SELECT 1 FROM BITableInfo B WHERE B.ID = 'P_SubProInsReportMonthlyRate')
            BEGIN
	            UPDATE B
	            SET b.TransferDate = getdate()
	            FROM BITableInfo B
	            WHERE B.ID = 'P_SubProInsReportMonthlyRate'
            END
            ELSE 
            BEGIN
	            INSERT INTO BITableInfo(Id, TransferDate)
	            VALUES('P_SubProInsReportMonthlyRate', GETDATE())
            END

            Drop Table #tmp
            ";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", sDate.ToString("yyyy/MM/dd")),
                    new SqlParameter("@EndDate", eDate.ToString("yyyy/MM/dd")),
                };

                finalResult = new Base_ViewModel()
                {
                    Result = Data.DBProxy.Current.ExecuteByConn(conn: sqlConn, cmdtext: sqlcmd, parameters: sqlParameters),
                };
            }

            return finalResult;
        }

    }
}

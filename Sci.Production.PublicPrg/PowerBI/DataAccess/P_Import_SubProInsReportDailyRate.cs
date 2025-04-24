using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SubProInsReportDailyRate
    {
        /// <inheritdoc/>
        public Base_ViewModel P_SubProInsReportDailyRate(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd"));
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
            string where = @" p.InspectionDate NOT BETWEEN  @StartDate AND @EndDate";
            string tmp = new Base().SqlBITableHistory("P_SubProInsReportDailyRate", "P_SubProInsReportDailyRate_History", "#tmp", where, false, false);

            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"          
            SELECT
            InspectionDate
            ,FactoryID
            ,[SubprocessRate] = CAST(A.TotalPassQty / TotalQty * 100 AS DECIMAL(10, 2))
            ,[TotalPassQty]
            ,[TotalQty]
            ,[BIFactoryID] =  (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
            ,[BIInsertDate] = GetDate()
            into #tmp
            FROM
            (
	            select 
	            InspectionDate
	            ,FactoryID
	            ,[TotalPassQty] = SUM(Qty) - SUM(RejectQty)
	            ,[TotalQty] = SUM(Qty)
	            from P_SubProInsReport
	            Where InspectionDate BETWEEN @StartDate and @EndDate
	            Group BY FactoryID,InspectionDate
            )A
            ORDER by InspectionDate
            
            {tmp}
            ----- 刪除
            DELETE P_SubProInsReportDailyRate WHERE InspectionDate NOT BETWEEN  @StartDate AND @EndDate

            ----更新
            UPDATE P SET
             P.[SubprocessRate] = ISNULL(T.[SubprocessRate],0)
            ,P.[TotalPassQty] = ISNULL(T.[TotalPassQty],0)
            ,P.[TotalQty] = ISNULL(T.[TotalQty],0)
            ,P.BIFactoryID = ISNULL(T.BIFactoryID, '')
            ,P.BIInsertDate = ISNULL(T.BIInsertDate, GetDate())
            FROM P_SubProInsReportDailyRate P
            INNER JOIN #TMP T ON P.[InspectionDate] = T.[InspectionDate] AND P.[FactoryID] = T.[FactoryID]
            
            -----新增
            INSERT INTO [dbo].[P_SubProInsReportDailyRate]
            (
	            [InspectionDate]
	            ,[FactoryID]
	            ,[SubprocessRate]
	            ,[TotalPassQty]
	            ,[TotalQty]
                ,[BIFactoryID]
                ,[BIInsertDate])
            SELECT
             [InspectionDate]
            ,[FactoryID] = ISNULL(T.[FactoryID],'')
            ,[SubprocessRate] = ISNULL(T.[SubprocessRate],0)
            ,[TotalPassQty] = ISNULL(T.[TotalPassQty],0)
            ,[TotalQty] = ISNULL(T.[TotalQty],0)
            ,[BIFactoryID] = isnull(BIFactoryID, '')
            ,[BIInsertDate] = isnull(BIInsertDate, GetDate())
            from #tmp T
            Where NOT EXISTS(SELECT 1 FROM P_SubProInsReportDailyRate P WHERE P.[InspectionDate] = T.[InspectionDate] AND P.[FactoryID] = T.[FactoryID])   


            IF EXISTS (SELECT 1 FROM BITableInfo B WHERE B.ID = 'P_SubProInsReportDailyRate')
            BEGIN
	            UPDATE B
	            SET b.TransferDate = getdate()
	            FROM BITableInfo B
	            WHERE B.ID = 'P_SubProInsReportDailyRate'
            END
            ELSE 
            BEGIN
	            INSERT INTO BITableInfo(Id, TransferDate)
	            VALUES('P_SubProInsReportDailyRate', GETDATE())
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

using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_RecevingInfoTrackingSummary
    {
        /// <inheritdoc/>
        public P_Import_RecevingInfoTrackingSummary()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel P_RecevingInfoTrackingSummary(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddMonths(-3).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd"));
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

            string sqlcmd = $@"        
WITH DateRange AS (
    SELECT @StartDate AS date
    UNION ALL
    SELECT DATEADD(DAY, 1, date)
    FROM DateRange
    WHERE date < @EndDate
)

SELECT DISTINCT 
    DateRange.date,
    main.FtyGroup
INTO #tmpDate
FROM DateRange
INNER JOIN dbo.P_BatchUpdateRecevingInfoTrackingList main
    ON DateRange.date = main.ArriveDate
    OR main.CutShadebandTime BETWEEN DATEADD(DAY, -7, DateRange.date) AND DateRange.date
OPTION (MAXRECURSION 0);


SELECT 
    TransferDate     = DATEADD(DAY, 1, d.date),
    FactoryID        = d.FtyGroup,
    UnloaderTtlKG    = ISNULL(SUM(main.Weight), 0),
    UnloaderTtlRoll  = SUM(IIF(main.FtyGroup IS NULL, 0, 1)),
    WHReceivingLT    = ISNULL(LT.WHReceivingLT, 0),
    BIFactoryID      = @BIFactoryID,
    BIInsertDate     = GETDATE(),
    BIStatus         = 'New'
INTO #tmp
FROM #tmpDate d
LEFT JOIN dbo.P_BatchUpdateRecevingInfoTrackingList main
    ON d.date = main.ArriveDate 
    AND d.FtyGroup = main.FtyGroup
OUTER APPLY (
    SELECT 
        WHReceivingLT = ROUND(
            SUM(
                IIF(
                    CutShadebandTime IS NULL OR ArriveDate IS NULL, 
                    NULL,
                    IIF(DATEDIFF(DAY, ArriveDate, CutShadebandTime) < 0, 0, DATEDIFF(DAY, ArriveDate, CutShadebandTime))
                )
            ) * 1.0 / COUNT(*), 
        2)
    FROM dbo.P_BatchUpdateRecevingInfoTrackingList
    WHERE 
        CutShadebandTime BETWEEN DATEADD(DAY, -7, d.date) AND d.date
        AND ArriveDate IS NOT NULL
        AND FtyGroup = d.FtyGroup
) LT
GROUP BY d.FtyGroup, d.date, LT.WHReceivingLT
ORDER BY d.date, d.FtyGroup

UPDATE T
SET
    T.UnloaderTtlKG     = tmp.UnloaderTtlKG,
    T.UnloaderTtlRoll   = tmp.UnloaderTtlRoll,
    T.WHReceivingLT     = tmp.WHReceivingLT,
    T.BIFactoryID       = tmp.BIFactoryID,
    T.BIInsertDate      = tmp.BIInsertDate,
    T.BIStatus          = tmp.BIStatus
FROM dbo.P_RecevingInfoTrackingSummary T
INNER JOIN #tmp tmp 
    ON T.TransferDate = tmp.TransferDate 
   AND T.FactoryID    = tmp.FactoryID

 INSERT INTO dbo.P_RecevingInfoTrackingSummary (
    TransferDate,
    FactoryID,
    UnloaderTtlKG,
    UnloaderTtlRoll,
    WHReceivingLT,
    BIFactoryID,
    BIInsertDate,
    BIStatus
)
SELECT
    tmp.TransferDate,
    tmp.FactoryID,
    tmp.UnloaderTtlKG,
    tmp.UnloaderTtlRoll,
    tmp.WHReceivingLT,
    tmp.BIFactoryID,
    tmp.BIInsertDate,
    tmp.BIStatus
FROM #tmp tmp
WHERE NOT EXISTS (
    SELECT 1 
    FROM dbo.P_RecevingInfoTrackingSummary T
    WHERE T.TransferDate = tmp.TransferDate 
      AND T.FactoryID = tmp.FactoryID
)

DROP TABLE #tmp
DROP TABLE #tmpDate
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

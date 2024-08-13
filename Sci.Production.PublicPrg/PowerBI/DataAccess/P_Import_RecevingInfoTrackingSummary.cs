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
        public Base_ViewModel P_RecevingInfoTrackingSummary(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddMonths(-3).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd"));
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
            --建立日期區間
            WITH DateRange AS (
                SELECT @StartDate AS date
                UNION ALL
                SELECT DATEADD(DAY, 1, date)
                FROM DateRange
                WHERE date < @EndDate
            )
            
            --取得所有符合的日期及Fty
            SELECT distinct DateRange.date,main.FtyGroup 
            into #tmpDate
            FROM DateRange
            inner join [dbo].[P_BatchUpdateRecevingInfoTrackingList] main on DateRange.date = main.ArriveDate or main.CutShadebandTime BETWEEN DATEADD(DAY,-7, DateRange.date) and DateRange.date
            OPTION (MAXRECURSION 0);

            select 
            [TransferDate] = DATEADD(DAY,1,DateRange.date),
            [FactoryID] = DateRange.FtyGroup,
            [UnloaderTtlKG] = isnull(sum(Weight),0),
            [UnloaderTtlRoll] = sum(iif(main.FtyGroup is null, 0, 1)),
            [WHReceivingLT] = isnull([WHReceivingLT],0)
            into #tmp
            from #tmpDate DateRange
            left join [dbo].[P_BatchUpdateRecevingInfoTrackingList] main on DateRange.date = main.ArriveDate and DateRange.FtyGroup = main.FtyGroup
            OUTER APPLY(
	            SELECT 
	            [WHReceivingLT] = round(sum(IIF(CutShadebandTime is null,null,IIF(ArriveDate is null,null, IIF(DATEDIFF(day,ArriveDate,CutShadebandTime)<0, 0, DATEDIFF(day, ArriveDate,CutShadebandTime)))))*1.0/count(*),2)
	            from [dbo].[P_BatchUpdateRecevingInfoTrackingList]
	            where CutShadebandTime between DATEADD(DAY,-7, DateRange.date) and DateRange.date
	            and ArriveDate is not null
	            and FtyGroup = DateRange.FtyGroup
            )getWHReceivingLT
            group by DateRange.FtyGroup,DateRange.date,getWHReceivingLT.WHReceivingLT
            ORDER BY DateRange.date,DateRange.FtyGroup

            ----更新
            UPDATE T SET
            T.[UnloaderTtlKG] = tmp.[UnloaderTtlKG],
            T.[UnloaderTtlRoll] = tmp.[UnloaderTtlRoll],
            T.[WHReceivingLT] = tmp.[WHReceivingLT]
            FROM P_RecevingInfoTrackingSummary T
            INNER JOIN #TMP tmp ON T.[TransferDate] = tmp.[TransferDate] AND  T.[FactoryID] = tmp.[FactoryID]
            
            -----新增
            INSERT INTO [dbo].[P_RecevingInfoTrackingSummary]
            (
	             [TransferDate]
	            ,[FactoryID]
	            ,[UnloaderTtlKG]
	            ,[UnloaderTtlRoll]
	            ,[WHReceivingLT]
            )
            SELECT
             [TransferDate]
            ,[FactoryID] 
	        ,[UnloaderTtlKG]
	        ,[UnloaderTtlRoll]
	        ,[WHReceivingLT]
            from #tmp tmp
            Where NOT EXISTS(SELECT 1 FROM P_RecevingInfoTrackingSummary T WHERE tmp.[TransferDate] = T.[TransferDate] AND tmp.[FactoryID] = T.[FactoryID])   
           
            IF EXISTS (SELECT 1 FROM BITableInfo B WHERE B.ID = 'P_RecevingInfoTrackingSummary')
            BEGIN
	            UPDATE B
	            SET b.TransferDate = getdate()
	            FROM BITableInfo B
	            WHERE B.ID = 'P_RecevingInfoTrackingSummary'
            END
            ELSE 
            BEGIN
	            INSERT INTO BITableInfo(Id, TransferDate)
	            VALUES('P_RecevingInfoTrackingSummary', GETDATE())
            END
            Drop Table #tmp
            Drop Table #tmpDate
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

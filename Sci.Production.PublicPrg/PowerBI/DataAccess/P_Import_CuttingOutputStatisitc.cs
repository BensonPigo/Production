using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_CuttingOutputStatisitc
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_CuttingOutputStatisitc(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-90).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.GetCuttingOutputStatisitc((DateTime)sDate, (DateTime)eDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, (DateTime)sDate, (DateTime)eDate);
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

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sdate, DateTime edate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@sDate", sdate.ToString("yyyy-MM-dd")),
                new SqlParameter("@eDate", edate.ToString("yyyy-MM-dd")),
            };

            using (sqlConn)
            {
                string sql = $@" 
Update p Set CutRateByDate = t.CutRateByDate,
             CutRateByMonth = t.CutRateByMonth,
             CutOutputByDate = t.CutOutputByDate,
             CutOutputIn7Days = t.CutOutputIn7Days,
             CutDelayIn7Days = t.CutDelayIn7Days
From P_CuttingOutputStatisitc p
inner join #tmp t on p.TransferDate = t.TransferDate 
                 and p.FactoryID = t.FactoryID
                 and (p.CutRateByDate != t.CutRateByDate
                      or p.CutRateByMonth != t.CutRateByMonth
                      or p.CutOutputByDate != t.CutOutputByDate
                      or p.CutOutputIn7Days != t.CutOutputIn7Days
                      or p.CutDelayIn7Days != t.CutDelayIn7Days )


Insert into P_CuttingOutputStatisitc ( TransferDate,
                                       FactoryID, 
                                       CutRateByDate, 
                                       CutRateByMonth, 
                                       CutOutputByDate, 
                                       CutOutputIn7Days, 
                                       CutDelayIn7Days
                                     )
Select TransferDate,
       FactoryID, 
       isnull(CutRateByDate, 0), 
       isnull(CutRateByMonth, 0), 
       isnull(CutOutputByDate, 0), 
       isnull(CutOutputIn7Days, 0),
       isnull(CutDelayIn7Days, 0)
From #tmp t
Where not exists ( select 1 
				   from P_CuttingOutputStatisitc p
				   where p.TransferDate = t.TransferDate 
				   and p.FactoryID = t.FactoryID
                )


Delete P_CuttingOutputStatisitc 
Where Not exists ( select 1 
				   from #tmp t
			       where P_CuttingOutputStatisitc.TransferDate = t.TransferDate 
				   and P_CuttingOutputStatisitc.FactoryID = t.FactoryID
                )
And P_CuttingOutputStatisitc.TransferDate Between @sDate and @eDate


";

                result = MyUtility.Tool.ProcessWithDatatable(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter);
            }

            finalResult.Result = result;

            return finalResult;
        }

        private Base_ViewModel GetCuttingOutputStatisitc(DateTime sdate, DateTime edate)
        {
            StringBuilder sqlCmd = new StringBuilder();

            #region SQL

            // 基本資料
            sqlCmd.Append($@"
SELECT
    FactoryID = psol.FactoryID,
    TransferDate = psol.EstCuttingDate,
    CutRateByDate = IIF(ISNULL(psConsumption.Value, 0) = 0, 0,
                        ISNULL((
                            SELECT SUM(P_CuttingScheduleOutputList.ActConsOutput)
                            FROM P_CuttingScheduleOutputList
                            WHERE EstCuttingDate = DATEADD(DAY, -1, psol.EstCuttingDate)
                              AND ActCuttingDate <= DATEADD(DAY, -1, psol.EstCuttingDate)
                        ) / psConsumption.Value, 0)),
    CutRateByMonth = IIF(ISNULL(psConsumptionByMonth.value, 0) = 0, 0,
                         ISNULL((
                             SELECT SUM(P_CuttingScheduleOutputList.ActConsOutput)
                             FROM P_CuttingScheduleOutputList WITH (NOLOCK)
                             WHERE EstCuttingDate BETWEEN DATEFROMPARTS(YEAR(psol.EstCuttingDate), MONTH(psol.EstCuttingDate), 1)
                                                    AND DATEADD(DAY, -1, psol.EstCuttingDate)
                               AND ActCuttingDate < DATEADD(DAY, -1, psol.EstCuttingDate)
                               AND FactoryID = psol.FactoryID
                         ) / psConsumptionByMonth.value, 0)),
    CutOutputByDate = ISNULL((
                             SELECT SUM(P_CuttingScheduleOutputList.Consumption)
                             FROM P_CuttingScheduleOutputList
                             WHERE P_CuttingScheduleOutputList.EstCuttingDate = psol.EstCuttingDate
                         ), 0),
    CutOutputIn7Days = ISNULL(psByWeek.ActConsOutput, 0),
    CutDelayIn7Days = ISNULL(psByWeek.BalanceCons, 0)
FROM (
    SELECT DISTINCT
        psol.EstCuttingDate,
        psol.FactoryID,
        psol.ActCuttingDate
    FROM P_CuttingScheduleOutputList psol WITH (NOLOCK)
    WHERE psol.EstCuttingDate BETWEEN @SDate AND @EDate
) psol
OUTER APPLY (
    SELECT value = SUM(pso.Consumption)
    FROM P_CuttingScheduleOutputList pso
    WHERE pso.EstCuttingDate = DATEADD(DAY, -1, psol.EstCuttingDate)
) psConsumption
OUTER APPLY (
    SELECT value = SUM(pso.Consumption)
    FROM P_CuttingScheduleOutputList pso WITH (NOLOCK)
    WHERE pso.EstCuttingDate BETWEEN DATEFROMPARTS(YEAR(psol.EstCuttingDate), MONTH(psol.EstCuttingDate), 1)
                                  AND DATEADD(DAY, -1, psol.EstCuttingDate)
      AND FactoryID = psol.FactoryID
) psConsumptionByMonth
OUTER APPLY (
    SELECT ActConsOutput = SUM(P_CuttingScheduleOutputList.ActConsOutput),
           BalanceCons = SUM(P_CuttingScheduleOutputList.BalanceCons)
    FROM P_CuttingScheduleOutputList WITH (NOLOCK)
    WHERE ActCuttingDate BETWEEN DATEADD(DAY, -7, psol.EstCuttingDate) AND psol.EstCuttingDate
      AND FactoryID = psol.FactoryID
) psByWeek
GROUP BY
    psol.FactoryID,
    psol.EstCuttingDate,
	psConsumption.value,
	psConsumptionByMonth.value,
	psByWeek.ActConsOutput,
	psByWeek.BalanceCons
ORDER BY
    psol.FactoryID,
    psol.EstCuttingDate ASC;

");

            #endregion

            List<SqlParameter> paras = new List<SqlParameter>
            {
                new SqlParameter("@SDate", sdate.ToString("yyyy-MM-dd")),
                new SqlParameter("@EDate", edate.ToString("yyyy-MM-dd")),
            };

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("PowerBI", sqlCmd.ToString(), paras, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }
    }
}
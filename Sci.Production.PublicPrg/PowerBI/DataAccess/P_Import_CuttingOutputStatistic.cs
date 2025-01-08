using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_CuttingOutputStatistic
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_CuttingOutputStatistic(DateTime? sDate, DateTime? eDate)
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
                Base_ViewModel resultReport = this.GetCuttingOutputStatistic((DateTime)sDate, (DateTime)eDate);
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
From P_CuttingOutputStatistic p
inner join #tmp t on p.TransferDate = t.TransferDate 
                 and p.FactoryID = t.FactoryID
                 and (p.CutRateByDate != t.CutRateByDate
                      or p.CutRateByMonth != t.CutRateByMonth
                      or p.CutOutputByDate != t.CutOutputByDate
                      or p.CutOutputIn7Days != t.CutOutputIn7Days
                      or p.CutDelayIn7Days != t.CutDelayIn7Days )


Insert into P_CuttingOutputStatistic ( TransferDate,
                                       FactoryID, 
                                       CutRateByDate, 
                                       CutRateByMonth, 
                                       CutOutputByDate, 
                                       CutOutputIn7Days, 
                                       CutDelayIn7Days
                                     )
Select TransferDate,
       FactoryID, 
       CutRateByDate, 
       CutRateByMonth, 
       CutOutputByDate, 
       CutOutputIn7Days,
       CutDelayIn7Days
From #tmp t
Where not exists ( select 1 
				   from P_CuttingOutputStatistic p
				   where p.TransferDate = t.TransferDate 
				   and p.FactoryID = t.FactoryID
                )


Delete P_CuttingOutputStatistic 
Where Not exists ( select 1 
				   from #tmp t
			       where P_CuttingOutputStatistic.TransferDate = t.TransferDate 
				   and P_CuttingOutputStatistic.FactoryID = t.FactoryID
                )
And P_CuttingOutputStatistic.TransferDate Between @sDate and @eDate


if exists (select 1 from BITableInfo b where b.id = 'P_CuttingOutputStatistic')
begin
    update b
        set b.TransferDate = getdate()
    from BITableInfo b
    where b.id = 'P_CuttingOutputStatistic'
end
else 
begin
    insert into BITableInfo(Id, TransferDate)
    values('P_CuttingOutputStatistic', getdate())
end
";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter);
            }

            finalResult.Result = result;

            return finalResult;
        }

        private Base_ViewModel GetCuttingOutputStatistic(DateTime sdate, DateTime edate)
        {
            StringBuilder sqlCmd = new StringBuilder();

            #region SQL

            // 基本資料
            sqlCmd.Append($@"
SELECT
    FactoryID = psol.FactoryID,
    TransferDate = psol.EstCuttingDate,
    CutRateByDate = IIF(ISNULL(psConsumption.Value, 0) = 0, 0,
                        ISNULL(psConsumptionMol.Value / psConsumption.Value, 0)),
    CutRateByMonth = IIF(ISNULL(psConsumptionByMonth.value, 0) = 0, 0,
                         ISNULL(psConsumptionByMonthMol.value / psConsumptionByMonth.value, 0)),
    CutOutputByDate = ISNULL(psCutOutputByDate.value, 0),
    CutOutputIn7Days = ISNULL(psByWeek.ActConsOutput, 0),
    CutDelayIn7Days = ISNULL(psByWeek.BalanceCons, 0)
FROM (
    SELECT distinct  psol.EstCuttingDate,
        psol.FactoryID
    FROM P_CuttingScheduleOutputList psol WITH (NOLOCK)
    WHERE psol.EstCuttingDate BETWEEN @SDate AND @EDate
) psol
OUTER APPLY (
	SELECT value = SUM(pso.ActConsOutput)
    FROM P_CuttingScheduleOutputList pso WITH (NOLOCK)
    WHERE pso.EstCuttingDate = DATEADD(DAY, -1, psol.EstCuttingDate)
    AND pso.ActCuttingDate <= DATEADD(DAY, -1, psol.EstCuttingDate)
	AND pso.FactoryID = psol.FactoryID
) psConsumptionMol
OUTER APPLY (
    SELECT value = SUM(pso.Consumption)
    FROM P_CuttingScheduleOutputList pso WITH (NOLOCK)
    WHERE pso.EstCuttingDate = DATEADD(DAY, -1, psol.EstCuttingDate)
	AND pso.FactoryID = psol.FactoryID
) psConsumption
OUTER APPLY (
    SELECT value = SUM(pso.ActConsOutput)
    FROM P_CuttingScheduleOutputList pso WITH (NOLOCK)
    WHERE pso.EstCuttingDate BETWEEN DATEFROMPARTS(YEAR(psol.EstCuttingDate), MONTH(psol.EstCuttingDate), 1)
                        AND DATEADD(DAY, -1, psol.EstCuttingDate)
    AND pso.ActCuttingDate <= DATEADD(DAY, -1, psol.EstCuttingDate)
    AND pso.FactoryID = psol.FactoryID
) psConsumptionByMonthMol
OUTER APPLY (
    SELECT value = SUM(pso.Consumption)
    FROM P_CuttingScheduleOutputList pso WITH (NOLOCK)
    WHERE pso.EstCuttingDate BETWEEN DATEFROMPARTS(YEAR(psol.EstCuttingDate), MONTH(psol.EstCuttingDate), 1)
                                  AND DATEADD(DAY, -1, psol.EstCuttingDate)
      AND FactoryID = psol.FactoryID
) psConsumptionByMonth
OUTER APPLY (
	SELECT value = SUM(pso.Consumption)
    FROM P_CuttingScheduleOutputList pso WITH (NOLOCK)
    WHERE pso.ActCuttingDate = DATEADD(DAY, -1, psol.EstCuttingDate)
	AND pso.FactoryID = psol.FactoryID
) psCutOutputByDate
OUTER APPLY (
    SELECT ActConsOutput = SUM(pso.ActConsOutput),
           BalanceCons = SUM(pso.BalanceCons)
    FROM P_CuttingScheduleOutputList pso WITH (NOLOCK)
    WHERE pso.ActCuttingDate BETWEEN DATEADD(DAY, -7, psol.EstCuttingDate) AND DATEADD(DAY, -1, psol.EstCuttingDate)
    AND pso.FactoryID = psol.FactoryID
) psByWeek
ORDER BY psol.FactoryID, psol.EstCuttingDate ASC

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
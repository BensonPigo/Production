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
                sDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.AddDays(-90).ToString("yyyy/MM/dd"));
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

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>();
            lisSqlParameter.Add(new SqlParameter("@sDate", sdate.ToString("yyyy-MM-dd")));
            lisSqlParameter.Add(new SqlParameter("@eDate", edate.ToString("yyyy-MM-dd")));

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
Select  FactoryID = psol.FactoryID
	  , TransferDate = psol.EstCuttingDate

	  , CutRateByDate = isnull((select sum(P_CuttingScheduleOutputList.ActConsOutput)
					            from P_CuttingScheduleOutputList 
						        where EstCuttingDate = DateADD(DAY, -1, psol.EstCuttingDate)
						        and ActCuttingDate <= DateADD(DAY, -1, psol.EstCuttingDate))
						        /
						       (select sum(P_CuttingScheduleOutputList.Consumption)
						        from P_CuttingScheduleOutputList 
						        where EstCuttingDate = DateADD(DAY, -1, psol.EstCuttingDate)),0)

	  , CutRateByMonth = iif(
                              (select sum(P_CuttingScheduleOutputList.Consumption)
						       from P_CuttingScheduleOutputList  with(nolock)
						       where EstCuttingDate between DATEFROMPARTS(YEAR(psol.EstCuttingDate), MONTH(psol.EstCuttingDate), 1) and DATEADD(DAY, -1, psol.EstCuttingDate)
						       and FactoryID = psol.FactoryID ) = 0,
                               0, 
                               isnull(
                                 ( 
                                --ActConsOutput
                                  (select sum(P_CuttingScheduleOutputList.ActConsOutput)
					               from P_CuttingScheduleOutputList  with(nolock)
					               where EstCuttingDate between DATEFROMPARTS(YEAR(psol.EstCuttingDate), MONTH(psol.EstCuttingDate), 1) and DATEADD(DAY, -1, psol.EstCuttingDate)
					               and ActCuttingDate < DATEADD(DAY, -1, psol.EstCuttingDate)
					               and FactoryID = psol.FactoryID
					              )
					              /  
					            --Consumption
					              (select sum(P_CuttingScheduleOutputList.Consumption)
						           from P_CuttingScheduleOutputList  with(nolock)
						           where EstCuttingDate between DATEFROMPARTS(YEAR(psol.EstCuttingDate), MONTH(psol.EstCuttingDate), 1) and DATEADD(DAY, -1, psol.EstCuttingDate)
						           and FactoryID = psol.FactoryID
					              )
                                ), 0)
                              )

	  , CutOutputByDate = isnull((select sum(P_CuttingScheduleOutputList.Consumption)
						          from P_CuttingScheduleOutputList 
							      where P_CuttingScheduleOutputList.EstCuttingDate = psol.EstCuttingDate)
                                ,0)

      , CutOutputIn7Days = isnull((select sum(P_CuttingScheduleOutputList.ActConsOutput) 
						           from P_CuttingScheduleOutputList  with(nolock)
						           where ActCuttingDate between DATEADD(DAY,-7,psol.EstCuttingDate ) and psol.EstCuttingDate
						           and FactoryID = psol.FactoryID)
                                 ,0)

	  , CutDelayIn7Days = isnull((select sum(P_CuttingScheduleOutputList.BalanceCons) 
                                  from P_CuttingScheduleOutputList  with(nolock)
						          where ActCuttingDate between DATEADD(DAY,-7,psol.EstCuttingDate ) and psol.EstCuttingDate
						          and FactoryID = psol.FactoryID)
                                ,0)
From (
		Select psol.EstCuttingDate
			 , psol.FactoryID
			 , psol.ActCuttingDate
		From P_CuttingScheduleOutputList psol with(nolock)
		Where psol.EstCuttingDate between @SDate and @EDate
) psol
Group By  psol.FactoryID ,psol.EstCuttingDate 
Order by  psol.FactoryID ,psol.EstCuttingDate  asc

");

            #endregion

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@SDate", sdate.ToString("yyyy-MM-dd")));
            paras.Add(new SqlParameter("@EDate", edate.ToString("yyyy-MM-dd")));

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
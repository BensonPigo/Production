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
    public class P_Import_MaterialCompletionRateByWeek
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_MaterialCompletionRateByWeek(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.GetMaterialCompletionRateByWeek(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, item);
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

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@Date", item.SDate),
                new SqlParameter("@IsTrans", item.IsTrans),
            };

            using (sqlConn)
            {
                string sql = $@" 
Update p Set MaterialCompletionRate = t.MaterialCompletionRate, 
             MTLCMP_SPNo = t.MTLCMP_SPNo,
             TTLSPNo = t.TTLSPNo,
             BIFactoryID = t.BIFactoryID,
             BIInsertDate = t.BIInsertDate
From P_MaterialCompletionRateByWeek p
inner join #tmp t on p.Year = t.Year 
                 and p.WeekNo = t.WeekNo 
                 and p.FactoryID = t.FactoryID

Insert into P_MaterialCompletionRateByWeek ( Year,
                                             WeekNo, 
                                             FactoryID, 
                                             MaterialCompletionRate, 
                                             MTLCMP_SPNo, 
                                             TTLSPNo,
                                             [BIFactoryID],
                                             [BIInsertDate]
                                            )
Select  Year,
        WeekNo, 
        FactoryID, 
        MaterialCompletionRate, 
        MTLCMP_SPNo, 
        TTLSPNo,
        [BIFactoryID],
        [BIInsertDate]
From #tmp t
Where not exists ( select 1 
				   from P_MaterialCompletionRateByWeek p with (nolock)
				   where p.Year = t.Year 
                   and p.WeekNo = t.WeekNo 
				   and p.FactoryID = t.FactoryID
                )

if @IsTrans = 1
begin
    Insert into P_MaterialCompletionRateByWeek_History([Year], [WeekNo], [FactoryID], [BIFactoryID], [BIInsertDate])
    Select [Year], [WeekNo], [FactoryID], [BIFactoryID], [BIInsertDate] = GetDate()
    From P_MaterialCompletionRateByWeek
    Where Not exists ( select 1 
				       from #tmp t
				       where P_MaterialCompletionRateByWeek.Year = t.Year 
                       and P_MaterialCompletionRateByWeek.WeekNo = t.WeekNo 
				       and P_MaterialCompletionRateByWeek.FactoryID = t.FactoryID
                    )
    And P_MaterialCompletionRateByWeek.Year <= YEAR(@Date)
    And WeekNo Between DATEPART(WEEK, @Date) And DATEPART(WEEK, @Date) + 3
end

Delete P_MaterialCompletionRateByWeek 
Where Not exists ( select 1 
				   from #tmp t
				   where P_MaterialCompletionRateByWeek.Year = t.Year 
                   and P_MaterialCompletionRateByWeek.WeekNo = t.WeekNo 
				   and P_MaterialCompletionRateByWeek.FactoryID = t.FactoryID
                )
And P_MaterialCompletionRateByWeek.Year <= YEAR(@Date)
And WeekNo Between DATEPART(WEEK, @Date) And DATEPART(WEEK, @Date) + 3

if @IsTrans = 1
begin
    Insert into P_MaterialCompletionRateByWeek_History([Year], [WeekNo], [FactoryID], [BIFactoryID], [BIInsertDate])
    Select [Year], [WeekNo], [FactoryID], [BIFactoryID], [BIInsertDate] = GetDate()
    From P_MaterialCompletionRateByWeek
    Where P_MaterialCompletionRateByWeek.Year <= YEAR(@Date)
    And P_MaterialCompletionRateByWeek.WeekNo < DATEPART(WEEK, @Date)
end

Delete P_MaterialCompletionRateByWeek 
Where P_MaterialCompletionRateByWeek.Year <= YEAR(@Date)
And P_MaterialCompletionRateByWeek.WeekNo < DATEPART(WEEK, @Date)
";

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql,  out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter);
            }

            return finalResult;
        }

        private Base_ViewModel GetMaterialCompletionRateByWeek(ExecutedList item)
        {
            StringBuilder sqlCmd = new StringBuilder();

            #region SQL

            // 基本資料
            sqlCmd.Append($@"
Select 	Year = YEAR(inline),	
        WeekNO = DATEPART(WEEK, psb.inline) ,
        FactoryID,
        MaterialCompletionRate = CONVERT(numeric(5, 2), iif(isnull(TTL.TTLSPNo,0) = 0, 0, round((CONVERT(numeric(5, 2),isnull(MTLCMP.MTLCMP_SPNo, 0))/(CONVERT(numeric(5, 2),TTL.TTLSPNo)))*　100, 2))) ,
        MTLCMP_SPNo = isnull(MTLCMP.MTLCMP_SPNo, ''),
        TTLSPNo = isnull(TTL.TTLSPNo, ''),
        [BIFactoryID] = @BIFactoryID,
        [BIInsertDate] = GETDATE()
From [P_SewingLineScheduleBySP] psb with (nolock)
Outer Apply(
	         Select Count(1) As MTLCMP_SPNo
	         From [P_SewingLineScheduleBySP] psbi with (nolock)
	         Where Category = 'B'
             And YEAR(psb.inline) = year(psbi.inline)
	         And DATEPART(WEEK, psb.inline) = DATEPART(WEEK, psbi.inline)
             And psbi.FactoryID = psb.FactoryID
	         And psbi.MTLExport  = 'OK'
) as MTLCMP
Outer Apply(
	         Select Count(1) As TTLSPNo
	         From [P_SewingLineScheduleBySP] psbi with (nolock)
	         Where Category = 'B'
             And YEAR(psb.inline) = year(psbi.inline)
             And psbi.FactoryID = psb.FactoryID
	         And DATEPART(WEEK, psb.inline) = DATEPART(WEEK, psbi.inline)
) as TTl
Where DATEPART(WEEK, psb.inline) Between DATEPART(WEEK, @Date) And DATEPART(WEEK, @Date) + 3
And Year(inline) =  YEAR(@Date)
Group by  YEAR(psb.inline),DATEPART(WEEK, psb.inline),FactoryID, MTLCMP.MTLCMP_SPNo,TTLSPNo
");

            #endregion

            List<SqlParameter> paras = new List<SqlParameter>
            {
                new SqlParameter("@Date", item.SDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("PowerBI", sqlCmd.ToString(), paras, out DataTable dt),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dt;
            return resultReport;
        }
    }
}
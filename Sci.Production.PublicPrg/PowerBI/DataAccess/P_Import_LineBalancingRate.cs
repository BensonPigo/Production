using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_LineBalancingRate
    {
        private List<DateTime> _lineBalancingRates;
        private SqlConnection sqlConn;
        /// <summary>
        /// 寫入 P_LineBalancingRate
        /// </summary>
        /// <param name="sDate">Start Date</param>
        /// <param name="eDate">End Data</param>
        /// <param name="biTableInfoID">bi table id</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel P_LineBalancingRate(DateTime? sDate, DateTime? eDate, string biTableInfoID)
        {
            DBProxy.Current.OpenConnection("PowerBI", out this.sqlConn);
            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.GetLineBalancingRate(sDate.Value, eDate.Value);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.DtArr, sDate.Value, eDate.Value);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }
                else
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, biTableInfoID, true);
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetLineBalancingRate(DateTime sDate, DateTime eDate)
        {
            var sb = new StringBuilder();
            this._lineBalancingRates = new List<DateTime>();

            for (DateTime date = sDate; date <= eDate; date = date.AddDays(1))
            {
                sb.AppendLine($"exec P_Import_LineBalancingRate '{date:yyyy/MM/dd}'");
                this._lineBalancingRates.Add(date);
            }

            var resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("PowerBI", sb.ToString(), out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable[] dt, DateTime sDate, DateTime eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            try
            {
                DualResult result;
                using (this.sqlConn)
                {
                    for (int i = 0; i < dt.Length; i++)
                    {
                        var paramters = new List<SqlParameter>
                        {
                            new SqlParameter("@SDate", this._lineBalancingRates[i]),
                        };

                        string sql = new Base().SqlBITableHistory("P_LineBalancingRate", "P_LineBalancingRate_History", "#tmp", "SewingDate = @SDate", needJoin: false) + Environment.NewLine;
                        sql += $@"
	                    delete t
	                    from P_LineBalancingRate t
	                    where not exists(
		                    select 1 from #tmp s
		                    where s.SewingDate = t.SewingDate
		                    and s.FactoryID = t.FactoryID
	                    )
	                    and SewingDate = @SDate

	                    update t
	                    set t.[Total SP Qty] = isnull(s.[Total SP Qty],0) 
	                    ,t.[Total LBR] = isnull(s.[Total LBR],0)
	                    ,t.[Avg. LBR] = isnull(s.[Avg. LBR],0)
	                    ,t.[Total SP Qty In7Days] = isnull(s.[Total SP Qty In7Days],0)
	                    ,t.[Total LBR In7Days] = isnull(s.[Total LBR In7Days],0)
	                    ,t.[Avg. LBR In7Days] = isnull(s.[Avg. LBR In7Days],0)
                        ,t.[BIFactoryID] = isnull(s.[BIFactoryID],'')
                        ,t.[BIInsertDate] = s.[BIInsertDate]
	                    from P_LineBalancingRate t
	                    inner join #tmp s 
		                    on s.SewingDate = t.SewingDate
		                    and s.FactoryID = t.FactoryID

	                    insert into P_LineBalancingRate(
	                    [SewingDate]
	                    ,[FactoryID]
	                    ,[Total SP Qty]
	                    ,[Total LBR]
	                    ,[Avg. LBR]
	                    ,[Total SP Qty In7Days]
	                    ,[Total LBR In7Days]
	                    ,[Avg. LBR In7Days]
                        ,[BIFactoryID]
                        ,[BIInsertDate]
	                    )
	                    select  
	                    [SewingDate]
	                    ,[FactoryID]
	                    ,ISNULL([Total SP Qty] ,0)
	                    ,ISNULL([Total LBR] ,0)
	                    ,ISNULL([Avg. LBR] ,0)
	                    ,ISNULL([Total SP Qty In7Days] ,0)
	                    ,ISNULL([Total LBR In7Days] ,0)
	                    ,ISNULL([Avg. LBR In7Days] ,0)
                        ,isnull([BIFactoryID],'')
                        ,[BIInsertDate]
	                    from #tmp t
	                    where not exists(
		                    select 1 from P_LineBalancingRate s
		                    where s.SewingDate = t.SewingDate
		                    and s.FactoryID = t.FactoryID
	                    )
                        drop table #tmp
                        ";
                        result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt[i], null, sqlcmd: sql, result: out DataTable dataTable2, temptablename: "#tmp", conn: this.sqlConn, paramters: paramters);

                        if (!result.Result)
                        {
                            throw result.GetException();
                        }
                    }

                    this.sqlConn.Close();
                    this.sqlConn.Dispose();
                }

                finalResult.Result = new DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new DualResult(false, ex);
                this.sqlConn.Close();
                this.sqlConn.Dispose();
            }

            return finalResult;
        }
    }
}

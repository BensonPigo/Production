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
    public class P_Import_LineBalancingRate
    {
        private List<DateTime> _lineBalancingRates;
        private DBProxy DBProxy;

        /// <summary>
        /// 寫入 P_LineBalancingRate
        /// </summary>
        /// <param name="item">Executed List</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel P_LineBalancingRate(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.GetLineBalancingRate(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.DtArr, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetLineBalancingRate(ExecutedList item)
        {
            var sb = new StringBuilder();
            this._lineBalancingRates = new List<DateTime>();

            for (DateTime date = item.SDate.Value; date <= item.EDate.Value; date = date.AddDays(1))
            {
                sb.AppendLine($"exec P_Import_LineBalancingRate '{date:yyyy/MM/dd}'");
                this._lineBalancingRates.Add(date);
            }

            var resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("PowerBI", sb.ToString(), out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable[] dt, ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            try
            {
                DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                using (sqlConn)
                {
                    for (int i = 0; i < dt.Length; i++)
                    {
                        var paramters = new List<SqlParameter>
                        {
                            new SqlParameter("@SDate", this._lineBalancingRates[i]),
                            new SqlParameter("@BIFactoryID", item.RgCode),
                            new SqlParameter("@IsTrans", item.IsTrans),
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
                            ,t.[BIFactoryID] = @BIFactoryID
                            ,t.[BIInsertDate] = GETDATE()
                            ,t.[BIStatus] = 'New'
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
                        ,[BIStatus] 
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
                        , @BIFactoryID
                        , GETDATE()
                        , 'New' 
	                    from #tmp t
	                    where not exists(
		                    select 1 from P_LineBalancingRate s
		                    where s.SewingDate = t.SewingDate
		                    and s.FactoryID = t.FactoryID
	                    )
                        drop table #tmp
                        ";
                        finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt[i], null, sqlcmd: sql, result: out DataTable dataTable2, temptablename: "#tmp", conn: sqlConn, paramters: paramters);
                    }
                }
            }
            catch (Exception ex)
            {
                finalResult.Result = new DualResult(false, ex);
            }

            return finalResult;
        }
    }
}

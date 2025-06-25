using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_MaterialLocationIndex
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_MaterialLocationIndex(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Now.AddMonths(-3);
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Now;
            }

            try
            {
                Base_ViewModel resultReport = this.GetMaterialLocationIndex_Data(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, item);
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
                new SqlParameter("@SDate", item.SDate),
                new SqlParameter("@EDate", item.EDate),
            };

            using (sqlConn)
            {
                string sql = new Base().SqlBITableHistory("P_MaterialLocationIndex", "P_MaterialLocationIndex_History", "#tmpFinal", "(CONVERT(date, p.AddDate) between @SDate and @EDate or CONVERT(date, p.EditDate) between @SDate and @EDate)", needJoin: false) + Environment.NewLine;
                sql += $@"
                delete t
	            from P_MaterialLocationIndex t
	            where not exists
                (
		            select 1 from #tmpFinal s
		            where s.ID = t.ID
		            and s.StockType = t.StockType
	            )
	            and 
                (
		            CONVERT(date, t.AddDate) between @SDate and @EDate
		            or
		            CONVERT(date, t.EditDate) between @SDate and @EDate
	            )

	            update t
	            set t.Junk = s.Junk
	            ,t.Description = s.Description
	            ,t.LocationType = s.LocationType
	            ,t.IsWMS = s.IsWMS
	            ,t.Capacity = s.Capacity
	            ,t.AddDate = s.AddDate
	            ,t.AddName = s.AddName
	            ,t.EditDate = s.EditDate
	            ,t.EditName = s.EditName
                ,t.BIFactoryID = s.BIFactoryID
                ,t.BIInsertDate = s.BIInsertDate
	            from P_MaterialLocationIndex t
	            inner join #tmpFinal s on s.ID = t.ID and s.StockType = t.StockType
	
	            insert into P_MaterialLocationIndex
                (
	                [ID]
                    ,[StockType]
                    ,[Junk]
                    ,[Description]
	                ,LocationType
                    ,[IsWMS]
                    ,[Capacity]
                    ,[AddName]
                    ,[AddDate]
                    ,[EditName]
                    ,[EditDate]
                    ,[BIFactoryID]
                    ,[BIInsertDate]
	            )
	            select
                    [ID]
                    ,[StockType]
                    ,[Junk]
                    ,[Description]
	                ,LocationType
                    ,[IsWMS]
                    ,[Capacity]
                    ,[AddName]
                    ,[AddDate]
                    ,[EditName]
                    ,[EditDate]
                    ,[BIFactoryID]
                    ,[BIInsertDate]
	            from #tmpFinal t
	            where not exists
                (
		            select 1 from P_MaterialLocationIndex s
		            where s.ID = t.ID
		            and s.StockType = t.StockType
	            )";
                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmpFinal");
            }

            return finalResult;
        }

        private Base_ViewModel GetMaterialLocationIndex_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@SDate", item.SDate),
                new SqlParameter("@EDate", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@"

			select [ID]
                ,[StockType]
                ,[Junk]
                ,[Description]
                ,[LocationType]
                ,[AddName]
                ,[AddDate]
                ,[EditName]
                ,[EditDate]
                ,[IsWMS]
                ,[Capacity]
                ,[BIFactoryID] = @BIFactoryID
                ,[BIInsertDate] = GETDATE()
	        from [MainServer].Production.dbo.MtlLocation
	        where 1=1
	        and 
            (
		        CONVERT(date, AddDate) between @SDate and @EDate
		        or
		        CONVERT(date, EditDate) between @SDate and @EDate
	        )";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, sqlParameters, out DataTable dt),
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

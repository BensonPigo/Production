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
    public class P_Import_StyleChangeover
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_StyleChangeover(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Now.AddMonths(-6);
            }

            try
            {
                Base_ViewModel resultReport = this.GetStyleChangeover_Data(item);
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
                new SqlParameter("@StartDate", item.SDate),
            };

            using (sqlConn)
            {
                string sql = $@" 
	            insert into P_StyleChangeover([ID], [FactoryID], [SewingLine], [Inline], [OldSP], [OldStyle], [OldComboType], [NewSP], [NewStyle], [NewComboType], [Category], [COPT(min)], [COT(min)], [BIFactoryID], [BIInsertDate])
	            select t.[ID], t.[Factory], t.[SewingLine], t.[Inline], t.[OldSP], t.[OldStyle], t.[OldComboType], t.[NewSP], t.[NewStyle], t.[NewComboType], t.[Category], t.[COPT(min)], t.[COT(min)], [BIFactoryID], [BIInsertDate]
	            from #tmp t
	            where not exists (select 1 from P_StyleChangeover p where p.[ID] = t.[ID])

	            update p
		            set p.[FactoryID] = t.[Factory]
			            , p.[SewingLine] = t.[SewingLine]
			            , p.[Inline] = t.[Inline]
			            , p.[OldSP] = t.[OldSP]
			            , p.[OldStyle] = t.[OldStyle]
			            , p.[OldComboType] = t.[OldComboType]
			            , p.[NewSP] = t.[NewSP]
			            , p.[NewStyle] = t.[NewStyle]
			            , p.[NewComboType] = t.[NewComboType]
			            , p.[Category] = t.[Category]
			            , p.[COPT(min)] = t.[COPT(min)]
			            , p.[COT(min)] = t.[COT(min)]
                        , p.[BIFactoryID]    = t.[BIFactoryID]
                        , p.[BIInsertDate]   = t.[BIInsertDate]
	            from P_StyleChangeover p
	            inner join #tmp t on p.[ID] = t.[ID]";

                sql += new Base().SqlBITableHistory("P_StyleChangeover", "P_StyleChangeover_History", "#tmp", "p.Inline >= @StartDate", needJoin: false) + Environment.NewLine;
                sql += $@"
                delete p
	             from P_StyleChangeover p
	             where not exists (select 1 from #tmp t where p.[ID] = t.[ID])
	             and p.Inline >= @StartDate";

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmp");
            }

            return finalResult;
        }

        private Base_ViewModel GetStyleChangeover_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@Date_S", item.SDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@"
		    select  
            [ID] = a.ID
			, [Factory] = isnull(a.FactoryID, '')
			, [SewingLine] = isnull(a.SewingLineID, '')
			, [Inline] = a.Inline
			, [OldSP] = isnull(b.OrderID, '')
			, [OldStyle] = isnull(b.StyleID, '')
			, [OldComboType] = isnull(b.ComboType, '')
			, [NewSP] = isnull(a.OrderID, '')
			, [NewStyle] = isnull(a.StyleID, '')
			, [NewComboType] = isnull(a.ComboType, '')
			, [Category] = isnull(a.Category, '')
			, [COPT(min)] = isnull(a.COPT, 0)
			, [COT(min)] = isnull(a.COT, 0)
            , [BIFactoryID] = @BIFactoryID
            , [BIInsertDate] = GETDATE()
		    from Production.[dbo].ChgOver a
		    outer apply
		    (
			    select top 1 b.OrderID,b.StyleID,b.ComboType
			    from Production.[dbo].ChgOver b
			    where b.Inline = (select max(c.Inline) from Production.[dbo].ChgOver c where c.FactoryID = a.FactoryID and c.SewingLineID = a.SewingLineID and c.Inline < a.Inline )
			    and b.FactoryID = a.FactoryID
			    and b.SewingLineID = a.SewingLineID
		    ) b
		    where a.Inline >= @Date_S
			";

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

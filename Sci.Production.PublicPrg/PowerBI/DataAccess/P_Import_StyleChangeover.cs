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
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_StyleChangeover
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_StyleChangeover(DateTime? sDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Now.AddMonths(-6);
            }

            try
            {
                Base_ViewModel resultReport = this.GetStyleChangeover_Data((DateTime)sDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, (DateTime)sDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }
                else
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_ImportScheduleList", false);
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sdate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>();
            lisSqlParameter.Add(new SqlParameter("@StartDate", sdate));

            using (sqlConn)
            {
                string sql = $@" 
	            insert into P_StyleChangeover([ID], [FactoryID], [SewingLine], [Inline], [OldSP], [OldStyle], [OldComboType], [NewSP], [NewStyle], [NewComboType], [Category], [COPT(min)], [COT(min)], [BIFactoryID], [BIInsertDate])
	            select t.[ID], t.[Factory], t.[SewingLine], t.[Inline], t.[OldSP], t.[OldStyle], t.[OldComboType], t.[NewSP], t.[NewStyle], t.[NewComboType], t.[Category], t.[COPT(min)], t.[COT(min)], t.[BIFactoryID], t.[BIInsertDate]
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

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmp");
            }

            finalResult.Result = result;

            return finalResult;
        }

        private Base_ViewModel GetStyleChangeover_Data(DateTime sdate)
        {
            string sqlcmd = $@"
            declare @Date_S date = '{sdate.ToString("yyyy/MM/dd")}'

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
            , [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
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
                Result = this.DBProxy.Select("Production", sqlcmd, out DataTable dataTables),
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

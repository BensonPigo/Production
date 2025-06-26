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
    internal class P_Import_ProdEfficiencyByFactorySewingLine
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_ProdEfficiencyByFactorySewingLine(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                var today = DateTime.Today;
                item.SDate = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            }

            try
            {
                Base_ViewModel resultReport = this.GetProdEfficiencyByFactorySewingLine_Data(item);
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
            string where = @"  p.[Year-Month] >= @StartDate";
            string tmp = new Base().SqlBITableHistory("P_ProdEfficiencyByFactorySewingLine", "P_ProdEfficiencyByFactorySewingLine_History", "#tmp", where, false, true);

            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@StartDate", item.SDate),
            };

            using (sqlConn)
            {
                string sql = $@" 
				insert into P_ProdEfficiencyByFactorySewingLine([Year-Month], FtyZone, Factory, Line, TotalQty, TotalCPU, TotalManhours, PPH, [EFF], BIFactoryID, BIInsertDate)
	            select t.[Year-Month], t.FtyZone, t.Factory, t.Line, t.TotalQty, t.TotalCPU, t.TotalManhours, t.PPH, t.[EFF], BIFactoryID, BIInsertDate
	            from #tmp t
	            where not exists (select 1 from P_ProdEfficiencyByFactorySewingLine p where p.[Year-Month] = t.[Year-Month] and p.FtyZone = t.FtyZone and p.Factory = t.Factory and p.Line = t.Line)

	            update p
		        set p.[TotalQty] = t.[TotalQty]
			        , p.[TotalCPU] = t.[TotalCPU]
			        , p.[TotalManhours] = t.[TotalManhours]
			        , p.[PPH] = t.[PPH]
			        , p.[EFF] = t.[EFF]
                    , p.[BIFactoryID] = t.BIFactoryID
                    , p.[BIInsertDate] = t.BIInsertDate
	            from P_ProdEfficiencyByFactorySewingLine p
	            inner join #tmp t on p.[Year-Month] = t.[Year-Month] and p.FtyZone = t.FtyZone and p.Factory = t.Factory and p.Line = t.Line

                {tmp}

	             delete p
	             from P_ProdEfficiencyByFactorySewingLine p
	             where not exists (select 1 from #tmp t where p.[Year-Month] = t.[Year-Month] and p.FtyZone = t.FtyZone and p.Factory = t.Factory and p.Line = t.Line)
	             and p.[Year-Month] >= @StartDate
                ";

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmp");
            }

            return finalResult;
        }

        private Base_ViewModel GetProdEfficiencyByFactorySewingLine_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@StartDate", item.SDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@"
			select  [Year-Month]
			, FtyZone 
			, [Factory] = FactoryID
			, [Line] = SewingLineID
			, [TotalQty] = TtlQty
			, [TotalCPU] = TtlCPU
			, [TotalManhours] = TtlManhour
			, [PPH] = IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2))
			, [EFF] = IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from  Production.dbo.System WITH (NOLOCK) ))*100, 2)) 
            , [BIFactoryID] = @BIFactoryID
            , [BIInsertDate] = GETDATE()   
	        from 
            (
		        select 
                FtyZone
				, FactoryID
				, [Year-Month]
				, SewingLineID
				, TtlCPU = Sum(ROUND( CPU*CPUFactor*Rate*QAQty,3))
				, TtlManhour = sum(ROUND( ActManPower * WorkHour, 2))
				, TtlQty = Sum(RateOutput) 
		        from 
                (
			        select o.ID
					        , [FtyZone] = f.FtyZone
					        , o.FactoryID
					        , o.CPU
					        , s.SewingLineID
					        , sd.WorkHour
					        , sd.QAQty
					        , o.CPUFactor
					        , Rate = isnull(Production.[dbo].[GetOrderLocation_Rate]( o.id ,sd.ComboType)/100,1) 
					        , s.OutputDate
					        , ActManPower= s.Manpower
					        , RateOutput = sd.QAQty  * isnull(Production.[dbo].[GetOrderLocation_Rate]( o.id ,sd.ComboType)/100,1) 
					        , [Year-Month] = CONVERT(date,dateadd(day ,-1, dateadd(m, datediff(m,0,s.OutputDate)+1,0)))
			        from Production.dbo.Orders o with(nolock)
			        inner join Production.dbo.SewingOutput_Detail sd with(nolock) on sd.OrderId = o.ID
			        inner join Production.dbo.SewingOutput s with(nolock) on s.ID = sd.ID
			        inner join Production.dbo.Factory f with(nolock) on f.ID = o.FactoryID
			        where o.Category = 'B'
			        and s.Shift <> 'O'
			        and ((o.LocalOrder = 1 and o.SubconInType in ('1','2')) or (o.LocalOrder = 0 and o.SubconInType = 0)) 
			        and s.OutputDate >= @StartDate
			        and f.Type <> 'S'
		        ) t
		        group by FtyZone, FactoryID, [Year-Month], SewingLineID
	        )t
	        Order by FtyZone, FactoryID, SewingLineID, [Year-Month]
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

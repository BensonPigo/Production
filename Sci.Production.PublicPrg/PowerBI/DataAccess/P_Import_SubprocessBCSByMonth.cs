using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SubprocessBCSByMonth
    {
        /// <inheritdoc/>
        public Base_ViewModel UpdateBIData(ExecutedList item)
        {
            string where = @"  not exists(
	select 1 from #tmpByMonth t
	where p.Factory = t.FactoryID
	and p.[Month] = t.[Month]
)";

            string tmp = new Base().SqlBITableHistory("P_SubprocessBCSByMonth", "P_SubprocessBCSByMonth_History", "#tmp", where, false, false);

            Base_ViewModel finalResult;
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@BIFactoryID", item.RgCode),
                new SqlParameter("@IsTrans", item.IsTrans),
            };
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = $@"	
-- P_SubprocessBCSByMonth
;with TTLBD as(
	select [Month] = format(SewingInline,'yyyyMM'),FactoryID
	,[TTLBundle] = count(1)
	from P_SubprocessWIP
	where SubprocessID='Loading'
	group by format(SewingInline,'yyyyMM'),FactoryID
)
, TTLLBD as(
	select [Month] = format(SewingInline,'yyyyMM'),FactoryID
	,[TTLLoadedBundle] =  sum(IIF(InTime is not null, 1,0))
	from P_SubprocessWIP
	where SubprocessID='Loading'
	group by format(SewingInline,'yyyyMM'),FactoryID
)
select a.[Month], a.FactoryID
, [SubprocessBCS] = 
	case when b.[TTLLoadedBundle] = 0 then 0
	else  ROUND(CONVERT(float,b.TTLLoadedBundle)/CONVERT(float,a.TTLBundle),4) * 100
	end
,[TTLLoadedBundle]
,[TTLBundle]
,[BIFactoryID] = @BIFactoryID
,[BIInsertDate] = GetDate()
into #tmpByMonth
from TTLBD a
inner join TTLLBD b on a.FactoryID = b.FactoryID and a.[Month] = b.[Month]

update  t
set t.SubprocessBCS = s.SubprocessBCS
,t.TTLBundle = s.TTLBundle
,t.TTLLoadedBundle = s.TTLLoadedBundle
,t.[BIFactoryID] = s.BIFactoryID
,t.[BIInsertDate] = s.BIInsertDate
from P_SubprocessBCSByMonth t
inner join #tmpByMonth s on t.Factory = s.FactoryID and t.[Month] = s.[Month]

insert P_SubprocessBCSByMonth([Month],Factory,SubprocessBCS,TTLBundle,TTLLoadedBundle, BIFactoryID, BIInsertDate)
select [MONTH],FactoryID,SubprocessBCS,TTLBundle,TTLLoadedBundle, BIFactoryID, BIInsertDate
from #tmpByMonth t
where not exists(
	select * from P_SubprocessBCSByMonth s
	where t.FactoryID = s.Factory
	and t.[Month] = s.[Month]
)
 
{tmp}

delete t
from P_SubprocessBCSByMonth t
where not exists(
	select 1 from #tmpByMonth s
	where t.Factory = s.FactoryID
	and t.[Month] = s.[Month]
)

drop table #tmpByMonth
";
                DBProxy.Current.DefaultTimeout = 1800;
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ExecuteTransactionScope("PowerBI", cmdtext: sql, parameters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

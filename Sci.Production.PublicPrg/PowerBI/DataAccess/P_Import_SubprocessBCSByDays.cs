using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SubprocessBCSByDays
    {
        /// <inheritdoc/>
        public Base_ViewModel UpdateBIData(ExecutedList item)
        {
            string where = @"  not exists(
	select 1 from #tmpByDays t
	where p.Factory = t.FactoryID
	and p.SewingInline = t.SewingInline
)";

            string tmp = new Base().SqlBITableHistory("P_SubprocessBCSByDays", "P_SubprocessBCSByDays_History", "#tmp", where, false, false);

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
-- P_SubprocessBCSByDays
with TTLBD as(
	select SewingInline,FactoryID
	,[TTLBundle] = count(1)
	from P_SubprocessWIP
	where SubprocessID='Loading'
	group by SewingInline,FactoryID
)
, TTLLBD as(
	select SewingInline,FactoryID	
	,[TTLLoadedBundle] =  sum(IIF(InTime is not null, 1,0))
	from P_SubprocessWIP
	where SubprocessID='Loading'
	group by SewingInline,FactoryID
)
select a.SewingInline, a.FactoryID
, [SubprocessBCS] = 
	case when b.[TTLLoadedBundle] = 0 then 0
	else  ROUND(CONVERT(float,b.TTLLoadedBundle)/CONVERT(float,a.TTLBundle),4)*100
	end
,[TTLLoadedBundle]
,[TTLBundle]
,[BIFactoryID] = @BIFactoryID
,[BIInsertDate] = GetDate()
into #tmpByDays
from TTLBD a
inner join TTLLBD b on a.FactoryID = b.FactoryID and a.SewingInline = b.SewingInline


update  t
set t.SubprocessBCS = s.SubprocessBCS
,t.TTLBundle = s.TTLBundle
,t.TTLLoadedBundle = s.TTLLoadedBundle
,t.BIFactoryID = s.BIFactoryID
,t.BIInsertDate = s.BIInsertDate
,t.BIStatus = 'New'
from P_SubprocessBCSByDays t
inner join #tmpByDays s on t.Factory = s.FactoryID and t.SewingInline = s.SewingInline

insert P_SubprocessBCSByDays(SewingInline,Factory,SubprocessBCS,TTLBundle,TTLLoadedBundle, BIFactoryID, BIInsertDate, BIStatus)
select SewingInline,FactoryID,SubprocessBCS,TTLBundle,TTLLoadedBundle, BIFactoryID, BIInsertDate, 'New'
from #tmpByDays t
where not exists(
	select * from P_SubprocessBCSByDays s
	where t.FactoryID = s.Factory
	and t.SewingInline = s.SewingInline
)

{tmp}

delete t
from P_SubprocessBCSByDays t
where not exists(
	select 1 from #tmpByDays s
	where t.Factory = s.FactoryID
	and t.SewingInline = s.SewingInline
) 

drop table #tmpByDays
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

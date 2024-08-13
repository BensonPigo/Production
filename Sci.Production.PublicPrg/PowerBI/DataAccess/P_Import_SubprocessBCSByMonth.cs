﻿using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SubprocessBCSByMonth
    {
        /// <inheritdoc/>
        public Base_ViewModel UpdateBIData()
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = @"	
-- P_SubprocessBCSByMonth
;with TTLBD as(
	select [Month] = format(SewingInline,'yyyyMM'),Factory
	,[TTLBundle] = count(1)
	from P_SubprocessWIP
	where SubprocessID='Loading'
	group by format(SewingInline,'yyyyMM'),Factory
)
, TTLLBD as(
	select [Month] = format(SewingInline,'yyyyMM'),Factory
	,[TTLLoadedBundle] =  sum(IIF(InTime is not null, 1,0))
	from P_SubprocessWIP
	where SubprocessID='Loading'
	group by format(SewingInline,'yyyyMM'),Factory
)
select a.[Month], a.Factory
, [SubprocessBCS] = 
	case when b.[TTLLoadedBundle] = 0 then 0
	else  ROUND(CONVERT(float,b.TTLLoadedBundle)/CONVERT(float,a.TTLBundle),4) * 100
	end
,[TTLLoadedBundle]
,[TTLBundle]
into #tmpByMonth
from TTLBD a
inner join TTLLBD b on a.Factory = b.Factory and a.[Month] = b.[Month]

update  t
set t.SubprocessBCS = s.SubprocessBCS
,t.TTLBundle = s.TTLBundle
,t.TTLLoadedBundle = s.TTLLoadedBundle
from P_SubprocessBCSByMonth t
inner join #tmpByMonth s on t.Factory = s.Factory and t.[Month] = s.[Month]

insert P_SubprocessBCSByMonth([Month],Factory,SubprocessBCS,TTLBundle,TTLLoadedBundle)
select [MONTH],Factory,SubprocessBCS,TTLBundle,TTLLoadedBundle
from #tmpByMonth t
where not exists(
	select * from P_SubprocessBCSByMonth s
	where t.Factory = s.Factory
	and t.[Month] = s.[Month]
)

delete t
from P_SubprocessBCSByMonth t
where not exists(
	select 1 from #tmpByMonth s
	where t.Factory = s.Factory
	and t.[Month] = s.[Month]
)

if exists (select 1 from BITableInfo b where b.id = 'P_SubprocessBCSByMonth')
begin
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_SubprocessBCSByMonth'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_SubprocessBCSByMonth', getdate())
end

drop table #tmpByMonth
";
                DBProxy.Current.DefaultTimeout = 1800;
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ExecuteTransactionScope("PowerBI", cmdtext: sql),
                };
            }

            return finalResult;
        }
    }
}

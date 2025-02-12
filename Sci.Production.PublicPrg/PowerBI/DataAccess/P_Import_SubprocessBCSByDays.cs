using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SubprocessBCSByDays
    {
        /// <inheritdoc/>
        public Base_ViewModel UpdateBIData()
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = @"	
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
into #tmpByDays
from TTLBD a
inner join TTLLBD b on a.FactoryID = b.FactoryID and a.SewingInline = b.SewingInline


update  t
set t.SubprocessBCS = s.SubprocessBCS
,t.TTLBundle = s.TTLBundle
,t.TTLLoadedBundle = s.TTLLoadedBundle
from P_SubprocessBCSByDays t
inner join #tmpByDays s on t.Factory = s.FactoryID and t.SewingInline = s.SewingInline

insert P_SubprocessBCSByDays(SewingInline,Factory,SubprocessBCS,TTLBundle,TTLLoadedBundle)
select SewingInline,FactoryID,SubprocessBCS,TTLBundle,TTLLoadedBundle
from #tmpByDays t
where not exists(
	select * from P_SubprocessBCSByDays s
	where t.FactoryID = s.Factory
	and t.SewingInline = s.SewingInline
)


delete t
from P_SubprocessBCSByDays t
where not exists(
	select 1 from #tmpByDays s
	where t.Factory = s.FactoryID
	and t.SewingInline = s.SewingInline
)


if exists (select 1 from BITableInfo b where b.id = 'P_SubprocessBCSByDays')
begin
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_SubprocessBCSByDays'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_SubprocessBCSByDays', getdate())
end

drop table #tmpByDays
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

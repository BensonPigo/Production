using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_OutStandingHPMS
    {
        /// <inheritdoc/>
        public Base_ViewModel P_OutStandingHPMS(DateTime? sDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
                if (!sDate.HasValue)
                {
                    sDate = DateTime.Now;
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(sDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DateTime? sDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", sDate),
            };
            using (sqlConn)
            {
                string sql = @"	
--declare @sDate as date = CONVERT(date, GETDATE()) 

select p.BuyerDelivery
	, [FactoryID] = f.FTYGroup
	, [OSTInHauling] = ISNULL(SUM(p2.OSTInHauling), 0)
	, [OSTInScanAndPack] = ISNULL(SUM(p3.OSTInScanAndPack), 0)
	, [OSTInCFA] = ISNULL(SUM(p4.OSTInCFA), 0)
into #tmp_P_OutStandingHPMS
from (
	select distinct p.BuyerDelivery, p.Fty 
	from P_CartonStatusTrackingList p 
	where p.BuyerDelivery >= @sDate
) p
inner join Production.dbo.Factory f WITH(NOLOCK) on p.fty = f.ID  
outer apply (
	select OSTInHauling = count(*)
	from P_CartonStatusTrackingList p2
	where p2.BuyerDelivery = p.BuyerDelivery
	and p2.Fty = p.fty
	and p2.Status = 'Hauling'	
) p2 
outer apply (
	select OSTInScanAndPack = count(*)
	from P_CartonStatusTrackingList  p3
	where p3.BuyerDelivery = p.BuyerDelivery
	and p3.Fty = p.fty
	and p3.Status = 'Scan & Pack'
) p3
outer apply (
	select OSTInCFA = count(*)
	from P_CartonStatusTrackingList  p4
	where p4.BuyerDelivery = p.BuyerDelivery
	and p4.Fty = p.fty
	and p4.Status = 'CFA'
) p4
GROUP BY p.BuyerDelivery, f.FTYGroup

update p
	set p.[OSTInHauling] = t.[OSTInHauling]
		, p.[OSTInScanAndPack] = t.[OSTInScanAndPack]
		, p.[OSTInCFA] = t.[OSTInCFA]
from P_OutStandingHPMS p
inner join #tmp_P_OutStandingHPMS t on p.[BuyerDelivery] = t.[BuyerDelivery] and p.[FactoryID] = t.[FactoryID]

insert into P_OutStandingHPMS([BuyerDelivery], [FactoryID], [OSTInHauling], [OSTInScanAndPack], [OSTInCFA])
select [BuyerDelivery], [FactoryID], [OSTInHauling], [OSTInScanAndPack], [OSTInCFA]
from #tmp_P_OutStandingHPMS t
where not exists (select 1 from P_OutStandingHPMS p where p.[BuyerDelivery] = t.[BuyerDelivery] and p.[FactoryID] = t.[FactoryID])

delete p
from P_OutStandingHPMS p
where p.[BuyerDelivery] < @sDate

if exists (select 1 from BITableInfo b where b.id = 'P_OutStandingHPMS')
begin
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_OutStandingHPMS'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_OutStandingHPMS', getdate())
end
";
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ExecuteByConnTransactionScope(conn: sqlConn, cmdtext: sql, parameters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

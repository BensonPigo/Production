using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_OutStandingHPMS
    {
        /// <inheritdoc/>
        public Base_ViewModel P_OutStandingHPMS(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
                if (!item.SDate.HasValue)
                {
                    item.SDate = DateTime.Now;
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(item);
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

        private Base_ViewModel UpdateBIData(ExecutedList item)
        {
            string where = @" p.[BuyerDelivery] < @sDate ";

            string tmp = new Base().SqlBITableHistory("P_OutStandingHPMS", "P_OutStandingHPMS_History", "#tmp", where, false, false);

            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", item.SDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
                new SqlParameter("@IsTrans", item.IsTrans),
            };
            using (sqlConn)
            {
                string sql = $@"	
--declare @sDate as date = CONVERT(date, GETDATE()) 

select p.BuyerDelivery
	, [FactoryID] = f.FTYGroup
	, [OSTInHauling] = ISNULL(SUM(p2.OSTInHauling), 0)
	, [OSTInScanAndPack] = ISNULL(SUM(p3.OSTInScanAndPack), 0)
	, [OSTInCFA] = ISNULL(SUM(p4.OSTInCFA), 0)
    , [BIFactoryID] = @BIFactoryID
    , [BIInsertDate] = GETDATE()
into #tmp_P_OutStandingHPMS
from (
	select distinct p.BuyerDelivery, p.FactoryID 
	from P_CartonStatusTrackingList p 
	where p.BuyerDelivery >= @sDate
) p
inner join Production.dbo.Factory f WITH(NOLOCK) on p.FactoryID = f.ID  
outer apply (
	select OSTInHauling = count(*)
	from P_CartonStatusTrackingList p2
	where p2.BuyerDelivery = p.BuyerDelivery
	and p2.FactoryID = p.FactoryID
	and p2.Status = 'Hauling'	
) p2 
outer apply (
	select OSTInScanAndPack = count(*)
	from P_CartonStatusTrackingList  p3
	where p3.BuyerDelivery = p.BuyerDelivery
	and p3.FactoryID = p.FactoryID
	and p3.Status = 'Scan & Pack'
) p3
outer apply (
	select OSTInCFA = count(*)
	from P_CartonStatusTrackingList  p4
	where p4.BuyerDelivery = p.BuyerDelivery
	and p4.FactoryID = p.FactoryID
	and p4.Status = 'CFA'
) p4
GROUP BY p.BuyerDelivery, f.FTYGroup

update p
	set p.[OSTInHauling] = t.[OSTInHauling]
		, p.[OSTInScanAndPack] = t.[OSTInScanAndPack]
		, p.[OSTInCFA] = t.[OSTInCFA]
        , p.[BIFactoryID] = t.[BIFactoryID]
        , p.[BIInsertDate] = t.[BIInsertDate]
        , p.[BIStatus] = 'New'
from P_OutStandingHPMS p
inner join #tmp_P_OutStandingHPMS t on p.[BuyerDelivery] = t.[BuyerDelivery] and p.[FactoryID] = t.[FactoryID]

insert into P_OutStandingHPMS([BuyerDelivery], [FactoryID], [OSTInHauling], [OSTInScanAndPack], [OSTInCFA] , [BIFactoryID], [BIInsertDate], [BIStatus])
select [BuyerDelivery], [FactoryID], [OSTInHauling], [OSTInScanAndPack], [OSTInCFA] , [BIFactoryID], [BIInsertDate], 'New'
from #tmp_P_OutStandingHPMS t
where not exists (select 1 from P_OutStandingHPMS p where p.[BuyerDelivery] = t.[BuyerDelivery] and p.[FactoryID] = t.[FactoryID])

{tmp}

delete p
from P_OutStandingHPMS p
where p.[BuyerDelivery] < @sDate
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

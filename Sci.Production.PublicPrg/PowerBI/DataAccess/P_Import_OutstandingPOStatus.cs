using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_OutstandingPOStatus
    {
        /// <inheritdoc/>
        public Base_ViewModel P_OutstandingPOStatus(ExecutedList item)
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
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", item.SDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
                new SqlParameter("@IsTrans", item.IsTrans),
            };

            string where = @" p.Buyerdelivery < @sDate";
            string tmp = new Base().SqlBITableHistory("P_OutstandingPOStatus", "P_OutstandingPOStatus_History", "#tmp", where, false, false);

            using (sqlConn)
            {
                string sql = $@"	
--declare @sDate as date = CONVERT(date, GETDATE()) 

select p.Buyerdelivery
	, f.FTYGroup
	, [TotalCMPQty] = SUM(cast(p.OSTCMPQty as int))
	, [TotalClogCtn] = SUM(p.OSTClogCtn)
	, [NotYet3rdSPCount] = SUM(IIF(p.[3rdPartyInspection] = 'Y' and p.[3rdPartyInspectionResult] = '', 1, 0))
    , [BIFactoryID] = @BIFactoryID
    , [BIInsertDate] = GETDATE()
into #tmp
from P_OustandingPO p
inner join Production.dbo.Factory f on p.FactoryID = f.ID
where p.BuyerDelivery >= @sDate
group by p.Buyerdelivery, f.FTYGroup

{tmp}

delete p 
from P_OutstandingPOStatus p
where p.Buyerdelivery < @sDate

insert into P_OutstandingPOStatus([Buyerdelivery], [FTYGroup], [TotalCMPQty], [TotalClogCtn], [NotYet3rdSPCount], [BIFactoryID], [BIInsertDate])
select [Buyerdelivery], [FTYGroup], [TotalCMPQty], [TotalClogCtn], [NotYet3rdSPCount], [BIFactoryID], [BIInsertDate]
from #tmp t
where not exists (select 1 from P_OutstandingPOStatus p where t.BuyerDelivery = p.Buyerdelivery and t.FTYGroup = p.FTYGroup) 

update p	
	set p.TotalCMPQty = t.TotalCMPQty
	 , p.TotalClogCtn = t.TotalClogCtn
	 , p.NotYet3rdSPCount = t.NotYet3rdSPCount
     , p.[BIFactoryID] = t.[BIFactoryID]
     , p.[BIInsertDate] =  t.[BIInsertDate]
from P_OutstandingPOStatus p
inner join #tmp t on t.BuyerDelivery = p.Buyerdelivery and t.FTYGroup = p.FTYGroup

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

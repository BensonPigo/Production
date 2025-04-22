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
    public class P_Import_OutstandingPOStatus
    {
        /// <inheritdoc/>
        public Base_ViewModel P_OutstandingPOStatus(DateTime? sDate)
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
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", sDate),
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
    , [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
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

if exists (select 1 from BITableInfo b where b.id = 'P_OutstandingPOStatus')
begin
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_OutstandingPOStatus'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_OutstandingPOStatus', getdate())
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

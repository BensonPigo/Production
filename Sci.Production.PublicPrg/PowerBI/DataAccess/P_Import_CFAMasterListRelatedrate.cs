using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_CFAMasterListRelatedrate
    {
        /// <inheritdoc/>
        public Base_ViewModel P_CFAMasterListRelatedrate(DateTime? sDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
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
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@BuyerDelivery", sDate),
                };
                string sql = @"
--declare @BuyerDelivery as date = dateadd(day, -6,getdate())

select *
	, [FinalRate] = cast(iif(p.[TotalSP] = 0, 0, p.[FinalInspectionSP] * 1.0 / p.[TotalSP]) * 100 as decimal(5, 2))
	, [PassRate] = cast(iif(p.[FinalInspectionSP] = 0, 0, p.[PassSP] * 1.0 / p.[FinalInspectionSP]) * 100 as decimal(5, 2))
into #tmp_P_CFAMasterListRelatedrate
from (
	select p.BuyerDelivery
		, [FactoryID] = o.FtyGroup
		, [FinalInspectionSP] = sum(isnull(p2.FinalInspectionSP, 0))
		, [TotalSP] = sum(isnull(p3.TotalSP, 0))
		, [PassSP] = sum(isnull(p4.PassSPQty, 0))
	from P_QA_CFAMasterList p with(nolock)
	inner join [MainServer].[Production].[dbo].Orders o on p.OrderID = o.ID
	left join (
		select p.OrderID, [FinalInspectionSP] = count(*)
		from P_QA_CFAMasterList p with(nolock)
		where p.BuyerDelivery = @BuyerDelivery
		and p.FinalInspDate is not null
		group by p.OrderID
	)p2 on p.OrderID = p.OrderID
	left join (
		select p.OrderID, [TotalSP] = count(*)
		from P_QA_CFAMasterList p with(nolock)
		where p.BuyerDelivery = @BuyerDelivery
		group by p.OrderID
	)p3 on p.OrderID = p3.OrderID
	left join (
		select p.OrderID, [PassSPQty] = count(*)
		from P_QA_CFAMasterList p with(nolock)
		where p.BuyerDelivery = @BuyerDelivery
		AND p.FinalInspDate is not null
		AND p.FinalInsp='Pass'
		group by p.OrderID
	)p4 on p.OrderID = p4.OrderID
	where p.BuyerDelivery = @BuyerDelivery
	group by p.BuyerDelivery, o.FtyGroup
) p

update p
	set p.[FinalRate] = t.[FinalRate]
		, p.[FinalInspectionSP] = t.[FinalInspectionSP]
		, p.[TotalSP] = t.[TotalSP]
		, p.[PassRate] = t.[PassRate]
		, p.[PassSP] = t.[PassSP]
from P_CFAMasterListRelatedrate p
inner join #tmp_P_CFAMasterListRelatedrate t on p.[BuyerDelivery]= t.[BuyerDelivery] and p.[FactoryID] = t.[FactoryID]

insert into P_CFAMasterListRelatedrate([Buyerdelivery], [FactoryID], [FinalRate], [FinalInspectionSP], [TotalSP], [PassRate], [PassSP])
select [Buyerdelivery], [FactoryID], [FinalRate], [FinalInspectionSP], [TotalSP], [PassRate], [PassSP]
from #tmp_P_CFAMasterListRelatedrate t
where not exists (select 1 from P_CFAMasterListRelatedrate p where p.[Buyerdelivery]= t.[Buyerdelivery] and p.[FactoryID] = t.[FactoryID])

if exists (select 1 from BITableInfo b where b.id = 'P_CFAMasterListRelatedrate')
begin
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_CFAMasterListRelatedrate'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_CFAMasterListRelatedrate', getdate())
end
";
                finalResult = new Base_ViewModel()
                {
                    Result = Data.DBProxy.Current.ExecuteByConn(conn: sqlConn, cmdtext: sql, parameters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

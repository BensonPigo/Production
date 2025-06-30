using Sci.Production.Prg.PowerBI.Logic;
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
        public Base_ViewModel P_CFAMasterListRelatedrate(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
                if (!item.SDate.HasValue)
                {
                    item.SDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                }

                if (!item.EDate.HasValue)
                {
                    item.EDate = DateTime.Parse(DateTime.Now.AddDays(8).ToString("yyyy/MM/dd"));
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
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@BuyerDeliveryS", item.SDate),
                    new SqlParameter("@BuyerDeliveryE", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };

                string tmp = new Base().SqlBITableHistory("P_CFAMasterListRelatedrate", "P_CFAMasterListRelatedrate_History", "#tmp_P_CFAMasterListRelatedrate", "p.[Buyerdelivery] <= cast(dateadd(day, -1, getdate()) as date)");

                string sql = $@"
--declare @BuyerDeliveryS as date = getdate()
--declare @BuyerDeliveryE as date = dateadd(day, 7, getdate())

select *
	, [FinalRate] = cast(iif(p.[TotalSP] = 0, 0, p.[FinalInspectionSP] * 1.0 / p.[TotalSP]) * 100 as decimal(5, 2))
	, [PassRate] = cast(iif(p.[FinalInspectionSP] = 0, 0, p.[PassSP] * 1.0 / p.[FinalInspectionSP]) * 100 as decimal(5, 2))
    , [BIFactoryID] = @BIFactoryID
    , [BIInsertDate] = GETDATE()
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
		select p.OrderID, p.BuyerDelivery, [FinalInspectionSP] = count(*)
		from P_QA_CFAMasterList p with(nolock)
		where p.BuyerDelivery >= @BuyerDeliveryS
		and p.FinalInspDate is not null
		group by p.OrderID, p.BuyerDelivery
	)p2 on p.OrderID = p2.OrderID and p.BuyerDelivery = p2.BuyerDelivery
	left join (
		select p.OrderID, p.BuyerDelivery, [TotalSP] = count(*)
		from P_QA_CFAMasterList p with(nolock)
		where p.BuyerDelivery >= @BuyerDeliveryS
		group by p.OrderID, p.BuyerDelivery
	)p3 on p.OrderID = p3.OrderID and p.BuyerDelivery = p3.BuyerDelivery
	left join (
		select p.OrderID, p.BuyerDelivery, [PassSPQty] = count(*)
		from P_QA_CFAMasterList p with(nolock)
		where p.BuyerDelivery >= @BuyerDeliveryS
		AND p.FinalInspDate is not null
		AND p.FinalInsp='Pass'
		group by p.OrderID, p.BuyerDelivery
	)p4 on p.OrderID = p4.OrderID and p.BuyerDelivery = p4.BuyerDelivery
	where p.BuyerDelivery >= @BuyerDeliveryS
    and p.BuyerDelivery <= @BuyerDeliveryE
	group by p.BuyerDelivery, o.FtyGroup
) p

update p
	set p.[FinalRate] = t.[FinalRate]
		, p.[FinalInspectionSP] = t.[FinalInspectionSP]
		, p.[TotalSP] = t.[TotalSP]
		, p.[PassRate] = t.[PassRate]
		, p.[PassSP] = t.[PassSP]
        , p.[BIFactoryID] = t.[BIFactoryID]
        , p.[BIInsertDate] = t.[BIInsertDate]
from P_CFAMasterListRelatedrate p
inner join #tmp_P_CFAMasterListRelatedrate t on p.[BuyerDelivery]= t.[BuyerDelivery] and p.[FactoryID] = t.[FactoryID]

insert into P_CFAMasterListRelatedrate([Buyerdelivery], [FactoryID], [FinalRate], [FinalInspectionSP], [TotalSP], [PassRate], [PassSP], [BIFactoryID], [BIInsertDate])
select [Buyerdelivery], [FactoryID], [FinalRate], [FinalInspectionSP], [TotalSP], [PassRate], [PassSP], [BIFactoryID], [BIInsertDate]
from #tmp_P_CFAMasterListRelatedrate t
where not exists (select 1 from P_CFAMasterListRelatedrate p where p.[Buyerdelivery] = t.[Buyerdelivery] and p.[FactoryID] = t.[FactoryID])

{tmp}

delete p
from P_CFAMasterListRelatedrate p
where p.[Buyerdelivery] <= cast(dateadd(day, -1, getdate()) as date)
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

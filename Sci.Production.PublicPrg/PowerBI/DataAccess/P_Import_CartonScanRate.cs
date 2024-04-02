using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_CartonScanRate
    {
        /// <inheritdoc/>
        public Base_ViewModel P_CartonScanRate()
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
                // insert into PowerBI
                finalResult = this.UpdateBIData();
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

        private Base_ViewModel UpdateBIData()
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = @"	
select p.BuyerDelivery
	, p.FTYGroup
	, [HaulingScanRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.HauledQty * 1.0 / p.TotalCartonQty) * 100) as decimal(5, 2))
	, [PackingAuditScanRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.PackingAuditQty * 1.0 / p.TotalCartonQty) * 100) as decimal(5,2))
	, [MDScanRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.MDScanQty * 1.0 / p.TotalCartonQty) * 100) as decimal(5, 2))
	, [ScanAndPackRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.ScanAndPackQty * 1.0 / p.TotalCartonQty) * 100) as decimal(5, 2))
	, [PullOutRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.SPCountWithPulloutCmplt * 1.0 / p.TotalCartonQty) * 100) as decimal(5,2))
	, [ClogReceivedRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.ClogReceivedQty * 1.0 / p.TotalCartonQty) * 100) as decimal(5,2))
into #tmp_P_CartonScanRate
from (
	select [SPCount] = count(distinct p.SP)
		, [SPCountWithPulloutCmplt] = isnull(p2.[SPCountWithPulloutCmplt], 0)
		, [HauledQty] = Sum(p.HauledQty)
		, [PackingAuditQty] = Sum(IIF(p.PackingAuditScanTime is null, 0, p.CartonQty))
		, [MDScanQty] = Sum(IIF(p.MDScanTime is null, 0, p.CartonQty))
		, [ClogReceivedQty] = isnull([CartonQtyWithClogReceiveTime], 0)
		, [ScanAndPackQty] = Sum(p.ScanQty)
		, [TotalCartonQty] = Sum(p.CartonQty)
		, f.FTYGroup
		, p.BuyerDelivery
	from P_CartonStatusTrackingList p WITH(NOLOCK)
	inner join [MainServer].Production.dbo.Factory f WITH(NOLOCK) on p.fty = f.ID
	left join (
		select [SPCountWithPulloutCmplt] = count(distinct p.SP)
			, f.FTYGroup, p.BuyerDelivery
		from P_CartonStatusTrackingList p WITH(NOLOCK)
		inner join [MainServer].Production.dbo.Factory f WITH(NOLOCK) on p.fty = f.ID
		where p.BuyerDelivery >= CONVERT(date, GETDATE()) 
		and p.BuyerDelivery <= DATEADD(DAY ,7,CONVERT(date, GETDATE())) 
		and p.PulloutComplete = 'Y'
		group by f.FTYGroup, p.BuyerDelivery
	) p2 on f.FTYGroup = p2.FTYGroup and p.BuyerDelivery = p2.BuyerDelivery
	left join (
		select [CartonQtyWithClogReceiveTime] = Sum(p.CartonQty)
			, f.FTYGroup, p.BuyerDelivery
		from P_CartonStatusTrackingList p WITH(NOLOCK)
		inner join [MainServer].Production.dbo.Factory f WITH(NOLOCK) on p.fty = f.ID
		where p.BuyerDelivery >= CONVERT(date, GETDATE()) 
		and p.BuyerDelivery <= DATEADD(DAY ,7,CONVERT(date, GETDATE())) 
		and p.ClogReceiveTime is not null
		group by f.FTYGroup, p.BuyerDelivery
	)p3 on f.FTYGroup = p3.FTYGroup and p.BuyerDelivery = p3.BuyerDelivery
	where p.BuyerDelivery >= CONVERT(date, GETDATE()) 
	and p.BuyerDelivery <= DATEADD(DAY ,7,CONVERT(date, GETDATE())) 
	group by f.FTYGroup, p.BuyerDelivery, p2.[SPCountWithPulloutCmplt], p3.[CartonQtyWithClogReceiveTime]
) p


update p
	set p.[HaulingScanRate] = t.[HaulingScanRate]
		, p.[PackingAuditScanRate] = t.[PackingAuditScanRate]
		, p.[MDScanRate] = t.[MDScanRate]
		, p.[ScanAndPackRate] = t.[ScanAndPackRate]
		, p.[PullOutRate] = t.[PullOutRate]
		, p.[ClogReceivedRate] = t.[ClogReceivedRate]
from P_CartonScanRate p
inner join #tmp_P_CartonScanRate t on p.[Date]= t.[BuyerDelivery] and p.[FactoryID] = t.[FTYGroup]


insert into P_CartonScanRate([Date], [FactoryID], [HaulingScanRate], [PackingAuditScanRate], [MDScanRate], [ScanAndPackRate], [PullOutRate], [ClogReceivedRate])
select [BuyerDelivery], [FTYGroup], [HaulingScanRate], [PackingAuditScanRate], [MDScanRate], [ScanAndPackRate], [PullOutRate], [ClogReceivedRate]
from #tmp_P_CartonScanRate t
where not exists (select 1 from P_CartonScanRate p where p.[Date]= t.[BuyerDelivery] and p.[FactoryID] = t.[FTYGroup])

if exists (select 1 from BITableInfo b where b.id = 'P_CartonScanRate')
begin
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_CartonScanRate'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_CartonScanRate', getdate())
end
";
                finalResult = new Base_ViewModel()
                {
                    Result = Data.DBProxy.Current.ExecuteByConn(conn: sqlConn, cmdtext: sql),
                };
            }

            return finalResult;
        }
    }
}

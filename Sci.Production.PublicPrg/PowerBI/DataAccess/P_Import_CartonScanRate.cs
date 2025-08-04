using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_CartonScanRate
    {
        /// <inheritdoc/>
        public Base_ViewModel P_CartonScanRate(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
                if (!item.SDate.HasValue)
                {
                    item.SDate = DateTime.Now;
                }

                if (!item.EDate.HasValue)
                {
                    item.EDate = DateTime.Now.AddDays(7);
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
                new SqlParameter("@eDate", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
                new SqlParameter("@IsTrans", item.IsTrans),
            };
            using (sqlConn)
            {
                string sql = $@"	
--declare @sDate as date = CONVERT(date, GETDATE()) 
--	, @eDate as date = DATEADD(DAY ,7,CONVERT(date, GETDATE())) 

select [Date] = p.BuyerDelivery
	, [FactoryID] = p.FTYGroup
	, [HaulingScanRate] = cast(iif(isnull(p.TotalCartonPieces, 0) = 0, 0, (p.HauledPieces * 1.0 / p.TotalCartonPieces) * 100) as decimal(5, 2))
	, [PackingAuditScanRate] = cast(iif(isnull(p.TotalCartonPieces, 0) = 0, 0, (p.PackingAuditPieces * 1.0 / p.TotalCartonPieces) * 100) as decimal(5,2))
	, [MDScanRate] = cast(iif(isnull(p.TotalCartonPieces, 0) = 0, 0, (p.MDScanPieces * 1.0 / p.TotalCartonPieces) * 100) as decimal(5, 2))
	, [ScanAndPackRate] = cast(iif(isnull(p.TotalCartonPieces, 0) = 0, 0, (p.ScanAndPackPieces * 1.0 / p.TotalCartonPieces) * 100) as decimal(5, 2))
	, [PullOutRate] = cast(iif(isnull(p.SPCount, 0) = 0, 0, (p.SPCountWithPulloutCmplt * 1.0 / p.SPCount) * 100) as decimal(5,2))
	, [ClogReceivedRate] = cast(iif(isnull(p.TotalCartonPieces, 0) = 0, 0, (p.ClogReceivedPieces * 1.0 / p.TotalCartonPieces) * 100) as decimal(5,2))
    , [BIFactoryID] = @BIFactoryID
    , [BIInsertDate] = GETDATE()
into #tmp_P_CartonScanRate
from (
	select [SPCount] = count(distinct p.SP)
		, [SPCountWithPulloutCmplt] = isnull(p2.[SPCountWithPulloutCmplt], 0)
		, [HauledPieces] = isnull(p_Hauling.PiecesQty, 0)
		, [PackingAuditPieces] = isnull(p_PackingAuditScanTime.PiecesQty, 0)
		, [MDScanPieces] = isnull(p_MDScanTime.PiecesQty, 0)
		, [ClogReceivedPieces] = isnull(p_ClogReceiveTime.PiecesQty, 0)
		, [ScanAndPackPieces] = isnull(p_ScanAndPackTime.PiecesQty, 0)
		, [TotalCartonQty] = Sum(p.CartonQty)
		, [TotalCartonPieces] = count(*)
		, f.FTYGroup
		, p.BuyerDelivery
	from P_CartonStatusTrackingList p WITH(NOLOCK)
	inner join Production.dbo.Factory f WITH(NOLOCK) on p.FactoryID = f.ID and f.IsProduceFty = 1
	left join (
		select [SPCountWithPulloutCmplt] = count(distinct p.SP)
			, f.FTYGroup, p.BuyerDelivery
		from P_CartonStatusTrackingList p WITH(NOLOCK)
		inner join Production.dbo.Factory f WITH(NOLOCK) on p.FactoryID = f.ID and f.IsProduceFty = 1
		where p.BuyerDelivery >= @sDate
		and p.BuyerDelivery <= @eDate
		and p.PulloutComplete = 'Y'
		group by f.FTYGroup, p.BuyerDelivery
	) p2 on f.FTYGroup = p2.FTYGroup and p.BuyerDelivery = p2.BuyerDelivery
	left join (
		select [PiecesQty] = count(*)
			, f.FTYGroup, p.BuyerDelivery
		from P_CartonStatusTrackingList p WITH(NOLOCK)
		inner join Production.dbo.Factory f WITH(NOLOCK) on p.FactoryID = f.ID and f.IsProduceFty = 1
		where p.BuyerDelivery >= @sDate 
		and p.BuyerDelivery <= @eDate
		and p.ClogReceiveTime is not null
		group by f.FTYGroup, p.BuyerDelivery
	)p_ClogReceiveTime on f.FTYGroup = p_ClogReceiveTime.FTYGroup and p.BuyerDelivery = p_ClogReceiveTime.BuyerDelivery
	left join (
		select [PiecesQty] = count(*)
			, f.FTYGroup, p.BuyerDelivery
		from P_CartonStatusTrackingList p WITH(NOLOCK)
		inner join Production.dbo.Factory f WITH(NOLOCK) on p.FactoryID = f.ID and f.IsProduceFty = 1
		where p.BuyerDelivery >= @sDate 
		and p.BuyerDelivery <= @eDate
		and p.HaulingScanTime is not null
		group by f.FTYGroup, p.BuyerDelivery
	)p_Hauling on f.FTYGroup = p_Hauling.FTYGroup and p.BuyerDelivery = p_Hauling.BuyerDelivery
	left join (
		select [PiecesQty] = count(*)
			, f.FTYGroup, p.BuyerDelivery
		from P_CartonStatusTrackingList p WITH(NOLOCK)
		inner join Production.dbo.Factory f WITH(NOLOCK) on p.FactoryID = f.ID and f.IsProduceFty = 1
		where p.BuyerDelivery >= @sDate
		and p.BuyerDelivery <= @eDate
		and p.PackingAuditScanTime is not null
		group by f.FTYGroup, p.BuyerDelivery
	)p_PackingAuditScanTime on f.FTYGroup = p_PackingAuditScanTime.FTYGroup and p.BuyerDelivery = p_PackingAuditScanTime.BuyerDelivery
	left join (
		select [PiecesQty] = count(*)
			, f.FTYGroup, p.BuyerDelivery
		from P_CartonStatusTrackingList p WITH(NOLOCK)
		inner join Production.dbo.Factory f WITH(NOLOCK) on p.FactoryID = f.ID and f.IsProduceFty = 1
		where p.BuyerDelivery >= @sDate
		and p.BuyerDelivery <= @eDate
		and p.MDScanTime is not null
		group by f.FTYGroup, p.BuyerDelivery
	)p_MDScanTime on f.FTYGroup = p_MDScanTime.FTYGroup and p.BuyerDelivery = p_MDScanTime.BuyerDelivery
	left join (
		select [PiecesQty] = count(*)
			, f.FTYGroup, p.BuyerDelivery
		from P_CartonStatusTrackingList p WITH(NOLOCK)
		inner join Production.dbo.Factory f WITH(NOLOCK) on p.FactoryID = f.ID and f.IsProduceFty = 1
		where p.BuyerDelivery >= @sDate 
		and p.BuyerDelivery <= @eDate
		and p.ScanAndPackTime is not null
		group by f.FTYGroup, p.BuyerDelivery
	)p_ScanAndPackTime on f.FTYGroup = p_ScanAndPackTime.FTYGroup and p.BuyerDelivery = p_ScanAndPackTime.BuyerDelivery
	where p.BuyerDelivery >= @sDate 
	and p.BuyerDelivery <= @eDate
	group by f.FTYGroup, p.BuyerDelivery, p2.[SPCountWithPulloutCmplt], p_ClogReceiveTime.PiecesQty, p_Hauling.PiecesQty, p_PackingAuditScanTime.PiecesQty, p_MDScanTime.PiecesQty, p_ScanAndPackTime.PiecesQty
) p

update p
	set p.[HaulingScanRate] = t.[HaulingScanRate]
		, p.[PackingAuditScanRate] = t.[PackingAuditScanRate]
		, p.[MDScanRate] = t.[MDScanRate]
		, p.[ScanAndPackRate] = t.[ScanAndPackRate]
		, p.[PullOutRate] = t.[PullOutRate]
		, p.[ClogReceivedRate] = t.[ClogReceivedRate]
		, p.[BIFactoryID] = t.[BIFactoryID]
		, p.[BIInsertDate] = t.[BIInsertDate]
		, p.[BIStatus] = 'New'
from P_CartonScanRate p
inner join #tmp_P_CartonScanRate t on p.[Date]= t.[Date] and p.[FactoryID] = t.[FactoryID]


insert into P_CartonScanRate([Date], [FactoryID], [HaulingScanRate], [PackingAuditScanRate], [MDScanRate], [ScanAndPackRate], [PullOutRate], [ClogReceivedRate], [BIFactoryID], [BIInsertDate], [BIStatus])
select [Date], [FactoryID], [HaulingScanRate], [PackingAuditScanRate], [MDScanRate], [ScanAndPackRate], [PullOutRate], [ClogReceivedRate], [BIFactoryID], [BIInsertDate], 'New'
from #tmp_P_CartonScanRate t
where not exists (select 1 from P_CartonScanRate p where p.[Date]= t.[Date] and p.[FactoryID] = t.[FactoryID])
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

using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_CartonStatusTrackingList
    {
        /// <inheritdoc/>
        public Base_ViewModel P_CartonStatusTrackingList(DateTime? sDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R21 biModel = new PPIC_R21();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd"));
            }

            try
            {
                PPIC_R21_ViewModel model = new PPIC_R21_ViewModel()
                {
                    BuyerDeliveryFrom = sDate.Value,
                    BuyerDeliveryTo = null,
                    DateTimeProcessFrom = null,
                    DateTimeProcessTo = null,
                    ComboProcess = string.Empty,
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                    OrderID = string.Empty,
                    PO = string.Empty,
                    PackID = string.Empty,
                    SCIDeliveryFrom = null,
                    SCIDeliveryTo = null,
                    IsPPICP26 = false,
                    ExcludeSisterTransferOut = false,
                    IsBI = true,
                };

                Base_ViewModel resultReport = biModel.GetCartonStatusTrackingList(model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt, sDate.Value);
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

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", sDate),
                };
                string sql = @"	
-- 先寫入 Histroy
insert into P_CartonStatusTrackingList_History(FactoryID,SP,SeqNo,PackingListID,CtnNo,BIFactoryID,BIInsertDate)
select p.[FactoryID],p.[SP], p.[SeqNo], p.[PackingListID], p.[CtnNo], p.[BIFactoryID], getdate()
from P_CartonStatusTrackingList p
where not exists (select 1 from #tmp t where p.[SP] = t.[SP] and p.[SeqNo] = t.[SeqNo] and p.[PackingListID] = t.[PackingListID] and p.[CtnNo] = t.[CtnNo])

insert into P_CartonStatusTrackingList([KPIGroup], [FactoryID], [Line], [SP], [SeqNo], [Category], [Brand], [Style], [PONO], [Season], [Destination], [SCIDelivery], [BuyerDelivery], [PackingListID], [CtnNo], [Size], [CartonQty], [Status], [HaulingScanTime], [HauledQty], [DryRoomReceiveTime], [DryRoomTransferTime], [MDScanTime], [MDFailQty], [PackingAuditScanTime], [PackingAuditFailQty], [M360MDScanTime], [M360MDFailQty], [TransferToPackingErrorTime], [ConfirmPackingErrorReviseTime], [ScanAndPackTime], [ScanQty], [FtyTransferToClogTime], [ClogReceiveTime], [ClogLocation], [ClogReturnTime], [ClogTransferToCFATime], [CFAReceiveTime], [CFAReturnTime], [CFAReturnDestination], [ClogReceiveFromCFATime], [DisposeDate], [PulloutComplete], [PulloutDate],[RefNo],[Description],[HaulingStatus],[HaulerName],[PackingAuditStatus],[PackingAuditName],
  [M360MDStatus],[M360MDName],[HangerPackScanTime],[HangerPackStatus],[HangerPackName],[JokerTagScanTime],[JokerTagStatus],[JokerTagName],[HeatSealScanTime],[HeatSealStatus],[HeatSealName]
)
select [KPIGroup], [FactoryID], [Line], [SP], [SeqNo], [Category], [Brand], [Style], [PONO], [Season], [Destination], [SCIDelivery], [BuyerDelivery], [PackingListID], [CtnNo], [Size], [CartonQty], [Status], [HaulingScanTime], [HauledQty], [DryRoomReceiveTime], [DryRoomTransferTime], [MDScanTime], [MDFailQty], [PackingAuditScanTime], [PackingAuditFailQty], [M360MDScanTime], [M360MDFailQty], [TransferToPackingErrorTime], [ConfirmPackingErrorReviseTime], [ScanAndPackTime], [ScanQty], [FtyTransferToClogTime], [ClogReceiveTime], [ClogLocation], [ClogReturnTime], [ClogTransferToCFATime], [CFAReceiveTime], [CFAReturnTime], [CFAReturnDestination], [ClogReceiveFromCFATime], [DisposeDate], [PulloutComplete], [PulloutDate],
 [RefNo],[Description],[HaulingStatus],[HaulerName],[PackingAuditStatus],[PackingAuditName],[M360MDStatus],[M360MDName],[HangerPackScanTime],[HangerPackStatus],[HangerPackName],[JokerTagScanTime],[JokerTagStatus],[JokerTagName],[HeatSealScanTime],[HeatSealStatus],[HeatSealName]
from #tmp t
where not exists (select 1 from P_CartonStatusTrackingList p where p.[SP] = t.[SP] and p.[SeqNo] = t.[SeqNo] and p.[PackingListID] = t.[PackingListID] and p.[CtnNo] = t.[CtnNo])


update p
	set p.[KPIGroup]						= t.[KPIGroup]
		, p.[FactoryID]						= t.[FactoryID]
		, p.[Line]							= t.[Line]
		, p.[Category]						= t.[Category]
		, p.[Brand]							= t.[Brand]
		, p.[Style]							= t.[Style]
		, p.[PONO]							= t.[PONO]
		, p.[Season]						= t.[Season]
		, p.[Destination]					= t.[Destination]
		, p.[SCIDelivery]					= t.[SCIDelivery]
		, p.[BuyerDelivery]					= t.[BuyerDelivery]
		, p.[Size]							= t.[Size]
		, p.[CartonQty]						= t.[CartonQty]
		, p.[Status]						= t.[Status]
		, p.[HaulingScanTime]				= t.[HaulingScanTime]
		, p.[HauledQty]						= t.[HauledQty]
		, p.[DryRoomReceiveTime]			= t.[DryRoomReceiveTime]
		, p.[DryRoomTransferTime]			= t.[DryRoomTransferTime]
		, p.[MDScanTime]					= t.[MDScanTime]
		, p.[MDFailQty]						= t.[MDFailQty]
		, p.[PackingAuditScanTime]			= t.[PackingAuditScanTime]
		, p.[PackingAuditFailQty]			= t.[PackingAuditFailQty]
		, p.[M360MDScanTime]				= t.[M360MDScanTime]
		, p.[M360MDFailQty]					= t.[M360MDFailQty]
		, p.[TransferToPackingErrorTime]	= t.[TransferToPackingErrorTime]
		, p.[ConfirmPackingErrorReviseTime]	= t.[ConfirmPackingErrorReviseTime]
		, p.[ScanAndPackTime]				= t.[ScanAndPackTime]
		, p.[ScanQty]						= t.[ScanQty]
		, p.[FtyTransferToClogTime]			= t.[FtyTransferToClogTime]
		, p.[ClogReceiveTime]				= t.[ClogReceiveTime]
		, p.[ClogLocation]					= t.[ClogLocation]
		, p.[ClogReturnTime]				= t.[ClogReturnTime]
		, p.[ClogTransferToCFATime]			= t.[ClogTransferToCFATime]
		, p.[CFAReceiveTime]				= t.[CFAReceiveTime]
		, p.[CFAReturnTime]					= t.[CFAReturnTime]
		, p.[CFAReturnDestination]			= t.[CFAReturnDestination]
		, p.[ClogReceiveFromCFATime]		= t.[ClogReceiveFromCFATime]
		, p.[DisposeDate]					= t.[DisposeDate]
		, p.[PulloutComplete]				= t.[PulloutComplete]
		, p.[PulloutDate]					= t.[PulloutDate]
        , p.[RefNo]							= t.[RefNo]
        , p.[Description]					= t.[Description]
        , p.[HaulingStatus]					= t.[HaulingStatus]
        , p.[HaulerName]					= t.[HaulerName]
        , p.[PackingAuditStatus]			= t.[PackingAuditStatus]
        , p.[PackingAuditName]				= t.[PackingAuditName]
        , p.[M360MDStatus]					= t.[M360MDStatus]
        , p.[M360MDName]					= t.[M360MDName]
        , p.[HangerPackScanTime]			= t.[HangerPackScanTime]
        , p.[HangerPackStatus]				= t.[HangerPackStatus]
        , p.[HangerPackName]				= t.[HangerPackName]
        , p.[JokerTagScanTime]				= t.[JokerTagScanTime]
        , p.[JokerTagStatus]				= t.[JokerTagStatus]
        , p.[JokerTagName]					= t.[JokerTagName]
        , p.[HeatSealScanTime]				= t.[HeatSealScanTime]
        , p.[HeatSealStatus]				= t.[HeatSealStatus]
        , p.[HeatSealName]					= t.[HeatSealName]
        , p.[MDMachineNo]                   = t.[MDMachineNo]
        , p.[BIFactoryID]                   = t.[BIFactoryID]
        , p.[BIInsertDate]                  = t.[BIInsertDate]
from P_CartonStatusTrackingList p
inner join #tmp t on p.[SP] = t.[SP] and p.[SeqNo] = t.[SeqNo] and p.[PackingListID] = t.[PackingListID] and p.[CtnNo] = t.[CtnNo]

-- 先寫入 Histroy
insert into P_CartonStatusTrackingList_History(FactoryID,SP,SeqNo,PackingListID,CtnNo,BIFactoryID,BIInsertDate)
select p.[FactoryID],p.[SP], p.[SeqNo], p.[PackingListID], p.[CtnNo], p.[BIFactoryID], getdate()
from P_CartonStatusTrackingList p
where not exists (select 1 from #tmp t where p.[SP] = t.[SP] and p.[SeqNo] = t.[SeqNo] and p.[PackingListID] = t.[PackingListID] and p.[CtnNo] = t.[CtnNo])
and p.Buyerdelivery  >= @StartDate

 delete p
 from P_CartonStatusTrackingList p
 where not exists (select 1 from #tmp t where p.[SP] = t.[SP] and p.[SeqNo] = t.[SeqNo] and p.[PackingListID] = t.[PackingListID] and p.[CtnNo] = t.[CtnNo])
 and p.Buyerdelivery  >= @StartDate

if exists (select 1 from BITableInfo b where b.id = 'P_CartonStatusTrackingList')
begin
	update b
		set b.TransferDate = getdate(),
            b.IS_Trans = 1
	from BITableInfo b
	where b.id = 'P_CartonStatusTrackingList'
end
else 
begin
	insert into BITableInfo(Id, TransferDate,IS_Trans)
	values('P_CartonStatusTrackingList', getdate(), 1)
end
";
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

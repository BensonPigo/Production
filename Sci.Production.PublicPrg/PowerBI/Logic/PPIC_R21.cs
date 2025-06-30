using Ict.Win.Defs;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <summary>
    /// 有調整到需要一併更新至BI
    /// </summary>
    public class PPIC_R21
    {
        /// <inheritdoc/>
        public PPIC_R21()
        {
            DBProxy.Current.DefaultTimeout = 7200;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetCartonStatusTrackingList(PPIC_R21_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@BuyerDeliveryFrom", SqlDbType.Date) { Value = (object)model.BuyerDeliveryFrom ?? DBNull.Value },
                new SqlParameter("@BuyerDeliveryTo", SqlDbType.Date) { Value = (object)model.BuyerDeliveryTo ?? DBNull.Value },
                new SqlParameter("@SCIDeliveryFrom", SqlDbType.Date) { Value = (object)model.SCIDeliveryFrom ?? DBNull.Value },
                new SqlParameter("@SCIDeliveryTo", SqlDbType.Date) { Value = (object)model.SCIDeliveryTo ?? DBNull.Value },
                new SqlParameter("@DateTimeProcessFrom", SqlDbType.DateTime2) { Value = (object)model.DateTimeProcessFrom ?? DBNull.Value },
                new SqlParameter("@DateTimeProcessTo", SqlDbType.DateTime2) { Value = (object)model.DateTimeProcessTo ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar) { Value = model.MDivisionID },
                new SqlParameter("@FactoryID", SqlDbType.VarChar) { Value = model.FactoryID },
                new SqlParameter("@OrderID", SqlDbType.VarChar) { Value = model.OrderID },
                new SqlParameter("@POID", SqlDbType.VarChar) { Value = model.PO },
                new SqlParameter("@PackID", SqlDbType.VarChar) { Value = model.PackID },
            };

            string sqlWhere = string.Empty;
            string sqlMdWhere = string.Empty;
            string sqlPKAuditWhere = string.Empty;
            string sqlPackWhere = string.Empty;
            string sqlProcessTime = string.Empty;
            string sqlWhereStatus = string.Empty;
            List<string> processList = model.ComboProcess.Split(',').ToList();

            // 過濾掉不用Status判斷的項目
            var validProcess = processList
                 .Select(p =>
                 {
                     string trimmed = p.Trim();
                     if (string.IsNullOrEmpty(trimmed))
                     {
                         return null;
                     }

                     if (trimmed == "Clog Receive" || trimmed == "Clog Return" || trimmed == "Clog Receive From CFA")
                     {
                         return "Clog";
                     }
                     else if (trimmed == "Clog Transfer To CFA")
                     {
                         return "CLOG transit to CFA";
                     }
                     else if (trimmed == "CFA Receive")
                     {
                         return "CFA";
                     }

                     return trimmed;
                 })
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct() // 可選：避免重複
                .ToList();
            if (validProcess.Any())
            {
                string inClause = string.Join(",", validProcess.Select(p => $"'{p}'"));
                sqlWhereStatus += $" AND Status IN ({inClause}) ";
            }

            if (model.BuyerDeliveryFrom.HasValue)
            {
                sqlWhere += " and s.BuyerDelivery >= @BuyerDeliveryFrom ";
            }

            if (model.BuyerDeliveryTo.HasValue)
            {
                sqlWhere += " and s.BuyerDelivery <= @BuyerDeliveryTo ";
            }

            if (model.SCIDeliveryFrom.HasValue)
            {
                sqlWhere += " and s.SCIDelivery >= @SCIDeliveryFrom ";
            }

            if (model.SCIDeliveryTo.HasValue)
            {
                sqlWhere += " and s.SCIDelivery <= @SCIDeliveryTo ";
            }

            string joinType = model.IsPPICP26 ? "LEFT" : "INNER";

            foreach (var process in processList)
            {
                switch (process)
                {
                    case "Dry Room Receive":
                        sqlMdWhere += $@" 
{joinType} JOIN DRYReceive on DRYReceive.PackingListID = pld.ID and DRYReceive.CTNStartNo = pld.CTNStartNo and DRYReceive.OrderID = pld.OrderID";
                        sqlProcessTime += @"
where DRYReceive.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += " and DryRoomReceiveTime.val between @DateTimeProcessFrom and @DateTimeProcessTo ";
                        break;
                    case "Dry Room Transfer":
                        sqlMdWhere += $@" 
{joinType} JOIN DRYTransfer on DRYTransfer.PackingListID = pld.ID and DRYTransfer.CTNStartNo = pld.CTNStartNo and DRYTransfer.OrderID = pld.OrderID";
                        sqlProcessTime += @"
where DRYTransfer.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += " and DryRoomTransferTime.val between @DateTimeProcessFrom and @DateTimeProcessTo ";
                        break;
                    case "Transfer To Packing Error":
                        sqlMdWhere += $@" 
{joinType} JOIN PackErrTransfer on PackErrTransfer.PackingListID = pld.ID and PackErrTransfer.CTNStartNo = pld.CTNStartNo and PackErrTransfer.OrderID = pld.OrderID";
                        sqlProcessTime += @"
where PackErrTransfer.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and TransferToPackingErrorTime.val between @DateTimeProcessFrom and @DateTimeProcessTo ";
                        break;
                    case "Confirm Packing Error Revise":
                        sqlMdWhere += $@" 
{joinType} JOIN PackErrCFM on PackErrCFM.PackingListID = pld.ID and PackErrCFM.CTNStartNo = pld.CTNStartNo and PackErrCFM.OrderID = pld.OrderID ";
                        sqlProcessTime += @"
where PackErrCFM.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and ConfirmPackingErrorReviseTime.val between @DateTimeProcessFrom and @DateTimeProcessTo ";
                        break;
                    case "Scan & Pack":
                        sqlPackWhere += @" 
and	pld.ScanEditDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and pld.ScanEditDate between @DateTimeProcessFrom and @DateTimeProcessTo ";
                        break;
                    case "Packing Audit":
                        sqlMdWhere += $@"
{joinType} JOIN CTNPackingAudit on CTNPackingAudit.PackingListID = pld.ID and CTNPackingAudit.CTNStartNo = pld.CTNStartNo and CTNPackingAudit.OrderID = pld.OrderID";
                        sqlProcessTime += @"
where CTNPackingAudit.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo
";
						sqlPKAuditWhere += " and PackingAuditScanTime.val between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "Dry Room MD":
                        sqlMdWhere += $@" 
{joinType} JOIN MDScan on MDScan.PackingListID = pld.ID and MDScan.CTNStartNo = pld.CTNStartNo and MDScan.OrderID = pld.OrderID ";
                        sqlProcessTime += @"
where MDScan.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and MDScan.val between @DateTimeProcessFrom and @DateTimeProcessTo ";
                        break;
                    case "Fty Transfer To Clog":
                        sqlMdWhere += $@" 
{joinType} JOIN TransferToClog on TransferToClog.PackingListID = pld.ID and TransferToClog.CTNStartNo = pld.CTNStartNo and TransferToClog.OrderID = pld.OrderID ";
                        sqlProcessTime += @"
where TransferToClog.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and FtyTransferToClogTime.val between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "Clog Receive":
                        sqlMdWhere += $@" 
{joinType} JOIN ClogReceive on ClogReceive.PackingListID = pld.ID and ClogReceive.CTNStartNo = pld.CTNStartNo and ClogReceive.OrderID = pld.OrderID ";
                        sqlProcessTime += @"
where ClogReceive.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and ClogReceiveTime.val between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "Clog Return":
                        sqlMdWhere += $@" 
{joinType} JOIN ClogReturn on ClogReturn.PackingListID = pld.ID and ClogReturn.CTNStartNo = pld.CTNStartNo and ClogReturn.OrderID = pld.OrderID ";
                        sqlProcessTime += @"
where ClogReturn.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and ClogReturnTime.val between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "Clog Transfer To CFA":
                        sqlMdWhere += $@" 
{joinType} JOIN TransferToCFA on TransferToCFA.PackingListID = pld.ID and TransferToCFA.CTNStartNo = pld.CTNStartNo and TransferToCFA.OrderID = pld.OrderID ";
                        sqlProcessTime += @"
where TransferToCFA.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and ClogTransferToCFATime.val between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "Clog Receive From CFA":
                        sqlMdWhere += $@" 
{joinType} JOIN ClogReceiveCFA on ClogReceiveCFA.PackingListID = pld.ID and ClogReceiveCFA.CTNStartNo = pld.CTNStartNo and ClogReceiveCFA.OrderID = pld.OrderID ";
                        sqlProcessTime += @"
where ClogReceiveCFA.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and ClogReceiveFromCFATime.val between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "CFA Receive":
                        sqlMdWhere += $@" 
{joinType} JOIN CFAReceive on CFAReceive.PackingListID = pld.ID and CFAReceive.CTNStartNo = pld.CTNStartNo and CFAReceive.OrderID = pld.OrderID ";
                        sqlProcessTime += @"
where CFAReceive.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and CFAReceiveTime.val between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "CFA Return":
                        sqlMdWhere += $@" 
{joinType} JOIN CFAReturn on CFAReturn.PackingListID = pld.ID and CFAReturn.CTNStartNo = pld.CTNStartNo and CFAReturn.OrderID = pld.OrderID ";
                        sqlProcessTime += @"
where CFAReturn.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and CFAReturnTime.val between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "Hauling":
                        sqlPackWhere += @" 
and	pld.HaulingDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and pld.HaulingDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "M360 MD":
                        sqlPackWhere += @" 
and	pld.M360MDScanDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and pld.M360MDScanDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "Pullout":
                        sqlPackWhere += @" 
and	p.PulloutDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and p.PulloutDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    case "M360 MD Scan":
                        sqlMdWhere += $@" 
{joinType} JOIN MDScan a on 	a.PackingListID = pld.ID and a.CTNStartNo = pld.CTNStartNo and a.OrderID = pld.OrderID";
                        sqlProcessTime += @"
where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                        sqlPKAuditWhere += "and M360MDScanTime.val between @DateTimeProcessFrom and @DateTimeProcessTo";
                        break;
                    default:
                        break;
                }
            }

            if (!MyUtility.Check.Empty(model.MDivisionID))
            {
                sqlWhere += $" and s.MDivisionID = '{model.MDivisionID}'";
            }

            if (!MyUtility.Check.Empty(model.FactoryID))
            {
                if (model.IsPPICP26)
                {
                    sqlMdWhere += $" and p.FactoryID = '{model.FactoryID}'";
                }
                else
                {
                    sqlWhere += $" and s.FTYGroup = '{model.FactoryID}'";
                }
            }

            if (model.ExcludeSisterTransferOut)
            {
                sqlWhere += $" and f.IsProduceFty = 1";
            }

            if (!model.IsBI && !model.IncludeCancelOrder)
            {
                sqlWhere += $" and s.Junk = 0";
            }

            if (!MyUtility.Check.Empty(model.OrderID))
            {
                sqlMdWhere += $" and pld.OrderID = '{model.OrderID}'";
            }

            if (!MyUtility.Check.Empty(model.OrderID))
            {
                sqlMdWhere += $" and pld.OrderID = '{model.OrderID}'";
            }

            if (!MyUtility.Check.Empty(model.PackID))
            {
                sqlMdWhere += $" and pld.ID = '{model.PackID}'";
            }

            if (!MyUtility.Check.Empty(model.PO))
            {
                sqlWhere += $" and s.CustPONo = '{model.PO}'";
            }

            string sqluseProcessTime = model.IsPPICP26 ? string.Empty : sqlProcessTime + Environment.NewLine + sqlPackWhere;
            if (model.IsPPICP26)
            {
                sqlMdWhere.ToString().Replace("INNER", "LEFT");
            }

            string finalColumn = string.Empty;
            if (model.IsPPICP26)
            {
                finalColumn = $@" 
with MaxValuesList as(
	select 
	PulloutDate = max(p.PulloutDate)
	,TransferDate = max(pld.TransferDate)
	,HaulingScanTime= max(HaulingScanTime.val)
	,[Packing Audit] = max(PackingAuditScanTime.val)
	,[M360 MD Scan] = max(M360MDScanTime.val)
	,[Hanger Pack] = max(CTNHangerPackTime.val)
	,[Joker Tag] = max(CTNJokerTagTime.val)
	,[Heat Seal] = max(CTNHeatSealTime.val)
	,[Dry Room Receive] = max(DryRoomReceiveTime.val)
	,[Dry Room Transfer] = max(DryRoomTransferTime.val)
	,[Transfer To Packing Error] = max(TransferToPackingErrorTime.val)
	,[Confirm Packing Error Revise] = max(ConfirmPackingErrorReviseTime.val)
	,[Scan & Pack] = max(pld.ScanEditDate)
	,[Clog] = max(ClogReceiveTime.val)
	,[Clog2] = max(ClogReceiveFromCFATime.val)
	,[CFA] = max(CFAReceiveTime.val)
	,[CFA transit to CLOG] = max(pld.CFAReturnClogDate)
	,[CLOG transit to CFA] = max(pld.TransferCFADate)
	,[Fty transit to CLOG] = max(FtyTransferToClogTime.val)
	--,[Status] = Status.val
	,[PackinglistID] = p.ID
	,[Factory] = p.FactoryID
	,[OrderID] = pld.OrderID
	,[CTNStartNo] = pld.CTNStartNo
	,[StyleID] = o.StyleID
	,[SeasonID] = o.SeasonID 
	,[BrandID] = o.BrandID
	,[CustPONo] = o.CustPONo
	,[Dest] = o.Dest
	,[BuyerDelivery] = o.BuyerDelivery
	,[SCIDelivery] = o.SciDelivery
";
            }
            else
            {
                finalColumn = @"
select distinct [KPIGroup] = f.KPICode
	, [FactoryID] = o.FactoryID
	, [Line] = ISNULL(Reverse(stuff(Reverse(o.SewLine),1,1,'')), '')
	, [SP] = o.ID
	, [SeqNo] = oqs.Seq
	, [Category] = ISNULL(d.Name, '')
	, [Brand] = o.BrandID
	, [Style] = o.StyleID
	, [PONO] = o.CustPONo
	, [Season] = o.SeasonID
	, [Destination] = ISNULL(p.Dest, '')
	, o.SciDelivery
	, oqs.BuyerDelivery
	, [PackingListID] = p.ID
	, [CtnNo] = pld.CTNStartNo
	, [Refno] = isnull(pld.Refno,'')
	, [Description] = isnull(LocalItem.Description,'')
	, [Size] = ISNULL(Size.val, '')
	, [CartonQty] = ISNULL(CartonQty.val, 0)
	, [Status] = Status.val
	, [HaulingScanTime] = HaulingScanTime.val
	, [HauledQty] = IIF(HaulingScanTime.val is null, 0, ISNULL(HauledQty.val, 0))
	, [HaulingStatus] = CASE    WHEN HauledReturn.val = 'Return' THEN 'Return'
								WHEN HauledReturn.val = 'Haul 'THEN 'Hauled'
							ELSE isnull(HauledReturn.val,'')
						END
	, [HaulerName] = isnull(HaulerNanme.val,'')
	, [DryRoomReceiveTime] = DryRoomReceiveTime.val
	, [DryRoomTransferTime] = DryRoomTransferTime.val 
	, [MDScanTime] = MDScan.val
	, [MDFailQty] = ISNULL(MDFailQty.val, 0)
	, [PackingAuditScanTime] = PackingAuditScanTime.val
	, [PackingAuditFailQty] = PackingAuditFailQty.val
	, [PackingAuditStatus] = CASE 　WHEN PackingAuditReturn.val = 'Return' THEN 'Return'
									WHEN PackingAuditReturn.val = 'Pass 'THEN 'Pass'
									WHEN PackingAuditReturn.val = 'Hold 'THEN 'Hold'
								ELSE isnull(PackingAuditReturn.val,'')
							END
	, [PackingAuditName] = isnull(PackingAuditName.val,'')
	, [M360MDScanTime] = M360MDScanTime.val
	, [M360MDFailQty] = M360MDFailQty.val
	, [M360MDStatus] =	CASE 　 WHEN M360MDReturn.val = 'Return'THEN 'Return'
								WHEN M360MDReturn.val = 'Pass 'THEN 'Pass'
								WHEN M360MDReturn.val = 'Hold 'THEN 'Hold'
							ELSE isnull(M360MDReturn.val,'')
						END
	, [M360MDName] = isnull(M360MDName.val,'')
	, [HangerPackScanTime] = CTNHangerPackTime.val
	, [HangerPackStatus] = CASE WHEN pld.HangerPackStatus = 'Return'THEN 'Return'
									WHEN pld.HangerPackStatus = 'Pass 'THEN 'Done'
								ELSE isnull(pld.HangerPackStatus,'')
							END
	, [HangerPackName] = isnull(HangerPackName.val,'')
	, [JokerTagScanTime] = CTNJokerTagTime.val 
	, [JokerTagStatus] = CASE WHEN pld.JokerTagStatus = 'Return'THEN 'Return'
								WHEN pld.JokerTagStatus = 'Pass 'THEN 'Done'
								ELSE isnull(pld.JokerTagStatus,'')
							END
	, [JokerTagName] = isnull(JokerTagName.val,'')
	, [HeatSealScanTime] = CTNHeatSealTime.val
	, [HeatSealStatus] = CASE WHEN pld.HeatSealStatus = 'Return'THEN 'Return'
								WHEN pld.HeatSealStatus = 'Pass 'THEN 'Done'
								ELSE isnull(pld.HeatSealStatus,'')
							END
    , [HeatSealName] = isnull(HeatSealName.val,'')
	, [TransferToPackingErrorTime] = TransferToPackingErrorTime.val
	, [ConfirmPackingErrorReviseTime] = ConfirmPackingErrorReviseTime.val
    , [MDMachineNo] = pld.MDMachineNo
	, [ScanAndPackTime] = pld.ScanEditDate
	, [ScanQty] = ISNULL(ScanQty.val, 0)
	, [FtyTransferToClogTime] = FtyTransferToClogTime.val
	, [ClogReceiveTime] = ClogReceiveTime.val
	, [ClogLocation] = ISNULL(pld.ClogLocationId, '')
	, [ClogReturnTime] = ClogReturnTime.val
	, [ClogTransferToCFATime] = ClogTransferToCFATime.val
	, [CFAReceiveTime] = CFAReceiveTime.val
	, [CFAReturnTime] = CFAReturnTime.val
	, [CFAReturnDestination] = ISNULL(CFAReturnDestination.val, '')
	, [ClogReceiveFromCFATime] = ClogReceiveFromCFATime.val
	, pld.DisposeDate
	, [PulloutComplete] = IIF(o.PulloutComplete = 1, 'Y', 'N')
	, p.PulloutDate
	, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
    , [BIInsertDate] = GETDATE()
";

            }
            string sql = $@"

-- 先限縮資料量

SELECT DISTINCT o.FactoryID
	,o.ID
	,o.BrandID
	,o.StyleID
	,o.CustPONo
	,o.SeasonID
	,o.Category	
	,o.SciDelivery
	,o.PulloutComplete
	,o.SewLine
	,o.Dest
	,o.BuyerDelivery
INTO #Orders
FROM (
	SELECT s.*
	FROM Production.dbo.Orders s
	INNER JOIN Production.dbo.Factory f with (nolock) on f.ID = s.FactoryID
	WHERE s.Category in ('B', 'G') {sqlWhere}	
) o
INNER JOIN Production.dbo.PackingList_Detail pld WITH (NOLOCK) on o.ID  = pld.OrderID
INNER JOIN Production.dbo.PackingList p WITH (NOLOCK) on p.ID = pld.ID
{sqlMdWhere}
{sqluseProcessTime}

SELECT pld.*
INTO #PackingList_Detail
FROM Production.dbo.PackingList_Detail pld WITH (NOLOCK)
WHERE EXISTS (
	select 1 
	from #Orders o
	WHERE pld.OrderID = o.ID 	
)

SELECT p.*
INTO #PackingList
FROM Production.dbo.PackingList p WITH (NOLOCK)
WHERE EXISTS (
	select 1 
	from #PackingList_Detail pld
	WHERE pld.ID = p.ID 	
)

SELECT ch.*
INTO #CTNHauling
FROM Production.dbo.CTNHauling ch
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE ch.PackingListID = pld.ID
      AND ch.CtnStartNo = pld.CtnStartNo
      AND ch.OrderID = pld.OrderID
);

SELECT ch.*
INTO #DryReceive
FROM Production.dbo.DryReceive ch
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE ch.PackingListID = pld.ID
      AND ch.CtnStartNo = pld.CtnStartNo
      AND ch.OrderID = pld.OrderID
);

SELECT ch.*
INTO #DryTransfer
FROM Production.dbo.DryTransfer ch
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE ch.PackingListID = pld.ID
      AND ch.CtnStartNo = pld.CtnStartNo
      AND ch.OrderID = pld.OrderID
);

SELECT t.*
INTO #MDScan
FROM Production.dbo.MDScan t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT t.*
INTO #CTNPackingAudit
FROM Production.dbo.CTNPackingAudit t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT t.*
INTO #PackErrTransfer
FROM Production.dbo.PackErrTransfer t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT t.*
INTO #PackErrCFM
FROM Production.dbo.PackErrCFM t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT t.*
INTO #TransferToClog
FROM Production.dbo.TransferToClog t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT t.*
INTO #ClogReceive
FROM Production.dbo.ClogReceive t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT t.*
INTO #ClogReturn
FROM Production.dbo.ClogReturn t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT t.*
INTO #TransferToCFA
FROM Production.dbo.TransferToCFA t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT t.*
INTO #CFAReceive
FROM Production.dbo.CFAReceive t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT t.*
INTO #CFAReturn
FROM Production.dbo.CFAReturn t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT t.*
INTO #ClogReceiveCFA
FROM Production.dbo.ClogReceiveCFA t
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE t.PackingListID = pld.ID
      AND t.CtnStartNo = pld.CtnStartNo
      AND t.OrderID = pld.OrderID
);

SELECT ch.*
INTO #CTNHangerPack
FROM Production.dbo.CTNHangerPack ch
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE ch.PackingListID = pld.ID
      AND ch.CtnStartNo = pld.CtnStartNo
      AND ch.OrderID = pld.OrderID
);

SELECT ch.*
INTO #CTNJokerTag
FROM Production.dbo.CTNJokerTag ch
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE ch.PackingListID = pld.ID
      AND ch.CtnStartNo = pld.CtnStartNo
      AND ch.OrderID = pld.OrderID
);

SELECT ch.*
INTO #CTNHeatSeal 
FROM Production.dbo.CTNHeatSeal  ch
WHERE EXISTS (
    SELECT 1
    FROM #PackingList_Detail pld
    WHERE ch.PackingListID = pld.ID
      AND ch.CtnStartNo = pld.CtnStartNo
      AND ch.OrderID = pld.OrderID
);
---------開始整理報表-------

{finalColumn}
from #Orders o
inner join Production.dbo.Order_QtyShip oqs with (nolock) on o.ID = oqs.Id
inner join Production.dbo.Factory f with (nolock) on f.ID = o.FactoryID
inner join #PackingList_Detail pld  on pld.OrderID = oqs.ID and pld.OrderShipmodeSeq = oqs.Seq and pld.CTNQty = 1
inner join #PackingList p with (nolock) on p.ID = pld.ID
left join Production.dbo.LocalItem with (nolock) on LocalItem.Refno = pld.Refno
left join Production.dbo.DropDownList d with (nolock) on d.Type = 'Category' AND d.ID = o.Category	
outer apply(
	select [val] = Stuff((select distinct concat('/',SizeCode) 
							from #PackingList_Detail pd with(nolock)
							where	pd.ID = pld.ID and
								pd.CtnStartNo = pld.CtnStartNo
							FOR XML PATH('')), 1, 1, '')
) Size
outer apply(
	select [val] = (select sum(pd.shipQty)
					from #PackingList_Detail pd with(nolock)
					where	pd.ID = pld.ID and
							pd.CtnStartNo = pld.CtnStartNo and
							pd.OrderID = pld.OrderID)
) CartonQty
outer apply(
	select [val] = (select sum(pd.ScanQty)
					from #PackingList_Detail pd with(nolock)
					where	pd.ID = pld.ID and
							pd.CtnStartNo = pld.CtnStartNo and
							pd.OrderID = pld.OrderID)
) ScanQty
outer apply(
	select [val] = (select max(ch.AddDate)
					from #CTNHauling ch with(nolock)
					where	ch.PackingListID = pld.ID and
							ch.CtnStartNo = pld.CtnStartNo and
							ch.OrderID = pld.OrderID)
) HaulingScanTime
outer apply(
	select [val] = (select isnull(sum(QtyPerCtn),0) 
					from #PackingList_Detail pd with(nolock)
					where pd.ID = pld.ID
					and exists (select 1 from #CTNHauling ch with(nolock)
								where ch.PackingListID = pld.ID
								and ch.CTNStartNo = pld.CTNStartNo
								and ch.OrderID = pld.OrderID
								and ch.SCICtnNo = pd.SCICtnNo))
) HauledQty
outer apply(
	select [val] = (select max(ch.Status)
					from #CTNHauling ch with(nolock)
					where ch.PackingListID = pld.ID 
					and ch.CTNStartNo = pld.CTNStartNo
					and ch.OrderID = pld.OrderID)

) HauledReturn
outer apply(
	select [val] = (
		select top 1 ch.AddName + '-' + isnull(pass1.Name, isnull(p.Name , ''))
		from #CTNHauling ch with(nolock)
		left join pass1 with(nolock) on pass1.ID  = ch.AddName
		left join [ExtendServer].ManufacturingExecution.dbo.Pass1 p WITH (NOLOCK) on p.ID = ch.AddName
		where ch.PackingListID = pld.ID 
		and ch.CTNStartNo = pld.CTNStartNo
		and ch.OrderID = pld.OrderID
	)
) HaulerNanme
outer apply(
	select [val] = (select max(dr.AddDate)
					from #DryReceive dr with(nolock)
					where	dr.PackingListID = pld.ID and
							dr.CtnStartNo = pld.CtnStartNo and
							dr.OrderID = pld.OrderID)
) DryRoomReceiveTime
outer apply(
	select [val] = (select max(dr.AddDate)
					from #DryTransfer dr with(nolock)
					where	dr.PackingListID = pld.ID and
							dr.CtnStartNo = pld.CtnStartNo and
							dr.OrderID = pld.OrderID)
) DryRoomTransferTime
outer apply( 
	select [val] = (select max(AddDate)
					from #MDScan md with (nolock) 
					where	md.DataRemark = 'Create from PMS' and
							md.PackingListID = pld.ID and 
							md.CTNStartNo = pld.CTNStartNo and 
							md.OrderID = pld.OrderID)
) MDScan
outer apply(
	select [val] = (select  MAX(MDFailQty)
			from #MDScan md with (nolock) 
			where	md.PackingListID = pld.ID and 
					md.CTNStartNo = pld.CTNStartNo and 
					md.OrderID = pld.OrderID and
					md.AddDate = MDScan.val)
) MDFailQty
outer apply(
	select [val] = (select MAX(AddDate)
			from #CTNPackingAudit pa with (nolock) 
			where	pa.PackingListID = pld.ID and 
					pa.SCICtnNo = pld.SCICtnNo)
) PackingAuditScanTime
outer apply(
	select [val] = (select isnull(sum(pa.Qty), 0)
			from #CTNPackingAudit pa with (nolock) 
			where	pa.PackingListID = pld.ID and 
					pa.SCICtnNo = pld.SCICtnNo and 
					pa.AddDate = PackingAuditScanTime.val)
) PackingAuditFailQty
outer apply(
	select [val] = (select max(pa.Status) 
			from #CTNPackingAudit pa with (nolock)
			where	pa.packingListID = pld.ID and
					pa.SCICtnNo = pld.SCICtnNo)
) PackingAuditReturn	
outer apply(
	select [val] = (
		select top 1 ch.AddName + '-' + isnull(pass1.Name,isnull(p.Name , ''))
		from #CTNPackingAudit ch with(nolock)
		left join pass1 with(nolock) on pass1.ID  = ch.AddName
		left join [ExtendServer].ManufacturingExecution.dbo.Pass1 p WITH (NOLOCK) on p.ID = ch.AddName
		where ch.PackingListID = pld.ID 
		and ch.CTNStartNo = pld.CTNStartNo
		and ch.OrderID = pld.OrderID
	)
) PackingAuditName
outer apply(
	select [val] = (select MAX(AddDate)
			from #MDScan md with (nolock) 
			where	md.DataRemark = 'Create from M360' and
					md.PackingListID = pld.ID and 
					md.SCICtnNo = pld.SCICtnNo)
) M360MDScanTime
outer apply(
	select [val] = (select isnull(sum(md.MDFailQty), 0)
			from #MDScan md with (nolock) 
			where	md.DataRemark = 'Create from M360' and
					md.PackingListID = pld.ID and 
					md.SCICtnNo = pld.SCICtnNo and
					md.AddDate = M360MDScanTime.val)
) M360MDFailQty
outer apply(
	select [val] = (select max(md.Status)
			from #MDScan md with (nolock)
			where	md.DataRemark = 'Create from M360' and
					md.PackingListID = pld.ID and
					md.SCICtnNo = pld.SCICtnNo)
) M360MDReturn
outer apply(
	select [val] = (
		select top 1 ch.AddName + '-' + isnull(pass1.Name,isnull(p.Name , ''))
		from #MDScan ch with(nolock)
		left join pass1 with(nolock) on pass1.ID  = ch.AddName
		left join [ExtendServer].ManufacturingExecution.dbo.Pass1 p WITH (NOLOCK) on p.ID = ch.AddName
		where ch.PackingListID = pld.ID 
		and ch.CTNStartNo = pld.CTNStartNo
		and ch.OrderID = pld.OrderID
	)
) M360MDName
outer apply(
	select [val] = (select MAX(AddDate)
			from #PackErrTransfer pe with (nolock) 
			where	pe.PackingListID = pld.ID and 
					pe.CTNStartNo = pld.CTNStartNo and
					pe.OrderID = pld.OrderID)
) TransferToPackingErrorTime
outer apply(
	select [val] = (select MAX(AddDate)
			from #PackErrCFM pe with (nolock) 
			where	pe.PackingListID = pld.ID and 
					pe.CTNStartNo = pld.CTNStartNo and
					pe.OrderID = pld.OrderID)
) ConfirmPackingErrorReviseTime
outer apply(
	select [val] = (select MAX(AddDate)
			from #TransferToClog tc with (nolock) 
			where	tc.PackingListID = pld.ID and 
					tc.CTNStartNo = pld.CTNStartNo and
					tc.OrderID = pld.OrderID)
) FtyTransferToClogTime
outer apply(
	select [val] = (select MAX(AddDate)
			from #ClogReceive cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and
					cr.OrderID = pld.OrderID)
) ClogReceiveTime
outer apply(
	select [val] = (select MAX(AddDate)
			from #ClogReturn cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and
					cr.OrderID = pld.OrderID)
) ClogReturnTime
outer apply(
	select [val] = (select MAX(AddDate)
			from #TransferToCFA tc with (nolock) 
			where	tc.PackingListID = pld.ID and 
					tc.CTNStartNo = pld.CTNStartNo and
					tc.OrderID = pld.OrderID)
) ClogTransferToCFATime
outer apply(
	select [val] = (select MAX(AddDate)
			from #CFAReceive cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and
					cr.OrderID = pld.OrderID)
) CFAReceiveTime
outer apply(
	select [val] = (select MAX(AddDate)
			from #CFAReturn cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and
					cr.OrderID = pld.OrderID)
) CFAReturnTime
outer apply(
	select [val] = (select distinct cr.ReturnTo
			from #CFAReturn cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and
					cr.OrderID = pld.OrderID and 
					cr.AddDate = CFAReturnTime.val)
) CFAReturnDestination
outer apply(
	select [val] = (select MAX(AddDate)
			from #ClogReceiveCFA cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and
					cr.OrderID = pld.OrderID)
) ClogReceiveFromCFATime
outer apply(
	select [val] = (select MAX(AddDate)
			from #CTNHangerPack cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and
					cr.OrderID = pld.OrderID)
) CTNHangerPackTime
outer apply(
	select [val] = (
		select top 1 ch.AddName + '-' + isnull(pass1.Name,isnull(p.Name , ''))
		from #CTNHangerPack ch with(nolock)
		left join pass1 with(nolock) on pass1.ID  = ch.AddName
		left join [ExtendServer].ManufacturingExecution.dbo.Pass1 p WITH (NOLOCK) on p.ID = ch.AddName
		where ch.PackingListID = pld.ID 
		and ch.CTNStartNo = pld.CTNStartNo
		and ch.OrderID = pld.OrderID
	)
) HangerPackName
outer apply(
	select [val] = (select MAX(AddDate)
			from #CTNJokerTag cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and
					cr.OrderID = pld.OrderID)
) CTNJokerTagTime
outer apply(
	select [val] = (
		select top 1 ch.AddName + '-' + isnull(pass1.Name,isnull(p.Name , ''))
		from #CTNJokerTag ch with(nolock)
		left join pass1 with(nolock) on pass1.ID  = ch.AddName
		left join [ExtendServer].ManufacturingExecution.dbo.Pass1 p WITH (NOLOCK) on p.ID = ch.AddName
		where ch.PackingListID = pld.ID 
		and ch.CTNStartNo = pld.CTNStartNo
		and ch.OrderID = pld.OrderID
	)
) JokerTagName
outer apply(
	select [val] = (select MAX(AddDate)
			from #CTNHeatSeal cr with (nolock) 
			where	cr.PackingListID = pld.ID and 
					cr.CTNStartNo = pld.CTNStartNo and
					cr.OrderID = pld.OrderID)
) CTNHeatSealTime
outer apply(
	select [val] = (
		select top 1 ch.AddName + '-' + isnull(pass1.Name,isnull(p.Name , ''))
		from #CTNHeatSeal ch with(nolock)
		left join pass1 with(nolock) on pass1.ID  = ch.AddName
		left join [ExtendServer].ManufacturingExecution.dbo.Pass1 p WITH (NOLOCK) on p.ID = ch.AddName
		where ch.PackingListID = pld.ID 
		and ch.CTNStartNo = pld.CTNStartNo
		and ch.OrderID = pld.OrderID
	)
) HeatSealName
outer apply(
select val = case 			
			when p.PulloutDate is not null then 'Pullout'
			when HaulingScanTime.val is null 
					and PackingAuditScanTime.val is null 
					and M360MDScanTime.val is null
					and CTNHangerPackTime.val is null
					and CTNJokerTagTime.val is null
					and CTNHeatSealTime.val is null
					and pld.ScanEditDate  is null 
					and pld.TransferDate is null
					and DryRoomReceiveTime.val is null
					and DryRoomTransferTime.val is null
					and TransferToPackingErrorTime.val is null
					and ConfirmPackingErrorReviseTime.val is null
					then 'Fty'
			when pld.TransferDate is null and 
				((ClogReturnTime.val >= isnull(PackingAuditScanTime.val, '19710101') 
				and ClogReturnTime.val >= isnull(M360MDScanTime.val, '19710101') 
				and ClogReturnTime.val >= isnull(pld.ScanEditDate, '19710101') 
				and ClogReturnTime.val >= isnull(HaulingScanTime.val, '19710101')
				and ClogReturnTime.val >= isnull(CTNHangerPackTime.val, '19710101')
				and ClogReturnTime.val >= isnull(CTNJokerTagTime.val, '19710101')
				and ClogReturnTime.val >= isnull(CTNHeatSealTime.val, '19710101')
				and ClogReturnTime.val >= isnull(DryRoomReceiveTime.val, '19710101')
				and ClogReturnTime.val >= isnull(DryRoomTransferTime.val, '19710101')
				and ClogReturnTime.val >= isnull(TransferToPackingErrorTime.val, '19710101')
				and ClogReturnTime.val >= isnull(ConfirmPackingErrorReviseTime.val, '19710101'))
				or 
				(CFAReturnTime.val >= isnull(PackingAuditScanTime.val, '19710101') 
				and CFAReturnTime.val >= isnull(M360MDScanTime.val, '19710101')
				and CFAReturnTime.val >= isnull(pld.ScanEditDate, '19710101') 
				and CFAReturnTime.val >= isnull(HaulingScanTime.val, '19710101')
				and CFAReturnTime.val >= isnull(CTNHangerPackTime.val, '19710101')
				and CFAReturnTime.val >= isnull(CTNJokerTagTime.val, '19710101')
				and CFAReturnTime.val >= isnull(CTNHeatSealTime.val, '19710101')
				and CFAReturnTime.val >= isnull(DryRoomReceiveTime.val, '19710101')
				and CFAReturnTime.val >= isnull(DryRoomTransferTime.val, '19710101')
				and CFAReturnTime.val >= isnull(TransferToPackingErrorTime.val, '19710101')
				and CFAReturnTime.val >= isnull(ConfirmPackingErrorReviseTime.val, '19710101')))
					then 'Fty'
			when pld.TransferDate is null 
					and HaulingScanTime.val > isnull(PackingAuditScanTime.val, '19710101') 
					and HaulingScanTime.val > isnull(M360MDScanTime.val, '19710101') 
					and HaulingScanTime.val > isnull(CTNHangerPackTime.val, '19710101') 
					and HaulingScanTime.val > isnull(CTNJokerTagTime.val, '19710101') 
					and HaulingScanTime.val > isnull(CTNHeatSealTime.val, '19710101')
					and HaulingScanTime.val > isnull(DryRoomReceiveTime.val, '19710101')
					and HaulingScanTime.val > isnull(DryRoomTransferTime.val, '19710101')
					and HaulingScanTime.val > isnull(TransferToPackingErrorTime.val, '19710101')
					and HaulingScanTime.val > isnull(ConfirmPackingErrorReviseTime.val, '19710101')					
					and HaulingScanTime.val > isnull(pld.ScanEditDate, '19710101')
					then 'Hauling'
			when pld.TransferDate is null 
					and PackingAuditScanTime.val >= isnull(HaulingScanTime.val, '19710101') 
					and PackingAuditScanTime.val > isnull(M360MDScanTime.val, '19710101') 
					and PackingAuditScanTime.val > isnull(CTNHangerPackTime.val, '19710101') 
					and PackingAuditScanTime.val > isnull(CTNJokerTagTime.val, '19710101') 
					and PackingAuditScanTime.val > isnull(CTNHeatSealTime.val, '19710101')
					and PackingAuditScanTime.val > isnull(DryRoomReceiveTime.val, '19710101')
					and PackingAuditScanTime.val > isnull(DryRoomTransferTime.val, '19710101')
					and PackingAuditScanTime.val > isnull(TransferToPackingErrorTime.val, '19710101')
					and PackingAuditScanTime.val > isnull(ConfirmPackingErrorReviseTime.val, '19710101')
					and PackingAuditScanTime.val > isnull(pld.ScanEditDate, '19710101')
					then 'Packing Audit'
			when pld.TransferDate is null 
					and M360MDScanTime.val >= isnull(PackingAuditScanTime.val, '19710101') 
					and M360MDScanTime.val >= isnull(HaulingScanTime.val, '19710101') 
					and M360MDScanTime.val > isnull(CTNHangerPackTime.val, '19710101') 
					and M360MDScanTime.val > isnull(CTNJokerTagTime.val, '19710101') 
					and M360MDScanTime.val > isnull(CTNHeatSealTime.val, '19710101')
					and M360MDScanTime.val > isnull(DryRoomReceiveTime.val, '19710101')
					and M360MDScanTime.val > isnull(DryRoomTransferTime.val, '19710101')
					and M360MDScanTime.val > isnull(TransferToPackingErrorTime.val, '19710101')
					and M360MDScanTime.val > isnull(ConfirmPackingErrorReviseTime.val, '19710101')					
					and M360MDScanTime.val > isnull(pld.ScanEditDate, '19710101')
					then 'M360 MD'
			when pld.TransferDate is null 
					and CTNHangerPackTime.val >= isnull(PackingAuditScanTime.val, '19710101') 
					and CTNHangerPackTime.val >= isnull(HaulingScanTime.val, '19710101') 
					and CTNHangerPackTime.val >= isnull(M360MDScanTime.val, '19710101') 
					and CTNHangerPackTime.val > isnull(CTNJokerTagTime.val, '19710101') 
					and CTNHangerPackTime.val > isnull(CTNHeatSealTime.val, '19710101')
					and CTNHangerPackTime.val > isnull(DryRoomReceiveTime.val, '19710101')
					and CTNHangerPackTime.val > isnull(DryRoomTransferTime.val, '19710101')
					and CTNHangerPackTime.val > isnull(TransferToPackingErrorTime.val, '19710101')
					and CTNHangerPackTime.val > isnull(ConfirmPackingErrorReviseTime.val, '19710101')
					and CTNHangerPackTime.val > isnull(pld.ScanEditDate, '19710101')
					then 'Hanger Pack'
			when pld.TransferDate is null 
					and CTNJokerTagTime.val >= isnull(CTNHangerPackTime.val, '19710101') 
					and CTNJokerTagTime.val >= isnull(PackingAuditScanTime.val, '19710101') 
					and CTNJokerTagTime.val >= isnull(HaulingScanTime.val, '19710101') 
					and CTNJokerTagTime.val >= isnull(M360MDScanTime.val, '19710101') 
					and CTNJokerTagTime.val > isnull(CTNHeatSealTime.val, '19710101')
					and CTNJokerTagTime.val > isnull(DryRoomReceiveTime.val, '19710101')
					and CTNJokerTagTime.val > isnull(DryRoomTransferTime.val, '19710101')
					and CTNJokerTagTime.val > isnull(TransferToPackingErrorTime.val, '19710101')
					and CTNJokerTagTime.val > isnull(ConfirmPackingErrorReviseTime.val, '19710101')
					and CTNJokerTagTime.val > isnull(pld.ScanEditDate, '19710101')
					then 'Joker Tag'
			when pld.TransferDate is null 
					and CTNHeatSealTime.val >= isnull(CTNHangerPackTime.val, '19710101') 
					and CTNHeatSealTime.val >= isnull(CTNJokerTagTime.val, '19710101') 
					and CTNHeatSealTime.val >= isnull(PackingAuditScanTime.val, '19710101') 
					and CTNHeatSealTime.val >= isnull(HaulingScanTime.val, '19710101') 
					and CTNHeatSealTime.val >= isnull(M360MDScanTime.val, '19710101')
					and CTNHeatSealTime.val > isnull(DryRoomReceiveTime.val, '19710101')
					and CTNHeatSealTime.val > isnull(DryRoomTransferTime.val, '19710101')
					and CTNHeatSealTime.val > isnull(TransferToPackingErrorTime.val, '19710101')
					and CTNHeatSealTime.val > isnull(ConfirmPackingErrorReviseTime.val, '19710101')
					and CTNHeatSealTime.val > isnull(pld.ScanEditDate, '19710101')
					then 'Heat Seal'
			when pld.TransferDate is null 
					and DryRoomReceiveTime.val >= isnull(PackingAuditScanTime.val, '19710101') 
					and DryRoomReceiveTime.val >= isnull(HaulingScanTime.val, '19710101') 
					and DryRoomReceiveTime.val >= isnull(M360MDScanTime.val, '19710101') 
					and DryRoomReceiveTime.val >= isnull(CTNHangerPackTime.val, '19710101') 
					and DryRoomReceiveTime.val >= isnull(CTNJokerTagTime.val, '19710101') 
					and DryRoomReceiveTime.val >= isnull(CTNHeatSealTime.val, '19710101')
					and DryRoomReceiveTime.val > isnull(DryRoomTransferTime.val, '19710101')
					and DryRoomReceiveTime.val > isnull(TransferToPackingErrorTime.val, '19710101')
					and DryRoomReceiveTime.val > isnull(ConfirmPackingErrorReviseTime.val, '19710101')
					and DryRoomReceiveTime.val > isnull(pld.ScanEditDate, '19710101')
					then 'Dry Room Receive'
			when pld.TransferDate is null 
					and DryRoomTransferTime.val >= isnull(PackingAuditScanTime.val, '19710101') 
					and DryRoomTransferTime.val >= isnull(HaulingScanTime.val, '19710101') 
					and DryRoomTransferTime.val >= isnull(M360MDScanTime.val, '19710101') 
					and DryRoomTransferTime.val >= isnull(CTNHangerPackTime.val, '19710101') 
					and DryRoomTransferTime.val >= isnull(CTNJokerTagTime.val, '19710101') 
					and DryRoomTransferTime.val >= isnull(CTNHeatSealTime.val, '19710101')
					and DryRoomTransferTime.val >= isnull(DryRoomReceiveTime.val, '19710101')
					and DryRoomTransferTime.val > isnull(TransferToPackingErrorTime.val, '19710101')
					and DryRoomTransferTime.val > isnull(ConfirmPackingErrorReviseTime.val, '19710101')
					and DryRoomTransferTime.val > isnull(pld.ScanEditDate, '19710101')
					then 'Dry Room Transfer'
			when pld.TransferDate is null 
					and TransferToPackingErrorTime.val >= isnull(PackingAuditScanTime.val, '19710101') 
					and TransferToPackingErrorTime.val >= isnull(HaulingScanTime.val, '19710101') 
					and TransferToPackingErrorTime.val >= isnull(M360MDScanTime.val, '19710101') 
					and TransferToPackingErrorTime.val >= isnull(CTNHangerPackTime.val, '19710101') 
					and TransferToPackingErrorTime.val >= isnull(CTNJokerTagTime.val, '19710101') 
					and TransferToPackingErrorTime.val >= isnull(CTNHeatSealTime.val, '19710101')
					and TransferToPackingErrorTime.val >= isnull(DryRoomReceiveTime.val, '19710101')
					and TransferToPackingErrorTime.val >= isnull(DryRoomTransferTime.val, '19710101')
					and TransferToPackingErrorTime.val > isnull(ConfirmPackingErrorReviseTime.val, '19710101')
					and TransferToPackingErrorTime.val > isnull(pld.ScanEditDate, '19710101')
					then 'Transfer To Packing Error'
			when pld.TransferDate is null 
					and ConfirmPackingErrorReviseTime.val >= isnull(PackingAuditScanTime.val, '19710101') 
					and ConfirmPackingErrorReviseTime.val >= isnull(HaulingScanTime.val, '19710101') 
					and ConfirmPackingErrorReviseTime.val >= isnull(M360MDScanTime.val, '19710101') 
					and ConfirmPackingErrorReviseTime.val >= isnull(CTNHangerPackTime.val, '19710101') 
					and ConfirmPackingErrorReviseTime.val >= isnull(CTNJokerTagTime.val, '19710101') 
					and ConfirmPackingErrorReviseTime.val >= isnull(CTNHeatSealTime.val, '19710101')
					and ConfirmPackingErrorReviseTime.val >= isnull(DryRoomReceiveTime.val, '19710101')
					and ConfirmPackingErrorReviseTime.val >= isnull(DryRoomTransferTime.val, '19710101')
					and ConfirmPackingErrorReviseTime.val >= isnull(TransferToPackingErrorTime.val, '19710101')
					and ConfirmPackingErrorReviseTime.val > isnull(pld.ScanEditDate, '19710101')
					then 'Confirm Packing Error Revise'
			when pld.TransferDate is null 
					and pld.ScanEditDate >= isnull(PackingAuditScanTime.val, '19710101') 
					and pld.ScanEditDate >= isnull(HaulingScanTime.val, '19710101') 
					and pld.ScanEditDate >= isnull(M360MDScanTime.val, '19710101') 
					and pld.ScanEditDate >= isnull(CTNHangerPackTime.val, '19710101') 
					and pld.ScanEditDate >= isnull(CTNJokerTagTime.val, '19710101') 
					and pld.ScanEditDate >= isnull(CTNHeatSealTime.val, '19710101')
					and pld.ScanEditDate >= isnull(DryRoomReceiveTime.val, '19710101')
					and pld.ScanEditDate >= isnull(DryRoomTransferTime.val, '19710101')
					and pld.ScanEditDate >= isnull(TransferToPackingErrorTime.val, '19710101')
					and pld.ScanEditDate >= isnull(ConfirmPackingErrorReviseTime.val, '19710101')
					then 'Scan & Pack'
			when pld.TransferDate is not null and
				((ClogReceiveTime.val >= isnull(FtyTransferToClogTime.val, '19710101')
				and ClogReceiveTime.val > isnull(ClogReturnTime.val, '19710101')
				and ClogReceiveTime.val > isnull(ClogTransferToCFATime.val, '19710101')
				and ClogReceiveTime.val > isnull(CFAReceiveTime.val, '19710101')
				and ClogReceiveTime.val > isnull(CFAReturnTime.val, '19710101'))
				or
				(ClogReceiveFromCFATime.val >= isnull(FtyTransferToClogTime.val, '19710101') 
				and ClogReceiveFromCFATime.val >= isnull(ClogReturnTime.val, '19710101')
				and ClogReceiveFromCFATime.val >= isnull(ClogTransferToCFATime.val, '19710101')
				and ClogReceiveFromCFATime.val >= isnull(CFAReceiveTime.val, '19710101')
				and ClogReceiveFromCFATime.val >= isnull(CFAReturnTime.val, '19710101')))
					then 'Clog'
			when pld.TransferDate is not null 
					and CFAReceiveTime.val > isnull(CFAReturnTime.val, '19710101') 
					then 'CFA'
			when pld.CFAReturnClogDate is not null and pld.ClogLocationID = '2Clog'
					then 'CFA transit to CLOG'
			when pld.TransferCFADate is not null and pld.ClogLocationID = '2CFA'
					then 'CLOG transit to CFA'
			when pld.TransferDate is not null 
					and FtyTransferToClogTime.val > isnull(ClogReceiveTime.val, '19710101')
					then 'Fty transit to CLOG'
		else '' end 
)Status
where 1=1
";
            if (model.IsPPICP26)
            {
                sql += $@"

	group by Status.val
	, p.ID,p.FactoryID, pld.OrderID, pld.CTNStartNo, o.StyleID, o.SeasonID , o.BrandID, o.CustPONo
	,o.Dest,o.BuyerDelivery, o.SciDelivery
)
select 
Status = 
        case 
            when PulloutDate = [ScanTime] then 'Pullout'
            when HaulingScanTime = [ScanTime] then 'Hauling'
            when [Packing Audit] = [ScanTime] then 'Packing Audit'
            when [M360 MD Scan] = [ScanTime] then 'M360 MD Scan'
            when [Hanger Pack] = [ScanTime] then 'Hanger Pack'
			when [Joker Tag] = [ScanTime] then 'Joker Tag'
			when [Heat Seal] = [ScanTime] then 'Heat Seal'
			when [Dry Room Receive] = [ScanTime] then 'Dry Room Receive'
			when [Dry Room Transfer] = [ScanTime] then 'Dry Room Transfer'
			when [Transfer To Packing Error] = [ScanTime] then 'Transfer To Packing Error'
			when [Confirm Packing Error Revise] = [ScanTime] then 'Confirm Packing Error Revise'
			when [Scan & Pack] = [ScanTime] then 'Scan & Pack'
			when [Clog] = [ScanTime] then 'Clog'
			when [Clog2] = [ScanTime] then 'Clog'
			when [CFA] = [ScanTime] then 'CFA'
			when [CFA transit to CLOG] = [ScanTime] then 'CFA transit to CLOG'
			when [CLOG transit to CFA] = [ScanTime] then 'CLOG transit to CFA'
			when [Fty transit to CLOG] = [ScanTime] then 'Fty Transfer To Clog'
        end
,*
into #tmpP26Final
from (
select *,
[ScanTime] = (
		select max(v)
		from (values 
			(PulloutDate),
			(TransferDate),
			(HaulingScanTime),
			([Packing Audit]),
			([M360 MD Scan]),
			([Hanger Pack]),
			([Joker Tag]),
			([Heat Seal]),
			([Dry Room Receive]),
			([Dry Room Transfer]),
			([Transfer To Packing Error]),
			([Confirm Packing Error Revise]),
			([Scan & Pack]),
			([Clog]),
			([Clog2]),
			([CFA]),
			([CFA transit to CLOG]),
			([CLOG transit to CFA]),
			([Fty transit to CLOG])
		) as value_list(v)
	)
	from MaxValuesList
) as t

select * 
from #tmpP26Final
where 1=1
{sqlWhereStatus}

drop table #TransferToCFA,#CFAReceive,#CFAReturn,#ClogReceive,#ClogReceiveCFA,#ClogReturn,#CTNHangerPack
,#CTNHauling,#CTNHeatSeal,#CTNJokerTag,#CTNPackingAudit,#DryReceive,#DryTransfer,#MDScan
,#Orders,#PackErrCFM,#PackErrTransfer,#PackingList,#PackingList_Detail,#TransferToClog
";
            }
            else
            {
                sql += sqlPKAuditWhere;
            }

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sql, listPar, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }
    }
}

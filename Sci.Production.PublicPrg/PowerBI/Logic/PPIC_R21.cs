using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
                new SqlParameter("@DateTimeProcessFrom", SqlDbType.Date) { Value = (object)model.DateTimeProcessFrom ?? DBNull.Value },
                new SqlParameter("@DateTimeProcessTo", SqlDbType.Date) { Value = (object)model.DateTimeProcessTo ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar) { Value = model.MDivisionID },
                new SqlParameter("@FactoryID", SqlDbType.VarChar) { Value = model.FactoryID },
            };

            string sqlWhere = string.Empty;
            if (model.BuyerDeliveryFrom.HasValue)
            {
                sqlWhere += " and o.BuyerDelivery >= @BuyerDeliveryFrom ";
            }

            if (model.BuyerDeliveryTo.HasValue)
            {
                sqlWhere += " and o.BuyerDelivery <= @BuyerDeliveryTo ";
            }

            switch (model.ComboProcess)
            {
                case "Dry Room Receive":
                    sqlWhere += @" and exists(select 1 from DRYReceive a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Dry Room Transfer":
                    sqlWhere += @" and exists(select 1 from DRYTransfer a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Transfer To Packing Error":
                    sqlWhere += @" and exists(select 1 from PackErrTransfer a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Confirm Packing Error Revise":
                    sqlWhere += @" and exists(select 1 from PackErrCFM a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Scan & Pack":
                    sqlWhere += @" and pld.ScanEditDate between @DateTimeProcessFrom and @DateTimeProcessTo";
                    break;
                case "MD Scan":
                    sqlWhere += @" and exists(select 1 from MDScan a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Fty Transfer To Clog":
                    sqlWhere += @" and exists(select 1 from TransferToClog a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Clog Receive":
                    sqlWhere += @" and exists(select 1 from ClogReceive a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Clog Return":
                    sqlWhere += @" and exists(select 1 from ClogReturn a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Clog Transfer To CFA":
                    sqlWhere += @" and exists(select 1 from TransferToCFA a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "Clog Receive From CFA":
                    sqlWhere += @" and exists(select 1 from ClogReceiveCFA a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "CFA Receive":
                    sqlWhere += @" and exists(select 1 from CFAReceive a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                case "CFA Return":
                    sqlWhere += @" and exists(select 1 from CFAReturn a with (nolock) 
												where	a.AddDate between @DateTimeProcessFrom and @DateTimeProcessTo and
														a.PackingListID = pld.ID and
														a.CTNStartNo = pld.CTNStartNo and
														a.OrderID = pld.OrderID)";
                    break;
                default:
                    break;
            }

            if (!MyUtility.Check.Empty(model.MDivisionID))
            {
                sqlWhere += $" and o.MDivisionID = '{model.MDivisionID}'";
            }

            if (!MyUtility.Check.Empty(model.FactoryID))
            {
                sqlWhere += $" and o.FTYGroup = '{model.FactoryID}'";
            }

            if (model.ExcludeSisterTransferOut)
            {
                sqlWhere += $" and f.IsProduceFty = 1";
            }

            if (!model.IsBI && !model.IncludeCancelOrder)
            {
                sqlWhere += $" and o.Junk = 0";
            }

            string sql = $@"
	select distinct [KPIGroup] = f.KPICode
		, [Fty] = o.FactoryID
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
		, [Size] = ISNULL(Size.val, '')
		, [CartonQty] = ISNULL(CartonQty.val, 0)
		, [Status] = case 
				when HaulingScanTime.val is null 
						and PackingAuditScanTime.val is null 
						and M360MDScanTime.val is null
						and pld.ScanEditDate  is null 
						and pld.TransferDate is null 
						then 'Fty'
				when pld.TransferDate is null and 
					((ClogReturnTime.val > isnull(PackingAuditScanTime.val, '19710101') 
					and ClogReturnTime.val > isnull(M360MDScanTime.val, '19710101') 
					and ClogReturnTime.val > isnull(pld.ScanEditDate, '19710101') 
					and ClogReturnTime.val > isnull(HaulingScanTime.val, '19710101')) 
					or 
					(CFAReturnTime.val > isnull(PackingAuditScanTime.val, '19710101') 
					and CFAReturnTime.val > isnull(M360MDScanTime.val, '19710101')
					and CFAReturnTime.val > isnull(pld.ScanEditDate, '19710101') 
					and CFAReturnTime.val > isnull(HaulingScanTime.val, '19710101')))
						then 'Fty'
				when pld.TransferDate is null and PackingAuditScanTime.val > isnull(HaulingScanTime.val, '19710101') and PackingAuditScanTime.val > isnull(M360MDScanTime.val, '19710101') and PackingAuditScanTime.val > isnull(pld.ScanEditDate, '19710101') 
						then 'Packing Audit'
				when pld.TransferDate is null and HaulingScanTime.val > isnull(PackingAuditScanTime.val, '19710101') and HaulingScanTime.val > isnull(M360MDScanTime.val, '19710101') and HaulingScanTime.val > isnull(pld.ScanEditDate, '19710101') 
						then 'Hauling'
				when pld.TransferDate is null and M360MDScanTime.val > isnull(PackingAuditScanTime.val, '19710101') and M360MDScanTime.val > isnull(HaulingScanTime.val, '19710101') and M360MDScanTime.val > isnull(pld.ScanEditDate, '19710101') 
						then 'M360 MD'
				when pld.TransferDate is null and pld.ScanEditDate > isnull(PackingAuditScanTime.val, '19710101') and pld.ScanEditDate > isnull(HaulingScanTime.val, '19710101') and pld.ScanEditDate > isnull(M360MDScanTime.val, '19710101') 
						then 'Scan & Pack'
				when pld.TransferDate is not null and pld.TransferDate > isnull(pld.ReceiveDate, '19710101')
						then 'Fty transit to CLOG'
				when pld.TransferCFADate is not null and pld.ClogLocationID = '2CFA'
						then 'CLOG transit to CFA'
				when pld.CFAReturnClogDate is not null and pld.ClogLocationID = '2Clog'
						then 'CFA transit to CLOG'
				when CFAReceiveTime.val is not null and CFAReceiveTime.val > isnull(CFAReturnTime.val, '19710101') 
						then 'CFA'
				when ClogReceiveTime.val is not null and (CFAReceiveTime.val is null or ClogReceiveFromCFATime.val > isnull(CFAReturnTime.val, '19710101') )
						then 'Clog'
			else '' end 
		, [HaulingScanTime] = HaulingScanTime.val
		, [HauledQty] = IIF(HaulingScanTime.val is null, 0, ISNULL(HauledQty.val, 0))
		, [HaulingReturn] = IIF(HauledReturn.val = 'Return', 'Yes', '')
		, [DryRoomReceiveTime] = DryRoomReceiveTime.val
		, [DryRoomTransferTime] = DryRoomTransferTime.val 
		, [MDScanTime] = MDScan.val
		, [MDFailQty] = ISNULL(MDFailQty.val, 0)
		, [PackingAuditScanTime] = PackingAuditScanTime.val
		, [PackingAuditFailQty] = PackingAuditFailQty.val
		, [PackingAuditReturn] = IIF(PackingAuditReturn.val = 'Return', 'Yes', '')
		, [M360MDScanTime] = M360MDScanTime.val
		, [M360MDFailQty] = M360MDFailQty.val
		, [M360MDReturn] = IIF(M360MDReturn.val = 'Return', 'Yes', '')
		, [HangerPackScanTime] = pld.HangerPackScanTime
		, [HangerPackStatus] = CASE WHEN pld.HangerPackStatus = 'Return'THEN 'Return'
									 WHEN pld.HangerPackStatus = 'Pass 'THEN 'Hold'
									ELSE pld.HangerPackStatus
								END

		, [HangerPackFailQty] = pld.HangerPackFailQty
		, [JokerTagScanTime] = pld.JokerTagScanTime
		, [JokerTagStatus] = CASE WHEN pld.JokerTagStatus = 'Return'THEN 'Return'
								  WHEN pld.JokerTagStatus = 'Pass 'THEN 'Hold'
								  ELSE pld.JokerTagStatus
							  END

		, [JokerTagFailQty] = pld.JokerTagFailQty
		, [HeatSealScanTime] = pld.HeatSealScanTime
		, [HeatSealStatus] = CASE WHEN pld.HeatSealStatus = 'Return'THEN 'Return'
								  WHEN pld.HeatSealStatus = 'Pass 'THEN 'Hold'
								  ELSE pld.HeatSealStatus
							  END
		, [HeatSealFailQty] = pld.HeatSealFailQty
		, [TransferToPackingErrorTime] = TransferToPackingErrorTime.val
		, [ConfirmPackingErrorReviseTime] = ConfirmPackingErrorReviseTime.val
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
	from Production.dbo.Orders o with (nolock)
	inner join Production.dbo.Order_QtyShip oqs with (nolock) on o.ID = oqs.Id
	inner join Production.dbo.Factory f with (nolock) on f.ID = o.FactoryID
	inner join Production.dbo.PackingList_Detail pld with (nolock) on pld.OrderID = oqs.ID and pld.OrderShipmodeSeq = oqs.Seq and pld.CTNQty = 1
	inner join Production.dbo.PackingList p with (nolock) on p.ID = pld.ID
	left join Production.dbo.DropDownList d with (nolock) on d.Type = 'Category' AND d.ID = o.Category	
	outer apply(
		select [val] = Stuff((select distinct concat('/',SizeCode) 
								from Production.dbo.PackingList_Detail pd with(nolock)
								where	pd.ID = pld.ID and
									pd.CtnStartNo = pld.CtnStartNo
								FOR XML PATH('')), 1, 1, '')
	) Size
	outer apply(
		select [val] = (select sum(pd.shipQty)
						from Production.dbo.PackingList_Detail pd with(nolock)
						where	pd.ID = pld.ID and
								pd.CtnStartNo = pld.CtnStartNo and
								pd.OrderID = pld.OrderID)
	) CartonQty
	outer apply(
		select [val] = (select sum(pd.ScanQty)
						from Production.dbo.PackingList_Detail pd with(nolock)
						where	pd.ID = pld.ID and
								pd.CtnStartNo = pld.CtnStartNo and
								pd.OrderID = pld.OrderID)
	) ScanQty
	outer apply(
		select [val] = (select max(ch.AddDate)
						from Production.dbo.CTNHauling ch with(nolock)
						where	ch.PackingListID = pld.ID and
								ch.CtnStartNo = pld.CtnStartNo and
								ch.OrderID = pld.OrderID)
	) HaulingScanTime
	outer apply(
		select [val] = (select isnull(sum(QtyPerCtn),0) 
						from Production.dbo.PackingList_Detail pd with(nolock)
						where pd.ID = pld.ID
						and exists (select 1 from Production.dbo.CTNHauling ch with(nolock)
									where ch.PackingListID = pld.ID
									and ch.CTNStartNo = pld.CTNStartNo
									and ch.OrderID = pld.OrderID
									and ch.SCICtnNo = pd.SCICtnNo))
	) HauledQty
	outer apply(
		select [val] = (select max(ch.Status)
						from Production.dbo.CTNHauling ch with(nolock)
						where ch.PackingListID = pld.ID 
						and ch.CTNStartNo = pld.CTNStartNo
						and ch.OrderID = pld.OrderID)

	) HauledReturn
	outer apply(
		select [val] = (select max(dr.AddDate)
						from Production.dbo.DryReceive dr with(nolock)
						where	dr.PackingListID = pld.ID and
								dr.CtnStartNo = pld.CtnStartNo and
								dr.OrderID = pld.OrderID)
	) DryRoomReceiveTime
	outer apply(
		select [val] = (select max(dr.AddDate)
						from Production.dbo.DryTransfer dr with(nolock)
						where	dr.PackingListID = pld.ID and
								dr.CtnStartNo = pld.CtnStartNo and
								dr.OrderID = pld.OrderID)
	) DryRoomTransferTime
	outer apply( 
		select [val] = (select max(AddDate)
						from Production.dbo.MDScan md with (nolock) 
						where	md.DataRemark = 'Create from PMS' and
								md.PackingListID = pld.ID and 
								md.CTNStartNo = pld.CTNStartNo and 
								md.OrderID = pld.OrderID)
	) MDScan
	outer apply(
		select [val] = (select  MAX(MDFailQty)
				from Production.dbo.MDScan md with (nolock) 
				where	md.PackingListID = pld.ID and 
						md.CTNStartNo = pld.CTNStartNo and 
						md.OrderID = pld.OrderID and
						md.AddDate = MDScan.val)
	) MDFailQty
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.CTNPackingAudit pa with (nolock) 
				where	pa.PackingListID = pld.ID and 
						pa.SCICtnNo = pld.SCICtnNo)
	) PackingAuditScanTime
	outer apply(
		select [val] = (select isnull(sum(pa.Qty), 0)
				from Production.dbo.CTNPackingAudit pa with (nolock) 
				where	pa.PackingListID = pld.ID and 
						pa.SCICtnNo = pld.SCICtnNo and 
						pa.AddDate = PackingAuditScanTime.val)
	) PackingAuditFailQty
	outer apply(
		select [val] = (select max(pa.Status)
				from Production.dbo.CTNPackingAudit pa with (nolock)
				where	pa.packingListID = pld.ID and
						pa.SCICtnNo = pld.SCICtnNo)
	) PackingAuditReturn	
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.MDScan md with (nolock) 
				where	md.DataRemark = 'Create from M360' and
						md.PackingListID = pld.ID and 
						md.SCICtnNo = pld.SCICtnNo)
	) M360MDScanTime
	outer apply(
		select [val] = (select isnull(sum(md.MDFailQty), 0)
				from Production.dbo.MDScan md with (nolock) 
				where	md.DataRemark = 'Create from M360' and
						md.PackingListID = pld.ID and 
						md.SCICtnNo = pld.SCICtnNo and
						md.AddDate = M360MDScanTime.val)
	) M360MDFailQty
	outer apply(
		select [val] = (select max(md.Status)
				from Production.dbo.MDScan md with (nolock)
				where	md.DataRemark = 'Create from M360' and
						md.PackingListID = pld.ID and
						md.SCICtnNo = pld.SCICtnNo)
	) M360MDReturn
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.PackErrTransfer pe with (nolock) 
				where	pe.PackingListID = pld.ID and 
						pe.CTNStartNo = pld.CTNStartNo and
						pe.OrderID = pld.OrderID)
	) TransferToPackingErrorTime
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.PackErrCFM pe with (nolock) 
				where	pe.PackingListID = pld.ID and 
						pe.CTNStartNo = pld.CTNStartNo and
						pe.OrderID = pld.OrderID)
	) ConfirmPackingErrorReviseTime
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.TransferToClog tc with (nolock) 
				where	tc.PackingListID = pld.ID and 
						tc.CTNStartNo = pld.CTNStartNo and
						tc.OrderID = pld.OrderID)
	) FtyTransferToClogTime
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.ClogReceive cr with (nolock) 
				where	cr.PackingListID = pld.ID and 
						cr.CTNStartNo = pld.CTNStartNo and
						cr.OrderID = pld.OrderID)
	) ClogReceiveTime
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.ClogReturn cr with (nolock) 
				where	cr.PackingListID = pld.ID and 
						cr.CTNStartNo = pld.CTNStartNo and
						cr.OrderID = pld.OrderID)
	) ClogReturnTime
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.TransferToCFA tc with (nolock) 
				where	tc.PackingListID = pld.ID and 
						tc.CTNStartNo = pld.CTNStartNo and
						tc.OrderID = pld.OrderID)
	) ClogTransferToCFATime
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.CFAReceive cr with (nolock) 
				where	cr.PackingListID = pld.ID and 
						cr.CTNStartNo = pld.CTNStartNo and
						cr.OrderID = pld.OrderID)
	) CFAReceiveTime
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.CFAReturn cr with (nolock) 
				where	cr.PackingListID = pld.ID and 
						cr.CTNStartNo = pld.CTNStartNo and
						cr.OrderID = pld.OrderID)
	) CFAReturnTime
	outer apply(
		select [val] = (select distinct cr.ReturnTo
				from Production.dbo.CFAReturn cr with (nolock) 
				where	cr.PackingListID = pld.ID and 
						cr.CTNStartNo = pld.CTNStartNo and
						cr.OrderID = pld.OrderID and 
						cr.AddDate = CFAReturnTime.val)
	) CFAReturnDestination
	outer apply(
		select [val] = (select MAX(AddDate)
				from Production.dbo.ClogReceiveCFA cr with (nolock) 
				where	cr.PackingListID = pld.ID and 
						cr.CTNStartNo = pld.CTNStartNo and
						cr.OrderID = pld.OrderID)
	) ClogReceiveFromCFATime
	where o.Category in ('B', 'G')
	{sqlWhere}	
";
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

CREATE PROCEDURE [dbo].[PPIC_R21_FORBI]
	@StartDate as date
AS
BEGIN
	SET NOCOUNT ON;

	if @StartDate is null
	begin
		set @StartDate = DATEADD(MONTH,-1,GETDATE())
	end

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
				when pld.TransferDate is null and PackingAuditScanTime.val > isnull(HaulingScanTime.val, '19710101') and PackingAuditScanTime.val > isnull(M360MDScanTime.val, '19710101') and PackingAuditScanTime.val > isnull(pld.ScanEditDate, '19710101') 
						then 'Packing Audit'
				when pld.TransferDate is null and HaulingScanTime.val > isnull(PackingAuditScanTime.val, '19710101') and HaulingScanTime.val > isnull(M360MDScanTime.val, '19710101') and HaulingScanTime.val > isnull(pld.ScanEditDate, '19710101') 
						then 'Hauiling'
				when pld.TransferDate is null and M360MDScanTime.val > isnull(PackingAuditScanTime.val, '19710101') and M360MDScanTime.val > isnull(HaulingScanTime.val, '19710101') and M360MDScanTime.val > isnull(pld.ScanEditDate, '19710101') 
						then 'M360 MD'
				when pld.TransferDate is null and pld.ScanEditDate > isnull(PackingAuditScanTime.val, '19710101') and pld.ScanEditDate > isnull(HaulingScanTime.val, '19710101') and pld.ScanEditDate > isnull(M360MDScanTime.val, '19710101') 
						then 'Scan & Pack'
				when pld.TransferDate is not null and ClogReceiveTime.val is null 
						then 'Transit to CLOG'
				when CFAReceiveTime.val is not null and CFAReceiveTime.val > isnull(CFAReturnTime.val, '19710101') 
						then 'CFA'
				when ClogReceiveTime.val is not null and (CFAReceiveTime.val is null or ClogReceiveFromCFATime.val > isnull(CFAReturnTime.val, '19710101') )
						then 'Clog'
			else '' end 
		, [HaulingScanTime] = HaulingScanTime.val
		, [HauledQty] = ISNULL(HauledQty.val, 0)
		, [DryRoomReceiveTime] = DryRoomReceiveTime.val
		, [DryRoomTransferTime] = DryRoomTransferTime.val 
		, [MDScanTime] = MDScan.val
		, [MDFailQty] = ISNULL(MDFailQty.val, 0)
		, [PackingAuditScanTime] = PackingAuditScanTime.val
		, [PackingAuditFailQty] = PackingAuditFailQty.val
		, [M360MDScanTime] = M360MDScanTime.val
		, [M360MDFailQty] = M360MDFailQty.val
		, [TransferToPackingErrorTime] = TransferToPackingErrorTime.val
		, [ConfirmPackingErrorReviseTime] = ConfirmPackingErrorReviseTime.val
		, [ScanAndPackTime] = pld.ScanEditDate
		, [ScanQty] = ISNULL(pld.ScanQty, 0)
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
	where oqs.BuyerDelivery >= @StartDate
	and o.Category in ('B', 'G')

END
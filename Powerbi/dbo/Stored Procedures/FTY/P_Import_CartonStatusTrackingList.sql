Create PROCEDURE [dbo].[P_Import_CartonStatusTrackingList]
	@StartDate as date
As
BEGIN
	declare @SQLCMD nvarchar(max), @SQLCMD1 nvarchar(max), @SQLCMD2 nvarchar(max), @SQLCMD3 nvarchar(max), @SQLCMD_final nvarchar(max)

	set @SQLCMD  = 'exec Production.dbo.PPIC_R21_FORBI ''''' + cast(@StartDate as varchar)  + ''''''

		set @SQLCMD3 = '	
	insert into P_CartonStatusTrackingList([KPIGroup], [Fty], [Line], [SP], [SeqNo], [Category], [Brand], [Style], [PONO], [Season], [Destination], [SCIDelivery], [BuyerDelivery], [PackingListID], [CtnNo], [Size], [CartonQty], [Status], [HaulingScanTime], [HauledQty], [DryRoomReceiveTime], [DryRoomTransferTime], [MDScanTime], [MDFailQty], [PackingAuditScanTime], [PackingAuditFailQty], [M360MDScanTime], [M360MDFailQty], [TransferToPackingErrorTime], [ConfirmPackingErrorReviseTime], [ScanAndPackTime], [ScanQty], [FtyTransferToClogTime], [ClogReceiveTime], [ClogLocation], [ClogReturnTime], [ClogTransferToCFATime], [CFAReceiveTime], [CFAReturnTime], [CFAReturnDestination], [ClogReceiveFromCFATime], [DisposeDate], [PulloutComplete], [PulloutDate])
	select [KPIGroup], [Fty], [Line], [SP], [SeqNo], [Category], [Brand], [Style], [PONO], [Season], [Destination], [SCIDelivery], [BuyerDelivery], [PackingListID], [CtnNo], [Size], [CartonQty], [Status], [HaulingScanTime], [HauledQty], [DryRoomReceiveTime], [DryRoomTransferTime], [MDScanTime], [MDFailQty], [PackingAuditScanTime], [PackingAuditFailQty], [M360MDScanTime], [M360MDFailQty], [TransferToPackingErrorTime], [ConfirmPackingErrorReviseTime], [ScanAndPackTime], [ScanQty], [FtyTransferToClogTime], [ClogReceiveTime], [ClogLocation], [ClogReturnTime], [ClogTransferToCFATime], [CFAReceiveTime], [CFAReturnTime], [CFAReturnDestination], [ClogReceiveFromCFATime], [DisposeDate], [PulloutComplete], [PulloutDate]
	from #tmp t
	where not exists (select 1 from P_CartonStatusTrackingList p where p.[SP] = t.[SP] and p.[SeqNo] = t.[SeqNo] and p.[PackingListID] = t.[PackingListID] and p.[CtnNo] = t.[CtnNo])

	update p
		set p.[KPIGroup]						= t.[KPIGroup]
			, p.[Fty]							= t.[Fty]
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
	from P_CartonStatusTrackingList p
	inner join #tmp t on p.[SP] = t.[SP] and p.[SeqNo] = t.[SeqNo] and p.[PackingListID] = t.[PackingListID] and p.[CtnNo] = t.[CtnNo]


	 delete p
	 from P_CartonStatusTrackingList p
	 where not exists (select 1 from #tmp t where p.[SP] = t.[SP] and p.[SeqNo] = t.[SeqNo] and p.[PackingListID] = t.[PackingListID] and p.[CtnNo] = t.[CtnNo])
	 and p.Buyerdelivery  >= ''' + cast(@StartDate as varchar) + '''
	'

	set @SQLCMD_final = '
	SELECT * 
	into #tmp 
	FROM OPENQUERY([MainServer], ''' + @SQLCMD + ''' )

	' +  @SQLCMD3 + '
	'

	select @SQLCMD_final
	EXEC sp_executesql @SQLCMD_final

	if exists (select 1 from BITableInfo b where b.id = 'P_CartonStatusTrackingList')
	begin
		update b
			set b.TransferDate = getdate()
		from BITableInfo b
		where b.id = 'P_CartonStatusTrackingList'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_CartonStatusTrackingList', getdate())
	end
END
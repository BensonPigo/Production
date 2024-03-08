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

	-- P_CartonScanRate
	begin
		select p.BuyerDelivery
			, p.FTYGroup
			, [HaulingScanRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.HauledQty * 1.0 / p.TotalCartonQty) * 100) as decimal(5, 2))
			, [PackingAuditScanRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.PackingAuditQty * 1.0 / p.TotalCartonQty) * 100) as decimal(5, 2))
			, [MDScanRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.MDScanQty * 1.0 / p.TotalCartonQty) * 100) as decimal(5, 2))
			, [ScanAndPackRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.ScanAndPackQty * 1.0 / p.TotalCartonQty) * 100) as decimal(5, 2))
			, [PullOutRate] = cast(iif(isnull(p.TotalCartonQty, 0) = 0, 0, (p.SPCountWithPulloutCmplt * 1.0 / p.TotalCartonQty) * 100) as decimal(5, 2))
		into #tmp_P_CartonScanRate
		from (
			select [SPCount] = count(distinct p.SP)
				, [SPCountWithPulloutCmplt] = isnull(p2.SPCountWithPulloutCmplt, 0)
				, [HauledQty] = Sum(p.HauledQty)
				, [PackingAuditQty] = Sum(IIF(p.PackingAuditScanTime is null, 0, p.CartonQty))
				, [MDScanQty] = Sum(IIF(p.MDScanTime is null, 0, p.CartonQty))
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
			where p.BuyerDelivery >= CONVERT(date, GETDATE()) 
			and p.BuyerDelivery <= DATEADD(DAY ,7,CONVERT(date, GETDATE())) 
			group by f.FTYGroup, p.BuyerDelivery, p2.SPCountWithPulloutCmplt
		) p


		update p
			set p.[HaulingScanRate] = t.[HaulingScanRate]
				, p.[PackingAuditScanRate] = t.[PackingAuditScanRate]
				, p.[MDScanRate] = t.[MDScanRate]
				, p.[ScanAndPackRate] = t.[ScanAndPackRate]
				, p.[PullOutRate] = t.[PullOutRate]
		from P_CartonScanRate p
		inner join #tmp_P_CartonScanRate t on p.[Date]= t.[BuyerDelivery] and p.[FactoryID] = t.[FTYGroup]
		

		insert into P_CartonScanRate([Date], [FactoryID], [HaulingScanRate], [PackingAuditScanRate], [MDScanRate], [ScanAndPackRate], [PullOutRate])
		select [BuyerDelivery], [FTYGroup], [HaulingScanRate], [PackingAuditScanRate], [MDScanRate], [ScanAndPackRate], [PullOutRate]
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
	end

END
CREATE PROCEDURE [dbo].[P_ImportSDP]
	@Date_S date,
	@Date_E date
AS
begin
	SET NOCOUNT ON
	declare @current_PMS_ServerName nvarchar(50) = 'MainServer'
	declare @SqlCmd nvarchar(max) ='';
	declare @SqlCmdDelete nvarchar(max) ='';
	declare @SqlCmdUpdata nvarchar(max) ='';
	declare @SqlCmdinsert nvarchar(max) ='';

	set @SqlCmd = 
	'/************* 撈取 Production 資料 *************/
	SELECT *
	into #tmp
	FROM OPENQUERY(['+ @current_PMS_ServerName +'], 
	''exec production.[dbo].GetSDP ''''' + Cast(@Date_S as varchar(8)) + ''''', ''''' + Cast(@Date_E as varchar(8)) + ''''' '')'

	set @SqlCmdDelete = 
	'
	-- 刪除掉#tmp不同Key且SPNo相同的資料
	Delete P_SDP
	from P_SDP as a 
	where not exists(
		select 1 from #tmp b where a.SPNo=b.SPNo and a.Style = b.Style and a.Seq = b.Seq
	)
	and exists(select 1 from #tmp b where a.SPNo = b.SPNo)

	Delete p
	from P_SDP p
	inner join Orders o with(nolock) on p.[SPNo] = o.ID
	inner join OrderType ot with(nolock) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID and o.BrandID = ot.BrandID
	where ot.IsGMTMaster = 1
	'
	set @SqlCmdUpdata ='
	/************* 更新P_SDP的資料*************/
	update t set
	t.[Country]						=	isnull(s.[Country],'''')
	,t.[KPIGroup]					=	isnull(s.[KPIGroup],'''')
	,t.[Brand]						=	isnull(s.[Brand],'''')
	,t.[BuyerDelivery]				=	isnull(s.[BuyerDelivery],'''')
	,t.[FactoryKPI]					=	isnull(s.[FactoryKPI],'''')
	,t.[Extension]					=	isnull(s.[Extension],'''')
	,t.[DeliveryByShipmode]			=	isnull(s.[DeliveryByShipmode],'''')
	,t.[OrderQty]					=	isnull(s.[OrderQty],0)
	,t.[OnTimeQty]					=	isnull(s.[OnTimeQty],0)
	,t.[FailQty]					=	isnull(s.[FailQty],0)
	,t.[ClogRec_OnTimeQty]			=	isnull(s.[ClogRec_OnTimeQty],0)
	,t.[ClogRec_FailQty]			=	isnull(s.[ClogRec_FailQty],0)
	,t.[PullOutDate]				=	s.[PullOutDate]
	,t.[Shipmode]					=	isnull(s.[Shipmode],'''')
	,t.[Pullouttimes]				=	isnull(s.[Pullouttimes],0)
	,t.[GarmentComplete]			=	isnull(s.[GarmentComplete],'''')
	,t.[ReasonID]					=	isnull(s.[ReasonID],'''')
	,t.[OrderReason]				=	isnull(s.[OrderReason],'''')
	,t.[Handle]						=	isnull(s.[Handle],'''')
	,t.[SMR]						=	isnull(s.[SMR],'''')
	,t.[POHandle]					=	isnull(s.[POHandle],'''')
	,t.[POSMR]						=	isnull(s.[POSMR],'''')
	,t.[OrderType]					=	isnull(s.[OrderType],'''')
	,t.[DevSample]					=	isnull(s.[DevSample],'''')
	,t.[SewingQty]					=	isnull(s.[SewingQty],0)
	,t.[FOCQty]						=	isnull(s.[FOCQty],0)
	,t.[LastSewingOutputDate]		=	s.[LastSewingOutputDate]
	,t.[LastCartonReceivedDate]		=	s.[LastCartonReceivedDate]
	,t.[IDDReason]					=	isnull(s.[IDDReason],'''')
	,t.[PartialShipment]			=	isnull(s.[PartialShipment],'''')
	,t.[Alias]						=	isnull(s.[Alias],'''')
	,t.[CFAInspectionDate]			=	s.[CFAInspectionDate]
	,t.[CFAFinalInspectionResult]	=	isnull(s.[CFAFinalInspectionResult],'''')
	,t.[CFA3rdInspectDate]			=	s.[CFA3rdInspectDate]
	,t.[CFA3rdInspectResult]		=	isnull(s.[CFA3rdInspectResult],'''')
	,t.[Destination]				=	isnull(s.[Destination],'''')
	,t.[PONO]						=	isnull(s.[PONO],'''')
	,t.[OutstandingReason]			=	isnull(s.[OutstandingReason],'''')
	,t.[ReasonRemark]				=	isnull(s.[ReasonRemark],'''')
	,t.[FactoryID]					=	isnull(s.[FactoryID],'''')
	from P_SDP t
	inner join #tmp s on t.[SPNO]  = s.[SPNO]	and
						 t.[Style] = s.[Style]	and
						 t.[Seq]   = s.[Seq]'
	set @SqlCmdinsert = '
	/************* 新增P_SDP的資料*************/
	insert into P_SDP (
		Country
		,KPIGroup
		,FactoryID
		,SPNo
		,Style
		,Seq
		,Brand
		,BuyerDelivery
		,FactoryKPI
		,Extension
		,DeliveryByShipmode
		,OrderQty
		,OnTimeQty
		,FailQty
		,ClogRec_OnTimeQty
		,ClogRec_FailQty
		,PullOutDate
		,ShipMode
		,Pullouttimes
		,GarmentComplete
		,ReasonID
		,OrderReason
		,Handle
		,SMR
		,POHandle
		,POSMR
		,OrderType
		,DevSample
		,SewingQty
		,FOCQty
		,LastSewingOutputDate
		,LastCartonReceivedDate
		,IDDReason
		,PartialShipment
		,Alias
		,CFAInspectionDate
		,CFAFinalInspectionResult
		,CFA3rdInspectDate
		,CFA3rdInspectResult
		,Destination
		,PONO
		,OutstandingReason
		,ReasonRemark
	)
	select 
		t.Country
		,t.KPIGroup
		,t.FactoryID
		,t.SPNo
		,t.Style
		,t.Seq
		,t.Brand
		,t.BuyerDelivery
		,t.FactoryKPI
		,t.Extension
		,t.DeliveryByShipmode
		,t.OrderQty
		,t.OnTimeQty
		,t.FailQty
		,t.ClogRec_OnTimeQty
		,t.ClogRec_FailQty
		,t.PullOutDate
		,t.ShipMode
		,t.Pullouttimes
		,t.GarmentComplete
		,t.ReasonID
		,isnull(t.OrderReason,'''')
		,t.Handle
		,t.SMR
		,t.POHandle
		,t.POSMR
		,t.OrderType
		,t.DevSample
		,isnull(t.SewingQty,'''')
		,t.FOCQty
		,t.LastSewingOutputDate
		,t.LastCartonReceivedDate
		,isnull(t.IDDReason,'''')
		,t.PartialShipment
		,t.Alias
		,t.CFAInspectionDate
		,isnull(t.CFAFinalInspectionResult,'''')
		,t.CFA3rdInspectDate
		,isnull(t.CFA3rdInspectResult,'''')
		,t.Destination
		,t.PONO
		,isnull(t.OutstandingReason,'''')
		,t.ReasonRemark
	from #tmp t
	where not exists (select 1 from P_SDP s where t.[SPNO] = s.[SPNO] and t.[Style] = s.[Style] and t.[Seq] = s.[Seq])

	drop table #tmp

	IF EXISTS (select 1 from BITableInfo b where b.id = ''P_SDP'')
	BEGIN
		update b
			set b.TransferDate = getdate()
		from BITableInfo b
		where b.id = ''P_SDP''
	END
	'

	DECLARE @SqlCmdAll nVARCHAR(MAX);
	set @SqlCmdAll = @SqlCmd + @SqlCmdDelete+@SqlCmdUpdata+@SqlCmdinsert


	EXEC sp_executesql @Sqlcmdall
end 
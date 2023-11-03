CREATE PROCEDURE [dbo].[P_ImportSDP]
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
	''exec production.[dbo].GetSDP '')'

	set @SqlCmdDelete = 
	'
	/************* 刪除P_SDP的資料*************/
	Delete P_SDP
	from P_SDP as a 
	inner join #tmp as b on a.FactoryID = b.FactoryID and a.SPNo=b.SPNo and a.Style = b.Style and a.Seq = b.Seq

	-- 刪除掉#tmp不同Key且SPNo相同的資料
	Delete P_SDP
	from P_SDP as a 
	where not exists(
		select 1 from #tmp b where a.FactoryID = b.FactoryID and a.SPNo=b.SPNo and a.Style = b.Style and a.Seq = b.Seq
	)
	and exists(select 1 from #tmp b where a.SPNo = b.SPNo)
	'
	set @SqlCmdUpdata ='
	/************* 更新P_SDP的資料*************/
	update t set
	t.[Country]						=	s.[Country]
	,t.[KPIGroup]					=	s.[KPIGroup]
	,t.[Brand]						=	s.[Brand]
	,t.[BuyerDelivery]				=	s.[BuyerDelivery]
	,t.[FactoryKPI]					=	s.[FactoryKPI]
	,t.[Extension]					=	s.[Extension]
	,t.[DeliveryByShipmode]			=	s.[DeliveryByShipmode]
	,t.[OrderQty]					=	s.[OrderQty]
	,t.[OnTimeQty]					=	s.[OnTimeQty]
	,t.[FailQty]					=	s.[FailQty]
	,t.[ClogRec_OnTimeQty]			=	s.[ClogRec_OnTimeQty]
	,t.[ClogRec_FailQty]			=	s.[ClogRec_FailQty]
	,t.[PullOutDate]				=	s.[PullOutDate]
	,t.[Shipmode]					=	s.[Shipmode]
	,t.[Pullouttimes]				=	s.[Pullouttimes]
	,t.[GarmentComplete]			=	s.[GarmentComplete]
	,t.[ReasonID]					=	s.[ReasonID]
	,t.[OrderReason]				=	s.[OrderReason]  
	,t.[Handle]						=	s.[Handle]
	,t.[SMR]						=	s.[SMR]
	,t.[POHandle]					=	s.[POHandle]
	,t.[POSMR]						=	s.[POSMR]
	,t.[OrderType]					=	s.[OrderType]
	,t.[DevSample]					=	s.[DevSample]
	,t.[SewingQty]					=	s.[SewingQty]
	,t.[FOCQty]						=	s.[FOCQty]
	,t.[LastSewingOutputDate]		=	s.[LastSewingOutputDate]
	,t.[LastCartonReceivedDate]		=	s.[LastCartonReceivedDate]
	,t.[IDDReason]					=	s.[IDDReason]
	,t.[PartialShipment]			=	s.[PartialShipment]
	,t.[Alias]						=	s.[Alias]
	,t.[CFAInspectionDate]			=	s.[CFAInspectionDate]
	,t.[CFAFinalInspectionResult]	=	s.[CFAFinalInspectionResult]
	,t.[CFA3rdInspectDate]			=	s.[CFA3rdInspectDate]
	,t.[CFA3rdInspectResult]		=	s.[CFA3rdInspectResult]
	,t.[Destination]				=	s.[Destination]
	,t.[PONO]						=	s.[PONO]
	,t.[OutstandingReason]			=	s.[OutstandingReason]
	,t.[ReasonRemark]				=	s.[ReasonRemark]
	from P_SDP s
	inner join #tmp t on t.FactoryID = s.FactoryID  and
						 t.[SPNO]  = s.[SPNO]	and
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
	left join P_SDP s on t.FactoryID = s.FactoryID  and
						 t.[SPNO]  = s.[SPNO]	and
						 t.[Style] = s.[Style]	and
						 t.[Seq]   = s.[Seq] 
	where s.FactoryID is null and s.SPNo is null and s.Seq is null and s.Style is null

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
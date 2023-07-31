CREATE PROCEDURE [dbo].[PPIC_R03_FORBI]
     @SCIDeliveryS as date = null,
	 @SCIDeliveryE as date = null
AS
BEGIN
	SET NOCOUNT ON;

	if @SCIDeliveryS is null
	begin
		set @SCIDeliveryS = dateadd(day, -30, Getdate())
	end

	if @SCIDeliveryE is null
	begin
		set @SCIDeliveryE = Getdate()
	end
	
	declare @StdTMS as int = (select StdTMS from System WITH (NOLOCK))

	select *
	into #tmp_Orders_base
	from Orders o WITH (NOLOCK) 
	where o.SCIDelivery >= @SCIDeliveryS 
	and o.SCIDelivery <= @SCIDeliveryE
	and (o.Category = 'B' or o.Category = 'S' or o.Category = 'G' or o.Category = '' )
	or (
	((select NoRestrictOrdersDelivery from System) = 0) 
		and (o.IsForecast = 0 or (o.IsForecast = 1 and (o.SciDelivery <= dateadd(m, datediff(m,0,dateadd(m, 5, GETDATE())),6) or o.BuyerDelivery < dateadd(m, datediff(m,0,dateadd(m, 5, GETDATE())),0))))
		and o.SCIDelivery >= @SCIDeliveryS 
		and o.SCIDelivery <= @SCIDeliveryE
		and (o.Category = 'B' or o.Category = 'S' or o.Category = 'G' or o.Category = '' )
	)

    select DISTINCT o.ID
            , o.MDivisionID
            , o.FtyGroup
            , o.FactoryID
            , o.BuyerDelivery
            , o.SciDelivery
            , o.POID
            , o.CRDDate
            , o.CFMDate
            , o.Dest
            , o.StyleID
            , s.StyleName
            , o.SeasonID
            , o.BrandID
            , o.ProjectID
            , Kit=(SELECT top 1 c.Kit From CustCD c WITH (NOLOCK) where c.ID=o.CustCDID AND c.BrandID=o.BrandID)
			,[PackingMethod]=d.Name 
            , o.HangerPack
            , o.Customize1
            , o.BuyMonth
            , o.CustPONo
            , o.CustCDID
            , o.ProgramID
			, [NonRevenue]=IIF(o.NonRevenue=1,'Y','N')
            , o.CdCodeID
	        , s.CDCodeNew
            , [ProductType] = r2.Name
		    , [FabricType] = r1.Name
		    , s.Lining
		    , s.Gender
		    , [Construction] = d1.Name
            , o.CPU
            , o.Qty as Qty
            , o.FOCQty
            , o.LocalOrder
            , o.PoPrice
            , o.CMPPrice
            , o.KPILETA
            , o.PFETA
            , o.LETA
            , o.MTLETA
            , o.SewETA
            , o.PackETA
            , o.MTLComplete
            , o.SewInLine
            , o.SewOffLine
            , o.CutInLine
            , o.CutOffLine
			, [CutInLine_SP] = csp.InLine
			, [CutOffLine_SP] = csp.OffLine
            , Category=case when o.Category='B'then'Bulk'
							when o.Category='G'then'Garment'
							when o.Category='M'then'Material'
							when o.Category='S'then'Sample'
							when o.Category='T'then'Sample mtl.'
							when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='' then'Bulk fc'
							when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='D' then'Dev. sample fc'
							when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='S' then'Sa. sample fc'
						end
            , o.PulloutDate
            , o.ActPulloutDate
            , o.SMR
            , o.MRHandle
            , o.MCHandle
            , o.OrigBuyerDelivery
            , o.DoxType
            , o.TotalCTN
            , PackErrCTN = isnull(o.PackErrCTN,0)
            , o.FtyCTN
            , o.ClogCTN
            , CFACTN=isnull(o.CFACTN,0)
            , o.VasShas
            , o.TissuePaper
            , [MTLExport]=IIF(o.MTLExport='OK','Y',o.MTLExport)
            , o.SewLine
            , o.ShipModeList
            , o.PlanDate
            , o.FirstProduction
			, o.LastProductionDate
            , o.OrderTypeID
            , o.SpecialMark
            , o.SampleReason
            , InspDate = QtyShip_InspectDate.Val
            , InspResult = QtyShip_Result.Val
            , InspHandle = QtyShip_Handle.Val
            , o.MnorderApv2
            , o.MnorderApv
            , o.PulloutComplete
            , o.FtyKPI
            , o.KPIChangeReason
            , o.EachConsApv
            , o.Junk
            , o.StyleUkey
            , o.CuttingSP
            , o.RainwearTestPassed
            , o.BrandFTYCode
            , o.CPUFactor
            , o.ClogLastReceiveDate
			, [IsMixMarker]=  CASE WHEN o.IsMixMarker=0 THEN 'Is Single Marker'
								WHEN o.IsMixMarker=1 THEN 'Is Mix  Marker'		
								WHEN o.IsMixMarker=2 THEN ' Is Mix Marker - SCI'
								ELSE ''
							END
            , o.GFR 
			, isForecast = iif(isnull(o.Category,'')='','1','')
            , [AirFreightByBrand] = IIF(o.AirFreightByBrand='1','Y','')
            , [BuyBack] = iif(exists (select 1 from Order_BuyBack WITH (NOLOCK) where ID = o.ID), 'Y', '')
            , [Cancelled] = case when o.junk = 1 then 
                                 case when o.NeedProduction = 1 then 'Y' 
                                      when o.KeepPanels = 1 then 'K'
                                      else 'N' end
                            else ''
                            end
            , o.Customize2
            , o.KpiMNotice
            , o.KpiEachConsCheck
            , o.LastCTNTransDate
            , o.LastCTNRecdDate
            , o.DryRoomRecdDate
            , o.DryRoomTransDate
            , o.MdRoomScanDate
            , [VasShasCutOffDate] = Format(DATEADD(DAY, -30, iif(GetMinDate.value is null, coalesce(o.BuyerDelivery, o.CRDDate, o.PlanDate, o.OrigBuyerDelivery), GetMinDate.value)), 'yyyy/MM/dd')
            , [StyleSpecialMark] = s.SpecialMark
             ,[IDD] = (SELECT  Stuff((select distinct concat( ',',Format(oqs.IDD, 'yyyy/MM/dd'))   from Order_QtyShip oqs with (nolock) where oqs.ID = o.ID FOR XML PATH('')),1,1,'') )
	        , [SewingMtlComplt]  = isnull(CompltSP.SewingMtlComplt, '')
	        , [PackingMtlComplt] = isnull(CompltSP.PackingMtlComplt, '')
            , o.OrganicCotton
            , o.DirectShip
            ,[StyleCarryover] = iif(s.NewCO = '2','V','')
            , o.PackLETA
	into #tmp_tmpOrders
	from #tmp_Orders_base o
	left join style s WITH (NOLOCK) on o.styleukey = s.ukey
	left join DropDownList d WITH(NOLOCK) ON o.CtnType=d.ID AND d.Type='PackingMethod'
	left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
    OUTER APPLY(
        SELECT  Name 
        FROM Pass1 WITH (NOLOCK) 
        WHERE Pass1.ID = O.InspHandle
    )I
	outer apply (
		select value = (
			select Min(Date)
			From (Values (o.BuyerDelivery), (o.CRDDate), (o.PlanDate), (o.OrigBuyerDelivery)) as tmp (Date)
			where tmp.Date is not null
		)
	) GetMinDate
    outer apply (
	    select 
		    [PackingMtlComplt] = max([PackingMtlComplt])
		    , [SewingMtlComplt] = max([SewingMtlComplt])
	    from 
	    (
		    select  f.ProductionType
			    , [PackingMtlComplt] = iif(f.ProductionType = 'Packing' and sum(iif(f.ProductionType = 'Packing', 1, 0)) = sum(iif(f.ProductionType = 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
			    , [SewingMtlComplt] = iif(f.ProductionType <> 'Packing' and sum(iif(f.ProductionType <> 'Packing', 1, 0)) = sum(iif(f.ProductionType <> 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
		    from 
		    (
			    select f.ProductionType
				    , psd.Complete
			    from PO_Supp_Detail psd WITH (NOLOCK)
			    inner join PO_Supp_Detail_OrderList psdo WITH (NOLOCK) on psd.ID = psdo.ID and psd.SEQ1 = psdo.SEQ1 and psd.SEQ2 = psdo.SEQ2
			    outer apply (
				    select [ProductionType] = iif(m.ProductionType = 'Packing', 'Packing', 'Sewing')
				    from Fabric f WITH (NOLOCK)
				    left join MtlType m WITH (NOLOCK) on f.MtlTypeID = m.ID
				    where f.SCIRefno = psd.SCIRefno
			    )f  
			    where psdo.OrderID	= o.ID
			    and psd.Junk = 0
		    )f
		    group by f.ProductionType
	    )f
    )CompltSP
    outer apply(
		    select InLine = MIN(w.EstCutDate),OffLine = MAX(w.EstCutDate) 
		    from WorkOrder_Distribute wd WITH (NOLOCK)
		    inner join WorkOrder w WITH (NOLOCK) on wd.WorkOrderUkey = w.Ukey
		    where w.EstCutDate is not null
		    and wd.OrderID = o.ID
	 )csp
	OUTER APPLY(
		SELECT [Val]=STUFF((
		    SELECT  DISTINCT ','+ Cast(CFAFinalInspectDate as varchar)
		    from Order_QtyShip oq WITH (NOLOCK)
		    WHERE ID = o.id
		    FOR XML PATH('')
		),1,1,'')
	)QtyShip_InspectDate
	OUTER APPLY(
		SELECT [Val]=STUFF((
		    SELECT  DISTINCT ','+ CFAFinalInspectResult
		    from Order_QtyShip oq WITH (NOLOCK)
		    WHERE ID = o.id AND CFAFinalInspectResult <> '' AND CFAFinalInspectResult IS NOT NULL
		    FOR XML PATH('')
		),1,1,'')
	)QtyShip_Result
	OUTER APPLY(
		SELECT [Val]=STUFF((
		SELECT  DISTINCT ','+ CFAFinalInspectHandle +'-'+ p.Name
		    from Order_QtyShip oq WITH (NOLOCK)
			LEFT JOIN Pass1 p WITH (NOLOCK) ON oq.CFAFinalInspectHandle = p.ID 
		    WHERE oq.ID = o.id AND CFAFinalInspectHandle <> '' AND CFAFinalInspectHandle IS NOT NULL
		    FOR XML PATH('')
		),1,1,'')
	)QtyShip_Handle
	outer apply(select oa.Article from Order_article oa WITH (NOLOCK) where oa.id = o.id) a

	CREATE NONCLUSTERED INDEX index_tmpOrders_ID ON #tmp_tmpOrders(	ID ASC);

	select ID, Remark
	into #tmp_PFRemark
	from (
		select ROW_NUMBER() OVER (PARTITION BY o.ID ORDER BY o.addDate, o.Ukey) r_id
			,o.ID, o.Remark
		from Order_PFHis o WITH (NOLOCK) 
		inner join #tmp_tmpOrders t on o.ID = t.ID 
		where AddDate = (
				select Max(o.AddDate) 
				from Order_PFHis o WITH (NOLOCK) 
				where ID = t.ID
			)   
		group by o.ID, o.Remark ,o.addDate, o.Ukey
	)a
	where r_id = '1'

	select StyleUkey, [GetStyleUkey] = dbo.GetSimilarStyleList(StyleUkey) 
	into #tmp_StyleUkey
	from #tmp_tmpOrders
	group by StyleUkey 

	select POID, [MTLDelay] = dbo.GetHaveDelaySupp(POID)
	into #tmp_MTLDelay
	from #tmp_tmpOrders
	group by POID 

	select pld.OrderID, [PackingQty] = SUM(pld.ShipQty)
	into #tmp_PackingQty
	from PackingList pl WITH (NOLOCK) 
	inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
	inner join #tmp_tmpOrders t on pld.OrderID = t.ID
	where  pl.Type <> 'F'  
	group by pld.OrderID  

	select pld.OrderID, [PackingFOCQty] = SUM(pld.ShipQty) 
	into #tmp_PackingFOCQty
	from PackingList pl WITH (NOLOCK) 
	inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
	inner join #tmp_tmpOrders t on pld.OrderID = t.ID
	where pl.Type = 'F' 
	group by pld.OrderID 

	select pld.OrderID, [BookingQty] = SUM(pld.ShipQty)
	into #tmp_BookingQty
	from PackingList pl WITH (NOLOCK) 
	inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
	inner join #tmp_tmpOrders t on pld.OrderID = t.ID
	where   (pl.Type = 'B' or pl.Type = 'S') 
			and pl.INVNo <> ''  
	group by pld.OrderID 
 
	select o.ID, o.Article 
	into #tmp_Article
	from Order_Qty o WITH (NOLOCK) 
	inner join #tmp_tmpOrders t on o.ID = t.ID
	group by o.ID,o.Article 

	CREATE NONCLUSTERED INDEX index_tmp_PFRemark ON #tmp_PFRemark(	ID ASC);
	CREATE NONCLUSTERED INDEX index_tmp_StyleUkey ON #tmp_StyleUkey(	StyleUkey ASC);
	CREATE NONCLUSTERED INDEX index_tmp_MTLDelay ON #tmp_MTLDelay(	POID ASC);
	CREATE NONCLUSTERED INDEX index_tmp_PackingQty ON #tmp_PackingQty(	OrderID ASC);
	CREATE NONCLUSTERED INDEX index_tmp_PackingFOCQty ON #tmp_PackingFOCQty(	OrderID ASC);
	CREATE NONCLUSTERED INDEX index_tmp_BookingQty ON #tmp_BookingQty(	OrderID ASC);

	select distinct 
				  t.ID
				, t.MDivisionID
				, t.FtyGroup
				, t.FactoryID
				, t.BuyerDelivery
				, t.SciDelivery
				, t.POID
				, t.CRDDate
				, t.CFMDate
				, t.Dest
				, t.StyleID
				, t.StyleName
				, t.SeasonID
				, t.BrandID
				, t.ProjectID
				, t.Kit
				,[PackingMethod] 
				, t.HangerPack
				, t.Customize1
				, t.BuyMonth
				, t.CustPONo
				, t.CustCDID
				, t.ProgramID
				, [NonRevenue]
				, t.CdCodeID
				, t.CDCodeNew
				, [ProductType]
				, t.[FabricType]
				, t.Lining
				, t.Gender
				, t.[Construction]
				, t.CPU
				, t.Qty
				, t.FOCQty
				, t.LocalOrder
				, t.PoPrice
				, t.CMPPrice
				, t.KPILETA
				, t.PFETA
				, t.LETA
				, t.MTLETA
				, t.SewETA
				, t.PackETA
				, t.MTLComplete
				, t.SewInLine
				, t.SewOffLine
				, t.CutInLine
				, t.CutOffLine
				, t.CutInLine_SP
				, t.CutOffLine_SP
				, t.Category
				, t.PulloutDate
				, t.ActPulloutDate
				, t.SMR
				, t.MRHandle
				, t.MCHandle
				, t.OrigBuyerDelivery
				, t.DoxType
				, t.TotalCTN
				, t.PackErrCTN
				, t.CFACTN
				, t.VasShas
				, t.TissuePaper
				, [MTLExport]
				, t.SewLine
				, t.ShipModeList
				, t.PlanDate
				, t.FirstProduction
				, t.LastProductionDate
				, t.OrderTypeID
				, t.SpecialMark
				, t.SampleReason
				, InspDate 
				, InspResult 
				, InspHandle 
				, t.MnorderApv2
				, t.MnorderApv
				, t.PulloutComplete
				, t.FtyKPI
				, t.KPIChangeReason
				, t.EachConsApv
				, t.Junk
				, t.StyleUkey
				, t.CuttingSP
				, t.RainwearTestPassed
				, t.BrandFTYCode
				, t.CPUFactor
				, t.ClogLastReceiveDate
				, [IsMixMarker]
				, t.GFR 
				, isForecast
				, [AirFreightByBrand]
				, [BuyBack]
				, [Cancelled]
				, t.Customize2
				, t.KpiMNotice
				, t.KpiEachConsCheck
				, t.[IDD] 
			, ModularParent = isnull (s.ModularParent, '')  
			, CPUAdjusted = isnull(s.CPUAdjusted * 100, 0)  
			, DestAlias = isnull (c.Alias, '') 
			, FactoryDisclaimer = isnull (dd.Name, '')
			, FactoryDisclaimerRemark = isnull (s.ExpectionFormRemark, '')
			, ApprovedRejectedDate  = s.ExpectionFormDate
			, WorkType = isnull(ct.WorkType,'')
			, ct.FirstCutDate
			, POSMR = isnull (p.POSMR, '')
			, POHandle = isnull (p.POHandle, '') 
			, PCHandle = isnull (p.PCHandle, '') 
			, FTYRemark = isnull (s.FTYRemark, '')
			, SewQtyTop = isnull ((select SUM(QAQty) 
								   from SewingOutput_Detail WITH (NOLOCK) 
								   where OrderId = t.ID 
										 and ComboType = 'T')
								 , 0)
			, SewQtyBottom = isnull ((select SUM(QAQty) 
									  from SewingOutput_Detail WITH (NOLOCK) 
									  where OrderId = t.ID 
											and ComboType = 'B')
									, 0)
			, SewQtyInner = isnull ((select SUM(QAQty) 
									 from SewingOutput_Detail WITH (NOLOCK) 
									 where OrderId = t.ID 
										   and ComboType = 'I')
								   , 0) 
			, SewQtyOuter = isnull ((select SUM(QAQty) 
									 from SewingOutput_Detail WITH (NOLOCK) 
									 where OrderId = t.ID 
										   and ComboType = 'O')
								   , 0)
			, TtlSewQty = isnull (dbo.getMinCompleteSewQty (t.ID, null, null), 0)
			, CutQty = isnull ((select SUM(Qty) 
								from CuttingOutput_WIP WITH (NOLOCK) 
								where OrderID = t.ID)
							  , 0)
			, PFRemark = isnull(pf.Remark,'')
			, EarliestSCIDlv = dbo.getMinSCIDelivery (t.POID, '')
			, KPIChangeReasonName = isnull ((select Name 
											 from Reason WITH (NOLOCK)  
											 where  ReasonTypeID = 'Order_BuyerDelivery' 
													and ID = t.KPIChangeReason)
											, '')
			, SMRName = isnull ((select Name 
								 from TPEPass1 WITH (NOLOCK) 
								 where Id = t.SMR)
								, '')
			, MRHandleName = isnull ((select Name 
									  from TPEPass1 WITH (NOLOCK) 
									  where Id = t.MRHandle)
									, '')
			, POSMRName = isnull ((select Name 
								   from TPEPass1 WITH (NOLOCK) 
								   where Id = p.POSMR)
								 , '')
			, POHandleName = isnull ((select Name 
									  from TPEPass1 WITH (NOLOCK) 
									  where Id = p.POHandle)
									, '')
			, PCHandleName = isnull ((select Name 
									  from TPEPass1 WITH (NOLOCK)
									  where Id = p.PCHandle)
									, '')
			, MCHandleName = isnull ((select Name 
									  from Pass1 WITH (NOLOCK) 
									  where Id = t.MCHandle)
									, '')
			, SampleReasonName = isnull ((select Name 
										  from Reason WITH (NOLOCK) 
										  where ReasonTypeID = 'Order_reMakeSample' 
												and ID = t.SampleReason)
										, '') 
			, SpecialMarkName = isnull ((select Name 
										 from Style_SpecialMark sp WITH(NOLOCK) 
										 where sp.ID = t.[StyleSpecialMark] 
										 and sp.BrandID = t.BrandID
										 and sp.Junk = 0)
									   , '')
			, MTLExportTimes = isnull ([dbo].getMTLExport (t.POID, t.MTLExport), '')
			, GMTLT = dbo.GetGMTLT(t.BrandID, t.StyleID, t.SeasonID, t.FactoryID, t.ID)
			, SimilarStyle = su.GetStyleUkey
			, MTLDelay = isnull(mt.MTLDelay,0)
			, PackingQty = isnull(pa.PackingQty ,0)
			, PackingFOCQty = isnull(paf.PackingFOCQty,0)
			, BookingQty = isnull(bo.BookingQty ,0)
			, InvoiceAdjQty = isnull (i.value, 0)
			, FOCAdjQty = isnull (i2.value, 0)
			, NotFOCAdjQty= isnull (i.value, 0)-isnull (i2.value, 0)
			, ct.LastCutDate
			, ArriveWHDate = (select Max(e.WhseArrival) 
							  from Export e WITH (NOLOCK) 
							  inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID 
							  where ed.POID = t.POID) 
			, FirstOutDate = (select Min(so.OutputDate) 
							  from SewingOutput so WITH (NOLOCK) 
							  inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
							  where sod.OrderID = t.ID) 
			, LastOutDate = (select Max(so.OutputDate) 
							 from SewingOutput so WITH (NOLOCK) 
							 inner join SewingOutput_Detail sod WITH (NOLOCK) on so.ID = sod.ID
							 where sod.OrderID = t.ID)
			, PulloutQty = isnull ((select sum(pd.ShipQty)
									from PackingList_Detail pd WITH (NOLOCK)
									inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
									where p.PulloutID <> ''
									and pd.OrderID = t.ID)
								  , 0)
			, ActPulloutTime = (select count(distinct p.PulloutID)
								from PackingList_Detail pd WITH (NOLOCK)
								inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
								where p.PulloutID <> ''
								and pd.OrderID = t.ID
								and pd.ShipQty > 0)
			, PackingCTN = isnull ((select Sum(CTNQty) 
									from PackingList_Detail WITH (NOLOCK) 
									where OrderID = t.ID)
								  , 0) 
			, FtyCtn = isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0)
			, ClogCTN = isnull(t.ClogCTN,0)
			, ClogRcvDate = t.ClogLastReceiveDate
			, Article = isnull ((select CONCAT(a.Article, ',') 
								 from #tmp_Article a 
								 where a.ID = t.ID
								 for xml path(''))
							   , '') 
			, [Fab_ETA]=(select max(FinalETA) F_ETA from PO_Supp_Detail WITH (NOLOCK) where id=p.ID  and FabricType='F')
			, [Acc_ETA]=(select max(FinalETA) A_ETA from PO_Supp_Detail WITH (NOLOCK) where id=p.ID  and FabricType='A')
			, [LastCTNTransDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.LastCTNTransDate, null)
			, [LastCTNRecdDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.LastCTNRecdDate, null)
			, [DryRoomRecdDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.DryRoomRecdDate, null)
			, [DryRoomTransDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.DryRoomTransDate, null)
			, [MdRoomScanDate] = IIF(isnull(t.TotalCTN,0) - isnull(t.FtyCTN,0) = 0 ,t.MdRoomScanDate, null)
			, t.VasShasCutOffDate
			, t.SewingMtlComplt
			, t.PackingMtlComplt
			, [OrganicCotton] = iif(t.OrganicCotton = 1, 'Y', 'N')
			, [DirectShip] = iif(t.DirectShip = 1, 'V','')
			, [StyleCarryover] = iif(s.NewCO = '2','V','')
			, t.PackLETA
	into #tmp_LastBase
	from #tmp_tmpOrders t
	left join Cutting ct WITH (NOLOCK) on ct.ID = t.CuttingSP
	left join Style s WITH (NOLOCK) on s.Ukey = t.StyleUkey
	left join DropDownList dd WITH (NOLOCK) on dd.Type = 'FactoryDisclaimer' and dd.id = s.ExpectionFormStatus
	left join PO p WITH (NOLOCK) on p.ID = t.POID
	left join Country c WITH (NOLOCK) on c.ID = t.Dest
	left join #tmp_PFRemark pf on pf.ID = t.ID
	left join #tmp_StyleUkey su on su.StyleUkey = t.StyleUkey 
	left join #tmp_MTLDelay mt on mt.POID = t.POID
	left join #tmp_PackingQty pa on pa.OrderID = t.ID
	left join #tmp_PackingFOCQty paf on paf.OrderID = t.ID
	left join #tmp_BookingQty bo on bo.OrderID = t.ID
	outer apply(
		select value = sum(iq.DiffQty) 
		from InvAdjust i WITH (NOLOCK) 
		inner join InvAdjust_Qty iq WITH (NOLOCK) on i.ID = iq.ID
		where i.OrderID = t.ID
	)i
	outer apply(
		select value = sum(iq.DiffQty) 
		from InvAdjust i WITH (NOLOCK) 
		inner join InvAdjust_Qty iq WITH (NOLOCK) on i.ID = iq.ID
		where i.OrderID = t.ID
		and iq.Price = 0
	)i2
	order by t.ID;


	select  *
			, rno = (ROW_NUMBER() OVER (ORDER BY a.ID, a.ColumnSeq))
	into #tmp_SubProcess
	from (
        SELECT  ID
                , Seq
                , ArtworkUnit
                , ProductionUnit
                , SystemType
                , FakeID = Seq + 'U1'
                , ColumnN = RTRIM(ID) + ' ('+ArtworkUnit+')'
                , ColumnSeq = '1'
        FROM ArtworkType WITH (NOLOCK) 
        WHERE   ArtworkUnit <> '' 
                and Classify in ('I','S','A','O') 
        
        union all
        SELECT  ID
                , Seq
                , ArtworkUnit
                , ProductionUnit=p.PUnit
                , SystemType
                , FakeID = Seq + 'U2'
                , ColumnN = RTRIM(ID) + ' ('+IIF(ProductionUnit = 'QTY','Price',p.PUnit)+')'
                , ColumnSeq = '2' 
        FROM ArtworkType WITH (NOLOCK) 
		outer apply(select  PUnit=iif('False'='true' and ProductionUnit = 'TMS','CPU',ProductionUnit))p
        WHERE   ProductionUnit <> '' 
                and Classify in ('I','S','A','O') 
				and ID<> 'PRINTING PPU' 
        
        union all
        SELECT  ID
                , Seq
                , ArtworkUnit
                , ProductionUnit
                , SystemType
                , FakeID = Seq + 'N'
                , ColumnN = RTRIM(ID)
                , ColumnSeq = '3'
        FROM ArtworkType WITH (NOLOCK) 
        WHERE   ArtworkUnit = '' 
                and ProductionUnit = '' 
                and Classify in ('I','S','A','O') 
        
    union all
    SELECT  ID = 'PrintSubCon'
            , Seq = ''
            , ArtworkUnit = '' 
            , ProductionUnit = '' 
            , SystemType = ''
            , FakeID = '9999ZZ'
            , ColumnN = 'POSubCon'
            , ColumnSeq = '998'
	union all
    SELECT  ID = 'PrintSubCon'
            , Seq = ''
            , ArtworkUnit = '' 
            , ProductionUnit = '' 
            , SystemType = ''
            , FakeID = '9999ZZ'
            , ColumnN = 'SubCon'
            , ColumnSeq = '999'
        
    ) a

    select  ID = 'TTL' + ID 
            , Seq
            , ArtworkUnit
            , ProductionUnit
            , SystemType
            , FakeID = 'T' + FakeID
            , ColumnN = 'TTL_' + ColumnN
            , ColumnSeq
            , rno = (ROW_NUMBER() OVER (ORDER BY ID, ColumnSeq)) + 1000
	into #tmp_TTL_Subprocess
    from #tmp_SubProcess
    where ID <> 'PrintSubCon' and ColumnN <> 'Printing LT' and ColumnN <> 'InkType/color/size'

	select  ID
			, Seq
			, ArtworkUnit
			, ProductionUnit
			, SystemType
			, FakeID
			, ColumnN
			, ColumnSeq
			, rno = (ROW_NUMBER() OVER (ORDER BY a.rno)) + 153
	into #tmp_ArtworkData
	from (
		select * 
		from #tmp_SubProcess

		union all
		SELECT  ID = 'TTLTMS'
				, Seq = ''
				, ArtworkUnit = '' 
				, ProductionUnit = '' 
				, SystemType = '' 
				, FakeID = 'TTLTMS'
				, FakeID = 'TTL_TMS'
				, ColumnSeq = '' 
				, rno = '999'
		union
		select * 
		from #tmp_TTL_Subprocess
	) a

	select  ot.ID
			, ot.ArtworkTypeID
			, ot.ArtworkUnit
			, ProductionUnit=p.PUnit
			, isnull(ot.Qty,0) Qty 
			, ot.TMS
			, isnull(ot.Price,0) Price
			, Supp = IIF(ot.ArtworkTypeID = 'PRINTING', IIF(ot.InhouseOSP = 'O', (select Abb 
																				  from LocalSupp WITH (NOLOCK) 
																				  where ID = LocalSuppID)
																			   , ot.LocalSuppID)
													  , '')
			, PoSupp = IIF(ot.ArtworkTypeID = 'PRINTING', IIF(ot.InhouseOSP = 'O', ap.Abb, ot.LocalSuppID) , '')
			, AUnitRno = a.rno 
			, PUnitRno = iif(ot.ArtworkTypeID='PRINTING PPU', a.rno , a1.rno)
			, NRno = a2.rno
			, TAUnitRno = a3.rno
			, TPUnitRno = iif(ot.ArtworkTypeID='PRINTING PPU', a3.rno, a4.rno )
			, TNRno = a5.rno 
	into #tmp_LastArtworkType
	from Order_TmsCost ot WITH (NOLOCK) 
	left join ArtworkType at WITH (NOLOCK) on at.ID = ot.ArtworkTypeID
	left join #tmp_ArtworkData a on a.FakeID = ot.Seq+'U1' 
	left join #tmp_ArtworkData a1 on a1.FakeID = ot.Seq+'U2'
	left join #tmp_ArtworkData a2 on a2.FakeID = ot.Seq
	left join #tmp_ArtworkData a3 on a3.FakeID = 'T'+ot.Seq+'U1' 
	left join #tmp_ArtworkData a4 on a4.FakeID = 'T'+ot.Seq+'U2'
	left join #tmp_ArtworkData a5 on a5.FakeID = 'T'+ot.Seq 
	outer apply(select  PUnit=iif('False'='true' and at.ProductionUnit = 'TMS','CPU',at.ProductionUnit))p
	outer apply(
		select Abb = Stuff((
				select distinct concat( ',', l.Abb)   
				from ArtworkPO ap WITH (NOLOCK)
				inner join ArtworkPO_Detail apd WITH (NOLOCK) on ap.ID = apd.ID
				left join LocalSupp l on ap.LocalSuppID = l.ID
				where ap.ArtworkTypeID = 'PRINTING'
				and apd.OrderID = ot.ID
			FOR XML PATH('')),1,1,'') 
	)ap
	where exists (select id from #tmp_tmpOrders t where ot.ID = t.ID )

	select *
	into #tmp_ArtworkTypeValue
	from 
	(
		select a.ID
			, [ColumnN] = AUnitRno.ColumnN
			, [Val] = cast(case when a.AUnitRno is not null then a.Qty
						   else null end as varchar(100))
		from #tmp_LastArtworkType a
		inner join #tmp_ArtworkData AUnitRno on a.AUnitRno = AUnitRno.rno
		union all
		select a.ID
			, [ColumnN] = PUnitRno.ColumnN
			, [Val] = cast(case when a.PUnitRno is not null then 
									case when a.ProductionUnit = 'TMS' then a.TMS  
									else a.Price end 
							else null end as varchar(100))
		from #tmp_LastArtworkType a
		inner join #tmp_ArtworkData PUnitRno on a.PUnitRno = PUnitRno.rno
		union all
		select a.ID
			, [ColumnN] = NRno.ColumnN
			, [Val] = cast(case when a.NRno is not null then a.Qty
					   else null end as varchar(100))
		from #tmp_LastArtworkType a
		inner join #tmp_ArtworkData NRno on a.NRno = NRno.rno
		union all
		select a.ID
			, [ColumnN] = TAUnitRno.ColumnN
			, [Val] = cast(case when a.TAUnitRno is not null then b.Qty * a.Qty
							else null end as varchar(100))
		from #tmp_LastArtworkType a
		inner join #tmp_ArtworkData TAUnitRno on a.TAUnitRno = TAUnitRno.rno
		left join #tmp_LastBase b on a.ID = b.ID
		union all
		select a.ID
			, [ColumnN] = TPUnitRno.ColumnN
			, [Val] = cast(case when a.TPUnitRno is not null then 
								case when a.ProductionUnit = 'TMS' then b.Qty * a.TMS  
								else b.Qty * a.Price end 
							else null end as varchar(100))
		from #tmp_LastArtworkType a
		inner join #tmp_ArtworkData TPUnitRno on a.TPUnitRno = TPUnitRno.rno
		left join #tmp_LastBase b on a.ID = b.ID
		union all
		select a.ID
			, [ColumnN] = TNRno.ColumnN
			, [Val] = cast(case when a.TNRno is not null then b.Qty * a.Qty
						 else null end as varchar(100))
		from #tmp_LastArtworkType a
		inner join #tmp_ArtworkData TNRno on a.TNRno = TNRno.rno
		left join #tmp_LastBase b on a.ID = b.ID
		union all
		select a.ID
			, [ColumnN] = 'POSubCon'
			, [Val] = a.PoSupp
		from #tmp_LastArtworkType a
		where ISNULL(a.PoSupp, '') <> ''
		union all
		select a.ID
			, [ColumnN] = 'SubCon'
			, [Val] = a.Supp
		from #tmp_LastArtworkType a
		where ISNULL(a.Supp, '') <> ''
		union all
		select b.ID
			, [ColumnN] = 'TTL_TMS'
			, [Val] = cast(b.Qty * b.CPU * @StdTMS as varchar(100))
		from #tmp_LastBase b
	) a

	drop table #tmp_Article, #tmp_BookingQty, #tmp_LastArtworkType, #tmp_MTLDelay, 
		#tmp_PackingFOCQty, #tmp_PackingQty, #tmp_PFRemark, #tmp_StyleUkey, 
		#tmp_SubProcess, #tmp_tmpOrders, #tmp_TTL_Subprocess

	select [M] = b.MDivisionID
		, [FactoryID] = b.FactoryID
		, [Delivery] = b.BuyerDelivery
		, [Delivery(YYYYMM)] = FORMAT(b.BuyerDelivery, 'yyyyMM')
		, [Earliest SCIDlv] = b.EarliestSCIDlv
		, [SCIDlv] = b.SciDelivery
		, [KEY] = case when CAST(FORMAT(b.SciDelivery, 'dd') as int) <= 7 then FORMAT(DATEADD(MONTH, -1, b.SciDelivery), 'yyyyMM') else FORMAT(b.SciDelivery, 'yyyyMM') end
		, [IDD] = b.IDD
		, [CRD] = b.CRDDate
		, [CRD(YYYYMM)] = FORMAT(b.CRDDate, 'yyyyMM')
		, [Check CRD] = case when b.BuyerDelivery is null or b.CRDDate is null then 'Y'
							when b.BuyerDelivery <> b.CRDDate then 'Y' 
							else '' end
		, [OrdCFM] = b.CFMDate
		, [CRD-OrdCFM] = case when b.CRDDate is null or b.CFMDate is null then 0 else DATEDIFF(day, b.CRDDate, b.CFMDate) end
		, [SPNO] = b.ID
		, [Category] = b.Category
		, [Est. download date] = case when b.isForecast = '' then '' else b.BuyMonth end
		, [Buy Back] = b.BuyBack
		, [Cancelled] = case when b.Junk = 1 then 'Y' else '' end
		, [NeedProduction] = b.Cancelled
		, [Dest] = b.DestAlias
		, [Style] = b.StyleID
		, [Style Name] = b.StyleName
		, [Modular Parent] = b.ModularParent
		, [CPUAdjusted] = b.CPUAdjusted
		, [Similar Style] = b.SimilarStyle
		, [Season] = b.SeasonID
		, [Garment L/T] = b.GMTLT
		, [Order Type] = b.OrderTypeID
		, [Project] = b.ProjectID
		, [PackingMethod] = b.PackingMethod
		, [Hanger pack] = b.HangerPack
		, [Order#] = b.Customize1
		, [Buy Month] = case when b.isForecast = '' then b.BuyMonth else '' end
		, [PONO] = b.CustPONo
		, [VAS/SHAS] = case when b.VasShas = 1 then 'Y' else '' end
		, [VAS/SHAS Apv.] = b.MnorderApv2
		, [VAS/SHAS Cut Off Date] = b.VasShasCutOffDate
		, [M/Notice Date] = b.MnorderApv
		, [Est M/Notice Apv.] = b.KPIMNotice
		, [Tissue] = case when b.TissuePaper =1 then 'Y' else '' end 
		, [AF by adidas] = b.AirFreightByBrand
		, [Factory Disclaimer] = b.FactoryDisclaimer
		, [Factory Disclaimer Remark] = b.FactoryDisclaimerRemark
		, [Approved/Rejected Date] = b.ApprovedRejectedDate
		, [Global Foundation Range] = case when b.GFR = 1 then 'Y' else '' end
		, [Brand] = b.BrandID
		, [Cust CD] = b.CustCDID
		, [KIT] = b.Kit
		, [Fty Code] = b.BrandFTYCode
		, [Program ] = b.ProgramID
		, [Non Revenue] = b.NonRevenue
		, [New CD Code] = b.CDCodeNew
		, [ProductType] = b.ProductType
		, [FabricType] = b.FabricType
		, [Lining] = b.Lining
		, [Gender] = b.Gender
		, [Construction] = b.Construction
		, [Cpu] = b.CPU
		, [Qty] = b.Qty
		, [FOC Qty] = b.FOCQty
		, [Total CPU] = b.CPU * b.Qty * b.CPUFactor
		, [SewQtyTop] = b.SewQtyTop
		, [SewQtyBottom] = b.SewQtyBottom
		, [SewQtyInner] = b.SewQtyInner
		, [SewQtyOuter] = b.SewQtyOuter
		, [Total Sewing Output] = b.TtlSewQty
		, [Cut Qty] = b.CutQty
		, [By Comb] = case when b.WorkType = 1 then 'Y' else '' end
		, [Cutting Status] = case when b.CutQty >= b.Qty then 'Y' else '' end
		, [Packing Qty] = b.PackingQty
		, [Packing FOC Qty] = b.PackingFOCQty
 		, [Booking Qty] = b.BookingQty
		, [FOC Adj Qty] = b.FOCAdjQty
		, [Not FOC Adj Qty] = b.NotFOCAdjQty -- 73
		, [FOB] = b.PoPrice
		, [Total] = b.Qty * b.PoPrice
		, [KPI L/ETA] = b.KPILETA  --BG 76
		, [PF ETA (SP)] = b.PFETA
		, [Pull Forward Remark] = b.PFRemark
		, [Pack L/ETA] = b.PackLETA
		, [SCHD L/ETA] = b.LETA
		, [Actual Mtl. ETA] = b.MTLETA
		, [Fab ETA] = b.Fab_ETA
		, [Acc ETA] = b.Acc_ETA
		, [Sewing Mtl Complt(SP)] = b.SewingMtlComplt
		, [Packing Mtl Complt(SP)] = b.PackingMtlComplt
		, [Sew. MTL ETA (SP)] = b.SewETA
		, [Pkg. MTL ETA (SP)] = b.PackETA
		, [MTL Delay] = case when b.MTLDelay = 1 then 'Y' else '' end
		, [MTL Cmplt] = case when isnull(b.MTLExport, '') = '' then b.MTLExportTimes else b.MTLExport end
		, [MTL Cmplt (SP)] = case when b.MTLComplete = 1 then 'Y' else '' end
		, [Arrive W/H Date] = b.ArriveWHDate
		, [Sewing InLine] =  b.SewInLine
		, [Sewing OffLine] = b.SewOffLine
		, [1st Sewn Date] = b.FirstOutDate
		, [Last Sewn Date] = b.LastOutDate
		, [First Production Date] = b.FirstProduction
		, [Last Production Date] = b.LastProductionDate
		, [Each Cons Apv Date] = b.EachConsApv
		, [Est Each Con Apv.] = b.KpiEachConsCheck
		, [Cutting InLine] = b.CutInLine
		, [Cutting OffLine] = b.CutOffLine
		, [Cutting InLine(SP)] = b.CutInLine_SP
		, [Cutting OffLine(SP)] = b.CutOffLine_SP
		, [1st Cut Date] = b.FirstCutDate
		, [Last Cut Date] = b.LastCutDate
		, [Est. Pullout] = b.PulloutDate
		, [Act. Pullout Date] = b.ActPulloutDate
		, [Pullout Qty] = b.PulloutQty
		, [Act. Pullout Times] = b.ActPulloutTime
		, [Act. Pullout Cmplt] = case when b.PulloutComplete = 1 then 'OK' else '' end
		, [KPI Delivery Date] = b.FtyKPI
		, [Update Delivery Reason] = case when ISNULL(b.KPIChangeReason, '') = '' then '' else concat(b.KPIChangeReason, '-', b.KPIChangeReasonName) end
		, [Plan Date] = b.PlanDate
		, [Original Buyer Delivery Date] = b.OrigBuyerDelivery
		, [SMR] = b.SMR
		, [SMR Name] = b.SMRName
		, [Handle] = b.MRHandle
		, [Handle Name] = b.MRHandleName
		, [Posmr] = b.POSMR
		, [Posmr Name] = b.POSMRName
		, [PoHandle] = b.POHandle
		, [PoHandle Name] = b.POHandleName
		, [PCHandle] = b.PCHandle
		, [PCHandle Name] = b.PCHandleName
		, [MCHandle] = b.MCHandle
		, [MCHandle Name] = b.MCHandleName
		, [DoxType] = b.DoxType
		, [Packing CTN] = b.PackingCTN
		, [TTLCTN] = b.TotalCTN
		, [Pack Error CTN] = b.PackErrCTN
		, [FtyCTN] = b.FtyCtn
		, [cLog CTN] = b.ClogCTN
		, [CFA CTN] = b.CFACTN
		, [cLog Rec. Date] = b.ClogRcvDate
		, [Final Insp. Date] = b.InspDate
		, [Insp. Result] = b.InspResult
		, [CFA Name] = b.InspHandle
		, [Sewing Line#] = b.SewLine
		, [ShipMode] = b.ShipModeList
		, [SI#] = b.Customize2
		, [ColorWay] = b.Article
		, [Special Mark] = b.SpecialMarkName
		, [Fty Remark] = b.FTYRemark
		, [Sample Reason] = b.SampleReasonName
		, [IS MixMarker] = b.IsMixMarker
		, [Cutting SP] = b.CuttingSP
		, [Rainwear test] = case when b.RainwearTestPassed = 1 then 'Y' else '' end
		, [TMS] = b.CPU * @StdTMS
		, [MD room scan date] = b.LastCTNTransDate
		, [Dry Room received date] = b.LastCTNRecdDate
		, [Dry room trans date] = b.DryRoomRecdDate
		, [Last ctn trans date] = b.DryRoomTransDate
		, [Last ctn recvd date] = b.MdRoomScanDate
		, [OrganicCotton] = b.OrganicCotton
		, [Direct Ship] = b.DirectShip		
		, [StyleCarryover] = b.StyleCarryover
		, t.ColumnN
		, t.Val
	into #tmp_final
	from #tmp_LastBase b
	left join #tmp_ArtworkTypeValue t on b.ID = t.ID

	declare @cols nvarchar(max) = stuff((select concat(',[',ColumnN,']') from #tmp_ArtworkData group by ColumnN, rno order by rno for xml path('')),1,1,'')
		, @query AS NVARCHAR(MAX)	

	SET @query = 'SELECT * FROM ( SELECT * FROM #tmp_final t ) src
				  PIVOT ( MAX(Val) FOR ColumnN IN (' + @cols + ') ) piv'
	EXECUTE(@query)

	drop table #tmp_ArtworkData, #tmp_ArtworkTypeValue, #tmp_final, #tmp_LastBase, #tmp_Orders_base
end
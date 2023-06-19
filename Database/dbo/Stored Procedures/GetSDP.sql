CREATE PROCEDURE [dbo].[GetSDP]
	@param1 int = 0,
	@param2 int
AS
begin
	SET NOCOUNT ON;	
	declare @Date_S date	
	declare @Date_E date

	set @Date_S = CONVERT(date, DATEADD(DAY,-30,GETDATE()))
	set @Date_E = GETDATE()
	
	/************************** 主表資訊 **************************/ 
	declare @tmp_main TABLE
	(
		[CountryID]				[varchar](30)	null
		,[KPICode]				[varchar](30)	null
		,[FactoryID]			[varchar](30)	null
		,[OrderID]				[varchar](30)	null
		,[StyleID]				[varchar](30)	null
		,[Seq]					[varchar](8)	null
		,[BrandID]				[varchar](30)	null
		,[BuyerDelivery]		[date]			
		,[FtyKPI]				[datetime]			
		,[ShipmodeID]			[varchar](30)	null
		,[OTDExtension]			[int]			null
		,[DeliveryByShipmode]	[varchar](30)	null
		,[OrderQty]				[int]			null
		,[Shipmode]				[varchar](30)	null
		,[GMTComplete]			[varchar](30)	null
		,[ReasonID]				[varchar](30)	null
		,[ReasonName]			[nvarchar](500)	null
		,[MRHandle]				[varchar](50)	null
		,[SMR]					[varchar](50)	null
		,[POHandle]				[varchar](50)	null
		,[POSMR]				[varchar](50)	null
		,[OrderTypeID]			[varchar](30)	null
		,[isDevSample]			[bit]			null				
		,[alias]				[varchar](30)	null
		,[MDivisionID]			[varchar](30)	null
		,[localorder]			[bit]			null
		,[OutsdReason]			[nvarchar](500)	null
		,[ReasonRemark]			[nvarchar](500)	null
		,[OnsiteSample]			[bit]			null
		,[CFAFinalInspectDate]	[date]			
		,[CFAFinalInspectResult][varchar](30)	null
		,[CFA3rdInspectDate]	[date]			
		,[CFA3rdInspectResult]	[varchar](30)	null
		,[IDDReason]			[nvarchar](max)	null
		,[Dest]					[varchar](30)	null
		,[CustPONo]				[varchar](30)	null
		,UNIQUE NONCLUSTERED ([OrderID], [Seq])
	)
	INSERT INTO @tmp_main
	SELECT
	CountryID = F.CountryID
	,KPICode = F.KPICode
	,FactoryID = o.FactoryID
	,OrderID = o.ID
	,o.StyleID
	,Seq = Order_QS.seq
	,BrandID = o.BrandID
	,Order_QS.BuyerDelivery
	,Order_QS.FtyKPI 
	,Order_QS.ShipmodeID
	,b.OTDExtension 
	,DeliveryByShipmode = Order_QS.ShipmodeID
	,OrderQty = Cast(Order_QS.QTY as int)										
	,Shipmode = Order_QS.ShipmodeID
	,GMTComplete = CASE o.GMTComplete WHEN 'C' THEN 'Y' 
									  WHEN 'S' THEN 'S' 
									  ELSE '' END
	,Order_QS.ReasonID
	,ReasonName = case o.Category when 'B' then r.Name
								  when 'S' then rs.Name
								  else '' end
	,o.MRHandle
	,o.SMR
	,PO.POHandle
	,PO.POSMR
	,o.OrderTypeID
	,ot.isDevSample				
	,c.alias
	,o.MDivisionID 
	,o.localorder
	, OutsdReason = rd.Name
	, ReasonRemark = o.OutstandingRemark
	,o.OnsiteSample
	,[CFAFinalInspectDate]=format(Order_QS.CFAFinalInspectDate, 'yyyy/MM/dd')
	,Order_QS.CFAFinalInspectResult
	,[CFA3rdInspectDate]=format(Order_QS.CFA3rdInspectDate, 'yyyy/MM/dd')
	,Order_QS.CFA3rdInspectResult
	,[IDDReason] = cr.Description
	,o.Dest
	,o.CustPONo
	FROM [Orders] o WITH (NOLOCK)
	LEFT JOIN [OrderType] ot with(nolock) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID and o.BrandID = ot.BrandID
	LEFT JOIN [Factory] f with(nolock) ON o.FACTORYID = f.ID
	LEFT JOIN [Country] c with(nolock) ON F.COUNTRYID = c.ID 
	inner JOIN [Order_QtyShip] Order_QS with(nolock) on Order_QS.id = o.id
	LEFT JOIN [PO] ON o.POID = PO.ID
	LEFT JOIN [Reason] r with(nolock) on r.id = Order_QS.ReasonID and r.ReasonTypeID = 'Order_BuyerDelivery'          
	LEFT JOIN [Reason] rs with(nolock) on rs.id = Order_QS.ReasonID and rs.ReasonTypeID = 'Order_BuyerDelivery_sample'
	Left join [Reason] rd with(nolock) on rd.id = o.OutstandingReason and rd.ReasonTypeID = 'Delivery_OutStand'
	LEFT JOIN [Brand] b with(nolock) on o.BrandID = b.ID
	LEFT JOIN [ClogReason] cr with(nolock) ON cr.ID = Order_QS.ClogReasonID  AND cr.Type='ID'
	where o.Junk = 0  
	and (isnull(ot.IsGMTMaster,0) = 0 or o.OrderTypeID = '') 
	and o.LocalOrder <> 1
	and o.IsForecast <> 1
	and exists (select 1 from [MainServer].[Production].[dbo].Factory where o.FactoryId = id and IsProduceFty = 1)
	and o.Category = 'B' 
	and f.[Type] = 'B'
	and 
	(
		(o.EditDate is not null and o.EditDate between @Date_S and @Date_E) 
		OR 
		(o.EditDate is null and o.AddDate between @Date_S and @Date_E)
		OR 
		(Order_QS.EditDate is not null and Order_QS.EditDate between @Date_S and @Date_E) 
		OR
		(Order_QS.EditDate is null and Order_QS.AddDate between @Date_S and @Date_E)
	)
	
	/************************** tmp_Pullout_Detail 資訊 **************************/
	declare @tmp_Pullout_Detail_main TABLE 
	(
		[countPulloutDate]	[date]			null,
		[OrderID]			[varchar](30)	null,
		[OrderShipmodeSeq]	[varchar](8)	null,
		[maxPulloutDate]	[date]		null,
		[Ukey]				[bigint]		null,
		[OTDExtension]		[int]			null,
		[BuyerDelivery]		[date]			null,
		[Pulloutdate]		[date]			null,
		[ShipQty]			[int]			null
	)
	INSERT INTO @tmp_Pullout_Detail_main
	select 
	countPulloutDate = pd.pulloutdate,
	OrderID = pd.OrderID,
	OrderShipmodeSeq = pd.OrderShipmodeSeq,
	maxPulloutDate =p.PulloutDate,
	pd.UKey,
	t.OTDExtension,
	t.BuyerDelivery,
	pd.pulloutdate,
	pdd.ShipQty
	from [Pullout_Detail] pd with(nolock)
	inner join @tmp_main t  on  pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq 
	INNER JOIN [Pullout] p with(nolock) ON p.Id=pd.id AND p.PulloutDate=pd.PulloutDate  
	inner join Pullout_Detail_Detail pdd on pd.Ukey = pdd.Pullout_DetailUKey 

	declare @tmp_Pullout_Detail_pd TABLE
	(
		[countPulloutDate]	[int]			null,
		[OrderID]			[varchar](30)	null,
		[OrderShipmodeSeq]	[varchar](8)	null,
		[maxPulloutDate]	[date]		null,
		[Qty]				[varchar](30)	null,
		[FailQty]			[varchar](30)	null
	)
	INSERT INTO @tmp_Pullout_Detail_pd
	select 
	countPulloutDate = count(main.countPulloutDate),
	OrderID = main.OrderID,
	OrderShipmodeSeq = main.OrderShipmodeSeq,
	maxPulloutDate =  max(main.maxPulloutDate),
	Qty = sum(IIF(main.pulloutdate <= DATEADD(day, isnull(main.OTDExtension,0), main.BuyerDelivery), main.ShipQty, 0)),
	FailQty = sum(IIF(main.pulloutdate >  DATEADD(day, isnull(main.OTDExtension,0), main.BuyerDelivery), main.shipqty, 0))
	from @tmp_Pullout_Detail_main main
	Group by main.OrderID,main.OrderShipmodeSeq
	--/************************** tmp_SewingOutput 資訊 **************************/
	declare @tmp_SewingOutput TABLE
	(
		[SewouptQty]		[int]			null
		,[SewLastDate]		[date]			null
		,[OrderID]			[varchar](30)	null
	)
	INSERT INTO @tmp_SewingOutput
	select 
	SewouptQty = sum(x.QaQty)
	,SewLastDate = CONVERT(date,max(SewLastDate))
	,OrderID
	from(
		Select OrderID, Article, SizeCode, 
			Min(QaQty) as QaQty,
			Min(SewLastDate) as SewLastDate
		From (
			Select ComboType, t.OrderID, Article, SizeCode, QaQty = Sum(SewingOutput_Detail_Detail.QaQty), Max(OutputDate) as SewLastDate
			From [SewingOutput_Detail_Detail]
			inner join (select distinct OrderID from @tmp_main) t on SewingOutput_Detail_Detail.OrderID= t.OrderID
			inner join [SewingOutput] on SewingOutput_Detail_Detail.ID = SewingOutput.ID 
			Group by ComboType, t.OrderID, Article, SizeCode
		) as sdd
		Group by OrderID, Article, SizeCode
	)x
	group by OrderID
	/************************** tmp_getQtyBySeq 資訊 **************************/
	declare @tmp_getQtyBySeq TABLE
	(
		[ID]				[varchar](30)	null
		,[Seq]				[varchar](30)	null
		,[FOB]				[int]			null
		,[FOC]				[int]			null
	)
	INSERT INTO @tmp_getQtyBySeq
	SELECT 
	oqsD.ID
	,oqsD.Seq
	,SUM(CASE WHEN poPrice > 0 THEN oqsD.Qty ELSE 0 END) AS FOB
	,SUM(CASE WHEN poPrice = 0 THEN oqsD.Qty ELSE 0 END) AS FOC
	FROM Order_QtyShip_Detail oqsD
	INNER JOIN @tmp_main t ON t.OrderID = oqsD.ID AND t.Seq = oqsD.Seq
	INNER JOIN (
		SELECT ID, Seq,Article, SizeCode, dbo.GetPoPriceByArticleSize(id, Article, SizeCode) AS poPrice
		FROM Order_QtyShip_Detail oqsD
	) po ON t.OrderID = po.ID AND t.Seq = po.Seq and oqsD.Article = po.Article and oqsD.SizeCode = po.SizeCode
	GROUP BY oqsD.ID, oqsD.Seq
	order by oqsD.ID desc,oqsD.Seq asc 

	/************************** tmp_ClogReceive 資訊 **************************/
	declare @CReceive_UNIONALL TABLE
	(
		[OrderID]			[varchar](30)	null
		,[Seq]				[varchar](30)	null
		,[AddDate]			[datetime]		null
	)
	INSERT INTO @CReceive_UNIONALL
	select distinct
	pld.OrderID,
	OrderShipmodeSeq,
	c.AddDate
	from PackingList_Detail pld with(nolock)
	inner join @tmp_main t on t.OrderID = pld.OrderID
	inner join ClogReceive c WITH (NOLOCK) on pld.ID = c.PackingListID and pld.OrderID = c.OrderID and pld.CTNStartNo = c.CTNStartNo
	where 
	CTNQty > 0
	and c.PackingListID != ''
	and c.OrderID != ''
	and c.CTNStartNo != ''
	union all -- 找拆箱
	select OrderID = pd.OrigOrderID, OrderShipmodeSeq, c.AddDate
	from PackingList_Detail pd with(nolock)
	inner join ClogReceive c WITH (NOLOCK) on pd.OrigID = c.PackingListID and pd.OrigOrderID = c.OrderID and pd.OrigCTNStartNo = c.CTNStartNo
	inner join @tmp_main t on t.OrderID = pd.OrderID
	where 
	CTNQty > 0
	and c.PackingListID != ''
	and c.OrderID != ''
	and c.CTNStartNo != ''


	declare @CReceive TABLE
	(
		[OrderID]					[varchar](30)	null
		,[Seq]				[varchar](30)	null　	
		,[AddDate]					[datetime]		null
		,UNIQUE NONCLUSTERED ([OrderID], [Seq]) 
	)
	INSERT INTO @CReceive
	select 
	t.OrderID, 
	t.Seq, 
	AddDate = MAX(AddDate)
	from @CReceive_UNIONALL t
	inner join @tmp_main c on c.OrderID = t.OrderID and c.Seq = t.Seq
	where not exists 
	(
		-- 每個紙箱必須放在 Clog（ReceiveDate 有日期）
		select 1 
		from Production.dbo.PackingList_Detail pdCheck with(nolock)
		where 
		t.OrderID = pdCheck.OrderID 
		and t.Seq = pdCheck.OrderShipmodeSeq
		and pdCheck.ReceiveDate is null
	)
	group by t.OrderID, t.Seq

	declare @tmp_ClogReceive TABLE
	(
		[OrderID]						[varchar](30)	null
		,[ordershipmodeseq]				[varchar](30)	null
		,[CTNLastReceiveDate]			[datetime]		null
	)
	INSERT INTO @tmp_ClogReceive
	SELECT  
	[orderid] = t.OrderID
	,[ordershipmodeseq] = t.Seq
	,[CTNLastReceiveDate] = c.AddDate
	from @tmp_main t 
	inner join @CReceive c on c.OrderID = t.OrderID and c.Seq = t.Seq
	UNION
	SELECT  
	[orderid] = t.OrderID
	,[ordershipmodeseq] = t.Seq
	,[CTNLastReceiveDate] = NULL
	from @tmp_main t 
	where NOT exists
	(
		select 1 from @CReceive c where c.OrderID = t.OrderID and c.Seq = t.Seq
	)

	/************************** 整合資料 **************************/
	declare @tmp TABLE
	(
				[Country]					[varchar](30)	null
		,[KPIGroup]					[varchar](30)	null
		,[FactoryID]				[varchar](30)	null
		,[SPNO]						[varchar](30)	null
		,[Style]					[varchar](30)	null
		,[Seq]						[varchar](30)	null
		,[Brand]					[varchar](30)	null
		,[BuyerDelivery]			[date]			null
		,[FactoryKPI]				[date]			null
		,[Extension]				[date]			null
		,[DeliveryByShipmode]		[varchar](30)	null
		,[OrderQty]					[int]			null
		,[OnTimeQty]				[int]			null
		,[FailQty]					[int]			null
		,[ClogRec_OnTimeQty]		[int]			null
		,[ClogRec_FailQty]			[int]			null
		,[PullOutDate]				[date]			null
		,[Shipmode]					[varchar](30)	null
		,[Pullouttimes]				[int]			null
		,[GarmentComplete]			[varchar](30)	null
		,[ReasonID]					[varchar](30)	null
		,[OrderReason]				[nvarchar](500)	null
		,[Handle]					[varchar](60)	null
		,[SMR]						[varchar](50)	null
		,[POHandle]					[varchar](50)	null
		,[POSMR]					[varchar](50)	null
		,[OrderType]				[varchar](50)	null
		,[DevSample]				[varchar](30)	null
		,[SewingQty]				[int]			null
		,[FOCQty]					[int]			null
		,[LastSewingOutputDate]		[date]			null
		,[LastCartonReceivedDate]	[dateTime]		null
		,[IDDReason]				[varchar](20)	null
		,[PartialShipment]			[varchar](5)	null
		,[Alias]					[varchar](20)	null
		,[CFAInspectionDate]		[date]			null
		,[CFAFinalInspectionResult]	[varchar](50)	null
		,[CFA3rdInspectDate]		[date]			null
		,[CFA3rdInspectResult]		[varchar](30)	null
		,[Destination]				[varchar](50)	null
		,[PONO]						[varchar](30)	null
		,[OutstandingReason]		[nvarchar](max)	null
		,[ReasonRemark]				[nvarchar](max)	null
	)
	INSERT INTO @tmp
	select *
	from 
	(
		select
		[Country] = t.CountryID
		,[KPIGroup] = t.KPICode
		,[FactoryID] = t.FactoryID
		,[SPNO] = t.OrderID
		,[Style] = t.StyleID
		,[Seq] = t.Seq
		,[Brand] = t.BrandID
		,[BuyerDelivery] = t.BuyerDelivery
		,[FactoryKPI] = isnull(CONVERT(date, t.FtyKPI),'')
		,[Extension] = CONVERT(date,DATEADD(day, isnull(t.OTDExtension,0), t.BuyerDelivery))
		,[DeliveryByShipmode] = t.ShipmodeID
		,[OrderQty] = t.OrderQty
		,[OnTimeQty] = CASE WHEN t.OnsiteSample = 1 THEN IIF(GetOnsiteSampleFail.isFail = 1 or sew.SewLastDate is null, 0, Cast(t.OrderQty as int))
						  WHEN t.GMTComplete = 'S' and isnull(tpd.maxPulloutDate, packPulloutDate.val) is null THEN Cast(0 as int) --[IST20190675] 若為短交且PullOutDate是空的,不算OnTime也不算Fail,直接給0
						  WHEN isnull(t.isDevSample,0) = 1 then iif(pd2.isFail = 1 or isnull(pd2.PulloutDate, packPulloutDate.val) is null, 0, Cast(t.OrderQty as int))
						  Else Cast(isnull(tpd.Qty, isnull(packOnTimeQty.val, 0)) as int)
						  End
		,[FailQty] = CASE WHEN t.OnsiteSample = 1 THEN IIF(GetOnsiteSampleFail.isFail = 1 or sew.SewLastDate is null, Cast(t.OrderQty as int), 0)
						 WHEN t.GMTComplete = 'S' and isnull(tpd.maxPulloutDate, packPulloutDate.val) is null THEN Cast(0 as int)
						 WHEN isnull(t.isDevSample,0) = 1 then iif(pd2.isFail = 1 or isnull(pd2.PulloutDate, packPulloutDate.val) is null, Cast(t.OrderQty as int), 0)
						 --當pullout與packing都沒又抓到fail qty時，就當作全部fail
						 WHEN tpd.FailQty is null and packFailQty.val is null and packOnTimeQty.val is null then Cast(t.OrderQty as int)
						 Else Cast(isnull(tpd.FailQty, isnull(packFailQty.val, 0)) as int)
						End
		,[ClogRec_OnTimeQty] =	Case When t.GMTComplete = 'S' and ctnr.CTNLastReceiveDate is null then Cast(0 as int)
								When GetCTNFail.isFail = 0 Then Cast(t.OrderQty as int)
								Else Cast(0 as int)
								End

		,[ClogRec_FailQty] =	Case When t.GMTComplete = 'S' and ctnr.CTNLastReceiveDate is null then Cast(0 as int)
								When GetCTNFail.isFail = 1 Then Cast(t.OrderQty as int)
								Else Cast(0 as int)
								End
		,[PullOutDate] = isnull(iif(isnull(t.isDevSample,0) = 1, CONVERT(date, pd2.PulloutDate), CONVERT(date, tpd.maxPulloutDate)), packPulloutDate.val)
		,[Shipmode] = t.ShipmodeID
		,Pullouttimes = (select count(1) from (select distinct ID,OrderID,OrderShipmodeSeq from Pullout_Detail p2 where p2.OrderID = t.OrderID and p2.ShipQty > 0)x)
		,[GarmentComplete] = t.GMTComplete 
		,[ReasonID] = t.ReasonID
		,[OrderReason] =t.ReasonName   
		,[Handle] = dbo.getTPEPass1_ExtNo(t.MRHandle)
		,[SMR] = dbo.getTPEPass1_ExtNo(t.SMR)
		,[POHandle] = isnull(dbo.getTPEPass1_ExtNo(t.POHandle),'')
		,[POSMR] = isnull(dbo.getTPEPass1_ExtNo(t.POSMR),'')
		,[OrderType] = t.OrderTypeID
		,[DevSample] = iif(t.isDevSample = 1, 'Y', '')
		,[SewingQty] = sew.SewouptQty
		,[FOCQty] = getQtyBySeq.FOC
		,[LastSewingOutputDate] = convert(date,sew.SewLastDate)
		,[LastCartonReceivedDate] = ctnr.CTNLastReceiveDate
		,[IDDReason] = t.IDDReason
		,[PartialShipment] = iif(ps.ct>1,'Y','')
		,[Alias] = t.Alias
		,[CFAInspectionDate] = t.CFAFinalInspectDate
		,[CFAFinalInspectionResult] = t.CFAFinalInspectResult
		,[CFA3rdInspectDate] = t.CFA3rdInspectDate
		,[CFA3rdInspectResult] = t.CFA3rdInspectResult
		,[Destination] = t.Dest + '-' + t.Alias
		,[PONO] = t.CustPONo
		,[OutstandingReason]  = t.OutsdReason
		,[ReasonRemark] = t.ReasonRemark
		from @tmp_main t 
		left join @tmp_Pullout_Detail_pd tpd on t.OrderID = tpd.OrderID  and t.Seq = tpd.OrderShipmodeSeq
		left join @tmp_SewingOutput sew  on sew.OrderId = t.OrderID
		left join @tmp_getQtyBySeq getQtyBySeq  on getQtyBySeq.ID = t.OrderID and getQtyBySeq.Seq = t.Seq
		left join @tmp_ClogReceive ctnr  on ctnr.orderid = t.OrderID and ctnr.ordershipmodeseq = t.Seq
		outer apply(
			select ct=count(distinct seq)
			from Order_QtyShip oq
			where oq.id = t.OrderID
		)ps
		outer apply (
			Select top 1 iif(pd.PulloutDate > DATEADD(day, isnull(t.OTDExtension,0), t.BuyerDelivery), 1, 0) isFail, pd.PulloutDate
			From pullout p
			inner join Pullout_Detail pd with (nolock) on p.ID  = pd.id
			where pd.OrderID = t.OrderID
			and pd.OrderShipmodeSeq = t.Seq
			order by pd.PulloutDate ASC
		) pd2
		outer apply (
			select  [val] = sum(pld.shipqty)
			from PackingList pl with (nolock)
			inner join PackingList_Detail pld with (nolock) on pl.id = pld.id
			where   tpd.OrderID is null
					and pld.OrderID = t.OrderID
					and pld.OrderShipmodeSeq = t.Seq
					and pl.PulloutID is not null
					and pl.pulloutdate <= iif(t.ShipmodeID in ('E/C', 'E/P'), t.BuyerDelivery, DATEADD(day, isnull(t.OTDExtension,0), t.BuyerDelivery))
		) packOnTimeQty
		outer apply (
			select  [val] = sum(pld.shipqty)
			from PackingList pl with (nolock)
			inner join PackingList_Detail pld with (nolock) on pl.id = pld.id
			where   tpd.OrderID is null
					and pld.OrderID = t.OrderID
					and pld.OrderShipmodeSeq = t.Seq
					and pl.PulloutID is not null
					and pl.pulloutdate > iif(t.ShipmodeID in ('E/C', 'E/P'), t.BuyerDelivery, DATEADD(day, isnull(t.OTDExtension,0), t.BuyerDelivery))
		) packFailQty
		outer apply (
			select  [val] = min(pl.PulloutDate)
			from PackingList pl with (nolock)
			inner join PackingList_Detail pld with (nolock) on pl.id = pld.id
			where   tpd.OrderID is null
					and pld.OrderID = t.OrderID
					and pld.OrderShipmodeSeq = t.Seq
					and pl.PulloutID is not null
		) packPulloutDate
		outer apply (
			Select isFail = iif(sew.SewLastDate > t.BuyerDelivery, 1, 0)
		) GetOnsiteSampleFail
		outer apply(
			Select isFail = iif(isnull(ctnr.CTNLastReceiveDate,GetDate()) > t.BuyerDelivery, 1, 0)
		) as GetCTNFail
		where t.OrderQty > 0
		union all
		select
		[Country] = t.CountryID
		,[KPIGroup] = t.KPICode
		,[FactoryID] = t.FactoryID
		,[SPNO] = t.OrderID
		,[Style] = t.StyleID
		,[Seq] = t.Seq
		,[Brand] = t.BrandID
		,[BuyerDelivery] = t.BuyerDelivery
		,[FactoryKPI] = isnull(CONVERT(date, t.FtyKPI),'')
		,[Extension] = CONVERT(date,DATEADD(day, isnull(t.OTDExtension,0), t.BuyerDelivery))
		,[DeliveryByShipmode] = t.ShipmodeID
		,[OrderQty] = 0
		,[OnTimeQty] = 0
		,[FailQty] =Cast(isnull(t.OrderQty - (CONVERT(int,tpd.Qty) + CONVERT(int,tpd.FailQty)),0) as int)
		,[ClogRec_OnTimeQty] = 0
		,[ClogRec_FailQty] = 0
		,[PullOutDate] = null
		,[Shipmode] = t.ShipmodeID
		,Pullouttimes = 0
		,[GarmentComplete] = t.GMTComplete 
		,[ReasonID] = t.ReasonID
		,[OrderReason] =t.ReasonName   
		,[Handle] = dbo.getTPEPass1_ExtNo(t.MRHandle)
		,[SMR] = dbo.getTPEPass1_ExtNo(t.SMR)
		,[POHandle] = isnull(dbo.getTPEPass1_ExtNo(t.POHandle),'')
		,[POSMR] = isnull(dbo.getTPEPass1_ExtNo(t.POSMR),'')
		,[OrderType] = t.OrderTypeID
		,[DevSample] = iif(t.isDevSample = 1, 'Y', '')
		,[SewingQty] = sew.SewouptQty
		,[FOCQty] = getQtyBySeq.FOC
		,[LastSewingOutputDate] = convert(date,sew.SewLastDate)
		,[LastCartonReceivedDate] = ctnr.CTNLastReceiveDate
		,[IDDReason] = t.IDDReason
		,[PartialShipment] = iif(ps.ct>1,'Y','')
		,[Alias] = t.Alias
		,[CFAInspectionDate] = t.CFAFinalInspectDate
		,[CFAFinalInspectionResult] = t.CFAFinalInspectResult
		,[CFA3rdInspectDate] = t.CFA3rdInspectDate
		,[CFA3rdInspectResult] = t.CFA3rdInspectResult
		,[Destination] = t.Dest + '-' + t.Alias
		,[PONO] = t.CustPONo
		,[OutstandingReason]  = t.OutsdReason
		,[ReasonRemark] = t.ReasonRemark
		from @tmp_main t
		outer apply (
			select isPartial = iif (count(distinct oqs.Seq) > 1 ,'Y','')
			from Order_QtyShip oqs
			where oqs.Id = t.OrderID
		) getPartial
		left join @tmp_Pullout_Detail_pd tpd on t.OrderID = tpd.OrderID  and t.Seq = tpd.OrderShipmodeSeq
		left join @tmp_SewingOutput sew on sew.OrderId = t.OrderID
		left join @tmp_getQtyBySeq getQtyBySeq on getQtyBySeq.ID = t.OrderID and getQtyBySeq.Seq = t.Seq
		left join @tmp_ClogReceive ctnr on ctnr.orderid = t.OrderID and ctnr.ordershipmodeseq = t.Seq
		outer apply(
			select ct=count(distinct seq)
			from Order_QtyShip oq
			where oq.id = t.OrderID
		)ps
		where t.GMTComplete != 'S' 
		and t.OnsiteSample = 0 
		and  Cast(isnull(t.OrderQty - (CONVERT(int,tpd.Qty) + CONVERT(int,tpd.FailQty)),0) as int) > 0
		and isnull(t.isDevSample,0) = 0 
	)tb


	declare @tmp_A TABLE
	(
		[Country]					[varchar](30)	null
		,[KPIGroup]					[varchar](30)	null
		,[FactoryID]				[varchar](30)	null
		,[SPNO]						[varchar](30)	null
		,[Style]					[varchar](30)	null
		,[Seq]						[varchar](30)	null
		,[Brand]					[varchar](30)	null
		,[BuyerDelivery]			[date]			null
		,[FactoryKPI]				[date]			null
		,[Extension]				[date]			null
		,[DeliveryByShipmode]		[varchar](30)	null
		,[OrderQty]					[int]			null
		,[OnTimeQty]				[int]			null
		,[FailQty]					[int]			null
		,[ClogRec_OnTimeQty]		[int]			null
		,[ClogRec_FailQty]			[int]			null
		,[PullOutDate]				[date]			null
		,[Shipmode]					[varchar](30)	null
		,[Pullouttimes]				[int]			null
		,[GarmentComplete]			[varchar](30)	null
		,[ReasonID]					[varchar](30)	null
		,[OrderReason]				[nvarchar](500)	null
		,[Handle]					[varchar](60)	null
		,[SMR]						[varchar](50)	null
		,[POHandle]					[varchar](50)	null
		,[POSMR]					[varchar](50)	null
		,[OrderType]				[varchar](50)	null
		,[DevSample]				[varchar](30)	null
		,[SewingQty]				[int]			null
		,[FOCQty]					[int]			null
		,[LastSewingOutputDate]		[date]			null
		,[LastCartonReceivedDate]	[dateTime]		null
		,[IDDReason]				[varchar](20)	null
		,[PartialShipment]			[varchar](5)	null
		,[Alias]					[varchar](20)	null
		,[CFAInspectionDate]		[date]			null
		,[CFAFinalInspectionResult]	[varchar](50)	null
		,[CFA3rdInspectDate]		[date]			null
		,[CFA3rdInspectResult]		[varchar](30)	null
		,[Destination]				[varchar](50)	null
		,[PONO]						[varchar](30)	null
		,[OutstandingReason]		[nvarchar](max)	null
		,[ReasonRemark]				[nvarchar](max)	null
	)
	INSERT INTO @tmp_A
	SELECT
	[Country]
	,[KPIGroup]
	,[FactoryID]
	,[SPNO]
	,[Style]
	,[Seq]
	,[Brand]
	,[BuyerDelivery]
	,[FactoryKPI]
	,[Extension]
	,[DeliveryByShipmode]
	,[OrderQty]
	,[OnTimeQty]
	,[FailQty]
	,[ClogRec_OnTimeQty]
	,[ClogRec_FailQty]
	,[PullOutDate]
	,[Shipmode]
	,[Pullouttimes]
	,[GarmentComplete]
	,[ReasonID]
	,[OrderReason]  
	,[Handle]
	,[SMR]
	,[POHandle]
	,[POSMR]
	,[OrderType]
	,[DevSample]
	,[SewingQty]
	,[FOCQty]
	,[LastSewingOutputDate]
	,[LastCartonReceivedDate]
	,[IDDReason]
	,[PartialShipment]
	,[Alias]
	,[CFAInspectionDate]
	,[CFAFinalInspectionResult]
	,[CFA3rdInspectDate]
	,[CFA3rdInspectResult]
	,[Destination]
	,[PONO]
	,[OutstandingReason]
	,[ReasonRemark]
	FROM @tmp t
	INNER JOIN Factory f ON t.KPIGroup=f.id

	select * from @tmp_A t ORDER BY  t.SPNO, t.seq, t.KPIGroup
end
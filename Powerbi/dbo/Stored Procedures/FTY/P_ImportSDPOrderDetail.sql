-- =============================================
-- Create date: 2020/03/20
-- Description:	Data Query Logic by PMS.Planning R17, Import Data to P_SDPOrderDetail
-- =============================================
CREATE PROCEDURE [dbo].[P_ImportSDPOrderDetail]	
	@BuyerDelivery_s Date,
	@BuyerDelivery_e Date  = NULL
AS
BEGIN

	IF @BuyerDelivery_e IS NULL
	BEGIN
		SET @BuyerDelivery_e = '2200/01/01'
	END

	----準備基礎資料
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
								WHEN 'S' THEN 'S' ELSE '' END
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
		,o.OutstandingReason
		,o.OutstandingRemark
	into #tmp_main
	FROM Production.dbo.Orders o WITH (NOLOCK)
	LEFT JOIN Production.dbo.OrderType ot on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID and o.BrandID = ot.BrandID
	LEFT JOIN Production.dbo.Factory f ON o.FACTORYID = f.ID --AND  o.FactoryID = f.KPICode
	LEFT JOIN Production.dbo.Country c ON F.COUNTRYID = c.ID 
	inner JOIN Production.dbo.Order_QtyShip Order_QS on Order_QS.id = o.id
	LEFT JOIN Production.dbo.PO ON o.POID = PO.ID
	LEFT JOIN Production.dbo.Reason r on r.id = Order_QS.ReasonID and r.ReasonTypeID = 'Order_BuyerDelivery'          
	LEFT JOIN Production.dbo.Reason rs on rs.id = Order_QS.ReasonID and rs.ReasonTypeID = 'Order_BuyerDelivery_sample'
	Left join Production.dbo.Reason rd on rd.id = o.OutstandingReason and rd.ReasonTypeID = 'Delivery_OutStand'
	LEFT JOIN Production.dbo.Brand b on o.BrandID = b.ID
	where o.Junk = 0  
	and (isnull(ot.IsGMTMaster,0) = 0 or o.OrderTypeID = '') 
	and o.LocalOrder <> 1
	and o.IsForecast <> 1
	AND ((o.Category = 'B' AND f.Type = 'B') OR (o.Category = 'S' AND f.Type = 'S')) 
	AND Order_QS.BuyerDelivery >= @BuyerDelivery_s 
	AND Order_QS.BuyerDelivery <= @BuyerDelivery_e
		--AND f.KPICode = 'MAI' 
 

 

	----準備Sewing資料
	select SewouptQty=sum(x.QaQty),SewLastDate=max(SewLastDate), OrderID
	into #tmp_SewingOutput
	from(
		Select OrderID, Article, SizeCode, 
			Min(QaQty) as QaQty,
			Min(SewLastDate) as SewLastDate
		From (
			Select ComboType, t.OrderID, Article, SizeCode, QaQty = Sum(SewingOutput_Detail_Detail.QaQty), Max(OutputDate) as SewLastDate
			From Production.dbo.SewingOutput_Detail_Detail
			inner join (select distinct OrderID from #tmp_main) t on SewingOutput_Detail_Detail.OrderID= t.OrderID
			inner join Production.dbo.SewingOutput on SewingOutput_Detail_Detail.ID = SewingOutput.ID 
			Group by ComboType, t.OrderID, Article, SizeCode
		) as sdd
		Group by OrderID, Article, SizeCode
	)x
	group by OrderID


 
		----準備 Pullout 資料
	select DISTINCT PulloutDate ,op.OrderID,op.OrderShipmodeSeq
	into #tmp_Pullout_Detail
	from Production.dbo.Pullout_Detail op 
	inner join #tmp_main t on  op.OrderID = t.OrderID and op.OrderShipmodeSeq = t.Seq 


	select max(p.PulloutDate)PulloutDate ,pd.OrderID,pd.OrderShipmodeSeq
	into #tmp_Pullout_Detail_p
	from Production.dbo.Pullout_Detail pd with (nolock)
	inner join #tmp_main t on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
	INNER JOIN Production.dbo.Pullout p ON p.Id=pd.id AND p.PulloutDate=pd.PulloutDate 
	inner join Production.dbo.Pullout_Detail_Detail pdd with (nolock) on pd.Ukey = pdd.Pullout_DetailUKey
	where pd.OrderID = t.OrderID and pd.OrderShipmodeSeq =  t.Seq 
	group by pd.OrderID,pd.OrderShipmodeSeq


	Select 
		Qty = Sum(rA.Qty),
		FailQty = Sum(rB.Qty),
		pd.OrderID,
		pd.OrderShipmodeSeq
	into #tmp_Pullout_Detail_pd
	From Production.dbo.Pullout_Detail pd  with (nolock)
	inner join #tmp_main t on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
	inner join Production.dbo.Pullout_Detail_Detail pdd with (nolock) on pd.Ukey = pdd.Pullout_DetailUKey
	Outer apply (Select Qty = IIF(pd.pulloutdate <= iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI)), pdd.shipqty, 0)) rA --On Time
	Outer apply (Select Qty = IIF(pd.pulloutdate >  iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI)), pdd.shipqty, 0)) rB --Fail
	group by pd.OrderID, pd.OrderShipmodeSeq


	----準備 Clog 倉入庫資訊
	select 
			t.OrderID
		,t.Seq
		,ReceiveDate=max(cr.AddDate)
	into #tmpReceiveDate1
	from #tmp_main t
	inner join Production.dbo.PackingList_Detail pd on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
	inner join Production.dbo.ClogReceive cr on (cr.PackingListID = pd.ID and cr.OrderID = pd.OrderID and cr.CTNStartNo = pd.CTNStartNo and pd.SCICtnNo <>'')
	where pd.OrderID = t.OrderID
	and pd.OrderShipmodeSeq = t.Seq
	and not exists(select 1 
				from Production.dbo.PackingList_Detail pdCheck
				where pdCheck.OrderID =t.OrderID
						and pdCheck.OrderShipmodeSeq = t.Seq
						and CTNQty > 0
						and pdCheck.ReceiveDate is null
			)
	group by t.OrderID,t.Seq

	select
			t.OrderID
		,t.Seq
		,ReceiveDate=max(cr.AddDate)
	into #tmpReceiveDate2
	from #tmp_main t
	inner join Production.dbo.PackingList_Detail pd on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq
	inner join Production.dbo.ClogReceive cr on cr.SCICtnNo = pd.SCICtnNo and pd.SCICtnNo <>''
	where pd.OrderID = t.OrderID
	and pd.OrderShipmodeSeq = t.Seq
	and not exists(select 1 
				from Production.dbo.PackingList_Detail pdCheck
				where pdCheck.OrderID =t.OrderID
						and pdCheck.OrderShipmodeSeq = t.Seq
						and CTNQty > 0
						and pdCheck.ReceiveDate is null
			)
	group by t.OrderID,t.Seq


	select  t.OrderID,t.Seq,ReceiveDate=max(t.ReceiveDate)
	into #maxReceiveDate
	from(
		select *from #tmpReceiveDate1
		union all
		select *from #tmpReceiveDate2
	)t
	group by t.OrderID,t.Seq


	SELECT  
		[orderid]= t.OrderID
		,[ordershipmodeseq]= t.Seq
		,[CTNLastReceiveDate]= CAST(r.ReceiveDate as DATETIME)-- format(r.ReceiveDate, 'yyyy/MM/dd HH:mm:ss')
	into #tmp_ClogReceive
	from #tmp_main t
	left join #maxReceiveDate r on t.OrderID = r.orderid and t.Seq = r.Seq


	Select  oqsD.ID,oqsD.Seq
			,sum (case when Production.dbo.GetPoPriceByArticleSize(oqsd.id,oqsD.Article,oqsD.SizeCode) > 0 then oqsD.Qty else 0 end) as FOB
			,sum (case when Production.dbo.GetPoPriceByArticleSize(oqsd.id,oqsD.Article,oqsD.SizeCode) = 0 then oqsD.Qty else 0 end) as FOC
	into #getQtyBySeq
	From Production.dbo.Order_QtyShip_Detail oqsD
	where exists(select 1 from #tmp_main where OrderID = oqsD.ID and  Seq =  oqsD.Seq)
	group by oqsD.ID,oqsD.Seq

	SELECT  * 
	INTO #tmp FROM 
	( 
		SELECT
				 Country = t.CountryID
				,KPIGroup=t.KPICode
				,Factory=t.FactoryID
				,SPNO=t.OrderID 
				,t.StyleID
				,t.seq
				,Brand=t.BrandID
				,BuyerDelivery = convert(varchar(10),t.BuyerDelivery,111)--G
				,FactoryKPI= convert(varchar(10),t.FtyKPI,111)
				,Extension = iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI))
				,DeliveryByShipmode = t.ShipmodeID
				,t.OrderQty 
				,OnTimeQty = CASE WHEN t.OnsiteSample = 1 THEN IIF(GetOnsiteSampleFail.isFail = 1 or sew.SewLastDate is null, 0, Cast(t.OrderQty as int))
									WHEN t.GMTComplete = 'S' and p.PulloutDate is null THEN Cast(0 as int) --[IST20190675] 若為短交且PullOutDate是空的,不算OnTime也不算Fail,直接給0
									Else iif(isnull(t.isDevSample,0) = 1, iif(pd2.isFail = 1 or pd2.PulloutDate is null, 0, Cast(t.OrderQty as int)), Cast(isnull(pd.Qty,0) as int)) 
								End
				,FailQty =  CASE WHEN t.OnsiteSample = 1 THEN IIF(GetOnsiteSampleFail.isFail = 1 or sew.SewLastDate is null, Cast(t.OrderQty as int), 0)
									WHEN t.GMTComplete = 'S' and p.PulloutDate is null THEN Cast(0 as int)
									Else iif(isnull(t.isDevSample,0) = 1, iif(pd2.isFail = 1 or pd2.PulloutDate is null, Cast(t.OrderQty as int), 0), Cast(isnull(pd.FailQty,t.OrderQty) as int)) 
						End
				,PullOutDate = iif(isnull(t.isDevSample,0) = 1, CONVERT(char(10), pd2.PulloutDate, 20), CONVERT(char(10), p.PulloutDate, 111))
				,Shipmode = t.ShipmodeID
				,P = (select count(1)from(select distinct ID,OrderID,OrderShipmodeSeq from Production.dbo.Pullout_Detail p2 where p2.OrderID = t.OrderID and p2.ShipQty > 0)x )  --未出貨,出貨次數=0 --不論這OrderID的OrderShipmodeSeq有沒有被撈出來, 都要計算
				,GarmentComplete=t.GMTComplete 
				,[ReasonID] = ISNULL(t.ReasonID ,'')
				,[OrderReason] = ISNULL(t.ReasonName ,'')
				,LastSewingOutputDate = convert(varchar(10),sew.SewLastDate,111) 
				,LastCartonReceivedDate=ctnr.CTNLastReceiveDate  
				,Handle = Production.dbo.getTPEPass1_ExtNo(t.MRHandle)
				,SMR = Production.dbo.getTPEPass1_ExtNo(t.SMR)
				,POHandle = ISNULL(Production.dbo.getTPEPass1_ExtNo(t.POHandle),'')
				,POSMR = ISNULL(Production.dbo.getTPEPass1_ExtNo(t.POSMR),'')
				,OrderType = t.OrderTypeID
				,DevSample = iif(t.isDevSample = 1, 'Y', '')
				,SewingQty=ISNULL(sew.SewouptQty,0)
				, FOCQty = getQtyBySeq.FOC
				,PartialShipment=iif(ps.ct>1,'Y','')
				,[OutstandingReason]=ISNULL(t.OutstandingReason,'')
				-----以下為PMS程式有，但沒有要寫入的
				,t.Alias 				
				,[OutstandingRemark]=ISNULL(t.OutstandingRemark,'')
				,[OSTClogCarton]=ISNULL(OSTClogCarton.Val,0)
				,t.ReasonRemark
		from #tmp_main t
		--出貨次數--
		left join #tmp_Pullout_Detail op on op.OrderID = t.OrderID and op.OrderShipmodeSeq = t.Seq 
		------------
		-----isDevSample=0-----
		left join  #tmp_Pullout_Detail_pd pd on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq  
		left join  #tmp_Pullout_Detail_p p on p.OrderID = t.OrderID and p.OrderShipmodeSeq = t.Seq 
		---------End-------
		-------sew
		left join #tmp_SewingOutput sew on sew.OrderId = t.OrderID
		----[CTNLastReceiveDate]
		left join #tmp_ClogReceive ctnr on ctnr.orderid = t.OrderID and ctnr.ordershipmodeseq = t.Seq
		left join #getQtyBySeq getQtyBySeq on getQtyBySeq.ID = t.OrderID and getQtyBySeq.Seq = t.Seq
		outer apply(
			select ct=count(distinct seq)
			from Production.dbo.Order_QtyShip oq
			where oq.id = t.OrderID
		)ps
		-----------isDevSample=1-----------
		outer apply (
			Select top 1 iif(pd.PulloutDate > iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI)), 1, 0) isFail, pd.PulloutDate
			From Production.dbo.pullout p
			inner join Production.dbo.Pullout_Detail pd with (nolock) on p.ID  = pd.id
			where pd.OrderID = t.OrderID
			and pd.OrderShipmodeSeq = t.Seq
			order by pd.PulloutDate ASC
		) pd2
		----------------End----------------
		-----------OnsiteSample=1-----------
		outer apply (
			Select isFail = iif(sew.SewLastDate > t.FtyKPI, 1, 0)
		) GetOnsiteSampleFail
		----------------End----------------
		OUTER APPLY(
			SELECT [Val]=SUM(IIF( pd.CFAReceiveDate IS NULL AND pd.ReceiveDate IS NULL,1,0))
			FROM Production.dbo.PackingList_Detail pd
			WHERE pd.OrderID= t.OrderID
			AND pd.OrderShipmodeSeq = t.Seq
		)OSTClogCarton
		where t.OrderQty > 0 
		-----End-------

		--部分未出貨Fail的自成一行,且ShipQty給0,避免在Excel整欄加總重覆計算
		UNION ALL ----------------------------------------------------
		SELECT 
			Country = t.CountryID
			,KPIGroup = t.KPICode
			,Factory = t.FactoryID
			,SPNO=t.OrderID  
			,t.StyleID
			,t.Seq 
			,Brand=t.BrandID  
			, BuyerDelivery = convert(varchar(10),t.BuyerDelivery,111)
			, FactoryKPI = convert(varchar(10),t.FtyKPI,111)
			, Extension = iif(t.ShipmodeID in ('A/C', 'A/P', 'E/C', 'E/P'), t.FtyKPI, DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI))
			, DeliveryByShipmode = t.ShipmodeID
			, OrderQty = 0
			, OnTimeQty =  0
			, FailQty = Cast(isnull(t.OrderQty - (pd.Qty + pd.FailQty),0) as int) --未出貨Qty
			, pullOutDate = null
			, Shipmode = t.ShipModeID
			,P =0 --未出貨,出貨次數=0
			,GarmentComplete=t.GMTComplete 
			,[ReasonID] = ISNULL(t.ReasonID ,'')
			,[OrderReason] = ISNULL(t.ReasonName ,'')
			, LastSewingOutputDate = convert(varchar(10),sew.SewLastDate,111)
			,LastCartonReceivedDate=ctnr.CTNLastReceiveDate
			,Handle = Production.dbo.getTPEPass1_ExtNo(t.MRHandle)
			,SMR = Production.dbo.getTPEPass1_ExtNo(t.SMR)
			,POHandle = ISNULL(Production.dbo.getTPEPass1_ExtNo(t.POHandle),'')
			,POSMR = ISNULL( Production.dbo.getTPEPass1_ExtNo(t.POSMR),'')
			,OrderType=t.OrderTypeID  
			,DevSample = iif(t.isDevSample = 1, 'Y', '')
			,SewingQty=ISNULL(sew.SewouptQty,0)
			, FOCQty = getQtyBySeq.FOC
			,PartialShipment=iif(ps.ct>1,'Y','')
			,[OutstandingReason]=ISNULL(t.OutstandingReason,'')
			-----以下為PMS程式有，但沒有要寫入的
			,t.Alias
			,[OutstandingRemark]=ISNULL(t.OutstandingRemark,'')
			,[OSTClogCarton]=ISNULL(OSTClogCarton.Val,0)
			,t.ReasonRemark
		From #tmp_main t 
		--是否有分批出貨
		outer apply (
			select isPartial = iif (count(distinct oqs.Seq) > 1 ,'Y','')
			from Production.dbo.Order_QtyShip oqs
			where oqs.Id = t.OrderID
		) getPartial

		-----isDevSample=0-----
		left join  #tmp_Pullout_Detail_pd pd on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq 
		left join  #tmp_Pullout_Detail_p p on p.OrderID = t.OrderID and p.OrderShipmodeSeq = t.Seq 
		---------End------- 
		-------sew
		left join #tmp_SewingOutput sew on sew.OrderId = t.OrderID
		----[CTNLastReceiveDate]
		left join #tmp_ClogReceive ctnr on ctnr.orderid = t.OrderID and ctnr.ordershipmodeseq = t.Seq
		left join #getQtyBySeq getQtyBySeq on getQtyBySeq.ID = t.OrderID and getQtyBySeq.Seq = t.Seq
		outer apply(
			select ct=count(distinct seq)
			from Production.dbo.Order_QtyShip oq
			where oq.id = t.OrderID
		)ps
		OUTER APPLY(
			SELECT [Val]=SUM(IIF( pd.CFAReceiveDate IS NULL AND pd.ReceiveDate IS NULL,1,0))
			FROM Production.dbo.PackingList_Detail pd
			WHERE pd.OrderID= t.OrderID
			AND pd.OrderShipmodeSeq = t.Seq
		)OSTClogCarton
		-------End-------
		where t.GMTComplete != 'S' 
		and t.OnsiteSample = 0 -- 2020/03/27 [IST20200536]ISP20200567 onSiteSample = 0 才需要看這邊規則是否Fail
		and t.OrderQty - (pd.Qty + pd.FailQty) > 0
		and isnull(t.isDevSample,0) = 0 --isDevSample = 0 才需要看這邊的規則是否Fail
	)a

	SELECT DISTINCT t.*
	INTO #tmp_SDPOrderDetail
	FROM #tmp t
	INNER JOIN Production.dbo.Factory f ON t.KPIGroup=f.id
	ORDER BY  t.SPNO, t.seq, t.KPIGroup


	drop table #tmp_main,#tmp_Pullout_Detail_p,#tmp_Pullout_Detail_pd,#tmp_Pullout_Detail
	,#tmp_SewingOutput
	,#tmp_ClogReceive
	,#tmp
	,#getQtyBySeq
	,#maxReceiveDate
	,#tmpReceiveDate1
	,#tmpReceiveDate2

	-----開始Merge  1. 有PullOut Date
	MERGE INTO P_SDPOrderDetail t
	USING (SELECT * FROM #tmp_SDPOrderDetail WHERE PullOutDate IS NOT NULL)s 
	ON t.Factory=s.Factory  AND t.SPNO=s.SPNO AND t.Seq = s.Seq  AND t.PullOutDate IS NOT NULL
	WHEN MATCHED THEN   
		UPDATE SET 
				t.Country = s.Country
			  , t.KPIGroup = s.KPIGroup
			  , t.Factory = s.Factory
			  , t.SPNO = s.SPNO
			  , t.StyleID = s.StyleID
			  , t.Seq = s.Seq
			  , t.Brand = s.Brand
			  , t.BuyerDelivery = s.BuyerDelivery
			  , t.FactoryKPI = s.FactoryKPI
			  , t.Extension = s.Extension
			  , t.DeliveryByShipMode = s.DeliveryByShipMode
			  , t.OrderQty = s.OrderQty
			  , t.OnTimeQty = s.OnTimeQty
			  , t.FailQty = s.FailQty
			  , t.PullOutDate = s.PullOutDate
			  , t.ShipMode = s.ShipMode
			  , t.P = s.P
			  , t.GarmentComplete = s.GarmentComplete
			  , t.ReasonID = s.ReasonID
			  , t.OrderReason = s.OrderReason
			  , t.LastSewingOutputDate = s.LastSewingOutputDate
			  , t.LastCartonReceivedDate = s.LastCartonReceivedDate
			  , t.Handle = s.Handle
			  , t.SMR = s.SMR
			  , t.POHandle = s.POHandle
			  , t.POSMR = s.POSMR
			  , t.OrderType = s.OrderType
			  , t.DevSample = s.DevSample
			  , t.SewingQty = s.SewingQty
			  , t.FOCQty = s.FOCQty
			  , t.PartialShipment = s.PartialShipment
			  , t.OutstandingReason = s.OutstandingReason
			  , t.OutstandingRemark = s.OutstandingRemark
			  , t.OSTClogCarton = s.OSTClogCarton
			  , t.Alias = s.Alias
			  , t.ReasonRemark = s.ReasonRemark
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (	Country				,KPIGroup				,Factory				,SPNO				,StyleID			,Seq			,Brand  ,BuyerDelivery      ,FactoryKPI      ,Extension
				  ,DeliveryByShipMode   ,OrderQty				,OnTimeQty				,FailQty			,PullOutDate		,ShipMode		,P      ,GarmentComplete	,ReasonID
				  ,OrderReason			,LastSewingOutputDate	,LastCartonReceivedDate	,Handle				,SMR				,POHandle		,POSMR  ,OrderType			,DevSample
				  ,SewingQty			,FOCQty					,PartialShipment		,OutstandingReason  ,OutstandingRemark  ,OSTClogCarton	,Alias  ,ReasonRemark
				)
		VALUES (	s.Country			,s.KPIGroup				,s.Factory					,s.SPNO					,s.StyleID				,s.Seq				,s.Brand  ,s.BuyerDelivery      ,s.FactoryKPI      ,s.Extension
				  ,s.DeliveryByShipMode	,s.OrderQty				,s.OnTimeQty				,s.FailQty				,s.PullOutDate			,s.ShipMode			,s.P      ,s.GarmentComplete	,s.ReasonID
				  ,s.OrderReason		,s.LastSewingOutputDate	,s.LastCartonReceivedDate	,s.Handle				,s.SMR					,s.POHandle			,s.POSMR  ,s.OrderType			,s.DevSample
				  ,s.SewingQty			,s.FOCQty				,s.PartialShipment			,s.OutstandingReason	,s.OutstandingRemark	,s.OSTClogCarton	,s.Alias  ,s.ReasonRemark
				)
	WHEN NOT MATCHED BY SOURCE AND t.BuyerDelivery >= @BuyerDelivery_s AND t.BuyerDelivery <= @BuyerDelivery_e  AND t.PullOutDate IS NOT NULL THEN 
		DELETE
	;
	
	-----開始Merge  2. 沒有PullOut Date	
	MERGE INTO P_SDPOrderDetail t
	USING (SELECT * FROM #tmp_SDPOrderDetail WHERE PullOutDate IS NULL)s 
	ON t.Factory=s.Factory  AND t.SPNO=s.SPNO AND t.Seq = s.Seq  AND t.PullOutDate IS NULL
	WHEN MATCHED THEN   
		UPDATE SET 
				t.Country = s.Country
			  , t.KPIGroup = s.KPIGroup
			  , t.Factory = s.Factory
			  , t.SPNO = s.SPNO
			  , t.StyleID = s.StyleID
			  , t.Seq = s.Seq
			  , t.Brand = s.Brand
			  , t.BuyerDelivery = s.BuyerDelivery
			  , t.FactoryKPI = s.FactoryKPI
			  , t.Extension = s.Extension
			  , t.DeliveryByShipMode = s.DeliveryByShipMode
			  , t.OrderQty = s.OrderQty
			  , t.OnTimeQty = s.OnTimeQty
			  , t.FailQty = s.FailQty
			  , t.PullOutDate = s.PullOutDate
			  , t.ShipMode = s.ShipMode
			  , t.P = s.P
			  , t.GarmentComplete = s.GarmentComplete
			  , t.ReasonID = s.ReasonID
			  , t.OrderReason = s.OrderReason
			  , t.LastSewingOutputDate = s.LastSewingOutputDate
			  , t.LastCartonReceivedDate = s.LastCartonReceivedDate
			  , t.Handle = s.Handle
			  , t.SMR = s.SMR
			  , t.POHandle = s.POHandle
			  , t.POSMR = s.POSMR
			  , t.OrderType = s.OrderType
			  , t.DevSample = s.DevSample
			  , t.SewingQty = s.SewingQty
			  , t.FOCQty = s.FOCQty
			  , t.PartialShipment = s.PartialShipment
			  , t.OutstandingReason = s.OutstandingReason
			  , t.OutstandingRemark = s.OutstandingRemark
			  , t.OSTClogCarton = s.OSTClogCarton
			  , t.Alias = s.Alias
			  , t.ReasonRemark = s.ReasonRemark
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (	Country				,KPIGroup				,Factory				,SPNO				,StyleID			,Seq			,Brand  ,BuyerDelivery      ,FactoryKPI      ,Extension
				  ,DeliveryByShipMode   ,OrderQty				,OnTimeQty				,FailQty			,PullOutDate		,ShipMode		,P      ,GarmentComplete	,ReasonID
				  ,OrderReason			,LastSewingOutputDate	,LastCartonReceivedDate	,Handle				,SMR				,POHandle		,POSMR  ,OrderType			,DevSample
				  ,SewingQty			,FOCQty					,PartialShipment		,OutstandingReason  ,OutstandingRemark  ,OSTClogCarton	,Alias  ,ReasonRemark
				)
		VALUES (	s.Country			,s.KPIGroup				,s.Factory					,s.SPNO					,s.StyleID				,s.Seq				,s.Brand  ,s.BuyerDelivery      ,s.FactoryKPI      ,s.Extension
				  ,s.DeliveryByShipMode	,s.OrderQty				,s.OnTimeQty				,s.FailQty				,s.PullOutDate			,s.ShipMode			,s.P      ,s.GarmentComplete	,s.ReasonID
				  ,s.OrderReason		,s.LastSewingOutputDate	,s.LastCartonReceivedDate	,s.Handle				,s.SMR					,s.POHandle			,s.POSMR  ,s.OrderType			,s.DevSample
				  ,s.SewingQty			,s.FOCQty				,s.PartialShipment			,s.OutstandingReason	,s.OutstandingRemark	,s.OSTClogCarton	,s.Alias  ,s.ReasonRemark
				)
	WHEN NOT MATCHED BY SOURCE AND t.BuyerDelivery >= @BuyerDelivery_s AND t.BuyerDelivery <= @BuyerDelivery_e  AND t.PullOutDate IS NULL THEN 
		DELETE
	;
	DROP TABLE #tmp_SDPOrderDetail
END


GO



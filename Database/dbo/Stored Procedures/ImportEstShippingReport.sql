CREATE PROCEDURE [dbo].[ImportEstShippingReport]
	@StartDate varchar(8),
	@EndDate varchar(8)
AS
BEGIN
 SET NOCOUNT ON;

declare @tmpFinal TABLE(
--create table #tmpFinal(
	[BuyerDelivery] [date] ,
	[Brand] [varchar](8) ,
	[SPNO] [varchar](13) ,
	[Category] [varchar](8) ,
	[Seq] [varchar](2) ,
	[PackingNo] [varchar](max) ,
	[PackingStatus] [varchar](max) ,
	[GBNo] [varchar](max) ,
	[PulloutDate] [varchar](max) ,
	[Order_TtlQty] [int] ,
	[SO_No] [varchar](16) ,
	[SO_CfmDate] [date] ,
	[CutOffDate] [datetime] ,
	[ShipPlanID] [varchar](13) ,
	[M] [varchar](8) ,
	[Factory] [varchar](8) ,
	[Destination] [varchar](30) ,
	[Price] [numeric](16, 3) ,
	[GW] [numeric](8, 3) ,
	[CBM] [numeric](13, 4) ,
	[ShipMode] [varchar](10) ,
	[Handle] [varchar](80) ,
	[LocalMR] [varchar](80) ,
	[SP_SEQ] [varchar](15) ,
	[If_Partial] [varchar](1) ,
	[CartonQtyAtCLog] [int] ,
	[SP_Prod_Output_Qty] [int] ,
	[OrderType] [varchar](20) ,
	[BuyBack] [varchar](1) ,
	[LoadingType] [varchar](max) ,
	[Est_PODD] [date] ,
	[Origin] [varchar](2) ,
	[Outstand] [nvarchar](max) ,
	[Outstanding_Reason2] [nvarchar](800) ,
	[Outstanding_Remark] [nvarchar](max) ,
	[ReturnedQty_bySeq] [int] ,
	[HC#] [varchar](max) ,
	[HCStatus] [varchar](max) ,
	[ShipQty_by_Seq] [int] ,
	[PackingQty_bySeq] [int] ,
	[CTNQty_bySeq] [int] ,
	[OrderQty_bySeq] [int] ,
	[FOC_Bal_Qty] [int] ,
	[Pullout_ID] [varchar](max) ,
	[IsGMTMaster] [bit] ,
	[PulloutComplete] [bit] 
)

	insert into @tmpFinal 
	select 
		 oq.BuyerDelivery
		,[Brand] = o.BrandID
		,[SPNo] = o.ID
		,[Category] = iif(o.Category = 'B','Bulk','Sample')
		,oq.Seq
		,PackingNo = pkid.pkid
		,PackingStatus = pkstatus.v
		,GBNo = pkINVNo
		,PulloutDate = pkPulloutDate.PulloutDate
		,Order_TtlQty = o.Qty
		,[SO_No] = gb2.SONo
		,[SO_CfmDate] = gb2.SOCFMDate
		,[CutOffDate] = gb2.CutOffDate
		,[ShipPlanID] = gb2.ShipPlanID
		,[M] = o.MDivisionID
		,[Factory] = o.FtyGroup
		,[Destination] = c.Alias
		,[Price] = o.PoPrice
		,plds.GW
		,[CBM] = cbm.CTNQty
		,[ShipMode] = oq.ShipmodeID
		,[Handle] = o.MRHandle+' - '+isnull((select Name + ' #' + ExtNo 
												   from TPEPass1 WITH (NOLOCK) 
												   where ID = o.MRHandle), '')  
		,[LocalMR] = o.LocalMR+' - '+isnull((select Name + ' #' + ExtNo 
												   from Pass1 WITH (NOLOCK) 
												   where ID = o.LocalMR), '')
		,[SP_SEQ] = oq.Id + oq.Seq
		,[If_Partial] = (select iif(count(1) > 1, 'Y', '') from Order_QtyShip with (nolock) where ID = o.ID)
		,[CartonQtyAtCLog] = isnull(o.ClogCTN, 0)
		,[SP_Prod_Output_Qty] = OutPutQty.MinSeqQty
		,[OrderType] = o.OrderTypeID
		,[BuyBack] = iif(exists(select 1 from Order_BuyBack with (nolock) where ID = o.ID), 'Y', '')
		,[LoadingType] = gbCYCFS.CYCFS
		,[Est_PODD] = o.EstPODD
		,[Origin] = m.CountryID
		,[Outstand] = isnull((select Name 
					from Reason WITH (NOLOCK) 
					where ReasonTypeID = 'Delivery_OutStand' and Id = o.OutstandingReason), '') 
		,[Outstanding_Reason2] = o.OutstandingReason + ' - ' + isnull((select Name 
																   from Reason WITH (NOLOCK) 
																   where ReasonTypeID = 'Delivery_OutStand' and Id = o.OutstandingReason), '') 
		,[Outstanding_Remark] = o.OutstandingRemark
		,[ReturnedQty_bySeq] = getInvAdjQty.value
		,[HC#] = pkExpressID.ExpressID
		,[HCStatus] = pkExpressStatus.ExpressStatus
		,[ShipQty_by_Seq] = (select isnull(sum(ShipQty), 0) 
							from Pullout_Detail WITH (NOLOCK) 
							where OrderID = o.ID and OrderShipmodeSeq = oq.Seq)
		,[PackingQty_bySeq] = plds.ShipQty
		,[CTNQty_bySeq] = plds.CTNQty
		,[OrderQty_bySeq] = oq.Qty
		,[FOC_Bal_Qty] = IIF(tmp1.Price = 0,o.FOCQty - isnull(tmp2.ShipQty,0) + isnull(tmp3.DiffQty,0),0)
		,[Pullout_ID] = pkPulloutID.PulloutID
		,ot.IsGMTMaster,o.PulloutComplete
	from Orders o WITH (NOLOCK) 
	inner join Factory f with (nolock) on o.FactoryID = f.ID and f.IsProduceFty=1
	inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
	left join OrderType ot WITH (NOLOCK) on ot.BrandID = o.BrandID and ot.id = o.OrderTypeID
	left join Country c WITH (NOLOCK) on o.Dest = c.ID
	left join MDivision m WITH (NOLOCK) on m.ID = o.MDivisionID
	left join
	(
		select distinct gb.FCRDate,pd.orderid, pd.OrderShipmodeSeq,p.ShipPlanID,gb.SONo,gb.SOCFMDate,gb.CutOffDate
		from packinglist_detail pd WITH (NOLOCK)
		inner join PackingList p WITH (NOLOCK) on p.id = pd.id 
		inner join GMTBooking gb WITH (NOLOCK)on gb.id = p.INVNo 
	)gb2 on  gb2.orderid = o.id and gb2.OrderShipmodeSeq = oq.seq
	outer apply(
		select pkid = stuff((
			select concat(',',a.id)
			from(
				select distinct pd.id
				from packinglist_detail pd WITH (NOLOCK)
				inner join packinglist p WITH (NOLOCK) on p.id = pd.id
				where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
			)a
			order by a.id
			for xml path('')
		),1,1,'')
	)pkid
	outer apply(
		select v = stuff((
			select concat(',',a.Status)
			from(
				select distinct pd.id,p.Status
				from packinglist_detail pd WITH (NOLOCK)
				inner join packinglist p WITH (NOLOCK) on p.id = pd.id
				where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
			)a
			order by a.id
			for xml path('')
		),1,1,'')
	)pkstatus
	outer apply(
		select pkINVNo = stuff((
			select concat(',',a.INVNo)
			from(
				select distinct p.INVNo,p.id
				from packinglist_detail pd WITH (NOLOCK)
				inner join PackingList p WITH (NOLOCK) on p.id = pd.id
				where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
			)a
			order by a.id
			for xml path('')
		),1,1,'')
	)pkINVNo
	outer apply(
		select PulloutDate = stuff((
			select concat(',',a.PulloutDate)
			from(
				select distinct p.PulloutDate,p.id
				from packinglist_detail pd WITH (NOLOCK)
				inner join PackingList p WITH (NOLOCK) on p.id = pd.id 
				where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
			)a
			order by a.id
			for xml path('')
		),1,1,'')
	)pkPulloutDate
	outer apply(
		select CTNQty=sum(pd.CTNQty),GW=sum(pd.GW),ShipQty=sum(pd.ShipQty)
		from packinglist_detail pd WITH (NOLOCK)
		inner join PackingList p WITH (NOLOCK) on p.id = pd.id
		where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
	)plds
	outer apply(
		select CTNQty=round(sum(l.CBM),4)
		from packinglist_detail pd WITH (NOLOCK)
		inner join PackingList p WITH (NOLOCK) on p.id = pd.id
		inner join LocalItem l WITH (NOLOCK) on l.refno = pd.refno
		where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
		and pd.CTNQty > 0
	)cbm
	outer apply(
		select CYCFS = stuff((
			select concat(',',a.CYCFS)
			from(
				select distinct gb.CYCFS,p.id
				from packinglist_detail pd WITH (NOLOCK)
				inner join PackingList p WITH (NOLOCK) on p.id = pd.id
				inner join GMTBooking gb WITH (NOLOCK) on gb.id = p.INVNo
				where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
            
			)a
			order by a.id
			for xml path('')
		),1,1,'')
	)gbCYCFS
	outer apply(
		select ExpressID = stuff((
			select concat(',',a.ExpressID)
			from(
				select distinct p.ExpressID,p.id
				from packinglist_detail pd WITH (NOLOCK)
				inner join PackingList p WITH (NOLOCK) on p.id = pd.id
				where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
			)a
			order by a.id
			for xml path('')
		),1,1,'')
	)pkExpressID
	outer apply(
		select ExpressStatus = stuff((
			select concat(',',a.Status)
			from(
				select distinct e.Status,p.ID
				from packinglist_detail pd WITH (NOLOCK)
				inner join PackingList p WITH (NOLOCK) on p.id = pd.id
				inner join Express e WITH (NOLOCK) on p.ExpressID = e.ID
				where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
			)a
			order by a.id
			for xml path('')
		),1,1,'')
	)pkExpressStatus
	outer apply(
		select PulloutID = stuff((
			select concat(',',PulloutID)
			from(
				select distinct p.PulloutID
				from packinglist_detail pd WITH (NOLOCK)
				inner join PackingList p WITH (NOLOCK) on p.id = pd.id
				where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
			)a
			order by a.PulloutID
			for xml path('')
		),1,1,'')
	)pkPulloutID
	outer apply(
		select MinSeqQty = MIN(a.QAQty)
			from (
				select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
				from Order_Location sl WITH (NOLOCK)
				left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = o.id and sdd.ComboType = sl.Location
				where sl.OrderID = o.id
				group by sl.Location
			) a
	)OutPutQty
	outer apply(
		select value = isnull(sum(iq.DiffQty),0)
		from InvAdjust i WITH (NOLOCK)
		, InvAdjust_Qty iq WITH (NOLOCK)
		where i.ID = iq.ID
		and i.OrderID = o.id
		and i.OrderShipmodeSeq = oq.Seq
	)getInvAdjQty
	outer apply(
		SELECT   [Article]=oq.Article
			,[SizeCode]=oq.SizeCode 
			,[Price]=ISNULL(ou1.POPrice, ISNULL(ou2.POPrice,-1))
		FROM Order_Qty oq WITH (NOLOCK)
		LEFT JOIN Order_UnitPrice ou1 WITH (NOLOCK) ON ou1.Id=oq.Id AND ou1.Article=oq.Article AND ou1.SizeCode=oq.SizeCode 
		LEFT JOIN Order_UnitPrice ou2 WITH (NOLOCK) ON ou2.Id=oq.Id AND ou2.Article='----' AND ou2.SizeCode='----' 
		WHERE oq.ID = o.ID
	)tmp1
	outer apply(
		SELECT pd.Article,pd.SizeCode,[ShipQty]=SUM(pd.ShipQty)
		FROM PackingList p WITH (NOLOCK)
		INNER JOIN PackingList_Detail pd WITH (NOLOCK) ON p.ID=pd.ID
		INNER JOIN Pullout pu WITH (NOLOCK) ON p.PulloutID=pu.ID
		WHERE pu.Status <> 'New' AND pd.OrderID = o.ID
		GROUP BY pd.Article,pd.SizeCode
	)tmp2
	outer apply(
		SELECt iq.Article,iq.SizeCode,[DiffQty]= SUM(iq.DiffQty)
		FROm InvAdjust i WITH (NOLOCK)
		INNER JOIN InvAdjust_Qty iq WITH (NOLOCK) ON i.ID = iq.ID
		WHERE i.OrderID = o.ID
		GROUP BY iq.Article,iq.SizeCode
	)tmp3
	where 1=1
	and oq.BuyerDelivery between @StartDate and @EndDate
	and o.Category in ('B','S','G')

	select distinct [BuyerDelivery], [Brand], [SPNo], [Category], [Seq], [PackingNo], [PackingStatus], [GBNo], [PulloutDate], [Order_TtlQty]
		,[SO_No], [SO_CfmDate], [CutOffDate], [ShipPlanID], [M], [Factory], [Destination], [Price], [GW], [CBM], [ShipMode], [Handle]												   
		,[LocalMR], [SP_SEQ], [If_Partial], [CartonQtyAtCLog], [SP_Prod_Output_Qty], [OrderType], [BuyBack], [LoadingType], [Est_PODD]
		,[Origin], [Outstanding_Reason2], [Outstanding_Remark], [ReturnedQty_bySeq], [HC#], [HCStatus], [ShipQty_by_Seq]
		,[PackingQty_bySeq], [CTNQty_bySeq], [OrderQty_bySeq], [FOC_Bal_Qty], [Pullout_ID]
	from(
		select * from @tmpFinal t
		where (
			isnull(t.IsGMTMaster,0) != 1
			and t.PulloutComplete = 0 and t.Order_TtlQty > 0
			and not exists (select 1 from Order_Finish Where ID = t.SPNo)
		)

		union all

		select * from @tmpFinal t
		where (
			exists (select 1 from Order_Finish Where ID = t.SPNo)
			and (select FOCQty from Order_Finish Where ID = t.SPNo) < FOC_Bal_Qty
		)
	) a 
END


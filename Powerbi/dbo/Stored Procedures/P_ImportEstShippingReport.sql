CREATE PROCEDURE [dbo].[P_ImportEstShippingReport]

	@StartDate Date,
	@EndDate Date,
	@LinkServerName varchar(50)
AS
BEGIN

declare @SqlCmd_Combin nvarchar(max) =''
declare @SqlCmd1 nvarchar(max) ='';
declare @SqlCmd2 nvarchar(max) ='';
declare @SqlCmd3 nvarchar(max) ='';
declare @SqlCmd4 nvarchar(max) ='';

declare @SDate varchar(20)  = cast(@StartDate as varchar)--CONVERT(date, DATEADD(year,-1, GETDATE())) 
declare @EDate varchar(20) = cast(@EndDate as varchar)--CONVERT(date, DATEADD(DAY,7, GETDATE()))

set @SqlCmd1 = '
select 
 oq.BuyerDelivery
,[Brand] = o.BrandID
,[SPNo] = o.ID
,[Category] = iif(o.Category = ''B'',''Bulk'',''Sample'')
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
,[Handle] = o.MRHandle+'' - ''+isnull((select Name + '' #'' + ExtNo 
										   from ['+@LinkServerName+'].Production.dbo.TPEPass1 WITH (NOLOCK) 
										   where ID = o.MRHandle), '''')  
,[LocalMR] = o.LocalMR+'' - ''+isnull((select Name + '' #'' + ExtNo 
										   from ['+@LinkServerName+'].Production.dbo.Pass1 WITH (NOLOCK) 
										   where ID = o.LocalMR), '''')
,[SP_SEQ] = oq.Id + oq.Seq
,[If_Partial] = (select iif(count(1) > 1, ''Y'', '''') from ['+@LinkServerName+'].Production.dbo.Order_QtyShip with (nolock) where ID = o.ID)
,[CartonQtyAtCLog] = isnull(o.ClogCTN, 0)
,[SP_Prod_Output_Qty] = OutPutQty.MinSeqQty
,[OrderType] = o.OrderTypeID
,[BuyBack] = iif(exists(select 1 from ['+@LinkServerName+'].Production.dbo.Order_BuyBack with (nolock) where ID = o.ID), ''Y'', '''')
,[LoadingType] = gbCYCFS.CYCFS
,[Est_PODD] = o.EstPODD
,[Origin] = m.CountryID
,[Outstand] = isnull((select Name 
			from ['+@LinkServerName+'].Production.dbo.Reason WITH (NOLOCK) 
			where ReasonTypeID = ''Delivery_OutStand'' and Id = o.OutstandingReason), '''') 
,[Outstanding_Reason2] = o.OutstandingReason + '' - '' + isnull((select Name 
														   from ['+@LinkServerName+'].Production.dbo.Reason WITH (NOLOCK) 
														   where ReasonTypeID = ''Delivery_OutStand'' and Id = o.OutstandingReason), '''') 
,[Outstanding_Remark] = o.OutstandingRemark
,[ReturnedQty_bySeq] = getInvAdjQty.value
,[HC#] = pkExpressID.ExpressID
,[HCStatus] = pkExpressStatus.ExpressStatus
,[ShipQty_by_Seq] = (select isnull(sum(ShipQty), 0) 
					from ['+@LinkServerName+'].Production.dbo.Pullout_Detail WITH (NOLOCK) 
					where OrderID = o.ID and OrderShipmodeSeq = oq.Seq)
,[PackingQty_bySeq] = plds.ShipQty
,[CTNQty_bySeq] = plds.CTNQty
,[OrderQty_bySeq] = oq.Qty
,[FOC_Bal_Qty] = IIF(tmp1.Price = 0,o.FOCQty - isnull(tmp2.ShipQty,0) + isnull(tmp3.DiffQty,0),0)
,[Pullout_ID] = pkPulloutID.PulloutID
,ot.IsGMTMaster,o.PulloutComplete
into #tmpFinal
from ['+@LinkServerName+'].Production.dbo.Orders o WITH (NOLOCK) 
inner join ['+@LinkServerName+'].Production.dbo.Factory f with (nolock) on o.FactoryID = f.ID and f.IsProduceFty=1
inner join ['+@LinkServerName+'].Production.dbo.Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
left join ['+@LinkServerName+'].Production.dbo.OrderType ot WITH (NOLOCK) on ot.BrandID = o.BrandID and ot.id = o.OrderTypeID
left join ['+@LinkServerName+'].Production.dbo.Country c WITH (NOLOCK) on o.Dest = c.ID
left join ['+@LinkServerName+'].Production.dbo.MDivision m on m.ID = o.MDivisionID
left join
(
	select distinct gb.FCRDate,pd.orderid, pd.OrderShipmodeSeq,p.ShipPlanID,gb.SONo,gb.SOCFMDate,gb.CutOffDate
	from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
	inner join ['+@LinkServerName+'].Production.dbo.PackingList p on p.id = pd.id 
	inner join ['+@LinkServerName+'].Production.dbo.GMTBooking gb on gb.id = p.INVNo 
)gb2 on  gb2.orderid = o.id and gb2.OrderShipmodeSeq = oq.seq
outer apply(
	select pkid = stuff((
		select concat('','',a.id)
		from(
			select distinct pd.id
			from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
            inner join ['+@LinkServerName+'].Production.dbo.packinglist p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
		)a
		order by a.id
		for xml path('''')
	),1,1,'''')
)pkid
'

set @SqlCmd2 = '
outer apply(
	select v = stuff((
		select concat('','',a.Status)
		from(
			select distinct pd.id,p.Status
			from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
            inner join ['+@LinkServerName+'].Production.dbo.packinglist p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
		)a
		order by a.id
		for xml path('''')
	),1,1,'''')
)pkstatus
outer apply(
	select pkINVNo = stuff((
		select concat('','',a.INVNo)
		from(
			select distinct p.INVNo,p.id
			from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
			inner join ['+@LinkServerName+'].Production.dbo.PackingList p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
		)a
		order by a.id
		for xml path('''')
	),1,1,'''')
)pkINVNo
outer apply(
	select PulloutDate = stuff((
		select concat('','',a.PulloutDate)
		from(
			select distinct p.PulloutDate,p.id
			from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
			inner join ['+@LinkServerName+'].Production.dbo.PackingList p on p.id = pd.id 
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
		)a
		order by a.id
		for xml path('''')
	),1,1,'''')
)pkPulloutDate
outer apply(
	select CTNQty=sum(pd.CTNQty),GW=sum(pd.GW),ShipQty=sum(pd.ShipQty)
	from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
	inner join ['+@LinkServerName+'].Production.dbo.PackingList p on p.id = pd.id
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
)plds
outer apply(
	select CTNQty=round(sum(l.CBM),4)
	from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
    inner join ['+@LinkServerName+'].Production.dbo.PackingList p on p.id = pd.id
	inner join ['+@LinkServerName+'].Production.dbo.LocalItem l on l.refno = pd.refno
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
    and pd.CTNQty > 0
)cbm
outer apply(
	select CYCFS = stuff((
		select concat('','',a.CYCFS)
		from(
			select distinct gb.CYCFS,p.id
			from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
			inner join ['+@LinkServerName+'].Production.dbo.PackingList p on p.id = pd.id
			inner join ['+@LinkServerName+'].Production.dbo.GMTBooking gb on gb.id = p.INVNo
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
            
		)a
		order by a.id
		for xml path('''')
	),1,1,'''')
)gbCYCFS
outer apply(
	select ExpressID = stuff((
		select concat('','',a.ExpressID)
		from(
			select distinct p.ExpressID,p.id
			from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
			inner join ['+@LinkServerName+'].Production.dbo.PackingList p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
		)a
		order by a.id
		for xml path('''')
	),1,1,'''')
)pkExpressID
outer apply(
	select ExpressStatus = stuff((
		select concat('','',a.Status)
		from(
			select distinct e.Status,p.ID
			from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
			inner join ['+@LinkServerName+'].Production.dbo.PackingList p on p.id = pd.id
			inner join ['+@LinkServerName+'].Production.dbo.Express e on p.ExpressID = e.ID
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
		)a
		order by a.id
		for xml path('''')
	),1,1,'''')
)pkExpressStatus
'

set @SqlCmd3 = '
outer apply(
	select PulloutID = stuff((
		select concat('','',PulloutID)
		from(
			select distinct p.PulloutID
			from ['+@LinkServerName+'].Production.dbo.packinglist_detail pd
			inner join ['+@LinkServerName+'].Production.dbo.PackingList p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq 
		)a
		order by a.PulloutID
		for xml path('''')
	),1,1,'''')
)pkPulloutID
outer apply(
	select MinSeqQty = MIN(a.QAQty)
		from (
			select sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
			from ['+@LinkServerName+'].Production.dbo.Order_Location sl WITH (NOLOCK)
			left join ['+@LinkServerName+'].Production.dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = o.id and sdd.ComboType = sl.Location
			where sl.OrderID = o.id
			group by sl.Location
		) a
)OutPutQty
outer apply(
	select value = isnull(sum(iq.DiffQty),0)
	from ['+@LinkServerName+'].Production.dbo.InvAdjust i WITH (NOLOCK)
	, ['+@LinkServerName+'].Production.dbo.InvAdjust_Qty iq WITH (NOLOCK)
	where i.ID = iq.ID
	and i.OrderID = o.id
	and i.OrderShipmodeSeq = oq.Seq
)getInvAdjQty
outer apply(
	SELECT   [Article]=oq.Article
		,[SizeCode]=oq.SizeCode 
		,[Price]=ISNULL(ou1.POPrice, ISNULL(ou2.POPrice,-1))
	FROM ['+@LinkServerName+'].Production.dbo.Order_Qty oq
	LEFT JOIN ['+@LinkServerName+'].Production.dbo.Order_UnitPrice ou1 ON ou1.Id=oq.Id AND ou1.Article=oq.Article AND ou1.SizeCode=oq.SizeCode 
	LEFT JOIN ['+@LinkServerName+'].Production.dbo.Order_UnitPrice ou2 ON ou2.Id=oq.Id AND ou2.Article=''----'' AND ou2.SizeCode=''----'' 
	WHERE oq.ID = o.ID
)tmp1
outer apply(
	SELECT pd.Article,pd.SizeCode,[ShipQty]=SUM(pd.ShipQty)
	FROM ['+@LinkServerName+'].Production.dbo.PackingList p 
	INNER JOIN ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd ON p.ID=pd.ID
	INNER JOIN ['+@LinkServerName+'].Production.dbo.Pullout pu ON p.PulloutID=pu.ID
	WHERE pu.Status <> ''New'' AND pd.OrderID = o.ID
	GROUP BY pd.Article,pd.SizeCode
)tmp2
outer apply(
	SELECt iq.Article,iq.SizeCode,[DiffQty]= SUM(iq.DiffQty)
	FROm ['+@LinkServerName+'].Production.dbo.InvAdjust i
	INNER JOIN ['+@LinkServerName+'].Production.dbo.InvAdjust_Qty iq ON i.ID = iq.ID
	WHERE i.OrderID = o.ID
	GROUP BY iq.Article,iq.SizeCode
)tmp3
where 1=1
and oq.BuyerDelivery between '''+@SDate+'''  and '''+@EDate+''' 
and o.Category in (''B'',''S'',''G'')


select distinct * 
into #tmpFinal2
from(
	select * from #tmpFinal t
	where (
		isnull(t.IsGMTMaster,0) != 1
		and t.PulloutComplete = 0 and t.Order_TtlQty > 0
		and not exists (select 1 from ['+@LinkServerName+'].Production.dbo.Order_Finish Where ID = t.SPNo)
	)

	union all

	select * from #tmpFinal t
	where (
		exists (select 1 from ['+@LinkServerName+'].Production.dbo.Order_Finish Where ID = t.SPNo)
		and (select FOCQty from ['+@LinkServerName+'].Production.dbo.Order_Finish Where ID = t.SPNo) < FOC_Bal_Qty
	)
) a

'

set @SqlCmd4 = '
BEGIN TRY
Begin tran

update t
set 
	t.BuyerDelivery =  s.BuyerDelivery,
	t.Brand =  s.Brand,
	t.SPNO =  s.SPNO,
	t.Category =  s.Category,
	t.Seq =  s.Seq,
	t.PackingNo =  s.PackingNo,
	t.PackingStatus =  s.PackingStatus,
	t.GBNo =  s.GBNo,
	t.PulloutDate =  s.PulloutDate,
	t.Order_TtlQty =  s.Order_TtlQty,
	t.SO_No =  s.SO_No,
	t.SO_CfmDate =  s.SO_CfmDate,
	t.CutOffDate =  s.CutOffDate,
	t.ShipPlanID =  s.ShipPlanID,
	t.M =  s.M,
	t.Factory =  s.Factory,
	t.Destination =  s.Destination,
	t.Price =  s.Price,
	t.GW =  s.GW,
	t.CBM =  s.CBM,
	t.ShipMode =  s.ShipMode,
	t.Handle =  s.Handle,
	t.LocalMR =  s.LocalMR,
	t.SP_SEQ =  s.SP_SEQ,
	t.If_Partial =  s.If_Partial,
	t.CartonQtyAtCLog =  s.CartonQtyAtCLog,
	t.SP_Prod_Output_Qty =  s.SP_Prod_Output_Qty,
	t.OrderType =  s.OrderType,
	t.BuyBack =  s.BuyBack,
	t.LoadingType =  s.LoadingType,
	t.Est_PODD =  s.Est_PODD,
	t.Origin =  s.Origin,
	t.Outstanding_Reason2 =  s.Outstanding_Reason2,
	t.Outstanding_Remark =  s.Outstanding_Remark,
	t.ReturnedQty_bySeq =  s.ReturnedQty_bySeq,
	t.HC# =  s.HC#,
	t.HCStatus =  s.HCStatus,
	t.ShipQty_by_Seq =  s.ShipQty_by_Seq,
	t.PackingQty_bySeq =  s.PackingQty_bySeq,
	t.CTNQty_bySeq =  s.CTNQty_bySeq,
	t.OrderQty_bySeq =  s.OrderQty_bySeq,
	t.FOC_Bal_Qty =  s.FOC_Bal_Qty,
	t.Pullout_ID =  s.Pullout_ID
from P_EstShippingReport t
inner join #tmpFinal2 s on t.SPNO = s.SPNO
and t.Seq = s.Seq and t.Factory = s.Factory

insert into P_EstShippingReport
select [BuyerDelivery]
      ,[Brand]
      ,[SPNO]
      ,[Category]
      ,[Seq]
      ,[PackingNo]
      ,[PackingStatus]
      ,[GBNo]
      ,[PulloutDate]
      ,[Order_TtlQty]
      ,[SO_No]
      ,[SO_CfmDate]
      ,[CutOffDate]
      ,[ShipPlanID]
      ,[M]
      ,[Factory]
      ,[Destination]
      ,[Price]
      ,[GW]
      ,[CBM]
      ,[ShipMode]
      ,[Handle]
      ,[LocalMR]
      ,[SP_SEQ]
      ,[If_Partial]
      ,[CartonQtyAtCLog]
      ,[SP_Prod_Output_Qty]
      ,[OrderType]
      ,[BuyBack]
      ,[LoadingType]
      ,[Est_PODD]
      ,[Origin]
      ,[Outstanding_Reason2]
      ,[Outstanding_Remark]
      ,[ReturnedQty_bySeq]
      ,[HC#]
      ,[HCStatus]
      ,[ShipQty_by_Seq]
      ,[PackingQty_bySeq]
      ,[CTNQty_bySeq]
      ,[OrderQty_bySeq]
      ,[FOC_Bal_Qty]
      ,[Pullout_ID]
from #tmpFinal2 s
where not exists(
	select 1 from P_EstShippingReport t
	where t.SPNO = s.SPNO
	and t.Seq = s.Seq
	and t.Factory = s.Factory
)

delete t
from P_EstShippingReport t
left join #tmpFinal2 s on t.SPNO = s.SPNO
	and t.Seq = s.Seq and t.Factory = s.Factory
where t.BuyerDelivery between '''+@SDate+'''  and '''+@EDate+''' 
and t.Factory in (select distinct Factory from #tmpFinal2)
and s.SPNO is null

drop table #tmpFinal,#tmpFinal2

Commit tran

END TRY
BEGIN CATCH
	RollBack Tran
	declare @ErrMsg varchar(1000) = ''Err# : '' + ltrim(str(ERROR_NUMBER())) + 
				CHAR(10)+''Error Severity:''+ltrim(str(ERROR_SEVERITY()  )) +
				CHAR(10)+''Error State:'' + ltrim(str(ERROR_STATE() ))  +
				CHAR(10)+''Error Proc:'' + isNull(ERROR_PROCEDURE(),'''')  +
				CHAR(10)+''Error Line:''+ltrim(str(ERROR_LINE()  )) +
				CHAR(10)+''Error Msg:''+ ERROR_MESSAGE() ;
    
    RaisError( @ErrMsg ,16,-1)

END CATCH
'
/*
print @SqlCmd1
print @SqlCmd2
print @SqlCmd3
print @SqlCmd4
*/

SET @SqlCmd_Combin = @SqlCmd1 + @SqlCmd2 + @SqlCmd3 + @SqlCmd4
	EXEC sp_executesql @SqlCmd_Combin

END

GO

-- =============================================
-- Create date: 2020/03/20
-- Description:	Data Query Logic by PMS.PPIC R16, Import Data to P_OustandingPO
-- =============================================
CREATE PROCEDURE [dbo].[P_ImportOustandingPO_Fty]	

@StartDate Date,
@EndDate Date

AS

BEGIN

declare @SDate varchar(20)  = cast(@StartDate as varchar)--DATEADD(DAY,-150,getdate())
declare @EDate varchar(20) = cast(@EndDate as varchar)--DATEADD(DAY,30,getdate())

SELECT 
	 o.FactoryID
    ,o.BrandID
	,o.ID
	,o.CustPONo
	,o.StyleID
	,oq.BuyerDelivery
	,oq.Seq
	,oq.ShipmodeID
	,[Dest] = c.Alias
	,[Category] =  CASE WHEN o.Category='B' THEN 'Bulk'
						WHEN o.Category='G' THEN'Garment'
						ELSE ''
				   END
	,[PartialShipment]=IIF(PartialShipment.Count > 1 ,'Y','')
	,[Cancelled]=IIF(o.Junk=1,'Y','N')
    ,o.PulloutComplete
	,[OrderQty]= isnull(oq.Qty,0)
    ,ShipQty=isnull(s.ShipQty,0)
    ,o.Qty
into #tmpOrderMain
FROM Production.dbo.Orders o WITH(NOLOCK)
INNER JOIN Production.dbo.Factory f WITH(NOLOCK) ON f.ID=o.FactoryID
LEFT JOIN Production.dbo.Order_QtyShip oq WITH(NOLOCK) ON o.ID=oq.ID
LEFT JOIN Production.dbo.OrderType ot WITH(NOLOCK) ON o.OrderTypeID=ot.ID AND o.BrandID = ot.BrandID
LEFT JOIN Production.dbo.Country c WITH(NOLOCK) on c.id = o.dest
OUTER APPLY(
	SELECT [Count] = COUNT(ID) FROM Production.dbo.Order_QtyShip oqq WITH(NOLOCK) WHERE oqq.Id=o.ID
)PartialShipment
outer apply(
	select ShipQty = sum(podd.ShipQty) 
	from Production.dbo.Pullout_Detail_Detail podd WITH (NOLOCK) 
	inner join Production.dbo.Order_Qty oq WITH (NOLOCK) on oq.id=podd.OrderID 
	and podd.Article= oq.Article and podd.SizeCode=oq.SizeCode
	where podd.OrderID = o.ID
)s
where o.Category IN ('B','G') 
      and isnull(ot.IsGMTMaster,0) = 0
      AND oq.BuyerDelivery >= @SDate
AND oq.BuyerDelivery <= @EDate
AND f.IsProduceFty=1


select 
pd.OrderID,
pd.OrderShipmodeSeq,
[PackingQty] = sum(isnull(pd.ShipQty,0)),
[PackingCarton] = sum(iif(pd.CTNQty = 1,1,0)),
[ClogReceivedCarton] = sum(iif(pd.CTNQty = 1 AND ( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL),1,0)),
[ClogReceivedQty] = sum(iif( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL,pd.ShipQty,0))
into #tmpPackingList_Detail
from Production.dbo.PackingList_Detail pd with (nolock)
where exists(select 1 from #tmpOrderMain main where pd.OrderID = main.ID and 
													pd.OrderShipmodeSeq = main.Seq)
group by pd.OrderID,pd.OrderShipmodeSeq


select   ins.OrderId,
        ins.Location,
        [DQSQty] = count(1),
        [LastDQSOutputDate] = MAX(iif(ins.Status in ('Pass','Fixed'), ins.AddDate, null))
into #tmpInspection_Step1
from [ManufacturingExecution].[dbo].[Inspection] ins WITH(NOLOCK)
where exists( select 1 
                from 
                [Production].[dbo].Orders o WITH(NOLOCK)
                INNER JOIN [Production].[dbo].Factory f WITH(NOLOCK) ON f.ID=o.FactoryID
                LEFT JOIN  [Production].[dbo].Order_QtyShip oq WITH(NOLOCK) ON o.ID=oq.ID
                where o.ID = ins.OrderID 
                        AND oq.BuyerDelivery >= @SDate
AND oq.BuyerDelivery <= @EDate
AND f.IsProduceFty=1
                )
group by ins.OrderId,ins.Location

select OrderId,
[DQSQty] = MIN(DQSQty),
[LastDQSOutputDate] = MAX(LastDQSOutputDate)
into #tmpInspection
from #tmpInspection_Step1
group by OrderId
;

select 
	main.FactoryID
    ,main.BrandID
	,main.ID
	,main.CustPONo
	,main.StyleID
	,main.BuyerDelivery
	,main.Seq
	,main.ShipmodeID
	,main.Dest
	,main.Category
	,main.PartialShipment
	,main.Cancelled
    ,PulloutComplete = case when main.PulloutComplete=1 and main.Qty > isnull(main.ShipQty,0) then 'S'
							when main.PulloutComplete=1 and main.Qty <= isnull(main.ShipQty,0) then 'Y'
							when main.PulloutComplete=0 then 'N'
							end
	,main.OrderQty
	,[PackingCarton] = isnull(pd.PackingCarton,0)
	,[PackingQty] = isnull(pd.PackingQty,0)
	,[ClogReceivedCarton] = isnull(pd.ClogReceivedCarton,0)
	,[ClogReceivedQty]=IIF(main.PartialShipment='Y' ,'NA' ,CAST( ISNULL( pd.ClogReceivedQty,0)  as varchar))
	,[LastCMPOutputDate]=LastCMPOutputDate.Value
    ,[CMPQty]=IIF(PartialShipment='Y' ,'NA', CAST(ISNULL( CMPQty.Value,0)  as varchar))
	,ins.LastDQSOutputDate
	,[DQSQty]=IIF(main.PartialShipment='Y' , 'NA' , CAST( ISNULL( ins.DQSQty,0)  as varchar))
	,[OSTPackingQty]=IIF(main.PartialShipment='Y' , 'NA' , CAST(( ISNULL(main.OrderQty,0) -  ISNULL(pd.PackingQty,0)) as varchar))
	,[OSTCMPQty]=IIF(main.PartialShipment='Y' ,  'NA' , CAST((  ISNULL(main.OrderQty,0) -  ISNULL(CMPQty.Value,0))  as varchar))
	,[OSTDQSQty]=IIF(main.PartialShipment='Y' ,  'NA' ,  CAST(( ISNULL(main.OrderQty,0) -  ISNULL(ins.DQSQty,0))  as varchar))
	,[OSTClogQty]=IIF(main.PartialShipment='Y' , 'NA' , CAST((  ISNULL(main.OrderQty,0) -  ISNULL(pd.ClogReceivedQty,0))  as varchar))
	,[OSTClogCtn]= ISNULL(pd.PackingCarton,0) - ISNULL(pd.ClogReceivedCarton,0)
into #final
from #tmpOrderMain main
left join #tmpPackingList_Detail pd on pd.OrderID = main.id and pd.OrderShipmodeSeq = main.Seq
left join #tmpInspection ins on ins.OrderId = main.ID
OUTER APPLY(
	SELECT [Value]=MAX(s.OutputDate)
	FROM Production.dbo.SewingOutput s WITH(NOLOCK)
	INNER JOIN Production.dbo.SewingOutput_Detail sd WITH(NOLOCK) ON s.ID=sd.ID
	WHERE sd.OrderId=main.ID AND sd.QAQty > 0
)LastCMPOutputDate
OUTER APPLY(
	-- SELECT [Value]=Production.dbo.[getMinCompleteSewQty](main.ID,NULL,NULL) 
	 select [Value] = QAQty
				from (
					select sum(isnull(sdd.QAQty,0)) as QAQty
					from Production.dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
					where sdd.OrderId = main.ID)a 
)CMPQty
 where ( main.OrderQty > ISNULL(pd.PackingQty,0) OR (ISNULL(pd.PackingCarton,0) - ISNULL(pd.ClogReceivedCarton,0)) <> 0 ) 

DROP TABLE #tmpOrderMain,#tmpPackingList_Detail,#tmpInspection,#tmpInspection_Step1


MERGE INTO P_OustandingPO t --�n�Qinsert/update/delete����
USING #Final s --�Q�ѦҪ���
   ON t.FactoryID=s.FactoryID  
   AND t.orderid=s.id 
   AND t.seq = s.seq 

WHEN MATCHED THEN   
    UPDATE SET 
		t.CustPONo =  s.CustPONo,
		t.StyleID =  s.StyleID,
		t.BrandID = s.BrandID,
		t.BuyerDelivery = s.BuyerDelivery,
		t.ShipModeID =  s.ShipModeID,
		t.Category =  s.Category,
		t.PartialShipment =  s.PartialShipment,
		t.Junk =  s.Cancelled,
		t.OrderQty =  s.OrderQty,
		t.PackingCtn =  s.PackingCarton,
		t.PackingQty =  s.PackingQty,
		t.ClogRcvCtn =  s.ClogReceivedCarton,
		t.ClogRcvQty =  s.ClogReceivedQty,
		t.LastCMPOutputDate =  s.LastCMPOutputDate,
		t.CMPQty =  s.CMPQty,
		t.LastDQSOutputDate =  s.LastDQSOutputDate,
		t.DQSQty =  s.DQSQty,
		t.OSTPackingQty =  s.OSTPackingQty,
		t.OSTCMPQty =  s.OSTCMPQty,
		t.OSTDQSQty =  s.OSTDQSQty,
		t.OSTClogQty =  s.OSTClogQty,
		t.OSTClogCtn =  s.OSTClogCtn,
		t.PulloutComplete = s.PulloutComplete,
		t.dest = s.dest

WHEN NOT MATCHED THEN
    INSERT ([FactoryID]
		  ,[OrderID]
		  ,[CustPONo]
		  ,[StyleID]
		  ,[BrandID]
		  ,[BuyerDelivery]
		  ,[Seq]
		  ,[ShipModeID]
		  ,[Category]
		  ,[PartialShipment]
		  ,[Junk]
		  ,[OrderQty]
		  ,[PackingCtn]
		  ,[PackingQty]
		  ,[ClogRcvCtn]
		  ,[ClogRcvQty]
		  ,[LastCMPOutputDate]
		  ,[CMPQty]
		  ,[LastDQSOutputDate]
		  ,[DQSQty]
		  ,[OSTPackingQty]
		  ,[OSTCMPQty]
		  ,[OSTDQSQty]
		  ,[OSTClogQty]
		  ,[OSTClogCtn]
		  ,[PulloutComplete]
		  ,[dest]
)
	VALUES (
			s.FactoryID,
			s.id,
			s.CustPONo,
			s.StyleID,
			s.BrandID,
			s.BuyerDelivery,
			s.Seq,
			s.ShipModeID,
			s.Category,
			s.PartialShipment,
			s.Cancelled,
			s.OrderQty,
			s.PackingCarton,
			s.PackingQty,
			s.ClogReceivedCarton,
			s.ClogReceivedQty,
			s.LastCMPOutputDate,
			s.CMPQty,
			s.LastDQSOutputDate,
			s.DQSQty,
			s.OSTPackingQty,
			s.OSTCMPQty,
			s.OSTDQSQty,
			s.OSTClogQty,
			s.OSTClogCtn,
			s.PulloutComplete,
			s.dest
		  )
	when not matched by source AND T.BuyerDelivery between @SDate and @EDate
	then 
			delete;

	drop table #final
End
GO



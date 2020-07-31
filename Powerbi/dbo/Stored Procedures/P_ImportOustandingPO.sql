USE [PBIReportData]
GO
/****** Object:  StoredProcedure [dbo].[P_ImportOustandingPO]    Script Date: 02/18/2020 上午 11:55:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists(select * from sys.procedures where name ='P_ImportOustandingPO')
begin
	drop procedure P_ImportOustandingPO;
end
go


CREATE PROCEDURE [dbo].[P_ImportOustandingPO]

@StartDate Date,
@EndDate Date,
@LinkServerName varchar(50)
	
AS

BEGIN

declare @SqlCmd_Combin nvarchar(max) =''
declare @SqlCmd1 nvarchar(max) ='';
declare @SqlCmd2 nvarchar(max) ='';
declare @SqlCmd3 nvarchar(max) ='';

declare @SDate varchar(20)  = cast(@StartDate as varchar)--DATEADD(DAY,-150,getdate())
declare @EDate varchar(20) = cast(@EndDate as varchar)--DATEADD(DAY,30,getdate())

SET @SqlCmd1 = '


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
	,[Category] =  CASE WHEN o.Category=''B'' THEN ''Bulk'' 
						WHEN o.Category=''G'' THEN ''Garment'' 
						ELSE ''''
				   END
	,[PartialShipment]=IIF(PartialShipment.Count > 1 ,''Y'','''')
	,[Cancelled]=IIF(o.Junk=1,''Y'',''N'')
    ,o.PulloutComplete
	,[OrderQty]= isnull(oq.Qty,0)
    ,ShipQty=isnull(s.ShipQty,0)
    ,o.Qty
into #tmpOrderMain
FROM ['+@LinkServerName+'].Production.dbo.Orders o WITH(NOLOCK)
INNER JOIN ['+@LinkServerName+'].Production.dbo.Factory f WITH(NOLOCK) ON f.ID=o.FactoryID
LEFT JOIN ['+@LinkServerName+'].Production.dbo.Order_QtyShip oq WITH(NOLOCK) ON o.ID=oq.ID
LEFT JOIN ['+@LinkServerName+'].Production.dbo.OrderType ot WITH(NOLOCK) ON o.OrderTypeID=ot.ID AND o.BrandID = ot.BrandID
LEFT JOIN ['+@LinkServerName+'].Production.dbo.Country c WITH(NOLOCK) on c.id = o.dest

OUTER APPLY(
	SELECT [Count] = COUNT(ID) FROM ['+@LinkServerName+'].Production.dbo.Order_QtyShip oqq WITH(NOLOCK) WHERE oqq.Id=o.ID
)PartialShipment
outer apply(
	select ShipQty = sum(podd.ShipQty) 
	from ['+@LinkServerName+'].Production.dbo.Pullout_Detail_Detail podd WITH (NOLOCK) 
	inner join ['+@LinkServerName+'].Production.dbo.Order_Qty oq WITH (NOLOCK) on oq.id=podd.OrderID 
	and podd.Article= oq.Article and podd.SizeCode=oq.SizeCode
	where podd.OrderID = o.ID
)s
where o.Category IN (''B'',''G'') 
      and isnull(ot.IsGMTMaster,0) = 0
      AND oq.BuyerDelivery >= '''+@SDate+''' 
AND oq.BuyerDelivery <= '''+@EDate+''' 
AND f.IsProduceFty=1


select 
pd.OrderID,
pd.OrderShipmodeSeq,
[PackingQty] = sum(isnull(pd.ShipQty,0)),
[PackingCarton] = sum(iif(pd.CTNQty = 1,1,0)),
[ClogReceivedCarton] = sum(iif(pd.CTNQty = 1 AND ( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL),1,0)),
[ClogReceivedQty] = sum(iif( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL,pd.ShipQty,0))
into #tmpPackingList_Detail
from ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd with (nolock)
where exists(select 1 from #tmpOrderMain main where pd.OrderID = main.ID and 
													pd.OrderShipmodeSeq = main.Seq)
group by pd.OrderID,pd.OrderShipmodeSeq


select   ins.OrderId,
        ins.Location,
        [DQSQty] = count(1),
        [LastDQSOutputDate] = MAX(iif(ins.Status in (''Pass'',''Fixed''), ins.AddDate, null))
into #tmpInspection_Step1
from ['+@LinkServerName+'].[ManufacturingExecution].[dbo].[Inspection] ins WITH(NOLOCK)
where exists( select 1 
                from 
                ['+@LinkServerName+'].[Production].[dbo].Orders o WITH(NOLOCK)
                INNER JOIN ['+@LinkServerName+'].[Production].[dbo].Factory f WITH(NOLOCK) ON f.ID=o.FactoryID
                LEFT JOIN  ['+@LinkServerName+'].[Production].[dbo].Order_QtyShip oq WITH(NOLOCK) ON o.ID=oq.ID
                where o.ID = ins.OrderID 
                        AND oq.BuyerDelivery >= '''+@SDate+''' 
AND oq.BuyerDelivery <= '''+@EDate+''' 
AND f.IsProduceFty=1
                )
group by ins.OrderId,ins.Location

select OrderId,
[DQSQty] = MIN(DQSQty),
[LastDQSOutputDate] = MAX(LastDQSOutputDate)
into #tmpInspection
from #tmpInspection_Step1
group by OrderId
';

SET @SqlCmd2 = 'select 
	main.FactoryID
    ,main.BrandID
	,main.ID
	,main.CustPONo
	,main.StyleID
	,main.BuyerDelivery
	,main.Seq
	,main.ShipmodeID
	,main.dest
	,main.Category
	,main.PartialShipment
	,main.Cancelled
    ,PulloutComplete = case when main.PulloutComplete=1 and main.Qty > isnull(main.ShipQty,0) then ''S''
							when main.PulloutComplete=1 and main.Qty <= isnull(main.ShipQty,0) then ''Y''
							when main.PulloutComplete=0 then ''N''
							end
	,main.OrderQty
	,[PackingCarton] = isnull(pd.PackingCarton,0)
	,[PackingQty] = isnull(pd.PackingQty,0)
	,[ClogReceivedCarton] = isnull(pd.ClogReceivedCarton,0)
	,[ClogReceivedQty]=IIF(main.PartialShipment=''Y'' ,''NA'' ,CAST( ISNULL( pd.ClogReceivedQty,0)  as varchar))
	,[LastCMPOutputDate]=LastCMPOutputDate.Value
    ,[CMPQty]=IIF(PartialShipment=''Y'' ,''NA'', CAST(ISNULL( CMPQty.Value,0)  as varchar))
	,ins.LastDQSOutputDate
	,[DQSQty]=IIF(main.PartialShipment=''Y'' , ''NA'' , CAST( ISNULL( ins.DQSQty,0)  as varchar))
	,[OSTPackingQty]=IIF(main.PartialShipment=''Y'' , ''NA'' , CAST(( ISNULL(main.OrderQty,0) -  ISNULL(pd.PackingQty,0)) as varchar))
	,[OSTCMPQty]=IIF(main.PartialShipment=''Y'' , ''NA'' , CAST((  ISNULL(main.OrderQty,0) -  ISNULL(CMPQty.Value,0))  as varchar))
	,[OSTDQSQty]=IIF(main.PartialShipment=''Y'' , ''NA'' ,  CAST(( ISNULL(main.OrderQty,0) -  ISNULL(ins.DQSQty,0))  as varchar))
	,[OSTClogQty]=IIF(main.PartialShipment=''Y'' , ''NA'' , CAST((  ISNULL(main.OrderQty,0) -  ISNULL(pd.ClogReceivedQty,0))  as varchar))
	,[OSTClogCtn]= ISNULL(pd.PackingCarton,0) - ISNULL(pd.ClogReceivedCarton,0)
into #final
from #tmpOrderMain main
left join #tmpPackingList_Detail pd on pd.OrderID = main.id and pd.OrderShipmodeSeq = main.Seq
left join #tmpInspection ins on ins.OrderId = main.ID
OUTER APPLY(
	SELECT [Value]=MAX(s.OutputDate)
	FROM ['+@LinkServerName+'].Production.dbo.SewingOutput s WITH(NOLOCK)
	INNER JOIN ['+@LinkServerName+'].Production.dbo.SewingOutput_Detail sd WITH(NOLOCK) ON s.ID=sd.ID
	WHERE sd.OrderId=main.ID AND sd.QAQty > 0
)LastCMPOutputDate
OUTER APPLY(
	-- SELECT [Value]=['+@LinkServerName+'].Production.dbo.[getMinCompleteSewQty](main.ID,NULL,NULL) 
	 select [Value] = QAQty
				from (
					select sum(isnull(sdd.QAQty,0)) as QAQty
					from ['+@LinkServerName+'].Production.dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
					where sdd.OrderId = main.ID)a 
)CMPQty

DROP TABLE #tmpOrderMain,#tmpPackingList_Detail,#tmpInspection,#tmpInspection_Step1
'

SET @SqlCmd3= '
BEGIN TRY
Begin tran

update t
SET 
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
from P_OustandingPO t
inner join #Final s  
		ON t.FactoryID=s.FactoryID  
		AND t.orderid=s.id 
		AND t.seq = s.seq 


insert into P_OustandingPO
select  s.FactoryID,
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
from #Final s
where not exists(
	select 1 from P_OustandingPO t 
	where t.FactoryID = s.FactoryID  
	AND t.orderid = s.id 
	AND t.seq = s.seq 
)
and ((OrderQty > PackingQty) OR (PackingCarton - ClogReceivedCarton <> 0 ))

delete t
from P_OustandingPO t
left join #Final s on t.FactoryID = s.FactoryID  
	AND t.orderid = s.id 
	AND t.seq = s.seq 
where T.BuyerDelivery between '''+@SDate+'''  and '''+@EDate+''' 
	and t.FactoryID in (select distinct FactoryID from #Final ) 
	and s.id IS NULL
	and ((s.OrderQty > s.PackingQty) OR (s.PackingCarton - s.ClogReceivedCarton <> 0 ))

	drop table #final

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

SET @SqlCmd_Combin = @SqlCmd1 + @SqlCmd2 + @SqlCmd3
	EXEC sp_executesql @SqlCmd_Combin

End








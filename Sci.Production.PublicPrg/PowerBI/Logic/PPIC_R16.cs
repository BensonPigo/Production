using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <summary>
    /// 有調整到需要一併更新至BI
    /// </summary>
    public class PPIC_R16
    {
        /// <inheritdoc/>
        public PPIC_R16()
        {
            DBProxy.Current.DefaultTimeout = 7200;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetOustandingPO(PPIC_R16_ViewModel model)
        {
            string sqlWhere = $"where oq.BuyerDelivery >= '{model.BuyerDeliveryFrom.Value.ToString("yyyy/MM/dd")}' AND oq.BuyerDelivery <= '{model.BuyerDeliveryTo.Value.ToString("yyyy/MM/dd")}'";
            string sqlWhereOutstanding = string.Empty;

            #region WHERE條件
            if (model.Is7DayEdit)
            {
                sqlWhere = $@" 
where ((oq.BuyerDelivery >= '{model.BuyerDeliveryFrom.Value.ToString("yyyy/MM/dd")}' AND oq.BuyerDelivery <= '{model.BuyerDeliveryTo.Value.ToString("yyyy/MM/dd")}')
    or (o.EditDate >= Cast(getdate()-7 as date))
    or (o.PulloutCmplDate >= Cast(getdate()-7 as date) AND o.PulloutCmplDate <= getdate())
)" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.BrandID))
            {
                sqlWhere += $" AND o.BrandID = '{model.BrandID}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.MDivisionID))
            {
                sqlWhere += $" AND o.MDivisionID = '{model.MDivisionID}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(model.FactoryID))
            {
                sqlWhere += $" AND o.FtyGroup = '{model.FactoryID}'" + Environment.NewLine;
            }

            if (model.IsOutstanding)
            {
                sqlWhereOutstanding += " where (main.OrderQty > ISNULL(pd.PackingQty,0) OR (ISNULL(pd.PackingCarton,0) - ISNULL(pd.ClogReceivedCarton,0)) <> 0 ) " + Environment.NewLine;
            }

            if (model.IsExcludeSister)
            {
                sqlWhere += " AND f.IsProduceFty=1" + Environment.NewLine;
            }

            if (model.IsExcludeCancelShortage)
            {
                sqlWhere += " AND o.GMTComplete <> 'S'" + Environment.NewLine;
            }

            if (model.IsBookingOrder)
            {
                sqlWhere += " AND isnull(ot.IsGMTMaster,0) = 0" + Environment.NewLine;
            }

            if (model.IsJunk)
            {
                sqlWhere += " AND o.Junk = 0 " + Environment.NewLine;
            }
            #endregion

            #region 組SQL

            string sqlcmd = $@"
SELECT 
	 o.FactoryID
    ,o.BrandID
	,o.ID
	,o.CustPONo
	,o.StyleID
	,oq.BuyerDelivery
	,[Seq] = ISNULL(oq.Seq,'')
	,oq.ShipmodeID
    ,[Dest] = c.Alias
	,[Category] =  CASE WHEN o.Category='B' THEN 'Bulk' 
						WHEN o.Category='G' THEN 'Garment' 
						ELSE ''
				   END
	,[PartialShipment]=IIF(PartialShipment.Count > 1 ,'Y','')
	,[Cancelled]=IIF(o.Junk=1,'Y','N')
    ,o.PulloutComplete
	,[OrderQty] = isnull(oq.Qty,0)
    ,[ShipQty] = isnull(s.ShipQty,0)
    ,o.Qty
	,f.KPICode 
	,CancelledButStillNeedProduction = IIF(o.NeedProduction = 0, 'N','Y')
	,[3rdPartyInspection] = IIF(oq.CFAIs3rdInspect =1,'Y','N')
	,[3rdPartyInspectionResult] = oq.CFA3rdInspectResult
    ,[BookingSP] = CASE WHEN o.Category='G' THEN OrderQtyGarment.value
                        WHEN ot.IsGMTMaster=1 THEN 'Y' 
                   ELSE ''
                   END
	,[CFAInspectionResult] = oq.CFAFinalInspectResult
into #tmpOrderMain
FROM Orders o WITH(NOLOCK)
INNER JOIN Factory f WITH(NOLOCK) ON f.ID=o.FactoryID
LEFT JOIN Order_QtyShip oq WITH(NOLOCK) ON o.ID=oq.ID
LEFT JOIN OrderType ot WITH(NOLOCK) ON o.OrderTypeID=ot.ID AND o.BrandID = ot.BrandID
LEFT JOIN Country c WITH(NOLOCK) on c.id = o.dest
OUTER APPLY(
	SELECT [Count] = COUNT(ID) FROM Order_QtyShip oqq WITH(NOLOCK) WHERE oqq.Id=o.ID
)PartialShipment
outer apply(
    select ShipQty = sum(pd.ShipQty)
    from PackingList_Detail pd WITH(NOLOCK)
    inner join Order_Qty oq WITH(NOLOCK) on oq.ID = pd.OrderID and oq.Article = pd.Article and oq.SizeCode = pd.SizeCode
    inner join PackingList p WITH(NOLOCK) on p.ID = pd.ID
    where p.PulloutID <> ''
    and pd.OrderID = o.ID
)s
outer apply(
    select value = STUFF((
        select CONCAT(',',OrderIDFrom)
        from(
            select distinct OrderIDFrom
            from Order_Qty_Garment WITH(NOLOCK)
            where ID = o.ID
        )s
        for xml path('')
        ),1,1,'')
) OrderQtyGarment 
{sqlWhere}
and o.Category IN ('B','G') and o.Qty <> 0
      

select 
    pd.OrderID,
    pd.OrderShipmodeSeq,
    [PackingQty] = sum(isnull(pd.ShipQty,0)),
    [PackingCarton] = sum(iif(pd.CTNQty = 1,1,0)),
    [ClogReceivedCarton] = sum(iif(pd.CTNQty = 1 AND ( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL),1,0)),
    [ClogReceivedQty] = sum(iif( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL,pd.ShipQty,0))
into #tmpPackingList_Detail
from PackingList_Detail pd with (nolock)
where exists(select 1 from #tmpOrderMain main where pd.OrderID = main.ID and 
													pd.OrderShipmodeSeq = main.Seq)
group by pd.OrderID,pd.OrderShipmodeSeq


select 
*
into #tmpInspection_Step1
from openquery([ExtendServer],'
    select  ins.OrderId,
            ins.Location,
            ins.article,
            ins.size,
            [DQSQty] = count(1),
            [LastDQSOutputDate] = MAX(iif(ins.Status in (''Pass'',''Fixed''), ins.AddDate, null))
    from [ManufacturingExecution].[dbo].[Inspection] ins WITH(NOLOCK)
    where exists(
        select 1 
        from [Production].[dbo].Orders o WITH(NOLOCK)
        INNER JOIN [Production].[dbo].Factory f WITH(NOLOCK) ON f.ID=o.FactoryID
        LEFT JOIN  [Production].[dbo].Order_QtyShip oq WITH(NOLOCK) ON o.ID=oq.ID
        LEFT JOIN [Production].[dbo].OrderType ot WITH(NOLOCK) ON o.OrderTypeID=ot.ID AND o.BrandID = ot.BrandID
        {sqlWhere.ToString().Replace("'", "''")}
        and o.ID = ins.OrderID 
    )
    and ins.Status in (''Pass'',''Fixed'')
    group by ins.OrderId,ins.Location,ins.article,ins.size
')

select OrderId,
    [LastDQSOutputDate] = MAX(LastDQSOutputDate)
into #tmpInspection
from #tmpInspection_Step1
group by OrderId

--要先找每個OrderID所有的Seq,因為可能被篩選掉
SELECT oq.Id,oq.Seq,oq.BuyerDelivery
INTO #tmpOrder_QtyShip
FROM Order_QtyShip oq
WHERE EXISTS(SELECT 1 FROM #tmpOrderMain WHERE ID = oq.Id)

--- DQSQTY 計算分配
SELECT m.Id,m.Seq,t.Article,t.Size, t.DQSQty, Qty = ISNULL(od.Qty, 0)
    ,RowNum = ROW_NUMBER() OVER(PARTITION BY m.Id,t.Article,t.Size ORDER BY m.BuyerDelivery, m.Seq)
INTO #PrepareRn
FROM #tmpOrder_QtyShip m
INNER JOIN #tmpInspection_Step1 t on t.OrderID = m.ID
Left JOIN Order_QtyShip_Detail od on od.Id = m.id AND od.Seq = m.Seq AND od.Article = t.Article AND od.SizeCode = t.Size
ORDER by m.Id,t.Article,t.Size,m.Seq

SELECT m.Id, m.Article, m.Size, RowNum, m.Seq, m.DQSQty, m.Qty
    ,remaining_DQSQty = m.DQSQty - SUM(m.Qty) OVER(PARTITION BY m.Id,m.Article,m.Size ORDER BY m.RowNum)
    ,MAXRowNum = MAX(m.RowNum) OVER(PARTITION BY m.Id,m.Article,m.Size)
INTO #PrepareCalculate
FROM #PrepareRn m
ORDER by m.Id,m.Article,m.Size,m.RowNum

SELECT *, Calculate_previous_remaining_DQSQty = ISNULL((LAG(remaining_DQSQty) OVER(PARTITION BY Id,Article,Size ORDER BY RowNum)), 0) 
INTO #PrepareCalculate2
FROM #PrepareCalculate

SELECT *, CalculateThisRowDQSQty = IIF(Calculate_previous_remaining_DQSQty < 0, 0, Calculate_previous_remaining_DQSQty)
INTO #PrepareCalculate3
FROM #PrepareCalculate2
--計算出當前這筆還可使用數
SELECT *, ThisRowDQSQty = IIF(RowNum = 1, DQSQty, CalculateThisRowDQSQty)
INTO #PrepareCalculate4
FROM #PrepareCalculate3

SELECT Id, Article, Size, RowNum, Seq, ThisRowDQSQty, Qty
    ,AssignedQty = CASE
        WHEN RowNum = MAXRowNum THEN ThisRowDQSQty
        WHEN ThisRowDQSQty >= QTY THEN QTY
        ELSE ThisRowDQSQty
        END
INTO #tmpAssignedQty_by_IDArtcleSizeSeq
FROM #PrepareCalculate4

SELECT ID,Seq,DQSQty = Sum(AssignedQty)
INTO #tmpDQSQty
from #tmpAssignedQty_by_IDArtcleSizeSeq
GROUP BY ID,Seq
--DQSQTY 計算結尾

--CMPQty 計算分配
SELECT sodd.OrderId, sodd.Article, sodd.SizeCode, QAQty = SUM(sodd.QAQty)--可分配總數
INTO #tmpSewingOutput_Detail_Detail
FROM SewingOutput_Detail_Detail sodd WITH(NOLOCK)
WHERE EXISTS(SELECT 1 FROM #tmpOrderMain WHERE ID = sodd.OrderId)
GROUP BY sodd.OrderId, sodd.Article, sodd.SizeCode

SELECT m.Id, m.Seq, t.Article, t.SizeCode, t.QAQty, Qty = ISNULL(od.Qty, 0)
    ,RowNum = ROW_NUMBER() OVER(PARTITION BY m.Id,t.Article,t.SizeCode ORDER BY m.BuyerDelivery, m.Seq)
INTO #PrepareRn_CMPQty
FROM #tmpOrder_QtyShip m
INNER JOIN #tmpSewingOutput_Detail_Detail t on t.OrderID = m.ID
Left JOIN Order_QtyShip_Detail od on od.Id = m.id AND od.Seq = m.Seq AND od.Article = t.Article AND od.SizeCode = t.SizeCode
ORDER by m.Id,t.Article,t.SizeCode,m.Seq

SELECT m.Id, m.Article, m.SizeCode, RowNum, m.Seq, m.QAQty, m.Qty,
    remaining_QAQty = m.QAQty - SUM(m.Qty) OVER(PARTITION BY m.Id,m.Article,m.SizeCode ORDER BY m.RowNum),
    MAXRowNum = MAX(m.RowNum) OVER(PARTITION BY m.Id,m.Article,m.SizeCode)
INTO #PrepareCalculate_CMPQty
FROM #PrepareRn_CMPQty m
ORDER by m.Id,m.Article,m.SizeCode,m.RowNum

SELECT *, Calculate_previous_remaining_QAQty = ISNULL((LAG(remaining_QAQty) OVER(PARTITION BY Id,Article,SizeCode ORDER BY RowNum)), 0) 
INTO #PrepareCalculate2_CMPQty
FROM #PrepareCalculate_CMPQty

SELECT *, CalculateThisRowQAQty = IIF(Calculate_previous_remaining_QAQty < 0, 0, Calculate_previous_remaining_QAQty)
INTO #PrepareCalculate3_CMPQty
FROM #PrepareCalculate2_CMPQty
--計算出當前這筆還可使用數
SELECT *, ThisRowQAQty = IIF(RowNum = 1, QAQty, CalculateThisRowQAQty)
INTO #PrepareCalculate4_CMPQty
FROM #PrepareCalculate3_CMPQty

SELECT *
    ,AssignedQty = CASE
        WHEN RowNum = MAXRowNum THEN ThisRowQAQty
        WHEN ThisRowQAQty >= QTY THEN QTY
        ELSE ThisRowQAQty
        END
INTO #tmpAssignedQty_by_IDArtcleSizeSeq_CMPQty
FROM #PrepareCalculate4_CMPQty

SELECT ID,Seq,QAQty = Sum(AssignedQty)
INTO #tmpCMPQty
from #tmpAssignedQty_by_IDArtcleSizeSeq_CMPQty
GROUP BY ID,Seq
--CMPQty 計算結尾

-- 新增欄位 [CFA inspection result]
select
    ID,
    CTNStartNo,
    OrderID,
    OrigID,
    OrigOrderID,
    OrigCTNStartNo,
    OrderShipmodeSeq
into #PackingList_Detail
from Production.dbo.PackingList_Detail pld WITH(NOLOCK)
where exists(select 1 from #tmpOrderMain where ID = pld.OrderID)
and CTNQty > 0

select OrderID, OrderShipmodeSeq, AddDate = MAX(AddDate)
into #CReceive
from (
select pd.OrderID, OrderShipmodeSeq, c.AddDate
from #PackingList_Detail pd 
inner join Production.dbo.ClogReceive c WITH (NOLOCK) on pd.ID = c.PackingListID 
and pd.OrderID = c.OrderID 
and pd.CTNStartNo = c.CTNStartNo
where c.PackingListID != ''
    and c.OrderID != ''
    and c.CTNStartNo != ''
 
union all -- 找拆箱
select OrderID = pd.OrigOrderID, OrderShipmodeSeq, c.AddDate
from #PackingList_Detail pd 
inner join Production.dbo.ClogReceive c WITH (NOLOCK) on pd.OrigID = c.PackingListID
and pd.OrigOrderID = c.OrderID
and pd.OrigCTNStartNo = c.CTNStartNo
where c.PackingListID != ''
    and c.OrderID != ''
    and c.CTNStartNo != ''
) t
where not exists (
	-- 每個紙箱必須放在 Clog（ReceiveDate 有日期）
	select 1 
	from Production.dbo.PackingList_Detail pdCheck WITH(NOLOCK)
	where t.OrderID = pdCheck.OrderID 
	and t.OrderShipmodeSeq = pdCheck.OrderShipmodeSeq
	and pdCheck.ReceiveDate is null
)
group by OrderID, OrderShipmodeSeq

;

select main.KPICode
	,main.FactoryID
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
	,[BookingSP] = ISNULL(main.BookingSP, '')
    ,main.Cancelled
	,main.CancelledButStillNeedProduction
    ,PulloutComplete = case when main.PulloutComplete=1 and main.Qty > isnull(main.ShipQty,0) then 'S'
							when main.PulloutComplete=1 and main.Qty <= isnull(main.ShipQty,0) then 'Y'
							when main.PulloutComplete=0 then 'N'
							end
	,main.OrderQty
	,[PackingCarton] = isnull(pd.PackingCarton,0)
	,[PackingQty] = isnull(pd.PackingQty,0)
	,[ClogReceivedCarton] = isnull(pd.ClogReceivedCarton,0)
	,[ClogReceivedQty] = CAST( ISNULL( pd.ClogReceivedQty,0)  as varchar)
	,[LastCMPOutputDate]=LastCMPOutputDate.Value
    ,[CMPQty] = CAST(ISNULL(cq.QAQty, 0) as varchar)
	,ins.LastDQSOutputDate
	,[DQSQty] = CAST(ISNULL(dq.DQSQty, 0) as varchar)
	,[OSTPackingQty]=IIF(main.PartialShipment='Y' , 'NA' , CAST(( ISNULL(main.OrderQty,0) -  ISNULL(pd.PackingQty,0)) as varchar))
	,[OSTCMPQty] = CAST((ISNULL(main.OrderQty,0) - ISNULL(cq.QAQty, 0)) as varchar)
	,[OSTDQSQty] = ISNULL(main.OrderQty, 0) - ISNULL(dq.DQSQty, 0)
	,[OSTClogQty]=IIF(main.PartialShipment='Y' , 'NA' , CAST((  ISNULL(main.OrderQty,0) -  ISNULL(pd.ClogReceivedQty,0))  as varchar))
	,[OSTClogCtn]= ISNULL(pd.PackingCarton,0) - ISNULL(pd.ClogReceivedCarton,0)
	,main.CFAInspectionResult
	,main.[3rdPartyInspection]
	,main.[3rdPartyInspectionResult]
	,[LastCartonReceivedDate]  = c.AddDate
    ,[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
    ,[BIInsertDate] = GETDATE()
from #tmpOrderMain main
left join #tmpPackingList_Detail pd on pd.OrderID = main.id and pd.OrderShipmodeSeq = main.Seq
left join #tmpInspection ins on ins.OrderId = main.ID
left join #CReceive c on c.OrderID = main.id and c.OrderShipmodeSeq = main.Seq
LEFT JOIN #tmpDQSQty dq on dq.ID = main.ID AND dq.Seq = main.Seq
LEFT JOIN #tmpCMPQty cq on cq.ID = main.ID AND cq.Seq = main.Seq
OUTER APPLY(
	SELECT [Value]=MAX(s.OutputDate)
	FROM SewingOutput s WITH(NOLOCK)
	INNER JOIN SewingOutput_Detail sd WITH(NOLOCK) ON s.ID=sd.ID
	WHERE sd.OrderId=main.ID AND sd.QAQty > 0
)LastCMPOutputDate
{sqlWhereOutstanding}
order by main.ID

drop table #tmpOrderMain,#tmpPackingList_Detail,#tmpInspection_Step1,#tmpInspection
drop table #PrepareCalculate,#PrepareCalculate2,#tmpAssignedQty_by_IDArtcleSizeSeq,#tmpDQSQty,#PrepareRn,#tmpOrder_QtyShip
drop table #tmpSewingOutput_Detail_Detail,#PrepareRn_CMPQty,#PrepareCalculate_CMPQty,#PrepareCalculate2_CMPQty,#tmpAssignedQty_by_IDArtcleSizeSeq_CMPQty,#tmpCMPQty
";
            #endregion

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlcmd, null, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }
    }
}

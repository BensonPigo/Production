using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Planning_P08
    {
        /// <inheritdoc/>
        public Planning_P08()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <summary>
        /// Get SqlCmd
        /// </summary>
        /// <param name="model">查詢資料</param>
        /// <returns>String Where</returns>
        public Base_ViewModel GetPlanning_P08(Planning_P08_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@MDivisionID", SqlDbType.VarChar, 8) { Value = (object)model.MDivisionID ?? DBNull.Value },
                new SqlParameter("@FactoryID", SqlDbType.VarChar, 8) { Value = (object)model.FactoryID ?? DBNull.Value },
                new SqlParameter("@SewingSDate", SqlDbType.Date) { Value = model.SewingSDate.HasValue ? (object)model.SewingSDate.Value : DBNull.Value },
                new SqlParameter("@SewingEDate", SqlDbType.Date) { Value = model.SewingEDate.HasValue ? (object)model.SewingEDate.Value : DBNull.Value },
                new SqlParameter("@SewingInlineSDate", SqlDbType.Date) { Value = model.SewingInlineSDate.HasValue ? (object)model.SewingInlineSDate.Value : DBNull.Value },
                new SqlParameter("@SewingInlineEDate", SqlDbType.Date) { Value = model.SewingInlineEDate.HasValue ? (object)model.SewingInlineEDate.Value : DBNull.Value },
                new SqlParameter("@SewingOfflineSDate", SqlDbType.Date) { Value = model.SewingOfflineSDate.HasValue ? (object)model.SewingOfflineSDate.Value : DBNull.Value },
                new SqlParameter("@SewingOfflineEDate", SqlDbType.Date) { Value = model.SewingOfflineEDate.HasValue ? (object)model.SewingOfflineEDate.Value : DBNull.Value },
            };

            #region sqlWhere
            string sqlWhere = string.Empty;

            if (model.OnlySchema)
            {
                sqlWhere += "AND 1 = 0";
            }

            // 特殊條件 Sewing Date Range 找出 SewingSchedule.Inline ~ SewingSchedule.Offline 有交集
            if (model.SewingSDate.HasValue && model.SewingEDate.HasValue)
            {
                sqlWhere += $@"
AND (
    -- 狀況 1: @SewingSDate 在 Inline 和 Offline 之間
    (@SewingSDate BETWEEN CAST(ss.Inline AS DATE) AND CAST(ss.Offline AS DATE))
    -- 狀況 2: @SewingEDate 在 Inline 和 Offline 之間
    OR (@SewingEDate BETWEEN CAST(ss.Inline AS DATE) AND CAST(ss.Offline AS DATE))
    -- 狀況 3: Inline 在 @SewingSDate 和 @SewingEDate 之間
    OR (CAST(ss.Inline AS DATE) BETWEEN @SewingSDate AND @SewingEDate)
    -- 狀況 4: Offline 在 @SewingSDate 和 @SewingEDate 之間
    OR (CAST(ss.Offline AS DATE) BETWEEN @SewingSDate AND @SewingEDate)
)
";
            }

            if (model.SewingInlineSDate.HasValue && model.SewingInlineEDate.HasValue)
            {
                sqlWhere += $@"
AND CAST(ss.Inline AS DATE) BETWEEN @SewingInlineSDate AND @SewingInlineEDate";
            }

            if (model.SewingOfflineSDate.HasValue && model.SewingOfflineEDate.HasValue)
            {
                sqlWhere += $@"
AND CAST(ss.Offline AS DATE) BETWEEN @SewingOfflineSDate AND @SewingOfflineEDate";
            }

            if (!string.IsNullOrEmpty(model.MDivisionID))
            {
                sqlWhere += $@"
AND ss.MDivisionID = @MDivisionID ";
            }

            if (!string.IsNullOrEmpty(model.FactoryID))
            {
                sqlWhere += $@"
AND ss.FactoryID = @FactoryID";
            }

            #endregion

            if (string.IsNullOrEmpty(sqlWhere))
            {
                return new Base_ViewModel() { Result = new Ict.DualResult(false, "Date can not all be empty!"), Dt = new DataTable() };
            }

            string sqlBISource1 = string.Empty;
            string sqlBI_tmpSumDailyStdQty = string.Empty;
            string sqlAlloQty = string.Empty;
            string sqlBIFinalColumn = string.Empty;
            string sqlBILeftJoin = string.Empty;
            string sqlNIdropTable = string.Empty;
            if (model.IsBI)
            {
                sqlBISource1 = @"
--BI 需要組合欄位資訊
SELECT
    ssd.Article
   ,ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
INTO #tmpSewingSchedule_Detail
FROM SewingSchedule_Detail ssd WITH (NOLOCK)
INNER JOIN #tmpSewingSchedule ss ON ss.ID = ssd.ID

-- BI By OrderID 欄位
SELECT DISTINCT OrderID
INTO #tmpOrderID
FROM #tmpSewingSchedule

SELECT
     o.OrderID
    ,Consumption = SUM(w.ConsPC  * wd.Qty)
INTO #tmpByOrderIDConsumption
FROM #tmpOrderID o
INNER JOIN WorkOrder_Distribute wd WITH (NOLOCK) ON wd.OrderID = o.OrderID
INNER JOIN WorkOrder w WITH (NOLOCK) ON w.Ukey = wd.WorkOrderUkey
GROUP BY o.OrderID

SELECT DISTINCT
    OrderID = ot.ID
   ,[Artwork] = at.Abbreviation + ':' + IIF(ot.Qty > 0, CONVERT(VARCHAR, ot.Qty), CONVERT(VARCHAR, TMS))
INTO #tmpArtwork
FROM Order_TmsCost ot WITH (NOLOCK)
INNER JOIN ArtworkType at WITH (NOLOCK) ON ot.ArtworkTypeID = at.ID
WHERE (ot.Price > 0 OR at.Classify IN ('O', 'I'))
AND (at.Classify IN ('S', 'I') OR at.IsSubprocess = 1)
AND (ot.TMS > 0 OR ot.Qty > 0)
AND at.Abbreviation != ''
AND EXISTS (SELECT 1 FROM #tmpOrderID o WHERE ot.ID = o.OrderID)

SELECT
     o.OrderID
    ,Artwork = STUFF((
        SELECT CONCAT(',', Artwork)
        FROM #tmpArtwork art
        WHERE art.OrderID = o.OrderID
        ORDER BY Artwork
        FOR XML PATH ('')
    ), 1, 1, '')    
INTO #tmpByOrderArtwork
FROM #tmpOrderID o

-- 找出 OrderID 有的 WorkOrderUkey 底下所有數量
SELECT
     wd.WorkOrderUkey
    ,wd.OrderID
    ,wd.Qty
    ,w.Cons
INTO #tmpAllWorkOrder
FROM WorkOrder_Distribute wd WITH (NOLOCK)
INNER JOIN WorkOrder w WITH (NOLOCK) ON w.Ukey = wd.WorkOrderUkey
WHERE EXISTS ( -- 找到有哪些 WorkOrderUkey
    SELECT 1
    FROM #tmpOrderID o 
    INNER JOIN WorkOrder_Distribute wdo WITH (NOLOCK) ON wdo.OrderID = o.OrderID
    INNER JOIN CuttingOutput_Detail cod WITH (NOLOCK) ON cod.WorkOrderUkey = wdo.WorkOrderUkey -- 只計算已經建立 P20, 同 Cutting P03 規則
    WHERE wdo.WorkOrderUkey = wd.WorkOrderUkey
)

SELECT
     WorkOrderUkey
    ,Qty = SUM(Qty) -- by WorkOrderUkey 分母
    ,A.Cons
INTO #tmpDenominator
FROM #tmpAllWorkOrder A
GROUP BY A.WorkOrderUkey, A.Cons

SELECT
     WorkOrderUkey
    ,A.OrderID
    ,Qty = SUM(Qty) -- by WorkOrderUkey, OrderID 分子
INTO #tmpNumerator
FROM #tmpAllWorkOrder A
INNER JOIN #tmpOrderID o ON o.OrderID = A.OrderID
GROUP BY A.WorkOrderUkey, A.OrderID

-- by OrderID 計算 Cons 比例
SELECT
     n.OrderID
    ,ActConsOutput = SUM(d.Cons * IIF(d.Qty = 0, 0, (n.Qty / d.Qty)))
INTO #tmpByOrderIDActConsumption
FROM #tmpNumerator n
INNER JOIN #tmpDenominator d ON d.WorkOrderUkey = n.WorkOrderUkey
GROUP BY n.OrderID
";
                sqlAlloQty = @",AlloQty = (SELECT SUM(AlloQty) FROM SewingSchedule_Detail ssd WITH (NOLOCK) WHERE ssd.ID = ss.ID)";
                sqlBI_tmpSumDailyStdQty = @"
   ,Article = STUFF((
        SELECT CONCAT(',', Article)
        FROM (
            SELECT DISTINCT
                ssd.Article
            FROM #tmpSewingSchedule_Detail ssd WITH (NOLOCK)
            WHERE ssd.SewingLineID = ss.SewingLineID
            AND ssd.OrderID = ss.OrderID
            AND ssd.FactoryID = ss.FactoryID
        ) s
        FOR XML PATH ('')), 1, 1, '')
   ,AlloQty = SUM(AlloQty)
";
                sqlBIFinalColumn = @"
    ,o.MDivisionID
    ,o.BrandID
    ,o.StyleID
    ,o.SeasonID
    ,o.CDCodeNew
    ,std.Article
    ,o.POID
    ,o.Category
    ,o.SCIDelivery
    ,o.BuyerDelivery
    ,std.AlloQty
    ,art.ArtWork
    ,JITDate = o.SewInLine
    ,BCSDate = o.SewInLine
    ,o.ReadyDate
    ,WorkHourPerDay = (SELECT SUM(Hours) FROM WorkHour WHERE WorkHour.SewingLineID = ss.SewingLineID AND WorkHour.FactoryID = ss.FactoryID AND WorkHour.Date = ss.SewingDate)
    ,cons.Consumption -- by SP 計算
    ,ActCons.ActConsOutput -- by SP 計算
";
                sqlBILeftJoin = @"
LEFT JOIN #tmpByOrderArtwork art ON art.OrderID = ss.OrderID
LEFT JOIN #tmpByOrderIDConsumption cons ON cons.OrderID = ss.OrderID
LEFT JOIN #tmpByOrderIDActConsumption ActCons ON ActCons.OrderID = ss.OrderID
";
            }

            #region 組SQL
            string sqlCmd = $@"
SELECT
    ss.ID
   ,ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
   ,ss.APSNo
   {sqlAlloQty}
   ,Inline = CAST(ss.Inline AS DATE)
   ,Offline = CAST(ss.Offline AS DATE)
INTO #tmpSewingSchedule
FROM SewingSchedule ss WITH (NOLOCK)
WHERE 1 = 1
{sqlWhere}

{sqlBISource1}

-- 將 Inline ~ Offline 列出每天日期展開, 使用遞增數字生成器展開日期範圍
;WITH DateRange AS (
    SELECT Increment = ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 FROM master.dbo.spt_values
),ExpandedDates AS (
    SELECT
        ss.SewingLineID
       ,ss.OrderID
       ,ss.FactoryID
       ,SewingDate = DATEADD(DAY, dr.Increment, ss.Inline)
    FROM #tmpSewingSchedule ss
    INNER JOIN DateRange dr ON DATEADD(DAY, dr.Increment, ss.Inline) <= ss.Offline --Inline 遞增到與 Offline 同一天	
)
SELECT DISTINCT
    SewingLineID
   ,OrderID
   ,FactoryID
   ,SewingDate
INTO #tmpPkeyColumns
FROM ExpandedDates e
where not exists (select 1 from Holiday h where e.SewingDate = h.HolidayDate and e.FactoryID = h.FactoryID)
 

--by Pkey 準備每日標準數(總和)--很慢
--by Pkey 會有多筆計畫狀況, EX: 11/01~11/03, 11/03~11/05, 此狀況 11/03 這日期標準數加總 且 Inline 取最小 Offline 取最大
SELECT
    ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
   ,std.Date
   ,StdQty = SUM(std.StdQ)
   ,Inline = MIN(ss.Inline)
   ,Offline = MAX(ss.Offline)
    {sqlBI_tmpSumDailyStdQty}
INTO #tmpSumDailyStdQty
FROM #tmpSewingSchedule ss
OUTER APPLY(SELECT * FROM dbo.getDailystdq (ss.APSNo) std) std
GROUP BY
    ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
   ,std.Date


-- 規則同 Subcon P42 只取最後更新 TransferTime 的成套數
SELECT
    s.OrderID
   ,s.SubProcessID
   ,TransferTime = MAX(s.TransferTime)
INTO #tmp_SetQtyBySubprocess_Last
FROM SetQtyBySubprocess s WITH (NOLOCK)
WHERE EXISTS (SELECT 1 FROM #tmpSewingSchedule t WHERE s.OrderID = t.OrderID)
GROUP BY s.OrderID, s.SubProcessID
 

-- OrderID, SubprocessID, TransferTime 加總
SELECT
    s.OrderID
   ,s.SubprocessID
   ,FinishedQtyBySet = SUM(s.FinishedQtyBySet)
INTO #tmp_SetQtyBySubprocess
FROM (
    SELECT
        s.OrderID
       ,s.Article
       ,s.SizeCode
       ,s.SubprocessID
	   ,[TransferTime] = CAST(s.TransferTime as date)
       ,[FinishedQtyBySet] =
            CASE
                WHEN p.InOutRule = 2 OR p.InOutRule = 3 THEN MIN(s.OutQtyBySet)
                WHEN p.InOutRule = 1 OR p.InOutRule = 4 THEN MIN(s.InQtyBySet)
                ELSE MIN(s.FinishedQtyBySet)
            END
    FROM SetQtyBySubprocess s WITH (NOLOCK)
    INNER JOIN SubProcess p WITH (NOLOCK) ON s.SubprocessID = p.Id
    WHERE EXISTS (
        SELECT 1
        FROM #tmp_SetQtyBySubprocess_Last t
        WHERE t.OrderID = s.OrderID
        AND t.SubProcessID = s.SubProcessID
        AND t.TransferTime = s.TransferTime
    )
    GROUP BY s.OrderID
            ,s.Article
            ,s.SizeCode
            ,s.SubprocessID
            ,p.InOutRule
			,CAST(s.TransferTime as date)
) s
GROUP BY s.OrderID,s.SubprocessID,s.TransferTime 


-- 計算各成套欄位組 
select *
	, [SortingQty] = case when [SortingLagoverQty] = 0 then 0
						when [SortingLagoverQty] > 0 then t.StdQty
						when ABS([SortingLagoverQty]) < t.StdQty then t.StdQty + [SortingLagoverQty]
					else 0
					end
	, [LoadingQty] = case when [LoadingLagoverQty] = 0 then 0
						when [LoadingLagoverQty] > 0 then t.StdQty
						when ABS([LoadingLagoverQty]) < t.StdQty then t.StdQty + [LoadingLagoverQty]
					else 0
					end
	, [ATQty] = case when [ATLagoverQty] = 0 then 0
						when [ATLagoverQty] > 0 then t.StdQty
						when ABS([ATLagoverQty]) < t.StdQty then t.StdQty + [ATLagoverQty]
					else 0
					end
	, [AUTQty] = case when [AUTLagoverQty] = 0 then 0
						when [AUTLagoverQty] > 0 then t.StdQty
						when ABS([AUTLagoverQty]) < t.StdQty then t.StdQty + [AUTLagoverQty]
					else 0
					end
	, [HTQty] = case when [HTLagoverQty] = 0 then 0
						when [HTLagoverQty] > 0 then t.StdQty
						when ABS([HTLagoverQty]) < t.StdQty then t.StdQty + [HTLagoverQty]
					else 0
					end
	, [BOQty] = case when [BOLagoverQty] = 0 then 0
						when [BOLagoverQty] > 0 then t.StdQty
						when ABS([BOLagoverQty]) < t.StdQty then t.StdQty + [BOLagoverQty]
					else 0
					end
	, [FMQty] = case when [FMLagoverQty] = 0 then 0
						when [FMLagoverQty] > 0 then t.StdQty
						when ABS([FMLagoverQty]) < t.StdQty then t.StdQty + [FMLagoverQty]
					else 0
					end
	, [PRTQty] = case when [PRTLagoverQty] = 0 then 0
						when [PRTLagoverQty] > 0 then t.StdQty
						when ABS([PRTLagoverQty]) < t.StdQty then t.StdQty + [PRTLagoverQty]
					else 0
					end
into #tmpSetQtyBySubprocess_Final
from (
	select t.*
		, [SortingLagoverQty] = ISNULL(Lag(sorting.FinishedQtyBySet - t.accStandardQty, 1, sorting.FinishedQtyBySet) over (partition by t.SewingLineID, t.OrderID, t.FactoryID order by t.Date) - t.StdQty, 0)
		, [LoadingLagoverQty] =ISNULL( Lag(loading.FinishedQtyBySet - t.accStandardQty, 1, loading.FinishedQtyBySet) over (partition by t.SewingLineID, t.OrderID, t.FactoryID order by t.Date) - t.StdQty, 0)
		, [ATLagoverQty] = ISNULL(Lag(att.FinishedQtyBySet - t.accStandardQty, 1, att.FinishedQtyBySet) over (partition by t.SewingLineID, t.OrderID, t.FactoryID order by t.Date) - t.StdQty, 0)
		, [AUTLagoverQty] = ISNULL(Lag(aut.FinishedQtyBySet - t.accStandardQty, 1, aut.FinishedQtyBySet) over (partition by t.SewingLineID, t.OrderID, t.FactoryID order by t.Date) - t.StdQty, 0)
		, [HTLagoverQty] = ISNULL(Lag(ht.FinishedQtyBySet - t.accStandardQty, 1, ht.FinishedQtyBySet) over (partition by t.SewingLineID, t.OrderID, t.FactoryID order by t.Date) - t.StdQty, 0)
		, [BOLagoverQty] = ISNULL(Lag(bo.FinishedQtyBySet - t.accStandardQty, 1, bo.FinishedQtyBySet) over (partition by t.SewingLineID, t.OrderID, t.FactoryID order by t.Date) - t.StdQty, 0)
		, [FMLagoverQty] = ISNULL(Lag(fm.FinishedQtyBySet - t.accStandardQty, 1, fm.FinishedQtyBySet) over (partition by t.SewingLineID, t.OrderID, t.FactoryID order by t.Date) - t.StdQty, 0)
		, [PRTLagoverQty] = ISNULL(Lag(prt.FinishedQtyBySet - t.accStandardQty, 1, prt.FinishedQtyBySet) over (partition by t.SewingLineID, t.OrderID, t.FactoryID order by t.Date) - t.StdQty, 0)
	from (
		select t.*
			, accStandardQty = sum(t.StdQty) over(partition by t.SewingLineID, t.OrderID order by t.Date) 
		from #tmpSumDailyStdQty t
	) t
	left join #tmp_SetQtyBySubprocess sorting on t.OrderID = sorting.OrderID and sorting.SubprocessID = 'Sorting'
	left join #tmp_SetQtyBySubprocess loading on t.OrderID = loading.OrderID and loading.SubprocessID = 'Loading'
	left join #tmp_SetQtyBySubprocess att on t.OrderID = att.OrderID and att.SubprocessID = 'AT'
	left join #tmp_SetQtyBySubprocess aut on t.OrderID = aut.OrderID and aut.SubprocessID = 'AUT'
	left join #tmp_SetQtyBySubprocess ht on t.OrderID = ht.OrderID and ht.SubprocessID = 'HT'
	left join #tmp_SetQtyBySubprocess bo on t.OrderID = bo.OrderID and bo.SubprocessID = 'BO'
	left join #tmp_SetQtyBySubprocess fm on t.OrderID = fm.OrderID and fm.SubprocessID = 'FM'
	left join #tmp_SetQtyBySubprocess prt on t.OrderID = prt.OrderID and prt.SubprocessID = 'PRT'
) t




--最後結果
SELECT ss.SewingLineID
	,ss.SewingDate
	,ss.FactoryID
	,ss.OrderID
	,[OrderQty] = o.Qty
	,std.Inline
	,std.Offline
	,StdQty = ISNULL(std.StdQty, 0)
	,[CuttingOutput] = sub.SortingQty
	,sdo.CuttingRemark
	,[LoadingOutput] = sub.LoadingQty
	,sdo.LoadingRemark
	,sdo.LoadingExclusion
	,[ATOutput] = sub.ATQty
	,sdo.ATRemark
	,sdo.ATExclusion
	,[AUTOutput] = sub.AUTQty
	,sdo.AUTRemark
	,sdo.AUTExclusion
	,[HTOutput] = sub.HTQty
	,sdo.HTRemark
	,sdo.HTExclusion
	,[BOOutput] = sub.BOQty
	,sdo.BORemark
	,sdo.BOExclusion
	,[FMOutput] = sub.FMQty
	,sdo.FMRemark
	,sdo.FMExclusion
	,[PRTOutput] = sub.PRTQty
	,sdo.PRTRemark
	,sdo.PRTExclusion
    {sqlBIFinalColumn}
FROM #tmpPkeyColumns ss
INNER JOIN Orders o WITH (NOLOCK) ON o.ID = ss.OrderID
LEFT JOIN #tmpSumDailyStdQty std ON std.Date = ss.SewingDate AND std.SewingLineID = ss.SewingLineID AND std.OrderID = ss.OrderID AND std.FactoryID = ss.FactoryID
LEFT JOIN SewingDailyOutputStatusRecord sdo ON sdo.SewingOutputDate = ss.SewingDate AND sdo.SewingLineID = ss.SewingLineID AND sdo.OrderID = ss.OrderID AND sdo.FactoryID = ss.FactoryID  
LEFT JOIN #tmpSetQtyBySubprocess_Final sub ON sub.Date = ss.SewingDate AND sub.SewingLineID = ss.SewingLineID AND sub.OrderID = ss.OrderID AND sub.FactoryID = ss.FactoryID  
{sqlBILeftJoin}
ORDER BY 
    ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
   ,ss.SewingDate

DROP TABLE #tmp_SetQtyBySubprocess,#tmp_SetQtyBySubprocess_Last,#tmpPkeyColumns,#tmpSetQtyBySubprocess_Final,#tmpSewingSchedule,#tmpSumDailyStdQty";
            #endregion 組SQL

            var result = DBProxy.Current.Select("Production", sqlCmd, listPar, out System.Data.DataTable dataTable);
            return new Base_ViewModel
            {
                Result = result,
                Dt = dataTable,
            };
        }
    }
}

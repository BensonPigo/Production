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

            #region BI 才串上去的字串
            string sqlBISource1 = string.Empty;
            string sqlBI_tmpSumDailyStdQty = string.Empty;
            string sqlBIAlloQty = string.Empty;
            string sqlBIFinalColumn = string.Empty;
            string sqlBILeftJoin = string.Empty;
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

SELECT
    t.OrderID
    ,Artwork
INTO #tmpArtwork
FROM #tmpOrderID t
--從 Planning R15 來的
OUTER APPLY (
    SELECT Artwork = STUFF((
        SELECT CONCAT('+', ArtworkTypeID)
        FROM (
            SELECT DISTINCT [ArtworkTypeId] = IIF(s1.ArtworkTypeId = '', s1.ID, s1.ArtworkTypeId)
            FROM Bundle b1
            INNER JOIN Bundle_Detail_Order bd1 WITH (NOLOCK) ON b1.ID = bd1.iD
            INNER JOIN Bundle_Detail_Art bda1 WITH (NOLOCK) ON bd1.BundleNo = bda1.Bundleno
            INNER JOIN Subprocess s1 WITH (NOLOCK) ON s1.ID = bda1.SubprocessId
            WHERE bd1.Orderid = t.OrderID
        ) tmpartwork
        FOR XML PATH ('')
    ), 1, 1, '')
) Artwork
";
                sqlBIAlloQty = @",AlloQty = (SELECT SUM(AlloQty) FROM SewingSchedule_Detail ssd WITH (NOLOCK) WHERE ssd.ID = ss.ID)";
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
    ,style.CDCodeNew
    ,std.Article
    ,o.POID
    ,o.Category
    ,o.SCIDelivery
    ,o.BuyerDelivery
    ,std.AlloQty
    ,JITDate = DATEADD(DAY, -14, o.SewInLine)
    ,BCSDate = DATEADD(DAY, -2, o.SewInLine)
    ,ReadyDate = DATEADD(DAY, -2, o.SewOffLine)
    ,WorkHourPerDay = (SELECT SUM(Hours) FROM WorkHour WHERE WorkHour.SewingLineID = ss.SewingLineID AND WorkHour.FactoryID = ss.FactoryID AND WorkHour.Date = ss.SewingDate)
    ,cons.Consumption -- by SP 計算
    ,ActCons.ActConsOutput -- by SP 計算
    ,ta.Artwork -- by SP 組合
    ,[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
    ,[BIInsertDate] = GETDATE()
";
                sqlBILeftJoin = @"
LEFT JOIN Style WITH (NOLOCK) ON Style.Ukey = o.StyleUkey
LEFT JOIN #tmpByOrderIDConsumption cons ON cons.OrderID = ss.OrderID
LEFT JOIN #tmpByOrderIDActConsumption ActCons ON ActCons.OrderID = ss.OrderID
LEFT JOIN #tmpArtwork ta ON ta.OrderID = ss.OrderID
";
            }
            #endregion

            #region 組SQL
            string sqlCmd = $@"
SELECT
    ss.ID
   ,ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
   ,ss.APSNo
   {sqlBIAlloQty}
   ,RateInAPSNo = 1.0 * AlloQty / (SELECT SUM(AlloQty) FROM SewingSchedule ss2 WITH (NOLOCK) WHERE ss2.APSNo = ss.APSNo)
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
WHERE EXISTS (SELECT 1 FROM Workhour_Detail WITH(NOLOCK) WHERE SewingLineID = e.SewingLineID AND FactoryID = e.FactoryID AND Date = e.SewingDate)
 

--by Pkey 準備每日標準數(總和)--很慢
--by Pkey 會有多筆計畫狀況, EX: 11/01~11/03, 11/03~11/05, 此狀況 11/03 這日期標準數加總 且 Inline 取最小 Offline 取最大
SELECT
    ss.SewingLineID
   ,ss.OrderID
   ,ss.FactoryID
   ,std.Date
   ,StdQty = CEILING(SUM(std.StdQ * ss.RateInAPSNo))
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

--準備 Bundle_Detail_Art 有的加工段
SELECT DISTINCT bdo.OrderID, bda.SubprocessId
INTO #tmpBundleSubprocessId
FROM Bundle_Detail_Order bdo WITH (NOLOCK)
INNER JOIN Bundle_Detail_Art bda WITH (NOLOCK) ON bda.Bundleno = bdo.Bundleno
WHERE EXISTS (SELECT 1 FROM #tmpSewingSchedule WHERE OrderID = bdo.OrderID)

--每日的累計標準數
SELECT *
    ,StdQty_AccSum = SUM(StdQty) OVER (PARTITION BY SewingLineID, OrderID, FactoryID ORDER BY Date)
INTO #tmpSumDailyStdQty_AccSum
FROM #tmpSumDailyStdQty

--準備前一個工作天的累計標準數, 第一天的前一天標準數 = 0
SELECT *
    ,StdQty_AccSum_BeforeWorkDate = LAG(StdQty_AccSum, 1, 0) OVER (PARTITION BY SewingLineID, OrderID, FactoryID ORDER BY Date)
INTO #tmpSumDailyStdQty_AccSum_BeforeWorkDate
FROM #tmpSumDailyStdQty_AccSum


--推算每天可能成套的數量
--[當前成套總數]達到[到此天的累計標準總數]: 則顯示當天標準數
--[當前成套總數]沒有達到[到前一個工作天的累計標準總數]: 顯示0
--[當前成套總數]沒有達到[到此天的累計標準總數] && 超過 [到前一個工作天的累計標準總數]: 顯示 [當前成套總數]扣除[到前一個工作天的累計標準總數]
SELECT
    t.*
    ,[SortingQty] =
        CASE WHEN sorting.FinishedQtyBySet >= t.StdQty_AccSum THEN t.StdQty
             WHEN sorting.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate < 0 THEN 0
             ELSE sorting.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate END
    ,[LoadingQty] =
        CASE WHEN loading.FinishedQtyBySet >= t.StdQty_AccSum THEN t.StdQty
             WHEN loading.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate < 0 THEN 0
             ELSE loading.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate END
    ,[ATQty] =
        CASE WHEN att.FinishedQtyBySet >= t.StdQty_AccSum THEN t.StdQty
             WHEN att.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate < 0 THEN 0
             ELSE att.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate END
    ,[AUTQty] =
        CASE WHEN aut.FinishedQtyBySet >= t.StdQty_AccSum THEN t.StdQty
             WHEN aut.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate < 0 THEN 0
             ELSE aut.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate END
    ,[HTQty] =
        CASE WHEN ht.FinishedQtyBySet >= t.StdQty_AccSum THEN t.StdQty
             WHEN ht.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate < 0 THEN 0
             ELSE ht.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate END
    ,[BOQty] =
        CASE WHEN bo.FinishedQtyBySet >= t.StdQty_AccSum THEN t.StdQty
             WHEN bo.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate < 0 THEN 0
             ELSE bo.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate END
    ,[FMQty] =
        CASE WHEN fm.FinishedQtyBySet >= t.StdQty_AccSum THEN t.StdQty
             WHEN fm.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate < 0 THEN 0
             ELSE fm.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate END
    ,[PRTQty] =
        CASE WHEN prt.FinishedQtyBySet >= t.StdQty_AccSum THEN t.StdQty
             WHEN prt.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate < 0 THEN 0
             ELSE prt.FinishedQtyBySet - t.StdQty_AccSum_BeforeWorkDate END
into #tmpSetQtyBySubprocess_Final
FROM #tmpSumDailyStdQty_AccSum_BeforeWorkDate t
LEFT JOIN #tmp_SetQtyBySubprocess sorting ON t.OrderID = sorting.OrderID AND sorting.SubprocessID = 'Sorting'
LEFT JOIN #tmp_SetQtyBySubprocess loading ON t.OrderID = loading.OrderID AND loading.SubprocessID = 'Loading'
LEFT JOIN #tmp_SetQtyBySubprocess att ON t.OrderID = att.OrderID AND att.SubprocessID = 'AT'
LEFT JOIN #tmp_SetQtyBySubprocess aut ON t.OrderID = aut.OrderID AND aut.SubprocessID = 'AUT'
LEFT JOIN #tmp_SetQtyBySubprocess ht ON t.OrderID = ht.OrderID AND ht.SubprocessID = 'HT'
LEFT JOIN #tmp_SetQtyBySubprocess bo ON t.OrderID = bo.OrderID AND bo.SubprocessID = 'BO'
LEFT JOIN #tmp_SetQtyBySubprocess fm ON t.OrderID = fm.OrderID AND fm.SubprocessID = 'FM'
LEFT JOIN #tmp_SetQtyBySubprocess prt ON t.OrderID = prt.OrderID AND prt.SubprocessID = 'PRT'




--最後結果
--選擇性的加工段若在 Bundle_Detail_Art 有, 擇要顯示 0
SELECT ss.SewingLineID
	,ss.SewingDate
	,ss.FactoryID
	,ss.OrderID
	,[OrderQty] = o.Qty
	,std.Inline
	,std.Offline
	,StdQty = ISNULL(std.StdQty, 0)
	,[CuttingOutput] = ISNULL(sub.SortingQty, 0)
	,sdo.CuttingRemark
	,[LoadingOutput] = ISNULL(sub.LoadingQty, 0)
	,sdo.LoadingRemark
	,sdo.LoadingExclusion
	,[ATOutput] = ISNULL(sub.ATQty, IIF(EXISTS(SELECT 1 FROM #tmpBundleSubprocessId WHERE SubprocessId = 'AT' AND OrderID = ss.OrderID), 0, NULL))
	,sdo.ATRemark
	,sdo.ATExclusion
	,[AUTOutput] = ISNULL(sub.AUTQty, IIF(EXISTS(SELECT 1 FROM #tmpBundleSubprocessId WHERE SubprocessId = 'AUT' AND OrderID = ss.OrderID), 0, NULL))
	,sdo.AUTRemark
	,sdo.AUTExclusion
	,[HTOutput] = ISNULL(sub.HTQty, IIF(EXISTS(SELECT 1 FROM #tmpBundleSubprocessId WHERE SubprocessId = 'HT' AND OrderID = ss.OrderID), 0, NULL))
	,sdo.HTRemark
	,sdo.HTExclusion
	,[BOOutput] = ISNULL(sub.BOQty, IIF(EXISTS(SELECT 1 FROM #tmpBundleSubprocessId WHERE SubprocessId = 'BO' AND OrderID = ss.OrderID), 0, NULL))
	,sdo.BORemark
	,sdo.BOExclusion
	,[FMOutput] = ISNULL(sub.FMQty, IIF(EXISTS(SELECT 1 FROM #tmpBundleSubprocessId WHERE SubprocessId = 'FM' AND OrderID = ss.OrderID), 0, NULL))
	,sdo.FMRemark
	,sdo.FMExclusion
	,[PRTOutput] = ISNULL(sub.PRTQty, IIF(EXISTS(SELECT 1 FROM #tmpBundleSubprocessId WHERE SubprocessId = 'PRT' AND OrderID = ss.OrderID), 0, NULL))
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

DROP TABLE #tmp_SetQtyBySubprocess,#tmp_SetQtyBySubprocess_Last,#tmpPkeyColumns,#tmpSetQtyBySubprocess_Final,#tmpSewingSchedule,#tmpSumDailyStdQty
    ,#tmpSumDailyStdQty_AccSum  
    ,#tmpSumDailyStdQty_AccSum_BeforeWorkDate
    ,#tmpBundleSubprocessId
";
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

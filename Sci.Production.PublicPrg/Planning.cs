using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.PublicPrg
{
    /// <inheritdoc/>
    public static partial class Prgs
    {
        #region GetWorkDate

        /// <inheritdoc/>
        public static DateTime GetWorkDate(string factoryid, int days, DateTime basicdate)
        {
            string sqlcmd = string.Format(
                @"declare @days as int  = {0} ,@count as int = 0, @bascidate as date = '{1}';
                                                declare @fetchdate as date;

                                                while @days <> 0
                                                begin
	                                                if  DATEPART(WEEKDAY, dateadd(day, @count,@bascidate)) >1
	                                                begin
		                                                DECLARE _cursor CURSOR FOR
		                                                select h.HolidayDate from Holiday h WITH (NOLOCK) 
		                                                where h.FactoryID='{2}'
		                                                and h.HolidayDate = dateadd(day, @count,@bascidate);
		                                                OPEN _cursor;
		                                                FETCH NEXT FROM _cursor INTO @fetchdate; 
		                                                if @@FETCH_STATUS != 0
		                                                begin
			                                                if @days > 0
				                                                set @days-=1;
			                                                else
				                                                set @days+=1;
		                                                end
		                                                CLOSE _cursor;
		                                                DEALLOCATE _cursor;
	                                                end
                                                    if @days > 0
		                                                set @count+=1;
													if @days < 0
		                                                set @count-=1;
                                                end
                                                select dateadd(day, @count,@bascidate) as workdate", days,
                basicdate.ToShortDateString(),
                factoryid);

            return DateTime.Parse(MyUtility.GetValue.Lookup(sqlcmd, null));
        }
        #endregion

        #region GetStdQ

        /// <inheritdoc/>
        public static int GetStdQ(string orderid)
        {
            string sqlcmd = string.Format(
                @"
WITH cte (DD,num, INLINE,OrderID,sewinglineid,FactoryID,WorkDay,StandardOutput,ComboType,Hours,WDAY) AS (  
      SELECT DATEDIFF(DAY,A.Inline,A.Offline)+1 AS DD
                    , 1 as num
                    , convert(date,A.Inline) inline 
                    ,A.OrderID
                    ,A.sewinglineid
                    ,a.FactoryID
					,a.WorkDay,
					a.StandardOutput
                    ,a.ComboType,W.Hours
					,IIF(W.Hours > 0,1,0) AS WDAY
	  FROM SewingSchedule A WITH (NOLOCK)
	  LEFT JOIN WorkHour W ON A.FactoryID=W.FactoryID AND A.SewingLineID=W.SewingLineID
	  WHERE ORDERID='{0}' and w.Date between convert(date,A.Inline) and convert(date,A.Offline)
      UNION ALL  
      SELECT DD,num + 1, DATEADD(DAY,1,INLINE) ,ORDERID,sewinglineid,FactoryID,WorkDay,StandardOutput,ComboType,Hours,WDAY
      FROM cte a where num < DD  AND ORDERID='{0}'
    )  
	select SUM(Hours)h,sum(WDAY)wday,WorkDay,StandardOutput
	into #std
	from cte
	group by WorkDay,StandardOutput
	select iif(WorkDay=0,0,avgh.avghours * StandardOutput) stdq
	from #std
	outer apply(select avghours=iif(wday=0,0,h/wday) from #std)avgh
	drop table #std
", orderid);

            DBProxy.Current.Select(null, sqlcmd, out DataTable dt);

            if (dt == null || dt.Rows.Count == 0 || dt.Rows[0].Table == null || dt.Rows[0].Table.Rows.Count == 0 || dt.Rows[0][0].Empty())
            {
                return 0;
            }
            else
            {
                return int.Parse(Math.Ceiling(decimal.Parse(MyUtility.GetValue.Lookup(sqlcmd, null))).ToString());
            }
        }
        #endregion
        #region

        /// <summary>
        /// 最終算出每張[By SP]或[By SP,Article,Size]目前可完成的成衣件數
        /// 此function目的Planning R15效能.若無大量資料需求, 請使用procedure的QtyBySetPerSubprocess
        /// SubprocessID 外加工段
        /// InStartDate 篩選裁片收進的起始日
        /// InEndDate 篩選裁片收進的結束日
        /// OutStartDate 篩選裁片完成加工段的起始日
        /// OutEndDate 篩選裁片完成加工段的結束日
        /// </summary>
        /// <param name="subprocessIDs">字串陣列,需要計算的工段</param>
        /// <param name="tempTable">傳入需有OrderID欄位</param>
        /// <param name="bySP">是否要計算出bySP的Temp table</param>
        /// <param name="isNeedCombinBundleGroup">是否要依照 BundleGroup 算成衣件數 true/false</param>
        /// <param name="isMorethenOrderQty">回傳Qty值是否超過訂單數, (生產有可能超過) </param>
        /// <param name="rfidProcessLocationID">rfidProcessLocationID </param>
        /// <returns>回傳字串, 提供接下去的Sql指令使用#temp Table</returns>
        // 非常重要 更新此處一定要把此dll檔案更新到MES,API
        // 非常重要 更新此處一定要把此dll檔案更新到MES,API
        // 非常重要 更新此處一定要把此dll檔案更新到MES,API
        // 非常重要 更新此處一定要把此dll檔案更新到MES,API
        // 非常重要 更新此處一定要把此dll檔案更新到MES,API
        public static string QtyBySetPerSubprocess(
            string[] subprocessIDs,
            string tempTable = "#cte",
            bool bySP = false,
            bool isNeedCombinBundleGroup = false,
            string isMorethenOrderQty = "0",
            string rfidProcessLocationID = "")
        {
            // 若 WIP_ByShell ，從 Order_BOF 判斷必須是 Kind = 1
            string sqlcmd = $@"
-- 成套標準：
-- 找出組成一件成衣，需要哪些裁片
/*
ISP20241140
   準備基準資料,回 P02 WorkOrderForPlanning 找到所有部位 PatternPanel, FabricPanelCode
   EX: WorkOrderForPlanning, 有FA, FB, FC, FD,FE , 但 WorkOrderForOutput 只有 FA

ISP20241140
   原本有一段相同 OrderID, Article, Size, FabricCode, Color 只取第一個 PatternPanel 為基準(以 FabricPanelCode 排序)
   Cutting P02/P09 操作規則, WorkOrder_PatternPanel 以 FabricPanelCode 排序取第一筆
   WorkOrder.FabricCombo = WorkOrder_PatternPanel.PatternPanel
   WorkOrder.FabricPanelCode = WorkOrder_PatternPanel.FabricPanelCode
   WorkOrder.FabricCode = WorkOrder_PatternPanel.FabricCode
   故可直接使用 WorkOrder 這3個欄位, 不用去 WorkOrder_PatternPanel 展開又剃除浪費效能

Other
   過去曾經以 Bundle 資訊為基準
   是因為 P10 可以再增加 Bundle 資訊,就出現基準比WorkOrder多狀況
   但是 P10 Bundle 建立後也可以回 P02 WorkOrder 增加資訊, 又會出現基準變動
   總之 ISP20241140 調整以 Cutting P02 WorkOrderForPlanning 為基準
*/
SELECT DISTINCT
    wd.OrderID
   ,wd.Article
   ,wd.SizeCode
   ,POID = wop.ID
   ,PatternPanel = wop.FabricCombo
   ,wop.FabricPanelCode
INTO #beforeAllO
FROM WorkOrderForOutput_Distribute wd WITH (NOLOCK) -- OrderID 只能找 WorkOrderForOutput_Distribute, ForPlanning 沒有 Distribute
INNER JOIN WorkOrderForOutput woo WITH (NOLOCK) ON woo.Ukey = wd.WorkOrderForOutputUkey
/*
   問題
   1. WorkOrderForOutput.WorkOrderForPlanningUkey = WorkOrderForPlanning.Ukey 但用 P09 Excel匯入/手動新增 就沒有WorkOrderForPlanningUkey
   2. 用 P09.ID = P02.ID <<可能>> [OrderID, Article, SizeCode] 有不同的 FabricCombo, FabricPanelCode
   但 2024/11/07 討論後同一個 POID 底下的 OrderID 部位都會一樣, 故以 ID 串到 P02 直接展開, 往後若有數字疑慮可先找這點
*/
INNER JOIN WorkOrderForPlanning wop WITH (NOLOCK) ON wop.ID = woo.ID
WHERE EXISTS (SELECT 1 FROM Orders o WITH (NOLOCK) WHERE wd.OrderID = o.ID AND EXISTS (SELECT 1 FROM {tempTable} t WHERE t.orderid = o.id AND o.LocalOrder = 0)) --非local單 
AND NOT EXISTS (SELECT 1 FROM Cutting_WIPExcludePatternPanel cw WITH (NOLOCK) WHERE cw.ID = wop.ID AND cw.PatternPanel = wop.FabricCombo) -- ISP20201886 排除指定 PatternPanel
AND (
    ((SELECT WIP_ByShell FROM system) = 1 AND EXISTS (SELECT 1 FROM Order_BOF bof WITH (NOLOCK) WHERE bof.Id = wop.Id AND bof.FabricCode = wop.FabricCode AND bof.Kind = 1))
    OR
     (SELECT WIP_ByShell FROM system) = 0
)

-- Bundle Local 單
select	distinct
		bdo.Orderid
		,bun.POID
		,bun.Article
		,bd.Sizecode
		,bun.PatternPanel
		,bun.FabricPanelCode
into #BundleLocal
from Bundle_Detail_Order bdo WITH (NOLOCK)
inner join Bundle_Detail bd WITH (NOLOCK) on bd.BundleNo = bdo.BundleNo
inner join Bundle bun WITH (NOLOCK) on bun.id = bd.id
where exists (select 1 from Orders o WITH (NOLOCK) where bdo.Orderid = o.ID and  bun.MDivisionID = o.MDivisionID 
		and exists (select 1 from {tempTable} t where t.OrderID = o.ID and o.LocalOrder = 1)) --Local單

select distinct t.Orderid,t.POID,t.Article,t.SizeCode,t.PatternPanel,t.FabricPanelCode
into #AllOrders
from #beforeAllO t
union all
select * from #BundleLocal

--回找 Bundle 資料, 拆成3個#tmp再組合是因為效能需求
--先加總, 同 Orderid, BundleNo 會有多筆紀錄
SELECT Orderid, BundleNo, ID, Qty = SUM(Qty)
INTO #tmp_Bundle_Detail_Order
FROM Bundle_Detail_Order bdo
WHERE EXISTS (SELECT 1 FROM #AllOrders x0 WHERE bdo.Orderid = x0.Orderid)
GROUP BY Orderid, BundleNo, ID

select ID, BundleGroup, BundleNo, Sizecode, Patterncode, IsPair
into #tmp_Bundle_Detail 
from Bundle_Detail bund WITH (NOLOCK) 
where exists (select 1 from #tmp_Bundle_Detail_Order bdo where bund.BundleNo = bdo.BundleNo and bdo.ID = bund.Id)
and exists (select 1 from #AllOrders x0 where bunD.Sizecode = x0.Sizecode)

select ID, PatternPanel, FabricPanelCode, Article, AddDate
into #tmp_Bundle
from Bundle bun WITH (NOLOCK)
where exists (select 1 from #tmp_Bundle_Detail bund where bun.id = bund.id)
and exists (select 1 from #AllOrders x0 where bun.Article = x0.Article and bun.PatternPanel = x0.PatternPanel and bun.FabricPanelCode = x0.FabricPanelCode)

--上方 Bundle_Detail_Order 已先總和 Qty
--此處展開到 Bundle_Detail_Order.Orderid, Qty
SELECT DISTINCT
    bun.AddDate
   ,bun.PatternPanel
   ,bun.FabricPanelCode
   ,bun.Article
   ,bunD.ID
   ,bunD.BundleGroup
   ,bunD.BundleNo
   ,bunD.Sizecode
   ,bunD.Patterncode
   ,bunD.IsPair
   ,bdo.Orderid
   ,bdo.Qty
INTO #tmp_Bundle_QtyBySubprocess
FROM #tmp_Bundle_Detail_Order bdo WITH (NOLOCK)
INNER JOIN #tmp_Bundle_Detail bund WITH (NOLOCK) ON bund.BundleNo = bdo.BundleNo AND bdo.ID = bund.Id
INNER JOIN #tmp_Bundle bun WITH (NOLOCK) ON bun.id = bund.id
WHERE EXISTS (
    SELECT 1
    FROM #AllOrders x0
    WHERE bdo.Orderid = x0.Orderid
    AND bunD.Sizecode = x0.Sizecode
    AND bun.Article = x0.Article
    AND bun.PatternPanel = x0.PatternPanel
    AND bun.FabricPanelCode = x0.FabricPanelCode
)

drop table #tmp_Bundle, #tmp_Bundle_Detail, #tmp_Bundle_Detail_Order

select *
into #tmp_Bundle_Detail_Art
from Bundle_Detail_Art bda WITH (NOLOCK)
where exists (select 1  from #tmp_Bundle_QtyBySubprocess bunD where bda.BundleNo = bunD.BundleNo)

create nonclustered index idx_QtyBySubprocess on #tmp_Bundle_QtyBySubprocess (Orderid, PatternPanel,Article,Sizecode,FabricPanelCode)
create nonclustered index idx_SubprocessId on #tmp_Bundle_Detail_Art (Bundleno, SubprocessId)
";
            foreach (string subprocessID in subprocessIDs)
            {
                string subprocessIDtmp = SubprocesstmpNoSymbol(subprocessID);

                // --Step 2. --
                //-- * 2.找出所有 Fabric Combo +Fabric Panel Code +Article + SizeCode->Cartpart(包含同部位數量)
                //--使用資料表 Bundle_Detail
                // --條件 訂單號碼 + Fabric Combo + Fabric Panel Code +Article + SizeCode
                // --top 1 Bundle Group 當作基準計算每個部位數量
                // --數量分成以下 2 種
                // --a.QtyBySet
                // --  數量直接加總
                // --b.QtyBySubprocess
                // --  部位有須要用 X 外加工計算
                sqlcmd += $@"
/*******************   {subprocessIDtmp} start  ********************/
BEGIN
select distinct	st1.OrderID,
		st1.POID,
		st1.Article,
		st1.SizeCode,
		st1.PatternPanel,
		st1.FabricPanelCode,
		CutpartCount.PatternCode,
        CutpartCount.PatternDesc,
		CutpartCount.QtyBySet,
		CutpartCount.QtyBySubprocess
into #QtyBySetPerCutpart{subprocessIDtmp}
from #AllOrders st1
outer apply (
	select	bunD.Patterncode
            ,bunD.PatternDesc
			, QtyBySet = count (1)
			, QtyBySubprocess = sum (isnull (QtyBySubprocess.v, 0))
	from (
 		select top 1 ID = isnull(x1.ID ,x2.ID)
			,BundleGroup = isnull(x1.BundleGroup ,x2.BundleGroup)
		from (select st1.Orderid,st1.PatternPanel,st1.Article,st1.Sizecode,st1.FabricPanelCode)x0 
		outer apply (
			select top 1
					bunD.ID
					, bunD.BundleGroup
					, bunD.BundleNo
			from #tmp_Bundle_QtyBySubprocess bunD
			where bunD.Orderid = x0.Orderid
					and bunD.PatternPanel = x0.PatternPanel
					and bunD.Article = x0.Article
					and bunD.Sizecode = x0.Sizecode
				    and bunD.FabricPanelCode = x0.FabricPanelCode
					and exists (select 1
									from #tmp_Bundle_Detail_Art BunDArt WITH (NOLOCK)
									where BunDArt.Bundleno = bunD.BundleNo
										and BunDArt.SubprocessId = '{subprocessID}')
			order by bunD.AddDate desc
		)x1
		outer apply(
			select top 1
					bunD.ID
					, bunD.BundleGroup
					, bunD.BundleNo
			from #tmp_Bundle_QtyBySubprocess bunD 
			where bunD.Orderid = x0.Orderid
					and bunD.PatternPanel = x0.PatternPanel
					and bunD.Article = x0.Article
					and bunD.Sizecode = x0.Sizecode
				    and bunD.FabricPanelCode = x0.FabricPanelCode
			order by bunD.AddDate desc
		)x2
	) getGroupInfo
	inner join Bundle_Detail bunD WITH (NOLOCK) on getGroupInfo.Id = bunD.Id and getGroupInfo.BundleGroup = bunD.BundleGroup
	outer apply (
		select v = (select 1
					where exists (select 1								  
									from #tmp_Bundle_Detail_Art BunDArt WITH (NOLOCK) 
									where BunDArt.Bundleno = bunD.BundleNo
										and BunDArt.SubprocessId = '{subprocessID}'))
	) QtyBySubprocess
	group by bunD.Patterncode,bunD.PatternDesc
) CutpartCount

----整理出標準：根據EachCons需要有哪些PatternPanel+FabricPanelCode組合，以及這些組合底下共有幾個部位(Patterncode)
----總共要檢查：1. Bundle的PatternPanel+FabricPanelCode組合，是否跟EachCons的一樣
----           2. Bundle每個組合底下的部位數量，是否都 >= 上面那一段取出來TOP 1 的部位數量
----有一個為否，則不能算做成套
SELECT  a.OrderID,a.Article,a.SizeCode,a.PatternPanel ,a.FabricPanelCode ,[Ctn]=COUNT(DISTINCT Patterncode)
INTO #{subprocessIDtmp}_StdCount
FROM #AllOrders a
LEFT JOIN #QtyBySetPerCutpart{subprocessIDtmp} b ON a.OrderID=b.OrderID AND a.Article=b.Article
AND a.SizeCode=b.SizeCode
AND a.PatternPanel=b.PatternPanel
AND a.FabricPanelCode=b.FabricPanelCode
GROUP BY a.OrderID,a.Article,a.SizeCode,a.PatternPanel ,a.FabricPanelCode

-- Step 3. --加總每個訂單各 Fabric Combo 所有捆包的『數量』
select	st2.Orderid
		, st2.Article
		, st2.Sizecode
		, st2.PatternPanel
		, QtyBySet = sum (st2.QtyBySet)
		, QtyBySubprocess = sum (st2.QtyBySubProcess)
into #CutpartBySet_PatternPanel{subprocessIDtmp}
from #QtyBySetPerCutpart{subprocessIDtmp} st2
group by st2.Orderid, st2.Article, st2.Sizecode, st2.PatternPanel

select	st2.Orderid
		, st2.Article
		, st2.Sizecode
		, QtyBySet = sum (st2.QtyBySet)
		, QtyBySubprocess = sum (st2.QtyBySubProcess)
into #CutpartBySet{subprocessIDtmp}
from #QtyBySetPerCutpart{subprocessIDtmp} st2
group by st2.Orderid, st2.Article, st2.Sizecode

--2020/3/18↓效能調整,移除join Order_SizeCode,現在上方準備資料階段SizeCode已從Bundle來源改成Bundle_Detail和Order_Qty, 不需要再去串Order_SizeCode確認此SizeCode是否存在
--2020/09/11效能調整, 因部分subprocessID跑很慢, outer apply欄位拆分計算 
select
	st0.*
	, SubprocessId=sub.id
	, sub.InOutRule
	, sub.IsRFIDDefault
	, IsLackPatternPanel = IsLackPatternPanel.Ctn
	, StdCtn = isnull(Std.Ctn,0)
into #QtyBySetPerCutpart2{subprocessIDtmp}
from #QtyBySetPerCutpart{subprocessIDtmp} st0
inner join SubProcess sub WITH (NOLOCK) on sub.ID = '{subprocessID}'
OUTER APPLY(----裁剪組合缺少的數量
	SELECT [Ctn]=COUNT(t.FabricPanelCode)
	FROM #{subprocessIDtmp}_StdCount t
	WHERE NOT EXISTS(
		SELECT 1
		FROM #QtyBySetPerCutpart{subprocessIDtmp} q 
		WHERE q.OrderID=t.OrderID AND q.Article=t.Article AND q.SizeCode=t.SizeCode AND q.PatternPanel=t.PatternPanel
	)AND 
	t.OrderID=st0.OrderID AND t.Article=st0.Article AND t.SizeCode=st0.SizeCode
)IsLackPatternPanel
OUTER APPLY(
    ----標準的部位數
	SELECT Ctn
	FROM #{subprocessIDtmp}_StdCount
	WHERE Orderid = st0.Orderid AND Article = st0.Article AND SizeCode = st0.SizeCode
		AND PatternPanel = st0.PatternPanel  AND FabricPanelCode = st0.FabricPanelCode
)Std

select    st0.Orderid
        , bund.BundleNo
		, st0.SubprocessId
		, st0.InOutRule
		, bunD.BundleGroup
		, Size=st0.SizeCode
		, st0.Article
        , st0.SizeCode
		, st0.PatternPanel
		, bunD.FabricPanelCode
		, st0.PatternCode
        , st0.PatternDesc
		, bunIO.InComing
		, bunIO.OutGoing
		, m=iif (st0.IsRFIDDefault = 1, st0.QtyBySet, st0.QtyBySubprocess)

        --ISP20210230 確認後不用特別指定 Loading,Sewingline
        --其它Subprocess若是Main(主裁片)，其Bundleno之下不會有NoBundleCardAfterSubprocess
		, NoBundleCardAfterSubprocess = isnull(x.NoBundleCardAfterSubprocess,0)
		, PostSewingSubProcess_SL =iif(isnull(Bundle_Art.PostSewingSubProcess,0) = 1 and bunIOS.OutGoing is not null and bunIOL.InComing is not null, 1, 0)
		, bunDQty = bunD.Qty
		, st0.IsLackPatternPanel
		, bundID= bund.ID
		, st0.StdCtn
into #QtyBySetPerCutpart3{subprocessIDtmp}
from #QtyBySetPerCutpart2{subprocessIDtmp} st0
left join #tmp_Bundle_QtyBySubprocess bund on bunD.Orderid = st0.Orderid 
							and bunD.PatternPanel = st0.PatternPanel 
							and	bunD.FabricPanelCode = st0.FabricPanelCode 
							and	bunD.Article = st0.Article  
							and	bunD.Patterncode = st0.Patterncode 
							and	bunD.Sizecode = st0.SizeCode
left join BundleInOut bunIO with (nolock)  on bunIO.BundleNo = bunD.BundleNo and bunIO.SubProcessId = st0.SubprocessId {(rfidProcessLocationID.ToUpper() == "ALL" ? string.Empty : $"and bunIO.RFIDProcessLocationID = '{rfidProcessLocationID}'")} 
left join BundleInOut bunIOS with (nolock) on bunIOS.BundleNo = bunD.BundleNo and bunIOS.SubProcessId = 'SORTING' and bunIOS.RFIDProcessLocationID = ''
left join BundleInOut bunIOL with (nolock) on bunIOL.BundleNo = bunD.BundleNo and bunIOL.SubProcessId = 'LOADING' and bunIOL.RFIDProcessLocationID = ''
OUTER APPLY(  --BundleNo + SubProcessId 可能多筆，故這樣處理
	SELECT DISTINCT PostSewingSubProcess 
    FROM #tmp_Bundle_Detail_Art bda WITH (NOLOCK) 
    WHERE bda.BundleNo = bunD.BundleNo and bda.SubProcessId = st0.SubprocessId
)Bundle_Art
outer apply(
	select NoBundleCardAfterSubprocess=MAX(cast(NoBundleCardAfterSubprocess as int))
	from #tmp_Bundle_Detail_Art bda WITH (NOLOCK) 
	where bda.BundleNo = bunD.BundleNo
)x
OUTER APPLY(  --取得這個bundle no的指定加工段
	SELECT  DISTINCT SubProcessId 
    FROM #tmp_Bundle_Detail_Art bda WITH (NOLOCK)
    WHERE bda.BundleNo = bunD.BundleNo and bda.SubProcessId = st0.SubprocessId
)BundleArt
 where (st0.IsRFIDDefault = 1 or st0.QtyBySubprocess != 0)
 {(subprocessIDtmp.ToUpper() != "SORTING" && subprocessIDtmp.ToUpper() != "LOADING" && subprocessIDtmp.ToUpper() != "SEWINGLINE" ? $"and BundleArt.SubprocessId='{subprocessID}'" : string.Empty)}

select
	st3.*
	, [Qty] = IIF( RealCont.Ctn < st3.StdCtn OR st3.IsLackPatternPanel > 0,0 ,st3.bunDQty)  ----如果部位數有少，直接歸零不算；如果超過沒關係
into #QtyBySetPerCutpart4{subprocessIDtmp}
from #QtyBySetPerCutpart3{subprocessIDtmp} st3
OUTER APPLY(
    ----PMS系統內，實際建立的Bundle 部位數
	SELECT [Ctn]=COUNT(*)
	from (
		SELECT Patterncode
		FROM #tmp_Bundle_QtyBySubprocess
		WHERE ID = st3.bundID AND BundleGroup = st3.BundleGroup AND Orderid = st3.Orderid AND Article = st3.Article 
		AND SizeCode = st3.SizeCode AND PatternPanel = st3.PatternPanel   AND FabricPanelCode = st3.FabricPanelCode
		GROUP BY Patterncode
	)a
)RealCont

select bdo.Orderid,bdo.BundleNo,bunD.Patterncode,bunD.Sizecode,bunD.IsPair,bun.PatternPanel,bun.Article, bun.AddDate
into #QtyBySetPerCutpart5{subprocessIDtmp}
from(select distinct st4.Orderid from #QtyBySetPerCutpart4{subprocessIDtmp} st4) x
inner join Orders o WITH (NOLOCK) ON x.Orderid = o.id
inner join Bundle_Detail_Order bdo on bdo.Orderid = x.Orderid
inner join Bundle_Detail bunD WITH (NOLOCK) on bunD.BundleNo = bdo.BundleNo
inner join Bundle bun WITH (NOLOCK) on bun.id = bunD.id and bun.MDivisionid=o.MDivisionID 

--#BundleInOutDetail... 在 WebApi 有使用到, 變更時注意
select st4.*, [IsPair]=ISNULL(TopIsPair.IsPair,0)
into #BundleInOutDetail{subprocessIDtmp}
from #QtyBySetPerCutpart4{subprocessIDtmp} st4
outer apply(
	select	top 1 IsPair
	from #QtyBySetPerCutpart5{subprocessIDtmp} st5
	where st4.Orderid =st5.Orderid		
	and st4.PatternPanel=st5.PatternPanel
	and st4.Article =st5.Article		
	and st4.Patterncode	=st5.Patterncode	
	and st4.SizeCode =st5.SizeCode	
	order by st5.AddDate desc
)TopIsPair

select	Orderid
		, SubprocessId
		, BundleGroup
		, Size
		, Article
		, PatternPanel
		, FabricPanelCode
		, PatternCode
		, InQty = FLOOR(sum( Case when PostSewingSubProcess_SL = 1 Then Qty
							when NoBundleCardAfterSubprocess=1 and(InOutRule = 1 or InOutRule = 4) Then Qty--不判斷InComing,直接計算數量
					   else iif(I_Judge.v = 1, Qty, 0)
					   End
					 ) / IsPair.M)

		, OutQty = FLOOR(sum( Case when PostSewingSubProcess_SL = 1 Then Qty
							 when NoBundleCardAfterSubprocess=1 and InOutRule = 3 Then Qty--不判斷OutGoing,直接計算數量
						else iif(O_Judge.v = 1, Qty, 0)
						End 
					  ) / IsPair.M)

		, OriInQty = sum( Case when PostSewingSubProcess_SL = 1 Then Qty
							   when NoBundleCardAfterSubprocess=1 and(InOutRule = 1 or InOutRule = 4) Then Qty--不判斷InComing,直接計算數量
						  else iif(I_Judge.v = 1, Qty, 0)
						  End
					 ) --原始裁片數總和

		, OriOutQty = sum( Case when PostSewingSubProcess_SL = 1 Then Qty
							    when NoBundleCardAfterSubprocess=1 and InOutRule = 3 Then Qty--不判斷OutGoing,直接計算數量
						   else iif(O_Judge.v = 1, Qty, 0)
						   End 
					  ) --原始裁片數總和

		, FinishedQty = FLOOR(sum(case when PostSewingSubProcess_SL = 1 Then Qty
								 when NoBundleCardAfterSubprocess = 1 and InOutRule = 1 Then Qty-- 不判斷InComing
								 when InOutRule = 1 then iif(I_Judge.v = 1,Qty,0)
								 when InOutRule = 2 then iif(O_Judge.v = 1,Qty,0)
								 when NoBundleCardAfterSubprocess = 1 and InOutRule = 3 Then iif(I_Judge.v = 1,Qty,0)-- 忽略OutGoing, 只判斷InComing									
								 when NoBundleCardAfterSubprocess = 1 and InOutRule = 4 Then iif(O_Judge.v = 1,Qty,0)-- 忽略InComing, 只判斷OutGoing
								 else iif(O_Judge.v = 1 and I_Judge.v = 1 ,Qty,0)
							end) 
						/ IsPair.M)
        , InStartDate,InEndDate,OutStartDate,OutEndDate
into #beforeBundleInOutDetail{subprocessIDtmp}
from #BundleInOutDetail{subprocessIDtmp} bt
outer  apply(
    select distinct InStartDate,InEndDate,OutStartDate,OutEndDate
    from {tempTable} t where t.OrderID = bt.Orderid
)x
outer apply(select v = iif(InComing is not null and (x.InStartDate is null or x.InStartDate <= InComing) and (x.InEndDate is null or InComing <= x.InEndDate),1,0))I_Judge
outer apply(select v = iif(OutGoing is not null and (x.OutStartDate is null or x.OutStartDate <= OutGoing) and (x.OutEndDate is null or OutGoing <= x.OutEndDate),1,0))O_Judge
outer apply(select M = iif(IsPair=1,2,1) )IsPair--此處判斷後才放入group by 欄位中 
group by OrderID, SubprocessId, InOutRule, BundleGroup, Size, PatternPanel, FabricPanelCode, Article, PatternCode,IsPair.m
    , InStartDate,InEndDate,OutStartDate,OutEndDate

--#BundleInOutQty... 在 WebApi 有使用到, 變更時注意
select *
into #BundleInOutQty{subprocessIDtmp}
from #beforeBundleInOutDetail{subprocessIDtmp} t
where FabricPanelCode is not null 
or (FabricPanelCode is null and not exists(select 1 from #beforeBundleInOutDetail{subprocessIDtmp} b where b.Orderid = t.Orderid and b.Size = t.Size and b.Article= t.Article and b.PatternPanel = t. PatternPanel and b.FabricPanelCode is not null))
";

                if (isNeedCombinBundleGroup)
                {
                    sqlcmd += $@"
--篩選 BundleGroup Step.1 --

select	OrderID
		, Size
		, Article
		, PatternPanel
		, InQty = min (InQty)
		, OutQty = min (OutQty)
		, FinishedQty = min (FinishedQty)
        , InStartDate,InEndDate,OutStartDate,OutEndDate
into #FinalQtyBySet_PatternPanel{subprocessIDtmp}
from (
	select	OrderID
			, Size
			, Article
			, PatternPanel
			, FabricPanelCode
			, InQty = sum (InQty)
			, OutQty = sum (OutQty)
			, FinishedQty = sum (FinishedQty)
            , InStartDate,InEndDate,OutStartDate,OutEndDate
	from (
		select	OrderID
				, Size
				, Article
				, PatternPanel
				, FabricPanelCode
				, BundleGroup
				, InQty = min (InQty)
				, OutQty = min (OutQty)
				, FinishedQty = min (FinishedQty)
                , InStartDate,InEndDate,OutStartDate,OutEndDate
		from #BundleInOutQty{subprocessIDtmp}
		group by OrderID, Size, Article, PatternPanel, FabricPanelCode, BundleGroup, InStartDate,InEndDate,OutStartDate,OutEndDate
	) minGroupCutpart							
	group by OrderID, Size, Article, PatternPanel, FabricPanelCode, InStartDate,InEndDate,OutStartDate,OutEndDate
) sumGroup
group by OrderID, Size, Article, PatternPanel, InStartDate,InEndDate,OutStartDate,OutEndDate

select	OrderID
		, Article
		, Size
		, InQty = min (InQty)
		, OutQty = min (OutQty)
		, FinishedQty = min (FinishedQty)
        , InStartDate,InEndDate,OutStartDate,OutEndDate
into #FinalQtyBySet{subprocessIDtmp}
from #FinalQtyBySet_PatternPanel{subprocessIDtmp} minFabricPanelCode
group by OrderID, Size, Article, InStartDate,InEndDate,OutStartDate,OutEndDate
";
                }
                else
                {
                    sqlcmd += $@"
select	OrderID
		, Size
		, Article
		, PatternPanel
		, InQty = min (InQty)
		, OutQty = min (OutQty)
		, FinishedQty = min (FinishedQty)
        , InStartDate,InEndDate,OutStartDate,OutEndDate
into #FinalQtyBySet_PatternPanel{subprocessIDtmp}
from (
	select	OrderID
			, Size
			, Article
			, PatternPanel
			, FabricPanelCode
			, InQty = min (InQty)
			, OutQty = min (OutQty)
			, FinishedQty = min (FinishedQty)
            , InStartDate,InEndDate,OutStartDate,OutEndDate
	from (
		select	OrderID
				, Size
				, PatternPanel
				, FabricPanelCode
				, Article
				, PatternCode
				, InQty = sum (InQty)
				, OutQty = sum (OutQty)
				, FinishedQty = sum (FinishedQty)
                , InStartDate,InEndDate,OutStartDate,OutEndDate
		from #BundleInOutQty{subprocessIDtmp}
		group by OrderID, Size, Article, PatternPanel, FabricPanelCode, PatternCode, InStartDate,InEndDate,OutStartDate,OutEndDate
	) sumbas
	group by OrderID, Size, Article, PatternPanel, FabricPanelCode, InStartDate,InEndDate,OutStartDate,OutEndDate
) minCutpart
group by OrderID, Size, Article, PatternPanel, InStartDate,InEndDate,OutStartDate,OutEndDate

select	OrderID
		, Article
		, Size
		, InQty = min (InQty)
		, OutQty = min (OutQty)
		, FinishedQty = min (FinishedQty)
        , InStartDate,InEndDate,OutStartDate,OutEndDate
into #FinalQtyBySet{subprocessIDtmp}
from #FinalQtyBySet_PatternPanel{subprocessIDtmp} minFabricPanelCode
group by OrderID, Size, Article, InStartDate,InEndDate,OutStartDate,OutEndDate
";
                }

                sqlcmd += $@"
-- Result Data --
--	 *	3.	最終算出每張訂單目前可完成的成衣件數
select	OrderID = cbs.OrderID
		, cbs.Article
		, cbs.Sizecode
		, cbs.PatternPanel
		, QtyBySet = cbs.QtyBySet
		, QtyBySubprocess = cbs.QtyBySubprocess
		, InQtyBySet = case when {isMorethenOrderQty} = 1 then sub.InQty
						when sub.InQty>oq.qty then oq.qty
						else sub.InQty
						end
		, OutQtyBySet = case when {isMorethenOrderQty} = 1 then sub.OutQty
						when sub.OutQty>oq.qty then oq.qty
						else sub.OutQty
						end
		, InQtyByPcs
		, OutQtyByPcs
		, FinishedQtyBySet = case when {isMorethenOrderQty} = 1  then sub.FinishedQty
						when sub.FinishedQty>oq.qty then oq.qty
						else sub.FinishedQty
						end
        , InStartDate,InEndDate,OutStartDate,OutEndDate
into #QtyBySetPerSubprocess_PatternPanel{subprocessIDtmp}
from #CutpartBySet_PatternPanel{subprocessIDtmp} cbs
inner join Order_Qty oq  WITH (NOLOCK) on oq.id = cbs.OrderID and oq.SizeCode = cbs.SizeCode and oq.Article = cbs.Article   --ISP20191573 SizeCode和Article 的資料都必須從Trade來，不是Bundle有的全部都用
left join #FinalQtyBySet_PatternPanel{subprocessIDtmp} sub on cbs.Orderid = sub.Orderid and cbs.Sizecode = sub.size and cbs.Article = sub.Article and sub.PatternPanel = cbs.PatternPanel--到這層會因為 InStartDate,InEndDate,OutStartDate,OutEndDate展開
outer apply (
	select	InQtyByPcs = sum (isnull (bunIO.OriInQty, 0))
			, OutQtyByPcs = sum (isnull (bunIO.OriOutQty, 0))
	from #BundleInOutQty{subprocessIDtmp} bunIO
	where cbs.OrderID = bunIO.OrderID and cbs.Sizecode = bunIO.Size and cbs.Article = bunIO.Article and cbs.PatternPanel = bunIO.PatternPanel
    and bunIO.InStartDate = sub.InStartDate and bunIO.InEndDate = sub.InEndDate and bunIO.OutStartDate = sub.OutStartDate and bunIO.OutEndDate = sub.OutEndDate
) IOQtyPerPcs
where FinishedQty is not null

-- Result Data --
--	 *	3.	最終算出每張訂單目前可完成的成衣件數
--QtyBySetPerSubprocess... 在 WebApi 有使用到, 變更時注意
select	OrderID = cbs.OrderID
		, cbs.Article
		, cbs.Sizecode
		, QtyBySet = cbs.QtyBySet
		, QtyBySubprocess = cbs.QtyBySubprocess
		, InQtyBySet = case when {isMorethenOrderQty} = 1 then sub.InQty
						when sub.InQty>oq.qty then oq.qty
						else sub.InQty
						end
		, OutQtyBySet = case when {isMorethenOrderQty} = 1 then sub.OutQty
						when sub.OutQty>oq.qty then oq.qty
						else sub.OutQty
						end
		, InQtyByPcs
		, OutQtyByPcs
		, FinishedQtyBySet = case when {isMorethenOrderQty} = 1  then sub.FinishedQty
						when sub.FinishedQty>oq.qty then oq.qty
						else sub.FinishedQty
						end
        , InStartDate,InEndDate,OutStartDate,OutEndDate
into #QtyBySetPerSubprocess{subprocessIDtmp}
from #CutpartBySet{subprocessIDtmp} cbs
inner join Order_Qty oq  WITH (NOLOCK) on oq.id = cbs.OrderID and oq.SizeCode = cbs.SizeCode and oq.Article = cbs.Article   --ISP20191573 SizeCode和Article 的資料都必須從Trade來，不是Bundle有的全部都用
left join #FinalQtyBySet{subprocessIDtmp} sub on cbs.Orderid = sub.Orderid and cbs.Sizecode = sub.size and cbs.Article = sub.Article--到這層會因為 InStartDate,InEndDate,OutStartDate,OutEndDate展開
outer apply (
	select	InQtyByPcs = sum (isnull (bunIO.OriInQty, 0))
			, OutQtyByPcs = sum (isnull (bunIO.OriOutQty, 0))
	from #BundleInOutQty{subprocessIDtmp} bunIO
	where cbs.OrderID = bunIO.OrderID and cbs.Sizecode = bunIO.Size and cbs.Article = bunIO.Article
    and bunIO.InStartDate = sub.InStartDate and bunIO.InEndDate = sub.InEndDate and bunIO.OutStartDate = sub.OutStartDate and bunIO.OutEndDate = sub.OutEndDate
) IOQtyPerPcs
where FinishedQty is not null

drop table #QtyBySetPerCutpart{subprocessIDtmp},#QtyBySetPerCutpart2{subprocessIDtmp},#QtyBySetPerCutpart3{subprocessIDtmp}
, #QtyBySetPerCutpart4{subprocessIDtmp}, #QtyBySetPerCutpart5{subprocessIDtmp}
, #CutpartBySet{subprocessIDtmp}, #FinalQtyBySet{subprocessIDtmp}


";
                if (bySP)
                {
                    sqlcmd += $@"
select OrderID, InQtyBySet = sum (InQty), OutQtyBySet = sum (OutQty),FinishedQtyBySet=sum(FinishedQtyBySet), InStartDate,InEndDate,OutStartDate,OutEndDate
into #{subprocessIDtmp}
from(
	select OrderID, SizeCode, InQty = min (InQtyBySet), OutQty = min (OutQtyBySet),FinishedQtyBySet=sum(FinishedQtyBySet)
        , InStartDate,InEndDate,OutStartDate,OutEndDate
	from #QtyBySetPerSubprocess{subprocessIDtmp}	minPatternPanel
	group by OrderID, SizeCode ,Article, InStartDate,InEndDate,OutStartDate,OutEndDate
) minArticle
group by OrderID, InStartDate,InEndDate,OutStartDate,OutEndDate
";
                }

                sqlcmd += Environment.NewLine + $@"
END
/*******************   {subprocessIDtmp} END  ********************/";
            }

            sqlcmd += Environment.NewLine + " drop table #AllOrders, #tmp_Bundle_QtyBySubprocess " + Environment.NewLine;
            return sqlcmd;
        }

        /// <summary>
        /// EX: 把PAD-PRT為PADPRT, 命名#table名稱用
        /// </summary>
        /// <param name="subprocessID">subprocessID</param>
        /// <returns>Subprocess tmpTableName</returns>
        public static string SubprocesstmpNoSymbol(string subprocessID)
        {
            return subprocessID.Replace("-", string.Empty); // 把PAD-PRT為PADPRT, 命名#table名稱用
        }
        #endregion
    }
}

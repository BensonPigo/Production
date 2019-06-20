using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sci.Data;
using Sci;
using Ict;
using Ict.Win;

namespace Sci.Production.PublicPrg
{

    public static partial class Prgs
    {
        #region GetWorkDate
        /// <summary>
        /// GetWorkDate()
        /// </summary>
        /// <param name="String factoryid"></param>
        /// <param name="int days"></param>
        /// <param name="DateTime basicdate"></param>
        /// <returns>datetime workdate</returns>
        public static DateTime GetWorkDate(string factoryid, int days, DateTime basicdate)
        {
            string sqlcmd = string.Format(@"declare @days as int  = {0} ,@count as int = 0, @bascidate as date = '{1}';
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
                                                select dateadd(day, @count,@bascidate) as workdate", days, basicdate.ToShortDateString(), factoryid);

            return DateTime.Parse((MyUtility.GetValue.Lookup(sqlcmd, null)));
        

        }
        #endregion
        #region GetStdQ
        /// <summary>
        /// GetStdQ()
        /// </summary>
        /// <param name="String OrderID"></param>
        /// <returns>Int StdQ</returns>
        public static int GetStdQ(string orderid)
        {
            string sqlcmd = string.Format(@"
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
            //原SQL
            //WITH cte (DD,num, INLINE,OrderID,sewinglineid,FactoryID,stdq,ComboType) AS (  
            //      SELECT DATEDIFF(DAY,A.Inline,A.Offline)+1 AS DD
            //                    , 1 as num
            //                    , convert(date,A.Inline) inline 
            //                    ,A.OrderID
            //                    ,sewinglineid
            //                    ,a.FactoryID
            //                    ,iif(a.WorkDay=0,(a.WorkHour / 1 * a.StandardOutput),(a.WorkHour / a.WorkDay * a.StandardOutput)) stdq
            //                    ,a.ComboType
            //	  FROM SewingSchedule A WITH (NOLOCK) WHERE ORDERID='{0}'
            //      UNION ALL  
            //      SELECT DD,num + 1, DATEADD(DAY,1,INLINE) ,ORDERID,sewinglineid,FactoryID,stdq,ComboType
            //	  FROM cte a where num < DD  AND ORDERID='{0}'
            //    )  
            //	select min(stdq) stdq
            //	from (
            //	 SELECT a.orderid,a.sewinglineid,a.ComboType,a.INLINE,sum(a.stdq) stdq, isnull(b.hours,0) workhours
            //	 FROM cte a left join WorkHour b WITH (NOLOCK) on convert(date,a.inline) = b.date and a.sewinglineid = b.SewingLineID and a.FactoryID=b.FactoryID 
            //	 group by a.orderid,a.sewinglineid,a.ComboType,a.INLINE,b.Hours
            //	 having isnull(b.hours,0) > 0) tmp
            DataTable dt;
            DBProxy.Current.Select(null,sqlcmd,out dt);
            //return int.Parse(dt.Rows[0][0].ToString());
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
        /// </summary>
        /// <param name="subprocessIDs">字串陣列,需要計算的工段</param>
        /// <param name="tempTable">傳入需有OrderID欄位</param>
        /// <param name="bySP">是否要計算出bySP的Temp table</param>
        /// <param name="isNeedCombinBundleGroup">是否要依照 BundleGroup 算成衣件數 true/false</param>
        /// <param name="isMorethenOrderQty">回傳Qty值是否超過訂單數, (生產有可能超過) </param>
        /// <returns>回傳字串, 提供接下去的Sql指令使用#temp Table</returns>
        public static string QtyBySetPerSubprocess(
            string[] subprocessIDs,
            string tempTable = "#cte",
            bool bySP = false,
            bool isNeedCombinBundleGroup = false,
            string isMorethenOrderQty = "0")
        {
            string sqlcmd = $@"
-- 1.	尋找指定訂單 Fabric Combo + Fabric Panel Code
-- 使用資料表 Bundle 去除重複即可得到每張訂單 Fabric Combo + Fabric Panel Code + Article + SizeCode
select	distinct
		bun.Orderid
		, bun.POID
		, bun.PatternPanel
		, bun.FabricPanelCode
		, bun.Article
		, bun.Sizecode
into #AllOrders
from Bundle bun WITH (NOLOCK) 
inner join Orders os  WITH (NOLOCK) on bun.Orderid = os.ID and bun.MDivisionID = os.MDivisionID
inner join {tempTable} t on t.OrderID = bun.Orderid
";

            foreach (string subprocessID in subprocessIDs)
            {
                string subprocessIDt = subprocessID.Replace("-", string.Empty); // 把PAD-PRT為PADPRT, 命名#table名稱用
                string isSpectialReader = string.Empty;
                if (subprocessID.ToLower().EqualString("sorting") || subprocessID.ToLower().EqualString("loading"))
                {
                    isSpectialReader = "1";
                }
                else
                {
                    isSpectialReader = "0";
                }

                // --Step 2. --
                //-- * 2.找出所有 Fabric Combo +Fabric Pancel Code +Article + SizeCode->Cartpart(包含同部位數量)
                //--使用資料表 Bundle_Detail
                // --條件 訂單號碼 + Fabric Combo + Fabric Panel Code +Article + SizeCode
                // --top 1 Bundle Group 當作基準計算每個部位數量
                // --數量分成以下 2 種
                // --a.QtyBySet
                // --  數量直接加總
                // --b.QtyBySubprocess
                // --  部位有須要用 X 外加工計算
                sqlcmd += $@"
select	*
		, num = count (1) over (partition by OrderID, PatternPanel, FabricPanelCode, Article, Sizecode)
into #QtyBySetPerCutpart{subprocessIDt}
from #AllOrders st1
outer apply (
	select	bunD.Patterncode
			, QtyBySet = count (1)
			, QtyBySubprocess = sum (isnull (QtyBySubprocess.v, 0))
	from (
		select	top 1
				bunD.ID
				, bunD.BundleGroup
		from Bundle_Detail bunD WITH (NOLOCK) 
		inner join Bundle bun  WITH (NOLOCK) on bunD.Id = bun.ID
		where bun.Orderid = st1.Orderid
				and bun.PatternPanel = st1.PatternPanel
				and bun.FabricPanelCode = st1.FabricPanelCode
				and bun.Article = st1.Article
				and bun.Sizecode = st1.Sizecode
	) getGroupInfo
	inner join Bundle_Detail bunD on getGroupInfo.Id = bunD.Id and getGroupInfo.BundleGroup = bunD.BundleGroup
	outer apply (
		select v = (select 1
					where exists (select 1								  
									from Bundle_Detail_Art BunDArt WITH (NOLOCK) 
									where BunDArt.Bundleno = bunD.BundleNo
										and BunDArt.SubprocessId = '{subprocessID}'))
	) QtyBySubprocess
	group by bunD.Patterncode
) CutpartCount

-- Step 3. --加總每個訂單各 Fabric Combo 所有捆包的『數量』
select	st2.Orderid
		, st2.Article
		, st2.Sizecode
		, QtyBySet = sum (st2.QtyBySet)
		, QtyBySubprocess = sum (st2.QtyBySubProcess)
into #CutpartBySet{subprocessIDt}
from #QtyBySetPerCutpart{subprocessIDt} st2
group by st2.Orderid, st2.Article, st2.Sizecode

-- Query by Set per Subprocess--
--1.	找出時間區間內指定訂單中裁片的進出資訊
select	st0.Orderid
		, st0.Article
		, st0.SizeCode
		, st0.PatternPanel
		, st0.FabricPanelCode
		, st0.Patterncode
		, InQty = case when {isSpectialReader} = 1 and st0.QtyBySet =0 then 0
					when {isSpectialReader} = 0 and st0.QtyBySubprocess = 0 then 0
					when {isSpectialReader} = 1 then FLOOR(sum(bunD.Qty) / st0.QtyBySet)
					when {isSpectialReader} = 0 then FLOOR(sum(bunD.Qty) / st0.QtyBySubprocess)
					end
		, OutQty = 0
		, bunD.BundleGroup
into #RFID{subprocessIDt}
from #QtyBySetPerCutpart{subprocessIDt} st0					
inner join Order_SizeCode os  WITH (NOLOCK) on st0.POID = os.Id and st0.Sizecode = os.SizeCode
inner join Bundle_Detail bunD  WITH (NOLOCK) on bunD.Patterncode = st0.Patterncode
inner join Bundle bun  WITH (NOLOCK) on bunD.Id = bun.ID and bun.Orderid = st0.Orderid
									and bun.PatternPanel = st0.PatternPanel
									and bun.FabricPanelCode = st0.FabricPanelCode
									and bun.Article = st0.Article
									and bun.Sizecode = st0.Sizecode
inner join BundleInOut bunIO  WITH (NOLOCK) on bunIO.BundleNo = bunD.BundleNo 
where ({isSpectialReader} = 1 or st0.QtyBySubprocess != 0) 
		and bunIO.SubProcessId = '{subprocessID}'
		and bunIO.InComing is not null
		and isnull(bunIO.RFIDProcessLocationID,'') = ''
group by st0.Orderid, st0.SizeCode, st0.PatternPanel, st0.FabricPanelCode, st0.Patterncode, st0.Article,bunD.BundleGroup, st0.QtyBySet, st0.QtyBySubprocess

union all
select	st0.Orderid
		, st0.Article
		, st0.SizeCode
		, st0.PatternPanel
		, st0.FabricPanelCode
		, st0.Patterncode
		, InQty = 0
		, OutQty = case when {isSpectialReader} = 1 and isnull(st0.QtyBySet,0) =0 then 0
					when {isSpectialReader} = 0 and isnull(st0.QtyBySubprocess,0) = 0 then 0
					when {isSpectialReader} = 1 then FLOOR(sum(bunD.Qty) / st0.QtyBySet)
					when {isSpectialReader} = 0 then FLOOR(sum(bunD.Qty) / st0.QtyBySubprocess)
					end
		, bunD.BundleGroup
from #QtyBySetPerCutpart{subprocessIDt} st0					
inner join Order_SizeCode os  WITH (NOLOCK) on st0.POID = os.Id and st0.Sizecode = os.SizeCode
inner join Bundle_Detail bunD  WITH (NOLOCK) on bunD.Patterncode = st0.Patterncode
inner join Bundle bun  WITH (NOLOCK) on bunD.Id = bun.ID and bun.Orderid = st0.Orderid
									and bun.PatternPanel = st0.PatternPanel
									and bun.FabricPanelCode = st0.FabricPanelCode
									and bun.Article = st0.Article
									and bun.Sizecode = st0.Sizecode
inner join BundleInOut bunIO  WITH (NOLOCK) on bunIO.BundleNo = bunD.BundleNo 
where (1 = 1 or st0.QtyBySubprocess != 0) 
		and bunIO.SubProcessId = '{subprocessID}'
		and bunIO.OutGoing is not null
		and isnull(bunIO.RFIDProcessLocationID,'') = ''
group by st0.Orderid, st0.SizeCode, st0.PatternPanel, st0.FabricPanelCode, st0.Patterncode, st0.Article,bunD.BundleGroup, st0.QtyBySet, st0.QtyBySubprocess
--
select	st0.Orderid
		, BundleGroup = r.BundleGroup
		, Size = os.SizeCode
		, st0.Article
		, st0.PatternPanel
		, st0.FabricPanelCode
		, st0.PatternCode
		, InQty = sum (isnull (r.InQty, 0))
		, OutQty = sum (isnull (r.OutQty, 0))
		, OriInQty = sum (isnull (r.InQty, 0))
		, OriOutQty = sum (isnull (r.OutQty, 0))
		, num = count (1) over (partition by st0.Orderid, os.SizeCode, st0.PatternPanel, st0.FabricPanelCode, r.BundleGroup)
into #BundleInOutQty{subprocessIDt}
from #QtyBySetPerCutpart{subprocessIDt} st0
left join Order_SizeCode os on st0.POID = os.Id and st0.Sizecode = os.SizeCode
left join #RFID{subprocessIDt} r on r.OrderID = st0.OrderID 
				and r.Article = st0.Article 
				and r.SizeCode = st0.SizeCode 
				and r.PatternPanel = st0.PatternPanel 
				and r.FabricPanelCode = st0.FabricPanelCode
				and r.PatternCode = st0.PatternCode
where ({isSpectialReader} = 1 or st0.QtyBySubprocess != 0)
group by st0.OrderID, r.BundleGroup, os.SizeCode, st0.PatternPanel, st0.FabricPanelCode, st0.Article, st0.PatternCode, st0.num
";

                if (isNeedCombinBundleGroup)
                {
                    sqlcmd += $@"
--篩選 BundleGroup Step.1 --
update bunInOut
set bunInOut.InQty = 0
	, bunInOut.OutQty = 0
from #BundleInOutQty{subprocessIDt} bunInOut
inner join #QtyBySetPerCutpart{subprocessIDt} bas on bunInOut.OrderID = bas.OrderID
										and bunInOut.PatternPanel = bas.PatternPanel
										and bunInOut.FabricPanelCode = bas.FabricPanelCode
										and bunInOut.Article = bas.Article
										and bunInOut.Size = bas.SizeCode
where bunInOut.num < bas.num

select	OrderID
		, Article
		, Size
		, InQty = min (InQty)
		, OutQty = min (OutQty)
into #FinalQtyBySet{subprocessIDt}
from (
	select	OrderID
			, Size
			, Article
			, PatternPanel
			, InQty = min (InQty)
			, OutQty = min (OutQty)
	from (
		select	OrderID
				, Size
				, Article
				, PatternPanel
				, FabricPanelCode
				, InQty = sum (InQty)
				, OutQty = sum (OutQty)
		from (
			select	OrderID
					, Size
					, Article
					, PatternPanel
					, FabricPanelCode
					, BundleGroup
					, InQty = min (InQty)
					, OutQty = min (OutQty)
			from #BundleInOutQty{subprocessIDt}
			group by OrderID, Size, Article, PatternPanel, FabricPanelCode, BundleGroup
		) minGroupCutpart							
		group by OrderID, Size, Article, PatternPanel, FabricPanelCode
	) sumGroup
	group by OrderID, Size, Article, PatternPanel
) minFabricPanelCode
group by OrderID, Size, Article
";
                }
                else
                {
                    sqlcmd += $@"
select	OrderID
		, Article
		, Size
		, InQty = min (InQty)
		, OutQty = min (OutQty)
into #FinalQtyBySet{subprocessIDt}
from (
	select	OrderID
			, Size
			, Article
			, PatternPanel
			, InQty = min (InQty)
			, OutQty = min (OutQty)
	from (
		select	OrderID
				, Size
				, Article
				, PatternPanel
				, FabricPanelCode
				, InQty = min (InQty)
				, OutQty = min (OutQty)
		from (
			select	OrderID
					, Size
					, PatternPanel
					, FabricPanelCode
					, Article
					, PatternCode
					, InQty = sum (InQty)
					, OutQty = sum (OutQty)
			from #BundleInOutQty{subprocessIDt}
			group by OrderID, Size, Article, PatternPanel, FabricPanelCode, PatternCode
		) sumbas
		group by OrderID, Size, Article, PatternPanel, FabricPanelCode
	) minCutpart
	group by OrderID, Size, Article, PatternPanel
) minFabricPanelCode
group by OrderID, Size, Article
";
                }

                sqlcmd += $@"
-- Result Data --
--	 *	3.	最終算出每張訂單目前可完成的成衣件數
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
into #QtyBySetPerSubprocess{subprocessIDt}
from #CutpartBySet{subprocessIDt} cbs
left join Order_Qty oq  WITH (NOLOCK) on oq.id = cbs.OrderID and oq.SizeCode = cbs.SizeCode and oq.Article = cbs.Article
left join #FinalQtyBySet{subprocessIDt} sub on cbs.Orderid = sub.Orderid and cbs.Sizecode = sub.size and cbs.Article = sub.Article
outer apply (
	select	InQtyByPcs = sum (isnull (bunIO.OriInQty, 0))
			, OutQtyByPcs = sum (isnull (bunIO.OriOutQty, 0))
	from #BundleInOutQty{subprocessIDt} bunIO
	where cbs.OrderID = bunIO.OrderID and cbs.Sizecode = bunIO.Size and cbs.Article = bunIO.Article
) IOQtyPerPcs
";
                if (bySP)
                {
                    sqlcmd += $@"
select OrderID, InQtyBySet = sum (InQty), OutQtyBySet = sum (OutQty)
into #{subprocessIDt}
from(
	select OrderID, SizeCode, InQty = min (InQtyBySet), OutQty = min (OutQtyBySet)
	from #QtyBySetPerSubprocess{subprocessIDt}	minPatternPanel
	group by OrderID, SizeCode
) minArticle
group by OrderID
";
                }
            }

            return sqlcmd;
        }
        #endregion
    }
}

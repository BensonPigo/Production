using Ict;
using Sci;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Transactions;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static partial class Prgs
    {
        #region -- UpdatePO_Supp_Detail --

        /// <inheritdoc/>
        /// <summary>
        /// UpdatePO_Supp_Detail()
        /// *   更新 Po3 的庫存
        /// *-----------------------------------------------------
        /// * 使用新方法
        /// * 1.各程式 OnDetailSelectCommandPrepare()欄位名稱要與這Sqlcommand對上 EX:統一用Location
        /// * 2.延續舊做法True 為增加 / False為減少
        /// * 3.Case2,8才需要傳List<>過來,重組Location
        ///
        /// * Type  :
        /// *   0.  更新Location
        /// *   2.  更新InQty
        /// *   4.  更新OutQty
        /// *   8.  更新LInvQty
        /// *   16. 更新LObQty
        /// *   32. 更新AdQty
        /// *   37. 更新ReturnQty
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="datas">datas</param>
        /// <param name="encoded">encoded</param>
        /// <param name="attachLocation">attachLocation</param>
        /// <param name="sqlConn">sqlConn</param>
        // (整批) A & B倉
        public static string UpdateMPoDetail(int type, List<Prgs_POSuppDetailData> datas, bool encoded, bool attachLocation = true, SqlConnection sqlConn = null)
        {
            #region 以原本的datas的5keys 去ftyinventory撈location和原本loction重組以逗號分開,塞回原本資料
            DataTable tBattachlocation;
            if (datas != null)
            {
                if (attachLocation)
                {
                    /*Location 更新
                        step 1. *** FtyInvetory Location 必須先更新 ***
                        step 2. MDivisionPoDetail 直接重組 FtyInventory
                        step 3. MDivisionPoDetail Locatio 直接 Update => step 2.
                        *** 優點：可以確認 Location 是最新的，且不須累加 Location
                        **  Step 1. 必須確實修改，否則會累加到 FtyInventory 舊的 Location
                    */
                    string sqlcmdforlocation = @"
alter table #Tmp alter column poid varchar(20)
alter table #Tmp alter column seq1 varchar(3)
alter table #Tmp alter column seq2 varchar(3)
alter table #Tmp alter column stocktype varchar(1)

select poid,seq1,seq2,qty,stocktype
,[location] = isnull(stuff(L.locationid,1,1,'' ), '')
from #tmp t
OUTER APPLY(
	SELECT locationid=(
		select distinct concat(',',u.location)
		from 
		(
			select poid,seq1,seq2,stocktype,[location] = fd.mtllocationid
			from ftyinventory f WITH (NOLOCK) 
			inner join ftyinventory_detail fd WITH (NOLOCK) on f.ukey = fd.ukey 
			where f.poid = t.POID and f.seq1 = t.Seq1 
			and f.seq2 = t.Seq2 and f.stocktype = t.StockType
		)u
		for xml path('')
	)
)L
;drop Table #Tmp;";
                    MyUtility.Tool.ProcessWithObject(datas, string.Empty, sqlcmdforlocation, out tBattachlocation, "#Tmp", sqlConn);
                    if (tBattachlocation != null)
                    {
                        var newDatas = tBattachlocation.AsEnumerable().Select(w =>
                                new Prgs_POSuppDetailData
                                {
                                    Poid = w.Field<string>("poid"),
                                    Seq1 = w.Field<string>("seq1"),
                                    Seq2 = w.Field<string>("seq2"),
                                    Stocktype = w.Field<string>("stocktype"),
                                    Qty = w.Field<decimal>("qty"),
                                    Location = w.Field<string>("location"),
                                }).ToList();
                        datas.Clear();
                        datas.AddRange(newDatas);
                    }
                }
            }
            #endregion
            string sqlcmd = string.Empty;
            switch (type)
            {
                case 0:
                    #region  -- case 0 Location --
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'I' then
	update 
	set target.blocation = src.location;

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'B' then
	update 
	set target.alocation = src.location;

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'O' then
	update 
	set target.Clocation = src.location;

drop Table #TmpSource";
                    break;
                #endregion
                case 2:
                    #region -- Case 2 InQty --
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'I' then
	update 
	set target.inqty = isnull(target.inqty,0.00) + src.qty 
    , target.blocation = src.location
when not matched by target and src.stocktype = 'I' then
    insert ([Poid],[Seq1],[Seq2],[inqty],[blocation])
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'B' then
	update 
	set target.inqty = isnull(target.inqty,0.00) + src.qty 
    , target.alocation = src.location
when not matched by target and src.stocktype = 'B' then
    insert ([Poid],[Seq1],[Seq2],[inqty],[alocation])
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'O' then
	update 
	set target.inqty = isnull(target.inqty,0.00) + src.qty 
    , target.Clocation = src.location
when not matched by target and src.stocktype = 'O' then
    insert ([Poid],[Seq1],[Seq2],[inqty],[Clocation])
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);
";
                    }
                    else
                    {
                        sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t
set t.inqty = isnull(t.inqty,0.00) + s.qty
from mdivisionpodetail t 
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2;";
                    }

                    sqlcmd += @";drop Table #TmpSource";
                    #endregion
                    break;
                case 4:
                    #region -- Case 4 OutQty -- 合併
                    sqlcmd = @"
                    alter table #TmpSource alter column poid varchar(20)
                    alter table #TmpSource alter column seq1 varchar(3)
                    alter table #TmpSource alter column seq2 varchar(3)
                    
                    update t
                    set t.OutQty = isnull(t.OutQty,0.00) + s.qty
                    from mdivisionpodetail t 
                    inner join #TmpSource s
                    on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2

                    drop Table #TmpSource;";
                    #endregion
                    break;
                case 8:
                    #region -- Case 8 LInvQty --
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

merge dbo.mdivisionpodetail as target
using #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched then
update
";
                    if (encoded)
                    {
                        sqlcmd += @"
set target.LInvQty = isnull(target.LInvQty,0.00) + src.qty , target.blocation = src.location
when not matched then
    insert ([Poid],[Seq1],[Seq2],[LInvQty],[blocation])
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);
";
                    }
                    else
                    {
                        sqlcmd += @" set target.LInvQty = isnull(target.LInvQty,0.00) + src.qty;";
                    }

                    sqlcmd += @";drop Table #TmpSource";
                    #endregion
                    break;
                case 16:
                    #region -- LObQty --
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t 
set t.LObQty = isnull(t.LObQty,0.00) + s.qty
from mdivisionpodetail t
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2

;drop Table #TmpSource";
                    #endregion
                    break;
                case 32:
                    #region -- AdjustQty --
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t 
set t.AdjustQty = isnull(t.AdjustQty,0.00) + s.qty
from mdivisionpodetail t 
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2

;drop Table #TmpSource";
                    #endregion
                    break;
                case 37:
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t
set t.ReturnQty  = isnull(t.ReturnQty ,0.00) + s.qty
from mdivisionpodetail t 
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2 = s.seq2;";

                    sqlcmd += @";drop Table #TmpSource;";
                    break;
            }

            return sqlcmd;
        }
        #endregion
        #region -- UpdateFtyInventory --

        /// <inheritdoc/>
        /// <summary>
        /// UpdateFtyInventory()
        /// *   更新 FtyInventory 的庫存
        /// *-----------------------------------------------------
        /// * 使用新方法
        /// * IList<DataRow>只是先預留,目前做法都不用,都先填上null即可
        /// ///
        /// * Type  :
        /// *   2.  更新InQty
        /// *   4.  更新OutQty
        /// *   6.  更新OutQty with Location
        /// *   8.  更新AdjustQty
        /// *   26. 更新Location
        /// *   37. 更新Return QTY
        /// *   66. 更新Tone
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="datas">datas</param>
        /// <param name="encoded">encoded</param>
        /// <param name="mtlAutoLock">mtlAutoLock</param>
        /// <returns>w</returns>
        // (整批)
        public static string UpdateFtyInventory_IO(int type, IList<DataRow> datas, bool encoded, int mtlAutoLock = 0)
        {
            string sqlcmd = string.Empty;
            switch (type)
            {
                case 2:
                    #region 更新 inqty
                    sqlcmd = $@"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)
alter table #TmpSource alter column dyelot varchar(8)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

select s.*,psdseq1=psd.seq1
into #tmpS11
from #tmpS1 s
left join PO_Supp_Detail psd on psd.id = s.poid and psd.seq1 = s.seq1 and psd.seq2 = s.seq2

merge dbo.FtyInventory as target
using #tmpS11 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set inqty = isnull(inqty,0.00) + s.qty,
        --本來就Lock的資料不做更新 ISP20211319
         Lock = iif(Lock = 1, Lock, iif(s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99' ,{mtlAutoLock},0)),
         LockName = iif(Lock = 1, LockName, iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {mtlAutoLock}=1 ,'{Env.User.UserID}','')),
         LockDate = iif(Lock = 1, LockDate, iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {mtlAutoLock}=1 ,getdate(),null))
when not matched then
    insert ( [MDivisionPoDetailUkey],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty], [Lock],[LockName],[LockDate])
    values ((select ukey from dbo.MDivisionPoDetail WITH (NOLOCK) 
			 where poid = s.poid and seq1 = s.seq1 and seq2 = s.seq2)
			 ,s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty,
              iif(s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99' ,{mtlAutoLock},0),
              iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {mtlAutoLock}=1 ,'{Env.User.UserID}',''),
              iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {mtlAutoLock}=1 ,getdate(),null)
            );
";
                    if (encoded)
                    {
                        sqlcmd += @"
select distinct [location] = location.[Data] ,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f WITH (NOLOCK) on f.poid = s.poid 
						 and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot
cross apply (select [Data] from [dbo].[SplitString](s.Location,',')) location

merge dbo.ftyinventory_detail as t 
using #tmp_L_K as s on t.ukey = s.ukey and isnull(t.mtllocationid,'') = isnull(s.location,'')
when not matched then
    insert ([ukey],[mtllocationid]) 
	values (s.ukey,isnull(s.location,''));

--delete t from FtyInventory_Detail t
--where  exists(select 1 from #tmp_L_K x where x.ukey=t.Ukey and x.location != t.MtlLocationID)
drop table #tmp_L_K 
"; // ↑最後一段delete寫法千萬不能用merge作,即使只有一筆資料也要跑超久
                    }

                    sqlcmd += @"drop table #tmpS1, #tmpS11; 
                                drop table #TmpSource;";
                    #endregion
                    break;
                case 4:
                    #region 更新OutQty
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

merge dbo.FtyInventory as target
using #tmpS1 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set outqty = isnull(outqty,0.00) + s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[outqty])
    values ((select ukey from dbo.MDivisionPoDetail WITH (NOLOCK) 
			 where poid = s.poid and seq1 = s.seq1 and seq2 = s.seq2)
			 ,s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty);

drop table #tmpS1;
drop table #TmpSource;";
                    #endregion
                    break;
                case 6:
                    #region 更新OutQty with Location
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

merge dbo.FtyInventory as target
using #tmpS1 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
    and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set outqty = isnull(outqty,0.00) + s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[outqty])
    values ((select ukey from dbo.MDivisionPoDetail WITH (NOLOCK) 
             where poid = s.poid and seq1 = s.seq1 and seq2 = s.seq2)
             ,s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty);";
                    if (encoded)
                    {
                        sqlcmd += @"
select distinct [location] = location.[Data] ,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f WITH (NOLOCK) on f.poid = s.poid 
						 and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot
cross apply (select [Data] from [dbo].[SplitString](s.Location,',')) location

merge dbo.ftyinventory_detail as t
using #tmp_L_K as s on t.ukey = s.ukey and t.mtllocationid = s.location
when not matched then
    insert ([ukey],[mtllocationid]) 
    values (s.ukey,isnull(s.location,''));

--delete t from FtyInventory_Detail t
--where  exists(select 1 from #tmp_L_K x where x.ukey=t.Ukey and x.location != t.MtlLocationID)
drop table #tmp_L_K
"; // ↑最後一段delete寫法千萬不能用merge作,即使只有一筆資料也要跑超久
                    }

                    sqlcmd += @"drop table #tmpS1;
                                drop table #TmpSource;";
                    #endregion
                    break;
                case 8:
                    #region 更新AdjustQty
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

merge dbo.FtyInventory as target
using #tmpS1 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
    and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set adjustqty = isnull(adjustqty,0.00) + s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[adjustqty])
    values ((select ukey from dbo.MDivisionPoDetail WITH (NOLOCK) 
             where poid = s.poid and seq1 = s.seq1 and seq2 = s.seq2)
             ,s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty);

drop table #tmpS1 
drop table #TmpSource;";
                    #endregion
                    break;
                case 26:
                    #region 更新Location
                    sqlcmd += @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column roll varchar(15)

select distinct [tolocation] = location.[Data] ,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f WITH (NOLOCK) on f.poid = s.poid 
						 and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot
cross apply (select [Data] from [dbo].[SplitString](s.tolocation,',')) location

delete t from FtyInventory_Detail t
where  t.ukey = (select distinct ukey from #tmp_L_K where t.Ukey = Ukey)                                          

merge dbo.ftyinventory_detail as t
using #tmp_L_K as s on t.ukey = s.ukey and isnull(t.mtllocationid,'') = isnull(s.tolocation,'')
when not matched AND s.Ukey IS NOT NULL then
    insert ([ukey],[mtllocationid]) 
       values (s.ukey,isnull(s.tolocation,''));

drop table #tmp_L_K
drop table #TmpSource
";
                    #endregion
                    break;
                case 27:
                    #region 更新Location (與26的差異: 只新增不刪除)
                    sqlcmd += @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column roll varchar(15)

select distinct [tolocation] = location.[Data] ,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f WITH (NOLOCK) on f.poid = s.poid 
						 and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot
cross apply (select [Data] from [dbo].[SplitString](s.tolocation,',')) location

--delete t from FtyInventory_Detail t
--where  t.ukey = (select distinct ukey from #tmp_L_K where t.Ukey = Ukey)                                          

merge dbo.ftyinventory_detail as t
using #tmp_L_K as s on t.ukey = s.ukey and isnull(t.mtllocationid,'') = isnull(s.tolocation,'')
when not matched AND s.Ukey IS NOT NULL then
    insert ([ukey],[mtllocationid]) 
       values (s.ukey,isnull(s.tolocation,''));

drop table #tmp_L_K
drop table #TmpSource
";
                    #endregion
                    break;
                case 37:
                    #region 更新Return QTY
                    sqlcmd = $@"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select poid, seq1, seq2, stocktype
    ,[roll] = RTRIM(LTRIM(isnull(roll, ''))) 
    ,[ReturnQty] = sum(qty)
    ,[dyelot] = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

select s.*,psdseq1=psd.seq1
into #tmpS11
from #tmpS1 s
left join PO_Supp_Detail psd on psd.id = s.poid and psd.seq1 = s.seq1 and psd.seq2 = s.seq2

merge dbo.FtyInventory as target
using #tmpS11 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set ReturnQty = isnull(target.ReturnQty ,0.00) + s.ReturnQty,
         Lock = iif(s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99' ,{mtlAutoLock},0),
         LockName = iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {mtlAutoLock}=1 ,'{Env.User.UserID}',''),
         LockDate = iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {mtlAutoLock}=1 ,getdate(),null)
when not matched then
    insert ( [MDivisionPoDetailUkey],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[ReturnQty], [Lock],[LockName],[LockDate])
    values ((select ukey from dbo.MDivisionPoDetail WITH (NOLOCK) 
			 where poid = s.poid and seq1 = s.seq1 and seq2 = s.seq2)
			 ,s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.ReturnQty,
              iif(s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99' ,{mtlAutoLock},0),
              iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {mtlAutoLock}=1 ,'{Env.User.UserID}',''),
              iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {mtlAutoLock}=1 ,getdate(),null)
            );
";
                    if (encoded)
                    {
                        sqlcmd += @"
select distinct [location] = location.[Data] ,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f WITH (NOLOCK) on f.poid = s.poid 
						 and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot
cross apply (select [Data] from [dbo].[SplitString](s.Location,',')) location

merge dbo.ftyinventory_detail as t 
using #tmp_L_K as s on t.ukey = s.ukey and isnull(t.mtllocationid,'') = isnull(s.location,'')
when not matched then
    insert ([ukey],[mtllocationid]) 
	values (s.ukey,isnull(s.location,''));

--delete t from FtyInventory_Detail t
--where  exists(select 1 from #tmp_L_K x where x.ukey=t.Ukey and x.location != t.MtlLocationID)
drop table #tmp_L_K 
"; // ↑最後一段delete寫法千萬不能用merge作,即使只有一筆資料也要跑超久
                    }

                    sqlcmd += @"drop table #tmpS1, #tmpS11; 
                                drop table #TmpSource;";
                    #endregion
                    break;
                case 66:
                    sqlcmd = $@"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)
alter table #TmpSource alter column dyelot varchar(8)
alter table #TmpSource alter column FabricType varchar(1)

update f
set Tone = sd.Tone
from #TmpSource sd
inner join FtyInventory f with(nolock) on f.POID = sd.poid
    and f.Seq1 = sd.seq1
    and f.Seq2 = sd.seq2
    and f.Roll = sd.roll
    and f.Dyelot = sd.dyelot
    and f.StockType = sd.stocktype
where sd.FabricType = 'F'

drop table #TmpSource;
";
                    break;
                case 99:
                    #region 物料解鎖/上鎖
                    int lockStatus = encoded ? 1 : 0;
                    sqlcmd = $@"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)
alter table #TmpSource alter column Dyelot varchar(15)

update t
set WMSLock = {lockStatus}
from FtyInventory t
where exists(
    select * from #TmpSource s
    where t.POID = s.POID
    and t.Seq1 = s.Seq1 and t.Seq2 = s.Seq2
    and t.Roll = s.Roll and t.Dyelot = s.Dyelot
    and t.StockType = s.StockType
)
";
                    #endregion
                    break;
            }

            return sqlcmd;
        }

        /// <summary>
        /// update Ftyinventory by P99
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>sqlcmd</returns>
        public static string UpdateFtyInventory_IO_P99(int type)
        {
            string sqlcmd = string.Empty;
            switch (type)
            {
                case 2:
                    #region 更新 inqty
                    sqlcmd = $@"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

select s.*,psdseq1=psd.seq1
into #tmpS11
from #tmpS1 s
left join PO_Supp_Detail psd on psd.id = s.poid and psd.seq1 = s.seq1 and psd.seq2 = s.seq2

merge dbo.FtyInventory as target
using #tmpS11 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set inqty = isnull(inqty,0.00) + s.qty;
";

                    sqlcmd += @"drop table #tmpS1, #tmpS11; 
                                drop table #TmpSource;";
                    #endregion
                    break;
                case 4:
                    #region 更新OutQty
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

merge dbo.FtyInventory as target
using #tmpS1 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set outqty = isnull(outqty,0.00) + s.qty;

drop table #tmpS1;
drop table #TmpSource;";
                    #endregion
                    break;
                case 6:
                    #region 更新OutQty with Location
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

merge dbo.FtyInventory as target
using #tmpS1 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
    and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set outqty = isnull(outqty,0.00) + s.qty;";
                    sqlcmd += @"drop table #tmpS1;
                                drop table #TmpSource;";
                    #endregion
                    break;
                case 8:
                    #region 更新AdjustQty
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

merge dbo.FtyInventory as target
using #tmpS1 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
    and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set adjustqty = isnull(adjustqty,0.00) + s.qty;

drop table #tmpS1 
drop table #TmpSource;";
                    #endregion
                    break;
            }

            return sqlcmd;
        }

        /// <summary>
        /// for update P99 only
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="encoded">encoded</param>
        /// <returns>string sqlcmd</returns>
        public static string UpdateMPoDetail_P99(int type, bool encoded)
        {
            string sqlcmd = string.Empty;
            switch (type)
            {
                case 2:
                    #region -- Case 2 InQty --
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched then
	update 
	set target.inqty = isnull(target.inqty,0.00) + src.qty ;
";
                    }
                    else
                    {
                        sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t
set t.inqty = isnull(t.inqty,0.00) + s.qty
from mdivisionpodetail t 
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2;";
                    }

                    sqlcmd += @";drop Table #TmpSource";
                    #endregion
                    break;
                case 4:
                    #region -- Case 4 OutQty -- 合併
                    sqlcmd = @"
                    alter table #TmpSource alter column poid varchar(20)
                    alter table #TmpSource alter column seq1 varchar(3)
                    alter table #TmpSource alter column seq2 varchar(3)
                    
                    update t
                    set t.OutQty = isnull(t.OutQty,0.00) + s.qty
                    from mdivisionpodetail t 
                    inner join #TmpSource s
                    on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2

                    drop Table #TmpSource;";
                    #endregion
                    break;
                case 8:
                    #region -- Case 8 LInvQty --
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

merge dbo.mdivisionpodetail as target
using #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched then
update
";
                    if (encoded)
                    {
                        sqlcmd += @"
set target.LInvQty = isnull(target.LInvQty,0.00) + src.qty 
";
                    }
                    else
                    {
                        sqlcmd += @" set target.LInvQty = isnull(target.LInvQty,0.00) + src.qty;";
                    }

                    sqlcmd += @";drop Table #TmpSource";
                    #endregion
                    break;
                case 16:
                    #region -- LObQty --
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t 
set t.LObQty = isnull(t.LObQty,0.00) + s.qty
from mdivisionpodetail t
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2

;drop Table #TmpSource";
                    #endregion
                    break;
                case 32:
                    #region -- AdjustQty --
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t 
set t.AdjustQty = isnull(t.AdjustQty,0.00) + s.qty
from mdivisionpodetail t 
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2

;drop Table #TmpSource";
                    #endregion
                    break;
                case 37:
                    #region Case 37 Return Qty
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t
set t.ReturnQty  = isnull(t.ReturnQty ,0.00) + s.qty
from mdivisionpodetail t 
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2 = s.seq2;

drop Table #TmpSource;
";
                    break;
                    #endregion
            }

            return sqlcmd;
        }

        #endregion
        #region  -- Update LocalOrderInventory --
        public static string UpdateLocalOrderInventory_IO(string type, IList<DataRow> datas, bool encoded)
        {
            string sqlcmd = string.Empty;
            switch (type)
            {
                case "In":
                    #region 更新 inqty
                    sqlcmd = $@"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)
alter table #TmpSource alter column dyelot varchar(8)
alter table #TmpSource alter column tone varchar(8)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, ''), tone = isnull(tone,'')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, ''),isnull(tone,'')


merge dbo.LocalOrderInventory as target
using #tmpS1 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set inqty = isnull(inqty,0.00) + s.qty    
when not matched then
    insert ( [Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty],[Tone])
    values ( s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty  ,s.tone);
";
                    if (encoded)
                    {
                        sqlcmd += @"
select distinct [location] = location.[Data] ,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join LocalOrderInventory f WITH (NOLOCK) on f.poid = s.poid 
						 and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot
cross apply (select [Data] from [dbo].[SplitString](s.Location,',')) location

merge dbo.LocalOrderInventory_Location as t 
using #tmp_L_K as s on t.LocalOrderInventoryUkey = s.ukey and isnull(t.mtllocationid,'') = isnull(s.location,'')
when not matched then
    insert ([LocalOrderInventoryUkey],[mtllocationid]) 
	values (s.ukey,isnull(s.location,''));

drop table #tmp_L_K

";
                    }

                    sqlcmd += @"drop table #tmpS1; 
                                drop table #TmpSource;";
                    #endregion
                    break;
                case "Out":
                    #region 更新OutQty
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

merge dbo.LocalOrderInventory as target
using #tmpS1 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set outqty = isnull(outqty,0.00) + s.qty
when not matched then
    insert ( [Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[outqty])
    values ( s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty);

drop table #tmpS1;
drop table #TmpSource;";
                    #endregion
                    break;
                case "Location":
                    #region 更新Location
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column roll varchar(15)

select distinct [tolocation] = location.[Data] ,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join LocalOrderInventory f WITH (NOLOCK) on f.poid = s.poid 
						 and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot
cross apply (select [Data] from [dbo].[SplitString](s.tolocation,',')) location

delete t from LocalOrderInventory_Location t
where  t.LocalOrderInventoryUkey = (select distinct ukey from #tmp_L_K where t.LocalOrderInventoryUkey = Ukey)                                          

merge dbo.LocalOrderInventory_Location as t
using #tmp_L_K as s on t.LocalOrderInventoryUkey = s.ukey and isnull(t.mtllocationid,'') = isnull(s.tolocation,'')
when not matched AND s.Ukey IS NOT NULL then
    insert ([LocalOrderInventoryUkey],[mtllocationid]) 
    values (s.ukey,isnull(s.tolocation,''));

drop table #tmp_L_K
drop table #TmpSource
";
                    #endregion
                    break;
                case "Adjust":
                    #region 更新AdjustQty
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) ,[qty] = sum(qty), dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource
group by poid, seq1, seq2, stocktype, RTRIM(LTRIM(isnull(roll, ''))) ,isnull(dyelot, '')

merge dbo.LocalOrderInventory as target
using #tmpS1 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
    and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set adjustqty = isnull(adjustqty,0.00) + s.qty
when not matched then
    insert ([Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[adjustqty])
    values (s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty);

drop table #tmpS1 
drop table #TmpSource;";
                    #endregion
                    break;
                case "Tone":
                    sqlcmd = $@"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)
alter table #TmpSource alter column dyelot varchar(8)
alter table #TmpSource alter column FabricType varchar(1)

update f
set Tone = sd.Tone
from #TmpSource sd
inner join LocalOrderInventory f with(nolock) on f.POID = sd.poid
    and f.Seq1 = sd.seq1
    and f.Seq2 = sd.seq2
    and f.Roll = sd.roll
    and f.Dyelot = sd.dyelot
    and f.StockType = sd.stocktype
where sd.FabricType = 'F'

drop table #TmpSource;
";
                    break;
            }

            return sqlcmd;
        }

        #endregion

        #region -- SelePoItem --

        /// <summary>
        /// selePoItemSqlCmd
        /// </summary>
        /// <param name="junk">junk</param>
        /// <returns>string</returns>
        public static string SelePoItemSqlCmd(bool junk = true)
        {
            return @"
select  psd.id,concat(Ltrim(Rtrim(psd.seq1)), ' ', psd.seq2) as seq
        , psd.Refno   
        , dbo.getmtldesc(psd.id,psd.seq1,psd.seq2,2,0) as Description 
        , ColorID = isnull(psdsC.SpecValue, '')
        , [WH_P07_Color] = Color.Value
        , SizeSpec= isnull(psdsS.SpecValue, '')
        , psd.FinalETA
        , isnull(m.InQty, 0) as InQty
        , psd.pounit
        , StockUnit = dbo.GetStockUnitBySPSeq (psd.id, psd.seq1, psd.seq2)
        , isnull(m.OutQty, 0) as outQty
        , isnull(m.AdjustQty, 0) as AdjustQty
        , isnull(m.ReturnQty, 0) as ReturnQty
        , isnull(m.inqty, 0) - isnull(m.OutQty, 0) + isnull(m.AdjustQty, 0) - isnull(m.ReturnQty, 0) as balance
        , isnull(m.LInvQty, 0) as LInvQty
        , isnull(m.LObQty, 0) as LObQty
        , psd.fabrictype
        , psd.seq1
        , psd.seq2
        , psd.scirefno
        , Qty = Round (psd.qty * v.Ratevalue, 2)
        ,[Status]=IIF(LockStatus.LockCount > 0 ,'Locked','Unlocked')
        ,Fabric.MtlTypeID
from dbo.PO_Supp_Detail psd WITH (NOLOCK) 
inner join View_WH_Orders o on psd.id = o.id
inner join Factory f on o.FtyGroup = f.id
inner join View_unitrate v on v.FROM_U = psd.POUnit and v.TO_U = dbo.GetStockUnitBySPSeq (psd.id, psd.seq1, psd.seq2)
left join dbo.mdivisionpodetail m WITH (NOLOCK) on m.poid = psd.id and m.seq1 = psd.seq1 and m.seq2 = psd.seq2
left JOIN Fabric WITH (NOLOCK) ON psd.SCIRefNo=Fabric.SCIRefNo
left join [dbo].[MtlType] mt WITH (NOLOCK) on mt.ID = Fabric.MtlTypeID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN Fabric.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(isnull(psd.SuppColor,'') = '', dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, '')), psd.SuppColor)
		 ELSE dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, ''))
	 END
)Color
OUTER APPLY(
	SELECT [LockCount]=COUNT(UKEY)
	FROM FtyInventory
	WHERE POID='{0}'
	AND Seq1=psd.Seq1
	AND Seq2=psd.Seq2
	AND Lock = 1
)LockStatus
where psd.id ='{0}'
" + (junk ? "and psd.Junk = 0" : string.Empty);
        }

        /// <summary>
        /// 右鍵開窗選取採購項
        /// </summary>
        /// <param name="poid">poid</param>
        /// <param name="defaultseq">defaultseq</param>
        /// <param name="filters">filters</param>
        /// <param name="junk">junk</param>
        /// <returns>Sci.Win.Tools.SelectItem</returns>
        public static Win.Tools.SelectItem SelePoItem(string poid, string defaultseq, string filters = null, bool junk = true)
        {
            DataTable dt;
            string poItemSql = SelePoItemSqlCmd(junk);
            if (!MyUtility.Check.Empty(poItemSql))
            {
                poItemSql += string.Format(" And {0}", filters);
            }

            string sqlcmd = string.Format(poItemSql, poid, Env.User.Keyword);
            poItemSql = string.Empty;

            DBProxy.Current.Select(null, sqlcmd, out dt);

            Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(dt, "Seq,refno,description,colorid,SizeSpec,FinalETA,inqty,stockunit,outqty,adjustqty,ReturnQty,balance,linvqty,LObQty", "6,8,35,8,10,6,6,6,6,6,6,6,6", defaultseq, "Seq,Ref#,Description,Color,Size,ETA,In Qty,Stock Unit,Out Qty,Adqty,Return Qty,Balance,Inventory Qty,Scrap Qty")
            {
                Width = 1024,
            };

            return selepoitem;
        }
        #endregion

        /// <summary>
        /// 右鍵開窗選取物料儲位 SelectLocation
        /// </summary>
        /// <param name="stocktype">stocktype</param>
        /// <param name="defaultseq">defaultseq</param>
        /// <returns>Sci.Win.Tools.SelectItem2</returns>
        public static Win.Tools.SelectItem2 SelectLocation(string stocktype, string defaultseq = "")
        {
            string sqlcmd = string.Empty;
            if (MyUtility.Check.Empty(stocktype))
            {
                sqlcmd = @"
SELECT  id
        , Description
        , StockType = Case StockType
                        when 'i' then 
                            'Inventory' 
                        when 'b' then 
                            'Bulk' 
                        when 'o' then 
                            'Scrap' 
                       End
        , StockTypeCode = StockType
FROM DBO.MtlLocation WITH (NOLOCK) 
WHERE   junk != '1'";
            }
            else
            {
                sqlcmd = $@"
SELECT  id
        , Description
        , StockType = Case StockType
                        when 'i' then 
                            'Inventory' 
                        when 'b' then 
                            'Bulk' 
                        when 'o' then 
                            'Scrap' 
                       End
FROM DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{stocktype}'
        and junk != '1'";
            }

            Win.Tools.SelectItem2 selectlocation = new Win.Tools.SelectItem2(sqlcmd, "Location ID,Description,Stock Type", "13,60,10", defaultseq, null, null, null)
            {
                Width = 1024,
            };

            return selectlocation;
        }

        /// <inheritdoc/>
        public static bool CheckLocationExists(string stocktype, string location)
        {
            if (MyUtility.Check.Empty(location))
            {
                return true;
            }

            string sqlcmd = string.Empty;
            if (MyUtility.Check.Empty(stocktype))
            {
                sqlcmd = $@"
SELECT  id
        , Description
        , StockType = Case StockType
                        when 'i' then 
                            'Inventory' 
                        when 'b' then 
                            'Bulk' 
                        when 'o' then 
                            'Scrap' 
                       End
        , StockTypeCode = StockType
FROM DBO.MtlLocation WITH (NOLOCK) 
WHERE   junk != '1' and ID = '{location}'";
            }
            else
            {
                sqlcmd = $@"
SELECT  id
        , Description
        , StockType = Case StockType
                        when 'i' then 
                            'Inventory' 
                        when 'b' then 
                            'Bulk' 
                        when 'o' then 
                            'Scrap' 
                       End
FROM DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{stocktype}'
        and junk != '1' and ID = '{location}'";
            }

            return MyUtility.Check.Seek(sqlcmd);
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public static IList<DataRow> Autopick(DataRow materials, bool isIssue = true, string stocktype = "B")
        {
            List<DataRow> items = new List<DataRow>();
            string sqlcmd;
            DataTable dt;
            decimal request; // 需求總數

            decimal accu_issue = 0m;
            if (isIssue)
            {
                request = decimal.Parse(materials["requestqty"].ToString()) - decimal.Parse(materials["accu_issue"].ToString());
                sqlcmd = $@"
with cte as (
    select  Dyelot
            , sum(inqty - OutQty + AdjustQty - ReturnQty) as GroupQty
    from dbo.FtyInventory a WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on  psd.id = a.POID and psd.seq1 = a.Seq1 and psd.seq2 = a.Seq2
    inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
    where   poid = '{materials["poid"]}' 
            and Stocktype = '{stocktype}' 
            and inqty - OutQty + AdjustQty - ReturnQty > 0
            and psd.SCIRefno = '{materials["scirefno"]}' 
            and isnull(psdsC.SpecValue, '') = '{materials["colorid"]}' 
            and a.Seq1 BETWEEN '00' AND '99'
    Group by Dyelot
) 
select  location = Stuff ((select ',' + t.mtllocationid 
                           from (
                                select MtlLocationID 
                                from dbo.FtyInventory_Detail WITH (NOLOCK) 
                                where ukey = a.Ukey
                           )t 
                           for xml path('')
                          ), 1, 1, '')
        , a.Ukey as FtyInventoryUkey
        , POID
        , a.seq1
        , a.Seq2
        , roll = RTRIM(LTRIM(roll))
        , stocktype
        , Dyelot = RTRIM(LTRIM(a.Dyelot))
        , inqty - OutQty + AdjustQty - ReturnQty qty
        , inqty
        , outqty
        , adjustqty
        , ReturnQty
        , inqty - OutQty + AdjustQty - ReturnQty balanceqty
        , running_total = sum(inqty - OutQty + AdjustQty - ReturnQty) over (order by c.GroupQty DESC,a.Dyelot,(inqty - OutQty + AdjustQty - ReturnQty) desc
                                                            rows between unbounded preceding and current row)
        --,c.GroupQty
from cte c 
inner join dbo.FtyInventory a WITH (NOLOCK) on a.Dyelot=c.Dyelot
inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on  psd.id = a.POID and psd.seq1 = a.Seq1 and psd.seq2 = a.Seq2
inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
where   poid = '{materials["poid"]}' 
        and Stocktype = '{stocktype}' 
        and inqty - OutQty + AdjustQty - ReturnQty > 0
        and psd.SCIRefno = '{materials["scirefno"]}' 
        and isnull(psdsC.SpecValue, '') = '{materials["colorid"]}' 
        and a.Seq1 BETWEEN '00' AND '99'
";
            }
            else if (isIssue == false && stocktype == "B")
            {
                request = decimal.Parse(materials["requestqty"].ToString());
                sqlcmd = string.Format(
                    @"
with cte as (
    select  Dyelot
            , sum(inqty - OutQty + AdjustQty - ReturnQty) as GroupQty
    from dbo.FtyInventory a WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on  p.id = a.POID 
                                                      and p.seq1 = a.Seq1 
                                                      and p.seq2 = a.Seq2
    where   poid = '{1}' 
            and Stocktype = '{4}' 
            and inqty - OutQty + AdjustQty - ReturnQty > 0
            and p.seq1 = '{2}' 
            and p.seq2 = '{3}'
    Group by Dyelot
) 
select  location = Stuff ((select ',' + t.mtllocationid 
                           from (
                                select MtlLocationID 
                                from dbo.FtyInventory_Detail WITH (NOLOCK) 
                                where ukey = a.Ukey
                           )t 
                           for xml path('')
                          ), 1, 1, '')
        , a.Ukey as FtyInventoryUkey
        , POID
        , a.seq1
        , a.Seq2
        , roll = RTRIM(LTRIM(roll))
        , stocktype
        , Dyelot = RTRIM(LTRIM(a.Dyelot))
        , inqty - OutQty + AdjustQty - ReturnQty qty
        , inqty
        , outqty
        , adjustqty
        , ReturnQty
        , inqty - OutQty + AdjustQty - ReturnQty balanceqty
        , running_total = sum(inqty - OutQty + AdjustQty - ReturnQty) over (order by c.GroupQty DESC,a.Dyelot,(inqty - OutQty + AdjustQty - ReturnQty) DESC
                                                            rows between unbounded preceding and current row) 
        , num = ROW_NUMBER()over(order by a.Dyelot)
        ,c.GroupQty
from dbo.FtyInventory a WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on  p.id = a.POID 
                                                  and p.seq1 = a.Seq1 
                                                  and p.seq2 = a.Seq2
inner join cte c on c.Dyelot = a.Dyelot
where   a.lock = 0
        and poid = '{1}' 
        and Stocktype = '{4}' 
        and inqty - OutQty + AdjustQty - ReturnQty > 0
        and p.seq1 = '{2}' 
        and p.seq2 = '{3}'", Env.User.Keyword, materials["poid"], materials["seq1"], materials["seq2"], stocktype);
            }
            else
            {
                // P29,P30 Auto Pick
                request = decimal.Parse(materials["requestqty"].ToString());
                sqlcmd = $@"
with cte as (
    select  Dyelot
            , sum(inqty - OutQty + AdjustQty - ReturnQty) as GroupQty
    from dbo.FtyInventory a WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on  p.id = a.POID 
                                                      and p.seq1 = a.Seq1 
                                                      and p.seq2 = a.Seq2
    where   poid = '{materials["StockPOID"]}' 
            and Stocktype = '{stocktype}' 
            and inqty - OutQty + AdjustQty - ReturnQty > 0
            and p.seq1 = '{materials["StockSeq1"]}' 
            and p.seq2 = '{materials["StockSeq2"]}'
    Group by Dyelot
) 
select  location = Stuff ((select ',' + t.mtllocationid 
                           from (
                                select MtlLocationID 
                                from dbo.FtyInventory_Detail WITH (NOLOCK) 
                                where ukey = a.Ukey
                           )t 
                           for xml path('')
                          ), 1, 1, '')
        , a.Ukey as FtyInventoryUkey
        , POID
        , a.seq1
        , a.Seq2
        , roll = RTRIM(LTRIM(roll))
        , stocktype
        , Dyelot = RTRIM(LTRIM(a.Dyelot))
        , inqty - OutQty + AdjustQty - ReturnQty qty
        , inqty
        , outqty
        , adjustqty
        , ReturnQty
        , inqty - OutQty + AdjustQty - ReturnQty balanceqty
        , running_total = sum(inqty - OutQty + AdjustQty - ReturnQty) over (order by c.GroupQty DESC,a.Dyelot,(inqty - OutQty + AdjustQty - ReturnQty) DESC
                                                                 rows between unbounded preceding and current row) 
        --,c.GroupQty
from dbo.FtyInventory a WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on  p.id = a.POID 
                                                  and p.seq1 = a.Seq1 
                                                  and p.seq2 = a.Seq2
inner join cte c on c.Dyelot = a.Dyelot
where   poid = '{materials["StockPOID"]}' 
        and Stocktype = '{stocktype}' 
        and inqty - OutQty + AdjustQty - ReturnQty > 0
        and p.seq1 = '{materials["StockSeq1"]}' 
        and p.seq2 = '{materials["StockSeq2"]}'";
            }

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(sqlcmd, "Sql Error");
                return null;
            }
            else
            {
                DataTable findrow = null;
                /*
                 * 先確認是否有數量剛好足夠的 Roll + Dyelot
                 * 若有則該項直接帶出
                 * 否則用 AutoPick 規則挑選
                 */
                if (isIssue == false && stocktype == "B")
                {
                    // P28 Auto Pick
                    decimal blance = request - accu_issue;
                    List<long> num = new List<long>(); // 紀錄已分配

                    while (blance > 0)
                    {
                        #region 未分配且, 任何一筆有=剩餘blance
                        if (blance > 0m && dt.AsEnumerable().Any(n => ((decimal)n["qty"]).EqualDecimal(blance) && !num.Contains((long)n["num"])))
                        {
                            items.Add(dt.AsEnumerable().Where(n => ((decimal)n["qty"]).EqualDecimal(blance) && !num.Contains((long)n["num"])).First());
                            blance = 0m;
                            break;
                        }
                        #endregion

                        #region 未分配且, 任何一Dyelot總和=剩餘blance
                        if (dt.AsEnumerable().Any(n => ((decimal)n["groupqty"]).EqualDecimal(blance) && !num.Contains((long)n["num"])))
                        {
                            var dyelotS = dt.AsEnumerable().Where(n => ((decimal)n["groupqty"]).EqualDecimal(blance) && !num.Contains((long)n["num"])).CopyToDataTable();

                            // 找出符合的dyelot資料, 筆數最小,若筆數一樣要qty的標準差最小 ,直接order by第一筆即是結果
                            string s = "select  top 1 dyelot,c=count(1),std = stdev(qty) from #tmp group by dyelot order by c ,std";
                            DataTable ct_std;
                            DualResult result1 = MyUtility.Tool.ProcessWithDatatable(dyelotS, "dyelot,qty", s, out ct_std);
                            if (!result1)
                            {
                                MyUtility.Msg.ErrorBox(result1.ToString());
                                break;
                            }

                            var dyelot = ct_std.Rows[0]["dyelot"].ToString();
                            var dyelotRows = dt.AsEnumerable().Where(n => ((string)n["Dyelot"]).EqualString(dyelot.ToString()));
                            foreach (var item in dyelotRows)
                            {
                                items.Add(item);
                            }

                            blance = 0m;
                            break;
                        }
                        #endregion

                        #region 未分配且, 任何一筆有 > blance
                        if (dt.AsEnumerable().Any(n => (decimal)n["qty"] > blance && !num.Contains((long)n["num"])))
                        {
                            DataRow x = dt.AsEnumerable().Where(n => (decimal)n["qty"] > blance && !num.Contains((long)n["num"])).OrderBy(n => (decimal)n["qty"]).First();
                            x["qty"] = blance;
                            items.Add(x);
                            blance = 0m;
                            break;
                        }
                        #endregion

                        #region 未分配且, 為最大qty的dyelot
                        var dyelot1 = dt.AsEnumerable().Where(n => !num.Contains((long)n["num"])).OrderByDescending(n => (decimal)n["qty"]);
                        if (dyelot1.Count() == 0)
                        {
                            break;
                        }

                        var maxdyelot = dt.AsEnumerable().Where(n => !num.Contains((long)n["num"])).OrderByDescending(n => (decimal)n["qty"]).First().GetValue("Dyelot");
                        var frow = dt.AsEnumerable().Where(n => ((string)n["Dyelot"]).EqualString(maxdyelot.ToString()) && !num.Contains((long)n["num"])).OrderByDescending(z => (decimal)z["qty"]);

                        foreach (var item in frow)
                        {
                            if (frow.Any(n => ((decimal)n["qty"]).EqualDecimal(blance) && !num.Contains((long)n["num"])))
                            {
                                items.Add(frow.Where(n => ((decimal)n["qty"]).EqualDecimal(blance) && !num.Contains((long)n["num"])).First());
                                blance = 0m;
                                break;
                            }
                            else
                            {
                                if (frow.Any(n => (decimal)n["qty"] > blance && !num.Contains((long)n["num"])))
                                {
                                    DataRow x = frow.Where(n => (decimal)n["qty"] > blance && !num.Contains((long)n["num"])).OrderBy(n => (decimal)n["qty"]).First();
                                    x["qty"] = blance;
                                    items.Add(x);
                                    blance = 0m;
                                    break;
                                }
                                else
                                {
                                    if ((blance - (decimal)item.GetValue("qty")) > 0)
                                    {
                                        items.Add(item);
                                        num.Add((long)item.GetValue("num")); // 紀錄已經用過的
                                        blance -= (decimal)item.GetValue("qty");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    if (dt.AsEnumerable().Any(n => ((decimal)n["qty"]).EqualDecimal(request)))
                    {
                        items.Add(dt.AsEnumerable().Where(n => ((decimal)n["qty"]).EqualDecimal(request)).CopyToDataTable().Rows[0]);
                    }
                    else
                    {
                        #region AutoPick
                        foreach (DataRow dr2 in dt.Rows)
                        {
                            if ((decimal)dr2["running_total"] < request)
                            {
                                items.Add(dr2);
                                accu_issue = decimal.Parse(dr2["running_total"].ToString());
                            }
                            else
                            {
                                // 依照最後一塊料的Dyelot來找到對應的Group來取得最後一塊料
                                findrow = dt.AsEnumerable().Where(row => row["Dyelot"].EqualString(dr2["Dyelot"].ToString())).CopyToDataTable();
                                break;
                            }
                        }

                        if (accu_issue < request && findrow != null)
                        {
                            // 累計發料數小於需求數時，再反向取得最後一塊料。
                            decimal balance = request - accu_issue;

                            // dt.DefaultView.Sort = "Dyelot,location,Seq1,seq2,Qty asc";
                            for (int i = findrow.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow find = items.Find(item => item["ftyinventoryukey"].ToString() == findrow.Rows[i]["ftyinventoryukey"].ToString());
                                if (MyUtility.Check.Empty(find))
                                {
                                    // if overlape
                                    if (balance > 0m)
                                    {
                                        if (balance >= (decimal)findrow.Rows[i]["qty"])
                                        {
                                            items.Add(findrow.Rows[i]);
                                            balance -= (decimal)findrow.Rows[i]["qty"];
                                        }
                                        else
                                        {
                                            // 最後裁切
                                            // P10最後裁切若有小數點需無條件進位
                                            if (isIssue)
                                            {
                                                if (Math.Ceiling(balance) >= (decimal)findrow.Rows[i]["qty"])
                                                {
                                                    items.Add(findrow.Rows[i]);
                                                    balance = 0m;
                                                }
                                                else
                                                {
                                                    findrow.Rows[i]["qty"] = Math.Ceiling(balance);
                                                    items.Add(findrow.Rows[i]);
                                                    balance = 0m;
                                                }
                                            }
                                            else
                                            {
                                                findrow.Rows[i]["qty"] = balance;
                                                items.Add(findrow.Rows[i]);
                                                balance = 0m;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                }
            }

            return items;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public static IList<DataRow> AutoPickTape(DataRow materials, string cutplanid, bool isIssue = true, string stocktype = "B")
        {
            List<DataRow> items = new List<DataRow>();
            string sqlcmd;
            DataTable dt;

            // 此筆需求數 = 總需求數 - 已經issue總數
            decimal request = decimal.Parse(materials["requestqty"].ToString()) - decimal.Parse(materials["accu_issue"].ToString());
            sqlcmd = $@"
select distinct a.Seq1, a.Seq2, ctpd.Dyelot 
,GroupQty = sum(a.InQty - a.OutQty + a.AdjustQty - a.ReturnQty)
,ReleaseQty = ReleaseQty.value
into #tmp
from  CutTapePlan_Detail ctpd
inner join CutTapePlan ctp on ctp.ID = ctpd.ID
inner join PO_Supp_Detail psd on psd.Refno = ctpd.RefNo and psd.ID = ctp.CuttingID
inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color' and isnull(psdsC.SpecValue, '') = ctpd.ColorID 
inner join FtyInventory a on a.POID = '{materials["poid"]}' and a.Seq1 = psd.Seq1 and a.Seq2 = psd.Seq2 and a.Dyelot = ctpd.Dyelot
outer apply(
	select value= sum(ReleaseQty)
	from CutTapePlan_Detail
	where ID = ctp.ID
	and Dyelot = ctpd.Dyelot
)ReleaseQty
where ctpd.id = '{cutplanid}' and a.Seq1 between '01' and '99' and a.StockType = '{stocktype}'
and ctpd.ColorID = '{materials["ColorID"]}'
group by a.Seq1, a.Seq2, ctpd.Dyelot, ctpd.ColorID,ReleaseQty.value

select
	location = Stuff ((select ',' + t.mtllocationid 
                           from (
                                select MtlLocationID 
                                from dbo.FtyInventory_Detail WITH (NOLOCK) 
                                where ukey = a.Ukey
                           )t 
                           for xml path('')
                          ), 1, 1, '')
	, FtyInventoryUkey = a.Ukey
	, a.POID
	, a.seq1
	, a.Seq2
	, roll = RTRIM(LTRIM(roll))
	, stocktype
	, Dyelot = RTRIM(LTRIM(a.Dyelot))
	, qty = inqty - OutQty + AdjustQty - ReturnQty
	, inqty
	, outqty
	, adjustqty
    , ReturnQty
    , ReleaseQty
	, balanceqty = inqty - OutQty + AdjustQty - ReturnQty
    , running_total = sum(inqty - OutQty + AdjustQty - ReturnQty) over (order by t.GroupQty DESC,a.Dyelot,(inqty - OutQty + AdjustQty - ReturnQty) desc
                                                        rows between unbounded preceding and current row)
from #tmp t
inner join FtyInventory a on a.POID = '{materials["poid"]}' and a.Seq1 = t.Seq1 and a.Seq2 = t.Seq2 and a.Dyelot = t.Dyelot and a.StockType = '{stocktype}'

drop table #tmp
";

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(sqlcmd, "Sql Error");
                return null;
            }

            if (dt.Rows.Count == 0 || dt.Select("balanceqty<>0").Length == 0)
            {
                return null;
            }

            DataTable dtDyelot = dt.DefaultView.ToTable(distinct: true, columnNames: new string[] { "Dyelot", "ReleaseQty" }).DefaultView.ToTable();

            dt = dt.Select("balanceqty<>0").CopyToDataTable();
            if (dt.AsEnumerable().Any(n => ((decimal)n["qty"]).EqualDecimal(request)))
            {
                items.Add(dt.AsEnumerable().Where(n => ((decimal)n["qty"]).EqualDecimal(request)).CopyToDataTable().Rows[0]);
            }
            else
            {
                #region AutoPick
                /*
                 1.Group by Dyelot和ReleaseQty
                 2.相同Dyelot依序分配數量,把RelsQty扣完為止
                 3.當前資料 若庫存 > 剩餘RelsQty,則剩餘數全部給該筆, 其餘沒分配的Dyelot則不顯示
                 */
                foreach (DataRow drDyelot in dtDyelot.Rows)
                {
                    decimal balQty = MyUtility.Convert.GetDecimal(drDyelot["ReleaseQty"]);
                    foreach (DataRow dr2 in dt.Rows)
                    {
                        // 找出相同的Dtelot
                        if (string.Compare(dr2["Dyelot"].ToString(), drDyelot["Dyelot"].ToString(), ignoreCase: true) == 0)
                        {
                            if (balQty >= (decimal)dr2["balanceqty"])
                            {
                                dr2["qty"] = dr2["balanceqty"];
                                items.Add(dr2);
                                balQty -= (decimal)dr2["qty"];
                            }
                            else
                            {
                                dr2["qty"] = balQty;
                                items.Add(dr2);
                                break;
                            }
                        }
                    }
                }
                #endregion
            }

            return items;
        }

        /// <summary>
        /// 目的：自動產生可以寫入Issue_Detail的DataRow
        /// </summary>
        /// <param name="material">P33表身</param>
        /// <param name="accuIssued">accuIssued</param>
        /// <returns>準備寫入P33第三層的DataRow (資料結構: Issue_Detail)</returns>
        public static List<DataRow> Thread_AutoPick(DataRow material, decimal accuIssued)
        {
            List<DataRow> items = new List<DataRow>();

            // foreach (DataRow material in materials)
            // {
            string sqlcmd = string.Empty;
            DataTable dt;
            decimal request; // 需求總數

            // decimal accu_issue = 0m;

            // decimal AccuIssued = MyUtility.Check.Empty(material["AccuIssued"]) ? 0 : decimal.Parse(material["AccuIssued"].ToString());
            decimal useQtyByStockUnit = MyUtility.Check.Empty(material["Use Qty By Stock Unit"]) ? 0 : decimal.Parse(material["Use Qty By Stock Unit"].ToString());

            // 需求量 - 已發累計量 = 待發的量
            request = useQtyByStockUnit; // - AccuIssued;

            // 取得所有項次號(欄位名稱跟Issue_Detail一樣)
            sqlcmd = $@"
select   [POID]=psd.ID
    , psd.Seq1
    , psd.Seq2
	, psd.SCIRefno
	, psd.SuppColor
    , a.stocktype
    , [BulkQty] =ISNULL( a.inqty - a.outqty + a.adjustqty - a.ReturnQty,0.00)
	, [Qty]=0.00
	, [BulkLocation]= STUFF ((
									SELECT ',' + MtlLocationID 
									FROM dbo.FtyInventory_Detail WITH (NOLOCK) 
									WHERE ukey = a.Ukey AND MtlLocationID <> ''
									FOR XML PATH('')
								 ), 1, 1, '')
    , [FtyInventoryUkey]=a.Ukey
from dbo.FtyInventory a WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on  psd.id = a.POID and psd.seq1 = a.Seq1 and psd.seq2 = a.Seq2
inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
where psd.ID = '{material["poid"]}'
and psd.SCIRefno = '{material["SCIRefno"]}' 
and isnull(psdsC.SpecValue, '') = '{material["ColorID"]}' 
AND (a.stocktype = 'B' OR a.stocktype IS NULL)
AND m.IsThread=1
";

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(sqlcmd, "Sql Error");
                return null;
            }
            else
            {
                if (!dt.AsEnumerable().Any())
                {
                    return items;
                }

                // 先確認是否有數量剛好足夠的 Seq1 + Seq2， 若有則該項直接帶出
                if (dt.AsEnumerable().Any(o => (decimal)o["BulkQty"] == request))
                {
                    DataRow imortRow = dt.AsEnumerable().Where(o => ((decimal)o["BulkQty"]).EqualDecimal(request)).FirstOrDefault();
                    imortRow["Qty"] = request;
                    items.Add(imortRow);
                }
                else
                {
                    // 沒有的話，則從Qty > 需求量的,找第一筆
                    if (dt.AsEnumerable().Any(o => (decimal)o["BulkQty"] > request))
                    {
                        DataRow imortRow = dt.AsEnumerable().Where(o => (decimal)o["BulkQty"] > request).FirstOrDefault();
                        imortRow["Qty"] = request;
                        items.Add(imortRow);
                    }
                    else
                    {
                        // 淪落至此，表示要出的量，沒有一個項次號的物料夠，因此必須從多個項次號出

                        // 先從數量少的開始出
                        DataTable orderDt = dt.AsEnumerable().OrderByDescending(o => (decimal)o["BulkQty"]).CopyToDataTable();
                        decimal totalQty = 0;

                        // 逐個項次號出
                        foreach (DataRow dr in orderDt.Rows)
                        {
                            if ((decimal)dr["BulkQty"] + totalQty < request)
                            {
                                dr["Qty"] = dr["BulkQty"];
                                items.Add(dr);
                                totalQty += (decimal)dr["Qty"];
                            }
                            else
                            {
                                dr["Qty"] = request - totalQty;
                                items.Add(dr);
                                break;
                            }
                        }
                    }
                }
            }

            // }
            return items;
        }

        /// <summary>
        /// 檢查實際到倉日不可早於到港日
        /// </summary>
        /// <param name="arrivedPortDate">arrivedPortDate</param>
        /// <param name="arrivedWhseDate">arrivedWhseDate</param>
        /// <param name="msg">msg</param>
        /// <returns>bool</returns>
        public static bool CheckArrivedWhseDateWithArrivedPortDate(DateTime arrivedPortDate, DateTime arrivedWhseDate, out string msg)
        {
            msg = string.Empty;
            if (arrivedPortDate > arrivedWhseDate)
            {
                msg = "Arrive Warehouse date can't be earlier than arrive port date!!";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查實際到倉日若早於ETA 3天或晚於 15天都回傳訊息。
        /// </summary>
        /// <param name="eta">eta</param>
        /// <param name="arrivedWhseDate">arrivedWhseDate</param>
        /// <param name="msg">msg</param>
        /// <returns>bool</returns>
        public static bool CheckArrivedWhseDateWithEta(DateTime eta, DateTime arrivedWhseDate, out string msg)
        {
            msg = string.Empty;

            // 到倉日如果早於ETA 3天，則提示窗請USER再確認是否存檔。
            if (DateTime.Compare(eta, arrivedWhseDate.AddDays(3)) > 0)
            {
                msg = "Arrive Warehouse date is earlier than ETA 3 days, do you save it?";
                return false;
            }

            // 到倉日如果晚於ETA 15天，則提示窗請USER再確認是否存檔。
            if (DateTime.Compare(eta.AddDays(15), arrivedWhseDate) < 0)
            {
                msg = "Arrive Warehouse date is later than ETA 15 days, do you save it?";
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool P22confirm(DataRow dr, DataTable dtDetail)
        {
            StringBuilder upd_MD_8T = new StringBuilder();
            string upd_MD_0F = string.Empty;
            string upd_Fty_4T = string.Empty;
            string upd_Fty_2T = string.Empty;

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty;
            string checkmsg = string.Empty;
            DataTable datacheck;

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            DualResult result = Prgs.GetFtyInventoryData(dtDetail, "P22", out DataTable dtOriFtyInventory);

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(dr["ID"].ToString(), "P22"))
            {
                return false;
            }
            #endregion
            #region -- 檢查庫存項lock --

            bool mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
            if (!mtlAutoLock)
            {
                sqlcmd = string.Format(
                    @"
Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
    ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) 
on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
and d.FromSeq1 = f.Seq1 and d.FromSeq2 = f.seq2 and d.FromRoll = f.Roll and d.FromDyelot = f.Dyelot
where f.lock=1 
and d.Id = '{0}'", dr["id"]);
                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    MyUtility.Msg.ErrorBox(sqlcmd + result.ToString());
                    return false;
                }
                else
                {
                    if (datacheck.Rows.Count > 0)
                    {
                        foreach (DataRow tmp in datacheck.Rows)
                        {
                            checkmsg += $"SP#: {tmp["frompoid"]} Seq#: {tmp["fromseq1"]}-{tmp["fromseq2"]} Roll#: {tmp["fromroll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                        }

                        MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + checkmsg, "Warning");
                        return false;
                    }
                }
            }
            #endregion
            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(dr["id"].ToString(), "SubTransfer_Detail_From"))
            {
                return false;
            }
            #endregion
            #region 檢查From/To Location是否為空值

            // From Location
            if (MyUtility.Check.Seek(@"select * from System where WH_MtlTransChkLocation = 1"))
            {
                DataRow[] dtArry = dtDetail.Select(@"Fromlocation = '' or Fromlocation is null");
                if (dtArry != null && dtArry.Length > 0)
                {
                    DataTable dtFromLocation_Empty = dtArry.CopyToDataTable();

                    // change column name
                    dtFromLocation_Empty.Columns["FromPoId"].ColumnName = "SP#";
                    dtFromLocation_Empty.Columns["Fromseq"].ColumnName = "Seq";
                    dtFromLocation_Empty.Columns["FromRoll"].ColumnName = "Roll";
                    dtFromLocation_Empty.Columns["FromDyelot"].ColumnName = "Dyelot";

                    ChkLocationEmpty(dtFromLocation_Empty, "From", "SP#,Seq,Roll,Dyelot");
                    return false;
                }

                // To Location
                dtArry = dtDetail.Select(@"ToLocation = '' or ToLocation is null");
                if (dtArry != null && dtArry.Length > 0)
                {
                    DataTable dtToLocation_Empty = dtArry.CopyToDataTable();

                    // change column name
                    dtToLocation_Empty.Columns["ToPoid"].ColumnName = "SP#";
                    dtToLocation_Empty.Columns["toseq"].ColumnName = "Seq";
                    dtToLocation_Empty.Columns["ToRoll"].ColumnName = "Roll";
                    dtToLocation_Empty.Columns["ToDyelot"].ColumnName = "Dyelot";

                    ChkLocationEmpty(dtToLocation_Empty, "To", "SP#,Seq,Roll,Dyelot");
                    return false;
                }
            }
            #endregion
            #region -- 檢查負數庫存 --

            // 判斷轉出方
            foreach (DataRow tmp in dtOriFtyInventory.Select("Qty > 0 and BalanceQty - Qty < 0"))
            {
                checkmsg += $"SP#: {tmp["frompoid"]} Seq#: {tmp["fromseq1"]}-{tmp["fromseq2"]} Roll#: {tmp["fromroll"]} Dyelot: {tmp["fromDyelot"]}'s balance: {tmp["balanceqty"]} is less than transfer qty: {tmp["qty"]}" + Environment.NewLine;
            }

            if (!checkmsg.Empty())
            {
                MyUtility.Msg.WarningBox("Bulk balacne Qty is not enough!!" + Environment.NewLine + checkmsg);
                return false;
            }

            // 判斷轉入方 (轉負數/反向轉倉)
            foreach (DataRow tmp in dtOriFtyInventory.Select("Qty < 0 and ToBalanceQty + Qty < 0"))
            {
                checkmsg += $"SP#: {tmp["topoid"]} Seq#: {tmp["toseq1"]}-{tmp["toseq2"]} Roll#: {tmp["toroll"]} Dyelot: {tmp["ToDyelot"]}'s balance: {tmp["ToBalanceQty"]} is less than transfer qty: {tmp["qty"]}" + Environment.NewLine;
            }

            if (!checkmsg.Empty())
            {
                MyUtility.Msg.WarningBox("Inventory balacne Qty is not enough!!" + Environment.NewLine + checkmsg);
                return false;
            }
            #endregion

            // 檢查 Barcode不可為空, 轉料功能再檢查庫存後,因為轉入方可能沒有 ftyinventory 資料
            if (!Prgs.CheckBarCode(dtOriFtyInventory, "P22"))
            {
                return false;
            }

            #region -- 更新mdivisionpodetail B倉數 --
            var data_MD_8T = (from b in dtDetail.AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("frompoid"),
                                  seq1 = b.Field<string>("fromseq1"),
                                  seq2 = b.Field<string>("fromseq2"),
                                  stocktype = b.Field<string>("fromstocktype"),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("frompoid"),
                                  Seq1 = m.First().Field<string>("fromseq1"),
                                  Seq2 = m.First().Field<string>("fromseq2"),
                                  Stocktype = m.First().Field<string>("fromstocktype"),
                                  Qty = m.Sum(w => MyUtility.Convert.GetDecimal(w["qty"])),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct()),
                              }).ToList();

            var data_MD_0F = (from b in dtDetail.AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("topoid"),
                                  seq1 = b.Field<string>("toseq1"),
                                  seq2 = b.Field<string>("toseq2"),
                                  stocktype = b.Field<string>("tostocktype"),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("topoid"),
                                  Seq1 = m.First().Field<string>("toseq1"),
                                  Seq2 = m.First().Field<string>("toseq2"),
                                  Stocktype = m.First().Field<string>("tostocktype"),
                                  Qty = 0,
                                  Location = string.Empty,
                              }).ToList();
            #endregion
            #region -- 更新庫存數量 ftyinventory --
            var data_Fty_4T = (from m in dtDetail.AsEnumerable()
                               select new
                               {
                                   poid = m.Field<string>("frompoid"),
                                   seq1 = m.Field<string>("fromseq1"),
                                   seq2 = m.Field<string>("fromseq2"),
                                   stocktype = m.Field<string>("fromstocktype"),
                                   qty = MyUtility.Convert.GetDecimal(m["qty"]),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("fromroll"),
                                   dyelot = m.Field<string>("fromdyelot"),
                               }).ToList();

            var data_Fty_2T = (from b in dtDetail.AsEnumerable()
                               select new
                               {
                                   poid = b.Field<string>("topoid"),
                                   seq1 = b.Field<string>("toseq1"),
                                   seq2 = b.Field<string>("toseq2"),
                                   stocktype = b.Field<string>("tostocktype"),
                                   qty = MyUtility.Convert.GetDecimal(b["qty"]),
                                   location = b.Field<string>("ToLocation"),
                                   roll = b.Field<string>("toroll"),
                                   dyelot = b.Field<string>("todyelot"),
                               }).ToList();
            upd_Fty_4T = UpdateFtyInventory_IO(4, null, true);
            upd_Fty_2T = UpdateFtyInventory_IO(2, null, true);

            #endregion 更新庫存數量 ftyinventory

            var data_Fty_AA4T = (from m in dtDetail.AsEnumerable()
                                 select new
                                 {
                                     poid = m.Field<string>("frompoid"),
                                     seq1 = m.Field<string>("fromseq1"),
                                     seq2 = m.Field<string>("fromseq2"),
                                     roll = m.Field<string>("fromroll"),
                                     dyelot = m.Field<string>("fromdyelot"),
                                 }).ToList();

            #region 判斷是否要沿用A倉的Lock狀態
            string updateLocak = $@"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column roll varchar(15)
alter table #TmpSource alter column dyelot varchar(8)

select DISTINCT target.*
into #tmpA
from #TmpSource s
inner join FtyInventory target on 
target.poid = s.poid and target.seq1 = s.seq1 and target.seq2 = s.seq2  and target.roll = s.roll and target.dyelot = s.dyelot and target.stocktype = 'B'


--ISP20220426，A to B倉的時候要根據mtlAutoLock判斷是否要沿用A倉的Lock狀態
-- 因為在 upd_Fty_2T 目前沒有傳遞參數 mtlAutoLock，因此並不會異動到 B 倉的狀態， upd_Fty_2T 執行完後只需專注在 B 倉 Unlock + A 倉 Lock 的狀況即可
UPDATE target
SET target.Lock = 1
    , target.LockName = '{Env.User.UserID}'
    , target.LockDate = getdate()
from FtyInventory target
inner join #tmpA s on target.poid = s.poid 
                        and target.seq1 = s.seq1 
                        and target.seq2 = s.seq2 
                        and target.roll = s.roll 
                        and target.dyelot = s.dyelot 
                        and target.StockType='I'
where EXISTS(select 1 from System where mtlAutoLock =1 )
      and (target.Lock = 0 and s.Lock = 1)

DROP TABLE #TmpSource
";
            #endregion 更新庫存數量 ftyinventory

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                using (sqlConn)
                {
                    try
                    {
                        /*
                         * 先更新 FtyInventory 後更新 MDivisionPoDetail
                         * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                         * 因為要在同一 SqlConnection 之下執行
                         */
                        DataTable resulttb;
                        #region FtyInventory
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4T, string.Empty, upd_Fty_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_AA4T, string.Empty, updateLocak, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        #endregion

                        #region MDivisionPoDetail
                        upd_MD_8T.Append(UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn));
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        upd_MD_0F = UpdateMPoDetail(0, data_MD_0F, false, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_0F, string.Empty, upd_MD_0F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        #endregion

                        if (!(result = DBProxy.Current.Execute(null, $"update SubTransfer set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{dr["id"]}'")))
                        {
                            throw result.GetException();
                        }

                        // 在更新 FtyInventory 之後, 更新 SubTransfer_Detail.FromBalanceQty = (From)FtyInventory 剩餘量
                        if (!(result = UpdateSubTransfer_DetailFromBalanceQty(MyUtility.Convert.GetString(dr["id"]), true)))
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(true, dtDetail, "P22", out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = UpdateFtyInventoryTone(dtDetail)))
                        {
                            throw result.GetException();
                        }

                        transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex;
                    }
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                MyUtility.Msg.ErrorBox(errMsg.ToString());
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public static DualResult P23confirm(string subTransfer_ID, DataTable dtDetail)
        {
            string upd_MD_4T = string.Empty;
            string upd_MD_8T = string.Empty;
            string upd_MD_2T = string.Empty;
            string upd_Fty_4T = string.Empty;
            string upd_Fty_2T = string.Empty;

            string sqlcmd = string.Empty;
            string sqlupd3, ids = string.Empty;
            DualResult result = new DualResult(true);
            DataTable datacheck;

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            result = Prgs.GetFtyInventoryData(dtDetail, "P23", out DataTable dtOriFtyInventory);

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(subTransfer_ID, "P23"))
            {
                return new DualResult(false);
            }
            #endregion

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(
                @"
Select  d.frompoid
    , d.fromseq1
    , d.fromseq2
    , d.fromRoll
    , d.Qty
    , balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
    ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID 
                                        and D.FromStockType = F.StockType
                                        and d.FromRoll = f.Roll 
                                        and d.FromSeq1 =f.Seq1 
                                        and d.FromSeq2 = f.Seq2
                                        and d.fromDyelot = f.Dyelot
where   f.lock=1 
    and d.Id = '{0}'", subTransfer_ID);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                return result;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["Dyelot"]);
                    }

                    return new DualResult(false, "Material Locked!!" + Environment.NewLine + ids);
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(subTransfer_ID, "SubTransfer_Detail_From"))
            {
                return new DualResult(false, "Material WMS Locked!!");
            }
            #endregion

            #region 檢查From/To Location是否為空值

            // From Location
            if (MyUtility.Check.Seek(@"select * from System where WH_MtlTransChkLocation = 1"))
            {
                DataRow[] dtArry = dtDetail.Select(@"Fromlocation = '' or Fromlocation is null");
                if (dtArry != null && dtArry.Length > 0)
                {
                    DataTable dtFromLocation_Empty = dtArry.CopyToDataTable();

                    // change column name
                    dtFromLocation_Empty.Columns["FromPoId"].ColumnName = "Inventory SP#";
                    dtFromLocation_Empty.Columns["Fromseq"].ColumnName = "Inventory Seq";
                    dtFromLocation_Empty.Columns["FromRoll"].ColumnName = "Roll";
                    dtFromLocation_Empty.Columns["FromDyelot"].ColumnName = "Dyelot";
                    dtFromLocation_Empty.Columns["topoid"].ColumnName = "Bulk SP#";
                    dtFromLocation_Empty.Columns["toseq"].ColumnName = "Bulk Seq";

                    ChkLocationEmpty(dtFromLocation_Empty, "From", "Inventory SP#,Inventory Seq,Roll,Dyelot,Bulk SP#,Bulk Seq");
                    return new DualResult(false);
                }

                // To Location
                dtArry = dtDetail.Select(@"ToLocation = '' or ToLocation is null");
                if (dtArry != null && dtArry.Length > 0)
                {
                    DataTable dtToLocation_Empty = dtArry.CopyToDataTable();

                    // change column name
                    dtToLocation_Empty.Columns["FromPoId"].ColumnName = "Inventory SP#";
                    dtToLocation_Empty.Columns["Fromseq"].ColumnName = "Inventory Seq";
                    dtToLocation_Empty.Columns["FromRoll"].ColumnName = "Roll";
                    dtToLocation_Empty.Columns["FromDyelot"].ColumnName = "Dyelot";
                    dtToLocation_Empty.Columns["topoid"].ColumnName = "Bulk SP#";
                    dtToLocation_Empty.Columns["toseq"].ColumnName = "Bulk Seq";

                    ChkLocationEmpty(dtToLocation_Empty, "To", "Inventory SP#,Inventory Seq,Roll,Dyelot,Bulk SP#,Bulk Seq");
                    return new DualResult(false);
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select  d.frompoid
        , d.fromseq1
        , d.fromseq2
        , d.fromRoll
        , d.Qty
        , balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
        , d.FromDyelot
from (
		Select  frompoid
                , fromseq1
                , fromseq2
                , fromRoll
                , Qty = sum(Qty)
                , FromStockType
                , FromDyelot
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty > 0
		Group by frompoid, fromseq1, fromseq2, fromRoll, FromStockType,FromDyelot
	 ) as d 
left join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID 
                                          and d.FromRoll = f.Roll 
                                          and d.FromSeq1 = f.Seq1 
                                          and d.FromSeq2 = f.Seq2 
                                          and D.FromStockType = F.StockType
                                          and d.FromDyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) < d.Qty) ", subTransfer_ID);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                return result;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine,
                            tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"], tmp["FromDyelot"]);
                    }

                    return new DualResult(false, "Inventory balance Qty is not enough!!" + Environment.NewLine + ids);
                }
            }

            sqlcmd = string.Format(
                @"
Select  d.topoid
        , d.toseq1  
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty, 0)
        , d.toDyelot
from (
		Select  topoid
                , toseq1
                , toseq2
                , toRoll
                , Qty = sum(Qty)
                , toStocktype
                , toDyelot
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty < 0
		Group by topoid, toseq1, toseq2, toRoll, toStocktype, toDyelot
	 ) as d
left join FtyInventory f WITH (NOLOCK) on   d.toPoId = f.PoId 
                                            and d.toSeq1 = f.Seq1 
                                            and d.toSeq2 = f.seq2 
                                            and d.toStocktype = f.StockType 
                                            and d.toRoll = f.Roll
                                            and d.toDyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) + d.Qty < 0) ", subTransfer_ID);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                return result;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Roll#: {3}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine,
                            tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["balanceqty"], tmp["qty"], tmp["toDyelot"]);
                    }

                    return new DualResult(false, "Bulk balacne Qty is not enough!!" + Environment.NewLine + ids);
                }
            }

            #endregion -- 檢查負數庫存 --

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, "P23"))
            {
                return new DualResult(false);
            }

            #region -- 更新表頭狀態資料 --
            sqlupd3 = string.Format(
                @"
update SubTransfer 
set status = 'Confirmed'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, subTransfer_ID);
            #endregion 更新表頭狀態資料

            DataTable dtSubTransfer_Detail;
            string sqlSubTransfer_Detail = $@"
select  a.id    
        , a.FromPoId
        , a.FromSeq1
        , a.FromSeq2
        , Fromseq = concat (Ltrim (Rtrim (a.FromSeq1)), ' ', a.FromSeq2)
        , p1.FabricType
        , stockunit = dbo.GetStockUnitBySPSeq (p1.ID, p1.seq1, p1.seq2)
        , [description] = dbo.getmtldesc (a.FromPoId, a.FromSeq1, a.FromSeq2, 2, 0)
        , a.FromRoll
        , a.FromDyelot
        , a.FromStocktype
        , a.Qty
        , a.ToPoid
        , a.ToSeq1
        , a.ToSeq2
        , toseq = concat (Ltrim (Rtrim (a.ToSeq1)), ' ', a.ToSeq2)
        , a.ToRoll
        , a.ToDyelot
        , a.ToStocktype
        , a.ToLocation
        , a.fromftyinventoryukey
        , a.ukey
        , location = dbo.Getlocation (fi.ukey)
from dbo.SubTransfer_detail a
left join PO_Supp_Detail p1 on p1.ID = a.FromPoId 
                                             and p1.seq1 = a.FromSeq1 
                                             and p1.SEQ2 = a.FromSeq2
left join FtyInventory FI on a.FromPoid = fi.poid 
                             and a.fromSeq1 = fi.seq1 
                             and a.fromSeq2 = fi.seq2
                             and a.fromRoll = fi.roll 
                             and a.fromStocktype = fi.stocktype
                             and a.fromDyelot = fi.Dyelot
Where a.id = '{subTransfer_ID}'";
            result = DBProxy.Current.Select(null, sqlSubTransfer_Detail, out dtSubTransfer_Detail);
            if (!result)
            {
                return result;
            }

            #region -- 更新mdivisionpodetail B倉數 --
            var data_MD_4T = (from b in dtSubTransfer_Detail.AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("frompoid"),
                                  seq1 = b.Field<string>("fromseq1"),
                                  seq2 = b.Field<string>("fromseq2"),
                                  stocktype = b.Field<string>("fromstocktype"),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("frompoid"),
                                  Seq1 = m.First().Field<string>("fromseq1"),
                                  Seq2 = m.First().Field<string>("fromseq2"),
                                  Stocktype = m.First().Field<string>("fromstocktype"),
                                  Qty = m.Sum(w => w.Field<decimal>("qty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct()),
                              }).ToList();

            var data_MD_8T = data_MD_4T.Select(data => new Prgs_POSuppDetailData
            {
                Poid = data.Poid,
                Seq1 = data.Seq1,
                Seq2 = data.Seq2,
                Stocktype = data.Stocktype,
                Qty = -data.Qty,
            }).ToList();

            #endregion

            #region -- 更新mdivisionpodetail A倉數 --
            var data_MD_2T = (from b in dtSubTransfer_Detail.AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("Topoid"),
                                  seq1 = b.Field<string>("Toseq1"),
                                  seq2 = b.Field<string>("Toseq2"),
                                  stocktype = b.Field<string>("Tostocktype"),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("Topoid"),
                                  Seq1 = m.First().Field<string>("Toseq1"),
                                  Seq2 = m.First().Field<string>("Toseq2"),
                                  Stocktype = m.First().Field<string>("Tostocktype"),
                                  Qty = m.Sum(w => w.Field<decimal>("qty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct()),
                              }).ToList();

            #endregion

            #region -- 更新庫存數量 ftyinventory --
            var data_Fty_4T = (from m in dtSubTransfer_Detail.AsEnumerable()
                               group m by new
                               {
                                   poid = m.Field<string>("frompoid"),
                                   seq1 = m.Field<string>("fromseq1"),
                                   seq2 = m.Field<string>("fromseq2"),
                                   stocktype = m.Field<string>("fromstocktype"),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("fromroll"),
                                   dyelot = m.Field<string>("fromdyelot"),
                               }
                                into g
                               select new
                               {
                                   poid = g.Key.poid,
                                   seq1 = g.Key.seq1,
                                   seq2 = g.Key.seq2,
                                   stocktype = g.Key.stocktype,
                                   qty = g.Sum(m => m.Field<decimal>("qty")),
                                   location = g.Key.location,
                                   roll = g.Key.roll,
                                   dyelot = g.Key.dyelot,
                               }).ToList();

            var data_Fty_2T = (from b in dtSubTransfer_Detail.AsEnumerable()
                               select new
                               {
                                   poid = b.Field<string>("topoid"),
                                   seq1 = b.Field<string>("toseq1"),
                                   seq2 = b.Field<string>("toseq2"),
                                   stocktype = b.Field<string>("tostocktype"),
                                   qty = b.Field<decimal>("qty"),
                                   location = b.Field<string>("ToLocation"),
                                   roll = b.Field<string>("toroll"),
                                   dyelot = b.Field<string>("todyelot"),
                               }).ToList();
            upd_Fty_4T = UpdateFtyInventory_IO(4, null, true);
            upd_Fty_2T = UpdateFtyInventory_IO(2, null, true);
            #endregion 更新庫存數量 ftyinventory

            #region 更新 Po_Supp_Detail StockUnit
            string sql_UpdatePO_Supp_Detail = @";
alter table #Tmp alter column ToPoid varchar(20)
alter table #Tmp alter column ToSeq1 varchar(3)
alter table #Tmp alter column ToSeq2 varchar(3)
alter table #Tmp alter column StockUnit varchar(20)

select  distinct ToPoid
        , ToSeq1
        , ToSeq2
        , StockUnit 
into #tmpD 
from #Tmp

update target
	set target.StockUnit = src.StockUnit
from dbo.PO_Supp_Detail as target
inner join #tmpD as src on   target.ID = src.ToPoid 
                        and target.seq1 = src.ToSeq1 
                        and target.seq2 = src.ToSeq2 ;
";
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                using (sqlConn)
                {
                    try
                    {
                        /*
                         * 先更新 FtyInventory 後更新 MDivisionPoDetail
                         * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                         * 因為要在同一 SqlConnection 之下執行
                         */
                        DataTable resulttb;
                        #region FtyInventory
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4T, string.Empty, upd_Fty_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            return result;
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            transactionscope.Dispose();
                            return result;
                        }
                        #endregion

                        #region MDivisionPoDetail
                        upd_MD_4T = UpdateMPoDetail(4, null, true, sqlConn: sqlConn);
                        upd_MD_8T = UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);
                        upd_MD_2T = UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T, string.Empty, upd_MD_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, string.Empty, upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }
                        #endregion

                        if (!(result = MyUtility.Tool.ProcessWithDatatable(
                            dtSubTransfer_Detail, string.Empty, sql_UpdatePO_Supp_Detail, out resulttb, "#tmp", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                        {
                            throw result.GetException();
                        }

                        // 在更新 FtyInventory 之後, 更新 SubTransfer_Detail.FromBalanceQty = (From)FtyInventory 剩餘量
                        if (!(result = UpdateSubTransfer_DetailFromBalanceQty(subTransfer_ID, true)))
                        {
                            throw result.GetException();
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(true, dtDetail, "P23", out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = UpdateFtyInventoryTone(dtDetail)))
                        {
                            throw result.GetException();
                        }

                        transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex;
                    }
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                return Result.F(errMsg);
            }

            return result;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public static DualResult P24confirm(string subTransfer_ID, DataTable dtDetail)
        {
            string upd_MD_16T = string.Empty;
            string upd_MD_8T = string.Empty;
            string upd_MD_4T = string.Empty;
            string upd_MD_0F = string.Empty;
            string upd_Fty_4T = string.Empty;
            string upd_Fty_2T = string.Empty;

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            result = Prgs.GetFtyInventoryData(dtDetail, "P24", out DataTable dtOriFtyInventory);

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, "P24"))
            {
                return new DualResult(false);
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(subTransfer_ID, "P24"))
            {
                return new DualResult(false);
            }
            #endregion

            #region -- 檢查庫存項lock --
            bool mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
            if (!mtlAutoLock)
            {
                sqlcmd = string.Format(
                    @"
Select 	d.frompoid
		,d.fromseq1
		,d.fromseq2
		,d.fromRoll
		,d.Qty
		,balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
        ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) 
    on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
       and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 and d.FromDyelot = f.Dyelot
where f.lock=1 
and d.Id = '{0}'", subTransfer_ID);
                if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    return result2;
                }
                else
                {
                    if (datacheck.Rows.Count > 0)
                    {
                        foreach (DataRow tmp in datacheck.Rows)
                        {
                            ids += string.Format(
                                "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                                tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["Dyelot"]);
                        }

                        return new DualResult(false, "Material Locked!!" + Environment.NewLine + ids);
                    }
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(subTransfer_ID, "SubTransfer_Detail_From"))
            {
                return new DualResult(false, "Material WMS Locked!!");
            }
            #endregion

            #region 檢查From/To Location是否為空值

            // From Location
            if (MyUtility.Check.Seek(@"select * from System where WH_MtlTransChkLocation = 1"))
            {
                DataRow[] dtArry = dtDetail.Select(@"Fromlocation = '' or Fromlocation is null");
                if (dtArry != null && dtArry.Length > 0)
                {
                    DataTable dtFromLocation_Empty = dtArry.CopyToDataTable();

                    // change column name
                    dtFromLocation_Empty.Columns["FromPoId"].ColumnName = "SP#";
                    dtFromLocation_Empty.Columns["Fromseq"].ColumnName = "Seq";
                    dtFromLocation_Empty.Columns["FromRoll"].ColumnName = "Roll";
                    dtFromLocation_Empty.Columns["FromDyelot"].ColumnName = "Dyelot";

                    ChkLocationEmpty(dtFromLocation_Empty, "From", "SP#,Seq,Roll,Dyelot");
                    return new DualResult(false);
                }

                // To Location
                dtArry = dtDetail.Select(@"ToLocation = '' or ToLocation is null");
                if (dtArry != null && dtArry.Length > 0)
                {
                    DataTable dtToLocation_Empty = dtArry.CopyToDataTable();

                    // change column name
                    dtToLocation_Empty.Columns["ToPoid"].ColumnName = "SP#";
                    dtToLocation_Empty.Columns["toseq"].ColumnName = "Seq";
                    dtToLocation_Empty.Columns["ToRoll"].ColumnName = "Roll";
                    dtToLocation_Empty.Columns["ToDyelot"].ColumnName = "Dyelot";

                    ChkLocationEmpty(dtToLocation_Empty, "To", "SP#,Seq,Roll,Dyelot");
                    return new DualResult(false);
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select 	d.frompoid
		,d.fromseq1
		,d.fromseq2
		,d.fromRoll
		,d.Qty
		,balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
        , f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) 
	on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
	   and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 and d.FromDyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) and d.Id = '{0}'", subTransfer_ID);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                return result2;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than qty: {5}" + Environment.NewLine,
                            tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    return new DualResult(false, "Balacne Qty is not enough!!" + Environment.NewLine + ids);
                }
            }

            #endregion -- 檢查負數庫存 --

            #region -- 更新表頭狀態資料 --

            sqlupd3 = $@"update SubTransfer set status='Confirmed', editname = '{Env.User.UserID}' , editdate = GETDATE()
                                where id = '{subTransfer_ID}'";

            #endregion 更新表頭狀態資料

            // 撈出明細資料
            DataTable dtSubTransfer_Detail;
            string sqlSubTransfer_Detail = $@"
select 
    a.id
    ,a.FromFtyinventoryUkey
    ,[FromPoId] = RTRIM(LTRIM(a.FromPoId))
    ,[FromSeq1] = RTRIM(LTRIM(a.FromSeq1))
    ,[FromSeq2] = RTRIM(LTRIM(a.FromSeq2))
    ,FromSeq = concat(Ltrim(Rtrim(a.FromSeq1)), ' ', RTRIM(LTRIM(a.FromSeq2)))
    ,FabricType = Case p1.FabricType WHEN 'F' THEN 'Fabric' WHEN 'A' THEN 'Accessory' ELSE 'Other'  END 
    ,[stockunit] = RTRIM(LTRIM(p1.stockunit))
    ,description = dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0)
    ,[FromRoll] = RTRIM(LTRIM(a.FromRoll))
    ,[FromDyelot] = RTRIM(LTRIM(a.FromDyelot))
    ,[FromStockType] = RTRIM(LTRIM(a.FromStockType))
    ,a.Qty
    ,[ToPoId] = RTRIM(LTRIM(a.ToPoId))
    ,[ToSeq1] = RTRIM(LTRIM(a.ToSeq1))
    ,[ToSeq2] = RTRIM(LTRIM(a.ToSeq2))
    ,[ToDyelot] = RTRIM(LTRIM(a.ToDyelot))
    ,[ToRoll] = RTRIM(LTRIM(a.ToRoll))
    ,[ToStockType] = RTRIM(LTRIM(a.ToStockType))
    ,dbo.Getlocation(f.Ukey)  as Fromlocation
    ,a.ukey
    ,a.tolocation
from dbo.SubTransfer_Detail a 
left join PO_Supp_Detail p1 on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 and p1.SEQ2 = a.FromSeq2
left join FtyInventory f on a.FromPOID=f.POID and a.FromSeq1=f.Seq1 and a.FromSeq2=f.Seq2 and a.FromRoll=f.Roll and a.FromDyelot=f.Dyelot and a.FromStockType=f.StockType
Where a.id = '{subTransfer_ID}'";

            result = DBProxy.Current.Select(null, sqlSubTransfer_Detail, out dtSubTransfer_Detail);
            if (!result)
            {
                return result;
            }

            #region -- 更新mdivisionpodetail Inventory 數 --
            var data_MD_4T = (from b in dtSubTransfer_Detail.AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("frompoid"),
                                  seq1 = b.Field<string>("fromseq1"),
                                  seq2 = b.Field<string>("fromseq2"),
                                  stocktype = b.Field<string>("fromstocktype"),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("frompoid"),
                                  Seq1 = m.First().Field<string>("fromseq1"),
                                  Seq2 = m.First().Field<string>("fromseq2"),
                                  Stocktype = m.First().Field<string>("fromstocktype"),
                                  Qty = m.Sum(w => w.Field<decimal>("qty")),
                              }).ToList();

            var data_MD_8T = data_MD_4T.Select(data => new Prgs_POSuppDetailData
            {
                Poid = data.Poid,
                Seq1 = data.Seq1,
                Seq2 = data.Seq2,
                Stocktype = data.Stocktype,
                Qty = -data.Qty,
            }).ToList();

            var data_MD_0F = data_MD_4T.Select(data => new Prgs_POSuppDetailData
            {
                Poid = data.Poid,
                Seq1 = data.Seq1,
                Seq2 = data.Seq2,
                Stocktype = data.Stocktype,
                Qty = 0,
            }).ToList();

            #endregion
            #region -- 更新mdivisionpodetail Scrap數 --
            var data_MD_16T = (from b in dtSubTransfer_Detail.AsEnumerable()
                               group b by new
                               {
                                   poid = b.Field<string>("topoid"),
                                   seq1 = b.Field<string>("toseq1"),
                                   seq2 = b.Field<string>("toseq2"),
                                   stocktype = b.Field<string>("tostocktype"),
                               }
                                into m
                               select new
                               {
                                   poid = m.First().Field<string>("topoid"),
                                   Seq1 = m.First().Field<string>("toseq1"),
                                   Seq2 = m.First().Field<string>("toseq2"),
                                   Stocktype = m.First().Field<string>("tostocktype"),
                                   Qty = m.Sum(w => w.Field<decimal>("qty")),
                               }).ToList();

            #endregion
            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_4T = (from m in dtSubTransfer_Detail.AsEnumerable()
                               select new
                               {
                                   poid = m.Field<string>("frompoid"),
                                   seq1 = m.Field<string>("fromseq1"),
                                   seq2 = m.Field<string>("fromseq2"),
                                   stocktype = m.Field<string>("fromstocktype"),
                                   qty = m.Field<decimal>("qty"),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("fromroll"),
                                   dyelot = m.Field<string>("fromdyelot"),
                               }).ToList();

            var data_Fty_2T = (from m in dtSubTransfer_Detail.AsEnumerable()
                               select new
                               {
                                   poid = m.Field<string>("topoid"),
                                   seq1 = m.Field<string>("toseq1"),
                                   seq2 = m.Field<string>("toseq2"),
                                   stocktype = m.Field<string>("tostocktype"),
                                   qty = m.Field<decimal>("qty"),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("toroll"),
                                   dyelot = m.Field<string>("todyelot"),
                               }).ToList();
            upd_Fty_4T = UpdateFtyInventory_IO(4, null, true);
            upd_Fty_2T = UpdateFtyInventory_IO(2, null, true);
            #endregion 更新庫存數量  ftyinventory
            #region ISP20191578 ToLocation的資料一併更新回MDivisionPODetail.CLocation欄位
            string updateMDivisionPODetailCLocation = string.Empty;
            var listMDivisionPODetailCLocation = dtSubTransfer_Detail.AsEnumerable()
                                                .Where(s => s["ToLocation"].ToString() != string.Empty)
                                                .Select(s => new
                                                {
                                                    FromPOID = s["FromPOID"].ToString(),
                                                    FromSeq1 = s["FromSeq1"].ToString(),
                                                    FromSeq2 = s["FromSeq2"].ToString(),
                                                    ToLocation = s["ToLocation"].ToString(),
                                                });

            var listDistinctPoIdSeq = listMDivisionPODetailCLocation
                .Select(s => new
                {
                    s.FromPOID,
                    s.FromSeq1,
                    s.FromSeq2,
                })
            .Distinct();

            foreach (var updItem in listDistinctPoIdSeq)
            {
                string pOID = updItem.FromPOID;
                string seq11 = updItem.FromSeq1;
                string seq21 = updItem.FromSeq2;

                List<string> new_CLocationList = listMDivisionPODetailCLocation
                            .Where(o => o.FromPOID == pOID &&
                                         o.FromSeq1 == seq11 &&
                                         o.FromSeq2 == seq21)
                            .Select(o => o.ToLocation)
                            .Distinct().ToList();

                // 從MDivisionPoDetail出現有的Location
                DataTable dT_MDivisionPoDetail;
                string cmdText = $@"
SELECt CLocation
FROM MDivisionPoDetail
WHERE POID='{pOID}'
AND Seq1='{seq11}' AND Seq2='{seq21}'
";
                DBProxy.Current.Select(null, cmdText, out dT_MDivisionPoDetail);

                List<string> dB_CLocations = dT_MDivisionPoDetail.Rows[0]["CLocation"].ToString().Split(',').Where(o => o != string.Empty).ToList();

                List<string> fincal = new List<string>();

                foreach (var new_CLocation in new_CLocationList)
                {
                    if (dB_CLocations.Count == 0 || !dB_CLocations.Contains(new_CLocation))
                    {
                        dB_CLocations.Add(new_CLocation);
                    }
                }

                foreach (var cLocation in dB_CLocations.Distinct().ToList())
                {
                    foreach (var a in cLocation.Split(',').Where(o => o != string.Empty).Distinct().ToList())
                    {
                        if (!fincal.Contains(a))
                        {
                            fincal.Add(a);
                        }
                    }
                }

                string cmd = $@"
UPDATE MDivisionPoDetail
SET CLocation='{fincal.Distinct().ToList().JoinToString(",")}'
WHERE POID='{pOID}' AND Seq1='{seq11}' AND Seq2='{seq21}'

";
                updateMDivisionPODetailCLocation += cmd;
            }
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                using (sqlConn)
                {
                    try
                    {
                        /*
                         * 先更新 FtyInventory 後更新 MDivisionPoDetail
                         * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                         * 因為要在同一 SqlConnection 之下執行
                         */
                        DataTable resulttb;
                        #region FtyInventory
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4T, string.Empty, upd_Fty_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }
                        #endregion

                        #region MDivisionPoDetail
                        upd_MD_4T = UpdateMPoDetail(4, data_MD_4T, true, sqlConn: sqlConn);
                        upd_MD_8T = UpdateMPoDetail(8, null, true, sqlConn: sqlConn);
                        upd_MD_16T = UpdateMPoDetail(16, null, true, sqlConn: sqlConn);
                        upd_MD_0F = UpdateMPoDetail(0, data_MD_0F, true, sqlConn: sqlConn);

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, string.Empty, upd_MD_8T.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T, string.Empty, upd_MD_4T.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_16T, string.Empty, upd_MD_16T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_0F, string.Empty, upd_MD_0F, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw result.GetException();
                        }
                        #endregion

                        if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                        {
                            throw result.GetException();
                        }

                        if (!MyUtility.Check.Empty(updateMDivisionPODetailCLocation))
                        {
                            result = DBProxy.Current.Execute(null, updateMDivisionPODetailCLocation);
                            if (!result)
                            {
                                throw result.GetException();
                            }
                        }

                        // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                        if (!(result = Prgs.UpdateWH_Barcode(true, dtDetail, "P24", out bool fromNewBarcode, dtOriFtyInventory)))
                        {
                            throw result.GetException();
                        }

                        if (!(result = UpdateFtyInventoryTone(dtDetail)))
                        {
                            throw result.GetException();
                        }

                        transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex;
                    }
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                return Result.F(errMsg);
            }

            return result;
        }

        /// <inheritdoc/>
        public static string GetNextValue(string strValue, int sequenceMode)
        {
            char[] charValue = strValue.ToArray();
            int sequenceValue = 0;
            string returnValue = string.Empty;
            int charAscii = 0;

            if (sequenceMode == 1)
            {
                // 當第一個字為字母
                if (Convert.ToInt32(charValue[0]) >= 65 && Convert.ToInt32(charValue[0]) <= 90)
                {
                    sequenceValue = Convert.ToInt32(strValue.Substring(1));

                    // 進位處理
                    if (((sequenceValue + 1).ToString().Length > sequenceValue.ToString().Length) && string.IsNullOrWhiteSpace(strValue.Substring(1).Replace("9", string.Empty)))
                    {
                        charAscii = Convert.ToInt32(charValue[0]);
                        if (charAscii + 1 > 90)
                        {
                            return strValue;
                        }
                        else
                        {
                            sequenceValue = 1;
                            if (charAscii == 72 || charAscii == 78)
                            {
                                // I or O略過
                                charValue[0] = Convert.ToChar(charAscii + 2);
                            }
                            else
                            {
                                charValue[0] = Convert.ToChar(charAscii + 1);
                            }
                        }
                    }
                    else
                    {
                        sequenceValue = sequenceValue + 1;
                    }

                    returnValue = charValue[0] + sequenceValue.ToString().PadLeft(strValue.Length - 1, '0');
                }
                else
                {
                    sequenceValue = Convert.ToInt32(strValue);

                    // 進位處理
                    if (((sequenceValue + 1).ToString().Length > sequenceValue.ToString().Length) && string.IsNullOrWhiteSpace(strValue.Replace("9", string.Empty)))
                    {
                        sequenceValue = 1;
                        charValue[0] = 'A';
                        returnValue = charValue[0] + sequenceValue.ToString().PadLeft(strValue.Length - 1, '0');
                    }
                    else
                    {
                        sequenceValue = sequenceValue + 1;
                        returnValue = sequenceValue.ToString().PadLeft(strValue.Length, '0');
                    }
                }
            }
            else
            {
                for (int i = charValue.Length - 1; i >= 0; i--)
                {
                    charAscii = Convert.ToInt32(charValue[i]);

                    if (charAscii == 57)
                    { // 遇9跳A
                        charValue[i] = 'A';
                        break;
                    }

                    if (charAscii == 72 || charAscii == 78)
                    {
                        // I or O略過
                        charValue[i] = Convert.ToChar(charAscii + 2);
                        break;
                    }

                    if (charAscii == 90)
                    {
                        // 當字母為Z
                        if (i > 0)
                        {
                            charValue[i] = '0';
                            continue;
                        }
                        else
                        {
                            return strValue;    // 超出最大上限ZZZ...., 返回原值
                        }
                    }

                    charValue[i] = Convert.ToChar(charAscii + 1);
                    break;
                }

                returnValue = new string(charValue);
            }

            return returnValue;
        }

        /// <inheritdoc/>
        public static bool ChkFtyInventory(string sp, string seq1, string seq2, string roll, string dyelot, string stockType)
        {
            string cmd = $"select COUNT(Ukey) from FtyInventory where POID='{sp}' AND SEQ1='{seq1}' AND SEQ2='{seq2}' AND Roll='{roll}' AND Dyelot='{dyelot}' AND StockType = '{stockType}' AND InQty > 0";

            int ftyCount = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(cmd));

            if (ftyCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// ISP20211385 & ISP20211566
        /// </summary>
        /// <param name="dt">dt</param>
        /// <param name="type">type</param>
        /// <param name="columns">columns</param>
        public static void ChkLocationEmpty(DataTable dt, string type, string columns)
        {
            /* ISP20211385
                清單內容會根據不同的功能顯示不同的欄位資訊
                1. P22, P28, P24, P30, P36
                    > SP#, Seq, Roll, Dyelot
                2. P23, P29
                    > Inventory SP#, Inventory Seq, Roll, Dyelot, Bulk SP#, Bulk Seq
                3. 借還料 : P31, P32
                    > From SP#, From Seq, From Roll, From Dyelot, From Stock Type, To SP#, To Seq, To Roll, To Dyelot
                ISP20211566
                1. 顯示不符合規定的物料清單
                    > SP#, Seq, Roll, Dyelot, Stock Type
                【提示訊息】
                08, 17, 26
                > Location cannot be empty, please update location.
                其餘功能
                > Location cannot be empty, please update material current location as below list in WH P26 first.
            */

            string msg = string.Empty;
            switch (type)
            {
                case "From":
                    msg = @"From location cannot be empty, please update material current location as below list in WH P26 first.";
                    break;
                case "To":
                    msg = @"To Location cannot be empty, please update to location.";
                    break;
                case "Other":
                    msg = @"Location cannot be empty, please update location.";
                    break;
                case "LocalOrder":
                    msg = @"Location cannot be empty, please update material current location as below list in WH P73 first.";
                    break;
                default:
                    msg = @"Location cannot be empty, please update material current location as below list in WH P26 first.";
                    break;
            }

            var from = MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Result", shownColumns: columns);
            from.Width = 800;
            from.grid1.Columns[0].Width = 150;
            from.Visible = false;
            from.ShowDialog();
        }

        /// <inheritdoc/>
        public static bool ChkLocation(string transcationID, string gridAlias, string msgType = "", bool isLocalOrder = false)
        {
            // 檢查Location是否為空值
            DualResult result;
            string sqlLocation = string.Empty;
            string strTable = isLocalOrder ? "LocalOrderInventory" : "FtyInventory";
            string strGetLocation = isLocalOrder ? @"(	
select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.LocalOrderInventory_Location d
						where d.LocalOrderInventoryUkey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, ''))" : "dbo.Getlocation(f.ukey) "
            ;
            switch (gridAlias)
            {
                case "Receiving_Detail":
                case "IssueReturn_Detail":
                case "LocalOrderReceiving_Detail":
                    sqlLocation = $@"
 select td.POID,seq = concat(Ltrim(Rtrim(td.seq1)), ' ', td.Seq2),td.Roll,td.Dyelot
 , StockType = case td.StockType 
		when 'B' then 'Bulk' 
		when 'I' then 'Inventory' 
		when 'O' then 'Scrap' 
		else td.StockType 
		end
 , [Location] = td.Location
 from {gridAlias} td
 left join Production.dbo.{strTable} f on f.POID = td.POID 
	and f.Seq1=td.Seq1 and f.Seq2=td.Seq2 
	and f.Roll=td.Roll and f.Dyelot=td.Dyelot
    and f.StockType = td.StockType
where td.ID = '{transcationID}'
";
                    break;
                case "Issue_Detail":
                case "IssueLack_Detail":
                case "ReturnReceipt_Detail":
                case "TransferOut_Detail":
                case "Adjust_Detail":
                case "StockTaking_detail":
                case "LocalOrderIssue_Detail":
                case "LocalOrderAdjust_Detail":
                    sqlLocation = $@"
 select td.POID,seq = concat(Ltrim(Rtrim(td.seq1)), ' ', td.Seq2),td.Roll,td.Dyelot
 , StockType = case td.StockType 
		when 'B' then 'Bulk' 
		when 'I' then 'Inventory' 
		when 'O' then 'Scrap' 
		else td.StockType 
		end
 , [Location] = {strGetLocation}
 from {gridAlias} td
 left join Production.dbo.{strTable} f on f.POID = td.POID 
	and f.Seq1=td.Seq1 and f.Seq2=td.Seq2 
	and f.Roll=td.Roll and f.Dyelot=td.Dyelot
    and f.StockType = td.StockType
where td.ID = '{transcationID}'
";
                    break;
                default:
                    break;
            }

            if (!(result = DBProxy.Current.Select(string.Empty, sqlLocation, out DataTable dtLocationDetail)))
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }

            if (MyUtility.Check.Seek(@"select 1 from System where WH_MtlTransChkLocation = 1"))
            {
                if (dtLocationDetail != null && dtLocationDetail.Rows.Count > 0)
                {
                    // Location
                    DataRow[] dtArry = dtLocationDetail.Select(@"Location = '' or Location is null");
                    if (dtArry != null && dtArry.Length > 0)
                    {
                        DataTable dtLocation_Empty = dtArry.CopyToDataTable();

                        // change column name
                        dtLocation_Empty.Columns["PoId"].ColumnName = "SP#";
                        dtLocation_Empty.Columns["seq"].ColumnName = "Seq";
                        dtLocation_Empty.Columns["Roll"].ColumnName = "Roll";
                        dtLocation_Empty.Columns["Dyelot"].ColumnName = "Dyelot";
                        dtLocation_Empty.Columns["StockType"].ColumnName = "Stock Type";
                        ChkLocationEmpty(dtLocation_Empty, msgType, @"SP#,Seq,Roll,Dyelot,Stock Type");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public static List<string> GetBarcodeNo_WH(string keyWord, int batchNumber = 1, int dateType = 3, string connectionName = null, int sequenceMode = 1, int sequenceLength = 0, DataTable dtBarcodeSource = null, AbstractDBProxyPMS proxyPMS = null)
        {
            string sqlGetRgCode = "select RgCode from system";
            string localRgcode = proxyPMS == null ? MyUtility.GetValue.Lookup(sqlGetRgCode) : proxyPMS.Lookup(sqlGetRgCode, "Production");
            List<string> iDList = new List<string>();
            DateTime today = DateTime.Today;
            string taiwanYear;

            keyWord = localRgcode + keyWord;
            switch (dateType)
            {
                case 1: // A yy xxxx
                    keyWord = keyWord.ToUpper().Trim() + today.ToString("yy");
                    break;
                case 2: // A yyMM xxxx
                    keyWord = keyWord.ToUpper().Trim() + today.ToString("yyMM");
                    break;
                case 3: // A yyMMdd xxxx
                    keyWord = keyWord.ToUpper().Trim() + today.ToString("yyMMdd");
                    break;
                case 4: // A yyyyMM xxxxx
                    keyWord = keyWord.ToUpper().Trim() + today.ToString("yyyyMM");
                    break;
                case 5: // 民國年 A yyyMM xxxx
                    taiwanYear = (today.Year - 1911).ToString().PadLeft(3, '0');
                    keyWord = keyWord.ToUpper().Trim() + taiwanYear + today.ToString("MM");
                    break;
                case 6: // A xxxx
                    keyWord = keyWord.ToUpper().Trim();
                    break;
                case 7: // A yyyy xxxx
                    keyWord = keyWord.ToUpper().Trim() + today.ToString("yyyy");
                    break;
                case 8: // 民國年 A yyyMMdd xxxx
                    taiwanYear = (today.Year - 1911).ToString().PadLeft(3, '0');
                    keyWord = keyWord.ToUpper().Trim() + taiwanYear + today.ToString("MM") + today.ToString("dd");
                    break;
                default:
                    return iDList;
            }

            string sqlCmd = $@"
select top 1 Barcode
from(
	select Barcode
	from FtyInventory
	where Barcode like '{keyWord}%'
	and len(Barcode) = 16

	union all
	select Barcode
	from FtyInventory_Barcode
	where Barcode like '{keyWord}%'
	and Barcode not like '%-%'
	and len(Barcode) = 16
    
	union all
	select From_OldBarcode
	from WHBarcodeTransaction 
	where From_OldBarcode<>''
	and From_OldBarcode not like '%-%'
	and From_OldBarcode like '{keyWord}%'
	and len(From_OldBarcode) = 16

	union all
	select To_OldBarcode
	from WHBarcodeTransaction 
	where To_OldBarcode<>''
	and To_OldBarcode not like '%-%'
	and To_OldBarcode like '{keyWord}%'
	and len(To_OldBarcode) = 16

	union all
	select From_NewBarcode
	from WHBarcodeTransaction 
	where From_NewBarcode<>''
	and From_NewBarcode not like '%-%'
	and From_NewBarcode like '{keyWord}%'
	and len(From_NewBarcode) = 16

	union all
	select To_NewBarcode
	from WHBarcodeTransaction 
	where To_NewBarcode<>''
	and To_NewBarcode not like '%-%'
	and To_NewBarcode like '{keyWord}%'
	and len(To_NewBarcode) = 16
)x
order by Barcode desc
";

            // 固定最大長度13, 雖然結構是開16, 但長度要留3
            // ISP20221525 一律16碼
            int columnTypeLength = 16;
            List<string> userInputBarcodes = new List<string>();

            if (dtBarcodeSource != null && dtBarcodeSource.Columns.Contains("Barcode") && dtBarcodeSource.Rows.Count > 0)
            {
                userInputBarcodes = dtBarcodeSource.AsEnumerable().Select(s => s["Barcode"].ToString()).ToList();
            }

            DualResult result = null;
            DataTable dtID = null;
            if (result = proxyPMS == null ? DBProxy.Current.Select(connectionName, sqlCmd, out dtID) : proxyPMS.Select("Production", sqlCmd, out dtID))
            {
                if (dtID.Rows.Count > 0 && !MyUtility.Check.Empty(dtID.Rows[0]["Barcode"]))
                {
                    string lastID = dtID.Rows[0]["Barcode"].ToString();
                    while (batchNumber > 0)
                    {
                        lastID = keyWord + GetNextValue(lastID.Substring(keyWord.Length), sequenceMode);

                        // 如果產生Barcode與user輸入的重複，就跳號繼續產生
                        if (userInputBarcodes.Any(s => s == lastID))
                        {
                            continue;
                        }

                        iDList.Add(lastID);
                        batchNumber = batchNumber - 1;
                    }
                }
                else
                {
                    if (sequenceLength > 0)
                    {
                        if ((columnTypeLength - keyWord.Length) >= sequenceLength)
                        {
                            string nextValue = GetNextValue("0".PadLeft(sequenceLength, '0'), sequenceMode);
                            string lastID = keyWord + nextValue;
                            while (batchNumber > 0)
                            {
                                // 如果產生Barcode與user輸入的重複，就跳號繼續產生
                                if (userInputBarcodes.Any(s => s == lastID))
                                {
                                    nextValue = GetNextValue(nextValue.PadLeft(sequenceLength, '0'), sequenceMode);
                                    lastID = keyWord + nextValue;
                                    continue;
                                }

                                iDList.Add(lastID);
                                batchNumber = batchNumber - 1;
                                nextValue = GetNextValue(nextValue.PadLeft(sequenceLength, '0'), sequenceMode);
                                lastID = keyWord + nextValue;
                            }
                        }
                    }
                    else
                    {
                        string nextValue = GetNextValue("0".PadLeft(columnTypeLength - keyWord.Length, '0'), sequenceMode);
                        string lastID = keyWord + nextValue;
                        while (batchNumber > 0)
                        {
                            // 如果產生Barcode與user輸入的重複，就跳號繼續產生
                            if (userInputBarcodes.Any(s => s == lastID))
                            {
                                nextValue = GetNextValue(nextValue.PadLeft(columnTypeLength - keyWord.Length, '0'), sequenceMode);
                                lastID = keyWord + nextValue;
                                continue;
                            }

                            iDList.Add(lastID);
                            batchNumber = batchNumber - 1;
                            nextValue = GetNextValue(nextValue.PadLeft(columnTypeLength - keyWord.Length, '0'), sequenceMode);
                            lastID = keyWord + nextValue;
                        }
                    }
                }
            }
            else
            {
                throw new Exception(result.ToString());
            }

            return iDList;
        }

        /// <summary>
        /// 檢查 FtyInventory or LocalOrderInventory BarCode 是否為空
        /// Confirm 時檢查
        /// </summary>
        /// <inheritdoc/>
        public static bool CheckBarCode(DataTable dtDetail, string function, bool isLocalOrderInventory = false, bool showmsg = true)
        {
            if (dtDetail == null)
            {
                return false;
            }

            if (dtDetail.Rows.Count == 0)
            {
                return true;
            }

            WHTableName detailTableName = GetWHDetailTableName(function);
            string showMsg = isLocalOrderInventory ? "Barcode cannot be empty." : "FtyInventory barcode can't empty.";
            string strTableName = isLocalOrderInventory ? "LocalOrderInventory" : "FtyInventory";
            string strBalanceQty = isLocalOrderInventory ? "f.InQty-f.OutQty+f.AdjustQty" : "f.InQty-f.OutQty+f.AdjustQty-f.ReturnQty";
            if (detailTableName == WHTableName.SubTransfer_Detail || detailTableName == WHTableName.BorrowBack_Detail)
            {
                DataTable emptyBarcodedt = new DataTable();
                emptyBarcodedt.Columns.Add("Ukey");
                emptyBarcodedt.Columns.Add("Qty");

                foreach (DataRow dr in dtDetail.Select("Qty >= 0 and FabricType = 'F' and isnull(Barcode, '') = ''"))
                {
                    emptyBarcodedt.ImportRow(dr);
                }

                foreach (DataRow dr in dtDetail.Select("Qty < 0 and FabricType = 'F' and isnull(ToBarcode, '') = ''"))
                {
                    DataRow newrow = emptyBarcodedt.NewRow();
                    newrow["Ukey"] = dr["ToUkey"];
                    newrow["Qty"] = dr["Qty"];
                    emptyBarcodedt.Rows.Add(newrow);
                }

                if (emptyBarcodedt.Rows.Count > 0)
                {
                    string sqlcmd = $@"
select f.POID,Seq = f.Seq1 + ' ' + f.Seq2,f.Roll,f.Dyelot
,StockType = case f.StockType 
			when 'I' then 'Inventory'
			when 'B' then 'Bulk'
			when 'O' then 'Scrap'
			else f.StockType end 
, BalanceQty = t.Qty
from {strTableName} f
inner join #tmp t on t.ukey = f.Ukey
";

                    DualResult result = MyUtility.Tool.ProcessWithDatatable(emptyBarcodedt, string.Empty, sqlcmd, out DataTable dtS);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox(result.ToString());
                    }

                    if (showmsg)
                    {
                        Class.MsgGridPrg form = new Class.MsgGridPrg(dtS, showMsg);
                        form.ShowDialog();
                    }

                    //Class.WH_BarcodeEmpty wH_Barcode = new Class.WH_BarcodeEmpty(emptyBarcodedt, showMsg, isSubTransferOrBorrowBack: true, isLocalOrderInventory: isLocalOrderInventory);
                    //wH_Barcode.ShowDialog();
                    return false;
                }
            }
            else
            {
                string checkFilter = "FabricType = 'F' and isnull(Barcode, '') = ''";
                if (detailTableName == WHTableName.Adjust_Detail || detailTableName == WHTableName.Stocktaking_Detail || detailTableName == WHTableName.IssueReturn_Detail || detailTableName == WHTableName.LocalOrderAdjust_Detail)
                {
                    checkFilter += " and balanceQty > 0";
                }

                if (dtDetail.Select(checkFilter).Length > 0)
                {
                    string sqlcmd = $@"
select f.POID,Seq = f.Seq1 + ' ' + f.Seq2,f.Roll,f.Dyelot
,StockType = case f.StockType 
			when 'I' then 'Inventory'
			when 'B' then 'Bulk'
			when 'O' then 'Scrap'
			else f.StockType end 
, BalanceQty = {strBalanceQty}
from {strTableName} f
inner join #tmp t on t.ukey = f.Ukey
";

                    DualResult result = MyUtility.Tool.ProcessWithDatatable(dtDetail.Select(checkFilter).CopyToDataTable(), string.Empty, sqlcmd, out DataTable dtS);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox(result.ToString());
                    }

                    Class.MsgGridPrg form = new Class.MsgGridPrg(dtS, showMsg);
                    form.ShowDialog();

                    //Class.WH_BarcodeEmpty wH_Barcode = new Class.WH_BarcodeEmpty(dtDetail.Select(checkFilter).CopyToDataTable(), showMsg, isLocalOrderInventory: isLocalOrderInventory);
                    //wH_Barcode.ShowDialog();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 使用6個Key PoId,Seq1,Seq2,Roll,Dyelot,StockType 找出 Detail 的 Ukey
        /// </summary>
        /// <inheritdoc/>
        public static DataTable GetWHDetailUkey(DataTable dt6Key, string function)
        {
            WHTableName detailTableName = GetWHDetailTableName(function);
            string sqlcmd = $@"
select sd.ukey
FROM Production.dbo.{detailTableName} sd
inner join #tmp s on s.POID = sd.PoId
    and s.Seq1 = sd.Seq1
    and s.Seq2 = sd.Seq2
    and s.Roll = sd.Roll
    and s.Dyelot = sd.Dyelot
    and s.StockType = sd.StockType ";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dt6Key, null, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }

            return dt;
        }

        /// <inheritdoc/>
        public static DualResult GetFtyInventoryData(DataTable dtDetail, string function, out DataTable dt, AbstractDBProxyPMS proxyPMS = null)
        {
            WHTableName detailTableName = GetWHDetailTableName(function);
            string psd_FtyDt = GetWHjoinPSD_Fty(detailTableName);

            // Issue 部分程式第 2 層是 Issue_Summary,第3層才是 Issue_Detail
            string ukeys = "0";
            if (dtDetail.Columns.Contains("Issue_DetailUkey") && dtDetail.Rows.Count > 0)
            {
                ukeys = dtDetail.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["Issue_DetailUkey"])).ToList().JoinToString(",");
            }

            if (dtDetail.Columns.Contains("Ukey") && dtDetail.Rows.Count > 0)
            {
                ukeys = dtDetail.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["Ukey"])).ToList().JoinToString(",");
            }

            string columnIsWMS = string.Empty;
            if (detailTableName == WHTableName.SubTransfer_Detail || detailTableName == WHTableName.BorrowBack_Detail)
            {
                columnIsWMS = @"
    ,ToUkey = fto.Ukey
    ,ToBarcode = fto.Barcode
    ,[ToBalanceQty] = isnull(fto.InQty,0) -isnull(fto.OutQty,0) + isnull(fto.AdjustQty,0) - isnull(fto.ReturnQty,0)
    ,sd.Qty
    ,sd.FromPOID
    ,sd.FromSeq1
    ,sd.FromSeq2
    ,sd.FromRoll
    ,sd.FromDyelot
    ,sd.FromStockType
    ,sd.ToPOID
    ,sd.ToSeq1
    ,sd.ToSeq2
    ,sd.ToRoll
    ,sd.ToDyelot
    ,sd.ToStockType
";
            }

            string sqlcmd = $@"
select f.*
    ,balanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
    ,psd.FabricType
    ,DetailUkey = sd.Ukey
{columnIsWMS}
FROM {detailTableName} sd with(nolock)
{psd_FtyDt}
where sd.Ukey in ({ukeys})
";
            return proxyPMS == null ? DBProxy.Current.Select("Production", sqlcmd, out dt) : proxyPMS.Select("Production", sqlcmd, out dt);
        }

        /// <inheritdoc/>
        public static DualResult GetLocalOrderInventoryData(DataTable dtDetail, string function, out DataTable dt, AbstractDBProxyPMS proxyPMS = null)
        {
            WHTableName detailTableName = GetWHDetailTableName(function);
            string ukeys = "0";

            if (dtDetail.Columns.Contains("Ukey") && dtDetail.Rows.Count > 0)
            {
                ukeys = dtDetail.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["Ukey"])).ToList().JoinToString(",");
            }

            string sqlcmd = $@"
select loi.*
    ,balanceQty = isnull(loi.InQty,0) - isnull(loi.OutQty,0) + isnull(loi.AdjustQty,0)
    ,FabricType = lom.FabricType
    ,DetailUkey = sd.Ukey
FROM {detailTableName} sd with(nolock)
LEFT JOIN LocalOrderMaterial lom ON lom.POID = sd.POID AND sd.Seq1 = lom.Seq1 AND sd.Seq2 = lom.Seq2
LEFT JOIN Production.dbo.LocalOrderInventory loi with(nolock) on loi.POID = isnull(sd.PoId, '')
    and loi.Seq1 = isnull(sd.Seq1, '')
    and loi.Seq2 = isnull(sd.Seq2, '')
    and loi.Roll = isnull(sd.Roll, '')
	and loi.Dyelot = isnull(sd.Dyelot, '')
    and loi.StockType = isnull(sd.StockType, '')
where sd.Ukey in ({ukeys})
";
            return proxyPMS == null ? DBProxy.Current.Select("Production", sqlcmd, out dt) : proxyPMS.Select("Production", sqlcmd, out dt);
        }

        /// <inheritdoc/>
        public static string GetWHjoinPSD_Fty(WHTableName detailTable)
        {
            // 轉料單&非轉料單 join 條件不同
            // left join 千萬不要改 THX
            string psd_FtyDt;
            if (detailTable == WHTableName.SubTransfer_Detail || detailTable == WHTableName.BorrowBack_Detail)
            {
                psd_FtyDt = $@"
left join Production.dbo.PO_Supp_Detail psd with(nolock) on psd.ID = sd.FromPoId and psd.SEQ1 = sd.FromSeq1 and psd.SEQ2 = sd.FromSeq2
left join Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join Production.dbo.PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join Production.dbo.FtyInventory f with(nolock) on f.POID = isnull(sd.FromPoId, '')
    and f.Seq1 = isnull(sd.FromSeq1, '')
    and f.Seq2 = isnull(sd.FromSeq2, '')
    and f.Roll = isnull(sd.FromRoll, '')
	and f.Dyelot = isnull(sd.FromDyelot, '')
    and f.StockType = isnull(sd.FromStockType, '')
left join FtyInventory fto with(nolock) on fto.POID = isnull(sd.ToPOID, '')
    and fto.Seq1 = isnull(sd.ToSeq1, '')
    and fto.Seq2 = isnull(sd.ToSeq2, '')
    and fto.Roll = isnull(sd.ToRoll, '')
    and fto.Dyelot = isnull(sd.ToDyelot, '')
    and fto.StockType = isnull(sd.ToStockType, '')
";
            }
            else
            {
                psd_FtyDt = $@"
left join Production.dbo.PO_Supp_Detail psd with(nolock) on psd.ID = sd.PoId and psd.SEQ1 = sd.Seq1 and psd.SEQ2 = sd.Seq2
left join Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join Production.dbo.PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join Production.dbo.FtyInventory f with(nolock) on f.POID = isnull(sd.PoId, '')
    and f.Seq1 = isnull(sd.Seq1, '')
    and f.Seq2 = isnull(sd.Seq2, '')
    and f.Roll = isnull(sd.Roll, '')
	and f.Dyelot = isnull(sd.Dyelot, '')
    and f.StockType = isnull(sd.StockType, '')";
            }

            return psd_FtyDt;
        }

        /// <summary>
        /// confrim / unconfrim 更新WH主料 Barcode
        /// 使用:因為需要判斷FtyInventory更新後的庫存,此function放在(更新 FtyInventory 的庫存)之後,一起包在 TransactionScope 內
        /// </summary>
        /// <param name="oriFtyInventory">尚未更新 FtyInventory 庫存的資料</param>
        /// <param name="isRevise">P99 的 Revise 功能</param>
        /// <param name="isDelete">P99 的 Delete 功能</param>
        /// <inheritdoc/>
        public static DualResult UpdateWH_Barcode(bool isConfirmed, DataTable dtDetailSource, string function, out bool fromNewBarcode, DataTable oriFtyInventory = null, bool isRevise = false, bool isDelete = false, bool getOriFtyInventory = false, AbstractDBProxyPMS proxyPMS = null, bool isLocalOrder = false)
        {
            // 庫存0 = 沒or清空 Barcode
            // ↓在不覆蓋移動目標 Barcode 原則下↓
            // 庫存 < 全部 > 移動 = 搬移 Barcode
            // 庫存 < 部分 > 移動 = 新的 BarcodeSeq
            // 更新 WHBarcodeTransaction / FtyInventory 沒有先後, 先處理 WHBarcodeTransaction Barcode, FtyInventory 直接使用 WHBarcodeTransaction 物件的 Barcode
            DualResult result;
            SqlConnection sqlConnection;
            fromNewBarcode = false;
            if (dtDetailSource.Rows.Count == 0)
            {
                return Result.True;
            }

            string strInvTableName = GetInventoryTableName(isLocalOrder);

            // Issue 部分程式第 2 層是 Issue_Summary,第3層才是 Issue_Detail
            DataTable dtDetail = dtDetailSource.Copy();
            if (dtDetail.Columns.Contains("Issue_DetailUkey"))
            {
                if (dtDetail.Columns.Contains("Ukey"))
                {
                    dtDetail.Columns.Remove("Ukey");
                }

                dtDetail.Columns["Issue_DetailUkey"].ColumnName = "Ukey";
            }

            if (dtDetail.Columns.Contains("StockQty"))
            {
                if (dtDetail.Columns.Contains("Qty"))
                {
                    dtDetail.Columns.Remove("Qty");
                }

                dtDetail.Columns["StockQty"].ColumnName = "Qty";
            }

            DataTable odt;
            WHTableName detailTableName = GetWHDetailTableName(function);

            // Batch Create 並 confirm 的程式沒有 Ukey
            if (!dtDetail.Columns.Contains("Ukey"))
            {
                string sqldetail = $"select * from {detailTableName} with(nolock) where id = '{dtDetail.Rows[0]["ID"]}'";
                if (!(result = proxyPMS == null ? DBProxy.Current.Select("Production", sqldetail, out dtDetail) : proxyPMS.Select("Production", sqldetail, out dtDetail)))
                {
                    return result;
                }
            }

            string ukeys = dtDetail.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["Ukey"])).ToList().JoinToString(",");

            #region (QMS)有建立直接 Confirm,需要回推原本庫存
            if (getOriFtyInventory && oriFtyInventory == null && isConfirmed)
            {
                if (isLocalOrder)
                {
                    if (!(result = GetLocalOrderInventoryData(dtDetailSource, function, out oriFtyInventory)))
                    {
                        return result;
                    }
                }
                else
                {
                    if (!(result = GetFtyInventoryData(dtDetailSource, function, out oriFtyInventory)))
                    {
                        return result;
                    }
                }

                string qty = (isRevise || isDelete) ? "DiffQty" : "Qty";
                foreach (DataRow ftydr in oriFtyInventory.Rows)
                {
                    DataRow deraildr = dtDetailSource.Select($"Ukey = {ftydr["DetailUkey"]}")[0];
                    decimal balanceQty = MyUtility.Convert.GetDecimal(ftydr["balanceQty"]);
                    switch (detailTableName)
                    {
                        // InQty
                        case WHTableName.Receiving_Detail:
                        case WHTableName.TransferIn_Detail:
                        case WHTableName.LocalOrderReceiving_Detail:
                            ftydr["balanceQty"] = balanceQty - (isConfirmed ? MyUtility.Convert.GetDecimal(deraildr[qty]) : -MyUtility.Convert.GetDecimal(deraildr[qty]));
                            break;

                        // ReturnQty
                        case WHTableName.ReturnReceipt_Detail:
                            ftydr["balanceQty"] = balanceQty - (isConfirmed ? -MyUtility.Convert.GetDecimal(deraildr[qty]) : MyUtility.Convert.GetDecimal(deraildr[qty]));
                            break;

                        // OutQty
                        case WHTableName.IssueReturn_Detail:
                        case WHTableName.Issue_Detail:
                        case WHTableName.IssueLack_Detail:
                        case WHTableName.TransferOut_Detail:
                        case WHTableName.LocalOrderIssue_Detail:
                            ftydr["balanceQty"] = balanceQty - (isConfirmed ? -MyUtility.Convert.GetDecimal(deraildr[qty]) : MyUtility.Convert.GetDecimal(deraildr[qty]));
                            break;

                        // OutQty : from
                        // InQty : to
                        case WHTableName.SubTransfer_Detail:
                        case WHTableName.BorrowBack_Detail:
                            ftydr["balanceQty"] = balanceQty - (isConfirmed ? -MyUtility.Convert.GetDecimal(deraildr[qty]) : MyUtility.Convert.GetDecimal(deraildr[qty]));
                            ftydr["tobalanceQty"] = MyUtility.Convert.GetDecimal(ftydr["tobalanceQty"]) - (isConfirmed ? MyUtility.Convert.GetDecimal(deraildr[qty]) : -MyUtility.Convert.GetDecimal(deraildr[qty]));

                            break;

                        // AdjustQty
                        case WHTableName.Adjust_Detail:
                        case WHTableName.LocalOrderAdjust_Detail:
                            decimal adjustQty;
                            if (isRevise || isDelete)
                            {
                                adjustQty = MyUtility.Convert.GetDecimal(deraildr["DiffQty"]);
                            }
                            else
                            {
                                adjustQty = MyUtility.Convert.GetDecimal(deraildr["QtyAfter"]) - MyUtility.Convert.GetDecimal(deraildr["QtyBefore"]);
                            }

                            ftydr["balanceQty"] = balanceQty - (isConfirmed ? adjustQty : -adjustQty);
                            break;
                    }
                }
            }
            #endregion

            #region 取得已經更新庫存(Qty) FtyInventory 資料, TransactionScope 內要 with(nolock)
            string columns = @"
	,sd.POID
	,sd.Seq1
	,sd.Seq2
	,sd.Roll
	,sd.Dyelot
	,sd.StockType";
            string ftytable = $@"
inner join Production.dbo.{strInvTableName} f with(nolock) on f.POID = sd.PoId
    and f.Seq1 = sd.Seq1
    and f.Seq2 = sd.Seq2
    and f.Roll = sd.Roll
	and f.Dyelot = sd.Dyelot
    and f.StockType = sd.StockType
";
            string othertables = string.Empty;
            switch (detailTableName)
            {
                case WHTableName.SubTransfer_Detail:
                case WHTableName.BorrowBack_Detail:
                    columns = @"
    ,POID = sd.FromPOID
    ,Seq1 = sd.FromSeq1
    ,Seq2 = sd.FromSeq2
    ,Roll = sd.FromRoll
    ,Dyelot = sd.FromDyelot
    ,StockType = sd.FromStockType
    ,sd.Qty

    ,ToFabric_FtyInventoryUkey = fto.Ukey
    ,[ToBalanceQty] = isnull(fto.InQty,0) -isnull(fto.OutQty,0) + isnull(fto.AdjustQty,0) - isnull(fto.ReturnQty,0)
    ,sd.ToPOID
    ,sd.ToSeq1
    ,sd.ToSeq2
    ,sd.ToRoll
    ,sd.ToDyelot
    ,sd.ToStockType
    ,ToBarcode = fto.Barcode
    ,ToBarcodeSeq = fto.BarcodeSeq";
                    ftytable = @"
inner join FtyInventory f with(nolock) on f.POID = sd.FromPOID
    and f.Seq1 = sd.FromSeq1
    and f.Seq2 = sd.FromSeq2
    and f.Roll = sd.FromRoll
    and f.Dyelot = sd.FromDyelot
    and f.StockType = sd.FromStockType";
                    othertables = @"
left join FtyInventory fto with(nolock) on fto.POID = sd.ToPOID
    and fto.Seq1 = sd.ToSeq1
    and fto.Seq2 = sd.ToSeq2
    and fto.Roll = sd.ToRoll
    and fto.Dyelot = sd.ToDyelot
    and fto.StockType = sd.ToStockType";
                    break;
                case WHTableName.ReturnReceipt_Detail:
                case WHTableName.Issue_Detail:
                case WHTableName.IssueLack_Detail:
                case WHTableName.TransferOut_Detail:
                case WHTableName.LocalOrderIssue_Detail:
                    string strLastBarcode = isLocalOrder ? "lastBarcode.To_NewBarcode" : @" iif (isnull(lastBarcode.To_NewBarcode, '') = '', fbOri.Barcode, lastBarcode.To_NewBarcode)";
                    string strLastBarcodeSeq = isLocalOrder ? "lastBarcode.To_NewBarcodeSeq" : @"iif (isnull(lastBarcode.To_NewBarcodeSeq, '') = '', fbOri.BarcodeSeq, lastBarcode.To_NewBarcodeSeq)";
                    columns += $@"
    ,lastBarcode =   {strLastBarcode}
    ,lastBarcodeSeq = {strLastBarcodeSeq}
";
                    if (isLocalOrder)
                    {
                        othertables = @"
outer apply(
	select w.To_NewBarcode, w.To_NewBarcodeSeq
	from WHBarcodeTransaction w
	where w.TransactionID = sd.id
	and w.Action = 'Confirm'
	and w.TransactionUkey = sd.ukey
)lastBarcode";
                    }
                    else
                    {
                        othertables = @"
outer apply(
	select w.To_NewBarcode, w.To_NewBarcodeSeq
	from WHBarcodeTransaction w
	where w.TransactionID = sd.id
	and w.Action = 'Confirm'
	and w.TransactionUkey = sd.ukey
)lastBarcode
outer apply(
	select
		Barcode = iif(Barcode like '%-%', SUBSTRING(Barcode,0,CHARINDEX('-',Barcode,0)), Barcode),
		BarcodeSeq = iif(Barcode like '%-%', SUBSTRING(Barcode,CHARINDEX('-',Barcode,0)+1,3), '')
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
	and t.TransactionID = sd.Id
)fbOri";
                    }
                    break;
            }

            if (isRevise)
            {
                columns += @"
    ,fbOld.OldData_BarCode";
                othertables += @"
outer apply(
    select OldData_BarCode = iif(barcode like '%-%', SUBSTRING(barcode, 0, CHARINDEX('-', barcode, 0)), barcode)
    from(
        select barcode = MAX(barcode)
        from FtyInventory_Barcode fb
        where fb.Ukey = f.Ukey
        and fb.TransactionID = sd.Id
    )x
)fbOld";
            }

            DataTable dt;
            string sqlcmd;
            string strReturnQty = isLocalOrder ? string.Empty : "- isnull(f.ReturnQty, 0)";
            if (isDelete)
            {
                sqlcmd = $@"
select
	sd.ID
    ,sd.Ukey
	,rn = ROW_NUMBER()over(order by sd.Ukey)

    ,Fabric_FtyInventoryUkey = f.Ukey
    ,[balanceQty] = isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0)  {strReturnQty}
    ,f.Barcode
    ,f.BarcodeSeq
    ,RankFtyInventory = RANK() over(order by f.Ukey)
    ,countFtyInventory = count(1) over(order by f.Ukey)
{columns}
from #tmp sd
{ftytable}
{othertables}
where 1=1
and sd.FabricType = 'F'
order by sd.Ukey
";
                if (proxyPMS == null)
                {
                    DBProxy._OpenConnection("Production", out sqlConnection); // for MES
                    using (sqlConnection)
                    {
                        result = MyUtility.Tool.ProcessWithDatatable(dtDetail, string.Empty, sqlcmd, out dt, conn: sqlConnection);
                        if (!result)
                        {
                            return result;
                        }
                    }
                }
                else
                {
                    result = proxyPMS.ProcessWithDatatable("Production", dtDetail, string.Empty, sqlcmd, out dt);
                    if (!result)
                    {
                        return result;
                    }
                }
            }
            else
            {
                string strFabric = isLocalOrder == false ? @"
and exists(
	select 1 from Production.dbo.PO_Supp_Detail psd
	where psd.id = f.Poid and psd.seq1 = f.seq1 and psd.seq2 = f.seq2 
	and psd.FabricType = 'F'
)" :
@"
and exists(
	select 1 from Production.dbo.LocalOrderMaterial lom
	where lom.Poid = f.Poid and lom.seq1 = f.seq1 and lom.seq2 = f.seq2 
	and lom.FabricType = 'F'
)
";

                if (detailTableName == WHTableName.Receiving_Detail ||
                    detailTableName == WHTableName.TransferIn_Detail||
                    detailTableName == WHTableName.LocalOrderReceiving_Detail)
                {
                    string strBarocde = isLocalOrder == false ? @" iif(isnull(sd.MINDQRCode, '') = '', f.Barcode, sd.MINDQRCode)" : "iif(isnull(sd.Barcode, '') = '', f.Barcode, sd.Barcode)";
                    sqlcmd = $@"
select
	sd.ID
    ,sd.Ukey
	,rn = ROW_NUMBER()over(order by sd.Ukey)
    ,Fabric_FtyInventoryUkey = f.Ukey
    ,[balanceQty] = isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) {strReturnQty}
    ,[Barcode] = {strBarocde}
    ,f.BarcodeSeq
    ,RankFtyInventory = RANK() over(order by f.Ukey)
    ,countFtyInventory = count(1) over(order by f.Ukey)
{columns}
from {detailTableName} sd with (nolock)
{ftytable}
{othertables}
where 1=1
{strFabric}
and sd.Ukey in ({ukeys})
order by sd.Ukey
";
                }
                else
                {
                    sqlcmd = $@"
select
	sd.ID
    ,sd.Ukey
	,rn = ROW_NUMBER()over(order by sd.Ukey)

    ,Fabric_FtyInventoryUkey = f.Ukey
    ,[balanceQty] = isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) {strReturnQty}
    ,f.Barcode
    ,f.BarcodeSeq
    ,RankFtyInventory = RANK() over(order by f.Ukey)
    ,countFtyInventory = count(1) over(order by f.Ukey)
{columns}
from {detailTableName} sd
{ftytable}
{othertables}
where 1=1
{strFabric}
and sd.Ukey in ({ukeys})
order by sd.Ukey
";
                }

                result = proxyPMS == null ? DBProxy.Current.Select("Production", sqlcmd, out dt) : proxyPMS.Select("Production", sqlcmd, out dt);
                if (!result)
                {
                    return result;
                }
            }

            // 此單沒主料
            if (dt.Rows.Count == 0)
            {
                return Result.True;
            }
            #endregion

            #region 取得新 BarCode
            List<string> newBarcodeList = new List<string>();
            if (oriFtyInventory != null)
            {
                string filter = string.Empty;
                switch (detailTableName)
                {
                    case WHTableName.Receiving_Detail:
                    case WHTableName.TransferIn_Detail:
                    case WHTableName.IssueReturn_Detail:
                    case WHTableName.LocalOrderReceiving_Detail:
                        filter = "FabricType = 'F' and isnull(Barcode, '') = ''";
                        break;
                    case WHTableName.Adjust_Detail:
                    case WHTableName.LocalOrderAdjust_Detail:
                        filter = "FabricType = 'F' and balanceQty <= 0"; // 舊資料有坑,所以判斷要用<未更新>庫存判斷
                        break;
                }

                if (!filter.Empty())
                {
                    int count = oriFtyInventory.Select(filter).Length;
                    if (count > 0)
                    {
                        newBarcodeList = Prgs.GetBarcodeNo_WH("F", count, dtBarcodeSource: dt, proxyPMS: proxyPMS);
                    }
                }
            }
            #endregion

            dt.Columns.Add("oriBalanceQty", typeof(decimal));

            #region 一次更新同物料有兩筆以上, balanceQty 庫存要改成逐筆計算 2022/04/11
            if (oriFtyInventory != null)
            {
                string qty = isRevise ? "DiffQty" : (isDelete ? "Old_Qty" : "Qty");
                foreach (DataRow dr in dt.Rows)
                {
                    if (oriFtyInventory.Select($"Ukey = '{dr["Fabric_FtyInventoryUkey"]}'").Length == 0)
                    {
                        continue;
                    }

                    DataRow ftydr = oriFtyInventory.Select($"Ukey = '{dr["Fabric_FtyInventoryUkey"]}'")[0];
                    DataRow deraildr = dtDetail.Select($"Ukey = '{dr["Ukey"]}'")[0];
                    decimal oriBalanceQty = MyUtility.Convert.GetDecimal(ftydr["balanceQty"]);
                    dr["oriBalanceQty"] = oriBalanceQty;

                    // 原庫存 +- Qty (+InQty - OutQty + AdjustQty - ReturnQty)
                    switch (detailTableName)
                    {
                        // InQty
                        case WHTableName.Receiving_Detail:
                        case WHTableName.TransferIn_Detail:
                        case WHTableName.IssueReturn_Detail: // OutQty 的減項計算同 InQty
                        case WHTableName.LocalOrderReceiving_Detail:
                            dr["balanceQty"] = ftydr["balanceQty"] = oriBalanceQty + (isConfirmed ? MyUtility.Convert.GetDecimal(deraildr[qty]) : -MyUtility.Convert.GetDecimal(deraildr[qty]));
                            break;

                        // ReturnQty
                        case WHTableName.ReturnReceipt_Detail:
                            dr["balanceQty"] = ftydr["balanceQty"] = oriBalanceQty + (isConfirmed ? -MyUtility.Convert.GetDecimal(deraildr[qty]) : MyUtility.Convert.GetDecimal(deraildr[qty]));
                            break;

                        // OutQty
                        case WHTableName.Issue_Detail:
                        case WHTableName.IssueLack_Detail:
                        case WHTableName.TransferOut_Detail:
                        case WHTableName.LocalOrderIssue_Detail:
                            dr["balanceQty"] = ftydr["balanceQty"] = oriBalanceQty + (isConfirmed ? -MyUtility.Convert.GetDecimal(deraildr[qty]) : MyUtility.Convert.GetDecimal(deraildr[qty]));
                            break;

                        // OutQty : from
                        // InQty : to
                        case WHTableName.SubTransfer_Detail:
                        case WHTableName.BorrowBack_Detail:
                            dr["balanceQty"] = ftydr["balanceQty"] = oriBalanceQty + (isConfirmed ? -MyUtility.Convert.GetDecimal(deraildr[qty]) : MyUtility.Convert.GetDecimal(deraildr[qty]));
                            if (oriFtyInventory.Select($"ToUkey = '{dr["ToFabric_FtyInventoryUkey"]}'").Length > 0)
                            {
                                DataRow toFtydr = oriFtyInventory.Select($"ToUkey = '{dr["ToFabric_FtyInventoryUkey"]}'")[0];
                                decimal toOriBalanceQty = MyUtility.Convert.GetDecimal(toFtydr["tobalanceQty"]);
                                dr["tobalanceQty"] = toFtydr["tobalanceQty"] = toOriBalanceQty + (isConfirmed ? MyUtility.Convert.GetDecimal(deraildr[qty]) : -MyUtility.Convert.GetDecimal(deraildr[qty]));
                            }

                            break;

                        // AdjustQty
                        case WHTableName.Adjust_Detail:
                        case WHTableName.LocalOrderAdjust_Detail:
                            decimal adjustQty;
                            if (isRevise)
                            {
                                adjustQty = MyUtility.Convert.GetDecimal(deraildr["DiffQty"]);
                            }
                            else if (isDelete)
                            {
                                adjustQty = MyUtility.Convert.GetDecimal(deraildr["Old_Qty"]) - MyUtility.Convert.GetDecimal(deraildr["QtyBefore"]);
                            }
                            else
                            {
                                adjustQty = MyUtility.Convert.GetDecimal(deraildr["QtyAfter"]) - MyUtility.Convert.GetDecimal(deraildr["QtyBefore"]);
                            }

                            dr["balanceQty"] = ftydr["balanceQty"] = oriBalanceQty + (isConfirmed ? adjustQty : -adjustQty);
                            break;
                    }
                }
            }
            #endregion

            #region 更新 WHBarcodeTransaction
            var wHBarcodeTransaction = dt.AsEnumerable().
                Select(s => new WHBarcodeTransaction
                {
                    Rn = MyUtility.Convert.GetLong(s["rn"]),
                    Function = function,
                    TransactionID = MyUtility.Convert.GetString(s["ID"]),
                    TransactionUkey = MyUtility.Convert.GetLong(s["Ukey"]),
                    Action = isConfirmed ? EnumStatus.Confirm.ToString() : EnumStatus.Unconfirm.ToString(),
                    From_OldBarcode = string.Empty,
                    From_OldBarcodeSeq = string.Empty,
                    From_NewBarcode = string.Empty,
                    From_NewBarcodeSeq = string.Empty,
                    To_OldBarcode = string.Empty,
                    To_OldBarcodeSeq = string.Empty,
                    To_NewBarcode = string.Empty,
                    To_NewBarcodeSeq = string.Empty,
                    UpdatethisItem = true,
                }).ToList();

            if (!isRevise)
            {
                int indexNewBarcode = 0;
                switch (detailTableName)
                {
                    case WHTableName.Receiving_Detail:
                    case WHTableName.TransferIn_Detail:
                    case WHTableName.IssueReturn_Detail:
                    case WHTableName.LocalOrderReceiving_Detail:
                        foreach (var item in wHBarcodeTransaction)
                        {
                            DataRow dr = dt.Select($"rn = {item.Rn}")[0];
                            if (isLocalOrder == true)
                            {
                                item.ToFabric_LocalOrderInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            }
                            else
                            {
                                item.ToFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            }

                            string barcode = MyUtility.Convert.GetString(dr["Barcode"]);
                            string barcodeSeq = MyUtility.Convert.GetString(dr["barcodeSeq"]);
                            if (isConfirmed)
                            {
                                if (!MyUtility.Check.Empty(barcode))
                                {
                                    item.To_OldBarcode = barcode;
                                    item.To_OldBarcodeSeq = barcodeSeq;
                                    item.To_NewBarcode = barcode;
                                    item.To_NewBarcodeSeq = barcodeSeq;
                                }
                                else
                                {
                                    item.To_NewBarcode = newBarcodeList[indexNewBarcode];
                                    indexNewBarcode++;

                                    UpdateSameFtyBarcode(dt, item, "Receiving", "Barcode", isLocalOrder: isLocalOrder);
                                }
                            }
                            else
                            {
                                item.To_OldBarcode = barcode;
                                item.To_OldBarcodeSeq = barcodeSeq;
                                if (!MyUtility.Check.Empty(dr["balanceQty"]))
                                {
                                    item.To_NewBarcode = barcode;
                                    item.To_NewBarcodeSeq = barcodeSeq;
                                }
                            }
                        }

                        break;
                    case WHTableName.ReturnReceipt_Detail:
                    case WHTableName.Issue_Detail:
                    case WHTableName.IssueLack_Detail:
                    case WHTableName.TransferOut_Detail:
                    case WHTableName.LocalOrderIssue_Detail:
                        foreach (var item in wHBarcodeTransaction)
                        {
                            DataRow dr = dt.Select($"rn = {item.Rn}")[0];
                            if (isLocalOrder == true)
                            {
                                item.FromFabric_LocalOrderInvnetoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                                item.ToFabric_LocalOrderInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            }
                            else
                            {
                                item.FromFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                                item.ToFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            }

                            string barcode = MyUtility.Convert.GetString(dr["Barcode"]);
                            string barcodeSeq = MyUtility.Convert.GetString(dr["barcodeSeq"]);
                            string lastBarcode = MyUtility.Convert.GetString(dr["lastBarcode"]);
                            string lastBarcodeSeq = MyUtility.Convert.GetString(dr["lastBarcodeSeq"]);
                            item.From_OldBarcode = barcode;
                            item.From_OldBarcodeSeq = barcodeSeq;
                            if (isConfirmed)
                            {
                                item.To_NewBarcode = barcode;
                                if (!MyUtility.Check.Empty(dr["balanceQty"]))
                                {
                                    item.From_NewBarcode = barcode;
                                    item.From_NewBarcodeSeq = barcodeSeq;
                                    item.To_NewBarcodeSeq = GetNextBarcodeSeqInObjWHBarcodeTransaction(barcode, wHBarcodeTransaction, "To");
                                }
                                else
                                {
                                    item.To_NewBarcodeSeq = barcodeSeq;
                                }
                            }
                            else
                            {

                                item.To_OldBarcode = lastBarcode;
                                item.To_OldBarcodeSeq = lastBarcodeSeq;
                                if (!MyUtility.Check.Empty(barcode))
                                {
                                    item.From_NewBarcode = barcode;
                                    item.From_NewBarcodeSeq = barcodeSeq;
                                }
                                else
                                {
                                    item.From_NewBarcode = lastBarcode;
                                    item.From_NewBarcodeSeq = lastBarcodeSeq;

                                    UpdateSameFtyBarcode(dt, item, "From", "Barcode", isLocalOrder);
                                }
                            }
                        }

                        break;
                    case WHTableName.SubTransfer_Detail:
                    case WHTableName.BorrowBack_Detail:
                        foreach (var item in wHBarcodeTransaction)
                        {
                            DataRow dr = dt.Select($"rn = {item.Rn}")[0];
                            item.FromFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            item.ToFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["ToFabric_FtyInventoryUkey"]);
                            string barcode = MyUtility.Convert.GetString(dr["Barcode"]);
                            string barcodeSeq = MyUtility.Convert.GetString(dr["BarcodeSeq"]);
                            string tobarcode = MyUtility.Convert.GetString(dr["ToBarcode"]);
                            string tobarcodeSeq = MyUtility.Convert.GetString(dr["ToBarcodeSeq"]);
                            double qty = MyUtility.Convert.GetDouble(dr["Qty"]);
                            if (isConfirmed)
                            {
                                // 轉出數量是否負數
                                if (qty >= 0)
                                {
                                    item.From_OldBarcode = barcode;
                                    item.From_OldBarcodeSeq = barcodeSeq;
                                    if (!MyUtility.Check.Empty(tobarcode))
                                    {
                                        item.To_OldBarcode = tobarcode;
                                        item.To_OldBarcodeSeq = tobarcodeSeq;
                                        item.To_NewBarcode = tobarcode;
                                        item.To_NewBarcodeSeq = tobarcodeSeq;
                                        if (!MyUtility.Check.Empty(dr["balanceQty"]))
                                        {
                                            item.From_NewBarcode = barcode;
                                            item.From_NewBarcodeSeq = barcodeSeq;
                                        }
                                    }
                                    else
                                    {
                                        item.To_NewBarcode = barcode;
                                        if (!MyUtility.Check.Empty(dr["balanceQty"]))
                                        {
                                            item.From_NewBarcode = barcode;
                                            item.From_NewBarcodeSeq = barcodeSeq;
                                            item.To_NewBarcodeSeq = GetNextBarcodeSeqInObjWHBarcodeTransaction(barcode, wHBarcodeTransaction, "To");
                                        }
                                        else
                                        {
                                            item.To_NewBarcodeSeq = barcodeSeq;
                                        }

                                        UpdateSameFtyBarcode(dt, item, "To", "ToBarcode", isLocalOrder);
                                    }
                                }

                                // 轉出數量為負數
                                else
                                {
                                    item.To_OldBarcode = tobarcode;
                                    item.To_OldBarcodeSeq = tobarcodeSeq;
                                    if (!MyUtility.Check.Empty(barcode))
                                    {
                                        item.From_NewBarcode = barcode;
                                        item.From_NewBarcodeSeq = barcodeSeq;
                                        if (!MyUtility.Check.Empty(dr["TobalanceQty"]))
                                        {
                                            item.To_NewBarcode = tobarcode;
                                            item.To_NewBarcodeSeq = tobarcodeSeq;
                                        }
                                    }
                                    else
                                    {
                                        item.From_NewBarcode = tobarcode;
                                        if (!MyUtility.Check.Empty(dr["TobalanceQty"]))
                                        {
                                            item.From_NewBarcodeSeq = GetNextBarcodeSeqInObjWHBarcodeTransaction(tobarcode, wHBarcodeTransaction, "From");
                                            item.To_NewBarcode = tobarcode;
                                            item.To_NewBarcodeSeq = tobarcodeSeq;
                                        }
                                        else
                                        {
                                            item.From_NewBarcodeSeq = tobarcodeSeq;
                                        }

                                        UpdateSameFtyBarcode(dt, item, "From", "Barcode", isLocalOrder);
                                    }
                                }
                            }
                            else
                            {
                                // 轉出數量是否負數
                                if (qty >= 0)
                                {
                                    item.To_OldBarcode = tobarcode;
                                    item.To_OldBarcodeSeq = tobarcodeSeq;
                                    if (!MyUtility.Check.Empty(barcode))
                                    {
                                        item.From_NewBarcode = barcode;
                                        item.From_NewBarcodeSeq = barcodeSeq;
                                        if (!MyUtility.Check.Empty(dr["TobalanceQty"]))
                                        {
                                            item.To_NewBarcode = tobarcode;
                                            item.To_NewBarcodeSeq = tobarcodeSeq;
                                        }
                                    }
                                    else
                                    {
                                        item.From_NewBarcode = tobarcode;
                                        if (!MyUtility.Check.Empty(dr["TobalanceQty"]))
                                        {
                                            item.From_NewBarcodeSeq = GetNextBarcodeSeqInObjWHBarcodeTransaction(tobarcode, wHBarcodeTransaction, "From");
                                            item.To_NewBarcode = tobarcode;
                                            item.To_NewBarcodeSeq = tobarcodeSeq;
                                        }
                                        else
                                        {
                                            item.From_NewBarcodeSeq = tobarcodeSeq;
                                        }

                                        UpdateSameFtyBarcode(dt, item, "From", "Barcode", isLocalOrder);
                                    }
                                }

                                // 轉出數量為負數
                                else
                                {
                                    item.From_OldBarcode = barcode;
                                    item.From_OldBarcodeSeq = barcodeSeq;
                                    if (!MyUtility.Check.Empty(tobarcode))
                                    {
                                        item.To_OldBarcode = tobarcode;
                                        item.To_OldBarcodeSeq = tobarcodeSeq;
                                        item.To_NewBarcode = tobarcode;
                                        item.To_NewBarcodeSeq = tobarcodeSeq;
                                        if (!MyUtility.Check.Empty(dr["balanceQty"]))
                                        {
                                            item.From_NewBarcode = barcode;
                                            item.From_NewBarcodeSeq = barcodeSeq;
                                        }
                                    }
                                    else
                                    {
                                        item.To_NewBarcode = barcode;
                                        if (!MyUtility.Check.Empty(dr["balanceQty"]))
                                        {
                                            item.From_NewBarcode = barcode;
                                            item.From_NewBarcodeSeq = barcodeSeq;
                                            item.To_NewBarcodeSeq = GetNextBarcodeSeqInObjWHBarcodeTransaction(barcode, wHBarcodeTransaction, "To");
                                        }
                                        else
                                        {
                                            item.To_NewBarcodeSeq = barcodeSeq;
                                        }

                                        UpdateSameFtyBarcode(dt, item, "To", "ToBarcode", isLocalOrder);
                                    }
                                }
                            }
                        }

                        break;
                    case WHTableName.Adjust_Detail:
                    case WHTableName.LocalOrderAdjust_Detail:
                        foreach (var item in wHBarcodeTransaction)
                        {
                            DataRow dr = dt.Select($"rn = {item.Rn}")[0];
                            if (isLocalOrder == true)
                            {
                                item.FromFabric_LocalOrderInvnetoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            }
                            else
                            {
                                item.FromFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            }

                            string barcode = MyUtility.Convert.GetString(dr["Barcode"]);
                            string barcodeSeq = MyUtility.Convert.GetString(dr["barcodeSeq"]);

                            // confrim / unconfrim 流程,更新值一樣
                            if (!MyUtility.Check.Empty(dr["oriBalanceQty"]))
                            {
                                item.From_OldBarcode = barcode;
                                item.From_OldBarcodeSeq = barcodeSeq;
                                if (!MyUtility.Check.Empty(dr["balanceQty"]))
                                {
                                    item.From_NewBarcode = barcode;
                                    item.From_NewBarcodeSeq = barcodeSeq;
                                }
                            }
                            else
                            {
                                if (!MyUtility.Check.Empty(dr["balanceQty"]))
                                {
                                    item.From_NewBarcode = newBarcodeList[indexNewBarcode];
                                    indexNewBarcode++;
                                }
                            }
                        }

                        break;
                }
            }

            // P99 Revise
            else
            {
                int indexNewBarcode = 0;
                foreach (var item in wHBarcodeTransaction)
                {
                    // 沒有找到就不更新 WHBarcodeTransaction
                    sqlcmd = $@"
select *
from WHBarcodeTransaction w with(nolock) 
where w.[Function] = '{item.Function}'
and w.TransactionID = '{item.TransactionID}'
and w.TransactionUkey = '{item.TransactionUkey}'
and w.Action = '{item.Action}'";
                    item.UpdatethisItem = MyUtility.Check.Seek(sqlcmd, out DataRow drwHBarcodeTransaction, "Production");
                    if (item.UpdatethisItem)
                    {
                        item.From_OldBarcode = MyUtility.Convert.GetString(drwHBarcodeTransaction["From_OldBarcode"]);
                        item.From_OldBarcodeSeq = MyUtility.Convert.GetString(drwHBarcodeTransaction["From_OldBarcodeSeq"]);
                        item.To_OldBarcode = MyUtility.Convert.GetString(drwHBarcodeTransaction["To_OldBarcode"]);
                        item.To_OldBarcodeSeq = MyUtility.Convert.GetString(drwHBarcodeTransaction["To_OldBarcodeSeq"]);
                        item.To_NewBarcode = MyUtility.Convert.GetString(drwHBarcodeTransaction["To_NewBarcode"]);
                        item.To_NewBarcodeSeq = MyUtility.Convert.GetString(drwHBarcodeTransaction["To_NewBarcodeSeq"]);
                    }

                    DataRow dr = dt.Select($"rn = {item.Rn}")[0];
                    switch (detailTableName)
                    {
                        case WHTableName.Receiving_Detail:
                        case WHTableName.TransferIn_Detail:
                        case WHTableName.IssueReturn_Detail:
                            item.ToFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            break;
                        case WHTableName.ReturnReceipt_Detail:
                        case WHTableName.Issue_Detail:
                        case WHTableName.IssueLack_Detail:
                        case WHTableName.TransferOut_Detail:
                            item.FromFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            item.ToFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            break;
                        case WHTableName.SubTransfer_Detail:
                        case WHTableName.BorrowBack_Detail:
                            item.FromFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            item.ToFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["ToFabric_FtyInventoryUkey"]);
                            break;
                        case WHTableName.Adjust_Detail:
                            item.FromFabric_FtyInventoryUkey = MyUtility.Convert.GetString(dr["Fabric_FtyInventoryUkey"]);
                            break;
                    }

                    // P99 < 收料 >  Revise 不清空 WHBarcodeTransaction.Barcode 需要找回用
                    if (detailTableName == WHTableName.Receiving_Detail || detailTableName == WHTableName.TransferIn_Detail || detailTableName == WHTableName.IssueReturn_Detail)
                    {
                        if (MyUtility.Check.Empty(dr["balanceQty"]))
                        {
                            item.UpdatethisItem = false;
                            item.To_NewBarcode = string.Empty;
                            item.To_NewBarcodeSeq = string.Empty;
                        }
                        else
                        {
                            if (MyUtility.Check.Empty(dr["Barcode"]))
                            {
                                item.To_NewBarcode = newBarcodeList[indexNewBarcode];
                                indexNewBarcode++;

                                UpdateSameFtyBarcode(dt, item, "Receiving", "Barcode", isLocalOrder);
                            }
                            else
                            {
                                item.To_NewBarcode = MyUtility.Convert.GetString(dr["Barcode"]);
                                item.To_NewBarcodeSeq = MyUtility.Convert.GetString(dr["BarcodeSeq"]);
                            }
                        }
                    }
                    else
                    {
                        if (MyUtility.Check.Empty(dr["balanceQty"]))
                        {
                            item.From_NewBarcode = string.Empty;
                            item.From_NewBarcodeSeq = string.Empty;
                        }
                        else
                        {
                            fromNewBarcode = true;
                            if (MyUtility.Check.Empty(dr["Barcode"]))
                            {
                                if (item.UpdatethisItem)
                                {
                                    switch (detailTableName)
                                    {
                                        case WHTableName.Adjust_Detail:
                                            if (!MyUtility.Check.Empty(drwHBarcodeTransaction["From_OldBarcode"]))
                                            {
                                                item.From_NewBarcode = MyUtility.Convert.GetString(drwHBarcodeTransaction["From_OldBarcode"]);
                                                item.From_NewBarcodeSeq = GetNextBarcodeSeqInObjWHBarcodeTransaction(MyUtility.Convert.GetString(drwHBarcodeTransaction["From_OldBarcode"]), wHBarcodeTransaction, "From");
                                            }
                                            else
                                            {
                                                item.From_NewBarcode = MyUtility.Convert.GetString(drwHBarcodeTransaction["From_NewBarcode"]);
                                                item.From_NewBarcodeSeq = GetNextBarcodeSeqInObjWHBarcodeTransaction(MyUtility.Convert.GetString(drwHBarcodeTransaction["From_NewBarcode"]), wHBarcodeTransaction, "From");
                                            }

                                            break;
                                        default:
                                            item.From_NewBarcode = MyUtility.Convert.GetString(drwHBarcodeTransaction["To_NewBarcode"]);
                                            item.From_NewBarcodeSeq = GetNextBarcodeSeqInObjWHBarcodeTransaction(MyUtility.Convert.GetString(drwHBarcodeTransaction["To_NewBarcode"]), wHBarcodeTransaction, "From");
                                            break;
                                    }
                                }
                                else
                                {
                                    // 用舊資料 FtyInventory_BarCode (下方仍要更新 FtyInventory BarCode 使用)
                                    switch (detailTableName)
                                    {
                                        case WHTableName.ReturnReceipt_Detail:
                                        case WHTableName.Issue_Detail:
                                        case WHTableName.IssueLack_Detail:
                                        case WHTableName.TransferOut_Detail:
                                            item.From_NewBarcode = MyUtility.Convert.GetString(dr["lastBarcode"]);
                                            item.From_NewBarcodeSeq = GetNextBarcodeSeqInObjWHBarcodeTransaction(MyUtility.Convert.GetString(dr["lastBarcode"]), wHBarcodeTransaction, "From");
                                            break;
                                        default:
                                            item.From_NewBarcode = MyUtility.Convert.GetString(dr["OldData_BarCode"]);
                                            item.From_NewBarcodeSeq = GetNextBarcodeSeqInObjWHBarcodeTransaction(MyUtility.Convert.GetString(dr["OldData_BarCode"]), wHBarcodeTransaction, "From");
                                            break;
                                    }
                                }

                                UpdateSameFtyBarcode(dt, item, "From", "Barcode", isLocalOrder);
                            }
                            else
                            {
                                // 把 Fty 的 Barcode 填入(有可能別張單中間處理過)
                                item.From_NewBarcode = MyUtility.Convert.GetString(dr["Barcode"]);
                                item.From_NewBarcodeSeq = MyUtility.Convert.GetString(dr["barcodeSeq"]);
                            }
                        }
                    }
                }
            }

            if (wHBarcodeTransaction.Where(w => w.UpdatethisItem).Any())
            {
                string sqlUpdateReceiving_Detail = string.Empty;
                if (isLocalOrder == true)
                {
                    sqlUpdateReceiving_Detail = $@"
    update rd 
    set rd.Barcode = t.To_NewBarcode
    from {detailTableName} rd 
    inner join #tmp t on rd.Ukey = t.TransactionUkey
    where rd.Barcode = ''
";
                }
                else
                {
                    sqlUpdateReceiving_Detail = $@"
    update rd set rd.MINDQRCode = t.To_NewBarcode
    from {detailTableName} rd 
    inner join #tmp t on rd.Ukey = t.TransactionUkey
    where rd.MINDQRCode = ''
";
                }

                if (proxyPMS == null)
                {
                    DBProxy._OpenConnection("Production", out sqlConnection); // for MES
                    using (sqlConnection)
                    {
                        if (!(result = MyUtility.Tool.ProcessWithObject(wHBarcodeTransaction.Where(w => w.UpdatethisItem), string.Empty, UpdateWHBarcodeTransaction(), out odt, conn: sqlConnection)))
                        {
                            return result;
                        }

                        if (detailTableName == WHTableName.Receiving_Detail ||
                            detailTableName == WHTableName.TransferIn_Detail ||
                            detailTableName == WHTableName.LocalOrderReceiving_Detail)
                        {
                            result = DBProxy.Current.ExecuteByConn(sqlConnection, sqlUpdateReceiving_Detail);
                            if (!result)
                            {
                                return result;
                            }
                        }
                    }
                }
                else
                {
                    if (!(result = proxyPMS.ProcessWithDatatable("Production", wHBarcodeTransaction.Where(w => w.UpdatethisItem).ToList().ToDataTable(), string.Empty, UpdateWHBarcodeTransaction(), out odt)))
                    {
                        return result;
                    }

                    if (detailTableName == WHTableName.Receiving_Detail ||
                        detailTableName == WHTableName.TransferIn_Detail)
                    {
                        result = proxyPMS.Execute(sqlUpdateReceiving_Detail, "Production");
                        if (!result)
                        {
                            return result;
                        }
                    }
                }
            }
            #endregion

            #region 更新 Inventory BarCode
            var data_FtyBarcode = dt.AsEnumerable().
             Select(s => new FtyInventory
             {
                 Ukey = wHBarcodeTransaction.Where(w => w.Rn == MyUtility.Convert.GetLong(s["rn"])).Select(s1 => s1.FromFabric_FtyInventoryUkey).First(),
                 ToUkey = wHBarcodeTransaction.Where(w => w.Rn == MyUtility.Convert.GetLong(s["rn"])).Select(s1 => s1.ToFabric_FtyInventoryUkey).First(),
                 Rn = MyUtility.Convert.GetLong(s["rn"]),
                 Poid = s.Field<string>("poid"),
                 Seq1 = s.Field<string>("seq1"),
                 Seq2 = s.Field<string>("seq2"),
                 Stocktype = s.Field<string>("stocktype"),
                 Roll = s.Field<string>("roll"),
                 Dyelot = s.Field<string>("dyelot"),
             }).ToList();

            if (isLocalOrder)
            {
                data_FtyBarcode = dt.AsEnumerable().
              Select(s => new FtyInventory
              {
                  Ukey = wHBarcodeTransaction.Where(w => w.Rn == MyUtility.Convert.GetLong(s["rn"])).Select(s1 => s1.FromFabric_LocalOrderInvnetoryUkey).First(),
                  ToUkey = wHBarcodeTransaction.Where(w => w.Rn == MyUtility.Convert.GetLong(s["rn"])).Select(s1 => s1.ToFabric_LocalOrderInventoryUkey).First(),
                  Rn = MyUtility.Convert.GetLong(s["rn"]),
                  Poid = s.Field<string>("poid"),
                  Seq1 = s.Field<string>("seq1"),
                  Seq2 = s.Field<string>("seq2"),
                  Stocktype = s.Field<string>("stocktype"),
                  Roll = s.Field<string>("roll"),
                  Dyelot = s.Field<string>("dyelot"),
              }).ToList();
            }

            switch (detailTableName)
            {
                case WHTableName.Receiving_Detail:
                case WHTableName.TransferIn_Detail:
                case WHTableName.IssueReturn_Detail:
                case WHTableName.LocalOrderReceiving_Detail:
                    foreach (var item in data_FtyBarcode)
                    {
                        if (isLocalOrder)
                        {
                            var toBarcode = wHBarcodeTransaction.Where(w => w.ToFabric_LocalOrderInventoryUkey == item.ToUkey).OrderByDescending(o => o.TransactionUkey).First();
                            item.Barcode = toBarcode.To_NewBarcode;
                            item.BarcodeSeq = toBarcode.To_NewBarcodeSeq;
                        }
                        else
                        {
                            var toBarcode = wHBarcodeTransaction.Where(w => w.ToFabric_FtyInventoryUkey == item.ToUkey).OrderByDescending(o => o.TransactionUkey).First();
                            item.Barcode = toBarcode.To_NewBarcode;
                            item.BarcodeSeq = toBarcode.To_NewBarcodeSeq;
                        }
                    }

                    break;

                default:
                    foreach (var item in data_FtyBarcode)
                    {
                        if (isLocalOrder)
                        {
                            var fromBarcode = wHBarcodeTransaction.Where(w => w.FromFabric_LocalOrderInvnetoryUkey == item.Ukey).OrderByDescending(o => o.TransactionUkey).First();
                            item.Barcode = fromBarcode.From_NewBarcode;
                            item.BarcodeSeq = fromBarcode.From_NewBarcodeSeq;
                        }
                        else
                        {
                            var fromBarcode = wHBarcodeTransaction.Where(w => w.FromFabric_FtyInventoryUkey == item.Ukey).OrderByDescending(o => o.TransactionUkey).First();
                            item.Barcode = fromBarcode.From_NewBarcode;
                            item.BarcodeSeq = fromBarcode.From_NewBarcodeSeq;
                        }
                    }

                    break;
            }

            if (proxyPMS == null)
            {
                DBProxy._OpenConnection("Production", out sqlConnection); // for MES
                using (sqlConnection)
                {
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_FtyBarcode, string.Empty, UpdateFtyInventoryBarCode(isLocalOrder), out odt, conn: sqlConnection)))
                    {
                        return result;
                    }
                }
            }
            else
            {
                if (!(result = proxyPMS.ProcessWithDatatable("Production", data_FtyBarcode.ToList().ToDataTable(), string.Empty, UpdateFtyInventoryBarCode(isLocalOrder), out odt)))
                {
                    return result;
                }
            }

            // 轉料單才有 To...資訊 (P99 數量不能改成 0, Revise 無須更新 To 部分)
            if (!isRevise)
            {
                if (detailTableName == WHTableName.SubTransfer_Detail || detailTableName == WHTableName.BorrowBack_Detail)
                {
                    var data_To_FtyBarcode = dt.AsEnumerable().
                        Select(s => new FtyInventory
                        {
                            Ukey = wHBarcodeTransaction.Where(w => w.Rn == MyUtility.Convert.GetLong(s["rn"])).Select(s1 => s1.ToFabric_FtyInventoryUkey).First(),
                            Poid = s.Field<string>("Topoid"),
                            Seq1 = s.Field<string>("Toseq1"),
                            Seq2 = s.Field<string>("Toseq2"),
                            Stocktype = s.Field<string>("Tostocktype"),
                            Roll = s.Field<string>("Toroll"),
                            Dyelot = s.Field<string>("Todyelot"),
                        }).ToList();

                    // 同一批更新可能多筆轉入相同物料,若原本Barcode為空白, 取 TransactionUkey 最大那筆 Barcode, Seq
                    foreach (var item in data_To_FtyBarcode)
                    {
                        var toBarcode = wHBarcodeTransaction.Where(w => w.ToFabric_FtyInventoryUkey == item.Ukey).OrderByDescending(o => o.TransactionUkey).First();
                        item.Barcode = toBarcode.To_NewBarcode;
                        item.BarcodeSeq = toBarcode.To_NewBarcodeSeq;
                    }

                    if (proxyPMS == null)
                    {
                        DBProxy._OpenConnection("Production", out sqlConnection);
                        using (sqlConnection)
                        {
                            if (!(result = MyUtility.Tool.ProcessWithObject(data_To_FtyBarcode, string.Empty, UpdateFtyInventoryBarCode(isLocalOrder), out odt, conn: sqlConnection)))
                            {
                                return result;
                            }
                        }
                    }
                    else
                    {
                        if (!(result = proxyPMS.ProcessWithDatatable("Production", data_To_FtyBarcode.ToDataTable(), string.Empty, UpdateFtyInventoryBarCode(isLocalOrder), out odt)))
                        {
                            return result;
                        }
                    }
                }
            }

            #endregion

            return Result.True;
        }

        /// <summary>
        /// 同物料狀況:需先更新Barcode用於下筆同物料判斷
        /// </summary>
        /// <inheritdoc/>
        public static void UpdateSameFtyBarcode(DataTable dt, WHBarcodeTransaction item, string ft, string barcode, bool isLocalOrder = false)
        {
            string ukey = string.Empty;
            switch (ft)
            {
                case "From":
                    ukey = isLocalOrder ? item.FromFabric_LocalOrderInvnetoryUkey : item.FromFabric_FtyInventoryUkey;
                    foreach (DataRow dupFtydr in dt.Select($"Fabric_FtyInventoryUkey = '{ukey}'"))
                    {
                        dupFtydr[barcode] = item.From_NewBarcode;
                        dupFtydr[barcode + "Seq"] = item.From_NewBarcodeSeq;
                    }

                    break;
                case "To":
                    ukey = isLocalOrder ? item.ToFabric_LocalOrderInventoryUkey : item.ToFabric_FtyInventoryUkey;
                    foreach (DataRow dupFtydr in dt.Select($"ToFabric_FtyInventoryUkey = '{ukey}'"))
                    {
                        dupFtydr[barcode] = item.To_NewBarcode;
                        dupFtydr[barcode + "Seq"] = item.To_NewBarcodeSeq;
                    }

                    break;
                case "Receiving":
                    ukey = isLocalOrder ? item.ToFabric_LocalOrderInventoryUkey : item.ToFabric_FtyInventoryUkey;
                    foreach (DataRow dupFtydr in dt.Select($"Fabric_FtyInventoryUkey = '{ukey}'"))
                    {
                        dupFtydr["Barcode"] = item.To_NewBarcode;
                        dupFtydr["BarcodeSeq"] = item.To_NewBarcodeSeq;
                    }

                    break;
            }
        }

        /// <inheritdoc/>
        public static string UpdateWHBarcodeTransaction()
        {
            return $@"
alter table #tmp alter column [Function] [varchar](4)
alter table #tmp alter column [TransactionID] [varchar](13)
alter table #tmp alter column [TransactionUkey] [bigint]
alter table #tmp alter column [Action] [varchar](10)
alter table #tmp alter column [FromFabric_FtyInventoryUkey] bigint null
alter table #tmp alter column [ToFabric_FtyInventoryUkey] bigint null
alter table #tmp alter column [From_OldBarcode] [varchar](255)
alter table #tmp alter column [From_OldBarcodeSeq] [varchar](10)
alter table #tmp alter column [From_NewBarcode] [varchar](255)
alter table #tmp alter column [From_NewBarcodeSeq] [varchar](10)
alter table #tmp alter column [To_OldBarcode] [varchar](255)
alter table #tmp alter column [To_OldBarcodeSeq] [varchar](10)
alter table #tmp alter column [To_NewBarcode] [varchar](255)
alter table #tmp alter column [To_NewBarcodeSeq] [varchar](10)
alter table #tmp alter column [FromFabric_LocalOrderInvnetoryUkey] [bigint]
alter table #tmp alter column [ToFabric_LocalOrderInventoryUkey] [bigint]

merge WHBarcodeTransaction as t
using #tmp as s 
	on t.[Function] = s.[Function]
	and t.TransactionID = s.TransactionID
	and t.TransactionUkey = s.TransactionUkey
	and t.Action = s.Action
when matched then
    update set
		[CommitTime] = getdate()
		,[FromFabric_FtyInventoryUkey]   = s.[FromFabric_FtyInventoryUkey]
        ,[FromFabric_LocalOrderInvnetoryUkey] = s.[FromFabric_LocalOrderInvnetoryUkey]
		,[From_OldBarcode]			   = s.[From_OldBarcode]
		,[From_OldBarcodeSeq]		   = s.[From_OldBarcodeSeq]
		,[From_NewBarcode]			   = s.[From_NewBarcode]
		,[From_NewBarcodeSeq]		   = s.[From_NewBarcodeSeq]
		,[ToFabric_FtyInventoryUkey]   = s.[ToFabric_FtyInventoryUkey]
        ,[ToFabric_LocalOrderInventoryUkey] = s.[ToFabric_LocalOrderInventoryUkey]
		,[To_OldBarcode]			   = s.[To_OldBarcode]
		,[To_OldBarcodeSeq]			   = s.[To_OldBarcodeSeq]
		,[To_NewBarcode]			   = s.[To_NewBarcode]
		,[To_NewBarcodeSeq]			   = s.[To_NewBarcodeSeq]
when not matched then
	INSERT
		([Function]
		,[TransactionID]
		,[TransactionUkey]
		,[Action]
		,[CommitTime]
		,[FromFabric_FtyInventoryUkey]
		,[From_OldBarcode]
		,[From_OldBarcodeSeq]
		,[From_NewBarcode]
		,[From_NewBarcodeSeq]
		,[ToFabric_FtyInventoryUkey]
		,[To_OldBarcode]
		,[To_OldBarcodeSeq]
		,[To_NewBarcode]
		,[To_NewBarcodeSeq]
        ,[FromFabric_LocalOrderInvnetoryUkey]
        ,[ToFabric_LocalOrderInventoryUkey]
    )
	values
		(s.[Function]
		,s.[TransactionID]
		,s.[TransactionUkey]
		,s.[Action]
		,getdate()
		,s.[FromFabric_FtyInventoryUkey]
		,s.[From_OldBarcode]
		,s.[From_OldBarcodeSeq]
		,s.[From_NewBarcode]
		,s.[From_NewBarcodeSeq]
		,s.[ToFabric_FtyInventoryUkey]
		,s.[To_OldBarcode]
		,s.[To_OldBarcodeSeq]
		,s.[To_NewBarcode]
		,s.[To_NewBarcodeSeq]
        ,s.[FromFabric_LocalOrderInvnetoryUkey]
        ,s.[ToFabric_LocalOrderInventoryUkey]
    );
";
        }

        /// <inheritdoc/>
        public static string UpdateFtyInventoryBarCode(bool isLocal)
        {
            string strTableName = isLocal ? "LocalOrderInventory" : "FtyInventory";
            return $@"
alter table #tmp alter column poid varchar(20)
alter table #tmp alter column seq1 varchar(3)
alter table #tmp alter column seq2 varchar(3)
alter table #tmp alter column stocktype varchar(1)
alter table #tmp alter column roll varchar(15)
alter table #tmp alter column Dyelot varchar(15)
alter table #tmp alter column Barcode varchar(255)

update t set
    t.Barcode = s.Barcode,
    t.BarcodeSeq = s.BarcodeSeq
from {strTableName} t
inner join #tmp s on t.POID = s.poid
    and t.Seq1 = s.seq1  and t.Seq2 = s.seq2
    and t.StockType = s.stocktype 
    and t.Roll = s.roll 
    and t.Dyelot = s.Dyelot";
        }

        /// <inheritdoc/>
        public static WHTableName GetWHDetailTableName(string function)
        {
            switch (function)
            {
                case "P07":
                case "P08":
                    return WHTableName.Receiving_Detail;
                case "P18":
                    return WHTableName.TransferIn_Detail;
                case "P17":
                    return WHTableName.IssueReturn_Detail;
                case "P37":
                    return WHTableName.ReturnReceipt_Detail;
                case "P10":
                case "P11":
                case "P12":
                case "P13":
                case "P33":
                case "P62":
                    return WHTableName.Issue_Detail;
                case "P15":
                case "P16":
                    return WHTableName.IssueLack_Detail;
                case "P19":
                    return WHTableName.TransferOut_Detail;
                case "P22":
                case "P23":
                case "P24":
                case "P25":
                case "P28":
                case "P29":
                case "P30":
                case "P36":
                    return WHTableName.SubTransfer_Detail;
                case "P31":
                case "P32":
                    return WHTableName.BorrowBack_Detail;
                case "P34":
                case "P35":
                case "P43":
                case "P45":
                case "P48":
                    return WHTableName.Adjust_Detail;
                case "P50":
                case "P51":
                    return WHTableName.Stocktaking_Detail;
                case "P21":
                case "P26":
                    return WHTableName.LocationTrans_Detail;
                case "P70":
                    return WHTableName.LocalOrderReceiving_Detail;
                case "P71":
                    return WHTableName.LocalOrderIssue_Detail;
                case "P72":
                    return WHTableName.LocalOrderAdjust_Detail;
                case "P73":
                    return WHTableName.LocalOrderLocationTrans_Detail;
                default:
                    return WHTableName.DefaultError;
            }
        }

        /// <inheritdoc/>
        public static string GetWHMainTableName(string function)
        {
            return GetWHDetailTableName(function).ToString().Replace("_Detail", string.Empty);
        }

        private class NowDetail
        {
            public string POID { get; set; }

            public string Seq1 { get; set; }

            public string Seq2 { get; set; }

            public List<string> DB_CLocations { get; set; }
        }

        /// <inheritdoc/>
        public static DualResult UpdateFtyInventoryMDivisionPoDetail(IList<DataRow> detailDatas)
        {
            StringBuilder sqlupd2 = new StringBuilder();
            List<NowDetail> nowDetails = new List<NowDetail>();
            DualResult result; // , result2;
            string upd_MD_2T = string.Empty;
            string upd_Fty_26F = string.Empty;

            // 先把表身POID Seq1 2原本的MDivisionPoDetail CLocation記下來  ISP20191578
            foreach (DataRow item in detailDatas.Where(o => o["StockType"].ToString() == "O" && o["ToLocation"].ToString() != string.Empty).ToList())
            {
                string pOID = item["POID"].ToString();
                string seq11 = item["Seq"].ToString().Split(' ')[0];
                string seq21 = item["Seq"].ToString().Split(' ')[1];

                DataTable dT_MDivisionPoDetail;

                // 從MDivisionPoDetail出現有的Location
                string c = $@"
SELECt CLocation
FROM MDivisionPoDetail
WHERE POID='{pOID}'
AND Seq1='{seq11}' AND Seq2='{seq21}'
";
                DBProxy.Current.Select("Production", c, out dT_MDivisionPoDetail);

                List<string> dB_CLocations = dT_MDivisionPoDetail.Rows[0]["CLocation"].ToString().Split(',').Where(o => o != string.Empty).ToList();

                NowDetail nData = new NowDetail()
                {
                    POID = pOID,
                    Seq1 = seq11,
                    Seq2 = seq21,
                    DB_CLocations = dB_CLocations,
                };
                nowDetails.Add(nData);
            }

            #region 更新庫存數量 ftyinventory

            var data_Fty_26F = (from b in detailDatas
                                select new
                                {
                                    poid = b.Field<string>("poid"),
                                    seq1 = b.Field<string>("seq1"),
                                    seq2 = b.Field<string>("seq2"),
                                    stocktype = b.Field<string>("stocktype"),
                                    qty = b.Field<decimal>("qty"),
                                    toLocation = b.Field<string>("ToLocation"),
                                    roll = b.Field<string>("roll"),
                                    dyelot = b.Field<string>("dyelot"),
                                }).ToList();

            upd_Fty_26F = UpdateFtyInventory_IO(26, null, false);
            #endregion 更新庫存數量 po_supp_detail & ftyinventory

            #region 更新庫存數量 mdivisionPoDetail
            var data_MD_2T = (from b in detailDatas
                              group b by new
                              {
                                  poid = b.Field<string>("poid"),
                                  seq1 = b.Field<string>("seq1"),
                                  seq2 = b.Field<string>("seq2"),
                                  stocktype = b.Field<string>("stocktype"),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("poid"),
                                  Seq1 = m.First().Field<string>("seq1"),
                                  Seq2 = m.First().Field<string>("seq2"),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("ToLocation")).Distinct()),
                                  Qty = 0,
                                  Stocktype = m.First().Field<string>("stocktype"),
                              }).ToList();
            #endregion

            #region ISP20191578 ToLocation的資料一併更新回MDivisionPODetail.CLocation欄位
            string updateMDivisionPODetailCLocation = string.Empty;
            try
            {
                foreach (DataRow item in detailDatas.Where(o => o["StockType"].ToString() == "O" && o["ToLocation"].ToString() != string.Empty))
                {
                    string pOID = item["POID"].ToString();
                    string seq11 = item["Seq"].ToString().Split(' ')[0];
                    string seq21 = item["Seq"].ToString().Split(' ')[1];

                    List<string> new_CLocationList = detailDatas.Where(o => o["POID"].ToString() == pOID && o["Seq"].ToString() == (seq11 + " " + seq21) && o["ToLocation"].ToString() != string.Empty)
                        .Select(o => o["ToLocation"].ToString())
                        .Distinct().ToList();

                    // List<string> DB_CLocations = DT_MDivisionPoDetail.Rows[0]["CLocation"].ToString().Split(',').Where(o => o != "").ToList();
                    List<string> dB_CLocations = nowDetails.Where(o => o.POID == pOID && o.Seq1 == seq11 && o.Seq2 == seq21).FirstOrDefault().DB_CLocations;

                    List<string> fincal = new List<string>();

                    foreach (var new_CLocation in new_CLocationList)
                    {
                        if (dB_CLocations.Count == 0 || !dB_CLocations.Contains(new_CLocation))
                        {
                            dB_CLocations.Add(new_CLocation);
                        }
                    }

                    foreach (var cLocation in dB_CLocations.Distinct().ToList())
                    {
                        foreach (var a in cLocation.Split(',').Where(o => o != string.Empty).Distinct().ToList())
                        {
                            if (!fincal.Contains(a))
                            {
                                fincal.Add(a);
                            }
                        }
                    }

                    string cmd = $@"
UPDATE MDivisionPoDetail
SET CLocation='{fincal.Distinct().ToList().JoinToString(",")}'
WHERE POID='{pOID}' AND Seq1='{seq11}' AND Seq2='{seq21}'

";
                    updateMDivisionPODetailCLocation += cmd;
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }
            #endregion

            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (sqlConn)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    DataTable resulttb;
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_26F, string.Empty, upd_Fty_26F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        return result;
                    }
                    #endregion

                    #region MDivisionPoDetail

                    upd_MD_2T = UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, string.Empty, upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        return result;
                    }
                    #endregion

                    if (!MyUtility.Check.Empty(updateMDivisionPODetailCLocation))
                    {
                        result = DBProxy.Current.Execute(null, updateMDivisionPODetailCLocation);

                        if (!result)
                        {
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new DualResult(false, ex);
                }
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        public static bool ChkWMSCompleteTime(DataTable dtDetail, string keyType)
        {
            if (!IsAutomation() || MyUtility.Check.Empty(dtDetail) || MyUtility.Check.Empty(keyType))
            {
                return true;
            }

            string sqlcmd = string.Empty;
            string errmsg = string.Empty;
            DualResult result;
            DataTable dt;
            switch (keyType)
            {
                case "Receiving_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.Receiving_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "TransferIn_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.TransferIn_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "TransferOut_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.TransferOut_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "IssueLack_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.IssueLack_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "Issue_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.Issue_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "Adjust_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.Adjust_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "SubTransfer_Detail_To":
                    sqlcmd = $@"
Select 
 [poid] = d.topoid
,[seq1] = d.toseq1
,[seq2] = d.toseq2
,[Roll] = d.toRoll
,[Dyelot] = d.toDyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "SubTransfer_Detail_From":
                    sqlcmd = $@"
Select 
 [poid] = d.Frompoid
,[seq1] = d.Fromseq1
,[seq2] = d.Fromseq2
,[Roll] = d.FromRoll
,[Dyelot] = d.FromDyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "ReturnReceipt_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.returnreceipt_Detail d WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "BorrowBack_Detail_To":
                    sqlcmd = $@"
Select  
 [poid] = d.topoid
,[seq1] = d.toseq1
,[seq2] = d.toseq2
,[Roll] = d.toRoll
,[Dyelot] = d.toDyelot
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "BorrowBack_Detail_From":
                    sqlcmd = $@"
Select  
 [poid] = d.Frompoid
,[seq1] = d.Fromseq1
,[seq2] = d.Fromseq2
,[Roll] = d.FromRoll
,[Dyelot] = d.FromDyelot
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;
                case "IssueReturn_Detail":
                    sqlcmd = $@"
Select  
 [poid] = d.poid
,[seq1] = d.seq1
,[seq2] = d.seq2
,[Roll] = d.Roll
,[Dyelot] = d.Dyelot
from dbo.IssueReturn_Detail d WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;
                case "LocalOrderReceiving_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.LocalOrderReceiving_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;
                case "LocalOrderIssue_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.LocalOrderIssue_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;
                case "LocalOrderAdjust_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.LocalOrderAdjust_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetail, string.Empty, sqlcmd, out dt)))
            {
                MyUtility.Msg.WarningBox(result.Messages.ToString());
                return false;
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    Class.MsgGridPrg form = new Class.MsgGridPrg(dt, "WMS system have finished it already, you cannot unconfirm it.");
                    form.Width = 650;
                    form.ShowDialog();

                    //foreach (DataRow tmp in dt.Rows)
                    //{
                    //    errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}." + Environment.NewLine;
                    //}

                    //MyUtility.Msg.WarningBox("WMS system have finished it already, you cannot unconfirm it." + Environment.NewLine + errmsg, "Warning");
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool ChkWMSLock(string id, string keyType)
        {
            if (!IsAutomation() || MyUtility.Check.Empty(id) || MyUtility.Check.Empty(keyType))
            {
                return true;
            }

            string sqlcmd = string.Empty;
            string errmsg = string.Empty;
            DualResult result;
            DataTable dt;
            switch (keyType)
            {
                case "Receiving_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.StockQty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Receiving_Detail d  WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.PoId = f.POID 
    and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 
    and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot 
where f.WMSLock = 1 
and d.Id = '{id}'";
                    break;
                case "Issue_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.POID = f.POID  
    and D.StockType = F.StockType
    and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.WMSLock = 1 
and d.Id = '{id}'
";
                    break;

                case "IssueLack_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.IssueLack_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.poid = f.POID 
    and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 
    and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.WMSLock = 1 
and d.Id = '{id}'
";
                    break;

                case "IssueReturn_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.IssueReturn_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.poid = f.POID 
    and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 
    and d.StockType = f.StockType and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.WMSLock = 1
and d.Id = '{id}'
";
                    break;

                case "TransferIn_Detail":
                    sqlcmd = $@"
Select d.poid, d.seq1, d.seq2, d.Roll, d.Qty
, balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0),f.Dyelot
from dbo.TransferIn_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.PoId = f.PoId
    and d.Seq1 = f.Seq1 and d.Seq2 = f.seq2 and d.StockType = f.StockType
    and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.WMSLock = 1
and d.Id = '{id}'
";
                    break;

                case "TransferOut_Detail":
                    sqlcmd = $@"
Select d.poid, d.seq1, d.seq2, d.Roll, d.Qty
, balanceQty = isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0),f.Dyelot
from dbo.TransferOut_Detail d WITH(NOLOCK)
inner join FtyInventory f WITH(NOLOCK) on d.PoId = f.PoId
    and d.Seq1 = f.Seq1 and d.Seq2 = f.seq2 and d.StockType = f.StockType
    and d.Roll = f.Roll and d.Dyelot = f.Dyelot
where f.WMSLock = 1
and d.Id = '{id}'
";
                    break;

                case "SubTransfer_Detail_To":
                    sqlcmd = $@"
Select 
 [poid] = d.topoid
,[seq1] = d.toseq1
,[seq2] = d.toseq2
,[Roll] = d.toRoll
,[Dyelot] = d.toDyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.toPoId = f.PoId
    and d.toSeq1 = f.Seq1 and d.toSeq2 = f.seq2 and d.toStocktype = f.StockType
    and d.toRoll = f.Roll and d.toDyelot = f.Dyelot 
where f.WMSLock = 1 
and d.Id = '{id}'
";
                    break;

                case "SubTransfer_Detail_From":
                    sqlcmd = $@"
Select  
 [poid] = d.frompoid 
,[seq1] = d.fromseq1
,[seq2] = d.fromseq2
,[Roll] = d.fromRoll
,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK, INDEX(MdID_POSeq)) on d.FromPOID = f.POID 
    and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 
    and D.FromStockType = F.StockType and d.FromDyelot = f.Dyelot
where f.WMSLock = 1 
and d.Id = '{id}'
";
                    break;

                case "BorrowBack_Detail_From":
                    sqlcmd = $@"
Select  
 [poid] = d.frompoid 
,[seq1] = d.fromseq1
,[seq2] = d.fromseq2
,[Roll] = d.fromRoll
,f.Dyelot
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID  
    AND D.FromStockType = F.StockType and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 
    and d.FromSeq2 = f.Seq2 and d.fromDyelot = f.Dyelot
where f.WMSLock = 1 
and d.Id = '{id}'";
                    break;

                case "BorrowBack_Detail_To":
                    sqlcmd = $@"
Select  
 [poid] = d.topoid
,[seq1] = d.toseq1
,[seq2] = d.toseq2
,[Roll] = d.toRoll
,[Dyelot] = d.toDyelot
from dbo.BorrowBack_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.toPoId = f.PoId 
    and d.toSeq1 = f.Seq1 and d.toSeq2 = f.seq2
    and d.toStocktype = f.StockType and d.toRoll = f.Roll and d.toDyelot = f.Dyelot
where f.WMSLock = 1 
and d.Id = '{id}'";
                    break;

                case "Issue_Summary":
                    sqlcmd = $@"
SELECT d.POID,d.Seq1,d.Seq2,d.Roll,d.Dyelot
FROM Issue i
INNER JOIN Issue_Summary s ON i.ID = s.ID 
INNER JOIN Issue_Detail d ON s.id=d.id AND s.Ukey = d.Issue_SummaryUkey
INNER JOIN FtyInventory f ON f.POID=s.Poid AND f.Seq1=d.Seq1 AND f.Seq2=d.Seq2
INNER JOIN PO_Supp_Detail psd ON psd.ID = s.Poid 
    AND psd.SCIRefno = s.SCIRefno AND psd.SCIRefno = s.SCIRefno AND psd.SEQ1=d.Seq1 AND psd.Seq2=d.Seq2
WHERE i.Id = '{id}' 
AND  f.WMSLock = 1 ";
                    break;
                case "Adjust_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,isnull(d.QtyAfter,0.00) - isnull(d.QtyBefore,0.00) qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Adjust_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.POID = f.POID  
    AND D.StockType = F.StockType
    and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot
where f.WMSLock = 1 
and d.Id = '{id}'";
                    break;

                case "ReturnReceipt_Detail":
                    sqlcmd = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.returnreceipt_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.PoId = f.POID 
    and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 
    and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot
where f.WMSLock = 1 
and d.Id = '{id}'";
                    break;
            }

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out dt)))
            {
                MyUtility.Msg.WarningBox(result.Messages.ToString());
                return false;
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow tmp in dt.Rows)
                    {
                        errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked cause from WMS system not received below material yet." + Environment.NewLine + errmsg, "Warning");
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool SentToWMS(DataTable dtDetail, bool isConfirmed, string formName, string ukeys = "")
        {
            WHTableName detailTableName = GetWHDetailTableName(formName);
            if (ukeys == string.Empty)
            {
                ukeys = dtDetail.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["Ukey"])).ToList().JoinToString(",");
            }

            string sqlcmd = $@"
update {detailTableName}
set SentToWMS = {(isConfirmed ? 1 : 0)}
where Ukey in ({ukeys})";

            DualResult result = DBProxy.Current.Execute("Production", sqlcmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// check WMS, non-WMS location in the same material
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="functionName">functionName</param>
        /// <returns>bool</returns>
        public static bool Chk_WMS_Location(string id, string functionName)
        {
            if (!IsAutomation() || MyUtility.Check.Empty(id) || MyUtility.Check.Empty(functionName))
            {
                return true;
            }

            string sqlcmd = string.Empty;
            DualResult result;
            DataTable dt;
            DataTable[] dts;
            string errmsg = string.Empty;
            switch (functionName)
            {
                case "P22":
                case "P23":
                case "P24":
                case "P28":
                case "P29":
                case "P30":
                case "P36":
                case "P31":
                case "P32":
                case "P07":
                case "P08":
                case "P18":
                    sqlcmd = $@"
declare @ID varchar(15) = '{id}'

select * from(

-- SubTransfer_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from SubTransfer_Detail t
		inner join FtyInventory f on t.FromPOID = f.POID
			and t.FromSeq1= f. Seq1 
			and t.FromSeq2 = f.Seq2 
			and t.FromRoll = f.Roll
			and t.FromDyelot = f.Dyelot 
			and t.FromStockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

union all

-- BorrowBack_Detail
select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from SubTransfer_Detail t
		inner join FtyInventory f on t.FromPOID = f.POID
			and t.FromSeq1= f. Seq1 
			and t.FromSeq2 = f.Seq2 
			and t.FromRoll = f.Roll
			and t.FromDyelot = f.Dyelot 
			and t.FromStockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

union all

-- Receiving_Detail
	select  * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from Receiving_Detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
					select 	distinct
						MtlLocationID 
					from dbo.FtyInventory_Detail d
					where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

union all 

-- TransferIn_Detail
	select  * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from TransferIn_Detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
					select 	distinct
						MtlLocationID
					from dbo.FtyInventory_Detail d
					where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a
) final
where rowCnt =2


select *
from(

-- SubTransfer_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct POID = t.ToPOID,Seq1 = t.ToSeq1,Seq2 = t.ToSeq2,Roll = t.ToRoll,Dyelot = t.ToDyelot,IsWMS = isnull( ml.IsWMS,0),Location = t.ToLocation
		from SubTransfer_Detail t
		outer apply(
			select ml.IsWMS
			from MtlLocation ml
			inner join dbo.SplitString(t.ToLocation,',') sp on sp.Data = ml.ID
		)ml
		where t.id=@ID
	) a

union all

-- BorrowBack_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct POID = t.ToPOID,Seq1 = t.ToSeq1,Seq2 = t.ToSeq2,Roll = t.ToRoll,Dyelot = t.ToDyelot,IsWMS = isnull( ml.IsWMS,0),Location = t.ToLocation
		from BorrowBack_Detail t
		outer apply(
			select ml.IsWMS
			from MtlLocation ml
			inner join dbo.SplitString(t.ToLocation,',') sp on sp.Data = ml.ID
		)ml
		where t.id=@ID
	) a

union all

-- Receiving_Detail
	select  * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from Receiving_Detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		outer apply(
			select ml.IsWMS
			from MtlLocation ml
			inner join (
				select 	distinct  MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
						union all
						select distinct sp.Data as MtlLocationID
						from dbo.SplitString(t.Location,',') sp 
			) sp on sp.MtlLocationID = ml.ID
		)ml
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
					select distinct MtlLocationID 
					from(
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
						union all
						select distinct sp.Data as MtlLocationID
						from dbo.SplitString(t.Location,',') sp 
						) s1
					) s
				for xml path ('')
			) , 1, 1, '')
		) s	
		where t.id=@ID
	) a


union all

-- TransferIn_Detail
	select  * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from TransferIn_Detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		outer apply(
			select ml.IsWMS
			from MtlLocation ml
			inner join (
				select 	distinct  MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
						union all
						select distinct sp.Data as MtlLocationID
						from dbo.SplitString(t.Location,',') sp 
			) sp on sp.MtlLocationID = ml.ID
		)ml
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
					select distinct MtlLocationID 
					from(
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
						union all
						select distinct sp.Data as MtlLocationID
						from dbo.SplitString(t.Location,',') sp 
						) s1
					) s
				for xml path ('')
			) , 1, 1, '')
		) s	
		where t.id=@ID
	) a


) final
where rowCnt =2

";

                    if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd, out dts)))
                    {
                        MyUtility.Msg.WarningBox(result.Messages.ToString());
                        return false;
                    }
                    else
                    {
                        if (dts[0] != null && dts[0].Rows.Count > 0)
                        {
                            foreach (DataRow tmp in dts[0].Rows)
                            {
                                errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} Location: {tmp["Location"]}" + Environment.NewLine;
                            }

                            MyUtility.Msg.WarningBox("These material exists in WMS Location and non-WMS location in same time , please use W/H P26 to correct these material location." + Environment.NewLine + errmsg, "Warning");
                            return false;
                        }
                        else if (dts[1] != null && dts[1].Rows.Count > 0)
                        {
                            foreach (DataRow tmp in dts[1].Rows)
                            {
                                errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} Location: {tmp["Location"]}" + Environment.NewLine;
                            }

                            MyUtility.Msg.WarningBox("These material exists in WMS Location and non-WMS location in same time , please revise below detail location column data." + Environment.NewLine + errmsg, "Warning");
                            return false;
                        }
                    }

                    break;

                case "P70":
                case "P71":
                case "P72":
                    sqlcmd = $@"

declare @ID varchar(15) = '{id}'

select * from(

	-- P70 LocalOrderReceiving_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct loi.POID,loi.Seq1,loi.Seq2,loi.Roll,loi.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from LocalOrderReceiving_Detail t
		inner join LocalOrderInventory loi on t.POID = loi.POID
			and t.Seq1= loi. Seq1 and t.Seq2 = loi.Seq2 and t.Roll = loi.Roll
			and t.Dyelot = loi.Dyelot and t.StockType = loi.StockType
		left join LocalOrderInventory_Location loil on loil.LocalOrderInventoryUkey = loi.Ukey
		left join MtlLocation ml on ml.ID = loil.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.LocalOrderInventory_Location d
						where d.LocalOrderInventoryUkey = loi.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

	union ALL

	-- P71 LocalOrderIssue_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct loi.POID,loi.Seq1,loi.Seq2,loi.Roll,loi.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from LocalOrderIssue_Detail t
		inner join LocalOrderInventory loi on t.POID = loi.POID
			and t.Seq1= loi. Seq1 and t.Seq2 = loi.Seq2 and t.Roll = loi.Roll
			and t.Dyelot = loi.Dyelot and t.StockType = loi.StockType
		left join LocalOrderInventory_Location loil on loil.LocalOrderInventoryUkey = loi.Ukey
		left join MtlLocation ml on ml.ID = loil.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.LocalOrderInventory_Location d
						where d.LocalOrderInventoryUkey = loi.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

	union ALL

	-- P72 LocalOrderAdjust_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct loi.POID,loi.Seq1,loi.Seq2,loi.Roll,loi.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from LocalOrderAdjust_Detail t
		inner join LocalOrderInventory loi on t.POID = loi.POID
			and t.Seq1= loi. Seq1 and t.Seq2 = loi.Seq2 and t.Roll = loi.Roll
			and t.Dyelot = loi.Dyelot and t.StockType = loi.StockType
		left join LocalOrderInventory_Location loil on loil.LocalOrderInventoryUkey = loi.Ukey
		left join MtlLocation ml on ml.ID = loil.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.LocalOrderInventory_Location d
						where d.LocalOrderInventoryUkey = loi.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

) final
where rowCnt =2
";

                    if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd, out dt)))
                    {
                        MyUtility.Msg.WarningBox(result.Messages.ToString());
                        return false;
                    }
                    else
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow tmp in dt.Rows)
                            {
                                errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} Location: {tmp["Location"]}" + Environment.NewLine;
                            }

                            MyUtility.Msg.WarningBox("These material exists in WMS Location and non-WMS location in same time , please use W/H P73 to correct these material location." + Environment.NewLine + errmsg, "Warning");
                            return false;
                        }
                    }

                    break;

                default:
                    sqlcmd = $@"

declare @ID varchar(15) = '{id}'

select * from(

	-- Issue_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from Issue_Detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

	union all

	-- Adjust_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from Adjust_Detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a


	union all 

	-- ReturnReceipt_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from ReturnReceipt_Detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

	union all 

	-- IssueReturn_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from IssueReturn_Detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

	union all

	-- Stocktaking_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from Stocktaking_Detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

	union all

	-- IssueLack_Detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from IssueLack_Detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

	union all

	--TransferOut_detail
	select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,Location order by IsWMS)
	from (
		select distinct f.POID,f.Seq1,f.Seq2,f.Roll,f.Dyelot,IsWMS = isnull( ml.IsWMS,0),s.Location
		from TransferOut_detail t
		inner join FtyInventory f on t.POID = f.POID
			and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
			and t.Dyelot = f.Dyelot and t.StockType = f.StockType
		left join FtyInventory_Detail fd on fd.Ukey = f.Ukey
		left join MtlLocation ml on ml.ID = fd.MtlLocationID
		outer apply(
			select Location = Stuff((
				select concat(',',MtlLocationID)
				from (
						select 	distinct
							MtlLocationID
						from dbo.FtyInventory_Detail d
						where d.Ukey = f.Ukey
					) s
				for xml path ('')
			) , 1, 1, '')
		) s
		where t.id=@ID
	) a

) final
where rowCnt =2
";

                    if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd, out dt)))
                    {
                        MyUtility.Msg.WarningBox(result.Messages.ToString());
                        return false;
                    }
                    else
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow tmp in dt.Rows)
                            {
                                errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} Location: {tmp["Location"]}" + Environment.NewLine;
                            }

                            MyUtility.Msg.WarningBox("These material exists in WMS Location and non-WMS location in same time , please use W/H P26 to correct these material location." + Environment.NewLine + errmsg, "Warning");
                            return false;
                        }
                    }

                    break;
            }

            return true;
        }

        /// <summary>
        /// check WMS, non-WMS location in the same material by Adjust
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <returns>bool</returns>
        public static bool Chk_WMS_Location_Adj(DataTable dtDetail, bool isLocal = false)
        {
            if (!IsAutomation() || MyUtility.Check.Empty(dtDetail) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            string srtTable = isLocal ? "LocalOrderInventory" : "FtyInventory";
            string srtTable2 = isLocal ? "left join LocalOrderInventory_Location fd on fd.LocalOrderInventoryUkey = f.Ukey" : "left join FtyInventory_Detail fd on fd.Ukey = f.Ukey";

            string sqlcmd = $@"
select f.* 
from #tmp t
inner join {srtTable} f on t.POID = f.POID
	and t.Seq1= f. Seq1 and t.Seq2 = f.Seq2 and t.Roll = f.Roll
	and t.Dyelot = f.Dyelot and t.StockType = f.StockType
{srtTable2}
left join MtlLocation ml on ml.ID = fd.MtlLocationID
where 1=1
and ml.IsWMS = 1
";
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetail, string.Empty, sqlcmd, out DataTable dt)))
            {
                MyUtility.Msg.WarningBox(result.Messages.ToString());
                return false;
            }
            else
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 自動倉儲
        /// </summary>
        /// <returns>bool</returns>
        public static bool IsAutomation()
        {
            return MyUtility.Check.Seek("select 1 from dbo.System where Automation = 1", "Production");
        }

        public static string GetInventoryTableName(bool isLocal)
        {
            if (isLocal)
            {
                return "LocalOrderInventory";
            }
            else
            {
                return "FtyInventory";
            }
        }

        /// <summary>
        /// 帶入 barcode 從 WHBarcodeTransaction 取得新的 BarcodeSeq (2碼)
        /// </summary>
        /// <param name="barcode">barcode</param>
        /// <returns>Next BarcodeSeq</returns>
        public static string GetNextBarcodeSeq(string barcode)
        {
            string sqlcmd = $@"select dbo.GetWH_NextBarcodeSeq('{barcode}')";
            return MyUtility.GetValue.Lookup(sqlcmd, "Production");
        }

        /// <summary>
        /// 狀況 :有同一物料多筆在 (obj) WHBarcodeTransaction 內
        /// 同一物料有 2 筆以上, 每筆 BarcodeSeq 要不一樣
        /// 帶入 barcode 從 物件 WHBarcodeTransaction 取得新的 BarcodeSeq (2碼)
        /// </summary>
        /// <param name="barcode">barcode</param>
        /// <param name="wHBarcodeTransaction">wHBarcodeTransaction</param>
        /// <param name="column">From or To</param>
        /// <returns>Next BarcodeSeq</returns>
        public static string GetNextBarcodeSeqInObjWHBarcodeTransaction(string barcode, List<WHBarcodeTransaction> wHBarcodeTransaction, string column)
        {
            string seq;
            if (column == "To")
            {
                seq = wHBarcodeTransaction.Where(w => w.To_NewBarcode == barcode).Select(s => s.To_NewBarcodeSeq).Max();
            }
            else
            {
                seq = wHBarcodeTransaction.Where(w => w.From_NewBarcode == barcode).Select(s => s.From_NewBarcodeSeq).Max();
            }

            if (MyUtility.Check.Empty(seq))
            {
                return GetNextBarcodeSeq(barcode);
            }

            int intSeq = MyUtility.Convert.GetInt(seq) + 1;
            return intSeq.ToString();
        }

        /// <inheritdoc/>
        public static bool NoGensong(string formName)
        {
            switch (formName)
            {
                case "P11":
                case "P12":
                case "P33":
                case "P15":
                case "P43":
                case "P45":
                case "P48":
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool NoVstrong(string formName)
        {
            switch (formName)
            {
                case "P07_ModifyRollDyelot":
                case "P10":
                case "P62":
                case "P16":
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查QRCode是否為PMS自動建立
        /// </summary>
        /// <param name="checkQRCode">checkQRCode</param>
        /// <returns>bool</returns>
        public static bool IsQRCodeCreatedByPMS(this string checkQRCode)
        {
            if (Regex.IsMatch(checkQRCode, "^([a-zA-Z0-9]{3}F|F)[0-9]{12}"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// WH P07 與 P18
        /// 修改資料時新增檢查
        /// 查該捲布是否已經有完成 Shadeband 的檢驗
        /// 如果有完成檢驗則不允許 UnConfirm 或者是 Modify Roll, Dyelot
        /// # 請注意此次調整皆只針對主料 Fabric
        /// </summary>
        /// <inheritdoc/>
        public static bool CheckShadebandResult(string function, string id)
        {
            WHTableName detailTableName = GetWHDetailTableName(function);
            string sqlcmd = $@"
select distinct
    FIR.POID,
    FIR.Seq1,
    FIR.Seq2,
    fs.Roll,
    fs.Dyelot,
    fs.Result
from {detailTableName} sd with(nolock)
inner join PO_Supp_Detail psd with(nolock) on psd.ID = sd.PoId and psd.SEQ1 = sd.Seq1 and psd.SEQ2 = sd.Seq2
inner join FIR with (nolock) on FIR.ReceivingID = sd.ID and FIR.POID = sd.PoId and FIR.SEQ1 = sd.Seq1 and FIR.SEQ2 = sd.Seq2
inner join FIR_Shadebone fs with (nolock) on fs.id = FIR.ID
where sd.id = '{id}'
and psd.FabricType = 'F'
and fs.Result <>''
";
            DualResult result = DBProxy.Current.Select("Production", sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                var m = MyUtility.Msg.ShowMsgGrid_LockScreen(dt, msg: "Those fabric roll already completed shade band inspection, please check with QA team and revise inspection result to empty before unconfirm.", caption: "Warring");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 負數庫存檢查 因為(舊)資料會改,同一張單有重複物料狀況,所以要加總 Adjustqty 計算
        /// </summary>
        /// <inheritdoc/>
        public static DualResult GetAdjustSumBalance(string id, bool isConfirm, out DataTable datacheck, bool isLocalOrder = false)
        {
            string chksql = string.Empty;
            if (isLocalOrder)
            {
                chksql = $@"
select x.*
from(
    Select
        a.POID,
        SEQ = concat(a.Seq1, '-',  a.Seq2),
        a.Roll,
        a.Dyelot,
	    BalanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0),
	    Adjustqty  = Sum(isnull(a.QtyAfter,0) - isnull(a.QtyBefore,0))
    from dbo.LocalOrderAdjust_Detail a WITH (NOLOCK) 
    inner join LocalOrderInventory f WITH (NOLOCK) on a.POID = f.POID and a.Roll = f.Roll and a.Seq1 =f.Seq1 and a.Seq2 = f.Seq2 and a.Dyelot = f.Dyelot and a.stocktype = f.stocktype 
    where a.Id = '{id}'
    group by a.POID, a.Seq1, a.Seq2, a.Roll, a.Dyelot, a.StockType ,f.InQty, f.OutQty, f.AdjustQty
)x
where x.BalanceQty {(isConfirm ? "+" : "-")} x.Adjustqty < 0";
            }
            else
            {
                chksql = $@"
select x.*
from(
    Select
        a.POID,
        SEQ = concat(a.Seq1, '-',  a.Seq2),
        a.Roll,
        a.Dyelot,
	    BalanceQty = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0),
	    Adjustqty  = Sum(isnull(a.QtyAfter,0) - isnull(a.QtyBefore,0))
    from dbo.Adjust_Detail a WITH (NOLOCK) 
    inner join FtyInventory f WITH (NOLOCK) on a.POID = f.POID and a.Roll = f.Roll and a.Seq1 =f.Seq1 and a.Seq2 = f.Seq2 and a.Dyelot = f.Dyelot and a.stocktype = f.stocktype 
    where a.Id = '{id}'
    group by a.POID, a.Seq1, a.Seq2, a.Roll, a.Dyelot, a.StockType ,f.InQty, f.OutQty, f.AdjustQty, f.ReturnQty
)x
where x.BalanceQty {(isConfirm ? "+" : "-")} x.Adjustqty < 0
";
            }

            return DBProxy.Current.Select(null, chksql, out datacheck);
        }

        /// <summary>
        /// 負數庫存檢查 因為(舊)資料會改,同一張單有重複物料狀況,所以要加總 Adjustqty 計算
        /// </summary>
        /// <inheritdoc/>
        public static bool CheckAdjustBalance(string id, bool isConfirm, bool isLocalOrder = false)
        {
            DualResult result = GetAdjustSumBalance(id, isConfirm, out DataTable datacheck, isLocalOrder);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return false;
            }

            if (datacheck.Rows.Count > 0)
            {
                //MyUtility.Msg.ShowMsgGrid_LockScreen(datacheck, "Balacne Qty is not enough!!");
                Class.MsgGridPrg form = new Class.MsgGridPrg(datacheck, "Balacne Qty is not enough!!");
                form.ShowDialog();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 調整單的 confrim & unconfrim 更新庫存
        /// </summary>
        /// <inheritdoc/>
        public static DualResult UpdateScrappAdjustFtyInventory(string id, bool isConfirm)
        {
            string sign = isConfirm ? "+" : "-";
            string upcmd = $@"
declare @POID varchar(13)
		, @seq1 varchar(3)
		, @seq2 varchar(3)
		, @Roll varchar(8)
		, @Dyelot varchar(8)
		, @StockType varchar(1)
		, @AdjustQty numeric(11, 2)

DECLARE _cursor CURSOR FOR
select ad.POID, ad.Seq1, ad.Seq2, ad.Roll, ad.Dyelot, ad.StockType, [AdjustQty] = isnull(ad.QtyAfter, 0) - isnull(ad.QtyBefore, 0)
from Adjust_Detail ad
where ad.id	 = '{id}'

OPEN _cursor
FETCH NEXT FROM _cursor INTO @POID, @seq1, @seq2, @Roll, @Dyelot, @StockType, @AdjustQty
WHILE @@FETCH_STATUS = 0
BEGIN	
	update f
		set [AdjustQty] = f.AdjustQty {sign} @AdjustQty
	from FtyInventory f
	where f.POID = @POID
	and f.Seq1 = @seq1
	and f.Seq2 = @seq2
	and f.Roll = @Roll
	and f.Dyelot = @Dyelot
	and f.StockType = 'O'

	update m
		set [LObQty] = m.LObQty {sign} @AdjustQty  
	from MDivisionPoDetail m
	where m.POID = @POID
	and m.Seq1 = @seq1
	and m.Seq2 = @seq2

	FETCH NEXT FROM _cursor INTO @POID, @seq1, @seq2, @Roll, @Dyelot, @StockType, @AdjustQty
END
CLOSE _cursor
DEALLOCATE _cursor
";
            return DBProxy.Current.Execute(null, upcmd);
        }

        /// <summary>
        /// 轉料單Confirm時更新FtyInventory.Tone
        /// </summary>
        /// <param name="dt">需有欄位From & To Poid,Seq1,Seq2,Roll,Dyelot,StockType</param>
        /// <inheritdoc/>
        public static DualResult UpdateFtyInventoryTone(DataTable dt)
        {
            string sqlcmd = @"
update fto
set Tone = f.Tone
from #tmp sd
inner join FtyInventory f with(nolock) on f.POID = sd.FromPOID
    and f.Seq1 = sd.FromSeq1
    and f.Seq2 = sd.FromSeq2
    and f.Roll = sd.FromRoll
    and f.Dyelot = sd.FromDyelot
    and f.StockType = sd.FromStockType
inner join FtyInventory fto with(nolock) on fto.POID = sd.ToPOID
    and fto.Seq1 = sd.ToSeq1
    and fto.Seq2 = sd.ToSeq2
    and fto.Roll = sd.ToRoll
    and fto.Dyelot = sd.ToDyelot
    and fto.StockType = sd.ToStockType
";

            return MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sqlcmd, out DataTable odt);
        }

        /// <summary>
        /// Confirm 時計算此單 From 轉給 To 的總量後 From 的庫存剩餘庫 並將剩餘數紀錄在 SubTransfer_Detail.FromBalanceQty
        /// </summary>
        /// <inheritdoc/>
        public static DualResult UpdateSubTransfer_DetailFromBalanceQty(string id, bool isConfirm)
        {
            string updateValue = isConfirm ? "f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty" : "0";
            string sqlcmd = $@"
Update sd
Set FromBalanceQty = {updateValue}
from SubTransfer_Detail sd with(nolock) 
inner join FtyInventory f with(nolock) on f.POID = sd.FromPOID
    and f.Seq1 = sd.FromSeq1
    and f.Seq2 = sd.FromSeq2
    and f.Roll = sd.FromRoll
    and f.Dyelot = sd.FromDyelot
    and f.StockType = sd.FromStockType
where sd.id = '{id}'
";
            return DBProxy.Current.Execute(null, sqlcmd);
        }

        #region 產生Temp PO2, PO3

        /// <summary>
        /// Create Temp PO table
        /// 從Trade 複製過來
        /// </summary>
        /// <param name="sqlConn"> sql connection </param>
        /// <returns> execute success or not </returns>
        /// <inheritdoc/>
        public static DualResult CreateTmpPOTable(SqlConnection sqlConn, int updateType = 0)
        {
            var primaryKey = updateType == 0
                    ? ", Primary Key (ID, Seq1, Seq2, Seq2_Count)"
                    : ", Primary Key (ID, Seq1, Seq2, Seq2_Count, OrderID)";
            var addCol = updateType == 0
                    ? string.Empty
                    : ", OrderID VarChar(13) default '', OrderList Varchar(max) default ''";
            var addCol2Spec = updateType == 0
                    ? string.Empty
                    : ", OrderList Varchar(max) default ''";

            var sqlCmd = $@"
Create Table #tmpPO_Supp
(  RowID BigInt Identity(1,1) Not Null, ID VarChar(13), Seq1 VarChar(3), SuppID VarChar(6) default ''
    , ShipTermID VarChar(5) default '', PayTermAPID VarChar(5) default ''
    , Remark NVarChar(Max) default '', Description NVarChar(Max) default '', CompanyID Numeric(2,0) default 0
    , StyleID VarChar(15), Junk Bit, Primary Key (ID, Seq1)
);

Create Table #tmpPO_Supp_Detail
(  RowID BigInt Identity(1,1) Not Null, ID VarChar(13), Seq1 VarChar(3), Seq2 VarChar(2), RefNo VarChar(36) default '', SCIRefNo VarChar(30) default ''
    , FabricType VarChar(1) default '', Price Numeric(14,4) default 0, UsedQty Numeric(10,4) default 0, Qty Numeric(10,2) default 0
    , POUnit VarChar(8) default '', Complete Bit default 0, SystemETD Date, CFMETD Date, RevisedETD Date, FinalETD Date, EstETA Date
    , ShipModeID VarChar(10) default '', PrintDate DateTime, PINO VarChar(25) default '', PIDate Date
    , ColorID VarChar(6) default '', SuppColor NVarChar(Max) default '', SizeSpec VarChar(15) default '', SizeUnit VarChar(8) default ''
    , Remark NVarChar(Max) default '', Special NVarChar(Max) default '', Width Numeric(5,2) default 0
    , StockQty Numeric(12,1) default 0, NetQty Numeric(10,2) default 0, LossQty Numeric(10,2) default 0, SystemNetQty Numeric(10,2) default 0
    , SystemCreate bit default 0, FOC Numeric(10,2) default 0, Junk bit default 0, ColorDetail NVarChar(200) default ''
    , BomZipperInsert VarChar(5) default '', BomCustPONo VarChar(30) default ''
    , ShipQty Numeric(10,2) default 0, Shortage Numeric(10,2) default 0, ShipFOC Numeric(10,2) default 0, ApQty Numeric(10,2) default 0
    , InputQty Numeric(10,2) default 0, OutputQty Numeric(10,2) default 0, Spec NVarChar(Max) default '', ShipETA Date, SystemLock Date
    , OutputSeq1 VarChar(3) default '', OutputSeq2 VarChar(2) default '', FactoryID VarChar(8) default ''
    , StockPOID VarChar(13) default '', StockSeq1 VarChar(3) default '', StockSeq2 VarChar(2) default '', InventoryUkey bigint default 0
    , KeyWord NVarChar(Max) default '', Article varchar(8)
    , Seq2_Count Int, Remark_Shell NVarChar(Max) default ''
    , Status varchar(1), Sel bit default 0, IsForOtherBrand bit, CannotOperateStock bit, Keyword_Original varchar(max)
    {addCol}
    {primaryKey}

);

--Create Table #tmpPO_Supp_Detail_OrderList
--(  RowID BigInt Identity(1,1) Not Null, ID VarChar(13), Seq1 VarChar(3), Seq2 VarChar(2), OrderID VarChar(13), Seq2_Count Int
--    , Primary Key (ID, Seq1, Seq2, OrderID, Seq2_Count)
--);

Create Table #tmpPO_Supp_Detail_Spec
(  RowID BigInt Identity(1,1) Not Null, ID VarChar(13), Seq1 VarChar(3), Seq2 VarChar(2), SpecColumnID VarChar(50), SpecValue VarChar(50), Seq2_Count Int
    {addCol2Spec}
    , Primary Key (ID, Seq1, Seq2, SpecColumnID, Seq2_Count)
);

--Create Table #tmpPO_Supp_Detail_Keyword
--(  RowID BigInt Identity(1,1) Not Null, ID VarChar(13), Seq1 VarChar(3), Seq2 VarChar(2), KeywordField VarChar(30), KeywordValue VarChar(200), Seq2_Count Int
--    {addCol2Spec}
--    , Primary Key (ID, Seq1, Seq2, KeywordField, Seq2_Count)
--);
";

            DualResult result = DBProxy.Current.ExecuteByConn(sqlConn, sqlCmd);
            return result;
        }
        #endregion

        #region 轉出BOF至採購單

        /// <summary>
        /// 將展開後的BOF資料轉入PO
        /// 從Trade 複製過來
        /// </summary>
        /// <param name="sqlConn">sqlConn</param>
        /// <param name="poID">採購母單</param>
        /// <param name="brandID">Brand</param>
        /// <param name="programID">Program</param>
        /// <param name="category">Category</param>
        /// <param name="testType">是否為虛擬庫存計算(0: 實際寫入Table; 1: 僅傳出Temp Table; 2: 不回傳Temp Table; 3: 實際寫入Table，但不回傳Temp Table)</param>
        /// <param name="isExpendArticle"> isExpendArticle </param>
        /// <inheritdoc />
        public static DualResult TransferToPO_1_ForBOF(SqlConnection sqlConn, string poID, string brandID, string programID, string category, int testType, bool isExpendArticle)
        {
            DualResult result;
            List<SqlParameter> paras = new List<SqlParameter>();

            // String sqlCmd = "";
            paras.Add(new SqlParameter("@PoID", poID));
            paras.Add(new SqlParameter("@BrandID", brandID));
            paras.Add(new SqlParameter("@ProgramID", programID));
            paras.Add(new SqlParameter("@Category", category));
            paras.Add(new SqlParameter("@TestType", testType));
            paras.Add(new SqlParameter("@IsExpendArticle", isExpendArticle));

            result = DBProxy.Current.ExecuteSPByConn(sqlConn, "TransferToPO_1_ForBOF", paras);

            return result;
        }

        #endregion

        #region 轉出BOA至採購單

        /// <summary>
        /// 將展開後的BOA資料轉入PO
        /// 從Trade 複製過來
        /// </summary>
        /// <param name="sqlConn">sqlConn</param>
        /// <param name="poID">採購母單</param>
        /// <param name="brandID">Brand</param>
        /// <param name="programID">Program</param>
        /// <param name="category">Category</param>
        /// <param name="testType">是否為虛擬庫存計算(0: 實際寫入Table; 1: 僅傳出Temp Table; 2: 不回傳Temp Table; 3: 實際寫入Table，但不回傳Temp Table)</param>
        /// <param name="isExpendArticle"> isExpendArticle </param>
        /// <inheritdoc />
        public static DualResult TransferToPO_1_ForBOA(SqlConnection sqlConn, string poID, string brandID, string programID, string category, int testType, bool isExpendArticle)
        {
            DualResult result;
            List<SqlParameter> paras = new List<SqlParameter>();

            // String sqlCmd = "";
            paras.Add(new SqlParameter("@PoID", poID));
            paras.Add(new SqlParameter("@BrandID", brandID));
            paras.Add(new SqlParameter("@ProgramID", programID));
            paras.Add(new SqlParameter("@Category", category));
            paras.Add(new SqlParameter("@TestType", testType));
            paras.Add(new SqlParameter("@IsExpendArticle", isExpendArticle));

            result = DBProxy.Current.ExecuteSPByConn(sqlConn, "TransferToPO_1_ForBOA", paras);

            return result;
        }

        /// <summary>
        /// Project是否為ARO
        /// 從Trade 複製過來
        /// </summary>
        /// <param name="id"> sp#</param>
        /// <returns> Expend Article </returns>
        public static bool IsExpendArticle(string id)
        {
            string project = MyUtility.GetValue.Lookup($"select ProjectID from dbo.Orders where ID = '{id}'", string.Empty);
            return project == "ARO";
        }
        #endregion

        #region 轉出A大項至採購單

        /// <summary>
        /// 將外裁的A1項及QT的A2轉入PO
        /// 從Trade 複製過來
        /// </summary>
        /// <param name="sqlConn">sqlConn</param>
        /// <param name="poID">採購母單</param>
        /// <param name="brandID">Brand</param>
        /// <param name="programID">Program</param>
        /// <param name="category">Category</param>
        /// <param name="testType">是否為虛擬庫存計算(0: 實際寫入Table; 1: 僅傳出Temp Table; 2: 不回傳Temp Table; 3: 實際寫入Table，但不回傳Temp Table)</param>
        /// <inheritdoc />
        public static DualResult TransferToPO_1_ForAItem(SqlConnection sqlConn, string poID, string brandID, string programID, string category, int testType)
        {
            DualResult result;
            List<SqlParameter> paras = new List<SqlParameter>();

            // String sqlCmd = "";
            paras.Add(new SqlParameter("@PoID", poID));
            paras.Add(new SqlParameter("@BrandID", brandID));
            paras.Add(new SqlParameter("@ProgramID", programID));
            paras.Add(new SqlParameter("@Category", category));
            paras.Add(new SqlParameter("@TestType", testType));

            result = DBProxy.Current.ExecuteSPByConn(sqlConn, "TransferToPO_1_ForAItem", paras);

            return result;
        }

        #endregion

        #region 依照AllowanceCombo將T項及轉入至採購單

        /// <summary>
        /// 依照AllowanceCombo將T項及轉入至採購單
        /// 從Trade 複製過來
        /// </summary>
        /// <param name="sqlConn">sqlConn</param>
        /// <param name="poID">採購母單</param>
        /// <inheritdoc />
        public static DualResult TransferToPO_1_ForThreadAllowance(SqlConnection sqlConn, string poID, bool isMaterialCompare = false)
        {
            DualResult result;
            List<SqlParameter> paras = new List<SqlParameter>();

            paras.Add(new SqlParameter("@PoID", poID));
            paras.Add(new SqlParameter("@UserID", Env.User.UserID));
            paras.Add(new SqlParameter("@ForMaterialCompare", isMaterialCompare));

            result = DBProxy.Current.ExecuteSPByConn(sqlConn, "TransferToPO_1_ForThreadAllowance", paras);

            return result;
        }

        #endregion

        #region 將轉出PO的temp Table欄位資料補上

        /// <summary>
        /// 將轉出PO的欄位資料補上
        /// </summary>
        /// <param name="sqlConn">sqlConn</param>
        /// <param name="poID">採購母單</param>
        /// <param name="appType">重新產生資料時是否要覆蓋原始資料</param>
        /// <param name="testType">是否為虛擬庫存計算(0: 實際寫入Table; 1: 僅傳出Temp Table; 2: 不回傳Temp Table; 3: 實際寫入Table，但不回傳Temp Table)</param>
        /// <inheritdoc />
        public static DualResult TransferToPO_2(SqlConnection sqlConn, string poID, bool appType, int testType)
        {
            DualResult result;
            List<SqlParameter> paras = new List<SqlParameter>();

            // String sqlCmd = "";
            paras.Add(new SqlParameter("@PoID", poID));
            paras.Add(new SqlParameter("@AppType", appType));
            paras.Add(new SqlParameter("@TestType", testType));

            result = DBProxy.Current.ExecuteSPByConn(sqlConn, "TransferToPO_2", paras);

            return result;
        }

        #endregion
    }
}
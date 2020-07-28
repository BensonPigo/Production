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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Transactions;

namespace Sci.Production.PublicPrg
{

    public static partial class Prgs
    {
        #region -- UpdatePO_Supp_Detail --
        /// <summary>
        /// UpdatePO_Supp_Detail()
        /// *	更新 Po3 的庫存
        /// *-----------------------------------------------------
        /// * 使用新方法
        /// * 1.各程式 OnDetailSelectCommandPrepare()欄位名稱要與這Sqlcommand對上 EX:統一用Location
        /// * 2.延續舊做法True 為增加 / False為減少
        /// * 3.Case2,8才需要傳List<>過來,重組Location
        /// 
        /// * Type	: 
        /// *   0.  更新Location
        /// *	2.	更新InQty
        /// *	4.	更新OutQty
        /// *	8.	更新LInvQty
        /// *	16.	更新LObQty
        /// *	32.	更新AdQty
        /// </summary>
        /// <param name="Int Type"></param>
        /// <param name="String Poid"></param>
        /// <param name="String Seq1"></param>
        /// <param name="String Seq2"></param>
        /// <param name="decimal qty"</param>
        /// <param name="bool encoded"></param>
        /// <param name="string stocktype"></param>
        /// <param name="string m"></param>
        /// <returns>String Sqlcmd</returns>
        //(整批) A & B倉
        public static string UpdateMPoDetail(int type, List<Prgs_POSuppDetailData> datas, bool encoded, bool attachLocation = true, SqlConnection sqlConn = null)
        {
            #region 以原本的datas的5keys 去ftyinventory撈location和原本loction重組以逗號分開,塞回原本資料
            DataTable TBattachlocation;
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
                    MyUtility.Tool.ProcessWithObject(datas, "", sqlcmdforlocation, out TBattachlocation, "#Tmp", sqlConn);
                    if (TBattachlocation != null)
                    {
                        var newDatas = TBattachlocation.AsEnumerable().Select(w =>
                                new Prgs_POSuppDetailData
                                {
                                    poid = w.Field<string>("poid"),
                                    seq1 = w.Field<string>("seq1"),
                                    seq2 = w.Field<string>("seq2"),
                                    stocktype = w.Field<string>("stocktype"),
                                    qty = w.Field<decimal>("qty"),
                                    location = w.Field<string>("location"),
                                }).ToList();
                        datas.Clear();
                        datas.AddRange(newDatas);
                    }
                }
            }
            #endregion
            String sqlcmd = "";
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
	set target.inqty = isnull(target.inqty,0.00) + src.qty , target.blocation = src.location
when not matched by target and src.stocktype = 'I' then
    insert ([Poid],[Seq1],[Seq2],[inqty],[blocation])
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'B' then
	update 
	set target.inqty = isnull(target.inqty,0.00) + src.qty , target.alocation = src.location
when not matched by target and src.stocktype = 'B' then
    insert ([Poid],[Seq1],[Seq2],[inqty],[alocation])
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2
when matched and src.stocktype = 'O' then
	update 
	set target.inqty = isnull(target.inqty,0.00) + src.qty , target.Clocation = src.location
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
from mdivisionpodetail t WITH (NOLOCK) 
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
                    from mdivisionpodetail t WITH (NOLOCK) 
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
from mdivisionpodetail t WITH (NOLOCK) 
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2

;drop Table #TmpSource";
                    #endregion
                    break;
            }
            return sqlcmd;
        }
        #endregion
        #region -- UpdateFtyInventory --
        /// <summary>
        /// UpdateFtyInventory()
        /// *	更新 FtyInventory 的庫存
        /// *-----------------------------------------------------
        /// * 使用新方法
        /// * IList<DataRow>只是先預留,目前做法都不用,都先填上null即可
        /// /// 
        /// * Type	: 
        /// *	2.	更新InQty
        /// *	4.	更新OutQty
        /// *	6.	更新OutQty with Location
        /// *	8.	更新AdjustQty
        /// *   26. 更新Location
        /// </summary>
        /// <param name="Int Type"></param>
        /// <param name="String Poid"></param>
        /// <param name="String Seq1"></param>
        /// <param name="String Seq2"></param>
        /// <param name="decimal qty"</param>
        ///<param name="roll"></param>
        ///<param name="dyelot"></param>
        /// <param name="char stocktype"></param>
        /// <param name="bool encoded"></param>
        /// <param name="location"></param>
        /// <returns>String Sqlcmd</returns>
        //(整批)
        public static string UpdateFtyInventory_IO(int type, IList<DataRow> datas, bool encoded, int MtlAutoLock = 0)
        {
            string sqlcmd = "";
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

select distinct poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) , qty, dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource

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
         Lock = iif(s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99' ,{MtlAutoLock},0),
         LockName = iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {MtlAutoLock}=1 ,'{Sci.Env.User.UserID}',''),
         LockDate = iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {MtlAutoLock}=1 ,getdate(),null)
when not matched then
    insert ( [MDivisionPoDetailUkey],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty], [Lock],[LockName],[LockDate])
    values ((select ukey from dbo.MDivisionPoDetail WITH (NOLOCK) 
			 where poid = s.poid and seq1 = s.seq1 and seq2 = s.seq2)
			 ,s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty,
              iif(s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99' ,{MtlAutoLock},0),
              iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {MtlAutoLock}=1 ,'{Sci.Env.User.UserID}',''),
              iif((s.psdseq1 between '01' and '69' or s.psdseq1 between '80' and '99') and {MtlAutoLock}=1 ,getdate(),null)
            );
";
                    if (encoded)
                    {
                        sqlcmd += @"
select location,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f WITH (NOLOCK) on f.poid = s.poid 
						 and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot
merge dbo.ftyinventory_detail as t 
using #tmp_L_K as s on t.ukey = s.ukey and isnull(t.mtllocationid,'') = isnull(s.location,'')
when not matched then
    insert ([ukey],[mtllocationid]) 
	values (s.ukey,isnull(s.location,''));

--delete t from FtyInventory_Detail t
--where  exists(select 1 from #tmp_L_K x where x.ukey=t.Ukey and x.location != t.MtlLocationID)
drop table #tmp_L_K 
";//↑最後一段delete寫法千萬不能用merge作,即使只有一筆資料也要跑超久
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

select distinct poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))), qty, dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource

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

select distinct  poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))), qty, dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource

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
select location,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f WITH (NOLOCK) on poid = s.poid 
                         and seq1 = s.seq1 and seq2 = s.seq2 and roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot
merge dbo.ftyinventory_detail as t
using #tmp_L_K as s on t.ukey = s.ukey and t.mtllocationid = s.location
when not matched then
    insert ([ukey],[mtllocationid]) 
    values (s.ukey,isnull(s.location,''));

--delete t from FtyInventory_Detail t
--where  exists(select 1 from #tmp_L_K x where x.ukey=t.Ukey and x.location != t.MtlLocationID)
drop table #tmp_L_K
";//↑最後一段delete寫法千萬不能用merge作,即使只有一筆資料也要跑超久
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

select distinct poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))), qty, dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource

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

select tolocation,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f WITH (NOLOCK) on f.poid = s.poid 
                                           and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll and f.stocktype = s.stocktype and f.dyelot = s.dyelot

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
                case 70:
                    #region 更新Barcode
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)
alter table #TmpSource alter column Barcode varchar(13)

select distinct poid, seq1, seq2, stocktype
, roll = RTRIM(LTRIM(isnull(roll, '')))
, dyelot = isnull(dyelot, '')
, Barcode
into #tmpS1
from #TmpSource

merge dbo.FtyInventory as target
using #tmpS1 as s
on target.poid = s.poid 
    and target.seq1 = s.seq1 
    and target.seq2 = s.seq2 
    and target.stocktype = s.stocktype 
    and target.roll = s.roll 
    and target.dyelot = s.dyelot
when matched then
    update
    set Barcode = s.Barcode;

drop table #tmpS1; 
drop table #TmpSource;";
                    #endregion
                    break;
            }
            return sqlcmd;
        }
        #endregion
        #region -- SelePoItem --
        public static string selePoItemSqlCmd(bool junk = true)
        {
            return @"
select  p.id,concat(Ltrim(Rtrim(p.seq1)), ' ', p.seq2) as seq
        , p.Refno   
        , dbo.getmtldesc(p.id,p.seq1,p.seq2,2,0) as Description 
        , p.ColorID
        , p.SizeSpec 
        , p.FinalETA
        , isnull(m.InQty, 0) as InQty
        , p.pounit
        , StockUnit = dbo.GetStockUnitBySPSeq (p.id, p.seq1, p.seq2)
        , isnull(m.OutQty, 0) as outQty
        , isnull(m.AdjustQty, 0) as AdjustQty
        , isnull(m.inqty, 0) - isnull(m.OutQty, 0) + isnull(m.AdjustQty, 0) as balance
        , isnull(m.LInvQty, 0) as LInvQty
        , p.fabrictype
        , p.seq1
        , p.seq2
        , p.scirefno
        , Qty = Round (p.qty * v.Ratevalue, 2)
        ,[Status]=IIF(LockStatus.LockCount > 0 ,'Locked','Unlocked')
from dbo.PO_Supp_Detail p WITH (NOLOCK) 
inner join Orders o on p.id = o.id
inner join Factory f on o.FtyGroup = f.id
left join dbo.mdivisionpodetail m WITH (NOLOCK) on m.poid = p.id and m.seq1 = p.seq1 and m.seq2 = p.seq2
inner join View_unitrate v on v.FROM_U = p.POUnit 
	                          and v.TO_U = dbo.GetStockUnitBySPSeq (p.id, p.seq1, p.seq2)
OUTER APPLY(
	SELECT [LockCount]=COUNT(UKEY)
	FROM FtyInventory
	WHERE POID='{0}'
	AND Seq1=p.Seq1
	AND Seq2=p.Seq2
	AND Lock = 1
)LockStatus
where p.id ='{0}'
" + (junk ? "and p.Junk = 0" : string.Empty);
        }
        /// <summary>
        /// 右鍵開窗選取採購項
        /// </summary>
        /// <param name="poid"></param>
        /// <param name="defaultseq"></param>
        /// <param name="filters"></param>
        /// <returns>Sci.Win.Tools.SelectItem</returns>
        public static Sci.Win.Tools.SelectItem SelePoItem(string poid, string defaultseq, string filters = null, bool junk = true)
        {
            DataTable dt;
            string PoItemSql = selePoItemSqlCmd(junk);
            if (!(MyUtility.Check.Empty(PoItemSql)))
            {
                PoItemSql += string.Format(" And {0}", filters);
            }
            string sqlcmd = string.Format(PoItemSql, poid, Sci.Env.User.Keyword);
            PoItemSql = "";

            DBProxy.Current.Select(null, sqlcmd, out dt);

            Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(dt
                            , "Seq,refno,description,colorid,SizeSpec,FinalETA,inqty,stockunit,outqty,adjustqty,balance,linvqty"
                            , "6,8,35,8,10,6,6,6,6,6,6", defaultseq, "Seq,Ref#,Description,Color,Size,ETA,In Qty,Stock Unit,Out Qty,Adqty,Balance,Inventory Qty");
            selepoitem.Width = 1024;

            return selepoitem;
        }
        #endregion
        #region-- SelectLocation --
        /// <summary>
        /// 右鍵開窗選取物料儲位
        /// </summary>
        /// <param name="stocktype"></param>
        /// <param name="defaultseq"></param>
        /// <returns>Sci.Win.Tools.SelectItem2</returns>
        public static Sci.Win.Tools.SelectItem2 SelectLocation(string stocktype, string defaultseq = "")
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
                sqlcmd = string.Format(@"
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
WHERE   StockType='{0}'
        and junk != '1'", stocktype);
            }


            Sci.Win.Tools.SelectItem2 selectlocation = new Win.Tools.SelectItem2(sqlcmd,
                            "Location ID,Description,Stock Type", "13,60,10", defaultseq, null, null, null);
            selectlocation.Width = 1024;

            return selectlocation;
        }
        #endregion

        public static bool CheckLocationExists(string stocktype, string location) {
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

        #region-- GetLocation --
        public static string GetLocation(int ukey, System.Data.SqlClient.SqlConnection conn = null)
        {
            //string rtn = "";
            DataRow dr;
            DataTable dt;
            if (null == conn)
            {
                MyUtility.Check.Seek(string.Format(@"select cast(tmp.MtlLocationID as nvarchar) +','
from (select f.MtlLocationID from dbo.FtyInventory_Detail f WITH (NOLOCK) where f.Ukey = {0}) tmp
for xml path('') ", ukey), out dr);
                if (MyUtility.Check.Empty(dr)) return "";
            }
            else
            {
                DBProxy.Current.SelectByConn(conn, string.Format(@"select cast(tmp.MtlLocationID as nvarchar) +','
from (select f.MtlLocationID from dbo.FtyInventory_Detail f WITH (NOLOCK) where f.Ukey = {0}) tmp
for xml path('') ", ukey), out dt);
                if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0) return "";
                dr = dt.Rows[0];
            }
            return dr[0].ToString();
        }
        #endregion
        #region-- Distinct String--
        /// <summary>
        /// 將字串依逗點分隔拆開，剔除重覆後重新以逗點分隔連接字串
        /// </summary>
        /// <param name="str"></param>
        /// <returns>String</returns>
        public static string DistinctString(string str)
        {
            string[] strA = Regex.Split(str, ",");
            string rtn = "";
            foreach (string i in strA.Distinct())
            {
                rtn += i + ",";
            }
            return rtn;
        }
        #endregion

        public static IList<DataRow> autopick(DataRow materials, bool isIssue = true, string stocktype = "B")
        {
            List<DataRow> items = new List<DataRow>();
            String sqlcmd;
            DataTable dt;
            decimal request; //需求總數

            decimal accu_issue = 0m;
            if (isIssue)//P10 Auto Pick
            {
                request = decimal.Parse(materials["requestqty"].ToString()) - decimal.Parse(materials["accu_issue"].ToString());
                sqlcmd = string.Format(@"
with cte as (
    select  Dyelot
            , sum(inqty-OutQty+AdjustQty) as GroupQty
    from dbo.FtyInventory a WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on  p.id = a.POID 
                                                      and p.seq1 = a.Seq1 
                                                      and p.seq2 = a.Seq2
    where   poid = '{1}' 
            and Stocktype = '{4}' 
            and inqty - OutQty + AdjustQty > 0
            and p.SCIRefno = '{2}' 
            and p.ColorID = '{3}' 
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
        , inqty - OutQty + AdjustQty qty
        , inqty
        , outqty
        , adjustqty
        , inqty - OutQty + AdjustQty balanceqty
        , running_total = sum(inqty-OutQty+AdjustQty) over (order by c.GroupQty DESC,a.Dyelot,inqty-OutQty+AdjustQty desc
                                                            rows between unbounded preceding and current row)
        --,c.GroupQty
from cte c 
inner join dbo.FtyInventory a WITH (NOLOCK) on a.Dyelot=c.Dyelot
inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on  p.id = a.POID 
                                                  and p.seq1 = a.Seq1 
                                                  and p.seq2 = a.Seq2
where   poid = '{1}' 
        and Stocktype = '{4}' 
        and inqty - OutQty + AdjustQty > 0
        and p.SCIRefno = '{2}' 
        and p.ColorID = '{3}' 
        and a.Seq1 BETWEEN '00' AND '99'", Sci.Env.User.Keyword, materials["poid"], materials["scirefno"], materials["colorid"], stocktype);
            }
            else if (isIssue == false && stocktype == "B")//P28 Auto Pick
            {
                request = decimal.Parse(materials["requestqty"].ToString());
                sqlcmd = string.Format(@"
with cte as (
    select  Dyelot
            , sum (inqty - OutQty + AdjustQty) as GroupQty
    from dbo.FtyInventory a WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on  p.id = a.POID 
                                                      and p.seq1 = a.Seq1 
                                                      and p.seq2 = a.Seq2
    where   poid = '{1}' 
            and Stocktype = '{4}' 
            and inqty - OutQty + AdjustQty > 0
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
        , inqty - OutQty + AdjustQty qty
        , inqty
        , outqty
        , adjustqty
        , inqty - OutQty + AdjustQty balanceqty
        , running_total = sum(inqty-OutQty+AdjustQty) over (order by c.GroupQty DESC,a.Dyelot,inqty-OutQty+AdjustQty DESC
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
        and inqty-OutQty+AdjustQty > 0
        and p.seq1 = '{2}' 
        and p.seq2 = '{3}'", Sci.Env.User.Keyword, materials["poid"], materials["seq1"], materials["seq2"], stocktype);
            }
            else//P29,P30 Auto Pick
            {
                request = decimal.Parse(materials["requestqty"].ToString());
                sqlcmd = string.Format(@"
with cte as (
    select  Dyelot
            , sum (inqty - OutQty + AdjustQty) as GroupQty
    from dbo.FtyInventory a WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on  p.id = a.POID 
                                                      and p.seq1 = a.Seq1 
                                                      and p.seq2 = a.Seq2
    where   poid = '{1}' 
            and Stocktype = '{4}' 
            and inqty - OutQty + AdjustQty > 0
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
        , inqty - OutQty + AdjustQty qty
        , inqty
        , outqty
        , adjustqty
        , inqty - OutQty + AdjustQty balanceqty
        , running_total = sum (inqty - OutQty + AdjustQty) over (order by c.GroupQty DESC,a.Dyelot,inqty-OutQty+AdjustQty DESC
                                                                 rows between unbounded preceding and current row) 
        --,c.GroupQty
from dbo.FtyInventory a WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on  p.id = a.POID 
                                                  and p.seq1 = a.Seq1 
                                                  and p.seq2 = a.Seq2
inner join cte c on c.Dyelot = a.Dyelot
where   poid = '{1}' 
        and Stocktype = '{4}' 
        and inqty - OutQty + AdjustQty > 0
        and p.seq1 = '{2}' 
        and p.seq2 = '{3}'", Sci.Env.User.Keyword, materials["StockPOID"], materials["StockSeq1"], materials["StockSeq2"], stocktype);
            }
            DualResult result = DBProxy.Current.Select("", sqlcmd, out dt);
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
                if (isIssue == false && stocktype == "B")//P28 Auto Pick
                {
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
                            DualResult Result = MyUtility.Tool.ProcessWithDatatable(dyelotS, "dyelot,qty", s, out ct_std);
                            if (!Result)
                            {
                                MyUtility.Msg.ErrorBox(Result.ToString());
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
                                        blance -= (decimal)(item.GetValue("qty"));
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
                                //依照最後一塊料的Dyelot來找到對應的Group來取得最後一塊料
                                findrow = dt.AsEnumerable().Where(row => row["Dyelot"].EqualString(dr2["Dyelot"].ToString())).CopyToDataTable();
                                break;
                            }
                        }

                        if (accu_issue < request && findrow != null)   // 累計發料數小於需求數時，再反向取得最後一塊料。
                        {
                            decimal balance = request - accu_issue;
                            //dt.DefaultView.Sort = "Dyelot,location,Seq1,seq2,Qty asc";
                            for (int i = findrow.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow find = items.Find(item => item["ftyinventoryukey"].ToString() == findrow.Rows[i]["ftyinventoryukey"].ToString());
                                if (MyUtility.Check.Empty(find))// if overlape
                                {
                                    if (balance > 0m)
                                    {
                                        if (balance >= (decimal)findrow.Rows[i]["qty"])
                                        {
                                            items.Add(findrow.Rows[i]);
                                            balance -= (decimal)findrow.Rows[i]["qty"];
                                        }
                                        else//最後裁切
                                        {
                                            //P10最後裁切若有小數點需無條件進位
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

        public static IList<DataRow> AutoPickTape(DataRow materials, string cutplanid, bool isIssue = true, string stocktype = "B")
        {
            List<DataRow> items = new List<DataRow>();
            string sqlcmd;
            DataTable dt;
            decimal accu_issue = 0;
            // 此筆需求數 = 總需求數 - 已經issue總數
            decimal request = decimal.Parse(materials["requestqty"].ToString()) - decimal.Parse(materials["accu_issue"].ToString());

            sqlcmd = string.Format(@"
select a.Seq1, a.Seq2, ctpd.Dyelot, sum(a.inqty-a.OutQty+a.AdjustQty) as GroupQty
into #tmp
from  CutTapePlan_Detail ctpd
inner join CutTapePlan ctp on ctp.ID = ctpd.ID
inner join PO_Supp_Detail psd on psd.ColorID = ctpd.ColorID and psd.Refno = ctpd.RefNo and psd.ID = ctp.CuttingID
inner join FtyInventory a on a.POID = '{0}' and a.Seq1 = psd.Seq1 and a.Seq2 = psd.Seq2 and a.Dyelot = ctpd.Dyelot
where ctpd.id = '{1}' and a.Seq1 between '01' and '99' and a.StockType = '{2}'
group by a.Seq1, a.Seq2, ctpd.Dyelot

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
	, inqty - OutQty + AdjustQty qty
	, inqty
	, outqty
	, adjustqty
	, inqty - OutQty + AdjustQty balanceqty
    , running_total = sum(inqty-OutQty+AdjustQty) over (order by t.GroupQty DESC,a.Dyelot,inqty-OutQty+AdjustQty desc
                                                        rows between unbounded preceding and current row)
from #tmp t
inner join FtyInventory a on a.POID = '{0}' and a.Seq1 = t.Seq1 and a.Seq2 = t.Seq2 and a.Dyelot = t.Dyelot and a.StockType = '{2}'

drop table #tmp", materials["poid"], cutplanid, stocktype);

            DualResult result = DBProxy.Current.Select("", sqlcmd, out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(sqlcmd, "Sql Error");
                return null;
            }

            DataTable findrow = null;
            if (dt.Rows.Count == 0 || dt.Select("balanceqty<>0").Length == 0)
            {
                return null;
            }

            dt = dt.Select("balanceqty<>0").CopyToDataTable();
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
                        //依照最後一塊料的Dyelot來找到對應的Group來取得最後一塊料
                        findrow = dt.AsEnumerable().Where(row => row["Dyelot"].EqualString(dr2["Dyelot"].ToString())).CopyToDataTable();
                        break;
                    }
                }

                if (accu_issue < request && findrow != null)   // 累計發料數小於需求數時，再反向取得最後一塊料。
                {
                    decimal balance = request - accu_issue;

                    for (int i = findrow.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow find = items.Find(item => item["ftyinventoryukey"].ToString() == findrow.Rows[i]["ftyinventoryukey"].ToString());
                        if (MyUtility.Check.Empty(find))// if overlape
                        {
                            if (balance > 0m)
                            {
                                if (balance >= (decimal)findrow.Rows[i]["qty"])
                                {
                                    items.Add(findrow.Rows[i]);
                                    balance -= (decimal)findrow.Rows[i]["qty"];
                                }
                                else//最後裁切
                                {
                                    //P10最後裁切若有小數點需無條件進位
                                    if (isIssue)
                                    {
                                        if (balance >= (decimal)findrow.Rows[i]["qty"])
                                        {
                                            items.Add(findrow.Rows[i]);
                                            balance = 0m;
                                        }
                                        else
                                        {
                                            findrow.Rows[i]["qty"] = balance;
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

            return items;
        }

        /// <summary>
        /// 目的：自動產生可以寫入Issue_Detail的DataRow
        /// </summary>
        /// <param name="materials">P33表身</param>
        /// <returns>準備寫入P33第三層的DataRow (資料結構: Issue_Detail)</returns>
        public static List<DataRow> Thread_AutoPick(DataRow material, decimal AccuIssued)
        {
            List<DataRow> items = new List<DataRow>();

            //foreach (DataRow material in materials)
            //{
            string sqlcmd = string.Empty;
            DataTable dt;
            decimal request; //需求總數
            decimal accu_issue = 0m;
            //decimal AccuIssued = MyUtility.Check.Empty(material["AccuIssued"]) ? 0 : decimal.Parse(material["AccuIssued"].ToString());
            decimal UseQtyByStockUnit = MyUtility.Check.Empty(material["Use Qty By Stock Unit"]) ? 0 : decimal.Parse(material["Use Qty By Stock Unit"].ToString());

            // 需求量 - 已發累計量 = 待發的量
            request = UseQtyByStockUnit;// - AccuIssued;


            // 取得所有項次號(欄位名稱跟Issue_Detail一樣)
            sqlcmd = $@"

select   [POID]=psd.ID
    , psd.Seq1
    , psd.Seq2
	, psd.SCIRefno
	, psd.SuppColor
    , a.stocktype
    , [BulkQty] =ISNULL( a.inqty - a.outqty + a.adjustqty,0.00)
	, [Qty]=0.00
	, [BulkLocation]= STUFF ((
									SELECT ',' + MtlLocationID 
									FROM dbo.FtyInventory_Detail WITH (NOLOCK) 
									WHERE ukey = a.Ukey AND MtlLocationID <> ''
									FOR XML PATH('')
								 ), 1, 1, '')
    , [FtyInventoryUkey]=a.Ukey
from dbo.FtyInventory a WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on  psd.id = a.POID 
												and psd.seq1 = a.Seq1 
												and psd.seq2 = a.Seq2

INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
where    psd.ID = '{material["poid"]}'
	and psd.SCIRefno = '{material["SCIRefno"]}' 
	and psd.ColorID = '{material["ColorID"]}' 
    AND (a.stocktype = 'B' OR a.stocktype IS NULL)
    AND m.IsThread=1
";

            DualResult result = DBProxy.Current.Select("", sqlcmd, out dt);
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
                    DataRow ImortRow = dt.AsEnumerable().Where(o => ((decimal)o["BulkQty"]).EqualDecimal(request)).FirstOrDefault();
                    ImortRow["Qty"] = request;
                    items.Add(ImortRow);
                }
                else
                {
                    // 沒有的話，則從Qty > 需求量的,找第一筆
                    if (dt.AsEnumerable().Any(o => (decimal)o["BulkQty"] > request))
                    {
                        DataRow ImortRow = dt.AsEnumerable().Where(o => (decimal)o["BulkQty"] > request).FirstOrDefault();
                        ImortRow["Qty"] = request;
                        items.Add(ImortRow);
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
            //}

            return items;
        }

        /// <summary>
        /// 檢查實際到倉日不可早於到港日
        /// </summary>
        /// <param name="ArrivedPortDate"></param>
        /// <param name="ArrivedWhseDate"></param>
        /// <param name="msg"></param>
        /// <returns>bool</returns>
        public static bool CheckArrivedWhseDateWithArrivedPortDate(DateTime ArrivedPortDate, DateTime ArrivedWhseDate, out String msg)
        {
            msg = "";
            if (ArrivedPortDate > ArrivedWhseDate)
            {
                msg = "Arrive Warehouse date can't be earlier than arrive port date!!";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 檢查實際到倉日若早於ETA 3天或晚於 15天都回傳訊息。
        /// </summary>
        /// <param name="Eta"></param>
        /// <param name="ArrivedWhseDate"></param>
        /// <param name="msg"></param>
        /// <returns>bool</returns>
        public static bool CheckArrivedWhseDateWithEta(DateTime Eta, DateTime ArrivedWhseDate, out String msg)
        {
            msg = "";
            // 到倉日如果早於ETA 3天，則提示窗請USER再確認是否存檔。
            if (DateTime.Compare(Eta, ArrivedWhseDate.AddDays(3)) > 0)
            {
                msg = "Arrive Warehouse date is earlier than ETA 3 days, do you save it?";
                return false;
            }
            // 到倉日如果晚於ETA 15天，則提示窗請USER再確認是否存檔。
            if (DateTime.Compare(Eta.AddDays(15), ArrivedWhseDate) < 0)
            {
                msg = "Arrive Warehouse date is later than ETA 15 days, do you save it?";
                return false;
            }
            return true;
        }

        public static bool P22confirm(DataRow dr, DataTable dt)
        {
            StringBuilder upd_MD_8T = new StringBuilder();
            String upd_MD_0F = "";
            String upd_Fty_4T = "";
            String upd_Fty_2T = "";

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;

            #region -- 檢查庫存項lock --

            bool MtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
            if (!MtlAutoLock)
            {
                sqlcmd = string.Format(@"Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
and d.FromSeq1 = f.Seq1 and d.FromSeq2 = f.seq2 and d.FromRoll = f.Roll and d.FromDyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", dr["id"]);
                if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    MyUtility.Msg.ErrorBox(sqlcmd + result2.ToString());
                    return false;
                }
                else
                {
                    if (datacheck.Rows.Count > 0)
                    {
                        foreach (DataRow tmp in datacheck.Rows)
                        {
                            ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine
                                , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["Dyelot"]);
                        }
                        MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                        return false;
                    }
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"Select d.frompoid,d.fromseq1,d.fromseq2,d.fromRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
and d.FromSeq1 = f.Seq1 and d.FromSeq2 = f.seq2 and d.FromRoll = f.Roll and d.FromDyelot = f.Dyelot
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Qty>0  and d.Id = '{0}'", dr["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                MyUtility.Msg.ErrorBox(sqlcmd + result2.ToString());
                return false;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Bulk balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return false;
                }
            }

            sqlcmd = string.Format(@"Select d.topoid,d.toseq1,d.toseq2,d.toRoll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty, f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on  d.toPoId = f.PoId
and d.toSeq1 = f.Seq1
and d.toSeq2 = f.seq2
and d.toStocktype = f.StockType
and d.toRoll = f.Roll
and d.toDyelot = f.Dyelot
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Qty<0 and d.Id = '{0}'", dr["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                MyUtility.Msg.ErrorBox(sqlcmd + result2.ToString());
                return false;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Inventory balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return false;
                }
            }

            #endregion -- 檢查負數庫存 --

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update SubTransfer set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, dr["id"]);

            #endregion 更新表頭狀態資料
            #region -- 更新mdivisionpodetail B倉數 --
            var data_MD_8T = (from b in dt.AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("frompoid"),
                                  seq1 = b.Field<string>("fromseq1"),
                                  seq2 = b.Field<string>("fromseq2"),
                                  stocktype = b.Field<string>("fromstocktype")
                              } into m
                              select new Prgs_POSuppDetailData
                              {
                                  poid = m.First().Field<string>("frompoid"),
                                  seq1 = m.First().Field<string>("fromseq1"),
                                  seq2 = m.First().Field<string>("fromseq2"),
                                  stocktype = m.First().Field<string>("fromstocktype"),
                                  qty = m.Sum(w => MyUtility.Convert.GetDecimal(w["qty"])),
                                  location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct())
                              }).ToList();

            var data_MD_0F = (from b in dt.AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("topoid"),
                                  seq1 = b.Field<string>("toseq1"),
                                  seq2 = b.Field<string>("toseq2"),
                                  stocktype = b.Field<string>("tostocktype")
                              } into m
                              select new Prgs_POSuppDetailData
                              {
                                  poid = m.First().Field<string>("topoid"),
                                  seq1 = m.First().Field<string>("toseq1"),
                                  seq2 = m.First().Field<string>("toseq2"),
                                  stocktype = m.First().Field<string>("tostocktype"),
                                  qty = 0,
                                  location = ""
                              }).ToList();
            #endregion            
            #region -- 更新庫存數量 ftyinventory --
            var data_Fty_4T = (from m in dt.AsEnumerable()
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

            DataTable newDt = dt.Clone();
            foreach (DataRow dtr in dt.Rows)
            {
                string[] dtrLocation = dtr["ToLocation"].ToString().Split(',');
                dtrLocation = dtrLocation.Distinct().ToArray();

                if (dtrLocation.Length == 1)
                {
                    DataRow newDr = newDt.NewRow();
                    newDr.ItemArray = dtr.ItemArray;
                    newDt.Rows.Add(newDr);
                }
                else
                {
                    foreach (string location in dtrLocation)
                    {
                        DataRow newDr = newDt.NewRow();
                        newDr.ItemArray = dtr.ItemArray;
                        newDr["ToLocation"] = location;
                        newDt.Rows.Add(newDr);
                    }
                }
            }

            var data_Fty_2T = (from b in newDt.AsEnumerable()
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
            upd_Fty_4T = Prgs.UpdateFtyInventory_IO(4, null, true);
            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion 更新庫存數量 ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);

            using (_transactionscope)
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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4T, "", upd_Fty_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.ErrorBox(sqlcmd + result2.ToString());
                        return false;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, "", upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.ErrorBox(sqlcmd + result2.ToString());
                        return false;
                    }
                    #endregion 

                    #region MDivisionPoDetail
                    upd_MD_8T.Append(Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn));
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, "", upd_MD_8T.ToString(), out resulttb
                        , "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.ErrorBox(sqlcmd + result2.ToString());
                        return false;
                    }

                    upd_MD_0F = Prgs.UpdateMPoDetail(0, data_MD_0F, false, sqlConn: sqlConn);
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_0F, "", upd_MD_0F.ToString(), out resulttb
                        , "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.ErrorBox(sqlcmd + result2.ToString());
                        return false;
                    }

                    #endregion 

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.ErrorBox(sqlcmd + result2.ToString());
                        return false;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    // MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    MyUtility.Msg.ErrorBox(ex.ToString());
                    return false;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            return true;
        }

        public static DualResult P23confirm(string SubTransfer_ID)
        {

            string upd_MD_4T = "";
            string upd_MD_8T = "";
            string upd_MD_2T = "";
            String upd_Fty_4T = "";
            String upd_Fty_2T = "";

            string sqlcmd = "";
            string sqlupd3, ids = "";
            DualResult result = new DualResult(true);
            DataTable datacheck;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"
Select  d.frompoid
    , d.fromseq1
    , d.fromseq2
    , d.fromRoll
    , d.Qty
    , balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
    ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID 
                                        and D.FromStockType = F.StockType
                                        and d.FromRoll = f.Roll 
                                        and d.FromSeq1 =f.Seq1 
                                        and d.FromSeq2 = f.Seq2
                                        and d.fromDyelot = f.Dyelot
where   f.lock=1 
    and d.Id = '{0}'", SubTransfer_ID);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["Dyelot"]);
                    }
                    return new DualResult(false, "Material Locked!!" + Environment.NewLine + ids);
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"
Select  d.frompoid
        , d.fromseq1
        , d.fromseq2
        , d.fromRoll
        , d.Qty
        , balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0),d.FromDyelot
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
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) < d.Qty) ", SubTransfer_ID);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"], tmp["FromDyelot"]);
                    }
                    return new DualResult(false, "Inventory balance Qty is not enough!!" + Environment.NewLine + ids);
                }
            }

            sqlcmd = string.Format(@"
Select  d.topoid
        , d.toseq1  
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
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
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) ", SubTransfer_ID);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Roll#: {3}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["balanceqty"], tmp["qty"], tmp["toDyelot"]);
                    }
                    return new DualResult(false, "Bulk balacne Qty is not enough!!" + Environment.NewLine + ids);
                }
            }

            #endregion -- 檢查負數庫存 --

            #region -- 更新表頭狀態資料 --
            sqlupd3 = string.Format(@"
update SubTransfer 
set status = 'Confirmed'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, SubTransfer_ID);
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
from dbo.SubTransfer_detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId 
                                             and p1.seq1 = a.FromSeq1 
                                             and p1.SEQ2 = a.FromSeq2
left join FtyInventory FI on a.FromPoid = fi.poid 
                             and a.fromSeq1 = fi.seq1 
                             and a.fromSeq2 = fi.seq2
                             and a.fromRoll = fi.roll 
                             and a.fromStocktype = fi.stocktype
                             and a.fromDyelot = fi.Dyelot
Where a.id = '{SubTransfer_ID}'";
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
                                  stocktype = b.Field<string>("fromstocktype")
                              } into m
                              select new Prgs_POSuppDetailData
                              {
                                  poid = m.First().Field<string>("frompoid"),
                                  seq1 = m.First().Field<string>("fromseq1"),
                                  seq2 = m.First().Field<string>("fromseq2"),
                                  stocktype = m.First().Field<string>("fromstocktype"),
                                  qty = m.Sum(w => w.Field<decimal>("qty")),
                                  location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct())
                              }).ToList();

            var data_MD_8T = data_MD_4T.Select(data => new Prgs_POSuppDetailData
            {
                poid = data.poid,
                seq1 = data.seq1,
                seq2 = data.seq2,
                stocktype = data.stocktype,
                qty = -(data.qty)
            }).ToList();

            #endregion

            #region -- 更新mdivisionpodetail A倉數 --
            var data_MD_2T = (from b in dtSubTransfer_Detail.AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("Topoid"),
                                  seq1 = b.Field<string>("Toseq1"),
                                  seq2 = b.Field<string>("Toseq2"),
                                  stocktype = b.Field<string>("Tostocktype")
                              } into m
                              select new Prgs_POSuppDetailData
                              {
                                  poid = m.First().Field<string>("Topoid"),
                                  seq1 = m.First().Field<string>("Toseq1"),
                                  seq2 = m.First().Field<string>("Toseq2"),
                                  stocktype = m.First().Field<string>("Tostocktype"),
                                  qty = m.Sum(w => w.Field<decimal>("qty")),
                                  location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct())
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
                                   dyelot = m.Field<string>("fromdyelot")
                               } into g
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

            DataTable newDt = dtSubTransfer_Detail.Clone();
            foreach (DataRow dtr in dtSubTransfer_Detail.Rows)
            {
                string[] dtrLocation = dtr["ToLocation"].ToString().Split(',');
                dtrLocation = dtrLocation.Distinct().ToArray();

                if (dtrLocation.Length == 1)
                {
                    DataRow newDr = newDt.NewRow();
                    newDr.ItemArray = dtr.ItemArray;
                    newDt.Rows.Add(newDr);
                }
                else
                {
                    foreach (string location in dtrLocation)
                    {
                        DataRow newDr = newDt.NewRow();
                        newDr.ItemArray = dtr.ItemArray;
                        newDr["ToLocation"] = location;
                        newDt.Rows.Add(newDr);
                    }
                }
            }

            var data_Fty_2T = (from b in newDt.AsEnumerable()
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
            upd_Fty_4T = Prgs.UpdateFtyInventory_IO(4, null, true);
            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);
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

merge dbo.PO_Supp_Detail as target
using #tmpD as src on   target.ID = src.ToPoid 
                        and target.seq1 = src.ToSeq1 
                        and target.seq2 =src.ToSeq2 
when matched then
    update
    set target.StockUnit = src.StockUnit;
";
            #endregion 
            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (_transactionscope)
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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4T, "", upd_Fty_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, "", upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    upd_MD_4T = (Prgs.UpdateMPoDetail(4, null, true, sqlConn: sqlConn));
                    upd_MD_8T = (Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn));
                    upd_MD_2T = Prgs.UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T, "", upd_MD_4T, out resulttb
                        , "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, "", upd_MD_8T, out resulttb
                        , "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, "", upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    #endregion

                    if (!(result = MyUtility.Tool.ProcessWithDatatable
                        (dtSubTransfer_Detail, "", sql_UpdatePO_Supp_Detail, out resulttb, "#tmp", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    return new DualResult(false, "Commit transaction error.", ex);
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            return result;
        }

        public static DualResult P24confirm(string SubTransfer_ID)
        {
            string upd_MD_16T = "";
            string upd_MD_8T = "";
            string upd_MD_4T = "";
            string upd_MD_0F = "";
            String upd_Fty_4T = "";
            String upd_Fty_2T = "";

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;

            #region -- 檢查庫存項lock --
            bool MtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
            if (!MtlAutoLock)
            {
                sqlcmd = string.Format(@"
Select 	d.frompoid
		,d.fromseq1
		,d.fromseq2
		,d.fromRoll
		,d.Qty
		,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) 
        ,f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) 
    on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
       and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 and d.FromDyelot = f.Dyelot
where f.lock=1 and d.Id = '{0}'", SubTransfer_ID);
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
                            ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine
                                , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["Dyelot"]);
                        }
                        return new DualResult(false, "Material Locked!!" + Environment.NewLine + ids);
                    }
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"
Select 	d.frompoid
		,d.fromseq1
		,d.fromseq2
		,d.fromRoll
		,d.Qty
		,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
        , f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) 
	on d.FromPOID = f.POID  AND D.FromStockType = F.StockType
	   and d.FromRoll = f.Roll and d.FromSeq1 =f.Seq1 and d.FromSeq2 = f.Seq2 and d.FromDyelot = f.Dyelot
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", SubTransfer_ID);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than qty: {5}" + Environment.NewLine
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }
                    return new DualResult(false, "Balacne Qty is not enough!!" + Environment.NewLine + ids);
                }
            }

            #endregion -- 檢查負數庫存 --

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update SubTransfer set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, SubTransfer_ID);

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
from dbo.SubTransfer_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 and p1.SEQ2 = a.FromSeq2
left join FtyInventory f WITH (NOLOCK) on a.FromPOID=f.POID and a.FromSeq1=f.Seq1 and a.FromSeq2=f.Seq2 and a.FromRoll=f.Roll and a.FromDyelot=f.Dyelot and a.FromStockType=f.StockType
Where a.id = '{SubTransfer_ID}'";

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
                                  stocktype = b.Field<string>("fromstocktype")
                              } into m
                              select new Prgs_POSuppDetailData
                              {
                                  poid = m.First().Field<string>("frompoid"),
                                  seq1 = m.First().Field<string>("fromseq1"),
                                  seq2 = m.First().Field<string>("fromseq2"),
                                  stocktype = m.First().Field<string>("fromstocktype"),
                                  qty = m.Sum(w => w.Field<decimal>("qty"))
                              }).ToList();

            var data_MD_8T = data_MD_4T.Select(data => new Prgs_POSuppDetailData
            {
                poid = data.poid,
                seq1 = data.seq1,
                seq2 = data.seq2,
                stocktype = data.stocktype,
                qty = -(data.qty)
            }).ToList();

            var data_MD_0F = data_MD_4T.Select(data => new Prgs_POSuppDetailData
            {
                poid = data.poid,
                seq1 = data.seq1,
                seq2 = data.seq2,
                stocktype = data.stocktype,
                qty = 0
            }).ToList();

            #endregion 
            #region -- 更新mdivisionpodetail Scrap數 --
            var data_MD_16T = (from b in dtSubTransfer_Detail.AsEnumerable()
                               group b by new
                               {
                                   poid = b.Field<string>("topoid"),
                                   seq1 = b.Field<string>("toseq1"),
                                   seq2 = b.Field<string>("toseq2"),
                                   stocktype = b.Field<string>("tostocktype")
                               } into m
                               select new
                               {
                                   poid = m.First().Field<string>("topoid"),
                                   seq1 = m.First().Field<string>("toseq1"),
                                   seq2 = m.First().Field<string>("toseq2"),
                                   stocktype = m.First().Field<string>("tostocktype"),
                                   qty = m.Sum(w => w.Field<decimal>("qty"))
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

            DataTable newDt = dtSubTransfer_Detail.Clone();
            foreach (DataRow dtr in dtSubTransfer_Detail.Rows)
            {
                string[] dtrLocation = dtr["ToLocation"].ToString().Split(',');
                dtrLocation = dtrLocation.Distinct().ToArray();

                if (dtrLocation.Length == 1)
                {
                    DataRow newDr = newDt.NewRow();
                    newDr.ItemArray = dtr.ItemArray;
                    newDt.Rows.Add(newDr);
                }
                else
                {
                    foreach (string location in dtrLocation)
                    {
                        DataRow newDr = newDt.NewRow();
                        newDr.ItemArray = dtr.ItemArray;
                        newDr["ToLocation"] = location;
                        newDt.Rows.Add(newDr);
                    }
                }
            }

            var data_Fty_2T = (from m in newDt.AsEnumerable()
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
            upd_Fty_4T = Prgs.UpdateFtyInventory_IO(4, null, true);
            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion 更新庫存數量  ftyinventory
            #region ISP20191578 ToLocation的資料一併更新回MDivisionPODetail.CLocation欄位
            string updateMDivisionPODetailCLocation = string.Empty;
            var listMDivisionPODetailCLocation = dtSubTransfer_Detail.AsEnumerable()
                                                .Where(s => s["ToLocation"].ToString() != "")
                                                .Select(s => new
                                                {
                                                    FromPOID = s["FromPOID"].ToString(),
                                                    FromSeq1 = s["FromSeq1"].ToString(),
                                                    FromSeq2 = s["FromSeq2"].ToString(),
                                                    ToLocation = s["ToLocation"].ToString()
                                                });

            var listDistinctPoIdSeq = listMDivisionPODetailCLocation
                .Select(s => new
                {
                    s.FromPOID,
                    s.FromSeq1,
                    s.FromSeq2
                }
            ).Distinct();

            foreach (var updItem in listDistinctPoIdSeq)
            {
                string POID = updItem.FromPOID;
                string Seq1 = updItem.FromSeq1;
                string Seq2 = updItem.FromSeq2;

                List<string> New_CLocationList = listMDivisionPODetailCLocation
                            .Where( o => o.FromPOID == POID && 
                                         o.FromSeq1 == Seq1 && 
                                         o.FromSeq2 == Seq2)
                            .Select(o => o.ToLocation)
                            .Distinct().ToList();

                //從MDivisionPoDetail出現有的Location
                DataTable DT_MDivisionPoDetail;
                DBProxy.Current.Select(null, $@"
SELECt CLocation
FROM MDivisionPoDetail
WHERE POID='{POID}'
AND Seq1='{Seq1}' AND Seq2='{Seq2}'
", out DT_MDivisionPoDetail);

                List<string> DB_CLocations = DT_MDivisionPoDetail.Rows[0]["CLocation"].ToString().Split(',').Where(o => o != "").ToList();

                List<string> Fincal = new List<string>();

                foreach (var New_CLocation in New_CLocationList)
                {
                    if (DB_CLocations.Count == 0 || !DB_CLocations.Contains(New_CLocation))
                    {
                        DB_CLocations.Add(New_CLocation);
                    }
                }

                foreach (var CLocation in DB_CLocations.Distinct().ToList())
                {
                    foreach (var a in CLocation.Split(',').Where(o => o != "").Distinct().ToList())
                    {
                        if (!Fincal.Contains(a))
                        {
                            Fincal.Add(a);
                        }
                    }
                }

                string cmd = $@"
UPDATE MDivisionPoDetail
SET CLocation='{Fincal.Distinct().ToList().JoinToString(",")}'
WHERE POID='{POID}' AND Seq1='{Seq1}' AND Seq2='{Seq2}'

";
                updateMDivisionPODetailCLocation += cmd;
            }
            #endregion


            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);

            using (_transactionscope)
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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4T, "", upd_Fty_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, "", upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    upd_MD_4T = (Prgs.UpdateMPoDetail(4, data_MD_4T, true, sqlConn: sqlConn));
                    upd_MD_8T = (Prgs.UpdateMPoDetail(8, null, true, sqlConn: sqlConn));
                    upd_MD_16T = Prgs.UpdateMPoDetail(16, null, true, sqlConn: sqlConn);
                    upd_MD_0F = Prgs.UpdateMPoDetail(0, data_MD_0F, true, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, "", upd_MD_8T.ToString(), out resulttb
                        , "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T, "", upd_MD_4T.ToString(), out resulttb
                        , "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_16T, "", upd_MD_16T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_0F, "", upd_MD_0F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }
                    #endregion 

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        return result;
                    }

                    if (!MyUtility.Check.Empty(updateMDivisionPODetailCLocation))
                    {
                        result = DBProxy.Current.Execute(null, updateMDivisionPODetailCLocation);
                        if (!result)
                        {
                            _transactionscope.Dispose();
                            return result;
                        }
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    //MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    return new DualResult(false, "Commit transaction error.", ex);
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            return result;
        }

        public static DualResult FillIssueDetailBarcodeNo(IList<DataRow> result)
        {
            int nCount = 0;
            int drcount = result.Count;
            IList<string> cListBarcodeNo;
            try
            {
                cListBarcodeNo = GetBatchID("", "Issue_Detail", default(DateTime), 3, "BarcodeNo", batchNumber: drcount, sequenceMode: 2, sequenceLength: 4, ignoreSeq: 3, orderByCol: "Ukey");
                foreach (DataRow dr in result)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (MyUtility.Check.Empty(dr["BarcodeNo"]))
                    {
                        string keyWord = MyUtility.GetValue.Lookup($"select FtyGroup from orders where id = '{dr["POID"]}'");
                        dr["BarcodeNo"] = keyWord + cListBarcodeNo[nCount];
                        nCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex.Message);
            }

            return new DualResult(true);
        }

        public static List<string> GetBatchID(string keyWord, string tableName, DateTime refDate = default(DateTime), int format = 2, string checkColumn = "ID", String connectionName = null, int sequenceMode = 1, int sequenceLength = 0, int batchNumber = 1, int ignoreSeq = 0, string orderByCol = "")
        {
            List<string> IDList = new List<string>();

            if (String.IsNullOrWhiteSpace(tableName)) return IDList;
            if (refDate == DateTime.MinValue) refDate = DateTime.Today;

            string TaiwanYear;
            switch (format)
            {
                case 1:     // A yy xxxx
                    keyWord = keyWord.ToUpper().Trim() + refDate.ToString("yy");
                    break;
                case 2:     // A yyMM xxxx
                    keyWord = keyWord.ToUpper().Trim() + refDate.ToString("yyMM");
                    break;
                case 3:      // A yyMMdd xxxx
                    keyWord = keyWord.ToUpper().Trim() + refDate.ToString("yyMMdd");
                    break;
                case 4:      // A yyyyMM xxxxx
                    keyWord = keyWord.ToUpper().Trim() + refDate.ToString("yyyyMM");
                    break;
                case 5:     // 民國年 A yyyMM xxxx
                    TaiwanYear = ((refDate.Year - 1911).ToString()).PadLeft(3, '0');
                    keyWord = keyWord.ToUpper().Trim() + TaiwanYear + refDate.ToString("MM");
                    break;
                case 6:     // A xxxx
                    keyWord = keyWord.ToUpper().Trim();
                    break;
                case 7:    // A yyyy xxxx
                    keyWord = keyWord.ToUpper().Trim() + refDate.ToString("yyyy");
                    break;
                case 8:    // 民國年 A yyyMMdd xxxx
                    TaiwanYear = ((refDate.Year - 1911).ToString()).PadLeft(3, '0');
                    keyWord = keyWord.ToUpper().Trim() + TaiwanYear + refDate.ToString("MM") + refDate.ToString("dd");
                    break;
                default:
                    return IDList;
            }

            //判斷schema欄位的結構長度

            DualResult result = null;
            DataTable dtID = null;
            int columnTypeLength = 0;
            ITableSchema tableSchema = null;

            if (result = DBProxy.Current.GetTableSchema(connectionName, tableName, out tableSchema))
            {
                foreach (IColumnSchema cs in tableSchema.Columns)
                {
                    if (cs.ColumnName.ToUpper() == checkColumn.ToUpper())
                        columnTypeLength = cs.MaxLength;
                }
                if (columnTypeLength == 0) return IDList;
            }
            else
                return IDList;

            string orderBy = MyUtility.Check.Empty(orderByCol) ? $"SUBSTRING({checkColumn},{ignoreSeq + 1},{columnTypeLength})" : orderByCol;
            string sqlCmd = $@" SELECT TOP 1 {checkColumn} into #tmp FROM {tableName} with (nolock) ORDER BY {orderBy} DESC
                SELECT {checkColumn} from #tmp where {checkColumn} like '%{keyWord.Trim()}%'";


            if (result = DBProxy.Current.Select(connectionName, sqlCmd, out dtID))
            {
                if (dtID.Rows.Count > 0)
                {
                    string lastID = dtID.Rows[0][checkColumn].ToString().Substring(ignoreSeq);
                    while (batchNumber > 0)
                    {
                        lastID = keyWord + GetNextValue(lastID.Substring(keyWord.Length), sequenceMode);
                        IDList.Add(lastID);
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
                            while (batchNumber > 0)
                            {
                                IDList.Add(keyWord + nextValue);
                                nextValue = GetNextValue(nextValue.PadLeft(sequenceLength, '0'), sequenceMode);
                                batchNumber = batchNumber - 1;
                            }
                        }
                    }
                    else
                    {
                        string nextValue = GetNextValue("0".PadLeft(columnTypeLength - keyWord.Length, '0'), sequenceMode);
                        while (batchNumber > 0)
                        {
                            IDList.Add(keyWord + nextValue);
                            nextValue = GetNextValue(nextValue.PadLeft(columnTypeLength - keyWord.Length, '0'), sequenceMode);
                            batchNumber = batchNumber - 1;
                        }
                    }
                }
            }

            return IDList;
        }

        public static string GetNextValue(string strValue, int sequenceMode)
        {
            char[] charValue = strValue.ToArray<char>();
            int sequenceValue = 0;
            string returnValue = "";
            int charAscii = 0;

            if (sequenceMode == 1)
            {
                // 當第一個字為字母
                if (System.Convert.ToInt32(charValue[0]) >= 65 && System.Convert.ToInt32(charValue[0]) <= 90)
                {
                    sequenceValue = System.Convert.ToInt32(strValue.Substring(1));
                    // 進位處理
                    if (((sequenceValue + 1).ToString().Length > sequenceValue.ToString().Length) && string.IsNullOrWhiteSpace(strValue.Substring(1).Replace("9", "")))
                    {
                        charAscii = System.Convert.ToInt32(charValue[0]);
                        if (charAscii + 1 > 90)
                        {
                            return strValue;
                        }
                        else
                        {
                            sequenceValue = 1;
                            if (charAscii == 72 || charAscii == 78) // I or O略過
                            {
                                charValue[0] = System.Convert.ToChar(charAscii + 2);
                            }
                            else
                            {
                                charValue[0] = System.Convert.ToChar(charAscii + 1);
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
                    sequenceValue = System.Convert.ToInt32(strValue);
                    // 進位處理
                    if (((sequenceValue + 1).ToString().Length > sequenceValue.ToString().Length) && string.IsNullOrWhiteSpace(strValue.Replace("9", "")))
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
                    charAscii = System.Convert.ToInt32(charValue[i]);

                    if (charAscii == 57)   // 遇9跳A
                    {
                        charValue[i] = 'A';
                        break;
                    }

                    if (charAscii == 72 || charAscii == 78) // I or O略過
                    {
                        charValue[i] = System.Convert.ToChar(charAscii + 2);
                        break;
                    }

                    if (charAscii == 90)  //當字母為Z
                    {
                        if (i > 0)
                        {
                            charValue[i] = '0';
                            continue;
                        }
                        else
                        {
                            return strValue;    //超出最大上限ZZZ...., 返回原值
                        }
                    }

                    charValue[i] = System.Convert.ToChar(charAscii + 1);
                    break;
                }
                returnValue = new String(charValue);
            }
            return returnValue;
        }
        public static DataTable RollTranscation(string PoID, string Seq1, string Seq2)
        {
            DataTable dt = new DataTable();
            DualResult result;
            string sqlcmd = string.Format(@"select tmp.Roll,
[stocktype] = case when stocktype = 'B' then 'Bulk'
                   when stocktype = 'I' then 'Invertory'
			       when stocktype = 'O' then 'Scrap' End
,Dyelot,IssueDate,ID,name,inqty,outqty,adjust,Remark,location,
sum(TMP.inqty - TMP.outqty+tmp.adjust) 
over (partition by tmp.stocktype,tmp.roll,tmp.dyelot order by tmp.IssueDate,tmp.stocktype,tmp.inqty desc,tmp.iD ) as [balance] 
,poid = '{0}',Seq1 = '{1}' , Seq2 = '{2}'
from (
	select b.roll,b.stocktype,b.dyelot,a.IssueDate, a.id
,Case type when 'A' then 'P35. Adjust Bulk Qty' 
                when 'B' then 'P34. Adjust Stock Qty' end as name
,0 as inqty,0 as outqty, sum(QtyAfter - QtyBefore) adjust, a.remark ,'' location
from Adjust a WITH (NOLOCK) , Adjust_Detail b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,type,b.roll,b.stocktype,b.dyelot

union all
	select b.FromRoll,b.FromStockType,b.FromDyelot,a.IssueDate, a.id
,case type when 'A' then 'P31. Material Borrow From' 
                when 'B' then 'P32. Material Give Back From' end as name
,0 as inqty, sum(qty) released,0 as adjust, a.remark ,'' location
from BorrowBack a WITH (NOLOCK) , BorrowBack_Detail b WITH (NOLOCK) 
where Status='Confirmed' and FromPoId ='{0}' and FromSeq1 = '{1}'and FromSeq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, FromPoId, FromSeq1,FromSeq2, a.remark,a.IssueDate,b.FromRoll,b.FromStockType,b.FromDyelot,a.type
union all
	select b.ToRoll,b.ToStockType,b.ToDyelot,issuedate, a.id
,case type when 'A' then 'P31. Material Borrow To' 
                when 'B' then 'P32. Material Give Back To' end as name
, sum(qty) arrived,0 as ouqty,0 as adjust, a.remark ,'' location
from BorrowBack a WITH (NOLOCK) , BorrowBack_Detail b WITH (NOLOCK) 
where Status='Confirmed' and ToPoid ='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, ToPoid, ToSeq1,ToSeq2, a.remark,a.IssueDate,b.ToRoll,b.ToStockType,b.ToDyelot,a.type
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
	,case type when 'A' then 'P10. Issue Fabric to Cutting Section' 
			when 'B' then 'P11. Issue Sewing Material by Transfer Guide' 
			when 'C' then 'P12. Issue Packing Material by Transfer Guide' 
			when 'D' then 'P13. Issue Material by Item'
			when 'E' then 'P72. Transfer Inventory to Bulk (Confirm)'
			when 'F' then 'P75. Material Borrow cross M (Confirm)'
			when 'G' then 'P77. Material Return Back cross M (Request)' 
			when 'H' then 'P14. Issue Thread Allowance' 
            end name
	,0 as inqty, sum(Qty) released,0 as adjust, a.remark,'' location
from Issue a WITH (NOLOCK) , Issue_Detail b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,a.type,b.roll,b.stocktype,b.dyelot,a.type                                                             
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
	,case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
                              when 'F' then 'P16. Issue Fabric Lacking & Replacement' end as name
	, 0 as inqty,sum(b.Qty) outqty ,0 as adjust, a.remark ,'' location
from IssueLack a WITH (NOLOCK) , IssueLack_Detail b WITH (NOLOCK) 
where Status in ('Confirmed','Closed') and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    and a.type != 'L'  --新增MDivisionID條件，避免下面DataRelation出錯 1026新增排除Lacking
group by a.id, poid, seq1,Seq2, a.remark  ,a.IssueDate,a.FabricType,b.roll,b.stocktype,b.dyelot                        



union all

	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
	,case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
                              when 'F' then 'P16. Issue Fabric Lacking & Replacement' end as name
	, 0 as inqty,0 outqty ,0 as adjust, a.remark ,'' location
from IssueLack a WITH (NOLOCK) , IssueLack_Detail b WITH (NOLOCK) 
where Status in ('Confirmed','Closed') and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
and a.type = 'L'  --20190305 新增Type= Lacking,則OutQty = 0
group by a.id, poid, seq1,Seq2, a.remark  ,a.IssueDate,a.FabricType,b.roll,b.stocktype,b.dyelot   
                                       
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id,'P17. R/Mtl Return' name, 0 as inqty, sum(0.00 - b.Qty) released,0 as adjust, remark,'' location
from IssueReturn a WITH (NOLOCK) , IssueReturn_Detail b WITH (NOLOCK) 
where status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.Id, poid, seq1,Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot                                                                           
union all
	select b.roll,b.stocktype,b.dyelot
        ,case type when 'A' then a.ETA else a.WhseArrival end as issuedate
        , a.id
	    ,case type when 'A' then 'P07. Material Receiving' 
                        when 'B' then 'P08. Warehouse Shopfloor Receiving' end name
	    , sum(b.StockQty) inqty,0 as outqty,0 as adjust,'' remark ,'' location
    from Receiving a WITH (NOLOCK) , Receiving_Detail b WITH (NOLOCK) 
    where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
        --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
    group by a.Id, poid, seq1,Seq2,a.WhseArrival,a.Type,b.roll,b.stocktype,b.dyelot,a.eta
union all
	select b.roll,b.stocktype,b.dyelot,issuedate
, a.id,'P37. Return Receiving Material' name, sum(-Qty) inqty,0 as released,0 as adjust, a.remark,'' location
from ReturnReceipt a WITH (NOLOCK) , ReturnReceipt_Detail b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯 
group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot                                                                           
union all
	select b.FromRoll,b.FromStockType,b.FromDyelot,issuedate, a.id
	,case type when 'B' then 'P23. Transfer Inventory to Bulk' 
                    when 'A' then 'P22. Transfer Bulk to Inventory' 
                    when 'C' then 'P36. Transfer Scrap to Inventory' 
                    when 'D' then 'P25. Transfer Bulk to Scrap' 
                    when 'E' then 'P24. Transfer Inventory to Scrap'
    end as name
	, 0 as inqty, sum(Qty) released,0 as adjust ,isnull(a.remark,'') remark ,'' location
from SubTransfer a WITH (NOLOCK) , SubTransfer_Detail b WITH (NOLOCK) 
where Status='Confirmed' and Frompoid='{0}' and Fromseq1 = '{1}' and FromSeq2 = '{2}'  and a.id = b.id
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
    and a.type <> 'C'  --排除C to B 的轉出紀錄，因目前不需要C倉交易紀錄，避免下面DataRelation出錯
group by a.id, frompoid, FromSeq1,FromSeq2,a.IssueDate,a.Type,b.FromRoll,b.FromStockType,b.FromDyelot,a.Type,a.remark
                                                                             
union all
	select b.ToRoll,b.ToStockType,b.ToDyelot,issuedate, a.id
	,case type when 'B' then 'P23. Transfer Inventory to Bulk' 
                    when 'A' then 'P22. Transfer Bulk to Inventory' 
                    when 'C' then 'P36. Transfer Scrap to Inventory' end as name
	        , sum(Qty) arrived,0 as ouqty,0 as adjust, a.remark
	        ,isnull((Select cast(tmp.ToLocation as nvarchar)+',' 
                        from (select b1.ToLocation 
                                    from SubTransfer a1 WITH (NOLOCK) 
                                    inner join SubTransfer_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
                                    where a1.status = 'Confirmed' and (b1.ToLocation is not null or b1.ToLocation !='')
                                        and b1.ToPoid = b.ToPoid
                                        and b1.ToSeq1 = b.ToSeq1
                                        and b1.ToSeq2 = b.ToSeq2 group by b1.ToLocation) tmp 
                        for XML PATH('')),'') as ToLocation
from SubTransfer a WITH (NOLOCK) , SubTransfer_Detail b WITH (NOLOCK) 
where Status='Confirmed' and ToPoid='{0}' and ToSeq1 = '{1}'and ToSeq2 = '{2}'  and a.id = b.id  
    AND TYPE not in ('D','E')  --570: WAREHOUSE_P03_RollTransaction。C倉不用算，所以要把TYPE為D及E的資料濾掉
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, ToPoid, ToSeq1,ToSeq2, a.remark ,a.IssueDate,b.ToRoll,b.ToStockType,b.ToDyelot,a.type	    

union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
            ,'P18. Transfer In' name
            , sum(Qty) arrived,0 as ouqty,0 as adjust, a.remark
	,(Select cast(tmp.Location as nvarchar)+',' 
                        from (select b1.Location 
                                    from TransferIn a1 WITH (NOLOCK) 
                                    inner join TransferIn_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
                                    where a1.status = 'Confirmed' and (b1.Location is not null or b1.Location !='')
                                        and b1.Poid = b.Poid
                                        and b1.Seq1 = b.Seq1
                                        and b1.Seq2 = b.Seq2 group by b1.Location) tmp 
                        for XML PATH('')) as Location
from TransferIn a WITH (NOLOCK) , TransferIn_Detail b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot                                                                        
union all
	select b.roll,b.stocktype,b.dyelot,issuedate, a.id
            ,'P19. Transfer Out' name
            , 0 as inqty, sum(Qty) released,0 as adjust, a.remark,'' location
from TransferOut a WITH (NOLOCK) , TransferOut_Detail b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, poid, Seq1,Seq2, a.remark,a.IssueDate,b.roll,b.stocktype,b.dyelot

union all
select b.roll,b.stocktype,b.dyelot,issuedate, a.id
    ,case type when 'B' then 'P73. Transfer Inventory to Bulk cross M (Receive)' 
	when 'D' then 'P76. Material Borrow cross M (Receive)' 
	when 'G' then 'P78. Material Return Back cross M (Receive)'  end name
    , sum(Qty) as inqty, 0 released,0 as adjust, a.remark,'' location
from RequestCrossM a WITH (NOLOCK) , RequestCrossM_Receive b WITH (NOLOCK) 
where Status='Confirmed' and poid='{0}' and seq1 = '{1}'and seq2 = '{2}'  and a.id = b.id 
    --and a.ToMDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
group by a.id, poid, seq1,Seq2, a.remark,a.IssueDate,a.type,b.roll,b.stocktype,b.dyelot,a.type 

) tmp where stocktype <> 'O'
group by IssueDate,inqty,outqty,adjust,id,Remark,location,tmp.name,tmp.roll,tmp.stocktype,tmp.dyelot
"
                , PoID
                , Seq1
                , Seq2
                , Sci.Env.User.Keyword);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out dt)))
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return null;
            }
            else
            {
                return dt;
            }
        }

        public static DualResult ReTransferMtlToScrapByPO(string poID, List<DataRow> listMtlItem)
        {
            string sqlRetransferToScrap = $@"
        -- 新增 報廢單主檔 & 明細檔
		IF EXISTS(SELECT * FROM [dbo].[SubTransfer] S WITH (NOLOCK) WHERE S.ID = '{poID}' AND S.Status='Confirmed')
			update [dbo].[SubTransfer] set [EditName]= '{Env.User.UserID}' , [EditDate] = GETDATE() WHERE ID = '{poID}' 
		ELSE 
		BEGIN
			INSERT INTO [dbo].[SubTransfer]
				   ([Id]				   ,[MDivisionID]				   ,[FactoryID]
				   ,[Type]
				   ,[IssueDate]				   ,[Status]				   ,[Remark]
				   ,[AddName]				   ,[AddDate]				   ,[EditName]
				   ,[EditDate])
			VALUES
					('{poID}' 
					,'{Env.User.Keyword}' 
					,'{Env.User.Factory}'
					,'D' -- A2C
					,GETDATE()
					,'Confirmed'
					,'Add by Warehouse Close'
					,'{Env.User.UserID}' 
					,GETDATE()
					,'{Env.User.UserID}' 
					,GETDATE()
					);
		END
";
            foreach (var retransferToScrapItem in listMtlItem)
            {
                string mDivisionPoDetailUkey = MyUtility.Check.Empty(retransferToScrapItem["MDivisionPoDetailUkey"]) ? "NULL" : retransferToScrapItem["MDivisionPoDetailUkey"].ToString();
                sqlRetransferToScrap += $@" 
    exec dbo.usp_ReTransferMtlToScrap {retransferToScrapItem["Ukey"]}
    exec dbo.usp_SingleItemRecaculate {mDivisionPoDetailUkey}, '{poID}', '{retransferToScrapItem["Seq1"]}', '{retransferToScrapItem["Seq2"]}'
    ";
            }

            return DBProxy.Current.Execute(null, sqlRetransferToScrap);
        }

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

        public static List<string> GetBarcodeNo(string TableName, string keyWord, int batchNumber = 1 ,int DateType = 3, string checkColumn = "Barcode", String connectionName = null, int sequenceMode = 1, int sequenceLength = 0)
        {
            List<string> IDList = new List<string>();
            if (MyUtility.Check.Empty(TableName))
            {
                throw new Exception("Parameter - tableName is not specified..");
            }

            DateTime today = DateTime.Today;
            string TaiwanYear;
            switch (DateType)
            {
                case 1:     // A yy xxxx
                    keyWord = keyWord.ToUpper().Trim() + today.ToString("yy");
                    break;
                case 2:     // A yyMM xxxx
                    keyWord = keyWord.ToUpper().Trim() + today.ToString("yyMM");
                    break;
                case 3:      // A yyMMdd xxxx
                    keyWord = keyWord.ToUpper().Trim() + today.ToString("yyMMdd");
                    break;
                case 4:      // A yyyyMM xxxxx
                    keyWord = keyWord.ToUpper().Trim() + today.ToString("yyyyMM");
                    break;
                case 5:     // 民國年 A yyyMM xxxx
                    TaiwanYear = ((today.Year - 1911).ToString()).PadLeft(3, '0');
                    keyWord = keyWord.ToUpper().Trim() + TaiwanYear + today.ToString("MM");
                    break;
                case 6:     // A xxxx
                    keyWord = keyWord.ToUpper().Trim();
                    break;
                case 7:    // A yyyy xxxx
                    keyWord = keyWord.ToUpper().Trim() + today.ToString("yyyy");
                    break;
                case 8:    // 民國年 A yyyMMdd xxxx
                    TaiwanYear = ((today.Year - 1911).ToString()).PadLeft(3, '0');
                    keyWord = keyWord.ToUpper().Trim() + TaiwanYear + today.ToString("MM") + today.ToString("dd");
                    break;
                default:
                    return IDList;
            }

            //判斷schema欄位的結構長度
            string returnID = "";
            string sqlCmd = string.Format("SELECT TOP 1 {0} FROM {1} WHERE {2} LIKE '{3}%' ORDER BY {4} DESC", checkColumn, TableName, checkColumn, keyWord.Trim(), checkColumn);

            DualResult result = null;
            DataTable dtID = null;
            int columnTypeLength = 0;
            ITableSchema tableSchema = null;

            if (result = DBProxy.Current.GetTableSchema(connectionName, TableName, out tableSchema))
            {
                foreach (IColumnSchema cs in tableSchema.Columns)
                {
                    if (cs.ColumnName.ToUpper() == checkColumn.ToUpper())
                    {
                        columnTypeLength = cs.MaxLength;
                    }
                }
                if (columnTypeLength == 0)
                {
                    throw new Exception("Parameter - checkColumn is not found!");
                }
            }
            else
            {
                throw new Exception(result.ToString());
            }

            if (result = DBProxy.Current.Select(connectionName, sqlCmd, out dtID))
            {
                if (dtID.Rows.Count > 0)
                {
                    string lastID = dtID.Rows[0][checkColumn].ToString();
                    while (batchNumber > 0)
                    {
                        lastID = keyWord + GetNextValue(lastID.Substring(keyWord.Length), sequenceMode);
                        IDList.Add(lastID);
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
                            while (batchNumber > 0)
                            {
                                IDList.Add(keyWord + nextValue);
                                nextValue = GetNextValue(nextValue.PadLeft(sequenceLength, '0'), sequenceMode);
                                batchNumber = batchNumber - 1;
                            }
                        }
                    }
                    else
                    {
                        string nextValue = GetNextValue("0".PadLeft(columnTypeLength - keyWord.Length, '0'), sequenceMode);
                        while (batchNumber > 0)
                        {
                            IDList.Add(keyWord + nextValue);
                            nextValue = GetNextValue(nextValue.PadLeft(columnTypeLength - keyWord.Length, '0'), sequenceMode);
                            batchNumber = batchNumber - 1;
                        }
                    }
                }
            }
            else
            {
                throw new Exception(result.ToString());
            }

            return IDList;
        }

        private class NowDetail
        {
            public string POID { get; set; }
            public string Seq1 { get; set; }
            public string Seq2 { get; set; }
            public List<string> DB_CLocations { get; set; }
        }
        public static DualResult UpdateFtyInventoryMDivisionPoDetail(IList<DataRow> detailDatas)
        {
            StringBuilder sqlupd2 = new StringBuilder();
            List<NowDetail> NowDetails = new List<NowDetail>();
            DualResult result;//, result2;
            string upd_MD_2T = "";
            string upd_Fty_26F = "";


            //先把表身POID Seq1 2原本的MDivisionPoDetail CLocation記下來  ISP20191578
            foreach (DataRow item in detailDatas.Where(o => o["StockType"].ToString() == "O" && o["ToLocation"].ToString() != "").ToList())
            {
                string POID = item["POID"].ToString();
                string Seq1 = item["Seq"].ToString().Split(' ')[0];
                string Seq2 = item["Seq"].ToString().Split(' ')[1];

                DataTable DT_MDivisionPoDetail;
                //從MDivisionPoDetail出現有的Location
                DBProxy.Current.Select(null, $@"
SELECt CLocation
FROM MDivisionPoDetail
WHERE POID='{POID}'
AND Seq1='{Seq1}' AND Seq2='{Seq2}'
", out DT_MDivisionPoDetail);

                List<string> DB_CLocations = DT_MDivisionPoDetail.Rows[0]["CLocation"].ToString().Split(',').Where(o => o != "").ToList();

                NowDetail nData = new NowDetail()
                {
                    POID = POID,
                    Seq1 = Seq1,
                    Seq2 = Seq2,
                    DB_CLocations = DB_CLocations
                };
                NowDetails.Add(nData);

            }

            #region 更新庫存數量 ftyinventory

            DataTable newDt = detailDatas[0].Table.Clone();
            foreach (DataRow dtr in detailDatas)
            {
                string[] dtrLocation = dtr["ToLocation"].ToString().Split(',');
                dtrLocation = dtrLocation.Distinct().ToArray();

                if (dtrLocation.Length == 1)
                {
                    DataRow newDr = newDt.NewRow();
                    newDr.ItemArray = dtr.ItemArray;
                    newDt.Rows.Add(newDr);
                }
                else
                {
                    foreach (string location in dtrLocation)
                    {
                        DataRow newDr = newDt.NewRow();
                        newDr.ItemArray = dtr.ItemArray;
                        newDr["ToLocation"] = location;
                        newDt.Rows.Add(newDr);
                    }
                }
            }

            var data_Fty_26F = (from b in newDt.AsEnumerable()
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

            upd_Fty_26F = Prgs.UpdateFtyInventory_IO(26, null, false);
            #endregion 更新庫存數量 po_supp_detail & ftyinventory

            #region 更新庫存數量 mdivisionPoDetail
            var data_MD_2T = (from b in detailDatas
                              group b by new
                              {
                                  poid = b.Field<string>("poid"),
                                  seq1 = b.Field<string>("seq1"),
                                  seq2 = b.Field<string>("seq2"),
                                  stocktype = b.Field<string>("stocktype")
                              } into m
                              select new Prgs_POSuppDetailData
                              {
                                  poid = m.First().Field<string>("poid"),
                                  seq1 = m.First().Field<string>("seq1"),
                                  seq2 = m.First().Field<string>("seq2"),
                                  location = string.Join(",", m.Select(r => r.Field<string>("ToLocation")).Distinct()),
                                  qty = 0,
                                  stocktype = m.First().Field<string>("stocktype")
                              }).ToList();
            #endregion

            #region ISP20191578 ToLocation的資料一併更新回MDivisionPODetail.CLocation欄位
            string updateMDivisionPODetailCLocation = string.Empty;
            try
            {
                foreach (DataRow item in detailDatas.Where(o => o["StockType"].ToString() == "O" && o["ToLocation"].ToString() != ""))
                {
                    string POID = item["POID"].ToString();
                    string Seq1 = item["Seq"].ToString().Split(' ')[0];
                    string Seq2 = item["Seq"].ToString().Split(' ')[1];

                    List<string> New_CLocationList = detailDatas.Where(o => o["POID"].ToString() == POID && o["Seq"].ToString() == (Seq1 + " " + Seq2) && o["ToLocation"].ToString() != "")
                        .Select(o => o["ToLocation"].ToString())
                        .Distinct().ToList();

                    //List<string> DB_CLocations = DT_MDivisionPoDetail.Rows[0]["CLocation"].ToString().Split(',').Where(o => o != "").ToList();
                    List<string> DB_CLocations = NowDetails.Where(o => o.POID == POID && o.Seq1 == Seq1 && o.Seq2 == Seq2).FirstOrDefault().DB_CLocations;

                    List<string> Fincal = new List<string>();

                    foreach (var New_CLocation in New_CLocationList)
                    {
                        if (DB_CLocations.Count == 0 || !DB_CLocations.Contains(New_CLocation))
                        {
                            DB_CLocations.Add(New_CLocation);
                        }
                    }

                    foreach (var CLocation in DB_CLocations.Distinct().ToList())
                    {
                        foreach (var a in CLocation.Split(',').Where(o => o != "").Distinct().ToList())
                        {
                            if (!Fincal.Contains(a))
                            {
                                Fincal.Add(a);
                            }
                        }
                    }

                    string cmd = $@"
UPDATE MDivisionPoDetail
SET CLocation='{Fincal.Distinct().ToList().JoinToString(",")}'
WHERE POID='{POID}' AND Seq1='{Seq1}' AND Seq2='{Seq2}'

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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_26F, "", upd_Fty_26F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        return result;
                    }
                    #endregion

                    #region MDivisionPoDetail


                    upd_MD_2T = Prgs.UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, "", upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
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
    }
    public class Prgs_POSuppDetailData
    {
        public string poid { get; set; }
        public string seq1 { get; set; }
        public string seq2 { get; set; }
        public string stocktype { get; set; }
        public decimal qty { get; set; }
        public string location { get; set; }
    }
}
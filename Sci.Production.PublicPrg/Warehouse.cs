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
    values (src.poid,src.seq1,src.seq2,src.qty,src.location);";
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
        public static string UpdateFtyInventory_IO(int type, IList<DataRow> datas, bool encoded)
        {
            string sqlcmd = "";
            switch (type)
            {
                case 2:
                    #region 更新 inqty
                    sqlcmd = @"
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)
alter table #TmpSource alter column roll varchar(15)

select distinct poid, seq1, seq2, stocktype, roll = RTRIM(LTRIM(isnull(roll, ''))) , qty, dyelot = isnull(dyelot, '')
into #tmpS1
from #TmpSource

merge dbo.FtyInventory as target
using #tmpS1 as s
    on target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.stocktype = s.stocktype and target.roll = s.roll and target.dyelot = s.dyelot
when matched then
    update
    set inqty = isnull(inqty,0.00) + s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select ukey from dbo.MDivisionPoDetail WITH (NOLOCK) 
			 where poid = s.poid and seq1 = s.seq1 and seq2 = s.seq2)
			 ,s.poid,s.seq1,s.seq2,s.roll,s.dyelot,s.stocktype,s.qty);
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
                    sqlcmd += @"drop table #tmpS1; 
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
when not matched then
    insert ([ukey],[mtllocationid]) 
       values (s.ukey,isnull(s.tolocation,''));

drop table #tmp_L_K
drop table #TmpSource
";
                    #endregion
                    break;
            }
            return sqlcmd;
        }
        #endregion
        #region -- SelePoItem --
        public static string selePoItemSqlCmd = @"
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
from dbo.PO_Supp_Detail p WITH (NOLOCK) 
inner join Orders o on p.id = o.id
inner join Factory f on o.FtyGroup = f.id
left join dbo.mdivisionpodetail m WITH (NOLOCK) on m.poid = p.id and m.seq1 = p.seq1 and m.seq2 = p.seq2
inner join View_unitrate v on v.FROM_U = p.POUnit 
	                          and v.TO_U = dbo.GetStockUnitBySPSeq (p.id, p.seq1, p.seq2)
where p.id ='{0}'
and p.Junk=0
";
        /// <summary>
        /// 右鍵開窗選取採購項
        /// </summary>
        /// <param name="poid"></param>
        /// <param name="defaultseq"></param>
        /// <param name="filters"></param>
        /// <returns>Sci.Win.Tools.SelectItem</returns>
        public static Sci.Win.Tools.SelectItem SelePoItem(string poid, string defaultseq, string filters = null)
        {
            DataTable dt;
            string PoItemSql = selePoItemSqlCmd;
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
            else//P29 Auto Pick
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
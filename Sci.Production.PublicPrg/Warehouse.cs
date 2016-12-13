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

namespace Sci.Production.PublicPrg
{

    public static partial class Prgs
    {
        #region -- UpdatePO_Supp_Detail --
        /// <summary>
        /// UpdatePO_Supp_Detail()
        /// *	更新 Po3 的庫存
        /// *-----------------------------------------------------
        /// * 使用新方法↓
        /// * 1.各程式 OnDetailSelectCommandPrepare()欄位名稱要與這Sqlcommand對上 EX:統一用Location
        /// * 2.延續舊做法True 為增加 / False為減少
        /// * 3.Case2,8才需要傳List<>過來,重組Location
        /// 
        /// * Type	: 
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
        /// 
        
        //新(整批)A倉
        public static string UpdateMPoDetail_A(int type, List<Prgs_POSuppDetailData_A> datas, bool encoded, bool attachLocation = true)
        {
            #region 以原本的datas的5keys 去ftyinventory撈location和原本loction重組以逗號分開,塞回原本資料
            DataTable TBattachlocation;
            if (attachLocation)
            {
                string sqlcmdforlocation = @"
alter table #Tmp alter column mdivisionid varchar(10)
alter table #Tmp alter column poid varchar(20)
alter table #Tmp alter column seq1 varchar(3)
alter table #Tmp alter column seq2 varchar(3)
alter table #Tmp alter column stocktype varchar(1)

select mdivisionid,poid,seq1,seq2,qty,stocktype
,[location] = stuff(L.locationid,1,1,'' )
from #tmp t
OUTER APPLY(
	SELECT locationid=(
		select distinct concat(',',u.location)
		from 
		(
			select mdivisionid,poid,seq1,seq2,stocktype,location
			from #tmp f
			where f.mdivisionid = t.MDivisionID and f.poid = t.POID and f.seq1 = t.Seq1 
			and f.seq2 = t.Seq2 and f.stocktype = t.StockType and f.location !=''
			union
			select mdivisionid,poid,seq1,seq2,stocktype,[location] = fd.mtllocationid
			from ftyinventory f 
			inner join ftyinventory_detail fd on f.ukey = fd.ukey 
			where f.mdivisionid = t.MDivisionID and f.poid = t.POID and f.seq1 = t.Seq1 
			and f.seq2 = t.Seq2 and f.stocktype = t.StockType
		)u
		for xml path('')
	)
)L";
                MyUtility.Tool.ProcessWithObject(datas, "", sqlcmdforlocation, out TBattachlocation, "#Tmp");
                var newDatas = TBattachlocation.AsEnumerable().Select(w =>
                        new Prgs_POSuppDetailData_A
                        {
                            mdivisionid = w.Field<string>("mdivisionid"),
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
            #endregion
            String sqlcmd = "";
            switch (type)
            {
                case 2:
                    #region -- Case 2 InQty --
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column stocktype varchar(1)

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2 and target.mdivisionid = src.mdivisionid
when matched and src.stocktype = 'I' then
	update 
	set target.inqty = isnull(target.inqty,0.00) + src.qty , target.blocation = src.location
when not matched by target and src.stocktype = 'I' then
    insert ([Poid],[Seq1],[Seq2],[MDivisionID],[inqty],[blocation])
    values (src.poid,src.seq1,src.seq2,src.mdivisionid,src.qty,src.location);

merge dbo.mdivisionpodetail as target
using  #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2 and target.mdivisionid = src.mdivisionid
when matched and src.stocktype = 'B' then
	update 
	set target.inqty = isnull(target.inqty,0.00) + src.qty , target.alocation = src.location
when not matched by target and src.stocktype = 'B' then
    insert ([Poid],[Seq1],[Seq2],[MDivisionID],[inqty],[alocation])
    values (src.poid,src.seq1,src.seq2,src.mdivisionid,src.qty,src.location);";
                    }
                    else
                    {
                        sqlcmd = @"
alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t
set t.inqty = isnull(t.inqty,0.00) - s.qty
from mdivisionpodetail t
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2 and t.mdivisionid = s.mdivisionid;";
                    }
                    #endregion
                    break;
            }
            return sqlcmd;
        }
        //新(整批)B倉
        public static string UpdateMPoDetail(int type, List<Prgs_POSuppDetailData_B> datas, bool encoded, bool attachLocation = true)
        {
            #region 以原本的datas的5keys 去ftyinventory撈location和原本loction重組以逗號分開,塞回原本資料
            DataTable TBattachlocation;
            if (datas != null)
            {
                if (attachLocation)
                {
                    string sqlcmdforlocation = @"
alter table #Tmp alter column mdivisionid varchar(10)
alter table #Tmp alter column poid varchar(20)
alter table #Tmp alter column seq1 varchar(3)
alter table #Tmp alter column seq2 varchar(3)
alter table #Tmp alter column stocktype varchar(1)

select mdivisionid,poid,seq1,seq2,qty,stocktype
,[location] = stuff(L.locationid,1,1,'' )
from #tmp t
OUTER APPLY(
	SELECT locationid=(
		select distinct concat(',',u.location)
		from 
		(
			select mdivisionid,poid,seq1,seq2,stocktype,location
			from #tmp f
			where f.mdivisionid = t.MDivisionID and f.poid = t.POID and f.seq1 = t.Seq1 
			and f.seq2 = t.Seq2 and f.stocktype = t.StockType and f.location !=''
			union
			select mdivisionid,poid,seq1,seq2,stocktype,[location] = fd.mtllocationid
			from ftyinventory f 
			inner join ftyinventory_detail fd on f.ukey = fd.ukey 
			where f.mdivisionid = t.MDivisionID and f.poid = t.POID and f.seq1 = t.Seq1 
			and f.seq2 = t.Seq2 and f.stocktype = t.StockType
		)u
		for xml path('')
	)
)L";
                    MyUtility.Tool.ProcessWithObject(datas, "", sqlcmdforlocation, out TBattachlocation, "#Tmp");
                    var newDatas = TBattachlocation.AsEnumerable().Select(w =>
                            new Prgs_POSuppDetailData_B
                            {
                                mdivisionid = w.Field<string>("mdivisionid"),
                                poid = w.Field<string>("poid"),
                                seq1 = w.Field<string>("seq1"),
                                seq2 = w.Field<string>("seq2"),
                                stocktype = w.Field<string>("stocktype"),
                                qty = w.Field<decimal>("qty"),
                                location = w.Field<string>("location"),
                            }).ToList();
                    datas.Clear();
                    datas.AddRange(newDatas);
                    #region 一筆一筆更新法location
                    //string tmplocation = "";//用此法要搬上去
                    //foreach (var item in datas)
                    //                {
                    //                    tmplocation = MyUtility.GetValue.Lookup(string.Format(@"
                    //select t.mtllocationid+','
                    //from (select distinct mtllocationid from ftyinventory f inner join ftyinventory_detail fd on f.ukey = fd.ukey 
                    //where f.mdivisionid ='{0}' and f.poid = '{1}' and f.seq1='{2}' and f.seq2='{3}' and stocktype='{4}') t 
                    //for xml path('')"
                    //                        , item.mdivisionid, item.poid, item.seq1, item.seq2, item.stocktype));
                    //                    item.location = DistinctString(tmplocation + item.location);
                    //                }
                    #endregion
                }
            }
            #endregion
            String sqlcmd = "";
            switch (type)
            {
                case 4:
                    #region -- Case 4 OutQty --
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t
set t.OutQty = isnull(t.OutQty,0.00) + s.qty
from mdivisionpodetail t
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2 and t.mdivisionid = s.mdivisionid;";
                    }
                    else
                    {
                        sqlcmd = @"
alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t
set t.OutQty = isnull(t.OutQty,0.00) - s.qty
from mdivisionpodetail t
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2 and t.mdivisionid = s.mdivisionid;";
                    }
                    #endregion
                    break;
                case 8:
                    #region -- Case 8 LInvQty --
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

merge dbo.mdivisionpodetail as target
using #TmpSource as src
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2 and target.mdivisionid = src.mdivisionid
when matched then
update 
set target.LInvQty = isnull(target.LInvQty,0.00) + src.qty , target.blocation = src.location
when not matched then
    insert ([Poid],[Seq1],[Seq2],[MDivisionID],[LInvQty],[blocation])
    values (src.poid,src.seq1,src.seq2,src.mdivisionid,src.qty,src.location);";
                    }
                    else
                    {
                        sqlcmd = @"alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t 
set t.LInvQty = isnull(t.LInvQty,0.00) - s.qty
from mdivisionpodetail t
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2 and t.mdivisionid = s.mdivisionid;";
                    }
                    #endregion
                    break;
                case 16:
                    #region -- LObQty --
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t 
set t.LObQty = isnull(t.LObQty,0.00) + s.qty
from mdivisionpodetail t
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2 and t.mdivisionid = s.mdivisionid";
                    }
                    else
                    {
                        sqlcmd = @"
alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t 
set t.LObQty = isnull(t.LObQty,0.00) - s.qty
from mdivisionpodetail t
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2 and t.mdivisionid = s.mdivisionid";
                    }
                    #endregion
                    break;
                case 32:
                    #region -- AdjustQty --
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t 
set t.AdjustQty = isnull(t.AdjustQty,0.00) + s.qty
from mdivisionpodetail t
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2 and t.mdivisionid = s.mdivisionid";
                    }
                    else
                    {
                        sqlcmd = @"
alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)

update t 
set t.AdjustQty = isnull(t.AdjustQty,0.00) - s.qty
from mdivisionpodetail t
inner join #TmpSource s
on t.poid = s.poid and t.seq1 = s.seq1 and t.seq2=s.seq2 and t.mdivisionid = s.mdivisionid";
                    }
                    #endregion
                    break;            
            }
            return sqlcmd;
        }
        //舊
        public static string UpdateMPoDetail(int type, string Poid, string seq1, string seq2, decimal qty, bool encoded, string stocktype, string m, string location = "", bool attachLocation = true)
        {
            string sqlcmd = null, tmplocation = "";
            if (attachLocation) tmplocation = MyUtility.GetValue.Lookup(string.Format(@"select t.mtllocationid+','
from (select distinct mtllocationid from ftyinventory f inner join ftyinventory_detail fd on f.ukey = fd.ukey 
where f.mdivisionid ='{0}' and f.poid = '{1}' and f.seq1='{2}' and f.seq2='{3}' and stocktype='{4}') t for xml path('')", m, Poid, seq1, seq2, stocktype));
            switch (type)
            {
                case 2:
                    #region -- Case 2 InQty --
                    if (encoded)
                    {
                        switch (stocktype)
                        {
                            case "I":
                                sqlcmd = string.Format(@"
merge dbo.mdivisionpodetail as target
using (values('{0}','{1}','{2}','{3}','{4}','{5}')) as src (poid,seq1,seq2,qty,m,blocation) 
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2 and target.mdivisionid = src.m
when matched then
update 
set  inqty = isnull(inqty,0.00) + src.qty , blocation = src.blocation
when not matched then
    insert ([Poid],[Seq1],[Seq2],[MDivisionID],[inqty],[blocation])
    values (src.poid,src.seq1,src.seq2,src.m,src.qty,src.blocation);", Poid, seq1, seq2, qty, m, DistinctString(tmplocation + location));
                                break;

                            case "B":
                                sqlcmd = string.Format(@"
merge dbo.mdivisionpodetail as target
using (values('{0}','{1}','{2}','{3}','{4}','{5}')) as src (poid,seq1,seq2,qty,m,alocation) 
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2 and target.mdivisionid = src.m
when matched then
update 
set  inqty = isnull(inqty,0.00) + src.qty , alocation = src.alocation
when not matched then
    insert ([Poid],[Seq1],[Seq2],[MDivisionID],[inqty],[alocation])
    values (src.poid,src.seq1,src.seq2,src.m,src.qty,src.alocation);", Poid, seq1, seq2, qty, m, DistinctString(tmplocation + location));
                                break;
                        }


                    }
                    else
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set  inqty = isnull(inqty,0.00) - {3} 
where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}';"
                            , Poid, seq1, seq2, qty, m);
                    }
                    #endregion
                    break;
                case 4:
                    #region -- Case 4 OutQty --
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set  OutQty = isnull(OutQty,0.00) + {3} 
                            where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}';"
                            , Poid, seq1, seq2, qty, m);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set  OutQty = isnull(OutQty,0.00) - {3} 
                            where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}';"
                            , Poid, seq1, seq2, qty, m);
                    }
                    #endregion
                    break;
                case 8:
                    #region -- Case 8 LInvQty --
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"
merge dbo.mdivisionpodetail as target
using (values('{0}','{1}','{2}','{3}','{4}','{5}')) as src (poid,seq1,seq2,qty,m,blocation) 
on target.poid = src.poid and target.seq1=src.seq1 and target.seq2=src.seq2 and target.mdivisionid = src.m
when matched then
update 
set  LInvQty = isnull(LInvQty,0.00) - src.qty , blocation = src.blocation
when not matched then
    insert ([Poid],[Seq1],[Seq2],[MDivisionID],[LInvQty],[blocation])
    values (src.poid,src.seq1,src.seq2,src.m,src.qty,src.blocation);", Poid, seq1, seq2, qty, m, DistinctString(tmplocation + location));
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set  LInvQty = isnull(LInvQty,0.00) + {3} 
where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}';"
                            , Poid, seq1, seq2, qty, m);
                    }
                    #endregion
                    break;
                case 16:
                    #region -- LObQty --
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set  LObQty = isnull(LObQty,0.00) + {3} 
where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}';"
                            , Poid, seq1, seq2, qty, m);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set  LObQty = isnull(LObQty,0.00) - {3} 
where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}';"
                            , Poid, seq1, seq2, qty, m);
                    }
                    #endregion
                    break;
                case 32:
                    #region -- AdjustQty --
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set  AdjustQty = isnull(AdjustQty,0.00) + {3} 
where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}';"
                            , Poid, seq1, seq2, qty, m);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set AdjustQty = isnull(AdjustQty,0.00) - {3} 
where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}';"
                            , Poid, seq1, seq2, qty, m);
                    }
                    #endregion
                    break;
            }
            #region 前人遺留下來
            //            if (encoded && (type == 2 || type == 8 || type == 16) && !MyUtility.Check.Empty(stocktype))
            //            {
            //                switch (stocktype)
            //                {
            //                    case "B":
            //                        sqlcmd += string.Format(@"update mdivisionpodetail set ALocation 
            //= (Select cast(tmp.MtlLocationID as nvarchar)+',' 
            //from (select d.mtllocationid from ftyinventory_detail d inner join ftyinventory f
            //on d.ukey = f.ukey
            //where f.poid = '{0}' and f.seq1 ='{1}' and f.seq2 ='{2}' and stocktype = 'B' 
            //group by d.MtlLocationID) tmp 
            //for XML PATH(''))
            //where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{3}';", Poid, seq1, seq2,m);
            //                        break;
            //                    case "I":
            //                        sqlcmd += string.Format(@"update mdivisionpodetail set BLocation 
            //= (Select cast(tmp.MtlLocationID as nvarchar)+',' 
            //from (select d.mtllocationid from ftyinventory_detail d inner join ftyinventory f
            //on d.ukey = f.ukey
            //where f.poid = '{0}' and f.seq1 ='{1}' and f.seq2 ='{2}' and stocktype = 'I' 
            //group by d.MtlLocationID) tmp 
            //for XML PATH(''))
            //where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{3}';", Poid, seq1, seq2,m);
            //                        break;
            //                    default:
            //                        break;
            //                }

            //            }
            #endregion
            return sqlcmd;
        }
        #endregion

        #region -- UpdateFtyInventory --
        /// <summary>
        /// UpdateFtyInventory()
        /// *	更新 FtyInventory 的庫存
        /// *-----------------------------------------------------
        /// * Type	: 
        /// *	2.	更新InQty
        /// *	4.	更新OutQty
        /// *	6.	更新OutQty with Location
        /// *	8.	更新AdjustQty
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
        //新(整批)
        public static string UpdateFtyInventory_IO(int type,IList<DataRow> datas, bool encoded)
        {
            string sqlcmd = "";
            switch (type)
            {
                case 2:
                    #region 更新 inqty
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column tomdivisionid varchar(10)
alter table #TmpSource alter column topoid varchar(20)
alter table #TmpSource alter column toseq1 varchar(3)
alter table #TmpSource alter column toseq2 varchar(3)
alter table #TmpSource alter column tostocktype varchar(1)
alter table #TmpSource alter column toroll varchar(15)

merge dbo.FtyInventory as target
using #TmpSource as s
    on target.mdivisionid = s.tomdivisionid and target.poid = s.topoid and target.seq1 = s.toseq1 
	and target.seq2 = s.toseq2 and target.stocktype = s.tostocktype and target.roll = s.toroll
when matched then
    update
    set inqty = isnull(inqty,0.00) + s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select ukey from dbo.MDivisionPoDetail 
			 where mdivisionid = s.tomdivisionid and poid = s.topoid and seq1 = s.toseq1 and seq2 = s.toseq2)
			 ,s.tomdivisionid,s.topoid,s.toseq1,s.toseq2,s.toroll,s.todyelot,s.tostocktype,s.qty);

select location,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f on mdivisionid = s.tomdivisionid and poid = s.topoid 
						 and seq1 = s.toseq1 and seq2 = s.toseq2 and roll = s.toroll
merge dbo.ftyinventory_detail as t
using #tmp_L_K as s on t.ukey = s.ukey and isnull(t.mtllocationid,'') = isnull(s.location,'')
when not matched then
    insert ([ukey],[mtllocationid]) 
	values (s.ukey,isnull(s.location,''));

delete t from FtyInventory_Detail t
where  exists(select 1 from #tmp_L_K x where x.ukey=t.Ukey and x.location != t.MtlLocationID)
";
                        //↑最後一段delete寫法千萬不能用merge作,即使只有一筆資料也要跑超久
                    }
                    else
                    {
                        sqlcmd = @"
alter table #TmpSource alter column tomdivisionid varchar(10)
alter table #TmpSource alter column topoid varchar(20)
alter table #TmpSource alter column toseq1 varchar(3)
alter table #TmpSource alter column toseq2 varchar(3)
alter table #TmpSource alter column tostocktype varchar(1)
alter table #TmpSource alter column toroll varchar(15)

merge dbo.FtyInventory as target
using #TmpSource as s
    on target.mdivisionid = s.tomdivisionid and target.poid = s.topoid and target.seq1 = s.toseq1 
	and target.seq2 = s.toseq2 and target.stocktype = s.tostocktype and target.roll = s.toroll
when matched then
    update
    set inqty = isnull(inqty,0.00) - s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select ukey FROM dbo.MDivisionPoDetail 
			 where mdivisionid = s.tomdivisionid and poid = s.topoid and seq1 = s.toseq1 and seq2 = s.toseq2)
			 ,s.tomdivisionid,s.topoid,s.toseq1,s.toseq2,s.toroll,s.todyelot,s.tostocktype,s.qty);";
                    }
                    break;
                #endregion
                case 4:
                    #region 更新OutQty
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column frommdivisionid varchar(10)
alter table #TmpSource alter column frompoid varchar(20)
alter table #TmpSource alter column fromseq1 varchar(3)
alter table #TmpSource alter column fromseq2 varchar(3)
alter table #TmpSource alter column fromstocktype varchar(1)
alter table #TmpSource alter column fromroll varchar(15)

merge dbo.FtyInventory as target
using #TmpSource as s
    on target.mdivisionid = s.frommdivisionid and target.poid = s.frompoid and target.seq1 = s.fromseq1 
	and target.seq2 = s.fromseq2 and target.stocktype = s.fromstocktype and target.roll = s.fromroll
when matched then
    update
    set outqty = isnull(outqty,0.00) + s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select ukey from dbo.MDivisionPoDetail 
			 where mdivisionid = s.frommdivisionid and poid = s.frompoid and seq1 = s.fromseq1 and seq2 = s.fromseq2)
			 ,s.frommdivisionid,s.frompoid,s.fromseq1,s.fromseq2,s.fromroll,s.fromdyelot,s.fromstocktype,s.qty);";
                    }
                    else
                    {
                        sqlcmd = @"
alter table #TmpSource alter column frommdivisionid varchar(10)
alter table #TmpSource alter column frompoid varchar(20)
alter table #TmpSource alter column fromseq1 varchar(3)
alter table #TmpSource alter column fromseq2 varchar(3)
alter table #TmpSource alter column fromstocktype varchar(1)
alter table #TmpSource alter column fromroll varchar(15)

merge dbo.FtyInventory as target
using #TmpSource as s
    on target.mdivisionid = s.frommdivisionid and target.poid = s.frompoid and target.seq1 = s.fromseq1 
	and target.seq2 = s.fromseq2 and target.stocktype = s.fromstocktype and target.roll = s.fromroll
when matched then
    update
    set outqty = isnull(outqty,0.00) - s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select ukey from dbo.MDivisionPoDetail 
			 where mdivisionid = s.frommdivisionid and poid = s.frompoid and seq1 = s.fromseq1 and seq2 = s.fromseq2)
			 ,s.frommdivisionid,s.frompoid,s.fromseq1,s.fromseq2,s.fromroll,s.fromdyelot,s.fromstocktype,s.qty);";
                    }
                    
                    #endregion
                    break;
                case 6:
                    #region 更新OutQty with Location
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column tomdivisionid varchar(10)
alter table #TmpSource alter column topoid varchar(20)
alter table #TmpSource alter column toseq1 varchar(3)
alter table #TmpSource alter column toseq2 varchar(3)
alter table #TmpSource alter column tostocktype varchar(1)
alter table #TmpSource alter column toroll varchar(15)

merge dbo.FtyInventory as target
using #TmpSource as s
    on target.mdivisionid = s.tomdivisionid and target.poid = s.topoid and target.seq1 = s.toseq1 
	and target.seq2 = s.toseq2 and target.stocktype = s.tostocktype and target.roll = s.toroll
when matched then
    update
    set outqty = isnull(outqty,0.00) + s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[inqty])
    values ((select ukey from dbo.MDivisionPoDetail 
			 where mdivisionid = s.tomdivisionid and poid = s.topoid and seq1 = s.toseq1 and seq2 = s.toseq2)
			 ,s.tomdivisionid,s.topoid,s.toseq1,s.toseq2,s.toroll,s.todyelot,s.tostocktype,s.qty);

select location,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f on mdivisionid = s.tomdivisionid and poid = s.topoid 
						 and seq1 = s.toseq1 and seq2 = s.toseq2 and roll = s.toroll
merge dbo.ftyinventory_detail as t
using #tmp_L_K as s on t.ukey = s.ukey and t.mtllocationid = s.location
when not matched then
    insert ([ukey],[mtllocationid]) 
	values (s.ukey,isnull(s.location,''));

delete t from FtyInventory_Detail t
where  exists(select 1 from #tmp_L_K x where x.ukey=t.Ukey and x.location != t.MtlLocationID)
";
                        //↑最後一段delete寫法千萬不能用merge作,即使只有一筆資料也要跑超久
                    }
                    else
                    {
                        sqlcmd = @"
alter table #TmpSource alter column tomdivisionid varchar(10)
alter table #TmpSource alter column topoid varchar(20)
alter table #TmpSource alter column toseq1 varchar(3)
alter table #TmpSource alter column toseq2 varchar(3)
alter table #TmpSource alter column tostocktype varchar(1)
alter table #TmpSource alter column toroll varchar(15)

merge dbo.FtyInventory as target
using #TmpSource as s
    on target.mdivisionid = s.tomdivisionid and target.poid = s.topoid and target.seq1 = s.toseq1 
	and target.seq2 = s.toseq2 and target.stocktype = s.tostocktype and target.roll = s.toroll
when matched then
    update
    set outqty = isnull(outqty,0.00) - s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[outqty])
    values ((select ukey FROM dbo.MDivisionPoDetail 
			 where mdivisionid = s.tomdivisionid and poid = s.topoid and seq1 = s.toseq1 and seq2 = s.toseq2)
			 ,s.tomdivisionid,s.topoid,s.toseq1,s.toseq2,s.toroll,s.todyelot,s.tostocktype,s.qty);";
                    }
#endregion
                    break;
                case 8:
                    #region 更新AdjustQty
                    if (encoded)
                    {
                        sqlcmd = @"
alter table #TmpSource alter column frommdivisionid varchar(10)
alter table #TmpSource alter column frompoid varchar(20)
alter table #TmpSource alter column fromseq1 varchar(3)
alter table #TmpSource alter column fromseq2 varchar(3)
alter table #TmpSource alter column fromstocktype varchar(1)
alter table #TmpSource alter column fromroll varchar(15)

merge dbo.FtyInventory as target
using #TmpSource as s
    on target.mdivisionid = s.frommdivisionid and target.poid = s.frompoid and target.seq1 = s.fromseq1 
	and target.seq2 = s.fromseq2 and target.stocktype = s.fromstocktype and target.roll = s.fromroll
when matched then
    update
    set adjustqty = isnull(adjustqty,0.00) + s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[adjustqty])
    values ((select ukey from dbo.MDivisionPoDetail 
			 where mdivisionid = s.frommdivisionid and poid = s.frompoid and seq1 = s.fromseq1 and seq2 = s.fromseq2)
			 ,s.frommdivisionid,s.frompoid,s.fromseq1,s.fromseq2,s.fromroll,s.fromdyelot,s.fromstocktype,s.qty);";
                    }
                    else
                    {
                        sqlcmd = @"
alter table #TmpSource alter column frommdivisionid varchar(10)
alter table #TmpSource alter column frompoid varchar(20)
alter table #TmpSource alter column fromseq1 varchar(3)
alter table #TmpSource alter column fromseq2 varchar(3)
alter table #TmpSource alter column fromstocktype varchar(1)
alter table #TmpSource alter column fromroll varchar(15)

merge dbo.FtyInventory as target
using #TmpSource as s
    on target.mdivisionid = s.frommdivisionid and target.poid = s.frompoid and target.seq1 = s.fromseq1 
	and target.seq2 = s.fromseq2 and target.stocktype = s.fromstocktype and target.roll = s.fromroll
when matched then
    update
    set adjustqty = isnull(adjustqty,0.00) - s.qty
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[adjustqty])
    values ((select ukey from dbo.MDivisionPoDetail 
			 where mdivisionid = s.frommdivisionid and poid = s.frompoid and seq1 = s.fromseq1 and seq2 = s.fromseq2)
			 ,s.frommdivisionid,s.frompoid,s.fromseq1,s.fromseq2,s.fromroll,s.fromdyelot,s.fromstocktype,s.qty);";
                    }
                    #endregion
                    break;
            }
            return sqlcmd;
        }
        //舊
        public static string UpdateFtyInventory(int type, string m, string Poid, string seq1, string seq2
            , decimal qty, string roll, string dyelot, string stocktype, bool encoded, string location = null)
        {
            string sqlcmd = null;
            switch (type)
            {
                case 2:
                    #region
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"
merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.mdivisionid ='{7}' and target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set inqty = isnull(inqty,0.00) + source.field1
when not matched then
    insert ([Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty],[MDivisionID],[MDivisionPoDetailUkey])
    values ('{0}','{1}','{2}','{4}','{5}','{6}',{3},'{7}'
                ,(select ukey from dbo.MDivisionPoDetail where mdivisionid='{7}' and poid='{0}' and seq1 = '{1}' and seq2='{2}')
              );", Poid, seq1, seq2, qty, roll, dyelot, stocktype, m);
                        if (location != null)
                        {
                            string[] str_array = location.Split(',');
                            for (int i = 0; i < str_array.Length; i++)
                            {
                                if (MyUtility.Check.Empty(str_array[i])) continue;
                                sqlcmd += string.Format(@" insert into #tmp (ukey,locationid) 
values ((select ukey from dbo.ftyinventory where poid='{1}' and seq1='{2}' and seq2 ='{3}' and roll='{4}' and stocktype='{5}'),'{0}');"
                                    , str_array[i], Poid, seq1, seq2, roll, stocktype, m) + Environment.NewLine;
                            }
                            sqlcmd += string.Format(@"merge dbo.ftyinventory_detail as t
using (select * from #tmp where ukey = (select ukey from dbo.ftyinventory 
where mdivisionid ='{5}' and poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}')) as s on t.ukey = s.ukey and t.mtllocationid = s.locationid
when not matched then
    insert ([ukey],[mtllocationid]) values ((select ukey from dbo.ftyinventory where mdivisionid ='{5}' and poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}'),s.locationid)
when not matched by source and t.ukey = (select ukey from dbo.ftyinventory where mdivisionid ='{5}' and poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}')
    THEN delete;", Poid, seq1, seq2, roll, stocktype, m);
                        }
                    }
                    else
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.mdivisionid ='{7}' and target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}'
when matched then
    update
    set inqty = isnull(inqty,0.00) - source.field1
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select ukey from dbo.MDivisionPoDetail where mdivisionid = '{7}' and poid='{0}' and seq1='{1}' and seq2='{2}'),'{7}'
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype, m);
                    }
                    break;
                #endregion                
                case 4:
                    #region
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.mdivisionid ='{7}' and target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}'
when matched then
    update
    set outqty = isnull(outqty,0.00) + source.field1
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select ukey from dbo.MDivisionPoDetail where mdivisionid = '{7}' and poid='{0}' and seq1='{1}' and seq2='{2}'),'{7}'
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype, m);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.mdivisionid ='{7}' and target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set outqty = isnull(outqty,0.00) - source.field1
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[outqty])
    values ((select ukey from dbo.MDivisionPoDetail where mdivisionid = '{7}' and poid='{0}' and seq1='{1}' and seq2='{2}'),'{7}'
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype, m);
                    }
                    break;
                #endregion                
                case 6:
                    #region
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.mdivisionid ='{7}' and target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set outqty = isnull(outqty,0.00) + source.field1
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[outqty])
    values ((select ukey from dbo.MDivisionPoDetail where mdivisionid = '{7}' and poid='{0}' and seq1='{1}' and seq2='{2}'),'{7}'
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype, m);
                        if (location != null)
                        {
                            string[] str_array = location.Split(',');
                            for (int i = 0; i < str_array.Length; i++)
                            {
                                if (MyUtility.Check.Empty(str_array[i])) continue;
                                sqlcmd += string.Format(@" insert into #tmp (ukey,locationid) 
values ((select ukey from dbo.ftyinventory where mdivisionid='{6}' and poid='{1}' and seq1='{2}' and seq2 ='{3}' and roll='{4}' and stocktype='{5}'),'{0}');"
                                    , str_array[i], Poid, seq1, seq2, roll, stocktype, m) + Environment.NewLine;
                            }
                            sqlcmd += string.Format(@"merge dbo.ftyinventory_detail as t
using (select * from #tmp where ukey = (select ukey from dbo.ftyinventory 
where mdivisionid ='{6}' and poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}')) as s on t.ukey = s.ukey and t.mtllocationid = s.locationid
when not matched then
insert ([ukey],[mtllocationid]) values ((select ukey from dbo.ftyinventory where mdivisionid='{6}' and poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}'),s.locationid)
when not matched by source and t.ukey = (select ukey from dbo.ftyinventory where mdivisionid='{6}' and poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}')
THEN delete;", Poid, seq1, seq2, roll, stocktype, m);
                        }
                    }
                    else
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.mdivisionid='{7}' and target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set outqty = isnull(outqty,0.00) - source.field1
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[outqty])
    values ((select ukey from dbo.MDivisionPoDetail where mdivisionid = '{7}' and poid='{0}' and seq1='{1}' and seq2='{2}'),'{7}'
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype, m);
                    }
                    break;
                #endregion                
                case 8:
                    #region
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({6}))
    as source (field1)
    on target.mdivisionid ='{7}' and target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and target.roll='{3}' and stocktype='{5}' 
when matched then
    update
    set adjustqty = isnull(adjustqty,0.00) + source.field1
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[adjustqty])
    values ((select ukey from dbo.MDivisionPoDetail where mdivisionid = '{7}' and poid='{0}' and seq1='{1}' and seq2='{2}'),'{7}'
	,'{0}','{1}','{2}','{3}','{4}','{5}',{6});	", Poid, seq1, seq2, roll, dyelot, stocktype, qty, m);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({6}))
    as source (field1)
    on target.mdivisionid ='{7}' and target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and target.roll='{3}' and stocktype='{5}'
when matched then
    update
    set adjustqty = isnull(adjustqty,0.00) - source.field1
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[adjustqty])
    values ((select ukey from dbo.MDivisionPoDetail where mdivisionid = '{7}' and poid='{0}' and seq1='{1}' and seq2='{2}'),'{7}'
	,'{0}','{1}','{2}','{3}','{4}','{5}',{6});", Poid, seq1, seq2, roll, dyelot, stocktype, qty, m);
                    }
                    break;
                #endregion
            }
            return sqlcmd;
        }
        #endregion

        #region -- SelePoItem --
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
            string sqlcmd = string.Format(@"select  m.poid,left(m.seq1+' ',3)+m.seq2 as seq, p.Refno, dbo.getmtldesc(m.poid,m.seq1,m.seq2,2,0) as Description 
,p.ColorID,p.FinalETA,m.InQty,p.pounit,p.StockUnit
,isnull(m.OutQty, 0) as outQty
,isnull(m.AdjustQty, 0) as AdjustQty
,isnull(m.inqty, 0) - isnull(m.OutQty, 0) + isnull(m.AdjustQty, 0) as balance
,m.LInvQty
,p.fabrictype
,m.seq1
,m.seq2
,p.scirefno
from dbo.mdivisionpodetail m left join dbo.PO_Supp_Detail p on m.poid = p.id and m.seq1 = p.seq1 and m.seq2 = p.seq2
where m.mdivisionid = '{1}' and m.poid ='{0}'", poid, Sci.Env.User.Keyword);

            if (!(MyUtility.Check.Empty(filters)))
            {
                sqlcmd += string.Format(" And {0}", filters);
            }

            DBProxy.Current.Select(null, sqlcmd, out dt);

            Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(dt
                            , "Seq,refno,description,colorid,FinalETA,inqty,stockunit,outqty,adjustqty,balance,linvqty"
                            , "6,8,8,8,10,6,6,6,6,6,6", defaultseq, "Seq,Ref#,Description,Color,ETA,In Qty,Stock Unit,Out Qty,Adqty,Balance,Inventory Qty");
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
            string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}' and mdivisionid='{1}'", stocktype, Sci.Env.User.Keyword);

            Sci.Win.Tools.SelectItem2 selectlocation = new Win.Tools.SelectItem2(sqlcmd,
                            "Location ID,Description,Stock Type", "13,60,10", defaultseq);
            selectlocation.Width = 1024;

            return selectlocation;
        }
        #endregion
        #region-- GetLocation --
        public static string GetLocation(int ukey, System.Data.SqlClient.SqlConnection conn = null)
        {
            string rtn = "";
            DataRow dr;
            DataTable dt;
            if (null == conn)
            {
                MyUtility.Check.Seek(string.Format(@"select cast(tmp.MtlLocationID as nvarchar) +','
from (select f.MtlLocationID from dbo.FtyInventory_Detail f where f.Ukey = {0}) tmp
for xml path('') ", ukey), out dr);
                if (MyUtility.Check.Empty(dr)) return "";
            }
            else
            {
                DBProxy.Current.SelectByConn(conn, string.Format(@"select cast(tmp.MtlLocationID as nvarchar) +','
from (select f.MtlLocationID from dbo.FtyInventory_Detail f where f.Ukey = {0}) tmp
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

        public static IList<DataRow> autopick(DataRow materials,bool isIssue=true,string stocktype = "B")
        {
            List<DataRow> items = new List<DataRow>();
            String sqlcmd;
            DataTable dt;

            decimal request = decimal.Parse(materials["requestqty"].ToString());
            decimal accu_issue = 0m;
            if (isIssue)
            {
                sqlcmd = string.Format(@"
select (select t.mtllocationid+',' from (select MtlLocationID from dbo.FtyInventory_Detail where ukey = a.Ukey)t for xml path('')) location
,a.Ukey as FtyInventoryUkey,MDivisionID,POID,a.seq1,a.Seq2,roll,stocktype,Dyelot,inqty-OutQty+AdjustQty qty
,inqty,outqty,adjustqty,inqty-OutQty+AdjustQty balanceqty
,sum(inqty-OutQty+AdjustQty) 
over (order by a.Dyelot,(select t.mtllocationid+',' from (select MtlLocationID from dbo.FtyInventory_Detail where ukey = a.Ukey)t for xml path(''))
,a.Seq1,a.seq2,inqty-OutQty+AdjustQty desc
rows between unbounded preceding and current row) as running_total
from dbo.FtyInventory a inner join dbo.PO_Supp_Detail p on p.id = a.POID and p.seq1 = a.Seq1 and p.seq2 = a.Seq2
where poid='{1}' and Stocktype='{4}' and inqty-OutQty+AdjustQty > 0
and MDivisionID = '{0}' and p.SCIRefno = '{2}' and p.ColorID = '{3}'
order by Dyelot,location,Seq1,seq2,Qty desc", Sci.Env.User.Keyword, materials["poid"], materials["scirefno"], materials["colorid"], stocktype);
            }
            else
            {
                sqlcmd = string.Format(@"
select (select t.mtllocationid+',' from (select MtlLocationID from dbo.FtyInventory_Detail where ukey = a.Ukey)t for xml path('')) location
,a.Ukey as FtyInventoryUkey,MDivisionID,POID,a.seq1,a.Seq2,roll,stocktype,Dyelot,inqty-OutQty+AdjustQty qty
,inqty,outqty,adjustqty,inqty-OutQty+AdjustQty balanceqty
,sum(inqty-OutQty+AdjustQty) 
over (order by a.Dyelot,(select t.mtllocationid+',' from (select MtlLocationID from dbo.FtyInventory_Detail where ukey = a.Ukey)t for xml path(''))
,a.Seq1,a.seq2,inqty-OutQty+AdjustQty desc
rows between unbounded preceding and current row) as running_total
from dbo.FtyInventory a inner join dbo.PO_Supp_Detail p on p.id = a.POID and p.seq1 = a.Seq1 and p.seq2 = a.Seq2
where poid='{1}' and Stocktype='{4}' and inqty-OutQty+AdjustQty > 0
and MDivisionID = '{0}' and p.seq1 = '{2}' and p.seq2 = '{3}'
order by Dyelot,location,Seq1,seq2,Qty desc", Sci.Env.User.Keyword, materials["poid"], materials["seq1"], materials["seq2"], stocktype);
            }
            DualResult result = DBProxy.Current.Select("", sqlcmd, out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(sqlcmd, "Sql Error");
                return null;
            }
            else
            {
                foreach (DataRow dr2 in dt.Rows)
                {
                    if ((decimal)dr2["running_total"] < request)
                    {
                        items.Add(dr2);
                        accu_issue = decimal.Parse(dr2["running_total"].ToString());
                    }
                    else
                    {
                        break;
                    }

                }

                if (accu_issue < request)   // 累計發料數小於需求數時，再反向。
                {
                    decimal balance = request - accu_issue;
                    //dt.DefaultView.Sort = "Dyelot,location,Seq1,seq2,Qty asc";
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow find = items.Find(item => item["ftyinventoryukey"].ToString() == dt.Rows[i]["ftyinventoryukey"].ToString());
                        if (MyUtility.Check.Empty(find))// if overlape
                        {
                            if (balance > 0m)
                            {
                                if (balance >= (decimal)dt.Rows[i]["qty"])
                                {
                                    items.Add(dt.Rows[i]);
                                    balance -= (decimal)dt.Rows[i]["qty"];
                                }
                                else//最後裁切
                                {
                                    dt.Rows[i]["qty"] = balance;
                                    items.Add(dt.Rows[i]);
                                    balance = 0m;
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
        public static bool CheckArrivedWhseDateWithEta(DateTime Eta, DateTime ArrivedWhseDate,out String msg)
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
    }

    public class Prgs_POSuppDetailData_B
    {
        public string mdivisionid {get;set;}
        public string poid {get;set;}
        public string seq1 {get;set;}
        public string seq2 {get;set;}
        public string stocktype {get;set;}
        public decimal qty { get; set; }
        public string location { get; set; }
    }
    public class Prgs_POSuppDetailData_A
    {
        public string mdivisionid { get; set; }
        public string poid { get; set; }
        public string seq1 { get; set; }
        public string seq2 { get; set; }
        public string stocktype { get; set; }
        public decimal qty { get; set; }
        public string location { get; set; }
    }
}
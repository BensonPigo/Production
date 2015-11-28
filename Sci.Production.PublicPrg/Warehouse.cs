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
        public static string UpdateMPoDetail(int type, string Poid, string seq1, string seq2,decimal qty,bool encoded, string stocktype,string m,string location="",bool attachLocation=true)
        {
            string sqlcmd=null, tmplocation="";
            if (attachLocation) tmplocation = MyUtility.GetValue.Lookup(string.Format(@"select t.mtllocationid+','
from (select distinct mtllocationid from ftyinventory f inner join ftyinventory_detail fd on f.ukey = fd.ukey 
where f.mdivisionid ='{0}' and f.poid = '{1}' and f.seq1='{2}' and f.seq2='{3}' and stocktype='{4}') t for xml path('')",m,Poid,seq1,seq2,stocktype));
            switch (type)
            {
                case 2:
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
    values (src.poid,src.seq1,src.seq2,src.m,src.qty,src.blocation);", Poid, seq1, seq2, qty, m, DistinctString(tmplocation+location));
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
    values (src.poid,src.seq1,src.seq2,src.m,src.qty,src.alocation);", Poid, seq1, seq2, qty, m, DistinctString(tmplocation+location));
                                break;
                        }

                        
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set  inqty = isnull(inqty,0.00) - {3} 
where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}';"
                            , Poid, seq1, seq2, qty, m);
                    }
                    break;
                case 4:
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
                    break;
                case 8:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set  LInvQty = isnull(LInvQty,0.00) + {3} 
where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}' ;"
                            , Poid, seq1, seq2, qty, m);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update mdivisionpodetail set  LInvQty = isnull(LInvQty,0.00) - {3} 
where poid = '{0}' and seq1 = '{1}' and seq2='{2}' and mdivisionid = '{4}';"
                            , Poid, seq1, seq2, qty, m);
                    }
                    break;
                case 16:
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
                    break;
                case 32:
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
                    break;
            }
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
            return sqlcmd;
        }
        #endregion

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
        #region -- UpdateFtyInventory --
        public static string UpdateFtyInventory(int type,string m, string Poid, string seq1, string seq2
            , decimal qty, string roll, string dyelot, string stocktype, bool encoded, string location = null)
        {
            string sqlcmd=null;
            switch (type)
            {
                case 2:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
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
                                if(MyUtility.Check.Empty(str_array[i])) continue ;
                                sqlcmd += string.Format(@" insert into #tmp (ukey,locationid) 
values ((select ukey from dbo.ftyinventory where poid='{1}' and seq1='{2}' and seq2 ='{3}' and roll='{4}' and stocktype='{5}'),'{0}');"
                                    , str_array[i], Poid, seq1, seq2, roll, stocktype, m)+Environment.NewLine;
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

                case 4:
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
                case 6:
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
                        if (location != null)
                        {
                            string[] str_array = location.Split(',');
                            for (int i = 0; i < str_array.Length; i++)
                            {
                                if (MyUtility.Check.Empty(str_array[i])) continue;
                                sqlcmd += string.Format(@" insert into #tmp (ukey,locationid) 
values ((select ukey from dbo.ftyinventory where mdivisionid='{6}' and poid='{1}' and seq1='{2}' and seq2 ='{3}' and roll='{4}' and stocktype='{5}'),'{0}');"
                                    , str_array[i], Poid, seq1, seq2, roll, stocktype,m) + Environment.NewLine;
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
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype,m);
                    }
                    break;
                case 8:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.mdivisionid ='{7}' and target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set adjustqty = isnull(adjustqty,0.00) + source.field1
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[adjustqty])
    values ((select ukey from dbo.MDivisionPoDetail where mdivisionid = '{7}' and poid='{0}' and seq1='{1}' and seq2='{2}'),'{7}'
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype,m);
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
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select ukey from dbo.MDivisionPoDetail where mdivisionid = '{7}' and poid='{0}' and seq1='{1}' and seq2='{2}'),'{7}'
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});", Poid, seq1, seq2, qty, roll, dyelot, stocktype, m);
                    }
                    break;
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
,p.ColorID,p.ata,m.InQty,p.pounit,p.StockUnit,m.OutQty,m.AdjustQty
,m.inqty - m.OutQty + m.AdjustQty as balance
,m.LInvQty
,p.fabrictype
,m.seq1
,m.seq2
,p.scirefno
from dbo.mdivisionpodetail m left join dbo.PO_Supp_Detail p on m.poid = p.id and m.seq1 = p.seq1 and m.seq2 = p.seq2
where m.mdivisionid = '{1}' and m.poid ='{0}'", poid, Sci.Env.User.Keyword);

            if(!(MyUtility.Check.Empty(filters)))
            {
                sqlcmd += string.Format(" And {0}",filters);
            }

            DBProxy.Current.Select(null, sqlcmd, out dt);
            
            Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(dt
                            , "Seq,refno,description,colorid,eta,inqty,stockunit,outqty,adjustqty,balanceqty,linvqty"
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
           
            Sci.Win.Tools.SelectItem2 selectlocation= new Win.Tools.SelectItem2(sqlcmd,
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
        #region-- Distinct Array--
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
    }
    
}
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
        /// <param name="string tablename"></param>
        /// <returns>String Sqlcmd</returns>
        public static string UpdatePO_Supp_Detail(int type, string Poid, string seq1, string seq2,decimal qty,bool encoded, string stocktype,string tablename="Po_supp_detail")
        {
            string sqlcmd=null;
            switch (type)
            {
                case 2:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"update {4} set  inqty = isnull(inqty,0.00) + {3} 
where id = '{0}' and seq1 = '{1}' and seq2='{2}';"
                            ,Poid,seq1,seq2,qty,tablename);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update {4} set  inqty = isnull(inqty,0.00) - {3} 
where id = '{0}' and seq1 = '{1}' and seq2='{2}';"
                            , Poid, seq1, seq2, qty, tablename);
                    }
                    break;
                case 4:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"update {4} set  OutQty = isnull(OutQty,0.00) + {3} 
where id = '{0}' and seq1 = '{1}' and seq2='{2}';"
                            , Poid, seq1, seq2, qty, tablename);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update {4} set  OutQty = isnull(OutQty,0.00) - {3} 
where id = '{0}' and seq1 = '{1}' and seq2='{2}';"
                            , Poid, seq1, seq2, qty, tablename);
                    }
                    break;
                case 8:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"update {4} set  LInvQty = isnull(LInvQty,0.00) + {3} 
where id = '{0}' and seq1 = '{1}' and seq2='{2}';"
                            , Poid, seq1, seq2, qty, tablename);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update {4} set  LInvQty = isnull(LInvQty,0.00) - {3} 
where id = '{0}' and seq1 = '{1}' and seq2='{2}';"
                            , Poid, seq1, seq2, qty, tablename);
                    }
                    break;
                case 16:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"update {4} set  LObQty = isnull(LObQty,0.00) + {3} 
where id = '{0}' and seq1 = '{1}' and seq2='{2}';"
                            , Poid, seq1, seq2, qty, tablename);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update {4} set  LObQty = isnull(LObQty,0.00) - {3} 
where id = '{0}' and seq1 = '{1}' and seq2='{2}';"
                            , Poid, seq1, seq2, qty, tablename);
                    }
                    break;
                case 32:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"update {4} set  AdjustQty = isnull(AdjustQty,0.00) + {3} 
where id = '{0}' and seq1 = '{1}' and seq2='{2}';"
                            , Poid, seq1, seq2, qty, tablename);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"update {4} set AdjustQty = isnull(AdjustQty,0.00) - {3} 
where id = '{0}' and seq1 = '{1}' and seq2='{2}';"
                            , Poid, seq1, seq2, qty, tablename);
                    }
                    break;
            }
            if (encoded && (type == 2 || type == 8 || type == 16) && !MyUtility.Check.Empty(stocktype))
            {
                switch (stocktype)
                {
                    case "B":
                        sqlcmd += string.Format(@"update {3} set ALocation 
= (Select cast(tmp.MtlLocationID as nvarchar)+',' 
from (select d.mtllocationid from ftyinventory_detail d inner join ftyinventory f
on d.ukey = f.ukey
where f.poid = '{0}' and f.seq1 ='{1}' and f.seq2 ='{2}' and stocktype = 'B' 
group by d.MtlLocationID) tmp 
for XML PATH(''))
where id = '{0}' and seq1 = '{1}' and seq2='{2}' ;", Poid, seq1, seq2, tablename);
                        break;
                    case "I":
                        sqlcmd += string.Format(@"update {3} set BLocation 
= (Select cast(tmp.MtlLocationID as nvarchar)+',' 
from (select d.mtllocationid from ftyinventory_detail d inner join ftyinventory f
on d.ukey = f.ukey
where f.poid = '{0}' and f.seq1 ='{1}' and f.seq2 ='{2}' and stocktype = 'I' 
group by d.MtlLocationID) tmp 
for XML PATH(''))
where id = '{0}' and seq1 = '{1}' and seq2='{2}' ;", Poid, seq1, seq2, tablename);
                        break;
                    default:
                        break;
                }
                
            }
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
        public static string UpdateFtyInventory(int type, string Poid, string seq1, string seq2
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
    on target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set inqty = isnull(inqty,0.00) + source.field1
when not matched then
    insert ( [Category],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select category from orders where id = '{0}')
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype);
                        if (location != null)
                        {
                             string[] str_array = location.Split(',');
                            for (int i = 0; i < str_array.Length; i++)
                            {
                                if(MyUtility.Check.Empty(str_array[i])) continue ;
                                sqlcmd += string.Format(@" insert into #tmp (ukey,locationid) 
values ((select ukey from dbo.ftyinventory where poid='{1}' and seq1='{2}' and seq2 ='{3}' and roll='{4}' and stocktype='{5}'),'{0}');"
                                    , str_array[i], Poid, seq1, seq2, roll, stocktype)+Environment.NewLine;
                            }
                            sqlcmd += string.Format(@"merge dbo.ftyinventory_detail as t
using (select * from #tmp where ukey = (select ukey from dbo.ftyinventory 
where poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}')) as s on t.ukey = s.ukey and t.mtllocationid = s.locationid
when not matched then
insert ([ukey],[mtllocationid]) values ((select ukey from dbo.ftyinventory where poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}'),s.locationid)
WHEN NOT MATCHED BY SOURCE and t.ukey = (select ukey from dbo.ftyinventory where poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}')
THEN delete;", Poid, seq1, seq2, roll, stocktype);
                        }
                    }
                    else
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set inqty = isnull(inqty,0.00) - source.field1
when not matched then
    insert ( [Category],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select category from orders where id = '{0}')
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype);
                    }
                    break;

                case 4:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set outqty = isnull(outqty,0.00) + source.field1
when not matched then
    insert ( [Category],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select category from orders where id = '{0}')
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set outqty = isnull(outqty,0.00) - source.field1
when not matched then
    insert ( [Category],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[outqty])
    values ((select category from orders where id = '{0}')
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype);
                    }
                    break;
                case 6:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set outqty = isnull(outqty,0.00) + source.field1
when not matched then
    insert ( [Category],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select category from orders where id = '{0}')
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype);
                        if (location != null)
                        {
                            string[] str_array = location.Split(',');
                            for (int i = 0; i < str_array.Length; i++)
                            {
                                if (MyUtility.Check.Empty(str_array[i])) continue;
                                sqlcmd += string.Format(@" insert into #tmp (ukey,locationid) 
values ((select ukey from dbo.ftyinventory where poid='{1}' and seq1='{2}' and seq2 ='{3}' and roll='{4}' and stocktype='{5}'),'{0}');"
                                    , str_array[i], Poid, seq1, seq2, roll, stocktype) + Environment.NewLine;
                            }
                            sqlcmd += string.Format(@"merge dbo.ftyinventory_detail as t
using (select * from #tmp where ukey = (select ukey from dbo.ftyinventory 
where poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}')) as s on t.ukey = s.ukey and t.mtllocationid = s.locationid
when not matched then
insert ([ukey],[mtllocationid]) values ((select ukey from dbo.ftyinventory where poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}'),s.locationid)
WHEN NOT MATCHED BY SOURCE and t.ukey = (select ukey from dbo.ftyinventory where poid='{0}' and seq1='{1}' and seq2 ='{2}' and roll='{3}' and stocktype='{4}')
THEN delete;", Poid, seq1, seq2, roll, stocktype);
                        }
                    }
                    else
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set outqty = isnull(outqty,0.00) - source.field1
when not matched then
    insert ( [Category],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[outqty])
    values ((select category from orders where id = '{0}')
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype);
                    }
                    break;
                case 8:
                    if (encoded)
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set adjustqty = isnull(adjustqty,0.00) + source.field1
when not matched then
    insert ( [Category],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[adjustqty])
    values ((select category from orders where id = '{0}')
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});	", Poid, seq1, seq2, qty, roll, dyelot, stocktype);
                    }
                    else
                    {
                        sqlcmd = string.Format(@"merge dbo.FtyInventory as target
using (values ({3}))
    as source (field1)
    on target.poid ='{0}' and target.seq1 = '{1}' and target.seq2 ='{2}' and stocktype='{6}' and target.roll='{4}' 
when matched then
    update
    set outqty = isnull(outqty,0.00) - source.field1
when not matched then
    insert ( [Category],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[StockType],[InQty])
    values ((select category from orders where id = '{0}')
	,'{0}','{1}','{2}','{4}','{5}','{6}',{3});", Poid, seq1, seq2, qty, roll, dyelot, stocktype);
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
            string sqlcmd = string.Format(@"select p.id poid,left(p.seq1+' ',3)+p.seq2 as seq, p.Refno, dbo.getmtldesc(p.id,p.seq1,p.seq2,2,0) as Description 
,p.ColorID,p.ata,p.InQty,p.pounit,p.StockUnit,p.OutQty,p.AdjustQty
,p.inqty - p.OutQty + p.AdjustQty as balance
,p.LInvQty
,p.fabrictype
,p.seq1
,p.seq2
,p.scirefno
from dbo.PO_Supp_Detail p
where id ='{0}'", poid);

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

        #region -- SeleShopfloorItem --
        /// <summary>
        /// 右鍵開窗選取採購項
        /// </summary>
        /// <param name="poid"></param>
        /// <param name="defaultseq"></param>
        /// <param name="filters"></param>
        /// <returns>Sci.Win.Tools.SelectItem</returns>
        public static Sci.Win.Tools.SelectItem SeleShopfloorItem(string poid, string defaultseq, string filters = null)
        {
            DataTable dt;
            string sqlcmd = string.Format(@"select p.id poid,left(p.seq1+' ',3)+p.seq2 as seq, p.Refno, dbo.getmtldesc(p.id,.p.seq1,p.seq2,2,0) as Description 
,p.ColorID,p.InQty,p.pounit,p.StockUnit,p.OutQty,p.AdjustQty
,p.inqty - p.OutQty + p.AdjustQty as balance
,p.fabrictype
,p.seq1
,p.seq2
from dbo.PO_Artwork p
where id ='{0}'", poid);
            if (!(MyUtility.Check.Empty(filters)))
            {
                sqlcmd += string.Format(" And {0}", filters);
            }
            DBProxy.Current.Select(null, sqlcmd, out dt);
            
            Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(dt
                            , "Seq,refno,description,colorid,inqty,stockunit,outqty,adjustqty,balanceqty"
                            , "6,15,8,8,6,6,6,6,6", defaultseq, "Seq,Ref#,Description,Color,In Qty,Stock Unit,Out Qty,Adqty,Balance");
            selepoitem.Width = 1024;

            return selepoitem;
        }
        #endregion 

        #region SelectLocation
        /// <summary>
        /// 右鍵開窗選取物料儲位
        /// </summary>
        /// <param name="stocktype"></param>
        /// <param name="defaultseq"></param>
        /// <returns>Sci.Win.Tools.SelectItem2</returns>
        public static Sci.Win.Tools.SelectItem2 SelectLocation(string stocktype, string defaultseq = "")
        {
            string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}'", stocktype);
           
            Sci.Win.Tools.SelectItem2 selectlocation= new Win.Tools.SelectItem2(sqlcmd,
                            "Location ID,Description,Stock Type", "13,60,10", defaultseq);
            selectlocation.Width = 1024;

            return selectlocation;
        }
        #endregion 

        #region GetLocation
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
    }
    
}
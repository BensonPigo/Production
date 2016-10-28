using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Win;
using Sci.Win.Tools;
using Ict;
using Ict.Data;
using Sci.Production.Class;
using Sci.Production.PublicPrg;
using System.Collections;
using System.Transactions;

namespace Sci.Production.Thread
{
    public partial class P01_CopyTo : Sci.Win.Subs.Base
    {
        private DataRow master;
        public P01_CopyTo(DataRow master)
        {
            InitializeComponent();
            this.master = master;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string season = txtseason1.Text;
            string styleSql = string.Format("Select Ukey from Style where id='{0}' and Seasonid = '{1}' and Brandid = '{2}'", master["id"].ToString(), season, master["Brandid"].ToString());
            string ukey_byNewSeason = MyUtility.GetValue.Lookup(styleSql);

            DataTable threadcolorcomb;
            //同ID,品牌會有一筆以上的Seasonid,找出ukey
            if (MyUtility.Check.Empty(ukey_byNewSeason))
            {
                MyUtility.Msg.WarningBox("Season dose not found.");
                return;
            }
            //如果有舊資料是否刪除
            string threadColorCombSql = string.Format("Select id from ThreadColorComb where StyleUkey = '{0}'", ukey_byNewSeason);
            if (MyUtility.Check.Seek(threadColorCombSql))
            {
                DialogResult diaRes = MyUtility.Msg.QuestionBox("Are you sure delete old data and copy data to this season?");
                if (diaRes == DialogResult.No) return; //不刪除則中斷
            }

            #region 準備 ThreadColorComb 資料
            string sql = "";
            List<string> threadColorCombList = new List<string>();
            List<string> threadColorCombListid = new List<string>();
            List<string> gridList = new List<string>();
            StringBuilder del3table = new StringBuilder();
            //準備ThreadColorComb新增(新season),刪除原本
            threadColorCombSql =
                string.Format(@"Select * 
                                from ThreadColorComb 
                                where StyleUkey = '{0}' 
                                order by StyleUkey", master["Ukey"].ToString());
            if (DBProxy.Current.Select(null, threadColorCombSql, out threadcolorcomb))
            {
                foreach (DataRow dr in threadcolorcomb.Rows)
                {
                    //準備:新增threadcolorcomb依據原本的資料,ukey_season為複製目標,並取得新增後ID
                    sql = string.Format(@"Insert ThreadColorComb
                            (ThreadCombid,Machinetypeid,StyleUkey,Length) 
                            values('{0}','{1}','{2}',{3});select @@IDENTITY as ii",
                            dr["ThreadCombid"].ToString(), dr["Machinetypeid"].ToString(), ukey_byNewSeason, dr["Length"].ToString());
                    threadColorCombList.Add(sql);
                    threadColorCombListid.Add(dr["id"].ToString());
                    //準備:刪除原本threadcolorcomb的資料依據原本ID
                    del3table.Append(String.Format("Delete from threadcolorcomb where id= '{0}'", dr["id"]));
                }
            }
            #endregion

            #region Copy 資料
            DataTable dt_newid;
            DualResult upResult;
            string newid, oid;
            StringBuilder insertSql = new StringBuilder();

            DataTable Tdeatil;



            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    for (int i = 0; i < threadColorCombList.Count; i++)//逐筆新增ThreadColorComb後刪除
                    {
                        if (!(upResult = DBProxy.Current.Select(null, threadColorCombList[i], out dt_newid)))//新增並取得id
                        {
                            ShowErr(threadColorCombList[i], upResult);
                            return;
                        }
                        else
                        {
                            newid = dt_newid.Rows[0]["ii"].ToString(); //新Identity
                            oid = threadColorCombListid[i].ToString();//原ID

                            string sqlTdetail = string.Format(@"select distinct td.ThreadLocationID
                                                                from threadcolorcomb t inner join ThreadColorComb_Detail td on td.id = t.id 
                                                                where t.id='{0}'", oid);
                            if (!(upResult = DBProxy.Current.Select(null, sqlTdetail, out Tdeatil)))
                            {
                                ShowErr(sqlTdetail, upResult);
                                return;
                            }

                            DataTable tbArticle;
                            DualResult ArticleResult;
                            if (Tdeatil.Rows.Count!=0)
                            {

                                string sqlArticle = string.Format(@"select Article from ThreadColorComb_Detail 
                                                       where id='{0}' and ThreadLocationID='{1}' order by Ukey",
                                                    oid, Tdeatil.Rows[0]["ThreadLocationID"].ToString());
                                if (!(ArticleResult = DBProxy.Current.Select(null, sqlArticle, out tbArticle)))//以原ID取得Article(多筆)
                                {
                                    ShowErr(sqlArticle, ArticleResult);
                                    return;
                                }
                                foreach (DataRow drArt in tbArticle.Rows)
                                {
                                    //新增時存入新ID,資料以原本ID帶入
                                    //準備:新增ThreadColorComb_Detail
                                    insertSql.Append(string.Format(@"Insert Into ThreadColorComb_Detail
                                    (id,Machinetypeid,ThreadCombid,Refno,Article,ThreadColorid,SEQ,ThreadLocationID) 
                                    Select {0},Machinetypeid,ThreadCombid,Refno,Article,ThreadColorid,SEQ,ThreadLocationID 
                                    From ThreadColorComb_Detail Where id='{1}' and Article='{2}';",
                                    newid, oid, drArt["Article"]));
                                }
                            }
                            //準備:新增ThreadColorComb_operation
                                insertSql.Append(string.Format(@"Insert Into ThreadColorComb_operation
                                    (id,Operationid) 
                                    Select {0},Operationid 
                                    from ThreadColorComb_Operation where id='{1}';",
                                    newid, oid));                            

                            //準備:此筆原ID有的Article準備完,準備刪除此筆ID ThreadColorComb_Detail, 和ThreadColorComb_operation
                            del3table.Append(string.Format("Delete from ThreadColorComb_Detail where id={0}",oid));
                            del3table.Append(string.Format("Delete from ThreadColorComb_operation where id={0}",oid));

                        }
                    }
                    //執行新增
                    if (!(upResult = DBProxy.Current.Execute(null, insertSql.ToString())))
                    {
                        MyUtility.Msg.WarningBox(string.Format("Season <{0}> exists, can't copy!!!", txtseason1.Text));
                        return;
                    }
                    //執行刪除
                    if (!MyUtility.Check.Empty(del3table.ToString()))
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, del3table.ToString())))
                        {
                            ShowErr(del3table.ToString(), upResult);
                            return;
                        }
                    }

                    _transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            MyUtility.Msg.InfoBox("Copy finished");
            this.Close();

            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

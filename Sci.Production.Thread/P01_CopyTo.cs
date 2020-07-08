using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using System.Transactions;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P01_CopyTo
    /// </summary>
    public partial class P01_CopyTo : Sci.Win.Subs.Base
    {
        private DataRow master;

        /// <summary>
        /// P01_CopyTo
        /// </summary>
        /// <param name="master">master</param>
        public P01_CopyTo(DataRow master)
        {
            this.InitializeComponent();
            this.master = master;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string season = this.txtseason.Text;
            if (season == this.master["seasonid"].ToString())
            {
                MyUtility.Msg.WarningBox("Season can not same.");
                return;
            }

            // 同ID,品牌會有一筆以上的Seasonid,找出ukey
            string styleSql = string.Format("Select Ukey from Style WITH (NOLOCK) where id='{0}' and Seasonid = '{1}' and Brandid = '{2}'", this.master["id"].ToString(), season, this.master["Brandid"].ToString());
            string ukey_byNewSeason = MyUtility.GetValue.Lookup(styleSql);
            if (MyUtility.Check.Empty(ukey_byNewSeason))
            {
                MyUtility.Msg.WarningBox("Season dose not found.");
                return;
            }

            DataTable threadcolorcomb;
            StringBuilder del3table = new StringBuilder();

            // 如果複製目的有舊資料是否刪除
            string threadColorCombSql = string.Format("Select id from ThreadColorComb WITH (NOLOCK) where StyleUkey = '{0}'", ukey_byNewSeason);
            if (MyUtility.Check.Seek(threadColorCombSql))
            {
                DialogResult diaRes = MyUtility.Msg.QuestionBox("Are you sure delete old data and copy data to this season?");
                if (diaRes == DialogResult.No)
                {
                    return; // 不刪除則中斷
                }

                #region 刪除字串
                string id_old = MyUtility.GetValue.Lookup(threadColorCombSql);

                // 準備:刪除threadcolorcomb資料
                del3table.Append(string.Format(@"Delete from threadcolorcomb WITH (NOLOCK) where StyleUkey = '{0}' ", ukey_byNewSeason));
                ////準備:刪除 ThreadColorComb_Detail, 和ThreadColorComb_operation
                del3table.Append(string.Format(@"Delete from ThreadColorComb_Detail WITH (NOLOCK) where id='{0}' ", id_old));
                del3table.Append(string.Format(@"Delete from ThreadColorComb_operation WITH (NOLOCK) where id='{0}' ", id_old));
                #endregion
            }

            #region 準備 ThreadColorComb 資料
            string sql = string.Empty;
            List<string> threadColorCombList = new List<string>();
            List<string> threadColorCombListid = new List<string>();
            List<string> gridList = new List<string>();

            // 準備ThreadColorComb新增(新season)
            threadColorCombSql = string.Format(
                @"
Select * 
from ThreadColorComb WITH (NOLOCK)
where StyleUkey = '{0}' 
order by StyleUkey",
                this.master["Ukey"].ToString());

            if (DBProxy.Current.Select(null, threadColorCombSql, out threadcolorcomb))
            {
                foreach (DataRow dr in threadcolorcomb.Rows)
                {
                    // 準備:新增threadcolorcomb依據原本的資料,ukey_season為複製目標,並取得新增後ID
                    sql = string.Format(
                        @"Insert ThreadColorComb
                            (ThreadCombid,Machinetypeid,StyleUkey,Length) 
                            values('{0}','{1}','{2}',{3});select @@IDENTITY as ii",
                        dr["ThreadCombid"].ToString(),
                        dr["Machinetypeid"].ToString(),
                        ukey_byNewSeason,
                        dr["Length"].ToString());

                    threadColorCombList.Add(sql);
                    threadColorCombListid.Add(dr["id"].ToString());
                }
            }
            #endregion

            #region Copy 資料
            DataTable dt_newid;
            DualResult upResult;
            string newid, oid;
            StringBuilder insertSql = new StringBuilder();
            DataTable tdeatil;

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    // 執行刪除
                    if (!MyUtility.Check.Empty(del3table.ToString()))
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, del3table.ToString())))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(del3table.ToString(), upResult);
                            return;
                        }
                    }

                    // 逐筆新增ThreadColorComb後刪除
                    for (int i = 0; i < threadColorCombList.Count; i++)
                    {
                        // 新增並取得id
                        if (!(upResult = DBProxy.Current.Select(null, threadColorCombList[i], out dt_newid)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(threadColorCombList[i], upResult);
                            return;
                        }
                        else
                        {
                            newid = dt_newid.Rows[0]["ii"].ToString(); // 新Identity
                            oid = threadColorCombListid[i].ToString(); // 原ID

                            string sqlTdetail = string.Format(
                                @"
select distinct td.ThreadLocationID
from threadcolorcomb t WITH (NOLOCK) 
inner join ThreadColorComb_Detail td WITH (NOLOCK) on td.id = t.id 
where t.id='{0}'",
                                oid);

                            if (!(upResult = DBProxy.Current.Select(null, sqlTdetail, out tdeatil)))
                            {
                                transactionscope.Dispose();
                                this.ShowErr(sqlTdetail, upResult);
                                return;
                            }

                            DataTable tbArticle;
                            DualResult articleResult;
                            if (tdeatil.Rows.Count != 0)
                            {
                                string sqlArticle = string.Format(
                                    @"
select Article 
from ThreadColorComb_Detail WITH (NOLOCK) 
where id='{0}' and ThreadLocationID='{1}' 
order by Ukey",
                                    oid,
                                    tdeatil.Rows[0]["ThreadLocationID"].ToString());

                                // 以原ID取得Article(多筆)
                                if (!(articleResult = DBProxy.Current.Select(null, sqlArticle, out tbArticle)))
                                {
                                    transactionscope.Dispose();
                                    this.ShowErr(sqlArticle, articleResult);
                                    return;
                                }

                                foreach (DataRow drArt in tbArticle.Rows)
                                {
                                    // 新增時存入新ID,資料以原本ID帶入
                                    // 準備:新增ThreadColorComb_Detail
                                    insertSql.Append(string.Format(
                                        @"Insert Into ThreadColorComb_Detail
                                    (id,Machinetypeid,ThreadCombid,Refno,Article,ThreadColorid,SEQ,ThreadLocationID) 
                                    Select {0},Machinetypeid,ThreadCombid,Refno,Article,ThreadColorid,SEQ,ThreadLocationID 
                                    From ThreadColorComb_Detail Where id='{1}' and Article='{2}';",
                                        newid,
                                        oid,
                                        drArt["Article"]));
                                }
                            }

                            // 準備:新增ThreadColorComb_operation
                            insertSql.Append(string.Format(
                                    @"Insert Into ThreadColorComb_operation
                                    (id,Operationid) 
                                    Select {0},Operationid 
                                    from ThreadColorComb_Operation where id='{1}';",
                                    newid,
                                    oid));
                        }
                    }

                    // 執行新增
                    if (!(upResult = DBProxy.Current.Execute(null, insertSql.ToString())))
                    {
                        transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(string.Format("Season <{0}> exists, can't copy!!!", this.txtseason.Text));
                        return;
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
            MyUtility.Msg.InfoBox("Copy finished");
            this.Close();

            #endregion
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

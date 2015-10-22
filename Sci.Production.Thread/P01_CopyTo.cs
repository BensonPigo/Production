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
            string styleSql = string.Format("Select Ukey from Style where id='{0}' and Seasonid = '{1}' and Brandid = '{2}'",master["id"].ToString(),season,master["Brandid"].ToString());
            string ukey = MyUtility.GetValue.Lookup(styleSql);
            string delSql = "";
            DataTable threadcolorcomb, threadcolorcomb_Detail, threadcolorcomb_operation;
            if (MyUtility.Check.Empty(ukey))
            {
                MyUtility.Msg.WarningBox("Season dose not found.");
                return;
            }
            string threadColorCombSql = string.Format("Select id from ThreadColorComb where StyleUkey = '{0}'", ukey);
            if (MyUtility.Check.Seek(threadColorCombSql))
            {
                DialogResult diaRes = MyUtility.Msg.QuestionBox("Are you sure delete old data and copy data to this season?");
                if (diaRes == DialogResult.No) //確認如果有舊資料是否刪除
                {
                    return;
                }
                else
                {   
                    if (DBProxy.Current.Select(null, threadColorCombSql, out threadcolorcomb))
                    {
                        delSql = delSql+String.Format("Delete from threadcolorcomb where Styleukey = '{0}';",ukey);
                        foreach (DataRow dr in threadcolorcomb.Rows)
                        {
                            delSql = delSql + String.Format("Delete from ThreadColorComb_Detail where id='{0}';Delete from ThreadColorComb_operation where id='{0}';", dr["id"].ToString());
                        }
                    }
                }
            }
            #region Copy 資料
            string sql="";
            List<string> threadColorCombList = new List<string>();
            List<string> gridList = new List<string>();
            string insertSql = "";
            threadColorCombSql = string.Format("Select * from ThreadColorComb where StyleUkey = '{0}'", master["Ukey"].ToString());
            if(DBProxy.Current.Select(null,threadColorCombSql,out threadcolorcomb))
            {
                foreach(DataRow dr in threadcolorcomb.Rows)
                {
                    sql = string.Format(@"Insert ThreadColorComb
                            (ThreadCombid,Machinetypeid,StyleUkey,Length) 
                            values('{0}','{1}','{2}',{3});select @@IDENTITY as ii",
                            dr["ThreadCombid"].ToString(),dr["Machinetypeid"].ToString(),ukey,dr["Length"].ToString());
                    threadColorCombList.Add(sql);
                }
            }
            //相同Article 才複製
            string artSql = string.Format("select a.article from style_article a,style_article b  where  a.article = b.article and a.styleukey = '{0}' and b.styleukey='{1}'", master["Ukey"].ToString(), ukey);
            DataTable artdt;
            string article = "";
            if (DBProxy.Current.Select(null, artSql, out artdt))
            {
                int i = 0;
                foreach (DataRow dr in artdt.Rows)
                {
                    if(i == 0) 
                    {
                        article = " and (";
                    }
                    else
                    {
                        article = article +" or";
                    }
                    article = article + string.Format(" article = '{0}'",dr["article"]);
                    i++;
                    if (i == artdt.Rows.Count) article = article + ")";
                }
            }
            DataTable dt;
            DualResult upResult;
            string id;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if(!MyUtility.Check.Empty(delSql))
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, delSql)))
                        {
                            ShowErr(delSql, upResult);
                            return;
                        }
                    }
                    for (int i = 0; i < threadColorCombList.Count; i++)
                    {
                        if (!(upResult = DBProxy.Current.Select(null, threadColorCombList[i], out dt)))
                        {
                            ShowErr(threadColorCombList[i], upResult);
                            return;
                        }
                        else
                        {
                            id = dt.Rows[0]["ii"].ToString(); //取出Identity
                            foreach(DataRow dr in threadcolorcomb.Rows)
                            {
                                if(!MyUtility.Check.Empty(article)) //若沒有相同的Article 就不新增Detail
                                {
                                   insertSql = insertSql + string.Format(@"Insert Into ThreadColorComb_Detail
                                    (id,Machinetypeid,ThreadCombid,Refno,Article,ThreadColorid,SEQ,ThreadLocationID) 
                                    Select {0},Machinetypeid,ThreadCombid,Refno,Article,ThreadColorid,SEQ,ThreadLocationID From         
                                    ThreadColorComb_Detail Where id='{1}'" + article + ";", id, dr["ID"].ToString());
                                }
                                insertSql = insertSql + string.Format(@"Insert Into ThreadColorComb_operation(id,Operationid) 
                                Select {0},Operationid from ThreadColorComb_Operation where id='{1}';", id, dr["ID"].ToString());
                            }
                        }
                    }
                    if (!(upResult = DBProxy.Current.Execute(null, insertSql)))
                    {
                        ShowErr(insertSql, upResult);
                        return;
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

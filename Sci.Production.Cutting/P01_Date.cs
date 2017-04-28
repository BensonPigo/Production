﻿using Ict;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Cutting
{
    public partial class P01_Date : Sci.Win.Subs.Base
    {

        public bool cancel = false;
        public P01_Date()
        {
            InitializeComponent();
            dateSewingInLineDateBefore.Value = DateTime.Now.AddDays(45);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sewdate;
            sewdate = dateSewingInLineDateBefore.Text;
            DualResult dresult;
            #region 先刪除不在SewingSchedule 內的Cutting 資料
            string sqlcmd = string.Format(@"Delete Cutting from Cutting join 
            (Select a.id from Cutting a where a.mDivisionid = '{1}' and a.Finished = 0 and a.id not in 
            (Select distinct c.cuttingsp from orders c, (SELECT orderid
            FROM Sewingschedule b 
            WHERE Inline <= '{0}' And offline is not null and offline !=''
            AND b.mDivisionid = '{1}' group by b.orderid) d where c.id = d.orderid and c.MDivisionID = '{1}')) f
            on cutting.id = f.ID", sewdate, Sci.Env.User.Keyword);

            DBProxy.Current.DefaultTimeout =300;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(dresult = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlcmd, dresult);
                        return;
                    }

                    _transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            #endregion
            #region 找出需新增或update 的Cutting
            DataTable cuttingtb;
            string updsql = "";
//            sqlcmd = string.Format(@"Select ord.cuttingsp,min(ord.sewinline) as inline ,max(ord.sewoffline) as offlinea 
//            from orders ord,
//            (Select distinct c.cuttingsp from orders c, 
//                (SELECT orderid FROM Sewingschedule b 
//                WHERE Inline <= '{0}' And offline is not null and offline !=''
//                AND b.mDivisionid = '{1}' group by b.orderid) d 
//            where c.id = d.orderid and c.IsForecast = 0 and c.LocalOrder = 0 ) cut 
//            where ord.cuttingsp = cut.CuttingSP and ord.mDivisionid = '{1}'
//            group by ord.CuttingSp order by ord.CuttingSP", sewdate, Sci.Env.User.Keyword);
            sqlcmd = string.Format(@"Select ord.cuttingsp,min(ord.sewinline) as inline ,max(ord.sewoffline) as offlinea 
            from orders ord WITH (NOLOCK) ,
            (Select * from (Select distinct c.cuttingsp from orders c WITH (NOLOCK) , 
                (SELECT orderid FROM Sewingschedule b WITH (NOLOCK) 
                WHERE Inline <= '{0}' And offline is not null and offline !=''
               AND b.mDivisionid = '{1}' group by b.orderid) d 
            where c.id = d.orderid and c.IsForecast = 0 and c.LocalOrder = 0 ) e Where e.cuttingsp is not null 
			and e.cuttingsp not in (Select id from cutting WITH (NOLOCK) )) cut
            where ord.cuttingsp = cut.CuttingSP and ord.mDivisionid = '{1}'
          group by ord.CuttingSp order by ord.CuttingSP", sewdate, Sci.Env.User.Keyword);
            dresult = DBProxy.Current.Select("Production", sqlcmd, out cuttingtb);
            string sewin, sewof;
            foreach (DataRow dr in cuttingtb.Rows)
            {
                if (dr["inline"] == DBNull.Value) sewin = "";
                else sewin = Convert.ToDateTime(dr["inline"]).ToShortDateString();
                if (dr["offlinea"] == DBNull.Value) sewof = "";
                else sewof = Convert.ToDateTime(dr["offlinea"]).ToShortDateString();


                updsql = updsql + string.Format("insert into cutting(ID,sewInline,sewoffline,mDivisionid,AddName,AddDate) Values('{0}','{1}','{2}','{3}','{4}',GetDate()); ", dr["cuttingsp"], sewin, sewof, Sci.Env.User.Keyword, Sci.Env.User.UserID);
            }
            sqlcmd = string.Format(@"Select ord.cuttingsp,min(ord.sewinline) as inline ,max(ord.sewoffline) as offlinea 
            from orders ord WITH (NOLOCK) ,
            (Select * from (Select distinct c.cuttingsp from orders c WITH (NOLOCK) , 
                (SELECT orderid FROM Sewingschedule b WITH (NOLOCK) 
                WHERE Inline <= '{0}' And offline is not null and offline !=''
               AND b.mDivisionid = '{1}' group by b.orderid) d 
            where c.id = d.orderid and c.IsForecast = 0 and c.LocalOrder = 0 ) e Where e.cuttingsp is not null 
			and e.cuttingsp in (Select id from cutting WITH (NOLOCK) )) cut
            where ord.cuttingsp = cut.CuttingSP and ord.mDivisionid = '{1}'
          group by ord.CuttingSp order by ord.CuttingSP", sewdate, Sci.Env.User.Keyword);
            dresult = DBProxy.Current.Select("Production", sqlcmd, out cuttingtb);
            foreach (DataRow dr in cuttingtb.Rows)
            {
                if (dr["inline"] == DBNull.Value) sewin = "";
                else sewin = Convert.ToDateTime(dr["inline"]).ToShortDateString();
                if (dr["offlinea"] == DBNull.Value) sewof = "";
                else sewof = Convert.ToDateTime(dr["offlinea"]).ToShortDateString();

                updsql = updsql + string.Format("update cutting set SewInLine ='{0}',sewoffline = '{1}' where id = '{2}'; ", sewin, sewof, dr["cuttingsp"]);
                

            }
            TransactionScope _transactionscope2 = new TransactionScope();
            using (_transactionscope2)
            {
                try
                {
                    if (!(dresult = DBProxy.Current.Execute(null, updsql)))
                    {
                        _transactionscope2.Dispose();
                        ShowErr(updsql, dresult);
                        return;
                    }
                    _transactionscope2.Complete();
                }
                catch (Exception ex)
                {
                    _transactionscope2.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope2.Dispose();
            _transactionscope2 = null;
            #endregion
            DBProxy.Current.DefaultTimeout = 0;
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
            
            this.Dispose();
        }
        

    }
}

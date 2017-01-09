using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Transactions;


namespace Sci.Production.PPIC
{
    public partial class P07 : Sci.Win.Tems.QueryForm
    {
        string useAPS;
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            string sqlCommand = "select UseAPS from factory where ID = '" + Sci.Env.User.Factory + "'";
            useAPS = MyUtility.GetValue.Lookup(sqlCommand, null);
            if (useAPS.ToUpper() == "FALSE")
            {
                MyUtility.Msg.WarningBox("Not yet use the APS, so can't use this function!!");
                button1.Enabled = false;
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (useAPS.ToUpper() == "FALSE")
            {
                this.Close();
            }
        }

        //Download
        private void button1_Click(object sender, EventArgs e)
        {
            DataRow dr;
            MyUtility.Check.Seek(string.Format("select SQLServerName,APSDatabaseName from MDivision where ID = '{0}'", Sci.Env.User.Keyword), out dr);
            if (MyUtility.Check.Empty(dr["SQLServerName"]) || MyUtility.Check.Empty(dr["APSDatabaseName"]))
            {
                MyUtility.Msg.WarningBox("Still not yet set APS Server data, Please contact Taipei MIS. Thank you.");
                return;
            }

            this.ShowWaitMessage("Data Downloading....");

            string sqlCmd = string.Format("exec dbo.usp_APSDataDownLoad '{0}','{1}','{2}','{3}'", MyUtility.Convert.GetString(dr["SQLServerName"]), MyUtility.Convert.GetString(dr["APSDatabaseName"]), Sci.Env.User.Factory, Sci.Env.User.UserID);
            DBProxy.Current.DefaultTimeout = 600;
            DualResult Result = DBProxy.Current.Execute(null, sqlCmd);
            DBProxy.Current.DefaultTimeout = 60;
            if (!Result)
            {
                MyUtility.Msg.WaitClear();
                ShowErr(sqlCmd, Result);
                return;
            }

            setcuttingdate();

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Download successful !!");
        }

        //Close
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setcuttingdate()
        {
            string sewdate = DateTime.Now.AddDays(45).ToShortDateString();
            DualResult dresult;

            #region 先刪除不在SewingSchedule 內的Cutting 資料
            string sqlcmd = string.Format(@"Delete Cutting from Cutting join 
            (Select a.id from Cutting a where a.FactoryID = '{1}' and a.Finished = 0 and a.id not in 
            (Select distinct c.cuttingsp from orders c, (SELECT orderid
            FROM Sewingschedule b 
            WHERE Inline <= '{0}' And offline is not null and offline !=''
            AND b.FactoryID = '{1}' group by b.orderid) d where c.id = d.orderid and c.FactoryID = '{1}')) f
            on cutting.id = f.ID", sewdate, Sci.Env.User.Factory);

            DBProxy.Current.DefaultTimeout = 300;
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
            sqlcmd = string.Format(@"Select ord.cuttingsp,min(ord.sewinline) as inline ,max(ord.sewoffline) as offlinea 
            from orders ord,
            (Select * from (Select distinct c.cuttingsp from orders c, 
                (SELECT orderid FROM Sewingschedule b 
                WHERE Inline <= '{0}' And offline is not null and offline !=''
               AND b.FactoryID = '{1}' group by b.orderid) d 
            where c.id = d.orderid and c.IsForecast = 0 and c.LocalOrder = 0 ) e Where e.cuttingsp is not null 
			and e.cuttingsp not in (Select id from cutting)) cut
            where ord.cuttingsp = cut.CuttingSP and ord.FactoryID = '{1}'
          group by ord.CuttingSp order by ord.CuttingSP", sewdate, Sci.Env.User.Factory);
            dresult = DBProxy.Current.Select("Production", sqlcmd, out cuttingtb);
            string sewin, sewof;
            foreach (DataRow dr in cuttingtb.Rows)
            {
                if (dr["inline"] == DBNull.Value) sewin = "";
                else sewin = Convert.ToDateTime(dr["inline"]).ToShortDateString();
                if (dr["offlinea"] == DBNull.Value) sewof = "";
                else sewof = Convert.ToDateTime(dr["offlinea"]).ToShortDateString();

                updsql = updsql + string.Format("insert into cutting(ID,sewInline,sewoffline,mDivisionid,FactoryID,AddName,AddDate) Values('{0}','{1}','{2}','{3}','{4}','{5}', GetDate()); ", dr["cuttingsp"], sewin, sewof, Sci.Env.User.Keyword, Sci.Env.User.Factory, Sci.Env.User.UserID);
            }
            sqlcmd = string.Format(@"Select ord.cuttingsp,min(ord.sewinline) as inline ,max(ord.sewoffline) as offlinea 
            from orders ord,
            (Select * from (Select distinct c.cuttingsp from orders c, 
                (SELECT orderid FROM Sewingschedule b 
                WHERE Inline <= '{0}' And offline is not null and offline !=''
               AND b.FactoryID = '{1}' group by b.orderid) d 
            where c.id = d.orderid and c.IsForecast = 0 and c.LocalOrder = 0 ) e Where e.cuttingsp is not null 
			and e.cuttingsp in (Select id from cutting)) cut
            where ord.cuttingsp = cut.CuttingSP and ord.FactoryID = '{1}'
          group by ord.CuttingSp order by ord.CuttingSP", sewdate, Sci.Env.User.Factory);
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

        }


    }
}

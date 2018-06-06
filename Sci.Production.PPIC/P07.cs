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
    /// <summary>
    /// P07
    /// </summary>
    public partial class P07 : Sci.Win.Tems.QueryForm
    {
        private string useAPS;

        /// <summary>
        /// P07
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            string sqlCommand = "select UseAPS from factory WITH (NOLOCK) where ID = '" + Sci.Env.User.Factory + "'";
            this.useAPS = MyUtility.GetValue.Lookup(sqlCommand, null);
            if (this.useAPS.ToUpper() == "FALSE")
            {
                MyUtility.Msg.WarningBox("Not yet use the APS, so can't use this function!!");
                this.btnDownload.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (this.useAPS.ToUpper() == "FALSE")
            {
                this.Close();
            }
        }

        // Download
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            DataRow dr;
            MyUtility.Check.Seek("select SQLServerName,APSDatabaseName from system WITH (NOLOCK)", out dr);
            if (MyUtility.Check.Empty(dr["SQLServerName"]) || MyUtility.Check.Empty(dr["APSDatabaseName"]))
            {
                MyUtility.Msg.WarningBox("Still not yet set APS Server data, Please contact Taipei MIS. Thank you.");
                return;
            }

            this.ShowWaitMessage("Data Downloading....");

            string sqlCmd = string.Format("exec dbo.usp_APSDataDownLoad '{0}','{1}','{2}','{3}'", MyUtility.Convert.GetString(dr["SQLServerName"]), MyUtility.Convert.GetString(dr["APSDatabaseName"]), Sci.Env.User.Factory, Sci.Env.User.UserID);
            DBProxy.Current.DefaultTimeout = 1200;
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WaitClear();
                this.ShowErr(sqlCmd, result);
                this.HideWaitMessage();
                return;
            }

            this.DeleteCutting();
            this.Setcuttingdate();

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Download successful !!");
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteCutting()
        {
            string sewdate = DateTime.Now.AddDays(90).ToShortDateString();

            this.HideWaitMessage();
            this.ShowWaitMessage("Data Update...");
            #region 先刪除不在SewingSchedule 內的Cutting 資料
            string sqlcmd = string.Format(
                @"Delete Cutting from Cutting WITH (NOLOCK) join 
            (Select a.id from Cutting a WITH (NOLOCK) where a.FactoryID = '{1}' and a.Finished = 0 and a.id not in 
            (Select distinct c.cuttingsp from orders c WITH (NOLOCK) , (SELECT orderid
            FROM Sewingschedule b WITH (NOLOCK) 
            WHERE Inline <= '{0}' And offline is not null and offline !=''
            AND b.FactoryID = '{1}' group by b.orderid) d where c.id = d.orderid and c.FtyGroup = '{1}')) f
            on cutting.id = f.ID",
                sewdate,
                Sci.Env.User.Factory);

            DBProxy.Current.DefaultTimeout = 600;

            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                MyUtility.Msg.WaitClear();
                this.ShowErr(sqlcmd, result);
                this.HideWaitMessage();
                return;
            }
            #region 移除使用TransactionScope

            // TransactionScope _transactionscope = new TransactionScope();
            // using (_transactionscope)
            // {
            //    try
            //    {
            //        if (!(dresult = DBProxy.Current.Execute(null, sqlcmd)))
            //        {
            //            _transactionscope.Dispose();
            //            ShowErr(sqlcmd, dresult);
            //            return;
            //        }

            // _transactionscope.Complete();
            //    }
            //    catch (Exception ex)
            //    {
            //        _transactionscope.Dispose();
            //        ShowErr("Commit transaction error.", ex);
            //        return;
            //    }
            // }
            // _transactionscope.Dispose();
            // _transactionscope = null;
            #endregion

            #endregion
        }

        private void Setcuttingdate()
        {
            string sewdate = DateTime.Now.AddDays(90).ToShortDateString();
            this.HideWaitMessage();
            this.ShowWaitMessage("Data Update.....");
            #region 找出需新增或update 的Cutting

            // DataTable cuttingtb;
            string updsql = string.Empty;
            updsql = string.Format(
                @"
insert into cutting(ID,worktype,sewInline,sewoffline,mDivisionid,FactoryID,AddName,AddDate)
Select id = ord.cuttingsp,worktype = a.Type,sewInline = min(ord.sewinline),sewoffline = max(ord.sewoffline),mDivisionid = '{2}',FactoryID = '{3}',AddName = '{4}' ,AddDate = GetDate()
from orders ord WITH (NOLOCK) inner join 
(
	Select cuttingsp
	from (
		Select distinct c.cuttingsp 
		from orders c WITH (NOLOCK)
		inner join Sewingschedule b WITH (NOLOCK) on b.orderid = c.id
		where c.IsForecast = 0 and c.LocalOrder = 0 
		and Inline <= '{0}' And offline is not null and offline !='' AND b.FactoryID = '{1}' 
	) e 
	Where e.cuttingsp is not null and e.cuttingsp !='' and e.cuttingsp not in (Select id from cutting WITH (NOLOCK) )
) cut on ord.cuttingsp = cut.CuttingSP 
outer apply(select top 1 Type from WorkOrder where ID = ord.cuttingsp)a
where ord.FtyGroup = '{1}'
group by ord.CuttingSp ,a.Type",
                sewdate,
                Sci.Env.User.Factory,
                Sci.Env.User.Keyword,
                Sci.Env.User.Factory,
                Sci.Env.User.UserID);

            // dresult = DBProxy.Current.Select("Production", sqlcmd, out cuttingtb);
            // string sewin, sewof;
            // foreach (DataRow dr in cuttingtb.Rows)
            // {
            //    if (dr["inline"] == DBNull.Value) sewin = "null";
            //    else sewin = Convert.ToDateTime(dr["inline"]).ToShortDateString();
            //    if (dr["offlinea"] == DBNull.Value) sewof = "null";
            //    else sewof = Convert.ToDateTime(dr["offlinea"]).ToShortDateString();

            // updsql = updsql + string.Format("insert into cutting(ID,sewInline,sewoffline,mDivisionid,FactoryID,AddName,AddDate) Values('{0}','{1}','{2}','{3}','{4}','{5}', GetDate()); ", dr["cuttingsp"], sewin, sewof, Sci.Env.User.Keyword, Sci.Env.User.Factory, Sci.Env.User.UserID);
            // }
            updsql = updsql + string.Format(
                @"
update cutting 
set SewInLine =s.inline,sewoffline = s.offlinea
from
(
	Select ord.cuttingsp,min(ord.sewinline) as inline ,max(ord.sewoffline) as offlinea 
	from orders ord WITH (NOLOCK) ,
	(
		Select * 
		from 
		(
			Select distinct c.cuttingsp 
			from orders c WITH (NOLOCK) , 
			(
				SELECT orderid 
				FROM Sewingschedule b WITH (NOLOCK) 
				WHERE Inline <= '{0}' And offline is not null and offline !='' AND b.FactoryID = '{1}' 
				group by b.orderid
			) d 
			where c.id = d.orderid and c.IsForecast = 0 and c.LocalOrder = 0 
		) e
		Where e.cuttingsp is not null 
		and e.cuttingsp in (Select id from cutting WITH (NOLOCK) )
	) cut
	where ord.cuttingsp = cut.CuttingSP and ord.FtyGroup = '{1}'
	group by ord.CuttingSp 
)s
where id = s.CuttingSP",
                sewdate,
                Sci.Env.User.Factory);

            DualResult result = DBProxy.Current.Execute(null, updsql);
            if (!result)
            {
                MyUtility.Msg.WaitClear();
                this.ShowErr(updsql, result);
                this.HideWaitMessage();
                return;
            }

            // dresult = DBProxy.Current.Select("Production", sqlcmd, out cuttingtb);
            // foreach (DataRow dr in cuttingtb.Rows)
            // {
            //    if (dr["inline"] == DBNull.Value) sewin = "";
            //    else sewin = Convert.ToDateTime(dr["inline"]).ToShortDateString();
            //    if (dr["offlinea"] == DBNull.Value) sewof = "";
            //    else sewof = Convert.ToDateTime(dr["offlinea"]).ToShortDateString();

            // updsql = updsql + string.Format("update cutting set SewInLine ='{0}',sewoffline = '{1}' where id = '{2}'; ", sewin, sewof, dr["cuttingsp"]);
            // }
            #region 修改資料,移除使用TransactionScope

            // TransactionScope _transactionscope = new TransactionScope();
            // using (_transactionscope)
            // {
            //    try
            //    {
            //        if (!(dresult = DBProxy.Current.Execute(null, updsql)))
            //        {
            //            _transactionscope.Dispose();
            //            ShowErr(updsql, dresult);
            //            return;
            //        }
            //        _transactionscope.Complete();
            //    }
            //    catch (Exception ex)
            //    {
            //        _transactionscope.Dispose();
            //        ShowErr("Commit transaction error.", ex);
            //        return;
            //    }
            // }
            // _transactionscope.Dispose();
            // _transactionscope = null;
            #endregion
            #endregion

            DBProxy.Current.DefaultTimeout = 0;
        }
    }
}

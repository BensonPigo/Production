using Ict;
using Sci.Data;
using System;
using Newtonsoft.Json;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Transactions;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using static Sci.Production.CallPmsAPI.PackingA2BWebAPI_Model;
using Sci.Production.CallPmsAPI;

namespace Sci.Production.Tools
{
    /// <inheritdoc/>
    public partial class SQL_Query : Win.Tems.QueryForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SQL_Query"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public SQL_Query(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.GridSetup();
        }

        private string sqlcmd;

        private void GridSetup()
        {
            this.gridSQLQuery.AutoGenerateColumns = true;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            this.sqlcmd = this.editSQL.Text;
            DualResult result;
            result = DBProxy.Current.Select(string.Empty, this.sqlcmd, null, out DataTable dt);
            if (!result)
            {
                this.ShowErr(this.sqlcmd, result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.btnSqlUpdate.Visible = true;
            }
            else
            {
                this.btnSqlUpdate.Visible = false;
            }
        }

        private void BtnSqlUpdate_Click(object sender, EventArgs e)
        {
            string[] dirs = Directory.GetFiles(Env.Cfg.ReportTempDir, "*.sql");
            string subject = string.Format("DataBase={0}, Account={1}, Factory={2} SQL Update Success !!", DBProxy.Current.DefaultModuleName, Env.User.UserName, Env.User.Factory);
            string desc = subject;

            if (dirs.Length == 0)
            {
                MyUtility.Msg.WarningBox("No update on this time !!", "Warning");
                return;
            }

            Exception exception = null;
            foreach (string dir in dirs)
            {
                string script = File.ReadAllText(dir);

                // DualResult upResult;
                TransactionScope transactionscope = new TransactionScope();
                using (transactionscope)
                {
                    try
                    {
                        DBProxy.Current.OpenConnection("Production", out SqlConnection connection);

                        using (connection)
                        {
                            Server db = new Server(new ServerConnection(connection));
                            db.ConnectionContext.ExecuteNonQuery(script);
                        }

                        transactionscope.Complete();
                        transactionscope.Dispose();
                    }
                    catch (Exception ex)
                    {
                        transactionscope.Dispose();
                        exception = ex;
                        subject = string.Format("DataBase={0}, Account={1}, Factory={2} SQL Update Fail !!", DBProxy.Current.DefaultModuleName, Env.User.UserName, Env.User.Factory);
                        desc = subject + string.Format(
                            @"
------------------------------------------------------------
{0}
-----------------------------------------------------------", ex.ToString());
                        this.Sendmail(subject, desc);
                        break;
                    }
                }

                transactionscope.Dispose();
                transactionscope = null;
            }

            if (exception != null)
            {
                this.ShowErr("Commit transaction error.", exception);
                return;
            }

            MyUtility.Msg.InfoBox("Update completed !!");
            this.Sendmail(subject, desc);
        }

        private void Sendmail(string subject, string desc)
        {
            string sql_update_receiver = ConfigurationManager.AppSettings["sql_update_receiver"];
            Win.Tools.MailTo mail = new Win.Tools.MailTo(Env.Cfg.MailFrom, sql_update_receiver, string.Empty, subject, string.Empty, desc, true, true);
            mail.ShowDialog();
        }

        private void btn_updateCBM_Click(object sender, EventArgs e)
        {
            if (!Env.User.IsAdmin || !MyUtility.Check.Seek($@"select 1 from Pass1 where ID='SCIMIS' and ID ='{Env.User.UserID}'"))
            {
                MyUtility.Msg.WarningBox("You don't have permission.");
                return;
            }

            DualResult result;
            string sqlPlFromRgCode = @"select distinct PLFromRgCode from GMTBooking_Detail";
            if (!(result = DBProxy.Current.Select(string.Empty, sqlPlFromRgCode, out DataTable dtFromRgCode)))
            {
                this.ShowErr(result);
                return;
            }

            if (dtFromRgCode == null || dtFromRgCode.Rows.Count == 0)
            {
                return;
            }

            DataTable dtShow = null;
            

            foreach (DataRow dr in dtFromRgCode.Rows)
            {
                string sqlGMTBooking = $@"select * from GMTBooking_Detail where PLFromRgCode = '{dr["PLFromRgCode"]}'";
                if (!(result = DBProxy.Current.Select(string.Empty, sqlGMTBooking, out DataTable dtGMTBooking)))
                {
                    this.ShowErr(result);
                    return;
                }

                if (dtGMTBooking != null && dtGMTBooking.Rows.Count > 0)
                {
                    string sqlGetModifyA2BPackingCBM = @"

use Production

-- 先建立基本檔需要修改的temp資料
select * 
into #tmpBasc
from 
(
	select NewCBM = case 
	when CtnUnit='Inch' 
		then convert(numeric(16,7),ROUND( CONVERT(float, CtnLength*CtnWidth*CtnHeight*0.00001639),7))
	when CtnUnit = 'MM' 
		then convert(numeric(16,7),ROUND(CONVERT(float, CtnLength*CtnWidth*CtnHeight/1000000000),7))
		else 0.00 end
	,CBM,RefNo
	from LocalItem l
	where CBM != (case 
		when CtnUnit='Inch' 
			then convert(numeric(16,7),ROUND( CONVERT(float, CtnLength*CtnWidth*CtnHeight*0.00001639),7))
		when CtnUnit = 'MM' 
			then convert(numeric(16,7),ROUND(CONVERT(float, CtnLength*CtnWidth*CtnHeight/1000000000),7))
			else 0.00 end
			)
	and exists(
		select 1 from PackingList_Detail pd
		inner join #tmp t on pd.ID = t.PackingListID
		where pd.RefNo = l.RefNo
	)

union all 
	-- 這些是6/20 user手動更新的 也要一併更新在關聯的Table
	select NewCBM = case 
	when CtnUnit='Inch' 
		then convert(numeric(16,7),ROUND( CONVERT(float, CtnLength*CtnWidth*CtnHeight*0.00001639),7))
	when CtnUnit = 'MM' 
		then convert(numeric(16,7),ROUND(CONVERT(float, CtnLength*CtnWidth*CtnHeight/1000000000),7))
		else 0.00 end
	,CBM,RefNo 
	from LocalItem where RefNo in ('C-05-4924','Z-800500160','Z-800500510','C-05-4924','C-05-4952','C-05-5015','C-05-1164','C-05-1165','C-05-1204','C-05-1205','C-05-128','C-05-2176','C-05-2177','C-05-4970')
) a

-- 更新CBM來源 LocalItem from #tmpBAse
update t
set t.CBM = s.NewCBM
from LocalItem t
inner join #tmpBasc s on t.RefNo = s.RefNo


-- 用更新後的CBM 重新計算Packinglist.CBM
select p1.ID,Packing_CMB = p1.CBM
,new_CBM = convert(numeric(11,4), sum(l.NewCBM * p2.CTNQty))
into #tmpPackingList
from PackingList p1
inner join PackingList_Detail p2 on p1.ID = p2.ID
inner join #tmpBasc l on l.RefNo = p2.RefNo
where exists(
	select 1 from #tmp t
	where t.PackingListID = p1.ID
)
group by p1.ID,p1.CBM

update t
set t.CBM = s.new_CBM
from PackingList t
inner join #tmpPackingList s on t.ID = s.ID
where s.Packing_CMB != s.new_CBM

select t.ID,ttlCBM = sum(p.CBM)
from #tmp t
inner join PackingList p on t.PackingListID = p.ID and t.id	 = p.INVNo
group by t.id	

drop table #tmpBasc,#tmpPackingList

";
                    DataBySql dataBySql = new DataBySql()
                    {
                        SqlString = sqlGetModifyA2BPackingCBM,
                        TmpTable = JsonConvert.SerializeObject(dtGMTBooking),
                        TmpTableName = "#tmp"
                    };
                    result = PackingA2BWebAPI.GetDataBySql(dr["PLFromRgCode"].ToString(), dataBySql, out DataTable dtA2BGMTBookingCBM);

                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    // 更新GMTBooking.TotalCBM
                    string updGMTBooling_CBM = @"
-- 要加總原廠區的Packinglist.CBM
select INVNo,ttlCBM= sum(CBM) 
into #tmpFinal
from (
-- A2B的CBM
select INVNo = ID,CBM = ttlCBM from #tmp
union all
-- 原廠區的CBM
select p.INVNo,p.CBM from PackingList p
inner join #tmp s on p.INVNo = s.ID
) a
group by INVNo

update t
set t.TotalCBM = s.ttlCBM
from GMTBooking t
inner join #tmpFinal s on t.ID = s.INVNo

select GMTBookingID = g.id,s.ShippingAPIDList,g.ttlCBM
from #tmp g
outer apply(
	select ShippingAPIDList = Stuff((
		select concat(',',ShippingAPID)
		from (
				select 	distinct
					ShippingAPID
				from dbo.ShareExpense se
				where se.InvNo = g.ID
			) s
		for xml path ('')
	) , 1, 1, '')
) s

drop table #tmpFinal
";
                    if (dtA2BGMTBookingCBM != null && dtA2BGMTBookingCBM.Rows.Count > 0)
                    {
                        result = MyUtility.Tool.ProcessWithDatatable(dtA2BGMTBookingCBM, string.Empty, updGMTBooling_CBM, out DataTable dtttlCBM);
                        if (!result)
                        {
                            this.ShowErr(result);
                            return;
                        }

                        if (dtShow == null)
                        {
                            dtShow = dtttlCBM;
                        }
                        else
                        {
                            dtShow.Merge(dtttlCBM);
                        }
                    }
                }
            }

            MyUtility.Msg.InfoBox("update successful datas will show on Grid");
            if (dtShow != null)
            {
                this.listControlBindingSource1.DataSource = dtShow;
            }
        }
    }
}

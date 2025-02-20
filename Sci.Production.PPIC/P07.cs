using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Threading.Tasks;
using Sci.Production.Automation;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P07
    /// </summary>
    public partial class P07 : Win.Tems.QueryForm
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
            string sqlCommand = "select UseAPS from factory WITH (NOLOCK) where ID = '" + Env.User.Factory + "'";
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

            this.labShowDateRange.Text = $@"Download from: {DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd")}〜{DateTime.Now.AddDays(365).ToString("yyyy-MM-dd")}";
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

            string sqlCmd = string.Format(
                @"
exec dbo.usp_APSDataDownLoad '{0}','{1}','{2}','{3}'
update factory set LastDownloadAPSDate  = getdate() where id = '{2}'

", MyUtility.Convert.GetString(dr["SQLServerName"]),
                MyUtility.Convert.GetString(dr["APSDatabaseName"]),
                Env.User.Factory,
                Env.User.UserID);

            DBProxy.Current.DefaultTimeout = 2400; // 40分鐘
            DataTable[] dsForAutomation;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dsForAutomation);
            if (!result)
            {
                MyUtility.Msg.WaitClear();
                this.ShowErr(sqlCmd, result);
                this.HideWaitMessage();
                return;
            }

            // 設定參數控制 0 = 排程執行、1 = 手動執行
            result = DBProxy.Current.Execute("Production", "exec dbo.ChangeOver 1");
            if (!result)
            {
                MyUtility.Msg.WaitClear();
                this.ShowErr(sqlCmd, result);
                this.HideWaitMessage();
                return;
            }

            string sql = $@"Select distinct Category, StyleType from ChgOverCheckList where FactoryID = '{Env.User.Factory}'";
            DataTable dt = new DataTable();
            result = DBProxy.Current.Select(null, sql, out dt);

            foreach (DataRow row in dt.Rows)
            {
                var sqlUpdate = @"
-- 找出符合以下條件的ChgOver
-- 1. Inline >= 今天，並日期大於2025-01-01
select *
into #ChgOver
from ChgOver co with (nolock)
where co.Category = @Category
and co.Type = @StyleType
and co.FactoryID = @FactoryID
and co.Inline >= DATEADD(year, -1, GETDATE())
and co.Inline >= '2025-01-01'

-- 刪除並重新寫入ChgOver_Check資料
if @@RowCount > 0
begin
    
    Delete cc
    From ChgOver_Check cc
    Where Not exists ( 
                       Select 1
                       From  ChgOverCheckList ckl 
                       Inner join ChgOverCheckList_Detail ckd with (nolock) on ckl.ID = ckd.ID
                       Where cc.ChgOverCheckListID = ckl.ID 
                       and cc.No = ckd.ChgOverCheckListBaseID 
                     )
    And cc.ID in (select ID from #ChgOver)

    Insert into ChgOver_Check (ID, ChgOverCheckListID, Deadline, No, LeadTime, ResponseDep)
    Select co.ID, ck.ID, dbo.CalculateWorkDate(co.Inline, -ckd.LeadTime, co.FactoryID) as s , ckd.ChgOverCheckListBaseID, ckd.LeadTime, ckd.ResponseDep
    From #ChgOver co
    Inner join ChgOverCheckList ck with (nolock) on ck.Category = @Category and ck.StyleType = @StyleType and ck.FactoryID = co.FactoryID
    Inner join ChgOverCheckList_Detail ckd with (nolock) on ck.ID = ckd.ID
    Where Not exists ( 
                        Select 1
                        From ChgOver_Check cc
                        Where cc.id = co.ID
                        and cc.ChgOverCheckListID = ck.ID
                        AND cc.No = ckd.ChgOverCheckListBaseID
                     )

    Update cc Set Deadline = dbo.CalculateWorkDate(co.Inline, -ckd.LeadTime, co.FactoryID),
                  LeadTime = ckd.LeadTime, 
                  ResponseDep = ckd.ResponseDep
    From ChgOver_Check cc
    Inner join #ChgOver co on cc.ID = co.ID
    Inner join ChgOverCheckList ck with (nolock) on ck.Category = @Category and ck.StyleType = @StyleType and ck.FactoryID = co.FactoryID
    Inner join ChgOverCheckList_Detail ckd with (nolock) on ck.ID = ckd.ID and cc.No = ckd.ChgOverCheckListBaseID
    
end
";
                List<SqlParameter> listPar = new List<SqlParameter>()
                {
                    new SqlParameter("@Category", row["Category"]),
                    new SqlParameter("@StyleType", row["StyleType"]),
                    new SqlParameter("@FactoryID", Env.User.Factory),
                };

                DualResult updateChgOver = DBProxy.Current.Execute(null, sqlUpdate, listPar);
                if (!updateChgOver)
                {
                    this.ShowErr(updateChgOver);
                }
            }

            if (dsForAutomation[0].Rows.Count > 0)
            {
                Task.Run(() => new Guozi_AGV().SentSewingLineToAGV(dsForAutomation[0]))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }

            if (dsForAutomation[1].Rows.Count > 0)
            {
                Task.Run(() => new Guozi_AGV().SentSewingScheduleToAGV(dsForAutomation[1]))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }

            if (dsForAutomation[2].Rows.Count > 0)
            {
                Task.Run(() => new Guozi_AGV().SentDeleteSewingSchedule(dsForAutomation[2]));
            }

            this.Setcuttingdate();

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Download successful !!");

            DBProxy.Current.DefaultTimeout = 300; // 5 分鐘
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                Env.User.Factory,
                Env.User.Keyword,
                Env.User.Factory,
                Env.User.UserID);

            updsql = updsql + string.Format(
                @"
--Sewingschedule有cutting也有
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
where id = s.CuttingSP


--Sewingschedule沒有，cutting有
update cutting set SewInLine = s.sewinline, sewoffline = s.sewoffline
from Cutting WITH (NOLOCK) 
join (
	Select o2.cuttingsp,sewinline=min(o2.sewinline) ,sewoffline=max(o2.sewoffline)
	from Cutting c WITH (NOLOCK) 
	inner join orders o2 with(nolock) on o2.CuttingSP =  c.ID
	where c.FactoryID = '{1}'
	and c.Finished = 0 
	and not exists (
		Select 1
		from orders o WITH (NOLOCK)
		inner join Sewingschedule s WITH (NOLOCK) on o.id = s.orderid 
		where s.Inline <= '{0}'
		And s.offline is not null and s.offline !='' 
		and s.FactoryID = '{1}'
		and o.FtyGroup = '{1}' 
		and o.cuttingsp  = c.id
	)
	and exists (
		select 1
		from Orders o
		where o2.POID = o.ID
				and o.IsForecast = 0 
				and o.LocalOrder = 0 
	)
	group by o2.CuttingSp 
)s on cutting.id = s.cuttingsp
",
                sewdate,
                Env.User.Factory);

            DualResult result = DBProxy.Current.Execute(null, updsql);
            if (!result)
            {
                MyUtility.Msg.WaitClear();
                this.ShowErr(updsql, result);
                this.HideWaitMessage();
                return;
            }

            #endregion
        }
    }
}

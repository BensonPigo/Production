using Sci.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Net;
using System.IO;
using Sci;
using System.Diagnostics;
using PostJobLog;
using System.Threading;
using System.Configuration;

namespace PowerBI.Daily
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        // 測試的開關寫在App.config
        private bool bolTEST = MyUtility.Convert.GetBool(System.Configuration.ConfigurationManager.AppSettings["TestBool"]);

        // mailserver 參數(位置在台北端)
        private string mailFrom = "foxpro@sportscity.com.tw";
        private string mailServer = "172.17.2.8";
        private string eMailID = "foxpro";
        private string eMailPwd = "orpxof";

        private int intHashCode;
        bool isAuto = false;
        TransferPms transferPMS = new TransferPms();
        bool isTestJobLog = MyUtility.Convert.GetBool(System.Configuration.ConfigurationManager.AppSettings["TestBool"]);
        string tpeMisMail = string.Empty;
        DataTable transAll;
        string mailTO,ccMailTo;
        DateTime? StartTime, EndTime;

        public BindingList<TransTask> Tasks_All { get; set; } = new BindingList<TransTask>();

        public BindingList<TransRegion> Regions_All { get; set; } = new BindingList<TransRegion>();

        /// <summary>
        /// delegate of Create Sql paramters
        /// </summary>
        /// <returns> list of sql parameter </returns>
        public delegate List<SqlParameter> CreateSqlParams();

        private int _Logger_Retry = 10;
        /// <summary>
        /// Log insert db retry times
        /// </summary>
        public int Logger_Retry
        {
            get { return this._Logger_Retry; }
            set { this._Logger_Retry = value; }
        }

        public Main()
        {
            InitializeComponent();
            isAuto = false;
        }

        public Main(String _isAuto)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(_isAuto))
            {
                isAuto = true;
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            OnRequery();

            //transferPMS.fromSystem = "PowerBI";

            if (isAuto)
            {
                ClickUpdate();
                this.Close();
            }
        }

        private void OnRequery()
        {
            mailTO = bolTEST ?
                  ConfigurationManager.AppSettings["TestMailTO"].ToString() :
                  ConfigurationManager.AppSettings["MailTO"].ToString();
            ccMailTo = bolTEST ?
                ConfigurationManager.AppSettings["TestMailTO"].ToString() :
                  ConfigurationManager.AppSettings["MailTO"].ToString();
        }

        #region 接Sql Server 進度訊息用
        private void InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            BeginInvoke(() => { this.labelProgress.Text = e.Message; });
        }
        #endregion

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ClickUpdate();
        }

        private void SendMail(String subject = "", String desc = "", bool isFail = true)
        {
            transferPMS.SetSMTP(mailServer, 25, eMailID, eMailPwd);
            string toAddress = mailTO;
            string ccAddress = ccMailTo;

            if (isFail)
            {
                toAddress += MyUtility.Check.Empty(toAddress) ? this.tpeMisMail : ";" + this.tpeMisMail;
            }

            if (String.IsNullOrEmpty(subject))
            {
                subject = ConfigurationManager.AppSettings["MailSubject"].ToString();
            }
            if (String.IsNullOrEmpty(desc))
            {
                desc = ConfigurationManager.AppSettings["MailContent"].ToString();
            }

            if (!MyUtility.Check.Empty(toAddress))
            {
                Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(mailFrom, toAddress, ccAddress, subject, "", desc, true, true);

                mail.ShowDialog();
            }
        }

        private void ClickUpdate()
        {
            SqlConnection conn;
            DateTime startDate;
            DateTime endDate;

            if (!Sci.SQL.GetConnection(out conn)) { return; }

            conn.InfoMessage += new SqlInfoMessageEventHandler(InfoMessage);

            startDate = DateTime.Now;
            DualResult result = AsyncHelper.Current.DataProcess(this, () =>
            {
                return AsyncUpdateExport(conn);
            });
            endDate = DateTime.Now;
            // 完成後發信
            //mymailTo(result.ToSimpleString());

            if (!result)
            {
                ShowErr(result);
            }
            else
            {
                writeJobLog(true);
            }
           
            conn.Close();
            issucess = true;
        }

        bool issucess = true;
        #region Export/Update (非同步)
        private DualResult AsyncUpdateExport(SqlConnection conn)
        {
            DualResult result;
            try
            {
                intHashCode = Guid.NewGuid().GetHashCode();
                result = DailyUpdate();
                if (!result)
                {
                    writeJobLog(false); ;
                    return result;
                }
            }
            catch (SqlException se)
            {
                issucess = false;
                return Ict.Result.F(se);
            }

            return Ict.Result.True;
        }
        #endregion

        /// <summary>
        /// Call JobLog web api回傳執行結果
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="desc"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isTest"></param>
        /// <param name="succeeded"></param>
        private void CallJobLogApi(string subject, string desc, string startDate, string endDate, bool isTest, bool succeeded)
        {
            JobLog jobLog = new JobLog()
            {
                GroupID = "P",
                SystemID = "Power BI",
                Region = "TPE",
                MDivisionID = "TPE",
                OperationName = subject,
                StartTime = startDate,
                EndTime = endDate,
                Description = desc,
                FileName = new List<string>(),
                FilePath = string.Empty,
                Succeeded = succeeded
            };
            CallTPEWebAPI callTPEWebAPI = new CallTPEWebAPI(isTest);
            callTPEWebAPI.CreateJobLogAsnc(jobLog, null);
        }

        private DualResult DailyUpdate()
        {
            DualResult result;

            #region Setup Data  
            string sqlCmd = string.Empty;
            sqlCmd = $@"
Use [PBIReportData];
SELECT Region
	,[DirName] = ''
	,[RarName] = ''
	,[Is_Export] = 0
	,[ConnectionName] = r.ConnectionName
	,[DBName] = ''
	,[DBFileName] = ''
	,[GroupID] = ROW_NUMBER() over(partition by seq order by region)--i.GroupID
	,[Seq] = i.Seq
	,[Name] = i.Name
,[TSQL] = [TSQL] + ' '+''''+LinkServerName+''''
FROM P_TransRegion r
left join P_TransImport i on r.ConnectionName = i.ImportConnectionName
";

            result = DBProxy.Current.Select("PBIReportData", sqlCmd, out transAll);
            if (!result) { return result; }

            if (transAll.Rows.Count > 0)
            {
                SetupData(transAll);
            }

            #endregion

            bool ByGroupStatus = Update_ByGroupID();

            #region 只要有任一Store Procedure沒都有寫進Log, or 錯誤訊息是DeadLock,就重新執行!

            DataTable dtReExec;
            string sqlReExec = $@"
Use [PBIReportData];
SELECT Region
	,[DirName] = ''
	,[RarName] = ''
	,[Is_Export] = 0
	,[ConnectionName] = r.ConnectionName
	,[DBName] = ''
	,[DBFileName] = ''
	,[GroupID] = ROW_NUMBER() over(partition by seq order by region)--i.GroupID
	,[Seq] = i.Seq
	,[Name] = i.Name
,[TSQL] = [TSQL] + ' '+''''+LinkServerName+''''
FROM P_TransRegion r
left join P_TransImport i on r.ConnectionName = i.ImportConnectionName
where not exists(
	select * from P_TransLog t
	where t.TransCode='{intHashCode}' and t.FunctionName not in ('Start Update_PoweBI_InThread','End Update_PowerBI_InThread')
	and i.Name = t.FunctionName
	and r.Region = t.RegionID
)
or exists(
	select * from P_TransLog t
	where t.TransCode = '{intHashCode}'
	and t.Description like '%deadlock%'
	and i.Name = t.FunctionName
	and r.Region = t.RegionID
)

";

            result = DBProxy.Current.Select("PBIReportData", sqlReExec, out dtReExec);
            if (!result) { return result; }

            if (dtReExec.Rows.Count > 0)
            {
                string sqlcmd = string.Empty;
                foreach (DataRow dr in dtReExec.Rows)
                {
                    sqlcmd = dr["TSQL"].ToString();
                    string regionID = dr["Region"].ToString().Trim();
                    var foundRegion = this.Regions_All.Where(region => region.Region.EqualString(regionID));

                    TransRegion tRegion = foundRegion.Count() == 0
                        ? new TransRegion(dr)
                        : foundRegion.First();
                    TransTask task = new TransTask(dr, tRegion);
                    task.TSQL = sqlcmd;
                    result = DBProxy.Current.OpenConnection("PBIReportData", out SqlConnection conn);
                    this.Transfer_Task(task, conn);
                }
            }
            #endregion

            #region 寫log End
            EndTime = DateTime.Now;

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@functionName", "End Update_PowerBI_InThread"));
            pars.Add(new SqlParameter("@Description", ""));
            pars.Add(new SqlParameter("@StartTime", DateTime.Now));
            pars.Add(new SqlParameter("@EndTime", DateTime.Now));
            pars.Add(new SqlParameter("@RegionID", ""));
            pars.Add(new SqlParameter("@GroupID", ""));
            pars.Add(new SqlParameter("@TransCode", intHashCode));

            string cmd = @"
insert into P_TransLog( functionName, Description, StartTime, EndTime, RegionID, GroupID, TransCode) 
                values(@functionName,@Description,@StartTime,@EndTime,@RegionID,@GroupID,@TransCode)";

            if (!(result = DBProxy.Current.Execute("", cmd, pars)))
            {
                ShowErr(result);
            }
            #endregion

            if (!ByGroupStatus)
            {
                return new DualResult(false, "Update failed!");
            }

            return Ict.Result.True;
        }

        private void btnTestWebAPI_Click(object sender, EventArgs e)
        {
          this.CallJobLogApi("Import PowerBI Report Data", "PowerBI Update DB ", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), true, true);
        }

        private void btnTestMail_Click(object sender, EventArgs e)
        {
            SendMail("PowerBI Test", "PowerBI Test");
        }

        /// <summary>
        /// Update Data By GroupID
        /// </summary>
        /// <returns> Execute Result </returns>
        private bool Update_ByGroupID()
        {
            if (this.Tasks_All == null || this.Tasks_All.Count == 0)
            {
                return true;
            }

            if (!this.Tasks_All.Any(t => t.TaskSelected && t.RegionSelected))
            {
                MyUtility.Msg.InfoBox("No task selected to do update");
                return true;
            }

            #region 寫log Start
            StartTime = DateTime.Now;

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@functionName", "Start Update_PoweBI_InThread"));
            pars.Add(new SqlParameter("@Description", ""));
            pars.Add(new SqlParameter("@StartTime", DateTime.Now));
            pars.Add(new SqlParameter("@EndTime", DateTime.Now));
            pars.Add(new SqlParameter("@regionID", ""));
            pars.Add(new SqlParameter("@GroupID", ""));
            pars.Add(new SqlParameter("@TransCode", intHashCode));

            string cmd = @"insert into P_TransLog(functionName,Description,StartTime,EndTime,RegionID,GroupID,TransCode) 
                        values(@functionName,@Description,@StartTime,@EndTime,@RegionID,@GroupID,@TransCode)";

            DualResult result;
            if (!(result = DBProxy.Current.Execute("", cmd, pars)))
            {
                ShowErr(result);
            }
            #endregion

            var success_Before = this.Tasks_All
              .Where(t => t.GroupID < 0)
              .Where(t => t.TaskSelected && t.RegionSelected && t.TransRegion.Is_Export == false && t.TransRegion.LastResult)
              //// by Region 拆完Thread
              .GroupBy(t => t.GroupID)
              .AsParallel()
              //// 各別 Region 執行的結果
              .Select(rTasks => this.Update_ByGroupID_InThread(rTasks.ToList()))
              .AsSequential()
              .All(boolValue => boolValue);

            if (!success_Before)
            {
                return false;
            }

            var success = this.Tasks_All
                .Where(t => t.GroupID >= 0)
                .Where(t => t.TaskSelected && t.RegionSelected && t.TransRegion.Is_Export == false && t.TransRegion.LastResult)
                //// by Region 拆完Thread
                .GroupBy(t => t.GroupID)
                .AsParallel()
                //// 各別 Region 執行的結果
                .Select(rTasks => this.Update_ByGroupID_InThread(rTasks.ToList()))
                .AsSequential()
                .All(boolValue => boolValue);

            return success;
        }

        /// <summary>
        /// Setup data
        /// </summary>
        /// <param name="task_fullJoin_region"> datatable TransRegion </param>
        public void SetupData(DataTable task_fullJoin_region)
        {
            this.Regions_All.Clear();
            this.Tasks_All.Clear();
            foreach (DataRow row in task_fullJoin_region.Rows)
            {
                string regionID = row["Region"].ToString().Trim();
                var foundRegion = this.Regions_All.Where(region => region.Region.EqualString(regionID));

                TransRegion tRegion = foundRegion.Count() == 0
                    ? new TransRegion(row)
                    : foundRegion.First();
                TransTask task = new TransTask(row, tRegion);
                this.Tasks_All.Add(task);

                if (foundRegion.Count() == 0)
                {
                    this.Regions_All.Add(tRegion);
                }

                task.TransRegion = tRegion;
            }
        }


        /// <summary>
        /// import by groupid in thread
        /// </summary>
        /// <param name = "region_Tasks" > region list</param>
        /// <returns> Execute Result</returns>
        private bool Update_ByGroupID_InThread(List<TransTask> region_Tasks)
        {
            if (region_Tasks == null || region_Tasks.Count == 0)
            {
                return true;
            }

            TransRegion region = region_Tasks[0].TransRegion;

            var tmp = region_Tasks
                .OrderBy(t => t.Region)
                .GroupBy(t => t.Region)
                .Select(regionTasks => Transfer_Task_BySeq(regionTasks.ToList()))
                .ToList();

            if (!tmp.All(regionSucceeded => regionSucceeded))
            {
                return false;
            }

            // 結束作業
            return this.After_Import_ByRegion(region);
        }

        /// <summary>
        /// 執行的先後順序依照Seq排序
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        private bool Transfer_Task_BySeq(List<TransTask> tasks)
        {
            SqlConnection conn;
            DualResult result = DBProxy.Current.OpenConnection(tasks[0].TransRegion.ConnectionName, out conn);
            return result
                && tasks.OrderBy(t => t.Seq)
                    .All(t => this.Transfer_Task(t, conn));
        }

        /// <summary>
        /// after import by region
        /// </summary>
        /// <param name="region"> region of factory </param>
        /// <returns> Execute Result </returns>
        public virtual bool After_Import_ByRegion(TransRegion region)
        {
            return true;
        }

        /// <summary>
        /// 包Try Catch和 Transaction
        /// </summary>
        /// <param name="task"> task </param>
        /// <param name="conn"> sql connection </param>
        /// <param name="pars"> sql parameters </param>
        /// <returns> Execute Result </returns>
        public virtual bool Transfer_Task(TransTask task, SqlConnection conn, List<SqlParameter> pars = null)
        {
            DateTime startTime = DateTime.Now;
            //// 將 task 內的 exec [storeProcedure]  重新用try catch 包裝
            string newSql =
@"BEGIN TRY
	Begin tran

    " + task.TSQL + @"

	Commit tran
END TRY
BEGIN CATCH
	RollBack Tran

    declare @ErrMsg varchar(1000) = 'Err# : ' + ltrim(str(ERROR_NUMBER())) + 
				CHAR(10)+'Error Severity:'+ltrim(str(ERROR_SEVERITY()  )) +
				CHAR(10)+'Error State:' + ltrim(str(ERROR_STATE() ))  +
				CHAR(10)+'Error Proc:' + isNull(ERROR_PROCEDURE(),'')  +
				CHAR(10)+'Error Line:'+ltrim(str(ERROR_LINE()  )) +
				CHAR(10)+'Error Msg:'+ ERROR_MESSAGE() ;
    
    RaisError( @ErrMsg ,16,-1)
END CATCH";
            DualResult result = DBProxy.Current.ExecuteByConn(conn, newSql);
            task.Succeeded = result;
            task.DualResult = result;
            this.Log(task.TransRegion, task, startTime, DateTime.Now);
            return result;
        }

        /// <summary>
        /// Save log
        /// </summary>
        /// <param name="region"> region of factory </param>
        /// <param name="task"> task </param>
        /// <param name="startTime"> start time </param>
        /// <param name="endTime"> end time </param>
        public virtual void Log(TransRegion region, TransTask task, DateTime? startTime, DateTime? endTime)
        {
            task.StartTime = startTime ?? DateTime.Now;
            task.EndTime = endTime ?? DateTime.Now;

            //// Log to DB with retry
            for (int i = 0; i < this.Logger_Retry; i++)
            {
                string cmd = string.Empty;
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@functionName", task.Name));
                pars.Add(new SqlParameter("@Description", task.DualResult.ToString()));
                pars.Add(new SqlParameter("@StartTime", task.StartTime));
                pars.Add(new SqlParameter("@EndTime", task.EndTime));
                pars.Add(new SqlParameter("@regionID", task.Region));
                pars.Add(new SqlParameter("@TransCode", intHashCode));
                pars.Add(new SqlParameter("@GroupID", task.GroupID));

                if (MyUtility.Check.Seek("select * from P_TransLog where TransCode = @TransCode and RegionID = @RegionID and FunctionName = @functionName ",pars))
                {
                    cmd = $@"update P_TransLog set StartTime = @StartTime,EndTime = @EndTime,Description = @Description where TransCode = @TransCode and RegionID = @RegionID and FunctionName = @functionName";
                }
                else
                {
                    cmd = @"insert into P_TransLog(functionName,Description,StartTime,EndTime,RegionID,TransCode,GroupID) 
                        values(@functionName,@Description,@StartTime,@EndTime,@RegionID,@TransCode,@GroupID)";
                }

                

                DualResult result;
                if (result = DBProxy.Current.Execute("", cmd, pars))
                {
                    break;
                }
            }
        }

        private void writeJobLog(bool isSucceed)
        {
            // 只要P_TransLog.Description有值, 代表有error msg所以判斷為Failed
            string cmd = $@"
select FailCnt = SUM(cnt),b.MailName
from(
select cnt = case when Description !='' then count(1) else 0 end
,FunctionName
from P_TransLog 
where TransCode = {intHashCode}
group by FunctionName,Description
) a
inner join P_TransImport b on a.FunctionName = b.Name
group by b.MailName
order by b.MailName";
            DualResult result;
            DataTable dtFail;
            if (!(result = DBProxy.Current.Select("", cmd, out dtFail)))
            {
                ShowErr(result);
            }

            string desc = $@"
Please check below information.
Transfer date: {DateTime.Now.ToString("yyyy-MM-dd")}
M: TPE" + Environment.NewLine;
            foreach (DataRow dr in dtFail.Rows)
            {
                string status = MyUtility.Check.Empty(dr["FailCnt"]) ? "is completed" : "Failed";
                desc += $"[{dr["MailName"]}] " + status + Environment.NewLine;
            }

            this.CallJobLogApi("Import PowerBI Report Data", desc, ((DateTime)StartTime).ToString("yyyyMMdd HH:mm:ss"), ((DateTime)EndTime).ToString("yyyyMMdd HH:mm:ss"), isTestJobLog, isSucceed);
        }

        /// <summary>
        /// 發信用
        /// </summary>
        /// <param name="msg"></param>
        private void mymailTo(string msg)
        {
            String subject = "";
            String desc = "";

            #region 完成後發送Mail
            #region 組合 Desc
            desc = "UPDATE PowerBI TPE Data " + Environment.NewLine;
            desc += Environment.NewLine + "Dear All:" + Environment.NewLine + Environment.NewLine +
                   "             (**Please don't reply this mail. **)" + Environment.NewLine + Environment.NewLine +
                   "PMS system already uploaded data to PowerBI." + Environment.NewLine +
                   "Pls confirm and take notes." + Environment.NewLine +
                   "================================" +
                    Environment.NewLine + ConfigurationManager.AppSettings["MailContent"].ToString() + Environment.NewLine;
            if (!MyUtility.Check.Empty(msg))
            {
                desc += "Sql msg:" + Environment.NewLine +
                    msg + Environment.NewLine;
                issucess = false;
            }
            #endregion

            subject = ConfigurationManager.AppSettings["MailSubject"].ToString().TrimEnd() + " - [" + this.CurrentData["RgCode"].ToString() + "]";
            if (issucess)
            {
                subject += " Success";
            }
            else
            {
                subject += " Error!";
            }
            SendMail(subject, desc, !issucess);
            this.CallJobLogApi(subject, desc, ((DateTime)StartTime).ToString("yyyyMMdd HH:mm:ss"), ((DateTime)EndTime).ToString("yyyyMMdd HH:mm:ss"), isTestJobLog, issucess);
            #endregion
        }
    }
}

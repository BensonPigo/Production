using Ict;
using PostJobLog;
using Sci.Data;
using Sci.Production.Prg.PowerBI.DataAccess;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Base
    {
        private enum ListName
        {
            P_MonthlySewingOutputSummary,
            P_SewingLineSchedule,
            P_SewingLineScheduleBySP,
            P_InlineDefectSummary,
            P_CuttingScheduleOutputList,
            P_QAR31,
            P_QA_CFAMasterList,
            P_CartonScanRate,
            P_CartonStatusTrackingList,
            P_FabricDispatchRate,
            P_IssueFabricByCuttingTransactionList,
        }

        /// <summary>
        /// Is Test
        /// </summary>
        /// <returns>bool</returns>
        public bool IsTest()
        {
            bool isTest = true;
            if (DBProxy.Current.DefaultModuleName.Contains("Formal"))
            {
                isTest = false;
            }

            return isTest;
        }

        /// <summary>
        /// Call JobLog web api回傳執行結果
        /// </summary>
        /// <param name="job">JobLog</param>
        /// <returns>Job Log Ukey</returns>
        public string CallJobLogApi(JobLog job)
        {
            string ukey = string.Empty;
            JobLog jobLog = new JobLog()
            {
                GroupID = job.GroupID,              // "P"
                SystemID = job.SystemID,            // "Power BI"
                Region = job.Region,
                MDivisionID = job.MDivisionID,
                OperationName = job.OperationName,  // "Import BI Data"
                StartTime = job.StartTime,
                EndTime = job.EndTime,
                Description = job.Description,
                FileName = job.FileName,
                FilePath = job.FilePath,
                Succeeded = job.Succeeded,
            };

            CallTPEWebAPI callTPEWebAPI = new CallTPEWebAPI(this.IsTest());
            ukey = callTPEWebAPI.CreateJobLogAsnc(jobLog, null);
            return ukey;
        }

        /// <summary>
        /// Get System Region
        /// </summary>
        /// <returns>RgCode</returns>
        public string GetRegion()
        {
            string sql = $"select [RgCode] = REPLACE(s.RgCode, 'PHI', 'PH1') from System s";
            return MyUtility.GetValue.Lookup(sql, connectionName: "Production");
        }

        /// <summary>
        /// Get MDivision
        /// </summary>
        /// <param name="factoryID">Factory ID</param>
        /// <returns>MDivision</returns>
        public string GetMDivision(string factoryID)
        {
            string sql = $"select f.MDivisionID from Factory f where f.ID = '{factoryID}'";
            return MyUtility.GetValue.Lookup(sql, connectionName: "Production");
        }

        /// <summary>
        /// Get DB BITaskInfo
        /// </summary>
        /// <returns>Executed List</returns>
        public List<ExecutedList> GetExecuteList()
        {
            List<ExecutedList> executes = new List<ExecutedList>();
            string sql = @"
SELECT i.*
	, [Group] = ISNULL(g.[GroupID], 0)
	, [SEQ] = ISNULL(g.[SEQ], 1)
FROM BITaskInfo i
LEFt JOIN BITaskGroup g ON i.[Name] = g.[Name]
WHERE i.[Junk] = 0
ORDER BY [Group], [SEQ], [NAME]";
            DualResult result = DBProxy.Current.Select("PowerBI", sql, out DataTable dataTable);
            if (!result || dataTable.Rows.Count == 0)
            {
                return executes;
            }

            foreach (DataRow dr in dataTable.Rows)
            {
                string className = dr["Name"].ToString();
                string procedureName = dr["ProcedureName"].ToString();
                string dbName = dr["DBName"].ToString();
                bool hasStartDate = (bool)dr["HasStartDate"];
                bool hasEndDate = (bool)dr["HasEndDate"];
                bool hasStartDate2 = (bool)dr["HasStartDate2"];
                bool hasEndDate2 = (bool)dr["HasEndDate2"];
                bool runOnSunday = (bool)dr["RunOnSunday"];
                DateTime? sDate = hasStartDate ? DateTime.Parse(this.GetSQLdate(dr["StartDateDefault"].ToString())) : (DateTime?)null;
                DateTime? eDate = hasEndDate ? DateTime.Parse(this.GetSQLdate(dr["EndDateDefault"].ToString())) : (DateTime?)null;
                DateTime? sDate2 = hasStartDate2 ? DateTime.Parse(this.GetSQLdate(dr["StartDateDefault2"].ToString())) : (DateTime?)null;
                DateTime? eDate2 = hasEndDate2 ? DateTime.Parse(this.GetSQLdate(dr["EndDateDefault2"].ToString())) : (DateTime?)null;
                string procedureNameS = !string.IsNullOrEmpty(procedureName) ? string.Concat("(", procedureName, ")") : string.Empty;
                string sDate_s = hasStartDate ? "Sdate : " + sDate.Value.ToShortDateString() : string.Empty;
                string eDate_s = hasEndDate ? "Edate : " + eDate.Value.ToShortDateString() : string.Empty;
                string sDate_s2 = hasStartDate2 ? "Sdate2 : " + sDate2.Value.ToShortDateString() : string.Empty;
                string eDate_s2 = hasEndDate2 ? "Edate2 : " + eDate2.Value.ToShortDateString() : string.Empty;
                string remark = $@"{dr["Source"]}{procedureNameS}{Environment.NewLine}{sDate_s}{Environment.NewLine}{eDate_s}{Environment.NewLine}{sDate_s2}{Environment.NewLine}{eDate_s2}";
                int group = (int)dr["Group"];
                int seq = (int)dr["SEQ"];

                ExecutedList model = new ExecutedList()
                {
                    ClassName = className,
                    ProcedureName = procedureName,
                    DBName = dbName,
                    SDate = sDate,
                    EDate = eDate,
                    SDate2 = sDate2,
                    EDate2 = eDate2,
                    RunOnSunday = runOnSunday,
                    Remark = remark,
                    Group = group,
                    SEQ = seq,
                };

                executes.Add(model);
            }

            return executes;
        }

        /// <summary>
        /// Get SQL Date Need Format Trans
        /// </summary>
        /// <param name="tsql">SQL</param>
        /// <returns>Date (string)</returns>
        public string GetSQLdate(string tsql)
        {
            string sql = $"select Date = {tsql}";
            return MyUtility.GetValue.Lookup(sql, connectionName: "Production");
        }

        /// <summary>
        /// Execute all BI List And Use Thread
        /// </summary>
        /// <param name="executedList">ExecutedList</param>
        public void ExecuteAll(List<ExecutedList> executedList)
        {
            if (executedList.Where(x => !string.IsNullOrEmpty(x.ClassName)).Count() == 0)
            {
                return;
            }

            DateTime stratExecutedTime = DateTime.Now;
            List<ExecutedList> executedListDetail = new List<ExecutedList>();
            List<ExecutedList> executedListEnd = new List<ExecutedList>();

            var results = executedList
                .GroupBy(x => x.Group)
                .AsParallel()
                .AsOrdered()
                .Select(item =>
                {
                    var results_detail = item
                        .OrderBy(x => x.SEQ)
                        .AsParallel()
                        .AsSequential()
                        .Select(detail => this.ExecuteSingle(detail))

                        // .TakeWhile(model => model.Group == 0 || model.Success) // 只保留成功的结果
                        .ToList();

                    foreach (var item_detail in results_detail)
                    {
                        executedListDetail.Add(item_detail);
                    }

                    return executedListDetail;
                });

            foreach (var item in results)
            {
                executedListEnd.AddRange(item);
            }

            this.UpdateJobLogAndSendMail(executedListEnd, stratExecutedTime);
        }

        /// <summary>
        /// Execute Single BI
        /// </summary>
        /// <param name="item">Need Execute List</param>
        /// <returns>Execute Result List</returns>
        public ExecutedList ExecuteSingle(ExecutedList item)
        {
            DateTime? executeSDate = DateTime.Now;
            Base_ViewModel result = new Base_ViewModel();

            if (Enum.TryParse(item.ClassName, out ListName className))
            {
                switch (className)
                {
                    case ListName.P_MonthlySewingOutputSummary:
                        result = new P_Import_MonthlySewingOutputSummary().P_MonthlySewingOutputSummary(item.SDate, item.EDate);
                        break;
                    case ListName.P_SewingLineSchedule:
                        result = new P_Import_SewingLineScheduleBIData().P_SewingLineScheduleBIData(item.SDate, item.EDate);
                        break;
                    case ListName.P_InlineDefectSummary:
                        result = new P_Import_InlineDefec().P_InlineDefecBIData(item.SDate, item.EDate);
                        break;
                    case ListName.P_CuttingScheduleOutputList:
                        result = new P_Import_CuttingScheduleOutputList().P_CuttingScheduleOutputList(item.SDate, item.EDate);
                        break;
                    case ListName.P_QAR31:
                        result = new P_Import_QAR31().P_QAR31(item.SDate, item.EDate);
                        break;
                    case ListName.P_QA_CFAMasterList:
                        result = new P_Import_QA_CFAMasterList().P_QA_CFAMasterList(item.SDate);
                        break;
                    case ListName.P_SewingLineScheduleBySP:
                        result = new P_Import_SewingLineScheduleBySP().P_SewingLineScheduleBySP(item.SDate, item.EDate);
                        break;
                    case ListName.P_CartonScanRate:
                        result = new P_Import_CartonScanRate().P_CartonScanRate();
                        break;
                    case ListName.P_CartonStatusTrackingList:
                        result = new P_Import_CartonStatusTrackingList().P_CartonStatusTrackingList(item.SDate);
						break;
                    case ListName.P_FabricDispatchRate:
                        result = new P_Import_FabricDispatchRate().P_FabricDispatchRate(item.SDate);
                        break;
                    case ListName.P_IssueFabricByCuttingTransactionList:
                        result = new P_Import_IssueFabricByCuttingTransactionList().P_IssueFabricByCuttingTransactionList(item.SDate, item.EDate);
                        break;
                }
            }
            else
            {
                // Execute all Stored Procedures
                result = this.ExecuteSP(item);
            }

            DateTime? executeEDate = DateTime.Now;
            ExecutedList model = new ExecutedList
            {
                ClassName = item.ClassName,
                ProcedureName = item.ProcedureName,
                Success = result.Result,
                Group = item.Group,
                SEQ = item.SEQ,
                ErrorMsg = !result.Result ? result.Result.Messages.ToString() : string.Empty,
                ExecuteSDate = executeSDate,
                ExecuteEDate = executeEDate,
            };

            return model;
        }

        /// <summary>
        /// Update JobLog And Send FailMail
        /// </summary>
        /// <param name="executedList">ExecutedList</param>
        /// <param name="stratExecutedTime">Strat ExecutedTime</param>
        public void UpdateJobLogAndSendMail(List<ExecutedList> executedList, DateTime stratExecutedTime)
        {
            string region = this.GetRegion();
            string description = string.Join(Environment.NewLine, executedList.Select(x => $"[{x.ClassName}] is {(x.Success ? "completed " : "fail ")} Time: {x.ExecuteSDate.Value.ToString("yyyy/MM/dd HH:mm:ss")} - {x.ExecuteEDate.Value.ToString("yyyy/MM/dd HH:mm:ss")}。 {(x.Success ? string.Empty : Environment.NewLine + x.ErrorMsg)}"));
            bool nonSucceed = executedList.Where(x => !x.Success).Count() > 0;

            JobLog jobLog = new JobLog()
            {
                GroupID = "P",
                SystemID = "Power BI",
                Region = region,
                MDivisionID = string.Empty,
                OperationName = "Factory BI transfer",
                StartTime = stratExecutedTime.ToString("yyyy/MM/dd HH:mm:ss"),
                EndTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                Description = description,
                FileName = new List<string>(),
                FilePath = string.Empty,
                Succeeded = !nonSucceed,
            };

            string jobLogUkey = this.CallJobLogApi(jobLog);

            if (nonSucceed)
            {
                // Send Mail
                string mailTo = this.IsTest() ? "jack.hsu@sportscity.com.tw" : "pmshelp@sportscity.com.tw";
                string mailCC = string.Empty;
                string subject = "Import BI Data Error - Facotry";
                string content = $@"
Please check below information.
Transfer date: {DateTime.Now.ToString("yyyy/MM/dd")}
M: {region}
{description}
";
                var email = new MailTo(Env.Cfg.MailFrom, mailTo, mailCC, subject, string.Empty, content, false, true);
            }
        }

        /// <summary>
        /// only Execute Store Prcedure
        /// </summary>
        /// <param name="item">Executed List</param>
        /// <returns>Base ViewModel</returns>
        public Base_ViewModel ExecuteSP(ExecutedList item)
        {
            string procedureName = item.ProcedureName;
            if (item.SDate.HasValue)
            {
                procedureName = procedureName.Replace("{0}", item.SDate.Value.ToShortDateString());
            }

            if (item.EDate.HasValue)
            {
                procedureName = procedureName.Replace("{1}", item.EDate.Value.ToShortDateString());
            }

            if (item.SDate2.HasValue)
            {
                procedureName = procedureName.Replace("{2}", item.SDate2.Value.ToShortDateString());
            }

            if (item.EDate2.HasValue)
            {
                procedureName = procedureName.Replace("{3}", item.EDate2.Value.ToShortDateString());
            }

            Base_ViewModel resultReport = new Base_ViewModel();
            DBProxy.Current.OpenConnection(item.DBName, out SqlConnection sqlConn);
            using (sqlConn)
            {
                resultReport.Result = DBProxy.Current.ExecuteByConn(sqlConn, "exec " + procedureName);
            }

            return resultReport;
        }
    }
}

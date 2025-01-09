using Ict;
using PostJobLog;
using Sci.Data;
using Sci.Production.Prg.PowerBI.DataAccess;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Win.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading;

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
            P_QA_R31,
            P_QA_CFAMasterList,
            P_CFAMasterListRelatedrate,
            P_CartonScanRate,
            P_CartonStatusTrackingList,
            P_OutStandingHPMS,
            P_FabricDispatchRate,
            P_IssueFabricByCuttingTransactionList,
            P_ProductionKitsTracking,
            P_PPICMASTERLIST,
            P_FabricInspReport_ReceivingTransferIn,
            P_MtlStatusAnalisis,
            P_SubProInsReport,
            P_SubProInsReportDailyRate,
            P_SubProInsReportMonthlyRate,
            P_OustandingPO,
            P_OutstandingPOStatus,
            P_SubprocessWIP,
            P_RightFirstTimeDailyReport,
            P_SDP,
            P_BatchUpdateRecevingInfoTrackingList,
            P_WIP,
            P_WIPBySPLine,
            P_WBScanRate,
            P_CuttingBCS,
            P_FabricStatus_And_IssueFabricTracking,
            P_SimilarStyle,
            P_FabricInspLabSummaryReport,
            P_FabricInspAvgInspLTInPast7Days,
            P_CuttingOutputStatistic,
            P_MaterialCompletionRateByWeek,
            P_RTLStatusByDay,
            P_DailyRTLStatusByLineByStyle,
            P_InventoryStockListReport,
            P_RecevingInfoTrackingSummary,
            P_MachineMasterListByDays,
            P_ScanPackList,
            P_MISCPurchaseOrderList,
            P_ReplacementReport,
            P_DailyAccuCPULoading,
            P_SewingDailyOutputStatusRecord,
            P_LineBalancingRate,
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
        /// Get System Timeout
        /// </summary>
        /// <returns>double</returns>
        public double GetTimeout()
        {
            string sql = $"select BITimeout from System s";
            return Convert.ToDouble(MyUtility.GetValue.Lookup(sql, connectionName: "Production"));
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
    , b.TransferDate
FROM BITaskInfo i
LEFt JOIN BITaskGroup g ON i.[Name] = g.[Name]
LEFT JOIN BITableInfo b ON i.[Name] = b.[Id]
WHERE i.[Junk] = 0
AND (b.TransferDate <= CAST(GETDATE() AS DATE) OR b.TransferDate IS NULL)
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
                int group = (int)dr["Group"];
                int seq = (int)dr["SEQ"];
                DateTime? sDate = hasStartDate ? DateTime.Parse(this.GetSQLdate(dr["StartDateDefault"].ToString())) : (DateTime?)null;
                DateTime? eDate = hasEndDate ? DateTime.Parse(this.GetSQLdate(dr["EndDateDefault"].ToString())) : (DateTime?)null;
                DateTime? sDate2 = hasStartDate2 ? DateTime.Parse(this.GetSQLdate(dr["StartDateDefault2"].ToString())) : (DateTime?)null;
                DateTime? eDate2 = hasEndDate2 ? DateTime.Parse(this.GetSQLdate(dr["EndDateDefault2"].ToString())) : (DateTime?)null;
                string procedureNameS = !string.IsNullOrEmpty(procedureName) ? string.Concat("(", procedureName, ")") : string.Empty;
                string sDate_s = hasStartDate ? "Sdate : " + sDate.Value.ToShortDateString() : string.Empty;
                string eDate_s = hasEndDate ? "Edate : " + eDate.Value.ToShortDateString() : string.Empty;
                string sDate_s2 = hasStartDate2 ? "Sdate2 : " + sDate2.Value.ToShortDateString() : string.Empty;
                string eDate_s2 = hasEndDate2 ? "Edate2 : " + eDate2.Value.ToShortDateString() : string.Empty;
                string remark = $@"{dr["Source"]}{procedureNameS}{Environment.NewLine}{sDate_s}{Environment.NewLine}{eDate_s}{Environment.NewLine}{sDate_s2}{Environment.NewLine}{eDate_s2}{"Group :" + group}{", SEQ :" + seq}";
                string source = dr["Source"].ToString();

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
                    Source = source,
                };

                executes.Add(model);
            }

            var executesOrderBy = from o in executes
                                  orderby o.Group, o.SEQ, o.ClassName
                                  select o;
            return executesOrderBy.ToList();
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
            List<ExecutedList> executedListEnd = new List<ExecutedList>();
            List<ExecutedList> executedListException = new List<ExecutedList>();
            List<ExecutedList> executedListGroupLevelError = new List<ExecutedList>();
            var semaphore = new SemaphoreSlim(8); // 全局共享, 限制同時執行 8 筆
            double timeout = this.GetTimeout();
            var results = executedList
                .GroupBy(x => x.Group)
                .AsParallel()
                .Select(item =>
                {
                    var executedListDetail = new ConcurrentBag<ExecutedList>();
                    using (var groupCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeout * 0.8)))
                    {
                        try
                        {
                            var results_detail = item
                                .OrderBy(x => x.SEQ)
                                .AsParallel()
                                .WithCancellation(groupCancellationTokenSource.Token)
                                .AsSequential()
                                .Select(detail =>
                                {
                                    this.WriteTranslog(detail.ClassName, "begin to wait");
                                    semaphore.Wait(); // 等待取得執行許可
                                    try
                                    {
                                        this.WriteTranslog(detail.ClassName, "start");
                                        ExecutedList detailResult = this.ExecuteSingle(detail);
                                        this.WriteTranslog(detail.ClassName, "end");
                                        lock (executedListDetail)
                                        {
                                            executedListDetail.Add(detailResult);
                                        }

                                        return detailResult;
                                    }
                                    catch (Exception ex)
                                    {
                                        this.WriteTranslog(detail.ClassName, "error " + ex.Message);
                                        detail.Success = false;
                                        detail.ErrorMsg = ex.Message;
                                        lock (executedListException)
                                        {
                                            executedListException.Add(detail);
                                        }

                                        return detail;
                                    }
                                    finally
                                    {
                                        semaphore.Release(); // 釋放執行許可
                                    }
                                })
                                .TakeWhile(model => model.Group == 0 || model.Success) // 只保留成功的结果
                                .ToList();
                        }
                        catch (OperationCanceledException)
                        {
                            executedListGroupLevelError.Add(new ExecutedList
                            {
                                ErrorMsg = "GroupLevel_Timeout, Time out cancel",
                            });
                        }
                        catch (Exception ex)
                        {
                            executedListGroupLevelError.Add(new ExecutedList
                            {
                                ErrorMsg = "GroupLevel_Other, " + ex.Message,
                            });
                        }
                    }

                    return executedListDetail;
                })
                .ToList();

            foreach (var item in results)
            {
                executedListEnd.AddRange(item);
            }

            foreach (var item in executedList.Where(x => !executedListEnd.Any(y => y.ClassName == x.ClassName)))
            {
                var executedListError = new ExecutedList()
                {
                    ClassName = item.ClassName,
                    Group = item.Group,
                    SEQ = item.SEQ,
                    Success = false,
                    ExecuteSDate = DateTime.Now,
                    ExecuteEDate = DateTime.Now,
                    ErrorMsg = "沒有執行",
                };
                var executedGroupLevelError = executedListGroupLevelError.Where(currException => currException.ClassName == item.ClassName).FirstOrDefault();
                var executedException = executedListException.Where(currException => currException.ClassName == item.ClassName).FirstOrDefault();
                if (executedException != null)
                {
                    // 如果是Throw Exception的話，紀錄Exception訊息
                    executedListError.ErrorMsg = executedException.ErrorMsg;
                }
                else if (executedGroupLevelError != null)
                {
                    executedListError.ErrorMsg = executedGroupLevelError.ErrorMsg;
                }
                else
                {
                    // 只有Group不為0的錯誤訊息要Show前面的執行階層
                    var queryErrorGroup = item.Group == 0
                        ? executedListEnd.Where(x => x.ClassName == item.ClassName && x.Success == false)
                        : executedListEnd.Where(x => x.Group == item.Group && x.SEQ < item.SEQ && x.Success == false);
                    if (queryErrorGroup.Any())
                    {
                        executedListError.ErrorMsg = string.Join(",", queryErrorGroup.Select(x => x.ClassName)) + " 執行失敗";
                    }
                }

                executedListEnd.Add(executedListError);
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
                result = this.ExecuteByClassName(className, item);
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

        private Base_ViewModel ExecuteByClassName(ListName className, ExecutedList item)
        {
            switch (className)
            {
                case ListName.P_MonthlySewingOutputSummary:
                    return new P_Import_MonthlySewingOutputSummary().P_MonthlySewingOutputSummary(item.SDate, item.EDate);
                case ListName.P_SewingLineSchedule:
                    return new P_Import_SewingLineScheduleBIData().P_SewingLineScheduleBIData(item.SDate, item.EDate);
                case ListName.P_InlineDefectSummary:
                    return new P_Import_InlineDefec().P_InlineDefecBIData(item.SDate, item.EDate);
                case ListName.P_CuttingScheduleOutputList:
                    return new P_Import_CuttingScheduleOutputList().P_CuttingScheduleOutputList(item.SDate, item.EDate);
                case ListName.P_QA_R31:
                    return new P_Import_QAR31().P_QAR31(item.SDate, item.EDate);
                case ListName.P_QA_CFAMasterList:
                    return new P_Import_QA_CFAMasterList().P_QA_CFAMasterList(item.SDate);
                case ListName.P_CFAMasterListRelatedrate:
                    return new P_Import_CFAMasterListRelatedrate().P_CFAMasterListRelatedrate(item.SDate, item.EDate);
                case ListName.P_SewingLineScheduleBySP:
                    return new P_Import_SewingLineScheduleBySP().P_SewingLineScheduleBySP(item.SDate, item.EDate);
                case ListName.P_CartonScanRate:
                    return new P_Import_CartonScanRate().P_CartonScanRate(item.SDate, item.EDate);
                case ListName.P_CartonStatusTrackingList:
                    return new P_Import_CartonStatusTrackingList().P_CartonStatusTrackingList(item.SDate);
                case ListName.P_OutStandingHPMS:
                    return new P_Import_OutStandingHPMS().P_OutStandingHPMS(item.SDate);
                case ListName.P_FabricDispatchRate:
                    return new P_Import_FabricDispatchRate().P_FabricDispatchRate(item.SDate);
                case ListName.P_IssueFabricByCuttingTransactionList:
                    return new P_Import_IssueFabricByCuttingTransactionList().P_IssueFabricByCuttingTransactionList(item.SDate, item.EDate);
                case ListName.P_ProductionKitsTracking:
                    return new P_Import_ProductionKitsTracking().P_ProductionKitsTracking(item.SDate, item.EDate);
                case ListName.P_PPICMASTERLIST:
                    return new P_Import_PPICMasterListBIData().P_PPICMasterListBIData(item.SDate);
                case ListName.P_FabricInspReport_ReceivingTransferIn:
                    return new P_Import_FabricInspReportReceivingTransferIn().P_FabricInspReportReceivingTransferIn(item.SDate, item.EDate);
                case ListName.P_MtlStatusAnalisis:
                    return new P_Import_MtlStatusAnalisis().P_MtlStatusAnalisis(item.SDate, item.EDate);
                case ListName.P_BatchUpdateRecevingInfoTrackingList:
                    return new P_Import_BatchUpdateRecevingInfoTrackingList().P_BatchUpdateRecevingInfoTrackingList(item.SDate, item.EDate);
                case ListName.P_SubProInsReport:
                    return new P_Import_SubProInsReport().P_SubProInsReport(item.SDate, item.EDate);
                case ListName.P_SubProInsReportDailyRate:
                    return new P_Import_SubProInsReportDailyRate().P_SubProInsReportDailyRate(item.SDate, item.EDate);
                case ListName.P_SubProInsReportMonthlyRate:
                    return new P_Import_SubProInsReportMonthlyRate().P_SubProInsReportMonthlyRate(item.SDate, item.EDate);
                case ListName.P_OustandingPO:
                    return new P_Import_OutstandingPO().P_OutstandingPO(item.SDate, item.EDate);
                case ListName.P_OutstandingPOStatus:
                    return new P_Import_OutstandingPOStatus().P_OutstandingPOStatus(item.SDate);
                case ListName.P_SubprocessWIP:
                    return new P_Import_SubprocessWIP().P_SubprocessWIP(item.SDate);
                case ListName.P_RightFirstTimeDailyReport:
                    return new P_Import_RightFirstTimeDailyReport().P_RightFirstTimeDailyReport(item.SDate, item.EDate);
                case ListName.P_SDP:
                    return new P_Import_SDP().P_SDP(item.SDate, item.EDate);
                case ListName.P_WIP:
                    return new P_Import_WIP().P_WIP(item.SDate, item.EDate);
                case ListName.P_WBScanRate:
                    return new P_Import_WBScanRate().P_WBScanRate(item.SDate);
                case ListName.P_WIPBySPLine:
                    return new P_Import_WIPBySPLine().P_WIPBySPLine(item.SDate, item.EDate);
                case ListName.P_CuttingBCS:
                    return new P_Import_CuttingBCS().P_CuttingBCS(item.SDate, item.EDate);
                case ListName.P_FabricStatus_And_IssueFabricTracking:
                    return new P_Import_FabricStatusAndIssueFabricTracking().P_FabricStatusAndIssueFabricTracking(item.SDate);
                case ListName.P_SimilarStyle:
                    return new P_Import_SimilarStyle().P_SimilarStyle(item.SDate);
                case ListName.P_FabricInspLabSummaryReport:
                    return new P_Import_FabricInspLabSummaryReport().P_FabricInspLabSummaryReport(item.SDate, item.EDate);
                case ListName.P_FabricInspAvgInspLTInPast7Days:
                    return new P_Import_FabricInspAvgInspLTInPast7Days().P_FabricInspAvgInspLTInPast7Days(item.SDate, item.EDate);
                case ListName.P_CuttingOutputStatistic:
                    return new P_Import_CuttingOutputStatistic().P_CuttingOutputStatistic(item.SDate, item.EDate);
                case ListName.P_MaterialCompletionRateByWeek:
                    return new P_Import_MaterialCompletionRateByWeek().P_MaterialCompletionRateByWeek(item.SDate);
                case ListName.P_RTLStatusByDay:
                    return new P_Import_RTLStatusByDay().P_RTLStatusByDay(item.SDate);
                case ListName.P_DailyRTLStatusByLineByStyle:
                    return new P_Import_DailyRTLStatusByLineByStyle().P_DailyRTLStatusByLineByStyle(item.SDate);
                case ListName.P_InventoryStockListReport:
                    return new P_Import_InventoryStockListReport().P_InventoryStockListReport(item.SDate, item.EDate);
                case ListName.P_RecevingInfoTrackingSummary:
                    return new P_Import_RecevingInfoTrackingSummary().P_RecevingInfoTrackingSummary(item.SDate, item.EDate);
                case ListName.P_MachineMasterListByDays:
                    return new P_Import_MachineMasterListByDays().P_MachineMasterListByDays(item.SDate, item.EDate);
                case ListName.P_ScanPackList:
                    return new P_Import_ScanPackList().P_ScanPackListTransferIn(item.SDate, item.EDate);
                case ListName.P_MISCPurchaseOrderList:
                    return new P_Import_MISCPurchaseOrderList().P_MISCPurchaseOrderList(item.SDate, item.EDate);
                case ListName.P_ReplacementReport:
                    return new P_Import_ReplacementReport().P_ReplacementReport(item.SDate, item.EDate);
                case ListName.P_DailyAccuCPULoading:
                    return new P_Import_DailyAccuCPULoading().P_DailyAccuCPULoading(item.SDate, item.EDate);
                case ListName.P_SewingDailyOutputStatusRecord:
                    return new P_Import_DailyOutputStatusRecord().P_DailyOutputStatusRecord(item.SDate, item.EDate, ListName.P_SewingDailyOutputStatusRecord.ToString());
                case ListName.P_LineBalancingRate:
                    return new P_Import_LineBalancingRate().P_LineBalancingRate(item.SDate, item.EDate);
                default:
                    // Execute all Stored Procedures
                    return this.ExecuteSP(item);
            }
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

            // 取消寄信
            /*
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
            */
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
                DBProxy.Current.DefaultTimeout = MyUtility.Convert.GetInt(this.GetTimeout());
                resultReport.Result = DBProxy.Current.ExecuteByConn(sqlConn, "exec " + procedureName);
            }

            return resultReport;
        }

        /// <summary>
        /// BITableInfo
        /// </summary>
        /// <param name="bITableInfoID">BITableInfo.ID</param>
        /// <param name="is_Trans">是否會回台北. 0:不會 1:會</param>
        /// <returns>String</returns>
        public string SqlBITableInfo(string bITableInfoID, bool is_Trans)
        {
            return $@"
DECLARE @BITableInfoID VARCHAR(50) = '{bITableInfoID}'
DECLARE @IS_Trans BIT = '{is_Trans}'

IF EXISTS (SELECT 1 FROM BITableInfo WHERE Id = @BITableInfoID)
BEGIN
    UPDATE BITableInfo
    SET TransferDate = GETDATE()
       ,IS_Trans = @IS_Trans
    WHERE ID = @BITableInfoID
END
ELSE
BEGIN
    INSERT INTO BITableInfo (Id, TransferDate, IS_Trans)
        VALUES (@BITableInfoID, GETDATE(), @IS_Trans)
END
";
        }

        private void WriteTranslog(string tableName, string description)
        {
            string functionName = "PowerBI-" + tableName;
            string nowDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            List<SqlParameter> sqlParameter = new List<SqlParameter>() {
                new SqlParameter("@functionName", functionName),
                new SqlParameter("@description", description),
                new SqlParameter("@startTime", nowDate),
                new SqlParameter("@endTime", nowDate),
            };

            string sqlCmd =
                $@"
insert into TransLog([FunctionName], [Description], [StartTime], [EndTime])
values(@functionName, @description, @startTime, @endTime)
";
            DualResult result = DBProxy.Current.Execute("Production", sqlCmd, sqlParameter);
        }
    }
}

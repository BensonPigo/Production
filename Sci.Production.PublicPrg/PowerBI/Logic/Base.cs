using Ict;
using PostJobLog;
using Sci.Data;
using Sci.Production.Prg.PowerBI.DataAccess;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Transactions;
using System.Windows.Forms;
using System.Xml.Linq;

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
            P_ChangeoverCheckList,
            P_ESG_Injury,
            P_CMPByDate,
            P_LoadingProductionOutput,
            P_DQSDefect_Summary,
            P_DQSDefect_Detail,
            P_CFAInspectionRecord_Detail,
            P_QA_P09,
            P_QA_R06,
            P_SewingDailyOutput,
            P_LoadingvsCapacity,
            P_PPICMasterList_ArtworkType,
            P_ActualCutOutputReport,
            P_FabricInspDailyReport_Detail,
            P_ICRAnalysis,
            P_ImportScheduleList,
            P_AccessoryInspLabStatus,
            P_ProdEfficiencyByFactorySewingLine,
            P_AdiCompReport,
            P_Changeover,
            P_LineMapping,
            P_MaterialLocationIndex,
            P_MtltoFTYAnalysis,
            P_ProdEffAnalysis,
            P_StationHourlyOutput,
            P_StyleChangeover,
            P_ProdctionStatus,
            P_FabricPhysicalInspectionList,
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
        /// Check Class Name
        /// </summary>
        /// <param name="biTableName">BI TABLE NAME</param>
        /// <returns>bool</returns>
        public bool CheckClassName(string biTableName)
        {
            return Enum.TryParse(biTableName, true, out ListName _);
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
                bool runOnPM = (bool)dr["RunOnPM"];
                bool isTrans = (bool)dr["IsTrans"];
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
                DateTime? transferDate = !string.IsNullOrEmpty(dr["TransferDate"].ToString()) ? DateTime.Parse(dr["TransferDate"].ToString()) : (DateTime?)null;

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
                    TransferDate = transferDate,
                    RunOnPM = runOnPM,
                    RgCode = this.GetRegion(),
                    IsTrans = isTrans,
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

        // Pseudocode:
        // 1. 檢查清單中是否有 ClassName (若無則結束).
        // 2. 紀錄起始時間.
        // 3. 建立三個集合:
        //    - executedListEnd: 存放所有執行結果 (成功或失敗).
        //    - executedListException: 存放在過程中丟出異常的執行項目.
        //    - executedListGroupLevelError: 存放整個群組因逾時或其他原因中斷時的錯誤.
        // 4. 建立 SemaphoreSlim(8), 限制同時並行數量為 8.
        // 5. 以 .GroupBy(x => x.Group) 分組後, 各組照 x.SEQ 排序, 逐一平行執行流程, 除Group = 0之外
        //    5.1 為每組建立 Group-Level CancellationToken, 避免該組執行逾時.
        //    5.2 逐個執行項目時先 semaphore.Wait() 等待可用, 執行完畢後 semaphore.Release().
        //    5.3 使用 .TakeWhile() 保證只要上個項目發生失敗就中斷該組後續執行.
        // 6. 若整組逾時或發生例外, 將錯誤加入 executedListGroupLevelError.
        // 7. 蒐集所有執行結果放入 executedListEnd.
        // 8. 檢查哪些項目甚至未執行, 創建新的失敗資訊並補進 executedListEnd.
        // 9. 呼叫 this.UpdateJobLogAndSendMail(...) 寫入記錄及後續處理.

        /// <summary>
        /// Execute all BI List And Use Thread
        /// </summary>
        /// <param name="executedList">ExecutedList</param>
        public void ExecuteAll(List<ExecutedList> executedList)
        {
            if (executedList.All(x => string.IsNullOrEmpty(x.ClassName)))
            {
                return;
            }

            DateTime startExecutedTime = DateTime.Now;
            var executedListEnd = new List<ExecutedList>();
            var executedListException = new List<ExecutedList>();
            var executedListGroupLevelError = new List<ExecutedList>();
            var semaphore = new SemaphoreSlim(8);
            double timeout = this.GetTimeout();

            // 將執行結果封裝為方法，減少重複
            Func<IGrouping<int, ExecutedList>, IEnumerable<ExecutedList>> executeGroup = groupItem =>
            {
                if (groupItem is null)
                {
                    throw new ArgumentNullException(nameof(groupItem));
                }

                var executedListDetail = new ConcurrentBag<ExecutedList>();
                using (var groupCts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout * 0.8)))
                {
                    try
                    {
                        var orderedItems = groupItem.OrderBy(x => x.SEQ).ToList();
                        if (groupItem.Key == 0)
                        {
                            // Group 0: 全部平行
                            orderedItems
                                .AsParallel()
                                .WithCancellation(groupCts.Token)
                                .ForAll(detail =>
                                {
                                    semaphore.Wait();
                                    try
                                    {
                                        var detailResult = this.ExecuteSingle(detail);
                                        executedListDetail.Add(detailResult);
                                    }
                                    catch (Exception ex)
                                    {
                                        detail.Success = false;
                                        detail.ErrorMsg = ex.Message;
                                        lock (executedListException)
                                        {
                                            executedListException.Add(detail);
                                        }

                                        executedListDetail.Add(detail);
                                    }
                                    finally
                                    {
                                        semaphore.Release();
                                    }
                                });
                        }
                        else
                        {
                            // 其他群組: 順序執行，遇失敗中斷
                            foreach (var detail in orderedItems)
                            {
                                semaphore.Wait();
                                try
                                {
                                    var detailResult = this.ExecuteSingle(detail);
                                    executedListDetail.Add(detailResult);
                                    if (!detailResult.Success)
                                    {
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    detail.Success = false;
                                    detail.ErrorMsg = ex.Message;
                                    lock (executedListException)
                                    {
                                        executedListException.Add(detail);
                                    }

                                    executedListDetail.Add(detail);
                                    break;
                                }
                                finally
                                {
                                    semaphore.Release();
                                }
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        executedListGroupLevelError.Add(new ExecutedList
                        {
                            Group = groupItem.Key,
                            ErrorMsg = "GroupLevel_Timeout, Time out cancel",
                        });
                    }
                    catch (Exception ex)
                    {
                        executedListGroupLevelError.Add(new ExecutedList
                        {
                            Group = groupItem.Key,
                            ErrorMsg = "GroupLevel_Other, " + ex.Message,
                        });
                    }
                }
                return executedListDetail;
            };

            // 平行處理每個群組
            var results = executedList
                .GroupBy(x => x.Group)
                .AsParallel()
                .SelectMany(executeGroup)
                .ToList();

            executedListEnd.AddRange(results);

            // 處理未執行的項目
            var notExecuted = executedList.Where(x => !executedListEnd.Any(y => y.ClassName == x.ClassName));
            foreach (var item in notExecuted)
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
                var groupLevelErr = executedListGroupLevelError.FirstOrDefault(exGrp => exGrp.Group == item.Group);
                var executedException = executedListException.FirstOrDefault(exItem => exItem.ClassName == item.ClassName);

                if (executedException != null)
                {
                    executedListError.ErrorMsg = executedException.ErrorMsg;
                }
                else if (groupLevelErr != null)
                {
                    executedListError.ErrorMsg = groupLevelErr.ErrorMsg;
                }
                else
                {
                    var sameGroupErr = item.Group == 0
                        ? executedListEnd.Where(x => x.ClassName == item.ClassName && !x.Success)
                        : executedListEnd.Where(x => x.Group == item.Group && x.SEQ < item.SEQ && !x.Success);
                    if (sameGroupErr.Any())
                    {
                        executedListError.ErrorMsg = string.Join(",", sameGroupErr.Select(x => x.ClassName)) + " 執行失敗";
                    }
                }

                executedListEnd.Add(executedListError);
            }

            this.UpdateJobLogAndSendMail(executedListEnd, startExecutedTime);
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
                item.ExecuteSDate = executeSDate;
                result = this.ExecuteByClassName(className, item);
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
                    return new P_Import_MonthlySewingOutputSummary().P_MonthlySewingOutputSummary(item);
                case ListName.P_SewingLineSchedule:
                    return new P_Import_SewingLineScheduleBIData().P_SewingLineScheduleBIData(item);
                case ListName.P_InlineDefectSummary:
                    return new P_Import_InlineDefec().P_InlineDefecBIData(item);
                case ListName.P_CuttingScheduleOutputList:
                    return new P_Import_CuttingScheduleOutputList().P_CuttingScheduleOutputList(item);
                case ListName.P_QA_R31:
                    return new P_Import_QAR31().P_QAR31(item);
                case ListName.P_QA_CFAMasterList:
                    return new P_Import_QA_CFAMasterList().P_QA_CFAMasterList(item);
                case ListName.P_CFAMasterListRelatedrate:
                    return new P_Import_CFAMasterListRelatedrate().P_CFAMasterListRelatedrate(item);
                case ListName.P_SewingLineScheduleBySP:
                    return new P_Import_SewingLineScheduleBySP().P_SewingLineScheduleBySP(item);
                case ListName.P_CartonScanRate:
                    return new P_Import_CartonScanRate().P_CartonScanRate(item);
                case ListName.P_CartonStatusTrackingList:
                    return new P_Import_CartonStatusTrackingList().P_CartonStatusTrackingList(item);
                case ListName.P_OutStandingHPMS:
                    return new P_Import_OutStandingHPMS().P_OutStandingHPMS(item);
                case ListName.P_FabricDispatchRate:
                    return new P_Import_FabricDispatchRate().P_FabricDispatchRate(item);
                case ListName.P_IssueFabricByCuttingTransactionList:
                    return new P_Import_IssueFabricByCuttingTransactionList().P_IssueFabricByCuttingTransactionList(item);
                case ListName.P_ProductionKitsTracking:
                    return new P_Import_ProductionKitsTracking().P_ProductionKitsTracking(item);
                case ListName.P_PPICMASTERLIST:
                    return new P_Import_PPICMasterListBIData().P_PPICMasterListBIData(item);
                case ListName.P_FabricInspReport_ReceivingTransferIn:
                    return new P_Import_FabricInspReportReceivingTransferIn().P_FabricInspReportReceivingTransferIn(item);
                case ListName.P_MtlStatusAnalisis:
                    return new P_Import_MtlStatusAnalisis().P_MtlStatusAnalisis(item);
                case ListName.P_BatchUpdateRecevingInfoTrackingList:
                    return new P_Import_BatchUpdateRecevingInfoTrackingList().P_BatchUpdateRecevingInfoTrackingList(item);
                case ListName.P_SubProInsReport:
                    return new P_Import_SubProInsReport().P_SubProInsReport(item);
                case ListName.P_SubProInsReportDailyRate:
                    return new P_Import_SubProInsReportDailyRate().P_SubProInsReportDailyRate(item);
                case ListName.P_SubProInsReportMonthlyRate:
                    return new P_Import_SubProInsReportMonthlyRate().P_SubProInsReportMonthlyRate(item);
                case ListName.P_OustandingPO:
                    return new P_Import_OutstandingPO().P_OutstandingPO(item);
                case ListName.P_OutstandingPOStatus:
                    return new P_Import_OutstandingPOStatus().P_OutstandingPOStatus(item);
                case ListName.P_SubprocessWIP:
                    return new P_Import_SubprocessWIP().P_SubprocessWIP(item);
                case ListName.P_RightFirstTimeDailyReport:
                    return new P_Import_RightFirstTimeDailyReport().P_RightFirstTimeDailyReport(item);
                case ListName.P_SDP:
                    return new P_Import_SDP().P_SDP(item);
                case ListName.P_WIP:
                    return new P_Import_WIP().P_WIP(item);
                case ListName.P_WBScanRate:
                    return new P_Import_WBScanRate().P_WBScanRate(item);
                case ListName.P_WIPBySPLine:
                    return new P_Import_WIPBySPLine().P_WIPBySPLine(item);
                case ListName.P_CuttingBCS:
                    return new P_Import_CuttingBCS().P_CuttingBCS(item);
                case ListName.P_FabricStatus_And_IssueFabricTracking:
                    return new P_Import_FabricStatusAndIssueFabricTracking().P_FabricStatusAndIssueFabricTracking(item);
                case ListName.P_SimilarStyle:
                    return new P_Import_SimilarStyle().P_SimilarStyle(item);
                case ListName.P_FabricInspLabSummaryReport:
                    return new P_Import_FabricInspLabSummaryReport().P_FabricInspLabSummaryReport(item);
                case ListName.P_FabricInspAvgInspLTInPast7Days:
                    return new P_Import_FabricInspAvgInspLTInPast7Days().P_FabricInspAvgInspLTInPast7Days(item);
                case ListName.P_CuttingOutputStatistic:
                    return new P_Import_CuttingOutputStatistic().P_CuttingOutputStatistic(item);
                case ListName.P_MaterialCompletionRateByWeek:
                    return new P_Import_MaterialCompletionRateByWeek().P_MaterialCompletionRateByWeek(item);
                case ListName.P_RTLStatusByDay:
                    return new P_Import_RTLStatusByDay().P_RTLStatusByDay(item);
                case ListName.P_DailyRTLStatusByLineByStyle:
                    return new P_Import_DailyRTLStatusByLineByStyle().P_DailyRTLStatusByLineByStyle(item);
                case ListName.P_InventoryStockListReport:
                    return new P_Import_InventoryStockListReport().P_InventoryStockListReport(item);
                case ListName.P_RecevingInfoTrackingSummary:
                    return new P_Import_RecevingInfoTrackingSummary().P_RecevingInfoTrackingSummary(item);
                case ListName.P_MachineMasterListByDays:
                    return new P_Import_MachineMasterListByDays().P_MachineMasterListByDays(item);
                case ListName.P_ScanPackList:
                    return new P_Import_ScanPackList().P_ScanPackListTransferIn(item);
                case ListName.P_MISCPurchaseOrderList:
                    return new P_Import_MISCPurchaseOrderList().P_MISCPurchaseOrderList(item);
                case ListName.P_ReplacementReport:
                    return new P_Import_ReplacementReport().P_ReplacementReport(item);
                case ListName.P_DailyAccuCPULoading:
                    return new P_Import_DailyAccuCPULoading().P_DailyAccuCPULoading(item);
                case ListName.P_SewingDailyOutputStatusRecord:
                    return new P_Import_DailyOutputStatusRecord().P_DailyOutputStatusRecord(item);
                case ListName.P_LineBalancingRate:
                    return new P_Import_LineBalancingRate().P_LineBalancingRate(item);
                case ListName.P_ChangeoverCheckList:
                    return new P_Import_ChangeoverCheckList().P_ChangeoverCheckList(item);
                case ListName.P_ESG_Injury:
                    return new P_Import_ESG_Injury().P_ESG_Injury(item);
                case ListName.P_CMPByDate:
                    return new P_Import_CMPByDate().P_CMPByDate(item);
                case ListName.P_LoadingProductionOutput:
                    return new P_Import_LoadingProductionOutput().P_LoadingProductionOutput(item);
                case ListName.P_DQSDefect_Summary:
                    return new P_Import_DQSDefect_Summary().P_DQSDefect_Summary(item);
                case ListName.P_DQSDefect_Detail:
                    return new P_Import_DQSDefect_Detail().P_DQSDefect_Detail(item);
                case ListName.P_CFAInspectionRecord_Detail:
                    return new P_Import_CFAInspectionRecord_Detail().P_CFAInspectionRecord_Detail(item);
                case ListName.P_QA_P09:
                    return new P_Import_QA_P09().P_QA_P09(item);
                case ListName.P_QA_R06:
                    return new P_Import_QA_R06().P_QA_R06(item);
                case ListName.P_SewingDailyOutput:
                    return new P_Import_SewingDailyOutput().P_SewingDailyOutput(item);
                case ListName.P_LoadingvsCapacity:
                    return new P_Import_LoadingvsCapacity().P_LoadingvsCapacity(item);
                case ListName.P_PPICMasterList_ArtworkType:
                    return new P_Import_PPICMasterList_ArtworkType().P_PPICMasterList_ArtworkType(item);
                case ListName.P_ActualCutOutputReport:
                    return new P_Import_ActualCutOutputReport().P_ActualCutOutputReport(item);
                case ListName.P_FabricInspDailyReport_Detail:
                    return new P_Import_FabricInspDailyReport_Detail().P_FabricInspDailyReport_Detail(item);
                case ListName.P_ICRAnalysis:
                    return new P_Import_ICRAnalysis().P_ICRAnalysis(item);
                case ListName.P_ImportScheduleList:
                    return new P_Import_ImportScheduleList().P_ImportScheduleList(item);
                case ListName.P_AccessoryInspLabStatus:
                    return new P_Import_AccessoryInspLabStatus().P_AccessoryInspLabStatus(item);
                case ListName.P_ProdEfficiencyByFactorySewingLine:
                    return new P_Import_ProdEfficiencyByFactorySewingLine().P_ProdEfficiencyByFactorySewingLine(item);
                case ListName.P_AdiCompReport:
                    return new P_Import_AdiCompReport().P_AdiCompReport(item);
                case ListName.P_Changeover:
                    return new P_Import_Changeover().P_Changeover(item);
                case ListName.P_LineMapping:
                    return new P_Import_LineMapping().P_LineMapping(item);
                case ListName.P_MaterialLocationIndex:
                    return new P_Import_MaterialLocationIndex().P_MaterialLocationIndex(item);
                case ListName.P_MtltoFTYAnalysis:
                    return new P_Import_MtltoFTYAnalysis().P_IMtltoFTYAnalysis(item);
                case ListName.P_ProdEffAnalysis:
                    return new P_Import_ProdEffAnalysis().P_ProdEffAnalysis(item);
                case ListName.P_StationHourlyOutput:
                    return new P_Import_StationHourlyOutput().P_StationHourlyOutput(item);
                case ListName.P_StyleChangeover:
                    return new P_Import_StyleChangeover().P_StyleChangeover(item);
                case ListName.P_ProdctionStatus:
                    return new P_Import_ProductionStatus().P_ProductionStatus(item);
                case ListName.P_FabricPhysicalInspectionList:
                    return new P_Import_FabricPhysicalInspectionList().P_FabricPhysicalInspectionList(item);
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
            this.WriteTranslog("JobLog", "start");
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
            this.WriteTranslog("JobLog", "end " + jobLogUkey);
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
        /// <param name="item">Executed List</param>
        /// <param name="is_Continuous">是否是接續使用</param>
        /// <returns>String</returns>
        public string SqlBITableInfo(ExecutedList item, bool is_Continuous = false)
        {
            string strDeclare = is_Continuous ? $@"SET @BITableInfoID = '{item.ClassName}'; SET  @IS_Trans = '{item.IsTrans}';" : $@"DECLARE @BITableInfoID VARCHAR(50) = '{item.ClassName}'; DECLARE @IS_Trans BIT = '{item.IsTrans}'";
            return $@"
            {strDeclare}
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

        /// <summary>
        /// 寫入 DML LOG
        /// </summary>
        /// <param name="item">是否回台北</param>
        /// <returns>SQL</returns>
        public string SqlInsertDmlLog(ExecutedList item)
        {
            string sqlCmd = string.Empty;
            if (!item.IsTrans)
            {
                return sqlCmd;
            }

            sqlCmd = $@"
exec Insert_DmlLog '{item.ClassName}', '{item.ExecuteSDate.Value.ToString("yyyy/MM/dd HH:mm:ss")}', 0
exec Insert_DmlLog '{item.ClassName}', '{item.ExecuteSDate.Value.ToString("yyyy/MM/dd HH:mm:ss")}', 1
";
            return sqlCmd;
        }

        /// <summary>
        /// Generates a SQL query to insert data into a BI table history.
        /// </summary>
        /// <param name="tableName">The name of the source table.</param>
        /// <param name="tableName_History">The name of the history table.</param>
        /// <param name="tmpTableName">The name of the temporary table used for comparison.</param>
        /// <param name="strWhere">The WHERE clause to filter the data.</param>
        /// <param name="needJoin">need join BIFactoryID</param>
        /// <param name="needExists">need exists in Table</param>
        /// <returns>A SQL query string.</returns>
        public string SqlBITableHistory(string tableName, string tableName_History, string tmpTableName, string strWhere = "", bool needJoin = true, bool needExists = true)
        {
            DataTable dt = new DataTable();
            string tableColumns = string.Empty;
            string tableColumns_History = string.Empty;
            string tmpColumns = string.Empty;

            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            #region 抓取欄位
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, 3600)))
            {
                string sqlcmd = $@"  
                SELECT  
                b.COLUMN_NAME  
                FROM  
                INFORMATION_SCHEMA.TABLES a  
                LEFT JOIN INFORMATION_SCHEMA.COLUMNS b ON (a.TABLE_NAME = b.TABLE_NAME)  
                WHERE  
                a.TABLE_NAME = '{tableName_History}'  
                AND b.COLUMN_NAME NOT IN (  
                    SELECT c.COLUMN_NAME  
                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc  
                    JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu ON tc.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME  
                    JOIN INFORMATION_SCHEMA.COLUMNS c ON c.TABLE_NAME = tc.TABLE_NAME AND c.COLUMN_NAME = ccu.COLUMN_NAME  
                    WHERE tc.TABLE_NAME = '{tableName_History}'  
                    AND tc.CONSTRAINT_TYPE = 'PRIMARY KEY'  
                )  
                AND b.COLUMN_NAME NOT IN ('BIFactoryID', 'BIInsertDate', 'BIStatus')  
                AND TABLE_TYPE = 'BASE TABLE'";
                DBProxy.Current.SelectByConn(sqlConn, sqlcmd, out dt);

                transactionScope.Complete();
                transactionScope.Dispose();
            }
            #endregion 抓取欄位

            #region 關聯建置
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                string columnName = row["COLUMN_NAME"].ToString();

                tableColumns_History += "[" + columnName + "]";
                tableColumns += "p." + "[" + columnName + "]";

                if (i == 0)
                {
                    tmpColumns += " t." + "[" + columnName + "]" + " = p." + "[" + columnName + "]" ;
                }
                else
                {
                    tmpColumns += " and t." + "[" + columnName + "]" + " = p." + "[" + columnName + "]";
                }

                if (i != dt.Rows.Count - 1)
                {
                    tableColumns_History += ",";
                    tableColumns += ",";
                }
                else
                {
                    tableColumns_History += " ";
                    tableColumns += " ";
                }
            }
            #endregion 關聯建置

            return $@" 
            if @IsTrans = 1
            begin
              INSERT INTO {tableName_History}  
              (  
                  {tableColumns_History},   
                  BIFactoryID,   
                  BIInsertDate  
              )   
              SELECT   
              {tableColumns},   
              {(needJoin ? " t.BIFactoryID," : "p.BIFactoryID,")}
              GETDATE()  
              FROM {tableName} p  
              {(needJoin ? $"INNER JOIN {tmpTableName} t ON {tmpColumns} " : string.Empty)}
              WHERE {(needExists ? $" not exists( Select 1 from {tmpTableName} t where {tmpColumns})" : "1 = 1")} 
              {(string.IsNullOrEmpty(strWhere) ? string.Empty : " and " + strWhere)}
            end";
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

        /// <summary>
        /// Update BI Data
        /// </summary>
        /// <param name="item">Executed List</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel UpdateBIData(ExecutedList item)
        {
            string sql = this.SqlBITableInfo(item); // + this.SqlInsertDmlLog(item); 改到佇列端判斷BIStatus執行
            return new Base_ViewModel()
            {
                Result = TransactionClass.ExecuteTransactionScope("PowerBI", sql),
            };
        }
    }
}

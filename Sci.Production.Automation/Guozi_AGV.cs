using Ict;
using Newtonsoft.Json;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sci.Production.Automation.UtilityAutomation;

namespace Sci.Production.Automation
{
    public class Guozi_AGV
    {
        private static string baseUrl = string.Empty;
        private string guoziSuppID = "SCI";
        private string moduleName = "SCI";
        private AutomationErrMsgPMS automationErrMsg;

        public Guozi_AGV()
        {
            this.automationErrMsg = new AutomationErrMsgPMS()
            {
                suppID = this.guoziSuppID,
                moduleName = this.moduleName
            };
        }

        public string BaseUrl
        {
            get
            {
                if (MyUtility.Check.Empty(baseUrl))
                {
                    baseUrl = this.GetBaseUrl();
                }

                return baseUrl;
            }
        }


        private string GetBaseUrl()
        {
            return MyUtility.GetValue.Lookup($"select URL from WebApiURL with (nolock) where SuppID = '{this.guoziSuppID}' and ModuleName = '{this.moduleName}' ");
        }

        /// <summary>
        /// SentWorkOrderToAGV
        /// </summary>
        /// <param name="dtWorkOrder">dtWorkOrder</param>
        public void SentWorkOrderToAGV(DataTable dtWorkOrder)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            List<WorkOrderToAGV_PostBody> listWorkOrder = new List<WorkOrderToAGV_PostBody>();
            DataTable dtWorkOrder_Distribute;
            string apiThread = "SentWorkOrderToAGV";
            string suppAPIThread = "api/GuoziAGV/SentDataByApiTag";
            string sqlGetData;
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            foreach (DataRow dr in dtWorkOrder.Rows)
            {
                sqlGetData = $"select WorkOrderUkey,OrderID,Article,SizeCode,Qty from WorkOrder_Distribute with (nolock) where WorkOrderUkey = {dr["Ukey"]}";
                DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtWorkOrder_Distribute);
                if (!result)
                {
                    this.automationErrMsg.SetErrInfo(result);
                    UtilityAutomation.SaveAutomationErrMsg(this.automationErrMsg);
                }

                listWorkOrder.Add(
                    new WorkOrderToAGV_PostBody()
                    {
                        WorkOrder_Distribute = dtWorkOrder_Distribute,
                        Ukey = (long)dr["Ukey"],
                        CutRef = dr["CutRef"].ToString(),
                        EstCutDate = (DateTime?)dr["EstCutDate"],
                        ID = dr["ID"].ToString(),
                        OrderID = dr["OrderID"].ToString(),
                        CutCellID = dr["CutCellID"].ToString()
                    });
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject.WorkOrder = listWorkOrder;

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "WorkOrder"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentBundleToAGV
        /// </summary>
        /// <param name="dtBundle">dtBundle</param>
        public void SentBundleToAGV(Func<List<BundleToAGV_PostBody>> funListBundle)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            DataTable dtBundle_SubProcess;
            string apiThread = "SentBundleToAGV";
            string suppAPIThread = "api/GuoziAGV/SentDataByApiTag";
            string sqlGetData;
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;
            List<BundleToAGV_PostBody> impListBundle;
            try
            {
                impListBundle = funListBundle();
            }
            catch (Exception ex)
            {
                this.automationErrMsg.SetErrInfo(new DualResult(false, ex));
                SaveAutomationErrMsg(this.automationErrMsg);
                return;
            }

            if (impListBundle == null)
            {
                return;
            }

            foreach (BundleToAGV_PostBody bundle in impListBundle)
            {
                sqlGetData = $@"
select  bda.Ukey,
        bda.BundleNo,
        bda.SubProcessID,
        [Seq] = isnull(Seq.Value, s.Seq),
        bda.PostSewingSubProcess,
        bda.NoBundleCardAfterSubprocess
from    Bundle_Detail_Art bda with (nolock)
left join SubProcess s with (nolock) on bda.SubProcessID = s.ID
outer apply( select [Value] = spd.Seq
            from orders o  with (nolock)
            inner join SubProcessSeq_Detail spd with (nolock) on o.StyleUkey = spd.StyleUkey and spd.SubProcessID = bda.SubProcessID
            where o.ID = '{bundle.OrderID}') Seq
where bda.BundleNo = '{bundle.BundleNo}'";
                DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtBundle_SubProcess);
                if (!result)
                {
                    this.automationErrMsg.SetErrInfo(result);
                    SaveAutomationErrMsg(this.automationErrMsg);
                }

                bundle.Bundle_SubProcess = dtBundle_SubProcess;
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject.Bundle = impListBundle;

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "Bundle"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentSubprocessToAGV
        /// </summary>
        /// <param name="dtSubprocess">dtSubprocess</param>
        public void SentSubprocessToAGV(DataTable dtSubprocess)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            string apiThread = "SentSubprocessToAGV";
            string suppAPIThread = "api/GuoziAGV/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.SubProcess = dtSubprocess.AsEnumerable()
                .Select(s => new
                {
                    ID = s["ID"].ToString(),
                    Junk = s["Junk"] == DBNull.Value ? false : (bool)s["Junk"]
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "SubProcess"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentCutCellToAGV
        /// </summary>
        /// <param name="dtCutCell">dtCutCell</param>
        public void SentCutCellToAGV(DataTable dtCutCell)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            string apiThread = "SentCutCellToAGV";
            string suppAPIThread = "api/GuoziAGV/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.CutCell = dtCutCell.AsEnumerable()
                .Select(s => new
                {
                    ID = s["ID"].ToString(),
                    Junk = s["Junk"] == DBNull.Value ? false : (bool)s["Junk"]
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "CutCell"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentSewingLineToAGV
        /// </summary>
        /// <param name="dtSewingLine">dtSewingLine</param>
        public void SentSewingLineToAGV(DataTable dtSewingLine)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            string apiThread = "SentSewingLineToAGV";
            string suppAPIThread = "api/GuoziAGV/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.SewingLine = dtSewingLine.AsEnumerable()
                .Select(s => new
                {
                    ID = s["ID"].ToString(),
                    Junk = s["Junk"] == DBNull.Value ? false : (bool)s["Junk"]
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "SewingLine"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentSewingScheduleToAGV
        /// </summary>
        /// <param name="dtSewingSchedule">dtSewingSchedule</param>
        public void SentSewingScheduleToAGV(DataTable dtSewingSchedule)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            string apiThread = "SentSewingScheduleToAGV";
            string suppAPIThread = "api/GuoziAGV/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.SewingSchedule = dtSewingSchedule.AsEnumerable()
                .Select(s => new
                {
                    ID = (long)s["ID"],
                    OrderID = s["OrderID"].ToString(),
                    SewingLineID = s["SewingLineID"].ToString(),
                    Inline = (DateTime?)s["Inline"],
                    Offline = (DateTime?)s["Offline"],
                    StdOutput = (int)s["StdOutput"]
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "SewingSchedule"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentDeleteWorkOrder
        /// </summary>
        /// <param name="dtWorkOrder">dtSewingSchedule</param>
        public void SentDeleteWorkOrder(List<long> dtWorkOrder)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            if (dtWorkOrder.Count == 0)
            {
                return;
            }

            string apiThread = "SentDeleteWorkOrderFromAGV";
            string suppAPIThread = "api/GuoziAGV/SentDeleteDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.WorkOrder = dtWorkOrder.Select(s => new
            {
                Ukey = s
            });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "WorkOrder"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentDeleteWorkOrder_Distribute
        /// </summary>
        /// <param name="dtWorkOrder_Distribute">dtSewingSchedule</param>
        public void SentDeleteWorkOrder_Distribute(List<WorkOrder_Distribute> deleteWorkOrder_Distribute)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            if (deleteWorkOrder_Distribute.Count == 0)
            {
                return;
            }

            string apiThread = "SentDeleteWorkOrderFromAGV";
            string suppAPIThread = "api/GuoziAGV/SentDeleteDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.WorkOrder_Distribute = deleteWorkOrder_Distribute;

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "WorkOrder_Distribute"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        public class WorkOrder_Distribute
        {
            public long WorkOrderUkey;
            public string OrderID;
            public string Article;
            public string SizeCode;
        }

        /// <summary>
        /// SentDeleteBundle
        /// </summary>
        /// <param name="dtBundle">dtSewingSchedule</param>
        public void SentDeleteBundle(DataTable dtBundle)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            string apiThread = "SentDeleteBundleFromAGV";
            string suppAPIThread = "api/GuoziAGV/SentDeleteDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.Bundle = dtBundle.AsEnumerable()
                .Select(s => new
                {
                    BundleNo = (string)s["BundleNo"]
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "Bundle"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentDeleteBundle_SubProcess
        /// </summary>
        /// <param name="dtBundle_SubProcess">dtSewingSchedule</param>
        public void SentDeleteBundle_SubProcess(DataTable dtBundle_SubProcess)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            string apiThread = "SentDeleteBundle_SubProcessFromAGV";
            string suppAPIThread = "api/GuoziAGV/SentDeleteDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.Bundle_SubProcess = dtBundle_SubProcess.AsEnumerable()
                .Select(s => new
                {
                    Ukey = (long)s["Ukey"]
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "Bundle_SubProcess"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentDeleteSewingSchedule
        /// </summary>
        /// <param name="dtSewingSchedule">dtSewingSchedule</param>
        public void SentDeleteSewingSchedule(DataTable dtSewingSchedule)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.guoziSuppID))
            {
                return;
            }

            string apiThread = "SentDeleteSewingScheduleFromAGV";
            string suppAPIThread = "api/GuoziAGV/SentDeleteDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.SewingSchedule = dtSewingSchedule.AsEnumerable()
                .Select(s => new
                {
                    ID = (long)s["ID"]
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "SewingSchedule"));

            SendWebAPI(this.BaseUrl, suppAPIThread, jsonBody, this.automationErrMsg);
        }

        private class WorkOrderToAGV_PostBody
        {
            public DataTable WorkOrder_Distribute;
            public long Ukey;
            public string CutRef;
            public DateTime? EstCutDate;
            public string ID;
            public string OrderID;
            public string CutCellID;
        }

        public class BundleToAGV_PostBody
        {
            public DataTable Bundle_SubProcess;
            public string ID;
            public string BundleNo;
            public string CutRef;
            public string OrderID;
            public string Article;
            public string PatternPanel;
            public string FabricPanelCode;
            public string PatternCode;
            public string PatternDesc;
            public decimal BundleGroup;
            public string SizeCode;
            public decimal Qty;
            public string SewingLineID;
            public DateTime? AddDate;
        }
    }
}

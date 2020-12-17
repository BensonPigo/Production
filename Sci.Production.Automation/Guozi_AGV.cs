using Ict;
using Newtonsoft.Json;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using static Sci.Production.Automation.UtilityAutomation;

namespace Sci.Production.Automation
{
    /// <inheritdoc/>
    public class Guozi_AGV
    {
        private readonly string guoziSuppID = "3A0197";
        private readonly string moduleName = "AGV";
        private AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS();

        /// <summary>
        /// SentWorkOrderToAGV
        /// </summary>
        /// <param name="dtWorkOrder">dtWorkOrder</param>
        public void SentWorkOrderToAGV(DataTable dtWorkOrder)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
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
                        CutCellID = dr["CutCellID"].ToString(),
                    });
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject.WorkOrder = listWorkOrder;

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "WorkOrder"));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentBundleToAGV
        /// </summary>
        /// <param name="funListBundle">List BundleToAGV_PostBody</param>
        public void SentBundleToAGV(Func<List<BundleToAGV_PostBody>> funListBundle)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
            {
                return;
            }

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
where bda.BundleNo = '{bundle.BundleNo}'

select  Ukey,
        BundleNo,
        OrderID,
        Qty
from    Bundle_Detail_Order with (nolock)
where   BundleNo = '{bundle.BundleNo}'

";
                DualResult result = DBProxy.Current.Select(null, sqlGetData, out DataTable[] dtResults);

                if (!result)
                {
                    this.automationErrMsg.SetErrInfo(result);
                    SaveAutomationErrMsg(this.automationErrMsg);
                }

                bundle.Bundle_SubProcess = dtResults[0];
                bundle.Bundle_Order = dtResults[1];
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject.Bundle = impListBundle.Select(s => new
            {
                s.Bundle_SubProcess,
                s.Bundle_Order,
                s.ID,
                s.POID,
                s.BundleNo,
                s.CutRef,
                s.Article,
                s.PatternPanel,
                s.FabricPanelCode,
                s.PatternCode,
                s.PatternDesc,
                s.BundleGroup,
                s.SizeCode,
                s.SewingLineID,
                s.AddDate,
            });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "Bundle"));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentSubprocessToAGV
        /// </summary>
        /// <param name="dtSubprocess">dtSubprocess</param>
        public void SentSubprocessToAGV(DataTable dtSubprocess)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
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
                    Junk = s["Junk"] == DBNull.Value ? false : (bool)s["Junk"],
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "SubProcess"));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentOrdersToAGV
        /// </summary>
        /// <param name="listPOID">listPOID</param>
        public void SentOrdersToAGV(List<string> listPOID)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
            {
                return;
            }

            if (listPOID.Count == 0)
            {
                return;
            }

            string apiThread = "SentOrdersToAGV";
            string suppAPIThread = "api/GuoziAGV/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            string wherePOID = listPOID.Select(s => $"'{s}'").JoinToString(",");
            string sqlGetOrders = $@"
select  distinct ID
from Orders with (nolock) where POID in ({wherePOID})
";
            DataTable dtSentOrderID;
            DualResult result = DBProxy.Current.Select(null, sqlGetOrders, out dtSentOrderID);
            if (!result)
            {
                throw result.GetException();
            }

            bodyObject.ID = dtSentOrderID.AsEnumerable().Select(s => s["ID"].ToString()).ToArray();
            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "Orders"));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentCutCellToAGV
        /// </summary>
        /// <param name="dtCutCell">dtCutCell</param>
        public void SentCutCellToAGV(DataTable dtCutCell)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
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
                    Junk = s["Junk"] == DBNull.Value ? false : (bool)s["Junk"],
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "CutCell"));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentSewingLineToAGV
        /// </summary>
        /// <param name="dtSewingLine">dtSewingLine</param>
        public void SentSewingLineToAGV(DataTable dtSewingLine)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
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
                    Junk = s["Junk"] == DBNull.Value ? false : (bool)s["Junk"],
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "SewingLine"));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentSewingScheduleToAGV
        /// </summary>
        /// <param name="dtSewingSchedule">dtSewingSchedule</param>
        public void SentSewingScheduleToAGV(DataTable dtSewingSchedule)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
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
                    StdOutput = (int)s["StdOutput"],
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "SewingSchedule"));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentDeleteWorkOrder
        /// </summary>
        /// <param name="dtWorkOrder">dtSewingSchedule</param>
        public void SentDeleteWorkOrder(List<long> dtWorkOrder)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
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
                Ukey = s,
            });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "WorkOrder"));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentDeleteWorkOrder_Distribute
        /// </summary>
        /// <param name="deleteWorkOrder_Distribute">List WorkOrder_Distribute</param>
        public void SentDeleteWorkOrder_Distribute(List<WorkOrder_Distribute> deleteWorkOrder_Distribute)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
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

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// WorkOrder Distribute
        /// </summary>
        public class WorkOrder_Distribute
        {
            /// <summary>
            /// WorkOrder Ukey
            /// </summary>
            public long WorkOrderUkey { get; set; }

            /// <summary>
            /// Order ID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// Article
            /// </summary>
            public string Article { get; set; }

            /// <summary>
            /// SizeCode
            /// </summary>
            public string SizeCode { get; set; }
        }

        /// <summary>
        /// SentDeleteBundle
        /// </summary>
        /// <param name="dtBundle">dtSewingSchedule</param>
        /// <returns>DualResult</returns>
        public DualResult SentDeleteBundle(DataTable dtBundle)
        {
            DualResult result;
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
            {
                return new DualResult(true);
            }

            string apiThread = "SentDeleteBundleFromAGV";
            string suppAPIThread = "api/GuoziAGV/SentDeleteDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.Bundle = dtBundle.AsEnumerable()
                .Select(s => new
                {
                    BundleNo = (string)s["BundleNo"],
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "Bundle"));

            result = SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
            return result;
        }

        /// <summary>
        /// SentDeleteBundle_SubProcess
        /// </summary>
        /// <param name="dtBundle_SubProcess">dtSewingSchedule</param>
        /// <returns>DualResult</returns>
        public DualResult SentDeleteBundle_SubProcess(DataTable dtBundle_SubProcess)
        {
            DualResult result;
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
            {
                return new DualResult(true);
            }

            string apiThread = "SentDeleteBundle_SubProcessFromAGV";
            string suppAPIThread = "api/GuoziAGV/SentDeleteDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.Bundle_SubProcess = dtBundle_SubProcess.AsEnumerable()
                .Select(s => new
                {
                    Ukey = (long)s["Ukey"],
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "Bundle_SubProcess"));

            result = SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
            return result;
        }

        /// <summary>
        /// SentDeleteBundle_Detail_Order
        /// </summary>
        /// <param name="dtBundle_Detail_Order">dtSewingSchedule</param>
        /// <returns>DualResult</returns>
        public DualResult SentDeleteBundle_Detail_Order(DataTable dtBundle_Detail_Order)
        {
            DualResult result;
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
            {
                return new DualResult(true);
            }

            string apiThread = "SentDeleteBundle_Detail_OrderFromAGV";
            string suppAPIThread = "api/GuoziAGV/SentDeleteDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.Bundle_Order = dtBundle_Detail_Order.AsEnumerable()
                .Select(s => new
                {
                    Ukey = (long)s["Ukey"],
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "Bundle_Order"));

            result = SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
            return result;
        }

        /// <summary>
        /// SentDeleteSewingSchedule
        /// </summary>
        /// <param name="dtSewingSchedule">dtSewingSchedule</param>
        public void SentDeleteSewingSchedule(DataTable dtSewingSchedule)
        {
            if (!IsModuleAutomationEnable(this.guoziSuppID, this.moduleName))
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
                    ID = (long)s["ID"],
                });

            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "SewingSchedule"));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        private class WorkOrderToAGV_PostBody
        {
            public DataTable WorkOrder_Distribute { get; set; }

            public long Ukey { get; set; }

            public string CutRef { get; set; }

            public DateTime? EstCutDate { get; set; }

            public string ID { get; set; }

            public string OrderID { get; set; }

            public string CutCellID { get; set; }
        }

        /// <summary>
        /// BundleToAGV PostBody
        /// </summary>
        public class BundleToAGV_PostBody
        {
            /// <summary>
            /// Bundle SubProcess
            /// </summary>
            public DataTable Bundle_SubProcess { get; set; }

            /// <summary>
            /// Bundle_Detail_Order
            /// </summary>
            public DataTable Bundle_Order { get; set; }

            /// <summary>
            /// Bundle ID
            /// </summary>
            public string ID { get; set; }

            /// <summary>
            /// POID
            /// </summary>
            public string POID { get; set; }

            /// <summary>
            /// Bundle No
            /// </summary>
            public string BundleNo { get; set; }

            /// <summary>
            /// CutRef
            /// </summary>
            public string CutRef { get; set; }

            /// <summary>
            /// Order ID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// Article
            /// </summary>
            public string Article { get; set; }

            /// <summary>
            /// PatternPanel
            /// </summary>
            public string PatternPanel { get; set; }

            /// <summary>
            /// FabricPanel Code
            /// </summary>
            public string FabricPanelCode { get; set; }

            /// <summary>
            /// PatternCode
            /// </summary>
            public string PatternCode { get; set; }

            /// <summary>
            /// PatternDesc
            /// </summary>
            public string PatternDesc { get; set; }

            /// <summary>
            /// Bundle Group
            /// </summary>
            public decimal BundleGroup { get; set; }

            /// <summary>
            /// SizeCode
            /// </summary>
            public string SizeCode { get; set; }

            /// <summary>
            /// Bundle Qty
            /// </summary>
            public decimal Qty { get; set; }

            /// <summary>
            /// SewingLine ID
            /// </summary>
            public string SewingLineID { get; set; }

            /// <summary>
            /// AddDate
            /// </summary>
            public DateTime? AddDate { get; set; }
        }
    }
}

using Ict;
using Newtonsoft.Json;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using static PmsWebApiUtility20.WebApiTool;
using static Sci.Production.Automation.UtilityAutomation;

namespace Sci.Production.Automation
{
    /// <inheritdoc/>
    public class Gensong_SpreadingSchedule
    {
        private static readonly string GensongSuppID = "3A0174";
        private static readonly string moduleName = "SpreadingSchedule";
        private AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS();

        /// <summary>
        /// SendSpreadingSchedule
        /// </summary>
        /// <param name="factoryID">factoryID</param>
        /// <param name="estCutDate">estCutDate</param>
        /// <param name="cutCellID">cutCellID</param>
        public DualResult SendSpreadingSchedule(string factoryID, DateTime estCutDate, string cutCellID)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return new DualResult(true);
            }

            string apiThread = "SendSpreadingSchedule";
            string suppAPIThread = "api/GensongSpreadingSchedule/SendSpreadingSchedule";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            var postBody = new
            {
                FactoryID = factoryID,
                EstCutDate = estCutDate.ToString("yyyy-MM-dd"),
                CutCellID = cutCellID,
            };

            string jsonBody = JsonConvert.SerializeObject(postBody);

            DualResult result = new DualResult(true);
            WebApiBaseResult webApiBaseResult;
            webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, 600);

            if (!webApiBaseResult.isSuccess)
            {
                string errMsg = MyUtility.Check.Empty(webApiBaseResult.responseContent) ? webApiBaseResult.exception.ToString() : webApiBaseResult.responseContent;
                return new DualResult(false, new Ict.BaseResult.MessageInfo(errMsg));
            }

            return new DualResult(true);
        }

        /// <summary>
        /// GetInventory
        /// </summary>
        /// <param name="factoryID">factoryID</param>
        /// <param name="estCutDate">estCutDate</param>
        /// <param name="cutCellID">cutCellID</param>
        /// <returns>InventorySendSpreadingSchedule</returns>
        public DualResult GetInventory(string factoryID, DateTime estCutDate, string cutCellID, out InventorySpreadingSchedule inventorySpreadingSchedule)
        {
            string baseUrl = UtilityAutomation.GetSuppApiUrl(GensongSuppID, moduleName);
            string requestUri = "PMS/GS_WebServices/GetInventory";

            Dictionary<string, string> dicQueryString = new Dictionary<string, string>();
            dicQueryString.Add("InventoryFactoryID", factoryID);
            dicQueryString.Add("InventoryEstCutDate", estCutDate.ToString("yyyy-MM-dd"));
            dicQueryString.Add("InventoryCutCellID", cutCellID);

            WebApiBaseResult webApiBaseResult;
            webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(baseUrl, requestUri, string.Empty, 600, queryStrings: dicQueryString);
            inventorySpreadingSchedule = new InventorySpreadingSchedule();

            switch (webApiBaseResult.webApiResponseStatus)
            {
                case WebApiResponseStatus.Success:
                    inventorySpreadingSchedule = JsonConvert.DeserializeObject<InventorySpreadingSchedule>(webApiBaseResult.responseContent);
                    return new DualResult(true);
                case WebApiResponseStatus.WebApiReturnFail:
                    return new DualResult(false, webApiBaseResult.responseContent);
                case WebApiResponseStatus.OtherException:
                    return new DualResult(false, webApiBaseResult.exception);
                case WebApiResponseStatus.ApiTimeout:
                    return new DualResult(false, webApiBaseResult.responseContent);
                default:
                    return new DualResult(false, webApiBaseResult.responseContent);
            }
        }

        /// <summary>
        /// InventoryItem
        /// </summary>
        public class InventoryItem
        {
            /// <summary>
            /// POID
            /// </summary>
            public string POID { get; set; }

            /// <summary>
            /// Cutref
            /// </summary>
            public string Cutref { get; set; }

            /// <summary>
            /// RefNo
            /// </summary>
            public string RefNo { get; set; }

            /// <summary>
            /// SCIRefNo
            /// </summary>
            public string SCIRefNo { get; set; }

            /// <summary>
            /// ColorID
            /// </summary>
            public string ColorID { get; set; }

            /// <summary>
            /// FromSingleRoll_Qty
            /// </summary>
            public decimal FromSingleRoll_Qty { get; set; }

            /// <summary>
            /// FromProduction_Qty
            /// </summary>
            public decimal FromProduction_Qty { get; set; }

            /// <summary>
            /// Ttl_Qty
            /// </summary>
            public decimal Ttl_Qty { get; set; }
        }

        /// <summary>
        /// InventorySendSpreadingSchedule
        /// </summary>
        public class InventorySpreadingSchedule
        {
            /// <summary>
            /// CutCellID
            /// </summary>
            public string CutCellID { get; set; }

            /// <summary>
            /// Inventory
            /// </summary>
            public List<InventoryItem> Inventory { get; set; }
        }
    }
}

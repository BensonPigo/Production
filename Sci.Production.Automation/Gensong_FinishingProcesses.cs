using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Sci.Production.Automation.UtilityAutomation;

namespace Sci.Production.Automation
{
    /// <inheritdoc/>
    public class Gensong_FinishingProcesses
    {
        private static readonly string GensongSuppID = "3A0174";
        private static readonly string moduleName = "FinishingProcesses";
        private AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS();

        /// <inheritdoc/>
        public static bool IsGensong_FinishingProcessesEnable => IsModuleAutomationEnable(GensongSuppID, moduleName);

        /// <summary>
        /// SentPackingListToFinishingProcesses
        /// </summary>
        /// <param name="listID">List ID</param>
        /// <param name="actionType">Action Type</param>
        public void SentOrdersToFinishingProcesses(string listID, string actionType)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentPackingListToFinishingProcesses";
            string suppAPIThread = "api/GensongFinishingProcesses/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;
            string tableName = actionType == "Delete" ? "Order_Delete" : "Orders";

            Dictionary<string, object> dataTable = new Dictionary<string, object>();

            var structureID = listID.Split(',').Select(s => new { ID = s });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure(tableName, structureID));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentPackingListToFinishingProcesses
        /// </summary>
        /// <param name="listID">List ID</param>
        /// <param name="actionType">Action Type</param>
        public void SentPackingListToFinishingProcesses(string listID, string actionType)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentPackingListToFinishingProcesses";
            string suppAPIThread = "api/GensongFinishingProcesses/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;
            string tableName = actionType == "Delete" ? "PackingList_Delete" : "PackingList";

            Dictionary<string, object> dataTable = new Dictionary<string, object>();

            var structureID = listID.Split(',').Select(s => new { ID = s });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure(tableName, structureID));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentPackingListToFinishingProcesses
        /// </summary>
        /// <param name="listID">List ID</param>
        /// <param name="actionType">Action Type</param>
        public void SentClogGarmentDisposeToFinishingProcesses(string listID, string actionType)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentClogGarmentDisposeToFinishingProcesses";
            string suppAPIThread = "api/GensongFinishingProcesses/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;
            string tableName = actionType == "ClogGarmentDispose_Delete" ? "ClogGarmentDispose_Delete" : "ClogGarmentDispose";

            Dictionary<string, object> dataTable = new Dictionary<string, object>();

            var structureID = listID.Split(',').Select(s => new { ID = s });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure(tableName, structureID));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        private object CreateGensongStructure(string tableName, object structureID)
        {
            Dictionary<string, object> resultObj = new Dictionary<string, object>
            {
                { "TableArray", new string[] { tableName } },
            };

            Dictionary<string, object> dataStructure = new Dictionary<string, object>
            {
                { tableName, structureID },
            };
            resultObj.Add("DataTable", dataStructure);

            return resultObj;
        }
    }
}

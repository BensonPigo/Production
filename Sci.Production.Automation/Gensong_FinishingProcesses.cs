using Newtonsoft.Json;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
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
        /// SentOrdersToFinishingProcesses_Gensong
        /// </summary>
        /// <param name="listID">List ID</param>
        /// <param name="transTable">transTable</param>
        public void SentOrdersToFinishingProcesses(string listID, string transTable)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentOrdersToFinishingProcesses_Gensong";
            string suppAPIThread = "api/GensongFinishingProcesses/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string orderTransTable = transTable;
            string tableArray;
            if (orderTransTable == "Order_Delete")
            {
                tableArray = "Order_Delete";
            }
            else
            {
                tableArray = "Orders";
            }

            object postBody;
            string[] structureID = listID.Split(',');

            int sendApiCount = MyUtility.Convert.GetInt(Math.Ceiling(structureID.Length / 500.0));
            Dictionary<string, object> dataTable = new Dictionary<string, object>();
            dataTable.Add("orderTransTable", orderTransTable);

            // 先以500筆為單位拆分後再傳出
            for (int i = 0; i < sendApiCount; i++)
            {
                int skipCount = i * 500;
                var orderIDs = structureID.Skip(skipCount).Take(500).Select(s => new { ID = s });
                dataTable.Add(tableArray, orderIDs);
                postBody = new { TableArray = new string[] { tableArray }, DataTable = dataTable };

                SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, JsonConvert.SerializeObject(postBody), this.automationErrMsg);

                dataTable.Remove(tableArray);
            }
        }

        /// <summary>
        /// SentPackingListToFinishingProcesses_Gensong
        /// </summary>
        /// <param name="listID">List ID</param>
        /// <param name="actionType">Action Type</param>
        public async void SentPackingListToFinishingProcesses(string listID, string actionType)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentPackingListToFinishingProcesses_Gensong";
            string suppAPIThread = "api/GensongFinishingProcesses/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;
            string tableName = actionType == "Delete" ? "PackingList_Delete" : "PackingList";

            Dictionary<string, object> dataTable = new Dictionary<string, object>();

            var structureID = listID.Split(',').Select(s => new { ID = s });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure(tableName, structureID));
            AutomationCreateRecord automationCreateRecord = new AutomationCreateRecord(this.automationErrMsg, jsonBody);
            SqlConnection sqlConnection = new SqlConnection();
            try
            {
                DBProxy._OpenConnection("Production", out sqlConnection);
                automationCreateRecord.SaveAutomationCreateRecord(sqlConnection);

                await SendWebAPIAsync(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);

                DBProxy._OpenConnection("Production", out sqlConnection);
                automationCreateRecord.DeleteAutomationCreateRecord(sqlConnection);
            }
            catch (Exception ex)
            {
                if (!MyUtility.Check.Empty(automationCreateRecord.ukey))
                {
                    DBProxy._OpenConnection("Production", out sqlConnection);
                    automationCreateRecord.DeleteAutomationCreateRecord(sqlConnection);
                }

                this.automationErrMsg.SetErrInfo(ex, jsonBody);
                SaveAutomationErrMsg(this.automationErrMsg);
            }
        }

        /// <summary>
        /// SentClogGarmentDisposeToFinishingProcesses_Gensong
        /// </summary>
        /// <param name="listID">List ID</param>
        /// <param name="actionType">Action Type</param>
        public void SentClogGarmentDisposeToFinishingProcesses(string listID, string actionType)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentClogGarmentDisposeToFinishingProcesses_Gensong";
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

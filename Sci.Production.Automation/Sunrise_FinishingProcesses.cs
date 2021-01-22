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
    public class Sunrise_FinishingProcesses
    {
        private static readonly string sunriseSuppID = "3A0134";
        private static readonly string moduleName = "FinishingProcesses";
        private AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS();

        /// <inheritdoc/>
        public static bool IsSunrise_FinishingProcessesEnable => IsModuleAutomationEnable(sunriseSuppID, moduleName);

        /// <summary>
        /// SentPackingToFinishingProcesses
        /// </summary>
        /// <param name="listID">List ID</param>
        /// <param name="actionType">Action Type</param>
        public void SentPackingToFinishingProcesses(string listID, string actionType)
        {
            if (!IsModuleAutomationEnable(sunriseSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentPackingToFinishingProcesses";
            string suppAPIThread = "api/SunriseFinishingProcesses/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;
            string tableName = actionType == "delete" ? "PackingList_Delete" : "PackingList_Detail";

            Dictionary<string, object> dataTable = new Dictionary<string, object>();

            string[] structureID = listID.Split(',');

            int sendApiCount = MyUtility.Convert.GetInt(Math.Ceiling(structureID.Length / 10.0));
            List<AutomationCreateRecord> listSendData = new List<AutomationCreateRecord>();
            SqlConnection sqlConnection = new SqlConnection();

            try
            {
                // 先以10筆為單位拆分後再傳出
                for (int i = 0; i < sendApiCount; i++)
                {
                    int skipCount = i * 10;
                    var packIDs = structureID.Skip(skipCount).Take(10).Select(s => new { ID = s });

                    string jsonBody = JsonConvert.SerializeObject(this.CreateSunriseStructure(tableName, packIDs));
                    AutomationCreateRecord automationCreateRecord = new AutomationCreateRecord(this.automationErrMsg, jsonBody);
                    DBProxy._OpenConnection("Production", out sqlConnection);
                    automationCreateRecord.SaveAutomationCreateRecord(sqlConnection);

                    listSendData.Add(automationCreateRecord);
                }


                foreach (AutomationCreateRecord item in listSendData)
                {
                    SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, item.json, this.automationErrMsg);
                    DBProxy._OpenConnection("Production", out sqlConnection);
                    item.DeleteAutomationCreateRecord(sqlConnection);
                }
            }
            catch (Exception ex)
            {
                if (listSendData.Count > 0)
                {
                    foreach (AutomationCreateRecord item in listSendData)
                    {
                        this.automationErrMsg.SetErrInfo(ex, item.json);
                        SaveAutomationErrMsg(this.automationErrMsg);
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SentLocalItemToFinishingProcesses
        /// </summary>
        /// <param name="listRefNo">List RefNo</param>
        public void SentLocalItemToFinishingProcesses(string listRefNo)
        {
            if (!IsModuleAutomationEnable(sunriseSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentLocalItemToFinishingProcesses";
            string suppAPIThread = "api/SunriseFinishingProcesses/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            Dictionary<string, object> dataTable = new Dictionary<string, object>();

            var structureID = listRefNo.Split(',').Select(s => new { RefNo = s });

            string jsonBody = JsonConvert.SerializeObject(this.CreateSunriseStructure("LocalItem", structureID));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentFinishingProcess
        /// </summary>
        /// <param name="listDM300">listDM300</param>
        public void SentFinishingProcess(string listDM300)
        {
            if (!IsModuleAutomationEnable(sunriseSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentLocalItemToFinishingProcesses";
            string suppAPIThread = "api/SunriseFinishingProcesses/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            Dictionary<string, object> dataTable = new Dictionary<string, object>();

            var structureID = listDM300.Split(',').Select(s => new { DM300 = s });

            string jsonBody = JsonConvert.SerializeObject(this.CreateSunriseStructure("FinishingProcess", structureID));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentSewingOutputTransfer
        /// </summary>
        /// <param name="listUkey">listUkey</param>
        public void SentSewingOutputTransfer(string listUkey)
        {
            if (!IsModuleAutomationEnable(sunriseSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentLocalItemToFinishingProcesses";
            string suppAPIThread = "api/SunriseFinishingProcesses/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            Dictionary<string, object> dataTable = new Dictionary<string, object>();

            var structureID = listUkey.Split(',').Select(s => new { Ukey = s });

            string jsonBody = JsonConvert.SerializeObject(this.CreateSunriseStructure("SewingOutputTransfer", structureID));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        private object CreateSunriseStructure(string tableName, object structureID)
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

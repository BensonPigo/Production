using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using static AutomationErrMsg;
using static PmsWebApiUtility20.WebApiTool;
using DualResult = Ict.DualResult;

namespace Sci.Production.Automation
{
    /// <summary>
    /// UtilityAutomation
    /// </summary>
    public static class UtilityAutomation
    {
        /// <inheritdoc/>
        public static string Sci => BaseUtilityAutomation.Sci;

        /// <inheritdoc/>
        public static bool IsAutomationEnable => PMSUtilityAutomation.UtilityAutomation.IsAutomationEnable;

        /// <inheritdoc/>
        public static string ModuleType => PMSUtilityAutomation.UtilityAutomation.ModuleType;

        /// <summary>
        /// IsModuleAutomationEnable
        /// </summary>
        /// <param name="suppid">Supp ID</param>
        /// <param name="module">Module</param>
        /// <returns>bool</returns>
        public static bool IsModuleAutomationEnable(string suppid, string module)
        {
            return PMSUtilityAutomation.UtilityAutomation.IsModuleAutomationEnable(suppid, module);
        }

        /// <summary>
        /// AppendBaseInfo
        /// </summary>
        /// <param name="bodyObject">dynamic Body Object</param>
        /// <param name="apiTag">ApiTag</param>
        /// <returns>dynamic</returns>
        public static dynamic AppendBaseInfo(dynamic bodyObject, string apiTag)
        {
            return PMSUtilityAutomation.UtilityAutomation.AppendBaseInfo(bodyObject, apiTag);
        }

        /// <summary>
        /// SaveAutomationErrMsg
        /// </summary>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        public static void SaveAutomationErrMsg(AutomationErrMsg automationErrMsg)
        {
            PMSUtilityAutomation.UtilityAutomation.SaveAutomationErrMsg(automationErrMsg);
        }

        /// <summary>
        /// Save Automation Check Msg
        /// </summary>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        public static void SaveAutomationCheckMsg(AutomationErrMsg automationErrMsg)
        {
            PMSUtilityAutomation.UtilityAutomation.SaveAutomationCheckMsg(automationErrMsg);
        }

        /// <summary>
        /// Save Automation Check Msg
        /// </summary>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        /// <param name="isAutoWH">isAutoWH</param>
        /// <returns>AutomationTransRecord Ukey</returns>
        public static long SaveAutomationTransRecord(AutomationErrMsg automationErrMsg, bool isAutoWH = true)
        {
            return PMSUtilityAutomation.UtilityAutomation.SaveAutomationTransRecord(automationErrMsg, isAutoWH);
        }

        /// <summary>
        /// AutomationExceptionHandler
        /// </summary>
        /// <param name="task">Task</param>
        public static void AutomationExceptionHandler(Task task)
        {
            var exception = task.Exception;
            MyUtility.Msg.ErrorBox(exception.ToString());
        }

        /// <summary>
        /// SendWebAPISaveAutomationCreateRecord
        /// </summary>
        /// <param name="baseUrl">baseUrl</param>
        /// <param name="requestUri">requestUri</param>
        /// <param name="jsonBody">jsonBody</param>
        /// <param name="automationErrMsg">automationErrMsg</param>
        /// <returns>DualResult</returns>
        public static DualResult SendWebAPISaveAutomationCreateRecord(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg)
        {
            return PMSUtilityAutomation.UtilityAutomation.SendWebAPISaveAutomationCreateRecord(baseUrl, requestUri, jsonBody, automationErrMsg);
        }

        /// <summary>
        /// SendWebAPI
        /// </summary>
        /// <param name="baseUrl">Base Url</param>
        /// <param name="requestUri">Request Url</param>
        /// <param name="jsonBody">Json Body</param>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        /// <returns>DualResult</returns>
        public static DualResult SendWebAPI(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg)
        {
            return PMSUtilityAutomation.UtilityAutomation.SendWebAPI(baseUrl, requestUri, jsonBody, automationErrMsg);
        }

        /// <summary>
        /// SendWebAPI
        /// </summary>
        /// <param name="baseUrl">Base Url</param>
        /// <param name="requestUri">Request Url</param>
        /// <param name="jsonBody">Json Body</param>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        /// <returns>DualResult</returns>
        public static async Task<DualResult> SendWebAPIAsync(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg)
        {
            return await PMSUtilityAutomation.UtilityAutomation.SendWebAPIAsync(baseUrl, requestUri, jsonBody, automationErrMsg);
        }

        /// <summary>
        /// GetCustomHeaders
        /// </summary>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, string> GetCustomHeaders()
        {
            return PMSUtilityAutomation.UtilityAutomation.GetCustomHeaders();
        }

        /// <summary>
        /// Send WebAPI To Auto W/H
        /// </summary>
        /// <param name="baseUrl">baseUrl</param>
        /// <param name="requestUri">requestUri</param>
        /// <param name="jsonBody">jsonBody</param>
        /// <param name="automationErrMsg">automationErrMsg</param>
        /// <param name="reSented">reSented</param>
        /// <returns>DualResult</returns>
        public static DualResult WH_Auto_SendWebAPI(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg, bool reSented = false)
        {
            return PMSUtilityAutomation.UtilityAutomation.WH_Auto_SendWebAPI(baseUrl: baseUrl, requestUri: requestUri, jsonBody: jsonBody, automationErrMsg: automationErrMsg, reSented: reSented);
        }

        /// <summary>
        /// Automation Err Msg PMS
        /// </summary>
        public class AutomationErrMsgPMS : AutomationErrMsg
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AutomationErrMsgPMS"/> class.
            /// </summary>
            public AutomationErrMsgPMS()
            {
                this.suppID = Sci;
                this.moduleName = Sci;
            }

            /// <summary>
            /// Set Err Info
            /// </summary>
            /// <param name="result">DualResult</param>
            public void SetErrInfo(DualResult result)
            {
                this.errorCode = string.Empty;
                this.errorMsg = result.GetException().ToString();
                this.json = string.Empty;
            }

            /// <summary>
            /// SetErrInfo
            /// </summary>
            /// <param name="ex">ex</param>
            /// <param name="json">json</param>
            public void SetErrInfo(Exception ex, string json)
            {
                this.errorCode = "995";
                this.errorMsg = "From PMS Exception" + Environment.NewLine + ex.ToString();
                this.json = json;
            }
        }

        /// <summary>
        /// Get Sci Url
        /// </summary>
        /// <returns>WebApiURL.URL</returns>
        public static string GetSciUrl()
        {
            return PMSUtilityAutomation.UtilityAutomation.GetSciUrl();
        }

        /// <summary>
        /// Get Supplier Url
        /// </summary>
        /// <param name="supp">supp</param>
        /// <param name="moduleName">moduleName</param>
        /// <returns>WebApiURL.URL</returns>
        public static string GetSupplierUrl(string supp, string moduleName)
        {
            return PMSUtilityAutomation.UtilityAutomation.GetSupplierUrl(supp, moduleName);
        }

        /// <inheritdoc/>
        public delegate ErrorRespone ParsingErrResponse(string webApiBaseResult);

        /// <inheritdoc/>
        public class ErrorRespone
        {
            private string error_code = string.Empty;
            private string error = string.Empty;

            /// <inheritdoc/>
            public string Error_code
            {
                get { return this.error_code; }
                set { this.error_code = value; }
            }

            /// <inheritdoc/>
            public string Error
            {
                get { return this.error; }
                set { this.error = value; }
            }
        }
    }
}

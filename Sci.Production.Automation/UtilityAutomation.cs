using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
        public static string Sci => "SCI";

        /// <inheritdoc/>
        public static bool IsAutomationEnable => MyUtility.Check.Seek("select 1 from dbo.System where Automation = 1", "Production");

        /// <inheritdoc/>
        public static string ModuleType
        {
            get
            {
                if (PmsWebAPI.IsDummy)
                {
                    return "Dummy";
                }
                else
                {
                    return "Formal";
                }
            }
        }

        /// <summary>
        /// IsModuleAutomationEnable
        /// </summary>
        /// <param name="suppid">Supp ID</param>
        /// <param name="module">Module</param>
        /// <returns>bool</returns>
        public static bool IsModuleAutomationEnable(string suppid, string module)
        {
            return IsAutomationEnable && MyUtility.Check.Seek($"select 1 from dbo.WebApiURL where SuppID = '{suppid}' and ModuleName = '{module}'  and ModuleType = '{ModuleType}' and Junk = 0 ", "Production");
        }

        /// <summary>
        /// AppendBaseInfo
        /// </summary>
        /// <param name="bodyObject">dynamic Body Object</param>
        /// <param name="apiTag">ApiTag</param>
        /// <returns>dynamic</returns>
        public static dynamic AppendBaseInfo(dynamic bodyObject, string apiTag)
        {
            dynamic newBodyObject = bodyObject;
            newBodyObject.APItags = apiTag;
            newBodyObject.CmdTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            return newBodyObject;
        }

        /// <summary>
        /// SaveAutomationErrMsg
        /// </summary>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        public static void SaveAutomationErrMsg(AutomationErrMsg automationErrMsg)
        {
            automationErrMsg.addName = Env.User.UserID;
            SqlConnection sqlConnection = new SqlConnection();
            DBProxy._OpenConnection("Production", out sqlConnection);
            using (sqlConnection)
            {
                PmsWebApiUtility20.Automation.SaveAutomationErrMsg(automationErrMsg, sqlConnection);
            }
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
        /// SendWebAPI
        /// </summary>
        /// <param name="baseUrl">Base Url</param>
        /// <param name="requestUri">Request Url</param>
        /// <param name="jsonBody">Json Body</param>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        /// <returns>DualResult</returns>
        public static DualResult SendWebAPI(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg)
        {
            DualResult result = new DualResult(true);
            WebApiBaseResult webApiBaseResult;
            webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(baseUrl, requestUri, jsonBody, 600);
            automationErrMsg.json = jsonBody;

            if (!webApiBaseResult.isSuccess)
            {
                result = new DualResult(false, new Ict.BaseResult.MessageInfo(automationErrMsg.errorMsg));
                automationErrMsg.SetErrInfo(webApiBaseResult, jsonBody);
                SaveAutomationErrMsg(automationErrMsg);
            }

            return result;
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
            return MyUtility.GetValue.Lookup($"select URL from WebApiURL with (nolock) where SuppID = '{Sci}' and ModuleName = '{Sci}' and ModuleType = '{ModuleType}' ", "Production");
        }

        /// <summary>
        /// GetSuppApiUrl
        /// </summary>
        /// <param name="suppID">suppID</param>
        /// <param name="moduleName">moduleName</param>
        /// <returns>string</returns>
        public static string GetSuppApiUrl(string suppID, string moduleName)
        {
            return MyUtility.GetValue.Lookup($"select URL from WebApiURL with (nolock) where SuppID = '{suppID}' and ModuleName = '{moduleName}' and ModuleType = '{ModuleType}'", "Production");
        }
    }
}

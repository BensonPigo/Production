using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.Prg;
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
        /// Save Automation Check Msg
        /// </summary>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        public static void SaveAutomationCheckMsg(AutomationErrMsg automationErrMsg)
        {
            string saveSql = $@"
      insert into AutomationCheckMsg( SuppID,  ModuleName,  APIThread,  SuppAPIThread,  ErrorCode,  ErrorMsg,  JSON,  AddName, AddDate)
                        values(@SuppID, @ModuleName, @APIThread, @SuppAPIThread, @ErrorCode, @ErrorMsg, @JSON, @AddName, GETDATE())
";
            List<SqlParameter> listPar = new List<SqlParameter>()
            {
               new SqlParameter("@SuppID", automationErrMsg.suppID),
               new SqlParameter("@ModuleName", automationErrMsg.moduleName),
               new SqlParameter("@APIThread", automationErrMsg.apiThread),
               new SqlParameter("@SuppAPIThread", automationErrMsg.suppAPIThread),
               new SqlParameter("@ErrorCode", automationErrMsg.errorCode),
               new SqlParameter("@ErrorMsg", automationErrMsg.errorMsg),
               new SqlParameter("@JSON", automationErrMsg.json),
               new SqlParameter("@AddName", Env.User.UserID),
            };

            DualResult result = DBProxy.Current.Execute("Production", saveSql, listPar);

            if (!result)
            {
                throw result.GetException();
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
            Dictionary<string, string> requestHeaders = GetCustomHeaders();
            webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(baseUrl, requestUri, jsonBody, 600, requestHeaders);
            automationErrMsg.json = jsonBody;
            if (!webApiBaseResult.isSuccess)
            {
                automationErrMsg.SetErrInfo(webApiBaseResult, jsonBody);
                result = new DualResult(false, new Ict.BaseResult.MessageInfo(automationErrMsg.errorMsg));
                SaveAutomationErrMsg(automationErrMsg);
            }

            return result;
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
            DualResult result = new DualResult(true);
            WebApiBaseResult webApiBaseResult;
            Dictionary<string, string> requestHeaders = GetCustomHeaders();
            webApiBaseResult = await PmsWebApiUtility45.WebApiTool.WebApiPostAsync(baseUrl, requestUri, jsonBody, 600, requestHeaders);
            automationErrMsg.json = jsonBody;
            if (!webApiBaseResult.isSuccess)
            {
                automationErrMsg.SetErrInfo(webApiBaseResult, jsonBody);
                result = new DualResult(false, new Ict.BaseResult.MessageInfo(automationErrMsg.errorMsg));
                SaveAutomationErrMsg(automationErrMsg);
            }

            return result;
        }

        /// <summary>
        /// GetCustomHeaders
        /// </summary>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, string> GetCustomHeaders()
        {
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            StackTrace stackTrace = new StackTrace();
            var sciStackTrace = stackTrace.GetFrames().Where(s => s.GetMethod().DeclaringType.FullName.Contains("Sci.Production"));
            MethodBase methodBase = sciStackTrace.Any() ? sciStackTrace.Last().GetMethod() : stackTrace.GetFrame(7).GetMethod();

            string callFrom = methodBase.DeclaringType.FullName;
            if (callFrom.Contains("+"))
            {
                callFrom = callFrom.Split('+')[0];
            }

            string activity = methodBase.Name;
            if (activity.Contains("<") && activity.Contains(">"))
            {
                activity = activity.Split('<')[1].Split('>')[0];
            }

            requestHeaders.Add("CallFrom", callFrom);
            requestHeaders.Add("Activity", activity);
            requestHeaders.Add("User", Env.User.UserID);

            return requestHeaders;
        }

        /// <summary>
        /// Send WebAPI To Auto W/H
        /// </summary>
        /// <param name="baseUrl">baseUrl</param>
        /// <param name="requestUri">requestUri</param>
        /// <param name="jsonBody">jsonBody</param>
        /// <param name="automationErrMsg">automationErrMsg</param>
        /// <returns>DualResult</returns>
        public static DualResult WH_Auto_SendWebAPI(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg)
        {
            DualResult result = new DualResult(true);
            WebApiBaseResult webApiBaseResult;
            webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(baseUrl, requestUri, jsonBody, 20);
            bool saveAllmsg = MyUtility.Convert.GetBool(ConfigurationManager.AppSettings["OpenAll_AutomationCheckMsg"]);

            if (!webApiBaseResult.isSuccess)
            {
                automationErrMsg.errorCode = "99";
                automationErrMsg.errorMsg = webApiBaseResult.responseContent.ToString();
                automationErrMsg.json = jsonBody;
                result = new DualResult(false, new Ict.BaseResult.MessageInfo(automationErrMsg.errorMsg));
                SaveAutomationCheckMsg(automationErrMsg);
            }
            else if (saveAllmsg)
            {
                automationErrMsg.json = jsonBody;
                SaveAutomationCheckMsg(automationErrMsg);
            }

            return result;

            // MyUtility.Msg.InfoBox("Send Web API to VS");
            // return new DualResult(true);
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
        /// Get Supplier Url
        /// </summary>
        /// <param name="supp">supp</param>
        /// <param name="moduleName">moduleName</param>
        /// <returns>WebApiURL.URL</returns>
        public static string GetSupplierUrl(string supp, string moduleName)
        {
            return MyUtility.GetValue.Lookup($"select URL from WebApiURL with (nolock) where SuppID = '{supp}' and ModuleName = '{moduleName}' and ModuleType = '{ModuleType}' ", "Production");
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

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
            newBodyObject.CmdTime = DateTime.Now;
            return newBodyObject;
        }

        /// <summary>
        /// SaveAutomationErrMsg
        /// </summary>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        public static void SaveAutomationErrMsg(AutomationErrMsg automationErrMsg)
        {
            string saveSql = $@"
 insert into AutomationErrMsg(SuppID, ModuleName, APIThread, SuppAPIThread, ErrorCode, ErrorMsg, JSON, ReSented, AddName, AddDate)
                        values(@SuppID, @ModuleName, @APIThread, @SuppAPIThread, @ErrorCode, @ErrorMsg, @JSON, 0, @AddName, GETDATE())
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
        public static void SendWebAPI(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg)
        {
            WebApiBaseResult webApiBaseResult;
            webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(baseUrl, requestUri, jsonBody, 10);

            if (!webApiBaseResult.isSuccess)
            {
                automationErrMsg.SetErrInfo(webApiBaseResult, jsonBody);
                SaveAutomationErrMsg(automationErrMsg);
            }
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
        }

        /// <summary>
        /// Get Sci Url
        /// </summary>
        /// <returns>WebApiURL.URL</returns>
        public static string GetSciUrl()
        {
            return MyUtility.GetValue.Lookup($"select URL from WebApiURL with (nolock) where SuppID = '{Sci}' and ModuleName = '{Sci}' and ModuleType = '{ModuleType}' ", "Production");
        }
    }
}

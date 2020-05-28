using Ict;
using Newtonsoft.Json;
using PmsWebApiUtility20;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
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
        public static bool IsAutomationEnable
        {
            get { return MyUtility.Check.Seek("select 1 from dbo.System where Automation = 1"); }
        }

        public static bool IsModuleAutomationEnable(string suppid, string module)
        {
            return IsAutomationEnable && MyUtility.Check.Seek($"select 1 from dbo.WebApiURL where SuppID = '{suppid}' and ModuleName = '{module}' and Junk = 0");
        }

        public static dynamic AppendBaseInfo(dynamic bodyObject, string apiTag)
        {
            dynamic newBodyObject = bodyObject;
            newBodyObject.APItags = apiTag;
            newBodyObject.CmdTime = DateTime.Now;
            return newBodyObject;
        }

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
                new SqlParameter("@AddName", Env.User.UserID)
            };

            DualResult result = DBProxy.Current.Execute(null, saveSql, listPar);

            if (!result)
            {
                throw result.GetException();
            }
        }

        public static void AutomationExceptionHandler(Task task)
        {
            var exception = task.Exception;
            MyUtility.Msg.ErrorBox(exception.ToString());
        }

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

        public class AutomationErrMsgPMS : AutomationErrMsg
        {
            public void SetErrInfo(DualResult result)
            {
                this.errorCode = string.Empty;
                this.errorMsg = result.GetException().ToString();
                this.json = string.Empty;
            }
        }

        public static string GetBaseUrl(string suppID, string moduleName)
        {
            return MyUtility.GetValue.Lookup($"select URL from WebApiURL with (nolock) where SuppID = '{suppID}' and ModuleName = '{moduleName}' ");
        }
    }
}

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
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using static AutomationErrMsg;
using static PmsWebApiUtility20.WebApiTool;
using DualResult = Ict.DualResult;

namespace Sci.Production.Automation
{
    /// <summary>
    /// BaseUtilityAutomation
    /// </summary>
    public abstract class BaseUtilityAutomation
    {
        /// <summary>
        /// Sci
        /// </summary>
        public static string Sci => "SCI";

        /// <summary>
        /// IsAutomationEnable
        /// </summary>
        public abstract bool IsAutomationEnable { get; }

        /// <inheritdoc/>
        public abstract string ModuleType { get; }

        /// <inheritdoc/>
        public abstract string UserID { get; }

        /// <summary>
        /// OpenAll_AutomationCheckMsg
        /// </summary>
        public abstract bool OpenAll_AutomationCheckMsg { get; }

        /// <summary>
        /// Select
        /// </summary>
        /// <param name="connname">connname</param>
        /// <param name="sqlCmd">sqlCmd</param>
        /// <param name="sqlPars">sqlPars</param>
        /// <param name="dtResult">dtResult</param>
        /// <returns>DualResult</returns>
        public abstract DualResult Select(string connname, string sqlCmd, IList<SqlParameter> sqlPars, out DataTable dtResult);

        public DualResult Execute(string connName, string sql,  IList<SqlParameter> listPar = null)
        {
            return this.Select(connName, sql, listPar, out DataTable dtResult);
        }

        /// <summary>
        /// OpenConnection
        /// </summary>
        /// <param name="connName">connName</param>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <returns>DualResult</returns>
        public abstract DualResult OpenConnection(string connName, out SqlConnection sqlConnection);

        /// <summary>
        /// Seek
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="connName">connName</param>
        /// <returns>bool</returns>
        public bool Seek(string sql, string connName)
        {
            DualResult result = this.Select(connName, sql, null, out DataTable dtResult);

            if (!result)
            {
                throw result.GetException();
            }

            return dtResult.Rows.Count > 0;
        }

        /// <summary>
        /// Lookup
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="connName">connName</param>
        /// <param name="listPar">listPar</param>
        /// <returns>string</returns>
        public string Lookup(string sql, string connName, IList<SqlParameter> listPar = null)
        {
            DualResult result = this.Select(connName, sql, listPar, out DataTable dtResult);

            if (!result)
            {
                throw result.GetException();
            }

            return dtResult.Rows.Count > 0 ? dtResult.Rows[0][0].ToString() : string.Empty;
        }

        /// <summary>
        /// IsModuleAutomationEnable
        /// </summary>
        /// <param name="suppid">Supp ID</param>
        /// <param name="module">Module</param>
        /// <returns>bool</returns>
        public bool IsModuleAutomationEnable(string suppid, string module)
        {
            return this.IsAutomationEnable && this.Seek($"select 1 from dbo.WebApiURL where SuppID = '{suppid}' and ModuleName = '{module}'  and ModuleType = '{this.ModuleType}' and Junk = 0 ", "Production");
        }

        /// <summary>
        /// AppendBaseInfo
        /// </summary>
        /// <param name="bodyObject">dynamic Body Object</param>
        /// <param name="apiTag">ApiTag</param>
        /// <returns>dynamic</returns>
        public dynamic AppendBaseInfo(dynamic bodyObject, string apiTag)
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
        public void SaveAutomationErrMsg(AutomationErrMsg automationErrMsg)
        {
            bool isAutoWH = (automationErrMsg.apiThread.Contains("GensongAutoWHFabric") || automationErrMsg.apiThread.Contains("VstrongAutoWHAccessory")) ? true : false;
            #region 先將資料新增至FPS AutomationTransRecord
            long automationTransRecordUkey = MyUtility.Check.Empty(automationErrMsg.AutomationTransRecordUkey) ? this.SaveAutomationTransRecord(automationErrMsg, isAutoWH) : automationErrMsg.AutomationTransRecordUkey;
            #endregion

            string saveSql = $@"
            insert into AutomationErrMsg(SuppID, ModuleName, APIThread, SuppAPIThread, ErrorCode, ErrorMsg, JSON, ReSented, AddName, AddDate, AutomationTransRecordUkey)
                        values(@SuppID, @ModuleName, @APIThread, @SuppAPIThread, @ErrorCode, @ErrorMsg, @JSON, 0, @AddName, GETDATE(), @AutomationTransRecordUkey)
                
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
                new SqlParameter("@AddName", this.UserID),
                new SqlParameter("@AutomationTransRecordUkey", automationTransRecordUkey),
            };

            DualResult result = this.Execute("Production", saveSql, listPar);

            if (!result)
            {
                throw result.GetException();
            }
        }

        /// <summary>
        /// Save Automation Check Msg
        /// </summary>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        public void SaveAutomationCheckMsg(AutomationErrMsg automationErrMsg)
        {
            bool isAutoWH = (automationErrMsg.apiThread.Contains("GensongAutoWHFabric") || automationErrMsg.apiThread.Contains("VstrongAutoWHAccessory")) ? true : false;
            #region 先將資料新增至FPS AutomationTransRecord
            long automationTransRecordUkey = automationTransRecordUkey = MyUtility.Check.Empty(automationErrMsg.AutomationTransRecordUkey) ? SaveAutomationTransRecord(automationErrMsg, isAutoWH) : automationErrMsg.AutomationTransRecordUkey;
            #endregion

            string saveSql = $@"
      insert into AutomationCheckMsg( SuppID,  ModuleName,  APIThread,  SuppAPIThread,  ErrorCode,  ErrorMsg,  JSON,  AddName, AddDate,AutomationTransRecordUkey)
                        values(@SuppID, @ModuleName, @APIThread, @SuppAPIThread, @ErrorCode, @ErrorMsg, @JSON, @AddName, GETDATE(),@AutomationTransRecordUkey)
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
               new SqlParameter("@AddName", this.UserID),
               new SqlParameter("@AutomationTransRecordUkey", automationTransRecordUkey),
            };

            DualResult result = this.Execute("Production", saveSql, listPar);

            if (!result)
            {
                throw result.GetException();
            }
        }

        /// <summary>
        /// Save Automation Check Msg
        /// </summary>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        public long SaveAutomationTransRecord(AutomationErrMsg automationErrMsg, bool isAutoWH = true)
        {
            string saveSql = $@"
insert into AutomationTransRecord(
[CallFrom]     ,
Activity       ,
SuppID         ,
ModuleName     ,
SuppAPIThread  ,
JSON           ,
TransJson      ,
AddName        ,
AddDate
)
values
(
@CallFrom     ,
@Activity       ,
@SuppID         ,
@ModuleName     ,
@SuppAPIThread  ,
@JSON           ,
@TransJson      ,
@AddName        ,
getdate()
)

declare @ID bigint = (select @@IDENTITY)
select ID = @ID
";
            string strSimpleJson = automationErrMsg.json;

            if (isAutoWH)
            {
                #region Get simple Json Data

                Dictionary<string, object> resultDictionary = new Dictionary<string, object>();
                #region 取得原始Json
                string strJson = automationErrMsg.json;
                int startIndex = strJson.LastIndexOf("[");
                int endIndex = strJson.LastIndexOf("]");
                int jsonLength = endIndex - startIndex;
                string oriJson = strJson.Substring(startIndex, jsonLength + 1);

                string strTableName = strJson.Substring(strJson.IndexOf("[") + 2, strJson.IndexOf("]") - strJson.IndexOf("[") - 3);
                #endregion

                // 取得Json的Status
                DataTable dtJson = (DataTable)JsonConvert.DeserializeObject(oriJson, typeof(DataTable));
                dynamic bodyObject = new ExpandoObject();
                bodyObject = dtJson.AsEnumerable()
                    .Select(dr => new
                    {
                        ID = dr["ID"].ToString(),
                    });

                strSimpleJson = JsonConvert.SerializeObject(this.CreateGensongStructure(strTableName, bodyObject));

                #endregion
            }

            List<SqlParameter> listPar = new List<SqlParameter>()
            {
               new SqlParameter("@CallFrom", automationErrMsg.CallFrom),
               new SqlParameter("@Activity", automationErrMsg.Activity),
               new SqlParameter("@SuppID", automationErrMsg.suppID),
               new SqlParameter("@ModuleName", automationErrMsg.moduleName),
               new SqlParameter("@SuppAPIThread", automationErrMsg.suppAPIThread),
               new SqlParameter("@JSON", strSimpleJson),
               new SqlParameter("@TransJson", automationErrMsg.json),
               new SqlParameter("@AddName", this.UserID),
            };

            DualResult result = this.Select("FPS", saveSql, listPar, out DataTable dt);

            if (!result)
            {
                throw result.GetException();
            }

            return MyUtility.Convert.GetLong(dt.Rows[0]["ID"]);
        }

        /// <inheritdoc/>
        public object CreateGensongStructure(string tableName, object structureID)
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

        /// <summary>
        /// SendWebAPISaveAutomationCreateRecord
        /// </summary>
        /// <param name="baseUrl">baseUrl</param>
        /// <param name="requestUri">requestUri</param>
        /// <param name="jsonBody">jsonBody</param>
        /// <param name="automationErrMsg">automationErrMsg</param>
        /// <returns>DualResult</returns>
        public DualResult SendWebAPISaveAutomationCreateRecord(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg)
        {
            AutomationCreateRecord automationCreateRecord = new AutomationCreateRecord(automationErrMsg, jsonBody);
            SqlConnection sqlConnection = new SqlConnection();

            this.OpenConnection("Production", out sqlConnection);
            automationCreateRecord.SaveAutomationCreateRecord(sqlConnection);

            DualResult result = this.SendWebAPI(baseUrl, requestUri, jsonBody, automationErrMsg);

            this.OpenConnection("Production", out sqlConnection);
            automationCreateRecord.DeleteAutomationCreateRecord(sqlConnection);

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
        public DualResult SendWebAPI(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg)
        {
            DualResult result = new DualResult(true);
            WebApiBaseResult webApiBaseResult;
            Dictionary<string, string> requestHeaders = this.GetCustomHeaders();
            automationErrMsg.CallFrom = requestHeaders["CallFrom"];
            automationErrMsg.Activity = requestHeaders["Activity"];
            webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(baseUrl, requestUri, jsonBody, 600, requestHeaders);
            automationErrMsg.AutomationTransRecordUkey = webApiBaseResult.TransRecordUkey;
            automationErrMsg.json = jsonBody;
            if (!webApiBaseResult.isSuccess)
            {
                automationErrMsg.SetErrInfo(webApiBaseResult, jsonBody);
                result = new DualResult(false, new Ict.BaseResult.MessageInfo(automationErrMsg.errorMsg));
                this.SaveAutomationErrMsg(automationErrMsg);
            }
            else
            {
                // 將資料新增至FPS AutomationTransRecord
                bool isAutoWH = (automationErrMsg.apiThread.Contains("GensongAutoWHFabric") || automationErrMsg.apiThread.Contains("VstrongAutoWHAccessory")) ? true : false;
                this.SaveAutomationTransRecord(automationErrMsg, isAutoWH);
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
        public async Task<DualResult> SendWebAPIAsync(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg)
        {
            DualResult result = new DualResult(true);
            WebApiBaseResult webApiBaseResult;
            Dictionary<string, string> requestHeaders = this.GetCustomHeaders();
            automationErrMsg.CallFrom = requestHeaders["CallFrom"];
            automationErrMsg.Activity = requestHeaders["Activity"];
            webApiBaseResult = await PmsWebApiUtility45.WebApiTool.WebApiPostAsync(baseUrl, requestUri, jsonBody, 600, requestHeaders);
            automationErrMsg.AutomationTransRecordUkey = webApiBaseResult.TransRecordUkey;
            automationErrMsg.json = jsonBody;
            if (!webApiBaseResult.isSuccess)
            {
                automationErrMsg.SetErrInfo(webApiBaseResult, jsonBody);
                result = new DualResult(false, new Ict.BaseResult.MessageInfo(automationErrMsg.errorMsg));
                this.SaveAutomationErrMsg(automationErrMsg);
            }
            else
            {
                // 將資料新增至FPS AutomationTransRecord
                bool isAutoWH = (automationErrMsg.apiThread.Contains("GensongAutoWHFabric") || automationErrMsg.apiThread.Contains("VstrongAutoWHAccessory")) ? true : false;
                this.SaveAutomationTransRecord(automationErrMsg, isAutoWH);
            }

            return result;
        }

        /// <summary>
        /// GetCustomHeaders
        /// </summary>
        /// <returns>Dictionary</returns>
        public Dictionary<string, string> GetCustomHeaders()
        {

            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();

            CallFromInfo callFromInfo = UtilityPMS.GetCallFrom();

            requestHeaders.Add("CallFrom", callFromInfo.CallFrom);
            requestHeaders.Add("Activity", callFromInfo.MethodName);
            requestHeaders.Add("User", this.UserID);

            return requestHeaders;
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
        public DualResult WH_Auto_SendWebAPI(string baseUrl, string requestUri, string jsonBody, AutomationErrMsg automationErrMsg, bool reSented = false)
        {
            DualResult result = new DualResult(true);
            WebApiBaseResult webApiBaseResult;
            if (reSented)
            {
                Dictionary<string, string> requestHeaders = this.GetCustomHeaders();
                automationErrMsg.CallFrom = requestHeaders["CallFrom"];
                automationErrMsg.Activity = requestHeaders["Activity"];
                webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(baseUrl, requestUri, jsonBody, 600, requestHeaders);
                automationErrMsg.json = jsonBody;

                if (!webApiBaseResult.isSuccess)
                {
                    automationErrMsg.errorCode = "99";
                    automationErrMsg.errorMsg = webApiBaseResult.responseContent.ToString();
                    automationErrMsg.json = jsonBody;
                    result = new DualResult(false, new Ict.BaseResult.MessageInfo(automationErrMsg.errorMsg));
                    this.SaveAutomationErrMsg(automationErrMsg);
                }
                else
                {
                    // 將資料新增至FPS AutomationTransRecord
                    bool isAutoWH = (automationErrMsg.apiThread.Contains("GensongAutoWHFabric") || automationErrMsg.apiThread.Contains("VstrongAutoWHAccessory")) ? true : false;
                    this.SaveAutomationTransRecord(automationErrMsg, isAutoWH);
                }
            }
            else
            {
                webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(baseUrl, requestUri, jsonBody, 60);
                automationErrMsg.AutomationTransRecordUkey = webApiBaseResult.TransRecordUkey;
                bool saveAllmsg = MyUtility.Convert.GetBool(ConfigurationManager.AppSettings["OpenAll_AutomationCheckMsg"]);

                if (!webApiBaseResult.isSuccess)
                {
                    automationErrMsg.errorCode = "99";
                    automationErrMsg.errorMsg = webApiBaseResult.responseContent.ToString();
                    automationErrMsg.json = jsonBody;
                    result = new DualResult(false, new Ict.BaseResult.MessageInfo(automationErrMsg.errorMsg));
                    Dictionary<string, string> requestHeaders = this.GetCustomHeaders();
                    automationErrMsg.CallFrom = requestHeaders["CallFrom"];
                    automationErrMsg.Activity = requestHeaders["Activity"];
                    this.SaveAutomationCheckMsg(automationErrMsg);
                }
                else if (saveAllmsg)
                {
                    automationErrMsg.json = jsonBody;
                    Dictionary<string, string> requestHeaders = this.GetCustomHeaders();
                    automationErrMsg.CallFrom = requestHeaders["CallFrom"];
                    automationErrMsg.Activity = requestHeaders["Activity"];
                    this.SaveAutomationCheckMsg(automationErrMsg);
                }
            }

            return result;

            // MyUtility.Msg.InfoBox("Send Web API to WMS");
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
        public string GetSciUrl()
        {
            return this.Lookup($"select URL from WebApiURL with (nolock) where SuppID = '{Sci}' and ModuleName = '{Sci}' and ModuleType = '{this.ModuleType}' ", "Production");
        }

        /// <summary>
        /// Get Supplier Url
        /// </summary>
        /// <param name="supp">supp</param>
        /// <param name="moduleName">moduleName</param>
        /// <returns>WebApiURL.URL</returns>
        public string GetSupplierUrl(string supp, string moduleName)
        {
            return this.Lookup($"select URL from WebApiURL with (nolock) where SuppID = '{supp}' and ModuleName = '{moduleName}' and ModuleType = '{this.ModuleType}' ", "Production");
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

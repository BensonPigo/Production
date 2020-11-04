using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict;
using Newtonsoft.Json;
using PmsWebApiUtility20;
using Sci.Data;
using Sci.Production.Prg;
using static PmsWebApiUtility20.WebApiTool;
using static PmsWebApiUtility45.WebApiTool;

namespace Sci.Production.Shipping
{
    public static class Utility_WebAPI
    {
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
        /// check SystemWebAPIURL is enable or exists 
        /// </summary>
        /// <param name="FactoryID"> Factory ID</param>
        /// <param name="Country"> Region Code</param>
        /// <returns>bool</returns>
        public static bool IsSystemWebAPIEnable(string FactoryID, string Country)
        {
            return MyUtility.Check.Empty($@"select * from SystemWebAPIURL where SystemName='{FactoryID}' and CountryID='{Country}' and Environment = '{ModuleType}' and Junk = 0");
        }

        /// <summary>
        /// Get SystemWebAPI URL
        /// </summary>
        /// <param name="FactoryID">FactoryID</param>
        /// <param name="Country">Country</param>
        /// <returns>string</returns>
        public static string GetSystemWebAPI(string FactoryID, string Country)
        {
            return MyUtility.GetValue.Lookup($@"select URL from SystemWebAPIURL where SystemName='{FactoryID}' and CountryID='{Country}' and Environment = '{ModuleType}' and Junk = 0", "Production");
        }

        /// <summary>
        /// SendWebAPI
        /// </summary>
        /// <param name="baseUrl">Base Url</param>
        /// <param name="requestUri">Request Url</param>
        /// <param name="jsonBody">Json Body</param>
        /// <param name="automationErrMsg">Automation Err Msg</param>
        public static void SendWebAPI(string baseUrl, string requestUri, string jsonBody, CustomsWebAPIErrMsg customsWebAPIErrMsg)
        {
            WebApiBaseResult webApiBaseResult;
            webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(baseUrl, requestUri, jsonBody, 130);

            if (!webApiBaseResult.isSuccess)
            {
                customsWebAPIErrMsg.SetErrInfo(webApiBaseResult, jsonBody);
                SaveCustomsErrMsg(customsWebAPIErrMsg);
            }
        }

        public static void CustomsExceptionHandler(Task task)
        {
            var exception = task.Exception;
            MyUtility.Msg.ErrorBox(exception.ToString());
        }

        public static dynamic AppendBaseInfo(dynamic bodyObject, string apiTag)
        {
            dynamic newBodyObject = bodyObject;
            newBodyObject.APItags = apiTag;
            newBodyObject.CmdTime = DateTime.Now;
            return newBodyObject;
        }

        public class CustomsWebAPIErrMsg
        {
            public string ErrorMsg = string.Empty;
            public string SystemName;
            public string apiThread;
            public string SystemAPIThread;
            public string CountryID;
            public string errorCode;
            public string json;

            public string errorMsg
            {
                get
                {
                    if (this.ErrorMsg == null)
                    {
                        return string.Empty;
                    }

                    byte[] lintStr = Encoding.Default.GetBytes(this.ErrorMsg);
                    if (lintStr.Length > 1000)
                    {
                        this.ErrorMsg = Encoding.Default.GetString(lintStr, 0, 998);
                    }

                    return this.ErrorMsg;
                }

                set
                {
                    this.errorCode = value;
                }
            }

            public void SetErrInfo(string apiThread, string errorCode, string errorMsg, string json)
            {
                this.apiThread = apiThread;
                this.errorCode = errorCode;
                this.errorMsg = errorMsg;
                this.json = json;
            }

            public delegate ErrorRespone ParsingErrResponse(string webApiBaseResult);

            public void SetErrInfo(WebApiBaseResult webApiBaseResult, string json)
            {
                this.SetErrInfo(webApiBaseResult, json, JsonConvert.DeserializeObject<ErrorRespone>);
            }

            public void SetErrInfo(WebApiBaseResult webApiBaseResult, string json, ParsingErrResponse parsingErrResponse)
            {
                this.json = json;

                if (webApiBaseResult.webApiResponseStatus == WebApiResponseStatus.WebApiReturnFail ||
                    webApiBaseResult.webApiResponseStatus == WebApiResponseStatus.Success)
                {
                    ErrorRespone errorRespone;
                    try
                    {
                        errorRespone = parsingErrResponse(webApiBaseResult.responseContent);
                        if (errorRespone.Error_code == null || errorRespone.Error_code == string.Empty)
                        {
                            this.errorCode = string.Empty;
                            this.errorMsg = webApiBaseResult.responseContent;
                        }
                        else
                        {
                            this.errorCode = errorRespone.Error_code;
                            this.errorMsg = errorRespone.Error;
                        }
                    }
                    catch (JsonSerializationException jse)
                    {
                        this.errorCode = string.Empty;
                        this.errorMsg = jse.ToString() + webApiBaseResult.responseContent;
                    }
                }
                else
                {
                    this.errorCode = string.Empty;
                    this.errorMsg = webApiBaseResult.responseContent;
                }
            }
        }

        public class ErrorRespone
        {
            private string error_code = string.Empty;
            private string error = string.Empty;

            public string Error_code
            {
                get { return this.error_code; }
                set { this.error_code = value; }
            }

            public string Error
            {
                get { return this.error; }
                set { this.error = value; }
            }
        }

        public static void SaveCustomsErrMsg(CustomsWebAPIErrMsg ErrMsg)
        {
            string saveSql = $@"
 insert into CustomsErrMsg(SystemName, CountryID, APIThread, SystemAPIThread, ErrorCode, ErrorMsg, JSON, ReSented, AddName, AddDate)
                        values(@SuppID, @ModuleName, @APIThread, @SuppAPIThread, @ErrorCode, @ErrorMsg, @JSON, 0, @AddName, GETDATE())
";
            List<SqlParameter> listPar = new List<SqlParameter>()
            {
               new SqlParameter("@SystemName", ErrMsg.SystemName),
               new SqlParameter("@CountryID", ErrMsg.CountryID),
               new SqlParameter("@APIThread", ErrMsg.apiThread),
               new SqlParameter("@SystemAPIThread", ErrMsg.SystemAPIThread),
               new SqlParameter("@ErrorCode", ErrMsg.errorCode),
               new SqlParameter("@ErrorMsg", ErrMsg.errorMsg),
               new SqlParameter("@JSON", ErrMsg.json),
               new SqlParameter("@AddName", Env.User.UserID),
            };

            Ict.DualResult result = DBProxy.Current.Execute("Production", saveSql, listPar);

            if (!result)
            {
                throw result.GetException();
            }
        }
    }
}

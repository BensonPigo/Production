using Newtonsoft.Json;
using PowerBI.Daily.PowerBI.Model;
using Sci;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using static PmsWebApiUtility20.WebApiTool;

namespace PowerBI.Daily.PowerBI.WebApi
{
    /// <inheritdoc/>
    public static class CallWebAPI
    {
        /// <summary>
        /// GetWebAPIUrl
        /// </summary>
        /// <param name="systemName">systemName</param>
        /// <returns>string</returns>
        public static string GetWebAPIUrl(string systemName)
        {
            string environment = string.Empty;

            
            if (DBProxy.Current.DefaultModuleName.ToUpper().Contains("BIN"))
            {
                return "http://172.17.3.97:16888/"; //測試走這裡
            }

            if (DBProxy.Current.DefaultModuleName.Contains("Training"))
            {
                environment = "Training";
            }

            if (DBProxy.Current.DefaultModuleName.Contains("Dummy"))
            {
                environment = "Dummy";
            }

            if (DBProxy.Current.DefaultModuleName.Contains("Formal"))
            {
                environment = "Formal";
            }
            return MyUtility.GetValue.Lookup($@"select [URL] from [Production].[dbo].[SystemWebAPIURL] with (nolock) where SystemName = '{systemName}' and Environment = '{environment}'", "Production");
        }

        public static string GetConnRegion(string systemName)
        {
            string finalDBName = systemName.ToUpper() == "PHI" ? "PH1" : systemName.ToUpper();
            if (DBProxy.Current.DefaultModuleName.ToUpper().Contains("BIN"))
            {
                return "TESTING_" + finalDBName; ;
            }

            return finalDBName + "_Formal";
        }

        /// <inheritdoc/>
        public static ResultInfo GetWebAPI<T>(string strServerName, string strAPI, int timeout, object dictionart = null)
        {
            WebApiBaseResult webApiBaseResult;
            string errorMsg = string.Empty;

            try
            {
                string apiUrl = CallWebAPI.GetWebAPIUrl(strServerName);
                Dictionary<string, string> dicHeaders = new Dictionary<string, string>();
                dicHeaders.Add("connectRegion", GetConnRegion(strServerName));
                webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(apiUrl, strAPI, dictionart, timeout, dicHeaders);
                if (!webApiBaseResult.isSuccess)
                {
                    if (webApiBaseResult.webApiResponseStatus == WebApiResponseStatus.WebApiReturnFail)
                    {
                        errorMsg = webApiBaseResult.responseContent;
                    }
                    else
                    {
                        errorMsg = webApiBaseResult.exception.ToString();
                    }

                    return new ResultInfo() { Result = errorMsg, ResultDT = new DataTable() };
                }

                string response = webApiBaseResult.responseContent;

                return new ResultInfo() { Result = errorMsg, ResultDT = ToTable<T>(response) };
            }
            catch (Exception e)
            {
                return new ResultInfo() { Result = e.ToString(), ResultDT = new DataTable() };
            }
        }

        /// <inheritdoc/>
        public static DataTable ToTable<T>(string strJson)
        {
            JsonModel<T> m = JsonConvert.DeserializeObject<JsonModel<T>>(strJson);
            return GetTable.ToDataTable<T>(m.ResultDt);
        }
    }
}

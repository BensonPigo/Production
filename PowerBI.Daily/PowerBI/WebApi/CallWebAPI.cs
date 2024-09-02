using Newtonsoft.Json;
using PowerBI.Daily.PowerBI.Model;
using Sci;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
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
        public static string GetWebAPIUrl(string systemName, bool isLAN = false)
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

            if (isLAN)
            {
                environment = "BI";
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
            string response = string.Empty;
            string apiUrl = string.Empty;
            int iCycle = 3;

            try
            {
                apiUrl = CallWebAPI.GetWebAPIUrl(strServerName);
                Dictionary<string, string> dicHeaders = new Dictionary<string, string>();
                dicHeaders.Add("connectRegion", GetConnRegion(strServerName));
                for (int i = 1; i <= iCycle; i++)
                {
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
                    }
                    else
                    {
                        response = webApiBaseResult.responseContent;
                        break;
                    }

                    if (i == 3)
                    {
                        iCycle = 6;
                        apiUrl = CallWebAPI.GetWebAPIUrl(strServerName, true);
                    }

                    Thread.Sleep(1500);

                }
                return new ResultInfo() { Result = errorMsg, ResultDT = MyUtility.Check.Empty(errorMsg) ? ToTable<T>(response) : new DataTable() };
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

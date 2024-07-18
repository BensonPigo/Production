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

            //測試記得打開

            #if DEBUG
                 return "http://172.17.3.97:16888/";
            #endif

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

        /// <inheritdoc/>
        public static DataTable GetWebAPI<T>(string strServerName, string strAPI, int timeout, Dictionary<string, string> dictionart = null)
        {
            WebApiBaseResult webApiBaseResult;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    string apiUrl = CallWebAPI.GetWebAPIUrl(strServerName);
                    webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(apiUrl, strAPI, string.Empty, timeout, dictionart);
                    if (!webApiBaseResult.isSuccess)
                    {
                        transactionScope.Dispose();
                        if (webApiBaseResult.webApiResponseStatus == WebApiResponseStatus.WebApiReturnFail)
                        {
                            MyUtility.Msg.WarningBox(webApiBaseResult.responseContent);
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox(webApiBaseResult.exception.ToString());
                        }

                        return new DataTable();
                    }

                    string response = webApiBaseResult.responseContent;
                    transactionScope.Complete();

                    return ToTable<T>(response);
                }
                catch (Exception)
                {
                    transactionScope.Dispose();
                    return new DataTable();
                }
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

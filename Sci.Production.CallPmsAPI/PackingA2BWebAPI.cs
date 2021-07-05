using Ict;
using Newtonsoft.Json;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PmsWebApiUtility20.WebApiTool;
using static Sci.Production.CallPmsAPI.PackingA2BWebAPI_Model;

namespace Sci.Production.CallPmsAPI
{
    /// <summary>
    /// PackingA2BWebAPI
    /// </summary>
    public static class PackingA2BWebAPI
    {

        /// <summary>
        /// IsDummy
        /// </summary>
        public static bool IsDummy
        {
            get
            {
                return DBProxy.Current.DefaultModuleName.Contains("testing") ||
                        DBProxy.Current.DefaultModuleName.Contains("Training") ||
                        DBProxy.Current.DefaultModuleName.Contains("Dummy");
            }
        }

        /// <summary>
        /// GetWebAPIUrl
        /// </summary>
        /// <param name="systemName">systemName</param>
        /// <returns>string</returns>
        public static string GetWebAPIUrl(string systemName)
        {
            string environment = IsDummy ? "Dummy" : "Formal";
            return MyUtility.GetValue.Lookup($"select URL from SystemWebAPIURL with (nolock) where SystemName = '{systemName}' and Environment = '{environment}'");
        }

        /// <summary>
        /// GetWebAPIUrl
        /// </summary>
        /// <param name="systemName">systemName</param>
        /// <returns>string</returns>
        public static string GetConnString(string systemName)
        {
            string environment = IsDummy ? "Dummy" : "Formal";
            return MyUtility.GetValue.Lookup($"select SqlConnection from SystemWebAPIURL with (nolock) where SystemName = '{systemName}' and Environment = '{environment}'");
        }

        public static string GetWebApiBaseResultError(WebApiBaseResult webApiBaseResult)
        {
            if (webApiBaseResult.exception != null)
            {
                return webApiBaseResult.exception.ToString();
            }
            else
            {
                return webApiBaseResult.responseContent;
            }
        }

        /// <summary>
        /// GetRegionFactory
        /// </summary>
        /// <param name="systemName">systemName</param>
        /// <param name="dtResult">dtResult</param>
        /// <returns>DualResult</returns>
        public static DualResult GetRegionFactory(string systemName, out DataTable dtResult)
        {
            try
            {
                string apiUrl = GetWebAPIUrl(systemName);
                WebApiBaseResult webApiBaseResult;
                webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiGet(apiUrl, "api/PackingA2B/GetRegionFactory", 30);

                if (!webApiBaseResult.isSuccess)
                {
                    dtResult = null;
                    return new DualResult(false, GetWebApiBaseResultError(webApiBaseResult));
                }

                List<RegionFactory> regionFactories = PmsWebApiUtility20.Json.ConvertToObject<List<RegionFactory>>(webApiBaseResult.responseContent);
                dtResult = ToDataTable<RegionFactory>(regionFactories);
                return new DualResult(true);
            }
            catch (Exception ex)
            {
                dtResult = null;
                return new DualResult(false, ex);
            }
        }


        public static DualResult GetP05_ImportFromPackingListQuery(string systemName, P05_ImportFromPackingListQuery p05_ImportFromPackingList, out DataTable dtResult)
        {
            try
            {
                string apiUrl = GetWebAPIUrl(systemName);
                WebApiBaseResult webApiBaseResult;
                string postBody = PmsWebApiUtility20.Json.ConvertToJson(p05_ImportFromPackingList);
                webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(apiUrl, "api/PackingA2B/GetP05_ImportFromPackingListQuery", postBody, 60);

                if (!webApiBaseResult.isSuccess)
                {
                    dtResult = null;
                    return new DualResult(false, GetWebApiBaseResultError(webApiBaseResult));
                }

                List<P05_ImportFromPackingListQueryResult> resultList = PmsWebApiUtility20.Json.ConvertToObject<List<P05_ImportFromPackingListQueryResult>>(webApiBaseResult.responseContent);
                dtResult = ToDataTable<P05_ImportFromPackingListQueryResult>(resultList);
                return new DualResult(true);
            }
            catch (Exception ex)
            {
                dtResult = null;
                return new DualResult(false, ex);
            }
        }

        /// <summary>
        /// GetDataBySql
        /// </summary>
        /// <param name="systemName">systemName</param>
        /// <param name="sqlString">sqlString</param>
        /// <param name="dtResult">dtResult</param>
        /// <returns>DualResult</returns>
        public static DualResult GetDataBySql<T>(string systemName, DataBySql dataBySql, out DataTable dtResult)
        {
            try
            {
                string apiUrl = GetWebAPIUrl(systemName);
                WebApiBaseResult webApiBaseResult;
                dataBySql.SqlString = dataBySql.SqlString.Base64Encrypt();
                webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(apiUrl, "api/PackingA2B/GetDataBySql", dataBySql, 75);
                string aa = JsonConvert.SerializeObject(dataBySql);
                if (!webApiBaseResult.isSuccess)
                {
                    dtResult = null;
                    return new DualResult(false, GetWebApiBaseResultError(webApiBaseResult));
                }

                List<T> resultList = PmsWebApiUtility20.Json.ConvertToObject<List<T>>(webApiBaseResult.responseContent);
                dtResult = ToDataTable<T>(resultList);
                return new DualResult(true);
            }
            catch (Exception ex)
            {
                dtResult = null;
                return new DualResult(false, ex);
            }
        }

    }
}

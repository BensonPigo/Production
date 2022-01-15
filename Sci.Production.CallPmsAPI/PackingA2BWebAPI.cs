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
        public class PackingA2BResult : DualResult
        {
            public bool isDataExists { get; set; } = false;


            public PackingA2BResult(bool result) : base(result)
            {
                this.isDataExists = isDataExists;
            }

            public PackingA2BResult(bool result, bool isDataExists) : base(result)
            {
                this.isDataExists = isDataExists;
            }

            public PackingA2BResult(bool result, Exception exception) : base(result, exception)
            {
            }

            public PackingA2BResult(bool result, string description) : base(result, description)
            {
            }
        }

        /// <summary>
        /// GetWebAPIUrl
        /// </summary>
        /// <param name="systemName">systemName</param>
        /// <returns>string</returns>
        public static string GetWebAPIUrl(string systemName)
        {
            string environment = string.Empty;

            if (DBProxy.Current.DefaultModuleName.Contains("testing"))
            {
                environment = "Testing";
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

            return MyUtility.GetValue.Lookup($"select URL from SystemWebAPIURL with (nolock) where SystemName = '{systemName}' and Environment = '{environment}'");
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

                if (MyUtility.Check.Empty(apiUrl))
                {
                    dtResult = new DataTable();
                    return new DualResult(true);
                }

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

        public static string GetPLFromRgCodeByPackID(string packID)
        {
            string sqlGetPLFromRgCode = $"select distinct PLFromRgCode from GMTBooking_Detail with (nolock) where PackingListID = '{packID}'";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetPLFromRgCode, out dtResult);
            if (!result)
            {
                throw result.GetException();
            }

            if (dtResult.Rows.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return dtResult.AsEnumerable().Select(s => s["PLFromRgCode"].ToString()).First(); ;
            }
        }

        public static List<string> GetAllPLFromRgCode()
        {
            string sqlGetPLFromRgCode = $@"
select distinct PLFromRgCode 
from GMTBooking_Detail with (nolock) ";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetPLFromRgCode, out dtResult);
            if (!result)
            {
                throw result.GetException();
            }

            if (dtResult.Rows.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                return dtResult.AsEnumerable().Select(s => s["PLFromRgCode"].ToString()).ToList(); ;
            }
        }

        public static List<string> GetPLFromRgCodeByPulloutID(string pulloutID)
        {
            string sqlGetPLFromRgCode = $@"
select distinct PLFromRgCode 
from GMTBooking_Detail with (nolock) 
where ID in (select InvNo from Pullout_Detail with (nolock) where ID = '{pulloutID}')";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetPLFromRgCode, out dtResult);
            if (!result)
            {
                throw result.GetException();
            }

            if (dtResult.Rows.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                return dtResult.AsEnumerable().Select(s => s["PLFromRgCode"].ToString()).ToList(); ;
            }
        }

        public static List<string> GetPLFromRgCodeByMutiInvNo(List<string> listInvNo)
        {
            if (listInvNo == null || listInvNo.Count == 0)
            {
                return new List<string>();
            }

            string sqlGetPLFromRgCode = $"select distinct PLFromRgCode from GMTBooking_Detail with (nolock) where ID in ({listInvNo.Select(s => $"'{s}'").JoinToString(",")})";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetPLFromRgCode, out dtResult);
            if (!result)
            {
                throw result.GetException();
            }

            if (dtResult.Rows.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                return dtResult.AsEnumerable().Select(s => s["PLFromRgCode"].ToString()).ToList(); ;
            }
        }

        public static List<string> GetPLFromRgCodeByInvNo(string InvNo)
        {
            string sqlGetPLFromRgCode = $"select distinct PLFromRgCode from GMTBooking_Detail with (nolock) where ID = '{InvNo}'";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetPLFromRgCode, out dtResult);
            if (!result)
            {
                throw result.GetException();
            }

            if (dtResult.Rows.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                return dtResult.AsEnumerable().Select(s => s["PLFromRgCode"].ToString()).ToList(); ;
            }
        }

        public static List<string> GetPLFromRgCodeByShipPlanID(string shipPlanID)
        {
            string sqlGetPLFromRgCode = $@"
select distinct gd.PLFromRgCode 
from GMTBooking_Detail gd with (nolock) 
where exists(select 1 from GMTBooking g with (nolock) where g.ShipPlanID = '{shipPlanID}' and g.ID = gd.ID)";

            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetPLFromRgCode, out dtResult);
            if (!result)
            {
                throw result.GetException();
            }

            if (dtResult.Rows.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                return dtResult.AsEnumerable().Select(s => s["PLFromRgCode"].ToString()).ToList(); ;
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

        public static DualResult GetDataBySql(string systemName, DataBySql dataBySql, out DataTable dtResult)
        {
            try
            {
                string apiUrl = GetWebAPIUrl(systemName);
                WebApiBaseResult webApiBaseResult;
                dataBySql.SqlString = dataBySql.SqlString.Base64Encrypt();
                webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(apiUrl, "api/PackingA2B/GetDataBySql", dataBySql, 75);

                if (!webApiBaseResult.isSuccess)
                {
                    dtResult = null;
                    return new DualResult(false, GetWebApiBaseResultError(webApiBaseResult));
                }

                dtResult = (DataTable)JsonConvert.DeserializeObject(webApiBaseResult.responseContent, (typeof(DataTable)));

                return new DualResult(true);
            }
            catch (Exception ex)
            {
                dtResult = null;
                return new DualResult(false, ex);
            }
        }

        public static DualResult ExecuteBySql(string systemName, DataBySql dataBySql)
        {
            try
            {
                string apiUrl = GetWebAPIUrl(systemName);
                WebApiBaseResult webApiBaseResult;
                dataBySql.SqlString = dataBySql.SqlString.Base64Encrypt();
                webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(apiUrl, "api/PackingA2B/GetDataBySql", dataBySql, 75);

                if (!webApiBaseResult.isSuccess)
                {
                    return new DualResult(false, GetWebApiBaseResultError(webApiBaseResult));
                }

                return new DualResult(true);
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }
        }

        public static PackingA2BResult SeekBySql(string systemName, DataBySql dataBySql, out DataRow drResult)
        {
            try
            {
                string apiUrl = GetWebAPIUrl(systemName);
                WebApiBaseResult webApiBaseResult;
                dataBySql.SqlString = dataBySql.SqlString.Base64Encrypt();
                webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(apiUrl, "api/PackingA2B/SeekBySql", dataBySql, 75);

                if (!webApiBaseResult.isSuccess)
                {
                    drResult = null;
                    return new PackingA2BResult(false, GetWebApiBaseResultError(webApiBaseResult));
                }


                SeekDataResult seekDataResult = JsonConvert.DeserializeObject<SeekDataResult>(webApiBaseResult.responseContent);
                if (seekDataResult.isExists)
                {
                    drResult = seekDataResult.resultDt.Rows[0];
                }
                else
                {
                    drResult = null;
                }
                return new PackingA2BResult(true, seekDataResult.isExists);
            }
            catch (Exception ex)
            {
                drResult = null;
                return new PackingA2BResult(false, ex);
            }
        }

        public static PackingA2BResult SeekBySql(string systemName, string seekSel, out DataRow drResult)
        {
            DataBySql dataBySql = new DataBySql() { SqlString = seekSel };
            return SeekBySql(systemName, dataBySql, out drResult);
        }

        public static PackingA2BResult SeekBySql(string systemName, string seekSel)
        {
            DataRow noUsingDr;
            DataBySql dataBySql = new DataBySql() { SqlString = seekSel };
            return SeekBySql(systemName, dataBySql, out noUsingDr);
        }

        public static List<PackingA2BResult> SeekBySql(string[] systemNames, string seekSel)
        {
            List<PackingA2BResult> packingA2BResults = new List<PackingA2BResult>();

            foreach (string systemName in systemNames)
            {
                DataRow noUsingDr;
                DataBySql dataBySql = new DataBySql() { SqlString = seekSel };
                packingA2BResults.Add(SeekBySql(systemName, dataBySql, out noUsingDr));
            }


            return packingA2BResults;
        }

        public static List<PackingA2BResult> SeekBySql(Dictionary<string, string> dicSeekSql)
        {
            List<PackingA2BResult> packingA2BResults = new List<PackingA2BResult>();

            foreach (KeyValuePair<string, string> seekSql in dicSeekSql)
            {
                DataRow noUsingDr;
                DataBySql dataBySql = new DataBySql() { SqlString = seekSql.Value };
                packingA2BResults.Add(SeekBySql(seekSql.Key, dataBySql, out noUsingDr));
            }


            return packingA2BResults;
        }

        public static DualResult GetDataBySql<T>(string systemName, string sqlCmd, out DataTable dtResult)
        {
            DataBySql dataBySql = new DataBySql() { SqlString = sqlCmd };
            return GetDataBySql<T>(systemName, dataBySql, out dtResult);
        }

        public static DualResult GetDataBySql(string systemName, string sqlCmd, out DataTable dtResult)
        {
            DataBySql dataBySql = new DataBySql() { SqlString = sqlCmd };
            return GetDataBySql(systemName, dataBySql, out dtResult);
        }

        public static DualResult ExecuteBySql(string systemName, string sqlCmd)
        {
            DataBySql dataBySql = new DataBySql() { SqlString = sqlCmd };
            return ExecuteBySql(systemName, dataBySql);
        }

        public static void AddSqlCmdByPLFromRgCode(this Dictionary<string, List<string>> srcDic, string plFromRgCode, string sqlCmd)
        {
            if (!srcDic.ContainsKey(plFromRgCode))
            {
                srcDic.Add(plFromRgCode, new List<string>());
            }

            srcDic[plFromRgCode].Add(sqlCmd);
        }

    }
}

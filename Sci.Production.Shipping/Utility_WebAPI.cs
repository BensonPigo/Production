using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ict;
using Newtonsoft.Json;
using PmsWebApiUtility20;
using Sci.Data;
using Sci.Production.Prg;
using static PmsWebApiUtility20.WebApiTool;
using static PmsWebApiUtility45.WebApiTool;
using static Sci.Production.Shipping.Customs_WebAPI;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public static class Utility_WebAPI
    {
        /// <inheritdoc/>
        public static string Country => "VN";

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

        /// <inheritdoc/>
        public static string SystemName;

        /// <summary>
        /// check SystemWebAPIURL is enable or exists 
        /// </summary>
        /// <param name="factoryID"> Factory ID</param>
        /// <param name="country"> Region Code</param>
        /// <returns>bool</returns>
        public static bool IsSystemWebAPIEnable(string factoryID, string country)
        {
            SystemName = factoryID;
            return MyUtility.Check.Empty($@"select * from SystemWebAPIURL where SystemName='{factoryID}' and CountryID='{country}' and Environment = '{ModuleType}' and Junk = 0");
        }

        /// <inheritdoc/>
        public static string GetSciUrl()
        {
            return MyUtility.GetValue.Lookup(
                $@"
select URL from SystemWebAPIURL with (nolock)
where SystemName = '{SystemName}' and CountryID = '{Country}' and Environment = '{ModuleType}' and Junk = 0", "Production");
        }

        /// <inheritdoc/>
        public static bool GetContractNo(out ListContract dataMode)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string systemAPIThread = "api/Shipping/GetContractNo";
                    Uri uri = new Uri(GetSciUrl() + systemAPIThread);
                    var json = client.DownloadString(uri);
                    dataMode = JsonConvert.DeserializeObject<ListContract>(json);
                }
            }
            catch (Exception ex)
            {
                dataMode = null;
                MyUtility.Msg.WarningBox(ex.Message);
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool GetCustomsCopyLoad(string styleID, string brandID, string contractNO, out ListCustomsCopyLoad dataMode)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string systemAPIThread = "api/Shipping/GetCustomsCopyLoad";
                    string apiParemeter = $@"?StyleID={styleID}&BrandID={brandID}&ContractNo={contractNO}";

                    Uri uri = new Uri(GetSciUrl() + systemAPIThread + apiParemeter);
                    var json = client.DownloadString(uri);
                    dataMode = JsonConvert.DeserializeObject<ListCustomsCopyLoad>(json);
                }
            }
            catch (Exception ex)
            {
                dataMode = null;
                MyUtility.Msg.WarningBox(ex.Message);
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool GetCustomsAllData(string styleID, string brandID, string contractNO, out ListCustomsAllData dataMode)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string systemAPIThread = "api/Shipping/GetCustomsAllData";
                    string apiParemeter = $@"?StyleID={styleID}&BrandID={brandID}&ContractNo={contractNO}";

                    Uri uri = new Uri(GetSciUrl() + systemAPIThread + apiParemeter);
                    var json = client.DownloadString(uri);
                    string convtJson = Regex.Replace(json, @"(\d+\/\d+)""", "$1\\\"");

                    dataMode = JsonConvert.DeserializeObject<ListCustomsAllData>(convtJson);
                    //dataMode = JsonConvert.DeserializeObject<Customs_WebAPI.ListCustomsAllData>(json);
                }
            }
            catch (Exception ex)
            {
                dataMode = null;
                MyUtility.Msg.WarningBox(ex.Message);
                return false;
            }

            return true;
        }

    }
}

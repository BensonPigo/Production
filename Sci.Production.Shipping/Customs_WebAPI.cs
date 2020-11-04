using Sci.Production.Automation;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using static Sci.Production.Shipping.Utility_WebAPI;
using System.Data;

namespace Sci.Production.Shipping
{
    public class Customs_WebAPI
    {
        private static readonly string FactoryID;
        private static readonly string Country;
        private CustomsWebAPIErrMsg customsErrMsg = new CustomsWebAPIErrMsg();

        public static bool IsCustoms_WebAPIEnable => Utility_WebAPI.IsSystemWebAPIEnable(FactoryID, Country);

        public void SentCustoms_load(DataTable funListCustomsKey)
        {
            if (funListCustomsKey.Rows.Count == 0)
            {
                return;
            }

            string apiThread = "SentDeleteWorkOrderFromAGV";
            string systemAPIThread = "api/GuoziAGV/SentDeleteDataByApiTag";
            this.customsErrMsg.apiThread = apiThread;
            this.customsErrMsg.SystemAPIThread = systemAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject.CustomsMasterKey = funListCustomsKey;
            string jsonBody = JsonConvert.SerializeObject(UtilityAutomation.AppendBaseInfo(bodyObject, "WorkOrder_Distribute"));

            Utility_WebAPI.SendWebAPI(UtilityAutomation.GetSciUrl(), systemAPIThread, jsonBody, this.customsErrMsg);
        }

        private object CreateStructure(string tableName, object structureID)
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

        public class CustomsMasterKey
        {
            public string StyleID { get; set; }

            public string BrandID { get; set; }

            public string ContractNo { get; set; }
        }
    }
}
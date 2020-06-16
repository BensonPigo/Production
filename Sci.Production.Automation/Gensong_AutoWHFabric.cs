using Ict;
using Newtonsoft.Json;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sci.Production.Automation.UtilityAutomation;

namespace Sci.Production.Automation
{
    public class Gensong_AutoWHFabric
    {
        private static readonly string GensongSuppID = "3A0174";
        private static readonly string moduleName = "AutoWHFabric";
        private AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS();

        public static bool IsGensong_AutoWHFabricEnable
        {
            get { return IsModuleAutomationEnable(GensongSuppID, moduleName); }
        }

        /// <summary>
        /// Receive_Detail
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        public void SentReceive_DetailToGensongAutoWHFabric(DataTable dtDetail)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentReceiving_DetailToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtDetail.AsEnumerable()
                .Select(dr => new
                {
                    ID = dr["ID"].ToString(),
                    InvNo = dr["InvNo"].ToString(),
                    PoId = dr["PoId"].ToString(),
                    Seq1 = dr["Seq1"].ToString(),
                    Seq2 = dr["Seq2"].ToString(),
                    Refno = dr["Refno"].ToString(),
                    ColorID = dr["ColorID"].ToString(),
                    Roll = dr["Roll"].ToString(),
                    Dyelot = dr["Dyelot"].ToString(),
                    StockUnit = dr["StockUnit"].ToString(),
                    StockQty = (decimal)dr["StockQty"],
                    PoUnit = dr["PoUnit"].ToString(),
                    ShipQty = (decimal)dr["ShipQty"],
                    Weight = (decimal)dr["Weight"],
                    StockType = dr["StockType"].ToString(),
                    Ukey = (long)dr["Ukey"],
                    IsInspection = (int)dr["IsInspection"],
                    Junk = (int)dr["Junk"],
                    CmdTime = DateTime.Now
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("Receiving_Detail", bodyObject));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Issue_Detail To Gensong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        public void SentIssue_DetailToGensongAutoWHFabric(DataTable dtDetail)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentIssue_DetailToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtDetail.AsEnumerable()
                .Select(dr => new
                {
                    Id = dr["id"].ToString(),
                    Type = dr["Type"].ToString(),
                    CutPlanID = dr["CutPlanID"].ToString(),
                    EstCutdate = (DateTime?)dr["EstCutdate"],
                    SpreadingNoID = dr["SpreadingNoID"].ToString(),
                    PoId = dr["PoId"].ToString(),
                    Seq1 = dr["Seq1"].ToString(),
                    Seq2 = dr["Seq2"].ToString(),
                    Roll = dr["Roll"].ToString(),
                    Dyelot = dr["Dyelot"].ToString(),
                    Barcode = dr["Barcode"].ToString(),
                    Qty = (decimal)dr["Qty"],
                    Ukey = (long)dr["Ukey"],
                    CmdTime = DateTime.Now
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("Issue_Detail", bodyObject));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// WHClose To Gensong
        /// </summary>
        /// <param name="strKey">PoID</param>
        public void SentWHCloseToGensongAutoWHFabric(DataTable dtDetail)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentWHCloseToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            var structureID = //strKey.Split(',').Select(s => new { POID = s });

            //string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("WHClose", structureID));
            //SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SubTransfer_Detail To Gensong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        public void SentSubTransfer_DetailToGensongAutoWHFabric(DataTable dtDetail)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentSubTransfer_DetailToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtDetail.AsEnumerable()
                .Select(s => new
                {
                    Ukey = s["Ukey"].ToString()
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("SubTransfer_Detail", bodyObject));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// MtlLocation To Gensong
        /// </summary>
        /// <param name="strKey">ID</param>
        public void SentMtlLocationToGensongAutoWHFabric(string strKey)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentMtlLocationToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            var structureID = strKey.Split(',').Select(s => new { ID = s });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("MtlLocation", structureID));
            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// RefnoRelaxtime To Gensong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        public void SentRefnoRelaxtimeToGensongAutoWHFabric(DataTable dtDetail)
        {
            /*
             *記得更新Function 也要一併更新到MES Automation.Dll檔
             */
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentMtlLocationToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtDetail.AsEnumerable()
                .Select(s => new
                {
                    Refno = s["Refno"].ToString()
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("RefnoRelaxtime", bodyObject));
            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Cutplan_Detail To Gensong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        public void SentCutplan_DetailToGensongAutoWHFabric(DataTable dtDetail)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentMtlLocationToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtDetail.AsEnumerable()
                .Select(s => new
                {
                    ID = s["ID"].ToString(),
                    WorkorderUkey = MyUtility.Convert.GetInt(s["WorkorderUkey"])
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("Cutplan_Detail", bodyObject));
            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        private object CreateGensongStructure(string tableName, object structureID)
        {
            Dictionary<string, object> resultObj = new Dictionary<string, object>();
            resultObj.Add("TableArray", new string[] { tableName });

            Dictionary<string, object> dataStructure = new Dictionary<string, object>();
            dataStructure.Add(tableName, structureID);
            resultObj.Add("DataTable", dataStructure);

            return resultObj;
        }

        public class Receiving_DetailToGensong_PostBody
        {
            public DataTable Receiving_Detail;
            public string ID;
            public string InvNo;
            public string PoId;
            public string Seq1;
            public string Seq2;
            public string Refno;
            public string ColorID;
            public string Roll;
            public string Dyelot;
            public string StockUnit;
            public decimal StockQty;
            public string PoUnit;
            public decimal ShipQty;
            public decimal Weight;
            public string StockType;
            public long Ukey;
            public bool IsInspection;
            public bool Junk;
            public DateTime? CmdTime;
        }

    }
}

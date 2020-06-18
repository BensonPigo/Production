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
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
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
                    IsInspection = (bool)dr["IsInspection"],
                    Junk = (bool)dr["Junk"],
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
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
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
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
                    EstCutdate = MyUtility.Check.Empty(dr["EstCutdate"]) ? null : (DateTime?)dr["EstCutdate"],
                    SpreadingNoID = dr["SpreadingNoID"].ToString(),
                    PoId = dr["PoId"].ToString(),
                    Seq1 = dr["Seq1"].ToString(),
                    Seq2 = dr["Seq2"].ToString(),
                    Roll = dr["Roll"].ToString(),
                    Dyelot = dr["Dyelot"].ToString(),
                    Barcode = dr["Barcode"].ToString(),
                    Qty = (decimal)dr["Qty"],
                    Ukey = (long)dr["Ukey"],
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("Issue_Detail", bodyObject));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// WHClose To Gensong
        /// </summary>
        /// <param name="strKey">PoID</param>
        public void SentWHCloseToGensongAutoWHFabric(DataTable dtMaster)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentWHCloseToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtMaster.AsEnumerable()
                .Select(dr => new
                {
                    POID = dr["POID"].ToString(),
                    WhseClose = MyUtility.Check.Empty(dr["WhseClose"]) ? null : (DateTime?)dr["WhseClose"],
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("WHClose", bodyObject));
            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SubTransfer_Detail To Gensong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        public void SentSubTransfer_DetailToGensongAutoWHFabric(DataTable dtMaster)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentSubTransfer_DetailToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = @"
select distinct 
[ID] = sd.ID
,s.Type
,sd.FromPOID,sd.FromSeq1,sd.FromSeq2,sd.FromRoll,sd.FromDyelot,sd.FromStockType
,sd.ToPOID,sd.ToSeq1,sd.ToSeq2,sd.ToRoll,sd.ToDyelot,sd.ToStockType
,fty.Barcode,sd.Ukey
,CmdTime = GetDate()
from Production.dbo.SubTransfer_Detail sd
inner join #tmp s on sd.ID=s.Id
outer apply(
		select Barcode
		from Production.dbo.FtyInventory
		where POID = sd.ToPOID and Seq1=sd.ToSeq1
		and Seq2=sd.ToSeq2 and Roll=sd.ToRoll and Dyelot=sd.ToDyelot
	)fty
where 1=1
and exists(
		select 1 from Production.dbo.PO_Supp_Detail 
		where id = sd.ToPOID and seq1=sd.ToSeq1 and seq2=sd.ToSeq2 
		and FabricType='F'
	)
and s.Type in ('A','B','D')
";
            DataTable dt = new DataTable();
            MyUtility.Tool.ProcessWithDatatable(dtMaster, null, sqlcmd, out dt);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dt.AsEnumerable()
                .Select(dr => new
                {
                    ID = dr["ID"].ToString(),
                    Type = dr["Type"].ToString(),
                    FromPOID = dr["FromPOID"].ToString(),
                    FromSeq1 = dr["FromSeq1"].ToString(),
                    FromSeq2 = dr["FromSeq2"].ToString(),
                    FromRoll = dr["FromRoll"].ToString(),
                    FromDyelot = dr["FromDyelot"].ToString(),
                    FromStockType = dr["FromStockType"].ToString(),
                    ToPOID = dr["ToPOID"].ToString(),
                    ToSeq1 = dr["ToSeq1"].ToString(),
                    ToSeq2 = dr["ToSeq2"].ToString(),
                    ToRoll = dr["ToRoll"].ToString(),
                    ToDyelot = dr["ToDyelot"].ToString(),
                    ToStockType = dr["ToStockType"].ToString(),
                    Barcode = dr["Barcode"].ToString(),
                    Ukey = (long)dr["Ukey"],
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("SubTransfer_Detail", bodyObject));

            SendWebAPI(UtilityAutomation.GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// MtlLocation To Gensong
        /// </summary>
        /// <param name="strKey">ID</param>
        public void SentMtlLocationToGensongAutoWHFabric(DataTable dtMaster)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentMtlLocationToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;
            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtMaster.AsEnumerable()
                .Select(dr => new
                {
                    ID = dr["ID"].ToString(),
                    StockType = dr["StockType"].ToString(),
                    Junk = (bool)dr["Junk"],
                    Description = dr["Description"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("MtlLocation", bodyObject));
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
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentRefnoRelaxtimeToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtDetail.AsEnumerable()
                .Select(s => new
                {
                    Refno = s["Refno"].ToString(),
                    FabricRelaxationID = s["FabricRelaxationID"].ToString(),
                    RelaxTime = s["RelaxTime"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
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
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentCutplan_DetailToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = @"
select distinct
    [CutplanID] = cp2.ID
    ,[CutRef] = cp2.CutRef
    ,[CutNo] = cp2.CutNo
    ,[PoID] = wo.ID
    ,[Seq1] = wo.SEQ1
    ,[Seq2] = wo.SEQ2
    ,[Refno] = wo.Refno
    ,[Article] = Article.value
    ,[Colorid] = cp2.Colorid
    ,[SizeCode] = SizeCode.value
    ,cp2.WorkorderUkey
	,[CmdTime] = GETDATE()
    from  Production.dbo.Cutplan_Detail cp2
    inner join Production.dbo.Cutplan cp1 on cp2.id = cp1.id
    inner join Production.dbo.WorkOrder wo on cp2.POID = wo.ID and cp2.CutRef = wo.CutRef
    outer apply(
        select value = STUFF((
        	select CONCAT(',',SizeCode)
        	from(
        		select distinct SizeCode
        		from Production.dbo.WorkOrder_Distribute
        		where WorkOrderUkey = wo.Ukey
        		)s
        		for xml path('')
        	),1,1,'')
    ) SizeCode
    outer apply(
        select value = STUFF((
        	select CONCAT(',',Article)
        	from(
        		select distinct Article
        		from Production.dbo.WorkOrder_Distribute
        		where WorkOrderUkey = wo.Ukey
        		and Article !=''
        		)s
        		for xml path('')
        	),1,1,'')
    ) Article
    where exists(
        select 1 from Issue_Detail where cp2.ID = CutplanID
    )
	and exists(
		select 1 from #tmp s where s.ID = cp2.ID and s.WorkorderUkey = cp2.WorkorderUkey
	)
";
            DataTable dt = new DataTable();
            MyUtility.Tool.ProcessWithDatatable(dtDetail, null, sqlcmd, out dt);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dt.AsEnumerable()
                .Select(s => new
                {
                    CutplanID = s["CutplanID"].ToString(),
                    CutRef = s["CutRef"].ToString(),
                    CutNo = (decimal)s["CutNo"],
                    POID = s["POID"].ToString(),
                    Seq1 = s["Seq1"].ToString(),
                    Seq2 = s["Seq2"].ToString(),
                    Refno = s["Refno"].ToString(),
                    Article = s["Article"].ToString(),
                    ColorID = s["ColorID"].ToString(),
                    SizeCode = s["SizeCode"].ToString(),
                    WorkorderUkey = (long)s["WorkorderUkey"],
                    CmdTime = DateTime.Now
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

    }
}

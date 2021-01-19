using Ict;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using static Sci.Production.Automation.UtilityAutomation;

namespace Sci.Production.Automation
{
    /// <inheritdoc/>
    public class Gensong_AutoWHFabric
    {
        private static readonly string GensongSuppID = "3A0174";
        private static readonly string moduleName = "AutoWHFabric";
        private AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS();

        /// <inheritdoc/>
        public static bool IsGensong_AutoWHFabricEnable => IsModuleAutomationEnable(GensongSuppID, moduleName);

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
                    Barcode = dr["Barcode"].ToString(),
                    Ukey = (long)dr["Ukey"],
                    IsInspection = (bool)dr["IsInspection"],
                    ETA = MyUtility.Check.Empty(dr["ETA"]) ? null : ((DateTime?)dr["ETA"]).Value.ToString("yyyy/MM/dd"),
                    WhseArrival = MyUtility.Check.Empty(dr["WhseArrival"]) ? null : ((DateTime?)dr["WhseArrival"]).Value.ToString("yyyy/MM/dd"),
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("Receiving_Detail", bodyObject));

            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
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
                    EstCutdate = MyUtility.Check.Empty(dr["EstCutdate"]) ? null : ((DateTime?)dr["EstCutdate"]).Value.ToString("yyyy/MM/dd"),
                    SpreadingNoID = dr["SpreadingNoID"].ToString(),
                    PoId = dr["PoId"].ToString(),
                    Seq1 = dr["Seq1"].ToString(),
                    Seq2 = dr["Seq2"].ToString(),
                    Roll = dr["Roll"].ToString(),
                    Dyelot = dr["Dyelot"].ToString(),
                    Barcode = dr["Barcode"].ToString(),
                    NewBarcode = dr["NewBarcode"].ToString(),
                    Qty = (decimal)dr["Qty"],
                    Ukey = (long)dr["Ukey"],
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("Issue_Detail", bodyObject));

            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// WHClose To Gensong
        /// </summary>
        /// <param name="dtMaster">Master DataSource</param>
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
                    WhseClose = MyUtility.Check.Empty(dr["WhseClose"]) ? null : ((DateTime?)dr["WhseClose"]).Value.ToString("yyyy/MM/dd"),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("WHClose", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SubTransfer_Detail To Gensong
        /// </summary>
        /// <param name="dtMaster">Master DataSource</param>
        /// <param name="isConfirmed">bool</param>
        public void SentSubTransfer_DetailToGensongAutoWHFabric(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentSubTransfer_DetailToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select distinct
[ID] = sd.ID
,Type = 'D'
,sd.FromPOID,sd.FromSeq1,sd.FromSeq2,sd.FromRoll,sd.FromDyelot,sd.FromStockType
,[FromBarcode] = FromBarcode.value
,[FromLocation] = Fromlocation.listValue
,sd.ToPOID,sd.ToSeq1,sd.ToSeq2,sd.ToRoll,sd.ToDyelot,sd.ToStockType
,[ToBarcode] = ToBarcode.value
,[ToLocation] = sd.ToLocation
,sd.Ukey
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,CmdTime = GetDate()
from Production.dbo.SubTransfer_Detail sd
inner join #tmp s on sd.ID=s.Id
left join FtyInventory FI on sd.fromPoid = fi.poid 
    and sd.fromSeq1 = fi.seq1 and sd.fromSeq2 = fi.seq2 
    and sd.fromDyelot = fi.Dyelot and sd.fromRoll = fi.roll 
    and sd.fromStocktype = fi.stocktype
outer apply(
	select listValue = Stuff((
			select concat(',',MtlLocationID)
			from (
					select 	distinct
						fd.MtlLocationID
					from FtyInventory_Detail fd
					where fd.Ukey = fi.Ukey
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = fi.Ukey
)FromBarcode
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	inner join FtyInventory toFi on toFi.Ukey = fb.Ukey
	where toFi.POID = sd.ToPOID
	and tofi.Seq1= sd.ToSeq1 and toFi.Seq2 = sd.ToSeq2
	and toFi.Roll = sd.ToRoll and toFi.Dyelot = sd.ToDyelot
	and toFi.StockType = sd.ToStockType
)ToBarcode
where 1=1
and exists(
    select 1 from Production.dbo.PO_Supp_Detail
    where id = sd.ToPOID and seq1=sd.ToSeq1 and seq2=sd.ToSeq2
    and FabricType='F'
)
and exists(
	select 1
	from MtlLocation ml 
	inner join dbo.SplitString(Fromlocation.listValue,',') sp on sp.Data = ml.ID 
		and ml.StockType=sd.FromStockType
	where ml.IsWMS = 1

	union all

    select 1 from MtlLocation ml 
	inner join dbo.SplitString(sd.ToLocation,',') sp on sp.Data = ml.ID
	and ml.StockType=sd.ToStockType
	where ml.IsWMS = 1
)

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
                    FromBarcode = dr["FromBarcode"].ToString(),
                    FromLocation = dr["FromLocation"].ToString(),
                    ToPOID = dr["ToPOID"].ToString(),
                    ToSeq1 = dr["ToSeq1"].ToString(),
                    ToSeq2 = dr["ToSeq2"].ToString(),
                    ToRoll = dr["ToRoll"].ToString(),
                    ToDyelot = dr["ToDyelot"].ToString(),
                    ToStockType = dr["ToStockType"].ToString(),
                    ToBarcode = dr["ToBarcode"].ToString(),
                    ToLocation = dr["ToLocation"].ToString(),
                    Ukey = (long)dr["Ukey"],
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("SubTransfer_Detail", bodyObject));

            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// MtlLocation To Gensong
        /// </summary>
        /// <param name="dtMaster">Master DataSource</param>
        public void SentMtlLocationToGensongAutoWHFabric(DataTable dtMaster)
        {
            // ISP20201856 已移除此功能
            if (true)
            {
                return; // 暫未開放
            }

            /*
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
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
            */
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
                    RelaxTime = (decimal)s["RelaxTime"],
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("RefnoRelaxtime", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Cutplan_Detail To Gensong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="isConfirmed">bool</param>
        public void SentCutplan_DetailToGensongAutoWHFabric(DataTable dtDetail, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentCutplan_DetailToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
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
    ,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
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
                    Status = s["Status"].ToString(),
                    CmdTime = DateTime.Now,
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("Cutplan_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// BorrowBack to Gensong
        /// </summary>
        /// <param name="dtDetail">bool</param>
        /// <param name="isConfirmed">bool Confirmed</param>
        public void SentBorrowBackToGensongAutoWHFabric(DataTable dtDetail, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentBorrowBackToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select distinct
[ID] = bb2.ID
,bb2.FromPOID,bb2.FromSeq1,bb2.FromSeq2,bb2.FromRoll,bb2.FromDyelot,bb2.FromStockType
,[FromBarcode] = FromBarcode.value
,[FromLocation] = Fromlocation.listValue
,bb2.ToPOID,bb2.ToSeq1,bb2.ToSeq2,bb2.ToRoll,bb2.ToDyelot,bb2.ToStockType
,[ToBarcode] = ToBarcode.value
,[ToLocation] = bb2.ToLocation
,bb2.Qty
,bb2.Ukey
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,CmdTime = GetDate()
from Production.dbo.BorrowBack_Detail bb2
inner join #tmp bb on bb.ID=bb2.Id
left join FtyInventory FI on bb2.FromPoid = Fi.Poid and bb2.FromSeq1 = Fi.Seq1 and bb2.FromSeq2 = Fi.Seq2 
    and bb2.FromRoll = Fi.Roll and bb2.FromDyelot = Fi.Dyelot and bb2.FromStockType = FI.StockType
outer apply(
	select listValue = Stuff((
			select concat(',',FromLocation)
			from (
					select 	distinct
						l.FromLocation
					from dbo.LocationTrans_detail l
					where l.FtyInventoryUkey = fi.Ukey
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = fi.Ukey
)FromBarcode
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	inner join FtyInventory toFi on toFi.Ukey = fb.Ukey
	where toFi.POID = bb2.ToPOID
	and tofi.Seq1= bb2.ToSeq1 and toFi.Seq2 = bb2.ToSeq2
	and toFi.Roll = bb2.ToRoll and toFi.Dyelot = bb2.ToDyelot
	and toFi.StockType = bb2.ToStockType
)ToBarcode
where 1=1
and exists(
	select 1
	from MtlLocation ml 
	inner join dbo.SplitString(Fromlocation.listValue,',') sp on sp.Data = ml.ID 
		and ml.StockType=bb2.FromStockType
	where ml.IsWMS = 1

	union all

    select 1 from MtlLocation ml 
	inner join dbo.SplitString(bb2.ToLocation,',') sp on sp.Data = ml.ID
	and ml.StockType=bb2.ToStockType
	where ml.IsWMS = 1
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
                .Select(dr => new
                {
                    ID = dr["ID"].ToString(),
                    FromPOID = dr["FromPOID"].ToString(),
                    FromSeq1 = dr["FromSeq1"].ToString(),
                    FromSeq2 = dr["FromSeq2"].ToString(),
                    FromRoll = dr["FromRoll"].ToString(),
                    FromDyelot = dr["FromDyelot"].ToString(),
                    FromStockType = dr["FromStockType"].ToString(),
                    FromBarcode = dr["FromBarcode"].ToString(),
                    FromLocation = dr["FromLocation"].ToString(),
                    ToPOID = dr["ToPOID"].ToString(),
                    ToSeq1 = dr["ToSeq1"].ToString(),
                    ToSeq2 = dr["ToSeq2"].ToString(),
                    ToRoll = dr["ToRoll"].ToString(),
                    ToDyelot = dr["ToDyelot"].ToString(),
                    ToStockType = dr["ToStockType"].ToString(),
                    ToBarcode = dr["ToBarcode"].ToString(),
                    ToLocation = dr["ToLocation"].ToString(),
                    Qty = (decimal)dr["Qty"],
                    Ukey = (long)dr["Ukey"],
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("BorrowBack_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// ReturnReceipt To Gensong
        /// </summary>
        /// <param name="dtMaster">dtMaster</param>
        /// <param name="isConfirmed">bool</param>
        public void SentReturnReceiptToGensongAutoWHFabric(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentReturnReceiptToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select rrd.Id
,rrd.POID
,rrd.Seq1
,rrd.Seq2
,rrd.Roll
,rrd.Dyelot
,rrd.Qty
,rrd.StockType
,rrd.Ukey
,[Barcode] = Barcode.value
,[CmdTime] = GETDATE()
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
from ReturnReceipt_Detail rrd
inner join #tmp rr on rrd.Id=rr.Id
left join FtyInventory f on rrd.FtyInventoryUkey = f.ukey
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey
)Barcode
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= rrd.POID and psd.SEQ1 = rrd.Seq1
    and psd.SEQ2 = rrd.Seq2 and psd.FabricType='F'
)
and exists(
	select 1
	from FtyInventory_Detail fd
	inner join MtlLocation ml on ml.ID = fd.MtlLocationID
	where  fd.Ukey=f.Ukey
	and ml.IsWMS =1 
)
";
            DataTable dt = new DataTable();
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtMaster, null, sqlcmd, out dt)))
            {
                return;
            }

            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dt.AsEnumerable()
                .Select(s => new
                {
                    ID = s["ID"].ToString(),
                    POID = s["POID"].ToString(),
                    Seq1 = s["Seq1"].ToString(),
                    Seq2 = s["Seq2"].ToString(),
                    Roll = s["Roll"].ToString(),
                    Dyelot = s["Dyelot"].ToString(),
                    Qty = (decimal)s["Qty"],
                    StockType = s["StockType"].ToString(),
                    Ukey = (long)s["Ukey"],
                    Barcode = s["Barcode"].ToString(),
                    Status = s["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("ReturnReceipt_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// LocationTrans To Gensong
        /// </summary>
        /// <param name="dtMaster">dtMaster</param>
        /// <param name="isConfirmed">bool</param>
        public void SentLocationTransToGensongAutoWHFabric(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentLocationTransToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select lt2.Id
,lt2.POID
,lt2.Seq1
,lt2.Seq2
,lt2.Roll
,lt2.Dyelot
,lt2.FromLocation
,lt2.ToLocation
,[Barcode] = Barcode.value
,lt2.Ukey
,lt2.StockType
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,[CmdTime] = GETDATE()
from LocationTrans_detail lt2
inner join #tmp lt on lt.Id=lt2.Id
left join FtyInventory f on lt2.FtyInventoryUkey = f.ukey
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey
)Barcode
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= lt2.POID and psd.SEQ1 = lt2.Seq1
    and psd.SEQ2 = lt2.Seq2 and psd.FabricType='F'
)
and exists(
    select 1
	from MtlLocation ml
    inner join dbo.SplitString(lt2.ToLocation,',') sp on sp.Data = ml.ID
	where  ml.IsWMS =1 
	union all
    select 1
	from MtlLocation ml
    inner join dbo.SplitString(lt2.FromLocation,',') sp on sp.Data = ml.ID
	where  ml.IsWMS =1 
)
";
            DataTable dt = new DataTable();
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtMaster, null, sqlcmd, out dt)))
            {
                return;
            }

            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dt.AsEnumerable()
                .Select(s => new
                {
                    ID = s["ID"].ToString(),
                    POID = s["POID"].ToString(),
                    Seq1 = s["Seq1"].ToString(),
                    Seq2 = s["Seq2"].ToString(),
                    Roll = s["Roll"].ToString(),
                    Dyelot = s["Dyelot"].ToString(),
                    FromLocation = s["FromLocation"].ToString(),
                    ToLocation = s["ToLocation"].ToString(),
                    Barcode = s["Barcode"].ToString(),
                    Ukey = (long)s["Ukey"],
                    StockType = s["StockType"].ToString(),
                    Status = s["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("LocationTrans_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Adjust_Detail To Gensong
        /// </summary>
        /// <param name="dtMaster">Detail DataSource</param>
        /// <param name="isConfirmed">bool</param>
        public void SentAdjust_DetailToGensongAutoWHAccessory(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentAdjust_DetailToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select distinct 
 [Id] = i2.Id 
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Ukey] = i2.ukey
,[StockType] = i2.StockType
,[QtyBefore] = i2.QtyBefore
,[Barcode] = f.Barcode
,[QtyAfter] = i2.QtyAfter
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,CmdTime = GetDate()
from Production.dbo.Adjust_Detail i2
inner join #tmp i on i.Id = i2.Id
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1=i2.Seq1
	and f.Seq2=i2.Seq2 and f.Roll=i2.Roll and f.Dyelot=i2.Dyelot
    and f.StockType = i2.StockType
left join PO_Supp_Detail po3 on po3.ID = i2.POID
	and po3.SEQ1 = i2.Seq1 and po3.SEQ2 = i2.Seq2
where 1=1 
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='F'
)
and exists(
	select 1
	from FtyInventory_Detail fd 
	inner join MtlLocation ml on ml.ID = fd.MtlLocationID
	where f.Ukey = fd.Ukey
	and ml.IsWMS = 1
)

";
            DataTable dt = new DataTable();
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtMaster, null, sqlcmd, out dt)))
            {
                return;
            }

            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dt.AsEnumerable()
                .Select(dr => new
                {
                    Id = dr["id"].ToString(),
                    PoId = dr["PoId"].ToString(),
                    Seq1 = dr["Seq1"].ToString(),
                    Seq2 = dr["Seq2"].ToString(),
                    StockType = dr["StockType"].ToString(),
                    QtyBefore = (decimal)dr["QtyBefore"],
                    QtyAfter = (decimal)dr["QtyAfter"],
                    Barcode = dr["Barcode"].ToString(),
                    Ukey = (long)dr["Ukey"],
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("Adjust_Detail", bodyObject));

            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        private object CreateGensongStructure(string tableName, object structureID)
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
    }
}

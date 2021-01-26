using Ict;
using Newtonsoft.Json;
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
    /// <inheritdoc/>
    public class Vstrong_AutoWHAccessory
    {
        private static readonly string VstrongSuppID = "3A0196";
        private static readonly string moduleName = "AutoWHAccessory";
        private AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS();

        /// <inheritdoc/>
        public static bool IsVstrong_AutoWHAccessoryEnable => IsModuleAutomationEnable(VstrongSuppID, moduleName);

        /// <summary>
        /// Receive_Detail
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        public void SentReceive_DetailToVstrongAutoWHAccessory(DataTable dtDetail)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentReceiving_DetailToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
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
                    Refno = dr["Refno"].ToString().TrimEnd(),
                    StockUnit = dr["StockUnit"].ToString(),
                    StockQty = (decimal)dr["StockQty"],
                    PoUnit = dr["PoUnit"].ToString(),
                    ShipQty = (decimal)dr["ShipQty"],
                    Color = dr["Color"].ToString().TrimEnd(),
                    SizeCode = dr["SizeCode"].ToString().TrimEnd(),
                    Weight = (decimal)dr["Weight"],
                    StockType = dr["StockType"].ToString(),
                    MtlType = dr["MtlType"].ToString(),
                    Ukey = (long)dr["Ukey"],
                    ETA = MyUtility.Check.Empty(dr["ETA"]) ? null : ((DateTime?)dr["ETA"]).Value.ToString("yyyy/MM/dd"),
                    WhseArrival = MyUtility.Check.Empty(dr["WhseArrival"]) ? null : ((DateTime?)dr["WhseArrival"]).Value.ToString("yyyy/MM/dd"),
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("Receiving_Detail", bodyObject));

            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Issue_Detail To Vstrong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        public void SentIssue_DetailToVstrongAutoWHAccessory(DataTable dtDetail)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentIssue_DetailToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtDetail.AsEnumerable()
                .Select(dr => new
                {
                    Id = dr["id"].ToString(),
                    Type = dr["Type"].ToString(),
                    PoId = dr["PoId"].ToString(),
                    Seq1 = dr["Seq1"].ToString(),
                    Seq2 = dr["Seq2"].ToString(),
                    Color = dr["Color"].ToString(),
                    SizeCode = dr["SizeCode"].ToString(),
                    StockType = dr["StockType"].ToString(),
                    Qty = (decimal)dr["Qty"],
                    StockPoId = dr["StockPoId"].ToString(),
                    StockSeq1 = dr["StockSeq1"].ToString(),
                    StockSeq2 = dr["StockSeq2"].ToString(),
                    Ukey = (long)dr["Ukey"],
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("Issue_Detail", bodyObject));

            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// RemoveC_Detail To Vstrong
        /// </summary>
        /// <param name="dtMaster">Detail DataSource</param>
        /// <param name="isConfirmed"> confirmed</param>
        public void SentRemoveC_DetailToVstrongAutoWHAccessory(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentRemoveC_DetailToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select distinct 
 [Id] = i2.Id 
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Qty]  = i2.QtyBefore - i2.QtyAfter
,[Ukey] = i2.ukey
,i2.StockType
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
where 1 = 1 
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='A'
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
                    Qty = (decimal)dr["Qty"],
                    Ukey = (long)dr["Ukey"],
                    StockType = dr["StockType"].ToString(),
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("RemoveC_Detail", bodyObject));

            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SubTransfer To Vstrong
        /// </summary>
        /// <param name="dtMaster">Detail DataSource</param>
        /// <param name="isConfirmed">bool</param>
        public void SentSubTransfer_DetailToVstrongAutoWHAccessory(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentSubTransfer_DetailToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select distinct
[ID] = sd.ID
,s.Type
,sd.FromPOID,sd.FromSeq1,sd.FromSeq2,sd.FromStockType
,[FromLocation] = Fromlocation.listValue
,sd.ToPOID,sd.ToSeq1,sd.ToSeq2,sd.ToStockType
,[ToLocation] = sd.ToLocation
,po3.Refno
,[StockUnit] = dbo.GetStockUnitBySpSeq (fi.POID, fi.seq1, fi.seq2)
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' ,po3.SuppColor,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[SizeCode] = po3.SizeSpec
,[MtlType] = Fabric.MtlTypeID
,sd.Qty
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
left join PO_Supp_Detail po3 on po3.ID = sd.FromPOID and po3.SEQ1 = sd.FromSeq1
and po3.SEQ2 = sd.FromSeq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
outer apply(
	select listValue = Stuff((
			select concat(',',MtlLocationID)
			from (
					select	distinct fd.MtlLocationID
					from dbo.FtyInventory_Detail fd
					where fd.Ukey = fi.Ukey
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
where 1=1
and exists(
    select 1 from Production.dbo.PO_Supp_Detail
    where id = sd.ToPOID and seq1=sd.ToSeq1 and seq2=sd.ToSeq2
    and FabricType='A'
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
                    Type = dr["Type"].ToString(),
                    FromPoId = dr["FromPoId"].ToString(),
                    FromSeq1 = dr["FromSeq1"].ToString(),
                    FromSeq2 = dr["FromSeq2"].ToString(),
                    FromStockType = dr["FromStockType"].ToString(),
                    FromLocation = dr["FromLocation"].ToString(),
                    ToPoId = dr["ToPoId"].ToString(),
                    ToSeq1 = dr["ToSeq1"].ToString(),
                    ToSeq2 = dr["ToSeq2"].ToString(),
                    ToStockType = dr["ToStockType"].ToString(),
                    ToLocation = dr["ToLocation"].ToString(),
                    Refno = dr["Refno"].ToString(),
                    StockUnit = dr["StockUnit"].ToString(),
                    Color = dr["Color"].ToString(),
                    SizeCode = dr["SizeCode"].ToString(),
                    MtlType = dr["MtlType"].ToString(),
                    Qty = (decimal)dr["Qty"],
                    Ukey = (long)dr["Ukey"],
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("SubTransfer_Detail", bodyObject));

            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// WHClose To Vstrong
        /// </summary>
        /// <param name="dtMaster">Master DataSource</param>
        public void SentWHCloseToVstrongAutoWHAccessory(DataTable dtMaster)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentWHCloseToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
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

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("WHClose", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// ReturnReceipt To Vstrong
        /// </summary>
        /// <param name="dtMaster">dtMaster</param>
        /// <param name="isConfirmed">bool</param>
        public void SentReturnReceiptToVstrongAutoWHAccessory(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentReturnReceiptToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select rrd.Id
,rrd.POID
,rrd.Seq1
,rrd.Seq2
,rrd.Qty
,rrd.StockType
,rrd.Ukey
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,[CmdTime] = GETDATE()
from ReturnReceipt_Detail rrd
inner join #tmp rr on rrd.Id=rr.Id
left join FtyInventory f on rrd.FtyInventoryUkey = f.ukey
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= rrd.POID and psd.SEQ1 = rrd.Seq1
    and psd.SEQ2 = rrd.Seq2 and psd.FabricType='A'
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
                    Qty = (decimal)s["Qty"],
                    StockType = s["StockType"].ToString(),
                    Ukey = (long)s["Ukey"],
                    Status = s["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("ReturnReceipt_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// BorrowBack to Vstrong
        /// </summary>
        /// <param name="dtDetail">DataTable</param>
        /// <param name="isConfirmed">bool</param>
        public void SentBorrowBackToVstrongAutoWHAccessory(DataTable dtDetail, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentBorrowBackToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select distinct
[ID] = bb2.ID
,bb2.FromPOID,bb2.FromSeq1,bb2.FromSeq2,bb2.FromStockType
,[FromLocation] = Fromlocation.listValue
,bb2.ToPOID,bb2.ToSeq1,bb2.ToSeq2,bb2.ToStockType
,[ToLocation] = bb2.ToLocation
,po3.Refno
,[StockUnit] = dbo.GetStockUnitBySpSeq (fi.POID, fi.seq1, fi.seq2)
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' ,po3.SuppColor,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[SizeCode] = po3.SizeSpec
,[MtlType] = Fabric.MtlTypeID
,bb2.Qty
,bb2.Ukey
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,CmdTime = GetDate()
from Production.dbo.BorrowBack_Detail bb2
inner join #tmp bb on bb.ID=bb2.Id
left join FtyInventory FI on bb2.FromPoid = Fi.Poid and bb2.FromSeq1 = Fi.Seq1 and bb2.FromSeq2 = Fi.Seq2 
    and bb2.FromRoll = Fi.Roll and bb2.FromDyelot = Fi.Dyelot and bb2.FromStockType = FI.StockType
left join PO_Supp_Detail po3 on po3.ID = bb2.FromPOID and po3.SEQ1 = bb2.FromSeq1
and po3.SEQ2 = bb2.FromSeq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
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
where 1=1
and exists(
    select 1 from Production.dbo.PO_Supp_Detail
    where id = bb2.ToPOID and seq1=bb2.ToSeq1 and seq2=bb2.ToSeq2
    and FabricType='A'
)
and exists(
	select 1
	from MtlLocation ml 
	inner join dbo.SplitString(Fromlocation.listValue,',') sp on sp.Data = ml.ID 
		and ml.StockType=bb2.FromStockType
	where ml.IsWMS = 1
union all
	select 1 
    from MtlLocation ml 
    inner join dbo.SplitString(bb2.ToLocation,',') sp on sp.Data = ml.ID 
	    and ml.StockType = bb2.FromStockType
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
                    FromStockType = dr["FromStockType"].ToString(),
                    FromLocation = dr["FromLocation"].ToString(),
                    ToPOID = dr["ToPOID"].ToString(),
                    ToSeq1 = dr["ToSeq1"].ToString(),
                    ToSeq2 = dr["ToSeq2"].ToString(),
                    ToStockType = dr["ToStockType"].ToString(),
                    ToLocation = dr["ToLocation"].ToString(),
                    Refno = dr["Refno"].ToString(),
                    StockUnit = dr["StockUnit"].ToString(),
                    Color = dr["Color"].ToString(),
                    SizeCode = dr["SizeCode"].ToString(),
                    MtlType = dr["MtlType"].ToString(),
                    Qty = (decimal)dr["Qty"],
                    Ukey = (long)dr["Ukey"],
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("BorrowBack_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// LocationTrans To Vstrong
        /// </summary>
        /// <param name="dtMaster">dtMaster</param>
        /// <param name="isConfirmed">bool</param>
        public void SentLocationTransToVstrongAutoWHAccessory(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentLocationTransToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select lt2.Id
,lt2.POID
,lt2.Seq1
,lt2.Seq2
,lt2.FromLocation
,lt2.ToLocation
,po3.Refno
,[StockUnit] = dbo.GetStockUnitBySpSeq (f.POID, f.seq1, f.seq2)
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' ,po3.SuppColor,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[SizeCode] = po3.SizeSpec
,[MtlType] = Fabric.MtlTypeID
,lt2.Ukey
,lt2.StockType
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,[Qty] = f.InQty - f.OutQty + f.AdjustQty
,[CmdTime] = GETDATE()
from LocationTrans_detail lt2
inner join #tmp lt on lt.Id=lt2.Id
left join FtyInventory f on lt2.FtyInventoryUkey = f.ukey
left join PO_Supp_Detail po3 on po3.ID = lt2.POID and po3.SEQ1 = lt2.Seq1
and po3.SEQ2 = lt2.Seq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= lt2.POID and psd.SEQ1 = lt2.Seq1
    and psd.SEQ2 = lt2.Seq2 and psd.FabricType='A'
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
                    FromLocation = s["FromLocation"].ToString(),
                    ToLocation = s["ToLocation"].ToString(),
                    Refno = s["Refno"].ToString(),
                    StockUnit = s["StockUnit"].ToString(),
                    Color = s["Color"].ToString(),
                    SizeCode = s["SizeCode"].ToString(),
                    MtlType = s["MtlType"].ToString(),
                    Qty = (decimal)s["Qty"],
                    Ukey = (long)s["Ukey"],
                    StockType = s["StockType"].ToString(),
                    Status = s["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("LocationTrans_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Adjust_Detail To Vstrong
        /// </summary>
        /// <param name="dtMaster">Detail DataSource</param>
        /// <param name="isConfirmed">bool</param>
        public void SentAdjust_DetailToVstrongAutoWHAccessory(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentAdjust_DetailToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
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
	and FabricType='A'
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
                    Ukey = (long)dr["Ukey"],
                    Status = dr["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("Adjust_Detail", bodyObject));

            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// IssueReturn_Detail To Vstrong
        /// </summary>
        /// <param name="dtMaster">dtMaster</param>
        /// <param name="isConfirmed">bool</param>
        public void SentIssueReturn_DetailToVstrongAutoWHAccessory(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentIssueReturn_DetailToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select ir2.Id
,ir2.POID
,ir2.Seq1
,ir2.Seq2
,po3.Refno
,[StockUnit] = dbo.GetStockUnitBySpSeq (f.POID, f.seq1, f.seq2)
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' ,po3.SuppColor,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[SizeCode] = po3.SizeSpec
,[MtlType] = Fabric.MtlTypeID
,ir2.Ukey
,ir2.StockType
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,[Qty] = ir2.Qty
,[CmdTime] = GETDATE()
from IssueReturn_Detail ir2
inner join #tmp lt on lt.Id=ir2.Id
left join FtyInventory f on ir2.FtyInventoryUkey = f.ukey
left join PO_Supp_Detail po3 on po3.ID = ir2.POID and po3.SEQ1 = ir2.Seq1
and po3.SEQ2 = ir2.Seq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= ir2.POID and psd.SEQ1 = ir2.Seq1
    and psd.SEQ2 = ir2.Seq2 and psd.FabricType='A'
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
                    StockType = s["StockType"].ToString(),
                    Ukey = (long)s["Ukey"],
                    Refno = s["Refno"].ToString(),
                    StockUnit = s["StockUnit"].ToString(),
                    Color = s["Color"].ToString(),
                    SizeCode = s["SizeCode"].ToString(),
                    MtlType = s["MtlType"].ToString(),
                    Qty = (decimal)s["Qty"],
                    Status = s["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("IssueReturn_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        private object CreateVstrongStructure(string tableName, object structureID)
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

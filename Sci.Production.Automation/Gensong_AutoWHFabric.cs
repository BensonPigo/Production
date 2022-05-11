using Newtonsoft.Json;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using static Sci.Production.Automation.UtilityAutomation;

namespace Sci.Production.Automation
{
    /// <inheritdoc/>
    public class Gensong_AutoWHFabric
    {
        private static readonly string GensongSuppID = "3A0174";
        private static readonly string moduleName = "AutoWHFabric";
        private static readonly string suppAPIThread = "pms/GS_WebServices";
        private static readonly string SCIAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
        private static readonly string URL = GetSupplierUrl(GensongSuppID, moduleName);

        /// PMS的action對應廠商statusAPI: Confrim(New),Unconfrim(Delete),Delete(Delete),Update(Revise)
        /// <param name="dtDetail">表身資訊,需要有ukey</param>
        /// <param name="formName">P10...P99</param>
        /// <param name="statusAPI">給廠商的動作指令 New/Delete/Revise/Lock/Unlock</param>
        /// <param name="action">PMS 的操作 Confrim, Unconfrim, (P99) Delete, Update</param>
        /// <param name="updateLocation">P21/P26更新後,若location不是自動倉要發給WMS做撤回(Delete), 整合後為了保持原寫法而加的參數, 日後若確認無用請刪掉此看似無用的參數</param>
        /// <inheritdoc/>
        public static bool Sent(bool doTask, DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, bool updateLocation = false, bool isP99 = false, bool fromNewBarcode = false)
        {
            if (!Prgs.NoGensong(formName))
            {
                return true;
            }

            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count == 0)
            {
                return false;
            }

            if (doTask)
            {
                Task.Run(() => Sent_Task(dtDetail, formName, statusAPI, action, updateLocation, isP99, fromNewBarcode))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                return true;
            }
            else
            {
                return Sent_Task(dtDetail, formName, statusAPI, action, updateLocation, isP99, fromNewBarcode);
            }
        }

        private static bool Sent_Task(DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, bool updateLocation = false, bool isP99 = false, bool fromNewBarcode = false)
        {
            // 取得資料
            DataTable dtMaster = LogicAutoWHData.GetWHData(dtDetail, formName, statusAPI, action, "F", fromNewBarcode, isP99);
            if (dtMaster == null)
            {
                return false;
            }

            if (dtMaster.Rows.Count == 0)
            {
                return true;
            }

            // DataTable轉化為JSON
            WHTableName dtNameforAPI = LogicAutoWHData.GetDetailNameforAPI(formName);
            string jsonBody = GetJsonBody(dtMaster, dtNameforAPI, statusAPI);
            AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
            {
                apiThread = $"Sent{dtNameforAPI}ToGensong",
                suppAPIThread = statusAPI == EnumStatus.New || updateLocation ? SCIAPIThread : suppAPIThread,
                moduleName = moduleName,
                suppID = GensongSuppID,
            };

            if (!LogicAutoWHData.SendWebAPI_Status(statusAPI, URL, automationErrMsg, jsonBody))
            {
                return false;
            }

            // 記錄 Confirmed/UnConfirmed 後有傳給WMS的資料
            if (statusAPI != EnumStatus.Lock && statusAPI != EnumStatus.UnLock)
            {
                PublicPrg.Prgs.SentToWMS(dtMaster, action == EnumStatus.Confirm, formName);
            }

            return true;
        }

        private static string GetJsonBody(DataTable dtDetail, WHTableName detailName, EnumStatus statusAPI)
        {
            string jsonBody = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Gensong_AutoWHFabric callMethod = new Gensong_AutoWHFabric();
            dynamic bodyObject = new ExpandoObject();
            switch (detailName)
            {
                case WHTableName.Receiving_Detail:
                case WHTableName.TransferIn_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                    .Select(dr => new
                    {
                        ID = dr["ID"].ToString(),
                        InvNo = dr["InvNo"].ToString(),
                        PoId = dr["PoId"].ToString(),
                        Seq1 = dr["Seq1"].ToString(),
                        Seq2 = dr["Seq2"].ToString(),
                        WeaveType = dr["WeaveTypeID"].ToString(),
                        Refno = dr["Refno"].ToString(),
                        Color = dr["Color"].ToString(),
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
                        Status = statusAPI.ToString(),
                        CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    });
                    break;
                case WHTableName.Issue_Detail:
                case WHTableName.IssueLack_Detail:
                case WHTableName.TransferOut_Detail:
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
                       WeaveType = dr["WeaveTypeID"].ToString(),
                       Roll = dr["Roll"].ToString(),
                       Dyelot = dr["Dyelot"].ToString(),
                       Barcode = dr["Barcode"].ToString(),
                       NewBarcode = dr["NewBarcode"].ToString(),
                       Description = dr["Description"].ToString(),
                       Tone = dr["Tone"].ToString(),
                       Qty = (decimal)dr["Qty"],
                       Ukey = (long)dr["Ukey"],
                       Status = statusAPI.ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;
                case WHTableName.Adjust_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                        .Select(dr => new
                        {
                            Id = dr["id"].ToString(),
                            PoId = dr["PoId"].ToString(),
                            Seq1 = dr["Seq1"].ToString(),
                            Seq2 = dr["Seq2"].ToString(),
                            WeaveType = dr["WeaveTypeID"].ToString(),
                            Roll = dr["Roll"].ToString(),
                            Dyelot = dr["Dyelot"].ToString(),
                            StockType = dr["StockType"].ToString(),
                            QtyBefore = (decimal)dr["QtyBefore"],
                            QtyAfter = (decimal)dr["QtyAfter"],
                            Barcode = dr["Barcode"].ToString(),
                            Ukey = (long)dr["Ukey"],
                            Status = statusAPI.ToString(),
                            CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        });
                    break;
                case WHTableName.SubTransfer_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                    .Select(dr => new
                    {
                        ID = dr["ID"].ToString(),
                        Type = dr["Type"].ToString(),
                        FromPOID = dr["FromPOID"].ToString(),
                        FromSeq1 = dr["FromSeq1"].ToString(),
                        FromSeq2 = dr["FromSeq2"].ToString(),
                        WeaveType = dr["WeaveTypeID"].ToString(),
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
                        Description = dr["Description"].ToString(),
                        ToBarcode = dr["ToBarcode"].ToString(),
                        ToLocation = dr["ToLocation"].ToString(),
                        Refno = dr["Refno"].ToString(),
                        Color = dr["Color"].ToString(),
                        Qty = (decimal)dr["Qty"],
                        Ukey = (long)dr["Ukey"],
                        Status = statusAPI.ToString(),
                        CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    });
                    break;
                case WHTableName.ReturnReceipt_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(dr => new
                   {
                       ID = dr["ID"].ToString(),
                       POID = dr["POID"].ToString(),
                       Seq1 = dr["Seq1"].ToString(),
                       Seq2 = dr["Seq2"].ToString(),
                       WeaveType = dr["WeaveTypeID"].ToString(),
                       Roll = dr["Roll"].ToString(),
                       Dyelot = dr["Dyelot"].ToString(),
                       Description = dr["Description"].ToString(),
                       Qty = (decimal)dr["Qty"],
                       StockType = dr["StockType"].ToString(),
                       Ukey = (long)dr["Ukey"],
                       Barcode = dr["Barcode"].ToString(),
                       Status = statusAPI.ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;
                case WHTableName.BorrowBack_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(dr => new
                   {
                       ID = dr["ID"].ToString(),
                       FromPOID = dr["FromPOID"].ToString(),
                       FromSeq1 = dr["FromSeq1"].ToString(),
                       FromSeq2 = dr["FromSeq2"].ToString(),
                       WeaveType = dr["WeaveTypeID"].ToString(),
                       FromRoll = dr["FromRoll"].ToString(),
                       FromDyelot = dr["FromDyelot"].ToString(),
                       Refno = dr["Refno"].ToString(),
                       Color = dr["Color"].ToString(),
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
                       Description = dr["Description"].ToString(),
                       Qty = (decimal)dr["Qty"],
                       Ukey = (long)dr["Ukey"],
                       Status = statusAPI.ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;
                case WHTableName.LocationTrans_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(s => new
                   {
                       ID = s["ID"].ToString(),
                       POID = s["POID"].ToString(),
                       Seq1 = s["Seq1"].ToString(),
                       Seq2 = s["Seq2"].ToString(),
                       WeaveType = s["WeaveTypeID"].ToString(),
                       Roll = s["Roll"].ToString(),
                       Dyelot = s["Dyelot"].ToString(),
                       Refno = s["Refno"].ToString(),
                       Color = s["Color"].ToString(),
                       FromLocation = s["FromLocation"].ToString(),
                       ToLocation = s["ToLocation"].ToString(),
                       Barcode = s["Barcode"].ToString(),
                       Ukey = (long)s["Ukey"],
                       StockType = s["StockType"].ToString(),
                       Description = s["Description"].ToString(),
                       Qty = (decimal)s["Qty"],
                       Status = statusAPI.ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;

                case WHTableName.IssueReturn_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(s => new
                   {
                       ID = s["ID"].ToString(),
                       POID = s["POID"].ToString(),
                       Seq1 = s["Seq1"].ToString(),
                       Seq2 = s["Seq2"].ToString(),
                       WeaveType = s["WeaveTypeID"].ToString(),
                       StockType = s["StockType"].ToString(),
                       Ukey = (long)s["Ukey"],
                       Refno = s["Refno"].ToString(),
                       StockUnit = s["StockUnit"].ToString(),
                       Color = s["Color"].ToString(),
                       Roll = s["Roll"].ToString(),
                       Dyelot = s["Dyelot"].ToString(),
                       Barcode = s["Barcode"].ToString(),
                       Description = s["Description"].ToString(),
                       Qty = (decimal)s["Qty"],
                       Status = statusAPI.ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;

                case WHTableName.Stocktaking_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(s => new
                   {
                       ID = s["ID"].ToString(),
                       POID = s["POID"].ToString(),
                       Seq1 = s["Seq1"].ToString(),
                       Seq2 = s["Seq2"].ToString(),
                       WeaveType = s["WeaveTypeID"].ToString(),
                       Roll = s["Roll"].ToString(),
                       Dyelot = s["Dyelot"].ToString(),
                       StockType = s["StockType"].ToString(),
                       Ukey = (long)s["Ukey"],
                       Barcode = s["Barcode"].ToString(),
                       Description = s["Description"].ToString(),
                       QtyBefore = (decimal)s["QtyBefore"],
                       Status = statusAPI.ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;
            }

            return jsonBody = JsonConvert.SerializeObject(LogicAutoWHData.CreateStructure(detailName.ToString(), bodyObject));
        }

        /// <inheritdoc/>
        public static void SentWHClose(bool doTask, DataTable dtMaster)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentWHCloseToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
            {
                apiThread = apiThread,
                suppAPIThread = suppAPIThread,
            };

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtMaster.AsEnumerable()
                .Select(dr => new
                {
                    POID = dr["POID"].ToString(),
                    WhseClose = MyUtility.Check.Empty(dr["WhseClose"]) ? null : ((DateTime?)dr["WhseClose"]).Value.ToString("yyyy/MM/dd"),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(LogicAutoWHData.CreateStructure("WHClose", bodyObject));
            if (doTask)
            {
                Task.Run(() => SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, automationErrMsg))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, automationErrMsg);
            }
        }

        /// <summary>
        /// Cutting P04 Cutplan_Detail To Gensong
        /// </summary>
        /// <inheritdoc/>
        public static void SentCutplan_Detail(bool doTask, DataTable dtDetail, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count == 0)
            {
                return;
            }

            if (doTask)
            {
                Task.Run(() => ProcessCutplan_Detail(dtDetail, isConfirmed))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                ProcessCutplan_Detail(dtDetail, isConfirmed);
            }
        }

        private static void ProcessCutplan_Detail(DataTable dtDetail, bool isConfirmed)
        {
            string apiThread = "SentCutplan_DetailToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
            {
                apiThread = apiThread,
                suppAPIThread = suppAPIThread,
            };

            string sqlcmd = $@"
select distinct
    [CutplanID] = cp2.ID
    ,[CutRef] = cp2.CutRef
    ,[CutNo] = cp2.CutNo
    ,[PoID] = wo.ID
    ,[Seq1] = wo.SEQ1
    ,[Seq2] = wo.SEQ2
    ,[WeaveTypeID] = isnull(Fabric.WeaveTypeID,'')
    ,[Refno] = wo.Refno
    ,[Article] = Article.value
    ,[Color] = LTRIM(RTRIM(Color.value))
    ,[SizeCode] = SizeCode.value
    ,cp2.WorkorderUkey
    ,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
    ,[CmdTime] = GETDATE()
    from  Production.dbo.Cutplan_Detail cp2
    inner join Production.dbo.Cutplan cp1 on cp2.id = cp1.id
    inner join Production.dbo.WorkOrder wo on cp2.WorkorderUkey = wo.Ukey
    LEFT join Production.dbo.PO_Supp_Detail po3 on po3.ID= cp2.PoId 
	    and po3.SEQ1=wo.Seq1 and po3.SEQ2=wo.Seq2
    LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
    OUTER APPLY(
     SELECT [Value]=
	     CASE WHEN Fabric.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') 
                THEN IIF(po3.SuppColor = '',dbo.GetColorMultipleID(po3.BrandID,po3.ColorID), po3.SuppColor)
                ELSE dbo.GetColorMultipleID(po3.BrandID,po3.ColorID)
	     END
    )Color
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
                    WeaveType = s["WeaveTypeID"].ToString(),
                    Refno = s["Refno"].ToString(),
                    Article = s["Article"].ToString(),
                    Color = s["Color"].ToString(),
                    SizeCode = s["SizeCode"].ToString(),
                    WorkorderUkey = (long)s["WorkorderUkey"],
                    Status = s["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(LogicAutoWHData.CreateStructure("Cutplan_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, automationErrMsg);
        }

        /// <inheritdoc/>
        public static bool IsGensong_AutoWHFabricEnable => IsModuleAutomationEnable(GensongSuppID, moduleName);

        /// <summary>
        /// 用在 MES FOS B02
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
            AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
            {
                apiThread = apiThread,
                suppAPIThread = suppAPIThread,
            };

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtDetail.AsEnumerable()
                .Select(s => new
                {
                    Refno = s["Refno"].ToString(),
                    FabricRelaxationID = s["FabricRelaxationID"].ToString(),
                    RelaxTime = (decimal)s["RelaxTime"],
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(LogicAutoWHData.CreateStructure("RefnoRelaxtime", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, automationErrMsg);
        }
    }
}
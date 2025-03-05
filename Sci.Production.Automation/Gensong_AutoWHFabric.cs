using Newtonsoft.Json;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
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
        // 廠商的資訊基本上都沒使用到, 也不需要使用
        private static readonly string GensongSuppID = "3A0174";
        private static readonly string moduleName = "AutoWHFabric";
        private static readonly string URL = GetSupplierUrl(GensongSuppID, moduleName);
        private static readonly string suppAPIThread = "pms/GS_WebServices";

        // 一律呼叫SCI 中繼API, SuppID & ModuleName 會影響到ReSent, 所以必須用SCI
        private static readonly string SCISuppID = "SCI";
        private static readonly string SCIModuleName = "SCI";
        private static readonly string SCIAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";

        /// PMS的action對應廠商statusAPI: Confrim(New),Unconfrim(Delete),Delete(Delete),Update(Revise)
        /// <param name="dtDetail">表身資訊,需要有ukey</param>
        /// <param name="formName">P10...P99</param>
        /// <param name="statusAPI">給廠商的動作指令 New/Delete/Revise/Lock/Unlock</param>
        /// <param name="action">PMS 的操作 Confrim, Unconfrim, (P99) Delete, Update</param>
        /// <inheritdoc/>
        public static bool Sent(bool doTask, DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, bool isP99 = false, bool fromNewBarcode = false, int typeCreateRecord = 0, List<AutoRecord> autoRecord = null)
        {
            if (!Prgs.NoGensong(formName) || dtDetail.Rows.Count == 0)
            {
                return true;
            }

            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return false;
            }

            if (doTask)
            {
                Task.Run(() => Sent_Task(dtDetail, formName, statusAPI, action, isP99, fromNewBarcode, typeCreateRecord, autoRecord: autoRecord))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                return true;
            }
            else
            {
                return Sent_Task(dtDetail, formName, statusAPI, action, isP99, fromNewBarcode, typeCreateRecord, autoRecord: autoRecord);
            }
        }

        private static bool Sent_Task(DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, bool isP99, bool fromNewBarcode, int typeCreateRecord, List<AutoRecord> autoRecord = null)
        {
            AutoRecord autoRecordbyType = null;
            if (typeCreateRecord == 2)
            {
                autoRecordbyType = autoRecord.Where(w => w.fabricType == "F").FirstOrDefault();
                if (autoRecordbyType != null)
                {
                    LogicAutoWHData.SentandUpdatebyAutomationCreateRecord(formName, statusAPI, action, autoRecordbyType, GetSciUrl());
                }
            }
            else
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

                if (statusAPI == EnumStatus.New)
                {
                    SentandUpdate(dtMaster, formName, statusAPI, action, 0);
                }
                else
                {
                    // 一次傳太大量會 Timeout 拆100傳出去
                    DataTable[] splittedtables = dtMaster.AsEnumerable()
                        .Select((row, index) => new { row, index })
                        .GroupBy(x => x.index / 100)
                        .Select(g => g.Select(x => x.row).CopyToDataTable())
                        .ToArray();

                    if (autoRecord != null)
                    {
                        autoRecord.Add(new AutoRecord { fabricType = "F", automationCreateRecordUkey = new List<string>(), wh_Detail_Ukey = new List<string>() });
                        autoRecordbyType = autoRecord.Where(w => w.fabricType == "F").FirstOrDefault();
                    }

                    for (int i = 0; i < splittedtables.Length; i++)
                    {
                        DataTable dt = splittedtables[i];
                        if (!SentandUpdate(dt, formName, statusAPI, action, typeCreateRecord, autoRecord: autoRecordbyType))
                        {
                            // 若 Lock 失敗 要把此次之前的 UnLock
                            if (statusAPI == EnumStatus.Lock)
                            {
                                for (int j = 0; j < i; j++)
                                {
                                    SentandUpdate(splittedtables[j], formName, EnumStatus.UnLock, action, typeCreateRecord);
                                }

                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// SentandUpdate
        /// </summary>
        /// <param name="dt">dt</param>
        /// <param name="formName">formName</param>
        /// <param name="statusAPI">statusAPI</param>
        /// <param name="action">action</param>
        /// <param name="typeCreateRecord">typeCreateRecord</param>
        /// <param name="autoRecord">autoRecord</param>
        /// <returns>bool</returns>
        public static bool SentandUpdate(DataTable dt, string formName, EnumStatus statusAPI, EnumStatus action, int typeCreateRecord, AutoRecord autoRecord = null)
        {
            // DataTable轉化為JSON
            WHTableName dtNameforAPI = LogicAutoWHData.GetDetailNameforAPI(formName);

            string jsonBody = GetJsonBody(dt, dtNameforAPI, statusAPI);
            AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
            {
                apiThread = $"Sent{dtNameforAPI}ToGensong",
                suppAPIThread = SCIAPIThread,
                moduleName = SCIModuleName,
                suppID = SCISuppID,
            };

            switch (typeCreateRecord)
            {
                case 0:
                    if (!LogicAutoWHData.SendWebAPI_Status(statusAPI, GetSciUrl(), automationErrMsg, jsonBody))
                    {
                        return false;
                    }

                    // 記錄 Confirmed/UnConfirmed 後有傳給WMS的資料
                    if (statusAPI != EnumStatus.Lock && statusAPI != EnumStatus.UnLock)
                    {
                        PublicPrg.Prgs.SentToWMS(dt, action == EnumStatus.Confirm, formName);
                    }

                    break;

                case 1:
                    if (!LogicAutoWHData.SaveAutomationCreateRecord(automationErrMsg, jsonBody, autoRecord: autoRecord))
                    {
                        return false;
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        autoRecord.wh_Detail_Ukey.Add(MyUtility.Convert.GetString(dr["Ukey"]));
                    }

                    break;
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
                case WHTableName.LocalOrderReceiving_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(s => new
                   {
                       ID = s["ID"].ToString(),
                       InvNo = s["InvNo"].ToString(),
                       POID = s["POID"].ToString(),
                       Seq1 = s["Seq1"].ToString(),
                       Seq2 = s["Seq2"].ToString(),
                       WeaveType = MyUtility.Check.Empty(s["WeaveType"]) ? null : s["WeaveType"].ToString(),
                       Refno = s["Refno"].ToString(),
                       Color = s["Color"].ToString(),
                       Roll = MyUtility.Check.Empty(s["Roll"]) ? null : s["Roll"].ToString(),
                       Dyelot = MyUtility.Check.Empty(s["Dyelot"]) ? null : s["Dyelot"].ToString(),
                       StockUnit = s["StockUnit"].ToString(),
                       StockQty = s["StockQty"].ToString(),
                       PoUnit = s["PoUnit"].ToString(),
                       ShipQty = s["ShipQty"].ToString(),
                       Weight = s["Weight"].ToString(),
                       StockType = "B",
                       Barcode = MyUtility.Check.Empty(s["Barcode"]) ? null : s["Barcode"].ToString(),
                       Ukey = (long)s["Ukey"],
                       IsInspection = 0,
                       ETA = string.Empty,
                       WhseArrival = s["WhseArrival"],
                       Status = statusAPI.ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;
                case WHTableName.LocalOrderIssue_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                    .Select(s => new
                    {
                        ID = s["ID"].ToString(),
                        Type = "P71",
                        CutplanID = string.Empty,
                        EstCutDate = s["EstCutdate"],
                        SpreadingNoID = string.Empty,
                        POID = s["POID"].ToString(),
                        Seq1 = s["Seq1"].ToString(),
                        Seq2 = s["Seq2"].ToString(),
                        WeaveType = s["WeaveType"].ToString(),
                        Roll = s["Roll"].ToString(),
                        Dyelot = s["Dyelot"].ToString(),
                        Barcode = s["Barcode"].ToString(),
                        NewBarcode = s["NewBarcode"].ToString(),
                        Description = s["Description"].ToString(),
                        Qty = s["Qty"].ToString(),
                        Ukey = (long)s["Ukey"],
                        Status = statusAPI.ToString(),
                        CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    });
                    break;
                case WHTableName.LocalOrderAdjust_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                    .Select(s => new
                    {
                        ID = s["ID"].ToString(),
                        POID = s["POID"].ToString(),
                        Seq1 = s["Seq1"].ToString(),
                        Seq2 = s["Seq2"].ToString(),
                        WeaveType = s["WeaveType"].ToString(),
                        Roll = s["Roll"].ToString(),
                        Dyelot = s["Dyelot"].ToString(),
                        StockType = s["StockType"].ToString(),
                        QtyBefore = s["QtyBefore"].ToString(),
                        QtyAfter = s["QtyAfter"].ToString(),
                        Barcode = s["Barcode"].ToString(),
                        Ukey = (long)s["Ukey"],
                        Status = statusAPI.ToString(),
                        CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    });
                    break;
                case WHTableName.LocalOrderLocationTrans_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                    .Select(s => new
                    {
                        ID = s["ID"].ToString(),
                        POID = s["POID"].ToString(),
                        Seq1 = s["Seq1"].ToString(),
                        Seq2 = s["Seq2"].ToString(),
                        WeaveType = s["WeaveType"].ToString(),
                        Roll = s["Roll"].ToString(),
                        Dyelot = s["Dyelot"].ToString(),
                        Refno = s["Refno"].ToString(),
                        Color = s["Color"].ToString(),
                        FromLocation = s["FromLocation"].ToString(),
                        ToLocation = s["ToLocation"].ToString(),
                        Barcode = s["Barcode"].ToString(),
                        Description = s["Description"].ToString(),
                        Qty = s["Qty"].ToString(),
                        Ukey = (long)s["Ukey"],
                        StockType = s["StockType"].ToString(),
                        Status = statusAPI.ToString(),
                        CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    });
                    break;
            }

            switch (detailName)
            {
                case WHTableName.LocalOrderReceiving_Detail:
                    detailName = WHTableName.Receiving_Detail;
                    break;
                case WHTableName.LocalOrderLocationTrans_Detail:
                    detailName = WHTableName.LocationTrans_Detail;
                    break;
                case WHTableName.LocalOrderIssue_Detail:
                    detailName = WHTableName.Issue_Detail;
                    break;
                case WHTableName.LocalOrderAdjust_Detail:
                    detailName = WHTableName.Adjust_Detail;
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
            AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
            {
                apiThread = apiThread,
                suppAPIThread = SCIAPIThread,
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
                Task.Run(() => SendWebAPI(GetSciUrl(), SCIAPIThread, jsonBody, automationErrMsg))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                SendWebAPI(GetSciUrl(), SCIAPIThread, jsonBody, automationErrMsg);
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
            AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
            {
                apiThread = apiThread,
                suppAPIThread = SCIAPIThread,
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
    ,[WorkorderUkey] = cp2.WorkOrderForPlanningUkey
    ,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
    ,[CmdTime] = GETDATE()
    from  Production.dbo.Cutplan_Detail cp2
    inner join Production.dbo.Cutplan cp1 on cp2.id = cp1.id
    inner join Production.dbo.WorkOrderForPlanning wo on cp2.WorkOrderForPlanningUkey = wo.Ukey
    LEFT join Production.dbo.PO_Supp_Detail po3 on po3.ID= cp2.PoId 
	    and po3.SEQ1=wo.Seq1 and po3.SEQ2=wo.Seq2
    LEFT JOIN Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = po3.id and psdsC.seq1 = po3.seq1 and psdsC.seq2 = po3.seq2 and psdsC.SpecColumnID = 'Color'
    LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
    OUTER APPLY(
     SELECT [Value]=
	     CASE WHEN Fabric.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') 
                THEN IIF(po3.SuppColor = '',dbo.GetColorMultipleID(po3.BrandID,isnull(psdsC.SpecValue ,'')), po3.SuppColor)
                ELSE dbo.GetColorMultipleID(po3.BrandID,isnull(psdsC.SpecValue ,''))
	     END
    )Color
    outer apply(
        select value = STUFF((
            select CONCAT(',',SizeCode)
            from(
                select distinct SizeCode
                from Production.dbo.WorkOrderForPlanning_Distribute
                where WorkOrderForPlanningUkey = wo.Ukey
                )s
                for xml path('')
            ),1,1,'')
    ) SizeCode
    outer apply(
        select value = STUFF((
            select CONCAT(',',Article)
            from(
                select distinct Article
                from Production.dbo.WorkOrderForPlanning_Distribute
                where WorkOrderForPlanningUkey = wo.Ukey
                and Article !=''
                )s
            for xml path('')
            ),1,1,'')
    ) Article
    where exists(
        select 1 from Issue_Detail where cp2.ID = CutplanID
    )
    and exists(
        select 1 from #tmp s where s.ID = cp2.ID and s.WorkOrderForPlanningUkey = cp2.WorkOrderForPlanningUkey
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
            SendWebAPI(GetSciUrl(), SCIAPIThread, jsonBody, automationErrMsg);
        }

        /// <inheritdoc/>
        public static bool IsGensong_AutoWHFabricEnable => IsModuleAutomationEnable(SCISuppID, SCIModuleName);

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
            AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
            {
                apiThread = apiThread,
                suppAPIThread = SCIAPIThread,
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
            SendWebAPI(GetSciUrl(), SCIAPIThread, jsonBody, automationErrMsg);
        }
    }
}
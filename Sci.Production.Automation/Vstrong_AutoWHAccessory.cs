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
    public class Vstrong_AutoWHAccessory
    {
        private static readonly string VstrongSuppID = "3A0196";
        private static readonly string moduleName = "AutoWHAccessory";
        private static readonly string SCIAPIThread = "api/VstrongAutoWHAccessory/SentDataByApiTag";
        private static readonly string suppAPIThread = "snpvsinterface/services/pmstowms";
        private static readonly string URL = GetSupplierUrl(VstrongSuppID, moduleName);

        /// PMS的action對應廠商statusAPI: Confrim(New),Unconfrim(Delete),Delete(Delete),Update(Revise)
        /// <param name="dtDetail">表身資訊,需要有ukey</param>
        /// <param name="formName">P10...P99</param>
        /// <param name="statusAPI">給廠商的動作指令 New/Delete/Revise/Lock/Unlock</param>
        /// <param name="action">PMS 的操作 Confrim, Unconfrim, (P99) Delete, Update</param>
        /// <inheritdoc/>
        public static bool Sent(bool doTask, DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, bool isP99 = false, int typeCreateRecord = 0, List<AutoRecord> autoRecord = null)
        {
            if (!Prgs.NoVstrong(formName))
            {
                return true;
            }

            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count == 0)
            {
                return false;
            }

            if (doTask)
            {
                Task.Run(() => Sent_Task(dtDetail, formName, statusAPI, action, isP99, typeCreateRecord, autoRecord: autoRecord))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                return true;
            }
            else
            {
                return Sent_Task(dtDetail, formName, statusAPI, action, isP99, typeCreateRecord, autoRecord: autoRecord);
            }
        }

        private static bool Sent_Task(DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, bool isP99, int typeCreateRecord = 0, List<AutoRecord> autoRecord = null)
        {
            AutoRecord autoRecordbyType = null;
            if (typeCreateRecord == 2)
            {
                autoRecordbyType = autoRecord.Where(w => w.fabricType == "A").FirstOrDefault();
                if (autoRecordbyType != null)
                {
                    LogicAutoWHData.SentandUpdatebyAutomationCreateRecord(formName, statusAPI, action, autoRecordbyType, URL);
                }
            }
            else
            {
                // 取得資料
                DataTable dtMaster = LogicAutoWHData.GetWHData(dtDetail, formName, statusAPI, action, "A", false, isP99: isP99);
                if (dtMaster == null)
                {
                    return false;
                }

                if (dtMaster.Rows.Count == 0)
                {
                    return true;
                }

                // 一次傳太大量會 Timeout 拆100傳出去
                DataTable[] splittedtables = dtMaster.AsEnumerable()
                    .Select((row, index) => new { row, index })
                    .GroupBy(x => x.index / 100)
                    .Select(g => g.Select(x => x.row).CopyToDataTable())
                    .ToArray();

                if (autoRecord != null)
                {
                    autoRecord.Add(new AutoRecord { fabricType = "A", automationCreateRecordUkey = new List<string>(), wh_Detail_Ukey = new List<string>() });
                    autoRecordbyType = autoRecord.Where(w => w.fabricType == "A").FirstOrDefault();
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

            return true;
        }

        private static bool SentandUpdate(DataTable dt, string formName, EnumStatus statusAPI, EnumStatus action, int typeCreateRecord, AutoRecord autoRecord = null)
        {
            // DataTable轉化為JSON
            WHTableName dtNameforAPI = LogicAutoWHData.GetDetailNameforAPI(formName);
            string jsonBody = GetJsonBody(dt, dtNameforAPI, statusAPI);
            AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
            {
                apiThread = $"Sent{dtNameforAPI}ToVstrong",
                suppAPIThread = statusAPI == EnumStatus.New ? SCIAPIThread : suppAPIThread,
                moduleName = moduleName,
                suppID = VstrongSuppID,
            };

            switch (typeCreateRecord)
            {
                case 0:
                    if (!LogicAutoWHData.SendWebAPI_Status(statusAPI, URL, automationErrMsg, jsonBody))
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
            Vstrong_AutoWHAccessory callMethod = new Vstrong_AutoWHAccessory();
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
                       ID = dr["ID"].ToString(),
                       Type = dr["Type"].ToString(),
                       PoId = dr["PoId"].ToString(),
                       Seq1 = dr["Seq1"].ToString(),
                       Seq2 = dr["Seq2"].ToString(),
                       Color = dr["Color"].ToString().TrimEnd(),
                       SizeCode = dr["SizeCode"].ToString().TrimEnd(),
                       StockType = dr["StockType"].ToString(),
                       Qty = (decimal)dr["Qty"],
                       StockPoId = dr["StockPoId"].ToString(),
                       StockSeq1 = dr["StockSeq1"].ToString(),
                       StockSeq2 = dr["StockSeq2"].ToString(),
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
                            Ukey = (long)dr["Ukey"],
                            StockType = dr["StockType"].ToString(),
                            QtyBefore = (decimal)dr["QtyBefore"],
                            QtyAfter = (decimal)dr["QtyAfter"],
                            Status = statusAPI.ToString(),
                            CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        });
                    break;
                case WHTableName.RemoveC_Detail:
                    bodyObject = dtDetail.AsEnumerable()
                        .Select(dr => new
                        {
                            Id = dr["id"].ToString(),
                            PoId = dr["PoId"].ToString(),
                            Seq1 = dr["Seq1"].ToString(),
                            Seq2 = dr["Seq2"].ToString(),
                            Qty = (decimal)dr["Qty"],
                            Ukey = (long)dr["Ukey"],
                            StockType = dr["StockType"].ToString(),
                            Status = statusAPI.ToString(),
                            CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        });
                    break;
                case WHTableName.SubTransfer_Detail:
                    bodyObject = dtDetail.AsEnumerable()
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
                       Qty = (decimal)dr["Qty"],
                       StockType = dr["StockType"].ToString(),
                       Ukey = (long)dr["Ukey"],
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
                       StockType = s["StockType"].ToString(),
                       Ukey = (long)s["Ukey"],
                       Refno = s["Refno"].ToString(),
                       StockUnit = s["StockUnit"].ToString(),
                       Color = s["Color"].ToString(),
                       SizeCode = s["SizeCode"].ToString(),
                       MtlType = s["MtlType"].ToString(),
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
                       StockType = s["StockType"].ToString(),
                       Ukey = (long)s["Ukey"],
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
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName))
            {
                return;
            }

            AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
            {
                apiThread = "SentWHCloseToVstrong",
                suppAPIThread = suppAPIThread,
                moduleName = moduleName,
                suppID = VstrongSuppID,
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
                Task.Run(() => SendWebAPI(GetSciUrl(), automationErrMsg.suppAPIThread, jsonBody, automationErrMsg))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                SendWebAPI(GetSciUrl(), automationErrMsg.suppAPIThread, jsonBody, automationErrMsg);
            }
        }
    }
}

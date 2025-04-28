using Ict;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    internal class Prgs_WMS
    {
        /// <summary>
        /// P21/P26/P73 調整前 Tolocation 不是自動倉, 要發給 WMS 要求撤回(Lock)
        /// </summary>
        /// <inheritdoc/>
        public static bool LockNotWMS(DataTable dtnotWMS)
        {
            // 找出要撤回的 P07 Ukey
            DataTable dt07 = Prgs.GetWHDetailUkey(dtnotWMS, "P07");

            // 找出要撤回的 P18 Ukey
            DataTable dt18 = Prgs.GetWHDetailUkey(dtnotWMS, "P18");

            // 找出要撤回的 P70 Ukey
            DataTable dt70 = Prgs.GetWHDetailUkey(dtnotWMS, "P70");

            if (dt07.Rows.Count > 0)
            {
                Prgs.GetFtyInventoryData(dt07, "P07", out DataTable dtOriFtyInventoryP07);
                if (!Prgs_WMS.WMSLock(dt07, dtOriFtyInventoryP07, "P07", EnumStatus.Unconfirm))
                {
                    return false;
                }
            }

            if (dt18.Rows.Count > 0)
            {
                Prgs.GetFtyInventoryData(dt18, "P18", out DataTable dtOriFtyInventoryP18);
                if (!Prgs_WMS.WMSLock(dt18, dtOriFtyInventoryP18, "P18", EnumStatus.Unconfirm))
                {
                    return false;
                }
            }

            if (dt70.Rows.Count > 0)
            {
                Prgs.GetLocalOrderInventoryData(dt70, "P70", out DataTable dtOriLocalOrderInventoryP70);
                if (!Prgs_WMS.WMSLock(dt70, dtOriLocalOrderInventoryP70, "P70", EnumStatus.Unconfirm))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// P21/P26 調整 Tolocation 不是自動倉, 過程有任何錯誤, 要發給 WMS 要求(UnLock)
        /// P21/P26 調整後 Tolocation 不是自動倉, 要發給 WMS 要求撤回(Delete)
        /// </summary>
        /// <inheritdoc/>
        public static void UnLockorDeleteNotWMS(DataTable dtnotWMS, EnumStatus statusAPI, List<AutoRecord> autoRecordListP07, List<AutoRecord> autoRecordListP18, int typeCreateRecord)
        {
            // 找出要撤回的 P07 Ukey
            DataTable dt07 = Prgs.GetWHDetailUkey(dtnotWMS, "P07");

            // 找出要撤回的 P18 Ukey
            DataTable dt18 = Prgs.GetWHDetailUkey(dtnotWMS, "P18");

            Gensong_AutoWHFabric.Sent(false, dt07, "P07", statusAPI, EnumStatus.Unconfirm, typeCreateRecord: typeCreateRecord, autoRecord: autoRecordListP07);
            Gensong_AutoWHFabric.Sent(false, dt18, "P18", statusAPI, EnumStatus.Unconfirm, typeCreateRecord: typeCreateRecord, autoRecord: autoRecordListP18);
            Vstrong_AutoWHAccessory.Sent(false, dt07, "P07", statusAPI, EnumStatus.Unconfirm, typeCreateRecord: typeCreateRecord, autoRecord: autoRecordListP07);
            Vstrong_AutoWHAccessory.Sent(false, dt18, "P18", statusAPI, EnumStatus.Unconfirm, typeCreateRecord: typeCreateRecord, autoRecord: autoRecordListP18);
        }

        /// <summary>
        /// P21 調整是自動倉, 過程有任何錯誤, 要發給 WMS 要求(UnLock)
        /// </summary>
        /// <inheritdoc/>
        public static void UnLockWMS(DataTable dtnotWMS, EnumStatus statusAPI, List<AutoRecord> autoRecordListP07, List<AutoRecord> autoRecordListP18)
        {
            // 找出要撤回的 P07 Ukey
            DataTable dt07 = Prgs.GetWHDetailUkey(dtnotWMS, "P07");

            // 找出要撤回的 P18 Ukey
            DataTable dt18 = Prgs.GetWHDetailUkey(dtnotWMS, "P18");

            if (dt07.Rows.Count > 0)
            {
                Prgs.GetFtyInventoryData(dt07, "P07", out DataTable dtOriFtyInventoryP07);
                Prgs_WMS.WMSUnLock(false, dt07, "P07", EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventoryP07);
            }

            if (dt18.Rows.Count > 0)
            {
                Prgs.GetFtyInventoryData(dt18, "P18", out DataTable dtOriFtyInventoryP18);
                Prgs_WMS.WMSUnLock(false, dt18, "P18", EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventoryP18);
            }
        }

        /// <summary>
        /// P21 調整後 Tolocation 是自動倉, 要發給 WMS 要求修正(Revise)
        /// </summary>
        /// <inheritdoc/>
        public static void ReviseWMS(DataTable dtnotWMS, List<AutoRecord> autoRecordListP07, List<AutoRecord> autoRecordListP18, int typeCreateRecord)
        {
            // 找出要Revise的 P07 Ukey
            DataTable dt07 = Prgs.GetWHDetailUkey(dtnotWMS, "P07");

            // 找出要Revise的 P18 Ukey
            DataTable dt18 = Prgs.GetWHDetailUkey(dtnotWMS, "P18");

            Gensong_AutoWHFabric.Sent(false, dt07, "P07", EnumStatus.Revise, EnumStatus.Confirm, typeCreateRecord: typeCreateRecord, autoRecord: autoRecordListP07);
            Gensong_AutoWHFabric.Sent(false, dt18, "P18", EnumStatus.Revise, EnumStatus.Confirm, typeCreateRecord: typeCreateRecord, autoRecord: autoRecordListP18);
        }

        /// <summary>
        /// P73 調整 Tolocation 不是自動倉, 過程有任何錯誤, 要發給 WMS 要求(UnLock)
        /// P73 調整後 Tolocation 不是自動倉, 要發給 WMS 要求撤回(Delete)
        /// </summary>
        /// <inheritdoc/>
        public static void UnLockorDeleteNotWMS_LocalOrder(DataTable dtnotWMS, EnumStatus statusAPI, List<AutoRecord> autoRecordListP70, int typeCreateRecord)
        {
            // 找出要撤回的 P70 Ukey
            DataTable dt70 = Prgs.GetWHDetailUkey(dtnotWMS, "P70");

            Gensong_AutoWHFabric.Sent(false, dt70, "P70", statusAPI, EnumStatus.Unconfirm, typeCreateRecord: typeCreateRecord, autoRecord: autoRecordListP70);
            Vstrong_AutoWHAccessory.Sent(false, dt70, "P70", statusAPI, EnumStatus.Unconfirm, typeCreateRecord: typeCreateRecord, autoRecord: autoRecordListP70);
        }

        /// <inheritdoc/>
        public static bool WMSprocess(bool doTask, DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, DataTable dthasFabricType = null, bool isP99 = false, bool fromNewBarcode = false, int typeCreateRecord = 0, List<AutoRecord> autoRecord = null)
        {
            dthasFabricType = dthasFabricType ?? dtDetail;
            List<string> fabricList = dthasFabricType.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["FabricType"])).ToList();
            if (Prgs.IsAutomation())
            {
                if (Prgs.NoGensong(formName) && fabricList.Contains("F"))
                {
                    Gensong_AutoWHFabric.Sent(doTask, dtDetail, formName, statusAPI, action, isP99: isP99, fromNewBarcode: fromNewBarcode, typeCreateRecord: typeCreateRecord, autoRecord: autoRecord);
                }

                if (Prgs.NoVstrong(formName) && fabricList.Contains("A"))
                {
                    Vstrong_AutoWHAccessory.Sent(doTask, dtDetail, formName, statusAPI, action, isP99: isP99, typeCreateRecord: typeCreateRecord, autoRecord: autoRecord);
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool WMSLock(DataTable dtDetail, DataTable dthasFabricType, string formName, EnumStatus action, bool isP99 = false, DataTable dtDetailA = null)
        {
            List<string> fabricList = dthasFabricType.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["FabricType"])).ToList();
            if (Prgs.IsAutomation())
            {
                // 避免解鎖到不是這次的資訊
                bool fprocess = false;
                bool aprocess = false;
                bool fsuccess = true;
                bool asuccess = true;
                if (Prgs.NoGensong(formName) && fabricList.Contains("F"))
                {
                    fprocess = true;
                    fsuccess = Gensong_AutoWHFabric.Sent(false, dtDetail, formName, EnumStatus.Lock, action, isP99: isP99);
                }

                if (!fsuccess)
                {
                    return false;
                }

                if (Prgs.NoVstrong(formName) && fabricList.Contains("A"))
                {
                    aprocess = true;
                    if (dtDetailA != null)
                    {
                        asuccess = Vstrong_AutoWHAccessory.Sent(false, dtDetailA, formName, EnumStatus.Lock, action, isP99: isP99);
                    }
                    else
                    {
                        asuccess = Vstrong_AutoWHAccessory.Sent(false, dtDetail, formName, EnumStatus.Lock, action, isP99: isP99);
                    }
                }

                if (aprocess && !asuccess)
                {
                    if (fprocess && fsuccess)
                    {
                        Gensong_AutoWHFabric.Sent(false, dtDetail, formName, EnumStatus.UnLock, action, isP99: isP99);
                    }

                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public static void WMSUnLock(bool doTask, DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, DataTable dthasFabricType = null, bool isP99 = false, bool fromNewBarcode = false)
        {
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            WMSprocess(doTask, dtDetail, formName, statusAPI, action, dthasFabricType, isP99, fromNewBarcode, 1, autoRecordList); // 先寫入DB AutomationCreateRecord ( Json ... 等)
            WMSprocess(doTask, dtDetail, formName, statusAPI, action, dthasFabricType, isP99, fromNewBarcode, 2, autoRecordList); // 再傳API(這可能會很久)
        }

        /// <summary>
        /// ISP20221590 檢查FtyInventory.SubConStatus
        /// </summary>
        /// <param name="listFtyInventoryUkey">listFtyInventoryUkey</param>
        /// <returns>bool</returns>
        public static bool CheckFtyInventorySubConStatus(List<long> listFtyInventoryUkey)
        {
            if (listFtyInventoryUkey.Count() == 0)
            {
                return true;
            }

            string whereFtyInventoryUkey = listFtyInventoryUkey.Select(s => $"'{s}'").JoinToString(",");
            string sqlCheck = $@"
select  [SP#] = f.POID,
        [Seq] = Concat(f.Seq1, ' ', f.Seq2),
        f.Roll,
        f.Dyelot,
        [Sub con status] = f.SubConStatus
from FtyInventory f with (nolock)
where   f.Ukey in ({whereFtyInventoryUkey}) and
        f.SubConStatus <> ''
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlCheck, out dtResult);

            if (!result)
            {
                MyUtility.Msg.getExceptionMsg(result.GetException());
            }

            if (dtResult.Rows.Count == 0)
            {
                return true;
            }

            MyUtility.Msg.ShowMsgGrid(dtResult, "Fabric transfer to sub con not return yet.", "SubConStatus");
            return false;
        }
    }
}

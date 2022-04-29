using Ict;
using Sci.Production.Automation;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    internal class Prgs_WMS
    {
        /// <summary>
        /// P21/P26 調整後 Tolocation 不是自動倉, 要發給 WMS 要求撤回(Delete)
        /// </summary>
        /// <inheritdoc/>
        public static void DeleteNotWMS(DataTable dtnotWMS)
        {
            // 找出要撤回的 P07 Ukey
            DataTable dt07 = Prgs.GetWHDetailUkey(dtnotWMS, "P07");

            // 找出要撤回的 P18 Ukey
            DataTable dt18 = Prgs.GetWHDetailUkey(dtnotWMS, "P18");

            Prgs.GetFtyInventoryData(dt07, "P07", out DataTable dtOriFtyInventoryP07);
            if (!Prgs_WMS.WMSLock(dt07, dtOriFtyInventoryP07, "P07", EnumStatus.Unconfirm))
            {
                return;
            }

            Prgs.GetFtyInventoryData(dt18, "P18", out DataTable dtOriFtyInventoryP18);
            if (!Prgs_WMS.WMSLock(dt18, dtOriFtyInventoryP18, "P18", EnumStatus.Unconfirm))
            {
                return;
            }

            Gensong_AutoWHFabric.Sent(true, dt07, "P07", EnumStatus.Delete, EnumStatus.Unconfirm, true);
            Gensong_AutoWHFabric.Sent(true, dt18, "P18", EnumStatus.Delete, EnumStatus.Unconfirm, true);
            Vstrong_AutoWHAccessory.Sent(true, dt07, "P07", EnumStatus.Delete, EnumStatus.Unconfirm, true);
            Vstrong_AutoWHAccessory.Sent(true, dt18, "P18", EnumStatus.Delete, EnumStatus.Unconfirm, true);
        }

        /// <inheritdoc/>
        public static bool WMSprocess(bool doTask, DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, DataTable dthasFabricType = null, bool isP99 = false, bool fromNewBarcode = false)
        {
            dthasFabricType = dthasFabricType ?? dtDetail;
            List<string> fabricList = dthasFabricType.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["FabricType"])).ToList();
            if (Prgs.IsAutomation())
            {
                if (Prgs.NoGensong(formName) && fabricList.Contains("F"))
                {
                    Gensong_AutoWHFabric.Sent(doTask, dtDetail, formName, statusAPI, action, isP99: isP99, fromNewBarcode: fromNewBarcode);
                }

                if (Prgs.NoVstrong(formName) && fabricList.Contains("A"))
                {
                    Vstrong_AutoWHAccessory.Sent(doTask, dtDetail, formName, statusAPI, action, isP99: isP99);
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
                        Gensong_AutoWHFabric.Sent(true, dtDetail, formName, EnumStatus.UnLock, action, isP99: isP99);
                    }

                    return false;
                }
            }

            return true;
        }
    }
}

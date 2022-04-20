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

            Gensong_AutoWHFabric.Sent(true, dt07, "P07", EnumStatus.Delete, EnumStatus.Unconfirm, true);
            Gensong_AutoWHFabric.Sent(true, dt18, "P18", EnumStatus.Delete, EnumStatus.Unconfirm, true);
            Vstrong_AutoWHAccessory.Sent(true, dt07, "P07", EnumStatus.Delete, EnumStatus.Unconfirm, true);
            Vstrong_AutoWHAccessory.Sent(true, dt18, "P18", EnumStatus.Delete, EnumStatus.Unconfirm, true);
        }

        /// <inheritdoc/>
        public static bool WMSprocess(bool doTask, DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, DataTable dthasFabricType = null, bool fromNewBarcode = false)
        {
            dthasFabricType = dthasFabricType ?? dtDetail;
            List<string> fabricList = dthasFabricType.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["FabricType"])).ToList();
            if (Prgs.IsAutomation())
            {
                if (NoGensong(formName) && fabricList.Contains("F"))
                {
                    Gensong_AutoWHFabric.Sent(doTask, dtDetail, formName, statusAPI, action, fromNewBarcode: fromNewBarcode);
                }

                if (NoVstrong(formName) && fabricList.Contains("A"))
                {
                    Vstrong_AutoWHAccessory.Sent(doTask, dtDetail, formName, statusAPI, action);
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool WMSLock(DataTable dtDetail, DataTable dthasFabricType, string formName, EnumStatus action)
        {
            List<string> fabricList = dthasFabricType.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["FabricType"])).ToList();
            if (Prgs.IsAutomation())
            {
                // 避免解鎖到不是這次的資訊
                bool fprocess = false;
                bool aprocess = false;
                bool fsuccess = true;
                bool asuccess = true;
                if (NoGensong(formName) && fabricList.Contains("F"))
                {
                    fprocess = true;
                    fsuccess = Gensong_AutoWHFabric.Sent(false, dtDetail, formName, EnumStatus.Lock, action);
                }

                if (!fsuccess)
                {
                    return false;
                }

                if (NoVstrong(formName) && fabricList.Contains("A"))
                {
                    aprocess = true;
                    asuccess = Vstrong_AutoWHAccessory.Sent(false, dtDetail, formName, EnumStatus.Lock, action);
                }

                if (aprocess && !asuccess)
                {
                    if (fprocess && fsuccess)
                    {
                        Gensong_AutoWHFabric.Sent(true, dtDetail, formName, EnumStatus.UnLock, action);
                    }

                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool NoGensong(string formName)
        {
            switch (formName)
            {
                case "P11":
                case "P12":
                case "P33":
                case "P15":
                case "P43":
                case "P45":
                case "P48":
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool NoVstrong(string formName)
        {
            switch (formName)
            {
                case "P07_ModifyRollDyelot":
                case "P10":
                case "P62":
                case "P16":
                    return false;
            }

            return true;
        }
    }
}

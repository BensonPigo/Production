using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02, P09共用
    /// </summary>
    public partial class CuttingWorkOrder
    {
        /// <summary>
        /// 這直接複製到P09, 會用到有4個地方
        /// 1. BtnAutoDistributeSP
        /// 2. P09_AutoDistToSP
        /// 3. P09 OnDetailGridDelete 傳入參數(this.sizeratioTb, ukey, newKey);
        /// 4. P09 OnDetailGridDelete 傳入參數(this.distqtyTb, ukey, newKey);
        /// </summary>
        internal static void DeleteThirdDatas(DataTable thirdTable, long workOrderUkey, long newKey)
        {
            DataRow[] byKeyThirdDatas = thirdTable.Select($"WorkOrderUkey = '{workOrderUkey}' and NewKey = {newKey}");
            for (int i = byKeyThirdDatas.Count() - 1; i >= 0; i--)
            {
                byKeyThirdDatas[i].Delete();
            }
        }

        /// <summary>
        /// 這直接複製到P09, 然後用Button[All SP# Distribute]直接呼叫它
        /// </summary>
        //private static void BtnAutoDistributeSPClick()
        //{
        //    if (!this.EditMode)
        //    {
        //        return;
        //    }

        //    this.detailgrid.EndEdit();

        //    // 先將可分配的WorkOrderForOutput 下面的WorkOrderForOutput_Distribute清空
        //    foreach (DataRow drWorkOrder in this.DetailDatas)
        //    {
        //        drWorkOrder["CanDoAutoDistribute"] = false;

        //        // 有CutplanID不清空
        //        if (!MyUtility.Check.Empty(drWorkOrder["CutplanID"]))
        //        {
        //            continue;
        //        }

        //        // 有建立bundle不清空
        //        if (!MyUtility.Check.Empty(drWorkOrder["CutRef"]) &&
        //            MyUtility.Check.Seek($"select 1 from Bundle with (nolock) where CutRef = '{drWorkOrder["CutRef"]}' and POID = '{this.CurrentMaintain["ID"]}'"))
        //        {
        //            continue;
        //        }

        //        drWorkOrder["CanDoAutoDistribute"] = true;
        //        DeleteThirdDatas(this.distqtyTb, MyUtility.Convert.GetLong(drWorkOrder["Ukey"]), MyUtility.Convert.GetLong(drWorkOrder["NewKey"]));
        //    }

        //    // 開始重新分配WorkOrderForOutput_Distribute
        //    foreach (DataRow drWorkOrder in this.DetailDatas.Where(s => MyUtility.Convert.GetBool(s["CanDoAutoDistribute"])))
        //    {
        //        var p09_AutoDistToSP = new P09_AutoDistToSP(drWorkOrder, this.sizeratioTb, this.distqtyTb, this.PatternPanelTb);
        //        DualResult result = p09_AutoDistToSP.DoAutoDistribute();
        //        if (!result)
        //        {
        //            this.ShowErr(result);
        //            return;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 這直接複製到P09, 然後用Button[Distribute This CutRef]直接呼叫它
        ///// </summary>
        //private static void BtnDistClick()
        //{
        //    this.GridValid();
        //    this.detailgrid.ValidateControl();
        //    var frm = new P09_AutoDistToSP(this.CurrentDetailData, this.sizeratioTb, this.distqtyTb, this.PatternPanelTb);
        //    frm.ShowDialog(this);
        //}
    }
}

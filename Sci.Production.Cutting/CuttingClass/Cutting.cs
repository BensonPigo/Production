using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public static class Cutting
    {
        /// <summary>
        /// 重編 Cutting P31 SpreadingSchdlSeq
        /// </summary>
        /// <inheritdoc/>
        public static void SpreadingSchdlSeq(IEnumerable<DataRow> detailDatas)
        {
            if (detailDatas.Count() == 0)
            {
                return;
            }

            var x = detailDatas
                .Where(w => MyUtility.Convert.GetString(w["Completed"]) == "N")
                .OrderBy(o => MyUtility.Convert.GetDate(o["EstCutDate"]))
                .ThenBy(o => MyUtility.Convert.GetString(o["OrderID"]))
                .ThenBy(o => MyUtility.Convert.GetDate(o["BuyerDelivery"]))
                .ThenBy(o => MyUtility.Convert.GetString(o["FabricCombo"]))
                .ThenBy(o => MyUtility.Convert.GetDecimal(o["Cutno"]));

            int i = detailDatas
                .Where(w => MyUtility.Convert.GetString(w["Completed"]) == "Y")
                .Select(s => MyUtility.Convert.GetInt(s["SpreadingSchdlSeq"]))
                .OrderByDescending(o => o)
                .FirstOrDefault() + 1;

            x.ToList().ForEach(f => f["SpreadingSchdlSeq"] = DBNull.Value);

            var distinctCutRef = x.Select(s => s["CutRef"].ToString()).Distinct();
            foreach (string cutRef in distinctCutRef)
            {
                if (i > 999)
                {
                    break;
                }

                var sameCutref = x.Where(w => MyUtility.Check.Empty(w["SpreadingSchdlSeq"])
                    && MyUtility.Convert.GetString(w["CutRef"]) == cutRef);
                foreach (DataRow cdr in sameCutref)
                {
                    cdr["SpreadingSchdlSeq"] = i;
                }

                i++;
            }
        }

        /// <summary>
        /// 從原本 P31 SavePost 原封不動搬過來
        /// 給 P31_ReviseSchedule 能走同樣流程使用
        /// 使用要包在 TransactionScope
        /// 此 function 是把表身轉到其它單下
        ///   例子: P31操作,兩張單都(大於 操作Today) 12/05, 12/06, 將 12/06(原單) 部分轉移到 12/05(新單)
        ///   1.在 12/05(新單) 編輯新增一筆輸入原本在 12/06 的 Cutref
        ///   2.按 Default 按鈕重新編碼(新單) SpreadingSchdlSeq
        ///   3.按存檔在 savepost 階段 兩張單都存在將轉移的 Cutref
        ///   3-1 先傳送原單的表頭資訊給廠商 (使用DeleteSpreadingSchedule)
        ///   3-2 刪除 DB 中原單表身轉移的 Cutref
        ///   3-3 把原單的表頭資訊再傳一次給廠商 (使用 SendSpreadingSchedule)
        ///   4.傳送轉移的 Cutref 新單表頭資訊 (使用SendSpreadingSchedule)
        /// </summary>
        /// <inheritdoc/>
        public static DualResult P31SavePost(long ukeyNew, string factorNew, DateTime estCutDateNew, string cutCellIDNew)
        {
            // 1.找出新單所有 CutRef 對應的原單表頭，這些將會刪除的單資訊先撈出，Call廠商API做整單刪除
            string sqlDeleteList = $@"
declare @today date = getdate()

SELECT DISTINCT
    ss.FactoryID
   ,ss.EstCutDate
   ,ss.CutCellID
FROM SpreadingSchedule ss WITH (NOLOCK)
INNER JOIN SpreadingSchedule_Detail ssd WITH (NOLOCK) ON ss.Ukey = ssd.SpreadingScheduleUkey
WHERE ss.Ukey <> {ukeyNew}
AND ss.EstCutDate > @today
AND ssd.IsAGVArrived = 0
AND EXISTS (SELECT 1 FROM SpreadingSchedule_Detail WITH (NOLOCK) WHERE CutRef = ssd.CutRef AND SpreadingScheduleUkey = {ukeyNew}) -- 新單所有 CutRef
";
            DualResult result = DBProxy.Current.Select(null, sqlDeleteList, out DataTable dt);
            if (!result)
            {
                return result;
            }

            foreach (DataRow dr in dt.Rows)
            {
                result = new Gensong_SpreadingSchedule().DeleteSpreadingSchedule(dr["FactoryID"].ToString(), (DateTime)dr["EstCutDate"], dr["CutCellID"].ToString());
                if (!result)
                {
                    return result;
                }
            }

            // 2.檢查此次有存檔CutRef是否有其它的日期，若有就刪除，已完成與這次維護的資料不刪除 (ISP20210219)
            string sqlDeleteSameFutureCutRef = $@"
declare @today date = getdate()

DELETE ssd
FROM SpreadingSchedule ss WITH (NOLOCK)
INNER JOIN SpreadingSchedule_Detail ssd WITH (NOLOCK) ON ss.Ukey = ssd.SpreadingScheduleUkey
WHERE ss.Ukey <> {ukeyNew}
AND ss.EstCutDate > @today
AND ssd.IsAGVArrived = 0
AND EXISTS (SELECT 1 FROM SpreadingSchedule_Detail WITH (NOLOCK) WHERE CutRef = ssd.CutRef AND SpreadingScheduleUkey = {ukeyNew}) -- 新單所有 CutRef
";
            result = DBProxy.Current.Execute(null, sqlDeleteSameFutureCutRef);
            if (!result)
            {
                return result;
            }

            // 3.呼叫中間API再把這些單重新傳給廠商新增 (呼叫中間API會依據Key重撈資料)
            foreach (DataRow dr in dt.Rows)
            {
                result = new Gensong_SpreadingSchedule().SendSpreadingSchedule(dr["FactoryID"].ToString(), (DateTime)dr["EstCutDate"], dr["CutCellID"].ToString());
                if (!result)
                {
                    return result;
                }
            }

            // 4.呼叫中間API 傳送當前這張單
            result = new Gensong_SpreadingSchedule().SendSpreadingSchedule(factorNew, estCutDateNew, cutCellIDNew);
            if (!result)
            {
                return result;
            }

            return Result.True;
        }
    }
}
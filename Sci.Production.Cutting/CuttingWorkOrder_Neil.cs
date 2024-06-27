using Ict;
using Sci.Data;
using Sci.Production.Automation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02, P09共用
    /// </summary>
    public partial class CuttingWorkOrder
    {
        /// <summary>
        /// 自動編碼CutNo。當前行的 "CutNo" 是空的並且 "EstCutDate" 不為空，根據 "FabricCombo" 計算當前最大 "CutNo"
        /// </summary>
        /// <param name="detailDatas">表身</param>
        /// <param name="form">來源Form</param>
        public static void AutoCut(DataTable detailDatas, CuttingForm form)
        {
            if (form == CuttingForm.P02)
            {
                return;
            }

            foreach (DataRow dr in detailDatas.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                if (MyUtility.Check.Empty(dr["CutNo"]) && !MyUtility.Check.Empty(dr["EstCutDate"]))
                {
                    string maxCutNoStr = detailDatas.Compute($"Max(CutNo)", $@"FabricCombo ='{dr["FabricCombo"]}'").ToString();
                    int maxcutno;

                    if (MyUtility.Check.Empty(maxCutNoStr) || !int.TryParse(maxCutNoStr, out maxcutno))
                    {
                        maxcutno = 1;
                    }
                    else
                    {
                        maxcutno += 1;
                    }

                    dr["CutNo"] = maxcutno;
                }
            }
        }

        /// <summary>
        /// 自動編碼CutRef，並Call API傳給廠商
        /// </summary>
        /// <param name="cuttingID">Cutting.ID</param>
        /// <param name="mDivision">MDivision</param>
        /// <param name="detailDatas">表身</param>
        /// <param name="form">form</param>
        /// <returns>result</returns>
        public static DualResult AutoRef(string cuttingID, string mDivision, DataTable detailDatas, CuttingForm form)
        {
            DataTable dtWorkOrder = detailDatas.AsEnumerable().CopyToDataTable();

            // 根據功能區分 TableName，Pkey Column Name
            string colName = string.Empty;
            string where = string.Empty;
            string cmdWhere = string.Empty;
            string outerApply = string.Empty;
            string nColumn = string.Empty;

            string workOrder_tableName = $@"WorkorderFor{GetWorkOrderName(form)}";
            string tableKey = GetWorkOrderUkeyName(form);

            // 額外條件
            switch (form)
            {
                case CuttingForm.P02:
                    colName = "CutPlanID";
                    where = string.Empty;
                    cmdWhere = "AND (CutPlanID IS NULL OR CutPlanID = '')";
                    nColumn = string.Empty;
                    outerApply = string.Empty;
                    break;

                // 不存在 P10 & 不存在 P20 & 不存在 P05 & WorkorderForOutput.SpreadingStatus = 'Ready'
                case CuttingForm.P09:
                    colName = "CutNo";
                    where = "And HasBundle = 0 And HasCuttingOutput = 0 And HasMarkerReq = 0 And SpreadingStatus = 'Ready'";
                    cmdWhere = "AND CutNo IS NOT NULL";
                    nColumn = ", ws.SizeRatio";
                    outerApply = $@"
OUTER APPLY (
    SELECT STUFF((
        SELECT ',' + b.SizeCode + ':' + CAST(b.Qty AS VARCHAR)
        FROM (
            SELECT SizeCode, Qty 
            FROM {workOrder_tableName}_SizeRatio ws 
            WHERE ws.{tableKey} = w.Ukey AND w.ID = ws.ID
        ) b FOR XML PATH('')
    ), 1, 1, '') AS SizeRatio
) ws";
                    break;
            }

            #region 找出相同 CutRef 的群組
            string cmdsql = $@"
SELECT
    w.CutRef,
    w.FabricCombo,
    w.MarkerName, 
    w.EstCutDate,
    w.{colName}
    {nColumn}
FROM #tmpWorkOrder w WITH (NOLOCK) 
{outerApply}
WHERE w.CutRef <> '' 
AND w.id = '{cuttingID}' AND w.mDivisionid = '{mDivision}'
{cmdWhere}
GROUP BY w.CutRef, w.FabricCombo, w.{colName}, w.MarkerName, w.EstCutDate {nColumn}";
            DualResult cutRefresult = MyUtility.Tool.ProcessWithDatatable(dtWorkOrder, string.Empty, cmdsql, out DataTable cutReftb, "#tmpWorkOrder");
            if (!cutRefresult)
            {
                return cutRefresult;
            }
            #endregion

            #region 找出空的CutRef
            cmdsql = $@"
SELECT * 
FROM #tmpWorkOrder w WITH (NOLOCK) 
{outerApply}
WHERE (w.CutRef IS NULL OR w.CutRef = '') 
        AND (w.EstCutDate IS NOT NULL AND w.EstCutDate != '')
        {where}
        {cmdWhere}
        AND w.id = '{cuttingID}' AND w.mDivisionid = '{mDivision}'
ORDER BY w.FabricCombo, w.{colName}";

            cutRefresult = MyUtility.Tool.ProcessWithDatatable(dtWorkOrder, string.Empty, cmdsql, out DataTable workordertmp, "#tmpWorkOrder");
            if (!cutRefresult)
            {
                return cutRefresult;
            }
            #endregion

            #region 組合SQL：寫入空的CutRef

            string updateCutRef = $@"
CREATE TABLE #tmp{workOrder_tableName} (Ukey BIGINT);
DECLARE @chk TINYINT = 0;
BEGIN TRANSACTION [Trans_Name];";

            foreach (DataRow dr in workordertmp.Rows)
            {
                string newCutRef = string.Empty;
                string maxref = Sci.Production.PublicPrg.Prgs.GetColumnValueNo($"{workOrder_tableName}", "CutRef");
                if (form == CuttingForm.P02)
                {
                    newCutRef = maxref;
                }
                else
                {
                    DataRow[] findrow = cutReftb.Select($@"MarkerName = '{dr["MarkerName"]}' AND FabricCombo = '{dr["FabricCombo"]}' AND CutNo = {dr["CutNo"]} AND EstCutDate = '{dr["EstCutDate"]}' AND SizeRatio = '{dr["SizeRatio"]}'");
                    if (findrow.Length != 0)
                    {
                        newCutRef = findrow[0]["CutRef"].ToString();
                    }
                    else
                    {
                        DataRow newdr = cutReftb.NewRow();
                        newdr["MarkerName"] = dr["MarkerName"] ?? string.Empty;
                        newdr["FabricCombo"] = dr["FabricCombo"] ?? string.Empty;
                        newdr["Cutno"] = dr["Cutno"];
                        newdr["EstCutDate"] = dr["EstCutDate"] ?? DBNull.Value;
                        newdr["CutRef"] = maxref;
                        newdr["SizeRatio"] = dr["SizeRatio"];
                        cutReftb.Rows.Add(newdr);
                        newCutRef = maxref;
                    }
                }

                updateCutRef += $@"
    IF (SELECT COUNT(1) FROM {workOrder_tableName} WITH (NOLOCK) WHERE CutRef = '{newCutRef}' AND id != '{cuttingID}') > 0
    BEGIN
        RAISERROR ('Duplicate CutRef. Please redo Auto Ref#', 12, 1);
        ROLLBACK TRANSACTION [Trans_Name];
    END
    UPDATE {workOrder_tableName} SET CutRef = '{newCutRef}' 
    OUTPUT INSERTED.Ukey INTO #tmp{workOrder_tableName}
    WHERE ukey = '{dr["ukey"]}';";
            }

            updateCutRef += $@"
IF @@ERROR <> 0 SET @chk = 1;
IF @chk <> 0
BEGIN
    ROLLBACK TRANSACTION [Trans_Name];
END
ELSE
BEGIN
    SELECT w.* 
    FROM #tmp{workOrder_tableName} tw
    INNER JOIN {workOrder_tableName} w WITH (NOLOCK) ON tw.Ukey = w.Ukey;
    COMMIT TRANSACTION [Trans_Name];
END";

            #endregion

            // 執行SQL & Call API
            DualResult result;
            DataTable dtWorkorder = new DataTable();
            result = DBProxy.Current.Select(null, updateCutRef, out dtWorkorder);
            if (!result)
            {
                if (result.ToString().Contains("Duplicate CutRef. Please redo Auto Ref#"))
                {
                    MyUtility.Msg.WarningBox("Duplicate CutRef. Please redo Auto Ref#");
                    return result;
                }
                else
                {
                    return result;
                }
            }

            if (dtWorkorder.Rows.Count > 0)
            {
                Task.Run(() => new Guozi_AGV().SentWorkOrderToAGV(dtWorkorder));
            }

            return result;
        }
    }
}

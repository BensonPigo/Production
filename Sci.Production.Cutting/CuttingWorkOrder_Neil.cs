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
        /// <param name="form">form</param>
        /// <returns>result</returns>
        public static DualResult AutoRef(string cuttingID, string mDivision, CuttingForm form)
        {
            // 根據功能區分 TableName，Pkey Column Name
            string colName = string.Empty;
            string where = string.Empty;
            string cmdWhere = string.Empty;

            string workOrder_tableName = $@"WorkorderFor{GetWorkOrderName(form)}";
            string tableKey = GetWorkOrderUkeyName(form);

            // 額外條件
            switch (form)
            {

                case CuttingForm.P02:
                    colName = "CutPlanID";
                    where = string.Empty;
                    cmdWhere = "AND (CutPlanID IS NULL OR CutPlanID = '')";
                    break;

                // 不存在 P10 & 不存在 P20 & 不存在 P05 & WorkorderForOutput.SpreadingStatus = 'Ready' & WorkorderForOutput.SourceFrom != 1
                case CuttingForm.P09:
                    colName = "CutNo";
                    where = "";
                    cmdWhere = "AND CutNo IS NOT NULL";
                    break;
            }

            #region 找出相同 CutRef 的群組
            string cmdsql = $@"
SELECT ISNULL(w.CutRef, '') AS CutRef, ISNULL(w.FabricCombo, '') AS FabricCombo, w.{colName},
        ISNULL(w.MarkerName, '') AS MarkerName, w.EstCutDate, ws.SizeRatio
FROM {workOrder_tableName} w WITH (NOLOCK) 
OUTER APPLY (
    SELECT STUFF((
        SELECT ',' + b.SizeCode + ':' + CAST(b.Qty AS VARCHAR)
        FROM (
            SELECT SizeCode, Qty 
            FROM {workOrder_tableName}_SizeRatio ws 
            WHERE ws.{tableKey} = w.Ukey AND w.ID = ws.ID
        ) b FOR XML PATH('')
    ), 1, 1, '') AS SizeRatio
) ws
WHERE w.CutRef IS NOT NULL AND w.CutRef != '' 
        AND w.id = '{cuttingID}' AND w.mDivisionid = '{mDivision}'
        {cmdWhere}
GROUP BY w.CutRef, w.FabricCombo, w.{colName}, w.MarkerName, w.EstCutDate, ws.SizeRatio";

            DualResult cutRefresult = DBProxy.Current.Select(null, cmdsql, out DataTable cutReftb);
            if (!cutRefresult)
            {
                return cutRefresult;
            }
            #endregion

            #region 找出空的CutRef
            cmdsql = $@"
SELECT * 
FROM {workOrder_tableName} w WITH (NOLOCK) 
OUTER APPLY (
    SELECT STUFF((
        SELECT ',' + b.SizeCode + ':' + CAST(b.Qty AS VARCHAR)
        FROM (
            SELECT SizeCode, Qty 
            FROM {workOrder_tableName}_SizeRatio ws 
            WHERE ws.{tableKey} = w.Ukey AND w.ID = ws.ID
        ) b FOR XML PATH('')
    ), 1, 1, '') AS SizeRatio
) ws
WHERE w.{colName} IS NOT NULL 
        AND (w.CutRef IS NULL OR w.CutRef = '') 
        AND (w.EstCutDate IS NOT NULL AND w.EstCutDate != '')
        {where}
        AND w.id = '{cuttingID}' AND w.mDivisionid = '{mDivision}'
ORDER BY w.FabricCombo, w.{colName}";

            cutRefresult = DBProxy.Current.Select(null, cmdsql, out DataTable workordertmp);
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
                DataRow[] findrow = cutReftb.Select($@"MarkerName = '{dr["MarkerName"]}' AND FabricCombo = '{dr["FabricCombo"]}' AND {colName} = {dr["Cutno"]} AND EstCutDate = '{dr["EstCutDate"]}' AND SizeRatio = '{dr["SizeRatio"]}'");
                string newCutRef = string.Empty;

                if (findrow.Length != 0 && form == CuttingForm.P02)
                {
                    newCutRef = findrow[0]["CutRef"].ToString();
                }
                else
                {
                    string maxref = Sci.Production.PublicPrg.Prgs.GetColumnValueNo($"{workOrder_tableName}", "CutRef");
                    DataRow newdr = cutReftb.NewRow();
                    newdr["MarkerName"] = dr["MarkerName"] ?? string.Empty;
                    newdr["FabricCombo"] = dr["FabricCombo"] ?? string.Empty;
                    if (form == CuttingForm.P02)
                    {
                        newdr["CutPlanID"] = dr["CutPlanID"];
                    }
                    else
                    {
                        newdr["Cutno"] = dr["Cutno"];
                    }

                    newdr["EstCutDate"] = dr["EstCutDate"] ?? string.Empty;
                    newdr["CutRef"] = maxref;
                    newdr["SizeRatio"] = dr["SizeRatio"];
                    cutReftb.Rows.Add(newdr);
                    newCutRef = maxref;
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
            using (TransactionScope transactionscope = new TransactionScope())
            {
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
            }

            if (dtWorkorder.Rows.Count > 0)
            {
                Task.Run(() => new Guozi_AGV().SentWorkOrderToAGV(dtWorkorder));
            }

            return result;
        }
    }
}

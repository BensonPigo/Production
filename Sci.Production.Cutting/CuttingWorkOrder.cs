using Ict;
using Sci.Andy.ExtensionMethods;
using Sci.Data;
using Sci.Win.Tools;
using Sci.Win.UI;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1602 // Enumeration items should be documented
    /// <summary>
    /// P02, P09共用
    /// </summary>
    public partial class CuttingWorkOrder
    {
        public enum DialogAction
        {
            Edit,
            Create,
        }

        /// <summary>
        /// 分配 Distribute
        /// </summary>
        /// <param name="id">Cutting.ID</param>
        /// <param name="listWorkOrderUkey">listWorkOrderUkey</param>
        /// <param name="tableMiddleName">傳入字串 P02: ForPlanning 或 P09:ForOutput </param>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <returns>DualResult</returns>
        public static DualResult InsertWorkOrder_Distribute(string id, List<long> listWorkOrderUkey, string tableMiddleName, SqlConnection sqlConnection)
        {
            string whereWorkOrderUkey = listWorkOrderUkey.Select(s => s.ToString()).JoinToString(",");
            string sqlInsertWorkOrder_Distribute = $@"
select w.Ukey, w.Colorid, w.FabricCombo, ws.SizeCode, [CutQty] = isnull(ws.Qty * w.Layer, 0)
into #tmpCutting
from WorkOrder{tableMiddleName} w with (nolock)
inner join WorkOrder{tableMiddleName}_SizeRatio ws with (nolock) on ws.WorkOrder{tableMiddleName}Ukey = w.Ukey
where w.Ukey in ({whereWorkOrderUkey})
order by ukey

select * from #tmpCutting

select oq.ID, oq.Article, oq.SizeCode, [Qty] = Cast(isnull(oq.Qty, 0) as int), oc.ColorID, oc.PatternPanel, o.SewInLine
from Orders o with (nolock)
inner join Order_Qty oq with (nolock) on oq.ID = o.ID
inner join Order_ColorCombo oc with (nolock) on oc.Id = o.POID and oc.Article = oq.Article
where   o.POID = '{id}'
order by o.SewInLine, oq.ID
";
            DualResult result = DBProxy.Current.SelectByConn(sqlConnection, sqlInsertWorkOrder_Distribute, out DataTable[] dtResults);
            if (!result)
            {
                return result;
            }

            DataTable dtWorkOrderDistribute = dtResults[0];
            DataTable dtOrderQty = dtResults[1];
            string sqlInsertWorkOrderDistribute = string.Empty;

            foreach (DataRow itemDistribute in dtWorkOrderDistribute.Rows)
            {
                if (MyUtility.Convert.GetInt(itemDistribute["CutQty"]) == 0)
                {
                    continue;
                }

                string filterDistribute = $@"
PatternPanel = '{itemDistribute["FabricCombo"]}' and
SizeCode = '{itemDistribute["SizeCode"]}' and
ColorID = '{itemDistribute["Colorid"]}' and
Qty > 0
";
                DataRow[] arrayDistributeOrderQty = dtOrderQty.Select(filterDistribute, "SewInLine,ID");

                foreach (DataRow drDistributeOrderQty in arrayDistributeOrderQty)
                {
                    int distributrQty = 0;
                    if (MyUtility.Convert.GetInt(itemDistribute["CutQty"]) >= MyUtility.Convert.GetInt(drDistributeOrderQty["Qty"]))
                    {
                        distributrQty = MyUtility.Convert.GetInt(drDistributeOrderQty["Qty"]);
                    }
                    else
                    {
                        distributrQty = MyUtility.Convert.GetInt(itemDistribute["CutQty"]);
                    }

                    // 表示workOrder size數已經分配完
                    if (distributrQty == 0)
                    {
                        break;
                    }

                    itemDistribute["CutQty"] = MyUtility.Convert.GetInt(itemDistribute["CutQty"]) - distributrQty;
                    drDistributeOrderQty["Qty"] = MyUtility.Convert.GetInt(drDistributeOrderQty["Qty"]) - distributrQty;

                    sqlInsertWorkOrderDistribute += $@"
insert into WorkOrder{tableMiddleName}_Distribute(WorkOrder{tableMiddleName}Ukey, ID, OrderID, Article, SizeCode, Qty)
values({itemDistribute["Ukey"]}, '{id}', '{drDistributeOrderQty["ID"]}', '{drDistributeOrderQty["Article"]}', '{itemDistribute["SizeCode"]}', '{distributrQty}')
";
                }

                // 如果還有未分配就insert EXCESS
                if (MyUtility.Convert.GetInt(itemDistribute["CutQty"]) > 0)
                {
                    sqlInsertWorkOrderDistribute += $@"
insert into WorkOrder{tableMiddleName}_Distribute(WorkOrder{tableMiddleName}Ukey, ID, OrderID, Article, SizeCode, Qty)
values({itemDistribute["Ukey"]}, '{id}', 'EXCESS', '', '{itemDistribute["SizeCode"]}', '{itemDistribute["CutQty"]}')
";
                }
            }

            if (!MyUtility.Check.Empty(sqlInsertWorkOrderDistribute))
            {
                result = DBProxy.Current.ExecuteByConn(sqlConnection, sqlInsertWorkOrderDistribute);
            }

            return result;
        }

        #region 檢查資料庫 P10(Bundle)

        /// <summary>
        /// 是否已經建立 P10(Bundle)
        /// </summary>
        /// <param name="cutRefs">cutRefs</param>
        /// <returns>存在:True/無:Fasle</returns>
        public static bool HasBundle(IEnumerable<string> cutRefs)
        {
            return GetBundlebyCutRef(cutRefs).AsEnumerable().Any();
        }

        /// <inheritdoc/>
        public static bool HasBundle(string cutRef)
        {
            return GetBundlebyCutRef(cutRef).AsEnumerable().Any();
        }

        /// <summary>
        /// 取得已經建立的 P10(Bundle) 資訊
        /// </summary>
        /// <param name="cutRefs">CutRef</param>
        /// <returns>DataTable</returns>
        public static DataTable GetBundlebyCutRef(IEnumerable<string> cutRefs)
        {
            string stringCutref = "'" + string.Join("','", cutRefs) + "'";
            return GetBundlebyCutRefInternal(stringCutref);
        }

        /// <inheritdoc/>
        public static DataTable GetBundlebyCutRef(string cutRef)
        {
            string stringCutref = $"'{cutRef}'";
            return GetBundlebyCutRefInternal(stringCutref);
        }

        private static DataTable GetBundlebyCutRefInternal(string stringCutref)
        {
            string sqlcmd = $@"
SELECT
     [Cutting_P10 ID] = Bundle.ID
    ,[Cut Ref#] = Bundle.CutRef
    ,[Create By] = Pass1.Name
    ,[Create Date] = Format(Bundle.AddDate, 'yyyy/MM/dd HH:mm:ss')
FROM  Bundle WITH(NOLOCK)
INNER JOIN Pass1 WITH(NOLOCK) ON Bundle.AddName = Pass1.ID
WHERE Bundle.CutRef IN ({stringCutref})
AND Bundle.CutRef <> ''
ORDER BY Bundle.ID, Bundle.CutRef, Pass1.Name
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtCheck);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dtCheck;
        }

        /// <summary>
        /// 顯示提示資訊,並 return 檢查是否通過
        /// </summary>
        /// <param name="cutRefs">cutRefs</param>
        /// <param name="msg">提示訊息</param>
        /// <returns>通過:True/不通過:False</returns>
        public static bool CheckBundleAndShowData(IEnumerable<string> cutRefs, string msg)
        {
            DataTable dtCheck = GetBundlebyCutRef(cutRefs);
            if (dtCheck.Rows.Count > 0)
            {
                new MsgGridForm(dtCheck, msg, "Exists bundle data").ShowDialog();
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool CheckBundleAndShowData(string cutRef, string msg)
        {
            DataTable dtCheck = GetBundlebyCutRef(cutRef);
            if (dtCheck.Rows.Count > 0)
            {
                new MsgGridForm(dtCheck, msg, "Exists bundle data").ShowDialog();
                return false;
            }

            return true;
        }
        #endregion

        #region 檢查資料庫 P20(CuttingOutput)

        /// <summary>
        /// 是否已經建立 P20(CuttingOutput) 資訊
        /// </summary>
        /// <param name="cutRefs">cutRefs</param>
        /// <returns>存在:True/無:Fasle</returns>
        public static bool HasCuttingOutput(IEnumerable<string> cutRefs)
        {
            return GetCuttingOutputbyCutRef(cutRefs).AsEnumerable().Any();
        }

        /// <inheritdoc/>
        public static bool HasCuttingOutput(string cutRef)
        {
            return GetCuttingOutputbyCutRef(cutRef).AsEnumerable().Any();
        }

        /// <summary>
        /// 取得已經建立的 P10(CuttingOutput) 資訊
        /// </summary>
        /// <param name="cutRefs">cutRefs</param>
        /// <returns>DataTable</returns>
        public static DataTable GetCuttingOutputbyCutRef(IEnumerable<string> cutRefs)
        {
            // 將列表轉換為單個字符串，用於SQL查詢
            string stringCutref = "'" + string.Join("','", cutRefs) + "'";
            return GetCuttingOutputbyCutRefInternal(stringCutref);
        }

        /// <inheritdoc/>
        public static DataTable GetCuttingOutputbyCutRef(string cutRef)
        {
            // 單個字符串直接用於SQL查詢
            string stringCutref = $"'{cutRef}'";
            return GetCuttingOutputbyCutRefInternal(stringCutref);
        }

        private static DataTable GetCuttingOutputbyCutRefInternal(string stringCutref)
        {
            string sqlcmd = $@"
SELECT
    [Cutting_P20 ID] = CuttingOutput.ID
    ,[Cut Ref#] = CuttingOutput_Detail.CutRef
    ,[Create By] = Pass1.Name
    ,[Create Date] = Format(CuttingOutput.AddDate, 'yyyy/MM/dd HH:mm:ss')
FROM CuttingOutput_Detail WITH(NOLOCK)
INNER JOIN CuttingOutput WITH(NOLOCK) ON CuttingOutput.ID = CuttingOutput_Detail.ID
INNER JOIN Pass1 WITH(NOLOCK) ON CuttingOutput.AddName = Pass1.ID
WHERE CuttingOutput_Detail.CutRef IN ({stringCutref})
ORDER BY CuttingOutput.ID, CuttingOutput_Detail.CutRef, Pass1.Name
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtCheck);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dtCheck;
        }

        /// <summary>
        /// 顯示提示資訊,並 return 檢查是否通過
        /// </summary>
        /// <param name="cutRefs">cutRefs</param>
        /// <returns>通過:True/不通過:Fasle</returns>
        /// <inheritdoc/>
        public static bool CheckCuttingOutputAndShowData(IEnumerable<string> cutRefs, string msg)
        {
            DataTable dtCheck = GetCuttingOutputbyCutRef(cutRefs);
            if (dtCheck.Rows.Count > 0)
            {
                new MsgGridForm(dtCheck, msg, "Exists cutting output data").ShowDialog();
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool CheckCuttingOutputCuttingOutputAndShowData(string cutRef, string msg)
        {
            DataTable dtCheck = GetCuttingOutputbyCutRef(cutRef);
            if (dtCheck.Rows.Count > 0)
            {
                new MsgGridForm(dtCheck, msg, "Exists cutting output data").ShowDialog();
                return false;
            }

            return true;
        }
        #endregion

        #region 檢查資料庫 P05(MarkerReq_Detail)

        /// <inheritdoc/>
        public static bool HasMarkerReq(string id)
        {
            return GetMarkerReqbyID(id).AsEnumerable().Any();
        }

        private static DataTable GetMarkerReqbyID(string id)
        {
            string sqlcmd = $@"
SELECT
    [Cutting_P05 ID] = MarkerReq.ID
   ,[Size Ratio] = MarkerReq_Detail.SizeRatio
   ,[Fabric Combo] = MarkerReq_Detail.FabricCombo
   ,[Marker Name] = MarkerReq_Detail.MarkerName
   ,[Layer] = MarkerReq_Detail.Layer
   ,[Request Qty] = MarkerReq_Detail.ReqQty
   ,[Release Qty] = MarkerReq_Detail.ReleaseQty
   ,[Create By] = Pass1.Name
   ,[Create Date] = Format(MarkerReq.AddDate, 'yyyy/MM/dd HH:mm:ss')
FROM MarkerReq_Detail WITH(NOLOCK)
INNER JOIN MarkerReq WITH(NOLOCK) ON MarkerReq.ID = MarkerReq_Detail.ID
INNER JOIN Pass1 WITH(NOLOCK) ON MarkerReq.AddName = Pass1.ID
WHERE MarkerReq_Detail.OrderID = '{id}'
ORDER BY MarkerReq.ID, MarkerReq_Detail.SizeRatio, Pass1.Name
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtCheck);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dtCheck;
        }

        /// <inheritdoc/>
        public static bool CheckMarkerReqAndShowData(string id, string msg)
        {
            DataTable dtCheck = GetMarkerReqbyID(id);
            if (dtCheck.Rows.Count > 0)
            {
                new MsgGridForm(dtCheck, msg, "Exists marker request data").ShowDialog();
                return false;
            }

            return true;
        }
        #endregion

        #region SpreadingNo 開窗/驗證

        /// <inheritdoc/>
        public static SelectItem PopupSpreadingNo(string defaults)
        {
            DataTable dt = GetSpreadingNo();
            SelectItem selectItem = new SelectItem(dt, "SpreadingNoID,CutCellID", "10@400,300", defaults, false, ",", headercaptions: "Spreading No,Cut Cell");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            return selectItem;
        }

        /// <inheritdoc/>
        public static bool ValidatingSpreadingNo(string newvalue, out DataRow dr)
        {
            dr = null;
            if (string.IsNullOrEmpty(newvalue))
            {
                return true;
            }

            DataRow[] row = GetSpreadingNo().Select($"SpreadingNoID = '{newvalue}'");
            if (row.Length == 0)
            {
                MyUtility.Msg.WarningBox($"<Spreading No> : {newvalue} data not found!");
                return false;
            }

            dr = row[0];
            return true;
        }

        /// <inheritdoc/>
        public static DataTable GetSpreadingNo()
        {
            string sqlcmd = $@"
SELECT
    SpreadingNoID = ID
   ,CutCellID
FROM SpreadingNo WITH (NOLOCK)
WHERE MDivisionID = '{Sci.Env.User.Keyword}'
AND Junk = 0
";

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }
        #endregion

        #region CutCell 開窗/驗證

        /// <inheritdoc/>
        public static SelectItem PopupCutCell(string defaults)
        {
            DataTable dt = GetCutCell();
            SelectItem selectItem = new SelectItem(dt, "ID", "10@300,300", defaults, false, ",");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            return selectItem;
        }

        /// <inheritdoc/>
        public static bool ValidatingCutCell(string newvalue)
        {
            if (string.IsNullOrEmpty(newvalue))
            {
                return true;
            }

            if (GetCutCell().Select($"ID = '{newvalue}'").Length == 0)
            {
                MyUtility.Msg.WarningBox($"<Cut Cell> : {newvalue} data not found!");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static DataTable GetCutCell()
        {
            string sqlcmd = $@"
SELECT
    ID
FROM CutCell WITH (NOLOCK)
WHERE MDivisionID = '{Sci.Env.User.Keyword}'
AND Junk = 0
";

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }
        #endregion

        #region Masked Text

        /// <summary>
        /// 處理 MarkerLength
        /// </summary>
        /// <inheritdoc/>
        public static string FormatMarkerLength(string input)
        {
            var match = Regex.Match(input, @"(\d+)Y(\d+)-(\d+)/(\d+)\+(\d+)\""");

            if (match.Success)
            {
                string part1 = match.Groups[1].Value.PadLeft(2, '0');
                string part2 = match.Groups[2].Value.PadLeft(2, '0');
                string part3 = match.Groups[3].Value.PadLeft(1, '0');
                string part4 = match.Groups[4].Value.PadLeft(1, '0');
                string part5 = match.Groups[5].Value.PadLeft(1, '0');

                // 返回格式化的字符串
                return $"{part1}Y{part2}-{part3}/{part4}+{part5}\"";
            }

            // 如果不匹配，返回原始输入或根据需求处理
            return input;
        }

        /// <summary>
        /// MarkerLength 欄位驗證
        /// </summary>
        /// <inheritdoc/>
        public static string SetMarkerLengthMaskString(string eventString)
        {
            eventString = eventString.Replace(" ", "0");
            if (eventString.Contains("Y"))
            {
                string[] strings = eventString.Split("Y");
                string[] strings2 = strings[1].Split("-");
                string[] strings3 = strings2[1].Split("/");
                string[] strings4 = strings3[1].Split("\"");
                eventString = $"{strings[0].PadLeft(2, '0')}Y{strings2[0].PadLeft(2, '0')}-{strings3[0].PadLeft(1, '0')}/{strings4[0].PadLeft(1, '0')}+{strings4[1].PadLeft(1, '0')}\"";
            }
            else
            {
                eventString = eventString.PadRight(8, '0');
                eventString = $"{eventString.Substring(0, 2)}Y{eventString.Substring(2, 2)}-{eventString.Substring(4, 1)}/{eventString.Substring(5, 1)}+{eventString.Substring(6, 1)}\"";
            }

            return eventString;
        }

        /// <summary>
        /// 處理 ActCuttingPerimeter, StraightLength, CurvedLength
        /// </summary>
        /// <inheritdoc/>
        public static string FormatData(string input)
        {
            // 正則表達式匹配格式 "數字Yd數字"數字"
            var match = Regex.Match(input, @"(\d+)Yd(\d+)\""(\d+)");
            if (match.Success)
            {
                // 提取數字部分
                string part1 = match.Groups[1].Value.PadLeft(3, '0');
                string part2 = match.Groups[2].Value.PadLeft(2, '0');
                string part3 = match.Groups[3].Value.PadLeft(2, '0');

                // 返回格式化的字串
                return $"{part1}Yd{part2}\"{part3}";
            }

            // 如果不匹配，返回原始輸入或根據需求處理
            return input;
        }

        /// <summary>
        /// ActCuttingPerimeter, StraightLength, CurvedLength 欄位驗證
        /// </summary>
        /// <inheritdoc/>
        public static string SetMaskString(string eventString)
        {
            eventString = eventString.Replace(" ", "0");
            if (eventString.Contains("Yd"))
            {
                string[] strings = eventString.Split("Yd");
                string[] strings2 = strings[1].Split("\"");
                eventString = $"{strings[0].PadLeft(3, '0')}Yd{strings2[0].PadLeft(2, '0')}\"{strings2[1].PadLeft(2, '0')}";
            }
            else
            {
                eventString = eventString.PadRight(7, '0');
                eventString = $"{eventString.Substring(0, 3)}Yd{eventString.Substring(3, 2)}\"{eventString.Substring(5, 2)}";
            }

            return eventString;
        }
        #endregion
    }
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore SA1602 // Enumeration items should be documented
}

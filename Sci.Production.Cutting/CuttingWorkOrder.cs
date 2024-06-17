using Ict;
using Microsoft.SqlServer.Management.Smo;
using Sci.Andy.ExtensionMethods;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using Sci.Win.UI;
using System;
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
        public enum CuttingForm
        {
            P02,
            P09,
        }

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

        #region Seq1,Seq2 開窗/驗證

        public static SelectItem PopupSEQ(string id, string seq1, string colorID)
        {
            DataTable dt = GetSEQ(id);
            SelectItem selectItem = new SelectItem(dt, "Seq1,Seq2,Refno,ColorID", "3,2,20,3@500,300", seq1, false, ",", headercaptions: "Seq1,Seq2,Refno,Color");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            string newColor = MyUtility.Convert.GetString(selectItem.GetSelecteds()[0]["ColorID"]);
            if (!CheckColorSame(colorID, newColor))
            {
                return null;
            }

            return selectItem;
        }

        /// <summary>
        /// 驗證Seq1,Seq2
        /// </summary>
        /// <param name="newvalue">傳入Seq1或Seq2的值</param>
        /// <param name="seqCloumn">傳入"Seq1"或"Seq2"</param>
        /// <inheritdoc/>
        public static bool ValidatingSEQ(string newvalue, string seqCloumn, DataRow detaildr, out DataRow outdr)
        {
            outdr = null;

            return true;
        }

        private static bool CheckColorSame(string oriColor, string newColor)
        {
            if (!oriColor.IsNullOrWhiteSpace() && oriColor != newColor)
            {
                DialogResult diaR = MyUtility.Msg.QuestionBox($@"Original assign colorID is {oriColor}, but you locate colorID is {newColor} now , Do you want to continue? ");
                if (diaR == DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        public static DataTable GetSEQ(string id)
        {
            string sqlcmd = $@"
SELECT
    psd.SEQ1
   ,psd.SEQ2
   ,psd.Refno
   ,ColorID = ISNULL(psdc.SpecValue, '')
   ,psd.SCIRefno
FROM PO_Supp_Detail psd WITH (NOLOCK)
LEFT JOIN PO_Supp_Detail_Spec psdc WITH (NOLOCK) ON psdc.ID = psd.id AND psdc.seq1 = psd.seq1 AND psdc.seq2 = psd.seq2 AND psdc.SpecColumnID = 'Color'
WHERE psd.ID = '{id}'
AND psd.Junk = 0
AND EXISTS (SELECT 1 FROM Order_BOF WITH (NOLOCK) WHERE ID = psd.ID AND SCIRefno = psd.SCIRefno)
";

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }

        public static DataTable GetPatternPanel(string id, string fabricPanelCode)
        {
            string sqlcmd = $@"
SELECT PatternPanel
FROM Order_FabricCode WITH (NOLOCK)
WHERE ID = '{id}'
AND FabricPanelCode = '{fabricPanelCode}'
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

        #region SpreadingNo 開窗/驗證

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

        public static bool ValidatingSpreadingNo(string newvalue, out DataRow outdr)
        {
            outdr = null;
            if (string.IsNullOrEmpty(newvalue))
            {
                return true;
            }

            try
            {
                DataRow[] row = GetSpreadingNo().Select($"SpreadingNoID = '{newvalue}'");
                if (row.Length == 0)
                {
                    MyUtility.Msg.WarningBox($"<Spreading No> : {newvalue} data not found!");
                    return false;
                }

                outdr = row[0];
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }

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

        public static bool ValidatingCutCell(string newvalue)
        {
            if (string.IsNullOrEmpty(newvalue))
            {
                return true;
            }

            try
            {
                if (GetCutCell().Select($"ID = '{newvalue}'").Length == 0)
                {
                    MyUtility.Msg.WarningBox($"<Cut Cell> : {newvalue} data not found!");
                    return false;
                }
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }

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

        #region Pattern No.(MarkerNo) 開窗/驗證

        public static SelectItem PopupMarkerNo(string id, string defaults)
        {
            DataTable dt = GetMarkerNo(id);
            SelectItem selectItem = new SelectItem(dt, "MarkerNo", "20@350,400", defaults, false, ",", "Pattern No.");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            return selectItem;
        }

        public static bool ValidatingMarkerNo(string id, string newvalue)
        {
            if (string.IsNullOrEmpty(newvalue))
            {
                return true;
            }

            try
            {
                if (GetMarkerNo(id).Select($"MarkerNo = '{newvalue}'").Length == 0)
                {
                    MyUtility.Msg.WarningBox($"<Pattern No.> : {newvalue} data not found!");
                    return false;
                }
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }

        public static DataTable GetMarkerNo(string id)
        {
            string sqlcmd = $@"
SELECT DISTINCT oec.MarkerNo
FROM Order_EachCons oec WITH(NOLOCK)
INNER JOIN Orders o WITH(NOLOCK) ON o.ID = oec.ID
WHERE o.POID = '{id}'
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

        #region SizeRatio SizeCode 開窗/驗證
        public static SelectItem PopupSizeCode(string id, string defaults)
        {
            DataTable dt = GetSizeCode(id);
            SelectItem selectItem = new SelectItem(dt, "SizeCode", "20@350,400", defaults, false, ",", "Size");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            return selectItem;
        }

        public static DataTable GetSizeCode(string id)
        {
            string sqlcmd = $@"
SELECT DISTINCT
    oq.SizeCode
FROM Order_Qty oq WITH (NOLOCK)
INNER JOIN Orders o WITH (NOLOCK) ON o.id = oq.id
WHERE o.CuttingSP = '{id}'
ORDER BY SizeCode
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }

        public static bool SizeCodeCellEditingMouseDown(
            Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e,
            Sci.Win.UI.Grid gridSizeRatio,
            DataRow currentDetailData,
            DataTable dtDistribute,
            CuttingForm form)
        {
            if (e.Button != MouseButtons.Right || gridSizeRatio.IsEditingReadOnly)
            {
                return false;
            }

            DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
            string oldvalue = MyUtility.Convert.GetString(dr["SizeCode"]);

            SelectItem selectItem = PopupSizeCode(currentDetailData["ID"].ToString(), oldvalue);
            if (selectItem == null)
            {
                return false;
            }

            dr["SizeCode"] = selectItem.GetSelectedString();
            dr.EndEdit();

            UpdateDistribute_Size(currentDetailData, dtDistribute, oldvalue, dr["SizeCode"].ToString(), form);
            return true;
        }

        /// <summary>
        /// return true → 要更新主表資訊
        /// </summary>
        /// <inheritdoc/>
        public static bool SizeCodeCellValidating(
            Ict.Win.UI.DataGridViewCellValidatingEventArgs e,
            Sci.Win.UI.Grid gridSizeRatio,
            DataRow currentDetailData,
            DataTable dtDistribute,
            CuttingForm form)
        {
            if (gridSizeRatio.IsEditingReadOnly)
            {
                return false;
            }

            DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
            string newvalue = e.FormattedValue.ToString();
            string oldvalue = MyUtility.Convert.GetString(dr["SizeCode"]);
            if (newvalue == oldvalue)
            {
                return false;
            }

            // 清除不跳訊息,但要更新後續其它表資訊  存檔才移除 SizeRatio.SizeCode = ''
            if (newvalue != string.Empty && GetSizeCode(currentDetailData["ID"].ToString()).Select($"SizeCode = '{newvalue}'").Length == 0)
            {
                MyUtility.Msg.WarningBox($"<Size> : {newvalue} data not found!");
                dr["SizeCode"] = string.Empty;
                e.Cancel = true;
                return false;
            }

            dr["SizeCode"] = newvalue;
            dr.EndEdit();

            UpdateDistribute_Size(currentDetailData, dtDistribute, oldvalue, newvalue, form);

            // 手輸入可以清空,所以要重算 Excess
            UpdateExcess(currentDetailData, (DataTable)gridSizeRatio.DataSource, dtDistribute, form);
            return true;
        }
        #endregion

        #region SizeRatio Qty 驗證

        /// <summary>
        /// return true → 要更新主表資訊
        /// </summary>
        /// <inheritdoc/>
        public static bool SizeRatioQtyCellValidating(
            Ict.Win.UI.DataGridViewCellValidatingEventArgs e,
            Sci.Win.UI.Grid gridSizeRatio,
            DataRow currentDetailData,
            DataTable dtDistribute,
            CuttingForm form)
        {
            DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
            int oldvalue = MyUtility.Convert.GetInt(dr["Qty"]);
            int newvalue = MyUtility.Convert.GetInt(e.FormattedValue);
            if (oldvalue == newvalue)
            {
                return false;
            }

            dr["Qty"] = newvalue;
            dr.EndEdit();

            UpdateExcess(currentDetailData, (DataTable)gridSizeRatio.DataSource, dtDistribute, form);

            return true;
        }
        #endregion

        #region Distribute OrderID 開窗/驗證

        public static DataTable GetDistributeOrderID(string id)
        {
            string sqlcmd = $@"
SELECT DISTINCT
    oq.ID
FROM Order_Qty oq WITH (NOLOCK)
INNER JOIN Orders o WITH (NOLOCK) ON o.id = oq.id
WHERE o.CuttingSP = '{id}'
ORDER BY ID
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }

        public static void Distribute3CellEditingMouseDown(Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e, string id, DataTable gridSizeRatio, Sci.Win.UI.Grid gridDistributeToSP)
        {
            DataRow dr = gridDistributeToSP.GetDataRow(e.RowIndex);
            if (e.Button != MouseButtons.Right ||
                gridDistributeToSP.IsEditingReadOnly ||
                dr["OrderID"].ToString().Equals("EXCESS", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (gridSizeRatio.DefaultView.ToTable().Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please insert size ratio data first!");
                return;
            }

            DataTable dt = FilterOrder_Qty_By_SizeRatio(id, MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), gridSizeRatio);

            string columnName = gridDistributeToSP.Columns[e.ColumnIndex].Name;
            SelectItem selectItem = new SelectItem(dt, "ID,Article,SizeCode", "20,15,10", MyUtility.Convert.GetString(dr[columnName]), false, ",", "SP#,Article,Size");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            dr["OrderID"] = selectItem.GetSelecteds()[0]["OrderID"];
            dr["Article"] = selectItem.GetSelecteds()[0]["Article"];
            dr["SizeCode"] = selectItem.GetSelecteds()[0]["SizeCode"];
            dr.EndEdit();
            return;
        }

        /// <summary>
        /// return true → 要更新主表資訊
        /// </summary>
        /// <inheritdoc/>
        public static bool Distribute3CellValidating(Ict.Win.UI.DataGridViewCellValidatingEventArgs e, string id, DataTable gridSizeRatio, Sci.Win.UI.Grid gridDistributeToSP)
        {
            DataRow dr = gridDistributeToSP.GetDataRow(e.RowIndex);
            string columnName = gridDistributeToSP.Columns[e.ColumnIndex].Name;
            string newvalue = e.FormattedValue.ToString();
            string oldvalue = dr[columnName].ToString();

            if (gridDistributeToSP.IsEditingReadOnly ||
                dr["OrderID"].ToString().Equals("EXCESS", StringComparison.OrdinalIgnoreCase) ||
                oldvalue == newvalue)
            {
                return false;
            }

            if (gridSizeRatio.DefaultView.ToTable().Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please insert size ratio data first!");
                return false;
            }

            if (newvalue == string.Empty)
            {
                dr[columnName] = string.Empty;
                dr.EndEdit();
                return true;
            }

            DataTable dt = FilterOrder_Qty_By_SizeRatio(id, MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), gridSizeRatio);
            if (dt.Rows.Count == 0)
            {
                dr[columnName] = string.Empty;
                dr.EndEdit();
                MyUtility.Msg.WarningBox($"<{columnName}> : {newvalue} data not found!");
                e.Cancel = true;
                return false;
            }

            // 檢查 Article, SizeCode
            if (!MyUtility.Check.Empty(dr["SizeCode"]) && !MyUtility.Check.Empty(dr["Article"]))
            {
                if (GetOrder_Qty_byCuttingSP(id).Select($"ID = '{newvalue}' AND Article ='{dr["Article"]}' AND SizeCode = '{dr["SizeCode"]}'").Length == 0)
                {
                    dr["OrderID"] = string.Empty;
                    dr.EndEdit();
                    MyUtility.Msg.WarningBox($"<SP#>:{dr["OrderID"]},<Article>:{dr["Article"]},<Size>:{dr["SizeCode"]} not exists qty break down");
                    e.Cancel = true;
                    return false;
                }
            }

            dr["OrderID"] = newvalue;
            dr.EndEdit();
            return true;
        }

        public static DataTable FilterOrder_Qty_By_SizeRatio(string id, string orderID, string article, string sizeCode, DataTable gridSizeRatio)
        {
            DataTable dt = FilterOrder_Qty(id, orderID, article, sizeCode);

            // SizeCode 需要存在 Size Ratio
            string sizeCodes = gridSizeRatio.DefaultView.ToTable().AsEnumerable().Select(row => "'" + MyUtility.Convert.GetString(row["SizeCode"]) + "'").Distinct().ToList().JoinToString(",");
            return dt.Select($"SizeCode IN ({sizeCodes})").TryCopyToDataTable(dt);
        }

        public static DataTable FilterOrder_Qty(string id, string orderID, string article, string sizeCode)
        {
            string filter = "1=1";
            if (!orderID.IsNullOrWhiteSpace())
            {
                filter += $" AND ID = '{orderID}'";
            }

            if (!article.IsNullOrWhiteSpace())
            {
                filter += $" AND Article = '{article}'";
            }

            if (!sizeCode.IsNullOrWhiteSpace())
            {
                filter += $" AND SizeCode = '{sizeCode}'";
            }

            DataTable dt = GetOrder_Qty_byCuttingSP(id);
            return dt.Select(filter).TryCopyToDataTable(dt);
        }

        #endregion

        #region Other Function
        public static DataTable GetOrder_Qty_byCuttingSP(string id)
        {
            string sqlcmd = $@"
SELECT
    oq.*
FROM Order_Qty oq WITH (NOLOCK)
INNER JOIN Orders o WITH (NOLOCK) ON o.id = oq.id
WHERE o.CuttingSP = '{id}'
ORDER BY oq.ID,oq.Article,oq.SizeCode
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 更新 CurrentDetailData 的欄位: SizeCode_CONCAT, TotalCutQty_CONCAT, 因 SizeRatio 的欄位: SizeCode,Qty 調整
        /// </summary>
        /// <inheritdoc/>
        public static void UpdateConcatString(DataRow currentDetailData, DataTable dtSizeRatio, CuttingForm form)
        {
            string filter = GetFilter(currentDetailData, form);
            DataRow[] dtSizeRatioFilter = dtSizeRatio.Select(filter);
            currentDetailData["SizeCode_CONCAT"] = GetSizeCode_CONCAT(dtSizeRatioFilter);
            currentDetailData["TotalCutQty_CONCAT"] = GetTotalCutQty_CONCAT(MyUtility.Convert.GetInt(currentDetailData["Layer"]), dtSizeRatioFilter);
        }

        public static string GetSizeCode_CONCAT(DataRow[] rows)
        {
            return rows.AsEnumerable().Select(row => row["SizeCode"].ToString() + "/ " + row["Qty"].ToString()).JoinToString(",");
        }

        public static string GetTotalCutQty_CONCAT(int layer, DataRow[] rows)
        {
            return rows.AsEnumerable().Select(row => row["SizeCode"].ToString() + "/ " + MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(row["Qty"]) * layer)).JoinToString(",");
        }

        /// <summary>
        /// 更新 CurrentDetailData 的欄位: TotalDistributeQty, 因 Distribute 資訊變動
        /// </summary>
        /// <inheritdoc/>
        public static void UpdateTotalDistributeQty(DataRow currentDetailData, DataTable dtDistribute, CuttingForm form)
        {
            string filter = GetFilter(currentDetailData, form) + " AND (OrderID = 'EXCESS' OR (OrderID <> '' AND Article <> '' AND SizeCode <>''))";
            currentDetailData["TotalDistributeQty"] = dtDistribute.Select(filter).Sum(row => MyUtility.Convert.GetInt(row["Qty"]));
        }

        /// <summary>
        /// 更新 DataTable Distribute 的欄位: SizeCode, 因 SizeRatio 的欄位: SizeCode 調整
        /// </summary>
        /// <inheritdoc/>
        public static void UpdateDistribute_Size(DataRow currentDetailData, DataTable dtDistribute, string oldvalue, string newvalue, CuttingForm form)
        {
            if (newvalue == string.Empty)
            {
                dtDistribute.Select(GetFilter(currentDetailData, form) + $" AND SizeCode = '{oldvalue}'").Delete();
            }
            else
            {
                foreach (DataRow row in dtDistribute.Select(GetFilter(currentDetailData, form) + $" AND SizeCode = '{oldvalue}'"))
                {
                    row["SizeCode"] = newvalue;
                }
            }
        }

        /// <summary>
        /// 更新 DataTable Distribute 剩餘數:Excess, 因 SizeRatio 的欄位: Qty 調整
        /// </summary>
        /// <inheritdoc/>
        public static void UpdateExcess(DataRow currentDetailData, DataTable dtSizeRatio, DataTable dtDistribute, CuttingForm form)
        {
            string filter = GetFilter(currentDetailData, form);
            foreach (DataRow dr in dtSizeRatio.Select(filter))
            {
                int ttlQty_SizeCode = MyUtility.Convert.GetInt(dr["Qty"]) * MyUtility.Convert.GetInt(dr["Layer"]); // 此 SizeCode 總數量
                string sizeCode = dr["SizeCode"].ToString();
                string filterSizeCode = $"{filter} AND SizeCode = '{sizeCode}'";

                int totalDistributedQty = dtDistribute.Select($"{filterSizeCode} AND OrderID != 'EXCESS'").AsEnumerable().Sum(row => MyUtility.Convert.GetInt(row["Qty"]));

                // 重算剩餘數,不能小於0
                int excess = Math.Max(ttlQty_SizeCode - totalDistributedQty, 0);

                DataRow[] distributeExcessRows = dtDistribute.Select($"{filterSizeCode} AND OrderID = 'EXCESS'");

                if (distributeExcessRows.Length > 0)
                {
                    distributeExcessRows[0]["Qty"] = excess;
                }
                else
                {
                    DataRow newExcessRow = dtDistribute.NewRow();
                    newExcessRow["WorkOrderUKey"] = currentDetailData["Ukey"];
                    newExcessRow["tmpUkey"] = currentDetailData["tmpUkey"];
                    newExcessRow["OrderID"] = "EXCESS";
                    newExcessRow["SizeCode"] = sizeCode;
                    newExcessRow["Qty"] = excess;
                    dtDistribute.Rows.Add(newExcessRow);
                }
            }
        }

        /// <summary>
        /// P02/P09 第3層用的 Filter. 子表:SizeRatio,Distribute,PatternPanel
        /// </summary>
        /// <param name="currentDetailData">主表 WorkOrder____ </param>
        /// <returns> filter字串 </returns>
        /// <inheritdoc/>
        public static string GetFilter(DataRow currentDetailData, CuttingForm form)
        {
            switch (form)
            {
                case CuttingForm.P02:
                    return $@"WorkOrderForPlanningUkey = {currentDetailData["Ukey"]} AND tmpKey = {currentDetailData["tmpKey"]}";
                case CuttingForm.P09:
                    return $@"WorkOrderForOutputUkey = {currentDetailData["Ukey"]} AND tmpKey = {currentDetailData["tmpKey"]}";
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore SA1602 // Enumeration items should be documented
}

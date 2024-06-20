using Ict;
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
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Ict.Win.UI.DataGridView;

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
        /// <param name="form">CuttingForm.P02/P09 </param>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <returns>DualResult</returns>
        public static DualResult InsertWorkOrder_Distribute(string id, List<long> listWorkOrderUkey, SqlConnection sqlConnection)
        {
            string whereWorkOrderUkey = listWorkOrderUkey.Select(s => s.ToString()).JoinToString(",");
            string sqlInsertWorkOrder_Distribute = $@"
select w.Ukey, w.Colorid, w.FabricCombo, ws.SizeCode, [CutQty] = isnull(ws.Qty * w.Layer, 0)
into #tmpCutting
from WorkOrderForOutput w with (nolock)
inner join WorkOrderForOutput_SizeRatio ws with (nolock) on ws.WorkOrderForOutputUkey = w.Ukey
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
insert into WorkOrderForOutput_Distribute(WorkOrderForOutputUkey, ID, OrderID, Article, SizeCode, Qty)
values({itemDistribute["Ukey"]}, '{id}', '{drDistributeOrderQty["ID"]}', '{drDistributeOrderQty["Article"]}', '{itemDistribute["SizeCode"]}', '{distributrQty}')
";
                }

                // 如果還有未分配就insert EXCESS
                if (MyUtility.Convert.GetInt(itemDistribute["CutQty"]) > 0)
                {
                    sqlInsertWorkOrderDistribute += $@"
insert into WorkOrderForOutput_Distribute(WorkOrderForOutputUkey, ID, OrderID, Article, SizeCode, Qty)
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

        public static bool HasMarkerReq(string cutRef)
        {
            return GetMarkerReqbyCutRef(cutRef).AsEnumerable().Any();
        }

        public static DataTable GetMarkerReqbyCutRef(IEnumerable<string> cutRefs)
        {
            // 將列表轉換為單個字符串，用於SQL查詢
            string stringCutref = "'" + string.Join("','", cutRefs) + "'";
            return GetMarkerReqbyID(stringCutref);
        }

        public static DataTable GetMarkerReqbyCutRef(string cutRef)
        {
            // 單個字符串直接用於SQL查詢
            string stringCutref = $"'{cutRef}'";
            return GetMarkerReqbyID(stringCutref);
        }

        private static DataTable GetMarkerReqbyID(string stringCutref)
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
WHERE MarkerReq_Detail.CutRef IN ({stringCutref})
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

        /// <summary>
        /// 顯示提示資訊,並 return 檢查是否通過
        /// </summary>
        /// <param name="cutRefs">cutRefs</param>
        /// <returns>通過:True/不通過:Fasle</returns>
        /// <inheritdoc/>
        public static bool CheckMarkerReqAndShowData(IEnumerable<string> cutRefs, string msg)
        {
            DataTable dtCheck = GetMarkerReqbyCutRef(cutRefs);
            if (dtCheck.Rows.Count > 0)
            {
                new MsgGridForm(dtCheck, msg, "Exists marker request data").ShowDialog();
                return false;
            }

            return true;
        }

        public static bool CheckMarkerReqAndShowData(string cutRef, string msg)
        {
            DataTable dtCheck = GetMarkerReqbyCutRef(cutRef);
            if (dtCheck.Rows.Count > 0)
            {
                new MsgGridForm(dtCheck, msg, "Exists marker request data").ShowDialog();
                return false;
            }

            return true;
        }
        #endregion

        #region 檢查當前這筆 SpreadingStatus

        public static bool CheckSpreadingStatus(DataRow currentDetailData, string msg)
        {
            if (currentDetailData["SpreadingStatus"].ToString() != "Ready")
            {
                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            return true;
        }

        #endregion

        #region Save Before 檢查

        /// <summary>
        /// 檢查 主表身 不可空欄位, 並移動到 detailgrid 那列
        /// </summary>
        /// <param name="detailDatas">排除已刪除的表身</param>
        /// <param name="detailgrid">detailgrid</param>
        /// <returns>bool</returns>
        public static bool ValidateDetailDatasEmpty(IList<DataRow> detailDatas, Sci.Win.UI.Grid detailgrid)
        {
            var validationRules = new Dictionary<string, string>
            {
                { "MarkerNo", "Marker No cannot be empty." },
                { "FabricPanelCode", "Fabric Panel Code cannot be empty." },
                { "MarkerName", "Marker Name cannot be empty." },
                { "Layer", "Layer cannot be empty." },
                { "SEQ1", "SEQ1 cannot be empty." },
                { "SEQ2", "SEQ2 cannot be empty." },
            };

            foreach (var rule in validationRules)
            {
                foreach (DataRow row in detailDatas.Where(row => MyUtility.Check.Empty(row[rule.Key])))
                {
                    int index = detailDatas.IndexOf(row);
                    detailgrid.SelectRowTo(index);
                    MyUtility.Msg.WarningBox(rule.Value);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 檢查 沒 CutRef,有 CutNo 清單
        /// 相同 (CutNo,FabricCombo) 的 (MarkerName或MarkerNo) 必須相同
        /// 並移動到 detailgrid 那列
        /// </summary>
        /// <param name="detailDatas">排除已刪除的表身</param>
        /// <param name="detailgrid">detailgrid</param>
        /// <returns>bool</returns>
        public static bool ValidateCutNoAndFabricCombo(IList<DataRow> detailDatas, Sci.Win.UI.Grid detailgrid)
        {
            var groupedRows = detailDatas
                .Where(row => MyUtility.Check.Empty(row["CutRef"]) && !MyUtility.Check.Empty(row["CutNo"]))
                .GroupBy(row => new { CutNo = row["CutNo"].ToString(), FabricCombo = row["FabricCombo"].ToString() })
                .Where(group => group.Count() > 1)
                .ToList();

            // 檢查每個分組內的 MarkerName 或 MarkerNo 是否一致
            foreach (var group in groupedRows)
            {
                var firstRow = group.First();
                if (group.Any(row => row["MarkerName"].ToString() != firstRow["MarkerName"].ToString() ||
                                     row["MarkerNo"].ToString() != firstRow["MarkerNo"].ToString()))
                {
                    int index = detailDatas.IndexOf(firstRow);
                    detailgrid.SelectRowTo(index);
                    MyUtility.Msg.WarningBox("In the same fabric combo, different 'Marker Name' and 'Marker No' cannot cut in one time which means cannot set the same cut#.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 檢查 有CutNo & 通過 checkContinue 清單
        /// 相同 (MarkerName,MarkerNo,CutNo) 的 (Markerlength或EstCutDate) 必須相同
        /// P09才有 checkContinue, P02直接帶 row => true
        /// 檢查  MarkerName, MarkerNo, CutNo 相同資料的 markerlength, EstCutDate 必須相同
        /// 顯示訊息
        /// 範例
        /// ValidateCutNoAndMarkerDetails(this.DetailDatas, row => true);
        /// ValidateCutNoAndMarkerDetails(this.DetailDatas, this.CheckContinue);
        /// 寫完了才發現 P02 GroupBy 欄位不一樣
        /// </summary>
        /// <param name="detailDatas">排除已刪除的表身</param>
        /// <param name="checkContinue">P09 檢查的 Function</param>
        /// <returns>bool</returns>
        public static bool ValidateCutNoAndMarkerDetails(IList<DataRow> detailDatas, Func<DataRow, bool> checkContinue)
        {
            // 先找出有 CutNo 且通過檢查的資料
            var groupedData = detailDatas
                .Where(row => !MyUtility.Check.Empty(row["CutNo"]) && checkContinue(row))
                .GroupBy(r => new
                {
                    MarkerName = MyUtility.Convert.GetString(r["MarkerName"]),
                    MarkerNo = MyUtility.Convert.GetString(r["MarkerNo"]),
                    CutNo = MyUtility.Convert.GetString(r["CutNo"]),
                })
                .Where(group => group.Count() > 1)
                .ToList();

            string checkmsg = string.Empty;
            foreach (var group in groupedData)
            {
                var firstRow = group.First();
                var markerlength = Prgs.ConvertFullWidthToHalfWidth(MyUtility.Convert.GetString(firstRow["markerlength"]));
                var estCutDate = MyUtility.Convert.GetDate(firstRow["EstCutDate"]);
                if (group.Any(row => Prgs.ConvertFullWidthToHalfWidth(MyUtility.Convert.GetString(row["markerlength"])) != markerlength ||
                                     MyUtility.Convert.GetDate(row["EstCutDate"]) != estCutDate))
                {
                    checkmsg += $"\r\nMarkerName: {firstRow["MarkerName"]}, MarkerNo: {firstRow["MarkerNo"]}, CutNo: {firstRow["CutNo"]}";
                }
            }

            if (!MyUtility.Check.Empty(checkmsg))
            {
                MyUtility.Msg.WarningBox("The following MarkerName, MarkerNo, and CutNo combinations have different Markerlength or EstCutDate:" + checkmsg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Cutref 相同資料的 CutNo, MarkerName, MarkerNo, Est.CutDate, CutCell, SpreadingNo, Shift 必須相同
        /// </summary>
        /// <param name="detailDatas">排除已刪除的表身</param>
        /// <param name="checkContinue">P09 檢查的 Function</param>
        /// <returns>bool</returns>
        public static bool ValidateCutref(IList<DataRow> detailDatas, Func<DataRow, bool> checkContinue)
        {
            string checkmsg = string.Empty;
            var groupedData = detailDatas
                .Where(row => !MyUtility.Check.Empty(row["Cutref"]) && checkContinue(row))
                .GroupBy(r => MyUtility.Convert.GetString(r["Cutref"]))
                .Where(group => group.Count() > 1)
                .ToList();

            foreach (var group in groupedData)
            {
                string cutRef = group.Key;
                var cutNoList = group.Select(r => MyUtility.Convert.GetString(r["CutNo"])).Distinct().ToList();
                if (cutNoList.Count > 1)
                {
                    MyUtility.Msg.WarningBox($"Cannot have different [Cut#] with same CutRef# <{cutRef}>");
                    return false;
                }

                var markerNameList = group.Select(r => MyUtility.Convert.GetString(r["MarkerName"])).Distinct().ToList();
                if (markerNameList.Count > 1)
                {
                    MyUtility.Msg.WarningBox($"Cannot have different [Marker Name] with same CutRef# <{cutRef}>");
                    return false;
                }

                var distinct1List = group.Select(row => new
                {
                    EstCutDate = MyUtility.Convert.GetDate(row["EstCutDate"]),
                    CutCellID = MyUtility.Convert.GetString(row["CutCellID"]),
                    SpreadingNoID = MyUtility.Convert.GetString(row["SpreadingNoID"]),
                    Shift = MyUtility.Convert.GetString(row["Shift"]),
                }).Distinct().ToList();

                if (distinct1List.Count > 1)
                {
                    MyUtility.Msg.WarningBox($"You can't set different [Est.CutDate] or [CutCell] or [Spreading No.] or [Shift] in same CutRef# <{cutRef}>");
                    return false;
                }

                var markerNoList = group.Select(r => MyUtility.Convert.GetString(r["MarkerNo"])).Distinct().ToList();
                if (markerNoList.Count > 1)
                {
                    var firstRow = group.First();
                    checkmsg += $"\r\n[Cutref]: {cutRef}, [CutNo]: {firstRow["CutNo"]}, [MarkerName]: {firstRow["MarkerName"]} - Different MarkerNo values found: {string.Join(", ", markerNoList)}";
                }
            }

            if (!MyUtility.Check.Empty(checkmsg))
            {
                checkmsg = "For the following Cutref, CutNo, MarkerName combinations, different MarkerNo values were found:" + checkmsg;
                MyUtility.Msg.WarningBox(checkmsg);
                return false;
            }

            return true;
        }

        #endregion

        #region Seq1,Seq2,Refno,Color 開窗/驗證

        public static SelectItem PopupSEQ(string id, string fabricCode, string seq1, string seq2, string refno, string colorID, bool isColor)
        {
            DataTable dt = GetFilterSEQ(id, fabricCode, seq1, seq2, refno, colorID);
            SelectItem selectItem = new SelectItem(dt, "Seq1,Seq2,Refno,ColorID", "3,2,20,3@500,300", seq1, false, ",", headercaptions: "Seq1,Seq2,Refno,Color");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            string newColor = MyUtility.Convert.GetString(selectItem.GetSelecteds()[0]["ColorID"]);
            if (!isColor && !CheckColorSame(colorID, newColor))
            {
                return null;
            }

            return selectItem;
        }

        public static bool ValidatingSEQ(string id, string fabricCode, string seq1, string seq2, string refno, string colorID, out DataTable dt)
        {
            dt = GetFilterSEQ(id, fabricCode, seq1, seq2, refno, colorID);
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

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

        public static DataTable GetFilterSEQ(string id, string fabricCode, string seq1, string seq2, string refno, string colorID)
        {
            string filter = "1=1";
            if (!seq1.IsNullOrWhiteSpace())
            {
                filter += $" AND Seq1 = '{seq1}'";
            }

            if (!seq2.IsNullOrWhiteSpace())
            {
                filter += $" AND Seq2 = '{seq2}'";
            }

            if (!refno.IsNullOrWhiteSpace())
            {
                filter += $" AND Refno = '{refno}'";
            }

            if (!colorID.IsNullOrWhiteSpace())
            {
                filter += $" AND ColorID = '{colorID}'";
            }

            DataTable dt = GetSEQ(id, fabricCode);
            return dt.Select(filter).TryCopyToDataTable(dt);
        }

        public static DataTable GetSEQ(string id, string fabricCode)
        {
            string sqlcmd = $@"
SELECT
    psd.SEQ1
   ,psd.SEQ2
   ,psd.Refno
   ,ColorID = ISNULL(psdc.SpecValue, '')
   ,psd.SCIRefno
FROM PO_Supp_Detail psd WITH (NOLOCK)
INNER JOIN PO_Supp_Detail_Spec psdc WITH (NOLOCK) ON psdc.ID = psd.id AND psdc.seq1 = psd.seq1 AND psdc.seq2 = psd.seq2 AND psdc.SpecColumnID = 'Color'
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
WHERE psd.ID = '{id}'
AND psd.Junk = 0
AND EXISTS (
    SELECT 1
    FROM Order_BOF WITH (NOLOCK)
    INNER JOIN Fabric WITH (NOLOCK) ON Fabric.SCIRefno = Order_BOF.SCIRefno
    WHERE Order_BOF.FabricCode = '{fabricCode}'
    AND Order_BOF.Id = psd.ID
    AND Fabric.BrandRefNo = f.BrandRefNo
)
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
            System.Windows.Forms.BindingSource sizeRatiobs = (System.Windows.Forms.BindingSource)gridSizeRatio.DataSource;
            UpdateExcess(currentDetailData, (DataTable)sizeRatiobs.DataSource, dtDistribute, form);
            return true;
        }
        #endregion

        #region SizeRatio / Distribute Qty 驗證

        /// <summary>
        /// return true → 要更新主表資訊
        /// </summary>
        /// <inheritdoc/>
        public static bool QtyCellValidating(
            Ict.Win.UI.DataGridViewCellValidatingEventArgs e,
            DataRow currentDetailData,
            Sci.Win.UI.Grid grid,
            DataTable dtSizeRatio,
            DataTable dtDistribute,
            CuttingForm form)
        {
            DataRow dr = grid.GetDataRow(e.RowIndex);
            if (grid.IsEditingReadOnly || dr == null)
            {
                return false;
            }

            int oldvalue = MyUtility.Convert.GetInt(dr["Qty"]);
            int newvalue = MyUtility.Convert.GetInt(e.FormattedValue);
            if (oldvalue == newvalue)
            {
                return false;
            }

            dr["Qty"] = newvalue;
            dr.EndEdit();

            UpdateExcess(currentDetailData, dtSizeRatio, dtDistribute, form);

            return true;
        }
        #endregion

        #region Distribute OrderID, Article, SizeCode 開窗/驗證

        public static bool Distribute3CellEditingMouseDown(
            Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e,
            DataRow currentDetailData,
            DataTable dtSizeRatio,
            Sci.Win.UI.Grid gridDistributeToSP)
        {
            DataRow dr = gridDistributeToSP.GetDataRow(e.RowIndex);
            if (e.Button != MouseButtons.Right || gridDistributeToSP.IsEditingReadOnly || dr["OrderID"].ToString().Equals("EXCESS", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // 正常操作流程不會觸發, 防呆用
            if (dtSizeRatio.DefaultView.ToTable().Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please insert size ratio data first!");
                return false;
            }

            // 開窗篩選條件不包含觸發欄位
            string columnName = gridDistributeToSP.Columns[e.ColumnIndex].Name;
            string orderID = MyUtility.Convert.GetString(dr["OrderID"]);
            string article = MyUtility.Convert.GetString(dr["Article"]);
            string sizeCode = MyUtility.Convert.GetString(dr["SizeCode"]);
            switch (columnName.ToLower())
            {
                case "orderid":
                    orderID = string.Empty;
                    break;
                case "article":
                    article = string.Empty;
                    break;
                case "sizecode":
                    sizeCode = string.Empty;
                    break;
            }

            DataTable dt = FilterOrder_Qty_By_SizeRatio(currentDetailData["ID"].ToString(), orderID, article, sizeCode, dtSizeRatio);
            SelectItem selectItem = new SelectItem(dt, "ID,Article,SizeCode", "20,15,10", MyUtility.Convert.GetString(dr[columnName]), false, ",", "SP#,Article,Size");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return false;
            }

            dr["OrderID"] = selectItem.GetSelecteds()[0]["ID"];
            dr["Article"] = selectItem.GetSelecteds()[0]["Article"];
            dr["SizeCode"] = selectItem.GetSelecteds()[0]["SizeCode"];
            dr.EndEdit();

            // 立即帶入 Sewinline
            dr["Sewinline"] = GetMinSewinline(dr["OrderID"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString());
            return true;
        }

        /// <summary>
        /// return true → 要更新主表資訊
        /// </summary>
        /// <inheritdoc/>
        public static bool Distribute3CellValidating(
            Ict.Win.UI.DataGridViewCellValidatingEventArgs e,
            DataRow currentDetailData,
            DataTable dtSizeRatio,
            Sci.Win.UI.Grid gridDistributeToSP,
            CuttingForm form)
        {
            DataRow dr = gridDistributeToSP.GetDataRow(e.RowIndex);
            string columnName = gridDistributeToSP.Columns[e.ColumnIndex].Name;
            string newvalue = e.FormattedValue.ToString();
            string oldvalue = dr[columnName].ToString();

            if (gridDistributeToSP.IsEditingReadOnly || dr["OrderID"].ToString().Equals("EXCESS", StringComparison.OrdinalIgnoreCase) || oldvalue == newvalue)
            {
                return false;
            }

            // 正常操作流程不會觸發, 防呆用
            if (dtSizeRatio.DefaultView.ToTable().Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please insert size ratio data first!");
                dr[columnName] = string.Empty;
                dr.EndEdit();
                e.Cancel = true;
                return false;
            }

            if (newvalue == string.Empty)
            {
                dr[columnName] = string.Empty;
                dr.EndEdit();
                return true;
            }

            string orderID = MyUtility.Convert.GetString(dr["OrderID"]);
            string article = MyUtility.Convert.GetString(dr["Article"]);
            string sizeCode = MyUtility.Convert.GetString(dr["SizeCode"]);
            string msg = string.Empty;
            switch (columnName.ToLower())
            {
                case "orderid":
                    orderID = newvalue;
                    msg = $"<SP#>:{newvalue},<Article>:{dr["Article"]},<Size>:{dr["SizeCode"]}";
                    break;
                case "article":
                    article = newvalue;
                    msg = $"<SP#>:{dr["OrderID"]},<Article>:{newvalue},<Size>:{dr["SizeCode"]}";
                    break;
                case "sizecode":
                    sizeCode = newvalue;
                    msg = $"<SP#>:{dr["OrderID"]},<Article>:{dr["Article"]},<Size>:{newvalue}";
                    break;
            }

            if (FilterOrder_Qty_By_SizeRatio(currentDetailData["ID"].ToString(), orderID, article, sizeCode, dtSizeRatio).Rows.Count == 0)
            {
                dr[columnName] = string.Empty;
                dr.EndEdit();
                MyUtility.Msg.WarningBox(msg + " not exists qty break down");
                e.Cancel = true;
                return false;
            }

            dr[columnName] = newvalue;
            dr.EndEdit();

            // 驗證需要重算Excess
            var distributeToSPbs = (System.Windows.Forms.BindingSource)gridDistributeToSP.DataSource;
            UpdateExcess(currentDetailData, dtSizeRatio, (DataTable)distributeToSPbs.DataSource, form);

            // 立即帶入 Sewinline
            dr["SewInline"] = GetMinSewinline(dr["OrderID"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString());
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

        public static object GetMinSewinline(string orderID, string article, string sizeCode)
        {
            string sqlcmd = $@"
SELECT
    Sewinline = MIN(SewingSchedule.Inline)
FROM SewingSchedule WITH (NOLOCK)
INNER JOIN SewingSchedule_Detail WITH (NOLOCK) ON SewingSchedule_detail.ID = SewingSchedule.ID
WHERE SewingSchedule_Detail.OrderID = '{orderID}'
AND SewingSchedule_Detail.Article = '{article}'
AND SewingSchedule_Detail.SizeCode = '{sizeCode}'
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt.Rows[0][0];
        }
        #endregion

        #region PatternPanel
        public static void BindPatternPanelEvents(Ict.Win.UI.DataGridViewTextBoxColumn column, string id)
        {
            column.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    DataTable dt = GetPatternPanel(id);
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele = new SelectItem(dt, "PatternPanel,FabricPanelCode", "10,10", row["PatternPanel"].ToString(), false, ",", "Pattern Panel,Fabric Panel Code");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    row["PatternPanel"] = sele.GetSelecteds()[0]["PatternPanel"];
                    row["FabricPanelCode"] = sele.GetSelecteds()[0]["FabricPanelCode"];
                }
            };

            column.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                string columnName = grid.Columns[e.ColumnIndex].Name;
                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                string oldValue = row[columnName].ToString();
                string newValue = e.FormattedValue.ToString();
                if (newValue.IsNullOrWhiteSpace() || oldValue == newValue)
                {
                    return;
                }

                DataTable dt = GetPatternPanel(id);
                if (dt.Select($"{columnName} = '{newValue}'").Length == 0)
                {
                    MyUtility.Msg.WarningBox($"< {columnName} : {newValue} > not found!!!");
                    row[columnName] = string.Empty;
                    e.Cancel = true;
                }
                else
                {
                    row[columnName] = newValue;
                }

                row.EndEdit();
            };
        }

        public static DataTable GetPatternPanel(string id)
        {
            string sqlcmd = $@"
SELECT
    PatternPanel
   ,FabricPanelCode
   ,FabricCode
FROM Order_FabricCode WITH(NOLOCK)
WHERE ID = '{id}'
ORDER BY FabricPanelCode,PatternPanel
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

        #region Other Function

        /// <summary>
        /// 更新 CurrentDetailData 的非實體欄位: SizeCode_CONCAT, TotalCutQty_CONCAT, 因 SizeRatio 的欄位: SizeCode,Qty 調整
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
        /// 更新 CurrentDetailData 的非實體欄位: TotalDistributeQty, 因 Distribute 資訊變動
        /// </summary>
        /// <inheritdoc/>
        public static void UpdateTotalDistributeQty(DataRow currentDetailData, DataTable dtDistribute, CuttingForm form)
        {
            string filter = GetFilter(currentDetailData, form) + " AND (OrderID = 'EXCESS' OR (OrderID <> '' AND Article <> '' AND SizeCode <>''))";
            currentDetailData["TotalDistributeQty"] = dtDistribute.Select(filter).Sum(row => MyUtility.Convert.GetInt(row["Qty"]));
        }

        /// <summary>
        /// 更新 CurrentDetailData 的非實體欄位: Sewinline, 因 Distribute 資訊變動
        /// </summary>
        /// <inheritdoc/>
        public static void UpdateMinSewinline(DataRow currentDetailData, DataTable dtDistribute, CuttingForm form)
        {
            DateTime? sewinline = dtDistribute.Select(GetFilter(currentDetailData, form)).AsEnumerable().Min(row => MyUtility.Convert.GetDate(row["Sewinline"]));
            currentDetailData["Sewinline"] = sewinline ?? (object)DBNull.Value;
        }

        /// <summary>
        /// 更新 CurrentDetailData 的非實體欄位: FabricCombo,FabricCode,FabricPanelCode,PatternPanel_CONCAT,FabricPanelCode_CONCAT 因 PatternPanel 資訊變動
        /// </summary>
        /// <inheritdoc/>
        public static void UpdatebyPatternPanel(DataRow currentDetailData, DataTable dtPatternPanel, CuttingForm form)
        {
            DataRow minFabricPanelCode = GetMinFabricPanelCode(currentDetailData, dtPatternPanel, form);
            if (minFabricPanelCode == null)
            {
                return;
            }

            DataTable dt = GetPatternPanel(currentDetailData["ID"].ToString());
            DataRow[] drs = dt.Select($"FabricPanelCode = '{minFabricPanelCode["FabricPanelCode"]}'");
            if (drs.Length > 0)
            {
                // 實體欄位
                currentDetailData["FabricCombo"] = drs[0]["PatternPanel"];
                currentDetailData["FabricPanelCode"] = drs[0]["FabricPanelCode"];
                currentDetailData["FabricCode"] = drs[0]["FabricCode"];

                // 串接字串
                DataRow[] drsPatternPanel = dtPatternPanel.Select(GetFilter(currentDetailData, form));
                currentDetailData["PatternPanel_CONCAT"] = drsPatternPanel.AsEnumerable().OrderBy(row => MyUtility.Convert.GetString(row["FabricPanelCode"])).Select(row => MyUtility.Convert.GetString(row["PatternPanel"])).JoinToString("+");
                currentDetailData["FabricPanelCode_CONCAT"] = drsPatternPanel.AsEnumerable().OrderBy(row => MyUtility.Convert.GetString(row["FabricPanelCode"])).Select(row => MyUtility.Convert.GetString(row["FabricPanelCode"])).JoinToString("+");
            }
        }

        public static DataRow GetMinFabricPanelCode(DataRow currentDetailData, DataTable dtPatternPanel, CuttingForm form)
        {
           return dtPatternPanel.Select(GetFilter(currentDetailData, form)).AsEnumerable().OrderBy(row => MyUtility.Convert.GetString(row["FabricPanelCode"])).FirstOrDefault();
        }

        /// <summary>
        /// 更新 CurrentDetailData 的*實體*欄位:OrderID, 只有Cutting.WorkType = 2(By SP)才執行, 取 Distribute 最小 OrderID
        /// </summary>
        /// <inheritdoc/>
        public static void UpdateMinOrderID(string workType, DataRow currentDetailData, DataTable dtDistribute, CuttingForm form)
        {
            if (workType != "2")
            {
                return;
            }

            string filter = GetFilter(currentDetailData, form) + " AND OrderID <> 'EXCESS'";
            currentDetailData["OrderID"] = dtDistribute.Select(filter).AsEnumerable().Min(row => MyUtility.Convert.GetString(row["OrderID"]));
        }

        /// <summary>
        /// 更新 DataTable Distribute 的*實體*欄位: SizeCode, 因 SizeRatio 的欄位: SizeCode 調整
        /// </summary>
        /// <inheritdoc/>
        public static void UpdateDistribute_Size(DataRow currentDetailData, DataTable dtDistribute, string oldvalue, string newvalue, CuttingForm form)
        {
            if (form == CuttingForm.P02)
            {
                return;
            }

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
            if (form == CuttingForm.P02)
            {
                return;
            }

            string filter = GetFilter(currentDetailData, form);
            foreach (DataRow dr in dtSizeRatio.Select(filter))
            {
                int ttlQty_SizeCode = MyUtility.Convert.GetInt(dr["Qty"]) * MyUtility.Convert.GetInt(currentDetailData["Layer"]); // 此 SizeCode 總數量
                string sizeCode = dr["SizeCode"].ToString();
                string filterSizeCode = $"{filter} AND SizeCode = '{sizeCode}'";

                int totalDistributedQty = dtDistribute.Select($"{filterSizeCode} AND OrderID <> 'EXCESS'").AsEnumerable().Sum(row => MyUtility.Convert.GetInt(row["Qty"]));

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
                    newExcessRow["WorkOrderForOutputUKey"] = currentDetailData["Ukey"];
                    newExcessRow["tmpkey"] = currentDetailData["tmpkey"];
                    newExcessRow["ID"] = currentDetailData["ID"];
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
            return $@"{GetWorkOrderUkeyName(form)} = {currentDetailData["Ukey"]} AND tmpKey = {currentDetailData["tmpKey"]}";
        }

        public static string GetWorkOrderUkeyName(CuttingForm form)
        {
            return $"WorkOrderFor{GetWorkOrderName(form)}Ukey";
        }

        /// <summary>
        /// Cutting P02/P09 資料表名稱的中間字串 WorkOrderFor____
        /// </summary>
        /// <inheritdoc/>
        public static string GetWorkOrderName(CuttingForm form)
        {
            switch (form)
            {
                case CuttingForm.P02:
                    return "Planning";
                case CuttingForm.P09:
                    return "Output";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 計算 ConsPC *實體*欄位
        /// </summary>
        /// <inheritdoc/>
        public static decimal CalculateConsPC(string markerLength, DataRow currentDetailData, DataTable dtSizeRatio, CuttingForm form)
        {
            if (markerLength == string.Empty || markerLength == "00Y00-0/0+0\"" || markerLength == "Y  - / + \"")
            {
                return 0;
            }

            decimal markerLengthNum = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"Select dbo.MarkerLengthToYDS('{markerLength}')"));
            decimal sizeRatioQty = dtSizeRatio.Select(GetFilter(currentDetailData, form)).AsEnumerable().Sum(row => MyUtility.Convert.GetDecimal(row["Qty"]));
            return sizeRatioQty == 0 ? 0 : markerLengthNum / sizeRatioQty;
        }

        /// <summary>
        /// 計算 Cons *實體*欄位, 當改變3個欄位 ConsPC,Layer,SizeRatio.Qty 時
        /// </summary>
        /// <inheritdoc/>
        public static decimal CalculateCons(DataRow currentDetailData, DataTable dtSizeRatio, CuttingForm form)
        {
            decimal sizeRatioQty = dtSizeRatio.Select(GetFilter(currentDetailData, form)).AsEnumerable().Sum(row => MyUtility.Convert.GetDecimal(row["Qty"]));
            return MyUtility.Convert.GetDecimal(currentDetailData["ConsPC"]) * MyUtility.Convert.GetDecimal(currentDetailData["Layer"]) * sizeRatioQty;
        }

        public static void AddThirdDatas(DataRow currentDetailData, DataRow oldRow, DataTable dtTarget, CuttingForm form)
        {
            string filter = GetFilter(oldRow, form);
            DataTable source = dtTarget.Select(filter).TryCopyToDataTable(dtTarget);

            // 複製出來的要填入對應新的 Key
            foreach (DataRow ddr in source.Rows)
            {
                ddr[GetWorkOrderUkeyName(form)] = 0;
                ddr["tmpKey"] = currentDetailData["tmpKey"];
                dtTarget.ImportRowAdded(ddr);
            }
        }
        #endregion
    }
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore SA1602 // Enumeration items should be documented
}

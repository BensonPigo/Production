using Ict;
using Ict.Win.UI;
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
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Ict;
using Sci.Production.Prg;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Security.Cryptography;
using Sci.Production.PublicPrg;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02, P09共用
    /// </summary>
    public partial class CuttingWorkOrder
    {
        /// <summary>
        /// 根據Cutting Poid，取得所有可用的Seq、Refno、Color
        /// </summary>
        private static DataTable dt_AllSeqRefnoColor;

        #region Cutting P02 專用

        /// <inheritdoc/>
        public static void SpEditingMouseDown(DataGridViewEditingControlMouseEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid, string workType)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Parent form 若是非編輯狀態，且Cutting.WorkType != 2 就 return
                if (!srcForm.EditMode || workType != "2")
                {
                    MyUtility.Msg.WarningBox("Only <By SP#> can use.");
                    e.EditingControl.Text = string.Empty;
                    return;
                }

                DataRow dr = srcGrid.GetDataRow(e.RowIndex);
                SelectItem sele;

                string cmd = $@"SELECT ID FROM Orders WHERE POID = '{poid}' AND Junk=0";

                DBProxy.Current.Select(null, cmd, out DataTable dtSP);

                if (dtSP == null)
                {
                    return;
                }

                sele = new SelectItem(dtSP, "ID", "20", MyUtility.Convert.GetString(dr["OrderID"]), false, ",");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                dr["OrderID"] = sele.GetSelecteds()[0]["ID"];
                e.EditingControl.Text = sele.GetSelectedString();
            }
        }

        /// <inheritdoc/>
        public static bool SpCellValidating(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid, string workType)
        {
            if (!srcForm.EditMode || workType != "2")
            {
                return true;
            }

            // 右鍵彈出功能
            if (e.RowIndex == -1)
            {
                return true;
            }

            DataRow dr = srcGrid.GetDataRow(e.RowIndex);
            string oldvalue = dr["OrderID"].ToString();
            string newvalue = e.FormattedValue.ToString();
            if (oldvalue == newvalue || string.IsNullOrEmpty(newvalue))
            {
                return true;
            }

            string cmd = $@"SELECT ID FROM Orders WHERE POID = '{poid}' AND Junk=0 AND ID = @ID ";
            DBProxy.Current.Select(null, cmd, new List<SqlParameter>() { new SqlParameter("@ID", newvalue) }, out DataTable dtSP);

            if (dtSP == null || dtSP.Rows.Count == 0)
            {
                dr["OrderID"] = string.Empty;
                dr.EndEdit();
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("<SP#> : {0} data not found!", newvalue));
                return false;
            }

            dr["OrderID"] = newvalue;
            dr.EndEdit();
            return true;
        }

        /// <inheritdoc/>
        public static void ArticleEditingMouseDown(object sender, DataGridViewEditingControlMouseEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid, string workType)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Parent form 若是非編輯狀態就 return
                if (!srcForm.EditMode)
                {
                    return;
                }

                DataRow dr = srcGrid.GetDataRow(e.RowIndex);
                SelectItem sele;

                string cmd = string.Empty;

                if (MyUtility.Convert.GetString(dr["Order_EachconsUkey"]) != "0" && !MyUtility.Check.Empty(dr["Order_EachconsUkey"]))
                {
                    cmd = $@"SELECT Article FROM Order_EachCons_Article WHERE Order_EachConsUkey = {dr["Order_EachconsUkey"]}";
                }
                else if (workType == "2")
                {
                    cmd = $@"SELECT Article FROM Order_Article WHERE ID='{dr["OrderID"]}' GROUP BY Article";
                }
                else if (workType == "1")
                {
                    cmd = $@"
SELECT Article 
FROM Order_Article
INNER JOIN Orders ON Orders.ID= Order_Article.ID
WHERE 1=1
AND Orders.POID = '{poid}'
GROUP BY Article";
                }

                DBProxy.Current.Select(null, cmd, out DataTable dtArticle);

                if (dtArticle == null)
                {
                    return;
                }

                sele = new SelectItem(dtArticle, "Article", "20", MyUtility.Convert.GetString(dr["Article"]), false, ",");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                dr["Article"] = sele.GetSelecteds()[0]["Article"];
                e.EditingControl.Text = sele.GetSelectedString();
            }
        }

        /// <inheritdoc/>
        public static bool ArticleCellValidating(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid, string workType)
        {
            if (!srcForm.EditMode)
            {
                return true;
            }

            if (e.RowIndex == -1)
            {
                return true;
            }

            DataRow dr = srcGrid.GetDataRow(e.RowIndex);
            string oldvalue = dr["Article"].ToString();
            string newvalue = e.FormattedValue.ToString();
            if (oldvalue == newvalue)
            {
                return true;
            }

            string cmd = string.Empty;

            if (MyUtility.Convert.GetString(dr["Order_EachconsUkey"]) != "0" && !MyUtility.Check.Empty(dr["Order_EachconsUkey"]))
            {
                cmd = $@"SELECT Article FROM Order_EachCons_Article WHERE Order_EachConsUkey = {dr["Order_EachconsUkey"]} AND Article = @Article";
            }
            else if (workType == "2")
            {
                cmd = $@"SELECT Article FROM Order_Article WHERE ID='{dr["OrderID"]}' AND Article = @Article GROUP BY Article";
            }
            else if (workType == "1")
            {
                cmd = $@"
SELECT Article 
FROM Order_Article
INNER JOIN Orders ON Orders.ID= Order_Article.ID
WHERE 1=1
AND Orders.POID = '{poid}'
AND Article = @Article
GROUP BY Article";
            }

            DBProxy.Current.Select(null, cmd, new List<SqlParameter>() { new SqlParameter("@Article", newvalue) }, out DataTable dtArticle);

            if (dtArticle == null || dtArticle.Rows.Count == 0)
            {
                dr["Article"] = string.Empty;
                dr.EndEdit();
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("<Article> : {0} data not found!", newvalue));
                return false;
            }

            dr["Article"] = newvalue;
            dr.EndEdit();
            return true;
        }
        #endregion

        #region Batch Assign 的Seq驗證，由於無法取得單一筆Fabric Code，所以需要

        /// <summary>
        /// 根據Cutting Poid，取得所有可用的Seq、Refno、Color，找過一次之後存在全域變數，後續校驗繼續用
        /// </summary>
        /// <param name="id">Cutting.ID</param>
        /// <returns>DataTable</returns>
        public static DataTable GetAllSeqRefnoColor(string id)
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
    WHERE Order_BOF.Id = psd.ID
    AND Fabric.BrandRefNo = f.BrandRefNo
	AND EXISTS(
		SELECT FabricCode
		FROM Order_FabricCode ofc WITH(NOLOCK)
		WHERE ofc.ID =  psd.ID AND ofc.FabricCode = Order_BOF.FabricCode
	)
)
";

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            // 撈過一次之後存進全域變數
            if (MyUtility.Check.Empty(dt_AllSeqRefnoColor) || dt_AllSeqRefnoColor.Rows.Count == 0)
            {
                dt_AllSeqRefnoColor = dt.Copy();
            }
            else if (!dt.AsEnumerable().Where(o => o["ID"].ToString() == id).Any())
            {
                dt_AllSeqRefnoColor = dt.Copy();
            }

            return dt;
        }

        /// <summary>
        /// 設定 所有可用的Seq、Refno、Color 的全域變數DataTable
        /// </summary>
        /// <param name="id">Cutting.ID</param>
        public static void SetGolobalAllSeqRefnoColor(string id)
        {
            // 根據POID，找出所有 Seq、fabricCode、refno、colorID
            if (MyUtility.Check.Empty(dt_AllSeqRefnoColor) || dt_AllSeqRefnoColor.Rows.Count == 0)
            {
                GetAllSeqRefnoColor(id);
            }
            else if (!dt_AllSeqRefnoColor.AsEnumerable().Where(o => o["ID"].ToString() == id).Any())
            {
                GetAllSeqRefnoColor(id);
            }
        }

        /// <summary>
        /// 根據Cutting Poid，取得所有可用的Seq、Refno、Color，然後開窗
        /// </summary>
        /// <param name="id">Cutting.ID</param>
        /// <returns>SelectItem</returns>
        public static SelectItem PopupAllSeqRefnoColor(string id)
        {
            // 根據POID，找出所有 Seq、fabricCode、refno、colorID
            SetGolobalAllSeqRefnoColor(id);

            DataTable dt = dt_AllSeqRefnoColor.Copy();
            SelectItem selectItem = new SelectItem(dt, "Seq1,Seq2,Refno,ColorID", "3,2,20,3@500,300", string.Empty, false, ",", headercaptions: "Seq1,Seq2,Refno,Color");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            return selectItem;
        }

        /// <summary>
        /// 根據Cutting Poid，取得所有可用的Seq、Refno、Color，再進行Filter
        /// </summary>
        /// <param name="id">根據Cutting Poid</param>
        /// <param name="seq1">seq1</param>
        /// <param name="seq2">seq2</param>
        /// <param name="refno">refno</param>
        /// <param name="colorID">colorID</param>
        /// <returns>DataTable</returns>
        public static DataTable GetFilterAllSeqRefnoColor(string id, string seq1, string seq2, string refno, string colorID)
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

            SetGolobalAllSeqRefnoColor(id);
            DataTable dt = dt_AllSeqRefnoColor.Copy();

            return dt.Select(filter).TryCopyToDataTable(dt);
        }

        /// <summary>
        /// Batch Assign的時候會批次寫入Seq，逐筆回DB檢查太慢，使用暫存的DataTable檢查
        /// </summary>
        /// <param name="id">Cutting POID</param>
        /// <param name="fabricCode">當前DateRow的fabricCode</param>
        /// <param name="seq1">當前DateRow的seq1</param>
        /// <param name="seq2">當前DateRow的seq2</param>
        /// <param name="refno">當前DateRow的refno</param>
        /// <param name="colorID">當前DateRow的colorID</param>
        /// <param name="dt">dt</param>
        /// <returns>結果</returns>
        public static bool ValidatingSeqWithoutFabricCode(string id, string fabricCode, string seq1, string seq2, string refno, string colorID, out DataTable dt)
        {
            SetGolobalAllSeqRefnoColor(id);

            // 以全域變數的DataTable來校驗，避免多次回DB撈
            dt = dt_AllSeqRefnoColor.Copy();

            dt.AsEnumerable().Where(o => o["ID"].ToString() == id
            && o["FabricCode"].ToString() == fabricCode
            && o["Seq1"].ToString() == seq1
            && o["Seq2"].ToString() == seq2
            && o["Refno"].ToString() == refno
            && o["ColorID"].ToString() == colorID).TryCopyToDataTable(dt);

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            return true;
        }
        #endregion

        #region 匯入功能

        #region 全域變數

        /// <summary>
        /// sheet 序號
        /// </summary>
        private int markerSerNo = 1;
        private List<long> listWorkOrderUkey;

        /// <summary>
        /// 當前要Insert哪一張表，P02的話是 WorkOrderForPlanning；P09的話是 WorkOrderForOutput
        /// </summary>
        private string TableName = string.Empty;

        /// <summary>
        /// P02的話是 WorkOrderForPlanningUkey；P09的話是 WorkOrderForOutputUkey
        /// </summary>
        private string TableMainKeyColName = string.Empty;

        private string CuttingPOID = string.Empty;
        private string MDivisionid = string.Empty;
        private string FactoryID = string.Empty;

        /// <summary>
        /// 碼、英吋換算比例
        /// </summary>
        private decimal inchToYdsRate = 0;

        private bool isNoMatchSP;
        #endregion

        /// <summary>
        /// 匯入指定格式的Excel，寫入資料庫WorkOrder、SizeRatio、PatternPanel、Distribute四張資料表的資料
        /// </summary>
        /// <param name="cuttingID">Cutting.ID</param>
        /// <param name="mDivisionid">mDivisionid</param>
        /// <param name="factoryID">factoryID</param>
        /// <param name="tableName">要寫入的Table</param>
        /// <returns>Result</returns>
        public DualResult ImportMarkerExcel(string cuttingID, string mDivisionid, string factoryID, string tableName)
        {
            this.CuttingPOID = cuttingID;
            this.MDivisionid = mDivisionid;
            this.FactoryID = factoryID;
            this.TableName = tableName;

            if (tableName == "WorkOrderForOutput")
            {
                // Cutting P09
                this.TableMainKeyColName = "WorkOrderForOutputUkey";
            }
            else if (tableName == "WorkOrderForPlanning")
            {
                // Cutting P02
                this.TableMainKeyColName = "WorkOrderForPlanningUkey";
            }

            // 取得換算Rate
            this.inchToYdsRate = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("SELECT dbo.GetUnitRate('Inch','YDS')"));

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";

            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return new DualResult(true, "NotImport");
            }

            string filename = openFileDialog.FileName;

            this.isNoMatchSP = true;

            try
            {
                // 取得所有 WorkOrder 資料
                List<WorkOrder> excelWk = this.LoadExcel(filename);

                if (!excelWk.Any())
                {
                    return new DualResult(true, "No correct data in excel file, please check format.");
                }

                SqlConnection sqlConn;
                DualResult result = DBProxy._OpenConnection(null, out sqlConn);

                if (!result)
                {
                    return result;
                }

                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5)))
                using (sqlConn)
                {
                    var poids = excelWk.Select(o => o.ID).Distinct().ToList();
                    var fabricPanelCodes = excelWk.Select(o => o.FabricPanelCode).Distinct().ToList();
                    var colorIDs = excelWk.Select(o => o.Colorid).Distinct().ToList();
                    List<WorkOrder> compare = this.GetOrder_ColorComboInfoList(poids, fabricPanelCodes, colorIDs);

                    foreach (var e in excelWk)
                    {
                        var firstCompare = compare.Where(o => o.ID == e.ID && o.FabricPanelCode == e.FabricPanelCode && o.Colorid == e.Colorid);
                        if (!firstCompare.Any())
                        {
                            return new DualResult(true, $"No data mapping in Order ColorCombo <Fabric Code>,<Color ID>: {e.FabricCode},{e.Colorid}.");
                        }

                        e.SEQ1 = firstCompare.FirstOrDefault().SEQ1;
                        e.SEQ2 = firstCompare.FirstOrDefault().SEQ2;
                        e.Refno = firstCompare.FirstOrDefault().Refno;
                        e.SCIRefno = firstCompare.FirstOrDefault().SCIRefno;
                        e.FabricCode = firstCompare.FirstOrDefault().FabricCode;
                        e.FabricPanelCode = firstCompare.FirstOrDefault().FabricPanelCode;
                        e.FabricCombo = firstCompare.FirstOrDefault().FabricCombo;

                        // 比較Excel填的ColorCombo設定的層數，看Excel填的有沒有超過上限
                        var layer = e.ExcelLayer > firstCompare.FirstOrDefault().CuttingLayer ? firstCompare.FirstOrDefault().CuttingLayer : e.ExcelLayer;
                        e.Layer = layer;
                        e.CuttingLayer = firstCompare.FirstOrDefault().CuttingLayer;

                        // Cons = garmentCnt * consPc * Layer
                        e.Cons = e.Cons * layer;
                    }

                    List<long> newWorkOrderUkey = this.InsertWorkOrder(excelWk, sqlConn);
                    if (!newWorkOrderUkey.Any())
                    {
                        return new DualResult(true, $"Insert data Fail.");
                    }

                    this.InsertWorkOrder_Distribute(newWorkOrderUkey, sqlConn);

                    transactionScope.Complete();
                    transactionScope.Dispose();
                }

                if (this.isNoMatchSP)
                {
                    MyUtility.Msg.InfoBox($"Excel not contain Cutting SP<{cuttingID}>");
                    return new DualResult(true, "NotImport");
                }

                return new DualResult(true);
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 載入Excel
        /// </summary>
        /// <param name="filename">檔名</param>
        /// <returns>組合好的物件</returns>
        private List<WorkOrder> LoadExcel(string filename)
        {
            List<WorkOrder> workOrders = new List<WorkOrder>();
            Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            excel.Workbooks.Open(MyUtility.Convert.GetString(filename));
            int sheetCnt = excel.ActiveWorkbook.Worksheets.Count;

            for (int i = 1; i <= sheetCnt; i++)
            {
                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[i];
                this.markerSerNo = 1;

                // 檢查裁剪母單單號
                string poID = worksheet.GetCellValue(2, 2);
                if (poID != this.CuttingPOID)
                {
                    Marshal.ReleaseComObject(worksheet);
                    continue;
                }

                this.isNoMatchSP = false;

                // 開始檢查Excel
                string keyWord_FabPanelCode = "Panel Code:";
                string keyWord_Layer = "Layers";
                string keyWord_MarkerEnd = "Ttl. Qty.";
                int curRowIndex = 1;
                int emptyRowCount = 0;

                // 紀錄這一區資料有幾Row，到哪一個Index
                int curDataStart_Y = 1;

                // 紀錄這一區資料有幾Column，到哪一個Index
                int curDataStart_X = 0;

                // 橫向開始找「Layers」欄位(對應 範本Excel的col = AB, Row = 8 )，但是User可能自己複製column，所以動態抓，整個Sheet的Column是一起的，所以一張Sheet找一次就好
                if (curDataStart_X == 0)
                {
                    while (worksheet.GetCellValue(curDataStart_X + 1, 8) != keyWord_Layer)
                    {
                        curDataStart_X++;
                    }
                }

                // 開始找這一區的表格
                while (true)
                {
                    // 如果讀取超過20行都沒有「Panel Code:」，就當作此sheet已經沒資料
                    if (emptyRowCount > 20)
                    {
                        break;
                    }

                    // 開始找「Panel Code:」欄位(對應 Excel的col = A, Row = 4 )
                    if (worksheet.GetCellValue(1, curRowIndex) != keyWord_FabPanelCode)
                    {
                        curRowIndex++;
                        emptyRowCount++;
                        continue;
                    }

                    // 找到「Panel Code:」欄位，重置
                    emptyRowCount = 0;
                    curDataStart_Y = 1;

                    // 若找到，「Panel Code:」的行數為 curRowIndex
                    // 計算「Panel Code:」到「Ttl. Qty.」距離多少Row，User填了幾Row的資料，超50 Row都找不到就算了(報表範本有鎖格式，所以應該不可能找不到)
                    // AA 25
                    curDataStart_Y += curRowIndex;
                    string a = worksheet.GetCellValue(27, curDataStart_Y);
                    while (a != keyWord_MarkerEnd && emptyRowCount < 50)
                    {
                        curDataStart_Y++;
                        emptyRowCount++;
                        a = worksheet.GetCellValue(27, curDataStart_Y);
                        continue;
                    }

                    emptyRowCount = 0;

                    // 下一個「Panel Code:」的起點
                    int nextFabPanelCodeStart = curDataStart_Y + 2;

                    // 計算Pattern Panel和Marker Name填寫的Row有幾行
                    // 6 = 「Panel Code:」到「NK Name」的距離
                    // 1 = 「Ttl. Qty.」跟最後一筆資料的距離
                    int markerRowCount = curDataStart_Y - curRowIndex - 6;

                    // 取得「Panel Code:」的值(對應 Excel的col = B, Row = 3 )，「Panel Code:」在「Panel Code:」的下一行，「Panel Code:」的行數為 curRowIndex
                    //wk.FabricPanelCode = worksheet.GetCellValue(2, curRowIndex);
                    string fabricPanelCode = worksheet.GetCellValue(2, curRowIndex);

                    // 如果「Panel Code:」找不到，則跳到下一個「Panel Code:」的起點
                    if (MyUtility.Check.Empty(fabricPanelCode))
                    {
                        continue;
                    }

                    // 取得「Color:」的值((對應 Excel的col = B, Row = 5 )
                    string[] colorInfo = worksheet.GetCellValue(2, curRowIndex + 1).Split('-');

                    string tmpColorid = colorInfo.Length >= 1 ? colorInfo[0] : string.Empty;
                    string tmpTone = colorInfo.Length >= 2 ? colorInfo[1] : string.Empty;
                    string markername = "MK_" + this.markerSerNo.ToString().PadLeft(3, '0');

                    // 取得Size Ratio Range (例如：1 ,4 ,28 ,24 )
                    Excel.Range rangeSizeRatio = worksheet.GetRange(1, curRowIndex, curDataStart_X + 1, nextFabPanelCodeStart - 3);

                    // 讀每一個MkName，起點是從A4「Panel Code:」往下數，所以是 = 7；終點是下筆資料「Panel Code:」的Y值 - 3，
                    // 直向往下找
                    for (int idxMarker = 7; idxMarker < 7 + markerRowCount; idxMarker++)
                    {
                        // 準備好物件存資料
                        WorkOrder nwk = new WorkOrder()
                        {
                            FabricPanelCode = fabricPanelCode,
                            ID = this.CuttingPOID,
                            FactoryID = this.FactoryID,
                            MDivisionId = this.MDivisionid,
                            Colorid = tmpColorid,
                            Tone = tmpTone,
                            Markername = markername,
                            IsCreateByUser = true,

                        };
                        int idxSize = 3;
                        int totalLayer = 0;
                        int garmentCnt = 0;
                        decimal consPc = 0;
                        decimal layerYDS = 0;
                        decimal layerInch = 0;
                        string importPatternPanel = string.Empty;
                        string markerLength = string.Empty;

                        Dictionary<string, int> dicSizeRatio = new Dictionary<string, int>();

                        // 橫向往右找
                        while (true)
                        {
                            // 找Size，C 5為起點，開始往右
                            string size = rangeSizeRatio.GetCellValue(idxSize, 2);
                            string totalSize = rangeSizeRatio.GetCellValue(idxSize, 1);

                            // 最後面是報表加總了(AB 4)，因此Break
                            if (totalSize == "Total Qty.")
                            {
                                // AB 10
                                totalLayer = MyUtility.Convert.GetInt(rangeSizeRatio.GetCellValue(idxSize, idxMarker));

                                // AD 10
                                layerYDS = MyUtility.Convert.GetDecimal(rangeSizeRatio.GetCellValue(idxSize + 2, idxMarker));

                                // AE 10
                                layerInch = MyUtility.Convert.GetDecimal(rangeSizeRatio.GetCellValue(idxSize + 3, idxMarker));

                                // 計算剩餘英吋數、碼等等
                                decimal inchDecimalPart = layerInch - Math.Floor(layerInch);
                                string inchFraction = inchDecimalPart == 0 ? "0/0" : Prg.ProjExts.DecimalToFraction(inchDecimalPart);
                                markerLength = $"{layerYDS}Y{Math.Floor(layerInch).ToString().PadLeft(2, '0')}-{inchFraction}+2";
                                layerYDS += layerInch * this.inchToYdsRate;
                                break;
                            }

                            // 找Size對應的數量，C 10 開始往右找
                            int sizeQty = MyUtility.Convert.GetInt(rangeSizeRatio.GetCellValue(idxSize, idxMarker));
                            if (sizeQty == 0)
                            {
                                idxSize++;
                                continue;
                            }

                            // 存下結果
                            dicSizeRatio.Add(size, sizeQty);
                            idxSize++;
                        }

                        if (dicSizeRatio.Count == 0 || totalLayer == 0)
                        {
                            continue;
                        }

                        // 開始找Pattern Panel，起點A 10
                        importPatternPanel = rangeSizeRatio.GetCellValue(1, idxMarker);

                        // 計算這個Pattern Panel的所有尺寸總和
                        garmentCnt = dicSizeRatio.Sum(s => s.Value);

                        // WorkOrder.ConsPC = 每層Yds / 每層SizeRatio
                        consPc = garmentCnt == 0 ? 0 : layerYDS / garmentCnt;

                        nwk.ConsPC = consPc;

                        // 這個層數不是最後的結果，等等還會去檢查 Order_ColorCombo.CuttingLayer設定值
                        nwk.ExcelLayer = totalLayer;
                        nwk.ImportPatternPanel = importPatternPanel; // FA
                        nwk.SizeRatio = dicSizeRatio; // {Size = 42，Qty = 10}、{Size = 46，Qty = 350}...

                        //int layer = Excel的TotalLayer > Construction.CuttingLayer ? Construction.CuttingLayer : Excel的TotalLayer;
                        // Cons = garmentCnt * consPc Layer
                        nwk.Cons = garmentCnt * consPc; // * wk.Layer;
                        //string markername = "MK_" + this.markerSerNo.ToString().PadLeft(3, '0');
                        //wk.Markername = markername;
                        nwk.MarkerLength = markerLength;
                        nwk.MarkerNo = "001";
                        nwk.MarkerVersion = "-1";

                        workOrders.Add(nwk);
                    }

                    // 當前區域的X Y 範圍已經處理完畢，設定下一個起點
                    curRowIndex = nextFabPanelCodeStart;
                }
            }

            return workOrders;
        }

        /// <summary>
        /// 根據Excel的POID，FabricPanelCode，ColorID，取得該部位的裁剪層數上限(Order_ColorCombo.CuttingLayer)，但我們不知道他們要裁的料號是誰，所以只能取TOP 1，再傳回去比對Excel上的資料
        /// </summary>
        /// <param name="poIDs">POID</param>
        /// <param name="fabricPanelCodes">Excel的FabricPanelCode</param>
        /// <param name="colorIDs">Excel的ColorID</param>
        /// <returns>組合好的物件</returns>
        private List<WorkOrder> GetOrder_ColorComboInfoList(List<string> poIDs, List<string> fabricPanelCodes, List<string> colorIDs)
        {
            WorkOrder wk = new WorkOrder();
            string sqlGetWorkOrderInfo = $@"
WITH RankedOrders AS (
    select  oc.ID,
	    oc.FabricPanelCode,
	    oc.ColorID,
	    FabricCombo = oc.PatternPanel,
	    oc.FabricCode,
	    ob.SCIRefno,
	    ob.Refno,
	    psd.SEQ1,
	    psd.SEQ2,
	    [CuttingLayer] = iif(isnull(c.CuttingLayer, 100) = 0, 100, isnull(c.CuttingLayer, 100)),
	    ROW_NUMBER() OVER (PARTITION BY oc.ID, oc.FabricPanelCode, oc.ColorID ORDER BY oc.ID) AS rn
    from Order_ColorCombo oc with (nolock)
    inner join Order_BOF ob with (nolock) on ob.Id = oc.id and ob.FabricCode = oc.FabricCode
    inner join PO_Supp_Detail psd with (nolock) on psd.ID = oc.Id and psd.SCIRefno = ob.SCIRefno
    inner join Fabric f with(nolock) ON ob.SCIRefno=f.SCIRefno
    left join Construction c on c.Id = f.ConstructionID and c.Junk = 0
    where oc.ID IN ('{poIDs.JoinToString("','")}' )
    and oc.FabricPanelCode IN ('{fabricPanelCodes.JoinToString("','")}' ) 
    and oc.ColorID IN ('{colorIDs.JoinToString("','")}' ) 
)

SELECT *
FROM RankedOrders
WHERE rn = 1
";
            DataTable drWorkOrderInfo;
            DualResult r = DBProxy.Current.Select(null, sqlGetWorkOrderInfo, out drWorkOrderInfo);

            if (!r)
            {
                throw r.GetException();
            }

            var wks = DataTableToList.ConvertToClassList<WorkOrder>(drWorkOrderInfo);

            if (wks.Any())
            {
                return wks.ToList();
            }
            else
            {
                return new List<WorkOrder>();
            }
        }

        /// <summary>
        /// Insert WorkOrder、WorkOrder_PatternPanel、WorkOrder_SizeRatio三張資料表
        /// </summary>
        /// <param name="workOrders">組合好的物件</param>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <returns>新的WorkOrder的Ukey集合</returns>
        private List<long> InsertWorkOrder(List<WorkOrder> workOrders, SqlConnection sqlConnection)
        {
            List<long> listWorkOrderUkey = new List<long>();

            foreach (var wk in workOrders)
            {
                var excelLayer = wk.Layer;
                var cuttingLayer = wk.CuttingLayer;

                // WorkOrder_PatternPanel
                string sqlInsertWorkOrder_PatternPanel = string.Empty;
                foreach (var itemPatternPanel in wk.ImportPatternPanel.Split('+'))
                {
                    if (itemPatternPanel.Length != 2)
                    {
                        continue;
                    }

                    string patternPanel = itemPatternPanel;
                    string fabricPanelCode = itemPatternPanel[1].ToString();

                    sqlInsertWorkOrder_PatternPanel += $@"
insert into {this.TableName}_PatternPanel(ID, {this.TableMainKeyColName}, PatternPanel, FabricPanelCode) 
values('{this.CuttingPOID}', @newWorkOrderUkey, '{patternPanel}', '{fabricPanelCode}')";
                }

                // WorkOrder_SizeRatio
                string sqlInsertWorkOrder_SizeRatio = string.Empty;
                foreach (KeyValuePair<string, int> itemSizeRatio in wk.SizeRatio)
                {
                    sqlInsertWorkOrder_SizeRatio += $@"
insert into {this.TableName}_SizeRatio ({this.TableMainKeyColName}, ID, SizeCode, Qty)
values( @newWorkOrderUkey, '{this.CuttingPOID}', '{itemSizeRatio.Key}', '{itemSizeRatio.Value}')
";
                }

                while (true)
                {
                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@consPc", wk.ConsPC),
                        new SqlParameter("@Cons",  wk.Cons),
                    };
                    string sqlInsertWorkOrder = $@"
insert into {this.TableName}(
ID
,FactoryID
,MDivisionId
,SEQ1
,SEQ2
,Layer
,Colorid
,Markername
,MarkerLength
,ConsPC
,Cons
,Refno
,SCIRefno
,MarkerNo
,AddName
,AddDate
,FabricCombo
,FabricCode
,FabricPanelCode
,Order_EachconsUkey
,Tone
,OrderID
,IsCreateByUser 
)
values
(
'{wk.ID}'
,'{wk.FactoryID}'
,'{wk.MDivisionId}'
,'{wk.SEQ1}'
,'{wk.SEQ2}'
,'{wk.Layer}'
,'{wk.Colorid}'
,'{wk.Markername}'
,'{wk.MarkerLength}' --MarkerLength
,@consPc --ConsPC
,@Cons --Cons
,'{wk.Refno}'
,'{wk.SCIRefno}'
,'001'
,'{Env.User.UserID}'
,getdate()
,'{wk.FabricCombo}'
,'{wk.FabricCode}'
,'{wk.FabricPanelCode}'
,-1
,'{wk.Tone}'
,'{wk.ID}'
,1
)

DECLARE @newWorkOrderUkey as bigint = (select @@IDENTITY)
{sqlInsertWorkOrder_PatternPanel}

{sqlInsertWorkOrder_SizeRatio}

select @newWorkOrderUkey
";
                    DualResult result = DBProxy.Current.SelectByConn(sqlConnection, sqlInsertWorkOrder, sqlParameters, out DataTable dtResult);
                    if (!result)
                    {
                        throw result.GetException();
                    }

                    long workOrderUkey = MyUtility.Convert.GetLong(dtResult.Rows[0][0]);
                    listWorkOrderUkey.Add(workOrderUkey);

                    // 層數如果沒超過上限的資料，便跳出去
                    if (excelLayer < cuttingLayer)
                    {
                        break;
                    }

                    // 超過上限，則拆多筆寫入
                    excelLayer -= cuttingLayer;
                }
            }

            return listWorkOrderUkey;
        }

        /// <summary>
        /// Insert WorkOrder_Distribute資料表
        /// </summary>
        /// <param name="listWorkOrderUkey">新的WorkOrder的Ukey集合</param>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <returns>結果</returns>
        private DualResult InsertWorkOrder_Distribute(List<long> listWorkOrderUkey, SqlConnection sqlConnection)
        {
            string whereWorkOrderUkey = listWorkOrderUkey.Select(s => s.ToString()).JoinToString(",");
            string sqlInsertWorkOrder_Distribute = $@"
select w.Ukey, w.Colorid, w.FabricCombo, ws.SizeCode, [CutQty] = isnull(ws.Qty * w.Layer, 0)
into #tmpCutting
from {this.TableName} w with (nolock)
inner join {this.TableName}_SizeRatio ws with (nolock) on ws.{this.TableMainKeyColName} = w.Ukey
where w.Ukey in ({whereWorkOrderUkey})
order by ukey

select * from #tmpCutting


select oq.ID, oq.Article, oq.SizeCode, [Qty] = Cast(isnull(oq.Qty, 0) as int), oc.ColorID, oc.PatternPanel, o.SewInLine
from Orders o with (nolock)
inner join Order_Qty oq with (nolock) on oq.ID = o.ID
inner join Order_ColorCombo oc with (nolock) on oc.Id = o.POID and oc.Article = oq.Article
where   o.POID = '{this.CuttingPOID}'
order by o.SewInLine, oq.ID


";
            DualResult result = new DualResult(true);
            DataTable[] dtResults;
            result = DBProxy.Current.SelectByConn(sqlConnection, sqlInsertWorkOrder_Distribute, out dtResults);
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
insert into {this.TableName}_Distribute({this.TableMainKeyColName}, ID, OrderID, Article, SizeCode, Qty)
values({itemDistribute["Ukey"]}, '{this.CuttingPOID}', '{drDistributeOrderQty["ID"]}', '{drDistributeOrderQty["Article"]}', '{itemDistribute["SizeCode"]}', '{distributrQty}')
";
                }

                // 如果還有未分配就insert EXCESS
                if (MyUtility.Convert.GetInt(itemDistribute["CutQty"]) > 0)
                {
                    sqlInsertWorkOrderDistribute += $@"
insert into {this.TableName}_Distribute({this.TableMainKeyColName}, ID, OrderID, Article, SizeCode, Qty)
values({itemDistribute["Ukey"]}, '{this.CuttingPOID}', 'EXCESS', '', '{itemDistribute["SizeCode"]}', '{itemDistribute["CutQty"]}')
";
                }
            }

            if (!MyUtility.Check.Empty(sqlInsertWorkOrderDistribute))
            {
                result = DBProxy.Current.ExecuteByConn(sqlConnection, sqlInsertWorkOrderDistribute);
            }

            return result;
        }

        /// <summary>
        /// 根據WorkOrder資料表建立的類別
        /// </summary>
        public class WorkOrder
        {
            /// <summary>
            /// 裁剪母單單號
            /// </summary>
            public string ID { get; set; } = string.Empty;
            public string FactoryID { get; set; } = string.Empty;
            public string MDivisionId { get; set; } = string.Empty;
            public string SEQ1 { get; set; } = string.Empty;
            public string SEQ2 { get; set; } = string.Empty;
            public string CutRef { get; set; } = string.Empty;
            public string OrderID { get; set; } = string.Empty;
            public string CutplanID { get; set; } = string.Empty;
            public decimal? Cutno { get; set; }

            /// <summary>
            /// 預計要裁幾層
            /// </summary>
            public decimal Layer { get; set; } = 0;

            /// <summary>
            /// 該布種的層數上限
            /// </summary>
            public decimal CuttingLayer { get; set; } = 0;
            /// <summary>
            /// Excel所填的Layer值
            /// </summary>
            public decimal ExcelLayer { get; set; } = 0;
            public string Colorid { get; set; } = string.Empty;
            public string Markername { get; set; } = string.Empty;
            public DateTime? EstCutDate { get; set; }
            public string CutCellid { get; set; } = string.Empty;
            public string MarkerLength { get; set; } = string.Empty;
            public decimal ConsPC { get; set; } = 0;
            public decimal Cons { get; set; } = 0;
            public string Refno { get; set; } = string.Empty;
            public string SCIRefno { get; set; } = string.Empty;
            public string MarkerNo { get; set; } = string.Empty;
            public string MarkerVersion { get; set; } = string.Empty;
            public long Ukey { get; set; } // 由於這是IDENTITY，因此不需要設定預設值
            public string Type { get; set; } = string.Empty;
            public string AddName { get; set; } = string.Empty;
            public DateTime? AddDate { get; set; }
            public string EditName { get; set; } = string.Empty;
            public DateTime? EditDate { get; set; }
            public string FabricCombo { get; set; } = string.Empty;
            public string MarkerDownLoadId { get; set; } = string.Empty;
            public string FabricCode { get; set; } = string.Empty;
            public string FabricPanelCode { get; set; } = string.Empty;
            public long Order_EachconsUkey { get; set; } = 0;
            public string OldFabricUkey { get; set; } = string.Empty;
            public string OldFabricVer { get; set; } = string.Empty;
            public string ActCuttingPerimeter { get; set; } = string.Empty;
            public string StraightLength { get; set; } = string.Empty;
            public string CurvedLength { get; set; } = string.Empty;
            public string SpreadingNoID { get; set; } = string.Empty;
            public string Shift { get; set; } = string.Empty;
            public DateTime? WKETA { get; set; }
            public string UnfinishedCuttingReason { get; set; } = string.Empty;
            public string Tone { get; set; } = string.Empty;
            public string Remark { get; set; } = string.Empty;
            public string CutRef_Old { get; set; } = string.Empty;
            public bool IsCreateByUser { get; set; } = false;
            public string ImportPatternPanel { get; set; } = string.Empty;
            public Dictionary<string, int> SizeRatio { get; set; }
        }

        #endregion
    }
}

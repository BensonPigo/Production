using Ict;
using Ict.Win.UI;
using Sci.Andy;
using Sci.Andy.ExtensionMethods;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using static Sci.Production.Cutting.CuttingWorkOrder;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1602 // Enumeration items should be documented
#pragma warning disable SA1204 // Static elements should appear before instance elements
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

        #region Import & 分配 Distribute

        private string CuttingPOID = string.Empty;
        private string MDivisionid = string.Empty;
        private string FactoryID = string.Empty;

        /// <summary>
        /// 碼、英吋換算比例
        /// </summary>
        private decimal inchToYdsRate = 0;

        /// <summary>
        /// 匯入指定格式的Excel，寫入資料庫WorkOrder、SizeRatio、PatternPanel、Distribute四張資料表的資料 (P02沒有Distribute)
        /// </summary>
        /// <param name="cuttingID">Cutting.ID</param>
        /// <param name="mDivisionid">mDivisionid</param>
        /// <param name="factoryID">factoryID</param>
        /// <param name="form">Import的目標功能(P02或P09)</param>
        /// <returns>Result</returns>
        public DualResult ImportMarkerExcel(string cuttingID, string mDivisionid, string factoryID, CuttingForm form)
        {
            this.CuttingPOID = cuttingID;
            this.MDivisionid = mDivisionid;
            this.FactoryID = factoryID;

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

            try
            {
                // 取得Excel所有 WorkOrder 資料
                List<WorkOrder> excelWk = this.LoadExcel(filename);

                if (!excelWk.Any())
                {
                    MyUtility.Msg.InfoBox($"Excel not contain Cutting SP<{cuttingID}>");
                    return new DualResult(true, "NotImport");
                }

                // Excel當中合法資料，可寫入DB
                List<WorkOrder> validWk = new List<WorkOrder>();

                // Excel當中非法資料，不寫入DB，用於顯示錯誤訊息
                List<WorkOrder> inValidWk = new List<WorkOrder>();

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

                    // 檢查Excel 資料合法性，撈一次資料即可
                    List<WorkOrder> dbData = this.GetOrderInfoList(poids);
                    List<WorkOrder> dbDataMarkerNo = this.GetOrder_EachConsInfoList(poids);

                    foreach (var workOrder in excelWk)
                    {
                        // FabricPanelCode、ImportPatternPanel可能會有 A+B、FA+FB這種資料，所以要拆開來判斷
                        var wks = this.TransWorkOrder(workOrder);

                        List<WorkOrder> compareOkData = new List<WorkOrder>();
                        bool isFail = false;
                        foreach (var item in wks)
                        {
                            if (!dbData.Any(x => x.FabricPanelCode == item.FabricPanelCode && x.FabricCombo == item.ImportPatternPanel))
                            {
                                inValidWk.Add(workOrder);
                                isFail = true;
                            }
                            else
                            {
                                compareOkData.Add(dbData.Where(x => x.FabricPanelCode == item.FabricPanelCode && x.FabricCombo == item.ImportPatternPanel).FirstOrDefault());
                            }
                        }

                        if (isFail)
                        {
                            continue;
                        }

                        workOrder.Refno = compareOkData.FirstOrDefault().Refno;
                        workOrder.SCIRefno = compareOkData.FirstOrDefault().SCIRefno;
                        workOrder.FabricCode = compareOkData.FirstOrDefault().FabricCode;
                        if (!dbDataMarkerNo.Any(o => o.MarkerNo == workOrder.MarkerNo))
                        {
                            workOrder.MarkerNo = string.Empty;
                        }

                        // 比較Excel填的ColorCombo設定的層數，看Excel填的有沒有超過上限
                        var layer = workOrder.ExcelLayer;
                        workOrder.Layer = layer;
                        workOrder.CuttingLayer = layer;
                        workOrder.Cons = workOrder.Cons * layer; // 從 DB 取得 Layer 乘上

                        validWk.Add(workOrder);
                    }

                    // 只要有不合法就全部不寫入
                    if (!inValidWk.Any() && validWk.Any())
                    {
                        // 合法資料開始寫入 WorkOrder、WorkOrder_PatternPanel、WorkOrder_SizeRati
                        List<long> newWorkOrderUkey = this.InsertWorkOrder(validWk, sqlConn, form);
                        if (!newWorkOrderUkey.Any())
                        {
                            return new DualResult(true, $"Insert data Fail.");
                        }

                        // 開始Distribute，P02則自動跳過
                        this.InsertWorkOrder_Distribute(this.CuttingPOID, newWorkOrderUkey, sqlConn, form);
                    }

                    transactionScope.Complete();
                }

                if (inValidWk.Any())
                {
                    DataTable dt = ListToDataTable.ToDataTable(inValidWk.Select(o => new { o.ID, o.SEQ1, o.SEQ2, o.Markername, o.FabricPanelCode }).Distinct().ToList());
                    var f = new MsgGridForm(dt, "Import file info. error", $"Invalid data");
                    f.grid1.ColumnsAutoSize();
                    f.ShowDialog();
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
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                workbook = excel.Workbooks.Open(MyUtility.Convert.GetString(filename));
                int sheetCnt = excel.ActiveWorkbook.Worksheets.Count;

                for (int i = 1; i <= sheetCnt; i++)
                {
                    worksheet = workbook.Worksheets[i];

                    // 檢查裁剪母單單號
                    string poID = worksheet.GetCellValue(2, 2);
                    if (poID != this.CuttingPOID)
                    {
                        Marshal.ReleaseComObject(worksheet);
                        continue;
                    }

                    // 開始檢查Excel
                    string keyWord_FabPanelCode = "Panel Code:";
                    string keyWord_Layer = "Layers";
                    string keyWord_MarkerEnd = "No.";

                    // 紀錄這一區的起點Index，起點是「Panel Code:」欄位
                    int rowIndexPanelCode = 1;

                    // 紀錄這一區的終點Index
                    int rowIndexTerminalPoint = 1;

                    // 紀錄資料有幾Column，到哪一個Index，整個Sheet都是一致的
                    int colIndexLayers = 0;

                    // 記錄有多少Row的暫存變數
                    int emptyRowCount = 0;

                    // 橫向開始找「Layers」欄位(對應 範本Excel的col = AB, Row = 8 )，但是User可能自己複製column，所以動態抓，整個Sheet的Column是一起的，所以一張Sheet找一次就好
                    if (colIndexLayers == 0)
                    {
                        while (worksheet.GetCellValue(colIndexLayers + 1, 8) != keyWord_Layer)
                        {
                            colIndexLayers++;
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
                        if (worksheet.GetCellValue(1, rowIndexPanelCode) != keyWord_FabPanelCode)
                        {
                            rowIndexPanelCode++;
                            emptyRowCount++;
                            continue;
                        }

                        // 找到「Panel Code:」欄位，重置
                        emptyRowCount = 0;
                        rowIndexTerminalPoint = 1;

                        // 若找到，「Panel Code:」的行數為 indexPanelCode
                        // 計算「Panel Code:」到下一個「No.」距離多少Row，User填了幾Row的資料，超 20 Row都找不到就算了(報表範本有鎖格式，所以應該不可能找不到)
                        // AA 25
                        rowIndexTerminalPoint += rowIndexPanelCode;
                        string text = worksheet.GetCellValue(1, rowIndexTerminalPoint);
                        while (text != keyWord_MarkerEnd && emptyRowCount < 20)
                        {
                            rowIndexTerminalPoint++;
                            emptyRowCount++;
                            text = worksheet.GetCellValue(1, rowIndexTerminalPoint);
                            continue;
                        }

                        // 找到下一個「No.」，往上推兩格是終點
                        rowIndexTerminalPoint -= 2;

                        emptyRowCount = 0;

                        // 下一個「Panel Code:」的起點，前一個終點的下3格
                        int nextFabPanelCodeStart = rowIndexTerminalPoint + 3;

                        // 計算Pattern Panel和Marker Name填寫的Row有幾行
                        // 6 = 「Panel Code:」到「MK Name」的距離
                        // 1 = 「Ttl. Qty.」跟最後一筆資料的距離
                        int markerRowCount = rowIndexTerminalPoint - rowIndexPanelCode - 6;

                        // 取得「Panel Code:」的值(對應 Excel的col = B, Row = 3 )，「Panel Code:」在「Panel Code:」的下一行，「Panel Code:」的行數為 indexPanelCode
                        string fabricPanelCode = worksheet.GetCellValue(2, rowIndexPanelCode);

                        // 如果「Panel Code:」找不到，則跳到下一個「Panel Code:」的起點
                        if (MyUtility.Check.Empty(fabricPanelCode))
                        {
                            throw new Exception("Panel Code can't be empty.");
                        }

                        // 取MarkerNo, Seq，在「Panel Code:」的上一Row
                        string markerNo = worksheet.GetCellValue(6, rowIndexPanelCode - 1);
                        string seq = worksheet.GetCellValue(11, rowIndexPanelCode - 1);
                        string seq1, seq2;

                        if (seq.Split('-').Length < 2)
                        {
                            seq1 = string.Empty;
                            seq2 = string.Empty;
                        }
                        else
                        {
                            seq1 = seq.Split('-')[0];
                            seq2 = seq.Split('-')[1];
                        }

                        // 取得「Color:」的值((對應 Excel的col = B, Row = 5 )
                        string[] colorInfo = worksheet.GetCellValue(2, rowIndexPanelCode + 1).Split('-');

                        string tmpColorid = colorInfo.Length >= 1 ? colorInfo[0] : string.Empty;
                        string tmpTone = colorInfo.Length >= 2 ? colorInfo[1] : string.Empty;

                        // 取得Size Ratio Range (例如：1 ,4 ,28 ,24 )
                        Excel.Range rangeSizeRatio = worksheet.GetRange(1, rowIndexPanelCode, colIndexLayers + 1, nextFabPanelCodeStart - 3);

                        // 讀每一個MkName，起點是從A4「Panel Code:」往下數，所以是 = 7；終點是下筆資料「Panel Code:」的Y值 - 3，
                        // 直向往下找
                        for (int idxMarker = 7; idxMarker < 7 + markerRowCount; idxMarker++)
                        {
                            // 若PatternPanel為空，這一列則跳過
                            string checkPatternPanel = rangeSizeRatio.GetCellValue(1, idxMarker);
                            if (string.IsNullOrEmpty(checkPatternPanel))
                            {
                                continue;
                            }

                            // 準備好物件存資料
                            WorkOrder nwk = new WorkOrder()
                            {
                                FabricPanelCode = fabricPanelCode.Trim(),
                                SEQ1 = seq1.Trim(),
                                SEQ2 = seq2.Trim(),
                                MarkerNo = markerNo.Trim(),
                                ID = this.CuttingPOID,
                                FactoryID = this.FactoryID,
                                MDivisionId = this.MDivisionid,
                                Colorid = tmpColorid.Trim(),
                                Tone = tmpTone.Trim(),
                                IsCreateByUser = true,
                            };

                            int idxSize = 3;
                            int totalLayer = 0;
                            int garmentCnt = 0;
                            decimal consPc = 0;
                            decimal layerYDS = 0;
                            decimal layerInch = 0;
                            string importPatternPanel = string.Empty;
                            string markerName = string.Empty;
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
                                    string inchFraction = Prg.ProjExts.ConvertToFractionString(inchDecimalPart);
                                    markerLength = $@"{layerYDS.ToString("00")}Y{Math.Floor(layerInch).ToString().PadLeft(2, '0')}-{inchFraction}+1""";
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

                            // 開始找 Marker Name，起點B 10
                            markerName = rangeSizeRatio.GetCellValue(2, idxMarker);

                            // 計算這個Pattern Panel的所有尺寸總和
                            garmentCnt = dicSizeRatio.Sum(s => s.Value);

                            // WorkOrder.ConsPC = 每層Yds / 每層SizeRatio
                            consPc = garmentCnt == 0 ? 0 : layerYDS / garmentCnt;

                            nwk.ConsPC = consPc;

                            // 這個層數不是最後的結果，等等還會去檢查 Order_ColorCombo.CuttingLayer設定值
                            nwk.ExcelLayer = totalLayer;
                            nwk.ImportPatternPanel = importPatternPanel; // FA
                            nwk.SizeRatio = dicSizeRatio; // {Size = 42，Qty = 10}、{Size = 46，Qty = 350}...

                            nwk.Cons = garmentCnt * consPc; // * wk.Layer; 這邊先不撈DB Layer 資訊, 之後才撈取 * Layer
                            nwk.MarkerLength = markerLength;
                            nwk.Markername = markerName;
                            nwk.MarkerVersion = "-1";
                            workOrders.Add(nwk);
                        }

                        // 當前區域的X Y 範圍已經處理完畢，設定下一個起點
                        rowIndexPanelCode = nextFabPanelCodeStart;
                    }

                    Marshal.ReleaseComObject(worksheet);
                    worksheet = null;
                }

                workbook.Close(false);
                Marshal.ReleaseComObject(workbook);
                workbook = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                excel.Quit();
                MyUtility.Excel.KillExcelProcess(excel);
            }

            return workOrders;
        }

        /// <summary>
        /// WorkOrder拆分作業，根據FabricPanelCode、PatternPanel拆分
        /// </summary>
        /// <param name="nwk">Excel讀出來的WorkOrder</param>
        /// <returns>WorkOrder物件集合</returns>
        public List<WorkOrder> TransWorkOrder(WorkOrder nwk)
        {
            List<WorkOrder> result = new List<WorkOrder>();

            // 拆分 FabricPanelCode 和 ImportPatternPanel
            string[] fabricPanels = nwk.FabricPanelCode.Split('+');
            string[] importPanels = nwk.ImportPatternPanel.Split('+');

            int maxLength = Math.Max(fabricPanels.Length, importPanels.Length);

            for (int i = 0; i < maxLength; i++)
            {
                // 淺複製 WorkOrder 並修改 FabricPanelCode 和 ImportPatternPanel
                WorkOrder newWorkOrder = (WorkOrder)nwk.ShallowCopy();

                newWorkOrder.FabricPanelCode = i < fabricPanels.Length ? fabricPanels[i] : string.Empty;
                newWorkOrder.ImportPatternPanel = i < importPanels.Length ? importPanels[i] : string.Empty;

                result.Add(newWorkOrder);
            }

            return result;
        }

        /// <summary>
        /// 根據Excel的POID，取得Seq、Refno、Color、Refno等欄位清單，包含裁剪層數上限(Construction.CuttingLayer)
        /// </summary>
        /// <param name="poIDs">POID</param>
        /// <returns>組合好的物件</returns>
        private List<WorkOrder> GetOrderInfoList(List<string> poIDs)
        {
            string sqlGetWorkOrderInfo = $@"
SELECT  
     psd.ID
    ,psd.SEQ1
    ,psd.SEQ2
    ,psd.Refno
    ,psd.SCIRefno
    ,ColorID = LTRIM(RTRIM(ISNULL(psdc.SpecValue, '')))
    ,CuttingLayer = iif(isnull(c.CuttingLayer, 100) = 0, 100, isnull(c.CuttingLayer, 100))
    ,ofc.FabricPanelCode
    ,FabricCombo = ofc.PatternPanel
    ,ofc.FabricCode
FROM PO_Supp_Detail psd WITH (NOLOCK)
INNER JOIN PO_Supp_Detail_Spec psdc WITH (NOLOCK) ON psdc.ID = psd.id AND psdc.seq1 = psd.seq1 AND psdc.seq2 = psd.seq2 AND psdc.SpecColumnID = 'Color'
INNER JOIN Fabric f WITH (NOLOCK) ON f.SCIRefno = psd.SCIRefno
INNER JOIN Order_FabricCode ofc WITH (NOLOCK) on ofc.Id = psd.ID
LEFT JOIN Construction c WITH (NOLOCK) on c.Id = f.ConstructionID and c.Junk = 0
WHERE psd.ID IN ('{poIDs.JoinToString("','")}' )
AND psd.Junk = 0
AND EXISTS (
    SELECT 1
    FROM Order_BOF WITH (NOLOCK)
    INNER JOIN Fabric WITH (NOLOCK) ON Fabric.SCIRefno = Order_BOF.SCIRefno
    WHERE Order_BOF.FabricCode = ofc.FabricCode
    AND Order_BOF.Id = psd.ID
    AND Fabric.BrandRefNo = f.BrandRefNo
)
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
        /// 根據Excel的POID，取得MarkerNo
        /// </summary>
        /// <param name="poIDs">POID</param>
        /// <returns>組合好的物件</returns>
        private List<WorkOrder> GetOrder_EachConsInfoList(List<string> poIDs)
        {
            WorkOrder wk = new WorkOrder();
            string sqlGetEachCons = $@"
SELECT DISTINCT oec.ID, oec.MarkerNo
FROM Order_EachCons oec WITH(NOLOCK)
INNER JOIN Orders o WITH(NOLOCK) ON o.ID = oec.ID
WHERE o.POID IN ('{poIDs.JoinToString("','")}' )
";
            DataTable dt;
            DualResult r = DBProxy.Current.Select(null, sqlGetEachCons, out dt);

            if (!r)
            {
                throw r.GetException();
            }

            var wks = DataTableToList.ConvertToClassList<WorkOrder>(dt);

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
        private List<long> InsertWorkOrder(List<WorkOrder> workOrders, SqlConnection sqlConnection, CuttingForm form)
        {
            List<long> listWorkOrderUkey = new List<long>();
            string tableName = GetTableName(form);
            string keyColumn = GetWorkOrderUkeyName(form);

            foreach (var wk in workOrders)
            {
                var excelLayer = wk.Layer;
                var cuttingLayer = wk.CuttingLayer;

                // sheet 序號
                int markerSerNo = 1;

                // WorkOrder_PatternPanel
                string sqlInsertWorkOrder_PatternPanel = string.Empty;

                for (int i = 0; i < wk.ImportPatternPanel.Split('+').Length; i++)
                {
                    string patternPanel = wk.ImportPatternPanel.Split('+')[i];
                    string fabricPanelCode = wk.FabricPanelCode.Split('+')[i];

                    // PatternPanel、FabricPanelCode檢驗方式同表身
                    sqlInsertWorkOrder_PatternPanel += $@"
IF EXISTS(
	SELECT 1
	FROM Order_FabricCode WITH(NOLOCK)
	WHERE ID = '{this.CuttingPOID}'
	AND PatternPanel = '{patternPanel}'
	AND FabricPanelCode = '{fabricPanelCode}'
)
BEGIN
    insert into {tableName}_PatternPanel(ID, {keyColumn}, PatternPanel, FabricPanelCode) 
    values('{this.CuttingPOID}', @newWorkOrderUkey, '{patternPanel}', '{fabricPanelCode}')
END
";
                }

                wk.FabricCombo = wk.ImportPatternPanel.Split('+')[0];
                wk.FabricPanelCode = wk.FabricPanelCode.Split('+')[0];

                // WorkOrder_SizeRatio
                string sqlInsertWorkOrder_SizeRatio = string.Empty;
                foreach (KeyValuePair<string, int> itemSizeRatio in wk.SizeRatio)
                {
                    // SizeCode 檢驗方式同表身
                    sqlInsertWorkOrder_SizeRatio += $@"
IF EXISTS(
	SELECT 1
	FROM Order_Qty oq WITH (NOLOCK)
	INNER JOIN Orders o WITH (NOLOCK) ON o.id = oq.id
	WHERE o.CuttingSP = '{this.CuttingPOID}' AND oq.SizeCode = '{itemSizeRatio.Key}'
)
BEGIN
    insert into {tableName}_SizeRatio ({keyColumn}, ID, SizeCode, Qty)
    values( @newWorkOrderUkey, '{this.CuttingPOID}', '{itemSizeRatio.Key}', '{itemSizeRatio.Value}')
END
";
                }

                while (true)
                {
                    string markername = "MK-" + markerSerNo.ToString();

                    // 若能轉成int，代表是excel自動產生的Marker Name，因此套用編碼規則；不能轉代表是User自己手Key的，就不異動了
                    if (int.TryParse(wk.Markername, out int x))
                    {
                        wk.Markername = "MK-" + wk.Markername.ToString();
                    }

                    markerSerNo++;

                    string sqlInsertWorkOrder = $@"
declare @consPc as numeric(12, 4) = '{Math.Round(wk.ConsPC, 4)}'
declare @Cons as numeric(16, 4) = '{Math.Round(wk.Cons, 4)}'

insert into {tableName}(
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
,'{wk.MarkerNo}'
,'{Env.User.UserID}'
,getdate()
,'{wk.FabricCombo}'
,'{wk.FabricCode}'
,'{wk.FabricPanelCode}'
,0
,'{wk.Tone}'
,'{wk.ID}'
,1
)

DECLARE @newWorkOrderUkey as bigint = (select @@IDENTITY)
{sqlInsertWorkOrder_PatternPanel}

{sqlInsertWorkOrder_SizeRatio}

select @newWorkOrderUkey
";
                    DualResult result = DBProxy.Current.SelectByConn(sqlConnection, sqlInsertWorkOrder, out DataTable dtResult);
                    if (!result)
                    {
                        throw result.GetException();
                    }

                    long workOrderUkey = MyUtility.Convert.GetLong(dtResult.Rows[0][0]);
                    listWorkOrderUkey.Add(workOrderUkey);

                    // 層數如果沒超過上限的資料，便跳出去
                    if (excelLayer <= cuttingLayer)
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
        /// 分配 Distribute,並 Insert WorkOrder_Distribute 資料表
        /// </summary>
        /// <param name="id">Cutting.ID</param>
        /// <param name="listWorkOrderUkey">listWorkOrderUkey</param>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <param name="form">P02/P09</param>
        /// <returns>DualResult</returns>
        public DualResult InsertWorkOrder_Distribute(string id, List<long> listWorkOrderUkey, SqlConnection sqlConnection, CuttingForm form)
        {
            string tableName = GetTableName(form);
            string ukeyName = GetWorkOrderUkeyName(form);
            string whereWorkOrderUkey = listWorkOrderUkey.Select(s => s.ToString()).JoinToString(",");
            string sqlInsertWorkOrder_Distribute = $@"
select w.Ukey, w.Colorid, w.FabricCombo, ws.SizeCode, [CutQty] = isnull(ws.Qty * w.Layer, 0)
into #tmpCutting
from {tableName} w with (nolock)
inner join {tableName}_SizeRatio ws with (nolock) on ws.{tableName}Ukey = w.Ukey
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
insert into {tableName}_Distribute({ukeyName}, ID, OrderID, Article, SizeCode, Qty)
values({itemDistribute["Ukey"]}, '{id}', '{drDistributeOrderQty["ID"]}', '{drDistributeOrderQty["Article"]}', '{itemDistribute["SizeCode"]}', '{distributrQty}')
";
                }

                // 如果還有未分配就insert EXCESS
                if (MyUtility.Convert.GetInt(itemDistribute["CutQty"]) > 0)
                {
                    sqlInsertWorkOrderDistribute += $@"
insert into {tableName}_Distribute({ukeyName}, ID, OrderID, Article, SizeCode, Qty)
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

        /// <summary>
        /// 根據WorkOrder資料表建立的類別
        /// </summary>
#pragma warning disable SA1516 // Elements should be separated by blank line
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

            public WorkOrder ShallowCopy()
            {
                return (WorkOrder)this.MemberwiseClone();
            }
        }
#pragma warning restore SA1516 // Elements should be separated by blank line
        #endregion

        #region Auto CutRef / Cutno

        /// <summary>
        /// 非編輯模式下,自動編碼CutRef，並Call API傳給廠商
        /// 有空的話把準備資料階段改成Linq, 大量資料編碼時傳到DB會卡卡
        /// </summary>
        /// <param name="cuttingID">Cutting.ID</param>
        /// <param name="mDivision">MDivision</param>
        /// <param name="detailDatas">表身</param>
        /// <param name="form">form</param>
        public static void AutoCutRef(string cuttingID, string mDivision, DataTable detailDatas, CuttingForm form)
        {
            DataTable dtWorkOrder = detailDatas.Copy();

            // 根據功能區分 TableName，Pkey Column Name
            string colName = string.Empty;
            string where = string.Empty;
            string cmdWhere = string.Empty;
            string outerApply = string.Empty;
            string nColumn = string.Empty;

            string tableName = GetTableName(form);
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
                    where = "And CanEdit = 1";
                    cmdWhere = "AND CutNo IS NOT NULL AND CutCellID <> ''";
                    nColumn = ", ws.SizeRatio";
                    outerApply = $@"
OUTER APPLY (
    SELECT STUFF((
        SELECT ',' + b.SizeCode + ':' + CAST(b.Qty AS VARCHAR)
        FROM (
            SELECT SizeCode, Qty 
            FROM {tableName}_SizeRatio ws 
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
    Layer = SUM(w.Layer),
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
                MyUtility.Msg.ErrorBox(cutRefresult.ToString());
                return;
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
                MyUtility.Msg.ErrorBox(cutRefresult.ToString());
                return;
            }
            #endregion

            #region 組合SQL：寫入空的CutRef

            string updateCutRef = $@"
CREATE TABLE #tmp{tableName} (Ukey BIGINT);
DECLARE @chk TINYINT = 0;
BEGIN TRANSACTION [Trans_Name];";

            foreach (DataRow dr in workordertmp.Rows)
            {
                string newCutRef = string.Empty;
                string maxref = Sci.Production.PublicPrg.Prgs.GetColumnValueNo(tableName, "CutRef");
                string spreadingStatus = "Ready";
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

                        // spreadingStatus 規則
                        int sameCutRefNewTotalLayer = findrow.AsEnumerable().Sum(row => MyUtility.Convert.GetInt(row["Layer"])) + MyUtility.Convert.GetInt(dr["Layer"]);
                        string sqlSpreadingLayers = $"SELECT SUM(SpreadingLayers) FROM WorkOrderForOutput_SpreadingFabric WITH(NOLOCK) WHERE CutRef = '{newCutRef}'";
                        int spreadingLayers = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlSpreadingLayers));

                        if (spreadingLayers == 0)
                        {
                            spreadingStatus = "Ready";
                        }
                        else if (sameCutRefNewTotalLayer > spreadingLayers)
                        {
                            spreadingStatus = "Spreading";
                        }
                        else
                        {
                            spreadingStatus = "Finished";
                        }
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

                string spreadingStatus_Col = form == CuttingForm.P02 ? string.Empty : $", SpreadingStatus = '{spreadingStatus}',LastCreateCutRefDate = GETDATE()";
                updateCutRef += $@"
    IF (SELECT COUNT(1) FROM {tableName} WITH (NOLOCK) WHERE CutRef = '{newCutRef}' AND id != '{cuttingID}') > 0
    BEGIN
        RAISERROR ('Duplicate CutRef. Please redo Auto Ref#', 12, 1);
        ROLLBACK TRANSACTION [Trans_Name];
    END
    UPDATE {tableName} SET CutRef = '{newCutRef}' {spreadingStatus_Col}
    OUTPUT INSERTED.Ukey INTO #tmp{tableName}
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
    FROM #tmp{tableName} tw
    INNER JOIN {tableName} w WITH (NOLOCK) ON tw.Ukey = w.Ukey;
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
                    return;
                }
                else
                {
                    MyUtility.Msg.ErrorBox(cutRefresult.ToString());
                    return;
                }
            }

            if (dtWorkorder.Rows.Count > 0 && form == CuttingForm.P09)
            {
                Task.Run(() => new Guozi_AGV().SentWorkOrderToAGV(dtWorkorder));
            }
        }

        /// <summary>
        /// 自動編碼 CutNo。當前行的 "CutNo" 是空的並且 "EstCutDate" 不為空，by "FabricCombo" 群組遞增
        /// </summary>
        /// <param name="detailDatas">表身</param>
        public static void AutoCut(IList<DataRow> detailDatas)
        {
            // 需要編碼的資料群
            var groupedData = detailDatas
                .Where(row => row["CutNo"] == DBNull.Value && !MyUtility.Check.Empty(row["EstCutDate"]))
                .GroupBy(row => row["FabricCombo"].ToString());

            foreach (var group in groupedData)
            {
                string fabricCombo = group.Key;

                int maxCutNo = detailDatas
                    .Where(row => row["FabricCombo"].ToString() == fabricCombo && row["CutNo"] != DBNull.Value)
                    .Select(row => MyUtility.Convert.GetInt(row["CutNo"]))
                    .DefaultIfEmpty(0)
                    .Max();

                foreach (DataRow dr in group)
                {
                    dr["CutNo"] = ++maxCutNo;
                }
            }
        }
        #endregion

        #region P10/P20/P05 檢查
        private static string GetCutrefIN(IEnumerable<string> cutRefs)
        {
            return string.Join(",", cutRefs.Where(cutref => !MyUtility.Check.Empty(cutref)).Select(cutref => $"'{cutref}'"));
        }

        private static DataTable GetDataByCutRefInternal(string stringCutref, string sqlTemplate)
        {
            if (string.IsNullOrEmpty(stringCutref))
            {
                return null;
            }

            string sqlcmd = string.Format(sqlTemplate, stringCutref);
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtCheck);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dtCheck;
        }

        private static bool CheckDataAndShowForm(string tableName, IEnumerable<string> cutRefs, string msg, string sqlTemplate)
        {
            string stringCutref = GetCutrefIN(cutRefs);
            DataTable dtCheck = GetDataByCutRefInternal(stringCutref, sqlTemplate);
            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                var form = new MsgGridForm(dtCheck, msg, $"Exists {tableName} data");
                form.grid1.ColumnsAutoSize();
                form.ShowDialog();
                return false;
            }

            return true;
        }

        private static bool CheckDataAndShowForm(string tableName, string cutRef, string msg, string sqlTemplate)
        {
            if (string.IsNullOrEmpty(cutRef))
            {
                return true;
            }

            string stringCutref = $"'{cutRef}'";
            DataTable dtCheck = GetDataByCutRefInternal(stringCutref, sqlTemplate);
            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                var form = new MsgGridForm(dtCheck, msg, $"Exists {tableName} data");
                form.grid1.ColumnsAutoSize();
                form.ShowDialog();
                return false;
            }

            return true;
        }

        // SQL Templates
        private static readonly string sqlTemplateBundle = @"
SELECT
     [Cutting_P10 ID] = Bundle.ID
    ,[Cut Ref#] = Bundle.CutRef
    ,[Create By] = Pass1.Name
    ,[Create Date] = Format(Bundle.AddDate, 'yyyy/MM/dd HH:mm:ss')
FROM  Bundle WITH(NOLOCK)
LEFT JOIN Pass1 WITH(NOLOCK) ON Bundle.AddName = Pass1.ID
WHERE Bundle.CutRef IN ({0})
AND Bundle.CutRef <> ''
ORDER BY Bundle.ID, Bundle.CutRef, Pass1.Name";

        private static readonly string sqlTemplateCuttingOutput = @"
SELECT
     [Cutting_P20 ID] = CuttingOutput.ID
    ,[Cut Ref#] = CuttingOutput_Detail.CutRef
    ,[Create By] = Pass1.Name
    ,[Create Date] = Format(CuttingOutput.AddDate, 'yyyy/MM/dd HH:mm:ss')
FROM CuttingOutput_Detail WITH(NOLOCK)
INNER JOIN CuttingOutput WITH(NOLOCK) ON CuttingOutput.ID = CuttingOutput_Detail.ID
LEFT JOIN Pass1 WITH(NOLOCK) ON CuttingOutput.AddName = Pass1.ID
WHERE CuttingOutput_Detail.CutRef IN ({0})
ORDER BY CuttingOutput.ID, CuttingOutput_Detail.CutRef, Pass1.Name";

        private static readonly string sqlTemplateMarkerReq = @"
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
LEFT JOIN Pass1 WITH(NOLOCK) ON MarkerReq.AddName = Pass1.ID
WHERE MarkerReq_Detail.CutRef IN ({0})
ORDER BY MarkerReq.ID, MarkerReq_Detail.SizeRatio, Pass1.Name";

        private static readonly string sqlTemplateSpreadingSchedule_Detail = @"
SELECT
[Cut Cell] = S.CutCellID,
[Est.Cut Date] = S.EstCutDate ,
[Create By] = P.[Name] ,
[Create Date] = Convert(nvarchar(19), S.AddDate,120)

FROM SpreadingSchedule S WITH(NOLOCK)
INNER JOIN SpreadingSchedule_Detail SD WITH(NOLOCK) ON S.Ukey = SD.SpreadingScheduleUkey
INNER JOIN Pass1 P WITH(NOLOCK) ON P.ID = S.AddName
WHERE SD.CutRef IN ({0})
Order by S.CutCellID, S.EstCutDate, P.[Name]";

        // Public Methods
        public static bool CheckBundleAndShowData(IEnumerable<string> cutRefs, string msg)
        {
            return CheckDataAndShowForm("bundle", cutRefs, msg, sqlTemplateBundle);
        }

        public static bool CheckBundleAndShowData(string cutRef, string msg)
        {
            return CheckDataAndShowForm("bundle", cutRef, msg, sqlTemplateBundle);
        }

        public static bool CheckCuttingOutputAndShowData(IEnumerable<string> cutRefs, string msg)
        {
            return CheckDataAndShowForm("cutting output", cutRefs, msg, sqlTemplateCuttingOutput);
        }

        public static bool CheckCuttingOutputAndShowData(string cutRef, string msg)
        {
            return CheckDataAndShowForm("cutting output", cutRef, msg, sqlTemplateCuttingOutput);
        }

        public static bool CheckMarkerReqAndShowData(IEnumerable<string> cutRefs, string msg)
        {
            return CheckDataAndShowForm("marker request", cutRefs, msg, sqlTemplateMarkerReq);
        }

        public static bool CheckMarkerReqAndShowData(string cutRef, string msg)
        {
            return CheckDataAndShowForm("marker request", cutRef, msg, sqlTemplateMarkerReq);
        }

        public static bool CheckSpreadingSchedule_DetailAndShowData(IEnumerable<string> cutRefs, string msg)
        {
            return CheckDataAndShowForm("Spreading Schedule", cutRefs, msg, sqlTemplateSpreadingSchedule_Detail);
        }

        public static bool CheckSpreadingSchedule_DetailAndShowData(string cutRef, string msg)
        {
            return CheckDataAndShowForm("Spreading Schedule", cutRef, msg, sqlTemplateSpreadingSchedule_Detail);
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
                { "MarkerNo", "Pattern No cannot be empty." },
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
                .Where(row => MyUtility.Check.Empty(row["CutRef"]) && row["CutNo"] != DBNull.Value)
                .GroupBy(row => new { CutNo = (int)row["CutNo"], FabricCombo = row["FabricCombo"].ToString() })
                .Where(group => group.Count() > 1)
                .ToList();

            // 檢查每個分組內的 MarkerName 或 MarkerNo 是否一致
            foreach (var group in groupedRows)
            {
                var firstRow = group.First();
                if (group.Any(row => row["MarkerName"].ToString() != firstRow["MarkerName"].ToString() || row["MarkerNo"].ToString() != firstRow["MarkerNo"].ToString()))
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
                .Where(row => row["CutNo"] != DBNull.Value && checkContinue(row))
                .GroupBy(r => new
                {
                    MarkerName = MyUtility.Convert.GetString(r["MarkerName"]),
                    MarkerNo = MyUtility.Convert.GetString(r["MarkerNo"]),
                    CutNo = MyUtility.Convert.GetInt(r["CutNo"]),
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
                var cutNoList = group.Select(r => MyUtility.Convert.GetInt(r["CutNo"])).Distinct().ToList();
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

        public static bool CheckDuplicateAndShowMessage(DataTable checkDt, List<string> columnsToCheck, string msgDt, IList<DataRow> detailDatas, CuttingForm form)
        {
            if (!Prgs.CheckForDuplicateKeys(checkDt, columnsToCheck, out DataTable dtCheck))
            {
                DataTable duplicateTable = new DataTable();
                duplicateTable.Columns.Add("CutRef", typeof(string));
                duplicateTable.Columns.Add("CutNo", typeof(string));
                duplicateTable.Columns.Add("MarkerName", typeof(string));

                // 先去重複
                var uniqueRows = dtCheck.AsEnumerable().GroupBy(row => string.Join("|", columnsToCheck.Select(col => row[col].ToString()))).Select(group => group.First()).ToList();

                foreach (DataRow row in uniqueRows)
                {
                    string ukeyName = GetWorkOrderUkeyName(form);
                    int workOrderForUkey = MyUtility.Convert.GetInt(row[ukeyName]);
                    int tmpKey = MyUtility.Convert.GetInt(row["tmpKey"]);
                    var matchingDetailData = detailDatas.FirstOrDefault(dr => MyUtility.Convert.GetInt(dr["Ukey"]) == workOrderForUkey && MyUtility.Convert.GetInt(dr["tmpKey"]) == tmpKey);

                    if (matchingDetailData != null)
                    {
                        DataRow newRow = duplicateTable.NewRow();
                        newRow["CutRef"] = MyUtility.Convert.GetString(matchingDetailData["CutRef"]);
                        newRow["CutNo"] = MyUtility.Convert.GetString(matchingDetailData["CutNo"]);
                        newRow["MarkerName"] = MyUtility.Convert.GetString(matchingDetailData["MarkerName"]);
                        duplicateTable.Rows.Add(newRow);
                    }
                }

                if (duplicateTable.AsEnumerable().Any())
                {
                    string msg = $"The {msgDt} duplicate, Please see below";
                    new MsgGridForm(duplicateTable, msg, "Duplicate Data", "CutRef,CutNo,MarkerName").ShowDialog();
                }

                return false;
            }

            return true;
        }
        #endregion

        #region GridCell/TextBox Seq1,Seq2,Refno,Color

        public static SelectItem PopupSEQ(string id, string fabricCode, string seq1, string seq2, string refno, string colorID, bool isColor)
        {
            DataTable dtSeq = GetSEQbyFabricCode(id, fabricCode);
            DataTable dt = GetFilterSEQ(seq1, seq2, refno, colorID, dtSeq);
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
            DataTable dtSeq = GetSEQbyFabricCode(id, fabricCode);
            dt = GetFilterSEQ(seq1, seq2, refno, colorID, dtSeq);
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

        public static DataTable GetFilterSEQ(string seq1, string seq2, string refno, string colorID, DataTable dt)
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

            return dt.Select(filter).TryCopyToDataTable(dt);
        }

        public static DataTable GetSEQbyFabricCode(string id, string fabricCode)
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
INNER JOIN Fabric f WITH (NOLOCK) ON f.SCIRefno = psd.SCIRefno
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

        public static DataTable GetAllSEQ_FabricCode(string id)
        {
            string sqlcmd = $@"
SELECT
    psd.SEQ1
   ,psd.SEQ2
   ,psd.Refno
   ,ColorID = ISNULL(psdc.SpecValue, '')
   ,psd.SCIRefno
   ,bof.FabricCode--展開相同 BrandRefNo 有多個 SCIRefno 對應的 FabricCode
FROM PO_Supp_Detail psd WITH (NOLOCK)
INNER JOIN PO_Supp_Detail_Spec psdc WITH (NOLOCK) ON psdc.ID = psd.id AND psdc.seq1 = psd.seq1 AND psdc.seq2 = psd.seq2 AND psdc.SpecColumnID = 'Color'
INNER JOIN Fabric f WITH (NOLOCK) ON f.SCIRefno = psd.SCIRefno
INNER JOIN Fabric f2 WITH (NOLOCK) ON f2.BrandRefNo = f.BrandRefNo--這展開,相同 BrandRefNo 有多個 SCIRefno
INNER JOIN Order_BOF bof WITH (NOLOCK) on bof.ID = psd.ID AND bof.SCIRefno = f2.SCIRefno -- 再找到 FabricCode
WHERE psd.ID = '{id}'
AND psd.Junk = 0
AND EXISTS (SELECT 1 from Order_FabricCode WITH (NOLOCK) WHERE ID = '{id}' AND FabricCode = bof.FabricCode)
";

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }

        public static void ConfigureSeqColumnEvents(Ict.Win.UI.DataGridViewTextBoxColumn col_TextBox, Sci.Win.UI.Grid detailgrid, Func<DataRow, bool> canEditData)
        {
            col_TextBox.EditingMouseDown += (sender, e) => SeqCellEditingMouseDown(sender, e, detailgrid, canEditData);
            col_TextBox.CellValidating += (sender, e) => SeqCellValidatingHandler(sender, e, detailgrid, canEditData);
        }

        private static void SeqCellEditingMouseDown(object sender, Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e, Sci.Win.UI.Grid detailgrid, Func<DataRow, bool> canEditData)
        {
            DataRow dr = detailgrid.GetDataRow(e.RowIndex);
            if (!canEditData(dr) || e.Button != MouseButtons.Right)
            {
                return;
            }

            if (MyUtility.Check.Empty(dr["FabricPanelCode"]))
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                return;
            }

            string columnName = detailgrid.Columns[e.ColumnIndex].Name;
            string id = dr["ID"].ToString();
            string fabricCode = dr["FabricCode"].ToString();
            string seq1 = dr["SEQ1"].ToString();
            string seq2 = dr["SEQ2"].ToString();
            string refno = dr["Refno"].ToString();
            string colorID = dr["ColorID"].ToString();

            // 触发的栏位不作为筛选条件
            switch (columnName.ToLower())
            {
                case "seq1":
                    seq1 = string.Empty;
                    break;
                case "seq2":
                    seq2 = string.Empty;
                    break;
            }

            SelectItem selectItem = PopupSEQ(id, fabricCode, seq1, seq2, refno, colorID, false);
            if (selectItem == null)
            {
                return;
            }

            dr["SEQ1"] = selectItem.GetSelecteds()[0]["SEQ1"];
            dr["SEQ2"] = selectItem.GetSelecteds()[0]["SEQ2"];
            dr["Refno"] = selectItem.GetSelecteds()[0]["Refno"];
            dr["ColorID"] = selectItem.GetSelecteds()[0]["ColorID"];
            dr["SCIRefno"] = selectItem.GetSelecteds()[0]["SCIRefno"];
            dr.EndEdit();
        }

        private static void SeqCellValidatingHandler(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e, Sci.Win.UI.Grid detailgrid, Func<DataRow, bool> canEditData)
        {
            DataRow dr = detailgrid.GetDataRow(e.RowIndex);
            string columnName = detailgrid.Columns[e.ColumnIndex].Name;
            string newvalue = e.FormattedValue.ToString();
            string oldvalue = dr[columnName].ToString();
            if (!canEditData(dr) || MyUtility.Check.Empty(newvalue) || newvalue == oldvalue)
            {
                return;
            }

            if (MyUtility.Check.Empty(dr["FabricCode"]))
            {
                dr[columnName] = string.Empty;
                dr.EndEdit();
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                return;
            }

            string id = dr["ID"].ToString();
            string fabricCode = dr["FabricCode"].ToString();
            string seq1 = dr["SEQ1"].ToString();
            string seq2 = dr["SEQ2"].ToString();
            string refno = dr["Refno"].ToString();
            string colorID = dr["ColorID"].ToString();
            switch (columnName.ToLower())
            {
                case "seq1":
                    seq1 = newvalue;
                    break;
                case "seq2":
                    seq2 = newvalue;
                    break;
            }

            if (ValidatingSEQ(id, fabricCode, seq1, seq2, refno, colorID, out DataTable dtValidating))
            {
                dr[columnName] = newvalue;

                // 唯一值时
                if (dtValidating.Rows.Count == 1)
                {
                    dr["SCIRefno"] = dtValidating.Rows[0]["SCIRefno"].ToString();
                }
            }
            else
            {
                dr[columnName] = string.Empty;
            }

            dr.EndEdit();
        }

        /// <summary>
        /// 根據Cutting Poid，取得所有可用的Seq、Refno、Color，然後開窗
        /// </summary>
        /// <returns>SelectItem</returns>
        /// <inheritdoc/>
        public static SelectItem PopupAllSeqRefnoColor(DataTable dt)
        {
            // 根據POID，找出所有 Seq、fabricCode、refno、colorID
            SelectItem selectItem = new SelectItem(dt, "Seq1,Seq2,Refno,ColorID", "3,2,20,3@500,300", string.Empty, false, ",", headercaptions: "Seq1,Seq2,Refno,Color");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            return selectItem;
        }

        #endregion

        #region WKETA

        public static DataTable GetWKETA(string poid)
        {
            string sqlcmd = $@"
SELECT WKETA = E.ETA, ED.Seq1, ED.Seq2
FROM Export_Detail ED WITH(NOLOCK)
INNER JOIN Export E WITH(NOLOCK) ON E.ID = ED.ID
WHERE ED.POID = '{poid}'
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

        #region GridCell/TextBox SpreadingNo

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

        public static void BindGridSpreadingNo(Ict.Win.UI.DataGridViewTextBoxColumn column, Sci.Win.UI.Grid detailgrid, Func<DataRow, bool> canEditData)
        {
            column.EditingMouseDown += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!canEditData(dr))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    SelectItem selectItem = PopupSpreadingNo(dr["SpreadingNoID"].ToString());
                    if (selectItem == null)
                    {
                        return;
                    }

                    dr["SpreadingNoID"] = selectItem.GetSelectedString();
                    dr["CutCellID"] = selectItem.GetSelecteds()[0]["CutCellID"];
                    dr.EndEdit();
                }
            };
            column.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!canEditData(dr))
                {
                    return;
                }

                if (!ValidatingSpreadingNo(e.FormattedValue.ToString(), out DataRow drV))
                {
                    dr["SpreadingNoID"] = string.Empty;
                    e.Cancel = true;
                }

                dr["SpreadingNoID"] = e.FormattedValue;
                if (drV != null)
                {
                    dr["CutCellID"] = drV["CutCellID"];
                }

                dr.EndEdit();
            };
        }
        #endregion

        #region GridCell/TextBox CutCell

        public static SelectItem PopupCutCell(string defaults)
        {
            DataTable dt = GetCutCell();
            SelectItem selectItem = new SelectItem(dt, "ID,Description", "5,40", defaults, false, ",");
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
    ID ,Description
FROM CutCell WITH (NOLOCK)
WHERE MDivisionID = '{Sci.Env.User.Keyword}'
AND Junk = 0
Order BY ID ASC
";

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dt;
        }

        public static void BindGridCutCell(Ict.Win.UI.DataGridViewTextBoxColumn column, Sci.Win.UI.Grid detailgrid, Func<DataRow, bool> canEditData)
        {
            column.EditingMouseDown += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!canEditData(dr))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    SelectItem selectItem = PopupCutCell(dr["CutCellID"].ToString());
                    if (selectItem == null)
                    {
                        return;
                    }

                    dr["CutCellID"] = selectItem.GetSelectedString();
                    dr.EndEdit();
                }
            };
            column.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!canEditData(dr))
                {
                    return;
                }

                if (!ValidatingCutCell(e.FormattedValue.ToString()))
                {
                    dr["CutCellID"] = string.Empty;
                    e.Cancel = true;
                }

                dr["CutCellID"] = e.FormattedValue;
                dr.EndEdit();
            };
        }
        #endregion

        #region GridCell/TextBox Masked Text, 遮罩字串處理, 四個欄位 MarkerLength, (P09: 這3個格式一樣 ActCuttingPerimeter, StraightLength, CurvedLength)

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
        /// 處理 ActCuttingPerimeter, StraightLength, CurvedLength
        /// </summary>
        /// <inheritdoc/>
        public static string FormatLengthData(string input)
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
            if (eventString == string.Empty || (int.TryParse(eventString, out int result) && result == 0))
            {
                return string.Empty;
            }

            eventString = eventString.Replace(" ", "0");
            if (eventString.Contains("Yd"))
            {
                string[] strings = eventString.Split("Yd");
                string[] strings2 = strings[1].Split("\"");
                eventString = $"{strings[0].PadLeft(3, '0')}Yd{strings2[0].PadLeft(2, '0')}\"{strings2[1].PadRight(2, '0')}"; // 最右邊用 PadRight 因為 TextBox.Text會trim前後空白, \" 右邊要往右補0
            }
            else
            {
                eventString = eventString.PadRight(7, '0');
                eventString = $"{eventString.Substring(0, 3)}Yd{eventString.Substring(3, 2)}\"{eventString.Substring(5, 2)}";
            }

            return eventString == "000Yd00\"00" ? string.Empty : eventString;
        }

        public static void Format4LengthColumn(DataRow row)
        {
            row["MarkerLength_Mask"] = Prgs.ConvertFullWidthToHalfWidth(FormatMarkerLength(row["MarkerLength"].ToString()));
            row["ActCuttingPerimeter_Mask"] = Prgs.ConvertFullWidthToHalfWidth(FormatLengthData(row["ActCuttingPerimeter"].ToString()));
            row["StraightLength_Mask"] = Prgs.ConvertFullWidthToHalfWidth(FormatLengthData(row["StraightLength"].ToString()));
            row["CurvedLength_Mask"] = Prgs.ConvertFullWidthToHalfWidth(FormatLengthData(row["CurvedLength"].ToString()));
        }
        #endregion

        #region GridCell/TextBox Pattern No.(MarkerNo)

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

        #region GridCell/TextBox Layers P02
        public static void P02_ValidatingLayers(
            DataRow currentMaintain,
            DataRow currentDetailData,
            int newvalue,
            DataTable dt_SizeRatio,
            DataTable dt_Distribute,
            DataTable dt_PatternPanel,
            CuttingForm formType)
        {
            int oldvalue = MyUtility.Convert.GetInt(currentDetailData["Layer"]);
            if (oldvalue == newvalue)
            {
                return;
            }

            currentDetailData["Layer"] = newvalue;
            UpdateExcess(currentDetailData, MyUtility.Convert.GetInt(currentDetailData["Layer"]), dt_SizeRatio, dt_Distribute, formType);

            // 更新右邊的Size Ratio Grid
            var workOrderForPlanningUkey = MyUtility.Convert.GetLong(currentDetailData["Ukey"]);
            var tmpKey = MyUtility.Convert.GetLong(currentDetailData["tmpKey"]);

            dt_SizeRatio.AsEnumerable()
            .Where(o => o.RowState != DataRowState.Deleted
                && MyUtility.Convert.GetLong(o["WorkOrderForPlanningUkey"]) == workOrderForPlanningUkey && MyUtility.Convert.GetLong(o["tmpKey"]) == tmpKey)
            .ToList().ForEach(o =>
            {
                o["Layer"] = newvalue;
                o["TotalCutQty_CONCAT"] = ConcatTTLCutQty(o);
            });

            currentDetailData["Cons"] = CalculateCons(currentDetailData, MyUtility.Convert.GetDecimal(currentDetailData["ConsPC"]), MyUtility.Convert.GetDecimal(currentDetailData["Layer"]), dt_SizeRatio, formType);
            UpdateConcatString(currentDetailData, dt_SizeRatio, formType);

            if (MyUtility.Convert.GetInt(currentMaintain["UseCutRefToRequestFabric"]) == 2)
            {
                var p09_AutoDistToSP = new P09_AutoDistToSP(currentDetailData, dt_SizeRatio, dt_Distribute, dt_PatternPanel, formType);
                DualResult result = p09_AutoDistToSP.DoAutoDistribute();
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }
            }

            currentDetailData.EndEdit();
        }
        #endregion

        #region GridCell SizeRatio SizeCode
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
            CuttingForm form,
            int layer = 0)
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

            System.Windows.Forms.BindingSource sizeRatiobs = (System.Windows.Forms.BindingSource)gridSizeRatio.DataSource;
            UpdateExcess(currentDetailData, layer, (DataTable)sizeRatiobs.DataSource, dtDistribute, form);
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
            CuttingForm form,
            int layer = 0)
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
            UpdateExcess(currentDetailData, layer, (DataTable)sizeRatiobs.DataSource, dtDistribute, form);
            return true;
        }
        #endregion

        #region GridCell SizeRatio Qty

        /// <summary>
        /// grid SizeRatio Qty 欄位驗證
        /// </summary>
        /// <inheritdoc/>
        public static bool P02_SizeRationQtyValidating(
            Sci.Win.UI.Grid grid,
            Ict.Win.UI.DataGridViewCellValidatingEventArgs e,
            DataRow currentMaintain,
            DataRow currentDetailData,
            DataTable dtSizeRatio,
            DataTable dtDistribute,
            DataTable dtPatternPanel,
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

            int layer = MyUtility.Convert.GetInt(currentDetailData["Layer"]);
            UpdateExcess(currentDetailData, layer, dtSizeRatio, dtDistribute, form);

            dr["TotalCutQty_CONCAT"] = ConcatTTLCutQty(dr);
            UpdateConcatString(currentDetailData, dtSizeRatio, form);
            currentDetailData["ConsPC"] = CalculateConsPC(currentDetailData, MyUtility.Convert.GetDecimal(currentDetailData["Cons"]), MyUtility.Convert.GetDecimal(currentDetailData["Layer"]), dtSizeRatio, form);
            if (MyUtility.Convert.GetInt(currentMaintain["UseCutRefToRequestFabric"]) == 2)
            {
                var p09_AutoDistToSP = new P09_AutoDistToSP(currentDetailData, dtSizeRatio, dtDistribute, dtPatternPanel, form);
                DualResult result = p09_AutoDistToSP.DoAutoDistribute();
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region GridCell SizeRatio / Distribute Qty

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
            CuttingForm form,
            int layer = 0)
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

            UpdateExcess(currentDetailData, layer, dtSizeRatio, dtDistribute, form);

            return true;
        }
        #endregion

        #region GridCell Distribute OrderID, Article, SizeCode

        /// <summary>
        /// Distribute
        /// </summary>
        /// <inheritdoc/>
        public static bool Distribute3CellEditingMouseDown(
            Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e,
            DataRow currentDetailData,
            DataTable dtSizeRatio,
            Sci.Win.UI.Grid gridDistributeToSP,
            CuttingForm form,
            int layer = 0)
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

            var distributeToSPbs = (System.Windows.Forms.BindingSource)gridDistributeToSP.DataSource;
            UpdateExcess(currentDetailData, layer, dtSizeRatio, (DataTable)distributeToSPbs.DataSource, form);
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
            CuttingForm form,
            int layer = 0)
        {
            var distributeToSPbs = (System.Windows.Forms.BindingSource)gridDistributeToSP.DataSource;
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
                UpdateExcess(currentDetailData, layer, dtSizeRatio, (DataTable)distributeToSPbs.DataSource, form);
                return true;
            }

            string orderID = MyUtility.Convert.GetString(dr["OrderID"]);
            string article = MyUtility.Convert.GetString(dr["Article"]);
            string sizeCode = MyUtility.Convert.GetString(dr["SizeCode"]);
            switch (columnName.ToLower())
            {
                case "orderid":
                    orderID = newvalue;
                    break;
                case "article":
                    article = newvalue;
                    break;
                case "sizecode":
                    sizeCode = newvalue;
                    break;
            }

            if (FilterOrder_Qty_By_SizeRatio(currentDetailData["ID"].ToString(), orderID, article, sizeCode, dtSizeRatio).Rows.Count == 0)
            {
                dr[columnName] = string.Empty;
                dr.EndEdit();
                MyUtility.Msg.WarningBox($"<SP#>:{orderID},<Article>:{article},<Size>:{sizeCode} not exists qty break down");
                e.Cancel = true;
                UpdateExcess(currentDetailData, layer, dtSizeRatio, (DataTable)distributeToSPbs.DataSource, form);
                return true;
            }

            dr[columnName] = newvalue;
            dr.EndEdit();

            // 驗證需要重算Excess
            UpdateExcess(currentDetailData, layer, dtSizeRatio, (DataTable)distributeToSPbs.DataSource, form);

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

        #region GridCell Article

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

                // 先判斷有沒有Order_EachCons_Article
                string cmd = $@"SELECT Article FROM Order_EachCons_Article WHERE Order_EachConsUkey = {dr["Order_EachconsUkey"]}";

                DBProxy.Current.Select(null, cmd, out DataTable dtArticle);

                if (dtArticle == null || dtArticle.Rows.Count == 0)
                {
                    // 沒有 Order_EachCons_Article的話則從 套用以下規則找 Article
                    if (workType == "2")
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

                    DBProxy.Current.Select(null, cmd, out dtArticle);
                }

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

        #region Grid PatternPanel
        public static void BindPatternPanelEvents(Ict.Win.UI.DataGridViewTextBoxColumn column, string id)
        {
            column.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    System.Windows.Forms.DataGridView grid = ((DataGridViewColumn)s).DataGridView;
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
                System.Windows.Forms.DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                string columnName = grid.Columns[e.ColumnIndex].Name;
                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                string oldValue = row[columnName].ToString();
                string newValue = e.FormattedValue.ToString();
                if (newValue.IsNullOrWhiteSpace() || oldValue == newValue)
                {
                    return;
                }

                string patternpanel = MyUtility.Convert.GetString(row["patternpanel"]);
                string fabricpanelcode = MyUtility.Convert.GetString(row["fabricpanelcode"]);
                switch (columnName.ToLower())
                {
                    case "patternpanel":
                        patternpanel = newValue;
                        break;
                    case "fabricpanelcode":
                        fabricpanelcode = newValue;
                        break;
                }

                DataTable dt = GetFilterPatternPanel(id, patternpanel, fabricpanelcode);
                if (dt.Rows.Count == 0)
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

        public static DataTable GetFilterPatternPanel(string id, string patternpanel, string fabricpanelcode)
        {
            string filter = "1=1";
            if (!patternpanel.IsNullOrWhiteSpace())
            {
                filter += $" AND patternpanel = '{patternpanel}'";
            }

            if (!fabricpanelcode.IsNullOrWhiteSpace())
            {
                filter += $" AND fabricpanelcode = '{fabricpanelcode}'";
            }

            DataTable dt = GetPatternPanel(id);
            return dt.Select(filter).TryCopyToDataTable(dt);
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

        #region Grid Event
        public static void Grid_ClickBeginEdit(object sender, EventArgs e)
        {
            Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)sender;
            if (MyUtility.Check.Empty(grid.CurrentCell))
            {
                return;
            }

            // 游標直接進入 Cell, 才不用點兩下
            grid.CurrentCell = grid[grid.CurrentCell.ColumnIndex, grid.CurrentCell.RowIndex];
            grid.BeginEdit(true);
        }

        public delegate bool CanEditDelegate(Sci.Win.UI.Grid grid, DataRow dr, string columnName);

        public static void ConfigureColumnEvents(Sci.Win.UI.Grid grid, CanEditDelegate canEditDelegate)
        {
            foreach (var column in grid.Columns)
            {
                // 欄位沒設定 IsEditingReadOnly 才作變更
                if (column is DataGridViewTextBoxBaseColumn textBoxBase && !textBoxBase.IsEditingReadOnly)
                {
                    textBoxBase.EditingControlShowing += (sender, e) => CustomEditingControlShowing(sender, e, canEditDelegate);
                    textBoxBase.CellFormatting += (sender, e) => CustomCellFormatting(sender, e, canEditDelegate);
                }
                else if (column is DataGridViewTextBoxBase2Column textBoxBase2 && !textBoxBase2.IsEditingReadOnly)
                {
                    textBoxBase2.EditingControlShowing += (sender, e) => CustomEditingControlShowing(sender, e, canEditDelegate);
                    textBoxBase2.CellFormatting += (sender, e) => CustomCellFormatting(sender, e, canEditDelegate);
                }
                else if (column is DataGridViewMaskedTextBoxBaseColumn maskedtextBoxBase && !maskedtextBoxBase.IsEditingReadOnly)
                {
                    maskedtextBoxBase.EditingControlShowing += (sender, e) => CustomEditingControlShowing(sender, e, canEditDelegate);
                    maskedtextBoxBase.CellFormatting += (sender, e) => CustomCellFormatting(sender, e, canEditDelegate);
                }
                else if (column is DataGridViewTextBoxBase2Column textBoxBaseEX && textBoxBaseEX.DataPropertyName == "WKETA")
                {
                    // 特別處理 WKETA 變色
                    textBoxBaseEX.CellFormatting += (sender, e) => CustomCellFormatting(sender, e, canEditDelegate);
                }
            }
        }

        private static void CustomEditingControlShowing(object sender, Ict.Win.UI.DataGridViewEditingControlShowingEventArgs e, CanEditDelegate canEditDelegate)
        {
            // 可否編輯
            Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)sender).DataGridView;
            DataRow dr = grid.GetDataRow(e.RowIndex);
            bool canEdit = canEditDelegate(grid, dr, grid.Columns[e.ColumnIndex].Name);
            if (e.Control is Ict.Win.UI.TextBoxBase textBoxBase)
            {
                textBoxBase.ReadOnly = !canEdit;
            }
            else if (e.Control is Ict.Win.UI.TextBoxBase2 textBoxBase2)
            {
                textBoxBase2.ReadOnly = !canEdit;
            }
            else if (e.Control is Ict.Win.UI.MaskedTextBoxBase textBoxBase3)
            {
                textBoxBase3.ReadOnly = !canEdit;
            }
        }

        private static void CustomCellFormatting(object sender, DataGridViewCellFormattingEventArgs e, CanEditDelegate canEditDelegate)
        {
            // 粉底紅字
            Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)sender).DataGridView;
            DataRow dr = grid.GetDataRow(e.RowIndex);
            string columnName = grid.Columns[e.ColumnIndex].Name.ToLower();
            bool canEdit = canEditDelegate(grid, dr, columnName);
            if (canEdit)
            {
                e.CellStyle.BackColor = Color.Pink;
                e.CellStyle.ForeColor = Color.Red;
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
            }
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
            // P02 沒有顯示 Sewinline
            if (form == CuttingForm.P02)
            {
                return;
            }

            DateTime? sewinline = dtDistribute.Select(GetFilter(currentDetailData, form)).AsEnumerable().Min(row => MyUtility.Convert.GetDate(row["Sewinline"]));
            currentDetailData["Sewinline"] = sewinline ?? (object)DBNull.Value;
        }

        /// <summary>
        /// 更新 CurrentDetailData 的*非實體*欄位:Article_CONCAT, 因 Distribute 資訊變動
        /// </summary>
        /// <inheritdoc/>
        public static void UpdateArticle_CONCAT(DataRow currentDetailData, DataTable dtDistribute, CuttingForm form)
        {
            string filter = GetFilter(currentDetailData, form) + " AND Article <> ''";
            currentDetailData["Article_CONCAT"] = dtDistribute.Select(filter).AsEnumerable().Select(row => row["Article"].ToString()).Distinct().OrderBy(a => a).JoinToString("/");
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
                currentDetailData["FabricCombo"] = string.Empty;
                currentDetailData["FabricPanelCode"] = string.Empty;
                currentDetailData["FabricCode"] = string.Empty;
                currentDetailData["PatternPanel_CONCAT"] = string.Empty;
                currentDetailData["FabricPanelCode_CONCAT"] = string.Empty;
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
            return dtPatternPanel.Select(GetFilter(currentDetailData, form)).AsEnumerable().Where(row => !MyUtility.Check.Empty(row["FabricPanelCode"])).OrderBy(row => MyUtility.Convert.GetString(row["FabricPanelCode"])).FirstOrDefault();
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
        public static void UpdateExcess(DataRow currentDetailData, int layer, DataTable dtSizeRatio, DataTable dtDistribute, CuttingForm form)
        {
            string workOrderUkey = GetWorkOrderUkeyName(form);

            string filter = GetFilter(currentDetailData, form);
            foreach (DataRow dr in dtSizeRatio.Select(filter))
            {
                int ttlQty_SizeCode = MyUtility.Convert.GetInt(dr["Qty"]) * layer; // 此 SizeCode 總數量
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
                else if (excess > 0)
                {
                    DataRow newExcessRow = dtDistribute.NewRow();
                    newExcessRow[workOrderUkey] = currentDetailData["Ukey"];
                    newExcessRow["tmpkey"] = currentDetailData["tmpkey"];
                    newExcessRow["ID"] = currentDetailData["ID"];
                    newExcessRow["OrderID"] = "EXCESS";
                    newExcessRow["Article"] = string.Empty;
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
            return form == CuttingForm.P02 ? $"WorkOrderForPlanningUkey" : $"WorkOrderForOutputUkey";
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
        /// 計算 ConsPC *實體*欄位, 只用在改變 SizeRario.Qty ,不管 MarkerLength 值
        /// </summary>
        /// <inheritdoc/>
        public static decimal CalculateConsPC(DataRow currentDetailData, decimal cons, decimal layer, DataTable dtSizeRatio, CuttingForm form)
        {
            decimal sizeRatioQty = dtSizeRatio.Select(GetFilter(currentDetailData, form)).AsEnumerable().Sum(row => MyUtility.Convert.GetDecimal(row["Qty"]));
            return sizeRatioQty * layer == 0 ? 0 : cons / (sizeRatioQty * layer);
        }

        /// <summary>
        /// 計算 Cons *實體*欄位, 當改變3個欄位 ConsPC,Layer,SizeRatio.Qty 時
        /// </summary>
        /// <inheritdoc/>
        public static decimal CalculateCons(DataRow currentDetailData, decimal consPC, decimal layer, DataTable dtSizeRatio, CuttingForm form)
        {
            decimal sizeRatioQty = dtSizeRatio.Select(GetFilter(currentDetailData, form)).AsEnumerable().Sum(row => MyUtility.Convert.GetDecimal(row["Qty"]));
            return consPC * layer * sizeRatioQty;
        }

        /// <summary>
        /// 用於新增 SizeRatio 不輸入 SizeCode 輸入 Qty, 會加總Qty算 ConsPC, 然後存檔, 空白 SizeCode 會刪除
        /// </summary>
        /// <inheritdoc/>
        public static void BeforeSaveCalculateConsPC(IList<DataRow> detailDatas, DataTable dtSizeRatio, CuttingForm form)
        {
            dtSizeRatio.AcceptChanges();
            foreach (DataRow row in detailDatas)
            {
                row["ConsPC"] = CalculateConsPC(row, MyUtility.Convert.GetDecimal(row["Cons"]), MyUtility.Convert.GetDecimal(row["Layer"]), dtSizeRatio, form);
            }
        }

        /// <summary>
        /// 複製資料使用
        /// </summary>
        /// <param name="currentDetailData">新增之後Row</param>
        /// <param name="oldRow">舊的Row包含資訊</param>
        /// <param name="dtTarget">要複製的DataTable</param>
        /// <param name="form">P02或P09</param>
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

        public static void SetControlFontSize(Control parent, float fontSize)
        {
            foreach (Control control in parent.Controls)
            {
                control.Font = new Font(control.Font.FontFamily, fontSize);
            }
        }

        /// <summary>
        /// P02 SizeRatio 表格個欄位計算Total Cut Qty，Layer使用自己身上的就好
        /// </summary>
        /// <returns>TotalCutQty_CONCAT</returns>
        /// <inheritdoc/>
        public static string ConcatTTLCutQty(DataRow dr)
        {
            int layerQty = MyUtility.Convert.GetInt(dr["Layer"]) * MyUtility.Convert.GetInt(dr["Qty"]);
            return $"{dr["SizeCode"]}/{layerQty}";
        }

        public static DataTable QueryQtyBreakDown(string cuttingID, CuttingForm form)
        {
            string tableName = GetTableName(form);

            string sqlcmd = $@"
SELECT
    wd.OrderID
   ,wd.Article
   ,wd.SizeCode
   ,wo.FabricCombo
   ,Qty = SUM(wd.Qty)
INTO #tmp
FROM {tableName} wo WITH (NOLOCK)
INNER JOIN {tableName}_Distribute wd WITH (NOLOCK) ON wo.ukey = wd.{GetWorkOrderUkeyName(form)}
WHERE wo.id = '{cuttingID}'
GROUP BY wo.FabricCombo
        ,wd.article
        ,wd.SizeCode
        ,wd.OrderID

SELECT
    oq.ID
   ,oq.Article
   ,oq.SizeCode
   ,oq.Qty
   ,Balance = ISNULL(balc.minQty - oq.qty, 0)
FROM Order_Qty oq WITH (NOLOCK)
INNER JOIN Orders o WITH (NOLOCK) ON oq.id = o.id
OUTER APPLY (
    SELECT minQty = MIN(Qty)
    FROM #tmp t
    WHERE t.OrderID = oq.ID
    AND t.article = oq.Article
    AND t.SizeCode = oq.SizeCode
) balc
WHERE o.CuttingSP = '{cuttingID}'
ORDER BY ID, Article, SizeCode
DROP TABLE #tmp";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtQtyBreakDown);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            return dtQtyBreakDown;
        }
        #endregion

        #region CutPartCheck

        /// <summary>
        /// CutPartCheck 使用,總和 非編輯數據 或 編輯中尚未存檔的數據, P02 by POID, P09 by OrderID 總和
        /// </summary>
        /// <param name="form">P02/P09</param>
        /// <param name="detailDatas">表身</param>
        /// <param name="dtSizeRatio">dtSizeRatio</param>
        /// <param name="dtDistribute">dtDistribute</param>
        /// <returns>DataTable</returns>
        public static DataTable ProcessWorkOrder_CutPartCheck(CuttingForm form, IList<DataRow> detailDatas, DataTable dtSizeRatio, DataTable dtDistribute)
        {
            string ukeyName = GetWorkOrderUkeyName(form);
            var query = from t1 in detailDatas.AsEnumerable()
                        join t2 in dtDistribute.AsEnumerable()
                        on new
                        {
                            ukey = MyUtility.Convert.GetInt(t1["Ukey"]),
                            tmpkey = MyUtility.Convert.GetInt(t1["tmpkey"]),
                        }
                        equals new
                        {
                            ukey = MyUtility.Convert.GetInt(t2[ukeyName]),
                            tmpkey = MyUtility.Convert.GetInt(t2["tmpkey"]),
                        }
                        group new { t1, t2 } by new
                        {
                            ID = MyUtility.Convert.GetString(t2["OrderID"]),
                            Article = MyUtility.Convert.GetString(t2["Article"]),
                            SizeCode = MyUtility.Convert.GetString(t2["SizeCode"]),
                            PatternPanel = MyUtility.Convert.GetString(t1["FabricCombo"]),
                        }
                        into g
                        select new
                        {
                            g.Key.ID,
                            g.Key.Article,
                            g.Key.SizeCode,
                            g.Key.PatternPanel,
                            CutQty = g.Sum(x => MyUtility.Convert.GetInt(x.t2["Qty"])),
                        };
            return query.ToList().ToDataTable();
        }

        /// <summary>
        /// Order 資訊為基底
        /// </summary>
        /// <param name="form">P02/P09</param>
        /// <param name="cuttingID">cuttingID</param>
        /// <param name="dtWorkOrder">ProcessWorkOrder_CutPartCheck 出來的資訊</param>
        /// <param name="dt">Order整理後 Left join dtWorkOrder</param>
        /// <returns>DualResult</returns>
        public static DualResult GetBase_CutPartCheck(CuttingForm form, string cuttingID, DataTable dtWorkOrder, out DataTable dt)
        {
            // P02 加回 Distribute, 可以 by OrderID 計算
            string columnID = ",o.ID";
            string sqlcmd = $@"
--基本資訊 Order_Qty 
SELECT o.POID, oq.ID, oq.Article, oq.SizeCode, oq.Qty
   ,IsCancel = CAST(IIF(o.Junk = 1 AND o.NeedProduction = 0, 1, 0) AS BIT)
INTO #tmpOrder_Qty
FROM Orders o WITH (NOLOCK)
INNER JOIN Order_Qty oq WITH (NOLOCK) ON o.ID = oq.ID
WHERE o.CuttingSP = '{cuttingID}'

--P02 by POID 總和的訂單數量
--P09 by OrderID 的訂單數量
SELECT o.POID{columnID}, o.Article, o.SizeCode, IsCancel, Qty = SUM(o.Qty)
INTO #tmpGroup
FROM #tmpOrder_Qty o
GROUP BY o.POID{columnID},o.Article, o.SizeCode, IsCancel -- P02 可能會有一個子單 IsCancel 無法 Group

--完成準備資訊 展開 PKey: FabricPanelCode
SELECT DISTINCT
    o.POID
   {columnID}
   ,o.Article
   ,o.SizeCode
   ,o.Qty
   ,o.IsCancel
   ,occ.ColorID
   ,occ.PatternPanel
INTO #tmpOrder
FROM #tmpGroup o
INNER JOIN Order_ColorCombo occ WITH (NOLOCK) ON occ.id = o.POID AND occ.Article = o.Article
WHERE occ.FabricCode <> ''
AND EXISTS (SELECT 1 FROM Order_EachCons WITH (NOLOCK) WHERE ID = o.POID AND FabricPanelCode = occ.FabricPanelCode AND CuttingPiece = 0)  --排除外裁 FabricPanelCode

-- 將 WorkOrder 裁剪數 CutQty
SELECT
   o.POID
   {columnID}
   ,o.Article
   ,o.SizeCode
   ,o.Qty
   ,o.ColorID
   ,o.PatternPanel
   ,o.IsCancel
   ,w.CutQty
   ,Variance = w.CutQty - o.Qty
INTO #tmpFinal
FROM #tmpOrder o
LEFT JOIN #tmp w ON w.ID = o.ID -- 此處 P09 會把 EXCESS 排除, P02 用 POID
                AND w.Article = o.Article
                AND w.SizeCode = o.SizeCode
                AND w.PatternPanel = o.PatternPanel
UNION ALL
SELECT
    o.POID
   {columnID}
   ,o.Article
   ,o.SizeCode
   ,o.Qty
   ,ColorID = ''
   ,Patternpanel = '='
   ,o.IsCancel
   ,CutQty = NULL
   ,Variance = NULL
FROM #tmpGroup o WITH (NOLOCK)

-- 取 SizeCode 排序
SELECT    
   o.ID
   , o.Article, o.SizeCode, o.Qty, o.ColorID, o.Patternpanel, o.IsCancel, o.CutQty, o.Variance
FROM #tmpFinal o
INNER JOIN Order_SizeCode os WITH (NOLOCK) ON os.ID = o.POID AND os.SizeCode = o.SizeCode
ORDER BY o.POID{columnID}, Article, os.Seq, PatternPanel    
";
            return MyUtility.Tool.ProcessWithDatatable(dtWorkOrder, string.Empty, sqlcmd, out dt);
        }

        /// <summary>
        /// 將 CutPartCheck 樞紐
        /// </summary>
        /// <param name="dtCutPartCheck">CutPartCheck</param>
        /// <returns>DataTable</returns>
        public static DataTable ProcessCutpartCheckSummary(DataTable dtCutPartCheck)
        {
            // 排除 Patternpanel='=' 的行
            var filteredRows = dtCutPartCheck.AsEnumerable().Where(row => MyUtility.Convert.GetString(row["Patternpanel"]) != "=");

            // 唯一的 ID, Article, SizeCode, Qty 組合
            var uniqueKeys = filteredRows
                .GroupBy(row => new
                {
                    ID = MyUtility.Convert.GetString(row["ID"]),
                    Article = MyUtility.Convert.GetString(row["Article"]),
                    SizeCode = MyUtility.Convert.GetString(row["SizeCode"]),
                    Qty = MyUtility.Convert.GetInt(row["Qty"]),
                    IsCancel = MyUtility.Convert.GetBool(row["IsCancel"]),
                })
                .Select(group => group.Key)
                .ToList();

            // 創建新的 DataTable 並添加相應的列
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("ID", typeof(string));
            resultTable.Columns.Add("Article", typeof(string));
            resultTable.Columns.Add("SizeCode", typeof(string));
            resultTable.Columns.Add("Qty", typeof(int));
            resultTable.Columns.Add("IsCancel", typeof(bool));
            resultTable.Columns.Add("Complete", typeof(string));

            // 動態添加 Patternpanel 作為列
            var listPatternPanels = filteredRows.Select(row => MyUtility.Convert.GetString(row["Patternpanel"])).Distinct().ToList();

            foreach (var panel in listPatternPanels)
            {
                resultTable.Columns.Add(panel, typeof(int));
            }

            // 填充數據
            foreach (var key in uniqueKeys)
            {
                var newRow = resultTable.NewRow();
                newRow["ID"] = key.ID;
                newRow["Article"] = key.Article;
                newRow["SizeCode"] = key.SizeCode;
                newRow["Qty"] = key.Qty;
                newRow["IsCancel"] = key.IsCancel;

                bool isComplete = true;

                foreach (var panel in listPatternPanels)
                {
                    var matchRow = filteredRows
                        .FirstOrDefault(row =>
                            MyUtility.Convert.GetString(row["ID"]) == key.ID &&
                            MyUtility.Convert.GetString(row["Article"]) == key.Article &&
                            MyUtility.Convert.GetString(row["SizeCode"]) == key.SizeCode &&
                            MyUtility.Convert.GetInt(row["Qty"]) == key.Qty &&
                            MyUtility.Convert.GetString(row["Patternpanel"]) == panel);

                    int cutQty = matchRow != null ? MyUtility.Convert.GetInt(matchRow["CutQty"]) : 0;
                    newRow[panel] = cutQty;

                    if (cutQty < key.Qty)
                    {
                        isComplete = false;
                    }
                }

                newRow["Complete"] = isComplete ? "Y" : string.Empty;

                resultTable.Rows.Add(newRow);
            }

            return resultTable;
        }
        #endregion

        #region 列印

        /// <summary>
        /// Key = Table名稱，Value = 無欄位的清單
        /// </summary>
        public Dictionary<string, List<string>> DicTableLostColumns
        {
            get
            {
                if (this._dicTableLostColumns == null)
                {
                    this.GetDictionaryTableLostColumns();
                }

                return this._dicTableLostColumns;
            }

            private set
            {
            }
        }

        private Dictionary<string, List<string>> _dicTableLostColumns;

        /// <summary>
        /// For Cutting P02、P09的範本檔下載
        /// </summary>
        /// <param name="cuttingForm">Type CuttingForm</param>
        /// <param name="mRow">資料來源主檔</param>
        /// <param name="errMsg">錯誤訊息</param>
        /// <returns>成功失敗</returns>
        public bool DownloadSampleFile(CuttingForm cuttingForm, DataRow mRow, out string errMsg)
        {
            errMsg = string.Empty;
            string xltxName = cuttingForm == CuttingForm.P02 ? "Cutting_P02. Import Marker Template Download" : "Cutting_P09. Import Marker Template Download";

            string style = MyUtility.GetValue.Lookup($"SELECT StyleID FROM Orders WITH(NOLOCK) WHERE ID = '{mRow["ID"]}'");
            try
            {
                string strXltName = Env.Cfg.XltPathDir + $@"\{xltxName}.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName); // 預先開啟excel app
                string strExcelName = Class.MicrosoftFile.GetName(xltxName);
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets["Upload"];
                worksheet.Cells[2, 2] = mRow["ID"].ToString();
                worksheet.Cells[2, 7] = style;

                workbook.SaveAs(strExcelName);
                workbook.Close();
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(workbook);
                strExcelName.OpenFile();
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 取得Print資料
        /// </summary>
        /// <param name="cuttingForm">From P02、P09</param>
        /// <param name="drInfoFrom">原資料帶入</param>
        /// <param name="s1">參數起</param>
        /// <param name="s2">參數迄</param>
        /// <param name="printType">"Cutref"、"Cutplanid"</param>
        /// <param name="sortType">排序，"Cutref"、"SpreadingNoID,CutCellID,Cutref"</param>
        /// <param name="arrDtType">輸出資料表陣列，配合Print的ToExcel時用</param>
        /// <returns>DualResult</returns>
        public DualResult GetPrintData(CuttingForm cuttingForm, DataRow drInfoFrom, string s1, string s2, string printType, string sortType, out DataTable[] arrDtType)
        {
            string tableName = GetTableName(cuttingForm);
            string tbPatternPanel = tableName + "_PatternPanel";
            string tbDistribute = tableName + "_Distribute";
            string tbSizeRatio = tableName + "_SizeRatio";
            bool isTbDistribute = this.CheckTableExist(tbDistribute); // 基本上P09(也就是ForOutput才有Distribute表)
            bool isTbSizeRatio = this.CheckTableExist(tbSizeRatio);
            string tbUkey = tableName + "Ukey";
            arrDtType = new DataTable[13];
            List<SqlParameter> paras = new List<SqlParameter>
            {
                new SqlParameter("@Cutref1", s1),
                new SqlParameter("@Cutref2", s2),
            };

            string sqlParas = string.Empty;
            foreach (var item in paras)
            {
                sqlParas += $"DECLARE {item.ParameterName} varchar(50) ='{item.Value}';\r\n";
            }

            string byType, byType2;
            string strOrderby = string.Empty;

            string sqlFabricKind = string.Empty;
            string sqlFabricKindinto = string.Empty;
            string sqlFabricKindjoin = string.Empty;
            if (printType == "Cutref")
            {
                byType = "Cutref";
                byType2 = ",shc";
                strOrderby = "order by " + sortType;
                if (isTbDistribute)
                {
                    sqlFabricKind = $@"
    SELECT distinct w.CutRef, wp.PatternPanel, x.FabricKind
    into #tmp3
    FROM #tmp W
    INNER JOIN {tbPatternPanel} WP ON W.Ukey = WP.{tbUkey}
    outer apply(
	    SELECT  FabricKind=DD.id + '-' + DD.NAME ,Refno
	    FROM dropdownlist DD 
	    OUTER apply(
			    SELECT OB.kind, 
			    OCC.id, 
			    OCC.article, 
			    OCC.colorid, 
			    OCC.fabricpanelcode, 
			    OCC.patternpanel ,
			    Refno
		    FROM order_colorcombo OCC 
		    INNER JOIN order_bof OB ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		    where exists(select 1 from {tbDistribute} wd where wd.{tbUkey} = W.Ukey and wd.Article = OCC.Article)
	    ) LIST 
	    WHERE LIST.id = w.id 
	    AND LIST.patternpanel = wp.patternpanel 
	    AND DD.[type] = 'FabricKind' 
	    AND DD.id = LIST.kind 
    )x

    select CutRef,ct = count(1) into #tmp4 from(select distinct CutRef,FabricKind from #tmp3)x group by CutRef

    select t4.CutRef,FabricKind = IIF(t4.ct = 1, x1.FabricKind, x2.FabricKind)
    into #tmp5
    from #tmp4 t4
    outer apply(
	    select distinct t3.FabricKind
	    from #tmp3 t3
	    where t3.CutRef = t4.CutRef and t4.ct = 1
    )x1
    outer apply(
	    select FabricKind = STUFF((
		    select concat(', ', t3.FabricKind, ': ', t3.PatternPanel)
		    from #tmp3 t3
		    where t3.CutRef = t4.CutRef and t4.ct > 1
		    for XML path('')
	    ),1,2,'')
    )x2
    ";
                    sqlFabricKindinto = $@" , rn=min(rn) into #tmp6 ";
                    sqlFabricKindjoin = $@"select t6.*,t5.FabricKind from #tmp6 t6 inner join #tmp5 t5 on t5.CutRef = t6.CutRef order by rn";
                }
                else
                {
                    sqlFabricKind = $@"
SELECT distinct w.CutRef, wp.PatternPanel, x.FabricKind
into #tmp3
FROM #tmp W
INNER JOIN {tbPatternPanel} WP ON W.Ukey = WP.{tbUkey}
outer apply(
	SELECT  FabricKind=DD.id + '-' + DD.NAME ,Refno
	FROM dropdownlist DD 
	OUTER apply(
			SELECT OB.kind, 
			OCC.id, 
			OCC.article, 
			OCC.colorid, 
			OCC.fabricpanelcode, 
			OCC.patternpanel ,
			Refno
		FROM order_colorcombo OCC 
		INNER JOIN order_bof OB ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		where w.Article = OCC.Article
	) LIST 
	WHERE LIST.id = w.id 
	AND LIST.patternpanel = wp.patternpanel 
	AND DD.[type] = 'FabricKind' 
	AND DD.id = LIST.kind 
)x

select CutRef,ct = count(1) into #tmp4 from(select distinct CutRef,FabricKind from #tmp3)x group by CutRef

select t4.CutRef,FabricKind = IIF(t4.ct = 1, x1.FabricKind, x2.FabricKind)
into #tmp5
from #tmp4 t4
outer apply(
	select distinct t3.FabricKind
	from #tmp3 t3
	where t3.CutRef = t4.CutRef and t4.ct = 1
)x1
outer apply(
	select FabricKind = STUFF((
		select concat(', ', t3.FabricKind, ': ', t3.PatternPanel)
		from #tmp3 t3
		where t3.CutRef = t4.CutRef and t4.ct > 1
		for XML path('')
	),1,2,'')
)x2
";
                    sqlFabricKindinto = $@" , rn=min(rn) into #tmp6 ";
                    sqlFabricKindjoin = $@"select t6.*,t5.FabricKind from #tmp6 t6 inner join #tmp5 t5 on t5.CutRef = t6.CutRef order by rn";
                }
            }
            else
            {
                byType = "Cutplanid";
                byType2 = string.Empty;
                strOrderby = "order by cutref";
            }

            string sqlColByType = this.CheckAndGetColumns(cuttingForm, byType);
            string sqlWhereByType = this.CheckTableLostColumns(cuttingForm, byType) ? "1=1" : $"{byType}>= @Cutref1 and {byType}<= @Cutref2";

            string workorder_cmd =
                $@"
Select a.AddDate
,a.AddName
,a.ColorID
,a.Cons
,a.ConsPC
,{this.CheckAndGetColumns(cuttingForm, "a.CutCellID")}
,{this.CheckAndGetColumns(cuttingForm, "a.CutNo")}
,{this.CheckAndGetColumns(cuttingForm, "a.CutPlanID")}
,{this.CheckAndGetColumns(cuttingForm, "a.Seq")}
,Article = Article_CONCAT
,a.CutRef
,a.EditDate
,a.EditName
,a.EstCutDate
,a.FabricCode
,a.FabricCombo
,a.FabricPanelCode
,a.FactoryID
,a.ID
,a.IsCreateByUser
,a.Layer
,a.MarkerLength
,a.MarkerName
,a.MarkerNo
--,a.MarkerVersion
,a.MDivisionID
,a.Order_EachconsUkey
,{this.CheckAndGetColumns(cuttingForm, "a.OrderID")}
, a.RefNo
,a.SCIRefNo
,a.Seq1
,a.Seq2
,{this.CheckAndGetColumns(cuttingForm, "a.SpreadingNoID")}
, a.Tone
,a.Ukey
,a.WKETA
,b.Description
,b.width
,dbo.MarkerLengthToYDS(a.MarkerLength) as yds 
,shc = iif(isnull(shc.RefNo,'')='','','Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension')
,oe.NoNotch
from {tableName} a WITH (NOLOCK)
Left Join Fabric b WITH (NOLOCK) on a.SciRefno = b.SciRefno
Left Join Order_EachCons oe with (nolock) on oe.Ukey = a.Order_EachconsUkey
outer apply(select RefNo from ShrinkageConcern where RefNo=a.RefNo and Junk=0) shc        
OUTER APPLY (
    SELECT Article_CONCAT = STUFF((
        SELECT DISTINCT CONCAT('/', Article)
        FROM {tbDistribute} WITH (NOLOCK)
        WHERE {tbUkey} = a.Ukey
        AND Article != ''
        FOR XML PATH ('')), 1, 1, '')
) Article_CONCAT    
Where {sqlWhereByType}
and a.id='{drInfoFrom["ID"]}'
{this.OrderByWithCheckColumns(cuttingForm, strOrderby)}
";

            DualResult dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out arrDtType[(int)TableType.WorkorderTb]);
            if (!dResult)
            {
                return dResult;
            }

            // 處理原Distribute相關欄位
            string tbDistributeColumns = string.Empty;
            string tbDistributeJoin = string.Empty;
            string tbDistributeWhere = "1=1";
            string tbDistributeOrderBy = string.Empty;
            if (isTbDistribute)
            {
                tbDistributeColumns = @"
,b.ID
,b.OrderID
,b.Article
,b.SizeCode
,b.Qty
,s.SewLineList
";
                tbDistributeJoin = $@"
inner join {tbDistribute} b WITH (NOLOCK) on  a.ukey = b.{tbUkey} 
outer apply(
	select SewLineList = Stuff((
		select concat('\',SewLine)
		from (
				select SewLine
				from dbo.orders d
				where exists(
					select 1 from {tbDistribute} where {tbUkey} = a.ukey
					and orderid = d.id
				)
			) s
		for xml path ('')
	) , 1, 1, '')
) s";
                tbDistributeOrderBy = "order by b.OrderID,b.Article,b.SizeCode";
            }
            else if (isTbSizeRatio)
            {
                // a = tableName
                tbDistributeColumns = @"
--,a.Ukey
,a.ID 
,a.OrderID
,a.Article
,b.SizeCode
,Qty = isnull(b.Qty,0) * isnull(a.Layer,0) --ForPlanning_SizeRatio.Qty * ForPlanning.Layer
,SewLineList = isnull(s.SewLine,'')
";
                tbDistributeJoin = $@"
inner join {tbSizeRatio} b WITH (NOLOCK) on  a.ukey = b.{tbUkey} 
left join (
    select d.ID,d.SewLine 
    from dbo.orders d) as s
	on s.ID=a.OrderID
";
                tbDistributeOrderBy = "order by a.OrderID,a.Article,b.SizeCode";
            }

            workorder_cmd = $@"
Select {sqlColByType},{this.CheckAndGetColumns(cuttingForm, "a.Cutno")},a.Colorid,a.Layer,a.Cons
{tbDistributeColumns}
from {tableName} a WITH (NOLOCK) 
{tbDistributeJoin}
Where {sqlWhereByType} and a.id='{drInfoFrom["ID"]}' and {tbDistributeWhere}
{tbDistributeOrderBy}";

            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out arrDtType[(int)TableType.WorkorderDisTb]);
            if (!dResult)
            {
                return dResult;
            }

            workorder_cmd = $"Select {sqlColByType},a.MarkerName,a.MarkerNo,a.MarkerLength,a.Cons,a.Layer,{this.CheckAndGetColumns(cuttingForm, "a.Cutno")},{this.CheckAndGetColumns(cuttingForm, "a.seq")}, a.colorid,a.FabricPanelCode,b.* from {tableName} a WITH (NOLOCK) ,{tbSizeRatio} b WITH (NOLOCK) ,Order_SizeCode c WITH (NOLOCK) Where {sqlWhereByType} and a.id='{drInfoFrom["ID"]}' and a.ukey = b.{tbUkey} and a.id = c.id and b.id = c.id and b.sizecode = c.sizecode order by c.seq";
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out arrDtType[(int)TableType.WorkorderSizeTb]);
            if (!dResult)
            {
                return dResult;
            }

            workorder_cmd = $"Select {sqlColByType},b.*,a.Markername,a.FabricPanelCode from {tableName} a,{tbPatternPanel} b Where {sqlWhereByType} and a.id='{drInfoFrom["ID"]}' and a.ukey = b.{tbUkey}";
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out arrDtType[(int)TableType.WorkorderPatternTb]);
            if (!dResult)
            {
                return dResult;
            }

            string sqlCutrefTb = $@"
    Select {sqlColByType},estCutDate{byType2},rn=ROW_NUMBER()over({this.OrderByWithCheckColumns(cuttingForm, strOrderby)}) into #tmp2 From #tmp

    {sqlFabricKind}

    select {sqlColByType},estCutDate{byType2} {sqlFabricKindinto} from #tmp2 group by {sqlColByType},estCutDate{byType2} order by min(rn)

    {sqlFabricKindjoin}
    ";
            dResult = MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderTb], "SpreadingNoID,CutCellID,Cutref,CutPlanID,estCutDate,shc,ukey,id,Article", sqlCutrefTb, out arrDtType[(int)TableType.CutrefTb]);
            if (!dResult)
            {
                return dResult;
            }

            dResult = MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderDisTb], $"{sqlColByType},OrderID,SewLineList", $@"Select distinct {sqlColByType},OrderID,SewLineList From #tmp", out arrDtType[(int)TableType.CutDisOrderIDTb]); // 整理sp，此處的OrderID是來自Orders

            if (!dResult)
            {
                return dResult;
            }

            dResult = MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderSizeTb], $"{sqlColByType},MarkerName,MarkerNo,MarkerLength,SizeCode,Cons,Qty,seq,FabricPanelCode", $"Select distinct {sqlColByType},MarkerName,MarkerNo,MarkerLength,SizeCode,Cons,Qty,seq,FabricPanelCode,dbo.MarkerLengthToYDS(MarkerLength) as yds From #tmp order by FabricPanelCode,MarkerName,seq", out arrDtType[(int)TableType.CutSizeTb]); // 整理SizeGroup,Qty

            if (!dResult)
            {
                return dResult;
            }

            dResult = MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderSizeTb], $"{sqlColByType},SizeCode,seq", $"Select distinct {sqlColByType},SizeCode,seq From #tmp order by seq ", out arrDtType[(int)TableType.SizeTb]); // 整理Size

            if (!dResult)
            {
                return dResult;
            }

            dResult = MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderSizeTb], $"{sqlColByType},MarkerName", $"Select distinct {sqlColByType},MarkerName From #tmp ", out arrDtType[(int)TableType.MarkerTB]); // 整理MarkerName

            if (!dResult)
            {
                return dResult;
            }

            dResult = MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderTb], $"{sqlColByType},FabricPanelCode,SCIRefno,shc", $"Select distinct {sqlColByType},a.FabricPanelCode,a.SCIRefno,b.Description,b.width,shc  From #tmp a Left Join Fabric b on a.SciRefno = b.SciRefno", out arrDtType[(int)TableType.FabricComboTb]); // 整理FabricPanelCode

            if (!dResult)
            {
                return dResult;
            }

            if (cuttingForm == CuttingForm.P02)
            {
                // 已限定是Plan，就不特別處理
                string issue_cmd = $"Select a.Cutplanid,b.Qty,b.Dyelot,b.Roll,Max(c.yds) as yds,c.Colorid from Issue a WITH (NOLOCK) ,Issue_Detail b WITH (NOLOCK) , #tmp c Where a.id=b.id and c.Cutplanid = a.Cutplanid and c.SEQ1 = b.SEQ1 and c.SEQ2 = b.SEQ2 group by a.Cutplanid,b.Qty,b.Dyelot,b.Roll,c.Colorid order by Dyelot,roll";
                MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderTb], "Cutplanid,SEQ1,SEQ2,yds,Colorid", issue_cmd, out arrDtType[(int)TableType.IssueTb]); // 整理FabricPanelCode
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// 取得各Table沒有的欄位清單
        /// </summary>
        private void GetDictionaryTableLostColumns()
        {
            string sql = $@"
--取出WorkOrderForPlanning的欄位
SELECT tbName=TABLE_NAME,colName = COLUMN_NAME
into #forPlan
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = N'WorkOrderForPlanning'
--取出WorkOrderForOutput的欄位
SELECT tbName=TABLE_NAME,colName = COLUMN_NAME
into #forOutput
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = N'WorkOrderForOutput'

--撈出各Table沒有的欄位
select tbName='WorkOrderForOutput',LostColName=A.colName
from #forPlan A
left join #forOutput B
on A.colName=B.colName
where B.colName is null
union
select tbName='WorkOrderForPlanning',LostColName=B.colName
from #forOutput B
left join #forPlan A
on B.colName=A.colName
where A.colName is null

drop table #forPlan,#forOutput
";
            DataTable dt;
            DBProxy.Current.Select(null, sql, out dt);
            var dic = dt.AsEnumerable()
                .GroupBy(r => r["tbName"].ToString())
                .ToDictionary(g => g.Key, g => g.Select(r => r["LostColName"].ToString()).ToList());
            var comparer = StringComparer.OrdinalIgnoreCase;
            this._dicTableLostColumns = new Dictionary<string, List<string>>(dic, comparer);
            #region 紀錄撈出的結果，方便查看，若資料結構有異動，可以同步補上
            /*
               tbName                  LostColName                  有無用到(在該cs檔Ctrl+F搜尋LostColName的值)
               WorkOrderForOutput      CutPlanID                    V
               WorkOrderForOutput      Remark                       X
               WorkOrderForOutput      Type                         X
               WorkOrderForPlanning    ActCuttingPerimeter          X
               WorkOrderForPlanning    CurvedLength                 X
               WorkOrderForPlanning    CutCellID                    V
               WorkOrderForPlanning    CutNo                        V
               WorkOrderForPlanning    CuttingMethod                X
               WorkOrderForPlanning    GroupID                      X
               WorkOrderForPlanning    MarkerVersion                X
               WorkOrderForPlanning    OrderID                      V
               WorkOrderForPlanning    Shift                        X
               WorkOrderForPlanning    SourceFrom                   X
               WorkOrderForPlanning    SpreadingNoID                V
               WorkOrderForPlanning    SpreadingRemark              X
               WorkOrderForPlanning    SpreadingStatus              X
               WorkOrderForPlanning    StraightLength               X
               WorkOrderForPlanning    UnfinishedCuttingReason      X
               WorkOrderForPlanning    UnfinishedCuttingRemark      X
               WorkOrderForPlanning    WorkOrderForPlanningUkey     X
            => CutPlanID, CutCellID, CutNo, OrderID, SpreadingNoID
            */
            #endregion
        }

        /// <summary>
        /// 檢查目標Table是否無該欄位
        /// </summary>
        /// <param name="cuttingForm">From P02、P09</param>
        /// <param name="column">檢查的Column</param>
        /// <returns>true代表無該欄位</returns>
        public bool CheckTableLostColumns(CuttingForm cuttingForm, string column)
        {
            string tableNameWorkOrder = GetTableName(cuttingForm);
            return this.DicTableLostColumns.ContainsKey(tableNameWorkOrder)
                && this.DicTableLostColumns[tableNameWorkOrder].FindIndex(x => x.Equals(column, StringComparison.OrdinalIgnoreCase)) != -1;
        }

        /// <summary>
        /// 檢查若不存在的欄位則補上設定空字串
        /// </summary>
        /// <param name="cuttingForm">From P02、P09</param>
        /// <param name="column">輸入欄位</param>
        /// <returns>回傳欄位</returns>
        private string CheckAndGetColumns(CuttingForm cuttingForm, string column)
        {
            string checkColumn = column.Contains(".") ? column.Split('.')[1] : column;
            return this.CheckTableLostColumns(cuttingForm, checkColumn) ? checkColumn + "=''" : column;
        }

        /// <summary>
        /// 檢查Order By的欄位是否存在，並重組出Order By字串
        /// </summary>
        /// <param name="cuttingForm">From P02、P09</param>
        /// <param name="orderBy">整串Order By的字串(輸入檢查)</param>
        /// <returns>整串Order By的字串(輸出回傳)</returns>
        private string OrderByWithCheckColumns(CuttingForm cuttingForm, string orderBy)
        {
            orderBy = Regex.Replace(orderBy, "order by", string.Empty, RegexOptions.IgnoreCase);
            List<string> listOrderByColumns = orderBy.Split(',').ToList();
            List<string> listResult = new List<string>();
            foreach (var item in listOrderByColumns)
            {
                var column = Regex.Replace(item, "asc", string.Empty, RegexOptions.IgnoreCase);
                column = Regex.Replace(column, "desc", string.Empty, RegexOptions.IgnoreCase);
                column = column.Trim();
                if (!this.CheckTableLostColumns(cuttingForm, column))
                {
                    listResult.Add(item);
                }
            }

            return "order by " + string.Join(",", listResult);
        }

        /// <summary>
        /// 取得Table名稱
        /// </summary>
        /// <param name="cuttingForm">From P02、P09</param>
        /// <returns>string</returns>
        private static string GetTableName(CuttingForm cuttingForm)
        {
            return cuttingForm == CuttingForm.P02 ? "WorkOrderForPlanning" : "WorkOrderForOutput";
        }

        private bool CheckTableExist(string tableName)
        {
            string sql = $@"SELECT distinct 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = N'{tableName}'";
            return mySqlClient.FindRow(null, sql) != null;
        }

        /// <summary>
        /// 將Print資料輸出Excel報表
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="arrDtType">資料表陣列，可由GetPrintData產出</param>
        /// <param name="cuttingForm">From P02、P09</param>
        /// <param name="printType">列印哪種報表</param>
        /// <param name="errMsg">錯誤訊息</param>
        /// <returns>bool</returns>
        public bool PrintToExcel(string id, DataTable[] arrDtType, CuttingForm cuttingForm, string printType, out string errMsg)
        {
            bool result;
            if (printType == "Cutref")
            {
                result = this.ByCutrefExcel(id, arrDtType, cuttingForm, out errMsg);
            }
            else
            {
                result = this.ByRequestExcel(id, arrDtType, out errMsg);
            }

            return result;
        }

        /// <summary>
        /// By Request Excel (Only For Cutting P02，因此CutPlanID必定有值 => 可參考GetDictionaryTableLostColumns)
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="arrDtType">arrDtType</param>
        /// <param name="errMsg">errMsg</param>
        /// <returns>bool</returns>
        private bool ByRequestExcel(string id, DataTable[] arrDtType, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                string strXltName = Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyRequest.xltx";
                DataRow orderDr;
                MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) Where id='{0}'", id), out orderDr);
                Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                excel.Visible = false;
                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                #region 寫入共用欄位
                worksheet.Cells[1, 6] = orderDr["factoryid"];
                worksheet.Cells[3, 2] = DateTime.Now.ToShortDateString();

                // P02 不會有Spreading No、Cut Cell
                worksheet.Cells[9, 2] = orderDr["Styleid"];
                worksheet.Cells[10, 2] = orderDr["Seasonid"];
                worksheet.Cells[9, 13] = arrDtType[(int)TableType.CutDisOrderIDTb].Rows[0]["SewLineList"].ToString();

                for (int nColumn = 3; nColumn <= 21; nColumn += 3)
                {
                    worksheet.Cells[36, nColumn] = orderDr["Styleid"];
                    worksheet.Cells[37, nColumn] = id;
                }

                int nSheet = 1;
                string spList = string.Empty;
                DataRow[] workorderArry;
                DataRow[] workorderDisArry;
                DataRow[] workorderSizeArry;
                DataRow[] workorderPatternArry;
                DataRow[] workorderOrderIDArry;
                DataRow[] sizeArry;
                DataRow[] sizeCodeArry;
                DataRow[] markerArry;
                DataRow[] fabricComboArry;
                DataRow[] issueArry;
                string pattern = string.Empty, line = string.Empty;
                string size = string.Empty, ratio = string.Empty;
                int totConsRowS = 19, totConsRowE = 20, nSizeColumn = 0;
                foreach (DataRow cutrefdr1 in arrDtType[(int)TableType.CutrefTb].Rows)
                {
                    spList = string.Empty;

                    // 有兩筆以上才做其他Sheet
                    if (nSheet >= 2)
                    {
                        worksheet = excel.ActiveWorkbook.Worksheets[nSheet - 1];
                        worksheet.Copy(Type.Missing, worksheet);
                    }

                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                    worksheet.Select();
                    worksheet.Name = cutrefdr1["Cutplanid"].ToString() + "-" + MyUtility.Convert.GetDate(cutrefdr1["Estcutdate"]).Value.ToString("yyyy-MM-dd");
                    worksheet.Cells[3, 19] = cutrefdr1["Cutplanid"].ToString();
                    worksheet.Cells[8, 13] = ((DateTime)MyUtility.Convert.GetDate(cutrefdr1["Estcutdate"])).ToShortDateString();
                    nSheet++;
                }

                nSheet = 1;
                #endregion
                foreach (DataRow cutrefdr in arrDtType[(int)TableType.CutrefTb].Rows)
                {
                    spList = string.Empty;
                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                    worksheet.Select();
                    string cutplanid = cutrefdr["Cutplanid"].ToString();
                    #region 撈表身Detail Array
                    workorderArry = arrDtType[(int)TableType.WorkorderTb].Select(string.Format("Cutplanid = '{0}'", cutplanid));
                    workorderDisArry = arrDtType[(int)TableType.WorkorderDisTb].Select(string.Format("Cutplanid='{0}'", cutplanid));
                    workorderSizeArry = arrDtType[(int)TableType.WorkorderSizeTb].Select(string.Format("Cutplanid='{0}'", cutplanid));
                    workorderOrderIDArry = arrDtType[(int)TableType.CutDisOrderIDTb].Select(string.Format("Cutplanid='{0}'", cutplanid), "Orderid");
                    fabricComboArry = arrDtType[(int)TableType.FabricComboTb].Select(string.Format("Cutplanid='{0}'", cutplanid));
                    sizeCodeArry = arrDtType[(int)TableType.SizeTb].Select(string.Format("Cutplanid='{0}'", cutplanid), "SEQ");
                    markerArry = arrDtType[(int)TableType.MarkerTB].Select(string.Format("Cutplanid = '{0}'", cutplanid));
                    issueArry = arrDtType[(int)TableType.IssueTb].Select(string.Format("Cutplanid = '{0}'", cutplanid));
                    #endregion

                    if (workorderArry.Length > 0)
                    {
                        worksheet.Cells[3, 7] = workorderArry[0]["CutCellid"].ToString();
                        worksheet.Cells[3, 12] = string.Empty;
                        #region 從後面開始寫 先寫Refno,Color

                        for (int nColumn = 3; nColumn <= 22; nColumn += 3)
                        {
                            worksheet.Cells[33, nColumn] = workorderArry[0]["Refno"];
                            worksheet.Cells[34, nColumn] = workorderArry[0]["Colorid"];
                        }
                        #endregion
                    }

                    int copyRow = 0;
                    int rowRange = 6;
                    int tmpn = 13;
                    if (fabricComboArry.Length > 0)
                    {
                        foreach (DataRow fabricComboDr in fabricComboArry)
                        {
                            if (copyRow > 0)
                            {
                                Excel.Range r = worksheet.get_Range("A" + (12 + (rowRange * (copyRow - 1))).ToString(), "A" + ((12 + (rowRange * (copyRow - 1))) + rowRange - 1).ToString()).EntireRow;
                                r.Copy();
                                r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); // 新增Row
                            }

                            workorderPatternArry = arrDtType[(int)TableType.WorkorderPatternTb].Select(string.Format("Cutplanid='{0}' and FabricPanelCode = '{1}'", cutplanid, fabricComboDr["FabricPanelCode"]), "PatternPanel");
                            pattern = string.Empty;
                            if (workorderPatternArry.Length > 0)
                            {
                                foreach (DataRow patDr in workorderPatternArry)
                                {
                                    if (!patDr["PatternPanel"].ToString().InList(pattern))
                                    {
                                        pattern = pattern + patDr["PatternPanel"].ToString() + ",";
                                    }
                                }
                            }

                            int fabricRow = 12 + (rowRange * copyRow);
                            worksheet.Cells[fabricRow, 2] = fabricComboDr["FabricPanelCode"].ToString();
                            worksheet.Cells[fabricRow, 5] = pattern;

                            string fd = fabricComboDr["Description"].ToString();
                            worksheet.Cells[fabricRow, 9] = fd;
                            int fl = 48;
                            int fla = fd.Length / fl;
                            for (int i = 1; i <= fla; i++)
                            {
                                if (fd.Length > fl * i)
                                {
                                    Excel.Range rangeRow13 = (Excel.Range)worksheet.Rows[13, Type.Missing];
                                    rangeRow13.RowHeight = 19.125 * (i + 1);
                                }
                            }

                            worksheet.Cells[fabricRow, 19] = fabricComboDr["width"].ToString();
                            copyRow++;
                        }
                    }

                    #region OrderSP List, Line List
                    if (workorderOrderIDArry.Length > 0)
                    {
                        foreach (DataRow disDr in workorderOrderIDArry)
                        {
                            if (disDr["OrderID"].ToString() != "EXCESS")
                            {
                                if (!disDr["OrderID"].ToString().InList(spList, "\\"))
                                {
                                    spList = spList + disDr["OrderID"].ToString() + "\\";
                                }
                            }
                            #region SewingLine
                            line = line + MyUtility.GetValue.Lookup("Sewline", disDr["OrderID"].ToString(), "Orders", "ID") + "\\";
                            #endregion
                        }

                        worksheet.Cells[8, 2] = spList;
                        worksheet.Cells[9, 13] = arrDtType[(int)TableType.CutDisOrderIDTb].Rows[0]["SewLineList"].ToString();
                        int l = 54;
                        int la = spList.Length / l;
                        for (int i = 1; i <= la; i++)
                        {
                            if (spList.Length > l * i)
                            {
                                Excel.Range rangeRow8 = (Excel.Range)worksheet.Rows[8, Type.Missing];
                                rangeRow8.RowHeight = 20.25 * (i + 1);
                            }
                        }
                    }
                    #endregion

                    #region Markname
                    int nRow = 11;

                    if (markerArry.Length > 0)
                    {
                        size = string.Empty;
                        ratio = string.Empty;
                        #region Size,Ratio
                        foreach (DataRow markerDr in markerArry)
                        {
                            Excel.Range r = worksheet.get_Range("A" + nRow.ToString(), "A" + nRow.ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); // 新增Row
                            nRow++;

                            sizeArry = arrDtType[(int)TableType.CutSizeTb].Select(string.Format("Cutplanid='{0}' and MarkerName = '{1}'", cutplanid, markerDr["MarkerName"]));
                            if (sizeArry.Length > 0)
                            {
                                size = string.Empty;
                                ratio = string.Empty;
                                foreach (DataRow sizeDr in sizeArry)
                                {
                                    size = size + sizeDr["SizeCode"].ToString() + ",";
                                    ratio = ratio + MyUtility.Convert.GetDouble(sizeDr["Qty"]).ToString() + ",";
                                }

                                double unit = Convert.ToDouble(sizeArry[0]["yds"]) * 0.9144;
                                worksheet.Cells[nRow, 1] = sizeArry[0]["MarkerName"].ToString();
                                worksheet.Cells[nRow, 4] = sizeArry[0]["MarkerNo"].ToString();
                                worksheet.Cells[nRow, 6] = sizeArry[0]["MarkerLength"].ToString() + "\n" + sizeArry[0]["yds"].ToString() + "Y (" + unit + "M)";
                            }

                            worksheet.Cells[nRow, 10] = size;
                            worksheet.Cells[nRow, 12] = ratio;

                            int l = 11;
                            int la = size.Length / l;
                            int la2 = ratio.Length / l;
                            for (int i = 1; i <= la; i++)
                            {
                                if (size.Length > l * i)
                                {
                                    Excel.Range rangeRow12 = (Excel.Range)worksheet.Rows[nRow, Type.Missing];
                                    rangeRow12.RowHeight = 16.875 * (i + 1);
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                    tmpn = nRow + 2;
                    nRow = nRow + 3; // Size
                    string str_PIVOT = string.Empty;
                    nSizeColumn = 4;
                    DataRow[] fabricComboTbsia = arrDtType[(int)TableType.FabricComboTb].Select(string.Format("Cutplanid = '{0}'", cutplanid));
                    List<string> sizeCodeDistinct = sizeCodeArry.AsEnumerable().Select(dr => dr["SizeCode"].ToString()).Distinct().ToList();
                    foreach (string s in sizeCodeDistinct)
                    {
                        str_PIVOT = str_PIVOT + $@"[{s}],";

                        // 寫入Size
                        for (int i = 0; i < fabricComboTbsia.Length; i++)
                        {
                            worksheet.Cells[nRow + (rowRange * i), nSizeColumn] = s;
                        }

                        nSizeColumn++;
                    }

                    str_PIVOT = str_PIVOT.Substring(0, str_PIVOT.Length - 1);
                    string pivot_cmd = string.Format(
                    @"Select * From
                (
                    Select FabricPanelCode,MarkerName,Cutno,Colorid,SizeCode,Cons,Layer,Seq,(Qty*Layer) as TotalQty from 
                    #tmp
                    Where Cutplanid = '{0} '
                ) as mTb
                Pivot(Sum(TotalQty)
                for SizeCode in ({1})) as pIvT 
                order by FabricPanelCode,Cutno,Colorid",
                    cutplanid,
                    str_PIVOT);
                    if (arrDtType[(int)TableType.CutQtyTb] != null)
                    {
                        arrDtType[(int)TableType.CutQtyTb].Clear();
                    }

                    MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderSizeTb], "FabricPanelCode,MarkerName,Cutno,Colorid,SizeCode,Qty,Layer,Cutplanid,Cons,Seq", pivot_cmd, out arrDtType[(int)TableType.CutQtyTb]);
                    nRow = nRow + 1;
                    bool lfirstComb = true;
                    string fabColor = string.Empty;
                    DataRow[] fabricComboTbsi = arrDtType[(int)TableType.FabricComboTb].Select(string.Format("Cutplanid = '{0}'", cutplanid));
                    foreach (DataRow fabricComboDr1 in fabricComboTbsi)
                    {
                        if (!MyUtility.Check.Empty(fabricComboDr1["shc"]))
                        {
                            Excel.Range rng = (Excel.Range)worksheet.Rows[tmpn, Type.Missing];
                            rng.Insert(Excel.XlDirection.xlDown);
                            Excel.Range rng2 = (Excel.Range)worksheet.get_Range("I" + tmpn, "U" + tmpn);
                            rng2.Merge();
                            rng2.Cells.Font.Color = Color.Red;
                            rng2.Cells.Font.Bold = true;
                            worksheet.Cells[tmpn, 9] = fabricComboDr1["shc"].ToString();
                            tmpn++;
                            nRow++;
                        }

                        tmpn += 6;
                        DataRow[] cutQtyArray = arrDtType[(int)TableType.CutQtyTb].Select(string.Format("FabricPanelCode = '{0}'", fabricComboDr1["FabricPanelCode"]));
                        if (cutQtyArray.Length > 0)
                        {
                            int copyrow = 0;
                            nRow = lfirstComb ? nRow : nRow + 4;
                            lfirstComb = false;
                            totConsRowS = nRow; // 第一個Cons
                            foreach (DataRow cutqtydr in cutQtyArray)
                            {
                                if (copyrow > 0)
                                {
                                    Excel.Range r = worksheet.get_Range("A" + nRow.ToString(), "A" + nRow.ToString()).EntireRow;
                                    r.Copy();
                                    r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
                                    tmpn++;
                                }

                                worksheet.Cells[nRow, 1] = cutqtydr["Seq"].ToString();
                                worksheet.Cells[nRow, 2] = cutqtydr["Colorid"].ToString();
                                worksheet.Cells[nRow, 3] = cutqtydr["Layer"].ToString();
                                worksheet.Cells[nRow, 20] = cutqtydr["Cons"].ToString();
                                fabColor = cutqtydr["Colorid"].ToString();

                                int col = 0;
                                foreach (string s in sizeCodeDistinct)
                                {
                                    var currentSeq = sizeCodeArry.AsEnumerable()
                                        .Where(o => o["Seq"].ToString() == cutqtydr["Seq"].ToString() && o["SizeCode"].ToString() == s)
                                        .Select(o => new { Seq = o["Seq"].ToString(), SizeCode = o["SizeCode"].ToString() });
                                    if (currentSeq.Any())
                                    {
                                        // cutqtydr[7 + col].ToString() 等同於 currentSeq找到的第一筆
                                        worksheet.Cells[nRow, col + 4] = cutqtydr[7 + col].ToString();
                                    }

                                    col++;
                                }

                                nRow++;
                                copyrow++;
                            }

                            totConsRowE = nRow; // 最後一個Cons
                            #region Total Cons
                            nRow = nRow + 1;
                            worksheet.Cells[nRow, 20] = string.Format("=SUM(T{0}:T{1})", totConsRowS, totConsRowE);
                            worksheet.Cells[nRow, 18] = fabColor;
                            #endregion
                        }
                    }

                    nRow = nRow + 4; // Roll Table
                    #region Issue Roll,Dyelot
                    if (issueArry.Length > 0)
                    {
                        bool lfirstdr = true;
                        foreach (DataRow issueDr in issueArry)
                        {
                            if (!lfirstdr)
                            {
                                Excel.Range r = worksheet.get_Range("A" + nRow.ToString(), "A" + nRow.ToString()).EntireRow;
                                r.Copy();
                                r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
                            }

                            lfirstdr = false;
                            worksheet.Cells[nRow, 1] = issueDr["Roll"].ToString();
                            worksheet.Cells[nRow, 2] = issueDr["Colorid"].ToString();
                            worksheet.Cells[nRow, 4] = issueDr["Dyelot"].ToString();
                            worksheet.Cells[nRow, 6] = issueDr["Qty"].ToString();

                            nRow++;
                        }
                    }
                    #endregion

                    nSheet++;
                }

                // 重製Mode以取消Copy區塊
                worksheet.Application.CutCopyMode = Excel.XlCutCopyMode.xlCopy;

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Cutting_P02_SpreadingReportbyRequest");
                Excel.Workbook workbook = excel.Workbooks[1];
                workbook.SaveAs(strExcelName);
                workbook.Close();
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// By Cutref Excel (Cutting P02、P09皆可用)
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="arrDtType">arrDtType</param>
        /// <param name="cuttingForm">CuttingForm</param>
        /// <param name="errMsg">errMsg</param>
        /// <returns>bool</returns>
        private bool ByCutrefExcel(string id, DataTable[] arrDtType, CuttingForm cuttingForm, out string errMsg)
        {
            errMsg = string.Empty;
            string tableName = GetTableName(cuttingForm);
            string tbDistribute = tableName + "_Distribute";
            string tbUkey = tableName + "Ukey";
            bool isTbDistribute = this.CheckTableExist(tbDistribute);
            try
            {
                bool isP02 = cuttingForm == CuttingForm.P02;
                int nSizeColumn;
                int sheetCount = arrDtType[(int)TableType.CutrefTb].Rows.Count;
                if (sheetCount == 0)
                {
                    errMsg = "Data not found!";
                    return false;
                }

                DataRow orderDr;
                MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) Where id='{0}'", id), out orderDr);

                #region By Cutref
                string strXltName = Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyCutref.xltx";
                Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                #region 寫入共用欄位
                worksheet.Cells[1, 6] = orderDr["factoryid"];
                worksheet.Cells[3, 2] = DateTime.Now.ToShortDateString();

                if (isP02)
                {
                    worksheet.Cells[3, 5] = "Cut Cell:";
                    worksheet.Cells[3, 10] = string.Empty;
                }

                worksheet.Cells[9, 2] = orderDr["Styleid"];
                worksheet.Cells[10, 2] = orderDr["Seasonid"];
                worksheet.Cells[9, 13] = arrDtType[(int)TableType.CutDisOrderIDTb].Rows[0]["SewLineList"].ToString();

                for (int nColumn = 3; nColumn <= 21; nColumn += 3)
                {
                    worksheet.Cells[40, nColumn] = orderDr["Styleid"];
                    worksheet.Cells[41, nColumn] = id;
                }

                #endregion

                int nSheet = 1;
                string spList = string.Empty;
                DataRow[] workorderArry;
                DataRow[] workorderDisArry = null;
                DataRow[] workorderSizeArry;
                DataRow[] workorderPatternArry;
                DataRow[] workorderOrderIDArry = null;
                DataRow[] sizeArry;
                DataRow[] sizeCodeArry;
                string pattern = string.Empty, line = string.Empty;
                int nDisCount = 0;
                double disRow = 0;
                string size = string.Empty, ratio = string.Empty;
                int totConsRowS = 18, totConsRowE = 19;

                foreach (DataRow cutrefdr in arrDtType[(int)TableType.CutrefTb].Rows)
                {
                    spList = string.Empty;

                    // 有兩筆以上才做其他Sheet
                    if (nSheet >= 2)
                    {
                        worksheet = excel.ActiveWorkbook.Worksheets[nSheet - 1];
                        worksheet.Copy(Type.Missing, worksheet);
                    }

                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                    worksheet.Select();
                    worksheet.Name = cutrefdr["Cutref"].ToString();
                    worksheet.Cells[3, 18] = cutrefdr["Cutref"].ToString();
                    worksheet.Cells[8, 13] = MyUtility.Check.Empty(cutrefdr["Estcutdate"]) == false ? ((DateTime)MyUtility.Convert.GetDate(cutrefdr["Estcutdate"])).ToShortDateString() : string.Empty;
                    worksheet.Cells[14, 14] = MyUtility.Convert.GetString(cutrefdr["FabricKind"]);

                    nSheet++;
                }

                // 右上角QR Code
                nSheet = 1;
                foreach (DataRow cutrefdr1 in arrDtType[(int)TableType.CutrefTb].Rows)
                {
                    Bitmap cutRefQRCode = this.NewQRcode(MyUtility.Convert.GetString(cutrefdr1["Cutref"].ToString()));
                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                    Excel.Range rng = worksheet.Range["T2:U3"];

                    // 將圖片存儲為暫時檔案
                    string tempFilePath = System.IO.Path.GetTempFileName();
                    cutRefQRCode.Save(tempFilePath);

                    // 將圖片插入到工作表中的指定位置
                    Excel.Shape pictureShape = worksheet.Shapes.AddPicture(
                        tempFilePath,
                        Microsoft.Office.Core.MsoTriState.msoFalse,
                        Microsoft.Office.Core.MsoTriState.msoCTrue,
                        (float)rng.Left,
                        (float)rng.Top,
                        (float)rng.Height,
                        (float)rng.Height);

                    // 刪除暫時檔案
                    System.IO.File.Delete(tempFilePath);

                    nSheet++;
                }

                nSheet = 1;
                foreach (DataRow cutrefdr2 in arrDtType[(int)TableType.CutrefTb].Rows)
                {
                    spList = string.Empty;
                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                    worksheet.Select();
                    string cutref = cutrefdr2["Cutref"].ToString();
                    #region 撈表身Detail Array
                    workorderArry = arrDtType[(int)TableType.WorkorderTb].Select(string.Format("Cutref = '{0}'", cutref));
                    workorderDisArry = arrDtType[(int)TableType.WorkorderDisTb].Select(string.Format("Cutref='{0}'", cutref));

                    workorderSizeArry = arrDtType[(int)TableType.WorkorderSizeTb].Select(string.Format("Cutref='{0}'", cutref));
                    workorderPatternArry = arrDtType[(int)TableType.WorkorderPatternTb].Select(string.Format("Cutref='{0}'", cutref), "PatternPanel");
                    workorderOrderIDArry = arrDtType[(int)TableType.CutDisOrderIDTb].Select(string.Format("Cutref='{0}'", cutref), "Orderid");

                    sizeArry = arrDtType[(int)TableType.CutSizeTb].DefaultView.ToTable(true, new string[] { "Cutref", "SizeCode" }).Select(string.Format("Cutref='{0}'", cutref));
                    sizeCodeArry = arrDtType[(int)TableType.SizeTb].Select(string.Format("Cutref='{0}'", cutref), "SEQ");
                    #endregion

                    if (workorderArry.Length > 0)
                    {
                        pattern = string.Empty;
                        worksheet.Cells[13, 2] = workorderArry[0]["FabricPanelCode"].ToString();

                        if (isP02)
                        {
                            worksheet.Cells[3, 7] = workorderArry[0]["CutCellid"].ToString();
                        }
                        else
                        {
                            worksheet.Cells[3, 7] = workorderArry[0]["SpreadingNoID"].ToString();
                            worksheet.Cells[3, 12] = workorderArry[0]["CutCellid"].ToString();
                        }

                        worksheet.Cells[22, 2] = workorderArry[0]["Tone"].ToString();
                        if (workorderPatternArry.Length > 0)
                        {
                            foreach (DataRow patDr in workorderPatternArry)
                            {
                                if (!patDr["PatternPanel"].ToString().InList(pattern))
                                {
                                    pattern = pattern + patDr["PatternPanel"].ToString() + ",";
                                }
                            }
                        }

                        worksheet.Cells[13, 2] = workorderArry[0]["FabricPanelCode"].ToString();
                        worksheet.Cells[13, 5] = pattern;
                        string fd = "#" + workorderArry[0]["SCIRefno"].ToString().Trim() + " " + workorderArry[0]["Description"].ToString();
                        worksheet.Cells[13, 9] = fd;
                        int fl = 48;
                        int fla = fd.Length / fl;
                        for (int i = 1; i <= fla; i++)
                        {
                            if (fd.Length > fl * i)
                            {
                                Excel.Range rangeRow13 = (Excel.Range)worksheet.Rows[13, Type.Missing];
                                rangeRow13.RowHeight = 19.125 * (i + 1);
                            }
                        }

                        worksheet.Cells[13, 20] = workorderArry[0]["width"].ToString();
                        #region 從後面開始寫 先寫Refno,Color

                        for (int nColumn = 3; nColumn <= 22; nColumn += 3)
                        {
                            worksheet.Cells[37, nColumn] = workorderArry[0]["Refno"];
                            worksheet.Cells[38, nColumn] = workorderArry[0]["Colorid"];
                        }
                        #endregion
                    }
                    #region OrderSP List, Line List
                    if (workorderOrderIDArry != null && workorderOrderIDArry.Length > 0)
                    {
                        foreach (DataRow disDr in workorderOrderIDArry)
                        {
                            if (disDr["OrderID"].ToString() != "EXCESS")
                            {
                                spList = spList + disDr["OrderID"].ToString() + "\\";
                            }
                            #region SewingLine
                            line = line + MyUtility.GetValue.Lookup("Sewline", disDr["OrderID"].ToString(), "Orders", "ID") + "\\";
                            #endregion
                        }

                        worksheet.Cells[8, 2] = spList;
                        int l = 54;
                        int la = spList.Length / l;
                        for (int i = 1; i <= la; i++)
                        {
                            if (spList.Length <= l * i)
                            {
                                continue;
                            }

                            double rowHeight = 20.25 * (i + 1);
                            Excel.Range rangeRow8 = (Excel.Range)worksheet.Rows[8, Type.Missing];

                            // row高只能到409
                            if (rowHeight > 409)
                            {
                                rangeRow8.RowHeight = 409;
                                break;
                            }

                            rangeRow8.RowHeight = 20.25 * (i + 1);
                        }
                    }
                    #endregion
                    #region Markname
                    int nrow = 12;

                    if (sizeArry.Length > 0)
                    {
                        size = string.Empty;
                        ratio = string.Empty;
                        #region Size,Ratio
                        DataRow[] wtmp = arrDtType[(int)TableType.WorkorderSizeTb].DefaultView.ToTable(false, new string[] { "Cutref", "SizeCode" }).Select(string.Format("Cutref='{0}'", cutref));
                        DataRow[] wtmp2 = arrDtType[(int)TableType.WorkorderSizeTb].DefaultView.ToTable(false, new string[] { "Cutref", "Qty" }).Select(string.Format("Cutref='{0}'", cutref));
                        foreach (DataRow sDr in wtmp)
                        {
                            size = size + sDr["SizeCode"].ToString() + ",";
                        }

                        foreach (DataRow sDr in wtmp2)
                        {
                            ratio = ratio + MyUtility.Convert.GetDouble(sDr["Qty"]).ToString() + ",";
                        }
                        #endregion
                        double unit = Convert.ToDouble(workorderArry[0]["yds"]) * 0.9144;
                        string markerNo = MyUtility.Convert.GetString(workorderArry[0]["MarkerNo"]);
                        string markerNo2 = string.Empty;
                        if (markerNo.Length >= 2)
                        {
                            markerNo2 = markerNo.Substring(markerNo.Length - 2);
                        }

                        worksheet.Cells[12, 1] = workorderArry[0]["MarkerName"].ToString();
                        worksheet.Cells[12, 3] = markerNo2;
                        worksheet.Cells[12, 4] = markerNo;
                        worksheet.Cells[12, 6] = workorderArry[0]["MarkerLength"].ToString() + "\n" + workorderArry[0]["yds"].ToString() + "Y (" + unit + "M)";
                        worksheet.Cells[12, 10] = size;
                        worksheet.Cells[12, 12] = ratio;
                        if (!MyUtility.Convert.GetBool(workorderArry[0]["NoNotch"]))
                        {
                            worksheet.Cells[12, 16] = string.Empty;
                        }

                        int l = 11;
                        int la = size.Length / l;
                        int la2 = ratio.Length / l;
                        for (int i = 1; i <= la; i++)
                        {
                            if (size.Length > l * i)
                            {
                                Excel.Range rangeRow12 = (Excel.Range)worksheet.Rows[12, Type.Missing];
                                rangeRow12.RowHeight = 16.875 * (i + 1);
                            }
                        }
                    }
                    #endregion

                    #region Distribute to SP#
                    if (workorderDisArry.Length > 0)
                    {
                        nrow = 16; // 到Distribute ROW
                        nDisCount = workorderDisArry.Length;
                        disRow = Math.Ceiling(Convert.ToDouble(nDisCount) / 2); // 每一個Row 有兩個可以用
                        int arrayrow = 0;
                        for (int i = 0; i < disRow; i++)
                        {
                            if (i > 0)
                            {
                                Excel.Range r = worksheet.get_Range("A" + (nrow - 1).ToString(), "A" + (nrow - 1).ToString()).EntireRow;
                                r.Copy();
                                r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
                            }

                            arrayrow = i * 2;
                            worksheet.Cells[nrow, 1] = workorderDisArry[arrayrow]["OrderID"].ToString();
                            worksheet.Cells[nrow, 4] = workorderDisArry[arrayrow]["Article"].ToString();
                            worksheet.Cells[nrow, 7] = workorderDisArry[arrayrow]["SizeCode"].ToString();
                            worksheet.Cells[nrow, 9] = workorderDisArry[arrayrow]["Qty"].ToString();
                            if (arrayrow + 1 < nDisCount)
                            {
                                worksheet.Cells[nrow, 11] = workorderDisArry[arrayrow + 1]["OrderID"].ToString();
                                worksheet.Cells[nrow, 14] = workorderDisArry[arrayrow + 1]["Article"].ToString();
                                worksheet.Cells[nrow, 17] = workorderDisArry[arrayrow + 1]["SizeCode"].ToString();
                                worksheet.Cells[nrow, 19] = workorderDisArry[arrayrow + 1]["Qty"].ToString();
                            }
                            else
                            {
                                worksheet.Cells[nrow, 11] = string.Empty;
                                worksheet.Cells[nrow, 14] = string.Empty;
                                worksheet.Cells[nrow, 17] = string.Empty;
                                worksheet.Cells[nrow, 19] = string.Empty;
                            }

                            nrow++;
                        }

                        // nrow = nrow + Convert.ToInt16(disRow);
                    }
                    #endregion

                    string str_PIVOT = string.Empty;
                    nSizeColumn = 4;
                    string pivot_cmd = string.Empty;
                    DualResult drwst;
                    foreach (DataRow dr in sizeArry)
                    {
                        str_PIVOT = str_PIVOT + string.Format("[{0}],", dr["SizeCode"].ToString());

                        // 寫入Size
                        worksheet.Cells[nrow + 1, nSizeColumn] = dr["SizeCode"].ToString();
                        nSizeColumn++;
                    }

                    str_PIVOT = str_PIVOT.Substring(0, str_PIVOT.Length - 1);

                    pivot_cmd =
                        $@"
Select Cutno,Seq,Colorid,SizeCode,Cons,Layer,{tbUkey},(Qty*Layer) as TotalQty from 
#tmp
Where Cutref = '{cutref}'";

                    if (arrDtType[(int)TableType.CutQtyTb] != null)
                    {
                        arrDtType[(int)TableType.CutQtyTb].Clear();
                    }

                    drwst = MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderSizeTb], $"Cutno,Seq,Colorid,SizeCode,Qty,Layer,Cutref,Cons,{tbUkey}", pivot_cmd, out arrDtType[(int)TableType.CutQtyTb]);
                    if (!drwst)
                    {
                        MyUtility.Msg.ErrorBox("SQL command Pivot_cmd error!");
                        return false;
                    }

                    nrow = nrow + 2;
                    int copyrow = 0;
                    totConsRowS = nrow; // 第一個Cons

                    var tmpCutQtyTB = arrDtType[(int)TableType.CutQtyTb].AsEnumerable();
                    var distinct_CutQtyTb = from r1 in tmpCutQtyTB
                                            group r1 by new
                                            {
                                                Cutno = MyUtility.Convert.GetString(r1["Cutno"]),
                                                Seq = MyUtility.Convert.GetInt(r1["Seq"]),
                                                Colorid = r1.Field<string>("Colorid"),
                                                Layer = r1.Field<int>("Layer"),
                                                workorderukey_CuttingForm = r1.Field<int>(tbUkey),
                                                Cons = r1.Field<decimal>("Cons"),
                                            }
                                            into g
                                            select new
                                            {
                                                g.Key.Cutno,
                                                g.Key.Seq,
                                                g.Key.Colorid,
                                                g.Key.Layer,
                                                g.Key.workorderukey_CuttingForm,
                                                g.Key.Cons,
                                            };
                    if (cuttingForm == CuttingForm.P09)
                    {
                        var range = worksheet.Range[worksheet.Cells[nrow - 2, 1], worksheet.Cells[nrow - 1, 1]];
                        range.Merge();
                        range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;  // 水平置中
                        range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;    // 垂直置中
                        range.Value = "Cut#";

                        var range2 = worksheet.Range[worksheet.Cells[nrow - 2, 2], worksheet.Cells[nrow - 1, 2]];
                        range2.Merge();
                        range2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;  // 水平置中
                        range2.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;    // 垂直置中
                        range2.Value = "Color";
                    }
                    else if (cuttingForm == CuttingForm.P02)
                    {
                        var range = worksheet.Range[worksheet.Cells[nrow - 2, 1], worksheet.Cells[nrow - 1, 1]];
                        range.Merge();
                        range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;  // 水平置中
                        range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;    // 垂直置中
                        range.Value = "Seq";

                        var range2 = worksheet.Range[worksheet.Cells[nrow - 2, 2], worksheet.Cells[nrow - 1, 2]];
                        range2.Merge();
                        range2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;  // 水平置中
                        range2.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;    // 垂直置中
                        range2.Value = "Color";
                    }
                    else
                    {
                        // 合併並把文字置中
                        var range = worksheet.Range[worksheet.Cells[nrow - 2, 1], worksheet.Cells[nrow - 1, 2]];
                        range.Merge();
                        range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;  // 水平置中
                        range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;    // 垂直置中
                        range.Value = "Color";

                        worksheet.Range[worksheet.Cells[nrow, 1], worksheet.Cells[nrow, 2]].Merge();
                        worksheet.Range[worksheet.Cells[nrow + 1, 1], worksheet.Cells[nrow + 1, 2]].Merge();
                    }

                    foreach (var dis_dr in distinct_CutQtyTb)
                    {
                        if (copyrow > 0)
                        {
                            Excel.Range r = worksheet.get_Range("A" + nrow.ToString(), "A" + nrow.ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
                        }

                        if (cuttingForm == CuttingForm.P09)
                        {
                            worksheet.Cells[nrow, 1] = dis_dr.Cutno;
                            worksheet.Cells[nrow, 2] = dis_dr.Colorid;
                        }
                        else if (cuttingForm == CuttingForm.P02)
                        {
                            worksheet.Cells[nrow, 1] = dis_dr.Seq;
                            worksheet.Cells[nrow, 2] = dis_dr.Colorid;
                        }
                        else
                        {
                            worksheet.Cells[nrow, 1] = dis_dr.Colorid;
                        }

                        worksheet.Cells[nrow, 3] = dis_dr.Layer;
                        worksheet.Cells[nrow, 20] = dis_dr.Cons;

                        foreach (DataRow dr in arrDtType[(int)TableType.CutQtyTb].Select($"{tbUkey} = '{dis_dr.workorderukey_CuttingForm}'"))
                        {
                            for (int i = 0; i < sizeArry.Length; i++)
                            {
                                if (sizeArry[i].Field<string>("SizeCode").Equals(dr["SizeCode"]))
                                {
                                    worksheet.Cells[nrow, i + 4] = dr["TotalQty"];
                                }
                            }
                        }

                        nrow++;
                        copyrow++;
                    }

                    totConsRowE = nrow - 1; // 最後一個Cons
                    #region Total Cons
                    worksheet.Cells[nrow + 1, 20] = string.Format("=SUM(T{0}:T{1})", totConsRowS, totConsRowE);
                    #endregion
                    nSheet++;
                }

                nSheet = 1;
                foreach (DataRow cutrefdr3 in arrDtType[(int)TableType.CutrefTb].Rows)
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                    worksheet.Select();
                    if (!MyUtility.Check.Empty(cutrefdr3["shc"]))
                    {
                        Excel.Range r = worksheet.get_Range("A14", "A14").EntireRow;
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); // 新增Row
                        Excel.Range rng2 = (Excel.Range)worksheet.get_Range("I14:U14");
                        rng2.Merge();
                        rng2.Cells.Font.Color = Color.Red;
                        rng2.Cells.Font.Bold = true;
                        worksheet.Cells[14, 9] = cutrefdr3["shc"];
                    }

                    nSheet++;
                }

                #endregion //End By CutRef

                // 重製Mode以取消Copy區塊
                worksheet.Application.CutCopyMode = Excel.XlCutCopyMode.xlCopy;
                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Cutting_P02_SpreadingReportbyCutref");
                Excel.Workbook workbook = excel.Workbooks[1];
                workbook.SaveAs(strExcelName);
                workbook.Close();
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                strExcelName.OpenFile();
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                errMsg += "\n" + ex.StackTrace.ToString();
                return false;
            }
        }

        private Bitmap NewQRcode(string strBarcode)
        {
            /*
  Level L (Low)      7%  of codewords can be restored.
  Level M (Medium)   15% of codewords can be restored.
  Level Q (Quartile) 25% of codewords can be restored.
  Level H (High)     30% of codewords can be restored.
*/
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    // Create Photo
                    Height = 79,
                    Width = 79,
                    Margin = 0,
                    CharacterSet = "UTF-8",
                    PureBarcode = true,

                    // 錯誤修正容量
                    // L水平    7%的字碼可被修正
                    // M水平    15%的字碼可被修正
                    // Q水平    25%的字碼可被修正
                    // H水平    30%的字碼可被修正
                    ErrorCorrection = ErrorCorrectionLevel.H,
                },
            };

            // Bitmap resizeQRcode = new Bitmap(writer.Write(strBarcode), new Size(38, 38));
            return writer.Write(strBarcode);
        }

        /// <summary>
        /// 0=WorkorderTb,
        /// 1=WorkorderSizeTb,
        /// 2=WorkorderDisTb,
        /// 3=WorkorderPatternTb,
        /// 4=CutrefTb,
        /// 5=CutDisOrderIDTb,
        /// 6=CutSizeTb,
        /// 7=SizeTb,
        /// 8=CutQtyTb,
        /// 9=MarkerTB,
        /// 10=FabricComboTb,
        /// 11=IssueTb,
        /// 12=OrderInfo,
        /// </summary>
        private enum TableType
        {
            WorkorderTb,
            WorkorderSizeTb,
            WorkorderDisTb,
            WorkorderPatternTb,
            CutrefTb,
            CutDisOrderIDTb,
            CutSizeTb,
            SizeTb,
            CutQtyTb,
            MarkerTB,
            FabricComboTb,
            IssueTb,
            OrderInfo,
        }
        #endregion
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore SA1602 // Enumeration items should be documented
}
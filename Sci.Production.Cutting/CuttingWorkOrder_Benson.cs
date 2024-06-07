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
        /// sheet 序號
        /// </summary>
        private int markerSerNo = 1;
        private List<long> listWorkOrderUkey;
        private string TableName = string.Empty;
        private string TableMainKeyColName = string.Empty;
        private string CuttingPOID = string.Empty;
        private string MDivisionid = string.Empty;
        private string FactoryID = string.Empty;
        private DataTable dtWorkOrder;
        private decimal inchToYdsRate = 0;
        private bool isNoMatchSP;

        private DualResult ImportMarkerExcel(string cuttingID, string mDivisionid, string factoryID, string tableName)
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
                        var firstCompare = compare.Where(o => o.ID == e.ID && o.FabricCode == e.FabricCode && o.Colorid == e.Colorid);
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

                        // 確定Excel當前的層數有沒有超過上限
                        var layer = e.Layer > firstCompare.FirstOrDefault().CuttingLayer ? firstCompare.FirstOrDefault().CuttingLayer : e.Layer;
                        e.Layer = layer;

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
        public List<WorkOrder> LoadExcel(string filename)
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

                    // 若找到，「Panel Code:」的行數為 curRowIndex
                    // 計算這個「Panel Code:」到下一個「Panel Code:」距離多少Row，代表User填了幾Row的資料
                    // A 27
                    curDataStart_Y += curRowIndex;
                    while (worksheet.GetCellValue(1, curDataStart_Y) != keyWord_FabPanelCode)
                    {
                        curDataStart_Y++;
                    }

                    // 下一個「Panel Code:」的起點
                    int nextFabPanelCodeStart = curDataStart_Y;

                    // 6 = 「Panel Code:」到「NK Name」的距離
                    // 2 = 下一個「Panel Code:」跟上一個最後一筆資料的距離
                    int markerRowCount = nextFabPanelCodeStart - curRowIndex - 6 - 2;

                    // 重置
                    curDataStart_Y = 1;

                    // 準備好物件存資料
                    WorkOrder wk = new WorkOrder();

                    // 取得「Panel Code:」的值(對應 Excel的col = B, Row = 3 )，「Panel Code:」在「Panel Code:」的下一行，「Panel Code:」的行數為 curRowIndex
                    wk.FabricPanelCode = worksheet.GetCellValue(2, curRowIndex + 1);

                    // 如果「Panel Code:」找不到，則跳到下一個「Panel Code:」的起點
                    if (MyUtility.Check.Empty(wk.FabricPanelCode))
                    {
                        curRowIndex = nextFabPanelCodeStart;
                        continue;
                    }

                    // 取得「Color:」的值((對應 Excel的col = B, Row = 5 )
                    string[] colorInfo = worksheet.GetCellValue(2, curRowIndex + 1).Split('-');

                    string tmpColorid = colorInfo.Length > 0 ? colorInfo[0] : string.Empty;
                    string tmpTone = colorInfo.Length >= 2 ? string.Empty : colorInfo[1];

                    // 裁剪母單單號
                    wk.ID = this.CuttingPOID;
                    //wk.OrderID = this.OrderID;
                    wk.FactoryID = this.FactoryID;
                    wk.MDivisionId = this.MDivisionid;
                    wk.Colorid = tmpColorid;
                    wk.Tone = tmpTone;
                    wk.IsCreateByUser = true;
                    string markername = "MK_" + this.markerSerNo.ToString().PadLeft(3, '0');
                    wk.Markername = markername;

                    // 取得Size Ratio Range (例如：1 ,4 ,28 ,24 )
                    Excel.Range rangeSizeRatio = worksheet.GetRange(1, curRowIndex, curDataStart_X + 1, nextFabPanelCodeStart - 3);

                    // 讀每一個MkName，起點是從A4「Panel Code:」往下數，所以是 = 7；終點是下筆資料「Panel Code:」的Y值 - 3，
                    // 直向往下找
                    for (int idxMarker = 7; idxMarker <= 7 + markerRowCount; idxMarker++)
                    {
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

                            // 最後面是報表加總了(AB 4)，因此Break
                            if (size.ToUpper() == "Total Qty.")
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

                        wk.ConsPC = consPc;

                        // 這個層數不是最後的結果，等等還會去檢查 Order_ColorCombo.CuttingLayer設定值
                        wk.Layer = totalLayer;
                        wk.ImportPatternPanel = importPatternPanel; // FA
                        wk.SizeRatio = dicSizeRatio; // {Size = 42，Qty = 10}、{Size = 46，Qty = 350}...

                        //int layer = Excel的TotalLayer > Construction.CuttingLayer ? Construction.CuttingLayer : Excel的TotalLayer;
                        // Cons = garmentCnt * consPc Layer
                        wk.Cons = garmentCnt * consPc; // * wk.Layer;
                        //string markername = "MK_" + this.markerSerNo.ToString().PadLeft(3, '0');
                        //wk.Markername = markername;
                        wk.MarkerLength = markerLength;
                        wk.MarkerNo = "001";
                        wk.MarkerVersion = "-1";

                        workOrders.Add(wk);
                    }
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
                var totalLayer = wk.Layer;
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
values('{this.CuttingPOID}', (select @@IDENTITY ), '{patternPanel}', '{fabricPanelCode}')";
                }

                // WorkOrder_SizeRatio
                string sqlInsertWorkOrder_SizeRatio = string.Empty;
                foreach (KeyValuePair<string, int> itemSizeRatio in wk.SizeRatio)
                {
                    sqlInsertWorkOrder_SizeRatio += $@"
insert into WorkOrder_SizeRatio ({this.TableMainKeyColName}, ID, SizeCode, Qty)
values( (select @@IDENTITY ), '{this.CuttingPOID}', '{itemSizeRatio.Key}', '{itemSizeRatio.Value}')
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

--select [WorkOrderUkey] = @@IDENTITY
{sqlInsertWorkOrder_PatternPanel}

{sqlInsertWorkOrder_SizeRatio}
";
                    DualResult result = DBProxy.Current.SelectByConn(sqlConnection, sqlInsertWorkOrder, sqlParameters, out DataTable dtResult);
                    if (!result)
                    {
                        throw result.GetException();
                    }

                    long workOrderUkey = MyUtility.Convert.GetLong(dtResult.Rows[0][0]);
                    listWorkOrderUkey.Add(workOrderUkey);

                    // 累計超過層數上限的資料，便跳出去
                    if (totalLayer < cuttingLayer)
                    {
                        break;
                    }

                    totalLayer -= cuttingLayer;
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
    }
}

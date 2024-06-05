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

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02, P09共用
    /// </summary>
    public partial class CuttingWorkOrder
    {
        private int markerSerNo = 1;
        private List<long> listWorkOrderUkey;
        private string TableName = string.Empty;
        private string OrderID = string.Empty;
        private string MDivisionid = string.Empty;
        private string FactoryID = string.Empty;
        private DataTable dtWorkOrder;

        private DualResult ImportKHMarkerExcel(string orderID, string mDivisionid, string factoryID, string tableName)
        {
            // 取得WorkOrder結構
            DBProxy.Current.Select(null, "select * from WorkOrder where 1 = 0", out dtWorkOrder);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";

            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return new DualResult(true, "NotImport");
            }

            //this.ShowWaitMessage("Loading...");

            this.TableName = tableName;
            string filename = openFileDialog.FileName;

            Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            excel.Workbooks.Open(MyUtility.Convert.GetString(filename));

            int sheetCnt = excel.ActiveWorkbook.Worksheets.Count;

            SqlConnection sqlConn;
            DualResult result = DBProxy._OpenConnection(null, out sqlConn);

            if (!result)
            {
                return result;
            }

            bool isNoMatchSP = true;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5)))
            using (sqlConn)
            {
                try
                {
                    for (int i = 1; i <= sheetCnt; i++)
                    {
                        Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[i];
                        this.markerSerNo = 1;

                        // 檢查SP
                        string poID = worksheet.GetCellValue(2, 2);

                        if (poID != orderID)
                        {
                            Marshal.ReleaseComObject(worksheet);
                            continue;
                        }

                        isNoMatchSP = false;

                        // inset WorkOrder, WorkOrder_SizeRatio
                        result = this.LoadDetailPart(worksheet, poID, sqlConn);

                        if (!result)
                        {
                            //this.HideWaitMessage();
                            Marshal.ReleaseComObject(worksheet);
                            excel.ActiveWorkbook.Close();
                            excel.Quit();
                            Marshal.ReleaseComObject(excel);
                            return result;
                        }

                        // inset WorkOrder_Distribute and update WorkOrder.OrderID
                        //this.InsertWorkOrder_Distribute(this.listWorkOrderUkey, sqlConn);
                        if (!result)
                        {
                            //this.HideWaitMessage();
                            Marshal.ReleaseComObject(worksheet);
                            excel.ActiveWorkbook.Close();
                            excel.Quit();
                            Marshal.ReleaseComObject(excel);
                            return result;
                        }

                        Marshal.ReleaseComObject(worksheet);
                    }

                    transactionScope.Complete();
                    transactionScope.Dispose();
                    excel.ActiveWorkbook.Close();
                    excel.Quit();
                    Marshal.ReleaseComObject(excel);
                }
                catch (Exception ex)
                {
                    //this.HideWaitMessage();
                    transactionScope.Dispose();
                    excel.ActiveWorkbook.Close();
                    excel.Quit();
                    Marshal.ReleaseComObject(excel);
                    return new DualResult(false, ex);
                }
            }

            //this.HideWaitMessage();

            if (isNoMatchSP)
            {
                MyUtility.Msg.InfoBox($"Excel not contain Cutting SP<{orderID}>");
                return new DualResult(true, "NotImport");
            }

            return new DualResult(true);
        }

        /// <summary>
        /// 讀取Sheet
        /// </summary>
        /// <param name="worksheet">worksheet</param>
        /// <param name="poID">poID</param>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <returns>結果</returns>
        private DualResult LoadDetailPart(Microsoft.Office.Interop.Excel.Worksheet worksheet, string poID, SqlConnection sqlConnection)
        {
            DualResult result = new DualResult(true);
            this.listWorkOrderUkey = new List<long>();
            string keyWord_FabPanelCode = "Panel Code:";


            int curRowIndex = 1;
            int emptyRowCount = 0;
            int curDataStart = 1;
            while (true)
            {
                // 如果讀取超過20行都沒有"No:"，就當作此sheet已經沒資料
                if (emptyRowCount > 20)
                {
                    break;
                }

                // 開始找「No.」欄位(對應 Excel的col = A, Row = 3 )
                if (worksheet.GetCellValue(1, curRowIndex) != keyWord_FabPanelCode)
                {
                    curRowIndex++;
                    emptyRowCount++;
                    continue;
                }

                // 若找到，No.的行數為 curRowIndex
                // 計算這個「No.」到下一個「No.」距離多少Row，代表User填了幾Row的資料
                curDataStart += curRowIndex;
                while (worksheet.GetCellValue(1, curDataStart) != keyWord_FabPanelCode)
                {
                    curDataStart++;
                }

                // 取得User填了幾Row的資料
                int dataCtn = curDataStart - curRowIndex;

                // 重置
                curDataStart = 1;

                // 找到「No.」欄位，開始讀取表格
                emptyRowCount = 0;

                DataRow drWorkOrder = this.dtWorkOrder.NewRow();

                // 取得「Panel Code:」的值(對應 Excel的col = B, Row = 3 )，「Panel Code:」在「No.」的下一行，「No.」的行數為 curRowIndex
                drWorkOrder["FabricPanelCode"] = worksheet.GetCellValue(2, curRowIndex + 1);

                // 如果Panel Code找不到，則跳過16列，16列是範本的設定一個No.的空格為16
                if (MyUtility.Check.Empty(drWorkOrder["FabricPanelCode"]))
                {
                    curRowIndex += 16;
                    continue;
                }

                // 取得「Color:」的值((對應 Excel的col = B, Row = 5 )
                string[] colorInfo = worksheet.GetCellValue(2, curRowIndex + 2).Split('-');

                string tmpColorid = colorInfo.Length > 0 ? colorInfo[0] : string.Empty;
                string tmpTone = colorInfo.Length >= 1 ? string.Empty : colorInfo[1];

                drWorkOrder["ID"] = this.OrderID;
                drWorkOrder["FactoryID"] = this.FactoryID;
                drWorkOrder["MDivisionId"] = this.MDivisionid;
                drWorkOrder["Colorid"] = tmpColorid;
                drWorkOrder["Tone"] = tmpTone;

                string sqlGetWorkOrderInfo = $@"
                select top 1	oc.PatternPanel,
                                oc.FabricPanelCode,
                				oc.FabricCode,
                				ob.SCIRefno,
                				ob.Refno,
                				psd.SEQ1,
                				psd.SEQ2,
                				[CuttingLayer] = iif(isnull(c.CuttingLayer, 100) = 0, 100, isnull(c.CuttingLayer, 100))
                from Order_ColorCombo oc with (nolock)
                inner join Order_BOF ob with (nolock) on ob.Id = oc.id and ob.FabricCode = oc.FabricCode
                inner join PO_Supp_Detail psd with (nolock) on psd.ID = oc.Id and psd.SCIRefno = ob.SCIRefno
                inner join Fabric f with(nolock) ON ob.SCIRefno=f.SCIRefno
                left join Construction c on c.Id = f.ConstructionID and c.Junk = 0
                where oc.ID = '{poID}' and oc.FabricPanelCode = '{drWorkOrder["FabricPanelCode"]}' and oc.ColorID = '{drWorkOrder["Colorid"]}'
                ";
                DataRow drWorkOrderInfo;
                bool existsColor = MyUtility.Check.Seek(sqlGetWorkOrderInfo, out drWorkOrderInfo);

                if (!existsColor)
                {
                    return new DualResult(false, $"<Color> {drWorkOrder["Colorid"]} not exists");
                }

                drWorkOrder["SEQ1"] = drWorkOrderInfo["SEQ1"];
                drWorkOrder["SEQ2"] = drWorkOrderInfo["SEQ2"];
                drWorkOrder["Refno"] = drWorkOrderInfo["Refno"];
                drWorkOrder["SCIRefno"] = drWorkOrderInfo["SCIRefno"];
                drWorkOrder["FabricCode"] = drWorkOrderInfo["FabricCode"];
                drWorkOrder["FabricPanelCode"] = drWorkOrderInfo["FabricPanelCode"];
                drWorkOrder["FabricCombo"] = drWorkOrderInfo["PatternPanel"];

                // 取得SizeRatio Range
                Excel.Range rangeSizeRatio = worksheet.GetRange(1, curRowIndex, 21, curRowIndex + 13);
                //result = this.LoadSizeRatio(rangeSizeRatio, drWorkOrder, MyUtility.Convert.GetInt(drWorkOrderInfo["CuttingLayer"]), sqlConnection);

                if (!result)
                {
                    return result;
                }

                curRowIndex += 16;
            }

            return result;
        }

        public void LoadExcel(string filename)
        {

            Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            excel.Workbooks.Open(MyUtility.Convert.GetString(filename));
            int sheetCnt = excel.ActiveWorkbook.Worksheets.Count;

            for (int i = 1; i <= sheetCnt; i++)
            {
                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[i];

                string keyWord_FabPanelCode = "Panel Code:";
                int curRowIndex = 1;
                int emptyRowCount = 0;
                int curDataStart = 1;

                while (true)
                {
                    // 如果讀取超過20行都沒有"No:"，就當作此sheet已經沒資料
                    if (emptyRowCount > 20)
                    {
                        break;
                    }

                    // 開始找「No.」欄位(對應 Excel的col = A, Row = 3 )
                    if (worksheet.GetCellValue(1, curRowIndex) != keyWord_FabPanelCode)
                    {
                        curRowIndex++;
                        emptyRowCount++;
                        continue;
                    }

                    // 若找到，No.的行數為 curRowIndex
                    // 計算這個「No.」到下一個「No.」距離多少Row，代表User填了幾Row的資料
                    curDataStart += curRowIndex;
                    while (worksheet.GetCellValue(1, curDataStart) != keyWord_FabPanelCode)
                    {
                        curDataStart++;
                    }

                    // 取得User填了幾Row的資料
                    int dataCtn = curDataStart - curRowIndex;

                    // 重置
                    curDataStart = 1;

                    // 找到「No.」欄位，開始讀取表格
                    emptyRowCount = 0;

                    DataRow drWorkOrder = this.dtWorkOrder.NewRow();

                    // 取得「Panel Code:」的值(對應 Excel的col = B, Row = 3 )，「Panel Code:」在「No.」的下一行，「No.」的行數為 curRowIndex
                    drWorkOrder["FabricPanelCode"] = worksheet.GetCellValue(2, curRowIndex + 1);

                    // 如果Panel Code找不到，則跳過16列，16列是範本的設定一個No.的空格為16
                    if (MyUtility.Check.Empty(drWorkOrder["FabricPanelCode"]))
                    {
                        curRowIndex += 16;
                        continue;
                    }

                    // 取得「Color:」的值((對應 Excel的col = B, Row = 5 )
                    string[] colorInfo = worksheet.GetCellValue(2, curRowIndex + 2).Split('-');

                    string tmpColorid = colorInfo.Length > 0 ? colorInfo[0] : string.Empty;
                    string tmpTone = colorInfo.Length >= 1 ? string.Empty : colorInfo[1];

                    drWorkOrder["ID"] = this.OrderID;
                    drWorkOrder["FactoryID"] = this.FactoryID;
                    drWorkOrder["MDivisionId"] = this.MDivisionid;
                    drWorkOrder["Colorid"] = tmpColorid;
                    drWorkOrder["Tone"] = tmpTone;

                    #region 原本要做的SQL檢查，移到後面步驟

                    //                    string sqlGetWorkOrderInfo = $@"
                    //select top 1	oc.PatternPanel,
                    //                oc.FabricPanelCode,
                    //				oc.FabricCode,
                    //				ob.SCIRefno,
                    //				ob.Refno,
                    //				psd.SEQ1,
                    //				psd.SEQ2,
                    //				[CuttingLayer] = iif(isnull(c.CuttingLayer, 100) = 0, 100, isnull(c.CuttingLayer, 100))
                    //from Order_ColorCombo oc with (nolock)
                    //inner join Order_BOF ob with (nolock) on ob.Id = oc.id and ob.FabricCode = oc.FabricCode
                    //inner join PO_Supp_Detail psd with (nolock) on psd.ID = oc.Id and psd.SCIRefno = ob.SCIRefno
                    //inner join Fabric f with(nolock) ON ob.SCIRefno=f.SCIRefno
                    //left join Construction c on c.Id = f.ConstructionID and c.Junk = 0
                    //where oc.ID = '{poID}' and oc.FabricPanelCode = '{drWorkOrder["FabricPanelCode"]}' and oc.ColorID = '{drWorkOrder["Colorid"]}'
                    //";
                    //                    DataRow drWorkOrderInfo;
                    //                    bool existsColor = MyUtility.Check.Seek(sqlGetWorkOrderInfo, out drWorkOrderInfo);

                    //                    if (!existsColor)
                    //                    {
                    //                        return new DualResult(false, $"<Color> {drWorkOrder["Colorid"]} not exists");
                    //                    }

                    //                    drWorkOrder["SEQ1"] = drWorkOrderInfo["SEQ1"];
                    //                    drWorkOrder["SEQ2"] = drWorkOrderInfo["SEQ2"];
                    //                    drWorkOrder["Refno"] = drWorkOrderInfo["Refno"];
                    //                    drWorkOrder["SCIRefno"] = drWorkOrderInfo["SCIRefno"];
                    //                    drWorkOrder["FabricCode"] = drWorkOrderInfo["FabricCode"];
                    //                    drWorkOrder["FabricPanelCode"] = drWorkOrderInfo["FabricPanelCode"];
                    //                    drWorkOrder["FabricCombo"] = drWorkOrderInfo["PatternPanel"];
                    #endregion


                    // 取得SizeRatio Range
                    Excel.Range rangeSizeRatio = worksheet.GetRange(1, curRowIndex, 21, curRowIndex + 13);
                }
            }
        }
    }


    public class Cutting
    {
        public string OrderID { get; set; }
        public string MDivisionid { get; set; }
        public string FactoryID { get; set; }
    }
}

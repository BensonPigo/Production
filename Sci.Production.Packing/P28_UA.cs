using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P28_UA : Sci.Win.Tems.QueryForm
    {
        private List<Packing_ExcelData> Packing_ExcelDatas = new List<Packing_ExcelData>();
        private List<Item_ExcelData> Item_ExcelDatas = new List<Item_ExcelData>();
        private List<PackingListCandidate_Datasource> PackingListCandidate_Datasources = new List<PackingListCandidate_Datasource>();

        /// <inheritdoc/>
        public P28_UA()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            DataGridViewGeneratorTextColumnSettings col_PackingListCandidate = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings col_Overwrite = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_Overwrite.HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None;

            col_PackingListCandidate.CellMouseClick += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridMatch.GetDataRow<DataRow>(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem item;

                    string fileName = MyUtility.Convert.GetString(dr["FileName"]);

                    if (!this.PackingListCandidate_Datasources.Where(o => o.FileName == fileName).Any())
                    {
                        return;
                    }

                    PackingListCandidate_Datasource pData = this.PackingListCandidate_Datasources.Where(o => o.FileName == fileName).FirstOrDefault();

                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn() { ColumnName = "PackingList ID", DataType = typeof(string) });

                    pData.PackingList_Candidate.ForEach(packingListID => dt.Rows.Add(dt.NewRow()["PackingList ID"] = packingListID));

                    item = new Win.Tools.SelectItem(dt, "PackingList ID", "15", this.Text, false, ",", "PackingList ID");

                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    var selectedData = item.GetSelecteds();
                    dr["PackingListID"] = selectedData[0]["PackingList ID"].ToString();
                    dr["OverWrite"] = false;
                }
            };

            col_PackingListCandidate.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridMatch.GetDataRow<DataRow>(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem item;

                    string fileName = MyUtility.Convert.GetString(dr["FileName"]);

                    if (!this.PackingListCandidate_Datasources.Where(o => o.FileName == fileName).Any())
                    {
                        return;
                    }

                    PackingListCandidate_Datasource pData = this.PackingListCandidate_Datasources.Where(o => o.FileName == fileName).FirstOrDefault();

                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn() { ColumnName = "PackingList ID", DataType = typeof(string) });

                    pData.PackingList_Candidate.ForEach(packingListID => dt.Rows.Add(dt.NewRow()["PackingList ID"] = packingListID));
                    item = new Win.Tools.SelectItem(dt, "PackingList ID", "15", this.Text, false, ",", "PackingList ID");

                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    var selectedData = item.GetSelecteds();
                    dr["PackingListID"] = selectedData[0]["PackingList ID"].ToString();
                    dr["OverWrite"] = false;
                }
            };

            col_PackingListCandidate.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridMatch.GetDataRow(e.RowIndex);
                string oldvalue = dr["PackingListID"].ToString();
                string newvalue = e.FormattedValue.ToString();
                string fileName = MyUtility.Convert.GetString(dr["FileName"]);

                if (!this.PackingListCandidate_Datasources.Where(o => o.FileName == fileName).Any())
                {
                    return;
                }

                if (this.EditMode && oldvalue.ToUpper() != newvalue.ToUpper() /* && newvalue.ToUpper() != "PLEASE SELECT"*/)
                {
                    if (!this.PackingListCandidate_Datasources.Where(o => o.FileName == fileName && o.PackingList_Candidate.Contains(newvalue)).Any() && newvalue != string.Empty)
                    {
                        dr["PackingListID"] = oldvalue;
                        dr.EndEdit();
                        MyUtility.Msg.WarningBox("Data not found");
                        return;
                    }
                    else
                    {
                        dr["PackingListID"] = newvalue;
                        dr["OverWrite"] = false;
                        dr.EndEdit();
                    }
                }
            };

            col_Overwrite.CellEditable += (s, e) =>
            {
                DataRow dr = this.gridMatch.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetBool(dr["ExistsSSCC"]))
                {
                    e.IsEditable = true;
                }
                else
                {
                    e.IsEditable = false;
                }
            };

            this.gridPackingFile.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridPackingFile)
              .Text("FileName", header: "FileName", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .Text("Result", header: "Result", width: Widths.AnsiChars(13), iseditingreadonly: true)
              ;

            this.gridItemFile.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridItemFile)
              .Text("FileName", header: "FileName", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .Text("Result", header: "Result", width: Widths.AnsiChars(13), iseditingreadonly: true)
              ;

            this.gridMatch.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridMatch)
              .Text("OrderNumber", header: "Order Number", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .Date("ShipDate", header: "Ship. Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .CheckBox("MultipleMatches", header: "Multiple" + Environment.NewLine + "Matches", width: Widths.AnsiChars(4), iseditable: false)
              .Text("PackingListID", header: "P/L" + Environment.NewLine + "Candidate", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: col_PackingListCandidate)
              .CheckBox("ExistsSSCC", header: "Cust CTN# already exists", trueValue: 1, falseValue: 0, width: Widths.AnsiChars(5), iseditable: false)
              .CheckBox("Overwrite", header: "Overwrite", trueValue: 1, falseValue: 0, width: Widths.AnsiChars(5), iseditable: true, settings: col_Overwrite)
              ;
        }

        private void BtnAddPackingFile_Click(object sender, EventArgs e)
        {
            this.openFileDialogPackingList.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";

            // 開窗且有選擇檔案
            if (this.openFileDialogPackingList.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.ShowWaitMessage("Processing...");
            this.BindingSourceMatch.DataSource = null;
            this.BindingSourcePackingFile.DataSource = null;
            this.PackingListCandidate_Datasources = new List<PackingListCandidate_Datasource>();
            this.Packing_ExcelDatas = new List<Packing_ExcelData>();

            // 取得所有檔名
            string[] files_SafeFileNames = this.openFileDialogPackingList.SafeFileNames;
            string[] files = this.openFileDialogPackingList.FileNames;

            DataTable gridData = new DataTable();
            gridData.ColumnsStringAdd("FileName");
            gridData.ColumnsStringAdd("Result");

            foreach (string safeFileName in files_SafeFileNames)
            {
                // 準備放在左邊Grid的Datatable
                DataRow nRow = gridData.NewRow();
                nRow["FileName"] = safeFileName;

                string fullFileName = files.Where(o => o.Contains(safeFileName)).FirstOrDefault();

                try
                {
                    Excel.Application excel = new Excel.Application();
                    excel.Workbooks.Open(MyUtility.Convert.GetString(fullFileName));
                    excel.Visible = false;

                    Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                    #region 確認是否有缺少必要欄位
                    int intColumnsCount = worksheet.UsedRange.Columns.Count;

                    // 必要欄位
                    List<string> mustColumn = new List<string>() { "Handling Unit ID", "Order Number", "Item Number", "Ship. Date", "HU Qty" };

                    // 紀錄必要欄位橫向的欄位位置
                    int idx_HandlingUnitID = 0;
                    int idx_OrderNumber = 0;
                    int idx_ItemNumber = 0;
                    int idx_ShipDate = 0;
                    int idx_HUQty = 0;

                    for (int x = 1; x <= intColumnsCount; x++)
                    {
                        var colName = worksheet.Cells[1, x].Value;

                        switch (colName)
                        {
                            case "Handling Unit ID":
                                idx_HandlingUnitID = x;
                                mustColumn.Remove("Handling Unit ID");
                                break;
                            case "Order Number":
                                idx_OrderNumber = x;
                                mustColumn.Remove("Order Number");
                                break;
                            case "Item Number":
                                idx_ItemNumber = x;
                                mustColumn.Remove("Item Number");
                                break;
                            case "Ship. Date":
                                idx_ShipDate = x;
                                mustColumn.Remove("Ship. Date");
                                break;
                            case "HU Qty":
                                idx_HUQty = x;
                                mustColumn.Remove("HU Qty");
                                break;
                            default:
                                break;
                        }
                    }

                    // 缺少欄位記錄下來
                    if (mustColumn.Count > 0)
                    {
                        string msg = $"Could not found column <{mustColumn.JoinToString(",")}> .";

                        nRow["Result"] = msg;

                        excel.Quit();
                        Marshal.ReleaseComObject(worksheet);
                        Marshal.ReleaseComObject(excel);
                        gridData.Rows.Add(nRow);
                        continue;
                    }
                    #endregion

                    int intRowsCount = worksheet.UsedRange.Rows.Count;

                    nRow["Result"] = string.Empty;

                    // 正在讀取的行數，由於第一行是Header，因此起始值為2
                    int intRowsReading = 2;

                    // 讀取Excel的資料並存下來，避免後續要再次讀取
                    while (intRowsReading <= intRowsCount)
                    {
                        Packing_ExcelData excelData = new Packing_ExcelData() { FileName = safeFileName };

                        // Handling Unit ID
                        var handlingUnitID = worksheet.Cells[intRowsReading, idx_HandlingUnitID].Value;

                        // Order Number
                        var orderNumber = worksheet.Cells[intRowsReading, idx_OrderNumber].Value;

                        // Item Number
                        var itemNumber = worksheet.Cells[intRowsReading, idx_ItemNumber].Value;

                        // Ship. Date
                        DateTime? shipDate = MyUtility.Convert.GetDate(worksheet.Cells[intRowsReading, idx_ShipDate].Value);

                        // HU Qty
                        var hUQty = worksheet.Cells[intRowsReading, idx_HUQty].Value;

                        if (orderNumber == null)
                        {
                            intRowsReading++;
                            continue;
                        }

                        excelData.HandlingUnitID = MyUtility.Convert.GetString(handlingUnitID);
                        excelData.OrderNumber = MyUtility.Convert.GetString(orderNumber);
                        excelData.ItemNumber = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(itemNumber));
                        excelData.ShipDate = shipDate.HasValue ? shipDate.Value.ToShortDateString() : string.Empty;
                        excelData.HUQty = MyUtility.Convert.GetString(hUQty);
                        excelData.ExecuteUpdate = false;

                        this.Packing_ExcelDatas.Add(excelData);
                        intRowsReading++;
                    }

                    gridData.Rows.Add(nRow);

                    excel.Quit();
                    Marshal.ReleaseComObject(worksheet);
                    Marshal.ReleaseComObject(excel);
                }
                catch (Exception ex)
                {
                    this.ShowErr(ex);
                }
            }

            this.BindingSourcePackingFile.DataSource = gridData;
            this.HideWaitMessage();
        }

        private void BtnRemovePackingFile_Click(object sender, EventArgs e)
        {
            this.BindingSourcePackingFile.DataSource = null;
            this.BindingSourceMatch.DataSource = null;
            this.Packing_ExcelDatas = new List<Packing_ExcelData>();
        }

        private void BtnAddItemFile_Click(object sender, EventArgs e)
        {
            this.openFileDialogItemFile.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";

            // 開窗且有選擇檔案
            if (this.openFileDialogItemFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.ShowWaitMessage("Processing...");
            this.BindingSourceItemFile.DataSource = null;
            this.BindingSourceMatch.DataSource = null;
            this.PackingListCandidate_Datasources = new List<PackingListCandidate_Datasource>();
            this.Item_ExcelDatas = new List<Item_ExcelData>();

            // 取得所有檔名
            string[] files_SafeFileNames = this.openFileDialogItemFile.SafeFileNames;
            string[] files = this.openFileDialogItemFile.FileNames;

            DataTable gridData = new DataTable();
            gridData.ColumnsStringAdd("FileName");
            gridData.ColumnsStringAdd("Result");

            foreach (string safeFileName in files_SafeFileNames)
            {
                // 準備放在左邊Grid的Datatable
                DataRow nRow = gridData.NewRow();
                nRow["FileName"] = safeFileName;

                string fullFileName = files.Where(o => o.Contains(safeFileName)).FirstOrDefault();

                try
                {
                    Excel.Application excel = new Excel.Application();
                    excel.Workbooks.Open(MyUtility.Convert.GetString(fullFileName));
                    excel.Visible = false;

                    Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                    #region 確認是否有缺少必要欄位
                    int intColumnsCount = worksheet.UsedRange.Columns.Count;

                    // 必要欄位
                    List<string> mustColumn = new List<string>() { "PO No.", "PO Item No.", "Size" };

                    // 紀錄必要欄位橫向的欄位位置
                    int idx_PONO = 0;
                    int idx_POItemNo = 0;
                    int idx_Size = 0;

                    for (int x = 1; x <= intColumnsCount; x++)
                    {
                        var colName = worksheet.Cells[1, x].Value;

                        switch (colName)
                        {
                            case "PO No.":
                                idx_PONO = x;
                                mustColumn.Remove("PO No.");
                                break;
                            case "PO Item No.":
                                idx_POItemNo = x;
                                mustColumn.Remove("PO Item No.");
                                break;
                            case "Size":
                                idx_Size = x;
                                mustColumn.Remove("Size");
                                break;
                            default:
                                break;
                        }
                    }

                    // 缺少欄位記錄下來
                    if (mustColumn.Count > 0)
                    {
                        string msg = $"Could not found column <{mustColumn.JoinToString(",")}> .";

                        nRow["Result"] = msg;

                        excel.Quit();
                        Marshal.ReleaseComObject(worksheet);
                        Marshal.ReleaseComObject(excel);
                        gridData.Rows.Add(nRow);
                        continue;
                    }
                    #endregion

                    int intRowsCount = worksheet.UsedRange.Rows.Count;

                    nRow["Result"] = string.Empty;

                    // 正在讀取的行數，由於第一行是Header，因此起始值為2
                    int intRowsReading = 2;

                    // 讀取Excel的資料並存下來，避免後續要再次讀取
                    while (intRowsReading <= intRowsCount)
                    {
                        Item_ExcelData excelData = new Item_ExcelData() { FileName = safeFileName };

                        // PO No.
                        var poNO = worksheet.Cells[intRowsReading, idx_PONO].Value;

                        // PO Item No
                        var pOItemNo = worksheet.Cells[intRowsReading, idx_POItemNo].Value;

                        // Size
                        var size = worksheet.Cells[intRowsReading, idx_Size].Value;

                        if (poNO == null)
                        {
                            intRowsReading++;
                            continue;
                        }

                        excelData.Pono = MyUtility.Convert.GetString(poNO);
                        excelData.POItemNo = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(pOItemNo));
                        excelData.Size = MyUtility.Convert.GetString(size);
                        excelData.ExecuteUpdate = false;

                        this.Item_ExcelDatas.Add(excelData);
                        intRowsReading++;
                    }

                    gridData.Rows.Add(nRow);

                    excel.Quit();
                    Marshal.ReleaseComObject(worksheet);
                    Marshal.ReleaseComObject(excel);
                }
                catch (Exception ex)
                {
                    this.ShowErr(ex);
                }
            }

            this.BindingSourceItemFile.DataSource = gridData;
            this.HideWaitMessage();
        }

        private void BtnRemoveItemFile_Click(object sender, EventArgs e)
        {
            this.BindingSourceItemFile.DataSource = null;
            this.BindingSourceMatch.DataSource = null;
            this.Item_ExcelDatas = new List<Item_ExcelData>();
        }

        private void BtnMapping_Click(object sender, EventArgs e)
        {
            this.BindingSourceMatch.DataSource = null;

            try
            {
                // 將所有Excel資料分群組
                List<Packing_ExcelData> groupDatas = this.Packing_ExcelDatas
                    .GroupBy(o => new { o.OrderNumber, o.ShipDate })
                    .Select(o => o.First()).Distinct().ToList();

                // 取得SP# & CTnType(相同OrderNumber + ShipDate = 相同 SP# CTnType)
                foreach (var obj in groupDatas)
                {
                    string custPONO = obj.OrderNumber;

                    string sql = $"SELECT ID,CtnType FROM Orders WHERE BrandID = 'U.ARMOUR' AND CustPONo = '{custPONO}'";

                    MyUtility.Check.Seek(sql, out DataRow dr);

                    string sp = MyUtility.Convert.GetString(dr["ID"]);
                    string ctnType = MyUtility.Convert.GetString(dr["CtnType"]);

                    this.Packing_ExcelDatas.Where(o => o.OrderNumber == obj.OrderNumber && o.ShipDate == obj.ShipDate).ToList().ForEach(i =>
                    {
                        i.OrderID = sp;
                        i.CTnType = ctnType;
                    });
                }

                List<MatchGridData> matchGridDatas = new List<MatchGridData>();
                foreach (var groupData in groupDatas)
                {
                    List<MatchGridData> oneData = this.MapPackingList(groupData);
                    matchGridDatas.AddRange(oneData);
                }

                DataTable matchGridDatasDT = ListToDataTable.ToDataTable(matchGridDatas);
                this.BindingSourceMatch.DataSource = matchGridDatasDT;
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        /// <inheritdoc/>
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (!this.ConfirmCheck())
            {
                return;
            }

            try
            {
                // 從右方的MatchGird找出需要更新的資料
                this.ShowWaitMessage("Processing...");
                DataTable packingFileDt = (DataTable)this.BindingSourcePackingFile.DataSource;

                // 取得要更新的File Name
                DataTable dt = (DataTable)this.BindingSourceMatch.DataSource;
                var needUpdateFile = dt.AsEnumerable()
                    .Where(o => (MyUtility.Convert.GetBool(o["ExistsSSCC"]) && MyUtility.Convert.GetBool(o["Overwrite"])) || !MyUtility.Convert.GetBool(o["ExistsSSCC"]))
                    .Select(o => new { Filename = MyUtility.Convert.GetString(o["Filename"]), PackingListID = MyUtility.Convert.GetString(o["PackingListID"]) }).Distinct();

                // 並不是所有資料都需要更新，因此加上註記
                foreach (var item in needUpdateFile)
                {
                    string packingListID = item.PackingListID;
                    this.Packing_ExcelDatas.Where(o => o.FileName == item.Filename && o.PackingListID == packingListID).ToList().ForEach(o =>
                    {
                        o.PackingListID = packingListID;
                        o.ExecuteUpdate = true;
                    });

                    // 開始更新，一次更新一個Packing
                    DualResult r = this.UpdateDatabase(this.Packing_ExcelDatas.Where(o => o.PackingListID == packingListID).ToList());

                    if (r)
                    {
                        packingFileDt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == item.Filename).FirstOrDefault()["Result"] = "Success";

                        #region ISP20200757 資料交換 - Sunrise

                        Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(packingListID, string.Empty))
                            .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

                        #endregion

                        #region ISP20201607 資料交換 - Gensong

                        // 不透過Call API的方式，自己組合，傳送API
                        Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(packingListID, string.Empty))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

                        #endregion
                    }
                    else
                    {
                        packingFileDt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == item.Filename).FirstOrDefault()["Result"] = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
            finally
            {
                MyUtility.Msg.InfoBox("Finish!!");
                this.HideWaitMessage();
                this.btnMapping.PerformClick();
            }
        }

        /// <inheritdoc/>
        private List<MatchGridData> MapPackingList(Packing_ExcelData groupData)
        {
            List<MatchGridData> matchGridDatas = new List<MatchGridData>();

            string cTNType = groupData.CTnType;
            string fileName = groupData.FileName;

            // 單色單碼
            if (cTNType == "1")
            {
                #region 取得所有的Size資訊

                // 先找出對應的 Item Size 報表
                var mappingDatas = this.Packing_ExcelDatas
                        .Where(o => o.CTnType == "1")
                        .GroupBy(o => new { o.OrderNumber, o.ItemNumber })
                        .Select(o => o.First()).Distinct().ToList();

                var sizeDatas = this.Item_ExcelDatas.Where(o =>
                    mappingDatas.Any(x => x.OrderNumber == o.Pono && x.ItemNumber == o.POItemNo)).ToList();

                // 若沒資料，表示沒有上傳Item Size的資料，不Mapping
                if (sizeDatas.Count == 0)
                {
                    return new List<MatchGridData>();
                }

                // Packing List報表與 Item Size 報表比對，找出Size ShipQty
                foreach (var excelData in this.Packing_ExcelDatas.Where(o => o.CTnType == "1"))
                {
                    var sizeData = sizeDatas.Where(o => o.Pono == excelData.OrderNumber && o.POItemNo == excelData.ItemNumber).FirstOrDefault();
                    string tmpSize = sizeData.Size;

                    string size = MyUtility.GetValue.Lookup($@"SELECT SizeCode FROM Order_SizeSpec WHERE SizeItem='S01' AND SizeSpec='{tmpSize}' AND ID='{excelData.OrderID}'");
                    string shipQty = excelData.HUQty;

                    excelData.SizeCode = size;
                    excelData.ShipQty = shipQty;
                }
                #endregion

                // 找出同一群的資料
                var samePack = this.Packing_ExcelDatas.Where(o => o.OrderNumber == groupData.OrderNumber && o.ShipDate == groupData.ShipDate).ToList();

                // 不想用ProcessWithDatatable，把物件手動弄成temp table
                string tmpTable = string.Empty;
                int count = 1;
                foreach (var excelData in samePack)
                {
                    string tmp = $@"SELECT [OrderID]='{excelData.OrderID}',[SizeCode]='{excelData.SizeCode}',[ShipQty]='{excelData.ShipQty}'";

                    tmpTable += tmp + Environment.NewLine;

                    if (count == 1)
                    {
                        tmpTable += $"INTO #tmp" + Environment.NewLine;
                    }

                    if (samePack.Count > count)
                    {
                        tmpTable += "UNION ALL" + Environment.NewLine;
                    }

                    count++;
                }

                #region SQL
                string cmd = $@"
{tmpTable}

SELECT DISTINCT p.ID,pd.Ukey,pd.CustCTN,pd.CTNStartNo
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
LEFT JOIN Pullout pu ON pu.ID = p.PulloutID
INNER JOIN #tmp t ON pd.OrderID =t.OrderID
	AND pd.SizeCode=t.SizeCode
	AND pd.ShipQty=t.ShipQty
WHERE 1=1
AND (pu.Status NOT IN ('Confirmed', 'Locked') OR pu.Status IS NULL)

DROP TABLE #tmp
";

                #endregion

                DualResult r = DBProxy.Current.Select(null, cmd, out DataTable dt);
                if (!r)
                {
                    throw r.GetException();
                }

                // 產生要填入Match Grid的物件
                MatchGridData m = new MatchGridData()
                {
                    FileName = fileName,
                    OrderNumber = groupData.OrderNumber,
                    ShipDate = MyUtility.Convert.GetDate(groupData.ShipDate).Value,
                    Overwrite = false,
                    ExistsSSCC = false,
                };

                #region  判斷PackingList_Detail的筆數是否與Excel對上
                if (dt.Rows.Count == samePack.Count)
                {
                    // 剛好一對一
                    string packingListID = dt.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["ID"])).Distinct().FirstOrDefault();

                    // 確認是否已經存在SSCC
                    bool existsSSCC = dt.AsEnumerable().Any(o => !MyUtility.Check.Empty(o["CustCTN"]));
                    samePack.ForEach(o =>
                    {
                        o.PackingListID = packingListID;
                        o.MultipleMatches = false;
                    });

                    PackingListCandidate_Datasource pp = new PackingListCandidate_Datasource()
                    {
                        FileName = fileName,
                        PackingList_Candidate = new List<string>() { packingListID },
                    };
                    m.PackingListID = packingListID;
                    m.MultipleMatches = false;
                    m.Overwrite = existsSSCC;
                    m.ExistsSSCC = existsSSCC;
                    matchGridDatas.Add(m);
                }
                else
                {
                    if (dt.Rows.Count != 0)
                    {
                        // 一筆對到多箱
                        List<string> tmpPackingListIDs = dt.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["ID"])).Distinct().ToList();
                        List<string> packingListIDs = new List<string>();

                        // PackingList_Detail資料筆數有對應上才需要理會
                        foreach (var packingListID in tmpPackingListIDs)
                        {
                            int ctn = dt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["ID"]) == packingListID).Count();
                            if (ctn == samePack.Count)
                            {
                                packingListIDs.Add(packingListID);
                            }
                        }

                        if (packingListIDs.Count() == 0)
                        {
                            // DB找不到
                            samePack.ForEach(o =>
                            {
                                o.PackingListID = string.Empty;
                                o.MultipleMatches = false;
                            });
                        }

                        // 只對應到一個Packing
                        if (packingListIDs.Count() == 1)
                        {
                            // 剛好一對一
                            string packingListID = packingListIDs.FirstOrDefault();

                            // 確認是否已經存在SSCC
                            bool existsSSCC = dt.AsEnumerable().Any(o => !MyUtility.Check.Empty(o["CustCTN"]));
                            samePack.ForEach(o =>
                            {
                                o.PackingListID = packingListID;
                                o.MultipleMatches = false;
                            });

                            PackingListCandidate_Datasource pp = new PackingListCandidate_Datasource()
                            {
                                FileName = fileName,
                                PackingList_Candidate = packingListIDs,
                            };
                            this.PackingListCandidate_Datasources.Add(pp);

                            m.PackingListID = packingListID;
                            m.MultipleMatches = false;
                            m.Overwrite = existsSSCC;
                            m.ExistsSSCC = existsSSCC;
                            matchGridDatas.Add(m);
                        }

                        if (packingListIDs.Count() > 1)
                        {
                            // 真的有多個對應到
                            samePack.ForEach(o =>
                            {
                                o.PackingListID = packingListIDs.FirstOrDefault();
                                o.PackingList_Candidate = packingListIDs;
                                o.MultipleMatches = true;
                            });
                            PackingListCandidate_Datasource p = new PackingListCandidate_Datasource()
                            {
                                FileName = fileName,
                                PackingList_Candidate = packingListIDs,
                            };
                            this.PackingListCandidate_Datasources.Add(p);
                            m.PackingListID = "Please select"; // packingListIDs.FirstOrDefault();
                            m.MultipleMatches = true;
                            matchGridDatas.Add(m);
                        }
                    }
                    else
                    {
                        // DB找不到
                        samePack.ForEach(o =>
                        {
                            o.PackingListID = string.Empty;
                            o.MultipleMatches = false;
                        });
                    }
                }
                #endregion
            }
            else
            {
                // 找出同一群的資料
                var samePack = this.Packing_ExcelDatas.Where(o => o.OrderNumber == groupData.OrderNumber && o.ShipDate == groupData.ShipDate).ToList();
                string orderID = samePack.FirstOrDefault().OrderID;

                // 混尺碼，Excel資料筆數 = PackingList_Detail箱數
                #region SQL
                string cmd = $@"

SELECT DISTINCT p.ID,pd.CTNStartNo ,pd.CustCTN
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
LEFT JOIN Pullout pu ON pu.ID = p.PulloutID
WHERE 1=1
AND pd.OrderID='{orderID}'
AND (pu.Status NOT IN ('Confirmed', 'Locked') OR pu.Status IS NULL)

";

                #endregion

                DualResult r = DBProxy.Current.Select(null, cmd, out DataTable dt);
                if (!r)
                {
                    throw r.GetException();
                }

                // 產生要填入Match Grid的物件
                MatchGridData m = new MatchGridData()
                {
                    FileName = fileName,
                    OrderNumber = groupData.OrderNumber,
                    ShipDate = MyUtility.Convert.GetDate(groupData.ShipDate).Value,
                    Overwrite = false,
                    ExistsSSCC = false,
                };

                #region  判斷PackingList_Detail的筆數是否與Excel對上
                if (dt.Rows.Count == samePack.Count)
                {
                    // 剛好一對一
                    string packingListID = dt.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["ID"])).Distinct().FirstOrDefault();

                    // 確認是否已經存在SSCC
                    bool existsSSCC = dt.AsEnumerable().Any(o => !MyUtility.Check.Empty(o["CustCTN"]));
                    samePack.ForEach(o =>
                    {
                        o.PackingListID = packingListID;
                        o.MultipleMatches = false;
                    });

                    PackingListCandidate_Datasource pp = new PackingListCandidate_Datasource()
                    {
                        FileName = fileName,
                        PackingList_Candidate = new List<string>() { packingListID },
                    };
                    this.PackingListCandidate_Datasources.Add(pp);
                    m.PackingListID = packingListID;
                    m.MultipleMatches = false;
                    m.Overwrite = existsSSCC;
                    m.ExistsSSCC = existsSSCC;
                    matchGridDatas.Add(m);
                }
                else
                {
                    if (dt.Rows.Count != 0)
                    {
                        // 一筆對到多箱
                        List<string> tmpPackingListIDs = dt.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["ID"])).Distinct().ToList();
                        List<string> packingListIDs = new List<string>();

                        // PackingList_Detail資料筆數有對應上才需要理會
                        foreach (var packingListID in tmpPackingListIDs)
                        {
                            int ctn = dt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["ID"]) == packingListID).Count();
                            if (ctn == samePack.Count)
                            {
                                packingListIDs.Add(packingListID);
                            }
                        }

                        if (packingListIDs.Count() == 0)
                        {
                            // DB找不到
                            samePack.ForEach(o =>
                            {
                                o.PackingListID = string.Empty;
                                o.MultipleMatches = false;
                            });
                        }

                        // 只對應到一個Packing
                        if (packingListIDs.Count() == 1)
                        {
                            // 剛好一對一
                            string packingListID = packingListIDs.FirstOrDefault();

                            // 確認是否已經存在SSCC
                            bool existsSSCC = dt.AsEnumerable().Any(o => !MyUtility.Check.Empty(o["CustCTN"]));
                            samePack.ForEach(o =>
                            {
                                o.PackingListID = packingListID;
                                o.MultipleMatches = false;
                            });

                            PackingListCandidate_Datasource pp = new PackingListCandidate_Datasource()
                            {
                                FileName = fileName,
                                PackingList_Candidate = packingListIDs,
                            };
                            this.PackingListCandidate_Datasources.Add(pp);

                            m.PackingListID = packingListID;
                            m.MultipleMatches = false;
                            m.Overwrite = existsSSCC;
                            m.ExistsSSCC = existsSSCC;
                            matchGridDatas.Add(m);
                        }

                        if (packingListIDs.Count() > 1)
                        {
                            // 真的有多個對應到
                            samePack.ForEach(o =>
                            {
                                o.PackingListID = packingListIDs.FirstOrDefault();
                                o.PackingList_Candidate = packingListIDs;
                                o.MultipleMatches = true;
                            });
                            PackingListCandidate_Datasource p = new PackingListCandidate_Datasource()
                            {
                                FileName = fileName,
                                PackingList_Candidate = packingListIDs,
                            };
                            this.PackingListCandidate_Datasources.Add(p);
                            m.PackingListID = "Please select"; // packingListIDs.FirstOrDefault();
                            m.MultipleMatches = true;
                            matchGridDatas.Add(m);
                        }
                    }
                    else
                    {
                        // DB找不到
                        samePack.ForEach(o =>
                        {
                            o.PackingListID = string.Empty;
                            o.MultipleMatches = false;
                        });
                    }
                }
                #endregion
            }

            return matchGridDatas;
        }

        /// <inheritdoc/>
        private bool ConfirmCheck()
        {
            if (this.BindingSourceMatch.DataSource == null)
            {
                return false;
            }

            DataTable dt = (DataTable)this.BindingSourceMatch.DataSource;

            if (dt.AsEnumerable().Any(o => MyUtility.Convert.GetBool(o["MultipleMatches"]) && (MyUtility.Check.Empty(o["PackingListID"]) || MyUtility.Convert.GetString(o["PackingListID"]) == "Please select")))
            {
                MyUtility.Msg.WarningBox("Please select Packing List ID!!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新DB
        /// </summary>
        /// <param name="excelDatas">相同Packing的Excel資料</param>
        /// <returns>result</returns>
        private DualResult UpdateDatabase(List<Packing_ExcelData> excelDatas)
        {
            // 不想用ProcessWithDatatable，把物件手動弄成temp table
            string tmpTable = string.Empty;
            string cmd = string.Empty;
            int count = 1;
            bool isMixed = false;
            var everyCarton = excelDatas.GroupBy(o => new { o.PackingListID, o.OrderID, o.Article, o.SizeCode, o.ShipQty, o.HandlingUnitID })
                                        .Select(o => o.First()).Distinct().ToList();

            foreach (var excelData in everyCarton)
            {
                if (excelData.CTnType == "1")
                {
                    string tmp = $@"SELECT [PackingListID]='{excelData.PackingListID}',[OrderID]='{excelData.OrderID}',[SizeCode]='{excelData.SizeCode}',[ShipQty]='{excelData.ShipQty}',[CartonBarcode]='{excelData.HandlingUnitID}'";

                    tmpTable += tmp + Environment.NewLine;

                    if (count == 1)
                    {
                        tmpTable += $"INTO #tmp" + Environment.NewLine;
                    }

                    if (excelDatas.Count > count)
                    {
                        tmpTable += "UNION" + Environment.NewLine;
                    }
                }
                else
                {
                    isMixed = true;
                    string tmp = $@"SELECT [PackingListID]='{excelData.PackingListID}',[OrderID]='{excelData.OrderID}',[CartonBarcode]='{excelData.HandlingUnitID}'";

                    tmpTable += tmp + Environment.NewLine;

                    if (count == 1)
                    {
                        tmpTable += $"INTO #tmp" + Environment.NewLine;
                    }

                    if (excelDatas.Count > count)
                    {
                        tmpTable += "UNION" + Environment.NewLine;
                    }
                }

                count++;
            }

            #region SQL
            if (!isMixed)
            {
                cmd = $@"
{tmpTable}

----整理出需要的範圍
SELECT DISTINCT pd.*
INTO #tmpPackingList_Detail
FROM PackingList_Detail pd
INNER JOIN #tmp t  ON pd.OrderID = t.OrderID
			AND pd.ID = t.PackingListID
			AND pd.SizeCode= t.SizeCode
			AND pd.ShipQty=  t.ShipQty 

----紀錄用過的紙箱
DECLARE @Table as table(UsedCTNStartNo  int)
DECLARE @CartonBarcode as  varchar(30)
DECLARE @TargetCTNStartNo as int

DECLARE MYCURSOR CURSOR FOR  
SELECT DISTINCT CartonBarcode FROM #tmp ORDER BY CartonBarcode

-- 開啟游標
OPEN MYCURSOR 
FETCH NEXT FROM MYCURSOR INTO @CartonBarcode  ----逐一讀取
WHILE @@FETCH_STATUS = 0
BEGIN

	SELECT DISTINCT TOP 1 @TargetCTNStartNo = Cast(pd.CTNStartNo as int)
	FROM #tmpPackingList_Detail pd
	INNER JOIN (
	select * from #tmp 
	WHERE CartonBarcode= @CartonBarcode ) t  ON pd.OrderID = t.OrderID
				AND pd.ID = t.PackingListID
				AND pd.SizeCode= t.SizeCode
				AND pd.ShipQty=  t.ShipQty 
	WHERE  CAST(pd.CTNStartNo as int ) NOT IN (SELECT UsedCTNStartNo FROM @Table)
	ORDER BY Cast(pd.CTNStartNo as int)
	
	INSERT INTO @Table (UsedCTNStartNo) VALUES (@TargetCTNStartNo)

	UPDATE pd
	SET CustCTN = @CartonBarcode
	FROM PackingList_Detail pd
	WHERE pd.Ukey IN (
		SELECT Ukey
		FROM #tmpPackingList_Detail
		WHERE Cast(CTNStartNo as int)= @TargetCTNStartNo
	)
FETCH NEXT FROM MYCURSOR INTO  @CartonBarcode
END
CLOSE MYCURSOR
DEALLOCATE MYCURSOR


DROP TABLE #tmp ,#tmpPackingList_Detail

";
            }
            else
            {
                cmd = $@"
{tmpTable}


----整理出需要的範圍
SELECT DISTINCT pd.*
INTO #tmpPackingList_Detail
FROM PackingList_Detail pd
INNER JOIN #tmp t  ON pd.OrderID = t.OrderID
			AND pd.ID = t.PackingListID

----紀錄用過的紙箱
DECLARE @Table as table(UsedCTNStartNo  int)
DECLARE @CartonBarcode as  varchar(30)
DECLARE @TargetCTNStartNo as int

DECLARE MYCURSOR CURSOR FOR  
SELECT DISTINCT CartonBarcode FROM #tmp ORDER BY CartonBarcode

-- 開啟游標
OPEN MYCURSOR 
FETCH NEXT FROM MYCURSOR INTO @CartonBarcode  ----逐一讀取
WHILE @@FETCH_STATUS = 0
BEGIN

	SELECT DISTINCT TOP 1 @TargetCTNStartNo = Cast(pd.CTNStartNo as int)
	FROM #tmpPackingList_Detail pd
	INNER JOIN (
	select * from #tmp 
	WHERE CartonBarcode= @CartonBarcode ) t  ON pd.OrderID = t.OrderID
				AND pd.ID = t.PackingListID
	WHERE  CAST(pd.CTNStartNo as int ) NOT IN (SELECT UsedCTNStartNo FROM @Table)
	ORDER BY Cast(pd.CTNStartNo as int)
	
	INSERT INTO @Table (UsedCTNStartNo) VALUES (@TargetCTNStartNo)

	UPDATE pd
	SET CustCTN = @CartonBarcode
	FROM PackingList_Detail pd
	WHERE pd.Ukey IN (
		SELECT Ukey
		FROM #tmpPackingList_Detail
		WHERE Cast(CTNStartNo as int)= @TargetCTNStartNo
	)
FETCH NEXT FROM MYCURSOR INTO  @CartonBarcode
END
CLOSE MYCURSOR
DEALLOCATE MYCURSOR


DROP TABLE #tmp ,#tmpPackingList_Detail



";
            }
            #endregion

            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
            {
                DualResult r = DBProxy.Current.Execute(null, cmd);
                if (!r)
                {
                    transactionscope.Dispose();
                    return r;
                }
                else
                {
                    transactionscope.Complete();
                    return new DualResult(true);
                }
            }
        }

        /// <inheritdoc/>
        public class Packing_ExcelData
        {
            /// <inheritdoc/>
            public string FileName { get; set; }

            /// <summary>
            /// Excel上的HandlingUnitID
            /// </summary>
            public string HandlingUnitID { get; set; }

            /// <summary>
            /// Excel上的OrderNumber
            /// </summary>
            public string OrderNumber { get; set; }

            /// <summary>
            /// Excel上的ItemNumber
            /// </summary>
            public string ItemNumber { get; set; }

            /// <summary>
            /// Excel上的ShipDate
            /// </summary>
            public string ShipDate { get; set; }

            /// <summary>
            /// Excel上的HUQty
            /// </summary>
            public string HUQty { get; set; }

            /// <summary>
            /// 從Excel整理出來的CustPONO
            /// </summary>
            public string CustPONO { get; set; }

            /// <summary>
            /// 對應DB取得的OrderID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// 對應DB取得的CTnType
            /// </summary>
            public string CTnType { get; set; }

            /// <summary>
            /// 從Excel Material拆解出來的Article
            /// </summary>
            public string Article { get; set; }

            /// <summary>
            /// 對應Excel的SizeDescription
            /// </summary>
            public string SizeCode { get; set; }

            /// <summary>
            /// 對應Excel的ItemQuantity
            /// </summary>
            public string ShipQty { get; set; }

            /// <inheritdoc/>
            public string PackingListID { get; set; }

            /// <summary>
            /// 是否Mapping到多個PackingListID
            /// </summary>
            public bool MultipleMatches { get; set; }

            /// <summary>
            /// 確定要更新到DB
            /// </summary>
            public bool ExecuteUpdate { get; set; }

            /// <summary>
            /// 重複的PackingListID
            /// </summary>
            public List<string> PackingList_Candidate { get; set; }
        }

        /// <inheritdoc/>
        public class Item_ExcelData
        {
            /// <inheritdoc/>
            public string FileName { get; set; }

            /// <summary>
            /// Excel上的PO No.
            /// </summary>
            public string Pono { get; set; }

            /// <summary>
            /// Excel上的PO Item No.
            /// </summary>
            public string POItemNo { get; set; }

            /// <summary>
            /// Excel上的Size
            /// </summary>
            public string Size { get; set; }

            /// <summary>
            /// 從Excel整理出來的CustPONO
            /// </summary>
            public string CustPONO { get; set; }

            /// <summary>
            /// 對應DB取得的OrderID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// 從Excel Material拆解出來的Article
            /// </summary>
            public string Article { get; set; }

            /// <summary>
            /// 對應Excel的SizeDescription
            /// </summary>
            public string SizeCode { get; set; }

            /// <summary>
            /// 對應Excel的ItemQuantity
            /// </summary>
            public string ShipQty { get; set; }

            /// <inheritdoc/>
            public string PackingListID { get; set; }

            /// <summary>
            /// 是否Mapping到多個PackingListID
            /// </summary>
            public bool MultipleMatches { get; set; }

            /// <summary>
            /// 確定要更新到DB
            /// </summary>
            public bool ExecuteUpdate { get; set; }

            /// <summary>
            /// 重複的PackingListID
            /// </summary>
            public List<string> PackingList_Candidate { get; set; }
        }

        /// <summary>
        /// 呈現在右方Match的表格物件
        /// </summary>
        public class MatchGridData
        {
            /// <inheritdoc/>
            public string FileName { get; set; }

            /// <inheritdoc/>
            public string PackingListID { get; set; }

            /// <summary>
            /// 是否Mapping到多個PackingListID
            /// </summary>
            public bool MultipleMatches { get; set; }

            /// <summary>
            /// 是否已經有SSCC，用於決定Overwrite能不能勾選
            /// </summary>
            public bool ExistsSSCC { get; set; }

            /// <summary>
            /// 是否覆寫現有資料
            /// </summary>
            public bool Overwrite { get; set; }

            /// <summary>
            /// 重複的PackingListID
            /// </summary>
            public List<string> PackingList_Candidate { get; set; }

            /// <summary>
            /// Excel上的Order Number
            /// </summary>
            public string OrderNumber { get; set; }

            /// <summary>
            /// Excel上的Item Number
            /// </summary>
            public DateTime ShipDate { get; set; }

            /// <summary>
            /// 對應DB取得的OrderID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// 從Excel Material拆解出來的Article
            /// </summary>
            public string Article { get; set; }

            /// <summary>
            /// 對應Excel的SizeDescription
            /// </summary>
            public string SizeCode { get; set; }

            /// <summary>
            /// 對應Excel的ItemQuantity
            /// </summary>
            public string ShipQty { get; set; }
        }

        /// <inheritdoc/>
        // PackingListCandidate的資料來源
        public class PackingListCandidate_Datasource
        {
            /// <inheritdoc/>
            public string FileName { get; set; }

            /// <inheritdoc/>
            public List<string> PackingList_Candidate { get; set; }
        }
    }
}

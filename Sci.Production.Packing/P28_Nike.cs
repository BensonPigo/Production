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
    public partial class P28_Nike : Sci.Win.Tems.QueryForm
    {
        private List<ExcelData> ExcelDatas = new List<ExcelData>();
        private List<PackingListCandidate_Datasource> PackingListCandidate_Datasources = new List<PackingListCandidate_Datasource>();

        /// <inheritdoc/>
        public P28_Nike()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            DataGridViewGeneratorTextColumnSettings col_PackingListCandidate = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings col_Overwrite = new DataGridViewGeneratorCheckBoxColumnSettings();

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

            this.gridFile.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridFile)
              .Text("FileName", header: "FileName", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .Text("Result", header: "Result", width: Widths.AnsiChars(13), iseditingreadonly: true)
              ;

            this.gridMatch.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridMatch)
              .Text("PONumber", header: "PONumber", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .Text("POItem", header: "POItem", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .Text("PONumber", header: "PONumber", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .Text("PLPOItemTotalCartons", header: "PLPOItem" + Environment.NewLine + "TotalCartons", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .CheckBox("MultipleMatches", header: "Multiple" + Environment.NewLine + "Matches", width: Widths.AnsiChars(4), iseditable: false)
              .Text("PackingListID", header: "P/L" + Environment.NewLine + "Candidate", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: col_PackingListCandidate)
              .CheckBox("Overwrite", header: "Overwrite", trueValue: 1, falseValue: 0, width: Widths.AnsiChars(5), iseditable: true, settings: col_Overwrite)
              ;
        }

        /// <inheritdoc/>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <inheritdoc/>
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.ShowWaitMessage("Processing...");
            this.BindingSourceFile.DataSource = null;
            this.BindingSourceMatch.DataSource = null;
            this.PackingListCandidate_Datasources = new List<PackingListCandidate_Datasource>();
            this.ExcelDatas = new List<ExcelData>();

            // 取得所有檔名
            string[] files_SafeFileNames = this.openFileDialog1.SafeFileNames;
            string[] files = this.openFileDialog1.FileNames;

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
                    List<string> mustColumn = new List<string>() { "PONumber", "POItem", "PLPOItemTotalCartons", "Material", "CartonBarcode", "SizeDescription", "ItemQuantity" };

                    // 紀錄必要欄位橫向的欄位位置
                    int idx_PONumber = 0;
                    int idx_POItem = 0;
                    int idx_PLPOItemTotalCartons = 0;
                    int idx_Material = 0;
                    int idx_CartonBarcode = 0;
                    int idx_SizeDescription = 0;
                    int idx_ItemQuantity = 0;

                    for (int x = 1; x <= intColumnsCount; x++)
                    {
                        var colName = worksheet.Cells[1, x].Value;

                        switch (colName)
                        {
                            case "PONumber":
                                idx_PONumber = x;
                                mustColumn.Remove("PONumber");
                                break;
                            case "POItem":
                                idx_POItem = x;
                                mustColumn.Remove("POItem");
                                break;
                            case "PLPOItemTotalCartons":
                                idx_PLPOItemTotalCartons = x;
                                mustColumn.Remove("PLPOItemTotalCartons");
                                break;
                            case "Material":
                                idx_Material = x;
                                mustColumn.Remove("Material");
                                break;
                            case "CartonBarcode":
                                idx_CartonBarcode = x;
                                mustColumn.Remove("CartonBarcode");
                                break;
                            case "SizeDescription":
                                idx_SizeDescription = x;
                                mustColumn.Remove("SizeDescription");
                                break;
                            case "ItemQuantity":
                                idx_ItemQuantity = x;
                                mustColumn.Remove("ItemQuantity");
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
                        ExcelData excelData = new ExcelData() { FileName = safeFileName };

                        // PONumber
                        var pONumber = worksheet.Cells[intRowsReading, idx_PONumber].Value;

                        // POItem
                        var pOItem = worksheet.Cells[intRowsReading, idx_POItem].Value;

                        // Material
                        var material = worksheet.Cells[intRowsReading, idx_Material].Value;

                        // PLPOItemTotalCartons
                        var pLPOItemTotalCartons = worksheet.Cells[intRowsReading, idx_PLPOItemTotalCartons].Value;

                        // CartonBarcode
                        var cartonBarcode = worksheet.Cells[intRowsReading, idx_CartonBarcode].Value;

                        // SizeDescription
                        var sizeDescription = worksheet.Cells[intRowsReading, idx_SizeDescription].Value;

                        // ItemQuantity
                        var itemQuantity = worksheet.Cells[intRowsReading, idx_ItemQuantity].Value;

                        excelData.PONumber = MyUtility.Convert.GetString(pONumber);
                        excelData.POItem = MyUtility.Convert.GetString(pOItem);
                        excelData.PLPOItemTotalCartons = MyUtility.Convert.GetString(pLPOItemTotalCartons);
                        excelData.Material = MyUtility.Convert.GetString(material);
                        excelData.CartonBarcode = MyUtility.Convert.GetString(cartonBarcode);
                        excelData.SizeDescription = MyUtility.Convert.GetString(sizeDescription);
                        excelData.ItemQuantity = MyUtility.Convert.GetString(itemQuantity);
                        excelData.ExecuteUpdate = false;

                        this.ExcelDatas.Add(excelData);
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

            this.BindingSourceFile.DataSource = gridData;
            this.HideWaitMessage();
        }

        /// <inheritdoc/>
        private void BtnRemoveFile_Click(object sender, EventArgs e)
        {
            this.BindingSourceFile.DataSource = null;
            this.BindingSourceMatch.DataSource = null;
            this.ExcelDatas = new List<ExcelData>();
        }

        /// <inheritdoc/>
        private void BtnMapping_Click(object sender, EventArgs e)
        {
            this.BindingSourceMatch.DataSource = null;

            try
            {
                List<ExcelData> groupDatas = this.ExcelDatas
                    .GroupBy(o => new { o.PONumber, o.POItem, o.PLPOItemTotalCartons })
                    .Select(o => o.First()).Distinct().ToList();

                // 取得SP#(相同PONumber + POItem = 相同 SP#)
                foreach (var obj in groupDatas)
                {
                    string custPONO = obj.PONumber + "-" + obj.POItem;
                    string sp = MyUtility.GetValue.Lookup($"SELECT ID FROM Orders WHERE BrandID = 'Nike' AND CustPONo = '{custPONO}' ");

                    if (MyUtility.Check.Empty(sp))
                    {
                        string sqlcmd = string.Empty;
                        DBProxy.Current.Select(null, "SELECT  Customize1,Customize2,Customize3 FROM Brand WHERE ID = 'Nike'", out DataTable dt);
                        foreach (DataColumn c in dt.Columns)
                        {
                            DataRow[] dr = dt.Select($"{c.ColumnName} = 'TRADING PO'");
                            if (dr.Length > 0)
                            {
                                // pOItem 要比對 CustPONo "-" 後的兩碼
                                sqlcmd = $"SELECT ID FROM Orders WHERE {c.ColumnName} = '{obj.PONumber}' and IIF(CustPONo not like '%-%','',SUBSTRING(CustPONo, CHARINDEX('-', CustPONo) + 1, 10) ) = '{obj.POItem}' AND  BrandID = 'Nike'";
                                break;
                            }
                        }

                        if (!MyUtility.Check.Empty(sqlcmd))
                        {
                            sp = MyUtility.GetValue.Lookup(sqlcmd);
                        }
                    }

                    this.ExcelDatas.Where(o => o.PONumber == obj.PONumber && o.POItem == obj.POItem).ToList().ForEach(i => i.OrderID = sp);
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
        private List<MatchGridData> MapPackingList(ExcelData groupData)
        {
            List<MatchGridData> matchGridDatas = new List<MatchGridData>();
            string fileName = groupData.FileName;

            // 先把Excel資訊抓出來
            foreach (var excelData in this.ExcelDatas)
            {
                excelData.Article = excelData.Material.Split('-').Length > 1 ? excelData.Material.Split('-')[1] : excelData.Material;
                excelData.SizeCode = excelData.SizeDescription;
                excelData.ShipQty = excelData.ItemQuantity;
            }

            // 找出同一群的資料
            var samePack = this.ExcelDatas.Where(o => o.PONumber == groupData.PONumber && o.POItem == groupData.POItem && o.PLPOItemTotalCartons == groupData.PLPOItemTotalCartons).ToList();

            // 不想用ProcessWithDatatable，把物件手動弄成temp table
            string tmpTable = string.Empty;
            int count = 1;
            foreach (var excelData in samePack)
            {
                string tmp = $@"SELECT [OrderID]='{excelData.OrderID}',[TotalCartons]='{excelData.PLPOItemTotalCartons}',[Article]='{excelData.Article}',[SizeCode]='{excelData.SizeCode}',[ShipQty]='{excelData.ShipQty}'";

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
	AND p.ShipQty=t.TotalCartons
	AND pd.Article=t.Article
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
                PONumber = groupData.PONumber,
                POItem = groupData.POItem,
                PLPOItemTotalCartons = groupData.PLPOItemTotalCartons,
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

                    // 真的有多個對應到
                    if (packingListIDs.Count() > 1)
                    {
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

            return matchGridDatas;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (!this.ConfirmCheck())
            {
                return;
            }

            try
            {
                this.ShowWaitMessage("Processing...");

                DataTable fileDt = (DataTable)this.BindingSourceFile.DataSource;

                // 取得要更新的File Name
                DataTable dt = (DataTable)this.BindingSourceMatch.DataSource;
                var needUpdateFile = dt.AsEnumerable()
                    .Where(o => (MyUtility.Convert.GetBool(o["ExistsSSCC"]) && MyUtility.Convert.GetBool(o["Overwrite"])) || !MyUtility.Convert.GetBool(o["ExistsSSCC"]))
                    .Select(o => new { Filename = MyUtility.Convert.GetString(o["Filename"]), PackingListID = MyUtility.Convert.GetString(o["PackingListID"]) }).Distinct();

                // 並不是所有資料都需要更新，因此加上註記
                foreach (var item in needUpdateFile)
                {
                    string packingListID = item.PackingListID;
                    this.ExcelDatas.Where(o => o.FileName == item.Filename && o.PackingListID == packingListID).ToList().ForEach(o =>
                    {
                        o.PackingListID = packingListID;
                        o.ExecuteUpdate = true;
                    });

                    // 開始更新
                    DualResult r = this.UpdateDatabase(this.ExcelDatas.Where(o => o.PackingListID == packingListID).ToList());

                    if (r)
                    {
                        fileDt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == item.Filename).FirstOrDefault()["Result"] = "Success";

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
                        fileDt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == item.Filename).FirstOrDefault()["Result"] = "Fail";
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

        private DualResult UpdateDatabase(List<ExcelData> excelDatas)
        {
            // 不想用ProcessWithDatatable，把物件手動弄成temp table
            string tmpTable = string.Empty;
            int count = 1;
            var everyCarton = excelDatas.GroupBy(o => new { o.PackingListID, o.OrderID, o.Article, o.SizeCode, o.ShipQty, o.CartonBarcode })
                                        .Select(o => o.First()).Distinct().ToList();

            foreach (var excelData in everyCarton)
            {
                string tmp = $@"SELECT [PackingListID]='{excelData.PackingListID}',[OrderID]='{excelData.OrderID}',[Article]='{excelData.Article}',[SizeCode]='{excelData.SizeCode}',[ShipQty]='{excelData.ShipQty}',[CartonBarcode]='{excelData.CartonBarcode}'";

                tmpTable += tmp + Environment.NewLine;

                if (count == 1)
                {
                    tmpTable += $"INTO #tmp" + Environment.NewLine;
                }

                if (excelDatas.Count > count)
                {
                    tmpTable += "UNION" + Environment.NewLine;
                }

                count++;
            }

            #region SQL
            string cmd = $@"
{tmpTable}

----整理出需要的範圍
SELECT DISTINCT pd.*
INTO #tmpPackingList_Detail
FROM PackingList_Detail pd
INNER JOIN #tmp t  ON pd.OrderID = t.OrderID
			AND pd.ID = t.PackingListID
			AND pd.Article= t.Article
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
				AND pd.Article= t.Article
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
        public class ExcelData
        {
            /// <inheritdoc/>
            public string FileName { get; set; }

            /// <summary>
            /// Excel上的PONumber
            /// </summary>
            public string PONumber { get; set; }

            /// <summary>
            /// Excel上的POItem
            /// </summary>
            public string POItem { get; set; }

            /// <summary>
            /// Excel上的PLPOItemTotalCartons
            /// </summary>
            public string PLPOItemTotalCartons { get; set; }

            /// <summary>
            /// Excel上的Material
            /// </summary>
            public string Material { get; set; }

            /// <summary>
            /// Excel上的CartonBarcode
            /// </summary>
            public string CartonBarcode { get; set; }

            /// <summary>
            /// Excel上的SizeDescription
            /// </summary>
            public string SizeDescription { get; set; }

            /// <summary>
            /// Excel上的ItemQuantity
            /// </summary>
            public string ItemQuantity { get; set; }

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
            /// Excel上的PONumber
            /// </summary>
            public string PONumber { get; set; }

            /// <summary>
            /// Excel上的POItem
            /// </summary>
            public string POItem { get; set; }

            /// <summary>
            /// Excel上的PLPOItemTotalCartons
            /// </summary>
            public string PLPOItemTotalCartons { get; set; }

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

using Ict;
using Ict.Win;
using Microsoft.ReportingServices.Interfaces;
using Sci.Production.Class.Command;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Planning
{
    /// <inheritdoc/>
    public partial class P08 : Win.Tems.QueryForm
    {
        private readonly string[] CuttingRemarkSource = new[]
        {
            "Short lead time",
            "Pending",
            "Ongoing cutting",
            "Quality issue",
            "Waiting for replacement",
            "No fabrics release from CWHS",
            "Lacking yardage / lacking lays",
            "Lacking combination",
            "Wrong arrival",
        };

        private readonly string[] LoadingRemarkSource = new[]
        {
            "Short lead time",
            "Pending",
            "Ongoing subprocess",
            "Material issue",
            "Quality issue",
        };

        private readonly string[] SubprocessRemarkSource = new[]
        {
            "Short lead time",
            "Pending",
            "Ongoing cutting",
            "Ongoing subprocess",
            "Material issue",
            "Quality issue",
        };

        private DataTable dataTable = new DataTable();

        /// <summary>
        /// 展開: By SewingLine, Sewing Date, SP, Factory(已是必輸入條件)
        /// </summary>
        /// <inheritdoc/>
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query(true);
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("SewingLineID", header: "Sewing Line", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Date("SewingDate", header: "Sewing Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("OrderID", header: "SP", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Date("Inline", header: "Sewing Inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("Offline", header: "Sewing Offline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("StdQty", header: "Standard Output/Day", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("CuttingOutput", header: "Cutting Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .ComboBox("CuttingRemark", header: "Cutting Remark", width: Widths.AnsiChars(25), settings: this.ComboBoxCuttingRemark())
                .Numeric("LoadingOutput", header: "Loading Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .ComboBox("LoadingRemark", header: "Loading Remark", width: Widths.AnsiChars(17), settings: this.ComboBoxLoadingRemark())
                .CheckBox("LoadingExclusion", header: "Loading Exclusion", width: Widths.AnsiChars(1), trueValue: true, falseValue: false, settings: this.CheckBoxExclusion())
                .Numeric("ATOutput", header: "AT Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .ComboBox("ATRemark", header: "AT Remark", width: Widths.AnsiChars(17), settings: this.ComboBoxSubprocessRemark())
                .CheckBox("ATExclusion", header: "AT Exclusion", width: Widths.AnsiChars(1), trueValue: true, falseValue: false, settings: this.CheckBoxExclusion())
                .Numeric("AUTOutput", header: "AUT Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .ComboBox("AUTRemark", header: "AUT Remark", width: Widths.AnsiChars(17), settings: this.ComboBoxSubprocessRemark())
                .CheckBox("AUTExclusion", header: "AUT Exclusion", width: Widths.AnsiChars(1), trueValue: true, falseValue: false, settings: this.CheckBoxExclusion())
                .Numeric("HTOutput", header: "HT Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .ComboBox("HTRemark", header: "HT Remark", width: Widths.AnsiChars(17), settings: this.ComboBoxSubprocessRemark())
                .CheckBox("HTExclusion", header: "HT Exclusion", width: Widths.AnsiChars(1), trueValue: true, falseValue: false, settings: this.CheckBoxExclusion())
                .Numeric("BOOutput", header: "BO Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .ComboBox("BORemark", header: "BO Remark", width: Widths.AnsiChars(17), settings: this.ComboBoxSubprocessRemark())
                .CheckBox("BOExclusion", header: "BO Exclusion", width: Widths.AnsiChars(1), trueValue: true, falseValue: false, settings: this.CheckBoxExclusion())
                .Numeric("FMOutput", header: "FM Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .ComboBox("FMRemark", header: "FM Remark", width: Widths.AnsiChars(17), settings: this.ComboBoxSubprocessRemark())
                .CheckBox("FMExclusion", header: "FM Exclusion", width: Widths.AnsiChars(1), trueValue: true, falseValue: false, settings: this.CheckBoxExclusion())
                .Numeric("PRTOutput", header: "PRT Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .ComboBox("PRTRemark", header: "PRT Remark", width: Widths.AnsiChars(17), settings: this.ComboBoxSubprocessRemark())
                .CheckBox("PRTExclusion", header: "PRT Exclusion", width: Widths.AnsiChars(1), trueValue: true, falseValue: false, settings: this.CheckBoxExclusion())
                ;
        }

        private DataGridViewGeneratorComboBoxColumnSettings ComboBoxCuttingRemark()
        {
            return new DataGridViewGeneratorComboBoxColumnSettings
            {
                DataSource = new BindingSource(this.CuttingRemarkSource.ToDictionary(item => item, item => item), null),
                ValueMember = "Key",
                DisplayMember = "Value",
            };
        }

        private DataGridViewGeneratorComboBoxColumnSettings ComboBoxLoadingRemark()
        {
            return new DataGridViewGeneratorComboBoxColumnSettings
            {
                DataSource = new BindingSource(this.LoadingRemarkSource.ToDictionary(item => item, item => item), null),
                ValueMember = "Key",
                DisplayMember = "Value",
            };
        }

        private DataGridViewGeneratorComboBoxColumnSettings ComboBoxSubprocessRemark()
        {
            return new DataGridViewGeneratorComboBoxColumnSettings
            {
                DataSource = new BindingSource(this.SubprocessRemarkSource.ToDictionary(item => item, item => item), null),
                ValueMember = "Key",
                DisplayMember = "Value",
            };
        }

        private DataGridViewGeneratorCheckBoxColumnSettings CheckBoxExclusion()
        {
            var settings = new DataGridViewGeneratorCheckBoxColumnSettings();
            settings.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                string columnName = ((DataGridViewColumn)s).DataPropertyName;
                this.dataTable.Select($"SewingLineID = '{dr["SewingLineID"]}' AND OrderID = '{dr["OrderID"]}'").AsEnumerable().ToList().ForEach(row => row[columnName] = e.FormattedValue);
            };

            return settings;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (!this.QueryBefore())
            {
                return;
            }

            this.ChangeGridReadOnly(true);
            this.Query();
        }

        private bool QueryBefore()
        {
            // MDivision, Factory 不可空
            if (MyUtility.Check.Empty(this.txtMdivision1.Text) || MyUtility.Check.Empty(this.txtfactory1.Text))
            {
                MyUtility.Msg.WarningBox("MDivision and Factory can not be empty!");
                return false;
            }

            // 3 個日期條件至少輸入一個
            if (!this.dateSewing.HasValue1 && !this.dateSewingInline.HasValue1 && !this.dateSewingOffline.HasValue1)
            {
                MyUtility.Msg.WarningBox("Date can not all be empty!");
                return false;
            }

            return true;
        }

        private void Query(bool onlySchema = false)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Planning_P08 biModel = new Planning_P08();
            try
            {
                Planning_P08_ViewModel viewModel = new Planning_P08_ViewModel()
                {
                    MDivisionID = this.txtMdivision1.Text,
                    FactoryID = this.txtfactory1.Text,
                    SewingSDate = this.dateSewing.Value1,
                    SewingEDate = this.dateSewing.Value2,
                    SewingInlineSDate = this.dateSewingInline.Value1,
                    SewingInlineEDate = this.dateSewingInline.Value2,
                    SewingOfflineSDate = this.dateSewingOffline.Value1,
                    SewingOfflineEDate = this.dateSewingOffline.Value2,
                    OnlySchema = onlySchema,
                };

                Base_ViewModel resultReport = biModel.GetPlanning_P08(viewModel);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                this.dataTable = resultReport.Dt;
                this.grid1.DataSource = this.dataTable;
                finalResult.Result = new Ict.DualResult(true);
                if (this.dataTable.Rows.Count == 0 && !onlySchema)
                {
                    MyUtility.Msg.InfoBox("Data not dound");
                }
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            if (!finalResult.Result)
            {
                this.ShowErr(finalResult.Result);
                return;
            }
        }

        private void BtnDownloadTemplate_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = new DataTable();
            if (this.dataTable.Rows.Count == 0)
            {
                return;
            }

            foreach (DataColumn col in this.dataTable.Columns)
            {
                if (col.ColumnName.Contains("Exclusion"))
                {
                    // 將包含 "Exclusion" 的欄位設定為字串類型
                    dtPrint.Columns.Add(col.ColumnName, typeof(string));
                }
                else
                {
                    // 其他欄位保持原類型
                    dtPrint.Columns.Add(col.ColumnName, col.DataType);
                }
            }

            // 複製資料並處理 "Exclusion" 欄位
            foreach (DataRow dr in this.dataTable.Rows)
            {
                DataRow newRow = dtPrint.NewRow();
                foreach (DataColumn col in this.dataTable.Columns)
                {
                    if (col.ColumnName.Contains("Exclusion"))
                    {
                        // 將布林值轉換為 "Y" 或 "N"
                        newRow[col.ColumnName] = MyUtility.Convert.GetBool(dr[col]) ? "Y" : "N";
                    }
                    else
                    {
                        // 其他欄位直接複製
                        newRow[col.ColumnName] = dr[col];
                    }
                }

                dtPrint.Rows.Add(newRow);
            }

            string fileName = "Planning_P08_Import";
            string fileNameXltx = fileName + ".xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileNameXltx}");
            MyUtility.Excel.CopyToXls(dtPrint, null, fileNameXltx, 1, false, excelApp: excelApp);
            excelApp.Visible = true;

            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];
            int dataCount = dtPrint.Rows.Count + 1;

            // 設置資料驗證和背景顏色
            int[] regularColumns = new[] { 10 }; // Cutting
            this.SetValidationAndColor(worksheet, 2, dataCount, regularColumns, "=DataSource!$A$2:$A$10");
            regularColumns = new[] { 12 }; // Loading
            this.SetValidationAndColor(worksheet, 2, dataCount, regularColumns, "=DataSource!$B$2:$B$6");
            regularColumns = new[] { 15, 18, 21, 24, 27, 30 }; // AT, AUT, HT, BO, FM, PRT
            this.SetValidationAndColor(worksheet, 2, dataCount, regularColumns, "=DataSource!$C$2:$C$7");
            int[] exclusionColumns = new[] { 13, 16, 19, 22, 25, 28, 31 }; // Exclusion columns
            this.SetValidationAndColor(worksheet, 2, dataCount, exclusionColumns, "=DataSource!$D$2:$D$3");

            excelApp.Visible = true;
        }

        private void SetValidationAndColor(Excel.Worksheet worksheet, int startRow, int endRow, int[] columnRanges, string rangeFormula)
        {
            foreach (var columnRange in columnRanges)
            {
                Excel.Range columnRangeRange = worksheet.Range[worksheet.Cells[startRow, columnRange], worksheet.Cells[endRow, columnRange]];
                columnRangeRange.Interior.Color = ColorTranslator.ToOle(Color.Pink);

                Excel.Validation validation = columnRangeRange.Validation;
                validation.Delete();
                validation.Add(
                    Excel.XlDVType.xlValidateList,
                    Excel.XlDVAlertStyle.xlValidAlertStop,
                    Excel.XlFormatConditionOperator.xlBetween,
                    rangeFormula,
                    Type.Missing);

                validation.IgnoreBlank = true;
                validation.InCellDropdown = true;
                validation.ShowInput = true;
                validation.ShowError = true;
            }
        }

        private void BtnExcelImport_Click(object sender, EventArgs e)
        {
            this.grid1.DataSource = null;
            this.ChangeGridReadOnly(true);

            this.openFileDialog1.Filter = "Excel files (*.xlsx;*.xls;*.xlt)|*.xlsx;*.xls;*.xlt";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // 清空Grid資料
            this.dataTable.Clear();

            this.ShowWaitMessage("Excel import...");
            Excel.Application excel = new Excel.Application();
            try
            {
                excel.Workbooks.Open(this.openFileDialog1.FileName);
                excel.Visible = false;
                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                Excel.Range usedRange = worksheet.UsedRange;

                // 獲取行數和列數
                int rowCount = usedRange.Rows.Count;
                int columnCount = usedRange.Columns.Count;

                // 從第二列開始逐行讀取資料
                for (int row = 2; row <= rowCount; row++)
                {
                    DataRow dataRow = this.dataTable.NewRow();
                    for (int col = 1; col <= columnCount; col++)
                    {
                        string columnName = worksheet.Cells[1, col].Text.ToString();
                        string value = worksheet.Cells[row, col].Text.ToString();
                        if (columnName.Contains("Exclusion"))
                        {
                            // 若為 Exclusion 欄位，將 "Y"/"N" 轉換為 bool
                            dataRow[col - 1] = value == "Y";
                        }
                        else
                        {
                            dataRow[col - 1] = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
                        }
                    }

                    this.dataTable.Rows.Add(dataRow);
                }

                this.ProcessExclusion();
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox($"An error occurred: {ex.Message}");
            }
            finally
            {
                // 釋放Excel資源
                if (excel.Workbooks != null)
                {
                    excel.Workbooks.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel.Workbooks);
                }

                if (excel != null)
                {
                    excel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                this.grid1.DataSource = this.dataTable;
                this.HideWaitMessage();
            }
        }

        private void ProcessExclusion()
        {
            // 先取得所有包含 "Exclusion" 的欄位
            var exclusionColumns = this.dataTable.Columns.Cast<DataColumn>()
                                                .Where(c => c.ColumnName.Contains("Exclusion"))
                                                .ToList();

            // 遍歷每一組 SewingLineID 和 OrderID 相同的資料列
            var groupedRows = this.dataTable.AsEnumerable()
                                            .GroupBy(row => new { SewingLineID = row["SewingLineID"], OrderID = row["OrderID"] });

            foreach (var group in groupedRows)
            {
                foreach (var exclusionColumn in exclusionColumns)
                {
                    // 檢查該組中是否有任何資料列的 "Exclusion" 欄位為 true
                    bool hasExclusionTrue = group.Any(row => Convert.ToBoolean(row[exclusionColumn]) == true);

                    if (hasExclusionTrue)
                    {
                        // 如果有為 true 的話，將該組所有資料列的該 "Exclusion" 欄位設定為 true
                        foreach (var row in group)
                        {
                            row[exclusionColumn] = true;
                        }
                    }
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            this.ChangeGridReadOnly(false);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            this.ChangeGridReadOnly(true);
            this.PreSave();

            // update
            string sqlcmd = $@"
UPDATE s
SET [SewingInLine]         = t.Inline
   ,[SewingOffLine]        = t.Offline
   ,[StandardOutputPerDay] = ISNULL(t.StdQty, 0)
   ,[CuttingRemark]        = ISNULL(t.[CuttingRemark], '')
   ,[LoadingRemark]        = ISNULL(t.[LoadingRemark], '')
   ,[LoadingExclusion]     = ISNULL(t.[LoadingExclusion], 0)
   ,[ATRemark]             = ISNULL(t.[ATRemark], '')
   ,[ATExclusion]          = ISNULL(t.[ATExclusion], 0)
   ,[AUTRemark]            = ISNULL(t.[AUTRemark], '')
   ,[AUTExclusion]         = ISNULL(t.[AUTExclusion], 0)
   ,[HTRemark]             = ISNULL(t.[HTRemark], '')
   ,[HTExclusion]          = ISNULL(t.[HTExclusion], 0)
   ,[BORemark]             = ISNULL(t.[BORemark], '')
   ,[BOExclusion]          = ISNULL(t.[BOExclusion], 0)
   ,[FMRemark]             = ISNULL(t.[FMRemark], '')
   ,[FMExclusion]          = ISNULL(t.[FMExclusion], 0)
   ,[PRTRemark]            = ISNULL(t.[PRTRemark], '')
   ,[PRTExclusion]         = ISNULL(t.[PRTExclusion], 0)
   ,EditName               = '{Sci.Env.User.UserID}'
   ,EditDate               = GETDATE()
FROM #tmp t
INNER JOIN [SewingDailyOutputStatusRecord] s
	 ON t.SewingLineID = s.SewingLineID
	AND t.SewingDate   = s.SewingOutputDate
	AND t.FactoryID    = s.FactoryID
	AND t.OrderID      = s.OrderID
";

            // insert
            sqlcmd += $@"
INSERT INTO [dbo].[SewingDailyOutputStatusRecord]
           ([SewingLineID]
           ,[SewingOutputDate]
           ,[FactoryID]
           ,[OrderID]
           ,[SewingInLine]
           ,[SewingOffLine]
           ,[StandardOutputPerDay]
           ,[CuttingRemark]
           ,[LoadingRemark]
           ,[LoadingExclusion]
           ,[ATRemark]
           ,[ATExclusion]
           ,[AUTRemark]
           ,[AUTExclusion]
           ,[HTRemark]
           ,[HTExclusion]
           ,[BORemark]
           ,[BOExclusion]
           ,[FMRemark]
           ,[FMExclusion]
           ,[PRTRemark]
           ,[PRTExclusion]
           ,[AddName]
           ,[AddDate])
SELECT
	 SewingLineID
	,SewingDate
	,FactoryID
	,OrderID
	,Inline
	,Offline
	,ISNULL(StdQty, 0)
	,ISNULL(CuttingRemark, '')
    ,ISNULL(LoadingRemark, '')
    ,ISNULL(LoadingExclusion, 0)
    ,ISNULL(ATRemark, '')
    ,ISNULL(ATExclusion, 0)
    ,ISNULL(AUTRemark, '')
    ,ISNULL(AUTExclusion, 0)
    ,ISNULL(HTRemark, '')
    ,ISNULL(HTExclusion, 0)
    ,ISNULL(BORemark, '')
    ,ISNULL(BOExclusion, 0)
    ,ISNULL(FMRemark, '')
    ,ISNULL(FMExclusion, 0)
    ,ISNULL(PRTRemark, '')
    ,ISNULL(PRTExclusion, 0)
	,'{Sci.Env.User.UserID}'
	,GETDATE()
FROM #tmp t
WHERE NOT EXISTS (
    SELECT 1 FROM SewingDailyOutputStatusRecord s
    WHERE t.SewingLineID = s.SewingLineID
	AND   t.SewingDate   = s.SewingOutputDate
	AND   t.FactoryID    = s.FactoryID
	AND   t.OrderID      = s.OrderID)
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dataTable, string.Empty, sqlcmd, out DataTable _);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Successfully");
        }

        private void PreSave()
        {
            foreach (DataRow dr in this.dataTable.Rows)
            {
                // Cutting output
                if (MyUtility.Convert.GetInt(dr["CuttingOutput"]) == MyUtility.Convert.GetInt(dr["StdQty"]))
                {
                    dr["CuttingRemark"] = string.Empty;
                }

                // Loading
                if (MyUtility.Convert.GetInt(dr["LoadingOutput"]) == MyUtility.Convert.GetInt(dr["StdQty"]))
                {
                    dr["LoadingRemark"] = string.Empty;
                }

                if (MyUtility.Convert.GetInt(dr["ATOutput"]) == MyUtility.Convert.GetInt(dr["StdQty"]))
                {
                    dr["ATRemark"] = string.Empty;
                }

                if (MyUtility.Convert.GetInt(dr["AUTOutput"]) == MyUtility.Convert.GetInt(dr["StdQty"]))
                {
                    dr["AUTRemark"] = string.Empty;
                }

                if (MyUtility.Convert.GetInt(dr["HTOutput"]) == MyUtility.Convert.GetInt(dr["StdQty"]))
                {
                    dr["HTRemark"] = string.Empty;
                }

                if (MyUtility.Convert.GetInt(dr["BOOutput"]) == MyUtility.Convert.GetInt(dr["StdQty"]))
                {
                    dr["BORemark"] = string.Empty;
                }

                if (MyUtility.Convert.GetInt(dr["FMOutput"]) == MyUtility.Convert.GetInt(dr["StdQty"]))
                {
                    dr["FMRemark"] = string.Empty;
                }

                if (MyUtility.Convert.GetInt(dr["PRTOutput"]) == MyUtility.Convert.GetInt(dr["StdQty"]))
                {
                    dr["PRTRemark"] = string.Empty;
                }
            }
        }

        private void ChangeGridReadOnly(bool isReadonly)
        {
            this.grid1.IsEditingReadOnly = isReadonly;

            this.grid1.Columns["CuttingRemark"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["LoadingRemark"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["LoadingExclusion"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["ATRemark"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["ATExclusion"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["AUTRemark"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["AUTExclusion"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["HTRemark"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["HTExclusion"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["BORemark"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["BOExclusion"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["FMRemark"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["FMExclusion"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["PRTRemark"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
            this.grid1.Columns["PRTExclusion"].DefaultCellStyle.BackColor = isReadonly ? Color.White : Color.Pink;
        }
    }
}

using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.CallPmsAPI.PackingA2BWebAPI_Model;

namespace Sci.Production.Planning
{
    /// <inheritdoc/>
    public partial class P07_Import : Win.Subs.Base
    {
        private DataTable gridData = new DataTable();
        private string P07_FileName = string.Empty;
        private Microsoft.Office.Interop.Excel.Application excel;

        /// <inheritdoc/>
        public P07_Import()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string sqlcmd = $@"SELECT * from DailyAccuCPULoading_Detail where 1=0";

            DualResult dualResult = DBProxy.Current.Select(null, "SELECT * from DailyAccuCPULoading_Detail where 1=0", out this.gridData);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = this.gridData;
            this.gridBatchImport.DataSource = this.listControlBindingSource1;

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.gridBatchImport)
            .Text("Date", header: "Date", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("WeekDay", header: "WeekDay", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("DailyCPULoading", header: "Daily" + Environment.NewLine + "CPU Loading", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("NewTarget", header: "New Target base on Actual" + Environment.NewLine + "output and left working days", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("ActCPUPerformed", header: "Actual CPU " + Environment.NewLine + "Performed", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 3)
            .Numeric("DailyCPUVarience", header: "Daily CPU Varience" + Environment.NewLine + "(based on new target)", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("AccuLoading", header: "Accumu-lated" + Environment.NewLine + "Loading", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("AccuActCPUPerformed", header: "Accumu-lated Actual" + Environment.NewLine + "CPU Performed", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("AccuCPUVariance", header: "Accumu-lated CPU" + Environment.NewLine + "Variance", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("LeftWorkDays", header: "Left working days", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("AvgWorkhours", header: "Average" + Environment.NewLine + "w/hours", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            .Numeric("PPH", header: "PPH", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            .Numeric("Direct", header: "DIRECT", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("Active", header: "ACTIVE", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("VPH", header: "VPH", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            .Numeric("ManpowerRatio", header: "Manpower" + Environment.NewLine + "Ratio", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            .Numeric("LineNo", header: "No. of" + Environment.NewLine + "Line#", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("LineManpower", header: "Manpower" + Environment.NewLine + "/Line", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 0)
            .Numeric("GPH", header: "GPH", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            .Numeric("SPH", header: "SPH", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 2)
            ;
            #endregion
        }

        private void BtnDownloadTemplate_Click(object sender, EventArgs e)
        {
            string strXltName = Env.Cfg.XltPathDir + "\\Planning_P07_Import.xltx";
            this.excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (this.excel == null)
            {
                return;
            }

            this.excel.Visible = true;
        }

        private void BtnImportExcel_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "Excel files (*.xlsx;*.xls;*.xlt)|*.xlsx;*.xls;*.xlt";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // 清空Grid資料
            if (this.gridData != null)
            {
                this.gridData.Clear();
            }

            try
            {
                this.excel = new Microsoft.Office.Interop.Excel.Application();
                this.excel.Workbooks.Open(this.openFileDialog1.FileName);
                this.excel.Visible = false;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = this.excel.ActiveWorkbook.Worksheets[1];
                int excel_Detail_Count = worksheet.Rows.Count - 16;

                #region 檢查匯入檔案欄位內容
                List<string> listErrorMsg = new List<string>();
                int rowIndex = 16;
                int totalColumns = worksheet.UsedRange.Columns.Count;
                List<NonDecimalValues> nonDecimalValues = new List<NonDecimalValues>();

                for (int i = rowIndex + 1; i <= worksheet.UsedRange.Rows.Count; i++)
                {
                    var detail_Date = (worksheet.Cells[i, 1] as Microsoft.Office.Interop.Excel.Range)?.Value2;
                    var detail_WeekDay = (worksheet.Cells[i, 2] as Microsoft.Office.Interop.Excel.Range)?.Value2;
                    if (detail_Date == null && detail_WeekDay == null)
                    {
                        continue;
                    }

                    for (int j = 3; j <= totalColumns; j++)
                    {
                        var cellValue = (worksheet.Cells[i, j] as Microsoft.Office.Interop.Excel.Range)?.Value2;
                        if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()) && !decimal.TryParse(cellValue.ToString(), out decimal _))
                        {
                            nonDecimalValues.Add(new NonDecimalValues { Row = i, Column = j, Value = cellValue });
                        }
                    }
                }

                if (nonDecimalValues.Any())
                {
                    var uniqueNonDecimalValues = nonDecimalValues.GroupBy(x => x.Column).Select(g => g.First()).ToList();
                    Microsoft.Office.Interop.Excel.Range range_DetailTitle = worksheet.Range["C15:T15"];
                    object[,] objCellArray_Error = range_DetailTitle.Value;

                    string strError = string.Join(Environment.NewLine, uniqueNonDecimalValues.Select(val => "<" + (string)MyUtility.Excel.GetExcelCellValue(objCellArray_Error[1, val.Column], "C") + ">"));

                    if (!MyUtility.Check.Empty(strError))
                    {
                        MyUtility.Msg.WarningBox($"Import failed: {Environment.NewLine + strError + Environment.NewLine} is not valid!");
                        return;
                    }
                }
                #endregion 檢查匯入檔案欄位內容

                #region 抓表頭資料
                Microsoft.Office.Interop.Excel.Range range_head = worksheet.Range["F2:F12"];
                object[,] objCellArray = range_head.Value;
                this.txtLoaded.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "N")), 0).ToString();
                this.txtUnLastMonth.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[2, 1], "N")), 0).ToString();
                this.txtLastMonth.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[3, 1], "N")), 0).ToString();
                this.txtCanceled.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[4, 1], "N")), 0).ToString();
                this.txtToSisterFactory.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[5, 1], "N")), 0).ToString();
                this.txtFromSisterFactory.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[6, 1], "N")), 0).ToString();
                this.txtAddPullFromMonth.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[7, 1], "N")), 0).ToString();
                this.txtDeductLoadingFromMonth.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[8, 1], "N")), 0).ToString();
                this.txtLocalInCpu.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[9, 1], "N")), 0).ToString();
                this.txtLoadOnCpu.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[10, 1], "N")), 0).ToString();
                this.txtRemainCPU.Text = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[11, 1], "N")), 0).ToString();
                #endregion 抓表頭資料

                #region 抓表身資料
                for (int i = 16; i < (worksheet.Rows.Count - 16); i++)
                {
                    Microsoft.Office.Interop.Excel.Range range_Detail = worksheet.Range[$@"A{i}:T{i}"];
                    objCellArray = range_Detail.Value;
                    DataRow newRow = this.gridData.NewRow();

                    string detail_Date = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C").ToString();
                    string detail_WeekDay = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C").ToString();
                    decimal detail_DailyCPULoading = MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "N"));
                    decimal detail_NewTarger = 0;
                    decimal detail_ActCPUPerformed = 0;
                    decimal detail_DailyCPUVarience = 0;
                    decimal detail_AccuLoading = 0;
                    decimal detail_AccuActCPUPerformed = 0;
                    decimal detail_AccuCPUVariance = 0;
                    decimal detail_LeftWorkDays = 0;
                    decimal detail_AvgWorkhours = 0;
                    decimal detail_PPH = 0;
                    decimal detail_Direct = 0;
                    decimal detail_Active = 0;
                    decimal detail_VPH = 0;
                    decimal deatil_ManpowerRatio = 0;
                    decimal detail_LineNo = 0;
                    decimal detail_LineManpower = 0;
                    decimal detail_GPH = 0;
                    decimal detail_SPH = 0;

                    if (!MyUtility.Check.Empty(detail_DailyCPULoading))
                    {
                        detail_NewTarger = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "N")));
                        detail_ActCPUPerformed = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "N")), 3);
                        detail_DailyCPUVarience = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "N")));
                        detail_AccuLoading = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "N")));
                        detail_AccuActCPUPerformed = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 8], "N")));
                        detail_AccuCPUVariance = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "N")));
                        detail_LeftWorkDays = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 10], "N")));

                        if (!MyUtility.Check.Empty(detail_ActCPUPerformed))
                        {
                            detail_AvgWorkhours = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 11], "N")), 2);
                            detail_PPH = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 12], "N")), 2);
                            detail_Direct = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 13], "N")));
                            detail_Active = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 14], "N")));
                            detail_VPH = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 15], "N")), 2);
                            deatil_ManpowerRatio = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 16], "N")), 2);
                            detail_LineNo = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 17], "N")));
                            detail_LineManpower = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 18], "N")));
                            detail_GPH = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 19], "N")), 2);
                            detail_SPH = Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 20], "N")), 2);
                        }
                    }

                    if (MyUtility.Check.Empty(detail_Date) && MyUtility.Check.Empty(detail_WeekDay))
                    {
                        for (int y = 0; y < this.gridBatchImport.Rows.Count; y++)
                        {
                            DataRow dataRows = this.gridBatchImport.GetDataRow(y);

                            for (int x = 0; x < this.gridBatchImport.Columns.Count; x++)
                            {
                                var cellValue = dataRows[x];

                                if ((cellValue is decimal && (decimal)cellValue < 0) || (cellValue is int && (int)cellValue < 0))
                                {
                                    this.gridBatchImport.Rows[y].Cells[x - 1].Style.ForeColor = Color.Red;
                                }
                            }
                        }

                        return;
                    }

                    if (detail_WeekDay.Length > 3)
                    {
                        MyUtility.Msg.WarningBox($@"The [W/day] column in Excel contains data entries that exceed 3 characters.");
                        return;
                    }

                    newRow["Date"] = Convert.ToDateTime(detail_Date).ToString("MM/dd", CultureInfo.CreateSpecificCulture("en-US"));
                    newRow["WeekDay"] = Convert.ToDateTime(detail_Date).ToString("ddd", CultureInfo.CreateSpecificCulture("en-US"));
                    newRow["DailyCPULoading"] = this.Type_Conversion(detail_DailyCPULoading);
                    newRow["NewTarget"] = this.Type_Conversion(detail_NewTarger);
                    newRow["ActCPUPerformed"] = this.Type_Conversion(detail_ActCPUPerformed);
                    newRow["DailyCPUVarience"] = this.Type_Conversion(detail_DailyCPUVarience);
                    newRow["AccuLoading"] = this.Type_Conversion(detail_AccuLoading);
                    newRow["AccuActCPUPerformed"] = this.Type_Conversion(detail_AccuActCPUPerformed);
                    newRow["AccuCPUVariance"] = this.Type_Conversion(detail_AccuCPUVariance);
                    newRow["LeftWorkDays"] = this.Type_Conversion(detail_LeftWorkDays);
                    newRow["AvgWorkhours"] = this.Type_Conversion(detail_AvgWorkhours);
                    newRow["PPH"] = this.Type_Conversion(detail_PPH);
                    newRow["Direct"] = this.Type_Conversion(detail_Direct);
                    newRow["Active"] = this.Type_Conversion(detail_Active);
                    newRow["VPH"] = this.Type_Conversion(detail_VPH);
                    newRow["ManpowerRatio"] = this.Type_Conversion(deatil_ManpowerRatio);
                    newRow["LineNo"] = this.Type_Conversion(detail_LineNo);
                    newRow["LineManpower"] = this.Type_Conversion(detail_LineManpower);
                    newRow["GPH"] = this.Type_Conversion(detail_GPH);
                    newRow["SPH"] = this.Type_Conversion(detail_SPH);
                    this.gridData.Rows.Add(newRow);
                }
                #endregion 抓表身資料

            }
            catch (Exception ex)
            {
                MyUtility.Msg.WarningBox($"An error occurred: {ex.Message}");
            }
            finally
            {
                // 釋放Excel資源
                if (this.excel.Workbooks != null)
                {
                    this.excel.Workbooks.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(this.excel.Workbooks);
                }

                if (this.excel.Workbooks != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(this.excel.Workbooks);
                }

                if (this.excel != null)
                {
                    this.excel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(this.excel);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private object Type_Conversion(object objValue)
        {
            if (MyUtility.Check.Empty(objValue))
            {
                return DBNull.Value;
            }
            else
            {
                return objValue;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            #region 欄位尚未輸入錯誤訊息
            List<string> emptyFields = new List<string>();

            if (MyUtility.Check.Empty(this.txtYear.Text))
            {
                emptyFields.Add("<Year>");
            }

            if (MyUtility.Check.Empty(this.txtMonth.Text))
            {
                emptyFields.Add("<Month>");
            }

            if (MyUtility.Check.Empty(this.txtfactory.Text))
            {
                emptyFields.Add("<Factory>");
            }

            if (emptyFields.Count > 0)
            {
                string errorMsg = string.Join(", ", emptyFields);
                MyUtility.Msg.WarningBox(errorMsg + " can't be empty.");
                return;
            }
            #endregion

            string sqlCmd = string.Empty;

            sqlCmd = $@"
            DECLARE @deleteUkey int;
            DECLARE @InsertUkey int; 
            SELECT @deleteUkey = ukey FROM DailyAccuCPULoading WHERE [Year] = '{this.txtYear.Text}' AND [Month] = '{this.txtMonth.Text}' AND FactoryID = '{this.txtfactory.Text}'

            Delete DailyAccuCPULoading WHERE Ukey = @deleteUkey
            Delete DailyAccuCPULoading_Detail WHERE DailyAccuCPULoadingUkey = @deleteUkey

            INSERT INTO [dbo].[DailyAccuCPULoading]
            ([Year]
            ,[Month]
            ,[FactoryID]
            ,[TTLCPULoaded]
            ,[UnfinishedLastMonth]
            ,[FinishedLastMonth]
            ,[CanceledStillNeedProd]
            ,[SubconToSisFactory]
            ,[SubconFromSisterFactory]
            ,[PullForwardFromNextMonths]
            ,[LoadingDelayFromThisMonth]
            ,[LocalSubconInCPU]
            ,[LocalSubconOutCPU]
            ,[RemainCPUThisMonth]
            ,[AddName]
            ,[AddDate])
             VALUES
            (
            '{this.txtYear.Text}'
            ,'{this.txtMonth.Text}'
            ,'{this.txtfactory.Text}'
            , {(this.txtLoaded.Text.Empty() ? "0" : this.txtLoaded.Text)}
            , {(this.txtUnLastMonth.Text.Empty() ? "0" : this.txtUnLastMonth.Text)}
            , {(this.txtLastMonth.Text.Empty() ? "0" : this.txtLastMonth.Text)}
            , {(this.txtCanceled.Text.Empty() ? "0" : this.txtCanceled.Text)}
            , {(this.txtToSisterFactory.Text.Empty() ? "0" : this.txtToSisterFactory.Text)}
            , {(this.txtFromSisterFactory.Text.Empty() ? "0" : this.txtFromSisterFactory.Text)}
            , {(this.txtAddPullFromMonth.Text.Empty() ? "0" : this.txtAddPullFromMonth.Text)}
            , {(this.txtDeductLoadingFromMonth.Text.Empty() ? "0" : this.txtDeductLoadingFromMonth.Text)}
            , {(this.txtLocalInCpu.Text.Empty() ? "0" : this.txtLocalInCpu.Text)}
            , {(this.txtLoadOnCpu.Text.Empty() ? "0" : this.txtLoadOnCpu.Text)}
            , {(this.txtRemainCPU.Text.Empty() ? "0" : this.txtRemainCPU.Text)}
            ,'{Env.User.UserID}'
            ,GETDATE())

            SET @InsertUkey = SCOPE_IDENTITY();

            INSERT INTO [dbo].[DailyAccuCPULoading_Detail]
            ([DailyAccuCPULoadingUkey]
            ,[Date]
            ,[WeekDay]
            ,[DailyCPULoading]
            ,[NewTarget]
            ,[ActCPUPerformed]
            ,[DailyCPUVarience]
            ,[AccuLoading]
            ,[AccuActCPUPerformed]
            ,[AccuCPUVariance]
            ,[LeftWorkDays]
            ,[AvgWorkhours]
            ,[PPH]
            ,[Direct]
            ,[Active]
            ,[VPH]
            ,[ManpowerRatio]
            ,[LineNo]
            ,[LineManpower]
            ,[GPH]
            ,[SPH])
            SELECT
             @InsertUkey
            ,ISNULL([Date]   , '')
            ,ISNULL([WeekDay], '')
            ,ISNULL([DailyCPULoading]     ,0)
            ,ISNULL([NewTarget]           ,0)
            ,ISNULL([ActCPUPerformed]     ,0)
            ,ISNULL([DailyCPUVarience]    ,0)
            ,ISNULL([AccuLoading]         ,0)
            ,ISNULL([AccuActCPUPerformed] ,0)
            ,ISNULL([AccuCPUVariance]     ,0)
            ,ISNULL([LeftWorkDays]        ,0)
            ,ISNULL([AvgWorkhours]        ,0)
            ,ISNULL([PPH]                 ,0)
            ,ISNULL([Direct]              ,0)
            ,ISNULL([Active]              ,0)
            ,ISNULL([VPH]                 ,0)
            ,ISNULL([ManpowerRatio]       ,0)
            ,ISNULL([LineNo]              ,0)
            ,ISNULL([LineManpower]        ,0)
            ,ISNULL([GPH]                 ,0)
            ,ISNULL([SPH]                 ,0)
            FROM #tmp

            ";

            DualResult upResult;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                if (!(upResult = MyUtility.Tool.ProcessWithDatatable(this.gridData, string.Empty, sqlCmd, out DataTable dt)))
                {
                    transactionscope.Dispose();
                    this.ShowErr(upResult);
                    return;
                }

                transactionscope.Complete();
            }
            this.Close();
        }

        private void TxtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtYear_TextChanged(object sender, EventArgs e)
        {
            if (this.txtYear.Text.Length > 4)
            {
                this.txtYear.Text = this.txtYear.Text.Substring(0, 4);
                this.txtYear.SelectionStart = this.txtYear.Text.Length;
            }
        }

        private void TxtMonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtMonth_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtMonth.Text))
            {
                return;
            }

            if (int.TryParse(this.txtMonth.Text, out int value))
            {
                if (value < 1 || value > 12)
                {
                    MyUtility.Msg.WarningBox("Input invalid month!");
                    this.txtMonth.Text = string.Empty;
                }
            }
            else
            {
                this.txtMonth.Text = string.Empty;
            }
        }
    }

    /// <inheritdoc/>
    public class NonDecimalValues
    {
        /// <inheritdoc/>
        public int Row { get; set; }

        /// <inheritdoc/>
        public int Column { get; set; }

        /// <inheritdoc/>
        public object Value { get; set; }
    }
}

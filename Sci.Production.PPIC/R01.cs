using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.SqlTypes;
using Sci.Production.Prg;
using System.Collections.Generic;
using System.Linq;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.Prg.PowerBI.Logic;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private string mDivision;
        private string factory;
        private string line1;
        private string line2;
        private string brand;
        private string type;
        private string subProcess;
        private DateTime? SewingDate1;
        private DateTime? SewingDate2;
        private DateTime? buyerDelivery1;
        private DateTime? buyerDelivery2;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private DataRow[] drSummary;

        private DataTable dtGantt;
        private DataTable[] dtGanttSumery;
        private DataTable dtHours;

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out DataTable mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FTYGroup from Factory WITH (NOLOCK) ", out DataTable factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboM.Text = Env.User.Keyword;

            // comboBox2.SelectedIndex = 0;
            this.comboFactory.Text = Env.User.Factory;

            this.comboSummaryBy.Add("SP#", "SP#");
            this.comboSummaryBy.Add("Article / Size", "Article / Size");
            this.comboSummaryBy.Add("Style, per each sewing date", "StylePerEachSewingDate");
            this.comboSummaryBy.SelectedIndex = 0;

            DBProxy.Current.Select(null, "select '' as ID union all select ID from ArtworkType WITH (NOLOCK) where ReportDropdown = 1", out DataTable subprocess);
            MyUtility.Tool.SetupCombox(this.comboSubProcess, 1, subprocess);
            this.comboSubProcess.SelectedIndex = 0;
        }

        // Sewing Line
        private void TxtSewingLineStart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLineStart.Text = this.SelectSewingLine(this.txtSewingLineStart.Text);
        }

        // Sewing Line
        private void TxtSewingLineEnd_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLineEnd.Text = this.SelectSewingLine(this.txtSewingLineEnd.Text);
        }

        private string SelectSewingLine(string line)
        {
            string sql = string.Format("Select Distinct ID From SewingLine WITH (NOLOCK) {0}  ", MyUtility.Check.Empty(this.comboFactory.Text) ? string.Empty : string.Format(" where FactoryID = '{0}'", this.comboFactory.Text));
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "3", line, false, ",")
            {
                Width = 300,
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return string.Empty;
            }
            else
            {
                return item.GetSelectedString();
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.line1 = this.txtSewingLineStart.Text;
            this.line2 = this.txtSewingLineEnd.Text;
            this.SewingDate1 = this.dateSewingDate.Value1;
            this.SewingDate2 = this.dateSewingDate.Value2;
            this.buyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.buyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.brand = this.txtbrand.Text;
            this.subProcess = this.comboSubProcess.Text;

            this.type = this.comboSummaryBy.SelectedValue2.ToString();

            if (MyUtility.Check.Empty(this.SewingDate1) && MyUtility.Check.Empty(this.SewingDate2) &&
                MyUtility.Check.Empty(this.buyerDelivery1) && MyUtility.Check.Empty(this.buyerDelivery2) &&
                MyUtility.Check.Empty(this.sciDelivery1) && MyUtility.Check.Empty(this.sciDelivery2))
            {
                MyUtility.Msg.WarningBox("Date can't be all empty!");
                return false;
            }

            if (this.chkGanttChart.Checked &&
                (MyUtility.Check.Empty(this.SewingDate1) || MyUtility.Check.Empty(this.SewingDate2)))
            {
                MyUtility.Msg.WarningBox($@"Please input sewing date first if {"\""}Include Gantt chart{"\""} is checked");
                return false;
            }

            // ISP20240496
            DateTime date2023 = DateTime.Parse("2023/01/01");
            if (!this.SewingDate1.HasValue)
            {
                this.SewingDate1 = date2023;
            }

            if (DateTime.Compare(this.SewingDate1.Value, date2023) == -1)
            {
                this.SewingDate1 = date2023;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result;
            /*
             *  原本的概念是 ArticleSize為細項，SP為細項加總，兩者寫成同一個SQL即可。
             *  無奈發現，細項與SP報表對不起來，只好在區分。
             */
            if (this.type == "SP#")
            {
                result = this.Query_by_SP();
            }
            else if (this.type == "StylePerEachSewingDate")
            {
                result = this.Query_by_StylePerEachSewingDate();
            }
            else
            {
                result = this.Query_by_ArticleSize();
            }

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            List<DataRow> filteredRows;

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            bool result;
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application objApp;
            Excel.Worksheet worksheet;

            if (this.type == "StylePerEachSewingDate")
            {
                objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R01_Style_PerEachSewingDate.xltx"); // 預先開啟excel app

                // #if DEBUG
                //                objApp.Visible = true;
                // #endif

                // 關閉Excel提示訊息
                objApp.DisplayAlerts = false;
                Excel._Workbook mWorkBook = objApp.Workbooks[1];
                worksheet = objApp.Sheets[3];
                worksheet.Select();
                this.printData.Columns.Remove("StyleName");
                this.printData.Columns.Remove("AddDate");
                this.printData.Columns.Remove("EditDate");
                this.printData.Columns.Remove("SewingInlineCategory");

                DataTable printDataFilter = this.printData.Copy();
                if (!this.chkIncludeCompleteSchedule.Checked)
                {
                    if (!MyUtility.Check.Empty(this.SewingDate1))
                    {
                        filteredRows = printDataFilter.AsEnumerable()
                       .Where(x => x.Field<DateTime>("SewingDay") >= DateTime.Parse(this.SewingDate1.Value.Date.ToString("yyyy/MM/dd 00:00:00")))
                       .ToList();

                        if (filteredRows.Count > 0)
                        {
                            printDataFilter = filteredRows.CopyToDataTable();
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("Data not found!");
                            return false;
                        }
                    }

                    if (!MyUtility.Check.Empty(this.SewingDate2))
                    {
                        filteredRows = printDataFilter.AsEnumerable()
                        .Where(x => x.Field<DateTime>("SewingDay") <= DateTime.Parse(this.SewingDate2.Value.Date.ToString("yyyy/MM/dd 00:00:00")))
                        .ToList();

                        if (filteredRows.Count > 0)
                        {
                            printDataFilter = filteredRows.CopyToDataTable();
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("Data not found!");
                            return false;
                        }
                    }
                }

                result = MyUtility.Excel.CopyToXls(printDataFilter, string.Empty, "PPIC_R01_Style_PerEachSewingDate.xltx", 1, false, string.Empty, objApp, false, worksheet, false);

                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    return false;
                }

                #region Sheet 1
                worksheet.Columns[1].ColumnWidth = 8;
                worksheet.Columns[2].ColumnWidth = 8;
                worksheet.Columns[7].ColumnWidth = 8;
                worksheet.Columns[8].ColumnWidth = 8;
                worksheet.Columns[9].ColumnWidth = 8;
                worksheet.Columns[10].ColumnWidth = 8;
                worksheet.Columns[11].ColumnWidth = 8;
                worksheet.Columns[12].ColumnWidth = 8;
                worksheet.Columns[13].ColumnWidth = 10;
                worksheet.Columns[14].ColumnWidth = 10;
                worksheet.Columns[15].ColumnWidth = 10;
                worksheet.Columns[16].ColumnWidth = 10;
                worksheet.Columns[17].ColumnWidth = 8;
                worksheet.Columns[18].ColumnWidth = 8;
                worksheet.Columns[19].ColumnWidth = 8;
                worksheet.Columns[20].ColumnWidth = 8;
                worksheet.Columns[21].ColumnWidth = 8;
                worksheet.Columns[22].ColumnWidth = 8;
                worksheet.Columns[23].ColumnWidth = 8;
                worksheet.Columns[24].ColumnWidth = 8;
                worksheet.Columns[25].ColumnWidth = 8;
                worksheet.Columns[26].ColumnWidth = 8;
                worksheet.Columns[27].ColumnWidth = 8;
                worksheet.Columns[28].ColumnWidth = 8;
                worksheet.Columns[33].ColumnWidth = 8;
                worksheet.Columns[34].ColumnWidth = 8;
                worksheet.Columns[35].ColumnWidth = 8;
                worksheet.Columns[38].ColumnWidth = 8;
                worksheet.Columns[39].ColumnWidth = 8;
                worksheet.Columns[40].ColumnWidth = 8;
                worksheet.Columns[41].ColumnWidth = 8;
                worksheet.Columns[42].ColumnWidth = 8;
                worksheet.Columns[43].ColumnWidth = 8;
                #endregion

                if (this.chkGanttChart.Checked)
                {
                    worksheet = objApp.Sheets[4];
                    worksheet.Select();

                    #region 產生甘特圖

                    // 填內容值
                    int intRowsStart = 1;
                    string writeFty = string.Empty;
                    string line = string.Empty;
                    int ftyCount = 5;
                    int colCount = 2;
                    int rowcnt = 0;
                    int monthDays = ((TimeSpan)(MyUtility.Convert.GetDate(this.SewingDate2) - MyUtility.Convert.GetDate(this.SewingDate1))).Days;
                    string colRight = PublicPrg.Prgs.GetExcelEnglishColumnName(monthDays + 4);

                    foreach (DataRow dr in this.dtGantt.Rows)
                    {
                        // 當有換工廠別時，要換Sheet
                        if (writeFty != MyUtility.Convert.GetString(dr["FactoryID"]))
                        {
                            if (ftyCount > 5)
                            {
                                // 將上一間工廠最後一條Line後面沒有Schedule的格子塗黑
                                if (colCount - 1 != monthDays)
                                {
                                    for (int i = colCount; i <= monthDays; i++)
                                    {
                                        string colttl = PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1);
                                        worksheet.Range[$"{colttl}{intRowsStart}:{colttl}{intRowsStart}"].Cells.Interior.Color = Color.Black;
                                    }
                                }

                                // 從dtGanttSumery  填入Total StdQty,PPH, CPU
                                // 對dtGanttSumery防錯誤用!
                                this.SetGanntTotal(worksheet, this.dtGanttSumery, this.dtHours, intRowsStart, writeFty, line, monthDays + 4);

                                // 畫線 & 設定格式
                                this.GanntCellStyle(worksheet, (rowcnt * 5) + 1, colRight);

                                // 複製Sheet[Gantt Chart]表頭格式
                                objApp.Sheets[mWorkBook.Sheets.Count].Copy(objApp.ActiveWorkbook.Worksheets[4]);

                                // 換Sheet 需要清空變數line,不然會抓到上一筆廠區的lineID
                                line = string.Empty;
                            }

                            worksheet = objApp.ActiveWorkbook.Worksheets[4];
                            worksheet.Select();

                            worksheet.Name = MyUtility.Convert.GetString(dr["FactoryID"]);
                            intRowsStart = 1;
                            rowcnt = 0;

                            // 填入SubTotal
                            this.drSummary = this.dtGanttSumery[1].Select($@" FactoryID = '{dr["FactoryID"]}'");

                            if (this.drSummary.Length > 0)
                            {
                                ((Excel.Range)worksheet.Cells[4, 3]).NumberFormat = "0.00";
                                ((Excel.Range)worksheet.Cells[6, 3]).NumberFormat = "#,##0_);(#,##0)";
                                ((Excel.Range)worksheet.Cells[8, 3]).NumberFormat = "#,##0_);(#,##0)";

                                worksheet.Cells[4, 3] = this.drSummary[0]["TotalPPH"];
                                worksheet.Cells[6, 3] = this.drSummary[0]["TotalStdQ"];
                                worksheet.Cells[8, 3] = this.drSummary[0]["TotalCPU"];
                            }

                            writeFty = MyUtility.Convert.GetString(dr["FactoryID"]);
                            ftyCount++;

                            #region 填入區間天數
                            for (int i = 0; i <= monthDays; i++)
                            {
                                worksheet.Cells[1, i + 3] = ((DateTime)MyUtility.Convert.GetDate(this.SewingDate1)).AddDays(i).ToString("yyyy/MM/dd");
                            }

                            // 加入Total header
                            worksheet.Cells[1, monthDays + 4] = "Total";

                            #endregion
                        }

                        // 換Sewing Line時，要填入Line#
                        if (line != MyUtility.Convert.GetString(dr["SewingLineID"]))
                        {
                            if (intRowsStart > 1)
                            {
                                // 將後面沒有Schedule的格子塗黑
                                if (colCount - 1 != monthDays)
                                {
                                    for (int i = colCount; i <= monthDays; i++)
                                    {
                                        string colttl = PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1);
                                        worksheet.Range[$"{colttl}{intRowsStart}:{colttl}{intRowsStart}"].Cells.Interior.Color = Color.Black;
                                    }
                                }
                            }

                            colCount = 2;
                            intRowsStart = (rowcnt * 5) + 2;

                            // 從dtGanttSumery  填入Total StdQty,PPH, CPU
                            this.SetGanntTotal(worksheet, this.dtGanttSumery, this.dtHours, intRowsStart - 5, dr["FactoryID"].ToString(), line, monthDays + 4);

                            // 插入四行
                            Excel.Range rngToInsert;
                            for (int i = 0; i < 5; i++)
                            {
                                rngToInsert = worksheet.get_Range(string.Format("B{0}:B{0}", MyUtility.Convert.GetString(intRowsStart + i)), Type.Missing).EntireRow;
                                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                                Marshal.ReleaseComObject(rngToInsert);
                            }

                            line = MyUtility.Convert.GetString(dr["SewingLineID"]);
                            for (int r = 0; r < 5; r++)
                            {
                                worksheet.Cells[intRowsStart + r, 1] = line;
                            }

                            Excel.Range rangeStart = worksheet.Cells[intRowsStart, 3];
                            Excel.Range rangeEnd = worksheet.Cells[intRowsStart + 4, monthDays + 3];
                            rngToInsert = (Excel.Range)worksheet.get_Range(rangeStart, rangeEnd);
                            rngToInsert.Cells.Interior.Color = Color.Black;

                            worksheet.Cells[intRowsStart, 2] = "Detail";
                            worksheet.Cells[intRowsStart + 1, 2] = "Std Q";
                            worksheet.Cells[intRowsStart + 2, 2] = "PPH";
                            worksheet.Cells[intRowsStart + 3, 2] = "CPU";
                            worksheet.Cells[intRowsStart + 4, 2] = "Working Hours";

                            rowcnt++;
                        }

                        // 算上下線日的總天數，用來做合併儲存格
                        DateTime sewingStartDate = Convert.ToDateTime(dr["Inline"]);
                        DateTime sewingEndDate = Convert.ToDateTime(dr["Offline"]);
                        int startCol = sewingStartDate.Subtract((DateTime)this.dateSewingDate.Value1).Days + 3;
                        int endCol = sewingEndDate.Subtract((DateTime)this.dateSewingDate.Value1).Days + 3;
                        int totalDays = sewingEndDate.Subtract(sewingStartDate).Days + 1;

                        // 將中間沒有Schedule的格子塗黑
                        if (colCount + 1 != startCol)
                        {
                            for (int i = colCount + 1; i < startCol; i++)
                            {
                                worksheet.Range[$"{PublicPrg.Prgs.GetExcelEnglishColumnName(i)}{intRowsStart}:{PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1)}{intRowsStart}"].Cells.Interior.Color = Color.Black;
                            }
                        }

                        // 算出Excel的Column的英文位置
                        string excelStartColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(startCol);
                        string excelEndColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(endCol);
                        Excel.Range selrng = worksheet.get_Range($"{excelStartColEng}{intRowsStart}:{excelEndColEng}{intRowsStart}", Type.Missing).EntireRow;

                        // 設置儲存格的背景色
                        Excel.Range rngColor = worksheet.Range[$"{excelStartColEng}{intRowsStart}:{excelEndColEng}{intRowsStart + 4}"];

                        // 合併儲存格,文字置中 (僅限Style)
                        worksheet.Range[$"{excelStartColEng}{intRowsStart}:{excelEndColEng}{intRowsStart}"].Merge(Type.Missing);
                        worksheet.Range[$"{excelStartColEng}{intRowsStart}:{excelEndColEng}{intRowsStart}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        // 顏色顯示優先權：Holiday紅色背景 > SCI Delivery為當月以前的紫色粗體 > SCI Delivery當月為以後的綠色粗體 > Bulk款式藍色粗體 > SMS款式紅色粗體 > 非以上四種情況的黑色字體
                        #region 填入內容值
                        if (MyUtility.Convert.GetString(dr["StyleID"]) == "Holiday")
                        {
                            rngColor.Cells.Interior.Color = Color.Red;
                            colCount = colCount + (startCol - colCount - 1) + totalDays;
                            Marshal.ReleaseComObject(selrng);
                            continue;
                        }
                        else if (MyUtility.Convert.GetString(dr["IsLastMonth"]).ToUpper() == "TRUE")
                        {
                            rngColor.Cells.Font.Color = Color.Purple;
                        }
                        else if (MyUtility.Convert.GetString(dr["IsNextMonth"]).ToUpper() == "TRUE")
                        {
                            rngColor.Cells.Font.Color = Color.Green;
                        }
                        else if (MyUtility.Convert.GetString(dr["IsBulk"]).ToUpper() == "TRUE")
                        {
                            rngColor.Cells.Font.Color = Color.Blue;
                        }
                        else if (MyUtility.Convert.GetString(dr["IsSMS"]).ToUpper() == "TRUE")
                        {
                            rngColor.Cells.Font.Color = Color.Red;
                        }
                        else
                        {
                            rngColor.Cells.Font.Bold = false;
                            rngColor.Cells.Font.Color = Color.Black;
                        }

                        worksheet.Cells[intRowsStart, startCol] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Check.Empty(dr["MinBuyerDelivery"]) ? string.Empty : " " + Convert.ToDateTime(dr["MinBuyerDelivery"]).ToString("yyyy/MM/dd"));

                        // 從Detail by Style,by line,by Sewing Date 填入StdQty,PPH, CPU
                        int dateRange = ((TimeSpan)(MyUtility.Convert.GetDate(dr["OffLine"]) - MyUtility.Convert.GetDate(dr["InLine"]))).Days;
                        for (int d = 0; d <= dateRange; d++)
                        {
                            this.SetGannt(worksheet, this.dtGanttSumery, this.dtHours, intRowsStart, dr, startCol, rngColor, d);
                        }

                        if (MyUtility.Convert.GetString(dr["StyleID"]) != "Holiday")
                        {
                            if (MyUtility.Convert.GetString(dr["IsSample"]).ToUpper() == "TRUE")
                            {
                                // 設置儲存格的背景色
                                rngColor.Cells.Interior.Color = Color.Yellow;
                            }
                            else if (MyUtility.Convert.GetString(dr["IsCrossStyle"]).ToUpper() == "TRUE")
                            {
                                if (MyUtility.Convert.GetString(dr["IsSimilarStyle"]).ToUpper() == "TRUE")
                                {
                                    // 設置儲存格的背景色
                                    rngColor.Cells.Interior.Color = Color.LightGray;
                                }
                                else
                                {
                                    // 設置儲存格的背景色
                                    rngColor.Cells.Interior.Color = Color.White;
                                }
                            }
                            else if (MyUtility.Convert.GetString(dr["IsRepeatStyle"]).ToUpper() == "TRUE")
                            {
                                // 設置儲存格的背景色
                                rngColor.Cells.Interior.Color = Color.FromArgb(175, 203, 154);
                            }
                            else if (MyUtility.Convert.GetString(dr["IsFirst"]).ToUpper() == "TRUE")
                            {
                                // 設置儲存格的背景色
                                rngColor.Cells.Interior.Color = Color.White;
                            }
                            else
                            {
                                // 設置儲存格的背景色
                                rngColor.Cells.Interior.Color = Color.FromArgb(134, 187, 249);
                            }
                        }
                        #endregion
                        colCount = colCount + (startCol - colCount - 1) + totalDays;
                        Marshal.ReleaseComObject(selrng);
                    }

                    // 從dtGanttSumery  填入Total StdQty,PPH, CPU
                    this.SetGanntTotal(worksheet, this.dtGanttSumery, this.dtHours, (rowcnt * 5) + 2 - 5, worksheet.Name, line, monthDays + 4);

                    if (colCount - 1 != monthDays)
                    {
                        for (int i = colCount; i <= monthDays; i++)
                        {
                            string colttl = PublicPrg.Prgs.GetExcelEnglishColumnName(i + 1);
                            worksheet.Range[$"{colttl}{intRowsStart}:{colttl}{intRowsStart}"].Cells.Interior.Color = Color.Black;
                        }
                    }

                    // 畫線 & 設定格式
                    this.GanntCellStyle(worksheet, (rowcnt * 5) + 1, colRight);

                    // 調整欄寬
                    for (int c = 1; c < monthDays + 4; c++)
                    {
                        worksheet.Columns[c].ColumnWidth = 10;
                    }

                    #endregion
                }
                else
                {
                    // 刪除Sheet[Gantt Chart]
                    worksheet = mWorkBook.Sheets[4];
                    worksheet.Delete();
                }

                // 刪除最後一個Sheet[Gantt Chart tmp]
                worksheet = mWorkBook.Sheets[mWorkBook.Sheets.Count];
                worksheet.Delete();

                // 移動Sheet: Detail]
                worksheet = mWorkBook.Sheets[3];
                // 移除Sheet: [Detail] CD Code欄位
                worksheet.get_Range("U:U").EntireColumn.Delete();

                // 移除Sheet: [Detail] Production Family ID Code欄位
                worksheet.get_Range("AA:AA").EntireColumn.Delete();

                worksheet.Move(After: mWorkBook.Sheets[mWorkBook.Sheets.Count]);

                // 移動Sheet: [Columns Description]
                worksheet = mWorkBook.Sheets[1];
                worksheet.Move(After: mWorkBook.Sheets[mWorkBook.Sheets.Count]);

                // 移動sheet: [Gantt chart definition]
                worksheet = mWorkBook.Sheets[1];
                worksheet.Move(After: mWorkBook.Sheets[mWorkBook.Sheets.Count]);

                worksheet = objApp.Sheets[1];
                worksheet.Select();

                #region Save & Show Excel

                string strExcelName = Class.MicrosoftFile.GetName("PPIC_R01_Style_PerEachSewingDate");
                Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }
            else
            {
                #region SP# & Article / Size

                if (this.checkForPrintOut.Checked == true)
                {
                    #region PPIC_R01_PrintOut
                    this.printData.Columns.Remove("MDivisionID");
                    this.printData.Columns.Remove("PFRemark");
                    this.printData.Columns.Remove("BuyerDelivery");
                    this.printData.Columns.Remove("VasShas");
                    this.printData.Columns.Remove("ShipModeList");
                    this.printData.Columns.Remove("Alias");
                    this.printData.Columns.Remove("CRDDate");
                    this.printData.Columns.Remove("CustPONo");
                    if (this.type == "SP#")
                    {
                        this.printData.Columns.Remove("TTL_PRINTING_PCS");
                        this.printData.Columns.Remove("TTL_PRINTING_PPU_PPU");
                        this.printData.Columns.Remove("SubCon");
                    }

                    objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R01_PrintOut.xltx"); // 預先開啟excel app
                    worksheet = objApp.Sheets[2];

                    // 貼資料前先刪欄位
                    if (this.type == "SP#")
                    {
                    }
                    else
                    {
                        ExcelPrg.ExcelDeleteColumn(worksheet, 4, "Sewing CPU");
                    }

                    result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "PPIC_R01_PrintOut.xltx", headerRow: 4, showExcel: false, excelApp: objApp, wSheet: objApp.Sheets[2]);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                        return false;
                    }

                    // Summary By = SP# 則刪除欄位Size
                    ExcelPrg.ExcelDeleteColumn(worksheet, 4, "CD");
                    if (this.type == "SP#")
                    {
                        ExcelPrg.ExcelDeleteColumn(worksheet, 4, "Size");
                    }
                    else
                    {
                    }

                    #region Set Excel Title
                    string factoryName = MyUtility.GetValue.Lookup(
                        string.Format(
                            @"
select NameEn 
from Factory 
where id = '{0}'", Env.User.Factory), null);
                    worksheet.Cells[1, 1] = factoryName;
                    worksheet.Cells[2, 1] = "Sewing Line Schedule Report";
                    worksheet.Cells[3, 1] = "Date:" + DateTime.Now.ToString("yyyy/MM/dd");
                    #endregion
                    for (int i = 1; i < this.printData.Rows.Count; i++)
                    {
                        DataRow frontRow = this.printData.Rows[i - 1];
                        DataRow row = this.printData.Rows[i];

                        // 當前後 SyleID 不同時，中間加上虛線
                        if (!frontRow["StyleID"].EqualString(row["StyleID"]))
                        {
                            // [2] = header 所佔的行數 + Excel 從 1 開始編號 = 1 + 1
                            Excel.Range excelRange = worksheet.get_Range("A" + (i + 5) + ":AB" + (i + 5));
                            excelRange.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlDash;
                        }
                    }

                    worksheet.Activate();

                    #region Save & Show Excel
                    string strExcelName = Class.MicrosoftFile.GetName("PPIC_R01_PrintOut");
                    Excel.Workbook workbook = objApp.ActiveWorkbook;
                    workbook.SaveAs(strExcelName);
                    workbook.Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(worksheet);
                    Marshal.ReleaseComObject(workbook);

                    strExcelName.OpenFile();
                    #endregion
                    #endregion
                }
                else
                {
                    #region PPIC_R01_SewingLineScheduleReport
                    objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R01_SewingLineScheduleReport.xltx"); // 預先開啟excel app
                    worksheet = objApp.Sheets[2];
                    worksheet.Activate();

                    // 貼資料前先刪欄位
                    if (this.type == "SP#")
                    {
                    }
                    else
                    {
                        ExcelPrg.ExcelDeleteColumn(worksheet, 1, "Sewing CPU");
                        ExcelPrg.ExcelDeleteColumn(worksheet, 1, "Sew. MTL ETA (SP)");
                    }

                    result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "PPIC_R01_SewingLineScheduleReport.xltx", headerRow: 1, showExcel: false, excelApp: objApp, wSheet: objApp.Sheets[2]);

                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                        return false;
                    }

                    // Summary By = SP# 則刪除欄位Size
                    ExcelPrg.ExcelDeleteColumn(worksheet, 1, "CD");
                    if (this.type == "SP#")
                    {
                        ExcelPrg.ExcelDeleteColumn(worksheet, 1, "Size");
                    }
                    else
                    {
                        ExcelPrg.ExcelDeleteColumn(worksheet, 1, "SubCon");
                    }

                    #region Save & Show Excel
                    string strExcelName = Class.MicrosoftFile.GetName("PPIC_R01_SewingLineScheduleReport");
                    Excel.Workbook workbook = objApp.ActiveWorkbook;
                    workbook.SaveAs(strExcelName);
                    workbook.Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(worksheet);
                    Marshal.ReleaseComObject(workbook);

                    strExcelName.OpenFile();
                    #endregion

                    #endregion
                }
                #endregion
            }

            return true;
        }

        private void GanntCellStyle(Excel.Worksheet worksheet, int row, string column)
        {
            worksheet.Range[$"A1:{column}{row}"].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            worksheet.Range[$"A1:{column}{row}"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            worksheet.Range[$"A1:{column}{row}"].Font.Bold = true;
            worksheet.Range[$"A2:{column}{row}"].Font.Size = 10;
            worksheet.Range[$"A1:{column}{row}"].Font.Name = "Calibri";

            // 相同line資訊用粗線框起來
            for (int i = 2; i <= row - 1; i++)
            {
                if ((i - 2) % 5 == 0)
                {
                    worksheet.Range[$"A{i}:{column}{i + 4}"].BorderAround2(LineStyle: 1, Weight: Excel.XlBorderWeight.xlMedium);
                }
            }
        }

        private void SetGannt(Excel.Worksheet worksheet, DataTable[] dtGanttSumery, DataTable drHours, int intRowsStart, DataRow dr, int startCol, Excel.Range rngColor, int d)
        {
            string sewingDate = ((DateTime)dr["InLine"]).AddDays(d).ToString("yyyy-MM-dd");
            this.drSummary = dtGanttSumery[0].Select($@" SewingDay = '{sewingDate}' and SewingLineID = '{dr["SewingLineID"]}' and FactoryID = '{dr["FactoryID"]}'");
            this.SetGanntValue(worksheet, drHours, intRowsStart, dr["FactoryID"].ToString(), dr["SewingLineID"].ToString(), startCol + d, sewingDate);
            if (this.drSummary.Length > 0)
            {
                rngColor.Cells.Interior.Color = Color.White;
            }
        }

        private void SetGanntTotal(Excel.Worksheet worksheet, DataTable[] dtGanttSumery, DataTable drHours, int intRowsStart, string factoryID, string line, int column)
        {
            if (!line.Empty())
            {
                DataRow[] drCheck = dtGanttSumery[0].Select($"sewinglineid = '{line}' and FactoryID = '{factoryID}'");
                if (drCheck.Length > 0)
                {
                    string strMaxDate = ((DateTime)dtGanttSumery[0].Compute("max([SewingDay])", $"sewinglineid = '{line}' and FactoryID = '{factoryID}'")).ToString("yyyy/MM/dd");
                    this.drSummary = dtGanttSumery[0].Select($@" SewingDay = '{strMaxDate}' and SewingLineID = '{line}' and FactoryID = '{factoryID}'");
                    this.SetGanntValue(worksheet, drHours, intRowsStart, factoryID, line, column, string.Empty);
                }
            }
        }

        private void SetGanntValue(Excel.Worksheet worksheet, DataTable drHours, int intRowsStart, string factoryID, string line, int column, string sewingDay)
        {
            if (this.drSummary.Length > 0)
            {
                worksheet.Cells[intRowsStart + 1, column] = this.drSummary[0]["Total_StdOutput"];
                ((Excel.Range)worksheet.Cells[intRowsStart + 1, column]).NumberFormat = "#,##0_);(#,##0)";
                worksheet.Cells[intRowsStart + 2, column] = this.drSummary[0]["PPH"];
                ((Excel.Range)worksheet.Cells[intRowsStart + 2, column]).NumberFormat = "0.00";
                worksheet.Cells[intRowsStart + 3, column] = this.drSummary[0]["CPU"];
                ((Excel.Range)worksheet.Cells[intRowsStart + 3, column]).NumberFormat = "#,##0_);(#,##0)";
                worksheet.Cells[intRowsStart + 4, column] = this.FilterHour(drHours, sewingDay, line, factoryID);
                ((Excel.Range)worksheet.Cells[intRowsStart + 4, column]).NumberFormat = "0.0";
            }
        }

        private void ComboFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtSewingLineStart.Text = string.Empty;
            this.txtSewingLineEnd.Text = string.Empty;
        }

        private void TxtSewingLineStart_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtSewingLineStart.Text == this.txtSewingLineStart.OldValue)
            {
                return;
            }

            if (!MyUtility.Check.Empty(this.txtSewingLineStart.Text))
            {
                string sql = string.Format("Select ID From SewingLine WITH (NOLOCK) where id='{0}' {1} ", this.txtSewingLineStart.Text, MyUtility.Check.Empty(this.comboFactory.Text) ? string.Empty : string.Format(" and FactoryID = '{0}'", this.comboFactory.Text));
                if (!MyUtility.Check.Seek(sql))
                {
                    this.txtSewingLineStart.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("< Sewing Line: {0} > not found!!!", this.txtSewingLineStart.Text));
                    return;
                }
            }
        }

        private void TxtSewingLineEnd_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtSewingLineEnd.Text == this.txtSewingLineEnd.OldValue)
            {
                return;
            }

            if (!MyUtility.Check.Empty(this.txtSewingLineEnd.Text))
            {
                string sql = string.Format("Select ID From SewingLine WITH (NOLOCK) where id='{0}' {1} ", this.txtSewingLineEnd.Text, MyUtility.Check.Empty(this.comboFactory.Text) ? string.Empty : string.Format(" and FactoryID = '{0}'", this.comboFactory.Text));
                if (!MyUtility.Check.Seek(sql))
                {
                    this.txtSewingLineEnd.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("< Sewing Line: {0} > not found!!!", this.txtSewingLineEnd.Text));
                    return;
                }
            }
        }

        private DualResult Query_by_SP()
        {
            PPIC_R01bySP_ViewModel model = new PPIC_R01bySP_ViewModel()
            {
                MDivisionID = this.mDivision,
                FactoryID = this.factory,
                SewingLineIDFrom = this.line1,
                SewingLineIDTo = this.line2,
                SewingDateFrom = this.SewingDate1,
                SewingDateTo = this.SewingDate2,
                BuyerDeliveryFrom = this.buyerDelivery1,
                BuyerDeliveryTo = this.buyerDelivery2,
                SciDeliveryFrom = this.sciDelivery1,
                SciDeliveryTo = this.sciDelivery2,
                BrandID = this.brand,
                SubProcess = this.subProcess,
                IsPowerBI = false,
            };

            PPIC_R01 biModel = new PPIC_R01();
            Base_ViewModel resultReport = biModel.GetSewingLineScheduleDataBySP(model);
            this.printData = resultReport.Dt;
            if (resultReport.Result)
            {
                this.printData.Columns.Remove("ID");
            }

            return resultReport.Result;
        }

        private DualResult Query_by_ArticleSize()
        {
            DualResult result;
            StringBuilder sqlCmd = new StringBuilder();

            #region Main
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                           Main                              */
-----------------------------------------------------------------
select  s.SewingLineID
            , s.MDivisionID
            , s.FactoryID
            , s.OrderID
			, o.CustPONo
            , s.ComboType 
            , [Switch to Workorder] = Iif(cutting.WorkType='1','Combination',Iif(cutting.WorkType='2','By SP#',''))
            , sd.Article
            , o.SeasonID
			, sd.SizeCode
            , o.CdCodeID
            , o.StyleID 
            , InspDate = InspctDate.Val
            , s.StandardOutput 
            , [Eff] = case when (s.sewer * s.workhour) = 0 then 0
                      ELSE ROUND(CONVERT(float ,(s.AlloQty * s.TotalSewingTime) / (s.sewer * s.workhour * 3600)) * 100,2)
                      END
            , o.KPILETA  
            , o.MTLETA
            , o.MTLExport
            , s.Inline
            , s.Offline
            , o.SciDelivery
            , o.BuyerDelivery 
			, o.CRDDate
            , IIF(o.VasShas=1, 'Y', '') as VasShas
            , o.ShipModeList,isnull(c.Alias, '') as Alias 
            , isnull((  select CONCAT(Remark, ', ') 
                        from (  select s1.SewingLineID+'('+s1.ComboType+'):'+CONVERT(varchar,s1.AlloQty) as Remark 
                                from SewingSchedule s1 WITH (NOLOCK) 
                                where   s1.OrderID = s.OrderID 
                                        and s1.ID != s.ID
                        ) a for xml path('')
                    ), '') as Remark
			, o.StyleUkey
			,o.CPU
			,o.CPUFactor
			,s.ID
            ,o.FtyGroup
			,[FirststCuttingOutputDate] = FirststCuttingOutputDate.Date
	        ,[CDCodeNew] = sty.CDCodeNew
	        ,[ProductType] = sty.ProductType
            ,[MatchFabric] = iif(o.IsNotRepeatOrMapping = 0 ,'Y','N')
	        ,[FabricType] = sty.FabricType
	        ,[Lining] = sty.Lining
	        ,[Gender] = sty.Gender
	        ,[Construction] = sty.Construction
            ,o.Category
			,SizeCodeSeq = oz.Seq
	into #tmp_main
    from SewingSchedule s WITH (NOLOCK) 
	inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID
    inner join Style st with (nolock) on st.Ukey = o.StyleUkey
	inner join SewingSchedule_Detail sd WITH (NOLOCK) on s.ID=sd.ID 
	left join Order_SizeCode oz  WITH (NOLOCK) on oz.ID =  o.POID AND oz.SizeCode=sd.SizeCode
    left join Country c WITH (NOLOCK) on o.Dest = c.ID 
    left join cutting on cutting.ID =o.CuttingSP 
	OUTER APPLY(	
		SELECT [Date]=MIN(co2.cDate)
		FROM  WorkOrder_Distribute wd2 WITH (NOLOCK)
		INNER JOIN CuttingOutput_Detail cod2 WITH (NOLOCK) on cod2.WorkOrderUkey = wd2.WorkOrderUkey
		INNER JOIN CuttingOutput co2 WITH (NOLOCK) on co2.id = cod2.id and co2.Status <> 'New'
		where wd2.OrderID =o.ID
	)FirststCuttingOutputDate
	OUTER APPLY(

		SELECT [Val]=STUFF((
		select  DISTINCT ','+ Cast(CFAFinalInspectDate as varchar)
		from Order_QtyShip oq
		WHERE ID = o.id
		FOR XML PATH('')
		),1,1,'')
	)InspctDate
    Outer apply (
	    SELECT s.[ID]
		    , ProductType = r2.Name
		    , FabricType = r1.Name
		    , Lining
		    , Gender
		    , Construction = d1.Name
            , s.CDCodeNew
	    FROM Style s WITH(NOLOCK)
	    left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	    left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	    left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	    where s.Ukey = o.StyleUkey
    )sty
    where 1 = 1 
");
            #endregion

            #region where條件
            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.line1))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID >= '{0}'", this.line1));
            }

            if (!MyUtility.Check.Empty(this.line2))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID <= '{0}'", this.line2));
            }

            if (!MyUtility.Check.Empty(this.SewingDate1))
            {
                sqlCmd.Append(string.Format(" and (convert(date,s.Inline) >= '{0}' or ('{0}' between convert(date,s.Inline) and convert(date,s.Offline)))", Convert.ToDateTime(this.SewingDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.SewingDate2))
            {
                sqlCmd.Append(string.Format(" and (convert(date,s.Offline) <= '{0}' or ('{0}' between convert(date,s.Inline) and convert(date,s.Offline)))", Convert.ToDateTime(this.SewingDate2).AddDays(1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDelivery1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDelivery2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDelivery1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.subProcess))
            {
                sqlCmd.Append(string.Format(
                    @"
and exists(select 1 from Style_TmsCost st where o.StyleUkey = st.StyleUkey and st.ArtworkTypeID = '{0}' AND (st.Qty>0 or st.TMS>0 and st.Price>0) )", this.subProcess));
            }
            #endregion

            #region TempTable
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                          TempTable                          */
-----------------------------------------------------------------

Select  w.FactoryID, w.SewingLineID, t.Inline, t.Offline
		,isnull(sum(w.Hours),0) as Hours
        , Count(w.Date) as ctn 
into #tmp_WorkHour
from WorkHour w WITH (NOLOCK) 
inner join (select distinct FtyGroup,SewingLineID,Convert(Date,Inline) Inline,Convert(Date,Offline)Offline from #tmp_main) t 
	on w.FactoryID = t.FtyGroup  and w.SewingLineID =t.SewingLineID and w.Date between Inline and Offline
where w.Hours > 0 
group by w.FactoryID, w.SewingLineID, t.Inline, t.Offline
  
select id, Remark as PFRemark
into #tmp_PFRemark
from
(
	 Select s.Id, s.Remark, s.AddDate, ROW_NUMBER() over(PARTITION BY s.Id order by s.AddDate desc) r_id
     from Order_PFHis s WITH (NOLOCK) 
	 inner join #tmp_main t on s.Id = t.OrderID
) a
where a.r_id = 1

select
wd.OrderID,
wd.Article,
wd.SizeCode,
[CutInLine] = MIN(w.EstCutDate)
into #tmp_CutInLine
from WorkOrder_Distribute wd with (nolock)
inner join WorkOrder w with (nolock) on wd.WorkOrderUkey = w.Ukey
where exists (select 1 from #tmp_main tw where  wd.OrderID = tw.OrderID and 
                                                    wd.Article = tw.Article and 
                                                    wd.SizeCode = tw.SizeCode)
group by wd.OrderID,
         wd.Article,
         wd.SizeCode

");
            #endregion

            #region 原CTE
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                           原CTE                             */
-----------------------------------------------------------------
select  ot.ID
        , at.Abbreviation
        , ot.Qty
        , ot.TMS
        , at.Classify
into #tmpAllArtwork
from Order_TmsCost ot WITH (NOLOCK) 
        , ArtworkType at WITH (NOLOCK) 
where   ot.ArtworkTypeID = at.ID
        and (ot.Price > 0 or at.Classify in ('O','I') )
        and (at.Classify in ('S','I') or at.IsSubprocess = 1)
        and (ot.TMS > 0 or ot.Qty > 0)
        and at.Abbreviation !=''
		and ot.ID in (select OrderID from #tmp_main) 

select * 
into #tmpArtWork
from (
    select  ID
            , Abbreviation+':'+Convert(varchar,Qty) as Artwork 
    from #tmpAllArtwork 
    where Qty > 0
        
    union all
    select  ID
            , Abbreviation+':'+Convert(varchar,TMS) as Artwork 
    from #tmpAllArtwork 
    where TMS > 0 and Classify in ('O','I')
) a 

select tmpArtWorkID.ID
        , Artwork = (select   CONCAT(Artwork,', ') 
						from #tmpArtWork 
						where ID = tmpArtWorkID.ID 
						order by Artwork for xml path(''))  
into #tmpOrderArtwork
from (
	select distinct ID
	from #tmpArtWork
) tmpArtWorkID

drop table #tmpAllArtwork,#tmpArtWork

");
            #endregion

            #region QTY
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                           QTY                               */
-----------------------------------------------------------------
select oq.ID as OrderID, oq.Article, oq.SizeCode, sum(qty) qty 
into #tmp_Qty
from Order_Qty oq
inner join (select distinct OrderID,Article,SizeCode from #tmp_main) t on oq.ID=t.OrderID and oq.Article=t.Article and oq.SizeCode=t.SizeCode 
group by oq.ID, oq.Article, oq.SizeCode

select s2.ID,s2.ComboType,s2.Article,s2.SizeCode,sum(AlloQty) AlloQty 
into #tmp_AlloQty
from SewingSchedule_Detail s2
inner join (select distinct ID,ComboType,Article,SizeCode from #tmp_main) t on s2.ID=t.ID and s2.ComboType=t.ComboType and s2.Article=t.Article and s2.SizeCode=t.SizeCode 
group by s2.ID,s2.ComboType,s2.Article,s2.SizeCode


select distinct cow.OrderID, cow.Article, cow.Size, cow.qty as CutQty
into #tmp_CutQty
from CuttingOutput_WIP cow
inner join (select distinct OrderID,Article,SizeCode from #tmp_main) t on cow.OrderID=t.OrderID and cow.Article=t.Article and cow.Size=t.SizeCode 

select sdd.OrderId, sdd.ComboType, sdd.Article, sdd.SizeCode, so.SewingLineID, sum(sdd.QAQty) SewingQty 
into #tmp_SewingQty
from SewingOutput so WITH (NOLOCK) 
inner join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on so.ID = sdd.ID
inner join (select distinct OrderID,ComboType,Article,SizeCode,SewingLineID from #tmp_main) t on sdd.OrderId=t.OrderID and sdd.ComboType=t.ComboType and sdd.Article=t.Article and sdd.SizeCode=t.SizeCode and so.SewingLineID = t.SewingLineID 
group by sdd.OrderId, sdd.ComboType, sdd.Article, sdd.SizeCode, so.SewingLineID


select pkd.OrderId,pkd.Article,pkd.SizeCode,sum(ShipQty) ClogQty 
into #tmp_ClogQty
from PackingList_Detail pkd
inner join (select distinct OrderID,Article,SizeCode from #tmp_main) t on pkd.OrderId=t.OrderID and pkd.Article=t.Article and pkd.SizeCode=t.SizeCode 
where pkd.ReceiveDate is not null
group by pkd.OrderId,pkd.Article,pkd.SizeCode

");

            #endregion

            #region Final
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                           Final                            */
-----------------------------------------------------------------


select  SewingLineID
        , MDivisionID
        , FactoryID
        , OrderID
		, CustPONo
        , Category
        , ComboType
        , [Switch to Workorder]
        , Article 
        , SeasonID
        , SizeCode
        , CdCodeID
	    , CDCodeNew
	    , ProductType
        , MatchFabric
	    , FabricType
	    , Lining
	    , Gender
	    , Construction
        , StyleID
        , Qty
        , AlloQty
        , CutQty
        , SewingQty
        , ClogQty
		, FirststCuttingOutputDate
        , InspDate
        , StandardOutput * WorkHour as TotalStandardOutput
        , WorkHour
        , StandardOutput
        , Eff
        , KPILETA
        , PFRemark
        , MTLETA
        , MTLExport
        , CutInLine
        , Inline
        , Offline
        , SciDelivery
        , BuyerDelivery
		, CRDDate
        , t.CPU * t.CPUFactor * ( isnull(isnull(t.ol_rate,t.sl_rate), 100) / 100) as CPU
        , VasShas
        , ShipModeList
        , Alias
        , ArtWork
        , IIF(Remark = '','',SUBSTRING(Remark,1,LEN(Remark)-1)) as Remark 
from 
(
	select t.*
		,dbo.GetOrderLocation_Rate(t.OrderID, t.ComboType) ol_rate
		,dbo.GetStyleLocation_Rate(t.StyleUkey,t.ComboType) sl_rate 
		,isnull(pf.PFRemark,'') PFRemark
		,IIF(w.ctn = 0, 0,w.Hours/w.ctn) WorkHour
		, isnull(SUBSTRING(ta.Artwork, 1, LEN(ta.Artwork) - 1), '') as ArtWork
		, [Qty] = qty.qty
		, [AlloQty] = ISNULL( alloQty.alloQty,0)
		, [CutQty] = ISNULL( cutQty.cutQty,0)
		, [SewingQty] = ISNULL( sewingQty.sewingQty,0)
		, [ClogQty] = ISNULL( clogQty.clogQty,0)
        , tc.CutInLine
	from #tmp_main t
	left join #tmp_PFRemark pf on t.OrderID = pf.Id
	left join #tmp_WorkHour w on w.FactoryID = t.FtyGroup  and w.SewingLineID =t.SewingLineID and w.Inline = Convert(Date,t.Inline) and w.Offline = Convert(Date,t.Offline) 
	left join #tmpOrderArtwork ta on ta.ID = t.OrderID
	left join #tmp_Qty qty on qty.OrderID = t.OrderID and qty.Article = t.Article and qty.SizeCode = t.SizeCode
	left join #tmp_AlloQty alloQty on alloQty.ID = t.ID and alloQty.Article = t.Article and alloQty.SizeCode = t.SizeCode and alloQty.ComboType = t.ComboType
	left join #tmp_CutQty cutQty on cutQty.OrderID = t.OrderID and cutQty.Article =t.Article and cutQty.Size = t.SizeCode
	left join #tmp_SewingQty sewingQty on sewingQty.OrderId = t.OrderID and sewingQty.Article = t.Article and sewingQty.SizeCode = t.SizeCode and sewingQty.ComboType =t.ComboType and sewingQty.SewingLineID =t.SewingLineID
	left join #tmp_ClogQty clogQty on clogQty.OrderID = t.OrderID and clogQty.Article = t.Article and clogQty.SizeCode = t.SizeCode
    left join #tmp_CutInLine tc on tc.OrderID = t.OrderID and tc.Article = t.Article and tc.SizeCode = t.SizeCode
) t
order by SewingLineID,MDivisionID,FactoryID,Inline,StyleID asc,SizeCodeSeq desc

drop table #tmp_main,#tmp_PFRemark,#tmp_WorkHour,#tmpOrderArtwork,#tmp_Qty,#tmp_AlloQty,#tmp_CutQty,#tmp_SewingQty,#tmp_ClogQty,#tmp_CutInLine
");
            #endregion

            DBProxy.Current.DefaultTimeout = 1800;
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            DBProxy.Current.DefaultTimeout = 300;
            return result;
        }

        /// <summary>
        /// ***若邏輯修改, PowerBI StoredProcedure [P_SewingLineSchedule]需一起調整***
        /// </summary>
        /// <returns>DualResult</returns>
        private DualResult Query_by_StylePerEachSewingDate()
        {
            PPIC_R01_ViewModel model = new PPIC_R01_ViewModel()
            {
                Inline = this.SewingDate1,
                Offline = this.SewingDate2,
                Line1 = this.line1,
                Line2 = this.line2,
                MDivisionID = this.mDivision,
                FactoryID = this.factory,
                BuyerDelivery1 = this.buyerDelivery1,
                BuyerDelivery2 = this.buyerDelivery2,
                SciDelivery1 = this.sciDelivery1,
                SciDelivery2 = this.sciDelivery2,
                Brand = this.brand,
                Subprocess = this.subProcess,
                IsPowerBI = false,
            };

            PPIC_R01 biModel = new PPIC_R01();
            Base_ViewModel resultReport = biModel.GetSewingLineScheduleData(model);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.printData = resultReport.DtArr[0];
            this.dtGantt = resultReport.DtArr[1];

            if (this.chkGanttChart.Checked)
            {
                resultReport.Result = this.GetGanttChartData();

                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }
            }

            return resultReport.Result;
        }

        private void ComboSummaryBy_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboSummaryBy.SelectedIndex == 2)
            {
                this.checkForPrintOut.Enabled = false;
                this.chkGanttChart.Enabled = true;
            }
            else
            {
                this.checkForPrintOut.Enabled = true;
                this.chkGanttChart.Enabled = false;
            }
        }

        private void BtnLastDownloadAPSDate_Click(object sender, EventArgs e)
        {
            new R01_LastAPSdownloadTime().ShowDialog();
        }

        private decimal FilterHour(DataTable drHours, string date, string sewingLineID, string factoryID)
        {
            if (date.Empty())
            {
                return MyUtility.Convert.GetDecimal(drHours.Compute("sum(Hours)", $@" SewingLineID = '{sewingLineID}' and FactoryID = '{factoryID}'"));
            }
            else
            {
                DataRow[] drHour = drHours.Select($@" Date = '{date}' and SewingLineID = '{sewingLineID}' and FactoryID = '{factoryID}'");
                decimal hour = 0;
                if (drHour.Length > 0)
                {
                    hour = MyUtility.Convert.GetDecimal(drHour[0]["Hours"]);
                }

                return hour;
            }
        }

        private DualResult GetGanttChartData()
        {
            #region Query Gantt Chart

            DualResult resultCmd;

            // from Detail甘特圖資料
            string sqlcmd2 = $@"
select SewingLineID,SewingDay,FactoryID
,[Total_StdOutput] = sum(StardardOutputPerDay)
,[s] = SewingCPU * sum(StardardOutputPerDay)
,[m] = sum(New_WorkHourPerDay) * Sewer
,[CPU] = sum(CPU)
into #tmpFinal_step1
from #tmp
where SewingDay between '{Convert.ToDateTime(this.SewingDate1):yyyy/MM/dd}' and '{Convert.ToDateTime(this.SewingDate2):yyyy/MM/dd}'
group by SewingLineID,SewingDay,SewingCPU,Sewer,FactoryID

select 
SewingLineID,SewingDay,FactoryID
,[CPU] = sum(CPU)
,[Total_StdOutput] = sum(Total_StdOutput)
,[PPH] = round(iif(isnull(sum(m),0)=0, 0, sum(s) / sum(m)),2)
from #tmpFinal_step1
group by SewingLineID,SewingDay,FactoryID

union all

-- Total by SewingLineID
select 
SewingLineID
,SewingDay = DATEADD(day,1, convert(date, max(SewingDay))) -- 目的:排序時 放置最後一欄位置
,FactoryID
,[CPU] = sum(CPU)
,[Total_StdOutput] = sum(Total_StdOutput)
,[PPH] = round(iif(isnull(sum(m),0)=0, 0, sum(s) / sum(m)),2)
from #tmpFinal_step1
group by SewingLineID,FactoryID
order by SewingLineID,SewingDay


-- by Factory SubTotal
select FactoryID
,[TotalCPU] = sum(CPU)
,[TotalStdQ] = sum(Total_StdOutput)
,[TotalPPH] = round(iif(isnull(sum(m),0)=0, 0, sum(s) / sum(m)),2)
from #tmpFinal_step1
group by FactoryID

drop table #tmpFinal_step1

";
            resultCmd = MyUtility.Tool.ProcessWithDatatable(this.printData, string.Empty, sqlcmd2, out this.dtGanttSumery);
            if (!resultCmd)
            {
                this.ShowErr(resultCmd);
                return resultCmd;
            }

            string sqlHours = $@"select FactoryID,Date,Hours,SewingLineID from WorkHour where date between '{Convert.ToDateTime(this.SewingDate1):yyyy/MM/dd}' and '{Convert.ToDateTime(this.SewingDate2):yyyy/MM/dd}'";
            resultCmd = DBProxy.Current.Select(null, sqlHours, out this.dtHours);
            if (!resultCmd)
            {
                this.ShowErr(resultCmd);
                return resultCmd;
            }
            #endregion

            return new DualResult(true);
        }
    }
}

using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R33 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DataTable[] tempDatas;
        private DateTime? AuditDate1;
        private DateTime? AuditDate2;
        private string MDivisionID;
        private string FactoryID;
        private string Brand;
        private string Stage;
        private List<DateTable> dateTables;
        private List<CFAInspectionRecord> cFAInspectionRecords;

        /// <inheritdoc/>
        public R33(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.AuditDate.Value1 = DateTime.Now;
            this.AuditDate.Value2 = DateTime.Now;
            this.comboStage.Text = "Staggered";
            this.comboM.SetDefalutIndex(false);
            this.comboFactory.SetDataSource();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.MDivisionID = this.comboM.Text;
            this.FactoryID = this.comboFactory.Text;
            this.Brand = this.txtBrand.Text;
            this.Stage = this.comboStage.Text;
            this.AuditDate1 = this.AuditDate.Value1;
            this.AuditDate2 = this.AuditDate.Value2;

            if (MyUtility.Check.Empty(this.AuditDate1) && MyUtility.Check.Empty(this.AuditDate2))
            {
                MyUtility.Msg.InfoBox("Audit Date can't be empty.");

                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.dateTables = new List<DateTable>();
            this.cFAInspectionRecords = new List<CFAInspectionRecord>();
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();
            string where = string.Empty;

            paramList.Add(new SqlParameter("@AuditDate1", this.AuditDate1));
            paramList.Add(new SqlParameter("@AuditDate2", this.AuditDate2));
            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                where += $"AND o.MDivisionID=@MDivisionID " + Environment.NewLine;
                paramList.Add(new SqlParameter("@MDivisionID", this.MDivisionID));
            }

            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                where += $"AND o.FtyGroup=@FactoryID " + Environment.NewLine;
                paramList.Add(new SqlParameter("@FactoryID", this.FactoryID));
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                where += $"AND o.BrandID=@BrandID " + Environment.NewLine;
                paramList.Add(new SqlParameter("@BrandID", this.Brand));
            }

            sqlCmd.Append($@"
/*1. 逐日表*/
CREATE Table #DateTable  (
      Date Date null
    , TotalInspectQty int null
    , TotalPassQty int null
    , PassRate numeric(7,4)
)
DECLARE @StartDate  date = @AuditDate1;
DECLARE @EndDate  date = @AuditDate2;
DECLARE @DayCount as int = (SELECT DATEDIFF(DAY,@StartDate,@EndDate));
DECLARE @Index as int = 0;

WHILE @Index <= @DayCount
BEGIN
	INSERT INTO #DateTable (Date)
	SELECT DATEADD(DAY, @Index, @StartDate) 
	SET @Index= @Index+1;
END


/*2. CFAInspectionRecord明細資料*/

----取得所有檢驗紀錄
SELECT c.*,co.OrderID,co.SEQ, co.Carton ,co.Ukey
INTO #MainData1
FROm CFAInspectionRecord  c
INNER JOIN CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
INNER JOIN Orders O ON o.ID = co.OrderID
WHERE 1=1
AND c.AuditDate BETWEEN @StartDate AND @EndDate
AND Stage='Staggered' 
AND Status='Confirmed'
AND c.FirstInspection = 1
{where}

----每個工廠的假日可能不同，因此不能存放在逐日表
SELECT [IsHoliday] = CAST( IIF(h.HolidayDate IS NULL , 0 , 1 ) as bit)
,[IsSunday]=CAST( IIF(DATENAME(DW, AuditDate) = 'Sunday', 1, 0) as bit)
,AuditDate
,Result
,MDivisionid
,c.FactoryID
,SewingLineID, Shift, InspectQty, DefectQty, c.ID
INTO #AllData
FROM CFAInspectionRecord c
LEFT JOIN Holiday h ON c.FactoryID = h.FactoryID AND c.AuditDate = h.HolidayDate
WHERE c.ID IN (SELECT ID FROM #MainData1)

----將by 日期計算的數值更改上去
UPDATE t
SET t.TotalInspectQty = total.TotalInspectQty
,t.TotalPassQty = pass.TotalInspectQty
,t.PassRate = (pass.TotalInspectQty * 1.0) / (total.TotalInspectQty * 1.0)
FROM #DateTable t
OUTER APPLY(
	SELECT [TotalInspectQty] = COUNT(1)
	FROM #AllData a
	WHERE a.AuditDate = t.Date
)total
OUTER APPLY(
	SELECT [TotalInspectQty] = COUNT(1)
	FROM #AllData a
	WHERE a.AuditDate = t.Date AND a.Result = 'Pass'
	GROUP BY AuditDate
)pass

SELECT * FROM #DateTable
SELECT * FROM #AllData

DROP TABLE #DateTable,#MainData1,#AllData
");
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), paramList, out this.tempDatas);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            this.dateTables = PublicPrg.DataTableToList.ConvertToClassList<DateTable>(this.tempDatas[0]).ToList();
            this.cFAInspectionRecords = PublicPrg.DataTableToList.ConvertToClassList<CFAInspectionRecord>(this.tempDatas[1]).ToList();

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.tempDatas[1].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Processing...");
            string templateName = "Quality_R33";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{templateName}.xltx"); // 預先開啟excel app

            objApp.Visible = false;

            // 是否跳出詢問視窗
            objApp.DisplayAlerts = false;
            Microsoft.Office.Interop.Excel.Worksheet perFactory_Sheet = objApp.ActiveWorkbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Worksheet perLine_Sheet = objApp.ActiveWorkbook.Worksheets[2];

            /*perFactory_Sheet*/
            #region perFactory_Sheet

            // 找出有哪些工廠，並複製出格子
            var factorys = this.cFAInspectionRecords.Select(o => o.FactoryID).Distinct();

            int colIndex = 2; // 起始位置為B欄
            foreach (var factory in factorys)
            {
                // 範本原本就有一個空格，因此第二個開始才要複製
                if (colIndex > 2)
                {
                    // 選擇要複製的
                    Microsoft.Office.Interop.Excel.Range copyRange = perFactory_Sheet.Range["B:D"];

                    // 選擇要被貼上的位置
                    Microsoft.Office.Interop.Excel.Range pasteRange = perFactory_Sheet.Range["E:E"];

                    // 選取要被複製的資料，然後貼上
                    pasteRange.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, copyRange.Copy());

                    // Sunday Holiday這兩個純文字去掉
                    perFactory_Sheet.Cells[2, 5] = string.Empty;
                    perFactory_Sheet.Cells[3, 5] = string.Empty;
                }

                // 順便填入值
                perFactory_Sheet.Cells[8, 2] = factory;

                // 一次跳三欄因此+3
                colIndex += 3;
            }

            // 找出有幾天，並複製出格子
            int rowIndex = 9; // 起始位置為第9列
            foreach (var item in this.dateTables.OrderByDescending(o => o.Date))
            {
                var currentDate = this.cFAInspectionRecords.Where(o => o.AuditDate == item.Date);

                if (rowIndex > 9)
                {
                    // 選擇要複製的
                    Microsoft.Office.Interop.Excel.Range copyRange = perFactory_Sheet.Range["9:9"];

                    // 選擇要被貼上的位置
                    Microsoft.Office.Interop.Excel.Range pasteRange = perFactory_Sheet.Range["10:10"];

                    // 選取要被複製的資料，然後貼上
                    pasteRange.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, copyRange.Copy());
                }

                // 填入資料
                perFactory_Sheet.Cells[9, 1] = item.Date;
                perFactory_Sheet.Cells[9, colIndex] = item.TotalInspectQty;
                perFactory_Sheet.Cells[9, colIndex + 1] = item.TotalPassQty;
                perFactory_Sheet.Cells[9, colIndex + 2] = item.PassRate;

                var currentDatas = this.cFAInspectionRecords.Where(o => o.AuditDate == item.Date);
                int ftyIdx = 2;
                foreach (var factory in factorys.ToList().OrderByDescending(x => x))
                {
                    var currentFty = currentDate.Where(o => o.FactoryID == factory);

                    int totalInspect = currentFty.Count();
                    int totalPass = currentFty.Where(o => o.FactoryID == factory && o.Result == "Pass").Count();
                    decimal passRate = MyUtility.Convert.GetDecimal((totalPass * 1.0) / (totalInspect * 1.0));
                    perFactory_Sheet.Cells[9, ftyIdx] = totalInspect;
                    perFactory_Sheet.Cells[9, ftyIdx + 1] = totalPass;
                    perFactory_Sheet.Cells[9, ftyIdx + 2] = passRate;

                    // 若沒有這天的檢驗資料，則另外判斷該天是否為假日
                    bool isHoliday = false;
                    if (!currentDatas.Any(o => o.IsHoliday && o.FactoryID == factory))
                    {
                        string c = $@"SELECT 1 FROM Holiday WHERE FactoryID='{factory}' AND HolidayDate='{item.Date.ToShortDateString()}' ";
                        isHoliday = MyUtility.Check.Seek(c);
                    }

                    // Cell顏色是否改變的判斷
                    string name = string.Empty;
                    if (item.Date.DayOfWeek.ToString() == "Sunday")
                    {
                        name = MyUtility.Excel.ConvertNumericToExcelColumn(ftyIdx);
                        perFactory_Sheet.get_Range($"{name}9:{name}9").Interior.Color = Color.FromArgb(255, 0, 0);

                        name = MyUtility.Excel.ConvertNumericToExcelColumn(ftyIdx + 1);
                        perFactory_Sheet.get_Range($"{name}9:{name}9").Interior.Color = Color.FromArgb(255, 0, 0);

                        name = MyUtility.Excel.ConvertNumericToExcelColumn(ftyIdx + 2);
                        perFactory_Sheet.get_Range($"{name}9:{name}9").Interior.Color = Color.FromArgb(255, 0, 0);
                    }
                    else if ((currentDatas.Any(o => o.IsHoliday && o.FactoryID == factory) && item.Date.DayOfWeek.ToString() != "Sunday") || isHoliday)
                    {
                        name = MyUtility.Excel.ConvertNumericToExcelColumn(ftyIdx);
                        perFactory_Sheet.get_Range($"{name}9:{name}9").Interior.Color = Color.FromArgb(0, 176, 240);

                        name = MyUtility.Excel.ConvertNumericToExcelColumn(ftyIdx + 1);
                        perFactory_Sheet.get_Range($"{name}9:{name}9").Interior.Color = Color.FromArgb(0, 176, 240);

                        name = MyUtility.Excel.ConvertNumericToExcelColumn(ftyIdx + 2);
                        perFactory_Sheet.get_Range($"{name}9:{name}9").Interior.Color = Color.FromArgb(0, 176, 240);
                    }
                    else
                    {
                        name = MyUtility.Excel.ConvertNumericToExcelColumn(ftyIdx);
                        perFactory_Sheet.get_Range($"{name}9:{name}9").Interior.Color = Color.FromArgb(255, 255, 255);

                        name = MyUtility.Excel.ConvertNumericToExcelColumn(ftyIdx + 1);
                        perFactory_Sheet.get_Range($"{name}9:{name}9").Interior.Color = Color.FromArgb(255, 255, 255);

                        name = MyUtility.Excel.ConvertNumericToExcelColumn(ftyIdx + 2);
                        perFactory_Sheet.get_Range($"{name}9:{name}9").Interior.Color = Color.FromArgb(255, 255, 255);
                    }

                    ftyIdx += 3;
                }

                // 一次跳1列，因此+1
                rowIndex += 1;
            }

            // 每個工廠下方的Total欄位
            int idx = 2;
            foreach (var factory in factorys)
            {
                string colName_TotalInspected = MyUtility.Excel.ConvertNumericToExcelColumn(idx);

                int dataStart = 9;
                int dataEnd = 9 + this.dateTables.Count - 1;

                // Sum TOTAL TIMES PO INSPECTED
                perFactory_Sheet.Cells[8 + this.dateTables.Count + 1, idx] = $"=SUM({colName_TotalInspected}{dataStart}:{colName_TotalInspected}{dataEnd})";

                // SUM TOTAL TIMES P.O PASSED
                string colName_TotalPass = MyUtility.Excel.ConvertNumericToExcelColumn(idx + 1);
                perFactory_Sheet.Cells[8 + this.dateTables.Count + 1, idx + 1] = $"=SUM({colName_TotalPass}{dataStart}:{colName_TotalPass}{dataEnd})";

                // AVG PASS RATE
                perFactory_Sheet.Cells[8 + this.dateTables.Count + 1, idx + 2] = $"={colName_TotalPass}{dataEnd + 1}/{colName_TotalInspected}{dataEnd + 1}";

                idx += 3;
            }

            // 填入最下方右邊Total， Y軸的值：上放固定的8列 + 日期天數 this.dateTables.Count + 1
            int lastRowIndex = 8 + this.dateTables.Count + 1;
            perFactory_Sheet.Cells[lastRowIndex, colIndex] = this.dateTables.Sum(o => o.TotalInspectQty);
            perFactory_Sheet.Cells[lastRowIndex, colIndex + 1] = this.dateTables.Sum(o => o.TotalPassQty);

            string col_TotalInspected = MyUtility.Excel.ConvertNumericToExcelColumn(colIndex);
            string col_TotalPass = MyUtility.Excel.ConvertNumericToExcelColumn(colIndex + 1);
            perFactory_Sheet.Cells[lastRowIndex, colIndex + 2] = $"={col_TotalPass}{lastRowIndex}/{col_TotalInspected}{lastRowIndex}";

            #endregion

            /*perLine_Sheet*/
            #region perLine_Sheet
            perLine_Sheet.Cells[1, 1] = $"Audit date: {this.AuditDate1.Value.ToShortDateString()} ~ {this.AuditDate2.Value.ToShortDateString()}";

            var data_ByLine_Shift = this.cFAInspectionRecords.Select(o => new { o.FactoryID, o.SewingLineID, o.Shift }).Distinct();
            int allCount = data_ByLine_Shift.Count();
            int allCounter = 0;
            int grandIdx = 6;

            foreach (var factory in factorys)
            {
                var sameFty = this.cFAInspectionRecords.Where(o => o.FactoryID == factory);
                var groupLineShift = sameFty.Select(o => new { o.SewingLineID, o.Shift }).Distinct().OrderByDescending(o => o.SewingLineID).ThenByDescending(o => o.Shift);

                int rowIdxByFty = 4;

                foreach (var lineShift in groupLineShift)
                {
                    // 第二筆開始複製
                    if (rowIdxByFty > 4 && allCounter < allCount)
                    {
                        // 選擇要複製的
                        Microsoft.Office.Interop.Excel.Range copyRange = perLine_Sheet.Range["4:4"];

                        // 選擇要被貼上的位置
                        Microsoft.Office.Interop.Excel.Range pasteRange = perLine_Sheet.Range["5:5"];

                        // 選取要被複製的資料，然後貼上
                        pasteRange.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, copyRange.Copy());

                        grandIdx += 1;
                    }

                    var sameLineShiftDatas = sameFty.Where(o => o.SewingLineID == lineShift.SewingLineID && o.Shift == lineShift.Shift);

                    perLine_Sheet.Cells[4, 1] = factory;
                    perLine_Sheet.Cells[4, 2] = lineShift.SewingLineID;
                    perLine_Sheet.Cells[4, 3] = lineShift.Shift;

                    int fail = sameLineShiftDatas.Where(o => o.Result == "Fail").Count();
                    int pass = sameLineShiftDatas.Where(o => o.Result == "Pass").Count();
                    int total = fail + pass;

                    perLine_Sheet.Cells[4, 4] = fail;
                    perLine_Sheet.Cells[4, 5] = pass;
                    perLine_Sheet.Cells[4, 6] = total;
                    perLine_Sheet.Cells[4, 7] = MyUtility.Convert.GetDecimal((pass * 1.0) / (total * 1.0));

                    // Total Sample Lot
                    int totalSample = sameLineShiftDatas.Sum(o => o.InspectQty);
                    perLine_Sheet.Cells[4, 8] = totalSample;

                    // Total Defect
                    int totalDefect = sameLineShiftDatas.Sum(o => o.DefectQty);
                    perLine_Sheet.Cells[4, 9] = totalDefect;

                    // SQR
                    perLine_Sheet.Cells[4, 10] = MyUtility.Convert.GetDecimal((totalDefect * 1.0) / (totalSample * 1.0));

                    rowIdxByFty++;
                    allCounter++;
                }

                int lastRowIdxByFty = rowIdxByFty;

                // 每個工廠寫一次就好
                int ftyFail = sameFty.Where(o => o.Result == "Fail").Count();
                int ftyPass = sameFty.Where(o => o.Result == "Pass").Count();
                int ftyTotal = ftyFail + ftyPass;
                perLine_Sheet.Cells[lastRowIdxByFty, 1] = factory;
                perLine_Sheet.Cells[lastRowIdxByFty, 4] = ftyFail;
                perLine_Sheet.Cells[lastRowIdxByFty, 5] = ftyPass;
                perLine_Sheet.Cells[lastRowIdxByFty, 6] = ftyTotal;
                perLine_Sheet.Cells[lastRowIdxByFty, 7] = MyUtility.Convert.GetDecimal((ftyPass * 1.0) / (ftyTotal * 1.0));

                // Fty Total Sample Lot
                int fty_totalSample = sameFty.Sum(o => o.InspectQty);
                perLine_Sheet.Cells[lastRowIdxByFty, 8] = fty_totalSample;

                // Fty Total Defect
                int fty_totalDefect = sameFty.Sum(o => o.DefectQty);
                perLine_Sheet.Cells[lastRowIdxByFty, 9] = fty_totalDefect;

                // Fty SQR
                perLine_Sheet.Cells[lastRowIdxByFty, 10] = MyUtility.Convert.GetDecimal((fty_totalDefect * 1.0) / (fty_totalSample * 1.0));

                // 設定全框線
                perLine_Sheet.Range[$"A4:J{lastRowIdxByFty}"].Borders.Weight = 2;

                // 合併儲存格
                perLine_Sheet.get_Range($"A4:A{lastRowIdxByFty}").Merge(false);

                if (allCounter < allCount)
                {
                    // 選擇要複製的
                    Microsoft.Office.Interop.Excel.Range copyRangeByFty = perLine_Sheet.Range[$"{lastRowIdxByFty - 1}:{lastRowIdxByFty}"];

                    // 選擇要被貼上的位置
                    Microsoft.Office.Interop.Excel.Range pasteRangeByFty = perLine_Sheet.Range["4:4"];

                    // 選取要被複製的資料，然後貼上
                    pasteRangeByFty.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, copyRangeByFty.Copy());

                    grandIdx += 2;
                }
            }

            int grand_Fail = this.cFAInspectionRecords.Where(o => o.Result == "Fail").Count();
            int grand_Pass = this.cFAInspectionRecords.Where(o => o.Result == "Pass").Count();
            int grand_Total = grand_Fail + grand_Pass;
            int grand_TotalSample = this.cFAInspectionRecords.Sum(o => o.InspectQty);
            int grand_Defect = this.cFAInspectionRecords.Sum(o => o.DefectQty);

            perLine_Sheet.Cells[grandIdx, 4] = grand_Fail;
            perLine_Sheet.Cells[grandIdx, 5] = grand_Pass;
            perLine_Sheet.Cells[grandIdx, 6] = grand_Total;
            perLine_Sheet.Cells[grandIdx, 7] = MyUtility.Convert.GetDecimal((grand_Pass * 1.0) / (grand_Total * 1.0));
            perLine_Sheet.Cells[grandIdx, 8] = grand_TotalSample;
            perLine_Sheet.Cells[grandIdx, 9] = grand_Defect;
            perLine_Sheet.Cells[grandIdx, 10] = MyUtility.Convert.GetDecimal((grand_Defect * 1.0) / (grand_TotalSample * 1.0));
            #endregion

            this.HideWaitMessage();

            #region Save Excel
            string strExcelName = Class.MicrosoftFile.GetName("QA_R33");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion

            return true;
        }

        private class CFAInspectionRecord
        {
            /// <inheritdoc/>
            public bool IsHoliday { get; set; }

            /// <inheritdoc/>
            public bool IsSunday { get; set; }

            /// <inheritdoc/>
            public DateTime AuditDate { get; set; }

            /// <inheritdoc/>
            public string Result { get; set; }

            /// <inheritdoc/>
            public string MDivisionID { get; set; }

            /// <inheritdoc/>
            public string FactoryID { get; set; }

            /// <inheritdoc/>
            public string SewingLineID { get; set; }

            /// <inheritdoc/>
            public string Shift { get; set; }

            /// <inheritdoc/>
            public int InspectQty { get; set; }

            /// <inheritdoc/>
            public int DefectQty { get; set; }
        }

        private class DateTable
        {
            /// <inheritdoc/>
            public DateTime Date { get; set; }

            /// <inheritdoc/>
            public int TotalInspectQty { get; set; }

            /// <inheritdoc/>
            public int TotalPassQty { get; set; }

            /// <inheritdoc/>
            public decimal PassRate { get; set; }
        }

    }
}

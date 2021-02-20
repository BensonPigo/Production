using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            this.comboM.SetDefalutIndex(true);
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
CREATE Table #DateTable  (Date Date null)
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

SELECT * FROM #DateTable

/*2. CFAInspectionRecord明細資料*/

SELECT c.*,co.OrderID,co.SEQ, co.Carton ,co.Ukey
INTO #MainData1
FROm CFAInspectionRecord  c
INNER JOIN CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
INNER JOIN Orders O ON o.ID = co.OrderID
WHERE 1=1
AND c.AuditDate BETWEEN @StartDate AND @EndDate
AND Stage='Staggered' 
AND Status='Confirmed'
{where}

SELECT [IsHoliday] = CAST( IIF(h.HolidayDate IS NULL , 0 , 1 ) as bit)
,AuditDate
,Result
,MDivisionid
,c.FactoryID
,SewingLineID,Shift,InspectQty,DefectQty
FROM CFAInspectionRecord c
LEFT JOIN Holiday h ON c.FactoryID = h.FactoryID AND c.AuditDate = h.HolidayDate
WHERE c.ID IN (SELECT ID FROM #MainData1)

DROP TABLE #DateTable,#MainData1
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

            string templateName = "Quality_R33";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{templateName}.xltx"); // 預先開啟excel app

            objApp.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet perFactory_Sheet = objApp.ActiveWorkbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Worksheet perLine_Sheet = objApp.ActiveWorkbook.Worksheets[2];

            /*perFactory_Sheet*/

            // 找出有哪些工廠，並複製出格子
            var factorys = this.cFAInspectionRecords.Select(o => o.FactoryID).Distinct();

            int colIndex = 2; // 起始位置為B欄
            foreach (var factory in factorys)
            {
                // 範本原本就有一個空格，因此第二個開始才要複製
                if (colIndex > 2)
                {
                    string excelColumn = MyUtility.Excel.ConvertNumericToExcelColumn(colIndex);

                    // 選擇要複製的
                    Microsoft.Office.Interop.Excel.Range copyRange = perFactory_Sheet.Range["B:D"];

                    // 選擇要被貼上的位置
                    Microsoft.Office.Interop.Excel.Range pasteRange = perFactory_Sheet.Range["E:E"];

                    // 選取要被複製的資料，然後貼上
                    pasteRange.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, copyRange.Copy());
                }

                // 順便填入值
                perFactory_Sheet.Cells[8, 2] = factory;

                // 一次跳三欄因此+3
                colIndex += 3;
            }

            // TOTAL TIMES PO INSPECTE 右邊要加總的欄未
            int ftyCount = factorys.Count();
            List<string> cols = new List<string>();
            string ss = "=SUM(";
            for (int i = 0; i <= ftyCount; i++)
            {
                int tOTALTIMESPOINSPECTED_Col = 2 + (i * 3);
                ss += MyUtility.Excel.ConvertNumericToExcelColumn(tOTALTIMESPOINSPECTED_Col) + "#Row#";

                if (i != ftyCount)
                {
                    ss += "+";
                }
                else
                {
                    ss += ")";
                }
            }

            // 找出有幾天，並複製出格子
            int rowIndex = 9; // 起始位置為第9列
            foreach (var item in this.dateTables.OrderByDescending(o => o.Date))
            {
                if (rowIndex > 9)
                {
                    // 選擇要複製的
                    Microsoft.Office.Interop.Excel.Range copyRange = perFactory_Sheet.Range["9:9"];

                    // 選擇要被貼上的位置
                    Microsoft.Office.Interop.Excel.Range pasteRange = perFactory_Sheet.Range["10:10"];

                    // 選取要被複製的資料，然後貼上
                    pasteRange.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, copyRange.Copy());
                }

                // 一次跳1列，因此+1
                perFactory_Sheet.Cells[9, 1] = item.Date;
                perFactory_Sheet.Cells[9, colIndex] = item.Date;
                string s = string.Empty;

                rowIndex += 1;
            }

            /*perLine_Sheet*/
            // 找出有哪些工廠，並複製出格子
            // 找出有幾Line，並複製出格子

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
        }

    }
}

using Ict;
using Sci.Data;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R15 : Win.Tems.PrintForm
    {
        private string sqlcmd;
        private DataTable dt;

        /// <inheritdoc/>
        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateInspection.HasValue1)
            {
                MyUtility.Msg.WarningBox("Inspection Date can't empty");
                return false;
            }

            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtuser1.TextBox1.Text))
            {
                where += $"AND fir.ShadeboneInspector = '{this.txtuser1.TextBox1.Text}'\r\n";
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text))
            {
                where += $"AND FIR.POID >= '{this.txtSP1.Text}'\r\n";
            }

            if (!MyUtility.Check.Empty(this.txtSP2.Text))
            {
                where += $"AND FIR.POID <= '{this.txtSP2.Text}'\r\n";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                where += $"AND o.BrandID = '{this.txtBrand.Text}'\r\n";
            }

            if (!MyUtility.Check.Empty(this.txtRefno1.Text))
            {
                where += $"AND FIR.Refno >= '{this.txtRefno1.Text}'\r\n";
            }

            if (!MyUtility.Check.Empty(this.txtRefno2.Text))
            {
                where += $"AND FIR.Refno <= '{this.txtRefno2.Text}'\r\n";
            }

            this.sqlcmd = $@"
SELECT
    InspectionDate = CAST(ShadeBondDate AS DATE)
   ,Pass1.Name
   ,[OutPut] = COUNT(1)
FROM FIR WITH(NOLOCK)
INNER JOIN FIR_Shadebone sb WITH(NOLOCK) ON sb.id = fir.ID
INNER JOIN orders o WITH(NOLOCK) ON o.POID = FIR.POID
INNER JOIN Pass1 WITH(NOLOCK) ON Pass1.ID = fir.ShadeboneInspector
WHERE CAST(ShadeBondDate AS DATE) BETWEEN '{this.dateInspection.Text1}' AND '{this.dateInspection.Text2}'
{where}
GROUP BY CAST(ShadeBondDate AS DATE),Pass1.Name
";
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlcmd, out this.dt);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.dt.Rows.Count);
            if (this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            string fileName = "Quality_R15";
            string fileNamexltx = fileName + ".xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + fileNamexltx); // 預先開啟excel app
            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            // 日期 Row
            var dateList = this.dt.AsEnumerable().Select(s => MyUtility.Convert.GetDate(s["InspectionDate"])).Distinct().OrderBy(o => o).ToList();
            int dateRowStart = 4;
            for (int i = 0; i < dateList.Count; i++)
            {
                if (i < dateList.Count - 1)
                {
                    worksheet.Rows[dateRowStart + 1 + i].Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                }

                worksheet.Cells[dateRowStart + i, 1] = dateList[i];
            }

            // 檢驗者 Column
            var nameList = this.dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["Name"])).Distinct().OrderBy(o => o).ToList();
            int nameColumnStart = 2;
            for (int i = 0; i < nameList.Count; i++)
            {
                if (i < nameList.Count - 1)
                {
                    worksheet.Columns[nameColumnStart + 1 + i].Insert(Excel.XlInsertShiftDirection.xlShiftToRight);
                    worksheet.Range[worksheet.Cells[2, nameColumnStart + 1 + i], worksheet.Cells[3, nameColumnStart + 1 + i]].Merge();
                }

                worksheet.Cells[2, nameColumnStart + i] = nameList[i];
            }

            // 將 Output List 中的數值填入對應的儲存格
            for (int i = 0; i < nameList.Count; i++)
            {
                for (int j = 0; j < dateList.Count; j++)
                {
                    var outputValue = this.dt.AsEnumerable()
                        .Where(s => MyUtility.Convert.GetString(s["Name"]) == nameList[i] && MyUtility.Convert.GetDate(s["InspectionDate"]) == dateList[j])
                        .Select(s => MyUtility.Convert.GetInt(s["Output"]))
                        .FirstOrDefault();

                    worksheet.Cells[dateRowStart + j, nameColumnStart + i] = outputValue;
                }
            }

            // 最右邊 Column 的總和公式
            int totalColumn = nameColumnStart + nameList.Count;
            int totalRow = dateRowStart + dateList.Count;
            for (int j = 0; j < dateList.Count; j++)
            {
                worksheet.Cells[dateRowStart + j, totalColumn] = $"=SUM({worksheet.Cells[dateRowStart + j, nameColumnStart].Address()}:{worksheet.Cells[dateRowStart + j, totalColumn - 1].Address()})";
            }

            // 下方 Row 的總和公式
            for (int i = 0; i < nameList.Count; i++)
            {
                worksheet.Cells[totalRow, nameColumnStart + i] = $"=SUM({worksheet.Cells[dateRowStart, nameColumnStart + i].Address()}:{worksheet.Cells[totalRow - 1, nameColumnStart + i].Address()})";
            }

            // 下方 Row 的平均公式
            for (int i = 0; i < nameList.Count; i++)
            {
                worksheet.Cells[totalRow + 1, nameColumnStart + i] = $"=AVERAGE({worksheet.Cells[dateRowStart, nameColumnStart + i].Address()}:{worksheet.Cells[totalRow - 1, nameColumnStart + i].Address()})";
            }

            // 填入最右下角的公式
            worksheet.Cells[totalRow, totalColumn] = $"=SUM({worksheet.Cells[totalRow, nameColumnStart].Address()}:{worksheet.Cells[totalRow, totalColumn - 1].Address()})";
            worksheet.Cells[totalRow + 1, totalColumn] = $"=AVERAGE({worksheet.Cells[totalRow + 1, nameColumnStart].Address()}:{worksheet.Cells[totalRow + 1, totalColumn - 1].Address()})";

            excelApp.Columns.AutoFit();
            string excelfile = Class.MicrosoftFile.GetName(fileName);
            excelApp.ActiveWorkbook.SaveAs(excelfile);
            excelApp.Visible = true;
            Marshal.ReleaseComObject(excelApp);
            this.HideWaitMessage();
            return true;
        }
    }
}

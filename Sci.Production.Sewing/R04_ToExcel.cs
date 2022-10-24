using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R04_ToExcel
    /// </summary>
    internal class R04_ToExcel
    {
        /// <summary>
        /// 製作R04 Excel
        /// </summary>
        /// <param name="isR04">isR04</param>
        /// <param name="show_Accumulate_output">show_Accumulate_output</param>
        /// <param name="dtR04">dtR04</param>
        /// <param name="dateMaxOutputDate">dateMaxOutputDate</param>
        /// <param name="excelName">excelName</param>
        /// <returns>true</returns>
        public static bool ToExcel(bool isR04, bool show_Accumulate_output, DataTable dtR04,DateTime? dateMaxOutputDate,ref string excelName)
        {
            int start_column = 0;
            string excelFile = "Sewing_R04_SewingDailyOutputList.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + excelFile); // 開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (show_Accumulate_output == true)
            {
                start_column = 49;
            }
            else
            {
                start_column = 47;
                objSheets.get_Range("AU:AV").EntireColumn.Delete();
            }

            for (int i = start_column; i < dtR04.Columns.Count; i++)
            {
                objSheets.Cells[1, i + 1] = dtR04.Columns[i].ColumnName;
            }

            string r = MyUtility.Excel.ConvertNumericToExcelColumn(dtR04.Columns.Count);
            objSheets.get_Range("A1", r + "1").Cells.Interior.Color = Color.LightGreen;
            objSheets.get_Range("A1", r + "1").AutoFilter(1);

            if (isR04 == true)
            {
                bool result = MyUtility.Excel.CopyToXls(dtR04, string.Empty, xltfile: excelFile, headerRow: 1, excelApp: objApp);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                }
            }
            else
            {
                objApp.Visible = false;
                if (dtR04.Rows.Count != 0)
                {
                    MyUtility.Excel.CopyToXls(dtR04, string.Empty, "Sewing_R04_SewingDailyOutputList.xltx", 1, false, null, objApp);
                }

                excelName = Path.Combine(
                    Env.Cfg.ReportTempDir,
                    "Sewing daily output list -" + ((DateTime)dateMaxOutputDate).ToString("_yyyyMMdd") + DateTime.Now.ToString("_HHmmssfff") + "(" + Env.User.Factory + ").xlsx");
                objApp.ActiveWorkbook.SaveAs(excelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);
            }

            return true;
        }
    }
}

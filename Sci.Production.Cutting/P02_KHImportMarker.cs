using Ict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02
    /// </summary>
    public partial class P02
    {
        private DualResult ImportKHMarkerExcel()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";

            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return new DualResult(true);
            }

            this.ShowWaitMessage("Loading...");
            string filename = openFileDialog.FileName;

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            excel.Workbooks.Open(MyUtility.Convert.GetString(filename));

            int sheetCnt = excel.ActiveWorkbook.Worksheets.Count;

            for (int i = 1; i <= sheetCnt; i++)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[i];

                // 檢查SP
                string poID = worksheet.Cells[2, 2].Value.ToString();

                if (poID != this.CurrentMaintain["ID"].ToString())
                {
                    Marshal.ReleaseComObject(worksheet);
                    continue;
                }

                

                Marshal.ReleaseComObject(worksheet);
            }

            return new DualResult(true);
        }
    }
}

using Ict;
using Sci.Utility.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MsExcel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Prg
{
    /// <inheritdoc />
    public static class ExcelExtension
    {
        /// <summary>
        /// GetValue
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="range">range</param>
        /// <returns>T GetValueT</returns>
        public static T GetValue<T>(this MsExcel.Range range)
        {
            return (T)range.get_Value();
        }

        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="range">range</param>
        /// <param name="value">value</param>
        public static void SetValue(this MsExcel.Range range, object value)
        {
            range.Value = value;
        }

        /// <summary>
        /// 用RangeName去取Excel.Range物件，等同於WorkSheet.Range["RangeName"]
        /// </summary>
        /// <inheritdoc />
        public static MsExcel.Range GetRange(this MsExcel.Worksheet sheet, string rangeName)
        {
            return sheet.Range[rangeName];
        }

        /// <inheritdoc />
        public static MsExcel.Range GetRange(this MsExcel.Worksheet sheet, int cellA_X, int cellA_Y, int cellB_X, int cellB_Y)
        {
            return sheet.Range[sheet.Cells[cellA_Y, cellA_X], sheet.Cells[cellB_Y, cellB_X]];
        }

        /// <summary>
        /// 把二層的物件陣列，轉為單層二維物件陣列
        /// </summary>
        /// <inheritdoc />
        public static object[,] DoubleArrayConvert2DArray(this IEnumerable<IEnumerable<object>> rowData)
        {
            var d1Length = rowData.Count();
            if (d1Length == 0)
            {
                return new object[0, 0];
            }

            var d2Length = rowData.Max(item => item.Count());
            if (d2Length == 0)
            {
                return new object[d1Length, 0];
            }

            var value2 = new object[d1Length, d2Length];

            rowData.Select((item1, index1) =>
            {
                var valueLevel2 = Enumerable.Range(0, d2Length)
                    .Select(index2 =>
                    {
                        if (item1.Count() > index2)
                        {
                            value2[index1, index2] = item1.Skip(index2).Take(1).First();
                        }

                        return true;
                    })
                    .ToList();
                return true;
            })
            .ToList();
            return value2;
        }

        /// <inheritdoc />
        public static void InsertMutipleRows(this MsExcel.Worksheet sheet, MsExcel.Range firstRow, int rowNumberToInsert)
        {
            var insertPos = sheet.Cells[1, firstRow.Row + 1].EntireRow;
            int i = 1;
            int step = 1;
            while (i < rowNumberToInsert)
            {
                // copy the existing row(s)
                sheet.get_Range(
                    "$" + firstRow.Row + ":$" + (firstRow.Row + step - 1),
                    Type.Missing).Copy(Type.Missing);

                // insert copied rows
                insertPos.Insert(MsExcel.XlInsertShiftDirection.xlShiftDown, Type.Missing);

                i += step;

                int diff = rowNumberToInsert - i;
                if (diff > (step * 2))
                {
                    step *= 2;
                }
                else
                {
                    step = diff;
                }
            }
        }

        /// <summary>
        /// 匯出Grid直接存檔案併回傳檔名
        /// </summary>
        /// <param name="fileName">Excel Title, FileName</param>
        /// <param name="grid">Grid</param>
        /// <returns>
        /// true : fileName in description
        /// flase : errorMSG in Exception
        /// </returns>
        public static DualResult GridToExcelOnlySave(string fileName, System.Windows.Forms.DataGridView grid)
        {
            if (grid == null)
            {
                return Result.F_ArgNull("grid");
            }

            DualResult result;
            string strExcelName;
            MsExcel.Application objApp = null;
            try
            {
                strExcelName = Class.MicrosoftFile.GetName(fileName);
                DataTable gridSource = null;
                if (grid.DataSource is DataTable)
                {
                    gridSource = (DataTable)grid.DataSource;
                }
                else if (grid.DataSource is System.Windows.Forms.BindingSource)
                {
                    gridSource = (DataTable)((System.Windows.Forms.BindingSource)grid.DataSource).DataSource;
                }
                else
                {
                    return Result.F("Grid data should be DataTable");
                }

                ExcelCOM com = new ExcelCOM(null, objApp);
                if (com.ExcelApp == null)
                {
                    com.CreateApplication();
                }

                objApp = com.ExcelApp;
                DualResult ok = com.WriteTable(gridSource, 1, needHeader: true);
                if (!ok)
                {
                    throw new Exception(ok.Messages.ToString());
                }

                com.ColumnsAutoFit = true;
                com.SaveOnly(strExcelName);
                objApp.Quit();

                result = new DualResult(true, strExcelName);
            }
            catch (Exception ex)
            {
                result = new DualResult(false, ex);
            }
            finally
            {
                Marshal.ReleaseComObject(objApp);
            }

            return result;
        }
    }
}

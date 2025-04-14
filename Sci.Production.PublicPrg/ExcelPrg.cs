using Ict;
using Sci.Production.PublicPrg;
using Sci.Utility.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using MsExcel = Microsoft.Office.Interop.Excel;
using Image = System.Drawing.Image;

namespace Sci.Production.Prg
{
    /// <summary>
    /// Excel Program
    /// </summary>
    public partial class ExcelPrg
    {
        /// <summary>
        /// 把Excel的一個WorkSheet變成一個DataTable
        /// </summary>
        /// <param name="fileName">如果不指定檔名，會開對話視窗去選</param>
        /// <param name="sheetNum">如果不指定SheetIndex會取第一個</param>
        /// <param name="captionIndex">如果不指定標題列位置，預設第一列</param>
        /// <param name="columnTypes">如果有指定，則遇到特定的標題，就會用特定的型態來給DataColumn用，未指定或找不到的，一律用string為預設</param>
        /// <inheritdoc/>
        public static System.Data.DataTable SelectExcelEx(string fileName = null, int sheetNum = 1, int captionIndex = 1, Dictionary<string, Type> columnTypes = null)
        {
            return SelectExcelEx(fileName, sheetNum, null, captionIndex, columnTypes);
        }

        private static System.Data.DataTable SelectExcelEx(string fileName, int sheetNum, string sheetName, int captionIndex, Dictionary<string, Type> columnTypes)
        {
            return new SelectExcelAgent()
            {
                FileName = fileName,
                SheetNumber = sheetNum,
                SheetName = sheetName,
                CaptionIndex = captionIndex,
                ColumnTypes = columnTypes,
            }.Load();
        }

        private static System.Data.DataTable SelectExcelEx(SelectExcelAgent setting)
        {
            if (setting.FileName.IsNullOrWhiteSpace())
            {
                SciFileDialog.ShowDialog(dlg => setting.FileName = dlg.FileName);
                if (setting.FileName.IsNullOrWhiteSpace())
                {
                    return null;
                }
            }

            MsExcel.Application app = new MsExcel.Application
            {
                FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip,
            };
            MsExcel.Worksheet sheet = null;
            try
            {
                if (setting.SheetName.IsNullOrWhiteSpace())
                {
                    sheet = app.Workbooks.Add(setting.FileName).Worksheets[setting.SheetNumber];
                }
                else
                {
                    sheet = app.Workbooks.Add(setting.FileName).Worksheets[setting.SheetName.TrimEnd('$')];
                }
            }
            catch (Exception ex)
            {
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
                throw ex;
            }
            finally
            {
                app.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationDefault;
            }

            var usedRange = sheet.UsedRange;
            int rowCount = usedRange.Rows[1].Row + usedRange.Rows.Count - 1;
            int columnCount = usedRange.Columns[1].Column + usedRange.Columns.Count - 1;

            var lines = new List<object[]>();
            try
            {
                // 假定的數字，希望一次載入不要超過十萬格
                var loadCellCountLimit = 100000;
                var batchRowCount = (rowCount * columnCount <= loadCellCountLimit) ? rowCount : Convert.ToInt32(loadCellCountLimit / columnCount);

                var totalLoaddedRowCount = 0;
                do
                {
                    var loadCountThisRound = rowCount - totalLoaddedRowCount > batchRowCount ? batchRowCount : rowCount - totalLoaddedRowCount;
                    usedRange = sheet.GetRange(1, totalLoaddedRowCount + 1, columnCount, totalLoaddedRowCount + loadCountThisRound);

                    object[,] rangeValue = null;
                    rangeValue = (object[,])usedRange.Value2;

                    var lineInThisLoad = Enumerable.Range(1, rangeValue.GetLength(0))
                        .Select(idxY => Enumerable.Range(1, rangeValue.GetLength(1))
                            .Select(idxX => rangeValue[idxY, idxX]).ToArray())
                            .ToList();

                    lines.AddRange(lineInThisLoad);

                    totalLoaddedRowCount += loadCountThisRound;
                }
                while (totalLoaddedRowCount < rowCount && app != null);
            }
            finally
            {
                sheet.Parent.Close(SaveChanges: false);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                sheet = null;
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
            }

            var usedColumnIndexies = new List<int>();
            var dateTimeColumnIndexies = new List<int>();
            var dt = new System.Data.DataTable();
            if (lines.Count >= setting.CaptionIndex)
            {
                lines.Skip(setting.CaptionIndex - 1)
                    .FirstOrDefault()
                    .Select(fieldValue => setting.AutoTrimColumnName ? Convert.ToString(fieldValue).Trim() : Convert.ToString(fieldValue))
                    .Select((fieldName, idx) =>
                    {
                        if (fieldName.IsNullOrWhiteSpace() == false)
                        {
                            var columnType = typeof(string);
                            if (setting.ColumnTypes != null && setting.ColumnTypes.ContainsKey(fieldName))
                            {
                                columnType = setting.ColumnTypes[fieldName];
                            }

                            usedColumnIndexies.Add(idx);

                            var idxSuffix = 1;
                            while (dt.Columns.Contains(fieldName))
                            {
                                fieldName = fieldName + idxSuffix++;
                            }

                            dt.Columns.Add(fieldName, columnType);
                            if (columnType == typeof(DateTime))
                            {
                                dateTimeColumnIndexies.Add(idx);
                            }
                        }

                        return 1;
                    })
                    .ToList();
                lines
                    .Skip(setting.CaptionIndex)
                    .ToList()
                    .ForEach(line => dt.LoadDataRow(
                        usedColumnIndexies.Select(idx =>
                        {
                            if (line[idx] == null)
                            {
                                return null;
                            }
                            else if (dateTimeColumnIndexies.Contains(idx))
                            {
                                if (line[idx] is double)
                                {
                                    return DateTime.FromOADate((double)line[idx]);
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            else
                            {
                                if (setting.AutoTrimContant)
                                {
                                    var obj = line[idx].ToString().Trim();
                                    if (string.IsNullOrEmpty(obj))
                                    {
                                        return null;
                                    }
                                    else
                                    {
                                        return (object)obj;
                                    }
                                }
                                else
                                {
                                    return line[idx];
                                }
                            }
                        }).ToArray(), true));
            }

            // 2020/01/22 [IST20200124] add by Poya 檢查若有整列都空白的資料直接排除
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                var row = dt.Rows[i];
                if (row.ItemArray.AsEnumerable().Where(w => w != DBNull.Value).Any() == false)
                {
                    dt.Rows.RemoveAt(i);
                }
            }

            return dt;
        }

        /// <summary>
        /// 如果資料超過100萬筆，自動拆分sheet，防止記憶體爆掉60萬放一個sheet
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="fileName">fileName</param>
        /// <param name="xltfile">xltfile</param>
        /// <param name="headerRow">headerRow</param>
        /// <param name="showExcel">showExcel</param>
        /// <param name="fieldList">fieldList</param>
        /// <param name="excelApp">excelApp</param>
        /// <param name="showSaveMsg">showSaveMsg</param>
        /// <param name="wSheet">wSheet</param>
        /// <param name="DisplayAlerts_ForSaveFile">DisplayAlerts_ForSaveFile</param>
        /// <param name="excelType">excelType</param>
        /// <returns>DualResult</returns>
        public static DualResult CopyToXlsAutoSplitSheet(DataTable data, string fileName, string xltfile = "", int headerRow = 1, bool showExcel = true, string fieldList = null, object excelApp = null, bool showSaveMsg = true, object wSheet = null, bool DisplayAlerts_ForSaveFile = false, ExcelCOM.ExcelType excelType = ExcelCOM.ExcelType.xlsx)
        {
            if (data == null)
            {
                return new DualResult(false, "Source no data");
            }

            if (data.Rows.Count == 0)
            {
                return new DualResult(false, "Source no data");
            }

            try
            {
                if (data.Rows.Count <= 1000000)
                {
                    MyUtility.Excel.CopyToXls(data, fileName, xltfile, headerRow, showExcel, fieldList, excelApp, showSaveMsg, wSheet, DisplayAlerts_ForSaveFile, excelType);
                }
                else
                {
                    int sheetCnt = MyUtility.Convert.GetInt(Math.Ceiling(data.Rows.Count / 600000.0));
                    MsExcel.Workbook xlWb = ((MsExcel.Application)excelApp).ActiveWorkbook;
                    for (int i = 1; i < sheetCnt; i++)
                    {
                        MsExcel.Worksheet xlSht = xlWb.Sheets[1];
                        xlSht.Copy(Type.Missing, xlWb.Sheets[xlWb.Sheets.Count]); // copy
                        Marshal.FinalReleaseComObject(xlSht);
                    }

                    for (int i = 0; i < sheetCnt; i++)
                    {
                        DataTable dtPrint = data.AsEnumerable().Skip(i * 600000).Take(600000).TryCopyToDataTable(data);
                        MyUtility.Excel.CopyToXls(dtPrint, fileName, xltfile, headerRow, showExcel, fieldList, excelApp, showSaveMsg, xlWb.Sheets[i + 1], DisplayAlerts_ForSaveFile, excelType);
                    }

                    Marshal.FinalReleaseComObject(xlWb);
                }
            }
            catch (Exception ex)
            {
                return new DualResult(true, ex);
            }

            return new DualResult(true);
        }

        /// <inheritdoc />
        public class SelectExcelAgent
        {
            /// <inheritdoc />
            public string FileName { get; set; }

            /// <inheritdoc />
            public string SheetName { get; set; }

            /// <inheritdoc />
            public int SheetNumber { get; set; }

            /// <inheritdoc />
            public int CaptionIndex { get; set; }

            /// <inheritdoc />
            public Dictionary<string, Type> ColumnTypes { get; set; }

            /// <inheritdoc />
            public bool AutoTrimColumnName { get; set; }

            /// <inheritdoc />
            public bool AutoTrimContant { get; set; }

            /// <inheritdoc />
            public SelectExcelAgent()
            {
                this.FileName = string.Empty;
                this.SheetName = string.Empty;
                this.SheetNumber = 1;
                this.CaptionIndex = 1;
                this.ColumnTypes = new Dictionary<string, Type>();
                this.AutoTrimColumnName = true;
                this.AutoTrimContant = false;
            }

            /// <inheritdoc />
            public System.Data.DataTable Load()
            {
                return SelectExcelEx(this);
            }
        }

        /// <summary>
        /// 輸入欄位名稱,刪除指定的 Excel 欄位
        /// </summary>
        /// <param name="worksheet">Microsoft.Office.Interop.Excel.Worksheet</param>
        /// <param name="headerRow">範本檔案設定欄位在第幾Row</param>
        /// <param name="columnName">欄位名稱</param>
        public static void ExcelDeleteColumn(MsExcel.Worksheet worksheet, int headerRow, string columnName)
        {
            MsExcel.Range headerRange = worksheet.Rows[headerRow];
            MsExcel.Range searchRange = headerRange.Find(columnName, Type.Missing, MsExcel.XlFindLookIn.xlValues, MsExcel.XlLookAt.xlWhole, MsExcel.XlSearchOrder.xlByRows, MsExcel.XlSearchDirection.xlNext, false, false, Type.Missing);
            if (searchRange != null)
            {
                // 刪除欄位
                int columnIndex = searchRange.Column;
                MsExcel.Range deleteRange = (MsExcel.Range)worksheet.Columns[columnIndex];
                deleteRange.Delete(MsExcel.XlDeleteShiftDirection.xlShiftToLeft);
            }
        }

        /// <summary>
        /// 將Byte array 轉成實體圖片檔
        /// </summary>
        /// <param name="imageBytes">圖片Byte array</param>
        /// <returns>圖片實體路徑</returns>
        public static string ConvertImgPath(byte[] imageBytes)
        {
            Image img = Image.FromStream(new MemoryStream(imageBytes));
            string imageName = $"{Guid.NewGuid()}.jpg";
            string imgPath;
            imgPath = Path.Combine(Env.Cfg.ReportTempDir, imageName);
            img.Save(imgPath);
            img.Dispose();
            return imgPath;
        }
    }
}

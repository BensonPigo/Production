using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sci.Production.PublicPrg;
using MsExcel = Microsoft.Office.Interop.Excel;

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

            MsExcel.Application app = new MsExcel.Application();
            app.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip;
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
    }
}

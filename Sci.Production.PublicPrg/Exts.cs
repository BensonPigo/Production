using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Drawing;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using MsExcel = Microsoft.Office.Interop.Excel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sci.Production.Prg
{
    /// <inheritdoc/>
    public static class ProjExts
    {
        /// <inheritdoc/>
        public static T Val<T>(this DataRow row, string name)
        {
            var v = row[name];
            if (v == DBNull.Value)
            {
                return default(T);
            }

            return (T)v;
        }

        /// <inheritdoc/>
        public static T Val<T>(this DataRow row, DataColumn col)
        {
            var v = row[col];
            if (v == DBNull.Value)
            {
                return default(T);
            }

            return (T)v;
        }

        /// <inheritdoc/>
        public static IDictionary<TKEY, DataRow> ToDictionary<TKEY>(this DataTable datas, Func<DataRow, TKEY> keyselector, bool ignore_deleted = false)
        {
            if (keyselector == null)
            {
                new ArgumentNullException("keyselector");
            }

            if (datas == null)
            {
                return null;
            }

            var dic = new Dictionary<TKEY, DataRow>();
            foreach (DataRow it in datas.Rows)
            {
                if (ignore_deleted && it.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                var key = keyselector(it);

                dic.Add(key, it);
            }

            return dic;
        }

        /// <inheritdoc/>
        public static IDictionary<TKEY, IList<DataRow>> ToDictionaryList<TKEY>(this DataTable datas, Func<DataRow, TKEY> keyselector, bool ignore_deleted = false)
        {
            if (keyselector == null)
            {
                new ArgumentNullException("keyselector");
            }

            if (datas == null)
            {
                return null;
            }

            var dic = new Dictionary<TKEY, IList<DataRow>>();
            foreach (DataRow it in datas.Rows)
            {
                if (ignore_deleted && it.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                var key = keyselector(it);

                IList<DataRow> rows;
                if (!dic.TryGetValue(key, out rows))
                {
                    dic.Add(key, rows = new List<DataRow>());
                }

                rows.Add(it);
            }

            return dic;
        }

        /// <summary>
        /// 比對Datarow 新舊值是否一致
        /// </summary>
        /// <param name="dr">dr</param>
        /// <param name="compareColumns">欄位名稱，比較多個欄位以逗號分隔(AA,BB)</param>
        /// <returns>bool</returns>
        public static bool CompareDataRowVersionValue(this DataRow dr, string compareColumns)
        {
            if (MyUtility.Check.Empty(compareColumns))
            {
                return false;
            }

            if (dr.RowState != DataRowState.Modified)
            {
                return false;
            }

            foreach (string col in compareColumns.Split(','))
            {
                switch (Type.GetTypeCode(dr[col].GetType()))
                {
                    case TypeCode.Boolean:
                        if (MyUtility.Convert.GetBool(dr[col, DataRowVersion.Current]) != MyUtility.Convert.GetBool(dr[col, DataRowVersion.Original]))
                        {
                            return true;
                        }

                        break;
                    case TypeCode.DateTime:
                        if (MyUtility.Convert.GetDate(dr[col, DataRowVersion.Current]) != MyUtility.Convert.GetDate(dr[col, DataRowVersion.Original]))
                        {
                            return true;
                        }

                        break;
                    default:
                        if (dr[col, DataRowVersion.Current].ToString() != dr[col, DataRowVersion.Original].ToString())
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// 全複製 source,target 結構必須一樣
        /// </summary>
        /// <inheritdoc/>
        public static DataRow CopyTo(this DataRow source, DataRow target)
        {
            try
            {
                List<string> columnList = new List<string>();
                foreach (DataColumn column in source.Table.Columns)
                {
                    columnList.Add(column.ColumnName);
                }

                string fieldNames = columnList.JoinToString(",");
                return source.CopyTo(target, fieldNames);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.WarningBox(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 如果資料列大於0，則會呼叫原本的CopyToDataTable，不然會用SrcTable做Clone
        /// </summary>
        /// <inheritdoc/>
        public static DataTable TryCopyToDataTable(this IEnumerable<DataRow> rows, DataTable srcTableForSchema)
        {
            if (rows.Any())
            {
                return rows.CopyToDataTable();
            }
            else
            {
                return srcTableForSchema.Clone();
            }
        }

        /// <summary>
        /// 複製datatable並保留來源datatable狀態
        /// </summary>
        /// <param name="source">source</param>
        /// <returns>DataTable</returns>
        public static DataTable ToTableKeepRowState(this DataView source)
        {
            DataTable copiedDataTable = source.Table.Clone();

            foreach (DataRowView originalRowView in source)
            {
                DataRow originalRow = originalRowView.Row;
                DataRow copiedRow = copiedDataTable.NewRow();
                copiedRow.ItemArray = originalRow.ItemArray; // 复制原始行的数据到新的行
                copiedDataTable.Rows.Add(copiedRow);

                // 设置新行的状态为原始行的状态
                copiedRow.AcceptChanges(); // 先将行状态重置为 Unchanged
                if (originalRow.RowState == DataRowState.Modified)
                {
                    copiedRow.SetModified();
                }
                else if (originalRow.RowState == DataRowState.Deleted)
                {
                    copiedRow.Delete();
                }
            }

            return copiedDataTable;
        }

        /// <inheritdoc/>
        public static void ImportRowAdded(this DataTable dt, DataRow row)
        {
            row.AcceptChanges();
            row.SetAdded();
            dt.ImportRow(row);
        }

        /// <summary>
        /// 取得字串右邊字數
        /// </summary>
        /// <param name="s">原字串</param>
        /// <param name="length">字數</param>
        /// <returns>string</returns>
        public static string Right(this string s, int length)
        {
            length = Math.Max(length, 0);
            if (s.Length > length)
            {
                return s.Substring(s.Length - length, length);
            }
            else
            {
                return s;
            }
        }

        /// <summary>
        /// 取得字串左邊字數
        /// </summary>
        /// <param name="s">原字串</param>
        /// <param name="length">字數</param>
        /// <returns>string</returns>
        public static string Left(this string s, int length)
        {
            length = Math.Max(length, 0);
            if (s.Length > length)
            {
                return s.Substring(0, length);
            }
            else
            {
                return s;
            }
        }

        /// <summary>
        /// 正規表示法 檢查 Email格式
        /// </summary>
        /// <param name="s">Email</param>
        /// <returns>bool</returns>
        public static bool IsEmail(this string s)
        {
            if (s.Empty())
            {
                return false;
            }
            else
            {
                return System.Text.RegularExpressions.Regex.IsMatch(s, @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            }
        }

        /// <summary>
        /// List To DataTable
        /// </summary>
        /// <typeparam name="T">List</typeparam>
        /// <param name="items">items</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Get all the properties
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                // Defining type of data column gives proper data table
                var type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;

                // Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    // inserting property values to datatable rows
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            // put a breakpoint here and check datatable
            return dataTable;
        }

        /// <summary>
        /// 將 QR code 轉換成圖片
        /// </summary>
        /// <param name="strBarcode">Barcode 內容</param>
        /// <returns>Image</returns>
        public static Bitmap ToBitmapQRcode(this string strBarcode, int height, int width)
        {
            /*
  Level L (Low)      7%  of codewords can be restored.
  Level M (Medium)   15% of codewords can be restored.
  Level Q (Quartile) 25% of codewords can be restored.
  Level H (High)     30% of codewords can be restored.
*/
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    // Create Photo
                    Height = height,
                    Width = width,
                    Margin = 0,
                    CharacterSet = "UTF-8",
                    PureBarcode = true,

                    // 錯誤修正容量
                    // L水平    7%的字碼可被修正
                    // M水平    15%的字碼可被修正
                    // Q水平    25%的字碼可被修正
                    // H水平    30%的字碼可被修正
                    ErrorCorrection = ErrorCorrectionLevel.L,
                },
            };

            return writer.Write(strBarcode);
        }

        /// <summary>
        /// GetCellValue
        /// </summary>
        /// <param name="worksheet">worksheet</param>
        /// <param name="indexCol">indexCol</param>
        /// <param name="indexRow">indexRow</param>
        /// <returns>string</returns>
        public static string GetCellValue(this MsExcel.Worksheet worksheet, int indexCol, int indexRow)
        {

            if (worksheet.Cells[indexCol, indexRow] == null)
            {
                return string.Empty;
            }
            else if (worksheet.Cells[indexRow, indexCol].Value == null)
            {
                return string.Empty;
            }
            else
            {
                return worksheet.Cells[indexRow, indexCol].Value.ToString();
            }
        }

        /// <summary>
        /// GetCellValue
        /// </summary>
        /// <param name="range">range</param>
        /// <param name="indexCol">indexCol</param>
        /// <param name="indexRow">indexRow</param>
        /// <returns>string</returns>
        public static string GetCellValue(this MsExcel.Range range, int indexCol, int indexRow)
        {

            if (range.Cells[indexCol, indexRow] == null)
            {
                return string.Empty;
            }
            else if (range.Cells[indexRow, indexCol].Value == null)
            {
                return string.Empty;
            }
            else
            {
                return range.Cells[indexRow, indexCol].Value.ToString();
            }
        }

        /// <summary>
        /// GetPackScanContent
        /// </summary>
        /// <param name="srcBarcode">srcBarcode</param>
        /// <returns>string</returns>
        public static string GetPackScanContent(this string srcBarcode)
        {
            if (MyUtility.Check.Seek($"select 1 from Packinglist_Detail with (nolock) where SCICtnNo = '{srcBarcode.Left(15)}'"))
            {
                return srcBarcode.Left(15);
            }

            return srcBarcode.Left(16);
        }

        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            return type.GetProperties()
                 .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name == attributeName)
                 .FirstOrDefault();
        }

        private static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// fill datarow value to object
        /// </summary>
        /// <param name="dataRow"> source datarow </param>
        /// <param name="item"> item of datarow table class </param>
        public static void DatarowFillObject(DataRow dataRow, object item)
        {
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo property = GetProperty(item.GetType(), column.ColumnName);

                if (property != null && dataRow[column] != DBNull.Value /*&& dataRow[column].ToString() != "NULL"*/)
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
            }
        }

        public static bool PropertiesEqual<T>(T self, T to, params string[] ignore)
            where T : class
        {
            if (self != null && to != null)
            {
                Type type = typeof(T);

                List<string> ignoreList = new List<string>();
                if (ignore != null)
                {
                    ignoreList = new List<string>(ignore);
                }

                foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    if (!ignoreList.Contains(pi.Name))
                    {
                        object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                        object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return self == to;
        }

        /// <summary>
        /// GetDescription
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>string</returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);

            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = field.GetCustomAttribute<DescriptionAttribute>();
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// DecimalToFraction
        /// </summary>
        /// <param name="number">number</param>
        /// <returns>string</returns>
        public static string DecimalToFraction(decimal number)
        {
            // 將小數轉換為分數
            decimal epsilon = 0.0001M; // 適當的誤差範圍
            int maxDenominator = 10; // 分母的最大值（1位數）

            for (int denominator = 1; denominator <= maxDenominator; denominator++)
            {
                int numerator = (int)Math.Round(number * denominator);

                // 檢查分子和分母是否超過1位數
                if (numerator > 9 || denominator > 9)
                {
                    return "0/0";
                }

                // 檢查分數是否接近原始數字
                if (Math.Abs(number - ((decimal)numerator / denominator)) < epsilon)
                {
                    return $"{numerator}/{denominator}";
                }
            }

            // 找不到符合條件的分數
            return "0/0";
        }

        /// <summary>
        /// 將小數轉換成,分母只有一位的字串
        /// </summary>
        /// <param name="decimalValue">0.xxx</param>
        /// <returns>string</returns>
        public static string ConvertToFractionString(decimal decimalValue)
        {
            if (decimalValue == 0)
            {
                return "0/0";
            }

            var fractions = new List<Tuple<int, int>>
            {
                Tuple.Create(1, 2),
                Tuple.Create(1, 3), Tuple.Create(2, 3),
                Tuple.Create(1, 4), Tuple.Create(3, 4),
                Tuple.Create(1, 5), Tuple.Create(2, 5), Tuple.Create(3, 5), Tuple.Create(4, 5),
                Tuple.Create(1, 6), Tuple.Create(5, 6),
                Tuple.Create(1, 7), Tuple.Create(2, 7), Tuple.Create(3, 7), Tuple.Create(4, 7), Tuple.Create(5, 7), Tuple.Create(6, 7),
                Tuple.Create(1, 8), Tuple.Create(3, 8), Tuple.Create(5, 8), Tuple.Create(7, 8),
                Tuple.Create(1, 9), Tuple.Create(2, 9), Tuple.Create(4, 9), Tuple.Create(5, 9), Tuple.Create(7, 9), Tuple.Create(8, 9),
            };

            // 將分數轉換成小數並排序
            var sortedFractions = fractions
                .OrderBy(f => (double)f.Item1 / f.Item2)
                .ToList();

            foreach (var fraction in sortedFractions)
            {
                decimal fractionValue = (decimal)fraction.Item1 / fraction.Item2;
                if (decimalValue <= fractionValue)
                {
                    return $"{fraction.Item1}/{fraction.Item2}";
                }
            }

            // 若超過8/9，仍返回8/9
            return "8/9";
        }
    }
}

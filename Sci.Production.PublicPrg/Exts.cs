﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Drawing;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

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
    }
}

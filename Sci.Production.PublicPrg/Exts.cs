using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

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

        /// <inheritdoc/>
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
    }
}

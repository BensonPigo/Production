using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Sci.Production.Prg
{
    public static class ProjExts
    {
        public static T Val<T>(this DataRow row, string name)
        {
            var v = row[name];
            if (v == DBNull.Value)
            {
                return default(T);
            }

            return (T)v;
        }

        public static T Val<T>(this DataRow row, DataColumn col)
        {
            var v = row[col];
            if (v == DBNull.Value)
            {
                return default(T);
            }

            return (T)v;
        }

        public static IDictionary<KEY, DataRow> ToDictionary<KEY>(this DataTable datas, Func<DataRow, KEY> keyselector,
            bool ignore_deleted = false)
        {
            if (keyselector == null)
            {
                new ArgumentNullException("keyselector");
            }

            if (datas == null)
            {
                return null;
            }

            var dic = new Dictionary<KEY, DataRow>();
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

        public static IDictionary<KEY, IList<DataRow>> ToDictionaryList<KEY>(this DataTable datas, Func<DataRow, KEY> keyselector,
            bool ignore_deleted = false)
        {
            if (keyselector == null)
            {
                new ArgumentNullException("keyselector");
            }

            if (datas == null)
            {
                return null;
            }

            var dic = new Dictionary<KEY, IList<DataRow>>();
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
    }
}

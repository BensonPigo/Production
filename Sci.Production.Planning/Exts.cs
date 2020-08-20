using System;
using System.Collections.Generic;
using System.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// ProjExts
    /// </summary>
    public static class ProjExts
    {
        /// <summary>
        /// Val
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="row">DataRow</param>
        /// <param name="name">string</param>
        /// <returns>v</returns>
        public static T Val<T>(this DataRow row, string name)
        {
            var v = row[name];
            if (v == DBNull.Value)
            {
                return default(T);
            }

            return (T)v;
        }

        /// <summary>
        /// Val
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="row">DataRow</param>
        /// <param name="col">DataColumn</param>
        /// <returns>v</returns>
        public static T Val<T>(this DataRow row, DataColumn col)
        {
            var v = row[col];
            if (v == DBNull.Value)
            {
                return default(T);
            }

            return (T)v;
        }

        /// <summary>
        /// ToDictionary
        /// </summary>
        /// <typeparam name="TKEY">KEY</typeparam>
        /// <param name="datas">DataTable</param>
        /// <param name="keyselector">Func</param>
        /// <param name="ignore_deleted">bool</param>
        /// <returns>dic</returns>
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

        /// <summary>
        /// ToDictionaryList
        /// </summary>
        /// <typeparam name="TKEY">KEY</typeparam>
        /// <param name="datas">DataTable</param>
        /// <param name="keyselector">Func</param>
        /// <param name="ignore_deleted">bool</param>
        /// <returns>dic</returns>
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
    }
}

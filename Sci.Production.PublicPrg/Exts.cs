﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Ict;
namespace Sci.Production.Prg
{
    internal static class ProjExts
    {
        public static T Val<T>(this DataRow row, string name)
        {
            var v = row[name];
            if (DBNull.Value == v) return default(T);
            return (T)v;
        }
        public static T Val<T>(this DataRow row, DataColumn col)
        {
            var v = row[col];
            if (DBNull.Value == v) return default(T);
            return (T)v;
        }

        public static IDictionary<KEY, DataRow> ToDictionary<KEY>(this DataTable datas, Func<DataRow, KEY> keyselector
            , bool ignore_deleted = false)
        {
            if (null == keyselector) new ArgumentNullException("keyselector");
            if (null == datas) return null;

            var dic = new Dictionary<KEY, DataRow>();
            foreach (DataRow it in datas.Rows)
            {
                if (ignore_deleted && DataRowState.Deleted == it.RowState) continue;
                var key = keyselector(it);

                dic.Add(key, it);
            }
            return dic;
        }
        public static IDictionary<KEY, IList<DataRow>> ToDictionaryList<KEY>(this DataTable datas, Func<DataRow, KEY> keyselector
            , bool ignore_deleted = false)
        {
            if (null == keyselector) new ArgumentNullException("keyselector");
            if (null == datas) return null;

            var dic = new Dictionary<KEY, IList<DataRow>>();
            foreach (DataRow it in datas.Rows)
            {
                if (ignore_deleted && DataRowState.Deleted == it.RowState) continue;
                var key = keyselector(it);

                IList<DataRow> rows;
                if (!dic.TryGetValue(key, out rows)) dic.Add(key, rows = new List<DataRow>());
                rows.Add(it);
            }
            return dic;
        }
    }
}

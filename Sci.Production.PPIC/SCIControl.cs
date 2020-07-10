using Ict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

// 請有興趣使用但是目前沒有那個領域的控制項，而想加新東西的人；
// 或是目前有那個領域東西，但是一些通用行為不支援的情況
// 找Evaon，不要自己加，感謝
namespace Sci.Production.Class
{
    /// <summary>
    /// 延伸方法
    /// </summary>
    public static class Extension
    {
        private static SqlConnection ObtainSqlConnection(Data.IDBProxy proxy)
        {
            SqlConnection cn;
            if (proxy.OpenConnection(string.Empty, out cn) == false)
            {
                throw new ApplicationException("can't open Connection");
            }

            return cn;
        }

        /// <summary>
        /// <para>透過原本的Select做查詢把結果包成DualDisposableResult(DataTable)</para>
        /// <para>使用ExtendedData取出結果資料表。會把傳入的Args兩兩一組的轉為SqlParameter，所以如果傳入的Args不是偶數會跳Exeception</para>
        /// </summary>
        /// <param name="proxy">IDBProxy</param>
        /// <param name="sql">string</param>
        /// <param name="args">object[]</param>
        /// <returns><![CDATA[DualDisposableResult<DataTable>]]></returns>
        public static DualDisposableResult<DataTable> SelectEx(this Data.IDBProxy proxy, string sql, params object[] args)
        {
            return SelectEx(proxy, sql, false, args);
        }

        /// <summary>
        /// <para>透過原本的Select做查詢把結果包成DualDisposableResult(DataTable)</para>
        /// <para>使用ExtendedData取出結果資料表。會把傳入的Args兩兩一組的轉為SqlParameter，所以如果傳入的Args不是偶數會跳Exeception</para>
        /// </summary>
        /// <param name="proxy">IDBProxy</param>
        /// <param name="sql">string</param>
        /// <param name="withSchema">bool</param>
        /// <param name="args">object[]</param>
        /// <returns><![CDATA[DualDisposableResult<DataTable>]]></returns>
        public static DualDisposableResult<DataTable> SelectEx(this Data.IDBProxy proxy, string sql, bool withSchema, params object[] args)
        {
            using (var cn = ObtainSqlConnection(proxy))
            {
                DualDisposableResult<DataTable> dr;
                var ps = new List<SqlParameter>();
                var argsClone = args.ToList();
                while (argsClone.Any())
                {
                    object v = argsClone[1];
                    ps.Add(new SqlParameter((string)argsClone[0], v == null ? DBNull.Value : v));
                    argsClone = argsClone.Skip(2).ToList();
                }

                using (var adapter = new SqlDataAdapter(sql, cn))
                {
                    adapter.SelectCommand.Parameters.AddRange(ps.ToArray());

                    var dt = new DataTable();
                    try
                    {
                        adapter.Fill(dt);
                        if (withSchema)
                        {
                            adapter.FillSchema(dt, SchemaType.Source);
                        }

                        dr = new DualDisposableResult<DataTable>(new DualResult(true));
                        dr.ExtendedData = dt;
                    }
                    catch (Exception ex)
                    {
                        var mixEx = new AggregateException(ex.Message + "\r\nsql: " + sql, ex);
                        dr = new DualDisposableResult<DataTable>(new DualResult(false, mixEx));
                    }
                }

                return dr;
            }
        }

        /// <summary>
        /// 合併DateTime.ToString()的功能
        /// </summary>
        /// <param name="date">DateTime?</param>
        /// <param name="format">string</param>
        /// <returns>re string</returns>
        public static string ToStringEx(this DateTime? date, string format)
        {
            if (date.HasValue)
            {
                return date.Value.ToString(format);
            }
            else
            {
                return null;
            }
        }
    }
}
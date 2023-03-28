using Sci.Data;
using Sci.Production.Class.Command;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// DBWarpperExtension
    /// </summary>
    public static class DBWarpperExtension
    {
        /// <summary>
        /// ExecuteInfo
        /// </summary>
        public class ExecuteInfo : IDisposable
        {
            /// <inheritdoc/>
            public string Sql { get; set; }

            /// <inheritdoc/>
            public string ConnectionName { get; set; }

            /// <inheritdoc/>
            public int Timeout { get; set; }

            /// <inheritdoc/>
            public ExecuteInfo(string sql)
            {
                this.Sql = sql;
                this.Timeout = 0;
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                this.Sql = null;
                this.ConnectionName = null;
                this.Timeout = default(int);
            }
        }

        /// <inheritdoc/>
        public class SelectExInfo : ExecuteInfo
        {
            /// <inheritdoc/>
            public bool WithSchema { get; set; }

            /// <inheritdoc/>
            public SelectExInfo(string sql)
                : base(sql)
            {
                this.WithSchema = false;
            }
        }

        /// <summary>
        /// 用來給SelectEx, ExecuteEx, LookupEx使用的參數物件，StyleCop套用後，會有參數不能兩兩一行的警告，所以改用此物件傳遞參數及其值
        /// </summary>
        public class ParameterPair
        {
            /// <summary>
            /// 參數名稱
            /// </summary>
            public string ParameterName { get; set; }

            /// <summary>
            /// 參數值
            /// </summary>
            public object ParameterValue { get; set; }

            /// <summary>
            /// 用來給SelectEx, ExecuteEx, LookupEx使用的參數物件，StyleCop套用後，會有參數不能兩兩一行的警告，所以改用此物件傳遞參數及其值
            /// </summary>
            /// <param name="parameterName">參數名稱</param>
            /// <param name="parameterValue">參數值</param>
            public ParameterPair(string parameterName, object parameterValue)
            {
                this.ParameterName = parameterName;
                this.ParameterValue = parameterValue;
            }

            /// <summary>
            /// 隱含轉換為ParameterPair物件
            /// </summary>
            /// <param name="args">{ ParameterName, ParameterValue }</param>
            public static implicit operator ParameterPair(object[] args)
            {
                if (args.Length != 2)
                {
                    throw new NotImplementedException("args must length = 2 (ParameterName, ParameterValue)");
                }

                return new ParameterPair((string)args.First(), args.Last());
            }
        }

        /// <summary>
        /// Lookup
        /// </summary>
        /// <typeparam name="TReturnValue">TReturnValue</typeparam>
        /// <param name="lookupRow">lookupRow</param>
        /// <param name="returnFieldName">returnFieldName</param>
        /// <returns>TReturnValues</returns>
        public static TReturnValue Lookup<TReturnValue>(this DataRow lookupRow, string returnFieldName)
        {
            if (lookupRow == null)
            {
                return default(TReturnValue);
            }

            if (string.IsNullOrEmpty(lookupRow.Table.TableName))
            {
                throw new ArgumentException("CachedDataSource don't have tablename");
            }

            if (lookupRow.Table.PrimaryKey == null || lookupRow.Table.PrimaryKey.Length == 0)
            {
                throw new ArgumentException("CachedDataSource don't have primarykey setting");
            }

            var tableName = lookupRow.Table.TableName;
            var idFieldsName = lookupRow.Table.PrimaryKey.Select(col => col.ColumnName).ToArray();
            var parameters = new List<System.Data.SqlClient.SqlParameter>();
            var sql = "Select " + returnFieldName + " from " + tableName + " Where " +
                string.Join(" and ", idFieldsName.Select((pkFieldName, idx) =>
                {
                    parameters.Add(new System.Data.SqlClient.SqlParameter("p" + idx.ToString(), lookupRow[pkFieldName]));
                    return pkFieldName + " = @p" + idx.ToString();
                }).ToArray());

            object scalar;
            if (SQL.ExecuteScalar(string.Empty, sql, out scalar, parameters) == true)
            {
                return (TReturnValue)scalar;
            }
            else
            {
                return default(TReturnValue);
            }
        }

        private static SqlConnection ObtainSqlConnection(IDBProxy proxy, string connectionName = "")
        {
            SqlConnection cn;
            if (proxy.OpenConnection(connectionName, out cn) == false)
            {
                throw new ApplicationException("can't open Connection");
            }

            return cn;
        }

        /// <inheritdoc/>
        public static DualDisposableResult<DataTable> SelectEx(this IDBProxy proxy, string sql, params object[] args)
        {
            return SelectEx<DataTable>(proxy, sql, false, args);
        }

        /// <inheritdoc/>
        public static DualDisposableResult<T> SelectEx<T>(this IDBProxy proxy, string sql, bool withSchema, params object[] args)
            where T : class, IDisposable, new()
        {
            using (var cn = ObtainSqlConnection(proxy))
            {
                var ps = new List<SqlParameter>();
                var argsClone = (args ?? Enumerable.Empty<object>()).ToList();
                while (argsClone.Any())
                {
                    if (argsClone[0] is ParameterPair)
                    {
                        var p = (ParameterPair)argsClone[0];
                        ps.Add(new SqlParameter(p.ParameterName, p.ParameterValue ?? DBNull.Value));
                        argsClone = argsClone.Skip(1).ToList();
                    }
                    else
                    {
                        object v = argsClone[1];
                        ps.Add(new SqlParameter((string)argsClone[0], v == null ? DBNull.Value : v));
                        argsClone = argsClone.Skip(2).ToList();
                    }
                }

                using (var adapter = new SqlDataAdapter(sql, cn))
                {
                    adapter.SelectCommand.Parameters.AddRange(ps.ToArray());

                    if (typeof(T) == typeof(DataTable))
                    {
                        var dt = new DataTable();
                        try
                        {
                            adapter.Fill(dt);
                            if (withSchema)
                            {
                                adapter.FillSchema(dt, SchemaType.Source);
                            }

                            return (dynamic)new DualDisposableResult<DataTable>(new DualResult(true))
                            {
                                ExtendedData = dt,
                            };
                        }
                        catch (Exception ex)
                        {
                            var mixEx = new AggregateException(ex.Message + "\r\nsql: " + sql, ex);
                            return (dynamic)new DualDisposableResult<DataTable>(new DualResult(false, mixEx));
                        }
                    }
                    else if (typeof(T) == typeof(DataSet))
                    {
                        var ds = new DataSet();
                        try
                        {
                            adapter.Fill(ds);
                            if (withSchema)
                            {
                                adapter.FillSchema(ds, SchemaType.Source);
                            }

                            return (dynamic)new DualDisposableResult<DataSet>(new DualResult(true))
                            {
                                ExtendedData = ds,
                            };
                        }
                        catch (Exception ex)
                        {
                            return (dynamic)new DualDisposableResult<DataSet>(new DualResult(false, ex.Message + "\r\nsql: " + sql, ex));
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        /// <inheritdoc/>
        public static DualDisposableResult<T> SelectEx<T>(this IDBProxy proxy, SelectExInfo info, params object[] args)
            where T : class, IDisposable
        {
            using (var cn = ObtainSqlConnection(proxy, info.ConnectionName))
            {
                var ps = new List<SqlParameter>();
                var argsClone = (args ?? Enumerable.Empty<object>()).ToList();
                while (argsClone.Any())
                {
                    object v = argsClone[1];
                    ps.Add(new SqlParameter((string)argsClone[0], v == null ? DBNull.Value : v));
                    argsClone = argsClone.Skip(2).ToList();
                }

                using (var adapter = new SqlDataAdapter(info.Sql, cn))
                {
                    if (info.Timeout > 0)
                    {
                        adapter.SelectCommand.CommandTimeout = info.Timeout;
                    }

                    adapter.SelectCommand.Parameters.AddRange(ps.ToArray());

                    if (typeof(T) == typeof(DataTable))
                    {
                        var dt = new DataTable();
                        try
                        {
                            adapter.Fill(dt);
                            if (info.WithSchema)
                            {
                                adapter.FillSchema(dt, SchemaType.Source);
                            }

                            return (dynamic)new DualDisposableResult<DataTable>(new DualResult(true))
                            {
                                ExtendedData = dt,
                            };
                        }
                        catch (Exception ex)
                        {
                            var mixEx = new AggregateException(ex.Message + "\r\nsql: " + info.Sql, ex);
                            return (dynamic)new DualDisposableResult<DataTable>(new DualResult(false, mixEx));
                        }
                    }
                    else if (typeof(T) == typeof(DataSet))
                    {
                        var ds = new DataSet();
                        try
                        {
                            adapter.Fill(ds);
                            if (info.WithSchema)
                            {
                                adapter.FillSchema(ds, SchemaType.Source);
                            }

                            return (dynamic)new DualDisposableResult<DataSet>(new DualResult(true))
                            {
                                ExtendedData = ds,
                            };
                        }
                        catch (Exception ex)
                        {
                            return (dynamic)new DualDisposableResult<DataSet>(new DualResult(false, ex.Message + "\r\nsql: " + info.Sql, ex));
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        /// <inheritdoc/>
        public static DualDisposableResult<T> SelectEx<T>(this IDBProxy proxy, SelectExInfo info, IList<SqlParameter> ps)
            where T : class, IDisposable
        {
            using (var cn = ObtainSqlConnection(proxy, info.ConnectionName))
            {
                using (var adapter = new SqlDataAdapter(info.Sql, cn))
                {
                    if (info.Timeout > 0)
                    {
                        adapter.SelectCommand.CommandTimeout = info.Timeout;
                    }

                    adapter.SelectCommand.Parameters.AddRange(ps.ToArray());

                    if (typeof(T) == typeof(DataTable))
                    {
                        var dt = new DataTable();
                        try
                        {
                            adapter.Fill(dt);
                            if (info.WithSchema)
                            {
                                adapter.FillSchema(dt, SchemaType.Source);
                            }

                            return (dynamic)new DualDisposableResult<DataTable>(new DualResult(true))
                            {
                                ExtendedData = dt,
                            };
                        }
                        catch (Exception ex)
                        {
                            var mixEx = new AggregateException(ex.Message + "\r\nsql: " + info.Sql, ex);
                            return (dynamic)new DualDisposableResult<DataTable>(new DualResult(false, mixEx));
                        }
                    }
                    else if (typeof(T) == typeof(DataSet))
                    {
                        var ds = new DataSet();
                        try
                        {
                            adapter.Fill(ds);
                            if (info.WithSchema)
                            {
                                adapter.FillSchema(ds, SchemaType.Source);
                            }

                            return (dynamic)new DualDisposableResult<DataSet>(new DualResult(true))
                            {
                                ExtendedData = ds,
                            };
                        }
                        catch (Exception ex)
                        {
                            return (dynamic)new DualDisposableResult<DataSet>(new DualResult(false, ex.Message + "\r\nsql: " + info.Sql, ex));
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        /// <inheritdoc/>
        public static DualResult<T> LookupEx<T>(this IDBProxy proxy, string sql, params object[] args)
        {
            return LookupEx<T>(proxy, new ExecuteInfo(sql), args);
        }

        /// <inheritdoc/>
        public static DualResult<T> LookupEx<T>(this IDBProxy proxy, ExecuteInfo info, params object[] args)
        {
            DualResult<T> dr;
            DataTable dt;
            var ps = new List<SqlParameter>();
            var argsClone = (args ?? Enumerable.Empty<object>()).ToList();
            while (argsClone.Any())
            {
                if (argsClone[0] is ParameterPair)
                {
                    var p = (ParameterPair)argsClone[0];
                    ps.Add(new SqlParameter(p.ParameterName, p.ParameterValue ?? DBNull.Value));
                    argsClone = argsClone.Skip(1).ToList();
                }
                else
                {
                    object v = argsClone[1];
                    ps.Add(new SqlParameter((string)argsClone[0], v == null ? DBNull.Value : v));
                    argsClone = argsClone.Skip(2).ToList();
                }
            }

            dr = new DualResult<T>(proxy.Select(info.ConnectionName, info.Sql, ps, out dt));
            if (dr == true)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0].ItemArray.Length > 0)
                    {
                        var value = dt.Rows[0][0];
                        if (value is T)
                        {
                            dr.ExtendedData = (T)value;
                        }
                        else
                        {
                            dr.ExtendedData = default(T);
                        }
                    }
                }
            }

            return dr;
        }

        /// <inheritdoc/>
        public static DualResult<T> Lookup<T>(this IDBProxy proxy, string sql, params ParameterPair[] args)
        {
            return LookupEx<T>(proxy, sql, (args ?? Enumerable.Empty<ParameterPair>()).SelectMany(arg => new object[] { arg.ParameterName, arg.ParameterValue }).ToArray());
        }

        /// <inheritdoc/>
        public static DualResult ExecuteEx(this IDBProxy proxy, string sql, params object[] args)
        {
            return ExecuteEx(
                proxy,
                new ExecuteInfo(sql),
                args);
        }

        /// <inheritdoc/>
        public static DualResult ExecuteEx(this IDBProxy proxy, ExecuteInfo info, params object[] args)
        {
            var argsClone = (args ?? Enumerable.Empty<object>()).ToList();

            using (var cn = ObtainSqlConnection(proxy, info.ConnectionName))
            using (var cm = cn.CreateCommand())
            {
                if (cn.State != ConnectionState.Open)
                {
                    cn.Open();
                }

                cm.CommandText = info.Sql;
                cm.CommandTimeout = info.Timeout;
                while (argsClone.Any())
                {
                    if (argsClone[0] is ParameterPair)
                    {
                        var p = (ParameterPair)argsClone[0];
                        cm.Parameters.Add(new SqlParameter(p.ParameterName, p.ParameterValue ?? DBNull.Value));
                        argsClone = argsClone.Skip(1).ToList();
                    }
                    else
                    {
                        object v = argsClone[1];
                        cm.Parameters.Add(new SqlParameter((string)argsClone[0], v == null ? DBNull.Value : v));
                        argsClone = argsClone.Skip(2).ToList();
                    }
                }

                try
                {
                    cm.ExecuteNonQuery();
                    return Result.True;
                }
                catch (Exception ex)
                {
                    return Result.F(ex);
                }
            }
        }

        /// <summary>
        /// 以指定sql語法進行select，回傳查找到的第一筆資料列(假如有找到)，否則ExtendedData就會是null
        /// </summary>
        /// <returns>是否成功與資料庫溝通，不是有沒有查找到！用result.ExtendedData != null來判斷是否有查找到</returns>
        /// <inheritdoc/>
        public static DualResult<DataRow> SeekEx(this IDBProxy proxy, string sql, params object[] args)
        {
            var p = args ?? Enumerable.Empty<object>();
            if (p.All(a => a is ParameterPair))
            {
                return proxy.SeekEx(sql, p.Select(a => (ParameterPair)a).ToArray());
            }
            else if (p.Any(a => a is ParameterPair))
            {
                throw new NotImplementedException("Must all of args are type of ParameterPair, or none of args is type ParameterPair");
            }
            else
            {
                var ps = new List<ParameterPair>();
                var argsClone = p.ToList();
                while (argsClone.Any())
                {
                    if ((argsClone[0] is string) == false)
                    {
                        throw new NotImplementedException("args 的奇數位置必須為字串(參數名稱)");
                    }
                    else if (argsClone.Count == 1)
                    {
                        throw new NotImplementedException("args 的總數必須為雙數，因為Name與Value 兩兩一組，除非使用DBWarpperExtension.ParameterPair物件作為參數傳入");
                    }

                    string field = (string)argsClone[0];
                    object value = argsClone[1];
                    ps.Add(new ParameterPair(field, value));
                    argsClone = argsClone.Skip(2).ToList();
                }

                return proxy.SeekEx(sql, ps.ToArray());
            }
        }

        /// <summary>
        /// 以指定sql語法進行select，回傳查找到的第一筆資料列(假如有找到)，否則ExtendedData就會是null
        /// </summary>
        /// <returns>是否成功與資料庫溝通，不是有沒有查找到！用result.ExtendedData != null來判斷是否有查找到</returns>
        /// <inheritdoc/>
        public static DualResult<DataRow> SeekEx(this IDBProxy proxy, string sql, params ParameterPair[] args)
        {
            using (var dr = proxy.SelectEx(sql, args))
            {
                if (dr == false)
                {
                    return new DualResult<DataRow>(dr.InnerResult);
                }
                else
                {
                    return new DualResult<DataRow>(Result.True)
                    {
                        ExtendedData = dr.ExtendedData.AsEnumerable().FirstOrDefault()?.GetDetachCopy(),
                    };
                }
            }
        }

        /// <summary>
        /// 以指定SelectExInfo進行select，回傳查找到的第一筆資料列(假如有找到)，否則ExtendedData就會是null
        /// </summary>
        /// <returns>是否成功與資料庫溝通，不是有沒有查找到！用result.ExtendedData != null來判斷是否有查找到</returns>
        /// <inheritdoc/>
        public static DualResult<DataRow> SeekEx(this IDBProxy proxy, SelectExInfo info)
        {
            return proxy.SeekEx(info, null);
        }

        /// <summary>
        /// 以指定SelectExInfo進行select，回傳查找到的第一筆資料列(假如有找到)，否則ExtendedData就會是null
        /// </summary>
        /// <returns>是否成功與資料庫溝通，不是有沒有查找到！用result.ExtendedData != null來判斷是否有查找到</returns>
        /// <inheritdoc/>
        public static DualResult<DataRow> SeekEx(this IDBProxy proxy, SelectExInfo info, params object[] args)
        {
            var p = args ?? Enumerable.Empty<object>();
            if (p.All(a => a is ParameterPair))
            {
                return proxy.SeekEx(info, p.Select(a => (ParameterPair)a).ToArray());
            }
            else if (p.Any(a => a is ParameterPair))
            {
                throw new NotImplementedException("Must all of args are type of ParameterPair, or none of args is type ParameterPair");
            }
            else
            {
                var ps = new List<ParameterPair>();
                var argsClone = p.ToList();
                while (argsClone.Any())
                {
                    if ((argsClone[0] is string) == false)
                    {
                        throw new NotImplementedException("args 的奇數位置必須為字串(參數名稱)");
                    }
                    else if (argsClone.Count == 1)
                    {
                        throw new NotImplementedException("args 的總數必須為雙數，因為Name與Value 兩兩一組，除非使用DBWarpperExtension.ParameterPair物件作為參數傳入");
                    }

                    string field = (string)argsClone[0];
                    object value = argsClone[1];
                    ps.Add(new ParameterPair(field, value));
                    argsClone = argsClone.Skip(2).ToList();
                }

                return proxy.SeekEx(info, ps.ToArray());
            }
        }

        /// <summary>
        /// 以指定SelectExInfo進行select，回傳查找到的第一筆資料列(假如有找到)，否則ExtendedData就會是null
        /// </summary>
        /// <returns>是否成功與資料庫溝通，不是有沒有查找到！用result.ExtendedData != null來判斷是否有查找到</returns>
        /// <inheritdoc/>
        public static DualResult<DataRow> SeekEx(this IDBProxy proxy, SelectExInfo info, params ParameterPair[] args)
        {
            using (var dr = proxy.SelectEx<DataTable>(info, args))
            {
                if (dr == false)
                {
                    return new DualResult<DataRow>(dr.InnerResult);
                }
                else
                {
                    return new DualResult<DataRow>(Result.True)
                    {
                        ExtendedData = dr.ExtendedData.AsEnumerable().FirstOrDefault()?.GetDetachCopy(),
                    };
                }
            }
        }

        /// <summary>
        /// 用DualDisposableResult包住SqlConnection回傳，提供using block的能力
        /// </summary>
        /// <param name="proxy">proxy</param>
        /// <param name="connectionName">connectionName</param>
        /// <returns>DualDisposableResult of SqlConnection</returns>
        public static DualDisposableResult<SqlConnection> GetConnection(this IDBProxy proxy, string connectionName = null)
        {
            SqlConnection cn;
            return new DualDisposableResult<SqlConnection>(new DualResult(SQL.GetConnection(out cn, connectionName)))
            {
                ExtendedData = cn,
            };
        }
    }
}

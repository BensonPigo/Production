using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// CrossUtility
    /// </summary>
    public class CrossUtility
    {
        /// <summary>
        /// ProcessWithDatatable
        /// </summary>
        /// <param name="source">DataTable source</param>
        /// <param name="tmp_columns">string tmp_columns</param>
        /// <param name="sqlcmd">string sqlcmd</param>
        /// <param name="result">out DataTable result</param>
        /// <param name="temptablename">string temptablename</param>
        /// <param name="conn">conn</param>
        /// <param name="paramters">paramters</param>
        /// <param name="initTmpCommand">initTmpCommand</param>
        /// <param name="defaultConnectionName">defaultConnectionName</param>
        /// <param name="defaultTimeout">defaultTimeout</param>
        /// <param name="columnTypes">指定欄位的SQL類型，例如 {"欄位名1":"varchar(50)", "欄位名2":"varchar(100)"}</param>
        /// <returns>result</returns>
        public static DualResult ProcessWithDatatable(DataTable source, string tmp_columns, string sqlcmd, out DataTable result, string temptablename = "#tmp", SqlConnection conn = null, List<SqlParameter> paramters = null, string initTmpCommand = null, string defaultConnectionName = "", int defaultTimeout = 15 * 60, Dictionary<string, string> columnTypes = null)
        {
            DataTable[] results;
            DualResult success = ProcessWithDatatable(source, tmp_columns, sqlcmd, out results, temptablename: temptablename, conn: conn, paramters: paramters, initTmpCommand: initTmpCommand, defaultConnectionName: defaultConnectionName, defaultTimeout: defaultTimeout, columnTypes: columnTypes);

            result = (results == null || results.Length == 0)
                ? null
                : results[0];

            return success;
        }

        /// <summary>
        /// ProcessWithDatatable
        /// </summary>
        /// <param name="source">DataTable source</param>
        /// <param name="tmp_columns">string tmp_columns</param>
        /// <param name="sqlcmd">string sqlcmd</param>
        /// <param name="result">out DataTable[] result</param>
        /// <param name="temptablename">string temptablename</param>
        /// <param name="conn">conn</param>
        /// <param name="paramters">paramters</param>
        /// <param name="initTmpCommand">initTmpCommand</param>
        /// <param name="defaultConnectionName">defaultConnectionName</param>
        /// <param name="defaultTimeout">defaultTimeout</param>
        /// <param name="columnTypes">指定欄位的SQL類型，例如 {"欄位名1":"varchar(50)", "欄位名2":"varchar(100)"}</param>
        /// <returns>result</returns>
        public static DualResult ProcessWithDatatable(DataTable source, string tmp_columns, string sqlcmd, out DataTable[] result, string temptablename = "#tmp", SqlConnection conn = null, List<SqlParameter> paramters = null, string initTmpCommand = null, string defaultConnectionName = "", int defaultTimeout = 15 * 60, Dictionary<string, string> columnTypes = null)
        {
            DualResult dualResult = null;
            result = null;
            if (string.IsNullOrWhiteSpace(tmp_columns))
            {
                tmp_columns = source.GetAllFields();
            }

            string[] cols = tmp_columns.Split(',');

            if (string.IsNullOrEmpty(initTmpCommand))
            {
                StringBuilder sb = new StringBuilder();
                if (temptablename.TrimStart().StartsWith("#"))
                {
                    sb.Append(string.Format("create table {0} (", temptablename));
                }
                else
                {
                    sb.Append(string.Format("create table #{0} (", temptablename));
                }

                for (int i = 0; i < cols.Length; i++)
                {
                    string thisColName = cols[i].Trim();
                    if (MyUtility.Check.Empty(thisColName))
                    {
                        continue;
                    }

                    string formattedColName = thisColName;
                    if (!formattedColName.Contains("["))
                    {
                        formattedColName = "[" + formattedColName + "]";
                    }

                    cols[i] = formattedColName;

                    // 檢查是否有指定的欄位類型
                    if (columnTypes != null && columnTypes.ContainsKey(thisColName))
                    {
                        sb.Append(string.Format("{0} {1}", formattedColName, columnTypes[thisColName]));
                    }
                    else
                    {
                        switch (Type.GetTypeCode(source.Columns[thisColName].DataType))
                        {
                            case TypeCode.Byte:
                                sb.Append(string.Format("{0} tinyint", formattedColName));
                                break;
                            case TypeCode.Boolean:
                                sb.Append(string.Format("{0} bit", formattedColName));
                                break;

                            case TypeCode.Char:
                                sb.Append(string.Format("{0} varchar(1)", formattedColName));
                                break;

                            case TypeCode.DateTime:
                                sb.Append(string.Format("{0} datetime", formattedColName));
                                break;

                            case TypeCode.Decimal:
                            case TypeCode.Double:
                                sb.Append(string.Format("{0} numeric(24,8)", formattedColName));
                                break;

                            case TypeCode.Int16:
                                sb.Append(string.Format("{0} int", formattedColName));
                                break;

                            case TypeCode.Int32:
                                sb.Append(string.Format("{0} int", formattedColName));
                                break;

                            case TypeCode.String:
                                sb.Append(string.Format("{0} nvarchar(max)", formattedColName));
                                break;

                            case TypeCode.Int64:
                                sb.Append(string.Format("{0} bigint", formattedColName));
                                break;
                            default:
                                break;
                        }
                    }

                    if (i < cols.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.Append(")");
                initTmpCommand = sb.ToString();
            }

            bool needCloseConnection = false;
            if (conn == null)
            {
                dualResult = DBProxy.Current.OpenConnection(defaultConnectionName, out conn);
                if (!dualResult)
                {
                    return dualResult;
                }

                needCloseConnection = true;
            }

            try
            {
                DBProxy.Current.DefaultTimeout = defaultTimeout;
                dualResult = DBProxy.Current.ExecuteByConn(conn, initTmpCommand);
                if (!dualResult)
                {
                    return dualResult;
                }

                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                {
                    bulkcopy.BulkCopyTimeout = defaultTimeout;
                    if (temptablename.TrimStart().StartsWith("#"))
                    {
                        bulkcopy.DestinationTableName = temptablename.Trim();
                    }
                    else
                    {
                        bulkcopy.DestinationTableName = string.Format("#{0}", temptablename.Trim());
                    }

                    for (int i = 0; i < cols.Length; i++)
                    {
                        bulkcopy.ColumnMappings.Add(cols[i], cols[i]);
                    }

                    bulkcopy.WriteToServer(source);
                    bulkcopy.Close();
                }

                dualResult = DBProxy.Current.SelectByConn(conn, sqlcmd, paramters, out result);
                if (!dualResult)
                {
                    return dualResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (needCloseConnection)
                {
                    conn.Close();
                }
            }

            return dualResult;
        }
    }
}

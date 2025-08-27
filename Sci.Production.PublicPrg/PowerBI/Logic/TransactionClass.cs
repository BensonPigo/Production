using Ict;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Transactions;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class TransactionClass
    {
        /// <summary>
        /// process with datatable with transaction scope and return datatable
        /// </summary>
        /// <param name="source">source datatable</param>
        /// <param name="tmp_columns">temp table columns</param>
        /// <param name="sqlcmd">Sql cmd</param>
        /// <param name="result">return dataTable result</param>
        /// <param name="temptablename">temp table name</param>
        /// <param name="conn">Sql Connection</param>
        /// <param name="paramters">Sql Parameter</param>
        /// <param name="initTmpCommand">init Tmp Command</param>
        /// <param name="defaultTimeoutInSeconds">Default Timeout In Seconds</param>
        /// <param name="columnTypes">指定欄位的SQL類型，例如 {"欄位名1":"varchar(50)", "欄位名2":"varchar(100)"}</param>
        /// <returns>DualResult</returns>
        public static DualResult ProcessWithDatatableWithTransactionScope(DataTable source, string tmp_columns, string sqlcmd, out DataTable result, string temptablename = "#tmp", SqlConnection conn = null, List<SqlParameter> paramters = null, string initTmpCommand = null, int defaultTimeoutInSeconds = 60 * 60, Dictionary<string, string> columnTypes = null)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                dualResult = PublicPrg.CrossUtility.ProcessWithDatatable(source, tmp_columns, sqlcmd, out result, temptablename, conn, paramters, initTmpCommand, defaultTimeout: defaultTimeoutInSeconds, columnTypes: columnTypes);

                if (!dualResult)
                {
                    transactionScope.Dispose();
                    return dualResult;
                }

                transactionScope.Complete();
                return dualResult;
            }
        }

        /// <summary>
        /// execute by connection with transaction scope and sql parameter
        /// </summary>
        /// <param name="conn">Sql Connection</param>
        /// <param name="cmdtext">Sql cmd</param>
        /// <param name="parameters">Sql Parameter</param>
        /// <param name="defaultTimeoutInSeconds">Default Timeout In Seconds</param>
        /// <returns>DualResult</returns>
        public static DualResult ExecuteByConnTransactionScope(SqlConnection conn, string cmdtext, IList<SqlParameter> parameters, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                Data.DBProxy.Current.DefaultTimeout = defaultTimeoutInSeconds;
                dualResult = Data.DBProxy.Current.ExecuteByConn(conn, cmdtext, parameters);

                if (!dualResult)
                {
                    return dualResult;
                }

                transactionScope.Complete();
                return dualResult;
            }
        }

        /// <summary>
        /// execute by connection with transaction scope
        /// </summary>
        /// <param name="conn">Sql Connection</param>
        /// <param name="cmdtext">Sql cmd</param>
        /// <param name="defaultTimeoutInSeconds">Default Timeout In Seconds</param>
        /// <returns>DualResult</returns>
        public static DualResult ExecuteByConnTransactionScope(SqlConnection conn, string cmdtext, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                Data.DBProxy.Current.DefaultTimeout = defaultTimeoutInSeconds;
                dualResult = Data.DBProxy.Current.ExecuteByConn(conn, cmdtext);

                if (!dualResult)
                {
                    return dualResult;
                }

                transactionScope.Complete();
                return dualResult;
            }
        }

        /// <summary>
        /// execute with transaction scope
        /// </summary>
        /// <param name="connname">connection name</param>
        /// <param name="cmdtext">Sql cmd</param>
        /// <param name="defaultTimeoutInSeconds">Default Timeout In Seconds</param>
        /// <returns>DualResult</returns>
        public static DualResult ExecuteTransactionScope(string connname, string cmdtext, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                Data.DBProxy.Current.DefaultTimeout = defaultTimeoutInSeconds;
                dualResult = Data.DBProxy.Current.Execute(connname, cmdtext);

                if (!dualResult)
                {
                    transactionScope.Dispose();
                    return dualResult;
                }

                transactionScope.Complete();
                transactionScope.Dispose();
                return dualResult;
            }
        }

        /// <summary>
        /// execute with transaction scope and sql parameter
        /// </summary>
        /// <param name="connname">connection name</param>
        /// <param name="cmdtext">Sql cmd</param>
        /// <param name="parameters">SqlParameter</param>
        /// <param name="defaultTimeoutInSeconds">Default Timeout In Seconds</param>
        /// <returns>DualResult</returns>
        public static DualResult ExecuteTransactionScope(string connname, string cmdtext, IList<SqlParameter> parameters, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                Data.DBProxy.Current.DefaultTimeout = defaultTimeoutInSeconds;
                dualResult = Data.DBProxy.Current.Execute(connname, cmdtext, parameters);

                if (!dualResult)
                {
                    return dualResult;
                }

                transactionScope.Complete();
                return dualResult;
            }
        }

        /// <summary>
        /// update BI Table Info data with transaction scope
        /// </summary>
        /// <param name="conn">conn</param>
        /// <param name="item">biTableInfoID</param>
        /// <param name="is_Trans">is_Trans</param>
        /// <param name="defaultTimeoutInSeconds">defaultTimeoutInSeconds</param>
        /// <returns>dualResult</returns>
        public static DualResult UpatteBIDataTransactionScope(SqlConnection conn, ExecutedList item, bool is_Trans, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                Data.DBProxy.Current.DefaultTimeout = defaultTimeoutInSeconds;
                string sql = new Base().SqlBITableInfo(item);
                dualResult = Data.DBProxy.Current.ExecuteByConn(conn, sql);

                if (!dualResult)
                {
                    transactionScope.Dispose();
                    return dualResult;
                }

                transactionScope.Complete();
                transactionScope.Dispose();
                return dualResult;
            }
        }
    }
}

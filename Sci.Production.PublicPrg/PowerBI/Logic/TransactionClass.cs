using Ict;
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
        /// <returns>DualResult</returns>
        public static DualResult ProcessWithDatatableWithTransactionScope(DataTable source, string tmp_columns, string sqlcmd, out DataTable result, string temptablename = "#tmp", SqlConnection conn = null, List<SqlParameter> paramters = null, string initTmpCommand = null, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                dualResult = PublicPrg.CrossUtility.ProcessWithDatatable(source, tmp_columns, sqlcmd, out result, temptablename, conn, paramters, initTmpCommand, defaultTimeout: defaultTimeoutInSeconds);

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
        /// process with datatable with transaction scope and return multiple datatable
        /// </summary>
        /// <param name="source">source datatable</param>
        /// <param name="tmp_columns">temp table columns</param>
        /// <param name="sqlcmd">Sql cmd</param>
        /// <param name="result">return dataTable array result</param>
        /// <param name="temptablename">temp table name</param>
        /// <param name="conn">Sql Connection</param>
        /// <param name="paramters">Sql Parameter</param>
        /// <param name="initTmpCommand">init Tmp Command</param>
        /// <param name="defaultTimeoutInSeconds">Default Timeout In Seconds</param>
        /// <returns>DualResult</returns>
        public static DualResult ProcessWithDatatableWithTransactionScope(DataTable source, string tmp_columns, string sqlcmd, out DataTable[] result, string temptablename = "#tmp", SqlConnection conn = null, List<SqlParameter> paramters = null, string initTmpCommand = null, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                dualResult = PublicPrg.CrossUtility.ProcessWithDatatable(source, tmp_columns, sqlcmd, out result, temptablename, conn, paramters, initTmpCommand, defaultTimeout: defaultTimeoutInSeconds);

                if (!dualResult)
                {
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
                    return dualResult;
                }

                transactionScope.Complete();
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
    }
}

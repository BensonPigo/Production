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
        /// <inheritdoc/>
        public static DualResult ProcessWithDatatableWithTransactionScope(DataTable source, string tmp_columns, string sqlcmd, out DataTable result, string temptablename = "#tmp", SqlConnection conn = null, List<SqlParameter> paramters = null, string initTmpCommand = null, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                dualResult = PublicPrg.CrossUtility.ProcessWithDatatable(source, tmp_columns, sqlcmd, out result, temptablename, conn, paramters, initTmpCommand, defaultTimeout: defaultTimeoutInSeconds);

                if (!dualResult)
                {
                    // transactionScope.Dispose();
                    return dualResult;
                }

                transactionScope.Complete();
                return dualResult;
            }
        }

        /// <inheritdoc/>
        public static DualResult ProcessWithDatatableWithTransactionScope(DataTable source, string tmp_columns, string sqlcmd, out DataTable[] result, string temptablename = "#tmp", SqlConnection conn = null, List<SqlParameter> paramters = null, string initTmpCommand = null, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                dualResult = PublicPrg.CrossUtility.ProcessWithDatatable(source, tmp_columns, sqlcmd, out result, temptablename, conn, paramters, initTmpCommand, defaultTimeout: defaultTimeoutInSeconds);

                if (!dualResult)
                {
                    // transactionScope.Dispose();
                    return dualResult;
                }

                transactionScope.Complete();
                return dualResult;
            }
        }

        /// <inheritdoc/>
        public static DualResult ExecuteByConnTransactionScope(SqlConnection conn, string cmdtext, IList<SqlParameter> parameters, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                dualResult = Data.DBProxy.Current.ExecuteByConn(conn, cmdtext, parameters);

                if (!dualResult)
                {
                    // transactionScope.Dispose();
                    return dualResult;
                }

                transactionScope.Complete();
                return dualResult;
            }
        }

        /// <inheritdoc/>
        public static DualResult ExecuteByConnTransactionScope(SqlConnection conn, string cmdtext, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                dualResult = Data.DBProxy.Current.ExecuteByConn(conn, cmdtext);

                if (!dualResult)
                {
                    // transactionScope.Dispose();
                    return dualResult;
                }

                transactionScope.Complete();
                return dualResult;
            }
        }

        /// <inheritdoc/>
        public static DualResult ExecuteTransactionScope(string connname, string cmdtext, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                dualResult = Data.DBProxy.Current.Execute(connname, cmdtext);

                if (!dualResult)
                {
                    // transactionScope.Dispose();
                    return dualResult;
                }

                transactionScope.Complete();
                return dualResult;
            }
        }

        /// <inheritdoc/>
        public static DualResult ExecuteTransactionScope(string connname, string cmdtext, IList<SqlParameter> parameters, int defaultTimeoutInSeconds = 60 * 60)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                dualResult = Data.DBProxy.Current.Execute(connname, cmdtext, parameters);

                if (!dualResult)
                {
                    // transactionScope.Dispose();
                    return dualResult;
                }

                transactionScope.Complete();
                return dualResult;
            }
        }
    }
}

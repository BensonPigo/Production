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
        public static DualResult ProcessWithDatatableWithTransactionScope(DataTable source, string tmp_columns, string sqlcmd, out DataTable result, string temptablename = "#tmp", SqlConnection conn = null, List<SqlParameter> paramters = null, string initTmpCommand = null)
        {
            DualResult dualResult;
            var transactionOptions = new TransactionOptions
            {
                Timeout = TimeSpan.FromSeconds(1800), // 設置超時時間為 1800 秒
            };
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                dualResult = MyUtility.Tool.ProcessWithDatatable(source, tmp_columns, sqlcmd, out result, temptablename, conn, paramters, initTmpCommand);

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
        public static DualResult ProcessWithDatatableWithTransactionScope(DataTable source, string tmp_columns, string sqlcmd, out DataTable[] result, string temptablename = "#tmp", SqlConnection conn = null, List<SqlParameter> paramters = null, string initTmpCommand = null)
        {
            DualResult dualResult;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                dualResult = MyUtility.Tool.ProcessWithDatatable(source, tmp_columns, sqlcmd, out result, temptablename, conn, paramters, initTmpCommand);

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
        public static DualResult ExecuteByConnTransactionScope(SqlConnection conn, string cmdtext, IList<SqlParameter> parameters)
        {
            DualResult dualResult;
            var transactionOptions = new TransactionOptions
            {
                Timeout = TimeSpan.FromSeconds(1800), // 設置超時時間為 1800 秒
            };
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
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
        public static DualResult ExecuteByConnTransactionScope(SqlConnection conn, string cmdtext)
        {
            DualResult dualResult;
            var transactionOptions = new TransactionOptions
            {
                Timeout = TimeSpan.FromSeconds(1800), // 設置超時時間為 1800 秒
            };
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
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
        public static DualResult ExecuteTransactionScope(string connname, string cmdtext)
        {
            DualResult dualResult;
            var transactionOptions = new TransactionOptions
            {
                Timeout = TimeSpan.FromSeconds(1800), // 設置超時時間為 1800 秒
            };
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
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
        public static DualResult ExecuteTransactionScope(string connname, string cmdtext, IList<SqlParameter> parameters)
        {
            DualResult dualResult;
            var transactionOptions = new TransactionOptions
            {
                Timeout = TimeSpan.FromSeconds(1800), // 設置超時時間為 1800 秒
            };
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
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

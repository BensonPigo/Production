using Ict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg
{
    /// <summary>
    /// IDBProxyPMS
    /// </summary>
    public abstract class AbstractDBProxyPMS
    {
        /// <summary>
        /// Select
        /// </summary>
        /// <param name="connname">connname</param>
        /// <param name="sqlCmd">sqlCmd</param>
        /// <param name="sqlPars">sqlPars</param>
        /// <param name="dtResult">dtResult</param>
        /// <returns>DualResult</returns>
        public abstract DualResult Select(string connname, string sqlCmd, IList<SqlParameter> sqlPars, out DataTable dtResult);

        /// <summary>
        /// ProcessWithDatatable
        /// </summary>
        /// <param name="conn">conn</param>
        /// <param name="source">source</param>
        /// <param name="tmp_columns">tmp_columns</param>
        /// <param name="sqlcmd">sqlcmd</param>
        /// <param name="result">result</param>
        /// <returns>DualResult</returns>
        public abstract DualResult ProcessWithDatatable(string conn, DataTable source, string tmp_columns, string sqlcmd, out DataTable[] result);

        /// <summary>
        /// Select
        /// </summary>
        /// <param name="connname">connname</param>
        /// <param name="sqlCmd">sqlCmd</param>
        /// <param name="dtResult">dtResult</param>
        /// <returns>DualResult</returns>
        public DualResult Select(string connname, string sqlCmd, out DataTable dtResult)
        {
            return this.Select(connname, sqlCmd, new List<SqlParameter>(), out dtResult);
        }

        /// <summary>
        /// Seek
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="connName">connName</param>
        /// <returns>bool</returns>
        public bool Seek(string sql, string connName)
        {
            DualResult result = this.Select(connName, sql, null, out DataTable dtResult);

            if (!result)
            {
                throw result.GetException();
            }

            return dtResult.Rows.Count > 0;
        }

        /// <summary>
        /// Lookup
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="connName">connName</param>
        /// <param name="listPar">listPar</param>
        /// <returns>string</returns>
        public string Lookup(string sql, string connName, IList<SqlParameter> listPar = null)
        {
            DualResult result = this.Select(connName, sql, listPar, out DataTable dtResult);

            if (!result)
            {
                throw result.GetException();
            }

            return dtResult.Rows.Count > 0 ? dtResult.Rows[0][0].ToString() : string.Empty;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="connName">connName</param>
        /// <param name="listPar">listPar</param>
        /// <returns>DualResult</returns>
        public DualResult Execute(string sql, string connName, IList<SqlParameter> listPar = null)
        {
            return this.Select(connName, sql, listPar, out DataTable dtResult);
        }

        /// <summary>
        /// ProcessWithDatatable
        /// </summary>
        /// <param name="conn">conn</param>
        /// <param name="source">source</param>
        /// <param name="tmp_columns">tmp_columns</param>
        /// <param name="sqlcmd">sqlcmd</param>
        /// <param name="dtResult">dtResult</param>
        /// <returns>DualResult</returns>
        public DualResult ProcessWithDatatable(string conn, DataTable source, string tmp_columns, string sqlcmd, out DataTable dtResult)
        {
            DataTable[] dts;
            DualResult result = this.ProcessWithDatatable(conn, source, tmp_columns, sqlcmd, out dts);

            if (!result)
            {
                dtResult = new DataTable();
                return result;
            }

            if (dts.Length == 0)
            {
                dtResult = new DataTable();
            }
            else
            {
                dtResult = dts[0];
            }

            return new DualResult(true);
        }
    }
}

using Ict;
using Sci.Data;
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
    /// DBProxyPMS
    /// </summary>
    public class DBProxyPMS : AbstractDBProxyPMS
    {
        /// <inheritdoc/>
        public override DualResult ProcessWithDatatable(string conn, DataTable source, string tmp_columns, string sqlcmd, out DataTable[] result)
        {
            return MyUtility.Tool.ProcessWithDatatable(source, tmp_columns, sqlcmd, out result);
        }

        /// <inheritdoc/>
        public override DualResult Select(string connname, string sqlCmd, IList<SqlParameter> sqlPars, out DataTable dtResult)
        {
            return DBProxy.Current.Select(connname, sqlCmd, sqlPars, out dtResult);
        }
    }
}

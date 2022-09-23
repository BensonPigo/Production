using Ict;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// P11_SewingOutputOrderChange
    /// </summary>
    internal class P11_SewingOutputOrderChange : SewingOutputOrderChange
    {
        /// <summary>
        /// P11_SewingOutputOrderChange
        /// </summary>
        /// <param name="currentMaintain">currentMaintain</param>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="userID">userID</param>
        /// <param name="isErrorReturnImmediate">isErrorReturnImmediate</param>
        public P11_SewingOutputOrderChange(DataRow currentMaintain, DataTable dtDetail, string userID, bool isErrorReturnImmediate = true) : base(currentMaintain, dtDetail, userID, isErrorReturnImmediate)
        {
        }

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

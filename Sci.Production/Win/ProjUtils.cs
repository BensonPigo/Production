using System;
using System.Data.SqlClient;
using Ict;
using Sci.Data;

namespace Sci.Production.Win
{
    /// <summary>
    /// ProjUtils
    /// </summary>
    public static class ProjUtils
    {
        /// <summary>
        /// Get Pass
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="password">PWD</param>
        /// <param name="data">Represents strongly named DataRow class</param>
        /// <returns>DualResult</returns>
        public static DualResult GetPass1(string account, string password, out SCHEMAS.PASS1Row data)
        {
            data = null;
            DualResult result;

            try
            {
                SqlConnection conn;
                if (!(result = DBProxy.Current.OpenConnection(null, out conn)))
                {
                    return result;
                }

                using (conn)
                {
                    using (var adapter = new SCHEMASTableAdapters.PASS1TableAdapter())
                    {
                        adapter.Connection = conn;

                        var datas = adapter.Login(account, password);
                        if (datas.Count > 0)
                        {
                            data = datas[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, "Load 'Pass1' data error.", ex);
            }

            return Ict.Result.True;
        }
    }
}

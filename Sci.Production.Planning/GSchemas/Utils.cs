using System;
using System.Data.SqlClient;

using Ict;
using Sci.Data;

namespace Sci.Production.Report.GSchemas
{
    /// <summary>
    /// Utils
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// ConnectionName
        /// </summary>
        public static string ConnectionName { get; set; }

        /// <summary>
        /// GetDropDownList
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="helper">helper</param>
        /// <returns>DualResult</returns>
        public static DualResult GetDropDownList(string type, out DropDownListHelper helper)
        {
            helper = null;
            DualResult result;

            GLO.DropDownListDataTable datas;
            if (!(result = GetDropDownList(type, out datas)))
            {
                return result;
            }

            helper = new DropDownListHelper(datas);
            return Ict.Result.True;
        }

        /// <summary>
        /// GetDropDownList
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="datas">datas</param>
        /// <returns>DualResult</returns>
        public static DualResult GetDropDownList(string type, out GLO.DropDownListDataTable datas)
        {
            datas = null;
            DualResult result;

            try
            {
                SqlConnection conn = null;
                if (!(result = DBProxy.Current.OpenConnection(ConnectionName, out conn)))
                {
                    return result;
                }

                using (conn)
                {
                    using (var adapter = new GLOTableAdapters.DropDownListTableAdapter())
                    {
                        adapter.Connection = conn;

                        datas = adapter.GetsByType(type);
                    }
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, "Get Ap Qty value error.", ex);
            }

            return Ict.Result.True;
        }
    }
}

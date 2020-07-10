using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_Utility
    /// </summary>
    internal class P01_Utility
    {
        /// <summary>
        /// CheckOrderCategoryTypeA
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <returns>DualResult</returns>
        public static DualResult CheckOrderCategoryTypeA(string orderID)
        {
            string checkSql = $@"
select ID
from Orders with (nolock)
where AllowanceComboID = @orderID and Finished = 0
";
            DataTable dtNotFinishOrders;

            DualResult result = DBProxy.Current.Select(null, checkSql, new List<SqlParameter>() { new SqlParameter("@orderID", orderID) }, out dtNotFinishOrders);

            if (!result)
            {
                return result;
            }

            if (dtNotFinishOrders.Rows.Count > 0)
            {
                string listNotFinishOrders = dtNotFinishOrders.AsEnumerable().Select(s => s["ID"].ToString()).JoinToString(",");
                string errMsg = $@"This <SP#>{orderID} can not do finished, because below order still not finished.{Environment.NewLine}{listNotFinishOrders}";
                return new DualResult(false, errMsg);
            }

            return new DualResult(true);
        }
    }
}

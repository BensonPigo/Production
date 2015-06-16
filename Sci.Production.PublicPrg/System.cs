using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sci.Data;
using Sci;
using Ict;
using Ict.Win;

namespace Sci.Production.PublicPrg
{
    
    public static partial class Prgs
    {
        #region GetAuthority
        /// <summary>
        /// GetAuthority()
        /// </summary>
        /// <param name="strLogin"></param>
        /// <returns>bool</returns>
        public static bool GetAuthority(string login)
        {
            return true;
        }
        #endregion

        #region GetCartonList
        /// <summary>
        /// GetCartonList(string)
        /// </summary>
        /// <param name="strLogin"></param>
        /// <returns>bool</returns>
        public static bool GetCartonList(string orderID)
        {
            string sqlCmd;

            sqlCmd =string.Format(@"update Orders 
                                                       set TotalCTN = (select sum(b.CTNQty) from PackingList a, PackingList_Detail b where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}'), 
                                                             FtyCTN = (select sum(b.CTNQty) from PackingList a, PackingList_Detail b where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and TransferToClogID != ''), 
                                                             ClogCTN = (select sum(b.CTNQty) from PackingList a, PackingList_Detail b where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and ClogReceiveID != ''), 
                                                            ClogLastReceiveDate = (select max(ReceiveDate) from PackingList a, PackingList_Detail b where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}') 
                                                       where ID = '{0}'", orderID);
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
    
}

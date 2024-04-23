using System.Collections.Generic;
using System.Data;
using Sci.Data;
using Ict;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static partial class Prgs
    {
        #region UpdateOrdersCTN

        /// <summary>
        /// UpdateOrdersCTN(string)
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <returns>bool</returns>
        public static DualResult UpdateOrdersCTN(string orderID)
        {
            string sqlCmd;

            sqlCmd = string.Format(
                @"
update Orders 
set TotalCTN = (
    select isnull(sum(b.CTNQty), 0) 
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0), 
FtyCTN = (
    select isnull(sum(b.CTNQty), 0)  
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0 and b.TransferDate is not null), 
ClogCTN = (
    select isnull(sum(b.CTNQty), 0)  
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0 and b.ReceiveDate is not null
    and b.TransferCFADate is null AND b.CFAReturnClogDate is null), 
ClogLastReceiveDate = (
    select max(ReceiveDate) 
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0), 
cfaCTN = (
    select isnull(sum(b.CTNQty), 0) 
    from PackingList a, PackingList_Detail b 
    where a.id = b.id and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0 and b.CFAReceiveDate is not null),
DRYCTN = (
    select ISNULL(sum(b.CTNQty),0)
    from PackingList a, PackingList_Detail b 
    where a.id = b.id and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0 and b.DRYReceiveDate is not null),
PackErrCTN = (
    select ISNULL(sum(b.CTNQty),0)
    from PackingList a, PackingList_Detail b 
    where a.id = b.id and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0 and b.PackErrTransferDate is not null)
,FtyToClogTransit = (
    SELECT SUM(pld.CTNQty)
    FROM PackingList pl
    INNER JOIN PackingList_Detail pld ON pld.ID = pl.ID
    WHERE pld.OrderID = '{0}' AND pld.TransferDate IS NOT NULL AND pld.ReceiveDate  IS NULL)
,ClogToCFATansit = (
    SELECT SUM(pld.CTNQty)
    FROM PackingList pl
    INNER JOIN PackingList_Detail pld ON pld.ID = pl.ID
    WHERE pld.OrderID = '{0}' AND pld.TransferCFADate IS NOT NULL AND pld.CFAReceiveDate IS NULL AND pld.ClogLocationID = '2CFA')
,CFAToClogTransit = (
    SELECT SUM(pld.CTNQty)
    FROM PackingList pl
    INNER JOIN PackingList_Detail pld ON pld.ID = pl.ID
    WHERE pld.OrderID = '{0}' AND pld.TransferCFADate IS NULL AND pld.CFAReceiveDate IS NULL AND pld.CFAReturnClogDate IS NOT NULL AND pld.ClogLocationID = '2Clog')
where ID = '{0}'",
                orderID);
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);

            return result;
        }

        /// <summary>
        /// UpdateOrdersCTN(DataTable)
        /// </summary>
        /// <param name="orderData">Order Data</param>
        /// <returns>DualResult</returns>
        public static DualResult UpdateOrdersCTN(DataTable orderData)
        {
            IList<string> updateCmds = new List<string>();
            foreach (DataRow dr in orderData.Rows)
            {
                updateCmds.Add(string.Format(
                    @"
update Orders 
set TotalCTN = (
    select isnull(sum(b.CTNQty), 0)
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0), 
FtyCTN = (
    select isnull(sum(b.CTNQty), 0) 
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0 and b.TransferDate is not null), 
ClogCTN = (
    select isnull(sum(b.CTNQty), 0) 
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0 and b.ReceiveDate is not null
    and b.TransferCFADate is null AND b.CFAReturnClogDate is null), 
ClogLastReceiveDate = (
    select max(ReceiveDate) 
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0), 
cfaCTN = (
    select isnull(sum(b.CTNQty), 0)
    from PackingList a, PackingList_Detail b 
    where a.id = b.id and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0 and b.CFAReceiveDate is not null),
DRYCTN = (
    select ISNULL(sum(b.CTNQty), 0)
    from PackingList a, PackingList_Detail b 
    where a.id = b.id and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0 and b.DRYReceiveDate is not null),
PackErrCTN = (
    select ISNULL(sum(b.CTNQty), 0)
    from PackingList a, PackingList_Detail b 
    where a.id = b.id and (a.Type = 'B' or a.Type = 'L') and b.OrderID = '{0}' and b.DisposeFromClog = 0 and b.PackErrTransferDate is not null)
,FtyToClogTransit = (
    SELECT SUM(pld.CTNQty)
    FROM PackingList pl
    INNER JOIN PackingList_Detail pld ON pld.ID = pl.ID
    WHERE pld.OrderID = '{0}' AND pld.TransferDate IS NOT NULL AND pld.ReceiveDate  IS NULL)
,ClogToCFATansit = (
    SELECT SUM(pld.CTNQty)
    FROM PackingList pl
    INNER JOIN PackingList_Detail pld ON pld.ID = pl.ID
    WHERE pld.OrderID = '{0}' AND pld.TransferCFADate IS NOT NULL AND pld.CFAReceiveDate IS NULL AND pld.ClogLocationID = '2CFA')
,CFAToClogTransit = (
    SELECT SUM(pld.CTNQty)
    FROM PackingList pl
    INNER JOIN PackingList_Detail pld ON pld.ID = pl.ID
    WHERE pld.OrderID = '{0}' AND pld.TransferCFADate IS NULL AND pld.CFAReceiveDate IS NULL AND pld.CFAReturnClogDate IS NOT NULL AND pld.ClogLocationID = '2Clog')
where ID = '{0}'",
                    dr["OrderID"].ToString()));
            }

            if (updateCmds.Count > 0)
            {
                DualResult result = DBProxy.Current.Executes(null, updateCmds);
                return result;
            }

            return Ict.Result.True;
        }
        #endregion
    }
}
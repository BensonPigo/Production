using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_WBScanRate
    {
        /// <inheritdoc/>
        public Base_ViewModel P_WBScanRate(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DateTime now = DateTime.Now;

            if (!item.SDate.HasValue)
            {
                item.SDate = MyUtility.Convert.GetDate(now);
            }

            try
            {
                // insert into PowerBI
                finalResult = this.UpdateBIData(item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        /// <inheritdoc/>
        private Base_ViewModel UpdateBIData(ExecutedList item)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string where = string.Empty;
            string tmp = new Base().SqlBITableHistory("P_WBScanRate", "P_WBScanRate_History", "#tmp_P_WBScanRate", where, true, false);

            string sqlcmd = $@"
            
           SELECT [Date] = CAST(GETDATE() AS DATE)
	            , [FactoryID] = f.FTYGroup
	            , [WBScanRate] = CAST(IIF(ISNULL(SUM(p.SewQty), 0) = 0, 0, (SUM(p.RFIDSewingLineInQty) * 1.0 / SUM(p.SewQty)) * 100) AS DECIMAL(5, 2))
	            , [TTLRFIDSewInlineQty] = SUM(p.RFIDSewingLineInQty)
	            , [TTLSewQty] = SUM(p.SewQty)
                , [BIFactoryID] = @BIFactoryID
                , [BIInsertDate] = GetDate()
            INTO #tmp_P_WBScanRate
            FROM P_WIP p
            INNER JOIN [MainServer].[Production].[dbo].[Factory] f ON P.FactoryID = f.ID
            WHERE p.SewInLine <= @StartDate
            AND p.SewOffLine >= @StartDate
            GROUP BY f.FTYGroup

            {tmp}

            UPDATE p
	            SET p.WBScanRate = t.WBScanRate
	                , p.TTLRFIDSewInlineQty = t.TTLRFIDSewInlineQty
	                , p.TTLSewQty = t.TTLSewQty
                    , p.[BIFactoryID] = t.BIFactoryID
                    , p.[BIInsertDate] = t.BIInsertDate
                    , p.[BIStatus] = 'New'
            FROM P_WBScanRate p
            INNER JOIN #tmp_P_WBScanRate t ON t.[Date] = p.[Date] AND t.FactoryID = p.FactoryID

            INSERT INTO P_WBScanRate([Date], FactoryID, WBScanRate, TTLRFIDSewInlineQty, TTLSewQty ,[BIFactoryID], [BIInsertDate], [BIStatus])
            SELECT [Date], FactoryID, [WBScanRate], [TTLRFIDSewInlineQty], [TTLSewQty], [BIFactoryID], [BIInsertDate], 'New'
            FROM #tmp_P_WBScanRate t 
            WHERE NOT EXISTS (SELECT 1 FROM P_WBScanRate p　WHERE t.[Date] = p.[Date] AND t.FactoryID = p.FactoryID)

            Drop Table #tmp_P_WBScanRate
            ";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", item.SDate.Value.ToString("yyyy/MM/dd")),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };

                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ExecuteByConnTransactionScope(conn: sqlConn, cmdtext: sqlcmd, parameters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

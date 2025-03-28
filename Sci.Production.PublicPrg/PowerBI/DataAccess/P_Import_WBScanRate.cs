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
        public Base_ViewModel P_WBScanRate(DateTime? sDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DateTime now = DateTime.Now;

            if (!sDate.HasValue)
            {
                sDate = MyUtility.Convert.GetDate(now);
            }

            try
            {
                // insert into PowerBI
                finalResult = this.UpdateBIData(sDate.Value);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        /// <inheritdoc/>
        private Base_ViewModel UpdateBIData(DateTime sDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"
            
           SELECT [Date] = CAST(GETDATE() AS DATE)
	            , f.FTYGroup
	            , [WBScanRate] = CAST(IIF(ISNULL(SUM(p.SewQty), 0) = 0, 0, (SUM(p.RFIDSewingLineInQty) * 1.0 / SUM(p.SewQty)) * 100) AS DECIMAL(5, 2))
	            , [TTLRFIDSewInlineQty] = SUM(p.RFIDSewingLineInQty)
	            , [TTLSewQty] = SUM(p.SewQty)
            INTO #tmp_P_WBScanRate
            FROM P_WIP p
            INNER JOIN [MainServer].[Production].[dbo].[Factory] f ON P.FactoryID = f.ID
            WHERE p.SewInLine <= @StartDate
            AND p.SewOffLine >= @StartDate
            GROUP BY f.FTYGroup

            UPDATE p
	            SET p.WBScanRate = t.WBScanRate
	                , p.TTLRFIDSewInlineQty = t.TTLRFIDSewInlineQty
	                , p.TTLSewQty = t.TTLSewQty
            FROM P_WBScanRate p
            INNER JOIN #tmp_P_WBScanRate t ON t.[Date] = p.[Date] AND t.FTYGroup = p.FactoryID

            INSERT INTO P_WBScanRate([Date], FactoryID, WBScanRate, TTLRFIDSewInlineQty, TTLSewQty)
            SELECT [Date], FTYGroup, [WBScanRate], [TTLRFIDSewInlineQty], [TTLSewQty]
            FROM #tmp_P_WBScanRate t 
            WHERE NOT EXISTS (SELECT 1 FROM P_WBScanRate p　WHERE t.[Date] = p.[Date] AND t.FTYGroup = p.FactoryID)

            if exists (select 1 from BITableInfo b where b.id = 'P_WBScanRate')
            begin
	            update b
		            set b.TransferDate = getdate()
	            from BITableInfo b
	            where b.id = 'P_WBScanRate'
            end
            else 
            begin
	            insert into BITableInfo(Id, TransferDate)
	            values('P_WBScanRate', getdate())
            end

            Drop Table #tmp_P_WBScanRate
            ";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", sDate.ToString("yyyy/MM/dd")),
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

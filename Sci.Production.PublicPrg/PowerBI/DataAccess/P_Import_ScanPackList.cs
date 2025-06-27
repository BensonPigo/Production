using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_ScanPackList
    {
        /// <inheritdoc/>
        public Base_ViewModel P_ScanPackListTransferIn(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Packing_R01 biModel = new Packing_R01();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Packing_R01_ViewModel model = new Packing_R01_ViewModel()
                {
                    SP1 = string.Empty,
                    SP2 = string.Empty,
                    PackingID1 = string.Empty,
                    PackingID2 = string.Empty,
                    BuyerDelivery1 = string.Empty,
                    BuyerDelivery2 = string.Empty,
                    ScanEditDate1 = item.SDate.Value.ToString("yyyy/MM/dd 00:00:00"),
                    ScanEditDate2 = item.EDate.Value.ToString("yyyy/MM/dd 23:59:59"),
                    PO1 = string.Empty,
                    PO2 = string.Empty,
                    Brand = string.Empty,
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                    ScanName = string.Empty,
                    IsSummary = false,
                    IsDetail = false,
                    Barcode = string.Empty,
                    IsPowerBI = true,
                };

                Base_ViewModel resultReport = biModel.GetPacking_R01Data(model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt, item);
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

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", item.SDate.Value.ToString("yyyy/MM/dd 00:00:00")),
                new SqlParameter("@eDate", item.EDate.Value.ToString("yyyy/MM/dd 23:59:59")),
                new SqlParameter("@BIFactoryID", item.RgCode),
                new SqlParameter("@IsTrans", item.IsTrans),
            };
            string sql = $@"	
if @IsTrans = 1
begin
    insert into [P_ScanPackList_History]([Ukey], [FactoryID], [BIFactoryID], [BIInsertDate])
    select [Ukey], [FactoryID], [BIFactoryID], GETDATE()
    from POWERBIReportData.dbo.P_ScanPackList p
    where p.[ScanDate] >= @sDate
    and p.[ScanDate] <= @eDate
end

delete from p
from POWERBIReportData.dbo.P_ScanPackList p
where p.[ScanDate] >= @sDate
and p.[ScanDate] <= @eDate

INSERT INTO POWERBIReportData.dbo.P_ScanPackList (
    FactoryID, PackingID, OrderID, CTNStartNo, ShipModeID,
    StyleID, BrandID, SeasonID, SewLine, Customize1,
    CustPONo, BuyerID, BuyerDelivery, Destination, Colorway,
    Color, Size, CTNBarcode, QtyPerCTN, ShipQty,
    QtyPerCTNScan, PackingError, ErrQty, AuditQCName, ActCTNWeight,
    HangtagBarcode, ScanDate, ScanName, CartonStatus, Lacking,
    LackingQty, BIFactoryID, BIInsertDate
)
SELECT 
    t.Factory,
    t.[Packing#],
    t.[SP#],
    t.[CTN#],
    ISNULL(t.Shipmode, ''),
    ISNULL(t.Style, ''),
    ISNULL(t.Brand, ''),
    ISNULL(t.Season, ''),
    ISNULL(t.Sewingline, ''),
    ISNULL(t.Customize1, ''),
    ISNULL(t.[P.O.#], ''),
    ISNULL(t.Buyer, ''),
    t.BuyerDelivery,
    ISNULL(t.Destination, ''),
    ISNULL(t.Colorway, ''),
    ISNULL(t.Color, ''),
    ISNULL(t.Size, ''),
    ISNULL(t.[CTN Barcode], ''),
    ISNULL(t.[PC/CTN], ''),
    ISNULL(t.QTY, 0),
    ISNULL(t.[PC/CTN Scanned], ''),
    ISNULL(t.PackingError, ''),
    ISNULL(t.ErrQty, 0),
    ISNULL(t.AuditQCName, ''),
    ISNULL(t.[Actual CTN Weight], 0),
    ISNULL(t.[Ref. Barcode], ''),
    t.[Scan Date],
    ISNULL(t.[Scan Name], ''),
    ISNULL(t.[Carton Status], ''),
    ISNULL(t.Lacking, ''),
    ISNULL(t.[Lacking Qty], 0),
    @BIFactoryID,
    GETDATE()
FROM #tmp t
WHERE t.[Scan Date] BETWEEN @sDate AND @eDate

if @IsTrans = 1
begin
    insert into [P_ScanPackList_History]([Ukey], [FactoryID], [BIFactoryID], [BIInsertDate])
    select [Ukey], [FactoryID], [BIFactoryID], GETDATE()
    from POWERBIReportData.dbo.P_ScanPackList p
    where not exists (
	    select 1 from [MainServer].[Production].[dbo].PackingList_Detail pld with (nolock)
	    inner join [MainServer].[Production].[dbo].PackingList pl with (nolock) on pl.ID = pld.ID
	    where pl.FactoryID = p.FactoryID and pld.ID = p.PackingID and pld.OrderID = p.OrderID and pld.CTNStartNo = p.CTNStartNo
    )
end

delete from p
from POWERBIReportData.dbo.P_ScanPackList p
where not exists (
	select 1 from [MainServer].[Production].[dbo].PackingList_Detail pld with (nolock)
	inner join [MainServer].[Production].[dbo].PackingList pl with (nolock) on pl.ID = pld.ID
	where pl.FactoryID = p.FactoryID and pld.ID = p.PackingID and pld.OrderID = p.OrderID and pld.CTNStartNo = p.CTNStartNo
)

";

            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

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
        public Base_ViewModel P_ScanPackListTransferIn(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Packing_R01 biModel = new Packing_R01();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
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
                    ScanEditDate1 = sDate.Value.ToString("yyyy/MM/dd 00:00:00"),
                    ScanEditDate2 = eDate.Value.ToString("yyyy/MM/dd 23:59:59"),
                    PO1 = string.Empty,
                    PO2 = string.Empty,
                    Brand = string.Empty,
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                    ScanName = string.Empty,
                    IsSummary = false,
                    IsDetail = false,
                    Barcode = string.Empty,
                };

                Base_ViewModel resultReport = biModel.GetPacking_R01Data(model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt, sDate, eDate);
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

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult;
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", sDate.Value.ToString("yyyy/MM/dd 00:00:00")),
                new SqlParameter("@eDate", eDate.Value.ToString("yyyy/MM/dd 23:59:59")),
            };
            string sql = @"	
delete from p
from POWERBIReportData.dbo.P_ScanPackList p
where p.[ScanDate] >= @sDate
and p.[ScanDate] <= @eDate

insert into POWERBIReportData.dbo.P_ScanPackList([FactoryID], [PackingID], [OrderID], [CTNStartNo], [ShipModeID], [StyleID], [BrandID], [SeasonID], [SewLine], [Customize1], [CustPONo], [BuyerID], [BuyerDelivery], [Destination], [Colorway], [Color], [Size], [CTNBarcode], [QtyPerCTN], [ShipQty], [QtyPerCTNScan]
, [PackingError], [ErrQty], [AuditQCName], [ActCTNWeight], [HangtagBarcode], [ScanDate], [ScanName], [CartonStatus], [Lacking], [LackingQty])
select t.Factory
    , t.[Packing#]
    , t.[SP#]
    , t.[CTN#]
    , Shipmode = ISNULL(t.Shipmode, '')
    , Style = ISNULL(t.Style, '')
    , Brand = ISNULL(t.Brand, '')
    , Season = ISNULL(t.Season, '')
    , [Sewingline] = ISNULL(t.[Sewingline], '')
    , Customize1 = ISNULL(t.Customize1, '')
    , [P.O.#] = ISNULL(t.[P.O.#], '')
    , Buyer = ISNULL(t.Buyer, '')
    , t.BuyerDelivery
    , Destination = ISNULL(t.Destination, '')
    , Colorway = ISNULL(t.Colorway, '')
    , Color = ISNULL(t.Color, '')
    , Size = ISNULL(t.Size, '')
    , [CTN Barcode] = ISNULL(t.[CTN Barcode], '')
    , [PC/CTN] = ISNULL(t.[PC/CTN], '')
    , QTY = ISNULL(t.QTY, 0)
    , [PC/CTN Scanned] = ISNULL(t.[PC/CTN Scanned], '')
    , PackingError = ISNULL(t.PackingError, '')
    , ErrQty = ISNULL(t.ErrQty, 0)
    , AuditQCName = ISNULL(t.AuditQCName, '')
    , [Actual CTN Weight] = ISNULL(t.[Actual CTN Weight], 0)
    , [Ref. Barcode] = ISNULL(t.[Ref. Barcode], '')
    , t.[Scan Date]
    , [Scan Name] = ISNULL(t.[Scan Name], '')
    , [Carton Status] = ISNULL(t.[Carton Status], '')
    , Lacking = ISNULL(t.Lacking, '')
    , [Lacking Qty] = ISNULL(t.[Lacking Qty], 0)
from #tmp t
where t.[Scan Date] >= @sDate
and t.[Scan Date] <= @eDate

delete from p
from POWERBIReportData.dbo.P_ScanPackList p
where not exists (
	select 1 from [MainServer].[Production].[dbo].PackingList_Detail pld with (nolock)
	inner join [MainServer].[Production].[dbo].PackingList pl with (nolock) on pl.ID = pld.ID
	where pl.FactoryID = p.FactoryID and pld.ID = p.PackingID and pld.OrderID = p.OrderID and pld.CTNStartNo = p.CTNStartNo
)


if exists (select 1 from BITableInfo b where b.id = 'P_ScanPackList')
	begin
		update b
			set b.TransferDate = getdate()
		from BITableInfo b
		where b.id = 'P_ScanPackList'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_ScanPackList', getdate())
	end

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

using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
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

                DataTable detailTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable);
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

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult;
            string sql = @"	
update p
	set p.[FactoryID]			= t.Factory
	, p.[PackingID]				= t.[Packing#]
	, p.[OrderID]				= t.[SP#]
	, p.[CTNStartNo]			= t.[CTN#]
	, p.[ShipModeID]			= t.Shipmode
	, p.[StyleID]				= t.Style
	, p.[BrandID]				= t.Brand
	, p.[SeasonID]				= t.Season
	, p.[SewLine]				= t.[Sewingline]
	, p.[Customize1]			= t.Customize1
	, p.[CustPONo]				= t.[P.O.#]
	, p.[BuyerID]				= t.Buyer
	, p.[BuyerDelivery]			= t.BuyerDelivery
	, p.[Destination]			= t.Destination
	, p.[Colorway]				= t.Colorway
	, p.[Color]					= t.Color
	, p.[Size]					= t.Size
	, p.[CTNBarcode]			= t.[CTN Barcode]
	, p.[QtyPerCTN]				= t.[PC/CTN]
	, p.[ShipQty]				= t.QTY
	, p.[QtyPerCTNScan]			= t.[PC/CTN Scanned]
	, p.[PackingError]			= t.PackingError
	, p.[ErrQty]				= t.ErrQty
	, p.[AuditQCName]			= t.AuditQCName
	, p.[ActCTNWeight]			= t.[Actual CTN Weight]
	, p.[HangtagBarcode]		= t.[Ref. Barcode]
	, p.[ScanName]				= t.[Scan Name]
	, p.[CartonStatus]			= t.[Carton Status]
	, p.[Lacking]				= t.Lacking
	, p.[LackingQty]			= t.[Lacking Qty]
from POWERBIReportData.dbo.P_ScanPackList p
inner join #tmp t on t.Factory = p.FactoryID and t.[Packing#] = p.PackingID and t.[SP#] = p.OrderID and t.[CTN#] = p.CTNStartNo
where p.[ScanDate] != t.[Scan Date]

insert into POWERBIReportData.dbo.P_ScanPackList([FactoryID], [PackingID], [OrderID], [CTNStartNo], [ShipModeID], [StyleID], [BrandID], [SeasonID], [SewLine], [Customize1], [CustPONo], [BuyerID], [BuyerDelivery], [Destination], [Colorway], [Color], [Size], [CTNBarcode], [QtyPerCTN], [ShipQty], [QtyPerCTNScan]
, [PackingError], [ErrQty], [AuditQCName], [ActCTNWeight], [HangtagBarcode], [ScanDate], [ScanName], [CartonStatus], [Lacking], [LackingQty])
select t.Factory, t.[Packing#], t.[SP#], t.[CTN#], t.Shipmode, t.Style, t.Brand, t.Season, t.[Sewingline]
    , t.Customize1, t.[P.O.#], t.Buyer, t.BuyerDelivery, t.Destination, t.Colorway, t.Color, t.Size, t.[CTN Barcode], t.[PC/CTN], t.QTY, t.[PC/CTN Scanned]
    , t.PackingError, t.ErrQty, t.AuditQCName, t.[Actual CTN Weight], t.[Ref. Barcode], t.[Scan Date], t.[Scan Name], t.[Carton Status], t.Lacking, t.[Lacking Qty]
from #tmp t
where not exists (select 1 from POWERBIReportData.dbo.P_ScanPackList p where t.Factory = p.FactoryID and t.[Packing#] = p.PackingID and t.[SP#] = p.OrderID and t.[CTN#] = p.CTNStartNo)

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
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: null),
                };
            }

            return finalResult;
        }
    }
}

using System;
using System.Collections.Generic;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System.Data;
using System.Data.SqlClient;
using Sci.Data;
using System.Text;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_InventoryStockListReport
    {
        /// <inheritdoc/>
        public P_Import_InventoryStockListReport()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel P_InventoryStockListReport(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Warehouse_R21 biModel = new Warehouse_R21();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddMonths(-3).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Warehouse_R21_ViewModel warehouse_R21 = new Warehouse_R21_ViewModel()
                {
                    AddEditDateStart = sDate,
                    AddEditDateEnd = eDate,
                    ReportType = 0,
                    BoolCheckQty = true,
                    ArriveWHFrom = sDate,
                    IsPowerBI = true,
                };

                Base_ViewModel resultReport = biModel.GetWarehouse_R21Data(warehouse_R21);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, sDate.Value, eDate.Value);
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

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate, DateTime eDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
                    new SqlParameter("@EDate", eDate),
                };
                string sql = @"	
UPDATE t
SET 
     t.[MDivisionID] = s.[MDivisionID]
    ,t.[FactoryID] = s.[FactoryID]
    ,t.[SewLine] = s.[SewLine]
    ,t.[Category] = s.[Category]
    ,t.[OrderTypeID] = s.[OrderTypeID]
    ,t.[WeaveTypeID] = s.[WeaveTypeID]
    ,t.[BuyerDelivery] = s.[BuyerDelivery]
    ,t.[OrigBuyerDelivery] = s.[OrigBuyerDelivery]
    ,t.[MaterialComplete] = s.[MaterialComplete]
    ,t.[ETA] = s.[ETA]
    ,t.[ArriveWHDate] = s.[ArriveWHDate]
    ,t.[ExportID] = s.[ExportID]
    ,t.[Packages] = s.[Packages]
    ,t.[ContainerNo] = s.[ContainerNo]
    ,t.[BrandID] = s.[BrandID]
    ,t.[StyleID] = s.[StyleID]
    ,t.[SeasonID] = s.[SeasonID]
    ,t.[ProjectID] = s.[ProjectID]
    ,t.[ProgramID] = s.[ProgramID]
    ,t.[MaterialType] = s.[MaterialType]
    ,t.[StockPOID] = s.[StockPOID]
    ,t.[StockSeq1] = s.[StockSeq1]
    ,t.[StockSeq2] = s.[StockSeq2]
    ,t.[Refno] = s.[Refno]
    ,t.[SCIRefno] = s.[SCIRefno]
    ,t.[Description] = s.[Description]
    ,t.[ColorID] = s.[ColorID]
    ,t.[ColorName] = s.[ColorName]
    ,t.[Size] = s.[Size]
    ,t.[StockUnit] = s.[StockUnit]
    ,t.[PurchaseQty] = s.[PurchaseQty]
    ,t.[OrderQty] = s.[OrderQty]
    ,t.[ShipQty] = s.[ShipQty]
    ,t.[InQty] = s.[InQty]
    ,t.[OutQty] = s.[OutQty]
    ,t.[AdjustQty] = s.[AdjustQty]
    ,t.[ReturnQty] = s.[ReturnQty]
    ,t.[BalanceQty] = s.[BalanceQty]
    ,t.[MtlLocationID] = s.[MtlLocationID]
    ,t.[MCHandle] = s.[MCHandle]
    ,t.[POHandle] = s.[POHandle]
    ,t.[POSMR] = s.[POSMR]
    ,t.[Supplier] = s.[Supplier]
    ,t.[VID] = s.[VID]
    ,t.[Grade] = s.[Grade]
    ,t.[AddDate] = s.[AddDate]
    ,t.[EditDate] = s.[EditDate]
from P_InventoryStockListReport t 
inner join #tmp s on t.[POID] = s.[POID] 
    AND t.[SEQ1] = s.[SEQ1] 
    AND t.[SEQ2] = s.[SEQ2] 
    AND t.[Roll] = s.[Roll]
    AND t.[Dyelot] = s.[Dyelot]
    AND t.[StockType] = s.[StockType]


INSERT INTO [dbo].[P_InventoryStockListReport]
           ([MDivisionID]
           ,[FactoryID]
           ,[SewLine]
           ,[POID]
           ,[Category]
           ,[OrderTypeID]
           ,[WeaveTypeID]
           ,[BuyerDelivery]
           ,[OrigBuyerDelivery]
           ,[MaterialComplete]
           ,[ETA]
           ,[ArriveWHDate]
           ,[ExportID]
           ,[Packages]
           ,[ContainerNo]
           ,[BrandID]
           ,[StyleID]
           ,[SeasonID]
           ,[ProjectID]
           ,[ProgramID]
           ,[SEQ1]
           ,[SEQ2]
           ,[MaterialType]
           ,[StockPOID]
           ,[StockSeq1]
           ,[StockSeq2]
           ,[Refno]
           ,[SCIRefno]
           ,[Description]
           ,[ColorID]
           ,[ColorName]
           ,[Size]
           ,[StockUnit]
           ,[PurchaseQty]
           ,[OrderQty]
           ,[ShipQty]
           ,[Roll]
           ,[Dyelot]
           ,[StockType]
           ,[InQty]
           ,[OutQty]
           ,[AdjustQty]
           ,[ReturnQty]
           ,[BalanceQty]
           ,[MtlLocationID]
           ,[MCHandle]
           ,[POHandle]
           ,[POSMR]
           ,[Supplier]
           ,[VID]
           ,[Grade]
           ,[AddDate]
           ,[EditDate])
select [MDivisionID]
           ,[FactoryID]
           ,[SewLine]
           ,[POID]
           ,[Category]
           ,[OrderTypeID]
           ,[WeaveTypeID]
           ,[BuyerDelivery]
           ,[OrigBuyerDelivery]
           ,[MaterialComplete]
           ,[ETA]
           ,[ArriveWHDate]
           ,[ExportID]
           ,[Packages]
           ,[ContainerNo]
           ,[BrandID]
           ,[StyleID]
           ,[SeasonID]
           ,[ProjectID]
           ,[ProgramID]
           ,[SEQ1]
           ,[SEQ2]
           ,[MaterialType]
           ,[StockPOID]
           ,[StockSeq1]
           ,[StockSeq2]
           ,[Refno]
           ,[SCIRefno]
           ,[Description]
           ,[ColorID]
           ,[ColorName]
           ,[Size]
           ,[StockUnit]
           ,[PurchaseQty]
           ,[OrderQty]
           ,[ShipQty]
           ,[Roll]
           ,[Dyelot]
           ,[StockType]
           ,[InQty]
           ,[OutQty]
           ,[AdjustQty]
           ,[ReturnQty]
           ,[BalanceQty]
           ,[MtlLocationID]
           ,[MCHandle]
           ,[POHandle]
           ,[POSMR]
           ,[Supplier]
           ,[VID]
           ,[Grade]
           ,[AddDate]
           ,[EditDate]
from #tmp s
where not exists (
    select 1 from P_InventoryStockListReport t 
    where t.[POID] = s.[POID] 
    AND t.[SEQ1] = s.[SEQ1] 
    AND t.[SEQ2] = s.[SEQ2] 
    AND t.[Roll] = s.[Roll]
    AND t.[Dyelot] = s.[Dyelot]
    AND t.[StockType] = s.[StockType]
)

delete t 
from dbo.P_InventoryStockListReport t
Outer Apply (
    Select Date = cast(Data as Date) From SplitString(t.[ArriveWHDate], ';') WHERE Data <> ''
)getArriveWHDate
where 
getArriveWHDate.Date Between @SDate and @eDate
and not exists (
    select 1 from #tmp s 
    where t.[POID] = s.[POID] 
    AND t.[SEQ1] = s.[SEQ1] 
    AND t.[SEQ2] = s.[SEQ2] 
    AND t.[Roll] = s.[Roll]
    AND t.[Dyelot] = s.[Dyelot]
    AND t.[StockType] = s.[StockType]
)


IF EXISTS (SELECT 1 FROM BITableInfo B WHERE B.ID = 'P_InventoryStockListReport')
BEGIN
    UPDATE B
    SET b.TransferDate = getdate()
    FROM BITableInfo B
    WHERE B.ID = 'P_InventoryStockListReport'
END
ELSE 
BEGIN
    INSERT INTO BITableInfo(Id, TransferDate)
    VALUES('P_InventoryStockListReport', GETDATE())
END
";
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

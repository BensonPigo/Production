using System;
using System.Collections.Generic;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System.Data;
using System.Data.SqlClient;
using Sci.Data;

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
        public Base_ViewModel P_InventoryStockListReport(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Warehouse_R21 biModel = new Warehouse_R21();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse("2021/12/01");
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Warehouse_R21_ViewModel warehouse_R21 = new Warehouse_R21_ViewModel()
                {
                    AddEditDateStart = item.SDate,
                    AddEditDateEnd = item.EDate,
                    ReportType = 0,
                    BoolCheckQty = true,
                    ArriveWHFrom = item.SDate,
                    ArriveWHTo = item.EDate,
                    IsPowerBI = true,
                };

                Base_ViewModel resultReport = biModel.GetWarehouse_R21Data(warehouse_R21);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, item);
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
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@EDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                string sql = @"	
                alter table #tmp alter column MDivisionID        varchar (8)
                alter table #tmp alter column FactoryID          varchar (8)
                alter table #tmp alter column SewLine            varchar (60)
                alter table #tmp alter column POID               varchar (13)
                alter table #tmp alter column Category           varchar (15)
                alter table #tmp alter column OrderTypeID        varchar (20)
                alter table #tmp alter column WeaveTypeID        varchar (20)
                alter table #tmp alter column BuyerDelivery      date
                alter table #tmp alter column OrigBuyerDelivery  date
                alter table #tmp alter column MaterialComplete   varchar (1)
                alter table #tmp alter column ETA                date
                alter table #tmp alter column ArriveWHDate       varchar (500)
                alter table #tmp alter column ExportID           varchar (300)
                alter table #tmp alter column Packages           varchar (100)
                alter table #tmp alter column ContainerNo        varchar (300)
                alter table #tmp alter column BrandID            varchar (8)
                alter table #tmp alter column StyleID            varchar (15)
                alter table #tmp alter column SeasonID           varchar (10)
                alter table #tmp alter column ProjectID          varchar (5)
                alter table #tmp alter column ProgramID          nvarchar (12)
                alter table #tmp alter column SEQ1               varchar (3)
                alter table #tmp alter column SEQ2               varchar (2)
                alter table #tmp alter column MaterialType       varchar (50)
                alter table #tmp alter column StockPOID          varchar (13)
                alter table #tmp alter column StockSeq1          varchar (3)
                alter table #tmp alter column StockSeq2          varchar (2)
                alter table #tmp alter column Refno              varchar (36)
                alter table #tmp alter column SCIRefno           varchar (30)
                alter table #tmp alter column Description        nvarchar (150)
                alter table #tmp alter column ColorID            varchar (100)
                alter table #tmp alter column ColorName          nvarchar (150)
                alter table #tmp alter column Size               varchar (50)
                alter table #tmp alter column StockUnit          varchar (8)
                alter table #tmp alter column PurchaseQty        decimal
                alter table #tmp alter column OrderQty           int
                alter table #tmp alter column ShipQty            decimal
                alter table #tmp alter column Roll               varchar (8)
                alter table #tmp alter column Dyelot             varchar (8)
                alter table #tmp alter column StockType          varchar (15)
                alter table #tmp alter column InQty              numeric (11, 2)
                alter table #tmp alter column OutQty             numeric (11, 2)
                alter table #tmp alter column AdjustQty          numeric (11, 2)
                alter table #tmp alter column ReturnQty          numeric (11, 2)
                alter table #tmp alter column BalanceQty         numeric (11, 2)
                alter table #tmp alter column MtlLocationID      varchar (500)
                alter table #tmp alter column MCHandle           varchar (100)
                alter table #tmp alter column POHandle           varchar (100)
                alter table #tmp alter column POSMR              varchar (100)
                alter table #tmp alter column Supplier           varchar (50)
                alter table #tmp alter column VID                varchar (200)
                alter table #tmp alter column AddDate            datetime
                alter table #tmp alter column EditDate           datetime
                alter table #tmp alter column Grade              varchar (10)

                if @IsTrans = 1
                begin
                    INSERT INTO P_InventoryStockListReport_History (
                        POID, SEQ1, SEQ2, Roll, Dyelot, StockType, BIFactoryID, BIInsertDate
                    )
                    SELECT 
                        POID, SEQ1, SEQ2, Roll, Dyelot, StockType, @BIFactoryID, GETDATE()
                    FROM dbo.P_InventoryStockListReport
                end

                DELETE FROM dbo.P_InventoryStockListReport

                INSERT INTO dbo.P_InventoryStockListReport (
                    MDivisionID, FactoryID, SewLine, POID, Category, OrderTypeID, WeaveTypeID, BuyerDelivery,
                    OrigBuyerDelivery, MaterialComplete, ETA, ArriveWHDate, ExportID, Packages, ContainerNo,
                    BrandID, StyleID, SeasonID, ProjectID, ProgramID, SEQ1, SEQ2, MaterialType, StockPOID,
                    StockSeq1, StockSeq2, Refno, SCIRefno, Description, ColorID, ColorName, Size, StockUnit,
                    PurchaseQty, OrderQty, ShipQty, Roll, Dyelot, StockType, InQty, OutQty, AdjustQty,
                    ReturnQty, BalanceQty, MtlLocationID, MCHandle, POHandle, POSMR, Supplier, VID, Grade,
                    AddDate, EditDate, BIFactoryID, BIInsertDate
                )
                SELECT 
                    MDivisionID, FactoryID, SewLine, POID, Category, OrderTypeID, WeaveTypeID, BuyerDelivery,
                    OrigBuyerDelivery, MaterialComplete, ETA, ArriveWHDate, ExportID, Packages, ContainerNo,
                    BrandID, StyleID, SeasonID, ProjectID, ProgramID, SEQ1, SEQ2, MaterialType, StockPOID,
                    StockSeq1, StockSeq2, Refno, SCIRefno, Description, ColorID, ColorName, Size, StockUnit,
                    PurchaseQty, OrderQty, ShipQty, Roll, Dyelot, StockType, InQty, OutQty, AdjustQty,
                    ReturnQty, BalanceQty, MtlLocationID, MCHandle, POHandle, POSMR, Supplier, VID, Grade,
                    AddDate, EditDate, @BIFactoryID, GETDATE()
                FROM #tmp s
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

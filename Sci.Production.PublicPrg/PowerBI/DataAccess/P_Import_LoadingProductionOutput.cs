using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 裡面用的GetProductionOutputSummary在Planning.R05跟Centralized.R05報表都呼叫這支
    /// </summary>
    public class P_Import_LoadingProductionOutput
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_LoadingProductionOutput(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.Year.ToString());
            }

            try
            {
                Base_ViewModel resultReport = this.GetLoadingProductionOutputData(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, item);
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
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@Year", item.SDate.Value.Year),
                new SqlParameter("@IsTrans", item.IsTrans),
            };

            using (sqlConn)
            {
                string sql = $@" 
UPDATE t
SET
    t.MDivisionID              = s.MDivisionID,
    t.FtyZone                  = s.FtyZone,
    t.FactoryID                = s.FactoryID,
    t.BuyerDelivery            = s.BuyerDelivery,
    t.SciDelivery              = s.SciDelivery,
    t.SCIKey                   = s.SCIKey,
    t.SCIKeyHalf               = s.SCIKeyHalf,
    t.BuyerKey                 = s.BuyerKey,
    t.BuyerKeyHalf             = s.BuyerKeyHalf,
    t.BuyerMonthHalf           = s.BuyerMonthHalf,
    t.SPNO                     = s.ID,
    t.Category                 = s.Category,
    t.Cancelled                = s.Cancelled,
    t.IsCancelNeedProduction   = s.IsCancelNeedProduction,
    t.PartialShipment          = s.PartialShipment,
    t.LastBuyerDelivery        = s.LastBuyerDelivery,
    t.StyleID                  = s.StyleID,
    t.SeasonID                 = s.SeasonID,
    t.CustPONO                 = s.CustPONO,
    t.BrandID                  = s.BrandID,
    t.CPU                      = s.CPU,
    t.Qty                      = s.Qty,
    t.FOCQty                   = s.FOCQty,
    t.PulloutQty               = s.PulloutQty,
    t.OrderShortageCPU         = s.OrderShortageCPU,
    t.TotalCPU                 = s.TotalCPU,
    t.SewingOutput             = s.SewingOutput,
    t.SewingOutputCPU          = s.SewingOutputCPU,
    t.BalanceQty               = s.BalanceQty,
    t.BalanceCPU               = s.BalanceCPU,
    t.BalanceCPUIrregular      = s.BalanceCPUIrregular,
    t.SewLine                  = s.SewLine,
    t.Dest                     = s.Dest,
    t.OrderTypeID              = s.OrderTypeID,
    t.ProgramID                = s.ProgramID,
    t.CdCodeID                 = s.CdCodeID,
    t.ProductionFamilyID       = s.ProductionFamilyID,
    t.FtyGroup                 = s.FtyGroup,
    t.PulloutComplete          = s.PulloutComplete,
    t.SewInLine                = s.SewInLine,
    t.SewOffLine               = s.SewOffLine,
    t.TransFtyZone             = s.TransFtyZone,
    t.CDCodeNew                = s.CDCodeNew,
    t.ProductType              = s.ProductType,
    t.FabricType               = s.FabricType,
    t.Lining                   = s.Lining,
    t.Gender                   = s.Gender,
    t.Construction             = s.Construction,
    t.[FM Sister]              = s.FMSister,
    t.[Sample Group]           = s.SampleGroup,
    t.[Order Reason]           = s.OrderReason,
    t.[BuyBackReason]          = s.BuyBackReason,
    t.[LastProductionDate]     = s.LastProductionDate,
    t.[CRDDate]                = s.CRDDate,
    t.[BIFactoryID]            = s.BIFactoryID,
    t.[BIInsertDate]           = s.BIInsertDate
FROM P_LoadingProductionOutput t
INNER JOIN #Final s 
    ON t.FactoryID = s.FactoryID  
   AND t.SPNO     = s.ID

INSERT INTO P_LoadingProductionOutput (
    MDivisionID, FtyZone, FactoryID, BuyerDelivery, SciDelivery,
    SCIKey, SCIKeyHalf, BuyerKey, BuyerKeyHalf, BuyerMonthHalf,
    SPNO, Category, Cancelled, IsCancelNeedProduction, PartialShipment,
    LastBuyerDelivery, StyleID, SeasonID, CustPONO, BrandID,
    CPU, Qty, FOCQty, PulloutQty, OrderShortageCPU, TotalCPU,
    SewingOutput, SewingOutputCPU, BalanceQty, BalanceCPU, BalanceCPUIrregular,
    SewLine, Dest, OrderTypeID, ProgramID, CdCodeID, ProductionFamilyID,
    FtyGroup, PulloutComplete, SewInLine, SewOffLine, TransFtyZone,
    CDCodeNew, ProductType, FabricType, Lining, Gender, Construction,
    [FM Sister], [Sample Group], [Order Reason], [BuyBackReason],
    [LastProductionDate], [CRDDate], BIFactoryID, BIInsertDate
)
SELECT
    s.MDivisionID, s.FtyZone, s.FactoryID, s.BuyerDelivery, s.SciDelivery,
    s.SCIKey, s.SCIKeyHalf, s.BuyerKey, s.BuyerKeyHalf, s.BuyerMonthHalf,
    s.ID, s.Category, s.Cancelled, s.IsCancelNeedProduction, s.PartialShipment,
    s.LastBuyerDelivery, s.StyleID, s.SeasonID, s.CustPONO, s.BrandID,
    s.CPU, s.Qty, s.FOCQty, s.PulloutQty, s.OrderShortageCPU, s.TotalCPU,
    s.SewingOutput, s.SewingOutputCPU, s.BalanceQty, s.BalanceCPU, s.BalanceCPUIrregular,
    s.SewLine, s.Dest, s.OrderTypeID, s.ProgramID, s.CdCodeID, s.ProductionFamilyID,
    s.FtyGroup, s.PulloutComplete, s.SewInLine, s.SewOffLine, s.TransFtyZone,
    s.CDCodeNew, s.ProductType, s.FabricType, s.Lining, s.Gender, s.Construction,
    s.FMSister, s.SampleGroup, s.OrderReason, s.BuyBackReason,
    s.LastProductionDate, s.CRDDate, s.BIFactoryID, s.BIInsertDate
FROM #Final s
WHERE NOT EXISTS (
    SELECT 1 
    FROM P_LoadingProductionOutput t 
    WHERE t.FactoryID = s.FactoryID AND t.SPNO = s.ID
)

if @IsTrans = 1
begin
    INSERT INTO P_LoadingProductionOutput_History (FactoryID, Ukey, BIFactoryID, BIInsertDate)
    SELECT t.FactoryID, t.Ukey, t.BIFactoryID, GETDATE()
    FROM P_LoadingProductionOutput t
    WHERE (
            YEAR(t.BuyerDelivery) = @Year OR 
            YEAR(DATEADD(DAY, -7, t.SciDelivery)) = @Year
          )
      AND EXISTS (
            SELECT 1 FROM #Final f 
            WHERE t.FactoryID = f.FactoryID AND t.MDivisionID = f.MDivisionID
          )
      AND NOT EXISTS (
            SELECT 1 FROM #Final s 
            WHERE t.FactoryID = s.FactoryID AND t.SPNO = s.ID
          )
end

DELETE t
FROM P_LoadingProductionOutput t WITH (NOLOCK)
WHERE (
        YEAR(t.BuyerDelivery) = @Year OR 
        YEAR(DATEADD(DAY, -7, t.SciDelivery)) = @Year
      )
  AND EXISTS (
        SELECT 1 FROM #Final f 
        WHERE t.FactoryID = f.FactoryID AND t.MDivisionID = f.MDivisionID
      )
  AND NOT EXISTS (
        SELECT 1 FROM #Final s 
        WHERE t.FactoryID = s.FactoryID AND t.SPNO = s.ID
      )

if @IsTrans = 1
begin
    -- 備份未對應到 Orders 的資料
    INSERT INTO P_LoadingProductionOutput_History (FactoryID, Ukey, BIFactoryID, BIInsertDate)
    SELECT t.FactoryID, t.Ukey, t.BIFactoryID, GETDATE()
    FROM P_LoadingProductionOutput t
    LEFT JOIN [MainServer].Production.dbo.Orders o ON t.SPNO = o.ID AND t.FactoryID = o.FactoryID
    WHERE o.ID IS NULL
end

-- 刪除未對應到 Orders 的資料
DELETE t
FROM P_LoadingProductionOutput t
LEFT JOIN [MainServer].Production.dbo.Orders o ON t.SPNO = o.ID AND t.FactoryID = o.FactoryID
WHERE o.ID IS NULL

                ";

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#Final");
            }

            return finalResult;
        }

        private Base_ViewModel GetLoadingProductionOutputData(ExecutedList item)
        {
            StringBuilder sqlcmdSP = new StringBuilder();

            sqlcmdSP.Append("exec dbo.GetProductionOutputSummary");
            sqlcmdSP.Append(!MyUtility.Check.Empty(item.SDate.Value.Year) ? $" '{item.SDate.Value.Year}'," : "'',"); // Year
            sqlcmdSP.Append("'',"); // Brand
            sqlcmdSP.Append("'',"); // Mdivision
            sqlcmdSP.Append("'',"); // Factory
            sqlcmdSP.Append("'',"); // Zone
            sqlcmdSP.Append(" 1,"); // DateType
            sqlcmdSP.Append(" 1,"); // ChkOrder
            sqlcmdSP.Append(" 1,"); // ChkForecast
            sqlcmdSP.Append(" 1,"); // ChkFtylocalOrder
            sqlcmdSP.Append(" 1,"); // ExcludeSampleFactory
            sqlcmdSP.Append(" 1,"); // ChkMonthly
            sqlcmdSP.Append(" 1,"); // @IncludeCancelOrder
            sqlcmdSP.Append(" 0,"); // IsFtySide 工廠端限制ForeCast單 僅顯示SCI delivery or buyer delivery 小於等於 當月份+4個月的月底+7天
            sqlcmdSP.Append(" 1,"); // @IsPowerBI
            sqlcmdSP.Append(" 0 "); // @IsByCMPLockDate

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmdSP.ToString(), out DataTable dt),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@BIFactoryID", item.RgCode),
            };
            string sql = $@" 
DECLARE @strID NVARCHAR(15) = N'SubCON-Out_'

-- 非外代工 (本廠)
SELECT
    t.MDivisionID,
    t.FtyZone,
    t.FactoryID,
    t.BuyerDelivery,
    t.SciDelivery,
    t.SCIKey,
    t.SCIKeyHalf,
    t.BuyerKey,
    t.BuyerKeyHalf,
    t.BuyerMonthHalf,
    t.ID,
    t.Category,
    t.Cancelled,
    t.IsCancelNeedProduction,
    t.Buyback,
    t.PartialShipment,
    t.LastBuyerDelivery,
    t.StyleID,
    t.SeasonID,
    t.CustPONO,
    t.BrandID,
    t.CPU,
    t.Qty,
    t.FOCQty,
    t.PulloutQty,
    t.OrderShortageCPU,
    t.TotalCPU,
    t.SewingOutput,
    t.SewingOutputCPU,
    t.BalanceQty,
    t.BalanceCPU,
    t.BalanceCPUIrregular,
    t.SewLine,
    t.Dest,
    t.OrderTypeID,
    t.ProgramID,
    CdCodeID = '',
    ProductionFamilyID = '',
    t.FtyGroup,
    t.PulloutComplete,
    t.SewInLine,
    t.SewOffLine,
    t.TransFtyZone,
    sty.CDCodeNew,
    sty.ProductType,
    sty.FabricType,
    sty.Lining,
    sty.Gender,
    sty.Construction,
    t.FMSister,
    t.SampleGroup,
    t.OrderReason,
    t.BuyBackReason,
    t.LastProductionDate,
    t.CRDDate,
    BIFactoryID = @BIFactoryID,
    BIInsertDate = GETDATE()
FROM #tmp t
OUTER APPLY (
    SELECT 
        s.CDCodeNew,
        s.ID,
        ProductType  = r2.Name,
        FabricType   = r1.Name,
        Lining       = s.Lining,
        Gender       = s.Gender,
        Construction = d1.Name
    FROM Production.dbo.Orders o WITH (NOLOCK)
    LEFT JOIN Production.dbo.Style s WITH (NOLOCK) ON s.Ukey = o.StyleUkey
    LEFT JOIN Production.dbo.DropDownList d1 WITH (NOLOCK) ON d1.Type = 'StyleConstruction' AND d1.ID = s.Construction
    LEFT JOIN Production.dbo.Reason r1 WITH (NOLOCK) ON r1.ReasonTypeID = 'Fabric_Kind' AND r1.ID = s.FabricType
    LEFT JOIN Production.dbo.Reason r2 WITH (NOLOCK) ON r2.ReasonTypeID = 'Style_Apparel_Type' AND r2.ID = s.ApparelType
    WHERE o.ID = t.ID
) sty
WHERE EXISTS (
    SELECT 1 
    FROM Production.dbo.Factory f WITH (NOLOCK)
    WHERE f.ID = t.FactoryID AND f.IsProduceFty = 1
)

UNION ALL

-- 外代工
SELECT
    F.MDivisionID,
    F.FtyZone,
    F.ID AS FactoryID,
    T.BuyerDelivery,
    T.SciDelivery,
    T.SCIKey,
    T.SCIKeyHalf,
    T.BuyerKey,
    T.BuyerKeyHalf,
    T.BuyerMonthHalf,
    ID = CONVERT(VARCHAR(24), @strID + T.ID),
    T.Category,
    T.Cancelled,
    T.IsCancelNeedProduction,
    T.Buyback,
    T.PartialShipment,
    T.LastBuyerDelivery,
    T.StyleID,
    T.SeasonID,
    T.CustPONO,
    T.BrandID,
    T.CPU,
    T.Qty,
    T.FOCQty,
    T.PulloutQty,
    T.OrderShortageCPU,
    TotalCPU             = -T.TotalCPU,
    SewingOutput         = 0,
    SewingOutputCPU      = 0,
    BalanceQty           = 0,
    BalanceCPU           = 0,
    BalanceCPUIrregular  = 0,
    T.SewLine,
    T.Dest,
    T.OrderTypeID,
    T.ProgramID,
    CdCodeID             = '',
    ProductionFamilyID   = '',
    T.FtyGroup,
    T.PulloutComplete,
    T.SewInLine,
    T.SewOffLine,
    T.TransFtyZone,
    sty.CDCodeNew,
    sty.ProductType,
    sty.FabricType,
    sty.Lining,
    sty.Gender,
    sty.Construction,
    T.FMSister,
    T.SampleGroup,
    T.OrderReason,
    T.BuyBackReason,
    T.LastProductionDate,
    T.CRDDate,
    BIFactoryID = @BIFactoryID,
    BIInsertDate = GETDATE()
FROM #tmp T
LEFT JOIN Production.dbo.SCIFty F WITH (NOLOCK) ON F.ID = T.TransFtyZone
OUTER APPLY (
    SELECT 
        s.CDCodeNew,
        s.ID,
        ProductType  = r2.Name,
        FabricType   = r1.Name,
        Lining       = s.Lining,
        Gender       = s.Gender,
        Construction = d1.Name
    FROM Production.dbo.Orders o WITH (NOLOCK)
    LEFT JOIN Production.dbo.Style s WITH (NOLOCK) ON s.Ukey = o.StyleUkey
    LEFT JOIN Production.dbo.DropDownList d1 WITH (NOLOCK) ON d1.Type = 'StyleConstruction' AND d1.ID = s.Construction
    LEFT JOIN Production.dbo.Reason r1 WITH (NOLOCK) ON r1.ReasonTypeID = 'Fabric_Kind' AND r1.ID = s.FabricType
    LEFT JOIN Production.dbo.Reason r2 WITH (NOLOCK) ON r2.ReasonTypeID = 'Style_Apparel_Type' AND r2.ID = s.ApparelType
    WHERE o.ID = T.ID
) sty
WHERE T.TransFtyZone <> ''

-- 清理暫存表
DROP TABLE #tmp
";

            resultReport.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable finalDataTable, paramters: sqlParameters);
            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = finalDataTable;

            return resultReport;
        }
    }
}

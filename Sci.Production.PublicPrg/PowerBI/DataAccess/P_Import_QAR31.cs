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
    public class P_Import_QAR31
    {
        /// <inheritdoc/>
        public Base_ViewModel P_QAR31(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            QA_R31 biModel = new QA_R31();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-60).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.AddDays(30).ToString("yyyy/MM/dd"));
            }

            try
            {
                QA_R31_ViewModel qa_R31 = new QA_R31_ViewModel()
                {
                    BuyerDelivery1 = item.SDate,
                    BuyerDelivery2 = item.EDate,
                    SP1 = string.Empty,
                    SP2 = string.Empty,
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                    Brand = string.Empty,
                    Category = string.Empty,
                    Exclude_Sister_Transfer_Out = false,
                    Outstanding = false,
                    InspStaged = string.Empty,
                    IsPowerBI = true,
                };

                Base_ViewModel resultReport = biModel.GetQA_R31Data(qa_R31);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

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
                };
                string sql = @"	
UPDATE t
SET 
    t.Stage                    = s.Stage,
    t.InspResult               = s.InspResult,
    t.[NotYetInspCtn#]         = s.[NotYetInspCtn#],
    t.NotYetInspCtn            = s.NotYetInspCtn,
    t.NotYetInspQty            = s.NotYetInspQty,
    t.[FailCtn#]               = s.[FailCtn#],
    t.FailCtn                  = s.FailCtn,
    t.FailQty                  = s.FailQty,
    t.MDivisionID              = s.MDivisionID,
    t.FactoryID                = s.FactoryID,
    t.BuyerDelivery            = s.BuyerDelivery,
    t.BrandID                  = s.BrandID,
    t.OrderID                  = s.ID,
    t.Category                 = s.Category,
    t.OrderTypeID              = s.OrderTypeID,
    t.CustPoNo                 = s.CustPoNo,
    t.StyleID                  = s.StyleID,
    t.StyleName                = s.StyleName,
    t.SeasonID                 = s.SeasonID,
    t.Dest                     = s.Dest,
    t.Customize1               = s.Customize1,
    t.CustCDID                 = s.CustCDID,
    t.Seq                      = s.Seq,
    t.ShipModeID               = s.ShipModeID,
    t.ColorWay                 = s.ColorWay,
    t.SewLine                  = s.SewLine,
    t.TtlCtn                   = s.TtlCtn,
    t.StaggeredCtn             = s.StaggeredCtn,
    t.ClogCtn                  = s.ClogCtn,
    t.[ClogCtn%]               = s.[ClogCtn%],
    t.LastCartonReceivedDate   = s.LastCartonReceivedDate,
    t.CFAFinalInspectDate      = s.CFAFinalInspectDate,
    t.CFA3rdInspectDate        = s.CFA3rdInspectDate,
    t.CFARemark                = s.CFARemark,
    t.BIFactoryID              = @BIFactoryID,
    t.BIInsertDate             = GETDATE()
FROM P_QA_R31 t 
INNER JOIN #tmp s 
    ON  t.OrderID    = s.ID 
    AND t.Stage      = s.Stage 
    AND t.StyleName  = s.StyleName 
    AND t.Seq        = s.Seq


INSERT INTO P_QA_R31 (
    Stage, InspResult, [NotYetInspCtn#], NotYetInspCtn, NotYetInspQty,
    [FailCtn#], FailCtn, FailQty, MDivisionID, FactoryID,
    BuyerDelivery, BrandID, OrderID, Category, OrderTypeID,
    CustPoNo, StyleID, StyleName, SeasonID, Dest,
    Customize1, CustCDID, Seq, ShipModeID, ColorWay,
    SewLine, TtlCtn, StaggeredCtn, ClogCtn, [ClogCtn%],
    LastCartonReceivedDate, CFAFinalInspectDate, CFA3rdInspectDate, CFARemark,
    BIFactoryID, BIInsertDate
)
SELECT  
    s.Stage, s.InspResult, s.[NotYetInspCtn#], s.NotYetInspCtn, s.NotYetInspQty,
    s.[FailCtn#], s.FailCtn, s.FailQty, s.MDivisionID, s.FactoryID,
    s.BuyerDelivery, s.BrandID, s.ID, s.Category, s.OrderTypeID,
    s.CustPoNo, s.StyleID, s.StyleName, s.SeasonID, s.Dest,
    s.Customize1, s.CustCDID, s.Seq, s.ShipModeID, s.ColorWay,
    s.SewLine, s.TtlCtn, s.StaggeredCtn, s.ClogCtn, s.[ClogCtn%],
    s.LastCartonReceivedDate, s.CFAFinalInspectDate, s.CFA3rdInspectDate, s.CFARemark,
    @BIFactoryID, GETDATE()
FROM #tmp s
WHERE NOT EXISTS (
    SELECT 1 
    FROM P_QA_R31 t 
    WHERE t.OrderID    = s.ID 
      AND t.Stage      = s.Stage 
      AND t.StyleName  = s.StyleName 
      AND t.Seq        = s.Seq
)

INSERT INTO P_QA_R31_History (Ukey, FactoryID, BIFactoryID, BIInsertDate)
SELECT 
    t.Ukey,
    t.FactoryID,
    t.BIFactoryID,
    GETDATE()
FROM P_QA_R31 t
WHERE NOT EXISTS (
    SELECT 1 
    FROM #tmp s 
    WHERE t.OrderID   = s.ID 
      AND t.Stage     = s.Stage 
      AND t.StyleName = s.StyleName 
      AND t.Seq       = s.Seq
)
AND t.BuyerDelivery BETWEEN @SDate AND @EDate
AND EXISTS (
    SELECT 1 
    FROM Production.dbo.Factory f 
    WHERE f.ID = t.FactoryID 
      AND f.ProduceM = f.MDivisionID
)


DELETE t
FROM dbo.P_QA_R31 t
WHERE NOT EXISTS (
    SELECT 1 
    FROM #tmp s 
    WHERE t.OrderID   = s.ID 
      AND t.Stage     = s.Stage 
      AND t.StyleName = s.StyleName 
      AND t.Seq       = s.Seq
)
AND t.BuyerDelivery BETWEEN @SDate AND @EDate
AND EXISTS (
    SELECT 1 
    FROM Production.dbo.Factory f
    WHERE f.ID = t.FactoryID 
      AND f.ProduceM = f.MDivisionID
)
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

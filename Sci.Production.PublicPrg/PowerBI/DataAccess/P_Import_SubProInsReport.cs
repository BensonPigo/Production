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
    public class P_Import_SubProInsReport
    {
        /// <inheritdoc/>
        public Base_ViewModel P_SubProInsReport(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            QA_R51 biModel = new QA_R51();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-90).ToString("yyyy/MM/01"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Now;
            }

            try
            {
                QA_R51_ViewModel qa_R51_ViewModel = new QA_R51_ViewModel()
                {
                    StartInspectionDate = item.SDate,
                    EndInspectionDate = item.EDate,
                    FormatType = "DefectType",
                    M = string.Empty,
                    Factory = string.Empty,
                    Shift = string.Empty,
                    SP = string.Empty,
                    Style = string.Empty,
                    SubProcess = string.Empty,
                    IsBI = true,
                };

                Base_ViewModel resultReport = biModel.Get_QA_R51(qa_R51_ViewModel);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.DtArr[0];

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

        /// <inheritdoc/>
        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"
IF @IsTrans = 1
BEGIN
    INSERT INTO P_SubProInsReport_History (
        FactoryID, SubProInsRecordUkey, DefectCode, BIFactoryID, BIInsertDate
    )
    SELECT 
        p.FactoryID, p.SubProInsRecordUkey, ISNULL(p.DefectCode, ''), p.BIFactoryID, GETDATE()
    FROM P_SubProInsReport p
    LEFT JOIN #tmp t 
        ON p.FactoryID = t.FactoryID 
       AND p.SubProInsRecordUkey = t.SubProInsRecordUkey 
       AND ISNULL(p.DefectCode, '') = ISNULL(t.DefectCode, '')
    WHERE (p.AddDate BETWEEN @StartDate AND @EndDate
        OR p.EditDate BETWEEN @StartDate AND @EndDate)
      AND t.SubProInsRecordUkey IS NULL
END

DELETE p
FROM P_SubProInsReport p
LEFT JOIN #tmp t 
    ON p.FactoryID = t.FactoryID 
   AND p.SubProInsRecordUkey = t.SubProInsRecordUkey 
   AND ISNULL(p.DefectCode, '') = ISNULL(t.DefectCode, '')
WHERE (p.AddDate BETWEEN @StartDate AND @EndDate
    OR p.EditDate BETWEEN @StartDate AND @EndDate)
  AND t.SubProInsRecordUkey IS NULL


UPDATE p
SET
    p.SubProLocationID = ISNULL(t.SubProLocationID, ''),
    p.InspectionDate = t.InspectionDate,
    p.SewInLine = t.SewInLine,
    p.SewinglineID = ISNULL(t.SewinglineID, ''),
    p.Shift = ISNULL(t.Shift, ''),
    p.RFT = CONVERT(NUMERIC(6,2), ISNULL(t.RFT, 0)),
    p.SubProcessID = ISNULL(t.SubProcessID, ''),
    p.BundleNo = ISNULL(t.BundleNo, ''),
    p.Artwork = ISNULL(t.Artwork, ''),
    p.OrderID = ISNULL(t.OrderID, ''),
    p.Alias = ISNULL(t.Alias, ''),
    p.BuyerDelivery = t.BuyerDelivery,
    p.BundleGroup = ISNULL(t.BundleGroup, 0),
    p.SeasonID = ISNULL(t.SeasonID, ''),
    p.StyleID = ISNULL(t.StyleID, ''),
    p.ColorID = ISNULL(t.ColorID, ''),
    p.SizeCode = ISNULL(t.SizeCode, ''),
    p.PatternDesc = ISNULL(t.PatternDesc, ''),
    p.Item = ISNULL(t.Item, ''),
    p.Qty = ISNULL(t.Qty, 0),
    p.RejectQty = ISNULL(t.RejectQty, 0),
    p.Machine = ISNULL(t.Machine, ''),
    p.Serial = ISNULL(t.Serial, ''),
    p.Junk = ISNULL(t.Junk, 0),
    p.Description = ISNULL(t.Description, ''),
    p.DefectQty = ISNULL(t.DefectQty, 0),
    p.Inspector = ISNULL(t.Inspector, ''),
    p.Remark = ISNULL(t.Remark, ''),
    p.AddDate = t.AddDate2,
    p.RepairedDatetime = t.RepairedDatetime,
    p.RepairedTime = ISNULL(t.RepairedTime, 0),
    p.ResolveTime = ISNULL(t.ResolveTime, 0),
    p.SubProResponseTeamID = ISNULL(t.SubProResponseTeamID, ''),
    p.CustomColumn1 = ISNULL(t.CustomColumn1, ''),
    p.MDivisionID = ISNULL(t.MDivisionID, ''),
    p.OperatorID = ISNULL(t.OperatorID, ''),
    p.OperatorName = ISNULL(t.OperatorName, ''),
    p.BIFactoryID = @BIFactoryID,
    p.BIInsertDate = GETDATE(),
    p.BIStatus = 'NEW',
    p.EditDate = t.EditDate
FROM P_SubProInsReport p
JOIN #tmp t 
    ON p.FactoryID = t.FactoryID 
   AND p.SubProInsRecordUkey = t.SubProInsRecordUkey 
   AND ISNULL(p.DefectCode, '') = ISNULL(t.DefectCode, '')


INSERT INTO P_SubProInsReport (
    FactoryID, SubProLocationID, InspectionDate, SewInLine, SewinglineID, Shift, RFT,
    SubProcessID, BundleNo, Artwork, OrderID, Alias, BuyerDelivery, BundleGroup,
    SeasonID, StyleID, ColorID, SizeCode, PatternDesc, Item, Qty, RejectQty, Machine,
    Serial, Junk, Description, DefectCode, DefectQty, Inspector, Remark, AddDate,
    RepairedDatetime, RepairedTime, ResolveTime, SubProResponseTeamID, CustomColumn1,
    MDivisionID, OperatorID, OperatorName, BIFactoryID, BIInsertDate, SubProInsRecordUkey, EditDate, BIStatus
)
SELECT
    ISNULL(FactoryID, ''),
    ISNULL(SubProLocationID, ''),
    InspectionDate,
    SewInLine,
    ISNULL(SewinglineID, ''),
    ISNULL(Shift, ''),
    ISNULL(CONVERT(NUMERIC(6,2), RFT), 0),
    ISNULL(SubProcessID, ''),
    ISNULL(BundleNo, ''),
    ISNULL(Artwork, ''),
    ISNULL(OrderID, ''),
    ISNULL(Alias, ''),
    BuyerDelivery,
    ISNULL(BundleGroup, 0),
    ISNULL(SeasonID, ''),
    ISNULL(StyleID, ''),
    ISNULL(ColorID, ''),
    ISNULL(SizeCode, ''),
    ISNULL(PatternDesc, ''),
    ISNULL(Item, ''),
    ISNULL(Qty, 0),
    ISNULL(RejectQty, 0),
    ISNULL(Machine, ''),
    ISNULL(Serial, ''),
    ISNULL(Junk, 0),
    ISNULL(Description, ''),
    ISNULL(DefectCode, ''),
    ISNULL(DefectQty, 0),
    ISNULL(Inspector, ''),
    ISNULL(Remark, ''),
    AddDate2,
    RepairedDatetime,
    ISNULL(RepairedTime, 0),
    ISNULL(ResolveTime, 0),
    ISNULL(SubProResponseTeamID, ''),
    ISNULL(CustomColumn1, ''),
    ISNULL(MDivisionID, ''),
    ISNULL(OperatorID, ''),
    ISNULL(OperatorName, ''),
    @BIFactoryID,
    GETDATE(),
    ISNULL(SubProInsRecordUkey, 0),
    EditDate,
    'New' 
FROM #tmp t
WHERE NOT EXISTS (
    SELECT 1
    FROM P_SubProInsReport p  
    WHERE p.FactoryID = ISNULL(t.FactoryID, '')  -- 保持與插入值一致
      AND p.SubProInsRecordUkey = ISNULL(t.SubProInsRecordUkey, 0) -- 保持與插入值一致
      AND p.DefectCode = ISNULL(t.DefectCode, '') -- 保持與插入值一致
)
";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@StartDate", item.SDate),
                    new SqlParameter("@EndDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                finalResult = new Base_ViewModel()
                {
                     Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sqlcmd, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

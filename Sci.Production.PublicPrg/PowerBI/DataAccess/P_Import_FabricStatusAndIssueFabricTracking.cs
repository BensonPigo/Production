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
    public class P_Import_FabricStatusAndIssueFabricTracking
    {
        /// <inheritdoc/>
        public Base_ViewModel P_FabricStatusAndIssueFabricTracking(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R04 biModel = new PPIC_R04();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd"));
            }

            try
            {
                PPIC_R04_ViewModel ppic_R04 = new PPIC_R04_ViewModel()
                {
                    LeadTime = 10800,
                    BIEditDate = item.SDate.Value.ToString("yyyy/MM/dd"),
                    IsPowerBI = true,

                    ReportType = string.Empty,
                    ApvDate1 = null,
                    ApvDate2 = null,
                    MDivisionID = string.Empty,
                    FactoryID = string.Empty,
                };

                Base_ViewModel resultReport = biModel.GetPPIC_R04Data(ppic_R04);
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
                    new SqlParameter("@BIFactoryID", item.RgCode),
                };
                string sql = $@"	
UPDATE t
SET 
    t.SewingCell              = s.SewingCell,
    t.LineID                  = s.SewingLineID,
    t.Department              = s.Department,
    t.StyleID                 = s.StyleID,
    t.FabricType              = s.FabricType,
    t.Color                   = s.ColorName,
    t.ApvDate                 = s.ApvDate,
    t.NoOfPcsRejected         = s.RejectQty,
    t.RequestQtyYrds          = s.RequestQty,
    t.IssueQtyYrds            = s.IssueQty,
    t.ReplacementFinishedDate = s.FinishedDate,
    t.Type                    = s.Type,
    t.Process                 = s.Process,
    t.Description             = s.Description,
    t.OnTime                  = s.OnTime,
    t.Remark                  = s.Remark,
    t.BIFactoryID             = @BIFactoryID,
    t.BIInsertDate            = GETDATE(),
    t.DetailRemark            = s.DetailRemark,
    t.StyleName               = s.StyleName,
    t.MaterialType            = s.MaterialType,
    t.SewingQty               = s.SewingQty
FROM P_FabricStatus_And_IssueFabricTracking t
INNER JOIN #tmp s 
    ON t.ReplacementID = s.ID 
    AND t.SP = s.OrderID 
    AND t.Seq = s.Seq 
    AND t.RefNo = s.RefNo



INSERT INTO P_FabricStatus_And_IssueFabricTracking (
    SewingCell, LineID, ReplacementID, Department, StyleID, SP, Seq, FabricType,
    Color, RefNo, ApvDate, NoOfPcsRejected, RequestQtyYrds, IssueQtyYrds,
    ReplacementFinishedDate, Type, Process, Description, OnTime, Remark,
    BIFactoryID, BIInsertDate, DetailRemark, StyleName, MaterialType, SewingQty, FactoryID
)
SELECT 
    s.SewingCell, s.SewingLineID, s.ID, s.Department, s.StyleID, s.OrderID, s.Seq, s.FabricType,
    s.ColorName, s.RefNo, s.ApvDate, s.RejectQty, s.RequestQty, s.IssueQty,
    s.FinishedDate, s.Type, s.Process, s.Description, s.OnTime, s.Remark,
    @BIFactoryID, GETDATE(), s.DetailRemark, s.StyleName, s.MaterialType, s.SewingQty, s.FactoryID
FROM #tmp s
WHERE NOT EXISTS (
    SELECT 1 
    FROM P_FabricStatus_And_IssueFabricTracking t 
    WHERE t.ReplacementID = s.ID 
      AND t.SP = s.OrderID 
      AND t.Seq = s.Seq 
      AND t.RefNo = s.RefNo
      AND t.FactoryID = s.FactoryID
)


INSERT INTO P_FabricStatus_And_IssueFabricTracking_History (ReplacementID, SP, Seq, RefNo, BIFactoryID, BIInsertDate)
SELECT ReplacementID, SP, Seq, RefNo, BIFactoryID, GETDATE()
FROM P_FabricStatus_And_IssueFabricTracking t
WHERE NOT EXISTS (
    SELECT 1 
    FROM #tmp s 
    WHERE t.ReplacementID = s.ID 
      AND t.SP = s.OrderID 
      AND t.Seq = s.Seq 
      AND t.RefNo = s.RefNo
      AND t.FactoryID = s.FactoryID
)
AND t.ReplacementFinishedDate >= @SDate

DELETE t 
FROM P_FabricStatus_And_IssueFabricTracking t
WHERE NOT EXISTS (
    SELECT 1 
    FROM #tmp s 
    WHERE t.ReplacementID = s.ID 
      AND t.SP = s.OrderID 
      AND t.Seq = s.Seq 
      AND t.RefNo = s.RefNo
      AND t.FactoryID = s.FactoryID
)
AND t.ReplacementFinishedDate >= @SDate


INSERT INTO P_FabricStatus_And_IssueFabricTracking_History (ReplacementID, SP, Seq, RefNo, BIFactoryID, BIInsertDate)
SELECT ReplacementID, SP, Seq, RefNo, BIFactoryID, GETDATE()
FROM P_FabricStatus_And_IssueFabricTracking p
WHERE NOT EXISTS (
    SELECT 1 
    FROM mainserver.production.dbo.lack_detail l
    WHERE p.ReplacementID = l.ID
      AND (l.Seq1 + ' ' + l.Seq2) = p.Seq
)

DELETE p
FROM P_FabricStatus_And_IssueFabricTracking p
WHERE NOT EXISTS (
    SELECT 1 
    FROM mainserver.production.dbo.lack_detail l
    WHERE p.ReplacementID = l.ID
      AND (l.Seq1 + ' ' + l.Seq2) = p.Seq
)
 
INSERT INTO P_FabricStatus_And_IssueFabricTracking_History (ReplacementID, SP, Seq, RefNo, BIFactoryID, BIInsertDate)
SELECT ReplacementID, SP, Seq, RefNo, BIFactoryID, GETDATE()
FROM P_FabricStatus_And_IssueFabricTracking p
WHERE NOT EXISTS (
    SELECT 1 
    FROM mainserver.production.dbo.Lack l
    WHERE l.ID = p.ReplacementID
      AND l.OrderID = p.SP
)

DELETE p
FROM P_FabricStatus_And_IssueFabricTracking p
WHERE NOT EXISTS (
    SELECT 1 
    FROM mainserver.production.dbo.Lack l
    WHERE l.ID = p.ReplacementID
      AND l.OrderID = p.SP
)

UPDATE p
SET 
    p.SewingQty = SewingQty.val,
    p.BIInsertDate = GETDATE()
FROM P_FabricStatus_And_IssueFabricTracking p
INNER JOIN Production.dbo.Orders o ON p.SP = o.ID
OUTER APPLY (
    SELECT val = SUM(minSewQty.val)
    FROM (
        SELECT 
            oq.Article,
            oq.SizeCode,
            sl.Location AS ComboType,
            val = SUM(ISNULL(sdd.QAQty, 0))
        FROM Production.dbo.Orders oop WITH (NOLOCK)
        INNER JOIN Production.dbo.Order_Location sl WITH (NOLOCK) ON sl.OrderId = oop.ID
        INNER JOIN Production.dbo.Order_Qty oq WITH (NOLOCK) ON oq.ID = oop.ID
        LEFT JOIN Production.dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK)
            ON sdd.OrderId = oop.ID
            AND sdd.Article = oq.Article
            AND sdd.SizeCode = oq.SizeCode
            AND sdd.ComboType = sl.Location
        WHERE oop.POID = o.POID
        GROUP BY oq.Article, oq.SizeCode, sl.Location
    ) minSewQty
) SewingQty
WHERE p.SewingQty <> SewingQty.val

UPDATE p
	 SET p.SewingQty = SewingQty.val
		, p.BIInsertDate = GETDATE()
FROM P_FabricStatus_And_IssueFabricTracking p
INNER JOIN Production.dbo.Orders o ON p.[SP] = o.id
OUTER APPLY
(
	SELECT 
    val = SUM(minSewQty.val)
	FROM
	(
		SELECT 
			oq.Article,
			oq.SizeCode,
			sl.Location AS ComboType,
			val = sum(ISNULL(sdd.QAQty, 0))
		FROM Production.dbo.Orders oop WITH (NOLOCK) 
		INNER JOIN Production.dbo.Order_Location sl WITH (NOLOCK) ON sl.OrderId =oop.ID
		INNER JOIN Production.dbo.Order_Qty oq WITH (NOLOCK) ON oq.ID = oop.ID
		LEFT JOIN Production.dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
			ON sdd.OrderId = oop.ID 
			AND sdd.Article = oq.Article 
			AND sdd.SizeCode = oq.SizeCode 
			AND sdd.ComboType = sl.Location
		WHERE oop.POID = o.POID
		GROUP BY oq.Article, oq.SizeCode, sl.Location
	) minSewQty
)SewingQty
WHERE p.SewingQty <> SewingQty.val
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

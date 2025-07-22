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
    public class P_Import_ProductionStatus
    {
        /// <inheritdoc/>
        public Base_ViewModel P_ProductionStatus(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.GetProductionStatus_Data(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

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

        private Base_ViewModel GetProductionStatus_Data(ExecutedList item)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@StartDate", item.SDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sql = @"
SELECT 
    sch.SewingLineID,
    sch.FactoryID,
    sch.SPNo,
    [StyleID] = ISNULL(sch.StyleID, ''),
    [StyleName] = ISNULL(sty.StyleName, ''),
    sch.ComboType,
    [SPCategory] = ISNULL(ord.Category, ''),
    sch.SciDelivery,
    sch.BuyerDelivery,
    [InlineDate] = sch.[Inline],
    [OfflineDate] = sch.[Offline],
    [OrderQty] = ISNULL(sch.OrderQty, 0),
    [AlloQty] = ISNULL(sch.AlloQty, 0),
    [SewingQty] = ISNULL(sch.SewingQty, 0),
	[SewingBalance] = ISNULL(t.SewingBalance, 0),
    [TtlSewingQtyByComboType] = ISNULL(schCombo.TtlSewingQtyByComboType, 0),
    [TtlSewingQtyBySP]= ISNULL(schSP.TtlSewingQtyBySP, 0),
    [ClogQty] = ISNULL(sch.ClogQty, 0),
	[TtlClogBalance] = ISNULL(t.TtlClogBalance, 0),
	[DaysOffToDDSched] = IIF(ISNULL(t.TtlClogBalance, 0) = 0, 'X', CAST(t.DaysOffToDDSched AS VARCHAR(8))),
    [DaysTodayToDD] = IIF(ISNULL(t.TtlClogBalance, 0) = 0, 'X', CAST(t.DaysTodayToDD AS VARCHAR(8))),
	[NeedQtyByStdOut] = IIF(ISNULL(t.TtlClogBalance, 0) = 0, 'X', CAST(t2.NeedQtyByStdOut AS VARCHAR(8))),
	[Pending] = ISNULL(t2.Pending, ''),
    [TotalStandardOutput] = ISNULL(sch.TotalStandardOutput, 0),
    [DaysToDrainByStdOut] = IIF(ISNULL(t.TtlClogBalance, 0) = 0, 'X', CAST(t2.DaysToDrainByStdOut AS VARCHAR(8))),
	[OfflineDateByStdOut] = IIF(ISNULL(t.TtlClogBalance, 0) = 0, NULL, t3.OfflineDateByStdOut),
	[DaysOffToDDByStdOut] = IIF(ISNULL(t.TtlClogBalance, 0) = 0, 'X', CAST(t4.DaysOffToDDByStdOut AS VARCHAR(8))),
    [MaxOutput] = IIF(t.MaxOutput IS NULL, 'X', CAST(t.MaxOutput AS VARCHAR(8))),
    [DaysToDrainByMaxOut] = IIF(ISNULL(t.TtlClogBalance, 0) = 0, 'X', CAST(t2.DaysToDrainByMaxOut AS VARCHAR(8))),
	[OfflineDateByMaxOut] = IIF(ISNULL(t.TtlClogBalance, 0) = 0, NULL, t3.OfflineDateByMaxOut),
    [DaysOffToDDByMaxOut] = IIF(ISNULL(t.TtlClogBalance, 0) = 0, 'X', CAST(t4.DaysOffToDDByMaxOut AS VARCHAR(8))),
    [TightByMaxOut] = ISNULL(t5.TightByMaxOut, ''),
    [TightByStdOut] = ISNULL(t5.TightByStdOut, ''),
    [BIFactoryID] = @BIFactoryID,
    [BIInsertDate] = GETDATE()
FROM dbo.P_SewingLineScheduleBySP sch WITH (NOLOCK)
INNER JOIN Production.dbo.Orders ord WITH (NOLOCK) ON sch.SPNo = ord.ID
INNER JOIN Production.dbo.Style sty WITH (NOLOCK) ON ord.StyleUkey = sty.Ukey
OUTER APPLY (
    SELECT [TtlSewingQtyByComboType] = SUM(s2.SewingQty)
    FROM dbo.P_SewingLineScheduleBySP s2 WITH (NOLOCK)
    WHERE sch.SPNo = s2.SPNo AND sch.ComboType = s2.ComboType
    GROUP BY s2.SPNo, s2.ComboType
) schCombo
OUTER APPLY (
    SELECT [TtlSewingQtyBySP] = MIN(s3.SewingQty)
    FROM (
        SELECT s3.SPNo, s3.ComboType
            , [SewingQty] = SUM(s3.SewingQty)
        FROM dbo.P_SewingLineScheduleBySP s3 WITH (NOLOCK)
        WHERE sch.SPNo = s3.SPNo
        GROUP BY s3.SPNo, s3.ComboType
    ) s3
    GROUP BY s3.SPNo
) schSP
LEFT JOIN (
    SELECT 
        StyleID,
        SewingLineID,
        TotalOutputQty = MAX(TotalOutputQty)
    FROM dbo.P_SewingDailyOutput WITH (NOLOCK)
    WHERE OutputDate >= DATEFROMPARTS(YEAR(GETDATE()) - 1, 1, 1)
      AND Category = 'Bulk'
    GROUP BY StyleID, SewingLineID
) sewOutput ON sch.StyleID = sewOutput.StyleID AND sch.SewingLineID = sewOutput.SewingLineID
CROSS APPLY (
    SELECT 
        SewingBalance     = IIF(sch.AlloQty - sch.SewingQty < 0, 0, sch.AlloQty - sch.SewingQty),
		TtlClogBalance	  = IIF(ord.Category = 'S' OR sch.OrderQty - schSP.TtlSewingQtyBySP < 0, 0, sch.OrderQty - schSP.TtlSewingQtyBySP),
        DaysOffToDDSched  = Production.dbo.CalculateWorkDayByWorkHour(sch.[Offline], sch.BuyerDelivery, sch.FactoryID, sch.SewingLineID),
        DaysTodayToDD     = Production.dbo.CalculateWorkDayByWorkHour(sch.BIInsertDate, sch.BuyerDelivery, sch.FactoryID, sch.SewingLineID),
        MaxOutput         = IIF(ISNULL(sewOutput.TotalOutputQty, 0) < 20, NULL, sewOutput.TotalOutputQty)
) t
CROSS APPLY (
    SELECT 
        NeedQtyByStdOut       = CEILING(IIF(t.DaysTodayToDD > 0, t.SewingBalance * 1.0 / t.DaysTodayToDD, t.SewingBalance * 1.0)),
        Pending               = IIF(sch.[Offline] < sch.BIInsertDate AND t.SewingBalance > 0, 'Y', 'N'),
        DaysToDrainByStdOut   = CEILING(IIF(sch.TotalStandardOutput = 0, 0, t.SewingBalance * 1.0 / sch.TotalStandardOutput)),
        DaysToDrainByMaxOut   = CEILING(IIF(ISNULL(t.MaxOutput, 0) = 0, 0, t.SewingBalance * 1.0 / t.MaxOutput))
) t2
CROSS APPLY (
    SELECT 
        OfflineDateByStdOut = IIF(t2.DaysToDrainByStdOut > 30, sch.[Offline], sch.BIInsertDate + t2.DaysToDrainByStdOut),
        OfflineDateByMaxOut = IIF(t2.DaysToDrainByMaxOut > 30, sch.[Offline],
                                  IIF(t.MaxOutput IS NULL, sch.[Offline], sch.BIInsertDate + t2.DaysToDrainByMaxOut))
) t3
CROSS APPLY (
    SELECT 
        DaysOffToDDByStdOut = DATEDIFF(DAY, t3.OfflineDateByStdOut, sch.BuyerDelivery) - 1,
        DaysOffToDDByMaxOut = DATEDIFF(DAY, t3.OfflineDateByMaxOut, sch.BuyerDelivery) - 1
) t4
CROSS APPLY (
    SELECT 
        TightByMaxOut = IIF(t4.DaysOffToDDByMaxOut <= 3, 'Y', 'N'),
        TightByStdOut = IIF(t4.DaysOffToDDByStdOut <= 3, 'Y', 'N')
) t5
WHERE sch.BIInsertDate >= DATEADD(DAY, -7, @StartDate)
AND NOT EXISTS (SELECT 1 FROM Production.dbo.Factory f WITH (NOLOCK) WHERE f.ID = sch.FactoryID AND f.IsSampleRoom = 1);
";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("PowerBI", sql, listPar, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                string sql = @"	
if @IsTrans = 1
begin
    INSERT INTO P_ProdctionStatus_History([SewingLineID], [FactoryID], [SPNO], [ComboType], [InlineDate], [OfflineDate], [BIFactoryID], [BIInsertDate])
    SELECT [SewingLineID], [FactoryID], [SPNO], [ComboType], [InlineDate], [OfflineDate], [BIFactoryID], GETDATE()
    FROM P_ProdctionStatus ps
    WHERE NOT EXISTS (
	    SELECT 1 FROM P_SewingLineScheduleBySP sch 
	    WHERE sch.SewingLineID = ps.SewingLineID
	    AND sch.FactoryID = ps.FactoryID
	    AND sch.SPNo = ps.SPNO
	    AND sch.ComboType = ps.ComboType
	    AND sch.Inline = ps.InlineDate
	    AND sch.Offline = ps.OfflineDate)
end

DELETE ps
FROM P_ProdctionStatus ps
WHERE NOT EXISTS (
	SELECT 1 FROM P_SewingLineScheduleBySP sch 
	WHERE sch.SewingLineID = ps.SewingLineID
	AND sch.FactoryID = ps.FactoryID
	AND sch.SPNo = ps.SPNO
	AND sch.ComboType = ps.ComboType
	AND sch.Inline = ps.InlineDate
	AND sch.Offline = ps.OfflineDate)

UPDATE P
	SET p.[StyleID]					= ISNULL(t.[StyleID], '')
	, p.[StyleName]					= ISNULL(t.[StyleName], '')
	, p.[SPCategory]				= ISNULL(t.[SPCategory], '')
	, p.[SCIDelivery]				= t.[SCIDelivery]
	, p.[BuyerDelivery]				= t.[BuyerDelivery]
	, p.[OrderQty]					= ISNULL(t.[OrderQty], 0)
	, p.[AlloQty]					= ISNULL(t.[AlloQty], 0)
	, p.[SewingQty]					= ISNULL(t.[SewingQty], 0)
	, p.[SewingBalance]				= ISNULL(t.[SewingBalance], 0)
	, p.[TtlSewingQtyByComboType]	= ISNULL(t.[TtlSewingQtyByComboType], 0)
	, p.[TtlSewingQtyBySP]			= ISNULL(t.[TtlSewingQtyBySP], 0)
	, p.[ClogQty]					= ISNULL(t.[ClogQty], 0)
	, p.[TtlClogBalance]			= ISNULL(t.[TtlClogBalance], 0)
	, p.[DaysOffToDDSched]			= ISNULL(t.[DaysOffToDDSched], '')
	, p.[DaysTodayToDD]				= ISNULL(t.[DaysTodayToDD], '')
	, p.[NeedQtyByStdOut]			= ISNULL(t.[NeedQtyByStdOut], '')
	, p.[Pending]					= ISNULL(t.[Pending], '')
	, p.[TotalStandardOutput]		= ISNULL(t.[TotalStandardOutput], 0)
	, p.[DaysToDrainByStdOut]		= ISNULL(t.[DaysToDrainByStdOut], '')
	, p.[OfflineDateByStdOut]		= t.[OfflineDateByStdOut]
	, p.[DaysOffToDDByStdOut]		= ISNULL(t.[DaysOffToDDByStdOut], '')
	, p.[MaxOutput]					= ISNULL(t.[MaxOutput], '')
	, p.[DaysToDrainByMaxOut]		= ISNULL(t.[DaysToDrainByMaxOut], '')
	, p.[OfflineDateByMaxOut]		= t.[OfflineDateByMaxOut]
	, p.[DaysOffToDDByMaxOut]		= ISNULL(t.[DaysOffToDDByMaxOut], '')
	, p.[TightByMaxOut]				= ISNULL(t.[TightByMaxOut], '')
	, p.[TightByStdOut]				= ISNULL(t.[TightByStdOut], '')
	, p.[BIFactoryID]				= ISNULL(t.[BIFactoryID], '')
	, p.[BIInsertDate]				= t.[BIInsertDate]
    , p.[BIStatus]                  = 'New'
FROM P_ProdctionStatus p
INNER JOIN #tmp t ON t.SewingLineID = p.SewingLineID
				AND t.FactoryID = p.FactoryID
				AND t.SPNo = p.SPNO
				AND t.ComboType = p.ComboType
				AND t.InlineDate = p.InlineDate
				AND t.OfflineDate = p.OfflineDate


INSERT INTO P_ProdctionStatus([SewingLineID], [FactoryID], [SPNO], [StyleID], [StyleName], [ComboType], [SPCategory], [SCIDelivery], [BuyerDelivery], [InlineDate], [OfflineDate], [OrderQty], [AlloQty], [SewingQty], [SewingBalance], [TtlSewingQtyByComboType], [TtlSewingQtyBySP], [ClogQty], [TtlClogBalance], [DaysOffToDDSched], [DaysTodayToDD], [NeedQtyByStdOut], [Pending], [TotalStandardOutput], [DaysToDrainByStdOut], [OfflineDateByStdOut], [DaysOffToDDByStdOut], [MaxOutput], [DaysToDrainByMaxOut], [OfflineDateByMaxOut], [DaysOffToDDByMaxOut], [TightByMaxOut], [TightByStdOut], [BIFactoryID], [BIInsertDate], [BIStatus])
SELECT 
ISNULL([SewingLineID], ''),
ISNULL([FactoryID], ''),
ISNULL([SPNO], ''),
ISNULL([StyleID], ''),
ISNULL([StyleName], ''),
ISNULL([ComboType], ''),
ISNULL([SPCategory], ''),
[SCIDelivery],
[BuyerDelivery],
[InlineDate],
[OfflineDate],
ISNULL([OrderQty], 0),
ISNULL([AlloQty], 0),
ISNULL([SewingQty], 0),
ISNULL([SewingBalance], 0),
ISNULL([TtlSewingQtyByComboType], 0),
ISNULL([TtlSewingQtyBySP], 0),
ISNULL([ClogQty], 0),
ISNULL([TtlClogBalance], 0),
ISNULL([DaysOffToDDSched], ''),
ISNULL([DaysTodayToDD], ''),
ISNULL([NeedQtyByStdOut], ''),
ISNULL([Pending], ''),
ISNULL([TotalStandardOutput], 0),
ISNULL([DaysToDrainByStdOut], ''),
[OfflineDateByStdOut],
ISNULL([DaysOffToDDByStdOut], ''),
ISNULL([MaxOutput], ''),
ISNULL([DaysToDrainByMaxOut], ''),
[OfflineDateByMaxOut],
ISNULL([DaysOffToDDByMaxOut], ''),
ISNULL([TightByMaxOut], ''),
ISNULL([TightByStdOut], ''),
ISNULL([BIFactoryID], ''),
GETDATE() ,
'New'
FROM #tmp t
WHERE NOT EXISTS (SELECT 1 FROM P_ProdctionStatus p WHERE t.SewingLineID = p.SewingLineID
														AND t.FactoryID = p.FactoryID
														AND t.SPNo = p.SPNO
														AND t.ComboType = p.ComboType
														AND t.InlineDate = p.InlineDate
														AND t.OfflineDate = p.OfflineDate)

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

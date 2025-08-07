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
    public class P_Import_SubprocessWIP
    {
        /// <inheritdoc/>
        public Base_ViewModel P_SubprocessWIP(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            SubCon_R41 biModel = new SubCon_R41();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
            }

            try
            {
                SubCon_R41_ViewModel subProcessWIP = new SubCon_R41_ViewModel()
                {
                    BIEditDate = item.SDate,
                    IsPowerBI = true,
                };

                finalResult = biModel.GetSubprocessWIPData(subProcessWIP);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(finalResult.Dt, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                // 用P_SubprocessWIP來更新SubprocessBCSByDays & SubprocessBCSByMonth
                finalResult = new P_Import_SubprocessBCSByDays().UpdateBIData(item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new P_Import_SubprocessBCSByMonth().UpdateBIData(item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
                item.ClassName = "P_SubprocessBCSByDays";
                finalResult = new Base().UpdateBIData(item);
                item.ClassName = "P_SubprocessBCSByMonth";
                finalResult = new Base().UpdateBIData(item);
                item.ClassName = "P_SubprocessWIP";
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
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                string sql = @"	
UPDATE t
SET 
	   t.[EXCESS] = ISNULL(s.[EXCESS], '')
      ,t.[FabricKind] = ISNULL(s.[FabricKind], '')
      ,t.[CutRef] = ISNULL(s.[CutRef], '')
      ,t.[Sp] = ISNULL(s.[Sp], '')
      ,t.[MasterSP] = ISNULL(s.[MasterSP], '')
      ,t.[M] = ISNULL(s.[M], '')
      ,t.[FactoryID] = ISNULL(s.[Factory], '')
      ,t.[Category] = ISNULL(s.[Category], '')
      ,t.[Program] = ISNULL(s.[Program], '')
      ,t.[Style] = ISNULL(s.[Style], '')
      ,t.[Season] = ISNULL(s.[Season], '')
      ,t.[Brand] = ISNULL(s.[Brand], '')
      ,t.[Comb] = ISNULL(s.[Comb], '')
      ,t.[CutNo] = ISNULL(s.[CutNo], 0)
      ,t.[FabPanelCode] = ISNULL(s.[FabPanelCode], '')
      ,t.[Article] = ISNULL(s.[Article], '')
      ,t.[Color] = ISNULL(s.[Color], '')
      ,t.[ScheduledLineID] = ISNULL(s.[ScheduledLineID], '')
      ,t.[ScannedLineID] = ISNULL(s.[ScannedLineID], '')
      ,t.[Cell] = ISNULL(s.[Cell], '')
      ,t.[Pattern] = ISNULL(s.[Pattern], '')
      ,t.[PtnDesc] = ISNULL(s.[PtnDesc], '')
      ,t.[Group] = ISNULL(s.[Group], 0)
      ,t.[Size] = ISNULL(s.[Size], '')
      ,t.[Artwork] = ISNULL(s.[Artwork], '')
      ,t.[Qty] = ISNULL(s.[Qty], 0)
      ,t.[SubprocessID] = ISNULL(s.[SubprocessID], '')
      ,t.[PostSewingSubProcess] = ISNULL(s.[PostSewingSubProcess], '')
      ,t.[NoBundleCardAfterSubprocess] = ISNULL(s.[NoBundleCardAfterSubprocess], '')
      ,t.[Location] = ISNULL(s.[Location], '')
      ,t.[BundleCreateDate] = s.[BundleCreateDate]
      ,t.[BuyerDeliveryDate] = s.[BuyerDeliveryDate]
      ,t.[SewingInline] = s.[SewingInline]
      ,t.[SubprocessQAInspectionDate] = s.[SubprocessQAInspectionDate]
      ,t.[InTime] = s.[InTime]
      ,t.[OutTime] = s.[OutTime]
      ,t.[POSupplier] = ISNULL(s.[POSupplier], '')
      ,t.[AllocatedSubcon] = ISNULL(s.[AllocatedSubcon], '')
      ,t.[AvgTime] = ISNULL(s.AvgTime, 0)
      ,t.[TimeRange] = ISNULL(s.[TimeRange], '')
      ,t.[EstimatedCutDate] = s.[EstimatedCutDate]
      ,t.[CuttingOutputDate] = s.CuttingOutputDate
      ,t.[Item] = ISNULL(s.Item, '')
      ,t.[PanelNo] = ISNULL(s.PanelNo, '')
      ,t.[CutCellID] = ISNULL(s.CutCellID, '')
      ,t.[SpreadingNo] = ISNULL(s.SpreadingNo, '')
      ,t.[LastSewDate] = s.LastSewDate
      ,t.[SewQty] = ISNULL(s.SewQty, 0)
      ,t.[BIFactoryID] = @BIFactoryID
      ,t.[BIInsertDate] = GETDATE()
      ,t.[BIStatus] = 'New'
from P_SubprocessWIP t 
inner join #tmp s on t.Bundleno = s.Bundleno
AND t.RFIDProcessLocationID = s.RFIDProcessLocationID 
AND t.SP = s.SP 
AND t.Pattern = s.Pattern
AND t.SubprocessID = s.SubprocessID

INSERT INTO P_SubprocessWIP (
   [Bundleno], [RFIDProcessLocationID], [EXCESS], [FabricKind], [CutRef], [Sp], [MasterSP],
   [M], [FactoryID], [Category], [Program], [Style], [Season], [Brand], [Comb], [CutNo], [FabPanelCode],
   [Article], [Color], [ScheduledLineID], [ScannedLineID], [Cell], [Pattern], [PtnDesc], [Group], [Size],
   [Artwork], [Qty], [SubprocessID], [PostSewingSubProcess], [NoBundleCardAfterSubprocess], [Location],
   [BundleCreateDate], [BuyerDeliveryDate], [SewingInline], [SubprocessQAInspectionDate], [InTime], [OutTime],
   [POSupplier], [AllocatedSubcon], [AvgTime], [TimeRange], [EstimatedCutDate], [CuttingOutputDate],
   [Item], [PanelNo], [CutCellID], [SpreadingNo], [LastSewDate], [SewQty], [BIFactoryID], [BIInsertDate], [BIStatus]
)
SELECT 
   ISNULL(s.[Bundleno], ''), ISNULL(s.[RFIDProcessLocationID], ''), ISNULL(s.[EXCESS], ''), ISNULL(s.[FabricKind], ''), ISNULL(s.[CutRef], ''),
   ISNULL(s.[Sp], ''), ISNULL(s.[MasterSP], ''), ISNULL(s.[M], ''), ISNULL(s.[Factory], ''), ISNULL(s.[Category], ''),
   ISNULL(s.[Program], ''), ISNULL(s.[Style], ''), ISNULL(s.[Season], ''), ISNULL(s.[Brand], ''), ISNULL(s.[Comb], ''),
   ISNULL(s.[Cutno], 0), ISNULL(s.[FabPanelCode], ''), ISNULL(s.[Article], ''), ISNULL(s.[Color], ''), ISNULL(s.[ScheduledLineID], ''),
   ISNULL(s.[ScannedLineID], ''), ISNULL(s.[Cell], ''), ISNULL(s.[Pattern], ''), ISNULL(s.[PtnDesc], ''), ISNULL(s.[Group], 0),
   ISNULL(s.[Size], ''), ISNULL(s.[Artwork], ''), ISNULL(s.[Qty], 0), ISNULL(s.[SubprocessID], ''), ISNULL(s.[PostSewingSubProcess], ''),
   ISNULL(s.[NoBundleCardAfterSubprocess], ''), ISNULL(s.[Location], ''), s.[BundleCreateDate], s.[BuyerDeliveryDate],
   s.[SewingInline], s.[SubprocessQAInspectionDate], s.[InTime], s.[OutTime], ISNULL(s.[POSupplier], ''),
   ISNULL(s.[AllocatedSubcon], ''), ISNULL(s.[AvgTime], 0), ISNULL(s.[TimeRange], ''), s.[EstimatedCutDate], s.[CuttingOutputDate],
   ISNULL(s.[Item], ''), ISNULL(s.[PanelNo], ''), ISNULL(s.[CutCellID], ''), ISNULL(s.[SpreadingNo], ''), s.[LastSewDate],
   ISNULL(s.[SewQty], 0), @BIFactoryID, GETDATE(), 'New'
FROM #tmp s
WHERE NOT EXISTS (
    SELECT 1 FROM P_SubprocessWIP t 
    WHERE t.Bundleno = s.Bundleno
    AND t.RFIDProcessLocationID = s.RFIDProcessLocationID 
    AND t.SP = s.SP 
    AND t.Pattern = s.Pattern
    AND t.SubprocessID = s.SubprocessID
)


-- delete P_SubprocessWIP 一次30萬筆分批刪除
WHILE 1 = 1
BEGIN
    if @IsTrans = 1
    begin
        INSERT INTO P_SubprocessWIP_History([Bundleno], [RFIDProcessLocationID], [Sp], [Pattern], [SubprocessID], [BIFactoryID], [BIInsertDate])
        SELECT TOP (300000) [Bundleno], [RFIDProcessLocationID], [Sp], [Pattern], [SubprocessID], [BIFactoryID], [BIInsertDate] = GetDate()
        FROM P_SubprocessWIP ps WITH (NOLOCK)
        WHERE NOT EXISTS (
            SELECT 1 FROM Production.dbo.Bundle_Detail bd WITH (NOLOCK) 
            WHERE ps.Bundleno = bd.BundleNo
        )
    end

    DELETE TOP (300000) ps
    FROM P_SubprocessWIP ps WITH (NOLOCK)
    WHERE NOT EXISTS (
        SELECT 1 FROM Production.dbo.Bundle_Detail bd WITH (NOLOCK) 
        WHERE ps.Bundleno = bd.BundleNo
    )

    IF @@ROWCOUNT = 0
        BREAK;
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

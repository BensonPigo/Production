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

                Base_ViewModel resultReport = biModel.GetSubprocessWIPData(subProcessWIP);
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
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            string where = @"  not exists (select 1 from #tmp t where p.[SP] = t.[SP] and p.[SeqNo] = t.[SeqNo] and p.[PackingListID] = t.[PackingListID] and p.[CtnNo] = t.[CtnNo])
                               and p.Buyerdelivery  >= @StartDate";

            string tmp = new Base().SqlBITableHistory("P_SubprocessWIP", "P_SubprocessWIP_History", "#tmp", where, false, false);

            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                };
                string sql = @"	
UPDATE t
SET 
	   t.[EXCESS] = s.[EXCESS]
      ,t.[FabricKind] = s.[FabricKind]
      ,t.[CutRef] = s.[CutRef]
      ,t.[Sp] = s.[Sp]
      ,t.[MasterSP] = s.[MasterSP]
      ,t.[M] = s.[M]
      ,t.[FactoryID] = s.[Factory]
      ,t.[Category] = s.[Category]
      ,t.[Program] = s.[Program]
      ,t.[Style] = s.[Style]
      ,t.[Season] = s.[Season]
      ,t.[Brand] = s.[Brand]
      ,t.[Comb] = s.[Comb]
      ,t.[CutNo] = s.[CutNo]
      ,t.[FabPanelCode] = s.[FabPanelCode]
      ,t.[Article] = s.[Article]
      ,t.[Color] = s.[Color]
      ,t.[ScheduledLineID] = s.[ScheduledLineID]
      ,t.[ScannedLineID] = s.[ScannedLineID]
      ,t.[Cell] = s.[Cell]
      ,t.[Pattern] = s.[Pattern]
      ,t.[PtnDesc] = s.[PtnDesc]
      ,t.[Group] = s.[Group]
      ,t.[Size] = s.[Size]
      ,t.[Artwork] = s.[Artwork]
      ,t.[Qty] = s.[Qty]
      ,t.[SubprocessID] = s.[SubprocessID]
      ,t.[PostSewingSubProcess] = s.[PostSewingSubProcess]
      ,t.[NoBundleCardAfterSubprocess] = s.[NoBundleCardAfterSubprocess]
      ,t.[Location] = s.[Location]
      ,t.[BundleCreateDate] = s.[BundleCreateDate]
      ,t.[BuyerDeliveryDate] = s.[BuyerDeliveryDate]
      ,t.[SewingInline] = s.[SewingInline]
      ,t.[SubprocessQAInspectionDate] = s.[SubprocessQAInspectionDate]
      ,t.[InTime] = s.[InTime]
      ,t.[OutTime] = s.[OutTime]
      ,t.[POSupplier] = s.[POSupplier]
      ,t.[AllocatedSubcon] = s.[AllocatedSubcon] 
      ,t.[AvgTime] = s.AvgTime
      ,t.[TimeRange] = s.[TimeRange]
      ,t.[EstimatedCutDate] = s.[EstimatedCutDate]
      ,t.[CuttingOutputDate] = s.CuttingOutputDate
      ,t.[Item] = s.Item
      ,t.[PanelNo] = s.PanelNo
      ,t.[CutCellID] = s.CutCellID
      ,t.[SpreadingNo] = s.SpreadingNo
      ,t.[LastSewDate] = s.LastSewDate
      ,t.[SewQty] = s.SewQty
      ,t.[BIFactoryID] = @BIFactoryID
      ,t.[BIInsertDate] = GETDATE()
from P_SubprocessWIP t 
inner join #tmp s on t.Bundleno = s.Bundleno
AND t.RFIDProcessLocationID = s.RFIDProcessLocationID 
AND t.SP = s.SP 
AND t.Pattern = s.Pattern
AND t.SubprocessID = s.SubprocessID


insert into P_SubprocessWIP (
   [Bundleno]   ,[RFIDProcessLocationID]   ,[EXCESS]  ,[FabricKind]  ,[CutRef]  ,[Sp]  ,[MasterSP]
      ,[M] ,[FactoryID],[Category],[Program],[Style],[Season],[Brand],[Comb],[CutNo],[FabPanelCode],[Article]
      ,[Color],[ScheduledLineID],[ScannedLineID],[Cell],[Pattern],[PtnDesc],[Group],[Size],[Artwork]
      ,[Qty],[SubprocessID],[PostSewingSubProcess],[NoBundleCardAfterSubprocess],[Location],[BundleCreateDate]
      ,[BuyerDeliveryDate],[SewingInline],[SubprocessQAInspectionDate],[InTime],[OutTime],[POSupplier]
      ,[AllocatedSubcon],[AvgTime],[TimeRange],[EstimatedCutDate],[CuttingOutputDate]
      ,[Item],[PanelNo],[CutCellID],[SpreadingNo],[LastSewDate],[SewQty],[BIFactoryID], [BIInsertDate]
)
select 	 s.[Bundleno] ,s.[RFIDProcessLocationID] ,s.[EXCESS] ,s.[FabricKind] ,s.[CutRef] ,
    s.[Sp] ,s.[MasterSP] ,s.[M] ,s.[Factory] ,s.[Category],s.[Program],s.[Style] ,s.[Season],
    s.[Brand],s.[Comb] ,s.[Cutno],s.[FabPanelCode] ,s.[Article] ,s.[Color] ,s.[ScheduledLineID] ,s.[ScannedLineID] ,
    s.[Cell] ,s.[Pattern] ,s.[PtnDesc] ,s.[Group] ,s.[Size] ,s.[Artwork] ,s.[Qty] ,s.[SubprocessID] ,
    s.[PostSewingSubProcess] ,s.[NoBundleCardAfterSubprocess] ,s.[Location] ,s.[BundleCreateDate],
    s.[BuyerDeliveryDate],s.[SewingInline],s.[SubprocessQAInspectionDate],s.[InTime],s.[OutTime],s.[POSupplier] ,
    s.[AllocatedSubcon] ,s.AvgTime ,s.[TimeRange],s.[EstimatedCutDate],s.CuttingOutputDate,	s.Item 
	,s.PanelNo	,s.CutCellID     ,s.SpreadingNo     ,s.[LastSewDate]     ,s.[SewQty] , @BIFactoryID, GETDATE()
from #tmp s
where not exists (
    select 1 from P_SubprocessWIP t 
    where t.Bundleno = s.Bundleno
    AND t.RFIDProcessLocationID = s.RFIDProcessLocationID 
    AND t.SP = s.SP 
    AND t.Pattern = s.Pattern
    AND t.SubprocessID = s.SubprocessID
)

-- delete P_SubprocessWIP 一次30萬筆分批刪除
WHILE 1 = 1
BEGIN

    INSERT INTO P_SubprocessWIP_History([Bundleno], [RFIDProcessLocationID], [Sp], [Pattern], [SubprocessID], [BIFactoryID], [BIInsertDate])
    SELECT TOP (300000) [Bundleno], [RFIDProcessLocationID], [Sp], [Pattern], [SubprocessID], [BIFactoryID], [BIInsertDate] = GetDate()
    FROM P_SubprocessWIP ps WITH (NOLOCK)
    WHERE NOT EXISTS (
        SELECT 1 FROM Production.dbo.Bundle_Detail bd WITH (NOLOCK) 
        WHERE ps.Bundleno = bd.BundleNo
    )

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

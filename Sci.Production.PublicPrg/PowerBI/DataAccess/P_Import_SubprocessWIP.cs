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
        public Base_ViewModel P_SubprocessWIP(DateTime? sDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            SubCon_R41 biModel = new SubCon_R41();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
            }

            try
            {
                SubCon_R41_ViewModel subProcessWIP = new SubCon_R41_ViewModel()
                {
                    BIEditDate = sDate,
                    IsPowerBI = true,
                };

                Base_ViewModel resultReport = biModel.GetSubprocessWIPData(subProcessWIP);
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
                finalResult = this.UpdateBIData(detailTable, sDate.Value);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                // 用P_SubprocessWIP來更新SubprocessBCSByDays & SubprocessBCSByMonth
                finalResult = new P_Import_SubprocessBCSByDays().UpdateBIData();
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new P_Import_SubprocessBCSByMonth().UpdateBIData();
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

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
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
      ,[Item],[PanelNo],[CutCellID],[SpreadingNo],[LastSewDate],[SewQty]
)
select 	 s.[Bundleno] ,s.[RFIDProcessLocationID] ,s.[EXCESS] ,s.[FabricKind] ,s.[CutRef] ,
    s.[Sp] ,s.[MasterSP] ,s.[M] ,s.[Factory] ,s.[Category],s.[Program],s.[Style] ,s.[Season],
    s.[Brand],s.[Comb] ,s.[Cutno],s.[FabPanelCode] ,s.[Article] ,s.[Color] ,s.[ScheduledLineID] ,s.[ScannedLineID] ,
    s.[Cell] ,s.[Pattern] ,s.[PtnDesc] ,s.[Group] ,s.[Size] ,s.[Artwork] ,s.[Qty] ,s.[SubprocessID] ,
    s.[PostSewingSubProcess] ,s.[NoBundleCardAfterSubprocess] ,s.[Location] ,s.[BundleCreateDate],
    s.[BuyerDeliveryDate],s.[SewingInline],s.[SubprocessQAInspectionDate],s.[InTime],s.[OutTime],s.[POSupplier] ,
    s.[AllocatedSubcon] ,s.AvgTime ,s.[TimeRange],s.[EstimatedCutDate],s.CuttingOutputDate,	s.Item 
	,s.PanelNo	,s.CutCellID     ,s.SpreadingNo     ,s.[LastSewDate]     ,s.[SewQty] 
from #tmp s
where not exists (
    select 1 from P_SubprocessWIP t 
    where t.Bundleno = s.Bundleno
    AND t.RFIDProcessLocationID = s.RFIDProcessLocationID 
    AND t.SP = s.SP 
    AND t.Pattern = s.Pattern
    AND t.SubprocessID = s.SubprocessID
)


if exists (select 1 from BITableInfo b where b.id = 'P_SubprocessWIP')
begin
	update b
		set b.TransferDate = getdate()
	from BITableInfo b
	where b.id = 'P_SubprocessWIP'
end
else 
begin
	insert into BITableInfo(Id, TransferDate)
	values('P_SubprocessWIP', getdate())
end
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

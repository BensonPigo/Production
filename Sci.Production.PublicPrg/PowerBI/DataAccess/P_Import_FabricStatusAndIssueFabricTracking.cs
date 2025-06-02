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
        public Base_ViewModel P_FabricStatusAndIssueFabricTracking(DateTime? sDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R04 biModel = new PPIC_R04();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd"));
            }

            try
            {
                PPIC_R04_ViewModel ppic_R04 = new PPIC_R04_ViewModel()
                {
                    LeadTime = 10800,
                    BIEditDate = ((DateTime)sDate).ToString("yyyy/MM/dd"),
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
                finalResult = this.UpdateBIData(detailTable, sDate.Value);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }
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
     t.[SewingCell] = s.[SewingCell]
    ,t.[LineID] = s.[SewingLineID]
    ,t.Department = s.Department
    ,t.[StyleID] = s.[StyleID]
    ,t.[FabricType] = s.[FabricType]
    ,t.[Color] = s.[ColorName]
    ,t.[ApvDate] = s.[ApvDate]
    ,t.[NoOfPcsRejected] = s.[RejectQty]
    ,t.[RequestQtyYrds] = s.[RequestQty]
    ,t.[IssueQtyYrds] = s.[IssueQty]
    ,t.[ReplacementFinishedDate] = s.[FinishedDate]
    ,t.[Type] = s.[Type]
    ,t.[Process] = s.[Process]
    ,t.[Description] = s.[Description]
    ,t.[OnTime] = s.[OnTime]
    ,t.[Remark] = s.[Remark]
    ,t.[DetailRemark] = s.[DetailRemark]
    ,t.[StyleName] = s.[StyleName]
    ,t.[MaterialType] = s.[MaterialType]
    ,t.[SewingQty] = s.[SewingQty]
from P_FabricStatus_And_IssueFabricTracking t 
inner join #tmp s on t.ReplacementID = s.ID 
AND t.SP = s.OrderID 
AND t.Seq = s.Seq 
AND t.RefNo = s.RefNo


insert into P_FabricStatus_And_IssueFabricTracking (
    [SewingCell]      ,[LineID]      ,[ReplacementID] , [Department]
    ,[StyleID]      ,[SP]     ,[Seq]      ,[FabricType]
    ,[Color]      ,[RefNo]      ,[ApvDate]
    ,[NoOfPcsRejected]      ,[RequestQtyYrds]      ,[IssueQtyYrds]
    ,[ReplacementFinishedDate]      ,[Type]
    ,[Process]      ,[Description]      ,[OnTime]      ,[Remark]
    ,[DetailRemark]
    ,[StyleName]
    ,[MaterialType]
    ,[SewingQty],[FactoryID]
)
select 	s.[SewingCell]      ,s.[SewingLineID]    ,s.[ID] ,s.[Department]
		  ,s.[StyleID]    ,s.[OrderID]     ,s.[Seq]      ,s.[FabricType]
		  ,s.[ColorName]      ,s.[RefNo]      ,s.[ApvDate]
		  ,s.[RejectQty]      ,s.[RequestQty]      ,s.[IssueQty]
		  ,s.[FinishedDate]      ,s.[Type]
		  ,s.[Process]      ,s.[Description]  ,s.[OnTime]      ,s.[Remark]
        ,s.[DetailRemark]
        ,s.[StyleName]
        ,s.[MaterialType]
        ,s.[SewingQty],[FactoryID]
from #tmp s
where not exists (
    select 1 from P_FabricStatus_And_IssueFabricTracking t 
    where t.ReplacementID = s.ID 
    AND t.SP = s.OrderID 
    AND t.Seq = s.Seq 
    AND t.RefNo = s.RefNo
    and t.[FactoryID] = s.[FactoryID]
)

delete t 
from dbo.P_FabricStatus_And_IssueFabricTracking t
where not exists (
    select 1 from #tmp s 
     where t.ReplacementID = s.ID 
    AND t.SP = s.OrderID 
    AND t.Seq = s.Seq 
    AND t.RefNo = s.RefNo
    AND t.[FactoryID] = s.[FactoryID]
)
and t.ReplacementFinishedDate >= @SDate

--移除不存在的Production.lack_detail的資料
Delete p
From P_FabricStatus_And_IssueFabricTracking p
Where not exists(
    select * from mainserver.production.dbo.lack_detail l
     where p.replacementID = l.id
     and (l.seq1+' '+l.Seq2) = p.seq
 )

--移除不存在的Production.lack的資料
Delete p
From P_FabricStatus_And_IssueFabricTracking p
Where 
 Not exists
 (
	 Select 1 from  mainserver.production.dbo.Lack l
	 where l.ID = p.replacementID
	 and l.OrderID = p.SP
 )

";
                sql += new Base().SqlBITableInfo("P_FabricStatus_And_IssueFabricTracking", true);
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

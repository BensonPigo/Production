using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_IssueFabricByCuttingTransactionList
    {
        /// <inheritdoc/>
        public Base_ViewModel P_IssueFabricByCuttingTransactionList(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Warehouse_R16 biModel = new Warehouse_R16();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Now.AddDays(-30);
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Now;
            }

            Warehouse_R16_ViewModel model = new Warehouse_R16_ViewModel()
            {
                IssueDateFrom = null,
                IssueDateTo = null,
                MDivisionID = string.Empty,
                FactoryID = string.Empty,
                CutplanIDFrom = string.Empty,
                CutplanIDTo = string.Empty,
                SPFrom = string.Empty,
                SPTo = string.Empty,
                EditDateFrom = item.SDate,
                EditDateTo = item.EDate,
            };

            try
            {
                Base_ViewModel resultReport = biModel.GetIssueFabricByCuttingTransactionList(model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTables = resultReport.Dt;

                // insert into PowerBI
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

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@EDate", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                string sql = new Base().SqlBITableHistory("P_IssueFabricByCuttingTransactionList", "P_IssueFabricByCuttingTransactionList_History", "#tmp", "((AddDate >= @SDate and AddDate <= @EDate) or (EditDate >= @SDate and EditDate <= @EDate))", needJoin: false) + Environment.NewLine;
                sql += @"	
-- 更新 P_IssueFabricByCuttingTransactionList
delete p
from P_IssueFabricByCuttingTransactionList p
where	((AddDate >= @SDate and AddDate <= @EDate) or
		(EditDate >= @SDate and EditDate <= @EDate)) and
		not exists(select 1 from #tmp t where p.Issue_DetailUkey = t.Issue_DetailUkey)

update p set p.MDivisionID						   = t.MDivisionID					
			,p.FactoryID						   = t.FactoryID					
			,p.CutplanID						   = t.CutplanID					
			,p.EstCutDate						   = t.EstCutDate					
			,p.IssueDate						   = t.IssueDate					
			,p.Line								   = t.Line							
			,p.CutCellID						   = t.CutCellID					
			,p.FabricComboAndCutNo				   = t.FabricComboAndCutNo			
			,p.IssueRemark						   = t.IssueRemark					
			,p.Refno							   = t.Refno						
			,p.ColorID							   = t.ColorID		
			,p.ColorName						   = t.ColorName		
			,p.Description						   = t.Description					
			,p.WeaveTypeID						   = t.WeaveTypeID					
			,p.RelaxTime						   = t.RelaxTime					
			,p.StockUnit						   = t.StockUnit					
			,p.IssueQty							   = t.IssueQty						
			,p.BulkLocation						   = t.BulkLocation					
			,p.IssueCreateName					   = t.IssueCreateName				
			,p.MINDReleaseName					   = t.MINDReleaseName				
			,p.IssueStartTime					   = t.IssueStartTime				
			,p.MINDReleaseDate					   = t.MINDReleaseDate				
			,p.PickingCompletion				   = t.PickingCompletion			
			,p.NeedUnroll						   = t.NeedUnroll					
			,p.UnrollScanName					   = t.UnrollScanName				
			,p.UnrollStartTime					   = t.UnrollStartTime				
			,p.UnrollEndTime					   = t.UnrollEndTime				
			,p.RelaxationStartTime				   = t.RelaxationStartTime			
			,p.RelaxationEndTime				   = t.RelaxationEndTime			
			,p.UnrollActualQty					   = t.UnrollActualQty				
			,p.UnrollRemark						   = t.UnrollRemark					
			,p.UnrollingRelaxationCompletion	   = t.UnrollingRelaxationCompletion
			,p.DispatchScanName					   = t.DispatchScanName				
			,p.DispatchScanTime					   = t.DispatchScanTime				
			,p.RegisterTime						   = t.RegisterTime					
			,p.DispatchTime						   = t.DispatchTime					
			,p.FactoryReceivedName				   = t.FactoryReceivedName			
			,p.FactoryReceivedTime				   = t.FactoryReceivedTime			
			,p.AddDate							   = t.AddDate						
			,p.EditDate							   = t.EditDate	
			,p.IssueID							   = t.IssueID	
			,p.Seq								   = t.Seq	
			,p.OrderID							   = t.OrderID	
			,p.Roll							       = t.Roll	
			,p.Dyelot							   = t.Dyelot	
			,p.StockType						   = t.StockType	
	 	    ,p.Style                               = t.Style
			,p.[BIFactoryID]					   = @BIFactoryID
			,p.[BIInsertDate]					   = GETDATE()
			,p.[BIStatus]				           = 'New'	
			,p.[RequestCons]					   = isnull(t.[Request Cons], 0)
			,p.[UnrollMachine]					   = isnull(t.[UnrollMachine], '')
			,p.[Rack]							   = isnull(t.[Rack] ,'')
			,p.[FabricRegisterBy]				   = isnull(t.[DispatchScanName], '')
			,p.[DispatchReason]					   = isnull(t.[DispatchReason], '')
			,p.[DispatchRemark]					   = isnull(t.[DispatchRemark], '')
from P_IssueFabricByCuttingTransactionList p
inner join #tmp t on p.Issue_DetailUkey = t.Issue_DetailUkey

insert into P_IssueFabricByCuttingTransactionList(
		IssueID
		,MDivisionID
		,FactoryID
		,CutplanID
		,EstCutDate
		,IssueDate
		,Line
		,CutCellID
		,FabricComboAndCutNo
		,IssueRemark
		,OrderID
		,Style
		,Seq
		,Refno
		,ColorID
		,ColorName
		,Description
		,WeaveTypeID
		,RelaxTime
		,Roll
		,Dyelot
		,StockType
		,StockUnit
		,IssueQty
		,BulkLocation
		,IssueCreateName
		,MINDReleaseName
		,IssueStartTime
		,MINDReleaseDate
		,PickingCompletion
		,NeedUnroll
		,UnrollScanName
		,UnrollStartTime
		,UnrollEndTime
		,RelaxationStartTime
		,RelaxationEndTime
		,UnrollActualQty
		,UnrollRemark
		,UnrollingRelaxationCompletion
		,DispatchScanName
		,DispatchScanTime
		,RegisterTime
		,DispatchTime
		,FactoryReceivedName
		,FactoryReceivedTime
		,AddDate
		,EditDate
		,Issue_DetailUkey
		,[BIFactoryID] 
		,[BIInsertDate]
		,[BIStatus]
		,[RequestCons]
		,[UnrollMachine]
		,[Rack]
		,[FabricRegisterBy]
		,[DispatchReason]
		,[DispatchRemark]
)
select	 t.IssueID
		,t.MDivisionID
		,t.FactoryID
		,t.CutplanID
		,t.EstCutDate
		,t.IssueDate
		,t.Line
		,t.CutCellID
		,t.FabricComboAndCutNo
		,t.IssueRemark
		,t.OrderID
		,t.Style
		,t.Seq
		,t.Refno
		,t.ColorID
		,t.ColorName
		,t.Description
		,t.WeaveTypeID
		,t.RelaxTime
		,t.Roll
		,t.Dyelot
		,t.StockType
		,t.StockUnit
		,t.IssueQty
		,t.BulkLocation
		,t.IssueCreateName
		,t.MINDReleaseName
		,t.IssueStartTime
		,t.MINDReleaseDate
		,t.PickingCompletion
		,t.NeedUnroll
		,t.UnrollScanName
		,t.UnrollStartTime
		,t.UnrollEndTime
		,t.RelaxationStartTime
		,t.RelaxationEndTime
		,t.UnrollActualQty
		,t.UnrollRemark
		,t.UnrollingRelaxationCompletion
		,t.DispatchScanName
		,t.DispatchScanTime
		,t.RegisterTime
		,t.DispatchTime
		,t.FactoryReceivedName
		,t.FactoryReceivedTime
		,t.AddDate
		,t.EditDate
		,t.Issue_DetailUkey
		,@BIFactoryID
		,GETDATE()
		,'New'
		,isnull(t.[Request Cons], 0)
		,isnull(t.UnrollMachine, '')
		,isnull(t.Rack, '')
		,isnull(t.DispatchScanName, '')
		,isnull(t.DispatchReason, '')
		,isnull(t.DispatchRemark, '')
from #tmp t
where not exists(	select 1 
					from P_IssueFabricByCuttingTransactionList p
					where	p.Issue_DetailUkey = t.Issue_DetailUkey)

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

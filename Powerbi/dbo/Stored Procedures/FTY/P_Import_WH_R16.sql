
Create PROCEDURE [dbo].[P_Import_WH_R16]
@StartDate date,
@EndDate date
AS
BEGIN

Declare @IssueDateFromString varchar(8) = Format(@StartDate, 'yyyyMMdd')
Declare @IssueDateToString varchar(8) = Format(@EndDate, 'yyyyMMdd')

select * into #tmpIssueFabricByCuttingTransactionList
from P_IssueFabricByCuttingTransactionList
where 1 = 0

DECLARE @DynamicSQL NVARCHAR(MAX) = '
insert into #tmpIssueFabricByCuttingTransactionList(
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
,Seq
,Refno
,ColorID
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
,Style
,ColorName
)
select IssueID
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
,Seq
,Refno
,ColorID
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
,Style
,ColorName
from OPENQUERY([MainServer], '' SET NOCOUNT ON; exec Production.dbo.Warehouse_Report_R16 @EditDateFrom = '''''+ @IssueDateFromString +''''', @EditDateTo = '''''+ @IssueDateToString +''''''')

'

EXEC sp_executesql @DynamicSQL

-- 更新 P_IssueFabricByCuttingTransactionList
delete p
from P_IssueFabricByCuttingTransactionList p
where	((AddDate >= @StartDate and AddDate <= @EndDate) or
		(EditDate >= @StartDate and EditDate <= @EndDate)) and
		not exists(select 1 from #tmpIssueFabricByCuttingTransactionList t 
												where	p.Issue_DetailUkey = t.Issue_DetailUkey)

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
			,p.Style							   = t.Style
			,p.ColorName						   = t.ColorName
from P_IssueFabricByCuttingTransactionList p
inner join #tmpIssueFabricByCuttingTransactionList t on p.Issue_DetailUkey = t.Issue_DetailUkey

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
		,Seq
		,Refno
		,ColorID
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
		,Style
		,ColorName
		,Issue_DetailUkey)
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
		,t.Seq
		,t.Refno
		,t.ColorID
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
		,Style
		,ColorName
		,t.Issue_DetailUkey
from #tmpIssueFabricByCuttingTransactionList t
where not exists(	select 1 
					from P_IssueFabricByCuttingTransactionList p
					where	p.Issue_DetailUkey = t.Issue_DetailUkey)

if exists(select 1 from BITableInfo where Id = 'P_IssueFabricByCuttingTransactionList')
begin
	update BITableInfo set TransferDate = getdate()
	where Id = 'P_IssueFabricByCuttingTransactionList'
end
else
begin
	insert into BITableInfo(Id, TransferDate, IS_Trans) values('P_IssueFabricByCuttingTransactionList', GETDATE(), 0)
end

end
GO


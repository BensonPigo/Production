CREATE PROCEDURE [dbo].[P_Import_QA_R08_Detail]
	@StartDate date = null,
	@EndDate date = null
AS
begin
Declare @EditDateFrom varchar(8) = Format(@StartDate, 'yyyyMMdd')
Declare @EditDateTo varchar(8) = Format(@EndDate, 'yyyyMMdd')

select * into #tmpP_FabricInspDailyReport_Detail
from P_FabricInspDailyReport_Detail
where 1 = 0

DECLARE @DynamicSQL NVARCHAR(MAX) = '
insert into #tmpP_FabricInspDailyReport_Detail(
InspDate
,Inspector
,InspectorName
,BrandID
,Factory
,StyleID
,POID
,SEQ
,StockType
,Wkno
,SuppID
,SuppName
,ATA
,Roll
,Dyelot
,RefNo
,Color
,ArrivedYDS
,ActualYDS
,LthOfDiff
,TransactionID
,QCIssueQty
,QCIssueTransactionID
,CutWidth
,ActualWidth
,Speed
,TotalDefectPoints
,Grade
,ActInspTimeStart
,CalculatedInspTimeStartFirstTime
,ActInspTimeFinish
,InspTimeFinishFirstTime
,QCMachineStopTime
,QCMachineRunTime
,Remark
,MCHandle
,WeaveType
,ReceivingID
,AddDate
,EditDate
)
select 
 InspDate
,Inspector
,InspectorName
,BrandID
,Factory
,StyleID
,POID
,SEQ
,StockType
,Wkno
,SuppID
,SuppName
,ATA
,Roll
,Dyelot
,RefNo
,Color
,ArrivedYDS
,ActualYDS
,LthOfDiff
,TransactionID
,QCIssueQty
,QCIssueTransactionID
,CutWidth
,ActualWidth
,Speed
,TotalDefectPoints
,Grade
,ActInspTimeStart
,CalculatedInspTimeStartFirstTime
,ActInspTimeFinish
,InspTimeFinishFirstTime
,QCMachineStopTime
,QCMachineRunTime
,Remark
,MCHandle
,WeaveType
,ReceivingID
,AddDate
,EditDate
from OPENQUERY([MainServer], '' SET NOCOUNT ON; select * from Production.dbo.GetQA_R08_Detail(null,null,'''''''','''''''','''''''','''''''','''''''','''''''','''''+ @EditDateFrom +''''','''''+ @EditDateTo +''''','''''''','''''''','''''''') '')
where	ATA is not null and
		InspDate is not null
'

EXEC sp_executesql @DynamicSQL

-- 更新 P_IssueFabricByCuttingTransactionList
delete p
from P_FabricInspDailyReport_Detail p
where	((p.AddDate >= @StartDate and p.AddDate <= @EndDate) or (p.EditDate >= @StartDate and p.EditDate <= @EndDate)) and
		not exists(select 1 from #tmpP_FabricInspDailyReport_Detail t 
							where	p.InspDate = t.InspDate and
									p.Inspector = t.Inspector and
									p.POID = t.POID and
									p.SEQ = t.SEQ and
									p.ATA = t.ATA and
									p.Roll = t.Roll and
									p.Dyelot = t.Dyelot)

update p set p.InspectorName						= t.InspectorName					
			,p.BrandID								= t.BrandID							
			,p.Factory								= t.Factory							
			,p.StyleID								= t.StyleID							
			,p.StockType							= t.StockType						
			,p.Wkno									= t.Wkno								
			,p.SuppID								= t.SuppID							
			,p.SuppName								= t.SuppName							
			,p.RefNo								= t.RefNo							
			,p.Color								= t.Color							
			,p.ArrivedYDS							= t.ArrivedYDS						
			,p.ActualYDS							= t.ActualYDS						
			,p.LthOfDiff							= t.LthOfDiff						
			,p.TransactionID						= t.TransactionID					
			,p.QCIssueQty							= t.QCIssueQty						
			,p.QCIssueTransactionID					= t.QCIssueTransactionID				
			,p.CutWidth								= t.CutWidth							
			,p.ActualWidth							= t.ActualWidth						
			,p.Speed								= t.Speed							
			,p.TotalDefectPoints					= t.TotalDefectPoints				
			,p.Grade								= t.Grade							
			,p.ActInspTimeStart						= t.ActInspTimeStart					
			,p.CalculatedInspTimeStartFirstTime		= t.CalculatedInspTimeStartFirstTime	
			,p.ActInspTimeFinish					= t.ActInspTimeFinish				
			,p.InspTimeFinishFirstTime				= t.InspTimeFinishFirstTime			
			,p.QCMachineStopTime					= t.QCMachineStopTime				
			,p.QCMachineRunTime						= t.QCMachineRunTime					
			,p.Remark								= t.Remark							
			,p.MCHandle								= t.MCHandle							
			,p.WeaveType							= t.WeaveType	
			,p.ReceivingID							= t.ReceivingID
			,p.AddDate								= t.AddDate							
			,p.EditDate								= t.EditDate							
from P_FabricInspDailyReport_Detail p
inner join #tmpP_FabricInspDailyReport_Detail t on	p.InspDate = t.InspDate and
												p.Inspector = t.Inspector and
												p.POID = t.POID and
												p.SEQ = t.SEQ and
												p.ATA = t.ATA and
												p.Roll = t.Roll and
												p.Dyelot = t.Dyelot

insert into P_FabricInspDailyReport_Detail(
		InspDate
		,Inspector
		,InspectorName
		,BrandID
		,Factory
		,StyleID
		,POID
		,SEQ
		,StockType
		,Wkno
		,SuppID
		,SuppName
		,ATA
		,Roll
		,Dyelot
		,RefNo
		,Color
		,ArrivedYDS
		,ActualYDS
		,LthOfDiff
		,TransactionID
		,QCIssueQty
		,QCIssueTransactionID
		,CutWidth
		,ActualWidth
		,Speed
		,TotalDefectPoints
		,Grade
		,ActInspTimeStart
		,CalculatedInspTimeStartFirstTime
		,ActInspTimeFinish
		,InspTimeFinishFirstTime
		,QCMachineStopTime
		,QCMachineRunTime
		,Remark
		,MCHandle
		,WeaveType
		,ReceivingID
		,AddDate
		,EditDate)
select	 t.InspDate
		,t.Inspector
		,t.InspectorName
		,t.BrandID
		,t.Factory
		,t.StyleID
		,t.POID
		,t.SEQ
		,t.StockType
		,t.Wkno
		,t.SuppID
		,t.SuppName
		,t.ATA
		,t.Roll
		,t.Dyelot
		,t.RefNo
		,t.Color
		,t.ArrivedYDS
		,t.ActualYDS
		,t.LthOfDiff
		,t.TransactionID
		,t.QCIssueQty
		,t.QCIssueTransactionID
		,t.CutWidth
		,t.ActualWidth
		,t.Speed
		,t.TotalDefectPoints
		,t.Grade
		,t.ActInspTimeStart
		,t.CalculatedInspTimeStartFirstTime
		,t.ActInspTimeFinish
		,t.InspTimeFinishFirstTime
		,t.QCMachineStopTime
		,t.QCMachineRunTime
		,t.Remark
		,t.MCHandle
		,t.WeaveType
		,t.ReceivingID
		,t.AddDate
		,t.EditDate
from #tmpP_FabricInspDailyReport_Detail t
where not exists(	select 1 
					from P_FabricInspDailyReport_Detail p
					where	p.InspDate = t.InspDate and
							p.Inspector = t.Inspector and
							p.POID = t.POID and
							p.SEQ = t.SEQ and
							p.ATA = t.ATA and
							p.Roll = t.Roll and
							p.Dyelot = t.Dyelot)

if exists(select 1 from BITableInfo where Id = 'P_FabricInspDailyReport_Detail')
begin
	update BITableInfo set TransferDate = getdate()
	where Id = 'P_FabricInspDailyReport_Detail'
end
else
begin
	insert into BITableInfo(Id, TransferDate, IS_Trans) values('P_FabricInspDailyReport_Detail', GETDATE(), 0)
end

end
go




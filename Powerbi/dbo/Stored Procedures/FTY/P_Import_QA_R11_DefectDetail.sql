Create PROCEDURE [dbo].[P_Import_QA_R11_DefectDetail]
	@StartDate date = null,
	@EndDate date = null
As
Begin
	Set NoCount On;
		
Declare @ExecSQL1 NVarChar(MAX);

Declare @EditDateFrom varchar(12) = Format(@StartDate, 'yyyyMMdd')
Declare @EditDateTo varchar(12) = Format(@EndDate, 'yyyyMMdd')

select *
into #tmpP_FabricInspReport_ReceivingTransferIn
from P_FabricInspReport_ReceivingTransferIn
where 1=0


set @ExecSQL1 = N'
insert into #tmpP_FabricInspReport_ReceivingTransferIn(
	[POID]
    ,[SEQ]
    ,[Wkno]
    ,[ReceivingID]
    ,[StyleID]
    ,[BrandID]
    ,[Supplier]
    ,[Refno]
    ,[Color]
    ,[ArriveWHDate]
    ,[ArriveQty]
    ,[WeaveTypeID]
    ,[Dyelot]
    ,[CutWidth]
    ,[Weight]
    ,[Composition]
    ,[Desc]
    ,[FabricConstructionID]
    ,[Roll]
    ,[InspDate]
    ,[Result]
    ,[Grade]
    ,[DefectCode]
    ,[DefectType]
    ,[DefectDesc]
    ,[Points]
    ,[DefectRate]
    ,[Inspector]
    ,[AddDate]
	,[EditDate]
)
select 
	[POID]
    ,[SEQ]
    ,[Wkno]
    ,[ReceivingID]
    ,[StyleID]
    ,[BrandID]
    ,[Supplier]
    ,[Refno]
    ,[Color]
    ,[ArriveWHDate]
    ,[ArriveQty]
    ,[WeaveTypeID]
    ,[Dyelot]
    ,[CutWidth]
    ,[Weight]
    ,[Composition]
    ,[Desc]
    ,[FabricConstructionID]
    ,[Roll]
    ,[InspDate]
    ,[Result]
    ,[Grade]
    ,DEFECTRECORD
    ,[DefectType]
    ,[DefectDesc]
    ,[Points]
    ,[DefectRate]
    ,[Inspector]
    ,[AddDate]
	,[EditDate]
from OPENQUERY([MainServer], '' SET NOCOUNT ON; select * from Production.dbo.GetQA_R11_ReceivingTransferIn_Detail('''''+ @EditDateFrom +''''','''''+ @EditDateTo +''''',null,null) '')
'

EXEC sp_executesql @ExecSQL1


Merge P_FabricInspReport_ReceivingTransferIn t
	using( select * from  #tmpP_FabricInspReport_ReceivingTransferIn) as s
	on t.POID = s.POID
	and t.SEQ = s.SEQ
	and t.ReceivingID = s.ReceivingID
	and t.Dyelot = s.Dyelot
	and t.Roll = s.Roll
	and t.DefectCode = s.DefectCode
	WHEN MATCHED then UPDATE SET	   
       t.[Wkno] = s.[Wkno]
      ,t.[StyleID] = s.[StyleID]
      ,t.[BrandID] = s.[BrandID]
      ,t.[Supplier] = s.[Supplier]
      ,t.[Refno] = s.[Refno]
      ,t.[Color] = s.[Color]
      ,t.[ArriveWHDate] = s.[ArriveWHDate]
      ,t.[ArriveQty] = s.[ArriveQty]
      ,t.[WeaveTypeID] = s.[WeaveTypeID]
      ,t.[CutWidth] = s.[CutWidth]
      ,t.[Weight] = s.[Weight]
      ,t.[Composition] = s.[Composition]
      ,t.[Desc] = s.[Desc]
      ,t.[FabricConstructionID] = s.[FabricConstructionID]
      ,t.[InspDate] = s.[InspDate]
      ,t.[Result] = s.[Result]
      ,t.[Grade] = s.[Grade]
	  ,t.[DefectCode] = s.[DefectCode]
      ,t.[DefectType] = s.[DefectType]
      ,t.[DefectDesc] = s.[DefectDesc]
      ,t.[Points] = s.[Points]
      ,t.[DefectRate] = s.[DefectRate]
      ,t.[Inspector] = s.[Inspector]
       ,t.[EditDate] = s.[EditDate]
	WHEN NOT MATCHED BY TARGET THEN
insert (
	 [POID],[SEQ],[Wkno],[ReceivingID],[StyleID],[BrandID],[Supplier],[Refno],[Color],[ArriveWHDate],[ArriveQty],[WeaveTypeID]
	 ,[Dyelot],[CutWidth],[Weight],[Composition],[Desc],[FabricConstructionID],[Roll],[InspDate],[Result],[Grade]
      ,[DefectCode],[DefectType],[DefectDesc],[Points],[DefectRate],[Inspector],[AddDate],[EditDate]
)
VALUES (
       s.[POID],s.[SEQ],s.[Wkno],s.[ReceivingID],s.[StyleID],s.[BrandID],s.[Supplier],s.[Refno],s.[Color],s.[ArriveWHDate],s.[ArriveQty]
      ,s.[WeaveTypeID],s.[Dyelot],s.[CutWidth],s.[Weight],s.[Composition],s.[Desc],s.[FabricConstructionID],s.[Roll],s.[InspDate]
      ,s.[Result],s.[Grade],s.[DefectCode],s.[DefectType],s.[DefectDesc],s.[Points],s.[DefectRate],s.[Inspector]
      ,s.[AddDate],s.[EditDate]
)
when not matched by source and t.[ReceivingID] in (select ReceivingID from  #tmpP_FabricInspReport_ReceivingTransferIn)
	then delete;


	if exists (select 1 from BITableInfo b where b.id = 'P_FabricInspReport_ReceivingTransferIn')
	begin
		update b
			set b.TransferDate = getdate()
		from BITableInfo b
		where b.id = 'P_FabricInspReport_ReceivingTransferIn'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_FabricInspReport_ReceivingTransferIn', getdate())
	end
End


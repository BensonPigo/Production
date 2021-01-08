-- =============================================
-- Create date: 2020/11/09
-- Description:	Quality Report
-- =============================================
CREATE PROCEDURE [dbo].[P_ImportQAInspection]
	@InspectionDate Date,
	@LinkServerName varchar(50)
AS
BEGIN
	
declare @sDate varchar(20) = cast(@InspectionDate as varchar) -- 2020/01/01

declare @SqlCmd_Combin nvarchar(max) =''
declare @SqlCmd1 nvarchar(max) ='';
declare @SqlCmd2 nvarchar(max) ='';
declare @SqlCmd3 nvarchar(max) ='';
declare @SqlCmd4 nvarchar(max) ='';

declare @SqlFinal1 nvarchar(max) = ''
declare @SqlFinal2 nvarchar(max) = ''
declare @SqlFinal3 nvarchar(max) = ''
declare @SqlFinal  nvarchar(max) = ''

set @SqlCmd1 = '
-- MES/Endline/R08
select [FirstInspectionDate] = cast(Ins.AddDate as date)
	,[Factory] = ins.FactoryID
	,[Brand] = ord.BrandID
	,[Style] = ord.styleid
	,[PO#] = ord.custpono
	,[SP#] = ins.OrderId
	,ins.Article
	,ins.Size
	,[Destination] = Cou.Alias
	,ord.CdCodeID
	,cdc.ProductionFamilyID
	,ins.Team
	,ins.AddName
	,ins.Shift
	,ins.Line
	,s.SewingCell
	,[PassQty] = SUM(IIF(ins.Status = (''Pass'') , 1 ,0))
	,[RejectAndFixedQty] = SUM(IIF(ins.Status in (''Reject'',''Fixed'',''Dispose'') , 1 ,0))
	,[TtlQty] = SUM(IIF(ins.Status = (''Pass'') , 1 ,0)) + SUM(IIF(ins.Status in (''Reject'',''Fixed'',''Dispose'') , 1 ,0))
into #tmp_summy_first
from ['+@LinkServerName+'].ManufacturingExecution.dbo.Inspection ins WITH(NOLOCK) 
inner join ['+@LinkServerName+'].Production.dbo.Orders ord WITH(NOLOCK) on ins.OrderId=ord.id
inner join ['+@LinkServerName+'].Production.dbo.Factory fac WITH(NOLOCK) on ins.FactoryID=fac.ID
left  join ['+@LinkServerName+'].Production.dbo.SewingLine s WITH(NOLOCK) on s.FactoryID = ins.FactoryID and s.ID = ins.Line
left  join ['+@LinkServerName+'].Production.dbo.Country Cou WITH(NOLOCK) on ord.Dest = Cou.ID
left  join ['+@LinkServerName+'].Production.dbo.CDCode cdc WITH(NOLOCK) on ord.CdCodeID = cdc.ID
where ins.Adddate >= '''+@sDate+'''
group by cast(Ins.AddDate as date), ins.FactoryID, ord.BrandID, ord.styleid, ord.custpono, 
ins.OrderId, ins.Article, ins.Size, Cou.Alias, ord.CdCodeID, cdc.ProductionFamilyID,
ins.Team, ins.AddName, ins.Shift, ins.Line, s.SewingCell

select t.FirstInspectionDate
	, t.Factory
	, t.Brand
	, t.Style
	, t.[PO#]
	, t.[SP#]
	, t.Article
	, t.Size
	, t.[Destination]
	, t.CdCodeID
	, t.ProductionFamilyID
	, t.Team
	, t.AddName
	, t.Shift
	, t.Line
	, t.SewingCell	
	, t.TtlQty
	, t.RejectAndFixedQty
	,[EndlineWFT] = iif(t.TtlQty = 0, 0, ROUND( (t.RejectAndFixedQty *1.0) / (t.TtlQty *1.0) *100,3))
	,[Endline RFT(%)] = iif(t.TtlQty = 0, 0, ROUND( (t.PassQty *1.0) / (t.TtlQty *1.0) *100,3)) 
into #Final_DQSDefect_Summary
from #tmp_summy_first t
'

set @SqlCmd2 = '

select fac.Zone
    , [Brand] = ord.BrandID
	, [Buyer Delivery Date] = ord.BuyerDelivery
	, ins.Line
	, [Factory] = ins.FactoryID
	, ins.Team 
	, ins.Shift
 	, [PO#] = ord.custpono  
	, [Style] = ord.styleid
	, [SP#] = ins.OrderId
	, ins.Article
	, ins.Status
	, [FixType] = iif(ins.FixType=''Repl.'',''Replacement'',ins.FixType)
	, [FirstInspectionDate] = cast(Ins.AddDate as date)
	, [FirstInspectedTime] = format(ins.AddDate,''HH:mm:ss'')
	, [Inspected QC] = Inspection_QC.Name
	, [Fixed Time] = iif(ins.Status<>''Fixed'','''', format(ins.EditDate,''yyyy/MM/dd HH:mm'') )
	, [Fixed QC] = iif(ins.Status<>''Fixed'','''',Inspection_fixQC.Name)
	, [ProductType] =  case when ins.Location = ''T'' then ''TOP''
							when ins.Location = ''B'' then ''BOTTOM''
							when ins.Location = ''I'' then ''INNER''
							when ins.Location = ''O'' then ''OUTER''
							else ''''
					   end
	, ins.Size
	, [DefectTypeDescritpion] = gdt.Description
	, [DefectCodeDescritpion] = gdc.Description
	, [Area] = ind.AreaCode
	, [ReworkCardNo] = ins.ReworkCardNo
	, [DefectTypeID] = ind.GarmentDefectTypeID
	, [DefectCodeID] = ind.GarmentDefectCodeID
into #Final_DQSDefect_Detail
from ['+@LinkServerName+'].ManufacturingExecution.dbo.Inspection ins WITH(NOLOCK)
inner join ['+@LinkServerName+'].Production.dbo.orders ord WITH(NOLOCK) on ins.OrderId=ord.id
inner join ['+@LinkServerName+'].Production.dbo.Factory fac WITH(NOLOCK) on ins.FactoryID=fac.ID
left  join ['+@LinkServerName+'].ManufacturingExecution.dbo.Inspection_Detail ind WITH(NOLOCK) on ins.id=ind.ID	
left  join ['+@LinkServerName+'].Production.dbo.GarmentDefectCode gdc WITH(NOLOCK) on ind.GarmentDefectTypeID=gdc.GarmentDefectTypeID and ind.GarmentDefectCodeID=gdc.ID
left  join ['+@LinkServerName+'].Production.dbo.GarmentDefectType gdt WITH(NOLOCK) on gdc.GarmentDefectTypeID=gdt.ID
outer apply(select name from ['+@LinkServerName+'].ManufacturingExecution.dbo.pass1 p WITH(NOLOCK) where p.id= ins.AddName) Inspection_QC
outer apply(select name from ['+@LinkServerName+'].ManufacturingExecution.dbo.pass1 p WITH(NOLOCK) where p.id= ins.EditName) Inspection_fixQC
where ins.Adddate >= '''+@sDate+'''
and ins.Status <> ''Pass''
Order by Zone,[Brand],[Factory],Line,Team,[SP#],Article,[ProductType],Size,[DefectTypeID],[DefectCodeID]

drop table #tmp_summy_first


-- PMS/QA/R21
select DISTINCT
[Action]= b.Action
,[Area]= b.CFAAreaID +'' - ''+ar.Description
,a.cDate
,c.BrandID
, c.BuyerDelivery 
,[Cfa] = isnull((select CONCAT(a.CFA, '':'', Name) from [PMS\pmsdb\SNP].Production.dbo.Pass1  WITH (NOLOCK) where ID = a.CFA),'''')
,[Defect Description]= gd.Description
,a.DefectQty
, [Destination]=ct.Alias
,c.FactoryID
,[GarmentOutput]= round(a.GarmentOutput/100,2)	
,[Stage]= 
		case a.Stage when ''I'' then ''Comments/Roving''
			when ''C'' then ''Change Over''
			when ''P'' then ''Stagger''
			when ''R'' then ''Re-Stagger''
			when ''F'' then ''Final''
			when ''B'' then ''Buyer''
else '''' end
,a.SewingLineID
,[No. Of Defect]=b.Qty
,c.Qty
,c.CustPONo
,[Remark]= b.Remark
,[Result]= case a.result 
			when ''P'' then ''Pass''
			when ''F'' then ''Fail''
else '''' end 
,a.InspectQty		
,[shift]= case a.shift 
			when ''D'' then ''DAY''
			when ''N'' then ''NIGHT''
			when ''O'' then ''SUBCON OUT''
			when ''I'' then ''SUBCON IN''
else '''' end
,a.OrderID
,[SQR]= iif(a.InspectQty=0,0,round(a.DefectQty/a.InspectQty,3)) 	
,c.StyleID
,a.Team
,[VAS/SHAS]= iif(c.VasShas=0,'''',''v'') 
into #Final_P_CFAInline_Detail
from ['+@LinkServerName+'].Production.dbo.Cfa a WITH (NOLOCK) 
inner join ['+@LinkServerName+'].Production.dbo.Cfa_Detail b WITH (NOLOCK) on b.id = a.ID 
inner join ['+@LinkServerName+'].Production.dbo.orders c WITH (NOLOCK) on c.id = a.OrderID
inner JOIN ['+@LinkServerName+'].Production.dbo.Country ct WITH (NOLOCK) ON ct.ID=c.Dest
outer apply(select Description from ['+@LinkServerName+'].Production.dbo.GarmentDefectCode a WITH(NOLOCK) where a.id=b.GarmentDefectCodeID) as gd
outer apply(select description from ['+@LinkServerName+'].Production.dbo.cfaarea a WITH(NOLOCK) where a.id=b.CFAAreaID) as ar
where a.Status = ''Confirmed'' and a.cDate >= '''+@sDate+'''
'
set @SqlCmd3 = '
--PMS/QA/R32

SELECT c.[ID]
      ,c.[AuditDate]
      ,c.[FactoryID]
      ,c.[MDivisionid]
      ,c.[SewingLineID]
      ,c.[Team]
      ,c.[Shift]
      ,c.[Stage]
      ,c.[Carton]
      ,c.[InspectQty]
      ,c.[DefectQty]
      ,c.[ClogReceivedPercentage]
      ,c.[Result]
      ,c.[CFA]
      ,c.[Status]
      ,c.[Remark]
      ,c.[AddName]
      ,c.[AddDate]
      ,c.[EditName]
      ,c.[EditDate]
      ,c.[IsCombinePO]
,co.OrderID,co.SEQ
INTO #MainData1
From ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord c WITH(NOLOCK)
INNER JOIN ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord_OrderSEQ co WITH(NOLOCK) ON c.ID = co.ID
INNER JOIN ['+@LinkServerName+'].Production.dbo.Orders O WITH(NOLOCK) ON o.ID = co.OrderID
WHERE 1=1
AND c.AuditDate >= '''+@sDate+'''


SELECT c.[ID]
      ,c.[AuditDate]
      ,c.[FactoryID]
      ,c.[MDivisionid]
      ,c.[SewingLineID]
      ,c.[Team]
      ,c.[Shift]
      ,c.[Stage]
      ,c.[Carton]
      ,c.[InspectQty]
      ,c.[DefectQty]
      ,c.[ClogReceivedPercentage]
      ,c.[Result]
      ,c.[CFA]
      ,c.[Status]
      ,c.[Remark]
      ,c.[AddName]
      ,c.[AddDate]
      ,c.[EditName]
      ,c.[EditDate]
      ,c.[IsCombinePO]
,co.OrderID,co.SEQ
INTO #MainData
From ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord  c
INNER JOIN ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
WHERE c.ID IN (SELECT ID FROM #MainData1)

SELECT 
	 c.AuditDate
	,BuyerDelivery = (SELECT BuyerDelivery FROM ['+@LinkServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,c.OrderID
	,CustPoNo = (SELECT CustPoNo FROM ['+@LinkServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,StyleID = (SELECT StyleID FROM ['+@LinkServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,BrandID = (SELECT BrandID FROM ['+@LinkServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,Dest = (SELECT Dest FROM ['+@LinkServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,Seq = (SELECT Seq FROM ['+@LinkServerName+'].Production.dbo.Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
	,c.SewingLineID
	,[VasShas]= (SELECT IIF(VasShas=1,''Y'',''N'')  FROM ['+@LinkServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,c.ClogReceivedPercentage
	,c.MDivisionid
	,c.FactoryID
	,c.Shift
	,c.Team
	,Qty = (SELECT Qty FROM ['+@LinkServerName+'].Production.dbo.Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
	,c.Status
	,c.Carton
	,[CfA] = isnull((select CONCAT(c.CFA, '':'', Name) from [PMS\pmsdb\SNP].Production.dbo.Pass1  WITH (NOLOCK) where ID = c.CFA),'''')
	,c.stage
	,c.Result
	,c.InspectQty
	,c.DefectQty
	,[SQR] = IIF( c.InspectQty = 0,0 , (c.DefectQty * 1.0 / c.InspectQty) * 100)
	,[DefectDescription] = g.Description
	,[AreaCodeDesc] = cd.CFAAreaID + '' - '' + CfaArea.Description
	,[NoOfDefect] = cd.Qty
	,cd.Remark
	,c.ID
	,c.IsCombinePO
	,[InsCtn]=IIF(c.stage = ''Final'' OR c.Stage =''3rd party'',
	( 
		SELECT [Val]= COUNT(DISTINCT cr.ID) + 1
		FROM ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord cr
		INNER JOIN ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord_OrderSEQ crd ON cr.ID = crd.ID
		WHERE crd.OrderID=c.OrderID AND crd.SEQ=c.SEQ
	    AND cr.Status = ''Confirmed''
	    AND cr.Stage=c.Stage
	    AND cr.AuditDate <= c.AuditDate
	    AND cr.ID  != c.ID
	)
	,NULL)
	,[Action]= cd.Action
	,[CFAInspectionRecord_Detail_Key]= concat(c.ID,iif(isnull(cd.GarmentDefectCodeID, '''') = '''', concat(row_Number()over(order by c.ID),''''), cd.GarmentDefectCodeID))
INTO #tmp
FROm #MainData  c
LEFT JOIN ['+@LinkServerName+'].Production.dbo.CFAInspectionRecord_Detail cd ON c.ID = cd.ID
LEFT JOIN ['+@LinkServerName+'].Production.dbo.GarmentDefectCode g ON g.ID = cd.GarmentDefectCodeID
LEFT JOIN ['+@LinkServerName+'].Production.dbo.CfaArea ON CfaArea.ID = cd.CFAAreaID


SELECT pd.*
INTO #PackingList_Detail
FROM ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd
INNER JOIN #tmp t ON pd.OrderID = t.OrderID ANd pd.OrderShipmodeSeq = t.SEQ AND t.ID = pd.StaggeredCFAInspectionRecordID


SELECT DISTINCT pd.*
INTO #PackingList_Detail2
FROM ['+@LinkServerName+'].Production.dbo.PackingList_Detail pd 
INNER JOIN #tmp t ON pd.OrderID = t.OrderID ANd pd.OrderShipmodeSeq = t.SEQ 
'

set @SqlCmd4 = '

SELECT  
Action
,AreaCodeDesc
,AuditDate
,BrandID
,BuyerDelivery
,CFA
,ClogReceivedPercentage
,DefectDescription
,DefectQty
,Dest
,FactoryID
,Carton
,[Inspected Ctn] = InspectedCtn.Val
,[Inspected PoQty]=InspectedPoQty.Val
,Stage
,SewingLineID
,MDivisionid
,NoOfDefect
,InsCtn = IIF(IsCombinePO=1,NULL,InsCtn)
,Qty
,CustPoNo
,Remark
,Result
,InspectQty
,Seq
,Shift
,OrderID
,SQR
,Status
,StyleID
,Team
,[TTL CTN] = TtlCtn.Val
,VasShas
into #Final_P_CFAInspectionRecord_Detail
FROM  #tmp t
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM #PackingList_Detail2 pd
	WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.Seq AND pd.CTNQty=1
)TtlCtn
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM #PackingList_Detail pd
	WHERE pd.StaggeredCFAInspectionRecordID= t.ID AND pd.CTNQty=1
)InspectedCtn
OUTER APPLY(
	SELECT [Val] = SUM(DISTINCT pd.ShipQty)
	FROM #PackingList_Detail pd
	WHERE pd.StaggeredCFAInspectionRecordID= t.ID
)InspectedPoQty
Order by id

DROP TABLE #tmp ,#PackingList_Detail ,#MainData ,#PackingList_Detail2,#MainData1
'

set @SqlFinal1 = '
BEGIN TRY
Begin tran

If Exists(Select * From PBIReportData.sys.tables Where Name = ''P_DQSDefect_Summary'')
DELETE T FROM P_DQSDefect_Summary T WHERE EXISTS(SELECT * FROM ['+@LinkServerName+'].Production.dbo.factory S WHERE T.FactoryID = S.ID)
;
If Exists(Select * From PBIReportData.sys.tables Where Name = ''P_DQSDefect_Detail'') 
DELETE T FROM P_DQSDefect_Detail T WHERE EXISTS(SELECT * FROM ['+@LinkServerName+'].Production.dbo.factory S WHERE T.FactoryID = S.ID)
;
If Exists(Select * From PBIReportData.sys.tables Where Name = ''P_CFAInline_Detail'') 
DELETE T FROM P_CFAInline_Detail T WHERE EXISTS(SELECT * FROM ['+@LinkServerName+'].Production.dbo.factory S WHERE T.FactoryID = S.ID)
;
If Exists(Select * From PBIReportData.sys.tables Where Name = ''P_CFAInspectionRecord_Detail'')
DELETE T FROM P_CFAInspectionRecord_Detail T WHERE EXISTS(SELECT * FROM ['+@LinkServerName+'].Production.dbo.factory S WHERE T.FactoryID = S.ID)
;

INSERT INTO [dbo].[P_DQSDefect_Summary]
           ([FirstInspectDate]
           ,[FactoryID]
           ,[BrandID]
           ,[StyleID]
           ,[POID]
           ,[SPNO]
           ,[Article]
           ,[SizeCode]
           ,[Destination]
           ,[CDCode]
           ,[ProductionFamilyID]
           ,[Team]
           ,[QCName]
           ,[Shift]
           ,[Line]
           ,[Cell]
           ,[InspectQty]
           ,[RejectQty]
           ,[WFT]
           ,[RFT])
 select * from #Final_DQSDefect_Summary

 INSERT INTO [dbo].[P_DQSDefect_Detail]
           ([FtyZon]
           ,[BrandID]
           ,[BuyerDelivery]
           ,[Line]
           ,[FactoryID]
           ,[Team]
           ,[Shift]
           ,[POID]
           ,[StyleID]
           ,[SPNO]
           ,[Article]
           ,[Status]
           ,[FixType]
           ,[FirstInspectDate]
           ,[FirstInspectTime]
           ,[InspectQCName]
           ,[FixedTime]
           ,[FixedQCName]
           ,[ProductType]
           ,[SizeCode]
           ,[DefectTypeDesc]
           ,[DefectCodeDesc]
           ,[AreaCode]
           ,[ReworkCardNo]
           ,[GarmentDefectTypeID]
           ,[GarmentDefectCodeID])
 select * from #Final_DQSDefect_Detail  
'

set @SqlFinal2 ='
INSERT INTO [dbo].[P_CFAInline_Detail]
           ([Action]
           ,[Area]
           ,[AuditDate]
           ,[BrandID]
           ,[BuyerDelivery]
           ,[CfaName]
           ,[DefectDesc]
           ,[DefectQty]
           ,[Destination]
           ,[FactoryID]
           ,[GarmentOutput]
           ,[InspectionStage]
           ,[Line]
           ,[NumberDefect]
           ,[OrderQty]
           ,[POID]
           ,[Remark]
           ,[Result]
           ,[InspectQty]
           ,[Shift]
           ,[SPNO]
           ,[SQR]
           ,[StyleID]
           ,[Team]
           ,[VASSHAS])
select * from #Final_P_CFAInline_Detail

INSERT INTO [dbo].[P_CFAInspectionRecord_Detail]
           ([Action]
           ,[AreaCodeDesc]
           ,[AuditDate]
           ,[BrandID]
           ,[BuyerDelivery]
           ,[CfaName]
           ,[ClogReceivedPercentage]
           ,[DefectDesc]
           ,[DefectQty]
           ,[Destination]
           ,[FactoryID]
           ,[Carton]
           ,[InspectedCtn]
           ,[InspectedPoQty]
           ,[InspectionStage]
           ,[SewingLineID]
           ,[Mdivisionid]
           ,[NoOfDefect]
           ,[NoOfInspection]
           ,[OrderQty]
           ,[PONO]
           ,[Remark]
           ,[Result]
           ,[SampleLot]
           ,[Seq]
           ,[Shift]
           ,[SPNO]
           ,[SQR]
           ,[Status]
           ,[StyleID]
           ,[Team]
           ,[TtlCTN]
           ,[VasShas])
select * from #Final_P_CFAInspectionRecord_Detail
'

set @SqlFinal = '
drop table #Final_DQSDefect_Summary
drop table #Final_DQSDefect_Detail
drop table #Final_P_CFAInline_Detail
drop table #Final_P_CFAInspectionRecord_Detail
Commit tran

END TRY
BEGIN CATCH
	RollBack Tran
	declare @ErrMsg varchar(1000) = ''Err# : '' + ltrim(str(ERROR_NUMBER())) + 
				CHAR(10)+''Error Severity:''+ltrim(str(ERROR_SEVERITY()  )) +
				CHAR(10)+''Error State:'' + ltrim(str(ERROR_STATE() ))  +
				CHAR(10)+''Error Proc:'' + isNull(ERROR_PROCEDURE(),'''')  +
				CHAR(10)+''Error Line:''+ltrim(str(ERROR_LINE()  )) +
				CHAR(10)+''Error Msg:''+ ERROR_MESSAGE() ;
    
    RaisError( @ErrMsg ,16,-1)

END CATCH
'

SET @SqlCmd_Combin = @SqlCmd1 + @SqlCmd2 + @SqlCmd3 + @SqlCmd4 + @SqlFinal1 + @SqlFinal2 + @SqlFinal
/*
print @SqlCmd1
print @SqlCmd2
print @SqlCmd3
print @SqlCmd4
print @SqlFinal1 
print @SqlFinal2 
print @SqlFinal
*/
EXEC sp_executesql  @SqlCmd_Combin

END
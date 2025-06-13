CREATE PROCEDURE [dbo].[P_ImportQAInspection_Fty] 
	@InspectionDate Date

AS
BEGIN
	SET NOCOUNT ON;
declare @sDate varchar(20) = cast(@InspectionDate as varchar) -- 2020/01/01

declare @SqlCmd_Combin nvarchar(max) =''
declare @SqlCmd1 nvarchar(max) ='';
declare @SqlCmd1_1 nvarchar(max) ='';
declare @SqlCmd2 nvarchar(max) ='';
declare @SqlCmd3 nvarchar(max) ='';
declare @SqlCmd4 nvarchar(max) ='';
declare @SqlCmd5 nvarchar(max) ='';
declare @SqlCmd6 nvarchar(max) ='';

declare @SqlFinal1 nvarchar(max) = ''
declare @SqlFinal2 nvarchar(max) = ''
declare @SqlFinal3 nvarchar(max) = ''
declare @SqlFinal  nvarchar(max) = ''

/*判斷當前Server後, 指定帶入正式機Server名稱*/
	declare @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)
	--依不同Server來抓到對應的備機ServerName
	--declare @current_PMS_ServerName nvarchar(50) 
	--= (
	--	select [value] = 
	--		CASE WHEN @current_ServerName= 'PHL-NEWPMS-02' THEN 'PHL-NEWPMS' -- PH1
	--			 WHEN @current_ServerName= 'VT1-PH2-PMS2b' THEN 'VT1-PH2-PMS2' -- PH2
	--			 WHEN @current_ServerName= 'system2017BK' THEN 'SYSTEM2017' -- SNP
	--			 WHEN @current_ServerName= 'SPS-SQL2' THEN 'SPS-SQL.spscd.com' -- SPS
	--			 WHEN @current_ServerName= 'SQLBK' THEN 'PMS-SXR' -- SPR
	--			 WHEN @current_ServerName= 'newerp-bak' THEN 'newerp' -- HZG		
	--			 WHEN @current_ServerName= 'SQL' THEN 'MainServer' -- HXG
	--			 when (select top 1 MDivisionID from Production.dbo.Factory) in ('VM2','VM1') then 'SYSTEM2016' -- ESP & SPT
	--		ELSE '' END
	--)

	declare @current_PMS_ServerName nvarchar(50) = 'MainServer' 

set @SqlCmd1 = '
-- MES/Endline/R08
select [InspectionDate] = ins.InspectionDate
	,[FirstInspectionDate] = cast(Ins.AddDate as date)
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
	,[CDCodeNew] = sty.CDCodeNew
	,[ProductType] = sty.ProductType
	,[FabricType] = sty.FabricType
	,[Lining] = sty.Lining
	,[Gender] = sty.Gender
	,[Construction] = sty.Construction
	,ord.SewInLine
into #tmp_summy_first
from ManufacturingExecution.dbo.Inspection ins WITH(NOLOCK) 
inner join ['+@current_PMS_ServerName+'].Production.dbo.Orders ord WITH(NOLOCK) on ins.OrderId=ord.id
inner join ['+@current_PMS_ServerName+'].Production.dbo.Factory fac WITH(NOLOCK) on ins.FactoryID=fac.ID
left  join ['+@current_PMS_ServerName+'].Production.dbo.SewingLine s WITH(NOLOCK) on s.FactoryID = ins.FactoryID and s.ID = ins.Line
left  join ['+@current_PMS_ServerName+'].Production.dbo.Country Cou WITH(NOLOCK) on ord.Dest = Cou.ID
left  join ['+@current_PMS_ServerName+'].Production.dbo.CDCode cdc WITH(NOLOCK) on ord.CdCodeID = cdc.ID
Outer apply (
	SELECT s.[ID]
		, ProductType = r2.Name
		, FabricType = r1.Name
		, Lining
		, Gender
		, Construction = d1.Name
		, s.CDCodeNew
	FROM ['+@current_PMS_ServerName+'].Production.dbo.Style s WITH(NOLOCK)
	left join ['+@current_PMS_ServerName+'].Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= ''StyleConstruction'' and d1.ID = s.Construction
	left join ['+@current_PMS_ServerName+'].Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= ''Fabric_Kind'' and r1.ID = s.FabricType
	left join ['+@current_PMS_ServerName+'].Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= ''Style_Apparel_Type'' and r2.ID = s.ApparelType
	where s.Ukey = ord.StyleUkey
)sty
where ins.Adddate >= '''+@sDate+'''
group by ins.InspectionDate, cast(Ins.AddDate as date), ins.FactoryID, ord.BrandID, ord.styleid, ord.custpono, 
ins.OrderId, ins.Article, ins.Size, Cou.Alias, ord.CdCodeID, cdc.ProductionFamilyID,
ins.Team, ins.AddName, ins.Shift, ins.Line, s.SewingCell, sty.CDCodeNew,
sty.ProductType, sty.FabricType, sty.Lining, sty.Gender, sty.Construction,ord.SewInLine
'

set @SqlCmd1_1 = '
select t.InspectionDate 
	, t.FirstInspectionDate
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
	,[Endline RFT(%)] =isnull(RftValue.VAL,0)
	, t.CDCodeNew
	, t.ProductType
	, t.FabricType
	, t.Lining
	, t.Gender
	, t.Construction
	, detail.DefectQty
into #Final_DQSDefect_Summary
from #tmp_summy_first t
outer apply
(
    SELECT 
    VAL = isnull(Convert(float(50),Convert(FLOAT(50), round(((R.InspectQty-R.RejectQty)/ nullif(R.InspectQty, 0))*100,2))),0)
    FROM ['+@current_PMS_ServerName+'].Production.dbo.SewingOutput_Detail sod 
    inner join ['+@current_PMS_ServerName+'].Production.dbo.SewingOutput so with(nolock) on so.id = sod.id
    inner join ['+@current_PMS_ServerName+'].Production.dbo.Rft r on r.OrderID = sod.OrderId AND
											   r.SewinglineID = so.SewingLineID AND
											   r.Team = so.Team AND
											   r.Shift = so.Shift AND
											   r.CDate = so.OutputDate
    WHERE sod.OrderId = t.[SP#] and so.SewinglineID = t.Line and so.FactoryID = t.Factory And sod.Article = t.Article
	and so.Shift= iif(t.Shift = ''Day'',''D'',''N'') 
	and r.CDate = t.SewInLine
)RftValue
outer apply (
	select DefectQty = COUNT(*)
	from ManufacturingExecution.dbo.Inspection insp WITH(NOLOCK) 
	inner join ManufacturingExecution.dbo.Inspection_Detail ind WITH(NOLOCK) on insp.ID = ind.ID
	where insp.Adddate >= '''+@sDate+'''
	and insp.Status <> ''Pass'' 
	and t.InspectionDate = insp.InspectionDate
	and t.FirstInspectionDate = cast(insp.AddDate as Date)
	and t.Factory = insp.FactoryID
	and t.[SP#] = insp.OrderId
	and t.Article = insp.Article
	and t.Size = insp.Size
	and t.Team = insp.Team
	and t.AddName = insp.AddName
	and t.Shift = insp.Shift
	and t.Line = insp.Line
) detail
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
	, [DefectCodeLocalDesc] = iif(isnull(gdc.LocalDescription,'''') = '''',gdc.Description,gdc.LocalDescription)
	, [IsCriticalDefect] = iif(isnull(IsCriticalDefect,0) = 0, '''', ''Y'')
	, [InspectionDetailUkey] = ind.Ukey
into #Final_DQSDefect_Detail
from ManufacturingExecution.dbo.Inspection ins WITH(NOLOCK)
inner join ['+@current_PMS_ServerName+'].Production.dbo.orders ord WITH(NOLOCK) on ins.OrderId=ord.id
inner join ['+@current_PMS_ServerName+'].Production.dbo.Factory fac WITH(NOLOCK) on ins.FactoryID=fac.ID
left  join ManufacturingExecution.dbo.Inspection_Detail ind WITH(NOLOCK) on ins.id=ind.ID	
left  join ['+@current_PMS_ServerName+'].Production.dbo.GarmentDefectCode gdc WITH(NOLOCK) on ind.GarmentDefectTypeID=gdc.GarmentDefectTypeID and ind.GarmentDefectCodeID=gdc.ID
left  join ['+@current_PMS_ServerName+'].Production.dbo.GarmentDefectType gdt WITH(NOLOCK) on gdc.GarmentDefectTypeID=gdt.ID
outer apply(select name from ManufacturingExecution.dbo.pass1 p WITH(NOLOCK) where p.id= ins.AddName) Inspection_QC
outer apply(select name from ManufacturingExecution.dbo.pass1 p WITH(NOLOCK) where p.id= ins.EditName) Inspection_fixQC
where ins.Adddate >= '''+@sDate+'''
and ins.Status <> ''Pass''
Order by Zone,[Brand],[Factory],Line,Team,[SP#],Article,[ProductType],Size,[DefectTypeID],[DefectCodeID]

drop table #tmp_summy_first
'

set @SqlCmd3 = '
-- PMS/QA/R21
select DISTINCT
[Action]= b.Action
,[Area]= b.CFAAreaID +'' - ''+ar.Description
,a.cDate
,c.BrandID
, c.BuyerDelivery 
,[Cfa] = isnull((select CONCAT(a.CFA, '':'', Name) from ['+@current_PMS_ServerName+'].Production.dbo.Pass1  WITH (NOLOCK) where ID = a.CFA),'''')
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
from ['+@current_PMS_ServerName+'].Production.dbo.Cfa a WITH (NOLOCK) 
inner join ['+@current_PMS_ServerName+'].Production.dbo.Cfa_Detail b WITH (NOLOCK) on b.id = a.ID 
inner join ['+@current_PMS_ServerName+'].Production.dbo.orders c WITH (NOLOCK) on c.id = a.OrderID
inner JOIN ['+@current_PMS_ServerName+'].Production.dbo.Country ct WITH (NOLOCK) ON ct.ID=c.Dest
outer apply(select Description from ['+@current_PMS_ServerName+'].Production.dbo.GarmentDefectCode a WITH(NOLOCK) where a.id=b.GarmentDefectCodeID) as gd
outer apply(select description from ['+@current_PMS_ServerName+'].Production.dbo.cfaarea a WITH(NOLOCK) where a.id=b.CFAAreaID) as ar
where a.Status = ''Confirmed'' and a.cDate >= '''+@sDate+'''
'

set @SqlCmd4 = '
--PMS/QA/R32

SELECT c.[ID]
      ,c.[AuditDate]
      ,c.[FactoryID]
      ,c.[MDivisionid]
      ,c.[SewingLineID]
      ,c.[Team]
      ,c.[Shift]
      ,c.[Stage]
      ,co.[Carton]
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
	  ,c.[FirstInspection]
,co.OrderID,co.SEQ
INTO #MainData1
From ['+@current_PMS_ServerName+'].Production.dbo.CFAInspectionRecord c WITH(NOLOCK)
INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.CFAInspectionRecord_OrderSEQ co WITH(NOLOCK) ON c.ID = co.ID
INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.Orders O WITH(NOLOCK) ON o.ID = co.OrderID
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
      ,co.[Carton]
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
	  ,c.[FirstInspection]
	  ,co.OrderID
	  ,co.SEQ
	  ,[InspectedSP] = cfos.OrderID
	  ,[InspectedSeq] = cfos.Seq
	  ,[ReInspection] =  iif(c.ReInspection =1, 1, 0)
INTO #MainData
From ['+@current_PMS_ServerName+'].Production.dbo.CFAInspectionRecord  c
INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
outer apply (
	select top 1 OrderID, Seq
	from  ['+@current_PMS_ServerName+'].Production.dbo.CFAInspectionRecord_OrderSEQ cfos 
	where c.ID = cfos.ID
	ORDER BY Ukey
) cfos
WHERE c.ID IN (SELECT ID FROM #MainData1)
'

set @SqlCmd5 = '

SELECT 
	 c.AuditDate
	,BuyerDelivery = (SELECT BuyerDelivery FROM ['+@current_PMS_ServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,c.OrderID
	,CustPoNo = (SELECT CustPoNo FROM ['+@current_PMS_ServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,StyleID = (SELECT StyleID FROM ['+@current_PMS_ServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,BrandID = (SELECT BrandID FROM ['+@current_PMS_ServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,Dest = (SELECT Dest FROM ['+@current_PMS_ServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,Seq = (SELECT Seq FROM ['+@current_PMS_ServerName+'].Production.dbo.Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
	,c.SewingLineID
	,[VasShas]= (SELECT IIF(VasShas=1,''Y'',''N'')  FROM ['+@current_PMS_ServerName+'].Production.dbo.Orders WHERE ID = c.OrderID)
	,c.ClogReceivedPercentage
	,c.MDivisionid
	,c.FactoryID
	,c.Shift
	,c.Team
	,Qty = (SELECT Qty FROM ['+@current_PMS_ServerName+'].Production.dbo.Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
	,c.Status
	,[Carton] = IIF(c.Carton ='''' AND c.Stage = ''3rd party'',''N/A'',c.Carton)
	,[CfA] = isnull((select CONCAT(c.CFA, '':'', Name) from ['+@current_PMS_ServerName+'].Production.dbo.Pass1  WITH (NOLOCK) where ID = c.CFA),'''')
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
		FROM ['+@current_PMS_ServerName+'].Production.dbo.CFAInspectionRecord cr
		INNER JOIN ['+@current_PMS_ServerName+'].Production.dbo.CFAInspectionRecord_OrderSEQ crd ON cr.ID = crd.ID
		WHERE crd.OrderID=c.OrderID AND crd.SEQ=c.SEQ
	    AND cr.Status = ''Confirmed''
	    AND cr.Stage=c.Stage
	    AND cr.AuditDate <= c.AuditDate
	    AND cr.ID  != c.ID
	)
	,NULL)
	,[Action]= cd.Action
	,[CFAInspectionRecord_Detail_Key]= concat(c.ID,iif(isnull(cd.GarmentDefectCodeID, '''') = '''', concat(row_Number()over(order by c.ID),''''), cd.GarmentDefectCodeID))
	,c.FirstInspection
	,c.[InspectedSP]
	,c.[InspectedSeq] 
	,c.[ReInspection]
INTO #tmp
FROm #MainData  c
LEFT JOIN ['+@current_PMS_ServerName+'].Production.dbo.CFAInspectionRecord_Detail cd ON c.ID = cd.ID
LEFT JOIN ['+@current_PMS_ServerName+'].Production.dbo.GarmentDefectCode g ON g.ID = cd.GarmentDefectCodeID
LEFT JOIN ['+@current_PMS_ServerName+'].Production.dbo.CfaArea ON CfaArea.ID = cd.CFAAreaID
'

set @SqlCmd6 = '

SELECT pd.*
INTO #PackingList_Detail
FROM ['+@current_PMS_ServerName+'].Production.dbo.PackingList_Detail pd
WHERE EXISTS (SELECT 1 FROM #tmp t WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.SEQ) 

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
	,FirstInspection  = IIF(FirstInspection = 1, ''Y'','''')
	,t.[InspectedSP]
	,t.[InspectedSeq] 
	,t.[ReInspection]
into #Final_P_CFAInspectionRecord_Detail
FROM  #tmp t
OUTER APPLY(
	SELECT [Val] = COUNT(1)
	FROM #PackingList_Detail pd
	WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.Seq 
	AND pd.CTNQty > 0 AND pd.CTNStartNo!=''''
)TtlCtn
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM #PackingList_Detail pd
	WHERE pd.OrderID = t.OrderID  AND pd.OrderShipmodeSeq = t.Seq
	AND ('','' + t.Carton + '','') like (''%,'' + pd.CTNStartNo + '',%'')
	AND pd.CTNQty=1
)InspectedCtn
OUTER APPLY(
	SELECT [Val] = SUM(pd.ShipQty)
	FROM #PackingList_Detail pd
	WHERE pd.OrderID = t.OrderID  AND pd.OrderShipmodeSeq = t.Seq
	AND ('','' + t.Carton + '','') like (''%,'' + pd.CTNStartNo + '',%'')
)InspectedPoQty
Order by id

DROP TABLE #tmp ,#PackingList_Detail ,#MainData ,#MainData1
'

set @SqlFinal1 = '

If Exists(Select * From POWERBIReportData.sys.tables Where Name = ''P_DQSDefect_Summary'')
DELETE T FROM P_DQSDefect_Summary T WHERE EXISTS(SELECT * FROM ['+@current_PMS_ServerName+'].Production.dbo.factory S WHERE T.FactoryID = S.ID)
;
If Exists(Select * From POWERBIReportData.sys.tables Where Name = ''P_DQSDefect_Detail'') 
DELETE T FROM P_DQSDefect_Detail T WHERE EXISTS(SELECT * FROM ['+@current_PMS_ServerName+'].Production.dbo.factory S WHERE T.FactoryID = S.ID)
;
If Exists(Select * From POWERBIReportData.sys.tables Where Name = ''P_CFAInline_Detail'') 
DELETE T FROM P_CFAInline_Detail T WHERE EXISTS(SELECT * FROM ['+@current_PMS_ServerName+'].Production.dbo.factory S WHERE T.FactoryID = S.ID)
;
If Exists(Select * From POWERBIReportData.sys.tables Where Name = ''P_CFAInspectionRecord_Detail'')
DELETE T FROM P_CFAInspectionRecord_Detail T WHERE EXISTS(SELECT * FROM ['+@current_PMS_ServerName+'].Production.dbo.factory S WHERE T.FactoryID = S.ID)
;

INSERT INTO [dbo].[P_DQSDefect_Summary]
           ([InspectionDate]
		   ,[FirstInspectDate]
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
           ,[RFT]
		   ,[CDCodeNew]
		   ,[ProductType]
		   ,[FabricType]
		   ,[Lining]
		   ,[Gender]
		   ,[Construction]
		   ,[DefectQty])
 select　InspectionDate 
	, FirstInspectionDate
	, isnull(Factory,'''')
	, isnull(Brand,'''')
	, isnull(Style,'''')
	, isnull([PO#], '''')
	, isnull([SP#], '''')
	, isnull(Article, '''')
	, isnull(Size, '''')
	, isnull(Destination, '''')
	, isnull(CdCodeID, '''')
	, isnull(ProductionFamilyID,'''')
	, isnull(Team, '''')
	, isnull(AddName ,'''')
	, isnull([Shift], '''')
	, isnull(Line, '''')
	, isnull(SewingCell, '''')	
	, isnull(TtlQty, 0)
	, isnull(RejectAndFixedQty, 0)
	, isnull([EndlineWFT], 0) 
	, isnull([Endline RFT(%)], 0) 
	, isnull(CDCodeNew, '''')
	, isnull(ProductType,'''')
	, isnull(FabricType, '''')
	, isnull(Lining,'''')
	, isnull(Gender,'''')
	, isnull(Construction,'''')
	, isnull(DefectQty, 0)
	from #Final_DQSDefect_Summary'

set @SqlFinal2 ='

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
           ,[GarmentDefectCodeID]
		   ,[DefectCodeLocalDesc]
		   ,[IsCriticalDefect])
		   ,[InspectionDetailUkey]
 select [Zone] = isnull([Zone],'''')
    , Brand = isnull(Brand,'''')
	, [Buyer Delivery Date]
	, Line = isnull(Line,'''')
	, [Factory] = isnull(Factory,'''')
	, Team = isnull(Team,'''')
	, [Shift] = isnull([Shift],'''')
 	, [PO#] = isnull([PO#],'''')
	, [Style] = isnull(Style,'''')
	, [SP#] = isnull([SP#],'''')
	, Article = isnull(Article,'''')
	, [Status] = isnull([Status],'''')
	, [FixType] = isnull(FixType,'''')
	, [FirstInspectionDate]
	, [FirstInspectedTime]
	, [Inspected QC] = isnull([Inspected QC],'''')
	, [Fixed Time] = isnull([Fixed Time],'''')
	, [Fixed QC] = isnull([Fixed QC],'''')
	, [ProductType] = isnull(ProductType,'''')
	, [Size]= isnull(Size,'''')
	, [DefectTypeDescritpion] = isnull([DefectTypeDescritpion],'''')
	, [DefectCodeDescritpion] = isnull([DefectCodeDescritpion],'''')
	, [Area] = isnull(Area,'''')
	, [ReworkCardNo] = isnull(ReworkCardNo,''''), [DefectTypeID] = isnull(DefectTypeID,''''), [DefectCodeID] = isnull(DefectCodeID,''''), DefectCodeLocalDesc = isnull(DefectCodeLocalDesc,''''), [IsCriticalDefect] = isnull(IsCriticalDefect,'''')
	, [InspectionDetailUkey] = isnull(InspectionDetailUkey,0)
 from #Final_DQSDefect_Detail  
'

set @SqlFinal3 ='
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
select 
	 [Action]= isnull([Action] ,'''' )
	,[Area]= isnull([Area] ,'''')
	,cDate
	,isnull(BrandID ,'''')
	,BuyerDelivery 
	,[Cfa] = isnull([Cfa],'''')
	,[Defect Description]= isnull([Defect Description],'''')
	,isnull(DefectQty, 0)
	,[Destination]=isnull([Destination],'''')
	,isnull(FactoryID,'''') 
	,[GarmentOutput]= isnull([GarmentOutput],0)
	,[Stage]= isnull([Stage],'''')
	,isnull(SewingLineID,'''')
	,isnull([No. Of Defect],0)
	,isnull(Qty,0)
	,isnull(CustPONo, '''')
	,isnull([Remark], '''')
	,isnull([Result], '''')
	,isnull(InspectQty, 0)		
	,isnull([shift], '''')
	,isnull(OrderID, '''')
	,isnull([SQR], 0)
	,isnull(StyleID, '''')
	,isnull(Team, '''')
	,isnull([VAS/SHAS], '''') 
from #Final_P_CFAInline_Detail

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
           ,[VasShas]
		   ,[1st_Inspection]
		   ,[InspectedSP]
		   ,[InspectedSeq] 
		   ,[ReInspection]
		   )
select 
	isnull(Action ,'''')
	,isnull(AreaCodeDesc , '''')
	,AuditDate
	,isnull(BrandID, '''')
	,BuyerDelivery
	,isnull(CFA, '''')
	,isnull(ClogReceivedPercentage , 0)
	,isnull(DefectDescription, '''')
	,isnull(DefectQty, 0)
	,isnull(Dest ,'''')
	,isnull(FactoryID, '''')
	,isnull(Carton, '''')
	,isnull([Inspected Ctn], 0)
	,isnull([Inspected PoQty], 0)
	,isnull(Stage ,'''')
	,isnull(SewingLineID, '''')
	,isnull(MDivisionid, '''')
	,isnull(NoOfDefect, 0)
	,isnull(Qty, 0)
	,isnull(CustPoNo, '''')
	,isnull(Remark, '''')
	,isnull(Result, '''')
	,isnull(InspectQty, 0)
	,isnull(Seq ,'''')
	,isnull(Shift, '''')
	,isnull(OrderID, '''')
	,isnull(SQR, 0)
	,isnull(Status,'''')
	,isnull(StyleID, '''')
	,isnull(Team, '''')
	,isnull([TTL CTN] , 0)
	,isnull(VasShas ,'''')
	,isnull(FirstInspection ,'''')
	,isnull([InspectedSP], '''')
	,isnull([InspectedSeq],'''') 
	,[ReInspection]
	from #Final_P_CFAInspectionRecord_Detail
'

set @SqlFinal = '
drop table #Final_DQSDefect_Summary
drop table #Final_DQSDefect_Detail
drop table #Final_P_CFAInline_Detail
drop table #Final_P_CFAInspectionRecord_Detail

update b
    set b.TransferDate = getdate()
		, b.IS_Trans = 1
from BITableInfo b
where b.id = ''P_DQSDefect_Summary''

update b
    set b.TransferDate = getdate()
		, b.IS_Trans = 1
from BITableInfo b
where b.id = ''P_DQSDefect_Detail'' 

update b
    set b.TransferDate = getdate()
		, b.IS_Trans = 1
from BITableInfo b
where b.id = ''P_CFAInline_Detail'' 

update b
    set b.TransferDate = getdate()
		, b.IS_Trans = 1
from BITableInfo b
where b.id = ''P_CFAInspectionRecord_Detail'' 
'

SET @SqlCmd_Combin = @SqlCmd1 + @SqlCmd1_1 + @SqlCmd2 + @SqlCmd3 + @SqlCmd4 + @SqlCmd5 + @SqlCmd6 + @SqlFinal1 + @SqlFinal2 + @SqlFinal3 + @SqlFinal
/*
print @SqlCmd1
print @SqlCmd1_1
print @SqlCmd2
print @SqlCmd3
print @SqlCmd4
print @SqlCmd5
print @SqlFinal1 
print @SqlFinal2 
print @SqlFinal3 
print @SqlFinal
*/

EXEC sp_executesql  @SqlCmd_Combin

END
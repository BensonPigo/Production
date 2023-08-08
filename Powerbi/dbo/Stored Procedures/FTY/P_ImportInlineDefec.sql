CREATE PROCEDURE [dbo].[P_ImportInlineDefec]
as
BEGIN
SET NOCOUNT ON
	DECLARE @SqlCmdAll NVARCHAR(MAX) = '';
	DECLARE @sqlcmddeclare NVARCHAR(MAX) = '';
	DECLARE @sqlcmdTmp NVARCHAR(MAX) = '';
	DECLARE @sqlcmdSummy1 NVARCHAR(MAX) = '';
	DECLARE @sqlcmdSummy2 NVARCHAR(MAX) = '';
	DECLARE @sqlcmdSummy3 NVARCHAR(MAX) = '';
	DECLARE @sqlcmdDetail NVARCHAR(MAX) = '';
	DECLARE @sqlcmdInsertSummy NVARCHAR(MAX) = '';
	DECLARE @sqlcmdInsertDetail NVARCHAR(MAX) = '';

	SET @sqlcmddeclare = '
	DECLARE @countS int =(SELECT COUNT(*) FROM P_InlineDefectSummary pids)
	DECLARE @StartDate date = IIF(@countS = 0 ,''2022/01/01'' , GETDATE()-1)
	DECLARE @EndDate date =  IIF(@countS = 0 , GETDATE() , GETDATE() -1) '

	SET @sqlcmdTmp = '
	select
	fac.Zone
	, ord.BrandID as [Brand]
	, ord.BuyerDelivery as [Buyer Delivery Date]
	, ins.FactoryID as [Factory]
	, ins.Line
	, s.SewingCell
	, ins.Team 
	, ins.Shift
	, ord.custpono as [PO#]
	, ord.styleid as [Style]
	, ins.OrderId as [SP#]
	, ins.Article
	, cast(Ins.AddDate as date) as [First Inspection Date]
	,  CONVERT(varchar,ins.AddDate, 111) as [Inspected Time]
	, Inspection_QC.Name as [Inspected QC]
	, [Destination] = Cou.Alias
	, ord.CdCodeID
	, ps.CDCodeNew
	, sty.ProductType
	, sty.FabricType
	, sty.Lining
	, sty.Gender
	, sty.Construction
	, cdc.ProductionFamilyID
	, [Product Type] = 
		case when ins.Location = ''T'' then ''TOP''
			when ins.Location = ''B'' then ''BOTTOM''
			when ins.Location = ''I'' then ''INNER''
			when ins.Location = ''O'' then ''OUTER''
			else ''''
		end 
	, ins.Operation
	, ins.SewerID + ''-'' + InlineEmployee.FirstName + '' '' + InlineEmployee.LastName as [Sewer Name]
	, ins.AddName
	, ins.PassWIP
	, ins.RejectWIP
	, ind.GarmentDefectTypeID as [Defect Type ID]
	, gdt.Description as [Defect Type Descritpion]
	, ind.GarmentDefectCodeID as [Defect Code ID]
	, gdc.Description as [Defect Code Descritpion] 
	, [IsCriticalDefect] = iif(isnull(gdc.IsCriticalDefect, 0) = 1, ''Y'', '''')
	into #tmp_src
	from ExtendServer.ManufacturingExecution.dbo.InlineInspection  ins  
	inner join MainServer.Production.dbo.Orders ord on ins.OrderId=ord.id
	inner join MainServer.Production.dbo.Factory fac on ins.FactoryID=fac.ID
	left join MainServer.Production.dbo.Style ps on ps.Ukey = ord.StyleUkey
	left join ExtendServer.ManufacturingExecution.dbo.InlineInspection_Detail ind on ins.id=ind.InlineInspectionID
	left join MainServer.Production.dbo.GarmentDefectCode gdc on ind.GarmentDefectTypeID=gdc.GarmentDefectTypeID and ind.GarmentDefectCodeID=gdc.ID
	left join MainServer.Production.dbo.GarmentDefectType gdt on gdc.GarmentDefectTypeID=gdt.ID
	left join MainServer.Production.dbo.SewingLine s on s.FactoryID = ins.FactoryID and s.ID = ins.Line
	left join MainServer.Production.dbo.Country Cou on ord.Dest = Cou.ID
	left join MainServer.Production.dbo.CDCode cdc on ord.CdCodeID = cdc.ID 
	outer apply(select FirstName, LastName from ExtendServer.ManufacturingExecution.dbo.InlineEmployee with(nolock) where InlineEmployee.EmployeeID= ins.SewerID) InlineEmployee
	outer apply(select name from ExtendServer.ManufacturingExecution.dbo.pass1 with(nolock) where pass1.id= ins.AddName) Inspection_QC
	outer apply(select name from ExtendServer.ManufacturingExecution.dbo.pass1 with(nolock) where pass1.id= ins.EditName) Inspection_fixQC
	Outer apply (
		SELECT ProductType = r2.Name
			, FabricType = r1.Name
			, Lining
			, Gender
			, Construction = d1.Name
		FROM MainServer.Production.dbo.Style s WITH(NOLOCK)
		left join MainServer.Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= ''StyleConstruction'' and d1.ID = s.Construction
		left join MainServer.Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= ''Fabric_Kind'' and r1.ID = s.FabricType
		left join MainServer.Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= ''Style_Apparel_Type'' and r2.ID = s.ApparelType
		where s.Ukey = ord.StyleUkey
	)sty
	where 
	ins.Adddate >= @StartDate AND
	ins.Adddate <=  @EndDate'
	SET @sqlcmdSummy1 = '
	select t.[First Inspection Date]
	,t.Factory
	,t.Brand
	,t.Style
	,t.[PO#]
	,t.[SP#]
    ,t.Article   
	,t.[Destination]
	,t.CdCodeID
    ,t.CDCodeNew
	,t.ProductType
	,t.FabricType
	,t.Lining
	,t.Gender
	,t.Construction
	,t.ProductionFamilyID
	,t.Team
	,AddName=(select stuff((select distinct concat('','',AddName) 
							from #tmp_src t2 
							where 1=1
							and t2.[First Inspection Date] = t.[First Inspection Date] 
							and t2.Factory = t.Factory 
							and t2.[SP#] = t.[SP#] 
							and t2.Team = t.Team 
							and t2.Shift = t.Shift 
							and t2.Line = t.Line for xml path('''') ),1,1,''''))
	,t.Shift
	,t.Line
	,t.SewingCell
	,t.PassWIP
	,t.RejectWIP
	into #tmp_Summy_first
	from #tmp_src t
	'
	SET @sqlcmdSummy2 = '
	select t.[First Inspection Date]
	,t.Factory
	,t.Brand
	,t.Style
	,t.[PO#]
	,t.[SP#]
	,t.[Destination]
	,t.CdCodeID
	,t.CDCodeNew
	,t.ProductType
	,t.FabricType
	,t.Lining
	,t.Gender
	,t.Construction
	,t.ProductionFamilyID
	,t.Team
	,t.AddName
	,t.Shift
	,t.Line
	,t.SewingCell
	,t.Article
	,[TtlQty]=SUM(t.PassWIP+t.RejectWIP) 
	,[PassQty] = Sum(t.PassWIP)
	,[RejectQty] = Sum(t.RejectWIP)
	into #tmp_Summy_Final
	from 
	(
		select t.[First Inspection Date]
				,t.Factory
				,t.Brand
				,t.Style
				,t.[PO#]
				,t.[SP#]
				,t.[Destination]
				,t.CdCodeID
				,t.CDCodeNew
				,t.ProductType
				,t.FabricType
				,t.Lining
				,t.Gender
				,t.Construction
				,t.ProductionFamilyID
				,t.Team
				,t.AddName
				,t.Shift
				,t.Line
				,t.SewingCell
				,sum(t.PassWIP) as [PassWIP]
				,sum(t.RejectWIP) as [RejectWIP]
				,t.Article
		from #tmp_Summy_first t
		group by t.[First Inspection Date],t.Factory,t.Brand,t.Style,t.[PO#],t.[SP#]
				,t.[Destination],t.CdCodeID,t.ProductionFamilyID,t.Team
				,t.AddName,t.Shift,t.Line,t.SewingCell, t.Article
				,t.CDCodeNew,t.ProductType,t.FabricType,t.Lining,t.Gender,t.Construction
	)t
	group by t.[First Inspection Date],t.Factory,t.Brand,t.Style,t.[PO#],t.[SP#]
			,t.[Destination],t.CdCodeID,t.ProductionFamilyID,t.Team
			,t.AddName,t.Shift,t.Line,t.SewingCell ,t.Article
			,t.CDCodeNew,t.ProductType,t.FabricType,t.Lining,t.Gender,t.Construction
	'
	SET @sqlcmdSummy3 = '
	select 
	t.[First Inspection Date]
	,t.Factory
	,t.Brand
	,t.Style
	,t.[PO#]
	,t.[SP#]
	,t.Article
	,t.[Destination]
	,t.CdCodeID
	,t.CDCodeNew
	,t.ProductType
	,t.FabricType
	,t.Lining
	,t.Gender
	,t.Construction
	,t.ProductionFamilyID
	,t.Team
	,t.AddName as [QCName]
	,t.Shift
	,t.Line
	,t.SewingCell as [Cell]
	,t.TtlQty as [InspectedQty]
	,t.RejectQty as [RejectQty]
	,[InlineWFT]=ROUND( (t.RejectQty *1.0) / (t.TtlQty *1.0) *100,3)
	,[InlineRFT]=ROUND( (t.PassQty *1.0) / (t.TtlQty *1.0) *100,3)
	into #tmpSummy
	from #tmp_Summy_Final t
	Order by t.[First Inspection Date], t.Factory, t.Brand, t.[SP#],t.Article, t.Line, t.Team
	'
	SET @sqlcmdDetail = '
	select
    t.Zone
    , t.Brand
    , t.[Buyer Delivery Date]
    , t.[Factory]
    , t.Line
    , t.Team 
    , t.Shift
    , t.[PO#]
    , t.[Style]
    , t.[SP#]
    , t.Article
    , t.[First Inspection Date]
    , t.[Inspected Time] as [First Inspected Time]     , t.[Inspected QC]
    , t.[Product Type]
    , t.Operation
    , t.[Sewer Name]
    , t.[Defect Type ID]
    , t.[Defect Type Descritpion]
    , t.[Defect Code ID]
    , t.[Defect Code Descritpion]  
    , t.IsCriticalDefect
    , t.RejectWIP
	into #tmpDetail
	from #tmp_src t
	where 1=1
	and t.RejectWIP > 0
	Order by t.Zone, t.[Brand], t.[Factory], t.Line, t.Team, t.[SP#], t.Article, t.[Product Type], t.[Defect Type ID], t.[Defect Code ID]
	'
	SET @sqlcmdInsertSummy = '
	insert into P_InlineDefectSummary
	(
		[FirstInspectedDate]
      ,[FactoryID]
      ,[BrandID]
      ,[StyleID]
      ,[CustPoNo]
      ,[OrderID]
      ,[Article]
      ,[Alias]
      ,[CDCodeID]
      ,[CDCodeNew]
      ,[ProductType]
      ,[FabricType]
      ,[Lining]
      ,[Gender]
      ,[Construction]
      ,[ProductionFamilyID]
      ,[Team]
      ,[QCName]
      ,[Shift]
      ,[Line]
      ,[SewingCell]
      ,[InspectedQty]
      ,[RejectWIP]
      ,[InlineWFT ]
      ,[InlineRFT]
	)
	select
	t.[First Inspection Date]
	,t.Factory
	,t.Brand
	,t.Style
	,t.[PO#]
	,t.[SP#]
	,t.Article
	,isnull(t.[Destination],'''')
	,t.CdCodeID
	,t.CDCodeNew
	,t.ProductType
	,t.FabricType
	,t.Lining
	,t.Gender
	,t.Construction
	,isnull(t.ProductionFamilyID,'''')
	,t.Team
	,t.[QCName]
	,t.Shift
	,t.Line
	,t.[Cell]
	,t.[InspectedQty]
	,t.[RejectQty]
	,t.[InlineWFT]
	,t.[InlineRFT]
	from #tmpSummy t

	IF EXISTS (select 1 from BITableInfo b where b.id = ''P_InlineDefectSummary'')
	BEGIN
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = ''P_InlineDefectSummary''
	END
	ELSE 
	BEGIN
		insert into BITableInfo(Id, TransferDate)
		values(''P_InlineDefectSummary'', getdate())
	END
	'
	SET @sqlcmdInsertDetail ='
	insert into P_InlineDefectDetail
	(
	   [Zone]
      ,[BrandID]
      ,[BuyerDelivery]
      ,[FactoryID]
      ,[Line]
      ,[Team]
      ,[Shift]
      ,[CustPoNo]
      ,[StyleID]
      ,[OrderId]
      ,[Article]
      ,[FirstInspectionDate]
      ,[FirstInspectedTime]
      ,[InspectedQC]
      ,[ProductType]
      ,[Operation]
      ,[SewerName]
      ,[GarmentDefectTypeID]
      ,[GarmentDefectTypeDesc]
      ,[GarmentDefectCodeID]
      ,[GarmentDefectCodeDesc]
      ,[IsCriticalDefect]
	)
	select
	t.Zone
    , t.Brand
    , t.[Buyer Delivery Date]
    , t.[Factory]
    , t.Line
    , t.Team 
    , t.Shift
    , t.[PO#]
    , t.[Style]
    , t.[SP#]
    , t.Article
    , t.[First Inspection Date]
    , t.[First Inspected Time]     
	, isnull(t.[Inspected QC],'''')
    , t.[Product Type]
    , t.Operation
    , t.[Sewer Name]
    , t.[Defect Type ID]
    , t.[Defect Type Descritpion]
    , t.[Defect Code ID]
    , t.[Defect Code Descritpion]  
    , t.IsCriticalDefect
	From #tmpDetail t

	IF EXISTS (select 1 from BITableInfo b where b.id = ''P_InlineDefectDetail'')
	BEGIN
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = ''P_InlineDefectDetail''
	END
	ELSE 
	BEGIN
		insert into BITableInfo(Id, TransferDate)
		values(''P_InlineDefectDetail'', getdate())
	END
	drop table #tmp_src,#tmp_Summy_first,#tmp_Summy_Final,#tmpSummy,#tmpDetail
	'
	SET @SqlCmdAll = @sqlcmddeclare + @sqlcmdTmp + @sqlcmdSummy1 + @sqlcmdSummy2 + @sqlcmdSummy3 + @sqlcmdInsertSummy + @sqlcmdDetail + @sqlcmdInsertDetail

	EXEC sp_executesql @SqlCmdAll
END




CREATE PROCEDURE [dbo].[P_ImportEfficiencyBI]

@StartDate date,
@EndDate date,
@LinkServerName varchar(50)

AS

BEGIN

declare @SDate as varchar(20) = CAST(@StartDate AS varchar) 
declare @EDate as varchar(20) = CAST(@EndDate AS varchar)   

declare @SqlCmd_Combin nvarchar(max) =''
declare @SqlCmd1 nvarchar(max) ='';
declare @SqlCmd2 nvarchar(max) ='';
declare @SqlCmd3 nvarchar(max) ='';
declare @SqlCmd4 nvarchar(max) ='';

SET @SqlCmd1 = '

--根據條件撈基本資料
select s.id,s.OutputDate,s.Category,s.Shift,s.SewingLineID,s.Team,s.MDivisionID,s.FactoryID
	,sd.OrderId,sd.ComboType,ActManPower = s.Manpower,sd.WorkHour,sd.QAQty,sd.InlineQty
	,o.LocalOrder,o.CustPONo,OrderCategory = isnull(o.Category,''''),OrderType = isnull(o.OrderTypeID,''''), CASE WHEN ot.IsDevSample =1 THEN ''Y'' ELSE ''N'' END AS IsDevSample
	,OrderBrandID = case 
		when o.BrandID != ''SUBCON-I'' then o.BrandID
		when Order2.BrandID is not null then Order2.BrandID
		when StyleBrand.BrandID is not null then StyleBrand.BrandID
		else o.BrandID end  
    ,OrderCdCodeID = isnull(o.CdCodeID,'''')
	,OrderProgram = isnull(o.ProgramID,'''')  ,OrderCPU = isnull(o.CPU,0) ,OrderCPUFactor = isnull(o.CPUFactor,0) ,OrderStyle = isnull(o.StyleID,'''') ,OrderSeason = isnull(o.SeasonID,'''')
	,MockupBrandID= isnull(mo.BrandID,'''')   ,MockupCDCodeID= isnull(mo.MockupID,'''')
	,MockupProgram= isnull(mo.ProgramID,'''') ,MockupCPU= isnull(mo.Cpu,0),MockupCPUFactor= isnull(mo.CPUFactor,0),MockupStyle= isnull(mo.StyleID,''''),MockupSeason= isnull(mo.SeasonID,'''')	
    ,Rate = isnull(dbo.[P_GetOrderLocation_Rate](o.id,sd.ComboType,''['+@LinkServerName+']''),100)/100,System.StdTMS
	, [ori_QAQty] = sd.QAQty
	, [ori_InlineQty] = sd.InlineQty
    ,BuyerDelivery = format(o.BuyerDelivery,''yyyy/MM/dd'')
    ,OrderQty = o.Qty
    ,s.SubconOutFty
    ,s.SubConOutContractNumber
    ,o.SubconInSisterFty
    ,[SewingReasonDesc]=isnull(sr.SewingReasonDesc,'''')
    ,o.SciDelivery
	,[CDCodeNew] = s.CDCodeNew
into #tmpSewingDetail
from ['+@LinkServerName+'].Production.dbo.System WITH (NOLOCK),['+@LinkServerName+'].Production.dbo.SewingOutput s WITH (NOLOCK) 
inner join  ['+@LinkServerName+'].Production.dbo.SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join  ['+@LinkServerName+'].Production.dbo.Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join  ['+@LinkServerName+'].Production.dbo.Style WITH (NOLOCK) on Style.Ukey = o.StyleUkey
left join  ['+@LinkServerName+'].Production.dbo.Factory f WITH (NOLOCK) on o.FactoryID = f.id
left join ['+@LinkServerName+'].Production.dbo.OrderType ot WITH (NOLOCK) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
left join ['+@LinkServerName+'].Production.dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
outer apply
(
	select [SewingReasonDesc]=stuff((
		select concat('','',sr.ID+''-''+sr.Description)
		from ['+@LinkServerName+'].Production.dbo.SewingReason sr WITH (NOLOCK)
		inner join ['+@LinkServerName+'].Production.dbo.SewingOutput_Detail sd2 WITH (NOLOCK) on sd2.SewingReasonID=sr.ID
		where sr.Type=''SO'' 
		and sd2.id = s.id
		and sd2.OrderId = sd.OrderId
		for xml path('''')
	),1,1,'''')
)sr
outer apply( select BrandID from ['+@LinkServerName+'].Production.dbo.orders o1 WITH (NOLOCK) where o.CustPONo = o1.id) Order2
outer apply( select top 1 BrandID from ['+@LinkServerName+'].Production.dbo.Style WITH (NOLOCK) where id = o.StyleID and SeasonID = o.SeasonID and BrandID != ''SUBCON-I'') StyleBrand
where 1=1 
and s.Shift <>''O''
--排除non sister的資料o.LocalOrder = 1 and o.SubconInSisterFty = 0
and((o.LocalOrder <> 1 and o.SubconInType not in (1, 2)) or (o.LocalOrder = 1 and o.SubconInType <> 0))
and (s.OutputDate between '''+@SDate+''' and  '''+@EDate+'''
	OR cast(s.EditDate as date) between '''+@SDate+''' and  '''+@EDate+''' )
and f.Type != ''S''
';

SET @SqlCmd2 = '
select distinct ID
	,OutputDate
	,Category
	,Shift
	,SewingLineID
	,Team
	,FactoryID
	,MDivisionID
	,OrderId
	,ComboType
	,[ActManPower] = s.Manpower
	,WorkHour = sum(Round(WorkHour,3))over(partition by id,OrderId,ComboType)
	,QAQty = sum(QAQty)over(partition by id,OrderId,ComboType)
	,[InlineQty] = sum(InlineQty)over(partition by id,OrderId,ComboType)
	,LocalOrder,CustPONo,OrderCategory,OrderType,IsDevSample
	,OrderBrandID ,OrderCdCodeID ,OrderProgram ,OrderCPU ,OrderCPUFactor ,OrderStyle ,OrderSeason
	,MockupBrandID,MockupCDCodeID,MockupProgram,MockupCPU,MockupCPUFactor,MockupStyle,MockupSeason
	,Rate,StdTMS
	,ori_QAQty = sum(ori_QAQty)over(partition by id,OrderId,ComboType)
	,ori_InlineQty = sum(ori_InlineQty)over(partition by id,OrderId,ComboType)
    ,BuyerDelivery
    ,SciDelivery
    ,OrderQty
    ,SubconOutFty
    ,SubConOutContractNumber
    ,SubconInSisterFty
    ,SewingReasonDesc
	,CDCodeNew
	,[ProductType] = sty.ProductType
	,[FabricType] = sty.FabricType
	,[Lining] = sty.Lining
	,[Gender] = sty.Gender
	,[Construction] = sty.Construction
into #tmpSewingGroup
from #tmpSewingDetail t
outer apply(
	select s.Manpower 
	from ['+@LinkServerName+'].Production.dbo.SewingOutput s WITH (NOLOCK)
	where s.ID = t.ID
)s
Outer apply (
	SELECT ProductType = r2.Name
		, FabricType = r1.Name
		, Lining
		, Gender
		, Construction = d1.Name
	FROM ['+@LinkServerName+'].Production.dbo.Style s WITH(NOLOCK)
	left join ['+@LinkServerName+'].Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= ''StyleConstruction'' and d1.ID = s.Construction
	left join ['+@LinkServerName+'].Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= ''Fabric_Kind'' and r1.ID = s.FabricType
	left join ['+@LinkServerName+'].Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= ''Style_Apparel_Type'' and r2.ID = s.ApparelType
	where s.ID = t.OrderStyle 
	and s.SeasonID = t.OrderSeason 
	and s.BrandID = t.OrderBrandID
)sty

select distinct s.SewingLineID,s.FactoryID,[OrderStyle] = o.StyleID, [MockupStyle] = mo.StyleID,s.OutputDate
into #tmp_s1
from ['+@LinkServerName+'].Production.dbo.SewingOutput s WITH (NOLOCK)
inner join ['+@LinkServerName+'].Production.dbo.SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
left join ['+@LinkServerName+'].Production.dbo.Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
left join ['+@LinkServerName+'].Production.dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId  
inner join (select [maxOutputDate] = max(OutputDate),[minOutputDate] = dateadd(day,-90, min(OutputDate)) from #tmpSewingGroup) t on s.OutputDate between t.minOutputDate and t.maxOutputDate

select t.*
	,[LastShift] = IIF(t.Shift <> ''O'' and t.Category <> ''M'' and t.LocalOrder = 1, ''I'',t.Shift)
	,[FtyType] = f.Type
	,[FtyCountry] = f.CountryID
	,CumulateDate= isnull(cl.cumulate,1) 
into #tmp1stFilter
from #tmpSewingGroup t
left join ['+@LinkServerName+'].Production.dbo.Factory f WITH (NOLOCK) on t.FactoryID = f.ID
outer apply
(
	select cumulate = IIF(Count(1)=0, 1, Count(1))
	from (
		select distinct s.OutputDate
		from #tmp_s1 s
		where (s.OrderStyle = t.OrderStyle  or s.MockupStyle = t.MockupStyle)
		and s.SewingLineID = t.SewingLineID
		and s.OutputDate between dateadd(day,-90, t.OutputDate) and t.OutputDate
		and s.FactoryID = t.FactoryID
	) s
	where s.OutputDate >(
							select date = max(Date)
							from (
								select top 360 w.Hours, w.Date
								from ['+@LinkServerName+'].Production.dbo.WorkHour w WITH (NOLOCK)
								where w.FactoryID = t.FactoryID
								and w.SewingLineID = t.SewingLineID
								and w.Date between dateadd(day,-90, t.OutputDate) and t.OutputDate
							) w 
							left join (
								select distinct s.OutputDate
								from #tmp_s1 s
								where (s.OrderStyle = t.OrderStyle  or s.MockupStyle = t.MockupStyle)
								and s.SewingLineID = t.SewingLineID
								and s.OutputDate between dateadd(day,-90, t.OutputDate) and t.OutputDate
								and s.FactoryID = t.FactoryID
							) s on s.OutputDate = w.Date
							where s.OutputDate is null
						)
)cl 
where t.OrderCategory in (''B'',''S'')-----Artwork
'

SET @SqlCmd3 = '
-----by orderid & all ArtworkTypeID

select * INTO #Final from(
	select distinct
		 MDivisionID,t.FactoryID
		,t.ComboType
		,FtyType = iif(FtyType=''B'',''Bulk'',iif(FtyType=''S'',''Sample'',FtyType))
		,FtyCountry
        ,t.OutputDate
        ,t.SewingLineID
		,Shift =    CASE    WHEN t.LastShift=''D'' then ''Day''
                            WHEN t.LastShift=''N'' then ''Night''
                            WHEN t.LastShift=''O'' then ''Subcon-Out''
                            WHEN t.LastShift=''I'' and SubconInSisterFty = 1 then ''Subcon-In(Sister)''
                            else ''Subcon-In(Non Sister)'' end
		,t.SubconOutFty
        ,t.SubConOutContractNumber
        ,t.Team
        ,t.OrderId
        ,CustPONo
        ,t.BuyerDelivery
        ,t.SciDelivery
        ,t.OrderQty
		,Brand = IIF(t.Category=''M'',MockupBrandID,OrderBrandID)
		,Category = IIF(t.OrderCategory=''M'',''Mockup'',IIF(LocalOrder = 1,''Local Order'',IIF(t.OrderCategory=''B'',''Bulk'',IIF(t.OrderCategory=''S'',''Sample'',IIF(t.OrderCategory=''G'',''Garment'','''')))))
		,Program = IIF(t.Category=''M'',MockupProgram,OrderProgram)
		,OrderType
        ,IsDevSample
		,CPURate = IIF(t.Category=''M'',MockupCPUFactor,OrderCPUFactor)
		,Style = IIF(t.Category=''M'',MockupStyle,OrderStyle)
		,Season = IIF(t.Category=''M'',MockupSeason,OrderSeason)
		,CDNo = IIF(t.Category=''M'',MockupCDCodeID,OrderCdCodeID)+''-''+t.ComboType
		,ActManPower = ActManPower
		,WorkHour
		,ManHour = ROUND(ActManPower*WorkHour,2)
		,TargetCPU = ROUND(ROUND(ActManPower*WorkHour,2)*3600/StdTMS,2)
		,TMS = IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*StdTMS
		,CPUPrice = IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)
		,TargetQty = IIF(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)>0,ROUND(ROUND(ActManPower*WorkHour,2)*3600/StdTMS,2)/IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate),0)
		,t.QAQty
		,TotalCPU = ROUND(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty,3)
		,CPUSewer = IIF(ROUND(ActManPower*WorkHour,2)>0,(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/ROUND(ActManPower*WorkHour,2),0)
		,EFF = ROUND(IIF(ROUND(ActManPower*WorkHour,2)>0,((IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/(ROUND(ActManPower*WorkHour,2)*3600/StdTMS))*100,0),1)
		,RFT = IIF(ori_InlineQty = 0, 0, ROUND(ori_QAQty* 1.0 / ori_InlineQty * 1.0 * 100 ,2))
		,CumulateDate
		,DateRange = IIF(CumulateDate>=10,''>=10'',CONVERT(VARCHAR,CumulateDate))
		,InlineQty,Diff = t.QAQty-InlineQty
		,rate
        ,isnull(t.SewingReasonDesc,'''')SewingReasonDesc
		,t.CDCodeNew
		,t.ProductType
		,t.FabricType
		,t.Lining
		,t.Gender
		,t.Construction
    from #tmp1stFilter t )a
order by MDivisionID,FactoryID,OutputDate,SewingLineID,Shift,Team,OrderId

drop table #tmpSewingDetail,#tmp1stFilter,#tmpSewingGroup,#tmp_s1

'

SET @SqlCmd4= '

MERGE INTO P_SewingDailyOutput t --要被insert/update/delete的表
USING #Final s --被參考的表
   ON t.FactoryID=s.FactoryID  
   AND t.MDivisionID=s.MDivisionID 
   AND t.SewingLineID=s.SewingLineID 
   AND t.Team=s.Team 
   AND t.Shift=s.Shift 
   AND t.orderid=s.orderid 
   AND t.ComboType=s.ComboType 
   AND t.OutputDate = s.OutputDate

WHEN MATCHED THEN   
    UPDATE SET 
		t.MDivisionID =s.MDivisionID
		,t.FactoryID =s.FactoryID
		,t.ComboType =s.ComboType
		,t.Category =s.FtyType
		,t.CountryID =s.FtyCountry
		,t.OutputDate =s.OutputDate
		,t.SewingLineID =s.SewingLineID
		,t.Shift =s.Shift
		,t.SubconOutFty =s.SubconOutFty
		,t.SubConOutContractNumber =s.SubConOutContractNumber
		,t.Team =s.Team
		,t.OrderID =s.OrderID
		,t.CustPONo = s.CustPONo
		,t.BuyerDelivery = s.BuyerDelivery
		,t.OrderQty = s.OrderQty
		,t.BrandID = s.Brand
		,t.OrderCategory = s.Category
		,t.ProgramID = s.Program
		,t.OrderTypeID = s.OrderType
		,t.DevSample = s.IsDevSample
		,t.CPURate = s.CPURate
		,t.StyleID = s.Style
		,t.Season = s.Season
		,t.CdCodeID = s.CDNo
		,t.ActualManpower = s.ActManPower
		,t.NoOfHours = s.WorkHour
		,t.TotalManhours = s.ManHour
		,t.TargetCPU = s.TargetCPU
		,t.TMS = s.TMS
		,t.CPUPrice = s.CPUPrice
		,t.TargetQty = s.TargetQty
		,t.TotalOutputQty = s.QAQTY
		,t.TotalCPU = s.TotalCPU
		,t.CPUSewerHR = s.CPUSewer
		,t.EFF = s.EFF
		,t.RFT = s.RFT
		,t.CumulateOfDays = s.CumulateDate
		,t.DateRange = s.DateRange
		,t.ProdOutput = s.InlineQty
		,t.Diff = s.Diff
		,t.Rate = s.Rate
		,t.SewingReasonDesc = s.SewingReasonDesc
		,t.SciDelivery = s.SciDelivery
		,t.CDCodeNew = s.CDCodeNew
		,t.ProductType = s.ProductType
		,t.FabricType = s.FabricType
		,t.Lining = s.Lining
		,t.Gender = s.Gender
		,t.Construction = s.Construction
WHEN NOT MATCHED THEN
    INSERT VALUES (
			s.MDivisionID
           ,s.FactoryID
		   ,s.ComboType
           ,s.FtyType
           ,s.FtyCountry
           ,s.OutputDate
           ,s.SewingLineID
           ,s.Shift
           ,s.SubconOutFty
           ,s.SubConOutContractNumber
           ,s.Team
           ,s.OrderID
           ,s.CustPONo
           ,s.BuyerDelivery
           ,s.OrderQty
           ,s.Brand
           ,s.Category
           ,s.Program
           ,s.OrderType
           ,s.IsDevSample
           ,s.CPURate
           ,s.Style
           ,s.Season
           ,s.CDNo
           ,s.ActManPower
           ,s.WorkHour
           ,s.ManHour
           ,s.TargetCPU
           ,s.TMS
           ,s.CPUPrice
           ,s.TargetQty
           ,s.QAQTY
           ,s.TotalCPU
           ,s.CPUSewer
           ,s.EFF
           ,s.RFT
           ,s.CumulateDate
           ,s.DateRange
           ,s.InlineQty
           ,s.Diff
           ,s.Rate
           ,s.SewingReasonDesc
		   ,s.SciDelivery
		   ,s.CDCodeNew
		   ,s.ProductType
		   ,s.FabricType
		   ,s.Lining
		   ,s.Gender
		   ,s.Construction
		  );

delete t
from P_SewingDailyOutput t WITH (NOLOCK)
where t.OutputDate between '''+@SDate+''' and  '''+@EDate+'''
and exists (select OrderID from #Final f where t.FactoryID=f.FactoryID  AND t.MDivisionID=f.MDivisionID ) 
and not exists (
select OrderID from #Final s 
	where t.FactoryID=s.FactoryID  
	AND t.MDivisionID=s.MDivisionID 
	AND t.SewingLineID=s.SewingLineID 
	AND t.Team=s.Team 
	AND t.Shift=s.Shift 
	AND t.orderid=s.orderid 
	AND t.ComboType=s.ComboType 
	AND t.OutputDate = s.OutputDate);

'

SET @SqlCmd_Combin = @SqlCmd1 + @SqlCmd2 + @SqlCmd3 + @SqlCmd4
EXEC sp_executesql @SqlCmd_Combin

End
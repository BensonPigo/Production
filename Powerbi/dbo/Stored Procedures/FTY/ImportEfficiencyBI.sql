-- =============================================
-- Create date: 2020/03/20
-- Description:	Data Query Logic by PMS.Sewing R04, Import Data to P_SewingDailyOutput
-- =============================================
CREATE PROCEDURE [dbo].[ImportEfficiencyBI]

@StartDate date,
@EndDate date

AS

BEGIN

declare @SDate as varchar(20) = CAST(@StartDate AS varchar) 
declare @EDate as varchar(20) = CAST(@EndDate AS varchar)   

--根據條件撈基本資料
select s.id
	,s.OutputDate
	,s.Category
	,s.Shift
	,s.SewingLineID
	,s.Team
	,s.MDivisionID
	,s.FactoryID
	,sd.OrderId
	,sd.ComboType
	,[ActManPower] = s.Manpower
	,sd.WorkHour
	,sd.QAQty
	,sd.InlineQty
	,o.LocalOrder
	,o.CustPONo
	,OrderCategory = isnull(o.Category,'')
	,OrderType = isnull(o.OrderTypeID,'')
	,[IsDevSample] = CASE WHEN ot.IsDevSample =1 THEN 'Y' ELSE 'N' END
	,OrderBrandID = case 
		when o.BrandID != 'SUBCON-I' then o.BrandID
		when Order2.BrandID is not null then Order2.BrandID
		when StyleBrand.BrandID is not null then StyleBrand.BrandID
		else o.BrandID end  
    ,OrderCdCodeID = isnull(o.CdCodeID,'')
	,OrderProgram = isnull(o.ProgramID,'')  
	,OrderCPU = isnull(o.CPU,0) 
	,OrderCPUFactor = isnull(o.CPUFactor,0) 
	,OrderStyle = isnull(o.StyleID,'') 
	,OrderSeason = isnull(o.SeasonID,'')
	,MockupBrandID= isnull(mo.BrandID,'')   
	,MockupCDCodeID= isnull(mo.MockupID,'')
	,MockupProgram= isnull(mo.ProgramID,'') 
	,MockupCPU= isnull(mo.Cpu,0)
	,MockupCPUFactor= isnull(mo.CPUFactor,0)
	,MockupStyle= isnull(mo.StyleID,'')
	,MockupSeason= isnull(mo.SeasonID,'')	
    ,Rate = isnull(Production.dbo.GetOrderLocation_Rate(o.id,sd.ComboType),100)/100
	,System.StdTMS
	,InspectQty = isnull(r.InspectQty,0)
	,RejectQty = isnull(r.RejectQty,0)
    ,BuyerDelivery = format(o.BuyerDelivery,'yyyy/MM/dd')
    ,OrderQty = o.Qty
    ,s.SubconOutFty
    ,s.SubConOutContractNumber
    ,o.SubconInSisterFty
    ,[SewingReasonDesc]=isnull(sr.SewingReasonDesc,'')
    ,o.SciDelivery
into #tmpSewingDetail
from Production.dbo.System WITH (NOLOCK),Production.dbo.SewingOutput s WITH (NOLOCK) 
inner join Production.dbo.SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Production.dbo.Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join Production.dbo.Factory f WITH (NOLOCK) on o.FactoryID = f.id
left join Production.dbo.OrderType ot WITH (NOLOCK) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
left join Production.dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
outer apply
(
    select top 1 InspectQty,RejectQty 
    from Production.dbo.Rft r WITH (NOLOCK) 
    where r.OrderID = sd.OrderId and r.CDate = s.OutputDate and r.SewinglineID = s.SewingLineID and r.FactoryID = s.FactoryID and r.Shift = s.Shift and r.Team = s.Team
) r
outer apply
(
	select [SewingReasonDesc]=stuff((
		select concat('','',sr.ID+'-'+sr.Description)
		from Production.dbo.SewingReason sr
		inner join Production.dbo.SewingOutput_Detail sd2 WITH (NOLOCK) on sd2.SewingReasonID=sr.ID
		where sr.Type='SO' 
		and sd2.id = s.id
		and sd2.OrderId = sd.OrderId
		for xml path('')
	),1,1,'')
)sr
outer apply( select BrandID from Production.dbo.orders o1 where o.CustPONo = o1.id) Order2
outer apply( select top 1 BrandID from Production.dbo.Style where id = o.StyleID and SeasonID = o.SeasonID and BrandID != 'SUBCON-I') StyleBrand
where 1=1 
and s.Shift <>'O'
--�ư�non sister�����o.LocalOrder = 1 and o.SubconInSisterFty = 0
and((o.LocalOrder <> 1 and o.SubconInType not in (1, 2)) or (o.LocalOrder = 1 and o.SubconInType <> 0))
and (s.OutputDate between @SDate and  @EDate
	OR cast(s.EditDate as date) between @SDate and @EDate )
and f.Type != 'S'


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
	,Rate,StdTMS,InspectQty,RejectQty
    ,BuyerDelivery
    ,SciDelivery
    ,OrderQty
    ,SubconOutFty
    ,SubConOutContractNumber
    ,SubconInSisterFty
    ,SewingReasonDesc
into #tmpSewingGroup
from #tmpSewingDetail t
outer apply(
	select s.Manpower 
	from Production.dbo.SewingOutput s
	where s.ID = t.ID
)s
 
select distinct s.SewingLineID,s.FactoryID,[OrderStyle] = o.StyleID, [MockupStyle] = mo.StyleID,s.OutputDate
into #tmp_s1
from Production.dbo.SewingOutput s WITH (NOLOCK)
inner join Production.dbo.SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
left join Production.dbo.Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
left join Production.dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId  
inner join (select [maxOutputDate] = max(OutputDate),[minOutputDate] = dateadd(day,-90, min(OutputDate)) from #tmpSewingGroup) t on s.OutputDate between t.minOutputDate and t.maxOutputDate

select t.*
	,[LastShift] = IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift)
	,[FtyType] = f.Type
	,[FtyCountry] = f.CountryID
	,[CumulateDate] = (select cumulate from Production.dbo.getSewingOutputCumulateOfDays(IIF(t.Category <> 'M',OrderStyle,MockupStyle),t.SewingLineID,t.OutputDate,t.FactoryID))
into #tmp1stFilter
from #tmpSewingGroup t
left join Production.dbo.Factory f on t.FactoryID = f.ID
where t.OrderCategory in ('B','S')-----Artwork
 
-----by orderid & all ArtworkTypeID

select * INTO #Final from(
	select distinct
		 MDivisionID,t.FactoryID
		,t.ComboType
		,FtyType = iif(FtyType='B','Bulk',iif(FtyType='S','Sample',FtyType))
		,FtyCountry
        ,t.OutputDate
        ,t.SewingLineID
		,Shift =    CASE    WHEN t.LastShift='D' then 'Day'
                            WHEN t.LastShift='N' then 'Night'
                            WHEN t.LastShift='O' then 'Subcon-Out'
                            WHEN t.LastShift='I' and SubconInSisterFty = 1 then 'Subcon-In(Sister)'
                            else 'Subcon-In(Non Sister)' end
		,t.SubconOutFty
        ,t.SubConOutContractNumber
        ,t.Team
        ,t.OrderId
		--,t.Ukey
        ,CustPONo
        ,t.BuyerDelivery
        ,t.SciDelivery
        ,t.OrderQty
		,Brand = IIF(t.Category='M',MockupBrandID,OrderBrandID)
		,Category = IIF(t.OrderCategory='M','Mockup',IIF(LocalOrder = 1,'Local Order',IIF(t.OrderCategory='B','Bulk',IIF(t.OrderCategory='S','Sample',IIF(t.OrderCategory='G','Garment','')))))
		,Program = IIF(t.Category='M',MockupProgram,OrderProgram)
		,OrderType
        ,IsDevSample
		,CPURate = IIF(t.Category='M',MockupCPUFactor,OrderCPUFactor)
		,Style = IIF(t.Category='M',MockupStyle,OrderStyle)
		,Season = IIF(t.Category='M',MockupSeason,OrderSeason)
		,CDNo = IIF(t.Category='M',MockupCDCodeID,OrderCdCodeID)+'-'+t.ComboType
		,ActManPower = ActManPower
		,WorkHour
		,ManHour = ROUND(ActManPower*WorkHour,2)
		,TargetCPU = ROUND(ROUND(ActManPower*WorkHour,2)*3600/StdTMS,2)
		,TMS = IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*StdTMS
		,CPUPrice = IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)
		,TargetQty = IIF(IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)>0,ROUND(ROUND(ActManPower*WorkHour,2)*3600/StdTMS,2)/IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate),0)
		,t.QAQty
		,TotalCPU = ROUND(IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty,3)
		,CPUSewer = IIF(ROUND(ActManPower*WorkHour,2)>0,(IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/ROUND(ActManPower*WorkHour,2),0)
		,EFF = ROUND(IIF(ROUND(ActManPower*WorkHour,2)>0,((IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/(ROUND(ActManPower*WorkHour,2)*3600/StdTMS))*100,0),1)
		,RFT = IIF(InspectQty>0,ROUND((InspectQty-RejectQty)/InspectQty*100,2),0)
		,CumulateDate
		,DateRange = IIF(CumulateDate>=10,'>=10',CONVERT(VARCHAR,CumulateDate))
		,InlineQty,Diff = t.QAQty-InlineQty
		,rate
        ,isnull(t.SewingReasonDesc,'')SewingReasonDesc
		 
    from #tmp1stFilter t )a
order by MDivisionID,FactoryID,OutputDate,SewingLineID,Shift,Team,OrderId

drop table #tmpSewingDetail,#tmp1stFilter,#tmpSewingGroup,#tmp_s1


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
		  );

delete t
from P_SewingDailyOutput t 
where t.OutputDate between @SDate and  @EDate
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

End

GO



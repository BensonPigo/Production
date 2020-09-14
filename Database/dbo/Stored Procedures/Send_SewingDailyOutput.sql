

-- =============================================
-- Create date: 2019/07/01
-- Description:	Sewing.P01 Send按鈕執行後將Sewing.R01、R04 report資料給Planning team、Team 3。
-- =============================================
CREATE PROCEDURE [dbo].[Send_SewingDailyOutput] 
	@Factory varchar(13),
	@MaxOutputDate date
AS
BEGIN
	--根據條件撈基本資料
select s.id,s.OutputDate,s.Category,s.Shift,s.SewingLineID,s.Team,s.MDivisionID,s.FactoryID
	,sd.OrderId,sd.ComboType,ActManPower = IIF(sd.QAQty=0, s.Manpower, s.Manpower * sd.QAQty),sd.WorkHour	
	, [ori_QAQty] = sd.QAQty
	, [ori_InlineQty] = sd.InlineQty
	,o.LocalOrder,o.CustPONo,OrderCategory = isnull(o.Category,''),OrderType = isnull(o.OrderTypeID,''), CASE WHEN ot.IsDevSample =1 THEN 'Y' ELSE 'N' END AS IsDevSample
	,OrderBrandID = case 
		when o.BrandID != 'SUBCON-I' then o.BrandID
		when Order2.BrandID is not null then Order2.BrandID
		when StyleBrand.BrandID is not null then StyleBrand.BrandID
		else o.BrandID end    
    ,OrderCdCodeID = isnull(o.CdCodeID,'')
	,OrderProgram = isnull(o.ProgramID,'')  ,OrderCPU = isnull(o.CPU,0) ,OrderCPUFactor = isnull(o.CPUFactor,0) ,OrderStyle = isnull(o.StyleID,'') ,OrderSeason = isnull(o.SeasonID,'')
	,MockupBrandID= isnull(mo.BrandID,'')   ,MockupCDCodeID= isnull(mo.MockupID,'')
	,MockupProgram= isnull(mo.ProgramID,'') ,MockupCPU= isnull(mo.Cpu,0),MockupCPUFactor= isnull(mo.CPUFactor,0),MockupStyle= isnull(mo.StyleID,''),MockupSeason= isnull(mo.SeasonID,'')	
    ,Rate = isnull([dbo].[GetOrderLocation_Rate](o.id,sd.ComboType),100)/100,System.StdTMS
    ,BuyerDelivery = format(o.BuyerDelivery,'yyyy/MM/dd')
    ,OrderQty = o.Qty
    ,s.SubconOutFty
    ,s.SubConOutContractNumber
    ,o.SubconInSisterFty
    ,[SewingReasonDesc]=sr.SewingReasonDesc
	,sd.Remark
    ,o.SciDelivery
into #tmpSewingDetail
from System WITH (NOLOCK),SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join OrderType ot WITH (NOLOCK) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
left join Factory f WITH (NOLOCK) on s.FactoryID = f.ID
outer apply
(
	select [SewingReasonDesc]=stuff((
		select concat(',',sr.ID+'-'+sr.Description)
		from SewingReason sr
		inner join SewingOutput_Detail sd2 WITH (NOLOCK) on sd2.SewingReasonID=sr.ID
		where sr.Type='SO' and sd2.id = s.id
		for xml path('')
	),1,1,'')
)sr
outer apply( select BrandID from orders o1 where o.CustPONo=o1.id )Order2
outer apply( select top 1 BrandID from Style where id=o.StyleID and SeasonID=o.SeasonID and BrandID!='SUBCON-I' )StyleBrand
where 1=1  and s.OutputDate = @MaxOutputDate
and s.FactoryID=@Factory
and f.Type != 'S'
--By Sewing單號 & SewingDetail的Orderid,ComboType 作加總 ActManPower,WorkHour,QAQty,InlineQty
select distinct OutputDate,Category,Shift,SewingLineID,Team,FactoryID,MDivisionID,OrderId,ComboType
	,ActManPower = Sum(ActManPower)over(partition by id,OrderId,ComboType),WorkHour = sum(Round(WorkHour,3))over(partition by id,OrderId,ComboType)
	,QAQty = sum(QAQty)over(partition by id,OrderId,ComboType),InlineQty = sum(InlineQty)over(partition by id,OrderId,ComboType)
	,LocalOrder,CustPONo,OrderCategory,OrderType,IsDevSample
	,OrderBrandID ,OrderCdCodeID ,OrderProgram ,OrderCPU ,OrderCPUFactor ,OrderStyle ,OrderSeason
	,MockupBrandID,MockupCDCodeID,MockupProgram,MockupCPU,MockupCPUFactor,MockupStyle,MockupSeason
	,Rate,StdTMS
	,ori_QAQty
	,ori_InlineQty
    ,BuyerDelivery
	,SciDelivery
    ,OrderQty
    ,SubconOutFty
    ,SubConOutContractNumber
    ,SubconInSisterFty
    ,SewingReasonDesc
    ,Remark
into #tmpSewingGroup
from #tmpSewingDetail

select t.*,IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) as LastShift,
f.Type as FtyType,f.CountryID as FtyCountry
,CumulateDate = (select cumulate from dbo.getSewingOutputCumulateOfDays(IIF(t.Category <> 'M',OrderStyle,MockupStyle),t.SewingLineID,t.OutputDate,t.FactoryID))
into #tmp1stFilter
from #tmpSewingGroup t
left join Factory f on t.FactoryID = f.ID
where 1=1 and (t.OrderCategory = 'B' or t.OrderCategory = 'S')-----Artwork
 
-----by orderid & all ArtworkTypeID
select * from(
	select distinct
		 MDivisionID
		,t.FactoryID
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
		,ActManPower = IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)
		,WorkHour
		,ManHour = ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)
		,TargetCPU = ROUND(ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)*3600/StdTMS,2)
		,TMS = IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*StdTMS
		,CPUPrice = IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)
		,TargetQty = IIF(IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)>0,ROUND(ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)*3600/StdTMS,2)/IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate),0)
		,t.QAQty
		,TotalCPU = ROUND(IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty,3)
		,CPUSewer = IIF(ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)>0,(IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2),0)
		,EFF = ROUND(IIF(ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)>0,((IIF(t.Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/(ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)*3600/StdTMS))*100,0),1)
	    ,RFT = IIF(ori_InlineQty = 0, 0, ROUND(ori_QAQty* 1.0 / ori_InlineQty * 1.0 * 100 ,2))
		,CumulateDate
		,DateRange = IIF(CumulateDate>=10,'>=10',CONVERT(VARCHAR,CumulateDate))
		,InlineQty,Diff = t.QAQty-InlineQty
		,rate
        ,t.Remark
        ,t.SewingReasonDesc
		 
    from #tmp1stFilter t )a
order by MDivisionID,FactoryID,OutputDate,SewingLineID,Shift,Team,OrderId

drop table #tmpSewingDetail,#tmp1stFilter,#tmpSewingGroup
 


END
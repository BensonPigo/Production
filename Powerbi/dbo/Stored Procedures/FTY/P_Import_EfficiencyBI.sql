-- =============================================
-- Create date: 2020/03/20
-- Description:	Data Query Logic by PMS.Sewing R04, Import Data to P_SewingDailyOutput
-- =============================================
USE POWERBIReportData
GO

CREATE PROCEDURE [dbo].[P_Import_EfficiencyBI]
	@StartDate date,
	@EndDate date
AS

BEGIN
SET NOCOUNT ON;

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
	,[Article] = isnull(sdd.Article, sd.Article)
	,[SizeCode] = isnull(sdd.SizeCode, '')
	,sd.ComboType
	,[ActManPower] = s.Manpower
	,[WorkHour] = iif(isnull(sd.QAQty, 0) = 0, sd.WorkHour, cast(sd.WorkHour * (sdd.QAQty * 1.0 / sd.QAQty) as numeric(6, 4)))
	,[QAQty] = isnull(sdd.QAQty, 0)
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
	, [ori_QAQty] = isnull(sdd.QAQty, 0)
	, [ori_InlineQty] = sd.InlineQty
    ,BuyerDelivery = format(o.BuyerDelivery,'yyyy/MM/dd')
    ,OrderQty = o.Qty
    ,s.SubconOutFty
    ,s.SubConOutContractNumber
    ,o.SubconInSisterFty
    ,[SewingReasonDesc]=cast('' as nvarchar(1000))
    ,o.SciDelivery
	,[LockStatus] = CASE WHEN s.Status = 'Locked' THEN 'Monthly Lock' 
						 WHEN s.Status = 'Sent' THEN 'Daily Lock' 
						 ELSE '' END
	,[Cancel] = iif(o.Junk = 1, 'Y', '')
	,[Remark] = cast('' as varchar(max))
	,[SPFactory] = o.FactoryID
	,[NonRevenue] = iif(o.NonRevenue = 1, 'Y', 'N')
	,[InlineCategoryID] =  InlineCategoryID.val
	,[Inline_Category] = cast('' as nvarchar(65))
	,[Low_output_Reason] = cast('' as nvarchar(65))
	,[New_Style_Repeat_style] = cast('' as varchar(20))
	,o.StyleUkey
	,ArtworkType=cast('' as varchar(100))
	,s.SewingReasonIDForTypeIC
	,s.SewingReasonIDForTypeLO
into #tmpSewingDetail
from Production.dbo.System WITH (NOLOCK),Production.dbo.SewingOutput s WITH (NOLOCK) 
inner join Production.dbo.SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Production.dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sd.UKey= sdd.SewingOutput_DetailUKey
left join Production.dbo.Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join Production.dbo.Factory f WITH (NOLOCK) on o.FactoryID = f.id
left join Production.dbo.OrderType ot WITH (NOLOCK) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
left join Production.dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
outer apply( select BrandID from Production.dbo.orders o1 where o.CustPONo = o1.id) Order2
outer apply( select top 1 BrandID from Production.dbo.Style where id = o.StyleID and SeasonID = o.SeasonID and BrandID != 'SUBCON-I') StyleBrand
OUTER APPLY
(
	select val = 
	CASE  WHEN o.Category = 'S' THEN '00005'
		  ELSE 
            CASE 
                WHEN ContinuousDaysCalc.ContinuousDays > 29 THEN '00004'
                WHEN ContinuousDaysCalc.ContinuousDays > 14 THEN '00003'
                WHEN ContinuousDaysCalc.ContinuousDays > 3 THEN '00002'
                ELSE '00001'
            END
	END
	from Production.dbo.SewingReason sr
	CROSS APPLY (
    SELECT 
        ContinuousDays = Production.dbo.GetCheckContinusProduceDays(o.StyleUkey, s.SewingLineID, o.FactoryID, s.Team, OutputDate)
	) ContinuousDaysCalc
	where sr.ID = s.SewingReasonIDForTypeIC and sr.Type='IC'
	
)InlineCategoryID
where 1=1 
--排除non sister的資料o.LocalOrder = 1 and o.SubconInSisterFty = 0
and((o.LocalOrder <> 1 and o.SubconInType not in (1, 2)) or (o.LocalOrder = 1 and o.SubconInType <> 0))
and (s.OutputDate between @SDate and  @EDate
	OR s.OutputDate in (Select OutputDate From Production.dbo.SewingOutput s2 with(nolock) where s2.EditDate >= @StartDate and s2.EditDate < (DateAdd(day, 1,@EndDate))))
and f.Type != 'S'

update s
set [SewingReasonDesc]=isnull(sr.SewingReasonDesc,''),
	[Remark] = isnull(ssd.SewingOutputRemark,''),
	[Inline_Category] = iif(s.SewingReasonIDForTypeIC = '00005', (select CONCAT(s.InlineCategoryID, '-' + SR.Description) from Production.dbo.SewingReason sr where sr.ID = s.InlineCategoryID and sr.Type='IC') , isnull(srICReason.Inline_Category, '')),
	[Low_output_Reason]=isnull(srLOReason.Low_output_Reason, ''),
	[ArtworkType]=isnull(apd.ArtworkType, '')
from #tmpSewingDetail s
outer apply
(
	select [SewingReasonDesc]=stuff((
		select concat('','',sr.ID+'-'+sr.Description)
		from Production.dbo.SewingReason sr
		inner join Production.dbo.SewingOutput_Detail sd2 WITH (NOLOCK) on sd2.SewingReasonID=sr.ID
		where sr.Type='SO' 
		and sd2.id = s.id
		and sd2.OrderId = s.OrderId
		for xml path('')
	),1,1,'')
)sr
outer apply
(
	select [SewingOutputRemark]=stuff((
		select concat(',',ssd.Remark)
		from Production.dbo.SewingOutput_Detail ssd WITH (NOLOCK) 
		where ssd.ID = s.ID
		and ssd.OrderId = s.OrderId
		and isnull(ssd.Remark ,'') <> ''
		for xml path('')
	),1,1,'')
)ssd
outer apply
(
	select Inline_Category=CONCAT(s.SewingReasonIDForTypeIC, '-' + SR.Description)
	from Production.dbo.SewingReason sr
	where sr.ID = s.SewingReasonIDForTypeIC
	and sr.Type='IC'
) srICReason
outer apply
(
	select Low_output_Reason=CONCAT(s.SewingReasonIDForTypeLO, '-' + SR.Description)
	from Production.dbo.SewingReason sr
	where sr.ID = s.SewingReasonIDForTypeLO and
	sr.Type='LO'
) srLOReason
outer apply
(
	select ArtworkType=stuff((
		select concat(',','',ap.ArtworkTypeID)
		from (
			select distinct ap.ArtworkTypeID
			from Production.dbo.ArtworkAP_Detail apd WITH (NOLOCK)
			inner join Production.dbo.ArtworkAP ap WITH (NOLOCK) on apd.ID=ap.Id
			where s.OrderID = apd.OrderID
		) ap
		for xml path('')
	),1,1,'')
)apd

SELECT	FactoryID,
		OutputDate,
		SewinglineID,
		Team,
		StyleUkey,
		[NewStyleRepeatStyle] = Production.dbo.IsRepeatStyleBySewingOutput(FactoryID, OutputDate, SewinglineID, Team, StyleUkey)
INTO	#tmpNewStyleRepeatStyle
from (	select distinct FactoryID, OutputDate, SewinglineID, Team, StyleUkey
		from #tmpSewingDetail ) a

update t set t.[New_Style_Repeat_style] = tp.NewStyleRepeatStyle
from #tmpSewingDetail t
inner join #tmpNewStyleRepeatStyle tp on	tp.FactoryID = t.FactoryID and
											tp.OutputDate = t.OutputDate and 
											tp.SewinglineID = t.SewinglineID and 
											tp.Team = t.Team and
											tp.StyleUkey = t.StyleUkey

select distinct ID
	,OutputDate
	,Category
	,Shift
	,SewingLineID
	,Team
	,FactoryID
	,MDivisionID
	,OrderId
	,Article
	,SizeCode
	,ComboType
	,[ActManPower] = s.Manpower
	,WorkHour = sum(Round(WorkHour,3))over(partition by id,OrderId,Article,SizeCode,ComboType)
	,QAQty = sum(QAQty)over(partition by id,OrderId,Article,SizeCode,ComboType)
	,[InlineQty] = sum(InlineQty)over(partition by id,OrderId,Article,SizeCode,ComboType)
	,LocalOrder,CustPONo,OrderCategory,OrderType,IsDevSample
	,OrderBrandID ,OrderCdCodeID ,OrderProgram ,OrderCPU ,OrderCPUFactor ,OrderStyle ,OrderSeason
	,MockupBrandID,MockupCDCodeID,MockupProgram,MockupCPU,MockupCPUFactor,MockupStyle,MockupSeason
	,Rate,StdTMS
	,ori_QAQty = sum(ori_QAQty)over(partition by id,OrderId,Article,SizeCode,ComboType)
	,ori_InlineQty = sum(ori_InlineQty)over(partition by id,OrderId,Article,SizeCode,ComboType)
    ,BuyerDelivery
    ,SciDelivery
    ,OrderQty
    ,SubconOutFty
    ,SubConOutContractNumber
    ,SubconInSisterFty
    ,SewingReasonDesc
	,sty.CDCodeNew
	,[ProductType] = sty.ProductType
	,[FabricType] = sty.FabricType
	,[Lining] = sty.Lining
	,[Gender] = sty.Gender
	,[Construction] = sty.Construction
	,t.LockStatus
	,t.[Cancel]
	,t.[Remark]
	,t.[SPFactory]
	,t.[NonRevenue]
	,t.[Inline_Category]
	,t.[Low_output_Reason]
	,t.[New_Style_Repeat_style]
	,t.ArtworkType
into #tmpSewingGroup
from #tmpSewingDetail t
outer apply(
	select s.Manpower 
	from Production.dbo.SewingOutput s
	where s.ID = t.ID
)s
Outer apply (
	SELECT ProductType = r2.Name
		, FabricType = r1.Name
		, Lining
		, Gender
		, Construction = d1.Name
		, s.CDCodeNew
	FROM Production.dbo.Style s WITH(NOLOCK)
	left join Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	where s.ID = t.OrderStyle 
	and s.SeasonID = t.OrderSeason 
	and s.BrandID = t.OrderBrandID
)sty

select [MaxOutputDate] = Max(OutputDate), [MinOutputDate] = MIN(OutputDate), MockupStyle, OrderStyle, SewingLineID, FactoryID 
into #tmpOutputDate
from(
	select distinct OutputDate, MockupStyle, OrderStyle, SewingLineID, FactoryID 
	from #tmpSewingGroup
) a
group by MockupStyle, OrderStyle, SewingLineID, FactoryID

select distinct t.FactoryID, t.SewingLineID ,t.OrderStyle, t.MockupStyle, s.OutputDate
into #tmpSewingOutput
from #tmpOutputDate t
inner join Production.dbo.SewingOutput s WITH (NOLOCK) on s.SewingLineID = t.SewingLineID 
											and s.FactoryID = t.FactoryID 
											and s.OutputDate between dateadd(day,-240, t.MinOutputDate) and t.MaxOutputDate
where   exists(	select 1 from Production.dbo.SewingOutput_Detail sd WITH (NOLOCK)
				left join Production.dbo.Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
				left join Production.dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
				where s.ID = sd.ID and (o.StyleID = t.OrderStyle or mo.StyleID = t.MockupStyle))
order by  FactoryID, t.SewingLineID ,t.OrderStyle, t.MockupStyle, s.OutputDate

select w.FactoryID, w.SewingLineID ,t.OrderStyle, t.MockupStyle, w.Date
into #tmpWorkHour
from Production.dbo.WorkHour w WITH (NOLOCK)
left join #tmpOutputDate t on t.SewingLineID = w.SewingLineID and t.FactoryID = w.FactoryID and w.Date between t.MinOutputDate and t.MaxOutputDate
where w.Holiday=0 and isnull(w.Hours,0) != 0 and w.Date >= (select dateadd(day,-240, min(MinOutputDate)) from #tmpOutputDate) and  w.Date <= (select max(MaxOutputDate) from #tmpOutputDate)
order by  FactoryID, t.SewingLineID ,t.OrderStyle, t.MockupStyle, w.Date

select t.*
	,[LastShift] = IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift)
	,[FtyType] = f.Type
	,[FtyCountry] = f.CountryID
	,[CumulateDate] = CumulateDate.val
	,[RFT] = IIF(isnull(A.InspectQty,0) = 0, 0, round(((A.InspectQty-A.RejectQty) / A.InspectQty)*100,2))
into #tmp1stFilter
from #tmpSewingGroup t
left join Production.dbo.Factory f on t.FactoryID = f.ID
outer apply (	select val = IIF(Count(1)=0, 1, Count(1))
				from #tmpSewingOutput s
				where	s.FactoryID = t.FactoryID and
						s.MockupStyle = t.MockupStyle and
						s.OrderStyle = t.OrderStyle and
						s.SewingLineID = t.SewingLineID and
						s.OutputDate <= t.OutputDate and
						s.OutputDate >(
										select case when max(iif(s1.OutputDate is null, w.Date, null)) is not null then max(iif(s1.OutputDate is null, w.Date, null))
													when min(w.Date) is not null then dateadd(day, -1, min(w.Date))
													else t.OutputDate end
										from #tmpWorkHour w 
										left join #tmpSewingOutput s1 on s1.OutputDate = w.Date and
																		 s1.FactoryID = w.FactoryID and
																		 s1.MockupStyle = t.MockupStyle and
																		 s1.OrderStyle = t.OrderStyle and
																		 s1.SewingLineID = w.SewingLineID
										where	w.FactoryID = t.FactoryID and
												isnull(w.MockupStyle, t.MockupStyle) = t.MockupStyle and
												isnull(w.OrderStyle, t.OrderStyle) = t.OrderStyle and
												w.SewingLineID = t.SewingLineID and
												w.Date <= t.OutputDate
									)
) CumulateDate
left join Production.dbo.RFT A with (nolock) on A.OrderID=t.OrderId
								 and A.CDate=t.OutputDate
								 and A.SewinglineID=t.SewinglineID
								 and A.FactoryID=t.FactoryID
								 and A.Shift=t.Shift
								 and A.Team=t.Team 
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
		,[LastShift]
		,t.SubconOutFty
        ,t.SubConOutContractNumber
        ,t.Team
        ,t.OrderId
		,t.Article
		,t.SizeCode
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
		,RFT
		,CumulateDate
		,DateRange = IIF(CumulateDate>=10,'>=10',CONVERT(VARCHAR,CumulateDate))
		,InlineQty,Diff = t.QAQty-InlineQty
		,rate
        ,isnull(t.SewingReasonDesc,'')SewingReasonDesc
		,t.CDCodeNew
		,t.ProductType
		,t.FabricType
		,t.Lining
		,t.Gender
		,t.Construction
		,t.LockStatus
		,t.[Cancel]
		,t.[Remark]
		,t.[SPFactory]
		,t.[NonRevenue]
		,t.[Inline_Category]
		,t.[Low_output_Reason]
		,t.[New_Style_Repeat_style]
		,[FCategory] = t.Category
		,t.ArtworkType
    from #tmp1stFilter t )a
order by MDivisionID,FactoryID,OutputDate,SewingLineID,Shift,Team,OrderId,Article,SizeCode

select ID,Seq,ArtworkUnit,ProductionUnit
into #AT
from Production.dbo.ArtworkType WITH (NOLOCK)
where Classify in ('I','A','P') and IsTtlTMS = 0 and Junk = 0

select ID,Seq
	,ArtworkType_Unit = concat(ID,iif(Unit='QTY','(Price)',iif(Unit = '','','('+Unit+')'))),Unit
	,ArtworkType_CPU = iif(Unit = 'TMS',concat(ID,'(CPU)'),'')
into #atall
from(
	Select ID,Seq,Unit = ArtworkUnit from #AT where ArtworkUnit !='' AND ProductionUnit !=''
	UNION
	Select ID,Seq,ProductionUnit from #AT where ArtworkUnit !='' AND ProductionUnit !=''
	UNION
	Select ID,Seq,ArtworkUnit from #AT where ArtworkUnit !='' AND ProductionUnit =''
	UNION
	Select ID,Seq,ProductionUnit from #AT where ArtworkUnit ='' AND ProductionUnit !=''
	UNION
	Select ID,Seq,'' from #AT where ArtworkUnit ='' AND ProductionUnit =''
)a

select *
into #atall2
from(
	select a.ID,a.Seq,c=1,a.ArtworkType_Unit,a.Unit from #atall a
	UNION
	select a.ID,a.Seq,2,a.ArtworkType_CPU,iif(a.ArtworkType_CPU='','','CPU')from #atall a
	where a.ArtworkType_CPU !=''
)b

--準備台北資料(須排除這些)
select ps.ID
into #TPEtmp
from Production.dbo.PO_Supp ps WITH (NOLOCK)
inner join Production.dbo.PO_Supp_Detail psd WITH (NOLOCK) on ps.ID=psd.id and ps.SEQ1=psd.Seq1
inner join Production.dbo.Fabric fb WITH (NOLOCK) on psd.SCIRefno = fb.SCIRefno 
inner join Production.dbo.MtlType ml WITH (NOLOCK) on ml.id = fb.MtlTypeID
where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
and ml.isThread=1 
and ps.SuppID <> 'FTY' and ps.Seq1 not Like '5%'

-----orderid & ArtworkTypeID & Seq
select distinct ot.ID,ot.ArtworkTypeID,ot.Seq,ot.Qty,ot.Price,ot.TMS,t.QAQty,t.FactoryID,t.Team,t.OutputDate,t.SewingLineID,t.SubConOutContractNumber,
                IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) as LastShift,t.Category,t.ComboType,t.SubconOutFty,t.Article,t.SizeCode
into #idat
from #tmpSewingGroup t
inner join Production.dbo.Order_TmsCost ot WITH (NOLOCK) on ot.id = t.OrderId
inner join Production.dbo.orders o with(nolock) on o.ID = t.OrderId
inner join #AT A on A.ID = ot.ArtworkTypeID
where  ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')

			  drop table #tmpSewingDetail,#tmp1stFilter,#tmpSewingGroup--,#tmp_s1

declare @columnsName nvarchar(max) = stuff((select concat(',[',ArtworkType_Unit,']') from #atall2 for xml path('')),1,1,'')
declare @NameZ nvarchar(max) = (select concat(',[',ArtworkType_Unit,']=isnull([',ArtworkType_Unit,'],0)')from #atall2 for xml path(''))
declare @NameFinal nvarchar(max) = replace((select concat('',col) 
from 
(select [col] = Replace(Replace(Replace(Replace(Replace(Replace(concat(iif(ArtworkType_Unit = '', '', ',['+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',['+ArtworkType_CPU+']'), iif(ArtworkType_Unit = '', '', ',[TTL_'+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',[TTL_'+ArtworkType_CPU+']')), ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
from #atall where id in ('AT (HAND)', 'AT (MACHINE)')
union all
select [col] = ',[TTL_AT_CPU] = iif([TTL_AT_HAND_CPU] > [TTL_AT_MACHINE_CPU], [TTL_AT_HAND_CPU], [TTL_AT_MACHINE_CPU])'
union all
select [col] = Replace(Replace(Replace(Replace(Replace(Replace(concat(iif(ArtworkType_Unit = '', '', ',['+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',['+ArtworkType_CPU+']'), iif(ArtworkType_Unit = '', '', ',[TTL_'+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',[TTL_'+ArtworkType_CPU+']')), ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
from #atall where id not in ('AT (HAND)', 'AT (MACHINE)')
) a for xml path(N'')), '&gt;', '>')

declare @FinalColumns nvarchar(max) = replace((select concat('',col) 
from 
(select [col] = Replace(Replace(Replace(Replace(Replace(Replace(concat(iif(ArtworkType_Unit = '', '', ',['+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',['+ArtworkType_CPU+']'), iif(ArtworkType_Unit = '', '', ',[TTL_'+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',[TTL_'+ArtworkType_CPU+']')), ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
from #atall where id in ('AT (HAND)', 'AT (MACHINE)')
union all
select [col] = ',[TTL_AT_CPU]'
union all
select [col] = Replace(Replace(Replace(Replace(Replace(Replace(concat(iif(ArtworkType_Unit = '', '', ',['+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',['+ArtworkType_CPU+']'), iif(ArtworkType_Unit = '', '', ',[TTL_'+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',[TTL_'+ArtworkType_CPU+']')), ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
from #atall where id not in ('AT (HAND)', 'AT (MACHINE)')
) a for xml path(N'')), '&gt;', '>')

declare @InsertColumns nvarchar(max) = replace((select concat('',col) 
from 
(
Select col = ',s.'+Data From dbo.SplitString(@FinalColumns, ',') where isnull(Data,'') != ''
) a for xml path(N'')), '&gt;', '>')

declare @UpdateColumns nvarchar(max) = replace((select concat('',col) 
from 
(
Select col = ',t.'+Data + ' = s.' + Data From dbo.SplitString(@FinalColumns, ',') where isnull(Data,'') != ''
) a for xml path(N'')), '&gt;', '>')

declare @TTLZ nvarchar(max) = 
(select concat(',['
,Replace(Replace(Replace(Replace(Replace(Replace(ArtworkType_Unit, ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
,']=Cast(Round(sum(isnull(Rate*[',ArtworkType_Unit,'],0)) over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber,t.Article,t.SizeCode),4) as Numeric(15,4))'
,iif(ArtworkType_CPU = '', '', concat(',['
,Replace(Replace(Replace(Replace(Replace(Replace(ArtworkType_CPU, ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
,']=Cast(Round(sum(isnull(Rate*[',ArtworkType_CPU,'],0)) over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber,t.Article,t.SizeCode),4) as Numeric(15,4))'))
,',[TTL_'
,Replace(Replace(Replace(Replace(Replace(Replace(ArtworkType_Unit, ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
,']=Cast(Round(sum(o.QAQty*Rate*[',ArtworkType_Unit,'])over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber,t.Article,t.SizeCode),',iif(Unit='QTY','4','3'),') as Numeric(15,4))'
,iif(ArtworkType_CPU = '', '', concat(',[TTL_'
,Replace(Replace(Replace(Replace(Replace(Replace(ArtworkType_CPU, ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
,']=Cast(Round(sum(o.QAQty*Rate*[',ArtworkType_CPU,'])over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber,t.Article,t.SizeCode),',iif(Unit='QTY','4','3'),') as Numeric(15,4))'))
)from #atall for xml path(''))

declare @lastSql nvarchar(max) = ''
set @lastSql = @lastSql + 'select orderid,SubconOutFty,SubConOutContractNumber,FactoryID,Team,OutputDate,SewingLineID,LastShift,Category,ComboType,qaqty,Article,SizeCode '+@NameZ+N'
into #oid_at
from
(
	select orderid = i.ID,a.ArtworkType_Unit,i.qaqty,ptq=iif(a.Unit=''QTY'',i.Price,iif(a.Unit=''TMS'',i.TMS,iif(a.Unit=''CPU'',i.Price,i.Qty))),
           i.FactoryID,i.Team,i.OutputDate,i.SewingLineID,i.LastShift,i.Category,i.ComboType,i.SubconOutFty,i.SubConOutContractNumber,i.Article,i.SizeCode
	from #atall2 a left join #idat i on i.ArtworkTypeID = a.ID and i.Seq = a.Seq
)a
PIVOT(min(ptq) for ArtworkType_Unit in('+@columnsName+N'))as pt
where orderid is not null
'
set @lastSql = @lastSql +'
Select 
	MDivisionID,FactoryID,ComboType,FtyType,FtyCountry,OutputDate,SewingLineID,Shift
	,SubconOutFty,SubConOutContractNumber,Team,OrderID,Article,SizeCode,CustPONo,BuyerDelivery
	,OrderQty,Brand,Category,Program,OrderType,IsDevSample,CPURate,Style,Season,CDNo,ActManPower
	,WorkHour,ManHour,TargetCPU,TMS,CPUPrice,TargetQty,QAQTY,TotalCPU,CPUSewer,EFF,RFT,CumulateDate
	,DateRange,InlineQty,Diff,Rate,SewingReasonDesc,SciDelivery,CDCodeNew,ProductType,FabricType
	,Lining,Gender,Construction,LockStatus, Cancel, Remark, SPFactory, NonRevenue, Inline_Category
	,Low_output_Reason, New_Style_Repeat_Style,ArtworkType'
	set @lastSql = @lastSql + ' ' + @NameFinal + N' '

		set @lastSql = @lastSql + '

into #FinalDt 
from (
	Select t.MDivisionID,t.FactoryID,t.ComboType,t.FtyType,t.FtyCountry,t.OutputDate,t.SewingLineID,t.Shift
	,t.SubconOutFty,t.SubConOutContractNumber,t.Team,t.OrderID,t.Article,t.SizeCode,t.CustPONo,t.BuyerDelivery
	,t.OrderQty,t.Brand,t.Category,t.Program,t.OrderType,t.IsDevSample,t.CPURate,t.Style,t.Season,t.CDNo,t.ActManPower
	,t.WorkHour,t.ManHour,t.TargetCPU,t.TMS,t.CPUPrice,t.TargetQty,t.QAQTY,t.TotalCPU,t.CPUSewer,t.EFF,t.RFT,t.CumulateDate
	,t.DateRange,t.InlineQty,t.Diff,t.Rate,t.SewingReasonDesc,t.SciDelivery,t.CDCodeNew,t.ProductType,t.FabricType
	,t.Lining,t.Gender,t.Construction,t.LockStatus, t.Cancel, t.Remark, t.SPFactory, t.NonRevenue, t.Inline_Category
	,t.Low_output_Reason, t.New_Style_Repeat_Style, t.ArtworkType
	'
	set @lastSql = @lastSql + ' ' + @TTLZ + N' '

		set @lastSql = @lastSql + '
From #Final t

'	
			set @lastSql = @lastSql + '
				left join #oid_at o on o.orderid = t.OrderId and 
                           o.FactoryID = t.FactoryID and
                           o.Team = t.Team and
                           o.OutputDate = t.OutputDate and
                           o.SewingLineID = t.SewingLineID and
                           o.LastShift = t.LastShift and
                           o.Category = t.[FCategory] and
                           o.ComboType = t.ComboType and
                           o.SubconOutFty = t.SubconOutFty and
                           o.SubConOutContractNumber = t.SubConOutContractNumber and
						   o.Article = t.Article and
						   o.SizeCode = t.SizeCode
				)a

insert into P_SewingDailyOutput(MDivisionID, FactoryID, ComboType, Category, CountryID, OutputDate, SewingLineID, Shift
	, SubconOutFty, SubConOutContractNumber, Team, OrderID, Article, SizeCode, CustPONo, BuyerDelivery
	, OrderQty, BrandID, OrderCategory, ProgramID, OrderTypeID, DevSample, CPURate, StyleID, Season, CdCodeID, ActualManpower
	, NoOfHours, TotalManhours, TargetCPU, TMS, CPUPrice, TargetQty, TotalOutputQty, TotalCPU, CPUSewerHR, EFF, RFT, CumulateOfDays
	, DateRange, ProdOutput, Diff, Rate, SewingReasonDesc, SciDelivery, CDCodeNew, ProductType, FabricType
	, Lining, Gender, Construction, LockStatus, Cancel, Remark, SPFactory, NonRevenue, Inline_Category
	, Low_output_Reason, New_Style_Repeat_Style,ArtworkType'
	set @lastSql = @lastSql + ' ' + @FinalColumns + N' '

		set @lastSql = @lastSql + ')
select s.MDivisionID,s.FactoryID,s.ComboType,s.FtyType,s.FtyCountry,s.OutputDate,s.SewingLineID,s.Shift
	,s.SubconOutFty,s.SubConOutContractNumber,s.Team,s.OrderID,s.Article,s.SizeCode,s.CustPONo,s.BuyerDelivery
	,s.OrderQty,s.Brand,s.Category,s.Program,s.OrderType,s.IsDevSample,s.CPURate,s.Style,s.Season,s.CDNo,s.ActManPower
	,s.WorkHour,s.ManHour,s.TargetCPU,s.TMS,s.CPUPrice,s.TargetQty,s.QAQTY,s.TotalCPU,s.CPUSewer,s.EFF,s.RFT,s.CumulateDate
	,s.DateRange,s.InlineQty,s.Diff,s.Rate,s.SewingReasonDesc,s.SciDelivery,s.CDCodeNew,s.ProductType,s.FabricType
	,s.Lining,s.Gender,s.Construction,s.LockStatus, s.Cancel, s.Remark, s.SPFactory, s.NonRevenue, s.Inline_Category
	,s.Low_output_Reason, s.New_Style_Repeat_Style, s.ArtworkType'
	set @lastSql = @lastSql + ' ' + @InsertColumns + N' '

		set @lastSql = @lastSql + '
from #FinalDt s
where not exists (select 1 from P_SewingDailyOutput t where t.FactoryID=s.FactoryID  
                                                       AND t.MDivisionID=s.MDivisionID 
                                                       AND t.SewingLineID=s.SewingLineID 
                                                       AND t.Team=s.Team 
                                                       AND t.Shift=s.Shift 
                                                       AND t.OrderId=s.OrderId 
                                                       AND t.Article=s.Article 
                                                       AND t.SizeCode=s.SizeCode 
                                                       AND t.ComboType=s.ComboType  
                                                       AND t.OutputDate = s.OutputDate
                                                       AND t.SubConOutContractNumber = s.SubConOutContractNumber)


update t
	set t.MDivisionID =s.MDivisionID
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
		,t.Article =s.Article
		,t.SizeCode =s.SizeCode
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
		,t.LockStatus = s.LockStatus
		,t.Cancel = s.Cancel
		,t.Remark = s.Remark
		,t.SPFactory = s.SPFactory
		,t.NonRevenue = s.NonRevenue
		,t.Inline_Category = s.Inline_Category
		,t.Low_output_Reason = s.Low_output_Reason
		,t.New_Style_Repeat_Style = s.New_Style_Repeat_Style
		,t.ArtworkType = s.ArtworkType'
		set @lastSql = @lastSql + ' ' + @UpdateColumns + N' '

		set @lastSql = @lastSql + '
from P_SewingDailyOutput t
inner join #FinalDt s on t.FactoryID=s.FactoryID  
				   AND t.MDivisionID=s.MDivisionID 
				   AND t.SewingLineID=s.SewingLineID 
				   AND t.Team=s.Team 
				   AND t.Shift=s.Shift 
				   AND t.OrderId=s.OrderId 
				   AND t.Article=s.Article 
				   AND t.SizeCode=s.SizeCode 
				   AND t.ComboType=s.ComboType  
				   AND t.OutputDate = s.OutputDate
				   AND t.SubConOutContractNumber = s.SubConOutContractNumber


delete t
from P_SewingDailyOutput t 
where t.OutputDate in (select outputDate from #FinalDt)
and exists (select OrderID from #FinalDt f where t.FactoryID=f.FactoryID  AND t.MDivisionID=f.MDivisionID ) 
and not exists (
select OrderID from #FinalDt s 
	where t.FactoryID=s.FactoryID  
	AND t.MDivisionID=s.MDivisionID 
	AND t.SewingLineID=s.SewingLineID 
	AND t.Team=s.Team 
	AND t.Shift=s.Shift 
	AND t.OrderID=s.OrderID 
	AND t.Article=s.Article 
	AND t.SizeCode=s.SizeCode 
	AND t.ComboType=s.ComboType 
	AND t.OutputDate = s.OutputDate);
			
update b
    set b.TransferDate = getdate()
		, b.IS_Trans = 1
from BITableInfo b
where b.id = ''P_SewingDailyOutput''

			'

	EXEC sp_executesql @lastSql
End
CREATE PROCEDURE [dbo].[P_ImportSewingLineSchedule]
	(
	 @ServerName varchar(50)
	)
AS

BEGIN
Declare @Inline nvarchar(20) = format(dateadd(Day,-15,getdate()) ,'yyyy/MM/dd' )
declare @SqlCmd_Combin nvarchar(max) =''
Declare @sql1 nvarchar(max) = N'
select
	s.APSNo ,
	s.MDivisionID,
	s.SewingLineID,
	s.FactoryID,
	[InlineDate] = Cast(s.Inline as date),
	[OfflineDate] = Cast(s.Offline as date),
	s.Inline,
	s.Offline,
    [InlineHour] = DATEDIFF(ss,Cast(s.Inline as date),s.Inline) / 3600.0	  ,
    [OfflineHour] = DATEDIFF(ss,Cast(s.Offline as date),s.Offline) / 3600.0	  ,
	s.OriEff,
    s.SewLineEff,
	LearnCurveID,
	Sewer,
	[AlloQty] = sum(s.AlloQty),
	[HourOutput] = iif(isnull(s.TotalSewingTime,0)=0,0,(s.Sewer * 3600.0 * ScheduleEff.val / 100) / s.TotalSewingTime),
	[OriWorkHour] = iif (s.Sewer = 0 or isnull(s.TotalSewingTime,0)=0, 0, sum(s.AlloQty) / ((s.Sewer * 3600.0 * ScheduleEff.val / 100) / s.TotalSewingTime)),
	[CPU] = cast(o.CPU * o.CPUFactor * isnull(dbo.[P_GetOrderLocation_Rate](s.OrderID,s.ComboType,''['+@ServerName+']''),isnull(dbo.[P_GetStyleLocation_Rate](o.StyleUkey,s.ComboType,''['+@ServerName+']''),100)) / 100 as float),
	s.TotalSewingTime,
	s.OrderID,
	s.LNCSERIALNumber,
    s.ComboType
	into #APSListWorkDay
from ['+@ServerName+'].Production.dbo.SewingSchedule s  WITH (NOLOCK) 
inner join ['+@ServerName+'].Production.dbo.Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join ['+@ServerName+'].Production.dbo.Factory f with (nolock) on f.id = s.FactoryID and Type <> ''S''
left join ['+@ServerName+'].Production.dbo.Country c WITH (NOLOCK) on o.Dest = c.ID
outer apply(select [val] = iif(s.OriEff is null and s.SewLineEff is null,s.MaxEff, isnull(s.OriEff,100) * isnull(s.SewLineEff,100) / 100) ) ScheduleEff
where 1 = 1  and (s.Inline >= '''+@Inline+N''' or (s.Inline <='''+@Inline+N''' and s.Offline >= '''+@Inline+N''')) and s.APSno <> 0
group by	s.APSNo ,
			s.MDivisionID,
			s.SewingLineID,
			s.FactoryID,
			s.Inline,
			s.Offline,
			ScheduleEff.val,
            s.OriEff,
            s.SewLineEff,
			s.LearnCurveID,
			s.Sewer,
			s.TotalSewingTime,
			o.CPU,
			o.CPUFactor,
			s.OrderID,
			s.ComboType,
			o.StyleUkey,
			s.LNCSERIALNumber
select 
	APSNo,
	MDivisionID,
	SewingLineID,
	FactoryID,
	Inline,
	Offline,
	LearnCurveID,
	Sewer,
    OriEff,
    SewLineEff,
	AlloQty = sum(AlloQty)
into #APSList
from #APSListWorkDay
group by APSNo,
		 MDivisionID,
		 SewingLineID,
		 FactoryID,
		 Inline,
		 Offline,
		 LearnCurveID,
		 Sewer,
         OriEff,
         SewLineEff

--取得OrderQty by APSNo
select  aps.APSNo,[OrderQty] =sum(o.Qty) 
into #APSOrderQty
from #APSList aps
inner join ['+@ServerName+'].Production.dbo.SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join ['+@ServerName+'].Production.dbo.Orders o with (nolock) on s.OrderID = o.ID
group by aps.APSNo

--取得Cutting Output by APSNo
select  aps.APSNo,[CuttingOutput] =sum(cw.Qty) 
into #APSCuttingOutput
from #APSList aps
inner join ['+@ServerName+'].Production.dbo.SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join ['+@ServerName+'].Production.dbo.CuttingOutput_WIP cw with (nolock) on s.OrderID = cw.OrderID
group by aps.APSNo

--取得Packing data by APSNo
select  aps.APSNo,[ScannedQty] =sum(pld.ScanQty),[ClogQty] = sum(pld.ShipQty)
into #APSPackingQty
from #APSList aps
inner join ['+@ServerName+'].Production.dbo.SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join ['+@ServerName+'].Production.dbo.PackingList_Detail pld with (nolock) on s.OrderID = pld.OrderID and pld.ReceiveDate is not null
group by aps.APSNo

--取得SewingOutput
select
aps.APSNo,
so.OutputDate,
[SewingOutput] = sum(isnull(sod.QAQty,0))
into #APSSewingOutput
from #APSList aps
inner join ['+@ServerName+'].Production.dbo.SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join ['+@ServerName+'].Production.dbo.SewingOutput_Detail sod with (nolock) on s.OrderID = sod.OrderID and s.ComboType = sod.ComboType
inner join ['+@ServerName+'].Production.dbo.SewingOutput so with (nolock) on so.ID = sod.ID and s.SewingLineID = so.SewingLineID
group by	aps.APSNo,
			so.OutputDate
'

Declare @sql2 nvarchar(max) = N'

--取得Artwork
select  distinct
		o.StyleID
        , at.Abbreviation
        , ot.Qty
        , ot.TMS
        , at.Classify
into #tmpAllArtwork
from ['+@ServerName+'].Production.dbo.Order_TmsCost ot WITH (NOLOCK) 
inner join ['+@ServerName+'].Production.dbo.ArtworkType at WITH (NOLOCK)  on ot.ArtworkTypeID = at.ID
inner join ['+@ServerName+'].Production.dbo.Orders o with (nolock) on ot.ID = o.id
where   (ot.Price > 0 or at.Classify in (''O'',''I'') )
        and (at.Classify in (''S'',''I'') or at.IsSubprocess = 1)
        and (ot.TMS > 0 or ot.Qty > 0)
        and at.Abbreviation !=''''
		and ot.ID in (select OrderID from ['+@ServerName+'].Production.dbo.SewingSchedule where exists(select 1 from #APSList where APSNo = SewingSchedule.APSNo))
select * 
into #tmpArtWork
from (
    select  StyleID
            , Abbreviation+'':''+Convert(varchar,Qty) as Artwork 
    from #tmpAllArtwork 
    where Qty > 0
        
    union all
    select  StyleID
            , Abbreviation+'':''+Convert(varchar,TMS) as Artwork 
    from #tmpAllArtwork 
    where TMS > 0 and Classify in (''O'',''I'')
) a 

select tmpArtWorkID.StyleID
        , Artwork = Stuff((select   CONCAT('','',Artwork) 
						from #tmpArtWork 
						where StyleID = tmpArtWorkID.StyleID 
						order by Artwork for xml path('''')),1,1,'''')   
into #tmpOrderArtwork
from (
	select distinct StyleID
	from #tmpArtWork
) tmpArtWorkID

drop table #tmpAllArtwork,#tmpArtWork

--取得order對應的Articl
select
s.APSNo,
[Colorway] = Rtrim(sd.Article) +''('' + cast(SUM(sd.AlloQty) as varchar) + '')''
into #APSListArticle
from ['+@ServerName+'].Production.dbo.SewingSchedule s with (nolock)
inner join ['+@ServerName+'].Production.dbo.SewingSchedule_Detail sd with (nolock) on s.ID = sd.ID
where exists( select 1 from #APSList where APSNo = s.APSNo)
group by s.APSNo,sd.Article

--取得 Remarks欄位
select
s.APSNo,
[Remarks] = s.OrderID + ''('' + s_other.SewingLineID + '','' + s.ComboType + '','' + CAST(sum(s_other.AlloQty) as varchar) + '')''
into #APSRemarks
from ['+@ServerName+'].Production.dbo.SewingSchedule s with (nolock)
inner join ['+@ServerName+'].Production.dbo.SewingSchedule s_other on s_other.OrderID = s.OrderID and s_other.ComboType = s.ComboType and s_other.APSNo <> s.APSNo
where  exists( select 1 from #APSList where APSNo = s.APSNo)
group by s.APSNo,s.OrderID,s_other.SewingLineID,s.ComboType

--取得每個計劃需要串接起來的欄位，供後續使用
select
APSNo,
o.CustPONo,
[SP] = s.OrderID+''('' + s.ComboType + '')'',
o.CdCodeID,
cd.ProductionFamilyID,
o.StyleID,
o.PFOrder,
o.MTLExport,
o.KPILETA,
o.MTLETA,
[Artwork] = o.StyleID+''(''  +oa.Artwork + '')'',
o.InspDate,
o.Category,
o.SCIDelivery,
o.BuyerDelivery
into #APSColumnGroup
from ['+@ServerName+'].Production.dbo.SewingSchedule s with (nolock)
inner join ['+@ServerName+'].Production.dbo.Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join ['+@ServerName+'].Production.dbo.CDCode cd with (nolock) on o.CdCodeID = cd.ID
left join #tmpOrderArtwork oa on oa.StyleID = o.StyleID
left join ['+@ServerName+'].Production.dbo.Country c WITH (NOLOCK) on o.Dest = c.ID 
where exists( select 1 from #APSList where APSNo = s.APSNo)
'

Declare @sql3 nvarchar(max) = N'
CREATE INDEX IDX_TMP_APSColumnGroup ON #APSColumnGroup (APSNo);
CREATE INDEX IDX_TMP_APSListArticle ON #APSListArticle (APSNo);

--填入資料串連欄位 by APSNo
select
al.APSNo,
al.SewingLineID,
[CustPO] = CustPO.val,
[CustPoCnt] =  iif(LEN(CustPO.val) > 0,(LEN(CustPO.val) - LEN(REPLACE(CustPO.val, '','', ''''))) / LEN('','') + 1,0),  --用,數量計算CustPO數量
[SP] = SP.val,
[SpCnt] = (select count(1) from ['+@ServerName+'].Production.dbo.SewingSchedule where APSNo = al.APSNo),
[Colorway] = Colorway.val,
[ColorwayCnt] = iif(LEN(Colorway.val) > 0,(LEN(Colorway.val) - LEN(REPLACE(Colorway.val, '','', ''''))) / LEN('','') + 1,0),  --用,數量計算Colorway數量
[CDCode] = CDCode.val,
[ProductionFamilyID] = ProductionFamilyID.val,
[Style] = Style.val,
[StyleCnt] = iif(LEN(Style.val) > 0,(LEN(Style.val) - LEN(REPLACE(Style.val, '','', ''''))) / LEN('','') + 1,0),
aoo.OrderQty,
al.AlloQty,
al.LearnCurveID,
al.Inline,
al.Offline,
[PFRemark] = iif(exists(select 1 from #APSColumnGroup where APSNo = al.APSNo and PFOrder = 1),''Y'',''''),
[MTLComplete] = iif(exists(select 1 from #APSColumnGroup where APSNo = al.APSNo and MTLExport = ''''),'''',''Y''),
OrderMax.KPILETA,
OrderMax.MTLETA,
[ArtworkType] = ArtworkType.val,
OrderMax.InspDate,
[Remarks] = Remarks.val,
[CuttingOutput] = isnull(aco.CuttingOutput,0),
[ScannedQty] = isnull(apo.ScannedQty,0),
[ClogQty] = isnull(apo.ClogQty,0),
al.MDivisionID,
al.FactoryID,
al.Sewer,
[Category] = Category.val,
OrderDateInfo.MaxSCIDelivery,
OrderDateInfo.MinSCIDelivery,
OrderDateInfo.MaxBuyerDelivery,
OrderDateInfo.MinBuyerDelivery,
OriEff,
SewLineEff
into #APSMain
from #APSList al
left join #APSCuttingOutput aco on al.APSNo = aco.APSNo
left join #APSOrderQty aoo on al.APSNo = aoo.APSNo
left join #APSPackingQty apo on al.APSNo = apo.APSNo
outer apply (SELECT val =  Stuff((select distinct concat( '','',CustPONo)   from #APSColumnGroup where APSNo = al.APSNo and CustPONo <> '''' FOR XML PATH('''')),1,1,'''') ) as CustPO
outer apply (SELECT val =  Stuff((select distinct concat( '','',SP)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('''')),1,1,'''') ) as SP
outer apply (SELECT val =  Stuff((select distinct concat( ''+'',Category)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('''')),1,1,'''') ) as Category
outer apply (SELECT val =  Stuff((select distinct concat( '','',Colorway)   from #APSListArticle where APSNo = al.APSNo FOR XML PATH('''')),1,1,'''') ) as Colorway
outer apply (SELECT val =  Stuff((select distinct concat( '','',CdCodeID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('''')),1,1,'''') ) as CDCode
outer apply (SELECT val =  Stuff((select distinct concat( '','',ProductionFamilyID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('''')),1,1,'''') ) as ProductionFamilyID
outer apply (SELECT val =  Stuff((select distinct concat( '','',StyleID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('''')),1,1,'''') ) as Style
outer apply (SELECT val =  Stuff((select distinct concat( '','',Artwork)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('''')),1,1,'''') ) as ArtworkType
outer apply (select [KPILETA] = MAX(KPILETA),[MTLETA] = MAX(MTLETA),[InspDate] = MAX(InspDate) from #APSColumnGroup where APSNo = al.APSNo) as OrderMax
outer apply (SELECT val =  Stuff((select distinct concat( '','',Remarks)   from #APSRemarks where APSNo = al.APSNo FOR XML PATH('''')),1,1,'''') ) as Remarks
outer apply (SELECT MaxSCIDelivery = Max(SCIDelivery),MinSCIDelivery = Min(SCIDelivery),
                    MaxBuyerDelivery = Max(BuyerDelivery),MinBuyerDelivery = Min(BuyerDelivery)
                    from #APSColumnGroup where APSNo = al.APSNo) as OrderDateInfo
'

Declare @sql4 nvarchar(max) = N'
--組出所有計畫最大Inline,最小Offline之間所有的日期，後面展開個計畫每日資料使用
Declare @StartDate date
Declare @EndDate date
select @StartDate = min(Inline),@EndDate = max(Offline)
from #APSList

SELECT f.FactoryID,cast(DATEADD(DAY,number,@StartDate) as datetime) [WorkDate]
into #WorkDate
FROM master.dbo.spt_values s
cross join (select distinct FactoryID from #APSList) f
WHERE s.type = ''P''
AND DATEADD(DAY,number,@StartDate) <= @EndDate

--展開計畫日期資料
select  al.APSNo,
        al.LearnCurveID,
        wkd.SewingLineID,
        wkd.FactoryID,
        [WorkDate] = cast( wkd.Date as datetime),
		al.inline,
		al.Offline,
		al.inlineDate,
		al.OfflineDate,
        [StartHour] = cast(wkd.StartHour as float),
        [EndHour] = cast(wkd.EndHour as float),
        al.InlineHour,
        al.OfflineHour,
		al.HourOutput,
		al.OriWorkHour,
		al.CPU,
		al.TotalSewingTime,
		al.Sewer,
		al.OrderID,
		al.LNCSERIALNumber,
        al.ComboType
into #Workhour_step1
from #APSListWorkDay al
inner join #WorkDate wd on wd.WorkDate >= al.InlineDate and wd.WorkDate <= al.OfflineDate and wd.FactoryID = al.FactoryID
inner join ['+@ServerName+'].Production.dbo.Workhour_Detail wkd with (nolock) on wkd.FactoryID = al.FactoryID and 
                                                wkd.SewingLineID = al.SewingLineID and 
                                                wkd.Date = wd.WorkDate

--刪除每個計畫inline,offline當天超過時間的班表                                                
delete #Workhour_step1 where WorkDate = InlineDate and EndHour <= InlineHour
delete #Workhour_step1 where WorkDate = OfflineDate and StartHour >= OfflineHour

--排出每天班表順序
select  APSNo,
        LearnCurveID,
        SewingLineID,
        FactoryID,
        WorkDate,
		inline,
		Offline,
		inlineDate,
		OfflineDate,
        StartHour,
        EndHour,
        InlineHour,
        OfflineHour,
		[StartHourSort] = ROW_NUMBER() OVER (PARTITION BY APSNo,WorkDate,OrderID,ComboType ORDER BY StartHour),
		[EndHourSort] = ROW_NUMBER() OVER (PARTITION BY APSNo,WorkDate,OrderID,ComboType ORDER BY EndHour desc),
		HourOutput,
		OriWorkHour,
		CPU,
		TotalSewingTime,
		Sewer,
		OrderID,
		LNCSERIALNumber,
        ComboType
into #Workhour_step2
from #Workhour_step1	

--依照班表順序，將inline,offline當天StartHour與EndHour update與inline,offline相同
update #Workhour_step2 set StartHour = InlineHour where WorkDate = InlineDate and StartHourSort = 1 and InlineHour > StartHour
update #Workhour_step2 set EndHour = OfflineHour where WorkDate = OfflineDate and EndHourSort = 1 and OfflineHour < EndHour

select 
APSNo,
LearnCurveID,
[SewingStart] = DATEADD(mi, min(StartHour) * 60,   WorkDate),
[SewingEnd] = DATEADD(mi, max(EndHour) * 60,   WorkDate),
WorkDate,
[WorkingTime] = sum(EndHour - StartHour),
[OriWorkDateSer] = ROW_NUMBER() OVER (PARTITION BY APSNo,OrderID,ComboType ORDER BY WorkDate),
HourOutput,
OriWorkHour,
CPU,
TotalSewingTime,
Sewer,
LNCSERIALNumber
into #APSExtendWorkDate_step1
from #Workhour_step2 
group by APSNo,LearnCurveID,WorkDate,HourOutput,
OriWorkHour,CPU,TotalSewingTime,Sewer,OrderID,LNCSERIALNumber,ComboType

--欄位 LNCSERIALNumber 
--    第一天學習曲線計算方式 = 最後一天對應的工作天數 『減去』（該計畫總生產天數 『減去』一天因為要推出第一天）
--    ※ 但請注意以下特出例外狀況
--       1. LNCSERIALNumber 為空值或零
--       2. 計算後的結果為零或負數
--       遇到以上特殊例外狀況，
--       生產的第一天都對應學習曲線的第一天。
select
APSNo,
LearnCurveID,
SewingStart,
SewingEnd,
WorkDate,
WorkingTime,
OriWorkDateSer,
[WorkDateSer] = case	when isnull(LNCSERIALNumber,0) = 0 then OriWorkDateSer
						when LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) <= 0 then OriWorkDateSer
						else OriWorkDateSer + LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) end,
HourOutput,
OriWorkHour,
CPU,
TotalSewingTime,
Sewer,
LNCSERIALNumber
into #APSExtendWorkDate
from #APSExtendWorkDate_step1

'

Declare @sql5 nvarchar(max) = N'
--取得每個計劃去除LearnCurve後的總工時
SELECT
awd.APSNo,
awd.WorkDate,
[TotalWorkHour] = sum(OriWorkHour)
into #OriTotalWorkHour
FROM #APSExtendWorkDate awd
group by awd.APSNo,awd.WorkDate

--取得LearnCurve Efficiency by Work Date
select  awd.APSNo
        , awd.SewingStart
        , awd.SewingEnd
        , [SewingOutput] = isnull(apo.SewingOutput,0)
        , awd.WorkingTime
        , [LearnCurveEff] = ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))
        , [StdOutput] = SUM(iif (isnull (otw.TotalWorkHour, 0) = 0, 0, awd.WorkingTime * awd.HourOutput * OriWorkHour / otw.TotalWorkHour)) * ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0
        , [CPU] = SUM(iif (isnull (otw.TotalWorkHour, 0) = 0, 0, awd.WorkingTime * awd.HourOutput * OriWorkHour / otw.TotalWorkHour * awd.CPU)) * ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0
        , [Efficienycy] = SUM(iif (isnull (otw.TotalWorkHour, 0) = 0, 0, awd.WorkingTime * awd.HourOutput * OriWorkHour / otw.TotalWorkHour * awd.TotalSewingTime)) * ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0 / (awd.WorkingTime * awd.Sewer * 3600.0)
into #APSExtendWorkDateFin
from #APSExtendWorkDate awd
inner join #OriTotalWorkHour otw on otw.APSNo = awd.APSNo and otw.WorkDate = awd.WorkDate
left join ['+@ServerName+'].Production.dbo.LearnCurve_Detail lcd with (nolock) on awd.LearnCurveID = lcd.ID and awd.WorkDateSer = lcd.Day
left join #APSSewingOutput apo on awd.APSNo = apo.APSNo and awd.WorkDate = apo.OutputDate
outer apply(select top 1 [val] = Efficiency from ['+@ServerName+'].Production.dbo.LearnCurve_Detail where ID = awd.LearnCurveID order by Day desc ) LastEff
group by awd.APSNo,
		 awd.SewingStart,
		 awd.SewingEnd,
		 isnull(apo.SewingOutput,0),
		 awd.WorkingTime,
		 ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0)),
		 awd.Sewer

--計算這一天的標準產量
--= (工作時數 / 車縫一件成衣需要花費的秒數) * 工人數 * 效率
--= (WorkingTime / SewingTime) * ManPower * Eff
--組合最終table
select
apm.APSNo,
apm.SewingLineID,
SewingDay = cast(apf.SewingStart as date),
SewingStartTime=apf.SewingStart,
SewingEndTime=apf.SewingEnd,
apm.MDivisionID,
apm.FactoryID,
PO=apm.CustPO,
POCount=apm.CustPoCnt,
apm.SP,
SPCount=apm.SpCnt,
EarliestSCIdelivery=apm.MinSCIDelivery,
LatestSCIdelivery=apm.MaxSCIDelivery,
EarliestBuyerdelivery=apm.MinBuyerDelivery,
LatestBuyerdelivery=apm.MaxBuyerDelivery,
apm.Category,
apm.Colorway,
ColorwayCount=apm.ColorwayCnt,
apm.CDCode,
apm.ProductionFamilyID,
apm.Style,
StyleCount=apm.StyleCnt,
apm.OrderQty,
apm.AlloQty,
StardardOutputPerDay = apf.StdOutput,
apf.CPU,
WorkHourPerDay = apf.WorkingTime,
StardardOutputPerHour = iif(apf.WorkingTime = 0,0,floor(apf.StdOutput / apf.WorkingTime)),
apf.Efficienycy,
ScheduleEfficiency=round(apm.OriEff / 100.0,2),
LineEfficiency=round(apm.SewLineEff / 100.0,2),
LearningCurve = round(apf.LearnCurveEff / 100.0,2),
SewingInline=apm.Inline,
SewingOffline=apm.Offline,
apm.PFRemark,
apm.MTLComplete,
apm.KPILETA,
apm.MTLETA,
apm.ArtworkType,
InspectionDate=apm.InspDate,
apm.Remarks,
CuttingOutput=round(apm.CuttingOutput,2),
apf.SewingOutput,
apm.ScannedQty,
apm.ClogQty
into #Final
from #APSMain apm
inner join #APSExtendWorkDateFin apf on apm.APSNo = apf.APSNo
order by apm.APSNo,apf.SewingStart

'

Declare @sql6 nvarchar(max) = N'

INSERT into P_SewingLineSchedule (
 s.[APSNo]
,s.[SewingLineID]
,s.[SewingDay]
,s.[SewingStartTime]
,s.[SewingEndTime]
,s.[MDivisionID]
,s.[FactoryID]
,s.[PO]
,s.[POCount]
,s.[SP]
,s.[SPCount]
,s.[EarliestSCIdelivery]
,s.[LatestSCIdelivery]
,s.[EarliestBuyerdelivery]
,s.[LatestBuyerdelivery]
,s.[Category]
,s.[Colorway]
,s.[ColorwayCount]
,s.[CDCode]
,s.[ProductionFamilyID]
,s.[Style]
,s.[StyleCount]
,s.[OrderQty]
,s.[AlloQty]
,s.[StardardOutputPerDay]
,s.[CPU]
,s.[WorkHourPerDay]
,s.[StardardOutputPerHour]
,s.[Efficienycy]
,s.[ScheduleEfficiency]
,s.[LineEfficiency]
,s.[LearningCurve]
,s.[SewingInline]
,s.[SewingOffline]
,s.[PFRemark]
,s.[MTLComplete]
,s.[KPILETA]
,s.[MTLETA]
,s.[ArtworkType]
,s.[InspectionDate]
,s.[Remarks]
,s.[CuttingOutput]
,s.[SewingOutput]
,s.[ScannedQty]
,s.[ClogQty]
)

select * from #Final

drop table	#APSListWorkDay,#APSList,#APSMain,#APSExtendWorkDateFin,#APSOrderQty,#APSCuttingOutput,#APSPackingQty,#APSSewingOutput,#APSRemarks,
			#tmpOrderArtwork,#APSListArticle,#APSColumnGroup,#WorkDate,#APSExtendWorkDate,#Workhour_step1,#Workhour_step2,#OriTotalWorkHour,#APSExtendWorkDate_step1,#Final
		  ;
'
SET @SqlCmd_Combin = @Sql1 + @Sql2 + @Sql3 + @Sql4 + @Sql5 + @sql6

EXEC sp_executesql @SqlCmd_Combin
END
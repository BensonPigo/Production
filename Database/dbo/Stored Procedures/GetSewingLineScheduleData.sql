CREATE PROCEDURE [dbo].[GetSewingLineScheduleData]
	@Inline DATE = null,
	@Offline DATE = null,
	@Line1 varchar(10) = '',
	@Line2 varchar(10) = '',
	@MDivisionID varchar(10) = '',
	@FactoryID varchar(10) = '',
	@BuyerDelivery1 date = null,
	@BuyerDelivery2 date = null,
	@SciDelivery1 date = null,
	@SciDelivery2 date = null,
	@Brand varchar(10) = ''
AS
BEGIN
	SET NOCOUNT ON;
--Declare @Inline nvarchar(20) = format(dateadd(Day,-15,getdate()) ,'yyyy/MM/dd' )
--畫面抓取條件，取得APSNo

declare  @APSListWorkDay TABLE(
	[APSNo] [int] NULL,
	[MDivisionID] [varchar](8) NULL,
	[SewingLineID] [varchar](2) NULL,
	[FactoryID] [varchar](8) NULL,
	[InlineDate] [date] NULL,
	[OfflineDate] [date] NULL,
	[Inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[InlineHour] [numeric](17, 6) NULL,
	[OfflineHour] [numeric](17, 6) NULL,
	[OriEff] [numeric](5, 2) NULL,
	[SewLineEff] [numeric](5, 2) NULL,
	[LearnCurveID] [int] NULL,
	[Sewer] [int] NULL,
	[AlloQty] [int] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [float] NULL,
	[TotalSewingTime] [int] NULL,
	[OrderID] [varchar](13) NOT NULL,
	[LNCSERIALNumber] [int] NULL,
	[ComboType] [varchar](1) NULL,
	[SwitchTime] [int] NULL
)
insert into @APSListWorkDay
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
	[OriWorkHour] = iif (isnull(s.Sewer,0) = 0 or isnull(ScheduleEff.val,0) = 0 or isnull(s.TotalSewingTime,0) = 0, 0, sum(s.AlloQty) / ((s.Sewer * 3600.0 * ScheduleEff.val / 100) / s.TotalSewingTime)),
	[CPU] = cast(o.CPU * o.CPUFactor * isnull(dbo.GetOrderLocation_Rate(s.OrderID,s.ComboType),isnull(dbo.GetStyleLocation_Rate(o.StyleUkey,s.ComboType),100)) / 100 as float),
	s.TotalSewingTime,
	s.OrderID,
	s.LNCSERIALNumber,
    s.ComboType,
	s.SwitchTime
from SewingSchedule s  WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join Factory f with (nolock) on f.id = s.FactoryID and Type <> 'S'
left join Country c WITH (NOLOCK) on o.Dest = c.ID
outer apply(select [val] = iif(isnull(s.OriEff,0)=0 or isnull(s.SewLineEff,0)=0, s.MaxEff, isnull(s.OriEff,100) * isnull(s.SewLineEff,100) / 100) ) ScheduleEff
where 1 = 1  
and (convert(date, s.Inline) >= @Inline  or (@Inline  between convert(date,s.Inline) and convert(date,s.Offline)) or @Inline is null)
and (convert(date,s.Offline) <= @Offline or (@Offline between convert(date,s.Inline) and convert(date,s.Offline)) or @Offline is null) 
and (s.MDivisionID = @MDivisionID or @MDivisionID = '')
and (s.FactoryID = @FactoryID or @FactoryID = '')
and (s.SewingLineID >= @Line1 or @Line1 = '')
and (s.SewingLineID <= @Line2 or @Line2 = '')
and (o.BuyerDelivery >= @BuyerDelivery1 or @BuyerDelivery1 is null)
and (o.BuyerDelivery <= @BuyerDelivery2 or @BuyerDelivery2 is null)
and (o.SciDelivery >= @SciDelivery1 or @SciDelivery1 is null)
and (o.SciDelivery <= @SciDelivery2 or @SciDelivery2 is null)
and (o.BrandID = @Brand or @Brand ='')
and s.APSno <> 0
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
			s.LNCSERIALNumber,
			s.SwitchTime

declare @APSList TABLE(
	[APSNo] [int] NULL,
	[MDivisionID] [varchar](8) NULL,
	[SewingLineID] [varchar](2) NULL,
	[FactoryID] [varchar](8) NULL,
	[Inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[LearnCurveID] [int] NULL,
	[Sewer] [int] NULL,
	[OriEff] [numeric](5, 2) NULL,
	[SewLineEff] [numeric](5, 2) NULL,
	[TotalSewingTime] int NULL,
	[AlloQty] [int] NULL
)
insert into @APSList
select 
	APSNo,
	MDivisionID,
	SewingLineID,
	FactoryID,
	Inline,
	Offline,
	LearnCurveID,
	Sewer,
    max(OriEff) as OriEff,
    SewLineEff,
	[TotalSewingTime]=iif(count(1) = 0, 0, SUM(TotalSewingTime) / count(1)),
	AlloQty = sum(AlloQty)
from @APSListWorkDay
group by APSNo,
		 MDivisionID,
		 SewingLineID,
		 FactoryID,
		 Inline,
		 Offline,
		 LearnCurveID,
		 Sewer,
         SewLineEff

--取得OrderQty by APSNo
declare @APSOrderQty TABLE(
	[APSNo] [int] NULL,
	[OrderQty] [int] NULL
)
insert into @APSOrderQty
select  aps.APSNo,[OrderQty] =sum(o.Qty) 
from @APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join Orders o with (nolock) on s.OrderID = o.ID
group by aps.APSNo

declare @APSCuttingOutput TABLE(
	[APSNo] [int] NULL,
	[CuttingOutput] [numeric](38, 2) NULL
)
--取得Cutting Output by APSNo
insert into @APSCuttingOutput
select  aps.APSNo,[CuttingOutput] =sum(cw.Qty) 
from @APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join CuttingOutput_WIP cw with (nolock) on s.OrderID = cw.OrderID
group by aps.APSNo

--取得Packing data by APSNo
declare @APSPackingQty TABLE(
	[APSNo] [int] NULL,
	[ScannedQty] [int] NULL,
	[ClogQty] [int] NULL
)
insert into @APSPackingQty
select  aps.APSNo,[ScannedQty] =sum(pld.ScanQty),[ClogQty] = sum(pld.ShipQty)
from @APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join PackingList_Detail pld with (nolock) on s.OrderID = pld.OrderID and pld.ReceiveDate is not null
group by aps.APSNo

--取得SewingOutput
declare @APSSewingOutput TABLE(
	[APSNo] [int] NULL,
	[OutputDate] [date] NOT NULL,
	[SewingOutput] [int] NULL
) 
insert into @APSSewingOutput
select
aps.APSNo,
so.OutputDate,
[SewingOutput] = sum(isnull(sod.QAQty,0))
from @APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join SewingOutput_Detail sod with (nolock) on s.OrderID = sod.OrderID and s.ComboType = sod.ComboType
inner join SewingOutput so with (nolock) on so.ID = sod.ID and s.SewingLineID = so.SewingLineID
group by	aps.APSNo,
			so.OutputDate

--取得Artwork
declare @tmpAllArtwork TABLE(
	[StyleID] [varchar](15) NULL,
	[Abbreviation] [varchar](2) NULL,
	[Qty] [numeric](6, 0) NULL,
	[TMS] [numeric](5, 0) NULL,
	[Classify] [varchar](1) NULL
)
insert into @tmpAllArtwork
select  distinct
		o.StyleID
        , at.Abbreviation
        , ot.Qty
        , ot.TMS
        , at.Classify
from Order_TmsCost ot WITH (NOLOCK) 
inner join ArtworkType at WITH (NOLOCK)  on ot.ArtworkTypeID = at.ID
inner join Orders o with (nolock) on ot.ID = o.id
where   (ot.Price > 0 or at.Classify in ('O','I') )
        and (at.Classify in ('S','I') or at.IsSubprocess = 1)
        and (ot.TMS > 0 or ot.Qty > 0)
        and at.Abbreviation !=''
		and ot.ID in (select OrderID from SewingSchedule where exists(select 1 from @APSList where APSNo = SewingSchedule.APSNo)) 

declare @tmpArtWork TABLE(
	[StyleID] [varchar](15) NULL,
	[Artwork] [varchar](33) NULL
)
insert into @tmpArtWork
select * 
from (
    select  StyleID
            , Abbreviation+':'+Convert(varchar,Qty) as Artwork 
    from @tmpAllArtwork 
    where Qty > 0
        
    union all
    select  StyleID
            , Abbreviation+':'+Convert(varchar,TMS) as Artwork 
    from @tmpAllArtwork 
    where TMS > 0 and Classify in ('O','I')
) a 

declare @tmpOrderArtwork TABLE(
	[StyleID] [varchar](15) NULL,
	[Artwork] [nvarchar](max) NULL
)
insert into @tmpOrderArtwork
select tmpArtWorkID.StyleID
        , Artwork = Stuff((select   CONCAT(',',Artwork) 
						from @tmpArtWork 
						where StyleID = tmpArtWorkID.StyleID 
						order by Artwork for xml path('')),1,1,'')  
from (
	select distinct StyleID
	from @tmpArtWork
) tmpArtWorkID

declare @APSListArticle TABLE(
	[APSNo] [int] NULL INDEX IDX_TMP_APSListArticle,
	[Colorway] [varchar](40) NULL
)
insert into @APSListArticle
--取得order對應的Articl
select
s.APSNo,
[Colorway] = Rtrim(sd.Article) +'(' + cast(SUM(sd.AlloQty) as varchar) + ')'
from SewingSchedule s with (nolock)
inner join SewingSchedule_Detail sd with (nolock) on s.ID = sd.ID
where exists( select 1 from @APSList where APSNo = s.APSNo)
group by s.APSNo,sd.Article

--取得 Remarks欄位
declare @APSRemarks TABLE(
	[APSNo] [int] NULL,
	[Remarks] [varchar](50) NULL
)
insert into @APSRemarks
select
s.APSNo,
[Remarks] = s.OrderID + '(' + s_other.SewingLineID + ',' + s.ComboType + ',' + CAST(sum(s_other.AlloQty) as varchar) + ')'
from SewingSchedule s with (nolock)
inner join SewingSchedule s_other on s_other.OrderID = s.OrderID and s_other.ComboType = s.ComboType and s_other.APSNo <> s.APSNo
where  exists( select 1 from @APSList where APSNo = s.APSNo)
group by s.APSNo,s.OrderID,s_other.SewingLineID,s.ComboType

--取得每個計劃需要串接起來的欄位，供後續使用
declare @APSColumnGroup TABLE(
	[APSNo] [int] NULL INDEX IDX_TMP_APSColumnGroup CLUSTERED,
	[CustPONo] [varchar](30) NULL,
	[SP] [varchar](16) NULL,
	[CdCodeID] [varchar](6) NULL,
	[ProductionFamilyID] [varchar](20) NULL,
	[StyleID] [varchar](15) NULL,
	[PFOrder] [bit] NULL,
	[MTLExport] [varchar](2) NULL,
	[KPILETA] [date] NULL,
	[MTLETA] [date] NULL,
	[Artwork] [nvarchar](max) NULL,
	[InspDate] [date] NULL,
	[Category] [varchar](1) NULL,
	[SCIDelivery] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[BrandID] [nvarchar](8) NULL,
	[OrderID] [varchar](13) NULL,
	[FirststCuttingOutputDate] [Date] NULL
)
insert into @APSColumnGroup
select
APSNo,
o.CustPONo,
[SP] = s.OrderID+'(' + s.ComboType + ')',
o.CdCodeID,
cd.ProductionFamilyID,
o.StyleID,
o.PFOrder,
o.MTLExport,
o.KPILETA,
o.MTLETA,
[Artwork] = o.StyleID+'('  +oa.Artwork + ')',
InspDate = InspctDate.Val,
o.Category,
o.SCIDelivery,
o.BuyerDelivery,
[BrandID] = o.BrandID
,s.OrderID
,[FirststCuttingOutputDate]=FirststCuttingOutputDate.Date
from SewingSchedule s with (nolock)
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join CDCode cd with (nolock) on o.CdCodeID = cd.ID
left join @tmpOrderArtwork oa on oa.StyleID = o.StyleID
left join Country c WITH (NOLOCK) on o.Dest = c.ID 
OUTER APPLY(	
	SELECT [Date]=MIN(co2.cDate)
	FROM  WorkOrder_Distribute wd2 WITH (NOLOCK)
	INNER JOIN CuttingOutput_Detail cod2 WITH (NOLOCK) on cod2.WorkOrderUkey = wd2.WorkOrderUkey
	INNER JOIN CuttingOutput co2 WITH (NOLOCK) on co2.id = cod2.id and co2.Status <> 'New'
	where wd2.OrderID =o.ID
)FirststCuttingOutputDate
OUTER APPLY(
	select [Val]=MAX(CFAFinalInspectDate)
	from Order_QtyShip oq
	WHERE ID = o.id
)InspctDate
where exists( select 1 from @APSList where APSNo = s.APSNo)

--填入資料串連欄位 by APSNo
declare @APSMain TABLE(
	[APSNo] [int] NULL,
	[SewingLineID] [varchar](2) NULL,
	[CustPO] [nvarchar](max) NULL,
	[CustPoCnt] [bigint] NULL,
	[SP] [nvarchar](max) NULL,
	[SpCnt] [int] NULL,
	[Colorway] [nvarchar](max) NULL,
	[ColorwayCnt] [bigint] NULL,
	[CDCode] [nvarchar](max) NULL,
	[ProductionFamilyID] [nvarchar](max) NULL,
	[Style] [nvarchar](max) NULL,
	[StyleCnt] [bigint] NULL,
	[OrderQty] [int] NULL,
	[AlloQty] [int] NULL,
	[LearnCurveID] [int] NULL,
	[Inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[PFRemark] [varchar](1) NOT NULL,
	[MTLComplete] [varchar](1) NOT NULL,
	[KPILETA] [date] NULL,
	[MTLETA] [date] NULL,
	[ArtworkType] [nvarchar](max) NULL,
	[InspDate] [date] NULL,
	[Remarks] [nvarchar](max) NULL,
	[FirststCuttingOutputDate] [Date] NULL,
	[CuttingOutput] [numeric](38, 2) NOT NULL,
	[ScannedQty] [int] NOT NULL,
	[ClogQty] [int] NOT NULL,
	[MDivisionID] [varchar](8) NULL,
	[FactoryID] [varchar](8) NULL,
	[Sewer] [int] NULL,
	[Category] [nvarchar](max) NULL,
	[MaxSCIDelivery] [date] NULL,
	[MinSCIDelivery] [date] NULL,
	[MaxBuyerDelivery] [date] NULL,
	[MinBuyerDelivery] [date] NULL,
	[OriEff] [numeric](5, 2) NULL,
	[SewLineEff] [numeric](5, 2) NULL,
	[TotalSewingTime] int NULL,
	[BrandID] [nvarchar](500) NULL
)
insert into @APSMain
select
	al.APSNo,
	al.SewingLineID,
	[CustPO] = CustPO.val,
	[CustPoCnt] =  iif(LEN(CustPO.val) > 0,(LEN(CustPO.val) - LEN(REPLACE(CustPO.val, ',', ''))) / LEN(',') + 1,0),  --��,�ƶq�p��CustPO�ƶq
	[SP] = SP.val,
	[SpCnt] = (select count(1) from SewingSchedule where APSNo = al.APSNo),
	[Colorway] = Colorway.val,
	[ColorwayCnt] = iif(LEN(Colorway.val) > 0,(LEN(Colorway.val) - LEN(REPLACE(Colorway.val, ',', ''))) / LEN(',') + 1,0),  --��,�ƶq�p��Colorway�ƶq
	[CDCode] = CDCode.val,
	[ProductionFamilyID] = ProductionFamilyID.val,
	[Style] = Style.val,
	[StyleCnt] = iif(LEN(Style.val) > 0,(LEN(Style.val) - LEN(REPLACE(Style.val, ',', ''))) / LEN(',') + 1,0),
	aoo.OrderQty,
	al.AlloQty,
	al.LearnCurveID,
	al.Inline,
	al.Offline,
	[PFRemark] = iif(exists(select 1 from @APSColumnGroup where APSNo = al.APSNo and PFOrder = 1),'Y',''),
	[MTLComplete] = iif(exists(select 1 from @APSColumnGroup where APSNo = al.APSNo and MTLExport = ''),'','Y'),
	OrderMax.KPILETA,
	OrderMax.MTLETA,
	[ArtworkType] = ArtworkType.val,
	OrderMax.InspDate,
	[Remarks] = Remarks.val,
	SP.FirststCuttingOutputDate,
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
	SewLineEff,
	TotalSewingTime,
	[BrandID] = BrandID.val
from @APSList al
left join @APSCuttingOutput aco on al.APSNo = aco.APSNo
left join @APSOrderQty aoo on al.APSNo = aoo.APSNo
left join @APSPackingQty apo on al.APSNo = apo.APSNo
outer apply (SELECT val =  Stuff((select distinct concat( ',',CustPONo)   from @APSColumnGroup where APSNo = al.APSNo and CustPONo <> '' FOR XML PATH('')),1,1,'') ) as CustPO
outer apply (
	SELECT val =  Stuff((select distinct concat( ',',SP)   from @APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') 
			,FirststCuttingOutputDate=(
				SELECT [Date]=MIN(co2.cDate)
				FROM  WorkOrder_Distribute wd2 WITH (NOLOCK)
				INNER JOIN CuttingOutput_Detail cod2 WITH (NOLOCK) on cod2.WorkOrderUkey = wd2.WorkOrderUkey
				INNER JOIN CuttingOutput co2 WITH (NOLOCK) on co2.id = cod2.id and co2.Status <> 'New'
				where wd2.OrderID IN (SELECT OrderID from @APSColumnGroup where APSNo = al.APSNo )
			)
) as SP
outer apply (SELECT val =  Stuff((select distinct concat( '+',Category)   from @APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Category
outer apply (SELECT val =  Stuff((select distinct concat( ',',Colorway)   from @APSListArticle where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Colorway
outer apply (SELECT val =  Stuff((select distinct concat( ',',CdCodeID)   from @APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as CDCode
outer apply (SELECT val =  Stuff((select distinct concat( ',',ProductionFamilyID)   from @APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as ProductionFamilyID
outer apply (SELECT val =  Stuff((select distinct concat( ',',StyleID)   from @APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Style
outer apply (SELECT val =  Stuff((select distinct concat( ',',Artwork)   from @APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as ArtworkType
outer apply (select [KPILETA] = MAX(KPILETA),[MTLETA] = MAX(MTLETA),[InspDate] = MAX(InspDate) from @APSColumnGroup where APSNo = al.APSNo) as OrderMax
outer apply (SELECT val =  Stuff((select distinct concat( ',',Remarks)   from @APSRemarks where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Remarks
outer apply (SELECT MaxSCIDelivery = Max(SCIDelivery),MinSCIDelivery = Min(SCIDelivery),
                    MaxBuyerDelivery = Max(BuyerDelivery),MinBuyerDelivery = Min(BuyerDelivery)
                    from @APSColumnGroup where APSNo = al.APSNo) as OrderDateInfo
outer apply (SELECT val =  Stuff((select distinct concat( ',',BrandID)   from @APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as BrandID

--組出所有計畫最大Inline,最小Offline之間所有的日期，後面展開個計畫每日資料使用
Declare @StartDate date
Declare @EndDate date
select @StartDate = min(Inline),@EndDate = max(Offline)
from @APSList

Declare @WorkDate table(
	[FactoryID] [varchar](8) NULL,
	[WorkDate] [datetime] NULL
)
insert into @WorkDate
SELECT f.FactoryID,cast(DATEADD(DAY,number,@StartDate) as datetime) [WorkDate]
FROM master..spt_values s
cross join (select distinct FactoryID from @APSList) f
WHERE s.type = 'P'
AND DATEADD(DAY,number,@StartDate) <= @EndDate

--展開計畫日期資料
Declare @Workhour_step1 table(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingLineID] [varchar](2) NULL,
	[FactoryID] [varchar](8) NULL,
	[WorkDate] [datetime] NULL,
	[inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[inlineDate] [date] NULL,
	[OfflineDate] [date] NULL,
	[StartHour] [float] NULL,
	[EndHour] [float] NULL,
	[InlineHour] [numeric](17, 6) NULL,
	[OfflineHour] [numeric](17, 6) NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [float] NULL,
	[TotalSewingTime] [int] NULL,
	[Sewer] [int] NULL,
	[OrderID] [varchar](13) NOT NULL,
	[LNCSERIALNumber] [int] NULL,
	[ComboType] [varchar](1) NULL,
	[SwitchTime] [int] NULL
)
insert into @Workhour_step1
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
        al.ComboType,
		al.SwitchTime
from @APSListWorkDay al
inner join @WorkDate wd on wd.WorkDate >= al.InlineDate and wd.WorkDate <= al.OfflineDate and wd.FactoryID = al.FactoryID
inner join Workhour_Detail wkd with (nolock) on wkd.FactoryID = al.FactoryID and 
                                                wkd.SewingLineID = al.SewingLineID and 
                                                wkd.Date = wd.WorkDate

--刪除每個計畫inline,offline當天超過時間的班表                                                
delete @Workhour_step1 where WorkDate = InlineDate and EndHour <= InlineHour
delete @Workhour_step1 where WorkDate = OfflineDate and StartHour >= OfflineHour

--排出每天班表順序
Declare @Workhour_step2 table(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingLineID] [varchar](2) NULL,
	[FactoryID] [varchar](8) NULL,
	[WorkDate] [datetime] NULL,
	[inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[inlineDate] [date] NULL,
	[OfflineDate] [date] NULL,
	[StartHour] [float] NULL,
	[EndHour] [float] NULL,
	[InlineHour] [numeric](17, 6) NULL,
	[OfflineHour] [numeric](17, 6) NULL,
	[StartHourSort] [bigint] NULL,
	[EndHourSort] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [float] NULL,
	[TotalSewingTime] [int] NULL,
	[Sewer] [int] NULL,
	[OrderID] [varchar](13) NOT NULL,
	[LNCSERIALNumber] [int] NULL,
	[ComboType] [varchar](1) NULL,
	[SwitchTime] [int] NULL
)
insert into @Workhour_step2
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
        ComboType,		
		SwitchTime
from @Workhour_step1	

--依照班表順序，將inline,offline當天StartHour與EndHour update與inline,offline相同
update @Workhour_step2 set StartHour = InlineHour where WorkDate = InlineDate and StartHourSort = 1 and InlineHour > StartHour
update @Workhour_step2 set EndHour = OfflineHour where WorkDate = OfflineDate and EndHourSort = 1 and OfflineHour < EndHour

Declare @APSExtendWorkDate_step1 table(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [float] NULL,
	[WorkingTime] [float] NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [float] NULL,
	[TotalSewingTime] [int] NULL,
	[Sewer] [int] NULL,
	[LNCSERIALNumber] [int] NULL
)
insert into @APSExtendWorkDate_step1
select 
APSNo,
LearnCurveID,
[SewingStart] = DATEADD(mi, min(StartHour) * 60,   WorkDate),
[SewingEnd] = DATEADD(mi, max(EndHour) * 60,   WorkDate),
SwitchTime,
WorkDate,
[Work_Minute] = sum(EndHour - StartHour) * 60,--round(sum(EndHour - StartHour) * 60,4),
[WorkingTime] = sum(EndHour - StartHour),--ROUND(sum(EndHour - StartHour),4),
[OriWorkDateSer] = ROW_NUMBER() OVER (PARTITION BY APSNo,OrderID,ComboType ORDER BY WorkDate),
HourOutput,
OriWorkHour,
CPU,
TotalSewingTime,
Sewer,
LNCSERIALNumber
from @Workhour_step2 
group by APSNo,LearnCurveID,WorkDate,HourOutput,
OriWorkHour,CPU,TotalSewingTime,Sewer,OrderID,LNCSERIALNumber,ComboType,SwitchTime

/* 
相同APSNo第一筆SewingStart Time 加上Switch Time
如果超過SewingEnd Time, 剩餘的時間 則往下一筆"相同"APSNo的SewingStart Time加上 , 以此類推
******
Work hour/Day 扣除Switch Time, 如過不夠扣除則將剩餘的分鐘數
往同APSNo 第二筆扣除, 以此類推
若Work Hour/Day 不夠扣除,則該筆APSNo不顯示
*/
-- 取得相同APSNO 加總的WorkTime by Minute
Declare @APSExtendWorkDate_step2 table(
	[Sum_Work_Minute] [float] NULL,
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [float] NULL,
	[WorkingTime] [float] NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [float] NULL,
	[TotalSewingTime] [int] NULL,
	[Sewer] [int] NULL,
	[LNCSERIALNumber] [int] NULL	
)
insert into @APSExtendWorkDate_step2
select [Sum_Work_Minute] = sum(Work_Minute) over(partition by APSNo order by SewingStart)
,* 
from @APSExtendWorkDate_step1
order by APSNo,SewingStart

-- 取得遞減的SwitchTime
Declare @APSExtendWorkDate_step3 table(
	[New_SwitchTime] [float] NULL,
	[Sum_Work_Minute] [float] NULL,
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [float] NULL,
	[WorkingTime] [float] NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [float] NULL,
	[TotalSewingTime] [int] NULL,
	[Sewer] [int] NULL,
	[LNCSERIALNumber] [int] NULL
)
insert into @APSExtendWorkDate_step3
select 
[New_SwitchTime] = case when (SwitchTime - LAG(Sum_Work_Minute,1,0) over (partition by APSNo order by sewingstart) <= 0) then 0
else SwitchTime - LAG(Sum_Work_Minute,1,0) over (partition by APSNo order by sewingstart)
end
,*
from @APSExtendWorkDate_step2

--取得遞減的Work Minute 
Declare @APSExtendWorkDate_step4 table(
	[New_Work_Minute] [float] NULL,
	[New_SwitchTime]  [float] NULL,
	[Sum_Work_Minute] [float] NULL,
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [float] NULL,
	[WorkingTime] [float] NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [float] NULL,
	[TotalSewingTime] [int] NULL,
	[Sewer] [int] NULL,
	[LNCSERIALNumber] [int] NULL
)
insert into @APSExtendWorkDate_step4
select 
[New_Work_Minute] = 
case when Work_Minute = Sum_Work_Minute and Work_Minute - SwitchTime > 0 then Work_Minute - SwitchTime
     when Work_Minute != Sum_Work_Minute and Sum_Work_Minute - Work_Minute > SwitchTime then Work_Minute
	 when LAG(New_SwitchTime,1,0) OVER (PARTITION BY APSNo ORDER BY SewingStart) !=0 
		and Work_Minute - LAG(New_SwitchTime,1,0) OVER (PARTITION BY APSNo ORDER BY SewingStart) > 0 
		then Work_Minute - LAG(New_SwitchTime,1,0) OVER (PARTITION BY APSNo ORDER BY SewingStart)
	 else 0
	 end
,* 
from @APSExtendWorkDate_step3

--欄位 LNCSERIALNumber 
--    第一天學習曲線計算方式 = 最後一天對應的工作天數 『減去』（該計畫總生產天數 『減去』一天因為要推出第一天）
--    ※ 但請注意以下特出例外狀況
--       1. LNCSERIALNumber 為空值或零
--       2. 計算後的結果為零或負數
--       遇到以上特殊例外狀況，
--       生產的第一天都對應學習曲線的第一天。
Declare @APSExtendWorkDate table(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[WorkDate] [datetime] NULL,
	[New_WorkingTime] [float] NULL,
	[New_SwitchTime] [float] NULL,
	[WorkingTime] [float] NULL,
	[OriWorkDateSer] [bigint] NULL,
	[WorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [float] NULL,
	[TotalSewingTime] [int] NULL,
	[Sewer] [int] NULL,
	[LNCSERIALNumber] [int] NULL
)
insert into @APSExtendWorkDate
select
APSNo,
LearnCurveID,
SewingStart,
SewingEnd,
WorkDate,
[New_WorkingTime] = IIF(SwitchTime = 0, WorkingTime,  CONVERT(float, New_Work_Minute)/60) ,
[New_SwitchTime]  = IIF(SwitchTime = 0, 0 , CONVERT(float, New_SwitchTime)/60) ,
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
from @APSExtendWorkDate_step4

--取得每個計劃去除LearnCurve後的總工時
Declare @OriTotalWorkHour table(
	[APSNo] [int] NULL,
	[WorkDate] [datetime] NULL,
	[TotalWorkHour] [numeric](38, 13) NULL
)
insert into @OriTotalWorkHour
SELECT
awd.APSNo,
awd.WorkDate,
[TotalWorkHour] = sum(OriWorkHour)
FROM @APSExtendWorkDate awd
group by awd.APSNo,awd.WorkDate

--取得LearnCurve Efficiency by Work Date
Declare @APSExtendWorkDateFin table(
	[APSNo] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SewingOutput] [int] NOT NULL,	
	[WorkingTime] [float] NULL,
	[New_WorkingTime] [float] NULL,
	[New_SwitchTime] [float] NULL,
	[LearnCurveEff] [int] NOT NULL,
	[StdOutput] [float] NULL,
	[CPU] [float] NULL,
	[Efficienycy] [float] NULL
)
insert into @APSExtendWorkDateFin
select  awd.APSNo
        , awd.SewingStart
        , awd.SewingEnd
        , [SewingOutput] = isnull(apo.SewingOutput,0)
        , awd.WorkingTime		
		, awd.New_WorkingTime
		, awd.New_SwitchTime
        , [LearnCurveEff] = ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))
		, StdOutput = s.StdQ
		, [CPU] = SUM(iif (isnull (otw.TotalWorkHour, 0) = 0 or isnull(awd.CPU,0) = 0 or (awd.New_WorkingTime = 0), 0, awd.New_WorkingTime * awd.HourOutput * OriWorkHour / otw.TotalWorkHour * awd.CPU)) * ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0
         , [Efficienycy] = SUM(iif (isnull (otw.TotalWorkHour, 0) = 0 or (awd.TotalSewingTime = 0) or (awd.New_WorkingTime = 0), 0, awd.New_WorkingTime * awd.HourOutput * OriWorkHour / otw.TotalWorkHour * awd.TotalSewingTime)) * iif(isnull(awd.New_WorkingTime,0) = 0 or isnull(awd.Sewer,0) = 0 ,0 , ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0 / (awd.New_WorkingTime * awd.Sewer * 3600.0))
from @APSExtendWorkDate awd
inner join @OriTotalWorkHour otw on otw.APSNo = awd.APSNo and otw.WorkDate = awd.WorkDate
left join LearnCurve_Detail lcd with (nolock) on awd.LearnCurveID = lcd.ID and awd.WorkDateSer = lcd.Day
left join @APSSewingOutput apo on awd.APSNo = apo.APSNo and awd.WorkDate = apo.OutputDate
outer apply(select top 1 [val] = Efficiency from LearnCurve_Detail where ID = awd.LearnCurveID order by Day desc ) LastEff
outer  apply(select * from dbo.[getDailystdq](awd.APSNo)x where x.APSNo=awd.APSNo and x.Date = cast(awd.SewingStart as date))s
group by awd.APSNo,
		 awd.SewingStart,
		 awd.SewingEnd,
		 isnull(apo.SewingOutput,0),
		 awd.WorkingTime,
		 awd.New_SwitchTime,
		 awd.New_WorkingTime,
		 ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0)),
		 awd.Sewer
		  , s.StdQ

--計算這一天的標準產量
--= (工作時數 / 車縫一件成衣需要花費的秒數) * 工人數 * 效率
--= (WorkingTime / SewingTime) * ManPower * Eff
--組合最終table
--delete SewingLineScheduleData
select
	[APSNo]=apm.APSNo,
	[SewingLineID]=apm.SewingLineID,
	[Sewer]=apm.Sewer,
	[SewingDay] = cast(apf.SewingStart as date),
	[SewingStartTime]=apf.SewingStart,
	[SewingEndTime]=apf.SewingEnd,
	[MDivisionID]=apm.MDivisionID,
	[FactoryID]=apm.FactoryID,
	[PO]=apm.CustPO,
	[POCount]=apm.CustPoCnt,
	[SP]=apm.SP,
	[SPCount]=apm.SpCnt,
	[EarliestSCIdelivery]=apm.MinSCIDelivery,
	[LatestSCIdelivery]=apm.MaxSCIDelivery,
	[EarliestBuyerdelivery]=apm.MinBuyerDelivery,
	[LatestBuyerdelivery]=apm.MaxBuyerDelivery,
	[Category]=apm.Category,
	[Colorway]=apm.Colorway,
	[ColorwayCount]=apm.ColorwayCnt,
	[CDCode]=apm.CDCode,
	[ProductionFamilyID]=apm.ProductionFamilyID,
	[Style]=apm.Style,
	[StyleCount]=apm.StyleCnt,
	[OrderQty]=apm.OrderQty,
	[AlloQty]=apm.AlloQty,
	[StardardOutputPerDay]= apf.StdOutput,
	[CPU]=apf.CPU,
	[SewingCPU] = (iif(isnull((SELECT StdTMS * 1.0 FROm System),0)=0, 0, apm.TotalSewingTime / (SELECT StdTMS * 1.0 FROm System)))  ,
	[Orig_WorkHourPerDay]= apf.WorkingTime,
	[New_SwitchTime] = apf.New_SwitchTime,
	[New_WorkHourPerDay] = apf.New_WorkingTime,
	[StardardOutputPerHour] = iif(apf.WorkingTime = 0,0,floor(apf.StdOutput / apf.WorkingTime)),
	[Efficienycy]= ROUND( apf.Efficienycy ,2,1),
	[ScheduleEfficiency]=round(apm.OriEff / 100.0,2),
	[LineEfficiency]=round(apm.SewLineEff / 100.0,2),
	[LearningCurve]= round(apf.LearnCurveEff / 100.0,2),
	[SewingInline]=apm.Inline,
	[SewingOffline]=apm.Offline,
	[PFRemark]=apm.PFRemark,
	[MTLComplete]=apm.MTLComplete,
	[KPILETA]=apm.KPILETA,
	[MTLETA]=apm.MTLETA,
	[ArtworkType]=apm.ArtworkType,
	[InspectionDate]=apm.InspDate,
	[Remarks]=apm.Remarks,
	[FirststCuttingOutputDate]=apm.FirststCuttingOutputDate,
	[CuttingOutput]=round(apm.CuttingOutput,2),
	[SewingOutput]=apf.SewingOutput,
	[ScannedQty]=apm.ScannedQty,
	[ClogQty]=apm.ClogQty,
	[BrandID]=apm.BrandID
from @APSMain apm
inner join @APSExtendWorkDateFin apf on apm.APSNo = apf.APSNo
order by apm.APSNo,apf.SewingStart


END

GO

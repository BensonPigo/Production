CREATE PROCEDURE  [dbo].[GetSewingLineScheduleData]
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
	@Brand varchar(10) = '',
	@subprocess varchar(20) = '',
	@IsPowerBI bit = 0
AS
begin
	SET NOCOUNT ON;

	if @IsPowerBI = 1 and @Inline is null
	begin
		set @Inline = dateadd(day, -90, getdate())
	end

	if @IsPowerBI = 1 and @Offline is null
	begin
		set @Offline = getdate()
	end

--開抓Detail資料
--Declare @Inline nvarchar(20) = format(dateadd(Day,-15,FORMAT(getdate(), 'hh:mm:ss')) ,'yyyy/MM/dd' )
--畫面抓取條件，取得APSNo

declare  @APSListWorkDay TABLE(
	[APSNo] [int] NULL,
	[MDivisionID] [varchar](8) NULL,
	[SewingLineID] [varchar](5) NULL,
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
	[CPU] [decimal](38,13) NULL,
	[TotalSewingTime] [int] NULL,
	[OrderID] [varchar](13) NOT NULL,
	[LNCSERIALNumber] [int] NULL,
	[ComboType] [varchar](1) NULL,
	[SwitchTime] [int] NULL,
	[StyleUkey] [bigint] NULL
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
	[CPU] = cast(o.CPU * o.CPUFactor * isnull(dbo.GetOrderLocation_Rate(s.OrderID,s.ComboType),isnull(dbo.GetStyleLocation_Rate(o.StyleUkey,s.ComboType),100)) / 100 as [decimal](38,13)),
	s.TotalSewingTime,
	s.OrderID,
	s.LNCSERIALNumber,
    s.ComboType,
	s.SwitchTime,
	o.StyleUkey
from SewingSchedule s  WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join Factory f with (nolock) on f.id = s.FactoryID and Type <> 'S'
left join Country c WITH (NOLOCK) on o.Dest = c.ID
outer apply(select [val] = iif(isnull(s.OriEff,0)=0 or isnull(s.SewLineEff,0)=0, s.MaxEff, isnull(s.OriEff,100) * isnull(s.SewLineEff,100) / 100) ) ScheduleEff
where ((@IsPowerBI = 0    
	and (convert(date,s.Inline) >= @Inline  or (@Inline  between convert(date,s.Inline) and convert(date,s.Offline)) or @Inline is null)
	and (convert(date,s.Offline) <= @Offline or (@Offline between convert(date,s.Inline) and convert(date,s.Offline)) or @Offline is null) 
)
or (@IsPowerBI = 1 and ((s.AddDate >= @Inline and s.AddDate < dateadd(day, 1, @Offline))
					or (s.EditDate >= @Inline and s.EditDate < dateadd(day, 1, @Offline))
					or (s.Offline >= @Inline and s.Offline < dateadd(day, 1, @Offline)))
))
and (s.MDivisionID = @MDivisionID or @MDivisionID = '')
and (s.FactoryID = @FactoryID or @FactoryID = '')
and (s.SewingLineID >= @Line1 or @Line1 = '')
and (s.SewingLineID <= @Line2 or @Line2 = '')
and (o.BuyerDelivery >= @BuyerDelivery1 or @BuyerDelivery1 is null)
and (o.BuyerDelivery <= @BuyerDelivery2 or @BuyerDelivery2 is null)
and (o.SciDelivery >= @SciDelivery1 or @SciDelivery1 is null)
and (o.SciDelivery <= @SciDelivery2 or @SciDelivery2 is null)
and (o.BrandID = @Brand or @Brand ='')
and (@subprocess = '' or
	(@subprocess<> '' 
	and exists(select 1 from Style_TmsCost st where o.StyleUkey = st.StyleUkey and st.ArtworkTypeID = @subprocess AND (st.Qty>0 or st.TMS>0 and st.Price>0) ) ))
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
			s.SwitchTime,
			o.StyleUkey
			
CREATE TABLE #StyleData (
    [APSNo] [int] NULL,
    [CDCodeNew] [varchar](Max) NULL,
    [ProductType] [nvarchar](Max) NULL,
    [FabricType] [nvarchar](Max) NULL,
    [Lining] [varchar](Max) NULL,
    [Gender] [varchar](Max) NULL,
    [Construction] [nvarchar](Max) NULL,
    [StyleName] [nvarchar](Max) NULL,
	Index IX_APSNo NonClustered (APSNo)
);
insert into #StyleData
select distinct a.APSNo,
	sty.[CDCodeNew],
	sty.[ProductType],
	sty.[FabricType],
	sty.[Lining],
	sty.[Gender],
	sty.[Construction],
	sty.[StyleName]
from @APSListWorkDay a
Outer apply (
	SELECT
		  ProductType = r2.Name
		, FabricType = r1.Name
		, Lining
		, Gender
		, Construction = d1.Name
		, s.CDCodeNew
		, s.StyleName
	FROM Style s WITH(NOLOCK)
	left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	where s.Ukey = a.StyleUkey
)sty

CREATE TABLE #StyleDatabyAPSNo (
	[APSNo] [int] NULL,
	[CDCodeNew] [varchar](Max) NULL,
	[ProductType] [nvarchar](Max) NULL,
	[FabricType] [nvarchar](Max) NULL,
	[Lining] [varchar](Max) NULL,
	[Gender] [varchar](Max) NULL,
	[Construction] [nvarchar](Max) NULL,
	[StyleName] [nvarchar](Max) NULL,
	Index IX_APSNo NonClustered (APSNo)
)
insert into #StyleDatabyAPSNo
select a.APSNo,[CDCodeNew],[ProductType],[FabricType],[Lining],[Gender],[Construction],[StyleName]
from(select distinct APSNo from #StyleData)a
outer apply (SELECT [CDCodeNew] =  Stuff((select distinct concat( '/',[CDCodeNew]) from #StyleData s where APSNo = a.APSNo FOR XML PATH('')),1,1,'') ) s1
outer apply (SELECT [ProductType] =  Stuff((select distinct concat( '/',[ProductType]) from #StyleData s where APSNo = a.APSNo FOR XML PATH('')),1,1,'') ) s2
outer apply (SELECT [FabricType] =  Stuff((select distinct concat( '/',[FabricType]) from #StyleData s where APSNo = a.APSNo FOR XML PATH('')),1,1,'') ) s3
outer apply (SELECT [Lining] =  Stuff((select distinct concat( '/',[Lining]) from #StyleData s where APSNo = a.APSNo FOR XML PATH('')),1,1,'') ) s4
outer apply (SELECT [Gender] =  Stuff((select distinct concat( '/',[Gender]) from #StyleData s where APSNo = a.APSNo FOR XML PATH('')),1,1,'') ) s5
outer apply (SELECT [Construction] =  Stuff((select distinct concat( '/',[Construction]) from #StyleData s where APSNo = a.APSNo FOR XML PATH('')),1,1,'') ) s6
outer apply (SELECT [StyleName] =  Stuff((select distinct concat( '/',[StyleName]) from #StyleData s where APSNo = a.APSNo FOR XML PATH('')),1,1,'') ) s7

CREATE TABLE #APSList(
	[APSNo] [int] NULL,
	[MDivisionID] [varchar](8) NULL,
	[SewingLineID] [varchar](5) NULL,
	[FactoryID] [varchar](8) NULL,
	[Inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[LearnCurveID] [int] NULL,
	[Sewer] [int] NULL,
	[OriEff] [numeric](5, 2) NULL,
	[SewLineEff] [numeric](5, 2) NULL,
	[TotalSewingTime] int NULL,
	[AlloQty] [int] NULL,
	Index IX_APSNo NonClustered (APSNo)
)
insert into #APSList
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
CREATE TABLE #APSOrderQty(
	[APSNo] [int] NULL,
	[OrderQty] [int] NULL,
    INDEX IX_APSNo NONCLUSTERED (APSNo)
)
insert into #APSOrderQty
select  aps.APSNo,[OrderQty] =sum(o.Qty) 
from #APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join Orders o with (nolock) on s.OrderID = o.ID
where (@subprocess = '' or
	(@subprocess<> '' 
	and exists(select 1 from Style_TmsCost st where o.StyleUkey = st.StyleUkey and st.ArtworkTypeID = @subprocess AND (st.Qty>0 or st.TMS>0 and st.Price>0) ) ))
group by aps.APSNo

CREATE TABLE #APSCuttingOutput(
	[APSNo] [int] NULL,
	[CuttingOutput] [numeric](38, 2) NULL,
    INDEX IX_APSNo NONCLUSTERED (APSNo)
)
--取得Cutting Output by APSNo
insert into #APSCuttingOutput
select  aps.APSNo,[CuttingOutput] =sum(cw.Qty) 
from #APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join CuttingOutput_WIP cw with (nolock) on s.OrderID = cw.OrderID
group by aps.APSNo

--取得Packing data by APSNo
CREATE TABLE #APSPackingQty(
	[APSNo] [int] NULL,
	[ScannedQty] [int] NULL,
	[ClogQty] [int] NULL,
    INDEX IX_APSNo NONCLUSTERED (APSNo)
)
insert into #APSPackingQty
select  aps.APSNo,[ScannedQty] =sum(pld.ScanQty),[ClogQty] = sum(pld.ShipQty)
from #APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join PackingList_Detail pld with (nolock) on s.OrderID = pld.OrderID and pld.ReceiveDate is not null
group by aps.APSNo

--取得SewingOutput
CREATE TABLE #APSSewingOutput(
	[APSNo] [int] NULL,
	[OutputDate] [date] NOT NULL,
	[SewingOutput] [int] NULL,
    INDEX IX_APSNo NONCLUSTERED (APSNo, OutputDate)
) 
insert into #APSSewingOutput
select
aps.APSNo,
so.OutputDate,
[SewingOutput] = sum(isnull(sod.QAQty,0))
from #APSList aps
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
		and ot.ID in (select OrderID from SewingSchedule where exists(select 1 from #APSList where APSNo = SewingSchedule.APSNo)) 		
		and (@subprocess = '' or
			(@subprocess<> '' 
			and exists(select 1 from Style_TmsCost st where o.StyleUkey = st.StyleUkey and st.ArtworkTypeID = @subprocess AND (st.Qty>0 or st.TMS>0 and st.Price>0) ) ))

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

CREATE TABLE #APSListArticle(
	[APSNo] [int] NULL,
	[Colorway] [varchar](40) NULL,
    INDEX IX_APSNo NONCLUSTERED (APSNo)
)
insert into #APSListArticle
--取得order對應的Articl
select
s.APSNo,
[Colorway] = Rtrim(sd.Article) +'(' + cast(SUM(sd.AlloQty) as varchar) + ')'
from SewingSchedule s with (nolock)
inner join SewingSchedule_Detail sd with (nolock) on s.ID = sd.ID
where exists( select 1 from #APSList where APSNo = s.APSNo)
group by s.APSNo,sd.Article

--取得 Remarks欄位
CREATE TABLE #APSRemarks (
	[APSNo] [int] NULL,
	[Remarks] [varchar](50) NULL,
    INDEX IX_APSNo NONCLUSTERED (APSNo)
)
insert into #APSRemarks
select
s.APSNo,
[Remarks] = s.OrderID + '(' + s_other.SewingLineID + ',' + s.ComboType + ',' + CAST(sum(s_other.AlloQty) as varchar) + ')'
from SewingSchedule s with (nolock)
inner join SewingSchedule s_other on s_other.OrderID = s.OrderID and s_other.ComboType = s.ComboType and s_other.APSNo <> s.APSNo
where  exists( select 1 from #APSList where APSNo = s.APSNo)
group by s.APSNo,s.OrderID,s_other.SewingLineID,s.ComboType

--取得每個計劃需要串接起來的欄位，供後續使用
CREATE TABLE #APSColumnGroup (
	[APSNo] [int] NULL,
	[CustPONo] [varchar](30) NULL,
	[SP] [varchar](16) NULL,
	[SP_ComboType][varchar](15) NULL,
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
	[MaxBuyerDelivery] [date] NULL,
	[MinBuyerDelivery] [date] NULL,
	[BrandID] [varchar](500) NULL,
	[OrderID] [varchar](13) NULL,
	[MatchFabric] varchar(8) NULL,
	[StyleSeason] varchar(10) null,
	[StyleUkey] bigint null,
    INDEX IX_APSNo NONCLUSTERED (APSNo)
)
insert into #APSColumnGroup
select
APSNo,
o.CustPONo,
[SP] = s.OrderID+'(' + s.ComboType + ')',
[SP_ComboType] = s.ComboType,
o.CdCodeID,
[ProductionFamilyID] = '',
o.StyleID,
o.PFOrder,
o.MTLExport,
o.KPILETA,
o.MTLETA,
[Artwork] = o.StyleID+'('  +oa.Artwork + ')',
o.InspDate,
o.Category,
o.SCIDelivery,
oq.MaxBuyerDelivery,
oq.MinBuyerDelivery,
[BrandID] = o.BrandID,
s.OrderID,
[MatchFabric] = iif(o.IsNotRepeatOrMapping = 0, 'Y','N'),
[StyleSeason] = Style.SeasonID,
[StyleUkey] = style.Ukey
from SewingSchedule s with (nolock)
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID
inner join Style with (nolock) on Style.Ukey = o.StyleUkey
outer apply(select MaxBuyerDelivery = max(oq.BuyerDelivery), MinBuyerDelivery = min(oq.BuyerDelivery) from Order_QtyShip oq where oq.id = o.id) oq
left join @tmpOrderArtwork oa on oa.StyleID = o.StyleID
left join Country c WITH (NOLOCK) on o.Dest = c.ID 
where exists( select 1 from #APSList where APSNo = s.APSNo)
and (@subprocess = '' or
	(@subprocess<> '' 
	and exists(select 1 from Style_TmsCost st where o.StyleUkey = st.StyleUkey and st.ArtworkTypeID = @subprocess AND (st.Qty>0 or st.TMS>0 and st.Price>0) ) ))


CREATE TABLE #StyleArtwork (
	[APSNo] [int] NULL,
	[StyleID] [varchar](15) NULL,
	[Article] [varchar](8) NULL,
	[AlloRatio] numeric(5,2) NULL,
	[EMBROIDERY] int NULL,
	[PRINTING] int NULL,
    INDEX IX_APSNo NONCLUSTERED (APSNo)
)
insert into #StyleArtwork
select	APSNo,
		StyleID,
		Article,
		[AlloRatio] = AlloQty * 1.0 / sum(AlloQty) over (partition by APSNO),
		EMBROIDERY,
		PRINTING
from (
select  ss.APSNo,
		o.StyleID,
		ssd.Article,
		[AlloQty] = sum(ssd.AlloQty),
		[EMBROIDERY] = MAX(isnull(ArtworkQty.EMBROIDERY, -1)),
		[PRINTING] = MAX(isnull(ArtworkQty.PRINTING, -1))
from SewingSchedule ss with (nolock)
inner join SewingSchedule_Detail ssd with (nolock) on ssd.ID = ss.ID
inner join orders o with (nolock) on o.ID = ss.OrderID
outer apply(select EMBROIDERY, PRINTING from 
				(select  oa.ArtworkTypeID, [Qty] = sum (oa.Qty) over (partition by oa.Article) 
				from Order_Artwork oa with (nolock)
				where oa.id = o.ID and
					  oa.ArtworkTypeID in ('EMBROIDERY', 'PRINTING') and
					  (oa.Article = ssd.Article or oa.Article = '----') and
					  exists(select 1 from Pattern_GL pg with (nolock) 
							 where	pg.PatternUKEY = (select top 1 PatternUkey from dbo.GetPatternUkey(o.POID, '', '', o.StyleUkey, '')) and
									iif(pg.SEQ = '0001', substring(pg.PatternCode, 11, 10), pg.PatternCode) = oa.PatternCode and
									pg.Location = ss.ComboType
							 )) t
				PIVOT (
					MAX(Qty)
					FOR ArtworkTypeID IN ([EMBROIDERY], [PRINTING])
				) p 
			) ArtworkQty
where ss.APSNo in (select APSNo from #APSList)
group by ss.APSNo,o.StyleID,ssd.Article) a
	
declare @tmp2 TABLE(
	id varchar(13) NULL,
	ColumnN varchar(50) null,
	val numeric(38,6),
	Supp varchar(50)
)
insert into @tmp2
select  ot.ID
		,ColumnN = RTRIM(at.ID) + ' ('+at.ArtworkUnit+')'
        , val =  iif(at.ArtworkUnit = 'PCS',isnull(ot.Qty,0),isnull(ot.Price,0) )
		, Supp = IIF(ot.ArtworkTypeID = 'PRINTING', IIF(ot.InhouseOSP = 'O', ls.abb, ot.LocalSuppID), '')
from Order_TmsCost ot WITH (NOLOCK) 
inner join ArtworkType at WITH (NOLOCK) on at.ID = ot.ArtworkTypeID and at.ID in('PRINTING','PRINTING PPU')
left join LocalSupp ls on ls.id = ot.LocalSuppID
where exists(select 1 from #APSColumnGroup where orderid = ot.id)


CREATE TABLE #tmpPrintData (
	[APSNo] [int] NULL,
	OrderID varchar(13),
	[PRINTING (PCS)] numeric(38,6),
	[PRINTING PPU (PPU)] numeric(38,6),
	SubCon nvarchar(max),
	AlloQty int,
    INDEX IX_APSNo NONCLUSTERED (APSNo)
)
insert into #tmpPrintData
select a.APSNo,
	a.OrderID,
	t.[PRINTING (PCS)],
	t.[PRINTING PPU (PPU)],
	t.SubCon,
	a.AlloQty
from @APSListWorkDay a
inner join (
	select t.*,SubCon
	from (
		select id,[PRINTING (PCS)],[PRINTING PPU (PPU)]
		from (select id,ColumnN,val from @tmp2)x
		pivot(min(val) for ColumnN in ([PRINTING (PCS)],[PRINTING PPU (PPU)]))p
	) t
	outer apply(select top 1 SubCon = supp from @tmp2 where id = t.id)s
) t on t.id = a.OrderID


CREATE TABLE #tmpPrintDataSum (
	[APSNo] [int] NULL,
	SubCon nvarchar(max),
	[Subcon Qty] int,
    INDEX IX_APSNo NONCLUSTERED (APSNo)
)
insert into #tmpPrintDataSum
select p.APSNo,x.SubCon, sum(iif(p.SubCon<>'', p.AlloQty, 0))
from #tmpPrintData p
outer apply(
	select SubCon = stuff((
		select distinct concat('/', subcon)
		from #tmpPrintData
		where APSNo = p.APSNo
		for xml path('')
	),1,1,'')
)x
group by p.APSNo,x.SubCon

CREATE TABLE #tmpPrintSumbuSP (
	[APSNo] [int] NULL,
	OrderID varchar(13),
	[PRINTING (PCS)] numeric(38,6),
	[PRINTING PPU (PPU)] numeric(38,6),
    INDEX IX_APSNo NONCLUSTERED (APSNo)
)
insert into #tmpPrintSumbuSP
select p.APSNo,p.OrderID,
	sum(p.[PRINTING (PCS)]),
	sum(p.[PRINTING PPU (PPU)])
from #tmpPrintData p
where subcon <>''
group by p.APSNo,p.OrderID


--填入資料串連欄位 by APSNo
declare @APSMain TABLE(
	[APSNo] [int] NULL,
	[SewingLineID] [varchar](5) NULL,
	[CustPO] [nvarchar](max) NULL,
	[CustPoCnt] [bigint] NULL,
	[SP] [nvarchar](max) NULL,
	[SP_Combotype][varchar](15) NULL,
	[SpCnt] [int] NULL,
	[Colorway] [nvarchar](max) NULL,
	[ColorwayCnt] [bigint] NULL,
	[CDCode] [varchar](max) NULL,
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
	[BrandID] [nvarchar](500) NULL,
	[CDCodeNew] [varchar](Max) NULL,
	[ProductType] [nvarchar](Max) NULL,
	[MatchFabric] [varchar](8) NULL,
	[FabricType] [nvarchar](Max) NULL,
	[Lining] [varchar](Max) NULL,
	[Gender] [varchar](Max) NULL,
	[Construction] [nvarchar](Max) NULL,
	[StyleName] [nvarchar](Max) NULL,
	SubCon nvarchar(max),
	[Subcon Qty] int,
	[EMBStitch] varchar(50),
	[EMBStitchCnt] int,
	[PrintPcs] int,
	[StyleSeason] NVARCHAR(MAX),
	[StyleUkey] [nvarchar](500) NULL,
	[AddDate] [date] NULL,
	[EditDate] [date] NULL
)
insert into @APSMain
select
	al.APSNo,
	al.SewingLineID,
	[CustPO] = CustPO.val,
	[CustPoCnt] =  iif(LEN(CustPO.val) > 0,(LEN(CustPO.val) - LEN(REPLACE(CustPO.val, ',', ''))) / LEN(',') + 1,0),  --用,數量計算CustPO數量
	[SP] = SP.val,
	[SP_Combotype] = SP.SP_Combotype,
	[SpCnt] = (select count(1) from SewingSchedule where APSNo = al.APSNo),
	[Colorway] = Colorway.val,
	[ColorwayCnt] = iif(LEN(Colorway.val) > 0,(LEN(Colorway.val) - LEN(REPLACE(Colorway.val, ',', ''))) / LEN(',') + 1,0),  --用,數量計算Colorway數量
	[CDCode] = CDCode.val,
	[ProductionFamilyID] = ProductionFamilyID.val,
	[Style] = Style.val,
	[StyleCnt] = iif(LEN(Style.val) > 0,(LEN(Style.val) - LEN(REPLACE(Style.val, ',', ''))) / LEN(',') + 1,0),
	aoo.OrderQty,
	al.AlloQty,
	al.LearnCurveID,
	al.Inline,
	al.Offline,
	[PFRemark] = iif(exists(select 1 from #APSColumnGroup where APSNo = al.APSNo and PFOrder = 1),'Y',''),
	[MTLComplete] = iif(exists(select 1 from #APSColumnGroup where APSNo = al.APSNo and MTLExport = ''),'','Y'),
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
	[BrandID] = BrandID.val,
	sty.CDCodeNew,
	sty.ProductType,
	MatchFabric = sp.MatchFabric,
	sty.FabricType,
	sty.Lining,
	sty.Gender,
	sty.Construction,
	sty.StyleName,
	PrintingData.SubCon,
	PrintingData.[Subcon Qty],
	[EMBStitch] = EMBStitch.val,
	[EMBStitchCnt] = LEN(EMBStitch.val) - LEN(REPLACE(EMBStitch.val, ',', '')) + 1,
	[PrintPcs] = (select sum(PRINTING) from (select [PRINTING] = Max(PRINTING) from #StyleArtwork where APSNo = al.APSNo and PRINTING <> -1 group by StyleID) a),
	[StyleSeason] = ISNULL(SP.StyleSeason, ''),
	[StyleUkey] = ISNULL(SP.StyleUkey, ''),
	s.AddDate,
	s.EditDate
from #APSList al
left join #APSCuttingOutput aco on al.APSNo = aco.APSNo
left join #APSOrderQty aoo on al.APSNo = aoo.APSNo
left join #APSPackingQty apo on al.APSNo = apo.APSNo
left join #StyleDatabyAPSNo sty on al.APSNo = sty.APSNo
outer apply (SELECT val =  Stuff((select distinct concat( ',',CustPONo)   from #APSColumnGroup where APSNo = al.APSNo and CustPONo <> '' FOR XML PATH('')),1,1,'') ) as CustPO
outer apply (
	SELECT val =  Stuff((select distinct concat( ',',SP)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') 
		  ,[SP_Combotype] = Stuff((select distinct concat( ',',SP_ComboType)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'')
		  ,[MatchFabric] = Stuff((select distinct concat( ',',MatchFabric)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'')
		  ,[StyleSeason] = Stuff((select distinct concat( ',',StyleSeason)   from #APSColumnGroup where APSNo = al.APSNo group by SP, StyleSeason FOR XML PATH('')),1,1,'')
		  ,[StyleUkey] = Stuff((select distinct concat( ',',StyleUkey)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'')
		  ,FirststCuttingOutputDate=(
				SELECT [Date]=MIN(co2.cDate)
				FROM  WorkOrder_Distribute wd2 WITH (NOLOCK)
				INNER JOIN CuttingOutput_Detail cod2 WITH (NOLOCK) on cod2.WorkOrderUkey = wd2.WorkOrderUkey
				INNER JOIN CuttingOutput co2 WITH (NOLOCK) on co2.id = cod2.id and co2.Status <> 'New'
				where wd2.OrderID IN (SELECT OrderID from #APSColumnGroup where APSNo = al.APSNo )
			)
) as SP
outer apply (SELECT val =  Stuff((select distinct concat( '+',Category)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Category
outer apply (SELECT val =  Stuff((select distinct concat( ',',Colorway)   from #APSListArticle where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Colorway
outer apply (SELECT val =  Stuff((select distinct concat( ',',CdCodeID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as CDCode
outer apply (SELECT val =  Stuff((select distinct concat( ',',ProductionFamilyID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as ProductionFamilyID
outer apply (SELECT val =  Stuff((select distinct concat( ',',StyleID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Style
outer apply (SELECT val =  Stuff((select distinct concat( ',',Artwork)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as ArtworkType
outer apply (select [KPILETA] = MAX(KPILETA),[MTLETA] = MAX(MTLETA),[InspDate] = MAX(InspDate) from #APSColumnGroup where APSNo = al.APSNo) as OrderMax
outer apply (SELECT val =  Stuff((select distinct concat( ',',Remarks)   from #APSRemarks where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Remarks
outer apply (SELECT MaxSCIDelivery = Max(SCIDelivery),MinSCIDelivery = Min(SCIDelivery),
                    MaxBuyerDelivery = Max(MaxBuyerDelivery),MinBuyerDelivery = Min(MinBuyerDelivery)
                    from #APSColumnGroup where APSNo = al.APSNo) as OrderDateInfo
outer apply (SELECT val =  Stuff((select distinct concat( ',',BrandID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as BrandID
outer apply (select * from #tmpPrintDataSum where  APSNo = al.APSNo) as PrintingData
outer apply (select [val] = Stuff((select distinct concat( ',',EMBROIDERY) from #StyleArtwork sa where sa.APSNo = al.APSNo and sa.EMBROIDERY <> -1 FOR XML PATH('')),1,1,'') ) EMBStitch
outer apply (select [AddDate] = min(s.AddDate), [EditDate] = max(s.EditDate) from SewingSchedule s with(nolock) where s.APSNo = al.APSNo) s

--組出所有計畫最大Inline,最小Offline之間所有的日期，後面展開個計畫每日資料使用
Declare @StartDate date
Declare @EndDate date
select @StartDate = min(Inline),@EndDate = max(Offline)
from #APSList

Declare @WorkDate table(
	[FactoryID] [varchar](8) NULL,
	[WorkDate] [datetime] NULL
)
insert into @WorkDate
SELECT f.FactoryID,cast(DATEADD(DAY,number,@StartDate) as datetime) [WorkDate]
FROM master..spt_values s
cross join (select distinct FactoryID from #APSList) f
WHERE s.type = 'P'
AND DATEADD(DAY,number,@StartDate) <= @EndDate

--展開計畫日期資料
Declare @Workhour_step1 table(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingLineID] [varchar](5) NULL,
	[FactoryID] [varchar](8) NULL,
	[WorkDate] [datetime] NULL,
	[inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[inlineDate] [date] NULL,
	[OfflineDate] [date] NULL,
	[StartHour] [decimal](38,13) NULL,
	[EndHour] [decimal](38,13) NULL,
	[InlineHour] [numeric](17, 6) NULL,
	[OfflineHour] [numeric](17, 6) NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [decimal](38,13) NULL,
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
        [StartHour] = cast(wkd.StartHour as [decimal](38,13)),
        [EndHour] = cast(wkd.EndHour as [decimal](38,13)),		
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
	[SewingLineID] [varchar](5) NULL,
	[FactoryID] [varchar](8) NULL,
	[WorkDate] [datetime] NULL,
	[inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[inlineDate] [date] NULL,
	[OfflineDate] [date] NULL,
	[StartHour] [decimal](38,13) NULL,
	[EndHour] [decimal](38,13) NULL,
	[InlineHour] [numeric](17, 6) NULL,
	[OfflineHour] [numeric](17, 6) NULL,
	[StartHourSort] [bigint] NULL,
	[EndHourSort] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [decimal](38,13) NULL,
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
	[Work_Minute] [decimal](38,13) NULL,
	[WorkingTime] [decimal](38,13) NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [decimal](38,13) NULL,
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
	[Sum_Work_Minute] [decimal](38,13) NULL,
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [decimal](38,13) NULL,
	[WorkingTime] [decimal](38,13) NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [decimal](38,13) NULL,
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
	[New_SwitchTime] [decimal](38,13) NULL,
	[Sum_Work_Minute] [decimal](38,13) NULL,
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [decimal](38,13) NULL,
	[WorkingTime] [decimal](38,13) NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [decimal](38,13) NULL,
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
	[New_Work_Minute] [decimal](38,13) NULL,
	[New_SwitchTime]  [decimal](38,13) NULL,
	[Sum_Work_Minute] [decimal](38,13) NULL,
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [decimal](38,13) NULL,
	[WorkingTime] [decimal](38,13) NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [decimal](38,13) NULL,
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
	[New_WorkingTime] [decimal](38,13) NULL,
	[New_SwitchTime] [decimal](38,13) NULL,
	[WorkingTime] [decimal](38,13) NULL,
	[OriWorkDateSer] [bigint] NULL,
	[WorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[CPU] [decimal](38,13) NULL,
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
[New_WorkingTime] = IIF(SwitchTime = 0, WorkingTime,  CONVERT([decimal](38,13), New_Work_Minute)/60) ,
[New_SwitchTime]  = IIF(SwitchTime = 0, 0 , CONVERT([decimal](38,13), New_SwitchTime)/60) ,
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
CREATE TABLE #OriTotalWorkHour(
	[APSNo] [int] NULL,
	[WorkDate] [datetime] NULL,
	[TotalWorkHour] [numeric](38, 13) NULL,
    INDEX IX_APSNo NONCLUSTERED (APSNo, WorkDate)
)
insert into #OriTotalWorkHour
SELECT
awd.APSNo,
awd.WorkDate,
[TotalWorkHour] = sum(OriWorkHour)
FROM @APSExtendWorkDate awd
group by awd.APSNo,awd.WorkDate

--單獨處理getDailystdq (效能)
SELECT s.*
INTO #tmpStdQ
FROM(SELECT DISTINCT awd.APSNo from @APSExtendWorkDate awd)awd
outer apply(select * from dbo.[getDailystdq](awd.APSNo) x) s

--取得LearnCurve Efficiency by Work Date
Declare @APSExtendWorkDateFin table(
	[APSNo] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SewingOutput] [int] NOT NULL,	
	[WorkingTime] [decimal](38,13) NULL,
	[New_WorkingTime] [decimal](38,13) NULL,
	[New_SwitchTime] [decimal](38,13) NULL,
	[LearnCurveEff] [int] NOT NULL,
	[StdOutput] [decimal](38,13) NULL,
	[Std Qty for printing] [decimal](38,13) NULL,
	[CPU] [decimal](38,13) NULL,
	[Efficienycy] [decimal](38,13) NULL,
	[TTL_PRINTING (PCS)] numeric(38,6),
	[TTL_PRINTING PPU (PPU)] numeric(38,6),
	[StdQtyEMB] varchar(50),
	[TtlQtyEMB] int
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
		, [Std Qty for printing] = s.StdQPrint
		, [CPU] = SUM(iif (isnull (otw.TotalWorkHour, 0) = 0 or isnull(awd.CPU,0) = 0 or (awd.New_WorkingTime = 0), 0, awd.New_WorkingTime * awd.HourOutput * OriWorkHour / otw.TotalWorkHour * awd.CPU)) * ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0
        , [Efficienycy] = SUM(iif (isnull (otw.TotalWorkHour, 0) = 0 or (awd.TotalSewingTime = 0) or (awd.New_WorkingTime = 0), 0, awd.New_WorkingTime * awd.HourOutput * OriWorkHour / otw.TotalWorkHour * awd.TotalSewingTime)) * iif(isnull(awd.New_WorkingTime,0) = 0 or isnull(awd.Sewer,0) = 0 ,0 , ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0 / (awd.New_WorkingTime * awd.Sewer * 3600.0))
		, [TTL_PRINTING (PCS)] = round([PRINTING (PCS)] * s.StdQPrint, 6)
		, [TTL_PRINTING PPU (PPU)] = round([PRINTING PPU (PPU)] * s.StdQPrint, 6)
		, [StdQtyEMB] = StdQtyEMB.val
		, [TtlQtyEMB] = TtlQtyEMB.val
from @APSExtendWorkDate awd
inner join #OriTotalWorkHour otw on otw.APSNo = awd.APSNo and otw.WorkDate = awd.WorkDate
left join LearnCurve_Detail lcd with (nolock) on awd.LearnCurveID = lcd.ID and awd.WorkDateSer = lcd.Day
left join #APSSewingOutput apo on awd.APSNo = apo.APSNo and awd.WorkDate = apo.OutputDate
LEFT JOIN #tmpStdQ s ON s.APSNo = awd.APSNo and s.Date = cast(awd.SewingStart as date)
outer apply(select top 1 [val] = Efficiency from LearnCurve_Detail where ID = awd.LearnCurveID order by Day desc ) LastEff
outer apply( 
			select [val] = Stuff((select concat( ',',StdQty) 
								   from (
									  select [StdQty] = sum(StdQty)
									  from (
												select	StyleID,
														[StdQty] =	case when IsLast = 0 then  s.StdQ - LAG(GrandStdQty,1,0) OVER (ORDER BY StyleID, AlloRatio desc)
																else StdQty end,
														EMBROIDERY
												from (	select  sa.StyleID,
																[GrandStdQty] = sum(round(sa.AlloRatio * s.StdQ, 0)) over(order by sa.StyleID, sa.Article, sa.AlloRatio desc),
																[StdQty] = round(sa.AlloRatio * s.StdQ, 0),
																[IsLast] = LEAD(sa.AlloRatio,1,0) over (order by sa.StyleID, sa.Article, sa.AlloRatio desc),
																sa.AlloRatio,
																sa.EMBROIDERY
														from #StyleArtwork sa
														where sa.APSNo = awd.APSNo) StdQtyEMBStep1
											) StdQtyEMBStep2 where EMBROIDERY <> -1 group by StyleID
									) StdQtyFinal
							FOR XML PATH('')),1,1,'') 
			) StdQtyEMB
outer apply(
			select val = sum(StdQty * EMBROIDERY)
			from (
					select [StdQty] =	case when IsLast = 0 then  s.StdQ - LAG(GrandStdQty,1,0) OVER (ORDER BY StyleID, Article, AlloRatio desc)
									else StdQty end,
						   EMBROIDERY
					from (	select  sa.StyleID,
									sa.Article,
									[GrandStdQty] = sum(round(sa.AlloRatio * s.StdQ, 0)) over(order by sa.StyleID, sa.Article, sa.AlloRatio desc),
									[StdQty] = round(sa.AlloRatio * s.StdQ, 0),
									[IsLast] = LEAD(sa.AlloRatio,1,0) over (order by sa.StyleID, sa.Article, sa.AlloRatio desc),
									sa.AlloRatio,
									sa.EMBROIDERY
							from #StyleArtwork sa
							where sa.APSNo = awd.APSNo) StdQtyEMBStep1
				) StdQtyEMBStep2 where EMBROIDERY <> -1 
			) TtlQtyEMB
outer apply(
	select [PRINTING (PCS)] = sum([PRINTING (PCS)]), [PRINTING PPU (PPU)]=sum([PRINTING PPU (PPU)])
	from(
		select tsp.APSNo,tsp.OrderID,
			[PRINTING (PCS)] = tsp.[PRINTING (PCS)] * sr.Rate,
			[PRINTING PPU (PPU)] = tsp.[PRINTING PPU (PPU)] * sr.Rate
		from #tmpPrintSumbuSP tsp
		outer apply(select * from dbo.getSPRatebyAPSNo(tsp.APSNo)x where x.APSNo=awd.APSNo and x.OrderID = tsp.OrderID)sr
		where tsp.APSNo = awd.APSNo
	)x
	group by x.APSNo
)x2
group by awd.APSNo,
		 awd.SewingStart,
		 awd.SewingEnd,
		 isnull(apo.SewingOutput,0),
		 awd.WorkingTime,
		 awd.New_SwitchTime,
		 awd.New_WorkingTime,
		 ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0)),
		 awd.Sewer,
		 s.StdQ,
		 s.StdQPrint,
		 x2.[PRINTING (PCS)],
		 x2.[PRINTING PPU (PPU)],
		 StdQtyEMB.val,
		 TtlQtyEMB.val


declare  @APSResult TABLE(
	APSNo [int] NULL,
	SewingLineID [varchar](5) NULL,
	Sewer int NULL,
	SewingDay date NULL,
	SewingStartTime datetime null,
	SewingEndTime datetime null,
	MDivisionID varchar(8) null,
	FactoryID varchar(10) null,
	PO varchar(max) null,
	POCount int null,
	SP varchar(max) null,
	SP_ComboType varchar(15) null,
	SPCount int null,
	EarliestSCIdelivery date null,
	LatestSCIdelivery date null,
	EarliestBuyerdelivery date null,
	LatestBuyerdelivery date null,
	Category nvarchar(max) null,
	Colorway nvarchar(max) null,
	ColorwayCount bigint null,
	CDCode varchar(max) null,
	CDCodeNew varchar(max) null,
	ProductType nvarchar(max) null,
	MatchFabric varchar(8) null,
	FabricType nvarchar(max) null,
	Lining varchar(max) null,
	Gender varchar(max) null,
	Construction nvarchar(max) null,
	ProductionFamilyID nvarchar(max) null,
	Style nvarchar(max) null,
	StyleCount bigint null,
	OrderQty int null,
	AlloQty int null,
	StardardOutputPerDay int null,
	CPU numeric(12,5) null,
	SewingCPU numeric(12,5) null,
	Orig_WorkHourPerDay numeric(10,5) null,
	New_SwitchTime int null,
	New_WorkHourPerDay  numeric(10,5) null,
	StardardOutputPerHour int null,
	Efficienycy numeric(10,5) null,
	ScheduleEfficiency numeric(10,5) null,
	LineEfficiency numeric(10,5) null,
	LearningCurve numeric(10,5) null,
	SewingInline datetime null,
	SewingOffline datetime null,
	PFRemark varchar(100) null,
	MTLComplete varchar(100) null,
	KPILETA date null,
	MTLETA date null,
	ArtworkType varchar(max) null,
	InspectionDate date null,
	Remarks varchar(max) null,
	FirststCuttingOutputDate date null,
	CuttingOutput numeric(12,2) null,
	SewingOutput int null,
	ScannedQty int null,
	ClogQty int null,
	BrandID varchar(500) null,
    [TTL_PRINTING (PCS)] numeric(38,6) null,
    [TTL_PRINTING PPU (PPU)] numeric(38,6) null,
    SubCon varchar(100) null,
	[Subcon Qty] int null,
	[Std Qty for printing] int null,
	StyleName varchar(max) null,
	StdQtyEMB varchar(50) null,
	EMBStitch varchar(50) null,
	EMBStitchCnt int null,
	TtlQtyEMB int null,
	PrintPcs int null,
	StyleSeason NVARCHAR(MAX),
	StyleUkey NVARCHAR(500),
	[AddDate] [date] NULL,
	[EditDate] [date] NULL
)

--計算這一天的標準產量
--= (工作時數 / 車縫一件成衣需要花費的秒數) * 工人數 * 效率
--= (WorkingTime / SewingTime) * ManPower * Eff
--組合最終table
--delete SewingLineScheduleData
insert into @APSResult(
	APSNo,
	SewingLineID,
	Sewer,
	SewingDay,
	SewingStartTime,
	SewingEndTime,
	MDivisionID,
	FactoryID,
	PO,
	POCount,
	SP,
	SP_ComboType,
	SPCount,
	EarliestSCIdelivery,
	LatestSCIdelivery,
	EarliestBuyerdelivery,
	LatestBuyerdelivery,
	Category,
	Colorway,
	ColorwayCount,
	CDCode,
	CDCodeNew,
	ProductType,
	MatchFabric,
	FabricType,
	Lining,
	Gender,
	Construction,
	ProductionFamilyID,
	Style,
	StyleCount,
	OrderQty,
	AlloQty,
	StardardOutputPerDay,
	CPU,
	SewingCPU,
	Orig_WorkHourPerDay,
	New_SwitchTime,
	New_WorkHourPerDay,
	StardardOutputPerHour,
	Efficienycy,
	ScheduleEfficiency,
	LineEfficiency,
	LearningCurve,
	SewingInline,
	SewingOffline,
	PFRemark,
	MTLComplete,
	KPILETA,
	MTLETA,
	ArtworkType,
	InspectionDate,
	Remarks,
	FirststCuttingOutputDate,
	CuttingOutput,
	SewingOutput,
	ScannedQty,
	ClogQty,
	BrandID,
    [TTL_PRINTING (PCS)],
    [TTL_PRINTING PPU (PPU)],
    SubCon,
	[Subcon Qty],
	[Std Qty for printing],
	StyleName,
	StdQtyEMB,
	EMBStitch,
	EMBStitchCnt,
	TtlQtyEMB,
	PrintPcs,
	StyleSeason,
	StyleUkey,
	AddDate,
	EditDate
)
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
	[SP_Combotype] = apm.SP_Combotype,
	[SPCount]=apm.SpCnt,
	[EarliestSCIdelivery]=apm.MinSCIDelivery,
	[LatestSCIdelivery]=apm.MaxSCIDelivery,
	[EarliestBuyerdelivery]=apm.MinBuyerDelivery,
	[LatestBuyerdelivery]=apm.MaxBuyerDelivery,
	[Category]=apm.Category,
	[Colorway]=apm.Colorway,
	[ColorwayCount]=apm.ColorwayCnt,
	[CDCode]=apm.CDCode,
	[CDCodeNew] = apm.CDCodeNew,
	[ProductType] = apm.ProductType,
	[MatchFabric] = apm.MatchFabric,
	[FabricType] = apm.FabricType,
	[Lining] = apm.Lining,
	[Gender] = apm.Gender,
	[Construction] = apm.Construction,
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
	[BrandID]=apm.BrandID,
    apf.[TTL_PRINTING (PCS)],
    apf.[TTL_PRINTING PPU (PPU)],
    apm.SubCon,
	apm.[Subcon Qty],
	[Std Qty for printing],
	StyleName,
	apf.StdQtyEMB,
	apm.EMBStitch,
	apm.EMBStitchCnt,
	apf.TtlQtyEMB,
	apm.PrintPcs,
	apm.StyleSeason,
	apm.StyleUkey,
	apm.AddDate,
	apm.EditDate
from @APSMain apm
inner join @APSExtendWorkDateFin apf on apm.APSNo = apf.APSNo
order by apm.APSNo,apf.SewingStart


--取得gantt資料
DECLARE @sewinginlineOri DATETIME = @Inline
DECLARE @sewingoffline DATETIME = @Offline

select	@sewinginlineOri = isnull(min(SewingDay), @Inline), 
		@sewingoffline = isnull(max(SewingDay), @Offline)
from @APSResult


declare  @daterange  TABLE(
	[date] date null
)
Declare @sewinginline DATETIME = dateadd(MONTH, -3, @sewinginlineOri) 
--整個月 table
;WITH cte AS (
    SELECT [date] = @sewinginline
    UNION ALL
    SELECT [date] + 1 FROM cte WHERE ([date] <= DATEADD(DAY,-1,@sewingoffline))
)
insert into @daterange([date])
SELECT [date] = cast([date] as date) 
FROM cte
option (maxrecursion 0) -- 突破遞迴100筆資料限制


--WorkHour table
CREATE TABLE #workhourtmp(
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[Date] date null,
	[Hours] numeric(3, 1) null,
	Holiday bit null,
    INDEX IDX_workhourtmp CLUSTERED(Date,FactoryID,SewingLineID)
)

insert into #workhourtmp(FactoryID,SewingLineID,Date,Hours,Holiday)
select FactoryID,SewingLineID,Date,Hours,Holiday
from WorkHour s
where (s.FactoryID = @FactoryID or @FactoryID = '')
and Date between @sewinginline and @sewingoffline
and (s.SewingLineID >= @Line1 or @Line1 = '')
and ( s.SewingLineID <= @Line2 or @Line2 = '')
--order by Date
--準備一整個月workhour的資料判斷Holiday
CREATE TABLE #tmpd (
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[Date] date null,
    INDEX IDX CLUSTERED(Date,FactoryID,SewingLineID)
)


insert into #tmpd(Date, FactoryID, SewingLineID)
select distinct d.date,w.FactoryID,w.SewingLineID
from #workhourtmp w
cross join @daterange d
--

CREATE TABLE #Holiday (
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[Date] date null,
	Holiday bit null,
	INDEX IDX CLUSTERED(Date,FactoryID,SewingLineID,Holiday)
)

insert into #Holiday(FactoryID, SewingLineID, [Date], Holiday)
select distinct d.FactoryID,d.SewingLineID,d.date,Holiday = iif(w.Holiday is null or w.Holiday = 1 or w.Hours = 0,1,0)
from #tmpd d
left join #workhourtmp w on d.date = w.Date and d.FactoryID = w.FactoryID and d.SewingLineID = w.SewingLineID
order by FactoryID,SewingLineID,date

--先將符合條件的Sewing schedule撈出來
declare  @Sewtmp  TABLE(
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	StyleID varchar(15) null,
	Inline date null,
	Offline date null,
	InlineHour numeric(20, 7) null,
	OfflineHour numeric(20, 7) null,
	OrderTypeID varchar(20) null,
	SciDelivery date null,
	BuyerDelivery date null,
	Category varchar(1) null,
	CdCodeID varchar(5) null,
	APSNo int null,
	CPU numeric(12,5) null,
	HourOutput numeric(20, 7) null,
	OriWorkHour numeric(20, 7) null,
	TotalSewingTime int null,
	LearnCurveID int null,
	LNCSERIALNumber int null,
	OrderID varchar(13) null,
	ComboType varchar(1) null
)

insert into @Sewtmp(FactoryID, SewingLineID, StyleID, Inline, Offline, InlineHour, OfflineHour, OrderTypeID, SciDelivery, BuyerDelivery, Category, CdCodeID, APSNo,
					CPU, HourOutput, OriWorkHour,TotalSewingTime, LearnCurveID, LNCSERIALNumber, OrderID, ComboType)
select
	 s.FactoryID
	,s.SewingLineID
	,o.StyleID
	,Inline = cast(s.Inline as date)
	,Offline = cast(s.Offline as date)
	,[InlineHour] = DATEDIFF(ss,Cast(s.Inline as date),s.Inline) / 3600.0
    ,[OfflineHour] = DATEDIFF(ss,Cast(s.Offline as date),s.Offline) / 3600.0
	,OrderTypeID = isnull(o.OrderTypeID,'')
	,o.SciDelivery
	,o.BuyerDelivery
	,Category = isnull(o.Category,'')
	,[CdCodeID] = st.CDCodeNew
	,s.APSNo
	,[CPU] = cast(o.CPU * o.CPUFactor * isnull(dbo.GetOrderLocation_Rate(s.OrderID,s.ComboType),isnull(dbo.GetStyleLocation_Rate(o.StyleUkey,s.ComboType),100)) / 100 as [decimal](38,13))
	,[HourOutput] = iif(isnull(s.TotalSewingTime,0) = 0,0,(s.Sewer * 3600.0 * ScheduleEff.val / 100) / s.TotalSewingTime)
	,[OriWorkHour] = iif (isnull(s.Sewer,0) = 0 or isnull(s.TotalSewingTime,0)=0 or isnull(ScheduleEff.val,0)=0 , 0, sum(s.AlloQty) / ((s.Sewer * 3600.0 * ScheduleEff.val / 100) / s.TotalSewingTime))
	,s.TotalSewingTime
	,s.LearnCurveID
	,s.LNCSERIALNumber
	,[OrderID] = o.ID
	,s.ComboType
from SewingSchedule s WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on s.OrderID = o.ID and exists (select 1 from Factory where o.FactoryId = id and IsProduceFty = 1)
left join Style st WITH (NOLOCK) on st.Ukey = o.StyleUkey
outer apply(select [val] = iif(isnull(s.OriEff,0) = 0 or isnull(s.SewLineEff,0) = 0,s.MaxEff, isnull(s.OriEff,100) * isnull(s.SewLineEff,100) / 100) ) ScheduleEff
where (
       CONVERT(date, s.Inline)  between @sewinginline and @sewingoffline 
    or CONVERT(date, s.Offline) between @sewinginline and @sewingoffline
	or @sewinginline  between CONVERT(date, s.Inline) and CONVERT(date, s.Offline)
	or @sewingoffline between CONVERT(date, s.Inline) and CONVERT(date, s.Offline)
)
and (s.MDivisionID = @MDivisionID or @MDivisionID = '')
and (s.FactoryID = @FactoryID or @FactoryID = '')
and (s.SewingLineID >= @Line1 or @Line1 = '')
and (s.SewingLineID <= @Line2 or @Line2 = '')
and (o.BuyerDelivery >= @BuyerDelivery1 or @BuyerDelivery1 is null)
and (o.BuyerDelivery <= @BuyerDelivery2 or @BuyerDelivery2 is null)
and (o.SciDelivery >= @SciDelivery1 or @SciDelivery1 is null)
and (o.SciDelivery <= @SciDelivery2 or @SciDelivery2 is null)
and (o.BrandID <= @Brand or @Brand = '')
and (	exists(select 1 from Style_TmsCost st where o.StyleUkey = st.StyleUkey and st.ArtworkTypeID = @subprocess AND (st.Qty>0 or st.TMS>0 and st.Price>0) ) or
		@subprocess = '')
group by  s.FactoryID,s.SewingLineID,o.StyleID,s.Inline,s.Offline
	,o.OrderTypeID
	,o.SciDelivery
	,o.BuyerDelivery
	,o.Category
	,st.CDCodeNew
	,s.APSNo
	,o.CPU,o.CPUFactor
	,s.Sewer
	,ScheduleEff.val
	,s.TotalSewingTime
	,s.LearnCurveID
	,s.LNCSERIALNumber
	,s.OrderID
	,s.ComboType
	,o.StyleUkey
	,o.ID

-- print '@Stmp'  + FORMAT(getdate(), 'hh:mm:ss')
declare  @Stmp  TABLE(
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[date] date null,
	StyleID varchar(15) null,
	Inline date null,
	Offline date null,
	InlineHour numeric(20, 7) null,
	OfflineHour numeric(20, 7) null,
	IsLastMonth int null,
	IsNextMonth int null,
	IsBulk int null,
	IsSMS int null,
	IsSample int null,
	BuyerDelivery date null,
	CdCodeID varchar(5) null,
	APSNo int null,
	StartHour [decimal](38,13) null,
	EndHour [decimal](38,13) null,
	CPU numeric(12,5) null,
	OriWorkHour numeric(20, 7) null,
	HourOutput numeric(20, 7) null,
	TotalSewingTime int null,
	LearnCurveID int null,
	LNCSERIALNumber int null,
	OrderID varchar(13) null,
	ComboType varchar(1) null
)

insert into @Stmp(FactoryID, SewingLineID, date, StyleID, Inline, Offline, InlineHour, OfflineHour, IsLastMonth, IsNextMonth, IsBulk, IsSMS,
					IsSample, BuyerDelivery, CdCodeID, APSNo, StartHour, EndHour, CPU, OriWorkHour, HourOutput, TotalSewingTime, LearnCurveID, LNCSERIALNumber,
					OrderID, ComboType)
select distinct
	d.FactoryID
	,d.SewingLineID
	,d.date
	,s.StyleID
	,s.Inline
	,s.Offline
	,s.InlineHour
	,s.OfflineHour
	,IsLastMonth = iif(s.SciDelivery < @sewinginline,1,0)--紫 優先度1
	,IsNextMonth = iif(s.SciDelivery > DateAdd(DAY,-1,@sewingoffline),1,0)--綠 優先度2
	,IsBulk = iif(s.Category = 'B',1,0)--藍 優先度3
	,IsSMS = iif(s.OrderTypeID = 'SMS',1,0)--紅 優先度4
    ,IsSample = iif(s.Category = 'S',1,0)
	,s.BuyerDelivery
	,s.CdCodeID
	,s.APSNo
	,StartHour = CAST(wkd.StartHour as [decimal](38,13))
	,EndHour = CAST(wkd.EndHour as [decimal](38,13))
	,s.CPU
	,s.OriWorkHour
	,s.HourOutput
	,s.TotalSewingTime
	,s.LearnCurveID
	,s.LNCSERIALNumber
	,s.OrderID
	,s.ComboType
from @Sewtmp s 
left join #tmpd d on d.FactoryID = s.FactoryID and d.SewingLineID = s.SewingLineID and d.date between s.Inline and s.Offline
left join Workhour_Detail wkd with(nolock) on wkd.FactoryID = s.FactoryID
										and wkd.SewingLineID = s.SewingLineID and wkd.Date = d.date
-- print '@tmpStmp_step1'  + FORMAT(getdate(), 'hh:mm:ss')
declare  @tmpStmp_step1  TABLE(
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[date] date null,
	StyleID varchar(15) null,
	Inline date null,
	Offline date null,
	InlineHour numeric(20, 7) null,
	OfflineHour numeric(20, 7) null,
	IsLastMonth int null,
	IsNextMonth int null,
	IsBulk int null,
	IsSMS int null,
	IsSample int null,
	BuyerDelivery date null,
	CdCodeID varchar(5) null,
	APSNo int null,
	StartHour [decimal](38,13) null,
	EndHour [decimal](38,13) null,
	CPU numeric(12,5) null,
	OriWorkHour numeric(20, 7) null,
	HourOutput numeric(20, 7) null,
	TotalSewingTime int null,
	LearnCurveID int null,
	LNCSERIALNumber int null,
	WorkingTime [decimal](38,13) null,
	OriWorkDateSer int null
)

insert into @tmpStmp_step1(FactoryID, SewingLineID, date, StyleID, Inline, Offline, InlineHour, OfflineHour, IsLastMonth, IsNextMonth, IsBulk, IsSMS,
					IsSample, BuyerDelivery, CdCodeID, APSNo, StartHour, EndHour, CPU, OriWorkHour, HourOutput, TotalSewingTime, LearnCurveID, LNCSERIALNumber,
					WorkingTime, OriWorkDateSer)
select FactoryID
, SewingLineID
,date
,StyleID
,Inline
,Offline
,InlineHour
,OfflineHour
,IsLastMonth
,IsNextMonth
,IsBulk
,IsSMS
,IsSample
,BuyerDelivery
,CdCodeID
,APSNo
,StartHour
,EndHour
,CPU
,OriWorkHour
,HourOutput
,TotalSewingTime
,LearnCurveID
,LNCSERIALNumber
,[WorkingTime] = sum(EndHour - StartHour)
,[OriWorkDateSer] = ROW_NUMBER() OVER (PARTITION BY APSNo,OrderID,ComboType ORDER BY date)
from @Stmp
group by FactoryID, SewingLineID,date,StyleID,IsLastMonth,IsNextMonth,IsBulk
,IsSMS,BuyerDelivery,CdCodeID,APSNo,StartHour,EndHour,CPU,OriWorkHour,HourOutput,TotalSewingTime
,LearnCurveID,LNCSERIALNumber,OrderID,ComboType,Inline,Offline,InlineHour,OfflineHour,IsSample

--抓出各style連續作業天數，不含假日
-- print '@tmpLongDayCheck1'  + FORMAT(getdate(), 'hh:mm:ss')
declare  @tmpLongDayCheck1  TABLE(
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[date] date null,
	StyleID varchar(15) null
)

insert into @tmpLongDayCheck1(FactoryID, SewingLineID, date, StyleID)
select distinct FactoryID, SewingLineID, date, StyleID from @Stmp where FactoryID is not null

-- print '@tmpLongDayCheck2'  + FORMAT(getdate(), 'hh:mm:ss')
declare  @tmpLongDayCheck2  TABLE(
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[date] date null,
	StyleID varchar(15) null,
	isBegin bit null,
	isEnd bit null
)

insert into @tmpLongDayCheck2(FactoryID, SewingLineID, date, StyleID, isBegin, isEnd)
select	FactoryID,
		SewingLineID,
		[date],
		StyleID,
		isBegin = iif(lag(date) over (order by factoryid, sewinglineid, styleID, date) = dateadd(day, -1, date), 0, 1),
		isEnd =  iif(lead(date) over (order by factoryid, sewinglineid, styleID, date) = dateadd(day, 1, date), 0, 1)
from @tmpLongDayCheck1
order by factoryid, sewinglineid, styleID, date

-- print '@tmpLongDayCheck3'  + FORMAT(getdate(), 'hh:mm:ss')
declare  @tmpLongDayCheck3  TABLE(
	rownum int null,
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	StyleID varchar(15) null,
	BeginDate date null,
	EndDate date null
)

insert into @tmpLongDayCheck3(rownum, factoryid, sewinglineid, styleID, BeginDate, EndDate)
select rownum, factoryid, sewinglineid, styleID, BeginDate = min(date), EndDate = max(date)
from (
select *, rownum = ROW_NUMBER() over (order by factoryid, sewinglineid, styleID, date) from @tmpLongDayCheck2 where isBegin = 1 
union all
select *, rownum = ROW_NUMBER() over (order by factoryid, sewinglineid, styleID, date) from @tmpLongDayCheck2 where isEnd = 1
) a 
group by rownum, factoryid, sewinglineid, styleID

-- print '#tmpLongDayCheck'  + FORMAT(getdate(), 'hh:mm:ss')
CREATE TABLE #tmpLongDayCheck (
	rownum int null,
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	StyleID varchar(15) null,
	BeginDate date null,
	EndDate date null,
	WorkDays int null,
	INDEX IDX CLUSTERED(StyleID,FactoryID,SewingLineID,BeginDate,EndDate)
)

insert into #tmpLongDayCheck(rownum, factoryid, sewinglineid, styleID, BeginDate, EndDate, WorkDays)
select t.rownum, t.factoryid, t.sewinglineid, t.styleID, t.BeginDate, t.EndDate, [WorkDays] = DATEDIFF(day, BeginDate, EndDate) + 1 - holiday.val
from @tmpLongDayCheck3 t
outer apply(select val = count(*) from #Holiday where Holiday = 1 and FactoryID = t.FactoryID and SewingLineID = t.SewingLineID and date between BeginDate and EndDate) holiday

--


--刪除每個計畫inline,offline當天超過時間的班表                                                
delete @tmpStmp_step1 where [date] = Inline and EndHour <= InlineHour
delete @tmpStmp_step1 where [date] = Offline and StartHour >= OfflineHour

-- print '#tmpStmp_step2'  + FORMAT(getdate(), 'hh:mm:ss')
CREATE TABLE #tmpStmp_step2 (
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[date] date null,
	StyleID varchar(15) null,
	IsLastMonth int null,
	IsNextMonth int null,
	IsBulk int null,
	IsSample int null,
	IsSMS int null,
	BuyerDelivery date null,
	CdCodeID varchar(5) null,
	APSNo int null,
	WorkingTime [decimal](38,13) null,
	CPU numeric(12,5) null,
	OriWorkHour numeric(20, 7) null,
	HourOutput numeric(20, 7) null,
	OriWorkDateSer int null,
	WorkDateSer int null,
	TotalSewingTime int null,
	LearnCurveID int null,
	LNCSERIALNumber int null,
	INDEX IDX CLUSTERED(Date,FactoryID,SewingLineID)
)

insert into #tmpStmp_step2(FactoryID
, SewingLineID
,date
,StyleID
,IsLastMonth
,IsNextMonth
,IsBulk
,IsSample
,IsSMS
,BuyerDelivery
,CdCodeID
,APSNo
,WorkingTime
,CPU
,OriWorkHour
,HourOutput
,OriWorkDateSer
,WorkDateSer
,TotalSewingTime
,LearnCurveID
,LNCSERIALNumber)
select FactoryID
, SewingLineID
,date
,StyleID
,IsLastMonth
,IsNextMonth
,IsBulk
,IsSample
,IsSMS
,BuyerDelivery
,CdCodeID
,APSNo
,WorkingTime
,CPU
,OriWorkHour
,HourOutput
,OriWorkDateSer
,[WorkDateSer] = case	when isnull(LNCSERIALNumber,0) = 0 then OriWorkDateSer
						when LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) <= 0 then OriWorkDateSer
						else OriWorkDateSer + LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) end

,TotalSewingTime
,LearnCurveID
,LNCSERIALNumber
from @tmpStmp_step1

-- print '#ConcatStyle'  + FORMAT(getdate(), 'hh:mm:ss')
CREATE TABLE #ConcatStyle(
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[date] date null,
	StyleID VARCHAR(MAX) null,
	INDEX IDX CLUSTERED(Date,FactoryID,SewingLineID)
)

insert into #ConcatStyle(FactoryID,SewingLineID,date,StyleID)
select distinct FactoryID,SewingLineID,date,StyleID = a.s
from #tmpStmp_step2 s
outer apply(
	select s =(
		select distinct concat(StyleID,'(',CdCodeID,')',';')
		from #tmpStmp_step2 s2
		where s2.FactoryID = s.FactoryID and s2.SewingLineID = s.SewingLineID and s2.date = s.date
		for xml path('')
	)
)a

--
-- print '@c'  + FORMAT(getdate(), 'hh:mm:ss')
declare  @c  TABLE(
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[date] date null,
	Holiday bit null,
	IsLastMonth int null,
	IsNextMonth int null,
	IsBulk int null,
	IsSample int null,
	IsSMS int null,
	BuyerDelivery date null,
	APSNo int null,
	WorkingTime [decimal](38,13) null,
	CPU numeric(12,5) null,
	TotalSewingTime int null,
	LearnCurveID int null,
	WorkDateSer int null,
	OriStyle varchar(100) null,
	IsRepeatStyle bit null
)

insert into @c(FactoryID, SewingLineID, [date], Holiday, IsLastMonth, IsNextMonth, IsBulk, IsSample, IsSMS, BuyerDelivery, APSNo, WorkingTime,
				CPU, TotalSewingTime, LearnCurveID, WorkDateSer, OriStyle, IsRepeatStyle)
select h.FactoryID,h.SewingLineID,h.date,h.Holiday,IsLastMonth,IsNextMonth,IsBulk,IsSample,IsSMS,BuyerDelivery
,s.APSNo
,WorkingTime
,s.CPU,s.TotalSewingTime
,s.LearnCurveID
,s.WorkDateSer
,[OriStyle] = s.StyleID
,[IsRepeatStyle] = iif(DENSE_RANK() OVER (PARTITION BY s.FactoryID, s.SewingLineID, s.StyleID, h.Holiday ORDER BY s.date) > 10, 1, 0)
from #Holiday h
left join #tmpStmp_step2 s on s.FactoryID = h.FactoryID and s.SewingLineID = h.SewingLineID and s.date = h.date
inner join(select distinct FactoryID,SewingLineID from #tmpStmp_step2) x on x.FactoryID = h.FactoryID and x.SewingLineID = h.SewingLineID--排掉沒有在SewingSchedule內的資料by FactoryID,SewingLineID
order by h.FactoryID,h.SewingLineID,h.date ASC, IsSample DESC

-- print '#tmpTotalWT'  + FORMAT(getdate(), 'hh:mm:ss')
CREATE TABLE #tmpTotalWT(
	FactoryID varchar(10) null,
	SewingLineID varchar(10) null,
	[date] date null,
	APSNo int null,
	TotalSewingTime int null,
	INDEX IDX CLUSTERED(APSNo,Date,FactoryID,SewingLineID)
)

insert into #tmpTotalWT(FactoryID, SewingLineID, [date], APSNo, TotalSewingTime)
select FactoryID,SewingLineID,date,APSNo
,[Total_WorkingTime] = sum(OriWorkHour)
from @Stmp
group by  FactoryID,SewingLineID,date,APSNo

------------------------------------------------------------------------------------------------
DECLARE cursor_sewingschedule CURSOR FOR
select  c.FactoryID
        ,c.SewingLineID
        ,c.date
	    ,StyleID = isnull(iif(c.Holiday = 1,'Holiday', cs.StyleID),'')
	    ,[IsLastMonth] = max(IsLastMonth)
		,[IsNextMonth] = max(IsNextMonth)
		,[IsBulk] = max(IsBulk)
		,IsSMS
        ,[BuyerDelivery] = min(BuyerDelivery)
	    ,StadOutPutQtyPerDay = sum(s.StdQ)
	    ,PPH = sum(iif(isnull(c.TotalSewingTime,0)=0 or isnull(s.StdQ,0)=0,0,c.CPU / c.TotalSewingTime * s.StdQ))	
        ,[IsSample] = max(IsSample)
	    ,c.IsRepeatStyle
	    ,c.OriStyle
from @c c 
left join #ConcatStyle cs on c.FactoryID = cs.FactoryID and c.SewingLineID = cs.SewingLineID and c.date = cs.date
left join #tmpTotalWT twt on twt.APSNo = c.APSNo and twt.SewingLineID = c.SewingLineID and twt.FactoryID = c.FactoryID
and twt.date = c.date
left join LearnCurve_Detail lcd with (nolock) on c.LearnCurveID = lcd.ID and c.WorkDateSer = lcd.Day
outer  apply(select * from dbo.[getDailystdq](c.APSNo)x where x.APSNo=c.APSNo and x.Date = cast(c.date as date)
			)s
where c.date >= @sewinginlineOri
group by c.FactoryID,c.SewingLineID,c.date,c.Holiday,cs.StyleID,IsSMS,c.IsRepeatStyle,c.OriStyle
order by c.FactoryID,c.SewingLineID,c.date ASC, IsSample DESC

--建立tmpe table存放最後要列印的資料
DECLARE @tempPintData TABLE (
   FactoryID VARCHAR(8),
   SewingLineID VARCHAR(5),
   StyleID VARCHAR(MAX),
   InLine DATE,
   OffLine DATE,
   IsBulk BIT,
   IsSample BIT,
   IsSMS BIT,
   IsLastMonth BIT,
   IsNextMonth BIT,
   MinBuyerDelivery DATE,
   IsFirst BIT default 0,
   StadOutPutQtyPerDay int,
   PPH numeric(12,4),
   IsRepeatStyle BIT,
   IsSimilarStyle BIT default 0,
   IsCrossStyle BIT default 0
)

--判斷相似款所使用暫存table
DECLARE @tempSimilarStyle TABLE (
   ScheduleDate date,
   MasterStyleID VARCHAR(15),
   ChildrenStyleID VARCHAR(15)
)
--
DECLARE @factory VARCHAR(8),
		@sewingline VARCHAR(5),
		@StyleID VARCHAR(200),
		@IsLastMonth int,
		@IsNextMonth int,
		@IsBulk int,
		@IsSample int,
		@IsSMS int,
		@BuyerDelivery DATE,
		@StadOutPutQtyPerDay int,
		@PPH numeric(12,4),
		@date DATE,
		@beforefactory VARCHAR(8) = '',
		@beforesewingline VARCHAR(5) = '',
		@beforeStyleID VARCHAR(200) = '',
		@beforeStyleIDExcludeHoliday VARCHAR(200) = '',
		@beforeIsLastMonth int,
		@beforeIsNextMonth int,
		@beforeIsBulk int,
		@beforeIsSMS int,
		@beforeBuyerDelivery DATE,
		@beforedate DATE,
		@IsFirst bit = 0,
		@beforeIsSample bit = 1,
		@IsRepeatStyle bit = 1,
		@beforeIsRepeatStyle bit = 1,
		@OriStyle varchar(15) = ''

OPEN cursor_sewingschedule
FETCH NEXT FROM cursor_sewingschedule INTO @factory,@sewingline,@date,@StyleID,@IsLastMonth,@IsNextMonth,@IsBulk,@IsSMS,@BuyerDelivery,@StadOutPutQtyPerDay,@PPH,@IsSample,@IsRepeatStyle,@OriStyle
WHILE @@FETCH_STATUS = 0
BEGIN
	if(@sewingline <> @beforesewingline)
		delete @tempSimilarStyle

	IF @factory <> @beforefactory or @sewingline <> @beforesewingline or @StyleID <> @beforeStyleID
	Begin
		if(@beforeStyleIDExcludeHoliday not like '%' + @StyleID + '%' and @sewingline = @beforesewingline)
            set @IsFirst = 1
        else
			set @IsFirst = 0

		INSERT INTO @tempPintData(FactoryID, SewingLineID, StyleID, InLine, OffLine, IsLastMonth, IsNextMonth, IsBulk, IsSMS, MinBuyerDelivery, StadOutPutQtyPerDay, PPH, IsSample, IsRepeatStyle, IsFirst) 
		VALUES (@factory, @sewingline, @StyleID, @date, @date, @IsLastMonth, @IsNextMonth, @IsBulk, @IsSMS, @BuyerDelivery, @StadOutPutQtyPerDay, @PPH, @IsSample, @IsRepeatStyle, @IsFirst);
	END
	ELSE IF @IsFirst = 1 and @date <> @beforedate and @StyleID <> 'Holiday' and @IsSample <> 1
	Begin
        set @IsFirst = 0
		INSERT INTO @tempPintData(FactoryID, SewingLineID, StyleID, InLine, OffLine, IsLastMonth, IsNextMonth, IsBulk, IsSMS, MinBuyerDelivery, StadOutPutQtyPerDay, PPH, IsSample, IsRepeatStyle) 
		VALUES (@factory, @sewingline, @StyleID, @date, @date, @IsLastMonth, @IsNextMonth, @IsBulk, @IsSMS, @BuyerDelivery, @StadOutPutQtyPerDay, @PPH, @IsSample, @IsRepeatStyle);
	END
	--有含Sample獨立顯示 or 三個月內生產超過10天
	else if (@IsSample <> @beforeIsSample or @beforeStyleID = 'Holiday' or (@IsRepeatStyle <> @beforeIsRepeatStyle and @IsSample <> 1)) and @date <> @beforedate and @StyleID <> 'Holiday'
	begin
		INSERT INTO @tempPintData(FactoryID, SewingLineID, StyleID, InLine, OffLine, IsLastMonth, IsNextMonth, IsBulk, IsSMS, MinBuyerDelivery, StadOutPutQtyPerDay, PPH, IsSample, IsRepeatStyle) 
		VALUES (@factory, @sewingline, @StyleID, @date, @date, @IsLastMonth, @IsNextMonth, @IsBulk, @IsSMS, @BuyerDelivery, @StadOutPutQtyPerDay, @PPH, @IsSample, @IsRepeatStyle);
	end
	ELSE
	Begin
		update @tempPintData set
			 OffLine = @date
			,IsLastMonth = iif(IsLastMonth = 1, IsLastMonth, @IsLastMonth)
			,IsNextMonth = iif(IsNextMonth = 1, IsNextMonth, @IsNextMonth)
			,IsBulk = iif(IsBulk = 1, IsBulk, @IsBulk)
			,IsSMS = iif(IsSMS = 1, IsSMS, @IsSMS)
			,MinBuyerDelivery = iif(MinBuyerDelivery < @BuyerDelivery,MinBuyerDelivery,@BuyerDelivery)
		where FactoryID = @factory and SewingLineID = @sewingline and StyleID = @StyleID and OffLine = @beforedate
	END
	
    if	(select count(*) from SplitString(@StyleID,';') where data <> '') > 1 and 
		@StyleID <> 'Holiday'
	begin
		--只保留當天與前一天的SimilarStyle資料
		delete @tempSimilarStyle 
		where	ScheduleDate <> (select top 1 ScheduleDate from @tempSimilarStyle where ScheduleDate <> @date order by ScheduleDate desc) and
				ScheduleDate <> @date
		
		if exists(select 1 from @tempSimilarStyle where ChildrenStyleID = @OriStyle)
			update @tempPintData set IsSimilarStyle = 1, IsCrossStyle = 1 where FactoryID = @factory and SewingLineID = @sewingline and StyleID = @StyleID and OffLine = @date
		else
			update @tempPintData set IsCrossStyle = 1 where FactoryID = @factory and SewingLineID = @sewingline and StyleID = @StyleID and OffLine = @date

		if not exists(select 1 from @tempSimilarStyle where ScheduleDate = @date and MasterStyleID = @OriStyle)
		insert into @tempSimilarStyle(ScheduleDate, MasterStyleID, ChildrenStyleID)
			select @date, MasterStyleID, ChildrenStyleID
			from Style_SimilarStyle where MasterStyleID = @OriStyle
	end

	set @beforefactory = @factory
	set @beforesewingline = @sewingline
	set @beforeStyleID = @StyleID
	set @beforeIsLastMonth = @IsLastMonth
	set @beforeIsNextMonth = @IsNextMonth
	set @beforeIsBulk = @IsBulk
	set @beforeIsSMS = @IsSMS
	set @beforeBuyerDelivery = @BuyerDelivery
	set @beforedate = @date
	set @beforeIsSample = @IsSample
	set @beforeIsRepeatStyle = @IsRepeatStyle
	set @beforeStyleIDExcludeHoliday = iif(@StyleID = 'Holiday', @beforeStyleIDExcludeHoliday, @StyleID)
	FETCH NEXT FROM cursor_sewingschedule INTO @factory,@sewingline,@date,@StyleID,@IsLastMonth,@IsNextMonth,@IsBulk,@IsSMS,@BuyerDelivery,@StadOutPutQtyPerDay,@PPH,@IsSample,@IsRepeatStyle,@OriStyle
END
CLOSE cursor_sewingschedule
DEALLOCATE cursor_sewingschedule

-- print '#tmpGantt'  + FORMAT(getdate(), 'hh:mm:ss')
CREATE TABLE #tmpGantt (
   FactoryID VARCHAR(8),
   SewingLineID VARCHAR(5),
   StyleID VARCHAR(MAX),
   InLine DATE,
   OffLine DATE,
   IsBulk BIT,
   IsSample BIT,
   IsSMS BIT,
   IsLastMonth BIT,
   IsNextMonth BIT,
   MinBuyerDelivery DATE,
   IsFirst BIT default 0,
   StadOutPutQtyPerDay int,
   PPH numeric(12,4),
   IsRepeatStyle BIT,
   IsSimilarStyle BIT default 0,
   IsCrossStyle BIT default 0,
   WorkDays int null,
   INDEX IDX CLUSTERED(FactoryID,SewingLineID,InLine,OffLine)
)

insert into #tmpGantt(
		FactoryID,
		SewingLineID,
		StyleID,
		InLine,
		OffLine,
		IsBulk,
		IsSample,
		IsSMS,
		IsLastMonth,
		IsNextMonth,
		MinBuyerDelivery,
		IsFirst,
		StadOutPutQtyPerDay,
		PPH,
		IsRepeatStyle,
		IsSimilarStyle,
		IsCrossStyle,
        WorkDays
	)
select  t.FactoryID,
		t.SewingLineID,
		t.StyleID,
		t.InLine,
		t.OffLine,
		t.IsBulk,
		t.IsSample,
		t.IsSMS,
		t.IsLastMonth,
		t.IsNextMonth,
		t.MinBuyerDelivery,
		t.IsFirst,
		t.StadOutPutQtyPerDay,
		t.PPH,
		t.IsRepeatStyle,
		t.IsSimilarStyle,
		t.IsCrossStyle,
        [WorkDays] = isnull(workDays.val, 0)
from @tempPintData t
outer apply (   select val = max(WorkDays) 
                from #tmpLongDayCheck tdc
                where   tdc.FactoryID = t.FactoryID and
                        tdc.SewingLineID = t.SewingLineID and
                        t.StyleID like '%' + tdc.StyleID + '%'  and
                        t.InLine between tdc.BeginDate and tdc.EndDate
            ) workDays
where t.StyleID <> ''

-- use Value Function [GetCheckContinusProduceDays]

Create Table #tmpDistinct 
(
	[StyleUkey] [nvarchar](500) NULL,
	[SewingLineID] [varchar](5) NULL,
	[FactoryID] [varchar](8) NULL,
	[SewingDay] date,
	[Category] varchar(10)
)
insert into #tmpDistinct
select distinct StyleUkey,SewingLineID,FactoryID,SewingDay, Category from @APSResult

Create Table #tmpProduceDays
(
	[StyleUkey]  [nvarchar](500) NULL,
	[SewingLineID] [varchar](5) NULL,
	[FactoryID] [varchar](8) NULL,
	[SewingDay] date,
	[Category] varchar(8) null,
	[SewingInlineCategory] varchar(50) null,
)
insert into #tmpProduceDays
select StyleUkey, SewingLineID, FactoryID, SewingDay,t.Category
	, SewingInlineCategory = ''
from #tmpDistinct t
CROSS APPLY (
SELECT 
	ContinuousDays = Production.dbo.GetCheckContinusProduceDays(t.StyleUkey, t.SewingLineID, t.FactoryID,null, t.SewingDay)
) ContinuousDaysCalc
where t.StyleUkey not like '%,%'

union all

select StyleUkey, SewingLineID, FactoryID, SewingDay,t.Category
	, SewingInlineCategory = ''
from #tmpDistinct t
where t.StyleUkey like '%,%'


--R01 detail and BI
-- print 'Final' + FORMAT(getdate(), 'hh:mm:ss') 
select distinct
	apm.APSNo,
	apm.SewingLineID,
	apm.Sewer,
	apm.SewingDay,
	apm.SewingStartTime,
	apm.SewingEndTime,
	apm.MDivisionID,
	apm.FactoryID,
	apm.PO,
	apm.POCount,
	apm.SP,
	apm.SP_ComboType,
	apm.SPCount,
	apm.EarliestSCIdelivery,
	apm.LatestSCIdelivery,
	apm.EarliestBuyerdelivery,
	apm.LatestBuyerdelivery,
	apm.Category,
	apm.Colorway,
	apm.ColorwayCount,
	apm.CDCode,
	apm.CDCodeNew,
	apm.ProductType,
	apm.MatchFabric,
	apm.FabricType,
	apm.Lining,
	apm.Gender,
	apm.Construction,
	apm.ProductionFamilyID,
	apm.Style,
	apm.StyleCount,
	apm.OrderQty,
	apm.AlloQty,
	apm.StardardOutputPerDay,
	apm.CPU,
	apm.SewingCPU,
	[Total Sewing CPU] = apm.StardardOutputPerDay * apm.SewingCPU,
	apm.Orig_WorkHourPerDay,
	apm.New_SwitchTime,
	apm.New_WorkHourPerDay,
	[Sewer Working hrs] = apm.Sewer * apm.New_WorkHourPerDay,
	apm.StardardOutputPerHour,
	apm.Efficienycy,
	apm.ScheduleEfficiency,
	apm.LineEfficiency,
	apm.LearningCurve,
	apm.SewingInline,
	apm.SewingOffline,
	apm.PFRemark,
	apm.MTLComplete,
	apm.KPILETA,
	apm.MTLETA,
	apm.ArtworkType,
	apm.InspectionDate,
	apm.Remarks,
	apm.FirststCuttingOutputDate,
	apm.CuttingOutput,
	apm.SewingOutput,
	apm.ScannedQty,
	apm.ClogQty,
	apm.BrandID,
    apm.[TTL_PRINTING (PCS)],
    apm.[TTL_PRINTING PPU (PPU)],
    apm.SubCon,
	apm.[Subcon Qty],
	apm.[Std Qty for printing],
	apm.StyleName,
	apm.StdQtyEMB,
	apm.EMBStitch,
	apm.EMBStitchCnt,
	apm.TtlQtyEMB,
	apm.PrintPcs,
	[InlineCategory] = case when tg.IsSample = 1 then 'Sample'
							when tg.IsCrossStyle = 1 and tg.IsSimilarStyle = 1 then 'Changeover for similar style'
							when tg.IsCrossStyle = 1 then  'Changeover day or 1st day inline for the style'
							when tg.IsRepeatStyle = 1 then 'Repeat'
							else 'New style' end,
	apm.StyleSeason,
	apm.AddDate,
	apm.EditDate,
    factory.LastDownloadAPSDate,
	[SewingInlineCategory] = ''
from @APSResult apm
left join #tmpGantt tg on tg.FactoryID = apm.FactoryID and tg.SewingLineID = apm.SewingLineID and cast(apm.SewingDay as date) between tg.InLine and tg.OffLine
left join #tmpProduceDays pd on pd.StyleUkey = apm.StyleUkey and pd.FactoryID = apm.FactoryID and pd.SewingDay = apm.SewingDay and pd.SewingLineID = apm.SewingLineID and pd.Category = apm.Category
outer apply (select fac.LastDownloadAPSDate from factory fac where fac.id = (select f.KPICode from factory f where f.id = apm.FactoryID)) factory
order by apm.APSNo,apm.SewingStartTime

--PPIC.R01用
if(@Inline is not null and @Offline is not null)
begin

delete #tmpGantt where InLine > @Offline or OffLine < @Inline
update #tmpGantt set	InLine = iif(InLine < @Inline, @Inline, InLine),
						OffLine = iif(OffLine > @Offline, @Offline, OffLine)
end

select * from #tmpGantt

DROP TABLE 
    #APSColumnGroup
    ,#APSCuttingOutput
    ,#APSList
    ,#APSListArticle
    ,#APSOrderQty
    ,#APSPackingQty
    ,#APSRemarks
    ,#StyleArtwork
    ,#StyleData
    ,#StyleDatabyAPSNo
    ,#tmpPrintDataSum
    ,#tmpPrintData
    ,#tmpPrintSumbuSP
    ,#OriTotalWorkHour
    ,#APSSewingOutput
    ,#tmpStdQ
    ,#workhourtmp
    ,#tmpd
    ,#tmpLongDayCheck
    ,#tmpStmp_step2
    ,#ConcatStyle
    ,#tmpTotalWT
    ,#tmpGantt
	,#tmpProduceDays
	,#tmpDistinct
END
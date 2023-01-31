
CREATE PROCEDURE [dbo].[GetProductionOutputSummary]

	@Year varchar(10) = '',
	@Brand varchar(10) = '',
	@MDivisionID varchar(10) = '',
	@Fty varchar(10) = '',
	@Zone varchar(10) = '',
	@DateType int = 1,
	@ChkOrder bit = 0,
	@ChkForecast bit = 0,
	@ChkFtylocalOrder bit = 0,
	@ExcludeSampleFactory bit = 0,
	@ChkMonthly bit = 0,
	@IncludeCancelOrder bit = 0,
	@IsFtySide bit = 0,
	@IsPowerBI bit =0,
	@IsByCMPLockDate bit = 0,
	@OutputDate varchar(10) = ''
AS
BEGIN
	SET NOCOUNT ON;

 -- Planning_R05 only!  Today + 5 Month + 6 Day
declare @SpecialDay date = (select date = DATEADD(DAY,6, DATEADD(m,5, dateadd(m, datediff(m,0,getdate()),0))))
declare @SpecialDay2 date = (select date = DATEADD(DAY,0, DATEADD(m,5, dateadd(m, datediff(m,0,getdate()),0))))
declare @SewLock date = (select top 1 sewLock from dbo.System)
declare @NoRestrictOrdersDelivery bit = (select NoRestrictOrdersDelivery from system)

declare @StartYYYY varchar(4) = iif(isnull(@Year, '') = '', Year(getdate()), @Year)
declare @StartDateForBI date = cast(@StartYYYY + '0108' as date)
declare @StartBuyerDeliveryForBI date = cast(@StartYYYY + '0101' as date)
declare @EndSCIDeliveryForBI date = dateadd(YEAR, 2, dateadd(day, -1, @StartDateForBI))
declare @EndBuyerDeliveryForBI date = dateadd(YEAR, 2, dateadd(day, -1, @StartBuyerDeliveryForBI))

declare @tmpBaseOrderID TABLE(
	[ID] [VARCHAR](13) NULL,  
	[FACTORYID] [VARCHAR](8) NULL,
	[TransFtyZone] [VARCHAR](8) NULL,
	INDEX tmpBaseOrderID_Index (ID,FACTORYID)
)
INSERT INTO @tmpBaseOrderID
select * 
--into #tmpBaseOrderID
from
(
	select  o.ID,o.FactoryID,[TransFtyZone] = ''
	from Orders o with(nolock)
	inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
	left join SCIFty with(nolock) on SCIFty.ID = o.FactoryID
	where    (@IsPowerBI = 1 or IsProduceFty = 1) and o.Category !='' and (o.IsBuyBack != 1 or o.BuyBackReason != 'Garment')
			 and 
			 (	-- if use in PowerBI then filter SciDelivery or BuyerDelivery
				(@IsPowerBI = 1 and 
					(
						o.SciDelivery between @StartDateForBI and @EndSCIDeliveryForBI
						or
						o.BuyerDelivery between @StartBuyerDeliveryForBI and @EndBuyerDeliveryForBI
					)
				)
				or -- if not use in PowerBI then depend on @DateType
				(@IsPowerBI = 0 and 
					(
						(@DateType = 1 and Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year )
						or 
						(@DateType = 2 and Year(o.BuyerDelivery) = @Year )
					)
				)
			 )
			 and (o.BrandID = @Brand or @Brand = '')
			 and (o.MDivisionID = @MDivisionID or @MDivisionID = '')
			 and (SCIFty.FtyZone = @Zone or @Zone = '')
			 and (o.FtyGroup = @Fty or @Fty = '')
			 and (
				(@ExcludeSampleFactory = 1 and SCIFty.Type <> 'S')
				or
				(@ExcludeSampleFactory = 0)
			 )
			 and (
				(@IsFtySide = 1 and
					(
						(
							@NoRestrictOrdersDelivery = 0 and
							(
								((@DateType = 1 and O.SciDelivery <= @SpecialDay) or (@DateType = 2 and o.BuyerDelivery < @SpecialDay2))	
							)
						)
						or @NoRestrictOrdersDelivery = 1
					)
				)
				or 
				(@IsFtySide = 0)
			 )
			 and (
				@IncludeCancelOrder = 0 and o.Junk = 0
				or
				@IncludeCancelOrder = 1 and 1 = 1
			)

	union all

	select  o.ID,o.FactoryID,[TransFtyZone] = ''
	from Orders o with(nolock)
	inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
	left join SCIFty with(nolock) on SCIFty.ID = o.FactoryID
	where   (@IsPowerBI = 1 or IsProduceFty = 1) and o.Category ='' and (o.IsBuyBack != 1 or o.BuyBackReason != 'Garment')
			 and 
			 (	-- if use in PowerBI then filter SciDelivery or BuyerDelivery
				(@IsPowerBI = 1 and 
					(
						o.SciDelivery between @StartDateForBI and @EndSCIDeliveryForBI
						or
						o.BuyerDelivery between @StartBuyerDeliveryForBI and @EndBuyerDeliveryForBI
					)
				)
				or -- if not use in PowerBI then depend on @DateType
				(@IsPowerBI = 0 and 
					(
						(@DateType = 1 and Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year )
						or 
						(@DateType = 2 and Year(o.BuyerDelivery) = @Year )
					)
				)
			 )
			 and (o.BrandID = @Brand or @Brand = '')
			 and (o.MDivisionID = @MDivisionID or @MDivisionID = '')
			 and (SCIFty.FtyZone = @Zone or @Zone = '')
			 and (o.FtyGroup = @Fty or @Fty = '')
			 and (
				(@ExcludeSampleFactory = 1 and SCIFty.Type <> 'S')
				or
				(@ExcludeSampleFactory = 0)
			 )
			 and (
				(@IsFtySide = 1 and
					(
						(
							@NoRestrictOrdersDelivery = 0 and
							(
								((@DateType = 1 and O.SciDelivery <= @SpecialDay) or (@DateType = 2 and o.BuyerDelivery < @SpecialDay2))	
							)
						)
						or @NoRestrictOrdersDelivery = 1
					)
				)
				or 
				(@IsFtySide = 0)
			 )
			and (
				@IncludeCancelOrder = 0 and o.Junk = 0
				or
				@IncludeCancelOrder = 1 and 1 = 1
			)
) a


--代工的訂單，以ProgramID抓出自己工廠的資料，後續要顯示在detail並扣除summary的資料
declare @tmpBaseTransOrderID TABLE(
	[ID] [VARCHAR](13) NULL,
	[TransFtyZone] [VARCHAR](8) NULL
	INDEX tmpBaseTransOrderID_Index (ID)
)
INSERT INTO @tmpBaseTransOrderID
select * 
--into    #tmpBaseTransOrderID
from (
	select  o.ID,[TransFtyZone] = f.FtyZone
	from Orders o with(nolock)
	left join Factory f with(nolock) on f.ID = o.ProgramID and f.junk = 0
	where   o.LocalOrder = 1 and o.SubconInType = 2 and o.Category !='' and (o.IsBuyBack != 1 or o.BuyBackReason != 'Garment')
		and (
				(@DateType = 1 and Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year )
				or 
				(@DateType = 2 and Year(o.BuyerDelivery) = @Year )

		 )
		and 
		(	-- if use in PowerBI then filter SciDelivery or BuyerDelivery
			(@IsPowerBI = 1 and 
				(
					o.SciDelivery between @StartDateForBI and @EndSCIDeliveryForBI
					or
					o.BuyerDelivery between @StartBuyerDeliveryForBI and @EndBuyerDeliveryForBI
				)
			)
			or -- if not use in PowerBI then depend on @DateType
			(@IsPowerBI = 0 and 
				(
					(@DateType = 1 and Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year )
					or 
					(@DateType = 2 and Year(o.BuyerDelivery) = @Year )
				)
			)
		)
		and (o.BrandID = @Brand or @Brand = '')
		and (
				@IncludeCancelOrder = 0 and o.Junk = 0
				or
				@IncludeCancelOrder = 1 and 1 = 1
		)
		and 
		(
			(@IsFtySide = 1 and
				(
					(
						@NoRestrictOrdersDelivery = 0 and
						(
							((@DateType = 1 and O.SciDelivery <= @SpecialDay) or (@DateType = 2 and o.BuyerDelivery < @SpecialDay2))	
						)
					)
					or @NoRestrictOrdersDelivery = 1
				)
			)
			or 
			(@IsFtySide = 0)
		)
		and o.ProgramID in (select distinct FactoryID from @tmpBaseOrderID)

	union all

	select  o.ID,[TransFtyZone] = f.FtyZone
	from Orders o with(nolock)
	left join Factory f with(nolock) on f.ID = o.ProgramID and f.junk = 0
	where   o.LocalOrder = 1 and o.SubconInType = 2 and o.Category='' and (o.IsBuyBack != 1 or o.BuyBackReason != 'Garment')
		 and 
		 (	-- if use in PowerBI then filter SciDelivery or BuyerDelivery
		 	(@IsPowerBI = 1 and 
		 		(
		 			o.SciDelivery between @StartDateForBI and @EndSCIDeliveryForBI
					or
					o.BuyerDelivery between @StartBuyerDeliveryForBI and @EndBuyerDeliveryForBI
		 		)
		 	)
		 	or -- if not use in PowerBI then depend on @DateType
		 	(@IsPowerBI = 0 and 
		 		(
		 			(@DateType = 1 and Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year )
		 			or 
		 			(@DateType = 2 and Year(o.BuyerDelivery) = @Year )
		 		)
		 	)
		 )
		 and (o.BrandID = @Brand or @Brand = '')
		 and (
				@IncludeCancelOrder = 0 and o.Junk = 0
				or
				@IncludeCancelOrder = 1 and 1 = 1
		 )
		and (
		(@IsFtySide = 1 and
			(
				(
					@NoRestrictOrdersDelivery = 0 and
					(
						((@DateType = 1 and O.SciDelivery <= @SpecialDay) or (@DateType = 2 and o.BuyerDelivery < @SpecialDay2))	
					)
				)
				or @NoRestrictOrdersDelivery = 1
			)
		)
		or 
		(
			@IsFtySide = 0)
		)
		 and (o.BrandID = @Brand or @Brand = '')
		 and o.ProgramID in (select distinct FactoryID from @tmpBaseOrderID)
)a

declare @tmpBaseStep1 TABLE(
	[ID] [VARCHAR](13) NULL ,
	[TransFtyZone] [VARCHAR](8) NULL,
	INDEX tmpBaseTransOrderID_Index (ID)
)
INSERT INTO @tmpBaseStep1
select * 
--into #tmpBaseStep1
from (
    select  ID,TransFtyZone from @tmpBaseOrderID where ID not in (select ID from @tmpBaseTransOrderID)
    union all
    select  ID,TransFtyZone from @tmpBaseTransOrderID
) a

Declare @tmpSewingOutput Table(
	[OrderID]  varchar(13)  INDEX tmpSewingOutputID_IDX CLUSTERED,
	[OutputDate] date,
	[QAQty] [NUMERIC](16,6)
)

Declare @OutputDateLimit date

if(@OutputDate = '' and @IsByCMPLockDate = 1)
begin
	set @OutputDateLimit = @SewLock
end

if(@OutputDate <> '')
begin
	set @OutputDateLimit = @OutputDate
end

insert into @tmpSewingOutput(OrderId, OutputDate, QAQty)
select	sdd.OrderId,
		s.OutputDate,
		isnull(sum(isnull(sdd.QAQty,0) * isnull(ol.Rate, sl.Rate)),0) / 100
from SewingOutput_Detail_Detail sdd with (nolock)
inner join SewingOutput s with (nolock) on s.ID = sdd.ID
inner join Orders o with(nolock) on o.ID = sdd.OrderId
left join Order_Location ol with (nolock) on ol.OrderId = sdd.OrderId and ol.Location = sdd.ComboType
left join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey and sl.Location = sdd.ComboType
where	exists(select 1 from @tmpBaseStep1 tbs where tbs.ID = sdd.OrderId) and 
		(@OutputDateLimit is null or s.OutputDate <= @OutputDateLimit)
group by	sdd.OrderId,
			s.OutputDate

insert into @tmpSewingOutput(OrderId, OutputDate, QAQty)
select	stf.FromOrderID,
		cast(st.EditDate as date),
		(isnull(sum(isnull(stf.TransferQty,0) * isnull(ol.Rate, sl.Rate)),0) / 100) 
from SewingOutputTransfer_Detail stf with (nolock)
inner join SewingOutputTransfer st with (nolock) on st.ID = stf.ID
inner join Orders o with(nolock) on o.ID = stf.FromOrderID and o.Junk=1 and o.NeedProduction=1
left join Order_Location ol with (nolock) on ol.OrderId = stf.FromOrderID and ol.Location = stf.FromComboType
left join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey and sl.Location = stf.FromComboType
where	st.Status= 'confirmed' and
		exists(select 1 from @tmpBaseStep1 tbs where tbs.ID = stf.FromOrderID)
group by stf.FromOrderID,
		cast(st.EditDate as date)


declare @tmpBase TABLE(
	[ID] [VARCHAR](13) NULL,
	[Date] [VARCHAR](8) NULL,
	[SCIKey] [VARCHAR](8) NULL,
	[SCIKeyHalf] [VARCHAR](8) NULL,
	[BuyerKey] [VARCHAR](8) NULL,
	[BuyerKeyHalf] [VARCHAR](8) NULL,
	[OutputDate] [VARCHAR](8) NULL,
	[OrderCPU] [NUMERIC](10,3) NULL,
	[OrderShortageCPU] [NUMERIC](10,4) NULL,
	[SewingOutput] [NUMERIC](16,6) NULL,
	[SewingOutputCPU] [NUMERIC](16,6) NULL,
	[Junk] [BIT] NULL,
	[Qty] [INT] NULL,
	[Category] [VARCHAR](1) NULL,
	[SubconInType] [VARCHAR](1) NULL,
	[IsForecast] [BIT] NULL,
	[LocalOrder] [BIT] NULL,
	[FactoryID] [VARCHAR](8) NULL,
	[FtyGroup] [VARCHAR](8) NULL,
	[IsProduceFty] [BIT] NULL,
	[FtyZone] [VARCHAR](8) NULL,
    [ProgramID] [VARCHAR](12) NULL,
	[TransFtyZone] [VARCHAR](8) NULL,
	[IsCancelNeedProduction] [VARCHAR](1) NULL
	INDEX tmpBaseTransOrderID_Index (ID)
)
INSERT INTO @tmpBase
select
    o.ID,
    [Date]= format(iif(@DateType = '1', KeyDate.SCI, KeyDate.Buyer), 'yyyyMM'),
    [SCIKey] = format(KeyDate.SCI, 'yyyyMM'),
    [SCIKeyHalf] = iif(cast(format(KeyDate.SCI, 'dd') as int) between 1 and 15, format(KeyDate.SCI, 'yyyyMM01'), format(KeyDate.SCI, 'yyyyMM02')),
    [BuyerKey] = format(KeyDate.Buyer, 'yyyyMM'),
    [BuyerKeyHalf] = iif(cast(format(KeyDate.Buyer, 'dd') as int) between 1 and 15, format(KeyDate.Buyer, 'yyyyMM01'), format(KeyDate.Buyer, 'yyyyMM02')),
    [OutputDate] = FORMAT(sdd.OutputDate,'yyyyMM'),
    [OrderCPU] = o.Qty * gcRate.CpuRate * o.CPU,
    [OrderShortageCPU] = iif(o.GMTComplete = 'S' ,(o.Qty - GetPulloutData.Qty)  * gcRate.CpuRate * o.CPU ,0),
	[SewingOutput] = isnull(sdd.QAQty, 0),
    [SewingOutputCPU] = isnull(sdd.QAQty, 0) * gcRate.CpuRate * o.CPU,
    o.Junk,
    o.Qty,
    o.Category,
    o.SubconInType,
    o.IsForecast,
    o.LocalOrder,
    o.FactoryID,
    o.FtyGroup,
    f.IsProduceFty,
    f.FtyZone,
    o.ProgramID,
    tbs.TransFtyZone,
    [IsCancelNeedProduction] = iif(o.Junk = 1 and o.NeedProduction = 1, 'Y' , 'N')
from @tmpBaseStep1 tbs
inner join Orders o with(nolock) on o.ID = tbs.ID
inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
left join @tmpSewingOutput sdd on sdd.OrderId = o.ID
outer apply (select [CpuRate] = case when o.IsForecast = 1 then o.CPUFactor
                                     when o.LocalOrder = 1 then 1
                                     else o.CPUFactor end
                     ) gcRate
outer apply (select Qty=sum(pd.ShipQty)
	from PackingList p with(nolock), PackingList_Detail pd with(nolock)
	where p.ID = pd.ID 
		  and p.PulloutID <> '' 
		  and pd.OrderID = o.ID
) GetPulloutData
outer apply (select [SCI] = dateadd(day,-7,o.SciDelivery),
                    [Buyer] = o.BuyerDelivery) KeyDate






declare @tmpBaseBySource TABLE(
	[ID] [VARCHAR](13) NULL,
	[FactoryID] [VARCHAR](8) NULL,
	[FtyGroup] [VARCHAR](8) NULL,
	[Date] [VARCHAR](8) NULL,
	[SCIKey] [VARCHAR](8) NULL,
	[SCIKeyHalf] [VARCHAR](8) NULL,
	[BuyerKey] [VARCHAR](8) NULL,
	[BuyerKeyHalf] [VARCHAR](8) NULL,
	[OutputDate] [VARCHAR](8) NULL,
	[OrderCPU] [NUMERIC](10,3) NULL,
	[OrderShortageCPU] [NUMERIC](10,4) NULL,
	[SewingOutput] [NUMERIC](16,6) NULL,
	[SewingOutputCPU] [NUMERIC](16,6) NULL,
	[IsProduceFty] [BIT] NULL,
	[isNormalOrderCanceled] [BIT] NULL,
	[FtyZone] [VARCHAR](8) NULL,
    [ProgramID] [VARCHAR](12) NULL,
	[TransFtyZone] [VARCHAR](8) NULL,
	[IsCancelNeedProduction] [VARCHAR](1) NULL,
	[DateByHalfMonth] [VARCHAR](8) NULL
)
INSERT INTO @tmpBaseBySource
select  ID,
        FactoryID,
        FtyGroup,
        Date,
        SCIKey,
        SCIKeyHalf,
        BuyerKey,
        BuyerKeyHalf,
        OutputDate,
        OrderCPU,
        OrderShortageCPU,
        SewingOutput,
        SewingOutputCPU,
        IsProduceFty,
        --summary頁面不算Junk訂單使用，Forecast沒排掉是因為Planning R10有含
        [isNormalOrderCanceled] = iif(  Junk = 1 and 
                                        --正常訂單
                                        (( Category in ('B','S')  and (localorder = 0 or SubconInType=2)) or
                                        --當地訂單
                                        (LocalOrder = 1 )),1,0),
        FtyZone,
        ProgramID,
        TransFtyZone,
        IsCancelNeedProduction,
        [DateByHalfMonth] = iif(@DateType = '1', SCIKeyHalf, BuyerKeyHalf)
--into #tmpBaseBySource
from @tmpBase
where 1=1
and (
	(@ChkForecast = 1 and ( IsForecast = 1 and (localorder = 0 or SubconInType=2)))
	or
	(@ChkFtylocalOrder = 1 and (LocalOrder = 1 and SubconInType <> 1))
	or
	(@ChkOrder = 1 and ( Category in ('B','S')  and (localorder = 0 or SubconInType=2)))
)
or TransFtyZone <> ''

declare @tmpBaseByOrderID TABLE(
	[ID] [VARCHAR](13) NULL,
	[Date] [VARCHAR](8) NULL,
	[OrderCPU] [NUMERIC](10,3) NULL,
	[OrderShortageCPU] [NUMERIC](10,4) NULL,
	[SewingOutput] [NUMERIC](16,6) NULL,
	[SewingOutputCPU] [NUMERIC](16,6) NULL,
	[FtyZone] [VARCHAR](8) NULL,
	[TransFtyZone] [VARCHAR](8) NULL,
	[IsCancelNeedProduction] [VARCHAR](1) NULL,
	[SCIKey] [VARCHAR](8) NULL,
	[SCIKeyHalf] [VARCHAR](8) NULL,
	[BuyerKey] [VARCHAR](8) NULL,
	[BuyerKeyHalf] [VARCHAR](8) NULL
)
INSERT INTO @tmpBaseByOrderID
select
    ID,
    Date,
    OrderCPU,
    OrderShortageCPU,
    [SewingOutput] = SUM(SewingOutput),
    [SewingOutputCPU] = SUM(SewingOutputCPU),
    FtyZone,
    TransFtyZone,
    IsCancelNeedProduction,
    SCIKey,
    SCIKeyHalf,
    BuyerKey,
    BuyerKeyHalf
--into #tmpBaseByOrderID
from @tmpBaseBySource
group by ID,Date,OrderCPU,OrderShortageCPU,FtyZone,TransFtyZone,IsCancelNeedProduction,SCIKey,SCIKeyHalf,BuyerKey,BuyerKeyHalf

declare @tmpOrder_QtyShip TABLE(
	[ID] [VARCHAR](13) NULL INDEX tmptmpOrder_QtyShipID_IDX CLUSTERED,
	[LastBuyerDelivery] [DATE] NULL,	
	[PartialShipment] [VARCHAR](1) NULL
)
INSERT INTO @tmpOrder_QtyShip

select  oq.ID, [LastBuyerDelivery] = Max(oq.BuyerDelivery), [PartialShipment] = iif(count(1) > 1, 'Y', '')
--into #tmpOrder_QtyShip
from    Order_QtyShip oq with (nolock)
where exists (select 1 from @tmpBaseByOrderID tb where tb.ID = oq.ID)
group by    oq.ID

declare @tmpPullout_Detail TABLE(
	[OrderID] [VARCHAR](13) NULL INDEX tmpPullout_DetailOrderID_IDX CLUSTERED,
	[PulloutQty] [INT] NULL
)
INSERT INTO @tmpPullout_Detail
select pd.OrderID, [PulloutQty] = sum(pd.shipQty)
                  from PackingList p with(nolock), PackingList_Detail pd with(nolock)
                  where p.ID = pd.ID
                  and p.PulloutID <> ''
                  and exists (select 1 from @tmpBaseByOrderID tb where tb.ID = pd.OrderID)
                  group by pd.OrderID

select
	o.MDivisionID,
    tb.FtyZone,
	o.FactoryID,
	o.BuyerDelivery,
	o.SciDelivery,
    tb.SCIKey,
    tb.SCIKeyHalf,
    tb.BuyerKey,
    tb.BuyerKeyHalf,
	o.ID,
	Category =case when o.Category='B' then 'Bulk'
				when o.Category='S' then 'Sample'
				when o.Category='' then 'Forecast'
				end,
	[SampleGroup] = SampleGroup.Name,
	Cancelled=iif(o.Junk=1,'Y',''),
    tb.IsCancelNeedProduction,
	[Buyback] = IIF(o.IsBuyBack = 1,'Y',''),
    toq.PartialShipment,
	[FMSister] = IIF(o.SubconInType in (1, 2), 'Y', ''),
    toq.LastBuyerDelivery,
	o.StyleID,
	[Article] = ArticleList.value,
	o.SeasonID,
	o.CustPONO,
	o.BrandID,
	o.CPU,
	o.Qty,
	o.FOCQty,
    tpd.PulloutQty,
    tb.OrderShortageCPU,
	[TotalCPU] = TotalCPU.val,
	tb.SewingOutput,
    tb.SewingOutputCPU,
	BalanceQty = isnull(o.Qty,0)-isnull(tb.SewingOutput,0),
	[BalanceCPU] = iif(BalanceCPU.val >= 0, BalanceCPU.val, null),
    BalanceCPUIrregular = iif(BalanceCPU.val >= 0, null, BalanceCPU.val),
	o.SewLine,
	o.Dest,
	o.OrderTypeID,
	o.ProgramID,
	[ProductType] = (select 
					[ProductType] = Reason.Name
					from Style s WITH (NOLOCK) 
					left join Reason WITH(NOLOCK) on ReasonTypeID= 'Style_Apparel_Type' and Reason.ID = s.ApparelType
					where s.Ukey = o.StyleUkey),
	s.CDCodeNew,
    o.FtyGroup,
    [PulloutComplete] = iif(o.PulloutComplete = 1, 'OK', ''),
    o.SewInLine,
    o.SewOffLine,
    tb.TransFtyZone,
	[OrderReason] = OrderReason.ResName
from @tmpBaseByOrderID tb 
inner join Orders o with(nolock) on o.id = tb.ID
inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
left join Style s with (nolock) on s.Ukey = o.StyleUkey
left join @tmpOrder_QtyShip toq on toq.ID = tb.ID
left join @tmpPullout_Detail tpd on tpd.OrderID = tb.ID
outer apply (select [val] = iif(tb.IsCancelNeedProduction = 'N' and o.Junk = 1, 0, isnull(tb.OrderCPU, 0))) TotalCPU
outer apply (select [val] =  TotalCPU.val - isnull(tb.SewingOutputCPU, 0) - isnull(tb.OrderShortageCPU, 0)) BalanceCPU
outer apply (Select TOP 1 ResName=b.Name
			from dbo.TradeHIS_Order a
			left join dbo.Reason b on a.ReasonID = b.ID and b.ReasonTypeID = 'Order_BuyerDelivery'
			where a.TableName = 'Order_QtyShip' and a.HisType = 'Order_QtyShipFtyKPI' and a.SourceID = o.ID
			order by a.AddDate DESC) OrderReason
outer apply (SELECT D.Name 
			from DropDownList D 
			where D.Type='ForecastSampleGroup' and D.ID= o.ForecastSampleGroup) SampleGroup
outer apply(
	select value = Stuff((
		select concat(',',Article)
		from (
				select 	distinct
					Article
				from dbo.Order_Article od
				where od.id = o.ID
			) s
		for xml path ('')
	) , 1, 1, '')
) ArticleList

if @IsPowerBI = 0
begin

	select  FtyGroup,
			[Date] = iif(@ChkMonthly = 1,  SUBSTRING(Date,1,4)+SUBSTRING(Date,5,6),DateByHalfMonth),
			ID,
			[OutputDate] = isnull(OutputDate, iif(SewingOutputCPU !=0 ,iif(@ChkMonthly = 1,  SUBSTRING(Date,1,4)+SUBSTRING(Date,5,6),DateByHalfMonth),'')),
			[OrderCPU] = iif(IsCancelNeedProduction = 'N' and isNormalOrderCanceled = 1,0 , ISNULL(OrderCPU, 0) - ISNULL(OrderShortageCPU, 0)),
			[CanceledCPU] = iif(IsCancelNeedProduction = 'Y', ISNULL(OrderCPU, 0), 0),
			[OrderShortageCPU] = ISNULL(OrderShortageCPU, 0),
			[SewingOutput] = ISNULL(SewingOutput, 0),
			[SewingOutputCPU] = ISNULL(SewingOutputCPU, 0),
			FtyZone,
			TransFtyZone,
            IsCancelNeedProduction
	from @tmpBaseBySource 

	select  FtyGroup,OutputDate,[SewingOutputCPU] = sum(SewingOutputCPU) * -1,FtyZone
	from    @tmpBase
	where   Junk=1 and IsCancelNeedProduction = 'N' and OutputDate is not null 
	group by FtyGroup,OutputDate,FtyZone

end


END


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
	@IsFtySide bit = 0
	
AS
BEGIN
	SET NOCOUNT ON;

 -- Planning_R05 only!  Today + 5 Month + 6 Day
declare @SpecialDay date = (select date = DATEADD(DAY,6, DATEADD(m,5, dateadd(m, datediff(m,0,getdate()),0))))


select * 
into #tmpBaseOrderID
from
(
	select  o.ID,o.FactoryID,[TransFtyZone] = ''
	from Orders o with(nolock)
	inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
	left join SCIFty with(nolock) on SCIFty.ID = o.FactoryID
	where   IsProduceFty = 1 and o.Category !='' and (o.IsBuyBack != 1 or o.BuyBackReason != 'Garment')
			 and (
				(@DateType = 1 and Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year )
				or 
				(@DateType = 2 and Year(o.BuyerDelivery) = @Year )

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
				@IncludeCancelOrder = 0 and o.Junk = 0
				or
				@IncludeCancelOrder = 1 and 1 = 1
			)

	union all

	select  o.ID,o.FactoryID,[TransFtyZone] = ''
	from Orders o with(nolock)
	inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
	left join SCIFty with(nolock) on SCIFty.ID = o.FactoryID
	where   IsProduceFty = 1 and o.Category ='' and (o.IsBuyBack != 1 or o.BuyBackReason != 'Garment')
			 and (
				(@DateType = 1 and Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year )
				or 
				(@DateType = 2 and Year(o.BuyerDelivery) = @Year )

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
				(@IsFtySide = 1 and (O.SciDelivery < @SpecialDay or o.BuyerDelivery < @SpecialDay))
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

select * 
into    #tmpBaseTransOrderID
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
		and (o.BrandID = @Brand or @Brand = '')
		and (
				@IncludeCancelOrder = 0 and o.Junk = 0
				or
				@IncludeCancelOrder = 1 and 1 = 1
		)
		and o.ProgramID in (select distinct FactoryID from #tmpBaseOrderID)

	union all

	select  o.ID,[TransFtyZone] = f.FtyZone
	from Orders o with(nolock)
	left join Factory f with(nolock) on f.ID = o.ProgramID and f.junk = 0
	where   o.LocalOrder = 1 and o.SubconInType = 2 and o.Category='' and (o.IsBuyBack != 1 or o.BuyBackReason != 'Garment')
		 and (
				(@DateType = 1 and Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year )
				or 
				(@DateType = 2 and Year(o.BuyerDelivery) = @Year )

		 )
		 and (o.BrandID = @Brand or @Brand = '')
		 and (
				@IncludeCancelOrder = 0 and o.Junk = 0
				or
				@IncludeCancelOrder = 1 and 1 = 1
		 )
		 and (
				(@IsFtySide = 1 and (O.SciDelivery < @SpecialDay or o.BuyerDelivery < @SpecialDay))
				or 
				(@IsFtySide = 0)
			)
		 and (o.BrandID = @Brand or @Brand = '')
		 and o.ProgramID in (select distinct FactoryID from #tmpBaseOrderID)
)a


select * into #tmpBaseStep1
from (
    select  ID,TransFtyZone from #tmpBaseOrderID where ID not in (select ID from #tmpBaseTransOrderID)
    union all
    select  ID,TransFtyZone from #tmpBaseTransOrderID
) a


select
    o.ID,
    [Date]= format(iif(@DateType = '1', KeyDate.SCI, KeyDate.Buyer), 'yyyyMM'),
    [SCIKey] = format(KeyDate.SCI, 'yyyyMM'),
    [SCIKeyHalf] = iif(cast(format(KeyDate.SCI, 'dd') as int) between 1 and 15, format(KeyDate.SCI, 'yyyyMM01'), format(KeyDate.SCI, 'yyyyMM02')),
    [BuyerKey] = format(KeyDate.Buyer, 'yyyyMM'),
    [BuyerKeyHalf] = iif(cast(format(KeyDate.Buyer, 'dd') as int) between 1 and 15, format(KeyDate.Buyer, 'yyyyMM01'), format(KeyDate.Buyer, 'yyyyMM02')),
    [OutputDate] = FORMAT(s.OutputDate,'yyyyMM'),
    [OrderCPU] = o.Qty * gcRate.CpuRate * o.CPU,
    [OrderShortageCPU] = iif(o.GMTComplete = 'S' ,(o.Qty - GetPulloutData.Qty)  * gcRate.CpuRate * o.CPU ,0),
	[SewingOutput] = (isnull(sum(isnull(sdd.QAQty,0) * isnull(ol.Rate, sl.Rate)),0) / 100) + (isnull(fromTransfer.Qty,0)  
		- iif(obq.OrderIDFrom != '',	isnull(ToTransfer.Qty,0),0))
		/100,
    [SewingOutputCPU] = isnull(sum(isnull(sdd.QAQty,0) * isnull(ol.Rate, sl.Rate)),0) * gcRate.CpuRate * o.CPU / 100 + ((isnull(fromTransfer.Qty,0) 
		- iif(obq.OrderIDFrom != '', isnull(ToTransfer.Qty,0),0)) 
		* gcRate.CpuRate * o.CPU/100),
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
into #tmpBase
from #tmpBaseStep1 tbs
inner join Orders o with(nolock) on o.ID = tbs.ID
inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
left join SewingOutput_Detail_Detail sdd with (nolock) on o.ID = sdd.OrderId
left join SewingOutput s with (nolock) on sdd.ID = s.ID
left join Order_Location ol with (nolock) on ol.OrderId = sdd.OrderId and ol.Location = sdd.ComboType
left join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey and sl.Location = sdd.ComboType
--left join (  Order_BuyBack_Qty obq with (nolock) on obq.OrderIDFrom = o.ID
outer apply(
	select distinct OrderIDFrom from Order_BuyBack_Qty	
	where OrderIDFrom = o.id
)obq
outer apply (select [CpuRate] = case when o.IsForecast = 1 then (select CpuRate from GetCPURate(o.OrderTypeID, o.ProgramID, o.Category, o.BrandID, 'O'))
                                     when o.LocalOrder = 1 then 1
                                     else (select CpuRate from GetCPURate(o.OrderTypeID, o.ProgramID, o.Category, o.BrandID, 'O')) end
                     ) gcRate
outer apply (select Qty=sum(shipQty) from Pullout_Detail where orderid = o.id) GetPulloutData
outer apply (select [SCI] = dateadd(day,-7,o.SciDelivery),
                    [Buyer] = o.BuyerDelivery) KeyDate
outer apply(
	select Qty = sum(b.TransferQty * isnull(ol.Rate, sl.Rate))
	from SewingOutputTransfer a
	inner join SewingOutputTransfer_Detail b on a.id=b.ID
	left join Order_Location ol with (nolock) on ol.OrderId = b.FromOrderID and ol.Location = b.FromComboType
	left join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey and sl.Location = b.FromComboType
	where a.Status= 'confirmed'
	and b.FromOrderID = o.ID 
	and o.Junk=1 and o.NeedProduction=1
) fromTransfer
outer apply(
	select Qty = sum(b.TransferQty * isnull(ol.Rate, sl.Rate)) 
	from SewingOutputTransfer a
	inner join SewingOutputTransfer_Detail b on a.id=b.ID
	left join Order_Location ol with (nolock) on ol.OrderId = b.ToOrderID and ol.Location = b.ToComboType
	left join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey and sl.Location = b.ToComboType
	where a.Status= 'confirmed'
	and b.ToOrderID = o.ID
	and o.Junk=1 and o.NeedProduction=1
	and b.ToOrderID = obq.OrderIDFrom
) ToTransfer

group by o.ID,
KeyDate.SCI,
KeyDate.Buyer,
FORMAT(s.OutputDate,'yyyyMM'), 
o.CPU, 
o.Qty,
o.Junk,
o.NeedProduction,
o.Qty,
o.Category,
o.SubconInType,
o.IsForecast,
o.LocalOrder,
o.FactoryID,
o.FtyGroup,
f.IsProduceFty,
gcRate.CpuRate,
GetPulloutData.Qty,
o.GMTComplete,
f.FtyZone,
o.ProgramID,
tbs.TransFtyZone,
fromTransfer.Qty,ToTransfer.Qty,
obq.OrderIDFrom

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
into #tmpBaseBySource
from #tmpBase
where 1=1
and (
	(@ChkForecast = 1 and ( IsForecast = 1 and (localorder = 0 or SubconInType=2)))
	or
	(@ChkFtylocalOrder = 1 and (LocalOrder = 1 and SubconInType <> 1))
	or
	(@ChkOrder = 1 and ( Category in ('B','S')  and (localorder = 0 or SubconInType=2)))
)
or TransFtyZone <> ''

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
into #tmpBaseByOrderID
from #tmpBaseBySource
group by ID,Date,OrderCPU,OrderShortageCPU,FtyZone,TransFtyZone,IsCancelNeedProduction,SCIKey,SCIKeyHalf,BuyerKey,BuyerKeyHalf

select  oq.ID, [LastBuyerDelivery] = Max(oq.BuyerDelivery), [PartialShipment] = iif(count(1) > 1, 'Y', '')
into #tmpOrder_QtyShip
from    Order_QtyShip oq with (nolock)
where exists (select 1 from #tmpBaseByOrderID tb where tb.ID = oq.ID)
group by    oq.ID

select  pd.OrderID, [PulloutQty] = sum(pd.shipQty)
into #tmpPullout_Detail
from Pullout_Detail pd with (nolock)
where exists (select 1 from #tmpBaseByOrderID tb where tb.ID = pd.OrderID)
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
	Cancelled=iif(o.Junk=1,'Y',''),
    tb.IsCancelNeedProduction,
	[Buyback] = IIF(o.IsBuyBack = 1,'Y',''),
    toq.PartialShipment,
    toq.LastBuyerDelivery,
	o.StyleID,
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
	o.CdCodeID,
	CDCode.ProductionFamilyID,
    o.FtyGroup,
    [PulloutComplete] = iif(o.PulloutComplete = 1, 'OK', ''),
    o.SewInLine,
    o.SewOffLine,
    tb.TransFtyZone
from #tmpBaseByOrderID tb with(nolock)
inner join Orders o with(nolock) on o.id = tb.ID
left join #tmpOrder_QtyShip toq on toq.ID = tb.ID
left join #tmpPullout_Detail tpd on tpd.OrderID = tb.ID
left join CDCode with(nolock) on CDCode.ID = o.CdCodeID
outer apply (select [val] = iif(tb.IsCancelNeedProduction = 'N' and o.Junk = 1, 0, isnull(tb.OrderCPU, 0))) TotalCPU
outer apply (select [val] =  TotalCPU.val - isnull(tb.SewingOutputCPU, 0) - isnull(tb.OrderShortageCPU, 0)) BalanceCPU


select  FtyGroup,
		[Date] = iif(@ChkMonthly = 1,  SUBSTRING(Date,1,4)+SUBSTRING(Date,5,6),DateByHalfMonth),
		ID,
		[OutputDate] = case when @IncludeCancelOrder = 1 and OutputDate is null 
							then SUBSTRING(Date,1,4)+SUBSTRING(Date,5,6)
			else OutputDate end,
		[OrderCPU] = iif(IsCancelNeedProduction = 'N' and isNormalOrderCanceled = 1,0 ,OrderCPU - OrderShortageCPU),
		[CanceledCPU] = iif(IsCancelNeedProduction = 'Y',OrderCPU, 0),
		OrderShortageCPU,
		SewingOutput,
		SewingOutputCPU,
		FtyZone,
		TransFtyZone 
from #tmpBaseBySource 
where SewingOutput !=0

select  FtyGroup,OutputDate,[SewingOutputCPU] = sum(SewingOutputCPU) * -1,FtyZone
from    #tmpBase
where   Junk=1 and IsCancelNeedProduction = 'N' and OutputDate is not null 
group by FtyGroup,OutputDate,FtyZone

drop table #tmpBaseOrderID,#tmpBaseByOrderID,#tmpBaseTransOrderID,#tmpBaseStep1,#tmpBase,#tmpBaseBySource,#tmpOrder_QtyShip,#tmpPullout_Detail

END
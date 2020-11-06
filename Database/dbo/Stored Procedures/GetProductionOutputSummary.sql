Create PROCEDURE [dbo].[GetProductionOutputSummary]

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
	@IsByCMPLockDate bit = 0
	
AS
BEGIN
	SET NOCOUNT ON;

 -- Planning_R05 only!  Today + 5 Month + 6 Day
declare @SpecialDay date = (select date = DATEADD(DAY,6, DATEADD(m,5, dateadd(m, datediff(m,0,getdate()),0))))
declare @SpecialDay2 date = (select date = DATEADD(DAY,0, DATEADD(m,5, dateadd(m, datediff(m,0,getdate()),0))))
declare @SewLock date = (select top 1 sewLock from dbo.System)

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
	where   IsProduceFty = 1 and o.Category !='' and (o.IsBuyBack != 1 or o.BuyBackReason != 'Garment')
			 and 
			 (	-- if use in PowerBI then filter SciDelivery or BuyerDelivery
				(@IsPowerBI = 1 and 
					(
						Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year
						or
						Year(o.BuyerDelivery) = @Year
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
			 and 
			 (	-- if use in PowerBI then filter SciDelivery or BuyerDelivery
				(@IsPowerBI = 1 and 
					(
						Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year
						or
						Year(o.BuyerDelivery) = @Year
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
				(@IsFtySide = 1 and ((@DateType = 1 and O.SciDelivery <= @SpecialDay) or (@DateType = 2 and o.BuyerDelivery < @SpecialDay2)))
				or 
				(@IsFtySide = 0)
			)
			and (
				@IncludeCancelOrder = 0 and o.Junk = 0
				or
				@IncludeCancelOrder = 1 and 1 = 1
			)
) a


--�N�u���q��A�HProgramID��X�ۤv�u�t����ơA����n��ܦbdetail�æ���summary�����
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
					Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year
					or
					Year(o.BuyerDelivery) = @Year
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
		 			Year(cast(dateadd(day,-7,o.SciDelivery) as date)) = @Year
		 			or
		 			Year(o.BuyerDelivery) = @Year
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
				(@IsFtySide = 1 and ((@DateType = 1 and O.SciDelivery <= @SpecialDay) or (@DateType = 2 and o.BuyerDelivery < @SpecialDay2)))
				or 
				(@IsFtySide = 0)
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



insert into @tmpSewingOutput(OrderId, OutputDate, QAQty)
select	sdd.OrderId,
		s.OutputDate,
		(isnull(sum(isnull(sdd.QAQty,0) * isnull(ol.Rate, sl.Rate)),0) / 100)
from SewingOutput_Detail_Detail sdd with (nolock)
inner join SewingOutput s with (nolock) on s.ID = sdd.ID
inner join Orders o with(nolock) on o.ID = sdd.OrderId
left join Order_Location ol with (nolock) on ol.OrderId = sdd.OrderId and ol.Location = sdd.ComboType
left join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey and sl.Location = sdd.ComboType
where exists(select 1 from @tmpBaseStep1 tbs where tbs.ID = sdd.OrderId) and (@IsByCMPLockDate = 0 or s.OutputDate <= @SewLock)
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
outer apply (select [CpuRate] = case when o.IsForecast = 1 then (select CpuRate from GetCPURate(o.OrderTypeID, o.ProgramID, o.Category, o.BrandID, 'O'))
                                     when o.LocalOrder = 1 then 1
                                     else (select CpuRate from GetCPURate(o.OrderTypeID, o.ProgramID, o.Category, o.BrandID, 'O')) end
                     ) gcRate
outer apply (select Qty=sum(shipQty) from Pullout_Detail where orderid = o.id) GetPulloutData
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
        --summary��������Junk�q��ϥΡAForecast�S�Ʊ��O�]��Planning R10���t
        [isNormalOrderCanceled] = iif(  Junk = 1 and 
                                        --���`�q��
                                        (( Category in ('B','S')  and (localorder = 0 or SubconInType=2)) or
                                        --��a�q��
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
select  pd.OrderID, [PulloutQty] = sum(pd.shipQty)
--into #tmpPullout_Detail
from Pullout_Detail pd with (nolock)
where exists (select 1 from @tmpBaseByOrderID tb where tb.ID = pd.OrderID)
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
from @tmpBaseByOrderID tb 
inner join Orders o with(nolock) on o.id = tb.ID
left join @tmpOrder_QtyShip toq on toq.ID = tb.ID
left join @tmpPullout_Detail tpd on tpd.OrderID = tb.ID
left join CDCode with(nolock) on CDCode.ID = o.CdCodeID
outer apply (select [val] = iif(tb.IsCancelNeedProduction = 'N' and o.Junk = 1, 0, isnull(tb.OrderCPU, 0))) TotalCPU
outer apply (select [val] =  TotalCPU.val - isnull(tb.SewingOutputCPU, 0) - isnull(tb.OrderShortageCPU, 0)) BalanceCPU

if @IsPowerBI = 0
begin

	select  FtyGroup,
			[Date] = iif(@ChkMonthly = 1,  SUBSTRING(Date,1,4)+SUBSTRING(Date,5,6),DateByHalfMonth),
			ID,
			[OutputDate] = isnull(OutputDate, iif(SewingOutputCPU !=0 ,iif(@ChkMonthly = 1,  SUBSTRING(Date,1,4)+SUBSTRING(Date,5,6),DateByHalfMonth),'')),
			[OrderCPU] = iif(IsCancelNeedProduction = 'N' and isNormalOrderCanceled = 1,0 ,OrderCPU - OrderShortageCPU),
			[CanceledCPU] = iif(IsCancelNeedProduction = 'Y',OrderCPU, 0),
			OrderShortageCPU,
			SewingOutput,
			SewingOutputCPU,
			FtyZone,
			TransFtyZone 
	from @tmpBaseBySource 

	select  FtyGroup,OutputDate,[SewingOutputCPU] = sum(SewingOutputCPU) * -1,FtyZone
	from    @tmpBase
	where   Junk=1 and IsCancelNeedProduction = 'N' and OutputDate is not null 
	group by FtyGroup,OutputDate,FtyZone

end


END

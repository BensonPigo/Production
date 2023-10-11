Create PROCEDURE [dbo].[P_Import_PPIC_MASTER_LIST_ArtWorkType] 
	 @Date_S as date = null,
	 @Date_E as date = null,
	 @YearMonth_S as date = null,
	 @YearMonth_E as date = null
AS
BEGIN
	SET NOCOUNT ON;

	IF @Date_S IS NULL
	BEGIN
		SET @Date_S = DATEADD(Year,-1,DATEADD(Year,DATEDIFF(Year,0,getdate()),0))+7 --一年前的1/8
	END

	IF @Date_E IS NULL
	BEGIN
		SET @Date_E = DATEADD(Year,3,DATEADD(Year,DATEDIFF(Year,0,getdate()),0))+6 --三年後的1/7
	END

	IF @YearMonth_S IS NULL
	BEGIN
		SET @YearMonth_S = DATEADD(Year,-1,DATEADD(Year,DATEDIFF(Year,0,getdate()),0)) --一年前的1/1
	END

	IF @YearMonth_E IS NULL
	BEGIN
		SET @YearMonth_E = DATEADD(Year,3,DATEADD(Year,DATEDIFF(Year,0,getdate()),0))-1 --二年後的12/31	
	END

	-- 撈取ArtworkType相關資訊---
	select *
	into #UseArtworkType
	from
	(
		select ID
				, unit = nu.nUnit
				, SEQ
				, isOri = 1
		FROM [MainServer].[Production].[dbo].ArtworkType with (nolock)
		OUTER APPLY (
			SELECT [nUnit] = CASE WHEN ArtworkUnit = 'PPU' THEN 'PPU'
								WHEN ProductionUnit = 'TMS' THEN 'TMS'
							ELSE 'Price'
							END 
		) nu
		where Junk = 0
		union
		select ID
				 , ArtworkUnit
				 , SEQ
				 , isOri = 1
		from [MainServer].[Production].[dbo].ArtworkType with (nolock)
		where Junk = 0 
		and ArtworkUnit NOT IN ('', 'PPU')
	)a 


	--訂單串接ArtworkType資訊，訂單包含一般訂單與Local訂單，其中Local訂單有姊妹廠代工的狀況(SubconInType in '1','2')，若有此情況需產生兩筆同樣的資訊其中[Value],[TTL_Value],[SubconInType]一正一負進行紀錄

		-- Order_TmsCost
		Select
			o.ID,
			o.FactoryID,
			ArtworkType = at.ID,
			at.SEQ,
			at.Unit,
			[Value] =  case at.Unit When 'TMS' Then t.TMS
						When 'STITCH' Then t.Qty
						When 'PCS' Then t.Qty
						Else t.Price
						End,
			TTL_Value = case at.Unit When 'TMS' Then o.Qty * t.TMS
						When 'STITCH' Then o.Qty * t.Qty / 1000
						When 'PCS' Then o.Qty * t.Qty
						Else o.Qty * t.Price
						End,
			SubconInType = 0,
			[ArtworkTypeKey] = CONCAT(at.ID, '-', at.Unit),
			[OrderDataKey] = CONCAT(o.ID, o.SubconInType)
		into #tmp
		from [MainServer].[Production].[dbo].Orders o WITH(NOLOCK)
		inner join [MainServer].[Production].[dbo].Order_TmsCost t WITH(NOLOCK) on t.ID = o.ID
		inner join #UseArtworkType at on at.ID = t.ArtworkTypeID
		where (o.ScIDelivery between @Date_S and @Date_E or o.BuyerDelivery BETWEEN @YearMonth_S and @YearMonth_E)
		and o.Category in ('B','S')
		and o.Junk = 0
		and o.LocalOrder = 0
	union all
		--Forecast Style_TmsCost
		Select 
			o.ID,
			o.FactoryID,
			ArtworkTypeID = at.ID,
			at.SEQ,
			at.Unit,
			Value = case at.Unit When 'TMS' Then t.TMS
					When 'STITCH' Then t.Qty * 1.0
					When 'PCS' Then t.Qty * 1.0
					Else t.Price
					End,
			TTL_Value = case at.Unit When 'TMS' Then o.Qty * t.TMS
					When 'STITCH' Then o.Qty * t.Qty * 1.0 / 1000
					When 'PCS' Then o.Qty * t.Qty * 1.0
					Else o.Qty * t.Price
					End,
			SubconInType = 0,
			[ArtworkTypeKey] = CONCAT(at.ID, '-', at.Unit),
			[OrderDataKey] = CONCAT(o.ID, o.SubconInType)
		From [MainServer].[Production].[dbo].Orders o WITH(NOLOCK)
		inner join [MainServer].[Production].[dbo].Style_TmsCost t WITH(NOLOCK) on t.StyleUkey = o.StyleUkey
		inner join #UseArtworkType at on at.ID = t.ArtworkTypeID
		WHERE not exists (select 1 from [MainServer].[Production].[dbo].Order_TmsCost t WITH(NOLOCK) where t.ID = o.ID)
		and (o.ScIDelivery between @Date_S and @Date_E or o.BuyerDelivery BETWEEN @YearMonth_S and @YearMonth_E)
		and o.Junk = 0
		and o.IsForecast = 1
	union  all
		--loacl訂單 Order_TmsCost 轉入
		Select　
			o.ID,
			o.FactoryID,
			ArtworkType = at.ID,
			at.SEQ,
			at.Unit,
			[Value] =  case at.Unit When 'TMS' Then t.TMS
						When 'STITCH' Then t.Qty
						When 'PCS' Then t.Qty
						Else t.Price
						End,
			TTL_Value = case at.Unit When 'TMS' Then o.Qty * t.TMS
						When 'STITCH' Then o.Qty * t.Qty / 1000
						When 'PCS' Then o.Qty * t.Qty
						Else o.Qty * t.Price
						End,
			SubconInType = o.SubconInType,
			[ArtworkTypeKey] = CONCAT(at.ID, '-', at.Unit),
			[OrderDataKey] = CONCAT(o.ID, '-', o.SubconInType)
		from [MainServer].[Production].[dbo].Orders o WITH(NOLOCK)
		inner join [MainServer].[Production].[dbo].Order_TmsCost t WITH(NOLOCK) on t.ID = o.ID
		inner join #UseArtworkType at on at.ID = t.ArtworkTypeID
		inner join [MainServer].[Production].[dbo].Factory f WITH(NOLOCK) on o.FactoryID = f.ID
		where (o.ScIDelivery between @Date_S and @Date_E or o.BuyerDelivery BETWEEN @YearMonth_S and @YearMonth_E)
		and SubconInType in ('1', '2')
		and o.Junk = 0
		and o.LocalOrder = 1
		and f.Foundry = 1
	union  all
		-- Local訂單 Order_TmsCost 轉出
		Select
			o.ID,
			o.FactoryID,
			ArtworkType = at.ID,
			at.SEQ,
			at.Unit,
			[Value] =  case at.Unit When 'TMS' Then t.TMS
						When 'STITCH' Then t.Qty
						When 'PCS' Then t.Qty
						Else t.Price
						End * -1,
			TTL_Value = case at.Unit When 'TMS' Then o.Qty * t.TMS
						When 'STITCH' Then o.Qty * t.Qty / 1000
						When 'PCS' Then o.Qty * t.Qty
						Else o.Qty * t.Price
						End * -1,
			SubconInType = '-' + o.SubconInType,
			[ArtworkTypeKey] = CONCAT(at.ID, '-', at.Unit),
			[OrderDataKey] = CONCAT(o.ID, '--', o.SubconInType)
		from [MainServer].[Production].[dbo].Orders o WITH(NOLOCK)
		inner join [MainServer].[Production].[dbo].Order_TmsCost t WITH(NOLOCK) on t.ID = o.ID
		inner join #UseArtworkType at on at.ID = t.ArtworkTypeID
		inner join [MainServer].[Production].[dbo].Factory f WITH(NOLOCK) on o.FactoryID = f.ID
		where (o.ScIDelivery between @Date_S and @Date_E or o.BuyerDelivery BETWEEN @YearMonth_S and @YearMonth_E)
		and SubconInType in ('1','2')
		and o.Junk = 0
		and o.LocalOrder = 1
		and f.Foundry = 1
		and f.IsProduceFty = 0


	insert into [P_PPICMasterList_ArtworkType] ([SP#], [FactoryID], [ArtworkTypeNo], [ArtworkType], [Value], [TotalValue], [ArtworkTypeUnit], [SubconInTypeID], [ArtworkTypeKey], [OrderDataKey])
	select t.ID, t.FactoryID, t.Seq, t.ArtworkType, ISNULL(t.[Value], 0), ISNULL(t.TTL_Value, 0), t.Unit, t.SubconInType, t.ArtworkTypeKey, t.OrderDataKey
	from #tmp t
	where not exists (select 1 from P_PPICMasterList_ArtworkType p where t.ID = p.[SP#] and t.FactoryID = p.[FactoryID] and t.SubconInType = p.[SubconInTypeID] and t.ArtworkTypeKey = p.[ArtworkTypeKey])
	order by ID, ArtworkType, SEQ

	update p
		set p.[ArtworkTypeNo] = t.Seq
			, p.[ArtworkType] = t.ArtworkType
			, p.[Value] = ISNULL(t.[Value], 0)
			, p.[TotalValue] = ISNULL(t.TTL_Value, 0)
			, p.[ArtworkTypeUnit] = t.Unit
			, p.[OrderDataKey] = t.OrderDataKey
	from P_PPICMasterList_ArtworkType p
	inner join #tmp t on t.ID = p.[SP#] and t.FactoryID = p.[FactoryID] and t.SubconInType = p.[SubconInTypeID] and t.ArtworkTypeKey = p.[ArtworkTypeKey]

	DROP TABLE #tmp, #UseArtworkType

	IF EXISTS (select 1 from BITableInfo b where b.id = 'P_PPICMasterList_ArtworkType')
	BEGIN
		update b
			set b.TransferDate = getdate()
		from BITableInfo b
		where b.id = 'P_PPICMasterList_ArtworkType'
	END
	ELSE 
	BEGIN
		insert into BITableInfo(Id, TransferDate)
		values('P_PPICMasterList_ArtworkType', getdate())
	END

END
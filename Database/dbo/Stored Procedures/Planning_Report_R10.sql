
CREATE PROCEDURE [dbo].[Planning_Report_R10]
	@ReportType int = 1 --1:整個月 2:半個月	--3:Production status 不做
	,@BrandID varchar(20) --= ''
	,@ArtWorkType varchar(20) --= 'SEWING'
	,@isSCIDelivery bit = 1
	,@Year int = 2017
	,@Month int = 1
	,@SourceStr varchar(50) = 'Order,Forecast,Fty Local Order'
	,@MDivisionID varchar(8) = ''
	,@Fty varchar(20) = ''
	,@HideFoundry bit = 1
	,@Zone varchar(8) = ''
	,@CalculateCPU bit = 0
	,@CalculateByBrand bit = 0
	,@IncludeCancelOrder bit = 0
AS
BEGIN
	/*
	Factory_Tms的Tms依照ArtworkTypeID不同而有不一樣的基準
	1.Sewing的基準是小時若要算成CPU則需乘上3600在除1400
	2.Stitch, Qty的基準是個數不用特別轉換單位
	3.除此之外的單位是分鐘所以不用轉換
	
	Style_TmsCost、Order_TmsCost的Tms全部都是以秒為單位(包含Sewing)所以要除60，除了Stitch跟Qty

	CPU => 秒 / 1400
	Stitch => Qty / 1000
	Qty => Qty
	*/
	
	set transaction isolation level read uncommitted

	declare @HasOrders bit = 0, @HasForecast bit = 0, @HasFtyLocalOrder bit = 0
	set @HasOrders = iif(exists(select 1 from dbo.SplitString(@SourceStr,',') where Data = 'Order'), 1, 0)
	set @HasForecast = iif(exists(select 1 from dbo.SplitString(@SourceStr,',') where Data = 'Forecast'), 1, 0)
	set @HasFtyLocalOrder = iif(exists(select 1 from dbo.SplitString(@SourceStr,',') where Data = 'Fty Local Order'), 1, 0)

	IF (@ArtWorkType = 'SEWING')
	BEGIN
		set @CalculateCPU = 1;
	END
	
	IF ((select ArtworkUnit from ArtworkType where id = @ArtWorkType) ='STITCH' 
		OR (select ProductionUnit from ArtworkType where id = @ArtWorkType) = 'QTY')
	BEGIN
		set @CalculateCPU = 0;
	END
	
	declare @mStandardTMS int = (select StdTMS from System) --1400
	declare @mSampleCPURate int = (select SampleRate from System) --1
	declare @date_s date = DATEFROMPARTS(@Year, 1, 8)--DATEFROMPARTS(@Year, 1, 1)
	declare @date_e date = DATEFROMPARTS(@Year + 1, 1, 7)--DATEFROMPARTS(@Year, 12, 31)
	declare @date_s_by date = DATEFROMPARTS(@Year, 1, 1)--DATEFROMPARTS(@Year, 1, 1)
	declare @date_e_by date = DATEFROMPARTS(@Year, 12, 31)--DATEFROMPARTS(@Year, 12, 31)
	if(@ReportType = 2)
	begin
		set @date_s = DATEFROMPARTS(@Year, @Month, 8)
		set @date_e = dateadd(day,-1,DATEADD(MONTH,6,@date_s))
		set @date_s_by = DATEFROMPARTS(@Year, @Month, 1)
		set @date_e_by = dateadd(day,-1,DATEADD(MONTH,6,@date_s_by))
	end
	
	---------------------------------------------------------------------------------------------------------------------------------
	
	SELECT CountryID, Factory.CountryID + '-' + Country.Alias as CountryName , Factory.ID as FactoryID
		, iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID
		, Factory.CPU
		, Factory_TMS.Year, Factory_TMS.Month, Factory_TMS.ArtworkTypeID, Factory_TMS.TMS 
		, Loading
		, Round(Loading * fw.HalfMonth1 / (fw.HalfMonth1 + fw.HalfMonth2),0) as HalfLoading1
		, Round(Loading * fw.HalfMonth2 / (fw.HalfMonth1 + fw.HalfMonth2),0) as HalfLoading2
		, iif(@ReportType = 1, Date1, Date2) as OrderYYMM
		, Factory.FactorySort
	into #tmpFactory From Factory
	inner join Country on Factory.CountryID = Country.ID
	left join Factory_TMS on Factory.ID = Factory_Tms.ID
		And ((@ReportType = 1 and Factory_Tms.Year = @Year)
		or (@ReportType = 2 and DATEFROMPARTS(Factory_Tms.Year,Factory_Tms.Month,8) between @date_s and @date_e))
	outer apply (select DATEFROMPARTS(Factory_Tms.Year,Factory_Tms.Month,8) as OrderDate) od
	left join ArtworkType on ArtworkType.Id = Factory_TMS.ArtworkTypeID
	left join Factory_WorkHour fw on Factory.ID = fw.ID and fw.Year = Factory_TMS.Year and fw.Month = Factory_Tms.Month	
	outer apply (select IIF(@ArtWorkType = 'Sewing', ROUND(Factory_TMS.Tms * 3600 / @mStandardTMS, 0),
						iif(ArtworkType.ArtworkUnit = 'STITCH', Factory_TMS.Tms,
						iif(ArtworkType.ProductionUnit = 'Qty', Factory_TMS.Tms,
						IIF(@CalculateCPU = 1, ROUND(Factory_Tms.Tms * 60 / @mStandardTMS, 0), Factory_TMS.TMS )))) as Loading) cc
	outer apply (select format(dateadd(day,-7,OrderDate),'yyyyMM') as Date1) odd1
	outer apply (select cast(Factory_TMS.Year as varchar(4)) + cast(Factory_TMS.Month as varchar(2)) as Date2) odd2
	Where ISsci = 1 /* And Factory.Junk = 0 */ And Artworktype.ReportDropdown = 1 
	And Artworktype.ID = @ArtWorkType
	And (@MDivisionID = '' or Factory.MDivisionID = @MDivisionID)
	And (@Fty = '' or Factory.ID = @Fty )
	And (@Zone = '' or Factory.Zone = @Zone)
	and Factory.IsProduceFty = 1

	---------------------------------------------------------------------------------------------------------------------------------
	--Order
	
	select Orders.id, FactoryID, Orders.CPU, OrderTypeID, ProgramID, Qty, Category, BrandID, BuyerDelivery, SciDelivery, CpuRate, SewLastDate, GMTComplete
		, Factory.Zone,Orders.MDivisionID
	into #Orders From Orders
	inner join Factory on Orders.FactoryID = Factory.ID	
	outer apply (select CpuRate from GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'O') ) gcRate
	outer apply (select SewLastDate = MAX(OutputDate) from SewingOutput s join SewingOutput_Detail sd on s.ID = sd.ID where sd.OrderId = Orders.ID )so -- 在trade是 Orders.SewLastDate (從PMS轉過去)
	Where ((@isSCIDelivery = 0 and Orders.BuyerDelivery between @date_s_by and @date_e_by)
	or (@isSCIDelivery = 1 and Orders.SciDelivery between @date_s and @date_e))
	And (@BrandID = '' or Orders.BrandID = @BrandID)
	And (Orders.Junk = 0 OR (@IncludeCancelOrder = 1 AND (Orders.Junk = 1 AND Orders.NeedProduction = 1))) 
	AND (Orders.IsBuyBack = 0 OR (Orders.IsBuyBack = 1 AND Orders.BuyBackReason = 'KeepPanel'))
	and Orders.Qty > 0  And Orders.Category in ('B','S') 
	AND @HasOrders = 1
	And localorder = 0
	and Factory.IsProduceFty = 1
	
	Select Orders.ID
	, rtrim(iif(Factory.FactorySort = '999', Factory.KpiCode, Factory.ID)) as FactoryID
	, CPURate
	, iif(Factory.Type = 'S', 'Sample', Orders.MDivisionID) as MDivisionID
	, Factory.CountryID	
	, Orders.CPU, cTms, cCPU
	, Order_TmsCost.ArtworktypeID
	, Orders.Qty as OrderQty
	, (cCPU * Orders.Qty * CpuRate) as OrderCapacity
	, (cCPU * iif(Orders.GMTComplete = 'S', Orders.Qty - GetPulloutData.Qty, 0) * CpuRate) as OrderShortage
	, iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	, OrderDate ,FactorySort
	, SewOutputQty as QAQty
	, SewLastDate as OutputDate
	, iif(@ReportType = 1, sDate1, sDate2) as SewingYYMM
	, SewLastDate as SewingYYMM_Ori
	, (cCPU * SewOutputQty * CPURate) as SewCapacity
	, Orders.Zone
	, BrandID
	into #tmpOrder1 from #Orders Orders
	inner join Factory on Orders.FactoryID = Factory.ID
	left Join Order_TmsCost on Orders.ID = Order_TmsCost.ID And Order_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = @ArtWorkType
	outer apply (select SewOutputQty = dbo.GetSewingQtybyRate(Orders.ID, null, null ) ) oq -- PMS無實體欄位, 用此Function取得
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Order_TmsCost.Qty / 1000,
						iif(ArtworkType.ArtworkUnit = 'PPU', Order_TmsCost.Price,
						iif(ArtworkType.ProductionUnit = 'Qty', Order_TmsCost.Qty,
						IIF( @CalculateCPU = 1, Order_TmsCost.Tms / @mStandardTMS, Order_TmsCost.Tms / 60 )))) as cTms) amt
	outer apply (select iif(@ArtWorkType = 'SEWING', Orders.CPU, cTms) as cCPU) ccpu
	outer apply (select iif(@isSCIDelivery = 0, Orders.BuyerDelivery, Orders.SCIDelivery) as OrderDate) odd
	outer apply (select format(dateadd(day,iif(@isSCIDelivery = 0, 0, -7),OrderDate),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(OrderDate, @isSCIDelivery) as Date2) odd2
	outer apply (select format(dateadd(day,iif(@isSCIDelivery = 0, 0, -7),SewLastDate),'yyyyMM') as sDate1) sodd1
	outer apply (select dbo.GetHalfMonWithYear(SewLastDate, @isSCIDelivery) as sDate2) sodd2
	outer apply (select Qty=sum(shipQty) from Pullout_Detail where orderid = Orders.id) GetPulloutData -- 出貨數量, PMS此處不用Function, 直接加總
	
	---------------------------------------------------------------------------------------------------------------------------------
	--Fty Local Order
	select Orders.ID, Orders.CPU, Qty
		, iif(Factory.FactorySort = '999', Factory.KpiCode, Factory.ID) as FactoryID
		, BuyerDelivery, SCIDelivery, SewOutputQty, SewLastDate
		, SubconInSisterFty, SubconInType, ProgramID, BrandID, StyleID, SeasonID
		, Factory.Zone
	into #FactoryOrder 
	from Orders 
	inner join Factory on Orders.FactoryID = Factory.ID
	outer apply (select SewOutputQty = dbo.GetSewingQtybyRate(Orders.ID, null, null ) ) oq -- PMS無實體欄位, 用此Function取得
	outer apply (select SewLastDate = MAX(OutputDate) from SewingOutput s join SewingOutput_Detail sd on s.ID = sd.ID where sd.OrderId = Orders.ID )so -- 在trade是 Orders.SewLastDate (從PMS轉過去)
	Where ((@isSCIDelivery = 0 and Orders.BuyerDelivery between @date_s_by and @date_e_by)
	or (@isSCIDelivery = 1 and Orders.SciDelivery between @date_s and @date_e))
	And (@BrandID = '' or Orders.BrandID = @BrandID)
	And Orders.Junk = 0 and Orders.Qty > 0	
	AND @HasFtyLocalOrder = 1
	AND Orders.LocalOrder = 1 -- PMS此處才加, 當地訂單在trade是記錄在Table:FactoryOrder
	AND Orders.IsForecast = 0
	and Factory.IsProduceFty = 1

	Select FactoryOrder.ID, rtrim(FactoryOrder.FactoryID) as FactoryID
	, iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID
	, iif(f2.Type = 'S', 'Sample', f2.MDivisionID) as MDivisionID2
	, f2.Zone as Zone2
	, Factory.CountryID
	, f2.CountryID as CountryID2
	, Style.CPU, cTms, cCPU
	, Style_TmsCost.ArtworkTypeID 
	, Style_TmsCost.TMS as ArtworkTypeTMS 
	, FactoryOrder.Qty as OrderQty
	, CPURate
	, (cCPU * FactoryOrder.Qty * CPURate) as FactoryOrderCapacity
	, FactoryOrder.BuyerDelivery
	, iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	, Factory.FactorySort, SewOutputQty, SewLastDate
	, SewOutputQty as QAQty
	, SewLastDate as OutputDate
	, iif(@ReportType = 1, sDate1, sDate2) as SewingYYMM
	, SewLastDate as SewingYYMM_Ori
	, (cCPU * SewOutputQty * CpuRate) as SewCapacity
	, SubconInSisterFty
	, SubconInType
	, iif(f2.FactorySort = '999', f2.KpiCode, f2.ID) as ProgramID
	, FactoryOrder.Zone
	, FactoryOrder.BrandID
	into #tmpFactoryOrder1 from #FactoryOrder FactoryOrder
	inner join Factory on FactoryOrder.FactoryID = Factory.ID
	left join Factory f2 on FactoryOrder.ProgramID = f2.ID
	left join Style on Style.BrandID = FactoryOrder.BrandID AND Style.SeasonID = FactoryOrder.SeasonID AND Style.ID = FactoryOrder.StyleID
	left join Style_TmsCost on Style.UKey = Style_TMSCost.StyleUkey And Style_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = @ArtWorkType
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Style_TMSCost.Qty / 1000,
						iif(ArtworkType.ArtworkUnit = 'PPU', Style_TmsCost.Price,
						iif(ArtworkType.ProductionUnit = 'Qty', Style_TMSCost.Qty,
						IIF(@CalculateCPU = 1, Style_TMSCost.Tms / @mStandardTMS, Style_TMSCost.Tms / 60 )))) as cTms) amt
	outer apply (select iif(@ArtWorkType = 'SEWING', FactoryOrder.CPU, cTms) as cCPU) ccpu
	outer apply (select 1 as CpuRate ) gcRate
	outer apply (select iif(@isSCIDelivery = 0, FactoryOrder.BuyerDelivery, FactoryOrder.SCIDelivery) as OrderDate) odd
	outer apply (select format(dateadd(day,iif(@isSCIDelivery = 0, 0, -7),OrderDate),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(OrderDate, @isSCIDelivery) as Date2) odd2
	outer apply (select format(dateadd(day,iif(@isSCIDelivery = 0, 0, -7),SewLastDate),'yyyyMM') as sDate1) sodd1
	outer apply (select dbo.GetHalfMonWithYear(SewLastDate, @isSCIDelivery) as sDate2) sodd2
	
	---------------------------------------------------------------------------------------------------------------------------------
	--Forecast
	Select Orders.ID
	, rtrim(iif(Factory.FactorySort = '999', Factory.KpiCode, Factory.ID)) as FactoryID
	, iif(Factory.Type = 'S', 'Sample', Orders.MDivisionID) as MDivisionID
	, Factory.CountryID
	, cTms as ArtworkTypeTMS
	, Style.CPU, cTms, cCPU
	, Orders.Qty as ForecastQty
	, Style_TmsCost.ArtworkTypeID
	, CpuRate
	, (cCPU * Orders.Qty * CpuRate) as ForecastCapacity
	, Orders.BuyerDelivery
	, iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	, FactorySort
	, Orders.BrandID
	, Factory.Zone
	into #tmpForecast1
	from Orders
	left join Factory on Factory.ID = Orders.FactoryID
	left join Style on Style.BrandID = Orders.BrandID AND Style.SeasonID = Orders.SeasonID AND Style.ID = Orders.StyleID
	left join Style_TmsCost on Style.UKey = Style_TMSCost.StyleUkey And Style_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = @ArtWorkType
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Style_TMSCost.Qty / 1000,
						iif(ArtworkType.ArtworkUnit = 'PPU', Style_TmsCost.Price,
						iif(ArtworkType.ProductionUnit = 'Qty', Style_TMSCost.Qty,
						IIF(@CalculateCPU = 1, Style_TMSCost.Tms / @mStandardTMS, Style_TMSCost.Tms / 60 )))) as cTms) amt
	outer apply (select iif(@ArtWorkType = 'SEWING', IIF(Orders.Category = 'B', Style.CPU, Orders.CPU), cTms) as cCPU) ccpu
	outer apply (select CpuRate from dbo.GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'S') ) gcRate
	outer apply (select format(dateadd(day,iif(@isSCIDelivery = 0, 0, -7),Orders.BuyerDelivery),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Orders.BuyerDelivery, @isSCIDelivery) as Date2) odd2
	where ((@isSCIDelivery = 0 and Orders.BuyerDelivery between @date_s_by and @date_e_by)
	or (@isSCIDelivery = 1 and Orders.BuyerDelivery between @date_s and @date_e))
	And (@BrandID = '' or Orders.BrandID = @BrandID)
	And Orders.Qty > 0
	AND @HasForecast = 1
	And localorder = 0
	AND Orders.IsForecast = 1 -- PMS此處才加, 預估單 在trade是記錄在Table:FactoryOrder
	and Factory.IsProduceFty = 1
	and (Orders.SciDelivery <= dateadd(m, datediff(m,0,dateadd(m, 5, GETDATE())),6) or Orders.BuyerDelivery <= dateadd(m, datediff(m,0,dateadd(m, 5, GETDATE())),6))

	---------------------------------------------------------------------------------------------------------------------------------
	
	declare @tmpFinal table (
		CountryID varchar(2)
		,MDivisionID varchar(8)
		,FactoryID varchar(10)
		,OrderYYMM varchar(10)
		,OrderLoadingCPU numeric(14,2)
		,OrderShortage numeric(14,2)
		,MaxOutputDate date
		,MinOutPutDate date
		,FactorySort varchar(3)
		,BrandID varchar(8)
	)

	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID, #tmpOrder1.OrderYYMM
	,sum(OrderCapacity) as OrderLoadingCPU
	,sum(OrderShortage) as OrderShortage
	,Max(OutPutDate) as MaxOutputDate, Min(OutPutDate) as MinOutPutDate
	,FactorySort
	,BrandID
	From #tmpOrder1 Group by CountryID,MDivisionID,FactoryID,#tmpOrder1.OrderYYMM,FactorySort,BrandID

	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID, #tmpForecast1.OrderYYMM
	,sum(ForecastCapacity) as OrderLoadingCPU
	,0 as OrderShortage
	,null as MaxOutputDate, null as MinOutPutDate
	,FactorySort
	,BrandID
	From #tmpForecast1 Group by CountryID,MDivisionID,FactoryID,#tmpForecast1.OrderYYMM,FactorySort,BrandID

	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID, #tmpFactoryOrder1.OrderYYMM
	,sum(FactoryOrderCapacity) as OrderLoadingCPU
	,0 as OrderShortage
	,Max(OutPutDate) as MaxOutputDate, Min(OutPutDate) as MinOutPutDate
	,FactorySort
	,BrandID
	From #tmpFactoryOrder1
	WHERE SubconInSisterFty = 0
	Group by CountryID,MDivisionID,FactoryID,#tmpFactoryOrder1.OrderYYMM,FactorySort,BrandID

	select 
		CountryID
		, MDivisionID
		, FactoryID
		, OrderYYMM
		, sum(OrderLoadingCPU) as OrderLoadingCPU
		, sum(OrderShortage) as OrderShortage
		, Max(MaxOutputDate) as MaxOutputDate
		, Min(MinOutPutDate) as MinOutPutDate
		,FactorySort
		,BrandID
	into #tmpFinal from @tmpFinal 
	group by CountryID,MDivisionID,FactoryID,OrderYYMM,FactorySort,BrandID
	
	if(@ReportType = 1)
	Begin
	--Report1 : 每個月區間為某一整年----------------------------------------------------------------------------------------------------------------------------------
		CREATE TABLE #tmpFtyList
		(
			CountryID varchar(2)
			, CountryName varchar(50)
			, FactoryID varchar(8)
			, MDivisionID varchar(8)
			, Type varchar(1)
			, FactorySort varchar(3)
		)

		select distinct 
			CountryID
			, CountryName = Factory.CountryID + '-' + Country.Alias
			, fty.FactoryID
			, MDivisionID = iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID)
			, BrandID
			, Type
			, FactorySort
		into #tmpFtyList1
		from (
			select FactoryID, BrandID from #tmpOrder1 where isnull(OrderCapacity, 0) != 0 union
			select FactoryID, BrandID from #tmpForecast1 where isnull(ForecastCapacity, 0) != 0
		) fty
		inner join Factory on Factory.ID = fty.FactoryID
		inner join Country on Factory.CountryID = Country.ID
		where Type in ('B','S') and isnull(FactorySort,'') <> ''		
		And (@MDivisionID = '' or Factory.MDivisionID = @MDivisionID)
		And (@Fty = '' or Factory.ID = @Fty) -- PMS這才有
		And (@Zone = '' or Factory.Zone = @Zone)
		AND Factory.IsProduceFty = 1
		order by FactorySort

		insert into #tmpFtyList1
		select
			CountryID
			, CountryName = Factory.CountryID + '-' + Country.Alias
			, iif(Factory.FactorySort = '999', Factory.KpiCode, Factory.ID) as FactoryID
			, MDivisionID = iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID)
			, BrandID = ''
			, Type
			, FactorySort
		from Factory
		inner join Country on Factory.CountryID = Country.ID
		WHERE Type in ('B','S') and isnull(FactorySort,'') <> ''
		And (@MDivisionID = '' or Factory.MDivisionID = @MDivisionID)
		And (@Fty = '' or Factory.ID = @Fty) -- PMS這才有
		And (@Zone = '' or Factory.Zone = @Zone)
		and (@HideFoundry = 0 or (@HideFoundry = 1 and isnull(Foundry,0) = 0))
		AND Factory.IsProduceFty = 1

		if(@CalculateByBrand = 0)
		BEGIN
			--By 所有 CountryID, MDisision, FactoryID
			insert into #tmpFtyList
			select CountryID, CountryName, FactoryID, MDivisionID, Type, FactorySort from #tmpFtyList1
			where FactoryID != ''
			group by CountryID, CountryName, FactoryID, MDivisionID, Type, FactorySort
		END
		ELSE
		BEGIN
			insert into #tmpFtyList
			select CountryID, CountryName, BrandID, MDivisionID, Type, FactorySort from #tmpFtyList1
			where BrandID != ''
			group by CountryID, CountryName, BrandID, MDivisionID, Type, FactorySort
		END

		select * from #tmpFtyList ORDER BY FactorySort, FactoryID

		--(A)+(B)By MDivisionID (C)By Factory
		select 
			a.CountryID, a.MDivisionID, a.tmpBrandFty as FactoryID, a.OrderYYMM as Month, a.CountryName, Zone
			, sum(a.Capacity) as Capacity
			, sum(a.Tms) as Tms
			, sum(a.FtyTmsCapa) as FtyTmsCapa
			, sum(a.OrderShortage) as OrderShortage
			, sum(a.Capacity - a.OrderShortage) as trueCapa
		from (
			select * from 
			(
				select 
					Factory.CountryID, c.MDivisionID, tmpBrandFty, FactoryID, OrderYYMM
					, CountryName = Factory.CountryID + '-' + Country.Alias
					, SUM(OrderCapacity) as Capacity, SUM(FtyTmsCapa) as FtyTmsCapa, SUM(OrderShortage) as OrderShortage, Factory.Zone
				from (
					select CountryID, MDivisionID, FactoryID, iif(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty, OrderYYMM, OrderCapacity, 0 as FtyTmsCapa, OrderShortage from #tmpOrder1 union all
					select CountryID, MDivisionID, FactoryID, iif(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty, OrderYYMM, ForecastCapacity,	0, 0 from #tmpForecast1 union all
					select CountryID, MDivisionID, FactoryID, iif(@CalculateByBrand = 1, '', FactoryID) as tmpBrandFty, OrderYYMM, 0,	Loading as FtyTmsCapa, 0 from #tmpFactory
				) c
				inner join Factory on c.FactoryID = Factory.ID
				inner join Country on Factory.CountryID = Country.ID
				where c.FactoryID in (select FactoryID from #tmpFtyList1)
				AND Factory.IsProduceFty = 1
				GROUP BY Factory.CountryID, c.MDivisionID, tmpBrandFty, FactoryID, OrderYYMM, Country.Alias, Factory.Zone
			) tmpC
			left join 
			(
				select Factory_Tms.ID, SUM(cc.Capacity) as Tms 
				from Factory_Tms
				left join ArtworkType on ArtworkType.Id = Factory_TMS.ArtworkTypeID
				outer apply (select IIF(@ArtWorkType = 'Sewing', ROUND(Factory_TMS.Tms * 3600 / @mStandardTMS, 0),
							iif(ArtworkType.ArtworkUnit = 'STITCH', Factory_TMS.Tms / 1000,
							iif(ArtworkType.ProductionUnit = 'Qty', Factory_TMS.Tms,
							IIF(@CalculateCPU = 1, ROUND(Factory_Tms.Tms * 60 / @mStandardTMS, 0), Factory_TMS.TMS )))) as Capacity) cc
				where YEAR = @Year and ArtworkTypeID = @ArtWorkType
				GROUP BY Factory_Tms.ID
			) d on tmpC.FactoryID = d.ID
		) a
		group by a.CountryID, a.MDivisionID, a.tmpBrandFty, a.OrderYYMM, a.CountryName, Zone
		ORDER BY a.CountryID, a.MDivisionID, a.tmpBrandFty

		--For Forecast shared
		select Zone, CountryID, MDivisionID, iif(@CalculateByBrand = 1, BrandID, FactoryID) as FactoryID, OrderYYMM as Month, sum(ForecastCapacity) as Capacity 
		from #tmpForecast1 
		group by Zone, CountryID, MDivisionID, iif(@CalculateByBrand = 1, BrandID, FactoryID), OrderYYMM
		
		--For Output, 及Output後面的Max日期
		select 
			CountryID, c.CountryID2, MDivisionID, MDivisionID2, tmpBrandFty as FactoryID, tmpBrandFty2 as FactoryID2
			, max(format(SewingYYMM_Ori,'yyyy/MM/dd')) as SewingYYMM, OrderYYMM as Month
			, sum(SewCapacity) as SewCapacity
			, sum(OrderCapacity) as OrderCapacity
			, SubconInType 
			, SubconInSisterFty
			, c.isOrder
			, c.Zone
			, c.Zone2
		from (
			Select CountryID, '' as CountryID2, MDivisionID, '' as MDivisionID2, IIF(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty, '' as tmpBrandFty2, SewingYYMM_Ori, OrderYYMM, SewCapacity, OrderCapacity, '0' as SubconInType, 0 AS SubconInSisterFty, isOrder = 1, Zone, '' as Zone2 from #tmpOrder1
			union ALL 
			Select CountryID, CountryID2, MDivisionID, MDivisionID2, IIF(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty, IIF(@CalculateByBrand = 1, BrandID, ProgramID) as tmpBrandFty2, SewingYYMM_Ori, OrderYYMM, SewCapacity, FactoryOrderCapacity, SubconInType, SubconInSisterFty, isOrder = 0, Zone, Zone2 from #tmpFactoryOrder1 --WHERE SubconInSisterFty = 0
		) c
		--where SewingYYMM_Ori is not null
		group by CountryID,c.CountryID2,MDivisionID,MDivisionID2,tmpBrandFty,tmpBrandFty2,OrderYYMM,SubconInType, SubconInSisterFty, c.isOrder, c.Zone, c.Zone2

	End
	else if(@ReportType = 2)
	Begin
	--Report2 : 每半個月，區間為設定的年月往後推半年----------------------------------------------------------------------------------------------------------------------------------
		
		--By 所有 CountryID, MDisision, FactoryID
		--select CountryID,MDivisionID,FactoryID,FactorySort from #tmpFinal group by CountryID,MDivisionID,FactoryID,FactorySort
		--order by FactorySort

		CREATE TABLE #tmpFtyList2
		(
			CountryID varchar(2)
			, CountryName varchar(50)
			, FactoryID varchar(8)
			, MDivisionID varchar(8)
			, Type varchar(1)
			, FactorySort varchar(3)
		)

		select distinct 
			CountryID
			, CountryName = Factory.CountryID + '-' + Country.Alias
			, fty.FactoryID
			, MDivisionID = iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID)
			, BrandID
			, Type
			, FactorySort
		into #tmpFtyList3
		from (
			select BrandID = '', FactoryID from #tmpFactory ft where ft.HalfLoading1 + ft.HalfLoading2 > 0 union
			select ft.BrandID, FactoryID from #tmpFinal ft where ft.OrderLoadingCPU > 0 union
			select BrandID, FactoryID from #tmpFactoryOrder1 --#tmpFactoryOrder1的Brand跟Factory在#tmpFinal的時候只有篩選SubconInSisterFty = 0，所以在此重新加入
		) fty
		inner join Factory on Factory.ID = fty.FactoryID
		inner join Country on Factory.CountryID = Country.ID
		where Type in ('B','S') and isnull(FactorySort,'') <> ''		
		And (@MDivisionID = '' or Factory.MDivisionID = @MDivisionID)
		And (@Fty = '' or Factory.ID = @Fty) -- PMS這才有
		And (@Zone = '' or Factory.Zone = @Zone)		
		AND Factory.IsProduceFty = 1
		order by FactorySort

		insert into #tmpFtyList3
		select
			CountryID
			, CountryName = Factory.CountryID + '-' + Country.Alias
			, iif(Factory.FactorySort = '999', Factory.KpiCode, Factory.ID) as FactoryID
			, MDivisionID = iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID)
			, BrandID = ''
			, Type
			, FactorySort		
		from Factory
		inner join Country on Factory.CountryID = Country.ID
		WHERE Type in ('B','S') and isnull(FactorySort,'') <> '' 
		And (@MDivisionID = '' or Factory.MDivisionID = @MDivisionID)
		And (@Fty = '' or Factory.ID = @Fty) -- PMS這才有
		And (@Zone = '' or Factory.Zone = @Zone)	
		and (@HideFoundry = 0 or (@HideFoundry = 1 and isnull(Foundry,0) = 0))
		AND Factory.IsProduceFty = 1

		if(@CalculateByBrand = 0)
		BEGIN
			--By 所有 CountryID, MDisision, FactoryID
			insert into #tmpFtyList2
			select CountryID, CountryName, FactoryID, MDivisionID, Type, FactorySort from #tmpFtyList3
			where FactoryID != ''
			group by CountryID, CountryName, FactoryID, MDivisionID, Type, FactorySort
		END
		ELSE
		BEGIN
			insert into #tmpFtyList2
			select CountryID, CountryName, BrandID, MDivisionID, Type, FactorySort from #tmpFtyList3
			where BrandID != ''
			group by CountryID, CountryName, BrandID, MDivisionID, Type, FactorySort
		END

		select * from #tmpFtyList2 ORDER BY FactorySort, FactoryID

		/*select distinct CountryID, IIF(@CalculateByBrand = 1, '', Factory.ID) as FactoryID, Factory.FactorySort
		, iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID
		into #tmpFtyList2
		from Factory
		inner join Country on Factory.CountryID = Country.ID
		where Type in ('B','S') and isnull(FactorySort,'') <> ''
		and (Factory.ID in (
			select distinct IIF(@CalculateByBrand = 1, '', ft.FactoryID) FactoryID from #tmpFactory ft where ft.HalfLoading1 + ft.HalfLoading2 > 0 AND @CalculateByBrand = 0
			union
			select distinct IIF(@CalculateByBrand = 1, ft.BrandID, ft.FactoryID) FactoryID from #tmpFinal ft where ft.OrderLoadingCPU > 0
		) OR
		EXISTS (select tmp.ID from Factory tmp WHERE (@HideFoundry = 0 or (@HideFoundry = 1 and tmp.Foundry = 0)) and Factory.ID = tmp.ID ))
		And (@MDivisionID = '' or Factory.MDivisionID = @MDivisionID)
		And (@Zone = '' or Factory.Zone = @Zone)
		order by FactorySort

		select * from #tmpFtyList2 order by FactorySort*/

		----(K) By Factory 最細的上下半月Capacity
		--(L) By Factory Loading CPU
		select a.Zone, a.CountryID, a.MDivisionID, a.tmpBrandFty as FactoryID, a.OrderYYMM as MONTH, b.OrderLoadingCPU as Capacity1, c.OrderLoadingCPU as Capacity2, a.FtyTmsCapa1, a.FtyTmsCapa2
		,a.CountryName, b.OrderShortage as OrderShortage1, c.OrderShortage as OrderShortage2
		from (
			select 
				Factory.Zone,
				Factory.CountryID,
				ma.MDivisionID,
				tmpBrandFty,
				OrderYYMM,
				CountryName = Factory.CountryID + '-' + Country.Alias,
				sum(HalfLoading1) as FtyTmsCapa1,
				sum(HalfLoading2) as FtyTmsCapa2
			from (
				select MDivisionID, FactoryID, IIF(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty, substring(OrderYYMM,1,6) as OrderYYMM, 0 as HalfLoading1, 0 as HalfLoading2, FactorySort from #tmpFinal UNION ALL
				select MDivisionID, FactoryID, IIF(@CalculateByBrand = 1, '', FactoryID) as tmpBrandFty, OrderYYMM as Month, HalfLoading1, HalfLoading2, FactorySort from #tmpFactory WHERE @CalculateByBrand = 0
			) ma
			inner join Factory on ma.FactoryID = Factory.ID
			inner join Country on Factory.CountryID = Country.ID
			where ma.tmpBrandFty in (select FactoryID from #tmpFtyList2)
			group by Factory.Zone, Factory.CountryID, ma.MDivisionID, tmpBrandFty, OrderYYMM, Country.Alias
		) a 
		left join (
			select 
				CountryID
				, IIF(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty
				, MDivisionID
				, substring(OrderYYMM,1,6) as OrderYYMM
				, sum(OrderLoadingCPU) as OrderLoadingCPU
				, sum(OrderShortage) as OrderShortage
			from #tmpFinal where RIGHT(OrderYYMM,1) = 1 
			group by IIF(@CalculateByBrand = 1, BrandID, FactoryID), MDivisionID, substring(OrderYYMM,1,6), CountryID
		) b on a.CountryID = b.CountryID and a.tmpBrandFty = b.tmpBrandFty and a.MDivisionID = b.MDivisionID and substring(a.OrderYYMM,1,6) = b.OrderYYMM
		left join (
			select 
				CountryID
				, IIF(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty
				, MDivisionID
				, substring(OrderYYMM,1,6) as OrderYYMM
				, sum(OrderLoadingCPU) as OrderLoadingCPU
				, sum(OrderShortage) as OrderShortage
			from #tmpFinal where RIGHT(OrderYYMM,1) = 2 
			group by IIF(@CalculateByBrand = 1, BrandID, FactoryID), MDivisionID, substring(OrderYYMM,1,6), CountryID
		) c on a.CountryID = c.CountryID and a.tmpBrandFty = c.tmpBrandFty and a.MDivisionID = c.MDivisionID and substring(a.OrderYYMM,1,6) = c.OrderYYMM
		

		--For Forecast shared
		select a.Zone, a.CountryID, a.MDivisionID, a.tmpBrandFty as FactoryID, a.OrderYYMM as MONTH, sum(b.ForecastCapacity) as Capacity1, sum(c.ForecastCapacity) as Capacity2 from (
			select DISTINCT a.Zone, a.CountryID,a.MDivisionID,iif(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty,substring(OrderYYMM,1,6) as OrderYYMM from #tmpForecast1 a
		) a 
		left join (
			select Zone, CountryID, MDivisionID, iif(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty, substring(OrderYYMM,1,6) as OrderYYMM, sum(ForecastCapacity) ForecastCapacity from #tmpForecast1 WHERE RIGHT(OrderYYMM,1) = '1'
			group by Zone, CountryID,MDivisionID,iif(@CalculateByBrand = 1, BrandID, FactoryID),OrderYYMM
		) b on a.Zone = b.Zone and a.CountryID = b.CountryID and a.tmpBrandFty = b.tmpBrandFty and a.OrderYYMM = b.OrderYYMM and a.MDivisionID = b.MDivisionID
		left join (
			select Zone, CountryID, MDivisionID, iif(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty, substring(OrderYYMM,1,6) as OrderYYMM, sum(ForecastCapacity) ForecastCapacity from #tmpForecast1 WHERE RIGHT(OrderYYMM,1) = '2'
			group by Zone, CountryID,MDivisionID,iif(@CalculateByBrand = 1, BrandID, FactoryID),OrderYYMM
		) c on a.Zone = c.Zone and a.CountryID = c.CountryID and a.tmpBrandFty = c.tmpBrandFty and a.OrderYYMM = c.OrderYYMM and a.MDivisionID = c.MDivisionID
		group by a.Zone, a.CountryID,a.MDivisionID,a.OrderYYMM,a.tmpBrandFty


		--For Output, 及Output後面的Max日期
		select 
			CountryID, CountryID2, MDivisionID, MDivisionID2, tmpBrandFty as FactoryID, tmpBrandFty2 as FactoryID2
			, max(format(SewingYYMM_Ori,'yyyy/MM/dd')) as SewingYYMM, OrderYYMM as Month
			, sum(SewCapacity) as SewCapacity
			, sum(OrderCapacity) as OrderCapacity
			, SubconInType
			, SubconInSisterFty
			, c.isOrder
			, c.Zone
			, c.Zone2
		from (
			Select CountryID, '' as CountryID2, MDivisionID, '' as MDivisionID2, iif(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty, '' as tmpBrandFty2, SewingYYMM_Ori, OrderYYMM, SewCapacity, OrderCapacity, '0' as SubconInType, 0 AS SubconInSisterFty, isOrder = 1, Zone, '' as Zone2 from #tmpOrder1
			union ALL 
			Select CountryID, CountryID2, MDivisionID, MDivisionID2, iif(@CalculateByBrand = 1, BrandID, FactoryID) as tmpBrandFty, iif(@CalculateByBrand = 1, BrandID, ProgramID) as tmpBrandFty2, SewingYYMM_Ori, OrderYYMM, SewCapacity, FactoryOrderCapacity, SubconInType, SubconInSisterFty, isOrder = 0, Zone, Zone2 from #tmpFactoryOrder1 --WHERE SubconInSisterFty = 0
		) c
		--where SewingYYMM_Ori is not null
		group by CountryID, c.CountryID2, MDivisionID, MDivisionID2, tmpBrandFty, tmpBrandFty2, OrderYYMM, SubconInType, SubconInSisterFty, c.isOrder, c.Zone, c.Zone2
		
	End

	
drop table #tmpFactory
drop table #tmpOrder1
drop table #tmpFactoryOrder1
drop table #tmpForecast1
drop table #tmpFinal
drop table #Orders
drop table #FactoryOrder

If Object_ID('tempdb..#tmpFtyList') Is not Null
Begin
	drop table #tmpFtyList
End;

If Object_ID('tempdb..#tmpFtyList1') Is not Null
Begin
	drop table #tmpFtyList1
End;

If Object_ID('tempdb..#tmpFtyList2') Is not Null
Begin
	drop table #tmpFtyList2
End;

If Object_ID('tempdb..#tmpFtyList3') Is not Null
Begin
	drop table #tmpFtyList3
End;

set transaction isolation level read committed

END
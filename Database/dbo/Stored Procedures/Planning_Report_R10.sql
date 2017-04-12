

CREATE PROCEDURE [dbo].[Planning_Report_R10]
	@ReportType int = 1 --1:整個月 2:半個月 --3:Production status 2017.01.04 Serena電話確認後，表示不用做了
	,@BrandID varchar(20)
	,@M varchar(20) = ''
	,@Fty varchar(20) = ''
	,@ArtWorkType varchar(20) --= 'CPU'
	,@isSCIDelivery bit = 1
	,@Year int = 2017
	,@Month int = 1
	,@SourceStr varchar(50) = 'Order,Forecast,Fty Local Order'
AS
BEGIN

	declare @HasOrders bit = 0, @HasForecast bit = 0, @HasFtyLocalOrder bit = 0
	set @HasOrders = iif(exists(select 1 from dbo.SplitString(@SourceStr,',') where Data = 'Order'), 1, 0)
	set @HasForecast = iif(exists(select 1 from dbo.SplitString(@SourceStr,',') where Data = 'Forecast'), 1, 0)
	set @HasFtyLocalOrder = iif(exists(select 1 from dbo.SplitString(@SourceStr,',') where Data = 'Fty Local Order'), 1, 0)
	
	declare @CalArtWorkType varchar(30)
	set @CalArtWorkType = iif(@ArtWorkType = 'SEWING', 'CPU', @ArtWorkType)
	
	declare @mStandardTMS int = (select StdTMS from System) --1400
	declare @mSampleCPURate int = (select SampleRate from System) --1
	declare @date_s date = DATEFROMPARTS(@Year, 1, 8)--DATEFROMPARTS(@Year, 1, 1)
	declare @date_e date = DATEFROMPARTS(@Year+1, 1, 7)--DATEFROMPARTS(@Year, 12, 31)
	if(@ReportType = 2)
	begin
		set @date_s = DATEFROMPARTS(@Year, @Month, 8)
		set @date_e = dateadd(day,-1,DATEADD(MONTH,6,@date_s))
	end

	SELECT CountryID, Factory.CountryID + '-' + Country.Alias as CountryName , Factory.ID as FactoryID
		,iif(Factory.Zone <> '', Factory.Zone, iif(Factory.Type = 'S', 'Sample', Factory.Zone)) as MDivisionID
	,Factory.CPU
	,Factory_TMS.Year, Factory_TMS.Month, Factory_TMS.ArtworkTypeID, Factory_TMS.TMS 
	,Capacity
	,Capacity * fw.HalfMonth1 / (fw.HalfMonth1 + fw.HalfMonth2) as HalfCapacity1
	,Capacity * fw.HalfMonth2 / (fw.HalfMonth1 + fw.HalfMonth2) as HalfCapacity2
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	,Factory.FactorySort
	into #tmpFactory From Factory
	inner join Country on Factory.CountryID = Country.ID
	left join Factory_TMS on Factory.ID = Factory_Tms.ID
		And ((@ReportType = 1 and Factory_Tms.Year = @Year)
		or (@ReportType = 2 and DATEFROMPARTS(Factory_Tms.Year,Factory_Tms.Month,8) between @date_s and @date_e))
	outer apply (select DATEFROMPARTS(Factory_Tms.Year,Factory_Tms.Month,8) as OrderDate) od
	left join ArtworkType on ArtworkType.Id = Factory_TMS.ArtworkTypeID
	left join Factory_WorkHour fw on Factory.ID = fw.ID and fw.Year = Factory_TMS.Year and fw.Month = Factory_Tms.Month	
	outer apply (select iif(@CalArtWorkType = 'CPU', Round(convert(numeric(10,0),Factory_Tms.TMS) * 3600 / @mStandardTMS ,0), convert(numeric(10,0),Factory_Tms.TMS)) as Capacity) cc
	outer apply (select format(dateadd(day,-7,OrderDate),'yyyyMM') as Date1) odd1
	outer apply (select cast(Factory_TMS.Year as varchar(4)) + cast(Factory_TMS.Month as varchar(2)) as Date2) odd2
	Where ISsci = 1  And Factory.Junk = 0 And Artworktype.ReportDropdown = 1 
	And Artworktype.ID = @ArtWorkType And (Factory.MDivisionID = @M or @M = '') And (Factory.ID = @Fty or @Fty = '')

	select orders.id,FactoryID,orders.CPU,OrderTypeID,ProgramID,Qty,Category,BrandID,BuyerDelivery,SciDelivery,CpuRate
	into #Orders From orders inner join Factory on Orders.FactoryID = Factory.ID
	outer apply (select CpuRate from GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'O') ) gcRate
	Where ((@isSCIDelivery = 0 and Orders.BuyerDelivery between @date_s and @date_e)
	or (@isSCIDelivery = 1 and Orders.SciDelivery between @date_s and @date_e))
	And (@BrandID = '' or Orders.BrandID = @BrandID)
	And Orders.Junk = 0 and Orders.Qty > 0  And Orders.Category in ('B','S') 
	AND @HasOrders = 1 
	And (Factory.MDivisionID = @M or @M = '') And (Factory.ID = @Fty or @Fty = '')
	
	--Order
	Select Orders.ID, rtrim(Orders.FactoryID) as FactoryID, CPURate
		,iif(Factory.Zone <> '', Factory.Zone, iif(Factory.Type = 'S', 'Sample', Factory.Zone)) as MDivisionID
	, Factory.CountryID	
	,Orders.CPU, OrderTMSCPU, cCPU
	,Order_TmsCost.ArtworktypeID
	,Orders.Qty as OrderQty
	,(cCPU * Orders.Qty * CpuRate) as OrderCapacity
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	,OrderDate ,FactorySort
	into #tmpOrder1 from #Orders Orders
	inner join Factory on Orders.FactoryID = Factory.ID
	left Join Order_TmsCost on @CalArtWorkType != 'CPU' and Orders.ID = Order_TmsCost.ID And Order_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = Order_TmsCost.ArtworkTypeID
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Order_TmsCost.Qty / 1000, iif(ArtworkType.ProductionUnit = 'Qty', Order_TmsCost.Qty, Order_TmsCost.Tms )) as cTms) amt
	outer apply (select cTms / @mStandardTMS as OrderTMSCPU) otmc
	outer apply (select iif(@CalArtWorkType = 'CPU', Orders.CPU, OrderTMSCPU) as cCPU) ccpu
	outer apply (select iif(@isSCIDelivery = 0, Orders.BuyerDelivery, Orders.SCIDelivery) as OrderDate) odd
	outer apply (select format(dateadd(day,-7,OrderDate),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(OrderDate) as Date2) odd2
	where 1=1 And (Factory.MDivisionID = @M or @M = '') And (Factory.ID = @Fty or @Fty = '')

	print 'Sew2'
	select * into #sew2 from Sewingoutput_Detail where ID in (select ID from #tmpOrder1)
	print 'Sew1'
	select * into #sew1 from Sewingoutput where ID in (select ID from #tmpOrder1)

	--By Sewing
	Select #tmpOrder1.*, Sewingoutput_Detail.QAQty, Sewingoutput_Detail.InlineQty, Sewingoutput.OutputDate	
	,iif(@ReportType = 1, Date1, Date2) as SewingYYMM
	,Sewingoutput.OutputDate as SewingYYMM_Ori
	,(cCPU * SewingOutput_Detail.QAQty * #tmpOrder1.CPURate) as SewCapacity --(CPU or OrderTMSCPU 來當乘數的值的決定, 是依下拉選擇Report來決定; 若是選擇All 可利用CPU , 若是選擇 非All外,就利用 OrderTMSCPU 來計算 ) 
	into #tmpOrder2 from #tmpOrder1
	left join #sew2 Sewingoutput_Detail on #tmpOrder1.ID = Sewingoutput_Detail.OrderID
	left join #sew1 Sewingoutput on Sewingoutput.ID = Sewingoutput_Detail.ID
	outer apply (select format(dateadd(day,-7,SewingOutput.OutputDate),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Sewingoutput.OutputDate) as Date2) odd2
	

	--Fty Local Order
	select orders.ID,orders.CPU,Qty,FactoryID,StyleUkey,BuyerDelivery into #FactoryOrder from orders
	inner join Factory on Orders.FactoryID = Factory.ID
	Where orders.BuyerDelivery Between @date_s and @date_e
	And (@BrandID = '' or orders.BrandID = @BrandID)
	And orders.Junk = 0 and orders.Qty > 0  
	AND @HasFtyLocalOrder = 1
	AND LocalOrder = 1
	And (Factory.MDivisionID = @M or @M = '') And (Factory.ID = @Fty or @Fty = '')

	Select FactoryOrder.ID, rtrim(FactoryOrder.FactoryID) as FactoryID
		,iif(Factory.Zone <> '', Factory.Zone, iif(Factory.Type = 'S', 'Sample', Factory.Zone)) as MDivisionID
	, Factory.CountryID
	,Style.CPU, FactoryOrderTMSCPU, cCPU
	,Style_TmsCost.ArtworkTypeID 
	,Style_TmsCost.TMS as ArtworkTypeTMS 
	,FactoryOrder.Qty as OrderQty
	,CPURate
	,(cCPU * FactoryOrder.Qty * CPURate) as FactoryOrderCapacity
	,FactoryOrder.BuyerDelivery
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	,FactorySort
	into #tmpFactoryOrder1 from #FactoryOrder FactoryOrder
	inner join Factory on FactoryOrder.FactoryID = Factory.ID
	left join Style on Style.Ukey = FactoryOrder.StyleUkey
	left join Style_TmsCost on @CalArtWorkType != 'CPU' and Style.UKey = Style_TMSCost.StyleUkey And Style_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = Style_TmsCost.ArtworkTypeID
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Style_TMSCost.Qty / 1000, iif(ArtworkType.ProductionUnit = 'Qty', Style_TMSCost.Qty, Style_TMSCost.Tms )) as cTms) amt
	outer apply (select cTms / @mStandardTMS as FactoryOrderTMSCPU ) fotmc
	outer apply (select iif(@CalArtWorkType = 'CPU', FactoryOrder.CPU, FactoryOrderTMSCPU) as cCPU) ccpu
	outer apply (select * from GetCPURate('', '', '', '', 'O') ) gcRate
	outer apply (select format(dateadd(day,-7,FactoryOrder.BuyerDelivery),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(FactoryOrder.BuyerDelivery) as Date2) odd2

	
	--By Sewing
	select * into #sew4 from SewingOutput_Detail where ID in (select ID from #tmpFactoryOrder1)
	select * into #sew3 from SewingOutput where ID in (select ID from #tmpFactoryOrder1)

	Select #tmpFactoryOrder1.*, Sewingoutput_Detail.QAQty, Sewingoutput_Detail.InlineQty, Sewingoutput.OutputDate
	,iif(@ReportType = 1, Date1, Date2) as SewingYYMM
	,Sewingoutput.OutputDate as SewingYYMM_Ori
	,(cCPU * SewingOutput_Detail.QAQty * CpuRate) as SewCapacity
	into #tmpFactoryOrder2 From #tmpFactoryOrder1
	left join #sew4 Sewingoutput_Detail on #tmpFactoryOrder1.ID = Sewingoutput_Detail.OrderID
	left join #sew3 Sewingoutput on Sewingoutput.ID = Sewingoutput_Detail.ID
	outer apply (select format(dateadd(day,-7,SewingOutput.OutputDate),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Sewingoutput.OutputDate) as Date2) odd2


	--Forecast
	Select Orders.ID, rtrim(Orders.FactoryID) as FactoryID
		,iif(Factory.Zone <> '', Factory.Zone, iif(Factory.Type = 'S', 'Sample', Factory.Zone)) as MDivisionID
	, Factory.CountryID
	,cTms as ArtworkTypeTMS
	,Style.CPU, ForecastTMSCPU, cCPU
	,Orders.Qty as ForecastQty
	,Style_TmsCost.ArtworkTypeID
	,CpuRate
	,(cCPU * Orders.Qty * CpuRate) as ForecastCapacity
	,Orders.BuyerDelivery
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	,FactorySort
	into #tmpForecast1 from Orders
	left join Factory on Factory.ID = Orders.FactoryID
	left join Style on Style.Ukey = Orders.StyleUkey
	left join Style_TmsCost on @CalArtWorkType != 'CPU' and Style.UKey = Style_TMSCost.StyleUkey And Style_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = Style_TmsCost.ArtworkTypeID
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Style_TMSCost.Qty / 1000, iif(ArtworkType.ProductionUnit = 'Qty', Style_TMSCost.Qty, Style_TMSCost.Tms )) as cTms) amt
	outer apply (select cTms / @mStandardTMS as ForecastTMSCPU ) fotmc
	outer apply (select iif(@CalArtWorkType = 'CPU', Orders.CPU, ForecastTMSCPU) as cCPU) ccpu
	outer apply (select CpuRate from GetCPURate('', '', '', '', 'O') ) gcRate
	outer apply (select format(dateadd(day,-7,Orders.BuyerDelivery),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Orders.BuyerDelivery) as Date2) odd2
	Where Orders.BuyerDelivery Between @date_s and @date_e
	And Orders.Qty > 0
	AND @HasForecast = 1
	AND Orders.IsForecast = 1 And (Factory.MDivisionID = @M or @M = '') And (Factory.ID = @Fty or @Fty = '')
	--
	declare @tmpFinal table (
		CountryID varchar(2)
		,MDivisionID varchar(8)
		,FactoryID varchar(10)
		,OrderYYMM varchar(10)
		,OrderLoadingCPU numeric(14,2)
		,OrderAccCPU numeric(14,2)
		,MaxOutputDate date
		,MinOutPutDate date
		,FactorySort varchar(3)
	)

	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID, #tmpOrder2.OrderYYMM
	,SUM(Round(cCPU * CPURate * OrderQty ,2)) as OrderLoadingCPU
	,SUM(Round(cCPU * CPURate * QAQty ,2)) as OrderAccCPU
	,Max(OutPutDate) as MaxOutputDate, Min(OutPutDate) as MinOutPutDate
	,FactorySort
	From #tmpOrder2 Group by CountryID,MDivisionID,FactoryID,#tmpOrder2.OrderYYMM,FactorySort

	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID, #tmpForecast1.OrderYYMM
	,SUM(Round(cCPU * CpuRate * ForecastQty,2)) as OrderLoadingCPU 
	,0 as OrderAccCPU	
	,null as MaxOutputDate, null as MinOutPutDate
	,FactorySort
	From #tmpForecast1 Group by CountryID,MDivisionID,FactoryID,#tmpForecast1.OrderYYMM,FactorySort

	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID, #tmpFactoryOrder2.OrderYYMM
	,SUM(Round(cCPU *  CPURate * OrderQty ,2)) as OrderLoadingCPU
	,SUM(Round(cCPU * CPURate * QAQty ,2)) as OrderAccCPU
	,Max(OutPutDate) as MaxOutputDate, Min(OutPutDate) as MinOutPutDate
	,FactorySort
	From #tmpFactoryOrder2 Group by CountryID,MDivisionID,FactoryID,#tmpFactoryOrder2.OrderYYMM,FactorySort

	select CountryID, MDivisionID, FactoryID, OrderYYMM, sum(OrderLoadingCPU) as OrderLoadingCPU, sum(OrderAccCPU) as OrderAccCPU, Max(MaxOutputDate) as MaxOutputDate, Min(MinOutPutDate) as MinOutPutDate
	,FactorySort
	into #tmpFinal from @tmpFinal 
	group by CountryID,MDivisionID,FactoryID,OrderYYMM,FactorySort

	if(@ReportType = 1)
	Begin
	--Report1 : 每個月區間為某一整年----------------------------------------------------------------------------------------------------------------------------------

		--By 所有 CountryID, MDisision, FactoryID
		select CountryID,MDivisionID,FactoryID,FactorySort from #tmpFinal group by CountryID,MDivisionID,FactoryID,FactorySort
		order by FactorySort

		--(A)+(B)By MDivisionID
		select CountryID, CountryName, MDivisionID, #tmpFactory.OrderYYMM as Month, sum(Capacity) as Capacity from #tmpFactory 
		group by CountryID, CountryName, MDivisionID,#tmpFactory.OrderYYMM
	
		--(C)By Factory
		select a.CountryID, a.MDivisionID, a.FactoryID,a.OrderYYMM as Month, a.Capacity , c.Tms from (
			select CountryID, MDivisionID, FactoryID, OrderYYMM,sum(Capacity) as Capacity from (
				select CountryID, MDivisionID, FactoryID, OrderYYMM, OrderCapacity as Capacity from #tmpOrder1 union all
				select CountryID, MDivisionID, FactoryID, OrderYYMM, ForecastCapacity from #tmpForecast1 
			) c group by CountryID, MDivisionID, FactoryID, OrderYYMM			
		) a
		left join (
			select ID,ArtworkTypeID,SUM(Tms) as Tms 
			from Factory_Tms where YEAR = @Year and ArtworkTypeID = @ArtWorkType
			GROUP BY ID,ArtworkTypeID
		) c on a.FactoryID = c.ID
		
		--(D)By non-sister
		Select CountryID, MDivisionID, FactoryID, OrderYYMM as Month, SUM(FactoryOrderCapacity) as Capacity  from #tmpFactoryOrder1
		Group by CountryID,MDivisionID,FactoryID,OrderYYMM

		--For Forecast shared
		select CountryID, MDivisionID, FactoryID, OrderYYMM as Month, sum(ForecastCapacity) as Capacity from #tmpForecast1 group by CountryID,MDivisionID,FactoryID,OrderYYMM
		
		--For Output, 及Output後面的Max日期
		select CountryID, MDivisionID, FactoryID, max(format(SewingYYMM_Ori,'MM/dd')) as SewingYYMM, SewingYYMM as Month, sum(Capacity) as Capacity from (
			Select CountryID, MDivisionID, FactoryID, SewingYYMM_Ori, SewingYYMM, SewCapacity as Capacity from #tmpOrder2
			union Select CountryID, MDivisionID, FactoryID, SewingYYMM_Ori, SewingYYMM, SewCapacity as Capacity from #tmpFactoryOrder2
		) c
		where SewingYYMM_Ori is not null
		group by CountryID,MDivisionID,FactoryID,SewingYYMM

	End
	else if(@ReportType = 2)
	Begin
	--Report2 : 每半個月，區間為設定的年月往後推半年----------------------------------------------------------------------------------------------------------------------------------
		
		--By 所有 CountryID, MDisision, FactoryID
		select CountryID,MDivisionID,FactoryID,FactorySort from #tmpFinal group by CountryID,MDivisionID,FactoryID,FactorySort
		order by FactorySort

		--(K) By Factory 最細的上下半月Capacity
		select CountryID, CountryName, MDivisionID, FactoryID, OrderYYMM as Month, sum(HalfCapacity1) as Capacity1, sum(HalfCapacity2) as Capacity2
		from #tmpFactory 
		group by CountryID,CountryName,MDivisionID,FactoryID,OrderYYMM
		
				
		--(L) By Factory Loading CPU
		select a.CountryID, MDivisionID, a.FactoryID, a.OrderYYMM as MONTH, sum(b.OrderLoadingCPU) as Capacity1, sum(c.OrderLoadingCPU) as Capacity2
		from #tmpFactory a 
		left join (
			select FactoryID, substring(OrderYYMM,1,6) as OrderYYMM, OrderLoadingCPU from #tmpFinal where RIGHT(OrderYYMM,1) = 1
		) b on a.FactoryID = b.FactoryID and a.OrderYYMM = b.OrderYYMM
		left join (
			select FactoryID, substring(OrderYYMM,1,6) as OrderYYMM, OrderLoadingCPU from #tmpFinal where RIGHT(OrderYYMM,1) = 2
		) c on a.FactoryID = c.FactoryID and a.OrderYYMM = c.OrderYYMM
		group by a.CountryID,MDivisionID,a.OrderYYMM,a.FactoryID

		--For Forecast shared
		select a.CountryID, MDivisionID, a.FactoryID, a.OrderYYMM as MONTH, sum(b.ForecastCapacity) as Capacity1, sum(c.ForecastCapacity) as Capacity2
		from #tmpFactory a 
		left join (
			select FactoryID, substring(OrderYYMM,1,6) as OrderYYMM, ForecastCapacity from #tmpForecast1 where RIGHT(OrderYYMM,1) = 1
		) b on a.FactoryID = b.FactoryID and a.OrderYYMM = b.OrderYYMM
		left join (
			select FactoryID, substring(OrderYYMM,1,6) as OrderYYMM, ForecastCapacity from #tmpForecast1 where RIGHT(OrderYYMM,1) = 2
		) c on a.FactoryID = c.FactoryID and a.OrderYYMM = c.OrderYYMM		
		group by a.CountryID,MDivisionID,a.OrderYYMM,a.FactoryID

	End


drop table #tmpFactory
drop table #tmpOrder1
drop table #tmpOrder2
drop table #tmpFactoryOrder1
drop table #tmpFactoryOrder2
drop table #tmpForecast1
drop table #tmpFinal
drop table #Orders
drop table #sew1
drop table #sew2
drop table #FactoryOrder
drop table #sew3
drop table #sew4
		

END
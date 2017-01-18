

CREATE PROCEDURE [dbo].[Planning_Report_R10]
	@ReportType int = 1 --1:整個月 2:半個月 --3:Production status 2017.01.04 Serena電話確認後，表示不用做了
	,@BrandID varchar(20)
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

	declare @mStandardTMS int = (select StdTMS from System) --1400
	declare @mSampleCPURate int = (select SampleRate from System) --1
	declare @date_s date = DATEFROMPARTS(@Year, 1, 1)
	declare @date_e date = DATEFROMPARTS(@Year, 12, 31)
	if(@ReportType = 2)
	begin
		set @date_s = DATEFROMPARTS(@Year, @Month, 8)
		set @date_e = dateadd(day,-1,DATEADD(MONTH,6,@date_s))	
	end

	SELECT CountryID, Factory.CountryID + '-' + Country.Alias as CountryName , Factory.ID as FactoryID, iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID, Factory.CPU
	,Factory_TMS.Year, Factory_TMS.Month, Factory_TMS.ArtworkTypeID, Factory_TMS.TMS 
	,Capacity
	,Capacity * fw.HalfMonth1 / (fw.HalfMonth1 + fw.HalfMonth2) as HalfCapacity1
	,Capacity * fw.HalfMonth2 / (fw.HalfMonth1 + fw.HalfMonth2) as HalfCapacity2
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	into #tmpFactory From Factory
	inner join Country on Factory.CountryID = Country.ID
	left join Factory_TMS on Factory.ID = Factory_Tms.ID
		And ((@ReportType = 1 and Factory_Tms.Year = @Year)
		or (@ReportType = 2 and DATEFROMPARTS(Factory_Tms.Year,Factory_Tms.Month,8) between @date_s and @date_e))
	outer apply (select DATEFROMPARTS(Factory_Tms.Year,Factory_Tms.Month,8) as OrderDate) od
	left join ArtworkType on ArtworkType.Id = Factory_TMS.ArtworkTypeID
	left join Factory_WorkHour fw on Factory.ID = fw.ID and fw.Year = Factory_TMS.Year and fw.Month = Factory_Tms.Month	
	outer apply (select iif(@ArtWorkType = 'CPU', Round(Factory_Tms.TMS * 3600 / @mStandardTMS ,0), Factory_Tms.TMS) as Capacity) cc
	outer apply (select substring(CONVERT(char(10), OrderDate, 111),6,2)  as Date1) odd1
	--outer apply (select dbo.GetHalfMonWithYear(OrderDate) as Date2) odd2
	outer apply (select Factory_TMS.Year + Factory_TMS.Month as Date2) odd2
	--Where ISsci = 1  And Factory.Junk = 0 And Artworktype.ReportDropdown = 1 
	Where  Factory.Junk = 0 And Artworktype.ReportDropdown = 1
	And Artworktype.ID = iif(@ArtWorkType = 'CPU' ,'SEWING', @ArtWorkType)

	--Order
	Select Orders.ID, rtrim(Orders.FactoryID) as FactoryID, CPURate, iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID, Factory.CountryID	
	,Orders.CPU, OrderTMSCPU, cCPU
	,Order_TmsCost.ArtworktypeID
	,Orders.Qty as OrderQty
	,(cCPU * Orders.Qty * CpuRate) as OrderCapacity
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	,OrderDate
	into #tmpOrder1 from Orders
	inner join Factory on Orders.FactoryID = Factory.ID
	left Join Order_TmsCost on @ArtWorkType != 'CPU' and Orders.ID = Order_TmsCost.ID And Order_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = Order_TmsCost.ArtworkTypeID
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Order_TmsCost.Qty / 1000, iif(ArtworkType.ProductionUnit = 'Qty', Order_TmsCost.Qty, Order_TmsCost.Tms )) as cTms) amt
	outer apply (select cTms / @mStandardTMS as OrderTMSCPU) otmc
	outer apply (select iif(@ArtWorkType = 'CPU', Orders.CPU, OrderTMSCPU) as cCPU) ccpu
	outer apply (select CpuRate from GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.CustCDID, @mSampleCPURate) ) gcRate
	outer apply (select iif(@isSCIDelivery = 0, Orders.BuyerDelivery, Orders.SCIDelivery) as OrderDate) odd
	outer apply (select substring(CONVERT(char(10), OrderDate, 111),6,2)  as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(OrderDate) as Date2) odd2
	Where ((@isSCIDelivery = 0 and Orders.BuyerDelivery between @date_s and @date_e)
	or (@isSCIDelivery = 1 and Orders.SciDelivery between @date_s and @date_e))
	And (@BrandID = '' or Orders.BrandID = @BrandID)
	And Orders.Junk = 0 and Orders.Qty > 0  And Orders.Category in ('B','S') 
	AND @HasOrders = 1

	--By Sewing
	Select #tmpOrder1.*, Sewingoutput_Detail.QAQty, Sewingoutput_Detail.InlineQty, Sewingoutput.OutputDate	
	,iif(@ReportType = 1, Date1, Date2) as SewingYYMM
	,Sewingoutput.OutputDate as SewingYYMM_Ori
	,(cCPU * SewingOutput_Detail.QAQty * #tmpOrder1.CPURate) as SewCapacity --(CPU or OrderTMSCPU 來當乘數的值的決定, 是依下拉選擇Report來決定; 若是選擇All 可利用CPU , 若是選擇 非All外,就利用 OrderTMSCPU 來計算 ) 
	into #tmpOrder2 from #tmpOrder1
	left join Sewingoutput_Detail on #tmpOrder1.ID = Sewingoutput_Detail.OrderID
	left join Sewingoutput on Sewingoutput.ID = Sewingoutput_Detail.ID
	outer apply (select substring(CONVERT(char(10), Sewingoutput.OutputDate, 111),6,2)  as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Sewingoutput.OutputDate) as Date2) odd2


	--Fty Local Order
	Select Orders.ID, rtrim(Orders.FactoryID) as FactoryID, iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID, Factory.CountryID
	,Style.CPU, FactoryOrderTMSCPU, cCPU
	,Style_TmsCost.ArtworkTypeID 
	,Style_TmsCost.TMS as ArtworkTypeTMS 
	,Orders.Qty as OrderQty
	,CPURate
	,(cCPU * Orders.Qty * CPURate) as FactoryOrderCapacity
	,Orders.BuyerDelivery
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	into #tmpFactoryOrder1 from Orders
	inner join Factory on Orders.FactoryID = Factory.ID
	left join Style on Style.Ukey = Orders.StyleUkey
	left join Style_TmsCost on @ArtWorkType != 'CPU' and Style.UKey = Style_TMSCost.StyleUkey And Style_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = Style_TmsCost.ArtworkTypeID
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Style_TMSCost.Qty / 1000, iif(ArtworkType.ProductionUnit = 'Qty', Style_TMSCost.Qty, Style_TMSCost.Tms )) as cTms) amt
	outer apply (select cTms / @mStandardTMS as FactoryOrderTMSCPU ) fotmc
	outer apply (select iif(@ArtWorkType = 'CPU', Orders.CPU, FactoryOrderTMSCPU) as cCPU) ccpu
	outer apply (select * from GetCPURate('', '', '', '', 0) ) gcRate
	outer apply (select substring(CONVERT(char(10), Orders.BuyerDelivery, 111),6,2)  as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Orders.BuyerDelivery) as Date2) odd2
	Where Orders.BuyerDelivery Between @date_s and @date_e
	And (@BrandID = '' or Orders.BrandID = @BrandID)
	And Orders.Junk = 0 and Orders.Qty > 0  
	AND @HasFtyLocalOrder = 1
	AND LocalOrder=1
	
	--By Sewing
	Select #tmpFactoryOrder1.*, Sewingoutput_Detail.QAQty, Sewingoutput_Detail.InlineQty, Sewingoutput.OutputDate
	,iif(@ReportType = 1, Date1, Date2) as SewingYYMM
	,Sewingoutput.OutputDate as SewingYYMM_Ori
	,(cCPU * SewingOutput_Detail.QAQty * CpuRate) as SewCapacity
	into #tmpFactoryOrder2 From #tmpFactoryOrder1
	left join Sewingoutput_Detail on #tmpFactoryOrder1.ID = Sewingoutput_Detail.OrderID
	left join Sewingoutput on Sewingoutput.ID = Sewingoutput_Detail.ID
	outer apply (select substring(CONVERT(char(10), Sewingoutput.OutputDate, 111),6,2)  as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Sewingoutput.OutputDate) as Date2) odd2


	--Forecast
	Select Orders.ID, rtrim(Orders.FactoryID) as FactoryID, iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID, Factory.CountryID
	,cTms as ArtworkTypeTMS
	,Style.CPU, ForecastTMSCPU, cCPU
	,Orders.Qty as ForecastQty
	,Style_TmsCost.ArtworkTypeID
	,CpuRate
	,(cCPU * Orders.Qty * CpuRate) as ForecastCapacity
	,Orders.BuyerDelivery
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	into #tmpForecast1 from Orders
	left join Factory on Factory.ID = Orders.FactoryID
	left join Style on Style.Ukey = Orders.StyleUkey
	left join Style_TmsCost on @ArtWorkType != 'CPU' and Style.UKey = Style_TMSCost.StyleUkey And Style_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = Style_TmsCost.ArtworkTypeID
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Style_TMSCost.Qty / 1000, iif(ArtworkType.ProductionUnit = 'Qty', Style_TMSCost.Qty, Style_TMSCost.Tms )) as cTms) amt
	outer apply (select cTms / @mStandardTMS as ForecastTMSCPU ) fotmc
	outer apply (select iif(@ArtWorkType = 'CPU', Orders.CPU, ForecastTMSCPU) as cCPU) ccpu
	outer apply (select CpuRate from GetCPURate('', '', '', '', 0) ) gcRate
	outer apply (select substring(CONVERT(char(10), Orders.BuyerDelivery, 111),6,2)  as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Orders.BuyerDelivery) as Date2) odd2
	Where Orders.BuyerDelivery Between @date_s and @date_e
	And Orders.Qty > 0  
	AND @HasForecast = 1
	AND IsForecast=1
	
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
	)

	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID, #tmpOrder2.OrderYYMM
	,SUM(Round(cCPU * CPURate * OrderQty ,2)) as OrderLoadingCPU
	,SUM(Round(cCPU * CPURate * QAQty ,2)) as OrderAccCPU
	,Max(OutPutDate) as MaxOutputDate, Min(OutPutDate) as MinOutPutDate
	From #tmpOrder2 Group by CountryID,MDivisionID,FactoryID,#tmpOrder2.OrderYYMM

	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID, #tmpForecast1.OrderYYMM
	,SUM(Round(cCPU * CpuRate * ForecastQty,2)) as OrderLoadingCPU 
	,0 as OrderAccCPU
	,null as MaxOutputDate, null as MinOutPutDate 
	From #tmpForecast1 Group by CountryID,MDivisionID,FactoryID,#tmpForecast1.OrderYYMM

	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID, #tmpFactoryOrder2.OrderYYMM
	,SUM(Round(cCPU *  CPURate * OrderQty ,2)) as OrderLoadingCPU
	,SUM(Round(cCPU * CPURate * QAQty ,2)) as OrderAccCPU
	,Max(OutPutDate) as MaxOutputDate, Min(OutPutDate) as MinOutPutDate
	From #tmpFactoryOrder2 Group by CountryID,MDivisionID,FactoryID,#tmpFactoryOrder2.OrderYYMM

	select CountryID, MDivisionID, FactoryID, OrderYYMM, sum(OrderLoadingCPU) as OrderLoadingCPU, sum(OrderAccCPU) as OrderAccCPU, Max(MaxOutputDate) as MaxOutputDate, Min(MinOutPutDate) as MinOutPutDate
	into #tmpFinal
	from @tmpFinal group by CountryID,MDivisionID,FactoryID,OrderYYMM

	if(@ReportType = 1)
	Begin
	--Report1 : 每個月區間為某一整年----------------------------------------------------------------------------------------------------------------------------------

		--By 所有 CountryID, MDisision, FactoryID
		select CountryID,MDivisionID,FactoryID from #tmpFinal group by CountryID,MDivisionID,FactoryID

		--(A)+(B)By MDivisionID
		select CountryID, CountryName, MDivisionID, #tmpFactory.Month, sum(Capacity) as Capacity from #tmpFactory 
		group by CountryID, CountryName, MDivisionID,#tmpFactory.Month
	
		--(C)By Factory
		select a.CountryID, a.MDivisionID, a.FactoryID,a.OrderYYMM as Month, a.Capacity , c.Tms from (
			select CountryID, MDivisionID, FactoryID, OrderYYMM,sum(Capacity) as Capacity from (
				select CountryID, MDivisionID, FactoryID, OrderYYMM, OrderCapacity as Capacity from #tmpOrder1 union all
				select CountryID, MDivisionID, FactoryID, OrderYYMM, ForecastCapacity from #tmpForecast1 
			) c group by CountryID, MDivisionID, FactoryID, OrderYYMM			
		) a
		left join (
			select ID,ArtworkTypeID,SUM(Tms) as Tms 
			from Factory_Tms where YEAR = @Year and ArtworkTypeID = iif(@ArtWorkType = 'CPU', 'SEWING', @ArtWorkType)
			GROUP BY ID,ArtworkTypeID
		) c on a.FactoryID = c.ID
				
		--(D)By non-sister
		Select CountryID, MDivisionID, FactoryID, OrderYYMM as Month, SUM(FactoryOrderCapacity) as Capacity  from #tmpFactoryOrder1
		Group by CountryID,MDivisionID,FactoryID,OrderYYMM

		--For Forecast shared
		select CountryID, MDivisionID, FactoryID, OrderYYMM as Month, sum(ForecastCapacity) as Capacity from #tmpForecast1 group by CountryID,MDivisionID,FactoryID,OrderYYMM
		
		--For Output, 及Output後面的Max日期
		select CountryID, MDivisionID, FactoryID, max(substring(CONVERT(char(10), SewingYYMM_Ori, 111),6,5)) as SewingYYMM, SewingYYMM as Month, sum(Capacity) as Capacity from (
			Select CountryID, MDivisionID, FactoryID, SewingYYMM_Ori, SewingYYMM, SewCapacity as Capacity from #tmpOrder2
			union Select CountryID, MDivisionID, FactoryID, SewingYYMM_Ori, SewingYYMM, SewCapacity as Capacity from #tmpFactoryOrder2
		) c
		where SewingYYMM_Ori is not null
		group by CountryID,MDivisionID,FactoryID,SewingYYMM

		--Select CountryID, MDivisionID, FactoryID, SewingYYMM_Ori, SewingYYMM as Month, sum(SewCapacity) as Capacity from #tmpOrder2 group by CountryID,MDivisionID,FactoryID,SewingYYMM_Ori,SewingYYMM
		--union
		--Select CountryID, MDivisionID, FactoryID, SewingYYMM_Ori, SewingYYMM as Month, sum(SewCapacity) as Capacity from #tmpFactoryOrder2 group by CountryID,MDivisionID,FactoryID,SewingYYMM_Ori,SewingYYMM

	End
	else if(@ReportType = 2)
	Begin
	--Report2 : 每半個月，區間為設定的年月往後推半年----------------------------------------------------------------------------------------------------------------------------------
		
		--By 所有 CountryID, MDisision, FactoryID
		select CountryID,MDivisionID,FactoryID from #tmpFinal group by CountryID,MDivisionID,FactoryID

		--(K) By Factory 最細的上下半月Capacity
		select CountryID, CountryName, MDivisionID, FactoryID, MONTH, sum(HalfCapacity1) as Capacity1, sum(HalfCapacity2) as Capacity2
		from #tmpFactory 
		group by CountryID,CountryName,MDivisionID,FactoryID,MONTH

		
		--(L) By Factory Loading CPU
		select a.CountryID, MDivisionID, a.FactoryID, substring(a.OrderYYMM,5,2) as MONTH, sum(b.OrderLoadingCPU) as Capacity1, sum(c.OrderLoadingCPU) as Capacity2
		from #tmpFactory a 
		left join (
			select FactoryID,substring(OrderYYMM,1,6) as OrderYYMM, OrderLoadingCPU from #tmpFinal where RIGHT(OrderYYMM,1) = 1			
		) b on a.FactoryID = b.FactoryID and a.OrderYYMM = b.OrderYYMM
		left join (
			select FactoryID,substring(OrderYYMM,1,6) as OrderYYMM, OrderLoadingCPU from #tmpFinal where RIGHT(OrderYYMM,1) = 2
		) c on a.FactoryID = c.FactoryID and a.OrderYYMM = c.OrderYYMM
		group by a.CountryID,MDivisionID,a.OrderYYMM,a.FactoryID

		--For Forecast shared
		select a.CountryID, MDivisionID, a.FactoryID, substring(a.OrderYYMM,5,2) as MONTH, sum(b.ForecastCapacity) as Capacity1, sum(c.ForecastCapacity) as Capacity2
		from #tmpFactory a 
		left join (
			select FactoryID,substring(OrderYYMM,1,6) as OrderYYMM, ForecastCapacity from #tmpForecast1 where RIGHT(OrderYYMM,1) = 1
		) b on a.FactoryID = b.FactoryID and a.OrderYYMM = b.OrderYYMM
		left join (
			select FactoryID,substring(OrderYYMM,1,6) as OrderYYMM, ForecastCapacity from #tmpForecast1 where RIGHT(OrderYYMM,1) = 2
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
		

END
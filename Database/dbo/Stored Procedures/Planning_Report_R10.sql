USE [Production]
GO

/****** Object:  StoredProcedure [dbo].[Planning_Report_R10]    Script Date: 2019/06/13 下午 03:41:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[Planning_Report_R10]
	@ReportType int = 1 --1:整個月 2:半個月	--3:Production status 不做
	,@BrandID varchar(20)
	,@ArtWorkType varchar(20) --= 'SEWING'
	,@isSCIDelivery bit = 1
	,@Year int = 2017
	,@Month int = 1
	,@SourceStr varchar(50) = 'Order,Forecast,Fty Local Order'
	,@M varchar(20)
	,@Fty varchar(20)
	,@Zone varchar(20)
AS
BEGIN

	declare @HasOrders bit = 0, @HasForecast bit = 0, @HasFtyLocalOrder bit = 0	
	set @HasOrders = iif(exists(select 1 from dbo.SplitString(@SourceStr,',') where Data = 'Order'), 1, 0)
	set @HasForecast = iif(exists(select 1 from dbo.SplitString(@SourceStr,',') where Data = 'Forecast'), 1, 0)
	set @HasFtyLocalOrder = iif(exists(select 1 from dbo.SplitString(@SourceStr,',') where Data = 'Fty Local Order'), 1, 0)

	declare @CalculateCPU bit = 0
	IF (@ArtWorkType = 'SEWING')
	BEGIN
		set @CalculateCPU = 1;
	END
	
	IF ((select ArtworkUnit from ArtworkType where id = @ArtWorkType) ='STITCH' 
		OR (select ProductionUnit from ArtworkType where id = @ArtWorkType) = 'QTY')
	BEGIN
		set @CalculateCPU = 0;
	END
	
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

	--#tmpFactory
	SELECT CountryID, Factory.CountryID + '-' + Country.Alias as CountryName , Factory.ID as FactoryID
		, iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID
	, Factory.CPU
	,Factory_TMS.Year, Factory_TMS.Month, Factory_TMS.ArtworkTypeID, Factory_TMS.TMS 
	,Capacity
	,Round(Capacity * fw.HalfMonth1 / (fw.HalfMonth1 + fw.HalfMonth2),10) as HalfCapacity1
	,Round(Capacity * fw.HalfMonth2 / (fw.HalfMonth1 + fw.HalfMonth2),10) as HalfCapacity2
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	,Factory.FactorySort
	into #tmpFactory 
	From Factory
	inner join Country on Factory.CountryID = Country.ID
	left join Factory_TMS on Factory.ID = Factory_Tms.ID
		And ((@ReportType = 1 and Factory_Tms.Year = @Year)
		or (@ReportType = 2 and DATEFROMPARTS(Factory_Tms.Year,Factory_Tms.Month,8) between @date_s and @date_e))
	outer apply (select DATEFROMPARTS(Factory_Tms.Year,Factory_Tms.Month,8) as OrderDate) od
	left join ArtworkType on ArtworkType.Id = Factory_TMS.ArtworkTypeID
	left join Factory_WorkHour fw on Factory.ID = fw.ID and fw.Year = Factory_TMS.Year and fw.Month = Factory_Tms.Month	
	--outer apply (select iif(@CalArtWorkType = 'CPU', Round(Factory_Tms.TMS * 3600 / @mStandardTMS ,0), Factory_Tms.TMS) as Capacity) cc
	outer apply (select IIF(@CalArtWorkType = 'CPU', ROUND(Factory_TMS.Tms * 3600 / @mStandardTMS, 0),
						iif(ArtworkType.ArtworkUnit = 'STITCH', Factory_TMS.Tms,
						iif(ArtworkType.ProductionUnit = 'Qty', Factory_TMS.Tms,
						IIF(@CalculateCPU = 1, ROUND(Factory_Tms.Tms * 60 / @mStandardTMS, 0), Factory_TMS.TMS )))) as Capacity) cc
	outer apply (select format(dateadd(day,-7,OrderDate),'yyyyMM') as Date1) odd1
	outer apply (select cast(Factory_TMS.Year as varchar(4)) + cast(Factory_TMS.Month as varchar(2)) as Date2) odd2
	Where ISsci = 1  And Factory.Junk = 0 And Artworktype.ReportDropdown = 1 
	And Artworktype.ID = @ArtWorkType
	And (factory.MDivisionID = @M or @M = '') And (factory.ID = @Fty or @Fty = '') and (Factory.Zone = @Zone or @Zone = '')

	--#Orders
	select id,FactoryID,CPU,OrderTypeID,ProgramID,Qty,Category,BrandID,BuyerDelivery,SciDelivery,CpuRate,GMTComplete
	,localorder,SubconInType
	into #Orders 
	From orders 
	outer apply (select CpuRate from GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'O') ) gcRate
	Where ((@isSCIDelivery = 0 and Orders.BuyerDelivery between @date_s and @date_e)
	or (@isSCIDelivery = 1 and Orders.SciDelivery between @date_s and @date_e))
	And (@BrandID = '' or Orders.BrandID = @BrandID)
	And Orders.Junk = 0 and Orders.Qty > 0  And Orders.Category in ('B','S') 
	AND @HasOrders = 1
	And (orders.MDivisionID = @M or @M = '') And (orders.FactoryID = @Fty or @Fty = '')  
	and (exists(select 1 from Factory where id = Orders.FactoryID and Zone = @Zone) or @Zone = '')
	and (localorder = 0 or SubconInType=2)
	
	if not exists(select 1 from #tmpFactory)
	begin
		insert into #tmpFactory
		SELECT top 1 CountryID, Factory.CountryID + '-' + Country.Alias as CountryName , '' as FactoryID
			, iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID
		, CPU=0		,Year=@Year, Month='00', ArtworkTypeID=@ArtWorkType, TMS =0
		,Capacity=0		, HalfCapacity1=0		, HalfCapacity2=0		,OrderYYMM=concat(@Year,'00')		,FactorySort = 0
		From Factory inner join Country on Factory.CountryID = Country.ID
		where (factory.ID = @Fty or @Fty = '') and (Factory.Zone = @Zone or Zone = '')
	end

	--Order
	Select Orders.ID, rtrim(Orders.FactoryID) as FactoryID, CPURate
		,iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID
	, Factory.CountryID	
	,Orders.CPU, cTms, cCPU
	,Order_TmsCost.ArtworktypeID
	,Orders.Qty as OrderQty
	,Round((cCPU * Orders.Qty * CpuRate),10) as OrderCapacity
	,Round((cCPU * iif(Orders.GMTComplete = 'S', Orders.Qty - GetPulloutData.Qty, 0) * CpuRate),0) as OrderShortage
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	,OrderDate ,FactorySort
	,localorder,SubconInType,ProgramID
	into #tmpOrder1 
	from #Orders Orders
	inner join Factory on Orders.FactoryID = Factory.ID
	left Join Order_TmsCost on @CalArtWorkType != 'CPU' and Orders.ID = Order_TmsCost.ID And Order_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = Order_TmsCost.ArtworkTypeID
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Order_TmsCost.Qty / 1000
							, IIF( ArtworkType.ArtworkUnit = 'PPU' 
									,Order_TmsCost.Price 
									,iif(ArtworkType.ProductionUnit = 'Qty'
											, Order_TmsCost.Qty
											, Order_TmsCost.Tms / 60 
											)
								)
						) as cTms) amt
	outer apply (select iif(@CalArtWorkType = 'CPU', Orders.CPU, cTms) as cCPU) ccpu
	outer apply (select iif(@isSCIDelivery = 0, Orders.BuyerDelivery, Orders.SCIDelivery) as OrderDate) odd	
	outer apply (select format(dateadd(day,iif(@isSCIDelivery = 0, 0, -7),OrderDate),'yyyyMM') as Date1) odd1	
	outer apply (select dbo.GetHalfMonWithYear(OrderDate,@isSCIDelivery) as Date2) odd2	
	outer apply (select Qty=sum(shipQty) from Pullout_Detail where orderid = Orders.id) GetPulloutData

	--#sew2
	select sd.ID,sd.OrderId,SUM(QAQty*ol.Rate/100) as QAQty 
	into #sew2 
	from (select *,sidx=ROW_NUMBER()over(partition by id,orderid,ComboType,Article,Color,OldDetailKey order by id) 
		from SewingOutput_Detail
		) sd 
	inner join Orders o on sd.OrderId = o.ID 
	inner join Order_Location ol on o.ID = ol.OrderId and ol.Location = sd.ComboType 
	where sd.OrderId in (select ID from #tmpOrder1) and sidx = 1 GROUP BY sd.ID,sd.OrderId	

	--#sew1
	select ID,OutputDate 
	into #sew1 
	from SewingOutput 
	where ID in (select ID from #sew2) GROUP BY ID,OutputDate

	--#sew2
	select s2.OrderId,sum(s2.QAQty) as QAQty, max(s1.OutputDate) as OutputDate 
	into #sew_Order 
	from #sew2 s2 
	inner join #sew1 s1 on s2.ID = s1.ID 
	group by s2.OrderId

	--By Sewing
	Select #tmpOrder1.*, SewingOutput.QAQty, Sewingoutput.OutputDate	
	,iif(@ReportType = 1, Date1, Date2) as SewingYYMM
	,Sewingoutput.OutputDate as SewingYYMM_Ori
	,Round((cCPU * SewingOutput.QAQty * #tmpOrder1.CPURate),10) as SewCapacity
	into #tmpOrder2 
	from #tmpOrder1
	left join #sew_Order SewingOutput on SewingOutput.OrderId = #tmpOrder1.ID
	outer apply (select format(dateadd(day,iif(@isSCIDelivery = 0, 0, -7),OrderDate),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Sewingoutput.OutputDate,@isSCIDelivery) as Date2) odd2	
	where LocalOrder=0

	--Fty Local Order
	select ID,CPU,Qty,FactoryID,StyleUkey,BuyerDelivery,SCIDelivery ,SubconInType,ProgramID
	into #FactoryOrder 
	from Orders
	Where ((@isSCIDelivery = 0 and Orders.BuyerDelivery between @date_s and @date_e)
	or (@isSCIDelivery = 1 and Orders.SciDelivery between @date_s and @date_e))
	And (@BrandID = '' or Orders.BrandID = @BrandID)
	And Orders.Junk = 0 and Orders.Qty > 0 	
	AND @HasFtyLocalOrder = 1
	AND Orders.LocalOrder = 1
	--And (Orders.MDivisionID = @M or @M = '') 
	--And (Orders.FactoryID = @Fty or @Fty = '')
	--and (exists(select 1 from Factory where id = Orders.FactoryID and Zone = @Zone) or @Zone = '')

	--#tmpFactoryOrder1
	Select FactoryOrder.ID, rtrim(FactoryOrder.FactoryID) as FactoryID
		,iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID
	, iif(f2.Type = 'S', 'Sample', f2.MDivisionID) as MDivisionID2
	, Factory.CountryID
	,Style.CPU, cTms, cCPU
	,Style_TmsCost.ArtworkTypeID 
	,Style_TmsCost.TMS as ArtworkTypeTMS 
	,FactoryOrder.Qty as OrderQty
	,CPURate
	,Round((cCPU * FactoryOrder.Qty * CPURate),10) as FactoryOrderCapacity
	,FactoryOrder.BuyerDelivery
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	,OrderDate
	,Factory.FactorySort
	,SubconInType
	,FactoryOrder.ProgramID
	into #tmpFactoryOrder1 
	from #FactoryOrder FactoryOrder
	inner join Factory on FactoryOrder.FactoryID = Factory.ID
	LEFT join Factory f2 on FactoryOrder.ProgramID = f2.ID
	left join Style on Style.Ukey = FactoryOrder.StyleUkey
	left join Style_TmsCost on @CalArtWorkType != 'CPU' and Style.UKey = Style_TMSCost.StyleUkey And Style_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = Style_TmsCost.ArtworkTypeID
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Style_TMSCost.Qty / 1000
								, IIF(ArtworkType.ArtworkUnit = 'PPU'
										,Style_TMSCost.Price
										,iif(ArtworkType.ProductionUnit = 'Qty', Style_TMSCost.Qty, CONVERT(numeric,Style_TMSCost.Tms) / 60 )
										)
							) as cTms) amt
	outer apply (select iif(@CalArtWorkType = 'CPU', FactoryOrder.CPU, cTms) as cCPU) ccpu
	outer apply (select 1 as CpuRate ) gcRate
	outer apply (select iif(@isSCIDelivery = 0, FactoryOrder.BuyerDelivery, FactoryOrder.SCIDelivery) as OrderDate) odd
	outer apply (select format(dateadd(day,iif(@isSCIDelivery = 0, 0, -7),OrderDate),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(OrderDate,@isSCIDelivery) as Date2) odd2
	
	
	--By Sewing from Local Order

	--#sew4
	select sd.ID,sd.OrderId,SUM(QAQty*ol.Rate/100) as QAQty 
	into #sew4 
	from (select *,sidx=ROW_NUMBER()over(partition by id,orderid,ComboType,Article,Color,OldDetailKey order by id) 
	from SewingOutput_Detail) sd 
	inner join Orders o on sd.OrderId = o.ID 
	inner join Order_Location ol on o.ID = ol.OrderId and ol.Location = sd.ComboType 
	where sd.OrderId in (select ID from #tmpFactoryOrder1) and sidx = 1 
	GROUP BY sd.ID,sd.OrderId	
	
	--#sew3
	select ID,OutputDate 
	into #sew3 
	from SewingOutput where ID in (select ID from #sew4) 
	GROUP BY ID,OutputDate

	--#sew_FtyOrder
	select s4.OrderId,sum(s4.QAQty) as QAQty, max(s3.OutputDate) as OutputDate 
	into #sew_FtyOrder 
	from #sew4 s4 inner join #sew3 s3 on s4.ID = s3.ID 
	group by s4.OrderId
	
	--#tmpFactoryOrder2
	Select #tmpFactoryOrder1.*, SewingOutput.QAQty, /*Sewingoutput_Detail.InlineQty,*/ Sewingoutput.OutputDate
	,iif(@ReportType = 1, Date1, Date2) as SewingYYMM
	,Sewingoutput.OutputDate as SewingYYMM_Ori
	,Round((cCPU * SewingOutput.QAQty * CpuRate),10) as SewCapacity
	into #tmpFactoryOrder2 
	From #tmpFactoryOrder1	
	left join #sew_FtyOrder SewingOutput on SewingOutput.OrderId = #tmpFactoryOrder1.ID
	outer apply (select format(dateadd(day,iif(@isSCIDelivery = 0, 0, -7),OrderDate),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Sewingoutput.OutputDate,@isSCIDelivery) as Date2) odd2


	--Forecast
	--#tmpForecast1
	Select Orders.ID, rtrim(Orders.FactoryID) as FactoryID
		,iif(Factory.Type = 'S', 'Sample', Factory.MDivisionID) as MDivisionID
	, Factory.CountryID
	,cTms as ArtworkTypeTMS
	,Style.CPU, cTms, cCPU
	,Orders.Qty as ForecastQty
	,Style_TmsCost.ArtworkTypeID
	,CpuRate
	,Round((cCPU * Orders.Qty * CpuRate),10) as ForecastCapacity
	,Orders.BuyerDelivery
	,iif(@ReportType = 1, Date1, Date2) as OrderYYMM
	,FactorySort
	,localorder,SubconInType,Orders.ProgramID
	into #tmpForecast1 
	from Orders
	left join Factory on Factory.ID = Orders.FactoryID
	left join Style on Style.Ukey = Orders.StyleUkey
	left join Style_TmsCost on @CalArtWorkType != 'CPU' and Style.UKey = Style_TMSCost.StyleUkey And Style_TmsCost.ArtworkTypeID = @ArtWorkType
	left join ArtworkType on ArtworkType.Id = Style_TmsCost.ArtworkTypeID
	outer apply (select iif(ArtworkType.ArtworkUnit = 'STITCH', Style_TMSCost.Qty / 1000
								, IIF(ArtworkType.ArtworkUnit = 'PPU'
										,Style_TMSCost.Price
										,iif(ArtworkType.ProductionUnit = 'Qty', Style_TMSCost.Qty, CONVERT(numeric,Style_TMSCost.Tms) / 60 )
										)
							) as cTms) amt
	outer apply (select iif(@CalArtWorkType = 'CPU', IIF(Orders.Category = 'B', Style.CPU, Orders.CPU), cTms) as cCPU) ccpu
	outer apply (select CpuRate from dbo.GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'S') ) gcRate
	outer apply (select format(dateadd(day,iif(@isSCIDelivery = 0, 0, -7),Orders.BuyerDelivery),'yyyyMM') as Date1) odd1
	outer apply (select dbo.GetHalfMonWithYear(Orders.BuyerDelivery,@isSCIDelivery) as Date2) odd2
	Where Orders.BuyerDelivery Between @date_s and @date_e
	And Orders.Qty > 0
	AND @HasForecast = 1
	AND Orders.IsForecast = 1
	And (Orders.MDivisionID = @M or @M = '') And (Orders.FactoryID = @Fty or @Fty = '')  
	and (Factory.Zone = @Zone or @Zone = '')
	and (localorder = 0 or SubconInType=2)

	And (@BrandID = '' or Orders.BrandID = @BrandID)
	--
	declare @tmpFinal table (
		CountryID varchar(2)
		,MDivisionID varchar(8)
		,FactoryID varchar(10)
		,ProgramID varchar(20)
		,OrderYYMM varchar(10)
		,OrderLoadingCPU numeric(14,2)
		,OrderShortage numeric(14,2)
		,MaxOutputDate date
		,MinOutPutDate date
		,FactorySort varchar(3)
		,SubconInType varchar(1)
	)
	
	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID
	, ProgramID=''
	, #tmpOrder2.OrderYYMM
	,sum(OrderCapacity) as OrderLoadingCPU
	,sum(OrderShortage) as OrderShortage
	,Max(OutPutDate) as MaxOutputDate, Min(OutPutDate) as MinOutPutDate
	,FactorySort	
	,SubconInType=''
	From #tmpOrder2 
	Group by CountryID,MDivisionID,FactoryID,#tmpOrder2.OrderYYMM,FactorySort

	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID
	, ProgramID=''
	, #tmpForecast1.OrderYYMM
	,sum(ForecastCapacity) as OrderLoadingCPU
	,0 as OrderShortage
	,null as MaxOutputDate, null as MinOutPutDate
	,FactorySort	
	,SubconInType=''
	From #tmpForecast1 
	where LocalOrder=0
	Group by CountryID,MDivisionID,FactoryID,#tmpForecast1.OrderYYMM,FactorySort
	
	insert into @tmpFinal
	Select CountryID, MDivisionID, FactoryID
	,ProgramID
	,#tmpFactoryOrder2.OrderYYMM
	,sum(FactoryOrderCapacity) as OrderLoadingCPU
	,0 as OrderShortage
	,Max(OutPutDate) as MaxOutputDate, Min(OutPutDate) as MinOutPutDate
	,FactorySort
	,SubconInType
	From #tmpFactoryOrder2 
	Group by CountryID,MDivisionID,FactoryID,#tmpFactoryOrder2.OrderYYMM,FactorySort,ProgramID,SubconInType

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
		,ProgramID
		,SubconInType
	into #tmpFinal 
	from @tmpFinal 
	group by CountryID,MDivisionID,FactoryID,OrderYYMM,FactorySort,ProgramID,SubconInType

	if(@ReportType = 1)
	Begin
	--Report1 : 每個月區間為某一整年
	----------------------------------------------------------------------------------------------------------------------------------
		select t.CountryID,t.MDivisionID,t.FactoryID,t.FactorySort
		from #tmpFinal t
		INNER JOIN Factory f ON t.FactoryID=f.ID AND f.KPICode IN (SELECT ID FROm Factory WHERE MDivisionID=@M or @M = '')
		group by t.CountryID,t.MDivisionID,FactoryID,t.FactorySort
		order by t.FactorySort



		--(A)+(B)By MDivisionID
		select CountryID, CountryName, MDivisionID, #tmpFactory.OrderYYMM as Month, sum(Capacity) as Capacity from #tmpFactory 
		group by CountryID, CountryName, MDivisionID,#tmpFactory.OrderYYMM
	
		--(C)By Factory
		
		select CountryID, MDivisionID, FactoryID, OrderYYMM
		,sum(Capacity1) as Capacity1,sum(Capacity2) as Capacity2
		,sum([FtyTmsCapa]) as [FtyTmsCapa] , sum(OrderShortage) as OrderShortage
		into #tmpByFactory
		from (
		-- Order
				Select CountryID, MDivisionID, FactoryID, OrderYYMM
				,[Capacity1] = isnull(OrderCapacity,0)
				,[Capacity2] = 0 
				,[FtyTmsCapa] = 0, OrderShortage
				from #tmpOrder1
				where SubconInType!=2 and LocalOrder=0
			union all
				Select CountryID, MDivisionID, FactoryID, OrderYYMM
				,[Capacity1] = 0
				,[Capacity2] = isnull(OrderCapacity,0)
				,[FtyTmsCapa] = 0, OrderShortage
				from #tmpOrder1
				where SubconInType=2
			union all
				Select t.CountryID, t.MDivisionID
				, [FactoryID] = case when f.ID is null then (select FactoryID from orders where id=t.ProgramID)
					else t.ProgramID end
				, t.OrderYYMM
				,[Capacity1] = 0
				,[Capacity2] = - isnull(OrderCapacity,0) 
				,[FtyTmsCapa] = 0, OrderShortage
				from #tmpOrder1 t
				left join SCIFty f on t.ProgramID=f.ID
				where SubconInType=2
			union all
		-- #tmpForecast1
				Select CountryID, MDivisionID, FactoryID, OrderYYMM
				,[Capacity1] = isnull(ForecastCapacity,0)
				,[Capacity2] = 0 
				,[FtyTmsCapa] = 0, OrderShortage =0
				from #tmpForecast1
				where SubconInType!=2 and LocalOrder=0
			union all
				Select CountryID, MDivisionID, FactoryID, OrderYYMM
				,[Capacity1] = 0
				,[Capacity2] = isnull(ForecastCapacity,0)
				,[FtyTmsCapa] = 0, OrderShortage=0
				from #tmpForecast1
				where SubconInType=2
			union all
				Select t.CountryID, t.MDivisionID
				, [FactoryID] = case when f.ID is null then (select FactoryID from orders where id=t.ProgramID)
					else t.ProgramID end
				, OrderYYMM
				,[Capacity1] = 0
				,[Capacity2] = - isnull(ForecastCapacity,0) 
				,[FtyTmsCapa] = 0, OrderShortage=0
				from #tmpForecast1 t
				left join SCIFty f on t.ProgramID=f.ID
				where SubconInType=2
			union all
			--	#tmpFactory
				select CountryID, MDivisionID, FactoryID, OrderYYMM, 0,0, Capacity as FtyTmsCapa, 0 
				from #tmpFactory 
			-- SubconInType = 2 , LocalOrder
			union all
			select t.CountryID, f.MDivisionID
				, [FactoryID] = case when f.ID is null then (select FactoryID from orders where id=t.ProgramID)
					else t.ProgramID end
				, OrderYYMM
				,[Capacity1] = 0
				,[Capacity2] = - isnull(OrderLoadingCPU,0) 
				,[FtyTmsCapa] = 0, OrderShortage=0
				from #tmpFinal t
				left join SCIFty f on t.ProgramID=f.ID
				where SubconInType = 2
		) a 
		group by CountryID, MDivisionID, FactoryID,OrderYYMM

		select 
			a.CountryID, a.MDivisionID, a.FactoryID, a.OrderYYMM as Month, Factory.CountryID + '-' + Country.Alias as CountryName,
			a.str_Capacity,a.Capacity
			, c.Tms, a.FtyTmsCapa, a.OrderShortage
		from (
			select CountryID, MDivisionID, FactoryID, OrderYYMM
			,[str_Capacity] =  case when (Capacity2 > 0) then ('='+ convert(varchar(100),Capacity1) +'+'+ convert(varchar(100),Capacity2))
									when (Capacity2 < 0) then ('='+ convert(varchar(100),Capacity1) + convert(varchar(100),Capacity2))
								else convert(varchar(100),isnull(Capacity1,0)) end
			,[Capacity] = case when (Capacity2 > 0) then ( Capacity1 + Capacity2 )
							when (Capacity2 < 0) then ( Capacity1 + Capacity2 )
							else convert(varchar(100),isnull(Capacity1,0)) end
			,FtyTmsCapa,OrderShortage
			from #tmpByFactory
		) a
		inner join Factory on a.FactoryID = Factory.ID
		inner join Country on Factory.CountryID = Country.ID
		left join (
			select ID, ArtworkTypeID, SUM(cc.Capacity) as Tms 
			from Factory_Tms 
			outer apply (select iif(@CalArtWorkType = 'CPU', Round(Factory_Tms.TMS * 3600 / @mStandardTMS ,0), Factory_Tms.TMS) as Capacity) cc
			where YEAR = @Year and ArtworkTypeID = @ArtWorkType			
			GROUP BY ID,ArtworkTypeID
		) c on a.FactoryID = c.ID
		
		--(D)By non-sister
		Select CountryID, MDivisionID, FactoryID, OrderYYMM as Month, SUM(FactoryOrderCapacity) as Capacity  
		from #tmpFactoryOrder1
		where SubconInType=3
		Group by CountryID,MDivisionID,FactoryID,OrderYYMM

		--For Forecast shared
		select CountryID, MDivisionID, FactoryID, OrderYYMM as Month, sum(ForecastCapacity) as Capacity 
		from #tmpForecast1 
		where LocalOrder=0
		group by CountryID,MDivisionID,FactoryID,OrderYYMM
		
		--For Output, ��Output�᭱��Max���
		select CountryID, MDivisionID, FactoryID, max(format(SewingYYMM_Ori,'yyyy/MM/dd')) as SewingYYMM, OrderYYMM as Month, sum(Capacity) as Capacity 
		from (
			Select CountryID, MDivisionID, FactoryID, SewingYYMM_Ori, OrderYYMM, SewCapacity as Capacity, SubconInType from #tmpOrder2
			union ALL Select CountryID, MDivisionID, FactoryID, SewingYYMM_Ori, SewingYYMM, SewCapacity as Capacity, SubconInType from #tmpFactoryOrder2
		) c
		where SewingYYMM_Ori is not null
		and SubconInType <> 1
		group by CountryID,MDivisionID,FactoryID,OrderYYMM

	drop table #tmpByFactory
	End
	else if(@ReportType = 2)
	Begin
	--Report2 : �C�b�Ӥ�A�϶����]�w���~�멹����b�~----------------------------------------------------------------------------------------------------------------------------------

		select t.CountryID,t.MDivisionID,t.FactoryID,t.FactorySort
		from #tmpFinal t
		INNER JOIN Factory f ON t.FactoryID=f.ID AND f.KPICode IN (SELECT ID FROm Factory WHERE MDivisionID=@M or @M = '')
		group by t.CountryID,t.MDivisionID,FactoryID,t.FactorySort
		order by t.FactorySort

		--(K) By Factory �̲Ӫ��W�U�b��Capacity
		select CountryID, CountryName, MDivisionID, FactoryID, OrderYYMM as Month, sum(HalfCapacity1) as Capacity1, sum(HalfCapacity2) as Capacity2
		from #tmpFactory 
		group by CountryID,CountryName,MDivisionID,FactoryID,OrderYYMM
		
				
		--(L) By Factory Loading CPU
		
select a.CountryID, MDivisionID, a.FactoryID, a.OrderYYMM as MONTH
,[str_Capacity1]  = case when (b.OrderLoadingCPU2 > 0) then ('='+ convert(varchar(100),b.OrderLoadingCPU1) +'+' 
					 + convert(varchar(100),b.OrderLoadingCPU2))
					 when (b.OrderLoadingCPU2 < 0) then ('='+ convert(varchar(100),b.OrderLoadingCPU1)  
					 + convert(varchar(100),b.OrderLoadingCPU2))
				else convert(varchar(100),isnull(b.OrderLoadingCPU1,0)) end
,[str_Capacity2]  = case when (c.OrderLoadingCPU2 > 0) then ('='+ convert(varchar(100),c.OrderLoadingCPU1) +'+' 
					 + convert(varchar(100),c.OrderLoadingCPU2))
					 when (c.OrderLoadingCPU2 < 0) then ('='+ convert(varchar(100),c.OrderLoadingCPU1)  
					 + convert(varchar(100),c.OrderLoadingCPU2))
				else convert(varchar(100),isnull(c.OrderLoadingCPU1,0)) end
,[Capacity1]  = case when (b.OrderLoadingCPU2 > 0) then ( b.OrderLoadingCPU1 +b.OrderLoadingCPU2 )
					 when (b.OrderLoadingCPU2 < 0) then ( b.OrderLoadingCPU1 +b.OrderLoadingCPU2 )
				else convert(varchar(100),isnull(b.OrderLoadingCPU1,0)) end
,[Capacity2]  = case when (c.OrderLoadingCPU2 > 0) then ( c.OrderLoadingCPU1 +c.OrderLoadingCPU2 )
					 when (c.OrderLoadingCPU2 < 0) then ( c.OrderLoadingCPU1 +c.OrderLoadingCPU2 )
				else convert(varchar(100),isnull(c.OrderLoadingCPU1,0)) end
from (
	select CountryID,MDivisionID,FactoryID,substring(OrderYYMM,1,6) as OrderYYMM
	,sum(OrderLoadingCPU) as OrderLoadingCPU
	,max(MaxOutputDate) as MaxOutputDate
	,min(MinOutPutDate) as MinOutPutDate
	, FactorySort 
	from #tmpFinal 
	group by CountryID,MDivisionID,FactoryID,substring(OrderYYMM,1,6),FactorySort 
) a 
left join (
select FactoryID,OrderYYMM,[OrderLoadingCPU1] = sum(OrderLoadingCPU1) ,[OrderLoadingCPU2] = sum(OrderLoadingCPU2)
	from (
		select FactoryID, substring(OrderYYMM,1,6) as OrderYYMM
		, isnull(sum(OrderLoadingCPU),0) as OrderLoadingCPU1 
		, 0 as OrderLoadingCPU2
		from #tmpFinal 
		where RIGHT(OrderYYMM,1) = 1 
		and SubconInType!=2
		group by FactoryID,substring(OrderYYMM,1,6)
	union all
		select FactoryID, substring(OrderYYMM,1,6) as OrderYYMM
		, 0 as OrderLoadingCPU1 
		, isnull(sum(OrderLoadingCPU),0) as OrderLoadingCPU2
		from #tmpFinal 
		where RIGHT(OrderYYMM,1) = 1 
		and SubconInType = 2
		group by FactoryID,substring(OrderYYMM,1,6)
	union all
		select [FactoryID] = ProgramID, substring(OrderYYMM,1,6) as OrderYYMM
		, 0 as OrderLoadingCPU1 
		, - isnull(sum(OrderLoadingCPU),0) as OrderLoadingCPU2
		from #tmpFinal 
		where RIGHT(OrderYYMM,1) = 1 
		and SubconInType = 2
		group by ProgramID,substring(OrderYYMM,1,6)
	) a
group by FactoryID,OrderYYMM
) b on a.FactoryID = b.FactoryID and substring(a.OrderYYMM,1,6) = b.OrderYYMM
left join (
	select FactoryID,OrderYYMM,[OrderLoadingCPU1] = sum(OrderLoadingCPU1) ,[OrderLoadingCPU2] = sum(OrderLoadingCPU2)
	from (
		select FactoryID, substring(OrderYYMM,1,6) as OrderYYMM
		, isnull(sum(OrderLoadingCPU),0) as OrderLoadingCPU1 
		, 0 as OrderLoadingCPU2
		from #tmpFinal 
		where RIGHT(OrderYYMM,1) = 2 
		and SubconInType!=2
		group by FactoryID,substring(OrderYYMM,1,6)
	union all
		select FactoryID, substring(OrderYYMM,1,6) as OrderYYMM
		, 0 as OrderLoadingCPU1 
		, isnull(sum(OrderLoadingCPU),0) as OrderLoadingCPU2
		from #tmpFinal 
		where RIGHT(OrderYYMM,1) = 2
		and SubconInType = 2
		group by FactoryID,substring(OrderYYMM,1,6)
	union all
		select [FactoryID] = ProgramID, substring(OrderYYMM,1,6) as OrderYYMM
		, 0 as OrderLoadingCPU1 
		, - isnull(sum(OrderLoadingCPU),0) as OrderLoadingCPU2
		from #tmpFinal 
		where RIGHT(OrderYYMM,1) = 2 
		and SubconInType = 2
		group by ProgramID,substring(OrderYYMM,1,6)
	) a
group by FactoryID,OrderYYMM
) c on a.FactoryID = c.FactoryID and substring(a.OrderYYMM,1,6) = c.OrderYYMM
		

		--For Forecast shared
		select a.CountryID, a.MDivisionID, a.FactoryID, a.OrderYYMM as MONTH, sum(b.ForecastCapacity) as Capacity1, sum(c.ForecastCapacity) as Capacity2 from (
			select DISTINCT a.CountryID,a.MDivisionID,a.FactoryID,substring(OrderYYMM,1,6) as OrderYYMM from #tmpForecast1 a
			where LocalOrder=0
		) a 
		left join (
			select CountryID, MDivisionID, FactoryID, substring(OrderYYMM,1,6) as OrderYYMM, sum(ForecastCapacity) ForecastCapacity 
			from #tmpForecast1 WHERE RIGHT(OrderYYMM,1) = '1' and LocalOrder=0
			group by CountryID,MDivisionID,FactoryID,OrderYYMM
		) b on a.CountryID = b.CountryID and a.FactoryID = b.FactoryID and a.OrderYYMM = b.OrderYYMM and a.MDivisionID = b.MDivisionID
		left join (
			select CountryID, MDivisionID, FactoryID, substring(OrderYYMM,1,6) as OrderYYMM, sum(ForecastCapacity) ForecastCapacity 
			from #tmpForecast1 WHERE RIGHT(OrderYYMM,1) = '2' and LocalOrder=0
			group by CountryID,MDivisionID,FactoryID,OrderYYMM
		) c on a.CountryID = c.CountryID and a.FactoryID = c.FactoryID and a.OrderYYMM = c.OrderYYMM and a.MDivisionID = c.MDivisionID
		--WHERE a.COUNTRYID = 'PH' AND a.MDivisionID = 'Zone 1' and a.OrderYYMM like '201707'
		group by a.CountryID,a.MDivisionID,a.OrderYYMM,a.FactoryID
		
		--For Output, 及Output後面的Max日期
		select CountryID, MDivisionID,MDivisionID2, FactoryID,SubconInType, max(format(SewingYYMM_Ori,'yyyy/MM/dd')) as SewingYYMM, OrderYYMM as Month, sum(Capacity) as Capacity 
		from (
			Select CountryID, MDivisionID, '' as MDivisionID2,FactoryID, SewingYYMM_Ori, OrderYYMM, SewCapacity as Capacity, SubconInType from #tmpOrder2
			union ALL 
			Select CountryID, MDivisionID, MDivisionID2,FactoryID, SewingYYMM_Ori, OrderYYMM, SewCapacity as Capacity, SubconInType from #tmpFactoryOrder2
		) c
		where SewingYYMM_Ori is not null
		group by CountryID,MDivisionID,FactoryID,OrderYYMM,MDivisionID2,SubconInType
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
drop table #sew_Order
drop table #FactoryOrder
drop table #sew3
drop table #sew4
drop table #sew_FtyOrder

END
GO



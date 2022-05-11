CREATE PROCEDURE [dbo].[Quaility_SearchList]
	@StyleID VARCHAR(15) = '',
	@SeasonID VARCHAR(10) = '',
	@BrandID VARCHAR(8) = ''
AS
BEGIN
	SET NOCOUNT ON;


	declare @tmp TABLE(
		[Article] [VARCHAR](8) NULL,  
		[Type] [VARCHAR](100) NULL,
		[OrderID] [VARCHAR](13) NULL,
		[FactoryID] [VARCHAR](8) NULL,
		[Result] [VARCHAR](8) NULL,
		[TestDate] date NULL
	)

	if @StyleID = '' or @SeasonID = '' or @BrandID = ''
	begin
		insert into @tmp
		select Article = '' 
			, Type = ''
			, OrderID = ''
			, FactoryID = ''
			, Result = '' 
			, TestDate = null
	end
	else
	begin
		insert into @tmp
		select Article = ''
			, Type = 'Fabric Crocking & Shrinkage Test (504, 405)'
			, OrderID = o.ID
			, FactoryID = o.FactoryID
			, [Result] = f_Result.Result
			, [TestDate] = f_TestDate.TestDate 
		from Orders o WITH(NOLOCK)
		outer apply (
			select TestDate = MAX(f.TestDate)
			from (
				select TestDate = (
						SELECT MAX(TestDate) FROM (
							SELECT TestDate = f.CrockingDate
							UNION
							SELECT TestDate = f.HeatDate
							UNION 
							SELECt TestDate = f.WashDate
						)tmp
					)
				from FIR_Laboratory f WITH(NOLOCK)
				where f.POID = o.ID
			)f
		)f_TestDate
		outer apply (
			select Result = case Max(case f.Result 
						 when 'Fail' then 2
						 when 'Pass' then 1
						 else 0
					   end)
					when 2 then 'Fail'
					when 1 then 'Pass'
					else ''
					end
			from FIR_Laboratory f WITH(NOLOCK)
			where f.POID = o.ID
		)f_Result
		where exists (select 1 from FIR_Laboratory f WITH(NOLOCK) where f.[POID] = o.ID)
		and o.StyleID = @StyleID
		and o.SeasonID = @SeasonID
		and o.BrandID = @BrandID
		and f_TestDate.TestDate <> ''

		Union all

		select Article = g.Article
			, Type = 'Garment Test (450, 451, 701, 710)'
			, OrderID = gd.OrderID
			, o.FactoryID 
			, [Result] = IIF(gd.Result='P','Pass', IIF(gd.Result='F','Fail',''))
			, [TestDate] = gd.InspDate
		from GarmentTest g WITH(NOLOCK)
		inner join GarmentTest_Detail gd WITH(NOLOCK) ON g.ID= gd.ID
		left join Orders o WITH(NOLOCK) ON o.ID = gd.OrderID
		where g.StyleID = @StyleID
		and g.SeasonID = @SeasonID
		and g.BrandID = @BrandID
		and gd.InspDate <> ''
		
		Union all

		select Article
			, Type = 'Mockup Crocking Test  (504)'
			, m.POID
			, o.FactoryID
			, Result
			, TestDate 
		from MockupCrocking m WITH(NOLOCK)
		left join Orders o WITH(NOLOCK) ON o.ID = m.POID
		where m.StyleID = @StyleID
		and m.SeasonID = @SeasonID
		and m.BrandID = @BrandID
		and TestDate <> ''

		Union all

		select Article
			, Type = 'Mockup Oven Test (514)'
			, m.POID
			, o.FactoryID
			, Result
			, TestDate 
		from MockupOven m WITH(NOLOCK)
		left join Orders o WITH(NOLOCK) ON o.ID = m.POID
		where m.Type = 'B'
		and m.StyleID = @StyleID
		and m.SeasonID = @SeasonID
		and m.BrandID = @BrandID
		and m.TestDate <> ''

		Union all

		select Article
			, Type = 'Mockup Wash Test (701)'
			, m.POID
			, o.FactoryID
			, Result
			, TestDate 
		from MockupWash m WITH(NOLOCK)
		left join Orders o WITH(NOLOCK) ON o.ID = m.POID
		where m.Type = 'B' 
		and m.StyleID = @StyleID
		and m.SeasonID = @SeasonID
		and m.BrandID = @BrandID
		and m.TestDate <> ''

		Union all

		select f.Article
			, Type = 'Fabric Oven Test (515)'
			, f.POID			
			, o.FactoryID
			, f.Result
			, f.InspDate
		from Oven f WITH(NOLOCK)
		inner join Orders o WITH(NOLOCK) on o.ID = f.POID
		where o.StyleID = @StyleID 
		and o.SeasonID = @SeasonID 
		and o.BrandID = @BrandID
		and f.InspDate <> ''

		Union all

		select f.Article
			, Type = 'Washing Fastness (501)'
			, f.POID
			, o.FactoryID
			, f.Result
			, f.InspDate
		from ColorFastness f WITH(NOLOCK)
		inner join Orders o WITH(NOLOCK) on o.ID = f.POID
		where o.StyleID = @StyleID 
		and o.SeasonID = @SeasonID 
		and o.BrandID = @BrandID
		and f.InspDate <> ''

		Union all

		select Article = ''
			, Type = 'Accessory Oven & Wash Test (515, 701)'
			, OrderID = o.ID
			, o.FactoryID
			, [Result] = f_Result.Result
			, [TestDate] = f_TestDate.TestDate 
		from Orders o WITH(NOLOCK)
		outer apply (
			select TestDate = MAX(f.TestDate)
			from (
				select TestDate = (
						SELECT MAX(TestDate) FROM (
							SELECT TestDate = f.OvenDate
							UNION 
							SELECt TestDate = f.WashDate
						)tmp
					)
				from AIR_Laboratory f WITH(NOLOCK)
				where f.POID = o.ID
			)f
		)f_TestDate
		outer apply (
			select Result = case Max(case f.Result 
						 when 'Fail' then 2
						 when 'Pass' then 1
						 else 0
					   end)
					when 2 then 'Fail'
					when 1 then 'Pass'
					else ''
					end
			from AIR_Laboratory f WITH(NOLOCK)
			where f.POID = o.ID
		)f_Result
		where exists (select 1 from AIR_Laboratory f WITH(NOLOCK) where f.[POID] = o.ID)
		and o.StyleID = @StyleID
		and o.SeasonID = @SeasonID
		and o.BrandID = @BrandID
		and f_TestDate.TestDate <> ''

		Union all

		select m.Article
			, Type = 'Pulling test for Snap/Botton/Rivet (437)'
			, m.POID
			, o.FactoryID
			, m.Result
			, m.TestDate
		from [ExtendServer].ManufacturingExecution.dbo.PullingTest m WITH(NOLOCK)
		left join Orders o WITH(NOLOCK) ON o.ID = m.POID
		where m.StyleID = @StyleID
		and m.SeasonID = @SeasonID
		and m.BrandID = @BrandID
		and m.TestDate <> ''

		Union all

		select Article
			, Type= 'Water Fastness Test(503)'
			, w.POID
			, o.FactoryID
			, w.Result
			, TestDate = w.InspDate
		from WaterFastness w WITH (NOLOCK) 
		inner join Orders o WITH(NOLOCK) ON o.ID = w.POID
		where o.StyleID = @StyleID 
		and o.SeasonID = @SeasonID 
		and o.BrandID = @BrandID
		and w.InspDate <> ''

		Union all

		select Article
			, Type= 'Perspiration Fastness (502)'
			, w.POID
			, o.FactoryID
			, w.Result
			, TestDate = w.InspDate
		from PerspirationFastness w WITH (NOLOCK) 
		inner join Orders o WITH(NOLOCK) ON o.ID = w.POID
		where o.StyleID = @StyleID 
		and o.SeasonID = @SeasonID 
		and o.BrandID = @BrandID
		and w.InspDate <> ''
	end
	
	select *
	from @tmp
END
Use Production;
GO

/* 
=======================================================================================================================================
Author:		Jack
Create date: 2020/04/21
Description: 根據傳入的日期、工廠等條件，查詢Total CPU、ManHours，另可決定是否排除姊妹廠的數量

參考來源 : Sewing R02 (ISSUE單號ISP20200653)

使用範例：
SELECT * FROM dbo.[SewinR02ForFMS]('2020/03/01','2020/03/31','MA2,MA3,MAI,MWI','Y')
SELECT * FROM dbo.[SewinR02ForFMS]('2020/03/01','2020/03/31','MAI','N')

=======================================================================================================================================
*/


CREATE function [dbo].[SewinR02ForFMS]
(
	  @BeginDate    date
	 ,@EndDate      date	
	 ,@Factorys		varchar(100)				
	 ,@Exclused_Non_Sister_Subcon varchar(1)   ----Y  or N
)
RETURNS @Return TABLE
(
	TotalCPU numeric(20,2),
	TotalManHours numeric(20,2)
)
AS
Begin
	declare @FactoryTable Table
	(
		FactoryID  varChar(10)		
	)
	
	--  整理並檢查工廠ID
	INSERT INTO @FactoryTable
	SELECT [FactoryID]=REPLACE(Data,' ','') 
	FROM SplitString(@Factorys,',')
	WHERE EXISTS( SELECT * FROM Factory WHERE ID = REPLACE(Data,' ','') )
	;
	declare @tmpSewingDetail Table
	(
		OutputDate date
		, Category varchar(10)
		, Shift varchar(10)
		, SewingLineID varchar(5)
		, ActManPower numeric(10,1)
		, Team varchar(2)
		, OrderId varchar(13)
		, ComboType varchar(2)
		, WorkHour numeric(15,3)
		, QAQty int
		, InlineQty int
		, OrderCategory varchar(2)
		, LocalOrder bit
		, FactoryID varchar(5)
		, OrderProgram varchar(20)
		, MockupProgram varchar(20)
		, [OrderCPU] numeric(20,3)
		, [OrderCPUFactor] numeric(15,1)
		, [MockupCPU] numeric(20,3)
		, [MockupCPUFactor] numeric(15,1)
		, [OrderStyle] varchar(20)
		, [MockupStyle] varchar(20)
        , [Rate] numeric(9,4)
		, StdTMS int
        , SubconInType varchar(5)
        , [SubconOutFty] varchar(10)
	)
	;
	declare @tmpSewingGroup Table	
	(
		  OutputDate date
		, Category varchar(10)
		, Shift varchar(10)
		, SewingLineID varchar(5)
		, ActManPower numeric(10,1)
		, Team varchar(2)
		, OrderId varchar(13)
		, ComboType varchar(2)
		, WorkHour numeric(15,3)
		, QAQty int
		, InlineQty int
		, OrderCategory varchar(2)
		, LocalOrder bit
		, FactoryID varchar(5)
		, OrderProgram varchar(20)
		, MockupProgram varchar(20)
		, [OrderCPU] numeric(20,3)
		, [OrderCPUFactor] numeric(15,1)
		, [MockupCPU] numeric(20,3)
		, [MockupCPUFactor] numeric(15,1)
		, [OrderStyle] varchar(20)
		, [MockupStyle] varchar(20)
        , [Rate] numeric(9,4)
		, StdTMS int
		, LastShift varchar(5)
        , SubconInType varchar(5)
        , [SubconOutFty] varchar(10)
	)
	;
	declare @tmp1stFilter Table	
	(
		  OutputDate date
		, Category varchar(10)
		, Shift varchar(10)
		, SewingLineID varchar(5)
		, ActManPower numeric(10,1)
		, Team varchar(2)
		, OrderId varchar(13)
		, ComboType varchar(2)
		, WorkHour numeric(15,3)
		, QAQty int
		, InlineQty int
		, OrderCategory varchar(2)
		, LocalOrder bit
		, FactoryID varchar(5)
		, OrderProgram varchar(20)
		, MockupProgram varchar(20)
		, [OrderCPU] numeric(20,3)
		, [OrderCPUFactor] numeric(15,1)
		, [MockupCPU] numeric(20,3)
		, [MockupCPUFactor] numeric(15,1)
		, [OrderStyle] varchar(20)
		, [MockupStyle] varchar(20)
        , [Rate] numeric(9,4)
		, StdTMS int
		, LastShift varchar(5)
        , SubconInType varchar(5)
        , [SubconOutFty] varchar(10)
		, Holiday bit
	)	
	;
	declare @AllDetail Table	
	(
		  OutputDate date
		, Shift varchar(10)
		, Team varchar(2)
		, SewingLineID varchar(5)
		, OrderId varchar(13)
		, Style varchar(50)
		, QAQty int
		, ActManPower numeric(10,1)
		, Program varchar(20)
		, WorkHour numeric(15,3)
		, StdTMS int
		, [MockupCPU] numeric(20,3)
		, [MockupCPUFactor] numeric(15,1)
		, [OrderCPU] numeric(20,3)
		, [OrderCPUFactor] numeric(15,1)
        , [Rate] numeric(9,4)
		, Category varchar(10)
		, LastShift varchar(5)
		, ComboType varchar(2)
		, FactoryID varchar(5)
        , SubconInType varchar(5)
        , [SubconOutFty] varchar(10)
	)
	;
	INSERT INTO @tmpSewingDetail	
	select  s.OutputDate
			, s.Category
			, s.Shift
			, s.SewingLineID
			, [ActManPower] = s.Manpower
			, s.Team
			, sd.OrderId
			, sd.ComboType
			, sd.WorkHour
			, sd.QAQty
			, sd.InlineQty
			, [OrderCategory] = isnull(o.Category,'')
			, o.LocalOrder
			, s.FactoryID
			, [OrderProgram] = isnull(o.ProgramID,'') 
			, [MockupProgram] = isnull(mo.ProgramID,'')
			, [OrderCPU] = isnull(o.CPU,0)
			, [OrderCPUFactor] = isnull(o.CPUFactor,0)
			, [MockupCPU] = isnull(mo.Cpu,0)
			, [MockupCPUFactor] = isnull(mo.CPUFactor,0)
			, [OrderStyle] = isnull(o.StyleID,'')
			, [MockupStyle] = isnull(mo.StyleID,'')
			, [Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id ,sd.ComboType),100)/100
			, System.StdTMS
			, o.SubconInType
			, [SubconOutFty] = iif(sf.id is null,'Other',s.SubconOutFty)
	----INTO #tmpSewingDetail
	from System,SewingOutput s WITH (NOLOCK) 
	inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
	left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
	left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
	left join SCIFty sf WITH (NOLOCK) on sf.ID = s.SubconOutFty
	left join factory f WITH (NOLOCK) on f.id=s.FactoryID
	where (o.CateGory NOT IN ('G','A') or s.Category='M') 
	and s.OutputDate between @BeginDate and @EndDate
	and s.FactoryID IN (SELECT FactoryID FROM @FactoryTable)
	;
	INSERT INTO @tmpSewingGroup
	select OutputDate
		   ,Category
		   , Shift
		   , SewingLineID
		   , ActManPower
		   , Team
		   , OrderId
		   , ComboType
		   , WorkHour = Round(sum(WorkHour),3)
		   , QAQty = sum(QAQty)
		   , InlineQty = sum(InlineQty)
		   , OrderCategory
		   , LocalOrder
		   , FactoryID
		   , OrderProgram
		   , MockupProgram
		   , OrderCPU
		   , OrderCPUFactor
		   , MockupCPU
		   , MockupCPUFactor
		   , OrderStyle
		   , MockupStyle
		   , Rate
		   , StdTMS
		   , IIF(Shift <> 'O' and Category <> 'M' and LocalOrder = 1, 'I',Shift) as LastShift
		   , SubconInType
		   , [SubconOutFty] = isnull(SubconOutFty,'')
	----INTO #tmpSewingGroup
	from @tmpSewingDetail
	group by OutputDate, Category, Shift, SewingLineID, Team, OrderId, ComboType
			 , OrderCategory, LocalOrder, FactoryID, OrderProgram, MockupProgram
			 , OrderCPU, OrderCPUFactor, MockupCPU, MockupCPUFactor, OrderStyle
			 , MockupStyle, Rate, StdTMS,SubconInType,isnull(SubconOutFty,'')
			,ActManPower
	;
	INSERT INTO @tmp1stFilter
	select t.*
			, isnull(w.Holiday, 0) as Holiday
	--INTO #tmp1stFilter
	from @tmpSewingGroup t
	left join WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID 
											and w.Date = t.OutputDate 
											and w.SewingLineID = t.SewingLineID
	;
	INSERT INTO @AllDetail
	select OutputDate
		   , Shift = IIF(LastShift = 'D', 'Day'
										, IIF(LastShift = 'N', 'Night'
															 , IIF(LastShift = 'O', 'Subcon-Out'
														 						  , 'Subcon-In')))
		   , Team
		   , SewingLineID
		   , OrderId
		   , Style = IIF(Category = 'M', MockupStyle, OrderStyle)
		   , QAQty
		   , ActManPower
		   , Program = IIF(Category = 'M',MockupProgram,OrderProgram)
		   , WorkHour
		   , StdTMS
		   , MockupCPU
		   , MockupCPUFactor
		   , OrderCPU
		   , OrderCPUFactor
		   , Rate
		   , Category
		   , LastShift
		   , ComboType
		   , FactoryID
		   , SubconInType
		   , SubconOutFty
	----INTO #tmp
	from @tmp1stFilter
	;

	---- 若要排除姊妹廠，則算出姊妹場的數量，再用總數去扣掉
	IF @Exclused_Non_Sister_Subcon = 'Y'
		BEGIN
			/*non Sister SubCon In*/
			;with tmpQty as (
				select StdTMS
					   , QAQty = Sum(QAQty)
					   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
					   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
				from @AllDetail
				where LastShift = 'I' and SubconInType in ('0','3')
				group by StdTMS
			)
			INSERT INTO @Return
			SELECT   TotalCPU = ISNULL( ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3) ,0)   - ISNULL((select q.TotalCPU from tmpQty q),0)
				   , ManHour = ISNULL( ROUND(Sum(WorkHour * ActManPower), 2) ,0)   - ISNULL((select q.ManHour from tmpQty q),0)
			from @AllDetail
			where LastShift <> 'O'
		END
	ELSE IF @Exclused_Non_Sister_Subcon = 'N'
		BEGIN
			INSERT INTO @Return
			/*Total CPU & Manhours*/
			select  TotalCPU = ISNULL( ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)  ,0) 
					,ManHour = ISNULL( ROUND(Sum(WorkHour * ActManPower), 2) ,0) 
			from @AllDetail
			where LastShift <> 'O' 
		END
	
	ELSE   ---- 其餘狀況預設帶不排除姊妹廠資料
		BEGIN
			INSERT INTO @Return
			/*Total CPU & Manhours*/
			select  TotalCPU = ISNULL( ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)  ,0) 
					,ManHour = ISNULL( ROUND(Sum(WorkHour * ActManPower), 2) ,0) 
			from @AllDetail
			where LastShift <> 'O' 
		END
	;
	return;
end

GO



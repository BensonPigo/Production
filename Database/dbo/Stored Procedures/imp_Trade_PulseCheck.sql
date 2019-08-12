CREATE PROCEDURE [dbo].[imp_Trade_PulseCheck]
		@Year int = 0,
		@Month int = 0,
		@MDivisionID varchar(8) = '',
		@UserID varchar(10) = ''
AS 

BEGIN
	Set NoCount On;

	--declare @Year int = 0, @Month int = 0, @MDivisionID varchar(8) ='', @UserID varchar(10) = ''
	
	--set @Year = 2019
	--set @Month = 5

	--@Year, @Month 為必輸條件
	if @Year <= 1911
	begin  
		return 
	end 

	if @Month < 1 or @Month > 12
	begin 
		return 
	end

	if @MDivisionID <> ''
	begin
		if exists
		(
			select MDivisionID
			from [PMS\pmsdb\PH1].[Production].dbo.Factory
			where ID = @MDivisionID
		)
		begin
			select @MDivisionID = MDivisionID
			from [PMS\pmsdb\PH1].[Production].dbo.Factory
			where ID = @MDivisionID
		end
			   
		if not exists(
			select 1
			from (
				select * from [PMS\pmsdb\PH1].[Production].dbo.MDivision  union all
				select * from [PMS\pmsdb\PH2].[Production].dbo.MDivision  union all
				select * from [PMS\pmsdb\ESP].[Production].dbo.MDivision  union all
				select * from [PMS\pmsdb\SNP].[Production].dbo.MDivision  union all
				select * from [PMS\pmsdb\SPT].[Production].dbo.MDivision  union all
				select * from [PMS\pmsdb\SPR].[Production].dbo.MDivision  union all
				select * from [PMS\pmsdb\SPS].[Production].dbo.MDivision  union all
				select * from [PMS\pmsdb\HZG].[Production].dbo.MDivision  union all
				select * from [PMS\pmsdb\HXG].[Production].dbo.MDivision  union all
				select * from [PMS\pmsdb\NAI].[Production].dbo.MDivision  
			)a
			where ID = @MDivisionID)
		begin 
			return 
		end
	end
			
	declare @firstDay as datetime , @lastDay as datetime, @sql as nvarchar(max)


select @firstDay = DATEADD(mm, DATEDIFF(mm, '', cast(@Year as varchar) + right(('00'+cast(@Month as varchar)),2) + '01'), '')   
	,@lastDay = DATEADD(day, -1, DATEADD(mm, DATEDIFF(mm, '', cast(@Year as varchar) + right(('00'+cast(@Month as varchar)),2) + '01')+1, ''))

select *
into #tmp_Dropdownlist
from Tradedb.Trade.dbo.Dropdownlist d
where d.type = 'PulseCheck'
and d.Name in ('Performed','Working Days','Avg Working Hours','PPH','Efficiency','Direct Manpower')


--產生結構
select  s.OutputDate
		, s.Category
		, s.Shift
		, s.SewingLineID
		, [ActManPower] = IIF(sd.QAQty = 0, s.Manpower, s.Manpower * sd.QAQty)
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
        , [Rate] =isnull(dbo.[GetOrderLocation_Rate_ByLinked](o.id ,sd.ComboType, o.MDivisionID),100)/100
		, System.StdTMS
        , o.SubconInSisterFty
        , [SubconOutFty] = iif(sf.id is null,'Other',s.SubconOutFty)
		, s.MDivisionID
INTO #tmpSewingDetail
from [PMS\pmsdb\PH1].[Production].dbo.System, [PMS\pmsdb\PH1].[Production].dbo.SewingOutput s WITH (NOLOCK) 
left join [PMS\pmsdb\PH1].[Production].dbo.SCIFty sf WITH (NOLOCK) on sf.ID = s.SubconOutFty
left join [PMS\pmsdb\PH1].[Production].dbo.factory f WITH (NOLOCK) on f.id=s.FactoryID
inner join [PMS\pmsdb\PH1].[Production].dbo.SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join [PMS\pmsdb\PH1].[Production].dbo.Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
left join [PMS\pmsdb\PH1].[Production].dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId 
where 1= 2


--組資料
DECLARE CURSOR_ CURSOR FOR
select name 
from sys.servers 
where left(name,9) = 'PMS\pmsdb' order by server_id

declare @_name as varchar(20)

OPEN CURSOR_
FETCH NEXT FROM CURSOR_ INTO @_name
While @@FETCH_STATUS = 0
Begin		 
		set @sql = '
		select  s.OutputDate
				, s.Category
				, s.Shift
				, s.SewingLineID
				, [ActManPower] = IIF(sd.QAQty = 0, s.Manpower, s.Manpower * sd.QAQty)
				, s.Team
				, sd.OrderId
				, sd.ComboType
				, sd.WorkHour
				, sd.QAQty
				, sd.InlineQty
				, [OrderCategory] = isnull(o.Category,'''')
				, o.LocalOrder
				, s.FactoryID
				, [OrderProgram] = isnull(o.ProgramID,'''') 
				, [MockupProgram] = isnull(mo.ProgramID,'''')
				, [OrderCPU] = isnull(o.CPU,0)
				, [OrderCPUFactor] = isnull(o.CPUFactor,0)
				, [MockupCPU] = isnull(mo.Cpu,0)
				, [MockupCPUFactor] = isnull(mo.CPUFactor,0)
				, [OrderStyle] = isnull(o.StyleID,'''')
				, [MockupStyle] = isnull(mo.StyleID,'''')
				, [Rate] = isnull(dbo.[GetOrderLocation_Rate_ByLinked](o.id ,sd.ComboType, o.MDivisionID),100)/100
				, System.StdTMS
				, o.SubconInSisterFty
				, [SubconOutFty] = iif(sf.id is null,''Other'',s.SubconOutFty)
				, [MDivisionID] = iif(f.CountryID = ''PH'', isnull(f.KPICode,f.ID) , s.MDivisionID)
		--INTO #tmpSewingDetail
		from [' + @_name + '].[Production].dbo.System, [' + @_name + '].[Production].dbo.SewingOutput s WITH (NOLOCK) 
		left join [' + @_name + '].[Production].dbo.SCIFty sf WITH (NOLOCK) on sf.ID = s.SubconOutFty
		left join [' + @_name + '].[Production].dbo.factory f WITH (NOLOCK) on f.id=s.FactoryID
		inner join [' + @_name + '].[Production].dbo.SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
		left join [' + @_name + '].[Production].dbo.Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
		left join [' + @_name + '].[Production].dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId  
		where s.OutputDate >= cast(''' + cast(@firstDay as varchar) + ''' as date)
		and s.OutputDate <= cast(''' + cast(@lastDay as varchar) + ''' as date)
		and (o.CateGory != ''G'' or s.Category=''M'') 
		and f.type <> ''S''
		and s.MDivisionID = iif(''' + @MDivisionID + ''' ='''', s.MDivisionID, ''' + @MDivisionID + ''')
		'

		--select @_name name, @sql sql

		insert into #tmpSewingDetail
		EXECUTE sp_executesql @sql

FETCH NEXT FROM CURSOR_ INTO @_name
End
CLOSE CURSOR_
DEALLOCATE CURSOR_ 

--select * from #tmpSewingDetail

--基本資料
select OutputDate,Category
	   , Shift
	   , SewingLineID
	   , ActManPower1 = Sum(ActManPower)
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
       , SubconInSisterFty
       , [SubconOutFty] = isnull(SubconOutFty,'')
	   , MDivisionID
INTO #tmpSewingGroup
from #tmpSewingDetail
group by OutputDate, Category, Shift, SewingLineID, Team, OrderId, ComboType
		 , OrderCategory, LocalOrder, FactoryID, OrderProgram, MockupProgram
		 , OrderCPU, OrderCPUFactor, MockupCPU, MockupCPUFactor, OrderStyle
		 , MockupStyle, Rate, StdTMS,SubconInSisterFty,isnull(SubconOutFty,'')
		 , MDivisionID


select t.*
	   , isnull(w.Holiday, 0) as Holiday
	   , IIF(isnull(QAQty, 0) = 0, ActManPower1, (ActManPower1 / QAQty)) as ActManPower
INTO #tmp1stFilter
from #tmpSewingGroup t
left join WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID 
									  and w.Date = t.OutputDate 
									  and w.SewingLineID = t.SewingLineID 

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
     , SubconInSisterFty
     , SubconOutFty
	 , MDivisionID
into #tmp
from #tmp1stFilter

drop table #tmpSewingDetail,#tmpSewingGroup,#tmp1stFilter 
  

--運算
select [TotalManHour] = sum(ManHour)
	, MDivisionID 
into #tmp_ManHour
from 
(
	select OutputDate
		   , StdTMS
		   , QAQty = Sum(QAQty)
		   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
		   , MDivisionID
	from #tmp
	where LastShift <> 'O'
	group by OutputDate, StdTMS, MDivisionID
)a
group by MDivisionID

--SQL By Date
select [TotalManpower] = sum(ManPower)
	, MDivisionID 
into #tmp_TotalManpower
from 
(
	select OutputDate
		   , ManPower = Sum(a.Manpower) - sum(iif(LastShift = 'I', 0, isnull(d.ManPower, 0)))
		   , MDivisionID 
	from ( 
		select OutputDate
			   , FactoryID
			   , SewingLineID
			   , LastShift
			   , Team
			   , ManPower = Max(ActManPower)
			   , MDivisionID
		from #tmp
		where LastShift <> 'O'
		group by OutputDate, FactoryID, SewingLineID, LastShift, Team , MDivisionID
	) a
	outer apply(
		select ManPower
		from (
			select OutputDate
				   , FactoryID
				   , SewingLineID
				   , LastShift
				   , Team
				   , ManPower = Max(ActManPower)
				   , MDivisionID
			from #tmp
			where LastShift <> 'O'
			group by OutputDate, FactoryID, SewingLineID, LastShift, Team, MDivisionID
		) m2
		where m2.LastShift = 'I' 
			  and m2.Team = a.Team 
			  and m2.SewingLineID = a.SewingLineID	
			  and a.OutputDate = m2.OutputDate
			  and m2.FactoryID = a.FactoryID	
			  and m2.MDivisionID = a.MDivisionID	
	) d
	group by OutputDate, MDivisionID
)a
group by MDivisionID

 
select StdTMS
	   , QAQty = Sum(QAQty)
	   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
	   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	   , MDivisionID
into #tmpQty
from #tmp
where LastShift <> 'O' 
      --排除Subcon in non sister資料
      and LastShift <> 'I'
	  or (LastShift = 'I' and SubconInSisterFty = 1)
group by StdTMS, MDivisionID

select ManPower = Sum(a.Manpower)  - sum(iif(LastShift = 'I', 0, isnull(d.ManPower, 0)))
	,MDivisionID
into #tmpTtlManPower
from (
	select OutputDate
			, FactoryID
			, SewingLineID
			, LastShift
			, Team
			, ManPower = Max(ActManPower)
			, MDivisionID
	from #tmp
	where LastShift <> 'O'
	--排除 subcon in non sister的數值
    and ((LastShift <> 'I') or ( LastShift = 'I' and SubconInSisterFty <> 0 ))   
	group by OutputDate, FactoryID, SewingLineID, LastShift, Team ,MDivisionID
) a
outer apply
(
	select ManPower
	from (
		select OutputDate
				, FactoryID
				, SewingLineID
				, LastShift
				, Team
				, ManPower = Max(ActManPower)
				,SubconInSisterFty
				,MDivisionID
		from #tmp
		where LastShift <> 'O'
		group by OutputDate, FactoryID, SewingLineID, LastShift, Team,SubconInSisterFty, MDivisionID
	) m2
	where  (m2.LastShift = 'I' and m2.SubconInSisterFty = 1)
			and m2.Team = a.Team 
			and m2.SewingLineID = a.SewingLineID	
			and a.OutputDate = m2.OutputDate
			and m2.FactoryID = a.FactoryID	
			and m2.MDivisionID = a.MDivisionID
) d
group by MDivisionID

select  [PPH] = IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 3))
	   , [Efficiency] = (IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 3))/(3600*1.0/1400*1.0))
	   , [Avg Working Hours] = IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2)) 
	   , q.MDivisionID
into #tmp_excludeInOutTotal
from #tmpQty q
left join #tmpTtlManPower mp on q.MDivisionID = mp.MDivisionID
 
select  [Total CPU Included Subcon-In] = CPU
	, MDivisionID 
into #tmp_cpuFactor
from 
(
	select CPU = sum(CPU)
		   , MDivisionID
   	from 
	(
		select CPUFactor = IIF(Category = 'M', MockupCPUFactor, OrderCPUFactor)
			   , QAQty
			   , CPU = QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)
			   , Style
			   , MDivisionID
		from #tmp
	) a
   	group by MDivisionID
)a

select [Subcon-Out Total CPU(sister)] =sum(TtlCPU)
	, MDivisionID
into #tmp_SubconCPU_sister
from
(
    Select Type = 'O'
		   , Company = t.SubconOutFty
		   , TtlCPU = ROUND(Sum(t.QAQty*IIF(t.Category = 'M', t.MockupCPU * t.MockupCPUFactor, t.OrderCPU * t.OrderCPUFactor * t.Rate)),3)
		   , MDivisionID
	from #tmp t
	where LastShift = 'O'
	and t.SubconOutFty <> 'Other'
	group by t.SubconOutFty, MDivisionID
)a 
group by MDivisionID

SELECT [Working Days] = COUNT(*)
	, MDivisionID
into #tmp_WorkingDays
from(
	select OutputDate, MDivisionID
	from #tmp 
	where LastShift <> 'O'
	group by OutputDate, MDivisionID
)a
group by MDivisionID 


--寫入資料
DECLARE CURSOR_ CURSOR FOR
select a.MDivisionID
	,[Performed] = cast(c.[Total CPU Included Subcon-In] - isnull(d.[Subcon-Out Total CPU(sister)],0) as decimal(18,2))
	,[Working Days] = cast(e.[Working Days] as decimal(18, 0))
	,[Avg Working Hours] =cast(a.[Avg Working Hours] as decimal(18,2))
	,[PPH] = cast(a.[PPH] as decimal(18,2))
	,[Efficiency] = cast(a.[Efficiency] as decimal(18,2))
	,[Direct Manpower] = cast(b.TotalManpower / e.[Working Days] as decimal(18,0))
from #tmp_excludeInOutTotal a
left join #tmp_TotalManpower b on a.MDivisionID = b.MDivisionID
left join #tmp_cpuFactor c on a.MDivisionID =c.MDivisionID
left join #tmp_SubconCPU_sister d on a.MDivisionID = d.MDivisionID
left join #tmp_WorkingDays e on a.MDivisionID = e.MDivisionID
left join #tmp_ManHour f on a.MDivisionID = f.MDivisionID


declare @_MDivisionID as varchar(8)
	, @Performed as decimal(18,6)
	, @WorkingDays as decimal(18,6)
	, @AvgWorkingHours as decimal(18,6)
	, @PPH as decimal(18,6)
	, @Efficiency as decimal(18,6)
	, @DirectManpower as decimal(18,6)
	, @ItemID as varchar(50)
 

OPEN CURSOR_
FETCH NEXT FROM CURSOR_ INTO @_MDivisionID, @Performed, @WorkingDays, @AvgWorkingHours, @PPH, @Efficiency, @DirectManpower
While @@FETCH_STATUS = 0
Begin	
	--Avg Working Hours
	select @ItemID = ID from #tmp_Dropdownlist where Name = 'Avg Working Hours' 
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID)
	begin
		update tradedb.trade.dbo.PulseCheck 
		set value = @AvgWorkingHours, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID 
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[MDivision],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_MDivisionID, @AvgWorkingHours, @UserID, GETDATE(), null, null
	end

	--PPH
	select @ItemID = ID from #tmp_Dropdownlist where Name = 'PPH'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID)
	begin
		update tradedb.trade.dbo.PulseCheck 
		set value = @PPH, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID 
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[MDivision],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_MDivisionID, @PPH, @UserID, GETDATE(), null, null
	end 

	--Efficiency
	select @ItemID = ID from #tmp_Dropdownlist where Name = 'Efficiency'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID)
	begin
		update tradedb.trade.dbo.PulseCheck 
		set value = @Efficiency, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID 
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[MDivision],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_MDivisionID, @Efficiency, @UserID, GETDATE(), null, null
	end  

	--Direct Manpower
	select @ItemID = ID from #tmp_Dropdownlist where Name = 'Direct Manpower'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID)
	begin
		update tradedb.trade.dbo.PulseCheck 
		set value = @DirectManpower, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID 
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[MDivision],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_MDivisionID, @DirectManpower, @UserID, GETDATE(), null, null
	end  


	--Performed
	select @ItemID = ID from #tmp_Dropdownlist where Name = 'Performed'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID)
	begin
		update tradedb.trade.dbo.PulseCheck 
		set value = @Performed, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID 
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[MDivision],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_MDivisionID, @Performed, @UserID, GETDATE(), null, null
	end  

	--Working Days
	select @ItemID = ID from #tmp_Dropdownlist where Name = 'Working Days'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID)
	begin
		update tradedb.trade.dbo.PulseCheck 
		set value = @WorkingDays, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and MDivision = @_MDivisionID 
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[MDivision],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_MDivisionID, @WorkingDays, @UserID, GETDATE(), null, null
	end

FETCH NEXT FROM CURSOR_ INTO @_MDivisionID, @Performed, @WorkingDays, @AvgWorkingHours, @PPH, @Efficiency, @DirectManpower
End
CLOSE CURSOR_
DEALLOCATE CURSOR_ 

drop table #tmp,#tmp_Dropdownlist

drop table #tmp_TotalManpower,#tmp_excludeInOutTotal,#tmpQty,#tmpTtlManPower,#tmp_cpuFactor,#tmp_SubconCPU_sister,#tmp_WorkingDays


--select b.name, a.* 
--from tradedb.trade.dbo.PulseCheck a
--left join Tradedb.Trade.dbo.Dropdownlist b on a.ItemID = b.ID and type = 'PulseCheck'

--select * from Tradedb.Trade.dbo.Dropdownlist

End

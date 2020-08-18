CREATE PROCEDURE [dbo].[imp_Trade_PulseCheck]
		@Year int = 0,
		@Month int = 0,
		@CountryID varchar(8) = '',
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

	if @CountryID <> ''
	begin  
		if not exists(
			select 1
			from (
				select * from [PMS\pmsdb\PH1].[Production].dbo.factory  union all
				select * from [PMS\pmsdb\PH2].[Production].dbo.factory  union all
				select * from [PMS\pmsdb\ESP].[Production].dbo.factory  union all
				select * from [PMS\pmsdb\SNP].[Production].dbo.factory  union all
				select * from [PMS\pmsdb\SPT].[Production].dbo.factory  union all
				select * from [PMS\pmsdb\SPR].[Production].dbo.factory  union all
				select * from [PMS\pmsdb\SPS].[Production].dbo.factory  union all
				select * from [PMS\pmsdb\HZG].[Production].dbo.factory  union all
				select * from [PMS\pmsdb\HXG].[Production].dbo.factory  union all
				select * from [PMS\pmsdb\NAI].[Production].dbo.factory  
			)a
			where CountryID = @CountryID)
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
and d.Name in (
	'Performed',
	'Working Days',
	'Avg Working Hours',
	'PPH',
	'Efficiency',
	'Direct Manpower',
	'SCI Loading',
	'Capacity',
	'Performed / Loading',
	'Performed / Capacity',
	'CMP Absent',
	'Subprocess CPU',
	'TTL DL Working Hour',
	'Direct Manpower'
)

--Basic
BEGIN
--產生結構
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
        , [Rate] =isnull(dbo.[GetOrderLocation_Rate_ByLinked](o.id ,sd.ComboType, o.MDivisionID),100)/100
		, System.StdTMS
        , o.SubconInType
        , [SubconOutFty] = iif(sf.id is null,'Other',s.SubconOutFty)
		, f.CountryID
		, f.Zone
		, [SewingOutputID] = S.ID 
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
				, [ActManPower] = s.Manpower
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
				, o.SubconInType
				, [SubconOutFty] = iif(sf.id is null,''Other'',s.SubconOutFty)
				, f.CountryID
				, f.Zone
				, [SewingOutputID] = S.ID 
		--INTO #tmpSewingDetail
		from [' + @_name + '].[Production].dbo.System, [' + @_name + '].[Production].dbo.SewingOutput s WITH (NOLOCK) 
		left join [' + @_name + '].[Production].dbo.SCIFty sf WITH (NOLOCK) on sf.ID = s.SubconOutFty
		left join [' + @_name + '].[Production].dbo.factory f WITH (NOLOCK) on f.id=s.FactoryID
		inner join [' + @_name + '].[Production].dbo.SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
		left join [' + @_name + '].[Production].dbo.Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
		left join [' + @_name + '].[Production].dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId  
		where s.OutputDate >= cast(''' + cast(@firstDay as varchar) + ''' as date)
		and s.OutputDate <= cast(''' + cast(@lastDay as varchar) + ''' as date)
		and (o.CateGory NOT IN (''G'',''A'') or s.Category=''M'') 
		and f.type <> ''S''
		and f.Junk = 0		
		'

		if @CountryID <>  ''
		begin 
			set @sql = @sql + ' and f.CountryID = ''' + @CountryID + ''' '
		end

		--select @_name name, @sql sql

		insert into #tmpSewingDetail
		EXECUTE sp_executesql @sql

FETCH NEXT FROM CURSOR_ INTO @_name
End
CLOSE CURSOR_
DEALLOCATE CURSOR_ 

--select * from #tmpSewingDetail

--基本資料
select CountryID, Zone 
	,[ManPower] = Sum(ActManPower) 
into #tmp_CMPAbsent
from
(
	select CountryID
		, Zone
		, ActManPower
		, SewingOutputID
	from #tmpSewingDetail
	group by CountryID, Zone, SewingOutputID, ActManPower 
)a
group by CountryID, Zone

select OutputDate,Category
	   , Shift
	   , SewingLineID
	   , ActManPower1 = ActManPower
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
	   , CountryID
	   , Zone
INTO #tmpSewingGroup
from #tmpSewingDetail
group by OutputDate, Category, Shift, SewingLineID, Team, OrderId, ComboType
		 , OrderCategory, LocalOrder, FactoryID, OrderProgram, MockupProgram
		 , OrderCPU, OrderCPUFactor, MockupCPU, MockupCPUFactor, OrderStyle
		 , MockupStyle, Rate, StdTMS,isnull(SubconOutFty,'')
		 , CountryID, Zone, SubconInType, ActManPower

select t.*
	   , isnull(w.Holiday, 0) as Holiday
	   , ActManPower1 as ActManPower
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
       , SubconInType
       , SubconOutFty
	   , CountryID
	   , Zone
into #tmp
from #tmp1stFilter

END

drop table #tmpSewingDetail,#tmpSewingGroup,#tmp1stFilter 

-- Subprocess CPU
-- declare @_name as varchar(100), @CountryID as varchar(100), @sql as nvarchar(max)
Begin

create table #tmp_SubprocessCPUper
(
	id_num int IDENTITY(1,1), 
	ArtworkTypeID nvarchar(50),
	CountryID varchar(2),
	Zone varchar(8),
	rs varchar(20),
	Price numeric(18,4)
)

DECLARE CURSOR_ CURSOR FOR
select name 
from sys.servers 
where left(name,9) = 'PMS\pmsdb' order by server_id

OPEN CURSOR_
FETCH NEXT FROM CURSOR_ INTO @_name
While @@FETCH_STATUS = 0
Begin		 
		set @sql = '
		Select ID
				, rs = iif(ProductionUnit = ''TMS'', ''CPU''
		   										, iif(ProductionUnit = ''QTY'', ''AMT''
		   																	, '''')),
				[DecimalNumber] =case    when ProductionUnit = ''QTY'' then 4
										when ProductionUnit = ''TMS'' then 3
										else 0 end
		into #tmpArtwork
		from [' + @_name + '].[Production].dbo.ArtworkType WITH (NOLOCK)
		where Classify in (''I'',''A'',''P'') 
				and IsTtlTMS = 0
				and IsPrintToCMP=1

		--準備台北資料(須排除這些)
		select ps.ID
		into #TPEtmp
		from [' + @_name + '].[Production].dbo.PO_Supp ps
		inner join [' + @_name + '].[Production].dbo.PO_Supp_Detail psd on ps.ID=psd.id and ps.SEQ1=psd.Seq1
		inner join [' + @_name + '].[Production].dbo.Fabric fb on psd.SCIRefno = fb.SCIRefno 
		inner join [' + @_name + '].[Production].dbo.MtlType ml on ml.id = fb.MtlTypeID
		where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
		and ml.isThread=1 
		and ps.SuppID <> ''FTY'' and ps.Seq1 not Like ''5%''

		select ot.ArtworkTypeID
				, a.OrderId
				, a.ComboType
				, Price = sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate_ByLinked](a.OrderId ,a.ComboType, o.MDivisionID), 100) / 100)
				, a.CountryID
				, a.Zone
		into  #tmpAllSubprocess
		from #tmp a
		inner join [' + @_name + '].[Production].dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
		inner join [' + @_name + '].[Production].dbo.Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.FtyGroup = a.FactoryID and o.Category NOT IN (''G'',''A'')
		where ((a.LastShift = ''O'' and o.LocalOrder <> 1) or (a.LastShift <> ''O'') ) 
				--排除 subcon in non sister的數值
			  and ((a.LastShift <> ''I'') or ( a.LastShift = ''I'' and a.SubconInType not in (''0'',''3'') ))           
			  and ot.Price > 0 		    
			  and ((ot.ArtworkTypeID = ''SP_THREAD'' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
				  or ot.ArtworkTypeID <> ''SP_THREAD'')
		group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price, a.CountryID, a.Zone, o.MDivisionID

		--FMS傳票部分顯示AT不分Hand/Machine，是因為政策問題，但比對Sewing R02時，會有落差，請根據SP#落在Hand CPU:10 /Machine:5，則只撈出Hand CPU:10這筆，抓其大值，以便加總總和等同於FMS傳票AT
		-- 當AT(Machine) = AT(Hand)時, 也要將Price歸0 (ISP20190520)
		update s set s.Price = 0
			from #tmpAllSubprocess s
			inner join (select * from #tmpAllSubprocess where ArtworkTypeID = ''AT (HAND)'') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType
			where s.ArtworkTypeID = ''AT (MACHINE)''  and s.Price <= a.Price

		update s set s.Price = 0
			from #tmpAllSubprocess s
			inner join (select * from #tmpAllSubprocess where ArtworkTypeID = ''AT (MACHINE)'') a on s.OrderId = a.OrderId and s.ComboType = a.ComboType
			where s.ArtworkTypeID = ''AT (HAND)''  and s.Price <= a.Price

		select ArtworkTypeID = t1.ID
			   , t2.CountryID
			   , t2.Zone
			   , t1.rs 
			   , Price = cast(isnull(sum(Round(Price,t1.DecimalNumber)), 0) as numeric(18,4))
		from #tmpArtwork t1
		left join #tmpAllSubprocess t2 on t2.ArtworkTypeID = t1.ID
		where rs = ''CPU''
		'

		if @CountryID <>  ''
		begin 
			set @sql = @sql + ' and t2.CountryID = ''' + @CountryID + ''' '
		end  

		set @sql = @sql + ' 
		group by t1.ID, t2.CountryID, t2.Zone, t1.rs 
		having isnull(sum(Price), 0) > 0 
		'

		insert into #tmp_SubprocessCPUper
		EXECUTE sp_executesql @sql

FETCH NEXT FROM CURSOR_ INTO @_name
End
CLOSE CURSOR_
DEALLOCATE CURSOR_ 

select t.CountryID, t.Zone,[Price] = SUM(t.Price)
into #tmp_SubprocessCPU
from #tmp_SubprocessCPUper t
group by t.CountryID, t.Zone

End

--GetPAMSHoliday / GetPAMSAttendance
Begin

create table #tmp_Holiday ( Holiday Date )
create table #tmp_Attendance ( Manpower int ) 

create table #tmp_PAMSHoliday
(
	id_num int IDENTITY(1,1), 
	FactoryID varchar(8),
	Holiday Date,
)

create table #tmp_PAMSAttendance
(
	id_num int IDENTITY(1,1), 
	CountryID varchar(2),
	Zone varchar(8),
	Manpower int,
)

declare @Factory as nvarchar(8), @Zone as varchar(8), @Country as varchar(2)
declare @YM as varchar(6) = concat(@Year, right(concat('00', @Month),2))

DECLARE CURSOR_ CURSOR FOR
select distinct t.FactoryID
from #tmp t

OPEN CURSOR_
FETCH NEXT FROM CURSOR_ INTO @Factory
While @@FETCH_STATUS = 0
Begin

	insert into #tmp_Holiday
	exec tradedb.trade.dbo.GetPAMSHoliday @Factory, @YM

	insert into #tmp_PAMSHoliday
	select [FactoryID] = @Factory, Holiday from #tmp_Holiday

	truncate table #tmp_Holiday
	FETCH NEXT FROM CURSOR_ INTO @Factory
END
CLOSE CURSOR_
DEALLOCATE CURSOR_ 

DECLARE CURSOR_ CURSOR FOR
select distinct t.CountryID, t.Zone
from #tmp t

OPEN CURSOR_
FETCH NEXT FROM CURSOR_ INTO @Country, @Zone
While @@FETCH_STATUS = 0
Begin

	insert into #tmp_Attendance
	exec tradedb.trade.dbo.GetPAMSAttendance @Country, @Zone, @YM

	insert into #tmp_PAMSAttendance
	select [CountryID] = @Country, [Zone] = @Zone, Manpower from #tmp_Attendance

	truncate table #tmp_Attendance
	FETCH NEXT FROM CURSOR_ INTO @Country, @Zone
END
CLOSE CURSOR_
DEALLOCATE CURSOR_ 
End

drop table #tmp_Holiday, #tmp_Attendance
-------------------------------------------------------------------

--運算
BEGIN

--SQL By Date
select [TotalManpower] = sum(ManPower)
	   , CountryID
	   , Zone 
into #tmp_TotalManpower
from 
(
	select OutputDate
		   , ManPower = Sum(a.Manpower) - sum(iif(LastShift = 'I', 0, isnull(d.ManPower, 0)))
		   , CountryID
		   , Zone
	from ( 
		select OutputDate
			   , FactoryID
			   , SewingLineID
			   , LastShift
			   , Team
			   , ManPower = Max(ActManPower)
			   , CountryID
			   , Zone
		from #tmp
		where LastShift <> 'O'
		group by OutputDate, FactoryID, SewingLineID, LastShift, Team, CountryID, Zone
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
				   , CountryID
				   , Zone
			from #tmp
			where LastShift <> 'O'
			group by OutputDate, FactoryID, SewingLineID, LastShift, Team, CountryID, Zone
		) m2
		where m2.LastShift = 'I' 
			  and m2.Team = a.Team 
			  and m2.SewingLineID = a.SewingLineID	
			  and a.OutputDate = m2.OutputDate
			  and m2.FactoryID = a.FactoryID	
			  and m2.CountryID = a.CountryID	
			  and m2.Zone = a.Zone
	) d
	group by OutputDate, CountryID, Zone
)a
group by CountryID, Zone 
 
select StdTMS
	   , QAQty = Sum(QAQty)
	   , ManHour = ROUND(Sum(WorkHour * ActManPower), 2)
	   , TotalCPU = ROUND(Sum(QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)), 3)
	   , CountryID
	   , Zone
into #tmpQty
from #tmp
where LastShift <> 'O' 
      --排除Subcon in non sister資料
      and LastShift <> 'I'
	  or (LastShift = 'I' and SubconInType in ('1','2'))
group by StdTMS, CountryID, Zone

select ManPower = Sum(a.Manpower)  - sum(iif(LastShift = 'I', 0, isnull(d.ManPower, 0)))
	, CountryID
	, Zone
into #tmpTtlManPower
from (
	select OutputDate
			, FactoryID
			, SewingLineID
			, LastShift
			, Team
			, ManPower = Max(ActManPower)
			, CountryID
			, Zone
	from #tmp
	where LastShift <> 'O'
	--排除 subcon in non sister的數值
    and ((LastShift <> 'I') or ( LastShift = 'I' and SubconInType not in ('0','3')))   
	group by OutputDate, FactoryID, SewingLineID, LastShift, Team ,CountryID, Zone
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
				, SubconInType
				, CountryID
				, Zone
		from #tmp
		where LastShift <> 'O'
		group by OutputDate, FactoryID, SewingLineID, LastShift, Team, SubconInType, CountryID, Zone
	) m2
	where  (m2.LastShift = 'I' and m2.SubconInType in ('1','2'))
			and m2.Team = a.Team 
			and m2.SewingLineID = a.SewingLineID	
			and a.OutputDate = m2.OutputDate
			and m2.FactoryID = a.FactoryID	
			and m2.CountryID = a.CountryID
			and m2.Zone = a.Zone
) d
group by CountryID, Zone

select  [PPH] = IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 3))
	   , [Efficiency] = (IIF(q.ManHour = 0, 0, Round(isnull(q.TotalCPU,0) / q.ManHour, 3))/(3600*1.0/1400*1.0))
	   , [Avg Working Hours] = IIF(isnull(mp.ManPower, 0) = 0, 0, Round(q.ManHour / mp.ManPower, 2)) 
	   , q.CountryID
	   , q.Zone
into #tmp_excludeInOutTotal
from #tmpQty q
left join #tmpTtlManPower mp on q.CountryID = mp.CountryID and q.Zone = mp.Zone
 
select  [Total CPU Included Subcon-In] = CPU
	, CountryID
	, Zone
into #tmp_cpuFactor
from 
(
	select CPU = sum(CPU)
		   , CountryID
		   , Zone
   	from 
	(
		select CPUFactor = IIF(Category = 'M', MockupCPUFactor, OrderCPUFactor)
			   , QAQty
			   , CPU = QAQty * IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)
			   , Style
			   , CountryID
			   , Zone
		from #tmp
	) a
   	group by CountryID, Zone
)a

select [Subcon-Out Total CPU(sister)] =sum(TtlCPU)
	, CountryID
	, Zone
into #tmp_SubconCPU_sister
from
(
    Select Type = 'O'
		   , Company = t.SubconOutFty
		   , TtlCPU = ROUND(Sum(t.QAQty*IIF(t.Category = 'M', t.MockupCPU * t.MockupCPUFactor, t.OrderCPU * t.OrderCPUFactor * t.Rate)),3)
		   , CountryID
		   , Zone
	from #tmp t
	where LastShift = 'O'
	and t.SubconOutFty <> 'Other'
	group by t.SubconOutFty, CountryID, Zone
)a 
group by CountryID, Zone

select [Subcon-Out Total CPU(sister)] =sum(TtlCPU)
	, CountryID
	, Zone
into #tmp_SubconCPU_sister_onlyPerformed
from
(
    Select Type = 'O'
		   , Company = t.SubconOutFty
		   , TtlCPU = ROUND(Sum(t.QAQty*IIF(t.Category = 'M', t.MockupCPU * t.MockupCPUFactor, t.OrderCPU * t.OrderCPUFactor * t.Rate)),3)
		   , CountryID
		   , Zone
	from #tmp t
	where LastShift = 'O'
	and t.Program != 'Uniform'
	and (t.SubconOutFty <> 'Other' or exists (select ID from Tradedb.Trade.dbo.DropDownList ddl where type = 'CMPSpecialSCIFty' and ddl.ID = t.SubconOutFty))
	group by t.SubconOutFty, CountryID, Zone
)a 
group by CountryID, Zone

select t.CountryID, t.Zone, [ManHour] = sum(t.ManHour)
into #tmp_ManHour
from #tmpQty t
group by t.CountryID, t.Zone

SELECT [Working Days] = COUNT(*)
	, CountryID
	, Zone
into #tmp_WorkingDays
from(
	select OutputDate, CountryID, Zone
	from #tmp 
	where LastShift <> 'O'
	group by OutputDate, CountryID, Zone
)a
group by CountryID, Zone

select [CountryID] = p.Region
	,[Zone] = p.Zone
	,[Name] = d.Name
	,[Value] = isnull(p.Value, 0)
into #tmp_PulseCheck
from tradedb.trade.dbo.PulseCheck p
inner join #tmp_excludeInOutTotal t on p.Region = t.CountryID and p.Zone = t.Zone
inner join #tmp_Dropdownlist d on p.ItemID = d.ID
where p.Year = @Year
and p.Month = @Month
and d.Name in ('SCI Loading','Capacity')

END

--寫入資料
DECLARE CURSOR_ CURSOR FOR
select a.CountryID
	, a.Zone
	,[Performed] = cast(c.[Total CPU Included Subcon-In] - isnull(op.[Subcon-Out Total CPU(sister)],0) as decimal(18,2))
	,[Working Days] = cast(e.[Working Days] as decimal(18, 0))
	,[Avg Working Hours] =cast(a.[Avg Working Hours] as decimal(18,2))
	,[PPH] = cast(a.[PPH] as decimal(18,2))
	,[Efficiency] = cast(a.[Efficiency] as decimal(18,2))
	,[Direct Manpower] = cast(iif(b.TotalManpower <= att.Manpower, 0, b.TotalManpower / e.[Working Days]) as decimal(18,0))
	,[Performed Loading] =iif(isnull(p1.Value,0) = 0, 0 ,cast((c.[Total CPU Included Subcon-In] - isnull(d.[Subcon-Out Total CPU(sister)],0)) /p1.Value as decimal(18,2)))
	,[Performed Capacity] =iif(isnull(p2.Value,0) = 0, 0 ,cast((c.[Total CPU Included Subcon-In] - isnull(d.[Subcon-Out Total CPU(sister)],0)) /p2.Value as decimal(18,2)))
	,[CMP Absent] = cm.ManPower
	,[Subprocess CPU] = s.Price
	,[TTL DL Working Hour] = f.ManHour
from #tmp_excludeInOutTotal a
left join #tmp_TotalManpower b on a.CountryID = b.CountryID and a.Zone = b.Zone
left join #tmp_cpuFactor c on a.CountryID =c.CountryID and a.Zone = c.Zone
left join #tmp_SubconCPU_sister d on a.CountryID = d.CountryID and a.Zone = d.Zone
left join #tmp_SubconCPU_sister_onlyPerformed op on a.CountryID = op.CountryID and a.Zone = op.Zone
left join #tmp_WorkingDays e on a.CountryID = e.CountryID and a.Zone = e.Zone
left join #tmp_PulseCheck p1 on a.CountryID = p1.CountryID and a.Zone = p1.Zone and p1.Name = 'SCI Loading'
left join #tmp_PulseCheck p2 on a.CountryID = p2.CountryID and a.Zone = p2.Zone and p2.Name = 'Capacity'
left join #tmp_CMPAbsent cm on a.CountryID = cm.CountryID and a.Zone = cm.Zone
left join #tmp_SubprocessCPU s on a.CountryID = s.CountryID and a.Zone = s.Zone
left join #tmp_ManHour f on a.CountryID = f.CountryID and a.Zone = f.Zone
left join #tmp_PAMSAttendance att on a.CountryID = att.CountryID and a.Zone = att.Zone

declare @_CountryID as varchar(8)
	, @_Zone as varchar(8)
	, @Performed as decimal(18,6)
	, @WorkingDays as decimal(18,6)
	, @AvgWorkingHours as decimal(18,6)
	, @PPH as decimal(18,6)
	, @Efficiency as decimal(18,6)
	, @DirectManpower as decimal(18,6)
	, @ItemID as varchar(50)
 	, @PerformedLoading as decimal(18,6)
	, @PerformedCapacity as decimal(18,6)
	, @CMPAbsent as decimal(18,6)
	, @SubprocessCPU as decimal(18,6)
	, @TTLDLWorkingHour as decimal(18,6)

OPEN CURSOR_
FETCH NEXT FROM CURSOR_ INTO @_CountryID, @_Zone, @Performed, @WorkingDays, @AvgWorkingHours, @PPH, @Efficiency, @DirectManpower, @PerformedLoading, @PerformedCapacity, @CMPAbsent, @SubprocessCPU, @TTLDLWorkingHour
While @@FETCH_STATUS = 0
Begin	
	--Avg Working Hours
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'Avg Working Hours' and [Description] = 'Monthly CMP Report'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck
		set value = @AvgWorkingHours, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @AvgWorkingHours, @UserID, GETDATE(), null, null
	end

	--PPH
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'PPH' and [Description] = 'Monthly CMP Report'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck
		set value = @PPH, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @PPH, @UserID, GETDATE(), null, null
	end 

	--Efficiency
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'Efficiency' and [Description] = 'Monthly CMP Report'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck
		set value = @Efficiency, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @Efficiency, @UserID, GETDATE(), null, null
	end  

	--Direct Manpower
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'Direct Manpower' and [Description] = 'Monthly CMP Report'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck
		set value = @DirectManpower, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @DirectManpower, @UserID, GETDATE(), null, null
	end  


	--Performed
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'Performed' and [Description] = 'Monthly CMP Report'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck
		set value = @Performed, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @Performed, @UserID, GETDATE(), null, null
	end  

	--Working Days
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'Working Days' and [Description] = 'Monthly CMP Report'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck 
		set value = @WorkingDays, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @WorkingDays, @UserID, GETDATE(), null, null
	end

	--Performed / Loading
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'Performed / Loading' and [Description] = 'Planning'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck
		set value = @PerformedLoading, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @PerformedLoading, @UserID, GETDATE(), null, null
	end

	--Performed / Capacity
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'Performed / Capacity' and [Description] = 'Planning'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck
		set value = @PerformedCapacity, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @PerformedCapacity, @UserID, GETDATE(), null, null
	end

	--CMP Absent
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'CMP Absent' and [Description] = 'Absent'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck
		set value = @CMPAbsent, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @CMPAbsent, @UserID, GETDATE(), null, null
	end

	--Subprocess CPU
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'Subprocess CPU' and [Description] = 'Monthly CMP Report'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck
		set value = @SubprocessCPU, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @SubprocessCPU, @UserID, GETDATE(), null, null
	end

	--TTL DL Working Hour
	select @ItemID = ID from #tmp_Dropdownlist where [Name] = 'TTL DL Working Hour' and [Description] = 'Monthly CMP Report'
	if exists(select 1 from tradedb.trade.dbo.PulseCheck where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone)
	begin
		update tradedb.trade.dbo.PulseCheck
		set value = @TTLDLWorkingHour, EditName = @UserID, EditDate =GETDATE()
		where ItemID = @ItemID and [Year] = @Year and [Month] = @Month and Region = @_CountryID and Zone = @_Zone
	end
	else
	begin
		insert into tradedb.trade.dbo.PulseCheck
		([ItemID],[Year],[Month],[Region],[Zone],[Value],[AddName],[AddDate],[EditName],[EditDate])
		select @ItemID, @Year, @Month, @_CountryID, @_Zone, @TTLDLWorkingHour, @UserID, GETDATE(), null, null
	end
FETCH NEXT FROM CURSOR_ INTO @_CountryID, @_Zone, @Performed, @WorkingDays, @AvgWorkingHours, @PPH, @Efficiency, @DirectManpower, @PerformedLoading, @PerformedCapacity, @CMPAbsent, @SubprocessCPU, @TTLDLWorkingHour
End
CLOSE CURSOR_
DEALLOCATE CURSOR_ 

drop table #tmp,#tmp_Dropdownlist

drop table #tmp_ManHour,#tmp_TotalManpower,#tmp_excludeInOutTotal,#tmpQty,#tmpTtlManPower,#tmp_cpuFactor,#tmp_SubconCPU_sister,#tmp_WorkingDays,#tmp_PulseCheck,#tmp_CMPAbsent

drop table #tmp_PAMSAttendance,#tmp_PAMSHoliday,#tmp_SubconCPU_sister_onlyPerformed,#tmp_SubprocessCPU,#tmp_SubprocessCPUper

--select b.name, a.* 
--from tradedb.trade.dbo.PulseCheck a
--left join Tradedb.Trade.dbo.Dropdownlist b on a.ItemID = b.ID and type = 'PulseCheck'

--select * from Tradedb.Trade.dbo.Dropdownlist

End

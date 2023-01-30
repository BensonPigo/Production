CREATE PROCEDURE GetCapacityForTrade
	@OutputMonth varchar(6)-- = '202111' 
AS
BEGIN
CREATE Table #PMS_Data
(
	OutputMonth varchar(6),
	FtyZone varchar(8),
	TtlCPU numeric(11,3),
	TtlCPUExcludeNonSisterSubcon numeric(11,3),
	[TTL DL Working Hour(Monthly CMP)] numeric(10,2),
	CountryID varchar(8)
)
--only for TSR
Declare serverName_cursor cursor for
select serverName ='[PMS\pmsdb\PH1]' union all
select serverName ='[PMS\pmsdb\PH2]' union all
select serverName ='[PMS\pmsdb\ESP]' union all
select serverName ='[PMS\pmsdb\SNP]' union all
select serverName ='[PMS\pmsdb\SPT]' union all
select serverName ='[PMS\pmsdb\SPR]' union all
select serverName ='[PMS\pmsdb\SPS]' union all
select serverName ='[PMS\pmsdb\HZG]' union all
select serverName ='[PMS\pmsdb\HXG]' union all
select serverName ='[PMS\pmsdb\SWR]' union all
select serverName ='[PMS\pmsdb\PAN]'

Declare @ServerName varchar(30)
open serverName_cursor
fetch next from serverName_cursor into @ServerName
While @@FETCH_STATUS = 0
Begin	
--因GetOrderLocation_Rate效能所以用openquery，openquery後面字串不能用@參數組合，故整個openquery外層再包一層字串用exec執行
--很難Debug 調整時用 print @cmd
	Declare @cmd nvarchar(max)=N'
insert into #PMS_Data (OutputMonth, FtyZone , TtlCPU, TtlCPUExcludeNonSisterSubcon, [TTL DL Working Hour(Monthly CMP)],CountryID)
select *
from openquery('+@ServerName+N',
''
declare @MonthFirstDate date = ''''' + @OutputMonth + N''''' + ''''01''''
declare @MonthLastDate date = dateadd(day ,-1, dateadd(m, datediff(m,0,@MonthFirstDate)+1,0))  
;with tmpSewingDetail as(
	select
		OutputMonth = format(s.OutputDate, ''''yyyyMM''''),
		FtyZone = iif(f.FtyZone = ''''ESP'''', f.ID, f.FtyZone),
		CPU = sd.QAQty * IIF(s.Category = ''''M'''', isnull(mo.Cpu,0) * isnull(mo.CPUFactor,0), isnull(o.CPU,0) * isnull(o.CPUFactor,0) * Rate),
		ManHour = sd.WorkHour * s.Manpower,
		o.SubconInType,
		f.CountryID,
		LastShift = IIF(Shift <> ''''O'''' and s.Category <> ''''M'''' and LocalOrder = 1, ''''I'''',Shift)
	from Production.dbo.SewingOutput s WITH (NOLOCK) 
	inner join Production.dbo.SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
	left join Production.dbo.Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
	left join Production.dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
	left join Production.dbo.factory f WITH (NOLOCK) on f.id=s.FactoryID
	outer apply(select Rate = isnull(Production.dbo.GetOrderLocation_Rate(o.id ,sd.ComboType),100)/100) Rate
	where s.OutputDate between @MonthFirstDate and @MonthLastDate
	and (o.CateGory NOT IN (''''G'''',''''A'''') or s.Category=''''M'''') 
), finalGroup as(
	select
		OutputMonth,
		FtyZone,
		TtlCPU = ROUND(sum(CPU), 3),
		TtlCPUExcludeNonSisterSubcon = ROUND(sum(iif(LastShift <> ''''I'''' or (LastShift = ''''I'''' and SubconInType in (''''1'''',''''2'''')), CPU, 0)), 3),
		[TTL DL Working Hour(Monthly CMP)] = ROUND(Sum(ManHour), 2),
		CountryID
	from tmpSewingDetail
	where LastShift <> ''''O''''
	group by OutputMonth, FtyZone, CountryID
)
select
	OutputMonth, FtyZone, TtlCPU, TtlCPUExcludeNonSisterSubcon, [TTL DL Working Hour(Monthly CMP)],CountryID
from finalGroup
''
)
'
--print @cmd
	exec (@cmd)
fetch next from serverName_cursor into @ServerName
end
CLOSE serverName_cursor
DEALLOCATE serverName_cursor



Create Table #PAMSAttendance([Direct Manpower (Monthly CMP)] int)
Create Table #PAMSAttendance_Key(countryID varchar(8), FtyZone varchar(8), [Direct Manpower (Monthly CMP)] int)
declare @CountryID varchar(8), @FtyZone varchar(8)
Declare PMS_Data_cursor cursor for select CountryID,FtyZone from #PMS_Data
open PMS_Data_cursor
fetch next from PMS_Data_cursor into @CountryID,@FtyZone
While @@FETCH_STATUS = 0
Begin	
	insert into #PAMSAttendance
	exec tradedb.trade.dbo.GetPAMSAttendance @CountryID, @FtyZone ,@OutputMonth, 'Capacity'

	insert into #PAMSAttendance_Key
	select @CountryID, @FtyZone, [Direct Manpower (Monthly CMP)] from #PAMSAttendance

	delete #PAMSAttendance

fetch next from PMS_Data_cursor into @CountryID,@FtyZone
end
CLOSE PMS_Data_cursor
DEALLOCATE PMS_Data_cursor

select a.OutputMonth, FactoryID = a.FtyZone, a.TtlCPU, a.TtlCPUExcludeNonSisterSubcon, a.[TTL DL Working Hour(Monthly CMP)], b.[Direct Manpower (Monthly CMP)]
from #PMS_Data a
left join #PAMSAttendance_Key b on b.countryID = a.CountryID and b.FtyZone = a.FtyZone

drop table #PMS_Data, #PAMSAttendance, #PAMSAttendance_Key

END
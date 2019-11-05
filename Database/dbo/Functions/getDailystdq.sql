-- =============================================
-- Author:		JIMMY
-- Create date: 2017/05/06
-- Description:	<SP#>
-- =============================================
Create FUNCTION [dbo].[getDailystdq]
(
	@tSP varchar(20)
)
RETURNS
@table TABLE 
(
	Date date,
	StdQ int,
	SewingScheduleID bigint ,
	ComboType Varchar(1)
)
AS
Begin

declare @APSListWorkDay Table(
	[id] [bigint] NOT NULL,
	[APSNo] [int] NULL,
	[MDivisionID] [varchar](8) NULL,
	[SewingLineID] [varchar](2) NULL,
	[FactoryID] [varchar](8) NULL,
	[InlineDate] [date] NULL,
	[OfflineDate] [date] NULL,
	[Inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[InlineHour] [numeric](17, 6) NULL,
	[OfflineHour] [numeric](17, 6) NULL,
	[OrderID] [varchar](13) NOT NULL,
	[ComboType] [varchar](1) NULL,
	[AlloQty] [int] NULL,
	[StandardOutput] [int] NULL
)
Insert Into @APSListWorkDay
select
	s.id,
	s.APSNo ,
	s.MDivisionID,
	s.SewingLineID,
	s.FactoryID,
	[InlineDate] = Cast(s.Inline as date),
	[OfflineDate] = Cast(s.Offline as date),
	s.Inline,
	s.Offline,
    [InlineHour] = DATEDIFF(ss,Cast(s.Inline as date),s.Inline) / 3600.0	  ,
    [OfflineHour] = DATEDIFF(ss,Cast(s.Offline as date),s.Offline) / 3600.0	  ,
	s.OrderID,
    s.ComboType,
	s.AlloQty,
	s.StandardOutput
from SewingSchedule s  WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join Factory f with (nolock) on f.id = s.FactoryID and Type <> 'S'
left join Country c WITH (NOLOCK) on o.Dest = c.ID
outer apply(select [val] = iif(s.OriEff is null and s.SewLineEff is null,s.MaxEff, isnull(s.OriEff,100) * isnull(s.SewLineEff,100) / 100) ) ScheduleEff
where 1 = 1 and s.OrderID = @tSP and s.APSno <> 0

declare @WorkDate TABLE(
	[FactoryID] [varchar](8) NULL,
	[WorkDate] [datetime] NULL
)

Insert Into @WorkDate
--組出所有計畫最大Inline,最小Offline之間所有的日期，後面展開個計畫每日資料使用
SELECT f.FactoryID,cast(DATEADD(DAY,number,(select CAST(min(Inline)AS date) from @APSListWorkDay)) as datetime) [WorkDate]
FROM master..spt_values s
cross join (select distinct FactoryID from @APSListWorkDay) f
WHERE s.type = 'P'
AND DATEADD(DAY,number,(select CAST(min(Inline)AS date) from @APSListWorkDay)) <= (select cast(max(Offline)as Date) from @APSListWorkDay)

declare @Workhour_step1 TABLE(
	[APSNo] [int] NULL,
	[SewingLineID] [varchar](2) NULL,
	[FactoryID] [varchar](8) NULL,
	[WorkDate] [datetime] NULL,
	[inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[inlineDate] [date] NULL,
	[OfflineDate] [date] NULL,
	[StartHour] [float] NULL,
	[EndHour] [float] NULL,
	[InlineHour] [numeric](17, 6) NULL,
	[OfflineHour] [numeric](17, 6) NULL,
	[OrderID] [varchar](13) NOT NULL,
	[ComboType] [varchar](1) NULL
)
--展開計畫日期資料
Insert Into @Workhour_step1
select  al.APSNo,
        wkd.SewingLineID,
        wkd.FactoryID,
        [WorkDate] = cast( wkd.Date as datetime),
		al.inline,
		al.Offline,
		al.inlineDate,
		al.OfflineDate,
        [StartHour] = cast(wkd.StartHour as float),
        [EndHour] = cast(wkd.EndHour as float),
        al.InlineHour,
        al.OfflineHour,
		al.OrderID,
        al.ComboType
from @APSListWorkDay al
inner join @WorkDate wd on wd.WorkDate >= al.InlineDate and wd.WorkDate <= al.OfflineDate and wd.FactoryID = al.FactoryID
inner join Workhour_Detail wkd with (nolock) on wkd.FactoryID = al.FactoryID and 
                                                wkd.SewingLineID = al.SewingLineID and 
                                                wkd.Date = wd.WorkDate

--刪除每個計畫inline,offline當天超過時間的班表                                                
delete @Workhour_step1 where WorkDate = InlineDate and EndHour <= InlineHour
delete @Workhour_step1 where WorkDate = OfflineDate and StartHour >= OfflineHour
declare @Workhour_step2 TABLE(
	[APSNo] [int] NULL,
	[SewingLineID] [varchar](2) NULL,
	[WorkDate] [datetime] NULL,
	[inline] [datetime] NULL,
	[Offline] [datetime] NULL,
	[inlineDate] [date] NULL,
	[OfflineDate] [date] NULL,
	[StartHour] [float] NULL,
	[EndHour] [float] NULL,
	[InlineHour] [numeric](17, 6) NULL,
	[OfflineHour] [numeric](17, 6) NULL,
	[StartHourSort] [bigint] NULL,
	[EndHourSort] [bigint] NULL,
	[ComboType] [varchar](1) NULL
)
--排出每天班表順序
Insert Into @Workhour_step2
select  APSNo,
        SewingLineID,
        WorkDate,
		inline,
		Offline,
		inlineDate,
		OfflineDate,
        StartHour,
        EndHour,
        InlineHour,
        OfflineHour,
		[StartHourSort] = ROW_NUMBER() OVER (PARTITION BY APSNo,WorkDate,OrderID,ComboType ORDER BY StartHour),
		[EndHourSort] = ROW_NUMBER() OVER (PARTITION BY APSNo,WorkDate,OrderID,ComboType ORDER BY EndHour desc),
        ComboType
from @Workhour_step1	

--依照班表順序，將inline,offline當天StartHour與EndHour update與inline,offline相同
update @Workhour_step2 set StartHour = InlineHour where WorkDate = InlineDate and StartHourSort = 1 and InlineHour > StartHour
update @Workhour_step2 set EndHour = OfflineHour where WorkDate = OfflineDate and EndHourSort = 1 and OfflineHour < EndHour


declare @APSExtendWorkDate_step1 Table(
	[APSNo] [int] NULL,
	[ComboType] [varchar](1) NULL,
	[WorkDate] [datetime] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL
) 
Insert Into @APSExtendWorkDate_step1
select 
	APSNo,ComboType,WorkDate  ,
	[SewingStart] = DATEADD(mi, min(StartHour) * 60,   WorkDate),
	[SewingEnd] = DATEADD(mi, max(EndHour) * 60,   WorkDate)
from @Workhour_step2 
group by APSNo,WorkDate,ComboType

Insert into @table
select Date = cast(WorkDate as Date)
	, stdQty = iif(s.AlloQty>sum(s1.std1) over (partition by s.id) and b.WorkDate = max(b.WorkDate) over (partition by s.id),s.AlloQty,s1.std1)
	, s.ID
	, s.ComboType
from @APSListWorkDay s
inner join @APSExtendWorkDate_step1 b on s.APSNo = b.APSNo  and s.ComboType = b.ComboType
outer apply(select perDayQty = CEILING(cast(s.StandardOutput as decimal)*(cast(DATEDIFF(mi ,b.[SewingStart], b.[SewingEnd])as decimal)/60)))x
outer apply(select std1 = iif(s.AlloQty<x.perDayQty,s.AlloQty,x.perDayQty))s1

Return;

End
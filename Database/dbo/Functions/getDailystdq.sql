-- =============================================
-- Author:		JIMMY
-- Create date: 2017/05/06
-- Description:	<SP#>
-- =============================================
CREATE FUNCTION [dbo].[getDailystdq]
(
	@tSP varchar(20)
)
RETURNS
@table TABLE 
(
	APSNo int,
	Date date,
	StdQ int
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
	[StandardOutput] [int] NULL,
	[LearnCurveID] [int] NULL,
	[HourOutput] decimal(30,10),
	[OriWorkHour] decimal(30,10),
	[LNCSERIALNumber] int,
	[Sewer] int
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
	s.StandardOutput,
	s.LearnCurveID,
	[HourOutput] = iif(isnull(s.TotalSewingTime,0)=0,0,(s.Sewer * 3600.0 * ScheduleEff.val / 100) / s.TotalSewingTime),
	[OriWorkHour] = iif (s.Sewer = 0 or isnull(s.TotalSewingTime,0)=0, 0, sum(s.AlloQty) / ((s.Sewer * 3600.0 * ScheduleEff.val / 100) / s.TotalSewingTime)),
	s.LNCSERIALNumber,
	s.Sewer
from SewingSchedule s  WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join Factory f with (nolock) on f.id = s.FactoryID and Type <> 'S'
left join Country c WITH (NOLOCK) on o.Dest = c.ID
outer apply(select [val] = iif(s.OriEff is null and s.SewLineEff is null,s.MaxEff, isnull(s.OriEff,100) * isnull(s.SewLineEff,100) / 100) ) ScheduleEff
where 1 = 1 and s.OrderID = @tSP and s.APSno <> 0
group by 
	s.id,
	s.APSNo ,
	s.MDivisionID,
	s.SewingLineID,
	s.FactoryID,
	s.Inline,
	s.Offline,
	s.OrderID,
    s.ComboType,
	s.AlloQty,
	s.StandardOutput,
	s.LearnCurveID,
	s.TotalSewingTime,
	s.LNCSERIALNumber,
	s.Sewer,
	ScheduleEff.val

declare @WorkDate TABLE(
	[FactoryID] [varchar](8) NULL,
	[WorkDate] [datetime] NULL
)

Insert Into @WorkDate
--�եX�Ҧ��p�e�̤jInline,�̤pOffline�����Ҧ�������A�᭱�i�}�ӭp�e�C���ƨϥ�
SELECT f.FactoryID,cast(DATEADD(DAY,number,(select CAST(min(Inline)AS date) from @APSListWorkDay)) as datetime) [WorkDate]
FROM master..spt_values s
cross join (select distinct FactoryID from @APSListWorkDay) f
WHERE s.type = 'P'
AND DATEADD(DAY,number,(select CAST(min(Inline)AS date) from @APSListWorkDay)) <= (select cast(max(Offline)as Date) from @APSListWorkDay)

declare @Workhour_step1 TABLE(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
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
	[ComboType] [varchar](1) NULL,
	[HourOutput] decimal(30,10),
	[OriWorkHour] decimal(30,10),
	[LNCSERIALNumber] int,
	[Sewer] int
)
--�i�}�p�e������
Insert Into @Workhour_step1
select  al.APSNo,
        al.LearnCurveID,
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
        al.ComboType,
		[HourOutput],
		[OriWorkHour],
		LNCSERIALNumber,
		Sewer
from @APSListWorkDay al
inner join @WorkDate wd on wd.WorkDate >= al.InlineDate and wd.WorkDate <= al.OfflineDate and wd.FactoryID = al.FactoryID
inner join Workhour_Detail wkd with (nolock) on wkd.FactoryID = al.FactoryID and 
                                                wkd.SewingLineID = al.SewingLineID and 
                                                wkd.Date = wd.WorkDate

--�R���C�ӭp�einline,offline��ѶW�L�ɶ����Z��                                                
delete @Workhour_step1 where WorkDate = InlineDate and EndHour <= InlineHour
delete @Workhour_step1 where WorkDate = OfflineDate and StartHour >= OfflineHour
declare @Workhour_step2 TABLE(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
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
	[ComboType] [varchar](1) NULL,
	[HourOutput] decimal(30,10),
	[OriWorkHour] decimal(30,10),
	[LNCSERIALNumber] int,
	[Sewer] int
)
Insert Into @Workhour_step2
select  APSNo,
        LearnCurveID,
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
        ComboType,
		[HourOutput],
		[OriWorkHour],
		LNCSERIALNumber,
		Sewer
from @Workhour_step1	

update @Workhour_step2 set StartHour = InlineHour where WorkDate = InlineDate and StartHourSort = 1 and InlineHour > StartHour
update @Workhour_step2 set EndHour = OfflineHour where WorkDate = OfflineDate and EndHourSort = 1 and OfflineHour < EndHour


declare @APSExtendWorkDate_step1 Table(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[WorkDate] [datetime] NULL,
	[ComboType] [varchar](1) NULL,
	[WorkingTime] decimal(30,10),
	[OriWorkDateSer] int,
	[HourOutput] decimal(30,10),
	[OriWorkHour] decimal(30,10),
	[Sewer] int,
	[LNCSERIALNumber] int
) 
Insert Into @APSExtendWorkDate_step1
select 
	APSNo,	LearnCurveID,
	[SewingStart] = DATEADD(mi, min(StartHour) * 60,   WorkDate),
	[SewingEnd] = DATEADD(mi, max(EndHour) * 60,   WorkDate),
	WorkDate,
	ComboType,
	[WorkingTime] = sum(EndHour - StartHour),
	[OriWorkDateSer] = ROW_NUMBER() OVER (PARTITION BY APSNo,ComboType ORDER BY WorkDate),
	HourOutput,	OriWorkHour,Sewer,LNCSERIALNumber
from @Workhour_step2 
group by APSNo,LearnCurveID,WorkDate,HourOutput,OriWorkHour,Sewer,LNCSERIALNumber,ComboType


declare @APSExtendWorkDate Table(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[WorkDate] [datetime] NULL,
	[WorkingTime] decimal(30,10),
	[WorkDateSer] int,
	[HourOutput] decimal(30,10),
	[OriWorkHour] decimal(30,10),
	[Sewer] int
) 
insert into @APSExtendWorkDate
select
	APSNo,LearnCurveID,SewingStart,SewingEnd,WorkDate,WorkingTime,
	WorkDateSer = case	when isnull(LNCSERIALNumber,0) = 0 then OriWorkDateSer
						when LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) <= 0 then OriWorkDateSer
						else OriWorkDateSer + LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) end,
	HourOutput,	OriWorkHour,Sewer
from @APSExtendWorkDate_step1

--取得每個計劃去除LearnCurve後的總工時
declare @OriTotalWorkHour Table(
	[APSNo] [int] NULL,
	[WorkDate] [datetime] NULL,
	[TotalWorkHour] decimal(30,10)
) 
insert into @OriTotalWorkHour
SELECT awd.APSNo,awd.WorkDate,[TotalWorkHour] = sum(OriWorkHour)
FROM @APSExtendWorkDate awd
group by awd.APSNo,awd.WorkDate

insert into @table
select 
	awd.APSNo
	, Date = cast(awd.SewingStart as date)
    , StdQty = round(sum(iif (isnull (otw.TotalWorkHour, 0) = 0, 0, awd.WorkingTime * awd.HourOutput * awd.OriWorkHour / otw.TotalWorkHour))
		* ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0,0)
from @APSExtendWorkDate awd
inner join @OriTotalWorkHour otw on otw.APSNo = awd.APSNo and otw.WorkDate = awd.WorkDate
left join LearnCurve_Detail lcd with (nolock) on awd.LearnCurveID = lcd.ID and awd.WorkDateSer = lcd.Day
outer apply(select top 1 [val] = Efficiency from LearnCurve_Detail where ID = awd.LearnCurveID order by Day desc ) LastEff
group by awd.APSNo,awd.SewingStart,awd.SewingEnd,awd.WorkingTime,ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0)),awd.Sewer

Return;

End
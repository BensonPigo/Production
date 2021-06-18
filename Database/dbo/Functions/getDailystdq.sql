

-- =============================================
-- Author:		JIMMY
-- Create date: 2017/05/06
-- Description:	<SP#>
-- =============================================
CREATE FUNCTION [dbo].[getDailystdq]
(
	@APSNo varchar(20)
)
RETURNS
@table TABLE 
(
	APSNo int,
	Date date,
	StdQ int,
	StdQPrint int
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
	[Sewer] int,
	[SwitchTime] [int] NULL
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
	s.Sewer,
	s.SwitchTime
from SewingSchedule s  WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join Factory f with (nolock) on f.id = s.FactoryID and Type <> 'S'
left join Country c WITH (NOLOCK) on o.Dest = c.ID
outer apply(select [val] = iif(s.OriEff is null and s.SewLineEff is null,s.MaxEff, isnull(s.OriEff,100) * isnull(s.SewLineEff,100) / 100) ) ScheduleEff
where 1 = 1 
--and s.OrderID = @tSP 
and s.APSNo = @APSNo 
and s.APSno <> 0
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
	ScheduleEff.val,
	s.SwitchTime

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
	[Sewer] int,
	[SwitchTime] [int] NULL
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
		Sewer,
		al.SwitchTime
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
	[Sewer] int,
	[SwitchTime] [int] NULL,
	[OrderID] [varchar](13) NOT NULL
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
		Sewer,		
		SwitchTime,
		OrderID
from @Workhour_step1	

update @Workhour_step2 set StartHour = InlineHour where WorkDate = InlineDate and StartHourSort = 1 and InlineHour > StartHour
update @Workhour_step2 set EndHour = OfflineHour where WorkDate = OfflineDate and EndHourSort = 1 and OfflineHour < EndHour


declare @APSExtendWorkDate_step1 Table(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [float] NULL,
	[WorkingTime] [float] NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,	
	[Sewer] [int] NULL,
	[LNCSERIALNumber] [int] NULL
) 
Insert Into @APSExtendWorkDate_step1
select 
	APSNo,	
	LearnCurveID,
	[SewingStart] = DATEADD(mi, min(StartHour) * 60,   WorkDate),
	[SewingEnd] = DATEADD(mi, max(EndHour) * 60,   WorkDate),
	SwitchTime,
	WorkDate,
	[Work_Minute] = sum(EndHour - StartHour) * 60,--round(sum(EndHour - StartHour) * 60,4),
	[WorkingTime] = sum(EndHour - StartHour),--ROUND(sum(EndHour - StartHour),4),
	[OriWorkDateSer] = ROW_NUMBER() OVER (PARTITION BY APSNo,orderID,ComboType ORDER BY WorkDate),
	HourOutput,	
	OriWorkHour,
	Sewer,
	LNCSERIALNumber
from @Workhour_step2 
group by APSNo,LearnCurveID,WorkDate,HourOutput,OriWorkHour,Sewer,LNCSERIALNumber,ComboType,SwitchTime,OrderID


/* 
相同APSNo第一筆SewingStart Time 加上Switch Time
如果超過SewingEnd Time, 剩餘的時間 則往下一筆"相同"APSNo的SewingStart Time加上 , 以此類推
******
Work hour/Day 扣除Switch Time, 如過不夠扣除則將剩餘的分鐘數
往同APSNo 第二筆扣除, 以此類推
若Work Hour/Day 不夠扣除,則該筆APSNo不顯示
*/
-- 取得相同APSNO 加總的WorkTime by Minute

Declare @APSExtendWorkDate_step2 table(
	[Sum_Work_Minute] [float] NULL,
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [float] NULL,
	[WorkingTime] [float] NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[Sewer] [int] NULL,
	[LNCSERIALNumber] [int] NULL	
)
insert into @APSExtendWorkDate_step2
select [Sum_Work_Minute] = sum(Work_Minute) over(partition by APSNo order by SewingStart)
,* 
from @APSExtendWorkDate_step1
order by APSNo,SewingStart

-- 取得遞減的SwitchTime
Declare @APSExtendWorkDate_step3 table(
	[New_SwitchTime] [float] NULL,
	[Sum_Work_Minute] [float] NULL,
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [float] NULL,
	[WorkingTime] [float] NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[Sewer] [int] NULL,
	[LNCSERIALNumber] [int] NULL
)
insert into @APSExtendWorkDate_step3
select 
[New_SwitchTime] = case when (SwitchTime - LAG(Sum_Work_Minute,1,0) over (partition by APSNo order by sewingstart) <= 0) then 0
else SwitchTime - LAG(Sum_Work_Minute,1,0) over (partition by APSNo order by sewingstart)
end
,*
from @APSExtendWorkDate_step2

--取得遞減的Work Minute 
Declare @APSExtendWorkDate_step4 table(
	[New_Work_Minute] [float] NULL,
	[New_SwitchTime]  [float] NULL,
	[Sum_Work_Minute] [float] NULL,
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[SwitchTime] [int] NULL,
	[WorkDate] [datetime] NULL,
	[Work_Minute] [float] NULL,
	[WorkingTime] [float] NULL,
	[OriWorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[Sewer] [int] NULL,
	[LNCSERIALNumber] [int] NULL
)
insert into @APSExtendWorkDate_step4
select 
[New_Work_Minute] = 
case when Work_Minute = Sum_Work_Minute and Work_Minute - SwitchTime > 0 then Work_Minute - SwitchTime
     when Work_Minute != Sum_Work_Minute and Sum_Work_Minute - Work_Minute > SwitchTime then Work_Minute
	 when LAG(New_SwitchTime,1,0) OVER (PARTITION BY APSNo ORDER BY SewingStart) !=0 
		and Work_Minute - LAG(New_SwitchTime,1,0) OVER (PARTITION BY APSNo ORDER BY SewingStart) > 0 
		then Work_Minute - LAG(New_SwitchTime,1,0) OVER (PARTITION BY APSNo ORDER BY SewingStart)
	 else 0
	 end
,* 
from @APSExtendWorkDate_step3

--欄位 LNCSERIALNumber 
--    第一天學習曲線計算方式 = 最後一天對應的工作天數 『減去』（該計畫總生產天數 『減去』一天因為要推出第一天）
--    ※ 但請注意以下特出例外狀況
--       1. LNCSERIALNumber 為空值或零
--       2. 計算後的結果為零或負數
--       遇到以上特殊例外狀況，
--       生產的第一天都對應學習曲線的第一天。
Declare @APSExtendWorkDate table(
	[APSNo] [int] NULL,
	[LearnCurveID] [int] NULL,
	[SewingStart] [datetime] NULL,
	[SewingEnd] [datetime] NULL,
	[WorkDate] [datetime] NULL,
	[New_WorkingTime] [float] NULL,
	[New_SwitchTime] [float] NULL,
	[WorkingTime] [float] NULL,
	[OriWorkDateSer] [bigint] NULL,
	[WorkDateSer] [bigint] NULL,
	[HourOutput] [numeric](38, 15) NULL,
	[OriWorkHour] [numeric](38, 13) NULL,
	[Sewer] [int] NULL,
	[LNCSERIALNumber] [int] NULL
)
insert into @APSExtendWorkDate
select	APSNo,
		LearnCurveID,
		SewingStart,
		SewingEnd,
		WorkDate,
		[New_WorkingTime] = IIF(SwitchTime = 0, WorkingTime,  CONVERT(float, New_Work_Minute)/60) ,
		[New_SwitchTime]  = IIF(SwitchTime = 0, 0 , CONVERT(float, New_SwitchTime)/60) ,
		WorkingTime,
		OriWorkDateSer,
		WorkDateSer = case	when isnull(LNCSERIALNumber,0) = 0 then OriWorkDateSer
						when LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) <= 0 then OriWorkDateSer
						else OriWorkDateSer + LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) end,
		HourOutput,	
		OriWorkHour,
		Sewer,
		LNCSERIALNumber
from @APSExtendWorkDate_step4

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

declare @stdq TABLE 
(
	APSNo int,
	Date date,
	StdQ int
)
insert into @stdq
select 
	awd.APSNo
	, Date = cast(awd.SewingStart as date)
    , StdQty = round(sum(iif (isnull (otw.TotalWorkHour, 0) = 0, 0, awd.New_WorkingTime * awd.HourOutput * awd.OriWorkHour / otw.TotalWorkHour))
		* ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0,0)
from @APSExtendWorkDate awd
inner join @OriTotalWorkHour otw on otw.APSNo = awd.APSNo and otw.WorkDate = awd.WorkDate
left join LearnCurve_Detail lcd with (nolock) on awd.LearnCurveID = lcd.ID and awd.WorkDateSer = lcd.Day
outer apply(select top 1 [val] = Efficiency from LearnCurve_Detail where ID = awd.LearnCurveID order by Day desc ) LastEff
group by awd.APSNo,awd.SewingStart,awd.SewingEnd,awd.WorkingTime,ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0)),awd.Sewer

--以下計算Printing的rate
declare @APSBase Table(
	[OrderID] [varchar](13) NOT NULL,
	[ComboType] [varchar](1) NULL,
	[Inline] [datetime] NULL,
	[AlloQty] [int] NULL
)

Insert Into @APSBase
select
	s.OrderID,
	s.ComboType,
	s.Inline,
	s.AlloQty
from SewingSchedule s  WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join Factory f with (nolock) on f.id = s.FactoryID and Type <> 'S'
where 1 = 1 
and s.APSNo = @APSNo
and s.APSno <> 0

-- 找出 < Suncon > 不為空的 OrderID -- < Suncon > not empty rule (ISP20210759)
declare @SunconNotEmpty TABLE([OrderID] [varchar](13) NOT NULL)
insert into @SunconNotEmpty
select distinct ot.ID
from Order_TmsCost ot WITH (NOLOCK) 
inner join ArtworkType at WITH (NOLOCK) on at.ID = ot.ArtworkTypeID and at.ID in('PRINTING','PRINTING PPU')
left join LocalSupp ls on ls.id = ot.LocalSuppID
where exists(select 1 from @APSBase where orderid = ot.id)
and ot.ArtworkTypeID = 'PRINTING'
and IIF(ot.InhouseOSP = 'O', ls.abb, ot.LocalSuppID) <> ''

-- 相同SP有多個ComboType時,只取Inline最早的ComboType -- same SP has more one ComboType,get first Inline data
declare @SP_ComboType Table(
	[OrderID] [varchar](13) NOT NULL,
	[ComboType] [varchar](1) NULL
)
insert into @SP_ComboType
select OrderID,ComboType
from(
	select rn =  row_number() over(partition by a.OrderID order by a.Inline),a.OrderID,a.ComboType
	from @APSBase a
	where exists(select 1 from @SunconNotEmpty where OrderID = a.OrderID)
)x
where rn = 1

declare @Printing_AlloQty int
declare @TTL_AlloQty int

select @Printing_AlloQty = sum(AlloQty)
from SewingSchedule s
inner join @SP_ComboType c on c.OrderID = s.OrderID and c.ComboType = s.ComboType
where APSNo = @APSNo

select @TTL_AlloQty = sum(AlloQty) from SewingSchedule where APSNo = @APSNo

declare @rate float
if @TTL_AlloQty = 0
	select @rate = 0.0
ELSE
	SELECT @rate = CAST(@Printing_AlloQty AS FLOAT)  / CAST(@TTL_AlloQty AS FLOAT) 

-- 此APSno每日標準數的總數 * Print的比例 
-- Print總數, 最後StdQty Print總數要等於此數
declare @ttlPrintingstd int = round((select sum(StdQ) * @rate from @stdq), 0)

declare @StdQprintRate TABLE 
(
	APSNo int,
	Date date,
	StdQ int,
	StdQtyforprinting int
)
insert into @StdQprintRate
select s.APSNo,s.Date,s.StdQ,
	StdQtyforprinting = ROUND(s.StdQ * @rate, 0)
from @stdq s

declare @diffPrintQty int = (select @ttlPrintingstd - (select sum(StdQtyforprinting) from @StdQprintRate))

update @StdQprintRate
set StdQtyforprinting = StdQtyforprinting + @diffPrintQty
where Date = (select max(date) from @StdQprintRate)

insert into @table
select APSNo,Date,StdQ,StdQtyforprinting
from @StdQprintRate

Return;

End
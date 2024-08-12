

CREATE PROCEDURE [dbo].[usp_GenSewingOutputDataForFMS]
	  @StartOutputDate    date
	 ,@EndOutputDate      date
     ,@MDivisionID	VarChar(8)	=''
	 ,@FactoryID		varchar(80) =''
AS
begin

--撈資料規則
--1. SewingOutput
select s.ID,s.OutputDate,s.SewingLineID,s.Team,s.FactoryID,s.Shift,s.Manpower,s.WorkHour,s.ManHour,s.TMS,s.QAQty,s.Efficiency,s.SewingReasonIDForTypeIC
,sr.Description as 'Inlne Category',s.Status,s.AddName,s.AddDate,s.EditName,s.EditDate,s.Category
into #tmpMaster
from SewingOutput s
left join SewingReason sr on s.SewingReasonIDForTypeIC = sr.ID and sr.Type = 'IC'
where s.OutputDate between @StartOutputDate and @EndOutputDate
and s.MDivisionID = iif(isnull(@MDivisionID,'') !='',@MDivisionID,s.MDivisionID)
and s.FactoryID =  iif(isnull(@FactoryID,'')!='',@FactoryID,s.FactoryID)


select s.ID,s.OutputDate,s.SewingLineID,s.Team,s.FactoryID,s.Shift,s.Manpower,s.WorkHour,s.ManHour,s.TMS,s.QAQty,s.Efficiency,s.SewingReasonIDForTypeIC
,sr.Description as 'Inlne Category',s.Status,s.AddName,s.AddDate,s.EditName,s.EditDate
	,s.Category
    ,[ActManPower] = s.Manpower
	,[MockupStyle] = isnull(mo.StyleID,'')
	,[OrderStyle] = isnull(o.StyleID,'') 
	,sd.UKey
into #tmp
from #tmpMaster s
inner join SewingOutput_Detail sd on s.ID = sd.ID
left join SewingReason sr on s.SewingReasonIDForTypeIC = sr.ID and sr.Type = 'IC'
left join Orders o on o.ID = sd.OrderId
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId

-- 準備中間資料
-- 取得Max & Min OutputDate
select 
  [MaxOutputDate] = Max(OutputDate)
, [MinOutputDate] = MIN(OutputDate)
, MockupStyle, OrderStyle, SewingLineID, FactoryID 
into #tmpOutputDate
from(
	select distinct OutputDate, MockupStyle, OrderStyle, SewingLineID, FactoryID 
	from #tmp
) a
group by MockupStyle, OrderStyle, SewingLineID, FactoryID

--get outputdate Min-240 day ~ Max OutPutDate
select distinct t.FactoryID, t.SewingLineID ,t.OrderStyle, t.MockupStyle, s.OutputDate
into #tmpSewingOutput
from #tmpOutputDate t
inner join SewingOutput s WITH (NOLOCK) on s.SewingLineID = t.SewingLineID and s.FactoryID = t.FactoryID and s.OutputDate 
between dateadd(day,-240, t.MinOutputDate) and t.MaxOutputDate
where   exists(	
	select 1 from SewingOutput_Detail sd WITH (NOLOCK)
	left join Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
	left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
	where s.ID = sd.ID and (o.StyleID = t.OrderStyle or mo.StyleID = t.MockupStyle)
)
order by  FactoryID, t.SewingLineID ,t.OrderStyle, t.MockupStyle, s.OutputDate

-- get WorkHour
select w.FactoryID, w.SewingLineID ,t.OrderStyle, t.MockupStyle, w.Date
into #tmpWorkHour
from WorkHour w WITH (NOLOCK)
left join #tmpOutputDate t on t.SewingLineID = w.SewingLineID and t.FactoryID = w.FactoryID and w.Date between t.MinOutputDate and t.MaxOutputDate
where w.Holiday=0 and isnull(w.Hours,0) != 0 
and w.Date >= (select dateadd(day,-240, min(MinOutputDate)) from #tmpOutputDate) 
and w.Date <= (select max(MaxOutputDate) from #tmpOutputDate)
order by  FactoryID, t.SewingLineID ,t.OrderStyle, t.MockupStyle, w.Date


select ID,OutputDate,SewingLineID,Team,FactoryID,Shift,Manpower,WorkHour,ManHour,TMS,QAQty,Efficiency,SewingReasonIDForTypeIC
,[Inlne Category],Status,AddName,AddDate,EditName,EditDate
from #tmpMaster

--2. SewingOutput_Detail
select distinct sd.ID, sd.OrderId, sd.Article, sd.Color
,[QA Output] = (
	select t.TEMP+',' 
	from (
		select sdd.SizeCode+'*'+CONVERT(varchar,sdd.QAQty) AS TEMP 
		from SewingOutput_Detail_Detail SDD WITH (NOLOCK) 
		where SDD.SewingOutput_DetailUKey = sd.UKey
	) t for xml path(''))
, sd.QAQty, sd.InlineQty,sd.UKey,o.StyleID,sd.ComboType,o.SeasonID
,[New Style/Repeat Style] = dbo.IsRepeatStyleBySewingOutput(t.FactoryID, t.OutputDate, t.SewinglineID, t.Team, o.StyleUkey)
,[WorkHour] = sd.WorkHour
,[CPU/piece] = IIF(t.Category='M'
	,isnull(mo.Cpu,0) * isnull(mo.CPUFactor,0)
	,isnull(o.CPU,0) * isnull(o.CPUFactor,0) * 
		isnull([dbo].[GetOrderLocation_Rate](o.id,sd.ComboType),100)/100
)
,[CPU Sewer/HR] = IIF(
	ROUND(t.ActManPower * sd.WorkHour,2) > 0
	,(IIF(t.Category = 'M', isnull(mo.Cpu,0) * isnull(mo.CPUFactor,0)
	,isnull(o.CPU,0) * isnull(o.CPUFactor,0) * 
		isnull([dbo].[GetOrderLocation_Rate](o.id,sd.ComboType),100)/100) * sd.QAQty)/ROUND(t.ActManPower * sd.WorkHour,2),0)
,[Cumulate Date] = IIF(CumulateDate.val > 180,'>180',CONVERT(VARCHAR,CumulateDate.val))
,[RFT] = IIF(isnull(RFT.InspectQty,0) = 0, 0, CAST(round(((RFT.InspectQty - RFT.RejectQty) / RFT.InspectQty)*100,2) as decimal(6,2)))
from SewingOutput_Detail sd
inner join #tmp t on sd.UKey = t.UKey
left join Orders o on o.ID = sd.OrderId
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
left join RFT with(nolock) on rft.SewinglineID = t.SewingLineID and t.FactoryID = rft.FactoryID and rft.OrderID = sd.OrderId and rft.Team = t.Team and RFT.Shift = t.Shift
and rft.CDate = t.OutputDate
outer apply (	
	select val = IIF(Count(1)=0, 1, Count(1))
	from #tmpSewingOutput s
	where	s.FactoryID = t.FactoryID and
			s.MockupStyle = t.MockupStyle and
			s.OrderStyle = t.OrderStyle and
			s.SewingLineID = t.SewingLineID and
			s.OutputDate <= t.OutputDate and
			s.OutputDate >
			(
				select case when max(iif(s1.OutputDate is null, w.Date, null)) is not null then max(iif(s1.OutputDate is null, w.Date, null))
							--區間內都連續生產，第一天也要算是生產日，所以要減一天
							when min(w.Date) is not null then DATEADD(day, -1, min(w.Date))
							else t.OutputDate end
				from #tmpWorkHour w 
				left join #tmpSewingOutput s1 on s1.OutputDate = w.Date and
													s1.FactoryID = w.FactoryID and
													s1.MockupStyle = t.MockupStyle and
													s1.OrderStyle = t.OrderStyle and
													s1.SewingLineID = w.SewingLineID
				where	w.FactoryID = t.FactoryID and
						isnull(w.MockupStyle, t.MockupStyle) = t.MockupStyle and
						isnull(w.OrderStyle, t.OrderStyle) = t.OrderStyle and
						w.SewingLineID = t.SewingLineID and
						w.Date <= t.OutputDate
			)
) CumulateDate

/*
(QA Output):請看PMS > Sewing > P01表身QA Output組資料規則
(New Style/Repeat Style):請看PMS > Sewing > P01表身New Style/Repeat Style欄位產生規則
(CPU/piece): 請看PMS > Sewing > R04產出excel中的CPU/piece欄位的規則
(CPU Sewer/HR): 請看PMS > Sewing > R04產出excel中的CPU Sewer/HR欄位的規則
(Cumulate Date): 請看PMS > Sewing > R04產出excel中的Cumulate Of Days欄位的規則
(RFT):請參考PMS > Quality > P20 表頭RFT欄位公式
*/

drop table #tmp,#tmpOutputDate,#tmpSewingOutput,#tmpWorkHour
end
GO



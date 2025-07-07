Create FUNCTION [dbo].[GetSpreadingSchedule]
(	
	@FactoryID varchar(8),
	@EstCutDate Date,
	@CutCellid varchar(3),
	@Ukey bigint = 0,
	@Cutref varchar(8) = ''
)
RETURNS TABLE 
AS
RETURN 
(
	select 
		sd.SpreadingScheduleUkey,
		w.CutRef,
		sd.IsAGVArrived,
		sd.IsSuspend,
		sd.SpreadingSchdlSeq,
		Completed = IIF(sd.IsAGVArrived = 1, 'Y', 'N'),
		Suspend = IIF(sd.IsSuspend = 1, 'Y', 'N'),
		MaterialStatus='',--暫時不開發，預計之後得到廠商API綠色代表還夠量
		Cutno = w.Seq,--時間緊迫不改APP, 為了不讓APP掛掉欄位名稱照舊 Cutno, 日後維護為了避免誤會希望能改成 SEQ 傳出
		w.Markername,
		w.FabricCombo,
		w.FabricPanelCode,
		Article = stuff((
				Select distinct concat('/' ,b.Article)
				From dbo.WorkOrderForPlanning_Distribute b WITH (NOLOCK) 
				Where b.WorkOrderForPlanningUkey = w.Ukey and b.Article!=''
				For XML path('')
			),1,1,''),
		w.Colorid,
		size.multisize,
		w.Layer,
		TotalCutQty = CutQty.CutQty,
		w.OrderID,
		w.SEQ1,
		w.SEQ2,
		EstCutDate = iif(@Ukey = 0 , w.EstCutDate, s.EstCutDate),
		actcutdate = CAST(NULL AS DATETIME),--時間緊迫不改APP,  為了不讓APP掛掉仍傳出這欄位為 null, 雖然那APP似乎沒人用, 有空再移除此欄位
		w.CutplanID,
		IssueID = Issues.IssueID,
		IsOutStanding = IIF(o.Finished = 0 and w.EstCutDate < CAST(getdate() as date), 'Y', 'N'),
		o.BuyerDelivery,
		w.SCIRefno,
		[ReqQty] = isnull(cp.Cons, 0),
		w.Cons,
		w.Refno,
		f.WeaveTypeID,
		[diffEstCutDate] = IIF(w.EstCutDate <> s.EstCutDate, 1,0)
	from WorkOrderForPlanning w with(nolock)
	inner join orders o with(nolock) on o.id = w.ID
	LEFT join SpreadingSchedule_Detail sd with(nolock) on w.CutRef = sd.CutRef
	LEFT join SpreadingSchedule s with(nolock) on s.Ukey = sd.SpreadingScheduleUkey
	left join Cutplan_Detail cp with (nolock) on cp.ID = w.CutplanID and cp.WorkOrderForPlanningUkey = w.Ukey
	left join Fabric f with (nolock) on f.SCIRefno = w.SCIRefno
	outer apply
	(
		Select multisize = iif(count(ws.sizecode)>1,2,1) 
		From WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK)
		Where w.ukey = ws.WorkOrderForPlanningUkey
	) size
	outer apply
	(
		select CutQty = stuff(
		(
			Select concat(', ', ws.sizecode, '/ ', ws.qty * w.layer)
		    From WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK)
		    Where w.ukey = ws.WorkOrderForPlanningUkey
			For XML path('')
		),1,2,'')
	) CutQty
	outer apply
	(
		select IssueID = stuff(
		(
			Select concat(', ', i.ID)
			From Issue i WITH (NOLOCK) 
			Where i.CutplanID = w.CutplanID AND i.CutplanID <> ''
			For XML path('')
		),1,1,'')
	) Issues

	where 1=1
	and o.Finished = 0
	and w.CutPlanID!=''
	and w.FactoryID = @FactoryID
	and (
		(
			@Ukey = 0 and 
			@Cutref = '' and 
			(w.EstCutDate = @EstCutDate or IsNull(@EstCutDate,'1900/01/01') = '1900/01/01') and
			(w.CutCellid = @CutCellid or IsNull(@CutCellid,'') = '') and
			--排除未來已排SpreadingSchedule的資料
			not exists (select 1 from	SpreadingSchedule ss with(nolock)
								 inner join SpreadingSchedule_Detail ssd with(nolock) on ss.Ukey = ssd.SpreadingScheduleUkey
								 where	(ss.EstCutDate <> @EstCutDate or IsNull(@EstCutDate,'1900/01/01') = '1900/01/01') and
								 		ssd.CutRef = w.CutRef)
		)
		or
		(
			@Ukey = 0 and w.CutRef = @Cutref and @Cutref <> ''
		)
		or
		(
			sd.SpreadingScheduleUkey = @Ukey and
			@Cutref = '' 
		)
	)
)
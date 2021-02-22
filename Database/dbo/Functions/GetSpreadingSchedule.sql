
CREATE FUNCTION [dbo].[GetSpreadingSchedule]
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
		Completed = IIF(sd.IsAGVArrived = 1 or act.actcutdate is not null, 'Y', 'N'),
		Suspend = IIF(sd.IsSuspend = 1, 'Y', 'N'),
		MaterialStatus='',--暫時不開發，預計之後得到廠商API綠色代表還夠量		
		w.Cutno,
		w.Markername,
		w.FabricCombo,
		w.FabricPanelCode,
		art.article,
		w.Colorid,
		size.multisize,
		w.Layer,
		TotalCutQty = CutQty.CutQty,
		w.OrderID,
		w.SEQ1,
		w.SEQ2,
		EstCutDate = iif(@Ukey = 0 , w.EstCutDate, s.EstCutDate),
		act.actcutdate,
		w.CutplanID,
		IsOutStanding = IIF(o.Finished = 0 and w.EstCutDate < CAST(getdate() as date), 'Y', 'N'),
		o.BuyerDelivery
	from WorkOrder w with(nolock)
	inner join orders o with(nolock) on o.id = w.ID
	left join SpreadingSchedule s with(nolock) on	s.FactoryID = @FactoryID
													and s.EstCutDate = @EstCutDate
													and s.CutCellid = @CutCellid
	left join SpreadingSchedule_Detail sd with(nolock) on w.CutRef = sd.CutRef and s.Ukey = sd.SpreadingScheduleUkey
	outer apply
	(
		select article = stuff(
		(
			Select distinct concat('/' ,Article)
			From dbo.WorkOrder_Distribute b WITH (NOLOCK) 
			Where b.workorderukey = w.Ukey and b.article!=''
			For XML path('')
		),1,1,'')
	) art
	outer apply
	(
		Select multisize = iif(count(size.sizecode)>1,2,1) 
		From WorkOrder_SizeRatio size WITH (NOLOCK) 
		Where w.ukey = size.WorkOrderUkey
	) size
	outer apply
	(
		select CutQty = stuff(
		(
			Select concat(', ', ws.sizecode, '/ ', ws.qty * w.layer)
			From WorkOrder_SizeRatio ws WITH (NOLOCK) 
			Where ws.WorkOrderUkey = w.Ukey 
			For XML path('')
		),1,2,'')
	) CutQty
	outer apply
	(
		Select actcutdate = iif(sum(cut_b.Layer) = w.Layer, Max(cut.cdate),null)
			From cuttingoutput cut WITH (NOLOCK) 
			inner join cuttingoutput_detail cut_b WITH (NOLOCK) on cut.id = cut_b.id
			Where cut_b.workorderukey = w.Ukey and cut.Status != 'New'
	) act
	where 1=1
	and o.Finished = 0
	and w.FactoryID = @FactoryID
	and (act.actcutdate is null or sd.CutRef is not null)
	and (
		(
			@Ukey = 0 and 
			@Cutref = '' and 
			w.EstCutDate <= @EstCutDate and
			w.CutCellid = @CutCellid and
			--排除未來已排SpreadingSchedule的資料
			not exists (select 1 from	SpreadingSchedule ss with(nolock)
											inner join SpreadingSchedule_Detail ssd with(nolock) on ss.Ukey = ssd.SpreadingScheduleUkey
									where	ss.EstCutDate >= FORMAT(getdate(), 'yyyyMMdd')  and
											ss.EstCutDate <> @EstCutDate and
											ssd.CutRef = w.CutRef)
		)
		or
		(
			@Ukey = 0 and w.CutRef = @Cutref
		)
		or
		(
			s.Ukey = @Ukey and
			--排除未來已排SpreadingSchedule的資料
			not exists (select 1 from	SpreadingSchedule ss with(nolock)
											inner join SpreadingSchedule_Detail ssd with(nolock) on ss.Ukey = ssd.SpreadingScheduleUkey
									where	ss.EstCutDate >= FORMAT(getdate(), 'yyyyMMdd') and
											ss.EstCutDate <> @EstCutDate and
											ssd.CutRef = w.CutRef)
		)
	)
)
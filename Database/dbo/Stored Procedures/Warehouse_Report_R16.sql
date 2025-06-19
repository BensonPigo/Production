CREATE PROCEDURE [dbo].[Warehouse_Report_R16]
	@IssueDateFrom date = null,
	@IssueDateTo date = null,
	@MDivisionID varchar(8) = '',
	@FactoryID varchar(8) = '',
	@CutplanIDFrom varchar(13) = '',
	@CutplanIDTo varchar(13) = '',
	@SPFrom varchar(13) = '',
	@SPTo varchar(13) = '',
	@EditDateFrom date = null,
	@EditDateTo date = null
AS
begin
	declare @tmpIssueID table(
		ID varchar(13)
	)

	insert into @tmpIssueID(ID)
	select distinct i.Id
	from issue i WITH (NOLOCK) 
	inner join issue_detail id WITH (NOLOCK) on i.id = id.id
	inner join Orders o WITH (NOLOCK) on id.POID = o.id
	where	(i.IssueDate >= @IssueDateFrom or @IssueDateFrom is null) and
			(i.IssueDate <= @IssueDateTo or @IssueDateTo is null) and
			(i.CutplanID >= @CutplanIDFrom or @CutplanIDFrom = '') and
			(i.CutplanID <= @CutplanIDTo or @CutplanIDTo = '') and
			(o.FactoryID = @FactoryID or @FactoryID = '') and
			(o.MDivisionID = @MDivisionID or @MDivisionID = '') And
			(id.POID <=  RIGHT('0000000000' + @SPFrom, 10) or @SPFrom = '') And
			(id.POID = RIGHT('ZZZZZZZZZZ' + @SPTo, 10) or @SPTo = '') And
			(i.AddDate >= @EditDateFrom or i.EditDate >= @EditDateFrom or @EditDateFrom is null) and
			(i.AddDate <= @EditDateTo or i.EditDate <= @EditDateTo or @EditDateTo is null) and
			i.type = 'A' AND i.Status = 'Confirmed' 

	select	 [IssueID] = i.Id
			,[MDivisionID] = o.MDivisionID 
			,[FactoryID] = o.FactoryID
			,[CutplanID] = i.CutplanID
			,[EstCutDate] = c.EstCutDate
			,[IssueDate] = i.IssueDate
			,[Line] =isnull((
				select stuff((
					select concat(',',t.SewLine)
					from (
						select distinct o.SewLine 
						from orders o WITH (NOLOCK)
						where c.Poid = o.POID and o.sewline !=''
					) t
					for xml path('')
				),1,1,'')
			), '')
			,[CutCellID] = c.CutCellID
			,[FabricComboAndCutNo] = isnull(x2.cutref, '')
			,[IssueRemark] = i.Remark
			,[OrderID] = id.POID
			,[Style] = isnull(o.StyleID, '')
			,[Seq] = CONCAT(LTRIM(RTRIM(id.Seq1)),' ',LTRIM(RTRIM(id.Seq2)))
			,[Refno] = isnull(psd.Refno, '')
			,[ColorID] = isnull(psdsC.SpecValue, '')
			,[ColorName] = isnull(Color.Name, '')
			,[Description] = LTRIM(RTRIM(isnull(f.DescDetail, '')))
			,[WeaveTypeID] = isnull(f.WeaveTypeID, '')
			,[RelaxTime] = isnull(r.Relaxtime, 0)
			,[Roll] = id.roll
			,[Dyelot] = id.dyelot
			,[StockUnit] = isnull(psd.StockUnit, '')
			,[IssueQty] = id.Qty
			,[BulkLocation] = isnull(dbo.Getlocation(fi.Ukey), '')
			,[IssueCreateName] = dbo.getPass1_ExtNo(i.AddName)
			,[MINDReleaseName] = concat(id.MINDReleaser,'-',(select Name from Pass1 where Pass1.id =id.MINDReleaser ))
			,[IssueStartTime] = format(i.IssueStartTime,'yyyy/MM/dd HH:mm')
			,[MINDReleaseDate] = format(id.MINDReleaseDate,'yyyy/MM/dd HH:mm')
			,[PickingCompletion] = isnull(CompletionNum.value, 0)
			,[NeedUnroll] = iif (id.NeedUnroll = 1, 'Y', '')
			,[UnrollScanName] = concat(isnull(fu.UnrollScanner, ''), '-', isnull((select Name from Pass1 where Pass1.id =fu.UnrollScanner ), ''))
			,[UnrollMachine] = MIOT.MachineID
			,[UnrollStartTime] = isnull(format(fu.UnrollStartTime, 'yyyy/MM/dd HH:mm'), '')
			,[UnrollEndTime] = isnull(format(fu.UnrollEndTime, 'yyyy/MM/dd HH:mm'), '')
			,[RelaxationStartTime] = isnull(format(fu.RelaxationStartTime,'yyyy/MM/dd HH:mm'), '')
			,[RelaxationEndTime] = isnull(format(fu.RelaxationEndTime,'yyyy/MM/dd HH:mm'), '')
			,[UnrollActualQty] = isnull(fu.UnrollActualQty, 0)
			,[UnrollRemark] = isnull(fu.UnrollRemark, '')
			,[UnrollingRelaxationCompletion] = case 
							when (id.NeedUnroll = 1 and fu.UnrollStatus = 'Done' and fu.RelaxationStartTime is null) then 100
							when (id.NeedUnroll = 1 and fu.UnrollStatus = 'Done' and fu.RelaxationStartTime is not null and fu.RelaxationEndTime <= GETDATE()) then 100
							else 0 end
			,[DispatchScanName] = CONCAT(id.DispatchScanner,'-',(select Name from Pass1 where Pass1.id =id.DispatchScanner ))
			,[DispatchScanTime] = id.DispatchScanTime
			,[RegisterTime] = m360.RegisterTime
            ,[DispatchBy] = CONCAT(m360.DispatchName,'-',(select Name from Pass1 where Pass1.id =m360.DispatchName))
			,[DispatchTime] = m360.DispatchTime
            ,[DispatchReason] = wr.Description
            ,[DispatchRemark] = id.M360MINDDispatchReasonRemark
			,[FactoryReceivedName] = CONCAT(m360.FactoryReceivedName,'-',(select name from Pass1 where Pass1.ID = m360.FactoryReceivedName))
			,[FactoryReceivedTime] = m360.FactoryReceivedTime
			,[AddDate] = i.AddDate
			,[EditDate] = i.EditDate
			,[StockType] = id.StockType
			,[Issue_DetailUkey] = id.ukey
	from issue i WITH (NOLOCK) 
	inner join issue_detail id WITH (NOLOCK) on i.id = id.id
	inner join Orders o WITH (NOLOCK) on id.POID = o.id
	inner join Cutplan c WITH (NOLOCK) on c.ID = i.CutplanID
	left join po_supp_detail psd WITH (NOLOCK) on psd.id = id.poid and psd.seq1 = id.seq1 and psd.seq2 =id.seq2
	left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	left join Color WITH (NOLOCK) on psdsC.SpecValue = Color.ID and Color.BrandId = o.BrandID
	left join Fabric f WITH (NOLOCK) on f.SCIRefno  = psd.SCIRefno
	left join FtyInventory fi WITH (NOLOCK) on fi.POID = id.POID and fi.Seq1 = id.Seq1 and fi.Seq2 = id.Seq2 and fi.Roll = id.Roll and fi.Dyelot = id.Dyelot and id.StockType = fi.StockType
	left join (
					select fr.Relaxtime,rr.Refno
					from [ExtendServer].ManufacturingExecution.dbo.FabricRelaxation fr
					left join [ExtendServer].ManufacturingExecution.dbo.RefnoRelaxtime rr on fr.ID = rr.FabricRelaxationID
				)r on r.Refno = psd.Refno
	left join M360MINDDispatch m360 on m360.Ukey = id.M360MINDDispatchUkey
	left join WHBarcodeTransaction w with (nolock) on w.TransactionID = id.ID
	                                                and w.TransactionUkey = id.Ukey
	                                                and w.Action = 'Confirm'
	left join Fabric_UnrollandRelax fu with (nolock) on fu.Barcode = w.To_NewBarcode
    left join [ExtendServer].ManufacturingExecution.dbo.MachineIoT MIOT with (nolock) on MIOT.Ukey = fu.MachineIoTUkey and MIOT.MachineIoTType= 'unroll'
    left join WhseReason wr on wr.ID = id.M360MINDDispatchReasonID and wr.Type = 'DR'
	outer apply(
		select cutref = stuff((
			Select concat(' / ', w.FabricCombo,'-',x1.CutNo)
			from Cutplan_Detail cd WITH (NOLOCK)
			inner join WorkOrderForPlanning w WITH (NOLOCK) on w.Ukey = cd.WorkOrderForPlanningUkey 
			outer apply(
				select CutNo=stuff((
					select concat(',',cd2.CutNo)
					from Cutplan_Detail cd2 WITH (NOLOCK)
					inner join WorkOrderForPlanning w2 WITH (NOLOCK) on w2.Ukey = cd2.WorkOrderForPlanningUkey 
					where cd2.ID=i.CutplanID and w2.FabricCombo=w.FabricCombo
					group by cd2.CutNo
	                order by cd2.CutNo
					for xml path('')
				),1,1,'')
			)x1
			where cd.ID=i.CutplanID
			group by w.FabricCombo,x1.CutNo
	        order by w.FabricCombo,x1.CutNo
			for xml path('')
		),1,3,'')
	)x2
	outer apply(
		select value = 
		round(
			cast((select count(1) from Issue_Detail sd with(nolock) where sd.id = i.id and sd.MINDReleaseDate is not null)as float) 
				/ (select count(1) from Issue_Detail sd with(nolock) where sd.id = i.id)*100, 2
		)
	)CompletionNum
	where i.Id in (select id from @tmpIssueID)
	order by i.IssueDate, i.ID, i.OrderId, id.Seq1, id.Seq2, id.Roll, id.Dyelot
end
-- =============================================
-- Create date: 2021/10/22
-- Description:	ISP20211230 QKPI Web FinalInspection Data Transfer to PMS QA.P32
-- =============================================
Create Procedure FinalInspectionDataToPMS

AS 
BEGIN
	Begin Transaction
	Begin Try

		SET NOCOUNT ON;

		select f.ID
			, f.AuditDate
			, f.FactoryID
			, f.MDivisionid 
			, f.[SewingLineID] 
			, f.Team 
			, f.[Shift] 
			, f.InspectionStage 
			, f.SampleSize 
			, f.RejectQty
			, [ClogReceivedPercentage] = isnull(clog.Value, 0)
			, f.InspectionResult 
			, f.CFA 
			, f.OthersRemark
			, f.AddName 
			, f.AddDate
			, f.EditName
			, f.EditDate
			, [IsCombinePO] = Forder.IsCombinePO
			, [FirstInspection] = IIF(f.InspectionTimes = 1, 1 , 0)	
			, f.InspectionTimes
		into #tmp_FinalInspection
		from [ExtendServer].[ManufacturingExecution].dbo.FinalInspection f
		left join CFAInspectionRecord c on f.ID = c.ID
		outer apply(
			select [IsCombinePO] = iif(exists(select 1 from [ExtendServer].[ManufacturingExecution].dbo.FinalInspection_Order fo where f.id = fo.id), 1, 0)
		) Forder
		outer apply(
			SELECT Value = CAST(ROUND( SUM(IIF( CFAReceiveDate IS NOT NULL OR ReceiveDate IS NOT NULL
									,ShipQty
									,0)
							) * 1.0 
							/  SUM(ShipQty) * 100 
				,0) AS INT) 
			FROM PackingList_Detail pd WITH(NOLOCK)
			left join CFAInspectionRecord_OrderSEQ co WITH(NOLOCK) on co.OrderID = pd.OrderID and co.SEQ = pd.OrderShipmodeSeq
			left join CFAInspectionRecord c WITH(NOLOCK) on c.ID = co.ID
			where f.ID = c.ID 
		) clog
		where f.submitdate is not null
		and (not exists (select 1 from CFAInspectionRecord c where f.ID = c.ID)
		 or exists (select 1 from CFAInspectionRecord c where f.ID = c.ID and c.ClogReceivedPercentage <> clog.Value))

		-- CFAInspectionRecord
		update t set
			t.ClogReceivedPercentage = s.ClogReceivedPercentage
		from Production.dbo.CFAInspectionRecord t
		inner join #tmp_FinalInspection s on t.ID = s.ID

		insert into [dbo].[CFAInspectionRecord] ([ID], [AuditDate], [FactoryID], [MDivisionid], [SewingLineID], [Team], [Shift]
			, [Stage], [InspectQty], [DefectQty], [ClogReceivedPercentage], [Result], [CFA], [Status], [Remark], [AddName]
			, [AddDate], [EditName], [EditDate], [IsCombinePO], [FirstInspection], [IsImportFromMES])
		select s.ID, s.AuditDate, isnull(s.FactoryID,''), isnull(s.MDivisionid,''), isnull(s.[SewingLineID],''), isnull(s.Team,'A'), isnull(s.[Shift],'D'),
			isnull(s.InspectionStage,''), s.SampleSize, s.RejectQty, s.ClogReceivedPercentage, isnull(s.InspectionResult,''), isnull(s.CFA,''),	'Confirmed', isnull(s.OthersRemark,''),	s.AddName,
			s.AddDate, isnull(s.EditName,''), s.EditDate, s.IsCombinePO, s.FirstInspection, '1'
		from #tmp_FinalInspection s
		where not exists (select 1 from CFAInspectionRecord t where s.ID = t.ID)

		-- CFAInspectionRecord_Detail
		insert into [dbo].[CFAInspectionRecord_Detail]([ID], [GarmentDefectCodeID], [GarmentDefectTypeID], [Qty])
		select fd.ID, fd.GarmentDefectCodeID, fd.GarmentDefectTypeID, fd.Qty
		from [ExtendServer].[ManufacturingExecution].dbo.FinalInspection_Detail fd	
		where exists (select 1 from #tmp_FinalInspection t where fd.ID = t.ID)
		and not exists (select 1 from CFAInspectionRecord_Detail t where fd.ID = t.ID and fd.GarmentDefectCodeID = t.GarmentDefectCodeID)

		-- CFAInspectionRecord_OrderSEQ
		insert into [dbo].[CFAInspectionRecord_OrderSEQ]([ID], [OrderID], [SEQ], [Carton])
		select foq.ID, foq.OrderID, foq.Seq
			,[CTNNo] = isnull(CTN.CTNNoList, '')
		from [ExtendServer].[ManufacturingExecution].dbo.FinalInspection_Order_QtyShip foq
		outer apply(
			select CTNNoList = Stuff((
				select distinct concat(',',CTNNo)
				from [ExtendServer].[ManufacturingExecution].dbo.FinalInspection_OrderCarton foc
				where foc.id = foq.ID
				and foc.OrderID = foq.OrderID
				and foc.Seq = foq.Seq
				for xml path ('')
			) , 1, 1, '')
		) CTN
		where exists (select 1 from #tmp_FinalInspection t where foq.ID = t.ID)
		and not exists (select 1 from CFAInspectionRecord_OrderSEQ t where foq.ID = t.ID and foq.OrderID = t.OrderID and foq.Seq = t.SEQ)

		-- 更新PackingList
		-- StaggeredCFAInspectionRecordID
		-- Stagger, Pass
		UPDATE PackingList_Detail
			SET StaggeredCFAInspectionRecordID = c.ID
		from Production.dbo.PackingList_Detail p WITH(NOLOCK) 
		inner join Production.dbo.CFAInspectionRecord_OrderSEQ co WITH(NOLOCK) on co.OrderID = p.OrderID and co.SEQ = p.OrderShipmodeSeq
		inner join Production.dbo.CFAInspectionRecord c WITH(NOLOCK) on c.ID = co.ID
		where exists (select 1 from #tmp_FinalInspection t where c.ID = t.ID)
		and exists(
			select 1 
			from SplitString(co.Carton,',') sp 
			where sp.Data = p.CTNStartNo
		)
		and c.Stage = 'Stagger'
		and c.Result = 'Pass'
		and p.StaggeredCFAInspectionRecordID = ''

		-- not Stagger, not Pass
		UPDATE PackingList_Detail
			SET StaggeredCFAInspectionRecordID = ''
		from Production.dbo.PackingList_Detail p WITH(NOLOCK) 
		inner join Production.dbo.CFAInspectionRecord_OrderSEQ co WITH(NOLOCK) on co.OrderID = p.OrderID and co.SEQ = p.OrderShipmodeSeq
		inner join Production.dbo.CFAInspectionRecord c WITH(NOLOCK) on c.ID = co.ID
		where exists (select 1 from #tmp_FinalInspection t where c.ID = t.ID)
		and c.Stage != 'Stagger'
		and c.Result != 'Pass'
		and p.StaggeredCFAInspectionRecordID != ''

		-- 更新PackingList
		-- FirstStaggeredCFAInspectionRecordID
		-- Stagger, Pass
		UPDATE PackingList_Detail
			SET FirstStaggeredCFAInspectionRecordID = c.ID
		from Production.dbo.PackingList_Detail p WITH(NOLOCK) 
		inner join Production.dbo.CFAInspectionRecord_OrderSEQ co WITH(NOLOCK) on co.OrderID = p.OrderID and co.SEQ = p.OrderShipmodeSeq
		inner join #tmp_FinalInspection c on c.ID = co.ID
		where exists(
			select 1 
			from SplitString(co.Carton,',') sp 
			where sp.Data = p.CTNStartNo
		)
		and c.InspectionStage = 'Stagger'
		and c.InspectionTimes = 1
		and p.FirstStaggeredCFAInspectionRecordID = ''

		drop table #tmp_FinalInspection
		Commit Transaction;
	End Try
	Begin Catch
		RollBack Transaction;

		Declare @ErrorMessage NVarChar(4000);
		Declare @ErrorSeverity Int;
		Declare @ErrorState Int;

		Set @ErrorMessage = Error_Message();
		Set @ErrorSeverity = Error_Severity();
		Set @ErrorState = Error_State();

		RaisError (@ErrorMessage,	-- Message text.
				   @ErrorSeverity,	-- Severity.
				   @ErrorState		-- State.
				  );
	end Catch

END
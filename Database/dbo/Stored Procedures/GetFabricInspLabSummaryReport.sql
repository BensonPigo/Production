CREATE PROCEDURE [dbo].[GetFabricInspLabSummaryReport]
	@StartDate as date = null,
	@EndDate as date = null
AS
BEGIN
	SET NOCOUNT ON;
	 
	if @StartDate is null 
	begin
		SET @StartDate = CONVERT(VARCHAR(10), DATEADD(MONTH, -3, GETDATE()), 120);
	end

	if @EndDate is null 
	begin
		SET @EndDate = CONVERT(VARCHAR(10), GETDATE(), 120);
	end;

	WITH Main AS(
		select 
		[BalanceQty]=sum(fit.inqty - fit.outqty + fit.adjustqty - fit.ReturnQty) 
		,rd.poid
		,rd.seq1
		,rd.seq2
		,RD.ID
		from dbo.View_AllReceivingDetail rd
		inner join FIR f on f.POID=rd.poid AND  f.ReceivingID = rd.id AND f.seq1 = rd.seq1 and f.seq2 = rd.Seq2
		inner join FtyInventory fit on fit.poid = rd.PoId and fit.seq1 = rd.seq1 and fit.seq2 = rd.Seq2 AND fit.StockType=rd.StockType and fit.Roll=rd.Roll and fit.Dyelot=rd.Dyelot
		WHERE (f.AddDate >= @StartDate AND f.AddDate <= @EndDate)
			OR (f.EditDate >= @StartDate AND f.EditDate <= @EndDate)
		GROUP BY rd.poid,rd.seq1,rd.seq2,RD.ID
	), TmpDataProcessing AS(
		select  
		F.POID
		,(F.SEQ1+'-'+F.SEQ2)SEQ
		,F.ReceivingID
		,O.factoryid
		,O.BrandID
		,O.StyleID
		,O.SeasonID
		,t.ExportId
		,t.InvNo
		,t.WhseArrival
		,[StockQty1] = t.StockQty
		,[InvStock] = t.InvStock
		,[BulkStock] = t.BulkStock
		,[BalanceQty] = IIF(BalanceQty.BalanceQty=0,NULL,BalanceQty.BalanceQty)
		,[TotalRollsCalculated] = t.TotalRollsCalculated
		,mp.ALocation
		,LT.BulkLocationDate
		,mp.BLocation	
		,ILT.InvLocationDate
		,[MinSciDelivery] = (SELECT MinSciDelivery FROM  DBO.GetSCI(F.Poid,O.Category))
		,[MinBuyerDelivery] = (SELECT MinBuyerDelivery  FROM  DBO.GetSCI(F.Poid,O.Category))
		,F.Refno
		,C.Description
		,[ColorID] = ps.SpecValue
		,[ColorName] = color.Name
		,[SupplierCode] = SP.SuppID
		,[SupplierName] = s.AbbEN
		,C.WeaveTypeID
		,[NAPhysical] = IIF(F.Nonphysical = 1,'Y',' ')
		,F.Result
		,[CutShadebandQtyByRoll] = Qty.Roll
		,[CutShadeband] = Shadeband.sCount
		,F.Physical
		,[PhysicalInspector] = (select name from Pass1 where id = f.PhysicalInspector)
		,F.PhysicalDate
		,TotalYardage = TotalYardage.Val
		,TotalYardageArrDate  = TotalYardage.Val -ActTotalYds.ActualYds
		,fta.ActualYds
		,[InspectionRate] = ROUND(iif(t.StockQty = 0,0,CAST (fta.ActualYds/t.StockQty AS FLOAT)) ,3)
		,ftp.TotalPoint
		,F.CustInspNumber
		,F.Weight
		,[WeightInspector] = (select name from Pass1 where id = f.WeightInspector)
		,F.WeightDate
		,F.ShadeBond
		,[ShadeboneInspector] = (select name from Pass1 where id = f.ShadeboneInspector)
		,F.ShadeBondDate
		,[ShadeBandPass] =  [Shade_Band_Pass].cnt
		,[ShadeBandFail] =  [Shade_Band_Fail].cnt
		,F.Continuity
		,[ContinuityInspector] = (select name from Pass1 where id = f.ContinuityInspector)
		,F.ContinuityDate
		,F.Odor
		,[OdorInspector] = (select name from Pass1 where id = f.OdorInspector)
		,F.OdorDate
		,F.Moisture
		,F.MoistureDate
		,[CrockingShrinkageOverAllResult] = L.Result
		,[NACrocking] = IIF(L.nonCrocking=1,'Y',' ')
		,LC.Crocking
		,fl.CrockingInspector
		,LC.CrockingDate
		,[NAHeatShrinkage] = IIF(L.nonHeat=1,'Y',' ')
		,LH.Heat
		,fl.HeatInspector
		,LH.HeatDate
		,[NAWashShrinkage] = IIF(L.nonWash=1,'Y',' ' )
		,LW.Wash
		,fl.WashInspector
		,LW.WashDate
		,[OvenTestResult] = V.Result
		,[OvenTestInspector] = v2.Name
		,[ColorFastnessResult] = CFD.Result
		,[ColorFastnessInspector] = cfd2.Name
		,[LocalMR] = ps1.LocalMR
		,[Category] = ddl.Name
		,[CuttingDate] = o.CutInLine
		,[OrderType] = O.OrderTypeID
		,[TotalYardsBC] = isnull(fptbc.TicketYds, 0)
		,[TotalPointBC] = isnull(fptbc.TotalPoint, 0)
		,[TotalPointA] = isnull(fpta.TotalPoint, 0)
		,F.AddDate
		,F.EditDate
		,t.StockType
		,[KPILETA] = O.KPILETA
		,[ACTETA] = Export.Eta
		,[Packages] = isnull(e.Packages,0)
		,[SampleRcvDate] = fl.ReceiveSampleDate
		,[InspectionGroup] = (Select InspectionGroup from Fabric where SCIRefno = p.SCIRefno)
		,[CGradeTOP3Defects] = isnull(CGradT3.Value,'')
		,[AGradeTOP3Defects] = ISNULL(AGradT3.Value,'')
		,[TotalLotNumber] = TotalLotNumber.TotalLotNumber
		,[InspectedLotNumber] = InspectedLotNumber.InspectedLotNumber
		,[CutShadebandTime] = Qty.CutTime
		,[OvenTestDate] = V3.InspDate
		,[ColorFastnessTestDate] = CFD3.InspDate
		,[MCHandle_id] = COALESCE(pass1_MCHandle.id, TPEPass1_MCHandle.id)
		,[MCHandle_name] = COALESCE(pass1_MCHandle.name, TPEPass1_MCHandle.name)
		,[MCHandle_extno] = COALESCE(pass1_MCHandle.extno, TPEPass1_MCHandle.extno)
		,[OrderQty] = Round(dbo.getUnitQty(p.POUnit, p.StockUnit, isnull(p.Qty, 0)), 2)
		,[ActTotalRollInspection] = ActTotalRollsInspection.Cnt
		,[Complete] = iif(P.Complete='1','Y','N')
		from dbo.FIR F WITH (NOLOCK) 
		cross apply(
			select rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2
			,[StockQty] = sum(RD.StockQty)
			,[InvStock] = iif(rd.StockType = 'I', sum(RD.StockQty), 0)
			,[BulkStock] = iif(rd.StockType = 'B', sum(RD.StockQty), 0)
			,[StockType] = rd.StockType
			,TotalRollsCalculated = count(1)
			from dbo.View_AllReceivingDetail rd WITH (NOLOCK) 
			where rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Id=F.ReceivingID
			AND ((f.AddDate >= @StartDate AND f.AddDate <= @EndDate)
				OR (f.EditDate >= @StartDate AND f.EditDate <= @EndDate))
			group by rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2,rd.StockType
		) t
		inner join (
			select distinct poid,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category,id ,CutInLine, o.OrderTypeID,MCHandle,o.KPILETA
			from dbo.Orders o WITH (NOLOCK)  
			 where O.Category in ('B','S','M','T','A')
		) O on O.id = F.POID
		left join pass1 pass1_MCHandle with(nolock) on pass1_MCHandle.id = O.MCHandle
		left join TPEPass1 TPEPass1_MCHandle with(nolock) on TPEPass1_MCHandle.id = O.MCHandle
		left join DropDownList ddl with(nolock) on o.Category = ddl.ID and ddl.Type = 'Category'
		inner join dbo.PO_Supp SP WITH (NOLOCK) on SP.id = F.POID and SP.SEQ1 = F.SEQ1
		inner join dbo.PO_Supp_Detail P WITH (NOLOCK) on P.ID = F.POID and P.SEQ1 = F.SEQ1 and P.SEQ2 = F.SEQ2
		left join dbo.PO_Supp_Detail_Spec ps WITH (NOLOCK) on P.ID = ps.id and P.SEQ1 = ps.SEQ1 and P.SEQ2 = ps.SEQ2 and ps.SpecColumnID='Color'
		inner join supp s WITH (NOLOCK) on s.id = SP.SuppID 
		LEFT JOIN Main BalanceQty ON BalanceQty.poid = f.POID and BalanceQty.seq1 = f.seq1 and BalanceQty.seq2 =f.seq2 AND BalanceQty.ID = f.ReceivingID
		left join MDivisionPoDetail mp on mp.POID=f.POID and mp.Seq1=f.SEQ1 and mp.Seq2=f.SEQ2
		left join Receiving on f.ReceivingID = Receiving.ID
		left join Export on Receiving.ExportId = Export.ID
		outer apply(select count(1) Cnt from FIR_Physical fp where fp.id = f.id) ActTotalRollsInspection
		outer apply(
			select [Packages] = sum(e.Packages)
			from Export e with (nolock)
			where exists(
				select 1
				from export e2 with(nolock)
				where e2.Blno = e.Blno
				and Receiving.ExportId = e2.ID
			)
		)e
		OUTER APPLY(
			SELECT * FROM  Fabric C WITH (NOLOCK) WHERE C.SCIRefno = F.SCIRefno
		)C
		OUTER APPLY(
				SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1		
				AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
				)L
		OUTER APPLY(
				SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1
				AND L.CrockingEncode=1 
				AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
				)LC
		OUTER APPLY(
				SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1 
				AND L.HeatEncode=1 
				AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
				)LH
		OUTER APPLY(
				SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1
				AND L.WashEncode=1 
				AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
				)LW
		OUTER APPLY(
		select Result = Stuff((
				select concat(',',Result)
				from (
					select distinct od.Result 
					from dbo.Oven ov WITH (NOLOCK) 
					inner join dbo.Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
					left join pass1 WITH (NOLOCK) on pass1.id = ov.Inspector
					where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 
					and ov.Status='Confirmed'
				) s
				for xml path ('')
			) , 1, 1, '')
		)V
		OUTER APPLY(
		select [name ]= Stuff((
				select concat(',',name)
				from (
					select distinct pass1.name 
					from dbo.Oven ov WITH (NOLOCK) 
					inner join dbo.Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
					left join pass1 WITH (NOLOCK) on pass1.id = ov.Inspector
					where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 
					and ov.Status='Confirmed'
				) s
				for xml path ('')
			) , 1, 1, '')
		)V2
		OUTER APPLY(
		select Result = Stuff((
				select concat(',',Result)
				from (
					select distinct cd.Result
					from dbo.ColorFastness CF WITH (NOLOCK) 
					inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
					left join pass1 WITH (NOLOCK) on pass1.id = cf.Inspector
					where CF.Status = 'Confirmed' 
					and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
				) s
				for xml path ('')
			) , 1, 1, '')
		)CFD
		OUTER APPLY(
		select Name = Stuff((
				select concat(',',Name)
				from (
					select distinct Pass1.Name
					from dbo.ColorFastness CF WITH (NOLOCK) 
					inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
					left join pass1 WITH (NOLOCK) on pass1.id = cf.Inspector
					where CF.Status = 'Confirmed' 
					and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
				) s
				for xml path ('')
			) , 1, 1, '')
		)CFD2
		Outer apply(
			select (A.id+' - '+ A.name + ' #'+A.extno) LocalMR 
			from orders od with(nolock) 
			inner join pass1 a with(nolock) on a.id=od.LocalMR 
			where od.id=o.POID
		) ps1
		outer apply(select TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp where fp.id=f.id) ftp
		outer apply
		(
			select Val = Sum(ISNULL(fi.InQty,0))
			from FtyInventory fi  with(nolock)
			inner join Receiving_Detail rd   with(nolock) on rd.PoId = fi.POID and rd.Seq1 = fi.Seq1 and rd.Seq2 = fi.Seq2 AND fi.StockType=rd.StockType and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
			where fi.POID = f.POID AND fi.Seq1 = f.Seq1 AND fi.Seq2 = f.Seq2 AND rd.Id=f.ReceivingID AND rd.ForInspection=1
		) TotalYardage
		outer apply
		(
			select ActualYds = Sum(fp.ActualYds) 
			from FIR_Physical fp with(nolock)
			where fp.ID = f.ID and EXISTS(
			select 1
			from Receiving r with(nolock)
			where r.Id=f.ReceivingID
			AND r.WhseArrival >= t.WhseArrival
			AND r.WhseArrival <= t.WhseArrival
		)
		) ActTotalYds

		outer apply(select ActualYds = Sum(fp.ActualYds) from FIR_Physical fp with(nolock) where fp.id=f.id) fta
		outer apply(select TicketYds = Sum(fp.TicketYds), TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp with(nolock) where fp.id = f.id and (fp.Grade = 'B' or fp.Grade = 'C')) fptbc
		outer apply(select TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp with(nolock) where fp.id = f.id and fp.Grade = 'A') fpta
		outer apply(select  CrockingInspector = (select name from Pass1 with(nolock) where id = CrockingInspector)
			,HeatInspector = (select name from Pass1 with(nolock) where id = HeatInspector)
			,WashInspector = (select name from Pass1 with(nolock) where id = WashInspector)
			,ReceiveSampleDate
			from FIR_Laboratory where Id=f.ID
		)FL
		outer apply
		(
			SELECT Min(a.EditDate) BulkLocationDate
			FROM LocationTrans a WITH (NOLOCK) 
			inner join LocationTrans_detail as b WITH (NOLOCK) on a.ID = b.ID 
			WHERE a.status = 'Confirmed' and b.stocktype='B'
			AND b.Poid=f.POID and b.Seq1=f.SEQ1 and b.Seq2=f.SEQ2
		)LT
		outer apply
		(
			SELECT Min(a.EditDate) InvLocationDate
			FROM LocationTrans a WITH (NOLOCK) 
			inner join LocationTrans_detail as b WITH (NOLOCK) on a.ID = b.ID 
			WHERE a.status = 'Confirmed' and b.stocktype='I'
			AND b.Poid=f.POID and b.Seq1=f.SEQ1 and b.Seq2=f.SEQ2
		)ILT
		outer apply
		(
			select Roll = count(Roll+Dyelot),CutTime=max(fs.CutTime) from FIR_Shadebone fs where fs.ID =F.ID and fs.CutTime is not null
		)
		Qty 
		outer apply
		(
			select
				[sCount] = iif(isnull(s.Roll,0) = 0 , 0, cast(Qty.Roll as float) / cast(s.Roll as float)) 
			from
			(
				select Roll = count(Roll+Dyelot) 
				from FIR_Shadebone fs
				where fs.ID = f.ID 
			) s
		) Shadeband
		outer apply(
			select cnt = count(1) 
			from FIR_Shadebone t
			where UPPER(Result) = UPPER('Pass')
			and t.ID = f.ID
		) [Shade_Band_Pass]
		outer apply(
			select cnt = count(1) 
			from FIR_Shadebone t
			where UPPER(Result) = UPPER('Fail')
			and t.ID = f.ID
		) [Shade_Band_Fail]
		OUTER APPLY(
			SELECT Name 
			FROM Color c WITH (NOLOCK)
			where c.BrandId = O.BrandID 
			and c.ID = ps.SpecValue
		)color
		OUTER APPLY
		(
			select [Value]= Stuff((
				select concat(',',defect)
				from (
					select TOP 3 defect = concat(fd.DescriptionEN,'(',count(1),')'),count(1) as Qty
					from FIR
					INNER JOIN FIR_Physical fp ON FIR.ID= FP.ID
					inner join FIR_Physical_Defect_Realtime fpd on fp.DetailUkey = fpd.FIR_PhysicalDetailUKey
					inner join FabricDefect fd on fd.ID=fpd.FabricdefectID
					where 1=1
					and fp.Grade='C'
					AND FIR.ID = F.ID 
					group by fd.DescriptionEN
					order by Qty desc, fd.DescriptionEN asc
				) s
				for xml path ('')
			) , 1, 1, '')
		)CGradT3
		OUTER APPLY
		(
			select [Value]= Stuff((
				select concat(',',defect)
				from (
					select TOP 3 defect = concat(fd.DescriptionEN,'(',count(1),')'),count(1) as Qty
					from FIR
					INNER JOIN FIR_Physical fp ON FIR.ID= FP.ID
					inner join FIR_Physical_Defect_Realtime fpd on fp.DetailUkey = fpd.FIR_PhysicalDetailUKey
					inner join FabricDefect fd on fd.ID=fpd.FabricdefectID
					where 1=1
					and fp.Grade='A'
					AND FIR.ID=F.ID
					group by fd.DescriptionEN
					order by Qty desc, fd.DescriptionEN asc
				) s
				for xml path ('')
			) , 1, 1, '')
		)AGradT3
		OUTER APPLY
		(
			SELECT TotalLotNumber = COUNT(1)
			FROM(
				SELECT DISTINCT rd.Dyelot
				FROM View_AllReceivingDetail rd WITH (NOLOCK)
				WHERE rd.Id = F.ReceivingID 
				AND rd.PoId = F.POID
				AND rd.Seq1 = F.SEQ1
				AND rd.Seq2 = F.SEQ2 
			)TotalLotNumber
		)TotalLotNumber
		OUTER APPLY
		(
			SELECT InspectedLotNumber = COUNT(1)
			FROM(
				SELECT DISTINCT rd.Dyelot
				FROM View_AllReceivingDetail rd WITH (NOLOCK)
				INNER JOIN FIR f2 WITH (NOLOCK) on f2.ReceivingID = rd.Id and f2.POID = rd.PoId and f2.SEQ1 = rd.Seq1 and f2.SEQ2 = rd.Seq2
				INNER JOIN FIR_Physical fp WITH (NOLOCK) on f.id = fp.ID and fp.Roll = rd.Roll and fp.Dyelot = fp.Dyelot
				WHERE rd.Id = F.ReceivingID 
				AND rd.PoId = F.POID
				AND rd.Seq1 = F.SEQ1
				AND rd.Seq2 = F.SEQ2 
			)InspectedLotNumber
		)InspectedLotNumber
		OUTER APPLY
		(
			SELECT InspDate = Stuff((
				SELECT CONCAT(',', InspDate)
				FROM (
					SELECT DISTINCT InspDate = FORMAT(ov.InspDate, 'yyyy/MM/dd')
					FROM dbo.Oven ov WITH (NOLOCK)
					INNER JOIN Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
					WHERE ov.POID = F.POID
					AND od.Seq1 = F.Seq1
					AND od.Seq2 = F.Seq2
					AND ov.Status = 'Confirmed'
				) s
				FOR XML PATH ('')
			), 1, 1, '')
		)V3
		OUTER APPLY
		(
			SELECT InspDate = Stuff((
					SELECT CONCAT(',',InspDate)
					FROM (
						SELECT DISTINCT InspDate = FORMAT(cf.InspDate, 'yyyy/MM/dd')
						FROM ColorFastness cf WITH (NOLOCK) 
						INNER JOIN ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = cf.ID
						WHERE cf.POID = F.POID
						AND cd.SEQ1 = F.Seq1
						AND cd.seq2 = F.Seq2
						AND cf.Status = 'Confirmed'
					) s
					FOR XML PATH ('')
				), 1, 1, '')
		)CFD3
	), Tmp_Finish AS(
		select [Category] = ISNULL([Category], '')
			,[POID] = ISNULL([POID], '')
			,[SEQ] = ISNULL([SEQ], '')
			,[FactoryID] = ISNULL([FactoryID], '')
			,[BrandID] = ISNULL([BrandID], '')
			,[StyleID] = ISNULL([StyleID], '')
			,[SeasonID] = ISNULL([SeasonID], '')
			,[Wkno] = ISNULL([ExportId], '')
			,[InvNo] = ISNULL([InvNo], '')
			,[CuttingDate]
			,[ArriveWHDate] = [WhseArrival]
			,[ArriveQty] = ISNULL([StockQty1], 0)
			,[Inventory] = ISNULL([InvStock], 0)
			,[Bulk] = ISNULL([BulkStock], 0)
			,[BalanceQty] = ISNULL([BalanceQty], 0)
			,[TtlRollsCalculated] = ISNULL([TotalRollsCalculated], 0)
			,[BulkLocation] = ISNULL([ALocation], '')
			,[FirstUpdateBulkLocationDate] = [BulkLocationDate]
			,[InventoryLocation] = ISNULL([BLocation], '')
			,[FirstUpdateStocksLocationDate] = [InvLocationDate]
			,[EarliestSCIDelivery] = [MinSciDelivery]
			,[BuyerDelivery] = [MinBuyerDelivery]
			,[Refno] = ISNULL([Refno], '')
			,[Description] = ISNULL([Description], '')
			,[Color] = ISNULL([ColorID], '')
			,[ColorName] = ISNULL([ColorName], '')
			,[SupplierCode] = ISNULL([SupplierCode], '')
			,[SupplierName] = ISNULL([SupplierName], '')
			,[WeaveType] = ISNULL([WeaveTypeID], '')
			,[NAPhysical] = ISNULL([NAPhysical], '')
			,[InspectionOverallResult] = ISNULL([Result], '')
			,[PhysicalInspResult] = ISNULL(Physical, '')
			,[TtlYrdsUnderBCGrade] = ISNULL([TotalYardsBC], 0)
			,[TtlPointsUnderBCGrade] = ISNULL([TotalPointBC], 0)
			,[TtlPointsUnderAGrade] = ISNULL([TotalPointA], 0)
			,[PhysicalInspector] = ISNULL([PhysicalInspector], '')
			,[PhysicalInspDate] = [PhysicalDate]
			,[TotalYardage] = ISNULL([TotalYardage],0)
			,[TotalYardageArrDate] = ISNULL([TotalYardageArrDate],0)
			,[ActTtlYdsInspection] = ISNULL([ActualYds], 0)
			,[InspectionPCT] = ISNULL(CAST([InspectionRate] * 100 AS NUMERIC(6,1)), 0)
			,[PhysicalInspDefectPoint] = ISNULL([TotalPoint], 0)
			,[CustInspNumber] = ISNULL([CustInspNumber], '')
			,[WeightTestResult] = ISNULL([Weight], '')
			,[WeightTestInspector] = ISNULL([WeightInspector], '')
			,[WeightTestDate] = [WeightDate]
			,[CutShadebandQtyByRoll] = ISNULL([CutShadebandQtyByRoll], 0)
			,[CutShadebandPCT] = ISNULL(CAST([CutShadeband] * 100 AS NUMERIC(5,2)), 0)
			,[ShadeBondResult] = ISNULL([ShadeBond], '')
			,[ShadeBondInspector] = ISNULL([ShadeboneInspector], '')
			,[ShadeBondDate] = [ShadeBondDate]
			,[NoOfRollShadebandPass] = ISNULL([ShadeBandPass], 0)
			,[NoOfRollShadebandFail] = ISNULL([ShadeBandFail], 0)
			,[ContinuityResult] = ISNULL([Continuity], '')
			,[ContinuityInspector] = ISNULL([ContinuityInspector], '')
			,[ContinuityDate]
			,[OdorResult] = ISNULL([Odor], '')
			,[OdorInspector] = ISNULL([OdorInspector], '')
			,[OdorDate]
			,[MoistureResult] = ISNULL([Moisture], '')
			,[MoistureDate]
			,[CrockingShrinkageOverAllResult] = ISNULL([CrockingShrinkageOverAllResult], '')
			,[NACrocking] = ISNULL([NACrocking], '')
			,[CrockingResult] = ISNULL([Crocking], '')
			,[CrockingInspector] = ISNULL([CrockingInspector], '')
			,[CrockingTestDate] = [CrockingDate]
			,[NAHeatShrinkage] = ISNULL([NAHeatShrinkage], '')
			,[HeatShrinkageTestResult] = ISNULL([Heat], '')
			,[HeatShrinkageInspector] = ISNULL([HeatInspector], '')
			,[HeatShrinkageTestDate] = [HeatDate]
			,[NAWashShrinkage] = ISNULL([NAWashShrinkage], '')
			,[WashShrinkageTestResult] = ISNULL([Wash], '')
			,[WashShrinkageInspector] = ISNULL([WashInspector], '')
			,[WashShrinkageTestDate] = [WashDate]
			,[OvenTestResult] = ISNULL([OvenTestResult], '')
			,[OvenTestInspector] = ISNULL([OvenTestInspector], '')
			,[ColorFastnessResult] = ISNULL([ColorFastnessResult], '')
			,[ColorFastnessInspector] = ISNULL([ColorFastnessInspector], '')
			,[LocalMR] = ISNULL([LocalMR], '')
			,[OrderType] = ISNULL([OrderType], '')
			,[ReceivingID] = ISNULL([ReceivingID], '')			
			,[AddDate]
			,[EditDate]
			,[StockType] = ISNULL([StockType], '')
			,[KPILETA] = [KPILETA]
			,[ACTETA] = [ACTETA]
			,[Packages] = ISNULL([Packages],0)
			,[SampleRcvDate]
			,[InspectionGroup] = ISNULL([InspectionGroup],'')
			,[CGradeTOP3Defects] = ISNULL([CGradeTOP3Defects],'')
			,[AGradeTOP3Defects] = ISNULL([AGradeTOP3Defects],'')
			,[TotalLotNumber] = ISNULL([TotalLotNumber],0)
			,[InspectedLotNumber] = ISNULL([InspectedLotNumber],0)
			,[CutShadebandTime]
			,[OvenTestDate] = ISNULL([OvenTestDate],'')
			,[ColorFastnessTestDate]  = ISNULL([ColorFastnessTestDate],'')
			,[MCHandle] = ISNULL(MCHandle_id + '-' + MCHandle_name + '#' + MCHandle_extno,'') 
			,[OrderQty]  = ISNULL([OrderQty],0)
			,[ActTotalRollInspection] = ISNULL([ActTotalRollInspection],0)
			,[Complete] = ISNULL([Complete],'')
			from TmpDataProcessing
	)
	SELECT * from Tmp_Finish ORDER BY POID,SEQ
END
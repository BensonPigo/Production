﻿CREATE PROCEDURE [dbo].[GetFabricInspLabSummaryReport]
AS
BEGIN
SET NOCOUNT ON;
	DECLARE @ArrivalStartDate VARCHAR(10);
	DECLARE @ArrivalEndDate VARCHAR(10);
	DECLARE @StartDate VARCHAR(10);
	DECLARE @EndDate VARCHAR(10);



	IF NOT EXISTS (SELECT 1 FROM [ExtendServer].[POWERBIReportData].[DBO].[P_FabricInspLabSummaryReport])
	BEGIN
		SET @ArrivalStartDate = '2022-01-01';
		SET @ArrivalEndDate= CONVERT(VARCHAR(10), GETDATE(), 120);
	END
	ELSE
	BEGIN
		SET @StartDate = CONVERT(VARCHAR(10), DATEADD(MONTH, -3, GETDATE()), 120);
		SET @EndDate = CONVERT(VARCHAR(10), GETDATE(), 120);
	END;

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
		WHERE
		1 = 1 
		AND (@ArrivalStartDate is null or rd.WhseArrival >= @ArrivalStartDate)
		AND (@ArrivalEndDate is null or rd.WhseArrival <= @ArrivalEndDate)
		AND
		(
			((@StartDate IS NULL AND @EndDate IS NULL) OR f.AddDate >= DATEADD(MONTH, -3, GETDATE()) AND f.AddDate <= @EndDate)
			OR
			((@StartDate IS NULL AND @EndDate IS NULL) OR f.EditDate >= DATEADD(MONTH, -3, GETDATE()) AND f.EditDate <= @EndDate)
		)
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
		from dbo.FIR F WITH (NOLOCK) 
		cross apply(
			select rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2
			,[StockQty] = sum(RD.StockQty)
			,[InvStock] = iif(rd.StockType = 'I', sum(RD.StockQty), 0)
			,[BulkStock] = iif(rd.StockType = 'B', sum(RD.StockQty), 0)
			,TotalRollsCalculated = count(1)
			from dbo.View_AllReceivingDetail rd WITH (NOLOCK) 
			where rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Id=F.ReceivingID
			AND (@ArrivalStartDate is null or rd.WhseArrival >= @ArrivalStartDate)
			AND (@ArrivalEndDate is null or rd.WhseArrival <= @ArrivalEndDate)
			AND
			(
				((@StartDate IS NULL AND @EndDate IS NULL) OR f.AddDate >= CONVERT(VARCHAR(10), DATEADD(MONTH, -3, GETDATE()), 120) AND f.AddDate <= @EndDate)
				OR
				((@StartDate IS NULL AND @EndDate IS NULL) OR f.EditDate >= CONVERT(VARCHAR(10), DATEADD(MONTH, -3, GETDATE()), 120) AND f.EditDate <= @EndDate)
			)
			group by rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2,rd.StockType
		) t
		inner join (
			select distinct poid,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category,id ,CutInLine, o.OrderTypeID
			from dbo.Orders o WITH (NOLOCK)  
			 where O.Category in ('B')
		) O on O.id = F.POID
		left join DropDownList ddl with(nolock) on o.Category = ddl.ID and ddl.Type = 'Category'
		inner join dbo.PO_Supp SP WITH (NOLOCK) on SP.id = F.POID and SP.SEQ1 = F.SEQ1
		inner join dbo.PO_Supp_Detail P WITH (NOLOCK) on P.ID = F.POID and P.SEQ1 = F.SEQ1 and P.SEQ2 = F.SEQ2
		left join dbo.PO_Supp_Detail_Spec ps WITH (NOLOCK) on P.ID = ps.id and P.SEQ1 = ps.SEQ1 and P.SEQ2 = ps.SEQ2 and ps.SpecColumnID='Color'
		inner join supp s WITH (NOLOCK) on s.id = SP.SuppID 
		LEFT JOIN Main BalanceQty ON BalanceQty.poid = f.POID and BalanceQty.seq1 = f.seq1 and BalanceQty.seq2 =f.seq2 AND BalanceQty.ID = f.ReceivingID
		left join MDivisionPoDetail mp on mp.POID=f.POID and mp.Seq1=f.SEQ1 and mp.Seq2=f.SEQ2
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
			from orders od 
			inner join pass1 a on a.id=od.LocalMR 
			where od.id=o.POID
		) ps1
		outer apply(select TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp where fp.id=f.id) ftp
		outer apply(select ActualYds = Sum(fp.ActualYds) from FIR_Physical fp where fp.id=f.id) fta
		outer apply(select TicketYds = Sum(fp.TicketYds), TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp where fp.id = f.id and (fp.Grade = 'B' or fp.Grade = 'C')) fptbc
		outer apply(select TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp where fp.id = f.id and fp.Grade = 'A') fpta
		outer apply(select  CrockingInspector = (select name from Pass1 where id = CrockingInspector)
			,HeatInspector = (select name from Pass1 where id = HeatInspector)
			,WashInspector = (select name from Pass1 where id = WashInspector)
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
			select Roll = count(Roll+Dyelot) from FIR_Shadebone fs where fs.ID =F.ID and fs.CutTime is not null
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
	), Tmp_Finish AS(
		select
		[Category]
		,[POID]
		,[SEQ]
		,[FactoryID]
		,[BrandID]
		,[StyleID]
		,[SeasonID]
		,[Wkno] = [ExportId]
		,[InvNo]
		,[CuttingDate]
		,[ArriveWHDate] = [WhseArrival]
		,[ArriveQty] = [StockQty1]
		,[Inventory] = [InvStock]
		,[Bulk] = [BulkStock]
		,[BalanceQty]
		,[TtlRollsCalculated] = [TotalRollsCalculated]
		,[BulkLocation] = [ALocation]
		,[FirstUpdateBulkLocationDate] = [BulkLocationDate]
		,[InventoryLocation] = [BLocation]
		,[FirstUpdateStocksLocationDate] = [InvLocationDate]
		,[EarliestSCIDelivery] = [MinSciDelivery]
		,[BuyerDelivery] = [MinBuyerDelivery]
		,[Refno]
		,[Description]
		,[Color] = [ColorID]
		,[ColorName]
		,[SupplierCode]
		,[SupplierName]
		,[WeaveType] = [WeaveTypeID]
		,[NAPhysical] = [NAPhysical]
		,[InspectionOverallResult] = Result
		,[PhysicalInspResult] = Physical
		,[TtlYrdsUnderBCGrade] =  [TotalYardsBC]
		,[TtlPointsUnderBCGrade] = [TotalPointBC]
		,[TtlPointsUnderAGrade] = [TotalPointA]
		,[PhysicalInspector]
		,[PhysicalInspDate] = [PhysicalDate]
		,[ActTtlYdsInspection] = [ActualYds]
		,[InspectionPCT] = CAST([InspectionRate] * 100 AS NUMERIC(6,1))
		,[PhysicalInspDefectPoint] = [TotalPoint]
		,[CustInspNumber] = [CustInspNumber]
		,[WeightTestResult] = [Weight]
		,[WeightTestInspector] = [WeightInspector]
		,[WeightTestDate] = [WeightDate]
		,[CutShadebandQtyByRoll]
		,[CutShadebandPCT] = CAST([CutShadeband] * 100 AS NUMERIC(5,2))
		,[ShadeBondResult] = [ShadeBond]
		,[ShadeBondInspector] = [ShadeboneInspector]
		,[ShadeBondDate] = [ShadeBondDate]
		,[NoOfRollShadebandPass] = [ShadeBandPass]
		,[NoOfRollShadebandFail] = [ShadeBandFail]
		,[ContinuityResult] = [Continuity]
		,[ContinuityInspector]
		,[ContinuityDate]
		,[OdorResult] = [Odor]
		,[OdorInspector]
		,[OdorDate]
		,[MoistureResult] = [Moisture]
		,[MoistureDate]
		,[CrockingShrinkageOverAllResult]
		,[NACrocking] 
		,[CrockingResult] = [Crocking]
		,[CrockingInspector]
		,[CrockingTestDate] = [CrockingDate]
		,[NAHeatShrinkage]
		,[HeatShrinkageTestResult] = [Heat]
		,[HeatShrinkageInspector] = [HeatInspector]
		,[HeatShrinkageTestDate] = [HeatDate]
		,[NAWashShrinkage] 
		,[WashShrinkageTestResult] = [Wash]
		,[WashShrinkageInspector] = [WashInspector]
		,[WashShrinkageTestDate] = [WashDate]
		,[OvenTestResult]
		,[OvenTestInspector]
		,[ColorFastnessResult]
		,[ColorFastnessInspector]
		,[LocalMR]
		,[OrderType]
		,[ReceivingID]
		,[AddDate]
		,[EditDate]
		from TmpDataProcessing
	)

	SELECT * from Tmp_Finish ORDER BY POID,SEQ
END
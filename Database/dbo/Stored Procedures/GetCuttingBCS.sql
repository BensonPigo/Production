CREATE PROCEDURE [dbo].[GetCuttingBCS]
AS
BEGIN
SET NOCOUNT ON;
	WITH StdQ_MainDates AS (
		SELECT 
		  [MDivisionID] = s.MDivisionID
		, [FactoryID] = s.FactoryID
		, [FactoryGroup] = o.FtyGroup
		, [OrderID]
		, [SewInLineDate] = CONVERT(DATE, MIN(CONVERT(DATE, Inline))) 
		, [SewOffLineDate] = CONVERT(DATE, MIN(CONVERT(DATE, offline)))
		, [MinDate] = CONVERT(DATE, MIN(CONVERT(DATE, Inline)))
		, [MaxDate] = CONVERT(DATE, Max(CONVERT(DATE, offline)))
		FROM SewingSchedule s
		inner join Orders o on o.id = s.OrderID
		WHERE  
		([Offline] BETWEEN DATEADD(DAY, -30, GETDATE()) AND GETDATE() -- Filter for the last 30 days
		OR [Inline] BETWEEN GETDATE() AND DATEADD(DAY, 75, GETDATE())) -- Filter for the next 75 days
		AND s.BIPImportCuttingBCCmdTime IS NULL
		GROUP BY s.FactoryID, OrderID, s.MDivisionID,o.FtyGroup
	),  StdQ_DateRange AS (
		SELECT DISTINCT
		  [APSNo] = SS.APSNo
		, [MDivisionID] = MD.MDivisionID
		, [FactoryID] = MD.FactoryID
		, [OrderID] = MD.OrderID
		, [SewInLineDate] = MD.SewInLineDate
		, [SewOffLineDate] = MD.SewOffLineDate
		, [MinDate] = MD.MinDate
		, [MaxDate] = MD.MaxDate
		, [SewingLineID] = SS.SewingLineID
		, [InlineDate] = Cast(SS.Inline as date)
		, [OfflineDate] = Cast(SS.Offline as date)
		, [ComboType] = SS.ComboType
		, [InlineHour] = DATEDIFF(SS,Cast(SS.Inline as date),SS.Inline) / 3600.0
		, [OfflineHour] = DATEDIFF(SS,Cast(SS.Offline as date),SS.Offline) / 3600.0
		, [HourOutput] = iif(isnull(SS.TotalSewingTime,0)=0,0,(SS.Sewer * 3600.0 * ScheduleEff.val / 100) / SS.TotalSewingTime)
		, [OriWorkHour] = iif (SS.Sewer = 0 or isnull(SS.TotalSewingTime,0)=0, 0, sum(SS.AlloQty) / ((SS.Sewer * 3600.0 * ScheduleEff.val / 100) / SS.TotalSewingTime))
		, [LearnCurveID] = SS.LearnCurveID
		, [LNCSERIALNumber] = SS.LNCSERIALNumber
		, [SwitchTime] = SS.SwitchTime
		, [Sewer] = SS.Sewer
		, [AlloQty] = SS.AlloQty
		FROM StdQ_MainDates MD WITH(NOLOCK)
		INNER JOIN SewingSchedule SS WITH(NOLOCK) ON MD.FactoryID = SS.FactoryID AND MD.OrderID = SS.OrderID AND MD.MDivisionID = SS.MDivisionID
		OUTER APPLY(SELECT  [val] = iif(SS.OriEff IS NULL AND SS.SewLineEff IS NULL,SS.MaxEff, isnull(SS.OriEff,100) * isnull(SS.SewLineEff,100) / 100) ) ScheduleEff 

		GROUP BY MD.MDivisionID, MD.FactoryID, MD.OrderID, MD.[SewInLineDate], MD.[SewOffLineDate], SS.SewingLineID,SS.Inline,SS.Offline,MD.[MaxDate], MD.[MinDate]
				,SS.TotalSewingTime,SS.Sewer,ScheduleEff.val,SS.TotalSewingTime,SS.ComboType,SS.APSNo, SS.LearnCurveID, SS.LNCSERIALNumber, SS.SwitchTime,SS.AlloQty
	), StdQ_WorkDate AS(
			SELECT f.FactoryID,cast(DATEADD(DAY,number,(select CAST(min([SewInLineDate])AS date) from StdQ_MainDates)) as datetime) [WorkDate]
		FROM master..spt_values s
		cross join (select distinct FactoryID from StdQ_MainDates) f
		WHERE s.type = 'P'
		AND DATEADD(DAY,number,(select CAST(min(MinDate)AS date) from StdQ_MainDates)) <= (select cast(max(MaxDate)as Date) from StdQ_MainDates)
	), StdQ_DateList AS(
		SELECT
		  [APSNo] = sdr.APSNo
		, [MDivisionID] = sdr.MDivisionID
		, [FactoryID] = sdr.FactoryID
		, [OrderID] = OrderID
		, [WorkDate] = cast( swd.WorkDate as datetime)
		, [SewingLineID] = sdr.SewingLineID
		, [Sewer]
		, [HourOutput]
		, [OriWorkHour]
		, [LearnCurveID]
		, [LNCSERIALNumber]
		, [ComboType]
		, [SwitchTime]
		, [StartHour] = iif( WorkDate = InlineDate and  ROW_NUMBER() OVER (PARTITION BY APSNo,WorkDate,OrderID,ComboType ORDER BY StartHour) = 1 and InlineHour > StartHour ,InlineHour ,cast(wkd.StartHour as float))
		, [EndHour] = iif(WorkDate = OfflineDate and ROW_NUMBER() OVER (PARTITION BY APSNo,WorkDate,OrderID,ComboType ORDER BY EndHour desc) = 1 and OfflineHour < EndHour,OfflineHour, cast(wkd.EndHour as float))
		, [StartHourSort] = ROW_NUMBER() OVER (PARTITION BY APSNo,WorkDate,OrderID,ComboType ORDER BY StartHour)
		, [EndHourSort] = ROW_NUMBER() OVER (PARTITION BY APSNo,WorkDate,OrderID,ComboType ORDER BY EndHour desc)
		FROM StdQ_DateRange sdr
		INNER JOIN StdQ_WorkDate swd on swd.WorkDate >= sdr.InlineDate and swd.WorkDate <= sdr.OfflineDate and swd.FactoryID = sdr.FactoryID
		INNER JOIN Workhour_Detail wkd with (nolock) on wkd.FactoryID = sdr.FactoryID and 
														wkd.SewingLineID = sdr.SewingLineID and 
														wkd.Date = swd.WorkDate
		WHERE NOT ((swd.WorkDate = sdr.InlineDate AND wkd.EndHour <= sdr.InlineHour) OR  (swd.WorkDate = sdr.OfflineDate AND wkd.StartHour >= sdr.OfflineHour)) 
	), StdQ_WorkDataList AS (
		SELECT
		  [APSNo] --
		, [MDivisionID]
		, [FactoryID]
		, [SewingLineID]
		, [Sewer] --
		, [OrderID]
		, [WorkDate]--
		, [HourOutput]--
		, [OriWorkHour]--
		, [ComboType]
		, [LearnCurveID] --
		, [LNCSERIALNumber] --
		, [SwitchTime] --
		, [SewingStart] = DATEADD(mi, min(StartHour) * 60,   WorkDate)
		, [SewingEnd] = DATEADD(mi, max(EndHour) * 60,   WorkDate)
		, [Work_Minute] = sum(EndHour - StartHour) * 60
		, [WorkingTime] = sum(EndHour - StartHour)
		, [OriWorkDateSer] = ROW_NUMBER() OVER (PARTITION BY APSNo,orderID,ComboType ORDER BY WorkDate)
		from StdQ_DateList
		group by APSNo,LearnCurveID,WorkDate,HourOutput,OriWorkHour,Sewer,LNCSERIALNumber,ComboType,SwitchTime,OrderID, [SewingLineID], [MDivisionID], [FactoryID]
	), StdQ_WorkDataList_Step1 AS(
		SELECT
		  [APSNo] --
		, [MDivisionID]
		, [FactoryID]
		, [SewingLineID]
		, [ComboType]
		, [Sewer] --
		, [OrderID]
		, [WorkDate]--
		, [HourOutput]--
		, [OriWorkHour]--
		, [LearnCurveID] --
		, [LNCSERIALNumber] --
		, [SwitchTime] --
		, [SewingStart]
		, [SewingEnd]
		, [Work_Minute]
		, [WorkingTime]
		, [OriWorkDateSer] 
		, [Sum_Work_Minute] = sum(Work_Minute) over(partition by APSNo order by SewingStart)
		FROM StdQ_WorkDataList
	), StdQ_WorkDataList_Step2 AS (
		select 
		  [APSNo] --
		, [MDivisionID]
		, [FactoryID]
		, [SewingLineID]
		, [ComboType]
		, [Sewer] --
		, [OrderID]
		, [WorkDate]--
		, [HourOutput]--
		, [OriWorkHour]--
		, [LearnCurveID] --
		, [LNCSERIALNumber] --
		, [SwitchTime] --
		, [SewingStart]
		, [SewingEnd]
		, [Work_Minute]
		, [WorkingTime]
		, [OriWorkDateSer] 
		, [Sum_Work_Minute]
		, [New_SwitchTime] = case when (SwitchTime - LAG(Sum_Work_Minute,1,0) over (partition by APSNo order by sewingstart) <= 0) then 0
			else SwitchTime - LAG(Sum_Work_Minute,1,0) over (partition by APSNo order by sewingstart)
			end
		from StdQ_WorkDataList_Step1
	), StdQ_WorkDataList_Step3 AS (
		select 
		  [APSNo] --
		, [MDivisionID]
		, [FactoryID]
		, [SewingLineID]
		, [ComboType]
		, [Sewer] --
		, [WorkDate]--
		, [OrderID]
		, [HourOutput]--
		, [OriWorkHour]--
		, [LearnCurveID] --
		, [LNCSERIALNumber] --
		, [SwitchTime] --
		, [SewingStart]
		, [SewingEnd]
		, [Work_Minute]
		, [WorkingTime]
		, [OriWorkDateSer] 
		, [Sum_Work_Minute]
		, [New_SwitchTime] 
		, [New_Work_Minute] = 
			case when Work_Minute = Sum_Work_Minute and Work_Minute - SwitchTime > 0 then Work_Minute - SwitchTime
				 when Work_Minute != Sum_Work_Minute and Sum_Work_Minute - Work_Minute > SwitchTime then Work_Minute
				 when LAG(New_SwitchTime,1,0) OVER (PARTITION BY APSNo ORDER BY SewingStart) !=0 
					and Work_Minute - LAG(New_SwitchTime,1,0) OVER (PARTITION BY APSNo ORDER BY SewingStart) > 0 
					then Work_Minute - LAG(New_SwitchTime,1,0) OVER (PARTITION BY APSNo ORDER BY SewingStart)
				 else 0
				 end
		from StdQ_WorkDataList_Step2
	), StdQ_WorkDataList_Step4 AS (
		select 
		  [APSNo] --
		, [MDivisionID]
		, [FactoryID]
		, [SewingLineID]
		, [ComboType]
		, [Sewer] --
		, [OrderID]
		, [WorkDate]--
		, [HourOutput]--
		, [OriWorkHour]--
		, [LearnCurveID] --
		, [LNCSERIALNumber] --
		, [SwitchTime] --
		, [SewingStart]
		, [SewingEnd]
		, [Work_Minute]
		, [WorkingTime]
		, [OriWorkDateSer] 
		, [Sum_Work_Minute]
		, [New_Work_Minute]
		, [New_WorkingTime] = IIF(SwitchTime = 0, WorkingTime,  CONVERT(float, New_Work_Minute)/60) 
		, [New_SwitchTime]  = IIF(SwitchTime = 0, 0 , CONVERT(float, New_SwitchTime)/60) 
		, [WorkDateSer] = case	when isnull(LNCSERIALNumber,0) = 0 then OriWorkDateSer
							when LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) <= 0 then OriWorkDateSer
							else OriWorkDateSer + LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) end
		from StdQ_WorkDataList_Step3
	), StdQ_OriTotalWorkHour AS (
		SELECT wds4.APSNo,wds4.WorkDate,[TotalWorkHour] = sum(OriWorkHour)
		FROM StdQ_WorkDataList_Step4 wds4
		group by wds4.APSNo,wds4.WorkDate
	),Stdq_Finsh AS (
		select 
		 [MDivisionID]
		, [FactoryID]
		, [ComboType]
		, awd.[OrderID]
		, awd.SewingLineID
		, [Date] = cast(awd.SewingStart as date)
		, StdQty = round(sum(iif (isnull (otw.TotalWorkHour, 0) = 0, 0, awd.New_WorkingTime * awd.HourOutput * awd.OriWorkHour / otw.TotalWorkHour))
				* ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0,0)
		from StdQ_WorkDataList_Step4 awd
		inner join StdQ_OriTotalWorkHour otw on otw.APSNo = awd.APSNo and otw.WorkDate = awd.WorkDate
		left join LearnCurve_Detail lcd with (nolock) on awd.LearnCurveID = lcd.ID and awd.WorkDateSer = lcd.Day
		outer apply(select top 1 [val] = Efficiency from LearnCurve_Detail where ID = awd.LearnCurveID order by Day desc ) LastEff
		group by awd.SewingStart,awd.SewingEnd,awd.WorkingTime,ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0)),awd.Sewer, awd.SewingLineID, awd.[OrderID], [MDivisionID], [FactoryID], [ComboType]
	),Stdq_Finsh_GroupBy AS(
		SELECT 
		[MDivisionID]
		, [FactoryID]
		, [ComboType]
		, [OrderID]
		, [SewingLineID]
		, [Date]
		, [StdQty] = isnull(SUM(STDQTY),0)
		FROM Stdq_Finsh 
		GROUP BY [MDivisionID], [FactoryID], [ComboType], [OrderID], [SewingLineID], [Date]
		HAVING ISNULL(SUM(STDQTY), 0) <> 0
	),Stdq_Finsh_Step1 AS (
		SELECT
		 [OrderID]
		,[Request date] = DATE
		,[COMBOTYPE] = ComboType
		,[SewingLineID] = SewingLineID
		,[STD QTY] = STDQTY
		,[PREVIOUS STD QTY] = LAG(STDQTY) OVER (PARTITION BY SewingLineID, COMBOTYPE ORDER BY DATE)
		FROM Stdq_Finsh_GroupBy
	), Stdq_Finsh_Step2 AS (

	  SELECT
		[OrderID],
		[Request date],
		[SewingLineID],
		[COMBOTYPE],
		[DAILY TOTAL],
		ROW_NUMBER() OVER (PARTITION BY [OrderID],[Request date], [SewingLineID] ORDER BY [DAILY TOTAL] DESC) AS RowNum
	  FROM (
		SELECT
		  [OrderID]
		  ,[Request date]
		  ,[SewingLineID]
		  ,[COMBOTYPE]
		  ,[DAILY TOTAL] = SUM([STD QTY]) OVER (PARTITION BY [OrderID],SewingLineID, COMBOTYPE ORDER BY [Request date])
		FROM Stdq_Finsh_Step1
		GROUP BY [OrderID], [Request date], [SewingLineID], [COMBOTYPE],[STD QTY]
	  ) AS Subquery
	), Stdq_Finsh_Step3 AS (
		SELECT
		[OrderID] = SFS2.OrderID
		,[Request date] = CONVERT(NVARCHAR,  SFS1.[Request date], 23)
		,[SewingLineID] = SFS1. SewingLineID
		,[COMBOTYPE] = SFS1.COMBOTYPE
		,[ACCU. STDQTY] = SUM(SFS1.[STD QTY]) OVER (PARTITION BY SFS1.OrderID, SFS1.COMBOTYPE ORDER BY SFS1.[Request date])
		--,[STD QTY BY LINE] = SFS1.[STD QTY]
		,[ACCU. STD QTY BY LINE] = MAX([DAILY TOTAL]) OVER (PARTITION BY SFS1.OrderID,SFS1.SewingLineID, SFS1.COMBOTYPE ORDER BY SFS1.[Request date])
		FROM Stdq_Finsh_Step1 SFS1 WITH(NOLOCK)
		JOIN Stdq_Finsh_Step2 SFS2 WITH(NOLOCK) ON SFS1.OrderID = SFS2.OrderID  AND SFS1.[Request date] = SFS2.[Request date] AND SFS1.SewingLineID = SFS2.SewingLineID AND SFS1.COMBOTYPE = SFS2.COMBOTYPE and SFS2.RowNum = 1
	), Stdq_Finsh_Step4 AS (
		select 
		[OrderID] 
		,[Request date] 
		,[SewingLineID]
		,[COMBOTYPE]
		,[ACCU. STDQTY] = case when  [ACCU. STDQTY] = LAG([ACCU. STDQTY]) OVER (ORDER BY [Request date]) 
							   THEN LAG([ACCU. STDQTY]) OVER (ORDER BY [Request date]) 
							   ELSE [ACCU. STDQTY] END
		,[STD QTY BY LINE] = [ACCU. STD QTY BY LINE] - ISNULL(LAG([ACCU. STD QTY BY LINE]) OVER (PARTITION BY OrderID, [SewingLineID] ORDER BY [Request date]),0) 
		,[ACCU. STD QTY BY LINE]
		--,[RowNum] = ROW_NUMBER() OVER (PARTITION BY OrderID,[Request date], [SewingLineID] ORDER BY[Request date]ASC , [ACCU. STD QTY BY LINE] DESC)
		FROM Stdq_Finsh_Step3

	), Stdq_Finsh_Step5 AS (
		SELECT
		 [OrderID] = a.[OrderID] 
		,[Request date]  = a.[Request date] 
		,[SewingLineID] = a.[SewingLineID]
		,[COMBOTYPE] = a.[COMBOTYPE]
		,[STDQTY] =  MAX(b.[ACCU. STDQTY]) - isnull(LAG(MAX(b.[ACCU. STDQTY])) OVER (PARTITION BY a.OrderID ORDER BY a.OrderID, a.[Request date]),0) 
		,[ACCU. STDQTY] = MAX(a.[ACCU. STDQTY]) OVER (PARTITION BY a.OrderID ORDER BY a.[Request date])
		,[STD QTY BY LINE] =a.[STD QTY BY LINE]
		,[ACCU. STD QTY BY LINE] = a.[ACCU. STD QTY BY LINE]
		From Stdq_Finsh_Step4 a
		inner join Stdq_Finsh_Step4 b on a.[Request date] = b.[Request date]
		--where a.RowNum = 1
		GROUP BY a.OrderID,a.[SewingLineID],a.[Request date],a.[STD QTY BY LINE],a.[ACCU. STD QTY BY LINE],a.[COMBOTYPE],a.[ACCU. STDQTY]
	
	),Stdq_Finsh_Step6 AS(
		SELECT
		[OrderID]
		,[Request date]
		,[SewingLineID]
		,[COMBOTYPE]
		,[STDQTY] = CASE
						WHEN [Request date] = LAG([Request date]) OVER (PARTITION BY OrderID ORDER BY [Request date])
						THEN LAG([STDQTY]) OVER (PARTITION BY OrderID  ORDER BY [Request date])
						WHEN [ACCU. STDQTY] = LAG([ACCU. STDQTY]) OVER (PARTITION BY OrderID ORDER BY [Request date])
						THEN 0
						ELSE [ACCU. STDQTY] - isnull(LAG([ACCU. STDQTY]) OVER (PARTITION BY OrderID ORDER BY [Request date]),0)
					END
		,[ACCU. STDQTY]
		,[STD QTY BY LINE]
		,[ACCU. STD QTY BY LINE]
		--,[RowNum] = ROW_NUMBER() OVER (PARTITION BY OrderID,[Request date], [SewingLineID] ORDER BY [ACCU. STD QTY BY LINE] DESC)
		FROM Stdq_Finsh_Step5
	),EstCutQty_Step1 As (
		select distinct
		wd.OrderID,
		POID = w.ID,
		wd.Article,
		wd.SizeCode,
		wp.PatternPanel,
		w.FabricCombo,
		w.FabricCode,
		wp.FabricPanelCode,
		w.Colorid
		,w.EstCutDate
		,[CutQty] = CutQty.val
		from WorkOrder w with(nolock)
		inner join WorkOrder_Distribute wd WITH(NOLOCK) on wd.WorkOrderUkey = w.Ukey
		inner join WorkOrder_PatternPanel wp WITH(NOLOCK) on wp.WorkOrderUkey = w.Ukey
		/* WorkOrder_Distribute.SizeCode與SewingSchedule_Detail.Size WorkOrder_Distribute.Article與SewingSchedule_Detail.Article對照*/
		inner join SewingSchedule_Detail ssd WITH(NOLOCK) on ssd.OrderID = wd.OrderID and ssd.Article = wd.Article and ssd.SizeCode = wd.SizeCode
		left join StdQ_MainDates main WITH(NOLOCK) on main.OrderID = wd.OrderID
		/****************************************************************************************************************************/
		outer apply
		(
			Select  val = c.qty * w.layer
			From WorkOrder_SizeRatio c
			Where  c.WorkOrderUkey =w.Ukey 
		) as CutQty
		where wd.OrderID = main.OrderID   
		and exists (select 1 from Orders o WITH(NOLOCK)  where wd.OrderID  = o.ID and o.LocalOrder = 0) --非local單 
		and not exists(select 1 from Cutting_WIPExcludePatternPanel cw WITH(NOLOCK) where cw.ID = w.ID and cw.PatternPanel = wp.PatternPanel) -- ISP20201886 排除指定 PatternPanel
		and (((select WIP_ByShell from system) = 1 and exists(select 1 from Order_BOF bof WITH(NOLOCK) where bof.Id = w.Id and bof.FabricCode = w.FabricCode and bof.Kind = 1)) or (select WIP_ByShell from system) = 0)
	)
	,EstCutQty_Step2 AS(
		select x.*,x2.PatternPanel,x2.FabricCombo,FabricPanelCode
		from (select distinct Orderid,POID,Article,SizeCode,FabricCode,ColorID,EstCutDate,[CutQty] from EstCutQty_Step1) x
		outer apply(select top 1 * from EstCutQty_Step1 where Orderid = x.Orderid and POID = x.POID and Article = x.Article and SizeCode = x.SizeCode and FabricCode = x.FabricCode and ColorID = x.ColorID order by FabricPanelCode)x2
	),EstCutWorkDate AS(
		SELECT DISTINCT [Date]
		FROM Workhour WH
		INNER JOIN StdQ_MainDates SM on WH.FactoryID = SM.FactoryGroup 
		where Holiday = 0 
		and not exists(select 1 from holiday where holiday.HolidayDate =WH.Date)
	),EstCutQty_Step3 AS(
		SELECT 
		  [OrderID] =ECQS2.Orderid
		, [POID]
		, [Article]
		, [SizeCode]
		, [EstCutDate]
		, [FabricCombo]
		, [CutQty]
		FROM EstCutQty_Step2 ECQS2 
		LEFT JOIN StdQ_MainDates SM  on SM.OrderID = ECQS2.OrderID
		left JOIN EstCutWorkDate ECWD on ECWD.[Date] = ECQS2.[EstCutDate]
	), EstCutCount AS (
		SELECT 
		OrderID 
		, Article
		,[Count] = COUNT(DISTINCT [FabricCombo])
		FROM EstCutQty_Step3 
		GROUP by OrderID , Article
	) ,EstCutSum as(
		SELECT
		a.OrderID,Article,SizeCode,
		[val] = COUNT(DISTINCT [FabricCombo])  
		FROM EstCutQty_Step2 a
		left JOIN StdQ_MainDates SM on a.orderID = SM.orderid
		Where 
		a.EstCutDate <= DATEADD(DAY, -2, SM.maxdate)
		GROUP BY a.OrderID,Article,SizeCode
	)
	,EstCutGroup AS (
		SELECT 
		[OrderID] = c.OrderID
		,[Count]
		,[SizeCode] = c.SizeCode
		,[CutQty] =  MIN(C.[CutQty])
		FROM 
		(
			SELECT
			t.OrderID
			,t.Article
			,t.SizeCode
			,t.EstCutDate
			,t.FabricCombo
			, [CutQty] = Sum(CutQty)
			, [Count] = e.[val]
			FROM EstCutQty_Step2 t
			--left JOIN StdQ_MainDates sfs6 on t.orderID = sfs6.orderid
			left join estcutsum e on e.OrderID = t. orderid AND e.Article = t.article AND e.SizeCode = t.sizecode
			WHERE EXISTS (SELECT 1 FROM StdQ_MainDates sfs6 WHERE t.OrderID = sfs6.OrderID)
			GROUP BY t.OrderID,t.Article,t.SizeCode,EStCutDate,FabricCombo,e.[val]
		) c
		inner join EstCutQty_Step2 ecqs2 on ecqs2.orderid = c.orderid
		GROUP BY c.SizeCode ,c.OrderID,c.[Count]
	), EstCutALLGroup AS(
		SELECT
		e.orderid
		,[AccuEstCutQty] = SUM(ec.[CutQty])
		FROM  EstCutCount e 
		left JOIN EstCutGroup  ec on ec.OrderID = e.OrderID and ec.[Count] = e.[Count] 
		 group by e.orderid
	),EstCutQty_Step4 AS(
	-------------------------------
		SELECT
		sfs6.[OrderID],
		sfs6.[Request date],
		sfs6.[SewingLineID],
		sfs6.[STDQTY],
		sfs6.[ACCU. STDQTY],
		sfs6.[STD QTY BY LINE],
		sfs6.[ACCU. STD QTY BY LINE],
		eag.[AccuEstCutQty]
		FROM Stdq_Finsh_Step6 sfs6
		LEFT join EstCutALLGroup eag on eag.orderid = sfs6.[OrderID]
	), EstCutQty_Step5 AS (
		SELECT
		[ROWNumber] = ROW_NUMBER() OVER (PARTITION BY OrderID, SewingLineID ORDER BY [Request Date])
		,*
		,[STDQTY BY SUM] = SUM([STD QTY BY LINE]) OVER(PARTITION BY OrderID,SewingLineID ORDER BY [Request Date])
		FROM EstCutQty_Step4
	), EstCutQty_Step6 AS (
		SELECT 
		* ,
		CASE
			WHEN AccuEstCutQty - [STDQTY BY SUM]  >= 0 THEN [STD QTY BY LINE]
			ELSE AccuEstCutQty - LAG([STDQTY BY SUM]) OVER (PARTITION BY OrderID,SewingLineID ORDER BY [Request date])
		END AS AccEstCutQtyByLine
		from EstCutQty_Step5
	), EstCutQty_Step7 AS(
		SELECT 
		 [MDivisionID] = MAIN.MDivisionID
		,[FactoryID] = MAIN.FactoryID
		,[BrandID] = O.BrandID
		,[StyleID] = O.StyleID
		,[SeasonID] = O.SeasonID
		,[CDCodeNew] = S.CDCodeNew
		,[FabricType] = S.FabricType
		,[POID] = O.POID
		,[Category] = DDL.[Name]
		,[WorkType] = CASE when c.WorkType = 1 THEN 'Combination'
						   when c.workType = 2 THEN 'By SP#'
						   ELSE ''
					  END
		,[MatchFabric] = MarkerInfo.MatchFabric + char(10) + MarkerInfo.OneTwoWay + ' ' + MarkerInfo.HorizontalCutting
		,[OrderID] = O.ID
		,[SciDelivery] = O.SciDelivery
		,[BuyerDelivery] = O.BuyerDelivery
		,[OrderQty] = O.Qty
		,[SewInLineDate] = MAIN.[SewInLineDate]
		,[SewOffLineDate] = MAIN.[SewOffLineDate]
		,[SewingLineID] = ECQE.SewingLineID
		,[RequestDate] = ECQE.[Request Date]
		,[StdQty] = ECQE.STDQTY
		,[StdQtyByLine] = ECQE.[STD QTY BY LINE]
		,[AccuStdQty] = ECQE.[ACCU. STDQTY]
		,[AccuStdQtyByLine] = ECQE.[ACCU. STD QTY BY LINE]
		,[AccuEstCutQty] = ECQE.AccuEstCutQty
		,[AccuEstCutQtyByLine] = ECQE.AccEstCutQtyByLine
		,[SupplyCutQty] = ECQE.AccuEstCutQty - ECQE.[ACCU. STDQTY]
		,[SupplyCutQtyByLine] = ECQE.AccEstCutQtyByLine - ECQE.[ACCU. STD QTY BY LINE]
		FROM EstCutQty_Step6 ECQE
		INNER JOIN StdQ_MainDates MAIN WITH(NOLOCK) on ECQE.OrderID = MAIN.OrderID
		INNER JOIN Orders O WITH(NOLOCK) ON MAIN.OrderID = O.ID
		left join Cutting c WITH (NOLOCK) on c.ID = o.CuttingSP
		INNER JOIN Style S WITH(NOLOCK) ON O.StyleID = S.ID AND o.BrandID = s.BrandID and o.SeasonID = s.SeasonID
		INNER JOIN DropDownList DDL WITH(NOLOCK) on DDL.ID = o.Category and DDL.[Type] = 'Category'
		OUTER APPLY(
			select  top 1 [MatchFabric] = ( case ml.MatchFabric
											when '1' then concat('Body Mapping:V-Repeat ',IIF(ml.V_Repeat is null,'',CAST(ml.V_Repeat AS VARCHAR)))
											when '2' then concat('Checker:V-Repeat ',IIF(ml.V_Repeat is null,'',CAST(ml.V_Repeat AS VARCHAR)),' Checker:H-Repeat '+IIF(ml.H_Repeat is null,'',CAST(ml.H_Repeat AS VARCHAR)))
											when '3' then concat('Horizontal stripe:V-Repeat ',IIF(ml.V_Repeat is null,'',CAST(ml.V_Repeat AS VARCHAR)))
											when '4' then concat('Straight stripe:H-Repeat ',IIF(ml.H_Repeat is null,'',CAST(ml.H_Repeat AS VARCHAR)))
											else '' end),
							[OneTwoWay] = IIF(ml.OneTwoWay=1, 'one way cutting', ''),
							[HorizontalCutting] = IIF(ml.HorizontalCutting =1, 'Straight fabric use Horizontal cutting', '')
			from    Marker m with (nolock)
			INNER JOIN WorkOrder wo on wo.OrderID = o.POID
			inner join Marker_ML ml with (nolock) on ml.ID = m.ID and m.Version = ml.Version and ml.MarkerName = wo.Markername
			inner join DropDownList d on ml.MatchFabric = d.ID and d.type = 'MatchFabric'
			where   wo.MarkerVersion = m.Version and wo.MarkerNo = m.MarkerNo 
			order by m.EditDate desc
		) MarkerInfo
	), EstCutQty_Step8 AS(
		SELECT 
		*
		,[BalanceCutQty] = SupplyCutQty - StdQty
		,[BalanceCutQtyByLine] = SupplyCutQtyByLine - StdQtyByLine
		FROM EstCutQty_Step7
	), EstCutQty_END AS(
		SELECT 
		* 
		,[SupplyCutQtyVSStdQty] = IIF (BalanceCutQty < 0, SupplyCutQty, StdQty)
		,[SupplyCutQtyVSStdQtyByLine] = IIF (BalanceCutQtyByLine < 0, SupplyCutQtyByLine, StdQtyByLine)

		FROM EstCutQty_Step8
	)

	SELECT * FROM EstCutQty_END;
END

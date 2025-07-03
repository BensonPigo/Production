using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_CuttingBCS
    {
        /// <inheritdoc/>
        public P_Import_CuttingBCS()
        {
            DBProxy.Current.DefaultTimeout = 2700;
        }

        /// <inheritdoc/>
        public Base_ViewModel P_CuttingBCS(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-15).ToString("yyyy/MM/dd"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Parse(DateTime.Now.AddDays(15).ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.GetCuttingBCS_Data(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetCuttingBCS_Data(ExecutedList item)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@StartDate", item.SDate),
                new SqlParameter("@EndDate", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sql = @"
            -- print concat(format(getdate(), 'HH:mm:ss'), ' #tmp_StdQ_MainDates')
			SELECT 
				  [MDivisionID] = s.MDivisionID
				, [FactoryID] = s.FactoryID
				, [FactoryGroup] = o.FtyGroup
				, [OrderID]
				, [SewInLineDate] = CONVERT(DATE, MIN(CONVERT(DATE, Inline))) 
				, [SewOffLineDate] = CONVERT(DATE, MIN(CONVERT(DATE, offline)))
				, [MinDate] = CONVERT(DATE, MIN(CONVERT(DATE, Inline)))
				, [MaxDate] = CONVERT(DATE, Max(CONVERT(DATE, offline)))
			into #tmp_StdQ_MainDates
			FROM SewingSchedule s WITH(NOLOCK)
			inner join Orders o WITH(NOLOCK) on o.id = s.OrderID and o.FtyGroup = s.FactoryID
			WHERE		
			(
				([Offline] BETWEEN @StartDate AND GETDATE() OR [Inline] BETWEEN GETDATE() AND @EndDate)
				OR 
				(
					S.AddDate > DATEADD(DAY, -3, GETDATE()) OR S.EditDate > DATEADD(DAY, -3, GETDATE())
				)
			)
			AND s.BIPImportCuttingBCSCmdTime IS NULL
			AND EXISTS(select 1 from Factory f WITH(NOLOCK) where o.FactoryID = f.ID and f.IsSampleRoom = 0)
			GROUP BY s.FactoryID, OrderID, s.MDivisionID,o.FtyGroup

			-- print concat(format(getdate(), 'HH:mm:ss'), ' #tmp_StdQ_DateRange')
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
			into #tmp_StdQ_DateRange
			FROM #tmp_StdQ_MainDates MD WITH(NOLOCK)
			INNER JOIN SewingSchedule SS WITH(NOLOCK) ON MD.FactoryID = SS.FactoryID AND MD.OrderID = SS.OrderID AND MD.MDivisionID = SS.MDivisionID
			OUTER APPLY(SELECT  [val] = iif(SS.OriEff IS NULL AND SS.SewLineEff IS NULL,SS.MaxEff, isnull(SS.OriEff,100) * isnull(SS.SewLineEff,100) / 100) ) ScheduleEff 

			GROUP BY MD.MDivisionID, MD.FactoryID, MD.OrderID, MD.[SewInLineDate], MD.[SewOffLineDate], SS.SewingLineID,SS.Inline,SS.Offline,MD.[MaxDate], MD.[MinDate]
					,SS.TotalSewingTime,SS.Sewer,ScheduleEff.val,SS.TotalSewingTime,SS.ComboType,SS.APSNo, SS.LearnCurveID, SS.LNCSERIALNumber, SS.SwitchTime,SS.AlloQty

			-- print concat(format(getdate(), 'HH:mm:ss'), ' #tmp_StdQ_WorkDate')
			SELECT f.FactoryID,cast(DATEADD(DAY,number,(select CAST(min([SewInLineDate])AS date) from #tmp_StdQ_MainDates)) as datetime) [WorkDate]
			into #tmp_StdQ_WorkDate
			FROM master..spt_values s
			cross join (select distinct FactoryID from #tmp_StdQ_MainDates) f
			WHERE s.type = 'P'
			AND DATEADD(DAY,number,(select CAST(min(MinDate)AS date) from #tmp_StdQ_MainDates)) <= (select cast(max(MaxDate)as Date) from #tmp_StdQ_MainDates)

			-- print concat(format(getdate(), 'HH:mm:ss'), ' #tmp_StdQ_DateList')
			SELECT [APSNo] = sdr.APSNo
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
			into #tmp_StdQ_DateList
			FROM #tmp_StdQ_DateRange sdr
			INNER JOIN #tmp_StdQ_WorkDate swd on swd.WorkDate >= sdr.InlineDate and swd.WorkDate <= sdr.OfflineDate and swd.FactoryID = sdr.FactoryID
			INNER JOIN Workhour_Detail wkd with (nolock) on wkd.FactoryID = sdr.FactoryID and 
															wkd.SewingLineID = sdr.SewingLineID and 
															wkd.Date = swd.WorkDate
			WHERE NOT ((swd.WorkDate = sdr.InlineDate AND wkd.EndHour <= sdr.InlineHour) OR  (swd.WorkDate = sdr.OfflineDate AND wkd.StartHour >= sdr.OfflineHour)) 

			-- print concat(format(getdate(), 'HH:mm:ss'), ' #tmp_StdQ_WorkDataList')
			SELECT [APSNo] --
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
			into #tmp_StdQ_WorkDataList
			from #tmp_StdQ_DateList
			group by APSNo,LearnCurveID,WorkDate,HourOutput,OriWorkHour,Sewer,LNCSERIALNumber,ComboType,SwitchTime,OrderID, [SewingLineID], [MDivisionID], [FactoryID]

			-- print concat(format(getdate(), 'HH:mm:ss'), ' #tmp_StdQ_WorkDataList_Step1')
			SELECT [APSNo] --
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
			into #tmp_StdQ_WorkDataList_Step1
			FROM #tmp_StdQ_WorkDataList

			-- print concat(format(getdate(), 'HH:mm:ss'), ' #tmp_StdQ_WorkDataList_Step2')
			select [APSNo] --
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
			into #tmp_StdQ_WorkDataList_Step2
			from #tmp_StdQ_WorkDataList_Step1

			-- print concat(format(getdate(), 'HH:mm:ss'), ' StdQ_WorkDataList_Step3')
			select [APSNo] --
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
			into #tmp_StdQ_WorkDataList_Step3
			from #tmp_StdQ_WorkDataList_Step2

			-- print concat(format(getdate(), 'HH:mm:ss'), ' StdQ_WorkDataList_Step4')
			select [APSNo] --
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
			into #tmp_StdQ_WorkDataList_Step4
			from #tmp_StdQ_WorkDataList_Step3

			-- print concat(format(getdate(), 'HH:mm:ss'), ' StdQ_OriTotalWorkHour')
			SELECT wds4.APSNo,wds4.WorkDate,[TotalWorkHour] = sum(OriWorkHour)
			into #tmp_StdQ_OriTotalWorkHour
			FROM #tmp_StdQ_WorkDataList_Step4 wds4
			group by wds4.APSNo,wds4.WorkDate

			-- print concat(format(getdate(), 'HH:mm:ss'), ' Stdq_Finsh')
			select [MDivisionID]
			, [FactoryID]
			, [ComboType]
			, awd.[OrderID]
			, awd.SewingLineID
			, [Date] = cast(awd.SewingStart as date)
			, StdQty = round(sum(iif (isnull (otw.TotalWorkHour, 0) = 0, 0, awd.New_WorkingTime * awd.HourOutput * awd.OriWorkHour / otw.TotalWorkHour))
					* ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0,0)
			into #tmp_Stdq_Finsh
			from #tmp_StdQ_WorkDataList_Step4 awd
			inner join #tmp_StdQ_OriTotalWorkHour otw on otw.APSNo = awd.APSNo and otw.WorkDate = awd.WorkDate
			left join LearnCurve_Detail lcd with (nolock) on awd.LearnCurveID = lcd.ID and awd.WorkDateSer = lcd.Day
			outer apply(select top 1 [val] = Efficiency from LearnCurve_Detail where ID = awd.LearnCurveID order by Day desc ) LastEff
			group by awd.SewingStart,awd.SewingEnd,awd.WorkingTime,ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0)),awd.Sewer, awd.SewingLineID, awd.[OrderID], [MDivisionID], [FactoryID], [ComboType]

			-- print concat(format(getdate(), 'HH:mm:ss'), ' Stdq_Finsh_GroupBy')
			SELECT [MDivisionID]
				, [FactoryID]
				, [ComboType]
				, [OrderID]
				, [SewingLineID]
				, [Date]
				, [StdQty] = isnull(SUM(STDQTY),0)
			into #tmp_Stdq_Finsh_GroupBy
			FROM #tmp_Stdq_Finsh 
			GROUP BY [MDivisionID], [FactoryID], [ComboType], [OrderID], [SewingLineID], [Date]
			HAVING ISNULL(SUM(STDQTY), 0) <> 0

			-- print concat(format(getdate(), 'HH:mm:ss'), ' Stdq_Finsh_Step1')
			SELECT [OrderID]
				,[Request date] = DATE
				,[COMBOTYPE] = ComboType
				,[SewingLineID] = SewingLineID
				,[STD QTY] = STDQTY
				,[PREVIOUS STD QTY] = LAG(STDQTY) OVER (PARTITION BY SewingLineID, COMBOTYPE ORDER BY DATE)
			into #tmp_Stdq_Finsh_Step1
			FROM #tmp_Stdq_Finsh_GroupBy

			-- print concat(format(getdate(), 'HH:mm:ss'), ' Stdq_Finsh_Step2')
			SELECT [OrderID],
			[Request date],
			[SewingLineID],
			[COMBOTYPE],
			[DAILY TOTAL],
			ROW_NUMBER() OVER (PARTITION BY [OrderID],[Request date], [SewingLineID] ORDER BY [DAILY TOTAL] DESC) AS RowNum
			into #tmp_Stdq_Finsh_Step2
			FROM (
				SELECT
					[OrderID]
					,[Request date]
					,[SewingLineID]
					,[COMBOTYPE]
					,[DAILY TOTAL] = SUM([STD QTY]) OVER (PARTITION BY [OrderID],SewingLineID, COMBOTYPE ORDER BY [Request date])
				FROM #tmp_Stdq_Finsh_Step1
				GROUP BY [OrderID], [Request date], [SewingLineID], [COMBOTYPE],[STD QTY]
			) AS Subquery

			-- print concat(format(getdate(), 'HH:mm:ss'), ' Stdq_Finsh_Step3')
			SELECT [OrderID] = SFS2.OrderID
				,[Request date] = CONVERT(NVARCHAR,  SFS1.[Request date], 23)
				,[SewingLineID] = SFS1. SewingLineID
				,[COMBOTYPE] = SFS1.COMBOTYPE
				,[ACCU. STDQTY] = SUM(SFS1.[STD QTY]) OVER (PARTITION BY SFS1.OrderID, SFS1.COMBOTYPE ORDER BY SFS1.[Request date])
				--,[STD QTY BY LINE] = SFS1.[STD QTY]
				,[ACCU. STD QTY BY LINE] = MAX([DAILY TOTAL]) OVER (PARTITION BY SFS1.OrderID,SFS1.SewingLineID, SFS1.COMBOTYPE ORDER BY SFS1.[Request date])
			into #tmp_Stdq_Finsh_Step3
			FROM #tmp_Stdq_Finsh_Step1 SFS1 WITH(NOLOCK)
			JOIN #tmp_Stdq_Finsh_Step2 SFS2 WITH(NOLOCK) ON SFS1.OrderID = SFS2.OrderID  AND SFS1.[Request date] = SFS2.[Request date] AND SFS1.SewingLineID = SFS2.SewingLineID AND SFS1.COMBOTYPE = SFS2.COMBOTYPE and SFS2.RowNum = 1

			-- print concat(format(getdate(), 'HH:mm:ss'), ' Stdq_Finsh_Step4')
			select [OrderID] 
				,[Request date] 
				,[SewingLineID]
				,[COMBOTYPE]
				,[ACCU. STDQTY] = case when  [ACCU. STDQTY] = LAG([ACCU. STDQTY]) OVER (ORDER BY [Request date]) 
										THEN LAG([ACCU. STDQTY]) OVER (ORDER BY [Request date]) 
										ELSE [ACCU. STDQTY] END
				,[STD QTY BY LINE] = [ACCU. STD QTY BY LINE] - ISNULL(LAG([ACCU. STD QTY BY LINE]) OVER (PARTITION BY OrderID, [SewingLineID] ORDER BY [Request date]),0) 
				,[ACCU. STD QTY BY LINE]
				--,[RowNum] = ROW_NUMBER() OVER (PARTITION BY OrderID,[Request date], [SewingLineID] ORDER BY[Request date]ASC , [ACCU. STD QTY BY LINE] DESC)
			into #tmp_Stdq_Finsh_Step4
			FROM #tmp_Stdq_Finsh_Step3

			-- print concat(format(getdate(), 'HH:mm:ss'), ' Stdq_Finsh_Step5')
			SELECT [OrderID] = a.[OrderID] 
				,[Request date]  = a.[Request date] 
				,[SewingLineID] = a.[SewingLineID]
				,[COMBOTYPE] = a.[COMBOTYPE]
				,[STDQTY] =  MAX(b.[ACCU. STDQTY]) - isnull(LAG(MAX(b.[ACCU. STDQTY])) OVER (PARTITION BY a.OrderID ORDER BY a.OrderID, a.[Request date]),0) 
				,[ACCU. STDQTY] = MAX(a.[ACCU. STDQTY]) OVER (PARTITION BY a.OrderID ORDER BY a.[Request date])
				,[STD QTY BY LINE] =a.[STD QTY BY LINE]
				,[ACCU. STD QTY BY LINE] = a.[ACCU. STD QTY BY LINE]
			into #tmp_Stdq_Finsh_Step5
			From #tmp_Stdq_Finsh_Step4 a
			inner join #tmp_Stdq_Finsh_Step4 b on a.[Request date] = b.[Request date]
			--where a.RowNum = 1
			GROUP BY a.OrderID,a.[SewingLineID],a.[Request date],a.[STD QTY BY LINE],a.[ACCU. STD QTY BY LINE],a.[COMBOTYPE],a.[ACCU. STDQTY]

			-- print concat(format(getdate(), 'HH:mm:ss'), ' Stdq_Finsh_Step6')
			SELECT [OrderID]
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
			into #tmp_Stdq_Finsh_Step6
			FROM #tmp_Stdq_Finsh_Step5

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutQty_Step1')
			select distinct wd.OrderID,
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
			into #tmp_EstCutQty_Step1
			from WorkOrderForOutput w with(nolock)
			inner join WorkOrderForOutput_Distribute wd WITH(NOLOCK) on wd.WorkOrderForOutputUkey = w.Ukey
			inner join WorkOrderForOutput_PatternPanel wp WITH(NOLOCK) on wp.WorkOrderForOutputUkey = w.Ukey
			/* WorkOrderForOutput_Distribute.SizeCode與SewingSchedule_Detail.Size WorkOrderForOutput_Distribute.Article與SewingSchedule_Detail.Article對照*/
			inner join SewingSchedule_Detail ssd WITH(NOLOCK) on ssd.OrderID = wd.OrderID and ssd.Article = wd.Article and ssd.SizeCode = wd.SizeCode
			/****************************************************************************************************************************/
			outer apply
			(
				Select  val = c.qty * w.layer
				From WorkOrderForOutput_SizeRatio c
				Where  c.WorkOrderForOutputUkey =w.Ukey 
			) as CutQty
			where exists (select 1 from #tmp_StdQ_MainDates main WITH(NOLOCK) where main.OrderID = wd.OrderID)
			and exists (select 1 from Orders o WITH(NOLOCK)  where wd.OrderID  = o.ID and o.LocalOrder = 0) --非local單 
			and not exists(select 1 from Cutting_WIPExcludePatternPanel cw WITH(NOLOCK) where cw.ID = w.ID and cw.PatternPanel = wp.PatternPanel) -- ISP20201886 排除指定 PatternPanel
			and (((select WIP_ByShell from system) = 1 and exists(select 1 from Order_BOF bof WITH(NOLOCK) where bof.Id = w.Id and bof.FabricCode = w.FabricCode and bof.Kind = 1)) or (select WIP_ByShell from system) = 0)

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutQty_Step2')
			select x.*,x2.PatternPanel,x2.FabricCombo,FabricPanelCode
			into #tmp_EstCutQty_Step2
			from (select distinct Orderid,POID,Article,SizeCode,FabricCode,ColorID,EstCutDate,[CutQty] from #tmp_EstCutQty_Step1) x
			outer apply(select top 1 * from #tmp_EstCutQty_Step1 where Orderid = x.Orderid and POID = x.POID and Article = x.Article and SizeCode = x.SizeCode and FabricCode = x.FabricCode and ColorID = x.ColorID order by FabricPanelCode)x2

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutWorkDate')
			SELECT DISTINCT [Date]
			into #tmp_EstCutWorkDate
			FROM Workhour WH
			INNER JOIN #tmp_StdQ_MainDates SM on WH.FactoryID = SM.FactoryGroup 
			where Holiday = 0 
			and not exists(select 1 from holiday where holiday.HolidayDate =WH.Date)

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutQty_Step3')
			SELECT [OrderID] =ECQS2.Orderid
				, [POID]
				, [Article]
				, [SizeCode]
				, [EstCutDate]
				, [FabricCombo]
				, [CutQty]
			into #tmp_EstCutQty_Step3
			FROM #tmp_EstCutQty_Step2 ECQS2 
			LEFT JOIN #tmp_StdQ_MainDates SM  on SM.OrderID = ECQS2.OrderID
			left JOIN #tmp_EstCutWorkDate ECWD on ECWD.[Date] = ECQS2.[EstCutDate]

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutCount')
			SELECT OrderID 
				, Article
				,[Count] = COUNT(DISTINCT [FabricCombo])
			into #tmp_EstCutCount
			FROM #tmp_EstCutQty_Step3 
			GROUP by OrderID , Article

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutSum')
			SELECT
				a.OrderID,Article,SizeCode,
				[val] = COUNT(DISTINCT [FabricCombo])  
			into #tmp_EstCutSum
			FROM #tmp_EstCutQty_Step2 a
			left JOIN #tmp_StdQ_MainDates SM on a.orderID = SM.orderid
			Where a.EstCutDate <= DATEADD(DAY, -2, SM.maxdate)
			GROUP BY a.OrderID,Article,SizeCode

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutGroup')
			SELECT [OrderID] = c.OrderID
				,[Count]
				,[SizeCode] = c.SizeCode
				,[CutQty] =  MIN(C.[CutQty])
			into #tmp_EstCutGroup
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
				FROM #tmp_EstCutQty_Step2 t
				--left JOIN StdQ_MainDates sfs6 on t.orderID = sfs6.orderid
				left join #tmp_EstCutSum e on e.OrderID = t. orderid AND e.Article = t.article AND e.SizeCode = t.sizecode
				WHERE EXISTS (SELECT 1 FROM #tmp_StdQ_MainDates sfs6 WHERE t.OrderID = sfs6.OrderID)
				GROUP BY t.OrderID,t.Article,t.SizeCode,EStCutDate,FabricCombo,e.[val]
			) c
			inner join #tmp_EstCutQty_Step2 ecqs2 on ecqs2.orderid = c.orderid
			GROUP BY c.SizeCode ,c.OrderID,c.[Count]

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutALLGroup')
			SELECT e.orderid
				,[AccuEstCutQty] = SUM(ec.[CutQty])
			into #tmp_EstCutALLGroup
			FROM #tmp_EstCutCount e 
			left JOIN #tmp_EstCutGroup  ec on ec.OrderID = e.OrderID and ec.[Count] = e.[Count] 
			group by e.orderid

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutQty_Step4')
			SELECT
				sfs6.[OrderID],
				sfs6.[Request date],
				sfs6.[SewingLineID],
				sfs6.[STDQTY],
				sfs6.[ACCU. STDQTY],
				sfs6.[STD QTY BY LINE],
				sfs6.[ACCU. STD QTY BY LINE],
				eag.[AccuEstCutQty]
			into #tmp_EstCutQty_Step4
			FROM #tmp_Stdq_Finsh_Step6 sfs6
			LEFT join #tmp_EstCutALLGroup eag on eag.orderid = sfs6.[OrderID]

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutQty_Step5')
			SELECT [ROWNumber] = ROW_NUMBER() OVER (PARTITION BY OrderID, SewingLineID ORDER BY [Request Date])
			,*
			,[STDQTY BY SUM] = SUM([STD QTY BY LINE]) OVER(PARTITION BY OrderID,SewingLineID ORDER BY [Request Date])
			into #tmp_EstCutQty_Step5
			FROM #tmp_EstCutQty_Step4

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutQty_Step6')
			SELECT  * ,
				CASE
					WHEN AccuEstCutQty - [STDQTY BY SUM]  >= 0 THEN [STD QTY BY LINE]
					ELSE AccuEstCutQty - LAG([STDQTY BY SUM]) OVER (PARTITION BY OrderID,SewingLineID ORDER BY [Request date])
				END AS AccEstCutQtyByLine
			into #tmp_EstCutQty_Step6
			from #tmp_EstCutQty_Step5

			-- print concat(format(getdate(), 'HH:mm:ss'), ' Marker_ML')
			select *
				, r_id = ROW_NUMBER() over(partition by OrderID order by case when [MatchFabric] <> '' or [OneTwoWay] <> '' or [HorizontalCutting] <> '' then 1 else 2 end)		
			into #tmp_Marker_ML
			from (
				select distinct [MatchFabric] = ( case ml.MatchFabric
													when '1' then concat('Body Mapping:V-Repeat ',IIF(ml.V_Repeat is null,'',CAST(ml.V_Repeat AS VARCHAR)))
													when '2' then concat('Checker:V-Repeat ',IIF(ml.V_Repeat is null,'',CAST(ml.V_Repeat AS VARCHAR)),' Checker:H-Repeat '+IIF(ml.H_Repeat is null,'',CAST(ml.H_Repeat AS VARCHAR)))
													when '3' then concat('Horizontal stripe:V-Repeat ',IIF(ml.V_Repeat is null,'',CAST(ml.V_Repeat AS VARCHAR)))
													when '4' then concat('Straight stripe:H-Repeat ',IIF(ml.H_Repeat is null,'',CAST(ml.H_Repeat AS VARCHAR)))
													else '' end),
						[OneTwoWay] = IIF(ml.OneTwoWay=1, 'one way cutting', ''),
						[HorizontalCutting] = IIF(ml.HorizontalCutting =1, 'Straight fabric use Horizontal cutting', ''),
						wo.OrderID,
						ml.ID,
						ml.Version
				from Marker_ML ml with (nolock)
				inner join WorkOrderForOutput wo with (nolock) on wo.MarkerVersion = ml.Version and ml.MarkerName = wo.Markername
				where exists (select 1 from DropDownList d with (nolock) where ml.MatchFabric = d.ID and d.type = 'MatchFabric')
				and exists (select 1 from Marker m with (nolock) where ml.ID = m.ID and m.Version = ml.Version and wo.MarkerNo = m.MarkerNo
					and m.EditDate = (select MAX(m2.EditDate) from Marker m2 with (nolock) where m.ID = m2.ID and m.Version = m2.Version)
				)
				and exists (select 1 from #tmp_StdQ_MainDates t WITH(NOLOCK) INNER JOIN Orders o WITH(NOLOCK) ON t.OrderID = o.ID where o.POID = wo.OrderID)
			) ml	

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutQty_Step7')
			SELECT [MDivisionID] = MAIN.MDivisionID
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
				,[MatchFabric] = ISNULL(MarkerInfo.MatchFabric, '') + char(10) + ISNULL(MarkerInfo.OneTwoWay, '') + ' ' + ISNULL(MarkerInfo.HorizontalCutting, '')
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
			into #tmp_EstCutQty_Step7
			FROM #tmp_EstCutQty_Step6 ECQE
			INNER JOIN #tmp_StdQ_MainDates MAIN WITH(NOLOCK) on ECQE.OrderID = MAIN.OrderID
			INNER JOIN Orders O WITH(NOLOCK) ON MAIN.OrderID = O.ID
			left join Cutting c WITH (NOLOCK) on c.ID = o.CuttingSP
			INNER JOIN Style S WITH(NOLOCK) ON O.StyleID = S.ID AND o.BrandID = s.BrandID and o.SeasonID = s.SeasonID
			INNER JOIN DropDownList DDL WITH(NOLOCK) on DDL.ID = o.Category and DDL.[Type] = 'Category'
			OUTER APPLY(
				select MatchFabric, OneTwoWay, HorizontalCutting
				from #tmp_Marker_ML ml
				where ml.OrderID = o.POID
				and ml.r_id = 1
			) MarkerInfo


			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutQty_Step8')
			SELECT  *
				,[BalanceCutQty] = SupplyCutQty - StdQty
				,[BalanceCutQtyByLine] = SupplyCutQtyByLine - StdQtyByLine
			into #tmp_EstCutQty_Step8
			FROM #tmp_EstCutQty_Step7

			-- print concat(format(getdate(), 'HH:mm:ss'), ' EstCutQty_END')
			SELECT  * 
				,[SupplyCutQtyVSStdQty] = IIF (BalanceCutQty < 0, SupplyCutQty, StdQty)
				,[SupplyCutQtyVSStdQtyByLine] = IIF (BalanceCutQtyByLine < 0, SupplyCutQtyByLine, StdQtyByLine)
			into #tmp_EstCutQty_END
			FROM #tmp_EstCutQty_Step8
	

			SELECT [MDivisionID] = ISNULL(MDivisionID, '')
				, [FactoryID] = ISNULL(FactoryID, '')
				, [BrandID] = ISNULL(BrandID, '')
				, [StyleID] = ISNULL(StyleID, '')
				, [SeasonID] = ISNULL(SeasonID, '')
				, [CDCodeNew] = ISNULL(CDCodeNew, '')
				, [FabricType] = ISNULL(FabricType, '')
				, [POID] = ISNULL(POID, '')
				, [Category] = ISNULL(Category, '')
				, [WorkType] = ISNULL(WorkType, '')
				, [MatchFabric] = ISNULL(MatchFabric, '')
				, [OrderID] = ISNULL(OrderID, '')
				, [SciDelivery] = SciDelivery
				, [BuyerDelivery] = BuyerDelivery
				, [OrderQty] = ISNULL(OrderQty, 0)
				, [SewInLineDate] = SewInLineDate
				, [SewOffLineDate] = SewOffLineDate
				, [SewingLineID] = ISNULL(SewingLineID, '')
				, [RequestDate] = RequestDate
				, [StdQty] = ISNULL(StdQty, 0)
				, [StdQtyByLine] = ISNULL(StdQtyByLine, 0)
				, [AccuStdQty] = ISNULL(AccuStdQty, 0)
				, [AccuStdQtyByLine] = ISNULL(AccuStdQtyByLine, 0)
				, [AccuEstCutQty] = ISNULL(AccuEstCutQty, 0)
				, [AccuEstCutQtyByLine] = ISNULL(AccuEstCutQtyByLine, 0)
				, [SupplyCutQty] = ISNULL(SupplyCutQty, 0)
				, [SupplyCutQtyByLine] = ISNULL(SupplyCutQtyByLine, 0)
				, [BalanceCutQty] = ISNULL(BalanceCutQty, 0)
				, [BalanceCutQtyByLine] = ISNULL(BalanceCutQtyByLine, 0)
				, [SupplyCutQtyVSStdQty] = ISNULL(SupplyCutQtyVSStdQty, 0)
				, [SupplyCutQtyVSStdQtyByLine] = ISNULL(SupplyCutQtyVSStdQtyByLine, 0)
				, [BIFactoryID] = @BIFactoryID
				, [BIInsertDate] = GetDate()
			FROM #tmp_EstCutQty_END;

			drop table #tmp_EstCutALLGroup,#tmp_EstCutCount,#tmp_EstCutGroup,#tmp_EstCutQty_END,#tmp_EstCutQty_Step1,#tmp_EstCutQty_Step2
			,#tmp_EstCutQty_Step3,#tmp_EstCutQty_Step4,#tmp_EstCutQty_Step5,#tmp_EstCutQty_Step6,#tmp_EstCutQty_Step7,#tmp_EstCutQty_Step8
			,#tmp_EstCutSum,#tmp_EstCutWorkDate,#tmp_StdQ_DateList,#tmp_StdQ_DateRange,#tmp_Stdq_Finsh,#tmp_Stdq_Finsh_GroupBy,#tmp_Stdq_Finsh_Step1
			,#tmp_Stdq_Finsh_Step2,#tmp_Stdq_Finsh_Step3,#tmp_Stdq_Finsh_Step4,#tmp_Stdq_Finsh_Step5,#tmp_Stdq_Finsh_Step6,#tmp_StdQ_MainDates
			,#tmp_StdQ_OriTotalWorkHour,#tmp_StdQ_WorkDataList,#tmp_StdQ_WorkDataList_Step1,#tmp_StdQ_WorkDataList_Step2,#tmp_StdQ_WorkDataList_Step3
			,#tmp_StdQ_WorkDataList_Step4,#tmp_StdQ_WorkDate,#tmp_Marker_ML";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sql, listPar, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@EDate", item.EDate),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                string sql = @"
				update b 
				set b.BIPImportCuttingBCSCmdTime = GETDATE()
				from [MainServer].[Production].[dbo].[SewingSchedule] b 
				where exists (select 1 from #tmp t where t.OrderID = b.OrderID)";
                sql += new Base().SqlBITableHistory("P_CuttingBCS", "P_CuttingBCS_History", "[MainServer].[Production].[dbo].[SewingSchedule]", string.Empty, needJoin: false,strWhereExists: " t.[OrderID] = p.[OrderID]") + Environment.NewLine;
                sql += $@"
                /************* 刪除P_CuttingBCS的資料，規則刪除相同的OrderID*************/
                Delete a
				FROM P_CuttingBCS a 
				WHERE NOT EXISTS (SELECT 1 FROM [MainServer].[Production].[dbo].[SewingSchedule] b WHERE a.OrderID = b.OrderID )

				/************* 更新P_CuttingBCS的資料*************/
				UPDATE P SET
				 P.[MDivisionID]							= t.[MDivisionID]
				,P.[FactoryID]								= t.[FactoryID]
				,P.[BrandID]								= t.[BrandID]
				,P.[StyleID]								= t.[StyleID]
				,P.[SeasonID]								= t.[SeasonID]
				,P.[CDCodeNew]								= t.[CDCodeNew]
				,P.[FabricType]								= t.[FabricType]
				,P.[POID]									= t.[POID]
				,P.[Category]								= t.[Category]
				,P.[WorkType]								= t.[WorkType]
				,P.[MatchFabric]							= t.[MatchFabric]
				,P.[SciDelivery]							= t.[SciDelivery]
				,P.[BuyerDelivery]							= t.[BuyerDelivery]
				,P.[OrderQty]								= t.[OrderQty]
				,P.[SewInLineDate]							= t.[SewInLineDate]
				,P.[SewOffLineDate]							= t.[SewOffLineDate]
				,P.[StdQty]									= t.[StdQty]
				,P.[StdQtyByLine]							= t.[StdQtyByLine]
				,P.[AccuStdQty]								= t.[AccuStdQty]
				,P.[AccuStdQtyByLine]						= t.[AccuStdQtyByLine]
				,P.[AccuEstCutQty]							= t.[AccuEstCutQty]
				,P.[AccuEstCutQtyByLine]					= t.[AccuEstCutQtyByLine]
				,P.[SupplyCutQty]							= t.[SupplyCutQty]
				,P.[SupplyCutQtyByLine]						= t.[SupplyCutQtyByLine]
				,P.[BalanceCutQty]							= t.[BalanceCutQty]
				,P.[BalanceCutQtyByLine]					= t.[BalanceCutQtyByLine]
				,P.[SupplyCutQtyVSStdQty]					= t.[SupplyCutQtyVSStdQty]
				,P.[SupplyCutQtyVSStdQtyByLine]				= t.[SupplyCutQtyVSStdQtyByLine]
				,P.[BIFactoryID]							= t.[BIFactoryID]
				,P.[BIInsertDate]							= t.[BIInsertDate]
				FROM P_CuttingBCS p
				INNER JOIN #tmp t on t.OrderID = p.OrderID AND T.SewingLineID = P.SewingLineID AND T.RequestDate = P.RequestDate

				/************* 新增P_CuttingBCS的資料*************/
				insert into P_CuttingBCS
				(
					 [MDivisionID]
					,[FactoryID]
					,[BrandID]
					,[StyleID]
					,[SeasonID]
					,[CDCodeNew]
					,[FabricType]
					,[POID]
					,[Category]
					,[WorkType]
					,[MatchFabric]
					,[OrderID]
					,[SciDelivery]
					,[BuyerDelivery]
					,[OrderQty]
					,[SewInLineDate]
					,[SewOffLineDate]
					,[SewingLineID]
					,[RequestDate]
					,[StdQty]
					,[StdQtyByLine]
					,[AccuStdQty]
					,[AccuStdQtyByLine]
					,[AccuEstCutQty]
					,[AccuEstCutQtyByLine]
					,[SupplyCutQty]
					,[SupplyCutQtyByLine]
					,[BalanceCutQty]
					,[BalanceCutQtyByLine]
					,[SupplyCutQtyVSStdQty]
					,[SupplyCutQtyVSStdQtyByLine]
					,[BIFactoryID]		
					,[BIInsertDate]		
				)
				select [MDivisionID]
					,[FactoryID]
					,[BrandID]
					,[StyleID]
					,[SeasonID]
					,[CDCodeNew]
					,[FabricType]
					,[POID]
					,[Category]
					,[WorkType]
					,[MatchFabric]
					,[OrderID]
					,[SciDelivery]
					,[BuyerDelivery]
					,[OrderQty]
					,[SewInLineDate]
					,[SewOffLineDate]
					,[SewingLineID]
					,[RequestDate]
					,[StdQty]
					,[StdQtyByLine]
					,[AccuStdQty]
					,[AccuStdQtyByLine]
					,[AccuEstCutQty]
					,[AccuEstCutQtyByLine]
					,[SupplyCutQty]
					,[SupplyCutQtyByLine]
					,[BalanceCutQty]
					,[BalanceCutQtyByLine]
					,[SupplyCutQtyVSStdQty]
					,[SupplyCutQtyVSStdQtyByLine]
					,[BIFactoryID]		
					,[BIInsertDate]		
				from #tmp a
				where not exists (select 1 from P_CuttingBCS b where a.OrderID = b.OrderID and a.SewingLineID = b.SewingLineID and a.RequestDate = b.RequestDate)
				";

                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}

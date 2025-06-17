-- =============================================
-- Author:		Benson
-- Create date: 2019/03/11
-- Description:	Get Hanger Output Data and Insert into SewingOutput�BRFT 
-- =============================================
CREATE PROCEDURE [dbo].[SNPAutoTransferToSewingOutput]
(	
	@execuDatetime as Datetime
)
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
			--Declare
			Declare 
			@DateStart date,
		    @mailBody  As VARCHAR(1000)

			set @DateStart = CAST (@execuDatetime AS DATE)

			
		select a.MDivision
				,a.FactoryID
				,a.Shift
				, [BeginTime] =  cast(convert(nvarchar(10),@DateStart,112) + ' ' + cast(a.BeginTime as varchar(8)) as datetime)
				, [EndTime] = cast(convert(nvarchar(10),@DateStart,112) + ' ' + cast(a.EndTime as varchar(8)) as datetime)
			into #ShiftByDate_Shift
			from [ExtendServer].ManufacturingExecution.dbo.Shift a
			inner join 
			(
				select MDivision, FactoryID, Shift, MAX(StartDate) StartDate
				from [ExtendServer].ManufacturingExecution.dbo.Shift
				where StartDate <= @DateStart
				group by MDivision, FactoryID, Shift
			) b on a.StartDate = b.StartDate and a.MDivision = b.MDivision and a.FactoryID = b.FactoryID and a.Shift = b.Shift

			select
				[MDivision] = a.MDivision
				,[Factory] = isnull(b.FactoryID, a.FactoryID)
				,[Shift] = a.Shift
				,[Line] = b.Line
				,[Team] = b.Team 
				,[BeginTime] = isnull(b.BeginTime,a.BeginTime)
				,[EndTime] = isnull(b.EndTime, a.EndTime)
				,[startHour] = datepart(hour, isnull(b.BeginTime,a.BeginTime))
			into #ShiftByDate_Efficiency
			from #ShiftByDate_Shift a
			left join 
			(
				select [MDivision] = f.MDivisionID
				  , eff.Shift
				  , eff.Team
				  , eff.Line
				  , eff.FactoryID
				  , [BeginTime] =  cast(convert(nvarchar(10),@execuDatetime,112) + ' ' + cast(eff.BeginTime as varchar(8)) as datetime)
				  , [EndTime] = cast(convert(nvarchar(10),@execuDatetime,112) + ' ' + cast(eff.EndTime as varchar(8)) as datetime)
				  , [EndTimeHH] = DATEPART(Hour, eff.EndTime)
				from [ExtendServer].ManufacturingExecution.dbo.Efficiency eff
				inner join Factory f on f.ID = eff.FactoryID
				where eff.Date = @DateStart 
			)b on a.MDivision = b.MDivision and a.FactoryID = b.FactoryID and a.Shift = b.Shift 
	
			select distinct
				[dDate] = convert(date, i.AddDate)
				,[dTime] = convert(smalldatetime, case  
								when Shift.cnt < 2 and i.AddDate >  s_HHmm.Shift_EndTime
									then format(i.AddDate,'yyyy-MM-dd '+ FORMAT(s_HHmm.Shift_EndTime,'HH') + ':' + s_HHmm.Shift_mm +':00')
								when Shift.cnt < 2 and i.AddDate <  s_HHmm.Shift_BeginTime
									then format(i.AddDate,'yyyy-MM-dd '+ FORMAT(s_HHmm.Shift_BeginTime,'HH') + ':' + s_HHmm.Shift_mm +':00')
								when s_HHmm.Shift_Minute <> 0 and DATEPART(MINUTE, i.AddDate) >= s_HHmm.Shift_Minute
									then format(i.AddDate,'yyyy-MM-dd '
									+ FORMAT(i.AddDate,'HH') 
									+ ':' + s_HHmm.Shift_mm +':00')
								when s_HHmm.Shift_Minute <> 0 
									then format(i.AddDate,'yyyy-MM-dd '
									 + FORMAT(dateAdd(Hour,-1, i.AddDate),'HH')
									 + ':' + s_HHmm.Shift_mm +':00')					
								else format(i.AddDate,'yyyy-MM-dd HH:00:00') 
								end) 
				,[BeginTime] = s_HHmm.Shift_BeginTime
				,[EndTime] = s_HHmm.Shift_EndTime
				,[MoNo] = i.OrderId+'-'+i.Location
				,[ColorName] = i.Article
				,[SizeName] = i.Size
				,[FactoryID] = i.FactoryID
				,[WorkShop]=''
				,[WorkLine]=i.Line
				,i.ID
				,shift.cnt 
				,i.Team
			into #Output1
			from [ExtendServer].ManufacturingExecution.dbo.inspection i
			left join Production.dbo.Factory f on i.FactoryID=f.ID
			left join #ShiftByDate_Efficiency s1 on s1.Factory =i.FactoryID and s1.Line = i.Line and s1.Team = i.Team
			left join #ShiftByDate_Shift s2 on s2.MDivision=f.MDivisionID and s2.Shift=i.Shift
			outer apply(select 
				 Shift_BeginTime = isnull(s1.BeginTime,s2.BeginTime)
				, Shift_EndTime = isnull(s1.EndTime,s2.EndTime)
				, Shift_HH = FORMAT(isnull(s1.BeginTime,s2.BeginTime),'HH')
				, Shift_mm = FORMAT(isnull(s1.BeginTime,s2.BeginTime),'mm')
				, Shift_Hour = datepart(HOUR, isnull(s1.BeginTime,s2.BeginTime))
				, Shift_Minute = DATEPART(MINUTE, isnull(s1.BeginTime,s2.BeginTime))  -- print '08'-1
			) s_HHmm
			outer apply(
				select count(1) cnt from 
				(
					select distinct MDivision,Shift 
					from [ExtendServer].ManufacturingExecution.dbo.Shift
					where MDivision = f.MDivisionID 
					and convert(date, StartDate) <= CONVERT(date,i.AddDate)
				) a
			)Shift
			where CONVERT(date, i.AddDate) = @DateStart


			select 
			dDate,dTime,MoNo,ColorName,SizeName,FactoryID,WorkShop,WorkLine,Team
			,[InputQty]= sum(InspQty.cnt)
			,[OutputQty]= sum(OutputQty.cnt)
			,[FailQty]= sum(FailQty.cnt)
			into #tmpFinalAddDate
			from #Output1 i
			outer apply (
				select cnt = count(1)
				from [ExtendServer].ManufacturingExecution.dbo.inspection
				where id=i.ID
			)InspQty
			outer apply(
				select cnt = COUNT(1)
				from [ExtendServer].ManufacturingExecution.dbo.inspection
				where Status='Pass'
				and id=i.ID
			)OutputQty
			outer apply(
				select cnt = COUNT(1)
				from [ExtendServer].ManufacturingExecution.dbo.Inspection insp
				where insp.id=i.ID
				and exists(select 1 from [ExtendServer].ManufacturingExecution.dbo.Inspection_Detail 
				where id = insp.ID and Junk=0)
			)FailQty
			group by dDate,dTime,MoNo,ColorName,SizeName,FactoryID,WorkShop,WorkLine,Team 

			select distinct
				[dDate] = convert(date, i.EditDate)
				,[dTime] = convert(smalldatetime,  case  
								when Shift.cnt < 2 and i.EditDate >  s_HHmm.Shift_EndTime
									then format(i.EditDate,'yyyy-MM-dd '+ FORMAT(s_HHmm.Shift_EndTime,'HH') + ':' + s_HHmm.Shift_mm +':00')
								when Shift.cnt < 2 and i.EditDate < s_HHmm.Shift_BeginTime
									then format(i.EditDate,'yyyy-MM-dd '+ FORMAT(s_HHmm.Shift_BeginTime,'HH')  + ':' + s_HHmm.Shift_mm +':00')
								when s_HHmm.Shift_Minute <> 0 and DATEPART(MINUTE, i.EditDate) >= s_HHmm.Shift_Minute
									then format(i.EditDate,'yyyy-MM-dd '+ FORMAT(i.EditDate,'HH') + ':' + s_HHmm.Shift_mm +':00')
								when s_HHmm.Shift_Minute <> 0 
									then format(i.EditDate,'yyyy-MM-dd '+ FORMAT(dateAdd(Hour,-1, i.EditDate),'HH') + ':' + s_HHmm.Shift_mm +':00')	
								else format(i.EditDate,'yyyy-MM-dd HH:00:00') 
								end)
				,[BeginTime] = s_HHmm.Shift_BeginTime
				,[EndTime] = s_HHmm.Shift_EndTime
				,[MoNo] = i.OrderId+'-'+i.Location
				,[ColorName] = i.Article
				,[SizeName] = i.Size
				,[FactoryID] = i.FactoryID
				,[WorkShop]=''
				,[WorkLine]=i.Line
				,i.ID
				,i.Status
				,i.Team
			into #Output2
			from [ExtendServer].ManufacturingExecution.dbo.inspection i
			left join Production.dbo.Factory f on i.FactoryID=f.ID
			left join #ShiftByDate_Efficiency s1 on s1.Factory =i.FactoryID and s1.Line = i.Line and s1.Team = i.Team
			left join #ShiftByDate_Shift s2 on s2.MDivision=f.MDivisionID and s2.Shift=i.Shift
			outer apply(select 
				 Shift_BeginTime = isnull(s1.BeginTime,s2.BeginTime)
				, Shift_EndTime = isnull(s1.EndTime,s2.EndTime)
				, Shift_HH = FORMAT(isnull(s1.BeginTime,s2.BeginTime),'HH')
				, Shift_mm = FORMAT(isnull(s1.BeginTime,s2.BeginTime),'mm')
				, Shift_Hour = datepart(HOUR, isnull(s1.BeginTime,s2.BeginTime))
				, Shift_Minute = DATEPART(MINUTE, isnull(s1.BeginTime,s2.BeginTime))  -- print '08'-1
			) s_HHmm
			outer apply(
				select count(1) cnt from 
				(
					select distinct MDivision,Shift 
					from [ExtendServer].ManufacturingExecution.dbo.Shift
					where MDivision = f.MDivisionID 
					and convert(date, StartDate) <= CONVERT(date,i.EditDate)
				) a
			)Shift
			where CONVERT(date, i.EditDate) = @DateStart
			--and exists (select 1 from #AvailableLine temp where temp.Line = i.Line)

			SELECT s.OutputDate,s.SewingLineID,sdd.OrderId,sdd.ComboType
			INTO  #now_Sewing_Data
			FROM Production.dbo.SewingOutput s WITH(NOLOCK)
			INNER JOIN Production.dbo.SewingOutput_Detail sd WITH(NOLOCK) ON s.ID=sd.ID
			INNER JOIN Production.dbo.SewingOutput_Detail_Detail sdd WITH(NOLOCK) 
			ON sd.ID=sdd.ID 
					AND sd.UKey=sdd.SewingOutput_DetailUKey
					AND sd.OrderId=sdd.OrderId
					AND sd.ComboType=sdd.ComboType
					AND sd.Article=sdd.Article
			WHERE s.OutputDate= CONVERT(date, @execuDatetime)
					AND s.[Shift] = 'D'
					AND s.[Team] = 'A'
					AND s.FactoryID='SNP'
					AND s.[MDivisionID]=(SELECT TOP 1 ID FROM Production.dbo.MDivision)

			select 
			dDate,dTime,MoNo,ColorName,SizeName,FactoryID,WorkShop,WorkLine,Team
			,[InputQty]= 0
			,[OutputQty]= COUNT(1)
			,[FailQty]= 0
			into #tmpFinalEditDate
			from #Output2 i
			where Status='Fixed'
			group by dDate,dTime,MoNo,ColorName,SizeName,FactoryID,WorkShop,WorkLine,Team 
			order by dDate,dTime,MoNo
			--Get Data which is already in DB , must exclude it

			--#now_Sewing_Data
			SELECt r.OrderID,r.CDate,r.SewinglineID
			INTO #now_RFT_Data
			FROM RFT r WITH(NOLOCK)
			INNER JOIN Rft_Detail rd WITH(NOLOCK) ON r.ID=rd.ID
			WHERE CDate=@DateStart
					AND r.[Shift] = 'D'
					AND r.[Team] = 'A'
					AND r.FactoryID='SNP'
					AND r.[MDivisionID]=(SELECT TOP 1 ID FROM MDivision)
					AND r.AddName='SCIMIS'


			----------[SUNRISE Tmp Table]
			SELECT a.* 
			INTO #tOutputTotal
			FROM
			(
				select [dDate]
				,[dTime]
				,[MoNo]
				,[ColorName]
				,[SizeName]
				,[FactoryID]
				,[WorkShop],[WorkLine]
				,[InputQty] = sum([InputQty])
				,[OutputQty] = sum([OutputQty])
				,[FailQty] = sum([FailQty])
				,[Team] = isnull(a.Team, '')
				from 
				(
					select * from #tmpFinalAddDate
					union all
					select * from #tmpFinalEditDate
				)A
				group by [dDate],[dTime],[MoNo],[ColorName],[SizeName],[FactoryID]
					,[WorkShop],[WorkLine],a.Team
			)a
			WHERE LEN((SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=1)) >=10 --Must Same as PMS DB Datatype
			AND  LEN((SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=2)) =1    --Must Same as PMS DB Datatype
			AND dDate = CAST (@DateStart AS DATE)
			AND NOT EXISTS (
				SELECT 1 FROM #now_Sewing_Data WHERE 
				OrderId=(SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=1)
				AND SewingLineID  =a.WorkLine  
				AND ComboType  = (SELECT TOP 1 Data FROM SplitString(a.Mono  ,'-') WHERE No=2)
				AND OutputDate = CAST (@DateStart AS DATE)
			)

			--#tReworkTotal
			SELECT a.* 
			INTO #tReworkTotal
			FROM
			(
				SELECT *
				FROM View_tReworkTotal
				WHERE MONo LIKE '%-%' 
			)a
			WHERE LEN((SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=1)) >=10 --Must Same as PMS DB Datatype
			AND  LEN((SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=2)) =1    --Must Same as PMS DB Datatype
			AND dDate = @DateStart
			AND NOT EXISTS (

				SELECT 1 FROM #now_RFT_Data WHERE 
											CDate = @DateStart
											AND OrderId  =(SELECT TOP 1 Data FROM SplitString(a.Mono  ,'-') WHERE No=1)
											AND SewingLineID  =a.WorkLine  
			)
			
			--#tReworkCount
			SELECT *
			INTO #tReworkCount
			FROM View_tReworkCount 
			WHERE MoNo Like '%-%'AND dDate = @DateStart
			----------


			--Prepare SewingOutput_Detail_Detail	
			SELECT 
				 dDate
				,WorkLine
				,[OrderId]= CASE WHEN MONo LIKE '%-%'
							THEN SUBSTRING(MONo, 1, CHARINDEX('-', MONo) - 1)
							ELSE MONo
							END
				,[ComboType]=   CASE WHEN MONo LIKE '%-%' 
								THEN  SUBSTRING(MONo, CHARINDEX('-', MONo) + 1, LEN(MONo) - CHARINDEX('-', MONo)) 
								ELSE ''--MONo
								END
				,ColorName as Article
				, SizeName as SizeCode
				,[QAQty]=sum(OutputQty) 
				,[InlineQty]=sum(InputQty) --4/13 InputQty�g�J��Sewing P01���e����prod qty��
				,[RowKey]=row_number()OVER (ORDER BY dDate ,WorkLine ,MONo ,ColorName ,SizeName)
			into #tmp_Into_SewingOutput_Detail_Detail_with0
			from #tOutputTotal
			where dDate = @DateStart
			group by dDate,WorkLine,MONo,ColorName,SizeName

			
			/*
			QAQty判斷說明：
			由於Hanger轉進的資料，會有「產出數量 > 訂單數量」的問題，因此寫入DB時要扣掉，以符合訂單數量。
			ComboType + OrderId + Article + SizeCode可能不只一個產線有生產，因此會有以下狀況
			
			[A]  Hanger的產出數量 >= 訂單數量 && 有生產該款式的產線 = 1 

				寫入PMS的產出數量 = 訂單數量 - 已經存在Production DB的數量


			[B]  Hanger的產出數量 >= 訂單數量 && 有生產該款式的產線 > 1 
			
				則產量最高的產線，寫入PMS的產出數量 = 訂單數量 - 已經存在Production DB的數量 - 所有產線的產出總和 + 自己這條產線的產出
				產量最高的產線不變

				舉例：若A款式
				1.訂單數量 = 100
				2.已經存在Production DB的數量 = 90
				3.產線01產出 = 7，產線02產出 = 5，總和產出12

				[B-1] 能寫入PMS的只有10，多餘的2要從產量最多的產線(01)去扣
					  =>產線01寫入： 100 - 90 - 12 + 7 =5
					  =>產線01寫入： 5

				另一種情況是 ：
				[B-2] 產線01 = 6，產線02 = 6，總和產出12
				  	  則只抓一筆來扣掉
					  =>產線01寫入： 100 - 90 - 12 + 6 = 4
					  =>產線01寫入： 6

			*/
			SELECt   t.dDate
					,t.WorkLine
					,t.OrderId
					,t.ComboType
					,t.Article
					,t.SizeCode
					,[QAQty]=CASE   WHEN (LineTotal.SumQty + AlreadyInPMS.Qty) >= Order_Qty.Qty AND LineCount.Val=1 THEN Order_Qty.Qty - AlreadyInPMS.Qty    ----狀況[A]
									WHEN (LineTotal.SumQty + AlreadyInPMS.Qty) >= Order_Qty.Qty AND LineCount.Val>1 AND t.RowKey=MaxQty.RowKey THEN (Order_Qty.Qty - AlreadyInPMS.Qty - LineTotal.SumQty + ISNULL(t.QAQty,0) )  ----狀況[B-1]
									ELSE ISNULL(t.QAQty,0)  ----狀況[B-2]
									END
					,t.[InlineQty]
			INTO #tmp_Into_SewingOutput_Detail_Detail_1
			FROM #tmp_Into_SewingOutput_Detail_Detail_with0 t
			outer apply(
				select value=1, o.DyeingLoss
				from Order_TmsCost ot with(nolock)
				inner join Order_Qty oq WITH (NOLOCK) on ot.id = oq.ID
				inner join orders o WITH (NOLOCK) on o.id = ot.id
				where ot.ArtworkTypeID = 'Garment Dye' and ot.Price > 0
				and oq.SizeCode=t.SizeCode and oq.Article=t.Article and o.id=t.OrderId
				and o.LocalOrder<>1
			)b
			OUTER APPLY(
                SELECT [Qty]=iif(b.value is not null,round(cast(ISNULL([Qty],0) as decimal) * (1+ isnull(b.DyeingLoss,0)/100),0),Order_Qty.Qty)
				FROM Order_Qty 
				WHERE ID=t.OrderId AND Article=t.Article AND SizeCode=t.SizeCode
			)Order_Qty  ----這個OrderId + Article + SizeCode的訂單數量
			OUTER APPLY(
				SELECT [Qty]=ISNULL(SUM(QAQty),0)
				FROM SewingOutput_Detail_Detail
				WHERE SewingOutput_DetailUKey
				IN(
					SELECT UKey
					FROM SewingOutput_Detail
					WHERE OrderID=t.OrderId
				)
				AND Article=t.Article
				AND SizeCode=t.SizeCode
			)AlreadyInPMS  ----這個ComboType + OrderId + Article + SizeCode，已經存在DB的產出數量
			OUTER APPLY(
				SELECT [SumQty]=SUM(ISNULL(t2.QAQty,0))
				FROM #tmp_Into_SewingOutput_Detail_Detail_with0 t2
				WHERE t.dDate=t2.dDate AND t.OrderId=t2.OrderId AND t.ComboType=t2.ComboType AND t.Article=t2.Article AND t.SizeCode=t2.SizeCode
			)LineTotal  ----加總所有產線的產出數量
			OUTER APPLY(
				SELECT TOP 1 [Val]=MAX(t2.QAQty) ,t2.dDate  ,t2.OrderId  ,t2.ComboType  ,t2.Article  ,t2.SizeCode , t2.InlineQty ,t2.RowKey
				FROM #tmp_Into_SewingOutput_Detail_Detail_with0 t2
				WHERE t.dDate=t2.dDate 
						AND t.OrderId=t2.OrderId 
						AND t.ComboType=t2.ComboType 
						AND t.Article=t2.Article 
						AND t.SizeCode=t2.SizeCode
				GROUP BY t2.dDate  ,t2.OrderId  ,t2.ComboType  ,t2.Article  ,t2.SizeCode , t2.InlineQty , t2.RowKey
				ORDER BY [Val] DESC
			)MaxQty  ----有生產 這個ComboType + OrderId + Article + SizeCode的產線當中，產出最高的那一條 (可能會有兩筆一模一樣的產出的，因此要抓TOP 1 的那一筆來扣)
			OUTER APPLY(
				SELECT [Val]=COUNT(t2.WorkLine)
				FROM #tmp_Into_SewingOutput_Detail_Detail_with0 t2
				WHERE t.dDate=t2.dDate AND t.OrderId=t2.OrderId AND t.ComboType=t2.ComboType AND t.Article=t2.Article AND t.SizeCode=t2.SizeCode
			)LineCount ----找出有幾條產線，是有生產 這個ComboType + OrderId + Article + SizeCode的
			
			
			SELECt  *
			INTO #tmp_Into_SewingOutput_Detail_Detail
			FROM #tmp_Into_SewingOutput_Detail_Detail_1 
			WHERE QAQty IS NOT NULL --WHERE QAQty > 0  4/15  > 0�n�Q�]�t�i�h


			--Prepare SewingOutput_Detail Fail
			select
			[OrderId]= CASE WHEN MONo LIKE '%-%'
					THEN SUBSTRING(MONo, 1, CHARINDEX('-', MONo) - 1)
					ELSE MONo
					END
			,[ComboType]=   CASE WHEN MONo LIKE '%-%' 
						THEN  SUBSTRING(MONo, CHARINDEX('-', MONo) + 1, LEN(MONo) - CHARINDEX('-', MONo)) 
						ELSE ''--MONo
						END
			, WorkLine
			, [FailCount] = count(*) 
			,[SizeCode]=SizeName
			into #tempFail
			from #tReworkCount
			where dDate =@DateStart
			group by MONo, WorkLine ,SizeName

			select 
			  dDate
			, t3.WorkLine
			, t3.OrderId
			, t3.ComboType
			, Article
			,[QAQty]= Sum(QAQty) 
			,[InlineQty]=  sum(InlineQty)
			--,[InlineQty]= CASE WHEN sum( ISNULL(FailCount,0) ) = 0 AND Sum(QAQty)=0 THEN sum(InlineQty) --�YtOutputTotal.FailQty�POutputQty����0���p�U�A�]�ݭn��InputQty�g�J��Sewing P01���e����prod qty���C
			--				ELSE sum( ISNULL(FailCount,0) ) + Sum(QAQty)   --�쥻�p���k
			--				END
			,[DefectQty]= (sum( ISNULL(FailCount,0) ) + Sum(QAQty)) - Sum(QAQty) 
			,[TMS] = TMS.CPU * TMS.CPUFactor * ( IIF(o.StyleUnit='PCS',100,IIF(Order_Rate.Rate is null,Style_Rate.Rate,Order_Rate.Rate) ) /100  ) * TMS.StdTMS--CPU * CPUFactor * (Rate/100) * StdTMS
			into #tmp_Into_SewingOutput_Detail
			from #tmp_Into_SewingOutput_Detail_Detail t3
			LEFT join #tempFail t on t.OrderId = t3.OrderId 
			AND t.ComboType=t3.ComboType 
			and t.WorkLine = t3.WorkLine 
			and t.SizeCode = t3.SizeCode 			
			LEFT JOIN Orders o ON o.ID=t3.OrderId
			OUTER APPLY(
				select  o.IsForecast
						, o.SewLine
						, o.CPU
						, o.CPUFactor
						, o.StyleUkey
						, StdTMS = (select StdTMS 
										from System WITH (NOLOCK))
				from Orders o WITH (NOLOCK) 
				inner join Factory f on o.FactoryID = f.ID
				where   o.FtyGroup = 'SNP' 
						and o.ID = t3.OrderId
						and o.Category != 'G'
						and f.IsProduceFty = 1
			)TMS
			OUTER APPLY(				
				select Location
				,[Rate] = isnull([dbo].[GetOrderLocation_Rate](o.ID,Location),[dbo].[GetStyleLocation_Rate](o.StyleUkey,Location)) 
				from Style_Location WITH (NOLOCK) 
				where StyleUkey = o.StyleUkey AND Location =t3.ComboType
			)Style_Rate
			OUTER APPLY(				
				select Location
				,[Rate] = isnull([dbo].[GetOrderLocation_Rate](o.ID,Location),[dbo].[GetStyleLocation_Rate](o.StyleUkey,Location)) 
				from Order_Location WITH (NOLOCK) 
				where OrderId = o.ID AND Location =t3.ComboType
			)Order_Rate
			group by dDate,t3.WorkLine, t3.OrderId, t3.ComboType, Article ,TMS.CPU ,TMS.CPUFactor ,Order_Rate.Rate,Style_Rate.Rate
			,o.StyleUnit ,TMS.StdTMS

		-- insert  Order_location when orderid not exists in Order_Location 
		--Start
			DECLARE CUR_SewingOutput_Detail CURSOR FOR 
                      select distinct OrderId from #tmp_Into_SewingOutput_Detail_Detail t
					  where not exists(select 1 from Order_Location s where s.OrderId = t.OrderId)

			declare @OrderId varchar(13) 
			OPEN CUR_SewingOutput_Detail   
			FETCH NEXT FROM CUR_SewingOutput_Detail INTO @OrderId 
			WHILE @@FETCH_STATUS = 0 
			BEGIN
			exec dbo.Ins_OrderLocation @OrderId
			FETCH NEXT FROM CUR_SewingOutput_Detail INTO @OrderId
			END
			CLOSE CUR_SewingOutput_Detail
			DEALLOCATE CUR_SewingOutput_Detail
		--End 
			
			--Prepare SewingOutput_Detail For Insrt
			select 
			[OutputDate]=dDate 
			, [SewingLineID]=WorkLine 
			, [QAQty]= Sum(QAQty) 
			, [DefectQty]= sum(DefectQty) 
			, [InlineQty]= sum(InlineQty) 
			, [TMS]=IIF(SUM(QAQTY)=0
							 , 0  
							 , SUM(TMS * QAQTY) / SUM(QAQTY)
						 )
			, [Manpower] = 0
			, [Manhour] = 0
			, [Efficiency]=0
			, [Shift] = 'D'
			, [Team] = 'A'
			, [Status] = 'New' 
			, [LockDate]=NULL
			, [Workhour] = 0
			, [FactoryID]='SNP'
			, [MDivision]=(SELECT TOP 1 ID FROM MDivision)
			, [Category]='O'
			, [SFCData]=0
			, [AddName] = 'SCIMIS' 
			, [AddDate] = GetDate() 
			, [EditName]= NULL
			, [EditDate]= NULL
			, [SubconOutFty]=NULL
			, [SubConOutContractNumber] = NULL
			INTO #tmp_1
			from #tmp_Into_SewingOutput_Detail
			group by dDate,WorkLine

			--Get ID
			SELECT [RowNumber]=row_number()OVER (ORDER BY OutputDate),*
			INTO  #tmp_2
			FROM  #tmp_1
			
			select 	
				[ID]= dbo.GetSewingOutputID_For_SNPAutoTransferToSewingOutput('SNPSM',RowNumber,@execuDatetime)
				,*
			INTO #tmp_SewingOutput
			FROM #tmp_2 t
			where not exists(
				select 1 from SewingOutput s 
				where s.OutputDate = t.OutputDate 
				and s.SewingLineID = t.SewingLineID 
				and s.FactoryID = t.FactoryID
				and s.Team = t.Team 
				and s.Shift = t.Shift
			)
			AND T.SewingLineID !=''

			

			--Begin Insert

			INSERT INTO SewingOutput 
			(ID ,OutputDate ,SewingLineID ,QAQty ,DefectQty ,InlineQty ,TMS ,Manpower ,ManHour ,Efficiency 
			,Shift ,Team ,Status ,LockDate ,WorkHour ,FactoryID ,MDivisionID , [Category], [SFCData], [AddName] , [AddDate] , [EditName]
			, [EditDate], [SubconOutFty], [SubConOutContractNumber])
			select 	
			  [ID]
			, [OutputDate]=CAST([OutputDate] AS DATE)
			, [SewingLineID]=ISNULL(ProductionLineAllocation.SewingLineID,ISNULL(ts.[SewingLineID],''))
			, ISNULL([QAQty] ,0)
			, ISNULL([DefectQty] ,0)
			, ISNULL([InlineQty] ,0)
			, ISNULL([TMS] ,0)
			, [Manpower]=0
			, [Manhour]=0
			, [Efficiency]=0
			, ISNULL([Shift] ,'')
			, ISNULL([Team],'')
			, ISNULL([Status] ,'')
			, [LockDate]=NULL
			, ISNULL([Workhour] ,'')
			, ISNULL([FactoryID],'')
			, ISNULL([MDivision],'')
			, ISNULL([Category],'')
			, [SFCData]
			, ISNULL([AddName] ,'')
			, [AddDate] 
			, [EditName]=''
			, [EditDate]=CAST([EditDate] AS DATETIME)
			, ISNULL([SubconOutFty] ,'')
			, ISNULL([SubConOutContractNumber] ,'')
			FROM #tmp_SewingOutput ts
			OUTER APPLY(			
				SELECT t2.SewingLineID
				FROM 
				(
					SELECT TOP 1 *
					FROM ProductionLineAllocation
					WHERE ProductionDate <=CAST(ts.[OutputDate] AS DATE)
					AND FactoryID=ts.[FactoryID]
					ORDER BY ProductionDate DESC
				) t
				INNER JOIN ProductionLineAllocation_Detail t2 ON t.FactoryID=t2.FactoryID AND t.ProductionDate=t2.ProductionDate
				WHERE t2.LineLocationID= ts.[SewingLineID] AND t2.Team=ts.[Team]
			)ProductionLineAllocation
		
			--insert SewingOutput_Detail
			INSERT INTO SewingOutput_Detail
				([ID],[OrderId],[ComboType],[Article],[Color],[TMS]
				,[HourlyStandardOutput],[WorkHour],[QAQty],[DefectQty]
				,[InlineQty],[OldDetailKey],[AutoCreate],[SewingReasonID],[Remark])
			SELECT 
			[ID]= b.ID
			,[OrderId]
			,[ComboType]
			,[Article]
			,[Color]=(SELECT TOP 1 ColorID FROM View_OrderFAColor WHERE ID =a.OrderId )
			,[TMS]=a.TMS
			,[HourlyStandardOutput]=NULL
			,[WorkHour]=0
			,[QAQty]=a.QAQty
			,[DefectQty]=a.DefectQty
			,[InlineQty]=a.InlineQty
			,[OldDetailKey]=NULL
			,[AutoCreate]=0
			,[SewingReasonID]= IIF(a.QAQty=0,'00001','') --�YQAQTY�A�w�]�N�Ĥ@��ReasionId
			,[Remark]=NULL
			FROM #tmp_Into_SewingOutput_Detail a
			INNER JOIN #tmp_SewingOutput b ON a.dDate=b.OutputDate AND a.WorkLine  = b.SewingLineID 
				
			--------------For cauclator SewingOutput_Detail Cumulate--------------

			DECLARE @Line as varchar(5)
				, @FactoryID as varchar(8) 
				, @OutputDate as date 


			DECLARE cursorSewingOutput 
			CURSOR FOR
				select s.SewingLineID, s.FactoryID, CAST(s.[OutputDate] AS DATE)
				from #tmp_SewingOutput s
				order by s.OutputDate

			OPEN cursorSewingOutput
			FETCH NEXT FROM cursorSewingOutput INTO @Line, @FactoryID, @OutputDate

			WHILE @@FETCH_STATUS = 0
			BEGIN
				exec RecalculateCumulateDay @Line, @FactoryID, @OutputDate

				FETCH NEXT FROM cursorSewingOutput INTO @Line, @FactoryID, @OutputDate
			END


			CLOSE cursorSewingOutput
			DEALLOCATE cursorSewingOutput
			
			--------------For cauclator SewingOutput.TMS--------------
			
			SELECT DISTINCT
				t2.ID 
				,[SumQaqty] = SUM(t2.QAQTY)
			INTO #tmp_SewingOutputTMS
			FROM #tmp_SewingOutput t
			INNER JOIN SewingOutput_Detail t2 ON t.ID=t2.ID
			WHERE t.ID IN (
				SELECT ID FROM #tmp_SewingOutput
			)
			group by t2.ID
				
			SELECT DISTINCT
			t.id
			,[TMS]=SUM( IIF (t3.SumQaqty=0,0, t2.TMS * t2.QAQty / t3.SumQaqty)  ) OVER (PARTITION BY  t.id ORDER BY  t.id) 
			INTO #tmp_SewingOutputTMS_Final
			FROm SewingOutput t
			INNER JOIN SewingOutput_Detail t2 ON t.ID=t2.ID
			INNER JOIN #tmp_SewingOutputTMS t3 ON t.ID=t3.ID

				
			UPDATE t
			SET t.TMS= ISNULL(t2.TMS ,0)
			FROM SewingOutput t
			INNER JOIN #tmp_SewingOutputTMS_Final t2 ON t2.ID=t.ID

			--------------For cauclator SewingOutput.TMS--------------


			SELECT 
			[ID]= b.ID
			,[OrderId]
			,[ComboType]
			,[Article]
			,[Color]=(SELECT TOP 1 ColorID FROM View_OrderFAColor WHERE ID =a.OrderId )
			,[TMS]=a.TMS
			,[HourlyStandardOutput]=NULL
			,[WorkHour]=0
			,[QAQty]=a.QAQty
			,[DefectQty]=a.DefectQty
			,[InlineQty]=a.InlineQty
			,[OldDetailKey]=NULL
			,[AutoCreate]=0
			,[SewingReasonID]=''
			,[Remark]=NULL
			INTO #tmp_SewingOutput_Detail
			FROM #tmp_Into_SewingOutput_Detail a
			INNER JOIN #tmp_SewingOutput b ON a.dDate=b.OutputDate AND a.WorkLine  =b.SewingLineID 
		
			--insert SewingOutput_Detail_Detail
			INSERT INTO SewingOutput_Detail_Detail
				([ID], [SewingOutput_DetailUKey],[OrderId],[ComboType],[Article],[SizeCode],[QAQty],[OldDetailKey])
			SELECT 
			[ID]= b.ID
			, [SewingOutput_DetailUKey]=Now_SewingOutput_Detail.ukey
			,a.[OrderId]
			,a.[ComboType]
			,a.[Article]
			,a.[SizeCode]
			,[QAQty] =a.QAQty
			,[OldDetailKey]=NULL
			FROM #tmp_Into_SewingOutput_Detail_Detail a
			INNER JOIN #tmp_SewingOutput_Detail b ON a.OrderID=b.OrderID AND a.ComboType=b.ComboType  AND a.Article=b.Article  
			INNER JOIN SewingOutput sop ON  sop.ID=b.ID AND a.WorkLine=sop.SewingLineID
			OUTER APPLY(
			 SELECT Ukey 
			 FROM SewingOutput s WITH(NOLOCK)
			 INNER JOIN SewingOutput_Detail sd WITH(NOLOCK) ON s.ID=sd.ID
			 WHERE s.ID=sop.ID 
					AND sd.OrderID = b.OrderID  
					AND sd.ComboType=b.ComboType   
					AND sd.Article=b.Article   
					AND s.SewingLineID=sop.SewingLineID
			)Now_SewingOutput_Detail	
			WHERE Now_SewingOutput_Detail.ukey IS NOT NULL	
			And a.QAQty > 0

			-------------Prepare RFT
			select 
			[OrderId] =	CASE WHEN t.MONo LIKE '%-%'
						THEN SUBSTRING(t.MONo, 1, CHARINDEX('-', t.MONo) - 1)
						ELSE t.MONo
						END
			, [CDate] = t.dDate
			, [SewingLineID] = ISNULL(ProductionLineAllocation.SewingLineID, t.WorkLine)
			, [FactoryID] = 'SNP'
			, [InspectQty] =ISNULL( (
							select sum(OutputQty) 
							from  #tOutputTotal tOT
							where 
							SUBSTRING(tOT.MONo, 1, CHARINDEX('-', tOT.MONo) - 1)
							= 
							SUBSTRING(t.MONo, 1, CHARINDEX('-', t.MONo) - 1)
							AND tOT.dDate  = t.dDate  AND tOT.WorkLine  = t.WorkLine 
						),0)

			, [RejectQty] = (
							select count(*) 
							from  #tReworkCount tRT
							where  
							SUBSTRING(tRT.MONo, 1, CHARINDEX('-', tRT.MONo) - 1)
							= 
							SUBSTRING(t.MONo, 1, CHARINDEX('-', t.MONo) - 1)
							AND tRT.dDate=t.dDate AND tRT.WorkLine  = t.WorkLine 
						)
			, [DefectQty] = isnull(sum(Qty), 0)
			, [Shift] = 'D'
			, [Team] = 'A'
			, [Status] = 'Confirmed'
			, [AddName] = 'SCIMIS'
			, [AddDate] = GetDate() 	
			INTO #tmp_Into_RFT
			from #tOutputTotal t
			left join #tReworkTotal mainTable on SUBSTRING(t.MONo, 1, CHARINDEX('-', t.MONo) - 1) = 
												 SUBSTRING(mainTable.MONo, 1, CHARINDEX('-', mainTable.MONo) - 1)
									AND t.dDate  = mainTable.dDate  AND t.WorkLine  = mainTable.WorkLine 
			OUTER APPLY(			
				SELECT t2.SewingLineID
				FROM 
				(
					SELECT TOP 1 *
					FROM ProductionLineAllocation
					WHERE ProductionDate <=CAST( @DateStart AS DATE)
					AND FactoryID='SNP'
					ORDER BY ProductionDate DESC
				) t
				INNER JOIN ProductionLineAllocation_Detail t2 ON t.FactoryID=t2.FactoryID AND t.ProductionDate=t2.ProductionDate
				WHERE t2.LineLocationID= mainTable.WorkLine AND t2.Team='A'
			)ProductionLineAllocation
			where t.dDate = @DateStart
			group by t.MONo, t.dDate, t.WorkLine ,ProductionLineAllocation.SewingLineID
			
			SELECT 
			[OrderID]
			,[CDate]
			,[SewinglineID]
			,[FactoryID]
			,[InspectQty]
			,[RejectQty]
			,[DefectQty]
			,[Shift]
			,[Team]
			,[Status]
			,[Remark]=''
			,[AddName]
			,[AddDate]
			,[EditName]=''
			,[EditDate]=NULL
			,[MDivisionid]=(SELECT TOP 1 ID FROM MDivision)
			INTO #RFT
			FROM #tmp_Into_RFT
			
			SELECT * 
			INTO #DontNeedInsrt
			FROM #tmp_Into_RFT t
			WHERE EXISTS (
				SElECT 1
				FROM Rft
				WHERE OrderID=t.OrderId AND CDate=t.CDate AND SewinglineID = t.SewinglineID AND FactoryID = t.FactoryID AND Shift=t.Shift AND Team=t.Team
			)

			INSERT INTO RFT
				([OrderID],[CDate],[SewinglineID],[FactoryID],[InspectQty],[RejectQty],[DefectQty]
				,[Shift],[Team],[Status],[Remark],[AddName],[AddDate],[EditName],[EditDate],[MDivisionid])
			SELECT 
			[OrderID]
			,[CDate]
			,[SewinglineID]
			,[FactoryID]
			,[InspectQty]
			,[RejectQty]
			,[DefectQty]
			,[Shift]
			,[Team]
			,[Status]
			,[Remark]=''
			,[AddName]
			,[AddDate]
			,[EditName]=''
			,[EditDate]=NULL
			,[MDivisionid]=(SELECT TOP 1 ID FROM MDivision)
			FROM #tmp_Into_RFT t
			WHERE SewingLineID IS NOT NULL
			AND NOT EXISTS (
				SElECT 1
				FROM #DontNeedInsrt
				WHERE OrderID=t.OrderId AND CDate=t.CDate AND SewinglineID = t.SewinglineID AND FactoryID = t.FactoryID AND Shift=t.Shift AND Team=t.Team
			)

			--Prepapre RFT_Detail
			SELECt * 
			INTO #RFT_With_ID
			FROM Production.dbo.RFT t
			WHERE OrderID  IN ( SELECT OrderID FROM #RFT)
			AND CDate = @DateStart
			AND AddDate < GETDATE()
			AND NOT EXISTS (
				SElECT 1
				FROM #DontNeedInsrt
				WHERE OrderID=t.OrderId AND CDate=t.CDate AND SewinglineID = t.SewinglineID AND FactoryID = t.FactoryID AND Shift=t.Shift AND Team=t.Team
			)

			----由於Line可能會切換，因此要把換過去的也算進去
			SELECT 
			[ID] = a.ID
			, [GarmentDefectCodeID] = CASE WHEN  NOT EXISTS (select ID from Production.dbo.GarmentDefectCode  where ID = CAST(FailCode  AS VARCHAR))
									THEN '200'
									ELSE FailCode 
									END
			, [GarmentDefectTypeid] = CASE WHEN NOT EXISTS (select ID from Production.dbo.GarmentDefectCode  where ID  = CAST(FailCode  AS VARCHAR)) 
									THEN '2'
									ELSE  (select GarmentDefectTypeid from Production.dbo.GarmentDefectCode where ID  = CAST(FailCode  AS VARCHAR))
									END
			, [Qty] = Qty 
			INTO #tmp_4
			FROm #RFT_With_ID a
			INNER JOIN #tReworkTotal b
			ON a.[OrderId]  = SUBSTRING(MONo, 1, CHARINDEX('-', MONo) - 1)
			   AND a.[CDate]=b.[dDate] 
			   AND a.[SewingLineID]  = b.workLine 			
			UNION ALL
			SELECT 
			[ID] = a.ID
			, [GarmentDefectCodeID] = CASE WHEN  NOT EXISTS (select ID from Production.dbo.GarmentDefectCode  where ID = CAST(FailCode  AS VARCHAR))
									THEN '200'
									ELSE FailCode 
									END
			, [GarmentDefectTypeid] = CASE WHEN NOT EXISTS (select ID from Production.dbo.GarmentDefectCode  where ID  = CAST(FailCode  AS VARCHAR)) 
									THEN '2'
									ELSE  (select GarmentDefectTypeid from Production.dbo.GarmentDefectCode where ID  = CAST(FailCode  AS VARCHAR))
									END
			, [Qty] = Qty 
			FROM #RFT_With_ID a
			INNER JOIN #tReworkTotal b ON a.[OrderId]  = SUBSTRING(MONo, 1, CHARINDEX('-', MONo) - 1)
										AND a.[CDate]=b.[dDate] 
										AND a.[SewingLineID]  <> b.workLine 
			INNER JOIN (				
				SELECT t2.SewingLineID , t2.LineLocationID ,t2.Team
				FROM 
				(
					SELECT TOP 1 *
					FROM ProductionLineAllocation
					WHERE ProductionDate <=CAST( @DateStart AS DATE)
					AND FactoryID='SNP'
					ORDER BY ProductionDate DESC
				) t
				INNER JOIN ProductionLineAllocation_Detail t2 ON t.FactoryID=t2.FactoryID AND t.ProductionDate=t2.ProductionDate
			)  tt
			ON tt.LineLocationID= b.workLine AND tt.SewingLineID=a.SewinglineID AND tt.Team= a.Team

			--INSERT  RFT_Detail
			INSERT INTO RFT_Detail
				([ID], [GarmentDefectCodeID] , [GarmentDefectTypeid], [Qty])
			SELECT 
					  [ID]
					, [GarmentDefectCodeID] 
					, [GarmentDefectTypeid]
					, [Qty] = Sum(Qty)
			FROm #tmp_4
			GROUP BY ID,[GarmentDefectCodeID],[GarmentDefectTypeid]
		
		------------------------------Masil send----------------------------
		--	SET @mailBody   =   'Transfer Date�G'+ Cast(@DateStart as varchar)
		--						+CHAR(10) + 'Result�GSuccess'
		--						+CHAR(10) 
		--						+CHAR(10) + 'This email is SUNRISEEXCH DB Transfer data to Production DB.'
		--						+CHAR(10) + 'Please do not reply this mail.';
		--	EXEC msdb.dbo.sp_send_dbmail  
		--		@profile_name = 'SUNRISEmailnotice',  
		--		@recipients ='pmshelp@sportscity.com.tw',
		--		@copy_recipients= 'roger.lo@sportscity.com.vn',
		--		@body = @mailBody,  
		--		@subject = 'Daily Hanger system data to PMS - Sewing Output & RFT'; 

		select distinct OutputDate, SewingLineID, Team, FactoryID from #tmp_SewingOutput

	END TRY
	BEGIN CATCH
		EXECUTE [usp_GetErrorString];
	END CATCH

END
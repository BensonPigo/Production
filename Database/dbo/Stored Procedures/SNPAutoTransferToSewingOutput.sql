﻿USE [Production]
GO

/****** Object:  StoredProcedure [dbo].[SNPAutoTransferToSewingOutput]    Script Date: 2019/04/15 �U�� 04:35:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







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
		begin transaction	
			--Declare
			Declare 
			@DateStart date,
		    @mailBody  As VARCHAR(1000)

			set @DateStart = CAST (@execuDatetime AS DATE)


			--Get Data which is already in DB , must exclude it
			
			--#now_Sewing_Data
			SELECT s.OutputDate,s.SewingLineID,sdd.OrderId,sdd.ComboType
			INTO  #now_Sewing_Data
			FROM SewingOutput s WITH(NOLOCK)
			INNER JOIN SewingOutput_Detail sd WITH(NOLOCK) ON s.ID=sd.ID
			INNER JOIN SewingOutput_Detail_Detail sdd WITH(NOLOCK) 
			ON sd.ID=sdd.ID 
					AND sd.UKey=sdd.SewingOutput_DetailUKey
					AND sd.OrderId=sdd.OrderId
					AND sd.ComboType=sdd.ComboType
					AND sd.Article=sdd.Article
			WHERE s.OutputDate=@DateStart
					AND s.[Shift] = 'D'
					AND s.[Team] = 'A'
					AND s.FactoryID='SNP'
					AND s.[MDivisionID]=(SELECT TOP 1 ID FROM MDivision)

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

			--#tOutputTotal
			SELECT a.* 
			INTO #tOutputTotal
			FROM
			(
				SELECT *
				FROM View_tOutputTotal
				WHERE MONo LIKE '%-%' 
			)a
			WHERE LEN((SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=1)) >=10 --Must Same as PMS DB Datatype
			AND  LEN((SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=2)) =1    --Must Same as PMS DB Datatype
			AND dDate = @DateStart
			AND NOT EXISTS (

				SELECT 1 FROM #now_Sewing_Data WHERE 
											OrderId=(SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=1)
											AND SewingLineID  =a.WorkLine  
											AND ComboType  = (SELECT TOP 1 Data FROM SplitString(a.Mono  ,'-') WHERE No=2)
											AND OutputDate = @DateStart
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
			into #tmp_Into_SewingOutput_Detail_Detail_with0
			from #tOutputTotal
			where dDate = @DateStart
			group by dDate,WorkLine,MONo,ColorName,SizeName

			

			SELECt  dDate
					,WorkLine
					,OrderId
					,ComboType
					,Article
					,SizeCode
					,[QAQty]=CASE   WHEN (ByLine.SumQty + AlreadyInPMS.Qty) >= Order_Qty.Qty AND LineCount.Val=1 THEN Order_Qty.Qty
									WHEN (ByLine.SumQty + AlreadyInPMS.Qty) >= Order_Qty.Qty AND LineCount.Val>1 AND t.QAQty=MaxQty.Val THEN (Order_Qty.Qty - AlreadyInPMS.Qty - ByLine.SumQty + ISNULL(t.QAQty,0) )
									ELSE ISNULL(t.QAQty,0)
									END
					,[InlineQty]
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
			)Order_Qty
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
			)AlreadyInPMS
			OUTER APPLY(
				SELECT [SumQty]=SUM(ISNULL(t2.QAQty,0))
				FROM #tmp_Into_SewingOutput_Detail_Detail_with0 t2
				WHERE t.dDate=t2.dDate AND t.OrderId=t2.OrderId AND t.ComboType=t2.ComboType AND t.Article=t2.Article AND t.SizeCode=t2.SizeCode AND t.InlineQty=t2.InlineQty
			)ByLine

			OUTER APPLY(
				SELECT [Val]=MAX(t2.QAQty)
				FROM #tmp_Into_SewingOutput_Detail_Detail_with0 t2
				WHERE t.dDate=t2.dDate AND t.OrderId=t2.OrderId AND t.ComboType=t2.ComboType AND t.Article=t2.Article AND t.SizeCode=t2.SizeCode AND t.InlineQty=t2.InlineQty
			)MaxQty
			OUTER APPLY(
				SELECT [Val]=COUNT(t2.WorkLine)
				FROM #tmp_Into_SewingOutput_Detail_Detail_with0 t2
				WHERE t.dDate=t2.dDate AND t.OrderId=t2.OrderId AND t.ComboType=t2.ComboType AND t.Article=t2.Article AND t.SizeCode=t2.SizeCode AND t.InlineQty=t2.InlineQty
			)LineCount
			
			
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

			

			--Begin Insert

			INSERT INTO SewingOutput 
			(ID ,OutputDate ,SewingLineID ,QAQty ,DefectQty ,InlineQty ,TMS ,Manpower ,ManHour ,Efficiency 
			,Shift ,Team ,Status ,LockDate ,WorkHour ,FactoryID ,MDivisionID , [Category], [SFCData], [AddName] , [AddDate] , [EditName]
			, [EditDate], [SubconOutFty], [SubConOutContractNumber])
			select 	
			  [ID]
			, [OutputDate]=CAST([OutputDate] AS DATE)
			, [SewingLineID]=ISNULL(ProductionLineAllocation.SewingLineID,ts.[SewingLineID])
			, [QAQty]
			, [DefectQty]
			, [InlineQty]
			, [TMS]
			, [Manpower]=0
			, [Manhour]=0
			, [Efficiency]=0
			, [Shift]
			, [Team]
			, [Status] 
			, [LockDate]=NULL
			, [Workhour] 
			, [FactoryID]
			, [MDivision]
			, [Category]
			, [SFCData]
			, [AddName] 
			, [AddDate] 
			, [EditName]=''
			, [EditDate]=CAST([EditDate] AS DATETIME)
			, [SubconOutFty]
			, [SubConOutContractNumber]
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
			SET t.TMS= t2.TMS
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
			[OrderId] =	CASE WHEN MONo LIKE '%-%'
						THEN SUBSTRING(MONo, 1, CHARINDEX('-', MONo) - 1)
						ELSE MONo
						END
			, [CDate] = dDate
			, [SewingLineID] = ISNULL(ProductionLineAllocation.SewingLineID, WorkLine)
			, [FactoryID] = 'SNP'
			, [InspectQty] =ISNULL( (
							select sum(OutputQty) 
							from  #tOutputTotal tOT
							where 
							SUBSTRING(tOT.MONo, 1, CHARINDEX('-', tOT.MONo) - 1)
							= 
							SUBSTRING(mainTable.MONo, 1, CHARINDEX('-', mainTable.MONo) - 1)
							AND tOT.dDate  = mainTable.dDate  AND tOT.WorkLine  = mainTable.WorkLine 
						),0)

			, [RejectQty] = (
							select count(*) 
							from  #tReworkCount tRT
							where  
							SUBSTRING(tRT.MONo, 1, CHARINDEX('-', tRT.MONo) - 1)
							= 
							SUBSTRING(mainTable.MONo, 1, CHARINDEX('-', mainTable.MONo) - 1)
							AND tRT.dDate=mainTable.dDate AND tRT.WorkLine  = mainTable.WorkLine 
						)
			, [DefectQty] = sum(Qty)
			, [Shift] = 'D'
			, [Team] = 'A'
			, [Status] = 'Confirmed'
			, [AddName] = 'SCIMIS'
			, [AddDate] = GetDate() 
	
			INTO #tmp_Into_RFT
			from #tReworkTotal mainTable
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
			where dDate = @DateStart
			group by MONo, dDate, WorkLine ,ProductionLineAllocation.SewingLineID
			
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
			FROM #tmp_Into_RFT
			WHERE SewingLineID IS NOT NULL

			--Prepapre RFT_Detail
			SELECt * 
			INTO #RFT_With_ID
			FROM Production.dbo.RFT
			WHERE OrderID  IN ( SELECT OrderID FROM #RFT)
			AND CDate = @DateStart
			AND AddDate < GETDATE()

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
		commit transaction
	END TRY
	BEGIN CATCH
		EXECUTE [usp_GetErrorString];
		--Prepare Mail
		--DECLARE @ErrorMessage As VARCHAR(1000) = CHAR(10)+'Error Code�G' +CAST(ERROR_NUMBER() AS VARCHAR)
		--										+CHAR(10)+'Error Message�G'+	ERROR_MESSAGE()
		--										+CHAR(10)+'Line�G'+	CAST(ERROR_LINE() AS VARCHAR)
		--										+CHAR(10)+'Procedure Name�G'+	ISNULL(ERROR_PROCEDURE(),'')
		--DECLARE @ErrorSeverity As Numeric = ERROR_SEVERITY()
		--DECLARE @ErrorState As Numeric = ERROR_STATE()
		
		--PRINT @ErrorMessage

		--SET @mailBody   =   'Transfer Date�G'+ Cast( Cast(@execuDatetime as Date) as Varchar)
		--					+CHAR(10) + 'Result�GError'
		--					+CHAR(10) 
		--					+ @ErrorMessage
		--					+CHAR(10) 
		--					+CHAR(10) + 'This email is SUNRISEEXCH DB Transfer data to Production DB.'
		--					+CHAR(10) + 'Please do not reply this mail.';
		IF @@TRANCOUNT > 0  
        ROLLBACK TRANSACTION;
				
		----------------------------Masil send----------------------------
		--EXEC msdb.dbo.sp_send_dbmail  
		--@profile_name = 'SUNRISEmailnotice',  
		--@recipients ='pmshelp@sportscity.com.tw',
		--@copy_recipients= 'roger.lo@sportscity.com.vn',
		--@body = @mailBody,  
		--@subject = 'Daily Hanger system data to PMS - Sewing Output & RFT'; 
	END CATCH


	--IF @@TRANCOUNT > 0  
	--	COMMIT TRANSACTION;  
END
Go

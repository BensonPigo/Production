USE [Production]
GO

/****** Object:  StoredProcedure [dbo].[SNPAutoTransferToSewingOutput]    Script Date: 2019/03/12 と 05:35:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Benson
-- Create date: 2019/03/11
-- Description:	Get Hanger Output Data and Insert into SewingOutputRFT 眔本╰参讽ら玻糶SewingOutputSewingOutputい
-- =============================================
CREATE PROCEDURE [dbo].[SNPAutoTransferToSewingOutput]
(	
	@execuDatetime as Datetime
)
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRANSACTION
	BEGIN TRY

			--Declare
			declare 
			@DateStart date,		
			@DateEnd date,

			@SewingOutput_ID varchar(13),
			@SewingOutput_Detail_Last_UKey bigint ,
			@RFT_Last_ID bigint,
			@SewingOutput_Count int,
		    @mailBody  As VARCHAR(1000)

			set @DateStart = CAST (@execuDatetime AS DATE)
			set @DateEnd =  CAST (@execuDatetime AS DATE)


			--Get Data which is already in DB , must exclude it
			SELECT s.OutputDate,s.SewingLineID,sdd.OrderId,sdd.ComboType
			INTO  #no_Sewing_Data
			FROM SewingOutput s
			INNER JOIN SewingOutput_Detail sd ON s.ID=sd.ID
			INNER JOIN SewingOutput_Detail_Detail sdd 
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

			SELECt r.OrderID,r.CDate,r.SewinglineID
			INTO #no_RFT_Data
			FROM RFT r
			INNER JOIN Rft_Detail rd ON r.ID=rd.ID
			WHERE CDate=@DateStart
					AND r.[Shift] = 'D'
					AND r.[Team] = 'A'
					AND r.FactoryID='SNP'
					AND r.[MDivisionID]=(SELECT TOP 1 ID FROM MDivision)
					AND r.AddName='SCIMIS'

			--#tOutputTotal
			SELECT a.* 
			INTO #tOutputTotal
			FROM
			(
				SELECT *
				FROM [testing\ph2].SUNRISEEXCH.dbo.tOutputTotal
				WHERE MONo LIKE '%-%' 
			)a
			WHERE LEN((SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=1)) >=10 --Must Same as PMS DB Datatype
			AND  LEN((SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=2)) =1    --Must Same as PMS DB Datatype
			AND dDate = @DateStart
			AND NOT EXISTS (

				SELECT 1 FROM #no_Sewing_Data WHERE 
											OrderId=(SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=1)
											AND SewingLineID=a.WorkLine
											AND ComboType = (SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=2)
											AND OutputDate = @DateStart
			)

			--#tReworkTotal
			SELECT a.* 
			INTO #tReworkTotal
			FROM
			(
				SELECT *
				FROM [testing\ph2].SUNRISEEXCH.dbo.tReworkTotal
				WHERE MONo LIKE '%-%' 
			)a
			WHERE LEN((SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=1)) >=10 --Must Same as PMS DB Datatype
			AND  LEN((SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=2)) =1    --Must Same as PMS DB Datatype
			AND dDate = @DateStart
			AND NOT EXISTS (

				SELECT 1 FROM #no_RFT_Data WHERE 
											CDate = @DateStart
											AND OrderId=(SELECT TOP 1 Data FROM SplitString(a.Mono,'-') WHERE No=1)
											AND SewingLineID=a.WorkLine
			)

			--Prepare SewingOutput_Detail_Detail	
			SELECT 
				[RowNumber]=row_number()OVER (ORDER BY dDate)
				,dDate
				,WorkLine
				,[OrderId]= CASE WHEN MONo LIKE '%-%'
							THEN SUBSTRING(MONo, 1, CHARINDEX('-', MONo) - 1)
							ELSE MONo
							END
				,[ComboType]=   CASE WHEN MONo LIKE '%-%' 
								THEN  SUBSTRING(MONo, CHARINDEX('-', MONo) + 1, LEN(MONo) - CHARINDEX('-', MONo)) 
								ELSE ''--MONo
								END
				,ColorName as Article, SizeName as SizeCode
				,[QAQty]=sum(OutputQty) 
			into #tmp_Into_SewingOutput_Detail_Detail
			from #tOutputTotal
			where dDate = @DateStart
			group by dDate,WorkLine,MONo,ColorName,SizeName

			--Prepare SewingOutput_Detail	
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
			, [ア毖Ω计] = count(*) 
			into #tempFail
			from [testing\ph2].SUNRISEEXCH.dbo.tReworkCount
			where dDate =@DateStart
			group by MONo, WorkLine

			select 
			t3.RowNumber
			, dDate
			, t3.WorkLine
			, t3.OrderId
			, t3.ComboType
			, Article
			,[QAQty]= Sum(QAQty) 
			,[InlineQty]= sum(ア毖Ω计) + Sum(QAQty) 
			,[DefectQty]= (sum(ア毖Ω计) + Sum(QAQty)) - Sum(QAQty)  
			into #tmp_Into_SewingOutput_Detail
			from #tmp_Into_SewingOutput_Detail_Detail t3
			left join #tempFail t on t.OrderId = t3.OrderId AND t.ComboType=t3.ComboType and t.WorkLine = t3.WorkLine
			group by t3.RowNumber,dDate,t3.WorkLine, t3.OrderId, t3.ComboType, Article

			--Begin INSERTSewingOutput first
	
			--insert SewingOutput
			select 
			[ID]= dbo.GetSewingOutputID_For_SNPAutoTransferToSewingOutput('SNPSM',RowNumber,@execuDatetime)
			,RowNumber
			, [OutputDate]=dDate 
			, [SewingLineID]=WorkLine 
			, [QAQty]= Sum(QAQty) 
			, [DefectQty]= sum(DefectQty) 
			, [InlineQty]= sum(InlineQty) 
			, [TMS]=0
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
			group by RowNumber,dDate,WorkLine	

			INSERT INTO SewingOutput
			select 
			[ID]= dbo.GetSewingOutputID_For_SNPAutoTransferToSewingOutput('SNPSM',RowNumber,@execuDatetime)
			, [OutputDate]=dDate 
			, [SewingLineID]=WorkLine 
			, [QAQty]= Sum(QAQty) 
			, [DefectQty]= sum(DefectQty) 
			, [InlineQty]= sum(InlineQty) 
			, [TMS]=0
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
			from #tmp_Into_SewingOutput_Detail
			group by RowNumber,dDate,WorkLine	
		
			--insert SewingOutput_Detail
			INSERT INTO SewingOutput_Detail
			SELECT 
			[ID]= b.ID
			,[OrderId]
			,[ComboType]
			,[Article]
			,[Color]=(SELECT TOP 1 ColorID FROM View_OrderFAColor WHERE ID=a.OrderId)
			,[TMS]=0
			,[HourlyStandardOutput]=NULL
			,[WorkHour]=0
			,[QAQty]=a.QAQty
			,[DefectQty]=a.DefectQty
			,[InlineQty]=a.InlineQty
			,[OldDetailKey]=NULL
			,[AutoCreate]=0
			,[SewingReasonID]=''
			,[Remark]=NULL
			FROM #tmp_Into_SewingOutput_Detail a
			INNER JOIN #tmp_1 b ON a.RowNumber=b.RowNumber
				
			--Get SewingOutput_DetailUKey ID
			SET @SewingOutput_Detail_Last_UKey =( SELECT IDENT_CURRENT( 'SewingOutput_Detail' )  )
		
			--insert SewingOutput_Detail_Detail
			INSERT INTO SewingOutput_Detail_Detail
			SELECT 
			[ID]= b.ID
			,[SewingOutput_DetailUKey]= @SewingOutput_Detail_Last_UKey - (SELECT Count (*) FROM #tmp_Into_SewingOutput_Detail_Detail) +a.RowNumber
			,[OrderId]
			,[ComboType]
			,[Article]
			,[SizeCode]
			,[QAQty] =a.QAQty
			,[OldDetailKey]=NULL
			FROM #tmp_Into_SewingOutput_Detail_Detail a
			INNER JOIN #tmp_1 b ON a.RowNumber=b.RowNumber
	
			-------------Prepare RFT
			select 
			[OrderId] =	CASE WHEN MONo LIKE '%-%'
					THEN SUBSTRING(MONo, 1, CHARINDEX('-', MONo) - 1)
					ELSE MONo
					END
			, [CDate] = dDate
			, [SewingLineID] = WorkLine
			, [FactoryID] = 'SNP'
			, [InspectQty] = (
							select sum(OutputQty) 
							from  #tOutputTotal tOT
							where 
							SUBSTRING(tOT.MONo, 1, CHARINDEX('-', tOT.MONo) - 1)
							= 
							SUBSTRING(MONo, 1, CHARINDEX('-', mainTable.MONo) - 1)
							AND tOT.dDate  = mainTable.dDate  AND tOT.WorkLine = mainTable.WorkLine
						)

			, [RejectQty] = (
							select count(*) 
							from  [testing\ph2].SUNRISEEXCH.dbo.tReworkCount tRT
							where  
							SUBSTRING(tRT.MONo, 1, CHARINDEX('-', tRT.MONo) - 1)
							= 
							SUBSTRING(MONo, 1, CHARINDEX('-', mainTable.MONo) - 1)
							AND tRT.dDate=mainTable.dDate AND tRT.WorkLine=mainTable.WorkLine
						)
			, [DefectQty] = sum(Qty)
			, [Shift] = 'D'
			, [Team] = 'A'
			, [Status] = 'Confirmed'
			, [AddName] = 'SCIMIS'
			, [AddDate] = GetDate() 
	
			INTO #tmp_Into_RFT
			from #tReworkTotal mainTable
			where dDate = @DateStart
			group by MONo, dDate, WorkLine
			
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

			--Prepapre RFT_Detail
			SELECt * 
			INTO #RFT_With_ID
			FROM Production.dbo.RFT
			WHERE OrderID IN ( SELECT OrderID FROM #RFT)
			AND AddDate > @DateStart

			SELECT 
			[ID] = a.ID
			, [GarmentDefectCodeID] = CASE WHEN  NOT EXISTS (select ID from Production.dbo.GarmentDefectCode  where ID = CAST(0 AS VARCHAR))
									THEN '200'
									ELSE FailCode
									END
			, [GarmentDefectTypeid] = CASE WHEN NOT EXISTS (select ID from Production.dbo.GarmentDefectCode  where ID = CAST(0 AS VARCHAR)) 
									THEN '2'
									ELSE  (select ID from Production.dbo.GarmentDefectCode where ID = CAST(FailCode AS VARCHAR))
									END
			, [Qty] = Qty
			INTO #tmp_2
			FROm #RFT_With_ID a
			INNER JOIN #tReworkTotal b
			ON a.[OrderId]=SUBSTRING(MONo, 1, CHARINDEX('-', MONo) - 1)
			   AND a.[CDate]=b.[dDate] 
			   AND a.[SewingLineID]=b.workLine

			--INSERT  RFT_Detail
			INSERT INTO RFT_Detail
			SELECT 
					  [ID]
					, [GarmentDefectCodeID] 
					, [GarmentDefectTypeid]
					, [Qty] = Sum(Qty)
			FROm #tmp_2
			GROUP BY ID,[GarmentDefectCodeID],[GarmentDefectTypeid]
		
		COMMIT TRANSACTION;
		
		----------------------------Masil send----------------------------
			SET @mailBody   =   'Transfer Date'+ Cast(@DateStart as varchar)
								+CHAR(10) + 'ResultSuccess'
								+CHAR(10) 
								+CHAR(10) + 'This email is SUNRISEEXCH DB Transfer data to Production DB.'
								+CHAR(10) + 'Please do not reply this mail.';
			EXEC msdb.dbo.sp_send_dbmail  
				@profile_name = 'SUNRISEmailnotice',  
				@recipients = 'pmshelp@sportscity.com.tw',
				@copy_recipients= 'roger.lo@sportscity.com.vn',  
				@body = @mailBody,  
				@subject = 'Daily Hanger system data to PMS - Sewing Output & RFT'; 
	END TRY

	BEGIN CATCH

		--Prepare Mail
		DECLARE @ErrorMessage As VARCHAR(1000) = CHAR(10)+'Error Code' +CAST(ERROR_NUMBER() AS VARCHAR)
												+CHAR(10)+'Error Message'+	ERROR_MESSAGE()
												+CHAR(10)+'Line'+	CAST(ERROR_LINE() AS VARCHAR)
												+CHAR(10)+'Procedure Name'+	ISNULL(ERROR_PROCEDURE(),'')
		DECLARE @ErrorSeverity As Numeric = ERROR_SEVERITY()
		DECLARE @ErrorState As Numeric = ERROR_STATE()
		
		SET @mailBody   =   'Transfer Date'+ Cast( Cast(@execuDatetime as Date) as Varchar)
							+CHAR(10) + 'ResultError'
							+CHAR(10) 
							+ @ErrorMessage
							+CHAR(10) 
							+CHAR(10) + 'This email is SUNRISEEXCH DB Transfer data to Production DB.'
							+CHAR(10) + 'Please do not reply this mail.';

		ROLLBACK TRANSACTION;
				
		----------------------------Masil send----------------------------
		EXEC msdb.dbo.sp_send_dbmail  
		@profile_name = 'SUNRISEmailnotice',  
		@recipients ='pmshelp@sportscity.com.tw',
		@copy_recipients= 'roger.lo@sportscity.com.vn',
		@body = @mailBody,  
		@subject = 'Daily Hanger system data to PMS - Sewing Output & RFT'; 
	

	END CATCH

END

GO



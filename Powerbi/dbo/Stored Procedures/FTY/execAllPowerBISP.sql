﻿Create PROCEDURE [dbo].[execAllPowerBISP]
	
AS
BEGIN

SET NOCOUNT ON;

/*判斷當前Server後, 指定帶入正式機Server名稱*/
DECLARE @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)
--依不同Server來抓到對應的備機ServerName
DECLARE @Region nvarchar(10) 

select @Region = REPLACE(RgCode, 'PHI', 'PH1')
from Production.dbo.System

DECLARE @M nvarchar(10) 
= (
	select [value] = 
		CASE @Region WHEN 'PH1' THEN 'PH1' -- PH1
				WHEN 'PH2' THEN 'PH2'					 -- PH2
				WHEN 'ESP' THEN 'VM2'					 -- ESP
				WHEN 'SPT' THEN 'VM1'						 -- SPT
				WHEN 'SNP' THEN 'VM3'					 -- SNP
				WHEN 'SPS' THEN 'KM2'						 -- SPS
				WHEN 'SPR' THEN 'KM1'							 -- SPR
				WHEN 'HZG' THEN 'CM2'					 -- HZG		
				WHEN 'HXG' THEN 'CM1'							 -- HXG				
		ELSE '' END
)

select M= @M, Region =@Region

DECLARE @desc nvarchar(4000)  = '';
DECLARE @ErrorMessage NVARCHAR(4000) = '';
DECLARE @ErrorStatus bit = 1;
DECLARE @StartTime datetime = getdate();
DECLARE @StartDate date
DECLARE @EndDate date
DECLARE @TransCode varchar(100) = (select format(getdate(),'yyyyMMdd_HHmmss'))
DECLARE @ErrDesc NVARCHAR(4000) = '';

DECLARE @Stime datetime, @Etime datetime

--02) P_Import_EfficiencyBI 
BEGIN TRY
	set @Stime = getdate()
	set @StartDate = CAST(DATEADD(day,-60, GETDATE()) AS date)
	set @EndDate   = CAST(GETDATE() AS date)
	EXEC P_Import_EfficiencyBI @StartDate	,@EndDate
	set @Etime = getdate()
END TRY
BEGIN CATCH
SET @ErrorMessage = 
'
[2-Production Efficiency]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

	SET @ErrorStatus = 0
END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) +'
[2-Production Efficiency] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_EfficiencyBI',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

--04) P_ImportOustandingPO_Fty、P_ImportSDPOrderDetail
BEGIN TRY
	set @Stime = getdate()
	set @StartDate = CAST(DATEADD(day,-150, GETDATE()) AS date)
	set @EndDate   = CAST(DATEADD(day,60, GETDATE()) AS date)
	execute [dbo].[P_ImportOustandingPO_Fty] @StartDate, @EndDate
	--DECLARE @BuyerDelivery_s as Date = '2020/01/01'
	--DECLARE @BuyerDelivery_e as Date = '2020/07/31'
	--execute [dbo].[P_ImportSDPOrderDetail] @BuyerDelivery_s--,@BuyerDelivery_e
	--set @Etime = getdate()
END TRY

BEGIN CATCH
SET @ErrorMessage = 
'
[4-Outstanding PO]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) +'
[4-Outstanding PO] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
BEGIN
	set @desc +=  CHAR(13) +@ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_ImportOustandingPO_Fty',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

--05) P_Import_LoadingProductionOutput
BEGIN TRY
	declare @P_Import_LoadingProductionOutput_UseYear varchar(4) = (select YEAR(GETDATE()))
	set @Stime = getdate()
	execute [dbo].[P_Import_LoadingProductionOutput] @P_Import_LoadingProductionOutput_UseYear
	set @Etime = getdate()
END TRY

BEGIN CATCH
SET @ErrorMessage =
'
[5-LoadingProductionOutput]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) +'
[5-LoadingProductionOutput] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
BEGIN
	set @desc +=  CHAR(13) +@ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_LoadingProductionOutput',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

--06) P_ImportQAInspection_Fty
BEGIN TRY
	set @Stime = getdate()
	set @StartDate = '2020-01-01'
	EXEC P_ImportQAInspection_Fty @StartDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[6-P_ImportQAInspection]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[6-P_ImportQAInspection] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_ImportQAInspection_Fty',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

--08-1) P_Import_QA_P09
BEGIN TRY
	set @Stime = getdate()
	set @StartDate = dateadd(month,-6, dateadd(day, -datepart(day, getdate())+1 ,convert(date, getdate())))
	set @EndDate = (select CONVERT(date, DATEADD(MONTH,3, GETDATE())))
	EXEC P_Import_QA_P09_Fty @StartDate,@EndDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[8-P_Import_QA_P09]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[8-P_Import_QA_P09] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_QA_P09',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''
/****************************************************************************************************************************/

--08-2) P_Import_QA_R06
BEGIN TRY
	set @Stime = getdate()  
	set @Etime = getdate()
	set @StartDate = dateadd(month,-6, dateadd(day, -datepart(day, getdate())+1 ,convert(date, getdate())))
	set @EndDate = (SELECT dateadd(day ,-1, dateadd(m, datediff(m,0,getdate())+1,0))  )
	execute [dbo].[P_Import_QA_R06] @StartDate,@EndDate
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[8-P_Import_QA_R06]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[8-P_Import_QA_R06] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_QA_R06',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/

--Sunday Job) P_ImportAdiCompReport

if (select DATEPART(WEEKDAY,GETDATE())) = 1
BEGIN
	BEGIN TRY
		set @Stime = getdate()
		EXEC P_ImportAdiCompReport
		set @Etime = getdate()
	END TRY

	BEGIN CATCH

	SET @ErrorMessage = 
	'
	[Sunday_Job-P_ImportAdiCompReport]' + CHAR(13) +
	',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
	',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
	',錯誤訊息: ' + ERROR_MESSAGE()

	SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
	',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
	',錯誤訊息: ' + ERROR_MESSAGE()

	SET @ErrorStatus = 0

	END CATCH;


	IF (@ErrorMessage IS NULL or @ErrorMessage='')
	BEGIN 
		set @desc += CHAR(13) + '
	[Sunday-Job-P_ImportAdiCompReport] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
	END
	ELSE
	BEGIN
		set @desc += CHAR(13) + @ErrorMessage
	END
	SET @ErrorMessage = ''

	-- Write in P_TransLog
		insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
		values('P_ImportAdiCompReport',@ErrDesc,@Stime,@Etime,@TransCode)

		SET @ErrDesc = ''
END

/****************************************************************************************************************************/

--P_SubProInsReport
BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].[P_ImportSubProInsReport]
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[10-P_SubProInsReport]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[10-P_SubProInsReport] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_SubProInsReport',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''


/****************************************************************************************************************************/
/***********************************P_ImportSDP****************************************************************/
BEGIN TRY
	set @StartDate = CONVERT(date, DATEADD(DAY,-60,GETDATE()))
	set @EndDate = GETDATE()
	set @Stime = getdate()  
	execute [dbo].[P_ImportSDP] @StartDate, @EndDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[11-P_ImportSDP]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[11-P_ImportSDP] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_ImportSDP',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''
/****************************************************************************************************************************/
/***********************************P_ImportLoadingvsCapacity****************************************************************/

BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].[P_ImportLoadingvsCapacity]
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[12-P_ImportLoadingvsCapacity]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[12-P_ImportLoadingvsCapacity] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_ImportLoadingvsCapacity',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''


/****************************************************************************************************************************/
/***********************************P_PPICMasterList_ArtworkType****************************************************************/

BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].[P_Import_PPIC_MASTER_LIST_ArtWorkType]
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[13-P_PPICMasterList_ArtworkType]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[13-P_PPICMasterList_ArtworkType] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_PPICMasterList_ArtworkType',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''
	
/****************************************************************************************************************************/
/***********************************P_PPICMASTERLIST****************************************************************/
BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].[P_Import_PPICMasterList]
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[14-P_Import_PPIC_MASTER_LIST]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[14-P_Import_PPIC_MASTER_LIST] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_PPIC_MASTER_LIST',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_Import_MtlStatusAnalisis****************************************************************/
BEGIN TRY
	set @StartDate = CONVERT(date, DATEADD(DAY,-60,GETDATE()))
	set @EndDate = GETDATE()
	set @Stime = getdate()  
	execute [dbo].[P_Import_MtlStatusAnalisis] @StartDate,@EndDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[15-P_Import_MtlStatusAnalisis]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[15-P_Import_MtlStatusAnalisis] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_MtlStatusAnalisis',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_ImportADIDASComplain_24Month****************************************************************/
BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].[P_ImportADIDASComplain_24Month]
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[17-P_ImportADIDASComplain_24Month]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[17-P_ImportADIDASComplain_24Month] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_ImportADIDASComplain_24Month',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_Import_MtltoFTYAnalysis****************************************************************/
BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].[P_Import_MtltoFTYAnalysis]
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[18-P_Import_MtltoFTYAnalysis]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[18-P_Import_MtltoFTYAnalysis] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_MtltoFTYAnalysis',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/****************************P_Import_FabricStatus_And_IssueFabricTracking**********************************************************/
BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].P_Import_FabricStatus_And_IssueFabricTracking
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[20 P_Import_FabricStatus_And_IssueFabricTracking]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[20-P_Import_FabricStatus_And_IssueFabricTracking] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_FabricStatus_And_IssueFabricTracking',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_Import_ActualCutOutputReport****************************************************************/

BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].P_Import_ActualCutOutputReport
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[21-P_Import_ActualCutOutputReport]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[21-P_Import_ActualCutOutputReport] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_ActualCutOutputReport',@ErrDesc,@Stime,@Etime,@TransCode)
	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***************************************P_Import_SubprocessWIP**********************************************************************/
BEGIN TRY
	Declare @StartDate_P_Import_SubprocessWIP date = dateadd(DAY, -7, getdate())

	set @Stime = getdate()  
	execute [dbo].P_Import_SubprocessWIP @StartDate_P_Import_SubprocessWIP
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[22-P_Import_SubprocessWIP]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[22-P_Import_SubprocessWIP] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_SubprocessWIP',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/*******************************************************P_Import_WIP****************************************************************/
BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].P_Import_WIP
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[23-P_Import_WIP]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[23-P_Import_WIP] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_WIP',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_Import_QA_R08_Detail****************************************************************/
BEGIN TRY
	Declare @P_Import_QA_R08_Detail_StartDate date = dateadd(MONTH, -3, getdate())
	Declare @P_Import_QA_R08_Detail_EndDate date = getdate()
	
	set @Stime = getdate()  
	execute [dbo].[P_Import_QA_R08_Detail] @P_Import_QA_R08_Detail_StartDate, @P_Import_QA_R08_Detail_EndDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[24-P_Import_QA_R08_Detail]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[24-P_Import_QA_R08_Detail] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_QA_R08_Detail',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_Import_FabricInspLabSummaryReport****************************************************************/
BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].[P_Import_FabricInspLabSummaryReport]
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[25-P_Import_FabricInspLabSummaryReport]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[25-P_Import_FabricInspLabSummaryReport] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_FabricInspLabSummaryReport',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_ImportCuttingBCS****************************************************************/
BEGIN TRY
	set @Stime = getdate()  
	execute [dbo].[P_ImportCuttingBCS]
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[26-P_ImportCuttingBCS]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[26-P_ImportCuttingBCS] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_ImportCuttingBCS',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************[P_Import_ICRAnalysis]****************************************************************/
BEGIN TRY
	set @StartDate = dateadd(DAY, -7, getdate())
	set @EndDate = getdate()
	set @Stime = getdate()
	execute [dbo].[P_Import_ICRAnalysis] @StartDate,@EndDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[29-P_Import_ICRAnalysis]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[29-P_Import_ICRAnalysis] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_ICRAnalysis',@ErrDesc,@Stime,@Etime,@TransCode)
	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_Import_WH_R16****************************************************************/
BEGIN TRY
	Declare @P_Import_WH_R16_EndDate date = getdate()
	Declare @P_Import_WH_R16_StartDate date = dateadd(DAY, -30, @P_Import_WH_R16_EndDate)
	set @Stime = getdate()  
	execute [dbo].[P_Import_WH_R16] @P_Import_WH_R16_StartDate, @P_Import_WH_R16_EndDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[31-P_Import_WH_R16]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[31-P_Import_WH_R16] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_WH_R16',@ErrDesc,@Stime,@Etime,@TransCode)
	SET @ErrDesc = ''
	
/****************************************************************************************************************************/
/***********************************P_Import_WH_R25****************************************************************/
BEGIN TRY
	Declare @P_Import_WH_R25_EndDate date = getdate()
	Declare @P_Import_WH_R25_StartDate date = dateadd(DAY, -90, @P_Import_WH_R25_EndDate)
	set @Stime = getdate()  
	execute [dbo].[P_Import_WH_R25] @P_Import_WH_R25_StartDate, @P_Import_WH_R25_EndDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[31-P_Import_WH_R25]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[31-P_Import_WH_R25] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_WH_R25',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_Import_AccessoryInspLabStatus****************************************************************/
BEGIN TRY
	Declare @P_Import_AccessoryInspLabStatus_EndDate date = getdate()
	Declare @P_Import_AccessoryInspLabStatus_StartDate date = dateadd(DAY, -90, @P_Import_WH_R25_EndDate)
	set @Stime = getdate()  
	execute [dbo].[P_Import_AccessoryInspLabStatus] @P_Import_AccessoryInspLabStatus_StartDate, @P_Import_AccessoryInspLabStatus_EndDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[32-P_Import_AccessoryInspLabStatus]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[32-P_Import_AccessoryInspLabStatus] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_AccessoryInspLabStatus',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_Import_ProdEfficiencyByFactorySewingLine****************************************************************/
BEGIN TRY
	set @StartDate = DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 1, 0)
	set @Stime = getdate()  
	execute [dbo].[P_Import_ProdEfficiencyByFactorySewingLine] @StartDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[33-P_Import_ProdEfficiencyByFactorySewingLine]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[33-P_Import_ProdEfficiencyByFactorySewingLine] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_ProdEfficiencyByFactorySewingLine',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
/***********************************P_Import_StyleChangeover****************************************************************/
BEGIN TRY
	set @StartDate = DATEADD(MONTH,-6,GETDATE())
	set @Stime = getdate()  
	execute [dbo].[P_Import_StyleChangeover] @StartDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[34-P_Import_StyleChangeover]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrDesc = '錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE())  +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;
IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[34-P_Import_StyleChangeover] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('P_Import_StyleChangeover',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

/****************************************************************************************************************************/
DECLARE @comboDesc nvarchar(4000);

	set @comboDesc = '
Please check below information.
Transfer date: '+ (select convert(nvarchar(30), convert(date,getdate()))) +
'
M: '+@Region+'
' + @desc


DECLARE @mailserver nvarchar(60)
DECLARE @eMailID nvarchar(40)
DECLARE @eMailPwd nvarchar(20)
DECLARE @sendFrom nvarchar(100)
DECLARE @toAddress nvarchar(500)

SELECT @mailserver = Mailserver 
,@eMailID = EmailID
,@eMailPwd = EmailPwd
,@sendFrom = Sendfrom
FROM Production.dbo.System

select @toAddress = ToAddress from Production.dbo.MailTo where id = '101'
DECLARE @EndTime datetime = getdate()

exec callJobLog_SP @mailserver,@eMailID,@eMailPwd,@sendFrom,@toAddress,'Import BI Data',@comboDesc,0,@ErrorStatus,0,'Power BI',@Region,@M,@StartTime,@EndTime


END
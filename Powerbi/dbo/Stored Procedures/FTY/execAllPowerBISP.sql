-- =============================================
-- Create date: 2020/03/20
-- Description:	Execeudte all SP for PowerBI Report Job in Fty server
-- =============================================
Create PROCEDURE [dbo].[execAllPowerBISP]
	
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

--01) ImportForecastLoadingBI
BEGIN TRY
	set @Stime = getdate()
	set @StartDate = '2019-01-08'
	set @EndDate = DATEADD(m, DATEDIFF(m,0,DATEADD(MM,5,GETDATE())),6)
	EXEC ImportForecastLoadingBI @StartDate,@EndDate
	set @Etime = getdate()
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
[1-Forecast Loading]' + CHAR(13) +
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
[1-Forecast Loading] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('ImportForecastLoadingBI',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

--02) ImportEfficiencyBI 
BEGIN TRY
	set @Stime = getdate()
	set @StartDate = CAST(DATEADD(day,-60, GETDATE()) AS date)
	set @EndDate   = CAST(GETDATE() AS date)
	EXEC ImportEfficiencyBI @StartDate	,@EndDate
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
	values('ImportEfficiencyBI',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

--03) ImportSewingLineScheduleBIData
BEGIN TRY
	set @Stime = getdate()
	set @StartDate = CAST(DATEADD(day,-60, GETDATE()) AS date)
	set @EndDate   = CAST(DATEADD(day,120, GETDATE()) AS date)
	execute [dbo].[ImportSewingLineScheduleBIData] @StartDate,@EndDate
	set @Etime = getdate()
END TRY

BEGIN CATCH
SET @ErrorMessage = 
'
[3-Production Sewing Line Schedule]' + CHAR(13) +
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
[3-Production Sewing Line Schedule] is completed' + ' Time:' + FORMAT(@Stime, 'yyyy/MM/dd HH:mm:ss') + ' - ' + FORMAT(@Etime, 'yyyy/MM/dd HH:mm:ss')
END
BEGIN
	set @desc +=  CHAR(13) +@ErrorMessage
END
SET @ErrorMessage = ''

-- Write in P_TransLog
	insert into P_TransLog(functionName,Description,StartTime,EndTime,TransCode) 
	values('ImportSewingLineScheduleBIData',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

--04) P_ImportOustandingPO_Fty、P_ImportSDPOrderDetail
BEGIN TRY
	set @Stime = getdate()
	set @StartDate = CAST(DATEADD(day,-150, GETDATE()) AS date)
	set @EndDate   = CAST(DATEADD(day,30, GETDATE()) AS date)
	execute [dbo].[P_ImportOustandingPO_Fty] @StartDate,@EndDate
	DECLARE @BuyerDelivery_s as Date = '2020/01/01'
	--DECLARE @BuyerDelivery_e as Date = '2020/07/31'
	execute [dbo].[P_ImportSDPOrderDetail] @BuyerDelivery_s--,@BuyerDelivery_e
	set @Etime = getdate()
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
	values('P_ImportOustandingPO_Fty & P_ImportSDPOrderDetail',@ErrDesc,@Stime,@Etime,@TransCode)

	SET @ErrDesc = ''

--05) P_ImportLoadingProductionOutput_FTY
BEGIN TRY
	set @Stime = getdate()
	execute [dbo].[P_ImportLoadingProductionOutput_FTY]
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
	values('P_ImportLoadingProductionOutput_FTY',@ErrDesc,@Stime,@Etime,@TransCode)

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
,@toAddress = 'willy.wei@sportscity.com.tw'
FROM Production.dbo.System

--select @toAddress = ToAddress from Production.dbo.MailTo where id = '101'
DECLARE @EndTime datetime = getdate()

exec callJobLog_SP @mailserver,@eMailID,@eMailPwd,@sendFrom,@toAddress,'Import PowerBI Report Data',@comboDesc,0,@ErrorStatus,0,'Power BI',@Region,@M,@StartTime,@EndTime


END
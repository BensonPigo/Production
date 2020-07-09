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
= (
	select [value] = 
		CASE @current_ServerName WHEN 'PHL-NEWPMS-02' THEN 'PH1' -- PH1
				WHEN 'VT1-PH2-PMS2B' THEN 'PH2' 				 -- PH2
				WHEN 'SYSTEM2016BK' THEN 'ESP'					 -- ESP
				WHEN 'SPT-176' THEN 'SPT' 						 -- SPT
				WHEN 'SYSTEM2017BK' THEN 'SNP' 					 -- SNP
				WHEN 'SPS-SQL2' THEN 'SPS' 						 -- SPS
				WHEN 'SQLBK' THEN 'SPR' 						 -- SPR
				WHEN 'NEWERP-BAK' THEN 'HZG' 					 -- HZG	
				WHEN 'SQL' THEN 'HXG'							 -- HXG	
		ELSE '' END
)

DECLARE @M nvarchar(10) 
= (
	select [value] = 
		CASE @current_ServerName WHEN 'PHL-NEWPMS-02' THEN 'PH1' -- PH1
				WHEN 'VT1-PH2-PMS2B' THEN 'PH2'					 -- PH2
				WHEN 'SYSTEM2016BK' THEN 'VM2'					 -- ESP
				WHEN 'SPT-176' THEN 'VM1'						 -- SPT
				WHEN 'SYSTEM2017BK' THEN 'VM3'					 -- SNP
				WHEN 'SPS-SQL2' THEN 'KM2'						 -- SPS
				WHEN 'SQLBK' THEN 'KM1'							 -- SPR
				WHEN 'NEWERP-BAK' THEN 'CM2'					 -- HZG		
				WHEN 'SQL' THEN 'CM1'							 -- HXG				
		ELSE '' END
)


DECLARE @desc nvarchar(1000)  = '';
DECLARE @ErrorMessage NVARCHAR(1000) = '';
DECLARE @ErrorStatus bit = 1;
DECLARE @StartTime datetime = getdate();
DECLARE @StartDate date
DECLARE @EndDate date
-- ImportForecastLoadingBI
BEGIN TRY
	set @StartDate = '2019-01-08'
	set @EndDate = DATEADD(m, DATEDIFF(m,0,DATEADD(MM,5,GETDATE())),6)
	EXEC ImportForecastLoadingBI @StartDate,@EndDate
END TRY

BEGIN CATCH

SET @ErrorMessage = 
'
錯誤程序名稱: [1-Forecast Loading]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;

IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[1-Forecast Loading] is completed'
END
ELSE
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END
SET @ErrorMessage = ''

-- ImportEfficiencyBI 
BEGIN TRY
	set @StartDate = CAST(DATEADD(day,-60, GETDATE()) AS date)
	set @EndDate   = CAST(GETDATE() AS date)
	EXEC  ImportEfficiencyBI @StartDate	,@EndDate
END TRY
BEGIN CATCH
	SET @ErrorMessage = 
'錯誤程序名稱: [2-Production Efficiency]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

	SET @ErrorStatus = 0
END CATCH;

IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) +'
[2-Production Efficiency] is completed'
END
BEGIN
	set @desc += CHAR(13) + @ErrorMessage
END

SET @ErrorMessage = ''

--ImportSewingLineScheduleBIData
BEGIN TRY
	set @StartDate = CAST(DATEADD(day,-60, GETDATE()) AS date)
	set @EndDate   = CAST(DATEADD(day,120, GETDATE()) AS date)
	execute [dbo].[ImportSewingLineScheduleBIData] @StartDate,@EndDate
END TRY

BEGIN CATCH
SET @ErrorMessage = 
'錯誤程序名稱: [3-Production Sewing Line Schedule]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;

IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) + '
[3-Production Sewing Line Schedule] is completed'
END
BEGIN
	set @desc +=  CHAR(13) +@ErrorMessage
END
SET @ErrorMessage = ''

--P_ImportOustandingPO_Fty、P_ImportSDPOrderDetail
BEGIN TRY
	set @StartDate = CAST(DATEADD(day,-150, GETDATE()) AS date)
	set @EndDate   = CAST(DATEADD(day,30, GETDATE()) AS date)
	execute [dbo].[P_ImportOustandingPO_Fty] @StartDate,@EndDate
	DECLARE @BuyerDelivery_s as Date = '2020/01/01'
	--DECLARE @BuyerDelivery_e as Date = '2020/07/31'
	execute [dbo].[P_ImportSDPOrderDetail] @BuyerDelivery_s--,@BuyerDelivery_e
END TRY

BEGIN CATCH
SET @ErrorMessage = 
'錯誤程序名稱: [4-Outstanding PO]' + CHAR(13) +
',錯誤代碼: ' + CONVERT(VARCHAR, ERROR_NUMBER()) + CHAR(13) +
',錯誤行數: ' + CONVERT(VARCHAR, ERROR_LINE()) + CHAR(13) +
',錯誤訊息: ' + ERROR_MESSAGE()

SET @ErrorStatus = 0

END CATCH;

IF (@ErrorMessage IS NULL or @ErrorMessage='')
BEGIN 
	set @desc += CHAR(13) +'
[4-Outstanding PO] is completed'
END
BEGIN
	set @desc +=  CHAR(13) +@ErrorMessage
END
SET @ErrorMessage = ''

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
--,@toAddress = 'willy.wei@sportscity.com.tw'
FROM Production.dbo.System

--select @toAddress = ToAddress from Production.dbo.MailTo where id = '101'
DECLARE @EndTime datetime = getdate()

exec callJobLog_SP @mailserver,@eMailID,@eMailPwd,@sendFrom,@toAddress,'Import PowerBI Report Data',@comboDesc,0,@ErrorStatus,0,'Power BI',@Region,@M,@StartTime,@EndTime

END



-- =============================================
-- Create date: 2020/03/20
-- Description:	Execeudte all SP for PowerBI Report Job in Fty server
-- =============================================
CREATE PROCEDURE [dbo].[execAllPowerBISP]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @desc nvarchar(1000)  = '';
DECLARE @ErrorMessage NVARCHAR(1000) = '';
DECLARE @ErrorStatus bit = 1;
DECLARE @StartTime datetime = getdate();

-- ImportForecastLoadingBI
BEGIN TRY
	EXEC ImportForecastLoadingBI
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
	declare @nowDate as datetime = getdate()
	exec  ImportEfficiencyBI @nowDate	
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
	declare @Inline date= '2020/01/01'
	execute [dbo].[ImportSewingLineScheduleBIData] @Inline
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
	execute [dbo].[P_ImportOustandingPO_Fty]
	DECLARE @BuyerDelivery_s as Date = '2020/01/01'
	DECLARE @BuyerDelivery_e as Date = '2020/07/31'
	execute [dbo].[P_ImportSDPOrderDetail] @BuyerDelivery_s,@BuyerDelivery_e
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
M: VM2
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

exec callJobLog_SP @mailserver,@eMailID,@eMailPwd,@sendFrom,@toAddress,'Import PowerBI Report Data',@comboDesc,0,@ErrorStatus,0,'Power BI','ESP','VM2',@StartTime,@EndTime

END


GO



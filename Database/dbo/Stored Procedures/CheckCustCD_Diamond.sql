
CREATE PROCEDURE [dbo].[CheckCustCD_Diamond]
(
	@BrandID As VARCHAR(50)='NIKE'   ----Default Set NIKE
)
AS
BEGIN

	SET NOCOUNT ON;
	
	Declare @MailBody  As VARCHAR(1000)=''
           ,@ToAddress As VARCHAR(1000)=''
           ,@CcAddress As VARCHAR(1000)=''
           ,@Subject   As VARCHAR(1000)=''
		   ,@IsEmpty As Bit = 0
		   ,@EmptyCustCD As VARCHAR(MAX)=''
		   ;

	SELECT   @ToAddress = ToAddress
			,@CcAddress = CcAddress
			,@Subject = Subject
			,@MailBody = Content
	FROM Production.dbo.MailTo
	WHERE ID = '018'


	IF EXISTS(
		SELECT BrandID,ID
		FROM CustCD
		WHERE BrandID = @BrandID
		AND (DiamondCustCD ='' OR DiamondCity='')
	)
	BEGIN
		SET @IsEmpty=1

		SELECT @EmptyCustCD = (
				SELECT '¡D'+ID + char(10)
				FROM CustCD
				WHERE BrandID = @BrandID
				AND (DiamondCustCD ='' OR DiamondCity='')
				FOR XML PATH('')
			)	
		;
		SET @mailBody = @mailBody+@EmptyCustCD;
	END

	IF @IsEmpty = 1
	BEGIN
		--±HEmail
		EXEC msdb.dbo.sp_send_dbmail  
			@profile_name = 'SUNRISEmailnotice',  
			@recipients = @ToAddress,
			@copy_recipients= @CcAddress,
			@body = @mailBody,  
			@subject = @Subject; 
	END
END
GO

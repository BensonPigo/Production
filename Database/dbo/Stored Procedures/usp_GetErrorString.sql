CREATE PROCEDURE [dbo].[usp_GetErrorString]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
	Declare @ErrorProcedure NVARCHAR(254);
	DECLARE @ErrorLine INT;
	DECLARE @ErrorNumber INT;


    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE(),
        @ErrorProcedure=ERROR_PROCEDURE(),
        @ErrorLine=ERROR_LINE(),
        @ErrorNumber=ERROR_NUMBER();

	IF (@ErrorProcedure IS NULL)
		BEGIN
			set @ErrorMessage = 'Error Line: '+ CONVERT(NVARCHAR,@ErrorLine) + char(13)+char(10) +
						'Error Number: '+CONVERT(NVARCHAR,@ErrorNumber)+ char(13)+char(10) +
						'Error Message: '+@ErrorMessage+ char(13)+char(10)+
						'----------------------------------------------';
		END
	ELSE
		BEGIN
			set @ErrorMessage = 'Error Procedure: ' + @ErrorProcedure + char(13)+char(10) +
						'Error Line: '+ CONVERT(NVARCHAR,@ErrorLine) + char(13)+char(10) +
						'Error Number: '+CONVERT(NVARCHAR,@ErrorNumber)+ char(13)+char(10) +
						'Error Message: '+@ErrorMessage+ char(13)+char(10)+
						'----------------------------------------------';
		END
		
    print @ErrorMessage
END
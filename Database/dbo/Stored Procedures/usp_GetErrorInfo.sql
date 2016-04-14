
-- =============================================
-- Author:		MIKE
-- Create date: 2016/04/13
-- Description:	抓取錯誤資訊
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetErrorInfo]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
	Declare @ErrorProcedure varchar(254);
	DECLARE @ErrorLine INT;
	DECLARE @ErrorNumber INT;


    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE(),
        @ErrorProcedure=ERROR_PROCEDURE(),
        @ErrorLine=ERROR_LINE(),
        @ErrorNumber=ERROR_NUMBER();

	set @ErrorMessage = 'Error Procedure:' + @ErrorProcedure + char(13)+char(10) +
						'Eroor Line:'+ CONVERT(VARCHAR,@ErrorLine) + char(13)+char(10) +
						'Error Number:'+CONVERT(VARCHAR,@ErrorNumber)+ char(13)+char(10) +
						'Error Message:'+@ErrorMessage;

    -- Use RAISERROR inside the CATCH block to return error
    -- information about the original error that caused
    -- execution to jump to the CATCH block.
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END
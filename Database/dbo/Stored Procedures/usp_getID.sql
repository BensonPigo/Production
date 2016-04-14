-- =============================================
-- Author:		MIKE
-- Create date: 2016/04/13
-- Description: getID
-- =============================================
CREATE PROCEDURE [dbo].[usp_getID]
	-- Add the parameters for the stored procedure here
	@keyword varchar(8)='',
	@docno varchar(3)='',
	@issuedate date = null,
	@tablename varchar(60),
	@newid varchar(13) out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		declare @SQLCMD nvarchar(2000)
		declare @YearMonth varchar(4)
		declare @MaxNo varchar(13)

		IF @issuedate is null
		BEGIN
			SET @issuedate = CONVERT(DATE,GETDATE());
		END

		SET @YearMonth = SUBSTRING(CONVERT(CHAR,@issuedate,12),1,4);
		--select @YearMonth

		set @SQLCMD =
		N';WITH  MAX_ID AS (select max(ID) MaxID,right(''00000000000''+convert(varchar,cast(SubString(max(ID),10,4) as bigint)+1),4) MaxNO
		from '+ @tablename +
		' where id like '''+@keyword+@docno+@YearMonth+'%'''+') select @MaxNoOutput=isnull(T.MaxNO,right(''000000000001'',4)) FROM MAX_ID T'

		DECLARE @ParmDefinition nvarchar(500);

		SET @ParmDefinition = N'@MaxNoOutput varchar(13) OUTPUT';
		EXEC sp_executesql @SQLCMD, @ParmDefinition, @MaxNoOutput=@MaxNo OUTPUT;
		--select @MaxNo
		set @newid= @keyword+@docno+@YearMonth+@MaxNo;

	END TRY
	BEGIN CATCH
		EXEC usp_GetErrorInfo;
	END CATCH
END
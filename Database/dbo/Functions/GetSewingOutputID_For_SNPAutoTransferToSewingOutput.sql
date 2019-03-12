USE [Production]
GO

/****** Object:  UserDefinedFunction [dbo].[GetSewingOutputID_For_SNPAutoTransferToSewingOutput]    Script Date: 2019/03/12 下午 05:36:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetSewingOutputID_For_SNPAutoTransferToSewingOutput]
(
	@ID_Key as varchar(5)
	,@RowNumber int
	,@execuDatetime Datetime
)
RETURNS varchar(13)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @SewingOutput_Head varchar(11);
	DECLARE @MaxID varchar(13);
	DECLARE @SewingOutput_ID varchar(2);

	IF @RowNumber=0 OR @execuDatetime IS NULL RETURN '';

	
	SELECT @SewingOutput_Head =
		@ID_Key
		+ RIGHT( CAST( YEAR(@execuDatetime) AS VARCHAR),2)

		+ RIGHT('00' + CONVERT(NVARCHAR(2), MONTH (@execuDatetime)), 2)

		+ RIGHT('00' + CONVERT(NVARCHAR(2), DAY (@execuDatetime)), 2)
	
	SET @MaxID = (SELECT [ID]= MAX(ID)
				 FROM SewingOutput 
				 WHERE ID LIKE @ID_Key
			 	 + RIGHT( CAST( YEAR(GETDATE()) AS VARCHAR),2)
			 	 + RIGHT('00' + CONVERT(NVARCHAR(2), MONTH (@execuDatetime)), 2)
				 + RIGHT('00' + CONVERT(NVARCHAR(2), DAY (@execuDatetime)), 2)
			     +'%');

	SET @SewingOutput_ID=
	(
		SELECT CASE WHEN @MaxID IS NULL  AND @RowNumber = 1 THEN '01'  --表示該日期沒有ID、且是第一筆從01開始
			   WHEN  @MaxID IS NULL AND @RowNumber > 1  THEN RIGHT ('00'+ CAST(@RowNumber AS Varchar) ,2)  
			   ELSE 
				     RIGHT('00'+ CAST( CAST(RIGHT(@MaxID,2) AS INT ) +  @RowNumber AS Varchar ) ,2)  --
			   END
	)
	RETURN @SewingOutput_Head + @SewingOutput_ID

END

GO



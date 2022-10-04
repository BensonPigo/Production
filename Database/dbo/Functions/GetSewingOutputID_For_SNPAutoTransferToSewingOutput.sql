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
	DECLARE @OverNum int
	IF @RowNumber=0 OR @execuDatetime IS NULL RETURN '';

	
	SELECT @SewingOutput_Head =
		@ID_Key
		+ RIGHT( CAST( YEAR(@execuDatetime) AS VARCHAR),2)

		+ RIGHT('00' + CONVERT(NVARCHAR(2), MONTH (@execuDatetime)), 2)

		+ RIGHT('00' + CONVERT(NVARCHAR(2), DAY (@execuDatetime)), 2)
	
	SET @MaxID = (SELECT [ID]= MAX(ID)
				 FROM SewingOutput WITH(NOLOCK)
				 WHERE ID LIKE @ID_Key
			 	 + RIGHT( CAST( YEAR(GETDATE()) AS VARCHAR),2)
			 	 + RIGHT('00' + CONVERT(NVARCHAR(2), MONTH (@execuDatetime)), 2)
				 + RIGHT('00' + CONVERT(NVARCHAR(2), DAY (@execuDatetime)), 2)
			     +'%');

	SET @OverNum = (CAST(isnull(RIGHT(@MaxID,2), 0) AS INT ) + @RowNumber) - 99

	SET @SewingOutput_ID=
	(
		SELECT CASE WHEN @MaxID IS NULL  AND @RowNumber = 1 THEN '01'  --表示該日期沒有ID、且是第一筆從01開始
			   --超過99號，以A1~Z9編號，若又超過，則重新以A1編，使程式報錯
			   WHEN  @OverNum > 0 then CONCAT(char(((@OverNum-1) / 9) % 26 + 65), iif((@OverNum % 9) = 0, 9, @OverNum % 9))
			   WHEN  @MaxID IS NULL AND @RowNumber > 1  THEN RIGHT ('00'+ CAST(@RowNumber AS Varchar) ,2)
			   ELSE 
				     RIGHT('00'+ CAST( CAST(RIGHT(@MaxID,2) AS INT ) +  @RowNumber AS Varchar ) ,2)  --
			   END
	)
	RETURN @SewingOutput_Head + @SewingOutput_ID

END

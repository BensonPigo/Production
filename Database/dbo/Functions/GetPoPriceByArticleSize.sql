-- =============================================
-- Author:		<Ray.Chiou>
-- Create date: <2016/06/13>
-- Description:	<指定訂單編號、顏色組、尺寸別取得訂單單價>
-- =============================================
CREATE FUNCTION [dbo].[GetPoPriceByArticleSize]
(
	@ID varchar(13),
	@Article Varchar(8),
	@SizeCode varchar(8)
)
RETURNS  numeric(8,3)
AS
BEGIN
	-- Declare the return variable here
	Declare @DefaultPrice  numeric(8,3) 
Set @DefaultPrice = isnull((select Poprice from Order_UnitPrice where ID = @ID and article = '----' and SizeCode = '----'),0) 

declare @Return  numeric(8,3)
select @Return = Poprice from Order_UnitPrice where ID = @ID and Article = @Article and SizeCode = @SizeCode
set @Return = isnull(@Return,@DefaultPrice)
Return @Return 

END









GO


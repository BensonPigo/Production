-- =============================================
-- Author:		Aaron
-- Create date: 2017/12/07
-- Description:	原使用GetStyleLocation_Rate改抓取Order_Location,回傳Rate
-- =============================================
CREATE FUNCTION [dbo].[GetOrderLocation_Rate]
(
	@OrderId varchar(13),@ComboType varchar(1)
)
RETURNS NUMERIC(5,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Rate as NUMERIC(5,2);

	-- Add the T-SQL statements to compute the return value here
	select @Rate = Rate from Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType

	-- Return the result of the function
	RETURN @Rate

END
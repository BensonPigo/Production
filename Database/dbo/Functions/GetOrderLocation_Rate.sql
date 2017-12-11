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
	IF (select b.styleunit from orders a WITH (NOLOCK) inner join style b WITH (NOLOCK) on  b.ukey = a.StyleUkey where a.id = @OrderId) = 'PCS'
	Begin
		set @Rate = 100
	End
	Else
	Begin
		select @Rate = Rate from Order_Location With(nolock) where OrderId =@OrderId and Location = @ComboType
	End
	-- Return the result of the function
	RETURN @Rate

END
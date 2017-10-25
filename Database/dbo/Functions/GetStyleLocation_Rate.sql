-- =============================================
-- Author:		Jeff
-- Create date: 2017/10/24
-- Description:	判斷是否為PCS,回傳Rate
-- =============================================
CREATE FUNCTION [dbo].[GetStyleLocation_Rate]
(
	@StyleUkey BIGINT,@ComboType varchar(1)
)
RETURNS NUMERIC(5,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Rate as NUMERIC(5,2);

	-- Add the T-SQL statements to compute the return value here
	IF (select styleunit from style where ukey = @StyleUkey) = 'PCS'
	Begin
		set @Rate = 100
	End
	Else
	Begin
		select @Rate = Rate from Style_Location With(nolock) where styleukey =@StyleUkey and Location = @ComboType
	End

	-- Return the result of the function
	RETURN @Rate

END
-- =============================================
-- Author:		Jack
-- Create date: 2019/07/05
-- Description:	依單位轉換SizeSpec
-- =============================================
CREATE FUNCTION [dbo].[getSizeSpecTrans]
(
	@val as varchar(500),
	@SizeUnit as varchar(20) = 'INCH'
)
RETURNS varchar(500)
AS
BEGIN
	--移除多餘空白
	set @val = LTRIM(RTRIM(@val))
	declare @rtnVal as varchar(500)

	if @SizeUnit = 'INCH' and @val not like'%[a-zA-Z]%' 
	begin
		if CHARINDEX('/', @val) > 0
		begin  
			set @val = dbo.getROF(@val)
		end
		else if CHARINDEX('.', @val) > 0
		begin 
			set @val = dbo.getFractional(@val)
		end 	 
	end
	else if @SizeUnit = 'CM' and @val not like'%[a-zA-Z]%' 
	begin
		if CHARINDEX('/', @val) > 0
		begin  
			set @val = dbo.getDecimal(@val)
		end  
	end

	set @rtnVal = @val

	-- Return the result of the function
	RETURN @rtnVal

END

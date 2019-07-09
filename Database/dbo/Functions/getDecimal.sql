
-- =============================================
-- Author:		Jack
-- Create date: 2019/07/05
-- Description:	分數轉小數
-- =============================================
CREATE FUNCTION [dbo].[getDecimal]
(
	@val as varchar(500)
)
RETURNS varchar(500)
AS
BEGIN
	--移除多餘空白
	set @val = LTRIM(RTRIM(@val))
	declare @rtnVal as varchar(500)

	declare @I as int,@M as int, @D as int, @GCD as int, @sql as varchar(500)
		,@blankIndex as int, @SlashIndex as int

	set @blankIndex = CHARINDEX(' ', @val)
	set @SlashIndex = CHARINDEX('/', @val)

	--判斷是否有整數
	if @blankIndex > 0
	begin
		select @I =  substring(@val, 0, @blankIndex)
			, @D = substring(@val, @blankIndex + 1, @SlashIndex -  @blankIndex -1)
			, @M = substring(@val, @SlashIndex + 1, len(@val) - @SlashIndex)
	end
	else
	begin 
		select @I =  0
			, @D = substring(@val, 0, @SlashIndex)
			, @M = substring(@val, @SlashIndex + 1, len(@val) - @SlashIndex)
	end 

	select @rtnVal = cast(@I + cast(@D *1.0 /@M * 1.0 as decimal(10,6)) as varchar(500))

	-- Return the result of the function
	RETURN @rtnVal

END

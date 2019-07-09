CREATE FUNCTION [dbo].[getROF]
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

	if @SlashIndex = 0
	begin
		RETURN @val
	end

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

	set @sql = cast(@D as varchar)+','+ cast(@M as varchar) 
	set @GCD = dbo.getGCD(@sql)

	set @rtnVal = iif(@blankIndex > 0 ,cast(@I as varchar) + ' ' + cast(@D/@GCD as varchar) + '/' + cast(@M/@GCD as varchar), cast(@D/@GCD as varchar) + '/' + cast(@M/@GCD as varchar))
	-- Return the result of the function
	RETURN @rtnVal

END

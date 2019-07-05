-- =============================================
-- Author:		Jack
-- Create date: 2019/07/05
-- Description:	小數轉分數
-- =============================================
CREATE FUNCTION [dbo].[getFractional]
(
	@val as varchar(500)
)
RETURNS varchar(500)
AS
BEGIN
	--移除多餘空白
	set @val = LTRIM(RTRIM(@val))
	declare @rtnVal as varchar(500)

	declare @I as int,@M as int, @D as int, @GCD as int, @sql as varchar(500), @PointIndex as int
		 
	set @PointIndex = CHARINDEX('.', @val)

	--判斷是否有整數
	if @PointIndex > 0
	begin 
		select @I = I, @D = D, @M = power(10, len(D)) 
		from (
			select D = substring(@val, @PointIndex + 1 , len(@val) - @PointIndex)
				,I = substring(@val, 0, @PointIndex)
		)a

		set @sql = cast(@D as varchar)+','+ cast(@M as varchar) 
		set @GCD = dbo.getGCD(@sql)

		set @rtnVal = iif(@I > 0 ,cast(@I as varchar) + ' ' + cast(@D/@GCD as varchar) + '/' + cast(@M/@GCD as varchar), cast(@D/@GCD as varchar) + '/' + cast(@M/@GCD as varchar))
	end
	else
	begin  
		set @rtnVal = @val
	end 

	-- Return the result of the function
	RETURN @rtnVal

END

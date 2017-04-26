
-- =============================================
-- Author:		Mike
-- Create date: 2016/07/07
-- Description:	傳入FtyInventory.Ukey取得存放的儲位
--				依逗點分隔。
-- =============================================
CREATE FUNCTION [dbo].[Getlocation]
(
	@ukey bigint
)
RETURNS varchar(300)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @locationStr as varchar(300);

	-- Add the T-SQL statements to compute the return value here
	select  @locationStr = isnull(stuff((select ',' + cast(MtlLocationid as varchar) 
										 from (select MtlLocationid 
											   from FtyInventory_Detail WITH (NOLOCK) 
											   where ukey = @ukey) t 
										 for xml path('')
										), 1, 1, '')
								  ,'')

	-- Return the result of the function
	RETURN @locationStr

END
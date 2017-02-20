
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
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	-- Declare the return variable here
	DECLARE @locationStr as varchar(300);

	-- Add the T-SQL statements to compute the return value here
	select  @locationStr = (select MtlLocationID+',' 
	from (select d.MtlLocationID from dbo.FtyInventory_Detail d where ukey = @ukey) t
	for xml path(''))

	-- Return the result of the function
	RETURN @locationStr

END
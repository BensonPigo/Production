-- =============================================
-- Author:		Mike
-- Create date: 2016/04/21
-- Description:	檢查傳入的工廠/日期是否為假日
-- =============================================
CREATE FUNCTION [dbo].[checkHoliday]
(
	-- Add the parameters for the function here
	@factoryid as varchar(8),
	@checkdate as date,
	@sewinglineid as varchar(5)
)
RETURNS bit
AS
BEGIN
	DECLARE @RTN AS BIT;
	-- 有設定工時基本檔且有時數，表示非假日(休假)
	IF EXISTS(SELECT * FROM WorkHour w WITH (NOLOCK) where w.FactoryID = @factoryid and w.Date = @checkdate 
	and w.SewingLineID = @sewinglineid and w.Hours > 0)
	SET @RTN = 0;
	ELSE
	SET @RTN = 1;

	RETURN @RTN;
END
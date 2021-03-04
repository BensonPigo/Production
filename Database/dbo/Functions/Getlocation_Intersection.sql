
CREATE FUNCTION [dbo].[Getlocation_Intersection]
(
	@ukey bigint, @toStockType varchar(1)
)
RETURNS varchar(300)
AS
BEGIN
	DECLARE @locationStr as varchar(300);

	select  @locationStr = stuff((	select ',' + MtlLocationID
									from (	
										select d.MtlLocationID	
										from dbo.FtyInventory_Detail d WITH (NOLOCK) 
										where ukey = @ukey 
										and d.MtlLocationID != ''
										and d.MtlLocationID is not null
										and exists(
											select 1 from MtlLocation m WITH (NOLOCK) 
											WHERE StockType=@toStockType
											and junk <> 1
											and m.ID = d.MtlLocationID
										)
									) t
									for xml path(''))
								, 1, 1, '')

	RETURN @locationStr

END
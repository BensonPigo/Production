CREATE FUNCTION [dbo].[GetLocalPurchaseStdCost]
(
	@id as varchar(13)
)
RETURNS decimal(10,3)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @rtn as decimal(10,3);

	select @rtn = isnull(sum(Price),0) 
	from
	(
		select a.id, ot.Price
		from Order_TmsCost ot WITH (NOLOCK) 
		inner join Orders o WITH (NOLOCK)  on ot.ID = o.ID
		inner join ArtworkType a WITH (NOLOCK) on ot.ArtworkTypeID = a.ID
		where 1=1
		and ot.ID = @id
		and a.Classify = 'P' 
		and exists (
				select 1
				from FirstSaleCostSetting f
				where a.id = f.ArtWorkID 
				and o.OrigBuyerDelivery between f.BeginDate and f.EndDate 
				and f.isjunk = 0 
				and not CostTypeID != 'FactoryCMT'
		) 
		group by a.id, ot.Price
	) as ot

	-- Return the result of the function
	RETURN @rtn;
END

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	計算Local Purchase Std. Cost
-- =============================================
CREATE FUNCTION [dbo].[GetLocalPurchaseStdCost]
(
	-- Add the parameters for the function here
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
		and (
			-- OrigBuyerDelivery 在設定的日期範圍內，有包含非 FactoryCMT 的採購項
			exists (
				select 1
				from FirstSaleCostSetting f
				where a.id = f.ArtWorkID 
				and o.OrigBuyerDelivery between f.BeginDate and f.EndDate 
				and f.isjunk = 0 
				and f.OrderCompanyID = o.OrderCompanyID
				and not CostTypeID != 'Factory CMT'
			)
			or 
			-- OrigBuyerDelivery 沒有在設定的日期範圍內
			not exists (
				select 1
				from FirstSaleCostSetting f
				where a.id = f.ArtWorkID 
				and o.OrigBuyerDelivery between f.BeginDate and f.EndDate 
				and f.OrderCompanyID = o.OrderCompanyID
			)
		)
		group by a.id, ot.Price
	) as ot

	-- Return the result of the function
	RETURN @rtn;
END

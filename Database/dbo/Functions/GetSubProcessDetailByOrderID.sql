CREATE FUNCTION [dbo].[GetSubProcessDetailByOrderID]
(
	@orderID varchar(13),
	@CostType varchar(10)
)
RETURNS @SubProcessDetail TABLE
(
	ArtworkTypeID varchar(20),
	Price numeric(16,4),
	CostType varchar(10) -- 1.AMT 2.CPU
)
AS
BEGIN
	declare @ProductionUnit varchar(3)

	if(@CostType = 'AMT')
	begin
		set @ProductionUnit = 'Qty'
	end
	
	if(@CostType = 'CPU')
	begin
		set @ProductionUnit = 'TMS'
	end

	insert into @SubProcessDetail
		Select	ot.ArtworkTypeID,ot.Price,@CostType
				from Order_TmsCost ot
				inner join ArtworkType a on ot.ArtworkTypeID = a.ID
				where ot.ID = @orderID and (a.Classify = 'A' or ( a.Classify = 'I' and a.IsTtlTMS = 0) and a.IsTMS=0)
				and a.ProductionUnit=@ProductionUnit			
		        union all
		        Select ot.ArtworkTypeID,ot.Price,@CostType
				from Order_TmsCost ot
				inner join ArtworkType a on ot.ArtworkTypeID = a.ID
				where ot.ID = @orderID and ((a.Classify = 'A' or a.Classify = 'I') and a.IsTtlTMS = 0 and a.IsTMS=1)
				and a.ProductionUnit=@ProductionUnit

	RETURN
END


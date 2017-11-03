-- =============================================
-- Author:		Jeff
-- Create date: 2017/11/02
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[getWaste]
(
	@StyleID as varchar(20),
	@BrandID as varchar(20),
	@SeasonID as varchar(20),
	@VNContractID as varchar(20),
	@NLCode as varchar(20)
)
RETURNS numeric(18,4)
AS
BEGIN
	DECLARE @Waste AS numeric(20,4)
	set @Waste = null

	select @Waste = max(vd.Waste)
	from VNConsumption v WITH (NOLOCK)
	inner join VNConsumption_Detail vd WITH (NOLOCK) on v.id = vd.ID
	where 
		v.StyleID	   =@StyleID
	and v.BrandID	   =@BrandID
	and v.SeasonID	   =@SeasonID
	and v.VNContractID =@VNContractID
	and vd.NLCode	   =@NLCode
	if(@Waste is null)
	Begin
		select @Waste = Waste
		from View_VNNLCodeWaste WITH (NOLOCK)
		where  NLCode = @NLCode
	End
	if(@Waste is null)
	Begin
		set @Waste = 0
	End
	RETURN @Waste;
END
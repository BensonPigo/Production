
Create FUNCTION [dbo].[GetReplaceSupp]
(
	@BrandID varchar(8),
	@SuppID varchar(6),
	@CountryID varchar(2),
	@SciRefNo varchar(30),
	@Dest varchar(2),
	@FactoryID varchar(8)
)
RETURNS varchar(6)
AS
BEGIN
	declare @returnSupp varchar(6) = @SuppID

	select top 1 @returnSupp = iif(@SciRefNo != '' and fs.SuppID is null, @SuppID, isnull(sr.SuppID, @SuppID))
	from Production.dbo.Supp_ReplaceSupplier sr
	left join Production.dbo.Fabric_Supp fs on sr.SuppID = fs.SuppID and fs.SCIRefno = @SciRefNo and sr.BrandID = fs.BrandID
	left join Production.dbo.Fabric fb on fs.SCIRefno = fb.SCIRefno
	where sr.BrandID = @BrandID
	and sr.ID = @SuppID
	and (sr.CountryID = @CountryID or sr.FactoryID = @FactoryID)
	and @CountryID = @Dest
	and fs.Lock = 0 and fs.Junk = 0 and fb.Junk = 0
	order by iif(sr.FactoryID = @FactoryID, 1, 2)

	return @returnSupp
END
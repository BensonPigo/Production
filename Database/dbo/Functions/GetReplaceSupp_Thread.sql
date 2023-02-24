
CREATE FUNCTION [dbo].[GetReplaceSupp_Thread]
(
	@BrandID varchar(8),
	@SuppID varchar(6),
	@CountryID varchar(2),
	@Dest varchar(2),
	@SciRefNo varchar(30),
	@StyleUkey bigint,
	@FactoryKpiCode varchar(8),
	@FactoryID varchar(8)
)
RETURNS varchar(6)
AS
BEGIN
	declare @returnSupp varchar(6) = @SuppID

	--若IsGSPPlus則國別換成VN，若ThreadStatus不為Locked、訂單不為B,S單則回傳原始的Supp
	select
		@CountryID = iif(sty.IsGSPPlus = 1, 'VN', @CountryID)
	from Production.dbo.Style sty
	where sty.Ukey = @StyleUkey and EXISTS (select 1 from Production.dbo.Supp where Supp.ID = @SuppID and Supp.SuppGroupFabric = '2450');

	select TOP 1 @returnSupp = iif(@SciRefNo != '' and fsNew.SuppID is null, @SuppID, isnull(sr.SuppID, @SuppID))
	from Production.dbo.Supp_Replace_Detail sr
	left join Production.dbo.Fabric_Supp fs on sr.SuppGroupFabric = fs.SuppID and fs.SCIRefno = @SciRefNo and fs.BrandID = @BrandID
	left join Production.dbo.Fabric fb on fs.SCIRefno = fb.SCIRefno
	left join Production.dbo.Fabric_Supp fsNew on sr.SuppID = fsNew.SuppID and fsNew.SCIRefno = @SciRefNo and fsNew.BrandID = @BrandID
	left join Production.dbo.Fabric fbNew on fsNew.SCIRefno = fbNew.SCIRefno
	where sr.SuppGroupFabric = @SuppID and sr.ToCountry = @CountryID and sr.Type = 'N' and (isnull(sr.FactoryKpiCode,'') ='' or sr.FactoryKpiCode=@FactoryKpiCode)
	and fsNew.Lock = 0 and fsNew.Junk = 0 and fbNew.Junk = 0
	and fs.POUnit = fsNew.POUnit
	ORDER by FactoryKpiCode desc

	--一般的替換廠代流程
	set @returnSupp = dbo.GetReplaceSupp(@BrandID, @returnSupp, @CountryID, @SciRefNo, @Dest, @FactoryID)
	
	return @returnSupp
END
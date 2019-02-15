CREATE PROCEDURE [dbo].[CreateVNConsumption_Detail]
	@ID varchar(15)
AS
begin
Declare @StyleUKey bigint
Declare @Article varchar(8)

select top 1 @StyleUKey = StyleUKey,@Article = va.Article
from VNConsumption v 
left join VNConsumption_Article va WITH (NOLOCK) on va.ID = v.ID
where v.ID = @ID


select 
vdd.ID,
vdd.NLCode,
vdd.HSCode,
vdd.UnitID,
[Qty] = Sum(vdd.Qty),
[SystemQty] = Sum(vdd.SystemQty),
[Waste] = [dbo].[getWaste](v.StyleID,v.BrandID,v.SeasonID,v.VNContractID,vdd.NLCode)
into #tmpVNConsumption_Detail
from VNConsumption v with (nolock)
inner join VNConsumption_Detail_Detail vdd with (nolock) on v.ID = vdd.ID
 where v.id = @ID
 group by vdd.ID,vdd.NLCode,vdd.HSCode,vdd.UnitID,v.StyleID,v.BrandID,v.SeasonID,v.VNContractID


DELETE VNConsumption_Detail where id = @ID

INSERT INTO VNConsumption_Detail(ID,NLCode,HSCode ,UnitID,Qty,SystemQty,Waste)
			SELECT ID,NLCode,HSCode ,UnitID,Qty,SystemQty,Waste FROM #tmpVNConsumption_Detail


drop table #tmpVNConsumption_Detail;

end

CREATE PROCEDURE [dbo].[CreateVNConsumption_Detail]
	@ID varchar(15)
AS
begin
DELETE VNConsumption_Detail where id = @ID

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

INSERT INTO VNConsumption_Detail(ID,NLCode,HSCode ,UnitID,Qty,SystemQty,Waste)
			SELECT ID,NLCode,HSCode ,UnitID,Qty,SystemQty,Waste FROM #tmpVNConsumption_Detail


drop table #tmpVNConsumption_Detail;

end

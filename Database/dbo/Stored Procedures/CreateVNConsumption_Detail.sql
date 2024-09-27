CREATE PROCEDURE [dbo].[CreateVNConsumption_Detail]
	@ID varchar(15),
	@IsBatchCreate bit = 0
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
SELECT ID,NLCode,HSCode ,UnitID,Qty,SystemQty
,Waste = iif(isnull(v3.Waste,0) !=0, v3.Waste,t.waste)
FROM #tmpVNConsumption_Detail t
outer apply(
	select top 1 waste from VNConsumption_Detail_Detail s
	where s.ID = t.ID and s.NLCode = t.NLCode
)v3


if @IsBatchCreate = 0
begin
	-- 更新Waste,要將相同的合約和物料(Style,Brand,Season,VnContractID,NLCode)
	-- 都一併更新相同的Waste
	update t
	set t.Waste = s.Waste
	FROM VNConsumption_Detail t
	inner join VNConsumption v on t.ID = v.ID
	inner join (
		select Waste = iif(isnull(v3.Waste,0) !=0, v3.Waste,svd.waste)
		,svd.NLCode,sv.StyleID,sv.BrandID,sv.SeasonID,sv.VNContractID 
		from #tmpVNConsumption_Detail svd
		inner join VNConsumption sv on svd.ID = sv.ID
		outer apply(
			select top 1 waste from VNConsumption_Detail_Detail s
			where s.ID = svd.ID and s.NLCode = svd.NLCode
		)v3
	) s on s.NLCode = t.NLCode and s.BrandID = v.BrandID and s.StyleID = v.StyleID and s.SeasonID = v.SeasonID and s.VNContractID = v.VNContractID
end


drop table #tmpVNConsumption_Detail;
END

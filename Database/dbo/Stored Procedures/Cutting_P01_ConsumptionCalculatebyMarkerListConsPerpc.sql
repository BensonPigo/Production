
CREATE PROCEDURE [dbo].[Cutting_P01_ConsumptionCalculatebyMarkerListConsPerpc]
	@OrderID VARCHAR(13)
AS
BEGIN

	--抓取ID為POID
	select @OrderID=POID FROM dbo.Orders where ID = @OrderID
	
	SELECT
	ORDERNO=RTRIM(POID) + d.spno ,STYLENO=StyleID+'-'+SeasonID ,QTY=SUM(Qty) ,FACTORY=FactoryID	
	FROM dbo.Orders
	OUTER APPLY(SELECT STUFF((SELECT '/'+SUBSTRING(ID,11,4) FROM production.dbo.Orders WHERE POID = @OrderID  order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno) d
	WHERE POID = @OrderID
	GROUP BY POID,d.spno,StyleID,SeasonID,FactoryID
	
	select ColorID,LossYds,LossYds_FOC,PlusName,SciRefNo into #LossAcc from dbo.GetLossAccessory(@OrderID,'') tmp

	select
	Fabric.Refno,csq.ColorID,SizeItem
	,[REF# Accessotry]=Fabric.Refno+'  SizeCode:'+Order_BOA.SizeItem + ' ' + Order_BOA.SizeItem_Elastic + char(13) + Fabric.Description
	,COLOR=csq.ColorDesc
	,Size=csq.SizeCode
	,SizeSpec=aa.mSizeSpec
	,[Q'ty/PCS]=max(Order_BOA.ConsPC)
	,[Order Qty]=sum(distinct csq.Qty) --以Orders join GetColorSizeQty，會出現一個QTY重複兩筆的情況，需要用distinct排除重複
	,[Unit]=Fabric.UsageUnit
	,[CONSUMPTION]= sum(distinct bb.UseCons)
	,[Unit.]=Fabric_Supp.POUnit
	,cc.mStep
	,cc.mUsageUnit
	,cc.nRound
	,csq.SizeCode
	,mLossQty
	,mFocQty
	,mPlusN
	,csq.Seq
	into #main
	from Orders
	inner join Order_BOA on Order_BOA.Id = Orders.ID
	inner join Fabric on Fabric.SCIRefno = Order_BOA.SCIRefno
	--inner join dbo.GetColorSizeQty(@OrderID) csq on Order_BOA.PatternPanel = csq.Patternpanel and CHARINDEX(Orders.id+'/',csq.OrderList) > 0
	outer apply(select SizeCode,SizeDesc,ColorDesc,ColorID,Seq,sum(Qty) as Qty from dbo.GetColorSizeQty(@OrderID) tmp where tmp.PatternPanel = Order_BOA.PatternPanel and Order_BOA.SizeItem = tmp.SizeItem group by SizeCode,SizeDesc,ColorDesc,ColorID,Seq ) csq
	outer apply(select dbo.GetUnitQty(Orders.SizeUnit ,Fabric.UsageUnit ,dbo.GetDigitalValue(csq.SizeDesc)) as mSizeSpec) aa
	inner join Fabric_Supp on Fabric_Supp.SuppID = Order_BOA.SuppID and Fabric.SCIRefno = Fabric_Supp.SCIRefno
	outer apply( select ROUND(Order_BOA.ConsPC * aa.mSizeSpec * csq.Qty ,2) as UseCons) bb
	
	--Round,UsageUnit,Step
	inner join Program on Program.ID = Orders.ProgramID and Program.BrandID = Orders.BrandID
	inner join Unit on Unit.ID = Fabric_Supp.POUnit
	outer apply (SELECT CASE WHEN Program.MiAdidas = 1 and Orders.Category = 'B' then Unit.MiAdidasRound else Unit.Round end as nRound 
	,CASE WHEN Program.MiAdidas = 1 and Orders.Category = 'B' then Unit.MiAdidasRound else 1 end as mUsageUnit
	,CASE WHEN Program.MiAdidas = 1 and Orders.Category = 'B' then 0 else Unit.RoundStep end as mStep) cc

	--mLossQty,mFocQty,mPlusN
	outer apply (select mLossQty=dbo.GetCeiling(dbo.GetUnitQty(Fabric.UsageUnit,Fabric_Supp.POUnit,LossYds),cc.mUsageUnit,cc.mStep)
		,mFocQty=LossYds_FOC,mPlusN=PlusName from #LossAcc tmp
		where tmp.SciRefNo = Order_BOA.SCIRefno AND tmp.ColorID = csq.ColorID --and tmp.Article = csq.Article
	) ee

	where Orders.poid = @OrderID and SizeItem <> '' and BomTypeCalculate = 1
	group by Fabric.Refno,csq.ColorID,SizeItem,SizeItem_Elastic,Fabric.Refno,Order_BOA.SizeItem,Fabric.Description
	,csq.ColorDesc,csq.SizeCode,aa.mSizeSpec,Fabric.UsageUnit,Fabric_Supp.POUnit
	,cc.mStep,cc.mUsageUnit,cc.nRound,csq.SizeCode,mLossQty,mFocQty,mPlusN,csq.Seq

	order by Seq



	--依照料號、顏色、Size彙總，在計算Consumption、total
	select
		a.[REF# Accessotry]
		,a.COLOR
		,a.Size
		,a.SizeSpec
		,isnull(a.[Q'ty/PCS],0) as [Q'ty/PCS]
		,isnull(a.[Order Qty],0) as [Order Qty]
		,a.[Unit]
		,isnull(a.[CONSUMPTION],0) as [CONSUMPTION]
		,a.[Unit.]
		,isnull(b.mNetQty,0) as [CONSUMPTION.]
		,isnull(b.mPlusN,0) as [PLUS(YDS/%)]
		,isnull(b.Ttlcons,0) as [TOTAL]
	from #main a left join 
	(
		select * from (
			--Group by Refno,ColorID,SizeItem
			select Refno,ColorID,SizeItem,Unit,[Unit.],nRound,mUsageUnit,mStep,mSum=sum([CONSUMPTION]),mLossQty=max(mLossQty),mFocQty=max(mFocQty),mPlusN
			from #main group by Refno,ColorID,SizeItem,Unit,[Unit.],nRound,mUsageUnit,mStep,mPlusN
		) a
		--mNetQty
		outer apply (select dbo.GetCeiling(dbo.GetUnitQty(a.Unit ,a.[Unit.] ,a.mSum),a.mUsageUnit,a.mStep) as mNetQty) dd
		--Total
		outer apply (select dbo.GetCeiling(dd.mNetQty+a.mLossQty,a.nRound,a.mStep) as Ttlcons) ff
	) b on a.Refno = b.Refno and a.ColorID = b.ColorID and a.SizeItem = b.SizeItem
	order by [REF# Accessotry],Color,Seq
	

	drop table #LossAcc
	drop table #main


	
END
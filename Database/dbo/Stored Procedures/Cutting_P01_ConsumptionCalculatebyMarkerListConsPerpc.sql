
CREATE PROCEDURE [dbo].[Cutting_P01_ConsumptionCalculatebyMarkerListConsPerpc]
	@OrderID VARCHAR(13)
AS
BEGIN
	declare @ProjectID Varchar(5)
	declare @IsExpendArticle bit = 0
	declare @IncludeQtyZero bit = 0
	declare @Category Varchar(1) = ''

	--抓取ID為POID
	select @OrderID=POID, @ProjectID = ProjectID, @Category = Category FROM dbo.Orders where ID = @OrderID
	
	set @IsExpendArticle = iif(@ProjectID = 'ARO', 1, 0);

	SELECT
	ORDERNO=RTRIM(POID) + isnull(d.spno,'') ,STYLENO=StyleID+'-'+SeasonID ,QTY=SUM(Qty) ,FACTORY = (select FactoryID from Orders where Id = @OrderID)
	FROM dbo.Orders WITH (NOLOCK)
	OUTER APPLY(SELECT STUFF((SELECT '/'+SUBSTRING(ID,11,4) FROM Production.dbo.Orders WITH (NOLOCK) WHERE POID = @OrderID  order by ID FOR XML PATH('')),1,1,'') as spno) d
	WHERE POID = @OrderID
	GROUP BY POID,d.spno,StyleID,SeasonID
	
	
	select ColorID,PlusName,SciRefNo,sum(LossYds) as LossYds, sum(LossYds_FOC) as LossYds_FOC into #LossAcc 
	from dbo.GetLossAccessory(@OrderID, '', 1, 0, 0) tmp 
	group by ColorID,PlusName,SciRefNo
	
	declare @Tmp_Order_Qty dbo.QtyBreakdown
	insert into @Tmp_Order_Qty
	select Order_Qty.ID ,Article ,SizeCode ,Order_Qty.Qty ,OriQty 			
		  From dbo.Order_Qty
		 Inner Join dbo.Orders
			On Orders.ID = Order_Qty.ID
		 Where Orders.PoID = @OrderID;
		 
	select ID,Order_BOAUkey,SCIRefNo,Article,ColorID,SizeCode,SizeSpec,SizeUnit,ColorDesc,
		sum(OrderQty) as OrderQty, sum(UsageQty) as UsageQty
	into #tmpBOAExpend from GetBOAExpend(@OrderID, 0, 1, 1, @Tmp_Order_Qty, @IsExpendArticle, @IncludeQtyZero)
	group by ID,Order_BOAUkey,SCIRefNo,Article,ColorID,SizeCode,SizeSpec,SizeUnit,ColorDesc

	select
	Fabric.Refno,SizeItem
	,[REF# Accessotry]=Fabric.Refno+/*'  SizeCode:'+ISNULL(Order_BOA.SizeItem,'') + ' ' + ISNULL(Order_BOA.SizeItem_Elastic,'') +*/ char(13) + ISNULL(Fabric.Description,'')
	,Order_BOA.SizeItem_Elastic
	,Order_BOA.ConsPC
	,Fabric.UsageUnit
	,Fabric_Supp.POUnit
	,cc.mStep, cc.mUsageUnit, cc.nRound
	,tmp.SCIRefNo,tmp.ColorID, tmp.UsageQty, tmp.OrderQty, tmp.SizeCode, tmp.SizeSpec, tmp.ColorDesc
	,LossYds
	,mFocQty
	,mPlusN
	,os.Seq
	,ROW_NUMBER() over(partition by tmp.SCIRefNo,tmp.ColorID order by os.Seq) lossidx
	into #main
	from Orders WITH (NOLOCK)
	inner join Order_BOA WITH (NOLOCK) on Order_BOA.Id = Orders.ID
	inner join Fabric WITH (NOLOCK) on Fabric.SCIRefno = Order_BOA.SCIRefno
	inner join #tmpBOAExpend tmp on tmp.Order_BOAUkey = Order_BOA.Ukey	
	inner join Production.dbo.Fabric_Supp WITH (NOLOCK) on Fabric_Supp.SuppID = Order_BOA.SuppID and Fabric.SCIRefno = Fabric_Supp.SCIRefno
	inner join Order_SizeCode os WITH (NOLOCK) on Orders.ID = os.Id and tmp.SizeCode = os.SizeCode
	--Round,UsageUnit,Step
	outer apply (select RoundStep as mStep, UnitRound as nRound, UsageRound as mUsageUnit from Production.dbo.GetUnitRound(Orders.BrandID,Orders.ProgramID,Orders.Category,Fabric_Supp.POUnit)) cc
	--mLossQty,mFocQty,mPlusN
	outer apply (select LossYds, mFocQty=LossYds_FOC, mPlusN=PlusName from #LossAcc loss
		where loss.SciRefNo = tmp.SCIRefno AND loss.ColorID = tmp.ColorID
	) ee
	where Orders.poid = @OrderID and SizeItem <> '' and BomTypeCalculate = 1
	order by os.Seq

	
	select 
		a.[REF# Accessotry]
		,a.ColorDesc as COLOR
		,iif(a.SizeItem_Elastic = '', a.SizeItem, a.SizeItem + '/' + a.SizeItem_Elastic)
		,a.SizeCode as Size
		,a.SizeSpec
		,a.ConsPC as [Q'ty/PCS]
		,a.OrderQty as [Order Qty]
		,a.UsageUnit as [Unit]
		,a.UsageQty as [CONSUMPTION]
		,a.POUnit as [Unit.]
		,IIF(@Category = 'M', b.Ttlcons, b.mNetQty) as [CONSUMPTION.]
		,b.mPlusN as [PLUS(YDS/%)]
		,b.Ttlcons as [TOTAL]
	from (
		select
			a.[REF# Accessotry], a.ColorDesc, a.SizeCode, a.SizeSpec,
			a.UsageUnit, a.POUnit, ColorID, SizeItem, a.Refno,
			a.SizeItem_Elastic,
			max(isnull(a.ConsPC,0)) ConsPC, sum(a.OrderQty) as OrderQty, sum(a.UsageQty) as UsageQty,
			Seq
		from #main a
		group by a.[REF# Accessotry], a.ColorDesc, a.SizeCode, a.SizeSpec, a.UsageUnit, a.POUnit, Seq ,ColorID, a.SizeItem, a.Refno, a.Seq, a.SizeItem_Elastic
	) a left join (
		select * from (
			--Group by Refno,ColorID,SizeItem
			select Refno,ColorID,UsageUnit,POUnit,nRound,mUsageUnit,mStep,mSum=sum(UsageQty),LossYds=sum(iif(lossIdx = 1, LossYds, 0)),mFocQty=sum(mFocQty),mPlusN
			from #main group by Refno,ColorID,UsageUnit,POUnit,nRound,mUsageUnit,mStep,mPlusN
		) a
		--mNetQty
		outer apply (select Production.dbo.GetCeiling(Production.dbo.GetUnitQty(a.UsageUnit ,a.POUnit ,a.mSum),a.mUsageUnit,0) as mNetQty) dd
		--mLossQty
		outer apply (select Production.dbo.GetCeiling(Production.dbo.GetUnitQty(a.UsageUnit ,a.POUnit ,a.LossYds),a.mUsageUnit,0) as mLossQty) ee
		--Total
		outer apply (select Production.dbo.GetCeiling(dd.mNetQty+ee.mLossQty,a.nRound,a.mStep) as Ttlcons) ff	
	) b on a.Refno = b.Refno and a.ColorID = b.ColorID --and a.SizeItem = b.SizeItem 		
	order by [REF# Accessotry],Color,a.SizeItem,Seq
	

	drop table #LossAcc
	drop table #main


	
END
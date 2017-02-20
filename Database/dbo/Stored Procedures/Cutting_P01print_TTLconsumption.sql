
CREATE PROCEDURE [dbo].[Cutting_P01print_TTLconsumption]
	@OrderID VARCHAR(13)
AS
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

	--抓取ID為POID
	select @OrderID=POID FROM dbo.Orders where ID = @OrderID

	SELECT ORDERNO=RTRIM(POID) + d.spno ,STYLENO=StyleID+'-'+a.SeasonID ,QTY=SUM(Qty) ,FACTORY=FactoryID ,FABTYPE=b.FabricType ,FLP=cast(c.TWLimitUp as varchar)+'%' ,e.MarkerDownloadID
	FROM dbo.Orders a
	inner join dbo.Style b on a.StyleID = b.Id and a.BrandID = b.BrandID and a.SeasonID = b.SeasonID
	inner join dbo.LossRateFabric c on b.FabricType = c.WeaveTypeID
	OUTER APPLY(SELECT STUFF((SELECT '/'+SUBSTRING(ID,11,4) FROM Production.dbo.Orders WHERE POID = @OrderID  order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno ) d
	OUTER APPLY(SELECT STUFF((SELECT '/'+rtrim(MarkerDownloadID) FROM Production.dbo.Order_EachCons WHERE Id = @OrderID and MarkerDownloadID <> '' group by MarkerDownloadID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as MarkerDownloadID ) e
	WHERE POID = @OrderID
	GROUP BY CuttingSP,POID,d.spno,StyleID,a.SeasonID,FactoryID,b.FabricType,c.TWLimitUp,e.MarkerDownloadID

	SELECT Order_EachCons.TYPE,Style_BOF.ConsPC,Fabric.Width,CuttingPiece,Orders.StyleUnit
	,[COLOR]=Color.Name
	,Order_EachCons_Color.YDS
	,ORDER_BOF.FabricCode
	,[REF# FABRIC]=Fabric.Refno+CHAR(10)+Fabric.[Description]
	,Weight=cast(cast(Round(Fabric.[Weight],1,1) as decimal(10,1)) as nvarchar) + 'g'
	,Fabric.UsageUnit
	,Fabric_Supp.POUnit
	,System.ProphetSingleSizeDeduct
	,Program.MiAdidas
	,Unit.UnitRound ,Unit.RoundStep ,Unit.UsageRound	
	,lf.RealLoss,lf.PlusName
	,Orders.IsMixMarker
	into #tmp
	FROM DBO.Order_EachCons
	inner join Order_EachCons_Color on Order_EachCons.Id = Order_EachCons_Color.Id and Order_EachCons.Ukey = Order_EachCons_Color.Order_EachConsUkey
	inner join Order_BOF on Order_EachCons.Id = ORDER_BOF.Id and Order_EachCons.FabricCode = ORDER_BOF.FabricCode
	inner join Fabric on ORDER_BOF.SCIRefno = Fabric.SCIRefno
	inner join Fabric_Supp on ORDER_BOF.SCIRefno = Fabric_Supp.SCIRefno and ORDER_BOF.SuppID = Fabric_Supp.SuppID
	inner join Production.dbo.System on 1 = 1--from Trade
	inner join Orders on Orders.ID = Order_EachCons.Id
	inner join Program on Orders.BrandID = Program.BrandID and Orders.ProgramID = Program.ID
	outer apply (select * from dbo.GetUnitRound(Orders.BrandID, Orders.ProgramID, Orders.Category, Fabric.UsageUnit)) Unit
	inner join Color on Color.ID = Order_EachCons_Color.ColorID and Color.BrandId = Orders.BrandID
	inner join Style_BOF on Orders.StyleUkey = Style_BOF.StyleUkey and Fabric.SCIRefno = Style_BOF.SCIRefno and Order_BOF.FabricCode = Style_BOF.FabricCode
	left join DBO.GetLossFabric(@OrderID, '') lf on lf.FabricCode = ORDER_BOF.FabricCode and lf.ColorID = Order_EachCons_Color.ColorID
	where Order_EachCons.Id = @OrderID
	--where Orders.POID = @OrderID

	--
	
	select [REF# FABRIC]
	,[COLOR]
	,[UNIT]=case when UsageUnit = POUnit then null else UsageUNIT end
	,[CONSUMPTION]=case when UsageUnit = POUnit then null else cast(Qty as decimal(10,2)) end --UsageCONSUMPTION
	,[UNIT.]=POUnit --PurchaseUNIT
	,[CONSUMPTION.]=cast(d.mNetQty as decimal(10,2)) --PurchaseCONSUMPTION
	,[PLUS(YDS/%)]=iif(isnumeric(PlusName) = 1, cast(cast(dbo.GetCeiling(PlusName,1,0) as numeric(8,2)) as varchar),PlusName)
	,[TOTAL(Inclcut. use)]=cast(e.total as decimal(10,2))
	,[CUTTING USE]=floor(cast(d.mNetQty49 as decimal(10,2))) --不顯示小數
	,[M/WIDTH]=cast(Width as varchar(5)) + '"'
	,[M/WEIGHT]=[Weight]
	,[TTL CONS(KG)]=''
	,[CONS/PC]=''
	,[STYLE DATA CONS/PC]=iif(ConsPC=0,'',Cast(ConsPC as varchar(20)))
	from (select [REF# FABRIC],[COLOR],IsMixMarker,MiAdidas,FabricCode,ProphetSingleSizeDeduct=max(ProphetSingleSizeDeduct),UsageUnit=max(UsageUnit),POUnit=max(POUnit)
		,PlusName=max(PlusName),Width=max(Width),[Weight]=max([Weight]),ConsPC=max(ConsPC)
		,RealLoss=max(RealLoss),UsageRound=max(UsageRound),UnitRound=max(UnitRound)
		,RoundStep=max(RoundStep),YDS=sum(YDS) 
		,SUM(YDS-(YDS*ProphetSingleSizeDeduct/100)) AS Qty
		,SUM(case when CuttingPiece = 1 and TYPE<>2 then YDS-(YDS*ProphetSingleSizeDeduct/100) else 0 end) AS Qty49
		from #tmp 
		group by [REF# FABRIC],[COLOR],FabricCode,IsMixMarker,MiAdidas) cc
	--計算轉換單位
	OUTER APPLY (SELECT dbo.GetUnitQty(UsageUnit,POUnit,Qty) AS mQty
	,dbo.GetUnitQty(UsageUnit,POUnit,Qty49) AS mQty49
	,dbo.GetUnitQty(UsageUnit,POUnit,RealLoss) as LossQty) b
	--計算小數進位
	OUTER APPLY (SELECT dbo.GetCeiling(b.mQty,UsageRound,RoundStep) AS mNetQty
	,dbo.GetCeiling(b.mQty49,UsageRound,RoundStep) AS mNetQty49
	,dbo.GetCeiling(b.LossQty,UsageRound,RoundStep) AS mLossQty) d
	OUTER APPLY(SELECT dbo.GetCeiling(d.mNetQty+d.mLossQty,UnitRound,RoundStep) as total) e
	ORDER BY FabricCode,COLOR
	
	drop table #tmp
	
END
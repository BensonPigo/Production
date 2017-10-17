﻿
CREATE PROCEDURE [dbo].[Cutting_P01print_TTLconsumption]
	@OrderID VARCHAR(13)
AS
BEGIN

	--抓取ID為POID
	select @OrderID=POID FROM dbo.Orders WITH (NOLOCK) where ID = @OrderID

	--SELECT ORDERNO=RTRIM(POID) + d.spno ,STYLENO=StyleID+'-'+a.SeasonID ,QTY=SUM(Qty) ,FACTORY=FactoryID ,FABTYPE=b.FabricType ,FLP=cast(c.TWLimitUp as varchar)+'%' ,e.MarkerDownloadID
	SELECT d.POComboList as OrderNo,STYLENO=StyleID+'-'+a.SeasonID ,QTY=SUM(Qty) ,FACTORY = (select FactoryID from Orders where Id = @OrderID) ,FABTYPE=b.FabricType ,FLP=cast(c.TWLimitUp as varchar)+'%' ,isnull(e.MarkerDownloadID,'') as MarkerDownloadID
	FROM dbo.Orders a WITH (NOLOCK)
	inner join dbo.Style b WITH (NOLOCK) on a.StyleID = b.Id and a.BrandID = b.BrandID and a.SeasonID = b.SeasonID
	left join dbo.LossRateFabric c WITH (NOLOCK) on b.FabricType = c.WeaveTypeID
	  Left Join dbo.Order_POComboList as d On a.POID = d.ID
	--OUTER APPLY(SELECT STUFF((SELECT '/'+SUBSTRING(ID,11,4) FROM Production.dbo.Orders WITH (NOLOCK) WHERE POID = @OrderID  order by ID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as spno ) d
	OUTER APPLY(SELECT STUFF((SELECT '/'+rtrim(MarkerDownloadID) FROM Production.dbo.Order_EachCons WITH (NOLOCK) WHERE Id = @OrderID and MarkerDownloadID <> '' group by MarkerDownloadID FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as MarkerDownloadID ) e
	WHERE POID = @OrderID
	GROUP BY CuttingSP,POID,d.POComboList,StyleID,a.SeasonID,b.FabricType,c.TWLimitUp,e.MarkerDownloadID
	--GROUP BY CuttingSP,POID,d.spno,StyleID,a.SeasonID,FactoryID,b.FabricType,c.TWLimitUp,e.MarkerDownloadID
	/*
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
	inner join Order_EachCons_Color WITH (NOLOCK) on Order_EachCons.Id = Order_EachCons_Color.Id and Order_EachCons.Ukey = Order_EachCons_Color.Order_EachConsUkey
	inner join Order_BOF WITH (NOLOCK) on Order_EachCons.Id = ORDER_BOF.Id and Order_EachCons.FabricCode = ORDER_BOF.FabricCode
	inner join Fabric WITH (NOLOCK) on ORDER_BOF.SCIRefno = Fabric.SCIRefno
	inner join Fabric_Supp WITH (NOLOCK) on ORDER_BOF.SCIRefno = Fabric_Supp.SCIRefno and ORDER_BOF.SuppID = Fabric_Supp.SuppID
	inner join Production.dbo.System on 1 = 1--from Trade
	inner join Orders WITH (NOLOCK) on Orders.ID = Order_EachCons.Id
	inner join Program WITH (NOLOCK) on Orders.BrandID = Program.BrandID and Orders.ProgramID = Program.ID
	outer apply (select * from dbo.GetUnitRound(Orders.BrandID, Orders.ProgramID, Orders.Category, Fabric.UsageUnit)) Unit
	inner join Color WITH (NOLOCK) on Color.ID = Order_EachCons_Color.ColorID and Color.BrandId = Orders.BrandID
	left join Style_BOF WITH (NOLOCK) on Orders.StyleUkey = Style_BOF.StyleUkey and Fabric.SCIRefno = Style_BOF.SCIRefno and Order_BOF.FabricCode = Style_BOF.FabricCode
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
	*/
	Select CuttingSP Into #tmpCuttingList From Production.dbo.Orders Where Orders.POID = @OrderID Group by CuttingSP;

	Select SciRefNo, ColorID, FabricCode, PlusName, 
		sum(LossYds) as LossYds,
		sum(RealLoss) as RealLoss 
	Into #tmpFabLoss From dbo.GetLossFabric(@OrderID, '') group by SciRefNo,ColorID,FabricCode,PlusName;

	With tmpTtlEachCons as
	(
		Select Order_BOF.FabricCode, Order_BOF.SCIRefno, Order_BOF.Refno
			 , Order_EachCons_Color.ColorID, IsNull(Color.Name, Order_EachCons_Color.ColorID) as ColorName
			 , IsNull(Fabric.UsageUnit, '') as UsageUnit
			 , IsNull(Fabric_Supp.POUnit, '') as POUnit
			 , IsNull(Fabric.Description, '') as Description
			 , IsNull(Fabric.Width, 0) as Width
			 , IsNull(Fabric.Weight, 0) as Weight
			 , Supp.CountryID as SuppCountry
			 , Order_BOF.LossType
			 , Order_BOF.LossPercent
			 , Unit.UnitRound, Unit.RoundStep, Unit.UsageRound
			 , Max(Order_EachCons.MarkerLength) as Yds
			 , Max(Order_EachCons.ConsPC) as PCS
			 , Max(Order_BOF.ConsPC) as ConsPC
			 , Sum(tmpQty.Qty) as Qty
			 , Sum(tmpQty49.Qty49) as Qty49
		  From #tmpCuttingList
		 Inner Join dbo.Orders
			On Orders.ID = #tmpCuttingList.CuttingSP
		 Inner Join dbo.Order_EachCons
			On Order_EachCons.ID = #tmpCuttingList.CuttingSP
		 Inner Join dbo.Order_EachCons_Color
			On Order_EachCons_Color.Order_EachConsUkey = Order_EachCons.Ukey
		 Inner Join dbo.Order_FabricCode
			On Order_FabricCode.ID = Orders.POID
			   And Order_FabricCode.FabricPanelCode = Order_EachCons.FabricPanelCode
		 Inner Join dbo.Order_BOF
			On	   Order_BOF.ID = Orders.POID
			   And Order_BOF.FabricCode = Order_FabricCode.FabricCode
		  Left Join dbo.Color
			On	   Color.BrandId = Orders.BrandID
			   And Color.ID = Order_EachCons_Color.ColorID
		  Left Join dbo.Fabric
			On Fabric.SCIRefno = Order_BOF.SCIRefno
		  Left Join dbo.Fabric_Supp
			On	   Fabric_Supp.SCIRefno = Order_BOF.SCIRefno
			   And Fabric_Supp.SuppID = OrdeR_BOF.SuppID
		  Left Join dbo.Supp
			On Supp.Id = OrdeR_BOF.SuppID
		 Cross Apply (Select ProphetSingleSizeDeduct From dbo.System) as tmpTradeSystem
		 Cross Apply (Select IIF(Order_EachCons.CuttingPiece = 0 And Orders.EachConsSource = 'S', Order_EachCons_Color.YDS * IsNull(tmpTradeSystem.ProphetSingleSizeDeduct, 0) / 100, 0) as Deduct) tmpDeduct
		 Cross Apply (Select (Order_EachCons_Color.Yds - tmpDeduct.Deduct) as Qty) as tmpQty
		 Cross Apply (Select IIF(Order_EachCons.CuttingPiece = 1 And Order_EachCons.Type != 'T', Order_EachCons_Color.Yds - tmpDeduct.Deduct, 0) Qty49) as tmpQty49
		 Outer Apply (Select * From dbo.GetUnitRound(Orders.BrandID, Orders.ProgramID, Orders.Category, Fabric.UsageUnit)) as Unit
		 Group by Order_BOF.FabricCode, Order_BOF.SCIRefno, Order_BOF.Refno
			 , Order_EachCons_Color.ColorID, Color.Name, Fabric.UsageUnit, Fabric_Supp.POUnit, Fabric.Description
			 , Fabric.Width, Fabric.Weight, Supp.CountryID, Order_BOF.LossType, Order_BOF.LossPercent
			 , Unit.UnitRound, Unit.RoundStep, Unit.UsageRound
	)
	Select [REF# FABRIC] = tmpTtlEachCons.Refno + CHAR(10) + tmpTtlEachCons.[Description]
		 , [COLOR] = tmpTtlEachCons.ColorName
		 , [UNIT] = IIF(tmpTtlEachCons.UsageUnit = tmpTtlEachCons.POUnit, Null, tmpTtlEachCons.UsageUNIT)
		 , [CONSUMPTION] = IIF(tmpTtlEachCons.UsageUnit = tmpTtlEachCons.POUnit, Null, Cast(tmpTtlEachCons.Qty as decimal(10,2)))	--UsageCONSUMPTION
		 , [UNIT.]=POUnit --PurchaseUNIT
		 , [CONSUMPTION.] = Cast(tmpQty2.NetQty as Decimal(10,2)) --PurchaseCONSUMPTION
		 , [PLUS(YDS/%)] = IIF(IsNumeric(tmpFabLoss.PlusName) = 1, Cast(Cast(dbo.GetCeiling(tmpFabLoss.PlusName,1,0) as Numeric(8,2)) As Varchar), tmpFabLoss.PlusName)
		 , [TOTAL(Inclcut. use)] = Cast(tmpTtlQty.Qty as Decimal(10,2))
		 , [CUTTING USE]=floor(cast(tmpQty2.NetQty49 as Decimal(10,2))) --不顯示小數
		 , [M/WIDTH] = Cast(tmpTtlEachCons.Width as varchar(5)) + '"'
		 , [M/WEIGHT] = Cast(Cast(Round(tmpTtlEachCons.[Weight], 1, 1) as Decimal(10,1)) as NVarchar) + 'g'
		 , [TTL CONS(KG)] = ''
		 , [CONS/PC] = ''
		 , [STYLE DATA CONS/PC] = IIF(tmpTtlEachCons.ConsPC = 0, '', Cast(tmpTtlEachCons.ConsPC as Varchar(20)))
	  From tmpTtlEachCons
	 Outer Apply (Select * From #tmpFabLoss Where #tmpFabLoss.FabricCode = tmpTtlEachCons.FabricCode And #tmpFabLoss.ColorID = tmpTtlEachCons.ColorID) as tmpFabLoss
	 --計算轉換單位
	 Outer Apply (Select dbo.GetUnitQty(UsageUnit,POUnit,Qty) as Qty
					   , dbo.GetUnitQty(UsageUnit,POUnit,Qty49) as Qty49
					   , dbo.GetUnitQty(UsageUnit,POUnit,RealLoss) as LossQty
				 ) as tmpQty
	 --計算小數進位
	 Outer Apply (Select dbo.GetCeiling(tmpQty.Qty, tmpTtlEachCons.UsageRound, tmpTtlEachCons.RoundStep) as NetQty
					   , dbo.GetCeiling(tmpQty.Qty49, tmpTtlEachCons.UsageRound, tmpTtlEachCons.RoundStep) as NetQty49
					   , dbo.GetCeiling(tmpQty.LossQty, tmpTtlEachCons.UsageRound, tmpTtlEachCons.RoundStep) AS LossQty
				 ) as tmpQty2
	 Outer Apply (Select dbo.GetCeiling(tmpQty2.NetQty + tmpQty2.LossQty, tmpTtlEachCons.UnitRound, tmpTtlEachCons.RoundStep) as Qty) as tmpTtlQty

	Drop Table #tmpCuttingList
END
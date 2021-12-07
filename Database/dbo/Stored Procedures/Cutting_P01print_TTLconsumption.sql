
CREATE PROCEDURE [dbo].[Cutting_P01print_TTLconsumption]
	@OrderID VARCHAR(13), @ReportFormat VARCHAR(1) = 'T'-- T: 原本來自 Trade 格式 ; S: PMS 才有的 Segmentation 格式
AS
BEGIN
	declare @ProjectID Varchar(5)
	declare @IsExpendArticle bit = 0
	declare @Category Varchar(1) = ''

	--抓取ID為POID
	select @OrderID=POID, @ProjectID = ProjectID, @Category = Category FROM dbo.Orders where ID = @OrderID
	
	set @IsExpendArticle = iif(@ProjectID = 'ARO', 1, 0);

	SELECT
		APPLYNO = d.SMNoticeID
		, MARKERNO = d.MARKERNO
		, opl.POComboList as OrderNo
		, STYLENO=StyleID + '-' + a.SeasonID
		, QTY = dbo.GetEachConsOrderQty(a.POID)
		, FACTORY = (select FactoryID from Orders where Id = @OrderID)
		, FABTYPE = fnFabricType
		, FLP = cast(isnull(c.TWLimitUp,0) as varchar) + '%'
		, isnull(e.MarkerDownloadID,'') as MarkerDownloadID
	FROM dbo.Orders a
	inner join production.dbo.Style b on a.StyleID = b.Id and a.BrandID = b.BrandID and a.SeasonID = b.SeasonID
	outer apply (select ReasonGroup from production.dbo.Reason r where r.ReasonTypeID = 'Fabric_Kind' and r.ID = b.FabricType) r
	outer apply (select fnFabricType = isnull(ReasonGroup, b.FabricType)) fnf
	left join production.dbo.LossRateFabric c on b.FabricType = c.WeaveTypeID
	outer apply ( select 
		STUFF((SELECT '/'+MarkerNo+'-'+MarkerVersion FROM dbo.Order_EachCons WHERE Id = @OrderID group by MarkerNo,MarkerVersion FOR XML PATH('')),1,1,'') as MarkerNo
		,STUFF((SELECT '/'+SMNoticeID FROM dbo.Order_EachCons WHERE Id = @OrderID group by SMNoticeID FOR XML PATH('')),1,1,'') as SMNoticeID) d
	Left Join dbo.Order_POComboList as opl On a.POID = opl.ID
	OUTER APPLY(SELECT STUFF((SELECT '/'+rtrim(MarkerDownloadID) FROM production.dbo.Order_EachCons WHERE Id = @OrderID and MarkerDownloadID <> '' group by MarkerDownloadID FOR XML PATH('')),1,1,'') as MarkerDownloadID ) e
	WHERE POID = @OrderID
	GROUP BY CuttingSP,POID,opl.POComboList,StyleID,a.SeasonID,fnFabricType,c.TWLimitUp,e.MarkerDownloadID, d.SMNoticeID, d.MARKERNO

	Select CuttingSP Into #tmpCuttingList From dbo.Orders Where Orders.POID = @OrderID Group by CuttingSP;

	Select SciRefNo, ColorID, FabricCode,
		sum(LossYds) as LossYds,
		sum(RealLoss) as RealLoss
	Into #tmpFabLoss From dbo.GetLossFabric(@OrderID, '', @IsExpendArticle) 
	group by SciRefNo,ColorID,FabricCode;
	
	SELECT *
	into #tmpTtlEachCons
	FROM 
	(
	Select Orders.ID,Order_BOF.FabricCode, Order_BOF.SCIRefno, Order_BOF.Refno
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
			, Sum(tmpQty49.Qty49) as Qty49
			, Fabric.WeaveTypeID
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
		Left Join production.dbo.Color
		On	   Color.BrandId = Orders.BrandID
			And Color.ID = Order_EachCons_Color.ColorID
		Left Join production.dbo.Fabric
		On Fabric.SCIRefno = Order_BOF.SCIRefno
		Left Join production.dbo.Fabric_Supp
		On	   Fabric_Supp.SCIRefno = Order_BOF.SCIRefno
			And Fabric_Supp.SuppID = OrdeR_BOF.SuppID
		Left Join production.dbo.Supp
		On Supp.Id = OrdeR_BOF.SuppID
		Cross Apply (Select ProphetSingleSizeDeduct From production.dbo.system) as tmpTradeSystem
		Cross Apply (Select IIF(Order_EachCons.CuttingPiece = 0 And Orders.EachConsSource = 'S', Order_EachCons_Color.YDS * IsNull(tmpTradeSystem.ProphetSingleSizeDeduct, 0) / 100, 0) as Deduct) tmpDeduct
		Cross Apply (Select IIF(Order_EachCons.CuttingPiece = 1 And Order_EachCons.Type != 'T', Order_EachCons_Color.Yds - tmpDeduct.Deduct, 0) Qty49) as tmpQty49
		Outer Apply (Select * From production.dbo.GetUnitRound(Orders.BrandID, Orders.ProgramID, Orders.Category, Fabric.UsageUnit)) as Unit
		Group by Orders.ID,Order_BOF.FabricCode, Order_BOF.SCIRefno, Order_BOF.Refno
			, Order_EachCons_Color.ColorID, Color.Name, Fabric.UsageUnit, Fabric_Supp.POUnit, Fabric.Description
			, Fabric.Width, Fabric.Weight, Supp.CountryID, Order_BOF.LossType, Order_BOF.LossPercent
			, Unit.UnitRound, Unit.RoundStep, Unit.UsageRound, Fabric.WeaveTypeID
	)a
	Cross Apply ( SELECT SUM(UsageQty) AS QTY FROM GetBOFExpend(a.ID, a.FabricCode, @IsExpendArticle) WHERE ColorID = a.ColorID) as tmpQty --QTY改用BOFexpend後的
	
	if @ReportFormat = 'T'
		Select [REF# FABRIC] = #tmpTtlEachCons.Refno + CHAR(10) + #tmpTtlEachCons.[Description]
			, [COLOR] = #tmpTtlEachCons.ColorName
			, [UNIT] = IIF(#tmpTtlEachCons.UsageUnit = #tmpTtlEachCons.POUnit, Null, #tmpTtlEachCons.UsageUNIT)
			, [CONSUMPTION] = IIF(#tmpTtlEachCons.UsageUnit = #tmpTtlEachCons.POUnit, Null, Cast(#tmpTtlEachCons.Qty as decimal(10,2)))	--UsageCONSUMPTION
			, [UNIT.]=POUnit --PurchaseUNIT
			, [CONSUMPTION.] =  Cast(tmpQty2.NetQty as Decimal(10,2)) --PurchaseCONSUMPTION
			, [PLUS(YDS/%)] = Cast( production.dbo.GetUnitQty(UsageUnit, POUnit, production.dbo.GetCeiling(tmpFabLoss.RealLoss,#tmpTtlEachCons.UsageRound,0) ) as Numeric(8,2))
			, [TOTAL(Inclcut. use)] = Cast(tmpTtlQty.Qty as Decimal(10,2))
			, [CUTTING USE]=Cast(tmpQty2.NetQty49 as Decimal(10,2)) --不顯示小數
			, [M/WIDTH] = Cast(#tmpTtlEachCons.Width as varchar(5)) + '"'
			, [M/WEIGHT] = Cast(Cast(Round(#tmpTtlEachCons.[Weight], 1, 1) as Decimal(10,1)) as NVarchar) + 'g'
			, [TTL CONS(KG)] = ''
			, [CONS/PC] = ''
			, [STYLE DATA CONS/PC] = IIF(#tmpTtlEachCons.ConsPC = 0, '', Cast(#tmpTtlEachCons.ConsPC as Varchar(20)))
		From #tmpTtlEachCons
		Outer Apply (Select * From #tmpFabLoss Where #tmpFabLoss.FabricCode = #tmpTtlEachCons.FabricCode And #tmpFabLoss.ColorID = #tmpTtlEachCons.ColorID) as tmpFabLoss
		--計算轉換單位
		Outer Apply (Select production.dbo.GetUnitQty(UsageUnit,POUnit,Qty) as Qty
						, production.dbo.GetUnitQty(UsageUnit,POUnit,Qty49) as Qty49
						, production.dbo.GetUnitQty(UsageUnit,POUnit,RealLoss) as LossQty
					) as tmpQty
		--計算小數進位
		Outer Apply (Select production.dbo.GetCeiling(tmpQty.Qty, #tmpTtlEachCons.UsageRound, #tmpTtlEachCons.RoundStep) as NetQty
						, production.dbo.GetCeiling(tmpQty.Qty49, #tmpTtlEachCons.UsageRound, #tmpTtlEachCons.RoundStep) as NetQty49
						, production.dbo.GetCeiling(tmpQty.LossQty, #tmpTtlEachCons.UsageRound, #tmpTtlEachCons.RoundStep) AS LossQty
					) as tmpQty2
		Outer Apply (Select production.dbo.GetCeiling(tmpQty2.NetQty + tmpQty2.LossQty, #tmpTtlEachCons.UnitRound, #tmpTtlEachCons.RoundStep) as Qty) as tmpTtlQty
		order by #tmpTtlEachCons.FabricCode, #tmpTtlEachCons.ColorID
	Else
		Select [REF# FABRIC] = #tmpTtlEachCons.Refno 
			, #tmpTtlEachCons.WeaveTypeID
			, [Description] = REPLACE(#tmpTtlEachCons.Description,#tmpTtlEachCons.WeaveTypeID+'; ','')
			, [COLOR CODE] = #tmpTtlEachCons.ColorID
			, [COOR NAME] = REPLACE(#tmpTtlEachCons.ColorName, #tmpTtlEachCons.ColorID +' ', '')
			, [UNIT] = IIF(#tmpTtlEachCons.UsageUnit = #tmpTtlEachCons.POUnit, Null, #tmpTtlEachCons.UsageUNIT)
			, [CONSUMPTION] = IIF(#tmpTtlEachCons.UsageUnit = #tmpTtlEachCons.POUnit, Null, Cast(#tmpTtlEachCons.Qty as decimal(10,2)))	--UsageCONSUMPTION
			, [UNIT.]=POUnit --PurchaseUNIT
			, [CONSUMPTION.] =  Cast(tmpQty2.NetQty as Decimal(10,2)) --PurchaseCONSUMPTION
			, [PLUS(YDS/%)] = Cast( production.dbo.GetUnitQty(UsageUnit, POUnit, production.dbo.GetCeiling(tmpFabLoss.RealLoss,#tmpTtlEachCons.UsageRound,0) ) as Numeric(8,2))
			, [TOTAL(Inclcut. use)] = Cast(tmpTtlQty.Qty as Decimal(10,2))
			, [CUTTING USE]=Cast(tmpQty2.NetQty49 as Decimal(10,2)) --不顯示小數
			, [M/WIDTH] = Cast(#tmpTtlEachCons.Width as varchar(5)) + '"'
			, [M/WEIGHT] = Cast(Cast(Round(#tmpTtlEachCons.[Weight], 1, 1) as Decimal(10,1)) as NVarchar) + 'g'
			, [TTL CONS(KG)] = ''
			, [CONS/PC] = ''
			, [STYLE DATA CONS/PC] = IIF(#tmpTtlEachCons.ConsPC = 0, '', Cast(#tmpTtlEachCons.ConsPC as Varchar(20)))
		From #tmpTtlEachCons
		Outer Apply (Select * From #tmpFabLoss Where #tmpFabLoss.FabricCode = #tmpTtlEachCons.FabricCode And #tmpFabLoss.ColorID = #tmpTtlEachCons.ColorID) as tmpFabLoss
		--計算轉換單位
		Outer Apply (Select production.dbo.GetUnitQty(UsageUnit,POUnit,Qty) as Qty
						, production.dbo.GetUnitQty(UsageUnit,POUnit,Qty49) as Qty49
						, production.dbo.GetUnitQty(UsageUnit,POUnit,RealLoss) as LossQty
					) as tmpQty
		--計算小數進位
		Outer Apply (Select production.dbo.GetCeiling(tmpQty.Qty, #tmpTtlEachCons.UsageRound, #tmpTtlEachCons.RoundStep) as NetQty
						, production.dbo.GetCeiling(tmpQty.Qty49, #tmpTtlEachCons.UsageRound, #tmpTtlEachCons.RoundStep) as NetQty49
						, production.dbo.GetCeiling(tmpQty.LossQty, #tmpTtlEachCons.UsageRound, #tmpTtlEachCons.RoundStep) AS LossQty
					) as tmpQty2
		Outer Apply (Select production.dbo.GetCeiling(tmpQty2.NetQty + tmpQty2.LossQty, #tmpTtlEachCons.UnitRound, #tmpTtlEachCons.RoundStep) as Qty) as tmpTtlQty
		order by #tmpTtlEachCons.FabricCode, #tmpTtlEachCons.ColorID



	Drop Table #tmpCuttingList, #tmpTtlEachCons
	--*/
END
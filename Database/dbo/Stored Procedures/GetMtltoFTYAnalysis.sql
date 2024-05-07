CREATE PROCEDURE  [dbo].[GetMtltoFTYAnalysis]
	@CloseDateFrom Datetime = null
AS
begin
	SET NOCOUNT ON;

	IF @CloseDateFrom is null
	BEGIN
		Set @CloseDateFrom = dateadd(day, -150, getdate())
	END

--開抓Detail資料
--Declare @Inline nvarchar(20) = format(dateadd(Day,-15,FORMAT(getdate(), 'hh:mm:ss')) ,'yyyy/MM/dd' )
--畫面抓取條件，取得APSNo

declare @P_MtltoFTYAnalysis TABLE(
	[Factory] [varchar](8) NOT NULL,
		[Country] [varchar](2) NOT NULL,
		[Brand] [varchar](8) NOT NULL,
		[WeaveType] [varchar](20) NOT NULL,
		[ETD] [date] NULL,
		[ETA] [date] NULL,
		[CloseDate] [date] NULL,
		[ActDate] [date] NULL,
		[Category] [nvarchar](50) NOT NULL,
		[OrderID] [varchar](13) NOT NULL,
		[Seq1] [varchar](3) NOT NULL,
		[Seq2] [varchar](2) NOT NULL,
		[OrderCfmDate] [date] NULL,
		[SciDelivery] [date] NULL,
		[Refno] [varchar](36) NOT NULL,
		[SCIRefno] [varchar](30) NOT NULL,
		[SuppID] [varchar](6) NOT NULL,
		[SuppName] [nvarchar](12) NOT NULL,
		[CurrencyID] [varchar](3) NOT NULL,
		[CurrencyRate] [numeric](2, 0) NOT NULL,
		[Price] [numeric](16, 4) NOT NULL,
		[Price(TWD)] [numeric](16, 4) NOT NULL,
		[Unit] [varchar](8) NOT NULL,
		[PoQty] [numeric](10, 2) NOT NULL,
		[PoFoc] [numeric](10, 2) NOT NULL,
		[ShipQty] [numeric](12, 2) NOT NULL,
		[ShipFoc] [numeric](12, 2) NOT NULL,
		[TTShipQty] [numeric](12, 2) NOT NULL,
		[ShipAmt(TWD)] [numeric](16, 4) NOT NULL,
		[FabricJunk] [varchar](1) NOT NULL,
		[WKID] [varchar](13) NOT NULL,
		[ShipmentTerm] [varchar](5) NOT NULL,
		[FabricType] [varchar](10) NOT NULL,
		[PINO] [varchar](25) NOT NULL,
		[PIDATE] [date] NULL,
		[Color] [varchar](6) NOT NULL,
		[ColorName] [nvarchar](150) NOT NULL,
		[Season] [varchar](10) NOT NULL,
		[PCHandle] [nvarchar](100) NOT NULL,
		[POHandle] [nvarchar](100) NOT NULL,
		[POSMR] [nvarchar](100) NOT NULL,
		[Style] [varchar](15) NOT NULL,
		[OrderType] [varchar](20) NOT NULL,
		[ShipModeID] [varchar](10) NOT NULL,
		[Supp1stCfmDate] [date] NULL,
		[BrandSuppCode] [varchar](6) NOT NULL,
		[BrandSuppName] [nvarchar](100) NOT NULL,
		[CountryofLoading] [varchar](30) NOT NULL,
		[SupdelRvsd] [date] NULL,
		[ProdItem] [varchar](20) NOT NULL,
		[KPILETA] [date] NULL,
		[MaterialConfirm] [varchar](1) NOT NULL,
		[SupplierGroup] [varchar](8) NOT NULL,
		[TransferBIDate] [date] NULL
)
	insert into @P_MtltoFTYAnalysis(Factory, Country, Brand, WeaveType, ETD, ETA, CloseDate, ActDate, Category, OrderID, Seq1, Seq2, OrderCfmDate, SciDelivery, Refno, SCIRefno, SuppID, SuppName, CurrencyID, CurrencyRate, Price, [Price(TWD)], Unit, PoQty, PoFoc, ShipQty, ShipFoc, TTShipQty, [ShipAmt(TWD)], FabricJunk, WKID, ShipmentTerm, FabricType, PINO, PIDATE, Color, ColorName, Season, PCHandle, POHandle, POSMR, Style, OrderType, ShipModeID, Supp1stCfmDate, BrandSuppCode, BrandSuppName, CountryofLoading, SupdelRvsd, ProdItem, KPILETA, MaterialConfirm, SupplierGroup, TransferBIDate)
	Select [FactoryID] = ISNULL(main.FactoryID, '')
		, [CountryID] = ISNULL(Factory.CountryID, '')
		, [BrandID] = ISNULL(main.BrandID, '')
		, [WeaveTypeID] = ISNULL(main.WeaveTypeID, '')
		, main.ETD
		, main.ETA
		, main.CloseDate
		, main.ActDate
		, [Category] = ISNULL(ddl.Name, '')
		, [POID] = ISNULL(main.POID, '')
		, [Seq1] = ISNULL(main.Seq1, '')
		, [Seq2] = ISNULL(main.Seq2, '')
		, main.CFMDate
		, main.SCIDelivery
		, [Refno] = ISNULL(main.Refno, '')
		, [SCIRefno] = ISNULL(main.SCIRefno, '')
		, [SuppID] = ISNULL(Supp.ID, '')
		, [AbbCh] = ISNULL(Supp.AbbCh, '')
		, [CurrencyID] = ISNULL(Supp.CurrencyID, '')
		, [CurrencyRate] = ISNULL(curTWD.Rate, 0)
		, [Price] = ISNULL(main.Price, 0)
		, [PriceTWD] = ISNULL(main.Price * curTWD.Rate, 0)
		, [POUnit] = ISNULL(main.POUnit, '')
		, [PoQty] = ISNULL(main.PoQty, 0)
		, [PoFoc] = ISNULL(main.PoFoc, 0)
		, [ShipQty] = ISNULL(main.ShipQty, 0)
		, [ShipFoc] = ISNULL(main.ShipFoc, 0)
		, [TTShipQty] = ISNULL(main.TTShipQty, 0)
		, [ShipAmtTWD] = ISNULL(dbo.GetAmount((main.Price * curTWD.Rate),main.ShipQty, 1, 2), 0) 
		, [FabricJunk] = ISNULL(main.FabricJunk, '')
		, [WKID] = ISNULL(main.ID, '')
		, [ShipmentTerm] = ISNULL(main.ShipmentTerm, '')
		, [FabricType] = ISNULL(main.FabricType, '')
		, [PINO] = ISNULL(main.PINO, '')
		, main.PIDate
		, [Color] = ISNULL(main.ColorID, '')
		, [ColorName] = ISNULL(Color.Name, '')
		, [Season] = ISNULL(main.SeasonID, '')
		, [PCHandle] = ISNULL(pcUser.IdAndNameAndExt, '')
		, [POHandle] = ISNULL(poUser.IdAndNameAndExt, '')
		, [POSMR] = ISNULL(posmrUser.IdAndNameAndExt, '')
		, [StyleID] = ISNULL(main.StyleID, '')
		, [OrderTypeID] = ISNULL(main.OrderTypeID, '')
		, [ShipModeID] = ISNULL(main.ShipModeID, '')
		, SupDel1stcfm = IsNull(stockPO3.CfmETD, stockPO3.SystemETD)
		, [SuppCode] = ISNULL(Supp_BrandSuppCode.SuppCode, '')
		, [SuppName] = ISNULL(Supp_BrandSuppCode.SuppName, '')
		, [Alias] = ISNULL(Country.Alias, '')
		, SupdelRvsd = stockPO3.RevisedETD
		, [ProdItem] = ISNULL(MtlType.ProductionType, '')
		, KPILETA = main.EstLETA
		, MaterialConfirm = iif(main.Confirm = 1, 'Y', 'N')
		, [SupplierGroup] = ISNULL(Supp.SuppGroupFabric, '')
		, TransferBIDate = GETDATE()
	From (
		SELECT Orders.FactoryID
			, Orders.BrandID
			, e.ETD
			, e.ETA
			, e.CloseDate
			, e.WhseArrival as ActDate
			, ed.POID
			, ed.Seq1
			, ed.Seq2
			, Orders.CFMDate
			, Orders.SCIDelivery
			, ed.Refno
			, ed.SCIRefno
			, ed.Price
			, po3.POUnit
			, po3.Qty as PoQty
			, po3.Foc as PoFoc
			, ed.Qty as ShipQty
			, ed.Foc as ShipFoc
			, ed.Qty + ed.Foc as TTShipQty
			, e.ID
			, e.ShipmentTerm
			, po3.PINO
			, po3.PIDate
			, ColorID = po3Spec.Color
			, Orders.SeasonID
			, Orders.StyleID
			, Orders.OrderTypeID
			, e.ShipModeID
			, Orders.Category
			, PO.PCHandle
			, PO.POHandle
			, PO.POSMR
			, ed.SuppID
			, po3.StockPOID
			, po3.StockSeq1
			, po3.StockSeq2
			, Fabric.WeaveTypeID
			, iif(cfu.Junk = 1, 'Y', '') as FabricJunk
			, iif(Fabric.Type = 'F', 'Fabric', 'Accessory') as FabricType
			, [Confirm] = po3.Complete
			, GetSci.EstLETA
		FROM [Production].dbo.Export e WITH (NOLOCK)
		Left join [Production].dbo.Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
		Left join [Production].dbo.PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = ed.POID and po3.Seq1 = ed.Seq1 and po3.Seq2 = ed.Seq2
		Outer apply [Production].dbo.GetPo3Spec(po3.ID, po3.Seq1, po3.Seq2) po3Spec
		Left join [Production].dbo.PO WITH (NOLOCK) on PO.ID = po3.ID
		Left join [Production].dbo.Orders WITH (NOLOCK) on Orders.ID = po3.ID
		Left join [Production].dbo.Fabric WITH (NOLOCK) on ed.SciRefno = Fabric.SciRefno
		Outer apply (Select * From [Production].dbo.CheckFabricUseful(ed.SCIRefno, Orders.SeasonID, ed.SuppID)) cfu
		Outer apply [Production].dbo.GetSCI(Orders.POID, Orders.Category) as GetSci
		Where e.CloseDate >= @CloseDateFrom AND ed.PoType = 'G' 

	) main
	inner join [Production].dbo.Factory WITH (NOLOCK) on main.FactoryID = Factory.ID and Factory.IsProduceFty = 1
	Left join [Production].dbo.Supp WITH (NOLOCK) on main.SuppID = Supp.ID
	Left join [Production].dbo.Country WITH (NOLOCK) On Country.ID = Supp.CountryID
	Left join [Production].dbo.Fabric WITH (NOLOCK) on main.SciRefno = Fabric.SciRefno
	Left Join [Production].dbo.MtlType WITH (NOLOCK) On Fabric.MtltypeId = MtlType.ID
	Left join [Production].dbo.Color WITH (NOLOCK) on Color.ID = main.ColorID And Color.BrandId = main.BrandID
	Outer Apply (select [IdAndNameAndExt] = [Production].dbo.getTPEPass1_ExtNo(main.PCHandle)) as pcUser
	Outer Apply (select [IdAndNameAndExt] = [Production].dbo.getTPEPass1_ExtNo(main.POHandle)) as poUser
	Outer Apply (select [IdAndNameAndExt] = [Production].dbo.getTPEPass1_ExtNo(main.POSMR)) as posmrUser 
	Left Join [Production].dbo.DropDownList ddl WITH (NOLOCK) on ddl.Type = 'Category' and ddl.ID = main.Category
	Left join [Production].dbo.Supp_BrandSuppCode WITH (NOLOCK) On Supp_BrandSuppCode.ID = Supp.ID and Supp_BrandSuppCode.BrandID = main.BrandID
	Outer apply [Production].dbo.GetCurrencyRate('20', Supp.CurrencyID, 'TWD' ,main.CFMDate) as curTWD
	Outer Apply (
		Select CfmETD, SystemETD, RevisedETD
		from [Production].dbo.PO_Supp_Detail tmpPO3 WITH (NOLOCK)
		inner join [Production].dbo.Po_Supp tmpPO2 WITH (NOLOCK) on tmpPO3.ID = tmpPO2.ID and tmpPO3.Seq1 = tmpPO2.Seq1
		inner join [Production].dbo.Supp stockSupp WITH (NOLOCK) on tmpPO2.SuppID = stockSupp.ID
		inner join [Production].dbo.Orders stockOrders WITH (NOLOCK) on stockOrders.ID = tmpPO3.ID
		where tmpPO3.ID = IIF(IsNull(main.StockPOID, '') = '' , main.POID, main.StockPOID)
			and tmpPO3.Seq1 = IIF(IsNull(main.StockPOID, '') = '' , main.Seq1, main.StockSeq1)
			and tmpPO3.Seq2 = IIF(IsNull(main.StockPOID, '') = '' , main.Seq2, main.StockSeq2)
	) stockPO3
	Order by main.ID, main.POID, main.Seq1, main.Seq2


	select * from @P_MtltoFTYAnalysis

END
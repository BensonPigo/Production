Create Procedure [dbo].[P_Import_MtltoFTYAnalysis]
As
Begin
	Set NoCount On;
	Declare @CloseDateFrom Datetime = '2022-01-01'
	
	Select main.FactoryID
		, Factory.CountryID
		, main.BrandID
		, main.WeaveTypeID
		, main.ETD
		, main.ETA
		, main.CloseDate
		, main.ActDate
		, ddl.Name as Category
		, main.POID
		, main.Seq1
		, main.Seq2
		, main.CFMDate
		, main.SCIDelivery
		, main.Refno
		, main.SCIRefno
		, SuppID = Supp.ID
		, Supp.AbbCh
		, Supp.CurrencyID
		, curTWD.Rate as CurrencyRate
		, main.Price
		, main.Price * curTWD.Rate as PriceTWD
		, main.POUnit
		, main.PoQty
		, main.PoFoc
		, main.ShipQty
		, main.ShipFoc
		, main.TTShipQty
		, dbo.GetAmount((main.Price * curTWD.Rate),main.ShipQty, 1, 2) as ShipAmtTWD
		, main.FabricJunk
		, main.ID
		, main.ShipmentTerm
		, main.FabricType
		, main.PINO
		, main.PIDate
		, main.ColorID
		, Color.Name
		, main.SeasonID
		, pcUser.IdAndNameAndExt as PCHandle
		, poUser.IdAndNameAndExt as POHandle
		, posmrUser.IdAndNameAndExt as POSMR
		, main.StyleID
		, main.OrderTypeID
		, main.ShipModeID
		, SupDel1stcfm = IsNull(stockPO3.CfmETD, stockPO3.SystemETD)
		, Supp_BrandSuppCode.SuppCode
		, Supp_BrandSuppCode.SuppName
		, Country.Alias
		, SupdelRvsd = stockPO3.RevisedETD
		, ProdItem = MtlType.ProductionType
		, KPILETA = main.MinKPILETA
		, MaterialConfirm = iif(main.Confirm = 1, 'Y', '')
		, SupplierGroup = Supp.SuppGroupFabric
		, TransferBIDate = GETDATE()
	Into #tmpTable
	From (
		SELECT Orders.FactoryID
			, Orders.BrandID
			, Export.ETD
			, Export.ETA
			, Export.CloseDate
			, Export.WhseArrival as ActDate
			, Export_Detail.POID
			, Export_Detail.Seq1
			, Export_Detail.Seq2
			, Orders.CFMDate
			, Orders.SCIDelivery
			, Export_Detail.Refno
			, Export_Detail.SCIRefno
			, Export_Detail.Price
			, po3.POUnit
			, po3.Qty as PoQty
			, po3.Foc as PoFoc
			, Export_Detail.Qty as ShipQty
			, Export_Detail.Foc as ShipFoc
			, Export_Detail.Qty + Export_Detail.Foc as TTShipQty
			, Export.ID
			, Export.ShipmentTerm
			, po3.PINO
			, po3.PIDate
			, ColorID = po3Spec.Color
			, Orders.SeasonID
			, Orders.StyleID
			, Orders.OrderTypeID
			, Export.ShipModeID
			, Orders.Category
			, PO.PCHandle
			, PO.POHandle
			, PO.POSMR
			, Export_Detail.SuppID
			, po3.StockPOID
			, po3.StockSeq1
			, po3.StockSeq2
			, Fabric.WeaveTypeID
			, iif(cfu.Junk = 1, 'Y', '') as FabricJunk
			, iif(Fabric.Type = 'F', 'Fabric', 'Accessory') as FabricType
			, Export.Confirm
			, GetSci.MinKPILETA
		FROM [MainServer].[Production].dbo.Export WITH (NOLOCK)
		Left join [MainServer].[Production].dbo.Export_Detail WITH (NOLOCK) on Export.ID = Export_Detail.ID
		Left join [MainServer].[Production].dbo.PO_Supp_Detail po3 WITH (NOLOCK) on po3.ID = Export_Detail.POID and po3.Seq1 = Export_Detail.Seq1 and po3.Seq2 = Export_Detail.Seq2
		Outer apply [MainServer].[Production].dbo.GetPo3Spec(po3.ID, po3.Seq1, po3.Seq2) po3Spec
		Left join [MainServer].[Production].dbo.PO WITH (NOLOCK) on PO.ID = po3.ID
		Left join [MainServer].[Production].dbo.Orders WITH (NOLOCK) on Orders.ID = po3.ID
		Left join [MainServer].[Production].dbo.Fabric WITH (NOLOCK) on Export_Detail.SciRefno = Fabric.SciRefno
		Outer apply (Select * From [MainServer].[Production].dbo.CheckFabricUseful(Export_Detail.SCIRefno, Orders.SeasonID, Export_Detail.SuppID)) cfu
		Outer apply [MainServer].[Production].dbo.GetSCI(Orders.POID, Orders.Category) as GetSci
		Where 1=1 And Export_Detail.PoType = 'G' And Export.CloseDate >= @CloseDateFrom

	) main
	Left join [MainServer].[Production].dbo.Factory WITH (NOLOCK) on main.FactoryID = Factory.ID
	Left join [MainServer].[Production].dbo.Supp WITH (NOLOCK) on main.SuppID = Supp.ID
	Left join [MainServer].[Production].dbo.Country WITH (NOLOCK) On Country.ID = Supp.CountryID
	Left join [MainServer].[Production].dbo.Fabric WITH (NOLOCK) on main.SciRefno = Fabric.SciRefno
	Left Join [MainServer].[Production].dbo.MtlType WITH (NOLOCK) On Fabric.MtltypeId = MtlType.ID
	Left join [MainServer].[Production].dbo.Color WITH (NOLOCK) on Color.ID = main.ColorID And Color.BrandId = main.BrandID
	Left Join [MainServer].[Production].dbo.GetName pcUser on pcUser.ID = main.PCHandle
	Left Join [MainServer].[Production].dbo.GetName poUser on poUser.ID = main.POHandle
	Left Join [MainServer].[Production].dbo.GetName posmrUser on posmrUser.ID = main.POSMR
	Left Join [MainServer].[Production].dbo.DropDownList ddl WITH (NOLOCK) on ddl.Type = 'Category' and ddl.ID = main.Category
	Left join [MainServer].[Production].dbo.Supp_BrandSuppCode WITH (NOLOCK) On Supp_BrandSuppCode.ID = Supp.ID and Supp_BrandSuppCode.BrandID = main.BrandID
	Outer apply [MainServer].[Production].dbo.GetCurrencyRate('20', Supp.CurrencyID, 'TWD' ,main.CFMDate) as curTWD
	Outer Apply (
		Select CfmETD, SystemETD, RevisedETD
		from [MainServer].[Production].dbo.PO_Supp_Detail tmpPO3 WITH (NOLOCK)
		inner join [MainServer].[Production].dbo.Po_Supp tmpPO2 WITH (NOLOCK) on tmpPO3.ID = tmpPO2.ID and tmpPO3.Seq1 = tmpPO2.Seq1
		inner join [MainServer].[Production].dbo.Supp stockSupp WITH (NOLOCK) on tmpPO2.SuppID = stockSupp.ID
		inner join [MainServer].[Production].dbo.Orders stockOrders WITH (NOLOCK) on stockOrders.ID = tmpPO3.ID
		where tmpPO3.ID = IIF(IsNull(main.StockPOID, '') = '' , main.POID, main.StockPOID)
			and tmpPO3.Seq1 = IIF(IsNull(main.StockPOID, '') = '' , main.Seq1, main.StockSeq1)
			and tmpPO3.Seq2 = IIF(IsNull(main.StockPOID, '') = '' , main.Seq2, main.StockSeq2)
	) stockPO3
	Order by main.ID, main.POID, main.Seq1, main.Seq2			

	update p
		set p.Factory = t.FactoryID
			, p.Country			  = t.CountryID
			, p.Brand				  = t.BrandID
			, p.WeaveType			  = t.WeaveTypeID
			, p.ETD				  = t.ETD
			, p.ETA				  = t.ETA
			, p.CloseDate			  = t.CloseDate
			, p.ActDate			  = t.ActDate
			, p.Category			  = t.Category
			, p.OrderCfmDate		  = t.CFMDate
			, p.SciDelivery		  = t.SCIDelivery
			, p.Refno				  = t.Refno
			, p.SCIRefno			  = t.SCIRefno
			, p.SuppID			  = t.[SuppID]
			, p.SuppName			  = t.AbbCh
			, p.CurrencyID		  = t.CurrencyID
			, p.CurrencyRate		  = t.CurrencyRate
			, p.Price				  = t.Price
			, p.[Price(TWD)]		  = t.PriceTWD
			, p.Unit				  = t.POUnit
			, p.PoQty				  = t.PoQty
			, p.PoFoc				  = t.PoFoc
			, p.ShipQty			  = t.ShipQty
			, p.ShipFoc			  = t.ShipFoc
			, p.TTShipQty			  = t.TTShipQty
			, p.[ShipAmt(TWD)]		  = t.ShipAmtTWD
			, p.FabricJunk		  = t.FabricJunk
			, p.ShipmentTerm		  = t.ShipmentTerm
			, p.FabricType		  = t.FabricType
			, p.PINO				  = t.PINO
			, p.PIDATE			  = t.PIDate
			, p.Color				  = t.ColorID
			, p.ColorName			  = t.Name
			, p.Season			  = t.SeasonID
			, p.PCHandle			  = t.PCHandle
			, p.POHandle			  = t.POHandle
			, p.POSMR				  = t.POSMR
			, p.Style				  = t.StyleID
			, p.OrderType			  = t.OrderTypeID
			, p.ShipModeID		  = t.ShipModeID
			, p.Supp1stCfmDate	  = t.SupDel1stcfm
			, p.BrandSuppCode		  = t.SuppCode
			, p.BrandSuppName		  = t.SuppName
			, p.CountryofLoading	  = t.Alias
			, p.SupdelRvsd		  = t.SupdelRvsd
			, p.ProdItem			  = t.ProdItem
			, p.KPILETA			  = t.KPILETA
			, p.MaterialConfirm	  = t.MaterialConfirm
			, p.SupplierGroup		  = t.SupplierGroup
			, p.TransferBIDate	  = t.TransferBIDate
	FROM P_MtltoFTYAnalysis p
	inner join #tmpTable t on p.WKID = t.ID and p.OrderID = t.POID and p.Seq1 = t.Seq1 and p.Seq2 = t.Seq2

	insert into P_MtltoFTYAnalysis([Factory], [Country], [Brand], [WeaveType], [ETD], [ETA], [CloseDate], [ActDate], [Category], [OrderID], [Seq1], [Seq2], [OrderCfmDate], [SciDelivery], [Refno], [SCIRefno], [SuppID], [SuppName], [CurrencyID], [CurrencyRate], [Price], [Price(TWD)], [Unit], [PoQty], [PoFoc], [ShipQty], [ShipFoc], [TTShipQty], [ShipAmt(TWD)], [FabricJunk], [WKID], [ShipmentTerm], [FabricType], [PINO], [PIDATE], [Color], [ColorName], [Season], [PCHandle], [POHandle], [POSMR], [Style], [OrderType], [ShipModeID], [Supp1stCfmDate], [BrandSuppCode], [BrandSuppName], [CountryofLoading], [SupdelRvsd], [ProdItem], [KPILETA], [MaterialConfirm], [SupplierGroup], [TransferBIDate])
	select t.FactoryID
		, t.CountryID
		, t.BrandID
		, t.WeaveTypeID
		, t.ETD
		, t.ETA
		, t.CloseDate
		, t.ActDate
		, t.Category
		, t.POID
		, t.Seq1
		, t.Seq2
		, t.CFMDate
		, t.SCIDelivery
		, t.Refno
		, t.SCIRefno
		, t.SuppID
		, t.AbbCh
		, t.CurrencyID
		, t.CurrencyRate
		, t.Price
		, t.[PriceTWD]
		, t.POUnit
		, t.PoQty
		, t.PoFoc
		, t.ShipQty
		, t.ShipFoc
		, t.TTShipQty
		, t.[ShipAmtTWD]
		, t.FabricJunk
		, t.ID
		, t.ShipmentTerm
		, t.FabricType
		, t.PINO
		, t.PIDate
		, t.ColorID
		, t.Name
		, t.SeasonID
		, t.PCHandle
		, t.POHandle
		, t.POSMR
		, t.StyleID
		, t.OrderTypeID
		, t.ShipModeID
		, t.SupDel1stcfm
		, t.SuppCode
		, t.SuppName
		, t.Alias
		, t.SupdelRvsd
		, t.ProdItem
		, t.KPILETA
		, t.MaterialConfirm
		, t.SupplierGroup
		, t.TransferBIDate
	from #tmpTable t
	where not exists (select 1 from P_MtltoFTYAnalysis p where p.WKID = t.ID and p.OrderID = t.POID and p.Seq1 = t.Seq1 and p.Seq2 = t.Seq2)

	if exists (select 1 from BITableInfo b where b.id = 'P_MtltoFTYAnalysis')
	begin
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = 'P_MtltoFTYAnalysis'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_MtltoFTYAnalysis', getdate())
	end
End
GO
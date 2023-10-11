Create Procedure [dbo].[P_Import_MtltoFTYAnalysis]
As
Begin
	Set NoCount On;
	Declare @CloseDateFrom Datetime = dateadd(day, -150, getdate())
	
	SELECT * into #Final 
	FROM OPENQUERY([MainServer],  'exec Production.dbo.GetMtltoFTYAnalysis' )

	update p
		set p.Factory				= t.Factory
			, p.Country				= t.Country
			, p.Brand				= t.Brand
			, p.WeaveType			= t.WeaveType
			, p.ETD					= t.ETD
			, p.ETA					= t.ETA
			, p.CloseDate			= t.CloseDate
			, p.ActDate				= t.ActDate
			, p.Category			= t.Category
			, p.OrderCfmDate		= t.OrderCfmDate
			, p.SciDelivery			= t.SciDelivery
			, p.Refno				= t.Refno
			, p.SCIRefno			= t.SCIRefno
			, p.SuppID				= t.SuppID
			, p.SuppName			= t.SuppName
			, p.CurrencyID			= t.CurrencyID
			, p.CurrencyRate		= t.CurrencyRate
			, p.Price				= t.Price
			, p.[Price(TWD)]		= t.[Price(TWD)]
			, p.Unit				= t.Unit
			, p.PoQty				= t.PoQty
			, p.PoFoc				= t.PoFoc
			, p.ShipQty				= t.ShipQty
			, p.ShipFoc				= t.ShipFoc
			, p.TTShipQty			= t.TTShipQty
			, p.[ShipAmt(TWD)]		= t.[ShipAmt(TWD)]
			, p.FabricJunk			= t.FabricJunk
			, p.ShipmentTerm		= t.ShipmentTerm
			, p.FabricType			= t.FabricType
			, p.PINO				= t.PINO
			, p.PIDATE				= t.PIDate
			, p.Color				= t.Color
			, p.ColorName			= t.ColorName
			, p.Season				= t.Season
			, p.PCHandle			= t.PCHandle
			, p.POHandle			= t.POHandle
			, p.POSMR				= t.POSMR
			, p.Style				= t.Style
			, p.OrderType			= t.OrderType
			, p.ShipModeID			= t.ShipModeID
			, p.Supp1stCfmDate		= t.Supp1stCfmDate
			, p.BrandSuppCode		= t.BrandSuppCode
			, p.BrandSuppName		= t.BrandSuppName
			, p.CountryofLoading	= t.CountryofLoading
			, p.SupdelRvsd			= t.SupdelRvsd
			, p.ProdItem			= t.ProdItem
			, p.KPILETA				= t.KPILETA
			, p.MaterialConfirm		= t.MaterialConfirm
			, p.SupplierGroup		= t.SupplierGroup
			, p.TransferBIDate		= t.TransferBIDate
	FROM P_MtltoFTYAnalysis p
	inner join #Final t on p.WKID = t.WKID and p.OrderID = t.OrderID and p.Seq1 = t.Seq1 and p.Seq2 = t.Seq2

	insert into P_MtltoFTYAnalysis([Factory], [Country], [Brand], [WeaveType], [ETD], [ETA], [CloseDate], [ActDate], [Category], [OrderID], [Seq1], [Seq2], [OrderCfmDate], [SciDelivery], [Refno], [SCIRefno], [SuppID], [SuppName], [CurrencyID], [CurrencyRate], [Price], [Price(TWD)], [Unit], [PoQty], [PoFoc], [ShipQty], [ShipFoc], [TTShipQty], [ShipAmt(TWD)], [FabricJunk], [WKID], [ShipmentTerm], [FabricType], [PINO], [PIDATE], [Color], [ColorName], [Season], [PCHandle], [POHandle], [POSMR], [Style], [OrderType], [ShipModeID], [Supp1stCfmDate], [BrandSuppCode], [BrandSuppName], [CountryofLoading], [SupdelRvsd], [ProdItem], [KPILETA], [MaterialConfirm], [SupplierGroup], [TransferBIDate])
	select [Factory], [Country], [Brand], [WeaveType], [ETD], [ETA], [CloseDate], [ActDate], [Category], [OrderID], [Seq1], [Seq2], [OrderCfmDate], [SciDelivery], [Refno], [SCIRefno], [SuppID], [SuppName], [CurrencyID], [CurrencyRate], [Price], [Price(TWD)], [Unit], [PoQty], [PoFoc], [ShipQty], [ShipFoc], [TTShipQty], [ShipAmt(TWD)], [FabricJunk], [WKID], [ShipmentTerm], [FabricType], [PINO], [PIDATE], [Color], [ColorName], [Season], [PCHandle], [POHandle], [POSMR], [Style], [OrderType], [ShipModeID], [Supp1stCfmDate], [BrandSuppCode], [BrandSuppName], [CountryofLoading], [SupdelRvsd], [ProdItem], [KPILETA], [MaterialConfirm], [SupplierGroup], [TransferBIDate]
	from #Final t
	where not exists (select 1 from P_MtltoFTYAnalysis p where p.WKID = t.WKID and p.OrderID = t.OrderID and p.Seq1 = t.Seq1 and p.Seq2 = t.Seq2)

	if exists (select 1 from BITableInfo b where b.id = 'P_MtltoFTYAnalysis')
	begin
		update b
			set b.TransferDate = getdate()				
		from BITableInfo b
		where b.id = 'P_MtltoFTYAnalysis'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_MtltoFTYAnalysis', getdate())
	end
End
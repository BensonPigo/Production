Create PROCEDURE [dbo].[P_Import_PPIC_MASTER_LIST] 
	 @SCIDeliveryS as date = null,
	 @SCIDeliveryE as date = null
AS
BEGIN
	SET NOCOUNT ON;

	create table #tmp_P_PPICMASTERLIST (
			[M] VARCHAR(8) NULL ,
			[FactoryID] VARCHAR(8) NULL ,
			[Delivery] DATE NULL ,
			[Delivery(YYYYMM)] VARCHAR(6) NULL ,
			[Earliest SCIDlv] DATE NULL ,
			[SCIDlv] DATE NULL ,
			[KEY] VARCHAR(6) NULL ,
			[IDD] varchar(500) null,
			[CRD] DATE NULL ,
			[CRD(YYYYMM)] VARCHAR(6) NULL ,
			[Check CRD] VARCHAR(1) NULL ,
			[OrdCFM] DATE NULL ,
			[CRD-OrdCFM] INT NULL ,
			[SPNO] VARCHAR(13) NULL ,
			[Category] VARCHAR(20) NULL ,
			[Est. download date] VARCHAR(16) NULL ,
			[Buy Back] VARCHAR(1) NULL ,
			[Cancelled] VARCHAR(1) NULL ,
			[NeedProduction] VARCHAR(1) NULL ,
			[Dest] VARCHAR(30) NULL ,
			[Style] VARCHAR(15) NULL ,
			[Style Name] NVARCHAR(50) NULL ,
			[Modular Parent] VARCHAR(20) NULL ,
			[CPUAdjusted] NUMERIC(38, 6) NULL ,
			[Similar Style] VARCHAR(MAX) NULL ,
			[Season] VARCHAR(10) NULL ,
			[Garment L/T] NUMERIC(3,0) NULL ,
			[Order Type] VARCHAR(20) NULL ,
			[Project] VARCHAR(5) NULL ,
			[PackingMethod] NVARCHAR(50) NULL ,
			[Hanger pack] BIT NULL ,
			[Order#] VARCHAR(30) NULL ,
			[Buy Month] VARCHAR(16) NULL ,
			[PONO] VARCHAR(30) NULL ,
			[VAS/SHAS] VARCHAR(1) NULL ,
			[VAS/SHAS Apv.] DATETIME NULL ,
			[VAS/SHAS Cut Off Date] DATETIME NULL ,
			[M/Notice Date] DATETIME NULL ,
			[Est M/Notice Apv.] DATE NULL ,
			[Tissue] VARCHAR(1) NULL ,
			[AF by adidas] VARCHAR(1) NULL ,
			[Factory Disclaimer] NVARCHAR(50) NULL ,
			[Factory Disclaimer Remark] NVARCHAR(MAX) NULL ,
			[Approved/Rejected Date] DATE NULL ,
			[Global Foundation Range] VARCHAR(1) NULL ,
			[Brand] VARCHAR(8) NULL ,
			[Cust CD] VARCHAR(16) NULL ,
			[KIT] VARCHAR(10) NULL ,
			[Fty Code] VARCHAR(10) NULL ,
			[Program] NVARCHAR(12) NULL ,
			[Non Revenue] VARCHAR(1) NULL ,
			[New CD Code] VARCHAR(5) NULL ,
			[ProductType] NVARCHAR(500) NULL ,
			[FabricType] NVARCHAR(500) NULL ,
			[Lining] VARCHAR(20) NULL ,
			[Gender] VARCHAR(10) NULL ,
			[Construction] NVARCHAR(50) NULL ,
			[Cpu] NUMERIC(38, 6) NULL ,
			[Qty] INT NULL ,
			[FOC Qty] INT NULL ,
			[Total CPU] NUMERIC(38, 6) NULL ,
			[SewQtyTop] INT NULL ,
			[SewQtyBottom] INT NULL ,
			[SewQtyInner] INT NULL ,
			[SewQtyOuter] INT NULL ,
			[Total Sewing Output] INT NULL ,
			[Cut Qty] NUMERIC(38, 6) NULL ,
			[By Comb] VARCHAR(1) NULL ,
			[Cutting Status] VARCHAR(1) NULL ,
			[Packing Qty] INT NULL ,
			[Packing FOC Qty] INT NULL ,
			[Booking Qty] INT NULL ,
			[FOC Adj Qty] INT NULL ,
			[Not FOC Adj Qty] INT NULL ,
			[FOB]　numeric(16,4)　NULL,
			[Total]　numeric(38,6) NULL,
			[KPI L/ETA] DATE NULL ,
			[PF ETA (SP)] DATE NULL ,
			[Pull Forward Remark] VARCHAR(MAX) NULL ,
			[Pack L/ETA] DATE NULL ,
			[SCHD L/ETA] DATE NULL ,
			[Actual Mtl. ETA] DATE NULL ,
			[Fab ETA] DATE NULL ,
			[Acc ETA] DATE NULL ,
			[Sewing Mtl Complt(SP)] VARCHAR(1) NULL ,
			[Packing Mtl Complt(SP)] VARCHAR(1) NULL ,
			[Sew. MTL ETA (SP)] DATE NULL ,
			[Pkg. MTL ETA (SP)] DATE NULL ,
			[MTL Delay] VARCHAR(1) NULL ,
			[MTL Cmplt] VARCHAR(3) NULL ,
			[MTL Cmplt (SP)] VARCHAR(1) NULL ,
			[Arrive W/H Date] DATE NULL ,
			[Sewing InLine] DATE NULL ,
			[Sewing OffLine] DATE NULL ,
			[1st Sewn Date] DATE NULL ,
			[Last Sewn Date] DATE NULL ,
			[First Production Date] DATE NULL ,
			[Last Production Date] DATE NULL ,
			[Each Cons Apv Date] DATETIME NULL ,
			[Est Each Con Apv.] DATE NULL ,
			[Cutting InLine] DATE NULL ,
			[Cutting OffLine] DATE NULL ,
			[Cutting InLine(SP)] DATE NULL,
			[Cutting OffLine(SP)] DATE NULL,
			[1st Cut Date] DATE NULL ,
			[Last Cut Date] DATE NULL ,
			[Est. Pullout] DATE NULL ,
			[Act. Pullout Date] DATE NULL ,
			[Pullout Qty] INT NULL ,
			[Act. Pullout Times] INT NULL ,
			[Act. Pullout Cmplt] VARCHAR(2) NULL ,
			[KPI Delivery Date] DATE NULL ,
			[Update Delivery Reason] NVARCHAR(500) NULL ,
			[Plan Date] DATE NULL ,
			[Original Buyer Delivery Date] DATE NULL ,
			[SMR] VARCHAR(10) NULL ,
			[SMR Name] VARCHAR(50) NULL ,
			[Handle] VARCHAR(10) NULL ,
			[Handle Name] VARCHAR(50) NULL ,
			[Posmr] VARCHAR(10) NULL ,
			[Posmr Name] VARCHAR(50) NULL ,
			[PoHandle] VARCHAR(10) NULL ,
			[PoHandle Name] VARCHAR(50) NULL ,
			[PCHandle] VARCHAR(10) NULL ,
			[PCHandle Name] VARCHAR(50) NULL ,
			[MCHandle] VARCHAR(10) NULL ,
			[MCHandle Name] VARCHAR(50) NULL ,
			[DoxType] VARCHAR(8) NULL ,
			[Packing CTN] INT NULL ,
			[TTLCTN] INT NULL ,
			[Pack Error CTN] INT NULL ,
			[FtyCTN] INT NULL ,
			[cLog CTN] INT NULL ,
			[CFA CTN] INT NULL ,
			[cLog Rec. Date] DATE NULL ,
			[Final Insp. Date] VARCHAR(500) NULL ,
			[Insp. Result] VARCHAR(500) NULL ,
			[CFA Name] VARCHAR(500) NULL ,
			[Sewing Line#] VARCHAR(60) NULL ,
			[ShipMode] VARCHAR(30) NULL ,
			[SI#] VARCHAR(30) NULL ,
			[ColorWay] NVARCHAR(MAX) NULL ,
			[Special Mark] VARCHAR(50) NULL ,
			[Fty Remark] NVARCHAR(MAX) NULL ,
			[Sample Reason] NVARCHAR(500) NULL ,
			[IS MixMarker] VARCHAR(25) NULL ,
			[Cutting SP] VARCHAR(13) NULL ,
			[Rainwear test] VARCHAR(1) NULL ,
			[TMS] NUMERIC(38, 6) NULL ,
			[MD room scan date] DATETIME NULL ,
			[Dry Room received date] DATETIME NULL ,
			[Dry room trans date] DATETIME NULL ,
			[Last ctn trans date] DATETIME NULL ,
			[Last ctn recvd date] DATETIME NULL ,
			[OrganicCotton] VARCHAR(1) NULL ,
			[Direct Ship] VARCHAR(1) NULL ,
			[StyleCarryover] VARCHAR(1) NULL
	)

	create table #tmp_P_PPICMasterList_Extend (
		[OrderID] [varchar](13) NULL,
		[ColumnName] [varchar](50) NULL,
		[ColumnValue] numeric(38, 6) NULL
	)

	insert into #tmp_P_PPICMASTERLIST
	exec [MainServer].[Production].[dbo].[PPIC_R03_FORBI] @SCIDeliveryS, @SCIDeliveryE, 1

	insert into #tmp_P_PPICMasterList_Extend
	exec [MainServer].[Production].[dbo].[PPIC_R03_FORBI] @SCIDeliveryS, @SCIDeliveryE, 2

	insert into P_PPICMASTERLIST([M], [FactoryID], [Delivery], [Delivery(YYYYMM)], [Earliest SCIDlv], [SCIDlv], [KEY], [IDD]
		, [CRD], [CRD(YYYYMM)], [Check CRD], [OrdCFM], [CRD-OrdCFM], [SPNO], [Category], [Est. download date], [Buy Back]
		, [Cancelled], [NeedProduction], [Dest], [Style], [Style Name], [Modular Parent], [CPUAdjusted], [Similar Style]
		, [Season], [Garment L/T], [Order Type], [Project], [PackingMethod], [Hanger pack], [Order#], [Buy Month], [PONO]
		, [VAS/SHAS], [VAS/SHAS Apv.], [VAS/SHAS Cut Off Date], [M/Notice Date], [Est M/Notice Apv.], [Tissue], [AF by adidas]
		, [Factory Disclaimer], [Factory Disclaimer Remark], [Approved/Rejected Date], [Global Foundation Range], [Brand], [Cust CD]
		, [KIT], [Fty Code], [Program], [Non Revenue], [New CD Code], [ProductType], [FabricType], [Lining], [Gender], [Construction]
		, [Cpu], [Qty], [FOC Qty], [Total CPU], [SewQtyTop], [SewQtyBottom], [SewQtyInner], [SewQtyOuter], [Total Sewing Output]
		, [Cut Qty], [By Comb], [Cutting Status], [Packing Qty], [Packing FOC Qty], [Booking Qty], [FOC Adj Qty], [Not FOC Adj Qty]
		, [FOB], [Total], [KPI L/ETA], [PF ETA (SP)], [Pull Forward Remark], [Pack L/ETA], [SCHD L/ETA], [Actual Mtl. ETA], [Fab ETA]
		, [Acc ETA], [Sewing Mtl Complt(SP)], [Packing Mtl Complt(SP)], [Sew. MTL ETA (SP)], [Pkg. MTL ETA (SP)], [MTL Delay], [MTL Cmplt]
		, [MTL Cmplt (SP)], [Arrive W/H Date], [Sewing InLine], [Sewing OffLine], [1st Sewn Date], [Last Sewn Date], [First Production Date]
		, [Last Production Date], [Each Cons Apv Date], [Est Each Con Apv.], [Cutting InLine], [Cutting OffLine], [Cutting InLine(SP)]
		, [Cutting OffLine(SP)], [1st Cut Date], [Last Cut Date], [Est. Pullout], [Act. Pullout Date], [Pullout Qty], [Act. Pullout Times]
		, [Act. Pullout Cmplt], [KPI Delivery Date], [Update Delivery Reason], [Plan Date], [Original Buyer Delivery Date], [SMR], [SMR Name]
		, [Handle], [Handle Name], [Posmr], [Posmr Name], [PoHandle], [PoHandle Name], [PCHandle], [PCHandle Name], [MCHandle], [MCHandle Name]
		, [DoxType], [Packing CTN], [TTLCTN], [Pack Error CTN], [FtyCTN], [cLog CTN], [CFA CTN], [cLog Rec. Date], [Final Insp. Date]
		, [Insp. Result], [CFA Name], [Sewing Line#], [ShipMode], [SI#], [ColorWay], [Special Mark], [Fty Remark], [Sample Reason], [IS MixMarker]
		, [Cutting SP], [Rainwear test], [TMS], [MD room scan date], [Dry Room received date], [Dry room trans date], [Last ctn trans date]
		, [Last ctn recvd date], [OrganicCotton], [Direct Ship], [StyleCarryover])
	select ISNULL(t.[M], '')
		, ISNULL(t.[FactoryID], '')
		, [Delivery]
		, ISNULL(t.[Delivery(YYYYMM)], '')
		, [Earliest SCIDlv]
		, [SCIDlv]
		, ISNULL(t.[KEY], '')
		, ISNULL(t.[IDD], '')
		, [CRD]
		, ISNULL(t.[CRD(YYYYMM)], '')
		, ISNULL(t.[Check CRD], '')
		, [OrdCFM]
		, ISNULL(t.[CRD-OrdCFM], 0)
		, ISNULL(t.[SPNO], '')
		, ISNULL(t.[Category], '')
		, ISNULL(t.[Est. download date], '')
		, ISNULL(t.[Buy Back], '')
		, ISNULL(t.[Cancelled], '')
		, ISNULL(t.[NeedProduction], '')
		, ISNULL(t.[Dest], '')
		, ISNULL(t.[Style], '')
		, ISNULL(t.[Style Name], '')
		, ISNULL(t.[Modular Parent], '')
		, ISNULL(t.[CPUAdjusted], 0)
		, ISNULL(t.[Similar Style], '')
		, ISNULL(t.[Season], '')
		, ISNULL(t.[Garment L/T], 0)
		, ISNULL(t.[Order Type], '')
		, ISNULL(t.[Project], '')
		, ISNULL(t.[PackingMethod], '')
		, ISNULL(t.[Hanger pack], 0)
		, ISNULL(t.[Order#], '')
		, ISNULL(t.[Buy Month], '')
		, ISNULL(t.[PONO], '')
		, ISNULL(t.[VAS/SHAS], '')
		, [VAS/SHAS Apv.]
		, [VAS/SHAS Cut Off Date]
		, [M/Notice Date]
		, [Est M/Notice Apv.]
		, ISNULL(t.[Tissue], '')
		, ISNULL(t.[AF by adidas], '')
		, ISNULL(t.[Factory Disclaimer], '')
		, ISNULL(t.[Factory Disclaimer Remark], '')
		, [Approved/Rejected Date]
		, ISNULL(t.[Global Foundation Range], '')
		, ISNULL(t.[Brand], '')
		, ISNULL(t.[Cust CD], '')
		, ISNULL(t.[KIT], '')
		, ISNULL(t.[Fty Code], '')
		, ISNULL(t.[Program], '')
		, ISNULL(t.[Non Revenue], '')
		, ISNULL(t.[New CD Code], '')
		, ISNULL(t.[ProductType], '')
		, ISNULL(t.[FabricType], '')
		, ISNULL(t.[Lining], '')
		, ISNULL(t.[Gender], '')
		, ISNULL(t.[Construction], '')
		, ISNULL(t.[Cpu], 0)
		, ISNULL(t.[Qty], 0)
		, ISNULL(t.[FOC Qty], 0)
		, ISNULL(t.[Total CPU], 0)
		, ISNULL(t.[SewQtyTop], 0)
		, ISNULL(t.[SewQtyBottom], 0)
		, ISNULL(t.[SewQtyInner], 0)
		, ISNULL(t.[SewQtyOuter], 0)
		, ISNULL(t.[Total Sewing Output], 0)
		, ISNULL(t.[Cut Qty], 0)
		, ISNULL(t.[By Comb], '')
		, ISNULL(t.[Cutting Status], '')
		, ISNULL(t.[Packing Qty], 0)
		, ISNULL(t.[Packing FOC Qty], 0)
		, ISNULL(t.[Booking Qty], 0)
		, ISNULL(t.[FOC Adj Qty], 0)
		, ISNULL(t.[Not FOC Adj Qty], 0)
		, ISNULL(t.[FOB], 0)
		, ISNULL(t.[Total], 0)
		, [KPI L/ETA]
		, [PF ETA (SP)]
		, ISNULL(t.[Pull Forward Remark], '')
		, [Pack L/ETA]
		, [SCHD L/ETA]
		, [Actual Mtl. ETA]
		, [Fab ETA]
		, [Acc ETA]
		, ISNULL(t.[Sewing Mtl Complt(SP)], '')
		, ISNULL(t.[Packing Mtl Complt(SP)], '')
		, [Sew. MTL ETA (SP)]
		, [Pkg. MTL ETA (SP)]
		, ISNULL(t.[MTL Delay], '')
		, ISNULL(t.[MTL Cmplt], '')
		, ISNULL(t.[MTL Cmplt (SP)], '')
		, [Arrive W/H Date]
		, [Sewing InLine]
		, [Sewing OffLine]
		, [1st Sewn Date]
		, [Last Sewn Date]
		, [First Production Date]
		, [Last Production Date]
		, [Each Cons Apv Date]
		, [Est Each Con Apv.]
		, [Cutting InLine]
		, [Cutting OffLine]
		, [Cutting InLine(SP)]
		, [Cutting OffLine(SP)]
		, [1st Cut Date]
		, [Last Cut Date]
		, [Est. Pullout]
		, [Act. Pullout Date]
		, ISNULL(t.[Pullout Qty], 0)
		, ISNULL(t.[Act. Pullout Times], 0)
		, ISNULL(t.[Act. Pullout Cmplt], '')
		, [KPI Delivery Date]
		, ISNULL(t.[Update Delivery Reason], '')
		, [Plan Date]
		, [Original Buyer Delivery Date]
		, ISNULL(t.[SMR], '')
		, ISNULL(t.[SMR Name], '')
		, ISNULL(t.[Handle], '')
		, ISNULL(t.[Handle Name], '')
		, ISNULL(t.[Posmr], '')
		, ISNULL(t.[Posmr Name], '')
		, ISNULL(t.[PoHandle], '')
		, ISNULL(t.[PoHandle Name], '')
		, ISNULL(t.[PCHandle], '')
		, ISNULL(t.[PCHandle Name], '')
		, ISNULL(t.[MCHandle], '')
		, ISNULL(t.[MCHandle Name], '')
		, ISNULL(t.[DoxType], '')
		, ISNULL(t.[Packing CTN], 0)
		, ISNULL(t.[TTLCTN], 0)
		, ISNULL(t.[Pack Error CTN], 0)
		, ISNULL(t.[FtyCTN], 0)
		, ISNULL(t.[cLog CTN], 0)
		, ISNULL(t.[CFA CTN], 0)
		, [cLog Rec. Date]
		, [Final Insp. Date]
		, ISNULL(t.[Insp. Result], '')
		, ISNULL(t.[CFA Name], '')
		, ISNULL(t.[Sewing Line#], '')
		, ISNULL(t.[ShipMode], '')
		, ISNULL(t.[SI#], '')
		, ISNULL(t.[ColorWay], '')
		, ISNULL(t.[Special Mark], '')
		, ISNULL(t.[Fty Remark], '')
		, ISNULL(t.[Sample Reason], '')
		, ISNULL(t.[IS MixMarker], '')
		, ISNULL(t.[Cutting SP], '')
		, ISNULL(t.[Rainwear test], '')
		, ISNULL(t.[TMS], 0)
		, [MD room scan date]
		, [Dry Room received date]
		, [Dry room trans date]
		, [Last ctn trans date]
		, [Last ctn recvd date]
		, ISNULL(t.[OrganicCotton], '')
		, ISNULL(t.[Direct Ship], '')
		, ISNULL(t.[StyleCarryover], '')
	from #tmp_P_PPICMASTERLIST t
	where not exists (select 1 from P_PPICMASTERLIST p where t.[SPNO] = p.[SPNO])

	update p 
		set p.[M] = ISNULL(t.[M], '')
			, p.[FactoryID] = ISNULL(t.[FactoryID], '')
			, p.[Delivery] = t.[Delivery]
			, p.[Delivery(YYYYMM)] = ISNULL(t.[Delivery(YYYYMM)], '')
			, p.[Earliest SCIDlv] = t.[Earliest SCIDlv]
			, p.[SCIDlv] = t.[SCIDlv]
			, p.[KEY] = ISNULL(t.[KEY], '')
			, p.[IDD] = ISNULL(t.[IDD], '')
			, p.[CRD] = t.[CRD]
			, p.[CRD(YYYYMM)] = ISNULL(t.[CRD(YYYYMM)], '')
			, p.[Check CRD] = ISNULL(t.[Check CRD], '')
			, p.[OrdCFM] = t.[OrdCFM]
			, p.[CRD-OrdCFM] = ISNULL(t.[CRD-OrdCFM], 0)
			, p.[SPNO] = ISNULL(t.[SPNO], '')
			, p.[Category] = ISNULL(t.[Category], '')
			, p.[Est. download date] = ISNULL(t.[Est. download date], '')
			, p.[Buy Back] = ISNULL(t.[Buy Back], '')
			, p.[Cancelled] = ISNULL(t.[Cancelled], '')
			, p.[NeedProduction] = ISNULL(t.[NeedProduction], '')
			, p.[Dest] = ISNULL(t.[Dest], '')
			, p.[Style] = ISNULL(t.[Style], '')
			, p.[Style Name] = ISNULL(t.[Style Name], '')
			, p.[Modular Parent] = ISNULL(t.[Modular Parent], '')
			, p.[CPUAdjusted] = ISNULL(t.[CPUAdjusted], 0)
			, p.[Similar Style] = ISNULL(t.[Similar Style], '')
			, p.[Season] = ISNULL(t.[Season], '')
			, p.[Garment L/T] = ISNULL(t.[Garment L/T], 0)
			, p.[Order Type] = ISNULL(t.[Order Type], '')
			, p.[Project] = ISNULL(t.[Project], '')
			, p.[PackingMethod] = ISNULL(t.[PackingMethod], '')
			, p.[Hanger pack] = ISNULL(t.[Hanger pack], 0)
			, p.[Order#] = ISNULL(t.[Order#], '')
			, p.[Buy Month] = ISNULL(t.[Buy Month], '')
			, p.[PONO] = ISNULL(t.[PONO], '')
			, p.[VAS/SHAS] = ISNULL(t.[VAS/SHAS], '')
			, p.[VAS/SHAS Apv.] = t.[VAS/SHAS Apv.]
			, p.[VAS/SHAS Cut Off Date] = t.[VAS/SHAS Cut Off Date]
			, p.[M/Notice Date] = t.[M/Notice Date]
			, p.[Est M/Notice Apv.] = t.[Est M/Notice Apv.]
			, p.[Tissue] = ISNULL(t.[Tissue], '')
			, p.[AF by adidas] = ISNULL(t.[AF by adidas], '')
			, p.[Factory Disclaimer] = ISNULL(t.[Factory Disclaimer], '')
			, p.[Factory Disclaimer Remark] = ISNULL(t.[Factory Disclaimer Remark], '')
			, p.[Approved/Rejected Date] = t.[Approved/Rejected Date]
			, p.[Global Foundation Range] = ISNULL(t.[Global Foundation Range], '')
			, p.[Brand] = ISNULL(t.[Brand], '')
			, p.[Cust CD] = ISNULL(t.[Cust CD], '')
			, p.[KIT] = ISNULL(t.[KIT], '')
			, p.[Fty Code] = ISNULL(t.[Fty Code], '')
			, p.[Program] = ISNULL(t.[Program], '')
			, p.[Non Revenue] = ISNULL(t.[Non Revenue], '')
			, p.[New CD Code] = ISNULL(t.[New CD Code], '')
			, p.[ProductType] = ISNULL(t.[ProductType], '')
			, p.[FabricType] = ISNULL(t.[FabricType], '')
			, p.[Lining] = ISNULL(t.[Lining], '')
			, p.[Gender] = ISNULL(t.[Gender], '')
			, p.[Construction] = ISNULL(t.[Construction], '')
			, p.[Cpu] = ISNULL(t.[Cpu], 0)
			, p.[Qty] = ISNULL(t.[Qty], 0)
			, p.[FOC Qty] = ISNULL(t.[FOC Qty], 0)
			, p.[Total CPU] = ISNULL(t.[Total CPU], 0)
			, p.[SewQtyTop] = ISNULL(t.[SewQtyTop], 0)
			, p.[SewQtyBottom] = ISNULL(t.[SewQtyBottom], 0)
			, p.[SewQtyInner] = ISNULL(t.[SewQtyInner], 0)
			, p.[SewQtyOuter] = ISNULL(t.[SewQtyOuter], 0)
			, p.[Total Sewing Output] = ISNULL(t.[Total Sewing Output], 0)
			, p.[Cut Qty] = ISNULL(t.[Cut Qty], 0)
			, p.[By Comb] = ISNULL(t.[By Comb], '')
			, p.[Cutting Status] = ISNULL(t.[Cutting Status], '')
			, p.[Packing Qty] = ISNULL(t.[Packing Qty], 0)
			, p.[Packing FOC Qty] = ISNULL(t.[Packing FOC Qty], 0)
			, p.[Booking Qty] = ISNULL(t.[Booking Qty], 0)
			, p.[FOC Adj Qty] = ISNULL(t.[FOC Adj Qty], 0)
			, p.[Not FOC Adj Qty] = ISNULL(t.[Not FOC Adj Qty], 0)
			, p.[FOB] = ISNULL(t.[FOB], 0)
			, p.[Total] = ISNULL(t.[Total], 0)
			, p.[KPI L/ETA] = t.[KPI L/ETA]
			, p.[PF ETA (SP)] = t.[PF ETA (SP)]
			, p.[Pull Forward Remark] = ISNULL(t.[Pull Forward Remark], '')
			, p.[Pack L/ETA] = t.[Pack L/ETA]
			, p.[SCHD L/ETA] = t.[SCHD L/ETA]
			, p.[Actual Mtl. ETA] = t.[Actual Mtl. ETA]
			, p.[Fab ETA] = t.[Fab ETA]
			, p.[Acc ETA] = t.[Acc ETA]
			, p.[Sewing Mtl Complt(SP)] = ISNULL(t.[Sewing Mtl Complt(SP)], '')
			, p.[Packing Mtl Complt(SP)] = ISNULL(t.[Packing Mtl Complt(SP)], '')
			, p.[Sew. MTL ETA (SP)] = t.[Sew. MTL ETA (SP)]
			, p.[Pkg. MTL ETA (SP)] = t.[Pkg. MTL ETA (SP)]
			, p.[MTL Delay] = ISNULL(t.[MTL Delay], '')
			, p.[MTL Cmplt] = ISNULL(t.[MTL Cmplt], '')
			, p.[MTL Cmplt (SP)] = ISNULL(t.[MTL Cmplt (SP)], '')
			, p.[Arrive W/H Date] = t.[Arrive W/H Date]
			, p.[Sewing InLine] = t.[Sewing InLine]
			, p.[Sewing OffLine] = t.[Sewing OffLine]
			, p.[1st Sewn Date] = t.[1st Sewn Date]
			, p.[Last Sewn Date] = t.[Last Sewn Date]
			, p.[First Production Date] = t.[First Production Date]
			, p.[Last Production Date] = t.[Last Production Date]
			, p.[Each Cons Apv Date] = t.[Each Cons Apv Date]
			, p.[Est Each Con Apv.] = t.[Est Each Con Apv.]
			, p.[Cutting InLine] = t.[Cutting InLine]
			, p.[Cutting OffLine] = t.[Cutting OffLine]
			, p.[Cutting InLine(SP)] = t.[Cutting InLine(SP)]
			, p.[Cutting OffLine(SP)] = t.[Cutting OffLine(SP)]
			, p.[1st Cut Date] = t.[1st Cut Date]
			, p.[Last Cut Date] = t.[Last Cut Date]
			, p.[Est. Pullout] = t.[Est. Pullout]
			, p.[Act. Pullout Date] = t.[Act. Pullout Date]
			, p.[Pullout Qty] = ISNULL(t.[Pullout Qty], 0)
			, p.[Act. Pullout Times] = ISNULL(t.[Act. Pullout Times], 0)
			, p.[Act. Pullout Cmplt] = ISNULL(t.[Act. Pullout Cmplt], '')
			, p.[KPI Delivery Date] = t.[KPI Delivery Date]
			, p.[Update Delivery Reason] = ISNULL(t.[Update Delivery Reason], '')
			, p.[Plan Date] = t.[Plan Date]
			, p.[Original Buyer Delivery Date] = t.[Original Buyer Delivery Date]
			, p.[SMR] = ISNULL(t.[SMR], '')
			, p.[SMR Name] = ISNULL(t.[SMR Name], '')
			, p.[Handle] = ISNULL(t.[Handle], '')
			, p.[Handle Name] = ISNULL(t.[Handle Name], '')
			, p.[Posmr] = ISNULL(t.[Posmr], '')
			, p.[Posmr Name] = ISNULL(t.[Posmr Name], '')
			, p.[PoHandle] = ISNULL(t.[PoHandle], '')
			, p.[PoHandle Name] = ISNULL(t.[PoHandle Name], '')
			, p.[PCHandle] = ISNULL(t.[PCHandle], '')
			, p.[PCHandle Name] = ISNULL(t.[PCHandle Name], '')
			, p.[MCHandle] = ISNULL(t.[MCHandle], '')
			, p.[MCHandle Name] = ISNULL(t.[MCHandle Name], '')
			, p.[DoxType] = ISNULL(t.[DoxType], '')
			, p.[Packing CTN] = ISNULL(t.[Packing CTN], 0)
			, p.[TTLCTN] = ISNULL(t.[TTLCTN], 0)
			, p.[Pack Error CTN] = ISNULL(t.[Pack Error CTN], 0)
			, p.[FtyCTN] = ISNULL(t.[FtyCTN], 0)
			, p.[cLog CTN] = ISNULL(t.[cLog CTN], 0)
			, p.[CFA CTN] = ISNULL(t.[CFA CTN], 0)
			, p.[cLog Rec. Date] = t.[cLog Rec. Date]
			, p.[Final Insp. Date] = t.[Final Insp. Date]
			, p.[Insp. Result] = ISNULL(t.[Insp. Result], '')
			, p.[CFA Name] = ISNULL(t.[CFA Name], '')
			, p.[Sewing Line#] = ISNULL(t.[Sewing Line#], '')
			, p.[ShipMode] = ISNULL(t.[ShipMode], '')
			, p.[SI#] = ISNULL(t.[SI#], '')
			, p.[ColorWay] = ISNULL(t.[ColorWay], '')
			, p.[Special Mark] = ISNULL(t.[Special Mark], '')
			, p.[Fty Remark] = ISNULL(t.[Fty Remark], '')
			, p.[Sample Reason] = ISNULL(t.[Sample Reason], '')
			, p.[IS MixMarker] = ISNULL(t.[IS MixMarker], '')
			, p.[Cutting SP] = ISNULL(t.[Cutting SP], '')
			, p.[Rainwear test] = ISNULL(t.[Rainwear test], '')
			, p.[TMS] = ISNULL(t.[TMS], 0)
			, p.[MD room scan date] = t.[MD room scan date]
			, p.[Dry Room received date] = t.[Dry Room received date]
			, p.[Dry room trans date] = t.[Dry room trans date]
			, p.[Last ctn trans date] = t.[Last ctn trans date]
			, p.[Last ctn recvd date] = t.[Last ctn recvd date]
			, p.[OrganicCotton] = ISNULL(t.[OrganicCotton], '')
			, p.[Direct Ship] = ISNULL(t.[Direct Ship], '')
			, p.[StyleCarryover] = ISNULL(t.[StyleCarryover], '')
	from P_PPICMASTERLIST p 
	inner join #tmp_P_PPICMASTERLIST t on p.[SPNO] = t.[SPNO]

	insert into P_PPICMasterList_Extend(OrderID, ColumnName, ColumnValue)
	select OrderID, ColumnName, isnull(ColumnValue, '')
	from #tmp_P_PPICMasterList_Extend t
	where not exists (select 1 from P_PPICMasterList_Extend p where t.OrderID = p.OrderID and t.ColumnName = p.ColumnName)

	update p 
		set p.[ColumnValue] = ISNULL(t.[ColumnValue], '')
	from P_PPICMasterList_Extend p 
	inner join #tmp_P_PPICMasterList_Extend t on t.OrderID = p.OrderID and t.ColumnName = p.ColumnName

	drop table #tmp_P_PPICMASTERLIST, #tmp_P_PPICMasterList_Extend

	if exists (select 1 from BITableInfo b where b.id = 'P_PPICMASTERLIST')
	begin
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = 'P_PPICMASTERLIST'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_PPICMASTERLIST', getdate())
	end

end
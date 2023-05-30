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
			[IDD] DATE NULL ,
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
			[Factory Disclaimer Remark] NVARCHAR(500) NULL ,
			[Approved/Rejected Date] DATE NULL ,
			[Global Foundation Range] VARCHAR(1) NULL ,
			[Brand] VARCHAR(8) NULL ,
			[Cust CD] VARCHAR(16) NULL ,
			[KIT] VARCHAR(10) NULL ,
			[Fty Code] VARCHAR(10) NULL ,
			[Program ] NVARCHAR(12) NULL ,
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
			[KPI L/ETA] DATE NULL ,
			[PF ETA (SP)] DATE NULL ,
			[Pull Forward Remark] VARCHAR(MAX) NULL ,
			[SCHD L/ETA] DATE NULL ,
			[Actual Mtl. ETA] DATE NULL ,
			[Fab ETA] DATE NULL ,
			[Acc ETA] DATE NULL ,
			[Sewing Mtl Complt(SP)] VARCHAR(1) NULL ,
			[Packing Mtl Complt(SP)] VARCHAR(1) NULL ,
			[Sew. MTL ETA (SP)] DATE NULL ,
			[Pkg. MTL ETA (SP)] DATE NULL ,
			[MTL Delay] VARCHAR(1) NULL ,
			[MTL Cmplt] VARCHAR(2) NULL ,
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
			[Final Insp. Date] DATE NULL ,
			[Insp. Result] VARCHAR(16) NULL ,
			[CFA Name] VARCHAR(10) NULL ,
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
			[3FLATLOCK (TMS)] NUMERIC(38, 6) NULL ,
			[4FLATLOCK-H (TMS)] NUMERIC(38, 6) NULL ,
			[4FLATLOCK-S (TMS)] NUMERIC(38, 6) NULL ,
			[AE RING-KNIFE (TMS)] NUMERIC(38, 6) NULL ,
			[AE RING-ULTRASONIC (TMS)] NUMERIC(38, 6) NULL ,
			[AT (TMS)] NUMERIC(38, 6) NULL ,
			[AT (HAND) (TMS)] NUMERIC(38, 6) NULL ,
			[AT (MACHINE) (TMS)] NUMERIC(38, 6) NULL ,
			[AUTO BELT LOOPER (TMS)] NUMERIC(38, 6) NULL ,
			[AUTO POCKET WELT (TMS)] NUMERIC(38, 6) NULL ,
			[AUTO-TEMPLATE (TMS)] NUMERIC(38, 6) NULL ,
			[AUTO-TEMPLATE QT (TMS)] NUMERIC(38, 6) NULL ,
			[B-HOT PRESS(BONDING) (TMS)] NUMERIC(38, 6) NULL ,
			[B-HOT PRESS(SUB-PRT) (TMS)] NUMERIC(38, 6) NULL ,
			[BH-TWICE FOR PLACKET (TMS)] NUMERIC(38, 6) NULL ,
			[BH-TWICE FOR WB (TMS)] NUMERIC(38, 6) NULL ,
			[BIG HOT FOR BONDING (TMS)] NUMERIC(38, 6) NULL ,
			[BIG HOT PRESS (TMS)] NUMERIC(38, 6) NULL ,
			[BLINDSTITCH (TMS)] NUMERIC(38, 6) NULL ,
			[BONDING (HAND) (TMS)] NUMERIC(38, 6) NULL ,
			[BONDING (MACHINE) (TMS)] NUMERIC(38, 6) NULL ,
			[COVERSTITCH CYLINDER (TMS)] NUMERIC(38, 6) NULL ,
			[COVERSTITCH L-CUTTER (TMS)] NUMERIC(38, 6) NULL ,
			[CUTTING (TMS)] NUMERIC(38, 6) NULL ,
			[CUTTING TAPE (Price)] NUMERIC(38, 6) NULL ,
			[D-chain ZIG ZAG (TMS)] NUMERIC(38, 6) NULL ,
			[DIE CUT (TMS)] NUMERIC(38, 6) NULL ,
			[DIE-CUT (TMS)] NUMERIC(38, 6) NULL ,
			[DOWN (TMS)] NUMERIC(38, 6) NULL ,
			[DOWN FILLING (TMS)] NUMERIC(38, 6) NULL ,
			[EM/DEBOSS (I/H) (TMS)] NUMERIC(38, 6) NULL ,
			[EMBOSS/DEBOSS (PCS)] NUMERIC(38, 6) NULL ,
			[EMBOSS/DEBOSS (Price)] NUMERIC(38, 6) NULL ,
			[EMBROIDERY (STITCH)] NUMERIC(38, 6) NULL ,
			[EMBROIDERY (Price)] NUMERIC(38, 6) NULL ,
			[EMBROIDERY EYELET (TMS)] NUMERIC(38, 6) NULL ,
			[EMBROIDERY TMS (TMS)] NUMERIC(38, 6) NULL ,
			[FARM OUT QUILTING (PCS)] NUMERIC(38, 6) NULL ,
			[FARM OUT QUILTING (Price)] NUMERIC(38, 6) NULL ,
			[FEEDOFARM (TMS)] NUMERIC(38, 6) NULL ,
			[FLATLOCK (TMS)] NUMERIC(38, 6) NULL ,
			[Fusible (TMS)] NUMERIC(38, 6) NULL ,
			[Garment Dye (PCS)] NUMERIC(38, 6) NULL ,
			[Garment Dye (Price)] NUMERIC(38, 6) NULL ,
			[GMT WASH (PCS)] NUMERIC(38, 6) NULL ,
			[GMT WASH (Price)] NUMERIC(38, 6) NULL ,
			[HEAT SET PLEAT (PCS)] NUMERIC(38, 6) NULL ,
			[HEAT SET PLEAT (Price)] NUMERIC(38, 6) NULL ,
			[HEAT TRANSFER (PANEL)] NUMERIC(38, 6) NULL ,
			[HEAT TRANSFER (TMS)] NUMERIC(38, 6) NULL ,
			[INDIRECT MANPOWER (TMS)] NUMERIC(38, 6) NULL ,
			[INSPECTION (TMS)] NUMERIC(38, 6) NULL ,
			[INTENSIVE MACHINE (TMS)] NUMERIC(38, 6) NULL ,
			[JOKERTAG (TMS)] NUMERIC(38, 6) NULL ,
			[KEY BUTTONHOLE (TMS)] NUMERIC(38, 6) NULL ,
			[LASER (TMS)] NUMERIC(38, 6) NULL ,
			[LASER CUTTER (TMS)] NUMERIC(38, 6) NULL ,
			[LASER-AXLETREE (TMS)] NUMERIC(38, 6) NULL ,
			[LASER-GALVANOMETER (TMS)] NUMERIC(38, 6) NULL ,
			[PACKING (TMS)] NUMERIC(38, 6) NULL ,
			[PAD PRINTING (PCS)] NUMERIC(38, 6) NULL ,
			[PAD PRINTING (Price)] NUMERIC(38, 6) NULL ,
			[POCKET WELT] NUMERIC(38, 6) NULL ,
			[PRESSING (TMS)] NUMERIC(38, 6) NULL ,
			[PRINTING (PCS)] NUMERIC(38, 6) NULL ,
			[PRINTING (Price)] NUMERIC(38, 6) NULL ,
			[PRINTING PPU (PPU)] NUMERIC(38, 6) NULL ,
			[POSubCon] NVARCHAR(500) NULL ,
			[SubCon] NVARCHAR(500) NULL ,
			[QUILTING (Price)] NUMERIC(38, 6) NULL ,
			[QUILTING(AT) (TMS)] NUMERIC(38, 6) NULL ,
			[QUILTING(HAND) (TMS)] NUMERIC(38, 6) NULL ,
			[REAL FLATSEAM (TMS)] NUMERIC(38, 6) NULL ,
			[RECOAT Garment] NUMERIC(38, 6) NULL ,
			[REPAIR GARMENT (Price)] NUMERIC(38, 6) NULL ,
			[ROLLER SUBLIMATION (TMS)] NUMERIC(38, 6) NULL ,
			[SEAM TAPING MACHINE (TMS)] NUMERIC(38, 6) NULL ,
			[SEAMSEAL (TMS)] NUMERIC(38, 6) NULL ,
			[SEWING (TMS)] NUMERIC(38, 6) NULL ,
			[S-HOT PRESS(BONDING) (TMS)] NUMERIC(38, 6) NULL ,
			[S-HOT PRESS(HT) (TMS)] NUMERIC(38, 6) NULL ,
			[S-HOT PRESS(SEWING) (TMS)] NUMERIC(38, 6) NULL ,
			[SMALL HOT PRESS (TMS)] NUMERIC(38, 6) NULL ,
			[SMALL HOT PRESS-LONG (TMS)] NUMERIC(38, 6) NULL ,
			[SUBLIMATION PRINT (TMS)] NUMERIC(38, 6) NULL ,
			[SUBLIMATION ROLLER (TMS)] NUMERIC(38, 6) NULL ,
			[SUBLIMATION SPRAY (TMS)] NUMERIC(38, 6) NULL ,
			[ULTRASONIC (TMS)] NUMERIC(38, 6) NULL ,
			[ULTRASONIC MACHINE (TMS)] NUMERIC(38, 6) NULL ,
			[ULTRASONIC-ELASTIC (TMS)] NUMERIC(38, 6) NULL ,
			[ULTRASONIC-LABEL ASM (TMS)] NUMERIC(38, 6) NULL ,
			[ULTRASONIC-TAPE CUT (TMS)] NUMERIC(38, 6) NULL ,
			[VELCRO MACHINE (TMS)] NUMERIC(38, 6) NULL ,
			[VELCROED M/C (TMS)] NUMERIC(38, 6) NULL ,
			[WELDED (TMS)] NUMERIC(38, 6) NULL ,
			[WELTED M/C (TMS)] NUMERIC(38, 6) NULL ,
			[ZIG ZAG (TMS)] NUMERIC(38, 6) NULL ,
			[ZIPPER HOT PRESS (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_TMS] NUMERIC(38, 6) NULL ,
			[TTL_3FLATLOCK (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_4FLATLOCK-H (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_4FLATLOCK-S (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_AE RING-KNIFE (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_AE RING-ULTRASONIC (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_AT (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_AT (HAND) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_AT (MACHINE) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_AUTO BELT LOOPER (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_AUTO POCKET WELT (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_AUTO-TEMPLATE (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_AUTO-TEMPLATE QT (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_B-HOT PRESS(BONDING) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_B-HOT PRESS(SUB-PRT) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_BH-TWICE FOR PLACKET (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_BH-TWICE FOR WB (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_BIG HOT FOR BONDING (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_BIG HOT PRESS (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_BLINDSTITCH (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_BONDING (HAND) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_BONDING (MACHINE) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_COVERSTITCH CYLINDER (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_COVERSTITCH L-CUTTER (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_CUTTING (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_CUTTING TAPE (Price)] NUMERIC(38, 6) NULL ,
			[TTL_D-chain ZIG ZAG (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_DIE CUT (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_DIE-CUT (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_DOWN (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_DOWN FILLING (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_EM/DEBOSS (I/H) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_EMBOSS/DEBOSS (PCS)] NUMERIC(38, 6) NULL ,
			[TTL_EMBOSS/DEBOSS (Price)] NUMERIC(38, 6) NULL ,
			[TTL_EMBROIDERY (STITCH)] NUMERIC(38, 6) NULL ,
			[TTL_EMBROIDERY (Price)] NUMERIC(38, 6) NULL ,
			[TTL_EMBROIDERY EYELET (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_EMBROIDERY TMS (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_FARM OUT QUILTING (PCS)] NUMERIC(38, 6) NULL ,
			[TTL_FARM OUT QUILTING (Price)] NUMERIC(38, 6) NULL ,
			[TTL_FEEDOFARM (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_FLATLOCK (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_Fusible (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_Garment Dye (PCS)] NUMERIC(38, 6) NULL ,
			[TTL_Garment Dye (Price)] NUMERIC(38, 6) NULL ,
			[TTL_GMT WASH (PCS)] NUMERIC(38, 6) NULL ,
			[TTL_GMT WASH (Price)] NUMERIC(38, 6) NULL ,
			[TTL_HEAT SET PLEAT (PCS)] NUMERIC(38, 6) NULL ,
			[TTL_HEAT SET PLEAT (Price)] NUMERIC(38, 6) NULL ,
			[TTL_HEAT TRANSFER (PANEL)] NUMERIC(38, 6) NULL ,
			[TTL_HEAT TRANSFER (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_INDIRECT MANPOWER (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_INSPECTION (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_INTENSIVE MACHINE (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_JOKERTAG (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_KEY BUTTONHOLE (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_LASER (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_LASER CUTTER (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_LASER-AXLETREE (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_LASER-GALVANOMETER (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_PACKING (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_PAD PRINTING (PCS)] NUMERIC(38, 6) NULL ,
			[TTL_PAD PRINTING (Price)] NUMERIC(38, 6) NULL ,
			[TTL_POCKET WELT] NUMERIC(38, 6) NULL ,
			[TTL_PRESSING (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_PRINTING (PCS)] NUMERIC(38, 6) NULL ,
			[TTL_PRINTING (Price)] NUMERIC(38, 6) NULL ,
			[TTL_PRINTING PPU (PPU)] NUMERIC(38, 6) NULL ,
			[TTL_QUILTING (Price)] NUMERIC(38, 6) NULL ,
			[TTL_QUILTING(AT) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_QUILTING(HAND) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_REAL FLATSEAM (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_RECOAT Garment] NUMERIC(38, 6) NULL ,
			[TTL_REPAIR GARMENT (Price)] NUMERIC(38, 6) NULL ,
			[TTL_ROLLER SUBLIMATION (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_SEAM TAPING MACHINE (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_SEAMSEAL (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_SEWING (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_S-HOT PRESS(BONDING) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_S-HOT PRESS(HT) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_S-HOT PRESS(SEWING) (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_SMALL HOT PRESS (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_SMALL HOT PRESS-LONG (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_SUBLIMATION PRINT (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_SUBLIMATION ROLLER (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_SUBLIMATION SPRAY (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_ULTRASONIC (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_ULTRASONIC MACHINE (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_ULTRASONIC-ELASTIC (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_ULTRASONIC-LABEL ASM (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_ULTRASONIC-TAPE CUT (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_VELCRO MACHINE (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_VELCROED M/C (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_WELDED (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_WELTED M/C (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_ZIG ZAG (TMS)] NUMERIC(38, 6) NULL ,
			[TTL_ZIPPER HOT PRESS (TMS)] NUMERIC(38, 6) NULL 
	)

	insert into #tmp_P_PPICMASTERLIST
	exec [MainServer].[Production].[dbo].[PPIC_R03_FORBI] @SCIDeliveryS, @SCIDeliveryE 

	insert into P_PPICMASTERLIST([M], [FactoryID], [Delivery], [Delivery(YYYYMM)], [Earliest SCIDlv], [SCIDlv], [KEY], [IDD]
		, [CRD], [CRD(YYYYMM)], [Check CRD], [OrdCFM], [CRD-OrdCFM], [SPNO], [Category], [Est. download date], [Buy Back]
		, [Cancelled], [NeedProduction], [Dest], [Style], [Style Name], [Modular Parent], [CPUAdjusted], [Similar Style]
		, [Season], [Garment L/T], [Order Type], [Project], [PackingMethod], [Hanger pack], [Order#], [Buy Month], [PONO]
		, [VAS/SHAS], [VAS/SHAS Apv.], [VAS/SHAS Cut Off Date], [M/Notice Date], [Est M/Notice Apv.], [Tissue], [AF by adidas]
		, [Factory Disclaimer], [Factory Disclaimer Remark], [Approved/Rejected Date], [Global Foundation Range], [Brand]
		, [Cust CD], [KIT], [Fty Code], [Program ], [Non Revenue], [New CD Code], [ProductType], [FabricType], [Lining]
		, [Gender], [Construction], [Cpu], [Qty], [FOC Qty], [Total CPU], [SewQtyTop], [SewQtyBottom], [SewQtyInner], [SewQtyOuter]
		, [Total Sewing Output], [Cut Qty], [By Comb], [Cutting Status], [Packing Qty], [Packing FOC Qty], [Booking Qty]
		, [FOC Adj Qty], [Not FOC Adj Qty], [KPI L/ETA], [PF ETA (SP)], [Pull Forward Remark], [SCHD L/ETA], [Actual Mtl. ETA]
		, [Fab ETA], [Acc ETA], [Sewing Mtl Complt(SP)], [Packing Mtl Complt(SP)], [Sew. MTL ETA (SP)], [Pkg. MTL ETA (SP)]
		, [MTL Delay], [MTL Cmplt], [MTL Cmplt (SP)], [Arrive W/H Date], [Sewing InLine], [Sewing OffLine], [1st Sewn Date]
		, [Last Sewn Date], [First Production Date], [Last Production Date], [Each Cons Apv Date], [Est Each Con Apv.]
		, [Cutting InLine], [Cutting OffLine], [1st Cut Date], [Last Cut Date], [Est. Pullout], [Act. Pullout Date], [Pullout Qty]
		, [Act. Pullout Times], [Act. Pullout Cmplt], [KPI Delivery Date], [Update Delivery Reason], [Plan Date], [Original Buyer Delivery Date]
		, [SMR], [SMR Name], [Handle], [Handle Name], [Posmr], [Posmr Name], [PoHandle], [PoHandle Name], [PCHandle], [PCHandle Name]
		, [MCHandle], [MCHandle Name], [DoxType], [Packing CTN], [TTLCTN], [Pack Error CTN], [FtyCTN], [cLog CTN], [CFA CTN]
		, [cLog Rec. Date], [Final Insp. Date], [Insp. Result], [CFA Name], [Sewing Line#], [ShipMode], [SI#], [ColorWay], [Special Mark]
		, [Fty Remark], [Sample Reason], [IS MixMarker], [Cutting SP], [Rainwear test], [TMS], [MD room scan date], [Dry Room received date]
		, [Dry room trans date], [Last ctn trans date], [Last ctn recvd date], [OrganicCotton], [Direct Ship], [3FLATLOCK (TMS)]
		, [4FLATLOCK-H (TMS)], [4FLATLOCK-S (TMS)], [AE RING-KNIFE (TMS)], [AE RING-ULTRASONIC (TMS)], [AT (TMS)], [AT (HAND) (TMS)]
		, [AT (MACHINE) (TMS)], [AUTO BELT LOOPER (TMS)], [AUTO POCKET WELT (TMS)], [AUTO-TEMPLATE (TMS)], [AUTO-TEMPLATE QT (TMS)]
		, [B-HOT PRESS(BONDING) (TMS)], [B-HOT PRESS(SUB-PRT) (TMS)], [BH-TWICE FOR PLACKET (TMS)], [BH-TWICE FOR WB (TMS)]
		, [BIG HOT FOR BONDING (TMS)], [BIG HOT PRESS (TMS)], [BLINDSTITCH (TMS)], [BONDING (HAND) (TMS)], [BONDING (MACHINE) (TMS)]
		, [COVERSTITCH CYLINDER (TMS)], [COVERSTITCH L-CUTTER (TMS)], [CUTTING (TMS)], [CUTTING TAPE (Price)], [D-chain ZIG ZAG (TMS)]
		, [DIE CUT (TMS)], [DIE-CUT (TMS)], [DOWN (TMS)], [DOWN FILLING (TMS)], [EM/DEBOSS (I/H) (TMS)], [EMBOSS/DEBOSS (PCS)]
		, [EMBOSS/DEBOSS (Price)], [EMBROIDERY (STITCH)], [EMBROIDERY (Price)], [EMBROIDERY EYELET (TMS)], [EMBROIDERY TMS (TMS)]
		, [FARM OUT QUILTING (PCS)], [FARM OUT QUILTING (Price)], [FEEDOFARM (TMS)], [FLATLOCK (TMS)], [Fusible (TMS)], [Garment Dye (PCS)]
		, [Garment Dye (Price)], [GMT WASH (PCS)], [GMT WASH (Price)], [HEAT SET PLEAT (PCS)], [HEAT SET PLEAT (Price)], [HEAT TRANSFER (PANEL)]
		, [HEAT TRANSFER (TMS)], [INDIRECT MANPOWER (TMS)], [INSPECTION (TMS)], [INTENSIVE MACHINE (TMS)], [JOKERTAG (TMS)], [KEY BUTTONHOLE (TMS)]
		, [LASER (TMS)], [LASER CUTTER (TMS)], [LASER-AXLETREE (TMS)], [LASER-GALVANOMETER (TMS)], [PACKING (TMS)], [PAD PRINTING (PCS)]
		, [PAD PRINTING (Price)], [POCKET WELT], [PRESSING (TMS)], [PRINTING (PCS)], [PRINTING (Price)], [PRINTING PPU (PPU)], [POSubCon]
		, [SubCon], [QUILTING (Price)], [QUILTING(AT) (TMS)], [QUILTING(HAND) (TMS)], [REAL FLATSEAM (TMS)], [RECOAT Garment], [REPAIR GARMENT (Price)]
		, [ROLLER SUBLIMATION (TMS)], [SEAM TAPING MACHINE (TMS)], [SEAMSEAL (TMS)], [SEWING (TMS)], [S-HOT PRESS(BONDING) (TMS)]
		, [S-HOT PRESS(HT) (TMS)], [S-HOT PRESS(SEWING) (TMS)], [SMALL HOT PRESS (TMS)], [SMALL HOT PRESS-LONG (TMS)], [SUBLIMATION PRINT (TMS)]
		, [SUBLIMATION ROLLER (TMS)], [SUBLIMATION SPRAY (TMS)], [ULTRASONIC (TMS)], [ULTRASONIC MACHINE (TMS)], [ULTRASONIC-ELASTIC (TMS)]
		, [ULTRASONIC-LABEL ASM (TMS)], [ULTRASONIC-TAPE CUT (TMS)], [VELCRO MACHINE (TMS)], [VELCROED M/C (TMS)], [WELDED (TMS)], [WELTED M/C (TMS)]
		, [ZIG ZAG (TMS)], [ZIPPER HOT PRESS (TMS)], [TTL_TMS], [TTL_3FLATLOCK (TMS)], [TTL_4FLATLOCK-H (TMS)], [TTL_4FLATLOCK-S (TMS)]
		, [TTL_AE RING-KNIFE (TMS)], [TTL_AE RING-ULTRASONIC (TMS)], [TTL_AT (TMS)], [TTL_AT (HAND) (TMS)], [TTL_AT (MACHINE) (TMS)], [TTL_AUTO BELT LOOPER (TMS)]
		, [TTL_AUTO POCKET WELT (TMS)], [TTL_AUTO-TEMPLATE (TMS)], [TTL_AUTO-TEMPLATE QT (TMS)], [TTL_B-HOT PRESS(BONDING) (TMS)]
		, [TTL_B-HOT PRESS(SUB-PRT) (TMS)], [TTL_BH-TWICE FOR PLACKET (TMS)], [TTL_BH-TWICE FOR WB (TMS)], [TTL_BIG HOT FOR BONDING (TMS)]
		, [TTL_BIG HOT PRESS (TMS)], [TTL_BLINDSTITCH (TMS)], [TTL_BONDING (HAND) (TMS)], [TTL_BONDING (MACHINE) (TMS)], [TTL_COVERSTITCH CYLINDER (TMS)]
		, [TTL_COVERSTITCH L-CUTTER (TMS)], [TTL_CUTTING (TMS)], [TTL_CUTTING TAPE (Price)], [TTL_D-chain ZIG ZAG (TMS)], [TTL_DIE CUT (TMS)]
		, [TTL_DIE-CUT (TMS)], [TTL_DOWN (TMS)], [TTL_DOWN FILLING (TMS)], [TTL_EM/DEBOSS (I/H) (TMS)], [TTL_EMBOSS/DEBOSS (PCS)], [TTL_EMBOSS/DEBOSS (Price)]
		, [TTL_EMBROIDERY (STITCH)], [TTL_EMBROIDERY (Price)], [TTL_EMBROIDERY EYELET (TMS)], [TTL_EMBROIDERY TMS (TMS)], [TTL_FARM OUT QUILTING (PCS)]
		, [TTL_FARM OUT QUILTING (Price)], [TTL_FEEDOFARM (TMS)], [TTL_FLATLOCK (TMS)], [TTL_Fusible (TMS)], [TTL_Garment Dye (PCS)], [TTL_Garment Dye (Price)]
		, [TTL_GMT WASH (PCS)], [TTL_GMT WASH (Price)], [TTL_HEAT SET PLEAT (PCS)], [TTL_HEAT SET PLEAT (Price)], [TTL_HEAT TRANSFER (PANEL)]
		, [TTL_HEAT TRANSFER (TMS)], [TTL_INDIRECT MANPOWER (TMS)], [TTL_INSPECTION (TMS)], [TTL_INTENSIVE MACHINE (TMS)], [TTL_JOKERTAG (TMS)]
		, [TTL_KEY BUTTONHOLE (TMS)], [TTL_LASER (TMS)], [TTL_LASER CUTTER (TMS)], [TTL_LASER-AXLETREE (TMS)], [TTL_LASER-GALVANOMETER (TMS)], [TTL_PACKING (TMS)]
		, [TTL_PAD PRINTING (PCS)], [TTL_PAD PRINTING (Price)], [TTL_POCKET WELT], [TTL_PRESSING (TMS)], [TTL_PRINTING (PCS)], [TTL_PRINTING (Price)]
		, [TTL_PRINTING PPU (PPU)], [TTL_QUILTING (Price)], [TTL_QUILTING(AT) (TMS)], [TTL_QUILTING(HAND) (TMS)], [TTL_REAL FLATSEAM (TMS)], [TTL_RECOAT Garment]
		, [TTL_REPAIR GARMENT (Price)], [TTL_ROLLER SUBLIMATION (TMS)], [TTL_SEAM TAPING MACHINE (TMS)], [TTL_SEAMSEAL (TMS)], [TTL_SEWING (TMS)]
		, [TTL_S-HOT PRESS(BONDING) (TMS)], [TTL_S-HOT PRESS(HT) (TMS)], [TTL_S-HOT PRESS(SEWING) (TMS)], [TTL_SMALL HOT PRESS (TMS)], [TTL_SMALL HOT PRESS-LONG (TMS)]
		, [TTL_SUBLIMATION PRINT (TMS)], [TTL_SUBLIMATION ROLLER (TMS)], [TTL_SUBLIMATION SPRAY (TMS)], [TTL_ULTRASONIC (TMS)], [TTL_ULTRASONIC MACHINE (TMS)]
		, [TTL_ULTRASONIC-ELASTIC (TMS)], [TTL_ULTRASONIC-LABEL ASM (TMS)], [TTL_ULTRASONIC-TAPE CUT (TMS)], [TTL_VELCRO MACHINE (TMS)], [TTL_VELCROED M/C (TMS)]
		, [TTL_WELDED (TMS)], [TTL_WELTED M/C (TMS)], [TTL_ZIG ZAG (TMS)], [TTL_ZIPPER HOT PRESS (TMS)])
	select [M], [FactoryID], [Delivery], [Delivery(YYYYMM)], [Earliest SCIDlv], [SCIDlv], [KEY], [IDD]
		, [CRD], [CRD(YYYYMM)], [Check CRD], [OrdCFM], [CRD-OrdCFM], [SPNO], [Category], [Est. download date], [Buy Back]
		, [Cancelled], [NeedProduction], [Dest], [Style], [Style Name], [Modular Parent], [CPUAdjusted], [Similar Style]
		, [Season], [Garment L/T], [Order Type], [Project], [PackingMethod], [Hanger pack], [Order#], [Buy Month], [PONO]
		, [VAS/SHAS], [VAS/SHAS Apv.], [VAS/SHAS Cut Off Date], [M/Notice Date], [Est M/Notice Apv.], [Tissue], [AF by adidas]
		, [Factory Disclaimer], [Factory Disclaimer Remark], [Approved/Rejected Date], [Global Foundation Range], [Brand]
		, [Cust CD], [KIT], [Fty Code], [Program ], [Non Revenue], [New CD Code], [ProductType], [FabricType], [Lining]
		, [Gender], [Construction], [Cpu], [Qty], [FOC Qty], [Total CPU], [SewQtyTop], [SewQtyBottom], [SewQtyInner], [SewQtyOuter]
		, [Total Sewing Output], [Cut Qty], [By Comb], [Cutting Status], [Packing Qty], [Packing FOC Qty], [Booking Qty]
		, [FOC Adj Qty], [Not FOC Adj Qty], [KPI L/ETA], [PF ETA (SP)], [Pull Forward Remark], [SCHD L/ETA], [Actual Mtl. ETA]
		, [Fab ETA], [Acc ETA], [Sewing Mtl Complt(SP)], [Packing Mtl Complt(SP)], [Sew. MTL ETA (SP)], [Pkg. MTL ETA (SP)]
		, [MTL Delay], [MTL Cmplt], [MTL Cmplt (SP)], [Arrive W/H Date], [Sewing InLine], [Sewing OffLine], [1st Sewn Date]
		, [Last Sewn Date], [First Production Date], [Last Production Date], [Each Cons Apv Date], [Est Each Con Apv.]
		, [Cutting InLine], [Cutting OffLine], [1st Cut Date], [Last Cut Date], [Est. Pullout], [Act. Pullout Date], [Pullout Qty]
		, [Act. Pullout Times], [Act. Pullout Cmplt], [KPI Delivery Date], [Update Delivery Reason], [Plan Date], [Original Buyer Delivery Date]
		, [SMR], [SMR Name], [Handle], [Handle Name], [Posmr], [Posmr Name], [PoHandle], [PoHandle Name], [PCHandle], [PCHandle Name]
		, [MCHandle], [MCHandle Name], [DoxType], [Packing CTN], [TTLCTN], [Pack Error CTN], [FtyCTN], [cLog CTN], [CFA CTN]
		, [cLog Rec. Date], [Final Insp. Date], [Insp. Result], [CFA Name], [Sewing Line#], [ShipMode], [SI#], [ColorWay], [Special Mark]
		, [Fty Remark], [Sample Reason], [IS MixMarker], [Cutting SP], [Rainwear test], [TMS], [MD room scan date], [Dry Room received date]
		, [Dry room trans date], [Last ctn trans date], [Last ctn recvd date], [OrganicCotton], [Direct Ship], [3FLATLOCK (TMS)]
		, [4FLATLOCK-H (TMS)], [4FLATLOCK-S (TMS)], [AE RING-KNIFE (TMS)], [AE RING-ULTRASONIC (TMS)], [AT (TMS)], [AT (HAND) (TMS)]
		, [AT (MACHINE) (TMS)], [AUTO BELT LOOPER (TMS)], [AUTO POCKET WELT (TMS)], [AUTO-TEMPLATE (TMS)], [AUTO-TEMPLATE QT (TMS)]
		, [B-HOT PRESS(BONDING) (TMS)], [B-HOT PRESS(SUB-PRT) (TMS)], [BH-TWICE FOR PLACKET (TMS)], [BH-TWICE FOR WB (TMS)]
		, [BIG HOT FOR BONDING (TMS)], [BIG HOT PRESS (TMS)], [BLINDSTITCH (TMS)], [BONDING (HAND) (TMS)], [BONDING (MACHINE) (TMS)]
		, [COVERSTITCH CYLINDER (TMS)], [COVERSTITCH L-CUTTER (TMS)], [CUTTING (TMS)], [CUTTING TAPE (Price)], [D-chain ZIG ZAG (TMS)]
		, [DIE CUT (TMS)], [DIE-CUT (TMS)], [DOWN (TMS)], [DOWN FILLING (TMS)], [EM/DEBOSS (I/H) (TMS)], [EMBOSS/DEBOSS (PCS)]
		, [EMBOSS/DEBOSS (Price)], [EMBROIDERY (STITCH)], [EMBROIDERY (Price)], [EMBROIDERY EYELET (TMS)], [EMBROIDERY TMS (TMS)]
		, [FARM OUT QUILTING (PCS)], [FARM OUT QUILTING (Price)], [FEEDOFARM (TMS)], [FLATLOCK (TMS)], [Fusible (TMS)], [Garment Dye (PCS)]
		, [Garment Dye (Price)], [GMT WASH (PCS)], [GMT WASH (Price)], [HEAT SET PLEAT (PCS)], [HEAT SET PLEAT (Price)], [HEAT TRANSFER (PANEL)]
		, [HEAT TRANSFER (TMS)], [INDIRECT MANPOWER (TMS)], [INSPECTION (TMS)], [INTENSIVE MACHINE (TMS)], [JOKERTAG (TMS)], [KEY BUTTONHOLE (TMS)]
		, [LASER (TMS)], [LASER CUTTER (TMS)], [LASER-AXLETREE (TMS)], [LASER-GALVANOMETER (TMS)], [PACKING (TMS)], [PAD PRINTING (PCS)]
		, [PAD PRINTING (Price)], [POCKET WELT], [PRESSING (TMS)], [PRINTING (PCS)], [PRINTING (Price)], [PRINTING PPU (PPU)], [POSubCon]
		, [SubCon], [QUILTING (Price)], [QUILTING(AT) (TMS)], [QUILTING(HAND) (TMS)], [REAL FLATSEAM (TMS)], [RECOAT Garment], [REPAIR GARMENT (Price)]
		, [ROLLER SUBLIMATION (TMS)], [SEAM TAPING MACHINE (TMS)], [SEAMSEAL (TMS)], [SEWING (TMS)], [S-HOT PRESS(BONDING) (TMS)]
		, [S-HOT PRESS(HT) (TMS)], [S-HOT PRESS(SEWING) (TMS)], [SMALL HOT PRESS (TMS)], [SMALL HOT PRESS-LONG (TMS)], [SUBLIMATION PRINT (TMS)]
		, [SUBLIMATION ROLLER (TMS)], [SUBLIMATION SPRAY (TMS)], [ULTRASONIC (TMS)], [ULTRASONIC MACHINE (TMS)], [ULTRASONIC-ELASTIC (TMS)]
		, [ULTRASONIC-LABEL ASM (TMS)], [ULTRASONIC-TAPE CUT (TMS)], [VELCRO MACHINE (TMS)], [VELCROED M/C (TMS)], [WELDED (TMS)], [WELTED M/C (TMS)]
		, [ZIG ZAG (TMS)], [ZIPPER HOT PRESS (TMS)], [TTL_TMS], [TTL_3FLATLOCK (TMS)], [TTL_4FLATLOCK-H (TMS)], [TTL_4FLATLOCK-S (TMS)]
		, [TTL_AE RING-KNIFE (TMS)], [TTL_AE RING-ULTRASONIC (TMS)], [TTL_AT (TMS)], [TTL_AT (HAND) (TMS)], [TTL_AT (MACHINE) (TMS)], [TTL_AUTO BELT LOOPER (TMS)]
		, [TTL_AUTO POCKET WELT (TMS)], [TTL_AUTO-TEMPLATE (TMS)], [TTL_AUTO-TEMPLATE QT (TMS)], [TTL_B-HOT PRESS(BONDING) (TMS)]
		, [TTL_B-HOT PRESS(SUB-PRT) (TMS)], [TTL_BH-TWICE FOR PLACKET (TMS)], [TTL_BH-TWICE FOR WB (TMS)], [TTL_BIG HOT FOR BONDING (TMS)]
		, [TTL_BIG HOT PRESS (TMS)], [TTL_BLINDSTITCH (TMS)], [TTL_BONDING (HAND) (TMS)], [TTL_BONDING (MACHINE) (TMS)], [TTL_COVERSTITCH CYLINDER (TMS)]
		, [TTL_COVERSTITCH L-CUTTER (TMS)], [TTL_CUTTING (TMS)], [TTL_CUTTING TAPE (Price)], [TTL_D-chain ZIG ZAG (TMS)], [TTL_DIE CUT (TMS)]
		, [TTL_DIE-CUT (TMS)], [TTL_DOWN (TMS)], [TTL_DOWN FILLING (TMS)], [TTL_EM/DEBOSS (I/H) (TMS)], [TTL_EMBOSS/DEBOSS (PCS)], [TTL_EMBOSS/DEBOSS (Price)]
		, [TTL_EMBROIDERY (STITCH)], [TTL_EMBROIDERY (Price)], [TTL_EMBROIDERY EYELET (TMS)], [TTL_EMBROIDERY TMS (TMS)], [TTL_FARM OUT QUILTING (PCS)]
		, [TTL_FARM OUT QUILTING (Price)], [TTL_FEEDOFARM (TMS)], [TTL_FLATLOCK (TMS)], [TTL_Fusible (TMS)], [TTL_Garment Dye (PCS)], [TTL_Garment Dye (Price)]
		, [TTL_GMT WASH (PCS)], [TTL_GMT WASH (Price)], [TTL_HEAT SET PLEAT (PCS)], [TTL_HEAT SET PLEAT (Price)], [TTL_HEAT TRANSFER (PANEL)]
		, [TTL_HEAT TRANSFER (TMS)], [TTL_INDIRECT MANPOWER (TMS)], [TTL_INSPECTION (TMS)], [TTL_INTENSIVE MACHINE (TMS)], [TTL_JOKERTAG (TMS)]
		, [TTL_KEY BUTTONHOLE (TMS)], [TTL_LASER (TMS)], [TTL_LASER CUTTER (TMS)], [TTL_LASER-AXLETREE (TMS)], [TTL_LASER-GALVANOMETER (TMS)], [TTL_PACKING (TMS)]
		, [TTL_PAD PRINTING (PCS)], [TTL_PAD PRINTING (Price)], [TTL_POCKET WELT], [TTL_PRESSING (TMS)], [TTL_PRINTING (PCS)], [TTL_PRINTING (Price)]
		, [TTL_PRINTING PPU (PPU)], [TTL_QUILTING (Price)], [TTL_QUILTING(AT) (TMS)], [TTL_QUILTING(HAND) (TMS)], [TTL_REAL FLATSEAM (TMS)], [TTL_RECOAT Garment]
		, [TTL_REPAIR GARMENT (Price)], [TTL_ROLLER SUBLIMATION (TMS)], [TTL_SEAM TAPING MACHINE (TMS)], [TTL_SEAMSEAL (TMS)], [TTL_SEWING (TMS)]
		, [TTL_S-HOT PRESS(BONDING) (TMS)], [TTL_S-HOT PRESS(HT) (TMS)], [TTL_S-HOT PRESS(SEWING) (TMS)], [TTL_SMALL HOT PRESS (TMS)], [TTL_SMALL HOT PRESS-LONG (TMS)]
		, [TTL_SUBLIMATION PRINT (TMS)], [TTL_SUBLIMATION ROLLER (TMS)], [TTL_SUBLIMATION SPRAY (TMS)], [TTL_ULTRASONIC (TMS)], [TTL_ULTRASONIC MACHINE (TMS)]
		, [TTL_ULTRASONIC-ELASTIC (TMS)], [TTL_ULTRASONIC-LABEL ASM (TMS)], [TTL_ULTRASONIC-TAPE CUT (TMS)], [TTL_VELCRO MACHINE (TMS)], [TTL_VELCROED M/C (TMS)]
		, [TTL_WELDED (TMS)], [TTL_WELTED M/C (TMS)], [TTL_ZIG ZAG (TMS)], [TTL_ZIPPER HOT PRESS (TMS)]
	from #tmp_P_PPICMASTERLIST t
	where not exists (select 1 from P_PPICMASTERLIST p where t.SPNO = p.SPNO)

	update p
		set p.[M] = t.[M] 
			, p.[FactoryID] = t.[FactoryID] 
			, p.[Delivery] = t.[Delivery] 
			, p.[Delivery(YYYYMM)] = t.[Delivery(YYYYMM)] 
			, p.[Earliest SCIDlv] = t.[Earliest SCIDlv] 
			, p.[SCIDlv] = t.[SCIDlv] 
			, p.[KEY] = t.[KEY] 
			, p.[IDD] = t.[IDD] 
			, p.[CRD] = t.[CRD] 
			, p.[CRD(YYYYMM)] = t.[CRD(YYYYMM)] 
			, p.[Check CRD] = t.[Check CRD] 
			, p.[OrdCFM] = t.[OrdCFM] 
			, p.[CRD-OrdCFM] = t.[CRD-OrdCFM] 
			, p.[SPNO] = t.[SPNO] 
			, p.[Category] = t.[Category] 
			, p.[Est. download date] = t.[Est. download date] 
			, p.[Buy Back] = t.[Buy Back] 
			, p.[Cancelled] = t.[Cancelled] 
			, p.[NeedProduction] = t.[NeedProduction] 
			, p.[Dest] = t.[Dest] 
			, p.[Style] = t.[Style] 
			, p.[Style Name] = t.[Style Name] 
			, p.[Modular Parent] = t.[Modular Parent] 
			, p.[CPUAdjusted] = t.[CPUAdjusted] 
			, p.[Similar Style] = t.[Similar Style] 
			, p.[Season] = t.[Season] 
			, p.[Garment L/T] = t.[Garment L/T] 
			, p.[Order Type] = t.[Order Type] 
			, p.[Project] = t.[Project] 
			, p.[PackingMethod] = t.[PackingMethod] 
			, p.[Hanger pack] = t.[Hanger pack] 
			, p.[Order#] = t.[Order#] 
			, p.[Buy Month] = t.[Buy Month] 
			, p.[PONO] = t.[PONO] 
			, p.[VAS/SHAS] = t.[VAS/SHAS] 
			, p.[VAS/SHAS Apv.] = t.[VAS/SHAS Apv.] 
			, p.[VAS/SHAS Cut Off Date] = t.[VAS/SHAS Cut Off Date] 
			, p.[M/Notice Date] = t.[M/Notice Date] 
			, p.[Est M/Notice Apv.] = t.[Est M/Notice Apv.] 
			, p.[Tissue] = t.[Tissue] 
			, p.[AF by adidas] = t.[AF by adidas] 
			, p.[Factory Disclaimer] = t.[Factory Disclaimer] 
			, p.[Factory Disclaimer Remark] = t.[Factory Disclaimer Remark] 
			, p.[Approved/Rejected Date] = t.[Approved/Rejected Date] 
			, p.[Global Foundation Range] = t.[Global Foundation Range] 
			, p.[Brand] = t.[Brand] 
			, p.[Cust CD] = t.[Cust CD] 
			, p.[KIT] = t.[KIT] 
			, p.[Fty Code] = t.[Fty Code] 
			, p.[Program ] = t.[Program ] 
			, p.[Non Revenue] = t.[Non Revenue] 
			, p.[New CD Code] = t.[New CD Code] 
			, p.[ProductType] = t.[ProductType] 
			, p.[FabricType] = t.[FabricType] 
			, p.[Lining] = t.[Lining] 
			, p.[Gender] = t.[Gender] 
			, p.[Construction] = t.[Construction] 
			, p.[Cpu] = t.[Cpu] 
			, p.[Qty] = t.[Qty] 
			, p.[FOC Qty] = t.[FOC Qty] 
			, p.[Total CPU] = t.[Total CPU] 
			, p.[SewQtyTop] = t.[SewQtyTop] 
			, p.[SewQtyBottom] = t.[SewQtyBottom] 
			, p.[SewQtyInner] = t.[SewQtyInner] 
			, p.[SewQtyOuter] = t.[SewQtyOuter] 
			, p.[Total Sewing Output] = t.[Total Sewing Output] 
			, p.[Cut Qty] = t.[Cut Qty] 
			, p.[By Comb] = t.[By Comb] 
			, p.[Cutting Status] = t.[Cutting Status] 
			, p.[Packing Qty] = t.[Packing Qty] 
			, p.[Packing FOC Qty] = t.[Packing FOC Qty] 
			, p.[Booking Qty] = t.[Booking Qty] 
			, p.[FOC Adj Qty] = t.[FOC Adj Qty] 
			, p.[Not FOC Adj Qty] = t.[Not FOC Adj Qty] 
			, p.[KPI L/ETA] = t.[KPI L/ETA] 
			, p.[PF ETA (SP)] = t.[PF ETA (SP)] 
			, p.[Pull Forward Remark] = t.[Pull Forward Remark] 
			, p.[SCHD L/ETA] = t.[SCHD L/ETA] 
			, p.[Actual Mtl. ETA] = t.[Actual Mtl. ETA] 
			, p.[Fab ETA] = t.[Fab ETA] 
			, p.[Acc ETA] = t.[Acc ETA] 
			, p.[Sewing Mtl Complt(SP)] = t.[Sewing Mtl Complt(SP)] 
			, p.[Packing Mtl Complt(SP)] = t.[Packing Mtl Complt(SP)] 
			, p.[Sew. MTL ETA (SP)] = t.[Sew. MTL ETA (SP)] 
			, p.[Pkg. MTL ETA (SP)] = t.[Pkg. MTL ETA (SP)] 
			, p.[MTL Delay] = t.[MTL Delay] 
			, p.[MTL Cmplt] = t.[MTL Cmplt] 
			, p.[MTL Cmplt (SP)] = t.[MTL Cmplt (SP)] 
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
			, p.[1st Cut Date] = t.[1st Cut Date] 
			, p.[Last Cut Date] = t.[Last Cut Date] 
			, p.[Est. Pullout] = t.[Est. Pullout] 
			, p.[Act. Pullout Date] = t.[Act. Pullout Date] 
			, p.[Pullout Qty] = t.[Pullout Qty] 
			, p.[Act. Pullout Times] = t.[Act. Pullout Times] 
			, p.[Act. Pullout Cmplt] = t.[Act. Pullout Cmplt] 
			, p.[KPI Delivery Date] = t.[KPI Delivery Date] 
			, p.[Update Delivery Reason] = t.[Update Delivery Reason] 
			, p.[Plan Date] = t.[Plan Date] 
			, p.[Original Buyer Delivery Date] = t.[Original Buyer Delivery Date] 
			, p.[SMR] = t.[SMR] 
			, p.[SMR Name] = t.[SMR Name] 
			, p.[Handle] = t.[Handle] 
			, p.[Handle Name] = t.[Handle Name] 
			, p.[Posmr] = t.[Posmr] 
			, p.[Posmr Name] = t.[Posmr Name] 
			, p.[PoHandle] = t.[PoHandle] 
			, p.[PoHandle Name] = t.[PoHandle Name] 
			, p.[PCHandle] = t.[PCHandle] 
			, p.[PCHandle Name] = t.[PCHandle Name] 
			, p.[MCHandle] = t.[MCHandle] 
			, p.[MCHandle Name] = t.[MCHandle Name] 
			, p.[DoxType] = t.[DoxType] 
			, p.[Packing CTN] = t.[Packing CTN] 
			, p.[TTLCTN] = t.[TTLCTN] 
			, p.[Pack Error CTN] = t.[Pack Error CTN] 
			, p.[FtyCTN] = t.[FtyCTN] 
			, p.[cLog CTN] = t.[cLog CTN] 
			, p.[CFA CTN] = t.[CFA CTN] 
			, p.[cLog Rec. Date] = t.[cLog Rec. Date] 
			, p.[Final Insp. Date] = t.[Final Insp. Date] 
			, p.[Insp. Result] = t.[Insp. Result] 
			, p.[CFA Name] = t.[CFA Name] 
			, p.[Sewing Line#] = t.[Sewing Line#] 
			, p.[ShipMode] = t.[ShipMode] 
			, p.[SI#] = t.[SI#] 
			, p.[ColorWay] = t.[ColorWay] 
			, p.[Special Mark] = t.[Special Mark] 
			, p.[Fty Remark] = t.[Fty Remark] 
			, p.[Sample Reason] = t.[Sample Reason] 
			, p.[IS MixMarker] = t.[IS MixMarker] 
			, p.[Cutting SP] = t.[Cutting SP] 
			, p.[Rainwear test] = t.[Rainwear test] 
			, p.[TMS] = t.[TMS] 
			, p.[MD room scan date] = t.[MD room scan date] 
			, p.[Dry Room received date] = t.[Dry Room received date] 
			, p.[Dry room trans date] = t.[Dry room trans date] 
			, p.[Last ctn trans date] = t.[Last ctn trans date] 
			, p.[Last ctn recvd date] = t.[Last ctn recvd date] 
			, p.[OrganicCotton] = t.[OrganicCotton] 
			, p.[Direct Ship] = t.[Direct Ship] 
			, p.[3FLATLOCK (TMS)] = t.[3FLATLOCK (TMS)] 
			, p.[4FLATLOCK-H (TMS)] = t.[4FLATLOCK-H (TMS)] 
			, p.[4FLATLOCK-S (TMS)] = t.[4FLATLOCK-S (TMS)] 
			, p.[AE RING-KNIFE (TMS)] = t.[AE RING-KNIFE (TMS)] 
			, p.[AE RING-ULTRASONIC (TMS)] = t.[AE RING-ULTRASONIC (TMS)] 
			, p.[AT (TMS)] = t.[AT (TMS)] 
			, p.[AT (HAND) (TMS)] = t.[AT (HAND) (TMS)] 
			, p.[AT (MACHINE) (TMS)] = t.[AT (MACHINE) (TMS)] 
			, p.[AUTO BELT LOOPER (TMS)] = t.[AUTO BELT LOOPER (TMS)] 
			, p.[AUTO POCKET WELT (TMS)] = t.[AUTO POCKET WELT (TMS)] 
			, p.[AUTO-TEMPLATE (TMS)] = t.[AUTO-TEMPLATE (TMS)] 
			, p.[AUTO-TEMPLATE QT (TMS)] = t.[AUTO-TEMPLATE QT (TMS)] 
			, p.[B-HOT PRESS(BONDING) (TMS)] = t.[B-HOT PRESS(BONDING) (TMS)] 
			, p.[B-HOT PRESS(SUB-PRT) (TMS)] = t.[B-HOT PRESS(SUB-PRT) (TMS)] 
			, p.[BH-TWICE FOR PLACKET (TMS)] = t.[BH-TWICE FOR PLACKET (TMS)] 
			, p.[BH-TWICE FOR WB (TMS)] = t.[BH-TWICE FOR WB (TMS)] 
			, p.[BIG HOT FOR BONDING (TMS)] = t.[BIG HOT FOR BONDING (TMS)] 
			, p.[BIG HOT PRESS (TMS)] = t.[BIG HOT PRESS (TMS)] 
			, p.[BLINDSTITCH (TMS)] = t.[BLINDSTITCH (TMS)] 
			, p.[BONDING (HAND) (TMS)] = t.[BONDING (HAND) (TMS)] 
			, p.[BONDING (MACHINE) (TMS)] = t.[BONDING (MACHINE) (TMS)] 
			, p.[COVERSTITCH CYLINDER (TMS)] = t.[COVERSTITCH CYLINDER (TMS)] 
			, p.[COVERSTITCH L-CUTTER (TMS)] = t.[COVERSTITCH L-CUTTER (TMS)] 
			, p.[CUTTING (TMS)] = t.[CUTTING (TMS)] 
			, p.[CUTTING TAPE (Price)] = t.[CUTTING TAPE (Price)] 
			, p.[D-chain ZIG ZAG (TMS)] = t.[D-chain ZIG ZAG (TMS)] 
			, p.[DIE CUT (TMS)] = t.[DIE CUT (TMS)] 
			, p.[DIE-CUT (TMS)] = t.[DIE-CUT (TMS)] 
			, p.[DOWN (TMS)] = t.[DOWN (TMS)] 
			, p.[DOWN FILLING (TMS)] = t.[DOWN FILLING (TMS)] 
			, p.[EM/DEBOSS (I/H) (TMS)] = t.[EM/DEBOSS (I/H) (TMS)] 
			, p.[EMBOSS/DEBOSS (PCS)] = t.[EMBOSS/DEBOSS (PCS)] 
			, p.[EMBOSS/DEBOSS (Price)] = t.[EMBOSS/DEBOSS (Price)] 
			, p.[EMBROIDERY (STITCH)] = t.[EMBROIDERY (STITCH)] 
			, p.[EMBROIDERY (Price)] = t.[EMBROIDERY (Price)] 
			, p.[EMBROIDERY EYELET (TMS)] = t.[EMBROIDERY EYELET (TMS)] 
			, p.[EMBROIDERY TMS (TMS)] = t.[EMBROIDERY TMS (TMS)] 
			, p.[FARM OUT QUILTING (PCS)] = t.[FARM OUT QUILTING (PCS)] 
			, p.[FARM OUT QUILTING (Price)] = t.[FARM OUT QUILTING (Price)] 
			, p.[FEEDOFARM (TMS)] = t.[FEEDOFARM (TMS)] 
			, p.[FLATLOCK (TMS)] = t.[FLATLOCK (TMS)] 
			, p.[Fusible (TMS)] = t.[Fusible (TMS)] 
			, p.[Garment Dye (PCS)] = t.[Garment Dye (PCS)] 
			, p.[Garment Dye (Price)] = t.[Garment Dye (Price)] 
			, p.[GMT WASH (PCS)] = t.[GMT WASH (PCS)] 
			, p.[GMT WASH (Price)] = t.[GMT WASH (Price)] 
			, p.[HEAT SET PLEAT (PCS)] = t.[HEAT SET PLEAT (PCS)] 
			, p.[HEAT SET PLEAT (Price)] = t.[HEAT SET PLEAT (Price)] 
			, p.[HEAT TRANSFER (PANEL)] = t.[HEAT TRANSFER (PANEL)] 
			, p.[HEAT TRANSFER (TMS)] = t.[HEAT TRANSFER (TMS)] 
			, p.[INDIRECT MANPOWER (TMS)] = t.[INDIRECT MANPOWER (TMS)] 
			, p.[INSPECTION (TMS)] = t.[INSPECTION (TMS)] 
			, p.[INTENSIVE MACHINE (TMS)] = t.[INTENSIVE MACHINE (TMS)] 
			, p.[JOKERTAG (TMS)] = t.[JOKERTAG (TMS)] 
			, p.[KEY BUTTONHOLE (TMS)] = t.[KEY BUTTONHOLE (TMS)] 
			, p.[LASER (TMS)] = t.[LASER (TMS)] 
			, p.[LASER CUTTER (TMS)] = t.[LASER CUTTER (TMS)] 
			, p.[LASER-AXLETREE (TMS)] = t.[LASER-AXLETREE (TMS)] 
			, p.[LASER-GALVANOMETER (TMS)] = t.[LASER-GALVANOMETER (TMS)] 
			, p.[PACKING (TMS)] = t.[PACKING (TMS)] 
			, p.[PAD PRINTING (PCS)] = t.[PAD PRINTING (PCS)] 
			, p.[PAD PRINTING (Price)] = t.[PAD PRINTING (Price)] 
			, p.[POCKET WELT] = t.[POCKET WELT] 
			, p.[PRESSING (TMS)] = t.[PRESSING (TMS)] 
			, p.[PRINTING (PCS)] = t.[PRINTING (PCS)] 
			, p.[PRINTING (Price)] = t.[PRINTING (Price)] 
			, p.[PRINTING PPU (PPU)] = t.[PRINTING PPU (PPU)] 
			, p.[POSubCon] = t.[POSubCon] 
			, p.[SubCon] = t.[SubCon] 
			, p.[QUILTING (Price)] = t.[QUILTING (Price)] 
			, p.[QUILTING(AT) (TMS)] = t.[QUILTING(AT) (TMS)] 
			, p.[QUILTING(HAND) (TMS)] = t.[QUILTING(HAND) (TMS)] 
			, p.[REAL FLATSEAM (TMS)] = t.[REAL FLATSEAM (TMS)] 
			, p.[RECOAT Garment] = t.[RECOAT Garment] 
			, p.[REPAIR GARMENT (Price)] = t.[REPAIR GARMENT (Price)] 
			, p.[ROLLER SUBLIMATION (TMS)] = t.[ROLLER SUBLIMATION (TMS)] 
			, p.[SEAM TAPING MACHINE (TMS)] = t.[SEAM TAPING MACHINE (TMS)] 
			, p.[SEAMSEAL (TMS)] = t.[SEAMSEAL (TMS)] 
			, p.[SEWING (TMS)] = t.[SEWING (TMS)] 
			, p.[S-HOT PRESS(BONDING) (TMS)] = t.[S-HOT PRESS(BONDING) (TMS)] 
			, p.[S-HOT PRESS(HT) (TMS)] = t.[S-HOT PRESS(HT) (TMS)] 
			, p.[S-HOT PRESS(SEWING) (TMS)] = t.[S-HOT PRESS(SEWING) (TMS)] 
			, p.[SMALL HOT PRESS (TMS)] = t.[SMALL HOT PRESS (TMS)] 
			, p.[SMALL HOT PRESS-LONG (TMS)] = t.[SMALL HOT PRESS-LONG (TMS)] 
			, p.[SUBLIMATION PRINT (TMS)] = t.[SUBLIMATION PRINT (TMS)] 
			, p.[SUBLIMATION ROLLER (TMS)] = t.[SUBLIMATION ROLLER (TMS)] 
			, p.[SUBLIMATION SPRAY (TMS)] = t.[SUBLIMATION SPRAY (TMS)] 
			, p.[ULTRASONIC (TMS)] = t.[ULTRASONIC (TMS)] 
			, p.[ULTRASONIC MACHINE (TMS)] = t.[ULTRASONIC MACHINE (TMS)] 
			, p.[ULTRASONIC-ELASTIC (TMS)] = t.[ULTRASONIC-ELASTIC (TMS)] 
			, p.[ULTRASONIC-LABEL ASM (TMS)] = t.[ULTRASONIC-LABEL ASM (TMS)] 
			, p.[ULTRASONIC-TAPE CUT (TMS)] = t.[ULTRASONIC-TAPE CUT (TMS)] 
			, p.[VELCRO MACHINE (TMS)] = t.[VELCRO MACHINE (TMS)] 
			, p.[VELCROED M/C (TMS)] = t.[VELCROED M/C (TMS)] 
			, p.[WELDED (TMS)] = t.[WELDED (TMS)] 
			, p.[WELTED M/C (TMS)] = t.[WELTED M/C (TMS)] 
			, p.[ZIG ZAG (TMS)] = t.[ZIG ZAG (TMS)] 
			, p.[ZIPPER HOT PRESS (TMS)] = t.[ZIPPER HOT PRESS (TMS)] 
			, p.[TTL_TMS] = t.[TTL_TMS] 
			, p.[TTL_3FLATLOCK (TMS)] = t.[TTL_3FLATLOCK (TMS)] 
			, p.[TTL_4FLATLOCK-H (TMS)] = t.[TTL_4FLATLOCK-H (TMS)] 
			, p.[TTL_4FLATLOCK-S (TMS)] = t.[TTL_4FLATLOCK-S (TMS)] 
			, p.[TTL_AE RING-KNIFE (TMS)] = t.[TTL_AE RING-KNIFE (TMS)] 
			, p.[TTL_AE RING-ULTRASONIC (TMS)] = t.[TTL_AE RING-ULTRASONIC (TMS)] 
			, p.[TTL_AT (TMS)] = t.[TTL_AT (TMS)] 
			, p.[TTL_AT (HAND) (TMS)] = t.[TTL_AT (HAND) (TMS)] 
			, p.[TTL_AT (MACHINE) (TMS)] = t.[TTL_AT (MACHINE) (TMS)] 
			, p.[TTL_AUTO BELT LOOPER (TMS)] = t.[TTL_AUTO BELT LOOPER (TMS)] 
			, p.[TTL_AUTO POCKET WELT (TMS)] = t.[TTL_AUTO POCKET WELT (TMS)] 
			, p.[TTL_AUTO-TEMPLATE (TMS)] = t.[TTL_AUTO-TEMPLATE (TMS)] 
			, p.[TTL_AUTO-TEMPLATE QT (TMS)] = t.[TTL_AUTO-TEMPLATE QT (TMS)] 
			, p.[TTL_B-HOT PRESS(BONDING) (TMS)] = t.[TTL_B-HOT PRESS(BONDING) (TMS)] 
			, p.[TTL_B-HOT PRESS(SUB-PRT) (TMS)] = t.[TTL_B-HOT PRESS(SUB-PRT) (TMS)] 
			, p.[TTL_BH-TWICE FOR PLACKET (TMS)] = t.[TTL_BH-TWICE FOR PLACKET (TMS)] 
			, p.[TTL_BH-TWICE FOR WB (TMS)] = t.[TTL_BH-TWICE FOR WB (TMS)] 
			, p.[TTL_BIG HOT FOR BONDING (TMS)] = t.[TTL_BIG HOT FOR BONDING (TMS)] 
			, p.[TTL_BIG HOT PRESS (TMS)] = t.[TTL_BIG HOT PRESS (TMS)] 
			, p.[TTL_BLINDSTITCH (TMS)] = t.[TTL_BLINDSTITCH (TMS)] 
			, p.[TTL_BONDING (HAND) (TMS)] = t.[TTL_BONDING (HAND) (TMS)] 
			, p.[TTL_BONDING (MACHINE) (TMS)] = t.[TTL_BONDING (MACHINE) (TMS)] 
			, p.[TTL_COVERSTITCH CYLINDER (TMS)] = t.[TTL_COVERSTITCH CYLINDER (TMS)] 
			, p.[TTL_COVERSTITCH L-CUTTER (TMS)] = t.[TTL_COVERSTITCH L-CUTTER (TMS)] 
			, p.[TTL_CUTTING (TMS)] = t.[TTL_CUTTING (TMS)] 
			, p.[TTL_CUTTING TAPE (Price)] = t.[TTL_CUTTING TAPE (Price)] 
			, p.[TTL_D-chain ZIG ZAG (TMS)] = t.[TTL_D-chain ZIG ZAG (TMS)] 
			, p.[TTL_DIE CUT (TMS)] = t.[TTL_DIE CUT (TMS)] 
			, p.[TTL_DIE-CUT (TMS)] = t.[TTL_DIE-CUT (TMS)] 
			, p.[TTL_DOWN (TMS)] = t.[TTL_DOWN (TMS)] 
			, p.[TTL_DOWN FILLING (TMS)] = t.[TTL_DOWN FILLING (TMS)] 
			, p.[TTL_EM/DEBOSS (I/H) (TMS)] = t.[TTL_EM/DEBOSS (I/H) (TMS)] 
			, p.[TTL_EMBOSS/DEBOSS (PCS)] = t.[TTL_EMBOSS/DEBOSS (PCS)] 
			, p.[TTL_EMBOSS/DEBOSS (Price)] = t.[TTL_EMBOSS/DEBOSS (Price)] 
			, p.[TTL_EMBROIDERY (STITCH)] = t.[TTL_EMBROIDERY (STITCH)] 
			, p.[TTL_EMBROIDERY (Price)] = t.[TTL_EMBROIDERY (Price)] 
			, p.[TTL_EMBROIDERY EYELET (TMS)] = t.[TTL_EMBROIDERY EYELET (TMS)] 
			, p.[TTL_EMBROIDERY TMS (TMS)] = t.[TTL_EMBROIDERY TMS (TMS)] 
			, p.[TTL_FARM OUT QUILTING (PCS)] = t.[TTL_FARM OUT QUILTING (PCS)] 
			, p.[TTL_FARM OUT QUILTING (Price)] = t.[TTL_FARM OUT QUILTING (Price)] 
			, p.[TTL_FEEDOFARM (TMS)] = t.[TTL_FEEDOFARM (TMS)] 
			, p.[TTL_FLATLOCK (TMS)] = t.[TTL_FLATLOCK (TMS)] 
			, p.[TTL_Fusible (TMS)] = t.[TTL_Fusible (TMS)] 
			, p.[TTL_Garment Dye (PCS)] = t.[TTL_Garment Dye (PCS)] 
			, p.[TTL_Garment Dye (Price)] = t.[TTL_Garment Dye (Price)] 
			, p.[TTL_GMT WASH (PCS)] = t.[TTL_GMT WASH (PCS)] 
			, p.[TTL_GMT WASH (Price)] = t.[TTL_GMT WASH (Price)] 
			, p.[TTL_HEAT SET PLEAT (PCS)] = t.[TTL_HEAT SET PLEAT (PCS)] 
			, p.[TTL_HEAT SET PLEAT (Price)] = t.[TTL_HEAT SET PLEAT (Price)] 
			, p.[TTL_HEAT TRANSFER (PANEL)] = t.[TTL_HEAT TRANSFER (PANEL)] 
			, p.[TTL_HEAT TRANSFER (TMS)] = t.[TTL_HEAT TRANSFER (TMS)] 
			, p.[TTL_INDIRECT MANPOWER (TMS)] = t.[TTL_INDIRECT MANPOWER (TMS)] 
			, p.[TTL_INSPECTION (TMS)] = t.[TTL_INSPECTION (TMS)] 
			, p.[TTL_INTENSIVE MACHINE (TMS)] = t.[TTL_INTENSIVE MACHINE (TMS)] 
			, p.[TTL_JOKERTAG (TMS)] = t.[TTL_JOKERTAG (TMS)] 
			, p.[TTL_KEY BUTTONHOLE (TMS)] = t.[TTL_KEY BUTTONHOLE (TMS)] 
			, p.[TTL_LASER (TMS)] = t.[TTL_LASER (TMS)] 
			, p.[TTL_LASER CUTTER (TMS)] = t.[TTL_LASER CUTTER (TMS)] 
			, p.[TTL_LASER-AXLETREE (TMS)] = t.[TTL_LASER-AXLETREE (TMS)] 
			, p.[TTL_LASER-GALVANOMETER (TMS)] = t.[TTL_LASER-GALVANOMETER (TMS)] 
			, p.[TTL_PACKING (TMS)] = t.[TTL_PACKING (TMS)] 
			, p.[TTL_PAD PRINTING (PCS)] = t.[TTL_PAD PRINTING (PCS)] 
			, p.[TTL_PAD PRINTING (Price)] = t.[TTL_PAD PRINTING (Price)] 
			, p.[TTL_POCKET WELT] = t.[TTL_POCKET WELT] 
			, p.[TTL_PRESSING (TMS)] = t.[TTL_PRESSING (TMS)] 
			, p.[TTL_PRINTING (PCS)] = t.[TTL_PRINTING (PCS)] 
			, p.[TTL_PRINTING (Price)] = t.[TTL_PRINTING (Price)] 
			, p.[TTL_PRINTING PPU (PPU)] = t.[TTL_PRINTING PPU (PPU)] 
			, p.[TTL_QUILTING (Price)] = t.[TTL_QUILTING (Price)] 
			, p.[TTL_QUILTING(AT) (TMS)] = t.[TTL_QUILTING(AT) (TMS)] 
			, p.[TTL_QUILTING(HAND) (TMS)] = t.[TTL_QUILTING(HAND) (TMS)] 
			, p.[TTL_REAL FLATSEAM (TMS)] = t.[TTL_REAL FLATSEAM (TMS)] 
			, p.[TTL_RECOAT Garment] = t.[TTL_RECOAT Garment] 
			, p.[TTL_REPAIR GARMENT (Price)] = t.[TTL_REPAIR GARMENT (Price)] 
			, p.[TTL_ROLLER SUBLIMATION (TMS)] = t.[TTL_ROLLER SUBLIMATION (TMS)] 
			, p.[TTL_SEAM TAPING MACHINE (TMS)] = t.[TTL_SEAM TAPING MACHINE (TMS)] 
			, p.[TTL_SEAMSEAL (TMS)] = t.[TTL_SEAMSEAL (TMS)] 
			, p.[TTL_SEWING (TMS)] = t.[TTL_SEWING (TMS)] 
			, p.[TTL_S-HOT PRESS(BONDING) (TMS)] = t.[TTL_S-HOT PRESS(BONDING) (TMS)] 
			, p.[TTL_S-HOT PRESS(HT) (TMS)] = t.[TTL_S-HOT PRESS(HT) (TMS)] 
			, p.[TTL_S-HOT PRESS(SEWING) (TMS)] = t.[TTL_S-HOT PRESS(SEWING) (TMS)] 
			, p.[TTL_SMALL HOT PRESS (TMS)] = t.[TTL_SMALL HOT PRESS (TMS)] 
			, p.[TTL_SMALL HOT PRESS-LONG (TMS)] = t.[TTL_SMALL HOT PRESS-LONG (TMS)] 
			, p.[TTL_SUBLIMATION PRINT (TMS)] = t.[TTL_SUBLIMATION PRINT (TMS)] 
			, p.[TTL_SUBLIMATION ROLLER (TMS)] = t.[TTL_SUBLIMATION ROLLER (TMS)] 
			, p.[TTL_SUBLIMATION SPRAY (TMS)] = t.[TTL_SUBLIMATION SPRAY (TMS)] 
			, p.[TTL_ULTRASONIC (TMS)] = t.[TTL_ULTRASONIC (TMS)] 
			, p.[TTL_ULTRASONIC MACHINE (TMS)] = t.[TTL_ULTRASONIC MACHINE (TMS)] 
			, p.[TTL_ULTRASONIC-ELASTIC (TMS)] = t.[TTL_ULTRASONIC-ELASTIC (TMS)] 
			, p.[TTL_ULTRASONIC-LABEL ASM (TMS)] = t.[TTL_ULTRASONIC-LABEL ASM (TMS)] 
			, p.[TTL_ULTRASONIC-TAPE CUT (TMS)] = t.[TTL_ULTRASONIC-TAPE CUT (TMS)] 
			, p.[TTL_VELCRO MACHINE (TMS)] = t.[TTL_VELCRO MACHINE (TMS)] 
			, p.[TTL_VELCROED M/C (TMS)] = t.[TTL_VELCROED M/C (TMS)] 
			, p.[TTL_WELDED (TMS)] = t.[TTL_WELDED (TMS)] 
			, p.[TTL_WELTED M/C (TMS)] = t.[TTL_WELTED M/C (TMS)] 
			, p.[TTL_ZIG ZAG (TMS)] = t.[TTL_ZIG ZAG (TMS)] 
			, p.[TTL_ZIPPER HOT PRESS (TMS)] = t.[TTL_ZIPPER HOT PRESS (TMS)] 
	from P_PPICMASTERLIST p 
	inner join #tmp_P_PPICMASTERLIST t on p.SPNO = t.SPNO


	drop table #tmp_P_PPICMASTERLIST

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
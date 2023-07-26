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
			[Factory Disclaimer Remark] NVARCHAR(500) NULL ,
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
			[StyleCarryover] VARCHAR(1) NULL,
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
		, [Last ctn recvd date], [OrganicCotton], [Direct Ship], [StyleCarryover], [3FLATLOCK (TMS)], [4FLATLOCK-H (TMS)], [4FLATLOCK-S (TMS)]
		, [AE RING-KNIFE (TMS)], [AE RING-ULTRASONIC (TMS)], [AT (TMS)], [AT (HAND) (TMS)], [AT (MACHINE) (TMS)], [AUTO BELT LOOPER (TMS)]
		, [AUTO POCKET WELT (TMS)], [AUTO-TEMPLATE (TMS)], [AUTO-TEMPLATE QT (TMS)], [B-HOT PRESS(BONDING) (TMS)], [B-HOT PRESS(SUB-PRT) (TMS)]
		, [BH-TWICE FOR PLACKET (TMS)], [BH-TWICE FOR WB (TMS)], [BIG HOT FOR BONDING (TMS)], [BIG HOT PRESS (TMS)], [BLINDSTITCH (TMS)]
		, [BONDING (HAND) (TMS)], [BONDING (MACHINE) (TMS)], [COVERSTITCH CYLINDER (TMS)], [COVERSTITCH L-CUTTER (TMS)], [CUTTING (TMS)]
		, [CUTTING TAPE (Price)], [D-chain ZIG ZAG (TMS)], [DIE CUT (TMS)], [DIE-CUT (TMS)], [DOWN (TMS)], [DOWN FILLING (TMS)], [EM/DEBOSS (I/H) (TMS)]
		, [EMBOSS/DEBOSS (PCS)], [EMBOSS/DEBOSS (Price)], [EMBROIDERY (STITCH)], [EMBROIDERY (Price)], [EMBROIDERY EYELET (TMS)], [EMBROIDERY TMS (TMS)]
		, [FARM OUT QUILTING (PCS)], [FARM OUT QUILTING (Price)], [FEEDOFARM (TMS)], [FLATLOCK (TMS)], [Fusible (TMS)], [Garment Dye (PCS)]
		, [Garment Dye (Price)], [GMT WASH (PCS)], [GMT WASH (Price)], [HEAT SET PLEAT (PCS)], [HEAT SET PLEAT (Price)], [HEAT TRANSFER (PANEL)]
		, [HEAT TRANSFER (TMS)], [INDIRECT MANPOWER (TMS)], [INSPECTION (TMS)], [INTENSIVE MACHINE (TMS)], [JOKERTAG (TMS)], [KEY BUTTONHOLE (TMS)]
		, [LASER (TMS)], [LASER CUTTER (TMS)], [LASER-AXLETREE (TMS)], [LASER-GALVANOMETER (TMS)], [PACKING (TMS)], [PAD PRINTING (PCS)], [PAD PRINTING (Price)]
		, [POCKET WELT], [PRESSING (TMS)], [PRINTING (PCS)], [PRINTING (Price)], [PRINTING PPU (PPU)], [POSubCon], [SubCon], [QUILTING (Price)]
		, [QUILTING(AT) (TMS)], [QUILTING(HAND) (TMS)], [REAL FLATSEAM (TMS)], [RECOAT Garment], [REPAIR GARMENT (Price)], [ROLLER SUBLIMATION (TMS)]
		, [SEAM TAPING MACHINE (TMS)], [SEAMSEAL (TMS)], [SEWING (TMS)], [S-HOT PRESS(BONDING) (TMS)], [S-HOT PRESS(HT) (TMS)], [S-HOT PRESS(SEWING) (TMS)]
		, [SMALL HOT PRESS (TMS)], [SMALL HOT PRESS-LONG (TMS)], [SUBLIMATION PRINT (TMS)], [SUBLIMATION ROLLER (TMS)], [SUBLIMATION SPRAY (TMS)]
		, [ULTRASONIC (TMS)], [ULTRASONIC MACHINE (TMS)], [ULTRASONIC-ELASTIC (TMS)], [ULTRASONIC-LABEL ASM (TMS)], [ULTRASONIC-TAPE CUT (TMS)]
		, [VELCRO MACHINE (TMS)], [VELCROED M/C (TMS)], [WELDED (TMS)], [WELTED M/C (TMS)], [ZIG ZAG (TMS)], [ZIPPER HOT PRESS (TMS)], [TTL_TMS]
		, [TTL_3FLATLOCK (TMS)], [TTL_4FLATLOCK-H (TMS)], [TTL_4FLATLOCK-S (TMS)], [TTL_AE RING-KNIFE (TMS)], [TTL_AE RING-ULTRASONIC (TMS)], [TTL_AT (TMS)]
		, [TTL_AT (HAND) (TMS)], [TTL_AT (MACHINE) (TMS)], [TTL_AUTO BELT LOOPER (TMS)], [TTL_AUTO POCKET WELT (TMS)], [TTL_AUTO-TEMPLATE (TMS)]
		, [TTL_AUTO-TEMPLATE QT (TMS)], [TTL_B-HOT PRESS(BONDING) (TMS)], [TTL_B-HOT PRESS(SUB-PRT) (TMS)], [TTL_BH-TWICE FOR PLACKET (TMS)]
		, [TTL_BH-TWICE FOR WB (TMS)], [TTL_BIG HOT FOR BONDING (TMS)], [TTL_BIG HOT PRESS (TMS)], [TTL_BLINDSTITCH (TMS)], [TTL_BONDING (HAND) (TMS)]
		, [TTL_BONDING (MACHINE) (TMS)], [TTL_COVERSTITCH CYLINDER (TMS)], [TTL_COVERSTITCH L-CUTTER (TMS)], [TTL_CUTTING (TMS)], [TTL_CUTTING TAPE (Price)]
		, [TTL_D-chain ZIG ZAG (TMS)], [TTL_DIE CUT (TMS)], [TTL_DIE-CUT (TMS)], [TTL_DOWN (TMS)], [TTL_DOWN FILLING (TMS)], [TTL_EM/DEBOSS (I/H) (TMS)]
		, [TTL_EMBOSS/DEBOSS (PCS)], [TTL_EMBOSS/DEBOSS (Price)], [TTL_EMBROIDERY (STITCH)], [TTL_EMBROIDERY (Price)], [TTL_EMBROIDERY EYELET (TMS)]
		, [TTL_EMBROIDERY TMS (TMS)], [TTL_FARM OUT QUILTING (PCS)], [TTL_FARM OUT QUILTING (Price)], [TTL_FEEDOFARM (TMS)], [TTL_FLATLOCK (TMS)], [TTL_Fusible (TMS)]
		, [TTL_Garment Dye (PCS)], [TTL_Garment Dye (Price)], [TTL_GMT WASH (PCS)], [TTL_GMT WASH (Price)], [TTL_HEAT SET PLEAT (PCS)], [TTL_HEAT SET PLEAT (Price)]
		, [TTL_HEAT TRANSFER (PANEL)], [TTL_HEAT TRANSFER (TMS)], [TTL_INDIRECT MANPOWER (TMS)], [TTL_INSPECTION (TMS)], [TTL_INTENSIVE MACHINE (TMS)]
		, [TTL_JOKERTAG (TMS)], [TTL_KEY BUTTONHOLE (TMS)], [TTL_LASER (TMS)], [TTL_LASER CUTTER (TMS)], [TTL_LASER-AXLETREE (TMS)], [TTL_LASER-GALVANOMETER (TMS)]
		, [TTL_PACKING (TMS)], [TTL_PAD PRINTING (PCS)], [TTL_PAD PRINTING (Price)], [TTL_POCKET WELT], [TTL_PRESSING (TMS)], [TTL_PRINTING (PCS)], [TTL_PRINTING (Price)]
		, [TTL_PRINTING PPU (PPU)], [TTL_QUILTING (Price)], [TTL_QUILTING(AT) (TMS)], [TTL_QUILTING(HAND) (TMS)], [TTL_REAL FLATSEAM (TMS)], [TTL_RECOAT Garment]
		, [TTL_REPAIR GARMENT (Price)], [TTL_ROLLER SUBLIMATION (TMS)], [TTL_SEAM TAPING MACHINE (TMS)], [TTL_SEAMSEAL (TMS)], [TTL_SEWING (TMS)], [TTL_S-HOT PRESS(BONDING) (TMS)]
		, [TTL_S-HOT PRESS(HT) (TMS)], [TTL_S-HOT PRESS(SEWING) (TMS)], [TTL_SMALL HOT PRESS (TMS)], [TTL_SMALL HOT PRESS-LONG (TMS)], [TTL_SUBLIMATION PRINT (TMS)]
		, [TTL_SUBLIMATION ROLLER (TMS)], [TTL_SUBLIMATION SPRAY (TMS)], [TTL_ULTRASONIC (TMS)], [TTL_ULTRASONIC MACHINE (TMS)], [TTL_ULTRASONIC-ELASTIC (TMS)]
		, [TTL_ULTRASONIC-LABEL ASM (TMS)], [TTL_ULTRASONIC-TAPE CUT (TMS)], [TTL_VELCRO MACHINE (TMS)], [TTL_VELCROED M/C (TMS)], [TTL_WELDED (TMS)], [TTL_WELTED M/C (TMS)]
		, [TTL_ZIG ZAG (TMS)], [TTL_ZIPPER HOT PRESS (TMS)])
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
		, ISNULL(t.[3FLATLOCK (TMS)], 0)
		, ISNULL(t.[4FLATLOCK-H (TMS)], 0)
		, ISNULL(t.[4FLATLOCK-S (TMS)], 0)
		, ISNULL(t.[AE RING-KNIFE (TMS)], 0)
		, ISNULL(t.[AE RING-ULTRASONIC (TMS)], 0)
		, ISNULL(t.[AT (TMS)], 0)
		, ISNULL(t.[AT (HAND) (TMS)], 0)
		, ISNULL(t.[AT (MACHINE) (TMS)], 0)
		, ISNULL(t.[AUTO BELT LOOPER (TMS)], 0)
		, ISNULL(t.[AUTO POCKET WELT (TMS)], 0)
		, ISNULL(t.[AUTO-TEMPLATE (TMS)], 0)
		, ISNULL(t.[AUTO-TEMPLATE QT (TMS)], 0)
		, ISNULL(t.[B-HOT PRESS(BONDING) (TMS)], 0)
		, ISNULL(t.[B-HOT PRESS(SUB-PRT) (TMS)], 0)
		, ISNULL(t.[BH-TWICE FOR PLACKET (TMS)], 0)
		, ISNULL(t.[BH-TWICE FOR WB (TMS)], 0)
		, ISNULL(t.[BIG HOT FOR BONDING (TMS)], 0)
		, ISNULL(t.[BIG HOT PRESS (TMS)], 0)
		, ISNULL(t.[BLINDSTITCH (TMS)], 0)
		, ISNULL(t.[BONDING (HAND) (TMS)], 0)
		, ISNULL(t.[BONDING (MACHINE) (TMS)], 0)
		, ISNULL(t.[COVERSTITCH CYLINDER (TMS)], 0)
		, ISNULL(t.[COVERSTITCH L-CUTTER (TMS)], 0)
		, ISNULL(t.[CUTTING (TMS)], 0)
		, ISNULL(t.[CUTTING TAPE (Price)], 0)
		, ISNULL(t.[D-chain ZIG ZAG (TMS)], 0)
		, ISNULL(t.[DIE CUT (TMS)], 0)
		, ISNULL(t.[DIE-CUT (TMS)], 0)
		, ISNULL(t.[DOWN (TMS)], 0)
		, ISNULL(t.[DOWN FILLING (TMS)], 0)
		, ISNULL(t.[EM/DEBOSS (I/H) (TMS)], 0)
		, ISNULL(t.[EMBOSS/DEBOSS (PCS)], 0)
		, ISNULL(t.[EMBOSS/DEBOSS (Price)], 0)
		, ISNULL(t.[EMBROIDERY (STITCH)], 0)
		, ISNULL(t.[EMBROIDERY (Price)], 0)
		, ISNULL(t.[EMBROIDERY EYELET (TMS)], 0)
		, ISNULL(t.[EMBROIDERY TMS (TMS)], 0)
		, ISNULL(t.[FARM OUT QUILTING (PCS)], 0)
		, ISNULL(t.[FARM OUT QUILTING (Price)], 0)
		, ISNULL(t.[FEEDOFARM (TMS)], 0)
		, ISNULL(t.[FLATLOCK (TMS)], 0)
		, ISNULL(t.[Fusible (TMS)], 0)
		, ISNULL(t.[Garment Dye (PCS)], 0)
		, ISNULL(t.[Garment Dye (Price)], 0)
		, ISNULL(t.[GMT WASH (PCS)], 0)
		, ISNULL(t.[GMT WASH (Price)], 0)
		, ISNULL(t.[HEAT SET PLEAT (PCS)], 0)
		, ISNULL(t.[HEAT SET PLEAT (Price)], 0)
		, ISNULL(t.[HEAT TRANSFER (PANEL)], 0)
		, ISNULL(t.[HEAT TRANSFER (TMS)], 0)
		, ISNULL(t.[INDIRECT MANPOWER (TMS)], 0)
		, ISNULL(t.[INSPECTION (TMS)], 0)
		, ISNULL(t.[INTENSIVE MACHINE (TMS)], 0)
		, ISNULL(t.[JOKERTAG (TMS)], 0)
		, ISNULL(t.[KEY BUTTONHOLE (TMS)], 0)
		, ISNULL(t.[LASER (TMS)], 0)
		, ISNULL(t.[LASER CUTTER (TMS)], 0)
		, ISNULL(t.[LASER-AXLETREE (TMS)], 0)
		, ISNULL(t.[LASER-GALVANOMETER (TMS)], 0)
		, ISNULL(t.[PACKING (TMS)], 0)
		, ISNULL(t.[PAD PRINTING (PCS)], 0)
		, ISNULL(t.[PAD PRINTING (Price)], 0)
		, ISNULL(t.[POCKET WELT], 0)
		, ISNULL(t.[PRESSING (TMS)], 0)
		, ISNULL(t.[PRINTING (PCS)], 0)
		, ISNULL(t.[PRINTING (Price)], 0)
		, ISNULL(t.[PRINTING PPU (PPU)], 0)
		, ISNULL(t.[POSubCon], '')
		, ISNULL(t.[SubCon], '')
		, ISNULL(t.[QUILTING (Price)], 0)
		, ISNULL(t.[QUILTING(AT) (TMS)], 0)
		, ISNULL(t.[QUILTING(HAND) (TMS)], 0)
		, ISNULL(t.[REAL FLATSEAM (TMS)], 0)
		, ISNULL(t.[RECOAT Garment], 0)
		, ISNULL(t.[REPAIR GARMENT (Price)], 0)
		, ISNULL(t.[ROLLER SUBLIMATION (TMS)], 0)
		, ISNULL(t.[SEAM TAPING MACHINE (TMS)], 0)
		, ISNULL(t.[SEAMSEAL (TMS)], 0)
		, ISNULL(t.[SEWING (TMS)], 0)
		, ISNULL(t.[S-HOT PRESS(BONDING) (TMS)], 0)
		, ISNULL(t.[S-HOT PRESS(HT) (TMS)], 0)
		, ISNULL(t.[S-HOT PRESS(SEWING) (TMS)], 0)
		, ISNULL(t.[SMALL HOT PRESS (TMS)], 0)
		, ISNULL(t.[SMALL HOT PRESS-LONG (TMS)], 0)
		, ISNULL(t.[SUBLIMATION PRINT (TMS)], 0)
		, ISNULL(t.[SUBLIMATION ROLLER (TMS)], 0)
		, ISNULL(t.[SUBLIMATION SPRAY (TMS)], 0)
		, ISNULL(t.[ULTRASONIC (TMS)], 0)
		, ISNULL(t.[ULTRASONIC MACHINE (TMS)], 0)
		, ISNULL(t.[ULTRASONIC-ELASTIC (TMS)], 0)
		, ISNULL(t.[ULTRASONIC-LABEL ASM (TMS)], 0)
		, ISNULL(t.[ULTRASONIC-TAPE CUT (TMS)], 0)
		, ISNULL(t.[VELCRO MACHINE (TMS)], 0)
		, ISNULL(t.[VELCROED M/C (TMS)], 0)
		, ISNULL(t.[WELDED (TMS)], 0)
		, ISNULL(t.[WELTED M/C (TMS)], 0)
		, ISNULL(t.[ZIG ZAG (TMS)], 0)
		, ISNULL(t.[ZIPPER HOT PRESS (TMS)], 0)
		, ISNULL(t.[TTL_TMS], 0)
		, ISNULL(t.[TTL_3FLATLOCK (TMS)], 0)
		, ISNULL(t.[TTL_4FLATLOCK-H (TMS)], 0)
		, ISNULL(t.[TTL_4FLATLOCK-S (TMS)], 0)
		, ISNULL(t.[TTL_AE RING-KNIFE (TMS)], 0)
		, ISNULL(t.[TTL_AE RING-ULTRASONIC (TMS)], 0)
		, ISNULL(t.[TTL_AT (TMS)], 0)
		, ISNULL(t.[TTL_AT (HAND) (TMS)], 0)
		, ISNULL(t.[TTL_AT (MACHINE) (TMS)], 0)
		, ISNULL(t.[TTL_AUTO BELT LOOPER (TMS)], 0)
		, ISNULL(t.[TTL_AUTO POCKET WELT (TMS)], 0)
		, ISNULL(t.[TTL_AUTO-TEMPLATE (TMS)], 0)
		, ISNULL(t.[TTL_AUTO-TEMPLATE QT (TMS)], 0)
		, ISNULL(t.[TTL_B-HOT PRESS(BONDING) (TMS)], 0)
		, ISNULL(t.[TTL_B-HOT PRESS(SUB-PRT) (TMS)], 0)
		, ISNULL(t.[TTL_BH-TWICE FOR PLACKET (TMS)], 0)
		, ISNULL(t.[TTL_BH-TWICE FOR WB (TMS)], 0)
		, ISNULL(t.[TTL_BIG HOT FOR BONDING (TMS)], 0)
		, ISNULL(t.[TTL_BIG HOT PRESS (TMS)], 0)
		, ISNULL(t.[TTL_BLINDSTITCH (TMS)], 0)
		, ISNULL(t.[TTL_BONDING (HAND) (TMS)], 0)
		, ISNULL(t.[TTL_BONDING (MACHINE) (TMS)], 0)
		, ISNULL(t.[TTL_COVERSTITCH CYLINDER (TMS)], 0)
		, ISNULL(t.[TTL_COVERSTITCH L-CUTTER (TMS)], 0)
		, ISNULL(t.[TTL_CUTTING (TMS)], 0)
		, ISNULL(t.[TTL_CUTTING TAPE (Price)], 0)
		, ISNULL(t.[TTL_D-chain ZIG ZAG (TMS)], 0)
		, ISNULL(t.[TTL_DIE CUT (TMS)], 0)
		, ISNULL(t.[TTL_DIE-CUT (TMS)], 0)
		, ISNULL(t.[TTL_DOWN (TMS)], 0)
		, ISNULL(t.[TTL_DOWN FILLING (TMS)], 0)
		, ISNULL(t.[TTL_EM/DEBOSS (I/H) (TMS)], 0)
		, ISNULL(t.[TTL_EMBOSS/DEBOSS (PCS)], 0)
		, ISNULL(t.[TTL_EMBOSS/DEBOSS (Price)], 0)
		, ISNULL(t.[TTL_EMBROIDERY (STITCH)], 0)
		, ISNULL(t.[TTL_EMBROIDERY (Price)], 0)
		, ISNULL(t.[TTL_EMBROIDERY EYELET (TMS)], 0)
		, ISNULL(t.[TTL_EMBROIDERY TMS (TMS)], 0)
		, ISNULL(t.[TTL_FARM OUT QUILTING (PCS)], 0)
		, ISNULL(t.[TTL_FARM OUT QUILTING (Price)], 0)
		, ISNULL(t.[TTL_FEEDOFARM (TMS)], 0)
		, ISNULL(t.[TTL_FLATLOCK (TMS)], 0)
		, ISNULL(t.[TTL_Fusible (TMS)], 0)
		, ISNULL(t.[TTL_Garment Dye (PCS)], 0)
		, ISNULL(t.[TTL_Garment Dye (Price)], 0)
		, ISNULL(t.[TTL_GMT WASH (PCS)], 0)
		, ISNULL(t.[TTL_GMT WASH (Price)], 0)
		, ISNULL(t.[TTL_HEAT SET PLEAT (PCS)], 0)
		, ISNULL(t.[TTL_HEAT SET PLEAT (Price)], 0)
		, ISNULL(t.[TTL_HEAT TRANSFER (PANEL)], 0)
		, ISNULL(t.[TTL_HEAT TRANSFER (TMS)], 0)
		, ISNULL(t.[TTL_INDIRECT MANPOWER (TMS)], 0)
		, ISNULL(t.[TTL_INSPECTION (TMS)], 0)
		, ISNULL(t.[TTL_INTENSIVE MACHINE (TMS)], 0)
		, ISNULL(t.[TTL_JOKERTAG (TMS)], 0)
		, ISNULL(t.[TTL_KEY BUTTONHOLE (TMS)], 0)
		, ISNULL(t.[TTL_LASER (TMS)], 0)
		, ISNULL(t.[TTL_LASER CUTTER (TMS)], 0)
		, ISNULL(t.[TTL_LASER-AXLETREE (TMS)], 0)
		, ISNULL(t.[TTL_LASER-GALVANOMETER (TMS)], 0)
		, ISNULL(t.[TTL_PACKING (TMS)], 0)
		, ISNULL(t.[TTL_PAD PRINTING (PCS)], 0)
		, ISNULL(t.[TTL_PAD PRINTING (Price)], 0)
		, ISNULL(t.[TTL_POCKET WELT], 0)
		, ISNULL(t.[TTL_PRESSING (TMS)], 0)
		, ISNULL(t.[TTL_PRINTING (PCS)], 0)
		, ISNULL(t.[TTL_PRINTING (Price)], 0)
		, ISNULL(t.[TTL_PRINTING PPU (PPU)], 0)
		, ISNULL(t.[TTL_QUILTING (Price)], 0)
		, ISNULL(t.[TTL_QUILTING(AT) (TMS)], 0)
		, ISNULL(t.[TTL_QUILTING(HAND) (TMS)], 0)
		, ISNULL(t.[TTL_REAL FLATSEAM (TMS)], 0)
		, ISNULL(t.[TTL_RECOAT Garment], 0)
		, ISNULL(t.[TTL_REPAIR GARMENT (Price)], 0)
		, ISNULL(t.[TTL_ROLLER SUBLIMATION (TMS)], 0)
		, ISNULL(t.[TTL_SEAM TAPING MACHINE (TMS)], 0)
		, ISNULL(t.[TTL_SEAMSEAL (TMS)], 0)
		, ISNULL(t.[TTL_SEWING (TMS)], 0)
		, ISNULL(t.[TTL_S-HOT PRESS(BONDING) (TMS)], 0)
		, ISNULL(t.[TTL_S-HOT PRESS(HT) (TMS)], 0)
		, ISNULL(t.[TTL_S-HOT PRESS(SEWING) (TMS)], 0)
		, ISNULL(t.[TTL_SMALL HOT PRESS (TMS)], 0)
		, ISNULL(t.[TTL_SMALL HOT PRESS-LONG (TMS)], 0)
		, ISNULL(t.[TTL_SUBLIMATION PRINT (TMS)], 0)
		, ISNULL(t.[TTL_SUBLIMATION ROLLER (TMS)], 0)
		, ISNULL(t.[TTL_SUBLIMATION SPRAY (TMS)], 0)
		, ISNULL(t.[TTL_ULTRASONIC (TMS)], 0)
		, ISNULL(t.[TTL_ULTRASONIC MACHINE (TMS)], 0)
		, ISNULL(t.[TTL_ULTRASONIC-ELASTIC (TMS)], 0)
		, ISNULL(t.[TTL_ULTRASONIC-LABEL ASM (TMS)], 0)
		, ISNULL(t.[TTL_ULTRASONIC-TAPE CUT (TMS)], 0)
		, ISNULL(t.[TTL_VELCRO MACHINE (TMS)], 0)
		, ISNULL(t.[TTL_VELCROED M/C (TMS)], 0)
		, ISNULL(t.[TTL_WELDED (TMS)], 0)
		, ISNULL(t.[TTL_WELTED M/C (TMS)], 0)
		, ISNULL(t.[TTL_ZIG ZAG (TMS)], 0)
		, ISNULL(t.[TTL_ZIPPER HOT PRESS (TMS)], 0)
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
			, p.[3FLATLOCK (TMS)] = ISNULL(t.[3FLATLOCK (TMS)], 0)
			, p.[4FLATLOCK-H (TMS)] = ISNULL(t.[4FLATLOCK-H (TMS)], 0)
			, p.[4FLATLOCK-S (TMS)] = ISNULL(t.[4FLATLOCK-S (TMS)], 0)
			, p.[AE RING-KNIFE (TMS)] = ISNULL(t.[AE RING-KNIFE (TMS)], 0)
			, p.[AE RING-ULTRASONIC (TMS)] = ISNULL(t.[AE RING-ULTRASONIC (TMS)], 0)
			, p.[AT (TMS)] = ISNULL(t.[AT (TMS)], 0)
			, p.[AT (HAND) (TMS)] = ISNULL(t.[AT (HAND) (TMS)], 0)
			, p.[AT (MACHINE) (TMS)] = ISNULL(t.[AT (MACHINE) (TMS)], 0)
			, p.[AUTO BELT LOOPER (TMS)] = ISNULL(t.[AUTO BELT LOOPER (TMS)], 0)
			, p.[AUTO POCKET WELT (TMS)] = ISNULL(t.[AUTO POCKET WELT (TMS)], 0)
			, p.[AUTO-TEMPLATE (TMS)] = ISNULL(t.[AUTO-TEMPLATE (TMS)], 0)
			, p.[AUTO-TEMPLATE QT (TMS)] = ISNULL(t.[AUTO-TEMPLATE QT (TMS)], 0)
			, p.[B-HOT PRESS(BONDING) (TMS)] = ISNULL(t.[B-HOT PRESS(BONDING) (TMS)], 0)
			, p.[B-HOT PRESS(SUB-PRT) (TMS)] = ISNULL(t.[B-HOT PRESS(SUB-PRT) (TMS)], 0)
			, p.[BH-TWICE FOR PLACKET (TMS)] = ISNULL(t.[BH-TWICE FOR PLACKET (TMS)], 0)
			, p.[BH-TWICE FOR WB (TMS)] = ISNULL(t.[BH-TWICE FOR WB (TMS)], 0)
			, p.[BIG HOT FOR BONDING (TMS)] = ISNULL(t.[BIG HOT FOR BONDING (TMS)], 0)
			, p.[BIG HOT PRESS (TMS)] = ISNULL(t.[BIG HOT PRESS (TMS)], 0)
			, p.[BLINDSTITCH (TMS)] = ISNULL(t.[BLINDSTITCH (TMS)], 0)
			, p.[BONDING (HAND) (TMS)] = ISNULL(t.[BONDING (HAND) (TMS)], 0)
			, p.[BONDING (MACHINE) (TMS)] = ISNULL(t.[BONDING (MACHINE) (TMS)], 0)
			, p.[COVERSTITCH CYLINDER (TMS)] = ISNULL(t.[COVERSTITCH CYLINDER (TMS)], 0)
			, p.[COVERSTITCH L-CUTTER (TMS)] = ISNULL(t.[COVERSTITCH L-CUTTER (TMS)], 0)
			, p.[CUTTING (TMS)] = ISNULL(t.[CUTTING (TMS)], 0)
			, p.[CUTTING TAPE (Price)] = ISNULL(t.[CUTTING TAPE (Price)], 0)
			, p.[D-chain ZIG ZAG (TMS)] = ISNULL(t.[D-chain ZIG ZAG (TMS)], 0)
			, p.[DIE CUT (TMS)] = ISNULL(t.[DIE CUT (TMS)], 0)
			, p.[DIE-CUT (TMS)] = ISNULL(t.[DIE-CUT (TMS)], 0)
			, p.[DOWN (TMS)] = ISNULL(t.[DOWN (TMS)], 0)
			, p.[DOWN FILLING (TMS)] = ISNULL(t.[DOWN FILLING (TMS)], 0)
			, p.[EM/DEBOSS (I/H) (TMS)] = ISNULL(t.[EM/DEBOSS (I/H) (TMS)], 0)
			, p.[EMBOSS/DEBOSS (PCS)] = ISNULL(t.[EMBOSS/DEBOSS (PCS)], 0)
			, p.[EMBOSS/DEBOSS (Price)] = ISNULL(t.[EMBOSS/DEBOSS (Price)], 0)
			, p.[EMBROIDERY (STITCH)] = ISNULL(t.[EMBROIDERY (STITCH)], 0)
			, p.[EMBROIDERY (Price)] = ISNULL(t.[EMBROIDERY (Price)], 0)
			, p.[EMBROIDERY EYELET (TMS)] = ISNULL(t.[EMBROIDERY EYELET (TMS)], 0)
			, p.[EMBROIDERY TMS (TMS)] = ISNULL(t.[EMBROIDERY TMS (TMS)], 0)
			, p.[FARM OUT QUILTING (PCS)] = ISNULL(t.[FARM OUT QUILTING (PCS)], 0)
			, p.[FARM OUT QUILTING (Price)] = ISNULL(t.[FARM OUT QUILTING (Price)], 0)
			, p.[FEEDOFARM (TMS)] = ISNULL(t.[FEEDOFARM (TMS)], 0)
			, p.[FLATLOCK (TMS)] = ISNULL(t.[FLATLOCK (TMS)], 0)
			, p.[Fusible (TMS)] = ISNULL(t.[Fusible (TMS)], 0)
			, p.[Garment Dye (PCS)] = ISNULL(t.[Garment Dye (PCS)], 0)
			, p.[Garment Dye (Price)] = ISNULL(t.[Garment Dye (Price)], 0)
			, p.[GMT WASH (PCS)] = ISNULL(t.[GMT WASH (PCS)], 0)
			, p.[GMT WASH (Price)] = ISNULL(t.[GMT WASH (Price)], 0)
			, p.[HEAT SET PLEAT (PCS)] = ISNULL(t.[HEAT SET PLEAT (PCS)], 0)
			, p.[HEAT SET PLEAT (Price)] = ISNULL(t.[HEAT SET PLEAT (Price)], 0)
			, p.[HEAT TRANSFER (PANEL)] = ISNULL(t.[HEAT TRANSFER (PANEL)], 0)
			, p.[HEAT TRANSFER (TMS)] = ISNULL(t.[HEAT TRANSFER (TMS)], 0)
			, p.[INDIRECT MANPOWER (TMS)] = ISNULL(t.[INDIRECT MANPOWER (TMS)], 0)
			, p.[INSPECTION (TMS)] = ISNULL(t.[INSPECTION (TMS)], 0)
			, p.[INTENSIVE MACHINE (TMS)] = ISNULL(t.[INTENSIVE MACHINE (TMS)], 0)
			, p.[JOKERTAG (TMS)] = ISNULL(t.[JOKERTAG (TMS)], 0)
			, p.[KEY BUTTONHOLE (TMS)] = ISNULL(t.[KEY BUTTONHOLE (TMS)], 0)
			, p.[LASER (TMS)] = ISNULL(t.[LASER (TMS)], 0)
			, p.[LASER CUTTER (TMS)] = ISNULL(t.[LASER CUTTER (TMS)], 0)
			, p.[LASER-AXLETREE (TMS)] = ISNULL(t.[LASER-AXLETREE (TMS)], 0)
			, p.[LASER-GALVANOMETER (TMS)] = ISNULL(t.[LASER-GALVANOMETER (TMS)], 0)
			, p.[PACKING (TMS)] = ISNULL(t.[PACKING (TMS)], 0)
			, p.[PAD PRINTING (PCS)] = ISNULL(t.[PAD PRINTING (PCS)], 0)
			, p.[PAD PRINTING (Price)] = ISNULL(t.[PAD PRINTING (Price)], 0)
			, p.[POCKET WELT] = ISNULL(t.[POCKET WELT], 0)
			, p.[PRESSING (TMS)] = ISNULL(t.[PRESSING (TMS)], 0)
			, p.[PRINTING (PCS)] = ISNULL(t.[PRINTING (PCS)], 0)
			, p.[PRINTING (Price)] = ISNULL(t.[PRINTING (Price)], 0)
			, p.[PRINTING PPU (PPU)] = ISNULL(t.[PRINTING PPU (PPU)], 0)
			, p.[POSubCon] = ISNULL(t.[POSubCon], '')
			, p.[SubCon] = ISNULL(t.[SubCon], '')
			, p.[QUILTING (Price)] = ISNULL(t.[QUILTING (Price)], 0)
			, p.[QUILTING(AT) (TMS)] = ISNULL(t.[QUILTING(AT) (TMS)], 0)
			, p.[QUILTING(HAND) (TMS)] = ISNULL(t.[QUILTING(HAND) (TMS)], 0)
			, p.[REAL FLATSEAM (TMS)] = ISNULL(t.[REAL FLATSEAM (TMS)], 0)
			, p.[RECOAT Garment] = ISNULL(t.[RECOAT Garment], 0)
			, p.[REPAIR GARMENT (Price)] = ISNULL(t.[REPAIR GARMENT (Price)], 0)
			, p.[ROLLER SUBLIMATION (TMS)] = ISNULL(t.[ROLLER SUBLIMATION (TMS)], 0)
			, p.[SEAM TAPING MACHINE (TMS)] = ISNULL(t.[SEAM TAPING MACHINE (TMS)], 0)
			, p.[SEAMSEAL (TMS)] = ISNULL(t.[SEAMSEAL (TMS)], 0)
			, p.[SEWING (TMS)] = ISNULL(t.[SEWING (TMS)], 0)
			, p.[S-HOT PRESS(BONDING) (TMS)] = ISNULL(t.[S-HOT PRESS(BONDING) (TMS)], 0)
			, p.[S-HOT PRESS(HT) (TMS)] = ISNULL(t.[S-HOT PRESS(HT) (TMS)], 0)
			, p.[S-HOT PRESS(SEWING) (TMS)] = ISNULL(t.[S-HOT PRESS(SEWING) (TMS)], 0)
			, p.[SMALL HOT PRESS (TMS)] = ISNULL(t.[SMALL HOT PRESS (TMS)], 0)
			, p.[SMALL HOT PRESS-LONG (TMS)] = ISNULL(t.[SMALL HOT PRESS-LONG (TMS)], 0)
			, p.[SUBLIMATION PRINT (TMS)] = ISNULL(t.[SUBLIMATION PRINT (TMS)], 0)
			, p.[SUBLIMATION ROLLER (TMS)] = ISNULL(t.[SUBLIMATION ROLLER (TMS)], 0)
			, p.[SUBLIMATION SPRAY (TMS)] = ISNULL(t.[SUBLIMATION SPRAY (TMS)], 0)
			, p.[ULTRASONIC (TMS)] = ISNULL(t.[ULTRASONIC (TMS)], 0)
			, p.[ULTRASONIC MACHINE (TMS)] = ISNULL(t.[ULTRASONIC MACHINE (TMS)], 0)
			, p.[ULTRASONIC-ELASTIC (TMS)] = ISNULL(t.[ULTRASONIC-ELASTIC (TMS)], 0)
			, p.[ULTRASONIC-LABEL ASM (TMS)] = ISNULL(t.[ULTRASONIC-LABEL ASM (TMS)], 0)
			, p.[ULTRASONIC-TAPE CUT (TMS)] = ISNULL(t.[ULTRASONIC-TAPE CUT (TMS)], 0)
			, p.[VELCRO MACHINE (TMS)] = ISNULL(t.[VELCRO MACHINE (TMS)], 0)
			, p.[VELCROED M/C (TMS)] = ISNULL(t.[VELCROED M/C (TMS)], 0)
			, p.[WELDED (TMS)] = ISNULL(t.[WELDED (TMS)], 0)
			, p.[WELTED M/C (TMS)] = ISNULL(t.[WELTED M/C (TMS)], 0)
			, p.[ZIG ZAG (TMS)] = ISNULL(t.[ZIG ZAG (TMS)], 0)
			, p.[ZIPPER HOT PRESS (TMS)] = ISNULL(t.[ZIPPER HOT PRESS (TMS)], 0)
			, p.[TTL_TMS] = ISNULL(t.[TTL_TMS], 0)
			, p.[TTL_3FLATLOCK (TMS)] = ISNULL(t.[TTL_3FLATLOCK (TMS)], 0)
			, p.[TTL_4FLATLOCK-H (TMS)] = ISNULL(t.[TTL_4FLATLOCK-H (TMS)], 0)
			, p.[TTL_4FLATLOCK-S (TMS)] = ISNULL(t.[TTL_4FLATLOCK-S (TMS)], 0)
			, p.[TTL_AE RING-KNIFE (TMS)] = ISNULL(t.[TTL_AE RING-KNIFE (TMS)], 0)
			, p.[TTL_AE RING-ULTRASONIC (TMS)] = ISNULL(t.[TTL_AE RING-ULTRASONIC (TMS)], 0)
			, p.[TTL_AT (TMS)] = ISNULL(t.[TTL_AT (TMS)], 0)
			, p.[TTL_AT (HAND) (TMS)] = ISNULL(t.[TTL_AT (HAND) (TMS)], 0)
			, p.[TTL_AT (MACHINE) (TMS)] = ISNULL(t.[TTL_AT (MACHINE) (TMS)], 0)
			, p.[TTL_AUTO BELT LOOPER (TMS)] = ISNULL(t.[TTL_AUTO BELT LOOPER (TMS)], 0)
			, p.[TTL_AUTO POCKET WELT (TMS)] = ISNULL(t.[TTL_AUTO POCKET WELT (TMS)], 0)
			, p.[TTL_AUTO-TEMPLATE (TMS)] = ISNULL(t.[TTL_AUTO-TEMPLATE (TMS)], 0)
			, p.[TTL_AUTO-TEMPLATE QT (TMS)] = ISNULL(t.[TTL_AUTO-TEMPLATE QT (TMS)], 0)
			, p.[TTL_B-HOT PRESS(BONDING) (TMS)] = ISNULL(t.[TTL_B-HOT PRESS(BONDING) (TMS)], 0)
			, p.[TTL_B-HOT PRESS(SUB-PRT) (TMS)] = ISNULL(t.[TTL_B-HOT PRESS(SUB-PRT) (TMS)], 0)
			, p.[TTL_BH-TWICE FOR PLACKET (TMS)] = ISNULL(t.[TTL_BH-TWICE FOR PLACKET (TMS)], 0)
			, p.[TTL_BH-TWICE FOR WB (TMS)] = ISNULL(t.[TTL_BH-TWICE FOR WB (TMS)], 0)
			, p.[TTL_BIG HOT FOR BONDING (TMS)] = ISNULL(t.[TTL_BIG HOT FOR BONDING (TMS)], 0)
			, p.[TTL_BIG HOT PRESS (TMS)] = ISNULL(t.[TTL_BIG HOT PRESS (TMS)], 0)
			, p.[TTL_BLINDSTITCH (TMS)] = ISNULL(t.[TTL_BLINDSTITCH (TMS)], 0)
			, p.[TTL_BONDING (HAND) (TMS)] = ISNULL(t.[TTL_BONDING (HAND) (TMS)], 0)
			, p.[TTL_BONDING (MACHINE) (TMS)] = ISNULL(t.[TTL_BONDING (MACHINE) (TMS)], 0)
			, p.[TTL_COVERSTITCH CYLINDER (TMS)] = ISNULL(t.[TTL_COVERSTITCH CYLINDER (TMS)], 0)
			, p.[TTL_COVERSTITCH L-CUTTER (TMS)] = ISNULL(t.[TTL_COVERSTITCH L-CUTTER (TMS)], 0)
			, p.[TTL_CUTTING (TMS)] = ISNULL(t.[TTL_CUTTING (TMS)], 0)
			, p.[TTL_CUTTING TAPE (Price)] = ISNULL(t.[TTL_CUTTING TAPE (Price)], 0)
			, p.[TTL_D-chain ZIG ZAG (TMS)] = ISNULL(t.[TTL_D-chain ZIG ZAG (TMS)], 0)
			, p.[TTL_DIE CUT (TMS)] = ISNULL(t.[TTL_DIE CUT (TMS)], 0)
			, p.[TTL_DIE-CUT (TMS)] = ISNULL(t.[TTL_DIE-CUT (TMS)], 0)
			, p.[TTL_DOWN (TMS)] = ISNULL(t.[TTL_DOWN (TMS)], 0)
			, p.[TTL_DOWN FILLING (TMS)] = ISNULL(t.[TTL_DOWN FILLING (TMS)], 0)
			, p.[TTL_EM/DEBOSS (I/H) (TMS)] = ISNULL(t.[TTL_EM/DEBOSS (I/H) (TMS)], 0)
			, p.[TTL_EMBOSS/DEBOSS (PCS)] = ISNULL(t.[TTL_EMBOSS/DEBOSS (PCS)], 0)
			, p.[TTL_EMBOSS/DEBOSS (Price)] = ISNULL(t.[TTL_EMBOSS/DEBOSS (Price)], 0)
			, p.[TTL_EMBROIDERY (STITCH)] = ISNULL(t.[TTL_EMBROIDERY (STITCH)], 0)
			, p.[TTL_EMBROIDERY (Price)] = ISNULL(t.[TTL_EMBROIDERY (Price)], 0)
			, p.[TTL_EMBROIDERY EYELET (TMS)] = ISNULL(t.[TTL_EMBROIDERY EYELET (TMS)], 0)
			, p.[TTL_EMBROIDERY TMS (TMS)] = ISNULL(t.[TTL_EMBROIDERY TMS (TMS)], 0)
			, p.[TTL_FARM OUT QUILTING (PCS)] = ISNULL(t.[TTL_FARM OUT QUILTING (PCS)], 0)
			, p.[TTL_FARM OUT QUILTING (Price)] = ISNULL(t.[TTL_FARM OUT QUILTING (Price)], 0)
			, p.[TTL_FEEDOFARM (TMS)] = ISNULL(t.[TTL_FEEDOFARM (TMS)], 0)
			, p.[TTL_FLATLOCK (TMS)] = ISNULL(t.[TTL_FLATLOCK (TMS)], 0)
			, p.[TTL_Fusible (TMS)] = ISNULL(t.[TTL_Fusible (TMS)], 0)
			, p.[TTL_Garment Dye (PCS)] = ISNULL(t.[TTL_Garment Dye (PCS)], 0)
			, p.[TTL_Garment Dye (Price)] = ISNULL(t.[TTL_Garment Dye (Price)], 0)
			, p.[TTL_GMT WASH (PCS)] = ISNULL(t.[TTL_GMT WASH (PCS)], 0)
			, p.[TTL_GMT WASH (Price)] = ISNULL(t.[TTL_GMT WASH (Price)], 0)
			, p.[TTL_HEAT SET PLEAT (PCS)] = ISNULL(t.[TTL_HEAT SET PLEAT (PCS)], 0)
			, p.[TTL_HEAT SET PLEAT (Price)] = ISNULL(t.[TTL_HEAT SET PLEAT (Price)], 0)
			, p.[TTL_HEAT TRANSFER (PANEL)] = ISNULL(t.[TTL_HEAT TRANSFER (PANEL)], 0)
			, p.[TTL_HEAT TRANSFER (TMS)] = ISNULL(t.[TTL_HEAT TRANSFER (TMS)], 0)
			, p.[TTL_INDIRECT MANPOWER (TMS)] = ISNULL(t.[TTL_INDIRECT MANPOWER (TMS)], 0)
			, p.[TTL_INSPECTION (TMS)] = ISNULL(t.[TTL_INSPECTION (TMS)], 0)
			, p.[TTL_INTENSIVE MACHINE (TMS)] = ISNULL(t.[TTL_INTENSIVE MACHINE (TMS)], 0)
			, p.[TTL_JOKERTAG (TMS)] = ISNULL(t.[TTL_JOKERTAG (TMS)], 0)
			, p.[TTL_KEY BUTTONHOLE (TMS)] = ISNULL(t.[TTL_KEY BUTTONHOLE (TMS)], 0)
			, p.[TTL_LASER (TMS)] = ISNULL(t.[TTL_LASER (TMS)], 0)
			, p.[TTL_LASER CUTTER (TMS)] = ISNULL(t.[TTL_LASER CUTTER (TMS)], 0)
			, p.[TTL_LASER-AXLETREE (TMS)] = ISNULL(t.[TTL_LASER-AXLETREE (TMS)], 0)
			, p.[TTL_LASER-GALVANOMETER (TMS)] = ISNULL(t.[TTL_LASER-GALVANOMETER (TMS)], 0)
			, p.[TTL_PACKING (TMS)] = ISNULL(t.[TTL_PACKING (TMS)], 0)
			, p.[TTL_PAD PRINTING (PCS)] = ISNULL(t.[TTL_PAD PRINTING (PCS)], 0)
			, p.[TTL_PAD PRINTING (Price)] = ISNULL(t.[TTL_PAD PRINTING (Price)], 0)
			, p.[TTL_POCKET WELT] = ISNULL(t.[TTL_POCKET WELT], 0)
			, p.[TTL_PRESSING (TMS)] = ISNULL(t.[TTL_PRESSING (TMS)], 0)
			, p.[TTL_PRINTING (PCS)] = ISNULL(t.[TTL_PRINTING (PCS)], 0)
			, p.[TTL_PRINTING (Price)] = ISNULL(t.[TTL_PRINTING (Price)], 0)
			, p.[TTL_PRINTING PPU (PPU)] = ISNULL(t.[TTL_PRINTING PPU (PPU)], 0)
			, p.[TTL_QUILTING (Price)] = ISNULL(t.[TTL_QUILTING (Price)], 0)
			, p.[TTL_QUILTING(AT) (TMS)] = ISNULL(t.[TTL_QUILTING(AT) (TMS)], 0)
			, p.[TTL_QUILTING(HAND) (TMS)] = ISNULL(t.[TTL_QUILTING(HAND) (TMS)], 0)
			, p.[TTL_REAL FLATSEAM (TMS)] = ISNULL(t.[TTL_REAL FLATSEAM (TMS)], 0)
			, p.[TTL_RECOAT Garment] = ISNULL(t.[TTL_RECOAT Garment], 0)
			, p.[TTL_REPAIR GARMENT (Price)] = ISNULL(t.[TTL_REPAIR GARMENT (Price)], 0)
			, p.[TTL_ROLLER SUBLIMATION (TMS)] = ISNULL(t.[TTL_ROLLER SUBLIMATION (TMS)], 0)
			, p.[TTL_SEAM TAPING MACHINE (TMS)] = ISNULL(t.[TTL_SEAM TAPING MACHINE (TMS)], 0)
			, p.[TTL_SEAMSEAL (TMS)] = ISNULL(t.[TTL_SEAMSEAL (TMS)], 0)
			, p.[TTL_SEWING (TMS)] = ISNULL(t.[TTL_SEWING (TMS)], 0)
			, p.[TTL_S-HOT PRESS(BONDING) (TMS)] = ISNULL(t.[TTL_S-HOT PRESS(BONDING) (TMS)], 0)
			, p.[TTL_S-HOT PRESS(HT) (TMS)] = ISNULL(t.[TTL_S-HOT PRESS(HT) (TMS)], 0)
			, p.[TTL_S-HOT PRESS(SEWING) (TMS)] = ISNULL(t.[TTL_S-HOT PRESS(SEWING) (TMS)], 0)
			, p.[TTL_SMALL HOT PRESS (TMS)] = ISNULL(t.[TTL_SMALL HOT PRESS (TMS)], 0)
			, p.[TTL_SMALL HOT PRESS-LONG (TMS)] = ISNULL(t.[TTL_SMALL HOT PRESS-LONG (TMS)], 0)
			, p.[TTL_SUBLIMATION PRINT (TMS)] = ISNULL(t.[TTL_SUBLIMATION PRINT (TMS)], 0)
			, p.[TTL_SUBLIMATION ROLLER (TMS)] = ISNULL(t.[TTL_SUBLIMATION ROLLER (TMS)], 0)
			, p.[TTL_SUBLIMATION SPRAY (TMS)] = ISNULL(t.[TTL_SUBLIMATION SPRAY (TMS)], 0)
			, p.[TTL_ULTRASONIC (TMS)] = ISNULL(t.[TTL_ULTRASONIC (TMS)], 0)
			, p.[TTL_ULTRASONIC MACHINE (TMS)] = ISNULL(t.[TTL_ULTRASONIC MACHINE (TMS)], 0)
			, p.[TTL_ULTRASONIC-ELASTIC (TMS)] = ISNULL(t.[TTL_ULTRASONIC-ELASTIC (TMS)], 0)
			, p.[TTL_ULTRASONIC-LABEL ASM (TMS)] = ISNULL(t.[TTL_ULTRASONIC-LABEL ASM (TMS)], 0)
			, p.[TTL_ULTRASONIC-TAPE CUT (TMS)] = ISNULL(t.[TTL_ULTRASONIC-TAPE CUT (TMS)], 0)
			, p.[TTL_VELCRO MACHINE (TMS)] = ISNULL(t.[TTL_VELCRO MACHINE (TMS)], 0)
			, p.[TTL_VELCROED M/C (TMS)] = ISNULL(t.[TTL_VELCROED M/C (TMS)], 0)
			, p.[TTL_WELDED (TMS)] = ISNULL(t.[TTL_WELDED (TMS)], 0)
			, p.[TTL_WELTED M/C (TMS)] = ISNULL(t.[TTL_WELTED M/C (TMS)], 0)
			, p.[TTL_ZIG ZAG (TMS)] = ISNULL(t.[TTL_ZIG ZAG (TMS)], 0)
			, p.[TTL_ZIPPER HOT PRESS (TMS)] = ISNULL(t.[TTL_ZIPPER HOT PRESS (TMS)], 0)
	from P_PPICMASTERLIST p 
	inner join #tmp_P_PPICMASTERLIST t on p.[SPNO] = t.[SPNO]


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
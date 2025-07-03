CREATE TABLE [dbo].[P_PPICMASTERLIST](
	[M] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Delivery] [date] NULL,
	[Delivery(YYYYMM)] [varchar](6) NOT NULL,
	[Earliest SCIDlv] [date] NULL,
	[SCIDlv] [date] NULL,
	[KEY] [varchar](6) NOT NULL,
	[IDD] [varchar](500) NOT NULL,
	[CRD] [date] NULL,
	[CRD(YYYYMM)] [varchar](6) NOT NULL,
	[Check CRD] [varchar](1) NOT NULL,
	[OrdCFM] [date] NULL,
	[CRD-OrdCFM] [int] NOT NULL,
	[SPNO] [varchar](13) NOT NULL,
	[Category] [varchar](20) NOT NULL,
	[Est. download date] [varchar](16) NOT NULL,
	[Buy Back] [varchar](1) NOT NULL,
	[Cancelled] [varchar](1) NOT NULL,
	[NeedProduction] [varchar](1) NOT NULL,
	[Dest] [varchar](2) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[Style Name] [nvarchar](50) NOT NULL,
	[Modular Parent] [varchar](20) NOT NULL,
	[CPUAdjusted] [numeric](38, 6) NOT NULL,
	[Similar Style] [varchar](max) NOT NULL,
	[Season] [varchar](10) NOT NULL,
	[Garment L/T] [numeric](3, 0) NOT NULL,
	[Order Type] [varchar](20) NOT NULL,
	[Project] [varchar](5) NOT NULL,
	[PackingMethod] [nvarchar](50) NOT NULL,
	[Hanger pack] [bit] NOT NULL,
	[Order#] [varchar](30) NOT NULL,
	[Buy Month] [varchar](16) NOT NULL,
	[PONO] [varchar](30) NOT NULL,
	[VAS/SHAS] [varchar](1) NOT NULL,
	[VAS/SHAS Apv.] [datetime] NULL,
	[VAS/SHAS Cut Off Date] [datetime] NULL,
	[M/Notice Date] [datetime] NULL,
	[Est M/Notice Apv.] [date] NULL,
	[Tissue] [varchar](1) NOT NULL,
	[AF by adidas] [varchar](1) NOT NULL,
	[Factory Disclaimer] [nvarchar](50) NOT NULL,
	[Factory Disclaimer Remark] [nvarchar](max) NOT NULL,
	[Approved/Rejected Date] [date] NULL,
	[Global Foundation Range] [varchar](1) NOT NULL,
	[Brand] [varchar](8) NOT NULL,
	[Cust CD] [varchar](16) NOT NULL,
	[KIT] [varchar](10) NOT NULL,
	[Fty Code] [varchar](10) NOT NULL,
	[Program] [nvarchar](12) NOT NULL,
	[Non Revenue] [varchar](1) NOT NULL,
	[New CD Code] [varchar](5) NOT NULL,
	[ProductType] [nvarchar](500) NOT NULL,
	[FabricType] [nvarchar](500) NOT NULL,
	[Lining] [varchar](20) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[Construction] [nvarchar](50) NOT NULL,
	[Cpu] [numeric](38, 6) NOT NULL,
	[Qty] [int] NOT NULL,
	[FOC Qty] [int] NOT NULL,
	[Total CPU] [numeric](38, 6) NOT NULL,
	[SewQtyTop] [int] NOT NULL,
	[SewQtyBottom] [int] NOT NULL,
	[SewQtyInner] [int] NOT NULL,
	[SewQtyOuter] [int] NOT NULL,
	[Total Sewing Output] [int] NOT NULL,
	[Cut Qty] [numeric](38, 6) NOT NULL,
	[By Comb] [varchar](1) NOT NULL,
	[Cutting Status] [varchar](1) NOT NULL,
	[Packing Qty] [int] NOT NULL,
	[Packing FOC Qty] [int] NOT NULL,
	[Booking Qty] [int] NOT NULL,
	[FOC Adj Qty] [int] NOT NULL,
	[Not FOC Adj Qty] [int] NOT NULL,
	[Total] [numeric](38, 6) NOT NULL,
	[KPI L/ETA] [date] NULL,
	[PF ETA (SP)] [date] NULL,
	[Pull Forward Remark] [varchar](max) NOT NULL,
	[Pack L/ETA] [date] NULL,
	[SCHD L/ETA] [date] NULL,
	[Actual Mtl. ETA] [date] NULL,
	[Fab ETA] [date] NULL,
	[Acc ETA] [date] NULL,
	[Sewing Mtl Complt(SP)] [varchar](1) NOT NULL,
	[Packing Mtl Complt(SP)] [varchar](1) NOT NULL,
	[Sew. MTL ETA (SP)] [date] NULL,
	[Pkg. MTL ETA (SP)] [date] NULL,
	[MTL Delay] [varchar](1) NOT NULL,
	[MTL Cmplt] [varchar](3) NULL,
	[MTL Cmplt (SP)] [varchar](1) NOT NULL,
	[Arrive W/H Date] [date] NULL,
	[Sewing InLine] [date] NULL,
	[Sewing OffLine] [date] NULL,
	[1st Sewn Date] [date] NULL,
	[Last Sewn Date] [date] NULL,
	[First Production Date] [date] NULL,
	[Last Production Date] [date] NULL,
	[Each Cons Apv Date] [datetime] NULL,
	[Est Each Con Apv.] [date] NULL,
	[Cutting InLine] [date] NULL,
	[Cutting OffLine] [date] NULL,
	[Cutting InLine(SP)] [date] NULL,
	[Cutting OffLine(SP)] [date] NULL,
	[1st Cut Date] [date] NULL,
	[Last Cut Date] [date] NULL,
	[Est. Pullout] [date] NULL,
	[Act. Pullout Date] [date] NULL,
	[Pullout Qty] [int] NOT NULL,
	[Act. Pullout Times] [int] NOT NULL,
	[Act. Pullout Cmplt] [varchar](2) NOT NULL,
	[KPI Delivery Date] [date] NULL,
	[Update Delivery Reason] [nvarchar](500) NOT NULL,
	[Plan Date] [date] NULL,
	[Original Buyer Delivery Date] [date] NULL,
	[SMR] [varchar](10) NOT NULL,
	[SMR Name] [varchar](50) NOT NULL,
	[Handle] [varchar](10) NOT NULL,
	[Handle Name] [varchar](50) NOT NULL,
	[Posmr] [varchar](10) NOT NULL,
	[Posmr Name] [varchar](50) NOT NULL,
	[PoHandle] [varchar](10) NOT NULL,
	[PoHandle Name] [varchar](50) NOT NULL,
	[PCHandle] [varchar](10) NOT NULL,
	[PCHandle Name] [varchar](50) NOT NULL,
	[MCHandle] [varchar](10) NOT NULL,
	[MCHandle Name] [varchar](50) NOT NULL,
	[DoxType] [varchar](8) NOT NULL,
	[Packing CTN] [int] NOT NULL,
	[TTLCTN] [int] NOT NULL,
	[Pack Error CTN] [int] NOT NULL,
	[FtyCTN] [int] NOT NULL,
	[cLog CTN] [int] NOT NULL,
	[CFA CTN] [int] NOT NULL,
	[cLog Rec. Date] [date] NULL,
	[Final Insp. Date] [varchar](500) NULL,
	[Insp. Result] [varchar](500) NOT NULL,
	[CFA Name] [varchar](500) NOT NULL,
	[Sewing Line#] [varchar](60) NOT NULL,
	[ShipMode] [varchar](30) NOT NULL,
	[SI#] [varchar](30) NOT NULL,
	[ColorWay] [nvarchar](max) NOT NULL,
	[Special Mark] [varchar](50) NOT NULL,
	[Fty Remark] [nvarchar](max) NOT NULL,
	[Sample Reason] [nvarchar](500) NOT NULL,
	[IS MixMarker] [varchar](25) NOT NULL,
	[Cutting SP] [varchar](13) NOT NULL,
	[Rainwear test] [varchar](1) NOT NULL,
	[TMS] [numeric](38, 6) NOT NULL,
	[MD room scan date] [datetime] NULL,
	[Dry Room received date] [datetime] NULL,
	[Dry room trans date] [datetime] NULL,
	[Last ctn trans date] [datetime] NULL,
	[Last ctn recvd date] [datetime] NULL,
	[OrganicCotton] [varchar](1) NOT NULL,
	[Direct Ship] [varchar](1) NOT NULL,
	[StyleCarryover] [varchar](1) NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[SCHDL/ETA(SP)] [date] NULL,
	[SewingMtlETA(SPexclRepl)] [date] NULL,
	[ActualMtlETA(exclRepl)] [date] NULL,
	[HalfKey] [varchar](8) NOT NULL,
	[DevSample] [varchar](1) NOT NULL,
	[POID] [varchar](13) NOT NULL,
	[KeepPanels] [varchar](1) NOT NULL,
	[BuyBackReason] [varchar](20) NOT NULL,
	[SewQtybyRate] [numeric](38, 6) NOT NULL,
	[Unit] [varchar](8) NOT NULL,
	[SubconInType] [varchar](100) NOT NULL,
	[Article] [varchar](500) NOT NULL,
	[ProduceRgPMS] [varchar](3) NOT NULL,
	[Country] [varchar](30) NOT NULL,
	[BuyerHalfKey] [varchar](8) NOT NULL,
	[Last scan and pack date] [datetime] NULL,
	[Third_Party_Insepction] [bit] NOT NULL,
	[ColorID] [nvarchar](max) NOT NULL,
	[FtyToClogTransit] [int] NOT NULL,
	[ClogToCFATansit] [int] NOT NULL,
	[CFAToClogTransit] [int] NOT NULL,
	[Shortage] [numeric](6, 0) NOT NULL,
	[Original CustPO] [varchar](30) NOT NULL,
	[Line Aggregator] [varchar](30) NOT NULL,
	[JokerTag] [bit] NOT NULL,
	[HeatSeal] [bit] NOT NULL,
	[CriticalStyle] [varchar](1) NOT NULL,
	[OrderCompanyID] [numeric](2, 0) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_PPICMASTERLIST] PRIMARY KEY CLUSTERED 
(
	[Ukey] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_M]  DEFAULT ('') FOR [M]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_DELIVERY(YYYYMM)]  DEFAULT ('') FOR [Delivery(YYYYMM)]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_KEY]  DEFAULT ('') FOR [KEY]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CRD(YYYYMM)]  DEFAULT ('') FOR [CRD(YYYYMM)]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CHECK CRD]  DEFAULT ('') FOR [Check CRD]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CRD-ORDCFM]  DEFAULT ((0)) FOR [CRD-OrdCFM]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SPNO]  DEFAULT ('') FOR [SPNO]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CATEGORY]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_EST. DOWNLOAD DATE]  DEFAULT ('') FOR [Est. download date]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_BUY BACK]  DEFAULT ('') FOR [Buy Back]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CANCELLED]  DEFAULT ('') FOR [Cancelled]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_NEEDPRODUCTION]  DEFAULT ('') FOR [NeedProduction]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_DEST]  DEFAULT ('') FOR [Dest]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_STYLE]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_STYLE NAME]  DEFAULT ('') FOR [Style Name]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_MODULAR PARENT]  DEFAULT ('') FOR [Modular Parent]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CPUADJUSTED]  DEFAULT ((0)) FOR [CPUAdjusted]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SIMILAR STYLE]  DEFAULT ('') FOR [Similar Style]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SEASON]  DEFAULT ('') FOR [Season]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_GARMENT L/T]  DEFAULT ((0)) FOR [Garment L/T]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_ORDER TYPE]  DEFAULT ('') FOR [Order Type]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PROJECT]  DEFAULT ('') FOR [Project]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PACKINGMETHOD]  DEFAULT ('') FOR [PackingMethod]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_HANGER PACK]  DEFAULT ((0)) FOR [Hanger pack]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_ORDER#]  DEFAULT ('') FOR [Order#]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_BUY MONTH]  DEFAULT ('') FOR [Buy Month]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PONO]  DEFAULT ('') FOR [PONO]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_VAS/SHAS]  DEFAULT ('') FOR [VAS/SHAS]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_TISSUE]  DEFAULT ('') FOR [Tissue]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_AF BY ADIDAS]  DEFAULT ('') FOR [AF by adidas]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_FACTORY DISCLAIMER]  DEFAULT ('') FOR [Factory Disclaimer]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_FACTORY DISCLAIMER REMARK]  DEFAULT ('') FOR [Factory Disclaimer Remark]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_GLOBAL FOUNDATION RANGE]  DEFAULT ('') FOR [Global Foundation Range]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_BRAND]  DEFAULT ('') FOR [Brand]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CUST CD]  DEFAULT ('') FOR [Cust CD]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_KIT]  DEFAULT ('') FOR [KIT]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_FTY CODE]  DEFAULT ('') FOR [Fty Code]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PROGRAM]  DEFAULT ('') FOR [Program]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_NON REVENUE]  DEFAULT ('') FOR [Non Revenue]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_NEW CD CODE]  DEFAULT ('') FOR [New CD Code]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PRODUCTTYPE]  DEFAULT ('') FOR [ProductType]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_FABRICTYPE]  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_LINING]  DEFAULT ('') FOR [Lining]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_GENDER]  DEFAULT ('') FOR [Gender]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CONSTRUCTION]  DEFAULT ('') FOR [Construction]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CPU]  DEFAULT ((0)) FOR [Cpu]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_QTY]  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_FOC QTY]  DEFAULT ((0)) FOR [FOC Qty]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_TOTAL CPU]  DEFAULT ((0)) FOR [Total CPU]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SEWQTYTOP]  DEFAULT ((0)) FOR [SewQtyTop]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SEWQTYBOTTOM]  DEFAULT ((0)) FOR [SewQtyBottom]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SEWQTYINNER]  DEFAULT ((0)) FOR [SewQtyInner]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SEWQTYOUTER]  DEFAULT ((0)) FOR [SewQtyOuter]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_TOTAL SEWING OUTPUT]  DEFAULT ((0)) FOR [Total Sewing Output]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CUT QTY]  DEFAULT ((0)) FOR [Cut Qty]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_BY COMB]  DEFAULT ('') FOR [By Comb]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CUTTING STATUS]  DEFAULT ('') FOR [Cutting Status]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PACKING QTY]  DEFAULT ((0)) FOR [Packing Qty]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PACKING FOC QTY]  DEFAULT ((0)) FOR [Packing FOC Qty]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_BOOKING QTY]  DEFAULT ((0)) FOR [Booking Qty]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_FOC ADJ QTY]  DEFAULT ((0)) FOR [FOC Adj Qty]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_NOT FOC ADJ QTY]  DEFAULT ((0)) FOR [Not FOC Adj Qty]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PULL FORWARD REMARK]  DEFAULT ('') FOR [Pull Forward Remark]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SEWING MTL COMPLT(SP)]  DEFAULT ('') FOR [Sewing Mtl Complt(SP)]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PACKING MTL COMPLT(SP)]  DEFAULT ('') FOR [Packing Mtl Complt(SP)]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_MTL DELAY]  DEFAULT ('') FOR [MTL Delay]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_MTL CMPLT]  DEFAULT ('') FOR [MTL Cmplt]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_MTL CMPLT (SP)]  DEFAULT ('') FOR [MTL Cmplt (SP)]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PULLOUT QTY]  DEFAULT ((0)) FOR [Pullout Qty]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_ACT. PULLOUT TIMES]  DEFAULT ((0)) FOR [Act. Pullout Times]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_ACT. PULLOUT CMPLT]  DEFAULT ('') FOR [Act. Pullout Cmplt]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_UPDATE DELIVERY REASON]  DEFAULT ('') FOR [Update Delivery Reason]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SMR]  DEFAULT ('') FOR [SMR]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SMR NAME]  DEFAULT ('') FOR [SMR Name]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_HANDLE]  DEFAULT ('') FOR [Handle]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_HANDLE NAME]  DEFAULT ('') FOR [Handle Name]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_POSMR]  DEFAULT ('') FOR [Posmr]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_POSMR NAME]  DEFAULT ('') FOR [Posmr Name]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_POHANDLE]  DEFAULT ('') FOR [PoHandle]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_POHANDLE NAME]  DEFAULT ('') FOR [PoHandle Name]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PCHANDLE]  DEFAULT ('') FOR [PCHandle]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PCHANDLE NAME]  DEFAULT ('') FOR [PCHandle Name]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_MCHANDLE]  DEFAULT ('') FOR [MCHandle]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_MCHANDLE NAME]  DEFAULT ('') FOR [MCHandle Name]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_DOXTYPE]  DEFAULT ('') FOR [DoxType]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PACKING CTN]  DEFAULT ((0)) FOR [Packing CTN]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_TTLCTN]  DEFAULT ((0)) FOR [TTLCTN]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_PACK ERROR CTN]  DEFAULT ((0)) FOR [Pack Error CTN]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_FTYCTN]  DEFAULT ((0)) FOR [FtyCTN]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CLOG CTN]  DEFAULT ((0)) FOR [cLog CTN]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CFA CTN]  DEFAULT ((0)) FOR [CFA CTN]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_INSP. RESULT]  DEFAULT ('') FOR [Insp. Result]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CFA NAME]  DEFAULT ('') FOR [CFA Name]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SEWING LINE#]  DEFAULT ('') FOR [Sewing Line#]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SHIPMODE]  DEFAULT ('') FOR [ShipMode]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SI#]  DEFAULT ('') FOR [SI#]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_COLORWAY]  DEFAULT ('') FOR [ColorWay]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SPECIAL MARK]  DEFAULT ('') FOR [Special Mark]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_FTY REMARK]  DEFAULT ('') FOR [Fty Remark]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SAMPLE REASON]  DEFAULT ('') FOR [Sample Reason]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_IS MIXMARKER]  DEFAULT ('') FOR [IS MixMarker]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CUTTING SP]  DEFAULT ('') FOR [Cutting SP]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_RAINWEAR TEST]  DEFAULT ('') FOR [Rainwear test]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_TMS]  DEFAULT ((0)) FOR [TMS]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_ORGANICCOTTON]  DEFAULT ('') FOR [OrganicCotton]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_DIRECT SHIP]  DEFAULT ('') FOR [Direct Ship]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_HalfKey]  DEFAULT ('') FOR [HalfKey]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_DevSample]  DEFAULT ('') FOR [DevSample]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_KeepPanels]  DEFAULT ('') FOR [KeepPanels]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_BuyBackReason]  DEFAULT ('') FOR [BuyBackReason]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SewQtybyRate]  DEFAULT ((0)) FOR [SewQtybyRate]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_Unit]  DEFAULT ('') FOR [Unit]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_SubconInType]  DEFAULT ('') FOR [SubconInType]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_ProduceRgPMS]  DEFAULT ('') FOR [ProduceRgPMS]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_Country]  DEFAULT ('') FOR [Country]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_BuyerHalfKey]  DEFAULT ('') FOR [BuyerHalfKey]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_Third_Party_Insepction]  DEFAULT ((0)) FOR [Third_Party_Insepction]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_ColorID]  DEFAULT ('') FOR [ColorID]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_FtyToClogTransit]  DEFAULT ((0)) FOR [FtyToClogTransit]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_ClogToCFATansit]  DEFAULT ((0)) FOR [ClogToCFATansit]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CFAToClogTransit]  DEFAULT ((0)) FOR [CFAToClogTransit]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMasterList_Shortage]  DEFAULT ((0)) FOR [Shortage]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_Original CustPO]  DEFAULT ('') FOR [Original CustPO]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_Line Aggregator]  DEFAULT ('') FOR [Line Aggregator]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_JokerTag]  DEFAULT ((0)) FOR [JokerTag]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_HeatSeal]  DEFAULT ((0)) FOR [HeatSeal]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_CriticalStyle]  DEFAULT ('') FOR [CriticalStyle]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  DEFAULT ((0)) FOR [OrderCompanyID]
GO

ALTER TABLE [dbo].[P_PPICMASTERLIST] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mdivision' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'M'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BuyerDelivery日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Delivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BuyerDelivery日期(yyyy/MM格式)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Delivery(YYYYMM)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單組合中最早SCI Delivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Earliest SCIDlv'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI Delivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SCIDlv'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'KEY'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Intended Delivery Date – 工廠預計出貨日，後續基本上會依照此日期安排出貨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'IDD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收貨日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CRD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收貨日(yyyy/mm)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CRD(YYYYMM)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢查船務是否到到齊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Check CRD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'業務確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'OrdCFM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'船務到齊日-業務確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CRD-OrdCFM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SPNO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Est. download date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Est. download date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為Buy Back' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Buy Back'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為取消訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cancelled'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單是否取消了仍需生產' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'NeedProduction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.Dest' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Dest'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Style Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Modular Parent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Modular Parent'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPUAdjusted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CPUAdjusted'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相似款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Similar Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新的Garment list' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Garment L/T'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Order Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ProjectID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Project'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PackingMethod'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單包裝是否懸掛' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Hanger pack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Order#'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Buy Month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PONO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'直客訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'VAS/SHAS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'mnoti_apv第二階段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'VAS/SHAS Apv.'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'直客訂單裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'VAS/SHAS Cut Off Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'製造單確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'M/Notice Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M Notice KPI Date (PMS only)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Est M/Notice Apv.'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'棉紙' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Tissue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否空運' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'AF by adidas'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠免責聲明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Factory Disclaimer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠免責聲明註解' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Factory Disclaimer Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'紀錄日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Approved/Rejected Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Global Foundation Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Global Foundation Range'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶資料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cust CD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UAWIP中會用到的CustCD對應Kit#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'KIT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Fty Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Program'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排除此訂單生產成本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Non Revenue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'New CD Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'New CD Code'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產品類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'襯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Lining'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Gender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Construction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單件耗用產能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cpu'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免費數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'FOC Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單CPU總計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Total CPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫數量-上衣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewQtyTop'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫數量-褲子' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewQtyBottom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫數量-內襯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewQtyInner'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫數量-外衣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewQtyOuter'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總車縫數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Total Sewing Output'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cut Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1= By Combination 2= by SP#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'By Comb'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cutting Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'非FOC的包裝數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Packing Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FOC的包裝數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Packing FOC Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'保留數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Booking Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FOC訂單調整數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'FOC Adj Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'非FOC訂單調整數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Not FOC Adj Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI L/ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'KPI L/ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P/F ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PF ETA (SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PF備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Pull Forward Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'14天LOCK的採購單交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SCHD L/ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Actual Mtl. ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料預計到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Fab ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'輔料預計到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Acc ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單車縫原料是否到齊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sewing Mtl Complt(SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單包裝原料是否到齊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Packing Mtl Complt(SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫原料預計到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sew. MTL ETA (SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝原料預計到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Pkg. MTL ETA (SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料是否延遲到貨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MTL Delay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料到貨狀態By母單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MTL Cmplt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料到貨狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MTL Cmplt (SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達倉庫日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Arrive W/H Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sewing InLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠下線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sewing OffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最早車縫日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'1st Sewn Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後車縫日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last Sewn Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'First production date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'First Production Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last production date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last Production Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Each-con 確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Each Cons Apv Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Each Cons. KPI Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Est Each Con Apv.'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cutting InLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪下線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cutting OffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'初次裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'1st Cut Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last Cut Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠出口日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Est. Pullout'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際出貨日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Act. Pullout Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總出貨數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Pullout Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨次數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Act. Pullout Times'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨完成狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Act. Pullout Cmplt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠KPI日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'KPI Delivery Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單日期更新原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Update Delivery Reason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plan Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Plan Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orig. buyer delivery date:' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Original Buyer Delivery Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組長名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SMR Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Handle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單Handle人員名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Handle Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購主管' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Posmr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購主管名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Posmr Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PoHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po Handle人員名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PoHandle Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排船表的PC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排船表的PC人員名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'PCHandle Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MC Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MC Handle人員名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MCHandle Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Form A' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'DoxType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Packing CTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bulk/Local訂單總箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'TTLCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝錯誤箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Pack Error CTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠端箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'FtyCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clog端箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'cLog CTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CFA箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CFA CTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clog最後接收日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'cLog Rec. Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Final Insp. Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Insp. Result'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後檢驗人員名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CFA Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sewing Line#'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運送方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'ShipMode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SI#'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'ColorWay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Special Mark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Special Mark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Planning 放置工廠備註用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Fty Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'樣品重製原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Sample Reason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單是否混馬克' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'IS MixMarker'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Cutting SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗測試結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Rainwear test'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time & Motion Study' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'TMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD room scan date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'MD room scan date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dry Room received date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Dry Room received date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dry room trans date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Dry room trans date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last ctn trans date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last ctn trans date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last ctn recvd date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last ctn recvd date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否使用有機棉' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'OrganicCotton'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否運送直達' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Direct Ship'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.Max_ScheETAbySP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SCHDL/ETA(SP)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.Sew_ScheETAnoReplace' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewingMtlETA(SPexclRepl)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.MaxShipETA_Exclude5x' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'ActualMtlETA(exclRepl)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI delivery-7 後分為上下半月' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'HalfKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OrderType.IsDevSample' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'DevSample'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.POID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.KeepPanels' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'KeepPanels'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.BuyBackReason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'BuyBackReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingOutput_Detail.QAQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SewQtybyRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.StyleUnit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Unit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orders.SubconInType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'SubconInType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order_Article.Article ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCIFty.ProduceRgCode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'ProduceRgPMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目的地國家' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Country'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Buyer delivery-7 後分為上下半月' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'BuyerHalfKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該訂單最後一箱scan and pack時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Last scan and pack date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單短交數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Shortage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自訂欄位4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Original CustPO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自訂欄位5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'Line Aggregator'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CriticalStyle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'CriticalStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
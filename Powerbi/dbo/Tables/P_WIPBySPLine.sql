CREATE TABLE [dbo].[P_WIPBySPLine](
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SewingLineID] [varchar](60) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[SCIDelivery] [date] NULL,
	[SewInLine] [date] NULL,
	[SewOffLine] [date] NULL,
	[IDD] [nvarchar](100) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[SPNO] [varchar](13) NOT NULL,
	[MasterSP] [varchar](13) NOT NULL,
	[IsBuyBack] [varchar](1) NOT NULL,
	[Cancelled] [varchar](1) NOT NULL,
	[CancelledStillNeedProd] [varchar](1) NOT NULL,
	[Dest] [varchar](30) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[OrderTypeID] [varchar](20) NOT NULL,
	[ShipMode] [varchar](30) NOT NULL,
	[PartialShipping] [varchar](1) NOT NULL,
	[OrderNo] [varchar](30) NOT NULL,
	[PONO] [varchar](30) NOT NULL,
	[ProgramID] [nvarchar](12) NOT NULL,
	[CDCodeID] [varchar](6) NOT NULL,
	[CDCodeNew] [varchar](5) NOT NULL,
	[ProductType] [nvarchar](100) NOT NULL,
	[FabricType] [nvarchar](100) NOT NULL,
	[Lining] [varchar](20) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[Construction] [nvarchar](100) NOT NULL,
	[KPILETA] [date] NULL,
	[SCHDLETA] [date] NULL,
	[ActMTLETA_MasterSP] [date] NULL,
	[SewMTLETA_SP] [date] NULL,
	[PkgMTLETA_SP] [date] NULL,
	[Cpu] [numeric](5, 3) NOT NULL,
	[TTLCPU] [numeric](15, 3) NOT NULL,
	[CPUClosed] [numeric](15, 3) NOT NULL,
	[CPUBal] [numeric](15, 3) NOT NULL,
	[Article] [nvarchar](500) NOT NULL,
	[Qty] [int] NOT NULL,
	[StandardOutput] [nvarchar](1000) NOT NULL,
	[OrigArtwork] [nvarchar](800) NOT NULL,
	[AddedArtwork] [nvarchar](800) NOT NULL,
	[BundleArtwork] [nvarchar](800) NOT NULL,
	[SubProcessDest] [nvarchar](900) NOT NULL,
	[EstCutDate] [date] NULL,
	[1stCutDate] [date] NULL,
	[CutQty] [numeric](14, 2) NOT NULL,
	[RFIDCutQty] [numeric](10, 0) NOT NULL,
	[RFIDSewingLineInQty] [numeric](6, 0) NOT NULL,
	[RFIDLoadingQty] [numeric](10, 0) NOT NULL,
	[RFIDEmbFarmInQty] [numeric](10, 0) NOT NULL,
	[RFIDEmbFarmOutQty] [numeric](10, 0) NOT NULL,
	[RFIDBondFarmInQty] [numeric](10, 0) NOT NULL,
	[RFIDBondFarmOutQty] [numeric](10, 0) NOT NULL,
	[RFIDPrintFarmInQty] [numeric](10, 0) NOT NULL,
	[RFIDPrintFarmOutQty] [numeric](10, 0) NOT NULL,
	[RFIDATFarmInQty] [numeric](10, 0) NOT NULL,
	[RFIDATFarmOutQty] [numeric](10, 0) NOT NULL,
	[RFIDPadPrintFarmInQty] [numeric](10, 0) NOT NULL,
	[RFIDPadPrintFarmOutQty] [numeric](10, 0) NOT NULL,
	[RFIDEmbossDebossFarmInQty] [numeric](10, 0) NOT NULL,
	[RFIDEmbossDebossFarmOutQty] [numeric](10, 0) NOT NULL,
	[RFIDHTFarmInQty] [numeric](10, 0) NOT NULL,
	[RFIDHTFarmOutQty] [numeric](10, 0) NOT NULL,
	[SubProcessStatus] [varchar](1) NOT NULL,
	[EmbQty] [numeric](10, 0) NOT NULL,
	[BondQty] [numeric](10, 0) NOT NULL,
	[PrintQty] [numeric](10, 0) NOT NULL,
	[SewQty] [numeric](10, 0) NOT NULL,
	[SewBal] [numeric](10, 0) NOT NULL,
	[1stSewDate] [date] NULL,
	[LastSewDate] [date] NULL,
	[AverageDailyOutput] [int] NOT NULL,
	[EstOfflinedate] [date] NULL,
	[ScannedQty] [int] NOT NULL,
	[PackedRate] [numeric](9, 4) NOT NULL,
	[TTLCTN] [int] NOT NULL,
	[FtyCTN] [int] NOT NULL,
	[cLogCTN] [int] NOT NULL,
	[CFACTN] [int] NOT NULL,
	[InspDate] [varchar](500) NOT NULL,
	[InspResult] [varchar](160) NOT NULL,
	[CFAName] [nvarchar](500) NOT NULL,
	[ActPulloutDate] [date] NULL,
	[KPIDeliveryDate] [datetime] NULL,
	[UpdateDeliveryReason] [nvarchar](800) NOT NULL,
	[PlanDate] [date] NULL,
	[SMR] [varchar](100) NOT NULL,
	[Handle] [varchar](100) NOT NULL,
	[Posmr] [varchar](100) NOT NULL,
	[PoHandle] [varchar](100) NOT NULL,
	[MCHandle] [varchar](100) NOT NULL,
	[doxtype] [varchar](8) NOT NULL,
	[SpecialMark] [varchar](50) NOT NULL,
	[GlobalFoundationRange] [bit] NOT NULL,
	[SampleReason] [varchar](5) NOT NULL,
	[TMS] [numeric](13, 3) NOT NULL,
	[UKey] [bigint] IDENTITY(1,1) NOT NULL,
	[RFIDAUTFarmInQty] [numeric](10, 0) NULL,
	[RFIDAUTFarmOutQty] [numeric](10, 0) NULL,
	[RFIDFMFarmInQty] [numeric](10, 0) NULL,
	[RFIDFMFarmOutQty] [numeric](10, 0) NULL,
 CONSTRAINT [PK_P_WIPBySPLine] PRIMARY KEY CLUSTERED 
(
	[UKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [IDD]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [SPNO]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [MasterSP]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [IsBuyBack]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [Cancelled]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [CancelledStillNeedProd]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [Dest]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [OrderTypeID]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [ShipMode]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [PartialShipping]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [OrderNo]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [PONO]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [ProgramID]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [CDCodeID]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [CDCodeNew]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [ProductType]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [Lining]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [Gender]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [Cpu]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [TTLCPU]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [CPUClosed]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [CPUBal]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [StandardOutput]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [OrigArtwork]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [AddedArtwork]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [BundleArtwork]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [SubProcessDest]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [CutQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDCutQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDSewingLineInQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDLoadingQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDEmbFarmInQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDEmbFarmOutQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDBondFarmInQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDBondFarmOutQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDPrintFarmInQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDPrintFarmOutQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDATFarmInQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDATFarmOutQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDPadPrintFarmInQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDPadPrintFarmOutQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDEmbossDebossFarmInQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDEmbossDebossFarmOutQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDHTFarmInQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [RFIDHTFarmOutQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [SubProcessStatus]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [EmbQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [BondQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [PrintQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [SewQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [SewBal]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [AverageDailyOutput]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [ScannedQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [PackedRate]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [TTLCTN]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [FtyCTN]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [cLogCTN]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [CFACTN]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [InspDate]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [InspResult]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [CFAName]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [UpdateDeliveryReason]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [SMR]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [Handle]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [Posmr]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [PoHandle]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [MCHandle]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [doxtype]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [SpecialMark]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [GlobalFoundationRange]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ('') FOR [SampleReason]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  DEFAULT ((0)) FOR [TMS]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  CONSTRAINT [DF_P_WIPBySPLine_RFIDAUTFarmInQty]  DEFAULT ((0)) FOR [RFIDAUTFarmInQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  CONSTRAINT [DF_P_WIPBySPLine_RFIDAUTFarmOutQty]  DEFAULT ((0)) FOR [RFIDAUTFarmOutQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  CONSTRAINT [DF_P_WIPBySPLine_RFIDFMFarmInQty]  DEFAULT ((0)) FOR [RFIDFMFarmInQty]
GO

ALTER TABLE [dbo].[P_WIPBySPLine] ADD  CONSTRAINT [DF_P_WIPBySPLine_RFIDFMFarmOutQty]  DEFAULT ((0)) FOR [RFIDFMFarmOutQty]
GO
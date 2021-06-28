CREATE TABLE [dbo].[PMS_Orders_History](
	[ID] [varchar](13) NOT NULL,
	[BrandID] [varchar](8) NULL,
	[ProgramID] [varchar](12) NULL,
	[StyleID] [varchar](15) NULL,
	[SeasonID] [varchar](10) NULL,
	[ProjectID] [varchar](5) NULL,
	[Category] [varchar](1) NULL,
	[OrderTypeID] [varchar](20) NULL,
	[BuyMonth] [varchar](16) NULL,
	[Dest] [varchar](2) NULL,
	[Model] [varchar](25) NULL,
	[HsCode1] [varchar](14) NULL,
	[HsCode2] [varchar](14) NULL,
	[PayTermARID] [varchar](10) NULL,
	[ShipTermID] [varchar](5) NULL,
	[ShipModeList] [varchar](30) NULL,
	[CdCodeID] [varchar](6) NULL,
	[CPU] [numeric](8, 3) NULL,
	[Qty] [int] NULL,
	[StyleUnit] [varchar](8) NULL,
	[PoPrice] [numeric](16, 3) NULL,
	[CFMPrice] [numeric](16, 3) NULL,
	[CurrencyID] [varchar](3) NULL,
	[Commission] [numeric](3, 2) NULL,
	[FactoryID] [varchar](8) NULL,
	[BrandAreaCode] [varchar](10) NULL,
	[BrandFTYCode] [varchar](10) NULL,
	[CTNQty] [smallint] NULL,
	[CustCDID] [varchar](16) NULL,
	[CustPONo] [varchar](30) NULL,
	[Customize1] [varchar](30) NULL,
	[Customize2] [varchar](30) NULL,
	[Customize3] [varchar](30) NULL,
	[CFMDate] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[SciDelivery] [date] NULL,
	[SewInLine] [date] NULL,
	[SewOffLine] [date] NULL,
	[CutInLine] [date] NULL,
	[CutOffLine] [date] NULL,
	[PulloutDate] [date] NULL,
	[CMPUnit] [varchar](8) NULL,
	[CMPPrice] [numeric](16, 4) NULL,
	[CMPQDate] [date] NULL,
	[CMPQRemark] [nvarchar](max) NULL,
	[EachConsApv] [datetime] NULL,
	[MnorderApv] [datetime] NULL,
	[CRDDate] [date] NULL,
	[InitialPlanDate] [date] NULL,
	[PlanDate] [date] NULL,
	[FirstProduction] [date] NULL,
	[FirstProductionLock] [date] NULL,
	[OrigBuyerDelivery] [date] NULL,
	[ExCountry] [date] NULL,
	[InDCDate] [date] NULL,
	[CFMShipment] [date] NULL,
	[PFETA] [date] NULL,
	[PackLETA] [date] NULL,
	[LETA] [date] NULL,
	[MRHandle] [varchar](10) NULL,
	[SMR] [varchar](10) NULL,
	[ScanAndPack] [bit] NULL,
	[VasShas] [bit] NULL,
	[SpecialCust] [bit] NULL,
	[TissuePaper] [bit] NULL,
	[Junk] [bit] NULL,
	[Packing] [nvarchar](max) NULL,
	[MarkFront] [nvarchar](max) NULL,
	[MarkBack] [nvarchar](max) NULL,
	[MarkLeft] [nvarchar](max) NULL,
	[MarkRight] [nvarchar](max) NULL,
	[Label] [nvarchar](max) NULL,
	[OrderRemark] [nvarchar](max) NULL,
	[ArtWorkCost] [varchar](1) NULL,
	[StdCost] [numeric](7, 2) NULL,
	[CtnType] [varchar](1) NULL,
	[FOCQty] [int] NULL,
	[SMnorderApv] [date] NULL,
	[FOC] [bit] NULL,
	[MnorderApv2] [datetime] NULL,
	[Packing2] [nvarchar](max) NULL,
	[SampleReason] [varchar](5) NULL,
	[RainwearTestPassed] [bit] NULL,
	[SizeRange] [nvarchar](max) NULL,
	[MTLComplete] [bit] NULL,
	[SpecialMark] [varchar](5) NULL,
	[OutstandingRemark] [nvarchar](max) NULL,
	[OutstandingInCharge] [varchar](10) NULL,
	[OutstandingDate] [datetime] NULL,
	[OutstandingReason] [varchar](5) NULL,
	[StyleUkey] [bigint] NULL,
	[POID] [varchar](13) NULL,
	[OrderComboID] [varchar](13) NULL,
	[IsNotRepeatOrMapping] [bit] NULL,
	[SplitOrderId] [varchar](13) NULL,
	[FtyKPI] [datetime] NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[SewLine] [varchar](60) NULL,
	[ActPulloutDate] [date] NULL,
	[ProdSchdRemark] [nvarchar](100) NULL,
	[IsForecast] [bit] NULL,
	[LocalOrder] [bit] NULL,
	[GMTClose] [date] NULL,
	[TotalCTN] [int] NULL,
	[ClogCTN] [int] NULL,
	[FtyCTN] [int] NULL,
	[PulloutComplete] [bit] NULL,
	[ReadyDate] [date] NULL,
	[PulloutCTNQty] [int] NULL,
	[Finished] [bit] NULL,
	[PFOrder] [bit] NULL,
	[SDPDate] [date] NULL,
	[InspDate] [date] NULL,
	[InspResult] [varchar](1) NULL,
	[InspHandle] [varchar](10) NULL,
	[KPILETA] [date] NULL,
	[MTLETA] [date] NULL,
	[SewETA] [date] NULL,
	[PackETA] [date] NULL,
	[MTLExport] [varchar](2) NULL,
	[DoxType] [varchar](8) NULL,
	[FtyGroup] [varchar](8) NULL,
	[MDivisionID] [varchar](8) NULL,
	[CutReadyDate] [date] NULL,
	[SewRemark] [nvarchar](60) NULL,
	[WhseClose] [date] NULL,
	[SubconInSisterFty] [bit] NULL,
	[MCHandle] [varchar](10) NULL,
	[LocalMR] [varchar](10) NULL,
	[KPIChangeReason] [varchar](5) NULL,
	[MDClose] [date] NULL,
	[MDEditName] [varchar](10) NULL,
	[MDEditDate] [datetime] NULL,
	[ClogLastReceiveDate] [date] NULL,
	[CPUFactor] [numeric](3, 1) NULL,
	[SizeUnit] [varchar](8) NULL,
	[CuttingSP] [varchar](13) NULL,
	[IsMixMarker] [int] NULL,
	[EachConsSource] [varchar](1) NULL,
	[KPIEachConsApprove] [date] NULL,
	[KPICmpq] [date] NULL,
	[KPIMNotice] [date] NULL,
	[GMTComplete] [varchar](1) NULL,
	[GFR] [bit] NULL,
	[CfaCTN] [int] NULL,
	[DRYCTN] [int] NOT NULL,
	[PackErrCTN] [int] NULL,
	[ForecastSampleGroup] [varchar](1) NULL,
	[DyeingLoss] [numeric](3, 0) NULL,
	[SubconInType] [varchar](1) NULL,
	[LastProductionDate] [date] NULL,
	[EstPODD] [date] NULL,
	[AirFreightByBrand] [bit] NULL,
	[AllowanceComboID] [varchar](13) NULL,
	[ChangeMemoDate] [date] NULL,
	[BuyBack] [varchar](20) NULL,
	[BuyBackOrderID] [varchar](13) NULL,
	[ForecastCategory] [varchar](1) NULL,
	[OnSiteSample] [bit] NULL,
	[PulloutCmplDate] [date] NULL,
	[NeedProduction] [bit] NULL,
	[IsBuyBack] [bit] NOT NULL,
	[KeepPanels] [bit] NULL,
	[BuyBackReason] [varchar](20) NOT NULL,
	[IsBuyBackCrossArticle] [bit] NOT NULL,
	[IsBuyBackCrossSizeCode] [bit] NOT NULL,
	[KpiEachConsCheck] [date] NULL,
	[NonRevenue] [bit] NOT NULL,
	[CAB] [varchar](10) NOT NULL,
	[FinalDest] [varchar](50) NOT NULL,
	[Customer_PO] [varchar](50) NOT NULL,
	[AFS_STOCK_CATEGORY] [varchar](50) NOT NULL,
	[CMPLTDATE] [date] NULL,
	[DelayCode] [varchar](4) NULL,
	[DelayDesc] [varchar](100) NULL,
	[HangerPack] [bit] NULL,
	[CDCodeNew] [varchar](5) NULL,
	[SizeUnitWeight] [varchar](8) NULL,
 CONSTRAINT [PK_PMS_Orders_History] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_ProgramID]  DEFAULT ('') FOR [ProgramID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_ProjectID]  DEFAULT ('') FOR [ProjectID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_OrderTypeID]  DEFAULT ('') FOR [OrderTypeID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_BuyMonth]  DEFAULT ('') FOR [BuyMonth]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Dest]  DEFAULT ('') FOR [Dest]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Model]  DEFAULT ('') FOR [Model]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_HsCode1]  DEFAULT ('') FOR [HsCode1]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_HsCode2]  DEFAULT ('') FOR [HsCode2]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_PayTermARID]  DEFAULT ('') FOR [PayTermARID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_ShipTermID]  DEFAULT ('') FOR [ShipTermID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_ShipModeList]  DEFAULT ('') FOR [ShipModeList]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CdCodeID]  DEFAULT ('') FOR [CdCodeID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CPU]  DEFAULT ((0)) FOR [CPU]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Qty]  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_StyleUnit]  DEFAULT ('') FOR [StyleUnit]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_PoPrice]  DEFAULT ((0)) FOR [PoPrice]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CFMPrice]  DEFAULT ((0)) FOR [CFMPrice]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CurrecnyID]  DEFAULT ('') FOR [CurrencyID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Commission]  DEFAULT ((0)) FOR [Commission]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_BrandAreaCode]  DEFAULT ('') FOR [BrandAreaCode]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_BrandFTYCode]  DEFAULT ('') FOR [BrandFTYCode]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CTNQty]  DEFAULT ((0)) FOR [CTNQty]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CustCDID]  DEFAULT ('') FOR [CustCDID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CustPONo]  DEFAULT ('') FOR [CustPONo]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Customize1]  DEFAULT ('') FOR [Customize1]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Customize2]  DEFAULT ('') FOR [Customize2]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Customize3]  DEFAULT ('') FOR [Customize3]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CMPUnit]  DEFAULT ('') FOR [CMPUnit]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CMPPrice]  DEFAULT ((0)) FOR [CMPPrice]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CMPQRemark]  DEFAULT ('') FOR [CMPQRemark]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_MRHandle]  DEFAULT ('') FOR [MRHandle]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SMR]  DEFAULT ('') FOR [SMR]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_ScanAndPack]  DEFAULT ((0)) FOR [ScanAndPack]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_VasShas]  DEFAULT ((0)) FOR [VasShas]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SpecialCust]  DEFAULT ((0)) FOR [SpecialCust]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_TissuePaper]  DEFAULT ((0)) FOR [TissuePaper]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Junk]  DEFAULT ((0)) FOR [Junk]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Packing]  DEFAULT ('') FOR [Packing]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_MarkFront]  DEFAULT ('') FOR [MarkFront]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_MarkBack]  DEFAULT ('') FOR [MarkBack]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_MarkLeft]  DEFAULT ('') FOR [MarkLeft]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_MarkRight]  DEFAULT ('') FOR [MarkRight]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Label]  DEFAULT ('') FOR [Label]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_OrderRemark]  DEFAULT ('') FOR [OrderRemark]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_ArtWorkCost]  DEFAULT ('') FOR [ArtWorkCost]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_StdCost]  DEFAULT ((0)) FOR [StdCost]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CtnType]  DEFAULT ('') FOR [CtnType]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_FOCQty]  DEFAULT ((0)) FOR [FOCQty]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_FOC]  DEFAULT ((0)) FOR [FOC]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Packing2]  DEFAULT ('') FOR [Packing2]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SampleReason]  DEFAULT ('') FOR [SampleReason]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_RainwearTestPassed]  DEFAULT ((0)) FOR [RainwearTestPassed]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SizeRange]  DEFAULT ('') FOR [SizeRange]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_MTLComplete]  DEFAULT ((0)) FOR [MTLComplete]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SpecialMark]  DEFAULT ('') FOR [SpecialMark]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_OutstandingRemark]  DEFAULT ('') FOR [OutstandingRemark]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_OutstandingInCharge]  DEFAULT ('') FOR [OutstandingInCharge]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_OutstandingReason]  DEFAULT ('') FOR [OutstandingReason]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_StyleUkey]  DEFAULT ((0)) FOR [StyleUkey]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_IsProPhet]  DEFAULT ((0)) FOR [IsNotRepeatOrMapping]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SplitOrderId]  DEFAULT ('') FOR [SplitOrderId]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SewLine]  DEFAULT ('') FOR [SewLine]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_ProdSchdRemark]  DEFAULT ('') FOR [ProdSchdRemark]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_IsForecast]  DEFAULT ((0)) FOR [IsForecast]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_LocalOrder]  DEFAULT ((0)) FOR [LocalOrder]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_TotalCTN]  DEFAULT ((0)) FOR [TotalCTN]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_ClogCTN]  DEFAULT ((0)) FOR [ClogCTN]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_FtyCTN]  DEFAULT ((0)) FOR [FtyCTN]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_PulloutComplete]  DEFAULT ((0)) FOR [PulloutComplete]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_PulloutCTNQty]  DEFAULT ((0)) FOR [PulloutCTNQty]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Finished]  DEFAULT ((0)) FOR [Finished]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_PFOrder]  DEFAULT ((0)) FOR [PFOrder]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_InspResult]  DEFAULT ('') FOR [InspResult]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_InspHandle]  DEFAULT ('') FOR [InspHandle]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_MTLExport]  DEFAULT ('') FOR [MTLExport]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_DoxType]  DEFAULT ('') FOR [DoxType]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_FtyGroup]  DEFAULT ('') FOR [FtyGroup]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SewRemark]  DEFAULT ('') FOR [SewRemark]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SubconInSisterFty]  DEFAULT ((0)) FOR [SubconInSisterFty]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_MCHandle]  DEFAULT ('') FOR [MCHandle]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_LocalMR]  DEFAULT ('') FOR [LocalMR]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_KPIChangeReason]  DEFAULT ('') FOR [KPIChangeReason]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_MDEditName]  DEFAULT ('') FOR [MDEditName]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_SizeUnit]  DEFAULT ('') FOR [SizeUnit]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CuttingSP]  DEFAULT ('') FOR [CuttingSP]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_IsMixMarker]  DEFAULT ((0)) FOR [IsMixMarker]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_GMTCompl]  DEFAULT ('') FOR [GMTComplete]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_GFR]  DEFAULT ((0)) FOR [GFR]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CfaCTN]  DEFAULT ((0)) FOR [CfaCTN]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_DRYCTN]  DEFAULT ((0)) FOR [DRYCTN]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_PackErrCTN]  DEFAULT ((0)) FOR [PackErrCTN]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_ForecastSampleGroup]  DEFAULT ('') FOR [ForecastSampleGroup]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  DEFAULT ((0)) FOR [DyeingLoss]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  DEFAULT ((0)) FOR [AirFreightByBrand]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  DEFAULT ('') FOR [ForecastCategory]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  DEFAULT ((0)) FOR [OnSiteSample]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  DEFAULT ((0)) FOR [NeedProduction]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  DEFAULT ((0)) FOR [IsBuyBack]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  DEFAULT ((0)) FOR [KeepPanels]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_BuyBackReason]  DEFAULT ('') FOR [BuyBackReason]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_IsBuyBackCrossArticle]  DEFAULT ((0)) FOR [IsBuyBackCrossArticle]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_IsBuyBackCrossSizeCode]  DEFAULT ((0)) FOR [IsBuyBackCrossSizeCode]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_NonRevenue]  DEFAULT ((0)) FOR [NonRevenue]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_CAB]  DEFAULT ('') FOR [CAB]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_FinalDest]  DEFAULT ('') FOR [FinalDest]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_Customer_PO]  DEFAULT ('') FOR [Customer_PO]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  CONSTRAINT [DF_PMS_Orders_History_AFS_STOCK_CATEGORY]  DEFAULT ('') FOR [AFS_STOCK_CATEGORY]
GO

ALTER TABLE [dbo].[PMS_Orders_History] ADD  DEFAULT ((0)) FOR [HangerPack]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ProgramID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'專案代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'OrderTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'BuyMonth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'進口國別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Dest'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'場域模式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Model'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中國海關HS編碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'HsCode1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中國海關HS編碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'HsCode2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'付款方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PayTermARID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交貨條件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ShipTermID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交貨方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ShipModeList'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CD#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CdCodeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單件耗用產能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'StyleUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平均單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PoPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'確認單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CFMPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'幣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CurrencyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'應付佣金%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Commission'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的區域代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'BrandAreaCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的工廠代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'BrandFTYCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每箱的包裝數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CTNQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶資料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CustCDID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CustPONo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Customize1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Customize2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人的自訂欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Customize3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CFMDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'飛雁交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SciDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SewInLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠下線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SewOffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪上線日　' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CutInLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪下線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CutOffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠出口日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PulloutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'cmp單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CMPUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'cmp單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CMPPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CMPQ的確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CMPQDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CMPQ上的備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CMPQRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Each-con 確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'EachConsApv'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'製造單確認日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MnorderApv'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CRD date.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CRDDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Initial Plan Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'InitialPlanDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Plan Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PlanDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'First production date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'FirstProduction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1st Production Lock' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'FirstProductionLock'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orig. buyer delivery date:' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'OrigBuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ex-Country Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ExCountry'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'In DC Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'InDCDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Confirm Shipment Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CFMShipment'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P/F ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PFETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包材的預計到貨日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PackLETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'14天LOCK的採購單交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'LETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MRHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Scan and Pack' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ScanAndPack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'VAS/SHAS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'VasShas'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Special customer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SpecialCust'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'棉紙' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'TissuePaper'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取消' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Packing'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨嘜頭(正面)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MarkFront'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨嘜頭(背面)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MarkBack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨嘜頭(左面)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MarkLeft'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨嘜頭(右面)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MarkRight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'圖片與商標位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Label'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'OrderRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工的展開方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ArtWorkCost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標準成本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'StdCost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝配比方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CtnType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免費數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'FOCQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMNotice Approved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SMnorderApv'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免費' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'FOC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'mnoti_apv第二階段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MnorderApv2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Packing第二階段資料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Packing2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SAMPLE REASON' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SampleReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗測式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'RainwearTestPassed'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸範圍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SizeRange'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料出清' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MTLComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Special Mark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SpecialMark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'延出備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'OutstandingRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'延出原因修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'OutstandingInCharge'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'延出原因修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'OutstandingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'延出備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'OutstandingReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式的唯一值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'StyleUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為非格子布或非Repeat或非Body Mapping' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'IsNotRepeatOrMapping'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拆單的原訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SplitOrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'FtyKPI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SewLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際出貨日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ActPulloutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Production Schedules Remark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ProdSchdRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預估單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'IsForecast'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠自行接單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'LocalOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garment 結單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'GMTClose'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單總箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'TotalCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'cLog 已收到箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ClogCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠已完成箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'FtyCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨出貨結清' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PulloutComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大貨Ready 的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ReadyDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pullout箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PulloutCTNQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shipment(Pull out)完成' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Finished'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pull forward 訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PFOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SDPDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成衣首次通過檢驗日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'InspDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CFA檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'InspResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CFA Finial檢驗人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'InspHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI L/ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'KPILETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MTLETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Mtl ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SewETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Packing Mtl ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PackETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料出貨結清' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MTLExport'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FormA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'DoxType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Factory Group' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'FtyGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Manufacturing Division ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cutting Ready Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CutReadyDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Remark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SewRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫關單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'WhseClose'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subcon In From Sister Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'SubconInSisterFty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MC Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Local MR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'LocalMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI Date變更理由' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'KPIChangeReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD Finished' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MDClose'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD 最後編輯人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MDEditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD 最後編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'MDEditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後收箱日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ClogLastReceiveDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為MixMarker ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'IsMixMarker'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Each Cons. KPI Date (PMS only)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'KPIEachConsApprove'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cmpq KPI Date (PMS only)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'KPICmpq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M Notice KPI Date (PMS only)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'KPIMNotice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garment Complete ( From Trade)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'GMTComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Global Foundation Range' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'GFR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CFA箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CfaCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'除溼室箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'DRYCTN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預估單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'ForecastCategory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PulloutComplete 最後的更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'PulloutCmplDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示可以跨Article領用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'IsBuyBackCrossArticle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示可以跨Size領用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'IsBuyBackCrossSizeCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Each Cons KPI檢查日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'KpiEachConsCheck'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排除此訂單生產成本，1:排除，0不排除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'NonRevenue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - CAB' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'CAB'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - FinalDest' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'FinalDest'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - Customer_PO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'Customer_PO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike - Mercury - AFS_STOCK_CATEGORY' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History', @level2type=N'COLUMN',@level2name=N'AFS_STOCK_CATEGORY'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PMS_Orders_History'
GO




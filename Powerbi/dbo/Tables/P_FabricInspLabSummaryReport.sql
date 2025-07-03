CREATE TABLE [dbo].[P_FabricInspLabSummaryReport](
	[Category] [nvarchar](100) NOT NULL,
	[POID] [varchar](13) NOT NULL,
	[SEQ] [varchar](6) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[Wkno] [varchar](13) NOT NULL,
	[InvNo] [varchar](25) NOT NULL,
	[CuttingDate] [date] NULL,
	[ArriveWHDate] [date] NULL,
	[ArriveQty] [int] NOT NULL,
	[Inventory] [int] NOT NULL,
	[Bulk] [int] NOT NULL,
	[BalanceQty] [int] NOT NULL,
	[TtlRollsCalculated] [int] NOT NULL,
	[BulkLocation] [varchar](5000) NOT NULL,
	[FirstUpdateBulkLocationDate] [datetime] NULL,
	[InventoryLocation] [varchar](5000) NOT NULL,
	[FirstUpdateStocksLocationDate] [datetime] NULL,
	[EarliestSCIDelivery] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[Refno] [varchar](36) NOT NULL,
	[Description] [nvarchar](150) NOT NULL,
	[Color] [varchar](50) NOT NULL,
	[ColorName] [nvarchar](150) NOT NULL,
	[SupplierCode] [varchar](6) NOT NULL,
	[SupplierName] [varchar](12) NOT NULL,
	[WeaveType] [varchar](20) NOT NULL,
	[NAPhysical] [varchar](1) NOT NULL,
	[InspectionOverallResult] [varchar](16) NOT NULL,
	[PhysicalInspResult] [varchar](10) NOT NULL,
	[TtlYrdsUnderBCGrade] [numeric](12, 2) NOT NULL,
	[TtlPointsUnderBCGrade] [numeric](9, 0) NOT NULL,
	[TtlPointsUnderAGrade] [numeric](9, 0) NOT NULL,
	[PhysicalInspector] [nvarchar](30) NOT NULL,
	[PhysicalInspDate] [datetime] NULL,
	[ActTtlYdsInspection] [numeric](10, 2) NOT NULL,
	[InspectionPCT] [numeric](6, 1) NOT NULL,
	[PhysicalInspDefectPoint] [int] NOT NULL,
	[CustInspNumber] [varchar](20) NOT NULL,
	[WeightTestResult] [varchar](5) NOT NULL,
	[WeightTestInspector] [nvarchar](30) NOT NULL,
	[WeightTestDate] [datetime] NULL,
	[CutShadebandQtyByRoll] [int] NOT NULL,
	[CutShadebandPCT] [numeric](5, 2) NOT NULL,
	[ShadeBondResult] [varchar](5) NOT NULL,
	[ShadeBondInspector] [nvarchar](30) NOT NULL,
	[ShadeBondDate] [datetime] NULL,
	[NoOfRollShadebandPass] [int] NOT NULL,
	[NoOfRollShadebandFail] [int] NOT NULL,
	[ContinuityResult] [varchar](5) NOT NULL,
	[ContinuityInspector] [nvarchar](30) NOT NULL,
	[ContinuityDate] [datetime] NULL,
	[OdorResult] [varchar](5) NOT NULL,
	[OdorInspector] [nvarchar](30) NOT NULL,
	[OdorDate] [datetime] NULL,
	[MoistureResult] [varchar](5) NOT NULL,
	[MoistureDate] [date] NULL,
	[CrockingShrinkageOverAllResult] [varchar](5) NOT NULL,
	[NACrocking] [varchar](1) NOT NULL,
	[CrockingResult] [varchar](5) NOT NULL,
	[CrockingInspector] [nvarchar](30) NOT NULL,
	[CrockingTestDate] [date] NULL,
	[NAHeatShrinkage] [varchar](1) NOT NULL,
	[HeatShrinkageTestResult] [varchar](5) NOT NULL,
	[HeatShrinkageInspector] [nvarchar](30) NOT NULL,
	[HeatShrinkageTestDate] [date] NULL,
	[NAWashShrinkage] [varchar](1) NOT NULL,
	[WashShrinkageTestResult] [varchar](5) NOT NULL,
	[WashShrinkageInspector] [nvarchar](30) NOT NULL,
	[WashShrinkageTestDate] [date] NULL,
	[OvenTestResult] [varchar](10) NOT NULL,
	[OvenTestInspector] [nvarchar](100) NOT NULL,
	[ColorFastnessResult] [varchar](10) NOT NULL,
	[ColorFastnessInspector] [nvarchar](100) NOT NULL,
	[LocalMR] [nvarchar](100) NOT NULL,
	[OrderType] [varchar](20) NOT NULL,
	[ReceivingID] [varchar](13) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditDate] [datetime] NULL,
	[StockType] [varchar](1) NOT NULL,
	[TotalYardageForInspection] [numeric](10, 2) NOT NULL,
	[ActualRemainingYardsForInspection] [numeric](10, 2) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
	[KPILETA] [date] NULL,
	[ACTETA] [date] NULL,
	[Packages] [int] NOT NULL,
	[SampleRcvDate] [date] NULL,
	[InspectionGroup] [varchar](5) NOT NULL,
	[CGradeTOP3Defects] [varchar](150) NOT NULL,
	[AGradeTOP3Defects] [varchar](150) NOT NULL,
	[ActTotalRollInspection] [int] NOT NULL,
	[TotalLotNumber] [int] NOT NULL,
	[InspectedLotNumber] [int] NOT NULL,
	[CutShadebandTime] [datetime] NULL,
	[OvenTestDate] [varchar](100) NOT NULL,
	[ColorFastnessTestDate] [varchar](100) NOT NULL,
	[MCHandle] [varchar](50) NOT NULL,
	[OrderQty] [varchar](30) NOT NULL,
	[Complete] [varchar](1) NOT NULL,
 CONSTRAINT [pk_P_FabricInspLabSummaryReporte] PRIMARY KEY CLUSTERED 
(
	[POID] ASC,
	[SEQ] ASC,
	[FactoryID] ASC,
	[StockType] ASC,
	[ReceivingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_SEQ]  DEFAULT ('') FOR [SEQ]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_Wkno]  DEFAULT ('') FOR [Wkno]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_InvNo]  DEFAULT ('') FOR [InvNo]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ArriveQty]  DEFAULT ((0)) FOR [ArriveQty]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_Inventory]  DEFAULT ((0)) FOR [Inventory]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_Bulk]  DEFAULT ((0)) FOR [Bulk]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_BalanceQty]  DEFAULT ((0)) FOR [BalanceQty]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlRollsCalculated]  DEFAULT ((0)) FOR [TtlRollsCalculated]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_BulkLocation]  DEFAULT ('') FOR [BulkLocation]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_InventoryLocation]  DEFAULT ('') FOR [InventoryLocation]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_Refno]  DEFAULT ('') FOR [Refno]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ColorName]  DEFAULT ('') FOR [ColorName]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_SupplierCode]  DEFAULT ('') FOR [SupplierCode]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_SupplierName]  DEFAULT ('') FOR [SupplierName]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_WeaveType]  DEFAULT ('') FOR [WeaveType]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_NAPhysical]  DEFAULT ('') FOR [NAPhysical]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_InspectionOverallResult]  DEFAULT ('') FOR [InspectionOverallResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_PhysicalInspResult]  DEFAULT ('') FOR [PhysicalInspResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlYrdsUnderBCGrade]  DEFAULT ((0)) FOR [TtlYrdsUnderBCGrade]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlPointsUnderBCGrade]  DEFAULT ((0)) FOR [TtlPointsUnderBCGrade]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_TtlPointsUnderAGrade]  DEFAULT ((0)) FOR [TtlPointsUnderAGrade]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_PhysicalInspector]  DEFAULT ('') FOR [PhysicalInspector]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ActTtlYdsInspection]  DEFAULT ((0)) FOR [ActTtlYdsInspection]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_InspectionPCT]  DEFAULT ((0)) FOR [InspectionPCT]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_PhysicalInspDefectPoint]  DEFAULT ((0)) FOR [PhysicalInspDefectPoint]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_CustInspNumber]  DEFAULT ('') FOR [CustInspNumber]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_WeightTestResult]  DEFAULT ('') FOR [WeightTestResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_WeightTestInspector]  DEFAULT ('') FOR [WeightTestInspector]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_CutShadebandQtyByRoll]  DEFAULT ((0)) FOR [CutShadebandQtyByRoll]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_CutShadebandPCT]  DEFAULT ((0)) FOR [CutShadebandPCT]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ShadeBondResult]  DEFAULT ('') FOR [ShadeBondResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ShadeBondInspector]  DEFAULT ('') FOR [ShadeBondInspector]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_NoOfRollShadebandPass]  DEFAULT ((0)) FOR [NoOfRollShadebandPass]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_NoOfRollShadebandFail]  DEFAULT ((0)) FOR [NoOfRollShadebandFail]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_[ContinuityResult]  DEFAULT ('') FOR [ContinuityResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ContinuityInspector]  DEFAULT ('') FOR [ContinuityInspector]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_OdorResult]  DEFAULT ('') FOR [OdorResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_[OdorInspector]  DEFAULT ('') FOR [OdorInspector]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_MoistureResult]  DEFAULT ('') FOR [MoistureResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_CrockingShrinkageOverAllResult]  DEFAULT ('') FOR [CrockingShrinkageOverAllResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_NACrocking]  DEFAULT ('') FOR [NACrocking]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_CrockingResult]  DEFAULT ('') FOR [CrockingResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_CrockingInspector]  DEFAULT ('') FOR [CrockingInspector]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_NAHeatShrinkage]  DEFAULT ('') FOR [NAHeatShrinkage]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_HeatShrinkageTestResult]  DEFAULT ('') FOR [HeatShrinkageTestResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_HeatShrinkageInspector]  DEFAULT ('') FOR [HeatShrinkageInspector]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_NAWashShrinkage]  DEFAULT ('') FOR [NAWashShrinkage]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_WashShrinkageTestResult]  DEFAULT ('') FOR [WashShrinkageTestResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_WashShrinkageInspector]  DEFAULT ('') FOR [WashShrinkageInspector]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_OvenTestResult]  DEFAULT ('') FOR [OvenTestResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_OvenTestInspector]  DEFAULT ('') FOR [OvenTestInspector]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ColorFastnessResult]  DEFAULT ('') FOR [ColorFastnessResult]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ColorFastnessInspector]  DEFAULT ('') FOR [ColorFastnessInspector]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_LocalMR]  DEFAULT ('') FOR [LocalMR]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_OrderType]  DEFAULT ('') FOR [OrderType]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [PK_P_FabricInspLabSummaryReport_ReceivingID]  DEFAULT ('') FOR [ReceivingID]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_StockType]  DEFAULT ('') FOR [StockType]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_TotalYardageForInspection]  DEFAULT ((0)) FOR [TotalYardageForInspection]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_ActualRemainingYardsForInspection]  DEFAULT ((0)) FOR [ActualRemainingYardsForInspection]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_Packages]  DEFAULT ((0)) FOR [Packages]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_InspectionGroup]  DEFAULT ('') FOR [InspectionGroup]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_CGradeTOP3Defects]  DEFAULT ('') FOR [CGradeTOP3Defects]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_AGradeTOP3Defects]  DEFAULT ('') FOR [AGradeTOP3Defects]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_ActTotalRollInspection]  DEFAULT ((0)) FOR [ActTotalRollInspection]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_TotalLotNumber]  DEFAULT ((0)) FOR [TotalLotNumber]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_InspectedLotNumber]  DEFAULT ((0)) FOR [InspectedLotNumber]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_OvenTestDate]  DEFAULT ('') FOR [OvenTestDate]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_ColorFastnessTestDate]  DEFAULT ('') FOR [ColorFastnessTestDate]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_MCHandle]  DEFAULT ('') FOR [MCHandle]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_FabricInspLabSummaryReport] ADD  CONSTRAINT [DF_P_FabricInspLabSummaryReport_Complete]  DEFAULT ('') FOR [Complete]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CategoryName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大項-小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Wkno' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Wkno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Invoice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InvNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CuttingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Arrive W/H Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ArriveWHDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Arrive Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ArriveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inventory Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Inventory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bulk Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Bulk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Balance Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BalanceQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total Rolls Calculated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'TtlRollsCalculated'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A倉儲位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BulkLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'FirstUpdateBulkLocationDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'B倉儲位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InventoryLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1st Update Stocks Location Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'FirstUpdateStocksLocationDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Earliest SCI Delivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'EarliestSCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Buyer Delivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商Refno' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fabric描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ColorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'SupplierCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'英文簡稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'SupplierName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WeaveType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否檢驗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NAPhysical'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InspectionOverallResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布瑕疵點檢驗Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'PhysicalInspResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GradeB&C帳面登記總碼長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'TtlYrdsUnderBCGrade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GradeB&C總瑕疵點數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'TtlPointsUnderBCGrade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GradeA總瑕疵點數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'TtlPointsUnderAGrade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Physical Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'PhysicalInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布瑕疵點檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'PhysicalInspDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際檢驗碼長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ActTtlYdsInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inspection%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InspectionPCT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總瑕疵點數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'PhysicalInspDefectPoint'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人檢驗系統中的檢驗報告單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CustInspNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重量檢驗Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WeightTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Weight Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WeightTestInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重量檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WeightTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cut Shadeband Qty(Roll)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CutShadebandQtyByRoll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'% Cut Shadeband' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CutShadebandPCT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ShadeBond  Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ShadeBondResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shadebone Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ShadeBondInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ShadeBond Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ShadeBondDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'No. of roll Shade band Pass' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NoOfRollShadebandPass'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'No. of roll Shade band Fail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NoOfRollShadebandFail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'漸進色Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ContinuityResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Continuity Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ContinuityInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'漸進色日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ContinuityDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'氣味Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OdorResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Odor Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OdorInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'氣味檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OdorDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'濕度檢測結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'MoistureResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'濕度檢測的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'MoistureDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CrockingShrinkageOverAllResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免測試色脫落' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NACrocking'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色脫落結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CrockingResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Crocking Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CrockingInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色脫落測試 日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CrockingTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'N/A Heat Shrinkage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NAHeatShrinkage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'熱壓縮律結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'HeatShrinkageTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Heat Shrinkage Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'HeatShrinkageInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'熱縮律測試日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'HeatShrinkageTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'N/A Wash Shrinkage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'NAWashShrinkage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Wash Shrinkage Test Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WashShrinkageTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Wash Shrinkage Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WashShrinkageInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗縮律測試日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'WashShrinkageTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Oven Test Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OvenTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Oven Test Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OvenTestInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Color Fastness Result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ColorFastnessResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Color Fastness Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ColorFastnessInspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Local MR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'LocalMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OrderType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ReceivingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AddDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EditDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Stock Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'StockType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI LETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'KPILETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ACT ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ACTETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裝箱數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Packages'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'試驗室收驗Sample日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'SampleRcvDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InspectionGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GradeC前三瑕疵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CGradeTOP3Defects'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GradeA前三瑕疵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'AGradeTOP3Defects'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際總檢驗卷數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ActTotalRollInspection'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總染缸數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'TotalLotNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗的染缸數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'InspectedLotNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁切色差布的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'CutShadebandTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實驗室烘箱測試時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OvenTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實驗室烘箱測試時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'ColorFastnessTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MCHandle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'MCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Complete' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspLabSummaryReport', @level2type=N'COLUMN',@level2name=N'Complete'
GO
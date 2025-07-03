CREATE TABLE [dbo].[P_AccessoryInspLabStatus](
	[POID] [varchar](13) NOT NULL,
	[SEQ] [varchar](6) NOT NULL,
	[Factory] [varchar](8) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[ShipModeID] [varchar](max) NOT NULL,
	[Wkno] [varchar](13) NOT NULL,
	[Invo] [varchar](25) NOT NULL,
	[ArriveWHDate] [date] NULL,
	[ArriveQty] [numeric](11, 2) NOT NULL,
	[Inventory] [numeric](11, 2) NOT NULL,
	[Bulk] [numeric](11, 2) NOT NULL,
	[BalanceQty] [numeric](11, 2) NOT NULL,
	[EarliestSCIDelivery] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[RefNo] [varchar](36) NOT NULL,
	[Article] [varchar](200) NOT NULL,
	[MaterialType] [varchar](20) NOT NULL,
	[Color] [varchar](50) NOT NULL,
	[ColorName] [nvarchar](150) NOT NULL,
	[Size] [varchar](50) NOT NULL,
	[Unit] [varchar](8) NOT NULL,
	[Supplier] [varchar](20) NOT NULL,
	[OrderQty] [numeric](11, 2) NOT NULL,
	[InspectionResult] [varchar](10) NOT NULL,
	[InspectedQty] [numeric](10, 2) NOT NULL,
	[RejectedQty] [numeric](10, 2) NOT NULL,
	[DefectType] [varchar](200) NOT NULL,
	[InspectionDate] [date] NULL,
	[Inspector] [nvarchar](30) NOT NULL,
	[Remark] [nvarchar](100) NOT NULL,
	[NALaboratory] [varchar](1) NOT NULL,
	[LaboratoryOverallResult] [varchar](5) NOT NULL,
	[NAOvenTest] [varchar](1) NOT NULL,
	[OvenTestResult] [varchar](5) NOT NULL,
	[OvenScale] [varchar](5) NOT NULL,
	[OvenTestDate] [date] NULL,
	[NAWashTest] [varchar](1) NOT NULL,
	[WashTestResult] [varchar](5) NOT NULL,
	[WashScale] [varchar](5) NOT NULL,
	[WashTestDate] [date] NULL,
	[ReceivingID] [varchar](13) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditDate] [datetime] NULL,
	[CategoryType] [varchar](30) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_AccessoryInspLabStatus] PRIMARY KEY CLUSTERED 
(
	[POID] ASC,
	[SEQ] ASC,
	[ReceivingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_SEQ]  DEFAULT ('') FOR [SEQ]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Factory]  DEFAULT ('') FOR [Factory]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_ShipModeID]  DEFAULT ('') FOR [ShipModeID]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Wkno]  DEFAULT ('') FOR [Wkno]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Invo]  DEFAULT ('') FOR [Invo]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_ArriveQty]  DEFAULT ((0)) FOR [ArriveQty]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Inventory]  DEFAULT ((0)) FOR [Inventory]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Bulk]  DEFAULT ((0)) FOR [Bulk]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_BalanceQty]  DEFAULT ((0)) FOR [BalanceQty]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_RefNo]  DEFAULT ('') FOR [RefNo]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_MaterialType]  DEFAULT ('') FOR [MaterialType]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_ColorName]  DEFAULT ('') FOR [ColorName]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Size]  DEFAULT ('') FOR [Size]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Unit]  DEFAULT ('') FOR [Unit]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Supplier]  DEFAULT ('') FOR [Supplier]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_InspectionResult]  DEFAULT ('') FOR [InspectionResult]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_InspectedQty]  DEFAULT ((0)) FOR [InspectedQty]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_RejectedQty]  DEFAULT ((0)) FOR [RejectedQty]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_DefectType]  DEFAULT ('') FOR [DefectType]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Inspector]  DEFAULT ('') FOR [Inspector]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_NALaboratory]  DEFAULT ('') FOR [NALaboratory]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_LaboratoryOverallResult]  DEFAULT ('') FOR [LaboratoryOverallResult]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_NAOvenTest]  DEFAULT ('') FOR [NAOvenTest]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_OvenTestResult]  DEFAULT ('') FOR [OvenTestResult]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_OvenScale]  DEFAULT ('') FOR [OvenScale]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_NAWashTest]  DEFAULT ('') FOR [NAWashTest]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_WashTestResult]  DEFAULT ('') FOR [WashTestResult]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_WashScale]  DEFAULT ('') FOR [WashScale]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_ReceivingID]  DEFAULT ('') FOR [ReceivingID]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_CategoryType]  DEFAULT ('') FOR [CategoryType]
GO

ALTER TABLE [dbo].[P_AccessoryInspLabStatus] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Factory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shipping mode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'ShipModeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作底稿編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Wkno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發票號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Invo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料日或單據日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'ArriveWHDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'庫存實收數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'ArriveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InvStock' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Inventory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BulkStock' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Bulk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BalanceQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'BalanceQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EarliestSCIDelivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'EarliestSCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BuyerDelivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Brand Refno' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'RefNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Article' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'MaterialType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SpecValue' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SpecValue' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Size'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Unit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商代碼-英文簡稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Supplier'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OrderQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'InspectionResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'InspectedQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返修數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'RejectedQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'有問題數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'DefectType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'烘箱結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'OvenTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'烘箱灰階' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'OvenScale'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'烘箱日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'OvenTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'不檢驗水洗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'NAWashTest'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'WashTestResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗灰階' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'WashScale'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'水洗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'WashTestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'ReceivingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Material Type的大類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'CategoryType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_AccessoryInspLabStatus', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
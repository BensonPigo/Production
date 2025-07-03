CREATE TABLE [dbo].[P_InventoryStockListReport](
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SewLine] [varchar](60) NOT NULL,
	[POID] [varchar](13) NOT NULL,
	[Category] [varchar](15) NOT NULL,
	[OrderTypeID] [varchar](20) NOT NULL,
	[WeaveTypeID] [varchar](20) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[OrigBuyerDelivery] [date] NULL,
	[MaterialComplete] [varchar](1) NOT NULL,
	[ETA] [date] NULL,
	[ArriveWHDate] [varchar](500) NOT NULL,
	[ExportID] [varchar](300) NOT NULL,
	[Packages] [varchar](100) NOT NULL,
	[ContainerNo] [varchar](300) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[ProjectID] [varchar](5) NOT NULL,
	[ProgramID] [nvarchar](12) NOT NULL,
	[SEQ1] [varchar](3) NOT NULL,
	[SEQ2] [varchar](2) NOT NULL,
	[MaterialType] [varchar](50) NOT NULL,
	[StockPOID] [varchar](13) NOT NULL,
	[StockSeq1] [varchar](3) NOT NULL,
	[StockSeq2] [varchar](2) NOT NULL,
	[Refno] [varchar](36) NOT NULL,
	[SCIRefno] [varchar](30) NOT NULL,
	[Description] [nvarchar](150) NOT NULL,
	[ColorID] [varchar](100) NOT NULL,
	[ColorName] [nvarchar](150) NOT NULL,
	[Size] [varchar](50) NOT NULL,
	[StockUnit] [varchar](8) NOT NULL,
	[PurchaseQty] [numeric](14, 2) NULL,
	[OrderQty] [int] NOT NULL,
	[ShipQty] [numeric](14, 2) NULL,
	[Roll] [varchar](8) NOT NULL,
	[Dyelot] [varchar](8) NOT NULL,
	[StockType] [varchar](15) NOT NULL,
	[InQty] [numeric](11, 2) NOT NULL,
	[OutQty] [numeric](11, 2) NOT NULL,
	[AdjustQty] [numeric](11, 2) NOT NULL,
	[ReturnQty] [numeric](11, 2) NOT NULL,
	[BalanceQty] [numeric](11, 2) NOT NULL,
	[MtlLocationID] [varchar](500) NOT NULL,
	[MCHandle] [varchar](100) NOT NULL,
	[POHandle] [varchar](100) NOT NULL,
	[POSMR] [varchar](100) NOT NULL,
	[Supplier] [varchar](50) NOT NULL,
	[VID] [varchar](200) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditDate] [datetime] NULL,
	[Grade] [varchar](10) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_InventoryStockListReport] PRIMARY KEY CLUSTERED 
(
	[POID] ASC,
	[SEQ1] ASC,
	[SEQ2] ASC,
	[Roll] ASC,
	[Dyelot] ASC,
	[StockType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_SewLine]  DEFAULT ('') FOR [SewLine]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_OrderTypeID]  DEFAULT ('') FOR [OrderTypeID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_WeaveTypeID]  DEFAULT ('') FOR [WeaveTypeID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_MaterialComplete]  DEFAULT ('') FOR [MaterialComplete]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_ArriveWHDate]  DEFAULT ('') FOR [ArriveWHDate]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_ExportID]  DEFAULT ('') FOR [ExportID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_Packages]  DEFAULT ('') FOR [Packages]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_ContainerNo]  DEFAULT ('') FOR [ContainerNo]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_ProjectID]  DEFAULT ('') FOR [ProjectID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_ProgramID]  DEFAULT ('') FOR [ProgramID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_SEQ1]  DEFAULT ('') FOR [SEQ1]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_SEQ2]  DEFAULT ('') FOR [SEQ2]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_MaterialType]  DEFAULT ('') FOR [MaterialType]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_StockPOID]  DEFAULT ('') FOR [StockPOID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_StockSeq1]  DEFAULT ('') FOR [StockSeq1]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_StockSeq2]  DEFAULT ('') FOR [StockSeq2]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_Refno]  DEFAULT ('') FOR [Refno]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_SCIRefno]  DEFAULT ('') FOR [SCIRefno]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_ColorID]  DEFAULT ('') FOR [ColorID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_ColorName]  DEFAULT ('') FOR [ColorName]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_Size]  DEFAULT ('') FOR [Size]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_StockUnit]  DEFAULT ('') FOR [StockUnit]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_PurchaseQty]  DEFAULT ((0)) FOR [PurchaseQty]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_ShipQty]  DEFAULT ((0)) FOR [ShipQty]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_Roll]  DEFAULT ('') FOR [Roll]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_Dyelot]  DEFAULT ('') FOR [Dyelot]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_StockType]  DEFAULT ('') FOR [StockType]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_InQty]  DEFAULT ((0)) FOR [InQty]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_OutQty]  DEFAULT ((0)) FOR [OutQty]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_AdjustQty]  DEFAULT ((0)) FOR [AdjustQty]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_ReturnQty]  DEFAULT ((0)) FOR [ReturnQty]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_BalanceQty]  DEFAULT ((0)) FOR [BalanceQty]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_MtlLocationID]  DEFAULT ('') FOR [MtlLocationID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_MCHandle]  DEFAULT ('') FOR [MCHandle]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_POHandle]  DEFAULT ('') FOR [POHandle]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_POSMR]  DEFAULT ('') FOR [POSMR]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_Supplier]  DEFAULT ('') FOR [Supplier]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_VID]  DEFAULT ('') FOR [VID]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_Grade]  DEFAULT ('') FOR [Grade]
GO

ALTER TABLE [dbo].[P_InventoryStockListReport] ADD  CONSTRAINT [DF_P_InventoryStockListReport_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Manufacturing Division ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'SewLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'OrderTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'WeaveTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orig. buyer delivery date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'OrigBuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'結清' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'MaterialComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫收料日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ArriveWHDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ExportID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總件/箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Packages'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'貨櫃種類+貨櫃編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ContainerNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'專案代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ProgramID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購大項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'SEQ1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'SEQ2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料型態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'MaterialType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用庫存SP#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StockPOID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用庫存SEQ1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StockSeq1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用庫存SEQ2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StockSeq2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'飛雁料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'SCIRefno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主料描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ColorID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ColorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Size'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StockUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'PurchaseQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ShipQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卷' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Roll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Dyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'StockType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'InQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發出量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'OutQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'調整量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'AdjustQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退回數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'ReturnQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料量+發出量-調整量+退回數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'BalanceQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'儲位編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'MtlLocationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'MCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'POHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購主管' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'POSMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商ID+Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'Supplier'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'含VID標的PO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'VID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
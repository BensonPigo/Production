CREATE TABLE [dbo].[P_MtltoFTYAnalysis](
	[Factory] [varchar](8) NOT NULL,
	[Country] [varchar](2) NOT NULL,
	[Brand] [varchar](8) NOT NULL,
	[WeaveType] [varchar](20) NOT NULL,
	[ETD] [date] NULL,
	[ETA] [date] NULL,
	[CloseDate] [date] NULL,
	[ActDate] [date] NULL,
	[Category] [nvarchar](50) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[Seq1] [varchar](3) NOT NULL,
	[Seq2] [varchar](2) NOT NULL,
	[OrderCfmDate] [date] NULL,
	[SciDelivery] [date] NULL,
	[Refno] [varchar](36) NOT NULL,
	[SCIRefno] [varchar](30) NOT NULL,
	[SuppID] [varchar](6) NOT NULL,
	[SuppName] [nvarchar](12) NOT NULL,
	[CurrencyID] [varchar](3) NOT NULL,
	[CurrencyRate] [numeric](2, 0) NOT NULL,
	[Price] [numeric](16, 4) NOT NULL,
	[Price(TWD)] [numeric](16, 4) NOT NULL,
	[Unit] [varchar](8) NOT NULL,
	[PoQty] [numeric](10, 2) NOT NULL,
	[PoFoc] [numeric](10, 2) NOT NULL,
	[ShipQty] [numeric](12, 2) NOT NULL,
	[ShipFoc] [numeric](12, 2) NOT NULL,
	[TTShipQty] [numeric](12, 2) NOT NULL,
	[ShipAmt(TWD)] [numeric](16, 4) NOT NULL,
	[FabricJunk] [varchar](1) NOT NULL,
	[WKID] [varchar](13) NOT NULL,
	[ShipmentTerm] [varchar](5) NOT NULL,
	[FabricType] [varchar](10) NOT NULL,
	[PINO] [varchar](25) NOT NULL,
	[PIDATE] [date] NULL,
	[Color] [varchar](6) NOT NULL,
	[ColorName] [nvarchar](150) NOT NULL,
	[Season] [varchar](10) NOT NULL,
	[PCHandle] [nvarchar](100) NOT NULL,
	[POHandle] [nvarchar](100) NOT NULL,
	[POSMR] [nvarchar](100) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[OrderType] [varchar](20) NOT NULL,
	[ShipModeID] [varchar](10) NOT NULL,
	[Supp1stCfmDate] [date] NULL,
	[BrandSuppCode] [varchar](6) NOT NULL,
	[BrandSuppName] [nvarchar](100) NOT NULL,
	[CountryofLoading] [varchar](30) NOT NULL,
	[SupdelRvsd] [date] NULL,
	[ProdItem] [varchar](20) NOT NULL,
	[KPILETA] [date] NULL,
	[MaterialConfirm] [varchar](1) NOT NULL,
	[SupplierGroup] [varchar](8) NOT NULL,
	[TransferBIDate] [date] NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
 [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_MtltoFTYAnalysis] PRIMARY KEY CLUSTERED 
(
	[WKID] ASC,
	[OrderID] ASC,
	[Seq1] ASC,
	[Seq2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Factory]  DEFAULT ('') FOR [Factory]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Country]  DEFAULT ('') FOR [Country]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Brand]  DEFAULT ('') FOR [Brand]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_WeaveType]  DEFAULT ('') FOR [WeaveType]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Seq1]  DEFAULT ('') FOR [Seq1]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Seq2]  DEFAULT ('') FOR [Seq2]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Refno]  DEFAULT ('') FOR [Refno]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_SCIRefno]  DEFAULT ('') FOR [SCIRefno]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_SuppID]  DEFAULT ('') FOR [SuppID]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_SuppName]  DEFAULT ('') FOR [SuppName]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_CurrencyID]  DEFAULT ('') FOR [CurrencyID]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_CurrencyRate]  DEFAULT ((0)) FOR [CurrencyRate]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Price]  DEFAULT ((0)) FOR [Price]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Price(TWD)]  DEFAULT ((0)) FOR [Price(TWD)]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Unit]  DEFAULT ('') FOR [Unit]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_PoQty]  DEFAULT ((0)) FOR [PoQty]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_PoFoc]  DEFAULT ((0)) FOR [PoFoc]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_ShipQty]  DEFAULT ((0)) FOR [ShipQty]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_ShipFoc]  DEFAULT ((0)) FOR [ShipFoc]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_TTShipQty]  DEFAULT ((0)) FOR [TTShipQty]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_ShipAmt(TWD)]  DEFAULT ((0)) FOR [ShipAmt(TWD)]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_FabricJunk]  DEFAULT ('') FOR [FabricJunk]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_WKID]  DEFAULT ('') FOR [WKID]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_ShipmentTerm]  DEFAULT ('') FOR [ShipmentTerm]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_FabricType]  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_PINO]  DEFAULT ('') FOR [PINO]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_ColorName]  DEFAULT ('') FOR [ColorName]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Season]  DEFAULT ('') FOR [Season]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_PCHandle]  DEFAULT ('') FOR [PCHandle]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_POHandle]  DEFAULT ('') FOR [POHandle]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_POSMR]  DEFAULT ('') FOR [POSMR]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_OrderType]  DEFAULT ('') FOR [OrderType]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_BrandSuppCode]  DEFAULT ('') FOR [BrandSuppCode]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_BrandSuppName]  DEFAULT ('') FOR [BrandSuppName]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_CountryofLoading]  DEFAULT ('') FOR [CountryofLoading]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_ProdItem]  DEFAULT ('') FOR [ProdItem]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_MaterialConfirm]  DEFAULT ('') FOR [MaterialConfirm]
GO

ALTER TABLE [dbo].[P_MtltoFTYAnalysis] ADD  CONSTRAINT [DF_P_MtltoFTYAnalysis_SupplierGroup]  DEFAULT ('') FOR [SupplierGroup]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裝船日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ETD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'結關日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'CloseDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達W/H日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ActDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Seq1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Seq2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'OrderCfmDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'飛雁交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'SciDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'SuppID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'SuppName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'幣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'CurrencyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'與台幣匯率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'CurrencyRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Price'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單價(TWD)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Price(TWD)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Unit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'PoQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免費數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'PoFoc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本次出口數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ShipQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本次出口FOC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ShipFoc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ShipQty+ShipFoc' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'TTShipQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運費(TWD)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ShipAmt(TWD)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料Junk與否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'FabricJunk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商的三聯式發票號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'PINO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收到廠商三聯式發票號碼的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'PIDATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ColorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'OrderType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌供應商代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'BrandSuppCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌供應商名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'BrandSuppName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RevisedETD' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'SupdelRvsd'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產分類(用於Pull forward)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtltoFTYAnalysis', @level2type=N'COLUMN',@level2name=N'ProdItem'
GO



EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' 記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtltoFTYAnalysis',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' 時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtltoFTYAnalysis',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'
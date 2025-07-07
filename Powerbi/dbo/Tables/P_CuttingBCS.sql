CREATE TABLE [dbo].[P_CuttingBCS](
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SeasonID] [varchar](15) NOT NULL,
	[CDCodeNew] [varchar](5) NOT NULL,
	[FabricType] [varchar](5) NOT NULL,
	[POID] [varchar](15) NOT NULL,
	[Category] [varchar](10) NOT NULL,
	[WorkType] [varchar](11) NOT NULL,
	[MatchFabric] [varchar](100) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[SciDelivery] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[OrderQty] [int] NOT NULL,
	[SewInLineDate] [date] NULL,
	[SewOffLineDate] [date] NULL,
	[SewingLineID] [varchar](5) NOT NULL,
	[RequestDate] [date] NOT NULL,
	[StdQty] [int] NOT NULL,
	[StdQtyByLine] [int] NOT NULL,
	[AccuStdQty] [int] NOT NULL,
	[AccuStdQtyByLine] [int] NOT NULL,
	[AccuEstCutQty] [int] NOT NULL,
	[AccuEstCutQtyByLine] [int] NOT NULL,
	[SupplyCutQty] [int] NOT NULL,
	[SupplyCutQtyByLine] [int] NOT NULL,
	[BalanceCutQty] [int] NOT NULL,
	[BalanceCutQtyByLine] [int] NOT NULL,
	[SupplyCutQtyVSStdQty] [int] NOT NULL,
	[SupplyCutQtyVSStdQtyByLine] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_CuttingBCS] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[SewingLineID] ASC,
	[RequestDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_CDCodeNew]  DEFAULT ('') FOR [CDCodeNew]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_FabricType]  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_WorkType]  DEFAULT ('') FOR [WorkType]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_MatchFabric]  DEFAULT ('') FOR [MatchFabric]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_SewingLineID]  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_StdQty]  DEFAULT ((0)) FOR [StdQty]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_StdQtyByLine]  DEFAULT ((0)) FOR [StdQtyByLine]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_AccuStdQty]  DEFAULT ((0)) FOR [AccuStdQty]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_AccuStdQtyByLine]  DEFAULT ((0)) FOR [AccuStdQtyByLine]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_AccuEstCutQty]  DEFAULT ((0)) FOR [AccuEstCutQty]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_AccuEstCutQtyByLine]  DEFAULT ((0)) FOR [AccuEstCutQtyByLine]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_SupplyCutQty]  DEFAULT ((0)) FOR [SupplyCutQty]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_SupplyCutQtyByLine]  DEFAULT ((0)) FOR [SupplyCutQtyByLine]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_BalanceCutQty]  DEFAULT ((0)) FOR [BalanceCutQty]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_BalanceCutQtyByLine]  DEFAULT ((0)) FOR [BalanceCutQtyByLine]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_SupplyCutQtyVSStdQty]  DEFAULT ((0)) FOR [SupplyCutQtyVSStdQty]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_SupplyCutQtyVSStdQtyByLine]  DEFAULT ((0)) FOR [SupplyCutQtyVSStdQtyByLine]
GO

ALTER TABLE [dbo].[P_CuttingBCS] ADD  CONSTRAINT [DF_P_CuttingBCS_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'報產出的M' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'報產出的工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'母單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工單轉置方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'WorkType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'特殊body對格對條的布種' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'MatchFabric'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SciDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計的SewingDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SewInLineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計的Sewing offline Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SewOffLineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SewingLineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計的SewingDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'RequestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該天的標準產出' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'StdQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該天該產線的的標準產出' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'StdQtyByLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'累計std qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'AccuStdQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'累計std qty by line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'AccuStdQtyByLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪量，用來與Accu Std qty比對' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'AccuEstCutQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪量，用來與Accu Std qty by line比對' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'AccuEstCutQtyByLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預期裁剪供給量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SupplyCutQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預期裁剪供給量 by line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SupplyCutQtyByLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'如果預期裁剪供給量大於StdQty則帶入Stdqty，反之則帶入預期裁剪供給量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SupplyCutQtyVSStdQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'如果預期裁剪供給量大於StdQty則帶入Stdqty，反之則帶入預期裁剪供給量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SupplyCutQtyVSStdQtyByLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
CREATE TABLE [dbo].[P_ProdEffAnalysis](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[Month] [date] NULL,
	[ArtworkType] [varchar](30) NOT NULL,
	[Program] [nvarchar](12) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[FtyZone] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Brand] [varchar](8) NOT NULL,
	[NewCDCode] [varchar](5) NOT NULL,
	[ProductType] [varchar](30) NOT NULL,
	[FabricType] [varchar](30) NOT NULL,
	[Lining] [varchar](20) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[Construction] [varchar](30) NOT NULL,
	[StyleDescription] [varchar](100) NOT NULL,
	[Season] [varchar](10) NOT NULL,
	[TotalQty] [numeric](15, 2) NOT NULL,
	[TotalCPU] [numeric](19, 4) NOT NULL,
	[TotalManHours] [varchar](15) NOT NULL,
	[PPH] [varchar](15) NOT NULL,
	[EFF] [varchar](15) NOT NULL,
	[Remark] [varchar](500) NOT NULL,
 [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] NCHAR(10) NULL, 
    CONSTRAINT [PK_P_ProdEffAnalysis] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_ArtworkType]  DEFAULT ('') FOR [ArtworkType]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_Program]  DEFAULT ('') FOR [Program]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_FtyZone]  DEFAULT ('') FOR [FtyZone]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_Brand]  DEFAULT ('') FOR [Brand]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_NewCDCode]  DEFAULT ('') FOR [NewCDCode]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_ProductType]  DEFAULT ('') FOR [ProductType]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_FabricType]  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_Lining]  DEFAULT ('') FOR [Lining]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_Gender]  DEFAULT ('') FOR [Gender]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_Construction]  DEFAULT ('') FOR [Construction]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_StyleDescription]  DEFAULT ('') FOR [StyleDescription]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_Season]  DEFAULT ('') FOR [Season]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_TotalQty]  DEFAULT ((0)) FOR [TotalQty]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_TotalCPU]  DEFAULT ((0)) FOR [TotalCPU]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_TotalManHours]  DEFAULT ('') FOR [TotalManHours]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_PPH]  DEFAULT ('') FOR [PPH]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_EFF]  DEFAULT ('') FOR [EFF]
GO

ALTER TABLE [dbo].[P_ProdEffAnalysis] ADD  CONSTRAINT [DF_P_ProdEffAnalysis_Remark]  DEFAULT ('') FOR [Remark]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計算月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'ArtworkType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Program'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠區域' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'FtyZone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新CDCode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'NewCDCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成衣種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料總累' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'襯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Lining'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Gender'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'構造' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Construction'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'StyleDescription'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'TotalQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'TotalCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總人工時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'TotalManHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PPH' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'PPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EFF' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'EFF'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEffAnalysis', @level2type=N'COLUMN',@level2name=N'Remark'
GO



EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ProdEffAnalysis',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N' 時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ProdEffAnalysis',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'
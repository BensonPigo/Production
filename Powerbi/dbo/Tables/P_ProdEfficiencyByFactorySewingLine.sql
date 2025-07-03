CREATE TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine](
	[Year-Month] [date] NOT NULL,
	[FtyZone] [varchar](8) NOT NULL,
	[Factory] [varchar](8) NOT NULL,
	[Line] [varchar](5) NOT NULL,
	[TotalQty] [int] NOT NULL,
	[TotalCPU] [numeric](15, 3) NOT NULL,
	[TotalManhours] [numeric](15, 3) NOT NULL,
	[PPH] [numeric](10, 2) NOT NULL,
	[EFF] [numeric](10, 2) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_ProdEfficiencyByFactorySewingLine] PRIMARY KEY CLUSTERED 
(
	[Year-Month] ASC,
	[FtyZone] ASC,
	[Factory] ASC,
	[Line] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine] ADD  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_TotalQty]  DEFAULT ((0)) FOR [TotalQty]
GO

ALTER TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine] ADD  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_TotalCPU]  DEFAULT ((0)) FOR [TotalCPU]
GO

ALTER TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine] ADD  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_TotalManhours]  DEFAULT ((0)) FOR [TotalManhours]
GO

ALTER TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine] ADD  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_PPH]  DEFAULT ((0)) FOR [PPH]
GO

ALTER TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine] ADD  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_EFF]  DEFAULT ((0)) FOR [EFF]
GO

ALTER TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine] ADD  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingOutput當月月底日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'Year-Month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty Zone' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'FtyZone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'Factory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Line ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總產出數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'TotalQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總產出數量的CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'TotalCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總工時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'TotalManhours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PPH' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'PPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EFF' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'EFF'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdEfficiencyByFactorySewingLine', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
CREATE TABLE [dbo].[P_CuttingOutputStatisitc](
	[TransferDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[CutRateByDate] [numeric](5, 2) NOT NULL,
	[CutRateByMonth] [numeric](5, 2) NOT NULL,
	[CutOutputByDate] [numeric](10, 4) NOT NULL,
	[CutOutputIn7Days] [numeric](10, 4) NOT NULL,
	[CutDelayIn7Days] [numeric](10, 4) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransferDate] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_CuttingOutputStatisitc] ADD  CONSTRAINT [DF_P_CuttingOutputStatisitc_TransferDate]  DEFAULT ('') FOR [TransferDate]
GO

ALTER TABLE [dbo].[P_CuttingOutputStatisitc] ADD  CONSTRAINT [DF_P_CuttingOutputStatisitc_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_CuttingOutputStatisitc] ADD  CONSTRAINT [DF_P_CuttingOutputStatisitc_CutRateByDate]  DEFAULT ((0)) FOR [CutRateByDate]
GO

ALTER TABLE [dbo].[P_CuttingOutputStatisitc] ADD  CONSTRAINT [DF_P_CuttingOutputStatisitc_CutRateByMonth]  DEFAULT ((0)) FOR [CutRateByMonth]
GO

ALTER TABLE [dbo].[P_CuttingOutputStatisitc] ADD  CONSTRAINT [DF_P_CuttingOutputStatisitc_CutOutputByDate]  DEFAULT ((0)) FOR [CutOutputByDate]
GO

ALTER TABLE [dbo].[P_CuttingOutputStatisitc] ADD  CONSTRAINT [DF_P_CuttingOutputStatisitc_CutOutputIn7Days]  DEFAULT ((0)) FOR [CutOutputIn7Days]
GO

ALTER TABLE [dbo].[P_CuttingOutputStatisitc] ADD  CONSTRAINT [DF_P_CuttingOutputStatisitc_CutDelayIn7Days]  DEFAULT ((0)) FOR [CutDelayIn7Days]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'轉換日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatisitc', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatisitc', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rate計算BY天' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatisitc', @level2type=N'COLUMN',@level2name=N'CutRateByDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rate計算BY月' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatisitc', @level2type=N'COLUMN',@level2name=N'CutRateByMonth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Output計算BY天' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatisitc', @level2type=N'COLUMN',@level2name=N'CutOutputByDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Output計算BY周' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatisitc', @level2type=N'COLUMN',@level2name=N'CutOutputIn7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Delay計算BY周' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingOutputStatisitc', @level2type=N'COLUMN',@level2name=N'CutDelayIn7Days'
GO



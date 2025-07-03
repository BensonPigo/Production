CREATE TABLE [dbo].[P_SubProInsReportMonthlyRate](
	[Month] [int] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SubprocessRate] [numeric](5, 2) NOT NULL,
	[TotalPassQty] [int] NOT NULL,
	[TotalQty] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_SubProInsReportMonthlyRate] PRIMARY KEY CLUSTERED 
(
	[Month] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_SubProInsReportMonthlyRate] ADD  CONSTRAINT [DF_P_SubProInsReportMonthlyRate_Month]  DEFAULT ((0)) FOR [Month]
GO

ALTER TABLE [dbo].[P_SubProInsReportMonthlyRate] ADD  CONSTRAINT [DF_P_SubProInsReportMonthlyRate_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_SubProInsReportMonthlyRate] ADD  CONSTRAINT [DF_P_SubProInsReportMonthlyRate_SubprocessRate]  DEFAULT ((0)) FOR [SubprocessRate]
GO

ALTER TABLE [dbo].[P_SubProInsReportMonthlyRate] ADD  CONSTRAINT [DF_P_SubProInsReportMonthlyRate_TotalPassQty]  DEFAULT ((0)) FOR [TotalPassQty]
GO

ALTER TABLE [dbo].[P_SubProInsReportMonthlyRate] ADD  CONSTRAINT [DF_P_SubProInsReportMonthlyRate_TotalQty]  DEFAULT ((0)) FOR [TotalQty]
GO

ALTER TABLE [dbo].[P_SubProInsReportMonthlyRate] ADD  CONSTRAINT [DF_P_SubProInsReportMonthlyRate_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'Month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SubprocessRate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'SubprocessRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上個月Pass的綁包總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'TotalPassQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上個月的綁包總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'TotalQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportMonthlyRate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
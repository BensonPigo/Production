CREATE TABLE [dbo].[P_SubProInsReportDailyRate](
	[InspectionDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SubprocessRate] [numeric](5, 2) NOT NULL,
	[TotalPassQty] [int] NOT NULL,
	[TotalQty] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_SubProInsReportDailyRate] PRIMARY KEY CLUSTERED 
(
	[InspectionDate] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_SubProInsReportDailyRate] ADD  CONSTRAINT [DF_P_SubProInsReportDailyRate_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_SubProInsReportDailyRate] ADD  CONSTRAINT [DF_P_SubProInsReportDailyRate_SubprocessRate]  DEFAULT ((0)) FOR [SubprocessRate]
GO

ALTER TABLE [dbo].[P_SubProInsReportDailyRate] ADD  CONSTRAINT [DF_P_SubProInsReportDailyRate_TotalPassQty]  DEFAULT ((0)) FOR [TotalPassQty]
GO

ALTER TABLE [dbo].[P_SubProInsReportDailyRate] ADD  CONSTRAINT [DF_P_SubProInsReportDailyRate_TotalQty]  DEFAULT ((0)) FOR [TotalQty]
GO

ALTER TABLE [dbo].[P_SubProInsReportDailyRate] ADD  CONSTRAINT [DF_P_SubProInsReportDailyRate_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InspectionDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'InspectionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SubprocessRate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'SubprocessRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InspectionDate當天Pass的綁包總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'TotalPassQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InspectionDate當天綁包總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'TotalQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReportDailyRate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
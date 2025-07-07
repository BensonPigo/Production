CREATE TABLE [dbo].[P_MaterialCompletionRateByWeek](
	[Year] [int] NOT NULL,
	[WeekNo] [int] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[MaterialCompletionRate] [numeric](5, 2) NOT NULL,
	[MTLCMP_SPNo] [int] NOT NULL,
	[TTLSPNo] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_MaterialCompletionRateByWeek] PRIMARY KEY CLUSTERED 
(
	[Year] ASC,
	[WeekNo] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_MaterialCompletionRateByWeek] ADD  CONSTRAINT [DF_P_CFAMasterListRelatedrate_Year]  DEFAULT ((0)) FOR [Year]
GO

ALTER TABLE [dbo].[P_MaterialCompletionRateByWeek] ADD  CONSTRAINT [DF_P_MaterialCompletionRateByWeek_WeekNo]  DEFAULT ((0)) FOR [WeekNo]
GO

ALTER TABLE [dbo].[P_MaterialCompletionRateByWeek] ADD  CONSTRAINT [DF_P_MaterialCompletionRateByWeek_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_MaterialCompletionRateByWeek] ADD  CONSTRAINT [DF_P_MaterialCompletionRateByWeek_MaterialCompletionRate]  DEFAULT ((0)) FOR [MaterialCompletionRate]
GO

ALTER TABLE [dbo].[P_MaterialCompletionRateByWeek] ADD  CONSTRAINT [DF_P_CFAMasterListRelatedrate_MTLCMP_SPNo]  DEFAULT ((0)) FOR [MTLCMP_SPNo]
GO

ALTER TABLE [dbo].[P_MaterialCompletionRateByWeek] ADD  CONSTRAINT [DF_P_MaterialCompletionRateByWeek_TTLSPNo]  DEFAULT ((0)) FOR [TTLSPNo]
GO

ALTER TABLE [dbo].[P_MaterialCompletionRateByWeek] ADD  CONSTRAINT [DF_P_MaterialCompletionRateByWeek_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'Year'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年度週數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'WeekNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MTLCMP_SPNo / TTLSPNo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'MaterialCompletionRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'同Year同WeekNo下P_SewingLineScheduleBySP.MTLExport = <OK> 的SPNo總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'MTLCMP_SPNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'同Year同WeekNo下SPNo的總數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'TTLSPNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialCompletionRateByWeek', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
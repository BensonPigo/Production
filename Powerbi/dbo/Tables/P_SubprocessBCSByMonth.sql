CREATE TABLE [dbo].[P_SubprocessBCSByMonth](
	[Month] [varchar](6) NOT NULL,
	[Factory] [varchar](8) NOT NULL,
	[SubprocessBCS] [decimal](5, 2) NOT NULL,
	[TTLLoadedBundle] [int] NOT NULL,
	[TTLBundle] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_SubprocessBCSByMonth] PRIMARY KEY CLUSTERED 
(
	[Month] ASC,
	[Factory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_SubprocessBCSByMonth] ADD  CONSTRAINT [DF_P_SubprocessBCSByMonth_SubprocessBCS]  DEFAULT ((0)) FOR [SubprocessBCS]
GO

ALTER TABLE [dbo].[P_SubprocessBCSByMonth] ADD  CONSTRAINT [DF_P_SubprocessBCSByMonth_TTLLoadedBundle]  DEFAULT ((0)) FOR [TTLLoadedBundle]
GO

ALTER TABLE [dbo].[P_SubprocessBCSByMonth] ADD  CONSTRAINT [DF_P_SubprocessBCSByMonth_TTLBundle]  DEFAULT ((0)) FOR [TTLBundle]
GO

ALTER TABLE [dbo].[P_SubprocessBCSByMonth] ADD  CONSTRAINT [DF_P_SubprocessBCSByMonth_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TTLLoadedBundle/TTLBundle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessBCSByMonth', @level2type=N'COLUMN',@level2name=N'SubprocessBCS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GroupBy後P_SubprocessWIP.Inline不為null的筆數統計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessBCSByMonth', @level2type=N'COLUMN',@level2name=N'TTLLoadedBundle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P_SubprocessWIP GroupBy後筆數統計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessBCSByMonth', @level2type=N'COLUMN',@level2name=N'TTLBundle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessBCSByMonth', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubprocessBCSByMonth', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
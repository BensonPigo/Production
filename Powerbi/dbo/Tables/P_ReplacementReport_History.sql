	CREATE TABLE [dbo].[P_ReplacementReport_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[FactoryID] [varchar](8) NOT NULL,
		[ID] [varchar](13) NOT NULL,
		[ResponsibilityDept] [varchar](8) NOT NULL,
		[ResponsibilityFty] [varchar](8) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_ReplacementReport_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_ReplacementReport_History] ADD  CONSTRAINT [DF_P_ReplacementReport_History_FactoryID]  DEFAULT ('') FOR [FactoryID]
	ALTER TABLE [dbo].[P_ReplacementReport_History] ADD  CONSTRAINT [DF_P_ReplacementReport_History_ID]  DEFAULT ('') FOR [ID]
	ALTER TABLE [dbo].[P_ReplacementReport_History] ADD  CONSTRAINT [DF_P_ReplacementReport_History_ResponsibilityDept]  DEFAULT ('') FOR [ResponsibilityDept]
	ALTER TABLE [dbo].[P_ReplacementReport_History] ADD  CONSTRAINT [DF_P_ReplacementReport_History_ResponsibilityFty]  DEFAULT ('') FOR [ResponsibilityFty]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ReplacementReport_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ReplacementReport_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ReplacementReport_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
	Go	
	CREATE TABLE [dbo].[P_ProductionKitsTracking_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[Article] [nvarchar](1000) not null,
		[Doc] [nvarchar](506) NOT NULL,
		[FactoryID] [varchar](8) not null,
		[ProductionKitsGroup] [varchar](8) NOT NULL,
		[SPNo] [varchar](13) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_ProductionKitsTracking_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_ProductionKitsTracking_History] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_History_Article]  DEFAULT ('') FOR [Article]
	ALTER TABLE [dbo].[P_ProductionKitsTracking_History] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_History_Doc]  DEFAULT ('') FOR [Doc]
	ALTER TABLE [dbo].[P_ProductionKitsTracking_History] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_History_FactoryID]  DEFAULT ('') FOR [FactoryID]
	ALTER TABLE [dbo].[P_ProductionKitsTracking_History] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_History_ProductionKitsGroup]  DEFAULT ('') FOR [ProductionKitsGroup]
	ALTER TABLE [dbo].[P_ProductionKitsTracking_History] ADD  CONSTRAINT [DF_P_ProductionKitsTracking_History_SPNo]  DEFAULT ('') FOR [SPNo]


	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'樣板' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking_History', @level2type=N'COLUMN',@level2name=N'Article'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking_History', @level2type=N'COLUMN',@level2name=N'SPNo'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
	Go
CREATE TABLE [dbo].[P_LoadingvsCapacity](
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Key] [varchar](6) NOT NULL,
	[Halfkey] [varchar](8) NOT NULL,
	[ArtworkTypeID] [varchar](20) NOT NULL,
	[Capacity(CPU)] [numeric](38, 6) NOT NULL,
	[Loading(CPU)] [numeric](38, 6) NOT NULL,
	[TransferBIDate] [datetime] NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_LoadingvsCapacity] PRIMARY KEY CLUSTERED 
(
	[MDivisionID] ASC,
	[Key] ASC,
	[FactoryID] ASC,
	[Halfkey] ASC,
	[ArtworkTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_LoadingvsCapacity] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_LoadingvsCapacity] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_LoadingvsCapacity] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_Key]  DEFAULT ('') FOR [Key]
GO

ALTER TABLE [dbo].[P_LoadingvsCapacity] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_Halfkey]  DEFAULT ('') FOR [Halfkey]
GO

ALTER TABLE [dbo].[P_LoadingvsCapacity] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_ArtworkTypeID]  DEFAULT ('') FOR [ArtworkTypeID]
GO

ALTER TABLE [dbo].[P_LoadingvsCapacity] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_Capacity(CPU)]  DEFAULT ((0)) FOR [Capacity(CPU)]
GO

ALTER TABLE [dbo].[P_LoadingvsCapacity] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_Loading(CPU)]  DEFAULT ((0)) FOR [Loading(CPU)]
GO

ALTER TABLE [dbo].[P_LoadingvsCapacity] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組織代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠kpi統計群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'Key'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'半月份代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'Halfkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'ArtworkTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Capacity(CPU)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'Capacity(CPU)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loading (CPU)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'Loading(CPU)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LoadingvsCapacity', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
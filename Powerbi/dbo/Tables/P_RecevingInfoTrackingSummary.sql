CREATE TABLE [dbo].[P_RecevingInfoTrackingSummary](
	[TransferDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[WHReceivingLT] [numeric](5, 2) NOT NULL,
	[UnloaderTtlRoll] [int] NOT NULL,
	[UnloaderTtlKG] [numeric](7, 2) NOT NULL,
	 CONSTRAINT [PK_P_RecevingInfoTrackingSummary] PRIMARY KEY CLUSTERED 
	(
		[TransferDate] ASC,
		[FactoryID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	Go
	ALTER TABLE [dbo].[P_RecevingInfoTrackingSummary] ADD  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_FactoryID]  DEFAULT ('') FOR [FactoryID]
	Go
	ALTER TABLE [dbo].[P_RecevingInfoTrackingSummary] ADD  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_WHReceivingLT]  DEFAULT ((0)) FOR [WHReceivingLT]
	Go
	ALTER TABLE [dbo].[P_RecevingInfoTrackingSummary] ADD  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_UnloaderTtlRoll]  DEFAULT ((0)) FOR [UnloaderTtlRoll]
	Go
	ALTER TABLE [dbo].[P_RecevingInfoTrackingSummary] ADD  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_UnloaderTtlKG]  DEFAULT ((0)) FOR [UnloaderTtlKG]
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'TransferDate'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'FactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卸貨總卷數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'UnloaderTtlRoll'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卸貨總重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'UnloaderTtlKG'
	Go
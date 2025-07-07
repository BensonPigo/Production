CREATE TABLE [dbo].[P_RecevingInfoTrackingSummary](
	[TransferDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[WHReceivingLT] [numeric](7, 2) NOT NULL,
	[UnloaderTtlRoll] [int] NOT NULL,
	[UnloaderTtlKG] [numeric](9, 2) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_RecevingInfoTrackingSummary] PRIMARY KEY CLUSTERED 
(
	[TransferDate] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_RecevingInfoTrackingSummary] ADD  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_RecevingInfoTrackingSummary] ADD  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_WHReceivingLT]  DEFAULT ((0)) FOR [WHReceivingLT]
GO

ALTER TABLE [dbo].[P_RecevingInfoTrackingSummary] ADD  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_UnloaderTtlRoll]  DEFAULT ((0)) FOR [UnloaderTtlRoll]
GO

ALTER TABLE [dbo].[P_RecevingInfoTrackingSummary] ADD  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_UnloaderTtlKG]  DEFAULT ((0)) FOR [UnloaderTtlKG]
GO

ALTER TABLE [dbo].[P_RecevingInfoTrackingSummary] ADD  CONSTRAINT [DF_P_RecevingInfoTrackingSummary_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卸貨總卷數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'UnloaderTtlRoll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卸貨總重量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'UnloaderTtlKG'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RecevingInfoTrackingSummary', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
CREATE TABLE [dbo].[P_CartonStatusTrackingList_History](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SP] [varchar](13) NOT NULL,
	[SeqNo] [varchar](2) NOT NULL,
	[PackingListID] [varchar](13) NOT NULL,
	[CtnNo] [varchar](6) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList_History] ADD  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList_History] ADD  DEFAULT ('') FOR [SP]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList_History] ADD  DEFAULT ('') FOR [SeqNo]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList_History] ADD  DEFAULT ('') FOR [PackingListID]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList_History] ADD  DEFAULT ('') FOR [CtnNo]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList_History] ADD  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shipment Seq' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'SeqNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Packing List #' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'PackingListID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Carton#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'CtnNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO



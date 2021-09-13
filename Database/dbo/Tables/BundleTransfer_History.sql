CREATE TABLE [dbo].[BundleTransfer_History](
	[Sid] [bigint] NULL,
	[RFIDReaderId] [varchar](24) NULL,
	[Type] [varchar](1) NULL,
	[SubProcessId] [varchar](15) NULL,
	[TagId] [varchar](24) NULL,
	[BundleNo] [nvarchar](43) NULL,
	[TransferDate] [datetime] NULL,
	[AddDate] [datetime] NULL,
	[LocationID] [varchar](10) NOT NULL,
	[RFIDProcessLocationID] [varchar](15) NOT NULL,
	[PanelNo] [varchar](24) NOT NULL,
	[CutCellID] [varchar](10) NOT NULL,
	[SewingLineID] [varchar](2) NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  CONSTRAINT [DF_BundleTransfer_History_Sid]  DEFAULT ((0)) FOR [Sid]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  CONSTRAINT [DF_BundleTransfer_History_RFIDReaderId]  DEFAULT ('') FOR [RFIDReaderId]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  CONSTRAINT [DF_BundleTransfer_History_Type]  DEFAULT ('') FOR [Type]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  CONSTRAINT [DF_BundleTransfer_History_SubProcessId]  DEFAULT ('') FOR [SubProcessId]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  CONSTRAINT [DF_BundleTransfer_History_TagId]  DEFAULT ('') FOR [TagId]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  CONSTRAINT [DF_BundleTransfer_History_BundleNo]  DEFAULT ('') FOR [BundleNo]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  CONSTRAINT [DF_BundleTransfer_History_LocationID]  DEFAULT ('') FOR [LocationID]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  CONSTRAINT [DF_BundleTransfer_History_RFIDProcessLocationID]  DEFAULT ('') FOR [RFIDProcessLocationID]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  CONSTRAINT [DF_BundleTransfer_History_PanelNo]  DEFAULT ('') FOR [PanelNo]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  CONSTRAINT [DF_BundleTransfer_History_CutCellID]  DEFAULT ('') FOR [CutCellID]
GO

ALTER TABLE [dbo].[BundleTransfer_History] ADD  DEFAULT ('') FOR [SewingLineID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleTransfer_History', @level2type=N'COLUMN',@level2name=N'Sid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RFID Reader Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleTransfer_History', @level2type=N'COLUMN',@level2name=N'RFIDReaderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleTransfer_History', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sub-process Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleTransfer_History', @level2type=N'COLUMN',@level2name=N'SubProcessId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RFID tag Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleTransfer_History', @level2type=N'COLUMN',@level2name=N'TagId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleTransfer_History', @level2type=N'COLUMN',@level2name=N'BundleNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Transfer Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleTransfer_History', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleTransfer_History', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle transfer record History' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleTransfer_History'
GO



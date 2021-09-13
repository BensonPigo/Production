
CREATE TABLE [dbo].[BundleInOut_History](
	[BundleNo] [varchar](10) NOT NULL,
	[SubProcessId] [varchar](15) NOT NULL,
	[InComing] [datetime] NULL,
	[OutGoing] [datetime] NULL,
	[AddDate] [datetime] NOT NULL,
	[EditDate] [datetime] NULL,
	[SewingLineID] [varchar](2) NOT NULL,
	[LocationID] [varchar](10) NOT NULL,
	[RFIDProcessLocationID] [varchar](15) NOT NULL,
	[PanelNo] [varchar](24) NOT NULL,
	[CutCellID] [varchar](10) NOT NULL,
 CONSTRAINT [PK_BundleInOut_History] PRIMARY KEY CLUSTERED 
(
	[BundleNo] ASC,
	[SubProcessId] ASC,
	[RFIDProcessLocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BundleInOut_History] ADD  CONSTRAINT [DF_BundleInOut_History_BundleNo]  DEFAULT ('') FOR [BundleNo]
GO

ALTER TABLE [dbo].[BundleInOut_History] ADD  CONSTRAINT [DF_BundleInOut_History_SubProcessId]  DEFAULT ('') FOR [SubProcessId]
GO

ALTER TABLE [dbo].[BundleInOut_History] ADD  CONSTRAINT [DF_BundleInOut_History_SewingLineID]  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[BundleInOut_History] ADD  CONSTRAINT [DF_BundleInOut_History_LocationID]  DEFAULT ('') FOR [LocationID]
GO

ALTER TABLE [dbo].[BundleInOut_History] ADD  CONSTRAINT [DF_BundleInOut_History_RFIDProcessLocationID]  DEFAULT ('') FOR [RFIDProcessLocationID]
GO

ALTER TABLE [dbo].[BundleInOut_History] ADD  CONSTRAINT [DF_BundleInOut_History_PanelNo]  DEFAULT ('') FOR [PanelNo]
GO

ALTER TABLE [dbo].[BundleInOut_History] ADD  CONSTRAINT [DF_BundleInOut_History_CutCellID]  DEFAULT ('') FOR [CutCellID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleInOut_History', @level2type=N'COLUMN',@level2name=N'BundleNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sub-process Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleInOut_History', @level2type=N'COLUMN',@level2name=N'SubProcessId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'In' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleInOut_History', @level2type=N'COLUMN',@level2name=N'InComing'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Out' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleInOut_History', @level2type=N'COLUMN',@level2name=N'OutGoing'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleInOut_History', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleInOut_History', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bundle-Subprocess In Out Date History' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BundleInOut_History'
GO

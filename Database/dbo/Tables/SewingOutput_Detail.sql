CREATE TABLE [dbo].[SewingOutput_Detail](
	[ID] [varchar](13) NOT NULL,
	[OrderId] [varchar](13) NOT NULL,
	[ComboType] [varchar](1) NULL,
	[Article] [varchar](8) NULL,
	[Color] [varchar](6) NULL,
	[TMS] [int] NULL,
	[HourlyStandardOutput] [int] NULL,
	[WorkHour] [numeric](6, 3) NOT NULL,
	[UKey] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[QAQty] [int] NULL,
	[DefectQty] [int] NULL,
	[InlineQty] [int] NULL,
	[OldDetailKey] [varchar](13) NULL,
	[AutoCreate] [bit] NULL,
	[SewingReasonID] [varchar](5) NOT NULL,
	[Remark] [nvarchar](1000) NULL,
	[ImportFromDQS] [bit] NOT NULL,
	[AutoSplit] [bit] NULL,
	[Cumulate] [int] NOT NULL,
	[CumulateSimilar] [int] NOT NULL,
	[DQSOutput] [int] NOT NULL,
	[InlineCategoryCumulate] [int] NOT NULL,
 CONSTRAINT [PK_SewingOutput_Detail] PRIMARY KEY CLUSTERED 
(
	[UKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_OrderId]  DEFAULT ('') FOR [OrderId]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_ComboType]  DEFAULT ('') FOR [ComboType]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_TMS]  DEFAULT ((0)) FOR [TMS]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_HourlyStandardOutput]  DEFAULT ((0)) FOR [HourlyStandardOutput]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_WorkHour]  DEFAULT ((0)) FOR [WorkHour]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_QAQty]  DEFAULT ((0)) FOR [QAQty]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_DefectQty]  DEFAULT ((0)) FOR [DefectQty]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_InlineQty]  DEFAULT ((0)) FOR [InlineQty]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_OldDetailKey]  DEFAULT ('') FOR [OldDetailKey]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  DEFAULT ('') FOR [SewingReasonID]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  DEFAULT ((0)) FOR [ImportFromDQS]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  DEFAULT ((0)) FOR [AutoSplit]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_Cumulate]  DEFAULT ((0)) FOR [Cumulate]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_CumulateSimilar]  DEFAULT ((0)) FOR [CumulateSimilar]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  DEFAULT ((0)) FOR [DQSOutput]
GO

ALTER TABLE [dbo].[SewingOutput_Detail] ADD  CONSTRAINT [DF_SewingOutput_Detail_InlineCategoryCumulate]  DEFAULT ((0)) FOR [InlineCategoryCumulate]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingOutput Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'OrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組合型態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Costing TMS (sec)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'TMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每小時標準產出' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'HourlyStandardOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作時數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'WorkHour'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Detail Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'UKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'QAQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'DefectQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上線數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'InlineQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'OldDetailKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'從DQS匯入' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'ImportFromDQS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'累積天數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'Cumulate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相似款式累積天數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'CumulateSimilar'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產完且通過檢驗(‘Pass’ or ‘Fixed’)的數量(工廠無法修改)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'DQSOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InlineCategory 累計天數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail', @level2type=N'COLUMN',@level2name=N'InlineCategoryCumulate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Dailiy output(車縫日報明細檔)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingOutput_Detail'
GO



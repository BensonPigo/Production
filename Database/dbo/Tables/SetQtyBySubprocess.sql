CREATE TABLE [dbo].[SetQtyBySubprocess](
	[OrderID] [varchar](13) NOT NULL,
	[Article] [varchar](8) NOT NULL,
	[SizeCode] [varchar](8) NOT NULL,
	[PatternPanel] [varchar](2) NOT NULL,
	[InQtyBySet] [int] NOT NULL,
	[OutQtyBySet] [int] NOT NULL,
	[FinishedQtyBySet] [int] NOT NULL,
	[SubprocessID] [varchar](15) NOT NULL,
	[TransferTime] [datetime] NOT NULL,
	[AddDate] [datetime] NULL,
 CONSTRAINT [PK_SetQtyBySubprocess] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[Article] ASC,
	[SizeCode] ASC,
	[PatternPanel] ASC,
	[SubprocessID] ASC,
	[TransferTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SetQtyBySubprocess] ADD  CONSTRAINT [DF_SetQtyBySubprocess_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[SetQtyBySubprocess] ADD  CONSTRAINT [DF_SetQtyBySubprocess_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[SetQtyBySubprocess] ADD  CONSTRAINT [DF_SetQtyBySubprocess_SizeCode]  DEFAULT ('') FOR [SizeCode]
GO

ALTER TABLE [dbo].[SetQtyBySubprocess] ADD  CONSTRAINT [DF_SetQtyBySubprocess_PatternPanel]  DEFAULT ('') FOR [PatternPanel]
GO

ALTER TABLE [dbo].[SetQtyBySubprocess] ADD  CONSTRAINT [DF_SetQtyBySubprocess_SubprocessID]  DEFAULT ('') FOR [SubprocessID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SetQtyBySubprocess', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SetQtyBySubprocess', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SetQtyBySubprocess', @level2type=N'COLUMN',@level2name=N'SizeCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部位別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SetQtyBySubprocess', @level2type=N'COLUMN',@level2name=N'PatternPanel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SetQtyBySubprocess', @level2type=N'COLUMN',@level2name=N'SubprocessID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'轉入時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SetQtyBySubprocess', @level2type=N'COLUMN',@level2name=N'TransferTime'
GO
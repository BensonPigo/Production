
CREATE TABLE [dbo].[Operation_His](
	[Ukey] [bigint] NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[TypeName] [nvarchar](40) NOT NULL,
	[OperationID] [varchar](20) NOT NULL,
	[OldValue] [nvarchar](200) NOT NULL,
	[NewValue] [nvarchar](200) NOT NULL,
	[Remark] [nvarchar](max) NULL,
	[EditDate] [datetime] NOT NULL,
	[EditName] [varchar](10) NOT NULL,
	CONSTRAINT [PK_Operation_His] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Operation_His] ADD  CONSTRAINT [DF_Operation_His_Type]  DEFAULT ('') FOR [Type]
GO

ALTER TABLE [dbo].[Operation_His] ADD  CONSTRAINT [DF_Operation_His_TypeName]  DEFAULT ('') FOR [TypeName]
GO

ALTER TABLE [dbo].[Operation_His] ADD  CONSTRAINT [DF_Operation_His_OperationID]  DEFAULT ('') FOR [OperationID]
GO

ALTER TABLE [dbo].[Operation_His] ADD  CONSTRAINT [DF_Operation_His_OldValue]  DEFAULT ('') FOR [OldValue]
GO

ALTER TABLE [dbo].[Operation_His] ADD  CONSTRAINT [DF_Operation_His_NewValue]  DEFAULT ('') FOR [NewValue]
GO

ALTER TABLE [dbo].[Operation_His] ADD  CONSTRAINT [DF_Operation_His_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[Operation_His] ADD  CONSTRAINT [DF_Operation_His_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UKEY' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類代號完整名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'TypeName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'OperationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'舊值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'OldValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'NewValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備誰' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation 歷史檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His'
GO
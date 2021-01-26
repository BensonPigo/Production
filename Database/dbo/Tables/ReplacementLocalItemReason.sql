

CREATE TABLE [dbo].[ReplacementLocalItemReason](
	[Type] [varchar](2) NOT NULL CONSTRAINT [DF_ReplacementLocalItemReason_Type]  DEFAULT (''),
	[ID] [varchar](5) NOT NULL CONSTRAINT [DF_ReplacementLocalItemReason_ID]  DEFAULT (''),
	[Description] [nvarchar](60) NOT NULL CONSTRAINT [DF_ReplacementLocalItemReason_Description]  DEFAULT (''),
	[Junk] [bit] NULL CONSTRAINT [DF_ReplacementLocalItemReason_Junk]  DEFAULT ((0)),
	[AddName] [varchar](10) NULL CONSTRAINT [DF_ReplacementLocalItemReason_AddName]  DEFAULT (''),
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL CONSTRAINT [DF_ReplacementLocalItemReason_EditName]  DEFAULT (''),
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_ReplacementLocalItemReason] PRIMARY KEY CLUSTERED 
(
	[Type] ASC,
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItemReason', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItemReason', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取消' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItemReason', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lack or Replacement' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItemReason', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItemReason', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItemReason', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItemReason', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItemReason', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItemReason'
GO

CREATE TABLE [dbo].[StatementReport](
	[FileType] [varchar](30) NOT NULL,
	[CreateDate] [date] NOT NULL,
	[FileName] [varchar](200) NOT NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_StatementReport] PRIMARY KEY CLUSTERED 
(
	[FileType] ASC,
	[CreateDate] ASC,
	[FileName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StatementReport] ADD  CONSTRAINT [DF_StatementReport_FileType]  DEFAULT ('') FOR [FileType]
GO

ALTER TABLE [dbo].[StatementReport] ADD  CONSTRAINT [DF_StatementReport_FileName]  DEFAULT ('') FOR [FileName]
GO

ALTER TABLE [dbo].[StatementReport] ADD  CONSTRAINT [DF_StatementReport_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'報表類型 ex. WarehouseR28, CuttingR14' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StatementReport', @level2type=N'COLUMN',@level2name=N'FileType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'指定的建立日期	' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StatementReport', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上傳人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StatementReport', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上傳時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StatementReport', @level2type=N'COLUMN',@level2name=N'EditDate'
GO


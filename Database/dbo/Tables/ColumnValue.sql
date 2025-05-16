CREATE TABLE [dbo].[ColumnValue](
	[TableName] [varchar](50) NOT NULL,
	[Column] [varchar](25) NOT NULL,
	[Value] [varchar](13) NOT NULL,
 CONSTRAINT [PK_ColumnValue] PRIMARY KEY CLUSTERED 
(
	[TableName] ASC,
	[Column] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ColumnValue] ADD  CONSTRAINT [DF_ColumnValue_TableName]  DEFAULT ('') FOR [TableName]
GO

ALTER TABLE [dbo].[ColumnValue] ADD  CONSTRAINT [DF_ColumnValue_Column]  DEFAULT ('') FOR [Column]
GO

ALTER TABLE [dbo].[ColumnValue] ADD  CONSTRAINT [DF_ColumnValue_Value]  DEFAULT ('') FOR [Value]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料表名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnValue', @level2type=N'COLUMN',@level2name=N'TableName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'欄位名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnValue', @level2type=N'COLUMN',@level2name=N'Column'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnValue', @level2type=N'COLUMN',@level2name=N'Value'
GO
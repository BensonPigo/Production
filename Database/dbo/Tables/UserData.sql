CREATE TABLE [dbo].[UserData](
	[EmpID] [varchar](10) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[FunctionCode] [varchar](30) NOT NULL,
	[SpecColumn] [varchar](30) NOT NULL,
	[SpecValue] [varchar](30) NOT NULL,
	[Editdate] [datetime] NULL,
 CONSTRAINT [PK_UserData] PRIMARY KEY CLUSTERED 
([EmpID], [SpecColumn], [FactoryID], [FunctionCode])WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserData] ADD  DEFAULT ('') FOR [SpecColumn]
GO

ALTER TABLE [dbo].[UserData] ADD  DEFAULT ('') FOR [SpecValue]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用者代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserData', @level2type=N'COLUMN',@level2name=N'EmpID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserData', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserData', @level2type=N'COLUMN',@level2name=N'FunctionCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'欄位名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserData', @level2type=N'COLUMN',@level2name=N'SpecColumn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'欄位值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserData', @level2type=N'COLUMN',@level2name=N'SpecValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近次勾選時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserData', @level2type=N'COLUMN',@level2name=N'Editdate'
GO

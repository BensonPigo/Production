CREATE TABLE [dbo].[InspectionType](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Category] [varchar](8) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Maximun] [numeric](8, 3) NOT NULL,
	[Minimum] [numeric](8, 3) NOT NULL,
	[MaxEqual] [bit] NOT NULL,
	[MinEqual] [bit] NOT NULL,
	[UserCustomize] [bit] NOT NULL,
	[Comment] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_InspectionType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[InspectionType] ADD  CONSTRAINT [DF_InspectionType_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[InspectionType] ADD  CONSTRAINT [DF_InspectionType_Name]  DEFAULT ('') FOR [Name]
GO

ALTER TABLE [dbo].[InspectionType] ADD  CONSTRAINT [DF_InspectionType_UserCustomize]  DEFAULT ((0)) FOR [UserCustomize]
GO

ALTER TABLE [dbo].[InspectionType] ADD  CONSTRAINT [DF_InspectionType_Comment]  DEFAULT ('') FOR [Comment]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InspectionType', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bulk/Sample' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InspectionType', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InspectionType', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標準上限，預設10000.000' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InspectionType', @level2type=N'COLUMN',@level2name=N'Maximun'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標準下限，預設-10000.000' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InspectionType', @level2type=N'COLUMN',@level2name=N'Minimum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否由使用者自行KeyIn Pass/Fail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InspectionType', @level2type=N'COLUMN',@level2name=N'UserCustomize'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶的標準值參照字串' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InspectionType', @level2type=N'COLUMN',@level2name=N'Comment'
GO